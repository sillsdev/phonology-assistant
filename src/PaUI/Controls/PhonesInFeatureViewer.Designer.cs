using SilUtils.Controls;

namespace SIL.Pa.UI.Controls
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhonesInFeatureViewer));
			this.cmnuViewOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuShowAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSep = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCompact = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExpanded = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlOuter = new SilPanel();
			this.pnlExpandedView = new System.Windows.Forms.Panel();
			this.header = new SIL.Pa.UI.Controls.HeaderLabel();
			this.btnDropDownArrow = new SIL.Pa.UI.Controls.XButton();
			this.cmnuViewOptions.SuspendLayout();
			this.pnlOuter.SuspendLayout();
			this.header.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmnuViewOptions
			// 
			resources.ApplyResources(this.cmnuViewOptions, "cmnuViewOptions");
			this.cmnuViewOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowAll,
            this.mnuSep,
            this.mnuCompact,
            this.mnuExpanded});
			this.cmnuViewOptions.Name = "contextMenuStrip1";
			// 
			// mnuShowAll
			// 
			this.mnuShowAll.Name = "mnuShowAll";
			resources.ApplyResources(this.mnuShowAll, "mnuShowAll");
			this.mnuShowAll.Click += new System.EventHandler(this.mnuShowAll_Click);
			// 
			// mnuSep
			// 
			this.mnuSep.Name = "mnuSep";
			resources.ApplyResources(this.mnuSep, "mnuSep");
			// 
			// mnuCompact
			// 
			this.mnuCompact.Name = "mnuCompact";
			resources.ApplyResources(this.mnuCompact, "mnuCompact");
			this.mnuCompact.Click += new System.EventHandler(this.mnuCompact_Click);
			// 
			// mnuExpanded
			// 
			this.mnuExpanded.Name = "mnuExpanded";
			resources.ApplyResources(this.mnuExpanded, "mnuExpanded");
			this.mnuExpanded.Click += new System.EventHandler(this.mnuExpanded_Click);
			// 
			// pnlOuter
			// 
			this.pnlOuter.BackColor = System.Drawing.SystemColors.Window;
			this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlOuter.ClipTextForChildControls = true;
			this.pnlOuter.ControlReceivingFocusOnMnemonic = null;
			this.pnlOuter.Controls.Add(this.pnlExpandedView);
			this.pnlOuter.Controls.Add(this.header);
			resources.ApplyResources(this.pnlOuter, "pnlOuter");
			this.pnlOuter.DoubleBuffered = true;
			this.pnlOuter.MnemonicGeneratesClick = false;
			this.pnlOuter.Name = "pnlOuter";
			this.pnlOuter.PaintExplorerBarBackground = false;
			// 
			// pnlExpandedView
			// 
			resources.ApplyResources(this.pnlExpandedView, "pnlExpandedView");
			this.pnlExpandedView.Name = "pnlExpandedView";
			// 
			// header
			// 
			this.header.ClipTextForChildControls = true;
			this.header.ControlReceivingFocusOnMnemonic = null;
			this.header.Controls.Add(this.btnDropDownArrow);
			resources.ApplyResources(this.header, "header");
			this.header.MnemonicGeneratesClick = true;
			this.header.Name = "header";
			this.header.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.header.Click += new System.EventHandler(this.header_Click);
			// 
			// btnDropDownArrow
			// 
			resources.ApplyResources(this.btnDropDownArrow, "btnDropDownArrow");
			this.btnDropDownArrow.BackColor = System.Drawing.Color.Transparent;
			this.btnDropDownArrow.CanBeChecked = false;
			this.btnDropDownArrow.Checked = false;
			this.btnDropDownArrow.DrawEmpty = false;
			this.btnDropDownArrow.DrawLeftArrowButton = false;
			this.btnDropDownArrow.DrawRightArrowButton = false;
			this.btnDropDownArrow.Image = null;
			this.btnDropDownArrow.Name = "btnDropDownArrow";
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

		private SilPanel pnlOuter;
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
