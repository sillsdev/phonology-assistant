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
			this.txtLanguage = new System.Windows.Forms.TextBox();
			this.lblLanguage = new System.Windows.Forms.Label();
			this.cmnuAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnuAddOtherDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddFwDataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlGrid = new SilUtils.Controls.SilPanel();
			this.m_grid = new SilUtils.SilGrid();
			this.pnlGridHdg = new SilUtils.Controls.SilGradientPanel();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlButtons.SuspendLayout();
			this.cmnuAdd.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			// 
			// btnCancel
			// 
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizingId(this.btnCancel, "ProjectSettingsDlg.btnCancel");
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizingId(this.btnOK, "ProjectSettingsDlg.btnOK");
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizingId(this.btnHelp, "ProjectSettingsDlg.btnHelp");
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
			this.locExtender.SetLocalizableToolTip(this.txtProjName, null);
			this.locExtender.SetLocalizationComment(this.txtProjName, null);
			this.locExtender.SetLocalizationPriority(this.txtProjName, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtProjName, "ProjectSettingsDlg.txtProjName");
			resources.ApplyResources(this.txtProjName, "txtProjName");
			this.txtProjName.Name = "txtProjName";
			this.txtProjName.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtTranscriber
			// 
			this.locExtender.SetLocalizableToolTip(this.txtTranscriber, null);
			this.locExtender.SetLocalizationComment(this.txtTranscriber, null);
			this.locExtender.SetLocalizationPriority(this.txtTranscriber, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtTranscriber, "ProjectSettingsDlg.txtTranscriber");
			resources.ApplyResources(this.txtTranscriber, "txtTranscriber");
			this.txtTranscriber.Name = "txtTranscriber";
			this.txtTranscriber.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtSpeaker
			// 
			resources.ApplyResources(this.txtSpeaker, "txtSpeaker");
			this.locExtender.SetLocalizableToolTip(this.txtSpeaker, null);
			this.locExtender.SetLocalizationComment(this.txtSpeaker, null);
			this.locExtender.SetLocalizationPriority(this.txtSpeaker, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtSpeaker, "ProjectSettingsDlg.txtSpeaker");
			this.txtSpeaker.Name = "txtSpeaker";
			this.txtSpeaker.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// txtComments
			// 
			resources.ApplyResources(this.txtComments, "txtComments");
			this.locExtender.SetLocalizableToolTip(this.txtComments, null);
			this.locExtender.SetLocalizationComment(this.txtComments, null);
			this.locExtender.SetLocalizationPriority(this.txtComments, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtComments, "ProjectSettingsDlg.txtComments");
			this.txtComments.Name = "txtComments";
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
			// txtLanguage
			// 
			this.locExtender.SetLocalizableToolTip(this.txtLanguage, null);
			this.locExtender.SetLocalizationComment(this.txtLanguage, null);
			this.locExtender.SetLocalizationPriority(this.txtLanguage, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtLanguage, "ProjectSettingsDlg.txtLanguage");
			resources.ApplyResources(this.txtLanguage, "txtLanguage");
			this.txtLanguage.Name = "txtLanguage";
			this.txtLanguage.TextChanged += new System.EventHandler(this.HandleTextChanged);
			// 
			// lblLanguage
			// 
			resources.ApplyResources(this.lblLanguage, "lblLanguage");
			this.locExtender.SetLocalizableToolTip(this.lblLanguage, null);
			this.locExtender.SetLocalizationComment(this.lblLanguage, "Label on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.lblLanguage, "ProjectSettingsDlg.lblLanguage");
			this.lblLanguage.Name = "lblLanguage";
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
			this.locExtender.SetLocalizationPriority(this.m_grid, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_grid, "ProjectSettingsDlg.m_grid");
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
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
			this.pnlGridHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlGridHdg, "pnlGridHdg");
			this.pnlGridHdg.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this.pnlGridHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlGridHdg, "Heading above list of data sources on project settings dialog box.");
			this.locExtender.SetLocalizingId(this.pnlGridHdg, "ProjectSettingsDlg.pnlGridHdg");
			this.pnlGridHdg.MakeDark = false;
			this.pnlGridHdg.MnemonicGeneratesClick = false;
			this.pnlGridHdg.Name = "pnlGridHdg";
			this.pnlGridHdg.PaintExplorerBarBackground = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// ProjectSettingsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnCustomFields);
			this.Controls.Add(this.btnProperties);
			this.Controls.Add(this.pnlGrid);
			this.Controls.Add(this.txtLanguage);
			this.Controls.Add(this.lblLanguage);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.txtComments);
			this.Controls.Add(this.txtSpeaker);
			this.Controls.Add(this.txtTranscriber);
			this.Controls.Add(this.txtProjName);
			this.Controls.Add(this.lblComments);
			this.Controls.Add(this.lblSpeaker);
			this.Controls.Add(this.lblTranscriber);
			this.Controls.Add(this.lblProjName);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ProjectSettingsDlg.WindowTitle");
			this.Name = "ProjectSettingsDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.lblProjName, 0);
			this.Controls.SetChildIndex(this.lblTranscriber, 0);
			this.Controls.SetChildIndex(this.lblSpeaker, 0);
			this.Controls.SetChildIndex(this.lblComments, 0);
			this.Controls.SetChildIndex(this.txtProjName, 0);
			this.Controls.SetChildIndex(this.txtTranscriber, 0);
			this.Controls.SetChildIndex(this.txtSpeaker, 0);
			this.Controls.SetChildIndex(this.txtComments, 0);
			this.Controls.SetChildIndex(this.btnAdd, 0);
			this.Controls.SetChildIndex(this.btnRemove, 0);
			this.Controls.SetChildIndex(this.lblLanguage, 0);
			this.Controls.SetChildIndex(this.txtLanguage, 0);
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.Controls.SetChildIndex(this.btnProperties, 0);
			this.Controls.SetChildIndex(this.btnCustomFields, 0);
			this.pnlButtons.ResumeLayout(false);
			this.cmnuAdd.ResumeLayout(false);
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
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
		private System.Windows.Forms.TextBox txtLanguage;
		private System.Windows.Forms.Label lblLanguage;
		private System.Windows.Forms.ContextMenuStrip cmnuAdd;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddOtherDataSource;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddFwDataSource;
		private SilPanel pnlGrid;
		private SilGradientPanel pnlGridHdg;
		private SilUtils.SilGrid m_grid;
		private System.Windows.Forms.Button btnProperties;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}