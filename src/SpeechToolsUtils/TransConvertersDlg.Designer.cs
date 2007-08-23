namespace SIL.SpeechTools.Utils
{
	partial class TransConvertersDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransConvertersDlg));
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkMakeBackup = new System.Windows.Forms.CheckBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblHelp = new System.Windows.Forms.Label();
			this.cmnuConverters = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiNone = new System.Windows.Forms.ToolStripMenuItem();
			this.tsSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiBrowse = new System.Windows.Forms.ToolStripMenuItem();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.pnlButtons.SuspendLayout();
			this.cmnuConverters.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlButtons
			// 
			this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
			this.pnlButtons.Controls.Add(this.btnCancel);
			this.pnlButtons.Controls.Add(this.chkMakeBackup);
			this.pnlButtons.Controls.Add(this.btnHelp);
			this.pnlButtons.Controls.Add(this.btnOK);
			resources.ApplyResources(this.pnlButtons, "pnlButtons");
			this.pnlButtons.Name = "pnlButtons";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.helpProvider1.SetShowHelp(this.btnCancel, ((bool)(resources.GetObject("btnCancel.ShowHelp"))));
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// chkMakeBackup
			// 
			resources.ApplyResources(this.chkMakeBackup, "chkMakeBackup");
			this.chkMakeBackup.Checked = true;
			this.chkMakeBackup.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMakeBackup.Name = "chkMakeBackup";
			this.helpProvider1.SetShowHelp(this.chkMakeBackup, ((bool)(resources.GetObject("chkMakeBackup.ShowHelp"))));
			this.chkMakeBackup.UseVisualStyleBackColor = true;
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// lblHelp
			// 
			this.lblHelp.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblHelp, "lblHelp");
			this.lblHelp.Name = "lblHelp";
			// 
			// cmnuConverters
			// 
			this.cmnuConverters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNone,
            this.tsSeparator,
            this.tsmiBrowse});
			this.cmnuConverters.Name = "cmnuConverters";
			this.cmnuConverters.ShowCheckMargin = true;
			this.cmnuConverters.ShowImageMargin = false;
			this.cmnuConverters.ShowItemToolTips = false;
			resources.ApplyResources(this.cmnuConverters, "cmnuConverters");
			this.cmnuConverters.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmnuConverters_ItemClicked);
			// 
			// tsmiNone
			// 
			this.tsmiNone.Name = "tsmiNone";
			resources.ApplyResources(this.tsmiNone, "tsmiNone");
			// 
			// tsSeparator
			// 
			this.tsSeparator.Name = "tsSeparator";
			resources.ApplyResources(this.tsSeparator, "tsSeparator");
			// 
			// tsmiBrowse
			// 
			this.tsmiBrowse.Name = "tsmiBrowse";
			resources.ApplyResources(this.tsmiBrowse, "tsmiBrowse");
			// 
			// helpProvider1
			// 
			resources.ApplyResources(this.helpProvider1, "helpProvider1");
			// 
			// TransConvertersDlg
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.lblHelp);
			this.Controls.Add(this.pnlButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TransConvertersDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.pnlButtons.ResumeLayout(false);
			this.pnlButtons.PerformLayout();
			this.cmnuConverters.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblHelp;
		private System.Windows.Forms.ContextMenuStrip cmnuConverters;
		private System.Windows.Forms.ToolStripMenuItem tsmiNone;
		private System.Windows.Forms.ToolStripMenuItem tsmiBrowse;
		private System.Windows.Forms.ToolStripSeparator tsSeparator;
		protected System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.CheckBox chkMakeBackup;
		protected System.Windows.Forms.Button btnCancel;
	}
}