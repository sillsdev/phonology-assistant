using System;
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
		private static int s_numberOfBeforePhonesToMatch = -1;
		private static int s_numberOfAfterPhonesToMatch = -1;

		private readonly PaWordListGrid m_grid;
		private Font m_headingFont;
		WordListCache m_cache;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListGroupingBuilder(PaWordListGrid grid)
		{
			m_grid = grid;
			if (grid != null)
				m_cache = grid.Cache;
		}

		#region static properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of phones in the environment before to match when grouping on the phonetic
		/// column in search result word lists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int NumberOfBeforePhonesToMatch
		{
			get
			{
				if (s_numberOfBeforePhonesToMatch < 0)
				{
					s_numberOfBeforePhonesToMatch = PaApp.SettingsHandler.GetIntSettingsValue(
						"phonestomatchforgrouping", "before", 1); 
				}
				
				return s_numberOfBeforePhonesToMatch;
			}
			set {s_numberOfBeforePhonesToMatch = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of phones in the environment after to match when grouping on the phonetic
		/// column in search result word lists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int NumberOfAfterPhonesToMatch
		{
			get
			{
				if (s_numberOfAfterPhonesToMatch < 0)
				{
					s_numberOfAfterPhonesToMatch = PaApp.SettingsHandler.GetIntSettingsValue(
						"phonestomatchforgrouping", "after", 1); 
				}
				
				return s_numberOfAfterPhonesToMatch;
			}
			set {s_numberOfAfterPhonesToMatch = value;}
		}

		#endregion 

		#region static methods
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
				PaApp.MsgMediator.SendMessage("AfterWordListGrouped", grid);
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
				PaApp.MsgMediator.SendMessage("AfterWordListUnGrouped", grid);
			}
		}

		#endregion

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

			if (m_cache == null || m_cache.Count < 2)
				return;

			if (m_cache.IsCIEList)
			{
				m_headingFont = FontHelper.MakeFont(FontHelper.PhoneticFont, FontStyle.Bold);
				GroupMinimalPairs();
			}
			else if (m_grid.GroupByField != null)
			{
				m_headingFont = FontHelper.MakeFont(m_grid.GroupByField.Font, FontStyle.Bold);

				if (!m_grid.GroupByField.IsPhonetic || !m_cache.IsForSearchResults)
					GroupOnField(m_grid.GroupByField.FieldName);
				else
				{
					// Should we group by the phonetic column's search item?
					if (m_grid.SortOptions.AdvSortOrder[1] == 0)
						GroupOnPhoneticSearchItem(m_grid.GroupByField.FieldName);
					else
						GroupOnPhoneticEnvironment(m_grid.GroupByField.FieldName);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups rows according to the grid's group on field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupOnField(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
				return;

			int lastChild = m_cache.Count - 1;
			string prevFldValue = m_cache[lastChild][fieldName];

			for (int i = m_cache.Count - 1; i >= 0; i--)
			{
				if (prevFldValue != m_cache[i][fieldName])
				{
					m_grid.Rows.Insert(i + 1, new SilHierarchicalGridRow(m_grid,
						prevFldValue, m_headingFont, i + 1, lastChild));
					
					((SilHierarchicalGridRow)m_grid.Rows[i + 1]).ExpandedStateChanged +=
						m_grid.GroupExpandedChangedHandler;

					prevFldValue = m_cache[i][fieldName];
					lastChild = i;
				}
			}

			// Insert the first group heading row and insert a hierarchical column for the
			// + and - glpyhs.
			m_grid.Rows.Insert(0, new SilHierarchicalGridRow(m_grid,
				prevFldValue, m_headingFont, 0, lastChild));
			
			((SilHierarchicalGridRow)m_grid.Rows[0]).ExpandedStateChanged +=
				m_grid.GroupExpandedChangedHandler;
			
			m_grid.m_suspendSavingColumnChanges = true;
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
			m_grid.m_suspendSavingColumnChanges = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupOnPhoneticSearchItem(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
				return;

			int slash = m_cache.SearchQuery.Pattern.IndexOf('/');
			string fmtHeading = "{0}" +
				(slash >= 0 ? m_cache.SearchQuery.Pattern.Substring(slash) : "/*_*");

			int lastChild = m_cache.Count - 1;
			string prevFldValue = m_cache[lastChild].SearchItem;

			for (int i = m_cache.Count - 1; i >= 0; i--)
			{
				if (prevFldValue != m_cache[i].SearchItem)
				{
					m_grid.Rows.Insert(i + 1, new SilHierarchicalGridRow(m_grid,
						string.Format(fmtHeading, prevFldValue), m_headingFont, i + 1, lastChild));
		
					((SilHierarchicalGridRow)m_grid.Rows[i + 1]).ExpandedStateChanged +=
						m_grid.GroupExpandedChangedHandler;

					prevFldValue = m_cache[i].SearchItem;
					lastChild = i;
				}
			}

			// Insert the first group heading row and insert a hierarchical column for the
			// + and - glpyhs.
			m_grid.Rows.Insert(0, new SilHierarchicalGridRow(m_grid,
				string.Format(fmtHeading, prevFldValue), m_headingFont, 0, lastChild));
			
			((SilHierarchicalGridRow)m_grid.Rows[0]).ExpandedStateChanged +=
				m_grid.GroupExpandedChangedHandler;

			m_grid.m_suspendSavingColumnChanges = true;
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
			m_grid.m_suspendSavingColumnChanges = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupOnPhoneticEnvironment(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
				return;

			bool matchBefore = (m_grid.SortOptions.AdvSortOrder[0] == 0);
			bool rightToLeft = (matchBefore ?
				m_grid.SortOptions.AdvRlOptions[0] :
				m_grid.SortOptions.AdvRlOptions[2]);

			int numberPhonesToMatch = (matchBefore ?
				NumberOfBeforePhonesToMatch : NumberOfAfterPhonesToMatch);
			
			if (numberPhonesToMatch <= 0)
				numberPhonesToMatch = int.MaxValue;

			string fmtHeading = "{0}";
			string pattern = m_cache.SearchQuery.Pattern;
			pattern = pattern.Replace("{", "{{");
			pattern = pattern.Replace("}", "}}");
			string[] ptrnParts = pattern.Split("/_".ToCharArray(), 3);
			if (ptrnParts.Length == 3)
			{
				fmtHeading = ptrnParts[0] + "/" +
					(matchBefore ? "{0}_" + ptrnParts[2] : ptrnParts[1] + "_{0}");
			}
			
			int lastChild = m_cache.Count - 1;
			WordListCacheEntry prevEntry = m_cache[lastChild];
			string heading;

			for (int i = m_cache.Count - 1; i >= 0; i--)
			{
				heading = CompareEnvironments(prevEntry, m_cache[i],
					matchBefore, rightToLeft, numberPhonesToMatch);
				
				if (heading != null)
				{
					m_grid.Rows.Insert(i + 1, new SilHierarchicalGridRow(m_grid,
						string.Format(fmtHeading, heading), m_headingFont, i + 1, lastChild));

					((SilHierarchicalGridRow)m_grid.Rows[i + 1]).ExpandedStateChanged +=
						m_grid.GroupExpandedChangedHandler;

					prevEntry = m_cache[i];
					lastChild = i;
				}
			}

			heading = GetHeadingTextFromEntry(m_cache[0], matchBefore,
				rightToLeft, numberPhonesToMatch);

			// Insert the first group heading row and insert a hierarchical column for the
			// + and - glpyhs.
			m_grid.Rows.Insert(0, new SilHierarchicalGridRow(m_grid,
				string.Format(fmtHeading, heading), m_headingFont, 0, lastChild));

			((SilHierarchicalGridRow)m_grid.Rows[0]).ExpandedStateChanged +=
				m_grid.GroupExpandedChangedHandler;

			m_grid.m_suspendSavingColumnChanges = true;
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
			m_grid.m_suspendSavingColumnChanges = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string CompareEnvironments(WordListCacheEntry prevEntry,
			WordListCacheEntry currEntry, bool matchBefore, bool rightToLeft,
			int numberPhonesToMatch)
		{
			int x, y, startP, startC;
			int endP, endC;
			int inc = (rightToLeft ? -1 : 1);
			string[] phonesP = prevEntry.Phones;
			string[] phonesC = currEntry.Phones;

			if (matchBefore)
			{
				if (string.IsNullOrEmpty(prevEntry.EnvironmentBefore) &&
					string.IsNullOrEmpty(currEntry.EnvironmentBefore))
				{
					return null;
				}

				if (string.IsNullOrEmpty(prevEntry.EnvironmentBefore))
					return "#";

				x = startP = (rightToLeft ? prevEntry.SearchItemOffset - 1 : 0);
				endP = (rightToLeft ? -1 : prevEntry.SearchItemOffset);
				y = startC = (rightToLeft ? currEntry.SearchItemOffset - 1 : 0);
				endC = (rightToLeft ? -1 : currEntry.SearchItemOffset);
			}
			else
			{
				if (string.IsNullOrEmpty(prevEntry.EnvironmentAfter) &&
					string.IsNullOrEmpty(currEntry.EnvironmentAfter))
				{
					return null;
				}

				if (string.IsNullOrEmpty(prevEntry.EnvironmentAfter))
					return "#";

				x = startP = (rightToLeft ? phonesP.Length - 1 :
					prevEntry.SearchItemOffset + prevEntry.SearchItemLength);

				endP = (rightToLeft ?
					prevEntry.SearchItemOffset + prevEntry.SearchItemLength - 1 :
					phonesP.Length);

				y = startC = (rightToLeft ? phonesC.Length - 1 :
					currEntry.SearchItemOffset + currEntry.SearchItemLength);

				endC = (rightToLeft ?
					currEntry.SearchItemOffset + currEntry.SearchItemLength - 1 :
					phonesC.Length);
			}

			bool match = false;
			int count = numberPhonesToMatch;
			while (x != endP && y != endC && count > 0)
			{
				if (phonesP[x] != phonesC[y])
				{
					match = false;
					break;
				}

				match = true;
				x += inc;
				y += inc;
				count--;
			}

			return (match ? null : GetHeadingTextFromEntry(prevEntry, matchBefore,
				rightToLeft, numberPhonesToMatch));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetHeadingTextFromEntry(WordListCacheEntry entry, bool matchBefore,
			bool rightToLeft, int numberPhonesToMatch)
		{
			int phonesToInclude = numberPhonesToMatch;
			int start = 0;
			bool includeEllipsisBefore = false;
			bool includeEllipsisAfter = false;

			if (matchBefore)
			{
				if (string.IsNullOrEmpty(entry.EnvironmentBefore))
					return "#";

				// Figure out how many phones to include in the returned string and 
				// which one will be the first one from the phones collection.
				phonesToInclude = Math.Min(phonesToInclude, entry.SearchItemOffset);
				start = (rightToLeft ? entry.SearchItemOffset - phonesToInclude : 0);
				includeEllipsisBefore = (rightToLeft && phonesToInclude < entry.SearchItemOffset);
				includeEllipsisAfter = (!rightToLeft && phonesToInclude < entry.SearchItemOffset); 
			}
			else
			{
				if (string.IsNullOrEmpty(entry.EnvironmentAfter))
					return "#";

				int phonesInEnvAfter = entry.Phones.Length -
					(entry.SearchItemOffset + entry.SearchItemLength);

				// Figure out how many phones to include in the returned string.
				phonesToInclude = Math.Min(phonesToInclude, phonesInEnvAfter);

				// Figure out which phone is the first phone in the returned string.
				start = (rightToLeft ? entry.Phones.Length - phonesToInclude :
					entry.SearchItemOffset + entry.SearchItemLength);

				includeEllipsisBefore = (rightToLeft && phonesToInclude < phonesInEnvAfter);
				includeEllipsisAfter = (!rightToLeft && phonesToInclude < phonesInEnvAfter); 
			}

			System.Text.StringBuilder heading = new System.Text.StringBuilder();

			for (int i = start; phonesToInclude > 0; i++, phonesToInclude--)
				heading.Append(entry.Phones[i]);

			return (includeEllipsisBefore ? "..." : string.Empty) + heading.ToString() +
				(includeEllipsisAfter ? "..." : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups the grid's rows according to the search items' environment(s).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupMinimalPairs()
		{
			int lastChild = m_cache.Count - 1;
			int prevGroup = m_cache[lastChild].CIEGroupId;

			for (int i = m_cache.Count - 1; i >= 0; i--)
			{
				if (prevGroup != m_cache[i].CIEGroupId)
				{
					string cieGroupText = null;
					if (m_cache.CIEGroupTexts != null)
						m_cache.CIEGroupTexts.TryGetValue(m_cache[i + 1].CIEGroupId, out cieGroupText);

					m_grid.Rows.Insert(i + 1, new SilHierarchicalGridRow(m_grid,
						cieGroupText, m_headingFont, i + 1, lastChild));
					
					((SilHierarchicalGridRow)m_grid.Rows[i + 1]).ExpandedStateChanged +=
						m_grid.GroupExpandedChangedHandler;

					prevGroup = m_cache[i].CIEGroupId;
					lastChild = i;
				}
			}

			// Insert the first group heading row.
			m_grid.Rows.Insert(0, new SilHierarchicalGridRow(m_grid,
				CIEBuilder.GetCIEPattern(m_cache[0], m_grid.CIEOptions), m_headingFont, 0, lastChild));
			
			((SilHierarchicalGridRow)m_grid.Rows[0]).ExpandedStateChanged +=
				m_grid.GroupExpandedChangedHandler;
			
			// Insert a hierarchical column for the + and - glpyhs.
			m_grid.m_suspendSavingColumnChanges = true;
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
			m_grid.m_suspendSavingColumnChanges = false;
		}
	}
}
