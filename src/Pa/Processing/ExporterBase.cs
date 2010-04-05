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
		protected readonly string m_outputFileName;
		protected DataGridView m_grid;
		protected readonly PaProject m_project;
		protected readonly OutputFormat m_outputFormat;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExporterBase(PaProject project, string outputFileName, DataGridView grid)
		{
			m_project = project;
			m_outputFileName = outputFileName;
			m_grid = grid;
			m_outputFormat = OutputFormat.XHTML;
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
			m_writer.WriteElementString("title", Title);

			// Close meta
			m_writer.WriteEndElement();

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
				ProcessHelper.WriteFieldFormattingInfo(m_writer, kvp.Key, kvp.Value);

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
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"numberOfRecords", App.WordCache.Count.ToString());

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
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"th", "scope", "colgroup", col.HeaderText);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableBody()
		{
			m_writer.WriteStartElement("tbody");
			WriteTableBodyContent();
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
				WriteTableRow(row);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableRow(DataGridViewRow row)
		{
			m_writer.WriteStartElement("tr");
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
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "td", "class",
				ProcessHelper.MakeAlphaNumeric(col.HeaderText));

			var value = row.Cells[col.Index].Value;
			if (value != null && value.ToString() != string.Empty)
				m_writer.WriteString(value.ToString());

			m_writer.WriteEndElement();
		}
	}
}
