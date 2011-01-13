namespace SilTools
{
	partial class SmallFadingWnd
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SmallFadingWnd));
			this.lblMsg = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			resources.ApplyResources(this.lblMsg, "lblMsg");
			this.lblMsg.AutoEllipsis = true;
			this.lblMsg.BackColor = System.Drawing.Color.Transparent;
			this.lblMsg.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.UseWaitCursor = true;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lblMsg);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.panel1.UseWaitCursor = true;
			// 
			// SmallFadingWnd
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SmallFadingWnd";
			this.Opacity = 0.8;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.UseWaitCursor = true;
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.Panel panel1;
	}
}