using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SilUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SilGrid : DataGridView
	{
		/// <summary>Occurs when a row is entered and after the current row's index changes.</summary>
		public event EventHandler CurrentRowChanged;

		public delegate void GetWaterMarkRectHandler(object sender,
			Rectangle adjustedClientRect, ref Rectangle rcProposed);
		
		public event GetWaterMarkRectHandler GetWaterMarkRect;

		private const string kDropDownStyle = "DropDown";
		
		protected bool _isDirty;
		protected bool _paintWaterMark;
		protected bool _showWaterMarkWhenDirty;
		protected string _waterMark = "!";
		protected int _prevRowIndex = -1;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilGrid()
		{
			DoubleBuffered = true;
			AllowUserToOrderColumns = true;
			AllowUserToResizeRows = false;
			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			FullRowFocusRectangleColor = SystemColors.ControlDark;
			BackgroundColor = DefaultCellStyle.BackColor = SystemColors.Window;
			base.ForeColor = DefaultCellStyle.ForeColor = SystemColors.WindowText;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			BorderStyle = BorderStyle.Fixed3D;
			ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			ColumnHeadersDefaultCellStyle.Font = SystemFonts.IconTitleFont;
			Font = SystemFonts.IconTitleFont;
			RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			RowHeadersWidth = 22;
			Color clr = SystemColors.Window;
			GridColor = Color.FromArgb(clr.R - 30, clr.G - 30, clr.B - 30);
			MultiSelect = false;
			PaintHeaderAcrossFullGridWidth = true;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the count of rows in the grid, excluding the new row (which really isn't a
		/// row until someone enters data into it).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RowCountLessNewRow
		{
			get { return (NewRowIndex == RowCount - 1 && RowCount > 0 ? RowCount - 1 : RowCount); }
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the grid's contents are dirty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDirty
		{
			get {return _isDirty;}
			set
			{
				_isDirty = value;
				_paintWaterMark = (value && _showWaterMarkWhenDirty);
				
				if (_showWaterMarkWhenDirty)
					Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints a header in the gap (if there is one) between the furthest right column and
		/// the right edge of the grid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool PaintHeaderAcrossFullGridWidth { get; set;}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the selection mode is full row and the right edge of the last column doesn't
		/// extend to the right edge of the client area, setting this flag to true will cause
		/// the full row selection rectangle to extend all the way to the right edge of the
		/// grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DefaultValue(false)]
		public bool ExtendFullRowSelectRectangleToEdge { get; set; }

		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DefaultValue(false)]
		public bool PaintFullRowFocusRectangle { get; set; }

		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		public Color FullRowFocusRectangleColor { get; set; }

		#endregion

		#region Watermark handling methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a water mark is shown when the grid
		/// is dirty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowWaterMarkWhenDirty
		{
			get { return _showWaterMarkWhenDirty; }
			set
			{
				_showWaterMarkWhenDirty = value;
				_paintWaterMark = (value && _isDirty);
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the character displayed as the water mark.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string WaterMark
		{
			get { return _waterMark; }
			set
			{
				_waterMark = value;
				if (_paintWaterMark)
					Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle in which the watermark is drawn.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Rectangle WaterMarkRectangle
		{
			get
			{
				Rectangle rc;
				int clientWidth = ClientSize.Width;

				if (Rows.Count > 0 && Columns.Count > 0 && FirstDisplayedCell != null)
				{
					// Determine whether or not the vertical scroll bar is showing.
					int visibleRows = Rows.GetRowCount(DataGridViewElementStates.Visible);
					rc = GetCellDisplayRectangle(0, FirstDisplayedCell.RowIndex, false);
					if (rc.Height * visibleRows >= ClientSize.Height)
						clientWidth -= SystemInformation.VerticalScrollBarWidth;
				}

				// Modify the client rectangle so it doesn't
				// include the vertical scroll bar width.
				rc = ClientRectangle;
				rc.Width = (int)(clientWidth * 0.5f);
				rc.Height = (int)(rc.Height * 0.5f);
				rc.X = (clientWidth - rc.Width) / 2;
				rc.Y = (ClientRectangle.Height - rc.Height) / 2;

				// Get the rectangle from subscribers if there are any.
				// If not, then just return a rectangle centered in the grid.
				if (GetWaterMarkRect != null)
				{
					Rectangle rcAdjustedClientRect = ClientRectangle;
					rcAdjustedClientRect.Width = clientWidth;
					GetWaterMarkRect(this, rcAdjustedClientRect, ref rc);
				}

				return rc;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the water mark when the grid changes size.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			bool paintWaterMark = _paintWaterMark;

			if (_paintWaterMark)
			{
				// Clear previous water mark.
				_paintWaterMark = false;
				Invalidate();
			}

			base.OnSizeChanged(e);

			_paintWaterMark = paintWaterMark;
			if (_paintWaterMark)
				Invalidate(WaterMarkRectangle);

			if (!_paintWaterMark && Focused && PaintFullRowFocusRectangle &&
				CurrentCellAddress.Y >= 0 && CurrentCellAddress.Y < RowCount)
			{
				InvalidateRow(CurrentCellAddress.Y);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);

			// For some reason, using OnEnter only redraws the current cell, not the
			// entire row. This event works better.
			InvalidateRowInFullRowSelectMode(CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			InvalidateRowInFullRowSelectMode(CurrentCellAddress.Y);
		}

		/// ------------------------------------------------------------------------------------
		public VScrollBar VScrollBar
		{
			get
			{
				foreach (var ctrl in Controls)
				{
					if (ctrl is VScrollBar)
						return ctrl as VScrollBar;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public HScrollBar HScrollBar
		{
			get
			{
				foreach (var ctrl in Controls)
				{
					if (ctrl is HScrollBar)
						return ctrl as HScrollBar;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the water mark when the grid scrolls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs e)
		{
			bool paintWaterMark = _paintWaterMark;

			if (_paintWaterMark)
			{
				// Clear previous water mark.
				_paintWaterMark = false;
				Invalidate();
			}

			base.OnScroll(e);

			_paintWaterMark = paintWaterMark;
			if (_paintWaterMark)
				Invalidate(WaterMarkRectangle);

			// This chunk of code takes care of painting problems when scrolling and a full
			// row focus rectangle is being drawn. The problems are:
			// 1) When there are fixed columns and the grid scrolls horizontally so that it
			//    exposes columns that were scrolled out of view under some fixed columns,
			//    the top focus rect. line of the cells scrolling into view are not painted.
			// 2) One problem has to do with the first row of pixels below the column headers.
			//    The focus rectangle color was getting left behind when selected rows were
			//    being scrolled upward, out of view.
			// 3) Another problem has to do with the selected row losing the top edge of its
			//    focus rectangle when it is scrolled downward after having been the top visible row.
			if (PaintFullRowFocusRectangle && SelectionMode == DataGridViewSelectionMode.FullRowSelect)
			{
				if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				{
					InvalidateRowInFullRowSelectMode(CurrentCellAddress.Y);
				}
				else
				{
					if (CurrentCellAddress.Y >= 0 && CurrentCellAddress.Y < RowCount)
					{
						Invalidate(new Rectangle(0, ColumnHeadersHeight - 1, ClientSize.Width, 2));

						if (FirstDisplayedScrollingRowIndex == CurrentCellAddress.Y - 1)
							InvalidateRowInFullRowSelectMode(CurrentCellAddress.Y);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints a water mark when the results are stale (i.e. the query settings have been
		/// changed since the results were shown).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!_paintWaterMark || string.IsNullOrEmpty(_waterMark))
				return;

			Rectangle rc = WaterMarkRectangle;
			GraphicsPath path = new GraphicsPath();
			FontFamily family = FontFamily.GenericSerif;

			// Find the first font size equal to or smaller than 256 that
			// fits in the water mark rectangle.
			for (int size = 256; size >= 0; size -= 2)
			{
				using (Font fnt = FontHelper.MakeFont(family.Name, size, FontStyle.Bold))
				{
					int height = TextRenderer.MeasureText(_waterMark, fnt).Height;
					if (height < rc.Height)
					{
						using (StringFormat sf = Utils.GetStringFormat(true))
							path.AddString(_waterMark, family, (int)FontStyle.Bold, size, rc, sf);

						break;
					}
				}
			}

			path.AddEllipse(rc);

			using (SolidBrush br = new SolidBrush(Color.FromArgb(35, DefaultCellStyle.ForeColor)))
				e.Graphics.FillRegion(br, new Region(path));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When a cell is in the edit mode and all the text in the edit control is selected,
		/// pressing the home key changes the cell to the first cell in the row and pressing
		/// the end key changes the cell to the last cell in the row. This seems counter
		/// intuitive. I would expect the cursor to move to the beginning or end of the text
		/// within the edit control, therefore, this method will trap those two keys and
		/// force this desired behavior, then eat the key message as though it wasn't
		/// dispatched.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (IsCurrentCellInEditMode && EditingControl is TextBox)
			{
				var txtBox = ((TextBox)EditingControl);

				// Only override the default behavior when all the text in the edit control is selected.
				if (keyData == Keys.Home || keyData == Keys.End && txtBox.SelectedText == txtBox.Text)
				{
					ProcessHomeAndEndKeys(txtBox, keyData);
					return true;
				}

				if (keyData == Keys.Up && ProcessUpKey(txtBox))
					return true;

				if (keyData == Keys.Down && ProcessDownKey(txtBox))
					return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes the home and end keys.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ProcessHomeAndEndKeys(TextBoxBase txtBox, Keys keys)
		{
			txtBox.SelectionStart = (keys == Keys.Home ? 0 : txtBox.Text.Length);
			txtBox.SelectionLength = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes up key when a grid cell is in the edit mode. This overrides the default
		/// behavior in a grid cell when it's being edited so using the up arrow will move the
		/// IP up one line rather than moving to the previous row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool ProcessUpKey(TextBox txtBox)
		{
			// Don't override the default behavior if all the text is selected.
			if (txtBox.SelectedText == txtBox.Text)
				return false;

			Point pt = txtBox.GetPositionFromCharIndex(txtBox.SelectionStart);
			
			if (pt.Y == 0)
				return false;
			
			pt.Y -= TextRenderer.MeasureText("x", txtBox.Font).Height;
			txtBox.SelectionStart = txtBox.GetCharIndexFromPosition(pt);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes down key when a grid cell is in the edit mode. This overrides the default
		/// behavior in a grid cell when it's being edited so using the down arrow will move the
		/// IP down one line rather than moving to the next row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool ProcessDownKey(TextBox txtBox)
		{
			// Don't override the default behavior if all the text is selected.
			if (txtBox.SelectedText == txtBox.Text)
				return false;

			int chrIndex = txtBox.SelectionStart;
			Point pt = txtBox.GetPositionFromCharIndex(txtBox.SelectionStart);
			pt.Y += TextRenderer.MeasureText("x", txtBox.Font).Height;
			txtBox.SelectionStart = txtBox.GetCharIndexFromPosition(pt);
			return (chrIndex != txtBox.SelectionStart);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// In order to achieve double buffering without the problem that arises from having
		/// double buffering on while sizing rows and columns or dragging columns around,
		/// monitor when the mouse goes down and turn off double buffering when it goes down
		/// on a column heading or over the dividers between rows or the dividers between
		/// columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (Cursor == Cursors.SizeNS || Cursor == Cursors.SizeWE)
				DoubleBuffered = false;

			base.OnCellMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When double buffering is off, it means it was turned off in the cell mouse down
		/// event. Therefore, turn it back on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (!DoubleBuffered)
				DoubleBuffered = true;

			base.OnCellMouseUp(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
		{
			base.OnRowsAdded(e);
			IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
		{
			base.OnRowsRemoved(e);
			IsDirty = true;
		}

		#endregion

		#region Events and methods for handling DropDown style combo box cells.
		/// ------------------------------------------------------------------------------------
		protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
		{
			if (!_isDirty)
				IsDirty  = IsCurrentCellDirty;
			
			base.OnCurrentCellDirtyStateChanged(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if we need to modify the drop-down style of the grid's combo box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
		{
			base.OnEditingControlShowing(e);

			// When the cell style's tag is storing a ComboBoxStyle value, we know that value is
			// ComboBoxStyle.DropDown. Therefore, modify the control's DropDownStyle property.
			ComboBox cbo = e.Control as ComboBox;
			if (cbo == null || (e.CellStyle.Tag as string) != kDropDownStyle)
				return;

			cbo.DropDownStyle = ComboBoxStyle.DropDown;
			
			// ENHANCE: Should an event delegate be provided? One to allow
			// subscribers to have a chance to use a custom sort.
			SortComboList(cbo);

			cbo.TextChanged += delegate
			{
				// This will make sure the validation process doesn't throw out text typed
				// directly into the cell rather than the value being set from the drop-down
				// list. I don't yet understand why this is necessary, but it works.
				NotifyCurrentCellDirty(true);
			};
		}

		/// ------------------------------------------------------------------------------------
		private static void SortComboList(ComboBox cbo)
		{
			SortedList lst = new SortedList();

			foreach (object obj in cbo.Items)
				lst[obj] = null;

			cbo.Items.Clear();

			foreach (DictionaryEntry de in lst)
				cbo.Items.Add(de.Key);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
		{
			base.OnCellValidating(e);

			// If a delegate already cancelled or the cell is not in a column with the
			// combo box drop-down style, then get out now.
			if (e.Cancel)
				return;

			try
			{
				DataGridViewComboBoxColumn col = Columns[e.ColumnIndex] as DataGridViewComboBoxColumn;
				if (col == null || (col.DefaultCellStyle.Tag as string) != kDropDownStyle)
					return;

				// If the entered value is empty, then don't add it to the list.
				string value = e.FormattedValue as string;
				if (value != null && value.Trim() == string.Empty)
					return;

				// Convert the formatted value to the type the column is expecting. Then add it
				// to the combo list if it isn't already in there.
				object obj = Convert.ChangeType(e.FormattedValue, col.ValueType);
				if (obj != null && !(col.Items.Contains(obj)))
					col.Items.Add(obj);
			}
			catch { }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For grids containing combo box columns created using the
		/// CreateDropDownComboBoxColumn method, this way of adding rows should be used since
		/// it will verify (before adding the row to the rows collection) that items added
		/// to those columns will have corresponding items in their drop-down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int AddRow(object[] items)
		{
			for (int i = 0; i < items.Length; i++)
			{
				DataGridViewComboBoxColumn col = Columns[i] as DataGridViewComboBoxColumn;
				if (col == null || (col.DefaultCellStyle.Tag as string) != kDropDownStyle)
					continue;

				// At this point, we know we're looking at a combo box column
				// whose style is DropDown. Therefore, make sure the item is in
				// it's list and if not, add it.
				if (items[i] != null && !col.Items.Contains(items[i]))
					col.Items.Add(items[i]);
			}

			return Rows.Add(items);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will go through the values in the specified row and verify, that for
		/// each DropDown style combo. item in the row, the combo. list contains the item in
		/// the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void VerifyComboItemsFromRow(DataGridViewRow row)
		{
			foreach (DataGridViewCell cell in row.Cells)
			{
				DataGridViewComboBoxColumn col = Columns[cell.ColumnIndex] as DataGridViewComboBoxColumn;
				if (col == null || (col.DefaultCellStyle.Tag as string) != kDropDownStyle)
					continue;

				// At this point, we know we're looking at a combo box column
				// whose style is DropDown. Therefore, make sure the item is in
				// it's list and if not, add it.
				if (cell.Value != null && !col.Items.Contains(cell.Value))
					col.Items.Add(cell.Value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selects the first, visible column that is adjacent to the current column (i.e.
		/// the column of the current cell). When searchLeftFirst is true, an attempt will
		/// be made to first find a visible column to the left of the current column. If
		/// that fails, then an attempt is made to find a visible column to the right of the
		/// current column. Of course, if searchLeftFirst is false, then searching right is
		/// first, then left. If a new column is successfully selected, true is returned.
		/// Otherwise, false is returned. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SelectAdjacentVisibleColumn(bool searchLeftFirst)
		{
			// Can't select a cell in the current row if there is no current row.
			if (CurrentCellAddress.Y < 0)
				return false;

			int inc = (searchLeftFirst ? -1 : 1);

			for (int pass = 0; pass < 2; pass++)
			{
				for (int i = CurrentCellAddress.X + inc; i >= 0 && i < ColumnCount; i += inc)
				{
					if (Columns[i].Visible)
					{
						CurrentCell = this[i, CurrentCellAddress.Y];
						return true;
					}
				}

				inc *= -1;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCurrentCellChanged(EventArgs e)
		{
			base.OnCurrentCellChanged(e);

			if (_prevRowIndex != CurrentCellAddress.Y)
			{
				InvalidateRowInFullRowSelectMode(_prevRowIndex);
				InvalidateRowInFullRowSelectMode(CurrentCellAddress.Y);
				_prevRowIndex = CurrentCellAddress.Y;
				OnCurrentRowChanged(EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			base.OnCellPainting(e);

			if (e.RowIndex == -1 && e.ColumnIndex == 0 && PaintHeaderAcrossFullGridWidth &&
				ClientSize.Width > Columns.GetColumnsWidth(DataGridViewElementStates.Visible))
			{
				DrawExtendedColumnHeaderRow(e);
			}

			if (!e.Handled && Focused & PaintFullRowFocusRectangle &&
				SelectionMode == DataGridViewSelectionMode.FullRowSelect)
			{
				DrawFocusRectangle(e);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method fills in the gap between the header of the last visible column and the
		/// right edge of the grid. That gap is filled with something that looks like one long
		/// empty column header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void DrawExtendedColumnHeaderRow(DataGridViewCellPaintingEventArgs e)
		{
			Rectangle rc = e.CellBounds;
			rc.Width = ClientSize.Width;

			VisualStyleElement element = VisualStyleElement.Header.Item.Normal;
			if (PaintingHelper.CanPaintVisualStyle(element))
			{
				if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
					rc.Height--;

				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(e.Graphics, rc);
			}
			else
			{
				// Draw this way when themes aren't supported.
				if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.None)
					e.Graphics.FillRectangle(SystemBrushes.Control, rc);
				else if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
				{
					ControlPaint.DrawButton(e.Graphics, rc, ButtonState.Normal);
					ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Flat, Border3DSide.Bottom);
				}
			}

			// Clean up the bottom edge.
			if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
				ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Etched, Border3DSide.Top);
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		protected virtual void DrawFocusRectangle(DataGridViewCellPaintingEventArgs e)
		{
			e.Handled = true;
			var paintParts = e.PaintParts;
			var rc = e.CellBounds;

			paintParts &= ~DataGridViewPaintParts.Focus;
			e.Paint(rc, paintParts);

			int rowAboveCurrent = (CurrentCellAddress.Y == FirstDisplayedScrollingRowIndex ?
				-1 : CurrentCellAddress.Y - 1);

			if (e.RowIndex == rowAboveCurrent && CurrentCellAddress.Y >= FirstDisplayedScrollingRowIndex)
			{
				// This draws the line across the top of the cell. (It's really
				// drawing the line across the bottom of the cell above the focused row.)
				using (var pen = new Pen(FullRowFocusRectangleColor))
					e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 1, rc.Right - 1, rc.Bottom - 1);
			}
			
			if (e.RowIndex != CurrentCellAddress.Y)
				return;

			using (var pen = new Pen(FullRowFocusRectangleColor))
			{
				// Draw a line across the bottom of the cell.
				e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 1, rc.Right - 1, rc.Bottom - 1);

				// If the column is the furthest left, then draw a
				// vertical line on the left edge of the cell.
				if (Columns[e.ColumnIndex].DisplayIndex == 0)
					e.Graphics.DrawLine(pen, rc.X, rc.Y, rc.X, rc.Bottom - 1);

				// If the column is the furthest right, then draw a
				// vertical line on the right edge of the cell.
				if (Columns[e.ColumnIndex].DisplayIndex == ColumnCount - 1)
					e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will only deal with painting in the area between the right edge of the
		/// furthest right displayed cell and the right edge of the grid's client area.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
		{
			base.OnRowPostPaint(e);

			if ((e.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.Selected ||
				!ExtendFullRowSelectRectangleToEdge || e.RowIndex < 0 || e.RowIndex >= RowCount)
			{
				return;
			}

			// Determine whether or not there is any gap between the right edge of the last
			// displayed cell and the right edge of the client area.
			var sumOfColWidths = Columns.GetColumnsWidth(DataGridViewElementStates.Displayed);
			if (sumOfColWidths > ClientSize.Width)
				return;

			var rc = GetRowDisplayRectangle(e.RowIndex, false);
			rc.Width -= (sumOfColWidths - 1);
			rc.X += (sumOfColWidths - 1);

			// Fill the gap between the last displayed column and the right edge of the client area
			// with the selection color so the row's selection highlighting spans the entire grid's
			// client width.
			using (var br = new SolidBrush(DefaultCellStyle.SelectionBackColor))
				e.Graphics.FillRectangle(br, rc);

			if (!Focused || !PaintFullRowFocusRectangle || e.RowIndex != CurrentCellAddress.Y)
				return;

			// Draw focus rectangle line across the top and bottom and along the right edge.
			using (var pen = new Pen(FullRowFocusRectangleColor))
			{
				e.Graphics.DrawLine(pen, rc.X, rc.Y - 1, rc.Right, rc.Y - 1);
				e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InvalidateRowInFullRowSelectMode(int row)
		{
			if (PaintFullRowFocusRectangle && SelectionMode == DataGridViewSelectionMode.FullRowSelect &&
				row >= 0 && row < RowCount)
			{
				var rc = GetRowDisplayRectangle(row, false);
				rc.Inflate(1, 2);
				Invalidate(rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnCurrentRowChanged(EventArgs e)
		{
			if (CurrentRowChanged != null)
				CurrentRowChanged(this, EventArgs.Empty);
		}

		#region Static methods for creating various column types
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a text box grid column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewColumn CreateTextBoxColumn(string name)
		{
			return CreateTextBoxColumn(name, name);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a text box grid column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewColumn CreateTextBoxColumn(string name, string headerText)
		{
			var col = new DataGridViewColumn();
			var templateCell = new DataGridViewTextBoxCell();
			templateCell.Style.Font = SystemFonts.MenuFont;
			col.HeaderCell.Style.Font = SystemFonts.MenuFont;
			col.CellTemplate = templateCell;
			col.Name = name;
			col.HeaderText = headerText;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a calendar control grid column that hosts calendar (date) cells.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewColumn CreateCalendarControlColumn(string name)
		{
			return CreateCalendarControlColumn(name, name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a calendar control grid column that hosts calendar (date) cells.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewColumn CreateCalendarControlColumn(string name, string headerText)
		{
			var col = new SilCalendarColumn();
			col.HeaderCell.Style.Font = SystemFonts.MenuFont;
			col.Name = name;
			col.HeaderText = headerText;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values must be chosen from the
		/// drop-down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownListComboBoxColumn(string name,
			List<string> items)
		{
			return CreateDropDownListComboBoxColumn(name, items);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values must be chosen from the
		/// drop-down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownListComboBoxColumn(string name, IEnumerable<string> items)
		{
			return CreateDropDownListComboBoxColumn(name, items.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values must be chosen from the
		/// drop-down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownListComboBoxColumn(string name, IEnumerable<object> items)
		{
			return CreateDropDownListComboBoxColumn(name, items.ToArray());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values must be chosen from the
		/// drop-down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownListComboBoxColumn(string name,
			ArrayList items)
		{
			return CreateDropDownListComboBoxColumn(name, (object[])items.ToArray(typeof(object)));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values must be chosen from the
		/// drop-down list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownListComboBoxColumn(string name, object[] items)
		{
			var col = new DataGridViewComboBoxColumn();
			var templateCell = new DataGridViewComboBoxCell();
			templateCell.DisplayStyleForCurrentCellOnly = true;
			templateCell.Style.Font = SystemFonts.MenuFont;
			templateCell.AutoComplete = true;
			col.HeaderCell.Style.Font = SystemFonts.MenuFont;
			col.CellTemplate = templateCell;
			col.FlatStyle = FlatStyle.Standard;
			col.MaxDropDownItems = 10;
			col.Name = name;
			col.HeaderText = name;

			// Set the data type expected for data in this column.
			if (items.Length > 0)
				col.ValueType = items[0].GetType();

			//gridCol.DataSource = items;
			col.Items.AddRange(items);

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values can be set by choosing a value
		/// from the drop-down list or by typing one into the cell, even one that doesn't
		/// appear in the drop-down list. If a value is typed into the cell that doesn't
		/// appear in the drop-down list, that value is added to the list when the cell is
		/// validated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownComboBoxColumn(string name, IEnumerable<string> items)
		{
			return CreateDropDownComboBoxColumn(name, items.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values can be set by choosing a value
		/// from the drop-down list or by typing one into the cell, even one that doesn't
		/// appear in the drop-down list. If a value is typed into the cell that doesn't
		/// appear in the drop-down list, that value is added to the list when the cell is
		/// validated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownComboBoxColumn(string name, IEnumerable<object> items)
		{
			return CreateDropDownComboBoxColumn(name, items.ToArray());
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values can be set by choosing a value
		/// from the drop-down list or by typing one into the cell, even one that doesn't
		/// appear in the drop-down list. If a value is typed into the cell that doesn't
		/// appear in the drop-down list, that value is added to the list when the cell is
		/// validated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownComboBoxColumn(string name,
			ArrayList items)
		{
			return CreateDropDownComboBoxColumn(name, (object[])items.ToArray(typeof(object)));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a combo. box grid column whose cell values can be set by choosing a value
		/// from the drop-down list or by typing one into the cell, even one that doesn't
		/// appear in the drop-down list. If a value is typed into the cell that doesn't
		/// appear in the drop-down list, that value is added to the list when the cell is
		/// validated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewComboBoxColumn CreateDropDownComboBoxColumn(string name, object[] items)
		{
			var col = CreateDropDownListComboBoxColumn(name, items);

			// Store a flag in the default cell style's tag because that seems to be the only
			// property for user data that is common to both the OnEditingControlShowing and
			// OnCellValidating events. This flag is used in OnEditingControlShowing to
			// indicate the combo box control's DropDownStyle should be set to DropDown.
			col.DefaultCellStyle.Tag = kDropDownStyle;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a check box grid column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewCheckBoxColumn CreateCheckBoxColumn(string name)
		{
			var col = new DataGridViewCheckBoxColumn();
			var templateCell = new DataGridViewCheckBoxCell();
			templateCell.Style.Font = SystemFonts.MenuFont;
			col.CellTemplate = templateCell;
			col.HeaderCell.Style.Font = SystemFonts.MenuFont;
			col.Name = name;
			col.HeaderText = name;
			col.FlatStyle = FlatStyle.Standard;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an image grid column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewImageColumn CreateImageColumn(string name)
		{
			var col = new DataGridViewImageColumn();
			var templateCell = new DataGridViewImageCell();
			templateCell.Style.Font = SystemFonts.MenuFont;
			templateCell.ImageLayout = DataGridViewImageCellLayout.Normal;
			col.CellTemplate = templateCell;
			col.HeaderCell.Style.Font = SystemFonts.MenuFont;
			col.ImageLayout = DataGridViewImageCellLayout.Normal;
			col.Name = name;
			col.HeaderText = name;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a SilButtonColumn grid column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SilButtonColumn CreateSilButtonColumn(string name)
		{
			var col = new SilButtonColumn(name);
			var templateCell = new SilButtonCell();
			templateCell.Style.Font = SystemFonts.MenuFont;
			templateCell.Style.SelectionForeColor = SystemColors.HighlightText;
			col.CellTemplate = templateCell;
			col.HeaderCell.Style.Font = SystemFonts.MenuFont;
			col.HeaderText = name;

			return col;
		}

		#endregion
	}
}
