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
			this._oldChrGrid = new SIL.Pa.UI.Controls.CharGrid();
			this._histogram = new SIL.Pa.UI.Controls.Histogram();
			this._toopTip = new System.Windows.Forms.ToolTip(this.components);
			this._splitOuter.Panel1.SuspendLayout();
			this._splitOuter.Panel2.SuspendLayout();
			this._splitOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			this._splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitOuter.Location = new System.Drawing.Point(0, 0);
			this._splitOuter.Name = "splitOuter";
			this._splitOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitOuter.Panel1
			// 
			this._splitOuter.Panel1.Controls.Add(this._oldChrGrid);
			this._splitOuter.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this._splitOuter.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitOuter.Panel2
			// 
			this._splitOuter.Panel2.Controls.Add(this._histogram);
			this._splitOuter.Panel2.Padding = new System.Windows.Forms.Padding(10, 1, 10, 10);
			this._splitOuter.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._splitOuter.Size = new System.Drawing.Size(630, 606);
			this._splitOuter.SplitterDistance = 351;
			this._splitOuter.SplitterWidth = 8;
			this._splitOuter.TabIndex = 4;
			// 
			// m_chrGrid
			// 
			this._oldChrGrid.CellWidth = 38;
			this._oldChrGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._oldChrGrid.HeadersVisible = true;
			this._oldChrGrid.Location = new System.Drawing.Point(10, 10);
			this._oldChrGrid.Name = "m_chrGrid";
			this._oldChrGrid.SearchWhenPhoneDoubleClicked = true;
			this._oldChrGrid.Size = new System.Drawing.Size(610, 341);
			this._oldChrGrid.TabIndex = 2;
			// 
			// m_histogram
			// 
			this._histogram.Dock = System.Windows.Forms.DockStyle.Fill;
			this._histogram.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._histogram.Location = new System.Drawing.Point(10, 1);
			this._histogram.Name = "m_histogram";
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
		protected SIL.Pa.UI.Controls.CharGrid _oldChrGrid;
		private System.Windows.Forms.SplitContainer _splitOuter;
		private SIL.Pa.UI.Controls.Histogram _histogram;

	}
}