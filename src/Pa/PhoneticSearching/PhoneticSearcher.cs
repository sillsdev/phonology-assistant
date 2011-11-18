using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.UI.Dialogs;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class PhoneticSearcher
	{
		public const string kBracketingError = "BRACKETING-ERROR";

		private readonly PaProject _project;
		private readonly SearchQuery _query;
		private readonly bool _precedingEnvHasOnlyWordBoundary;
		private readonly bool _followingEnvHasOnlyWordBoundary;
		private bool _precedingEnvHasZeroOrMore;
		private bool _followingEnvHasZeroOrMore;
		private bool _precedingEnvHasOneOrMore;
		private bool _followingEnvHasOneOrMore;
		private Regex _regExpFollowingEnv;
		private Regex _regExpPrecedingEnv;
		private Regex _regExpSrchItem;
		private RegularExpressionSearchDebugDlg _debugDlg;

		public WordListCache ResultCache { get; private set; }
		public List<string> Errors { get; private set; }
		public Action ReportProgressAction { get; set; }

		/// ------------------------------------------------------------------------------------
		public PhoneticSearcher(PaProject project, SearchQuery query)
		{
			_project = project;
			_query = query;
			_precedingEnvHasOnlyWordBoundary = (_query.PrecedingEnvironment == "#");
			_followingEnvHasOnlyWordBoundary = (_query.FollowingEnvironment == "#");
			Errors = new List<string>();
		}

		#region Methods for parsing search query pattern
		/// ------------------------------------------------------------------------------------
		public bool Parse()
		{
			if (!DoesPatternPassBasicChecks())
				return false;

			var parser = new PatternParser(_project);
			var srchItemExpression = GetSearchItemExpression(parser);
			var precedingEnvExpression = GetPrecedingEnvironmentExpression(parser);
			var followingEnvExpression = GetFollowingEnvironmentExpression(parser);

			if (Errors.Count > 0)
				return false;

			if (parser.HasErrors)
			{
				Errors.AddRange(parser.Errors);
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
				Errors.Add(App.GetString("PhoneticSearchingMessages.InvalidCharactersInSearchItem",
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

			return parser.Parse(_query.PrecedingEnvironment, PatternEnvironment.Preceding,
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

			return parser.Parse(_query.FollowingEnvironment, PatternEnvironment.Following,
				_query.IgnoreDiacritics, _query.GetIgnoredCharacters());
		}

		#endregion

		#region Methods for searching word list cache
		/// ------------------------------------------------------------------------------------
		public bool Search(bool returnCountOnly, out int resultCount)
		{
			_debugDlg = Application.OpenForms.OfType<RegularExpressionSearchDebugDlg>().FirstOrDefault();

			if (_debugDlg != null)
				_debugDlg.LoadExpressions(_query, _regExpSrchItem, _regExpPrecedingEnv, _regExpFollowingEnv);

			ResultCache = (returnCountOnly ? null : new WordListCache());
			resultCount = 0;

			foreach (var wentry in _project.WordCache)
			{
				if (ReportProgressAction != null)
					ReportProgressAction();

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
				if (!match.Success)
					return matchCount;

				startIndex = match.Index + 1;

				// Get the index of the matched phone in the phone collection and then build
				// a string that contains only the phones preceding the match. That will be
				// scanned for a match against the preceding environment pattern.
				int phoneIndex = TranslateMatchIndexToPhoneIndex(phonesInWord, match.Index);
				var environment = string.Join(string.Empty, phonesInWord, 0, phoneIndex);
				if (!GetDoesMatchPrecedingEnvironment(environment))
					continue;

				// Get the number of phones in the search item match and then build a string
				// that contains only the phones following the match. That will be scanned
				// for a match against the following environment pattern.
				int numberOfPhonesInMatch = TranslateMatchLengthToNumberOfPhones(phonesInWord, phoneIndex, match.Length);
				environment = (phoneIndex + numberOfPhonesInMatch == phonesInWord.Length ? string.Empty :
					string.Join(string.Empty, phonesInWord, phoneIndex + numberOfPhonesInMatch,
						phonesInWord.Length - (phoneIndex + numberOfPhonesInMatch)));

				if (!GetDoesMatchFollowingEnvironment(environment))
					continue;

				matchCount++;
				if (returnCountOnly)
					continue;

				addCacheEntryAction(phoneIndex, numberOfPhonesInMatch);

				if (_debugDlg != null)
					_debugDlg.LoadMatch(phoneticWord, match);
			}
		}

		// ------------------------------------------------------------------------------------
		private bool GetDoesMatchPrecedingEnvironment(string phonesPrecedingSrchItemMatch)
		{
			if (_precedingEnvHasZeroOrMore)
				return true;

			// If the preceding environment only contains the word boundary symbol, then
			// we can handle that without using a regular expression match. The process
			// is to strip out all the ignored characters and if there's nothing left
			// then the preceding environment matches the query pattern.
			if (_precedingEnvHasOnlyWordBoundary)
				return GetIsAnythingLeftAfterRemovingIgnoredCharacters(phonesPrecedingSrchItemMatch);

			return _regExpPrecedingEnv.Matches(phonesPrecedingSrchItemMatch).Cast<Match>()
				.Any(m => m.Index + m.Length == phonesPrecedingSrchItemMatch.Length &&
					(!_precedingEnvHasOneOrMore || m.Index > 0));
		}

		// ------------------------------------------------------------------------------------
		private bool GetDoesMatchFollowingEnvironment(string phonesFollowingSrchItemMatch)
		{
			if (_followingEnvHasZeroOrMore)
				return true;

			// If the following environment only contains the word boundary symbol, then
			// we can handle that without using a regular expression match. The process
			// is to strip out all the ignored characters and if there's nothing left
			// then the following environment matches the query pattern.
			if (_followingEnvHasOnlyWordBoundary)
				return GetIsAnythingLeftAfterRemovingIgnoredCharacters(phonesFollowingSrchItemMatch);

			return _regExpFollowingEnv.Matches(phonesFollowingSrchItemMatch).Cast<Match>().Any(m => m.Index == 0 &&
				(!_followingEnvHasOneOrMore || m.Index + m.Length < phonesFollowingSrchItemMatch.Length - 1));
		}

		// ------------------------------------------------------------------------------------
		public bool GetIsAnythingLeftAfterRemovingIgnoredCharacters(string phones)
		{
			if (phones.Length == 0)
				return true;

			phones = _query.GetIgnoredCharacters()
				.Aggregate(phones, (curr, ignoredSymbol) => curr.Replace(ignoredSymbol, string.Empty));

			return (phones.Length == 0);
		}

		// ------------------------------------------------------------------------------------
		public int TranslateMatchIndexToPhoneIndex(string[] phonesInWord, int matchIndex)
		{
			int phoneIndex = 0;
			int accumulatedLength = 0;

			for (; phoneIndex < phonesInWord.Length && accumulatedLength < matchIndex; phoneIndex++)
				accumulatedLength += phonesInWord[phoneIndex].Length;

			return phoneIndex;
		}

		// ------------------------------------------------------------------------------------
		public int TranslateMatchLengthToNumberOfPhones(string[] phonesInWord, int phoneIndex,
			int matchLength)
		{
			var numberOfPhonesInMatch = 0;
			var accumulatedLength = 0;

			for (int i = phoneIndex; i < phonesInWord.Length && accumulatedLength < matchLength; i++)
			{
				numberOfPhonesInMatch++;
				accumulatedLength += phonesInWord[i].Length;
			}

			return numberOfPhonesInMatch;
		}

		#endregion

		#region Methods for verifying validity of search query pattern and building error string
		/// ------------------------------------------------------------------------------------
		public bool DoesPatternPassBasicChecks()
		{
			var checksPassed = true;

			if (_query.PrecedingEnvironment.Count(c => "#*+".Contains(c)) > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.InvalidCharactersInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains an illegal combination of characters. " +
					"The symbols '#', '+' or '*' are not allowed together in the preceding environment."));
			}

			if (_query.FollowingEnvironment.Count(c => "#*+".Contains(c)) > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.InvalidCharactersInFollowingEnvironment",
					"The following environment portion of the search pattern contains an illegal combination of characters. " +
					"The symbols '#', '+' or '*' are not allowed together in the following environment."));
			}

			int count = _query.PrecedingEnvironment.Count(c => c == '#');
			if (count > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.TooManyWordBoundarySymbolsInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains too many word boundary symbols. " +
					"Only one is allowed and it must be at the beginning."));
			}

			if (count == 1 && !_query.PrecedingEnvironment.StartsWith("#"))
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedWordBoundarySymbolInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains a misplaced word boundary symbol. " +
					"It must be at the beginning."));
			}

			count = _query.FollowingEnvironment.Count(c => c == '#');
			if (count > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.TooManyWordBoundarySymbolsInFollowingEnvironment",
					"The following environment portion of the search pattern contains too many word boundary symbols. " +
					"Only one is allowed and it must be at the end."));
			}

			if (count == 1 && !_query.FollowingEnvironment.EndsWith("#"))
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedWordBoundarySymbolInFollowingEnvironment",
					"The following environment portion of the search pattern contains a misplaced word boundary symbol. " +
					"It must be at the end."));
			}

			count = _query.PrecedingEnvironment.Count(c => c == '*');
			if (count > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.TooManyZeroOrMoreSymbolsInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains too many 'zero or more' symbols. " +
					"Only one is allowed and it must be at the beginning."));
			}

			if (count == 1 && !_query.PrecedingEnvironment.StartsWith("*"))
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedZeroOrMoreSymbolInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains a misplaced 'zero or more' symbol. " +
					"It must be at the beginning."));
			}

			count = _query.FollowingEnvironment.Count(c => c == '*');
			if (count > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.TooManyZeroOrMoreSymbolsInFollowingEnvironment",
					"The following environment portion of the search pattern contains too many 'zero or more' symbols. " +
					"Only one is allowed and it must be at the end."));
			}

			if (count == 1 && !_query.FollowingEnvironment.EndsWith("*"))
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedZeroOrMoreSymbolInFollowingEnvironment",
					"The following environment portion of the search pattern contains a misplaced 'zero or more' symbol. " +
					"It must be at the end."));
			}

			count = _query.PrecedingEnvironment.Count(c => c == '+');
			if (count > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.TooManyOneOrMoreSymbolsInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains too many 'one or more' symbols. " +
					"Only one is allowed and it must be at the beginning."));
			}

			if (count == 1 && !_query.PrecedingEnvironment.StartsWith("+"))
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedOneOrMoreSymbolInPrecedingEnvironment",
					"The preceding environment portion of the search pattern contains a misplaced 'one or more' symbol. " +
					"It must be at the beginning."));
			}

			count = _query.FollowingEnvironment.Count(c => c == '+');
			if (count > 1)
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.TooManyOneOrMoreSymbolsInFollowingEnvironment",
					"The following environment portion of the search pattern contains too many 'one or more' symbols. " +
					"Only one is allowed and it must be at the end."));
			}

			if (count == 1 && !_query.FollowingEnvironment.EndsWith("+"))
			{
				checksPassed = false;
				Errors.Add(App.GetString("PhoneticSearchingMessages.MisplacedOneOrMoreSymbolInFollowingEnvironment",
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
			if (Errors.Count == 0)
				return null;

			var bracketingError = Errors.FirstOrDefault(msg => msg.StartsWith(kBracketingError));
			if (bracketingError != null)
				return bracketingError;

			var errors = new StringBuilder();
			foreach (var err in Errors)
			{
				errors.Append(err);
				errors.Append(separateErrorsWithLineBreaks ? Environment.NewLine : " ");
			}

			var fmt = App.GetString("PhoneticSearchingMessages.GeneralErrorOverviewMsg",
				"The following error(s) occurred using the search pattern:\n\n{0}");

			return string.Format(fmt, errors.ToString().Trim());
		}

		#endregion
	}
}
