using System.Windows.Forms;
using SilTools.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class FindDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.fldSelGridSrchCols = new SIL.Pa.UI.Controls.FieldSelectorGrid();
			this.gbOptions = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkReverseSearch = new SilTools.Controls.AutoHeightCheckBox();
			this.chkMatchEntireWord = new SilTools.Controls.AutoHeightCheckBox();
			this.chkRegEx = new SilTools.Controls.AutoHeightCheckBox();
			this.chkSrchCollapsedGrps = new SilTools.Controls.AutoHeightCheckBox();
			this.chkStartsWith = new SilTools.Controls.AutoHeightCheckBox();
			this.chkMatchCase = new SilTools.Controls.AutoHeightCheckBox();
			this.lblFindWhat = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnFind = new System.Windows.Forms.Button();
			this.cboFindWhat = new System.Windows.Forms.ComboBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.tblLayoutFindWhat = new System.Windows.Forms.TableLayoutPanel();
			this.lblSearchColumns = new System.Windows.Forms.Label();
			this.tblLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridSrchCols)).BeginInit();
			this.gbOptions.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tblLayoutFindWhat.SuspendLayout();
			this.tblLayoutButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// fldSelGridSrchCols
			// 
			this.fldSelGridSrchCols.AllowUserToAddRows = false;
			this.fldSelGridSrchCols.AllowUserToDeleteRows = false;
			this.fldSelGridSrchCols.AllowUserToResizeColumns = false;
			this.fldSelGridSrchCols.AllowUserToResizeRows = false;
			this.fldSelGridSrchCols.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.fldSelGridSrchCols.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.fldSelGridSrchCols.BackgroundColor = System.Drawing.SystemColors.Window;
			this.fldSelGridSrchCols.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.fldSelGridSrchCols.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fldSelGridSrchCols.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.fldSelGridSrchCols.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.fldSelGridSrchCols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.fldSelGridSrchCols.ColumnHeadersVisible = false;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.fldSelGridSrchCols.DefaultCellStyle = dataGridViewCellStyle2;
			this.fldSelGridSrchCols.DrawTextBoxEditControlBorder = false;
			this.fldSelGridSrchCols.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.fldSelGridSrchCols.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.fldSelGridSrchCols.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridSrchCols.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridSrchCols, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridSrchCols, null);
			this.locExtender.SetLocalizingId(this.fldSelGridSrchCols, "FindDlg.fldSelGridSrchCols");
			this.fldSelGridSrchCols.Location = new System.Drawing.Point(0, 76);
			this.fldSelGridSrchCols.Margin = new System.Windows.Forms.Padding(0, 5, 0, 2);
			this.fldSelGridSrchCols.MultiSelect = false;
			this.fldSelGridSrchCols.Name = "fldSelGridSrchCols";
			this.fldSelGridSrchCols.PaintHeaderAcrossFullGridWidth = true;
			this.fldSelGridSrchCols.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridSrchCols.RowHeadersVisible = false;
			this.fldSelGridSrchCols.RowHeadersWidth = 22;
			this.fldSelGridSrchCols.SelectedCellBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridSrchCols.SelectedCellForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridSrchCols.SelectedRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(213)))), ((int)(((byte)(255)))));
			this.fldSelGridSrchCols.SelectedRowForeColor = System.Drawing.SystemColors.WindowText;
			this.fldSelGridSrchCols.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridSrchCols.ShowWaterMarkWhenDirty = false;
			this.fldSelGridSrchCols.Size = new System.Drawing.Size(191, 182);
			this.fldSelGridSrchCols.TabIndex = 1;
			this.fldSelGridSrchCols.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.fldSelGridSrchCols.WaterMark = "!";
			this.fldSelGridSrchCols.AfterUserChangedValue += new SIL.Pa.UI.Controls.FieldSelectorGrid.AfterUserChangedValueHandler(this.fldSelGridSrchCols_AfterUserChangedValue);
			// 
			// gbOptions
			// 
			this.gbOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbOptions.Controls.Add(this.panel1);
			this.gbOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.gbOptions, null);
			this.locExtender.SetLocalizationComment(this.gbOptions, "Text in frame around check boxes in find dialog box.");
			this.locExtender.SetLocalizingId(this.gbOptions, "FindDlg.gbOptions");
			this.gbOptions.Location = new System.Drawing.Point(199, 54);
			this.gbOptions.Margin = new System.Windows.Forms.Padding(8, 3, 0, 0);
			this.gbOptions.Name = "gbOptions";
			this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 0, 3, 5);
			this.tblLayoutFindWhat.SetRowSpan(this.gbOptions, 2);
			this.gbOptions.Size = new System.Drawing.Size(246, 206);
			this.gbOptions.TabIndex = 2;
			this.gbOptions.TabStop = false;
			this.gbOptions.Text = "Options";
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(10, 14);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(233, 187);
			this.panel1.TabIndex = 7;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.chkReverseSearch, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.chkMatchEntireWord, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkRegEx, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.chkSrchCollapsedGrps, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.chkStartsWith, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.chkMatchCase, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(233, 174);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// chkReverseSearch
			// 
			this.chkReverseSearch.AutoSize = true;
			this.chkReverseSearch.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkReverseSearch.Dock = System.Windows.Forms.DockStyle.Top;
			this.chkReverseSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkReverseSearch, null);
			this.locExtender.SetLocalizationComment(this.chkReverseSearch, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkReverseSearch, "FindDlg.chkReverseSearch");
			this.chkReverseSearch.Location = new System.Drawing.Point(10, 97);
			this.chkReverseSearch.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
			this.chkReverseSearch.Name = "chkReverseSearch";
			this.chkReverseSearch.Size = new System.Drawing.Size(220, 19);
			this.chkReverseSearch.TabIndex = 3;
			this.chkReverseSearch.Text = "&Reverse Search";
			this.chkReverseSearch.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkReverseSearch.UseVisualStyleBackColor = true;
			// 
			// chkMatchEntireWord
			// 
			this.chkMatchEntireWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkMatchEntireWord.AutoSize = true;
			this.chkMatchEntireWord.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkMatchEntireWord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkMatchEntireWord, null);
			this.locExtender.SetLocalizationComment(this.chkMatchEntireWord, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkMatchEntireWord, "FindDlg.chkMatchEntireWord");
			this.chkMatchEntireWord.Location = new System.Drawing.Point(10, 68);
			this.chkMatchEntireWord.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
			this.chkMatchEntireWord.Name = "chkMatchEntireWord";
			this.chkMatchEntireWord.Size = new System.Drawing.Size(220, 19);
			this.chkMatchEntireWord.TabIndex = 2;
			this.chkMatchEntireWord.Text = "Find whole words onl&y";
			this.chkMatchEntireWord.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkMatchEntireWord.UseVisualStyleBackColor = false;
			this.chkMatchEntireWord.CheckedChanged += new System.EventHandler(this.chkMatchEntireWord_CheckedChanged);
			// 
			// chkRegEx
			// 
			this.chkRegEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.chkRegEx.AutoSize = true;
			this.chkRegEx.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkRegEx.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkRegEx, null);
			this.locExtender.SetLocalizationComment(this.chkRegEx, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkRegEx, "FindDlg.chkRegEx");
			this.chkRegEx.Location = new System.Drawing.Point(10, 155);
			this.chkRegEx.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
			this.chkRegEx.Name = "chkRegEx";
			this.chkRegEx.Size = new System.Drawing.Size(220, 19);
			this.chkRegEx.TabIndex = 5;
			this.chkRegEx.Text = "&Use Regular Expressions";
			this.chkRegEx.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkRegEx.UseVisualStyleBackColor = true;
			this.chkRegEx.CheckedChanged += new System.EventHandler(this.cbRegEx_CheckedChanged);
			// 
			// chkSrchCollapsedGrps
			// 
			this.chkSrchCollapsedGrps.AutoSize = true;
			this.chkSrchCollapsedGrps.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSrchCollapsedGrps.Dock = System.Windows.Forms.DockStyle.Top;
			this.chkSrchCollapsedGrps.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkSrchCollapsedGrps, null);
			this.locExtender.SetLocalizationComment(this.chkSrchCollapsedGrps, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkSrchCollapsedGrps, "FindDlg.chkSrchCollapsedGrps");
			this.chkSrchCollapsedGrps.Location = new System.Drawing.Point(10, 126);
			this.chkSrchCollapsedGrps.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
			this.chkSrchCollapsedGrps.Name = "chkSrchCollapsedGrps";
			this.chkSrchCollapsedGrps.Size = new System.Drawing.Size(220, 19);
			this.chkSrchCollapsedGrps.TabIndex = 4;
			this.chkSrchCollapsedGrps.Text = "Search &Collapsed Groups";
			this.chkSrchCollapsedGrps.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkSrchCollapsedGrps.UseVisualStyleBackColor = true;
			// 
			// chkStartsWith
			// 
			this.chkStartsWith.AutoSize = true;
			this.chkStartsWith.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkStartsWith.Dock = System.Windows.Forms.DockStyle.Top;
			this.chkStartsWith.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkStartsWith, null);
			this.locExtender.SetLocalizationComment(this.chkStartsWith, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkStartsWith, "FindDlg.chkStartsWith");
			this.chkStartsWith.Location = new System.Drawing.Point(10, 39);
			this.chkStartsWith.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
			this.chkStartsWith.Name = "chkStartsWith";
			this.chkStartsWith.Size = new System.Drawing.Size(220, 19);
			this.chkStartsWith.TabIndex = 1;
			this.chkStartsWith.Text = "&Starts With";
			this.chkStartsWith.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkStartsWith.UseVisualStyleBackColor = true;
			this.chkStartsWith.CheckedChanged += new System.EventHandler(this.chkStartsWith_CheckedChanged);
			// 
			// chkMatchCase
			// 
			this.chkMatchCase.AutoSize = true;
			this.chkMatchCase.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkMatchCase.Dock = System.Windows.Forms.DockStyle.Top;
			this.chkMatchCase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.chkMatchCase, null);
			this.locExtender.SetLocalizationComment(this.chkMatchCase, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkMatchCase, "FindDlg.chkMatchCase");
			this.chkMatchCase.Location = new System.Drawing.Point(10, 10);
			this.chkMatchCase.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
			this.chkMatchCase.Name = "chkMatchCase";
			this.chkMatchCase.Size = new System.Drawing.Size(220, 19);
			this.chkMatchCase.TabIndex = 0;
			this.chkMatchCase.Text = "Matc&h Case";
			this.chkMatchCase.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.chkMatchCase.UseVisualStyleBackColor = true;
			// 
			// lblFindWhat
			// 
			this.lblFindWhat.AutoSize = true;
			this.tblLayoutFindWhat.SetColumnSpan(this.lblFindWhat, 2);
			this.lblFindWhat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblFindWhat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblFindWhat, null);
			this.locExtender.SetLocalizationComment(this.lblFindWhat, "Label above drop-down box of items to find in find dialog box.");
			this.locExtender.SetLocalizingId(this.lblFindWhat, "FindDlg.lblFindWhat");
			this.lblFindWhat.Location = new System.Drawing.Point(3, 7);
			this.lblFindWhat.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
			this.lblFindWhat.Name = "lblFindWhat";
			this.lblFindWhat.Size = new System.Drawing.Size(65, 15);
			this.lblFindWhat.TabIndex = 1;
			this.lblFindWhat.Text = "Fi&nd What:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, "Text on button on find dialog box.");
			this.locExtender.SetLocalizingId(this.btnCancel, "FindDlg.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(280, 7);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(5, 7, 0, 7);
			this.btnCancel.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 26);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnFind
			// 
			this.btnFind.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFind.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFind.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnFind, null);
			this.locExtender.SetLocalizationComment(this.btnFind, "Text on button on find dialog box.");
			this.locExtender.SetLocalizationPriority(this.btnFind, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnFind, "FindDlg.btnFind");
			this.btnFind.Location = new System.Drawing.Point(195, 7);
			this.btnFind.Margin = new System.Windows.Forms.Padding(5, 7, 0, 7);
			this.btnFind.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(80, 26);
			this.btnFind.TabIndex = 0;
			this.btnFind.Text = "Find";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// cboFindWhat
			// 
			this.cboFindWhat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tblLayoutFindWhat.SetColumnSpan(this.cboFindWhat, 2);
			this.locExtender.SetLocalizableToolTip(this.cboFindWhat, null);
			this.locExtender.SetLocalizationComment(this.cboFindWhat, null);
			this.locExtender.SetLocalizationPriority(this.cboFindWhat, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboFindWhat, "FindDlg.cboFindWhat");
			this.cboFindWhat.Location = new System.Drawing.Point(0, 27);
			this.cboFindWhat.Margin = new System.Windows.Forms.Padding(0, 5, 0, 3);
			this.cboFindWhat.MaxDropDownItems = 5;
			this.cboFindWhat.Name = "cboFindWhat";
			this.cboFindWhat.Size = new System.Drawing.Size(445, 21);
			this.cboFindWhat.TabIndex = 0;
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Text on button on find dialog box.");
			this.locExtender.SetLocalizingId(this.btnHelp, "FindDlg.btnHelp");
			this.btnHelp.Location = new System.Drawing.Point(365, 7);
			this.btnHelp.Margin = new System.Windows.Forms.Padding(5, 7, 0, 7);
			this.btnHelp.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(80, 26);
			this.btnHelp.TabIndex = 2;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// tblLayoutFindWhat
			// 
			this.tblLayoutFindWhat.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutFindWhat.ColumnCount = 2;
			this.tblLayoutFindWhat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
			this.tblLayoutFindWhat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
			this.tblLayoutFindWhat.Controls.Add(this.gbOptions, 1, 2);
			this.tblLayoutFindWhat.Controls.Add(this.fldSelGridSrchCols, 0, 3);
			this.tblLayoutFindWhat.Controls.Add(this.lblSearchColumns, 0, 2);
			this.tblLayoutFindWhat.Controls.Add(this.cboFindWhat, 0, 1);
			this.tblLayoutFindWhat.Controls.Add(this.lblFindWhat, 0, 0);
			this.tblLayoutFindWhat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblLayoutFindWhat.Location = new System.Drawing.Point(10, 10);
			this.tblLayoutFindWhat.Name = "tblLayoutFindWhat";
			this.tblLayoutFindWhat.RowCount = 4;
			this.tblLayoutFindWhat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutFindWhat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutFindWhat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutFindWhat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutFindWhat.Size = new System.Drawing.Size(445, 260);
			this.tblLayoutFindWhat.TabIndex = 3;
			// 
			// lblSearchColumns
			// 
			this.lblSearchColumns.AutoSize = true;
			this.lblSearchColumns.BackColor = System.Drawing.Color.Transparent;
			this.lblSearchColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.lblSearchColumns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblSearchColumns, null);
			this.locExtender.SetLocalizationComment(this.lblSearchColumns, "Label above column list in find dialog box.");
			this.locExtender.SetLocalizingId(this.lblSearchColumns, "FindDlg.lblSearchColumns");
			this.lblSearchColumns.Location = new System.Drawing.Point(3, 56);
			this.lblSearchColumns.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.lblSearchColumns.Name = "lblSearchColumns";
			this.lblSearchColumns.Size = new System.Drawing.Size(136, 15);
			this.lblSearchColumns.TabIndex = 0;
			this.lblSearchColumns.Text = "&Find in Which Columns:";
			// 
			// tblLayoutButtons
			// 
			this.tblLayoutButtons.AutoSize = true;
			this.tblLayoutButtons.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutButtons.ColumnCount = 4;
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tblLayoutButtons.Controls.Add(this.btnHelp, 3, 0);
			this.tblLayoutButtons.Controls.Add(this.btnCancel, 2, 0);
			this.tblLayoutButtons.Controls.Add(this.btnFind, 1, 0);
			this.tblLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tblLayoutButtons.Location = new System.Drawing.Point(10, 270);
			this.tblLayoutButtons.Name = "tblLayoutButtons";
			this.tblLayoutButtons.RowCount = 1;
			this.tblLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayoutButtons.Size = new System.Drawing.Size(445, 40);
			this.tblLayoutButtons.TabIndex = 4;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip;
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// FindDlg
			// 
			this.AcceptButton = this.btnFind;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(465, 310);
			this.Controls.Add(this.tblLayoutFindWhat);
			this.Controls.Add(this.tblLayoutButtons);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FindDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(375, 250);
			this.Name = "FindDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Find";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridSrchCols)).EndInit();
			this.gbOptions.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tblLayoutFindWhat.ResumeLayout(false);
			this.tblLayoutFindWhat.PerformLayout();
			this.tblLayoutButtons.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFindWhat;
		private System.Windows.Forms.Button btnCancel;
		private AutoHeightCheckBox chkMatchCase;
		private AutoHeightCheckBox chkSrchCollapsedGrps;
		private AutoHeightCheckBox chkMatchEntireWord;
		private AutoHeightCheckBox chkRegEx;
		private AutoHeightCheckBox chkStartsWith;
		private AutoHeightCheckBox chkReverseSearch;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.ComboBox cboFindWhat;
		private System.Windows.Forms.GroupBox gbOptions;
		private SIL.Pa.UI.Controls.FieldSelectorGrid fldSelGridSrchCols;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.TableLayoutPanel tblLayoutFindWhat;
		private System.Windows.Forms.TableLayoutPanel tblLayoutButtons;
		private Localization.UI.LocalizationExtender locExtender;
		private Panel panel1;
		private TableLayoutPanel tableLayoutPanel1;
		private Label lblSearchColumns;
	}
}