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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.m_chrGrid = new SIL.Pa.Controls.CharGrid();
			this.m_histogram = new SIL.Pa.Controls.Histogram();
			this.m_toopTip = new System.Windows.Forms.ToolTip(this.components);
			this.pnlMasterOuter = new System.Windows.Forms.Panel();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.pnlMasterOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.m_chrGrid);
			resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.m_histogram);
			resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
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
			// pnlMasterOuter
			// 
			this.pnlMasterOuter.Controls.Add(this.splitContainer1);
			resources.ApplyResources(this.pnlMasterOuter, "pnlMasterOuter");
			this.pnlMasterOuter.Name = "pnlMasterOuter";
			// 
			// ChartWndBase
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlMasterOuter);
			this.Name = "ChartWndBase";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.pnlMasterOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip m_toopTip;
		protected SIL.Pa.Controls.CharGrid m_chrGrid;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private SIL.Pa.Controls.Histogram m_histogram;
		private System.Windows.Forms.Panel pnlMasterOuter;

	}
}