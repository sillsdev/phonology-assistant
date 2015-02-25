// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides a way to export a vowel or consonant chart to an XML format that
	/// is transformed into an html file with an accompanying cascading style sheet.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchResultExporter : DataCorpusExporter
	{
		/// ------------------------------------------------------------------------------------
		public static new bool ToHtml(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.XHTML, grid, openAfterExport,
				Settings.Default.AppThatOpensHtml, Pipeline.ProcessType.ExportToXHTML);
		}

		/// ------------------------------------------------------------------------------------
		public static new bool ToWordXml(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.WordXml, grid, openAfterExport,
				Settings.Default.AppThatOpensWordXml, Pipeline.ProcessType.ExportToWord);
		}

		/// ------------------------------------------------------------------------------------
		public static new bool ToXLingPaper(PaProject project, string outputFileName,
			PaWordListGrid grid, bool openAfterExport)
		{
			return Process(project, outputFileName, OutputFormat.XHTML, grid, openAfterExport,
				Settings.Default.AppThatOpensXLingPaperXML,
				new[] { Pipeline.ProcessType.ExportToXLingPaper} );
		}

		/// ------------------------------------------------------------------------------------
		private static bool Process(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView grid, bool openAfterExport,
			string appToOpenOutput, Pipeline.ProcessType finalPipeline)
		{
			return Process(project, outputFileName, outputFormat, grid, openAfterExport,
				appToOpenOutput, Pipeline.ProcessType.ExportSearchResult, finalPipeline);
		}

		/// ------------------------------------------------------------------------------------
		private static bool Process(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView grid, bool openAfterExport,
			string appToOpenOutput, params Pipeline.ProcessType[] pipeline)
		{
			var exporter = new SearchResultExporter(project, outputFileName, outputFormat, grid);

			var result = exporter.InternalProcess(
				Settings.Default.KeepTempSearchResultExportFile, pipeline);

			if (result && openAfterExport)
				CallAppToExportedFile(appToOpenOutput, outputFileName);

			return result;
		}

		/// ------------------------------------------------------------------------------------
		private SearchResultExporter(PaProject project, string outputFileName,
			OutputFormat outputFormat, DataGridView dgrid)
			: base(project, outputFileName, outputFormat, dgrid)
		{
			if (!m_isGridGrouped)
				return;
			
			var grid = m_grid as PaWordListGrid;
			if (grid.GroupByColumn == grid.PhoneticColumn || grid.Cache.IsCIEList)
				return;

			int groupByColIndex = m_groupByColumn.DisplayIndex;
			int phoneticColIndex = grid.PhoneticColumn.DisplayIndex;

			// If the phonetic column is to the left of the group-by column, then add
			// 2 to the left column span to account for the preceding, item and following
			// pieces of the phonetic search result. Otherwise, add 2 to the column span
			// to the right of the group-by column.
			if (phoneticColIndex < groupByColIndex)
				m_leftColSpanForGroupedList += 2;
			else
				m_rightColSpanForGroupedList += 2;
		}

		/// ------------------------------------------------------------------------------------
		protected override string Title
		{
			get
			{
				var title = ((PaWordListGrid)m_grid).Cache.SearchQuery.Name;
				return (string.IsNullOrEmpty(title) ? "Search" : title);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string Name
		{
			get { return ((PaWordListGrid)m_grid).Cache.SearchQuery.Name; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string View
		{
			get { return "Search"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string SearchPattern
		{
			get { return (((PaWordListGrid)m_grid).Cache.SearchQuery.Pattern); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string CIEOption
		{
			get
			{
				return (((PaWordListGrid)m_grid).Cache.IsCIEList ?
					((PaWordListGrid)m_grid).CIEOptions.Type.ToString() : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetMetadataDetailNameTag()
		{
			return "pattern name";
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteMeatadataPhoneticSortOptions(PaWordListGrid grid)
		{
			base.WriteMeatadataPhoneticSortOptions(grid);

			if (!grid.SortOptions.AdvancedEnabled)
				return;

			ProcessHelper.WriteStartElementWithAttrib(m_writer,
				"li", "class", "phoneticSearchSubfieldOrder");

			m_writer.WriteStartElement("ol");

			var classes = new[] { "Phonetic preceding", "Phonetic item", "Phonetic following" };
			var items = new KeyValuePair<string, string>[3];
			var options = grid.SortOptions;
			
			for (int i = 0; i < 3; i++)
			{
				int order = options.AdvSortOrder[i];
				items[i] = new KeyValuePair<string,string>(classes[order],
					(options.AdvRlOptions[order] ? "rightToLeft" : "leftToRight"));
			}

			for (int i = 0; i < 3; i++)
			{
				ProcessHelper.WriteStartElementWithAttribAndValue(m_writer,
					"li", "class", items[i].Key, items[i].Value);
			}

			m_writer.WriteEndElement();
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTableHeadingColumnGroups()
		{
			foreach (var col in GetGridColumns())
			{
				ProcessHelper.WriteColumnGroup(m_writer,
					(col == ((PaWordListGrid)m_grid).PhoneticColumn ? 3 : 1));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTableGroupHeadingGroupField(SilHierarchicalGridRow row)
		{
			var grid = m_grid as PaWordListGrid;

			if (grid == null || !m_isGridGrouped || m_groupByField.Type != FieldType.Phonetic || row.Text == null)
			{
				base.WriteTableGroupHeadingGroupField(row);
				return;
			}

			var text = row.Text.Replace("__", "_");
			var pieces = text.Split('_');

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "class", "Phonetic preceding");
			m_writer.WriteAttributeString("scope", "col");
			m_writer.WriteString(pieces.Length >= 0 ? pieces[0] : string.Empty);
			m_writer.WriteEndElement();

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "class", "Phonetic item");
			m_writer.WriteAttributeString("scope", "col");
			m_writer.WriteString("_");
			m_writer.WriteEndElement();

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "class", "Phonetic following");
			m_writer.WriteAttributeString("scope", "col");
			m_writer.WriteString(pieces.Length >= 0 ? pieces[1] : string.Empty);
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTableHeadingContentForColumn(DataGridViewColumn col)
		{
			if (col != ((PaWordListGrid)m_grid).PhoneticColumn)
				base.WriteTableHeadingContentForColumn(col);
			else
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "th", "scope", "colgroup");
				m_writer.WriteAttributeString("colspan", "3");
				m_writer.WriteString(col.HeaderText);
				m_writer.WriteEndElement();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void WriteTableRowCell(DataGridViewRow row, DataGridViewColumn col)
		{
			var grid = m_grid as PaWordListGrid;

			if (grid == null || !grid.Cache.IsForSearchResults)
				return;
			
			if (col != grid.PhoneticColumn)
			{
				base.WriteTableRowCell(row, col);
				return;
			}

			var wlentry = grid.GetWordEntry(row.Index);

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "td", "class", "Phonetic preceding");
			if (!string.IsNullOrEmpty(wlentry.EnvironmentBefore))
				m_writer.WriteString(wlentry.EnvironmentBefore);

			m_writer.WriteEndElement();

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "td", "class", "Phonetic item");
			if (!string.IsNullOrEmpty(wlentry.SearchItem))
				m_writer.WriteString(wlentry.SearchItem);

			m_writer.WriteEndElement();

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "td", "class", "Phonetic following");
			if (!string.IsNullOrEmpty(wlentry.EnvironmentAfter))
				m_writer.WriteString(wlentry.EnvironmentAfter);

			m_writer.WriteEndElement();
		}
	}
}
