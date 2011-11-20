using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	public class PhoneticSearcher
	{
		public const string kBracketingError = "BRACKETING-ERROR";

		private readonly PaProject _project;
		private readonly SearchQuery _query;
		private PatternParser _parser;

		public WordListCache ResultCache { get; private set; }
		public List<string> Errors { get; private set; }
		public Action ReportProgressAction { get; set; }

		/// ------------------------------------------------------------------------------------
		public PhoneticSearcher(PaProject project, SearchQuery query)
		{
			_project = project;
			_query = query;
			Errors = new List<string>();
		}

		#region Methods for parsing search query pattern
		/// ------------------------------------------------------------------------------------
		public bool Parse()
		{
			if (!DoesPatternPassBasicChecks())
				return false;

			_parser = new PatternParser(_project, _query);

			if (_parser.Parse())
				return true;

			Errors.AddRange(_parser.Errors);
			return false;
		}

		#endregion

		#region Methods for searching word list cache
		/// ------------------------------------------------------------------------------------
		public bool Search(bool returnCountOnly, out int resultCount)
		{
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
			int matchCount = 0;
			int startIndex = 0;

			while (true)
			{
				int matchIndex;
				int numberOfPhonesInMatch;

				if (!_parser.SearchItemPatternMember.GetSearchItemMatch(phonesInWord, startIndex,
					out matchIndex, out numberOfPhonesInMatch))
				{
					return matchCount;
				}

				startIndex = matchIndex + 1;

				if (_parser.PrecedingPatternMember.GetEnvironmentMatch(phonesInWord.Take(matchIndex)) &&
					_parser.FollowingPatternMember.GetEnvironmentMatch(phonesInWord.Skip(matchIndex + numberOfPhonesInMatch)))
				{
					matchCount++;
					if (!returnCountOnly)
						addCacheEntryAction(matchIndex, numberOfPhonesInMatch);
				}
			}
		}

		#endregion

		#region Methods for verifying validity of search query pattern and building error string
		/// ------------------------------------------------------------------------------------
		public bool DoesPatternPassBasicChecks()
		{
			var checksPassed = true;

			if (_query.SearchItem.Count(c => "#*+".Contains(c)) > 0)
			{
				Errors.Add(App.GetString("PhoneticSearchingMessages.InvalidCharactersInSearchItem",
					"The search item portion of the search pattern contain an illegal symbol. " +
					"The symbols '#', '+' and '*' are not valid in the search item."));
			}

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
