using System.Windows.Forms;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a vowel or consonant chart to an XML format that
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
		public static bool Process(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			var exporter = new DataCorpusExporter(project, outputFileName, grid, openAfterExport);
			return exporter.InternalProcess(Settings.Default.KeepIntermediateDataCorpusExportFile,
				Pipeline.ProcessType.ExportDataCorpus, Pipeline.ProcessType.ExportToXHTML);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected DataCorpusExporter(PaProject project, string outputFileName, DataGridView grid,
			bool openAfterExport) : base(project, outputFileName, grid, openAfterExport)
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
	}
}
