namespace SIL.Pa
{
	partial class DataCorpusWnd
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.paPanel1 = new SIL.Pa.Controls.PaPanel();
			this.rawRecVw = new SIL.Pa.Controls.RawRecordView();
			this.pnlMasterOuter = new System.Windows.Forms.Panel();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.sblblMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.sblblProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.paPanel1.SuspendLayout();
			this.pnlMasterOuter.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.paPanel1);
			this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
			this.splitContainer1.Panel2MinSize = 0;
			this.splitContainer1.Size = new System.Drawing.Size(676, 444);
			this.splitContainer1.SplitterDistance = 352;
			this.splitContainer1.SplitterWidth = 8;
			this.splitContainer1.TabIndex = 0;
			this.splitContainer1.TabStop = false;
			// 
			// paPanel1
			// 
			this.paPanel1.BackColor = System.Drawing.SystemColors.Window;
			this.paPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.paPanel1.Controls.Add(this.rawRecVw);
			this.paPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.paPanel1.DoubleBuffered = false;
			this.paPanel1.Location = new System.Drawing.Point(10, 0);
			this.paPanel1.Name = "paPanel1";
			this.paPanel1.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
			this.paPanel1.Size = new System.Drawing.Size(656, 74);
			this.paPanel1.TabIndex = 0;
			// 
			// rawRecVw
			// 
			this.rawRecVw.BackColor = System.Drawing.SystemColors.Window;
			this.rawRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rawRecVw.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rawRecVw.Location = new System.Drawing.Point(4, 4);
			this.rawRecVw.Name = "rawRecVw";
			this.rawRecVw.ReadOnly = true;
			this.rawRecVw.Size = new System.Drawing.Size(650, 68);
			this.rawRecVw.TabIndex = 0;
			this.rawRecVw.TabStop = false;
			this.rawRecVw.Text = "";
			this.rawRecVw.WordWrap = false;
			// 
			// pnlMasterOuter
			// 
			this.pnlMasterOuter.Controls.Add(this.splitContainer1);
			this.pnlMasterOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMasterOuter.Location = new System.Drawing.Point(0, 0);
			this.pnlMasterOuter.Name = "pnlMasterOuter";
			this.pnlMasterOuter.Size = new System.Drawing.Size(676, 444);
			this.pnlMasterOuter.TabIndex = 1;
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sblblMain,
            this.sblblProgress,
            this.sbProgress});
			this.statusStrip.Location = new System.Drawing.Point(0, 444);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(676, 24);
			this.statusStrip.TabIndex = 2;
			// 
			// sblblMain
			// 
			this.sblblMain.AutoSize = false;
			this.sblblMain.Name = "sblblMain";
			this.sblblMain.Size = new System.Drawing.Size(430, 19);
			this.sblblMain.Spring = true;
			this.sblblMain.Text = "#";
			this.sblblMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// sblblProgress
			// 
			this.sblblProgress.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.sblblProgress.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.sblblProgress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sblblProgress.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
			this.sblblProgress.Name = "sblblProgress";
			this.sblblProgress.Size = new System.Drawing.Size(19, 19);
			this.sblblProgress.Text = "#";
			// 
			// sbProgress
			// 
			this.sbProgress.AutoSize = false;
			this.sbProgress.Name = "sbProgress";
			this.sbProgress.Size = new System.Drawing.Size(200, 18);
			// 
			// DataCorpusWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(676, 468);
			this.Controls.Add(this.pnlMasterOuter);
			this.Controls.Add(this.statusStrip);
			this.Name = "DataCorpusWnd";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Data Corpus";
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.paPanel1.ResumeLayout(false);
			this.pnlMasterOuter.ResumeLayout(false);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private SIL.Pa.Controls.PaPanel paPanel1;
		private SIL.Pa.Controls.RawRecordView rawRecVw;
		private System.Windows.Forms.Panel pnlMasterOuter;
		public System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel sblblMain;
		private System.Windows.Forms.ToolStripStatusLabel sblblProgress;
		private System.Windows.Forms.ToolStripProgressBar sbProgress;

	}
}