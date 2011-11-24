using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SearchQueryValidatorTests : TestBase
	{
		private SearchQueryValidator _validator;

		/// ------------------------------------------------------------------------------------
		public override void FixtureSetup()
		{
			base.FixtureSetup();

			App.IPASymbolCache.Add('^', new IPASymbol { Literal = "^", IsBase = false });
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			App.DottedCircle = "0";
			App.DottedCircleC = '0';
			App.DiacriticPlaceholder = "[0]";
			
			_validator = new SearchQueryValidator(_prj);
		}

		#region VerifyGeneralPatternStructure tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyGeneralPatternStructure_FormatCorrect_ReturnsTrue()
		{
			Assert.IsTrue(_validator.VerifyGeneralPatternStructure("123/abc_def"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyGeneralPatternStructure_NoSlashOrUnderscore_ReturnsFalse()
		{
			Assert.IsFalse(_validator.VerifyGeneralPatternStructure("abcdef"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyGeneralPatternStructure_NoSlash_ReturnsFalse()
		{
			Assert.IsFalse(_validator.VerifyGeneralPatternStructure("123_abcdef"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyGeneralPatternStructure_NoUnderscore_ReturnsFalse()
		{
			Assert.IsFalse(_validator.VerifyGeneralPatternStructure("123/abcdef"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyGeneralPatternStructure_UnderscoreBeforeSlash_ReturnsFalse()
		{
			Assert.IsFalse(_validator.VerifyGeneralPatternStructure("123_abc/def"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyGeneralPatternStructure_TooManySlashes_ReturnsFalse()
		{
			Assert.IsFalse(_validator.VerifyGeneralPatternStructure("123/abc/def"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyGeneralPatternStructure_TooManyUnderscores_ReturnsFalse()
		{
			Assert.IsFalse(_validator.VerifyGeneralPatternStructure("123_abc_def"));
		}

		#endregion

		#region VerifySearchItem tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifySearchItem_ValidText_CausesNoErrors()
		{
			_validator.VerifySearchItem("abc");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifySearchItem_EmptyString_CausesErrors()
		{
			_validator.VerifySearchItem(string.Empty);
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifySearchItem_ContainsZeroOrMoreSymbol_CausesErrors()
		{
			_validator.VerifySearchItem("ab*cd");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifySearchItem_ContainsOneOrMoreSymbol_CausesErrors()
		{
			_validator.VerifySearchItem("ab+cd");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifySearchItem_ContainsWordBoundarySymbol_CausesErrors()
		{
			_validator.VerifySearchItem("ab#cd");
			Assert.IsTrue(_validator.HasErrors);
		}

		#endregion

		#region VerifyPrecedingEnvironment tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyPrecedingEnvironment_MiscGoodExamples_CausesNoErrors()
		{
			_validator.VerifyPrecedingEnvironment("#abc");
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyPrecedingEnvironment("+abc");
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyPrecedingEnvironment("*");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyPrecedingEnvironment_ContainsTwoOrMoreWildcardSymbols_CausesErrors()
		{
			_validator.VerifyPrecedingEnvironment("#*");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyPrecedingEnvironment("#+");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyPrecedingEnvironment("*+");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyPrecedingEnvironment_ContainsTooManyOfSameWildcardSymbol_CausesErrors()
		{
			_validator.VerifyPrecedingEnvironment("##");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyPrecedingEnvironment("++");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyPrecedingEnvironment("**");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyPrecedingEnvironment_MisplacedWordBoundary_CausesErrors()
		{
			_validator.VerifyPrecedingEnvironment("a#");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyPrecedingEnvironment_MisplacedZeroOrMoreSymbol_CausesErrors()
		{
			_validator.VerifyPrecedingEnvironment("a*");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyPrecedingEnvironment_MisplacedOneOrMoreSymbol_CausesErrors()
		{
			_validator.VerifyPrecedingEnvironment("a+");
			Assert.IsTrue(_validator.HasErrors);
		}

		#endregion

		#region VerifyFollowingEnvironment tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyFollowingEnvironment_MiscGoodExamples_CausesNoErrors()
		{
			_validator.VerifyFollowingEnvironment("abc#");
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyFollowingEnvironment("abc+");
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyFollowingEnvironment("*");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyFollowingEnvironment_ContainsTwoOrMoreWildcardSymbols_CausesErrors()
		{
			_validator.VerifyFollowingEnvironment("#*");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyFollowingEnvironment("#+");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyFollowingEnvironment("*+");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyFollowingEnvironment_ContainsTooManyOfSameWildcardSymbol_CausesErrors()
		{
			_validator.VerifyFollowingEnvironment("##");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyFollowingEnvironment("++");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Errors.Clear();
			_validator.VerifyFollowingEnvironment("**");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyFollowingEnvironment_MisplacedWordBoundary_CausesErrors()
		{
			_validator.VerifyFollowingEnvironment("#a");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyFollowingEnvironment_MisplacedZeroOrMoreSymbol_CausesErrors()
		{
			_validator.VerifyFollowingEnvironment("*a");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyFollowingEnvironment_MisplacedOneOrMoreSymbol_CausesErrors()
		{
			_validator.VerifyFollowingEnvironment("+a");
			Assert.IsTrue(_validator.HasErrors);
		}

		#endregion

		#region VerifyTextInSquareBrackets tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_ValidDescriptiveFeatureName_CausesNoErrors()
		{
			_validator.VerifyTextInSquareBrackets("blah blah [mid tone] blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_ValidPlusDistinctiveFeatureName_CausesNoErrors()
		{
			App.BFeatureCache.Clear();
			App.BFeatureCache.LoadFromList(new[] { new Feature { Name = "+stupid" } });
			_validator.VerifyTextInSquareBrackets("blah blah [+stupid] blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_ValidMinusDistinctiveFeatureName_CausesNoErrors()
		{
			App.BFeatureCache.Clear();
			App.BFeatureCache.LoadFromList(new[] { new Feature { Name = "+stupid" } });
			_validator.VerifyTextInSquareBrackets("blah blah [-stupid] blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_InFeatureName_CausesErrors()
		{
			_validator.VerifyTextInSquareBrackets("blah blah [inner blah] blah blah");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_NestedBracketsWithValidName_CausesNoErrors()
		{
			_validator.VerifyTextInSquareBrackets("blah blah [[mid tone][close]] blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_NestedBracketsWithInvalidName_CausesErrors()
		{
			_validator.VerifyTextInSquareBrackets("blah blah [[mid tone][purple]] blah blah");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_NoTextBetweenBrackets_CausesNoErrors()
		{
			_validator.VerifyTextInSquareBrackets("blah blah [] blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_ConsonantClass_CausesNoErrors()
		{
			_validator.VerifyTextInSquareBrackets("blah blah [C] blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInSquareBrackets_VowelClass_CausesNoErrors()
		{
			_validator.VerifyTextInSquareBrackets("blah blah [V] blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		#endregion

		#region VerifyNoEmptyTextBetweenOpenAndCloseSymbols tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_NoPairs_CausesNoErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah blah blah",
				PatternParser.FindInnerMostSquareBracketPairs, string.Empty);
			
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah blah blah",
				PatternParser.FindInnerMostBracesPair, string.Empty);

			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah blah blah",
				PatternParser.FindInnerAngleBracketPairs, string.Empty);

			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_NoEmptySquareBrackets_CausesNoErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah [blah] blah blah",
				PatternParser.FindInnerMostSquareBracketPairs, string.Empty);
			
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_OneSetOfEmptySquareBrackets_CausesErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah [] blah blah",
				PatternParser.FindInnerMostSquareBracketPairs, string.Empty);
			
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_NestedSquareBrackets_CausesErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah [[][]] blah blah",
				PatternParser.FindInnerMostSquareBracketPairs, string.Empty);
			
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_NoEmptyBraces_CausesNoErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah {blah} blah blah",
				PatternParser.FindInnerMostBracesPair, string.Empty);

			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_OneSetOfEmptyBraces_CausesErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah {} blah blah",
				PatternParser.FindInnerMostBracesPair, string.Empty);

			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_NestedBraces_CausesErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah {{}{}} blah blah",
				PatternParser.FindInnerMostBracesPair, string.Empty);

			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_NoEmptyAngleBrackets_CausesNoErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah <blah> blah blah",
				PatternParser.FindInnerAngleBracketPairs, string.Empty);

			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyNoEmptyTextBetweenOpenAndCloseSymbols_OneSetOfAngleBrackets_CausesErrors()
		{
			_validator.VerifyNoEmptyTextBetweenOpenAndCloseSymbols("blah blah <> blah blah",
				PatternParser.FindInnerAngleBracketPairs, string.Empty);

			Assert.IsTrue(_validator.HasErrors);
		}

		#endregion

		#region VerifyTextInAngleBrackets tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInAngleBrackets_ValidClassName_CausesNoErrors()
		{
			_prj.SearchClasses.Clear();
			_prj.SearchClasses.Add(new SearchClass { Name = "basket weaving" });
			_validator.VerifyTextInAngleBrackets("blah blah <basket weaving> blah blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInAngleBrackets_InvalidClassName_CausesErrors()
		{
			_prj.SearchClasses.Clear();
			_prj.SearchClasses.Add(new SearchClass { Name = "basket making" });
			_validator.VerifyTextInAngleBrackets("blah blah <basket weaving> blah blah");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyTextInAngleBrackets_ValidAndInvalidClassNames_CausesErrors()
		{
			_prj.SearchClasses.Clear();
			_prj.SearchClasses.Add(new SearchClass { Name = "basket making" });
			_validator.VerifyTextInAngleBrackets("blah blah <basket weaving> blah <basket making>");
			Assert.IsTrue(_validator.HasErrors);
		}

		#endregion

		#region VerifyMatchingOpenAndCloseSymbols tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyMatchingOpenAndCloseSymbols_SimpleMatchExample_CausesNoErrors()
		{
			_validator.VerifyMatchingOpenAndCloseSymbols("blah [] blah", '[', ']', string.Empty);
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyMatchingOpenAndCloseSymbols_NestedMatchExample_CausesNoErrors()
		{
			_validator.VerifyMatchingOpenAndCloseSymbols("blah [[][[]]] blah", '[', ']', string.Empty);
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyMatchingOpenAndCloseSymbols_SimpleMismatchExample_CausesErrors()
		{
			_validator.VerifyMatchingOpenAndCloseSymbols("blah > blah", '<', '>', string.Empty);
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyMatchingOpenAndCloseSymbols_NestedMismatchExample_CausesErrors()
		{
			_validator.VerifyMatchingOpenAndCloseSymbols("blah {{}{{}} blah", '{', '}', string.Empty);
			Assert.IsTrue(_validator.HasErrors);
		}

		#endregion

		#region VerifyOneDiacriticPlaceholderPerANDGroup tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyOneDiacriticPlaceholderPerANDGroup_NoDiacriticPlaceholders_CausesNoErrors()
		{
			_validator.VerifyOneDiacriticPlaceholderPerANDGroup("blah [[close][open]] blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyOneDiacriticPlaceholderPerANDGroup_OneDiacriticPlaceholders_CausesNoErrors()
		{
			_validator.VerifyOneDiacriticPlaceholderPerANDGroup("blah [[close][open][0]] blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyOneDiacriticPlaceholderPerANDGroup_MultipleDiacriticPlaceholders_CausesErrors()
		{
			_validator.VerifyOneDiacriticPlaceholderPerANDGroup("blah [[close][0~][open][0]] blah");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyOneDiacriticPlaceholderPerANDGroup_MultipleDiacriticPlaceholders_CausesNoErrors()
		{
			_validator.VerifyOneDiacriticPlaceholderPerANDGroup("blah{[[close][0~]],[[open][0]]}blah");
			Assert.IsFalse(_validator.HasErrors);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyDiacriticPlaceholders_OnlyOneWildcard_CausesNoErrors()
		{
			_validator.VerifyDiacriticPlaceholders("[0*]");
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyDiacriticPlaceholders("[0^+]");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyDiacriticPlaceholders_TooManyZeroOrMoreSymbols_CausesErrors()
		{
			_validator.VerifyDiacriticPlaceholders("[0**]");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyDiacriticPlaceholders_TooManyOneOrMoreSymbols_CausesErrors()
		{
			_validator.VerifyDiacriticPlaceholders("[0++]");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyDiacriticPlaceholders_BothWildcardsTogether_CausesErrors()
		{
			_validator.VerifyDiacriticPlaceholders("[0*+]");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyDiacriticPlaceholders_ContainsValidDiacritic_CausesNoErrors()
		{
			_validator.VerifyDiacriticPlaceholders("[0^]");
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyDiacriticPlaceholders("[0^*]");
			Assert.IsFalse(_validator.HasErrors);

			_validator.VerifyDiacriticPlaceholders("[0^+]");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyDiacriticPlaceholders_ContainsBaseCharacter_CausesErrors()
		{
			_validator.VerifyDiacriticPlaceholders("[0^g]");
			Assert.IsTrue(_validator.HasErrors);
		}
	}
}
