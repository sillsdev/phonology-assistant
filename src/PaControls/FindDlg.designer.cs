namespace SIL.Pa.Controls
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.lblFindWhat = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkMatchCase = new System.Windows.Forms.CheckBox();
			this.btnFind1 = new System.Windows.Forms.Button();
			this.cboFindWhat = new System.Windows.Forms.ComboBox();
			this.chkMatchEntireWord = new System.Windows.Forms.CheckBox();
			this.chkRegEx = new System.Windows.Forms.CheckBox();
			this.chkStartsWith = new System.Windows.Forms.CheckBox();
			this.gbOptions = new System.Windows.Forms.GroupBox();
			this.chkReverseSearch = new System.Windows.Forms.CheckBox();
			this.lblSearchColumns = new System.Windows.Forms.Label();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.pnlFindWhat = new System.Windows.Forms.Panel();
			this.pnlColumnOptions = new System.Windows.Forms.Panel();
			this.fldSelGridSrchCols = new SIL.Pa.Controls.FieldSelectorGrid();
			this.btnHelp = new System.Windows.Forms.Button();
			this.gbOptions.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.pnlFindWhat.SuspendLayout();
			this.pnlColumnOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fldSelGridSrchCols)).BeginInit();
			this.SuspendLayout();
			// 
			// lblFindWhat
			// 
			resources.ApplyResources(this.lblFindWhat, "lblFindWhat");
			this.lblFindWhat.Name = "lblFindWhat";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkMatchCase
			// 
			resources.ApplyResources(this.chkMatchCase, "chkMatchCase");
			this.chkMatchCase.Name = "chkMatchCase";
			this.chkMatchCase.UseVisualStyleBackColor = true;
			// 
			// btnFind1
			// 
			resources.ApplyResources(this.btnFind1, "btnFind1");
			this.btnFind1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFind1.Name = "btnFind1";
			this.btnFind1.UseVisualStyleBackColor = true;
			this.btnFind1.Click += new System.EventHandler(this.btnFind);
			// 
			// cboFindWhat
			// 
			resources.ApplyResources(this.cboFindWhat, "cboFindWhat");
			this.cboFindWhat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cboFindWhat.Name = "cboFindWhat";
			// 
			// chkMatchEntireWord
			// 
			resources.ApplyResources(this.chkMatchEntireWord, "chkMatchEntireWord");
			this.chkMatchEntireWord.Name = "chkMatchEntireWord";
			this.chkMatchEntireWord.UseVisualStyleBackColor = true;
			this.chkMatchEntireWord.CheckedChanged += new System.EventHandler(this.chkMatchEntireWord_CheckedChanged);
			// 
			// chkRegEx
			// 
			resources.ApplyResources(this.chkRegEx, "chkRegEx");
			this.chkRegEx.Name = "chkRegEx";
			this.chkRegEx.UseVisualStyleBackColor = true;
			this.chkRegEx.CheckedChanged += new System.EventHandler(this.cbRegEx_CheckedChanged);
			// 
			// chkStartsWith
			// 
			resources.ApplyResources(this.chkStartsWith, "chkStartsWith");
			this.chkStartsWith.Name = "chkStartsWith";
			this.chkStartsWith.UseVisualStyleBackColor = true;
			this.chkStartsWith.CheckedChanged += new System.EventHandler(this.chkStartsWith_CheckedChanged);
			// 
			// gbOptions
			// 
			resources.ApplyResources(this.gbOptions, "gbOptions");
			this.gbOptions.Controls.Add(this.chkReverseSearch);
			this.gbOptions.Controls.Add(this.chkMatchCase);
			this.gbOptions.Controls.Add(this.chkStartsWith);
			this.gbOptions.Controls.Add(this.chkMatchEntireWord);
			this.gbOptions.Controls.Add(this.chkRegEx);
			this.gbOptions.Name = "gbOptions";
			this.gbOptions.TabStop = false;
			// 
			// chkReverseSearch
			// 
			resources.ApplyResources(this.chkReverseSearch, "chkReverseSearch");
			this.chkReverseSearch.Name = "chkReverseSearch";
			this.chkReverseSearch.UseVisualStyleBackColor = true;
			// 
			// lblSearchColumns
			// 
			resources.ApplyResources(this.lblSearchColumns, "lblSearchColumns");
			this.lblSearchColumns.BackColor = System.Drawing.Color.Transparent;
			this.lblSearchColumns.Name = "lblSearchColumns";
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnFind1);
			this.pnlButtons.Controls.Add(this.btnCancel);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
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
			this.fldSelGridSrchCols.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.fldSelGridSrchCols.IsDirty = false;
			this.fldSelGridSrchCols.MultiSelect = false;
			this.fldSelGridSrchCols.Name = "fldSelGridSrchCols";
			this.fldSelGridSrchCols.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.fldSelGridSrchCols.RowHeadersVisible = false;
			this.fldSelGridSrchCols.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.fldSelGridSrchCols.AfterUserChangedValue += new SIL.Pa.Controls.FieldSelectorGrid.AfterUserChangedValueHandler(this.fldSelGridSrchCols_AfterUserChangedValue);
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// FindDlg
			// 
			this.AcceptButton = this.btnFind1;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.pnlColumnOptions);
			this.Controls.Add(this.pnlFindWhat);
			this.Controls.Add(this.pnlButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFindWhat;
		//private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkMatchCase;
		private System.Windows.Forms.Button btnFind1;
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
		private SIL.Pa.Controls.FieldSelectorGrid fldSelGridSrchCols;
		private System.Windows.Forms.Button btnHelp;
	}
}