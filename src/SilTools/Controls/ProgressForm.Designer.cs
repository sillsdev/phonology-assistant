namespace SilTools.Controls
{
	partial class ProgressForm
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
			this.prgWheel = new SilTools.Controls.ProgressWheel();
			this.SuspendLayout();
			// 
			// prgWheel
			// 
			this.prgWheel.AutoSize = true;
			this.prgWheel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.prgWheel.BackColor = System.Drawing.Color.Transparent;
			this.prgWheel.Location = new System.Drawing.Point(10, 10);
			this.prgWheel.Maximum = 100;
			this.prgWheel.Message = "Message goes here";
			this.prgWheel.Name = "prgWheel";
			this.prgWheel.Size = new System.Drawing.Size(173, 48);
			this.prgWheel.TabIndex = 0;
			this.prgWheel.Value = 0;
			// 
			// ProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(203, 73);
			this.ControlBox = false;
			this.Controls.Add(this.prgWheel);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ProgressWheel prgWheel;
	}
}