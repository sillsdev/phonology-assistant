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
			this.pickerIgnore = new SIL.Pa.Controls.CharPicker();
			this.SuspendLayout();
			// 
			// lblSSegsToIgnore
			// 
			this.lblSSegsToIgnore.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblSSegsToIgnore.AutoEllipsis = true;
			this.lblSSegsToIgnore.AutoSize = true;
			this.lblSSegsToIgnore.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSSegsToIgnore.Location = new System.Drawing.Point(13, 10);
			this.lblSSegsToIgnore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSSegsToIgnore.Name = "lblSSegsToIgnore";
			this.lblSSegsToIgnore.Size = new System.Drawing.Size(195, 18);
			this.lblSSegsToIgnore.TabIndex = 1;
			this.lblSSegsToIgnore.Text = "Ignored Suprasegmentals";
			// 
			// lnkRefresh
			// 
			this.lnkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkRefresh.AutoSize = true;
			this.lnkRefresh.Location = new System.Drawing.Point(17, 98);
			this.lnkRefresh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lnkRefresh.Name = "lnkRefresh";
			this.lnkRefresh.Size = new System.Drawing.Size(96, 17);
			this.lnkRefresh.TabIndex = 3;
			this.lnkRefresh.TabStop = true;
			this.lnkRefresh.Text = "Refresh Chart";
			// 
			// lnkHelp
			// 
			this.lnkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lnkHelp.AutoSize = true;
			this.lnkHelp.Location = new System.Drawing.Point(164, 98);
			this.lnkHelp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lnkHelp.Name = "lnkHelp";
			this.lnkHelp.Size = new System.Drawing.Size(37, 17);
			this.lnkHelp.TabIndex = 4;
			this.lnkHelp.TabStop = true;
			this.lnkHelp.Text = "Help";
			this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
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
			this.pickerIgnore.Location = new System.Drawing.Point(13, 40);
			this.pickerIgnore.Name = "pickerIgnore";
			this.pickerIgnore.Padding = new System.Windows.Forms.Padding(0);
			this.pickerIgnore.Size = new System.Drawing.Size(194, 49);
			this.pickerIgnore.TabIndex = 2;
			this.pickerIgnore.Text = "charPicker1";
			// 
			// ChartOptionsDropDown
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lnkHelp);
			this.Controls.Add(this.lnkRefresh);
			this.Controls.Add(this.pickerIgnore);
			this.Controls.Add(this.lblSSegsToIgnore);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "ChartOptionsDropDown";
			this.Padding = new System.Windows.Forms.Padding(13, 40, 13, 37);
			this.Size = new System.Drawing.Size(220, 126);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSSegsToIgnore;
		private CharPicker pickerIgnore;
		public System.Windows.Forms.LinkLabel lnkRefresh;
		public System.Windows.Forms.LinkLabel lnkHelp;
	}
}
