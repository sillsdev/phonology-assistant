using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Localization;
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ProjectSettingsDlg : OKCancelDlgBase
	{
		private PaProject m_project;
		private readonly bool m_newProject;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ProjectSettingsDlg() : this(null)
		{
			base.Text = Properties.Resources.kstidNewProjectSettingsDlgCaption;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ProjectSettingsDlg(PaProject project)
		{
			Application.DoEvents();

			// Make sure to save the project's field info. list because we may change it while working in
			// this dialog or it's child dialogs (more specifically the custom fields dialog).
			if (project != null)
				project.FieldInfo.Save(project);

			InitializeComponent();

			base.Text = Properties.Resources.kstidProjectSettingsDlgCaption;
			m_newProject = (project == null);

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

			BuildGrid();
			pnlGridHdg.BorderStyle = BorderStyle.None;
			pnlGridHdg.ControlReceivingFocusOnMnemonic = m_grid;

			if (project == null)
				m_project = new PaProject(true);
			else
			{
				m_project = project;
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
			FwDataSourcePrep();
			cmnuAddFwDataSource.Enabled = FwDBUtils.IsSQLServerInstalled(false);
			
			DialogResult = DialogResult.Cancel;
			m_dirty = m_newProject;
			m_grid.IsDirty = false;
			Utils.WaitCursors(false);

			UpdateButtonStates();
			Disposed += ProjectSettingsDlg_Disposed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ProjectSettingsDlg_Disposed(object sender, EventArgs e)
		{
			Disposed -= ProjectSettingsDlg_Disposed;

			if (m_grid != null && !m_grid.IsDisposed)
			{
				m_grid.CurrentCellChanged -= m_grid_CurrentCellChanged;
				m_grid.CellClick -= m_grid_CellClick;
				m_grid.RowsAdded -= m_grid_RowsAdded;
				m_grid.RowsRemoved -= m_grid_RowsRemoved;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check for any FW data sources in the project. If any are found, attempt to start 
		/// SQL server if it isn't already. Also backup the writing system information in case
		/// the user goes to the FW data source properties dialog to make changes to the
		/// writing system information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FwDataSourcePrep()
		{
			if (m_project.DataSources == null)
				return;

			foreach (PaDataSource ds in m_project.DataSources)
			{
				if (ds.DataSourceType == DataSourceType.FW)
				{
					FwDBUtils.StartSQLServer(true);
					break;
				}
			}
		}

		#region Grid setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
		    m_grid.Name = Name + "Grid";
		    m_grid.AutoGenerateColumns = false;
			m_grid.MultiSelect = true;
		    m_grid.Font = FontHelper.UIFont;
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			m_grid.RowEnter += m_grid_RowEnter;

		    DataGridViewColumn col = SilGrid.CreateTextBoxColumn("sourcefiles");
		    col.ReadOnly = true;
		    col.Width = 250;
			m_grid.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_grid.Columns["sourceFiles"],
				"ProjectSettingsDlg.DataSourceFileColumnHdg", "Source", null, null,
				"Column heading in data source list in project settings dialog box.",
				"Dialog Boxes", LocalizationCategory.DataGridViewColumnHeading,
				LocalizationPriority.High);

		    col = SilGrid.CreateTextBoxColumn("type");
		    col.ReadOnly = true;
		    col.Width = 75;
		    m_grid.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_grid.Columns["type"],
				"ProjectSettingsDlg.DataSourceFileTypeColumnHdg", "Type", null, null,
				"Column heading in data source list in project settings dialog box.",
				"Dialog Boxes", LocalizationCategory.DataGridViewColumnHeading,
				LocalizationPriority.High);

		    col = SilGrid.CreateSilButtonColumn("xslt");
		    col.ReadOnly = true;
		    col.Width = 170;
			((SilButtonColumn)col).ButtonWidth = 20;
			((SilButtonColumn)col).DrawTextWithEllipsisPath = true;
			((SilButtonColumn)col).ButtonText = Properties.Resources.kstidXSLTColButtonText;
			((SilButtonColumn)col).ButtonToolTip = Properties.Resources.kstidXSLTColButtonToolTip;
			((SilButtonColumn)col).ButtonClicked += HandleSpecifyXSLTClick;
			m_grid.Columns.Add(col);
			App.L10NMngr.LocalizeObject(m_grid.Columns["xslt"],
				"ProjectSettingsDlg.DataSourceFileXSLTColumnHdg", "XSLT", null, null,
				"Column heading in data source list in project settings dialog box.",
				"Dialog Boxes", LocalizationCategory.DataGridViewColumnHeading,
				LocalizationPriority.High);

			if (Settings.Default.ProjectSettingsDlgGrid != null)
				Settings.Default.ProjectSettingsDlgGrid.InitializeGrid(m_grid);

			// When xslt transforms are supported when reading data, then this should become visible.
			m_grid.Columns["xslt"].Visible = false;

			m_grid.CurrentCellChanged += m_grid_CurrentCellChanged;
			m_grid.CellClick += m_grid_CellClick;
			m_grid.RowsAdded += m_grid_RowsAdded;
			m_grid.RowsRemoved += m_grid_RowsRemoved;
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
			if (m_project.DataSources == null || m_project.DataSources.Count == 0)
				return;

			m_grid.Rows.Add(m_project.DataSources.Count);

			for (int i = 0; i < m_project.DataSources.Count; i++)
			{
				m_grid.Rows[i].Cells["sourcefiles"].Value = m_project.DataSources[i].ToString(true);
				m_grid.Rows[i].Cells["type"].Value = m_project.DataSources[i].DataSourceTypeString;
				m_grid.Rows[i].Cells["xslt"].Value = m_project.DataSources[i].XSLTFile;
			}

			// If the current row used to be the last row and that last row no
			// longer exists, then make the new current row the new last row.
			if (preferredRow == m_grid.Rows.Count)
				preferredRow--;

			// Try to restore the current row to what it was before removing all the rows.
			if (m_grid.Rows.Count > 0 && preferredRow >= 0 && preferredRow < m_grid.Rows.Count)
				m_grid.CurrentCell = m_grid[0, preferredRow];
		}

		#endregion

		#region Methods for enabling and disabling buttons
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			UpdateButtonStates();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			UpdateButtonStates();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			UpdateButtonStates();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_CurrentCellChanged(object sender, EventArgs e)
		{
			UpdateButtonStates();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update button enabled states.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateButtonStates()
		{
			bool enableRemoveButton = ((m_grid.CurrentRow != null &&
				m_grid.CurrentRow.Index < m_project.DataSources.Count) ||
				(m_grid.SelectedRows.Count > 0));
			
			if (enableRemoveButton != btnRemove.Enabled)
				btnRemove.Enabled = enableRemoveButton;

			bool enablePropertiesButton = false;

			if (m_grid.CurrentRow != null && m_grid.CurrentRow.Index < m_project.DataSources.Count)
			{
				if (m_grid.SelectedRows.Count <= 1)
				{
					PaDataSource dataSource = m_project.DataSources[m_grid.CurrentRow.Index];

					enablePropertiesButton =
						(dataSource.DataSourceType == DataSourceType.SFM ||
						dataSource.DataSourceType == DataSourceType.Toolbox ||
						(dataSource.DataSourceType == DataSourceType.FW &&
						dataSource.FwSourceDirectFromDB));
				}
			}

			if (btnProperties.Enabled != enablePropertiesButton)
				btnProperties.Enabled = enablePropertiesButton;
		}

		#endregion
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject Project
		{
		    get { return m_project; }
		    set { m_project = value; }
		}

		#region Saving Settings and Verifying/Saving changes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (!e.Cancel && DialogResult != DialogResult.OK)
			{
				// If the project isn't new and the user is NOT saving the project
				// settings then reload the original field info. for the project.
				if (!m_newProject)
				{
					PaProject project = m_project.ReLoadProjectFileOnly(true);
					if (project != null)
					{
						if (App.Project != null)
							App.Project.Dispose();

						App.Project = project;
					}
				}

				m_project.Dispose();
				m_project = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.ProjectSettingsDlgGrid = GridSettings.Create(m_grid);
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return (m_dirty || m_grid.IsDirty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			string msg = null;
			int offendingIndex = -1;
			Control offendingCtrl = null;

			// Verify a project name was specified.
			if (txtProjName.Text.Trim() == string.Empty)
			{
				msg = App.L10NMngr.LocalizeString(
					"Dialog Boxes.ProjectSettingsDlg.MissingProjectNameMsg",
					"You must specify a project name.", null,
					App.kLocalizationGroupDialogs, LocalizationCategory.ErrorOrWarningMessage,
					LocalizationPriority.Medium);

				offendingCtrl = txtProjName;
			}
			else if (txtLanguageName.Text.Trim() == string.Empty)
			{
				msg = App.L10NMngr.LocalizeString(
					"Dialog Boxes.ProjectSettingsDlg.MissingLanguageNameMsg",
					"You must specify a language name.", null,
					App.kLocalizationGroupDialogs, LocalizationCategory.ErrorOrWarningMessage,
					LocalizationPriority.Medium);
	
				offendingCtrl = txtLanguageName;
			}
			else
			{
				for (int i = 0; i < m_project.DataSources.Count; i++)
				{
					if (m_project.DataSources[i].DataSourceType == DataSourceType.PAXML)
						continue;

					if (m_project.DataSources[i].DataSourceType == DataSourceType.XML &&
						string.IsNullOrEmpty(m_project.DataSources[i].XSLTFile))
					{
						// No XSLT file was specified
						offendingIndex = i;
						
						msg = App.L10NMngr.LocalizeString("Dialog Boxes.ProjectSettingsDlg.MissingXSLTMsg",
							"You must specify an XSLT file for '{0}'", null,
							App.kLocalizationGroupDialogs, LocalizationCategory.ErrorOrWarningMessage,
							LocalizationPriority.Medium);

						msg = string.Format(msg, Utils.PrepFilePathForMsgBox(m_project.DataSources[i].DataSourceFile));
						break;
					}
					
					if (!m_project.DataSources[i].MappingsExist &&
						(m_project.DataSources[i].DataSourceType == DataSourceType.SFM ||
						m_project.DataSources[i].DataSourceType == DataSourceType.Toolbox))
					{
						// No mappings have been specified.
						offendingIndex = i;
						msg = App.L10NMngr.LocalizeString("Dialog Boxes.ProjectSettingsDlg.NoMappingsMsg",
							"You must specify field mappings for\n\n'{0}'.\n\nSelect it in the Data Sources list and click 'Properties'.",
							null, App.kLocalizationGroupDialogs, LocalizationCategory.ErrorOrWarningMessage,
							LocalizationPriority.Medium);
						
						msg = string.Format(msg, m_project.DataSources[i].DataSourceFile);
						break;
					}
					
					if (m_project.DataSources[i].DataSourceType == DataSourceType.FW &&
						m_project.DataSources[i].FwSourceDirectFromDB &&
						!m_project.DataSources[i].FwDataSourceInfo.HasWritingSystemInfo(
							m_project.FieldInfo.PhoneticField.FwQueryFieldName))
					{
						// FW data source information is incomplete.
						offendingIndex = i;

						msg = App.L10NMngr.LocalizeString("Dialog Boxes.ProjectSettingsDlg.MissingFwDatasourceWsMsg",
							"The writing system for the phonetic field has not been specified for the FieldWorks data source '{0}'.\n\nSelect the FieldWorks data source and click the properties button.",
							null, App.kLocalizationGroupDialogs, LocalizationCategory.ErrorOrWarningMessage,
							LocalizationPriority.Medium);

						msg = string.Format(msg, m_project.DataSources[i].FwDataSourceInfo);
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
					m_grid.CurrentCell = m_grid[0, offendingIndex];
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
			if (m_project.FileName == null)
			{
				m_project.FileName = GetProjectFileName();
				if (m_project.FileName == null)
					return false;
			}

			m_project.Name = txtProjName.Text.Trim();
			m_project.LanguageName = txtLanguageName.Text.Trim();
			m_project.LanguageCode = txtLanguageCode.Text.Trim();
			m_project.Researcher = txtResearcher.Text.Trim();
			m_project.Transcriber = txtTranscriber.Text.Trim();
			m_project.SpeakerName = txtSpeaker.Text.Trim();
			m_project.Comments = txtComments.Text.Trim();
			m_project.Save();

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
				m_project.Name : txtProjName.Text.Trim()) + ".pap";

		    DialogResult result = dlg.ShowDialog(this);

		    return (string.IsNullOrEmpty(dlg.FileName) || result == DialogResult.Cancel ?
				null : dlg.FileName);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTextChanged(object sender, EventArgs e)
		{
		    m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only show the buttons in the current row under certain circumstances.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			int row = e.RowIndex;

			if (e.RowIndex >= 0 && row < m_project.DataSources.Count)
			{
				DataSourceType type = m_project.DataSources[row].DataSourceType;
				((SilButtonColumn)m_grid.Columns["xslt"]).ShowButton = (type == DataSourceType.XML);
			}
		}

		#region Button click handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the open file dialog so the user may specify a non FieldWorks data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuAddOtherDataSource_Click(object sender, EventArgs e)
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
				true, m_project.Folder ?? App.DefaultProjectFolder);

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

				m_project.DataSources.Add(new PaDataSource(file));
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
		private void cmnuAddFwDataSource_Click(object sender, EventArgs e)
		{
			// Make sure SQL Server is started.
			if (!FwDBUtils.IsSQLServerStarted)
			{
				// Start SQL Server
				if (!FwDBUtils.StartSQLServer(true))
					return;
			}

			// This commented out code was used when the program could only look
			// on the current machine for FieldWorks databases. Now it can look
			// for remote databases on other computers on a network.
			//// See if there are any FW databases on this computer.
			//FwDataSourceInfo[] fwDataSourceInfo = FwDBUtils.FwDataSourceInfoList;
			//if (fwDataSourceInfo == null)
			//{
			//    Utils.MsgBox(Properties.Resources.kstidNoFwProjectsFoundMsg,
			//        MessageBoxButtons.OK);

			//    return;
			//}

			using (FwProjectsDlg dlg = new FwProjectsDlg(m_project))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ChosenDatabase != null)
				{
					if (ProjectContainsFwDataSource(dlg.ChosenDatabase) &&
						Utils.MsgBox(string.Format(DupDataSourceMsg, dlg.ChosenDatabase.ProjectName),
							MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}

					m_project.DataSources.Add(new PaDataSource(dlg.ChosenDatabase));
					LoadGrid(m_grid.Rows.Count);
					m_dirty = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private string DupDataSourceMsg
		{
			get
			{
				return App.L10NMngr.LocalizeString(
					"ProjectSettingsDlg.DuplicateDataSourceQuestion",
					"The data source '{0}' is already in your list of data sources.\n\nDo you want to add another copy?",
					"Dialog Boxes");
			}
		}

		/// ------------------------------------------------------------------------------------
		private void btnAdd_Click(object sender, EventArgs e)
		{
			Point pt = btnAdd.PointToScreen(new Point(0, btnAdd.Height));
			cmnuAdd.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (Utils.MsgBox(Properties.Resources.kstidDataSourceDeleteConfirmation,
				MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				List<int> indexesToDelete = new List<int>();

				// Start from the end of the list.
				for (int i = m_grid.Rows.Count - 1; i >= 0; i--)
				{
					if (m_grid.Rows[i].Selected)
						indexesToDelete.Add(i);
				}

				foreach (int i in indexesToDelete)
					m_project.DataSources.RemoveAt(i);

				LoadGrid(-1);
				m_grid.Focus();
				m_grid.IsDirty = true;
			}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void HandleDataSourceFilePropertiesClick(object sender, DataGridViewCellMouseEventArgs e)
		//{
		//    PaDataSource dataSource = m_project.DataSources[e.RowIndex];

		//    if (dataSource.DataSourceType == DataSourceType.SFM ||
		//        dataSource.DataSourceType == DataSourceType.Toolbox)
		//    {
		//        ShowMappingsDialog(dataSource);
		//    }
		//    else if (dataSource.DataSourceType == DataSourceType.FW &&
		//        dataSource.FwSourceDirectFromDB)
		//    {
		//        ShowFwDataSourcePropertiesDialog(dataSource);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnProperties_Click(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow == null || m_grid.CurrentRow.Index >= m_project.DataSources.Count)
				return;

			PaDataSource dataSource = m_project.DataSources[m_grid.CurrentRow.Index];

			if (dataSource.DataSourceType == DataSourceType.SFM ||
				dataSource.DataSourceType == DataSourceType.Toolbox)
			{
				ShowMappingsDialog(dataSource);
			}
			else if (dataSource.DataSourceType == DataSourceType.FW &&
				dataSource.FwSourceDirectFromDB)
			{
				ShowFwDataSourcePropertiesDialog(dataSource);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the mappings dialog for SFM and Toolbox data source types.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowMappingsDialog(PaDataSource dataSource)
		{
			string filename = dataSource.DataSourceFile;

			// Make sure the file exists before going to the mappings dialog.
			if (!File.Exists(filename))
			{
				string filePath = Utils.PrepFilePathForMsgBox(filename);
				Utils.MsgBox(
					string.Format(Properties.Resources.kstidFileMissingMsg, filePath),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return;
			}

			// Open the mappings dialog.
			using (SFDataSourcePropertiesDlg dlg =
				new SFDataSourcePropertiesDlg(m_project.FieldInfo, dataSource))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					if (dlg.ChangesWereMade)
						m_dirty = true;
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
		private void ShowFwDataSourcePropertiesDialog(PaDataSource dataSource)
		{
			if (dataSource.FwDataSourceInfo.IsMissing)
			{
				dataSource.FwDataSourceInfo.ShowMissingMessage();
				return;
			}

			using (FwDataSourcePropertiesDlg dlg =
				new FwDataSourcePropertiesDlg(m_project, dataSource.FwDataSourceInfo))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					if (dlg.ChangesWereMade)
						m_dirty = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSpecifyXSLTClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int filterIndex = Settings.Default.OFD_LastFileTypeChosen_DataSourceXslt;

			var filter = App.kstidFileTypeXSLT + "|" + App.kstidFileTypeAllFiles;
			var filename = App.OpenFileDialog("xslt", filter, ref filterIndex,
				Properties.Resources.kstidDataSourceOpenFileXSLTCaption);

			if (filename != null)
			{
				m_project.DataSources[e.RowIndex].XSLTFile = filename;
				m_grid.Refresh();
				Settings.Default.OFD_LastFileTypeChosen_DataSourceXslt = filterIndex;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCustomFields_Click(object sender, EventArgs e)
		{
			using (CustomFieldsDlg dlg = new CustomFieldsDlg(m_project))
			{
				dlg.ShowDialog(this);
				if (dlg.ChangesWereMade)
				{
					m_project.CleanUpMappings();
					m_dirty = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns true if the project contains a data source file with the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ProjectContainsDataSource(string filename)
		{
			foreach (PaDataSource datasource in m_project.DataSources)
			{
				if (datasource.ToString().ToLower() == filename.ToLower())
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns true if the project contains a FW data source with the specified project
		/// and machine name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ProjectContainsFwDataSource(FwDataSourceInfo fwDataSourceInfo)
		{
			string machineName = fwDataSourceInfo.MachineName.ToLower();
			string projectName = fwDataSourceInfo.ProjectName.ToLower();

			foreach (PaDataSource datasource in m_project.DataSources)
			{
				if (datasource.FwDataSourceInfo != null &&
					datasource.FwDataSourceInfo.MachineName != null &&
					datasource.FwDataSourceInfo.MachineName.ToLower() == machineName &&
					datasource.FwDataSourceInfo.ProjectName != null &&
					datasource.FwDataSourceInfo.ProjectName.ToLower() == projectName)
				{
					return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			if (m_newProject)
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0 || e.RowIndex >= m_grid.RowCount ||
				e.ColumnIndex != m_grid.Columns["sourcefiles"].Index)
			{
				return;
			}

			// Draw default everything but text
			DataGridViewPaintParts paintParts = DataGridViewPaintParts.All;
			paintParts &= ~DataGridViewPaintParts.ContentForeground;
			e.Paint(e.ClipBounds, paintParts);

			Color clr = (m_grid.Rows[e.RowIndex].Selected ?
				m_grid.DefaultCellStyle.SelectionForeColor : m_grid.DefaultCellStyle.ForeColor);

			TextFormatFlags flags = TextFormatFlags.VerticalCenter |
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
		private void m_grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
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
		private void m_grid_KeyDown(object sender, KeyEventArgs e)
		{
			if (btnProperties.Enabled && e.KeyCode == Keys.Enter)
			{
				btnProperties.PerformClick();
				e.Handled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkEthnologue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
	}
}