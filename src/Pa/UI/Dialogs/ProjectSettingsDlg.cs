using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
		private readonly bool m_isProjectNew;
		private readonly List<PaDataSource> m_dataSources = new List<PaDataSource>();

		public PaProject Project { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ProjectSettingsDlg()
		{
			InitializeComponent();
			
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
		}

		/// ------------------------------------------------------------------------------------
		public ProjectSettingsDlg(PaProject project) : this()
		{
			Application.DoEvents();

			m_isProjectNew = (project == null);

			BuildGrid();
			pnlGridHdg.ControlReceivingFocusOnMnemonic = m_grid;
			pnlGridHdg.BorderStyle = BorderStyle.None;

			if (project == null)
				Project = new PaProject(true);
			else
			{
				Project = project;
				m_dataSources = project.DataSources.Select(ds => ds.Copy()).ToList();
				txtProjName.Text = project.Name;
				txtLanguageName.Text = project.LanguageName;
				txtLanguageCode.Text = project.LanguageCode;
				txtResearcher.Text = project.Researcher;
				txtTranscriber.Text = project.Transcriber;
				txtSpeaker.Text = project.SpeakerName;
				txtComments.Text = project.Comments;
				LoadGrid(-1);
			}

			Utils.WaitCursors(true);
	
			if (m_dataSources.Any(ds => ds.Type == DataSourceType.FW))
				FwDBUtils.StartSQLServer(true);

			mnuAddFw6DataSource.Enabled = FwDBUtils.IsSQLServerInstalled(false);
			mnuAddFw7DataSource.Enabled = FwDBUtils.IsFw7Installed;

			DialogResult = DialogResult.Cancel;
			m_dirty = m_isProjectNew;
			m_grid.IsDirty = false;
			Utils.WaitCursors(false);

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
		protected override void SetWindowText()
		{
			if (!m_isProjectNew)
				base.SetWindowText();
			else
			{
				Text = App.LocalizeString("ProjectSettingsDlg.WindowTitleWhenProjectIsNew",
					"New Project Settings", "Caption for project settings dialog when project is new.",
					App.kLocalizationGroupDialogs);
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

			m_grid.Columns.Add(SilGrid.CreateCheckBoxColumn("skip"));
			App.LocalizeObject(m_grid.Columns["skip"],
				"ProjectSettingsDlg.LoadDataSourceColumnHdg", "Load",
				"Column heading in data source list in project settings dialog box.",
				App.kLocalizationGroupDialogs);

		    DataGridViewColumn col = SilGrid.CreateTextBoxColumn("sourcefiles");
		    col.ReadOnly = true;
		    col.Width = 250;
			m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns["sourceFiles"],
				"ProjectSettingsDlg.DataSourceNameColumnHdg", "Source",
				"Column heading in data source list in project settings dialog box.",
				App.kLocalizationGroupDialogs);

			col = SilGrid.CreateTextBoxColumn("type");
		    col.ReadOnly = true;
			col.Width = 75;
		    m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns["type"],
				"ProjectSettingsDlg.DataSourceTypeColumnHdg", "Type",
				"Column heading in data source list in project settings dialog box.",
				App.kLocalizationGroupDialogs);

		    col = SilGrid.CreateSilButtonColumn("xslt");
		    col.ReadOnly = true;
		    col.Width = 170;
			((SilButtonColumn)col).ButtonWidth = 20;
			((SilButtonColumn)col).DrawTextWithEllipsisPath = true;
			((SilButtonColumn)col).ButtonText = Properties.Resources.kstidXSLTColButtonText;
			((SilButtonColumn)col).ButtonToolTip = Properties.Resources.kstidXSLTColButtonToolTip;
			((SilButtonColumn)col).ButtonClicked += HandleSpecifyXSLTClick;
			m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns["xslt"],
				"ProjectSettingsDlg.DataSourceFileXSLTColumnHdg", "XSLT",
				"Column heading in data source list in project settings dialog box.",
				App.kLocalizationGroupDialogs);

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
			if (m_dataSources.Count == 0)
				return;

			m_grid.RowCount = m_dataSources.Count;

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
				(m_grid.CurrentRow != null && m_grid.CurrentRow.Index < m_dataSources.Count));
			
			btnRemove.Enabled = enableRemoveButton;

			bool enablePropertiesButton = false;

			if (m_grid.SelectedRows.Count <= 1 && m_grid.CurrentRow != null &&
				m_grid.CurrentRow.Index < m_dataSources.Count)
			{
				var dataSource = m_dataSources[m_grid.CurrentRow.Index];

				enablePropertiesButton = (
					dataSource.Type == DataSourceType.SFM ||
					dataSource.Type == DataSourceType.Toolbox ||
					dataSource.Type == DataSourceType.FW7 ||
					(dataSource.Type == DataSourceType.FW && dataSource.FwSourceDirectFromDB));
			}

			btnProperties.Enabled = enablePropertiesButton;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxTextChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		#region Saving Settings and Verifying/Saving changes
		///// ------------------------------------------------------------------------------------
		//protected override void OnFormClosing(FormClosingEventArgs e)
		//{
		//    base.OnFormClosing(e);

		//    if (e.Cancel || DialogResult == DialogResult.OK)
		//        return;

		//    // If the project isn't new and the user is NOT saving the project
		//    // settings then reload the original project.
		//    if (!m_isProjectNew)
		//    {
		//        var project = m_project.ReLoadProjectFileOnly();
		//        if (project != null)
		//        {
		//            if (App.Project != null)
		//                App.Project.Dispose();

		//            App.Project = project;
		//        }
		//    }

		//    m_project.Dispose();
		//    m_project = null;
		//}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.ProjectSettingsDlgGrid = GridSettings.Create(m_grid);
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return (m_dirty || m_grid.IsDirty); }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			string msg = null;
			int offendingIndex = -1;
			Control offendingCtrl = null;

			// Verify a project name was specified.
			if (txtProjName.Text.Trim() == string.Empty)
			{
				msg = App.LocalizeString("ProjectSettingsDlg.MissingProjectNameMsg",
					"You must specify a project name.", App.kLocalizationGroupDialogs);

				offendingCtrl = txtProjName;
			}
			else if (txtLanguageName.Text.Trim() == string.Empty)
			{
				msg = App.LocalizeString("ProjectSettingsDlg.MissingLanguageNameMsg",
					"You must specify a language name.", App.kLocalizationGroupDialogs);
	
				offendingCtrl = txtLanguageName;
			}
			else
			{
				for (int i = 0; i < m_dataSources.Count; i++)
				{
					if (m_dataSources[i].Type == DataSourceType.PAXML)
						continue;

					if (m_dataSources[i].Type == DataSourceType.XML &&
						string.IsNullOrEmpty(m_dataSources[i].XSLTFile))
					{
						// No XSLT file was specified
						offendingIndex = i;
						
						msg = App.LocalizeString("ProjectSettingsDlg.MissingXSLTMsg",
							"You must specify an XSLT file for '{0}'", App.kLocalizationGroupDialogs);

						msg = string.Format(msg, Utils.PrepFilePathForMsgBox(m_dataSources[i].SourceFile));
						break;
					}
					
					//if (!Project.DataSources[i].MappingsExist &&
					//    (Project.DataSources[i].Type == DataSourceType.SFM ||
					//    Project.DataSources[i].Type == DataSourceType.Toolbox))
					//{
					//    // No mappings have been specified.
					//    offendingIndex = i;
					//    msg = App.LocalizeString("ProjectSettingsDlg.NoMappingsMsg",
					//        "You must specify field mappings for\n\n'{0}'.\n\nSelect it in the Data Sources list and click 'Properties'.",
					//        App.kLocalizationGroupDialogs);
						
					//    msg = string.Format(msg, Project.DataSources[i].DataSourceFile);
					//    break;
					//}
					
					// TODO: Fix so "phonetic" is not hardcoded.
					if (m_dataSources[i].Type == DataSourceType.FW && m_dataSources[i].FwSourceDirectFromDB &&
						!m_dataSources[i].FwDataSourceInfo.HasWritingSystemInfo("phonetic"))
					{
						// FW data source information is incomplete.
						offendingIndex = i;

						msg = App.LocalizeString("ProjectSettingsDlg.MissingFwDatasourceWsMsg",
							"The writing system for the phonetic field has not been specified for the FieldWorks data source '{0}'.\n\nSelect the FieldWorks data source and click the properties button.",
							App.kLocalizationGroupDialogs);

						msg = string.Format(msg, m_dataSources[i].FwDataSourceInfo);
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

			Project.Name = txtProjName.Text.Trim();
			Project.LanguageName = txtLanguageName.Text.Trim();
			Project.LanguageCode = txtLanguageCode.Text.Trim();
			Project.Researcher = txtResearcher.Text.Trim();
			Project.Transcriber = txtTranscriber.Text.Trim();
			Project.SpeakerName = txtSpeaker.Text.Trim();
			Project.Comments = txtComments.Text.Trim();
			Project.DataSources = m_dataSources;
			Project.SetFields(m_dataSources.SelectMany(ds => ds.FieldMappings)
				.Select(m => m.Field).Distinct(new FieldNameComparer()));
			
			Project.Save();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Opens the save file dialog, asking the user what file name to give his new project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetProjectFileName()
		{
		    SaveFileDialog dlg = new SaveFileDialog();
		    dlg.OverwritePrompt = true;
		    dlg.CheckFileExists = false;
		    dlg.CheckPathExists = true;
		    dlg.AddExtension = true;
		    dlg.DefaultExt = "pap";
			
			dlg.Filter = string.Format(App.kstidFileTypePAProject,
				Application.ProductName) + "|" + App.kstidFileTypeAllFiles;
			
			dlg.ShowHelp = false;
		    dlg.Title = string.Format(Properties.Resources.kstidPAFilesCaptionSFD, Application.ProductName);
		    dlg.RestoreDirectory = false;
		    dlg.InitialDirectory = Environment.CurrentDirectory;
		    dlg.FilterIndex = 0;
			dlg.FileName = (txtProjName.Text.Trim() == string.Empty ?
				Project.Name : txtProjName.Text.Trim()) + ".pap";

		    var result = dlg.ShowDialog(this);

		    return (string.IsNullOrEmpty(dlg.FileName) || result == DialogResult.Cancel ?
				null : dlg.FileName);
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

			StringBuilder fileTypes = new StringBuilder();
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

			string[] filenames = App.OpenFileDialog("db", fileTypes.ToString(),
				ref filterIndex, Properties.Resources.kstidDataSourceOpenFileCaption,
				true, Project.Folder ?? App.DefaultProjectFolder);

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

				m_dataSources.Add(new PaDataSource(file));
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

			using (var dlg = new FwProjectsDlg(Project))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ChosenDatabase != null)
				{
					if (ProjectContainsFwDataSource(dlg.ChosenDatabase) &&
						Utils.MsgBox(string.Format(DupDataSourceMsg, dlg.ChosenDatabase.ProjectName),
							MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}

					m_dataSources.Add(new PaDataSource(dlg.ChosenDatabase));
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

			var info = new FwDataSourceInfo(name, server, DataSourceType.FW7);

			if (ProjectContainsFwDataSource(info) &&
				Utils.MsgBox(string.Format(DupDataSourceMsg, info.ProjectName),
					MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}

			m_dataSources.Add(new PaDataSource(info));
			LoadGrid(m_grid.Rows.Count);
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		private string DupDataSourceMsg
		{
			get
			{
				return App.LocalizeString("ProjectSettingsDlg.DuplicateDataSourceQuestion",
					"The data source '{0}' is already in your list of data sources.\n\nDo you want to add another copy?",
					App.kLocalizationGroupDialogs);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleAddButtonClick(object sender, EventArgs e)
		{
			Point pt = btnAdd.PointToScreen(new Point(0, btnAdd.Height));
			mnuAdd.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRemoveButtonClick(object sender, EventArgs e)
		{
			var msg = App.LocalizeString("ProjectSettingsDlg.DeleteDataSourceConfirmationMsg",
				"Are you sure you want to delete the selected data source(s)?",
				App.kLocalizationGroupDialogs);

			if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;

			// Get indexes of selected rows, starting from end of the list.
			var indexesToDelete = m_grid.Rows.Cast<DataGridViewRow>().Where(r => r.Selected)
				.OrderByDescending(r => r.Index).Select(r => r.Index);

			foreach (int i in indexesToDelete)
				m_dataSources.RemoveAt(i);

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
			if (m_grid.CurrentRow == null || m_grid.CurrentRow.Index >= m_dataSources.Count)
				return;

			var dataSource = m_dataSources[m_grid.CurrentRow.Index];

			if (dataSource.Type == DataSourceType.SFM ||
				dataSource.Type == DataSourceType.Toolbox)
			{
				ShowMappingsDialog(dataSource);
			}
			else if ((dataSource.Type == DataSourceType.FW &&
				dataSource.FwSourceDirectFromDB) || dataSource.Type == DataSourceType.FW7)
			{
				ShowFwDataSourcePropertiesDialog(dataSource);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the mappings dialog for SFM and Toolbox data source types.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowMappingsDialog(PaDataSource ds)
		{
			// Make sure the file exists before going to the mappings dialog.
			if (!File.Exists(ds.SourceFile))
			{
				var msg = App.LocalizeString("ProjectSettingsDlg.DataSourceFileMissingMsg",
					"The data source file '{0}' is missing.", App.kLocalizationGroupDialogs);

				msg = string.Format(msg, Utils.PrepFilePathForMsgBox(ds.SourceFile));
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			using (var dlg = new SFDataSourcePropertiesDlg(ds, GetFieldsMappedToSfm()))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ChangesWereMade)
					m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all fields, from all the SFM data sources, that have been mapped to markers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private IEnumerable<PaField> GetFieldsMappedToSfm()
		{
			// Get only those fields found in SFM/Toolbox data sources.
			return m_dataSources.Where(ds => ds.FieldMappings != null && ds.IsSfmType)
				.SelectMany(ds => ds.FieldMappings)
				.Select(m => m.Field).Distinct(new FieldNameComparer());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show properties dialog for FW data sources of types where the data is being read
		/// directly from an FW database as opposed to a PAXML data source with some FW
		/// information in it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowFwDataSourcePropertiesDialog(PaDataSource dataSource)
		{
			if (dataSource.FwDataSourceInfo.IsMissing)
			{
				dataSource.FwDataSourceInfo.ShowMissingMessage();
				return;
			}

			using (var dlg = new FwDataSourcePropertiesDlg(Project, dataSource.FwDataSourceInfo))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ChangesWereMade)
					m_dirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSpecifyXSLTClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int filterIndex = Settings.Default.OFD_LastFileTypeChosen_DataSourceXslt;

			var filter = App.kstidFileTypeXSLT + "|" + App.kstidFileTypeAllFiles;
			var filename = App.OpenFileDialog("xslt", filter, ref filterIndex,
				Properties.Resources.kstidDataSourceOpenFileXSLTCaption);

			if (filename != null)
			{
				m_dataSources[e.RowIndex].XSLTFile = filename;
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
			return m_dataSources.Any(ds => ds.ToString().ToLower() == filename.ToLower());
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

			return m_dataSources.Any(ds =>
				ds.FwDataSourceInfo != null &&
				ds.FwDataSourceInfo.Server != null &&
				ds.FwDataSourceInfo.Server.ToLower() == server &&
				ds.FwDataSourceInfo.Name != null &&
				ds.FwDataSourceInfo.Name.ToLower() == name);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			if (m_isProjectNew)
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

			if (i >= m_dataSources.Count)
				return;

			switch (m_grid.Columns[e.ColumnIndex].Name)
			{
				case "skip": e.Value = !m_dataSources[i].SkipLoading; break;
				case "sourcefiles": e.Value = m_dataSources[i].ToString(true); break;
				case "type": e.Value = m_dataSources[i].TypeAsString; break;
				case "xslt": e.Value = m_dataSources[i].XSLTFile; break;
				default: e.Value = null; break;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex >= 0 && m_grid.Columns["skip"].Index == e.ColumnIndex)
				m_dataSources[e.RowIndex].SkipLoading = !(bool)e.Value;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only show the buttons in the current row under certain circumstances.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleCurrentRowChanged(object sender, EventArgs e)
		{
			int rowIndex = m_grid.CurrentCellAddress.Y;

			if (rowIndex >= 0 && rowIndex < m_dataSources.Count)
			{
				var type = m_dataSources[rowIndex].Type;
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

		#endregion
	}
}