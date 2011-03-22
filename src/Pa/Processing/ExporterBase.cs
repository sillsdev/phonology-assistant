using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	public class ExporterBase
	{
		public enum OutputFormat
		{
			XHTML,
			WordXml,
		}

		protected BackgroundWorker m_worker;
		protected XmlWriter m_writer;
		protected DataGridView m_grid;
		protected bool m_showExportProgress = true;
		protected int m_leftColSpanForGroupedList;
		protected int m_rightColSpanForGroupedList;
		protected string m_outputFileName;
		protected OutputFormat m_outputFormat;
		protected readonly string m_groupedFieldName;
		protected readonly DataGridViewColumn m_groupByColumn;
		protected readonly PaField m_groupByField;
		protected readonly bool m_isGridGrouped;
		protected readonly PaProject m_project;

		/// ------------------------------------------------------------------------------------
		public static bool CallAppToExportedFile(string application, string outputFileName)
		{
			if (File.Exists(outputFileName))
			{
				try
				{
					Process.Start(application, string.Format("\"{0}\"", outputFileName));
					return true;
				}
				catch
				{
					try
					{
						Process.Start(outputFileName);
						return true;
					}
					catch (Exception e)
					{
						Utils.MsgBox(e.Message);
					}
				}
			}
				
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected ExporterBase(PaProject project)
		{
			m_project = project;

			// Make sure the project has a style sheet file.
			if (!(this is ProjectCssBuilder))
				ProjectCssBuilder.Process(m_project);
		}

		/// ------------------------------------------------------------------------------------
		protected ExporterBase(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView dgrid) : this(project)
		{
			m_outputFileName = outputFileName;
			m_outputFormat = outputFormat;
			m_grid = dgrid;

			var grid = m_grid as PaWordListGrid;
			if (grid == null)
				return;

			if (grid.Cache.IsCIEList)
				m_groupByColumn = grid.PhoneticColumn;
			else if (grid.IsGroupedByField)
				m_groupByColumn = grid.GroupByColumn;

			m_isGridGrouped = (grid.Columns[0] is SilHierarchicalGridColumn && m_groupByColumn != null);

			if (m_isGridGrouped)
			{
				int groupByColIndex = m_groupByColumn.DisplayIndex;

				// Subtract on at the end to account for the column containing the
				// expand/collapse glyph. That doesn't count in the col. span.
				m_leftColSpanForGroupedList = (from x in m_grid.Columns.Cast<DataGridViewColumn>()
											   where x.Visible && x.DisplayIndex < groupByColIndex
											   select x).Count() - 1;

				m_rightColSpanForGroupedList = (from x in m_grid.Columns.Cast<DataGridViewColumn>()
												where x.Visible && x.DisplayIndex > groupByColIndex
												select x).Count();

				var field = project.GetPhoneticField();
				m_groupByField = (grid.Cache.IsCIEList ? field : ((PaWordListGrid)m_grid).GroupByField);
				m_groupedFieldName = ProcessHelper.MakeAlphaNumeric(m_groupByField.DisplayName);
			}
		}

		#region Processing methods
		/// ------------------------------------------------------------------------------------
		protected virtual bool InternalProcess(bool keepIntermediateFile,
			params Pipeline.ProcessType[] processTypes)
		{
			Debug.Assert(processTypes.Length > 0);

			App.MsgMediator.SendMessage("BeforeExport", new object[] { this, processTypes });

			// Create a stream of xml data containing the phones in the project.
			var inputStream = CreateInputFileToTransformPipeline(keepIntermediateFile);

			var msg = App.LocalizeString("ExportProgressMsg", "Exporting (Step {0})...",
				"Message displayed when exporting lists and charts.");

			MemoryStream outputStream = null;
			int processingStep = 0;

			foreach (var pipeline in processTypes.Select(pt => ProcessHelper.CreatePipeline(pt)).Where(pl => pl != null))
			{
				if (m_showExportProgress)
				{
					App.InitializeProgressBar(string.Format(msg, ++processingStep), pipeline.ProcessingSteps.Count);
					pipeline.BeforeStepProcessed += BeforePipelineStepProcessed;
				}

				// Kick off the processing and then save the results to a file.
				outputStream = pipeline.Transform(inputStream);
				inputStream = outputStream;
				pipeline.BeforeStepProcessed -= BeforePipelineStepProcessed;
			}

			var result = ProcessHelper.WriteStreamToFile(outputStream,
				m_outputFileName, true, false);

			App.UninitializeProgressBar();

			App.MsgMediator.SendMessage("AfterExport", new object[] { this, processTypes });
			
			return result;
		}

		/// ------------------------------------------------------------------------------------
		void BeforePipelineStepProcessed(Pipeline pipeline, Step step)
		{
			App.IncProgressBar();
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		protected virtual string Title
		{
			get { return "unknown title"; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string Name
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string View
		{
			get { return "unknown view"; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string TableClass
		{
			get { return "unknown table class"; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string SearchPattern
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string CIEOption
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string IntermediateFileName
		{
			get
			{
				return ProcessHelper.MakeTempFilePath(m_project,
					Path.ChangeExtension(m_outputFileName, "tmp"));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string NumberOfRecords
		{
			get { throw new NotImplementedException("Must implement in derived classes"); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string NumberOfGroups
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string NumberOfPhones
		{
			get { return null; }
		}

		#endregion

		#region Methods for getting the rows, columns and field info. for the grid
		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<DataGridViewColumn> GetGridColumns()
		{
			return from x in m_grid.Columns.Cast<DataGridViewColumn>()
					orderby x.DisplayIndex
					where x.Visible
					select x;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<DataGridViewRow> GetGridRows()
		{
			return m_grid.Rows.Cast<DataGridViewRow>().Where(x => x.Index != m_grid.NewRowIndex);
		}
		
		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<KeyValuePair<string, Font>> GetFormattingFieldInfo()
		{
			return GetGridColumns().Select(col =>
				new KeyValuePair<string, Font>(col.HeaderText, col.DefaultCellStyle.Font));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected virtual MemoryStream CreateInputFileToTransformPipeline(bool writeStreamToDisk)
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

			if (writeStreamToDisk)
			{
				ProcessHelper.WriteStreamToFile(memStream, IntermediateFileName,
					Settings.Default.TidyUpTempExportFilesAfterSaving);
			}

			return memStream;
		}

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
		protected virtual void WriteBody()
		{
			m_writer.WriteStartElement("body");
			WriteMetadata();
			WriteTable();
			m_writer.WriteEndElement();
		}

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
		protected virtual void WriteMetadataOptions()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "ul", "class", "options");

			if (m_outputFormat == OutputFormat.WordXml)
				WriteMetadataWordXMLOptions();
			else if (m_outputFormat == OutputFormat.XHTML)
				WriteMetadataXHTMLOptions();

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadataWordXMLOptions()
		{
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
			"li", "class", "format", "Word 2003 XML");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "fileName", Path.GetFileName(m_outputFileName));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadataXHTMLOptions()
		{
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "format", "XHTML");

			var outPath = Path.GetDirectoryName(m_outputFileName);
			WriteRelativePath("genericRelativePath", outPath, App.DefaultProjectFolder);
			WriteRelativePath("specificRelativePath", outPath, Path.GetDirectoryName(m_project.CssFileName));

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class",
				"specificStylesheetFile", Path.GetFileName(m_project.CssFileName));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void WriteRelativePath(string elementName, string path1, string path2)
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class", elementName);

			path1 = ProcessHelper.TerminateFolderPath(path1);
			path2 = ProcessHelper.TerminateFolderPath(path2);
			var uri = new Uri(path1);
			var relativePath = uri.MakeRelativeUri(new Uri(path2)).ToString();
			relativePath = relativePath.Replace("%20", " ");
			if (relativePath != string.Empty)
				m_writer.WriteString(relativePath);

			m_writer.WriteEndElement();
		}

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
		protected virtual void WriteMeatadataSortInformation()
		{
		}
	
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadataDetails()
		{
			// Open ul
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "ul", "class", "details");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer, "li", "class", "view", View);

			if (m_project.CurrentFilter != null)
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "filter", m_project.CurrentFilter.Name);
			}

			if (!string.IsNullOrEmpty(Name))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "name", Name);
			}

			if (!string.IsNullOrEmpty(SearchPattern))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "searchPattern", SearchPattern);
			}

			if (!string.IsNullOrEmpty(NumberOfPhones))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "numberOfPhones", NumberOfPhones);
			}

			if (!string.IsNullOrEmpty(NumberOfRecords))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "numberOfRecords", NumberOfRecords);
			}

			if (!string.IsNullOrEmpty(NumberOfGroups))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "numberOfGroups", NumberOfGroups);
			}

			if (CIEOption != null)
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "minimalPairs", CIEOption);
			}

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "projectName", m_project.Name);

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "languageName", m_project.LanguageName);

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "languageCode", m_project.LanguageCode);

			if (!string.IsNullOrEmpty(m_project.Researcher))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "researcher", m_project.Researcher);
			}

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "date", DateTime.Today.ToString("yyyy-MM-dd"));

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "time", DateTime.Now.ToString("HH:mm"));

			// Close ul
			m_writer.WriteEndElement();
		}

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
		protected virtual void WriteTableHeadingColumnGroups()
		{
			for (int i = 0; i < GetGridColumns().Count(); i++)
				ProcessHelper.WriteColumnGroup(m_writer, 1);
		}

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
		protected virtual void WriteTableHeadingContent()
		{
			foreach (var col in GetGridColumns())
				WriteTableHeadingContentForColumn(col);
		}

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
		protected virtual void WriteTableBody()
		{
			if (!m_isGridGrouped)
				m_writer.WriteStartElement("tbody");
		
			WriteTableBodyContent();

			if (!m_isGridGrouped)
				m_writer.WriteEndElement();
		}

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
		protected virtual void WriteTableGroupHeadingGroupField(SilHierarchicalGridRow row)
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "class", m_groupedFieldName);
			m_writer.WriteAttributeString("scope", "colgroup");
			if (!string.IsNullOrEmpty(row.Text))
				m_writer.WriteString(row.Text);
			
			m_writer.WriteEndElement();
		}

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
		protected virtual void WriteTableRow(DataGridViewRow row)
		{
			m_writer.WriteStartElement("tr");

			if (m_isGridGrouped)
				m_writer.WriteAttributeString("class", "data");

			WriteTableRowContent(row);
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableRowContent(DataGridViewRow row)
		{
			foreach (var col in GetGridColumns())
				WriteTableRowCell(row, col);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void WriteTableRowCell(DataGridViewRow row, DataGridViewColumn col)
		{
			m_writer.WriteStartElement("td");

			if (!(col is SilHierarchicalGridColumn))
			{
				m_writer.WriteAttributeString("class", ProcessHelper.MakeAlphaNumeric(col.HeaderText));
				var value = GetTableRowCellValue(row, col);
				if (!string.IsNullOrEmpty(value))
					m_writer.WriteString(value);
			}

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetTableRowCellValue(DataGridViewRow row, DataGridViewColumn col)
		{
			return row.Cells[col.Index].Value as string;
		}
	}
}
