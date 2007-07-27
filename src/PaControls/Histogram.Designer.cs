namespace SIL.Pa.Controls
{
	partial class Histogram
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
			this.pnlScroller = new System.Windows.Forms.Panel();
			this.pnlPhones = new System.Windows.Forms.Panel();
			this.lblBarValue = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pnlFixedBorder = new SIL.Pa.Controls.PaPanel();
			this.pnlBars = new SIL.Pa.Controls.PaPanel();
			this.pnlYaxis = new SIL.Pa.Controls.PaPanel();
			this.pnlScroller.SuspendLayout();
			this.pnlFixedBorder.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlScroller
			// 
			this.pnlScroller.AutoScroll = true;
			this.pnlScroller.Controls.Add(this.pnlPhones);
			this.pnlScroller.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlScroller.Location = new System.Drawing.Point(40, 300);
			this.pnlScroller.Name = "pnlScroller";
			this.pnlScroller.Size = new System.Drawing.Size(347, 45);
			this.pnlScroller.TabIndex = 4;
			this.pnlScroller.Scroll += new System.Windows.Forms.ScrollEventHandler(this.pnlScroller_Scroll);
			// 
			// pnlPhones
			// 
			this.pnlPhones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.pnlPhones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlPhones.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.pnlPhones.Location = new System.Drawing.Point(0, 0);
			this.pnlPhones.Name = "pnlPhones";
			this.pnlPhones.Size = new System.Drawing.Size(229, 43);
			this.pnlPhones.TabIndex = 2;
			// 
			// lblBarValue
			// 
			this.lblBarValue.AutoSize = true;
			this.lblBarValue.BackColor = System.Drawing.SystemColors.Info;
			this.lblBarValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblBarValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblBarValue.Location = new System.Drawing.Point(83, 203);
			this.lblBarValue.Name = "lblBarValue";
			this.lblBarValue.Size = new System.Drawing.Size(37, 15);
			this.lblBarValue.TabIndex = 0;
			this.lblBarValue.Text = "label1";
			this.lblBarValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblBarValue.Visible = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(40, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(347, 6);
			this.panel1.TabIndex = 7;
			// 
			// pnlFixedBorder
			// 
			this.pnlFixedBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlFixedBorder.Controls.Add(this.pnlBars);
			this.pnlFixedBorder.DoubleBuffered = false;
			this.pnlFixedBorder.Location = new System.Drawing.Point(82, 47);
			this.pnlFixedBorder.Name = "pnlFixedBorder";
			this.pnlFixedBorder.Size = new System.Drawing.Size(276, 119);
			this.pnlFixedBorder.TabIndex = 4;
			this.pnlFixedBorder.Resize += new System.EventHandler(this.pnlFixedBorder_Resize);
			// 
			// pnlBars
			// 
			this.pnlBars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.pnlBars.BackColor = System.Drawing.SystemColors.Window;
			this.pnlBars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBars.DoubleBuffered = true;
			this.pnlBars.Location = new System.Drawing.Point(0, 0);
			this.pnlBars.Name = "pnlBars";
			this.pnlBars.Size = new System.Drawing.Size(232, 117);
			this.pnlBars.TabIndex = 4;
			this.pnlBars.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBars_Paint);
			// 
			// pnlYaxis
			// 
			this.pnlYaxis.BackColor = System.Drawing.Color.Transparent;
			this.pnlYaxis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlYaxis.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlYaxis.DoubleBuffered = true;
			this.pnlYaxis.Location = new System.Drawing.Point(0, 0);
			this.pnlYaxis.Name = "pnlYaxis";
			this.pnlYaxis.Size = new System.Drawing.Size(40, 345);
			this.pnlYaxis.TabIndex = 6;
			this.pnlYaxis.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlYaxis_Paint);
			// 
			// Histogram
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pnlScroller);
			this.Controls.Add(this.pnlFixedBorder);
			this.Controls.Add(this.pnlYaxis);
			this.Controls.Add(this.lblBarValue);
			this.Name = "Histogram";
			this.Size = new System.Drawing.Size(387, 345);
			this.pnlScroller.ResumeLayout(false);
			this.pnlFixedBorder.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlScroller;
		private System.Windows.Forms.Panel pnlPhones;
		private PaPanel pnlFixedBorder;
		private PaPanel pnlBars;
		private System.Windows.Forms.Label lblBarValue;
		private PaPanel pnlYaxis;
		private System.Windows.Forms.Panel panel1;
	}
}
