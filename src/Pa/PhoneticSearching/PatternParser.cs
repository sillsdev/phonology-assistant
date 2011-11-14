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
		public string Parse(string pattern)
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

			var bldr = new StringBuilder();

			foreach (var chr in pattern)
				bldr.Append(chr < kMinToken ? chr.ToString() : CreateOrGroupFromPhoneGroup(chr));

			return bldr.ToString();
		}

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

		/// ------------------------------------------------------------------------------------
		public string CreateOrGroupFromPhoneGroup(char token)
		{
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
					// Log error that search class doesn't exist.
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
						// Log error about invalid characters in an and group.
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

		#region Methods for handling diacritic placeholders in the pattern
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the strings in the specified list of phones for a diacritic placeholder
		/// cluster. If it finds one, then it scans the rest of the strings in the list to
		/// determine whether or not each meets the criteria imposed by the cluster pattern.
		/// A list is returned that contains only the phones that meet the cluster's critera.
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
				// Log error that "AND groups can only have one diacritic placeholder cluster
				return new List<string>(0);
			}

			return (placeHolderClusters.Count == 0 ? andList : phoneList.Where(phone =>
				GetDoesPhoneMatchDiacriticPlaceholderCluster(phone, placeHolderClusters[0])));
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
				// Log error that having + and * in a diacritic placeholder cluster is not valid.
				return false;
			}

			cluster = cluster.Replace("+", string.Empty).Replace("*", string.Empty);
			if (cluster.Length == 0)
			{
				// Log error that only having + or * in a diacritic placeholder cluster (with the circle) is not valid.
				return false;
			}

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
				// Log error about invalid characters in a or group
				return false;
			}

			if (orGroupMember.Length == 1)
				return true;

			if (orGroupMember.Contains(App.kDottedCircle))
			{
				// Log error about diacritic placeholders not being in or groups
				return false;
			}

			if (!orGroupMember.StartsWith("(") && !orGroupMember.EndsWith(")"))
			{
				// log error about having sequences of phonoes between commas without grouping them with parenthese
				return false;
			}

			return true;
		}

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
	}
}
