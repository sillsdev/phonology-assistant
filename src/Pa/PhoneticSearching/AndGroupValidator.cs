using System.Linq;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class AndGroupValidator : GroupValidatorBase
	{
		/// ------------------------------------------------------------------------------------
		public AndGroupValidator(PaProject project) : base(project)
		{
		}

		/// ------------------------------------------------------------------------------------
		public override void Verify(string pattern)
		{
			base.Verify(pattern);

			VerifyDiacriticPlaceholdersAreInAndGroups(pattern);

			pattern = TranslateInnerSquareBracketedTextToTokens(pattern);

			while (true)
			{
				var match = PatternParser.FindInnerMostSquareBracketPairs(pattern);
				if (!match.Success)
					return;

				LookForMalformedGroup(match.Value);
				_tokenBins[++_token] = match.Value;
				pattern = pattern.Replace(match.Value, _token.ToString());
			}
		}

		/// ------------------------------------------------------------------------------------
		public string TranslateInnerSquareBracketedTextToTokens(string pattern)
		{
			var match = PatternParser.FindInnerMostSquareBracketPairs(pattern);
			while (match.Success)
			{
				_tokenBins[++_token] = match.Value;
				var newPattern = pattern.Substring(0, match.Index) + _token + new string('$', match.Length - 1);
				if (match.Index + match.Length < pattern.Length)
					newPattern += pattern.Substring(match.Index + match.Length);

				pattern = newPattern;
				match = match.NextMatch();
			}

			return pattern.Replace("$", string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void VerifyDiacriticPlaceholdersAreInAndGroups(string pattern)
		{
			while (true)
			{
				var match = PatternParser.FindInnerMostSquareBracketPairs(pattern);
				if (!match.Success)
					break;

				if (!match.Value.Contains(App.DottedCircleC))
					pattern = pattern.Replace(match.Value, string.Empty);
				else
				{
					_tokenBins[++_token] = match.Value;
					pattern = pattern.Replace(match.Value, _token.ToString());
				}
			}

			var fmt = App.GetString("PhoneticSearchingMessages.DiacriticPlaceholderNotInAndGroupMsg",
			    "The diacritic placeholder pattern '{0}' is not inside an AND group. Diacritic placeholder " +
			    "patterns must be placed inside AND groups.");

			foreach (var error in pattern.Where(c => c > kMinToken)
				.Select(c => new SearchQueryValidationError(string.Format(fmt, _tokenBins[c]))))
			{
				error.HelpLinks.AddRange(new[] { "hidSearchPatternsDiacriticPlaceholders", "hidSearchPatternsAndGroups" });
				Errors.Add(error);
			}

			_tokenBins.Clear();
			_token = kMinToken;
		}

		/// ------------------------------------------------------------------------------------
		private void LookForMalformedGroup(string andGroupPattern)
		{
			var origAndGroupPattern = andGroupPattern;

			andGroupPattern = andGroupPattern.Trim('[', ']');

			// Convert classes within the AND group to tokens.
			// TODO: Add a check for AND groups containing more than one phone class or
			// one phone class and another phone. That is an invalid combination.
			andGroupPattern = TranslateTextBetweenOpenAndCloseSymbolsToTokens(
				andGroupPattern, PatternParser.FindInnerAngleBracketPairs);

			// Convert OR groups within the AND group to tokens.
			andGroupPattern = TranslateTextBetweenOpenAndCloseSymbolsToTokens(
				andGroupPattern, PatternParser.FindInnerMostBracesPair);

			// At this point any commas found are invalid.
			if (andGroupPattern.Contains(','))
			{
				var msg = string.Format(App.GetString("PhoneticSearchingMessages.AndGroupContainsCommaMsg",
					"The AND group '{0}' contains a comma. Commas are only valid in OR groups."),
					TranslateTokenizedTextToReadableText(origAndGroupPattern));

				var error = new SearchQueryValidationError(msg);
				error.HelpLinks.AddRange(new[] { "hidSearchPatternsAndGroups", "hidSearchPatternsOrGroups", "hidSearchPatternsExamples" });
				Errors.Add(error);
				andGroupPattern = andGroupPattern.Replace(",", string.Empty);
			}

			// Collect all the characters that are not tokens and parse them into phones. If
			// more than one phone is found (i.e. a run), then the AND group is malformed.
			var text = andGroupPattern.Where(c => c < kMinToken)
				.Aggregate(string.Empty, (curr, c) => curr + c.ToString());

			if (text == string.Empty)
				return;

			var phones = _project.PhoneticParser.Parse(text, true, false);
			if (phones.Length > 1)
			{
				var msg = string.Format(App.GetString("PhoneticSearchingMessages.AndGroupContainsPhoneRunMsg",
					"The AND group '{0}' contains more than one literal phone, which is invalid and does " +
					"not make sense in an AND group. Only single phones are allowed in AND groups."),
					TranslateTokenizedTextToReadableText(origAndGroupPattern));

				var error = new SearchQueryValidationError(msg);
				error.HelpLinks.AddRange(new[] { "hidSearchPatternsAndGroups", "hidSearchPatternsExamples" });
				Errors.Add(error);
			}
		}
	}
}
