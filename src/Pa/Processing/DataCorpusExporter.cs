using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a data corpus list to an XML format that
	/// is transformed into an html file with an accompanying cascading style sheet.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DataCorpusExporter : ExporterBase
	{
		/// ------------------------------------------------------------------------------------
		public static bool ToHtml(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.XHTML, grid, openAfterExport,
				Settings.Default.AppThatOpensHtml, Pipeline.ProcessType.ExportToXHTML);
		}

		/// ------------------------------------------------------------------------------------
		public static bool ToWordXml(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.WordXml, grid, openAfterExport,
				Settings.Default.AppThatOpensWordXml, Pipeline.ProcessType.ExportToWord);
		}

		/// ------------------------------------------------------------------------------------
		public static bool ToXLingPaper(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.XHTML, grid,
				openAfterExport, Settings.Default.AppThatOpensXLingPaperXML,
				new[] { Pipeline.ProcessType.ExportToXLingPaper });
		}

		/// ------------------------------------------------------------------------------------
		private static bool Process(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView grid, bool openAfterExport,
			 string appToOpenOutput, Pipeline.ProcessType finalPipeline)
		{
			return Process(project, outputFileName, outputFormat, grid, openAfterExport,
				appToOpenOutput, Pipeline.ProcessType.ExportDataCorpus, finalPipeline);
		}

		/// ------------------------------------------------------------------------------------
		private static bool Process(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView grid, bool openAfterExport,
			string appToOpenOutput, params Pipeline.ProcessType[] pipeline)
		{
			var exporter = new DataCorpusExporter(project, outputFileName, outputFormat, grid);

			bool result =
				exporter.InternalProcess(Settings.Default.KeepTempDataCorpusExportFile, pipeline);

			if (result && openAfterExport)
				CallAppToExportedFile(appToOpenOutput, outputFileName);

			return result;
		}

		/// ------------------------------------------------------------------------------------
		protected DataCorpusExporter(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView grid)
			: base(project, outputFileName, outputFormat, grid)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override string Title
		{
			get { return "Data Corpus"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string View
		{
			get { return "Data Corpus"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string TableClass
		{
			get { return "list"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string NumberOfRecords
		{
			get { return ((PaWordListGrid)m_grid).Cache.Count.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string NumberOfGroups
		{
			get 
			{
				return (m_isGridGrouped && ((PaWordListGrid)m_grid).GroupCount > 0 ?
					((PaWordListGrid)m_grid).GroupCount.ToString() : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteMeatadataSortInformation()
		{
			var grid = m_grid as PaWordListGrid;
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "ul", "class", "sorting");
			WriteFieldSortOrder(grid);
			WriteMeatadataPhoneticSortOptions(grid);
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMeatadataPhoneticSortOptions(PaWordListGrid grid)
		{
			if (grid.SortOptions.SortType != PhoneticSortType.Unicode)
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class", "phoneticSortOption");
				m_writer.WriteString(grid.SortOptions.SortType == PhoneticSortType.MOA ?
					"manner_or_height" : "place_or_backness");
				m_writer.WriteEndElement();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void WriteFieldSortOrder(PaWordListGrid grid)
		{
			if (grid.SortOptions.SortFields == null ||
				grid.SortOptions.SortFields.Count == 0)
			{
				return;
			}

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class", "fieldOrder");
			m_writer.WriteStartElement("ol");

			foreach (var sortInfo in grid.SortOptions.SortFields)
			{
				var field = sortInfo.Field.DisplayName;
				
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class",
					ProcessHelper.MakeAlphaNumeric(field));
				
				m_writer.WriteAttributeString("title", field);
				m_writer.WriteString(sortInfo.Ascending ? "ascending" : "descending");
				m_writer.WriteEndElement();
			}

			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetTableRowCellValue(DataGridViewRow row, DataGridViewColumn col)
		{
			var grid = ((PaWordListGrid)m_grid);

			if (col != grid.PhoneticColumn)
				return grid[col.Index, row.Index].Value as string;

			int i = (row is PaCacheGridRow ? ((PaCacheGridRow)row).CacheEntryIndex : row.Index);
			return grid.Cache[i].WordCacheEntry.PhoneticValueWithPrimaryUncertainty;
		}
	}
}
