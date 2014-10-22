using System;
using System.Drawing;
using System.Linq;
using System.Text;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Helper class to group and ungroup rows in a PaWordListGrid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordListGroupingBuilder
	{
		private Font m_headingFont;
		private readonly PaWordListGrid m_grid;
		private readonly WordListCache m_cache;

		/// ------------------------------------------------------------------------------------
		public WordListGroupingBuilder(PaWordListGrid grid)
		{
			m_grid = grid;
			if (grid != null)
				m_cache = grid.Cache;
		}

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
				SilTools.Utils.SetWindowRedraw(grid, false, false);
				var builder = new WordListGroupingBuilder(grid);
				builder.InternalGroup();
				SilTools.Utils.SetWindowRedraw(grid, true, true);
				grid.Invalidate();

				if (grid.GroupByField != null)
					App.MsgMediator.SendMessage("AfterWordListGroupedByField", grid);
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
				SilTools.Utils.SetWindowRedraw(grid, false, false);
				WordListGroupingBuilder builder = new WordListGroupingBuilder(grid);
				builder.InternalUnGroup();
				SilTools.Utils.SetWindowRedraw(grid, true, true);
				App.MsgMediator.SendMessage("AfterWordListUnGroupedByField", grid);
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
					var row = m_grid.Rows[i] as SilHierarchicalGridRow;
					m_grid.Rows.RemoveAt(i);
					row.Dispose();
				}
			}

			// Remove hierarchical columns.
			for (int i = m_grid.ColumnCount - 1; i >= 0; i--)
			{
				if (m_grid.Columns[i] is SilHierarchicalGridColumn)
				{
					var col = m_grid.Columns[i] as SilHierarchicalGridColumn;
					m_grid.Columns.Remove(col);
					col.Dispose();
				}
			}

			if (m_grid.GroupHeadingFont != null)
			{
				m_grid.GroupHeadingFont.Dispose();
				m_grid.GroupHeadingFont = null;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		private void InternalGroup()
		{
			// Before grouping, make sure previous groupings are removed.
			InternalUnGroup();

			if (m_cache == null || m_cache.Count < 2)
				return;

			if (m_cache.IsCIEList)
			{
				m_headingFont = FontHelper.MakeFont(App.PhoneticFont, FontStyle.Bold);
				GroupMinimalPairs();
			}
			else if (m_grid.GroupByField != null)
			{
				m_headingFont = FontHelper.MakeFont(m_grid.GroupByField.Font, FontStyle.Bold);

				if (m_grid.GroupByField.Type != FieldType.Phonetic || !m_cache.IsForSearchResults)
					GroupOnField(m_grid.GroupByField.Name);
				else
				{
					// Should we group by the phonetic column's search item?
					if (m_grid.SortOptions.AdvSortOrder[1] == 0)
						GroupOnPhoneticSearchItemPartExactly(m_grid.GroupByField.Name, 1);
					else
					{
						bool matchBefore = (m_grid.SortOptions.AdvSortOrder[0] == 0);

						int numberPhonesToMatch = (matchBefore ?
							Settings.Default.WordListGroupingPhonesToMatchBefore :
							Settings.Default.WordListGroupingPhonesToMatchAfter);

						if (numberPhonesToMatch == 0)
						{
							// Match exactly on the environment before or after.
							GroupOnPhoneticSearchItemPartExactly(m_grid.GroupByField.Name,
								(matchBefore ? 0 : 2));
						}
						else
						{
							// Match on the environment before or after
							// up to the specified number of phones.
							GroupOnPhoneticEnvironment(m_grid.GroupByField.Name,
								matchBefore, numberPhonesToMatch);
						}
					}
				}
			}

			m_grid.GroupHeadingFont = m_headingFont;
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
					AddGroupHeadingRow(i + 1, lastChild, null, prevFldValue, m_headingFont);
					prevFldValue = m_cache[i][fieldName];
					lastChild = i;
				}
			}

			FinishGrouping(prevFldValue, lastChild);
		}

		#region methods for grouping on phonetic fields in search result grids
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups on one of the three parts of the phonetic field for search result grids.
		/// When determining groups, exact matches on the item are required for two entries
		/// to be included in the same group. The part argument is 0 for the environment
		/// before, 1 for the search item and 2 for the environment after.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupOnPhoneticSearchItemPartExactly(string fieldName, int part)
		{
			if (string.IsNullOrEmpty(fieldName))
				return;

			var fmtHeading = GetHeadingFormatForGroupingByPhonetic();
			int lastChild = m_cache.Count - 1;
			var prevFldValue = (part == 0 ? m_cache[lastChild].EnvironmentBefore :
				part == 1 ? m_cache[lastChild].SearchItem : m_cache[lastChild].EnvironmentAfter);

			for (int i = m_cache.Count - 1; i >= 0; i--)
			{
				var currFldValue = (part == 0 ? m_cache[i].EnvironmentBefore :
					part == 1 ? m_cache[i].SearchItem : m_cache[i].EnvironmentAfter);

				if (prevFldValue != currFldValue)
				{
					AddGroupHeadingRow(i + 1, lastChild, fmtHeading, prevFldValue, m_headingFont);
					lastChild = i;
					prevFldValue = (part == 0 ? m_cache[i].EnvironmentBefore :
						part == 1 ? m_cache[i].SearchItem : m_cache[i].EnvironmentAfter);
				}
			}

			FinishGrouping(fmtHeading, prevFldValue, lastChild);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups on the environment before or after for a search result grid when the number
		/// of phones to match is greater than zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GroupOnPhoneticEnvironment(string fieldName, bool matchBefore,
			int numberPhonesToMatch)
		{
			if (string.IsNullOrEmpty(fieldName))
				return;

			bool rightToLeft = (matchBefore ?
				m_grid.SortOptions.AdvRlOptions[0] :
				m_grid.SortOptions.AdvRlOptions[2]);

			string fmtHeading = GetHeadingFormatForGroupingByPhonetic();
			
			int lastChild = m_cache.Count - 1;
			var prevEntry = m_cache[lastChild];
			string heading;

			for (int i = m_cache.Count - 1; i >= 0; i--)
			{
				heading = CompareEnvironments(prevEntry, m_cache[i],
					matchBefore, rightToLeft, numberPhonesToMatch);
				
				if (heading != null)
				{
					AddGroupHeadingRow(i + 1, lastChild, fmtHeading, heading, m_headingFont);
					prevEntry = m_cache[i];
					lastChild = i;
				}
			}

			heading = GetHeadingTextFromEntry(m_cache[0], matchBefore,
				rightToLeft, numberPhonesToMatch);

			FinishGrouping(fmtHeading, heading, lastChild);
		}

		/// ------------------------------------------------------------------------------------
		private static string CompareEnvironments(WordListCacheEntry prevEntry,
			WordListCacheEntry currEntry, bool matchBefore, bool rightToLeft,
			int numberPhonesToMatch)
		{
			int x, y;
			int endP, endC;
			int inc = (rightToLeft ? -1 : 1);
			var phonesP = prevEntry.Phones;
			var phonesC = currEntry.Phones;

			if (matchBefore)
			{
				if (string.IsNullOrEmpty(prevEntry.EnvironmentBefore) &&
					string.IsNullOrEmpty(currEntry.EnvironmentBefore))
				{
					return null;
				}

				if (string.IsNullOrEmpty(prevEntry.EnvironmentBefore))
					return "#";

				x = (rightToLeft ? prevEntry.SearchItemOffset - 1 : 0);
				endP = (rightToLeft ? -1 : prevEntry.SearchItemOffset);
				y = (rightToLeft ? currEntry.SearchItemOffset - 1 : 0);
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

				x = (rightToLeft ? phonesP.Length - 1 :
					prevEntry.SearchItemOffset + prevEntry.SearchItemLength);

				endP = (rightToLeft ?
					prevEntry.SearchItemOffset + prevEntry.SearchItemLength - 1 :
					phonesP.Length);

				y = (rightToLeft ? phonesC.Length - 1 :
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
		/// Gets the format for the heading text when grouping by the phonetic column for
		/// search result views.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetHeadingFormatForGroupingByPhonetic()
		{
			var fmtHeading = "{0}";
			var pattern = m_cache.SearchQuery.Pattern;
			pattern = pattern.Replace("{", "{{");
			pattern = pattern.Replace("}", "}}");

			if (m_grid.SortOptions.AdvSortOrder[1] == 0)
			{
				int slash = pattern.IndexOf('/');
				fmtHeading = "{0}" + (slash >= 0 ? pattern.Substring(slash) : "/*_*");
			}
			else
			{
				var ptrnParts = pattern.Split(new[] {'/', '_' }, 3);
				if (ptrnParts.Length == 3)
				{
					fmtHeading = ptrnParts[0] + "/" + (m_grid.SortOptions.AdvSortOrder[0] == 0 ?
						"{0}_" + ptrnParts[2] : ptrnParts[1] + "_{0}");
				}
			}

			return fmtHeading;
		}

		/// ------------------------------------------------------------------------------------
		private static string GetHeadingTextFromEntry(WordListCacheEntry entry, bool matchBefore,
			bool rightToLeft, int numberPhonesToMatch)
		{
			int phonesToInclude = numberPhonesToMatch;
			int start;

			if (matchBefore)
			{
				if (string.IsNullOrEmpty(entry.EnvironmentBefore))
					return "#";

				// Figure out how many phones to include in the returned string and 
				// which one will be the first one from the phones collection.
				phonesToInclude = Math.Min(phonesToInclude, entry.SearchItemOffset);
				start = (rightToLeft ? entry.SearchItemOffset - phonesToInclude : 0);
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
			}

			var heading = new StringBuilder();

			for (int i = start; phonesToInclude > 0; i++, phonesToInclude--)
				heading.Append(entry.Phones[i]);

			return (rightToLeft ? "..." : string.Empty) + heading +
				(!rightToLeft ? "..." : string.Empty);
		}

		#endregion

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
				if (prevGroup == m_cache[i].CIEGroupId)
					continue;
				
				string cieGroupText = null;
				if (m_cache.CIEGroupTexts != null)
					m_cache.CIEGroupTexts.TryGetValue(m_cache[i + 1].CIEGroupId, out cieGroupText);

				AddGroupHeadingRow(i + 1, lastChild, null, cieGroupText, m_headingFont);
				prevGroup = m_cache[i].CIEGroupId;
				lastChild = i;
			}

			FinishGrouping(CIEBuilder.GetCIEPattern(m_cache[0], m_grid.CIEOptions), lastChild);
		}

		/// ------------------------------------------------------------------------------------
		private void FinishGrouping(string heading, int grpsLastChild)
		{
			FinishGrouping("{0}", heading, grpsLastChild);
		}

		/// ------------------------------------------------------------------------------------
		private void FinishGrouping(string fmtHeading, string heading, int grpsLastChild)
		{
			// Insert the first group heading row and insert a hierarchical column for the
			// + and - glpyhs.
			AddGroupHeadingRow(0, grpsLastChild, fmtHeading, heading, m_headingFont);
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());

			// If all the groups were not collapsed, force the row to be expanded.
			// Otherwise the expanded state for rows formly collapsed will be all
			// messed up.
			bool expandGroups = !m_grid.AllGroupsCollapsed;
			
			foreach (var shgrow in m_grid.Rows.OfType<SilHierarchicalGridRow>())
			{
				// For some reason, on a Windows Vista machine, this should be called
				// for each hierarchical row *after* all hierarchical rows are added
				// to a grid, rather than as the rows are being added. That means this
				// should not be called in the row's constructor or clone event. When
				// it is, sometimes (and it appears to be random) a hand-full of
				// hierarchical rows never get the grid's RowPostPaint event. This
				// fixes PA-584.
				shgrow.SubscribeToOwningGridEvents();
					
				if (expandGroups)
					shgrow.SetExpandedState(true, true, false);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void AddGroupHeadingRow(int insertIndex, int lastChildInGroup,
			string fmtHeading, string heading, Font fntHeading)
		{
			if (fmtHeading == null)
			{
				m_grid.Rows.Insert(insertIndex, new SilHierarchicalGridRow(m_grid,
					heading, fntHeading, insertIndex, lastChildInGroup));
			}
			else
			{
				m_grid.Rows.Insert(insertIndex, new SilHierarchicalGridRow(m_grid,
					string.Format(fmtHeading, heading), fntHeading, insertIndex,
					lastChildInGroup));
			}

			var shgrow = m_grid.Rows[insertIndex] as SilHierarchicalGridRow;
			shgrow.ExpandedStateChanged += m_grid.GroupExpandedChangedHandler;

			if (m_grid.AllGroupsCollapsed)
				shgrow.SetExpandedState(false, false);
		}
	}
}
