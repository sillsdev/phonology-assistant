using System;
using System.Linq;
using Localization;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class OrGroupValidator : GroupValidatorBase
	{
		/// ------------------------------------------------------------------------------------
		public OrGroupValidator(PaProject project) : base(project)
		{
		}

		/// ------------------------------------------------------------------------------------
		public override void Verify(string orGroupPattern)
		{
			base.Verify(orGroupPattern);

			var pattern = TranslateTextBetweenOpenAndCloseSymbolsToTokens(
				orGroupPattern.Trim('{', '}'), PatternParser.FindInnerAngleBracketPairs);

			pattern = TranslateTextBetweenOpenAndCloseSymbolsToTokens(
				pattern, PatternParser.FindInnerMostSquareBracketPairs);

			var orItems = pattern.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			LookForMalformedItems(orItems);

			if (HasErrors || orItems.Length > 1)
				return;

			var msg = LocalizationManager.GetString("PhoneticSearchingMessages.OrGroupContainsOnlyOneItemMsg",
				"The OR group '{0}' only contains one item. OR groups should contain multiple items " +
				"separated by commas.");

			var error = new SearchQueryValidationError(string.Format(msg, orGroupPattern));
			error.HelpLinks.AddRange(new[] { "hidSearchPatternsOrGroups", "hidSearchPatternsExamples" });
			Errors.Add(error);
		}

		/// ------------------------------------------------------------------------------------
		public void LookForMalformedItems(string[] orItems)
		{
			foreach (var item in orItems)
			{
				var text = item.Trim();
				LookForInvalidOrGroupItems(text);

				var phones = _project.PhoneticParser.Parse(text, true, false);
                if (phones.Length == 1 || (text.StartsWith("(", StringComparison.Ordinal) && text.EndsWith(")", StringComparison.Ordinal)))
					continue;

				text = string.Empty;
				text = phones.Aggregate(text, (curr, phone) => curr + phone);
				text = TranslateTokenizedTextToReadableText(text);

				var msg = LocalizationManager.GetString("PhoneticSearchingMessages.OrGroupContainsPhoneRunMsg",
					"A match on the pattern '{0}' will include more than one phone. This text was " +
					"found in an OR group and as such, it is invalid. OR groups should contain " +
					"multiple items separated by commas and each item may represent a match on only " +
					"one phone unless the item is surrounded by parentheses. Either one or more " +
					"commas are missing or the item should be placed between parentheses.");

				var error = new SearchQueryValidationError(string.Format(msg, text));
				error.HelpLinks.AddRange(new[] { "hidSearchPatternsOrGroups", "hidSearchPatternsExamples" });
				Errors.Add(error);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void LookForInvalidOrGroupItems(string orItem)
		{
			if ("[]{}()+*_#<>".Contains(orItem))
			{
				var error = new SearchQueryValidationError(
					LocalizationManager.GetString("PhoneticSearchingMessages.InvalidSymbolsInORGroup",
						"The symbols '[]{}()<>+*_#' are not allowed in OR groups."));

				error.HelpLinks.AddRange(new[] { "hidSearchPatternsOrGroups", "hidSearchPatternsExamples" });
				Errors.Add(error);
			}

			if (orItem.Contains(App.DottedCircle))
			{
				var error = new SearchQueryValidationError(
					LocalizationManager.GetString("PhoneticSearchingMessages.InvalidDiacriticPlaceholderInORGroup",
						"Diacritic placeholders are not valid in OR groups."));

				error.HelpLinks.AddRange(new[] { "hidSearchPatternsOrGroups", "hidSearchPatternsDiacriticPlaceholders", "hidSearchPatternsExamples" });
				Errors.Add(error);
			}

			var phonesInMember = _project.PhoneticParser.Parse(orItem, true, false);
			if (phonesInMember == null)
			{
				var msg = LocalizationManager.GetString("PhoneticSearchingMessages.InvalidORGroupMember",
					"The text '{0}' is not recognized as valid phonetic data.");

				var error = new SearchQueryValidationError(string.Format(msg, orItem));
				error.HelpLinks.AddRange(new[] { "hidTroubleshootingUndefinedPhoneticCharacters", "hidSearchPatternsExamples" });
				Errors.Add(error);
			}
		}
	}
}
