namespace SIL.Pa.AddOn
{
	partial class RegExpressionSearchDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegExpressionSearchDlg));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tbShowResults = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tbInsert = new System.Windows.Forms.ToolStripDropDownButton();
			this.matchOnZeroOrMoreCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oneOrMoreCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wordBoundaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.consonantPhoneGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vowelPhoneGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPhonesInAFeature = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPhonesInBFeature = new System.Windows.Forms.ToolStripMenuItem();
			this.tbSearchOptions = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tbClear = new System.Windows.Forms.ToolStripButton();
			this.txtSrchItem = new System.Windows.Forms.TextBox();
			this.txtEnvAfter = new System.Windows.Forms.TextBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.txtEnvBefore = new System.Windows.Forms.TextBox();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.pnlEvnBefore = new SIL.Pa.Controls.PaTextPanel();
			this.splitInner = new System.Windows.Forms.SplitContainer();
			this.pnlSrchItem = new SIL.Pa.Controls.PaTextPanel();
			this.pnlEnvAfter = new SIL.Pa.Controls.PaTextPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.toolStrip1.SuspendLayout();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitInner.Panel1.SuspendLayout();
			this.splitInner.Panel2.SuspendLayout();
			this.splitInner.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbShowResults,
            this.toolStripSeparator1,
            this.tbInsert,
            this.tbSearchOptions,
            this.toolStripSeparator2,
            this.tbClear});
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Name = "toolStrip1";
			// 
			// tbShowResults
			// 
			this.tbShowResults.Image = global::SIL.Pa.AddOn.Properties.Resources.kimidShowResults;
			resources.ApplyResources(this.tbShowResults, "tbShowResults");
			this.tbShowResults.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
			this.tbShowResults.Name = "tbShowResults";
			this.tbShowResults.Click += new System.EventHandler(this.tbShowResults_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// tbInsert
			// 
			this.tbInsert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbInsert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.matchOnZeroOrMoreCharactersToolStripMenuItem,
            this.oneOrMoreCharactersToolStripMenuItem,
            this.wordBoundaryToolStripMenuItem,
            this.toolStripMenuItem1,
            this.consonantPhoneGroupToolStripMenuItem,
            this.vowelPhoneGroupToolStripMenuItem,
            this.mnuPhonesInAFeature,
            this.mnuPhonesInBFeature});
			this.tbInsert.Image = global::SIL.Pa.AddOn.Properties.Resources.kimidInsertElementIntoPattern;
			resources.ApplyResources(this.tbInsert, "tbInsert");
			this.tbInsert.Name = "tbInsert";
			// 
			// matchOnZeroOrMoreCharactersToolStripMenuItem
			// 
			this.matchOnZeroOrMoreCharactersToolStripMenuItem.Name = "matchOnZeroOrMoreCharactersToolStripMenuItem";
			resources.ApplyResources(this.matchOnZeroOrMoreCharactersToolStripMenuItem, "matchOnZeroOrMoreCharactersToolStripMenuItem");
			this.matchOnZeroOrMoreCharactersToolStripMenuItem.Click += new System.EventHandler(this.matchOnZeroOrMoreCharactersToolStripMenuItem_Click);
			// 
			// oneOrMoreCharactersToolStripMenuItem
			// 
			this.oneOrMoreCharactersToolStripMenuItem.Name = "oneOrMoreCharactersToolStripMenuItem";
			resources.ApplyResources(this.oneOrMoreCharactersToolStripMenuItem, "oneOrMoreCharactersToolStripMenuItem");
			this.oneOrMoreCharactersToolStripMenuItem.Click += new System.EventHandler(this.oneOrMoreCharactersToolStripMenuItem_Click);
			// 
			// wordBoundaryToolStripMenuItem
			// 
			this.wordBoundaryToolStripMenuItem.Name = "wordBoundaryToolStripMenuItem";
			resources.ApplyResources(this.wordBoundaryToolStripMenuItem, "wordBoundaryToolStripMenuItem");
			this.wordBoundaryToolStripMenuItem.Click += new System.EventHandler(this.wordBoundaryToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			// 
			// consonantPhoneGroupToolStripMenuItem
			// 
			this.consonantPhoneGroupToolStripMenuItem.Name = "consonantPhoneGroupToolStripMenuItem";
			resources.ApplyResources(this.consonantPhoneGroupToolStripMenuItem, "consonantPhoneGroupToolStripMenuItem");
			this.consonantPhoneGroupToolStripMenuItem.Click += new System.EventHandler(this.consonantPhoneGroupToolStripMenuItem_Click);
			// 
			// vowelPhoneGroupToolStripMenuItem
			// 
			this.vowelPhoneGroupToolStripMenuItem.Name = "vowelPhoneGroupToolStripMenuItem";
			resources.ApplyResources(this.vowelPhoneGroupToolStripMenuItem, "vowelPhoneGroupToolStripMenuItem");
			this.vowelPhoneGroupToolStripMenuItem.Click += new System.EventHandler(this.vowelPhoneGroupToolStripMenuItem_Click);
			// 
			// mnuPhonesInAFeature
			// 
			this.mnuPhonesInAFeature.Name = "mnuPhonesInAFeature";
			resources.ApplyResources(this.mnuPhonesInAFeature, "mnuPhonesInAFeature");
			// 
			// mnuPhonesInBFeature
			// 
			this.mnuPhonesInBFeature.Name = "mnuPhonesInBFeature";
			resources.ApplyResources(this.mnuPhonesInBFeature, "mnuPhonesInBFeature");
			// 
			// tbSearchOptions
			// 
			this.tbSearchOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbSearchOptions.Image = global::SIL.Pa.AddOn.Properties.Resources.kimidSearchOptions;
			resources.ApplyResources(this.tbSearchOptions, "tbSearchOptions");
			this.tbSearchOptions.Name = "tbSearchOptions";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// tbClear
			// 
			this.tbClear.Image = global::SIL.Pa.AddOn.Properties.Resources.kimidClearPattern;
			resources.ApplyResources(this.tbClear, "tbClear");
			this.tbClear.Name = "tbClear";
			this.tbClear.Click += new System.EventHandler(this.tbClear_Click);
			// 
			// txtSrchItem
			// 
			resources.ApplyResources(this.txtSrchItem, "txtSrchItem");
			this.txtSrchItem.Name = "txtSrchItem";
			this.txtSrchItem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleTextBoxKeyPress);
			// 
			// txtEnvAfter
			// 
			resources.ApplyResources(this.txtEnvAfter, "txtEnvAfter");
			this.txtEnvAfter.Name = "txtEnvAfter";
			this.txtEnvAfter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleTextBoxKeyPress);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// txtEnvBefore
			// 
			resources.ApplyResources(this.txtEnvBefore, "txtEnvBefore");
			this.txtEnvBefore.Name = "txtEnvBefore";
			this.txtEnvBefore.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleTextBoxKeyPress);
			// 
			// splitOuter
			// 
			resources.ApplyResources(this.splitOuter, "splitOuter");
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.txtEnvBefore);
			this.splitOuter.Panel1.Controls.Add(this.pnlEvnBefore);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.splitInner);
			// 
			// pnlEvnBefore
			// 
			this.pnlEvnBefore.BackColor = System.Drawing.Color.Transparent;
			this.pnlEvnBefore.ControlReceivingFocusOnMnemonic = this.pnlEvnBefore;
			resources.ApplyResources(this.pnlEvnBefore, "pnlEvnBefore");
			this.pnlEvnBefore.MnemonicGeneratesClick = false;
			this.pnlEvnBefore.Name = "pnlEvnBefore";
			// 
			// splitInner
			// 
			resources.ApplyResources(this.splitInner, "splitInner");
			this.splitInner.Name = "splitInner";
			// 
			// splitInner.Panel1
			// 
			this.splitInner.Panel1.Controls.Add(this.txtSrchItem);
			this.splitInner.Panel1.Controls.Add(this.pnlSrchItem);
			// 
			// splitInner.Panel2
			// 
			this.splitInner.Panel2.Controls.Add(this.txtEnvAfter);
			this.splitInner.Panel2.Controls.Add(this.pnlEnvAfter);
			// 
			// pnlSrchItem
			// 
			this.pnlSrchItem.BackColor = System.Drawing.Color.Transparent;
			this.pnlSrchItem.ControlReceivingFocusOnMnemonic = this.txtSrchItem;
			resources.ApplyResources(this.pnlSrchItem, "pnlSrchItem");
			this.pnlSrchItem.MnemonicGeneratesClick = false;
			this.pnlSrchItem.Name = "pnlSrchItem";
			// 
			// pnlEnvAfter
			// 
			this.pnlEnvAfter.BackColor = System.Drawing.Color.Transparent;
			this.pnlEnvAfter.ControlReceivingFocusOnMnemonic = this.txtEnvAfter;
			resources.ApplyResources(this.pnlEnvAfter, "pnlEnvAfter");
			this.pnlEnvAfter.MnemonicGeneratesClick = false;
			this.pnlEnvAfter.Name = "pnlEnvAfter";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitOuter);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// RegExpressionSearchDlg
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "RegExpressionSearchDlg";
			this.ShowIcon = false;
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel1.PerformLayout();
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitInner.Panel1.ResumeLayout(false);
			this.splitInner.Panel1.PerformLayout();
			this.splitInner.Panel2.ResumeLayout(false);
			this.splitInner.Panel2.PerformLayout();
			this.splitInner.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tbShowResults;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripDropDownButton tbInsert;
		private System.Windows.Forms.ToolStripDropDownButton tbSearchOptions;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tbClear;
		private System.Windows.Forms.TextBox txtSrchItem;
		private System.Windows.Forms.TextBox txtEnvAfter;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox txtEnvBefore;
		private System.Windows.Forms.SplitContainer splitOuter;
		private System.Windows.Forms.SplitContainer splitInner;
		private SIL.Pa.Controls.PaTextPanel pnlEvnBefore;
		private SIL.Pa.Controls.PaTextPanel pnlSrchItem;
		private SIL.Pa.Controls.PaTextPanel pnlEnvAfter;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem matchOnZeroOrMoreCharactersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem oneOrMoreCharactersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem wordBoundaryToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem consonantPhoneGroupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem vowelPhoneGroupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuPhonesInAFeature;
		private System.Windows.Forms.ToolStripMenuItem mnuPhonesInBFeature;
	}
}