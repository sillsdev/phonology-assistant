using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	partial class CharGrid
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_grid = new SIL.Pa.UI.Controls.CharGridView();
			this.pnlWrapper = new SilPanel();
			this.pnlGrid = new System.Windows.Forms.Panel();
			this.m_vsplitter = new System.Windows.Forms.Splitter();
			this.pnlRowHeaderOuter = new System.Windows.Forms.Panel();
			this.m_hsplitter = new System.Windows.Forms.Splitter();
			this.pnlColHeaderOuter = new System.Windows.Forms.Panel();
			this.pnlCorner = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
			this.pnlWrapper.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			this.pnlColHeaderOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_grid
			// 
			this.m_grid.AllowDrop = true;
			this.m_grid.AllowUserToAddRows = false;
			this.m_grid.AllowUserToDeleteRows = false;
			this.m_grid.AllowUserToResizeColumns = false;
			this.m_grid.AllowUserToResizeRows = false;
			this.m_grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.ColumnHeadersVisible = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_grid.DefaultCellStyle = dataGridViewCellStyle1;
			this.m_grid.EnableHeadersVisualStyles = false;
			this.m_grid.Location = new System.Drawing.Point(0, 0);
			this.m_grid.Name = "m_grid";
			this.m_grid.ReadOnly = true;
			this.m_grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_grid.RowHeadersVisible = false;
			this.m_grid.RowHeadersWidth = 75;
			this.m_grid.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.m_grid.ShowCellToolTips = false;
			this.m_grid.ShowEditingIcon = false;
			this.m_grid.ShowRowErrors = false;
			this.m_grid.Size = new System.Drawing.Size(135, 137);
			this.m_grid.TabIndex = 0;
			this.m_grid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseDown);
			this.m_grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_grid_KeyDown);
			this.m_grid.DragOver += new System.Windows.Forms.DragEventHandler(this.m_grid_DragOver);
			this.m_grid.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseMove);
			this.m_grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.m_grid_CellPainting);
			this.m_grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.m_grid_RowsAdded);
			this.m_grid.LocationChanged += new System.EventHandler(this.m_grid_LocationChanged);
			this.m_grid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseUp);
			this.m_grid.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellMouseEnter);
			this.m_grid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseDoubleClick);
			this.m_grid.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_grid_DragDrop);
			this.m_grid.CurrentCellChanged += new System.EventHandler(this.m_grid_CurrentCellChanged);
			this.m_grid.DragLeave += new System.EventHandler(this.m_grid_DragLeave);
			// 
			// pnlWrapper
			// 
			this.pnlWrapper.AutoScroll = true;
			this.pnlWrapper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlWrapper.ClipTextForChildControls = true;
			this.pnlWrapper.ControlReceivingFocusOnMnemonic = null;
			this.pnlWrapper.Controls.Add(this.pnlGrid);
			this.pnlWrapper.Controls.Add(this.m_vsplitter);
			this.pnlWrapper.Controls.Add(this.pnlRowHeaderOuter);
			this.pnlWrapper.Controls.Add(this.m_hsplitter);
			this.pnlWrapper.Controls.Add(this.pnlColHeaderOuter);
			this.pnlWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlWrapper.DoubleBuffered = false;
			this.pnlWrapper.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlWrapper.Location = new System.Drawing.Point(0, 0);
			this.pnlWrapper.MnemonicGeneratesClick = false;
			this.pnlWrapper.Name = "pnlWrapper";
			this.pnlWrapper.PaintExplorerBarBackground = false;
			this.pnlWrapper.Size = new System.Drawing.Size(456, 380);
			this.pnlWrapper.TabIndex = 1;
			// 
			// pnlGrid
			// 
			this.pnlGrid.AutoScroll = true;
			this.pnlGrid.BackColor = System.Drawing.SystemColors.Window;
			this.pnlGrid.Controls.Add(this.m_grid);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.Location = new System.Drawing.Point(85, 36);
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.Size = new System.Drawing.Size(369, 342);
			this.pnlGrid.TabIndex = 8;
			this.pnlGrid.Resize += new System.EventHandler(this.pnlGrid_Resize);
			// 
			// m_vsplitter
			// 
			this.m_vsplitter.BackColor = System.Drawing.SystemColors.Window;
			this.m_vsplitter.Location = new System.Drawing.Point(81, 36);
			this.m_vsplitter.Name = "m_vsplitter";
			this.m_vsplitter.Size = new System.Drawing.Size(4, 342);
			this.m_vsplitter.TabIndex = 7;
			this.m_vsplitter.TabStop = false;
			this.m_vsplitter.Paint += new System.Windows.Forms.PaintEventHandler(this.m_vsplitter_Paint);
			this.m_vsplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.HandleSplitterMoved);
			// 
			// pnlRowHeaderOuter
			// 
			this.pnlRowHeaderOuter.BackColor = System.Drawing.SystemColors.Window;
			this.pnlRowHeaderOuter.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlRowHeaderOuter.Location = new System.Drawing.Point(0, 36);
			this.pnlRowHeaderOuter.Name = "pnlRowHeaderOuter";
			this.pnlRowHeaderOuter.Size = new System.Drawing.Size(81, 342);
			this.pnlRowHeaderOuter.TabIndex = 6;
			// 
			// m_hsplitter
			// 
			this.m_hsplitter.BackColor = System.Drawing.SystemColors.Window;
			this.m_hsplitter.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_hsplitter.Location = new System.Drawing.Point(0, 32);
			this.m_hsplitter.Name = "m_hsplitter";
			this.m_hsplitter.Size = new System.Drawing.Size(454, 4);
			this.m_hsplitter.TabIndex = 4;
			this.m_hsplitter.TabStop = false;
			this.m_hsplitter.Paint += new System.Windows.Forms.PaintEventHandler(this.m_hsplitter_Paint);
			this.m_hsplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.HandleSplitterMoved);
			// 
			// pnlColHeaderOuter
			// 
			this.pnlColHeaderOuter.BackColor = System.Drawing.SystemColors.Window;
			this.pnlColHeaderOuter.Controls.Add(this.pnlCorner);
			this.pnlColHeaderOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlColHeaderOuter.Location = new System.Drawing.Point(0, 0);
			this.pnlColHeaderOuter.Name = "pnlColHeaderOuter";
			this.pnlColHeaderOuter.Size = new System.Drawing.Size(454, 32);
			this.pnlColHeaderOuter.TabIndex = 3;
			// 
			// pnlCorner
			// 
			this.pnlCorner.BackColor = System.Drawing.SystemColors.Window;
			this.pnlCorner.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlCorner.Location = new System.Drawing.Point(0, 0);
			this.pnlCorner.Name = "pnlCorner";
			this.pnlCorner.Size = new System.Drawing.Size(99, 32);
			this.pnlCorner.TabIndex = 1;
			this.pnlCorner.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCorner_Paint);
			// 
			// CharGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlWrapper);
			this.Name = "CharGrid";
			this.Size = new System.Drawing.Size(456, 380);
			((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
			this.pnlWrapper.ResumeLayout(false);
			this.pnlGrid.ResumeLayout(false);
			this.pnlColHeaderOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private CharGridView m_grid;
		private SilPanel pnlWrapper;
		private System.Windows.Forms.Panel pnlCorner;
		private System.Windows.Forms.Panel pnlColHeaderOuter;
		private System.Windows.Forms.Splitter m_hsplitter;
		private System.Windows.Forms.Panel pnlRowHeaderOuter;
		private System.Windows.Forms.Splitter m_vsplitter;
		private System.Windows.Forms.Panel pnlGrid;
	}
}
