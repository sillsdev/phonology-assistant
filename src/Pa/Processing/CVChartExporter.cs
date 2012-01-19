using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a vowel or consonant charts to an XML format that
	/// is transformed into an html file with an accompanying cascading style sheet.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVChartExporter : ExporterBase
	{
		private readonly CVChartType m_chartType;

		/// ------------------------------------------------------------------------------------
		public static bool ToHtml(PaProject project, CVChartType chartType,
			string outputFileName, CVChartGrid grid, bool openAfterExport)
		{
			return ToHtml(project, chartType, outputFileName, grid, openAfterExport, true);
		}

		/// ------------------------------------------------------------------------------------
		public static bool ToHtml(PaProject project, CVChartType chartType,
			string outputFileName, CVChartGrid grid, bool openAfterExport,
			bool showExportProgress)
		{
			return Process(project, chartType, outputFileName, OutputFormat.XHTML, grid,
				openAfterExport, Settings.Default.AppThatOpensHtml, showExportProgress, 
				Pipeline.ProcessType.ExportToXHTML);
		}

		/// ------------------------------------------------------------------------------------
		public static bool ToWordXml(PaProject project, CVChartType chartType,
			string outputFileName, CVChartGrid grid, bool openAfterExport)
		{
			return Process(project, chartType, outputFileName, OutputFormat.WordXml, grid,
				openAfterExport, Settings.Default.AppThatOpensWordXml, true,
				Pipeline.ProcessType.ExportToWord);
		}

		/// ------------------------------------------------------------------------------------
		public static bool ToXLingPaper(PaProject project, CVChartType chartType,
			string outputFileName, CVChartGrid grid, bool openAfterExport)
		{
			return Process(project, chartType, outputFileName, OutputFormat.XHTML, grid,
				openAfterExport, Settings.Default.AppThatOpensXLingPaperXML, true,
				new[] { Pipeline.ProcessType.ExportToXLingPaper });
		}
		
		/// ------------------------------------------------------------------------------------
		private static bool Process(PaProject project, CVChartType chartType,
			string outputFileName, OutputFormat outputFormat, DataGridView grid,
			bool openAfterExport, string appToOpenOutput, bool showExportProgress,
			Pipeline.ProcessType finalPipeline)
			
		{
			return Process(project, chartType, outputFileName, outputFormat, grid,
				openAfterExport, appToOpenOutput, showExportProgress,
				Pipeline.ProcessType.ExportCVChart, finalPipeline);
		}

		/// ------------------------------------------------------------------------------------
		private static bool Process(PaProject project, CVChartType chartType, 
			string outputFileName, OutputFormat outputFormat, DataGridView grid,
			bool openAfterExport, string appToOpenOutput, bool showExportProgress,
			params Pipeline.ProcessType[] pipeline)
		{
			var exporter = new CVChartExporter(project, chartType, outputFileName, outputFormat, grid);
			exporter.m_showExportProgress = showExportProgress;

			bool result = exporter.InternalProcess(Settings.Default.KeepTempCVChartExportFile, pipeline);
			
			if (result && openAfterExport)
				CallAppToExportedFile(appToOpenOutput, outputFileName);

			return result;
		}
	
		/// ------------------------------------------------------------------------------------
		protected CVChartExporter(PaProject project, CVChartType chartType,
			string outputFileName, OutputFormat outputFormat, DataGridView grid)
			: base(project, outputFileName, outputFormat, grid)
		{
			m_chartType = chartType;
		}

		/// ------------------------------------------------------------------------------------
		protected override string Title
		{
			get { return (m_chartType == CVChartType.Consonant ? "Consonants" : "Vowels"); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string View
		{
			get { return Title; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string TableClass
		{
			get { return "CV chart"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string NumberOfRecords
		{
			get { return m_project.WordCache.Count.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string NumberOfPhones
		{
			get 
			{
				int count = 0;

				for (int c = 0; c < m_grid.ColumnCount; c++)
					count += m_grid.Rows.Cast<DataGridViewRow>().Count(x => x.Cells[c].Value != null);
				
				return (count > 0 ? count.ToString() : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetMetadataDetailNumberPhonesTag()
		{
			return "number " + (m_chartType == CVChartType.Consonant ? "consonant" : "vowel");
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<KeyValuePair<string, Font>> GetFormattingFieldInfo()
		{
			yield return new KeyValuePair<string, Font>("Phonetic",
				m_project.GetPhoneticField().Font);
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTableHeadingColumnGroups()
		{
			ProcessHelper.WriteColumnGroup(m_writer, 1);

			foreach (var grp in ((CVChartGrid)m_grid).ColumnGroups)
				ProcessHelper.WriteColumnGroup(m_writer, 2);
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTableHeadingContent()
		{
			m_writer.WriteStartElement("th");
			m_writer.WriteEndElement();

			foreach (var grp in ((CVChartGrid)m_grid).ColumnGroups)
			{
				m_writer.WriteStartElement("th");
				m_writer.WriteAttributeString("scope", "colgroup");
				m_writer.WriteAttributeString("colspan", "2");
				m_writer.WriteString(grp.Text);
				m_writer.WriteEndElement();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTableBody()
		{
			foreach (var rwgrp in ((CVChartGrid)m_grid).RowGroups)
				WriteChartRowGroup(rwgrp);
		}

		/// ------------------------------------------------------------------------------------
		private void WriteChartRowGroup(CVChartRowGroup rwgrp)
		{
			m_writer.WriteStartElement("tbody");

			foreach (var row in rwgrp.Rows)
			{
				m_writer.WriteStartElement("tr");

				if (row == rwgrp.Rows[0])
				{
					ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "scope", "rowgroup");
					m_writer.WriteAttributeString("rowspan", rwgrp.Rows.Count.ToString());
					m_writer.WriteString(rwgrp.Text);
					m_writer.WriteEndElement();
				}
				
				WriteChartRowGroupRow(row);
				m_writer.WriteEndElement();
			}

			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		private void WriteChartRowGroupRow(DataGridViewRow row)
		{
			foreach (DataGridViewTextBoxCell cell in row.Cells)
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "td", "class", "Phonetic");
				var phone = cell.Value as string;
				if (!string.IsNullOrEmpty(phone))
					m_writer.WriteString(phone);

				m_writer.WriteEndElement();
			}
		}
	}
}
