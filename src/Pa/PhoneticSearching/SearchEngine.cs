using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class SearchEngine
	{
		public enum WordBoundaryCondition
		{
			NoCondition,
			BeginningOfSearchItem,
			EndOfSearchItem
		}

		public enum ZeroOrMoreCondition
		{
			NoCondition,
			InSearchItem,
			MoreThanOneInEnvBefore,
			MoreThanOneInEnvAfter,
			NotBeginningOfEnvBefore,
			NotEndOfEnvAfter,
		}

		public enum OneOrMoreCondition
		{
			NoCondition,
			InSearchItem,
			MoreThanOneInEnvBefore,
			MoreThanOneInEnvAfter,
			NotBeginningOfEnvBefore,
			NotEndOfEnvAfter,
		}
		
		public const string kIgnoredPhone = "\uFFFC";
		private static SearchQuery s_currQuery = new SearchQuery();

		private static bool s_ignoreUndefinedChars = true;

		private readonly PatternGroup m_envBefore;
		private readonly PatternGroup m_envAfter;
		private readonly PatternGroup m_srchItem;
		private readonly string m_envBeforeStr = string.Empty;
		private readonly string m_envAfterStr = string.Empty;
		private readonly string m_srchItemStr = string.Empty;
		private string[] m_phones;
		int m_matchIndex;

		private readonly List<string> m_errors = new List<string>();

		/// ------------------------------------------------------------------------------------
		static SearchEngine()
		{
			IgnoredPhones = new List<string>();
			IgnoredChars = new List<char>();
			IgnoreDiacritics = true;
		}

		/// ------------------------------------------------------------------------------------
		public SearchEngine(SearchQuery query, Dictionary<string, IPhoneInfo> phoneCache)
			: this(query.Pattern)
		{
			CurrentSearchQuery = query;
			PhoneCache = phoneCache;

			if (m_errors != null && m_errors.Count > 0)
				query.ErrorMessages.AddRange(m_errors);
		}

		/// ------------------------------------------------------------------------------------
		public SearchEngine(SearchQuery query) : this(query.Pattern)
		{
			CurrentSearchQuery = query;

			if (m_errors != null && m_errors.Count > 0)
				query.ErrorMessages.AddRange(m_errors);
		}

		/// ------------------------------------------------------------------------------------
		public SearchEngine(string pattern)
		{
			m_errors.Clear();

			if (pattern == null)
				pattern = string.Empty;

			string[] patterns = GetPatternPieces(pattern);
			if (patterns == null)
			{
				m_errors.Add(string.Format(GetSyntaxErrorMsg(), pattern));
				return;
			}

			if (patterns.Length == 0 || patterns.Length > 3 ||
				string.IsNullOrEmpty(patterns[0]) ||
				(pattern.IndexOf('_') >= 0 && pattern.IndexOf('/') < 0))
			{
				m_errors.Add(string.Format(GetPatternSyntaxErrorMsg(), App.kEmptyDiamondPattern));
				return;
			}

			if (patterns.Length < 2)
				patterns = new[] { patterns[0], "*", "*" };
			else if (patterns.Length < 3)
				patterns = new[] { patterns[0], patterns[1], "*" };

			if (string.IsNullOrEmpty(patterns[1]))
				patterns[1] = "*";
			
			if (string.IsNullOrEmpty(patterns[2]))
				patterns[2] = "*";

			patterns[1] = patterns[1].Replace(App.kSearchPatternDiamond, "*");
			patterns[2] = patterns[2].Replace(App.kSearchPatternDiamond, "*");

			try
			{
				m_srchItem = new PatternGroup(EnvironmentType.Item);
				m_envBefore = new PatternGroup(EnvironmentType.Before);
				m_envAfter = new PatternGroup(EnvironmentType.After);

				Parse(m_srchItem, patterns[0]);
				Parse(m_envBefore, patterns[1]);
				Parse(m_envAfter, patterns[2]);
			}
			catch
			{
				m_errors.Add(string.Format(GetPatternSyntaxErrorMsg(), App.kEmptyDiamondPattern));
			}

			m_srchItemStr = patterns[0];
			m_envBeforeStr = patterns[1];
			m_envAfterStr = patterns[2];
		}

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
				StringBuilder bldr = new StringBuilder(pattern);
				Stack<char> bracketBucket = new Stack<char>();
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
				// or undersores that were replace by tokens above.
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the phones in a the search pattern
		/// should be run through the experimental transcription conversion process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ConvertPatternWithTranscriptionChanges { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Contains a list of errors when there are any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] ErrorMessages
		{
			get { return m_errors.ToArray(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines the list of error messages into a single message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCombinedErrorMessages()
		{
			return GetCombinedErrorMessages(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines the list of error messages into a single message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCombinedErrorMessages(bool separateErrorsWithLineBreaks)
		{
			if (m_errors == null || m_errors.Count == 0)
				return null;

			var errors = new StringBuilder();
			foreach (var err in m_errors)
			{
				errors.Append(err);
				errors.Append(separateErrorsWithLineBreaks ? Environment.NewLine : " ");
			}

			var fmt = App.GetString("SearchEngine.PatternParsingErrorMsg",
				"The following error(s) occurred when parsing the search pattern:\n\n{0}",
				"Search Query Messages");

			return string.Format(fmt, errors.ToString().Trim());
		}

		/// ------------------------------------------------------------------------------------
		private void Parse(PatternGroup ptrnGrp, string pattern)
		{
			var success = ptrnGrp.Parse(pattern, m_errors);
			var envType = GetEnvironmentTypeString(ptrnGrp.EnvironmentType);
			
			if (ptrnGrp.Members == null || ptrnGrp.Members.Count == 0)
			{
				var fmt = App.GetString("SearchEngine.ParsedToNothingErrorMsg",
					"Error parsing the {0}.", "Search Query Messages");

				m_errors.Add(string.Format(fmt, envType));
				return;
			}

			if (!success)
				m_errors.Add(string.Format(GetSyntaxErrorMsg(), envType));
		}

		/// ------------------------------------------------------------------------------------
		public static string GetEnvironmentTypeString(EnvironmentType type)
		{
			switch (type)
			{
				case EnvironmentType.Before: return App.GetString("SearchEngine.EnvironmentBefore", "preceding environment");
				case EnvironmentType.After: return App.GetString("SearchEngine.EnvironmentAfter", "following environment");
				case EnvironmentType.Item: return App.GetString("SearchEngine.SearchItem", "search item");
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetSyntaxErrorMsg()
		{
			return App.GetString("SearchEngine.SyntaxErrorMsg", "Syntax error in {0}.");
		}

		/// ------------------------------------------------------------------------------------
		private static string GetPatternSyntaxErrorMsg()
		{
			return App.GetString("SearchEngine.PatternSyntaxErrorMsg",
				"Syntax error. Pattern is not in the correct format: {0}");
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the phone cache the search engine will use when searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Dictionary<string, IPhoneInfo> PhoneCache { get; set; }

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
				IgnoreDiacritics = value.IgnoreDiacritics;

				IgnoredPhones.Clear();
				IgnoredChars.Clear();

				// Go through the ignored items and move those that are base characters
				// or complete phones (e.g. tone stick figures) to one collection and
				// those that aren't to another. It's assumed that ignored items that
				// are not base characters are only one codepoint in length.
				foreach (string ignoredItem in value.CompleteIgnoredList)
				{
					var charInfo = App.IPASymbolCache[ignoredItem];
					if (charInfo != null)
					{
						if (charInfo.IsBase)
							IgnoredPhones.Add(ignoredItem);
						else
							IgnoredChars.Add(ignoredItem[0]);
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
			if (App.IPASymbolCache.UndefinedCharacters != null && IgnoredPhones != null)
			{
				foreach (var upci in App.IPASymbolCache.UndefinedCharacters.Where(upci => !IgnoredPhones.Contains(upci.Character.ToString())))
				{
					IgnoredPhones.Add(upci.Character.ToString());
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
			if (App.IPASymbolCache.UndefinedCharacters != null && IgnoredPhones != null)
			{
				foreach (var upci in App.IPASymbolCache.UndefinedCharacters)
				{
					if (IgnoredPhones.Contains(upci.Character.ToString()))
						IgnoredPhones.Remove(upci.Character.ToString());
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public static List<char> IgnoredChars { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of all the characters to ignore when searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<string> IgnoredPhones { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not diacritics are ignored when searching.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IgnoreDiacritics { get; private set; }

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
		public string[] GetPhonesInPattern()
		{
			var bldrPhones = new StringBuilder();
			bldrPhones.Append(GetPhonesFromMember(m_srchItem));
			bldrPhones.Append(GetPhonesFromMember(m_envBefore));
			bldrPhones.Append(GetPhonesFromMember(m_envAfter));

			return (bldrPhones.Length == 0 ? null :
				App.Project.PhoneticParser.Parse(bldrPhones.ToString(), true,
				ConvertPatternWithTranscriptionChanges));
		}

		/// ------------------------------------------------------------------------------------
		public string[] GetPhonesInSearchItem()
		{
			var bldrPhones = new StringBuilder(GetPhonesFromMember(m_srchItem));
			return (bldrPhones.Length == 0 ? null :
				App.Project.PhoneticParser.Parse(bldrPhones.ToString(), true,
				ConvertPatternWithTranscriptionChanges));
		}

		/// ------------------------------------------------------------------------------------
		public string[] GetPhonesInPrecedingEnv()
		{
			var bldrPhones = new StringBuilder(GetPhonesFromMember(m_envBefore));
			return (bldrPhones.Length == 0 ? null :
				App.Project.PhoneticParser.Parse(bldrPhones.ToString(), true,
				ConvertPatternWithTranscriptionChanges));
		}

		/// ------------------------------------------------------------------------------------
		public string[] GetPhonesInFollowingEnv()
		{
			var bldrPhones = new StringBuilder(GetPhonesFromMember(m_envAfter));
			return (bldrPhones.Length == 0 ? null :
				App.Project.PhoneticParser.Parse(bldrPhones.ToString(), true,
				ConvertPatternWithTranscriptionChanges));
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of undefined phonetic characters found in all the IPA character and
		/// IPA character run members of all the pattern pieces (i.e. search item and before
		/// and after environments). 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public char[] GetInvalidSymbolsInPattern()
		{
			var undefinedChars = new List<char>();
			undefinedChars.AddRange(GetInvalidSymbolsInSearchItem());
			undefinedChars.AddRange(GetInvalidSymbolsInPrecedingEnv());
			undefinedChars.AddRange(GetInvalidSymbolsInFollowingEnv());
			return (undefinedChars.Count == 0 ? null : undefinedChars.ToArray());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<char> GetInvalidSymbolsInSearchItem()
		{
			return GetInvalidCharsFromMember(m_srchItem);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<char> GetInvalidSymbolsInPrecedingEnv()
		{
			return GetInvalidCharsFromMember(m_envBefore);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<char> GetInvalidSymbolsInFollowingEnv()
		{
			return GetInvalidCharsFromMember(m_envAfter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not there is an invalid word boundary symbol
		/// found in the search pattern, and where it is.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordBoundaryCondition GetWordBoundaryCondition()
		{
			string srchItemPattern = (m_srchItem == null ? string.Empty : m_srchItem.ToString());
			if (srchItemPattern.StartsWith("#"))
				return WordBoundaryCondition.BeginningOfSearchItem;

			if (srchItemPattern.EndsWith("#"))
				return WordBoundaryCondition.EndOfSearchItem;

			return WordBoundaryCondition.NoCondition;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not there is an invalid word boundary symbol
		/// found in the search pattern, and where it is.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ZeroOrMoreCondition GetZeroOrMoreCondition()
		{
			string tmpSrchItem = RemoveDiacriticPlaceholderModifiers(m_srchItemStr);
			string tmpEnvBefore = RemoveDiacriticPlaceholderModifiers(m_envBeforeStr);
			string tmpEnvAfter = RemoveDiacriticPlaceholderModifiers(m_envAfterStr);

			// Check search item
			if (tmpSrchItem.Contains("*"))
				return ZeroOrMoreCondition.InSearchItem;

			// Check environment before
			string[] pieces = tmpEnvBefore.Split("*".ToCharArray());

			if (pieces.Length > 2)
			    return ZeroOrMoreCondition.MoreThanOneInEnvBefore;

			if (pieces.Length > 1 && !tmpEnvBefore.StartsWith("*"))
				return ZeroOrMoreCondition.NotBeginningOfEnvBefore;

			// Check environment after
			pieces = tmpEnvAfter.Split("*".ToCharArray());

			if (pieces.Length > 2)
				return ZeroOrMoreCondition.MoreThanOneInEnvAfter;

			if (pieces.Length > 1 && !tmpEnvAfter.EndsWith("*"))
				return ZeroOrMoreCondition.NotEndOfEnvAfter;

			return ZeroOrMoreCondition.NoCondition;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not there is an invalid word boundary symbol
		/// found in the search pattern, and where it is.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OneOrMoreCondition GetOneOrMoreCondition()
		{
			string tmp = RemovePlusBinaryFeatures(m_srchItemStr);
			tmp = RemoveDiacriticPlaceholderModifiers(tmp);

			// Check search item
			if (tmp.Contains("+"))
				return OneOrMoreCondition.InSearchItem;

			// Check environment before
			tmp = RemovePlusBinaryFeatures(m_envBeforeStr);
			string[] pieces = tmp.Split("+".ToCharArray());

			if (pieces.Length > 2)
				return OneOrMoreCondition.MoreThanOneInEnvBefore;

			if (pieces.Length > 1 && !tmp.StartsWith("+"))
				return OneOrMoreCondition.NotBeginningOfEnvBefore;

			// Check environment after
			tmp = RemovePlusBinaryFeatures(m_envAfterStr);
			pieces = tmp.Split("+".ToCharArray());

			if (pieces.Length > 2)
				return OneOrMoreCondition.MoreThanOneInEnvAfter;

			if (pieces.Length > 1 && !tmp.EndsWith("+"))
				return OneOrMoreCondition.NotEndOfEnvAfter;

			return OneOrMoreCondition.NoCondition;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get rid of all +binary feature names in the specified string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string RemovePlusBinaryFeatures(string pattern)
		{
			pattern = pattern.ToLower();

			foreach (Feature feature in App.BFeatureCache.Values)
			{
				// Remove those whose short name was specified.
				string ptrnFeature = string.Format("[{0}]", feature.Name.ToLower());
				pattern = pattern.Replace(ptrnFeature, string.Empty);

				// Remove those whose full name was specified.
				ptrnFeature = string.Format("[{0}]", feature.FullName.ToLower());
				pattern = pattern.Replace(ptrnFeature, string.Empty);
			}

			return pattern;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get rid of all the stuff between diacritic placeholder (i.e. dotted circle) and
		/// its enclosing closed square bracket.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string RemoveDiacriticPlaceholderModifiers(string pattern)
		{
			int start = 0;

			while ((start = pattern.IndexOf(App.kDottedCircleC, start)) >= 0)
			{
				int end = pattern.IndexOf(']', start);
				if (end < start)
					break;

				start++;
				pattern = pattern.Remove(start, end - start);
			}

			return pattern;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetPhonesFromMember(PatternGroup grp)
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
						member.MemberType == MemberType.SinglePhone)
					{
						phones.Append(member.Member.Trim());
					}

					//PatternGroupMember member = obj as PatternGroupMember;
					//if (member != null && member.Member != null &&
					//    member.Member.Trim() != string.Empty &&
					//    (member.MemberType == MemberType.SinglePhone ||
					//    member.MemberType == MemberType.IPACharacterRun))
					//{
					//    phones.Append(member.Member.Trim());
					//}
				}
			}

			return phones.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static List<char> GetInvalidCharsFromMember(PatternGroup grp)
		{
			List<char> undefinedChars = new List<char>();

			if (grp != null)
			{
				foreach (object obj in grp.Members)
				{
					if (obj is PatternGroup)
						undefinedChars.AddRange(GetInvalidCharsFromMember(obj as PatternGroup));
					else
					{
						PatternGroupMember member = obj as PatternGroupMember;
						if (member != null)
							undefinedChars.AddRange(member.UndefinedPhoneticChars);
					}
				}

			}
		
			return undefinedChars;
		}

		#region Diacritic Pattern comparer used by pattern group members and pattern groups.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a phone into its base portion and its diacritics and returns a list of
		/// the characters that are not found in the phonetic character inventory, if any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<char> ParsePhone(string phone, out string basePhone, out string diacritics)
		{
			// First, check if the phone is a tone letter.
			if (App.IPASymbolCache.ToneLetterInfo(phone) != null)
			{
				basePhone = phone;
				diacritics = null;
				return null;
			}

			StringBuilder sbBasePhone = new StringBuilder();
			List<char> sbDiacritics = new List<char>(5);
			List<char> undefinedChars = new List<char>();
			bool tiebarFound = false;

			foreach (char c in phone)
			{
				var charInfo = App.IPASymbolCache[c];

				// This should never be null.
				if (charInfo == null || charInfo.IsUndefined)
					undefinedChars.Add(c);
				
				if (charInfo != null)
				{
					// Tie bars are counted as part of the base character.
					if (charInfo.IsBase || c == App.kBottomTieBarC || c == App.kTopTieBarC)
					{
						sbBasePhone.Append(c);
						if (!tiebarFound && (c == App.kBottomTieBarC || c == App.kTopTieBarC))
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
			return (undefinedChars.Count == 0 ? null : undefinedChars);
		}
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified phonetic character array for pattern matches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SearchWord(string phonetic, out int[] result)
		{
			result = new[] { -1, -1 };

			if (App.IPASymbolCache == null)
				return false;

			m_matchIndex = 0;
			return SearchWord(App.Project.PhoneticParser.Parse(phonetic, true), out result);
		}

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
			result = new[] {-1, -1};

			if (m_phones == null)
				return false;

			while (m_matchIndex < m_phones.Length)
			{
				result = new[] { -1, -1 };

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
