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
			this.cmnuAddFw7DataSource = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnuAddFw6DataSource = new System.Windows.Forms.ToolStripMenuItem();
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
			this.cmnuAdd.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.pnlGrid.SuspendLayout();
			this.tblLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblProjName
			// 
			resources.ApplyResources(this.lblProjName, "lblProjName");
			this.locExtender.SetLocalizableToolTip(this.lblProjName, null);
			this.locExtender.SetLocalizationComment(this.lblProjName, null);
			this.locExtender.SetLocalizingId(this.lblProjName, "ProjectSettingsDlg.lblProjName");
			this.lblProjName.Name = "lblProjName";
			// 
			// lblTranscriber
			// 
			resources.ApplyResources(this.lblTranscriber, "lblTranscriber");
			this.locExtender.SetLocalizableToolTip(this.lblTranscriber, null);
			this.locExtender.SetLocalizationComment(this.lblTranscriber, null);
			this.locExtender.SetLocalizingId(this.lblTranscriber, "ProjectSettingsDlg.lblTranscriber");
			this.lblTranscriber.Name = "lblTranscriber";
			// 
			// lblSpeaker
			// 
			resources.ApplyResources(this.lblSpeaker, "lblSpeaker");
			this.locExtender.SetLocalizableToolTip(this.lblSpeaker, null);
			this.locExtender.SetLocalizationComment(this.lblSpeaker, null);
			this.locExtender.SetLocalizingId(this.lblSpeaker, "ProjectSettingsDlg.lblSpeaker");
			this.lblSpeaker.Name = "lblSpeaker";
			// 
			// lblComments
			// 
			resources.ApplyResources(this.lblComments, "lblComments");
			this.locExtender.SetLocalizableToolTip(this.lblComments, null);
			this.locExtender.SetLocalizationComment(this.lblComments, null);
			this.locExtender.SetLocalizingId(this.lblComments, "ProjectSettingsDlg.lblComments");
			this.lblComments.Name = "lblComments";
			// 
			// txtProjName
			// 
			resources.ApplyResources(this.txtProjName, "txtProjName");
			this.tblLayout.SetColumnSpan(this.txtProjName, 2);
			this.locExtender.SetLocalizableToolTip(this.txtProjName, null);
			this.locExtender.SetLocalizationComment(this.txtProjName, null);
			this.locExtender.SetLocalizingId(this.txtProjName, "ProjectSettingsDlg.txtProjName");
			this.txtProjName.Name = "txtProjName";
			this.txtProjName.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// txtTranscriber
			// 
			resources.ApplyResources(this.txtTranscriber, "txtTranscriber");
			this.tblLayout.SetColumnSpan(this.txtTranscriber, 2);
			this.locExtender.SetLocalizableToolTip(this.txtTranscriber, null);
			this.locExtender.SetLocalizationComment(this.txtTranscriber, null);
			this.locExtender.SetLocalizingId(this.txtTranscriber, "ProjectSettingsDlg.txtTranscriber");
			this.txtTranscriber.Name = "txtTranscriber";
			this.txtTranscriber.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// txtSpeaker
			// 
			resources.ApplyResources(this.txtSpeaker, "txtSpeaker");
			this.tblLayout.SetColumnSpan(this.txtSpeaker, 2);
			this.locExtender.SetLocalizableToolTip(this.txtSpeaker, null);
			this.locExtender.SetLocalizationComment(this.txtSpeaker, null);
			this.locExtender.SetLocalizingId(this.txtSpeaker, "ProjectSettingsDlg.txtSpeaker");
			this.txtSpeaker.Name = "txtSpeaker";
			this.txtSpeaker.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// txtComments
			// 
			resources.ApplyResources(this.txtComments, "txtComments");
			this.tblLayout.SetColumnSpan(this.txtComments, 2);
			this.locExtender.SetLocalizableToolTip(this.txtComments, null);
			this.locExtender.SetLocalizationComment(this.txtComments, null);
			this.locExtender.SetLocalizingId(this.txtComments, "ProjectSettingsDlg.txtComments");
			this.txtComments.Name = "txtComments";
			this.tblLayout.SetRowSpan(this.txtComments, 2);
			// 
			// btnAdd
			// 
			resources.ApplyResources(this.btnAdd, "btnAdd");
			this.btnAdd.Image = global::SIL.Pa.Properties.Resources.kimidButtonDropDownArrow;
			this.locExtender.SetLocalizableToolTip(this.btnAdd, null);
			this.locExtender.SetLocalizationComment(this.btnAdd, null);
			this.locExtender.SetLocalizingId(this.btnAdd, "ProjectSettingsDlg.btnAdd");
			this.btnAdd.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.HandleAddButtonClick);
			// 
			// btnRemove
			// 
			resources.ApplyResources(this.btnRemove, "btnRemove");
			this.locExtender.SetLocalizableToolTip(this.btnRemove, null);
			this.locExtender.SetLocalizationComment(this.btnRemove, null);
			this.locExtender.SetLocalizingId(this.btnRemove, "ProjectSettingsDlg.btnRemove");
			this.btnRemove.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.HandleRemoveButtonClick);
			// 
			// btnCustomFields
			// 
			resources.ApplyResources(this.btnCustomFields, "btnCustomFields");
			this.locExtender.SetLocalizableToolTip(this.btnCustomFields, null);
			this.locExtender.SetLocalizationComment(this.btnCustomFields, null);
			this.locExtender.SetLocalizingId(this.btnCustomFields, "ProjectSettingsDlg.btnCustomFields");
			this.btnCustomFields.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnCustomFields.Name = "btnCustomFields";
			this.btnCustomFields.UseVisualStyleBackColor = true;
			this.btnCustomFields.Click += new System.EventHandler(this.HandleCustomFieldsButtonClick);
			// 
			// btnProperties
			// 
			resources.ApplyResources(this.btnProperties, "btnProperties");
			this.locExtender.SetLocalizableToolTip(this.btnProperties, null);
			this.locExtender.SetLocalizationComment(this.btnProperties, null);
			this.locExtender.SetLocalizingId(this.btnProperties, "ProjectSettingsDlg.btnProperties");
			this.btnProperties.MinimumSize = new System.Drawing.Size(95, 26);
			this.btnProperties.Name = "btnProperties";
			this.btnProperties.UseVisualStyleBackColor = true;
			this.btnProperties.Click += new System.EventHandler(this.HandlePropertyButtonClick);
			// 
			// txtLanguageName
			// 
			resources.ApplyResources(this.txtLanguageName, "txtLanguageName");
			this.tblLayout.SetColumnSpan(this.txtLanguageName, 2);
			this.locExtender.SetLocalizableToolTip(this.txtLanguageName, null);
			this.locExtender.SetLocalizationComment(this.txtLanguageName, null);
			this.locExtender.SetLocalizingId(this.txtLanguageName, "ProjectSettingsDlg.txtLanguageName");
			this.txtLanguageName.Name = "txtLanguageName";
			this.txtLanguageName.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// lblLanguageName
			// 
			resources.ApplyResources(this.lblLanguageName, "lblLanguageName");
			this.locExtender.SetLocalizableToolTip(this.lblLanguageName, null);
			this.locExtender.SetLocalizationComment(this.lblLanguageName, null);
			this.locExtender.SetLocalizingId(this.lblLanguageName, "ProjectSettingsDlg.lblLanguageName");
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
			this.locExtender.SetLocalizationComment(this.cmnuAddOtherDataSource, null);
			this.locExtender.SetLocalizingId(this.cmnuAddOtherDataSource, ".cmnuAddOtherDataSource");
			this.cmnuAddOtherDataSource.Name = "cmnuAddOtherDataSource";
			resources.ApplyResources(this.cmnuAddOtherDataSource, "cmnuAddOtherDataSource");
			this.cmnuAddOtherDataSource.Click += new System.EventHandler(this.HandleAddOtherDataSourceClick);
			// 
			// cmnuAddFwDataSource
			// 
			this.cmnuAddFwDataSource.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuAddFw7DataSource,
            this.cmnuAddFw6DataSource});
			this.locExtender.SetLocalizableToolTip(this.cmnuAddFwDataSource, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddFwDataSource, null);
			this.locExtender.SetLocalizingId(this.cmnuAddFwDataSource, ".cmnuAddFwDataSource");
			this.cmnuAddFwDataSource.Name = "cmnuAddFwDataSource";
			resources.ApplyResources(this.cmnuAddFwDataSource, "cmnuAddFwDataSource");
			// 
			// cmnuAddFw7DataSource
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddFw7DataSource, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddFw7DataSource, null);
			this.locExtender.SetLocalizingId(this.cmnuAddFw7DataSource, ".cmnuAddFw7DataSource");
			this.cmnuAddFw7DataSource.Name = "cmnuAddFw7DataSource";
			resources.ApplyResources(this.cmnuAddFw7DataSource, "cmnuAddFw7DataSource");
			this.cmnuAddFw7DataSource.Click += new System.EventHandler(this.HandleAddFw7DataSourceClick);
			// 
			// cmnuAddFw6DataSource
			// 
			this.locExtender.SetLocalizableToolTip(this.cmnuAddFw6DataSource, null);
			this.locExtender.SetLocalizationComment(this.cmnuAddFw6DataSource, null);
			this.locExtender.SetLocalizingId(this.cmnuAddFw6DataSource, ".cmnuAddFw6DataSource");
			this.cmnuAddFw6DataSource.Name = "cmnuAddFw6DataSource";
			resources.ApplyResources(this.cmnuAddFw6DataSource, "cmnuAddFw6DataSource");
			this.cmnuAddFw6DataSource.Click += new System.EventHandler(this.HandleAddFw6DataSourceClick);
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
			this.m_grid.DrawTextBoxEditControlBorder = false;
			this.m_grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.m_grid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_grid, null);
			this.locExtender.SetLocalizationComment(this.m_grid, null);
			this.locExtender.SetLocalizingId(this.m_grid, "ProjectSettingsDlg.m_grid");
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.PaintHeaderAcrossFullGridWidth = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.m_grid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_grid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.StandardTab = true;
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
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// lblLanguageCode
			// 
			resources.ApplyResources(this.lblLanguageCode, "lblLanguageCode");
			this.locExtender.SetLocalizableToolTip(this.lblLanguageCode, null);
			this.locExtender.SetLocalizationComment(this.lblLanguageCode, null);
			this.locExtender.SetLocalizingId(this.lblLanguageCode, "ProjectSettingsDlg.lblLanguageCode");
			this.lblLanguageCode.Name = "lblLanguageCode";
			// 
			// txtLanguageCode
			// 
			resources.ApplyResources(this.txtLanguageCode, "txtLanguageCode");
			this.locExtender.SetLocalizableToolTip(this.txtLanguageCode, null);
			this.locExtender.SetLocalizationComment(this.txtLanguageCode, null);
			this.locExtender.SetLocalizingId(this.txtLanguageCode, "ProjectSettingsDlg.txtLanguageCode");
			this.txtLanguageCode.Name = "txtLanguageCode";
			this.txtLanguageCode.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// lnkEthnologue
			// 
			this.lnkEthnologue.AutoEllipsis = true;
			resources.ApplyResources(this.lnkEthnologue, "lnkEthnologue");
			this.locExtender.SetLocalizableToolTip(this.lnkEthnologue, null);
			this.locExtender.SetLocalizationComment(this.lnkEthnologue, null);
			this.locExtender.SetLocalizingId(this.lnkEthnologue, "ProjectSettingsDlg.lnkEthnologue");
			this.lnkEthnologue.Name = "lnkEthnologue";
			this.lnkEthnologue.TabStop = true;
			this.lnkEthnologue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleEthnologueLinkClicked);
			// 
			// txtResearcher
			// 
			resources.ApplyResources(this.txtResearcher, "txtResearcher");
			this.tblLayout.SetColumnSpan(this.txtResearcher, 2);
			this.locExtender.SetLocalizableToolTip(this.txtResearcher, null);
			this.locExtender.SetLocalizationComment(this.txtResearcher, null);
			this.locExtender.SetLocalizingId(this.txtResearcher, "ProjectSettingsDlg.txtResearcher");
			this.txtResearcher.Name = "txtResearcher";
			this.txtResearcher.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// lblResearcher
			// 
			resources.ApplyResources(this.lblResearcher, "lblResearcher");
			this.locExtender.SetLocalizableToolTip(this.lblResearcher, null);
			this.locExtender.SetLocalizationComment(this.lblResearcher, null);
			this.locExtender.SetLocalizingId(this.lblResearcher, "ProjectSettingsDlg.lblResearcher");
			this.lblResearcher.Name = "lblResearcher";
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.tblLayout.SetColumnSpan(this.pnlGrid, 5);
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this.m_grid);
			this.pnlGrid.Controls.Add(this.pnlGridHdg);
			resources.ApplyResources(this.pnlGrid, "pnlGrid");
			this.pnlGrid.DoubleBuffered = true;
			this.pnlGrid.DrawOnlyBottomBorder = false;
			this.pnlGrid.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizingId(this.pnlGrid, "ProjectSettingsDlg.pnlGrid");
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.tblLayout.SetRowSpan(this.pnlGrid, 4);
			// 
			// pnlGridHdg
			// 
			this.pnlGridHdg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlGridHdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGridHdg.ClipTextForChildControls = true;
			this.pnlGridHdg.ColorBottom = System.Drawing.Color.Empty;
			this.pnlGridHdg.ColorTop = System.Drawing.Color.Empty;
			this.pnlGridHdg.ControlReceivingFocusOnMnemonic = null;
			resources.ApplyResources(this.pnlGridHdg, "pnlGridHdg");
			this.pnlGridHdg.DoubleBuffered = true;
			this.pnlGridHdg.DrawOnlyBottomBorder = true;
			this.pnlGridHdg.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlGridHdg, null);
			this.locExtender.SetLocalizationComment(this.pnlGridHdg, null);
			this.locExtender.SetLocalizingId(this.pnlGridHdg, "ProjectSettingsDlg.pnlGridHdg");
			this.pnlGridHdg.MakeDark = false;
			this.pnlGridHdg.MnemonicGeneratesClick = false;
			this.pnlGridHdg.Name = "pnlGridHdg";
			this.pnlGridHdg.PaintExplorerBarBackground = false;
			// 
			// tblLayout
			// 
			resources.ApplyResources(this.tblLayout, "tblLayout");
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
			this.tblLayout.Controls.Add(this.btnCustomFields, 5, 7);
			this.tblLayout.Name = "tblLayout";
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
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.pnlGrid.ResumeLayout(false);
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
		private System.Windows.Forms.Button btnCustomFields;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.TextBox txtLanguageName;
		private System.Windows.Forms.Label lblLanguageName;
		private System.Windows.Forms.ContextMenuStrip cmnuAdd;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddOtherDataSource;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddFwDataSource;
		private SilTools.SilGrid m_grid;
		private System.Windows.Forms.Button btnProperties;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private System.Windows.Forms.Label lblLanguageCode;
		private System.Windows.Forms.TextBox txtLanguageCode;
		private System.Windows.Forms.LinkLabel lnkEthnologue;
		private System.Windows.Forms.Label lblResearcher;
		private System.Windows.Forms.TextBox txtResearcher;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddFw7DataSource;
		private System.Windows.Forms.ToolStripMenuItem cmnuAddFw6DataSource;
		private SilPanel pnlGrid;
		private SilGradientPanel pnlGridHdg;
	}
}