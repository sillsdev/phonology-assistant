using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public interface IRecordView
	{
		void Clear();
		void UpdateFonts();
		void UpdateRecord(RecordCacheEntry entry);
		void UpdateRecord(RecordCacheEntry entry, bool forceUpdate);
	}
	
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// An RTF control that displays the contents of a PA record.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class RtfRecordView : RichTextBox, IRecordView
	{
		internal class RTFFieldInfo
		{
			internal string field;
			internal string fieldValue;
			internal string label;
			internal int labelWidth;
			internal int valueWidth;
			internal int displayIndex;

			// The following fields are specifically for interlinear fields.
			internal bool isInterlinearField;
			internal bool isFirstLine;
			internal string[] columnValues;
			internal Dictionary<int, string[]> parsedColValues;
		}

		private const char kZeroWidthSpace = '\u200B';
		private const int kInterlinearColPadding = 130;
		private const int kGapBetweenLabelAndValue = 200;
		private const int kGapBetweenColumns = 500;
		private const string khdr = "{\\rtf1\\ansi\\deff0";
		private const string ktab = "\\tx{0}";
		private const string kline = "\\f{0}\\fs{1} {2}";
		private const string kbold = "\\b ";
		private const string kitalic = "\\i ";
		private const string kFmtOneLineOneCol = @"\cf{0}\b\fs{1}\f{2} {3}:\cf0\b0\tab";

		private Dictionary<string, int> m_fontSizes = new Dictionary<string, int>();
		private readonly Dictionary<string, int> m_fontNumbers = new Dictionary<string, int>();
		private readonly Dictionary<string, Font> m_fonts = new Dictionary<string, Font>();
		
		private readonly List<RTFFieldInfo> m_rtfFields = new List<RTFFieldInfo>();
		private List<int> m_firstILLineTabs;
		private List<int> m_subordinateILLineTabs;
		private int m_rowsInCol1;
		private int m_numInterlinearFields;
		private string m_rtf;
		private int m_uiFontSize;
		private int m_uiFontNumber;
		private int m_maxFontSize;
		private int m_maxFontNumber;
		private int m_lineSpacing;
		private bool m_useExactLineSpacing;
		private int m_fieldLabelColorRefNumber;
		private float m_pixelsPerInch;
		private RecordCacheEntry m_recEntry;
		private const TextFormatFlags kTxtFmtFlags = TextFormatFlags.NoPadding |
			TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

		/// ------------------------------------------------------------------------------------
		public RtfRecordView()
		{
			ReadOnly = true;
			TabStop = false;
			WordWrap = false;

			if (!App.DesignMode)
				UpdateFonts();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Empties the contents of the record view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Clear()
		{
			base.Clear();
			Rtf = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the Rtf contents of the text box. When setting the value to null or
		/// an empty string, a "no data" message is displayed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new string Rtf
		{
			get { return base.Rtf; }
			set
			{

				if (string.IsNullOrEmpty(value))
					Text = Properties.Resources.kstidEmtpyRawRecView;
				else
					base.Rtf = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Store the dots per inch value;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			using (Graphics g = CreateGraphics())
				m_pixelsPerInch = g.DpiX;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFonts()
		{
			UpdateFonts(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the RTF header, the bulk of which, is the font table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateFonts(bool updateRecord)
		{
			m_uiFontSize = (int)FontHelper.UIFont.SizeInPoints * 2;

			if (App.Project == null)
				return;

			m_fontSizes = new Dictionary<string, int>();
			foreach (var field in App.Project.Fields.Where(f => f.Font != null))
			{
				m_fontSizes[field.Name] = (int)(field.Font.SizeInPoints * 2);
				m_fonts[field.Name] = field.Font;
			}

			m_rtf = khdr + RtfHelper.FontTable(m_fontNumbers, ref m_uiFontNumber);

			if (updateRecord)
				UpdateRecord(m_recEntry, true);
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateRecord(RecordCacheEntry entry)
		{
			UpdateRecord(entry, false);
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateRecord(RecordCacheEntry entry, bool forceUpdate)
		{
			if (entry == null)
			{
				m_recEntry = entry;
				Rtf = string.Empty;
				return;
			}

			// Don't bother rebuilding the RTF if the record hasn't changed.
			if (entry == m_recEntry && !forceUpdate)
				return;

			if (m_fontSizes.Count != App.Project.Fields.Count())
				UpdateFonts(false);

			m_recEntry = entry;
			GetFieldsAndValuesFromRecord();

			// For now, assume there is only one column of
			// fields and that it will contain all the fields.
			m_rowsInCol1 = m_rtfFields.Count;
			if (m_numInterlinearFields > 0)
				GetInterlinearTabStopLocations();
			else
			{
				// There are no interlinear fields, so two columns of fields will be
				// displayed. Therefore, figure out how many rows in column one. That
				// will imply how many in column two.
				m_rowsInCol1 /= 2;
				if ((m_rtfFields.Count & 1) > 0)
					m_rowsInCol1++;
			}

			string firstLineTabStopsString;
			string subordinateTabStopsString;
			string tabStopsString = GetTabStops(out firstLineTabStopsString,
				out subordinateTabStopsString);

			FormatRTF(tabStopsString, firstLineTabStopsString, subordinateTabStopsString);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Apply the bold and/or italic styles if applicable.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string ApplyFontStyle(Font dataFont, bool beginFormatting)
		{
			string fmt = string.Empty;

			if (!beginFormatting)
				fmt = (dataFont.Bold || dataFont.Italic ? "}" : string.Empty);
			else
			{
				if (dataFont.Bold || dataFont.Italic)
					fmt = "{";
				if (dataFont.Bold)
					fmt += kbold;
				if (dataFont.Italic)
					fmt += kitalic;
			}

			return fmt;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs all the final steps (i.e. formatting all the information into a big RTF
		/// blob) for setting the text box's RTF.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FormatRTF( string tabStopsString,
			string firstLineTabStopsString, string subordinateTabStopsString)
		{
			StringBuilder lines = new StringBuilder(m_rtf);
			Dictionary<int, int> colorReferences;
			Color clrFieldLabel = Settings.Default.RecordViewFieldLabelColor;
			lines.AppendLine();
			lines.AppendLine(RtfHelper.ColorTable(clrFieldLabel, out colorReferences));

			GetLargestFontInfo();
			m_fieldLabelColorRefNumber = colorReferences[clrFieldLabel.ToArgb()];

			for (int i = 0; i < m_rowsInCol1; i++)
			{
				lines.AppendLine(@"\pard\plain");

				if (m_useExactLineSpacing)
				{
					// The default line spacing is usually larger than necessary, especially if
					// one of the fonts is Doulos SIL. Therefore, scrunch them together a little.
					lines.AppendFormat(@"\sl-{0}\slmult0", m_lineSpacing);
				}

				if (m_rtfFields[i].isInterlinearField)
				{
					FormatInterlinearForRTF(m_rtfFields[i], lines,
						firstLineTabStopsString, subordinateTabStopsString);
				}
				else
				{
					lines.AppendLine(tabStopsString);
					
					int dataFontNumber = m_fontNumbers[m_rtfFields[i].field];
					int dataFontSize = m_fontSizes[m_rtfFields[i].field];
					Font dataFont = m_fonts[m_rtfFields[i].field];

					lines.AppendFormat(kFmtOneLineOneCol, new object[] {m_fieldLabelColorRefNumber,
						m_uiFontSize, m_uiFontNumber, m_rtfFields[i].label});

					lines.Append(ApplyFontStyle(dataFont, true));
					lines.AppendFormat(kline, dataFontNumber, dataFontSize, m_rtfFields[i].fieldValue);
					lines.Append(ApplyFontStyle(dataFont, false));

					if (m_rowsInCol1 + i < m_rtfFields.Count)
					{
						lines.Append("\\tab ");
						dataFontNumber = m_fontNumbers[m_rtfFields[m_rowsInCol1 + i].field];
						dataFontSize = m_fontSizes[m_rtfFields[m_rowsInCol1 + i].field];
						dataFont = m_fonts[m_rtfFields[m_rowsInCol1 + i].field];

						lines.AppendFormat(kFmtOneLineOneCol, new object[] {m_fieldLabelColorRefNumber,
							m_uiFontSize, m_uiFontNumber, m_rtfFields[m_rowsInCol1 + i].label});

						lines.Append(ApplyFontStyle(dataFont, true));
						lines.AppendFormat(kline, dataFontNumber, dataFontSize,
							m_rtfFields[m_rowsInCol1 + i].fieldValue);
						lines.Append(ApplyFontStyle(dataFont, false));
					}

					if (m_useExactLineSpacing)
					{
						// Add a zero width space at the end of the line using the largest font so all
						// the lines will have uniform spacing between. I tried using a regular space
						// but the RTF control ignored it. I also tried forcing the line spacing using
						// the \slN RTF code, but that didn't seem to work either. It looked great in
						// Word, but not the RichTextBox. Grrr!
						lines.AppendFormat(@"\fs{0}\f{1} {2}", m_maxFontSize, m_maxFontNumber,
							kZeroWidthSpace);
					}

					lines.AppendLine(@"\par");
				}
			}

			lines.Append("}");
			Rtf = RtfHelper.TranslateUnicodeChars(lines.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetLargestFontInfo()
		{
			m_useExactLineSpacing = Settings.Default.RTFRecordViewUseExactLineSpacing;
			
			if (!m_useExactLineSpacing)
				return;

			float exactLineHeightMultiplier =
				Settings.Default.RTFRecordViewPercentageOfExactLineHeightToUse / 100f;
						
			float dpiY;
			using (Graphics g = CreateGraphics())
				dpiY = g.DpiY;

			// Figure out which font is the tallest.
			m_maxFontSize = m_uiFontSize;
			m_maxFontNumber = m_uiFontNumber;
			float maxFontHeight = FontHelper.UIFont.GetHeight(dpiY);

			foreach (var fldInfo in m_rtfFields)
			{
				var field = App.Project.GetFieldForName(fldInfo.field);

				if (field != null && maxFontHeight < field.Font.GetHeight(dpiY))
				{
					maxFontHeight = field.Font.GetHeight(dpiY);
					m_maxFontSize = m_fontSizes[fldInfo.field];
					m_maxFontNumber = m_fontNumbers[fldInfo.field];
				}
			}

			m_lineSpacing = (int)((maxFontHeight / 72) * 1440.0);
			m_lineSpacing = (int)(m_lineSpacing * exactLineHeightMultiplier);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FormatInterlinearForRTF(RTFFieldInfo rtfField, StringBuilder bldr,
			string firstLineTabStopsString, string subordinateTabStopsString)
		{
			bldr.AppendLine((rtfField.isFirstLine ?
				firstLineTabStopsString : subordinateTabStopsString));

			int dataFontNumber = m_fontNumbers[rtfField.field];
			int dataFontSize = m_fontSizes[rtfField.field];
			Font dataFont = m_fonts[rtfField.field];

			bldr.AppendFormat(kFmtOneLineOneCol, new object[] {m_fieldLabelColorRefNumber,
				m_uiFontSize, m_uiFontNumber, rtfField.label});

			bldr.AppendFormat(kline, dataFontNumber, dataFontSize, string.Empty);

			for (int col = 0; col < rtfField.columnValues.Length; col++)
			{
				// The first field dosen't have sub columns.
				if (rtfField.isFirstLine)
				{
					bldr.Append(ApplyFontStyle(dataFont, true));
					bldr.Append(rtfField.columnValues[col]);
					bldr.Append(ApplyFontStyle(dataFont, false));
					bldr.Append("\\tab ");
				}
				else
				{
					// Go through the column's sub columns
					foreach (string subcolValue in rtfField.parsedColValues[col])
					{
						bldr.Append(ApplyFontStyle(dataFont, true));
						bldr.Append(subcolValue);
						bldr.Append(ApplyFontStyle(dataFont, false));
						bldr.Append("\\tab ");
					}
				}
			}

			// Strip off the last tab and append the correct line ending marker.
			bldr.Remove(bldr.Length - 5, 5);

			if (m_useExactLineSpacing)
			{
				// Add a zero width space at the end of the line using the largest font so all
				// the lines will have uniform spacing between. I tried using a regular space
				// but the RTF control ignored it. I also tried forcing the line spacing using
				// the \slN RTF code, but that didn't seem to work either. It looked great in
				// Word, but not the RichTextBox. Grrr!
				bldr.AppendFormat(@"\fs{0}\f{1} {2}", m_maxFontSize, m_maxFontNumber,
					kZeroWidthSpace);
			}

			bldr.AppendLine(@"\par");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Stores record's field names and their values in a collection. Only fields whose
		/// values are non null are saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetFieldsAndValuesFromRecord()
		{
			m_rtfFields.Clear();
			m_numInterlinearFields = 0;
			m_firstILLineTabs = null;
			m_subordinateILLineTabs = null;

			using (Graphics g = CreateGraphics())
			{
				// If there's more than one interlinear field,
				// then get the information for those fields.
				if (m_recEntry.HasInterlinearData || 
					(m_recEntry.InterlinearFields != null && m_recEntry.InterlinearFields.Count > 1))
				{
					GetInterlinearFieldsAndValues(g);
				}

				// Go through the fields in the record and use reflection to get their values
				// from the record cache entry. Don't bother with fields whose value is null,
				// those that aren't supposed to be visible in the record view, and those
				// that are interlinear (interlinear fields are handled above).
				foreach (var field in App.Project.Fields)
				{
					string fieldValue = m_recEntry[field.Name];
					if (!field.VisibleInRecView || field.DisplayIndexInRecView < 0 ||
						fieldValue == null || m_recEntry.IsInterlinearField(field.Name))
					{
						continue;
					}

					// Save the field name, it's displayable name (e.g. Freeform = Free Form)
					// and the field's value. Replace any backslashes with double ones for
					// the sake of RTF.
					var info = new RTFFieldInfo();
					info.field = field.Name;
					info.label = field.DisplayName;
					info.displayIndex = field.DisplayIndexInRecView;
					info.fieldValue = fieldValue.Replace("\\", "\\\\");

					// All headers are bold
					using (var headerFont = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold))
					{
						info.labelWidth = TextRenderer.MeasureText(g, field.DisplayName,
							headerFont, Size.Empty, kTxtFmtFlags).Width;
					}

					info.valueWidth = TextRenderer.MeasureText(g, fieldValue, field.Font,
						Size.Empty, kTxtFmtFlags).Width;

					m_rtfFields.Add(info);
				}
			}

			// Now sort the list on the order in which the fields should be displayed.
			m_rtfFields.Sort((x, y) => x.displayIndex.CompareTo(y.displayIndex));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the interlinear fields information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetInterlinearFieldsAndValues(IDeviceContext g)
		{
			if (string.IsNullOrEmpty(m_recEntry.FirstInterlinearField))
				return;

			SortedList sortedFieldInfo = new SortedList();
			var tmpFields = new List<RTFFieldInfo>();

			foreach (var fieldName in m_recEntry.InterlinearFields)
			{
				var field = App.Project.GetFieldForName(fieldName);
				string[] colValues = m_recEntry.GetParsedFieldValues(field, true);
				if (field == null || !field.VisibleInRecView ||
					field.DisplayIndexInRecView < 0 || colValues == null)
				{
					continue;
				}

				var info = new RTFFieldInfo();
				info.isInterlinearField = true;
				info.field = field.Name;
				info.label = field.DisplayName;
				info.displayIndex = field.DisplayIndexInRecView;
				info.columnValues = colValues;
				using (var headerFont = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold))
				{
					info.labelWidth = TextRenderer.MeasureText(g, info.label,
						headerFont, Size.Empty, kTxtFmtFlags).Width;
				}

				// Sort the info by their display order
				sortedFieldInfo.Add(info.displayIndex, info);
			}

			bool firstField = true;
			foreach (RTFFieldInfo info in sortedFieldInfo.Values)
			{
				if (firstField)
				{
					// The first field is the one with the lowest DisplayIndexInRecView
					firstField = false;
					info.isFirstLine = true;
					tmpFields.Insert(0, info);
				}
				else
				{
					info.parsedColValues = new Dictionary<int, string[]>();
					tmpFields.Add(info);
				}
			}

			// Now parse the columns into sub columns for interlinear
			// rows that are not the first interlinear row.
			if (tmpFields.Count > 1) // Make sure there are interlinear rows to parse
			{
				for (int col = 0; col < tmpFields[0].columnValues.Length; col++)
				{
					// Copy the values for each row for a single column.
					List<string> colValues = new List<string>();
					for (int row = 1; row < tmpFields.Count; row++)
					{
						// If the current row doesn't have an value for
						// the current column just add an empty space.
						colValues.Add(col >= tmpFields[row].columnValues.Length ? string.Empty :
							tmpFields[row].columnValues[col]);
					}

					Dictionary<int, List<string>> parsedColValues =
						GetInterlinearSubColumnContents(colValues);

					// After parsing a single column, we need to save the results.
					for (int row = 1; row < tmpFields.Count; row++)
						tmpFields[row].parsedColValues[col] = parsedColValues[row - 1].ToArray();
				}
			}

			m_numInterlinearFields = tmpFields.Count;
			m_rtfFields.AddRange(tmpFields);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates the tab stops for non interlinear fields. If interlinear fields are
		/// present in the current record, then the tab stops for the interlinear fields are
		/// adjusted to account for the field labels and the calculation made to determine
		/// which field label is the widest.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetTabStops(out string firstLineTabStopsString,
			out string subordinateTabStopsString)
		{
			firstLineTabStopsString = null;
			subordinateTabStopsString = null;

			List<int> tabStops = new List<int>();
			int maxLabelWidthCol1 = 0;
			int maxValueWidthCol1 = 0;
			int maxLabelWidthCol2 = 0;

			// Calculate which field label is the widest for column one, and, If there are no
			// interlinear fields, calculate the field label that's widest for column two.
			for (int i = 0; i < m_rtfFields.Count; i++)
			{
				if (i >= m_rowsInCol1 && m_numInterlinearFields == 0)
					maxLabelWidthCol2 = Math.Max(maxLabelWidthCol2, m_rtfFields[i].labelWidth);
				else
				{
					maxLabelWidthCol1 = Math.Max(maxLabelWidthCol1, m_rtfFields[i].labelWidth);
					maxValueWidthCol1 = Math.Max(maxValueWidthCol1, m_rtfFields[i].valueWidth);
				}
			}

			// Calculate tab stop between 1st column's field label and the field query.
			float stopLocation = (maxLabelWidthCol1 / m_pixelsPerInch) *
				1440 + kGapBetweenLabelAndValue;
			tabStops.Add((int)stopLocation);

			// Calculate tab stop between 1st column's field query and 2nd column's field label.
			stopLocation += (maxValueWidthCol1 / m_pixelsPerInch) *
				1440 + kGapBetweenColumns;
			tabStops.Add((int)stopLocation);

			if (m_numInterlinearFields == 0)
			{
				// Tab between 2nd column's field label and the 2nd column's field query.
				stopLocation += (maxLabelWidthCol2 / m_pixelsPerInch) *
					1440 + kGapBetweenLabelAndValue;
				tabStops.Add((int)stopLocation);
			}
			else
			{
				// Adjust all the previously calculated tab stops between and within
				// interlinear columns to account for each field's label.
				for (int i = 0; i < m_firstILLineTabs.Count; i++)
					m_firstILLineTabs[i] += tabStops[0];

				for (int i = 0; i < m_subordinateILLineTabs.Count; i++)
					m_subordinateILLineTabs[i] += tabStops[0];

				// Now add to the interlinear set of tab stops the stop between the
				// interlinear field's label and the interlinear's field query.
				m_firstILLineTabs.Insert(0, tabStops[0]);
				m_subordinateILLineTabs.Insert(0, tabStops[0]);
			}

			// Now create an RTF string for all the tab stops,
			// starting with the interlinear tab stops.
			StringBuilder bldrTabStops = new StringBuilder();

			if (m_numInterlinearFields > 0)
			{
				foreach (int stop in m_firstILLineTabs)
					bldrTabStops.AppendFormat(ktab, stop);
				firstLineTabStopsString = bldrTabStops.ToString();

				bldrTabStops.Length = 0;
				foreach (int stop in m_subordinateILLineTabs)
					bldrTabStops.AppendFormat(ktab, stop);
				subordinateTabStopsString = bldrTabStops.ToString();
			}

			// Now for the non interlinear fields tab stops.
			bldrTabStops.Length = 0;
			foreach (int stop in tabStops)
				bldrTabStops.AppendFormat(ktab, stop);

			return bldrTabStops.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetInterlinearTabStopLocations()
		{
			List<int> firstLineWidths;
			Dictionary<int, List<int>> subColWidths;
			GetInterlinearColWidths(out firstLineWidths, out subColWidths);

			if (firstLineWidths.Count == 0 || subColWidths.Count == 0)
				return;

			m_firstILLineTabs = new List<int>();
			m_subordinateILLineTabs = new List<int>();
			float stopLocation = 0;

			// Go through the widths of each column.
			for (int col = 0; col < firstLineWidths.Count; col++)
			{
				// Go through the max widths for the sub columns
				for (int subcol = 0; subcol < subColWidths[col].Count; subcol++)
				{
					stopLocation +=
						(subColWidths[col][subcol] / m_pixelsPerInch) * 1440 +
						kInterlinearColPadding;

					m_subordinateILLineTabs.Add((int)stopLocation);
				}

				m_firstILLineTabs.Add((int)stopLocation);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates two lists of column widths. The first are the widths of the first line
		/// interlinear columns. The second list of widths for each sub column within each
		/// interlinear column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetInterlinearColWidths(
			out List<int> firstLineWidths, out Dictionary<int, List<int>> subColWidths)
		{
			// The key to this dictionary is the column number 
			subColWidths = new Dictionary<int, List<int>>();
			firstLineWidths = new List<int>();
			List<int> maxSubColWidths;

			RTFFieldInfo firstField;
			RTFFieldInfo secondField;
			GetFirstAndSecondInterlinearFields(out firstField, out secondField);

			var firstLineFont = App.Project.GetFieldForName(firstField.field).Font;
			int numInterlinearColumns = firstField.columnValues.Length;

			using (var g = CreateGraphics())
			{
				// Iterate through the interlinear columns.
				for (int col = 0; col < numInterlinearColumns; col++)
				{
					maxSubColWidths = new List<int>();
					int totalColWidth = 0;
					int numSubColumnsInCurrCol;
					// Test to see if there is only 1 interlinear field
					if (secondField == null || secondField.parsedColValues == null)
						numSubColumnsInCurrCol = 0;
					else
						numSubColumnsInCurrCol = secondField.parsedColValues[col].Length;

					int firstLineColWidth = TextRenderer.MeasureText(g,
						firstField.columnValues[col], firstLineFont, Size.Empty,
						kTxtFmtFlags).Width;

					// Iterate through the sub columns of the current
					// column to find which one is the widest.
					for (int subcol = 0; subcol < numSubColumnsInCurrCol; subcol++)
					{
						int maxWidth = GetMaxSubColumnWidth(g, col, subcol, ref totalColWidth);
						if (numSubColumnsInCurrCol == 1)
						{
							maxWidth = Math.Max(firstLineColWidth, maxWidth);
							totalColWidth = maxWidth;
						}

						maxSubColWidths.Add(maxWidth);
					}

					subColWidths[col] = maxSubColWidths;
					firstLineWidths.Add(totalColWidth);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Find the first and second interlinear fields in the RTF fields collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetFirstAndSecondInterlinearFields(out RTFFieldInfo firstField,
			out RTFFieldInfo secondField)
		{
			firstField = null;
			secondField = null;
			
			foreach (RTFFieldInfo rtfField in m_rtfFields)
			{
				//if (rtfField.isFirstLine)
				if (rtfField.isInterlinearField)
				{
					rtfField.isFirstLine = true;
					firstField = rtfField;
					int firstFieldIndex = m_rtfFields.IndexOf(firstField);
					// Find the second interlinear field
					for (int i = firstFieldIndex + 1; i < m_rtfFields.Count; i++)
					{
						if (m_rtfFields[i].isInterlinearField)
						{
							secondField = m_rtfFields[i];
							break;
						}
					}
					return;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Go through each row after the first interlinear row for the specified sub column
		/// and determine which row has the widest sub column content.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetMaxSubColumnWidth(IDeviceContext g, int col, int subcol, ref int accumulatedWidth)
		{
			RTFFieldInfo firstField;
			RTFFieldInfo secondField;
			GetFirstAndSecondInterlinearFields(out firstField, out secondField);

			int startIndex = m_rtfFields.IndexOf(secondField);
			int maxSubColWidth = 0;
			for (int row = startIndex; row < m_rtfFields.Count; row++)
			{
				if (!m_rtfFields[row].isInterlinearField || m_rtfFields[row].parsedColValues == null ||
					subcol >= m_rtfFields[row].parsedColValues[col].Length)
				{
					continue;
				}

				var field = App.Project.GetFieldForName(m_rtfFields[row].field);
				int width = TextRenderer.MeasureText(g, m_rtfFields[row].parsedColValues[col][subcol],
					field.Font, Size.Empty, kTxtFmtFlags).Width;

				maxSubColWidth = Math.Max(width, maxSubColWidth);
				accumulatedWidth += width;
			}

			return maxSubColWidth;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Builds a single line and column for the RTF.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void BuildColumnLine(int i, ref StringBuilder bldr)
		//{
		//    int dataFontNumber = m_fontNumbers[m_rtfFields[i].field];
		//    int dataFontSize = m_fontSizes[m_rtfFields[i].field];
		//    Font dataFont = m_fonts[m_rtfFields[i].field];

		//    bldr.Append("{" + kbold);
		//    bldr.AppendFormat(kline, m_uiFontNumber, m_uiFontSize, m_rtfFields[i].label);
		//    bldr.Append(":}\\tab");

		//    bldr.Append(ApplyFontStyle(dataFont, true));
		//    bldr.AppendFormat(kline, dataFontNumber, dataFontSize, m_rtfFields[i].fieldValue);
		//    bldr.Append(ApplyFontStyle(dataFont, false));
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the contents for a single interlinear column. The contents may contain sub
		/// columns. A collection of sub columns is returned.
		/// </summary>
		/// <remarks>
		/// Take the following example as illustration:
		/// 
		/// \tx familia yangu        inaishi         nakuru.
		/// \mb familia y-   angu    i-   na-   ishi Nakuru
		/// \ge family  cls- 1S POSS cl-  PRES- live Nakuru
		/// \ps n       prf- pos     prf- TEMP- v    prnm
		///
		/// The column under "inaishi" contains three sub columns that must be lined up in
		/// the RTF. This method will create a dictionary whose key values are the field
		/// name and whose values are strings of sub column contents.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		private static Dictionary<int, List<string>> GetInterlinearSubColumnContents(
			List<string> unparsedColValues)
		{
			var parsedColContent = new Dictionary<int, List<string>>();

			// No need to do any parsing if there's only one row's worth of information.
			if (unparsedColValues.Count == 1)
			{
				parsedColContent[0] = unparsedColValues;
				return parsedColContent;
			}

			// Find the longest unparsed interlinear field.
			int maxWidth = unparsedColValues.Aggregate(0, (current, t) => Math.Max((sbyte) current, (sbyte) t.Length));

			// Preallocate space for the returned values and pad the
			// unparsed column values so each value is the same width.
			for (int i = 0; i < unparsedColValues.Count; i++)
			{
				unparsedColValues[i] = unparsedColValues[i].PadRight(maxWidth, ' ');
				parsedColContent[i] = new List<string>();
			}

			int[] indexes = new int[unparsedColValues.Count];
			int startIndex = 0;
			bool done = false;
			while (!done)
			{
				for (int i = 0; i < unparsedColValues.Count; i++)
					indexes[i] = FindPotentialSubColEnd(unparsedColValues[i], indexes[i]);

				if (SubColumnFound(ref indexes))
				{
					for (int i = 0; i < unparsedColValues.Count; i++)
					{
						done = (indexes[0] == unparsedColValues[i].Length);
						parsedColContent[i].Add(
							unparsedColValues[i].Substring(startIndex, indexes[0] - startIndex).Trim());
					}

					startIndex = indexes[0];
				}
			}

			return parsedColContent;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldContent"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private static int FindPotentialSubColEnd(string fieldContent, int startIndex)
		{
			if (startIndex == fieldContent.Length)
				return startIndex;

			int i = startIndex;
			while (i < fieldContent.Length - 1)
			{
				if (fieldContent[i] == ' ' && fieldContent[i + 1] != ' ')
					break;

				i++;
			}

			return i + 1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if all the indexes are equal.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool SubColumnFound(ref int[] indexes)
		{
			if (indexes.Length <= 1)
				return true;

			bool found = true;
			for (int i = 0; i < indexes.Length - 1 && found; i++)
			{
				if (indexes[i] != indexes[i + 1])
					found = false;
			}

			if (found)
				return true;

			// When indexes are not all the same then find the index
			// with the smallest value and set all of them to that value.
			int minIndex = indexes.Aggregate(int.MaxValue, (current, t) => Math.Min((sbyte) current, (sbyte) t));

			for (int i = 0; i < indexes.Length; i++)
				indexes[i] = minIndex;

			return false;
		}
	}
}
