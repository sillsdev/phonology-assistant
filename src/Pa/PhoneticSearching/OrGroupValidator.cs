using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class OrGroupValidator : ValidatorBase
	{
		private readonly Dictionary<char, string> _tokenBins;
		private const char kMinToken = (char)(char.MaxValue - 256);
		private char _token = kMinToken;

		/// ------------------------------------------------------------------------------------
		public OrGroupValidator(PaProject project) : base(project)
		{
			_tokenBins = new Dictionary<char, string>();
		}

		/// ------------------------------------------------------------------------------------
		public void Verify(string orGroupPattern)
		{
			Errors.Clear();

			var pattern = TranslateTextBetweenOpenAndCloseSymbolsToTokens(
				orGroupPattern.Trim('{', '}'), PatternParser.FindInnerAngleBracketPairs);

			pattern = TranslateTextBetweenOpenAndCloseSymbolsToTokens(
				pattern, PatternParser.FindInnerMostSquareBracketPairs);

			var orItems = pattern.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			LookForMalformedItems(orItems);

			if (HasErrors || orItems.Length > 1)
				return;

			var msg = App.GetString("PhoneticSearchingMessages.OrGroupContainsOnlyOneItemMsg",
				"The OR group '{0}' only contains one item. OR groups should contain multiple items " +
				"separated by commas.");

			Errors[string.Format(msg, orGroupPattern)] = null;
		}

		/// ------------------------------------------------------------------------------------
		public string TranslateTextBetweenOpenAndCloseSymbolsToTokens(string pattern,
			Func<string, Match> parsingProvider)
		{
			while (true)
			{
				var match = parsingProvider(pattern);
				if (!match.Success)
					return pattern;

				_tokenBins[++_token] = match.Value;
				pattern = pattern.Replace(match.Value, _token.ToString());
			}
		}

		/// ------------------------------------------------------------------------------------
		public void LookForMalformedItems(string[] orItems)
		{
			foreach (var item in orItems)
			{
				var text = item.Trim();
				var phones = _project.PhoneticParser.Parse(text, true, false);
				if (phones.Length == 1 || (text.StartsWith("(") && text.EndsWith(")")))
					continue;

				text = string.Empty;
				text = phones.Aggregate(text, (curr, phone) => curr + phone);

				while (text.Any(c => c > kMinToken))
				{
					var token = text.First(c => c > kMinToken);
					text = text.Replace(token.ToString(), _tokenBins[token]);
				}

				var msg = App.GetString("PhoneticSearchingMessages.OrGroupContainsPhoneRunMsg",
					"A match on the pattern '{0}' will include more than one phone. This text was " +
					"found in an OR group and as such, it is invalid. OR groups should contain " +
					"multiple items separated by commas and each item may represent a match on only " +
					"one phone unless the item is surrounded by parentheses. Either one or more " +
					"commas are missing or the item should be placed between parentheses.");

				Errors[string.Format(msg, text)] = null;
			}
		}

		///// ------------------------------------------------------------------------------------
		//private string TranslateTokenizedTextToReadableText(string text)
		//{
		//    while (text.Any(c => c > kMinToken))
		//    {
		//        var token = text.First(c => c > kMinToken);
		//        text = text.Replace(token.ToString(), _tokenBins[token]);
		//    }
		//}
	}
}
