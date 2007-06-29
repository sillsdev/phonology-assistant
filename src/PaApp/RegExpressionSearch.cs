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
				m_regExAfter = new Regex(regEx[2]);
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

			int index = 0;
			int length;

			foreach (WordCacheEntry wordEntry in PaApp.WordCache)
			{
				index = 0;
				string phonetic = wordEntry.PhoneticValue;

				while (index < phonetic.Length)
				{
					Match match = m_regExItem.Match(phonetic, index);
					if (!match.Success)
						break;

					index = match.Index;
					length = match.Length;

					// Search for the environment before.
					match = m_regExBefore.Match(phonetic);
					if (!match.Success || match.Index + match.Length != index)
						break;

					// Search for the environment after.
					match = m_regExAfter.Match(phonetic, index + length);
					if (!match.Success || match.Index != index + length)
						break;

					// Add result cache entry.
					index++;
				}
			}

			return null;
		}
	}
}
