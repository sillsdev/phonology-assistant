using System.Windows.Forms;
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ToHtml(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.XHTML, grid, openAfterExport,
				Pipeline.ProcessType.ExportToXHTML, Settings.Default.AppThatOpensHtml);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ToWordXml(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.WordXml, grid, openAfterExport,
				Pipeline.ProcessType.ExportToWord, Settings.Default.AppThatOpensWordXml);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool Process(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView grid, bool openAfterExport,
			Pipeline.ProcessType finalPipeline, string appToOpenOutput)
		{
			var exporter = new DataCorpusExporter(project, outputFileName, outputFormat, grid);

			var result = exporter.InternalProcess(Settings.Default.KeepTempDataCorpusExportFile,
				Pipeline.ProcessType.ExportDataCorpus, finalPipeline);

			if (result && openAfterExport)
				CallAppToOpenWordXML(appToOpenOutput, outputFileName);

			return result;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected DataCorpusExporter(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView grid)
			: base(project, outputFileName, outputFormat, grid)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string Title
		{
			get { return "Data Corpus"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string View
		{
			get { return "Data Corpus"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string TableClass
		{
			get { return "list"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string NumberOfRecords
		{
			get { return ((PaWordListGrid)m_grid).Cache.Count.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteMeatadataPhoneticSortOptions(PaWordListGrid grid)
		{
			if (grid.SortOptions.SortType != Model.PhoneticSortType.Unicode)
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class", "phoneticSortOption");
				m_writer.WriteString(grid.SortOptions.SortType == Model.PhoneticSortType.MOA ?
					"mannerOfArticulation" : "placeOfArticulation");
				m_writer.WriteEndElement();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteFieldSortOrder(PaWordListGrid grid)
		{
			if (grid.SortOptions.SortInformationList == null ||
				grid.SortOptions.SortInformationList.Count == 0)
			{
				return;
			}

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class", "fieldOrder");
			m_writer.WriteStartElement("ol");

			foreach (var sortInfo in grid.SortOptions.SortInformationList)
			{
				var field = sortInfo.FieldInfo.DisplayText;
				
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "li", "class",
					ProcessHelper.MakeAlphaNumeric(field));
				
				m_writer.WriteAttributeString("title", field);
				m_writer.WriteString(sortInfo.ascending ? "ascending" : "descending");
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
		protected override string GetTableRowCellValue(DataGridViewRow row, DataGridViewColumn col)
		{
			var grid = ((PaWordListGrid)m_grid);

			return (col == grid.PhoneticColumn ?
				grid.Cache[row.Index].WordCacheEntry.PhoneticValueWithPrimaryUncertainty :
				grid[col.Index, row.Index].Value as string);
		}
	}
}
