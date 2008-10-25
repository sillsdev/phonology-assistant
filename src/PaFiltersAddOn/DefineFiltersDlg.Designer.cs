namespace SIL.Pa.FiltersAddOn
{
	partial class DefineFiltersDlg
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
			this.lvFilters = new System.Windows.Forms.ListView();
			this.hdrFilter = new System.Windows.Forms.ColumnHeader();
			this.lblFilters = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.pnlButtons2 = new System.Windows.Forms.Panel();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.m_grid = new System.Windows.Forms.DataGridView();
			this.pnlName = new System.Windows.Forms.Panel();
			this.pnlButtons.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.pnlButtons2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.Location = new System.Drawing.Point(10, 352);
			this.pnlButtons.Size = new System.Drawing.Size(626, 40);
			this.pnlButtons.TabIndex = 200;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(460, 7);
			this.btnCancel.TabIndex = 1;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(374, 7);
			this.btnOK.TabIndex = 0;
			// 
			// btnHelp
			// 
			this.btnHelp.Location = new System.Drawing.Point(546, 7);
			this.btnHelp.TabIndex = 2;
			// 
			// lvFilters
			// 
			this.lvFilters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrFilter});
			this.lvFilters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvFilters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvFilters.HideSelection = false;
			this.lvFilters.Location = new System.Drawing.Point(0, 30);
			this.lvFilters.MultiSelect = false;
			this.lvFilters.Name = "lvFilters";
			this.lvFilters.Size = new System.Drawing.Size(180, 281);
			this.lvFilters.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvFilters.TabIndex = 1;
			this.lvFilters.UseCompatibleStateImageBehavior = false;
			this.lvFilters.View = System.Windows.Forms.View.Details;
			// 
			// lblFilters
			// 
			this.lblFilters.AutoEllipsis = true;
			this.lblFilters.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblFilters.Location = new System.Drawing.Point(0, 0);
			this.lblFilters.Name = "lblFilters";
			this.lblFilters.Size = new System.Drawing.Size(180, 30);
			this.lblFilters.TabIndex = 0;
			this.lblFilters.Text = "Available &Filters";
			this.lblFilters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(10, 10);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lvFilters);
			this.splitContainer1.Panel1.Controls.Add(this.pnlButtons2);
			this.splitContainer1.Panel1.Controls.Add(this.lblFilters);
			this.splitContainer1.Panel1MinSize = 180;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.m_grid);
			this.splitContainer1.Panel2.Controls.Add(this.pnlName);
			this.splitContainer1.Size = new System.Drawing.Size(626, 342);
			this.splitContainer1.SplitterDistance = 180;
			this.splitContainer1.SplitterWidth = 8;
			this.splitContainer1.TabIndex = 104;
			// 
			// pnlButtons2
			// 
			this.pnlButtons2.Controls.Add(this.btnCopy);
			this.pnlButtons2.Controls.Add(this.btnRemove);
			this.pnlButtons2.Controls.Add(this.btnAdd);
			this.pnlButtons2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons2.Location = new System.Drawing.Point(0, 311);
			this.pnlButtons2.Name = "pnlButtons2";
			this.pnlButtons2.Size = new System.Drawing.Size(180, 31);
			this.pnlButtons2.TabIndex = 2;
			// 
			// btnCopy
			// 
			this.btnCopy.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCopy.Location = new System.Drawing.Point(60, 5);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(56, 26);
			this.btnCopy.TabIndex = 1;
			this.btnCopy.Text = "&Copy";
			this.btnCopy.UseVisualStyleBackColor = true;
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.Location = new System.Drawing.Point(120, 5);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(60, 26);
			this.btnRemove.TabIndex = 2;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Location = new System.Drawing.Point(0, 5);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(56, 26);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "&Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			// 
			// m_grid
			// 
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_grid.Location = new System.Drawing.Point(0, 30);
			this.m_grid.Name = "m_grid";
			this.m_grid.Size = new System.Drawing.Size(438, 312);
			this.m_grid.TabIndex = 1;
			// 
			// pnlName
			// 
			this.pnlName.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlName.Location = new System.Drawing.Point(0, 0);
			this.pnlName.Name = "pnlName";
			this.pnlName.Size = new System.Drawing.Size(438, 30);
			this.pnlName.TabIndex = 0;
			// 
			// DefineFiltersDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(646, 392);
			this.Controls.Add(this.splitContainer1);
			this.Name = "DefineFiltersDlg";
			this.Text = "Define Filters";
			this.Controls.SetChildIndex(this.pnlButtons, 0);
			this.Controls.SetChildIndex(this.splitContainer1, 0);
			this.pnlButtons.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.pnlButtons2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvFilters;
		private System.Windows.Forms.Label lblFilters;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Panel pnlButtons2;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Panel pnlName;
		private System.Windows.Forms.DataGridView m_grid;
		private System.Windows.Forms.ColumnHeader hdrFilter;
	}
}