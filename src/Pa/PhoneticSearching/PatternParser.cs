using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class PatternParser
	{
		private readonly List<string> _errors = new List<string>();
		private readonly PaProject _project;

		private const char kMinToken = (char)(char.MaxValue - 256);
		private char _token = kMinToken;
		private readonly Dictionary<char, List<string>> _phoneGroups =
			new Dictionary<char, List<string>>();

		//private readonly Dictionary<char, List<string>> _phoneGroupsSubjectToDiacriticPlaceholderPattern =
		//    new Dictionary<char, List<string>>();

		// TODO: Check for empty pairs (i.e. <> {} []) and mismatched pairs.

		/// ------------------------------------------------------------------------------------
		public PatternParser(PaProject project)
		{
			_project = project;
		}

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

		/// ------------------------------------------------------------------------------------
		public string Parse(string pattern, bool ignoreDiacritics, IEnumerable<string> ignoredCharacters)
		{
			_errors.Clear();
			_token = kMinToken;
			_phoneGroups.Clear();

			if (!VerifyBracketedText(pattern))
				return pattern;

			pattern = ReplaceBracketedClassNamesWithPattern(pattern);
			pattern = ReplaceBracketedTextWithPhoneGroups(pattern);
			while (ParseTextInBrackets(ref pattern) == 1) { }
			while (ParseTextInBraces(ref pattern) == 1) { }

			pattern = pattern.Trim('+', '*');

			var ignoredCharList = (ignoredCharacters == null ? new List<string>(0) : ignoredCharacters.ToList());
			var bldr = new StringBuilder();

			for (int i = 0; i < pattern.Length; i++)
			{
				if (pattern[i] < kMinToken)
				{
					if (pattern[i] != '+' && pattern[i] != '*')
						bldr.Append(pattern[i].ToString());
				}
				else
				{
					// Build a regular expression group containing possible phones.
					var regExpression = CreateRegExpressionOrGroupFromPhoneGroup(pattern[i], ignoreDiacritics, ignoredCharList);

					// If this is not the last character in the pattern and there are some ignored, non
					// base suprasegmentals, then make sure the regular expression indicates the non base
					// suprasegmentals that can be ignored.
					if (i < pattern.Length - 1 && ignoredCharList.Count > 0)
						regExpression += GetRegExpressionForIngoredBaseSymbols(ignoredCharList);

					bldr.Append(regExpression);
				}
			}

			pattern = bldr.ToString();

			if (pattern.StartsWith("#"))
				pattern = "^" + GetRegExpressionForIngoredBaseSymbols(ignoredCharList) + pattern.TrimStart('#');

			if (pattern.EndsWith("#"))
				pattern = pattern.TrimEnd('#') + "$";

			return pattern.Replace(".", "\\.");
		}

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
		public string ReplaceBracketedTextWithPhoneGroups(string pattern)
		{
			_phoneGroups.Clear();
			var match = FindInnerMostSquareBracketPairs(pattern);

			while (match.Success)
			{
				var bracketedText = match.Result("${bracketedText}");

				var token = string.Empty;

				if (bracketedText.Contains(App.kDottedCircle))
					token = StoreDiacriticPlaceholderCluster(bracketedText);
				else if (match.Value == "[C]")
				{
					token = CreatePhoneGroupForListOfPhones((from p in _project.PhoneCache.Values
															 where p.CharType == IPASymbolType.consonant
															 select p.Phone).ToList());
				}
				else if (match.Value == "[V]")
				{
					token = CreatePhoneGroupForListOfPhones((from p in _project.PhoneCache.Values
															 where p.CharType == IPASymbolType.vowel
															 select p.Phone).ToList());
				}
				else
					token = CreatePhoneGroupFromFeatureName(bracketedText);

				pattern = pattern.Replace(match.Value, token);
				match = match.NextMatch();
			}

			return pattern;
		}

		/// ------------------------------------------------------------------------------------
		public string StoreDiacriticPlaceholderCluster(string placeholderCluster)
		{
			_phoneGroups[++_token] = new List<string>(new[] { (placeholderCluster) });
			return _token.ToString();
		}

		/// ------------------------------------------------------------------------------------
		public string CreatePhoneGroupFromFeatureName(string featureName)
		{
			var isAFeature = (!featureName.StartsWith("+") && !featureName.StartsWith("-"));

			var mask = (isAFeature ? App.AFeatureCache.GetMask(featureName) : App.BFeatureCache.GetMask(featureName));

			return CreatePhoneGroupForListOfPhones((from p in _project.PhoneCache.Values
													where (isAFeature && !p.AMask.IsEmpty && p.AMask.ContainsOneOrMore(mask, false)) ||
													   (!isAFeature && !p.BMask.IsEmpty && p.BMask.ContainsOneOrMore(mask, false))
													select p.Phone).ToList());
		}

		/// ------------------------------------------------------------------------------------
		public string CreatePhoneGroupForListOfPhones(List<string> phoneList)
		{
			if (phoneList.Count == 0)
				return string.Empty;

			_phoneGroups[++_token] = phoneList;
			return _token.ToString();
		}

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

				foreach (var chr in bracketedText)
				{
					if (chr <= kMinToken)
					{
						var msg = App.GetString("PhoneticSearchingMessages.InvalidCharacterInANDGroup",
							"The symbol '{0}' is invalid in an AND group.");

						_errors.Add(string.Format(msg, chr));
						return -1;
					}

					andList = AndTwoPhoneGroups(andList, _phoneGroups[chr]).ToList();
					_phoneGroups.Remove(chr);
				}

				_phoneGroups[++_token] = ModifyListIfContainsDiacriticPlaceholderCluster(andList).ToList();
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
						orList = OrTwoPhoneGroups(orList, _phoneGroups[piece[0]]).ToList();
						_phoneGroups.Remove(piece[0]);
					}
				}

				_phoneGroups[++_token] = orList;
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

		#endregion

		#region Methods for handling diacritic placeholders in the pattern
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the strings in the specified list of phones for a diacritic placeholder
		/// cluster. If it finds one, it's removed from the list and then the rest of the
		/// strings in the list are scanned to determine whether or not each meets the
		/// criteria imposed by the cluster pattern. A list is returned that contains only
		/// the phones that meet the cluster's critera. Each phone returned has a '>' prefix
		/// to mark it for processing ignored diacritics later in the pattern parsing process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> ModifyListIfContainsDiacriticPlaceholderCluster(List<string> andList)
		{
			// Strip out the strings that are diacritic placeholder clusters.
			var phoneList = andList.Where(s => !s.Contains(App.kDottedCircle));

			// Gather the diacritic placeholder clusters in their own list, tossing out the dotted circle along the way.
			var placeHolderClusters = (from cluster in andList
									   where cluster.Contains(App.kDottedCircle)
									   select cluster.Replace(App.kDottedCircle, string.Empty)).ToList();

			if (placeHolderClusters.Count > 1)
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.TooManyDiacriticPlaceholderMsg",
					"There were too many diacritic placeholders found in one of the AND groups."));
				
				return new List<string>(0);
			}

			if (placeHolderClusters.Count == 0)
				return andList;

			return from phone in phoneList
				   where GetDoesPhoneMatchDiacriticPlaceholderCluster(phone, placeHolderClusters[0])
				   select ">" + phone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified phone meets the criteria in the specified
		/// diacritic placeholder cluster pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetDoesPhoneMatchDiacriticPlaceholderCluster(string phone, string cluster)
		{
			var hasOneOrMore = (cluster.IndexOf('+') >= 0);
			var hasZeroOrMore = (cluster.IndexOf('*') >= 0);

			if (hasOneOrMore && hasZeroOrMore)
			{
				_errors.Add(App.GetString("PhoneticSearchingMessages.InvalidDiacriticPlaceholderSyntax",
					"The symbols '*' and '+' may not appear between square brackets together " +
					"with a diacritic placeholder. One or the other is allowed, but not both."));

				return false;
			}

			cluster = cluster.Replace("+", string.Empty).Replace("*", string.Empty);

			// If the phone does not contain all the diacritics in the cluster, we're done.
			if (!cluster.All(diacritic => phone.Contains(diacritic)))
				return false;

			// At this point, we know the phone contains all the diacritics. If the zero
			// or more symbol was found then the phone passes the test and we're done.
			if (hasZeroOrMore)
				return true;

			var phonesDiacriticCount = phone.Count(s =>
				App.IPASymbolCache[s] != null && !App.IPASymbolCache[s].IsBase);

			return (hasOneOrMore && phonesDiacriticCount > cluster.Length) ||
				(!hasOneOrMore && phonesDiacriticCount == cluster.Length);
		}

		#endregion

		#region Methods for combining groups of phones
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> AndTwoPhoneGroups(List<string> x, List<string> y)
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
		public IEnumerable<string> OrTwoPhoneGroups(List<string> x, List<string> y)
		{
			if (x.Count == 0)
				return y;

			return (y.Count == 0 ? x : x.Union(y, StringComparer.Ordinal));
		}

		#endregion

		#region Methods returning reg. expression for parsing pattern
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

		#region Methods for building regular expression used for phonetic searching
		/// ------------------------------------------------------------------------------------
		private string CreateRegExpressionOrGroupFromPhoneGroup(char token, bool ignoreDiacritics,
			ICollection<string> ignoredCharacters)
		{
			if (ignoreDiacritics)
			{
				_phoneGroups[token] = AddPhonesToGroupThatMatchIgnoredNonBaseSymbols(_phoneGroups[token],
					symbolInfo => symbolInfo.Type == IPASymbolType.diacritic);
			}

			if (ignoredCharacters.Count > 0)
			{
				_phoneGroups[token] = AddPhonesToGroupThatMatchIgnoredNonBaseSymbols(_phoneGroups[token],
					symbolInfo => symbolInfo.Type == IPASymbolType.suprasegmental &&
						ignoredCharacters.Contains(symbolInfo.Literal));
			}

			// Remove the prefix that tells the ignore diacritics and symbols
			// processing to bypass the phone.
			for (int i = 0; i < _phoneGroups[token].Count; i++)
				_phoneGroups[token][i] =_phoneGroups[token][i].TrimStart('>');

			var bldr = new StringBuilder("(");

			foreach (var phone in _phoneGroups[token])
				bldr.AppendFormat("{0}|", phone);

			bldr.Length--;
			if (bldr.Length == 0)
				return string.Empty;

			var regExpression = bldr + ")";
			if (!regExpression.Contains("|"))
				regExpression = regExpression.Trim('(', ')');

			return regExpression;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will go through each non-base symbol in each phone and check whether
		/// or not the symbols are ignored based on the search query's ignored diacritics
		/// flag value and ignored symbols collection. For each symbol that's ignored, a
		/// version of the phone without that ignored symbol is added to the phone group.
		/// When a phone in phoneGroup is found that has a '>' prefix it means that previously
		/// in the pattern parsing process, the phone matched the the criteria imposed by a
		/// diacritic placeholder cluster pattern. Those cases trump the query's ignore
		/// diacritics value and ignored symbols collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> AddPhonesToGroupThatMatchIgnoredNonBaseSymbols(List<string> phoneGroup,
			Func<IPASymbol, bool> symbolQualifiesProvider)
		{
			int initialNumberOfPhones = phoneGroup.Count;

			for (int i = 0; i < initialNumberOfPhones; i++)
			{
				// If the phone has a '>' prefix then skip to the next phone.
				if (phoneGroup[i].StartsWith(">"))
					continue;

				var nonBaseSymbolsInPhone = (from s in phoneGroup[i]
											 let symbolInfo = App.IPASymbolCache[s]
											 where symbolInfo != null && !symbolInfo.IsBase && symbolQualifiesProvider(symbolInfo)
											 select s).ToArray();

				if (nonBaseSymbolsInPhone.Length == 0)
					continue;

				var finalNewPhone = phoneGroup[i];

				foreach (var symbol in nonBaseSymbolsInPhone)
				{
					finalNewPhone = finalNewPhone.Replace(symbol.ToString(), string.Empty);
					var newPhone = phoneGroup[i].Replace(symbol.ToString(), string.Empty);
					if (!phoneGroup.Contains(newPhone))
						phoneGroup.Add(newPhone);
				}

				if (!phoneGroup.Contains(finalNewPhone))
					phoneGroup.Add(finalNewPhone);
			}

			return phoneGroup;
		}

		/// ------------------------------------------------------------------------------------
		public string GetRegExpressionForIngoredBaseSymbols(List<string> ignoredCharacters)
		{
			var ignoredNonBaseSymbols = (from s in ignoredCharacters
										 let symbolInfo = App.IPASymbolCache[s]
										 where symbolInfo != null && symbolInfo.IsBase
										 select s).ToArray();

			if (ignoredNonBaseSymbols.Length == 0)
				return string.Empty;

			var bldr = new StringBuilder("(");
			foreach (var sseg in ignoredNonBaseSymbols)
				bldr.AppendFormat("{0}|", sseg);

			bldr.Length--;
			return bldr + ")?";
		}

		#endregion
	}
}
