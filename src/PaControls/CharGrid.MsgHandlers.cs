using System.Windows.Forms;
using System.Xml;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CharGrid
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When a header is clicked, then make sure the one clicked becomes current and
		/// that the current cell tracks with it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCharGridHeaderClicked(object args)
		{
			CharGridHeader hdr = args as CharGridHeader;
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || hdr == null)
				return false;

			if (hdr.IsForColumnHeadings)
			{
				if (hdr != m_currentColHeader)
				{
					int currRow = m_grid.CurrentCell.RowIndex;
					m_grid.CurrentCell = m_grid[hdr.OwnedColumns[0].Index, currRow];
				}
			}
			else
			{
				if (hdr != m_currentRowHeader)
				{
					int currCol = m_grid.CurrentCell.ColumnIndex;
					m_grid.CurrentCell = m_grid[currCol, hdr.OwnedRows[0].Index];
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Popup a context menu when a header is right-clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCharGridHeaderRightClicked(object args)
		{
			m_currentHeader = args as CharGridHeader;
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			string menu = (m_currentHeader.IsForColumnHeadings ?
				"cmnuCharChartColHeader" : "cmnuCharChartRowHeader");

			m_tmAdapter.PopupMenu(menu, MousePosition.X, MousePosition.Y);
			return true;
		}

		#region Message for editing heading labels
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Put the user in the edit mode for column header labels.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnEditCharChartLabel(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			m_currentHeader.EditLabel();
			return true;
		}

		#endregion

		#region Message/update handlers for showing sub headings	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle the visible state of a row heading's sub-headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowChartRowSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnShowCharChartSubHeadings(args);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle the visible state of a column heading's sub-headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowChartColSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnShowCharChartSubHeadings(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle the visible state of a heading's sub-headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowCharChartSubHeadings(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			m_currentHeader.SubHeadingsVisible = !m_currentHeader.SubHeadingsVisible;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the "Show sub-headings" menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowChartRowSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnUpdateShowCharChartSubHeadings(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the "Show sub-headings" menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowChartColSubHeadingsTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnUpdateShowCharChartSubHeadings(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the "Show sub-headings" menu.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCharChartSubHeadings(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;

			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				m_currentHeader == null || itemProps == null)
			{
				return false;
			}

			int ownedCount = (m_currentHeader.IsForColumnHeadings ?
				m_currentHeader.OwnedColumns.Count :
				m_currentHeader.OwnedRows.Count);

			itemProps.Visible = true;
			itemProps.Enabled = (ownedCount > 1);
			itemProps.Checked = m_currentHeader.SubHeadingsVisible;
			itemProps.Update = true;
			return true;
		}

		#endregion
		
		#region Methods for adding new rows
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row above the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowBeforeTBMenu(object args)
		{
			return AddNewRow(true);
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row above the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowBefore(object args)
		{
			return AddNewRow(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row below the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowAfterTBMenu(object args)
		{
			return AddNewRow(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row below the current row and in the same header as the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowAfter(object args)
		{
			return AddNewRow(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new row to the grid (and to the current row's header) either before
		/// or after the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AddNewRow(bool beforeCurrRow)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				m_grid.CurrentCell == null || m_grid.CurrentCell.RowIndex < 0)
			{
				return false;
			}

			int currRowIndex = m_grid.CurrentCell.RowIndex;
			DataGridViewRow currRow = m_grid.Rows[currRowIndex];
			if (currRow.Tag == null || currRow.Tag.GetType() != typeof(CharGridHeader))
				return false;

			if (!beforeCurrRow)
				currRowIndex++;

			DataGridViewRow newRow = new DataGridViewRow();
			newRow.Height = kCellHeight;
			m_grid.Rows.Insert(currRowIndex, newRow);
			((CharGridHeader)currRow.Tag).AddRow(m_grid.Rows[currRowIndex]);
			CalcHeights();
			return true;
		}

		#endregion

		#region Methods for adding new columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added before the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColBeforeTBMenu(object args)
		{
			return AddNewColumn(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added after the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColAfterTBMenu(object args)
		{
			return AddNewColumn(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added before the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColBefore(object args)
		{
			return AddNewColumn(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column below the header that owns the current column. The new column
		/// is added after the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColAfter(object args)
		{
			return AddNewColumn(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a new column to the grid (and to the current column's header) either before
		/// or after the current column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool AddNewColumn(bool beforeCurrColumn)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				m_grid.CurrentCell == null || m_grid.CurrentCell.ColumnIndex < 0)
			{
				return false;
			}

			int currColIndex = m_grid.CurrentCell.ColumnIndex;
			DataGridViewColumn currCol = m_grid.Columns[currColIndex];
			if (currCol.Tag == null || currCol.Tag.GetType() != typeof(CharGridHeader))
				return false;

			DataGridViewColumn newCol = CreateColumn();
			((CharGridHeader)currCol.Tag).AddColumn(newCol);
			CalcWidths();
			m_grid.Columns.Insert(currColIndex + (beforeCurrColumn ? 0 : 1), newCol);
			return true;
		}

		#endregion

		#region Methods for adding new row headings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowHeadingBeforeTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnAddCharChartRowHeadingBefore(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowHeadingBefore(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertRowHeader(true, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartRowHeadingAfterTBMenu(object args)
		{
			m_currentHeader = m_currentRowHeader;
			return OnAddCharChartRowHeadingAfter(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartRowHeadingAfter(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertRowHeader(false, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts a row header before the specified header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader InsertRowHeaderBefore(CharGridHeader hdr)
		{
			if (hdr != null)
			{
				m_currentHeader = hdr;
				return InsertRowHeader(true, false);
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CharGridHeader InsertRowHeader(bool beforeClickedOnRow, bool editLabel)
		{
			if (m_currentHeader != null)
				m_currentHeader.EndEditLabel();

			// Get the index of where in the grid we're going to
			// insert the new header's first grid column.
			int newGridRowIndex = (beforeClickedOnRow ?
				m_currentHeader.OwnedRows[0].Index :
				m_currentHeader.LastRow.Index + 1);

			// Get the index of where in the header collection we'll insert the new header.
			int newHdrRowIndex = m_rowHdrs.IndexOf(m_currentHeader);
			if (!beforeClickedOnRow)
				newHdrRowIndex++;

			CharGridHeader newHdr = CreateRowHeader(string.Empty, newHdrRowIndex);
			DataGridViewRow newRow = new DataGridViewRow();
			newRow.Height = kCellHeight;

			m_grid.Rows.Insert(newGridRowIndex, newRow);
			newHdr.AddRow(m_grid.Rows[newGridRowIndex]);
			m_grid.CurrentCell = m_grid[0, newGridRowIndex];
			
			if (editLabel)
				newHdr.EditLabel();
	
			return newHdr;
		}

		#endregion

		#region Methods for adding new column headings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColHeadingBeforeTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnAddCharChartColHeadingBefore(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColHeadingBefore(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertColumnHeader(true, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new row heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddChartColHeadingAfterTBMenu(object args)
		{
			m_currentHeader = m_currentColHeader;
			return OnAddCharChartColHeadingAfter(null);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new column heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnAddCharChartColHeadingAfter(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			InsertColumnHeader(false, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts a column header before the specified header.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeader InsertColumnHeaderBefore(CharGridHeader hdr)
		{
			if (hdr != null)
			{
				m_currentHeader = hdr;
				return InsertColumnHeader(true, false);
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CharGridHeader InsertColumnHeader(bool insertBeforeCurr, bool editLabel)
		{
			if (m_currentHeader != null)
				m_currentHeader.EndEditLabel();

			// Get the index of where in the grid we're going to
			// insert the new header's first grid column.
			int newGridColIndex = (insertBeforeCurr ?
				m_currentHeader.OwnedColumns[0].Index :
				m_currentHeader.LastColumn.Index + 1);

			// Get the index of where in the header collection we'll insert the new header.
			int newHdrColIndex = m_colHdrs.IndexOf(m_currentHeader);
			if (!insertBeforeCurr)
				newHdrColIndex++;

			CharGridHeader newHdr = CreateColumnHeader(string.Empty, newHdrColIndex);
			DataGridViewColumn newCol = CreateColumn();

			newHdr.AddColumn(newCol);
			m_grid.Columns.Insert(newGridColIndex, newCol);
			m_grid.CurrentCell = m_grid[newCol.Index, 0];

			if (editLabel)
				newHdr.EditLabel();
	
			return newHdr;
		}

		#endregion

		#region Methods for removing rows and columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row from the grid (toolbar menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartRowTBMenu(object args)
		{
			return OnRemoveCharChartRow(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes row toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartRowTBMenu(object args)
		{
			return OnUpdateRemoveCharChartRow(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column from the grid (toolbar menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartColTBMenu(object args)
		{
			return OnRemoveCharChartCol(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes column toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartColTBMenu(object args)
		{
			return OnUpdateRemoveCharChartCol(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row from the grid (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartRow(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_grid.CurrentCell == null)
				return false;

			RemoveRow(m_grid.CurrentCell.RowIndex);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes row context menu item (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartRow(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				itemProps == null || m_grid.CurrentCell == null)
			{
				return false;
			}

			itemProps.Enabled = IsRowEmtpy(m_grid.CurrentCell.RowIndex);
			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column from the grid (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartCol(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_grid.CurrentCell == null)
				return false;

			RemoveColumn(m_grid.CurrentCell.ColumnIndex);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the removes column context menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartCol(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				itemProps == null || m_grid.CurrentCell == null)
			{
				return false;
			}

			itemProps.Enabled = IsColumnEmtpy(m_grid.CurrentCell.ColumnIndex);
			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not the specified row is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsRowEmtpy(int rowIndex)
		{
			for (int i = 0; i < m_grid.Columns.Count; i++)
			{
				CharGridCell cgc = m_grid[i, rowIndex].Value as CharGridCell;
				if (cgc != null && cgc.Visible && !string.IsNullOrEmpty(cgc.Phone))
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not the specified column is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsColumnEmtpy(int colIndex)
		{
			for (int i = 0; i < m_grid.Rows.Count; i++)
			{
				CharGridCell cgc = m_grid[colIndex, i].Value as CharGridCell;
				if (cgc != null && cgc.Visible && !string.IsNullOrEmpty(cgc.Phone))
					return false;
			}

			return true;
		}

		#endregion

		#region Message handlers for removing all empty rows and columns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all empty rows and columns in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveAllEmptyChartRowsAndColsTBMenu(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()))
				return false;

			RemoveAllEmptyRowsAndColumns();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the toolbar menu item for removing all empty rows and columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveAllEmptyChartRowsAndColsTBMenu(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || itemProps == null)
				return false;

			itemProps.Enabled = false;

			// Check if any row headers have empty rows.
			foreach (CharGridHeader hdr in m_rowHdrs)
			{
				if (hdr.IsAnyOwnedRowEmpty)
				{
					itemProps.Enabled = true;
					break;
				}
			}

			// If none of the row headers had any empty rows, then
			// check if any columns headers have empty columns.
			if (!itemProps.Enabled)
			{
				foreach (CharGridHeader hdr in m_colHdrs)
				{
					if (hdr.IsAnyOwnedColumnEmpty)
					{
						itemProps.Enabled = true;
						break;
					}
				}
			}

			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		#endregion

		#region Message handlers and update handlers for removing row and column headings.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row heading and all its grid rows (toolbar button menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartRowHeadingTBMenu(object args)
		{
			m_currentHeader = (m_grid.CurrentCell == null ? null :
				GetRowsHeader(m_grid.CurrentCell.RowIndex));

			return OnRemoveCharChartRowHeading(args);
		}		

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove row heading for current cell toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartRowHeadingTBMenu(object args)
		{
			m_currentHeader = (m_grid.CurrentCell == null ? null :
				GetRowsHeader(m_grid.CurrentCell.RowIndex));

			return OnUpdateRemoveCharChartRowHeading(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a row heading and all its grid rows (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartRowHeading(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			RemoveRowHeader(m_currentHeader);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove row heading context menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartRowHeading(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				itemProps == null || m_currentHeader == null)
			{
				return false;
			}

			itemProps.Visible = true;
			itemProps.Enabled = m_currentHeader.AreAllOwnedRowsEmpty;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column heading and all its grid columns (toolbar item menu).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveChartColHeadingTBMenu(object args)
		{
			m_currentHeader = (m_grid.CurrentCell == null ? null :
				GetColumnsHeader(m_grid.CurrentCell.ColumnIndex));

			return OnRemoveCharChartColHeading(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove column heading for current cell toolbar menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveChartColHeadingTBMenu(object args)
		{
			m_currentHeader = (m_grid.CurrentCell == null ? null :
				GetColumnsHeader(m_grid.CurrentCell.ColumnIndex));

			return OnUpdateRemoveCharChartColHeading(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes a column heading and all its grid columns (context menu item).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemoveCharChartColHeading(object args)
		{
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) || m_currentHeader == null)
				return false;

			RemoveColumnHeader(m_currentHeader);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the remove row heading context menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemoveCharChartColHeading(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsViewOrFormActive(m_owningViewType, FindForm()) ||
				itemProps == null || m_currentHeader == null)
			{
				return false;
			}

			itemProps.Visible = true;
			itemProps.Enabled = m_currentHeader.AreAllOwnedColumnsEmpty;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves a row down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMoveCharChartRowDown(object args)
		{
			//if (!PaApp.IsFormActive(FindForm()) || m_currentHeader.GetType() != typeof(int))
			//    return false;

			//SwapRows((int)m_currentHeader, (int)m_currentHeader + 1);
			return true;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the move row down option should be enabled. Only enable
		/// when the row heading clicked is not the last row in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateMoveCharChartRowDown(object args)
		{
			//TMItemProperties itemProps = args as TMItemProperties;
			//if (!PaApp.IsFormActive(FindForm()) || itemProps == null)
			//    return false;

			//itemProps.Update = true;

			//if (m_currentHeader.GetType() != typeof(int))
			//    itemProps.Enabled = false;
			//else
			//{
			//    int colIndex = (int)m_currentHeader;
			//    itemProps.Enabled = (colIndex < m_grid.Rows.Count - 2);
			//}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves a row up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMoveCharChartRowUp(object args)
		{
			//if (!PaApp.IsFormActive(FindForm()) || m_currentHeader.GetType() != typeof(int))
			//    return false;

			//SwapRows((int)m_currentHeader, (int)m_currentHeader - 1);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the move row up option should be enabled. Only enable
		/// when the row heading clicked is not the first row in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateMoveCharChartRowUp(object args)
		{
			//TMItemProperties itemProps = args as TMItemProperties;
			//if (!PaApp.IsFormActive(FindForm()) || itemProps == null)
			//    return false;

			//itemProps.Update = true;

			//if (m_currentHeader.GetType() != typeof(int))
			//    itemProps.Enabled = false;
			//else
			//{
			//    int colIndex = (int)m_currentHeader;
			//    itemProps.Enabled = (colIndex > 0);
			//}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			foreach (DataGridViewColumn col in m_grid.Columns)
				col.DefaultCellStyle.Font = m_chartFont;

			// Return false to allow other windows to update their fonts.
			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Swaps the contents of the specified rows.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SwapRows(int row1, int row2)
		{
			// Swap the values in the row we're moving down with the
			// values in the row below it.
			for (int i = 0; i < m_grid.Columns.Count; i++)
			{
				string tmpPhone = m_grid[i, row2].Value as string;
				m_grid[i, row2].Value = m_grid[i, row1].Value;
				m_grid[i, row1].Value = tmpPhone;
			}

			// Now swap the row heading labels.
			//string tmpLabel = m_rowHdrTexts[row2];
			//m_rowHdrTexts[row2] = m_rowHdrTexts[row1];
			//m_rowHdrTexts[row1] = tmpLabel;
			m_grid.Refresh();
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}
}
