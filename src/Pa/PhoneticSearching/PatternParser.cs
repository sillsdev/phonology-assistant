using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class PatternParser
	{
		private readonly PaProject _project;
		private readonly Dictionary<char, object> _tokenGroups;

		private const char kMinToken = (char)(char.MaxValue - 256);
		private char _token = kMinToken;

		private EnvironmentType _currEnvType;

		/// ------------------------------------------------------------------------------------
		public PatternParser(PaProject project)
		{
			_project = project;
			_tokenGroups = new Dictionary<char, object>();
		}

		/// ------------------------------------------------------------------------------------
		public PatternGroup Parse(string pattern, EnvironmentType envType)
		{
			// By the time we get here, the pattern should have been validated already so
			// certain assumptions are made that would throw exceptions were the pattern
			// not valid.

			_currEnvType = envType;
			_tokenGroups.Clear();
			_token = kMinToken;

			pattern = ReplaceBracketedClassNamesWithPatterns(pattern);
			pattern = ReplaceSquarBracketedTextWithTokens(pattern);

			pattern = ParseTextBetweenOpenAndCloseSymbols(pattern,
				FindInnerMostSquareBracketPairs, ParseTextInBrackets);
			
			pattern = ParseTextBetweenOpenAndCloseSymbols(pattern,
				FindInnerMostBracesPair, ParseTextInBraces);

			return CreateOuterMostPatternGroup(pattern);
		}

		/// ------------------------------------------------------------------------------------
		private PatternGroup CreateOuterMostPatternGroup(string pattern)
		{
			//foreach (var grp in _tokenGroups.Values.OfType<PatternGroup>().Where(g => g.Members.Count == 1))
			//    grp.GroupType = GroupType.Sequential;

			var group = new PatternGroup(_currEnvType) { GroupType = GroupType.Sequential };
			PatternGroupMember member = null;

			foreach (char chr in pattern)
			{
				if (chr == '+')
				{
					if (member != null)
					{
						group.AddRangeOfMembers(member.CloseMember() ?? new[] { member });
						member = null;
					}
					
					group.AddMember(new PatternGroupMember("+"));
				}
				else if (chr < kMinToken)
					(member ?? (member = new PatternGroupMember())).AddToMember(chr);
				else
				{
					if (member != null)
					{
						group.AddRangeOfMembers(member.CloseMember() ?? new[] { member });
						member = null;
					}

					group.AddMember(_tokenGroups[chr]);
				}
			}

			if (member != null)
				group.AddRangeOfMembers(member.CloseMember() ?? new[] { member });

			if (group.Members.Count == 1 && group.Members[0] is PatternGroup)
				group = (PatternGroup)group.Members[0];

			//if (group.Members.Count == 1 && group.Members[0] is PatternGroupMember)
			//    group.GroupType = GroupType.Sequential;

			return group;
		}

		#region Methods for parsing text between brackets and braces
		/// ------------------------------------------------------------------------------------
		public string ReplaceBracketedClassNamesWithPatterns(string pattern)
		{
			var match = FindInnerAngleBracketPairs(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");
				pattern = pattern.Replace(match.Value, _project.SearchClasses[bracketedText].Pattern);
				match = match.NextMatch();
			}

			return pattern;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method goes through the pattern and replaces consonant and vowel class
		/// specifiers (i.e. [C] and [V]) and feature names between square brackets with
		/// tokens. The replacment includes the square brackets.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ReplaceSquarBracketedTextWithTokens(string pattern)
		{
			_tokenGroups.Clear();
			var match = FindInnerMostSquareBracketPairs(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");

				if (!bracketedText.Contains(App.DottedCircle))
					_tokenGroups[++_token] = new PatternGroupMember(bracketedText);
				else
					_tokenGroups[++_token] = bracketedText;

				pattern = ReplaceMatchedTextWithToken(pattern, match, _token);
				match = match.NextMatch();
			}

			return pattern.Replace("$", string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will parse the text between brackets or braces, depending on what the
		/// grouping symbol regular expression provider returns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ParseTextBetweenOpenAndCloseSymbols(string pattern,
			Func<string, Match> groupingSymbolRegexProvider,
			Func<string, Match, string> parsingProvider)
		{
			while (true)
			{
				var match = groupingSymbolRegexProvider(pattern);
				if (!match.Success)
					return pattern;

				while (match.Success)
				{
					pattern = parsingProvider(pattern, match);
					match = match.NextMatch();
				}

				pattern = pattern.Replace("$", string.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		private string ParseTextInBrackets(string pattern, Match match)
		{
			var group = new PatternGroup(_currEnvType) {GroupType = GroupType.And};
			var symbolsInBracketedText = string.Empty;
			var bracketedText = ParseTextBetweenOpenAndCloseSymbols(match.Result("${bracketedText}"),
				FindInnerMostBracesPair, ParseTextInBraces);

			foreach (var chr in bracketedText)
			{
				if (chr <= kMinToken)
					symbolsInBracketedText += chr;
				else if (_tokenGroups[chr] is string)
				{
					// The only time a token group is a string is when it contains a diacritic pattern cluster.
					if (((string)_tokenGroups[chr]).Contains(App.DottedCircle))
						group.SetDiacriticPattern((string)_tokenGroups[chr]);
				}
				else
				{
					group.AddMember(_tokenGroups[chr]);
					_tokenGroups.Remove(chr);
				}
			}

			if (symbolsInBracketedText != string.Empty)
			{
				// If plain text (i.e. not features or classes) is found between the square brackets,
				// then if it represents a single phone, that's fine. Otherwise, it's an error.
				// This deals with patterns like "[b[0*]]" (where 0 is the diacritic placeholder).
				// By the time we get here, it's assumed the SearchQueryValidator has caught errors.
				var phonesInBrackets = _project.PhoneticParser.Parse(symbolsInBracketedText, true, false);
				group.AddMember(new PatternGroupMember(phonesInBrackets[0]));
			}

			if (group.Members.Count == 1 && group.Members[0] is PatternGroup)
				group = (PatternGroup) group.Members[0];

			_tokenGroups[++_token] = group;
			return ReplaceMatchedTextWithToken(pattern, match, _token);
		}

		/// ------------------------------------------------------------------------------------
		private string ParseTextInBraces(string pattern, Match match)
		{
			var group = new PatternGroup(_currEnvType) {GroupType = GroupType.Or};
			var bracketedText = match.Result("${bracketedText}");
			var piecesBetweenBraces = bracketedText.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

			foreach (var piece in piecesBetweenBraces)
			{
				if (piece[0] >= kMinToken)
				{
					group.AddMember(_tokenGroups[piece[0]]);
					_tokenGroups.Remove(piece[0]);
				}
				else if (piece.StartsWith("(") && piece.EndsWith(")"))
					group.AddMember(ParseTextInParentheses(piece.Trim('(', ')')));
				else
					group.AddMember(new PatternGroupMember(piece));
			}

			if (group.Members.Count == 1 && group.Members[0] is PatternGroup)
				group = (PatternGroup) group.Members[0];

			_tokenGroups[++_token] = group;
			return ReplaceMatchedTextWithToken(pattern, match, _token);
		}

		/// ------------------------------------------------------------------------------------
		private PatternGroup ParseTextInParentheses(string text)
		{
			var subGroup = new PatternGroup(_currEnvType) {GroupType = GroupType.Sequential};
			
			foreach (var phone in _project.PhoneticParser.Parse(text, true, false))
			{
				if (phone[0] < kMinToken)
					subGroup.AddMember(new PatternGroupMember(phone));
				else
				{
					subGroup.AddMember(_tokenGroups[phone[0]]);
					_tokenGroups.Remove(phone[0]);
				}
			}

			return subGroup;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Replaces the matched text in the specified pattern with the specified token.
		/// The token is followed by dollar signs so the length of the token and dollar signs
		/// is the same length as the length of the text being replaced. This is so the
		/// overall length of the pattern doesn't change as it's being parsed. The dollar
		/// signs are removed later in the process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string ReplaceMatchedTextWithToken(string pattern, Match match, char token)
		{
			var newPattern = pattern.Substring(0, match.Index) + token.ToString();
			newPattern += new string('$', match.Length - 1);

			if (match.Index + match.Length < pattern.Length)
				newPattern += pattern.Substring(match.Index + match.Length);

			return newPattern;
		}

		#endregion
		
		#region Methods returning reg. expression for parsing a pattern
		/// ------------------------------------------------------------------------------------
		public static Match FindInnerAngleBracketPairs(string pattern)
		{
			var regex = new Regex(@"\<(?<bracketedText>[^<>]*)>");
			return regex.Match(pattern);
		}

		/// ------------------------------------------------------------------------------------
		public static Match FindInnerMostSquareBracketPairs(string pattern)
		{
			var regex = new Regex(@"\[(?<bracketedText>[^\[\]]*)\]");
			return regex.Match(pattern);
		}

		/// ------------------------------------------------------------------------------------
		public static Match FindInnerMostBracesPair(string pattern)
		{
			var regex = new Regex(@"\{(?<bracketedText>[^\{\}]*)\}");
			return regex.Match(pattern);
		}

		#endregion
	}
}
