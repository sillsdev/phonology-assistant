namespace SIL.Pa.Controls
{
	partial class PhonesInFeatureViewer
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
			this.components = new System.ComponentModel.Container();
			this.cmnuViewOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCompact = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExpanded = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlOuter = new SIL.Pa.Controls.PaPanel();
			this.pnlExpandedView = new System.Windows.Forms.Panel();
			this.header = new SIL.Pa.Controls.HeaderLabel();
			this.btnDropDownArrow = new SIL.Pa.Controls.XButton();
			this.cmnuViewOptions.SuspendLayout();
			this.pnlOuter.SuspendLayout();
			this.header.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmnuViewOptions
			// 
			this.cmnuViewOptions.AutoSize = false;
			this.cmnuViewOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowAll,
            this.mnuSep,
            this.mnuCompact,
            this.mnuExpanded});
			this.cmnuViewOptions.Name = "contextMenuStrip1";
			this.cmnuViewOptions.Size = new System.Drawing.Size(167, 98);
			// 
			// mnuShowAll
			// 
			this.mnuShowAll.Name = "mnuShowAll";
			this.mnuShowAll.Size = new System.Drawing.Size(166, 22);
			this.mnuShowAll.Text = "&Show All Phones";
			this.mnuShowAll.ToolTipText = "Show phones, even when they don\'t occur in the selected feature(s)";
			this.mnuShowAll.Click += new System.EventHandler(this.mnuShowAll_Click);
			// 
			// mnuSep
			// 
			this.mnuSep.Name = "mnuSep";
			this.mnuSep.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuCompact
			// 
			this.mnuCompact.Name = "mnuCompact";
			this.mnuCompact.Size = new System.Drawing.Size(166, 22);
			this.mnuCompact.Text = "&Compact View";
			this.mnuCompact.ToolTipText = "Arranges without new rows for each manner of articulation";
			this.mnuCompact.Click += new System.EventHandler(this.mnuCompact_Click);
			// 
			// mnuExpanded
			// 
			this.mnuExpanded.Name = "mnuExpanded";
			this.mnuExpanded.Size = new System.Drawing.Size(166, 22);
			this.mnuExpanded.Text = "&Expanded View";
			this.mnuExpanded.ToolTipText = "Arranges with new rows for each manner of articulation";
			this.mnuExpanded.Click += new System.EventHandler(this.mnuExpanded_Click);
			// 
			// pnlOuter
			// 
			this.pnlOuter.BackColor = System.Drawing.SystemColors.Window;
			this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlOuter.Controls.Add(this.pnlExpandedView);
			this.pnlOuter.Controls.Add(this.header);
			this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlOuter.DoubleBuffered = true;
			this.pnlOuter.Location = new System.Drawing.Point(0, 0);
			this.pnlOuter.Name = "pnlOuter";
			this.pnlOuter.PaintExplorerBarBackground = false;
			this.pnlOuter.Size = new System.Drawing.Size(150, 150);
			this.pnlOuter.TabIndex = 10;
			// 
			// pnlExpandedView
			// 
			this.pnlExpandedView.AutoScroll = true;
			this.pnlExpandedView.Location = new System.Drawing.Point(23, 30);
			this.pnlExpandedView.Name = "pnlExpandedView";
			this.pnlExpandedView.Size = new System.Drawing.Size(96, 41);
			this.pnlExpandedView.TabIndex = 0;
			// 
			// header
			// 
			this.header.Controls.Add(this.btnDropDownArrow);
			this.header.Dock = System.Windows.Forms.DockStyle.Top;
			this.header.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.header.Location = new System.Drawing.Point(0, 0);
			this.header.MnemonicGeneratesClick = true;
			this.header.Name = "header";
			this.header.Size = new System.Drawing.Size(148, 24);
			this.header.TabIndex = 12;
			this.header.Text = "#";
			this.header.Click += new System.EventHandler(this.header_Click);
			// 
			// btnDropDownArrow
			// 
			this.btnDropDownArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDropDownArrow.BackColor = System.Drawing.Color.Transparent;
			this.btnDropDownArrow.CanBeChecked = false;
			this.btnDropDownArrow.Checked = false;
			this.btnDropDownArrow.DrawEmpty = false;
			this.btnDropDownArrow.DrawLeftArrowButton = false;
			this.btnDropDownArrow.DrawRightArrowButton = false;
			this.btnDropDownArrow.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnDropDownArrow.Image = null;
			this.btnDropDownArrow.Location = new System.Drawing.Point(128, 4);
			this.btnDropDownArrow.Name = "btnDropDownArrow";
			this.btnDropDownArrow.Size = new System.Drawing.Size(16, 16);
			this.btnDropDownArrow.TabIndex = 1;
			this.btnDropDownArrow.Text = "6";
			this.btnDropDownArrow.Click += new System.EventHandler(this.btnDropDownArrow_Click);
			// 
			// PhonesInFeatureViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.pnlOuter);
			this.DoubleBuffered = true;
			this.Name = "PhonesInFeatureViewer";
			this.cmnuViewOptions.ResumeLayout(false);
			this.pnlOuter.ResumeLayout(false);
			this.header.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private PaPanel pnlOuter;
		private HeaderLabel header;
		private XButton btnDropDownArrow;
		private System.Windows.Forms.ContextMenuStrip cmnuViewOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuShowAll;
		private System.Windows.Forms.ToolStripSeparator mnuSep;
		private System.Windows.Forms.ToolStripMenuItem mnuCompact;
		private System.Windows.Forms.ToolStripMenuItem mnuExpanded;
		private System.Windows.Forms.Panel pnlExpandedView;
	}
}
