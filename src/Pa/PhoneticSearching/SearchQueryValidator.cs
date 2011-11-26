using System;
using System.Linq;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class SearchQueryValidator : ValidatorBase
	{
		/// ------------------------------------------------------------------------------------
		public SearchQueryValidator(PaProject project) : base(project)
		{
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsValid(SearchQuery query)
		{
			Errors.Clear();

			if (!VerifyGeneralPatternStructure(query.Pattern))
				Errors[GetPatternSyntaxErrorMsg()] = null;

			VerifySearchItem(query.SearchItem);
			VerifyPrecedingEnvironment(query.PrecedingEnvironment);
			VerifyFollowingEnvironment(query.FollowingEnvironment);

			foreach (var item in new[] { query.SearchItem, query.PrecedingEnvironment, query.FollowingEnvironment })
			{
				VerifyTextInSquareBrackets(item);
				VerifyTextInAngleBrackets(item);
				VerifyDiacriticPlaceholders(item);
				VerifyOneDiacriticPlaceholderPerANDGroup(item);

				VerifyNoEmptyTextBetweenOpenAndCloseSymbols(item, PatternParser.FindInnerMostSquareBracketPairs,
					string.Format(App.GetString("PhoneticSearchingMessages.EmptySquareBracketsMsg",
						"The pattern '{0}' contains at least one set of empty square brackets."), item));

				VerifyNoEmptyTextBetweenOpenAndCloseSymbols(item, PatternParser.FindInnerMostBracesPair,
					string.Format(App.GetString("PhoneticSearchingMessages.EmptyBracesMsg",
						"The pattern '{0}' contains at least one set of empty braces."), item));

				VerifyNoEmptyTextBetweenOpenAndCloseSymbols(item, PatternParser.FindInnerAngleBracketPairs,
					string.Format(App.GetString("PhoneticSearchingMessages.EmptyAngleBracketsMsg",
						"The pattern '{0}' contains at least one set of empty angle brackets."), item));

				VerifyMatchingOpenAndCloseSymbols(item, '[', ']',
					string.Format(App.GetString("PhoneticSearchingMessages.MismatchedNumberOfBracketsMsg",
						"In the pattern '{0}', the number of open square brackets does not match the number of closed."), item));

				VerifyMatchingOpenAndCloseSymbols(item, '{', '}',
					string.Format(App.GetString("PhoneticSearchingMessages.MismatchedNumberOfBracesMsg",
						"In the pattern '{0}', the number of open braces does not match the number of closed."), item));

				VerifyMatchingOpenAndCloseSymbols(item, '<', '>',
					string.Format(App.GetString("PhoneticSearchingMessages.MismatchedNumberOfAngleBracketsMsg",
						"In the pattern '{0}', the number of open angle brackets does not match the number of closed."), item));
				
				VerifyMatchingOpenAndCloseSymbols(item, '(', ')',
					string.Format(App.GetString("PhoneticSearchingMessages.MismatchedNumberOfParenthesesMsg",
						"In the pattern '{0}', the number of open parentheses does not match the number of closed."), item));

				ValidateOrGroups(item);
			}
			
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetPatternSyntaxErrorMsg()
		{
			return App.GetString("PhoneticSearchingMessages.PatternSyntaxErrorMsg",
				"The pattern contains a syntax error. The correct format for the pattern is '{0}'");
		}

		/// ------------------------------------------------------------------------------------
		public bool VerifyGeneralPatternStructure(string pattern)
		{
			if (pattern.IndexOf('/') < 0 || pattern.IndexOf('_') < 0)
				return false;

			if (pattern.IndexOf('/') > pattern.IndexOf('_'))
				return false;

			// TODO: What if user created a class with a slash in the name?
			if (pattern.Count(c => c == '/') > 1)
				return false;

			// TODO: What if user created a class with an underscore in the name?
			if (pattern.Count(c => c == '_') > 1)
				return false;

			return (pattern.Split('/', '_').Length == 3);
		}

		/// ------------------------------------------------------------------------------------
		public void VerifySearchItem(string srchItemPattern)
		{
			if (string.IsNullOrEmpty(srchItemPattern))
			{
				Errors[App.GetString("PhoneticSearchingMessages.SearchItemMissingMsg",
					"You must specify a search item.")] = null;
			}

			if (srchItemPattern.Count(c => "#*+".Contains(c)) > 0)
			{
				Errors[App.GetString("PhoneticSearchingMessages.InvalidCharactersInSearchItemMsg",
					"The search item portion of the search pattern contain an illegal symbol. " +
					"The symbols '#', '+' and '*' are not valid in the search item.")] = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void VerifyPrecedingEnvironment(string precedingEnv)
		{
			if (precedingEnv.Count(c => "#*+".Contains(c)) > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.InvalidCharactersInPrecedingEnvironmentMsg",
					"The preceding environment portion of the search pattern contains an illegal combination of characters. " +
					"The symbols '#', '+' or '*' are not allowed together in the preceding environment.")] = null;
			}

			int count = precedingEnv.Count(c => c == '#');
			if (count > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.TooManyWordBoundarySymbolsInPrecedingEnvironmentMsg",
					"The preceding environment portion of the search pattern contains too many word boundary symbols. " +
					"Only one is allowed and it must be at the beginning.")] = null;
			}

			if (count == 1 && !precedingEnv.StartsWith("#"))
			{
				Errors[App.GetString("PhoneticSearchingMessages.MisplacedWordBoundarySymbolInPrecedingEnvironmentMsg",
					"The preceding environment portion of the search pattern contains a misplaced word boundary symbol. " +
					"It must be at the beginning.")] = null;
			}

			count = precedingEnv.Count(c => c == '*');
			if (count > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.TooManyZeroOrMoreSymbolsInPrecedingEnvironmentMsg",
					"The preceding environment portion of the search pattern contains too many 'zero or more' symbols. " +
					"Only one is allowed and it must be at the beginning.")] = null;
			}

			if (count == 1 && !precedingEnv.StartsWith("*"))
			{
				Errors[App.GetString("PhoneticSearchingMessages.MisplacedZeroOrMoreSymbolInPrecedingEnvironmentMsg",
					"The preceding environment portion of the search pattern contains a misplaced 'zero or more' symbol. " +
					"It must be at the beginning.")] = null;
			}

			count = precedingEnv.Count(c => c == '+');
			if (count > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.TooManyOneOrMoreSymbolsInPrecedingEnvironmentMsg",
					"The preceding environment portion of the search pattern contains too many 'one or more' symbols. " +
					"Only one is allowed and it must be at the beginning.")] = null;
			}

			if (count == 1 && !precedingEnv.StartsWith("+"))
			{
				Errors[App.GetString("PhoneticSearchingMessages.MisplacedOneOrMoreSymbolInPrecedingEnvironmentMsg",
					"The preceding environment portion of the search pattern contains a misplaced 'one or more' symbol. " +
					"It must be at the beginning.")] = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void VerifyFollowingEnvironment(string followingEnv)
		{
			if (followingEnv.Count(c => "#*+".Contains(c)) > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.InvalidCharactersInFollowingEnvironmentMsg",
					"The following environment portion of the search pattern contains an illegal combination of characters. " +
					"The symbols '#', '+' or '*' are not allowed together in the following environment.")] = null;
			}

			var count = followingEnv.Count(c => c == '#');
			if (count > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.TooManyWordBoundarySymbolsInFollowingEnvironmentMsg",
					"The following environment portion of the search pattern contains too many word boundary symbols. " +
					"Only one is allowed and it must be at the end.")] = null;
			}

			if (count == 1 && !followingEnv.EndsWith("#"))
			{
				Errors[App.GetString("PhoneticSearchingMessages.MisplacedWordBoundarySymbolInFollowingEnvironmentMsg",
					"The following environment portion of the search pattern contains a misplaced word boundary symbol. " +
					"It must be at the end.")] = null;
			}
			count = followingEnv.Count(c => c == '*');
			if (count > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.TooManyZeroOrMoreSymbolsInFollowingEnvironmentMsg",
					"The following environment portion of the search pattern contains too many 'zero or more' symbols. " +
					"Only one is allowed and it must be at the end.")] = null;
			}

			if (count == 1 && !followingEnv.EndsWith("*"))
			{
				Errors[App.GetString("PhoneticSearchingMessages.MisplacedZeroOrMoreSymbolInFollowingEnvironmentMsg",
					"The following environment portion of the search pattern contains a misplaced 'zero or more' symbol. " +
					"It must be at the end.")] = null;
			}

			count = followingEnv.Count(c => c == '+');
			if (count > 1)
			{
				Errors[App.GetString("PhoneticSearchingMessages.TooManyOneOrMoreSymbolsInFollowingEnvironmentMsg",
					"The following environment portion of the search pattern contains too many 'one or more' symbols. " +
					"Only one is allowed and it must be at the end.")] = null;
			}

			if (count == 1 && !followingEnv.EndsWith("+"))
			{
				Errors[App.GetString("PhoneticSearchingMessages.MisplacedOneOrMoreSymbolInFollowingEnvironmentMsg",
					"The following environment portion of the search pattern contains a misplaced 'one or more' symbol. " +
					"It must be at the end.")] = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols(string pattern,
			Func<string, Match> groupingSymbolRegexProvider, string errorMsg)
		{
			var match = groupingSymbolRegexProvider(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");

				if (bracketedText == string.Empty)
				{
					Errors[string.Format(errorMsg, pattern)] = null;
					return;
				}

				match = match.NextMatch();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void VerifyMatchingOpenAndCloseSymbols(string pattern, char open, char close,
			string errorMsg)
		{
			if (pattern.Count(c => c == open) != pattern.Count(c => c == close))
				Errors[errorMsg] = null;
		}

		/// ------------------------------------------------------------------------------------
		public void VerifyTextInSquareBrackets(string pattern)
		{
			var match = PatternParser.FindInnerMostSquareBracketPairs(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");

				if (bracketedText != string.Empty &&
					!bracketedText.Contains(App.DottedCircle) &&
					bracketedText != "C" && bracketedText != "V" &&
					!App.AFeatureCache.Keys.Any(f => f == bracketedText) &&
					!App.BFeatureCache.Keys.Any(f => f == bracketedText))
				{
					var msg = App.GetString("PhoneticSearchingMessages.InvalidTextInSquareBracketsMsg",
						"The text '{0}' in square brackets is invalid. Other than surrounding " +
						"'AND' groups, square brackets are only used to surround descriptive or " +
						"distinctive features, the designators for consonant or vowel classes " +
						"('[C]' and '[V]'), or a diacritic placeholder with its diacritics and wildcards.");

					Errors[string.Format(msg, bracketedText)] = null;
				}

				match = match.NextMatch();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void VerifyTextInAngleBrackets(string pattern)
		{
			var match = PatternParser.FindInnerAngleBracketPairs(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");

				if (!_project.SearchClasses.Any(c => c.Name == bracketedText))
				{
					var msg = App.GetString("PhoneticSearchingMessages.InvalidTextInAngleBracketsMsg",
						"The text '{0}' in angled brackets is not a valid class name.");

					Errors[string.Format(msg, bracketedText)] = null;
				}

				match = match.NextMatch();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void VerifyOneDiacriticPlaceholderPerANDGroup(string pattern)
		{
			var originalPattern = pattern;

			var match = PatternParser.FindInnerMostSquareBracketPairs(pattern);
			while (match.Success)
			{
				var newPattern = pattern.Substring(0, match.Index) +
					(match.Value.Contains(App.DottedCircleC) ? '$' : '@');
				
				newPattern += new string('@', match.Length - 1);
				if (match.Index + match.Length < pattern.Length)
					newPattern += pattern.Substring(match.Index + match.Length);

				pattern = newPattern;
				match = match.NextMatch();
			}

			pattern = pattern.Replace("@", string.Empty);
			match = PatternParser.FindInnerMostSquareBracketPairs(pattern);

			while (match.Success)
			{
				if (match.Value.Count(c => c == '$') > 1)
				{
					var msg = App.GetString("PhoneticSearchingMessages.TooManyDiacriticPlaceholderMsg",
						"The pattern '{0}' contains too many diacritic placeholders in one of the AND groups. " +
						"Only one diacritic placeholder is allowed per AND group.");

					Errors[string.Format(msg, originalPattern)] = null;
					return;
				}

				match = match.NextMatch();
			}
		}
		
		/// ------------------------------------------------------------------------------------
		public void VerifyDiacriticPlaceholders(string pattern)
		{
			var match = PatternParser.FindInnerMostSquareBracketPairs(pattern);
			
			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");
				if (bracketedText.Contains(App.DottedCircleC))
				{
					bracketedText = bracketedText.Replace(App.DottedCircle, string.Empty);
					if (bracketedText.Contains("+") && bracketedText.Contains("*"))
					{
						Errors[App.GetString("PhoneticSearchingMessages.InvalidDiacriticPlaceholderSyntaxMsg",
							"The symbols '*' and '+' may not appear between square brackets together " +
							"with a diacritic placeholder. One or the other is allowed, but not both.")] = null;
					}

					if (bracketedText.Count(s => s == '+') > 1)
					{
						Errors[App.GetString("PhoneticSearchingMessages.TooManyOneOrMoreSymbolsInDiacriticPlaceholderMsg",
							"The One-Or-More-Diacritics symbol (+) appears too many times with a diacritic placeholder. Only one is allowed.")] = null;
					}

					if (bracketedText.Count(s => s == '*') > 1)
					{
						Errors[App.GetString("PhoneticSearchingMessages.TooManyZeroOrMoreSymbolsInDiacriticPlaceholderMsg",
							"The Zero-Or-More-Diacritics symbol (*) appears too many times with a diacritic placeholder. Only one is allowed.")] = null;
					}

					foreach (var symbolInfo in bracketedText.Where(s => s != '+' && s != '*').Select(s => App.IPASymbolCache[s]))
					{
						if (symbolInfo != null && symbolInfo.IsBase)
						{
							Errors[App.GetString("PhoneticSearchingMessages.InvalidSymbolInDiacriticPlaceholderMsg",
								"The symbol '{0}' is a base character and was found between square brackets " +
								"together with a diacritic placeholder. Base characters are not allowed with " +
								"diacritic placeholders.")] = null;
						}
					}
				}

				match = match.NextMatch();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void ValidateOrGroups(string item)
		{
			var orGroupValidator = new OrGroupValidator(_project);
			var match = PatternParser.FindInnerMostBracesPair(item);

			while (match.Success)
			{
				orGroupValidator.Verify(match.Value);
				if (orGroupValidator.HasErrors)
				{
					foreach (var kvp in orGroupValidator.Errors)
						Errors[kvp.Key] = kvp.Value;
				}

				match = match.NextMatch();
			}
		}
	}
}
