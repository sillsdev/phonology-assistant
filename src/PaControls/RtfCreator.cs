using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using SIL.Pa;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;
using SIL.Pa.Resources;

namespace SIL.Pa.Controls
{
	public class RtfCreator
	{
		#region Declaration

		// Enum's
		private ExportFormat m_exportFormat;
		public enum ExportFormat { Table, TabDelimited };
		private ExportTarget m_exportTarget;
		public enum ExportTarget { File, FileAndOpen, Clipboard };
		private CreateSearchItemTabs m_createSearchItemTabs;
		public enum CreateSearchItemTabs { FirstTab, SecondTab, FinishedTabs, Stop };

		// Constants
		private const string khdr = "{\\rtf1\\ansi\\deff0";
		private const string kline = "{\\line}";
		// The first 3 markups in 'kcellLine' draw a line under the column headers
		private const string kcellLine = "\\clbrdrb\\brdrs\\brdrw30\\cellx{0}";
		private const string kcell = "\\cellx{0}";
		private const string ktxcell = "\\tx{0}";
		private const string kcellHdr = "{0}\\cell ";
		private const string kcellValues = "\\f{0} \\fs{1} {2}\\cell ";
		private const string ktab = "\\tab ";
		private const string ktxHdr = "{0}\\tx ";
		private const string ktxValues = "\\f{0} \\fs{1} {2}\\tab ";
		private const string khighlight = "\\highlight{0} ";
		private const int kMaxPageWidth = 9360; // 6.5 inches in twips
		private const int kDistBetweenCols = 360; // 0.25 inch in twips between columns
		private const int kTwipsPerInch = 1440;
		private const int kSilHierGridRowKey = 9999;
		private const string kInvalidEditor = "Invalid Editor";

		// Member Variables
		private int m_numberOfColumns = 0;
		private int m_numberOfRecords = 0;
		private int m_MaxColWidth = 2160; // 1.5 inches in twips
		private int m_uiFontSize;
		private float m_pixelsPerInch;
		private Dictionary<String, String> m_columnHeaders = new Dictionary<String, String>();
		private Dictionary<string, int> m_fontSizes = new Dictionary<string, int>();
		private Dictionary<string, int> m_fontNumbers = new Dictionary<string, int>();
		// Each dict represents a row. The key is the column index & the value is the cell's value
		private List<Dictionary<int, object[]>> m_wordListRows = new List<Dictionary<int, object[]>>();
		private Dictionary<int, string> m_alignedSearchItems = new Dictionary<int, string>();
		// The key is the column index and the value is the column width
		private Dictionary<int, int> m_maxColumnWidths = new Dictionary<int, int>();
		private Dictionary<int, object[]> m_rowValues;
		private int m_cellTextWidth = 0;
		private string m_rtfEditor = string.Empty;
		private StringBuilder m_rtfBldr;
		private int m_extraSpaceToShare = 0;
		private int m_largeColCount = 0;
		private WordListCache m_cache;
		private Font m_phoneticColumnFont;
		private DataGridView m_grid;
		private TextFormatFlags m_flags;
		private float m_beforeEnvTwipWidth = 0f;
		private int m_maxBeforeEnvTextWidth = 0;
		private int m_maxSrchItemAftEnvTextWidth = 0;
		private int m_searchItemColorRefNumber = 0;
		private Graphics m_graphics;
		private StringBuilder m_tabFormatBldr = new StringBuilder();
		private StringBuilder m_cellFormatBldr = new StringBuilder();
		private StringBuilder m_cellLineFormatBldr = new StringBuilder();
		private float m_columnStartPoint = 0;
		private enum ArrayDataType { GroupingFieldName, RecordIndex, SilHierarchicalGridRow };
		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// RtfCreator constructor.
		/// </summary>
		/// <param name="target">ExportTarget</param>
		/// <param name="format">ExportFormat</param>
		/// <param name="grid">PaWordListGrid</param>
		/// <param name="rtfEditor">string</param>
		/// <returns>RtfCreator</returns>
		/// ------------------------------------------------------------------------------------
		public RtfCreator(ExportTarget target, ExportFormat format, DataGridView grid, WordListCache cache, string rtfEditor)
		{
			m_exportTarget = target;
			m_exportFormat = format;
			m_grid = grid;
			if (m_grid != null)
				m_graphics = m_grid.CreateGraphics();
			m_cache = cache;
			m_rtfEditor = rtfEditor;
			m_createSearchItemTabs = CreateSearchItemTabs.FirstTab;
			m_rtfBldr = new StringBuilder();

			// Add support for highlighting the search item
			if (m_cache.IsForSearchResults)
			{
				Dictionary<int, int> colorReferences;
				RtfHelper.ColorTable(PaApp.QuerySearchItemBackColor, out colorReferences);
				m_searchItemColorRefNumber = colorReferences[PaApp.QuerySearchItemBackColor.ToArgb()];
			}

			CalculateMaxColumnWidths();
			CreateReportHeadings();

			// This is only for running tests
			if (m_maxColumnWidths.Count == 0)
				return;

			BuildColumns();
			BuildCells();
			WriteToFileOrClipboard();
		}
		#endregion

