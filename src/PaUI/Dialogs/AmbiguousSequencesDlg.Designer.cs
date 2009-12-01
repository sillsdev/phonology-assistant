namespace SIL.Pa
{
	partial class AmbiguousSequencesDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AmbiguousSequencesDlg));
			this.pnlGrid = new SIL.Pa.Controls.PaPanel();
			this.m_grid = new SilUtils.SilGrid();
			this.chkShowDefaults = new System.Windows.Forms.CheckBox();
			this.pnlButtons.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.chkShowDefaults);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Controls.SetChildIndex(this.btnOK, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnCancel, 0);
			this.pnlButtons.Controls.SetChildIndex(this.btnHelp, 0);
			this.pnlButtons.Controls.SetChildIndex(this.chkShowDefaults, 0);
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
			// pnlGrid
			// 
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this.m_grid);
			resources.ApplyResources(this.pnlGrid, "pnlGrid");
			this.pnlGrid.DoubleBuffered = false;
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
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
			this.m_grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			resources.ApplyResources(this.m_grid, "m_grid");
			this.m_grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(174)))));
			this.m_grid.IsDirty = false;
			this.m_grid.MultiSelect = false;
			this.m_grid.Name = "m_grid";
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_grid.ShowWaterMarkWhenDirty = false;
			this.m_grid.WaterMark = "!";
			this.m_grid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.m_grid_UserDeletingRow);
			this.m_grid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.m_grid_CellBeginEdit);
			this.m_grid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.m_grid_CellValidating);
			this.m_grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.m_grid_RowsAdded);
			this.m_grid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellEndEdit);
			this.m_grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.m_grid_DefaultValuesNeeded);
			// 
			// chkShowDefaults
			// 
			resources.ApplyResources(this.chkShowDefaults, "chkShowDefaults");
			this.chkShowDefaults.BackColor = System.Drawing.Color.Transparent;
			this.chkShowDefaults.Checked = true;
			this.chkShowDefaults.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowDefaults.Name = "chkShowDefaults";
			this.chkShowDefaults.UseVisualStyleBackColor = false;
			this.chkShowDefaults.CheckedChanged += new System.EventHandler(this.chkShowDefaults_CheckedChanged);
			// 
			// AmbiguousSequencesDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlGrid);
			this.Name = "AmbiguousSequencesDlg";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.pnlGrid, 0);
			this.pnlButtons.ResumeLayout(false);
			this.pnlButtons.PerformLayout();
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SIL.Pa.Controls.PaPanel pnlGrid;
		private SilUtils.SilGrid m_grid;
		private System.Windows.Forms.CheckBox chkShowDefaults;


	}
}