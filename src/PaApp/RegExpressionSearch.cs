using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.Data;

namespace SIL.Pa
{
	public class RegExpressionSearch
	{
		private SearchQuery m_query;
		private Regex m_regExBefore;
		private Regex m_regExItem;
		private Regex m_regExAfter;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RegExpressionSearch(SearchQuery query)
		{
			System.Diagnostics.Debug.Assert(query != null);

			m_query = query;

			if (!m_query.IsPatternRegExpression)
				return;

			string[] regEx = query.Pattern.Split(new char[] { DataUtils.kOrc });
			if (regEx.Length == 3)
			{
				m_regExBefore = new Regex(regEx[0]);
				m_regExItem = new Regex(regEx[1]);
				m_regExAfter = new Regex(regEx[1] + regEx[2]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and loads a result cache for the specified search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache Search()
		{
			if (!m_query.IsPatternRegExpression || m_regExBefore == null ||
				m_regExItem == null || m_regExAfter == null)
			{
				return null;
			}

			WordListCache resultCache = new WordListCache();
			int offset;
			int length;

			foreach (WordCacheEntry wordEntry in PaApp.WordCache)
			{
				offset = 0;
				string phonetic = wordEntry.PhoneticValueWithPrimaryUncertainty;

				//if (phonetic == "inaweza")
				//    offset = 0;

				while (offset < phonetic.Length)
				{
					// Find the search item starting at offset.
					Match match = m_regExItem.Match(phonetic, offset);
					if (!match.Success)
						break;

					offset = match.Index;
					length = match.Length;

					// Search for the environment before, looking for a match that
					// butts up against the match on the search item.
					match = m_regExBefore.Match(phonetic);
					while (match.Success && match.Index + match.Length < offset)
					    match = m_regExBefore.Match(phonetic, match.Index + 1);

					if (match.Success && match.Index + match.Length == offset)
					{
						// Search for the environment after.
						match = m_regExAfter.Match(phonetic, offset);
						if (match.Success && match.Index == offset)
							resultCache.AddEntryFromRegExpSearch(wordEntry, null, offset, length, false);
					}
					
					offset++;
				}
			}

			resultCache.ExtendRegExpMatchesToPhoneBoundaries();
			resultCache.IsForSearchResults = true;
			resultCache.IsForRegExpSearchResults = true;
			return resultCache;
		}
	}
}
