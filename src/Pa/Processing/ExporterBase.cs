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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Localization;
using SIL.Pa.Filters;
using SIL.Pa.Model;
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

		protected BackgroundWorker m_worker;
		protected XmlWriter m_writer;
		protected DataGridView m_grid;
		protected int m_leftColSpanForGroupedList;
		protected int m_rightColSpanForGroupedList;
		protected string m_outputFileName;
		protected OutputFormat m_outputFormat;
		protected readonly bool m_openAfterExport;
		protected readonly string m_groupedFieldName;
		protected readonly DataGridViewColumn m_groupByColumn;
		protected readonly PaFieldInfo m_groupByField;
		protected readonly bool m_isGridGrouped;
		protected readonly PaProject m_project;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ExporterBase(PaProject project)
		{
			m_project = project;

			// Make sure the project has a style sheet file.
			if (!(this is ProjectCssBuilder))
				ProjectCssBuilder.Process(m_project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ExporterBase(PaProject project, string outputFileName, DataGridView dgrid,
			bool openAfterExport) : this(project)
		{
			m_outputFileName = outputFileName;
			m_grid = dgrid;
			m_openAfterExport = openAfterExport;
			m_outputFormat = OutputFormat.XHTML;

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

				m_groupByField = (grid.Cache.IsCIEList ? App.FieldInfo.PhoneticField :
					((PaWordListGrid)m_grid).GroupByField);

				m_groupedFieldName = ProcessHelper.MakeAlphaNumeric(m_groupByField.DisplayText);
			}
		}

		#region Processing methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool InternalProcess(bool keepIntermediateFile,
			params Pipeline.ProcessType[] processTypes)
		{
			Debug.Assert(processTypes.Length > 0);

			App.MsgMediator.SendMessage("BeforeExport", new object[] { this, processTypes });

			// Create a stream of xml data containing the phones in the project.
			var inputStream = CreateInputFileToTransformPipeline(keepIntermediateFile);

			var msg = LocalizationManager.LocalizeString("ExportProgressMsg", "Exporting (Step {0})...",
				"Message displayed when exporting lists and charts.", App.kLocalizationGroupInfoMsg,
				LocalizationCategory.GeneralMessage, LocalizationPriority.Medium);

			MemoryStream outputStream = null;
			int processingStep = 0;

			foreach (var prsType in processTypes)
			{
				var pipeline = ProcessHelper.CreatePipline(prsType);

				// REVIEW: Should we warn the user that this failed?
				if (pipeline == null)
					continue;

				App.InitializeProgressBar(string.Format(msg, ++processingStep),
					pipeline.ProcessingSteps.Count);
				
				// Kick off the processing and then save the results to a file.
				pipeline.BeforeStepProcessed += BeforePipelineStepProcessed;
				outputStream = pipeline.Transform(inputStream);
				inputStream = outputStream;
				pipeline.BeforeStepProcessed -= BeforePipelineStepProcessed;
			}

			ProcessHelper.WriteStreamToFile(outputStream, m_outputFileName, false);
	
			if (File.Exists(m_outputFileName) && m_openAfterExport)
				Process.Start(m_outputFileName);

			App.UninitializeProgressBar();

			App.MsgMediator.SendMessage("AfterExport", new object[] { this, processTypes });
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void BeforePipelineStepProcessed(Pipeline pipeline, Step step)
		{
			App.IncProgressBar();
		}

		#endregion

		#region Properties
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
		protected virtual string Name
		{
			get { return null; }
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
		protected virtual string CIEOption
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string IntermediateFileName
		{
			get { return Path.ChangeExtension(m_outputFileName, "tmp"); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string NumberOfRecords
		{
			get { throw new NotImplementedException(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string NumberOfGroups
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string NumberOfPhones
		{
			get { return null; }
		}

		#endregion

		#region Methods for getting the rows, columns and field info. for the grid
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
			return m_grid.Rows.Cast<DataGridViewRow>().Where(x => x.Index != m_grid.NewRowIndex);
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

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
				ProcessHelper.WriteStreamToFile(memStream, IntermediateFileName);

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
				WriteMetadataWordXMLOptions();
			else if (m_outputFormat == OutputFormat.XHTML)
				WriteMetadataXHTMLOptions();

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMetadataWordXMLOptions()
		{
			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
			"li", "class", "format", "Word 2003 XML");

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "fileName", m_outputFileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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

			if (View != Title && !string.IsNullOrEmpty(Title))
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", "title", Title);
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

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "date", DateTime.Today.ToShortDateString());

			ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
				"li", "class", "time", DateTime.Now.ToShortTimeString());

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
				var value = GetTableRowCellValue(row, col);
				if (!string.IsNullOrEmpty(value))
					m_writer.WriteString(value);
			}

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string GetTableRowCellValue(DataGridViewRow row, DataGridViewColumn col)
		{
			return row.Cells[col.Index].Value as string;
		}
	}
}
