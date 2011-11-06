namespace SIL.Pa.UI.Dialogs
{
	partial class BackupDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._buttonBackup = new System.Windows.Forms.Button();
			this._checkBoxIncludeDataSources = new SilTools.Controls.AutoHeightCheckBox();
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._buttonClose = new System.Windows.Forms.Button();
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._labelProjectValue = new System.Windows.Forms.Label();
			this._labelProject = new System.Windows.Forms.Label();
			this._checkBoxIncludeAudioFiles = new SilTools.Controls.AutoHeightCheckBox();
			this._labelBackupFileValue = new System.Windows.Forms.Label();
			this._labelBackupFile = new System.Windows.Forms.Label();
			this._labelBackupFolder = new System.Windows.Forms.Label();
			this._labelBackupFolderValue = new System.Windows.Forms.Label();
			this._buttonSeeErrorDetails = new System.Windows.Forms.Button();
			this._buttonChangeFolder = new System.Windows.Forms.Button();
			this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayoutPanel.SuspendLayout();
			this._tableLayoutButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _buttonBackup
			// 
			this._buttonBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonBackup.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonBackup, null);
			this.locExtender.SetLocalizationComment(this._buttonBackup, null);
			this.locExtender.SetLocalizingId(this._buttonBackup, "BackupDlg._buttonBackup");
			this._buttonBackup.Location = new System.Drawing.Point(117, 10);
			this._buttonBackup.Margin = new System.Windows.Forms.Padding(0, 10, 3, 0);
			this._buttonBackup.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonBackup.Name = "_buttonBackup";
			this._buttonBackup.Size = new System.Drawing.Size(75, 26);
			this._buttonBackup.TabIndex = 0;
			this._buttonBackup.Text = "Backup";
			this._buttonBackup.UseVisualStyleBackColor = true;
			this._buttonBackup.Click += new System.EventHandler(this.HandleBackupButtonClick);
			// 
			// _checkBoxIncludeDataSources
			// 
			this._checkBoxIncludeDataSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._checkBoxIncludeDataSources.AutoSize = true;
			this._checkBoxIncludeDataSources.BackColor = System.Drawing.Color.Transparent;
			this._checkBoxIncludeDataSources.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBoxIncludeDataSources.Checked = true;
			this._checkBoxIncludeDataSources.CheckState = System.Windows.Forms.CheckState.Checked;
			this._tableLayoutPanel.SetColumnSpan(this._checkBoxIncludeDataSources, 2);
			this.locExtender.SetLocalizableToolTip(this._checkBoxIncludeDataSources, null);
			this.locExtender.SetLocalizationComment(this._checkBoxIncludeDataSources, null);
			this.locExtender.SetLocalizingId(this._checkBoxIncludeDataSources, "BackupDlg._checkBoxIncludeDataSources");
			this._checkBoxIncludeDataSources.Location = new System.Drawing.Point(2, 75);
			this._checkBoxIncludeDataSources.Margin = new System.Windows.Forms.Padding(2, 20, 0, 8);
			this._checkBoxIncludeDataSources.Name = "_checkBoxIncludeDataSources";
			this._checkBoxIncludeDataSources.Size = new System.Drawing.Size(349, 30);
			this._checkBoxIncludeDataSources.TabIndex = 2;
			this._checkBoxIncludeDataSources.Text = "&Include data source files in backup\r\n(does not apply to FieldWorks data sources)" +
    "";
			this._checkBoxIncludeDataSources.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBoxIncludeDataSources.UseVisualStyleBackColor = false;
			// 
			// _progressBar
			// 
			this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutPanel.SetColumnSpan(this._progressBar, 2);
			this._progressBar.Location = new System.Drawing.Point(0, 361);
			this._progressBar.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._progressBar.Name = "_progressBar";
			this._progressBar.Size = new System.Drawing.Size(351, 18);
			this._progressBar.TabIndex = 3;
			this._progressBar.Visible = false;
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "BackupDlg._buttonCancel");
			this._buttonCancel.Location = new System.Drawing.Point(198, 10);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 10, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 5;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			this._buttonCancel.Visible = false;
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonClose.AutoSize = true;
			this._buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._buttonClose, null);
			this.locExtender.SetLocalizationComment(this._buttonClose, null);
			this.locExtender.SetLocalizingId(this._buttonClose, "BackupDlg._buttonClose");
			this._buttonClose.Location = new System.Drawing.Point(276, 10);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(3, 10, 0, 0);
			this._buttonClose.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 6;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.AutoSize = true;
			this._tableLayoutPanel.ColumnCount = 2;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this._labelProjectValue, 1, 0);
			this._tableLayoutPanel.Controls.Add(this._labelProject, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._checkBoxIncludeAudioFiles, 0, 4);
			this._tableLayoutPanel.Controls.Add(this._labelBackupFileValue, 1, 2);
			this._tableLayoutPanel.Controls.Add(this._labelBackupFile, 0, 2);
			this._tableLayoutPanel.Controls.Add(this._labelBackupFolder, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._checkBoxIncludeDataSources, 0, 3);
			this._tableLayoutPanel.Controls.Add(this._labelBackupFolderValue, 1, 1);
			this._tableLayoutPanel.Controls.Add(this._buttonSeeErrorDetails, 1, 6);
			this._tableLayoutPanel.Controls.Add(this._progressBar, 0, 7);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 8;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(351, 379);
			this._tableLayoutPanel.TabIndex = 7;
			// 
			// _labelProjectValue
			// 
			this._labelProjectValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelProjectValue.AutoSize = true;
			this._labelProjectValue.ForeColor = System.Drawing.Color.DarkBlue;
			this.locExtender.SetLocalizableToolTip(this._labelProjectValue, null);
			this.locExtender.SetLocalizationComment(this._labelProjectValue, null);
			this.locExtender.SetLocalizationPriority(this._labelProjectValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelProjectValue, "BackupDlg._labelProjectValue");
			this._labelProjectValue.Location = new System.Drawing.Point(82, 0);
			this._labelProjectValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 8);
			this._labelProjectValue.Name = "_labelProjectValue";
			this._labelProjectValue.Size = new System.Drawing.Size(269, 13);
			this._labelProjectValue.TabIndex = 14;
			this._labelProjectValue.Text = "#";
			// 
			// _labelProject
			// 
			this._labelProject.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelProject, null);
			this.locExtender.SetLocalizationComment(this._labelProject, null);
			this.locExtender.SetLocalizingId(this._labelProject, "BackupDlg._labelProject");
			this._labelProject.Location = new System.Drawing.Point(0, 0);
			this._labelProject.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this._labelProject.Name = "_labelProject";
			this._labelProject.Size = new System.Drawing.Size(43, 13);
			this._labelProject.TabIndex = 13;
			this._labelProject.Text = "Project:";
			// 
			// _checkBoxIncludeAudioFiles
			// 
			this._checkBoxIncludeAudioFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._checkBoxIncludeAudioFiles.AutoSize = true;
			this._checkBoxIncludeAudioFiles.BackColor = System.Drawing.Color.Transparent;
			this._checkBoxIncludeAudioFiles.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBoxIncludeAudioFiles.Checked = true;
			this._checkBoxIncludeAudioFiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this._tableLayoutPanel.SetColumnSpan(this._checkBoxIncludeAudioFiles, 2);
			this.locExtender.SetLocalizableToolTip(this._checkBoxIncludeAudioFiles, null);
			this.locExtender.SetLocalizationComment(this._checkBoxIncludeAudioFiles, null);
			this.locExtender.SetLocalizingId(this._checkBoxIncludeAudioFiles, "BackupDlg._checkBoxIncludeAudioFiles");
			this._checkBoxIncludeAudioFiles.Location = new System.Drawing.Point(2, 113);
			this._checkBoxIncludeAudioFiles.Margin = new System.Windows.Forms.Padding(2, 0, 0, 10);
			this._checkBoxIncludeAudioFiles.Name = "_checkBoxIncludeAudioFiles";
			this._checkBoxIncludeAudioFiles.Size = new System.Drawing.Size(349, 30);
			this._checkBoxIncludeAudioFiles.TabIndex = 12;
			this._checkBoxIncludeAudioFiles.Text = "&Include audio files in backup\r\n(does not apply to FieldWorks data sources)";
			this._checkBoxIncludeAudioFiles.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBoxIncludeAudioFiles.UseVisualStyleBackColor = false;
			// 
			// _labelBackupFileValue
			// 
			this._labelBackupFileValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelBackupFileValue.AutoSize = true;
			this._labelBackupFileValue.ForeColor = System.Drawing.Color.DarkBlue;
			this.locExtender.SetLocalizableToolTip(this._labelBackupFileValue, null);
			this.locExtender.SetLocalizationComment(this._labelBackupFileValue, null);
			this.locExtender.SetLocalizationPriority(this._labelBackupFileValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelBackupFileValue, "BackupDlg._labelBackupFileValue");
			this._labelBackupFileValue.Location = new System.Drawing.Point(82, 42);
			this._labelBackupFileValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._labelBackupFileValue.Name = "_labelBackupFileValue";
			this._labelBackupFileValue.Size = new System.Drawing.Size(269, 13);
			this._labelBackupFileValue.TabIndex = 11;
			this._labelBackupFileValue.Text = "#";
			// 
			// _labelBackupFile
			// 
			this._labelBackupFile.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelBackupFile, null);
			this.locExtender.SetLocalizationComment(this._labelBackupFile, null);
			this.locExtender.SetLocalizingId(this._labelBackupFile, "BackupDlg._labelBackupFile");
			this._labelBackupFile.Location = new System.Drawing.Point(0, 42);
			this._labelBackupFile.Margin = new System.Windows.Forms.Padding(0);
			this._labelBackupFile.Name = "_labelBackupFile";
			this._labelBackupFile.Size = new System.Drawing.Size(66, 13);
			this._labelBackupFile.TabIndex = 9;
			this._labelBackupFile.Text = "Backup File:";
			// 
			// _labelBackupFolder
			// 
			this._labelBackupFolder.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelBackupFolder, null);
			this.locExtender.SetLocalizationComment(this._labelBackupFolder, null);
			this.locExtender.SetLocalizingId(this._labelBackupFolder, "BackupDlg._labelBackupFolder");
			this._labelBackupFolder.Location = new System.Drawing.Point(0, 21);
			this._labelBackupFolder.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this._labelBackupFolder.Name = "_labelBackupFolder";
			this._labelBackupFolder.Size = new System.Drawing.Size(79, 13);
			this._labelBackupFolder.TabIndex = 8;
			this._labelBackupFolder.Text = "Backup Folder:";
			// 
			// _labelBackupFolderValue
			// 
			this._labelBackupFolderValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelBackupFolderValue.AutoSize = true;
			this._labelBackupFolderValue.ForeColor = System.Drawing.Color.DarkBlue;
			this.locExtender.SetLocalizableToolTip(this._labelBackupFolderValue, null);
			this.locExtender.SetLocalizationComment(this._labelBackupFolderValue, null);
			this.locExtender.SetLocalizationPriority(this._labelBackupFolderValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelBackupFolderValue, "BackupDlg._labelBackupFolderValue");
			this._labelBackupFolderValue.Location = new System.Drawing.Point(82, 21);
			this._labelBackupFolderValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 8);
			this._labelBackupFolderValue.Name = "_labelBackupFolderValue";
			this._labelBackupFolderValue.Size = new System.Drawing.Size(269, 13);
			this._labelBackupFolderValue.TabIndex = 0;
			this._labelBackupFolderValue.Text = "#";
			// 
			// _buttonSeeErrorDetails
			// 
			this._buttonSeeErrorDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonSeeErrorDetails.AutoSize = true;
			this._buttonSeeErrorDetails.Image = global::SIL.Pa.Properties.Resources.kimidWarning;
			this._buttonSeeErrorDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.locExtender.SetLocalizableToolTip(this._buttonSeeErrorDetails, null);
			this.locExtender.SetLocalizationComment(this._buttonSeeErrorDetails, null);
			this.locExtender.SetLocalizingId(this._buttonSeeErrorDetails, "BackupDlg._buttonSeeErrorDetails");
			this._buttonSeeErrorDetails.Location = new System.Drawing.Point(196, 322);
			this._buttonSeeErrorDetails.Margin = new System.Windows.Forms.Padding(0, 10, 0, 3);
			this._buttonSeeErrorDetails.MinimumSize = new System.Drawing.Size(155, 26);
			this._buttonSeeErrorDetails.Name = "_buttonSeeErrorDetails";
			this._buttonSeeErrorDetails.Size = new System.Drawing.Size(155, 26);
			this._buttonSeeErrorDetails.TabIndex = 16;
			this._buttonSeeErrorDetails.Text = "View Exception Details...";
			this._buttonSeeErrorDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonSeeErrorDetails.UseVisualStyleBackColor = true;
			this._buttonSeeErrorDetails.Visible = false;
			// 
			// _buttonChangeFolder
			// 
			this._buttonChangeFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._buttonChangeFolder.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonChangeFolder, null);
			this.locExtender.SetLocalizationComment(this._buttonChangeFolder, null);
			this.locExtender.SetLocalizingId(this._buttonChangeFolder, "BackupDlg._buttonChangeFolder");
			this._buttonChangeFolder.Location = new System.Drawing.Point(0, 10);
			this._buttonChangeFolder.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._buttonChangeFolder.MinimumSize = new System.Drawing.Size(115, 26);
			this._buttonChangeFolder.Name = "_buttonChangeFolder";
			this._buttonChangeFolder.Size = new System.Drawing.Size(117, 26);
			this._buttonChangeFolder.TabIndex = 10;
			this._buttonChangeFolder.Text = "Change Backup Folder...";
			this._buttonChangeFolder.UseVisualStyleBackColor = true;
			this._buttonChangeFolder.Click += new System.EventHandler(this.HandleChangeFolderButtonClick);
			// 
			// _tableLayoutButtons
			// 
			this._tableLayoutButtons.AutoSize = true;
			this._tableLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutButtons.ColumnCount = 4;
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.Controls.Add(this._buttonChangeFolder, 0, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonBackup, 1, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonCancel, 2, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonClose, 3, 0);
			this._tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._tableLayoutButtons.Location = new System.Drawing.Point(15, 394);
			this._tableLayoutButtons.Name = "_tableLayoutButtons";
			this._tableLayoutButtons.RowCount = 1;
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutButtons.Size = new System.Drawing.Size(351, 36);
			this._tableLayoutButtons.TabIndex = 8;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// BackupDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(381, 445);
			this.Controls.Add(this._tableLayoutPanel);
			this.Controls.Add(this._tableLayoutButtons);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "BackupDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(345, 415);
			this.Name = "BackupDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Backup";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this._tableLayoutButtons.ResumeLayout(false);
			this._tableLayoutButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _buttonBackup;
		private SilTools.Controls.AutoHeightCheckBox _checkBoxIncludeDataSources;
		private System.Windows.Forms.ProgressBar _progressBar;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.Label _labelBackupFile;
		private System.Windows.Forms.Label _labelBackupFolder;
		private System.Windows.Forms.Button _buttonChangeFolder;
		private SilTools.Controls.AutoHeightCheckBox _checkBoxIncludeAudioFiles;
		private System.Windows.Forms.Label _labelBackupFileValue;
		private System.Windows.Forms.Label _labelBackupFolderValue;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutButtons;
		private System.Windows.Forms.Label _labelProjectValue;
		private System.Windows.Forms.Label _labelProject;
		protected Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Button _buttonSeeErrorDetails;
	}
}