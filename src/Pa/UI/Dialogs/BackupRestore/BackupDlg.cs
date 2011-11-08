using System;
using System.Windows.Forms;
using Palaso.Reporting;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class BackupDlg : Form
	{
		private readonly BackupDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public BackupDlg()
		{
			InitializeComponent();

			_labelProject.Font = FontHelper.UIFont;
			_labelProjectValue.Font = FontHelper.UIFont;
			_labelBackupFolder.Font = FontHelper.UIFont;
			_labelBackupFile.Font = FontHelper.UIFont;
			_labelBackupFileValue.Font = FontHelper.UIFont;
			_labelBackupFolderValue.Font = FontHelper.UIFont;
			_checkBoxIncludeDataSources.Font = FontHelper.UIFont;
			_checkBoxIncludeAudioFiles.Font = FontHelper.UIFont;

			_checkBoxIncludeAudioFiles.Checked = Settings.Default.IncludeAudioFilesInPaBackups;
			_checkBoxIncludeDataSources.Checked = Settings.Default.IncludeDataSourceFilesInPaBackups;
		}

		/// ------------------------------------------------------------------------------------
		public BackupDlg(BackupDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_labelProjectValue.Text = _viewModel.Project.Name;
			_labelBackupFolderValue.Text = _viewModel.TargetFolder;
			_labelBackupFileValue.Text = _viewModel.BackupFile;

			_viewModel.LogBox.Font = FontHelper.UIFont;
			_viewModel.LogBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			_viewModel.LogBox.Margin = new Padding(0);
			_viewModel.LogBox.ReportErrorLinkClicked += delegate { Close(); };
			_tableLayoutPanel.Controls.Add(_viewModel.LogBox, 0, 5);
			_tableLayoutPanel.SetColumnSpan(_viewModel.LogBox, 2);

			_buttonClose.Click += delegate { Close(); };
			_buttonCancel.Click += delegate { _viewModel.Cancel = true; };

			if (_viewModel.GetAreAllDataSourcesFieldWorks())
			{
				_checkBoxIncludeDataSources.Enabled = false;
				_checkBoxIncludeAudioFiles.Enabled = false;
				_checkBoxIncludeDataSources.Checked = false;
				_checkBoxIncludeAudioFiles.Checked = false;
			}
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
			Settings.Default.BackupDlg = App.InitializeForm(this, Settings.Default.BackupDlg);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			Settings.Default.LastBackupFolder = _viewModel.TargetFolder;
			
			if (_checkBoxIncludeDataSources.Enabled)
			{
				Settings.Default.IncludeAudioFilesInPaBackups = _checkBoxIncludeAudioFiles.Checked;
				Settings.Default.IncludeDataSourceFilesInPaBackups = _checkBoxIncludeDataSources.Checked;
			}
			
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleChangeFolderButtonClick(object sender, EventArgs e)
		{
			var description = string.Format(App.GetString(
					"DialogBoxes.BackupDlg.ChangeFolderBrowserDlgDescription",
					"Specify the folder where the backup file will be written for the '{0}' project."),
					_viewModel.Project.Name);

			if (_viewModel.SpecifyTargetFolder(this, description, null))
				_labelBackupFolderValue.Text = _viewModel.TargetFolder;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBackupButtonClick(object sender, EventArgs e)
		{
			_buttonBackup.Enabled = false;
			_buttonChangeFolder.Enabled = false;
			_buttonCancel.Visible = true;
			_buttonClose.Visible = false;
			_progressBar.Visible = true;
			_progressBar.Maximum = _viewModel.GetNumberOfFilesToBackup(
				_checkBoxIncludeDataSources.Checked, _checkBoxIncludeAudioFiles.Checked);

			_viewModel.Backup(_checkBoxIncludeDataSources.Checked,
				_checkBoxIncludeAudioFiles.Checked,
				pct => Invoke((Action)(() => _progressBar.Value = pct)), HandleBackupComplete);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBackupComplete()
		{
			_buttonCancel.Visible = false;
			_buttonClose.Visible = true;
			_progressBar.Visible = (!_viewModel.Cancel && _viewModel.BackupRestoreException == null);

			if (_viewModel.BackupRestoreException == null)
				return;

			_progressBar.Visible = false;
			_buttonSeeErrorDetails.Visible = true;
			_buttonSeeErrorDetails.Click += delegate
			{
				ErrorReport.ReportNonFatalExceptionWithMessage(_viewModel.BackupRestoreException,
					App.GetString("DialogBoxes.BackupDlg.BackupErrorMsg",
					"There was an error while backing up the '{0}' project."),
					_viewModel.Project.Name);
			};
		}
	}
}
