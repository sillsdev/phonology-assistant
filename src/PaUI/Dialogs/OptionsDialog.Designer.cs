using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using SIL.Pa.Resources;

namespace SIL.Pa.UI.Dialogs
{
	public partial class OptionsDlg
	{
		private TabControl tabOptions;
		private TabPage tpgFonts;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDlg));
			this.tabOptions = new System.Windows.Forms.TabControl();
			this.tpgWordLists = new System.Windows.Forms.TabPage();
			this.grpColSettings = new System.Windows.Forms.GroupBox();
			this.fldSelGridWrdList = new SIL.Pa.UI.Controls.FieldSelectorGrid();
			this.btnMoveColDown = new System.Windows.Forms.Button();
			this.btnMoveColUp = new System.Windows.Forms.Button();
			this.lblShowColumns = new System.Windows.Forms.Label();
			this.grpColChanges = new System.Windows.Forms.GroupBox();
			this.lblExplanation = new System.Windows.Forms.Label();
			this.chkSaveReorderedColumns = new System.Windows.Forms.CheckBox();
			this.chkSaveColHdrHeight = new System.Windows.Forms.CheckBox();
			this.chkSaveColWidths = new System.Windows.Forms.CheckBox();
			this.nudMaxEticColWidth = new System.Windows.Forms.NumericUpDown();
			this.chkAutoAdjustPhoneticCol = new System.Windows.Forms.CheckBox();
			this.grpGridLines = new System.Windows.Forms.GroupBox();
			this.rbGridLinesHorizontal = new System.Windows.Forms.RadioButton();
			this.rbGridLinesVertical = new System.Windows.Forms.RadioButton();
			this.rbGridLinesBoth = new System.Windows.Forms.RadioButton();
			this.rbGridLinesNone = new System.Windows.Forms.RadioButton();
			this.tpgRecView = new System.Windows.Forms.TabPage();
			this.grpFieldSettings = new System.Windows.Forms.GroupBox();
			this.fldSelGridRecView = new SIL.Pa.UI.Controls.FieldSelectorGrid();
			this.lblShowFields = new System.Windows.Forms.Label();
			this.btnMoveRecVwFldDown = new System.Windows.Forms.Button();
			this.btnMoveRecVwFldUp = new System.Windows.Forms.Button();
			this.tpgFindPhones = new System.Windows.Forms.TabPage();
			this.lblShowDiamondPattern = new System.Windows.Forms.Label();
			this.chkShowDiamondPattern = new System.Windows.Forms.CheckBox();
			this.grpClassSettings = new System.Windows.Forms.GroupBox();
			this.rdoClassMembers = new System.Windows.Forms.RadioButton();
			this.rdoClassName = new System.Windows.Forms.RadioButton();
			this.lblClassDisplayBehavior = new System.Windows.Forms.Label();
			this.tpgCVPatterns = new System.Windows.Forms.TabPage();
			this.grpDisplayChars = new System.Windows.Forms.GroupBox();
			this.lblExampleDesc2 = new System.Windows.Forms.Label();
			this.lblExampleCV = new System.Windows.Forms.Label();
			this.txtCustomChars = new System.Windows.Forms.TextBox();
			this.lblExampleCVCV = new System.Windows.Forms.Label();
			this.lblInstruction = new System.Windows.Forms.Label();
			this.txtExampleInput = new System.Windows.Forms.TextBox();
			this.lblExampleDesc1 = new System.Windows.Forms.Label();
			this.chkTone = new System.Windows.Forms.CheckBox();
			this.grpTone = new System.Windows.Forms.GroupBox();
			this.pnlTone = new System.Windows.Forms.Panel();
			this.tonePicker = new SIL.Pa.UI.Controls.CharPicker();
			this.chkLength = new System.Windows.Forms.CheckBox();
			this.grpLength = new System.Windows.Forms.GroupBox();
			this.pnlLength = new System.Windows.Forms.Panel();
			this.lengthPicker = new SIL.Pa.UI.Controls.CharPicker();
			this.chkStress = new System.Windows.Forms.CheckBox();
			this.grpStress = new System.Windows.Forms.GroupBox();
			this.pnlStress = new System.Windows.Forms.Panel();
			this.stressPicker = new SIL.Pa.UI.Controls.CharPicker();
			this.tpgSorting = new System.Windows.Forms.TabPage();
			this.lblSaveManual = new System.Windows.Forms.Label();
			this.grpColSortOptions = new System.Windows.Forms.GroupBox();
			this.m_sortingGrid = new SilUtils.SilGrid();
			this.btnMoveSortFieldUp = new System.Windows.Forms.Button();
			this.lblSortFldsHdr = new System.Windows.Forms.Label();
			this.btnMoveSortFieldDown = new System.Windows.Forms.Button();
			this.chkSaveManual = new System.Windows.Forms.CheckBox();
			this.lblSortInfo = new System.Windows.Forms.Label();
			this.grpPhoneticSortOptions = new System.Windows.Forms.GroupBox();
			this.phoneticSortOptions = new SIL.Pa.UI.Controls.SortOptionsDropDown();
			this.lblListType = new System.Windows.Forms.Label();
			this.cboListType = new System.Windows.Forms.ComboBox();
			this.tpgFonts = new System.Windows.Forms.TabPage();
			this.tpgColors = new System.Windows.Forms.TabPage();
			this.tpgUI = new System.Windows.Forms.TabPage();
			this.lblUILanguage = new System.Windows.Forms.Label();
			this.picSaveInfo = new System.Windows.Forms.PictureBox();
			this.lblSaveInfo = new System.Windows.Forms.Label();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.cboUILanguage = new System.Windows.Forms.ComboBox();
			this.pnlButtons.SuspendLayout();
			this.tabOptions.SuspendLayout();
			this.tpgWordLists.SuspendLayout();
			this.grpColSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridWrdList)).BeginInit();
			this.grpColChanges.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxEticColWidth)).BeginInit();
			this.grpGridLines.SuspendLayout();
			this.tpgRecView.SuspendLayout();
			this.grpFieldSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridRecView)).BeginInit();
			this.tpgFindPhones.SuspendLayout();
			this.grpClassSettings.SuspendLayout();
			this.tpgCVPatterns.SuspendLayout();
			this.grpDisplayChars.SuspendLayout();
			this.grpTone.SuspendLayout();
			this.pnlTone.SuspendLayout();
			this.grpLength.SuspendLayout();
			this.pnlLength.SuspendLayout();
			this.grpStress.SuspendLayout();
			this.pnlStress.SuspendLayout();
			this.tpgSorting.SuspendLayout();
			this.grpColSortOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_sortingGrid)).BeginInit();
			this.grpPhoneticSortOptions.SuspendLayout();
			this.tpgUI.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSaveInfo)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.picSaveInfo);
			this.pnlButtons.Controls.Add(this.lblSaveInfo);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.lblSaveInfo, 0);
			this.pnlButtons.Controls.SetChildIndex(this.picSaveInfo, 0);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			// 
			// tabOptions
			// 
			resources.ApplyResources(this.tabOptions, "tabOptions");
			this.tabOptions.Controls.Add(this.tpgWordLists);
			this.tabOptions.Controls.Add(this.tpgRecView);
			this.tabOptions.Controls.Add(this.tpgFindPhones);
			this.tabOptions.Controls.Add(this.tpgCVPatterns);
			this.tabOptions.Controls.Add(this.tpgSorting);
			this.tabOptions.Controls.Add(this.tpgFonts);
			this.tabOptions.Controls.Add(this.tpgColors);
			this.tabOptions.Controls.Add(this.tpgUI);
			this.tabOptions.HotTrack = true;
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			// 
			// tpgWordLists
			// 
			this.tpgWordLists.Controls.Add(this.grpColSettings);
			this.tpgWordLists.Controls.Add(this.grpColChanges);
			this.tpgWordLists.Controls.Add(this.nudMaxEticColWidth);
			this.tpgWordLists.Controls.Add(this.chkAutoAdjustPhoneticCol);
			this.tpgWordLists.Controls.Add(this.grpGridLines);
			resources.ApplyResources(this.tpgWordLists, "tpgWordLists");
			this.tpgWordLists.Name = "tpgWordLists";
			this.tpgWordLists.UseVisualStyleBackColor = true;
			// 
			// grpColSettings
			// 
			this.grpColSettings.Controls.Add(this.fldSelGridWrdList);
			this.grpColSettings.Controls.Add(this.btnMoveColDown);
			this.grpColSettings.Controls.Add(this.btnMoveColUp);
			this.grpColSettings.Controls.Add(this.lblShowColumns);
			resources.ApplyResources(this.grpColSettings, "grpColSettings");
			this.grpColSettings.Name = "grpColSettings";
			this.grpColSettings.TabStop = false;
			// 
			// fldSelGridWrdList
			// 
			this.fldSelGridWrdList.AllowUserToAddRows = false;
			this.fldSelGridWrdList.AllowUserToDeleteRows = false;
			this.fldSelGridWrdList.AllowUserToResizeColumns = false;
			this.fldSelGridWrdList.AllowUserToResizeRows = false;
			this.fldSelGridWrdList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.fldSelGridWrdList.BackgroundColor = System.Drawing.SystemColors.Window;
			this.fldSelGridWrdList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.fldSelGridWrdList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fldSelGridWrdList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.fldSelGridWrdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.fldSelGridWrdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fldSelGridWrdList.ColumnHeadersVisible = false;
			this.fldSelGridWrdList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridWrdList.IsDirty = false;
			resources.ApplyResources(this.fldSelGridWrdList, "fldSelGridWrdList");
			this.fldSelGridWrdList.MultiSelect = false;
			this.fldSelGridWrdList.Name = "fldSelGridWrdList";
			this.fldSelGridWrdList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridWrdList.RowHeadersVisible = false;
			this.fldSelGridWrdList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridWrdList.ShowWaterMarkWhenDirty = false;
			this.fldSelGridWrdList.WaterMark = "!";
			this.fldSelGridWrdList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.fldSelGridWrdList_RowEnter);
			// 
			// btnMoveColDown
			// 
			this.btnMoveColDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			resources.ApplyResources(this.btnMoveColDown, "btnMoveColDown");
			this.btnMoveColDown.Name = "btnMoveColDown";
			this.m_toolTip.SetToolTip(this.btnMoveColDown, resources.GetString("btnMoveColDown.ToolTip"));
			this.btnMoveColDown.UseVisualStyleBackColor = true;
			this.btnMoveColDown.Click += new System.EventHandler(this.btnMoveColDown_Click);
			// 
			// btnMoveColUp
			// 
			this.btnMoveColUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			resources.ApplyResources(this.btnMoveColUp, "btnMoveColUp");
			this.btnMoveColUp.Name = "btnMoveColUp";
			this.m_toolTip.SetToolTip(this.btnMoveColUp, resources.GetString("btnMoveColUp.ToolTip"));
			this.btnMoveColUp.UseVisualStyleBackColor = true;
			this.btnMoveColUp.Click += new System.EventHandler(this.btnMoveColUp_Click);
			// 
			// lblShowColumns
			// 
			this.lblShowColumns.AutoEllipsis = true;
			resources.ApplyResources(this.lblShowColumns, "lblShowColumns");
			this.lblShowColumns.Name = "lblShowColumns";
			// 
			// grpColChanges
			// 
			this.grpColChanges.Controls.Add(this.lblExplanation);
			this.grpColChanges.Controls.Add(this.chkSaveReorderedColumns);
			this.grpColChanges.Controls.Add(this.chkSaveColHdrHeight);
			this.grpColChanges.Controls.Add(this.chkSaveColWidths);
			resources.ApplyResources(this.grpColChanges, "grpColChanges");
			this.grpColChanges.Name = "grpColChanges";
			this.grpColChanges.TabStop = false;
			// 
			// lblExplanation
			// 
			this.lblExplanation.AutoEllipsis = true;
			resources.ApplyResources(this.lblExplanation, "lblExplanation");
			this.lblExplanation.Name = "lblExplanation";
			// 
			// chkSaveReorderedColumns
			// 
			this.chkSaveReorderedColumns.AutoEllipsis = true;
			resources.ApplyResources(this.chkSaveReorderedColumns, "chkSaveReorderedColumns");
			this.chkSaveReorderedColumns.Name = "chkSaveReorderedColumns";
			this.chkSaveReorderedColumns.UseVisualStyleBackColor = true;
			// 
			// chkSaveColHdrHeight
			// 
			this.chkSaveColHdrHeight.AutoEllipsis = true;
			resources.ApplyResources(this.chkSaveColHdrHeight, "chkSaveColHdrHeight");
			this.chkSaveColHdrHeight.Name = "chkSaveColHdrHeight";
			this.chkSaveColHdrHeight.UseVisualStyleBackColor = true;
			// 
			// chkSaveColWidths
			// 
			this.chkSaveColWidths.AutoEllipsis = true;
			resources.ApplyResources(this.chkSaveColWidths, "chkSaveColWidths");
			this.chkSaveColWidths.Name = "chkSaveColWidths";
			this.chkSaveColWidths.UseVisualStyleBackColor = true;
			// 
			// nudMaxEticColWidth
			// 
			resources.ApplyResources(this.nudMaxEticColWidth, "nudMaxEticColWidth");
			this.nudMaxEticColWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudMaxEticColWidth.Name = "nudMaxEticColWidth";
			this.m_toolTip.SetToolTip(this.nudMaxEticColWidth, resources.GetString("nudMaxEticColWidth.ToolTip"));
			// 
			// chkAutoAdjustPhoneticCol
			// 
			this.chkAutoAdjustPhoneticCol.AutoEllipsis = true;
			resources.ApplyResources(this.chkAutoAdjustPhoneticCol, "chkAutoAdjustPhoneticCol");
			this.chkAutoAdjustPhoneticCol.Name = "chkAutoAdjustPhoneticCol";
			this.chkAutoAdjustPhoneticCol.UseVisualStyleBackColor = true;
			this.chkAutoAdjustPhoneticCol.CheckedChanged += new System.EventHandler(this.chkAutoAdjustPhoneticCol_CheckedChanged);
			// 
			// grpGridLines
			// 
			this.grpGridLines.Controls.Add(this.rbGridLinesHorizontal);
			this.grpGridLines.Controls.Add(this.rbGridLinesVertical);
			this.grpGridLines.Controls.Add(this.rbGridLinesBoth);
			this.grpGridLines.Controls.Add(this.rbGridLinesNone);
			resources.ApplyResources(this.grpGridLines, "grpGridLines");
			this.grpGridLines.Name = "grpGridLines";
			this.grpGridLines.TabStop = false;
			// 
			// rbGridLinesHorizontal
			// 
			resources.ApplyResources(this.rbGridLinesHorizontal, "rbGridLinesHorizontal");
			this.rbGridLinesHorizontal.Image = global::SIL.Pa.Properties.Resources.kimidHorizontalGridLines;
			this.rbGridLinesHorizontal.Name = "rbGridLinesHorizontal";
			this.rbGridLinesHorizontal.TabStop = true;
			this.rbGridLinesHorizontal.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesVertical
			// 
			this.rbGridLinesVertical.AutoEllipsis = true;
			resources.ApplyResources(this.rbGridLinesVertical, "rbGridLinesVertical");
			this.rbGridLinesVertical.Image = global::SIL.Pa.Properties.Resources.kimidVerticalGridLines;
			this.rbGridLinesVertical.Name = "rbGridLinesVertical";
			this.rbGridLinesVertical.TabStop = true;
			this.rbGridLinesVertical.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesBoth
			// 
			this.rbGridLinesBoth.AutoEllipsis = true;
			resources.ApplyResources(this.rbGridLinesBoth, "rbGridLinesBoth");
			this.rbGridLinesBoth.Image = global::SIL.Pa.Properties.Resources.kimidBothGridLines;
			this.rbGridLinesBoth.Name = "rbGridLinesBoth";
			this.rbGridLinesBoth.TabStop = true;
			this.rbGridLinesBoth.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesNone
			// 
			this.rbGridLinesNone.AutoEllipsis = true;
			resources.ApplyResources(this.rbGridLinesNone, "rbGridLinesNone");
			this.rbGridLinesNone.Image = global::SIL.Pa.Properties.Resources.kimidNoGridLines;
			this.rbGridLinesNone.Name = "rbGridLinesNone";
			this.rbGridLinesNone.TabStop = true;
			this.rbGridLinesNone.UseVisualStyleBackColor = true;
			// 
			// tpgRecView
			// 
			this.tpgRecView.Controls.Add(this.grpFieldSettings);
			resources.ApplyResources(this.tpgRecView, "tpgRecView");
			this.tpgRecView.Name = "tpgRecView";
			this.tpgRecView.UseVisualStyleBackColor = true;
			// 
			// grpFieldSettings
			// 
			this.grpFieldSettings.Controls.Add(this.fldSelGridRecView);
			this.grpFieldSettings.Controls.Add(this.lblShowFields);
			this.grpFieldSettings.Controls.Add(this.btnMoveRecVwFldDown);
			this.grpFieldSettings.Controls.Add(this.btnMoveRecVwFldUp);
			resources.ApplyResources(this.grpFieldSettings, "grpFieldSettings");
			this.grpFieldSettings.Name = "grpFieldSettings";
			this.grpFieldSettings.TabStop = false;
			// 
			// fldSelGridRecView
			// 
			this.fldSelGridRecView.AllowUserToAddRows = false;
			this.fldSelGridRecView.AllowUserToDeleteRows = false;
			this.fldSelGridRecView.AllowUserToOrderColumns = true;
			this.fldSelGridRecView.AllowUserToResizeColumns = false;
			this.fldSelGridRecView.AllowUserToResizeRows = false;
			this.fldSelGridRecView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.fldSelGridRecView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.fldSelGridRecView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.fldSelGridRecView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fldSelGridRecView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.fldSelGridRecView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.fldSelGridRecView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fldSelGridRecView.ColumnHeadersVisible = false;
			this.fldSelGridRecView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridRecView.IsDirty = false;
			resources.ApplyResources(this.fldSelGridRecView, "fldSelGridRecView");
			this.fldSelGridRecView.MultiSelect = false;
			this.fldSelGridRecView.Name = "fldSelGridRecView";
			this.fldSelGridRecView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridRecView.RowHeadersVisible = false;
			this.fldSelGridRecView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridRecView.ShowWaterMarkWhenDirty = false;
			this.fldSelGridRecView.WaterMark = "!";
			this.fldSelGridRecView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.fldSelGridRecView_RowEnter);
			// 
			// lblShowFields
			// 
			this.lblShowFields.AutoEllipsis = true;
			resources.ApplyResources(this.lblShowFields, "lblShowFields");
			this.lblShowFields.Name = "lblShowFields";
			// 
			// btnMoveRecVwFldDown
			// 
			this.btnMoveRecVwFldDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			resources.ApplyResources(this.btnMoveRecVwFldDown, "btnMoveRecVwFldDown");
			this.btnMoveRecVwFldDown.Name = "btnMoveRecVwFldDown";
			this.m_toolTip.SetToolTip(this.btnMoveRecVwFldDown, resources.GetString("btnMoveRecVwFldDown.ToolTip"));
			this.btnMoveRecVwFldDown.UseVisualStyleBackColor = true;
			this.btnMoveRecVwFldDown.Click += new System.EventHandler(this.btnMoveRecVwFldDown_Click);
			// 
			// btnMoveRecVwFldUp
			// 
			this.btnMoveRecVwFldUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			resources.ApplyResources(this.btnMoveRecVwFldUp, "btnMoveRecVwFldUp");
			this.btnMoveRecVwFldUp.Name = "btnMoveRecVwFldUp";
			this.m_toolTip.SetToolTip(this.btnMoveRecVwFldUp, resources.GetString("btnMoveRecVwFldUp.ToolTip"));
			this.btnMoveRecVwFldUp.UseVisualStyleBackColor = true;
			this.btnMoveRecVwFldUp.Click += new System.EventHandler(this.btnMoveRecVwFldUp_Click);
			// 
			// tpgFindPhones
			// 
			this.tpgFindPhones.Controls.Add(this.lblShowDiamondPattern);
			this.tpgFindPhones.Controls.Add(this.chkShowDiamondPattern);
			this.tpgFindPhones.Controls.Add(this.grpClassSettings);
			resources.ApplyResources(this.tpgFindPhones, "tpgFindPhones");
			this.tpgFindPhones.Name = "tpgFindPhones";
			this.tpgFindPhones.UseVisualStyleBackColor = true;
			// 
			// lblShowDiamondPattern
			// 
			resources.ApplyResources(this.lblShowDiamondPattern, "lblShowDiamondPattern");
			this.lblShowDiamondPattern.Name = "lblShowDiamondPattern";
			// 
			// chkShowDiamondPattern
			// 
			resources.ApplyResources(this.chkShowDiamondPattern, "chkShowDiamondPattern");
			this.chkShowDiamondPattern.Name = "chkShowDiamondPattern";
			this.chkShowDiamondPattern.UseVisualStyleBackColor = true;
			// 
			// grpClassSettings
			// 
			resources.ApplyResources(this.grpClassSettings, "grpClassSettings");
			this.grpClassSettings.Controls.Add(this.rdoClassMembers);
			this.grpClassSettings.Controls.Add(this.rdoClassName);
			this.grpClassSettings.Controls.Add(this.lblClassDisplayBehavior);
			this.grpClassSettings.Name = "grpClassSettings";
			this.grpClassSettings.TabStop = false;
			// 
			// rdoClassMembers
			// 
			resources.ApplyResources(this.rdoClassMembers, "rdoClassMembers");
			this.rdoClassMembers.Name = "rdoClassMembers";
			this.rdoClassMembers.TabStop = true;
			this.rdoClassMembers.UseVisualStyleBackColor = true;
			// 
			// rdoClassName
			// 
			resources.ApplyResources(this.rdoClassName, "rdoClassName");
			this.rdoClassName.Name = "rdoClassName";
			this.rdoClassName.TabStop = true;
			this.rdoClassName.UseVisualStyleBackColor = true;
			// 
			// lblClassDisplayBehavior
			// 
			resources.ApplyResources(this.lblClassDisplayBehavior, "lblClassDisplayBehavior");
			this.lblClassDisplayBehavior.Name = "lblClassDisplayBehavior";
			// 
			// tpgCVPatterns
			// 
			this.tpgCVPatterns.Controls.Add(this.grpDisplayChars);
			this.tpgCVPatterns.Controls.Add(this.chkTone);
			this.tpgCVPatterns.Controls.Add(this.grpTone);
			this.tpgCVPatterns.Controls.Add(this.chkLength);
			this.tpgCVPatterns.Controls.Add(this.grpLength);
			this.tpgCVPatterns.Controls.Add(this.chkStress);
			this.tpgCVPatterns.Controls.Add(this.grpStress);
			resources.ApplyResources(this.tpgCVPatterns, "tpgCVPatterns");
			this.tpgCVPatterns.Name = "tpgCVPatterns";
			this.tpgCVPatterns.UseVisualStyleBackColor = true;
			// 
			// grpDisplayChars
			// 
			resources.ApplyResources(this.grpDisplayChars, "grpDisplayChars");
			this.grpDisplayChars.Controls.Add(this.lblExampleDesc2);
			this.grpDisplayChars.Controls.Add(this.lblExampleCV);
			this.grpDisplayChars.Controls.Add(this.txtCustomChars);
			this.grpDisplayChars.Controls.Add(this.lblExampleCVCV);
			this.grpDisplayChars.Controls.Add(this.lblInstruction);
			this.grpDisplayChars.Controls.Add(this.txtExampleInput);
			this.grpDisplayChars.Controls.Add(this.lblExampleDesc1);
			this.grpDisplayChars.Name = "grpDisplayChars";
			this.grpDisplayChars.TabStop = false;
			// 
			// lblExampleDesc2
			// 
			resources.ApplyResources(this.lblExampleDesc2, "lblExampleDesc2");
			this.lblExampleDesc2.Name = "lblExampleDesc2";
			// 
			// lblExampleCV
			// 
			resources.ApplyResources(this.lblExampleCV, "lblExampleCV");
			this.lblExampleCV.Name = "lblExampleCV";
			this.lblExampleCV.UseMnemonic = false;
			// 
			// txtCustomChars
			// 
			resources.ApplyResources(this.txtCustomChars, "txtCustomChars");
			this.txtCustomChars.Name = "txtCustomChars";
			this.txtCustomChars.TextChanged += new System.EventHandler(this.txtCustomChars_TextChanged);
			this.txtCustomChars.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomChars_KeyDown);
			// 
			// lblExampleCVCV
			// 
			resources.ApplyResources(this.lblExampleCVCV, "lblExampleCVCV");
			this.lblExampleCVCV.Name = "lblExampleCVCV";
			this.lblExampleCVCV.UseMnemonic = false;
			// 
			// lblInstruction
			// 
			resources.ApplyResources(this.lblInstruction, "lblInstruction");
			this.lblInstruction.Name = "lblInstruction";
			// 
			// txtExampleInput
			// 
			resources.ApplyResources(this.txtExampleInput, "txtExampleInput");
			this.txtExampleInput.Name = "txtExampleInput";
			this.txtExampleInput.ReadOnly = true;
			// 
			// lblExampleDesc1
			// 
			resources.ApplyResources(this.lblExampleDesc1, "lblExampleDesc1");
			this.lblExampleDesc1.Name = "lblExampleDesc1";
			// 
			// chkTone
			// 
			resources.ApplyResources(this.chkTone, "chkTone");
			this.chkTone.BackColor = System.Drawing.Color.Transparent;
			this.chkTone.Name = "chkTone";
			this.chkTone.ThreeState = true;
			this.chkTone.UseVisualStyleBackColor = false;
			// 
			// grpTone
			// 
			this.grpTone.Controls.Add(this.pnlTone);
			resources.ApplyResources(this.grpTone, "grpTone");
			this.grpTone.Name = "grpTone";
			this.grpTone.TabStop = false;
			// 
			// pnlTone
			// 
			resources.ApplyResources(this.pnlTone, "pnlTone");
			this.pnlTone.Controls.Add(this.tonePicker);
			this.pnlTone.Name = "pnlTone";
			// 
			// tonePicker
			// 
			resources.ApplyResources(this.tonePicker, "tonePicker");
			this.tonePicker.AutoSizeItems = false;
			this.tonePicker.BackColor = System.Drawing.Color.Transparent;
			this.tonePicker.CheckItemsOnClick = true;
			this.tonePicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tonePicker.ItemSize = new System.Drawing.Size(30, 32);
			this.tonePicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.tonePicker.Name = "tonePicker";
			// 
			// chkLength
			// 
			resources.ApplyResources(this.chkLength, "chkLength");
			this.chkLength.BackColor = System.Drawing.Color.Transparent;
			this.chkLength.Name = "chkLength";
			this.chkLength.ThreeState = true;
			this.chkLength.UseVisualStyleBackColor = false;
			// 
			// grpLength
			// 
			this.grpLength.Controls.Add(this.pnlLength);
			resources.ApplyResources(this.grpLength, "grpLength");
			this.grpLength.Name = "grpLength";
			this.grpLength.TabStop = false;
			// 
			// pnlLength
			// 
			resources.ApplyResources(this.pnlLength, "pnlLength");
			this.pnlLength.Controls.Add(this.lengthPicker);
			this.pnlLength.Name = "pnlLength";
			// 
			// lengthPicker
			// 
			resources.ApplyResources(this.lengthPicker, "lengthPicker");
			this.lengthPicker.AutoSizeItems = false;
			this.lengthPicker.BackColor = System.Drawing.Color.Transparent;
			this.lengthPicker.CheckItemsOnClick = true;
			this.lengthPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.lengthPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.lengthPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.lengthPicker.Name = "lengthPicker";
			// 
			// chkStress
			// 
			resources.ApplyResources(this.chkStress, "chkStress");
			this.chkStress.BackColor = System.Drawing.Color.Transparent;
			this.chkStress.Name = "chkStress";
			this.chkStress.ThreeState = true;
			this.chkStress.UseVisualStyleBackColor = false;
			// 
			// grpStress
			// 
			this.grpStress.Controls.Add(this.pnlStress);
			resources.ApplyResources(this.grpStress, "grpStress");
			this.grpStress.Name = "grpStress";
			this.grpStress.TabStop = false;
			// 
			// pnlStress
			// 
			resources.ApplyResources(this.pnlStress, "pnlStress");
			this.pnlStress.Controls.Add(this.stressPicker);
			this.pnlStress.Name = "pnlStress";
			// 
			// stressPicker
			// 
			resources.ApplyResources(this.stressPicker, "stressPicker");
			this.stressPicker.AutoSizeItems = false;
			this.stressPicker.BackColor = System.Drawing.Color.Transparent;
			this.stressPicker.CheckItemsOnClick = true;
			this.stressPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.stressPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.stressPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.stressPicker.Name = "stressPicker";
			// 
			// tpgSorting
			// 
			this.tpgSorting.Controls.Add(this.lblSaveManual);
			this.tpgSorting.Controls.Add(this.grpColSortOptions);
			this.tpgSorting.Controls.Add(this.chkSaveManual);
			this.tpgSorting.Controls.Add(this.lblSortInfo);
			this.tpgSorting.Controls.Add(this.grpPhoneticSortOptions);
			this.tpgSorting.Controls.Add(this.lblListType);
			this.tpgSorting.Controls.Add(this.cboListType);
			resources.ApplyResources(this.tpgSorting, "tpgSorting");
			this.tpgSorting.Name = "tpgSorting";
			this.tpgSorting.UseVisualStyleBackColor = true;
			// 
			// lblSaveManual
			// 
			resources.ApplyResources(this.lblSaveManual, "lblSaveManual");
			this.lblSaveManual.Name = "lblSaveManual";
			// 
			// grpColSortOptions
			// 
			this.grpColSortOptions.Controls.Add(this.m_sortingGrid);
			this.grpColSortOptions.Controls.Add(this.btnMoveSortFieldUp);
			this.grpColSortOptions.Controls.Add(this.lblSortFldsHdr);
			this.grpColSortOptions.Controls.Add(this.btnMoveSortFieldDown);
			resources.ApplyResources(this.grpColSortOptions, "grpColSortOptions");
			this.grpColSortOptions.Name = "grpColSortOptions";
			this.grpColSortOptions.TabStop = false;
			// 
			// m_sortingGrid
			// 
			this.m_sortingGrid.AllowUserToAddRows = false;
			this.m_sortingGrid.AllowUserToDeleteRows = false;
			this.m_sortingGrid.AllowUserToOrderColumns = true;
			this.m_sortingGrid.AllowUserToResizeRows = false;
			this.m_sortingGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.m_sortingGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_sortingGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_sortingGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_sortingGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.m_sortingGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_sortingGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.m_sortingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_sortingGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_sortingGrid.IsDirty = false;
			resources.ApplyResources(this.m_sortingGrid, "m_sortingGrid");
			this.m_sortingGrid.MultiSelect = false;
			this.m_sortingGrid.Name = "m_sortingGrid";
			this.m_sortingGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_sortingGrid.RowHeadersVisible = false;
			this.m_sortingGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_sortingGrid.ShowWaterMarkWhenDirty = false;
			this.m_sortingGrid.WaterMark = "!";
			this.m_sortingGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_RowEnter);
			this.m_sortingGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_sortingGrid_CellContentClick);
			// 
			// btnMoveSortFieldUp
			// 
			this.btnMoveSortFieldUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			resources.ApplyResources(this.btnMoveSortFieldUp, "btnMoveSortFieldUp");
			this.btnMoveSortFieldUp.Name = "btnMoveSortFieldUp";
			this.m_toolTip.SetToolTip(this.btnMoveSortFieldUp, resources.GetString("btnMoveSortFieldUp.ToolTip"));
			this.btnMoveSortFieldUp.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldUp.Click += new System.EventHandler(this.btnMoveSortFieldUp_Click);
			// 
			// lblSortFldsHdr
			// 
			resources.ApplyResources(this.lblSortFldsHdr, "lblSortFldsHdr");
			this.lblSortFldsHdr.Name = "lblSortFldsHdr";
			// 
			// btnMoveSortFieldDown
			// 
			this.btnMoveSortFieldDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			resources.ApplyResources(this.btnMoveSortFieldDown, "btnMoveSortFieldDown");
			this.btnMoveSortFieldDown.Name = "btnMoveSortFieldDown";
			this.m_toolTip.SetToolTip(this.btnMoveSortFieldDown, resources.GetString("btnMoveSortFieldDown.ToolTip"));
			this.btnMoveSortFieldDown.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldDown.Click += new System.EventHandler(this.btnMoveSortFieldDown_Click);
			// 
			// chkSaveManual
			// 
			resources.ApplyResources(this.chkSaveManual, "chkSaveManual");
			this.chkSaveManual.Name = "chkSaveManual";
			this.chkSaveManual.UseVisualStyleBackColor = true;
			this.chkSaveManual.Click += new System.EventHandler(this.chkSaveManual_Click);
			// 
			// lblSortInfo
			// 
			resources.ApplyResources(this.lblSortInfo, "lblSortInfo");
			this.lblSortInfo.Name = "lblSortInfo";
			// 
			// grpPhoneticSortOptions
			// 
			this.grpPhoneticSortOptions.Controls.Add(this.phoneticSortOptions);
			resources.ApplyResources(this.grpPhoneticSortOptions, "grpPhoneticSortOptions");
			this.grpPhoneticSortOptions.Name = "grpPhoneticSortOptions";
			this.grpPhoneticSortOptions.TabStop = false;
			// 
			// phoneticSortOptions
			// 
			this.phoneticSortOptions.AdvancedOptionsEnabled = true;
			this.phoneticSortOptions.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.phoneticSortOptions, "phoneticSortOptions");
			this.phoneticSortOptions.MakePhoneticPrimarySortFieldWhenOptionsChange = true;
			this.phoneticSortOptions.Name = "phoneticSortOptions";
			this.phoneticSortOptions.ShowAdvancedOptions = true;
			this.phoneticSortOptions.ShowHelpLink = false;
			this.phoneticSortOptions.SortOptionsChanged += new SIL.Pa.UI.Controls.SortOptionsDropDown.SortOptionsChangedHandler(this.phoneticSortOptions_SortOptionsChanged);
			// 
			// lblListType
			// 
			resources.ApplyResources(this.lblListType, "lblListType");
			this.lblListType.Name = "lblListType";
			// 
			// cboListType
			// 
			this.cboListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cboListType, "cboListType");
			this.cboListType.FormattingEnabled = true;
			this.cboListType.Items.AddRange(new object[] {
            resources.GetString("cboListType.Items"),
            resources.GetString("cboListType.Items1"),
            resources.GetString("cboListType.Items2")});
			this.cboListType.Name = "cboListType";
			this.cboListType.SelectedIndexChanged += new System.EventHandler(this.cboListType_SelectedIndexChanged);
			// 
			// tpgFonts
			// 
			resources.ApplyResources(this.tpgFonts, "tpgFonts");
			this.tpgFonts.Name = "tpgFonts";
			this.tpgFonts.UseVisualStyleBackColor = true;
			// 
			// tpgColors
			// 
			resources.ApplyResources(this.tpgColors, "tpgColors");
			this.tpgColors.Name = "tpgColors";
			this.tpgColors.UseVisualStyleBackColor = true;
			// 
			// tpgUI
			// 
			this.tpgUI.Controls.Add(this.cboUILanguage);
			this.tpgUI.Controls.Add(this.lblUILanguage);
			resources.ApplyResources(this.tpgUI, "tpgUI");
			this.tpgUI.Name = "tpgUI";
			this.tpgUI.UseVisualStyleBackColor = true;
			// 
			// lblUILanguage
			// 
			resources.ApplyResources(this.lblUILanguage, "lblUILanguage");
			this.lblUILanguage.Name = "lblUILanguage";
			// 
			// picSaveInfo
			// 
			resources.ApplyResources(this.picSaveInfo, "picSaveInfo");
			this.picSaveInfo.Image = global::SIL.Pa.Properties.Resources.kimidInformation;
			this.picSaveInfo.Name = "picSaveInfo";
			this.picSaveInfo.TabStop = false;
			// 
			// lblSaveInfo
			// 
			resources.ApplyResources(this.lblSaveInfo, "lblSaveInfo");
			this.lblSaveInfo.Name = "lblSaveInfo";
			// 
			// cboUILanguage
			// 
			this.cboUILanguage.FormattingEnabled = true;
			resources.ApplyResources(this.cboUILanguage, "cboUILanguage");
			this.cboUILanguage.Name = "cboUILanguage";
			// 
			// OptionsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.tabOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "OptionsDlg";
			this.Controls.SetChildIndex(this.tabOptions, 0);
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.pnlButtons.ResumeLayout(false);
			this.tabOptions.ResumeLayout(false);
			this.tpgWordLists.ResumeLayout(false);
			this.grpColSettings.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridWrdList)).EndInit();
			this.grpColChanges.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudMaxEticColWidth)).EndInit();
			this.grpGridLines.ResumeLayout(false);
			this.tpgRecView.ResumeLayout(false);
			this.grpFieldSettings.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridRecView)).EndInit();
			this.tpgFindPhones.ResumeLayout(false);
			this.tpgFindPhones.PerformLayout();
			this.grpClassSettings.ResumeLayout(false);
			this.grpClassSettings.PerformLayout();
			this.tpgCVPatterns.ResumeLayout(false);
			this.tpgCVPatterns.PerformLayout();
			this.grpDisplayChars.ResumeLayout(false);
			this.grpDisplayChars.PerformLayout();
			this.grpTone.ResumeLayout(false);
			this.pnlTone.ResumeLayout(false);
			this.grpLength.ResumeLayout(false);
			this.pnlLength.ResumeLayout(false);
			this.grpStress.ResumeLayout(false);
			this.pnlStress.ResumeLayout(false);
			this.tpgSorting.ResumeLayout(false);
			this.tpgSorting.PerformLayout();
			this.grpColSortOptions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_sortingGrid)).EndInit();
			this.grpPhoneticSortOptions.ResumeLayout(false);
			this.tpgUI.ResumeLayout(false);
			this.tpgUI.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSaveInfo)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private TabPage tpgFindPhones;
		private GroupBox grpClassSettings;
		private Label lblClassDisplayBehavior;
		private RadioButton rdoClassName;
		private RadioButton rdoClassMembers;
		private TabPage tpgColors;
		private CheckBox chkShowDiamondPattern;
		private TabPage tpgWordLists;
		private RadioButton rbGridLinesNone;
		private GroupBox grpGridLines;
		private RadioButton rbGridLinesHorizontal;
		private RadioButton rbGridLinesVertical;
		private RadioButton rbGridLinesBoth;
		private Label lblSaveInfo;
		private PictureBox picSaveInfo;
		private CheckBox chkSaveColHdrHeight;
		private CheckBox chkSaveColWidths;
		private CheckBox chkAutoAdjustPhoneticCol;
		private NumericUpDown nudMaxEticColWidth;
		private GroupBox grpColChanges;
		private CheckBox chkSaveReorderedColumns;
		private Label lblExplanation;
		private ToolTip m_toolTip;
		private IContainer components;
		private TabPage tpgRecView;
		private Button btnMoveRecVwFldDown;
		private Button btnMoveRecVwFldUp;
		private Label lblShowFields;
		private TabPage tpgCVPatterns;
		public CheckBox chkStress;
		private GroupBox grpStress;
		private SIL.Pa.UI.Controls.CharPicker stressPicker;
		public CheckBox chkLength;
		private GroupBox grpLength;
		private SIL.Pa.UI.Controls.CharPicker lengthPicker;
		public CheckBox chkTone;
		private GroupBox grpTone;
		private SIL.Pa.UI.Controls.CharPicker tonePicker;
		private GroupBox grpDisplayChars;
		private TextBox txtCustomChars;
		private Label lblInstruction;
		private Label lblExampleDesc1;
		private Label lblExampleCVCV;
		private Label lblExampleCV;
		private TextBox txtExampleInput;
		private Label lblExampleDesc2;
		private TabPage tpgSorting;
		private Label lblListType;
		private ComboBox cboListType;
		private Label lblSortFldsHdr;
		private GroupBox grpPhoneticSortOptions;
		private Button btnMoveSortFieldDown;
		private Button btnMoveSortFieldUp;
		private Label lblSortInfo;
		private SilUtils.SilGrid m_sortingGrid;
		private CheckBox chkSaveManual;
		private SIL.Pa.UI.Controls.FieldSelectorGrid fldSelGridRecView;
		private SIL.Pa.UI.Controls.SortOptionsDropDown phoneticSortOptions;
		private GroupBox grpColSettings;
		private SIL.Pa.UI.Controls.FieldSelectorGrid fldSelGridWrdList;
		private Button btnMoveColDown;
		private Button btnMoveColUp;
		private Label lblShowColumns;
		private GroupBox grpFieldSettings;
		private Label lblShowDiamondPattern;
		private GroupBox grpColSortOptions;
		private Label lblSaveManual;
		private Panel pnlStress;
		private Panel pnlLength;
		private Panel pnlTone;
		private TabPage tpgUI;
		private Label lblUILanguage;
		private ComboBox cboUILanguage;
	}
}
