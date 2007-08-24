namespace SIL.Pa
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartVwBase));
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.m_chrGrid = new SIL.Pa.Controls.CharGrid();
			this.m_histogram = new SIL.Pa.Controls.Histogram();
			this.m_toopTip = new System.Windows.Forms.ToolTip(this.components);
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			resources.ApplyResources(this.splitOuter, "splitOuter");
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.m_chrGrid);
			resources.ApplyResources(this.splitOuter.Panel1, "splitOuter.Panel1");
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.m_histogram);
			resources.ApplyResources(this.splitOuter.Panel2, "splitOuter.Panel2");
			this.splitOuter.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
			// 
			// m_chrGrid
			// 
			this.m_chrGrid.CellWidth = 38;
			resources.ApplyResources(this.m_chrGrid, "m_chrGrid");
			this.m_chrGrid.HeadersVisible = true;
			this.m_chrGrid.Name = "m_chrGrid";
			this.m_chrGrid.SearchWhenPhoneDoubleClicked = true;
			// 
			// m_histogram
			// 
			resources.ApplyResources(this.m_histogram, "m_histogram");
			this.m_histogram.Name = "m_histogram";
			// 
			// ChartVwBase
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitOuter);
			this.Name = "ChartVwBase";
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip m_toopTip;
		protected SIL.Pa.Controls.CharGrid m_chrGrid;
		private System.Windows.Forms.SplitContainer splitOuter;
		private SIL.Pa.Controls.Histogram m_histogram;

	}
}