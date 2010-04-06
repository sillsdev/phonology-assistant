// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ExporterBase.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Filters;
using SIL.Pa.UI.Controls;
using SilUtils;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ExporterBase
	{
		public enum OutputFormat
		{
			XHTML,
			WordXml
		}

		protected XmlWriter m_writer;
		protected DataGridView m_grid;
		protected int m_leftColSpanForGroupedList;
		protected int m_rightColSpanForGroupedList;
		protected readonly string m_groupedFieldName;
		protected readonly bool m_isGridGrouped;
		protected readonly PaProject m_project;
		protected readonly OutputFormat m_outputFormat;
		protected readonly string m_outputFileName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExporterBase(PaProject project, string outputFileName, DataGridView dgrid)
		{
			m_project = project;
			m_outputFileName = outputFileName;
			m_grid = dgrid;
			m_outputFormat = OutputFormat.XHTML;

			var grid = m_grid as PaWordListGrid;

			m_isGridGrouped = (grid != null && grid.IsGroupedByField && grid.GroupByColumn != null &&
					grid.Columns[0] is SilHierarchicalGridColumn);

			if (m_isGridGrouped)
			{
				int groupByColIndex = ((PaWordListGrid)m_grid).GroupByColumn.DisplayIndex;

				// Subtract on at the end to account for the column containing the
				// expand/collapse glyph. That doesn't count in the col. span.
				m_leftColSpanForGroupedList = (from x in m_grid.Columns.Cast<DataGridViewColumn>()
						where x.Visible && x.DisplayIndex < groupByColIndex
						select x).Count() - 1;

				m_rightColSpanForGroupedList = (from x in m_grid.Columns.Cast<DataGridViewColumn>()
						where x.Visible && x.DisplayIndex > groupByColIndex
						select x).Count();

				m_groupedFieldName = ProcessHelper.MakeAlphaNumeric(
					((PaWordListGrid)m_grid).GroupByField.DisplayText);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string Title
		{
			get { return "unknown title"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string View
		{
			get { return "unknown view"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string TableClass
		{
			get { return "unknown table class"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string SearchPattern
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool InternalProcess(bool keepIntermediateFile,
			params Pipeline.ProcessType[] processTypes)
		{
			System.Diagnostics.Debug.Assert(processTypes.Length > 0);
	
			// Create a stream of xml data containing the phones in the project.
			var inputStream = CreateXHTML();

			if (keepIntermediateFile)
			{
				var intermediateFileName = Path.ChangeExtension(m_outputFileName, "htm");
				ProcessHelper.WriteStreamToFile(inputStream, intermediateFileName);
			}

			// Create a processing pipeline for a series of xslt transforms to be applied to the stream.
			var processFileName = Path.Combine(App.ProcessingFolder, "Processing.xml");

			MemoryStream outputStream = null;

			foreach (var prsType in processTypes)
			{
				var pipeline = Pipeline.Create(prsType, processFileName, App.ProcessingFolder);

				// REVIEW: Should we warn the user that this failed?
				if (pipeline == null)
					continue;

				// Kick off the processing and then save the results to a file.
				outputStream = pipeline.Transform(inputStream);
				inputStream = outputStream;
			}

			ProcessHelper.WriteStreamToFile(outputStream, m_outputFileName, false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<DataGridViewColumn> GetGridColumns()
		{
			return from x in m_grid.Columns.Cast<DataGridViewColumn>()
					orderby x.DisplayIndex
					where x.Visible
					select x;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<DataGridViewRow> GetGridRows()
		{
			return from x in m_grid.Rows.Cast<DataGridViewRow>()
				   where x.Visible && x.Index != m_grid.NewRowIndex
				   select x;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<KeyValuePair<string, Font>> GetFormattingFieldInfo()
		{
			foreach (DataGridViewColumn col in GetGridColumns())
				yield return new KeyValuePair<string, Font>(col.HeaderText, col.DefaultCellStyle.Font);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual MemoryStream CreateXHTML()
		{
			var memStream = new MemoryStream();

			var settings = new XmlWriterSettings();
			settings.Indent = false;

			using (m_writer = XmlWriter.Create(memStream, settings))
			{
				m_writer.WriteStartDocument();
				m_writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");

				WriteHead();
				WriteBody();

				// Close html.
				m_writer.WriteEndElement();

				m_writer.Flush();
				m_writer.Close();
			}

			return memStream;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteHead()
		{
			m_writer.WriteStartElement("head");
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "meta", "http-equiv", "content-type");
			m_writer.WriteAttributeString("content", "text/html; charset=utf-8");
			m_writer.WriteEndElement();
			m_writer.WriteElementString("title", Title);

			// Close head
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteBody()
		{
			m_writer.WriteStartElement("body");
			WriteMetadata();
			WriteTable();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadata()
		{
			ProcessHelper.WriteMetadata(m_writer, m_project, false);
			WriteMetadataOptions();
			WriteMetadataFormatting();
			WriteMeatadataSortInformation();
			WriteMetadataDetails();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadataOptions()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "ul", "class", "options");

			if (m_outputFormat == OutputFormat.WordXml)
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "format", "Word 2003 XML");

				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "fileName", m_outputFileName);
			}
			else if (m_outputFormat == OutputFormat.XHTML)
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "format", "XHTML");

				var prjPath = ProcessHelper.TerminateFolderPath(m_project.ProjectPath);
				var usrPath = ProcessHelper.TerminateFolderPath(App.DefaultProjectFolder);
				var uri = new Uri(prjPath);
				var relativePath = uri.MakeRelativeUri(new Uri(usrPath)).ToString();
				relativePath = relativePath.Replace("%20", " ");

				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
					"genericRelativePath", relativePath);

				// TODO: Correct this when I know what to do.
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class",
					"specificRelativePath");
				m_writer.WriteEndElement();

				var cssFileName = (m_project.Name).Replace(' ', '_') + ".css";
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
					"specificStylesheetFile", cssFileName);
			}

			// Close ul
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadataFormatting()
		{
			var fieldInfo = GetFormattingFieldInfo();
			if (fieldInfo == null)
				return;

			// Open table, tbody and tr
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "table", "class", "formatting");
			m_writer.WriteStartElement("tbody");

			foreach (var kvp in fieldInfo)
			{
				if (kvp.Key != null && kvp.Value != null)
					ProcessHelper.WriteFieldFormattingInfo(m_writer, kvp.Key, kvp.Value);
			}

			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMeatadataSortInformation()
		{
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadataDetails()
		{
			// Open ul
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "ul", "class", "details");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class", "view", View);

			if (FilterHelper.CurrentFilter != null)
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "filter", FilterHelper.CurrentFilter.Name);
			}
			
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class", "title", Title);
			
			if (!string.IsNullOrEmpty(SearchPattern))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
					"searchPattern", SearchPattern);
			}
			
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"numberOfRecords", ((PaWordListGrid)m_grid).Cache.Count.ToString());

			if (m_isGridGrouped && ((PaWordListGrid)m_grid).GroupCount > 0)
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
					"numberOfGroups", ((PaWordListGrid)m_grid).GroupCount.ToString());
			}

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"projectName", m_project.Name);

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"languageName", m_project.LanguageName);

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"languageCode", m_project.LanguageCode);

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"date", DateTime.Today.ToShortDateString());

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"time", DateTime.Now.ToShortTimeString());

			// Close ul
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTable()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "table", "class", TableClass);
			WriteTableHeadingColumnGroups();
			WriteTableHeading();
			WriteTableBody();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableHeadingColumnGroups()
		{
			for (int i = 0; i < GetGridColumns().Count(); i++)
				ProcessHelper.WriteColumnGroup(m_writer, 1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableHeading()
		{
			m_writer.WriteStartElement("thead");
			m_writer.WriteStartElement("tr");

			WriteTableHeadingContent();

			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableHeadingContent()
		{
			foreach (var col in GetGridColumns())
				WriteTableHeadingContentForColumn(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableHeadingContentForColumn(DataGridViewColumn col)
		{
			m_writer.WriteStartElement("th");

			if (col is SilHierarchicalGridColumn)
			{
				m_writer.WriteAttributeString("class", "group");
				m_writer.WriteAttributeString("scope", "colgroup");
			}
			else
			{
				m_writer.WriteAttributeString("scope", "colgroup");
				m_writer.WriteString(col.HeaderText);
			}

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableBody()
		{
			if (!m_isGridGrouped)
				m_writer.WriteStartElement("tbody");
		
			WriteTableBodyContent();

			if (!m_isGridGrouped)
				m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableBodyContent()
		{
			foreach (var row in GetGridRows())
			{
				if (m_isGridGrouped && row is SilHierarchicalGridRow)
				{
					// Close previous group when not on first row.
					if (row.Index > 0)
						m_writer.WriteEndElement();

					ProcessHelper.WriteStartElementWithAttrib(m_writer, "tbody", "class", "group");
					WriteTableGroupHeading(row as SilHierarchicalGridRow);
				}
				else
					WriteTableRow(row);
			}

			// Close last group
			if (m_isGridGrouped)
				m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableGroupHeading(SilHierarchicalGridRow row)
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "tr", "class", "heading");
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "th", "class",
				"count", row.ChildCount.ToString());

			WriteLeftColSpan();
			WriteTableGroupHeadingGroupField(row);
			WriteRightColSpan();

			// Close tr
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableGroupHeadingGroupField(SilHierarchicalGridRow row)
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "class", m_groupedFieldName);
			m_writer.WriteAttributeString("scope", "colgroup");
			if (!string.IsNullOrEmpty(row.Text))
				m_writer.WriteString(row.Text);
			
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteLeftColSpan()
		{
			if (m_leftColSpanForGroupedList > 0)
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "colspan",
					m_leftColSpanForGroupedList.ToString());

				m_writer.WriteEndElement();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteRightColSpan()
		{
			if (m_rightColSpanForGroupedList > 0)
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "colspan",
					m_rightColSpanForGroupedList.ToString());

				m_writer.WriteEndElement();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableRow(DataGridViewRow row)
		{
			m_writer.WriteStartElement("tr");

			if (m_isGridGrouped)
				m_writer.WriteAttributeString("class", "data");

			WriteTableRowContent(row);
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableRowContent(DataGridViewRow row)
		{
			foreach (var col in GetGridColumns())
				WriteTableRowCell(row, col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableRowCell(DataGridViewRow row, DataGridViewColumn col)
		{
			m_writer.WriteStartElement("td");

			if (!(col is SilHierarchicalGridColumn))
			{
				m_writer.WriteAttributeString("class", ProcessHelper.MakeAlphaNumeric(col.HeaderText));
				var value = row.Cells[col.Index].Value;
				if (value != null && value.ToString() != string.Empty)
					m_writer.WriteString(value.ToString());
			}

			m_writer.WriteEndElement();
		}
	}
}
