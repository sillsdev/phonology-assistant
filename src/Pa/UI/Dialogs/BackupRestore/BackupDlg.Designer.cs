using System.Windows.Forms;

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
			this._checkBoxIncludeDataSources = new System.Windows.Forms.CheckBox();
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._buttonClose = new System.Windows.Forms.Button();
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._groupBoxDestinationFolder = new System.Windows.Forms.GroupBox();
			this._tableLayoutDestinationFolder = new System.Windows.Forms.TableLayoutPanel();
			this._labelDefaultFolderValue = new System.Windows.Forms.Label();
			this._radioDefaultFolder = new System.Windows.Forms.RadioButton();
			this._linkOtherFolderValue = new System.Windows.Forms.LinkLabel();
			this._radioOtherFolder = new System.Windows.Forms.RadioButton();
			this._groupIncludeInBackup = new System.Windows.Forms.GroupBox();
			this._tableLayoutIncludeInBackup = new System.Windows.Forms.TableLayoutPanel();
			this._checkBoxIncludeAudioFiles = new System.Windows.Forms.CheckBox();
			this._labelProjectValue = new System.Windows.Forms.Label();
			this._labelProject = new System.Windows.Forms.Label();
			this._linkViewExceptionDetails = new System.Windows.Forms.LinkLabel();
			this._labelBackupFile = new System.Windows.Forms.Label();
			this._textBoxBackupFile = new System.Windows.Forms.TextBox();
			this._labelBackupFileValue = new System.Windows.Forms.Label();
			this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayoutPanel.SuspendLayout();
			this._groupBoxDestinationFolder.SuspendLayout();
			this._tableLayoutDestinationFolder.SuspendLayout();
			this._groupIncludeInBackup.SuspendLayout();
			this._tableLayoutIncludeInBackup.SuspendLayout();
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
			this.locExtender.SetLocalizingId(this._buttonBackup, "DialogBoxes.BackupDlg.BackupButton");
			this._buttonBackup.Location = new System.Drawing.Point(123, 10);
			this._buttonBackup.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
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
			this.locExtender.SetLocalizableToolTip(this._checkBoxIncludeDataSources, null);
			this.locExtender.SetLocalizationComment(this._checkBoxIncludeDataSources, null);
			this.locExtender.SetLocalizingId(this._checkBoxIncludeDataSources, "DialogBoxes.BackupDlg.IncludeDataSourcesCheckbox");
			this._checkBoxIncludeDataSources.Location = new System.Drawing.Point(2, 0);
			this._checkBoxIncludeDataSources.Margin = new System.Windows.Forms.Padding(2, 0, 0, 8);
			this._checkBoxIncludeDataSources.Name = "_checkBoxIncludeDataSources";
			this._checkBoxIncludeDataSources.Size = new System.Drawing.Size(338, 17);
			this._checkBoxIncludeDataSources.TabIndex = 0;
			this._checkBoxIncludeDataSources.Text = "Include data source files in backup";
			this._checkBoxIncludeDataSources.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBoxIncludeDataSources.UseVisualStyleBackColor = false;
			// 
			// _progressBar
			// 
			this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutPanel.SetColumnSpan(this._progressBar, 2);
			this._progressBar.Location = new System.Drawing.Point(0, 386);
			this._progressBar.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._progressBar.Name = "_progressBar";
			this._progressBar.Size = new System.Drawing.Size(360, 18);
			this._progressBar.TabIndex = 8;
			this._progressBar.Visible = false;
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.BackupDlg.CancelButton");
			this._buttonCancel.Location = new System.Drawing.Point(204, 10);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(6, 10, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 1;
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
			this.locExtender.SetLocalizingId(this._buttonClose, "DialogBoxes.BackupDlg.CloseButton");
			this._buttonClose.Location = new System.Drawing.Point(285, 10);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(6, 10, 0, 0);
			this._buttonClose.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 2;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.AutoSize = true;
			this._tableLayoutPanel.ColumnCount = 2;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this._groupBoxDestinationFolder, 0, 3);
			this._tableLayoutPanel.Controls.Add(this._groupIncludeInBackup, 0, 5);
			this._tableLayoutPanel.Controls.Add(this._labelProjectValue, 1, 0);
			this._tableLayoutPanel.Controls.Add(this._labelProject, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._progressBar, 0, 8);
			this._tableLayoutPanel.Controls.Add(this._linkViewExceptionDetails, 1, 7);
			this._tableLayoutPanel.Controls.Add(this._labelBackupFile, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._textBoxBackupFile, 1, 1);
			this._tableLayoutPanel.Controls.Add(this._labelBackupFileValue, 1, 2);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 9;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(360, 404);
			this._tableLayoutPanel.TabIndex = 0;
			// 
			// _groupBoxDestinationFolder
			// 
			this._groupBoxDestinationFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._groupBoxDestinationFolder.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._groupBoxDestinationFolder, 2);
			this._groupBoxDestinationFolder.Controls.Add(this._tableLayoutDestinationFolder);
			this.locExtender.SetLocalizableToolTip(this._groupBoxDestinationFolder, null);
			this.locExtender.SetLocalizationComment(this._groupBoxDestinationFolder, null);
			this.locExtender.SetLocalizingId(this._groupBoxDestinationFolder, "DialogBoxes.BackupDlg._DestinationFolderGroupBox");
			this._groupBoxDestinationFolder.Location = new System.Drawing.Point(0, 67);
			this._groupBoxDestinationFolder.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._groupBoxDestinationFolder.Name = "_groupBoxDestinationFolder";
			this._groupBoxDestinationFolder.Padding = new System.Windows.Forms.Padding(10, 8, 10, 0);
			this._groupBoxDestinationFolder.Size = new System.Drawing.Size(360, 107);
			this._groupBoxDestinationFolder.TabIndex = 5;
			this._groupBoxDestinationFolder.TabStop = false;
			this._groupBoxDestinationFolder.Text = "Destination Folder";
			// 
			// _tableLayoutDestinationFolder
			// 
			this._tableLayoutDestinationFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutDestinationFolder.AutoSize = true;
			this._tableLayoutDestinationFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutDestinationFolder.ColumnCount = 1;
			this._tableLayoutDestinationFolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutDestinationFolder.Controls.Add(this._labelDefaultFolderValue, 0, 1);
			this._tableLayoutDestinationFolder.Controls.Add(this._radioDefaultFolder, 0, 0);
			this._tableLayoutDestinationFolder.Controls.Add(this._linkOtherFolderValue, 0, 3);
			this._tableLayoutDestinationFolder.Controls.Add(this._radioOtherFolder, 0, 2);
			this._tableLayoutDestinationFolder.Location = new System.Drawing.Point(10, 20);
			this._tableLayoutDestinationFolder.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutDestinationFolder.Name = "_tableLayoutDestinationFolder";
			this._tableLayoutDestinationFolder.RowCount = 4;
			this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutDestinationFolder.Size = new System.Drawing.Size(340, 74);
			this._tableLayoutDestinationFolder.TabIndex = 0;
			// 
			// _labelDefaultFolderValue
			// 
			this._labelDefaultFolderValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelDefaultFolderValue.AutoSize = true;
			this._labelDefaultFolderValue.BackColor = System.Drawing.Color.Transparent;
			this._labelDefaultFolderValue.ForeColor = System.Drawing.Color.DarkSlateGray;
			this.locExtender.SetLocalizableToolTip(this._labelDefaultFolderValue, null);
			this.locExtender.SetLocalizationComment(this._labelDefaultFolderValue, null);
			this.locExtender.SetLocalizationPriority(this._labelDefaultFolderValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelDefaultFolderValue, "DialogBoxes.BackupDlg.DefaultFolderValueLabel");
			this._labelDefaultFolderValue.Location = new System.Drawing.Point(17, 20);
			this._labelDefaultFolderValue.Margin = new System.Windows.Forms.Padding(17, 0, 0, 0);
			this._labelDefaultFolderValue.Name = "_labelDefaultFolderValue";
			this._labelDefaultFolderValue.Size = new System.Drawing.Size(323, 13);
			this._labelDefaultFolderValue.TabIndex = 1;
			this._labelDefaultFolderValue.Text = "#";
			// 
			// _radioDefaultFolder
			// 
			this._radioDefaultFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._radioDefaultFolder.AutoSize = true;
			this._radioDefaultFolder.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._radioDefaultFolder, null);
			this.locExtender.SetLocalizationComment(this._radioDefaultFolder, null);
			this.locExtender.SetLocalizingId(this._radioDefaultFolder, "DialogBoxes.BackupDlg.DefaultFolderRadioButton");
			this._radioDefaultFolder.Location = new System.Drawing.Point(0, 0);
			this._radioDefaultFolder.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._radioDefaultFolder.Name = "_radioDefaultFolder";
			this._radioDefaultFolder.Size = new System.Drawing.Size(340, 17);
			this._radioDefaultFolder.TabIndex = 0;
			this._radioDefaultFolder.TabStop = true;
			this._radioDefaultFolder.Text = "Backup to Default Folder";
			this._radioDefaultFolder.UseVisualStyleBackColor = false;
			// 
			// _linkOtherFolderValue
			// 
			this._linkOtherFolderValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._linkOtherFolderValue.AutoSize = true;
			this._linkOtherFolderValue.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._linkOtherFolderValue, "Click to select alternate destination folder");
			this.locExtender.SetLocalizationComment(this._linkOtherFolderValue, null);
			this.locExtender.SetLocalizationPriority(this._linkOtherFolderValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._linkOtherFolderValue, "DialogBoxes.BackupDlg.OtherFolderValueLink");
			this._linkOtherFolderValue.Location = new System.Drawing.Point(17, 61);
			this._linkOtherFolderValue.Margin = new System.Windows.Forms.Padding(17, 0, 0, 0);
			this._linkOtherFolderValue.Name = "_linkOtherFolderValue";
			this._linkOtherFolderValue.Size = new System.Drawing.Size(323, 13);
			this._linkOtherFolderValue.TabIndex = 3;
			this._linkOtherFolderValue.TabStop = true;
			this._linkOtherFolderValue.Text = "#";
			this._linkOtherFolderValue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleOtherFolderValueLinkClick);
			// 
			// _radioOtherFolder
			// 
			this._radioOtherFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._radioOtherFolder.AutoSize = true;
			this._radioOtherFolder.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._radioOtherFolder, null);
			this.locExtender.SetLocalizationComment(this._radioOtherFolder, null);
			this.locExtender.SetLocalizingId(this._radioOtherFolder, "DialogBoxes.BackupDlg.OtherFolderRadioButton");
			this._radioOtherFolder.Location = new System.Drawing.Point(0, 41);
			this._radioOtherFolder.Margin = new System.Windows.Forms.Padding(0, 8, 0, 3);
			this._radioOtherFolder.Name = "_radioOtherFolder";
			this._radioOtherFolder.Size = new System.Drawing.Size(340, 17);
			this._radioOtherFolder.TabIndex = 2;
			this._radioOtherFolder.TabStop = true;
			this._radioOtherFolder.Text = "Backup to Other Folder";
			this._radioOtherFolder.UseVisualStyleBackColor = false;
			// 
			// _groupIncludeInBackup
			// 
			this._groupIncludeInBackup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._groupIncludeInBackup.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._groupIncludeInBackup, 2);
			this._groupIncludeInBackup.Controls.Add(this._tableLayoutIncludeInBackup);
			this.locExtender.SetLocalizableToolTip(this._groupIncludeInBackup, null);
			this.locExtender.SetLocalizationComment(this._groupIncludeInBackup, null);
			this.locExtender.SetLocalizingId(this._groupIncludeInBackup, "DialogBoxes.BackupDlg.IncludeInBackupGroupBox");
			this._groupIncludeInBackup.Location = new System.Drawing.Point(0, 184);
			this._groupIncludeInBackup.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this._groupIncludeInBackup.Name = "_groupIncludeInBackup";
			this._groupIncludeInBackup.Padding = new System.Windows.Forms.Padding(10, 8, 10, 0);
			this._groupIncludeInBackup.Size = new System.Drawing.Size(360, 77);
			this._groupIncludeInBackup.TabIndex = 6;
			this._groupIncludeInBackup.TabStop = false;
			this._groupIncludeInBackup.Text = "Include in Backup (for non FieldWorks data sources)";
			// 
			// _tableLayoutIncludeInBackup
			// 
			this._tableLayoutIncludeInBackup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutIncludeInBackup.AutoSize = true;
			this._tableLayoutIncludeInBackup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutIncludeInBackup.ColumnCount = 1;
			this._tableLayoutIncludeInBackup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutIncludeInBackup.Controls.Add(this._checkBoxIncludeDataSources, 0, 0);
			this._tableLayoutIncludeInBackup.Controls.Add(this._checkBoxIncludeAudioFiles, 0, 1);
			this._tableLayoutIncludeInBackup.Location = new System.Drawing.Point(10, 22);
			this._tableLayoutIncludeInBackup.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutIncludeInBackup.Name = "_tableLayoutIncludeInBackup";
			this._tableLayoutIncludeInBackup.RowCount = 2;
			this._tableLayoutIncludeInBackup.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutIncludeInBackup.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutIncludeInBackup.Size = new System.Drawing.Size(340, 42);
			this._tableLayoutIncludeInBackup.TabIndex = 0;
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
			this.locExtender.SetLocalizableToolTip(this._checkBoxIncludeAudioFiles, null);
			this.locExtender.SetLocalizationComment(this._checkBoxIncludeAudioFiles, null);
			this.locExtender.SetLocalizingId(this._checkBoxIncludeAudioFiles, "DialogBoxes.BackupDlg.IncludeAudioFilesCheckbox");
			this._checkBoxIncludeAudioFiles.Location = new System.Drawing.Point(2, 25);
			this._checkBoxIncludeAudioFiles.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this._checkBoxIncludeAudioFiles.Name = "_checkBoxIncludeAudioFiles";
			this._checkBoxIncludeAudioFiles.Size = new System.Drawing.Size(338, 17);
			this._checkBoxIncludeAudioFiles.TabIndex = 1;
			this._checkBoxIncludeAudioFiles.Text = "Include audio files in backup";
			this._checkBoxIncludeAudioFiles.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this._checkBoxIncludeAudioFiles.UseVisualStyleBackColor = false;
			// 
			// _labelProjectValue
			// 
			this._labelProjectValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelProjectValue.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelProjectValue, null);
			this.locExtender.SetLocalizationComment(this._labelProjectValue, null);
			this.locExtender.SetLocalizationPriority(this._labelProjectValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelProjectValue, "DialogBoxes.BackupDlg.ProjectValueLabel");
			this._labelProjectValue.Location = new System.Drawing.Point(75, 0);
			this._labelProjectValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 8);
			this._labelProjectValue.Name = "_labelProjectValue";
			this._labelProjectValue.Size = new System.Drawing.Size(285, 13);
			this._labelProjectValue.TabIndex = 1;
			this._labelProjectValue.Text = "#";
			// 
			// _labelProject
			// 
			this._labelProject.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelProject, null);
			this.locExtender.SetLocalizationComment(this._labelProject, null);
			this.locExtender.SetLocalizingId(this._labelProject, "DialogBoxes.BackupDlg.ProjectLabel");
			this._labelProject.Location = new System.Drawing.Point(6, 0);
			this._labelProject.Margin = new System.Windows.Forms.Padding(6, 0, 0, 8);
			this._labelProject.Name = "_labelProject";
			this._labelProject.Size = new System.Drawing.Size(43, 13);
			this._labelProject.TabIndex = 0;
			this._labelProject.Text = "Project:";
			// 
			// _linkViewExceptionDetails
			// 
			this._linkViewExceptionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._linkViewExceptionDetails.AutoSize = true;
			this._linkViewExceptionDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.locExtender.SetLocalizableToolTip(this._linkViewExceptionDetails, null);
			this.locExtender.SetLocalizationComment(this._linkViewExceptionDetails, null);
			this.locExtender.SetLocalizingId(this._linkViewExceptionDetails, "DialogBoxes.BackupDlg.ViewExceptionDetailsLink");
			this._linkViewExceptionDetails.Location = new System.Drawing.Point(261, 360);
			this._linkViewExceptionDetails.Margin = new System.Windows.Forms.Padding(0, 10, 0, 3);
			this._linkViewExceptionDetails.Name = "_linkViewExceptionDetails";
			this._linkViewExceptionDetails.Size = new System.Drawing.Size(99, 13);
			this._linkViewExceptionDetails.TabIndex = 7;
			this._linkViewExceptionDetails.TabStop = true;
			this._linkViewExceptionDetails.Text = "View Error Details...";
			this._linkViewExceptionDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this._linkViewExceptionDetails.Visible = false;
			// 
			// _labelBackupFile
			// 
			this._labelBackupFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelBackupFile.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelBackupFile, null);
			this.locExtender.SetLocalizationComment(this._labelBackupFile, null);
			this.locExtender.SetLocalizingId(this._labelBackupFile, "DialogBoxes.BackupDlg.BackupFileLabel");
			this._labelBackupFile.Location = new System.Drawing.Point(6, 24);
			this._labelBackupFile.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
			this._labelBackupFile.Name = "_labelBackupFile";
			this._labelBackupFile.Size = new System.Drawing.Size(66, 13);
			this._labelBackupFile.TabIndex = 2;
			this._labelBackupFile.Text = "Backup File:";
			// 
			// _textBoxBackupFile
			// 
			this._textBoxBackupFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._textBoxBackupFile, null);
			this.locExtender.SetLocalizationComment(this._textBoxBackupFile, null);
			this.locExtender.SetLocalizationPriority(this._textBoxBackupFile, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._textBoxBackupFile, "textBox1.textBox1");
			this._textBoxBackupFile.Location = new System.Drawing.Point(75, 21);
			this._textBoxBackupFile.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._textBoxBackupFile.Name = "_textBoxBackupFile";
			this._textBoxBackupFile.Size = new System.Drawing.Size(285, 20);
			this._textBoxBackupFile.TabIndex = 3;
			this._textBoxBackupFile.TextChanged += new System.EventHandler(this.HandleFileNameTextChanged);
			// 
			// _labelBackupFileValue
			// 
			this._labelBackupFileValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelBackupFileValue.AutoSize = true;
			this._labelBackupFileValue.ForeColor = System.Drawing.Color.DarkSlateGray;
			this.locExtender.SetLocalizableToolTip(this._labelBackupFileValue, null);
			this.locExtender.SetLocalizationComment(this._labelBackupFileValue, null);
			this.locExtender.SetLocalizationPriority(this._labelBackupFileValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelBackupFileValue, "BackupDlg._labelBackupFileValue");
			this._labelBackupFileValue.Location = new System.Drawing.Point(75, 44);
			this._labelBackupFileValue.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
			this._labelBackupFileValue.Name = "_labelBackupFileValue";
			this._labelBackupFileValue.Size = new System.Drawing.Size(285, 13);
			this._labelBackupFileValue.TabIndex = 4;
			this._labelBackupFileValue.Text = "#";
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
			this._tableLayoutButtons.Controls.Add(this._buttonBackup, 1, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonCancel, 2, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonClose, 3, 0);
			this._tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._tableLayoutButtons.Location = new System.Drawing.Point(15, 419);
			this._tableLayoutButtons.Name = "_tableLayoutButtons";
			this._tableLayoutButtons.RowCount = 1;
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutButtons.Size = new System.Drawing.Size(360, 36);
			this._tableLayoutButtons.TabIndex = 1;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// BackupDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 470);
			this.Controls.Add(this._tableLayoutPanel);
			this.Controls.Add(this._tableLayoutButtons);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.BackupDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(400, 500);
			this.Name = "BackupDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Backup";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this._groupBoxDestinationFolder.ResumeLayout(false);
			this._groupBoxDestinationFolder.PerformLayout();
			this._tableLayoutDestinationFolder.ResumeLayout(false);
			this._tableLayoutDestinationFolder.PerformLayout();
			this._groupIncludeInBackup.ResumeLayout(false);
			this._groupIncludeInBackup.PerformLayout();
			this._tableLayoutIncludeInBackup.ResumeLayout(false);
			this._tableLayoutIncludeInBackup.PerformLayout();
			this._tableLayoutButtons.ResumeLayout(false);
			this._tableLayoutButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _buttonBackup;
		private CheckBox _checkBoxIncludeDataSources;
		private System.Windows.Forms.ProgressBar _progressBar;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.Label _labelBackupFile;
		private CheckBox _checkBoxIncludeAudioFiles;
		private System.Windows.Forms.Label _labelBackupFileValue;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutButtons;
		private System.Windows.Forms.Label _labelProjectValue;
		private System.Windows.Forms.Label _labelProject;
		protected Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TextBox _textBoxBackupFile;
		private System.Windows.Forms.LinkLabel _linkViewExceptionDetails;
		private System.Windows.Forms.GroupBox _groupBoxDestinationFolder;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutDestinationFolder;
		private System.Windows.Forms.Label _labelDefaultFolderValue;
		private System.Windows.Forms.RadioButton _radioDefaultFolder;
		private System.Windows.Forms.LinkLabel _linkOtherFolderValue;
		private System.Windows.Forms.RadioButton _radioOtherFolder;
		private System.Windows.Forms.GroupBox _groupIncludeInBackup;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutIncludeInBackup;
	}
}