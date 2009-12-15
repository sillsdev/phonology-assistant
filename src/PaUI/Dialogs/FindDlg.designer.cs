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
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnHelp = new System.Windows.Forms.Button();
			this.pnlFindWhat = new System.Windows.Forms.Panel();
			this.pnlColumnOptions = new System.Windows.Forms.Panel();
			this.fldSelGridSrchCols = new SIL.Pa.UI.Controls.FieldSelectorGrid();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.gbOptions.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.pnlFindWhat.SuspendLayout();
			this.pnlColumnOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridSrchCols)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
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
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkMatchCase
			// 
			resources.ApplyResources(this.chkMatchCase, "chkMatchCase");
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
			this.locExtender.SetLocalizationPriority(this.btnFind, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnFind, "FindDlg.btnFind");
			this.btnFind.Name = "btnFind";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// cboFindWhat
			// 
			resources.ApplyResources(this.cboFindWhat, "cboFindWhat");
			this.locExtender.SetLocalizableToolTip(this.cboFindWhat, null);
			this.locExtender.SetLocalizationComment(this.cboFindWhat, null);
			this.locExtender.SetLocalizationPriority(this.cboFindWhat, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.cboFindWhat, "FindDlg.cboFindWhat");
			this.cboFindWhat.Name = "cboFindWhat";
			// 
			// chkMatchEntireWord
			// 
			resources.ApplyResources(this.chkMatchEntireWord, "chkMatchEntireWord");
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
			this.locExtender.SetLocalizableToolTip(this.chkStartsWith, null);
			this.locExtender.SetLocalizationComment(this.chkStartsWith, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkStartsWith, "FindDlg.chkStartsWith");
			this.chkStartsWith.Name = "chkStartsWith";
			this.chkStartsWith.UseVisualStyleBackColor = true;
			this.chkStartsWith.CheckedChanged += new System.EventHandler(this.chkStartsWith_CheckedChanged);
			// 
			// gbOptions
			// 
			resources.ApplyResources(this.gbOptions, "gbOptions");
			this.gbOptions.Controls.Add(this.chkSrchCollapsedGrps);
			this.gbOptions.Controls.Add(this.chkReverseSearch);
			this.gbOptions.Controls.Add(this.chkMatchCase);
			this.gbOptions.Controls.Add(this.chkStartsWith);
			this.gbOptions.Controls.Add(this.chkMatchEntireWord);
			this.gbOptions.Controls.Add(this.chkRegEx);
			this.locExtender.SetLocalizableToolTip(this.gbOptions, null);
			this.locExtender.SetLocalizationComment(this.gbOptions, "Text in frame around check boxes in find dialog box.");
			this.locExtender.SetLocalizingId(this.gbOptions, "FindDlg.gbOptions");
			this.gbOptions.Name = "gbOptions";
			this.gbOptions.TabStop = false;
			// 
			// chkSrchCollapsedGrps
			// 
			resources.ApplyResources(this.chkSrchCollapsedGrps, "chkSrchCollapsedGrps");
			this.locExtender.SetLocalizableToolTip(this.chkSrchCollapsedGrps, null);
			this.locExtender.SetLocalizationComment(this.chkSrchCollapsedGrps, "Check box option on find dialog box.");
			this.locExtender.SetLocalizingId(this.chkSrchCollapsedGrps, "FindDlg.chkSrchCollapsedGrps");
			this.chkSrchCollapsedGrps.Name = "chkSrchCollapsedGrps";
			this.chkSrchCollapsedGrps.UseVisualStyleBackColor = true;
			// 
			// chkReverseSearch
			// 
			resources.ApplyResources(this.chkReverseSearch, "chkReverseSearch");
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
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnFind);
			this.pnlButtons.Controls.Add(this.btnCancel);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Text on button on find dialog box.");
			this.locExtender.SetLocalizingId(this.btnHelp, "FindDlg.btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// pnlFindWhat
			// 
			resources.ApplyResources(this.pnlFindWhat, "pnlFindWhat");
			this.pnlFindWhat.Controls.Add(this.cboFindWhat);
			this.pnlFindWhat.Controls.Add(this.lblFindWhat);
			this.pnlFindWhat.Name = "pnlFindWhat";
			// 
			// pnlColumnOptions
			// 
			this.pnlColumnOptions.Controls.Add(this.fldSelGridSrchCols);
			this.pnlColumnOptions.Controls.Add(this.gbOptions);
			this.pnlColumnOptions.Controls.Add(this.lblSearchColumns);
			resources.ApplyResources(this.pnlColumnOptions, "pnlColumnOptions");
			this.pnlColumnOptions.Name = "pnlColumnOptions";
			// 
			// fldSelGridSrchCols
			// 
			this.fldSelGridSrchCols.AllowUserToAddRows = false;
			this.fldSelGridSrchCols.AllowUserToDeleteRows = false;
			this.fldSelGridSrchCols.AllowUserToResizeColumns = false;
			this.fldSelGridSrchCols.AllowUserToResizeRows = false;
			resources.ApplyResources(this.fldSelGridSrchCols, "fldSelGridSrchCols");
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
			this.fldSelGridSrchCols.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridSrchCols.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.fldSelGridSrchCols, null);
			this.locExtender.SetLocalizationComment(this.fldSelGridSrchCols, null);
			this.locExtender.SetLocalizingId(this.fldSelGridSrchCols, "FindDlg.fldSelGridSrchCols");
			this.fldSelGridSrchCols.MultiSelect = false;
			this.fldSelGridSrchCols.Name = "fldSelGridSrchCols";
			this.fldSelGridSrchCols.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridSrchCols.RowHeadersVisible = false;
			this.fldSelGridSrchCols.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridSrchCols.ShowWaterMarkWhenDirty = false;
			this.fldSelGridSrchCols.WaterMark = "!";
			this.fldSelGridSrchCols.AfterUserChangedValue += new SIL.Pa.UI.Controls.FieldSelectorGrid.AfterUserChangedValueHandler(this.fldSelGridSrchCols_AfterUserChangedValue);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// FindDlg
			// 
			this.AcceptButton = this.btnFind;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.pnlColumnOptions);
			this.Controls.Add(this.pnlFindWhat);
			this.Controls.Add(this.pnlButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "FindDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.TopMost = true;
			this.gbOptions.ResumeLayout(false);
			this.gbOptions.PerformLayout();
			this.pnlButtons.ResumeLayout(false);
			this.pnlFindWhat.ResumeLayout(false);
			this.pnlFindWhat.PerformLayout();
			this.pnlColumnOptions.ResumeLayout(false);
			this.pnlColumnOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridSrchCols)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFindWhat;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkMatchCase;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.ComboBox cboFindWhat;
		private System.Windows.Forms.CheckBox chkMatchEntireWord;
		private System.Windows.Forms.CheckBox chkRegEx;
		private System.Windows.Forms.CheckBox chkStartsWith;
		private System.Windows.Forms.GroupBox gbOptions;
		private System.Windows.Forms.Label lblSearchColumns;
		private System.Windows.Forms.CheckBox chkReverseSearch;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Panel pnlFindWhat;
		private System.Windows.Forms.Panel pnlColumnOptions;
		private SIL.Pa.UI.Controls.FieldSelectorGrid fldSelGridSrchCols;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.CheckBox chkSrchCollapsedGrps;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}