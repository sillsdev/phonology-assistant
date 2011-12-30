using SilTools.Controls;

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
			this.cmnuViewOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuCompact = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExpanded = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlOuter = new SilTools.Controls.SilPanel();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._flowLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.header = new SilTools.Controls.HeaderLabel();
			this.btnDropDownArrow = new SilTools.Controls.XButton();
			this.cmnuViewOptions.SuspendLayout();
			this.pnlOuter.SuspendLayout();
			this.header.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmnuViewOptions
			// 
			this.cmnuViewOptions.AutoSize = false;
			this.cmnuViewOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCompact,
            this.mnuExpanded});
			this.cmnuViewOptions.Name = "contextMenuStrip1";
			this.cmnuViewOptions.Size = new System.Drawing.Size(167, 98);
			// 
			// mnuCompact
			// 
			this.mnuCompact.Name = "mnuCompact";
			this.mnuCompact.Size = new System.Drawing.Size(153, 22);
			this.mnuCompact.Text = "&Compact View";
			this.mnuCompact.ToolTipText = "Arranges without new rows for each manner of articulation";
			this.mnuCompact.Click += new System.EventHandler(this.mnuCompact_Click);
			// 
			// mnuExpanded
			// 
			this.mnuExpanded.Name = "mnuExpanded";
			this.mnuExpanded.Size = new System.Drawing.Size(153, 22);
			this.mnuExpanded.Text = "&Expanded View";
			this.mnuExpanded.ToolTipText = "Arranges with new rows for each manner of articulation";
			this.mnuExpanded.Click += new System.EventHandler(this.mnuExpanded_Click);
			// 
			// pnlOuter
			// 
			this.pnlOuter.BackColor = System.Drawing.SystemColors.Window;
			this.pnlOuter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlOuter.ClipTextForChildControls = true;
			this.pnlOuter.ControlReceivingFocusOnMnemonic = null;
			this.pnlOuter.Controls.Add(this._tableLayout);
			this.pnlOuter.Controls.Add(this._flowLayout);
			this.pnlOuter.Controls.Add(this.header);
			this.pnlOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlOuter.DoubleBuffered = true;
			this.pnlOuter.DrawOnlyBottomBorder = false;
			this.pnlOuter.DrawOnlyTopBorder = false;
			this.pnlOuter.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlOuter.ForeColor = System.Drawing.SystemColors.ControlText;
			this.pnlOuter.Location = new System.Drawing.Point(0, 0);
			this.pnlOuter.MnemonicGeneratesClick = false;
			this.pnlOuter.Name = "pnlOuter";
			this.pnlOuter.PaintExplorerBarBackground = false;
			this.pnlOuter.Size = new System.Drawing.Size(245, 231);
			this.pnlOuter.TabIndex = 10;
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoScroll = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayout.Location = new System.Drawing.Point(47, 43);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayout.Size = new System.Drawing.Size(141, 88);
			this._tableLayout.TabIndex = 14;
			// 
			// _flowLayout
			// 
			this._flowLayout.AutoScroll = true;
			this._flowLayout.BackColor = System.Drawing.Color.Transparent;
			this._flowLayout.Location = new System.Drawing.Point(24, 151);
			this._flowLayout.Name = "_flowLayout";
			this._flowLayout.Size = new System.Drawing.Size(138, 66);
			this._flowLayout.TabIndex = 13;
			// 
			// header
			// 
			this.header.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.header.ClipTextForChildControls = true;
			this.header.ControlReceivingFocusOnMnemonic = null;
			this.header.Controls.Add(this.btnDropDownArrow);
			this.header.Dock = System.Windows.Forms.DockStyle.Top;
			this.header.DoubleBuffered = true;
			this.header.DrawOnlyBottomBorder = false;
			this.header.DrawOnlyTopBorder = false;
			this.header.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.header.ForeColor = System.Drawing.SystemColors.ControlText;
			this.header.Location = new System.Drawing.Point(0, 0);
			this.header.MnemonicGeneratesClick = true;
			this.header.Name = "header";
			this.header.PaintExplorerBarBackground = false;
			this.header.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.header.Size = new System.Drawing.Size(243, 24);
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
			this.btnDropDownArrow.Location = new System.Drawing.Point(223, 4);
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
			this.Size = new System.Drawing.Size(245, 231);
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
		private System.Windows.Forms.ToolStripMenuItem mnuCompact;
		private System.Windows.Forms.ToolStripMenuItem mnuExpanded;
		private System.Windows.Forms.FlowLayoutPanel _flowLayout;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
	}
}
