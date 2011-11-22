using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class PatternParser
	{
		private readonly List<string> _errors;
		private readonly PaProject _project;
		private readonly Dictionary<char, object> _tokenGroups;

		private const char kMinToken = (char)(char.MaxValue - 256);
		private char _token = kMinToken;

		private EnvironmentType _currEnvType;

		// TODO: Check for empty pairs (i.e. <> {} []) and mismatched pairs.

		/// ------------------------------------------------------------------------------------
		public PatternParser(PaProject project)
		{
			_project = project;
			_errors = new List<string>();
			_tokenGroups = new Dictionary<char, object>();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> Errors
		{
			get { return _errors; }
		}

		/// ------------------------------------------------------------------------------------
		public bool HasErrors
		{
			get { return _errors.Count > 0; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void ResetErrors()
		{
			_errors.Clear();
		}

		/// ------------------------------------------------------------------------------------
		public PatternGroup Parse(string pattern, EnvironmentType envType)
		{
			_currEnvType = envType;
			_tokenGroups.Clear();
			_token = kMinToken;

			if (!VerifyBracketedText(pattern))
				return null;

			pattern = ReplaceBracketedClassNamesWithPatterns(pattern);
			pattern = ReplaceSquarBracketedTextWithTokens(pattern);

			pattern = ParseTextBetweenOpenAndCloseSymbols(pattern,
				FindInnerMostSquareBracketPairs, ParseTextInBrackets);
			
			pattern = ParseTextBetweenOpenAndCloseSymbols(pattern,
				FindInnerMostBracesPair, ParseTextInBraces);

			var group = CreateOuterMostPatternGroup(pattern);
			return (HasErrors ? null : group);
		}

		/// ------------------------------------------------------------------------------------
		private PatternGroup CreateOuterMostPatternGroup(string pattern)
		{
			var group = new PatternGroup(_currEnvType) { GroupType = GroupType.Sequential };
			PatternGroupMember member = null;

			foreach (char chr in pattern)
			{
				if (chr < kMinToken)
				{
					if (member == null)
						member = new PatternGroupMember();

					member.AddToMember(chr);
				}
				else
				{
					if (member != null)
					{
						foreach (var newMember in (member.CloseMember() ?? new[] { member }))
							group.AddMember(newMember);

						member = null;
					}

					group.AddMember(_tokenGroups[chr]);
				}
			}

			if (member != null)
			{
				foreach (var newMember in (member.CloseMember() ?? new[] { member }))
					group.AddMember(newMember);
			}

			if (group.Members.Count == 1 && group.Members[0] is PatternGroup)
				group = (PatternGroup)group.Members[0];

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
				var srchClass = _project.SearchClasses[bracketedText];
				if (srchClass != null)
					pattern = pattern.Replace(match.Value, srchClass.Pattern);
				else
				{
					var msg = App.GetString("PhoneticSearchingMessages.SearchClassNotFound",
						"The specified search class '{0}' does not exist. Verify the search " +
						"class name by going to the Classes dialog box.");

					_errors.Add(string.Format(msg, bracketedText));
					pattern = string.Empty;
				}

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

				if (!bracketedText.Contains(App.kDottedCircle))
					_tokenGroups[++_token] = new PatternGroupMember(bracketedText);
				else
				{
					if (bracketedText.Contains("+") && bracketedText.Contains("*"))
					{
						_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidDiacriticPlaceholderSyntax",
							"The symbols '*' and '+' may not appear between square brackets together " +
							"with a diacritic placeholder. One or the other is allowed, but not both."));
					}

					_tokenGroups[++_token] = bracketedText;
				}

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
			int diacriticPlaceholderCount = 0;
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
					if (((string)_tokenGroups[chr]).Contains(App.kDottedCircle))
					{
						group.SetDiacriticPattern((string)_tokenGroups[chr]);
						diacriticPlaceholderCount++;
					}
				}
				else
				{
					group.AddMember(_tokenGroups[chr]);
					_tokenGroups.Remove(chr);
				}
			}

			if (diacriticPlaceholderCount > 1)
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyDiacriticPlaceholderMsg",
					"There were too many diacritic placeholders found in one of the AND groups."));
			}

			if (symbolsInBracketedText != string.Empty)
			{
				// If plain text (i.e. not features or classes) is found between the square brackets,
				// then if it represents a single phone, that's fine. Otherwise, it's an error.
				// This deals with patterns like "[b[0*]]" (where 0 is the diacritic placeholder).
				var phonesInBrackets = _project.PhoneticParser.Parse(symbolsInBracketedText, true, false);
				if (phonesInBrackets.Length == 1)
					group.AddMember(new PatternGroupMember(phonesInBrackets[0]));
				else
				{
					var msg = App.GetString("PhoneticSearchingMessages.InvalidCharacterInANDGroup",
						"The text '{0}' is invalid in an AND group.");

					_errors.Add(string.Format(msg, symbolsInBracketedText));
					return string.Empty;
				}
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

			if (piecesBetweenBraces.Length == 1)
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.OnlyOneItemInOrGroup",
					"Only one item was found in an OR group. Verify that all items between braces {} are separated by commas."));
			}

			foreach (var piece in piecesBetweenBraces.Where(piece => GetIsOrGroupMemberValid(piece)))
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
		public bool GetIsOrGroupMemberValid(string orGroupMember)
		{
			if ("[]{}()+*_#<>".Contains(orGroupMember))
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidSymbolsInORGroup",
					"The symbols '[]{}()<>+*_#' are not allowed in OR groups."));
				
				return false;
			}

			if (orGroupMember.Contains(App.kDottedCircle))
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidDiacriticPlaceholderInORGroup",
					"Diacritic placeholders are not valid in OR groups."));

				return false;
			}

			var phonesInMember = _project.PhoneticParser.Parse(orGroupMember, true, false);
			if (phonesInMember == null)
			{
				var msg = App.GetString("PhoneticSearchingMessages.InvalidORGroupMember",
					"The text '{0}' is not recognized as valid phonetic data.");

				_errors.Add(string.Format(msg, orGroupMember));
				return false;
			}

			if (phonesInMember.Length > 1 && !orGroupMember.StartsWith("(") && !orGroupMember.EndsWith(")"))
			{
				var msg = App.GetString("PhoneticSearchingMessages.UnparentheticalTextInORGroup",
					"The text '{0}' is in an OR group and must be surrounded in parentheses " +
					"because it represents more than a single phone.");

				_errors.Add(string.Format(msg, orGroupMember));
				return false;
			}

			return true;
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
		
		#region Methods for verifying pattern validity
		/// ------------------------------------------------------------------------------------
		public bool VerifyBracketedText(string pattern)
		{
			var match = FindInnerMostSquareBracketPairs(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");

				if (!bracketedText.Contains(App.kDottedCircle) &&
					bracketedText != "C" && bracketedText != "V" &&
					!App.AFeatureCache.Keys.Any(f => f == bracketedText) &&
					!App.BFeatureCache.Keys.Any(f => f == bracketedText))
				{
					_errors.Add(SearchEngine.kBracketingError + ":" + bracketedText);
					return false;
				}

				match = match.NextMatch();
			}

			return true;
		}

		#endregion

		#region Methods returning reg. expression for parsing a pattern
		/// ------------------------------------------------------------------------------------
		private Match FindInnerAngleBracketPairs(string pattern)
		{
			var regex = new Regex(@"\<(?<bracketedText>[^<>]+)>");
			return regex.Match(pattern);
		}

		/// ------------------------------------------------------------------------------------
		private Match FindInnerMostSquareBracketPairs(string pattern)
		{
			var regex = new Regex(@"\[(?<bracketedText>[^\[\]]+)\]");
			return regex.Match(pattern);
		}

		/// ------------------------------------------------------------------------------------
		private Match FindInnerMostBracesPair(string pattern)
		{
			var regex = new Regex(@"\{(?<bracketedText>[^\{\}]+)\}");
			return regex.Match(pattern);
		}

		#endregion
	}
}
