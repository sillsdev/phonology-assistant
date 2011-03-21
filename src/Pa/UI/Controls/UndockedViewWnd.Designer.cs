namespace SIL.Pa.UI.Controls
{
	partial class UndockedViewWnd
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.sblblMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.sblblProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.sblblPercent = new System.Windows.Forms.ToolStripStatusLabel();
			this.sbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.sblblFilter = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sblblMain,
            this.sblblProgress,
            this.sblblPercent,
            this.sbProgress,
            this.sblblFilter});
			this.statusStrip.Location = new System.Drawing.Point(0, 458);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(710, 25);
			this.statusStrip.TabIndex = 2;
			// 
			// sblblMain
			// 
			this.sblblMain.AutoSize = false;
			this.sblblMain.BackColor = System.Drawing.SystemColors.Control;
			this.sblblMain.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
			this.sblblMain.Name = "sblblMain";
			this.sblblMain.Size = new System.Drawing.Size(321, 20);
			this.sblblMain.Spring = true;
			this.sblblMain.Text = "#";
			this.sblblMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// sblblProgress
			// 
			this.sblblProgress.BackColor = System.Drawing.SystemColors.Control;
			this.sblblProgress.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.sblblProgress.Image = global::SIL.Pa.Properties.Resources.LoadingWheel;
			this.sblblProgress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.sblblProgress.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
			this.sblblProgress.Name = "sblblProgress";
			this.sblblProgress.Size = new System.Drawing.Size(30, 20);
			this.sblblProgress.Text = "#";
			this.sblblProgress.VisibleChanged += new System.EventHandler(this.HandleProgressLabelVisibleChanged);
			// 
			// sblblPercent
			// 
			this.sblblPercent.BackColor = System.Drawing.Color.Transparent;
			this.sblblPercent.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.sblblPercent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.sblblPercent.Name = "sblblPercent";
			this.sblblPercent.Size = new System.Drawing.Size(14, 20);
			this.sblblPercent.Text = "#";
			// 
			// sbProgress
			// 
			this.sbProgress.AutoSize = false;
			this.sbProgress.Name = "sbProgress";
			this.sbProgress.Size = new System.Drawing.Size(200, 19);
			// 
			// sblblFilter
			// 
			this.sblblFilter.AutoSize = false;
			this.sblblFilter.Name = "sblblFilter";
			this.sblblFilter.Size = new System.Drawing.Size(118, 20);
			this.sblblFilter.Text = "FilterIndicator";
			// 
			// UndockedViewWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(710, 483);
			this.Controls.Add(this.statusStrip);
			this.Name = "UndockedViewWnd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "#";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel sblblMain;
		private System.Windows.Forms.ToolStripStatusLabel sblblProgress;
		private System.Windows.Forms.ToolStripProgressBar sbProgress;
		private System.Windows.Forms.ToolStripStatusLabel sblblFilter;
		private System.Windows.Forms.ToolStripStatusLabel sblblPercent;
	}
}