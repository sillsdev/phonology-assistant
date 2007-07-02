using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a grid to an XML format that is transformed into
	/// an html file with an accompanying cascading style sheet.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class HTMLGridWriter : HTMLWriterBase
	{
		private PaWordListGrid m_grid;
		private SortedList<int, DataGridViewColumn> m_sortedColList;
		private bool m_isForSearchResult;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Exports the specified grid to XML, then HTML.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string Export(PaWordListGrid grid, string defaultFileName,
			string[] rootAttribValues)
		{
			HTMLGridWriter writer = new HTMLGridWriter(grid, defaultFileName, rootAttribValues);
			return writer.HtmlOutputFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create an instance of the writer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private HTMLGridWriter(PaWordListGrid grid, string defaultFileName, string[] rootAttribValues)
			: base(defaultFileName, rootAttribValues)
		{
			if (!m_error)
			{
				m_grid = grid;
				m_isForSearchResult = m_grid.Cache.IsForSearchResults;
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

			writer.WriteStartElement("PaDataExport");
			writer.WriteAttributeString("language", languageName);
			
			if (rootAttribValues.Length == 1)
				writer.WriteAttributeString("view", rootAttribValues[0]);
			else if (rootAttribValues.Length > 1)
			{
				writer.WriteAttributeString("query", rootAttribValues[0]);
				writer.WriteAttributeString("queryFormula", rootAttribValues[1]);
			}
			
			writer.WriteStartElement("table");
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the XSL file to use to transform the XML to xhtml.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string XSLFileName
		{
			get { return "GridXml2Xhtml.xsl"; }
		}
		
		#region Methods for writing the header nodes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the contents of the "thead" node.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteColumnHeadings()
		{
			XmlElement element = m_xmlDoc.CreateElement("thead");
			m_currNode = m_currNode.AppendChild(element);
			OpenRow(true);

			m_sortedColList = new SortedList<int, DataGridViewColumn>();

			// First, make a collection of visible columns in display order.
			foreach (DataGridViewColumn col in m_grid.Columns)
			{
				if (col.Visible)
					m_sortedColList[col.DisplayIndex] = col;
			}

			foreach (DataGridViewColumn col in m_sortedColList.Values)
			{
				element = m_xmlDoc.CreateElement("td");
				element.SetAttribute("class", "colhead");

				// If the column is phonetic then we know it will span three columns.
				PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[col.Name];
				if (m_isForSearchResult && fieldInfo != null && fieldInfo.IsPhonetic)
					element.SetAttribute("colspan", "3");
				
				XmlNode node = m_currNode.AppendChild(element);
				SetNodesText(node, col.HeaderText);
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
			XmlNode root = m_xmlDoc.DocumentElement;
			m_currNode = root.SelectSingleNode("table");
			XmlElement element = m_xmlDoc.CreateElement("tbody");
			m_currNode = m_currNode.AppendChild(element);

			for (int i = 0; i < m_grid.Rows.Count; i++)
			{
				OpenRow(false);
				WriteRowData(m_grid.Rows[i]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Write a single grid row's worth of query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteRowData(DataGridViewRow row)
		{
			foreach (DataGridViewColumn col in m_sortedColList.Values)
			{
				PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[col.Name];
				if (!m_isForSearchResult || fieldInfo == null || !fieldInfo.IsPhonetic)
					WriteRowDataValue(col.Name, row.Cells[col.Index].Value as string);
				else
				{
					WordListCacheEntry entry = m_grid.Cache[row.Index];
					int itemOffset = entry.SearchItemOffset;
					int itemLength = entry.SearchItemLength;

					StringBuilder bldr = new StringBuilder();
					for (int i = 0; i < itemOffset; i++)
						bldr.Append(entry.Phones[i]);

					WriteRowDataValue("phbefore", bldr.ToString());
					WriteRowDataValue("phtarget", entry.SearchItem);

					bldr.Length = 0;
					for (int i = itemOffset + itemLength; i < entry.Phones.Length; i++)
						bldr.Append(entry.Phones[i]);

					WriteRowDataValue("phafter", bldr.ToString());
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes a single XML row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteRowDataValue(string field, string value)
		{
			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "d " + field);
			XmlNode node = m_currNode.AppendChild(element);
			string text = value;
			SetNodesText(node, text);
		}

		#endregion

		#region Helper methods.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new table row for the specified row number.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void OpenRow(bool inHeaderNode)
		{
			XmlNode root = m_xmlDoc.DocumentElement;
			m_currNode = root.SelectSingleNode("table/t" + (inHeaderNode ? "head" : "body"));
			XmlElement element = m_xmlDoc.CreateElement("tr");
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
