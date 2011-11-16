using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class PhoneticSearcher
	{
		public const string kBracketingError = "BRACKETING-ERROR";

		private readonly PaProject _project;
		private readonly SearchQuery _query;
		private readonly List<string> _errors;
		private Regex _regExpFollowingEnv;
		private Regex _regExpPrecedingEnv;
		private Regex _regExpSrchItem;
		private bool _precedingEnvHasZeroOrMore;
		private bool _followingEnvHasZeroOrMore;
		private bool _precedingEnvHasOneOrMore;
		private bool _followingEnvHasOneOrMore;

		public WordListCache ResultCache { get; private set; }

		/// ------------------------------------------------------------------------------------
		public PhoneticSearcher(PaProject project, SearchQuery query)
		{
			_project = project;
			_query = query;
			_errors = new List<string>();
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> Errors
		{
			get { return _errors; }
		}

		/// ------------------------------------------------------------------------------------
		public bool Parse()
		{
			if (!DoesPatternPassBasicChecks())
				return false;

			var parser = new PatternParser(_project);
			var srchItemExpression = GetSearchItemExpression(parser);
			var precedingEnvExpression = GetPrecedingEnvironmentExpression(parser);
			var followingEnvExpression = GetFollowingEnvironmentExpression(parser);

			if (_errors.Count > 0)
				return false;

			if (parser.HasErrors)
			{
				_errors.AddRange(parser.Errors);
				return false;
			}

			_regExpSrchItem = new Regex(srchItemExpression);
			
			if (!_precedingEnvHasZeroOrMore)
				_regExpPrecedingEnv = new Regex(precedingEnvExpression);
	
			if (!_followingEnvHasZeroOrMore)
				_regExpFollowingEnv = new Regex(followingEnvExpression);
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public string GetSearchItemExpression(PatternParser parser)
		{
			var expression =  parser.Parse(_query.SearchItem,
				_query.IgnoreDiacritics, _query.GetIgnoredCharacters());

			if (expression.Count(c => "#*+".Contains(c)) > 0)
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidCharactersInSearchItem",
					"The search item portion of the search pattern contains illegal symbols. " +
					"The symbols '#', '+' and '*' are not valid."));
			}

			return expression;
		}

		/// ------------------------------------------------------------------------------------
		public string GetPrecedingEnvironmentExpression(PatternParser parser)
		{
			if (_query.PrecedingEnvironment == "*")
			{
				_precedingEnvHasZeroOrMore = true;
				return null;
			}

			if (_query.PrecedingEnvironment.StartsWith("+"))
				_precedingEnvHasOneOrMore = true;

			return parser.Parse(_query.PrecedingEnvironment,
				_query.IgnoreDiacritics, _query.GetIgnoredCharacters());
		}

		/// ------------------------------------------------------------------------------------
		public string GetFollowingEnvironmentExpression(PatternParser parser)
		{
			if (_query.FollowingEnvironment == "*")
			{
				_followingEnvHasZeroOrMore = true;
				return null;
			}
		
			if (_query.FollowingEnvironment.EndsWith("+"))
				_followingEnvHasOneOrMore = true;

			return parser.Parse(_query.FollowingEnvironment,
				_query.IgnoreDiacritics, _query.GetIgnoredCharacters());
		}

		/// ------------------------------------------------------------------------------------
		public bool Search(bool returnCountOnly, out int resultCount)
		{
			ResultCache = (returnCountOnly ? null : new WordListCache());
			resultCount = 0;

			foreach (var wentry in _project.WordCache)
			{
				// Get a list of word(s) for this entry. If the query's IncludeAllUncertainPossibilities
				// flag is set, there will be multiple words, one for each uncertain possibility.
				// Otherwise there will be only one. Each word is delivered in the form of an array of
				// phones.
				var wordsList = (_query.IncludeAllUncertainPossibilities && wentry.ContiansUncertainties ?
					wentry.GetAllPossibleUncertainWords(false) : new[] { wentry.Phones });

				foreach (var phonesInWord in wordsList)
				{
					resultCount = SearchWord(phonesInWord, returnCountOnly,
						(phoneIndexOfMatch, numberOfPhonesInMatch) =>
							ResultCache.Add(wentry, phonesInWord, phoneIndexOfMatch, numberOfPhonesInMatch, true));
				}
			}
			
			return true;
		}

		// ------------------------------------------------------------------------------------
		public int SearchWord(string[] phonesInWord, bool returnCountOnly,
			Action<int, int> addCacheEntryAction)
		{
			var phoneticWord = string.Join(string.Empty, phonesInWord);
			int matchCount = 0;
			int startIndex = 0;

			while (true)
			{
				var match = _regExpSrchItem.Match(phoneticWord, startIndex);
				if (!match.Success || 
					!GetDoesMatchPrecedingEnvironment(phoneticWord, match.Index) ||
					!GetDoesMatchFollowingEnvironment(phoneticWord, match.Index + match.Length))
				{
					return matchCount;
				}

				matchCount++;
				startIndex = match.Index + 1;

				if (returnCountOnly)
					continue;

				int phoneIndex = TranslateMatchIndexToPhoneIndex(phonesInWord, match.Index);
				int numberOfPhonesInMatch = TranslateMatchLengthToNumberOfPhones(phonesInWord, phoneIndex, match.Length);
				addCacheEntryAction(phoneIndex, numberOfPhonesInMatch);
			}
		}

		// ------------------------------------------------------------------------------------
		public int TranslateMatchIndexToPhoneIndex(string[] phonesInWord, int matchIndex)
		{
			int phoneIndex = 0;
			int accumulatedLength = 0;

			for (; phoneIndex < phonesInWord.Length; phoneIndex++)
			{
				if (accumulatedLength > matchIndex)
					break;

				accumulatedLength += phonesInWord[phoneIndex].Length;
			}

			if (phoneIndex > 0)
				phoneIndex--;

			return phoneIndex;
		}

		// ------------------------------------------------------------------------------------
		public int TranslateMatchLengthToNumberOfPhones(string[] phonesInWord, int phoneIndex,
			int matchLength)
		{
			var numberOfPhonesInMatch = 0;
			var accumulatedLength = phonesInWord[phoneIndex].Length;

			for (int i = phoneIndex + 1; i < phonesInWord.Length; i++)
			{
				numberOfPhonesInMatch++;
				if (accumulatedLength >= matchLength)
					break;

				accumulatedLength += phonesInWord[i].Length;
			}

			if (numberOfPhonesInMatch == 0)
				numberOfPhonesInMatch = 1;

			return numberOfPhonesInMatch;
		}

		// ------------------------------------------------------------------------------------
		private bool GetDoesMatchPrecedingEnvironment(string word, int indexOfSrchItemMatch)
		{
			if (_precedingEnvHasZeroOrMore)
				return true;

			return _regExpPrecedingEnv.Matches(word).Cast<Match>()
				.Any(m => m.Index + m.Length == indexOfSrchItemMatch &&
					(!_precedingEnvHasOneOrMore || m.Index> 0));
		}

		// ------------------------------------------------------------------------------------
		private bool GetDoesMatchFollowingEnvironment(string word, int startIndex)
		{
			if (_followingEnvHasZeroOrMore)
				return true;

			return _regExpFollowingEnv.Matches(word, startIndex).Cast<Match>()
				.Any(m => m.Index == startIndex &&
					(!_followingEnvHasOneOrMore || m.Index + m.Length < word.Length - 1));
		}

		/// ------------------------------------------------------------------------------------
		public bool DoesPatternPassBasicChecks()
		{
			var checksPassed = true;

			if (_query.PrecedingEnvironment.Count(c => "#*+".Contains(c)) > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidCharactersInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains an illegal combination of characters. " +
					"The symbols '#', '+' or '*' are not allowed together in the preceding environment."));
			}

			if (_query.FollowingEnvironment.Count(c => "#*+".Contains(c)) > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidCharactersInFollowingEnvironment",
					"The following environment portion of the search pattern contains an illegal combination of characters. " +
					"The symbols '#', '+' or '*' are not allowed together in the following environment."));
			}

			int count = _query.PrecedingEnvironment.Count(c => c == '#');
			if (count > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyWordBoundarySymbolsInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains too many word boundary symbols. " +
					"Only one is allowed and it must be at the beginning."));
			}

			if (count == 1 && !_query.PrecedingEnvironment.StartsWith("#"))
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedWordBoundarySymbolInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains a misplaced word boundary symbol. " +
					"It must be at the beginning."));
			}

			count = _query.FollowingEnvironment.Count(c => c == '#');
			if (count > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyWordBoundarySymbolsInFollowingEnvironment",
					"The following environment portion of the search pattern contains too many word boundary symbols. " +
					"Only one is allowed and it must be at the end."));
			}

			if (count == 1 && !_query.FollowingEnvironment.EndsWith("#"))
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedWordBoundarySymbolInFollowingEnvironment",
					"The following environment portion of the search pattern contains a misplaced word boundary symbol. " +
					"It must be at the end."));
			}

			count = _query.PrecedingEnvironment.Count(c => c == '*');
			if (count > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyZeroOrMoreSymbolsInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains too many 'zero or more' symbols. " +
					"Only one is allowed and it must be at the beginning."));
			}

			if (count == 1 && !_query.PrecedingEnvironment.StartsWith("*"))
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedZeroOrMoreSymbolInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains a misplaced 'zero or more' symbol. " +
					"It must be at the beginning."));
			}

			count = _query.FollowingEnvironment.Count(c => c == '*');
			if (count > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyZeroOrMoreSymbolsInFollowingEnvironment",
					"The following environment portion of the search pattern contains too many 'zero or more' symbols. " +
					"Only one is allowed and it must be at the end."));
			}

			if (count == 1 && !_query.FollowingEnvironment.EndsWith("*"))
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedZeroOrMoreSymbolInFollowingEnvironment",
					"The following environment portion of the search pattern contains a misplaced 'zero or more' symbol. " +
					"It must be at the end."));
			}

			count = _query.PrecedingEnvironment.Count(c => c == '+');
			if (count > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyOneOrMoreSymbolsInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains too many 'one or more' symbols. " +
					"Only one is allowed and it must be at the beginning."));
			}

			if (count == 1 && !_query.PrecedingEnvironment.StartsWith("+"))
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedOneOrMoreSymbolInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains a misplaced 'one or more' symbol. " +
					"It must be at the beginning."));
			}

			count = _query.FollowingEnvironment.Count(c => c == '+');
			if (count > 1)
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyOneOrMoreSymbolsInFollowingEnvironment",
					"The following environment portion of the search pattern contains too many 'one or more' symbols. " +
					"Only one is allowed and it must be at the end."));
			}

			if (count == 1 && !_query.FollowingEnvironment.EndsWith("+"))
			{
				checksPassed = false;
				_errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedOneOrMoreSymbolInFollowingEnvironment",
					"The following environment portion of the search pattern contains a misplaced 'one or more' symbol. " +
					"It must be at the end."));
			}

			return checksPassed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines the list of error messages into a single message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCombinedErrorMessages(bool separateErrorsWithLineBreaks)
		{
			if (_errors.Count == 0)
				return null;

			var bracketingError = _errors.FirstOrDefault(msg => msg.StartsWith(kBracketingError));
			if (bracketingError != null)
				return bracketingError;

			var errors = new StringBuilder();
			foreach (var err in _errors)
			{
				errors.Append(err);
				errors.Append(separateErrorsWithLineBreaks ? Environment.NewLine : " ");
			}

			var fmt = App.GetString("PhoneticSearchingMessages.GeneralErrorOverviewMsg",
				"The following error(s) occurred when parsing the search pattern:\n\n{0}");

			return string.Format(fmt, errors.ToString().Trim());
		}
	}
}
