using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SIL.SpeechTools.Utils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SilGrid : DataGridView
	{
		public delegate void GetWaterMarkRectHandler(object sender,
			Rectangle adjustedClientRect, ref Rectangle rcProposed);
		
		public event GetWaterMarkRectHandler GetWaterMarkRect;

		private static readonly string kDropDownStyle = "DropDown";
		
		private bool m_isDirty = false;
		private bool m_paintWaterMark = false;
		private bool m_showWaterMarkWhenDirty = false;
		private string m_waterMark = "!";

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
			BackgroundColor = DefaultCellStyle.BackColor = SystemColors.Window;
			base.ForeColor = DefaultCellStyle.ForeColor = SystemColors.WindowText;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			BorderStyle = BorderStyle.Fixed3D;
			ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			ColumnHeadersDefaultCellStyle.Font = SystemInformation.MenuFont;
			RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			RowHeadersWidth = 22;
			Color clr = SystemColors.Window;
			GridColor = Color.FromArgb(clr.R - 30, clr.G - 30, clr.B - 30);

			MultiSelect = false;
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
			get {return m_isDirty;}
			set
			{
				m_isDirty = value;
				m_paintWaterMark = (value && m_showWaterMarkWhenDirty);
				
				if (m_showWaterMarkWhenDirty)
					Invalidate();
			}
		}

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
			get { return m_showWaterMarkWhenDirty; }
			set
			{
				m_showWaterMarkWhenDirty = value;
				m_paintWaterMark = (value && m_isDirty);
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
			get { return m_waterMark; }
			set
			{
				m_waterMark = value;
				if (m_paintWaterMark)
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
			bool paintWaterMark = m_paintWaterMark;

			if (m_paintWaterMark)
			{
				// Clear previous water mark.
				m_paintWaterMark = false;
				Invalidate();
			}

			base.OnSizeChanged(e);

			m_paintWaterMark = paintWaterMark;
			if (m_paintWaterMark)
				Invalidate(WaterMarkRectangle);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the water mark when the grid scrolls.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs e)
		{
			bool paintWaterMark = m_paintWaterMark;

			if (m_paintWaterMark)
			{
				// Clear previous water mark.
				m_paintWaterMark = false;
				Invalidate();
			}

			base.OnScroll(e);

			m_paintWaterMark = paintWaterMark;
			if (m_paintWaterMark)
				Invalidate(WaterMarkRectangle);
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

			if (!m_paintWaterMark || string.IsNullOrEmpty(m_waterMark))
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
					int height = TextRenderer.MeasureText(m_waterMark, fnt).Height;
					if (height < rc.Height)
					{
						using (StringFormat sf = STUtils.GetStringFormat(true))
							path.AddString(m_waterMark, family, (int)FontStyle.Bold, size, rc, sf);

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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
		{
			base.OnRowsAdded(e);
			IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
		{
			base.OnRowsRemoved(e);
			IsDirty = true;
		}

		#endregion

		#region Events and methods for handling DropDown style combo box cells.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
		{
			if (!m_isDirty)
				IsDirty  = IsCurrentCellDirty;
			
			base.OnCurrentCellDirtyStateChanged(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if we need to modify the drop-down style of the grid's combo box.
		/// </summary>
		/// <param name="e"></param>
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cbo"></param>
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
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
		/// This method fills in the gap between the header of the last visible column and the
		/// right edge of the grid. That gap is filled with something that looks like one long
		/// empty column header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			base.OnCellPainting(e);

			int colWidths = Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
			if (e.RowIndex > -1 || e.ColumnIndex != 0 || colWidths > ClientSize.Width)
				return;

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
					ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Flat,
						Border3DSide.Bottom);
				}
			}
		
			if (ColumnHeadersBorderStyle == DataGridViewHeaderBorderStyle.Raised)
			{
				// Clean up the bottom edge.
				ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.Etched,
					Border3DSide.Top);
			}
		}

		#region Static methods for creating various column types
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a text box grid column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewColumn CreateTextBoxColumn(string name)
		{
			DataGridViewColumn col = new DataGridViewColumn();
			DataGridViewTextBoxCell templateCell = new DataGridViewTextBoxCell();
			templateCell.Style.Font = SystemInformation.MenuFont;
			col.HeaderCell.Style.Font = SystemInformation.MenuFont;
			col.CellTemplate = templateCell;
			col.Name = name;
			col.HeaderText = name;

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
			object[] objects = new object[items.Count];
			for (int i = 0; i < items.Count; i++)
				objects[i] = items[i];
			
			return CreateDropDownListComboBoxColumn(name, objects);
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
			DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
			DataGridViewComboBoxCell templateCell = new DataGridViewComboBoxCell();
			templateCell.DisplayStyleForCurrentCellOnly = true;
			templateCell.Style.Font = SystemInformation.MenuFont;
			templateCell.AutoComplete = true;
			col.HeaderCell.Style.Font = SystemInformation.MenuFont;
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
		/// <param name="name"></param>
		/// <param name="items"></param>
		/// <returns></returns>
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
			DataGridViewComboBoxColumn col = CreateDropDownListComboBoxColumn(name, items);

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
			DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
			DataGridViewCheckBoxCell templateCell = new DataGridViewCheckBoxCell();
			templateCell.Style.Font = SystemInformation.MenuFont;
			col.CellTemplate = templateCell;
			col.HeaderCell.Style.Font = SystemInformation.MenuFont;
			col.Name = name;
			col.HeaderText = name;
			col.FlatStyle = FlatStyle.Standard;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an image grid column.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static DataGridViewImageColumn CreateImageColumn(string name)
		{
			DataGridViewImageColumn col = new DataGridViewImageColumn();
			DataGridViewImageCell templateCell = new DataGridViewImageCell();
			templateCell.Style.Font = SystemInformation.MenuFont;
			templateCell.ImageLayout = DataGridViewImageCellLayout.Normal;
			col.CellTemplate = templateCell;
			col.HeaderCell.Style.Font = SystemInformation.MenuFont;
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
			SilButtonColumn col = new SilButtonColumn(name);
			SilButtonCell templateCell = new SilButtonCell();
			templateCell.Style.Font = SystemInformation.MenuFont;
			templateCell.Style.SelectionForeColor = SystemColors.HighlightText;
			col.CellTemplate = templateCell;
			col.HeaderCell.Style.Font = SystemInformation.MenuFont;
			col.HeaderText = name;

			return col;
		}

		#endregion
	}
}
