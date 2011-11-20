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
		private readonly List<string> _errors = new List<string>();
		private readonly PaProject _project;
		private readonly SearchQuery _query;

		public PatternMember FollowingPatternMember { get; private set; }
		public PatternMember PrecedingPatternMember { get; private set; }
		public PatternMember SearchItemPatternMember { get; private set; }

		private const char kMinToken = (char)(char.MaxValue - 256);
		private char _token = kMinToken;
		private readonly Dictionary<char, List<string>> _tokenGroups =
			new Dictionary<char, List<string>>();

		// TODO: Check for empty pairs (i.e. <> {} []) and mismatched pairs.

		/// ------------------------------------------------------------------------------------
		public PatternParser(PaProject project, SearchQuery query)
		{
			_project = project;
			_query = query;
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
		public bool Parse()
		{
			var ignoredCharsList = _query.GetIgnoredCharacters().ToList();

			var srchItemPattern = _query.SearchItem;
			var precedingPattern = _query.PrecedingEnvironment;
			var followingPattern = _query.FollowingEnvironment;

			SearchItemPatternMember = new PatternMember(PatternPart.SearchItem, _query.IgnoreDiacritics, ignoredCharsList);
			PrecedingPatternMember = new PatternMember(PatternPart.Preceding, _query.IgnoreDiacritics, ignoredCharsList);
			FollowingPatternMember = new PatternMember(PatternPart.Following, _query.IgnoreDiacritics, ignoredCharsList);

			InternalParse(srchItemPattern, SearchItemPatternMember);
			InternalParse(precedingPattern, PrecedingPatternMember);
			InternalParse(followingPattern, FollowingPatternMember);

			return !HasErrors;
		}

		/// ------------------------------------------------------------------------------------
		public void InternalParse(string pattern, PatternMember patternMember)
		{
			_errors.Clear();
			_tokenGroups.Clear();
			_token = kMinToken;

			if (!VerifyBracketedText(pattern))
				return;

			var originalPattern = pattern;
			pattern = ReplaceBracketedClassNamesWithPattern(pattern);
			pattern = ReplaceBracketedTextWithTokens(pattern);
			while (ParseTextInBrackets(ref pattern) == 1) { }
			while (ParseTextInBraces(ref pattern) == 1) { }

			pattern = pattern.Trim('+', '*');

			foreach (char chr in pattern)
			{
				if (chr < kMinToken)
					patternMember.AddSymbol(chr);
				else
					patternMember.AddPhoneGroup(_tokenGroups[chr]);
			}

			patternMember.FinalizeParse(originalPattern, _project.PhoneticParser);
		}

		#region Methods for parsing text between brackets and braces
		/// ------------------------------------------------------------------------------------
		public string ReplaceBracketedClassNamesWithPattern(string pattern)
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
		public string ReplaceBracketedTextWithTokens(string pattern)
		{
			_tokenGroups.Clear();
			var match = FindInnerMostSquareBracketPairs(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");

				if (bracketedText.Contains(App.kDottedCircle))
					_tokenGroups[++_token] = new List<string>(new[] { bracketedText });
				else if (match.Value == "[C]" || match.Value == "[V]")
					_tokenGroups[++_token] = new List<string> { match.Value };
				else
					_tokenGroups[++_token] = new List<string> { "F:" + bracketedText };

				pattern = pattern.Replace(match.Value, _token.ToString());
				match = match.NextMatch();


				//var token = string.Empty;

				//if (bracketedText.Contains(App.kDottedCircle))
				//    token = StoreDiacriticPlaceholderCluster(bracketedText);
				//else if (match.Value == "[C]")
				//{
				//    token = CreatePhoneGroupForListOfPhones((from p in _project.PhoneCache.Values
				//                                             where p.CharType == IPASymbolType.consonant
				//                                             select p.Phone).ToList());
				//}
				//else if (match.Value == "[V]")
				//{
				//    token = CreatePhoneGroupForListOfPhones((from p in _project.PhoneCache.Values
				//                                             where p.CharType == IPASymbolType.vowel
				//                                             select p.Phone).ToList());
				//}
				//else
				//    token = CreatePhoneGroupFromFeatureName(bracketedText);

				//pattern = pattern.Replace(match.Value, token);
				//match = match.NextMatch();
			}

			return pattern;
		}

		///// ------------------------------------------------------------------------------------
		//public string StoreDiacriticPlaceholderCluster(string placeholderCluster)
		//{
		//    _tokenGroups[++_token] = new List<string>(new[] { placeholderCluster });
		//    return _token.ToString();
		//}

		///// ------------------------------------------------------------------------------------
		//public string CreatePhoneGroupFromFeatureName(string featureName)
		//{
		//    var isAFeature = (!featureName.StartsWith("+") && !featureName.StartsWith("-"));

		//    var mask = (isAFeature ? App.AFeatureCache.GetMask(featureName) : App.BFeatureCache.GetMask(featureName));

		//    return CreatePhoneGroupForListOfPhones((from p in _project.PhoneCache.Values
		//                                            where (isAFeature && !p.AMask.IsEmpty && p.AMask.ContainsOneOrMore(mask, false)) ||
		//                                               (!isAFeature && !p.BMask.IsEmpty && p.BMask.ContainsOneOrMore(mask, false))
		//                                            select p.Phone).ToList());
		//}

		/// ------------------------------------------------------------------------------------
		//public string CreatePhoneGroupForListOfPhones(List<string> phoneList)
		//{
		//    if (phoneList.Count == 0)
		//        return string.Empty;

		//    _tokenGroups[++_token] = phoneList;
		//    return _token.ToString();
		//}

		/// ------------------------------------------------------------------------------------
		public int ParseTextInBrackets(ref string pattern)
		{
			var match = FindInnerMostSquareBracketPairs(pattern);
			if (!match.Success)
				return 0;

			while (match.Success)
			{
				var andList = new List<string>();
				var bracketedText = match.Result("${bracketedText}");
				while (ParseTextInBraces(ref bracketedText) == 1) { }

				var symbolsInBracketedText = string.Empty;

				foreach (var chr in bracketedText)
				{
					if (chr <= kMinToken)
						symbolsInBracketedText += chr;
					else
					{
						andList = AndTwoTokenGroups(andList, _tokenGroups[chr]).ToList();
						_tokenGroups.Remove(chr);
					}
				}

				if (symbolsInBracketedText != string.Empty)
				{
					// If plain text (i.e. not features or classes) is found between the square brackets,
					// then if it represents a single phone, that's fine. Otherwise, it's an error.
					// This deals with patterns like "[b[0*]]" (where 0 is the diacritic placeholder).
					var phonesInBrackets = _project.PhoneticParser.Parse(symbolsInBracketedText, true, false);
					if (phonesInBrackets.Length == 1 && andList.Count == 1 && andList[0].Contains(App.kDottedCircle))
						andList = OrTwoTokenGroups(andList, phonesInBrackets.ToList()).ToList();
					else
					{
						var msg = App.GetString("PhoneticSearchingMessages.InvalidCharacterInANDGroup",
							"The text '{0}' is invalid in an AND group.");

						_errors.Add(string.Format(msg, symbolsInBracketedText));
						return -1;
					}
				}

				if (!VerifyDiacriticPlaceholderClusterIfPresent(andList))
					return -1;

				_tokenGroups[++_token] = andList;
				pattern = pattern.Replace(match.Value, _token.ToString());
				match = match.NextMatch();
			}

			return 1;
		}

		/// ------------------------------------------------------------------------------------
		public int ParseTextInBraces(ref string pattern)
		{
			var match = FindInnerMostBracesPair(pattern);
			if (!match.Success)
				return 0;

			while (match.Success)
			{
				var orList = new List<string>();
				var bracketedText = match.Result("${bracketedText}");
				var piecesBetweenBraces = bracketedText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				
				foreach (var piece in piecesBetweenBraces.Where(piece => GetIsOrGroupMemberValid(piece)))
				{
					if (piece[0] < kMinToken)
						orList.Add(piece.Trim('(', ')'));
					else
					{
						orList = OrTwoTokenGroups(orList, _tokenGroups[piece[0]]).ToList();
						_tokenGroups.Remove(piece[0]);
					}
				}

				_tokenGroups[++_token] = orList;
				pattern = pattern.Replace(match.Value, _token.ToString());
				match = match.NextMatch();
			}

			return 1;
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
		private bool VerifyDiacriticPlaceholderClusterIfPresent(IEnumerable<string> andList)
		{
			// Gather the diacritic placeholder clusters in their own list, tossing out the dotted circle along the way.
			var clusters = (from cluster in andList
							where cluster.Contains(App.kDottedCircle)
							select cluster.Replace(App.kDottedCircle, string.Empty)).ToList();

			if (clusters.Count > 1)
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyDiacriticPlaceholderMsg",
					"There were too many diacritic placeholders found in one of the AND groups."));

				return false;
			}

			if (clusters[0].IndexOf('+') >= 0 && clusters[0].IndexOf('*') >= 0)
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidDiacriticPlaceholderSyntax",
					"The symbols '*' and '+' may not appear between square brackets together " +
					"with a diacritic placeholder. One or the other is allowed, but not both."));

				return false;
			}

			return true;
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

		#region Methods for combining groups of toekns
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> AndTwoTokenGroups(List<string> x, List<string> y)
		{
			if (x.Count == 0)
				return y;

			// Add diacritic placeholder clusters to the list to process later.
			if (y.Count == 1 && y[0].Contains(App.kDottedCircle))
			{
				x.Add(y[0]);
				return x;
			}

			return (y.Count == 0 ? x : x.Intersect(y, StringComparer.Ordinal));
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> OrTwoTokenGroups(List<string> x, List<string> y)
		{
			if (x.Count == 0)
				return y;

			return (y.Count == 0 ? x : x.Union(y, StringComparer.Ordinal));
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
