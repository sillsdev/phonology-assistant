namespace SIL.Pa.UI.Dialogs
{
	partial class RestoreDlg
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this._buttonRestore = new System.Windows.Forms.Button();
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
            this._grid = new SilTools.SilGrid();
            this._colProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colBackupFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._labelBackupFilesFound = new System.Windows.Forms.Label();
            this._linkSelectOtherBackupFile = new System.Windows.Forms.LinkLabel();
            this._linkViewExceptionDetails = new System.Windows.Forms.LinkLabel();
            this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
            this._buttonLoadProject = new System.Windows.Forms.Button();
            this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
            this._tableLayoutPanel.SuspendLayout();
            this._groupBoxDestinationFolder.SuspendLayout();
            this._tableLayoutDestinationFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._tableLayoutButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.SuspendLayout();
            // 
            // _buttonRestore
            // 
            this._buttonRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonRestore.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._buttonRestore, "Restore Selected Backup File");
            this.locExtender.SetLocalizationComment(this._buttonRestore, null);
            this.locExtender.SetLocalizingId(this._buttonRestore, "DialogBoxes.RestoreDlg.RestoreButton");
            this._buttonRestore.Location = new System.Drawing.Point(164, 12);
            this._buttonRestore.Margin = new System.Windows.Forms.Padding(8, 12, 0, 0);
            this._buttonRestore.MinimumSize = new System.Drawing.Size(100, 32);
            this._buttonRestore.Name = "_buttonRestore";
            this._buttonRestore.Size = new System.Drawing.Size(100, 33);
            this._buttonRestore.TabIndex = 1;
            this._buttonRestore.Text = "Restore";
            this._buttonRestore.UseVisualStyleBackColor = true;
            this._buttonRestore.Click += new System.EventHandler(this.HandleRestoreButtonClick);
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayoutPanel.SetColumnSpan(this._progressBar, 2);
            this._progressBar.Location = new System.Drawing.Point(0, 475);
            this._progressBar.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(480, 22);
            this._progressBar.TabIndex = 5;
            this._progressBar.Visible = false;
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
            this.locExtender.SetLocalizationComment(this._buttonCancel, null);
            this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.RestoreDlg.CancelButton");
            this._buttonCancel.Location = new System.Drawing.Point(272, 12);
            this._buttonCancel.Margin = new System.Windows.Forms.Padding(8, 12, 0, 0);
            this._buttonCancel.MinimumSize = new System.Drawing.Size(100, 32);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(100, 33);
            this._buttonCancel.TabIndex = 2;
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
            this.locExtender.SetLocalizingId(this._buttonClose, "DialogBoxes.RestoreDlg.CloseButton");
            this._buttonClose.Location = new System.Drawing.Point(380, 12);
            this._buttonClose.Margin = new System.Windows.Forms.Padding(8, 12, 0, 0);
            this._buttonClose.MinimumSize = new System.Drawing.Size(100, 32);
            this._buttonClose.Name = "_buttonClose";
            this._buttonClose.Size = new System.Drawing.Size(100, 33);
            this._buttonClose.TabIndex = 3;
            this._buttonClose.Text = "Close";
            this._buttonClose.UseVisualStyleBackColor = true;
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.AutoSize = true;
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this._groupBoxDestinationFolder, 0, 2);
            this._tableLayoutPanel.Controls.Add(this._grid, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._labelBackupFilesFound, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._progressBar, 0, 5);
            this._tableLayoutPanel.Controls.Add(this._linkSelectOtherBackupFile, 1, 0);
            this._tableLayoutPanel.Controls.Add(this._linkViewExceptionDetails, 1, 4);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(20, 18);
            this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 6;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(480, 497);
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
            this.locExtender.SetLocalizingId(this._groupBoxDestinationFolder, "DialogBoxes.RestoreDlg.DestinationFolderGroupBox");
            this._groupBoxDestinationFolder.Location = new System.Drawing.Point(0, 156);
            this._groupBoxDestinationFolder.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this._groupBoxDestinationFolder.Name = "_groupBoxDestinationFolder";
            this._groupBoxDestinationFolder.Padding = new System.Windows.Forms.Padding(13, 10, 13, 0);
            this._groupBoxDestinationFolder.Size = new System.Drawing.Size(480, 134);
            this._groupBoxDestinationFolder.TabIndex = 3;
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
            this._tableLayoutDestinationFolder.Location = new System.Drawing.Point(13, 25);
            this._tableLayoutDestinationFolder.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutDestinationFolder.Name = "_tableLayoutDestinationFolder";
            this._tableLayoutDestinationFolder.RowCount = 4;
            this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutDestinationFolder.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutDestinationFolder.Size = new System.Drawing.Size(453, 94);
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
            this.locExtender.SetLocalizationPriority(this._labelDefaultFolderValue, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._labelDefaultFolderValue, "DialogBoxes.RestoreDlg.RestoreFolderValueLabel");
            this._labelDefaultFolderValue.Location = new System.Drawing.Point(23, 25);
            this._labelDefaultFolderValue.Margin = new System.Windows.Forms.Padding(23, 0, 0, 0);
            this._labelDefaultFolderValue.Name = "_labelDefaultFolderValue";
            this._labelDefaultFolderValue.Size = new System.Drawing.Size(430, 17);
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
            this.locExtender.SetLocalizingId(this._radioDefaultFolder, "DialogBoxes.RestoreDlg.DefaultFolderRadioButton");
            this._radioDefaultFolder.Location = new System.Drawing.Point(0, 0);
            this._radioDefaultFolder.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this._radioDefaultFolder.Name = "_radioDefaultFolder";
            this._radioDefaultFolder.Size = new System.Drawing.Size(453, 21);
            this._radioDefaultFolder.TabIndex = 0;
            this._radioDefaultFolder.TabStop = true;
            this._radioDefaultFolder.Text = "Restore to Default Folder";
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
            this.locExtender.SetLocalizationPriority(this._linkOtherFolderValue, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._linkOtherFolderValue, "DialogBoxes.RestoreDlg.OtherFolderValueLink");
            this._linkOtherFolderValue.Location = new System.Drawing.Point(23, 77);
            this._linkOtherFolderValue.Margin = new System.Windows.Forms.Padding(23, 0, 0, 0);
            this._linkOtherFolderValue.Name = "_linkOtherFolderValue";
            this._linkOtherFolderValue.Size = new System.Drawing.Size(430, 17);
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
            this.locExtender.SetLocalizingId(this._radioOtherFolder, "DialogBoxes.RestoreDlg.OtherFolderRadioButton");
            this._radioOtherFolder.Location = new System.Drawing.Point(0, 52);
            this._radioOtherFolder.Margin = new System.Windows.Forms.Padding(0, 10, 0, 4);
            this._radioOtherFolder.Name = "_radioOtherFolder";
            this._radioOtherFolder.Size = new System.Drawing.Size(453, 21);
            this._radioOtherFolder.TabIndex = 2;
            this._radioOtherFolder.TabStop = true;
            this._radioOtherFolder.Text = "Restore to Other Folder";
            this._radioOtherFolder.UseVisualStyleBackColor = false;
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToOrderColumns = true;
            this._grid.AllowUserToResizeRows = false;
            this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colProject,
            this._colBackupFile});
            this._tableLayoutPanel.SetColumnSpan(this._grid, 2);
            this._grid.DrawTextBoxEditControlBorder = false;
            this._grid.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
            this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this._grid.IsDirty = false;
            this.locExtender.SetLocalizableToolTip(this._grid, null);
            this.locExtender.SetLocalizationComment(this._grid, null);
            this.locExtender.SetLocalizingId(this._grid, "silGrid1.silGrid1");
            this._grid.Location = new System.Drawing.Point(0, 22);
            this._grid.Margin = new System.Windows.Forms.Padding(0, 5, 0, 12);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.PaintHeaderAcrossFullGridWidth = true;
            this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidth = 22;
            this._grid.SelectedCellBackColor = System.Drawing.Color.Empty;
            this._grid.SelectedCellForeColor = System.Drawing.Color.Empty;
            this._grid.SelectedRowBackColor = System.Drawing.Color.Empty;
            this._grid.SelectedRowForeColor = System.Drawing.Color.Empty;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.ShowWaterMarkWhenDirty = false;
            this._grid.Size = new System.Drawing.Size(480, 122);
            this._grid.TabIndex = 2;
            this._grid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
            this._grid.WaterMark = "!";
            this._grid.CurrentRowChanged += new System.EventHandler(this.HandleGridCurrentRowChanged);
            this._grid.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleGridPainting);
            // 
            // _colProject
            // 
            this._colProject.HeaderText = "_L10N_:DialogBoxes.RestoreDlg.ColumnHeadings.Project!Project";
            this._colProject.Name = "_colProject";
            this._colProject.ReadOnly = true;
            // 
            // _colBackupFile
            // 
            this._colBackupFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._colBackupFile.FillWeight = 80F;
            this._colBackupFile.HeaderText = "_L10N_:DialogBoxes.RestoreDlg.ColumnHeadings.BackupFile!Backup File";
            this._colBackupFile.Name = "_colBackupFile";
            this._colBackupFile.ReadOnly = true;
            // 
            // _labelBackupFilesFound
            // 
            this._labelBackupFilesFound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._labelBackupFilesFound.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._labelBackupFilesFound, null);
            this.locExtender.SetLocalizationComment(this._labelBackupFilesFound, null);
            this.locExtender.SetLocalizingId(this._labelBackupFilesFound, "DialogBoxes.RestoreDlg.BackupFilesFoundLabel");
            this._labelBackupFilesFound.Location = new System.Drawing.Point(3, 0);
            this._labelBackupFilesFound.Margin = new System.Windows.Forms.Padding(3, 0, 4, 0);
            this._labelBackupFilesFound.Name = "_labelBackupFilesFound";
            this._labelBackupFilesFound.Size = new System.Drawing.Size(132, 17);
            this._labelBackupFilesFound.TabIndex = 0;
            this._labelBackupFilesFound.Text = "Backup Files Found";
            // 
            // _linkSelectOtherBackupFile
            // 
            this._linkSelectOtherBackupFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._linkSelectOtherBackupFile.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._linkSelectOtherBackupFile, null);
            this.locExtender.SetLocalizationComment(this._linkSelectOtherBackupFile, null);
            this.locExtender.SetLocalizingId(this._linkSelectOtherBackupFile, "DialogBoxes.RestoreDlg.SelectOtherBackupFileLink");
            this._linkSelectOtherBackupFile.Location = new System.Drawing.Point(304, 0);
            this._linkSelectOtherBackupFile.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this._linkSelectOtherBackupFile.Name = "_linkSelectOtherBackupFile";
            this._linkSelectOtherBackupFile.Size = new System.Drawing.Size(176, 17);
            this._linkSelectOtherBackupFile.TabIndex = 1;
            this._linkSelectOtherBackupFile.TabStop = true;
            this._linkSelectOtherBackupFile.Text = "Select Other Backup File...";
            this._linkSelectOtherBackupFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleSelectOtherBackupFileLinkClick);
            // 
            // _linkViewExceptionDetails
            // 
            this._linkViewExceptionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._linkViewExceptionDetails.AutoSize = true;
            this._linkViewExceptionDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.locExtender.SetLocalizableToolTip(this._linkViewExceptionDetails, null);
            this.locExtender.SetLocalizationComment(this._linkViewExceptionDetails, null);
            this.locExtender.SetLocalizingId(this._linkViewExceptionDetails, "DialogBoxes.RestoreDlg.ViewExceptionDetailsLink");
            this._linkViewExceptionDetails.Location = new System.Drawing.Point(348, 442);
            this._linkViewExceptionDetails.Margin = new System.Windows.Forms.Padding(0, 12, 0, 4);
            this._linkViewExceptionDetails.Name = "_linkViewExceptionDetails";
            this._linkViewExceptionDetails.Size = new System.Drawing.Size(132, 17);
            this._linkViewExceptionDetails.TabIndex = 4;
            this._linkViewExceptionDetails.TabStop = true;
            this._linkViewExceptionDetails.Text = "View Error Details...";
            this._linkViewExceptionDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._linkViewExceptionDetails.Visible = false;
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
            this._tableLayoutButtons.Controls.Add(this._buttonLoadProject, 0, 0);
            this._tableLayoutButtons.Controls.Add(this._buttonRestore, 1, 0);
            this._tableLayoutButtons.Controls.Add(this._buttonCancel, 2, 0);
            this._tableLayoutButtons.Controls.Add(this._buttonClose, 3, 0);
            this._tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._tableLayoutButtons.Location = new System.Drawing.Point(20, 515);
            this._tableLayoutButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._tableLayoutButtons.Name = "_tableLayoutButtons";
            this._tableLayoutButtons.RowCount = 1;
            this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutButtons.Size = new System.Drawing.Size(480, 45);
            this._tableLayoutButtons.TabIndex = 1;
            // 
            // _buttonLoadProject
            // 
            this._buttonLoadProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonLoadProject.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._buttonLoadProject, "");
            this.locExtender.SetLocalizationComment(this._buttonLoadProject, null);
            this.locExtender.SetLocalizingId(this._buttonLoadProject, "DialogBoxes.RestoreDlg.LoadProjectButton");
            this._buttonLoadProject.Location = new System.Drawing.Point(0, 12);
            this._buttonLoadProject.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this._buttonLoadProject.MinimumSize = new System.Drawing.Size(160, 32);
            this._buttonLoadProject.Name = "_buttonLoadProject";
            this._buttonLoadProject.Size = new System.Drawing.Size(160, 33);
            this._buttonLoadProject.TabIndex = 0;
            this._buttonLoadProject.Text = "Load Project && Close";
            this._buttonLoadProject.UseVisualStyleBackColor = true;
            this._buttonLoadProject.Visible = false;
            this._buttonLoadProject.Click += new System.EventHandler(this.HandleLoadProjectButtonClick);
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "Pa";
            // 
            // RestoreDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 578);
            this.Controls.Add(this._tableLayoutPanel);
            this.Controls.Add(this._tableLayoutButtons);
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizingId(this, "DialogBoxes.RestoreDlg.WindowTitle");
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(527, 605);
            this.Name = "RestoreDlg";
            this.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Restore";
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this._groupBoxDestinationFolder.ResumeLayout(false);
            this._groupBoxDestinationFolder.PerformLayout();
            this._tableLayoutDestinationFolder.ResumeLayout(false);
            this._tableLayoutDestinationFolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._tableLayoutButtons.ResumeLayout(false);
            this._tableLayoutButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _buttonRestore;
		private System.Windows.Forms.ProgressBar _progressBar;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutButtons;
		protected L10NSharp.UI.L10NSharpExtender locExtender;
		private SilTools.SilGrid _grid;
		private System.Windows.Forms.Label _labelBackupFilesFound;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colProject;
		private System.Windows.Forms.DataGridViewTextBoxColumn _colBackupFile;
		private System.Windows.Forms.LinkLabel _linkOtherFolderValue;
		private System.Windows.Forms.GroupBox _groupBoxDestinationFolder;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutDestinationFolder;
		private System.Windows.Forms.Label _labelDefaultFolderValue;
		private System.Windows.Forms.RadioButton _radioDefaultFolder;
		private System.Windows.Forms.RadioButton _radioOtherFolder;
		private System.Windows.Forms.LinkLabel _linkViewExceptionDetails;
		private System.Windows.Forms.LinkLabel _linkSelectOtherBackupFile;
		private System.Windows.Forms.Button _buttonLoadProject;
	}
}