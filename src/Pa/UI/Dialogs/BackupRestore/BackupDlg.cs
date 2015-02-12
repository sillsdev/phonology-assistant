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
	public partial class BackupDlg : Form
	{
		private readonly BackupDlgViewModel _viewModel;
		private readonly Font _boldFont;

		/// ------------------------------------------------------------------------------------
		public BackupDlg()
		{
			InitializeComponent();

			_boldFont = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold);
			_labelProject.Font = FontHelper.UIFont;
			_labelProjectValue.Font = _boldFont; 
			_labelBackupFile.Font = FontHelper.UIFont;
			_labelBackupFileValue.Font = FontHelper.UIFont;
			_textBoxBackupFile.Font = FontHelper.UIFont;

			_groupBoxDestinationFolder.Font = FontHelper.UIFont;
			_radioDefaultFolder.Font = FontHelper.UIFont;
			_labelDefaultFolderValue.Font = FontHelper.UIFont;
			_radioOtherFolder.Font = FontHelper.UIFont;
			_linkOtherFolderValue.Font = FontHelper.UIFont;

			_groupIncludeInBackup.Font = FontHelper.UIFont;
			_checkBoxIncludeDataSources.Font = FontHelper.UIFont;
			_checkBoxIncludeAudioFiles.Font = FontHelper.UIFont;
			
			_linkViewExceptionDetails.Font = FontHelper.UIFont;

			_checkBoxIncludeAudioFiles.Checked = Settings.Default.IncludeAudioFilesInPaBackups;
			_checkBoxIncludeDataSources.Checked = Settings.Default.IncludeDataSourceFilesInPaBackups;

			_radioDefaultFolder.Checked = true;

			if (Settings.Default.LastOtherBackupFolder != null &&
				Directory.Exists(Settings.Default.LastOtherBackupFolder))
			{
				_radioOtherFolder.Checked = Settings.Default.BackupToOtherFolder;
			}
		}

		/// ------------------------------------------------------------------------------------
		public BackupDlg(BackupDlgViewModel viewModel) : this()
		{
			_viewModel = viewModel;
			_labelProjectValue.Text = _viewModel.Project.Name;

			_viewModel.LogBox.Font = FontHelper.UIFont;
			_viewModel.LogBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			_viewModel.LogBox.Margin = new Padding(0);
			_viewModel.LogBox.ReportErrorLinkClicked += delegate { Close(); };
			_tableLayoutPanel.Controls.Add(_viewModel.LogBox, 0, 6);
			_tableLayoutPanel.SetColumnSpan(_viewModel.LogBox, 2);

			_buttonClose.Click += delegate { Close(); };
			_buttonCancel.Click += delegate { _viewModel.Cancel = true; };

			_radioOtherFolder.CheckedChanged += delegate { UpdateDisplay(); };
			_radioDefaultFolder.CheckedChanged += delegate { UpdateDisplay(); };

			var lastTargetBackupFolder = Settings.Default.LastOtherBackupFolder;
			_viewModel.OtherDestFolder =
				(lastTargetBackupFolder != null && Directory.Exists(lastTargetBackupFolder) ? lastTargetBackupFolder : null);

			if (_viewModel.GetAreAllDataSourcesFieldWorks())
			{
				_groupIncludeInBackup.Enabled = false;
				_checkBoxIncludeDataSources.Checked = false;
				_checkBoxIncludeAudioFiles.Checked = false;
			}

			_textBoxBackupFile.Text = Path.GetFileNameWithoutExtension(_viewModel.BackupFile);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				if (_boldFont != null)
					_boldFont.Dispose();

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
			Settings.Default.LastOtherBackupFolder = _viewModel.OtherDestFolder;
			Settings.Default.BackupToOtherFolder = _radioOtherFolder.Checked;

			if (!_viewModel.GetAreAllDataSourcesFieldWorks())
			{
				Settings.Default.IncludeAudioFilesInPaBackups = _checkBoxIncludeAudioFiles.Checked;
				Settings.Default.IncludeDataSourceFilesInPaBackups = _checkBoxIncludeDataSources.Checked;
			}
			
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFileNameTextChanged(object sender, EventArgs e)
		{
			_viewModel.SetBackupFile(_textBoxBackupFile.Text);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOtherFolderValueLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var description = string.Format(LocalizationManager.GetString(
					"DialogBoxes.BackupDlg.ChangeFolderBrowserDlgDescription",
					"Specify the folder where the backup file will be written for the '{0}' project."),
					_viewModel.Project.Name);

			if (_viewModel.SpecifyTargetFolder(this, description, null))
			{
				if (_viewModel.OtherDestFolder == _viewModel.DefaultBackupFolder)
					TellUserHeSelectedTheDefaultFolder();
				else
					UpdateDisplay();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void TellUserHeSelectedTheDefaultFolder()
		{
			var msg = LocalizationManager.GetString(
				"DialogBoxes.BackupDlg.OtherFolderLinkText.WhenEnabledAndIsSameAsDefaultFolder",
				"(specified folder is the same as the default)");

			_viewModel.OtherDestFolder = null;
			_linkOtherFolderValue.Links.Clear();
			_linkOtherFolderValue.Text = msg;
			_linkOtherFolderValue.LinkColor = Color.Red;
			_linkOtherFolderValue.LinkArea = new LinkArea(0, _linkOtherFolderValue.Text.Length);
			_buttonBackup.Enabled = false;

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
		private void UpdateDisplay()
		{
			_labelDefaultFolderValue.Enabled = _radioDefaultFolder.Checked;
			_labelDefaultFolderValue.Text = _viewModel.DefaultBackupFolder;

			UpdateOtherFolderValueLink();
			UpdateBackupFileValueLabel();

			_buttonBackup.Enabled = (_viewModel.GetIsBackupFileNameValid() && !_viewModel.GetDoesBackupAlreadyExist() &&
				(_radioDefaultFolder.Checked || (_radioOtherFolder.Checked && Directory.Exists(_linkOtherFolderValue.Text))));
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateOtherFolderValueLink()
		{
			if (_viewModel.OtherDestFolder != null)
				_linkOtherFolderValue.Text = _viewModel.OtherDestFolder;
			else if (_radioOtherFolder.Checked)
			{
				_linkOtherFolderValue.Text = LocalizationManager.GetString(
					"DialogBoxes.BackupDlg.OtherFolderLinkText.WhenNotSpecifiedAndEnabled",
					"(click to specify)");
			}
			else
			{
				_linkOtherFolderValue.Text = LocalizationManager.GetString(
					"DialogBoxes.BackupDlg.OtherFolderLinkText.WhenNotSpecifiedAndNotEnabled",
					"(not specified)");
			}

			_linkOtherFolderValue.Enabled = _radioOtherFolder.Checked;
			_linkOtherFolderValue.LinkColor = _linkViewExceptionDetails.LinkColor;
			_linkOtherFolderValue.Links.Clear();
			_linkOtherFolderValue.LinkArea = new LinkArea(0, _linkOtherFolderValue.Text.Length);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateBackupFileValueLabel()
		{
			if (!_viewModel.GetIsBackupFileNameValid())
			{
				_labelBackupFileValue.Text = LocalizationManager.GetString("DialogBoxes.BackupDlg.InvalidBackupFileNameMSg", "Invalid file name!");
				_labelBackupFileValue.ForeColor = Color.Red;
			}
			else if (_viewModel.GetDoesBackupAlreadyExist())
			{
				_labelBackupFileValue.Text = LocalizationManager.GetString("DialogBoxes.BackupDlg.BackupFileAlreadyExistsMsg", "File already exists!");
				_labelBackupFileValue.ForeColor = Color.Red;
			}
			else
			{
				_labelBackupFileValue.Text = _viewModel.BackupFile;
				_labelBackupFileValue.ForeColor = Color.DarkSlateGray;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBackupButtonClick(object sender, EventArgs e)
		{
			_textBoxBackupFile.Enabled = false;
			_groupBoxDestinationFolder.Enabled = false;
			_groupIncludeInBackup.Enabled = false;
			_buttonBackup.Visible = false;
			_buttonClose.Visible = false;
			_buttonCancel.Visible = true;
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
			_linkViewExceptionDetails.Visible = true;
			_linkViewExceptionDetails.LinkClicked += delegate
			{
				ErrorReport.ReportNonFatalExceptionWithMessage(_viewModel.BackupRestoreException,
					LocalizationManager.GetString("DialogBoxes.BackupDlg.BackupErrorMsg",
					"There was an error while backing up the '{0}' project."),
					_viewModel.Project.Name);
			};
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
