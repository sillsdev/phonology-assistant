namespace SIL.Pa.UI.Controls
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartOptionsDropDown));
			this.lblSSegsToIgnore = new System.Windows.Forms.Label();
			this.lnkRefresh = new System.Windows.Forms.LinkLabel();
			this.lnkHelp = new System.Windows.Forms.LinkLabel();
			this.pnlPicker = new System.Windows.Forms.Panel();
			this.pickerIgnore = new SIL.Pa.UI.Controls.CharPicker();
			this.pnlPicker.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblSSegsToIgnore
			// 
			this.lblSSegsToIgnore.AutoEllipsis = true;
			resources.ApplyResources(this.lblSSegsToIgnore, "lblSSegsToIgnore");
			this.lblSSegsToIgnore.Name = "lblSSegsToIgnore";
			// 
			// lnkRefresh
			// 
			resources.ApplyResources(this.lnkRefresh, "lnkRefresh");
			this.lnkRefresh.Name = "lnkRefresh";
			this.lnkRefresh.TabStop = true;
			// 
			// lnkHelp
			// 
			resources.ApplyResources(this.lnkHelp, "lnkHelp");
			this.lnkHelp.Name = "lnkHelp";
			this.lnkHelp.TabStop = true;
			this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelp_LinkClicked);
			// 
			// pnlPicker
			// 
			this.pnlPicker.Controls.Add(this.pickerIgnore);
			resources.ApplyResources(this.pnlPicker, "pnlPicker");
			this.pnlPicker.Name = "pnlPicker";
			// 
			// pickerIgnore
			// 
			resources.ApplyResources(this.pickerIgnore, "pickerIgnore");
			this.pickerIgnore.AutoSizeItems = false;
			this.pickerIgnore.BackColor = System.Drawing.Color.Transparent;
			this.pickerIgnore.CheckItemsOnClick = true;
			this.pickerIgnore.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.pickerIgnore.ItemSize = new System.Drawing.Size(30, 30);
			this.pickerIgnore.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
			this.pickerIgnore.Name = "pickerIgnore";
			// 
			// ChartOptionsDropDown
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.pnlPicker);
			this.Controls.Add(this.lnkHelp);
			this.Controls.Add(this.lnkRefresh);
			this.Controls.Add(this.lblSSegsToIgnore);
			this.Name = "ChartOptionsDropDown";
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
