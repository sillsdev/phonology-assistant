using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public class FindInfo
	{
		#region Member Variables
		// Declare member variables
		private static PaWordListGrid s_grid;
		private static bool s_firstMatch;
		private static int s_firstMatchedRow;
		private static int s_firstMatchedCol;
		private static int s_matchedRow;
		private static int s_matchedColumn;
		private static int s_startRow;
		private static int s_iPreviousRow;
		private static int s_iRow;
		private static int s_iColumn;
		private static bool s_reverseFind;
		private static bool s_doneFinding;
		private static bool s_performedFind;
		private static bool s_firstLoop = true;
		private static bool s_searchedAllRowCols;
		private static int s_numberOfFinds;
		private static bool s_changedFindDirection;
		private static bool s_searchCollapsedGroups;
		private static bool s_findBackwards;
		private static SilHierarchicalGridRow s_silHierarchicalGridRow;

		static FindInfo()
		{
			FindText = string.Empty;
			FindPattern = string.Empty;
			ShowMessages = true;
			ColumnsToSearch = new FindDlgColItem[] { };
		}

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
					s_grid.Columns[dlgColItem.FieldName].Index,
					dlgColItem.DisplayIndex,
					dlgColItem.ToString(), dlgColItem.FieldName);

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
			if (s_grid == null)
				return;

			if (updateColIndexes)
				UpdateColumnIndexes();

			s_firstMatch = false;
			s_firstLoop = true;
			s_doneFinding = false;
			// Start searching from the current row & column
			if (s_grid != null && s_grid.CurrentCell != null)
			{
				SetCurrentRow(s_grid.CurrentCell.RowIndex);
				SetCurrentColumn(s_grid.CurrentCell.ColumnIndex);
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
			for (int i = 0; i < ColumnsToSearch.Length; i++)
			{
				if (ColumnsToSearch[i].DisplayIndex == currCellDisplayIndex)
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
		private static void CalcStartSearchCol()
		{
			int currCellDisplayIndex = s_grid.Columns[s_grid.CurrentCell.ColumnIndex].DisplayIndex;

			if (s_grid.Cache.IsCIEList || s_grid.IsGroupedByField) // IsCIEList -> Minimal Pairs
			{
				// Covert currCellDisplayIndex to 0 based array for comparison
				currCellDisplayIndex--;
				if (currCellDisplayIndex < 0)
					currCellDisplayIndex = 0;
			}
			int currColumnSortPosition = GetColumnSortPosition(currCellDisplayIndex);

			if (s_reverseFind)
			{
				if (currCellDisplayIndex != 0)
				{
					for (int i = ColumnsToSearch.Length - 1; i >= 0; i--)
					{
						if (s_doneFinding && s_grid.CurrentCell.RowIndex == s_iPreviousRow)
						{
							if (ColumnsToSearch[i].DisplayIndex < currCellDisplayIndex)
							{
								SetCurrentColumn(i);
								return;
							}
						}
						else if (s_doneFinding && s_grid.CurrentCell.RowIndex != s_iPreviousRow)
						{
							SetCurrentColumn(ColumnsToSearch.Length - 1);
							return;
						}
						else if (s_doneFinding && (currColumnSortPosition < ColumnsToSearch.Length - 1))
						{
							if (ColumnsToSearch[i].DisplayIndex <= currCellDisplayIndex)
							{
								SetCurrentColumn(i);
								return;
							}
						}
						else
						{
							if (ColumnsToSearch[i].DisplayIndex < currCellDisplayIndex)
							{
								SetCurrentColumn(i);
								return;
							}
						}
					}
				}
				if (s_grid.CurrentCell.RowIndex > 0)
					SetCurrentRow(s_grid.CurrentCell.RowIndex - 1);
				else
					SetCurrentRow(s_grid.Rows.Count - 1); // last row becomes the current row
				SetCurrentColumn(ColumnsToSearch.Length - 1);
			}
			else // FORWARD FIND
			{
				if (currColumnSortPosition != ColumnsToSearch.Length - 1)
				{
					for (int i = 0; i < ColumnsToSearch.Length; i++)
					{
						if (s_doneFinding && s_grid.CurrentCell.RowIndex == s_iPreviousRow)
						{
							if (ColumnsToSearch[i].DisplayIndex > currCellDisplayIndex)
							{
								SetCurrentColumn(i);
								return;
							}
						}
						else if (s_doneFinding && s_grid.CurrentCell.RowIndex != s_iPreviousRow)
						{
							SetCurrentColumn(0);
							return;
						}
						else if (s_doneFinding && currColumnSortPosition > 0)
						{
							if (ColumnsToSearch[i].DisplayIndex >= currCellDisplayIndex)
							{
								SetCurrentColumn(i);
								return;
							}
						}
						else
						{
							if (ColumnsToSearch[i].DisplayIndex > currCellDisplayIndex)
							{
								SetCurrentColumn(i);
								return;
							}
						}
					}
				}
				if (s_grid.CurrentCell.RowIndex + 1 < s_grid.Rows.Count)
					SetCurrentRow(s_grid.CurrentCell.RowIndex + 1);
				else
					SetCurrentRow(0); // first row becomes the current row
				SetCurrentColumn(0);
			}
			return;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the current row value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetCurrentRow(int currentRow)
		{
			s_startRow = currentRow;
			s_iRow = currentRow;
			s_matchedRow = currentRow;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the current column value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetCurrentColumn(int currentColumn)
		{
			s_iColumn = currentColumn;
			s_matchedColumn = currentColumn;
			return;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches for first findPattern match with a cell beginning with the last
		/// matched row and column
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool FindFirst(bool findPrevious)
		{
			// 'Find Next' & 'Find Previous' enable when a Find has been executed
			s_performedFind = true;

			s_numberOfFinds = 0;
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
			s_changedFindDirection = (findPrevious != s_reverseFind);

			// Current cell will be null if its column is hidden thru the OptionsDlg>>WordList tab
			if (s_grid.CurrentCell == null)
				s_grid.CurrentCell = s_grid[0, 0];

			if (findPrevious)
			{
				s_reverseFind = true;
				// This occurs when the user finds a match on the last Find column in the row
				// with a 'Find Next' and then switches direction with a 'Find Prev'.
				if (s_changedFindDirection && s_searchedAllRowCols)
					s_matchedRow--; // back up the row by 1
				
				s_searchedAllRowCols = false;
				
				// This occurs when the user hits the end of the Find loop with a 'Find Next',
				// gets the info popup, and then switches direction with a 'Find Prev'.
				if (s_changedFindDirection && s_doneFinding)
				{
					// back up to row with last find match
					s_matchedRow = s_grid.CurrentCell.RowIndex;
					s_doneFinding = false; // so will set column to prev sort column
				}
				
				CalcStartSearchCol();
				return FindBackward();
			}

			// FORWARD SEARCH
			s_reverseFind = false;
			
			// This occurs when the user finds a match on the first Find column in the row
			// with a 'Find Prev' and then switches direction with a 'Find Next'.
			if (s_changedFindDirection && s_searchedAllRowCols)
				s_matchedRow++; // advance the row by 1
		
			s_searchedAllRowCols = false;
			
			// This occurs when the user hits the end of the Find loop with a 'Find Prev',
			// gets the info popup, and then switches direction with a 'Find Next'.
			if (s_changedFindDirection && s_doneFinding)
			{
				// advance to row with last find match
				s_matchedRow = s_grid.CurrentCell.RowIndex;
				s_doneFinding = false; // so will set column to next sort column
			}
			
			CalcStartSearchCol();
			return FindForward();
		}
			
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches for first findPattern match with a cell beginning with the last
		/// matched row and column
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool FindForward()
		{
			s_findBackwards = false;

			// Start the Find over with the first record when reach the bottom.
			if (s_iRow == s_grid.Rows.Count)
				s_matchedRow = 0;

			for (s_iRow = s_matchedRow; s_iRow < s_grid.Rows.Count; s_iRow++)
			{
				if (DataNotFound())
				{
					s_firstLoop = true;
					return false;
				}

				// Save the hierarchical row for later expansion if needed (when searching collapsed recs)
				if (s_grid.Rows[s_iRow] is SilHierarchicalGridRow)
					s_silHierarchicalGridRow = s_grid.Rows[s_iRow] as SilHierarchicalGridRow;

				s_iPreviousRow = s_iRow;
				for (s_iColumn = s_matchedColumn; s_iColumn < ColumnsToSearch.Length; s_iColumn++)
				{
					if (ProcessColumns())
						return true;
				}

				// Didn't find a match, so start searching again in the 1st column on the next row
				s_matchedRow = s_iRow + 1;
				SetCurrentColumn(0);

				// Restart the search from the top if there has been at least 1 match already
				if (s_matchedRow >= s_grid.Rows.Count)
				{
					s_iRow = -1;
					s_matchedRow = 0;
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
			s_findBackwards = true;

			// Start the Find over with the last record when reach the top.
			if (s_iRow < 0)
				s_matchedRow = s_grid.Rows.Count - 1;

			for (s_iRow = s_matchedRow; s_iRow >= 0; s_iRow--)
			{
				if (DataNotFound())
				{
					s_firstLoop = true;
					return false;
				}

				//// Don't search Hierarchical Row's
				//if (s_grid.Rows[s_iRow] is SilHierarchicalGridRow)
				//        continue;

				s_iPreviousRow = s_iRow;
				for ( s_iColumn = s_matchedColumn; s_iColumn >= 0; s_iColumn--)
				{
					if (ProcessColumns())
						return true;
				}

				// Didn't find a match, so start searching again in the last column on the previous row
				s_matchedRow = s_iRow - 1;
				SetCurrentColumn(ColumnsToSearch.Length - 1);

				// Restart the search from the bottom if there has been at least 1 match already
				if (s_matchedRow < 0)
				{
					s_iRow = s_grid.Rows.Count;
					s_matchedRow = s_grid.Rows.Count - 1;
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
			if (!s_firstLoop)
			{
				if (s_iRow == s_startRow && !s_firstMatch)
				{
					if (ShowMessages)
						Utils.MsgBox(Properties.Resources.kstidFindDataNotFound + FindText);
					
					return true;
				}
			}
			s_firstLoop = false;
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
			// Make sure the column to search is still in the grid and visible.
			if (s_grid.Columns[ColumnsToSearch[s_iColumn].FieldName] == null ||
				!s_grid.Columns[ColumnsToSearch[s_iColumn].FieldName].Visible)
			{
				var tmpLst = new List<FindDlgColItem>(ColumnsToSearch);
				tmpLst.RemoveAt(s_iColumn);
				ColumnsToSearch = tmpLst.ToArray();
				return false;
			}

			if (SearchCellDataForMatch(
				s_grid[ColumnsToSearch[s_iColumn].ColIndex, s_grid.Rows[s_iRow].Index], FindPattern))
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
		private static void ExpandPreviousHierarchicalGridRow()
		{
			for (int rowIndex = s_iRow; rowIndex >= 0; rowIndex--)
			{
				SilHierarchicalGridRow row = s_grid.Rows[rowIndex] as SilHierarchicalGridRow;
				if (row != null)
				{
					row.Expanded = true;
					return;
				}
			}
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
			if (!cell.Visible && !s_searchCollapsedGroups &&
				(s_grid.IsGroupedByField || s_grid.Cache.IsCIEList))
			{
				return false;
			}

			string cellValue = cell.Value as string;
			if (cellValue == null)
				return false;

			PaFieldInfo fieldInfo =
				App.Project.FieldInfo[s_grid.Columns[cell.ColumnIndex].Name];

			if (!fieldInfo.IsPhonetic)
				cellValue = FFNormalizer.Normalize(cellValue);

			try
			{
				if (!Regex.IsMatch(cellValue, findPattern))
					return false;

				if (DoneSearching(cell))
					return true;

				if (!cell.Visible && s_searchCollapsedGroups &&
					(s_grid.IsGroupedByField || s_grid.Cache.IsCIEList))
				{
					if (s_findBackwards)
						ExpandPreviousHierarchicalGridRow();
					else
					{
						// s_silHierarchicalGridRow will only be null when the user
						// Groups by Sorted Field in the middle of a Find and then
						// does a Find Next
						if (s_silHierarchicalGridRow == null)
							ExpandPreviousHierarchicalGridRow();
						else if (!s_silHierarchicalGridRow.Expanded)
							s_silHierarchicalGridRow.Expanded = true;
					}
				}

				s_numberOfFinds++;

				// If the cell is off the screen, move the cell's row to the screen's
				// middle. s_showMessages will only be false if running tests.
				if (!cell.Displayed && ShowMessages)
					s_grid.ScrollRowToMiddleOfGrid(cell.RowIndex);

				if (cell.Visible)
					s_grid.CurrentCell = cell;
			}
			catch (Exception ex)
			{
				if (ShowMessages)
					Utils.MsgBox(ex.Message);
			}

			// Done searching
			return true;
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
			if (!s_firstMatch)
			{
				s_firstMatch = true;
				s_firstMatchedRow = cell.RowIndex;
				s_firstMatchedCol = cell.ColumnIndex;
				return false;
			}

			if (s_doneFinding)
			{
				s_doneFinding = false;
				if (s_numberOfFinds < 2)
				{
					// Display message that all records have been searched
					if (ShowMessages)
						Utils.MsgBox(Properties.Resources.kstidFindDoneSearching);
					s_firstMatchedRow = cell.RowIndex;
					s_firstMatchedCol = cell.ColumnIndex;
				}
				s_numberOfFinds = 0;
				return false;
			}

			if (s_firstMatch && 
				cell.RowIndex == s_firstMatchedRow && 
				cell.ColumnIndex == s_firstMatchedCol)
			{
				if (!s_changedFindDirection)
				{
					s_doneFinding = true;
					// Display message that all records have been searched
					if (ShowMessages)
						Utils.MsgBox(Properties.Resources.kstidFindDoneSearching);
					
					return true;
				}
					
				s_changedFindDirection = false;
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
			if (s_doneFinding)
				return;

			// There was a match
			if (s_reverseFind)
			{
				if (s_iColumn - 1 < 0)
				{
					// Already searched all the selected columns in this row
					s_matchedRow = s_iRow - 1;
					s_matchedColumn = ColumnsToSearch.Length - 1;
					s_searchedAllRowCols = true;
				}
				else
					// Still some selected columns left to search in this row
					s_matchedColumn = s_iColumn - 1;
			}
			else // FORWARD SEARCH
			{
				if (s_iColumn >= (ColumnsToSearch.Length - 1))
				{
					// Already searched all the selected columns in this row
					s_matchedRow = s_iRow + 1;
					s_matchedColumn = 0;
					s_searchedAllRowCols = true;
				}
				else
					// Still some selected columns left to search in this row
					s_matchedColumn = s_iColumn + 1;
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
			s_grid.CellClick -= HandleCellClick;
			s_grid.HandleDestroyed -= HandleGridDestroyed;
			ResetStartSearchCell(false);
			s_grid = null;
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
			get { return s_grid; }
			set
			{
				if (s_grid != value)
				{
					if (s_grid != null)
					{
						s_grid.CellClick -= HandleCellClick;
						s_grid.HandleDestroyed -= HandleGridDestroyed;
					}

					s_grid = value;
					
					if (s_grid != null)
					{
						s_grid.HandleDestroyed += HandleGridDestroyed;
						s_grid.CellClick += HandleCellClick;
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
			get { return (s_performedFind && Grid != null); }
			set { s_performedFind = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search text. This is only used in the info msg about not finding any
		/// search matches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FindText { private get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FindPattern { private get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool FindDlgIsOpen { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ShowMessages { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & set the columns to search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FindDlgColItem[] ColumnsToSearch { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the FirstMatchedRow.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int FirstMatchedRow
		{
			get { return s_firstMatchedRow; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to search in collapsed groups for
		/// a match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool SearchCollapsedGroups
		{
			get { return s_searchCollapsedGroups; }
			set { s_searchCollapsedGroups = value; }
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
		private readonly int s_colIndex;
		private readonly int s_displayIndex;
		private readonly string s_text;
		private readonly string s_fieldName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// FindDlgColItem constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FindDlgColItem(int index, int displayIndex, string text, string fieldName)
		{
			s_colIndex = index;
			s_displayIndex = displayIndex;
			s_text = text;
			s_fieldName = fieldName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the column index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int ColIndex
		{
			get { return s_colIndex; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the display index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DisplayIndex
		{
			get { return s_displayIndex; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FieldName
		{
			get { return s_fieldName; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The text the user sees in the UI.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return s_text;
		}
	}
	#endregion
}
