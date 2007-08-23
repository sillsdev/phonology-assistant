using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SIL.SpeechTools.Utils.Properties;

namespace SIL.SpeechTools.Utils
{
	#region SilHierarchicalGridRow class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SilHierarchicalGridRow : DataGridViewRow
	{
		public delegate void ExpandedStateChangedHandler(SilHierarchicalGridRow row);
		public event ExpandedStateChangedHandler ExpandedStateChanged;
		private bool m_expanded = true;
		private DataGridView m_owningGrid;
		private int m_glyphWidth;
		private int m_glyphHeight;
		private string m_text;
		private Font m_font;
		private int m_level;
		private int m_childCount = -1;
		private int m_hierarchicalColWidths = -1;
		private bool m_subscribeToOwnersEventsOnNextClone = false;
		private string[] m_recCountFmt;
		private int m_firstCacheIndex = -1;
		private int m_lastCacheIndex = -1;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow()
		{
			base.ReadOnly = true;
			Image glyph = Resources.kimidExpand;
			m_glyphWidth = glyph.Width;
			m_glyphHeight = glyph.Height;

			m_recCountFmt = new string[] {
				Resources.kstidHierarchicalRowChildCountLongFmt,
				Resources.kstidHierarchicalRowChildCountMedFmt,
				Resources.kstidHierarchicalRowChildCountShortFmt};
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow(DataGridView owningGrid) : this()
		{
			m_owningGrid = owningGrid;
			m_subscribeToOwnersEventsOnNextClone = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow(DataGridView owningGrid, string text) : this(owningGrid)
		{
			m_text = text;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow(DataGridView owningGrid, string text, Font font)
			: this(owningGrid, text)
		{
			m_font = font;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow(DataGridView owningGrid, string text, int childCount)
			: this(owningGrid, text)
		{
			m_childCount = childCount;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow(DataGridView owningGrid, string text, Font font,
			int childCount) : this(owningGrid, text, font)
		{
			m_childCount = childCount;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridRow(DataGridView owningGrid, string text, Font font,
			int firstCacheIndex, int lastCacheIndex)
			: this(owningGrid, text, font)
		{
			m_firstCacheIndex = firstCacheIndex;
			m_lastCacheIndex = lastCacheIndex;
			m_childCount = lastCacheIndex - firstCacheIndex + 1;
		}

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up, like a good boy.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			UnsubscribeToOwningGridEvents();

			if (m_font != null)
			{
				m_font.Dispose();
				m_font = null;
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an exact copy of the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override object Clone()
		{
			object clone = base.Clone();

			if (clone is SilHierarchicalGridRow)
			{
				SilHierarchicalGridRow row = clone as SilHierarchicalGridRow;
				row.m_owningGrid = m_owningGrid;
				row.m_expanded = m_expanded;
				row.m_glyphWidth = m_glyphWidth;
				row.m_glyphHeight = m_glyphHeight;
				row.m_text = m_text;
				row.m_font = m_font;
				row.m_level = m_level;
				row.m_childCount = m_childCount;
				row.m_firstCacheIndex = m_firstCacheIndex;
				row.m_lastCacheIndex = m_lastCacheIndex;
				row.m_recCountFmt = m_recCountFmt;

				// This is kludgy but here's the ridiculous problem I observe. Whenever a row
				// is added to the grid, internally, that row is cloned and added to the grid,
				// which forces the row to fire it's OnDataGridViewChanged event since an
				// owning grid has just been assigned to the row. However, it appears that
				// just cloning a row (not just adding it to the grid's row collection) also
				// fires the row's OnDataGridViewChanged event. In our case, we're subscribing
				// to some owning grid's events in the OnDataGridViewChanged event which means
				// clones will subscribe to those events, even if the clone isn't added to a grid.
				// Normally that's not bad since I don't go explicitly making clones of rows. But
				// it turns out the following example will cause a row to be cloned.
				// Suppose most of the rows in the grid are of type PaCacheGridRow but a few
				// are of type SilHierarchicalGridRow. In the following example, when
				// grid.Row[i] is of type SilHierarchicalGridRow and the following code is
				// executed: PaCacheGridRow row = grid.Row[i] as PaCacheGridRow;
				// row is returned as null (which is correct) but in the process, grid.Row[i]
				// is cloned. Aaaaaaaahhhhhhh!!!! What's that all about!!!??? Why in the world
				// would grid.Row[i] be cloned? Normally, I assume the clone would be disposed
				// faily quickly except that in my OnDataGridViewChanged I subscribe to the
				// the owning grid's events which will cause the clone to not get disposed
				// because it's hanging on to some subscriptions. The example code above is
				// executed many times which means there will be gobbs of clones hanging around
				// and each clone's subscribed events will be fired unecessarily. Arrgh!!!
				// This check is designed to work around the problem.
				if (m_subscribeToOwnersEventsOnNextClone)
				{
					row.SubscribeToOwningGridEvents();
					m_subscribeToOwnersEventsOnNextClone = false;
				}
			}

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SubscribeToOwningGridEvents()
		{
			if (m_owningGrid != null)
			{
				m_owningGrid.MouseClick += m_owningGrid_MouseClick;
				m_owningGrid.CellDoubleClick += m_owningGrid_CellDoubleClick;
				m_owningGrid.RowPostPaint += m_owningGrid_RowPostPaint;
				m_owningGrid.SizeChanged += m_owningGrid_SizeChanged;
				m_owningGrid.Scroll += m_owningGrid_Scroll;
				m_owningGrid.KeyDown += m_owningGrid_KeyDown;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UnsubscribeToOwningGridEvents()
		{
			if (m_owningGrid != null)
			{
				m_owningGrid.MouseClick -= m_owningGrid_MouseClick;
				m_owningGrid.CellDoubleClick -= m_owningGrid_CellDoubleClick;
				m_owningGrid.RowPostPaint -= m_owningGrid_RowPostPaint;
				m_owningGrid.SizeChanged -= m_owningGrid_SizeChanged;
				m_owningGrid.Scroll -= m_owningGrid_Scroll;
				m_owningGrid.KeyDown -= m_owningGrid_KeyDown;
			}
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the row's text
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Text
		{
			get { return m_text; }
			set { m_text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font used to draw the row's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Font Font
		{
			get
			{
				if (m_font == null)
				{
					m_font = FontHelper.MakeFont((m_owningGrid == null ||
						m_owningGrid.DefaultCellStyle.Font == null ? SystemInformation.MenuFont :
						m_owningGrid.DefaultCellStyle.Font), FontStyle.Bold);
				}

				return m_font;
			}
			set { m_font = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the hierarchical level of the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Level
		{
			get { return m_level; }
			set
			{
				m_level = value;
				if (m_owningGrid != null && Index >= 0)
					m_owningGrid.InvalidateRow(Index);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets an index of a row in a cache corresponding to the first child row that
		/// is subordinate to this row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int FirstCacheIndex
		{
			get { return m_firstCacheIndex; }
			set { m_firstCacheIndex = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets an index of a row in a cache corresponding to the last child row that
		/// is subordinate to this row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int LastCacheIndex
		{
			get { return m_lastCacheIndex; }
			set { m_lastCacheIndex = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the number of child items subordinate to the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int ChildCount
		{
			get { return m_childCount; }
			set { m_childCount = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The format strings for displaying count information at the far right of the
		/// row. There must be three strings: a long version, medium version and a short
		/// version. The format strings in the string array are assumed to be in that order.
		/// When there is enough room, the long will always be used. An example of the three
		/// would be:
		/// 
		/// Long: "({0} records)"
		/// Med.: "({0} rec.)"
		/// Short: "({0})"
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] CountFormatStrings
		{
			get
			{
				if (m_recCountFmt == null || m_recCountFmt.Length != 3)
				{
					m_recCountFmt = new string[] {
						Resources.kstidHierarchicalRowChildCountLongFmt,
						Resources.kstidHierarchicalRowChildCountMedFmt,
						Resources.kstidHierarchicalRowChildCountShortFmt};
				}
				
				return m_recCountFmt;
			}
			set
			{
				if (value.Length == 3)
					m_recCountFmt = value;
			}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets or sets the collection of rows belonging to this parent row.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public List<DataGridViewRow> ChildRows
		//{
		//    get { return m_childRows; }
		//    set
		//    {
		//        m_childRows = (value == null ? new List<DataGridViewRow>() : value);

		//        if (m_childRows.Count > 0 && m_owningGrid != null)
		//        {
		//            // Remove this row and insert it above the first child row.
		//            if (m_owningGrid.Rows.Contains(this as DataGridViewRow))
		//                m_owningGrid.Rows.Remove(this as DataGridViewRow);

		//            m_owningGrid.Rows.Insert(m_childRows[0].Index, this as DataGridViewRow);
		//        }
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle for the + or - glyph in the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Rectangle GlyphRectangle
		{
			get
			{
				if (m_owningGrid == null || Index < 0 || Cells.Count <= m_level ||
					!(m_owningGrid.Columns[m_level] is SilHierarchicalGridColumn))
				{
					return Rectangle.Empty;
				}

				Rectangle rc = m_owningGrid.GetCellDisplayRectangle(m_level, Index, false);
				Rectangle rcGlyph = new Rectangle(0, 0, m_glyphWidth, m_glyphHeight);
				rcGlyph.X = rc.X + ((rc.Width - m_glyphWidth) / 2);
				rcGlyph.Y = rc.Y + ((rc.Height - m_glyphHeight) / 2);
				return rcGlyph;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Given an absolute row index of a row in the grid, this method will calculate what
		/// cache entry index the row corresponds to. It is assumed the row passed to this
		/// method comes after this row in the grid and before the next SilHierarchical row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetCacheIndex(int gridRow)
		{
			int delta = gridRow - Index - 1;
			int cacheIndex = m_firstCacheIndex + delta;
			return (cacheIndex < m_firstCacheIndex || cacheIndex > m_lastCacheIndex ? -1 : cacheIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the row's child rows are visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Expanded
		{
			get { return m_expanded; }
			set {SetExpandedState(value, false);}
		}

		#endregion

		#region Public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetExpandedState(bool expanded, bool force)
		{
			SetExpandedState(expanded, force, true);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetExpandedState(bool expanded, bool force, bool suspendAndResumeLayout)
		{
			if (m_expanded == expanded && !force)
				return;

			m_expanded = expanded;

			if (m_owningGrid == null || Index == m_owningGrid.Rows.Count - 1)
				return;

			if (suspendAndResumeLayout)
				m_owningGrid.SuspendLayout();

			// Make hide or unhide all rows between this one and the next SilHierarchicalGridRow
			// at the same level or higher or the end of the list, whatever comes first.
			for (int i = Index + 1; i < m_owningGrid.Rows.Count; i++)
			{
				SilHierarchicalGridRow row = m_owningGrid.Rows[i] as SilHierarchicalGridRow;

				if (row != null && row.Level <= m_level)
					break;

				m_owningGrid.Rows[i].Visible = m_expanded;
			}

			if (ExpandedStateChanged != null)
				ExpandedStateChanged(this);

			if (suspendAndResumeLayout)
				m_owningGrid.ResumeLayout();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsRowOwned(int childIndex)
		{
			if (m_owningGrid == null || Index == m_owningGrid.Rows.Count - 1 ||
				childIndex <= Index)
			{
				return false;
			}

			// Make hide or unhide all rows between this one and the next SilHierarchicalGridRow
			// at the same level or higher or the end of the list, whatever comes first.
			for (int i = Index + 1; i < m_owningGrid.Rows.Count; i++)
			{
				if (m_owningGrid.Rows[i] is SilHierarchicalGridRow)
					return false;

				if (i == childIndex)
					return true;
			}
			return false;
		}
		#endregion

		#region Owning grid events and painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cause Alt+Right, + or = to expand collapsed groups and Alt+Left, - or _ to
		/// collapse expanded groups.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_owningGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_owningGrid.CurrentRow == null || m_owningGrid.CurrentRow.Index != Index)
				return;

			bool toggle = (Expanded ?
				(e.KeyCode == Keys.OemMinus ||	(!e.Shift && e.KeyCode == Keys.Left)) :
				(e.KeyCode == Keys.Oemplus || (!e.Shift && e.KeyCode == Keys.Right)));

			if (toggle)
			{
				Expanded = !Expanded;
				e.Handled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if the + or - glyph was clicked upon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_owningGrid_MouseClick(object sender, MouseEventArgs e)
		{
			if (GlyphRectangle.Contains(e.X, e.Y))
				Expanded = !Expanded;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle expansion when the user double-clicks on the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_owningGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == Index)
				Expanded = !Expanded;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the owning grid was resized, redraw the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_owningGrid_SizeChanged(object sender, EventArgs e)
		{
			if (Index >= 0)
				m_owningGrid.InvalidateRow(Index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user scrolled the grid horizontally, redraw the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_owningGrid_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll && Index >= 0)
				m_owningGrid.InvalidateRow(Index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the row ourselves.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_owningGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			if (e.RowIndex != Index)
				return;

			Rectangle rcHighlight;
			Rectangle rcText;
			GetRectangles(e.RowBounds, out rcHighlight, out rcText);
			DrawHighlight(e.Graphics, ref rcHighlight, ref rcText);
			DrawGlyph(e.Graphics);
			DrawText(e.Graphics, ref rcText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the highlighted row in the proper color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawHighlight(Graphics g, ref Rectangle rcHighlight, ref Rectangle rcText)
		{
			if (Selected)
			{
				using (SolidBrush br = new SolidBrush(m_owningGrid.DefaultCellStyle.SelectionBackColor))
					g.FillRectangle(br, rcHighlight);
			}
			else
			{
				Fill(g, rcHighlight, m_owningGrid);
				Rectangle rc = rcText;
				rc.Width++;
				rc.X--;
				Fill(g, rc, m_owningGrid, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the text in the row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawText(Graphics g, ref Rectangle rcText)
		{
			// Prepare to draw the text.
			Color clr = (Selected ? m_owningGrid.DefaultCellStyle.SelectionForeColor :
				m_owningGrid.ForeColor);

			TextFormatFlags flags = TextFormatFlags.EndEllipsis |
				TextFormatFlags.LeftAndRightPadding | TextFormatFlags.SingleLine |
				TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping;

			if (!string.IsNullOrEmpty(m_text))
				TextRenderer.DrawText(g, m_text, m_font, rcText, clr, flags);

			// Should we draw the record count string?
			if (m_childCount == 0)
				return;

			// Shrink the rectangle to what's left after drawing the text.
			int textWidth = TextRenderer.MeasureText(g, m_text, m_font, Size.Empty,
				flags &= ~TextFormatFlags.EndEllipsis).Width;
			Rectangle rcRecCount = rcText;
			rcRecCount.Width -= textWidth;
			rcRecCount.X += textWidth;

			string recCount = null;

			// Don't want end ellipsis when measuring text. It will mess up the measurment.
			TextFormatFlags measureflags = (flags & ~TextFormatFlags.EndEllipsis);

			// Try three different format strings for drawing the record count, using
			// the longest one that will fit in the remaining room.
			for (int i = 0; i < 3 && recCount == null; i++)
			{
				recCount = string.Format(m_recCountFmt[i], m_childCount);
				if (TextRenderer.MeasureText(g, recCount, SystemInformation.MenuFont, Size.Empty,
					measureflags).Width > rcRecCount.Width)
				{
					recCount = null;
				}
			}

			if (recCount != null)
			{
				TextRenderer.DrawText(g, recCount, SystemInformation.MenuFont, rcRecCount, clr,
					flags | TextFormatFlags.Right);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangles for the selection and the text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetRectangles(Rectangle rcRow, out Rectangle rcHighlight,
			out Rectangle rcText)
		{
			if (m_hierarchicalColWidths < 0)
			{
				foreach (DataGridViewColumn col in m_owningGrid.Columns)
				{
					if (col is SilHierarchicalGridColumn)
						m_hierarchicalColWidths += col.Width;
				}
			}

			// Get the total width of all the visible columns.
			int visibleColWidths =
				m_owningGrid.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - 1;

			// Get the total width of the grid's client width less row headers and vertical scroll bar.
			int clientWidth = m_owningGrid.ClientSize.Width -
				(m_owningGrid.RowHeadersVisible ? m_owningGrid.RowHeadersWidth : 0) -
				(OwningGridsVerticalScrollBarVisible() ? SystemInformation.VerticalScrollBarWidth : 0);
			
			// Determine the background's rectangle.
			rcHighlight = rcRow;
			rcHighlight.Height--;
			rcHighlight.Width = Math.Min(clientWidth, visibleColWidths);

			if (m_owningGrid.RowHeadersVisible)
				rcHighlight.X += m_owningGrid.RowHeadersWidth;

			// Determine the text's rectangle.
			rcText = rcHighlight;

			// To get the text's rectangle shrink it so it doesn't include any
			// PaHierarchicalGridColumns.
			for (int i = 0; i <= m_level; i++)
			{
				int colWidth = m_owningGrid.Columns[i].Width;
				rcText.X += colWidth;
				rcText.Width -= colWidth;

				if (i < m_level)
				{
					rcHighlight.X += colWidth;
					rcHighlight.Width -= colWidth;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the + or - glyph.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawGlyph(Graphics graphics)
		{
			Image glyph = (m_expanded ? Resources.kimidCollapse :
				Resources.kimidExpand);

			graphics.DrawImage(glyph, GlyphRectangle);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Fill(Graphics g, Rectangle rc, DataGridView grid)
		{
			Fill(g, rc, grid, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Fill(Graphics g, Rectangle rc, DataGridView grid, bool useGradient)
		{
			Color clr = grid.BackgroundColor;
			Color clrLeft = Color.FromArgb(clr.R - 30, clr.G - 30, clr.B - 30);
			Color clrRight = Color.FromArgb(clr.R - 18, clr.G - 18, clr.B - 18);

			if (useGradient)
			{
				using (LinearGradientBrush br = new LinearGradientBrush(rc, clrLeft, clrRight, 0f))
					g.FillRectangle(br, rc);

				// There's a bug in the gradient fill that sometimes occurs (haven't been
				// able to determine what causes it) in which the first column of pixels
				// in the filled rectangle has the opposite color, of the two gradient colors,
				// from the one it should. Therefore, refill that column with the proper
				// color of the gradient by shrinking the rectangle's width and letting
				// the following paint occur.
				rc.Width = 2;
			}
				
			using (SolidBrush br = new SolidBrush(clrLeft))
				g.FillRectangle(br, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the vertical scrollbar is visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool OwningGridsVerticalScrollBarVisible()
		{
			if (m_owningGrid.ScrollBars == ScrollBars.None ||
				m_owningGrid.ScrollBars == ScrollBars.Horizontal)
			{
				return false;
			}

			int index = m_owningGrid.Rows.GetLastRow(DataGridViewElementStates.Visible);
			Rectangle rcUnclipped = m_owningGrid.GetRowDisplayRectangle(index, false);
			if (rcUnclipped == Rectangle.Empty)
				return true;
			
			Rectangle rcClipped = m_owningGrid.GetRowDisplayRectangle(index, true);
			if (rcUnclipped.Bottom > rcClipped.Bottom)
				return true;

			index = m_owningGrid.Rows.GetFirstRow(DataGridViewElementStates.Visible);
			rcUnclipped = m_owningGrid.GetRowDisplayRectangle(index, false);
			rcClipped = m_owningGrid.GetRowDisplayRectangle(index, true);
			return (rcUnclipped.Top < rcClipped.Top);
		}

		#endregion
	}

	#endregion

	#region SilHierarchicalGridColumn class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SilHierarchicalGridColumn : DataGridViewColumn
	{
		private int m_level = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilHierarchicalGridColumn() : base(new SilHierarchicalGridCell())
		{
			base.ReadOnly = true;
			base.Frozen = true;
			SortMode = DataGridViewColumnSortMode.NotSortable;
			AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			Width = Resources.kimidExpand.Width + 8;
			base.Resizable = DataGridViewTriState.False;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override object Clone()
		{
			object col = base.Clone();
			if (col is SilHierarchicalGridColumn)
				((SilHierarchicalGridColumn)col).m_level = m_level;

			return col;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the template is always a SilHierarchicalGridCell cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override DataGridViewCell CellTemplate
		{
			get { return base.CellTemplate; }
			set
			{
				if (value != null && !value.GetType().IsAssignableFrom(typeof(SilHierarchicalGridCell)))
					throw new InvalidCastException("Must be a PaHierarchicalGridCell");

				base.CellTemplate = value;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Level
		{
			get { return m_level; }
			set { m_level = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows or Hides all the SilHierarchicalGridColumns in the specified grid,
		/// suspending the grid's layout beforehand, if specified to do so, and resuming the
		/// grid's layout afterward, if specified to do so.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShowHierarchicalColumns(DataGridView grid, bool show,
			bool suspendLayout, bool resumeLayout)
		{
			if (grid == null)
				return;

			if (suspendLayout)
				grid.SuspendLayout();

			foreach (DataGridViewColumn col in grid.Columns)
			{
				if (col.Visible != show && col is SilHierarchicalGridColumn)
				{
					if (show)
						col.DisplayIndex = 0;

					col.Visible = show;
				}
			}

			if (resumeLayout)
				grid.ResumeLayout();
		}
	}

	#endregion

	#region SilHierarchicalGridCell class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SilHierarchicalGridCell : DataGridViewTextBoxCell
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't allow the cell to have the selection color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Paint(Graphics graphics, Rectangle clipBounds,
			Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
			object value, object formattedValue, string errorText,
			DataGridViewCellStyle cellStyle,
			DataGridViewAdvancedBorderStyle advancedBorderStyle,
			DataGridViewPaintParts paintParts)
		{
			SilHierarchicalGridRow.Fill(graphics, cellBounds, DataGridView);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't let the cell become current.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(int rowIndex, bool throughMouseClick)
		{
			base.OnEnter(rowIndex, throughMouseClick);

			try
			{
				// I hate moving off this cell this way, but I have no choice. When I try to set
				// the owning grid's current cell, I get a reentrant exception. Grr!
				SendKeys.Send("+{Right}");
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get rid of all borders except the bottom.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override DataGridViewAdvancedBorderStyle AdjustCellBorderStyle(
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput,
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceHolder,
			bool singleVerticalBorderAdded,	bool singleHorizontalBorderAdded,
			bool firstVisibleColumn, bool firstVisibleRow)
		{
			dataGridViewAdvancedBorderStylePlaceHolder.Left =
				DataGridViewAdvancedCellBorderStyle.None;

			bool isRowPHGR = (RowIndex > -1 && DataGridView.Rows[RowIndex] is SilHierarchicalGridRow);

			// Only allow a right border if we're the furthest right
			// hierarchical column and we're not in a hierarchical row.
			if (!isRowPHGR)
			{
				dataGridViewAdvancedBorderStylePlaceHolder.Right =
					dataGridViewAdvancedBorderStyleInput.Right;
			}
			else
			{
				dataGridViewAdvancedBorderStylePlaceHolder.Right =
					DataGridViewAdvancedCellBorderStyle.None;
			}

			dataGridViewAdvancedBorderStylePlaceHolder.Bottom =
				DataGridViewAdvancedCellBorderStyle.None;

			dataGridViewAdvancedBorderStylePlaceHolder.Top =
				DataGridViewAdvancedCellBorderStyle.None;

			return dataGridViewAdvancedBorderStylePlaceHolder;
		}
	}

	#endregion
}
