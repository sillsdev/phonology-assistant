using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SIL.Pa.Data;

namespace SIL.Pa.FFSearchEngine
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
		private static Dictionary<int, string> s_diacriticPlaceholders = new Dictionary<int, string>();

		private bool m_isRootGroup = false;
		private string m_cachedSearchWord = null;
		private string[] m_cachedSearchChars = null;
		private GroupType m_type = GroupType.And;
		private PatternGroupMember m_currMember;
		private EnvironmentType m_envType = EnvironmentType.Item;
		private string m_diacriticPattern = null;

		// m_members can contain both objects of type PatternGroup and PatternGroupMember.
		private ArrayList m_members;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PatternGroup(EnvironmentType envType) : this(envType, true)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private PatternGroup(EnvironmentType envType,  bool isRootGroup)
		{
			m_envType = envType;
			m_isRootGroup = isRootGroup;
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
			if (!m_isRootGroup)
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
				int i = i = pattern.Length - 1;
				while ((int)pattern[i] < 32)
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
					if (i + 1 < pattern.Length && (int)pattern[i + 1] < 32)
					{
						CloseCurrentMember();
						AddDiacriticPatternToCurrentMember((int)pattern[++i]);
					}

					break;
				}

				// Are we beginning a sub group?
				if (pattern[i] == '[' || pattern[i] == '{')
				{
					BeginSubGroup(pattern, ref i);
					continue;
				}

				if ((int)pattern[i] < 32)
				{
					AddDiacriticPatternToCurrentMember((int)pattern[i]);
					continue;
				}

				if (pattern[i] == '|')
				{
					CloseCurrentMember();
					continue;
				}

				// Are we at the end of a group member?
				if (pattern[i] == '$' || pattern[i] == ',')
				{
					// We've reached the end of a PatternGroupMember so close it out.
					// At this point, CloseCurrentMember should never return false.
					if (!CloseCurrentMember())
					{
						// TODO: Log a meaningful error
					}
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

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Close out the current member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CloseCurrentMember()
		{
			if (m_currMember == null)
				return false;

			m_currMember.CloseMember();

			if (m_currMember.DiacriticPattern != null && m_currMember.DiacriticPattern.Length > 0 &&
				(m_currMember.Member == null || m_currMember.Member.Length == 0))
			{
				// TODO: log meaningful error - diacritic patterns should
				// never come before any other members nor be by themselves.
				return false;
			}

			// If the member just closed came back as with the SinglePhone type and
			// the current group is Sequential (which can only be true for the root
			// group) then change the member's type to IPACharacterRun since IPA
			// characters contained in a sequential group can only be runs and IPA
			// characters contained in AND or OR groups can only be singletons.
			if (m_currMember.MemberType == MemberType.SinglePhone &&
				m_type == GroupType.Sequential)
			{
				m_currMember.MemberType = MemberType.IPACharacterRun;
			}

			m_members.Add(m_currMember);
			m_currMember = null;
			return true;
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
				PatternGroup subGroup = new PatternGroup(m_envType, false);
				if (!subGroup.Parse(subGroupPattern))
				{
					// TODO: log meaningful error.
				}
				
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
			char closeBracket = (openBracket == '[' ? ']' : '}');
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

			// TODO: display meaningful error
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
		private bool AddDiacriticPatternToCurrentMember(int diacriticClusterKey)
		{
			if (m_members.Count == 0 && m_currMember == null)
			{
				// TODO: log meaningful error - diacritic patterns should
				// never come before any other members nor be by themselves.
				return false;
			}

			string diacriticPattern;
			if (!s_diacriticPlaceholders.TryGetValue(diacriticClusterKey, out diacriticPattern))
			{
				// TODO: log meaningful error.
				return false;
			}

			// Modify the current member or group with the diacritic pattern.
			if (m_currMember != null)
				m_currMember.DiacriticPattern = diacriticPattern;
			else
			{
				m_diacriticPattern = diacriticPattern;
				PropogateGroupDiacriticPatternToMembers(this, diacriticPattern);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycles through the members of the specified group and applies the group's diacritic
		/// pattern to group's members and then clears the group's diacritic pattern. When
		/// a member of the group is another group, then this method is called recursively
		/// to deal with that group's members.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PropogateGroupDiacriticPatternToMembers(PatternGroup group,
			string diacriticPattern)
		{
			System.Diagnostics.Debug.Assert(group != null);

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
		private string MergeDiacriticPatterns(string ptrn1, string ptrn2)
		{
			if (string.IsNullOrEmpty(ptrn1))
				return ptrn2;

			if (string.IsNullOrEmpty(ptrn2))
				return ptrn1;

			bool containsZorM = (ptrn1.IndexOf("*") >= 0) || (ptrn2.IndexOf("*") >= 0);
			bool containsOorM = (ptrn1.IndexOf("+") >= 0) || (ptrn2.IndexOf("+") >= 0);

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
				// TODO: show meaningful message
				return false;
			}

			pattern = pattern.Replace("[c]", "[C]");
			pattern = pattern.Replace("[v]", "[V]");

			// Get rid of all spaces.
			while (pattern.IndexOf(' ') >= 0)
				pattern = pattern.Replace(" ", string.Empty);

			// Get rid of double wildcards.
			while (pattern.IndexOf("**") >= 0)
				pattern = pattern.Replace("**", "*");

			// Get rid of double wildcards.
			while (pattern.IndexOf("++") >= 0)
				pattern = pattern.Replace("++", "+");

			if (!VerifyMatchingBrackets(pattern, '[', ']'))
			{
				// TODO: Show error.
				return false;
			}

			if (!VerifyMatchingBrackets(pattern, '{', '}'))
			{
				// TODO: Show error.
				return false;
			}

			if (!VerifyBracketOrdering(pattern))
			{
				// TODO: Show error.
				return false;
			}

			if (!VerifyDiacriticPlaceholderCluster(ref pattern))
			{
				// TODO: Show error.
				return false;
			}

			if (!VerifyZeroOneOrMorePlacement(ref pattern))
			{
				// TODO: Show error.
				return false;
			}

			// TODO: Other verifications to perform
			// More than 2 # in an environment.
			// More than 2 * in an environment.
			// Char. modifiers before a dotted circle but within the square brackets.
			// [[C][C][o^h] or [[V][V][o^h] don't make sense to have double con. or vowel in AND group.
			// For that matter, things like this don't make sense [[C][p]] or [[V][a]] or
			// Characters in square brakets don't make sense. Ex. [a,b] - a match cannot be both an 'a' AND 'b'


			//pattern = pattern.Replace("#", string.Empty);
			pattern = pattern.Replace(",,", ",");
			pattern = pattern.Replace("],", "]");
			pattern = pattern.Replace("},", "}");
			pattern = pattern.Replace(",+", "+");
			pattern = pattern.Replace(",-", "-");
			pattern = FFNormalizer.Normalize(pattern);
			pattern = DelimitMembers(pattern);
			pattern = pattern.Replace("#", " ");

			if (m_isRootGroup)
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
					// TODO: Show meaningful error
					return false;
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
		private bool VerifyMatchingBrackets(string pattern, char open, char close)
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
		private bool VerifyBracketOrdering(string pattern)
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
			while ((i = pattern.IndexOf(DataUtils.kDottedCircleC, i)) >= 0)
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
				cluster = cluster.Replace(DataUtils.kDottedCircle, string.Empty);

				if (foundOneOrMoreSymbol && foundOneOrMoreSymbol)
				{
					// TODO: Log meaningful error.
				}

				// This should never happen.
				if (s_currDiacriticPlaceholderMaker == 31)
				{
					// TODO: Log meaningful error.
					// We've already got too many diacritic placeholder markers so just
					// throw out this one. :o) As I said, this should never happen.
					pattern = pattern.Substring(0, i - 1) +	pattern.Substring(idxCloseBracket + 1);
					continue;
				}

				// Replace the diacritic cluster in the pattern with a marker.
				pattern = pattern.Substring(0, i - 1) + 
					((char)s_currDiacriticPlaceholderMaker).ToString() +
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
				return false;

			// Check how many pluses are in the pattern. Only one is allowed.
			splits = tmpPattern.Split("+".ToCharArray());
			if (splits.Length > 2)
				return false;
			
			int z = tmpPattern.IndexOf('*');
			int o = tmpPattern.IndexOf('+');

			// Can't have both in the same pattern.
			if (z >= 0 && o >= 0)
				return false;

			// Niether one are valid in the search item.
			if (m_envType == EnvironmentType.Item && (z >= 0 || o >= 0))
				return false;

			if (z >= 0)
			{
				if (m_envType == EnvironmentType.Before)
				{
					// Must be first item in pattern.
					if (z != 0)
						return false;

					tmpPattern = "|*$" + tmpPattern.Substring(1);
				}
				else
				{
					// Must be last item in pattern
					if (z != tmpPattern.Length - 1)
						return false;

					tmpPattern = tmpPattern.TrimEnd("*".ToCharArray());
					tmpPattern += "|*$";
				}
			}

			if (o >= 0)
			{
				if (m_envType == EnvironmentType.Before)
				{
					// Must be first item in pattern.
					if (o != 0)
						return false;

					tmpPattern = "|+$" + tmpPattern.Substring(1);
				}
				else
				{
					// Must be last item in pattern
					if (o != tmpPattern.Length - 1)
						return false;

					tmpPattern = tmpPattern.TrimEnd("+".ToCharArray());
					tmpPattern += "|+$";
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
			//string replace = string.Format("][{0}", DataUtils.kDottedCircle);
			//string tmpPattern = pattern.Replace(replace, DataUtils.kDottedCircle);
			string tmpPattern = pattern;
			StringBuilder modifiedPtrn = new StringBuilder(tmpPattern);

			CommaDelimitBracketedMember("[+", tmpPattern, ref modifiedPtrn);
			CommaDelimitBracketedMember("[-", tmpPattern, ref modifiedPtrn);
			CommaDelimitBracketedMember("[C", tmpPattern, ref modifiedPtrn);
			CommaDelimitBracketedMember("[V", tmpPattern, ref modifiedPtrn);
			DelimitArticulatoryFeatures(tmpPattern, ref modifiedPtrn);

			string finalModified = modifiedPtrn.ToString();
			
			finalModified = modifiedPtrn.ToString();
			finalModified = finalModified.Replace(" ", string.Empty);
			finalModified = finalModified.Replace("$,|", "$|");
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
		private bool DelimitArticulatoryFeatures(string pattern, ref StringBuilder modifiedPtrn)
		{
			string tmpPattern = pattern.ToLower();

			foreach (KeyValuePair<string, AFeature> info in DataUtils.AFeatureCache)
			{
				string feature = "[" + info.Value.Name.ToLower();
				if (!CommaDelimitBracketedMember(feature, tmpPattern, ref modifiedPtrn))
					return false;
			}

			return true;
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
			char dottedCircle = DataUtils.kDottedCircleC;

			while ((i = pattern.IndexOf(lookFor, i)) >= 0)
			{
				int closeIndex = i;
				
				// Once we find what we're looking for, step through the following 
				// characters looking for the end of the feature.
				while (closeIndex < pattern.Length && pattern[closeIndex] != ']')
				{
					// Check if we've run into a diacritic pattern cluster along the way.
					if (!foundDiacriticPlaceholder && pattern[closeIndex] == dottedCircle)
						foundDiacriticPlaceholder = true;

					closeIndex++;
				}

				// We had better find the close bracket for the member.
				if (closeIndex == pattern.Length)
				{
					// TODO: Log useful error
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
						// TODO: Log useful error
						return false;
					}
				
					modifiedPtrn[i - 1] = ' ';
					modifiedPtrn[closeIndex + 1] = ' ';
				}

				modifiedPtrn[closeIndex] = '$';
				modifiedPtrn[i] = '|';
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
		private GroupType GetRootGroupType(string pattern)
		{
			// Because diacritic placeholder groups don't count as pattern groups,
			// take them out before trying to determine the root group type.
			StringBuilder modifiedPattern = new StringBuilder();
			for (int i = 0; i < pattern.Length; i++)
			{
				if ((int)pattern[i] >= 32)
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
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Search(string eticWord, int startIndex, out int[] results)
		{
			if (eticWord != m_cachedSearchWord)
			{
				m_cachedSearchWord = eticWord;
				m_cachedSearchChars = IPACharCache.PhoneticParser(eticWord, false);
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
			results = new int[] {-1, -1};

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

			// At this point, we know this group is either and And or Or group. Though
			// it may contain decendent groups, it only contains one child group.
			for (int i = startIndex; i < phones.Length; i++)
			{
				if (phones[i] == string.Empty)
					continue;

				CompareResultType compareResult = SearchGroup(phones[i]);
				
				if (compareResult == CompareResultType.Ignored)
					continue;

				if (compareResult != CompareResultType.NoMatch)
				{
					// Return where the match was found.
					results[0] = i;
					results[1] = 1;
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
			CompareResultType compareResult = CompareResultType.NoMatch;
			int i = startIndex;
			int incvalue = (m_envType == EnvironmentType.Before ? -1 : 1);
			int mindex = (m_envType == EnvironmentType.Before ? m_members.Count - 1 : 0);
			int ignoredPhoneCount = 0;
			PatternGroupMember member = null;

			while (m_members != null && mindex >= 0 && mindex < m_members.Count)
			{
				member = m_members[mindex] as PatternGroupMember;

				// If we're looking at the before or after environment then check if the current
				// member is a zero/one or more member and process it if it is.
				if (m_envType != EnvironmentType.Item && member != null)
				{
					// Check if the current member is a zero/one or more member.
					if (ZeroOneOrMoreProcessed(phones.Length, member, ref compareResult,
						i, startIndex, ignoredPhoneCount))
					{
						mindex += incvalue;
						break;
					}
				}

				// Break out of the loop if we've run out of phones.
				if (i < 0 || i == phones.Length)
					break;

				// Check if phone is ignored.
				if (SearchEngine.IgnoredPhones.Contains(phones[i]))
				{
					i += incvalue;
					ignoredPhoneCount++;
					continue;
				}

				int stoCharIndex = i;

				// If member is null it means the current member
				// is a PatternGroup, not a PatternGroupMember.
				if (member == null)
					compareResult = ((PatternGroup)m_members[mindex]).SearchGroup(phones[i]);
				else
				{
					compareResult = (member.MemberType == MemberType.IPACharacterRun ?
						member.ContainsMatch(m_envType, phones, ref i) :
						member.ContainsMatch(phones[i]));
				}

				// If the phone was ignored, then move on to the next one.
				if (compareResult == CompareResultType.Ignored)
				{
					i += incvalue;
					ignoredPhoneCount++;
					continue;
				}

				if (compareResult == CompareResultType.Match)
				{
					// Point to the next member in the group.
					mindex += incvalue;

					// If we haven't yet stored the pointer into the phonetic
					// word where we first got a match, then do so.
					if (results[0] == -1)
						results[0] = stoCharIndex;
				}
				else
				{
					// Since no match was found and we're not searching the
					// "Item" environment then we've already failed. So get out.
					if (m_envType != EnvironmentType.Item)
						return false;

					// Since no match was found and we're searching for the "Item" then
					// reset the pointer to point back to the first member and clear
					// the index of where a previous match was found.
					mindex = 0;

					// If we had a previous match, we need to reset the current phone pointer
					// back to where that match was. This will prepare to begin the next round
					// of searching at the next phone following it.
					if (results[0] > -1)
						stoCharIndex = results[0];
					
					results[0] = -1;
				}

				// Figure out what phone to point to next. When there's no match, then we need
				// to reset the phone pointer back. Otherwise, go to the next phone.
				i = (compareResult == CompareResultType.NoMatch ?
					stoCharIndex + 1 : i + incvalue);
			}

			if (compareResult == CompareResultType.Match)
			{
				// If we ended up with a match after coming to the end of the phones to
				// search, but before getting to the end of the search pattern members
				// and the last member in the pattern is not a word boundary, then we
				// really don't have a match. Therefore, return false.
				if ((m_envType == EnvironmentType.After || m_envType == EnvironmentType.Item) &&
					i == phones.Length && mindex < m_members.Count)
				{
					if (member == null || member.Member != " " || mindex < m_members.Count - 1)
					{
						results[0] = results[1] = -1;
						return false;
					}
				}

				// If we ended up with a match after coming to the beginning of the phones
				// to search, but before getting to the beginning of the search pattern members
				// (when we're searching backward for the environment before) and the first
				// member in the pattern is not a word boundary, then we really don't have a
				// match. Therefore, return false.
				if (m_envType == EnvironmentType.Before && i < 0 && mindex >= 0)
				{
					if (member == null || member.Member != " " || mindex > 0)
					{
						results[0] = results[1] = -1;
						return false;
					}
				}

				// If we found a match and we've been searching for matches on the search item,
				// then save the length of the portion of phonetic characters that made the match.
				if (m_envType == EnvironmentType.Item && results[0] != -1)
					results[1] = i - results[0];
			}

			return (compareResult == CompareResultType.Match);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified group member is a zero or more or one or more member
		/// and determines whether or not a match has been found.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ZeroOneOrMoreProcessed(int phoneCount, PatternGroupMember member,
			ref CompareResultType compareResult, int currPhoneIndex, int srchStartIndex,
			int ignoredPhoneCount)
		{
			if (member.MemberType == MemberType.ZeroOrMore)
			{
				compareResult = CompareResultType.Match;
				return true;
			}

			if (member.MemberType == MemberType.OneOrMore)
			{
				if (m_envType == EnvironmentType.Before)
				{
					if (currPhoneIndex < 0)
						compareResult = CompareResultType.NoMatch;
					else
					{
						compareResult = (srchStartIndex >= 0 &&	srchStartIndex >= ignoredPhoneCount ?
							CompareResultType.Match : CompareResultType.NoMatch);
					}
				}
				else if (m_envType == EnvironmentType.After)
				{
					if (currPhoneIndex == phoneCount)
						compareResult = CompareResultType.NoMatch;
					else
					{
						int startDelta = phoneCount - srchStartIndex;
						int countDelta = phoneCount - ignoredPhoneCount;
						compareResult = (srchStartIndex <= phoneCount - 1 && startDelta < countDelta ?
							CompareResultType.Match : CompareResultType.NoMatch);
					}
				}

				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CompareResultType SearchGroup(string phone)
		{
			if (m_members == null)
				return CompareResultType.NoMatch;
			
			CompareResultType compareResult = CompareResultType.NoMatch;

			foreach (object member in m_members)
			{
				compareResult = (member is PatternGroup ?
					((PatternGroup)member).SearchGroup(phone) :
					((PatternGroupMember)member).ContainsMatch(phone));

				if (compareResult == CompareResultType.Match && m_type != GroupType.And)
					return compareResult;

				if (compareResult == CompareResultType.NoMatch && m_type != GroupType.Or)
					return compareResult;
			}

			if (compareResult == CompareResultType.Ignored)
				return compareResult;

			// We should only get this far for two reasons. 1) we're an OR group and no
			// match was found, or 2) we're an AND group and every member yielded a match.
			return (m_type == GroupType.And ? CompareResultType.Match : CompareResultType.NoMatch);
		}

		#endregion
	}
}
