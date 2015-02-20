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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa
{
	#region CIEBuilder class
	/// ----------------------------------------------------------------------------------------
	public class CIEBuilder
	{
		private readonly SortOptions _sortOptions;
		private CIEOptions _cieOptions;
	    public static bool IsMinimalpair = false;
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs an object to find the list of minimal pairs within the specified cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEBuilder(WordListCache cache, SortOptions sortOptions, CIEOptions cieOptions)
		{
			if (cache == null || cache.Count <= 2)
				return;

			Cache = cache;
			CIEOptions = cieOptions;
			_sortOptions = sortOptions.Copy();
			_sortOptions.AdvSortOrder = (CIEOptions.Type == CIEOptions.IdenticalType.After ?
				new[] { 2, 0, 1 } : new[] { 1, 0, 2 });
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the cache that is searched for minimal pairs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache Cache { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the CIE options used when finding minimal pairs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptions CIEOptions
		{
			get { return _cieOptions; }
			set {_cieOptions = (value ?? new CIEOptions());}
		}

		/// ------------------------------------------------------------------------------------
		public WordListCache FindMinimalPairs()
		{
		    IsMinimalpair = true;
			if (Cache == null || !Cache.IsForSearchResults)
				return null;

			foreach (var entry in Cache)
			    entry.CIEGroupId = -1;

			// First, send a message to see if there is an AddOn to find minimal pairs. If so,
			// then return the cache it generated instead of the one built by this method.
			object args = this;
			if (App.MsgMediator.SendMessage("FindMinimalPairsAlternate", args))
			{
				if (args is WordListCache)
					return (args as WordListCache);
			}

			var cieGroups = new Dictionary<string, List<WordListCacheEntry>>();

			foreach (var entry in Cache)
			{
				string pattern = GetCIEPattern(entry, _cieOptions);

				List<WordListCacheEntry> entryList;
				if (!cieGroups.TryGetValue(pattern, out entryList))
				{
					entryList = new List<WordListCacheEntry>();
					cieGroups[pattern] = entryList;
				}

				entryList.Add(entry);
			}

			// The groups are not guaranteed to be in any particular order, just the words within groups.
			// TODO: Sort groups by POA, or MOA, based on what's specified in _sortOptions.

			// Create a new cache which is the subset containing minimal pair entries.
			int cieGroupId = 0;
			var cieGroupTexts = new SortedList<int, string>();
			var cieCache = new WordListCache();
			
			foreach (var grp in cieGroups.Where(g => g.Value.Count >= 2))
			{
				foreach (var entry in grp.Value)
				{
					entry.CIEGroupId = cieGroupId;
					cieCache.Add(entry);
				}

				cieGroupTexts[cieGroupId++] = grp.Key;
			}

			cieCache.IsCIEList = true;
			cieCache.CIEGroupTexts = cieGroupTexts;
			cieCache.IsForSearchResults = true;
			cieCache.Sort(_sortOptions);
			cieCache.SearchQuery = Cache.SearchQuery.Clone();
			return cieCache;
		}

        /// ------------------------------------------------------------------------------------
        public WordListCache FindSimilarPairs()
        {
            IsMinimalpair = false;
            if (Cache == null || !Cache.IsForSearchResults)
                return null;

            foreach (var entry in Cache)
                entry.CIEGroupId = -1;

            // First, send a message to see if there is an AddOn to find minimal pairs. If so,
            // then return the cache it generated instead of the one built by this method.
            object args = this;
            if (App.MsgMediator.SendMessage("FindSimilarPairsAlternate", args))
            {
                if (args is WordListCache)
                    return (args as WordListCache);
            }

            var cieGroups = new Dictionary<string, List<WordListCacheEntry>>();

            foreach (var entry in Cache)
            {
                string pattern = GetCIESimilarPattern(entry, _cieOptions);

                List<WordListCacheEntry> entryList;
                if (!cieGroups.TryGetValue(pattern, out entryList))
                {
                    entryList = new List<WordListCacheEntry>();
                    cieGroups[pattern] = entryList;
                }
                entryList.Add(entry);
            }

            // The groups are not guaranteed to be in any particular order, just the words within groups.
            // TODO: Sort groups by POA, or MOA, based on what's specified in _sortOptions.

            // Create a new cache which is the subset containing minimal pair entries.
            int cieGroupId = 0;
            var cieGroupTexts = new SortedList<int, string>();
            var cieCache = new WordListCache();

            foreach (var grp in cieGroups.Where(g => g.Value.Count >= 2))
            {
                foreach (var entry in grp.Value)
                {
                    entry.CIEGroupId = cieGroupId;
                    cieCache.Add(entry);
                }

                cieGroupTexts[cieGroupId++] = grp.Key;
            }
            cieCache.IsCIEList = true;
            cieCache.CIEGroupTexts = cieGroupTexts;
            cieCache.IsForSearchResults = true;
            cieCache.Sort(_sortOptions);
            cieCache.SearchQuery = Cache.SearchQuery.Clone();
            return cieCache;
        }


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For the phonetic data in the specified entry, this method gets a pattern
		/// appropriate to the CIE options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetCIEPattern(WordListCacheEntry entry, CIEOptions cieOptions)
		{
			if (cieOptions.Type == CIEOptions.IdenticalType.After)
			{
				string env = RemoveIgnoredCharacters(entry.EnvironmentAfter, cieOptions);
				return ("*__" + (string.IsNullOrEmpty(env) ? "#" : env));
			}

			if (cieOptions.Type == CIEOptions.IdenticalType.Before)
			{
				string env = RemoveIgnoredCharacters(entry.EnvironmentBefore, cieOptions);
				return ((string.IsNullOrEmpty(env) ? "#" : env) + "__*");
			}

			string before = RemoveIgnoredCharacters(entry.EnvironmentBefore, cieOptions);
			string after = RemoveIgnoredCharacters(entry.EnvironmentAfter, cieOptions);

			return ((string.IsNullOrEmpty(before) ? "#" : before) + "__" +
				(string.IsNullOrEmpty(after) ? "#" : after));
		}
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// For the phonetic data in the specified entry, this method gets a pattern
        /// appropriate to the CIE options.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static string GetCIESimilarPattern(WordListCacheEntry entry, CIEOptions cieOptions)
        {
            if (cieOptions.Type == CIEOptions.IdenticalType.After)
            {
                string env = GetPatternvalue(RemoveIgnoredCharacters(entry.EnvironmentAfter, cieOptions), CIEOptions.IdenticalType.After);
                return ("*__" + (string.IsNullOrEmpty(env) ? "#" : env));
            }

            if (cieOptions.Type == CIEOptions.IdenticalType.Before)
            {
                string env = GetPatternvalue(RemoveIgnoredCharacters(entry.EnvironmentBefore, cieOptions), CIEOptions.IdenticalType.Before);
                return ((string.IsNullOrEmpty(env) ? "#" : env) + "__*");
            }

            string before = GetPatternvalue(RemoveIgnoredCharacters(entry.EnvironmentBefore, cieOptions), CIEOptions.IdenticalType.Before);
            string after = GetPatternvalue(RemoveIgnoredCharacters(entry.EnvironmentAfter, cieOptions), CIEOptions.IdenticalType.After);
            return ((string.IsNullOrEmpty(before) ? "#" : before) + "__" +
                (string.IsNullOrEmpty(after) ? "#" : after));
        }

        private static string GetPatternvalue(string pattern, CIEOptions.IdenticalType identype)
        {
            var value = string.Empty;
            if (pattern == null) return value;
            var t = pattern;
            switch (identype)
            {
                case CIEOptions.IdenticalType.Before:
                    for (int i = t.Length - 1; i >= 0; i--)
                    {
                        var info = App.IPASymbolCache[t[i]];
                        if (info.Type == IPASymbolType.consonant || info.Type == IPASymbolType.vowel)
                        {
                            value = t[i].ToString(CultureInfo.InvariantCulture);
                            break;
                        }
                    }
                    break;
                default:
                    for (int i = 0; i < t.Length; i++)
                    {
                        var info = App.IPASymbolCache[t[i]];
                        if (info.Type == IPASymbolType.consonant || info.Type == IPASymbolType.vowel)
                        {
                            value = t[i].ToString(CultureInfo.InvariantCulture);
                            break;
                        }
                    }
                    break;
            }
            return value;
        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes ignored characters from the specified environment string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string RemoveIgnoredCharacters(string environment, CIEOptions cieOptions)
		{
			if (string.IsNullOrEmpty(environment))
				return null;

			var ignoredList = cieOptions.SearchQuery.GetIgnoredCharacters(); 
			var bldrEnv = new StringBuilder(environment);

			// Get rid of all explicitly ignored characters (as opposed to
			// getting rid of all characters that are considered diacritics).
			bldrEnv = ignoredList.Aggregate(bldrEnv, (curr, ignoredChar) =>
				curr.Replace(ignoredChar, string.Empty));

			if (cieOptions.SearchQuery.IgnoreDiacritics)
			{
				// Loop through the characters in the environment string,
				// getting rid of diacritics.
				for (int i = bldrEnv.Length - 1; i >= 0; i--)
				{
					char chr = bldrEnv[i];
					if (chr != App.kBottomTieBarC && chr != App.kTopTieBarC)
					{
						var info = App.IPASymbolCache[chr];
						if (info != null && !info.IsBase)
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
	public class CIEOptions
	{
		/// ------------------------------------------------------------------------------------
		public enum IdenticalType
		{
			After,
			Before,
			Both
		}

		/// ------------------------------------------------------------------------------------
		public CIEOptions()
		{
			SearchQuery = new SearchQuery();
			Type = IdenticalType.Both;
		}

		/// ------------------------------------------------------------------------------------
		public CIEOptions Clone()
		{
			var options = new CIEOptions();
			options.SearchQuery = SearchQuery.Clone();
			options.Type = Type;
			return options;
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("IdenticalType")]
		public IdenticalType Type { get; set; }

		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery { get; set; }
	}

	#endregion
}
