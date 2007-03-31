namespace SIL.Pa.Controls
{
	partial class GridCellInfoPopup
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridCellInfoPopup));
			this.pnlHeading = new System.Windows.Forms.Panel();
			this.lnkHelp = new System.Windows.Forms.LinkLabel();
			this.lnkCommand = new System.Windows.Forms.LinkLabel();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// pnlHeading
			// 
			this.pnlHeading.BackColor = System.Drawing.SystemColors.Control;
			resources.ApplyResources(this.pnlHeading, "pnlHeading");
			this.pnlHeading.Name = "pnlHeading";
			this.pnlHeading.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeading_Paint);
			// 
			// lnkHelp
			// 
			resources.ApplyResources(this.lnkHelp, "lnkHelp");
			this.lnkHelp.Name = "lnkHelp";
			this.lnkHelp.TabStop = true;
			this.lnkHelp.Click += new System.EventHandler(this.HandleLinkClick);
			// 
			// lnkCommand
			// 
			resources.ApplyResources(this.lnkCommand, "lnkCommand");
			this.lnkCommand.Name = "lnkCommand";
			this.lnkCommand.TabStop = true;
			this.lnkCommand.Click += new System.EventHandler(this.HandleLinkClick);
			// 
			// GridCellInfoPopup
			// 
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lnkCommand);
			this.Controls.Add(this.lnkHelp);
			this.Controls.Add(this.pnlHeading);
			this.Name = "GridCellInfoPopup";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlHeading;
		private System.Windows.Forms.LinkLabel lnkHelp;
		private System.Windows.Forms.LinkLabel lnkCommand;
		private System.Windows.Forms.ToolTip m_toolTip;
	}
}
