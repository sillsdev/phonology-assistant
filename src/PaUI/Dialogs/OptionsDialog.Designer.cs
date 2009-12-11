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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
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
			this.cboUILanguage = new System.Windows.Forms.ComboBox();
			this.lblUILanguage = new System.Windows.Forms.Label();
			this.picSaveInfo = new System.Windows.Forms.PictureBox();
			this.lblSaveInfo = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
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
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
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
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizationPriority(this.btnCancel, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCancel, "Localized in base class");
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizationPriority(this.btnOK, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnOK, "Localized in base class");
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, null);
			this.locExtender.SetLocalizationPriority(this.btnHelp, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnHelp, "Localized in base class");
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
			this.locExtender.SetLocalizableToolTip(this.tpgWordLists, null);
			this.locExtender.SetLocalizationComment(this.tpgWordLists, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgWordLists, "OptionsDlg.tpgWordLists");
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
			this.locExtender.SetLocalizableToolTip(this.grpColSettings, null);
			this.locExtender.SetLocalizationComment(this.grpColSettings, "Frame text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpColSettings, "OptionsDlg.WordListTab.grpColSettings");
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
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.fldSelGridWrdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.fldSelGridWrdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fldSelGridWrdList.ColumnHeadersVisible = false;
			this.fldSelGridWrdList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridWrdList.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridWrdList, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridWrdList, null);
			this.locExtender.SetLocalizationPriority(this.fldSelGridWrdList, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.fldSelGridWrdList, "OptionsDlg.fldSelGridWrdList");
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
			this.locExtender.SetLocalizableToolTip(this.btnMoveColDown, "Move Down");
			this.locExtender.SetLocalizationComment(this.btnMoveColDown, "Button on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveColDown, "OptionsDlg.WordListTab.btnMoveColDown");
			this.btnMoveColDown.Name = "btnMoveColDown";
			this.btnMoveColDown.UseVisualStyleBackColor = true;
			this.btnMoveColDown.Click += new System.EventHandler(this.btnMoveColDown_Click);
			// 
			// btnMoveColUp
			// 
			this.btnMoveColUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			resources.ApplyResources(this.btnMoveColUp, "btnMoveColUp");
			this.locExtender.SetLocalizableToolTip(this.btnMoveColUp, "Move Up");
			this.locExtender.SetLocalizationComment(this.btnMoveColUp, "Button on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveColUp, "OptionsDlg.WordListTab.btnMoveColUp");
			this.btnMoveColUp.Name = "btnMoveColUp";
			this.btnMoveColUp.UseVisualStyleBackColor = true;
			this.btnMoveColUp.Click += new System.EventHandler(this.btnMoveColUp_Click);
			// 
			// lblShowColumns
			// 
			this.lblShowColumns.AutoEllipsis = true;
			resources.ApplyResources(this.lblShowColumns, "lblShowColumns");
			this.locExtender.SetLocalizableToolTip(this.lblShowColumns, null);
			this.locExtender.SetLocalizationComment(this.lblShowColumns, "Label on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblShowColumns, "OptionsDlg.WordListTab.lblShowColumns");
			this.lblShowColumns.Name = "lblShowColumns";
			// 
			// grpColChanges
			// 
			this.grpColChanges.Controls.Add(this.lblExplanation);
			this.grpColChanges.Controls.Add(this.chkSaveReorderedColumns);
			this.grpColChanges.Controls.Add(this.chkSaveColHdrHeight);
			this.grpColChanges.Controls.Add(this.chkSaveColWidths);
			this.locExtender.SetLocalizableToolTip(this.grpColChanges, null);
			this.locExtender.SetLocalizationComment(this.grpColChanges, "Frame text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpColChanges, "OptionsDlg.WordListTab.grpColChanges");
			resources.ApplyResources(this.grpColChanges, "grpColChanges");
			this.grpColChanges.Name = "grpColChanges";
			this.grpColChanges.TabStop = false;
			// 
			// lblExplanation
			// 
			this.lblExplanation.AutoEllipsis = true;
			resources.ApplyResources(this.lblExplanation, "lblExplanation");
			this.locExtender.SetLocalizableToolTip(this.lblExplanation, null);
			this.locExtender.SetLocalizationComment(this.lblExplanation, "Label on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblExplanation, "OptionsDlg.WordListTab.lblExplanation");
			this.lblExplanation.Name = "lblExplanation";
			// 
			// chkSaveReorderedColumns
			// 
			this.chkSaveReorderedColumns.AutoEllipsis = true;
			resources.ApplyResources(this.chkSaveReorderedColumns, "chkSaveReorderedColumns");
			this.locExtender.SetLocalizableToolTip(this.chkSaveReorderedColumns, null);
			this.locExtender.SetLocalizationComment(this.chkSaveReorderedColumns, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveReorderedColumns, "OptionsDlg.WordListTab.chkSaveReorderedColumns");
			this.chkSaveReorderedColumns.Name = "chkSaveReorderedColumns";
			this.chkSaveReorderedColumns.UseVisualStyleBackColor = true;
			// 
			// chkSaveColHdrHeight
			// 
			this.chkSaveColHdrHeight.AutoEllipsis = true;
			resources.ApplyResources(this.chkSaveColHdrHeight, "chkSaveColHdrHeight");
			this.locExtender.SetLocalizableToolTip(this.chkSaveColHdrHeight, null);
			this.locExtender.SetLocalizationComment(this.chkSaveColHdrHeight, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveColHdrHeight, "OptionsDlg.WordListTab.chkSaveColHdrHeight");
			this.chkSaveColHdrHeight.Name = "chkSaveColHdrHeight";
			this.chkSaveColHdrHeight.UseVisualStyleBackColor = true;
			// 
			// chkSaveColWidths
			// 
			this.chkSaveColWidths.AutoEllipsis = true;
			resources.ApplyResources(this.chkSaveColWidths, "chkSaveColWidths");
			this.locExtender.SetLocalizableToolTip(this.chkSaveColWidths, null);
			this.locExtender.SetLocalizationComment(this.chkSaveColWidths, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveColWidths, "OptionsDlg.WordListTab.chkSaveColWidths");
			this.chkSaveColWidths.Name = "chkSaveColWidths";
			this.chkSaveColWidths.UseVisualStyleBackColor = true;
			// 
			// nudMaxEticColWidth
			// 
			this.locExtender.SetLocalizableToolTip(this.nudMaxEticColWidth, "Maximum automatically calculated phonetic column width");
			this.locExtender.SetLocalizationComment(this.nudMaxEticColWidth, "Phonetic column width adjuster on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.nudMaxEticColWidth, "OptionsDlg.WordListTab.nudMaxEticColWidth");
			resources.ApplyResources(this.nudMaxEticColWidth, "nudMaxEticColWidth");
			this.nudMaxEticColWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudMaxEticColWidth.Name = "nudMaxEticColWidth";
			// 
			// chkAutoAdjustPhoneticCol
			// 
			this.chkAutoAdjustPhoneticCol.AutoEllipsis = true;
			resources.ApplyResources(this.chkAutoAdjustPhoneticCol, "chkAutoAdjustPhoneticCol");
			this.locExtender.SetLocalizableToolTip(this.chkAutoAdjustPhoneticCol, null);
			this.locExtender.SetLocalizationComment(this.chkAutoAdjustPhoneticCol, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkAutoAdjustPhoneticCol, "OptionsDlg.WordListTab.chkAutoAdjustPhoneticCol");
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
			this.locExtender.SetLocalizableToolTip(this.grpGridLines, null);
			this.locExtender.SetLocalizationComment(this.grpGridLines, "Frame text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpGridLines, "OptionsDlg.WordListTab.grpGridLines");
			resources.ApplyResources(this.grpGridLines, "grpGridLines");
			this.grpGridLines.Name = "grpGridLines";
			this.grpGridLines.TabStop = false;
			// 
			// rbGridLinesHorizontal
			// 
			resources.ApplyResources(this.rbGridLinesHorizontal, "rbGridLinesHorizontal");
			this.rbGridLinesHorizontal.Image = global::SIL.Pa.Properties.Resources.kimidHorizontalGridLines;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesHorizontal, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesHorizontal, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesHorizontal, "OptionsDlg.WordListTab.rbGridLinesHorizontal");
			this.rbGridLinesHorizontal.Name = "rbGridLinesHorizontal";
			this.rbGridLinesHorizontal.TabStop = true;
			this.rbGridLinesHorizontal.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesVertical
			// 
			this.rbGridLinesVertical.AutoEllipsis = true;
			resources.ApplyResources(this.rbGridLinesVertical, "rbGridLinesVertical");
			this.rbGridLinesVertical.Image = global::SIL.Pa.Properties.Resources.kimidVerticalGridLines;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesVertical, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesVertical, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesVertical, "OptionsDlg.WordListTab.rbGridLinesVertical");
			this.rbGridLinesVertical.Name = "rbGridLinesVertical";
			this.rbGridLinesVertical.TabStop = true;
			this.rbGridLinesVertical.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesBoth
			// 
			this.rbGridLinesBoth.AutoEllipsis = true;
			resources.ApplyResources(this.rbGridLinesBoth, "rbGridLinesBoth");
			this.rbGridLinesBoth.Image = global::SIL.Pa.Properties.Resources.kimidBothGridLines;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesBoth, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesBoth, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesBoth, "OptionsDlg.WordListTab.rbGridLinesBoth");
			this.rbGridLinesBoth.Name = "rbGridLinesBoth";
			this.rbGridLinesBoth.TabStop = true;
			this.rbGridLinesBoth.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesNone
			// 
			this.rbGridLinesNone.AutoEllipsis = true;
			resources.ApplyResources(this.rbGridLinesNone, "rbGridLinesNone");
			this.rbGridLinesNone.Image = global::SIL.Pa.Properties.Resources.kimidNoGridLines;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesNone, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesNone, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesNone, "OptionsDlg.WordListTab.rbGridLinesNone");
			this.rbGridLinesNone.Name = "rbGridLinesNone";
			this.rbGridLinesNone.TabStop = true;
			this.rbGridLinesNone.UseVisualStyleBackColor = true;
			// 
			// tpgRecView
			// 
			this.tpgRecView.Controls.Add(this.grpFieldSettings);
			this.locExtender.SetLocalizableToolTip(this.tpgRecView, null);
			this.locExtender.SetLocalizationComment(this.tpgRecView, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgRecView, "OptionsDlg.tpgRecView");
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
			this.locExtender.SetLocalizableToolTip(this.grpFieldSettings, null);
			this.locExtender.SetLocalizationComment(this.grpFieldSettings, "Frame text on record view tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpFieldSettings, "OptionsDlg.RecordViewTab.grpFieldSettings");
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
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle8.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.fldSelGridRecView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
			this.fldSelGridRecView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fldSelGridRecView.ColumnHeadersVisible = false;
			this.fldSelGridRecView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridRecView.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridRecView, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridRecView, null);
			this.locExtender.SetLocalizationPriority(this.fldSelGridRecView, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.fldSelGridRecView, "OptionsDlg.fldSelGridRecView");
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
			this.locExtender.SetLocalizableToolTip(this.lblShowFields, null);
			this.locExtender.SetLocalizationComment(this.lblShowFields, "Label on record view tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblShowFields, "OptionsDlg.RecordViewTab.lblShowFields");
			this.lblShowFields.Name = "lblShowFields";
			// 
			// btnMoveRecVwFldDown
			// 
			this.btnMoveRecVwFldDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			resources.ApplyResources(this.btnMoveRecVwFldDown, "btnMoveRecVwFldDown");
			this.locExtender.SetLocalizableToolTip(this.btnMoveRecVwFldDown, "Move Down");
			this.locExtender.SetLocalizationComment(this.btnMoveRecVwFldDown, "Button on record view of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveRecVwFldDown, "OptionsDlg.RecordViewTab.btnMoveRecVwFldDown");
			this.btnMoveRecVwFldDown.Name = "btnMoveRecVwFldDown";
			this.btnMoveRecVwFldDown.UseVisualStyleBackColor = true;
			this.btnMoveRecVwFldDown.Click += new System.EventHandler(this.btnMoveRecVwFldDown_Click);
			// 
			// btnMoveRecVwFldUp
			// 
			this.btnMoveRecVwFldUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			resources.ApplyResources(this.btnMoveRecVwFldUp, "btnMoveRecVwFldUp");
			this.locExtender.SetLocalizableToolTip(this.btnMoveRecVwFldUp, "Move Up");
			this.locExtender.SetLocalizationComment(this.btnMoveRecVwFldUp, "Button on record view tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveRecVwFldUp, "OptionsDlg.RecordViewTab.btnMoveRecVwFldUp");
			this.btnMoveRecVwFldUp.Name = "btnMoveRecVwFldUp";
			this.btnMoveRecVwFldUp.UseVisualStyleBackColor = true;
			this.btnMoveRecVwFldUp.Click += new System.EventHandler(this.btnMoveRecVwFldUp_Click);
			// 
			// tpgFindPhones
			// 
			this.tpgFindPhones.Controls.Add(this.lblShowDiamondPattern);
			this.tpgFindPhones.Controls.Add(this.chkShowDiamondPattern);
			this.tpgFindPhones.Controls.Add(this.grpClassSettings);
			this.locExtender.SetLocalizableToolTip(this.tpgFindPhones, null);
			this.locExtender.SetLocalizationComment(this.tpgFindPhones, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgFindPhones, "OptionsDlg.tpgFindPhones");
			resources.ApplyResources(this.tpgFindPhones, "tpgFindPhones");
			this.tpgFindPhones.Name = "tpgFindPhones";
			this.tpgFindPhones.UseVisualStyleBackColor = true;
			// 
			// lblShowDiamondPattern
			// 
			resources.ApplyResources(this.lblShowDiamondPattern, "lblShowDiamondPattern");
			this.locExtender.SetLocalizableToolTip(this.lblShowDiamondPattern, null);
			this.locExtender.SetLocalizationComment(this.lblShowDiamondPattern, "Label text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblShowDiamondPattern, "OptionsDlg.SearchPatternsTab.lblShowDiamondPattern");
			this.lblShowDiamondPattern.Name = "lblShowDiamondPattern";
			// 
			// chkShowDiamondPattern
			// 
			resources.ApplyResources(this.chkShowDiamondPattern, "chkShowDiamondPattern");
			this.locExtender.SetLocalizableToolTip(this.chkShowDiamondPattern, null);
			this.locExtender.SetLocalizationComment(this.chkShowDiamondPattern, "Check box text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkShowDiamondPattern, "OptionsDlg.SearchPatternsTab.chkShowDiamondPattern");
			this.chkShowDiamondPattern.Name = "chkShowDiamondPattern";
			this.chkShowDiamondPattern.UseVisualStyleBackColor = true;
			// 
			// grpClassSettings
			// 
			resources.ApplyResources(this.grpClassSettings, "grpClassSettings");
			this.grpClassSettings.Controls.Add(this.rdoClassMembers);
			this.grpClassSettings.Controls.Add(this.rdoClassName);
			this.grpClassSettings.Controls.Add(this.lblClassDisplayBehavior);
			this.locExtender.SetLocalizableToolTip(this.grpClassSettings, null);
			this.locExtender.SetLocalizationComment(this.grpClassSettings, "Frame text on search pattern tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpClassSettings, "OptionsDlg.SearchPatternsTab.grpClassSettings");
			this.grpClassSettings.Name = "grpClassSettings";
			this.grpClassSettings.TabStop = false;
			// 
			// rdoClassMembers
			// 
			resources.ApplyResources(this.rdoClassMembers, "rdoClassMembers");
			this.locExtender.SetLocalizableToolTip(this.rdoClassMembers, null);
			this.locExtender.SetLocalizationComment(this.rdoClassMembers, "Radio button text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rdoClassMembers, "OptionsDlg.SearchPatternsTab.rdoClassMembers");
			this.rdoClassMembers.Name = "rdoClassMembers";
			this.rdoClassMembers.TabStop = true;
			this.rdoClassMembers.UseVisualStyleBackColor = true;
			// 
			// rdoClassName
			// 
			resources.ApplyResources(this.rdoClassName, "rdoClassName");
			this.locExtender.SetLocalizableToolTip(this.rdoClassName, null);
			this.locExtender.SetLocalizationComment(this.rdoClassName, "Radio button text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rdoClassName, "OptionsDlg.SearchPatternsTab.rdoClassName");
			this.rdoClassName.Name = "rdoClassName";
			this.rdoClassName.TabStop = true;
			this.rdoClassName.UseVisualStyleBackColor = true;
			// 
			// lblClassDisplayBehavior
			// 
			resources.ApplyResources(this.lblClassDisplayBehavior, "lblClassDisplayBehavior");
			this.locExtender.SetLocalizableToolTip(this.lblClassDisplayBehavior, null);
			this.locExtender.SetLocalizationComment(this.lblClassDisplayBehavior, "Label text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblClassDisplayBehavior, "OptionsDlg.SearchPatternsTab.lblClassDisplayBehavior");
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
			this.locExtender.SetLocalizableToolTip(this.tpgCVPatterns, null);
			this.locExtender.SetLocalizationComment(this.tpgCVPatterns, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgCVPatterns, "OptionsDlg.tpgCVPatterns");
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
			this.locExtender.SetLocalizableToolTip(this.grpDisplayChars, null);
			this.locExtender.SetLocalizationComment(this.grpDisplayChars, "Frame text on CV pattern tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpDisplayChars, "OptionsDlg.CVPatternsTab.grpDisplayChars");
			this.grpDisplayChars.Name = "grpDisplayChars";
			this.grpDisplayChars.TabStop = false;
			// 
			// lblExampleDesc2
			// 
			resources.ApplyResources(this.lblExampleDesc2, "lblExampleDesc2");
			this.locExtender.SetLocalizableToolTip(this.lblExampleDesc2, null);
			this.locExtender.SetLocalizationComment(this.lblExampleDesc2, "Label on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblExampleDesc2, "OptionsDlg.CVPatternsTab.lblExampleDesc2");
			this.lblExampleDesc2.Name = "lblExampleDesc2";
			// 
			// lblExampleCV
			// 
			resources.ApplyResources(this.lblExampleCV, "lblExampleCV");
			this.locExtender.SetLocalizableToolTip(this.lblExampleCV, null);
			this.locExtender.SetLocalizationComment(this.lblExampleCV, null);
			this.locExtender.SetLocalizationPriority(this.lblExampleCV, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblExampleCV, "OptionsDlg.lblExampleCV");
			this.lblExampleCV.Name = "lblExampleCV";
			this.lblExampleCV.UseMnemonic = false;
			// 
			// txtCustomChars
			// 
			resources.ApplyResources(this.txtCustomChars, "txtCustomChars");
			this.locExtender.SetLocalizableToolTip(this.txtCustomChars, null);
			this.locExtender.SetLocalizationComment(this.txtCustomChars, null);
			this.locExtender.SetLocalizationPriority(this.txtCustomChars, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtCustomChars, "OptionsDlg.txtCustomChars");
			this.txtCustomChars.Name = "txtCustomChars";
			this.txtCustomChars.TextChanged += new System.EventHandler(this.txtCustomChars_TextChanged);
			this.txtCustomChars.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomChars_KeyDown);
			// 
			// lblExampleCVCV
			// 
			resources.ApplyResources(this.lblExampleCVCV, "lblExampleCVCV");
			this.locExtender.SetLocalizableToolTip(this.lblExampleCVCV, null);
			this.locExtender.SetLocalizationComment(this.lblExampleCVCV, null);
			this.locExtender.SetLocalizationPriority(this.lblExampleCVCV, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblExampleCVCV, "OptionsDlg.lblExampleCVCV");
			this.lblExampleCVCV.Name = "lblExampleCVCV";
			this.lblExampleCVCV.UseMnemonic = false;
			// 
			// lblInstruction
			// 
			resources.ApplyResources(this.lblInstruction, "lblInstruction");
			this.locExtender.SetLocalizableToolTip(this.lblInstruction, null);
			this.locExtender.SetLocalizationComment(this.lblInstruction, "Label on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblInstruction, "OptionsDlg.CVPatternsTab.lblInstruction");
			this.lblInstruction.Name = "lblInstruction";
			// 
			// txtExampleInput
			// 
			resources.ApplyResources(this.txtExampleInput, "txtExampleInput");
			this.locExtender.SetLocalizableToolTip(this.txtExampleInput, null);
			this.locExtender.SetLocalizationComment(this.txtExampleInput, null);
			this.locExtender.SetLocalizationPriority(this.txtExampleInput, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtExampleInput, "OptionsDlg.txtExampleInput");
			this.txtExampleInput.Name = "txtExampleInput";
			this.txtExampleInput.ReadOnly = true;
			// 
			// lblExampleDesc1
			// 
			resources.ApplyResources(this.lblExampleDesc1, "lblExampleDesc1");
			this.locExtender.SetLocalizableToolTip(this.lblExampleDesc1, null);
			this.locExtender.SetLocalizationComment(this.lblExampleDesc1, "Label on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblExampleDesc1, "OptionsDlg.CVPatternsTab.lblExampleDesc1");
			this.lblExampleDesc1.Name = "lblExampleDesc1";
			// 
			// chkTone
			// 
			resources.ApplyResources(this.chkTone, "chkTone");
			this.chkTone.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.chkTone, null);
			this.locExtender.SetLocalizationComment(this.chkTone, "Check box text on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkTone, "OptionsDlg.CVPatternsTab.chkTone");
			this.chkTone.Name = "chkTone";
			this.chkTone.ThreeState = true;
			this.chkTone.UseVisualStyleBackColor = false;
			// 
			// grpTone
			// 
			this.grpTone.Controls.Add(this.pnlTone);
			this.locExtender.SetLocalizableToolTip(this.grpTone, null);
			this.locExtender.SetLocalizationComment(this.grpTone, null);
			this.locExtender.SetLocalizationPriority(this.grpTone, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpTone, "OptionsDlg.grpTone");
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
			this.locExtender.SetLocalizableToolTip(this.tonePicker, null);
			this.locExtender.SetLocalizationComment(this.tonePicker, null);
			this.locExtender.SetLocalizationPriority(this.tonePicker, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tonePicker, "OptionsDlg.tonePicker");
			this.tonePicker.Name = "tonePicker";
			// 
			// chkLength
			// 
			resources.ApplyResources(this.chkLength, "chkLength");
			this.chkLength.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.chkLength, null);
			this.locExtender.SetLocalizationComment(this.chkLength, "Check box text on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkLength, "OptionsDlg.CVPatternsTab.chkLength");
			this.chkLength.Name = "chkLength";
			this.chkLength.ThreeState = true;
			this.chkLength.UseVisualStyleBackColor = false;
			// 
			// grpLength
			// 
			this.grpLength.Controls.Add(this.pnlLength);
			this.locExtender.SetLocalizableToolTip(this.grpLength, null);
			this.locExtender.SetLocalizationComment(this.grpLength, null);
			this.locExtender.SetLocalizationPriority(this.grpLength, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpLength, "OptionsDlg.grpLength");
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
			this.locExtender.SetLocalizableToolTip(this.lengthPicker, null);
			this.locExtender.SetLocalizationComment(this.lengthPicker, null);
			this.locExtender.SetLocalizationPriority(this.lengthPicker, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lengthPicker, "OptionsDlg.lengthPicker");
			this.lengthPicker.Name = "lengthPicker";
			// 
			// chkStress
			// 
			resources.ApplyResources(this.chkStress, "chkStress");
			this.chkStress.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.chkStress, null);
			this.locExtender.SetLocalizationComment(this.chkStress, "Check box text on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkStress, "OptionsDlg.CVPatternsTab.chkStress");
			this.chkStress.Name = "chkStress";
			this.chkStress.ThreeState = true;
			this.chkStress.UseVisualStyleBackColor = false;
			// 
			// grpStress
			// 
			this.grpStress.Controls.Add(this.pnlStress);
			this.locExtender.SetLocalizableToolTip(this.grpStress, null);
			this.locExtender.SetLocalizationComment(this.grpStress, null);
			this.locExtender.SetLocalizationPriority(this.grpStress, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpStress, "OptionsDlg.grpStress");
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
			this.locExtender.SetLocalizableToolTip(this.stressPicker, null);
			this.locExtender.SetLocalizationComment(this.stressPicker, null);
			this.locExtender.SetLocalizationPriority(this.stressPicker, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.stressPicker, "OptionsDlg.stressPicker");
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
			this.locExtender.SetLocalizableToolTip(this.tpgSorting, null);
			this.locExtender.SetLocalizationComment(this.tpgSorting, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizationPriority(this.tpgSorting, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tpgSorting, "OptionsDlg.tpgSorting");
			resources.ApplyResources(this.tpgSorting, "tpgSorting");
			this.tpgSorting.Name = "tpgSorting";
			this.tpgSorting.UseVisualStyleBackColor = true;
			// 
			// lblSaveManual
			// 
			resources.ApplyResources(this.lblSaveManual, "lblSaveManual");
			this.locExtender.SetLocalizableToolTip(this.lblSaveManual, null);
			this.locExtender.SetLocalizationComment(this.lblSaveManual, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSaveManual, "OptionsDlg.SortingTab.lblSaveManual");
			this.lblSaveManual.Name = "lblSaveManual";
			// 
			// grpColSortOptions
			// 
			this.grpColSortOptions.Controls.Add(this.m_sortingGrid);
			this.grpColSortOptions.Controls.Add(this.btnMoveSortFieldUp);
			this.grpColSortOptions.Controls.Add(this.lblSortFldsHdr);
			this.grpColSortOptions.Controls.Add(this.btnMoveSortFieldDown);
			this.locExtender.SetLocalizableToolTip(this.grpColSortOptions, null);
			this.locExtender.SetLocalizationComment(this.grpColSortOptions, "Frame text on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpColSortOptions, "OptionsDlg.SortingTab.grpColSortOptions");
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
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle9.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_sortingGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.m_sortingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_sortingGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_sortingGrid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_sortingGrid, null);
			this.locExtender.SetLocalizationComment(this.m_sortingGrid, null);
			this.locExtender.SetLocalizationPriority(this.m_sortingGrid, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_sortingGrid, "OptionsDlg.m_sortingGrid");
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
			this.locExtender.SetLocalizableToolTip(this.btnMoveSortFieldUp, "Move Up");
			this.locExtender.SetLocalizationComment(this.btnMoveSortFieldUp, "Button on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveSortFieldUp, "OptionsDlg.SortingTab.btnMoveSortFieldUp");
			this.btnMoveSortFieldUp.Name = "btnMoveSortFieldUp";
			this.btnMoveSortFieldUp.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldUp.Click += new System.EventHandler(this.btnMoveSortFieldUp_Click);
			// 
			// lblSortFldsHdr
			// 
			resources.ApplyResources(this.lblSortFldsHdr, "lblSortFldsHdr");
			this.locExtender.SetLocalizableToolTip(this.lblSortFldsHdr, null);
			this.locExtender.SetLocalizationComment(this.lblSortFldsHdr, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSortFldsHdr, "OptionsDlg.SortingTab.lblSortFldsHdr");
			this.lblSortFldsHdr.Name = "lblSortFldsHdr";
			// 
			// btnMoveSortFieldDown
			// 
			this.btnMoveSortFieldDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			resources.ApplyResources(this.btnMoveSortFieldDown, "btnMoveSortFieldDown");
			this.locExtender.SetLocalizableToolTip(this.btnMoveSortFieldDown, "Move Down");
			this.locExtender.SetLocalizationComment(this.btnMoveSortFieldDown, "Button on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveSortFieldDown, "OptionsDlg.SortingTab.btnMoveSortFieldDown");
			this.btnMoveSortFieldDown.Name = "btnMoveSortFieldDown";
			this.btnMoveSortFieldDown.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldDown.Click += new System.EventHandler(this.btnMoveSortFieldDown_Click);
			// 
			// chkSaveManual
			// 
			resources.ApplyResources(this.chkSaveManual, "chkSaveManual");
			this.locExtender.SetLocalizableToolTip(this.chkSaveManual, null);
			this.locExtender.SetLocalizationComment(this.chkSaveManual, "Check box text on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveManual, "OptionsDlg.SortingTab.chkSaveManual");
			this.chkSaveManual.Name = "chkSaveManual";
			this.chkSaveManual.UseVisualStyleBackColor = true;
			this.chkSaveManual.Click += new System.EventHandler(this.chkSaveManual_Click);
			// 
			// lblSortInfo
			// 
			resources.ApplyResources(this.lblSortInfo, "lblSortInfo");
			this.locExtender.SetLocalizableToolTip(this.lblSortInfo, null);
			this.locExtender.SetLocalizationComment(this.lblSortInfo, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSortInfo, "OptionsDlg.SortingTab.lblSortInfo");
			this.lblSortInfo.Name = "lblSortInfo";
			// 
			// grpPhoneticSortOptions
			// 
			this.grpPhoneticSortOptions.Controls.Add(this.phoneticSortOptions);
			this.locExtender.SetLocalizableToolTip(this.grpPhoneticSortOptions, null);
			this.locExtender.SetLocalizationComment(this.grpPhoneticSortOptions, "Frame text on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpPhoneticSortOptions, "OptionsDlg.SortingTab.grpPhoneticSortOptions");
			resources.ApplyResources(this.grpPhoneticSortOptions, "grpPhoneticSortOptions");
			this.grpPhoneticSortOptions.Name = "grpPhoneticSortOptions";
			this.grpPhoneticSortOptions.TabStop = false;
			// 
			// phoneticSortOptions
			// 
			this.phoneticSortOptions.AdvancedOptionsEnabled = true;
			this.phoneticSortOptions.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.phoneticSortOptions, "phoneticSortOptions");
			this.locExtender.SetLocalizableToolTip(this.phoneticSortOptions, null);
			this.locExtender.SetLocalizationComment(this.phoneticSortOptions, null);
			this.locExtender.SetLocalizationPriority(this.phoneticSortOptions, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.phoneticSortOptions, "OptionsDlg.SortOptionsDropDown");
			this.phoneticSortOptions.MakePhoneticPrimarySortFieldWhenOptionsChange = true;
			this.phoneticSortOptions.Name = "phoneticSortOptions";
			this.phoneticSortOptions.ShowAdvancedOptions = true;
			this.phoneticSortOptions.ShowHelpLink = false;
			this.phoneticSortOptions.SortOptionsChanged += new SIL.Pa.UI.Controls.SortOptionsDropDown.SortOptionsChangedHandler(this.phoneticSortOptions_SortOptionsChanged);
			// 
			// lblListType
			// 
			resources.ApplyResources(this.lblListType, "lblListType");
			this.locExtender.SetLocalizableToolTip(this.lblListType, null);
			this.locExtender.SetLocalizationComment(this.lblListType, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblListType, "OptionsDlg.SortingTab.lblListType");
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
			this.locExtender.SetLocalizableToolTip(this.cboListType, null);
			this.locExtender.SetLocalizationComment(this.cboListType, null);
			this.locExtender.SetLocalizingId(this.cboListType, "OptionsDlg.SortingTab.cboListType");
			this.cboListType.Name = "cboListType";
			this.cboListType.SelectedIndexChanged += new System.EventHandler(this.cboListType_SelectedIndexChanged);
			// 
			// tpgFonts
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgFonts, null);
			this.locExtender.SetLocalizationComment(this.tpgFonts, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgFonts, "OptionsDlg.tpgFonts");
			resources.ApplyResources(this.tpgFonts, "tpgFonts");
			this.tpgFonts.Name = "tpgFonts";
			this.tpgFonts.UseVisualStyleBackColor = true;
			// 
			// tpgColors
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgColors, null);
			this.locExtender.SetLocalizationComment(this.tpgColors, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgColors, "OptionsDlg.tpgColors");
			resources.ApplyResources(this.tpgColors, "tpgColors");
			this.tpgColors.Name = "tpgColors";
			this.tpgColors.UseVisualStyleBackColor = true;
			// 
			// tpgUI
			// 
			this.tpgUI.Controls.Add(this.cboUILanguage);
			this.tpgUI.Controls.Add(this.lblUILanguage);
			this.locExtender.SetLocalizableToolTip(this.tpgUI, null);
			this.locExtender.SetLocalizationComment(this.tpgUI, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgUI, "OptionsDlg.tpgUI");
			resources.ApplyResources(this.tpgUI, "tpgUI");
			this.tpgUI.Name = "tpgUI";
			this.tpgUI.UseVisualStyleBackColor = true;
			// 
			// cboUILanguage
			// 
			this.cboUILanguage.DropDownHeight = 200;
			this.cboUILanguage.FormattingEnabled = true;
			resources.ApplyResources(this.cboUILanguage, "cboUILanguage");
			this.locExtender.SetLocalizableToolTip(this.cboUILanguage, null);
			this.locExtender.SetLocalizationComment(this.cboUILanguage, null);
			this.locExtender.SetLocalizationPriority(this.cboUILanguage, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboUILanguage, "OptionsDlg.cboUILanguage");
			this.cboUILanguage.Name = "cboUILanguage";
			// 
			// lblUILanguage
			// 
			resources.ApplyResources(this.lblUILanguage, "lblUILanguage");
			this.locExtender.SetLocalizableToolTip(this.lblUILanguage, null);
			this.locExtender.SetLocalizationComment(this.lblUILanguage, "Label on user interface tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblUILanguage, "OptionsDlg.SortingTab.lblUILanguage");
			this.lblUILanguage.Name = "lblUILanguage";
			// 
			// picSaveInfo
			// 
			resources.ApplyResources(this.picSaveInfo, "picSaveInfo");
			this.picSaveInfo.Image = global::SIL.Pa.Properties.Resources.kimidInformation;
			this.locExtender.SetLocalizableToolTip(this.picSaveInfo, null);
			this.locExtender.SetLocalizationComment(this.picSaveInfo, null);
			this.locExtender.SetLocalizationPriority(this.picSaveInfo, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.picSaveInfo, "OptionsDlg.picSaveInfo");
			this.picSaveInfo.Name = "picSaveInfo";
			this.picSaveInfo.TabStop = false;
			// 
			// lblSaveInfo
			// 
			resources.ApplyResources(this.lblSaveInfo, "lblSaveInfo");
			this.locExtender.SetLocalizableToolTip(this.lblSaveInfo, null);
			this.locExtender.SetLocalizationComment(this.lblSaveInfo, "Label next to OK/Cancel/Help buttons on options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSaveInfo, "OptionsDlg.lblSaveInfo");
			this.lblSaveInfo.Name = "lblSaveInfo";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// OptionsDlg
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.tabOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Title of options dialog box.");
			this.locExtender.SetLocalizingId(this, "OptionsDlg.WindowTitle");
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
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
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
		private IContainer components;
		private TabPage tpgRecView;
		private Button btnMoveRecVwFldDown;
		private Button btnMoveRecVwFldUp;
		private Label lblShowFields;
		private TabPage tpgCVPatterns;
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
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
		private CheckBox chkStress;
	}
}
