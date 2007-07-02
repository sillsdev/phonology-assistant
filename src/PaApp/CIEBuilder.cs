using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	#region CIEBuilder class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CIEBuilder
	{
		private SortOptions m_sortOptions;
		private CIEOptions m_cieOptions;
		private WordListCache m_cache;
		private List<string> m_ignoredList;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs an object to find the list of minimal pairs within the specified cache.
		/// (This overload uses default CIE options and sort options.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEBuilder(WordListCache cache) : this(cache, new CIEOptions())
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs an object to find the list of minimal pairs within the specified cache.
		/// (This overload uses default sort options.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEBuilder(WordListCache cache, CIEOptions cieOptions) : this(cache, null, cieOptions)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs an object to find the list of minimal pairs within the specified cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEBuilder(WordListCache cache, SortOptions sortOptions, CIEOptions cieOptions)
		{
			if (cache == null || cache.Count <= 2)
				return;

			m_cache = cache;
			m_sortOptions = (sortOptions == null ? new SortOptions(true) : sortOptions);
			CIEOptions = cieOptions;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the cache that is searched for minimal pairs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache Cache
		{
			get { return m_cache; }
			set { m_cache = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the CIE options used when finding minimal pairs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptions CIEOptions
		{
			get { return m_cieOptions; }
			set
			{
				m_cieOptions = (value == null ? new CIEOptions() : value);
				m_ignoredList = m_cieOptions.SearchQuery.CompleteIgnoredList;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache FindMinimalPairs()
		{
			if (m_cache == null || !m_cache.IsForSearchResults)
				return null;

			foreach (WordListCacheEntry entry in m_cache)
			    entry.CIEGroupId = -1;

			Dictionary<string, List<WordListCacheEntry>> cieGroups =
				new Dictionary<string, List<WordListCacheEntry>>();

			foreach (WordListCacheEntry entry in m_cache)
			{
				string pattern = GetCIEPattern(entry);

				List<WordListCacheEntry> entryList;
				if (!cieGroups.TryGetValue(pattern, out entryList))
				{
					entryList = new List<WordListCacheEntry>();
					cieGroups[pattern] = entryList;
				}

				entryList.Add(entry);
			}

			// Create a new cache which is the subset containing minimal pair entries.
			int cieGroupId = 0;
			SortedList<int, string> cieGroupTexts = new SortedList<int, string>();
			WordListCache cieCache = new WordListCache();
			foreach (KeyValuePair<string, List<WordListCacheEntry>> grp in cieGroups)
			{
				if (grp.Value.Count < 2)
					continue;

				foreach (WordListCacheEntry entry in grp.Value)
				{
					entry.CIEGroupId = cieGroupId;
					cieCache.Add(entry);
				}

				cieGroupTexts[cieGroupId++] = grp.Key;
			}

			cieCache.IsCIEList = true;
			cieCache.CIEGroupTexts = cieGroupTexts;
			cieCache.IsForSearchResults = true;
			cieCache.Sort(m_sortOptions);
			return cieCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For the phonetic data in the specified entry, this method gets a pattern
		/// appropriate to the CIE options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetCIEPattern(WordListCacheEntry entry)
		{
			if (m_cieOptions.Type == CIEOptions.IdenticalType.After)
			{
				string env = RemoveIgnoredCharacters(entry.EnvironmentAfter);
				return ("*__" + (string.IsNullOrEmpty(env) ? "#" : env));
			}

			if (m_cieOptions.Type == CIEOptions.IdenticalType.Before)
			{
				string env = RemoveIgnoredCharacters(entry.EnvironmentBefore);
				return ((string.IsNullOrEmpty(env) ? "#" : env) + "__*");
			}

			string before = RemoveIgnoredCharacters(entry.EnvironmentBefore);
			string after = RemoveIgnoredCharacters(entry.EnvironmentAfter);

			return ((string.IsNullOrEmpty(before) ? "#" : before) + "__" +
				(string.IsNullOrEmpty(after) ? "#" : after));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes ignored characters from the specified environment string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string RemoveIgnoredCharacters(string environment)
		{
			if (string.IsNullOrEmpty(environment))
				return null;

			StringBuilder bldrEnv = new StringBuilder(environment);

			// Get rid of all explicitly ignored characters (as opposed to
			// getting rid of all characters that are considered diacritics).
			foreach (string ignoredChar in m_ignoredList)
				bldrEnv = bldrEnv.Replace(ignoredChar, string.Empty);

			if (m_cieOptions.SearchQuery.IgnoreDiacritics)
			{
				// Loop through the characters in the environment string,
				// getting rid of diacritics.
				for (int i = bldrEnv.Length - 1; i >= 0; i--)
				{
					char chr = bldrEnv[i];
					if (chr != DataUtils.kBottomTieBarC && chr != DataUtils.kTopTieBarC)
					{
						IPACharInfo info = DataUtils.IPACharCache[chr];
						if (info != null && !info.IsBaseChar)
							bldrEnv.Remove(i, 1);
					}
				}
			}

			return bldrEnv.ToString();
		}
	}

	#endregion

	#region CIEOptions class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CIEOptions
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public enum IdenticalType
		{
			After,
			Before,
			Both
		}

		private SearchQuery m_query = new SearchQuery();
		private IdenticalType m_identicalType = IdenticalType.Both;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptions Clone()
		{
			CIEOptions options = new CIEOptions();
			options.m_query = m_query.Clone();
			options.m_identicalType = m_identicalType;
			return options;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("IdenticalType")]
		public IdenticalType Type
		{
			get { return m_identicalType; }
			set { m_identicalType = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery
		{
			get { return m_query; }
			set { m_query = value; }
		}
	}

	#endregion
}
