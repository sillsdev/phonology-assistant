namespace SIL.Pa.UI.Views
{
	partial class ChartVwBase
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
			this._splitOuter = new System.Windows.Forms.SplitContainer();
			this._pnlGrid = new SilTools.Controls.SilPanel();
			this._histogram = new SIL.Pa.UI.Controls.Histogram();
			this._toopTip = new System.Windows.Forms.ToolTip(this.components);
			this._splitOuter.Panel1.SuspendLayout();
			this._splitOuter.Panel2.SuspendLayout();
			this._splitOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// _splitOuter
			// 
			this._splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitOuter.Location = new System.Drawing.Point(0, 0);
			this._splitOuter.Name = "_splitOuter";
			this._splitOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// _splitOuter.Panel1
			// 
			this._splitOuter.Panel1.Controls.Add(this._pnlGrid);
			this._splitOuter.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this._splitOuter.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// _splitOuter.Panel2
			// 
			this._splitOuter.Panel2.Controls.Add(this._histogram);
			this._splitOuter.Panel2.Padding = new System.Windows.Forms.Padding(10, 1, 10, 10);
			this._splitOuter.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._splitOuter.Size = new System.Drawing.Size(630, 606);
			this._splitOuter.SplitterDistance = 351;
			this._splitOuter.SplitterWidth = 8;
			this._splitOuter.TabIndex = 4;
			// 
			// _pnlGrid
			// 
			this._pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._pnlGrid.ClipTextForChildControls = true;
			this._pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this._pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pnlGrid.DoubleBuffered = true;
			this._pnlGrid.DrawOnlyBottomBorder = false;
			this._pnlGrid.DrawOnlyTopBorder = false;
			this._pnlGrid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._pnlGrid.ForeColor = System.Drawing.SystemColors.ControlText;
			this._pnlGrid.Location = new System.Drawing.Point(10, 10);
			this._pnlGrid.MnemonicGeneratesClick = false;
			this._pnlGrid.Name = "_pnlGrid";
			this._pnlGrid.PaintExplorerBarBackground = false;
			this._pnlGrid.Size = new System.Drawing.Size(610, 341);
			this._pnlGrid.TabIndex = 0;
			// 
			// _histogram
			// 
			this._histogram.Dock = System.Windows.Forms.DockStyle.Fill;
			this._histogram.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._histogram.Location = new System.Drawing.Point(10, 1);
			this._histogram.Name = "_histogram";
			this._histogram.Size = new System.Drawing.Size(610, 236);
			this._histogram.TabIndex = 0;
			// 
			// ChartVwBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._splitOuter);
			this.Name = "ChartVwBase";
			this.Size = new System.Drawing.Size(630, 606);
			this._splitOuter.Panel1.ResumeLayout(false);
			this._splitOuter.Panel2.ResumeLayout(false);
			this._splitOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip _toopTip;
		private System.Windows.Forms.SplitContainer _splitOuter;
		private SIL.Pa.UI.Controls.Histogram _histogram;
		private SilTools.Controls.SilPanel _pnlGrid;

	}
}