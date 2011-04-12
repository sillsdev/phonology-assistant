namespace SIL.Pa.UI.Views
{
	partial class ChartVwBase
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
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.m_chrGrid = new SIL.Pa.UI.Controls.CharGrid();
			this.m_histogram = new SIL.Pa.UI.Controls.Histogram();
			this.m_toopTip = new System.Windows.Forms.ToolTip(this.components);
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.Location = new System.Drawing.Point(0, 0);
			this.splitOuter.Name = "splitOuter";
			this.splitOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.m_chrGrid);
			this.splitOuter.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.splitOuter.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.m_histogram);
			this.splitOuter.Panel2.Padding = new System.Windows.Forms.Padding(10, 1, 10, 10);
			this.splitOuter.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitOuter.Size = new System.Drawing.Size(630, 606);
			this.splitOuter.SplitterDistance = 351;
			this.splitOuter.SplitterWidth = 8;
			this.splitOuter.TabIndex = 4;
			// 
			// m_chrGrid
			// 
			this.m_chrGrid.CellWidth = 38;
			this.m_chrGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_chrGrid.HeadersVisible = true;
			this.m_chrGrid.Location = new System.Drawing.Point(10, 10);
			this.m_chrGrid.Name = "m_chrGrid";
			this.m_chrGrid.SearchWhenPhoneDoubleClicked = true;
			this.m_chrGrid.Size = new System.Drawing.Size(610, 341);
			this.m_chrGrid.TabIndex = 2;
			// 
			// m_histogram
			// 
			this.m_histogram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_histogram.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.m_histogram.Location = new System.Drawing.Point(10, 1);
			this.m_histogram.Name = "m_histogram";
			this.m_histogram.Size = new System.Drawing.Size(610, 236);
			this.m_histogram.TabIndex = 0;
			// 
			// ChartVwBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitOuter);
			this.Name = "ChartVwBase";
			this.Size = new System.Drawing.Size(630, 606);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip m_toopTip;
		protected SIL.Pa.UI.Controls.CharGrid m_chrGrid;
		private System.Windows.Forms.SplitContainer splitOuter;
		private SIL.Pa.UI.Controls.Histogram m_histogram;

	}
}