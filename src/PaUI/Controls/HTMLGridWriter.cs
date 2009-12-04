using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a grid to an XML format that is transformed into
	/// an html file with an accompanying cascading style sheet.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class HTMLGridWriter : HTMLWriterBase
	{
		private SortedList<int, DataGridViewColumn> m_sortedColList;
		private readonly PaWordListGrid m_grid;
		private readonly bool m_isForSearchResult;
		private readonly bool m_writeGrpHdgCount = false;
		private readonly int m_grpHdgRowColSpan = 0;

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

				if (m_grid.GroupByField != null || m_grid.Cache.IsCIEList)
				{
					m_groupHeadingFont = (m_grid.Cache.IsCIEList ?
						FontHelper.PhoneticFont : m_grid.GroupByField.Font);

					// Span all the columns in the table.
					m_grpHdgRowColSpan = m_sortedColList.Count + (m_isForSearchResult ? 2 : 0);

					// If there is more than 1 column (or 3 in case where the phonetic column is
					// displaying search results) then show the number of child records in the
					// group. When that happens, then we need to adjust the colspan for the
					// group heading text so it spans all but the last column in the table. The
					// last column is reserved for the child count.
					m_writeGrpHdgCount =
						!(m_grpHdgRowColSpan == 1 || (m_isForSearchResult && m_grpHdgRowColSpan == 3));

					if (m_writeGrpHdgCount)
						m_grpHdgRowColSpan--;
				}

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
			Debug.Assert(rootAttribValues.Length >= 1);

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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string ModifyCSS(string xslContent)
		{
			xslContent = base.ModifyCSS(xslContent);
		
			// Make sure the right border of the text portion (not the count portion)
			// of group headings is turned off, which is just uncommenting the CSS
			// setting that's already in the xsl file.
			if (m_writeGrpHdgCount)
			{
				xslContent = xslContent.Replace("/*==|", string.Empty);
				xslContent = xslContent.Replace("|==*/", string.Empty);
			}

			return xslContent;
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
				if (col.Visible && !(col is SilHierarchicalGridColumn))
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

			for (int i = 0; i < m_grid.RowCount; i++)
			{
				OpenRow(false);

				if (m_grid.Rows[i] is SilHierarchicalGridRow)
					WriteGroupHeadingRow(m_grid.Rows[i] as SilHierarchicalGridRow);
				else
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
					int cacheRow = ((row is PaCacheGridRow) ?
						((PaCacheGridRow)row).CacheEntryIndex : row.Index);

					WordListCacheEntry entry = m_grid.Cache[cacheRow];
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
		/// Write a single grid row's worth of query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteGroupHeadingRow(SilHierarchicalGridRow row)
		{
			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", "groupheadtext");
			element.SetAttribute("colspan", m_grpHdgRowColSpan.ToString());
			XmlNode node = m_currNode.AppendChild(element);
			SetNodesText(node, row.Text);

			if (m_writeGrpHdgCount)
			{
				element = m_xmlDoc.CreateElement("td");
				element.SetAttribute("class", "groupheadcount");
				node = m_currNode.AppendChild(element);
				SetNodesText(node, string.Format(row.CountFormatStrings[0], row.ChildCount));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes a single XML row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteRowDataValue(string field, string value)
		{
			string modifiedName;
			if (m_modifiedFieldNames.TryGetValue(field, out modifiedName))
				field = modifiedName;

			XmlElement element = m_xmlDoc.CreateElement("td");
			element.SetAttribute("class", field);
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
