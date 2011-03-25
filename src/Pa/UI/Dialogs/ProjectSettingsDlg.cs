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
		public IEnumerable<string> NewlyMappedFields { get; private set; }
		private readonly IEnumerable<string> m_originallyMappedFields = new List<string>(0);

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
				m_originallyMappedFields = project.GetMappedFields().Select(f => f.Name).ToList();
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
		protected override void OnStringsLocalized()
		{
			if (!m_isProjectNew)
				base.OnStringsLocalized();
			else
			{
				Text = App.GetString("ProjectSettingsDlg.WindowTitleWhenProjectIsNew",
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

			m_grid.Columns.Add(SilGrid.CreateCheckBoxColumn("skip"));
			App.RegisterForLocalization(m_grid.Columns["skip"],
				"ProjectSettingsDlg.LoadDataSourceColumnHdg", "Load",
				"Column heading in data source list in project settings dialog box.");

		    DataGridViewColumn col = SilGrid.CreateTextBoxColumn("sourcefiles");
		    col.ReadOnly = true;
		    col.Width = 250;
			m_grid.Columns.Add(col);
			App.RegisterForLocalization(m_grid.Columns["sourceFiles"],
				"ProjectSettingsDlg.DataSourceNameColumnHdg", "Source",
				"Column heading in data source list in project settings dialog box.");

			col = SilGrid.CreateTextBoxColumn("type");
		    col.ReadOnly = true;
			col.Width = 75;
		    m_grid.Columns.Add(col);
			App.RegisterForLocalization(m_grid.Columns["type"],
				"ProjectSettingsDlg.DataSourceTypeColumnHdg", "Type",
				"Column heading in data source list in project settings dialog box.");

		    col = SilGrid.CreateSilButtonColumn("xslt");
		    col.ReadOnly = true;
		    col.Width = 170;
			((SilButtonColumn)col).ButtonWidth = 20;
			((SilButtonColumn)col).DrawTextWithEllipsisPath = true;
			((SilButtonColumn)col).ButtonClicked += HandleSpecifyXSLTClick;
			
			((SilButtonColumn)col).ButtonText = App.GetString("ProjectSettingsDlg.XsltColButtonText",
				"...", "Text on the button in the XSLT column in the project settings dialog");

			((SilButtonColumn)col).ButtonToolTip = App.GetString("ProjectSettingsDlg.XsltColButtonToolTip",
				"Specify XSLT", "Tooltip for the button in the XSLTe column in the project settings dialog");
			
			m_grid.Columns.Add(col);
			App.RegisterForLocalization(m_grid.Columns["xslt"],
				"ProjectSettingsDlg.DataSourceFileXSLTColumnHdg", "XSLT",
				"Column heading in data source list in project settings dialog box.");

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

			if (txtProjName.Text.Trim() == string.Empty)
			{
				// A project name was not specified.
				msg = App.GetString("ProjectSettingsDlg.MissingProjectNameMsg",
					"You must specify a project name.");

				offendingCtrl = txtProjName;
			}
			else if (txtLanguageName.Text.Trim() == string.Empty)
			{
				// A language name was not specified.
				msg = App.GetString("ProjectSettingsDlg.MissingLanguageNameMsg",
					"You must specify a language name.");
	
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
						
						msg = App.GetString("ProjectSettingsDlg.MissingXSLTMsg",
							"You must specify an XSLT file for '{0}'");

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
					//if (m_dataSources[i].Type == DataSourceType.FW && m_dataSources[i].FwSourceDirectFromDB &&
					//    !m_dataSources[i].FwDataSourceInfo.HasWritingSystemInfo("phonetic"))
					//{
					//    // FW data source information is incomplete.
					//    offendingIndex = i;

					//    msg = App.LocalizeString("ProjectSettingsDlg.MissingFwDatasourceWsMsg",
					//        "The writing system for the phonetic field has not been specified for the FieldWorks data source '{0}'.\n\nSelect the FieldWorks data source and click the properties button.",
					//        App.kLocalizationGroupDialogs);

					//    msg = string.Format(msg, m_dataSources[i].FwDataSourceInfo);
					//    break;
					//}
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
			for (int i = 0; i < m_dataSources.Count; i++)
			{
				if (!FieldMapping.IsPhoneticMapped(m_dataSources[i].FieldMappings, true))
				{
					m_grid.CurrentCell = m_grid[1, i];
					m_grid.Focus();
					return false;
				}

				if ((m_dataSources[i].Type == DataSourceType.SFM || m_dataSources[i].Type == DataSourceType.Toolbox) &&
					string.IsNullOrEmpty(m_dataSources[i].SfmRecordMarker))
				{
					var msg = App.GetString("ProjectSettingsDlg.NoSfmRecordMarkerSpecifiedErrorMsg",
						"A record marker must be specified for '{0}'.");
						
					msg = string.Format(msg, Path.GetFileName(m_dataSources[i].SourceFile));
					Utils.MsgBox(msg);
					m_grid.CurrentCell = m_grid[1, i];
					m_grid.Focus();
					return false;
				}
			}

			Utils.WaitCursors(true);
			Project.Name = txtProjName.Text.Trim();
			Project.LanguageName = txtLanguageName.Text.Trim();
			Project.LanguageCode = txtLanguageCode.Text.Trim();
			Project.Researcher = txtResearcher.Text.Trim();
			Project.Transcriber = txtTranscriber.Text.Trim();
			Project.SpeakerName = txtSpeaker.Text.Trim();
			Project.Comments = txtComments.Text.Trim();
			Project.DataSources = m_dataSources;
			Project.FixupFieldsAndMappings();

			if (m_isProjectNew)
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
								 where !m_originallyMappedFields.Contains(mapping.Field.Name)
								 select mapping.Field.Name).ToList();

			Utils.WaitCursors(false);
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
		/// <summary>
		/// Opens the save file dialog, asking the user what file name to give his new project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetProjectFileName()
		{
		    var dlg = new SaveFileDialog();
		    dlg.OverwritePrompt = true;
		    dlg.CheckFileExists = false;
		    dlg.CheckPathExists = true;
		    dlg.AddExtension = true;
		    dlg.DefaultExt = "pap";
		    dlg.RestoreDirectory = false;
		    dlg.InitialDirectory = (Settings.Default.LastFolderForSavedProject ?? App.DefaultProjectFolder);
			dlg.ShowHelp = false;
		    dlg.FilterIndex = 0;
			
			dlg.FileName = (txtProjName.Text.Trim() == string.Empty ?
				Project.Name : txtProjName.Text.Trim()) + ".pap";
			
			dlg.Filter = string.Format(App.kstidFileTypePAProject,
				Application.ProductName) + "|" + App.kstidFileTypeAllFiles;
			
		    dlg.Title = string.Format(App.GetString("ProjectSettingsDlg.ProjectSaveDialogText", "Save {0} Project File",
				"Caption for the save PA project dialog. The parameter is for the application name."),
				Application.ProductName);

		    var result = dlg.ShowDialog(this);

			if (result != DialogResult.Cancel && !string.IsNullOrEmpty(dlg.FileName))
			{
				Settings.Default.LastFolderForSavedProject = Path.GetDirectoryName(dlg.FileName);
				Settings.Default.Save();
				return dlg.FileName;
			}

			return null;
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

			var caption = App.GetString("ProjectSettingsDlg.DataSourceOpenFileDialogText",
				"Choose Data Source File(s)", "Open file dialog caption when choosing data source files");

			string[] filenames = App.OpenFileDialog("db", fileTypes.ToString(),
				ref filterIndex, caption, true, Project.Folder ?? App.DefaultProjectFolder);

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

				m_dataSources.Add(new PaDataSource(Project.Fields, file));
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

					m_dataSources.Add(new PaDataSource(Project.Fields, dlg.ChosenDatabase));
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

			m_dataSources.Add(new PaDataSource(Project.Fields, info));
			LoadGrid(m_grid.Rows.Count);
			m_dirty = true;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		private string DupDataSourceMsg
		{
			get
			{
				return App.GetString("ProjectSettingsDlg.DuplicateDataSourceQuestion",
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
			var msg = App.GetString("ProjectSettingsDlg.DeleteDataSourceConfirmationMsg",
				"Are you sure you want to delete the selected data source(s)?");

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
				var msg = App.GetString("ProjectSettingsDlg.DataSourceFileMissingMsg",
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
					Project.FixupFieldsAndMappings();
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
					Project.FixupFieldsAndMappings();
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

			// Go through the new mappings and mark those that should be parsed.
			foreach (var mapping in ds.FieldMappings
				.Where(m => Settings.Default.ParsedFw7Fields.Contains(m.NameInDataSource)))
			{
				mapping.IsParsed = true;
			}

			Project.FixupFieldsAndMappings();
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSpecifyXSLTClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int filterIndex = Settings.Default.OFD_LastFileTypeChosen_DataSourceXslt;

			var filter = App.kstidFileTypeXSLT + "|" + App.kstidFileTypeAllFiles;
			var filename = App.OpenFileDialog("xslt", filter, ref filterIndex,
				App.GetString("ProjectSettingsDlg.XsltDataSourceOpenFileDialogText",
					"Choose XSLT to Transform Data Source",
					"Open file dialog caption when choosing an XSLT file"));

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