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
			this.Grid = new SIL.Pa.UI.Controls.CharGridView();
			this.pnlWrapper = new SilPanel();
			this.pnlGrid = new System.Windows.Forms.Panel();
			this.m_vsplitter = new System.Windows.Forms.Splitter();
			this.pnlRowHeaderOuter = new System.Windows.Forms.Panel();
			this.m_hsplitter = new System.Windows.Forms.Splitter();
			this.pnlColHeaderOuter = new System.Windows.Forms.Panel();
			this.pnlCorner = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
			this.pnlWrapper.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			this.pnlColHeaderOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_grid
			// 
			this.Grid.AllowDrop = true;
			this.Grid.AllowUserToAddRows = false;
			this.Grid.AllowUserToDeleteRows = false;
			this.Grid.AllowUserToResizeColumns = false;
			this.Grid.AllowUserToResizeRows = false;
			this.Grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.Grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.Grid.ColumnHeadersVisible = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.Grid.DefaultCellStyle = dataGridViewCellStyle1;
			this.Grid.EnableHeadersVisualStyles = false;
			this.Grid.Location = new System.Drawing.Point(0, 0);
			this.Grid.Name = "Grid";
			this.Grid.ReadOnly = true;
			this.Grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.Grid.RowHeadersVisible = false;
			this.Grid.RowHeadersWidth = 75;
			this.Grid.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.Grid.ShowCellToolTips = false;
			this.Grid.ShowEditingIcon = false;
			this.Grid.ShowRowErrors = false;
			this.Grid.Size = new System.Drawing.Size(135, 137);
			this.Grid.TabIndex = 0;
			this.Grid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseDown);
			this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_grid_KeyDown);
			this.Grid.DragOver += new System.Windows.Forms.DragEventHandler(this.m_grid_DragOver);
			this.Grid.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseMove);
			this.Grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.m_grid_CellPainting);
			this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.m_grid_RowsAdded);
			this.Grid.LocationChanged += new System.EventHandler(this.m_grid_LocationChanged);
			this.Grid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseUp);
			this.Grid.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_grid_CellMouseEnter);
			this.Grid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_CellMouseDoubleClick);
			this.Grid.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_grid_DragDrop);
			this.Grid.CurrentCellChanged += new System.EventHandler(this.m_grid_CurrentCellChanged);
			this.Grid.DragLeave += new System.EventHandler(this.m_grid_DragLeave);
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
			this.pnlGrid.Controls.Add(this.Grid);
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
			((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
			this.pnlWrapper.ResumeLayout(false);
			this.pnlGrid.ResumeLayout(false);
			this.pnlColHeaderOuter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SilPanel pnlWrapper;
		private System.Windows.Forms.Panel pnlCorner;
		private System.Windows.Forms.Panel pnlColHeaderOuter;
		private System.Windows.Forms.Splitter m_hsplitter;
		private System.Windows.Forms.Panel pnlRowHeaderOuter;
		private System.Windows.Forms.Splitter m_vsplitter;
		private System.Windows.Forms.Panel pnlGrid;
	}
}
