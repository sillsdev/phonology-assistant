using SilTools.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class ProjectSettingsDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblProjName = new System.Windows.Forms.Label();
			this.lblTranscriber = new System.Windows.Forms.Label();
			this.lblSpeaker = new System.Windows.Forms.Label();
			this.lblComments = new System.Windows.Forms.Label();
			this.txtProjName = new System.Windows.Forms.TextBox();
			this.txtTranscriber = new System.Windows.Forms.TextBox();
			this.txtSpeaker = new System.Windows.Forms.TextBox();
			this.txtComments = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnProperties = new System.Windows.Forms.Button();
			this.txtLanguageName = new System.Windows.Forms.TextBox();
			this.lblLanguageName = new System.Windows.Forms.Label();
			this.mnuAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAddOtherDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddFwDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddFw7DataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddFw6DataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.m_grid = new SilTools.SilGrid();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.lblLanguageCode = new System.Windows.Forms.Label();
			this.txtLanguageCode = new System.Windows.Forms.TextBox();
			this.lnkEthnologue = new System.Windows.Forms.LinkLabel();
			this.txtResearcher = new System.Windows.Forms.TextBox();
			this.lblResearcher = new System.Windows.Forms.Label();
			this.pnlGrid = new SilTools.Controls.SilPanel();
			this.pnlGridHdg = new SilTools.Controls.SilGradientPanel();
			this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
			this._chkMakeFolder = new System.Windows.Forms.CheckBox();
			this.mnuAdd.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.pnlGrid.SuspendLayout();
			this.tblLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblProjName
			// 
			this.lblProjName.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblProjName, null);
			this.locExtender.SetLocalizationComment(this.lblProjName, null);
			this.locExtender.SetLocalizingId(this.lblProjName, "ProjectSettingsDlg.lblProjName");
			this.lblProjName.Location = new System.Drawing.Point(3, 3);
			this.lblProjName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.lblProjName.Name = "lblProjName";
			this.lblProjName.Size = new System.Drawing.Size(74, 13);
			this.lblProjName.TabIndex = 0;
			this.lblProjName.Text = "Project &Name:";
			// 
			// lblTranscriber
			// 
			this.lblTranscriber.AutoSize = true;
			this.lblTranscriber.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblTranscriber, null);
			this.locExtender.SetLocalizationComment(this.lblTranscriber, null);
			this.locExtender.SetLocalizingId(this.lblTranscriber, "ProjectSettingsDlg.lblTranscriber");
			this.lblTranscriber.Location = new System.Drawing.Point(317, 3);
			this.lblTranscriber.Margin = new System.Windows.Forms.Padding(8, 3, 3, 0);
			this.lblTranscriber.Name = "lblTranscriber";
			this.lblTranscriber.Size = new System.Drawing.Size(63, 13);
			this.lblTranscriber.TabIndex = 9;
			this.lblTranscriber.Text = "&Transcriber:";
			// 
			// lblSpeaker
			// 
			this.lblSpeaker.AutoSize = true;
			this.lblSpeaker.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSpeaker, null);
			this.locExtender.SetLocalizationComment(this.lblSpeaker, null);
			this.locExtender.SetLocalizingId(this.lblSpeaker, "ProjectSettingsDlg.lblSpeaker");
			this.lblSpeaker.Location = new System.Drawing.Point(317, 31);
			this.lblSpeaker.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
			this.lblSpeaker.Name = "lblSpeaker";
			this.lblSpeaker.Size = new System.Drawing.Size(50, 13);
			this.lblSpeaker.TabIndex = 11;
			this.lblSpeaker.Text = "&Speaker:";
			// 
			// lblComments
			// 
			this.lblComments.AutoSize = true;
			this.lblComments.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblComments, null);
			this.locExtender.SetLocalizationComment(this.lblComments, null);
			this.locExtender.SetLocalizingId(this.lblComments, "ProjectSettingsDlg.lblComments");
			this.lblComments.Location = new System.Drawing.Point(317, 59);
			this.lblComments.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
			this.lblComments.Name = "lblComments";
			this.lblComments.Size = new System.Drawing.Size(59, 13);
			this.lblComments.TabIndex = 13;
			this.lblComments.Text = "&Comments:";
			// 
			// txtProjName
			// 
			this.txtProjName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.SetColumnSpan(this.txtProjName, 2);
			this.locExtender.SetLocalizableToolTip(this.txtProjName, null);
			this.locExtender.SetLocalizationComment(this.txtProjName, null);
			this.locExtender.SetLocalizingId(this.txtProjName, "ProjectSettingsDlg.txtProjName");
			this.txtProjName.Location = new System.Drawing.Point(98, 0);
			this.txtProjName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.txtProjName.Name = "txtProjName";
			this.txtProjName.Size = new System.Drawing.Size(208, 20);
			this.txtProjName.TabIndex = 1;
			this.txtProjName.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// txtTranscriber
			// 
			this.txtTranscriber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.SetColumnSpan(this.txtTranscriber, 2);
			this.locExtender.SetLocalizableToolTip(this.txtTranscriber, null);
			this.locExtender.SetLocalizationComment(this.txtTranscriber, null);
			this.locExtender.SetLocalizingId(this.txtTranscriber, "ProjectSettingsDlg.txtTranscriber");
			this.txtTranscriber.Location = new System.Drawing.Point(386, 0);
			this.txtTranscriber.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
			this.txtTranscriber.Name = "txtTranscriber";
			this.txtTranscriber.Size = new System.Drawing.Size(239, 20);
			this.txtTranscriber.TabIndex = 10;
			this.txtTranscriber.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// txtSpeaker
			// 
			this.txtSpeaker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.SetColumnSpan(this.txtSpeaker, 2);
			this.locExtender.SetLocalizableToolTip(this.txtSpeaker, null);
			this.locExtender.SetLocalizationComment(this.txtSpeaker, null);
			this.locExtender.SetLocalizingId(this.txtSpeaker, "ProjectSettingsDlg.txtSpeaker");
			this.txtSpeaker.Location = new System.Drawing.Point(386, 28);
			this.txtSpeaker.Margin = new System.Windows.Forms.Padding(3, 5, 0, 3);
			this.txtSpeaker.Name = "txtSpeaker";
			this.txtSpeaker.Size = new System.Drawing.Size(239, 20);
			this.txtSpeaker.TabIndex = 12;
			this.txtSpeaker.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// txtComments
			// 
			this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.SetColumnSpan(this.txtComments, 2);
			this.locExtender.SetLocalizableToolTip(this.txtComments, null);
			this.locExtender.SetLocalizationComment(this.txtComments, null);
			this.locExtender.SetLocalizingId(this.txtComments, "ProjectSettingsDlg.txtComments");
			this.txtComments.Location = new System.Drawing.Point(386, 56);
			this.txtComments.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.txtComments.Multiline = true;
			this.txtComments.Name = "txtComments";
			this.tblLayout.SetRowSpan(this.txtComments, 2);
			this.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtComments.Size = new System.Drawing.Size(239, 48);
			this.txtComments.TabIndex = 14;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.AutoSize = true;
			this.btnAdd.Image = global::SIL.Pa.Properties.Resources.kimidButtonDropDownArrow;
			this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnAdd, null);
			this.locExtender.SetLocalizationComment(this.btnAdd, null);
			this.locExtender.SetLocalizingId(this.btnAdd, "ProjectSettingsDlg.btnAdd");
			this.btnAdd.Location = new System.Drawing.Point(530, 110);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.btnAdd.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(95, 26);
			this.btnAdd.TabIndex = 16;
			this.btnAdd.Text = "&Add";
			this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.HandleAddButtonClick);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.btnRemove, null);
			this.locExtender.SetLocalizationComment(this.btnRemove, null);
			this.locExtender.SetLocalizingId(this.btnRemove, "ProjectSettingsDlg.btnRemove");
			this.btnRemove.Location = new System.Drawing.Point(530, 141);
			this.btnRemove.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.btnRemove.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(95, 26);
			this.btnRemove.TabIndex = 17;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.HandleRemoveButtonClick);
			// 
			// btnProperties
			// 
			this.btnProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnProperties.AutoSize = true;
			this.btnProperties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnProperties, null);
			this.locExtender.SetLocalizationComment(this.btnProperties, null);
			this.locExtender.SetLocalizingId(this.btnProperties, "ProjectSettingsDlg.btnProperties");
			this.btnProperties.Location = new System.Drawing.Point(530, 172);
			this.btnProperties.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.btnProperties.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnProperties.Name = "btnProperties";
			this.btnProperties.Size = new System.Drawing.Size(95, 26);
			this.btnProperties.TabIndex = 18;
			this.btnProperties.Text = "&Properties...";
			this.btnProperties.UseVisualStyleBackColor = true;
			this.btnProperties.Click += new System.EventHandler(this.HandlePropertyButtonClick);
			// 
			// txtLanguageName
			// 
			this.txtLanguageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.SetColumnSpan(this.txtLanguageName, 2);
			this.locExtender.SetLocalizableToolTip(this.txtLanguageName, null);
			this.locExtender.SetLocalizationComment(this.txtLanguageName, null);
			this.locExtender.SetLocalizingId(this.txtLanguageName, "ProjectSettingsDlg.txtLanguageName");
			this.txtLanguageName.Location = new System.Drawing.Point(98, 28);
			this.txtLanguageName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.txtLanguageName.Name = "txtLanguageName";
			this.txtLanguageName.Size = new System.Drawing.Size(208, 20);
			this.txtLanguageName.TabIndex = 3;
			this.txtLanguageName.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// lblLanguageName
			// 
			this.lblLanguageName.AutoSize = true;
			this.lblLanguageName.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblLanguageName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblLanguageName, null);
			this.locExtender.SetLocalizationComment(this.lblLanguageName, null);
			this.locExtender.SetLocalizingId(this.lblLanguageName, "ProjectSettingsDlg.lblLanguageName");
			this.lblLanguageName.Location = new System.Drawing.Point(3, 31);
			this.lblLanguageName.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this.lblLanguageName.Name = "lblLanguageName";
			this.lblLanguageName.Size = new System.Drawing.Size(89, 13);
			this.lblLanguageName.TabIndex = 2;
			this.lblLanguageName.Text = "&Language Name:";
			// 
			// mnuAdd
			// 
			this.mnuAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddOtherDataSource,
            this.mnuAddFwDataSource});
			this.locExtender.SetLocalizableToolTip(this.mnuAdd, null);
			this.locExtender.SetLocalizationComment(this.mnuAdd, null);
			this.locExtender.SetLocalizationPriority(this.mnuAdd, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.mnuAdd, "ProjectSettingsDlg.mnuAdd");
			this.mnuAdd.Name = "cmnuAdd";
			this.mnuAdd.ShowImageMargin = false;
			this.mnuAdd.Size = new System.Drawing.Size(209, 70);
			// 
			// mnuAddOtherDataSource
			// 
			this.locExtender.SetLocalizableToolTip(this.mnuAddOtherDataSource, null);
			this.locExtender.SetLocalizationComment(this.mnuAddOtherDataSource, null);
			this.locExtender.SetLocalizingId(this.mnuAddOtherDataSource, "ProjectSettingsDlg.mnuAddOtherDataSource");
			this.mnuAddOtherDataSource.Name = "mnuAddOtherDataSource";
			this.mnuAddOtherDataSource.Size = new System.Drawing.Size(208, 22);
			this.mnuAddOtherDataSource.Text = "&Non FieldWorks Data Source...";
			this.mnuAddOtherDataSource.Click += new System.EventHandler(this.HandleAddOtherDataSourceClick);
			// 
			// mnuAddFwDataSource
			// 
			this.mnuAddFwDataSource.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddFw7DataSource,
            this.mnuAddFw6DataSource});
			this.locExtender.SetLocalizableToolTip(this.mnuAddFwDataSource, null);
			this.locExtender.SetLocalizationComment(this.mnuAddFwDataSource, null);
			this.locExtender.SetLocalizingId(this.mnuAddFwDataSource, "ProjectSettingsDlg.mnuAddFwDataSource");
			this.mnuAddFwDataSource.Name = "mnuAddFwDataSource";
			this.mnuAddFwDataSource.Size = new System.Drawing.Size(208, 22);
			this.mnuAddFwDataSource.Text = "&FieldWorks Data Source";
			// 
			// mnuAddFw7DataSource
			// 
			this.locExtender.SetLocalizableToolTip(this.mnuAddFw7DataSource, null);
			this.locExtender.SetLocalizationComment(this.mnuAddFw7DataSource, null);
			this.locExtender.SetLocalizingId(this.mnuAddFw7DataSource, "ProjectSettingsDlg.mnuAddFw7DataSource");
			this.mnuAddFw7DataSource.Name = "mnuAddFw7DataSource";
			this.mnuAddFw7DataSource.Size = new System.Drawing.Size(153, 22);
			this.mnuAddFw7DataSource.Text = "&7.0 or Later...";
			this.mnuAddFw7DataSource.Click += new System.EventHandler(this.HandleAddFw7DataSourceClick);
			// 
			// mnuAddFw6DataSource
			// 
			this.locExtender.SetLocalizableToolTip(this.mnuAddFw6DataSource, null);
			this.locExtender.SetLocalizationComment(this.mnuAddFw6DataSource, null);
			this.locExtender.SetLocalizingId(this.mnuAddFw6DataSource, "ProjectSettingsDlg.mnuAddFw6DataSource");
			this.mnuAddFw6DataSource.Name = "mnuAddFw6DataSource";
			this.mnuAddFw6DataSource.Size = new System.Drawing.Size(153, 22);
			this.mnuAddFw6DataSource.Text = "&6.0.6 or Older...";
			this.mnuAddFw6DataSource.Click += new System.EventHandler(this.HandleAddFw6DataSourceClick);
			// 
			// m_grid
			// 
			this.m_grid.AllowUserToAddRows = false;
			this.m_grid.AllowUserToDeleteRows = false;
			this.m_grid.AllowUserToOrderColumns = true;
			this.m_grid.AllowUserToResizeRows = false;
			this.m_grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grid.DrawTextBoxEditControlBorder = false;
			this.m_grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.m_grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.m_grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_grid, null);
			this.locExtender.SetLocalizationComment(this.m_grid, null);
			this.locExtender.SetLocalizingId(this.m_grid, "ProjectSettingsDlg.m_grid");
			this.m_grid.Location = new System.Drawing.Point(0, 25);
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.PaintHeaderAcrossFullGridWidth = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersWidth = 22;
			this.m_grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_grid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.Size = new System.Drawing.Size(523, 146);
			this.m_grid.StandardTab = true;
			this.m_grid.TabIndex = 1;
			this.m_grid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_grid.VirtualMode = true;
			this.m_grid.WaterMark = "!";
			this.m_grid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.HandleGridCellMouseDoubleClick);
			this.m_grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.HandleGridCellPainting);
			this.m_grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.HandleGridCellNeeded);
			this.m_grid.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.HandleGridCellValuePushed);
			this.m_grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleGridKeyDown);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// lblLanguageCode
			// 
			this.lblLanguageCode.AutoSize = true;
			this.lblLanguageCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblLanguageCode, null);
			this.locExtender.SetLocalizationComment(this.lblLanguageCode, null);
			this.locExtender.SetLocalizingId(this.lblLanguageCode, "ProjectSettingsDlg.lblLanguageCode");
			this.lblLanguageCode.Location = new System.Drawing.Point(3, 59);
			this.lblLanguageCode.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this.lblLanguageCode.Name = "lblLanguageCode";
			this.lblLanguageCode.Size = new System.Drawing.Size(86, 13);
			this.lblLanguageCode.TabIndex = 4;
			this.lblLanguageCode.Text = "Language C&ode:";
			// 
			// txtLanguageCode
			// 
			this.txtLanguageCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.txtLanguageCode, null);
			this.locExtender.SetLocalizationComment(this.txtLanguageCode, null);
			this.locExtender.SetLocalizingId(this.txtLanguageCode, "ProjectSettingsDlg.txtLanguageCode");
			this.txtLanguageCode.Location = new System.Drawing.Point(98, 56);
			this.txtLanguageCode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.txtLanguageCode.Name = "txtLanguageCode";
			this.txtLanguageCode.Size = new System.Drawing.Size(141, 20);
			this.txtLanguageCode.TabIndex = 5;
			this.txtLanguageCode.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// lnkEthnologue
			// 
			this.lnkEthnologue.AutoEllipsis = true;
			this.lnkEthnologue.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lnkEthnologue, null);
			this.locExtender.SetLocalizationComment(this.lnkEthnologue, null);
			this.locExtender.SetLocalizingId(this.lnkEthnologue, "ProjectSettingsDlg.lnkEthnologue");
			this.lnkEthnologue.Location = new System.Drawing.Point(245, 59);
			this.lnkEthnologue.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this.lnkEthnologue.Name = "lnkEthnologue";
			this.lnkEthnologue.Size = new System.Drawing.Size(61, 13);
			this.lnkEthnologue.TabIndex = 6;
			this.lnkEthnologue.TabStop = true;
			this.lnkEthnologue.Text = "Ethnologue";
			this.lnkEthnologue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lnkEthnologue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleEthnologueLinkClicked);
			// 
			// txtResearcher
			// 
			this.txtResearcher.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.SetColumnSpan(this.txtResearcher, 2);
			this.locExtender.SetLocalizableToolTip(this.txtResearcher, null);
			this.locExtender.SetLocalizationComment(this.txtResearcher, null);
			this.locExtender.SetLocalizingId(this.txtResearcher, "ProjectSettingsDlg.txtResearcher");
			this.txtResearcher.Location = new System.Drawing.Point(98, 84);
			this.txtResearcher.Margin = new System.Windows.Forms.Padding(3, 5, 3, 6);
			this.txtResearcher.Name = "txtResearcher";
			this.txtResearcher.Size = new System.Drawing.Size(208, 20);
			this.txtResearcher.TabIndex = 8;
			this.txtResearcher.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// lblResearcher
			// 
			this.lblResearcher.AutoSize = true;
			this.lblResearcher.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblResearcher, null);
			this.locExtender.SetLocalizationComment(this.lblResearcher, null);
			this.locExtender.SetLocalizingId(this.lblResearcher, "ProjectSettingsDlg.lblResearcher");
			this.lblResearcher.Location = new System.Drawing.Point(3, 87);
			this.lblResearcher.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this.lblResearcher.Name = "lblResearcher";
			this.lblResearcher.Size = new System.Drawing.Size(65, 13);
			this.lblResearcher.TabIndex = 7;
			this.lblResearcher.Text = "R&esearcher:";
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.tblLayout.SetColumnSpan(this.pnlGrid, 5);
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this._chkMakeFolder);
			this.pnlGrid.Controls.Add(this.m_grid);
			this.pnlGrid.Controls.Add(this.pnlGridHdg);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.DoubleBuffered = true;
			this.pnlGrid.DrawOnlyBottomBorder = false;
			this.pnlGrid.DrawOnlyTopBorder = false;
			this.pnlGrid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlGrid.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizingId(this.pnlGrid, "ProjectSettingsDlg.pnlGrid");
			this.pnlGrid.Location = new System.Drawing.Point(0, 110);
			this.pnlGrid.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.tblLayout.SetRowSpan(this.pnlGrid, 4);
			this.pnlGrid.Size = new System.Drawing.Size(525, 173);
			this.pnlGrid.TabIndex = 15;
			// 
			// pnlGridHdg
			// 
			this.pnlGridHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlGridHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGridHdg.ClipTextForChildControls = true;
			this.pnlGridHdg.ColorBottom = System.Drawing.Color.Empty;
			this.pnlGridHdg.ColorTop = System.Drawing.Color.Empty;
			this.pnlGridHdg.ControlReceivingFocusOnMnemonic = null;
			this.pnlGridHdg.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlGridHdg.DoubleBuffered = true;
			this.pnlGridHdg.DrawOnlyBottomBorder = true;
			this.pnlGridHdg.DrawOnlyTopBorder = false;
			this.pnlGridHdg.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlGridHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlGridHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlGridHdg, null);
			this.locExtender.SetLocalizingId(this.pnlGridHdg, "ProjectSettingsDlg.pnlGridHdg");
			this.pnlGridHdg.Location = new System.Drawing.Point(0, 0);
			this.pnlGridHdg.MakeDark = false;
			this.pnlGridHdg.MnemonicGeneratesClick = false;
			this.pnlGridHdg.Name = "pnlGridHdg";
			this.pnlGridHdg.PaintExplorerBarBackground = false;
			this.pnlGridHdg.Size = new System.Drawing.Size(523, 25);
			this.pnlGridHdg.TabIndex = 0;
			this.pnlGridHdg.Text = "&Data Sources";
			// 
			// tblLayout
			// 
			this.tblLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayout.AutoSize = true;
			this.tblLayout.ColumnCount = 6;
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayout.Controls.Add(this.txtLanguageCode, 1, 2);
			this.tblLayout.Controls.Add(this.lblResearcher, 0, 3);
			this.tblLayout.Controls.Add(this.txtResearcher, 1, 3);
			this.tblLayout.Controls.Add(this.lblProjName, 0, 0);
			this.tblLayout.Controls.Add(this.lblLanguageName, 0, 1);
			this.tblLayout.Controls.Add(this.lblLanguageCode, 0, 2);
			this.tblLayout.Controls.Add(this.txtProjName, 1, 0);
			this.tblLayout.Controls.Add(this.txtLanguageName, 1, 1);
			this.tblLayout.Controls.Add(this.txtComments, 4, 2);
			this.tblLayout.Controls.Add(this.txtSpeaker, 4, 1);
			this.tblLayout.Controls.Add(this.txtTranscriber, 4, 0);
			this.tblLayout.Controls.Add(this.lblTranscriber, 3, 0);
			this.tblLayout.Controls.Add(this.lblSpeaker, 3, 1);
			this.tblLayout.Controls.Add(this.lblComments, 3, 2);
			this.tblLayout.Controls.Add(this.lnkEthnologue, 2, 2);
			this.tblLayout.Controls.Add(this.btnAdd, 5, 4);
			this.tblLayout.Controls.Add(this.btnRemove, 5, 5);
			this.tblLayout.Controls.Add(this.pnlGrid, 0, 4);
			this.tblLayout.Controls.Add(this.btnProperties, 5, 6);
			this.tblLayout.Location = new System.Drawing.Point(11, 13);
			this.tblLayout.Name = "tblLayout";
			this.tblLayout.RowCount = 8;
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayout.Size = new System.Drawing.Size(625, 283);
			this.tblLayout.TabIndex = 0;
			// 
			// _chkMakeFolder
			// 
			this._chkMakeFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._chkMakeFolder.AutoSize = true;
			this._chkMakeFolder.BackColor = System.Drawing.Color.Transparent;
			this._chkMakeFolder.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.locExtender.SetLocalizableToolTip(this._chkMakeFolder, null);
			this.locExtender.SetLocalizationComment(this._chkMakeFolder, null);
			this.locExtender.SetLocalizingId(this._chkMakeFolder, "ProjectSettingsDlg.CreateProjectFolderLabel");
			this._chkMakeFolder.Location = new System.Drawing.Point(83, 91);
			this._chkMakeFolder.Name = "_chkMakeFolder";
			this._chkMakeFolder.Size = new System.Drawing.Size(201, 19);
			this._chkMakeFolder.TabIndex = 2;
			this._chkMakeFolder.Text = "Create new folder for project files";
			this._chkMakeFolder.UseVisualStyleBackColor = false;
			// 
			// ProjectSettingsDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(646, 336);
			this.Controls.Add(this.tblLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ProjectSettingsDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(580, 325);
			this.Name = "ProjectSettingsDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.Text = "Project Settings";
			this.Controls.SetChildIndex(this.tblLayout, 0);
			this.mnuAdd.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.pnlGrid.ResumeLayout(false);
			this.pnlGrid.PerformLayout();
			this.tblLayout.ResumeLayout(false);
			this.tblLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblProjName;
		private System.Windows.Forms.Label lblTranscriber;
		private System.Windows.Forms.Label lblSpeaker;
		private System.Windows.Forms.Label lblComments;
		private System.Windows.Forms.TextBox txtProjName;
		private System.Windows.Forms.TextBox txtTranscriber;
		private System.Windows.Forms.TextBox txtSpeaker;
		private System.Windows.Forms.TextBox txtComments;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.TextBox txtLanguageName;
		private System.Windows.Forms.Label lblLanguageName;
		private System.Windows.Forms.ContextMenuStrip mnuAdd;
		private System.Windows.Forms.ToolStripMenuItem mnuAddOtherDataSource;
		private System.Windows.Forms.ToolStripMenuItem mnuAddFwDataSource;
		private SilTools.SilGrid m_grid;
		private System.Windows.Forms.Button btnProperties;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private System.Windows.Forms.Label lblLanguageCode;
		private System.Windows.Forms.TextBox txtLanguageCode;
		private System.Windows.Forms.LinkLabel lnkEthnologue;
		private System.Windows.Forms.Label lblResearcher;
		private System.Windows.Forms.TextBox txtResearcher;
		private System.Windows.Forms.ToolStripMenuItem mnuAddFw7DataSource;
		private System.Windows.Forms.ToolStripMenuItem mnuAddFw6DataSource;
		private SilPanel pnlGrid;
		private SilGradientPanel pnlGridHdg;
		private System.Windows.Forms.CheckBox _chkMakeFolder;
	}
}