		#region Export Rtf
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Format and save the 'align search item tabs' string.
		/// </summary>
		/// <param name="cacheEntry">WordListCacheEntry</param>
		/// ------------------------------------------------------------------------------------
		private void FormatAlignSearchIteamString(int rowIndex, WordListCacheEntry entry)
		{
			string colorRef = string.Format(khighlight, m_searchItemColorRefNumber);
			m_alignedSearchItems[rowIndex] = ktab + entry.EnvironmentBefore + ktab.Trim() + colorRef +
				entry.SearchItem + string.Format(khighlight, 0) + entry.EnvironmentAfter;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate the Phonetic column's maximum before & after environment widths.
		/// </summary>
		/// <param name="cacheEntry">WordListCacheEntry</param>
		/// ------------------------------------------------------------------------------------
		private void CalcMaxPhoneticEnvWidths(WordListCacheEntry cacheEntry)
		{
			// Calculate the text widths
			int beforeEnvTextWidth = TextRenderer.MeasureText(m_graphics, cacheEntry.EnvironmentBefore,
				m_phoneticColumnFont, Size.Empty, m_flags).Width;
			string srchItemAftEnv = " " + cacheEntry.SearchItem + cacheEntry.EnvironmentAfter;
			int srchItemAftEnvTextWidth = TextRenderer.MeasureText(m_graphics, srchItemAftEnv,
				m_phoneticColumnFont, Size.Empty, m_flags).Width;

			// Update the max values
			if (beforeEnvTextWidth > m_maxBeforeEnvTextWidth)
				m_maxBeforeEnvTextWidth = beforeEnvTextWidth;
			if (srchItemAftEnvTextWidth > m_maxSrchItemAftEnvTextWidth)
				m_maxSrchItemAftEnvTextWidth = srchItemAftEnvTextWidth;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate the maximum widths for the header columns.
		/// </summary>
		/// <param name="column">DataGridViewColumn</param>
		/// ------------------------------------------------------------------------------------
		private void CalcMaxHdrColWidths(DataGridViewColumn column)
		{
			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[column.Name];

			// Save the column header text width length
			m_columnHeaders.Add(column.Name, column.HeaderText);
			m_cellTextWidth = TextRenderer.MeasureText(m_graphics, column.HeaderText,
				column.DefaultCellStyle.Font, Size.Empty, m_flags).Width;

			if (m_cache.IsForSearchResults && fieldInfo.IsPhonetic)
			{
				m_phoneticColumnFont = column.DefaultCellStyle.Font;
				foreach (DataGridViewRow row in m_grid.Rows)
				{
					if (row is PaCacheGridRow)
					{
						int cacheIndex = ((PaCacheGridRow)row).CacheEntryIndex;
						FormatAlignSearchIteamString(row.Index, m_cache[cacheIndex]);
						CalcMaxPhoneticEnvWidths(m_cache[cacheIndex]);
					}
				}

				m_beforeEnvTwipWidth = 
					((float)m_maxBeforeEnvTextWidth / m_pixelsPerInch) * kTwipsPerInch;
				m_cellTextWidth = (m_maxBeforeEnvTextWidth + m_maxSrchItemAftEnvTextWidth);
			}

			// "Grouping By Field"
			if (m_cellTextWidth > 0)
				m_maxColumnWidths.Add(column.Index, m_cellTextWidth);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate the maximum cell column widths.
		/// </summary>
		/// <param name="cell">DataGridViewCell</param>
		/// ------------------------------------------------------------------------------------
		private void CalcMaxCellColWidths(DataGridViewCell cell)
		{
			Font columnFont = m_grid.Columns[cell.ColumnIndex].DefaultCellStyle.Font;
			string cellValue = cell.Value.ToString();

			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[m_grid.Columns[cell.ColumnIndex].Name];

			// Only display the FileName of the WaveFile when the export format is TabDelimited
			if (m_exportFormat == ExportFormat.TabDelimited && fieldInfo.IsAudioFile)
				cellValue = Path.GetFileName(cellValue);

			m_cellTextWidth = TextRenderer.MeasureText(m_graphics, cellValue, columnFont, 
				Size.Empty, m_flags).Width;

			// Update the max column length if the cell text width is greater
			if (m_cellTextWidth > m_maxColumnWidths[cell.ColumnIndex])
				m_maxColumnWidths[cell.ColumnIndex] = m_cellTextWidth;

			m_rowValues.Add(cell.ColumnIndex, new object[3] { cell.Value.ToString(), cell.RowIndex, null });
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate the maximum column widths.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CalculateMaxColumnWidths()
		{
			// For testing
			if (m_grid == null)
				return;

			// Clear the dictionary
			m_maxColumnWidths.Clear();
			// Set the flags
			m_flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.SingleLine;
			// Store the dots per inch value from the grid's graphic object
			m_pixelsPerInch = m_graphics.DpiX;

			// Sort the displayed columns by their display order
			SortedList sortedColumns = new SortedList();
			foreach (DataGridViewColumn column in m_grid.Columns)
				if (column.Visible)
					sortedColumns.Add(column.DisplayIndex, column);

			// Calculate the maximum width of the header columns
			foreach (DataGridViewColumn column in sortedColumns.Values)
			{
				// True when showing "Minimal Pairs"
				if (column.Name == string.Empty && column.Index == 0)
					continue;
				CalcMaxHdrColWidths(column);
			}

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				m_rowValues = new Dictionary<int, object[]>();

				// "Grouping By Field"
				if (row is SilHierarchicalGridRow)
				{
					if (!(row as SilHierarchicalGridRow).Expanded)
						continue;

					m_rowValues.Add(kSilHierGridRowKey, new object[3] {
						(row as SilHierarchicalGridRow).Text, row.Index, row });

					m_wordListRows.Add(m_rowValues);
					continue;
				}

				if (!row.Visible)
					continue;

				m_numberOfRecords++;

				// Sort the cells by their display order
				SortedList sortedCellColumns = new SortedList();
				foreach (DataGridViewCell cell in row.Cells)
					if (cell.Visible)
						sortedCellColumns.Add(m_grid.Columns[cell.ColumnIndex].DisplayIndex, cell);

				// Calculate the maximum width of the cell columns
				foreach (DataGridViewCell cell in sortedCellColumns.Values)
				{
					if (cell.Value == null)
						m_rowValues.Add(cell.ColumnIndex, new object[3] { string.Empty, cell.RowIndex, null });
					else
						CalcMaxCellColWidths(cell);
				}
				m_wordListRows.Add(m_rowValues);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create Report Headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CreateReportHeadings()
		{
			// For testing
			if (m_cache == null)
				return;

			m_uiFontSize = (int)FontHelper.UIFont.SizeInPoints * 2;
			m_rtfBldr.AppendLine(khdr);
			m_rtfBldr.AppendLine(RtfHelper.FontTable(m_fontNumbers, ref m_uiFontSize));
			
			// Add color support
			if (m_cache.IsForSearchResults)
			{
				Dictionary<int, int> colorReferences;
				m_rtfBldr.AppendLine(RtfHelper.ColorTable(PaApp.QuerySearchItemBackColor, out colorReferences));
			}

			m_rtfBldr.AppendLine("\\pard\\plain ");
			m_rtfBldr.AppendLine(string.Format(ktxcell, 2160));
			m_rtfBldr.AppendLine("\\f0 \\fs18 {\\b");
			// SearchQuery is null when showing "Minimal Pairs"
			if (m_cache.IsForSearchResults && m_cache.SearchQuery != null)
				m_rtfBldr.AppendFormat(Properties.Resources.kstidRtfGridHdrSearchPattern,
					ktab, m_cache.SearchQuery.Pattern);
			else
				m_rtfBldr.AppendFormat(Properties.Resources.kstidRtfGridHdrWordListName,
					ktab, Properties.Resources.kstidRtfGridAllWordsLabel);
			m_rtfBldr.AppendLine(kline);
			m_rtfBldr.AppendFormat(Properties.Resources.kstidRtfGridHdrNbrOfRecords,
				ktab, m_numberOfRecords);
			m_rtfBldr.AppendLine(kline);
			m_rtfBldr.AppendFormat(Properties.Resources.kstidRtfGridHdrProjectName,
				ktab, PaApp.Project.ProjectName);
			m_rtfBldr.AppendLine(kline);
			m_rtfBldr.AppendFormat(Properties.Resources.kstidRtfGridHdrLanguageName,
				ktab, PaApp.Project.Language);
			m_rtfBldr.AppendLine(kline);
			m_rtfBldr.AppendFormat(Properties.Resources.kstidRtfGridHdrDateTimeName,
				ktab, DateTime.Now.ToString());
			
			m_rtfBldr.Append("}");
			m_rtfBldr.AppendLine("\\par\\par\\pard");
			if (m_exportFormat == ExportFormat.Table)
				m_rtfBldr.Append("\\trowd");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate the extra column space that can be shared / redistributed among
		/// the larger columns.
		/// </summary>
		/// <returns>The calculated extraShareSpace</returns>
		/// ------------------------------------------------------------------------------------
		private int CalcExtraColumnSpace(float totalColWidth)
		{
			// Does this column have some extra space to share?
			if ((int)totalColWidth < m_MaxColWidth)
				return (m_MaxColWidth - (int)totalColWidth);

			m_largeColCount++;
			return 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the extra column space.
		/// </summary>
		/// <returns>The calculated extraShareSpace</returns>
		/// ------------------------------------------------------------------------------------
		private int ExtraSpaceToShare()
		{
			int extraShareSpace = 0;
			float totalColWidth = 0;

			// Calculate the new maximum column width
			m_MaxColWidth = kMaxPageWidth / m_maxColumnWidths.Count;

			foreach (int colWidth in m_maxColumnWidths.Values)
			{
				totalColWidth = ((float)colWidth / m_pixelsPerInch) * kTwipsPerInch +
					(m_exportFormat == ExportFormat.Table ? kDistBetweenCols : 0);
				extraShareSpace += CalcExtraColumnSpace(totalColWidth);
			}

			// Subtract the "kDistBetweenCols" padding from the last column
			if ((int)totalColWidth < m_MaxColWidth)
				extraShareSpace -= kDistBetweenCols;

			return extraShareSpace;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Size the Phonetic Column so that it is just wide enough for the longest word.
		/// </summary>
		/// <param name="columnTwipWidth">float</param>
		/// <param name="columnStartPoint">float</param>
		/// <returns>the columnStartPoint</returns>
		/// ------------------------------------------------------------------------------------
		private float SizePhoneticColumn(float columnTwipWidth, float columnStartPoint)
		{
			m_numberOfColumns++;
			if (m_cache != null) // for testing
				if (!m_cache.IsForSearchResults)
					columnTwipWidth += 30; // Fudge Factor

			// Make sure the Phonetic column width is always just long enough so there is NO wrapping
			return columnStartPoint += columnTwipWidth;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shorten the column widths.
		/// </summary>
		/// <param name="columnTwipWidth">float</param>
		/// <param name="columnStartPoint">float</param>
		/// <param name="fieldInfo">string</param>
		/// <returns>the columnStartPoint</returns>
		/// ------------------------------------------------------------------------------------
		private float ShortenColumns(float columnTwipWidth, float columnStartPoint,
			PaFieldInfo fieldInfo)
		{
			if (fieldInfo.IsPhonetic)
				return SizePhoneticColumn(columnTwipWidth, columnStartPoint);

			// There is no extra space to share
			if (m_extraSpaceToShare == 0)
				return columnStartPoint += m_MaxColWidth;

			if (m_numberOfColumns < m_maxColumnWidths.Count)
			{
				m_numberOfColumns++;
				// Calculate the extra space per large column
				int extraSpacePerLargeCol = m_extraSpaceToShare / m_largeColCount;
				
				int neededWidth = (int)columnTwipWidth +
					(m_exportFormat == ExportFormat.Table ? kDistBetweenCols : 0);

				if (neededWidth > m_MaxColWidth)
				{
					// The large column needs some extra space
					if (neededWidth - m_MaxColWidth > 0)
						columnStartPoint += m_MaxColWidth + extraSpacePerLargeCol;
				}
				else
					// The column is smaller than the maximum column width
					columnStartPoint += neededWidth;

				// Last column processing
				if (m_numberOfColumns == m_maxColumnWidths.Count)
				{
					// Remove the "kDistBetweenCols" padding if the last column has a shorter width
					if (columnTwipWidth < m_MaxColWidth)
						neededWidth -= kDistBetweenCols;
					
					columnStartPoint += neededWidth;
					
					// Don't ever go past the kMaxPageWidth
					if (columnStartPoint > kMaxPageWidth)
						columnStartPoint = kMaxPageWidth;
				}
			}

			return columnStartPoint;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create the column headers
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CreateColumnHeaders()
		{
			m_rtfBldr.Append(m_exportFormat == ExportFormat.Table ? "\n\\intbl" : "\n");
			m_rtfBldr.Append("\\f0 \\fs20 {\\b ");
			//float columnHdrsTwipWidth = 0f;
			//float maxColHdrTwipWidth = 0;

			//foreach (string columnHeader in m_columnHeaders.Values)

			foreach (KeyValuePair<string, string> colHdr in m_columnHeaders)
			{
				// "Grouping By Field"
				if (colHdr.Key == string.Empty && colHdr.Value == string.Empty)
					continue;

				if (m_exportFormat == ExportFormat.Table)
					m_rtfBldr.Append(string.Format(kcellHdr, colHdr.Value));
				else
				{
					//maxColHdrTwipWidth =
					//    ((float)m_maxColumnWidths[colHdr.Key] / m_pixelsPerInch) * kTwipsPerInch;
					////columnHdrsTwipWidth += (maxColHdrTwipWidth + kDistBetweenCols);
					//columnHdrsTwipWidth += maxColHdrTwipWidth;
					//// Don't print out headers past the max page width
					//if (columnHdrsTwipWidth <= kMaxPageWidth)
					//{
						m_rtfBldr.Append(string.Format(ktxHdr, colHdr.Value));
						m_rtfBldr.Append(ktab);

						PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[colHdr.Key];
					
						// This fixes the placement of the columnHeader following 'Phonetic', because
						// it takes into consideration the extra 2 tab stops for Search Item alignment.
						if (fieldInfo.IsPhonetic && m_cache.IsForSearchResults)
						{
							m_rtfBldr.Append(ktab);
							m_rtfBldr.Append(ktab);
						}
					//}
					//columnHdrsTwipWidth += kDistBetweenCols;
				}
			}

			if (m_exportFormat == ExportFormat.Table)
			{
				m_rtfBldr.AppendLine("} \\row");
				m_rtfBldr.Append("\\trowd");
			}
			else
				m_rtfBldr.AppendLine("} " + kline);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Recalculate m_MaxColWidth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RecalcMaxColWidth()
		{
			// Get the columnTwipWidth of the Phonetic column
			float phoneticTwipWidth = ((float)m_maxColumnWidths[0] / m_pixelsPerInch) * kTwipsPerInch;
			if (!m_cache.IsForSearchResults)
				phoneticTwipWidth += 30; // Fudge Factor
			m_MaxColWidth = (kMaxPageWidth - (int)phoneticTwipWidth) / (m_maxColumnWidths.Count - 1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private StringBuilder FormatSearchItemTabString(StringBuilder tabFormatBldr, float columnStartPoint)
		{
			tabFormatBldr.Append("\\tqr");
			while (m_createSearchItemTabs != CreateSearchItemTabs.Stop)
			{
				if (m_createSearchItemTabs == CreateSearchItemTabs.FirstTab)
				{
					if (m_exportFormat == ExportFormat.TabDelimited)
						m_beforeEnvTwipWidth += columnStartPoint;
					m_createSearchItemTabs = CreateSearchItemTabs.SecondTab;
				}
				else
					if (m_createSearchItemTabs == CreateSearchItemTabs.SecondTab)
					{
						m_createSearchItemTabs = CreateSearchItemTabs.FinishedTabs;
						m_beforeEnvTwipWidth += 28;
					}

				tabFormatBldr.Append(string.Format(ktxcell, (int)m_beforeEnvTwipWidth));
				if (m_createSearchItemTabs == CreateSearchItemTabs.FinishedTabs)
					m_createSearchItemTabs = CreateSearchItemTabs.Stop;
			}
			return tabFormatBldr;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculate and set column widths.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetColumnWidths(KeyValuePair<int, int> maxColumnWidth, float totalColumnsWidth)
		{
			PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[m_grid.Columns[maxColumnWidth.Key].Name];

			// Create the 2 tabs for the aligning the Phonetic column's search item
			if (fieldInfo.IsPhonetic && m_cache.IsForSearchResults)
				m_tabFormatBldr = FormatSearchItemTabString(m_tabFormatBldr, m_columnStartPoint);

			// Only shorten columns width if the total columns width is greater than kMaxPageWidth
			float columnTwipWidth = ((float)maxColumnWidth.Value / m_pixelsPerInch) * kTwipsPerInch;
			
			if (totalColumnsWidth > kMaxPageWidth && m_exportFormat == ExportFormat.Table)
				m_columnStartPoint = ShortenColumns(columnTwipWidth, m_columnStartPoint, fieldInfo);
			else
				m_columnStartPoint += columnTwipWidth + kDistBetweenCols;

			if (m_exportFormat == ExportFormat.TabDelimited)
			{
				m_cellFormatBldr.Append(string.Format(ktxcell, (int)m_columnStartPoint)); // set tabs
			}
			else
			{
				// cellLineFormatBldr has the bold underlined header cells
				m_cellLineFormatBldr.Append(string.Format(kcellLine, (int)m_columnStartPoint));
			
				// cellFormatBldr holds the 'normal' formatted cells
				m_cellFormatBldr.Append(string.Format(kcell, (int)m_columnStartPoint));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildColumns()
		{
			// Reset the maximum column length
			m_MaxColWidth = 2160;
			m_numberOfColumns = 0;
			// Compute the extra space that can be redistributed among the report columns
			m_extraSpaceToShare = ExtraSpaceToShare();

			// If there is no extra space, recalculate the maxColWidth with the Phonetic column's twip width
			if (m_extraSpaceToShare == 0)
				RecalcMaxColWidth();

			// Calculate the total column width to see if it is greater than 6.5 inches
			float totalColumnsWidth = 0;
			foreach (int colWidth in m_maxColumnWidths.Values)
				totalColumnsWidth += ((float)colWidth / m_pixelsPerInch) * kTwipsPerInch +
				(m_exportFormat == ExportFormat.Table ? kDistBetweenCols : 0);

			//m_maxColumnWidths.Remove(m_maxColumnWidths.Count);
			foreach (KeyValuePair<int, int> maxColumnWidth in m_maxColumnWidths)
				SetColumnWidths(maxColumnWidth, totalColumnsWidth);

			if (m_exportFormat == ExportFormat.Table)
				m_rtfBldr.Append(m_cellLineFormatBldr.ToString());

			m_rtfBldr.AppendLine(string.Empty);

			if (m_exportFormat == ExportFormat.TabDelimited)
				m_rtfBldr.AppendLine(m_cellFormatBldr.ToString());

			CreateColumnHeaders();

			if (m_cache.IsForSearchResults)
				m_rtfBldr.AppendLine(m_tabFormatBldr.ToString());

			m_rtfBldr.AppendLine(m_cellFormatBldr.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Format the cell values.
		/// </summary>
		/// <param name="row">Dictionary</param>
		/// <param name="rowIndex">int</param>
		/// ------------------------------------------------------------------------------------
		private void FormatCellValues(Dictionary<int, object[]> row, int rowIndex)
		{
			if (m_exportFormat == ExportFormat.Table)
				m_rtfBldr.Append("\\intbl");

			foreach (KeyValuePair<int, object[]> col in row)
			{
				// "Grouping By Field"
				if (col.Key == kSilHierGridRowKey)	
				{
					if (m_exportFormat == ExportFormat.Table)
						m_rtfBldr.Remove((m_rtfBldr.Length - "\\intbl".Length), "\\intbl".Length);

					m_rtfBldr.AppendLine("\\trowd" + string.Format(kcell, (int)m_columnStartPoint));
					m_rtfBldr.Append("\\intbl\\f0 \\fs20 {\\b ");

					// Print the Group Header with the child row counts
					SilHierarchicalGridRow silHierGridRow = 
						col.Value[(int)ArrayDataType.SilHierarchicalGridRow] as SilHierarchicalGridRow;

					m_rtfBldr.AppendLine(col.Value[(int)ArrayDataType.GroupingFieldName] + "  " + 
						string.Format(silHierGridRow.CountFormatStrings[0], silHierGridRow.ChildCount) + "\\cell } \\row");

					m_rtfBldr.Append("\\trowd");
					if (m_cache.IsForSearchResults)
						m_rtfBldr.AppendLine(m_tabFormatBldr.ToString());

					m_rtfBldr.AppendLine(m_cellFormatBldr.ToString());
					continue;
				}
				string colName = m_grid.Columns[col.Key].Name;

				// "Grouping By Field"
				if (colName == string.Empty)
					continue;

				PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[colName];
				int fontNumber = m_fontNumbers[colName];
				int fontSize = m_fontSizes[colName];
				string colValue = col.Value[(int)ArrayDataType.GroupingFieldName].ToString().Replace("\\", "\\\\");

				if (m_cache.IsForSearchResults && fieldInfo.IsPhonetic)
				{
					m_rtfBldr.Append(string.Format(
						(m_exportFormat == ExportFormat.Table ?	kcellValues : ktxValues),
						fontNumber, fontSize, m_alignedSearchItems[(int)col.Value[(int)ArrayDataType.RecordIndex]]));
				}
				else
				{
					if (m_exportFormat == ExportFormat.Table)
						m_rtfBldr.Append(string.Format(kcellValues, fontNumber, fontSize, colValue));
					else
					{
						// Only display the FileName of the WaveFile when TabDelimited export fomat
						if (fieldInfo.IsAudioFile)
							colValue = Path.GetFileName(colValue);

						m_rtfBldr.Append(string.Format(ktxValues, fontNumber, fontSize, colValue));
					}
				}
			}

			// Removes the last "\\tab "
			if (m_exportFormat == ExportFormat.TabDelimited)
				m_rtfBldr.Remove((m_rtfBldr.Length - 5), 4);

			// "Grouping By Field"
			if (!row.ContainsKey(kSilHierGridRowKey))
				m_rtfBldr.AppendLine(m_exportFormat == ExportFormat.Table ? "\\row" : kline);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Build the cells.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildCells()
		{
			m_fontSizes = new Dictionary<string, int>();

			foreach (PaFieldInfo fieldInfo in PaApp.Project.FieldInfo)
			{
				if (fieldInfo.Font != null)
					m_fontSizes[fieldInfo.FieldName] = (int)(fieldInfo.Font.SizeInPoints * 2);
			}

			int rowIndex = 0;
			foreach (Dictionary<int, object[]> row in m_wordListRows)
			{
				FormatCellValues(row, rowIndex);
				rowIndex++;
			}

			m_rtfBldr.AppendLine("}");
		}		

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Write the Rtf string to an output file or the clipboard.
		/// </summary>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		private bool WriteToFileOrClipboard()
		{
			string rtf = RtfHelper.TranslateUnicodeChars(m_rtfBldr.ToString());

			if (m_exportTarget == ExportTarget.Clipboard)
			{
				Clipboard.SetText(rtf, TextDataFormat.Rtf);
				return true;
			}

			string filter = ResourceHelper.GetString("kstidFiletypeRTF") + "|" +
				ResourceHelper.GetString("kstidFileTypeAllFiles");

			int filterIndex = 0;
			string filename = PaApp.SaveFileDialog("rtf", filter, ref filterIndex,
				Properties.Resources.kstidRTFExportCaptionSFD, string.Empty);

			if (filename != string.Empty)
			{
				try
				{
					using (StreamWriter sw = new StreamWriter(filename))
						sw.Write(rtf);
				}
				catch (Exception ex)
				{
					// Display the error message
					STUtils.STMsgBox(ex.Message, MessageBoxButtons.OK);
					return false;
				}
			}

			// Open the file with the specified RTF editor
			if (File.Exists(m_rtfEditor))
			{
				if (m_exportTarget == ExportTarget.FileAndOpen && !string.IsNullOrEmpty(m_rtfEditor))
					if (filename.ToString().Trim().Length != 0) // Make sure a filename was selected
						Process.Start("\"" + m_rtfEditor + "\"", "\"" + filename + "\"");
			}
			else
			{
				string errorMsg =
					string.Format(Properties.Resources.kstidRtfInvalidEditor, m_rtfEditor);
				MessageBox.Show(errorMsg, kInvalidEditor, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			return true;
		}
		#endregion
	}
}
