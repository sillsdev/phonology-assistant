namespace SIL.Pa.UI.Controls
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
			this.pnlHeading = new System.Windows.Forms.Panel();
			this.lnkHelp = new System.Windows.Forms.LinkLabel();
			this.lnkCommand = new System.Windows.Forms.LinkLabel();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlHeading
			// 
			this.pnlHeading.BackColor = System.Drawing.SystemColors.Control;
			this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeading.Location = new System.Drawing.Point(0, 0);
			this.pnlHeading.Name = "pnlHeading";
			this.pnlHeading.Size = new System.Drawing.Size(127, 32);
			this.pnlHeading.TabIndex = 0;
			this.pnlHeading.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeading_Paint);
			// 
			// lnkHelp
			// 
			this.lnkHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lnkHelp.AutoSize = true;
			this.lnkHelp.BackColor = System.Drawing.Color.Transparent;
			this.lnkHelp.Enabled = false;
			this.locExtender.SetLocalizableToolTip(this.lnkHelp, null);
			this.locExtender.SetLocalizationComment(this.lnkHelp, null);
			this.locExtender.SetLocalizingId(this.lnkHelp, "Views.WordLists.CellInfoPopup.HelpLink");
			this.lnkHelp.Location = new System.Drawing.Point(64, 186);
			this.lnkHelp.Name = "lnkHelp";
			this.lnkHelp.Size = new System.Drawing.Size(29, 13);
			this.lnkHelp.TabIndex = 1;
			this.lnkHelp.TabStop = true;
			this.lnkHelp.Text = "Help";
			this.lnkHelp.Click += new System.EventHandler(this.HandleLinkClick);
			// 
			// lnkCommand
			// 
			this.lnkCommand.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.lnkCommand.AutoSize = true;
			this.lnkCommand.BackColor = System.Drawing.Color.Transparent;
			this.lnkCommand.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lnkCommand, null);
			this.locExtender.SetLocalizationComment(this.lnkCommand, null);
			this.locExtender.SetLocalizationPriority(this.lnkCommand, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lnkCommand, "GridCellInfoPopup.lnkCommand");
			this.lnkCommand.Location = new System.Drawing.Point(44, 186);
			this.lnkCommand.Name = "lnkCommand";
			this.lnkCommand.Size = new System.Drawing.Size(14, 13);
			this.lnkCommand.TabIndex = 2;
			this.lnkCommand.TabStop = true;
			this.lnkCommand.Text = "#";
			this.lnkCommand.Click += new System.EventHandler(this.HandleLinkClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// GridCellInfoPopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lnkCommand);
			this.Controls.Add(this.lnkHelp);
			this.Controls.Add(this.pnlHeading);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "GridCellInfoPopup.PaPopup");
			this.Name = "GridCellInfoPopup";
			this.Size = new System.Drawing.Size(127, 206);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlHeading;
		private System.Windows.Forms.LinkLabel lnkHelp;
		private System.Windows.Forms.LinkLabel lnkCommand;
		private System.Windows.Forms.ToolTip m_toolTip;
		private L10NSharp.UI.L10NSharpExtender locExtender;
	}
}
