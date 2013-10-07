using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single saved search query
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchQuery
	{
		public const float kCurrVersion = 3.0f;

		private List<SearchQueryValidationError> _errors = new List<SearchQueryValidationError>();
		private string _pattern;

		/// ------------------------------------------------------------------------------------
		public SearchQuery()
		{
			Reset();
			ShowAllOccurrences = true;
		}

		/// ------------------------------------------------------------------------------------
		public SearchQuery(string pattern) : this()
		{
			Pattern = pattern;
		}

		#region Public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resets options to defaults.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			Pattern = null;
			Category = null;
			ShowAllOccurrences = true;
			IgnoreDiacritics = true;
			IgnoredCharacters = GetDefaultIgnoredCharacters();
			_errors.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones this instance of the SearchQuery object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery Clone()
		{
			var clone = new SearchQuery
			{
				Name = Name,
				Pattern = Pattern,
				Id = Id,
				Category = Category,
				ShowAllOccurrences = ShowAllOccurrences,
				IncludeAllUncertainPossibilities = IncludeAllUncertainPossibilities,
				IgnoreDiacritics = IgnoreDiacritics,
				IgnoredCharacters = IgnoredCharacters,
				IsPatternRegExpression = IsPatternRegExpression,
			};

			foreach (var error in Errors)
				clone.Errors.Add(error.Copy());

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares the query with the one specified to determine if they're contents are
		/// the same. This is not a reference comparison. In fact, passing in "this" is not
		/// valid;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsEqual(SearchQuery query)
		{
			Debug.Assert(query != this);

			if (Name != query.Name || Pattern != query.Pattern ||
				Category != query.Category || PatternOnly != query.PatternOnly)
			{
				return false;
			}

			return AreOptionsEqual(query);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares the options of the query with those of the one specified and returns
		/// true if they are the same. Otherwise false is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreOptionsEqual(SearchQuery query)
		{
			Debug.Assert(query != this);

			if (ShowAllOccurrences != query.ShowAllOccurrences ||
				IgnoreDiacritics != query.IgnoreDiacritics ||
				IncludeAllUncertainPossibilities != query.IncludeAllUncertainPossibilities)
			{
				return false;
			}

			return (IgnoredCharacters.Equals(query.IgnoredCharacters, StringComparison.Ordinal));

			//return (StringContentsEqual(IgnoredLengthList, query.IgnoredLengthList) &&
			//    StringContentsEqual(IgnoredStressList, query.IgnoredStressList) &&
			//    StringContentsEqual(IgnoredToneList, query.IgnoredToneList) &&
			//    StringContentsEqual(IgnoredBoundaryList, query.IgnoredBoundaryList));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return's the SearchQuery's name if it's not null or the pattern when name is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(Name))
				return Name;

			//if (IsPatternRegExpression)
			//{
			//    string[] patternParts = Pattern.Split(App.kOrc);

			//    if (patternParts.Length == 3)
			//        return patternParts[0] + "/" + patternParts[1] + "_" + patternParts[2];
			//}

			return SearchItem + "/" + PrecedingEnvironment + "_" + FollowingEnvironment;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of errors generated by parsing a pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<SearchQueryValidationError> Errors
		{
			get { return (_errors ?? (_errors = new List<SearchQueryValidationError>())); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the pattern in this query is the
		/// only valid information. This property is used for dragging and dropping
		/// SearchQuery objects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool PatternOnly { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating the query's version.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("version")]
		public float Version
		{
			get { return kCurrVersion; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Pattern
		{
			get { return _pattern; }
			set
			{
				_pattern = (value == null ? null : value.Replace("/" + App.kSearchPatternDiamond, "*")
					.Replace("_" + App.kSearchPatternDiamond, "*"));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the pattern is a regular expression.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsPatternRegExpression { get; set; }

		/// ------------------------------------------------------------------------------------
		public string PrecedingEnvironment
		{
			get
			{
				var pieces = GetPatternPieces(Pattern);

				if (pieces == null || pieces.Length < 2 || pieces[1] == string.Empty)
					return "*";

				pieces[1] = pieces[1].Replace(App.kSearchPatternDiamond, "*");
				pieces[1] = pieces[1].Trim();
                if (pieces[1].StartsWith("*", StringComparison.Ordinal) && pieces[1].Length > 1)
					pieces[1] = pieces[1].TrimStart('*');

                if (pieces[1].StartsWith("#*", StringComparison.Ordinal))
					pieces[1] = "#" + pieces[1].TrimStart('*', '#');

				return pieces[1];
			}
		}

		/// ------------------------------------------------------------------------------------
		public string SearchItem
		{
			get
			{
				var pieces = GetPatternPieces(Pattern);
				return (pieces != null && pieces.Length == 3 ? pieces[0] : String.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		public string FollowingEnvironment
		{
			get
			{
				var pieces = GetPatternPieces(Pattern);

				if (pieces == null || pieces.Length < 3 || pieces[2] == string.Empty)
					return "*";

				pieces[2] = pieces[2].Replace(App.kSearchPatternDiamond, "*");
				pieces[2] = pieces[2].Trim();
                if (pieces[2].EndsWith("*", StringComparison.Ordinal) && pieces[2].Length > 1)
					pieces[2] = pieces[2].TrimEnd('*');

                if (pieces[2].EndsWith("*#", StringComparison.Ordinal))
					pieces[2] = pieces[2].TrimEnd('*', '#') + "#";

				return pieces[2];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the original pattern as it was last read from the persisted store.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Id { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Category { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool ShowAllOccurrences { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IncludeAllUncertainPossibilities { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IgnoreDiacritics { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("ignoredCharacters")]
		public string IgnoredCharacters { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetIgnoredCharacters()
		{
			var list = (IgnoredCharacters == null ? new string[0] :
				IgnoredCharacters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

			// Now sort ignored characters by their length -- longest to shortest.
			return list.OrderByDescending(ic => ic.Length);
		}

		/// ------------------------------------------------------------------------------------
		public SearchEngine GetSearchEngine(out Exception e)
		{
			e = null;
			Errors.Clear();
			SearchQuery modifiedQuery;

			if (!App.ConvertClassesToPatterns(this, out modifiedQuery, false))
			{
				e = new Exception("There was an error converting classes to patterns in " + Pattern);
				return null;
			}

			return new SearchEngine(modifiedQuery, App.Project.PhoneCache);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Checks each character in the query to see if they are in the phonetic character
		///// inventory. If there are some that are invalid, then a list of them is returned.
		/////  If the pattern failed to parse, then a SearchQueryException is returned. 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public object GetSymbolsNotInInventory()
		//{
		//    SearchQueryException e;
		//    var engine = GetSearchEngine(out e);

		//    if (e != null)
		//        return e;

		//    return engine.GetInvalidSymbolsInPattern();
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Checks each phone in the query to see if it's in the project's phone cache. A list
		///// is made of all phones in the query that are not in the cache. If the pattern
		///// failed to parse, then a SearchQueryException is returned. 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public object GetPhonesNotInCache()
		//{
		//    SearchQueryException e;
		//    var engine = GetSearchEngine(out e);

		//    if (e != null)
		//        return e;

		//    var phonesInQuery = engine.GetPhonesInPattern();
		//    if (phonesInQuery == null)
		//        return null;

		//    var phonesNotInData = phonesInQuery.Where(p => !App.Project.PhoneCache.ContainsKey(p))
		//        .Distinct(StringComparer.Ordinal).ToArray();

		//    return (phonesNotInData.Length == 0 ? null : phonesNotInData);
		//}

		/// ------------------------------------------------------------------------------------
		public static string GetDefaultIgnoredCharacters()
		{
			if (App.IPASymbolCache == null)
				return null;

			var ignoreList = new StringBuilder();
			foreach (var info in App.IPASymbolCache.Where(i => i.Value.SubType != IPASymbolSubType.notApplicable))
				ignoreList.AppendFormat("{0},", info.Value.Literal);

			return (ignoreList.Length == 0 ? null : ignoreList.ToString().TrimEnd(','));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a pattern that infers the value of the environments when they are missing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetInferredPattern()
		{
			return (Pattern == App.kEmptyDiamondPattern ? Pattern :
				SearchItem + "/" + PrecedingEnvironment + "_" + FollowingEnvironment);
		}

		#region Static methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the pattern into its search item pattern, its environment before pattern
		/// and its environment after pattern. Before doing so, however, it checks for
		/// slashes and underscores that may be part of feature names (e.g. Tap/Flap).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string[] GetPatternPieces(string pattern)
		{
			// Replace slashes and underscores that occur between square brackets with tokens
			// that are replaced with the slashes and underscores after the pattern is split up.
			try
			{
				var bldr = new StringBuilder(pattern);
				var bracketBucket = new Stack<char>();
				for (int i = 0; i < bldr.Length; i++)
				{
					switch (bldr[i])
					{
						case '[': bracketBucket.Push(bldr[i]); break;
						case ']': bracketBucket.Pop(); break;
						case '/':
							// When slashes are found inside brackets, replace them with codepoint 1.
							if (bracketBucket.Count > 0)
								bldr[i] = (char)1;
							break;

						case '_':
							// When underscores are found inside brackets, replace them with codepoint 2.
							if (bracketBucket.Count > 0)
								bldr[i] = (char)2;
							break;
					}
				}

				// Split up the pattern into it's pieces. Three pieces are expected.
				string[] pieces = bldr.ToString().Split('/', '_');

				// Now go through the pieces and put back any slashes
				// or undersores that were replaced by tokens above.
				for (int i = 0; i < pieces.Length; i++)
				{
					pieces[i] = pieces[i].Replace((char)1, '/');
					pieces[i] = pieces[i].Replace((char)2, '_');
				}

				return pieces;
			}
			catch { }

			return null;
		}

		#endregion
	}
}
