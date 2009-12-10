using SilUtils.Controls;

namespace SIL.Pa.UI.Dialogs
{
	partial class UndefinedPhoneticCharactersDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UndefinedPhoneticCharactersDlg));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.chkIgnoreInSearches = new System.Windows.Forms.CheckBox();
			this.chkShowUndefinedCharDlg = new System.Windows.Forms.CheckBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblInfo = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.pnlSummary = new SilPanel();
			this.m_gridChars = new SilUtils.SilGrid();
			this.pgpChars = new SilGradientPanel();
			this.pnlDetails = new SilPanel();
			this.m_gridWhere = new SilUtils.SilGrid();
			this.pgpWhere = new SilGradientPanel();
			this.pnlButtons.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.pnlSummary.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_gridChars)).BeginInit();
			this.pnlDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_gridWhere)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
			this.pnlButtons.Controls.Add(this.chkIgnoreInSearches);
			this.pnlButtons.Controls.Add(this.chkShowUndefinedCharDlg);
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnOK);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// chkIgnoreInSearches
			// 
			resources.ApplyResources(this.chkIgnoreInSearches, "chkIgnoreInSearches");
			this.chkIgnoreInSearches.AutoEllipsis = true;
			this.chkIgnoreInSearches.Name = "chkIgnoreInSearches";
			this.chkIgnoreInSearches.UseVisualStyleBackColor = true;
			// 
			// chkShowUndefinedCharDlg
			// 
			resources.ApplyResources(this.chkShowUndefinedCharDlg, "chkShowUndefinedCharDlg");
			this.chkShowUndefinedCharDlg.Name = "chkShowUndefinedCharDlg";
			this.chkShowUndefinedCharDlg.UseVisualStyleBackColor = true;
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.Name = "lblInfo";
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.pnlSummary);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pnlDetails);
			// 
			// pnlSummary
			// 
			this.pnlSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSummary.ClipTextForChildControls = true;
			this.pnlSummary.ControlReceivingFocusOnMnemonic = null;
			this.pnlSummary.Controls.Add(this.m_gridChars);
			this.pnlSummary.Controls.Add(this.pgpChars);
			resources.ApplyResources(this.pnlSummary, "pnlSummary");
			this.pnlSummary.DoubleBuffered = true;
			this.pnlSummary.MnemonicGeneratesClick = false;
			this.pnlSummary.Name = "pnlSummary";
			this.pnlSummary.PaintExplorerBarBackground = false;
			// 
			// m_gridChars
			// 
			this.m_gridChars.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.m_gridChars.AllowUserToAddRows = false;
			this.m_gridChars.AllowUserToDeleteRows = false;
			this.m_gridChars.AllowUserToOrderColumns = true;
			this.m_gridChars.AllowUserToResizeRows = false;
			this.m_gridChars.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			this.m_gridChars.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_gridChars.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_gridChars.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridChars.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_gridChars.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this.m_gridChars, "m_gridChars");
			this.m_gridChars.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_gridChars.IsDirty = false;
			this.m_gridChars.MultiSelect = false;
			this.m_gridChars.Name = "m_gridChars";
			this.m_gridChars.ReadOnly = true;
			this.m_gridChars.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridChars.RowHeadersVisible = false;
			this.m_gridChars.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridChars.ShowWaterMarkWhenDirty = false;
			this.m_gridChars.WaterMark = "!";
			this.m_gridChars.Enter += new System.EventHandler(this.HandleGridEnter);
			this.m_gridChars.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_gridChars_RowEnter);
			this.m_gridChars.Leave += new System.EventHandler(this.HandleGridLeave);
			// 
			// pgpChars
			// 
			this.pgpChars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pgpChars.ClipTextForChildControls = false;
			this.pgpChars.ControlReceivingFocusOnMnemonic = this.m_gridChars;
			resources.ApplyResources(this.pgpChars, "pgpChars");
			this.pgpChars.DoubleBuffered = true;
			this.pgpChars.MakeDark = false;
			this.pgpChars.MnemonicGeneratesClick = false;
			this.pgpChars.Name = "pgpChars";
			this.pgpChars.PaintExplorerBarBackground = false;
			// 
			// pnlDetails
			// 
			this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlDetails.ClipTextForChildControls = true;
			this.pnlDetails.ControlReceivingFocusOnMnemonic = null;
			this.pnlDetails.Controls.Add(this.m_gridWhere);
			this.pnlDetails.Controls.Add(this.pgpWhere);
			resources.ApplyResources(this.pnlDetails, "pnlDetails");
			this.pnlDetails.DoubleBuffered = true;
			this.pnlDetails.MnemonicGeneratesClick = false;
			this.pnlDetails.Name = "pnlDetails";
			this.pnlDetails.PaintExplorerBarBackground = false;
			// 
			// m_gridWhere
			// 
			this.m_gridWhere.AllowUserToAddRows = false;
			this.m_gridWhere.AllowUserToDeleteRows = false;
			this.m_gridWhere.AllowUserToOrderColumns = true;
			this.m_gridWhere.AllowUserToResizeRows = false;
			this.m_gridWhere.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_gridWhere.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_gridWhere.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_gridWhere.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gridWhere.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_gridWhere.DefaultCellStyle = dataGridViewCellStyle4;
			resources.ApplyResources(this.m_gridWhere, "m_gridWhere");
			this.m_gridWhere.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_gridWhere.IsDirty = false;
			this.m_gridWhere.MultiSelect = false;
			this.m_gridWhere.Name = "m_gridWhere";
			this.m_gridWhere.ReadOnly = true;
			this.m_gridWhere.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_gridWhere.RowHeadersVisible = false;
			this.m_gridWhere.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_gridWhere.ShowWaterMarkWhenDirty = false;
			this.m_gridWhere.VirtualMode = true;
			this.m_gridWhere.WaterMark = "!";
			this.m_gridWhere.Enter += new System.EventHandler(this.HandleGridEnter);
			this.m_gridWhere.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_gridWhere_CellValueNeeded);
			this.m_gridWhere.Leave += new System.EventHandler(this.HandleGridLeave);
			this.m_gridWhere.RowHeightInfoNeeded += new System.Windows.Forms.DataGridViewRowHeightInfoNeededEventHandler(this.m_grid_RowHeightInfoNeeded);
			// 
			// pgpWhere
			// 
			this.pgpWhere.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pgpWhere.ClipTextForChildControls = false;
			this.pgpWhere.ControlReceivingFocusOnMnemonic = this.m_gridWhere;
			resources.ApplyResources(this.pgpWhere, "pgpWhere");
			this.pgpWhere.DoubleBuffered = true;
			this.pgpWhere.MakeDark = false;
			this.pgpWhere.MnemonicGeneratesClick = false;
			this.pgpWhere.Name = "pgpWhere";
			this.pgpWhere.PaintExplorerBarBackground = false;
			// 
			// UndefinedPhoneticCharactersDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOK;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.pnlButtons);
			this.Controls.Add(this.lblInfo);
			this.Name = "UndefinedPhoneticCharactersDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.pnlButtons.ResumeLayout(false);
			this.pnlButtons.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.pnlSummary.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_gridChars)).EndInit();
			this.pnlDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_gridWhere)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOK;
		private SilUtils.SilGrid m_gridChars;
		private System.Windows.Forms.Label lblInfo;
		protected System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.CheckBox chkShowUndefinedCharDlg;
		private System.Windows.Forms.CheckBox chkIgnoreInSearches;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private SilUtils.SilGrid m_gridWhere;
		private SilPanel pnlSummary;
		private SilGradientPanel pgpChars;
		private SilPanel pnlDetails;
		private SilGradientPanel pgpWhere;
	}
}