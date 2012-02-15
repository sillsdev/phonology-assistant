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
			this.paPanel1 = new SilTools.Controls.SilPanel();
			this._htmlRecView = new SIL.Pa.UI.Controls.HtmlRecordView();
			this._rtfRecVw = new SIL.Pa.UI.Controls.RtfRecordView();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.paPanel1.SuspendLayout();
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
			this.splitOuter.Panel2.Controls.Add(this.paPanel1);
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
			// paPanel1
			// 
			this.paPanel1.BackColor = System.Drawing.SystemColors.Window;
			this.paPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.paPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.paPanel1.ClipTextForChildControls = true;
			this.paPanel1.ControlReceivingFocusOnMnemonic = null;
			this.paPanel1.Controls.Add(this._htmlRecView);
			this.paPanel1.Controls.Add(this._rtfRecVw);
			this.paPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.paPanel1.DoubleBuffered = false;
			this.paPanel1.DrawOnlyBottomBorder = false;
			this.paPanel1.DrawOnlyTopBorder = false;
			this.paPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.paPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.paPanel1.Location = new System.Drawing.Point(10, 0);
			this.paPanel1.MnemonicGeneratesClick = false;
			this.paPanel1.Name = "paPanel1";
			this.paPanel1.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
			this.paPanel1.PaintExplorerBarBackground = false;
			this.paPanel1.Size = new System.Drawing.Size(656, 79);
			this.paPanel1.TabIndex = 0;
			// 
			// _htmlRecView
			// 
			this._htmlRecView.AllowWebBrowserDrop = false;
			this._htmlRecView.Location = new System.Drawing.Point(322, 7);
			this._htmlRecView.MinimumSize = new System.Drawing.Size(20, 20);
			this._htmlRecView.Name = "_htmlRecView";
			this._htmlRecView.Size = new System.Drawing.Size(329, 67);
			this._htmlRecView.TabIndex = 1;
			// 
			// _rtfRecVw
			// 
			this._rtfRecVw.BackColor = System.Drawing.SystemColors.Window;
			this._rtfRecVw.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._rtfRecVw.Location = new System.Drawing.Point(4, 4);
			this._rtfRecVw.Name = "_rtfRecVw";
			this._rtfRecVw.ReadOnly = true;
			this._rtfRecVw.Size = new System.Drawing.Size(260, 73);
			this._rtfRecVw.TabIndex = 0;
			this._rtfRecVw.TabStop = false;
			this._rtfRecVw.Text = global::SIL.Pa.ResourceStuff.PaTMStrings.kstidExportAsToolTip;
			this._rtfRecVw.WordWrap = false;
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
			this.paPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitOuter;
		private SilPanel paPanel1;
		private SIL.Pa.UI.Controls.RtfRecordView _rtfRecVw;
		private SilPanel pnlGrid;
		private Controls.HtmlRecordView _htmlRecView;
	}
}