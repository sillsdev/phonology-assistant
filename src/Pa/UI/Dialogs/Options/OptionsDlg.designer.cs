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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
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
			this.m_sortingGrid = new SilTools.SilGrid();
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
			this.pnlFonts = new SilTools.Controls.SilPanel();
			this.tpgColors = new System.Windows.Forms.TabPage();
			this.tpgUI = new System.Windows.Forms.TabPage();
			this.cboUILanguage = new System.Windows.Forms.ComboBox();
			this.lblUILanguage = new System.Windows.Forms.Label();
			this.picSaveInfo = new System.Windows.Forms.PictureBox();
			this.lblSaveInfo = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
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
			this.tpgFonts.SuspendLayout();
			this.tpgUI.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSaveInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// tabOptions
			// 
			this.tabOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabOptions.Controls.Add(this.tpgWordLists);
			this.tabOptions.Controls.Add(this.tpgRecView);
			this.tabOptions.Controls.Add(this.tpgFindPhones);
			this.tabOptions.Controls.Add(this.tpgCVPatterns);
			this.tabOptions.Controls.Add(this.tpgSorting);
			this.tabOptions.Controls.Add(this.tpgFonts);
			this.tabOptions.Controls.Add(this.tpgColors);
			this.tabOptions.Controls.Add(this.tpgUI);
			this.tabOptions.HotTrack = true;
			this.tabOptions.Location = new System.Drawing.Point(12, 12);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			this.tabOptions.Size = new System.Drawing.Size(540, 408);
			this.tabOptions.TabIndex = 0;
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
			this.locExtender.SetLocalizingId(this.tpgWordLists, "OptionsDlg.WordListTab.Text");
			this.tpgWordLists.Location = new System.Drawing.Point(4, 22);
			this.tpgWordLists.Name = "tpgWordLists";
			this.tpgWordLists.Padding = new System.Windows.Forms.Padding(3);
			this.tpgWordLists.Size = new System.Drawing.Size(532, 382);
			this.tpgWordLists.TabIndex = 4;
			this.tpgWordLists.Text = "Word Lists";
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
			this.grpColSettings.Location = new System.Drawing.Point(6, 12);
			this.grpColSettings.Name = "grpColSettings";
			this.grpColSettings.Size = new System.Drawing.Size(196, 290);
			this.grpColSettings.TabIndex = 0;
			this.grpColSettings.TabStop = false;
			this.grpColSettings.Text = "Column Display Options";
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
			this.fldSelGridWrdList.DrawTextBoxEditControlBorder = false;
			this.fldSelGridWrdList.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.fldSelGridWrdList.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.fldSelGridWrdList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridWrdList.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridWrdList, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridWrdList, null);
			this.locExtender.SetLocalizationPriority(this.fldSelGridWrdList, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.fldSelGridWrdList, "OptionsDlg.WordListTab.fldSelGridWrdList");
			this.fldSelGridWrdList.Location = new System.Drawing.Point(8, 57);
			this.fldSelGridWrdList.MultiSelect = false;
			this.fldSelGridWrdList.Name = "fldSelGridWrdList";
			this.fldSelGridWrdList.PaintHeaderAcrossFullGridWidth = true;
			this.fldSelGridWrdList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridWrdList.RowHeadersVisible = false;
			this.fldSelGridWrdList.RowHeadersWidth = 22;
			this.fldSelGridWrdList.SelectedCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridWrdList.SelectedCellForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridWrdList.SelectedRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridWrdList.SelectedRowForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridWrdList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridWrdList.ShowWaterMarkWhenDirty = false;
			this.fldSelGridWrdList.Size = new System.Drawing.Size(151, 225);
			this.fldSelGridWrdList.TabIndex = 1;
			this.fldSelGridWrdList.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.fldSelGridWrdList.WaterMark = "!";
			this.fldSelGridWrdList.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.fldSelGridWrdList_RowEnter);
			// 
			// btnMoveColDown
			// 
			this.btnMoveColDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			this.btnMoveColDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveColDown, "Move Down");
			this.locExtender.SetLocalizationComment(this.btnMoveColDown, "Button on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveColDown, "OptionsDlg.WordListTab.btnMoveColDown");
			this.btnMoveColDown.Location = new System.Drawing.Point(165, 84);
			this.btnMoveColDown.Name = "btnMoveColDown";
			this.btnMoveColDown.Size = new System.Drawing.Size(24, 24);
			this.btnMoveColDown.TabIndex = 3;
			this.btnMoveColDown.UseVisualStyleBackColor = true;
			this.btnMoveColDown.Click += new System.EventHandler(this.btnMoveColDown_Click);
			// 
			// btnMoveColUp
			// 
			this.btnMoveColUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			this.btnMoveColUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveColUp, "Move Up");
			this.locExtender.SetLocalizationComment(this.btnMoveColUp, "Button on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveColUp, "OptionsDlg.WordListTab.btnMoveColUp");
			this.btnMoveColUp.Location = new System.Drawing.Point(165, 57);
			this.btnMoveColUp.Name = "btnMoveColUp";
			this.btnMoveColUp.Size = new System.Drawing.Size(24, 24);
			this.btnMoveColUp.TabIndex = 2;
			this.btnMoveColUp.UseVisualStyleBackColor = true;
			this.btnMoveColUp.Click += new System.EventHandler(this.btnMoveColUp_Click);
			// 
			// lblShowColumns
			// 
			this.lblShowColumns.AutoEllipsis = true;
			this.lblShowColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblShowColumns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblShowColumns, null);
			this.locExtender.SetLocalizationComment(this.lblShowColumns, "Label on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblShowColumns, "OptionsDlg.WordListTab.lblShowColumns");
			this.lblShowColumns.Location = new System.Drawing.Point(10, 18);
			this.lblShowColumns.Name = "lblShowColumns";
			this.lblShowColumns.Size = new System.Drawing.Size(151, 37);
			this.lblShowColumns.TabIndex = 0;
			this.lblShowColumns.Text = "&Specify the columns to display and their order.";
			this.lblShowColumns.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.grpColChanges.Location = new System.Drawing.Point(211, 12);
			this.grpColChanges.Name = "grpColChanges";
			this.grpColChanges.Size = new System.Drawing.Size(310, 191);
			this.grpColChanges.TabIndex = 1;
			this.grpColChanges.TabStop = false;
			this.grpColChanges.Text = "Column Changes";
			// 
			// lblExplanation
			// 
			this.lblExplanation.AutoEllipsis = true;
			this.lblExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblExplanation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExplanation, null);
			this.locExtender.SetLocalizationComment(this.lblExplanation, "Label on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblExplanation, "OptionsDlg.WordListTab.lblExplanation");
			this.lblExplanation.Location = new System.Drawing.Point(6, 21);
			this.lblExplanation.Name = "lblExplanation";
			this.lblExplanation.Size = new System.Drawing.Size(295, 52);
			this.lblExplanation.TabIndex = 0;
			this.lblExplanation.Text = "Use the following to control how the last changes made to any existing word list " +
				"will affect new word lists.";
			// 
			// chkSaveReorderedColumns
			// 
			this.chkSaveReorderedColumns.AutoEllipsis = true;
			this.chkSaveReorderedColumns.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveReorderedColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkSaveReorderedColumns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkSaveReorderedColumns, null);
			this.locExtender.SetLocalizationComment(this.chkSaveReorderedColumns, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveReorderedColumns, "OptionsDlg.WordListTab.chkSaveReorderedColumns");
			this.chkSaveReorderedColumns.Location = new System.Drawing.Point(9, 76);
			this.chkSaveReorderedColumns.Name = "chkSaveReorderedColumns";
			this.chkSaveReorderedColumns.Size = new System.Drawing.Size(292, 35);
			this.chkSaveReorderedColumns.TabIndex = 1;
			this.chkSaveReorderedColumns.Text = "Save arrangement of &reordered columns as default order";
			this.chkSaveReorderedColumns.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveReorderedColumns.UseVisualStyleBackColor = true;
			// 
			// chkSaveColHdrHeight
			// 
			this.chkSaveColHdrHeight.AutoEllipsis = true;
			this.chkSaveColHdrHeight.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColHdrHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkSaveColHdrHeight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkSaveColHdrHeight, null);
			this.locExtender.SetLocalizationComment(this.chkSaveColHdrHeight, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveColHdrHeight, "OptionsDlg.WordListTab.chkSaveColHdrHeight");
			this.chkSaveColHdrHeight.Location = new System.Drawing.Point(9, 152);
			this.chkSaveColHdrHeight.Name = "chkSaveColHdrHeight";
			this.chkSaveColHdrHeight.Size = new System.Drawing.Size(292, 35);
			this.chkSaveColHdrHeight.TabIndex = 3;
			this.chkSaveColHdrHeight.Text = "Save adjusted column heading &height as default height";
			this.chkSaveColHdrHeight.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColHdrHeight.UseVisualStyleBackColor = true;
			// 
			// chkSaveColWidths
			// 
			this.chkSaveColWidths.AutoEllipsis = true;
			this.chkSaveColWidths.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColWidths.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkSaveColWidths.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkSaveColWidths, null);
			this.locExtender.SetLocalizationComment(this.chkSaveColWidths, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveColWidths, "OptionsDlg.WordListTab.chkSaveColWidths");
			this.chkSaveColWidths.Location = new System.Drawing.Point(9, 114);
			this.chkSaveColWidths.Name = "chkSaveColWidths";
			this.chkSaveColWidths.Size = new System.Drawing.Size(292, 35);
			this.chkSaveColWidths.TabIndex = 2;
			this.chkSaveColWidths.Text = "Save adjusted column &widths as default widths";
			this.chkSaveColWidths.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveColWidths.UseVisualStyleBackColor = true;
			// 
			// nudMaxEticColWidth
			// 
			this.locExtender.SetLocalizableToolTip(this.nudMaxEticColWidth, "Maximum automatically calculated phonetic column width");
			this.locExtender.SetLocalizationComment(this.nudMaxEticColWidth, "Phonetic column width adjuster on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.nudMaxEticColWidth, "OptionsDlg.WordListTab.nudMaxEticColWidth");
			this.nudMaxEticColWidth.Location = new System.Drawing.Point(462, 345);
			this.nudMaxEticColWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudMaxEticColWidth.Name = "nudMaxEticColWidth";
			this.nudMaxEticColWidth.Size = new System.Drawing.Size(59, 20);
			this.nudMaxEticColWidth.TabIndex = 4;
			this.nudMaxEticColWidth.Visible = false;
			// 
			// chkAutoAdjustPhoneticCol
			// 
			this.chkAutoAdjustPhoneticCol.AutoEllipsis = true;
			this.chkAutoAdjustPhoneticCol.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkAutoAdjustPhoneticCol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkAutoAdjustPhoneticCol.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkAutoAdjustPhoneticCol, null);
			this.locExtender.SetLocalizationComment(this.chkAutoAdjustPhoneticCol, "Check box text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkAutoAdjustPhoneticCol, "OptionsDlg.WordListTab.chkAutoAdjustPhoneticCol");
			this.chkAutoAdjustPhoneticCol.Location = new System.Drawing.Point(11, 316);
			this.chkAutoAdjustPhoneticCol.Name = "chkAutoAdjustPhoneticCol";
			this.chkAutoAdjustPhoneticCol.Size = new System.Drawing.Size(451, 54);
			this.chkAutoAdjustPhoneticCol.TabIndex = 3;
			this.chkAutoAdjustPhoneticCol.Text = resources.GetString("chkAutoAdjustPhoneticCol.Text");
			this.chkAutoAdjustPhoneticCol.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkAutoAdjustPhoneticCol.UseVisualStyleBackColor = true;
			this.chkAutoAdjustPhoneticCol.Visible = false;
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
			this.grpGridLines.Location = new System.Drawing.Point(211, 214);
			this.grpGridLines.Name = "grpGridLines";
			this.grpGridLines.Size = new System.Drawing.Size(310, 88);
			this.grpGridLines.TabIndex = 2;
			this.grpGridLines.TabStop = false;
			this.grpGridLines.Text = "Grid Lines";
			// 
			// rbGridLinesHorizontal
			// 
			this.rbGridLinesHorizontal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.rbGridLinesHorizontal.Image = global::SIL.Pa.Properties.Resources.kimidHorizontalGridLines;
			this.rbGridLinesHorizontal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesHorizontal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesHorizontal, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesHorizontal, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesHorizontal, "OptionsDlg.WordListTab.rbGridLinesHorizontal");
			this.rbGridLinesHorizontal.Location = new System.Drawing.Point(10, 49);
			this.rbGridLinesHorizontal.Name = "rbGridLinesHorizontal";
			this.rbGridLinesHorizontal.Size = new System.Drawing.Size(143, 24);
			this.rbGridLinesHorizontal.TabIndex = 1;
			this.rbGridLinesHorizontal.TabStop = true;
			this.rbGridLinesHorizontal.Text = "  H&orizontal only";
			this.rbGridLinesHorizontal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesHorizontal.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesVertical
			// 
			this.rbGridLinesVertical.AutoEllipsis = true;
			this.rbGridLinesVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.rbGridLinesVertical.Image = global::SIL.Pa.Properties.Resources.kimidVerticalGridLines;
			this.rbGridLinesVertical.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesVertical.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesVertical, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesVertical, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesVertical, "OptionsDlg.WordListTab.rbGridLinesVertical");
			this.rbGridLinesVertical.Location = new System.Drawing.Point(10, 19);
			this.rbGridLinesVertical.Name = "rbGridLinesVertical";
			this.rbGridLinesVertical.Size = new System.Drawing.Size(143, 24);
			this.rbGridLinesVertical.TabIndex = 0;
			this.rbGridLinesVertical.TabStop = true;
			this.rbGridLinesVertical.Text = "  &Vertical only";
			this.rbGridLinesVertical.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesVertical.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesBoth
			// 
			this.rbGridLinesBoth.AutoEllipsis = true;
			this.rbGridLinesBoth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.rbGridLinesBoth.Image = global::SIL.Pa.Properties.Resources.kimidBothGridLines;
			this.rbGridLinesBoth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesBoth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesBoth, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesBoth, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesBoth, "OptionsDlg.WordListTab.rbGridLinesBoth");
			this.rbGridLinesBoth.Location = new System.Drawing.Point(159, 19);
			this.rbGridLinesBoth.Name = "rbGridLinesBoth";
			this.rbGridLinesBoth.Size = new System.Drawing.Size(146, 24);
			this.rbGridLinesBoth.TabIndex = 2;
			this.rbGridLinesBoth.TabStop = true;
			this.rbGridLinesBoth.Text = "  &Both";
			this.rbGridLinesBoth.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesBoth.UseVisualStyleBackColor = true;
			// 
			// rbGridLinesNone
			// 
			this.rbGridLinesNone.AutoEllipsis = true;
			this.rbGridLinesNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.rbGridLinesNone.Image = global::SIL.Pa.Properties.Resources.kimidNoGridLines;
			this.rbGridLinesNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.rbGridLinesNone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rbGridLinesNone, null);
			this.locExtender.SetLocalizationComment(this.rbGridLinesNone, "Radio button text on word list tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rbGridLinesNone, "OptionsDlg.WordListTab.rbGridLinesNone");
			this.rbGridLinesNone.Location = new System.Drawing.Point(159, 49);
			this.rbGridLinesNone.Name = "rbGridLinesNone";
			this.rbGridLinesNone.Size = new System.Drawing.Size(146, 24);
			this.rbGridLinesNone.TabIndex = 3;
			this.rbGridLinesNone.TabStop = true;
			this.rbGridLinesNone.Text = "  &None";
			this.rbGridLinesNone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.rbGridLinesNone.UseVisualStyleBackColor = true;
			// 
			// tpgRecView
			// 
			this.tpgRecView.Controls.Add(this.grpFieldSettings);
			this.locExtender.SetLocalizableToolTip(this.tpgRecView, null);
			this.locExtender.SetLocalizationComment(this.tpgRecView, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgRecView, "OptionsDlg.RecordViewTab.Text");
			this.tpgRecView.Location = new System.Drawing.Point(4, 22);
			this.tpgRecView.Name = "tpgRecView";
			this.tpgRecView.Padding = new System.Windows.Forms.Padding(3);
			this.tpgRecView.Size = new System.Drawing.Size(532, 382);
			this.tpgRecView.TabIndex = 5;
			this.tpgRecView.Text = "Record View";
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
			this.grpFieldSettings.Location = new System.Drawing.Point(15, 12);
			this.grpFieldSettings.Name = "grpFieldSettings";
			this.grpFieldSettings.Size = new System.Drawing.Size(225, 341);
			this.grpFieldSettings.TabIndex = 0;
			this.grpFieldSettings.TabStop = false;
			this.grpFieldSettings.Text = "Field Display Options";
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
			this.fldSelGridRecView.DrawTextBoxEditControlBorder = false;
			this.fldSelGridRecView.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.fldSelGridRecView.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.fldSelGridRecView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridRecView.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridRecView, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridRecView, null);
			this.locExtender.SetLocalizationPriority(this.fldSelGridRecView, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.fldSelGridRecView, "OptionsDlg.RecordViewTab.fldSelGridRecView");
			this.fldSelGridRecView.Location = new System.Drawing.Point(11, 57);
			this.fldSelGridRecView.MultiSelect = false;
			this.fldSelGridRecView.Name = "fldSelGridRecView";
			this.fldSelGridRecView.PaintHeaderAcrossFullGridWidth = true;
			this.fldSelGridRecView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridRecView.RowHeadersVisible = false;
			this.fldSelGridRecView.RowHeadersWidth = 22;
			this.fldSelGridRecView.SelectedCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridRecView.SelectedCellForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridRecView.SelectedRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridRecView.SelectedRowForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridRecView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridRecView.ShowWaterMarkWhenDirty = false;
			this.fldSelGridRecView.Size = new System.Drawing.Size(173, 273);
			this.fldSelGridRecView.TabIndex = 1;
			this.fldSelGridRecView.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.fldSelGridRecView.WaterMark = "!";
			this.fldSelGridRecView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.fldSelGridRecView_RowEnter);
			// 
			// lblShowFields
			// 
			this.lblShowFields.AutoEllipsis = true;
			this.lblShowFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblShowFields.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblShowFields, null);
			this.locExtender.SetLocalizationComment(this.lblShowFields, "Label on record view tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblShowFields, "OptionsDlg.RecordViewTab.lblShowFields");
			this.lblShowFields.Location = new System.Drawing.Point(13, 18);
			this.lblShowFields.Name = "lblShowFields";
			this.lblShowFields.Size = new System.Drawing.Size(171, 37);
			this.lblShowFields.TabIndex = 0;
			this.lblShowFields.Text = "&Specify the fields to display and their order.";
			this.lblShowFields.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnMoveRecVwFldDown
			// 
			this.btnMoveRecVwFldDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			this.btnMoveRecVwFldDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveRecVwFldDown, "Move Down");
			this.locExtender.SetLocalizationComment(this.btnMoveRecVwFldDown, "Button on record view of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveRecVwFldDown, "OptionsDlg.RecordViewTab.btnMoveRecVwFldDown");
			this.btnMoveRecVwFldDown.Location = new System.Drawing.Point(190, 84);
			this.btnMoveRecVwFldDown.Name = "btnMoveRecVwFldDown";
			this.btnMoveRecVwFldDown.Size = new System.Drawing.Size(24, 24);
			this.btnMoveRecVwFldDown.TabIndex = 3;
			this.btnMoveRecVwFldDown.UseVisualStyleBackColor = true;
			this.btnMoveRecVwFldDown.Click += new System.EventHandler(this.btnMoveRecVwFldDown_Click);
			// 
			// btnMoveRecVwFldUp
			// 
			this.btnMoveRecVwFldUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			this.btnMoveRecVwFldUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveRecVwFldUp, "Move Up");
			this.locExtender.SetLocalizationComment(this.btnMoveRecVwFldUp, "Button on record view tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveRecVwFldUp, "OptionsDlg.RecordViewTab.btnMoveRecVwFldUp");
			this.btnMoveRecVwFldUp.Location = new System.Drawing.Point(190, 57);
			this.btnMoveRecVwFldUp.Name = "btnMoveRecVwFldUp";
			this.btnMoveRecVwFldUp.Size = new System.Drawing.Size(24, 24);
			this.btnMoveRecVwFldUp.TabIndex = 2;
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
			this.locExtender.SetLocalizingId(this.tpgFindPhones, "OptionsDlg.SearchPatternsTab.Text");
			this.tpgFindPhones.Location = new System.Drawing.Point(4, 22);
			this.tpgFindPhones.Name = "tpgFindPhones";
			this.tpgFindPhones.Size = new System.Drawing.Size(532, 382);
			this.tpgFindPhones.TabIndex = 2;
			this.tpgFindPhones.Text = "Search Patterns";
			this.tpgFindPhones.UseVisualStyleBackColor = true;
			// 
			// lblShowDiamondPattern
			// 
			this.lblShowDiamondPattern.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblShowDiamondPattern, null);
			this.locExtender.SetLocalizationComment(this.lblShowDiamondPattern, "Label text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblShowDiamondPattern, "OptionsDlg.SearchPatternsTab.lblShowDiamondPattern");
			this.lblShowDiamondPattern.Location = new System.Drawing.Point(45, 189);
			this.lblShowDiamondPattern.Name = "lblShowDiamondPattern";
			this.lblShowDiamondPattern.Size = new System.Drawing.Size(261, 177);
			this.lblShowDiamondPattern.TabIndex = 4;
			this.lblShowDiamondPattern.Text = "Displays a diamond pattern (i.e. {0}) when the Current Search Pattern is empty in" +
				" the Search view.";
			// 
			// chkShowDiamondPattern
			// 
			this.chkShowDiamondPattern.AutoSize = true;
			this.chkShowDiamondPattern.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowDiamondPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkShowDiamondPattern.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkShowDiamondPattern, null);
			this.locExtender.SetLocalizationComment(this.chkShowDiamondPattern, "Check box text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkShowDiamondPattern, "OptionsDlg.SearchPatternsTab.chkShowDiamondPattern");
			this.chkShowDiamondPattern.Location = new System.Drawing.Point(29, 170);
			this.chkShowDiamondPattern.Name = "chkShowDiamondPattern";
			this.chkShowDiamondPattern.Size = new System.Drawing.Size(159, 19);
			this.chkShowDiamondPattern.TabIndex = 3;
			this.chkShowDiamondPattern.Text = "&Display diamond pattern";
			this.chkShowDiamondPattern.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkShowDiamondPattern.UseVisualStyleBackColor = true;
			// 
			// grpClassSettings
			// 
			this.grpClassSettings.AutoSize = true;
			this.grpClassSettings.Controls.Add(this.rdoClassMembers);
			this.grpClassSettings.Controls.Add(this.rdoClassName);
			this.grpClassSettings.Controls.Add(this.lblClassDisplayBehavior);
			this.locExtender.SetLocalizableToolTip(this.grpClassSettings, null);
			this.locExtender.SetLocalizationComment(this.grpClassSettings, "Frame text on search pattern tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpClassSettings, "OptionsDlg.SearchPatternsTab.grpClassSettings");
			this.grpClassSettings.Location = new System.Drawing.Point(17, 13);
			this.grpClassSettings.Name = "grpClassSettings";
			this.grpClassSettings.Size = new System.Drawing.Size(231, 134);
			this.grpClassSettings.TabIndex = 2;
			this.grpClassSettings.TabStop = false;
			this.grpClassSettings.Text = "Class Display Behavior";
			// 
			// rdoClassMembers
			// 
			this.rdoClassMembers.AutoSize = true;
			this.rdoClassMembers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rdoClassMembers, null);
			this.locExtender.SetLocalizationComment(this.rdoClassMembers, "Radio button text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rdoClassMembers, "OptionsDlg.SearchPatternsTab.rdoClassMembers");
			this.rdoClassMembers.Location = new System.Drawing.Point(12, 98);
			this.rdoClassMembers.Name = "rdoClassMembers";
			this.rdoClassMembers.Size = new System.Drawing.Size(95, 17);
			this.rdoClassMembers.TabIndex = 1;
			this.rdoClassMembers.TabStop = true;
			this.rdoClassMembers.Text = "Class &members";
			this.rdoClassMembers.UseVisualStyleBackColor = true;
			// 
			// rdoClassName
			// 
			this.rdoClassName.AutoSize = true;
			this.rdoClassName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.rdoClassName, null);
			this.locExtender.SetLocalizationComment(this.rdoClassName, "Radio button text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.rdoClassName, "OptionsDlg.SearchPatternsTab.rdoClassName");
			this.rdoClassName.Location = new System.Drawing.Point(12, 75);
			this.rdoClassName.Name = "rdoClassName";
			this.rdoClassName.Size = new System.Drawing.Size(79, 17);
			this.rdoClassName.TabIndex = 0;
			this.rdoClassName.TabStop = true;
			this.rdoClassName.Text = "Class &name";
			this.rdoClassName.UseVisualStyleBackColor = true;
			// 
			// lblClassDisplayBehavior
			// 
			this.lblClassDisplayBehavior.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblClassDisplayBehavior, null);
			this.locExtender.SetLocalizationComment(this.lblClassDisplayBehavior, "Label text on search patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblClassDisplayBehavior, "OptionsDlg.SearchPatternsTab.lblClassDisplayBehavior");
			this.lblClassDisplayBehavior.Location = new System.Drawing.Point(9, 20);
			this.lblClassDisplayBehavior.Name = "lblClassDisplayBehavior";
			this.lblClassDisplayBehavior.Size = new System.Drawing.Size(216, 44);
			this.lblClassDisplayBehavior.TabIndex = 3;
			this.lblClassDisplayBehavior.Text = "When displaying classes in search patterns and nested class definitions, show:";
			this.lblClassDisplayBehavior.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.locExtender.SetLocalizingId(this.tpgCVPatterns, "OptionsDlg.CVPatternsTab.Text");
			this.tpgCVPatterns.Location = new System.Drawing.Point(4, 22);
			this.tpgCVPatterns.Name = "tpgCVPatterns";
			this.tpgCVPatterns.Padding = new System.Windows.Forms.Padding(3);
			this.tpgCVPatterns.Size = new System.Drawing.Size(532, 382);
			this.tpgCVPatterns.TabIndex = 6;
			this.tpgCVPatterns.Text = "CV Patterns";
			this.tpgCVPatterns.UseVisualStyleBackColor = true;
			// 
			// grpDisplayChars
			// 
			this.grpDisplayChars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
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
			this.grpDisplayChars.Location = new System.Drawing.Point(293, 8);
			this.grpDisplayChars.Name = "grpDisplayChars";
			this.grpDisplayChars.Padding = new System.Windows.Forms.Padding(7, 10, 7, 7);
			this.grpDisplayChars.Size = new System.Drawing.Size(225, 362);
			this.grpDisplayChars.TabIndex = 14;
			this.grpDisplayChars.TabStop = false;
			this.grpDisplayChars.Text = "Display these characters";
			// 
			// lblExampleDesc2
			// 
			this.lblExampleDesc2.AutoSize = true;
			this.lblExampleDesc2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleDesc2, null);
			this.locExtender.SetLocalizationComment(this.lblExampleDesc2, "Label on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblExampleDesc2, "OptionsDlg.CVPatternsTab.lblExampleDesc2");
			this.lblExampleDesc2.Location = new System.Drawing.Point(13, 203);
			this.lblExampleDesc2.Name = "lblExampleDesc2";
			this.lblExampleDesc2.Size = new System.Drawing.Size(62, 13);
			this.lblExampleDesc2.TabIndex = 9;
			this.lblExampleDesc2.Text = "would yield:";
			// 
			// lblExampleCV
			// 
			this.lblExampleCV.AutoSize = true;
			this.lblExampleCV.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.lblExampleCV.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleCV, null);
			this.locExtender.SetLocalizationComment(this.lblExampleCV, null);
			this.locExtender.SetLocalizationPriority(this.lblExampleCV, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblExampleCV, "OptionsDlg.CVPatternsTab.lblExampleCV");
			this.lblExampleCV.Location = new System.Drawing.Point(89, 227);
			this.lblExampleCV.Name = "lblExampleCV";
			this.lblExampleCV.Size = new System.Drawing.Size(46, 24);
			this.lblExampleCV.TabIndex = 6;
			this.lblExampleCV.Text = "CVʔ";
			this.lblExampleCV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblExampleCV.UseMnemonic = false;
			// 
			// txtCustomChars
			// 
			this.txtCustomChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.locExtender.SetLocalizableToolTip(this.txtCustomChars, null);
			this.locExtender.SetLocalizationComment(this.txtCustomChars, null);
			this.locExtender.SetLocalizationPriority(this.txtCustomChars, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtCustomChars, "OptionsDlg.CVPatternsTab.txtCustomChars");
			this.txtCustomChars.Location = new System.Drawing.Point(16, 77);
			this.txtCustomChars.Name = "txtCustomChars";
			this.txtCustomChars.Size = new System.Drawing.Size(193, 29);
			this.txtCustomChars.TabIndex = 0;
			this.txtCustomChars.TextChanged += new System.EventHandler(this.txtCustomChars_TextChanged);
			this.txtCustomChars.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomChars_KeyDown);
			// 
			// lblExampleCVCV
			// 
			this.lblExampleCVCV.AutoSize = true;
			this.lblExampleCVCV.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.lblExampleCVCV.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleCVCV, null);
			this.locExtender.SetLocalizationComment(this.lblExampleCVCV, null);
			this.locExtender.SetLocalizationPriority(this.lblExampleCVCV, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblExampleCVCV, "OptionsDlg.CVPatternsTab.lblExampleCVCV");
			this.lblExampleCVCV.Location = new System.Drawing.Point(25, 227);
			this.lblExampleCVCV.Name = "lblExampleCVCV";
			this.lblExampleCVCV.Size = new System.Drawing.Size(62, 24);
			this.lblExampleCVCV.TabIndex = 7;
			this.lblExampleCVCV.Text = "CṼCV";
			this.lblExampleCVCV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblExampleCVCV.UseMnemonic = false;
			// 
			// lblInstruction
			// 
			this.lblInstruction.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblInstruction, null);
			this.locExtender.SetLocalizationComment(this.lblInstruction, "Label on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblInstruction, "OptionsDlg.CVPatternsTab.lblInstruction");
			this.lblInstruction.Location = new System.Drawing.Point(13, 23);
			this.lblInstruction.Name = "lblInstruction";
			this.lblInstruction.Size = new System.Drawing.Size(199, 47);
			this.lblInstruction.TabIndex = 2;
			this.lblInstruction.Text = "To include characters not shown in the lists to the left, enter them below. Inclu" +
				"de a space between each character. Include a diacritic placeholder (Ctrl+0) for " +
				"non base characters.";
			// 
			// txtExampleInput
			// 
			this.txtExampleInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
			this.locExtender.SetLocalizableToolTip(this.txtExampleInput, null);
			this.locExtender.SetLocalizationComment(this.txtExampleInput, null);
			this.locExtender.SetLocalizationPriority(this.txtExampleInput, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtExampleInput, "OptionsDlg.CVPatternsTab.txtExampleInput");
			this.txtExampleInput.Location = new System.Drawing.Point(16, 157);
			this.txtExampleInput.Name = "txtExampleInput";
			this.txtExampleInput.ReadOnly = true;
			this.txtExampleInput.Size = new System.Drawing.Size(193, 29);
			this.txtExampleInput.TabIndex = 8;
			this.txtExampleInput.Text = "◌̃ ʔ";
			// 
			// lblExampleDesc1
			// 
			this.lblExampleDesc1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblExampleDesc1, null);
			this.locExtender.SetLocalizationComment(this.lblExampleDesc1, "Label on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblExampleDesc1, "OptionsDlg.CVPatternsTab.lblExampleDesc1");
			this.lblExampleDesc1.Location = new System.Drawing.Point(13, 111);
			this.lblExampleDesc1.Name = "lblExampleDesc1";
			this.lblExampleDesc1.Size = new System.Drawing.Size(199, 38);
			this.lblExampleDesc1.TabIndex = 3;
			this.lblExampleDesc1.Text = "Examples: Entering nasalization and a glottal stop like this above,";
			// 
			// chkTone
			// 
			this.chkTone.AutoSize = true;
			this.chkTone.BackColor = System.Drawing.Color.Transparent;
			this.chkTone.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkTone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkTone, null);
			this.locExtender.SetLocalizationComment(this.chkTone, "Check box text on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkTone, "OptionsDlg.CVPatternsTab.chkTone");
			this.chkTone.Location = new System.Drawing.Point(24, 164);
			this.chkTone.Name = "chkTone";
			this.chkTone.Size = new System.Drawing.Size(88, 17);
			this.chkTone.TabIndex = 11;
			this.chkTone.Text = "Display &Tone";
			this.chkTone.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkTone.ThreeState = true;
			this.chkTone.UseVisualStyleBackColor = false;
			// 
			// grpTone
			// 
			this.grpTone.Controls.Add(this.pnlTone);
			this.locExtender.SetLocalizableToolTip(this.grpTone, null);
			this.locExtender.SetLocalizationComment(this.grpTone, null);
			this.locExtender.SetLocalizationPriority(this.grpTone, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpTone, "OptionsDlg.CVPatternsTab.grpTone");
			this.grpTone.Location = new System.Drawing.Point(14, 164);
			this.grpTone.Name = "grpTone";
			this.grpTone.Padding = new System.Windows.Forms.Padding(7, 10, 7, 7);
			this.grpTone.Size = new System.Drawing.Size(270, 205);
			this.grpTone.TabIndex = 12;
			this.grpTone.TabStop = false;
			// 
			// pnlTone
			// 
			this.pnlTone.AutoScroll = true;
			this.pnlTone.Controls.Add(this.tonePicker);
			this.pnlTone.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTone.Location = new System.Drawing.Point(7, 23);
			this.pnlTone.Name = "pnlTone";
			this.pnlTone.Size = new System.Drawing.Size(256, 175);
			this.pnlTone.TabIndex = 2;
			// 
			// tonePicker
			// 
			this.tonePicker.AutoSize = false;
			this.tonePicker.AutoSizeItems = false;
			this.tonePicker.BackColor = System.Drawing.Color.Transparent;
			this.tonePicker.CheckItemsOnClick = true;
			this.tonePicker.Dock = System.Windows.Forms.DockStyle.None;
			this.tonePicker.FontSize = 14F;
			this.tonePicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tonePicker.ItemSize = new System.Drawing.Size(30, 32);
			this.tonePicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this.tonePicker, null);
			this.locExtender.SetLocalizationComment(this.tonePicker, null);
			this.locExtender.SetLocalizationPriority(this.tonePicker, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tonePicker, "OptionsDlg.CVPatternsTab.tonePicker");
			this.tonePicker.Location = new System.Drawing.Point(0, 0);
			this.tonePicker.Name = "tonePicker";
			this.tonePicker.Padding = new System.Windows.Forms.Padding(0);
			this.tonePicker.Size = new System.Drawing.Size(197, 59);
			this.tonePicker.TabIndex = 0;
			// 
			// chkLength
			// 
			this.chkLength.AutoSize = true;
			this.chkLength.BackColor = System.Drawing.Color.Transparent;
			this.chkLength.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkLength, null);
			this.locExtender.SetLocalizationComment(this.chkLength, "Check box text on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkLength, "OptionsDlg.CVPatternsTab.chkLength");
			this.chkLength.Location = new System.Drawing.Point(24, 86);
			this.chkLength.Name = "chkLength";
			this.chkLength.Size = new System.Drawing.Size(96, 17);
			this.chkLength.TabIndex = 9;
			this.chkLength.Text = "Display &Length";
			this.chkLength.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkLength.ThreeState = true;
			this.chkLength.UseVisualStyleBackColor = false;
			// 
			// grpLength
			// 
			this.grpLength.Controls.Add(this.pnlLength);
			this.locExtender.SetLocalizableToolTip(this.grpLength, null);
			this.locExtender.SetLocalizationComment(this.grpLength, null);
			this.locExtender.SetLocalizationPriority(this.grpLength, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpLength, "OptionsDlg.CVPatternsTab.grpLength");
			this.grpLength.Location = new System.Drawing.Point(14, 86);
			this.grpLength.Name = "grpLength";
			this.grpLength.Padding = new System.Windows.Forms.Padding(7, 10, 7, 7);
			this.grpLength.Size = new System.Drawing.Size(270, 70);
			this.grpLength.TabIndex = 10;
			this.grpLength.TabStop = false;
			// 
			// pnlLength
			// 
			this.pnlLength.AutoScroll = true;
			this.pnlLength.Controls.Add(this.lengthPicker);
			this.pnlLength.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlLength.Location = new System.Drawing.Point(7, 23);
			this.pnlLength.Name = "pnlLength";
			this.pnlLength.Size = new System.Drawing.Size(256, 40);
			this.pnlLength.TabIndex = 1;
			// 
			// lengthPicker
			// 
			this.lengthPicker.AutoSize = false;
			this.lengthPicker.AutoSizeItems = false;
			this.lengthPicker.BackColor = System.Drawing.Color.Transparent;
			this.lengthPicker.CheckItemsOnClick = true;
			this.lengthPicker.Dock = System.Windows.Forms.DockStyle.None;
			this.lengthPicker.FontSize = 14F;
			this.lengthPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.lengthPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.lengthPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this.lengthPicker, null);
			this.locExtender.SetLocalizationComment(this.lengthPicker, null);
			this.locExtender.SetLocalizationPriority(this.lengthPicker, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lengthPicker, "OptionsDlg.CVPatternsTab.lengthPicker");
			this.lengthPicker.Location = new System.Drawing.Point(0, 0);
			this.lengthPicker.Name = "lengthPicker";
			this.lengthPicker.Padding = new System.Windows.Forms.Padding(0);
			this.lengthPicker.Size = new System.Drawing.Size(119, 40);
			this.lengthPicker.TabIndex = 0;
			this.lengthPicker.Text = "charPicker1";
			// 
			// chkStress
			// 
			this.chkStress.AutoSize = true;
			this.chkStress.BackColor = System.Drawing.Color.Transparent;
			this.chkStress.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkStress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkStress, null);
			this.locExtender.SetLocalizationComment(this.chkStress, "Check box text on CV patterns tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkStress, "OptionsDlg.CVPatternsTab.chkStress");
			this.chkStress.Location = new System.Drawing.Point(24, 8);
			this.chkStress.Name = "chkStress";
			this.chkStress.Size = new System.Drawing.Size(133, 17);
			this.chkStress.TabIndex = 5;
			this.chkStress.Text = "Display &Stress/Syllable";
			this.chkStress.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkStress.ThreeState = true;
			this.chkStress.UseVisualStyleBackColor = false;
			// 
			// grpStress
			// 
			this.grpStress.Controls.Add(this.pnlStress);
			this.locExtender.SetLocalizableToolTip(this.grpStress, null);
			this.locExtender.SetLocalizationComment(this.grpStress, null);
			this.locExtender.SetLocalizationPriority(this.grpStress, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.grpStress, "OptionsDlg.CVPatternsTab.grpStress");
			this.grpStress.Location = new System.Drawing.Point(14, 8);
			this.grpStress.Name = "grpStress";
			this.grpStress.Padding = new System.Windows.Forms.Padding(7, 10, 7, 7);
			this.grpStress.Size = new System.Drawing.Size(270, 70);
			this.grpStress.TabIndex = 6;
			this.grpStress.TabStop = false;
			// 
			// pnlStress
			// 
			this.pnlStress.AutoScroll = true;
			this.pnlStress.Controls.Add(this.stressPicker);
			this.pnlStress.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlStress.Location = new System.Drawing.Point(7, 23);
			this.pnlStress.Name = "pnlStress";
			this.pnlStress.Size = new System.Drawing.Size(256, 40);
			this.pnlStress.TabIndex = 0;
			// 
			// stressPicker
			// 
			this.stressPicker.AutoSize = false;
			this.stressPicker.AutoSizeItems = false;
			this.stressPicker.BackColor = System.Drawing.Color.Transparent;
			this.stressPicker.CheckItemsOnClick = true;
			this.stressPicker.Dock = System.Windows.Forms.DockStyle.None;
			this.stressPicker.FontSize = 14F;
			this.stressPicker.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.stressPicker.ItemSize = new System.Drawing.Size(30, 32);
			this.stressPicker.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.locExtender.SetLocalizableToolTip(this.stressPicker, null);
			this.locExtender.SetLocalizationComment(this.stressPicker, null);
			this.locExtender.SetLocalizationPriority(this.stressPicker, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.stressPicker, "OptionsDlg.CVPatternsTab.stressPicker");
			this.stressPicker.Location = new System.Drawing.Point(0, 0);
			this.stressPicker.Name = "stressPicker";
			this.stressPicker.Padding = new System.Windows.Forms.Padding(0);
			this.stressPicker.Size = new System.Drawing.Size(136, 40);
			this.stressPicker.TabIndex = 0;
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
			this.locExtender.SetLocalizationPriority(this.tpgSorting, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tpgSorting, "OptionsDlg.SortingTab.Text");
			this.tpgSorting.Location = new System.Drawing.Point(4, 22);
			this.tpgSorting.Name = "tpgSorting";
			this.tpgSorting.Padding = new System.Windows.Forms.Padding(15, 13, 15, 0);
			this.tpgSorting.Size = new System.Drawing.Size(532, 382);
			this.tpgSorting.TabIndex = 7;
			this.tpgSorting.Text = "Sorting";
			this.tpgSorting.UseVisualStyleBackColor = true;
			// 
			// lblSaveManual
			// 
			this.lblSaveManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblSaveManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSaveManual, null);
			this.locExtender.SetLocalizationComment(this.lblSaveManual, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSaveManual, "OptionsDlg.SortingTab.lblSaveManual");
			this.lblSaveManual.Location = new System.Drawing.Point(33, 343);
			this.lblSaveManual.Name = "lblSaveManual";
			this.lblSaveManual.Size = new System.Drawing.Size(480, 32);
			this.lblSaveManual.TabIndex = 6;
			this.lblSaveManual.Text = "For example, clicking column headings in the Data Corpus view will change the def" +
				"ault options set on this tab for that view.";
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
			this.grpColSortOptions.Location = new System.Drawing.Point(11, 96);
			this.grpColSortOptions.Name = "grpColSortOptions";
			this.grpColSortOptions.Size = new System.Drawing.Size(273, 210);
			this.grpColSortOptions.TabIndex = 3;
			this.grpColSortOptions.TabStop = false;
			this.grpColSortOptions.Text = "Column Sort Options";
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
			this.m_sortingGrid.DrawTextBoxEditControlBorder = false;
			this.m_sortingGrid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.m_sortingGrid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_sortingGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_sortingGrid.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.m_sortingGrid, null);
			this.locExtender.SetLocalizationComment(this.m_sortingGrid, null);
			this.locExtender.SetLocalizationPriority(this.m_sortingGrid, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_sortingGrid, "OptionsDlg.SortingTab.Grid");
			this.m_sortingGrid.Location = new System.Drawing.Point(8, 57);
			this.m_sortingGrid.MultiSelect = false;
			this.m_sortingGrid.Name = "m_sortingGrid";
			this.m_sortingGrid.PaintHeaderAcrossFullGridWidth = true;
			this.m_sortingGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_sortingGrid.RowHeadersVisible = false;
			this.m_sortingGrid.RowHeadersWidth = 22;
			this.m_sortingGrid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_sortingGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_sortingGrid.ShowWaterMarkWhenDirty = false;
			this.m_sortingGrid.Size = new System.Drawing.Size(228, 145);
			this.m_sortingGrid.TabIndex = 1;
			this.m_sortingGrid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_sortingGrid.WaterMark = "!";
			this.m_sortingGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleSortingGridCellContentClick);
			this.m_sortingGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleSortingGridRowEnter);
			// 
			// btnMoveSortFieldUp
			// 
			this.btnMoveSortFieldUp.Image = global::SIL.Pa.Properties.Resources.kimidMoveUp;
			this.btnMoveSortFieldUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveSortFieldUp, "Move Up");
			this.locExtender.SetLocalizationComment(this.btnMoveSortFieldUp, "Button on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveSortFieldUp, "OptionsDlg.SortingTab.btnMoveSortFieldUp");
			this.btnMoveSortFieldUp.Location = new System.Drawing.Point(242, 57);
			this.btnMoveSortFieldUp.Name = "btnMoveSortFieldUp";
			this.btnMoveSortFieldUp.Size = new System.Drawing.Size(24, 25);
			this.btnMoveSortFieldUp.TabIndex = 2;
			this.btnMoveSortFieldUp.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldUp.Click += new System.EventHandler(this.HandleButtonMoveSortFieldUpClick);
			// 
			// lblSortFldsHdr
			// 
			this.lblSortFldsHdr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblSortFldsHdr.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSortFldsHdr, null);
			this.locExtender.SetLocalizationComment(this.lblSortFldsHdr, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSortFldsHdr, "OptionsDlg.SortingTab.lblSortFldsHdr");
			this.lblSortFldsHdr.Location = new System.Drawing.Point(10, 18);
			this.lblSortFldsHdr.Name = "lblSortFldsHdr";
			this.lblSortFldsHdr.Size = new System.Drawing.Size(222, 37);
			this.lblSortFldsHdr.TabIndex = 0;
			this.lblSortFldsHdr.Text = "Specify the &Columns to sort and in what order.";
			this.lblSortFldsHdr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnMoveSortFieldDown
			// 
			this.btnMoveSortFieldDown.Image = global::SIL.Pa.Properties.Resources.kimidMoveDown;
			this.btnMoveSortFieldDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnMoveSortFieldDown, "Move Down");
			this.locExtender.SetLocalizationComment(this.btnMoveSortFieldDown, "Button on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.btnMoveSortFieldDown, "OptionsDlg.SortingTab.btnMoveSortFieldDown");
			this.btnMoveSortFieldDown.Location = new System.Drawing.Point(242, 84);
			this.btnMoveSortFieldDown.Name = "btnMoveSortFieldDown";
			this.btnMoveSortFieldDown.Size = new System.Drawing.Size(24, 25);
			this.btnMoveSortFieldDown.TabIndex = 3;
			this.btnMoveSortFieldDown.UseVisualStyleBackColor = true;
			this.btnMoveSortFieldDown.Click += new System.EventHandler(this.HandleButtonMoveSortFieldDownClick);
			// 
			// chkSaveManual
			// 
			this.chkSaveManual.AutoSize = true;
			this.chkSaveManual.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.chkSaveManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkSaveManual, null);
			this.locExtender.SetLocalizationComment(this.chkSaveManual, "Check box text on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.chkSaveManual, "OptionsDlg.SortingTab.chkSaveManual");
			this.chkSaveManual.Location = new System.Drawing.Point(18, 321);
			this.chkSaveManual.Name = "chkSaveManual";
			this.chkSaveManual.Size = new System.Drawing.Size(325, 19);
			this.chkSaveManual.TabIndex = 5;
			this.chkSaveManual.Text = "&Save last manually specified sort options as the default.";
			this.chkSaveManual.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSaveManual.UseVisualStyleBackColor = true;
			this.chkSaveManual.Click += new System.EventHandler(this.HandleCheckSaveManualClick);
			// 
			// lblSortInfo
			// 
			this.lblSortInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSortInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblSortInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSortInfo, null);
			this.locExtender.SetLocalizationComment(this.lblSortInfo, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSortInfo, "OptionsDlg.SortingTab.lblSortInfo");
			this.lblSortInfo.Location = new System.Drawing.Point(15, 13);
			this.lblSortInfo.Name = "lblSortInfo";
			this.lblSortInfo.Size = new System.Drawing.Size(502, 33);
			this.lblSortInfo.TabIndex = 0;
			this.lblSortInfo.Text = "To specify the default sort options for a specific type of list, choose one from " +
				"the list and select the desired options.";
			// 
			// grpPhoneticSortOptions
			// 
			this.grpPhoneticSortOptions.Controls.Add(this.phoneticSortOptions);
			this.locExtender.SetLocalizableToolTip(this.grpPhoneticSortOptions, null);
			this.locExtender.SetLocalizationComment(this.grpPhoneticSortOptions, "Frame text on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.grpPhoneticSortOptions, "OptionsDlg.SortingTab.grpPhoneticSortOptions");
			this.grpPhoneticSortOptions.Location = new System.Drawing.Point(294, 96);
			this.grpPhoneticSortOptions.Margin = new System.Windows.Forms.Padding(0);
			this.grpPhoneticSortOptions.Name = "grpPhoneticSortOptions";
			this.grpPhoneticSortOptions.Padding = new System.Windows.Forms.Padding(0);
			this.grpPhoneticSortOptions.Size = new System.Drawing.Size(227, 210);
			this.grpPhoneticSortOptions.TabIndex = 4;
			this.grpPhoneticSortOptions.TabStop = false;
			this.grpPhoneticSortOptions.Text = "Phonetic Sort Options";
			// 
			// phoneticSortOptions
			// 
			this.phoneticSortOptions.AdvancedOptionsEnabled = true;
			this.phoneticSortOptions.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.phoneticSortOptions.AutoScroll = true;
			this.phoneticSortOptions.BackColor = System.Drawing.Color.Transparent;
			this.phoneticSortOptions.DrawWithGradientBackground = false;
			this.locExtender.SetLocalizableToolTip(this.phoneticSortOptions, null);
			this.locExtender.SetLocalizationComment(this.phoneticSortOptions, null);
			this.locExtender.SetLocalizationPriority(this.phoneticSortOptions, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.phoneticSortOptions, "OptionsDlg.SortOptionsDropDown");
			this.phoneticSortOptions.Location = new System.Drawing.Point(9, 17);
			this.phoneticSortOptions.MakePhoneticPrimarySortFieldWhenOptionsChange = true;
			this.phoneticSortOptions.Margin = new System.Windows.Forms.Padding(2);
			this.phoneticSortOptions.Name = "phoneticSortOptions";
			this.phoneticSortOptions.ShowAdvancedOptions = true;
			this.phoneticSortOptions.ShowButtons = false;
			this.phoneticSortOptions.Size = new System.Drawing.Size(205, 191);
			this.phoneticSortOptions.TabIndex = 0;
			this.phoneticSortOptions.SortOptionsChanged += new SIL.Pa.UI.Controls.SortOptionsDropDown.SortOptionsChangedHandler(this.HandlePhoneticSortOptionsChanged);
			// 
			// lblListType
			// 
			this.lblListType.AutoSize = true;
			this.lblListType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblListType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblListType, null);
			this.locExtender.SetLocalizationComment(this.lblListType, "Label on sorting tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblListType, "OptionsDlg.SortingTab.lblListType");
			this.lblListType.Location = new System.Drawing.Point(15, 58);
			this.lblListType.Name = "lblListType";
			this.lblListType.Size = new System.Drawing.Size(90, 15);
			this.lblListType.TabIndex = 1;
			this.lblListType.Text = "&Word List Type:";
			// 
			// cboListType
			// 
			this.cboListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboListType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.cboListType.FormattingEnabled = true;
			this.cboListType.Items.AddRange(new object[] {
            "Data Corpus",
            "Search Results",
            "Distribution Chart Search Results"});
			this.locExtender.SetLocalizableToolTip(this.cboListType, null);
			this.locExtender.SetLocalizationComment(this.cboListType, null);
			this.locExtender.SetLocalizingId(this.cboListType, "OptionsDlg.SortingTab.cboListType");
			this.cboListType.Location = new System.Drawing.Point(151, 54);
			this.cboListType.Name = "cboListType";
			this.cboListType.Size = new System.Drawing.Size(216, 23);
			this.cboListType.TabIndex = 2;
			this.cboListType.SelectedIndexChanged += new System.EventHandler(this.HandleListTypeComboSelectedIndexChanged);
			// 
			// tpgFonts
			// 
			this.tpgFonts.Controls.Add(this.pnlFonts);
			this.locExtender.SetLocalizableToolTip(this.tpgFonts, null);
			this.locExtender.SetLocalizationComment(this.tpgFonts, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgFonts, "OptionsDlg.FontsTab.Text");
			this.tpgFonts.Location = new System.Drawing.Point(4, 22);
			this.tpgFonts.Name = "tpgFonts";
			this.tpgFonts.Padding = new System.Windows.Forms.Padding(10);
			this.tpgFonts.Size = new System.Drawing.Size(532, 382);
			this.tpgFonts.TabIndex = 1;
			this.tpgFonts.Text = "Fonts";
			this.tpgFonts.UseVisualStyleBackColor = true;
			// 
			// pnlFonts
			// 
			this.pnlFonts.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlFonts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlFonts.ClipTextForChildControls = true;
			this.pnlFonts.ControlReceivingFocusOnMnemonic = null;
			this.pnlFonts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlFonts.DoubleBuffered = true;
			this.pnlFonts.DrawOnlyBottomBorder = false;
			this.pnlFonts.DrawOnlyTopBorder = false;
			this.pnlFonts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlFonts.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this.pnlFonts, null);
			this.locExtender.SetLocalizationComment(this.pnlFonts, null);
			this.locExtender.SetLocalizationPriority(this.pnlFonts, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlFonts, "OptionsDlg.FontsTab.pnlFonts");
			this.pnlFonts.Location = new System.Drawing.Point(10, 10);
			this.pnlFonts.MnemonicGeneratesClick = false;
			this.pnlFonts.Name = "pnlFonts";
			this.pnlFonts.PaintExplorerBarBackground = false;
			this.pnlFonts.Size = new System.Drawing.Size(512, 362);
			this.pnlFonts.TabIndex = 0;
			// 
			// tpgColors
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgColors, null);
			this.locExtender.SetLocalizationComment(this.tpgColors, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizationPriority(this.tpgColors, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tpgColors, "OptionsDlg.tpgColors");
			this.tpgColors.Location = new System.Drawing.Point(4, 22);
			this.tpgColors.Name = "tpgColors";
			this.tpgColors.Size = new System.Drawing.Size(532, 382);
			this.tpgColors.TabIndex = 3;
			this.tpgColors.Text = "Colors";
			this.tpgColors.UseVisualStyleBackColor = true;
			// 
			// tpgUI
			// 
			this.tpgUI.Controls.Add(this.cboUILanguage);
			this.tpgUI.Controls.Add(this.lblUILanguage);
			this.locExtender.SetLocalizableToolTip(this.tpgUI, null);
			this.locExtender.SetLocalizationComment(this.tpgUI, "Text on tab in options dialog box.");
			this.locExtender.SetLocalizingId(this.tpgUI, "OptionsDlg.UITab.Text");
			this.tpgUI.Location = new System.Drawing.Point(4, 22);
			this.tpgUI.Name = "tpgUI";
			this.tpgUI.Padding = new System.Windows.Forms.Padding(3);
			this.tpgUI.Size = new System.Drawing.Size(532, 382);
			this.tpgUI.TabIndex = 8;
			this.tpgUI.Text = "User Interface";
			this.tpgUI.UseVisualStyleBackColor = true;
			// 
			// cboUILanguage
			// 
			this.cboUILanguage.DropDownHeight = 200;
			this.cboUILanguage.FormattingEnabled = true;
			this.cboUILanguage.IntegralHeight = false;
			this.locExtender.SetLocalizableToolTip(this.cboUILanguage, null);
			this.locExtender.SetLocalizationComment(this.cboUILanguage, null);
			this.locExtender.SetLocalizationPriority(this.cboUILanguage, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboUILanguage, "OptionsDlg.UITab.cboUILanguage");
			this.cboUILanguage.Location = new System.Drawing.Point(31, 56);
			this.cboUILanguage.Name = "cboUILanguage";
			this.cboUILanguage.Size = new System.Drawing.Size(208, 21);
			this.cboUILanguage.TabIndex = 1;
			// 
			// lblUILanguage
			// 
			this.lblUILanguage.AutoSize = true;
			this.lblUILanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.lblUILanguage, null);
			this.locExtender.SetLocalizationComment(this.lblUILanguage, "Label on user interface tab of options dialog box.");
			this.locExtender.SetLocalizingId(this.lblUILanguage, "OptionsDlg.UITab.lblUILanguage");
			this.lblUILanguage.Location = new System.Drawing.Point(32, 34);
			this.lblUILanguage.Name = "lblUILanguage";
			this.lblUILanguage.Size = new System.Drawing.Size(145, 15);
			this.lblUILanguage.TabIndex = 0;
			this.lblUILanguage.Text = "User Interface Language:";
			// 
			// picSaveInfo
			// 
			this.picSaveInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.picSaveInfo.Image = global::SIL.Pa.Properties.Resources.kimidInformation;
			this.picSaveInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.picSaveInfo, null);
			this.locExtender.SetLocalizationComment(this.picSaveInfo, null);
			this.locExtender.SetLocalizationPriority(this.picSaveInfo, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.picSaveInfo, "OptionsDlg.picSaveInfo");
			this.picSaveInfo.Location = new System.Drawing.Point(12, 7);
			this.picSaveInfo.Name = "picSaveInfo";
			this.picSaveInfo.Size = new System.Drawing.Size(16, 16);
			this.picSaveInfo.TabIndex = 10;
			this.picSaveInfo.TabStop = false;
			this.picSaveInfo.Visible = false;
			// 
			// lblSaveInfo
			// 
			this.lblSaveInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSaveInfo.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.lblSaveInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSaveInfo, null);
			this.locExtender.SetLocalizationComment(this.lblSaveInfo, "Label next to OK/Cancel/Help buttons on options dialog box.");
			this.locExtender.SetLocalizingId(this.lblSaveInfo, "OptionsDlg.lblSaveInfo");
			this.lblSaveInfo.Location = new System.Drawing.Point(34, 5);
			this.lblSaveInfo.Name = "lblSaveInfo";
			this.lblSaveInfo.Size = new System.Drawing.Size(245, 33);
			this.lblSaveInfo.TabIndex = 0;
			this.lblSaveInfo.Text = "The options for this tab will be saved only for the current project.";
			this.lblSaveInfo.Visible = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = null;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// OptionsDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(563, 463);
			this.Controls.Add(this.tabOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Title of options dialog box.");
			this.locExtender.SetLocalizingId(this, "OptionsDlg.WindowTitle");
			this.Name = "OptionsDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Options";
			this.Controls.SetChildIndex(this.tabOptions, 0);
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
			this.tpgFonts.ResumeLayout(false);
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
		private SilTools.SilGrid m_sortingGrid;
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
		private Localization.UI.LocalizationExtender locExtender;
		private CheckBox chkStress;
		private SilTools.Controls.SilPanel pnlFonts;
	}
}
