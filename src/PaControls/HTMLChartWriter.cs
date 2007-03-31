using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using SIL.SpeechTools.Utils;
using SIL.Pa.Resources;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a vowel or consonant chart to an XML format that
	/// is transformed into an html file with an accompanying cascading style sheet.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class HTMLChartWriter : HTMLWriterBase
	{
		private bool m_colSubHeadingsVisible = false;
		private bool m_rowSubHeadingsVisible = false;
		private CharGrid m_chrGrid;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Exports the phone chart to an XML, then to HTML.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string Export(CharGrid chrGrid, string defaultFileName, string chartName)
		{
			HTMLChartWriter writer = new HTMLChartWriter(chrGrid, defaultFileName, chartName);
			return writer.HtmlOutputFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create an instance of the writer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private HTMLChartWriter(CharGrid chrGrid, string defaultFileName, string chartName)
			: base(defaultFileName, new string[] { chartName })
		{
			if (!m_error)
			{
				m_chrGrid = chrGrid;
				m_colSubHeadingsVisible = m_chrGrid.ColumnHeadersCollectionPanel.AreAnySubHeadingsVisible;
				m_rowSubHeadingsVisible = m_chrGrid.RowHeadersCollectionPanel.AreAnySubHeadingsVisible;

				WriteColumnHeadings();
				WriteBody();
				WriteHTMLFile();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void WriteOuterElements(XmlWriter writer, string languageName,
			string[] rootAttribValues)
		{
			System.Diagnostics.Debug.Assert(rootAttribValues.Length >= 1);

			writer.WriteStartElement("table");
			writer.WriteAttributeString("language", languageName);
			writer.WriteAttributeString("chartType", rootAttribValues[0]);
			writer.WriteEndElement();
		}
		

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the XSL file to use to transform the XML to xhtml.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string XSLFileName
		{
			get	{return "ChartXml2Xhtml.xsl";}
		}

		#region Methods for writing the header nodes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the contents of the "thead" node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteColumnHeadings()
		{
			OpenHead();
			WriteTopLeftCornerHeading();

			char col = (m_rowSubHeadingsVisible ? 'C' : 'B');
			foreach (CharGridHeader hdr in m_chrGrid.ColumnHeaders)
			{
				AddColumnHeading(col, hdr.SubHeadingsVisible,
					hdr.OwnedColumns.Count, hdr.HeadingText);

				char subCol = col;
				foreach (Label subHdr in hdr.SubHeaders)
				{
					AddColumnSubHeading(subCol, subHdr.Text);
					subCol = (char)((int)subCol + 1);
				}

				col = (char)((int)col + hdr.OwnedColumns.Count);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the thead node and the column heading row node. It also adds the sub
		/// heading row node if there are any column sub headings turned on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OpenHead()
		{
			XmlElement element = m_xmlDoc.CreateElement("thead");
			m_currNode = m_currNode.AppendChild(element);
			OpenRow(1, true);

			// Create a row for sub headings if any are visible.
			if (m_colSubHeadingsVisible)
				OpenRow(2, true);
			
			// Select the first row's node.
			m_currNode = m_xmlDoc.SelectSingleNode("table/thead").FirstChild;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will write the information for the top, left cell in the grid, which
		/// is considered part of the heading. It is assumed the current node is pointing
		/// to the correct node before this method is called.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteTopLeftCornerHeading()
		{
			// Add the top, left corner cell that holds no query
			XmlElement element = m_xmlDoc.CreateElement("td");

			int numColsSpanned = (m_rowSubHeadingsVisible ? 2 : 1);
			string numRowsSpanned = (m_colSubHeadingsVisible ? "2" : "1");
			string cellEnd = numRowsSpanned + "*" + (char)((numColsSpanned - 1) + (int)'A');

			element.SetAttribute("class", "colhead");
			element.SetAttribute("cellstart", "1*A");
			element.SetAttribute("cellend", cellEnd);
			element.SetAttribute("colspan", numColsSpanned.ToString());
			element.SetAttribute("rowspan", numRowsSpanned);

			m_currNode.AppendChild(element);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes a single column heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumnHeading(char col1, bool columnsSubHeadingsVisible,
			int numColsSpanned, string text)
		{
			GotoHeaderNode();

			// m_colSubHeadingsVisible = true if there are any column sub
			// headings showing in any column; columnsSubHeadingsVisible = true
			// if the sub headings are showing in this column.
			string numRowsSpanned =
				(m_colSubHeadingsVisible && !columnsSubHeadingsVisible ? "2" : "1");

			string row2 = (m_colSubHeadingsVisible ? "2" : "1");
			char col2 = (char)((int)col1 + numColsSpanned - 1);

			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "colhead" + (columnsSubHeadingsVisible ? "p" : "s"));
			element.SetAttribute("cellstart", "1*" + col1.ToString());
			element.SetAttribute("cellend", row2 + "*" + col2.ToString());
			element.SetAttribute("colspan", numColsSpanned.ToString());
			element.SetAttribute("rowspan", numRowsSpanned);
			XmlNode node = m_currNode.AppendChild(element);
			SetNodesText(node, text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes a single column sub heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumnSubHeading(char col, string text)
		{
			if (!m_colSubHeadingsVisible || string.IsNullOrEmpty(text))
				return;

			GotoSubHeaderNode();

			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "colheadc");
			element.SetAttribute("cellstart", "2*" + col);
			XmlNode node = m_currNode.AppendChild(element);
			SetNodesText(node, text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes to the thead/R1 node
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GotoHeaderNode()
		{
			m_currNode = m_xmlDoc.SelectSingleNode("table/thead");
			m_currNode = m_currNode.FirstChild;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes to the thead/R2 node
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GotoSubHeaderNode()
		{
			if (m_colSubHeadingsVisible)
			{
				m_currNode = m_xmlDoc.SelectSingleNode("table/thead");
				m_currNode = m_currNode.LastChild;
			}
		}

		#endregion

		#region Methods for writing the body
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the "tbody" node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteBody()
		{
			OpenBody();

			int row = (m_colSubHeadingsVisible ? 3 : 2);
			foreach (CharGridHeader hdr in m_chrGrid.RowHeaders)
			{
				OpenRow(row, false);
				AddRowHeader(row, hdr.SubHeadingsVisible, hdr.OwnedRows.Count, hdr.HeadingText);

				int subHdr = 0;
				foreach (DataGridViewRow gridRow in hdr.OwnedRows)
				{
					if (subHdr > 0)
						OpenRow(row, false);
					
					if (hdr.SubHeadingsVisible)
						AddRowSubHeader(row, hdr.SubHeaders[subHdr].Text);
			
					AddCellData(row, gridRow);
					row++;
					subHdr++;
				}
			}
		}

		//private int GetNumberOfHeaderRowsToWrite(CharGridHeader hdr)
		//{
		//    int count = 0;

		//    // Go through each owned row and see if any contain query.
		//    for (int i = 0; i < hdr.OwnedRows.Count; i++)
		//    {
		//        bool empty = true;

		//        // First determine if any of the cells in the row have query.
		//        foreach (DataGridViewCell cell in hdr.OwnedRows[i].Cells)
		//        {
		//            if (cell.Value != null)
		//            {
		//                empty = false;
		//                break;
		//            }
		//        }

		//        // If at least one cell had query or there's a
		//        // sub-heading for the row then count the row.
		//        string subHeadText = (hdr.SubHeadingsVisible ? hdr.SubHeaders[i].Text : null);
		//        if (!empty || !string.IsNullOrEmpty(subHeadText))
		//            count++;
		//    }

		//    return count;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the tbody node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OpenBody()
		{
			m_currNode = m_xmlDoc.SelectSingleNode("table");
			XmlElement element = m_xmlDoc.CreateElement("tbody");
			m_currNode = m_currNode.AppendChild(element);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddRowHeader(int row1, bool rowSubHeadingsVisible, int numRowsSpanned,
			string text)
		{
			// m_rowSubHeadingsVisible = true if there are any row sub headings showing in any
			// row; rowSubHeadingsVisible = true if the sub headings are showing in this row.
			string numColsSpanned =
				(m_rowSubHeadingsVisible && !rowSubHeadingsVisible ? "2" : "1");
			
			int row2 = row1 + numRowsSpanned - 1;
			char col2 = (char)((int)'A' + (m_rowSubHeadingsVisible ? 1 : 0));

			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "rowhead" + (numRowsSpanned == 1 ? "s" : "p"));
			element.SetAttribute("cellstart", row1.ToString() + "*A");
			element.SetAttribute("cellend", row2.ToString() + "*" + col2.ToString());
			element.SetAttribute("colspan", numColsSpanned);
			element.SetAttribute("rowspan", numRowsSpanned.ToString());
			XmlNode node = m_currNode.AppendChild(element);
			SetNodesText(node, text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddRowSubHeader(int row, string text)
		{
			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "rowheadc");
			element.SetAttribute("cellstart", row.ToString() + "*B");
			XmlNode node = m_currNode.AppendChild(element);
			SetNodesText(node, text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddCellData(int row, DataGridViewRow gridRow)
		{
			char col = (m_rowSubHeadingsVisible ? 'C' : 'B');

			foreach (DataGridViewCell cell in gridRow.Cells)
			{
				XmlElement element = m_xmlDoc.CreateElement("td");
				element.SetAttribute("class", "d");
				element.SetAttribute("cellstart", row.ToString() + "*" + col.ToString());
				XmlNode node = m_currNode.AppendChild(element);

				CharGridCell charCell = cell.Value as CharGridCell;
				string phone = (charCell == null ? null : charCell.Phone);
				SetNodesText(node, phone);
				col = (char)((int)col + 1);
			}
		}

		#endregion

		#region Helper methods.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new table row for the specified row number.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OpenRow(int rowNumber, bool inHeaderNode)
		{
			m_currNode = m_xmlDoc.SelectSingleNode("table/t" + (inHeaderNode ? "head" : "body"));
			XmlElement element = m_xmlDoc.CreateElement("tr");
			element.SetAttribute("id", "R" + rowNumber.ToString());
			m_currNode = m_currNode.AppendChild(element);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the text value of the specified node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetNodesText(XmlNode node, string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				XmlText textNode = m_xmlDoc.CreateTextNode(text);
				node.AppendChild(textNode);
			}
		}

		#endregion
	}
}
