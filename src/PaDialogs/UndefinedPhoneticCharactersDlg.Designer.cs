namespace SIL.Pa.Dialogs
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
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.chkIgnoreInSearches = new System.Windows.Forms.CheckBox();
			this.chkShowUndefinedCharDlg = new System.Windows.Forms.CheckBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.m_grid = new SIL.SpeechTools.Utils.SilGrid();
			this.lblInfo = new System.Windows.Forms.Label();
			this.pnlButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
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
			// m_grid
			// 
			this.m_grid.AllowUserToAddRows = false;
			this.m_grid.AllowUserToDeleteRows = false;
			this.m_grid.AllowUserToOrderColumns = true;
			this.m_grid.AllowUserToResizeRows = false;
			this.m_grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_grid.DefaultCellStyle = dataGridViewCellStyle2;
			resources.ApplyResources(this.m_grid, "m_grid");
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.ReadOnly = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersVisible = false;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.VirtualMode = true;
			this.m_grid.WaterMark = "!";
			this.m_grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.m_grid_CellValueNeeded);
			this.m_grid.RowHeightInfoNeeded += new System.Windows.Forms.DataGridViewRowHeightInfoNeededEventHandler(this.m_grid_RowHeightInfoNeeded);
			// 
			// lblInfo
			// 
			resources.ApplyResources(this.lblInfo, "lblInfo");
			this.lblInfo.Name = "lblInfo";
			// 
			// UndefinedPhoneticCharactersDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOK;
			this.Controls.Add(this.m_grid);
			this.Controls.Add(this.pnlButtons);
			this.Controls.Add(this.lblInfo);
			this.Name = "UndefinedPhoneticCharactersDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.pnlButtons.ResumeLayout(false);
			this.pnlButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOK;
		private SIL.SpeechTools.Utils.SilGrid m_grid;
		private System.Windows.Forms.Label lblInfo;
		protected System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.CheckBox chkShowUndefinedCharDlg;
		private System.Windows.Forms.CheckBox chkIgnoreInSearches;


	}
}