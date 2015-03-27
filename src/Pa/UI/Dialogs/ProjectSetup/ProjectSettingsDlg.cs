// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Progress;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
    /// ----------------------------------------------------------------------------------------
    public partial class ProjectSettingsDlg : OKCancelDlgBase
    {
        private readonly bool _isProjectNew;
        private readonly List<PaDataSource> _dataSources = new List<PaDataSource>();
        private readonly IEnumerable<string> _originallyMappedFields = new List<string>(0);

        public PaProject Project { get; private set; }
        public IEnumerable<string> NewlyMappedFields { get; private set; }

        /// ------------------------------------------------------------------------------------
        public ProjectSettingsDlg()
        {
            InitializeComponent();

            NewlyMappedFields = new List<string>(0);

            pnlGridHdg.Font = FontHelper.UIFont;
            lblLanguageName.Font = FontHelper.UIFont;
            lblLanguageCode.Font = FontHelper.UIFont;
            lblResearcher.Font = FontHelper.UIFont;
            lblSpeaker.Font = FontHelper.UIFont;
            lblTranscriber.Font = FontHelper.UIFont;
            lblProjName.Font = FontHelper.UIFont;
            lblComments.Font = FontHelper.UIFont;
            txtLanguageName.Font = FontHelper.UIFont;
            txtLanguageCode.Font = FontHelper.UIFont;
            lnkEthnologue.Font = FontHelper.UIFont;
            txtResearcher.Font = FontHelper.UIFont;
            txtSpeaker.Font = FontHelper.UIFont;
            txtTranscriber.Font = FontHelper.UIFont;
            txtProjName.Font = FontHelper.UIFont;
            txtComments.Font = FontHelper.UIFont;
            _chkMakeFolder.Font = FontHelper.UIFont;
            _labelDistinctiveFeaturesSet.Font = FontHelper.UIFont;
            _comboDistinctiveFeaturesSet.Font = FontHelper.UIFont;

            _chkMakeFolder.Parent.Controls.Remove(_chkMakeFolder);
            tblLayoutButtons.Controls.Add(_chkMakeFolder, 0, 0);

            foreach (var featureSetName in BFeatureCache.GetAvailableFeatureSetNames().OrderBy(n => n))
            {
                if (featureSetName != BFeatureCache.DefaultFeatureSetName)
                    _comboDistinctiveFeaturesSet.Items.Add(featureSetName);
                else
                {
                    _comboDistinctiveFeaturesSet.Items.Insert(0, LocalizationManager.GetString(
                        "DialogBoxes.ProjectSettingsDlg.DefaultFeatureSetName", "(default)"));
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        public ProjectSettingsDlg(PaProject project)
            : this()
        {
            Application.DoEvents();

            _isProjectNew = (project == null);
            _chkMakeFolder.Visible = (_isProjectNew && !Settings.Default.AutoCreateProjectFilesAndFolderOnProjectCreation);
            _chkMakeFolder.Checked = (Settings.Default.CreateProjectFolderForNewProject || !_chkMakeFolder.Visible);

            BuildGrid();
            pnlGridHdg.ControlReceivingFocusOnMnemonic = m_grid;
            pnlGridHdg.BorderStyle = BorderStyle.None;

            if (project == null)
                Project = new PaProject(true);
            else
            {
                Project = project;
                _originallyMappedFields = project.GetMappedFields().Select(f => f.Name).ToList();
                _dataSources = project.DataSources.Select(ds => ds.Copy()).ToList();
                txtProjName.Text = project.Name;
                txtLanguageName.Text = project.LanguageName;
                txtLanguageCode.Text = project.LanguageCode;
                txtResearcher.Text = project.Researcher;
                txtTranscriber.Text = project.Transcriber;
                txtSpeaker.Text = project.SpeakerName;
                txtComments.Text = project.Comments;
                LoadGrid(-1);
            }

            WaitCursor.Show();

            if (Project.DistinctiveFeatureSet == BFeatureCache.DefaultFeatureSetName)
                _comboDistinctiveFeaturesSet.SelectedIndex = 0;
            else
                _comboDistinctiveFeaturesSet.SelectedItem = Project.DistinctiveFeatureSet;

            if (_dataSources.Any(ds => ds.Type == DataSourceType.FW))
                FwDBUtils.StartSQLServer(true);

            mnuAddFw6DataSource.Enabled = FwDBUtils.IsSQLServerInstalled(false);
            mnuAddFw7DataSource.Enabled = FwDBUtils.IsFw7Installed;

            DialogResult = DialogResult.Cancel;
            m_dirty = _isProjectNew;
            m_grid.IsDirty = false;
            WaitCursor.Hide();

            UpdateButtonStates();
        }

        /// ------------------------------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// ------------------------------------------------------------------------------------
        protected override void OnStringsLocalized()
        {
            if (!_isProjectNew)
                base.OnStringsLocalized();
            else
            {
                Text = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.WindowTitle.WhenProjectIsNew",
                    "New Project Settings", "Caption for project settings dialog when project is new.");
            }
        }

        #region Grid setup
        /// ------------------------------------------------------------------------------------
        private void BuildGrid()
        {
            m_grid.Name = Name + "Grid";
            m_grid.AutoGenerateColumns = false;
            m_grid.MultiSelect = true;
            m_grid.Font = FontHelper.UIFont;
            m_grid.CurrentRowChanged += HandleCurrentRowChanged;
            App.SetGridSelectionColors(m_grid, false);

            DataGridViewColumn col = SilGrid.CreateCheckBoxColumn("skip");
            m_grid.Columns.Add(col);
            col.HeaderText = LocalizationManager.GetString(
                "DialogBoxes.ProjectSettingsDlg.DataSourceGrid.ColumnHeadings.Load", "Load", null, col);

            col = SilGrid.CreateTextBoxColumn("sourcefiles");
            col.ReadOnly = true;
            col.Width = 250;
            m_grid.Columns.Add(col);
            col.HeaderText = LocalizationManager.GetString(
                "DialogBoxes.ProjectSettingsDlg.DataSourceGrid.ColumnHeadings.Source", "Source", null, col);

            col = SilGrid.CreateTextBoxColumn("type");
            col.ReadOnly = true;
            col.Width = 75;
            m_grid.Columns.Add(col);
            col.HeaderText = LocalizationManager.GetString(
                "DialogBoxes.ProjectSettingsDlg.DataSourceGrid.ColumnHeadings.Type", "Type", null, col);

            col = SilGrid.CreateTextBoxColumn("Phonetic_Source");
            col.ReadOnly = true;
            col.Width = 100;
            m_grid.Columns.Add(col);
            col.HeaderText = LocalizationManager.GetString(
                "DialogBoxes.ProjectSettingsDlg.DataSourceGrid.ColumnHeadings.Phonetic_Source", "Phonetic Source", null, col);

            col = SilGrid.CreateSilButtonColumn("xslt");
            col.ReadOnly = true;
            col.Width = 170;
            ((SilButtonColumn)col).ButtonWidth = 20;
            ((SilButtonColumn)col).DrawTextWithEllipsisPath = true;
            ((SilButtonColumn)col).ButtonClicked += HandleSpecifyXSLTClick;
            ((SilButtonColumn)col).ButtonText = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.XsltColButtonText",
                "...", "Text on the button in the XSLT column in the project settings dialog");

            ((SilButtonColumn)col).ButtonToolTip = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.XsltColButtonToolTip",
                "Specify XSLT", "Tooltip for the button in the XSLTe column in the project settings dialog");

            m_grid.Columns.Add(col);
            col.HeaderText = LocalizationManager.GetString(
                "DialogBoxes.ProjectSettingsDlg.DataSourceGrid.ColumnHeadings.XSLT", "XSLT", null, col);

            m_grid.AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.ColumnHeader);

            if (Settings.Default.ProjectSettingsDlgGrid != null)
                Settings.Default.ProjectSettingsDlgGrid.InitializeGrid(m_grid);

            m_grid.Columns["skip"].Visible = Settings.Default.ShowLoadColumnInProjectSettingsDlg;

            // When xslt transforms are supported when reading data, then this should become visible.
            m_grid.Columns["xslt"].Visible = false;

            m_grid.CurrentCellChanged += HandleGridsCurrentCellChanged;
            m_grid.CellClick += delegate { UpdateButtonStates(); };
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the grid with the project's data source specifications.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void LoadGrid(int preferredRow)
        {
            if (preferredRow == -1)
                preferredRow = (m_grid.CurrentRow != null ? m_grid.CurrentRow.Index : 0);

            // Clear the grid and start over.
            m_grid.Rows.Clear();

            // Check if there are any data sources specified for this project.
            if (_dataSources.Count == 0)
                return;

            m_grid.RowCount = _dataSources.Count;

            // If the current row used to be the last row and that last row no
            // longer exists, then make the new current row the new last row.
            if (preferredRow == m_grid.RowCount)
                preferredRow--;

            // Try to restore the current row to what it was before removing all the rows.
            if (m_grid.RowCount > 0 && preferredRow >= 0 && preferredRow < m_grid.RowCount)
                m_grid.MakeFirstVisibleCellCurrentInRow(preferredRow);
        }

        #endregion

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Update button enabled states.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void UpdateButtonStates()
        {
            bool enableRemoveButton = ((m_grid.SelectedRows.Count > 0) ||
                (m_grid.CurrentRow != null && m_grid.CurrentRow.Index < _dataSources.Count));

            btnRemove.Enabled = enableRemoveButton;

            bool enablePropertiesButton = false;

            if (m_grid.SelectedRows.Count <= 1 && m_grid.CurrentRow != null &&
                m_grid.CurrentRow.Index < _dataSources.Count)
            {
                var dataSource = _dataSources[m_grid.CurrentRow.Index];

                enablePropertiesButton = (
                    dataSource.Type == DataSourceType.SFM ||
                    dataSource.Type == DataSourceType.Toolbox ||
                    dataSource.Type == DataSourceType.FW7 ||
                    (dataSource.Type == DataSourceType.FW && dataSource.FwSourceDirectFromDB));
            }

            btnProperties.Enabled = enablePropertiesButton;
        }

        #region Saving Settings and Verifying/Saving changes
        /// ------------------------------------------------------------------------------------
        protected override void SaveSettings()
        {
            if (_chkMakeFolder.Visible)
                Settings.Default.CreateProjectFolderForNewProject = _chkMakeFolder.Checked;

            Settings.Default.ProjectSettingsDlgGrid = GridSettings.Create(m_grid);
            base.SaveSettings();
        }

        /// ------------------------------------------------------------------------------------
        protected override bool IsDirty
        {
            get
            {
                if (m_grid.IsDirty || _isProjectNew || m_dirty)
                    return true;

                if (_comboDistinctiveFeaturesSet.SelectedIndex == 0)
                {
                    if (Project.DistinctiveFeatureSet != BFeatureCache.DefaultFeatureSetName)
                        return true;
                }
                else if (_comboDistinctiveFeaturesSet.SelectedItem as string != Project.DistinctiveFeatureSet)
                    return true;

                return (txtProjName.Text.Trim() != Project.Name ||
                    txtLanguageName.Text.Trim() != Project.LanguageName ||
                    txtLanguageCode.Text.Trim() != Project.LanguageCode ||
                    txtResearcher.Text.Trim() != Project.Researcher ||
                    txtTranscriber.Text.Trim() != Project.Transcriber ||
                    txtSpeaker.Text.Trim() != Project.SpeakerName ||
                    txtComments.Text.Trim() != Project.Comments);
            }
        }

        /// ------------------------------------------------------------------------------------
        protected override bool Verify()
        {
            string msg = null;
            int offendingIndex = -1;
            Control offendingCtrl = null;

            if (txtProjName.Text.Trim() == string.Empty)
            {
                // A project name was not specified.
                msg = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.MissingProjectNameMsg",
                    "You must specify a project name.");

                offendingCtrl = txtProjName;
            }
            else if (txtLanguageName.Text.Trim() == string.Empty)
            {
                // A language name was not specified.
                msg = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.MissingLanguageNameMsg",
                    "You must specify a language name.");

                offendingCtrl = txtLanguageName;
            }
            else
            {
                for (int i = 0; i < _dataSources.Count; i++)
                {
                    if (_dataSources[i].Type == DataSourceType.PAXML)
                        continue;

                    if (_dataSources[i].Type == DataSourceType.XML &&
                        string.IsNullOrEmpty(_dataSources[i].XSLTFile))
                    {
                        // No XSLT file was specified
                        offendingIndex = i;

                        msg = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.MissingXSLTMsg",
                            "You must specify an XSLT file for '{0}'");

                        msg = string.Format(msg, Utils.PrepFilePathForMsgBox(_dataSources[i].SourceFile));
                        break;
                    }
                }
            }

            if (msg != null)
            {
                Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Give the appropriate control focus.
                if (offendingCtrl != null)
                    offendingCtrl.Focus();
                else
                {
                    // Clear all selected rows.
                    for (int i = 0; i < m_grid.Rows.Count; i++)
                        m_grid.Rows[i].Selected = false;

                    // Select the offending row and give the grid focus.
                    m_grid.MakeFirstVisibleCellCurrentInRow(offendingIndex);
                    m_grid.Rows[offendingIndex].Selected = true;
                    m_grid.Focus();
                }

                return false;
            }

            return true;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Save the changes in response to closing the dialog.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override bool SaveChanges()
        {
            // Get a project file name if the project is new.
            if (Project.FileName == null)
            {
                Project.FileName = GetProjectFileName();
                if (Project.FileName == null)
                    return false;
            }

            // Make sure there's a phonetic mapping for each data source.
            for (int i = 0; i < _dataSources.Count; i++)
            {
                if (!FieldMapping.IsPhoneticMapped(_dataSources[i].FieldMappings, true))
                {
                    m_grid.CurrentCell = m_grid[1, i];
                    m_grid.Focus();
                    return false;
                }

                if ((_dataSources[i].Type == DataSourceType.SFM || _dataSources[i].Type == DataSourceType.Toolbox) &&
                    string.IsNullOrEmpty(_dataSources[i].SfmRecordMarker))
                {
                    var msg = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.NoSfmRecordMarkerSpecifiedErrorMsg",
                        "A record marker must be specified for '{0}'.");

                    msg = string.Format(msg, Path.GetFileName(_dataSources[i].SourceFile));
                    Utils.MsgBox(msg);
                    m_grid.CurrentCell = m_grid[1, i];
                    m_grid.Focus();
                    return false;
                }
            }

            WaitCursor.Show();

            Project.Name = txtProjName.Text.Trim();
            Project.LanguageName = txtLanguageName.Text.Trim();
            Project.LanguageCode = txtLanguageCode.Text.Trim();
            Project.Researcher = txtResearcher.Text.Trim();
            Project.Transcriber = txtTranscriber.Text.Trim();
            Project.SpeakerName = txtSpeaker.Text.Trim();
            Project.Comments = txtComments.Text.Trim();
            Project.DataSources = _dataSources;
            Project.SynchronizeProjectFieldMappingsWithDataSourceFieldMappings();

            Project.DistinctiveFeatureSet = (_comboDistinctiveFeaturesSet.SelectedIndex == 0 ?
                BFeatureCache.DefaultFeatureSetName : _comboDistinctiveFeaturesSet.SelectedItem as string);

            Project.LoadFeatureOverrides();

            if (_isProjectNew)
            {
                Project.Save();

                if (Project.DataSources.Any(ds => ds.Type == DataSourceType.FW7))
                    InitializeFontsToFw7Values();

                int i = 0;
                foreach (var field in Settings.Default.DefaultVisibleFieldsForNewProject.Cast<string>()
                    .Select(fname => Project.GetMappedFields()
                        .SingleOrDefault(f => f.Name == fname))
                        .Where(field => field != null))
                {
                    field.DisplayIndexInGrid = i;
                    field.DisplayIndexInRecView = i++;
                    field.VisibleInGrid = true;
                    field.VisibleInRecView = true;
                }
            }

            Project.Save();

            NewlyMappedFields = (from mapping in Project.DataSources.SelectMany(ds => ds.FieldMappings)
                                 where !_originallyMappedFields.Contains(mapping.Field.Name)
                                 select mapping.Field.Name).ToList();

            WaitCursor.Hide();
            return true;
        }

        /// ------------------------------------------------------------------------------------
        private void InitializeFontsToFw7Values()
        {
            var ds = Project.DataSources.First(d => d.Type == DataSourceType.FW7);
            foreach (var mapping in ds.FieldMappings.Where(m => m.FwWsId != null))
            {
                var ws = ds.FwDataSourceInfo.GetWritingSystems().First(w => w.Id == mapping.FwWsId);
                var fontString = string.Format(Settings.Default.DefaultFw7InferredFontSizeAndStyle, ws.DefaultFontName);
                mapping.Field.Font = FontHelper.MakeFont(fontString);
            }
        }

        /// ------------------------------------------------------------------------------------
        private string GetProjectFileName()
        {
            if (!Settings.Default.AutoCreateProjectFilesAndFolderOnProjectCreation)
                return GetProjectFileNameFromUser();

            string prjFileName;
            string prjFolder;
            GetProjectFolderAndCreateIfNecessary(out prjFileName, out prjFolder);
            prjFileName = Path.ChangeExtension(prjFileName, "pap");
            return Path.Combine(prjFolder, prjFileName);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Opens the save file dialog, asking the user what file name to give his new project.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private string GetProjectFileNameFromUser()
        {
            string prjFileName;
            string prjFolder;
            var prjFolderCreated = GetProjectFolderAndCreateIfNecessary(out prjFileName, out prjFolder);

            using (var dlg = new SaveFileDialog())
            {
                dlg.OverwritePrompt = true;
                dlg.CheckFileExists = false;
                dlg.CheckPathExists = true;
                dlg.AddExtension = true;
                dlg.ShowHelp = false;
                dlg.DefaultExt = "pap";
                dlg.RestoreDirectory = false;
                dlg.FileName = Path.ChangeExtension(prjFileName, "pap");
                dlg.InitialDirectory = prjFolder;
                dlg.FilterIndex = 0;

                dlg.Filter = string.Format(App.kstidFileTypePAProject,
                    Application.ProductName) + "|" + App.kstidFileTypeAllFiles;

                dlg.Title = LocalizationManager.GetString(
                    "DialogBoxes.ProjectSettingsDlg.ProjectSaveFileDialogBoxCaption", "Save Phonology Assistant Project File",
                    "Caption for the save project dialog. The parameter is for the application name.");

                var result = dlg.ShowDialog(this);

                if (result != DialogResult.Cancel && !string.IsNullOrEmpty(dlg.FileName))
                {
                    if (prjFolderCreated && Path.GetDirectoryName(dlg.FileName) != prjFolder)
                        Directory.Delete(prjFolder);

                    Settings.Default.LastFolderForSavedProject = Path.GetDirectoryName(dlg.FileName);
                    return dlg.FileName;
                }

                if (prjFolderCreated)
                    Directory.Delete(prjFolder);
            }

            return null;
        }

        /// ------------------------------------------------------------------------------------
        private bool GetProjectFolderAndCreateIfNecessary(out string prjFileName, out string prjFolder)
        {
            prjFileName = (txtProjName.Text.Trim() == string.Empty ? Project.Name : txtProjName.Text.Trim());
            prjFileName = PaProject.GetCleanNameForFileName(prjFileName);

            if (!_chkMakeFolder.Checked)
            {
                prjFolder = (Settings.Default.LastFolderForSavedProject ?? string.Empty);
                if (!Directory.Exists(prjFolder))
                    prjFolder = App.ProjectFolder;

                return false;
            }

            var preferredProjectFileName = prjFileName;
            prjFolder = Path.Combine(App.ProjectFolder, prjFileName);

            if (Directory.Exists(prjFolder))
            {
                if (!Directory.GetFiles(prjFolder, "*.pap").Any())
                    return false;

                int i = 2;
                while (Directory.Exists(prjFolder))
                {
                    prjFileName = string.Format("{0} ({1})", preferredProjectFileName, i++);
                    prjFolder = Path.Combine(App.ProjectFolder, prjFileName);
                }
            }

            Directory.CreateDirectory(prjFolder);
            return true;
        }

        #endregion

        #region Button click handlers
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Show the open file dialog so the user may specify a non FieldWorks data source.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleAddOtherDataSourceClick(object sender, EventArgs e)
        {
            int filterIndex = Settings.Default.OFD_LastFileTypeChosen_DataSource;

            var fileTypes = new StringBuilder();
            fileTypes.Append(App.kstidFileTypeToolboxDB);
            fileTypes.Append("|");
            fileTypes.Append(App.kstidFileTypeToolboxITX);
            fileTypes.Append("|");
            fileTypes.Append(string.Format(App.kstidFileTypePAXML, Application.ProductName));
            fileTypes.Append("|");
            fileTypes.Append(App.kstidFiletypeSASoundWave);
            fileTypes.Append("|");
            fileTypes.Append(App.kstidFiletypeSASoundMP3);
            fileTypes.Append("|");
            fileTypes.Append(App.kstidFiletypeSASoundWMA);
            fileTypes.Append("|");
            fileTypes.Append(App.kstidFileTypeAllFiles);

            var caption = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.DataSourceOpenFileDialogCaption",
                "Choose Data Source File(s)", "Open file dialog caption when choosing data source files");

            string[] filenames = App.OpenFileDialog("db", fileTypes.ToString(),
                ref filterIndex, caption, true, Project.Folder ?? App.ProjectFolder);

            if (filenames.Length == 0)
                return;

            Settings.Default.OFD_LastFileTypeChosen_DataSource = filterIndex;

            // Add the selected files to the data source list.
            foreach (string file in filenames)
            {
                if (ProjectContainsDataSource(file) &&
                    Utils.MsgBox(string.Format(DupDataSourceMsg, file),
                        MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    continue;
                }

                _dataSources.Add(new PaDataSource(Project.Fields, file));
            }

            LoadGrid(m_grid.Rows.Count);
            m_grid.Focus();
            m_grid.IsDirty = true;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Show the dialog to allow the user to specify a FieldWorks database as a data source.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleAddFw6DataSourceClick(object sender, EventArgs e)
        {
            // Make sure SQL Server is started.
            if (!FwDBUtils.IsSQLServerStarted && !FwDBUtils.StartSQLServer(true))
                return;

            using (var dlg = new FwProjectsDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ChosenDatabase != null)
                {
                    if (ProjectContainsFwDataSource(dlg.ChosenDatabase) &&
                        Utils.MsgBox(string.Format(DupDataSourceMsg, dlg.ChosenDatabase.ProjectName),
                            MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }

                    _dataSources.Add(new PaDataSource(Project.Fields, dlg.ChosenDatabase));
                    LoadGrid(m_grid.Rows.Count);
                    m_dirty = true;
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Show the dialog to allow the user to specify a FieldWorks database as a data source.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleAddFw7DataSourceClick(object sender, EventArgs e)
        {
            string name;
            string server;
            if (!FwDBUtils.GetFw7Project(this, out name, out server))
                return;

            Utils.WaitCursors(true);
            var info = new FwDataSourceInfo(name, server, DataSourceType.FW7);

            if (ProjectContainsFwDataSource(info) &&
                Utils.MsgBox(string.Format(DupDataSourceMsg, info.ProjectName),
                    MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            _dataSources.Add(new PaDataSource(Project.Fields, info));
            LoadGrid(m_grid.Rows.Count);
            m_dirty = true;
            Utils.WaitCursors(false);
        }

        /// ------------------------------------------------------------------------------------
        private string DupDataSourceMsg
        {
            get
            {
                return LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.DuplicateDataSourceQuestion",
                    "The data source '{0}' is already in your list of data sources.\n\nDo you want to add another copy?");
            }
        }

        /// ------------------------------------------------------------------------------------
        private void HandleAddButtonClick(object sender, EventArgs e)
        {
            var pt = btnAdd.PointToScreen(new Point(0, btnAdd.Height));
            mnuAdd.Show(pt);
        }

        /// ------------------------------------------------------------------------------------
        private void HandleRemoveButtonClick(object sender, EventArgs e)
        {
            var msg = LocalizationManager.GetString(
                "DialogBoxes.ProjectSettingsDlg.DeleteDataSourceConfirmationMsg",
                "Are you sure you want to delete the selected data source(s)?");

            if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            // Get indexes of selected rows, starting from end of the list.
            var indexesToDelete = m_grid.Rows.Cast<DataGridViewRow>().Where(r => r.Selected)
                .OrderByDescending(r => r.Index).Select(r => r.Index);

            foreach (int i in indexesToDelete)
                _dataSources.RemoveAt(i);

            m_grid.CurrentCellChanged -= HandleGridsCurrentCellChanged;
            LoadGrid(-1);
            m_grid.Focus();
            m_grid.IsDirty = true;
            m_grid.CurrentCellChanged += HandleGridsCurrentCellChanged;
            UpdateButtonStates();
        }

        /// ------------------------------------------------------------------------------------
        private void HandlePropertyButtonClick(object sender, EventArgs e)
        {
            if (m_grid.CurrentRow == null || m_grid.CurrentRow.Index >= _dataSources.Count)
                return;

            var dataSource = _dataSources[m_grid.CurrentRow.Index];

            if (dataSource.Type == DataSourceType.SFM || dataSource.Type == DataSourceType.Toolbox)
                ShowSfmMappingsDialog(dataSource);
            else if (dataSource.Type == DataSourceType.FW && dataSource.FwSourceDirectFromDB)
                ShowFwDataSourcePropertiesDialog(dataSource);
            else if (dataSource.Type == DataSourceType.FW7)
                ShowFw7DataSourcePropertiesDialog(dataSource);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Shows the mappings dialog for SFM and Toolbox data source types.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void ShowSfmMappingsDialog(PaDataSource ds)
        {
            // Make sure the file exists before going to the mappings dialog.
            if (!File.Exists(ds.SourceFile))
            {
                var msg = LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.DataSourceFileMissingMsg",
                    "The data source file '{0}' is missing.");

                msg = string.Format(msg, Utils.PrepFilePathForMsgBox(ds.SourceFile));
                Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            using (var dlg = new SFDataSourcePropertiesDlg(ds, Project.Fields))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ChangesWereMade)
                {
                    m_dirty = true;
                    Project.SynchronizeProjectFieldMappingsWithDataSourceFieldMappings();
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Show properties dialog for FW data sources of types where the data is being read
        /// directly from an FW database as opposed to a PAXML data source with some FW
        /// information in it.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void ShowFwDataSourcePropertiesDialog(PaDataSource ds)
        {
            if (ds.FwDataSourceInfo.IsMissing)
            {
                ds.FwDataSourceInfo.ShowMissingMessage();
                return;
            }

            using (var dlg = new FwDataSourcePropertiesDlg(Project, ds))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ChangesWereMade)
                {
                    m_dirty = true;
                    Project.SynchronizeProjectFieldMappingsWithDataSourceFieldMappings();
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        private void ShowFw7DataSourcePropertiesDialog(PaDataSource ds)
        {
            if (ds.FwDataSourceInfo.IsMissing)
            {
                ds.FwDataSourceInfo.ShowMissingMessage();
                return;
            }

            Utils.WaitCursors(true);
            using (var dlg = new Fw7DataSourcePropertiesDlg(ds, Project.Fields))
            {
                Utils.WaitCursors(false);
                if (dlg.ShowDialog(this) != DialogResult.OK || !dlg.ChangesWereMade)
                    return;
            }
            m_grid.Refresh();
            // Go through the new mappings and mark those that should be parsed.
            foreach (var mapping in ds.FieldMappings
                .Where(m => Settings.Default.ParsedFw7Fields.Contains(m.NameInDataSource)))
            {
                mapping.IsParsed = true;
            }

            Project.SynchronizeProjectFieldMappingsWithDataSourceFieldMappings();
            m_dirty = true;
        }

        /// ------------------------------------------------------------------------------------
        private void HandleSpecifyXSLTClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int filterIndex = Settings.Default.OFD_LastFileTypeChosen_DataSourceXslt;

            var filter = App.kstidFileTypeXSLT + "|" + App.kstidFileTypeAllFiles;
            var filename = App.OpenFileDialog("xslt", filter, ref filterIndex,
                LocalizationManager.GetString("DialogBoxes.ProjectSettingsDlg.XsltDataSourceOpenFileDialogCaption",
                    "Choose XSLT to Transform Data Source",
                    "Open file dialog caption when choosing an XSLT file"));

            if (filename != null)
            {
                _dataSources[e.RowIndex].XSLTFile = filename;
                m_grid.Refresh();
                Settings.Default.OFD_LastFileTypeChosen_DataSourceXslt = filterIndex;
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Returns true if the project contains a data source file with the specified name.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private bool ProjectContainsDataSource(string filename)
        {
            return _dataSources.Any(ds => ds.ToString().ToLower() == filename.ToLower());
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Returns true if the project contains a FW data source with the specified project
        /// and machine name.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private bool ProjectContainsFwDataSource(FwDataSourceInfo fwDataSourceInfo)
        {
            var name = fwDataSourceInfo.Name.ToLower();
            var server = (fwDataSourceInfo.Server == null ? null : fwDataSourceInfo.Server.ToLower());

            return _dataSources.Any(ds =>
                ds.FwDataSourceInfo != null &&
                ds.FwDataSourceInfo.Server != null &&
                ds.FwDataSourceInfo.Server.ToLower() == server &&
                ds.FwDataSourceInfo.Name != null &&
                ds.FwDataSourceInfo.Name.ToLower() == name);
        }

        /// ------------------------------------------------------------------------------------
        protected override void HandleHelpClick(object sender, EventArgs e)
        {
            if (_isProjectNew)
                App.ShowHelpTopic("hidNewProjectSettingsDlg");
            else
                base.HandleHelpClick(sender, e);
        }

        #endregion

        #region Painting methods
        ///// ------------------------------------------------------------------------------------
        ///// <summary>
        ///// Paint the ellipsis on the button where I want them.
        ///// </summary>
        ///// ------------------------------------------------------------------------------------
        //private void HandleButtonPaint(object sender, PaintEventArgs e)
        //{
        //    Button btn = sender as Button;

        //    if (btn == null)
        //        return;

        //    using (StringFormat sf = Utils.GetStringFormat(true))
        //    using (Font fnt = FontHelper.MakeFont(btn.Font, 8, FontStyle.Regular))
        //    {
        //        e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
        //        Rectangle rc = btn.ClientRectangle;
        //        e.Graphics.DrawString("...", fnt, SystemBrushes.ControlText, rc, sf);
        //    }
        //}

        ///// ------------------------------------------------------------------------------------
        ///// <summary>
        ///// Sizes and locates one of the buttons that look like their on the grid.
        ///// </summary>
        ///// ------------------------------------------------------------------------------------
        //private Rectangle LocateGridButton(Button btn, Rectangle rcCell)
        //{
        //    btn.Size = new Size(rcCell.Height + 1, rcCell.Height + 1);
        //    Point pt = m_grid.PointToScreen(new Point(rcCell.Right - rcCell.Height - 1, rcCell.Y - 1));
        //    btn.Location = m_grid.Parent.PointToClient(pt);
        //    btn.Invalidate();
        //    rcCell.Width -= (rcCell.Height + 2);
        //    return rcCell;
        //}

        #endregion

        #region Misc. Grid event handlers
        /// ------------------------------------------------------------------------------------
        void HandleGridsCurrentCellChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        /// ------------------------------------------------------------------------------------
        private void HandleGridCellNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            int i = e.RowIndex;

            if (i >= _dataSources.Count)
                return;

            switch (m_grid.Columns[e.ColumnIndex].Name)
            {
                case "skip":
                    e.Value = !_dataSources[i].SkipLoading;
                    break;
                case "sourcefiles":
                    e.Value = _dataSources[i].ToString(true);
                    break;
                case "type":
                    e.Value = _dataSources[i].TypeAsString;
                    break;
                case "Phonetic_Source":
                    var srcInfo = _dataSources[i].FwDataSourceInfo;
                    if (srcInfo != null)
                    {
                        e.Value = srcInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm
                            ? _dataSources[i].FwDataSourceInfo.PhoneticSourceField
                            : srcInfo.PhoneticStorageMethod.ToString();
                    }
                    else
                    {
                        var field = _dataSources[i].FieldMappings.Find(fld => fld.PaFieldName == "Phonetic");
                        if (field != null)
                            e.Value = field.NameInDataSource;
                        else
                            e.Value = null;
                    }
                    break;
                case "xslt":
                    e.Value = _dataSources[i].XSLTFile;
                    break;
                default:
                    e.Value = null;
                    break;
            }
        }

        /// ------------------------------------------------------------------------------------
        private void HandleGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && m_grid.Columns["skip"].Index == e.ColumnIndex)
                _dataSources[e.RowIndex].SkipLoading = !(bool)e.Value;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Only show the buttons in the current row under certain circumstances.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        void HandleCurrentRowChanged(object sender, EventArgs e)
        {
            int rowIndex = m_grid.CurrentCellAddress.Y;

            if (rowIndex >= 0 && rowIndex < _dataSources.Count)
            {
                var type = _dataSources[rowIndex].Type;
                ((SilButtonColumn)m_grid.Columns["xslt"]).ShowButton = (type == DataSourceType.XML);
            }
        }

        /// ------------------------------------------------------------------------------------
        private void HandleGridCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= m_grid.RowCount ||
                e.ColumnIndex != m_grid.Columns["sourcefiles"].Index)
            {
                return;
            }

            // Draw default everything but text
            var paintParts = DataGridViewPaintParts.All;
            paintParts &= ~DataGridViewPaintParts.ContentForeground;
            e.Paint(e.ClipBounds, paintParts);

            Color clr = (m_grid.Rows[e.RowIndex].Selected ?
                e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor);

            var flags = TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.PathEllipsis |
                (m_grid.RightToLeft == RightToLeft.Yes ?
                TextFormatFlags.RightToLeft : TextFormatFlags.Left) |
                TextFormatFlags.PreserveGraphicsClipping;

            TextRenderer.DrawText(e.Graphics,
                m_grid.Rows[e.RowIndex].Cells["sourcefiles"].Value as string,
                m_grid.Font, e.CellBounds, clr, flags);

            e.Handled = true;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Assume when the user double-clicks on a data source row, they want to go to the
        /// properties dialog.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleGridCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < m_grid.RowCount && e.ColumnIndex >= 0 && btnProperties.Enabled)
                btnProperties.PerformClick();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Assume when the user presses enter when the grid has focus that they want to go
        /// to the properties dialog.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleGridKeyDown(object sender, KeyEventArgs e)
        {
            if (btnProperties.Enabled && e.KeyCode == Keys.Enter)
            {
                btnProperties.PerformClick();
                e.Handled = true;
            }
        }

        /// ------------------------------------------------------------------------------------
        private void HandleEthnologueLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkData = Settings.Default.EthnologueIndexPage;

            if (txtLanguageCode.Text.Trim().Length == 3)
            {
                linkData = string.Format(
                    Settings.Default.EthnologueCodeSearch, txtLanguageCode.Text.Trim());
            }
            else if (txtLanguageName.Text.Trim().Length > 0)
            {
                linkData = string.Format(Settings.Default.EthnologueFirstLetterOfNameSearch,
                    txtLanguageName.Text.Trim()[0]);
            }

            Process.Start(linkData);
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
        }

        #endregion
    }
}