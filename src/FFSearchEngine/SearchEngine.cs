using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SIL.Pa.Data;

namespace SIL.Pa.FFSearchEngine
{
	public class SearchEngine
	{
		public const string kIgnoredPhone = "\uFFFC";
		private static SearchQuery s_currQuery = new SearchQuery();
		private static List<string> s_ignoredPhones = new List<string>();
		private static List<char> s_ignoredChars = new List<char>();

		private static bool s_ignoreDiacritics = true;
		private static Dictionary<string, IPhoneInfo> s_phoneCache;
		private static bool s_ignoreUndefinedChars = true;

		private PatternGroup m_envBefore;
		private PatternGroup m_envAfter;
		private PatternGroup m_srchItem;
		private string[] m_phones = null;
		int m_matchIndex = 0;

		private List<string> m_errorMessages = new List<string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchEngine(SearchQuery query, Dictionary<string, IPhoneInfo> phoneCache)
			: this(query.Pattern)
		{
			CurrentSearchQuery = query;
			PhoneCache = phoneCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchEngine(SearchQuery query) : this(query.Pattern)
		{
			CurrentSearchQuery = query;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Contains a list of errors when there are any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] ErrorMessages
		{
			get { return m_errorMessages.ToArray(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchEngine(string pattern)
		{
			m_errorMessages.Clear();

			if (pattern == null)
				pattern = "";

			string[] patterns = pattern.Split(new char[] { '/', '_' });
			
			if (patterns.Length == 0 || string.IsNullOrEmpty(patterns[0]) ||
				(pattern.IndexOf('_') >= 0 && pattern.IndexOf('/') < 0))
			{
				m_errorMessages.Add(string.Format(
					Properties.Resources.kstidPatternSyntaxError,
					DataUtils.kEmptyDiamondPattern));

				return;
			}

			if (patterns.Length < 2)
				patterns = new string[] { patterns[0], "*", "*" };
			else if (patterns.Length < 3)
				patterns = new string[] { patterns[0], patterns[1], "*" };
			
			if (string.IsNullOrEmpty(patterns[1]))
				patterns[1] = "*";
			
			if (string.IsNullOrEmpty(patterns[2]))
				patterns[2] = "*";

			patterns[1] = patterns[1].Replace(DataUtils.kSearchPatternDiamond, "*");
			patterns[2] = patterns[2].Replace(DataUtils.kSearchPatternDiamond, "*");

			try
			{
				m_srchItem = new PatternGroup(EnvironmentType.Item);
				m_envBefore = new PatternGroup(EnvironmentType.Before);
				m_envAfter = new PatternGroup(EnvironmentType.After);

				if (!m_srchItem.Parse(patterns[0]))
					m_errorMessages.Add(Properties.Resources.kstidItemSyntaxError);
				else if (m_srchItem.Members == null || m_srchItem.Members.Count == 0)
				{
					m_errorMessages.Add(string.Format(
						Properties.Resources.kstidPatternParsedToNothingError,
						Properties.Resources.kstidSearchItemText));
				}

				if (!m_envBefore.Parse(patterns[1]))
					m_errorMessages.Add(Properties.Resources.kstidEnvBeforeSyntaxError);
				else if (m_envBefore.Members == null || m_envBefore.Members.Count == 0)
				{
					m_errorMessages.Add(string.Format(
						Properties.Resources.kstidPatternParsedToNothingError,
						Properties.Resources.kstidBeforeEnvironmentText));
				}

				if (!m_envAfter.Parse(patterns[2]))
					m_errorMessages.Add(Properties.Resources.kstidEnvAfterSyntaxError);
				else if (m_envAfter.Members == null || m_envAfter.Members.Count == 0)
				{
					m_errorMessages.Add(string.Format(
						Properties.Resources.kstidPatternParsedToNothingError,
						Properties.Resources.kstidAfterEnvironmentText));
				}
			}
			catch
			{
				m_errorMessages.Add(string.Format(
					Properties.Resources.kstidPatternSyntaxError, DataUtils.kEmptyDiamondPattern));
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the phone cache the search engine will use when searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Dictionary<string, IPhoneInfo> PhoneCache
		{
			get { return s_phoneCache; }
			set { s_phoneCache = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the search options used for subsequent searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SearchQuery CurrentSearchQuery
		{
			get { return s_currQuery; }
			set
			{
				s_currQuery = value;
				s_ignoreDiacritics = value.IgnoreDiacritics;

				s_ignoredPhones.Clear();
				s_ignoredChars.Clear();

				// Go through the ignored items and move those that are base characters
				// or complete phones (e.g. tone stick figures) to one collection and
				// those that aren't to another. It's assumed that ignored items that
				// are not base characters are only one codepoint in length.
				foreach (string ignoredItem in value.CompleteIgnoredList)
				{
					IPACharInfo charInfo = DataUtils.IPACharCache[ignoredItem];
					if (charInfo != null)
					{
						if (charInfo.IsBaseChar)
							s_ignoredPhones.Add(ignoredItem);
						else
							s_ignoredChars.Add(ignoredItem[0]);
					}
				}

				if (s_ignoreUndefinedChars)
					MergeInUndefinedIgnoredPhones();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Merges the undefined list of ignored phones with the main list of ignored phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void MergeInUndefinedIgnoredPhones()
		{
			if (IPACharCache.UndefinedCharacters != null && s_ignoredPhones != null)
			{
				foreach (UndefinedPhoneticCharactersInfo upci in IPACharCache.UndefinedCharacters)
				{
					if (!s_ignoredPhones.Contains(upci.Character.ToString()))
						s_ignoredPhones.Add(upci.Character.ToString());
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the undefined list of ignored phones from the main list of ignored phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void UnMergeInUndefinedIgnoredPhones()
		{
			if (IPACharCache.UndefinedCharacters != null && s_ignoredPhones != null)
			{
				foreach (UndefinedPhoneticCharactersInfo upci in IPACharCache.UndefinedCharacters)
				{
					if (s_ignoredPhones.Contains(upci.Character.ToString()))
						s_ignoredPhones.Remove(upci.Character.ToString());
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<char> IgnoredChars
		{
			get { return s_ignoredChars; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of all the characters to ignore when searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<string> IgnoredPhones
		{
			get { return s_ignoredPhones; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not diacritics are ignored when searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IgnoreDiacritics
		{
			get { return s_ignoreDiacritics; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IgnoreUndefinedCharacters
		{
			get { return s_ignoreUndefinedChars; }
			set
			{
				s_ignoreUndefinedChars = value;

				if (value)
					MergeInUndefinedIgnoredPhones();
				else
					UnMergeInUndefinedIgnoredPhones();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the environment before's pattern group
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternGroup EnvBeforePatternGroup
		{
			get { return m_envBefore; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the search item's pattern group
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternGroup SrchItemPatternGroup
		{
			get { return m_srchItem; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the environment after's pattern group
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternGroup EnvAfterPatternGroup
		{
			get { return m_envAfter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string of phones found in all the IPA character and IPA character run
		/// members of all the pattern pieces (i.e. search item and before and after
		/// environments).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] PhonesInPattern
		{
			get
			{
				StringBuilder bldrPhones = new StringBuilder();
				bldrPhones.Append(GetPhonesFromMember(m_srchItem));
				bldrPhones.Append(GetPhonesFromMember(m_envBefore));
				bldrPhones.Append(GetPhonesFromMember(m_envAfter));
				
				return (bldrPhones.Length == 0 ? null :
					IPACharCache.PhoneticParser(bldrPhones.ToString(), true));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of undefined phonetic characters found in all the IPA character and
		/// IPA character run members of all the pattern pieces (i.e. search item and before
		/// and after environments). 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public char[] InvalidCharactersInPattern
		{
			get
			{
				List<char> badChars = new List<char>();

				StringBuilder bldrPhones = new StringBuilder();
				bldrPhones.Append(GetPhonesFromMember(m_srchItem));
				bldrPhones.Append(GetPhonesFromMember(m_envBefore));
				bldrPhones.Append(GetPhonesFromMember(m_envAfter));

				foreach (char c in bldrPhones.ToString())
				{
					if (DataUtils.IPACharCache == null || DataUtils.IPACharCache[c] == null)
						badChars.Add(c);
				}

				return (badChars.Count == 0 ? null : badChars.ToArray());
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetPhonesFromMember(PatternGroup grp)
		{
			StringBuilder phones = new StringBuilder();

			if (grp == null)
				return string.Empty;

			foreach (object obj in grp.Members)
			{
				if (obj is PatternGroup)
					phones.Append(GetPhonesFromMember(obj as PatternGroup));
				else
				{
					PatternGroupMember member = obj as PatternGroupMember;
					if (member != null && member.Member != null &&
						member.Member.Trim() != string.Empty &&
						(member.MemberType == MemberType.SinglePhone ||
						member.MemberType == MemberType.IPACharacterRun))
					{
						phones.Append(member.Member.Trim());
					}
				}
			}

			return phones.ToString();
		}

		#region Diacritic Pattern comparer used by pattern group members and pattern groups.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a phone into its base portion and its diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ParsePhone(string phone, out string basePhone, out string diacritics)
		{
			// First, check if the phone is a tone letter.
			if (DataUtils.IPACharCache.ToneLetterInfo(phone) != null)
			{
				basePhone = phone;
				diacritics = null;
				return;
			}

			StringBuilder sbBasePhone = new StringBuilder();
			List<char> sbDiacritics = new List<char>(5);
			bool tiebarFound = false;

			foreach (char c in phone)
			{
				IPACharInfo charInfo = DataUtils.IPACharCache[c];

				// This should never be null. TODO: log meaningful error if it is.
				if (charInfo != null)
				{
					// Tie bars are counted as part of the base character.
					if (charInfo.IsBaseChar || c == DataUtils.kBottomTieBarC || c == DataUtils.kTopTieBarC)
					{
						sbBasePhone.Append(c);
						if (!tiebarFound && (c == DataUtils.kBottomTieBarC || c == DataUtils.kTopTieBarC))
							tiebarFound = true;
					}
					else
					{
						// The check will make sure we don't add duplicate diacritic marks to the
						// list which could happen if both characters under (or over) a tiebar are
						// modified with the same diacritic.
						if (!tiebarFound || !sbDiacritics.Contains(c))
							sbDiacritics.Add(c);
					}
				}
			}

			basePhone = sbBasePhone.ToString();
			diacritics = (sbDiacritics.Count == 0 ? null : new string(sbDiacritics.ToArray()));
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method kicks off comparing the specified phone's diacritics from a phone with
		/// those specified in the pattern (i.e. patternDiacritics).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CompareDiacritics(string patternDiacritics, string phone)
		{
			return CompareDiacritics(patternDiacritics, phone, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares the specified diacritics from the value in text to determine whether or
		/// not they match the diacritics in patternDiacritics. When textIsCompletePhone is
		/// true, the diacritics are stripped out. Otherwise, this method assumes text only
		/// contains diacritics and no base characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CompareDiacritics(string patternDiacritics, string text,
			bool textIsCompletePhone)
		{
			if (patternDiacritics == "*")
				return true;
			else if (patternDiacritics == null)
				patternDiacritics = string.Empty;

			string phonesDiacritics = text;

			// If this is true it's assumed the text is a complete phone (i.e. base character with
			// all its diacritics). Otherwise it's assumed the text is just one or more diacritics.
			if (textIsCompletePhone)
			{
				// Parse the phone (i.e. text) into it's base phone and the diacritics modying it.
				string basePhone;
				ParsePhone(text, out basePhone, out phonesDiacritics);
			}

			// Check for zero or more diacritics, plus the one(s) specified in the pattern.
			if (patternDiacritics.Contains("*"))
				return DiacriticsMatchZeroOrMore(patternDiacritics, phonesDiacritics);

			// Check for one or more diacritics, plus the one(s) specified in the pattern.
			if (patternDiacritics.Contains("+"))
				return DiacriticsMatchOneOrMore(patternDiacritics, phonesDiacritics);

			// Stip off the ignored diacritics from the phone.
			phonesDiacritics = RemoveIgnoredDiacritics(patternDiacritics, phonesDiacritics);

			// At this point, we know the diacritics specified in the pattern
			// must match exactly those modifying the vowel or consonant.
			return (phonesDiacritics == patternDiacritics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all ignored diacritics and suprasegmentals from the specifiec string of
		/// diacritics that were stripped from a phone and that are not in the pattern's
		/// diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string RemoveIgnoredDiacritics(string patternDiacritics, string phonesDiacritics)
		{
			if (phonesDiacritics == null)
				return string.Empty;

			// If non suprasegmental diacritics are ignored, then first remove them from the
			// phone's diacritics if they are not found in the pattern's diacritics.
			if (IgnoreDiacritics)
			{
				for (int i = 0; i < phonesDiacritics.Length; i++)
				{
					if (patternDiacritics.IndexOf(phonesDiacritics[i]) < 0)
					{
						IPACharInfo charInfo = DataUtils.IPACharCache[phonesDiacritics[i]];
						if (charInfo != null && charInfo.CharType == IPACharacterType.Diacritics)
							phonesDiacritics = phonesDiacritics.Replace(phonesDiacritics[i], DataUtils.kOrc);
					}
				}

				phonesDiacritics = phonesDiacritics.Replace(DataUtils.kOrc.ToString(), string.Empty);
			}

			// Now remove all ignored, non base char. suprasegmentals
			// that are not explicitly in the pattern's diacritics.
			foreach (char sseg in s_ignoredChars)
			{
				if (patternDiacritics.IndexOf(sseg) < 0)
					phonesDiacritics = phonesDiacritics.Replace(sseg.ToString(), string.Empty);

				if (phonesDiacritics.Length == 0)
					break;
			}

			return phonesDiacritics;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a phone's diacritics match when the member contains a * following
		/// a base character, V or C.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool DiacriticsMatchZeroOrMore(string patternDiacritics, string phoneDiacritics)
		{
			// If the pattern is just *, then we have a match, regardless of whether or not
			// the phone is modified by any diacritics.
			if (patternDiacritics == "*")
				return true;

			if (phoneDiacritics == null)
				return false;

			// Split the diacritic pattern at the + or * symbol.
			string[] pieces = patternDiacritics.Split("*".ToCharArray());

			// Check if the pattern is something like "*^" (where ^ represents diacritics)
			if (pieces[0] == string.Empty)
				return phoneDiacritics.EndsWith(pieces[1]);

			// Check if the pattern is something like "^*" (where ^ represents diacritics)
			if (pieces[1] == string.Empty)
				return phoneDiacritics.StartsWith(pieces[0]);

			// Check if the pattern is something like "^*^" (where ^ represents diacritics)
			return phoneDiacritics.StartsWith(pieces[0]) &&
				phoneDiacritics.EndsWith(pieces[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a phone's diacritics match when the member contains a + following
		/// a base character, V or C.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool DiacriticsMatchOneOrMore(string patternDiacritics, string phoneDiacritics)
		{
			if (phoneDiacritics == null)
				return false;

			// If the pattern is just *, then we have a match, regardless of whether or not
			// the phone is modified by any diacritics.
			if (patternDiacritics == "+")
				return (phoneDiacritics.Length > 0);

			// Split the diacritic pattern at the + or * symbol.
			string[] pieces = patternDiacritics.Split("+".ToCharArray());

			// Check if the pattern is something like "+^" (where ^ represents diacritics)
			if (pieces[0] == string.Empty)
			{
				int i = phoneDiacritics.IndexOf(pieces[1]);
				return (phoneDiacritics.EndsWith(pieces[1]) && i > 0);
			}

			// Check if the pattern is something like "^+" (where ^ represents diacritics)
			if (pieces[1] == string.Empty)
			{
				return (phoneDiacritics.StartsWith(pieces[0]) &&
					phoneDiacritics.Length > pieces[0].Length);
			}

			// Check if the pattern is something like "^+^" (where ^ represents diacritics)
			return (phoneDiacritics.StartsWith(pieces[0]) &&
				phoneDiacritics.EndsWith(pieces[1]) &&
				phoneDiacritics.Length > (pieces[0].Length + pieces[1].Length));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified phonetic character array for pattern matches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SearchWord(string[] eticChars, out int[] result)
		{
			m_phones = eticChars;
			m_matchIndex = 0;
			return SearchWord(out result);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches a previously specified word or array of phonetic characters for a pattern
		/// match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SearchWord(out int[] result)
		{
			result = new int[] {-1, -1};

			if (m_phones == null)
				return false;

			while (m_matchIndex < m_phones.Length)
			{
				result = new int[] { -1, -1 };

				// First, look for the search item.
				if (m_srchItem == null || !m_srchItem.Search(m_phones, m_matchIndex, out result))
					return false;

				// Save where the match was found.
				m_matchIndex = result[0];

				// Now search before the match and after the match to
				// see if we match on the environment before and after.
				if (m_envBefore.Search(m_phones, m_matchIndex - 1))
				{
					if (m_envAfter.Search(m_phones, m_matchIndex + result[1]))
					{
						m_matchIndex++;
						return true;
					}
				}

				m_matchIndex++;
			}

			return false;
		}
	}
}
