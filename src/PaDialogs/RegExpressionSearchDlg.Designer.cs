namespace SIL.Pa.Dialogs
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tbShowResults = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tbInsert = new System.Windows.Forms.ToolStripDropDownButton();
			this.tbSearchOptions = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tbClear = new System.Windows.Forms.ToolStripButton();
			this.txtSrchItem = new System.Windows.Forms.TextBox();
			this.txtEnvAfter = new System.Windows.Forms.TextBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.txtEnvBefore = new System.Windows.Forms.TextBox();
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.splitInner = new System.Windows.Forms.SplitContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pnlEvnBefore = new SIL.Pa.Controls.PaTextPanel();
			this.pnlSrchItem = new SIL.Pa.Controls.PaTextPanel();
			this.pnlEnvAfter = new SIL.Pa.Controls.PaTextPanel();
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
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(363, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tbShowResults
			// 
			this.tbShowResults.Image = global::SIL.Pa.Dialogs.Properties.Resources.kimidShowResults;
			this.tbShowResults.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbShowResults.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
			this.tbShowResults.Name = "tbShowResults";
			this.tbShowResults.Size = new System.Drawing.Size(106, 22);
			this.tbShowResults.Text = "&Show Results";
			this.tbShowResults.ToolTipText = "Show Search Results";
			this.tbShowResults.Click += new System.EventHandler(this.tbShowResults_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tbInsert
			// 
			this.tbInsert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbInsert.Image = global::SIL.Pa.Dialogs.Properties.Resources.kimidInsertElementIntoPattern;
			this.tbInsert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbInsert.Name = "tbInsert";
			this.tbInsert.Size = new System.Drawing.Size(29, 22);
			this.tbInsert.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.tbInsert.ToolTipText = "Insert Element";
			// 
			// tbSearchOptions
			// 
			this.tbSearchOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbSearchOptions.Image = global::SIL.Pa.Dialogs.Properties.Resources.kimidSearchOptions;
			this.tbSearchOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbSearchOptions.Name = "tbSearchOptions";
			this.tbSearchOptions.Size = new System.Drawing.Size(29, 22);
			this.tbSearchOptions.ToolTipText = "Search Options";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tbClear
			// 
			this.tbClear.Image = global::SIL.Pa.Dialogs.Properties.Resources.kimidClearPattern;
			this.tbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbClear.Name = "tbClear";
			this.tbClear.Size = new System.Drawing.Size(56, 22);
			this.tbClear.Text = "&Clear";
			this.tbClear.ToolTipText = "Clear Patterns";
			this.tbClear.Click += new System.EventHandler(this.tbClear_Click);
			// 
			// txtSrchItem
			// 
			this.txtSrchItem.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSrchItem.Location = new System.Drawing.Point(0, 26);
			this.txtSrchItem.Multiline = true;
			this.txtSrchItem.Name = "txtSrchItem";
			this.txtSrchItem.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSrchItem.Size = new System.Drawing.Size(349, 66);
			this.txtSrchItem.TabIndex = 4;
			// 
			// txtEnvAfter
			// 
			this.txtEnvAfter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtEnvAfter.Location = new System.Drawing.Point(0, 26);
			this.txtEnvAfter.Multiline = true;
			this.txtEnvAfter.Name = "txtEnvAfter";
			this.txtEnvAfter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEnvAfter.Size = new System.Drawing.Size(349, 66);
			this.txtEnvAfter.TabIndex = 6;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(276, 325);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(80, 26);
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// txtEnvBefore
			// 
			this.txtEnvBefore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtEnvBefore.Location = new System.Drawing.Point(0, 26);
			this.txtEnvBefore.Multiline = true;
			this.txtEnvBefore.Name = "txtEnvBefore";
			this.txtEnvBefore.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEnvBefore.Size = new System.Drawing.Size(349, 66);
			this.txtEnvBefore.TabIndex = 2;
			// 
			// splitOuter
			// 
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.IsSplitterFixed = true;
			this.splitOuter.Location = new System.Drawing.Point(7, 6);
			this.splitOuter.Name = "splitOuter";
			this.splitOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.txtEnvBefore);
			this.splitOuter.Panel1.Controls.Add(this.pnlEvnBefore);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.splitInner);
			this.splitOuter.Size = new System.Drawing.Size(349, 284);
			this.splitOuter.SplitterDistance = 92;
			this.splitOuter.TabIndex = 9;
			// 
			// splitInner
			// 
			this.splitInner.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitInner.IsSplitterFixed = true;
			this.splitInner.Location = new System.Drawing.Point(0, 0);
			this.splitInner.Name = "splitInner";
			this.splitInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
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
			this.splitInner.Size = new System.Drawing.Size(349, 188);
			this.splitInner.SplitterDistance = 92;
			this.splitInner.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitOuter);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 25);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 10);
			this.panel1.Size = new System.Drawing.Size(363, 300);
			this.panel1.TabIndex = 10;
			// 
			// pnlEvnBefore
			// 
			this.pnlEvnBefore.BackColor = System.Drawing.Color.Transparent;
			this.pnlEvnBefore.ControlReceivingFocusOnMnemonic = this.pnlEvnBefore;
			this.pnlEvnBefore.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlEvnBefore.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlEvnBefore.Location = new System.Drawing.Point(0, 0);
			this.pnlEvnBefore.MnemonicGeneratesClick = false;
			this.pnlEvnBefore.Name = "pnlEvnBefore";
			this.pnlEvnBefore.Size = new System.Drawing.Size(349, 26);
			this.pnlEvnBefore.TabIndex = 3;
			this.pnlEvnBefore.Text = "Environment &Before";
			// 
			// pnlSrchItem
			// 
			this.pnlSrchItem.BackColor = System.Drawing.Color.Transparent;
			this.pnlSrchItem.ControlReceivingFocusOnMnemonic = this.txtSrchItem;
			this.pnlSrchItem.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSrchItem.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlSrchItem.Location = new System.Drawing.Point(0, 0);
			this.pnlSrchItem.MnemonicGeneratesClick = false;
			this.pnlSrchItem.Name = "pnlSrchItem";
			this.pnlSrchItem.Size = new System.Drawing.Size(349, 26);
			this.pnlSrchItem.TabIndex = 4;
			this.pnlSrchItem.Text = "Search &Item";
			// 
			// pnlEnvAfter
			// 
			this.pnlEnvAfter.BackColor = System.Drawing.Color.Transparent;
			this.pnlEnvAfter.ControlReceivingFocusOnMnemonic = this.txtEnvAfter;
			this.pnlEnvAfter.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlEnvAfter.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlEnvAfter.Location = new System.Drawing.Point(0, 0);
			this.pnlEnvAfter.MnemonicGeneratesClick = false;
			this.pnlEnvAfter.Name = "pnlEnvAfter";
			this.pnlEnvAfter.Size = new System.Drawing.Size(349, 26);
			this.pnlEnvAfter.TabIndex = 4;
			this.pnlEnvAfter.Text = "Environment &After";
			// 
			// RegExpressionSearchDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(363, 360);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "RegExpressionSearchDlg";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 35);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Regular Expression Search";
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
	}
}