using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIL.Pa.Model;

namespace SIL.Pa.PhoneticSearching
{
	public class GroupValidatorBase : ValidatorBase
	{
		protected readonly Dictionary<char, string> _tokenBins;
		protected const char kMinToken = (char)(char.MaxValue - 256);
		protected char _token = kMinToken;

		/// ------------------------------------------------------------------------------------
		public GroupValidatorBase(PaProject project) : base(project)
		{
			_tokenBins = new Dictionary<char, string>();
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Verify(string pattern)
		{
			Errors.Clear();
			_tokenBins.Clear();
			_token = kMinToken;
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
		public string TranslateTokenizedTextToReadableText(string text)
		{
			while (text.Any(c => c > kMinToken))
			{
				var token = text.First(c => c > kMinToken);
				text = text.Replace(token.ToString(), _tokenBins[token]);
			}

			return text;
		}
	}
}
