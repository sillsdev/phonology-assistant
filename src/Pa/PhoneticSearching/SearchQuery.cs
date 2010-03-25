using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	#region SearchQuery class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single saved search query
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchQuery
	{
		private const float kCurrVersion = 2.0f;

		public static string s_defaultIgnoredStressChars = null;
		public static string s_defaultIgnoredToneChars = null;
		public static string s_defaultIgnoredLengthChars = null;

		private string m_name;
		private int m_id = 0;
		private string m_pattern;
		private bool m_showAllOccurrences = true;
		private bool m_includeAllUncertainPossibilities = false;
		private bool m_ignoreDiacritics = false;
		private string m_ignoredStressChars = DefaultIgnoredStressChars;
		private string m_ignoredToneChars = DefaultIgnoredToneChars;
		private string m_ignoredLengthChars = DefaultIgnoredLengthChars;
		private bool m_patternOnly = false;
		private string m_category;
		private List<string> m_ignoredStressList;
		private List<string> m_ignoredToneList;
		private List<string> m_ignoredLengthList;
		private bool m_isPatternRegExp = false;

		private List<string> m_errors = new List<string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery(string pattern)
		{
			Reset();
			m_pattern = pattern;
		}

		#region Public methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resets options to defaults.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			m_pattern = null;
			m_category = null;
			m_showAllOccurrences = true;
			m_ignoreDiacritics = true;
			m_ignoredStressChars = DefaultIgnoredStressChars;
			m_ignoredToneChars = DefaultIgnoredToneChars;
			m_ignoredLengthChars = DefaultIgnoredLengthChars;
			m_errors.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones this instance of the SearchQuery object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery Clone()
		{
			SearchQuery clone = new SearchQuery();
			clone.Name = Name;
			clone.Pattern = Pattern;
			clone.Id = Id;
			clone.Category = Category;
			clone.ShowAllOccurrences = ShowAllOccurrences;
			clone.IncludeAllUncertainPossibilities = IncludeAllUncertainPossibilities;
			clone.IgnoreDiacritics = IgnoreDiacritics;
			clone.IgnoredStressChars = IgnoredStressChars;
			clone.IgnoredToneChars = IgnoredToneChars;
			clone.IgnoredLengthChars = IgnoredLengthChars;
			clone.IsPatternRegExpression = IsPatternRegExpression;
			clone.ErrorMessages.AddRange(ErrorMessages);

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

			if (m_name != query.m_name || m_pattern != query.m_pattern ||
				m_category != query.m_category || m_patternOnly != query.m_patternOnly)
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

			if (m_showAllOccurrences != query.m_showAllOccurrences ||
				m_ignoreDiacritics != query.m_ignoreDiacritics ||
				m_includeAllUncertainPossibilities != query.m_includeAllUncertainPossibilities)
			{
				return false;
			}

			return (StringContentsEqual(IgnoredLengthList, query.IgnoredLengthList) &&
				StringContentsEqual(IgnoredStressList, query.IgnoredStressList) &&
				StringContentsEqual(IgnoredToneList, query.IgnoredToneList));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return's the SearchQuery's name if it's not null or the pattern when name is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(m_name))
				return m_name;

			if (m_isPatternRegExp)
			{
				string[] patternParts = m_pattern.Split(new char[] { App.kOrc });

				if (patternParts.Length == 3)
					return patternParts[0] + "/" + patternParts[1] + "_" + patternParts[2];
			}

			return m_pattern;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of errors generated by parsing a pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> ErrorMessages
		{
			get { return (m_errors != null ? m_errors : (m_errors = new List<string>())); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the pattern in this query is the
		/// only valid information. This property is used for dragging and dropping
		/// SearchQuery objects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool PatternOnly
		{
			get { return m_patternOnly; }
			set { m_patternOnly = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating the query's version.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("version")]
		public float Version
		{
			get { return kCurrVersion; }
			set { ; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the name of the query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the pattern of the query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Pattern
		{
			get { return m_pattern; }
			set { m_pattern = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the pattern is a regular expression.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsPatternRegExpression
		{
			get { return m_isPatternRegExp; }
			set { m_isPatternRegExp = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the original pattern as it was last read from the persisted store.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the query's category.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Category
		{
			get { return m_category; }
			set { m_category = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowAllOccurrences
		{
			get { return m_showAllOccurrences; }
			set { m_showAllOccurrences = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IncludeAllUncertainPossibilities
		{
			get { return m_includeAllUncertainPossibilities; }
			set { m_includeAllUncertainPossibilities = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IgnoreDiacritics
		{
			get	{return m_ignoreDiacritics;	}
			set { m_ignoreDiacritics = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of ignored stress characters in a comma-delimited string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string IgnoredStressChars
		{
			get { return m_ignoredStressChars; }
			set
			{
				m_ignoredStressChars = value;
				m_ignoredStressList = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of ignored stress characters in a collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> IgnoredStressList
		{
			get
			{
				if (m_ignoredStressList == null)
					ParseIgnoredChars(m_ignoredStressChars, out m_ignoredStressList);

				return m_ignoredStressList;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of ignored tone characters in a comma-delimited string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string IgnoredToneChars
		{
			get { return m_ignoredToneChars; }
			set
			{
				m_ignoredToneChars = value;
				m_ignoredToneList = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of ignored tone characters in a collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> IgnoredToneList
		{
			get
			{
				if (m_ignoredToneList == null)
					ParseIgnoredChars(m_ignoredToneChars, out m_ignoredToneList);

				return m_ignoredToneList;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of ignored length characters in a comma-delimited string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string IgnoredLengthChars
		{
			get { return m_ignoredLengthChars; }
			set
			{
				m_ignoredLengthChars = value;
				m_ignoredLengthList = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of ignored length characters in a collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> IgnoredLengthList
		{
			get
			{
				if (m_ignoredLengthList == null)
					ParseIgnoredChars(m_ignoredLengthChars, out m_ignoredLengthList);

				return m_ignoredLengthList;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a single list containing all of the ingored tone, stress and length
		/// characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> CompleteIgnoredList
		{
			get
			{
				List<string> allIgnored = new List<string>();
				allIgnored.AddRange(IgnoredToneList);
				allIgnored.AddRange(IgnoredStressList);
				allIgnored.AddRange(IgnoredLengthList);
				return allIgnored;
			}
		}

		#endregion

		#region Static properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default list of stress and syllable characters to ignore when peforming
		/// find phone searches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string DefaultIgnoredStressChars
		{
			get
			{
				if (s_defaultIgnoredStressChars != null)
					return s_defaultIgnoredStressChars;

				if (App.IPASymbolCache == null)
					return null;

				StringBuilder ignoreList = new StringBuilder();
				foreach (KeyValuePair<int, IPASymbol> info in App.IPASymbolCache)
				{
					if (info.Value.IgnoreType == IPASymbolIgnoreType.StressSyllable)
					{
						ignoreList.Append(info.Value.Literal);
						ignoreList.Append(",");
					}
				}

				return ignoreList.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default list of tone characters to ignore when peforming
		/// find phone searches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string DefaultIgnoredToneChars
		{
			get
			{
				if (s_defaultIgnoredToneChars != null)
					return s_defaultIgnoredToneChars;

				if (App.IPASymbolCache == null)
					return null;

				StringBuilder ignoreList = new StringBuilder();
				foreach (KeyValuePair<int, IPASymbol> info in App.IPASymbolCache)
				{
					if (info.Value.IgnoreType == IPASymbolIgnoreType.Tone)
					{
						ignoreList.Append(info.Value.Literal);
						ignoreList.Append(",");
					}
				}

				return ignoreList.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default list of length characters to ignore when peforming
		/// find phone searches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string DefaultIgnoredLengthChars
		{
			get
			{
				if (s_defaultIgnoredLengthChars != null)
					return s_defaultIgnoredLengthChars;

				if (App.IPASymbolCache == null)
					return null;
				
				StringBuilder ignoreList = new StringBuilder();
				foreach (KeyValuePair<int, IPASymbol> info in App.IPASymbolCache)
				{
					if (info.Value.IgnoreType == IPASymbolIgnoreType.Length)
					{
						ignoreList.Append(info.Value.Literal);
						ignoreList.Append(",");
					}
				}

				return ignoreList.ToString();
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a comma-delimited string into a collection of strings and returns the
		/// collection sorted by the length of the strings in the collection with longer
		/// strings coming before shorter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ParseIgnoredChars(string ignoredChars, out List<string> ignoredList)
		{
			ignoredList = new List<string>();
			if (string.IsNullOrEmpty(ignoredChars))
				return;

			// Parse the ignored character string into a collection of strings.
			ignoredList.AddRange(ignoredChars.Split(",".ToCharArray(),
				StringSplitOptions.RemoveEmptyEntries));

			// Now sort the strings in the list by their length -- longest to shortest.
			for (int i = ignoredList.Count - 1; i >= 0; i--)
			{
				if (ignoredList[i].Length > ignoredList[0].Length)
				{
					ignoredList.Insert(0, ignoredList[i]);
					ignoredList.RemoveAt(i + 1);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the strings in one collection are all found
		/// in another. For the contents to be equal, the collection lengths must be identical
		/// but the order of the strings within the collections do not.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool StringContentsEqual(List<string> lst1, List<string> lst2)
		{
			if (lst1 == null && lst2 == null)
				return true;

			if ((lst1 == null && lst2 != null) || (lst2 == null && lst1 != null))
				return false;

			if (lst1.Count != lst2.Count)
				return false;

			foreach (string item in lst1)
			{
				if (!lst2.Contains(item))
					return false;
			}

			return true;
		}
	}

	#endregion
}
