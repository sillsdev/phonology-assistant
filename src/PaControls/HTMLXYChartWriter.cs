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
	public class HTMLXYChartWriter : HTMLWriterBase
	{
		private XYGrid m_xyGrid;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Exports the phone chart to an XML, then to HTML.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string Export(XYGrid xyGrid, string defaultFileName, string chartType,
			string chartName)
		{
			HTMLXYChartWriter writer = new HTMLXYChartWriter(xyGrid, defaultFileName,
				chartType, chartName);

			return writer.HtmlOutputFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create an instance of the writer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private HTMLXYChartWriter(XYGrid xyGrid, string defaultFileName, string chartType,
			string chartName) : base(defaultFileName, new string[] {chartType, chartName})
		{
			if (!m_error)
			{
				m_xyGrid = xyGrid;
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
			System.Diagnostics.Debug.Assert(rootAttribValues.Length == 2);

			writer.WriteStartElement("table");
			writer.WriteAttributeString("language", languageName);
			writer.WriteAttributeString("chartType", rootAttribValues[0]);
			
			if (!string.IsNullOrEmpty(rootAttribValues[1]))
				writer.WriteAttributeString("chartName", rootAttribValues[1]);
	
			writer.WriteEndElement();
		}
		

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the XSL file to use to transform the XML to xhtml.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string XSLFileName
		{
			get	{return "XYChartXml2Xhtml.xsl";}
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

			char col = 'B';

			// Start at the second column since the first is empty and end with
			// the second to last since the last one should also be empty.
			for (int i = 1; i < m_xyGrid.ColumnCount - 1; i++)
				AddColumnHeading((char)((int)col + (i - 1)), m_xyGrid[i, 0].Value as string);
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
			element.SetAttribute("class", "colhead");
			element.SetAttribute("cellstart", "1*A");
			element.SetAttribute("cellend", "1*A");
			element.SetAttribute("colspan", "1");
			element.SetAttribute("rowspan", "1");
			m_currNode.AppendChild(element);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes a single column heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumnHeading(char col, string text)
		{
			GotoHeaderNode();

			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "colhead" + "s");
			element.SetAttribute("cellstart", "1*" + col.ToString());
			element.SetAttribute("cellend", "1*" + col.ToString());
			element.SetAttribute("colspan", "1");
			element.SetAttribute("rowspan", "1");
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

			for (int i = 1; i < m_xyGrid.RowCount - 1; i++)
			{
				OpenRow(i, false);
				AddRowHeader(i, m_xyGrid[0, i].Value as string);
				AddCellData(i);
			}
		}

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
		private void AddRowHeader(int row, string text)
		{
			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "rowheads");
			element.SetAttribute("cellstart", (row + 1).ToString() + "*A");
			element.SetAttribute("cellend", (row + 1).ToString() + "*A");
			element.SetAttribute("colspan", "1");
			element.SetAttribute("rowspan", "1");
			XmlNode node = m_currNode.AppendChild(element);
			SetNodesText(node, text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddCellData(int row)
		{
			char col = 'B';

			for (int i = 1; i < m_xyGrid.ColumnCount - 1; i++)
			{
				XmlElement element = m_xmlDoc.CreateElement("td");
				element.SetAttribute("class", "d");
				element.SetAttribute("cellstart", (row + 1).ToString() + "*" + col.ToString());
				XmlNode node = m_currNode.AppendChild(element);

				string text = (m_xyGrid[i, row].Value == null ? string.Empty :
					m_xyGrid[i, row].Value.ToString());
				
				SetNodesText(node, text);
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
