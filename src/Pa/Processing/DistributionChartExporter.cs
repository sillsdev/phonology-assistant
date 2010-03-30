using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilUtils;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a vowel or consonant chart to an XML format that
	/// is transformed into an html file with an accompanying cascading style sheet.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DistributionChartExporter
	{
		private const string kDefaultChartName = "Distribution Chart";

		private XmlWriter m_writer;
		private readonly string m_title;
		private readonly PaProject m_project;
		private readonly XYGrid m_distChartGrid;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Process(PaProject project, XYGrid distChartGrid)
		{
			var exporter = new DistributionChartExporter(project, distChartGrid);
			return exporter.InternalProcess();

			//HTMLXYChartWriter writer = new DistributionChartExporter(distChartGrid,
			//    defaultFileName,
			//    chartType, chartName);

			//return writer.HtmlOutputFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private DistributionChartExporter(PaProject project, XYGrid distChartGrid)
		{
			m_project = project;
			m_distChartGrid = distChartGrid;
			m_title = (string.IsNullOrEmpty(m_distChartGrid.ChartName) ?
				kDefaultChartName : m_distChartGrid.ChartName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool InternalProcess()
		{
			// Create a stream of xml data containing the phones in the project.
			var inputStream = CreateXHTML();

			if (Settings.Default.KeepIntermediateDistributionChartExportFile)
			{
				var intermediateFileName = OutputFileNameWOExt + ".IntermediateDistChart.xml";
				intermediateFileName = Path.Combine(m_project.ProjectPath, intermediateFileName);
				ProcessHelper.WriteStreamToFile(inputStream, intermediateFileName);
			}

			// Create a processing pipeline for a series of xslt transforms to be applied to the stream.
			var processFileName = Path.Combine(App.ProcessingFolder, "Processing.xml");
			var pipeline = Pipeline.Create("export", "Distribution Chart", processFileName,
				App.ProcessingFolder);

			// REVIEW: Should we warn the user that this failed?
			if (pipeline == null)
				return false;

			// Kick off the processing and then save the results to a file.
			var outputFileName = OutputFileNameWOExt + ".html";
			outputFileName = Path.Combine(m_project.ProjectPath, outputFileName);
			pipeline.Transform(inputStream, outputFileName);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string OutputFileNameWOExt
		{
			get
			{
				var path = (m_distChartGrid.ChartName ?? kDefaultChartName);
				return path.Replace(' ', '_');
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private MemoryStream CreateXHTML()
		{
			var memStream = new MemoryStream();

			using (m_writer = XmlWriter.Create(memStream))
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
		private void WriteHead()
		{
			m_writer.WriteStartElement("head");

			ProcessHelper.WriteElement(m_writer, "title", m_title);
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "meta", "http-equiv", "content-type");
			m_writer.WriteAttributeString("content", "text/html; charset=utf-8");

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
		private void WriteBody()
		{
			m_writer.WriteStartElement("body");
			WriteMetadata();			
			WriteTable();
			m_writer.WriteEndElement();
		}

		#region Methods for writing metadata
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteMetadata()
		{
			ProcessHelper.WriteMetadata(m_writer, m_project, false);
			WriteMetadataOptions();
			WriteMetadataFormatting();
			WriteMetadataDetails();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteMetadataOptions()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "div", "class", "options");
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "ul", "class", "format");
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class", "XHTML");

			m_writer.WriteStartElement("ul");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"genericRelativePath", "../../");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"specificRelativePath", string.Empty);

			var cssFileName = OutputFileNameWOExt + ".css";
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"specificStylesheetFile", cssFileName);

			// Close ul, li and ul elements
			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
			m_writer.WriteEndElement();

			// When the program displays Export To Whatever dialog boxes, view options will go here.

			// Close div
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteMetadataFormatting()
		{
			// Open table, tbody and tr
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "table", "class", "formatting");
			m_writer.WriteStartElement("tbody");
			m_writer.WriteStartElement("tr");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "td", "class",
				"name", "Phonetic");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "td", "class",
				"class", "Phonetic");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "td", "class",
				"font-family", FontHelper.PhoneticFont.Name);

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "td", "class",
				"font-size", FontHelper.PhoneticFont.SizeInPoints.ToString());

			if (!FontHelper.PhoneticFont.Bold)
				ProcessHelper.WriteEmptyElement(m_writer, "td");
			else
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "td", "class",
					"font-weight", "bold");
			}

			if (!FontHelper.PhoneticFont.Italic)
				ProcessHelper.WriteEmptyElement(m_writer, "td");
			else
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "td", "class",
					"font-style", "italic");
			}

			// Close tr, tbody and table elements
			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteMetadataDetails()
		{
			// Open ul
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "ul", "class", "details");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"view", "Distribution Chart");
			
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"title", m_title);
			
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

		#endregion

		#region Methods for writing the table
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteTable()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "table", "class", "distribution chart");
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
		private void WriteTableHeadingColumnGroups()
		{
			// Write group for far left column containing search item.
			m_writer.WriteStartElement("colgroup");
			ProcessHelper.WriteEmptyElement(m_writer, "col");
			m_writer.WriteEndElement();

			// Write group for each column after the search item column, but not including
			// the last column which is always empty to provide a space for the user to
			// add another search pattern column.
			m_writer.WriteStartElement("colgroup");

			for (int i = 1; i < m_distChartGrid.ColumnCount - 1; i++)
				ProcessHelper.WriteEmptyElement(m_writer, "col");

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteTableHeading()
		{
			m_writer.WriteStartElement("thead");
			m_writer.WriteStartElement("tr");

			ProcessHelper.WriteEmptyElement(m_writer, "th");

			for (int i = 1; i < m_distChartGrid.ColumnCount - 1; i++)
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "class", "Phonetic");
				ProcessHelper.WriteAttrib(m_writer, "scope", "col");
				m_writer.WriteString(m_distChartGrid[i, 0].Value as string);
				m_writer.WriteEndElement();
			}

			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteTableBody()
		{
			m_writer.WriteStartElement("tbody");

			for (int i = 1; i < m_distChartGrid.RowCount; i++)
			{
				if (i < m_distChartGrid.NewRowIndex)
					WriteTableRow(m_distChartGrid.Rows[i]);
			}

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteTableRow(DataGridViewRow row)
		{
			m_writer.WriteStartElement("tr");

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "class", "Phonetic");
			ProcessHelper.WriteAttrib(m_writer, "scope", "row");
			m_writer.WriteString(row.Cells[0].Value as string);
			m_writer.WriteEndElement();
	
			for (int i = 1; i < m_distChartGrid.ColumnCount - 1; i++)
			{
				if (row.Cells[i].Value is XYChartException)
					ProcessHelper.WriteStartElementWithAttribAndEmptyValue(m_writer, "td", "class", "error");
				else if (row.Cells[i].Value == null)
					ProcessHelper.WriteEmptyElement(m_writer, "td");
				else
				{
					if (row.Cells[i].Value.GetType() == typeof(int))
						ProcessHelper.WriteElement(m_writer, "td", ((int)row.Cells[i].Value).ToString());
					else if (row.Cells[i].Value is string)
						ProcessHelper.WriteElement(m_writer, "td", row.Cells[i].Value as string);
				}
			}

			m_writer.WriteEndElement();
		}

		#endregion
	}
}
