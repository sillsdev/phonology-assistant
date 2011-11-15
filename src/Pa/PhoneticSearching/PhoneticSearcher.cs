using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class PhoneticSearcher
	{
		private readonly PaProject _project;
		private readonly SearchQuery _query;
		private Regex _regExpFollowingEnv;
		private Regex _regExpPrecedingEnv;
		private Regex _regExpSrchItem;
		private bool _precedingEnvHasZeroOrMore;
		private bool _followingEnvHasZeroOrMore;
		private bool _precedingEnvHasOneOrMore;
		private bool _followingEnvHasOneOrMore;

		public WordListCache ResultCache { get; private set; }
		public IEnumerable<string> Errors { get; private set; }

		// TODO: Check for out of place * and +

		/// ------------------------------------------------------------------------------------
		public PhoneticSearcher(PaProject project, SearchQuery query)
		{
			_project = project;
			_query = query;
		}

		/// ------------------------------------------------------------------------------------
		public bool Parse()
		{
			var parser = new PatternParser(_project);
			var srchItemExpression = parser.Parse(_query.SearchItem, false, null);
			var precedingEnvExpression = GetPrecedingEnvironmentExpression(parser);
			var followingEnvExpression = GetFollowingEnvironmentExpression(parser);
			
			if (parser.HasErrors)
			{
				Errors = parser.Errors;
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
		public string GetPrecedingEnvironmentExpression(PatternParser parser)
		{
			if (_query.PrecedingEnvironment == "*")
			{
				_precedingEnvHasZeroOrMore = true;
				return null;
			}

			if (_query.PrecedingEnvironment.StartsWith("+"))
				_precedingEnvHasOneOrMore = true;

			return parser.Parse(_query.PrecedingEnvironment, false, null);
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

			return parser.Parse(_query.FollowingEnvironment, false, null);
		}

		/// ------------------------------------------------------------------------------------
		public bool Search(bool returnCountOnly, bool showErrMsg, out int resultCount)
		{
			resultCount = 0;

			ResultCache = (returnCountOnly ? null : new WordListCache());

			foreach (var wentry in _project.WordCache)
			{
				var phonetic = wentry.PhoneticValueWithPrimaryUncertainty;
				
				var srchItemMatches = _regExpSrchItem.Matches(phonetic);
				if (srchItemMatches.Count == 0)
					continue;

				foreach (Match match in srchItemMatches.Cast<Match>()
					.Where(match => GetDoesPrecedingMatch(phonetic, match.Index) &&
						GetDoesFollowingMatch(phonetic, match.Index + match.Length)))
				{
					int phoneIndex;
					int numberOfPhonesInMatch;
					GetMatchInfoRelativeToPhoneArray(wentry.Phones, match.Index, match.Length,
						out phoneIndex, out numberOfPhonesInMatch);

					ResultCache.Add(wentry, wentry.Phones, phoneIndex, numberOfPhonesInMatch, true);
				}
			}

			return true;
		}

		// ------------------------------------------------------------------------------------
		public void GetMatchInfoRelativeToPhoneArray(string[] phones, int matchIndex, int matchLength,
			out int phoneIndex, out int numberOfPhonesInMatch)
		{
			phoneIndex = 0;
			numberOfPhonesInMatch = 0;

			// Translate the match index to the index of the phone.
			int accumulatedLength = 0;

			for (; phoneIndex < phones.Length; phoneIndex++)
			{
				if (accumulatedLength > matchIndex)
				{
					phoneIndex--;
					break;
				}

				accumulatedLength += phones[phoneIndex].Length;
			}

			// Calcluate how many phones in the match.
			accumulatedLength = phones[phoneIndex].Length;

			for (int i = phoneIndex + 1; i < phones.Length; i++)
			{
				numberOfPhonesInMatch++;
				if (accumulatedLength >= matchLength)
					break;

				accumulatedLength += phones[i].Length;
			}
		}

		// ------------------------------------------------------------------------------------
		private bool GetDoesPrecedingMatch(string word, int indexOfSrchItemMatch)
		{
			if (_precedingEnvHasZeroOrMore)
				return true;

			return _regExpPrecedingEnv.Matches(word).Cast<Match>()
				.Any(m => m.Index + m.Length == indexOfSrchItemMatch &&
					(!_precedingEnvHasOneOrMore || m.Index> 0));
		}

		// ------------------------------------------------------------------------------------
		private bool GetDoesFollowingMatch(string word, int startIndex)
		{
			//"(a|b){ }?(x|y)" //  a x  b y 

			if (_followingEnvHasZeroOrMore)
				return true;

			return _regExpFollowingEnv.Matches(word, startIndex).Cast<Match>()
				.Any(m => m.Index == startIndex &&
					(!_followingEnvHasOneOrMore || m.Index + m.Length < word.Length - 1));
		}

	}
}
