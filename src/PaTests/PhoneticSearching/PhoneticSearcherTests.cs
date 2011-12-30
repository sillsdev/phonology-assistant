using NUnit.Framework;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PhoneticSearcherTests
	{
		private PhoneticSearcher _searcher;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_searcher = new PhoneticSearcher(null, null);
		}

		#region TranslateMatchIndexToPhoneIndex tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchIndexToPhoneIndex_MatchAtBeginningOfSingleCodepointPhones_ReturnsZero()
		{
			var phones = new[]  { "a", "b", "c", "d", "e" };
			Assert.AreEqual(0, _searcher.TranslateMatchIndexToPhoneIndex(phones, 0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchIndexToPhoneIndex_MatchAtBeginningOfMultiCodepointPhones_ReturnsZero()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(0, _searcher.TranslateMatchIndexToPhoneIndex(phones, 0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchIndexToPhoneIndex_MatchAtEndOfSingleCodepointPhones_ReturnsCorrectIndex()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(4, _searcher.TranslateMatchIndexToPhoneIndex(phones, 4));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchIndexToPhoneIndex_MatchAtEndOfMultiCodepointPhones_ReturnsCorrectIndex()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(4, _searcher.TranslateMatchIndexToPhoneIndex(phones, 10));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchIndexToPhoneIndex_MatchInMiddleOfSingleCodepointPhones_ReturnsCorrectIndex()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(3, _searcher.TranslateMatchIndexToPhoneIndex(phones, 3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchIndexToPhoneIndex_MatchInMiddleOfMultiCodepointPhones_ReturnsCorrectIndex()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(3, _searcher.TranslateMatchIndexToPhoneIndex(phones, 6));
		}

		#endregion

		#region TranslateMatchLengthToNumberOfPhones tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchSingleCodepointPhoneAtBeginning_ReturnsOne()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 0, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchMultiCodepointPhoneAtBeginning_ReturnsOne()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 0, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchSingleCodepointPhoneAtEnd_ReturnsOne()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 4, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchMultiCodepointPhoneAtEnd_ReturnsOne()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 4, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchSingleCodepointPhoneInMiddle_ReturnsOne()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 2, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchMultiCodepointPhoneInMiddle_ReturnsOne()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 3, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchLengthSpansSingleCodepointPhonesAtBeginning_ReturnsTwo()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(2, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 0, 2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchLengthSpansMultiCodepointPhonesAtBeginning_ReturnsTwo()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(2, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 0, 5));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchLengthSpansSingleCodepointPhonesAtEnd_ReturnsTwo()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(2, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 3, 2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchLengthSpansMultiCodepointPhonesAtEnd_ReturnsTwo()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(2, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 3, 6));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchLengthSpansSingleCodepointPhonesInMiddle_ReturnsThree()
		{
			var phones = new[] { "a", "b", "c", "d", "e" };
			Assert.AreEqual(3, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 1, 3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchLengthSpansMultiCodepointPhonesInMiddle_ReturnsThree()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(3, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 1, 7));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchOnPartialPhoneAtBeginning_ReturnsOne()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 0, 2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchOnPartialPhoneAtEnd_ReturnsOne()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 4, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchOnPartialPhoneInMiddle_ReturnsOne()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(1, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 3, 2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchMultiplePhonesOneBeingPartialAtBeginning_ReturnsTwo()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(2, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 0, 4));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchMultiplePhonesOneBeingPartialAtEnd_ReturnsTwo()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(2, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 3, 5));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateMatchLengthToNumberOfPhones_MatchMultiplePhonesOneBeingPartialInMiddle_ReturnsThree()
		{
			var phones = new[] { "aaa", "bb", "c", "dddd", "ee" };
			Assert.AreEqual(3, _searcher.TranslateMatchLengthToNumberOfPhones(phones, 1, 4));
		}

		#endregion

		#region DoesPatternPassBasicChecks tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_PrecedingEnvironmentContainsTooManyWordBoundarySymbols_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/##_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_PrecedingEnvironmentContainsTooManyOneOrMoreSymbols_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/++_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_FollowingEnvironmentContainsTooManyWordBoundarySymbols_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_##"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_FollowingEnvironmentContainsTooManyOneOrMoreSymbols_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_++"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_PrecedingEnvironmentContainsInvlalidCombinationOfSymbols_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/+#_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());

			_searcher = new PhoneticSearcher(null, new SearchQuery("a/+*_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());

			_searcher = new PhoneticSearcher(null, new SearchQuery("a/#+*_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_FollowingEnvironmentContainsInvlalidCombinationOfSymbols_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_+#"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());

			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_*+"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());

			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_*+#"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_PrecedingEnvironmentWithMisplacedWordBoundary_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/b#_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_FollowingEnvironmentWithMisplacedWordBoundary_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_#b"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_PrecedingEnvironmentWithMisplacedZeroOrMore_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/b*_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_FollowingEnvironmentWithMisplacedZeroOrMore_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_*b"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_PrecedingEnvironmentWithMisplacedOneOrMore_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/b+_[C]"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DoesPatternPassBasicChecks_FollowingEnvironmentWithMisplacedOneOrMore_ReturnsFalse()
		{
			_searcher = new PhoneticSearcher(null, new SearchQuery("a/[C]_+b"));
			Assert.IsFalse(_searcher.DoesPatternPassBasicChecks());
		}

		#endregion
	}
}
