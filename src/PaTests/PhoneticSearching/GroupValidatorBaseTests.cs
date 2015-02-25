// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
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
