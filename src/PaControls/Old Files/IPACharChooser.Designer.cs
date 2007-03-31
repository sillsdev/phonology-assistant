namespace SIL.Pa.Controls
{
	partial class IPACharChooser
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.flPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// flPanel
			// 
			this.flPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flPanel.Location = new System.Drawing.Point(0, 0);
			this.flPanel.Name = "flPanel";
			this.flPanel.Size = new System.Drawing.Size(176, 182);
			this.flPanel.TabIndex = 0;
			// 
			// IPACharChooser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.flPanel);
			this.Name = "IPACharChooser";
			this.Size = new System.Drawing.Size(176, 182);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flPanel;

	}
}
