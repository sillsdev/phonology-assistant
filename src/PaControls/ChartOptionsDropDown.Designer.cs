namespace SIL.Pa.Controls
{
	partial class ChartOptionsDropDown
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
			this.lblSSegsToIgnore = new System.Windows.Forms.Label();
			this.lnkRefresh = new System.Windows.Forms.LinkLabel();
			this.lnkHelp = new System.Windows.Forms.LinkLabel();
			this.pnlPicker = new System.Windows.Forms.Panel();
			this.pickerIgnore = new SIL.Pa.Controls.CharPicker();
			this.pnlPicker.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblSSegsToIgnore
			// 
			this.lblSSegsToIgnore.AutoEllipsis = true;
			this.lblSSegsToIgnore.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSSegsToIgnore.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSSegsToIgnore.Location = new System.Drawing.Point(0, 0);
			this.lblSSegsToIgnore.Name = "lblSSegsToIgnore";
			this.lblSSegsToIgnore.Size = new System.Drawing.Size(165, 28);
			this.lblSSegsToIgnore.TabIndex = 1;
			this.lblSSegsToIgnore.Text = "Ignored Suprasegmentals";
			this.lblSSegsToIgnore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lnkRefresh
			// 
			this.lnkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkRefresh.AutoSize = true;
			this.lnkRefresh.Location = new System.Drawing.Point(10, 80);
			this.lnkRefresh.Name = "lnkRefresh";
			this.lnkRefresh.Size = new System.Drawing.Size(72, 13);
			this.lnkRefresh.TabIndex = 3;
			this.lnkRefresh.TabStop = true;
			this.lnkRefresh.Text = "Refresh Chart";
			// 
			// lnkHelp
			// 
			this.lnkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkHelp.AutoSize = true;
			this.lnkHelp.Location = new System.Drawing.Point(126, 80);
			this.lnkHelp.Name = "lnkHelp";
			this.lnkHelp.Size = new System.Drawing.Size(29, 13);
			this.lnkHelp.TabIndex = 4;
			this.lnkHelp.TabStop = true;
			this.lnkHelp.Text = "Help";
			this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
			// 
			// pnlPicker
			// 
			this.pnlPicker.Controls.Add(this.pickerIgnore);
			this.pnlPicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPicker.Location = new System.Drawing.Point(0, 28);
			this.pnlPicker.Name = "pnlPicker";
			this.pnlPicker.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.pnlPicker.Size = new System.Drawing.Size(165, 44);
			this.pnlPicker.TabIndex = 5;
			// 
			// pickerIgnore
			// 
			this.pickerIgnore.AutoSize = false;
			this.pickerIgnore.AutoSizeItems = false;
			this.pickerIgnore.BackColor = System.Drawing.Color.Transparent;
			this.pickerIgnore.CheckItemsOnClick = true;
			this.pickerIgnore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pickerIgnore.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.pickerIgnore.ItemSize = new System.Drawing.Size(30, 30);
			this.pickerIgnore.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.pickerIgnore.Location = new System.Drawing.Point(10, 0);
			this.pickerIgnore.Name = "pickerIgnore";
			this.pickerIgnore.Padding = new System.Windows.Forms.Padding(0);
			this.pickerIgnore.Size = new System.Drawing.Size(145, 44);
			this.pickerIgnore.TabIndex = 2;
			this.pickerIgnore.Text = "charPicker1";
			// 
			// ChartOptionsDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.pnlPicker);
			this.Controls.Add(this.lnkHelp);
			this.Controls.Add(this.lnkRefresh);
			this.Controls.Add(this.lblSSegsToIgnore);
			this.Name = "ChartOptionsDropDown";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 30);
			this.Size = new System.Drawing.Size(165, 102);
			this.pnlPicker.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSSegsToIgnore;
		private CharPicker pickerIgnore;
		public System.Windows.Forms.LinkLabel lnkRefresh;
		public System.Windows.Forms.LinkLabel lnkHelp;
		private System.Windows.Forms.Panel pnlPicker;
	}
}
