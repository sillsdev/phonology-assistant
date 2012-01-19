using NUnit.Framework;
using SIL.Pa.TestUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class GroupValidatorBaseTests : TestBase
	{
		private GroupValidatorBase _validator;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_validator = new GroupValidatorBase(_prj);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateTextBetweenOpenAndCloseSymbolsToTokens_ContainsNoBracketedText_ReturnsInputString()
		{
			Assert.AreEqual("a,b,c", _validator.TranslateTextBetweenOpenAndCloseSymbolsToTokens("a,b,c",
				PatternParser.FindInnerMostSquareBracketPairs));

			Assert.AreEqual("a,b,c", _validator.TranslateTextBetweenOpenAndCloseSymbolsToTokens("a,b,c",
				PatternParser.FindInnerAngleBracketPairs));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateTextBetweenOpenAndCloseSymbolsToTokens_ContainsAndGroup_ReturnsModifiedString()
		{
			var result = _validator.TranslateTextBetweenOpenAndCloseSymbolsToTokens("a,[[high][low]],c",
				PatternParser.FindInnerMostSquareBracketPairs);

			Assert.AreEqual("a,", result.Substring(0, 2));
			Assert.IsTrue(result[2] > 60000);
			Assert.AreEqual(",c", result.Substring(3, 2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateTextBetweenOpenAndCloseSymbolsToTokens_ContainsClass_ReturnsModifiedString()
		{
			var result = _validator.TranslateTextBetweenOpenAndCloseSymbolsToTokens("a,<stupid>,c",
				PatternParser.FindInnerAngleBracketPairs);

			Assert.AreEqual("a,", result.Substring(0, 2));
			Assert.IsTrue(result[2] > 60000);
			Assert.AreEqual(",c", result.Substring(3, 2));
		}
	}
}
