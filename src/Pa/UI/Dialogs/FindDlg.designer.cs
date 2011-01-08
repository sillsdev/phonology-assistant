using System.Windows.Forms;
using SilUtils.Controls;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblFindWhat = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkMatchCase = new System.Windows.Forms.CheckBox();
			this.btnFind = new System.Windows.Forms.Button();
			this.cboFindWhat = new System.Windows.Forms.ComboBox();
			this.chkMatchEntireWord = new System.Windows.Forms.CheckBox();
			this.chkRegEx = new System.Windows.Forms.CheckBox();
			this.chkStartsWith = new System.Windows.Forms.CheckBox();
			this.gbOptions = new System.Windows.Forms.GroupBox();
			this.chkSrchCollapsedGrps = new System.Windows.Forms.CheckBox();
			this.chkReverseSearch = new System.Windows.Forms.CheckBox();
			this.lblSearchColumns = new System.Windows.Forms.Label();
			this.btnHelp = new System.Windows.Forms.Button();
			this.tblLayoutFindWhat = new System.Windows.Forms.TableLayoutPanel();
			this.fldSelGridSrchCols = new SIL.Pa.UI.Controls.FieldSelectorGrid();
			this.tblLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.splitColsAndOptions = new System.Windows.Forms.SplitContainer();
			this.tblLayoutColumns = new System.Windows.Forms.TableLayoutPanel();
			this.gbOptions.SuspendLayout();
			this.tblLayoutFindWhat.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridSrchCols)).BeginInit();
			this.tblLayoutButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.splitColsAndOptions.Panel1.SuspendLayout();
			this.splitColsAndOptions.Panel2.SuspendLayout();
			this.splitColsAndOptions.SuspendLayout();
			this.tblLayoutColumns.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblFindWhat
			// 
			resources.ApplyResources(this.lblFindWhat, "lblFindWhat");
			this.locExtender.SetLocalizableToolTip(this.lblFindWhat, null);
			this.locExtender.SetLocalizationComment(this.lblFindWhat, "Label above drop-down box of items to find in find dialog box.");
			this.locExtender.SetLocalizingId(this.lblFindWhat, "FindDlg.lblFindWhat");
			this.lblFindWhat.Name = "lblFindWhat";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, "Text on button on find dialog box.");
			this.locExtender.SetLocalizingId(this.btnCancel, "FindDlg.btnCancel");
			this.btnCancel.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkMatchCase
			// 
			resources.ApplyResources(this.chkMatchCase, "chkMatchCase");
			this.chkMatchCase.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.chkMatchCase, null);
			this.locExtender.SetLocalizationComment(this.chkMatchCase, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkMatchCase, "FindDlg.chkMatchCase");
			this.chkMatchCase.Name = "chkMatchCase";
			this.chkMatchCase.UseVisualStyleBackColor = true;
			// 
			// btnFind
			// 
			this.btnFind.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			resources.ApplyResources(this.btnFind, "btnFind");
			this.btnFind.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this.btnFind, null);
			this.locExtender.SetLocalizationComment(this.btnFind, "Text on button on find dialog box.");
			this.locExtender.SetLocalizationPriority(this.btnFind, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnFind, "FindDlg.btnFind");
			this.btnFind.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnFind.Name = "btnFind";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// cboFindWhat
			// 
			resources.ApplyResources(this.cboFindWhat, "cboFindWhat");
			this.locExtender.SetLocalizableToolTip(this.cboFindWhat, null);
			this.locExtender.SetLocalizationComment(this.cboFindWhat, null);
			this.locExtender.SetLocalizationPriority(this.cboFindWhat, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboFindWhat, "FindDlg.cboFindWhat");
			this.cboFindWhat.Name = "cboFindWhat";
			// 
			// chkMatchEntireWord
			// 
			resources.ApplyResources(this.chkMatchEntireWord, "chkMatchEntireWord");
			this.chkMatchEntireWord.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.chkMatchEntireWord, null);
			this.locExtender.SetLocalizationComment(this.chkMatchEntireWord, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkMatchEntireWord, "FindDlg.chkMatchEntireWord");
			this.chkMatchEntireWord.Name = "chkMatchEntireWord";
			this.chkMatchEntireWord.UseVisualStyleBackColor = true;
			this.chkMatchEntireWord.CheckedChanged += new System.EventHandler(this.chkMatchEntireWord_CheckedChanged);
			// 
			// chkRegEx
			// 
			resources.ApplyResources(this.chkRegEx, "chkRegEx");
			this.chkRegEx.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.chkRegEx, null);
			this.locExtender.SetLocalizationComment(this.chkRegEx, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkRegEx, "FindDlg.chkRegEx");
			this.chkRegEx.Name = "chkRegEx";
			this.chkRegEx.UseVisualStyleBackColor = true;
			this.chkRegEx.CheckedChanged += new System.EventHandler(this.cbRegEx_CheckedChanged);
			// 
			// chkStartsWith
			// 
			resources.ApplyResources(this.chkStartsWith, "chkStartsWith");
			this.chkStartsWith.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.chkStartsWith, null);
			this.locExtender.SetLocalizationComment(this.chkStartsWith, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkStartsWith, "FindDlg.chkStartsWith");
			this.chkStartsWith.Name = "chkStartsWith";
			this.chkStartsWith.UseVisualStyleBackColor = true;
			this.chkStartsWith.CheckedChanged += new System.EventHandler(this.chkStartsWith_CheckedChanged);
			// 
			// gbOptions
			// 
			this.gbOptions.Controls.Add(this.chkMatchCase);
			this.gbOptions.Controls.Add(this.chkStartsWith);
			this.gbOptions.Controls.Add(this.chkRegEx);
			this.gbOptions.Controls.Add(this.chkMatchEntireWord);
			this.gbOptions.Controls.Add(this.chkSrchCollapsedGrps);
			this.gbOptions.Controls.Add(this.chkReverseSearch);
			resources.ApplyResources(this.gbOptions, "gbOptions");
			this.locExtender.SetLocalizableToolTip(this.gbOptions, null);
			this.locExtender.SetLocalizationComment(this.gbOptions, "Text in frame around check boxes in find dialog box.");
			this.locExtender.SetLocalizingId(this.gbOptions, "FindDlg.gbOptions");
			this.gbOptions.Name = "gbOptions";
			this.gbOptions.TabStop = false;
			// 
			// chkSrchCollapsedGrps
			// 
			resources.ApplyResources(this.chkSrchCollapsedGrps, "chkSrchCollapsedGrps");
			this.chkSrchCollapsedGrps.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.chkSrchCollapsedGrps, null);
			this.locExtender.SetLocalizationComment(this.chkSrchCollapsedGrps, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkSrchCollapsedGrps, "FindDlg.chkSrchCollapsedGrps");
			this.chkSrchCollapsedGrps.Name = "chkSrchCollapsedGrps";
			this.chkSrchCollapsedGrps.UseVisualStyleBackColor = true;
			// 
			// chkReverseSearch
			// 
			resources.ApplyResources(this.chkReverseSearch, "chkReverseSearch");
			this.chkReverseSearch.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.chkReverseSearch, null);
			this.locExtender.SetLocalizationComment(this.chkReverseSearch, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkReverseSearch, "FindDlg.chkReverseSearch");
			this.chkReverseSearch.Name = "chkReverseSearch";
			this.chkReverseSearch.UseVisualStyleBackColor = true;
			// 
			// lblSearchColumns
			// 
			resources.ApplyResources(this.lblSearchColumns, "lblSearchColumns");
			this.lblSearchColumns.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblSearchColumns, null);
			this.locExtender.SetLocalizationComment(this.lblSearchColumns, "Label above column list in find dialog box.");
			this.locExtender.SetLocalizingId(this.lblSearchColumns, "FindDlg.lblSearchColumns");
			this.lblSearchColumns.Name = "lblSearchColumns";
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Text on button on find dialog box.");
			this.locExtender.SetLocalizingId(this.btnHelp, "FindDlg.btnHelp");
			this.btnHelp.MinimumSize = new System.Drawing.Size(80, 26);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// tblLayoutFindWhat
			// 
			resources.ApplyResources(this.tblLayoutFindWhat, "tblLayoutFindWhat");
			this.tblLayoutFindWhat.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutFindWhat.Controls.Add(this.cboFindWhat, 0, 1);
			this.tblLayoutFindWhat.Controls.Add(this.lblFindWhat, 0, 0);
			this.tblLayoutFindWhat.Name = "tblLayoutFindWhat";
			// 
			// fldSelGridSrchCols
			// 
			this.fldSelGridSrchCols.AllowUserToAddRows = false;
			this.fldSelGridSrchCols.AllowUserToDeleteRows = false;
			this.fldSelGridSrchCols.AllowUserToResizeColumns = false;
			this.fldSelGridSrchCols.AllowUserToResizeRows = false;
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
			resources.ApplyResources(this.fldSelGridSrchCols, "fldSelGridSrchCols");
			this.fldSelGridSrchCols.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridSrchCols.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridSrchCols, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridSrchCols, null);
			this.locExtender.SetLocalizingId(this.fldSelGridSrchCols, "FindDlg.fldSelGridSrchCols");
			this.fldSelGridSrchCols.MultiSelect = false;
			this.fldSelGridSrchCols.Name = "fldSelGridSrchCols";
			this.fldSelGridSrchCols.PaintHeaderAcrossFullGridWidth = true;
			this.fldSelGridSrchCols.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridSrchCols.RowHeadersVisible = false;
			this.fldSelGridSrchCols.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridSrchCols.ShowWaterMarkWhenDirty = false;
			this.fldSelGridSrchCols.WaterMark = "!";
			this.fldSelGridSrchCols.AfterUserChangedValue += new SIL.Pa.UI.Controls.FieldSelectorGrid.AfterUserChangedValueHandler(this.fldSelGridSrchCols_AfterUserChangedValue);
			// 
			// tblLayoutButtons
			// 
			resources.ApplyResources(this.tblLayoutButtons, "tblLayoutButtons");
			this.tblLayoutButtons.BackColor = System.Drawing.Color.Transparent;
			this.tblLayoutButtons.Controls.Add(this.btnHelp, 3, 0);
			this.tblLayoutButtons.Controls.Add(this.btnCancel, 2, 0);
			this.tblLayoutButtons.Controls.Add(this.btnFind, 1, 0);
			this.tblLayoutButtons.Name = "tblLayoutButtons";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// splitColsAndOptions
			// 
			resources.ApplyResources(this.splitColsAndOptions, "splitColsAndOptions");
			this.splitColsAndOptions.Name = "splitColsAndOptions";
			// 
			// splitColsAndOptions.Panel1
			// 
			this.splitColsAndOptions.Panel1.Controls.Add(this.tblLayoutColumns);
			// 
			// splitColsAndOptions.Panel2
			// 
			this.splitColsAndOptions.Panel2.Controls.Add(this.gbOptions);
			resources.ApplyResources(this.splitColsAndOptions.Panel2, "splitColsAndOptions.Panel2");
			this.splitColsAndOptions.TabStop = false;
			// 
			// tblLayoutColumns
			// 
			resources.ApplyResources(this.tblLayoutColumns, "tblLayoutColumns");
			this.tblLayoutColumns.Controls.Add(this.lblSearchColumns, 0, 0);
			this.tblLayoutColumns.Controls.Add(this.fldSelGridSrchCols, 0, 1);
			this.tblLayoutColumns.Name = "tblLayoutColumns";
			// 
			// FindDlg
			// 
			this.AcceptButton = this.btnFind;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.splitColsAndOptions);
			this.Controls.Add(this.tblLayoutFindWhat);
			this.Controls.Add(this.tblLayoutButtons);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FindDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.TopMost = true;
			this.gbOptions.ResumeLayout(false);
			this.tblLayoutFindWhat.ResumeLayout(false);
			this.tblLayoutFindWhat.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridSrchCols)).EndInit();
			this.tblLayoutButtons.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.splitColsAndOptions.Panel1.ResumeLayout(false);
			this.splitColsAndOptions.Panel2.ResumeLayout(false);
			this.splitColsAndOptions.ResumeLayout(false);
			this.tblLayoutColumns.ResumeLayout(false);
			this.tblLayoutColumns.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFindWhat;
		private System.Windows.Forms.Button btnCancel;
		private CheckBox chkMatchCase;
		private CheckBox chkSrchCollapsedGrps;
		private CheckBox chkMatchEntireWord;
		private CheckBox chkRegEx;
		private CheckBox chkStartsWith;
		private CheckBox chkReverseSearch;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.ComboBox cboFindWhat;
		private System.Windows.Forms.GroupBox gbOptions;
		private System.Windows.Forms.Label lblSearchColumns;
		private SIL.Pa.UI.Controls.FieldSelectorGrid fldSelGridSrchCols;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.TableLayoutPanel tblLayoutFindWhat;
		private System.Windows.Forms.TableLayoutPanel tblLayoutButtons;
		private Localization.UI.LocalizationExtender locExtender;
		private SplitContainer splitColsAndOptions;
		private TableLayoutPanel tblLayoutColumns;
	}
}