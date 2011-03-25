using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public enum GroupType
	{
		And,
		Or,
		Sequential
	}

	public enum EnvironmentType
	{
		Item,
		Before,
		After
	}

	public enum CompareResultType
	{
		NoMatch,
		Match,
		Ignored,
		Error
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class that stores a collection of patterns in a single AND or OR group.
	/// Two examples are: {+front, +voiced} or {a,e,i}
	/// Pattern groups may also contain nested pattern groups.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PatternGroup
	{
		private const int kFirstDiacriticPlaceholderMaker = 1;
	
		private static int s_currDiacriticPlaceholderMaker;
		private static readonly Dictionary<int, string> s_diacriticPlaceholders = new Dictionary<int, string>();

		private string m_cachedSearchWord;
		private string[] m_cachedSearchChars;
		private GroupType m_type = GroupType.And;
		private PatternGroupMember m_currMember;
		private string m_diacriticPattern;
		private readonly EnvironmentType m_envType = EnvironmentType.Item;
		private readonly PatternGroup m_rootGroup;
		private List<string> m_errors;

		// m_members can contain both objects of type PatternGroup and PatternGroupMember.
		private ArrayList m_members;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternGroup(EnvironmentType envType) : this(envType, null)
		{
			m_rootGroup = this;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private PatternGroup(EnvironmentType envType,  PatternGroup rootGroup)
		{
			m_envType = envType;
			m_rootGroup = rootGroup;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of members in the pattern group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ArrayList Members
		{
			get { return m_members; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating what type of group this is: AND, OR or the root group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GroupType GroupType
		{
			get { return m_type; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the group's environment type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public EnvironmentType EnvironmentType
		{
			get { return m_envType; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the diacritic pattern applied to phones that match this group. After a phone
		/// successfully matches this group, it is further checked to see if it matches the
		/// diacritic pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DiacriticPattern
		{
			get {return m_diacriticPattern;}
			internal set {m_diacriticPattern = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the group that is the root of all other groups.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternGroup RootGroup
		{
			get { return m_rootGroup; }
		}

		#endregion

		#region Pattern Parsing Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Perform parsing on the group's search pattern.
		/// </summary>
		///
		/// <example>
		/// 
		/// An example of how patterns are parsed can be illustrated using the following pattern.
		/// "[ [ {a,e} { [+high][dental] } ] [-dorsal] ]"
		/// 
		/// The group and member hierarchy would be as follows:
		/// 
		/// Pattern Group (AND'd): [[{a,e}{[+high][[dental]}][-dorsal]]
		///    |
		///    +-- Pattern Group (AND'd): [{a,e}{[+high][[dental]}]
		///    |     |
		///    |     +-- Pattern Group (OR'd): {a,e}
		///    |     |     |
		///    |     |     +-- Pattern Member: 'a'
		///    |     |     +-- Pattern Member: 'e'
		///    |     |
		///    |     +-- Pattern Group (OR'd): {[+high][dental]}
		///    |           |
		///    |           +-- Pattern Member: [+high]
		///    |           +-- Pattern Member: [dental]
		///    |
		///    +-- Pattern Member: [-dorsal]
		/// 
		/// </example>
		/// ----------------------------------------------------------------------------------------
		public bool Parse(string pattern)
		{
			return Parse(pattern, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Parse(string pattern, List<string> errors)
		{
			m_errors = errors;

			if (m_rootGroup != this)
				m_type = (pattern[0] == '[' ? GroupType.And : GroupType.Or);
			else
			{
				if (m_members != null)
					m_members.Clear();

				m_diacriticPattern = null;
				s_currDiacriticPlaceholderMaker = kFirstDiacriticPlaceholderMaker;
				s_diacriticPlaceholders.Clear();
				if (!PreParseProcessing(ref pattern))
					return false;
			}

			// As long as we're an AND or OR group, we'll need to know the
			// closing bracket or brace to look for as we're parsing the pattern.
			char closeBracket = char.MinValue;
			if (m_type != GroupType.Sequential)
			{
				int i = pattern.Length - 1;
				while (pattern[i] < 32)
					i--;

				closeBracket = pattern[i];
				
				// Toss out the opening bracket or brace since we're done with it.
				pattern = pattern.Substring(1);
			}

			m_members = new ArrayList();

			for (int i = 0; i < pattern.Length; i++)
			{
				// If we've found our final close bracket, then we're done.
				if (m_type != GroupType.Sequential && pattern[i] == closeBracket)
				{
					if (i + 1 < pattern.Length && pattern[i + 1] < 32)
					{
						CloseCurrentMember();
						AddDiacriticPatternToCurrentMember(pattern[++i]);
					}

					break;
				}

				// Are we beginning a sub group?
				if (pattern[i] == '[' || pattern[i] == '{' || pattern[i] == '(')
				{
					BeginSubGroup(pattern, ref i);
					continue;
				}

				if (pattern[i] < 32)
				{
					AddDiacriticPatternToCurrentMember(pattern[i]);
					continue;
				}

				if (pattern[i] == '%')
				{
					CloseCurrentMember();
					continue;
				}

				// Are we at the end of a group member?
				if (pattern[i] == '$' || pattern[i] == ',' || pattern[i] == ')')
				{
					// We've reached the end of a PatternGroupMember so close it out.
					CloseCurrentMember();
				}
				else
				{
					if (m_currMember == null)
						m_currMember = new PatternGroupMember();

					m_currMember.AddToMember(pattern[i]);
				}
			}

			// Make sure the last member of the pattern is accounted
			// for and added to the collection of members.
			CloseCurrentMember();

			if (m_members.Count == 0)
				m_members = null;
			else if (m_members.Count == 1)
			{
				// Because of the way diacritic placeholders are represented in patterns,
				// sometimes patterns that include them will be parsed so the group they're
				// in may only contain one member but be considered an And or Or group when
				// it should be a sequential group.
				m_type = GroupType.Sequential;
			}

			if (m_rootGroup == this && m_type == GroupType.Sequential)
			    CollapsNestedSequentialGroups(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Usually sequential groups don't contain other sequential groups unless some
		/// sequences are delimited by parenthese. In that case, then the members of a sub
		/// sequential group are pulled up so they are sibling members of the parent
		/// sequential group. This method accomplishes that.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void CollapsNestedSequentialGroups(PatternGroup group)
		{
			for (int i = 0; i < group.Members.Count; i++)
			{
				PatternGroup subGroup = group.Members[i] as PatternGroup;

				if (subGroup != null && subGroup.GroupType == GroupType.Sequential)
				{
					for (int j = subGroup.Members.Count - 1; j >= 0; j--)
					{
						if (subGroup.Members[j] is PatternGroup &&
							((PatternGroup)subGroup.Members[j]).GroupType == GroupType.Sequential)
						{
							CollapsNestedSequentialGroups((PatternGroup)subGroup.Members[j]);
						}

						group.Members.Insert(i + 1, subGroup.Members[j]);
					}

					group.Members.RemoveAt(i);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Close out the current member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CloseCurrentMember()
		{
			if (m_currMember == null)
				return;

			// When CloseMember returns a collection of members, it's because the
			// member contained one or more phones. Each phone is returned in it's
			// own member, with m_currMember being the first in the collection. If
			// the member is of type feature or class, etc., then null is returned.
			PatternGroupMember[] members = m_currMember.CloseMember();

			if (members == null)
				m_members.Add(m_currMember);
			else
			{
				// At this point, we know we just closed a member that was a run of phonetic
				// characters. When that happens, CloseMember returns a collection of members,
				// one for each of the phones found in the phonetic character run. Therefore,
				// each of those phones needs to be added to our member collection as a single
				// phone member. The first member in the collection will always be m_currMember.
				foreach (var member in members)
				{
					if (!string.IsNullOrEmpty(member.DiacriticPattern) &&
						(string.IsNullOrEmpty(member.Member)))
					{
						LogError(GetErrorMsg("MisplacedDiacriticErrorMsg"));
						return;
					}

					m_members.Add(member);
				}
			}
				
			m_currMember = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes pending group members and begins a new sub (or nested) group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BeginSubGroup(string pattern, ref int i)
		{
			// Close any pending PatternGroupMember being accumulated.
			CloseCurrentMember();

			// We've run into a group nested in the current group. Therefore,
			// extract its pattern, creating a sub group for it and add it
			// to our collection of members.
			string subGroupPattern = ExtractSubGroup(pattern, ref i);
			if (subGroupPattern != null)
			{
				PatternGroup subGroup = new PatternGroup(m_envType, m_rootGroup);
				if (!subGroup.Parse(subGroupPattern))
				{
					// Should anything be done here?
				}

				if (subGroupPattern[0] == '(')
					subGroup.m_type = GroupType.Sequential;

				m_members.Add(subGroup);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Starts at the specified index and extracts a sub string of a pattern that is
		/// assumed to be an embedded pattern group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string ExtractSubGroup(string pattern, ref int i)
		{
			int openBracketCount = 0;
			
			// The assumption here is that we're pointing to an opening bracket or brace.
			// Therefore, get the closed counterpart.
			char openBracket = pattern[i];
			char closeBracket = (openBracket == '[' ? ']' : openBracket == '{' ? '}' : ')');
			int start = i;

			while (i < pattern.Length)
			{
				if (pattern[i] == openBracket)
					openBracketCount++;

				if (pattern[i] == closeBracket)
				{
					openBracketCount--;

					if (openBracketCount == 0)
						return pattern.Substring(start, i - start + 1);
				}

				i++;
			}

			LogError();

			// Should never get here.
			i = start;
			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method updates the current member to include the specified diacritic pattern.
		/// </summary>
		/// <remarks>
		/// When a diacritic placeholder cluster is read, it cannot be a member unto itself.
		/// It is assumed it modifies the current member.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		private void AddDiacriticPatternToCurrentMember(int diacriticClusterKey)
		{
			if (m_members.Count == 0 && m_currMember == null)
			{
				LogError(GetErrorMsg("IrregularDiacriticPlaceholderErrorMsg"));
				return;
			}

			string diacriticPattern;
			if (!s_diacriticPlaceholders.TryGetValue(diacriticClusterKey, out diacriticPattern))
			{
				LogError();
				return;
			}

			// Modify the current member or group with the diacritic pattern.
			if (m_currMember != null)
				m_currMember.DiacriticPattern = diacriticPattern;
			else
			{
				m_diacriticPattern = diacriticPattern;
				PropogateGroupDiacriticPatternToMembers(this, diacriticPattern);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycles through the members of the specified group and applies the group's diacritic
		/// pattern to group's members and then clears the group's diacritic pattern. When
		/// a member of the group is another group, then this method is called recursively
		/// to deal with that group's members.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void PropogateGroupDiacriticPatternToMembers(PatternGroup group,
			string diacriticPattern)
		{
			Debug.Assert(group != null);

			foreach (object pgMember in group.Members)
			{
				if (pgMember is PatternGroupMember)
				{
					PatternGroupMember member = pgMember as PatternGroupMember;
					member.DiacriticPattern =
						MergeDiacriticPatterns(member.DiacriticPattern, diacriticPattern);
				}
				else if (pgMember is PatternGroup)
				{
					PatternGroup member = pgMember as PatternGroup;
					string mergedDiacriticPattern =
						MergeDiacriticPatterns(member.DiacriticPattern, diacriticPattern);

					PropogateGroupDiacriticPatternToMembers(member, mergedDiacriticPattern);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string MergeDiacriticPatterns(string ptrn1, string ptrn2)
		{
			if (string.IsNullOrEmpty(ptrn1))
				return ptrn2;

			if (string.IsNullOrEmpty(ptrn2))
				return ptrn1;

			bool containsZorM = (ptrn1.IndexOf('*') >= 0) || (ptrn2.IndexOf('*') >= 0);
			bool containsOorM = (ptrn1.IndexOf('+') >= 0) || (ptrn2.IndexOf('+') >= 0);

			string newPattern = ptrn1 + ptrn2;
			newPattern = "X" + newPattern.Replace("*", string.Empty);
			newPattern = newPattern.Replace("+", string.Empty);
			newPattern = newPattern.Normalize(NormalizationForm.FormD);
			newPattern = newPattern.Replace("X", string.Empty);

			if (containsZorM)
				newPattern += "*";
			else if (containsOorM)
				newPattern += "+";

			return newPattern;
		}

		#endregion

		#region Pattern Verification and pre-parse processing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs misc. preprocessing and verification of the pattern, before the pattern
		/// is parsed into groups and group members.
		/// 
		/// | - Opens a member
		/// $ - Closes a member
		/// C - Consonant member
		/// V - Vowel member
		/// [ - Opens an And group
		/// ] - Closes an And group
		/// { - Opens an Or group
		/// } - Closes an Or group
		/// 0 - Temporarily, zero or more diacritics wildcard
		/// 1 - Temporarily, one or more diacritics wildcard
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool PreParseProcessing(ref string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
			{
				LogError(GetErrorMsg("PatternEmptyErrorMsg"));
				return false;
			}

			pattern = pattern.Replace("], [", "],[");

			if (!VerifyNoIllegalSpaces(pattern))
			{
				LogError(GetErrorMsg("PatternContainsSpacesErrorMsg"));
				return false;
			}

			pattern = pattern.Replace("[c]", "[C]");
			pattern = pattern.Replace("[v]", "[V]");

			// Check for too many zero-or-more symbols
			if (pattern.IndexOf("**") >= 0)
			{
				LogError(GetErrorMsg("TooManyZeroOrMoreErrorMsg"));
				return false;
			}

			// Check for too many one-or-more symbols
			if (pattern.IndexOf("++") >= 0)
			{
				LogError(GetErrorMsg("TooManyOneOrMoreErrorMsg"));
				return false;
			}

			if (!VerifyMatchingBrackets(pattern, '[', ']'))
			{
				LogError(GetErrorMsg("MismatchedBracketsErrorMsg"));
				return false;
			}

			if (!VerifyMatchingBrackets(pattern, '{', '}'))
			{
				LogError(GetErrorMsg("MismatchedBracesErrorMsg"));
				return false;
			}

			if (!VerifyBracketOrdering(pattern))
			{
				LogError(GetErrorMsg("MismatchedBracketsOrBracesErrorMsg"));
				return false;
			}

			if (!VerifyDiacriticPlaceholderCluster(ref pattern))
			{
				LogError(GetErrorMsg("IrregularDiacriticPlaceholderErrorMsg"));
				return false;
			}

			if (!VerifyZeroOneOrMorePlacement(ref pattern))
				return false;

			// TODO: Other verifications to perform
			// More than 2 # in an environment.
			// More than 2 * in an environment.
			// Char. modifiers before a dotted circle but within the square brackets.
			// [[C][C][o^h] or [[V][V][o^h] don't make sense to have double con. or vowel in AND group.
			// For that matter, things like this don't make sense [[C][p]] or [[V][a]] or
			// Characters in square brakets don't make sense. Ex. [a,b] - a match cannot be both an 'a' AND 'b'

			//pattern = DelimitOrGroupMembers(pattern);

			//pattern = pattern.Replace("#", string.Empty);
			pattern = pattern.Replace(",,", ",");
			//pattern = pattern.Replace("],", "]");
			//pattern = pattern.Replace("},", "}");
			pattern = pattern.Replace(",+", "+");
			pattern = pattern.Replace(",-", "-");
			pattern = FFNormalizer.Normalize(pattern);
			pattern = DelimitMembers(pattern);
			pattern = pattern.Replace("#", " ");

			if (m_rootGroup == this)
				m_type = GetRootGroupType(pattern);
			else
			{
				char closeBracket = pattern[pattern.Length - 1];

				// Since we're not the root group, we'd better begin
				// and end with a set of braces or square brackets.
				if ((pattern[0] == '[' && closeBracket != ']') ||
					(pattern[0] == '{' && closeBracket != '}') ||
					(pattern[0] != '[' && pattern[0] != '{') ||
					(closeBracket != ']' && closeBracket != '}'))
				{
					LogError(GetErrorMsg("MismatchedBracketsOrBracesErrorMsg"));
					return false;
				}
			}

			return true;
		}

		// ------------------------------------------------------------------------------------
		// <summary>
		// This method surrounds all sequences in OR groups with parentheses. Example: the
		// pattern {ab,[C][V],[V]} is converted to {(ab),([C][V]),[V]}.
		// </summary>
		// ------------------------------------------------------------------------------------
		//private static string DelimitOrGroupMembers(string pattern)
		//{
		//    StringBuilder tmpPattern = new StringBuilder();
		//    Stack<char> braceStack = new Stack<char>();

		//    foreach (char c in pattern)
		//    {
		//        if (braceStack.Count > 0 && c == ',')
		//            tmpPattern.Append("),(");
		//        else if (c == '{')
		//        {
		//            tmpPattern.Append("{(");
		//            braceStack.Push(c);
		//        }
		//        else if (c == '}')
		//        {
		//            tmpPattern.Append(")}");
		//            braceStack.Pop();
		//        }
		//        else
		//            tmpPattern.Append(c);
		//    }

		//    tmpPattern = tmpPattern.Replace("((", "(");
		//    tmpPattern = tmpPattern.Replace("))", ")");
		//    tmpPattern = tmpPattern.Replace("{({", "{{");
		//    tmpPattern = tmpPattern.Replace("})}", "}}");
		//    tmpPattern = tmpPattern.Replace("({", "{");
		//    tmpPattern = tmpPattern.Replace("})", "}");
		//    tmpPattern = tmpPattern.Replace("([V])", "[V]");
		//    tmpPattern = tmpPattern.Replace("([C])", "[C]");
			
		//    return tmpPattern.ToString();
		//}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified pattern for spaces. If any are found, the pattern is scanned
		/// for an open bracket before and a closed bracket after the space. What is between
		/// is assumed to be a feature name and is then checked against the articulatory and
		/// binary feature caches to see if the feature name is valid. If not, the space is
		/// illegal in the pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool VerifyNoIllegalSpaces(string pattern)
		{
			int i = 0;
			while (i < pattern.Length && (i = pattern.IndexOf(' ', i)) >= 0)
			{
				int open = i++;
				
				// Search backward for the first opening bracket.
				while (open >= 0 && pattern[open] != '[') open--;

				// Search forward for the next closing bracket.
				int closed = pattern.IndexOf(']', i);

				if (open < 0 || closed < 0)
					return false;

				if (closed > open)
				{
					string text = pattern.Substring(open + 1, closed - open - 1);
					if (App.AFeatureCache[text] == null &&
						App.BFeatureCache[text] == null)
					{
						return false;
					}
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that all the open brackets or braces in a pattern have matching closing
		/// counterparts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool VerifyMatchingBrackets(string pattern, char open, char close)
		{
			int bracketOpenCount = 0;
			int bracketCloseCount = 0;

			for (int i = 0; i < pattern.Length; i++)
			{
				if (pattern[i] == open)
					bracketOpenCount++;
				if (pattern[i] == close)
					bracketCloseCount++;
			}

			return (bracketCloseCount == bracketOpenCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that open brackets don't have the wrong type of closing brackets.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool VerifyBracketOrdering(IEnumerable<char> pattern)
		{
			Stack<char> bracketStack = new Stack<char>();
			
			foreach (char c in pattern)
			{
				if (c == '[' || c == '{')
					bracketStack.Push(c);
				else if (c == ']' || c == '}')
				{
					char expected = (c == ']' ? '[' : '{');
					if (expected != bracketStack.Pop())
						return false;
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if the pattern contains any diacritic placeholder cluster and verifies
		/// some things about it. When a diacritic placeholder is found, the entire cluster
		/// in the pattern (which includes the square brackets and everything inside them)
		/// is replace by a single character between 1 and 31 (inclusively). Those characters
		/// serve as markers where a diacritic place holder should go and it also serves as
		/// a key into a hash table containing all the clusters of diacritics found in 
		/// all the diacritic placeholders for the full pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyDiacriticPlaceholderCluster(ref string pattern)
		{
			int i = 0;
			while ((i = pattern.IndexOf(App.kDottedCircleC, i)) >= 0)
			{
				// The dotted circle may not be at the extremes of the pattern and it
				// must be preceeded by an open square bracket but must not be the only
				// thing between its opening and closing square brackets. 
				if (i == 0 || i == pattern.Length - 1 || pattern[i - 1] != '[' || pattern[i + 1] == ']')
					return false;

				bool foundOneOrMoreSymbol = false;
				bool foundZeroOrMoreSymbol = false;
				StringBuilder bldr = new StringBuilder();
				int idxCloseBracket = i;

				// Extract the cluster.
				while (pattern[idxCloseBracket] != ']' && idxCloseBracket < pattern.Length)
				{
					if (pattern[idxCloseBracket] == '*')
					{
						if (!foundOneOrMoreSymbol && !foundZeroOrMoreSymbol)
							bldr.Append(pattern[idxCloseBracket]);
						
						idxCloseBracket++;
						foundZeroOrMoreSymbol = true;
					}
					else if (pattern[idxCloseBracket] == '+')
					{
						if (!foundOneOrMoreSymbol && !foundZeroOrMoreSymbol)
							bldr.Append(pattern[idxCloseBracket]);
	
						idxCloseBracket++;
						foundOneOrMoreSymbol = true;
					}
					else
						bldr.Append(pattern[idxCloseBracket++]);
				}

				// The cluster should end in a closed bracket.
				if (idxCloseBracket == pattern.Length || pattern[idxCloseBracket] != ']')
					return false;

				string cluster = bldr.ToString();
				cluster = cluster.Normalize(NormalizationForm.FormD);
				cluster = cluster.Replace(App.kDottedCircle, string.Empty);

				if (foundZeroOrMoreSymbol && foundOneOrMoreSymbol)
					LogError(GetErrorMsg("ZeroAndOneOrMoreFoundErrorMsg"));

				// This should never happen.
				if (s_currDiacriticPlaceholderMaker == 31)
				{
					LogError(GetErrorMsg("TooManyDiacriticPlaceholdersErrorMsg"));
					pattern = pattern.Substring(0, i - 1) + pattern.Substring(idxCloseBracket + 1);
					continue;
				}

				// Replace the diacritic cluster in the pattern with a marker.
				pattern = pattern.Substring(0, i - 1) + ((char)s_currDiacriticPlaceholderMaker) +
					pattern.Substring(idxCloseBracket + 1);

				i = 0;

				// Save the cleaned-up cluster.
				s_diacriticPlaceholders[s_currDiacriticPlaceholderMaker++] = cluster;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool VerifyZeroOneOrMorePlacement(ref string pattern)
		{
			// Remove any pluses that maybe attached to binary features so
			// they're not confused with a plus that's for one or more characters.
			string tmpPattern = pattern.Replace("[+", "[=");

			// Check how many asterisks are in the pattern. Only one is allowed.
			string[] splits = tmpPattern.Split("*".ToCharArray());
			if (splits.Length > 2)
			{
				LogError(GetErrorMsg("TooManyZeroOrMoreErrorMsg"));
				return false;
			}

			// Check how many pluses are in the pattern. Only one is allowed.
			splits = tmpPattern.Split("+".ToCharArray());
			if (splits.Length > 2)
			{
				LogError(GetErrorMsg("TooManyOneOrMoreErrorMsg"));
				return false;
			}
			
			int z = tmpPattern.IndexOf('*');
			int o = tmpPattern.IndexOf('+');

			// Can't have both in the same pattern.
			if (z >= 0 && o >= 0)
			{
				LogError(GetErrorMsg("ZeroAndOneOrMoreFoundErrorMsg"));
				return false;
			}

			// Niether one are valid in the search item.
			if (m_envType == EnvironmentType.Item && (z >= 0 || o >= 0))
			{
				LogError(GetErrorMsg("ZeroOneOrMoreFoundInSrchItemErrorMsg"));
				return false;
			}

			if (z >= 0)
			{
				if (m_envType == EnvironmentType.Before)
				{
					// Must be first item in pattern.
					if (z != 0)
					{
						LogError(GetErrorMsg("ZeroOrMoreBeginningErrorMsg"));
						return false;
					}

					tmpPattern = "%*$" + tmpPattern.Substring(1);
				}
				else
				{
					// Must be last item in pattern
					if (z != tmpPattern.Length - 1)
					{
						LogError(GetErrorMsg("ZeroOrMoreEndingErrorMsg"));
						return false;
					}

					tmpPattern = tmpPattern.TrimEnd("*".ToCharArray());
					tmpPattern += "%*$";
				}
			}

			if (o >= 0)
			{
				if (m_envType == EnvironmentType.Before)
				{
					// Must be first item in pattern.
					if (o != 0)
					{
						LogError(GetErrorMsg("OneOrMoreBeginningErrorMsg"));
						return false;
					}

					tmpPattern = "%+$" + tmpPattern.Substring(1);
				}
				else
				{
					// Must be last item in pattern
					if (o != tmpPattern.Length - 1)
					{
						LogError(GetErrorMsg("OneOrMoreEndingErrorMsg"));
						return false;
					}

					tmpPattern = tmpPattern.TrimEnd("+".ToCharArray());
					tmpPattern += "%+$";
				}
			}

			// Put back any pluses that belong to binary features.
			pattern = tmpPattern.Replace("[=", "[+");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes extraneous square brackets from the pattern and puts a comma at the end of
		/// feature names (unless the name is terminated by a brace). This method assumes
		/// extra spaces and all commas (except those between IPA characters) should have
		/// already been stripped from the pattern as well as a verification that all opening
		/// brackets and braces have matching closes. So, for example, when this method
		/// receives a pattern stripped of those things, like
		/// "{[[+high][+cons]][[+vcd][+cons]]}" this method will return
		/// "{[+high,+cons,][+vcd,+cons,]}"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string DelimitMembers(string pattern)
		{
			// First, merge any diacritic pattern in square brackets
			// with the preceeding feature (C and V included).
			//string replace = string.Format("][{0}", PaApp.kDottedCircle);
			//string tmpPattern = pattern.Replace(replace, PaApp.kDottedCircle);
			string tmpPattern = pattern;
			StringBuilder modifiedPtrn = new StringBuilder(tmpPattern);

			CommaDelimitBracketedMember("[+", tmpPattern, ref modifiedPtrn);
			CommaDelimitBracketedMember("[-", tmpPattern, ref modifiedPtrn);
			CommaDelimitBracketedMember("[C", tmpPattern, ref modifiedPtrn);
			CommaDelimitBracketedMember("[V", tmpPattern, ref modifiedPtrn);
			DelimitArticulatoryFeatures(tmpPattern, ref modifiedPtrn);

			string finalModified = modifiedPtrn.ToString();
			finalModified = finalModified.Replace("$,%", "$%");
			finalModified = finalModified.Replace("],[", "][");
			finalModified = finalModified.Replace("},{", "}{");
			return finalModified;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Looks for articulatory features in a pattern and replaces their surrounding
		/// square brackets with commas.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DelimitArticulatoryFeatures(string pattern, ref StringBuilder modifiedPtrn)
		{
			string tmpPattern = pattern.ToLower();

			foreach (KeyValuePair<string, Feature> info in App.AFeatureCache)
			{
				string feature = "[" + info.Value.Name.ToLower();
				if (!CommaDelimitBracketedMember(feature, tmpPattern, ref modifiedPtrn))
					return;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Looks for the specified substring (which is assumed to begin with an open square
		/// bracket) within the pattern and replaces the open bracket and its associated
		/// closing bracket with commas. If the substring is found but the associated closing
		/// bracket is not found, then false is returned. That's an error condition.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CommaDelimitBracketedMember(string lookFor, string pattern,
			ref StringBuilder modifiedPtrn)
		{
			int i = 0;
			bool foundDiacriticPlaceholder = false;

			while ((i = pattern.IndexOf(lookFor, i, System.StringComparison.Ordinal)) >= 0)
			{
				int closeIndex = i;
				
				// Once we find what we're looking for, step through the following 
				// characters looking for the end of the feature.
				while (closeIndex < pattern.Length && pattern[closeIndex] != ']')
				{
					// Check if we've run into a diacritic pattern cluster along the way.
					if (!foundDiacriticPlaceholder && pattern[closeIndex] == App.kDottedCircleC)
						foundDiacriticPlaceholder = true;

					closeIndex++;
				}

				// We had better find the close bracket for the member.
				if (closeIndex == pattern.Length)
				{
					LogError(GetErrorMsg("MismatchedBracketsErrorMsg"));
					return false;
				}

				if (foundDiacriticPlaceholder)
				{
					// Make sure the diacritic cluster is closed. This test is probably somewhat
					// uncessary due the fact that it should have already been done in a verify
					// method. But, just in case...
					if (i == 0 || pattern[i - 1] != '[' || closeIndex >= pattern.Length - 1 ||
						pattern[closeIndex + 1] != ']')
					{
						LogError(GetErrorMsg("MismatchedBracketsErrorMsg"));
						return false;
					}
				
					modifiedPtrn[i - 1] = ' ';
					modifiedPtrn[closeIndex + 1] = ' ';
				}

				modifiedPtrn[closeIndex] = '$';
				modifiedPtrn[i] = '%';
				i++;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the type that should be assigned the root group. For example, the following
		/// pattern yields a group type of sequential: "[[+high][+con]]abc[dental]"
		/// On the other hand, one may have a pattern like the following that would yield
		/// a group type of AND: "[[+high][+con]{a,e}]" since all members in the group should
		/// be AND'd together.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static GroupType GetRootGroupType(string pattern)
		{
			// Because diacritic placeholder groups don't count as pattern groups,
			// take them out before trying to determine the root group type.
			StringBuilder modifiedPattern = new StringBuilder();
			for (int i = 0; i < pattern.Length; i++)
			{
				if (pattern[i] >= 32)
					modifiedPattern.Append(pattern[i]);
			}

			pattern = modifiedPattern.ToString();

			// If the pattern begins with or ends with bracketing,
			// then we know we have a sequential group.
			if (pattern.Length == 0 || (pattern[0] != '[' && pattern[0] != '{') ||
				(!pattern.EndsWith("]") && !pattern.EndsWith("}")))
			{
				return GroupType.Sequential;
			}

			// At this point we know there is bracketing at the pattern's extremes, but that
			// doesn't mean the entire pattern is enclosed in the first and last bracket.
			// Therefore, we need to step into the pattern and see if that's the case.
			
			int bracketCount = 0;
			char firstBracket = (char)0;

			for (int i = 0; i < pattern.Length; i++)
			{
				if (pattern[i] == ']' || pattern[i] == '}')
					bracketCount--;
				else if (pattern[i] == '[' || pattern[i] == '{')
				{
					bracketCount++;
					if (firstBracket == 0)
						firstBracket = pattern[i];
				}

				// If we've already hit an open bracket and our bracket count is back down
				// to zero, then we're finished checking. In that case, when i is not at
				// the end of the pattern we know we're looking at a sequential pattern.
				if (firstBracket != 0 && bracketCount == 0)
				{
					if (i < pattern.Length - 1)
						return GroupType.Sequential;

					break;
				}
			}

			// At this point, we know we're looking at a non sequential group,
			// therefore determine what type it is by looking at the first bracket found.
			return (firstBracket == '[' ? GroupType.And : GroupType.Or);
		}

		#region Error logging
		/// ------------------------------------------------------------------------------------
		private static string GetErrorMsg(string id)
		{
			switch (id)
			{
				case "TooManyOneOrMoreErrorMsg" :
					return App.GetString("TooManyOneOrMoreErrorMsg", "Too many one-or-more symbols (+) found in the {0}. Only one is allowed.");
				
				case "PatternContainsSpacesErrorMsg" :
					return App.GetString("PatternContainsSpacesErrorMsg", "There were spaces found in the {0}. Use '#' instead.");
				
				case "PatternEmptyErrorMsg" :
					return App.GetString("PatternEmptyErrorMsg", "The {0} is empty.");
				
				case "MisplacedDiacriticErrorMsg" :
					return App.GetString("MisplacedDiacriticErrorMsg", "Misplaced diacritic in the {0}.");
				
				case "IrregularDiacriticPlaceholderErrorMsg" :
					return App.GetString("IrregularDiacriticPlaceholderErrorMsg", "Irregular diacritic placeholder syntax in {0}.");

				case "TooManyZeroOrMoreErrorMsg":
					return App.GetString("TooManyZeroOrMoreErrorMsg", "Too many zero-or-more symbols (*) found in the {0}. Only one is allowed.");

				case "MismatchedBracketsErrorMsg":
					return App.GetString("MismatchedBracketsErrorMsg", "Mismatched brackets found in the {0}.");

				case "MismatchedBracesErrorMsg":
					return App.GetString("MismatchedBracesErrorMsg", "Mismatched braces found in the {0}.");

				case "MismatchedBracketsOrBracesErrorMsg":
					return App.GetString("MismatchedBracketsOrBracesErrorMsg", "Mismatched brackets or braces found in the {0}.");

				case "ZeroAndOneOrMoreFoundErrorMsg":
					return App.GetString("ZeroAndOneOrMoreFoundErrorMsg", "The zero-or-more symbol (*) cannot be in the {0} with the one-or-more symbol (+).");

				case "TooManyDiacriticPlaceholdersErrorMsg":
					return App.GetString("TooManyDiacriticPlaceholdersErrorMsg", "There are too many diacritic placeholders between a single set of brackets in the {0}.");

				case "ZeroOneOrMoreFoundInSrchItemErrorMsg":
					return App.GetString("ZeroOneOrMoreFoundInSrchItemErrorMsg", "The zero-or-more (*) and one-or-more (+) symbols are not allowed in the search item.");

				case "ZeroOrMoreBeginningErrorMsg":
					return App.GetString("ZeroOrMoreBeginningErrorMsg", "When the zero-or-more symbol (*) is present in the preceding environment, it must be at the beginning.");

				case "ZeroOrMoreEndingErrorMsg":
					return App.GetString("ZeroOrMoreEndingErrorMsg", "When the zero-or-more symbol (*) is present in the following environment, it must be at the end.");

				case "OneOrMoreBeginningErrorMsg":
					return App.GetString("OneOrMoreBeginningErrorMsg", "When the one-or-more symbol (+) is present in the preceding environment, it must be  at the beginning.");

				case "OneOrMoreEndingErrorMsg":
					return App.GetString("OneOrMoreEndingErrorMsg", "When the one-or-more symbol (+) is present in the following environment, it must be  at the end.");
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private void LogError()
		{
			LogError(SearchEngine.GetSyntaxErrorMsg());
		}

		/// ------------------------------------------------------------------------------------
		private void LogError(string msg)
		{
			if (m_rootGroup == null || msg == null)
				return;

			var envType = SearchEngine.GetEnvironmentTypeString(m_rootGroup.m_envType);

			if (m_rootGroup.m_errors == null)
				m_rootGroup.m_errors = new List<string>();

			m_rootGroup.m_errors.Add(string.Format(msg, envType));
		}

		#endregion

		#endregion

		#region ToString method
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Return a linquistically appropriate version of the pattern group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			if (m_members == null)
				return string.Empty;
			
			StringBuilder patternGroup = new StringBuilder();

			foreach (object member in m_members)
			{
				patternGroup.Append(member.ToString());
				if (m_type == GroupType.Or)
					patternGroup.Append(',');
			}

			// Remove the trailing comma if one got added.
			if (patternGroup.Length > 0 && patternGroup[patternGroup.Length - 1] == ',')
				patternGroup.Remove(patternGroup.Length - 1, 1);

			string pattern = patternGroup.ToString();

			// Surrounded with the appropriate bracketing if necessary.
			if (m_type == GroupType.And)
				pattern = "[" + pattern + "]";
			else if (m_type == GroupType.Or)
				pattern = "{" + pattern + "}";

			return pattern.Replace(' ', '#');
		}

		#endregion

		#region Searching methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This overload should only be used when searching the Before and After environments
		/// since returning results other than a flag indicating success or not is irrelevant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Search(string eticWord, int startIndex)
		{
			int[] results;
			return Search(eticWord, startIndex, out results);
		}

		/// ------------------------------------------------------------------------------------
		public bool Search(string eticWord, int startIndex, out int[] results)
		{
			if (eticWord != m_cachedSearchWord)
			{
				m_cachedSearchWord = eticWord;
				m_cachedSearchChars = App.Project.PhoneticParser.Parse(eticWord, false);
			}

			// TODO: For this overload of Search, the index in results should probably
			// be a codepoint offset rather than a phone offset. However, that may mess up
			// the index into the cached word... or not?
			return Search(m_cachedSearchChars, startIndex, out results);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search the array of characters (which is supposed to be the phonetic characters
		/// contained in a word) for a match in the pattern. startIndex is where searching
		/// begins. (This overload should only be used when searching the Before and After
		/// environments since returning results other than a flag indicating success or not
		/// is irrelevant.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Search(string[] phones, int startIndex)
		{
			int[] results;
			return Search(phones, startIndex, out results);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search the array of characters (which is supposed to be the phonetic characters
		/// contained in a word) for a match in the pattern. startIndex is where searching
		/// begins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Search(string[] phones, int startIndex, out int[] results)
		{
			results = new[] {-1, -1};

			if (phones == null)
			{
				// TODO: log error
				return false;
			}

			// This special case handles word intial/final matches when the start
			// index for the search is beyond the bounds of the transcription and
			// we're searching for a match on the environment before or after and
			// the before or after environment pattern contains only one member,
			// which is the zero or more phones symbol.
			if (startIndex < 0 || startIndex >= phones.Length)
			{
				if (m_members != null && m_members.Count == 1 &&
					(m_envType == EnvironmentType.Before || m_envType == EnvironmentType.After))
				{
					PatternGroupMember member = m_members[0] as PatternGroupMember;
					if (member != null && member.Member == "*")
						return true;
				}

				// TODO log error
				return false;
			}

			// When this group is sequential, it means it's made up of one
			// or more other groups and/or IPA character runs.
			if (m_type == GroupType.Sequential)
				return SearchSequentially(phones, startIndex, ref results);

			int inc = (m_envType == EnvironmentType.Before ? -1 : 1);

			// At this point, we know this group is either and And or Or group. Though
			// it may contain decendent groups, it only contains one child group.
			for (int i = startIndex; i < phones.Length && i >= 0; i += inc)
			{
				if (phones[i] == string.Empty)
					continue;

				int matchLength;
				CompareResultType compareResult = SearchGroup(phones, ref i, out matchLength);
				
				if (compareResult == CompareResultType.Ignored)
					continue;

				if (compareResult != CompareResultType.NoMatch)
				{
					if (results[0] == -1 || results[1] == -1)
					{
						// Return where the match was found.
						results[0] = i;
						results[1] = matchLength;
					}
					
					return true;
				}

				// If we didn't get a match on the character pointed to by startIndex and
				// we're searching in the environment after or the environment before, it
				// means we've failed so don't bother going on.
				if (m_envType != EnvironmentType.Item)
					return false;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches through a sequential collection of pattern members and phones, to
		/// determine whether or not a contiguous group of phones matches the full pattern
		/// of an environment or search item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool SearchSequentially(string[] phones, int startIndex, ref int[] results)
		{
			switch (m_envType)
			{
				case EnvironmentType.Before:
					return SearchSequentiallyForEnvBefore(phones, startIndex, ref results);

				case EnvironmentType.After:
					return SearchSequentiallyForEnvAfter(phones, startIndex, ref results);

				case EnvironmentType.Item:
					return SearchSequentiallyForSrchItem(phones, startIndex, ref results);
			
				default:
					break;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches through a sequential collection of pattern members and phones, to
		/// determine whether or not a run of phones matches the search item portion of the
		/// full pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool SearchSequentiallyForSrchItem(string[] phones, int startIndex, ref int[] results)
		{
			CompareResultType compareResult = CompareResultType.NoMatch;
			PatternGroupMember member = null;
			int ip = startIndex;				// Index into the collection of phones
			int im = 0;							// Index into the collection of members

			while (m_members != null && im < m_members.Count)
			{
				member = m_members[im] as PatternGroupMember;

				// Break out of the loop if we've run out of phones.
				if (ip == phones.Length)
					break;

				// Check if the phone is ignored, making sure the current member is not in
				// the ignored list. If the current member is in the ignored list, then
				// don't ignore it because it has been explicitly included in the pattern.
				if (SearchEngine.IgnoredPhones.Contains(phones[ip]) && member != null &&
					!SearchEngine.IgnoredPhones.Contains(member.Member))
				{
					ip++;
					continue;
				}

				// Check for a match. If member is null it means the current
				// member is a PatternGroup, not a PatternGroupMember.
				compareResult = (member != null ? member.ContainsMatch(phones[ip]) :
					((PatternGroup)m_members[im]).SearchGroup(phones, ref ip));

				switch (compareResult)
				{
					case CompareResultType.Ignored:
						break;

					case CompareResultType.Match:
						// Stored the index of the matched phone, if it hasn't already been done.
						if (results[0] == -1)
							results[0] = ip;

						im++;
						break;

					default:
						// We've failed so go back to where we found a previous match.
						if (results[0] > -1)
							ip = results[0];

						im = 0;
						results[0] = -1;
						break;
				}

				// Point to next phone.
				ip++;
			}

			if (compareResult == CompareResultType.Match)
			{
				// If we ended up with a match after coming to the end of the phones to
				// search, but before getting to the end of the search pattern members
				// and the last member in the pattern is not a word boundary, then we
				// really don't have a match. Therefore, return false.
				if (ip == phones.Length && im < m_members.Count)
				{
					if (member == null || member.Member != " " || im < m_members.Count - 1)
					{
						results[0] = results[1] = -1;
						return false;
					}
				}

				// Save the number of phones that matched.
				if (results[0] != -1)
					results[1] = ip - results[0];
			}

			return (compareResult == CompareResultType.Match);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches through a sequential collection of pattern members and phones, to
		/// determine whether or not a run of phones matches the environment before portion
		/// of the full pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool SearchSequentiallyForEnvBefore(string[] phones, int startIndex, ref int[] results)
		{
			CompareResultType compareResult = CompareResultType.NoMatch;
			PatternGroupMember member = null;
			int ip = startIndex;				// Index into the collection of phones
			int im = m_members.Count - 1;		// Index into the collection of members

			while (m_members != null && im >= 0)
			{
				member = m_members[im] as PatternGroupMember;

				// Check if the current member is a zero/one or more member.
				if (member != null)
				{
					if (member.MemberType == MemberType.ZeroOrMore)
						return true;

					if (member.MemberType == MemberType.OneOrMore)
						return CheckForOneOrMorePhonesBefore(ip, phones);
				}

				// Break out of the loop if we've run past the beginning of the phones.
				if (ip < 0)
					break;

				// Check if the phone is ignored, making sure the current member is not in
				// the ignored list. If the current member is in the ignored list, then
				// don't ignore it because it has been explicitly included in the pattern.
				if (SearchEngine.IgnoredPhones.Contains(phones[ip]) && member != null &&
					!SearchEngine.IgnoredPhones.Contains(member.Member))
				{
					ip--;
					continue;
				}

				// Check for a match. If member is null it means the current
				// member is a PatternGroup, not a PatternGroupMember.
				compareResult = (member != null ? member.ContainsMatch(phones[ip]) :
					((PatternGroup)m_members[im]).SearchGroup(phones, ref ip));

				switch (compareResult)
				{
					case CompareResultType.Match:
						// Save phone index of match, if it hasn't been done yet.
						if (results[0] == -1)
							results[0] = ip;
						break;

					case CompareResultType.Ignored:	break;
					default: return false;			
				}	

				// Point to phone to the left and the next pattern member to the left.
				ip--;
				im--;
			}

			// If we ended up with a match after coming to the beginning of the phones to
			// search, but before getting to the beginning of the search pattern members
			// and the first member in the pattern is not a word boundary, then we really
			// don't have a match. Therefore, return false.
			if (ip < 0 && im >= 0)
			{
				if (member == null || member.Member != " " || im > 0)
				{
					results[0] = results[1] = -1;
					return false;
				}
			}

			return (compareResult == CompareResultType.Match);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches through a sequential collection of pattern members and phones, to
		/// determine whether or not a run of phones matches the environment after portion
		/// of the full pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool SearchSequentiallyForEnvAfter(string[] phones, int startIndex, ref int[] results)
		{
			CompareResultType compareResult = CompareResultType.NoMatch;
			PatternGroupMember member = null;
			int ip = startIndex;				// Index into the collection of phones
			int im = 0;							// Index into the collection of members

			while (m_members != null && im < m_members.Count)
			{
				member = m_members[im] as PatternGroupMember;

				// Check if the current member is a zero/one or more member.
				if (member != null)
				{
					if (member.MemberType == MemberType.ZeroOrMore)
						return true;

					if (member.MemberType == MemberType.OneOrMore)
						return CheckForOneOrMorePhonesAfter(ip, phones);
				}

				// Break out of the loop if we've run out of phones.
				if (ip == phones.Length)
					break;

				// Check if the phone is ignored, making sure the current member is not in
				// the ignored list. If the current member is in the ignored list, then
				// don't ignore it because it has been explicitly included in the pattern.
				if (SearchEngine.IgnoredPhones.Contains(phones[ip]) && member != null &&
					!SearchEngine.IgnoredPhones.Contains(member.Member))
				{
					ip++;
					continue;
				}

				// Check for a match. If member is null it means the current
				// member is a PatternGroup, not a PatternGroupMember.
				compareResult = (member != null ? member.ContainsMatch(phones[ip]) :
					((PatternGroup)m_members[im]).SearchGroup(phones, ref ip));

				switch (compareResult)
				{
					case CompareResultType.Match:
						// Save phone index of match, if it hasn't been done yet.
						if (results[0] == -1)
							results[0] = ip;
						break;

					case CompareResultType.Ignored: break;
					default: return false;
				}

				// Point to next phone to the right and the next pattern member to the right.
				ip++;
				im++;
			}

			// If we ended up with a match after coming to the end of the phones to search,
			// but before getting to the end of the list of search pattern members and the
			// last member in the pattern is not a word boundary, then we really don't have
			// a match. Therefore, return false.
			if (ip == phones.Length && im < m_members.Count)
			{
				if (member == null || member.Member != " " || im < m_members.Count - 1)
				{
					results[0] = results[1] = -1;
					return false;
				}
			}

			return (compareResult == CompareResultType.Match);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool CheckForOneOrMorePhonesBefore(int ip, string[] phones)
		{
			int count = 0;

			for (int i = ip; i >= 0 && phones[i] != " "; i--)
			{
				if (!SearchEngine.IgnoredPhones.Contains(phones[i]))
					count++;
			}

			return (count > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool CheckForOneOrMorePhonesAfter(int ip, string[] phones)
		{
			int count = 0;

			for (int i = ip; i < phones.Length && phones[i] != " "; i++)
			{
				if (!SearchEngine.IgnoredPhones.Contains(phones[i]))
					count++;
			}

			return (count > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CompareResultType SearchGroup(string[] phones, ref int ip)
		{
			int matchLength;
			return SearchGroup(phones, ref ip, out matchLength);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CompareResultType SearchGroup(string[] phones, ref int ip, out int matchLength)
		{
			matchLength = -1;

			if (m_members == null)
				return CompareResultType.NoMatch;

			int[] results = new[] { -1, -1 };
			CompareResultType compareResult = CompareResultType.NoMatch;

			foreach (object member in m_members)
			{
				PatternGroup group = member as PatternGroup;

				if (group == null)
				{
					compareResult = ((PatternGroupMember)member).ContainsMatch(phones[ip]);
					matchLength = 1;
				}
				else
				{
					if (group.GroupType != GroupType.Sequential)
						compareResult = group.SearchGroup(phones, ref ip, out matchLength);
					else
					{
						if (SearchEngine.IgnoredPhones.Contains(phones[ip]))
						{
							if (m_envType == EnvironmentType.Before && ip > 0)
								ip--;
							else if (ip < phones.Length - 1)
								ip++;
						}

						matchLength = -1;
						if (group.SearchSequentially(phones, ip, ref results) && results[0] == ip)
						{
							matchLength = results[1];
							return CompareResultType.Match;
						}

						results[0] = results[1] = -1;
					}
				}

				if (compareResult == CompareResultType.Ignored ||
					(compareResult == CompareResultType.Match && m_type != GroupType.And) ||
					(compareResult == CompareResultType.NoMatch && m_type != GroupType.Or))
				{
					return compareResult;
				}
			}

			// We should only get this far for two reasons. 1) we're an OR group and no
			// match was found, or 2) we're an AND group and every member yielded a match.
			return (m_type == GroupType.And ? CompareResultType.Match : CompareResultType.NoMatch);
		}

		#endregion
	}
}
