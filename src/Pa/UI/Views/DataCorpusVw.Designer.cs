using SilTools.Controls;

namespace SIL.Pa.UI.Views
{
	partial class DataCorpusVw
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
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.pnlGrid = new SilTools.Controls.SilPanel();
			this._recView = new SIL.Pa.UI.Controls.RecordViewControls.RecordViewPanel();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.Location = new System.Drawing.Point(0, 0);
			this.splitOuter.Name = "splitOuter";
			this.splitOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitOuter.Panel1.Controls.Add(this.pnlGrid);
			this.splitOuter.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this._recView);
			this.splitOuter.Panel2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
			this.splitOuter.Panel2MinSize = 0;
			this.splitOuter.Size = new System.Drawing.Size(676, 468);
			this.splitOuter.SplitterDistance = 371;
			this.splitOuter.SplitterWidth = 8;
			this.splitOuter.TabIndex = 0;
			this.splitOuter.TabStop = false;
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.DoubleBuffered = true;
			this.pnlGrid.DrawOnlyBottomBorder = false;
			this.pnlGrid.DrawOnlyTopBorder = false;
			this.pnlGrid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlGrid.ForeColor = System.Drawing.SystemColors.ControlText;
			this.pnlGrid.Location = new System.Drawing.Point(10, 10);
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.pnlGrid.Size = new System.Drawing.Size(656, 361);
			this.pnlGrid.TabIndex = 0;
			// 
			// _recView
			// 
			this._recView.BackColor = System.Drawing.SystemColors.Window;
			this._recView.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._recView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._recView.ClipTextForChildControls = true;
			this._recView.ControlReceivingFocusOnMnemonic = null;
			this._recView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._recView.DoubleBuffered = true;
			this._recView.DrawOnlyBottomBorder = false;
			this._recView.DrawOnlyTopBorder = false;
			this._recView.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._recView.ForeColor = System.Drawing.SystemColors.ControlText;
			this._recView.Location = new System.Drawing.Point(10, 0);
			this._recView.MnemonicGeneratesClick = false;
			this._recView.Name = "_recView";
			this._recView.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
			this._recView.PaintExplorerBarBackground = false;
			this._recView.Size = new System.Drawing.Size(656, 79);
			this._recView.TabIndex = 0;
			// 
			// DataCorpusVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitOuter);
			this.Name = "DataCorpusVw";
			this.Size = new System.Drawing.Size(676, 468);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitOuter;
		private SilPanel pnlGrid;
		private Controls.RecordViewControls.RecordViewPanel _recView;
	}
}