using SilUtils.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class ProjectSettingsDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectSettingsDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
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
			this.btnCustomFields = new System.Windows.Forms.Button();
			this.btnProperties = new System.Windows.Forms.Button();
			this.txtLanguageName = new System.Windows.Forms.TextBox();
			this.lblLanguageName = new System.Windows.Forms.Label();
			this.cmnuAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuAddOtherDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddFwDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlGrid = new SilUtils.Controls.SilPanel();
			this.m_grid = new SilUtils.SilGrid();
			this.pnlGridHdg = new SilUtils.Controls.SilGradientPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.lblLanguageCode = new System.Windows.Forms.Label();
			this.txtLanguageCode = new System.Windows.Forms.TextBox();
			this.lnkEthnologue = new System.Windows.Forms.LinkLabel();
			this.txtResearcher = new System.Windows.Forms.TextBox();
			this.lblResearcher = new System.Windows.Forms.Label();
			this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
			this.pnlDataSourcesMngmnt = new System.Windows.Forms.Panel();
			this.pnlLanguageCode = new System.Windows.Forms.Panel();
			this.cmnuAdd.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tblLayout.SuspendLayout();
			this.pnlDataSourcesMngmnt.SuspendLayout();
			this.pnlLanguageCode.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizationPriority(this.btnCancel, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCancel, "Localized in base class");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "Localized in base class");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "Localized in base class");
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// lblProjName
			// 
			resources.ApplyResources(this.lblProjName, "lblProjName");
			this.locExtender.SetLocalizableToolTip(this.lblProjName, null);
			this.locExtender.SetLocalizationComment(this.lblProjName, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblProjName, "ProjectSettingsDlg.lblProjName");
			this.lblProjName.Name = "lblProjName";
			// 
			// lblTranscriber
			// 
			resources.ApplyResources(this.lblTranscriber, "lblTranscriber");
			this.locExtender.SetLocalizableToolTip(this.lblTranscriber, null);
			this.locExtender.SetLocalizationComment(this.lblTranscriber, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblTranscriber, "ProjectSettingsDlg.lblTranscriber");
			this.lblTranscriber.Name = "lblTranscriber";
			// 
			// lblSpeaker
			// 
			resources.ApplyResources(this.lblSpeaker, "lblSpeaker");
			this.locExtender.SetLocalizableToolTip(this.lblSpeaker, null);
			this.locExtender.SetLocalizationComment(this.lblSpeaker, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblSpeaker, "ProjectSettingsDlg.lblSpeaker");
			this.lblSpeaker.Name = "lblSpeaker";
			// 
			// lblComments
			// 
			resources.ApplyResources(this.lblComments, "lblComments");
			this.locExtender.SetLocalizableToolTip(this.lblComments, null);
			this.locExtender.SetLocalizationComment(this.lblComments, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblComments, "ProjectSettingsDlg.lblComments");
			this.lblComments.Name = "lblComments";
			// 
			// txtProjName
			// 
			resources.ApplyResources(this.txtProjName, "txtProjName");
			this.locExtender.SetLocalizableToolTip(this.txtProjName, null);
			this.locExtender.SetLocalizationComment(this.txtProjName, null);
			this.locExtender.SetLocalizationPriority(this.txtProjName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtProjName, "ProjectSettingsDlg.txtProjName");
			this.txtProjName.Name = "txtProjName";
			this.txtProjName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtTranscriber
			// 
			resources.ApplyResources(this.txtTranscriber, "txtTranscriber");
			this.locExtender.SetLocalizableToolTip(this.txtTranscriber, null);
			this.locExtender.SetLocalizationComment(this.txtTranscriber, null);
			this.locExtender.SetLocalizationPriority(this.txtTranscriber, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtTranscriber, "ProjectSettingsDlg.txtTranscriber");
			this.txtTranscriber.Name = "txtTranscriber";
			this.txtTranscriber.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtSpeaker
			// 
			resources.ApplyResources(this.txtSpeaker, "txtSpeaker");
			this.locExtender.SetLocalizableToolTip(this.txtSpeaker, null);
			this.locExtender.SetLocalizationComment(this.txtSpeaker, null);
			this.locExtender.SetLocalizationPriority(this.txtSpeaker, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtSpeaker, "ProjectSettingsDlg.txtSpeaker");
			this.txtSpeaker.Name = "txtSpeaker";
			this.txtSpeaker.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtComments
			// 
			resources.ApplyResources(this.txtComments, "txtComments");
			this.locExtender.SetLocalizableToolTip(this.txtComments, null);
			this.locExtender.SetLocalizationComment(this.txtComments, null);
			this.locExtender.SetLocalizationPriority(this.txtComments, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtComments, "ProjectSettingsDlg.txtComments");
			this.txtComments.Name = "txtComments";
			this.tblLayout.SetRowSpan(this.txtComments, 2);
			this.txtComments.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Image = global::SIL.Pa.Properties.Resources.kimidButtonDropDownArrow;
			this.locExtender.SetLocalizableToolTip(this.btnAdd, "Add a new data source");
			this.locExtender.SetLocalizationComment(this.btnAdd, "Button on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.btnAdd, "ProjectSettingsDlg.btnAdd");
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			resources.ApplyResources(this.btnRemove, "btnRemove");
			this.locExtender.SetLocalizableToolTip(this.btnRemove, "Remove the selected data source(s)");
			this.locExtender.SetLocalizationComment(this.btnRemove, "Button on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.btnRemove, "ProjectSettingsDlg.btnRemove");
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnCustomFields
			// 
			resources.ApplyResources(this.btnCustomFields, "btnCustomFields");
			this.locExtender.SetLocalizableToolTip(this.btnCustomFields, "Define custom fields");
			this.locExtender.SetLocalizationComment(this.btnCustomFields, "Button on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.btnCustomFields, "ProjectSettingsDlg.btnCustomFields");
			this.btnCustomFields.Name = "btnCustomFields";
			this.btnCustomFields.UseVisualStyleBackColor = true;
			this.btnCustomFields.Click += new System.EventHandler(this.btnCustomFields_Click);
			// 
			// btnProperties
			// 
			resources.ApplyResources(this.btnProperties, "btnProperties");
			this.locExtender.SetLocalizableToolTip(this.btnProperties, "Modify data source properties");
			this.locExtender.SetLocalizationComment(this.btnProperties, "Button on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.btnProperties, "ProjectSettingsDlg.btnProperties");
			this.btnProperties.Name = "btnProperties";
			this.btnProperties.UseVisualStyleBackColor = true;
			this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
			// 
			// txtLanguageName
			// 
			resources.ApplyResources(this.txtLanguageName, "txtLanguageName");
			this.locExtender.SetLocalizableToolTip(this.txtLanguageName, null);
			this.locExtender.SetLocalizationComment(this.txtLanguageName, null);
			this.locExtender.SetLocalizationPriority(this.txtLanguageName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtLanguageName, "ProjectSettingsDlg.txtLanguage");
			this.txtLanguageName.Name = "txtLanguageName";
			this.txtLanguageName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblLanguageName
			// 
			resources.ApplyResources(this.lblLanguageName, "lblLanguageName");
			this.locExtender.SetLocalizableToolTip(this.lblLanguageName, null);
			this.locExtender.SetLocalizationComment(this.lblLanguageName, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblLanguageName, "ProjectSettingsDlg.lblLanguage");
			this.lblLanguageName.Name = "lblLanguageName";
			// 
			// cmnuAdd
			// 
			this.cmnuAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuAddOtherDataSource,
            this.cmnuAddFwDataSource});
			this.locExtender.SetLocalizableToolTip(this.cmnuAdd, null);
			this.locExtender.SetLocalizationComment(this.cmnuAdd, null);
			this.locExtender.SetLocalizingId(this.cmnuAdd, "cmnuAdd.cmnuAdd");
			this.cmnuAdd.Name = "cmnuAdd";
			this.cmnuAdd.ShowImageMargin = false;
			resources.ApplyResources(this.cmnuAdd, "cmnuAdd");
			// 
			// cmnuAddOtherDataSource
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddOtherDataSource, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddOtherDataSource, "On add button\'s drop-down on the project settings dialog box.");
			this.locExtender.SetLocalizingId(this.cmnuAddOtherDataSource, "ProjectSettingsDlg.cmnuAddOtherDataSource");
			this.cmnuAddOtherDataSource.Name = "cmnuAddOtherDataSource";
			resources.ApplyResources(this.cmnuAddOtherDataSource, "cmnuAddOtherDataSource");
			this.cmnuAddOtherDataSource.Click += new System.EventHandler(this.cmnuAddOtherDataSource_Click);
			// 
			// cmnuAddFwDataSource
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddFwDataSource, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddFwDataSource, "On add button\'s drop-down on the project settings dialog box.");
			this.locExtender.SetLocalizingId(this.cmnuAddFwDataSource, "ProjectSettingsDlg.cmnuAddFwDataSource");
			this.cmnuAddFwDataSource.Name = "cmnuAddFwDataSource";
			resources.ApplyResources(this.cmnuAddFwDataSource, "cmnuAddFwDataSource");
			this.cmnuAddFwDataSource.Click += new System.EventHandler(this.cmnuAddFwDataSource_Click);
			// 
			// pnlGrid
			// 
			resources.ApplyResources(this.pnlGrid, "pnlGrid");
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this.m_grid);
			this.pnlGrid.Controls.Add(this.pnlGridHdg);
			this.pnlGrid.DoubleBuffered = false;
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizingId(this.pnlGrid, "ProjectSettingsDlg.pnlGrid");
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
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
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			resources.ApplyResources(this.m_grid, "m_grid");
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_grid, null);
			this.locExtender.SetLocalizationComment(this.m_grid, null);
			this.locExtender.SetLocalizationPriority(this.m_grid, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_grid, "ProjectSettingsDlg.m_grid");
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.PaintHeaderAcrossFullGridWidth = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.WaterMark = "!";
			this.m_grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.m_grid_CellPainting);
			this.m_grid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseDoubleClick);
			this.m_grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_grid_KeyDown);
			// 
			// pnlGridHdg
			// 
			this.pnlGridHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(185)))), ((int)(((byte)(127)))));
			this.pnlGridHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGridHdg.ClipTextForChildControls = true;
			this.pnlGridHdg.ControlReceivingFocusOnMnemonic = this.m_grid;
			resources.ApplyResources(this.pnlGridHdg, "pnlGridHdg");
			this.pnlGridHdg.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlGridHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlGridHdg, "Heading above list of data sources on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.pnlGridHdg, "ProjectSettingsDlg.pnlGridHdg");
			this.pnlGridHdg.MakeDark = false;
			this.pnlGridHdg.MnemonicGeneratesClick = true;
			this.pnlGridHdg.Name = "pnlGridHdg";
			this.pnlGridHdg.PaintExplorerBarBackground = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// lblLanguageCode
			// 
			resources.ApplyResources(this.lblLanguageCode, "lblLanguageCode");
			this.locExtender.SetLocalizableToolTip(this.lblLanguageCode, null);
			this.locExtender.SetLocalizationComment(this.lblLanguageCode, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblLanguageCode, "ProjectSettingsDlg.lblLanguageCode");
			this.lblLanguageCode.Name = "lblLanguageCode";
			// 
			// txtLanguageCode
			// 
			resources.ApplyResources(this.txtLanguageCode, "txtLanguageCode");
			this.locExtender.SetLocalizableToolTip(this.txtLanguageCode, null);
			this.locExtender.SetLocalizationComment(this.txtLanguageCode, null);
			this.locExtender.SetLocalizationPriority(this.txtLanguageCode, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtLanguageCode, "ProjectSettingsDlg.txtLanguage");
			this.txtLanguageCode.Name = "txtLanguageCode";
			this.txtLanguageCode.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lnkEthnologue
			// 
			resources.ApplyResources(this.lnkEthnologue, "lnkEthnologue");
			this.lnkEthnologue.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.lnkEthnologue, "Lookup Language on Ethnologue Website");
			this.locExtender.SetLocalizationComment(this.lnkEthnologue, null);
			this.locExtender.SetLocalizingId(this.lnkEthnologue, "ProjectSettingsDlg.lnkEthnologue");
			this.lnkEthnologue.Name = "lnkEthnologue";
			this.lnkEthnologue.TabStop = true;
			this.lnkEthnologue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEthnologue_LinkClicked);
			// 
			// txtResearcher
			// 
			resources.ApplyResources(this.txtResearcher, "txtResearcher");
			this.locExtender.SetLocalizableToolTip(this.txtResearcher, null);
			this.locExtender.SetLocalizationComment(this.txtResearcher, null);
			this.locExtender.SetLocalizationPriority(this.txtResearcher, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtResearcher, "ProjectSettingsDlg.txtLanguage");
			this.txtResearcher.Name = "txtResearcher";
			this.txtResearcher.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblResearcher
			// 
			resources.ApplyResources(this.lblResearcher, "lblResearcher");
			this.locExtender.SetLocalizableToolTip(this.lblResearcher, null);
			this.locExtender.SetLocalizationComment(this.lblResearcher, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblResearcher, "ProjectSettingsDlg.lblResearcher");
			this.lblResearcher.Name = "lblResearcher";
			// 
			// tblLayout
			// 
			resources.ApplyResources(this.tblLayout, "tblLayout");
			this.tblLayout.Controls.Add(this.lblResearcher, 0, 3);
			this.tblLayout.Controls.Add(this.txtResearcher, 1, 3);
			this.tblLayout.Controls.Add(this.pnlDataSourcesMngmnt, 0, 4);
			this.tblLayout.Controls.Add(this.lblProjName, 0, 0);
			this.tblLayout.Controls.Add(this.lblLanguageName, 0, 1);
			this.tblLayout.Controls.Add(this.pnlLanguageCode, 1, 2);
			this.tblLayout.Controls.Add(this.lblLanguageCode, 0, 2);
			this.tblLayout.Controls.Add(this.txtProjName, 1, 0);
			this.tblLayout.Controls.Add(this.txtLanguageName, 1, 1);
			this.tblLayout.Controls.Add(this.lblComments, 2, 2);
			this.tblLayout.Controls.Add(this.txtComments, 3, 2);
			this.tblLayout.Controls.Add(this.txtSpeaker, 3, 1);
			this.tblLayout.Controls.Add(this.lblSpeaker, 2, 1);
			this.tblLayout.Controls.Add(this.lblTranscriber, 2, 0);
			this.tblLayout.Controls.Add(this.txtTranscriber, 3, 0);
			this.tblLayout.Name = "tblLayout";
			// 
			// pnlDataSourcesMngmnt
			// 
			this.tblLayout.SetColumnSpan(this.pnlDataSourcesMngmnt, 4);
			this.pnlDataSourcesMngmnt.Controls.Add(this.pnlGrid);
			this.pnlDataSourcesMngmnt.Controls.Add(this.btnAdd);
			this.pnlDataSourcesMngmnt.Controls.Add(this.btnCustomFields);
			this.pnlDataSourcesMngmnt.Controls.Add(this.btnRemove);
			this.pnlDataSourcesMngmnt.Controls.Add(this.btnProperties);
			resources.ApplyResources(this.pnlDataSourcesMngmnt, "pnlDataSourcesMngmnt");
			this.pnlDataSourcesMngmnt.Name = "pnlDataSourcesMngmnt";
			// 
			// pnlLanguageCode
			// 
			resources.ApplyResources(this.pnlLanguageCode, "pnlLanguageCode");
			this.pnlLanguageCode.Controls.Add(this.txtLanguageCode);
			this.pnlLanguageCode.Controls.Add(this.lnkEthnologue);
			this.pnlLanguageCode.Name = "pnlLanguageCode";
			// 
			// ProjectSettingsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tblLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ProjectSettingsDlg.WindowTitle");
			this.Name = "ProjectSettingsDlg";
			this.Controls.SetChildIndex(this.tblLayout, 0);
			this.cmnuAdd.ResumeLayout(false);
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.tblLayout.ResumeLayout(false);
			this.tblLayout.PerformLayout();
			this.pnlDataSourcesMngmnt.ResumeLayout(false);
			this.pnlLanguageCode.ResumeLayout(false);
			this.pnlLanguageCode.PerformLayout();
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
		private System.Windows.Forms.Button btnCustomFields;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.TextBox txtLanguageName;
		private System.Windows.Forms.Label lblLanguageName;
		private System.Windows.Forms.ContextMenuStrip cmnuAdd;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddOtherDataSource;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddFwDataSource;
		private SilPanel pnlGrid;
		private SilGradientPanel pnlGridHdg;
		private SilUtils.SilGrid m_grid;
		private System.Windows.Forms.Button btnProperties;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private System.Windows.Forms.Label lblLanguageCode;
		private System.Windows.Forms.TextBox txtLanguageCode;
		private System.Windows.Forms.Panel pnlDataSourcesMngmnt;
		private System.Windows.Forms.Panel pnlLanguageCode;
		private System.Windows.Forms.LinkLabel lnkEthnologue;
		private System.Windows.Forms.Label lblResearcher;
		private System.Windows.Forms.TextBox txtResearcher;
	}
}