using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class RestoreDlg : Form
	{
		private readonly RestoreDlgViewModel _viewModel;

		public string RestoredProjectFileName { get; private set; }

		/// ------------------------------------------------------------------------------------
		public RestoreDlg()
		{
			InitializeComponent();

			DialogResult = DialogResult.None;

			_labelBackupFilesFound.Font = FontHelper.UIFont;
			_linkSelectOtherBackupFile.Font = FontHelper.UIFont;
			_radioDefaultFolder.Font = FontHelper.UIFont;
			_radioOtherFolder.Font = FontHelper.UIFont;
			_linkOtherFolderValue.Font = FontHelper.UIFont;
			_labelDefaultFolderValue.Font = FontHelper.UIFont;
			_linkViewExceptionDetails.Font = FontHelper.UIFont;
			_groupBoxDestinationFolder.Font = FontHelper.UIFont;

			_grid.AutoResizeColumnHeadersHeight();
			_grid.ColumnHeadersHeight += 4;

			_radioDefaultFolder.Checked = true;

			if (Settings.Default.LastOtherRestoreFolder != null &&
				Directory.Exists(Settings.Default.LastOtherRestoreFolder))
			{
				_radioOtherFolder.Checked = Settings.Default.RestoreToOtherFolder;
			}
		}

		/// ------------------------------------------------------------------------------------
		public RestoreDlg(RestoreDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_viewModel.LogBox.Font = FontHelper.UIFont;
			_viewModel.LogBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			_viewModel.LogBox.Margin = new Padding(0);
			_viewModel.LogBox.ReportErrorLinkClicked += delegate { Close(); };
			_tableLayoutPanel.Controls.Add(_viewModel.LogBox, 0, 3);
			_tableLayoutPanel.SetColumnSpan(_viewModel.LogBox, 2);

			_buttonClose.Click += delegate { Close(); };
			_buttonCancel.Click += delegate { _viewModel.Cancel = true; };

			_radioOtherFolder.CheckedChanged += delegate { UpdateDisplay(); };
			_radioDefaultFolder.CheckedChanged += delegate { UpdateDisplay(); };

			var lastTargetRestoreFolder = Settings.Default.LastOtherRestoreFolder;
			_viewModel.OtherDestFolder =
				(lastTargetRestoreFolder != null && Directory.Exists(lastTargetRestoreFolder) ? lastTargetRestoreFolder : null);

			LoadGrid();
			HandleGridCurrentRowChanged(null, null);
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
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.RestoreDlg = App.InitializeForm(this, Settings.Default.RestoreDlg);
			base.OnLoad(e);
			BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Settings.Default.LastOtherRestoreFolder = _viewModel.OtherDestFolder;
			Settings.Default.RestoreToOtherFolder = _radioOtherFolder.Checked;

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			_grid.Rows.Clear();

			foreach (var backupInfo in _viewModel.AvailableBackups)
			{
				int rowIndex = _grid.Rows.Add(backupInfo.Key, Path.GetFileName(backupInfo.Value));
				_grid[1, rowIndex].ToolTipText = backupInfo.Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSelectOtherBackupFileLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var caption = LocalizationManager.GetString(
				"DialogBoxes.RestoreDlg.SelectOtherBackupFileDlg.Caption", "Spelect Backup File");

			var paBackupfilterString = LocalizationManager.GetString(
				"DialogBoxes.RestoreDlg.SelectOtherBackupFileDlg.BackupFileTypeText", "Phonology Assistant Backup");

			var filters = paBackupfilterString + " (*.pabackup)|*.pabackup|" + App.kstidFileTypeAllFiles;

			var backupFile = App.OpenFileDialog("pabackup", filters, caption);

			if (backupFile == null || !RestoreDlgViewModel.GetIsValidBackupFile(backupFile, true))
				return;

			_viewModel.AddBackupFileToListAndMakeCurrent(backupFile);
			LoadGrid();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOtherFolderValueLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var description = string.Format(LocalizationManager.GetString(
				"DialogBoxes.RestoreDlg.ChangeFolderBrowserDlg.Description",
				"Specify the folder where the '{0}' project will be restored."),
				_grid.CurrentRow.Cells[0].Value as string);

			var folderContainsProjectMsg = LocalizationManager.GetString(
				"DialogBoxes.RestoreDlg.ChangeFolderBrowserDlg.FolderAlreadyContainsProjectMsg",
				"The folder you selected already contains a Phonology Assistant project. Please select a folder that does not contain a project.");

			if (_viewModel.SpecifyTargetFolder(this, description, folderContainsProjectMsg))
			{
				if (_viewModel.OtherDestFolder == _viewModel.SuggestedDestFolder)
					TellUserHeSelectedTheDefaultFolder();
				else
					UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void TellUserHeSelectedTheDefaultFolder()
		{
			var msg = LocalizationManager.GetString(
				"DialogBoxes.RestoreDlg.OtherFolderLinkText.WhenEnabledAndIsSameAsDefaultFolder",
				"(specified folder is the same as the default)");

			_viewModel.OtherDestFolder = null;
			_linkOtherFolderValue.Links.Clear();
			_linkOtherFolderValue.Text = msg;
			_linkOtherFolderValue.LinkColor = Color.Red;
			_linkOtherFolderValue.LinkArea = new LinkArea(0, _linkOtherFolderValue.Text.Length);
			_buttonRestore.Enabled = false;

			var timer = new Timer();
			timer.Interval = 3000;
			timer.Tick += delegate
			{
				timer.Stop();
				timer.Dispose();
				UpdateDisplay();
			};

			timer.Start();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCurrentRowChanged(object sender, EventArgs e)
		{
			_viewModel.SetCurrentBackupFile(_grid.CurrentCellAddress.Y);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridPainting(object sender, PaintEventArgs e)
		{
			if (_grid.RowCount > 0)
				return;

			var msg = LocalizationManager.GetString("DialogBoxes.RestoreDlg.SelectBackupFilesPromptInEmptyList",
				"No backup files were found.\nClick '{0}'\nto specify a backup file.");

			_grid.DrawMessageInCenterOfGrid(e.Graphics,
				string.Format(msg, _linkSelectOtherBackupFile.Text.TrimEnd('.')),
				FontHelper.UIFont, 0);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_labelDefaultFolderValue.Enabled = _radioDefaultFolder.Checked;
			_labelDefaultFolderValue.Text = _viewModel.SuggestedDestFolder;

			UpdateOtherFolderValueLink();
	
			_buttonRestore.Enabled = (_viewModel.AvailableBackups.Count > 0 && (_radioDefaultFolder.Checked ||
				(_radioOtherFolder.Checked && Directory.Exists(_linkOtherFolderValue.Text))));

			_groupBoxDestinationFolder.Enabled = (_viewModel.AvailableBackups.Count > 0);
		}
		
		/// ------------------------------------------------------------------------------------
		private void UpdateOtherFolderValueLink()
		{
			if (_viewModel.OtherDestFolder != null)
				_linkOtherFolderValue.Text = _viewModel.OtherDestFolder;
			else if (_radioOtherFolder.Checked)
			{
				_linkOtherFolderValue.Text = LocalizationManager.GetString(
					"DialogBoxes.RestoreDlg.OtherFolderLinkText.WhenNotSpecifiedAndEnabled",
					"(click to specify)");
			}
			else
			{
				_linkOtherFolderValue.Text = LocalizationManager.GetString(
					"DialogBoxes.RestoreDlg.OtherFolderLinkText.WhenNotSpecifiedAndNotEnabled",
					"(not specified)");
			}

			_linkOtherFolderValue.Enabled = _radioOtherFolder.Checked;
			_linkOtherFolderValue.LinkColor = _linkViewExceptionDetails.LinkColor;
			_linkOtherFolderValue.Links.Clear();
			_linkOtherFolderValue.LinkArea = new LinkArea(0, _linkOtherFolderValue.Text.Length);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRestoreButtonClick(object sender, EventArgs e)
		{
			_linkSelectOtherBackupFile.Enabled = false;
			_grid.Enabled = false;
			_groupBoxDestinationFolder.Enabled = false;
			_buttonRestore.Visible = false;
			_buttonClose.Visible = false;
			_buttonCancel.Visible = true;
			_progressBar.Visible = true;
			_progressBar.Maximum = _viewModel.GetNumberOfFilesToRestore();

			_viewModel.Restore(pct => Invoke((Action)(() => _progressBar.Increment(1))),
				HandleRestoreComplete);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRestoreComplete()
		{
			_buttonCancel.Visible = false;
			_buttonClose.Visible = true;
			_buttonLoadProject.Visible = (!_viewModel.Cancel && _viewModel.BackupRestoreException == null);
			_progressBar.Visible = (!_viewModel.Cancel && _viewModel.BackupRestoreException == null);

			if (_viewModel.BackupRestoreException == null)
				return;

			_progressBar.Visible = false;
			_linkViewExceptionDetails.Visible = true;
			_linkViewExceptionDetails.LinkClicked += delegate
			{
				ErrorReport.ReportNonFatalExceptionWithMessage(_viewModel.BackupRestoreException,
					LocalizationManager.GetString("DialogBoxes.RestoreDlg.RestoreErrorMsg",
					"There was an error while restoring the '{0}' project."),
					_viewModel.NameOfProjectToRestore);
			};
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLoadProjectButtonClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			RestoredProjectFileName = Path.Combine(_viewModel.OtherDestFolder ??
				_viewModel.SuggestedDestFolder, _viewModel.CurrentProjectFileName);
			
			Close();
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
	}
}
