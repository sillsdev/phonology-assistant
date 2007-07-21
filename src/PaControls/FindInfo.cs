using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	public partial class FindInfo
	{
		#region Member Variables
		// Declare member variables
		private static PaWordListGrid m_grid;
		private static bool m_firstMatch = false;
		private static int m_firstMatchedRow = 0;
		private static int m_firstMatchedCol = 0;
		private static int m_matchedRow = 0;
		private static int m_matchedColumn = 0;
		private static string m_findPattern = string.Empty;
		private static string m_findText = string.Empty;
		private static FindDlgColItem[] m_colsToSearch = new FindDlgColItem[] { };
		private static int m_startRow = 0;
		private static int m_iPreviousRow = 0;
		private static int m_iRow = 0;
		private static int m_iColumn = 0;
		private static bool m_reverseFind = false;
		private static bool m_doneFinding = false;
		private static bool m_performedFind = false;
		private static bool m_firstLoop = true;
		private static bool m_findDlgIsOpen = false;
		private static bool m_searchedAllRowCols = false;
		private static int m_numberOfFinds = 0;
		private static bool m_changedFindDirection = false;
		private static bool m_showMessages = true;
		private static bool m_searchCollapsedGroups = false;
		private static bool m_findBackwards = false;
		private static SilHierarchicalGridRow m_silHierarchicalGridRow;

		#endregion

		#region Finding
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the column indexes, because Group by Sorted Field and Minimal Pairs
		/// have an extra column at the beginning.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UpdateColumnIndexes()
		{
			// Update the column indexes
			List<FindDlgColItem> columnsToSearch = new List<FindDlgColItem>();
			foreach (FindDlgColItem dlgColItem in ColumnsToSearch)
			{
				FindDlgColItem item = new FindDlgColItem(
					m_grid.Columns[dlgColItem.FieldName].Index,
					dlgColItem.DisplayIndex,
					dlgColItem.ToString(), dlgColItem.FieldName);

				if (item != null)
					columnsToSearch.Add(item);
			}
			ColumnsToSearch = columnsToSearch.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resets the current cell information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ResetStartSearchCell(bool updateColIndexes)
		{
			if (updateColIndexes)
				UpdateColumnIndexes();

			m_firstMatch = false;
			m_firstLoop = true;
			m_doneFinding = false;
			// Start searching from the current row & column
			if (m_grid != null && m_grid.CurrentCell != null)
			{
				SetCurrentRow(m_grid.CurrentCell.RowIndex);
				SetCurrentColumn(m_grid.CurrentCell.ColumnIndex);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate the column's sort position.
		/// </summary>
		/// <returns>The column's sort position</returns>
		/// ------------------------------------------------------------------------------------
		private static int GetColumnSortPosition(int currCellDisplayIndex)
		{
			for (int i = 0; i < m_colsToSearch.Length; i++)
			{
				if (m_colsToSearch[i].DisplayIndex == currCellDisplayIndex)
					return i;
			}
			return 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate the start search column based on the cell the user clicked on.
		/// </summary>
		/// <returns>column display index to start searching from</returns>
		/// ------------------------------------------------------------------------------------
		private static int CalcStartSearchCol()
		{
			int currCellDisplayIndex = m_grid.Columns[m_grid.CurrentCell.ColumnIndex].DisplayIndex;

			if (m_grid.Cache.IsCIEList || m_grid.IsGroupedByField) // IsCIEList -> Minimal Pairs
			{
				// Covert currCellDisplayIndex to 0 based array for comparison
				currCellDisplayIndex--;
				if (currCellDisplayIndex < 0)
					currCellDisplayIndex = 0;
			}
			int currColumnSortPosition = GetColumnSortPosition(currCellDisplayIndex);

			if (m_reverseFind)
			{
				if (currCellDisplayIndex != 0)
				{
					for (int i = m_colsToSearch.Length - 1; i >= 0; i--)
					{
						if (m_doneFinding && m_grid.CurrentCell.RowIndex == m_iPreviousRow)
						{
							if (m_colsToSearch[i].DisplayIndex < currCellDisplayIndex)
								return SetCurrentColumn(i);
						}
						else if (m_doneFinding && m_grid.CurrentCell.RowIndex != m_iPreviousRow)
						{
							return SetCurrentColumn(m_colsToSearch.Length - 1);
						}
						else if (m_doneFinding && (currColumnSortPosition < m_colsToSearch.Length - 1))
						{
							if (m_colsToSearch[i].DisplayIndex <= currCellDisplayIndex)
								return SetCurrentColumn(i);
						}
						else
						{
							if (m_colsToSearch[i].DisplayIndex < currCellDisplayIndex)
								return SetCurrentColumn(i);
						}
					}
				}
				if (m_grid.CurrentCell.RowIndex > 0)
					SetCurrentRow(m_grid.CurrentCell.RowIndex - 1);
				else
					SetCurrentRow(m_grid.Rows.Count - 1); // last row becomes the current row
				SetCurrentColumn(m_colsToSearch.Length - 1);
			}
			else // FORWARD FIND
			{
				if (currColumnSortPosition != m_colsToSearch.Length - 1)
				{
					for (int i = 0; i < m_colsToSearch.Length; i++)
					{
						if (m_doneFinding && m_grid.CurrentCell.RowIndex == m_iPreviousRow)
						{
							if (m_colsToSearch[i].DisplayIndex > currCellDisplayIndex)
								return SetCurrentColumn(i);
						}
						else if (m_doneFinding && m_grid.CurrentCell.RowIndex != m_iPreviousRow)
						{
							return SetCurrentColumn(0);
						}
						else if (m_doneFinding && currColumnSortPosition > 0)
						{
							if (m_colsToSearch[i].DisplayIndex >= currCellDisplayIndex)
								return SetCurrentColumn(i);
						}
						else
						{
							if (m_colsToSearch[i].DisplayIndex > currCellDisplayIndex)
								return SetCurrentColumn(i);
						}
					}
				}
				if (m_grid.CurrentCell.RowIndex + 1 < m_grid.Rows.Count)
					SetCurrentRow(m_grid.CurrentCell.RowIndex + 1);
				else
					SetCurrentRow(0); // first row becomes the current row
				SetCurrentColumn(0);
			}
			return m_iColumn;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the current row value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetCurrentRow(int currentRow)
		{
			m_startRow = currentRow;
			m_iRow = currentRow;
			m_matchedRow = currentRow;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the current column value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static int SetCurrentColumn(int currentColumn)
		{
			m_iColumn = currentColumn;
			m_matchedColumn = currentColumn;
			return currentColumn;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches for first findPattern match with a cell beginning with the last
		/// matched row and column
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		public static bool FindFirst(bool findPrevious)
		{
			// 'Find Next' & 'Find Previous' enable when a Find has been executed
			m_performedFind = true;

			m_numberOfFinds = 0;
			ResetStartSearchCell(false);
			return Find(findPrevious);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Perform search backwards or forwards.
		/// This method is directly called by 'Find Next' & 'Find Previous'.
		/// </summary>
		/// <param name="findPrevious">bool</param>
		/// ------------------------------------------------------------------------------------
		public static bool Find(bool findPrevious)
		{
			// Has there been a change in the Find direction?
			if (findPrevious != m_reverseFind)
				m_changedFindDirection = true;
			else
				m_changedFindDirection = false;

			// Current cell will be null if its column is hidden thru the OptionsDlg>>WordList tab
			if (m_grid.CurrentCell == null)
				m_grid.CurrentCell = m_grid[0, 0];

			if (findPrevious)
			{
				m_reverseFind = true;
				// This occurs when the user finds a match on the last Find column in the row
				// with a 'Find Next' and then switches direction with a 'Find Prev'.
				if (m_changedFindDirection && m_searchedAllRowCols)
					m_matchedRow--; // back up the row by 1
				m_searchedAllRowCols = false;
				// This occurs when the user hits the end of the Find loop with a 'Find Next',
				// gets the info popup, and then switches direction with a 'Find Prev'.
				if (m_changedFindDirection && m_doneFinding)
				{
					// back up to row with last find match
					m_matchedRow = m_grid.CurrentCell.RowIndex;
					m_doneFinding = false; // so will set column to prev sort column
				}
				CalcStartSearchCol();
				return FindBackward();
			}
			else // FORWARD SEARCH
			{
				m_reverseFind = false;
				// This occurs when the user finds a match on the first Find column in the row
				// with a 'Find Prev' and then switches direction with a 'Find Next'.
				if (m_changedFindDirection && m_searchedAllRowCols)
					m_matchedRow++; // advance the row by 1
				m_searchedAllRowCols = false;
				// This occurs when the user hits the end of the Find loop with a 'Find Prev',
				// gets the info popup, and then switches direction with a 'Find Next'.
				if (m_changedFindDirection && m_doneFinding)
				{
					// advance to row with last find match
					m_matchedRow = m_grid.CurrentCell.RowIndex;
					m_doneFinding = false; // so will set column to next sort column
				}
				CalcStartSearchCol();
				return FindForward();
			}
		}
			
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches for first findPattern match with a cell beginning with the last
		/// matched row and column
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool FindForward()
		{
			m_findBackwards = false;

			// Start the Find over with the first record when reach the bottom.
			if (m_iRow == m_grid.Rows.Count)
				m_matchedRow = 0;

			for (m_iRow = m_matchedRow; m_iRow < m_grid.Rows.Count; m_iRow++)
			{
				if (DataNotFound())
				{
					m_firstLoop = true;
					return false;
				}

				// Save the hierarchical row for later expansion if needed (when searching collapsed recs)
				if (m_grid.Rows[m_iRow] is SilHierarchicalGridRow)
					m_silHierarchicalGridRow = m_grid.Rows[m_iRow] as SilHierarchicalGridRow;

				m_iPreviousRow = m_iRow;
				for (m_iColumn = m_matchedColumn; m_iColumn < m_colsToSearch.Length; m_iColumn++)
				{
					if (ProcessColumns())
						return true;
				}

				// Didn't find a match, so start searching again in the 1st column on the next row
				m_matchedRow = m_iRow + 1;
				SetCurrentColumn(0);

				// Restart the search from the top if there has been at least 1 match already
				if (m_matchedRow >= m_grid.Rows.Count)
				{
					m_iRow = -1;
					m_matchedRow = 0;
				}
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches for first findPattern match with a cell beginning with the last
		/// matched row and column
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool FindBackward()
		{
			m_findBackwards = true;

			// Start the Find over with the last record when reach the top.
			if (m_iRow < 0)
				m_matchedRow = m_grid.Rows.Count - 1;

			for (m_iRow = m_matchedRow; m_iRow >= 0; m_iRow--)
			{
				if (DataNotFound())
				{
					m_firstLoop = true;
					return false;
				}

				//// Don't search Hierarchical Row's
				//if (m_grid.Rows[m_iRow] is SilHierarchicalGridRow)
				//        continue;

				m_iPreviousRow = m_iRow;
				for ( m_iColumn = m_matchedColumn; m_iColumn >= 0; m_iColumn--)
				{
					if (ProcessColumns())
						return true;
				}

				// Didn't find a match, so start searching again in the last column on the previous row
				m_matchedRow = m_iRow - 1;
				SetCurrentColumn(m_colsToSearch.Length - 1);

				// Restart the search from the bottom if there has been at least 1 match already
				if (m_matchedRow < 0)
				{
					m_iRow = m_grid.Rows.Count;
					m_matchedRow = m_grid.Rows.Count - 1;
				}
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check for missing query.
		/// </summary>
		/// <returns>true if there is a match</returns>
		/// ------------------------------------------------------------------------------------
		private static bool DataNotFound()
		{
			if (!m_firstLoop)
			{
				if (m_iRow == m_startRow && !m_firstMatch)
				{
					if (m_showMessages)
					{
						STUtils.STMsgBox(
							Properties.Resources.kstidFindDataNotFound + m_findText, MessageBoxButtons.OK);
					}
					return true;
				}
			}
			m_firstLoop = false;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Process columns search.
		/// </summary>
		/// <returns>true if there is a match</returns>
		/// ------------------------------------------------------------------------------------
		private static bool ProcessColumns()
		{
			if (SearchCellDataForMatch(
				m_grid[m_colsToSearch[m_iColumn].ColIndex, m_grid.Rows[m_iRow].Index], m_findPattern))
			{
				Match();
				return true; // match
			}
			return false; // no match
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches backwards for the previous HierarchicalGridRow and expands it.
		/// </summary>
		/// <returns>true if a hierarchial grid row was expanded</returns>
		/// ------------------------------------------------------------------------------------
		private static bool ExpandPreviousHierarchicalGridRow()
		{
			for (int rowIndex = m_iRow; rowIndex >= 0; rowIndex--)
			{
				if (m_grid.Rows[rowIndex] is SilHierarchicalGridRow)
				{
					(m_grid.Rows[rowIndex] as SilHierarchicalGridRow).Expanded = true;
					return true;
				}
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Find the cell query for a match and reposition the matching cell's row to the middle.
		/// </summary>
		/// <param name="cell">DataGridViewCell</param>
		/// <param name="findPattern">string</param>
		/// <returns>true if there is a match</returns>
		/// ------------------------------------------------------------------------------------
		private static bool SearchCellDataForMatch(DataGridViewCell cell, string findPattern)
		{
			// There is NOT a match if the cell is NOT visible (i.e. group(s) collapsed) AND
			// searching collapsed groups AND (the grid is grouped on a sorted field OR minimal pairs).
			if (!cell.Visible && !m_searchCollapsedGroups &&
				(m_grid.IsGroupedByField || m_grid.Cache.IsCIEList))
			{
				return false;
			}

			string cellValue = cell.Value as string;
			if (cellValue == null)
				return false;

			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[m_grid.Columns[cell.ColumnIndex].Name];

			if (!fieldInfo.IsPhonetic)
				cellValue = FFNormalizer.Normalize(cellValue);

			try
			{
				if (!Regex.IsMatch(cellValue, findPattern))
					return false;

				if (DoneSearching(cell))
					return true;

				if (!cell.Visible && m_searchCollapsedGroups &&
					(m_grid.IsGroupedByField || m_grid.Cache.IsCIEList))
				{
					if (m_findBackwards)
						ExpandPreviousHierarchicalGridRow();
					else
					{
						// m_silHierarchicalGridRow will only be null when the user
						// Groups by Sorted Field in the middle of a Find and then
						// does a Find Next
						if (m_silHierarchicalGridRow == null)
							ExpandPreviousHierarchicalGridRow();
						else if (!m_silHierarchicalGridRow.Expanded)
							m_silHierarchicalGridRow.Expanded = true;
					}
				}

				m_numberOfFinds++;

				// If the cell is off the screen, move the cell's row to the screen's
				// middle. m_showMessages will only be false if running tests.
				if (!cell.Displayed && m_showMessages)
					m_grid.ScrollRowToMiddleOfGrid(cell.RowIndex);

				if (cell.Visible)
					m_grid.CurrentCell = cell;

				// Done searching
				return true;
			}
			catch (Exception ex)
			{
				if (m_showMessages)
					STUtils.STMsgBox(ex.Message, MessageBoxButtons.OK);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display message when done searching the records.
		/// </summary>
		/// <param name="cell">DataGridViewCell</param>
		/// <returns>true when done searching</returns>
		/// ------------------------------------------------------------------------------------
		private static bool DoneSearching(DataGridViewCell cell)
		{
			if (!m_firstMatch)
			{
				m_firstMatch = true;
				m_firstMatchedRow = cell.RowIndex;
				m_firstMatchedCol = cell.ColumnIndex;
				return false;
			}

			if (m_doneFinding)
			{
				m_doneFinding = false;
				if (m_numberOfFinds < 2)
				{
					// Display message that all records have been searched
					if (m_showMessages)
						STUtils.STMsgBox(Properties.Resources.kstidFindDoneSearching, MessageBoxButtons.OK);
					m_firstMatchedRow = cell.RowIndex;
					m_firstMatchedCol = cell.ColumnIndex;
				}
				m_numberOfFinds = 0;
				return false;
			}

			if (m_firstMatch && 
				cell.RowIndex == m_firstMatchedRow && 
				cell.ColumnIndex == m_firstMatchedCol)
			{
				if (!m_changedFindDirection)
				{
					m_doneFinding = true;
					// Display message that all records have been searched
					if (m_showMessages)
						STUtils.STMsgBox(Properties.Resources.kstidFindDoneSearching, MessageBoxButtons.OK);
					return true;
				}
				else
					m_changedFindDirection = false;
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check to see if there are more columns to search in the row and
		/// adjust row/column incrementers appropriately.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void Match()
		{
			if (m_doneFinding)
				return;

			// There was a match
			if (m_reverseFind)
			{
				if (m_iColumn - 1 < 0)
				{
					// Already searched all the selected columns in this row
					m_matchedRow = m_iRow - 1;
					m_matchedColumn = m_colsToSearch.Length - 1;
					m_searchedAllRowCols = true;
				}
				else
					// Still some selected columns left to search in this row
					m_matchedColumn = m_iColumn - 1;
			}
			else // FORWARD SEARCH
			{
				if (m_iColumn >= (m_colsToSearch.Length - 1))
				{
					// Already searched all the selected columns in this row
					m_matchedRow = m_iRow + 1;
					m_matchedColumn = 0;
					m_searchedAllRowCols = true;
				}
				else
					// Still some selected columns left to search in this row
					m_matchedColumn = m_iColumn + 1;
			}
		}
		#endregion

		#region Handling Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cleanup when the grid is destroyed / replaced with a new one.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleGridDestroyed(object sender, EventArgs e)
		{
			// Unsubscribe
			m_grid.CellClick -= HandleCellClick;
			m_grid.HandleDestroyed -= HandleGridDestroyed;
			ResetStartSearchCell(false);
			m_grid = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the cell click event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleCellClick(object sender, DataGridViewCellEventArgs e)
		{
			ResetStartSearchCell(false);
		}
		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaWordListGrid Grid
		{
			get { return m_grid; }
			set
			{
				if (m_grid != value)
				{
					if (m_grid != null)
					{
						m_grid.CellClick -= HandleCellClick;
						m_grid.HandleDestroyed -= HandleGridDestroyed;
					}

					m_grid = value;
					if (m_grid != null)
					{
						m_grid.HandleDestroyed += new EventHandler(HandleGridDestroyed);
						m_grid.CellClick += new DataGridViewCellEventHandler(HandleCellClick);
					}
					ResetStartSearchCell(false);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get whether the user performed a Find operation.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CanFindAgain
		{
			get { return (m_performedFind && Grid != null); }
			set { m_performedFind = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search text. This is only used in the info msg about not finding any
		/// search matches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FindText
		{
			set { m_findText = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FindPattern
		{
			set { m_findPattern = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool FindDlgIsOpen
		{
			get { return m_findDlgIsOpen; }
			set { m_findDlgIsOpen = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ShowMessages
		{
			get { return m_showMessages; }
			set { m_showMessages = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & set the columns to search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FindDlgColItem[] ColumnsToSearch
		{
			get { return m_colsToSearch; }
			set { m_colsToSearch = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the FirstMatchedRow.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int FirstMatchedRow
		{
			get { return m_firstMatchedRow; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to search in collapsed groups for
		/// a match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool SearchCollapsedGroups
		{
			get { return m_searchCollapsedGroups; }
			set { m_searchCollapsedGroups = value; }
		}

		#endregion
	}

	#region FindDlgColItem Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// FindDlgColItem class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FindDlgColItem
	{
		private int m_colIndex;
		private int m_displayIndex;
		private string m_text;
		private string m_fieldName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// FindDlgColItem constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FindDlgColItem(int index, int displayIndex, string text, string fieldName)
		{
			m_colIndex = index;
			m_displayIndex = displayIndex;
			m_text = text;
			m_fieldName = fieldName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the column index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int ColIndex
		{
			get { return m_colIndex; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the display index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DisplayIndex
		{
			get { return m_displayIndex; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FieldName
		{
			get { return m_fieldName; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The text the user sees in the UI.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_text;
		}
	}
	#endregion
}
