using System.Drawing;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Helper class to group and ungroup rows in a PaWordListGrid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordListGroupingBuilder
	{
		private readonly PaWordListGrid m_grid;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates (or refreshes) groups in the specified grid. (This method will also
		/// ungroup if the grid's GroupByField property is not null).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Group(PaWordListGrid grid)
		{
			if (grid != null)
			{
				WordListGroupingBuilder builder = new WordListGroupingBuilder(grid);
				grid.SuspendLayout();
				builder.InternalGroup();
				grid.ResumeLayout();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes groups from the specified grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UnGroup(PaWordListGrid grid)
		{
			if (grid != null)
			{
				WordListGroupingBuilder builder = new WordListGroupingBuilder(grid);
				grid.SuspendLayout();
				builder.InternalUnGroup();
				grid.ResumeLayout();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListGroupingBuilder(PaWordListGrid grid)
		{
			m_grid = grid;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove all groups from the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalUnGroup()
		{
			// Remove all existing group rows.
			for (int i = m_grid.RowCount - 1; i >= 0; i--)
			{
				if (m_grid.Rows[i] is PaCacheGridRow)
					((PaCacheGridRow)m_grid.Rows[i]).ParentRow = null;
				else if (m_grid.Rows[i] is SilHierarchicalGridRow)
				{
					SilHierarchicalGridRow row = m_grid.Rows[i] as SilHierarchicalGridRow;
					m_grid.Rows.RemoveAt(i);
					row.Dispose();
				}
			}

			// Remove hierarchical columns.
			for (int i = m_grid.ColumnCount - 1; i >= 0; i--)
			{
				if (m_grid.Columns[i] is SilHierarchicalGridColumn)
				{
					SilHierarchicalGridColumn col = m_grid.Columns[i] as SilHierarchicalGridColumn;
					m_grid.Columns.RemoveAt(i);
					col.Dispose();
				}
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InternalGroup()
		{
			// Before grouping, make sure previous groupings are removed.
			InternalUnGroup();

			if (m_grid.Cache == null || m_grid.Cache.Count < 2)
				return;

			if (m_grid.Cache.IsCIEList)
				GroupMinimalPairs();
			else if (m_grid.GroupByField != null)
				GroupOnField(m_grid.GroupByField);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups rows according to the grid's group on field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupOnField(PaFieldInfo fieldInfo)
		{
			if (fieldInfo == null)
				return;

			WordListCache cache = m_grid.Cache;
			Font fnt = FontHelper.MakeFont(fieldInfo.Font, FontStyle.Bold);
			string prevFldValue = cache[cache.Count - 1][fieldInfo];
			int lastChild = cache.Count - 1;

			for (int i = cache.Count - 1; i >= 0; i--)
			{
				if (prevFldValue != cache[i][fieldInfo])
				{
					m_grid.Rows.Insert(i + 1,
						new SilHierarchicalGridRow(m_grid, prevFldValue, fnt, i + 1, lastChild));
					((SilHierarchicalGridRow)m_grid.Rows[i + 1]).ExpandedStateChanged +=
						m_grid.GroupExpandedChangedHandler;

					prevFldValue = cache[i][fieldInfo];
					lastChild = i;
				}
			}

			// Insert the first group heading row and insert a hierarchical column for the
			// + and - glpyhs.
			m_grid.Rows.Insert(0, new SilHierarchicalGridRow(m_grid, prevFldValue, fnt, 0, lastChild));
			((SilHierarchicalGridRow)m_grid.Rows[0]).ExpandedStateChanged +=
				m_grid.GroupExpandedChangedHandler;
			
			m_grid.m_suspendSavingColumnChanges = true;
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
			m_grid.m_suspendSavingColumnChanges = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups the grid's rows according to the search items' environment(s).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupMinimalPairs()
		{
			WordListCache cache = m_grid.Cache;
			Font fnt = FontHelper.MakeFont(FontHelper.PhoneticFont, FontStyle.Bold);
			int prevGroup = cache[cache.Count - 1].CIEGroupId;
			int lastChild = cache.Count - 1;

			for (int i = cache.Count - 1; i >= 0; i--)
			{
				if (prevGroup != cache[i].CIEGroupId)
				{
					string cieGroupText = null;
					if (cache.CIEGroupTexts != null)
						 cache.CIEGroupTexts.TryGetValue(cache[i + 1].CIEGroupId, out cieGroupText);

					m_grid.Rows.Insert(i + 1,
						new SilHierarchicalGridRow(m_grid, cieGroupText, fnt, i + 1, lastChild));
					((SilHierarchicalGridRow)m_grid.Rows[i + 1]).ExpandedStateChanged +=
						m_grid.GroupExpandedChangedHandler;

					prevGroup = cache[i].CIEGroupId;
					lastChild = i;
				}
			}

			// Insert the first group heading row.
			m_grid.Rows.Insert(0, new SilHierarchicalGridRow(m_grid,
				CIEBuilder.GetCIEPattern(cache[0], m_grid.CIEOptions), fnt, 0, lastChild));
			((SilHierarchicalGridRow)m_grid.Rows[0]).ExpandedStateChanged +=
				m_grid.GroupExpandedChangedHandler;
			
			// Insert a hierarchical column for the + and - glpyhs.
			m_grid.m_suspendSavingColumnChanges = true;
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
			m_grid.m_suspendSavingColumnChanges = false;
		}
	}
}
