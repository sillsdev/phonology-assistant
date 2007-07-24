using NUnit.Framework;
using SIL.Pa.Data;
using SIL.SpeechTools.TestUtils;

namespace SIL.Pa.FFSearchEngine
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PatternGroupTests : TestBase
	{
		private SearchQuery m_query;
		private PhoneCache m_phoneCache;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			DataUtils.LoadIPACharCache(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Close and delete the test database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the default state of the search query is that they contain nothing to
		/// ignore.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_phoneCache = new PhoneCache();
			m_query = new SearchQuery();
			m_query.IgnoredLengthChars = string.Empty;
			m_query.IgnoredStressChars = string.Empty;
			m_query.IgnoredToneChars = string.Empty;
			m_query.IgnoreDiacritics = false;
			SearchEngine.CurrentSearchQuery = m_query;

			m_phoneCache["t"] = new TestPhoneInfo();
			m_phoneCache["t"].CharType = IPACharacterType.Consonant;
			m_phoneCache["b"] = new TestPhoneInfo();
			m_phoneCache["b"].CharType = IPACharacterType.Consonant;
			m_phoneCache["d"] = new TestPhoneInfo();
			m_phoneCache["d"].CharType = IPACharacterType.Consonant;
			m_phoneCache["h"] = new TestPhoneInfo();
			m_phoneCache["h"].CharType = IPACharacterType.Consonant;
			m_phoneCache["s"] = new TestPhoneInfo();
			m_phoneCache["s"].CharType = IPACharacterType.Consonant;
			m_phoneCache["x"] = new TestPhoneInfo();
			m_phoneCache["x"].CharType = IPACharacterType.Consonant;
			
			m_phoneCache["a"] = new TestPhoneInfo();
			m_phoneCache["a"].CharType = IPACharacterType.Vowel;
			m_phoneCache["e"] = new TestPhoneInfo();
			m_phoneCache["e"].CharType = IPACharacterType.Vowel;
			m_phoneCache["i"] = new TestPhoneInfo();
			m_phoneCache["i"].CharType = IPACharacterType.Vowel;
			m_phoneCache["o"] = new TestPhoneInfo();
			m_phoneCache["o"].CharType = IPACharacterType.Vowel;
			m_phoneCache["u"] = new TestPhoneInfo();
			m_phoneCache["u"].CharType = IPACharacterType.Vowel;

			m_phoneCache["."] = new TestPhoneInfo();
			m_phoneCache["."].CharType = IPACharacterType.Suprasegmentals;
			m_phoneCache["'"] = new TestPhoneInfo();
			m_phoneCache["'"].CharType = IPACharacterType.Suprasegmentals;

			SearchEngine.PhoneCache = m_phoneCache;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetRootGroupType method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRootGroupTypeTest()
		{
			string pattern = "[+high+con]abc,dental";
			GroupType result = (GroupType)GetResult(typeof(PatternGroup), "GetRootGroupType", pattern);
			Assert.AreEqual(GroupType.Sequential, result);

			pattern = "[[+high][+con]{a,e}]";
			result = (GroupType)GetResult(typeof(PatternGroup), "GetRootGroupType", pattern);
			Assert.AreEqual(GroupType.And, result);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the SearchGroup method for AND and OR groups. This tests matches in members
		/// of type AnyConsonant, AnyVowel, Binary feature, and Articulatory feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchNonSequentialGroupsTest_1()
		{
			// Put mock data in the articulatory and binary feature caches.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			DataUtils.BFeatureCache["voice"].MinusMask = 8;
			DataUtils.AFeatureCache["nasal"].Mask = 17;
			DataUtils.AFeatureCache["nasal"].MaskNumber = 1;

			SearchEngine.PhoneCache["d"].BinaryMask = (4 | 8);
			SearchEngine.PhoneCache["d"].Masks = new ulong[] { 0, 16 };

			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("[{[+high],[-voice]}[C]]");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", "d"));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[{[-high],[+voice]}[C]]");
			Assert.AreEqual(CompareResultType.NoMatch, GetResult(group, "SearchGroup", "d"));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[[+high][+voice]],[nasal]}");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", "d"));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[[+high][+voice]],[dental]}");
			Assert.AreEqual(CompareResultType.NoMatch, GetResult(group, "SearchGroup", "d"));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[[+high][+voice]][nasal]]");
			Assert.AreEqual(CompareResultType.NoMatch, GetResult(group, "SearchGroup", "d"));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[[+high][-voice]][nasal]]");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", "d"));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[V]}");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", "a"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Search method for AND and OR groups. This tests matches in members
		/// of type SinglePhone and one when the environment is the search item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchNonSequentialGroupsTest_SrchItem()
		{
			// Put mock data in the articulatory and binary feature caches.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			DataUtils.BFeatureCache["voice"].MinusMask = 8;
			DataUtils.AFeatureCache["nasal"].Mask = 17;
			DataUtils.AFeatureCache["nasal"].MaskNumber = 1;

			m_phoneCache["b"].BinaryMask = 0;
			m_phoneCache["d"].BinaryMask = (4 | 8);
			m_phoneCache["d"].Masks = new ulong[] { 0, 16 };
			m_phoneCache["a"].BinaryMask = 0;

			int[] results;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,d}[+high]]");
			Assert.IsTrue(group.Search("bad", 0, out results));
			Assert.AreEqual(2, results[0]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,e}[+high]]");
			Assert.IsFalse(group.Search("bad", 0, out results));

			m_phoneCache["a"].BinaryMask = (4 | 8);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,e}[+high]]");
			Assert.IsTrue(group.Search("bad", 1, out results));
			Assert.AreEqual(1, results[0]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("{a,b,c,e}");
			Assert.IsTrue(group.Search("badlerdash", 2, out results));
			Assert.AreEqual(4, results[0]);
			Assert.IsTrue(group.Search("badlerdash", 5, out results));
			Assert.AreEqual(7, results[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Search method for AND and OR groups. This tests matches in members
		/// of type SinglePhone and one when the environment is "Before".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchNonSequentialGroupsTest_EnvBefore()
		{
			int[] results;

			PatternGroup group = new PatternGroup(EnvironmentType.Before);
			group.Parse("{a,b,c,e}");
			Assert.IsTrue(group.Search("badlerdash", 4, out results));
			Assert.AreEqual(4, results[0]);
			Assert.IsFalse(group.Search("badlerdash", 5));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Search method for AND and OR groups. This tests matches in members
		/// of type SinglePhone and one when the environment is "Before".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchNonSequentialGroupsTest_EnvAfter()
		{
			int[] results;

			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("{a,b,c,e}");
			Assert.IsTrue(group.Search("badlerdash", 4, out results));
			Assert.AreEqual(4, results[0]);
			Assert.IsFalse(group.Search("badlerdash", 5));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Search method for sequential groups when the pattern is for the search
		/// item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchSequentialGroupsTest_SrchItem()
		{
			// Put mock data in the articulatory and binary feature caches.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			DataUtils.AFeatureCache["nasal"].Mask = 17;
			DataUtils.AFeatureCache["nasal"].MaskNumber = 1;
			m_phoneCache["d"].BinaryMask = (4 | 8);
			m_phoneCache["a"].Masks = new ulong[] { 0, 16 };

			int[] results;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("der[+high]");
			Assert.IsTrue(group.Search("balderdash", 0, out results));
			Assert.AreEqual(3, results[0]);
			Assert.AreEqual(4, results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("der[+high][nasal]");
			Assert.IsTrue(group.Search("balderdash", 0, out results));
			Assert.AreEqual(3, results[0]);
			Assert.AreEqual(5, results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("al[+high]{i,a,e}");
			Assert.IsTrue(group.Search("balderdash", 1, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(4, results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("al[+high]{i,a,u}");
			Assert.IsFalse(group.Search("balderdash", 1));

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("al[-high]{i,a,e}");
			Assert.IsFalse(group.Search("balderdash", 0));

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("er[+high]ashi");
			Assert.IsFalse(group.Search("balderdash", 4));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Search method for sequential groups when the pattern is for the
		/// environment before.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchSequentialGroupsTest_EnvBefore()
		{
			// Put mock data in the articulatory and binary feature caches.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			m_phoneCache["d"].BinaryMask = (4 | 8);

			PatternGroup group = new PatternGroup(EnvironmentType.Before);
			group.Parse("der[+high]");
			Assert.IsTrue(group.Search("balderdash", 6));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("ber[+high]");
			Assert.IsFalse(group.Search("balderdash", 6));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("bal{b,d,p}");
			Assert.IsTrue(group.Search("balderdash", 3));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("abal{b,d,p}");
			Assert.IsFalse(group.Search("balderdash", 3));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests for proper results when word initial is part of the pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForWordInitial()
		{
			// Put mock data in the feature cache.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			m_phoneCache["b"].BinaryMask = (4 | 8);

			PatternGroup group = new PatternGroup(EnvironmentType.Before);
			group.Parse("#{d,r}");
			Assert.IsFalse(group.Search("balderdash", 5));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("#[+high]");
			Assert.IsFalse(group.Search("baldebrdash", 5));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("#der");
			Assert.IsFalse(group.Search("balderdash", 5));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("#{d,b}");
			Assert.IsTrue(group.Search("balderdash", 0));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("#ba");
			Assert.IsTrue(group.Search(" balderdash", 2));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("#[+high]");
			Assert.IsTrue(group.Search("balderdash", 0));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests for proper results when word final is part of the pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForWordFinal()
		{
			// Put mock data in the feature cache.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			m_phoneCache["h"].BinaryMask = (4 | 8);

			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("{d,h}#");
			Assert.IsFalse(group.Search("baldherdash", 4));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[+high]#");
			Assert.IsFalse(group.Search("baldherdash", 4));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("bald#");
			Assert.IsFalse(group.Search("balderdash", 0));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{h,d}#");
			Assert.IsTrue(group.Search("balderdash ", 9));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("sh#");
			Assert.IsTrue(group.Search("balderdash ", 8));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[+high]#");
			Assert.IsTrue(group.Search("balderdash ", 9));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchesZeroOrMoreTest()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.Before);
			group.Parse("*");
			Assert.IsTrue(group.Search("balderdash", -1));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("*");
			Assert.IsTrue(group.Search("balderdash", 0));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("*");
			Assert.IsTrue(group.Search("balderdash", 4));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("*");
			Assert.IsTrue(group.Search("balderdash", 9));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("*");
			Assert.IsTrue(group.Search("balderdash", 10));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchesOneOrMoreTest()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.Before);
			group.Parse("+");
			Assert.IsFalse(group.Search("balderdash", -1));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("+");
			Assert.IsTrue(group.Search("balderdash", 0));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("+");
			Assert.IsTrue(group.Search("balderdash", 4));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("+");
			Assert.IsTrue(group.Search("balderdash", 9));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("+");
			Assert.IsFalse(group.Search("balderdash", 10));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("+a");
			Assert.IsTrue(group.Search("balderdash", 1));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("s+");
			Assert.IsTrue(group.Search("balderdash", 8));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("+b");
			Assert.IsFalse(group.Search("balderdash", 0));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("h+");
			Assert.IsFalse(group.Search("balderdash", 9));

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("+b");
			Assert.IsFalse(group.Search("balderdash", -1));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("h+");
			Assert.IsFalse(group.Search("balderdash", 10));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search for an modified vowels and consonants, matching the modification exactly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForModifiedConVow_Exact()
		{
			MakeMockCacheEntries();

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse(string.Format("[[C][{0}~]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~ou", 0, out results));
			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);

			group.Parse(string.Format("[[V][{0}~]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("anmo~xyz", 0, out results));
			Assert.AreEqual(3, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search for a modified consonant, matching zero or more modification.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForModifiedConsonant_ZeroOrMore()
		{
			MakeMockCacheEntries();

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse(string.Format("[[C][{0}~*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out results));
			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}^*]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("ateit~^ou", 0, out results));

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);

			Assert.IsTrue(group.Search("ateit~^ou", 2, out results));
			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}'*]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("ateit~^ou", 0, out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search for a modified consonant, matching zero or more modification.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForDiacriticPattern_ZeroOrMore()
		{
			// Treat digits as mock diacritics. (0 and 1 cannot be used because they're used
			// as part of the pattern parsing process.)
			AddMockDiacritics("23456789");
			AddMockPhones(new string[] {"a", "e", "c", "i", "o", "c2", "c23", "c234",
				"c2345", "c2356", "c23456", "c234567"}, IPACharacterType.Consonant);

			TestDiacriticPattern("[[C][{0}*]]/*_*", "c", true);
			TestDiacriticPattern("[[C][{0}23*]]/*_*", "c23", true);
			TestDiacriticPattern("[[C][{0}23*]]/*_*", "c234", true);
			TestDiacriticPattern("[[C][{0}23*]]/*_*", "c2", false);
			TestDiacriticPattern("[[C][{0}*23]]/*_*", "c23", true);
			TestDiacriticPattern("[[C][{0}*45]]/*_*", "c2345", true);
			TestDiacriticPattern("[[C][{0}*45]]/*_*", "c5", false);
			TestDiacriticPattern("[[C][{0}23*56]]/*_*", "c2356", true);
			TestDiacriticPattern("[[C][{0}23*56]]/*_*", "c2345", false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search for a modified consonant, matching one or more modification.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForDiacriticPattern_OneOrMore()
		{
			// Treat digits as mock diacritics. (0 and 1 cannot be used because they're used
			// as part of the pattern parsing process.)
			AddMockDiacritics("23456789");
			AddMockPhones(new string[] {"a", "e", "c", "i", "o", "c2", "c23", "c234",
				"c2345", "c2356", "c23456", "c234567"}, IPACharacterType.Consonant);

			TestDiacriticPattern("[[C][{0}+]]/*_*", "c", false);
			TestDiacriticPattern("[[C][{0}+]]/*_*", "c2", true);
			TestDiacriticPattern("[[C][{0}2+]]/*_*", "c2", false);
			TestDiacriticPattern("[[C][{0}2+]]/*_*", "c23", true);
			TestDiacriticPattern("[[C][{0}+2]]/*_*", "c2", false);
			TestDiacriticPattern("[[C][{0}+3]]/*_*", "c23", true);
			TestDiacriticPattern("[[C][{0}23+56]]/*_*", "c2356", false);
			TestDiacriticPattern("[[C][{0}23+56]]/*_*", "c23456", true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void TestDiacriticPattern(string pattern, string phone, bool expectedResult)
		{
			int[] results;
			m_query = new SearchQuery();
			m_query.IgnoreDiacritics = true;
			m_query.Pattern = string.Format(pattern, DataUtils.kDottedCircle);
			SearchEngine engine = new SearchEngine(m_query);
			string[] word = DataUtils.IPACharCache.PhoneticParser("ae" + phone + "io", false);
			Assert.AreEqual(expectedResult, engine.SearchWord(word, out results));

			if (expectedResult)
			{
				Assert.AreEqual(2, results[0]);
				Assert.AreEqual(1, results[1]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search for a modified vowel, matching zero or more modification.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForModifiedVowel_ZeroOrMore()
		{
			MakeMockCacheEntries();

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse(string.Format("[[V][{0}~*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}^*]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("amo~^xyz", 0, out results));

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out results));
			Assert.AreEqual(0, results[0]);
			Assert.AreEqual(1, results[1]);

			Assert.IsTrue(group.Search("amo~^xyz", 2, out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}'*]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("amo~^xyz", 0, out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search for a modified consonant, matching one or more modification.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForModifiedConsonant_OneOrMore()
		{
			MakeMockCacheEntries();

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse(string.Format("[[C][{0}~+]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out results));
			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);
			group.Parse(string.Format("[[C][{0}~+]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("ateit~ou", 0, out results));

			group.Parse(string.Format("[[C][{0}~+]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("at~eit~^ou", 0, out results));
			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Search for a modified IPA character, matching one or more modification.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForModifiedIPAChar_OneOrMoreDiacritics()
		{
			MakeMockCacheEntries();

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			string pattern = string.Format("t[{0}~+]", DataUtils.kDottedCircle);

			group.Parse(pattern);
			Assert.IsTrue(group.Search("ateit~^ou", 0, out results));
			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);
			group.Parse(pattern);
			Assert.IsFalse(group.Search("ateit~ou", 0, out results));

			group.Parse(pattern);
			Assert.IsTrue(group.Search("at~eit~^ou", 0, out results));
			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreWithNonIPAPattern()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredLengthChars = "b";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredToneChars = string.Empty;
			query.IgnoreDiacritics = false;
			SearchEngine.CurrentSearchQuery = query;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse("[V][V]");
			Assert.IsTrue(group.Search("xabax", 0, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(3, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForAnyVowConWithDiacriticsInData()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[V]/[C]_[C]";
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("t^at", true), out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);

			engine = new SearchEngine(query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ieout^o~t", true), out results));
			Assert.AreEqual(5, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithBaseCharsInIgnoreList()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredToneChars = ".";
			query.IgnoreDiacritics = false;
			SearchEngine.CurrentSearchQuery = query;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse("aa");
			Assert.IsTrue(group.Search("xa.ax", 0, out results));

			group.Parse("[V][C]");
			Assert.IsTrue(group.Search("a.sa", 0, out results));
			Assert.AreEqual(0, results[0]);
			Assert.AreEqual(3, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace1()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]#/*_*";
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("his bat", true), out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(2, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace2()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]/#_*";
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			string[] phones = DataUtils.IPACharCache.PhoneticParser(" his bat", true);
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);

			Assert.IsTrue(engine.SearchWord(out results));
			Assert.AreEqual(5, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace3()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]/*_#";
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			string[] phones = DataUtils.IPACharCache.PhoneticParser("his bat ", true);
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			Assert.IsTrue(engine.SearchWord(out results));
			Assert.AreEqual(6, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace4()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]#[C]/*_*";
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("his bat", true), out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(3, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace5()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[V]#[C]/*_*";
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = DataUtils.IPACharCache.PhoneticParser("bea ", true);
			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord(phones, out results));

			query.Pattern = "[V]/*_#";
			engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			query.Pattern = "[V]#/*_*";
			engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(2, results[1]);

			query.Pattern = "[V]#/*_+";
			engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord(phones, out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternWithNoBeforeEnvironment()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = DataUtils.IPACharCache.PhoneticParser("beat", true);
			int[] results;

			query.Pattern = "[V]/_*";
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternWithNoAfterEnvironment()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = DataUtils.IPACharCache.PhoneticParser("beat", true);
			int[] results;

			query.Pattern = "[V]/*_";
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternWithNoEnvironment()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = DataUtils.IPACharCache.PhoneticParser("beat", true);
			int[] results;

			query.Pattern = "[V]/_";
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);

			query.Pattern = "[V]/";
			engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);

			query.Pattern = "[V]";
			engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForVCWithVVCInWord()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[V][C]/*_*";
			query.IgnoredToneChars = string.Empty;
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsTrue(engine.SearchWord("beat", out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(2, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordInitial1()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "h[V]/#_*";
			query.IgnoredToneChars = ".";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord(" .xawadambo ", out results));
			Assert.IsTrue(engine.SearchWord(" .hawadambta ", out results));
			Assert.IsTrue(engine.SearchWord(" h.awadambta ", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordInitialButInPattern()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = ".abc/*_*";
			query.IgnoredToneChars = ".";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord("abcdef", out results));
			Assert.IsTrue(engine.SearchWord(".abcdef", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordFinalButInPattern()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "ef./*_*";
			query.IgnoredToneChars = ".";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord("abcdef", out results));
			Assert.IsTrue(engine.SearchWord("abcdef.", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordMedialButInPattern()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "b.c/*_*";
			query.IgnoredToneChars = ".";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord("abcdef", out results));
			Assert.IsTrue(engine.SearchWord("ab.cdef", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordFinal()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "t[V]/*_#";
			query.IgnoredToneChars = ".";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord(" hawadambo. ", out results));
			Assert.IsTrue(engine.SearchWord(" hawadambta. ", out results));
			Assert.IsTrue(engine.SearchWord(" hawadambt.a ", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnorePhoneFoundAtWordInitial_OneOrMore()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "h[V]/+_*";
			query.IgnoredToneChars = ".";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord(".xawadambo", out results));
			Assert.IsFalse(engine.SearchWord(".hawadambta", out results));
			Assert.IsTrue(engine.SearchWord("x.h.awadambta", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoredPhoneFoundAtWordFinal_OneOrMore()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "t[V]/*_+";
			query.IgnoredToneChars = ".";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredLengthChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord("hawadambt.a", out results));
			Assert.IsFalse(engine.SearchWord("hawadambta.", out results));
			Assert.IsFalse(engine.SearchWord("hawadambt.a", out results));
			Assert.IsTrue(engine.SearchWord("hawadambta.a", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoringDiacritics()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoreDiacritics = true;
			SearchEngine.CurrentSearchQuery = query;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse("t");
			Assert.IsTrue(group.Search("xat~ax", 0, out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithoutIgnoringDiacritics()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoreDiacritics = false;
			SearchEngine.CurrentSearchQuery = query;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse("t");
			Assert.IsFalse(group.Search("xat~ax", 0, out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithDiacriticsInIgnoreList()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredToneChars = "~";
			query.IgnoreDiacritics = false;
			SearchEngine.CurrentSearchQuery = query;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse("t");
			Assert.IsTrue(group.Search("xat~ax", 0, out results));

			group.Parse("t");
			Assert.IsFalse(group.Search("xat^~ax", 0, out results));

			group.Parse("t");
			Assert.IsFalse(group.Search("xat~^ax", 0, out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that zero or more wildcard can be in the same pattern piece with something
		/// else.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithZeroOrMore_Match()
		{
			m_query.Pattern = "o/*a_*";
			int[] results;
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			m_query.Pattern = "o/*_u*";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			m_query.Pattern = "o/*a_u*";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			m_query.Pattern = "t/*u_*";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("tatoutb", out results));
			Assert.AreEqual(5, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that zero or more wildcard can be in the same pattern piece with something
		/// else.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithZeroOrMore_NoMatch()
		{
			m_query.Pattern = "o/*x_*";
			int[] results;
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsFalse(engine.SearchWord("taoub", out results));

			m_query.Pattern = "o/*_x*";
			engine = new SearchEngine(m_query);
			Assert.IsFalse(engine.SearchWord("taoub", out results));

			m_query.Pattern = "o/*x_x*";
			engine = new SearchEngine(m_query);
			Assert.IsFalse(engine.SearchWord("taoub", out results));

			m_query.Pattern = "t/*u_*";
			engine = new SearchEngine(m_query);
			Assert.IsFalse(engine.SearchWord("taoub", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that one or more wildcard can be in the same pattern piece with something
		/// else.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithOneOrMore_Match()
		{
			m_query.Pattern = "o/+a_+";
			int[] results;
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			m_query.Pattern = "o/+_u+";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			m_query.Pattern = "o/+a_u+";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			m_query.Pattern = "ao/+_+";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(2, results[1]);

			m_query.Pattern = "a[V]/+_+";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(engine.SearchWord("taoub", out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(2, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that one or more wildcard can be in the same pattern piece with something
		/// else.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithOneOrMore_NoMatch()
		{
			int[] results;

			m_query.Pattern = "t/+_+";
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("taoub", false), out results));

			m_query.Pattern = "a/+t_+";
			engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("taoub", false), out results));

			m_query.Pattern = "u/+_b+";
			engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("taoub", false), out results));

			m_query.Pattern = "o/+ta_ub+";
			engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("taoub", false), out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that searching fails when a match is found all the way to the end of the
		/// transcription but there is more pattern to match but no more phones on which to
		/// match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithMatchUntilPhonesRunOutAtEnd()
		{
			m_query.Pattern = "x/*_[V][V]";
			int[] results;

			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("abxio", false), out results));

			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("abxi", false), out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that searching fails when a match is found all the way to the beginning of
		/// the transcription but there is more pattern to match but no more phones on which
		/// to match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithMatchUntilPhonesRunOutAtBeginning()
		{
			m_query.Pattern = "x/[V][V]_*";
			int[] results;

			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("aixio", false), out results));

			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ixio", false), out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that when the first char. in a word is ignored, it's not matched to '+'
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithFirstPhoneIgnored()
		{
			if (DataUtils.IPACharCache['\''] == null)
			{
				IPACharInfo charInfo = new IPACharInfo();
				charInfo.CharType = IPACharacterType.Suprasegmentals;
				charInfo.IsBaseChar = true;
				charInfo.IPAChar = "'";
				DataUtils.IPACharCache.Add((int)'\'', charInfo);
			}

			SearchQuery query = new SearchQuery();
			query.Pattern = "t/+_*";
			query.IgnoredStressChars = "'";
			query.IgnoredLengthChars = string.Empty;
			query.IgnoredToneChars = string.Empty;
			query.IgnoreDiacritics = false;

			int[] results;
			SearchEngine engine = new SearchEngine(query);
			Assert.IsFalse(engine.SearchWord("'tub", out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithMatchOnlyWordInitial()
		{
			m_query.Pattern = "t/*_*";

			int[] results;
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("teehee", true), out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithMatchOnlyWordFinal()
		{
			m_query.Pattern = "t/*_*";

			int[] results;
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("heeheet", true), out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithTieBarTest1()
		{
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", DataUtils.kTopTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", DataUtils.kTopTieBarC, false);
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", DataUtils.kBottomTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", DataUtils.kBottomTieBarC, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithTieBarTest2()
		{
			string patternFmt = "[t" + DataUtils.kTopTieBar + "s[{0}{1}]]/*_*";
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, DataUtils.kTopTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, DataUtils.kTopTieBarC, false);
			
			patternFmt = "[t" + DataUtils.kBottomTieBar + "s[{0}{1}]]/*_*";
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, DataUtils.kBottomTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, DataUtils.kBottomTieBarC, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SearchForTieBarWithDiacriticsInDataTest(string patternFmt, char tiebar,
			bool ignoreDiacritics)
		{
			string dental = "\u032A";
			string dentalT = "t" + dental;
			string dentalS = "s" + dental;

			m_query.Pattern = string.Format(patternFmt, DataUtils.kDottedCircle, dental);
			m_query.IgnoreDiacritics = ignoreDiacritics;

			int[] results;
			SearchEngine engine = new SearchEngine(m_query);

			// Target phone is: dental t/dental/top tie bar/s
			string word = string.Format("ab{0}{1}scd", dentalT, tiebar);
			string[] phones = DataUtils.IPACharCache.PhoneticParser(word, true);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			// Target phone is: dental t/top tie bar/s/dental
			word = string.Format("abct{0}{1}d", tiebar, dentalS);
			phones = DataUtils.IPACharCache.PhoneticParser(word, true);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(3, results[0]);
			Assert.AreEqual(1, results[1]);

			// Target phone is: dental t/dental/top tie bar/s/dental
			word = string.Format("a{0}{1}{2}cd", dentalT, tiebar, dentalS);
			phones = DataUtils.IPACharCache.PhoneticParser(word, true);
			Assert.IsTrue(engine.SearchWord(phones, out results));
			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the results for patterns containing diacritics to search for when those
		/// diacritics are in one of the ignore lists. When diacritics are explicitly part
		/// of a search pattern, then it doesn't matter whether or not they are in an ignore
		/// list. They should always be considered when explicitly entered into a pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWhenDiacriticInPatternAndIgnoredTest()
		{
			int[] results;

			MakeMockCacheEntries();
			m_query.IgnoredToneChars = "^,~";

			// Test when ignored diacritics in search item
			m_query.Pattern = "o~^/*_*";
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("abo~^cd", false), out results));

			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			// Test when ignored diacritics in environment before
			m_query.Pattern = "c/o~^_*";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("abo~^cd", false), out results));

			Assert.AreEqual(3, results[0]);
			Assert.AreEqual(1, results[1]);

			// Test when ignored diacritics in environment after
			m_query.Pattern = "b/*_o~^";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("abo~^cd", false), out results));

			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[[V][{0}~^]]/*_*", DataUtils.kDottedCircle);
			engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("abo~^cd", false), out results));

			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(1, results[1]);

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("o~^/*_*", DataUtils.kDottedCircle);
			engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("abo^cd", false), out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the results for patterns containing phones to search for when those
		/// phones are in one of the ignore lists. When a phone is explicitly part
		/// of a search pattern, then it doesn't matter whether or not it is ignored.
		/// It should always be treated as not ignored when explicitly entered into a pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWhenPhoneInPatternAndIgnoredTest()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredLengthChars = "b";
			query.IgnoredStressChars = string.Empty;
			query.IgnoredToneChars = string.Empty;
			query.IgnoreDiacritics = false;
			SearchEngine.CurrentSearchQuery = query;

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			int[] results;

			group.Parse("[V]b[V]");
			Assert.IsFalse(group.Search("xaax", 0, out results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a diacritic place holder is AND'd with an OR group
		/// containing phones without diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingOrGroupTest1()
		{
			int[] results;

			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[{{t,o}}[{0}~]]/*_*", DataUtils.kDottedCircleC);
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bct~de", false), out results));

			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a diacritic place holder is AND'd with an OR group
		/// containing phones with diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingOrGroupTest2()
		{
			int[] results;

			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			string pattern = "{t^,e}[0~]/*_*";
			m_query.Pattern = pattern.Replace('0', DataUtils.kDottedCircleC);
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bct^~de", false), out results));

			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);

			// Test when ignored diacritics in search item
			pattern = "[{t^,e}[0~]]/*_*";
			m_query.Pattern = pattern.Replace('0', DataUtils.kDottedCircleC);
			engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bct^~de", false), out results));

			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a diacritic place holder is AND'd with an OR group
		/// containing phones with diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingOrGroupTest3()
		{
			int[] results;

			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("{{{{t^,e}}{{o,a}}}}[{0}~]/*_*", DataUtils.kDottedCircleC);
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bct^~de", false), out results));

			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test diacritic placeholder following a feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingFeature()
		{
			int[] results;

			MakeMockCacheEntries();

			// Put mock data in the articulatory and binary feature caches.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			DataUtils.BFeatureCache["voice"].MinusMask = 8;

			DataUtils.AFeatureCache["nasal"].Mask = 16;
			DataUtils.AFeatureCache["nasal"].MaskNumber = 1;
			m_phoneCache["a"].Masks = new ulong[] { 0, 16 };
			m_phoneCache["o~"].Masks = new ulong[] { 0, 16 };

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[nasal][{0}~]/*_*", DataUtils.kDottedCircleC);
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bct^~de", false), out results));

			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test diacritic placeholder following an And group containing two features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingFeatureAndGroup()
		{
			int[] results;

			MakeMockCacheEntries();

			// Put mock data in the articulatory and binary feature caches.
			DataUtils.BFeatureCache["high"].PlusMask = 4;
			DataUtils.AFeatureCache["nasal"].Mask = 16;
			DataUtils.AFeatureCache["nasal"].MaskNumber = 1;
			m_phoneCache["a"].BinaryMask = 4;
			m_phoneCache["a"].Masks = new ulong[] { 0, 16 };
			m_phoneCache["o~"].BinaryMask = 4;
			m_phoneCache["o~"].Masks = new ulong[] { 0, 16 };

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[[+high][nasal]][{0}~]/*_*", DataUtils.kDottedCircleC);
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bct^~de", false), out results));

			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(1, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test ignoring undefined characters in a search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IgnoreUndefinedCharacters()
		{
			int[] results;

			DataUtils.IPACharCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			DataUtils.IPACharCache.UndefinedCharacters.Add('X', "XYZ");
			DataUtils.IPACharCache.UndefinedCharacters.Add('Y', "XYZ");
			DataUtils.IPACharCache.UndefinedCharacters.Add('Z', "XYZ");
			SearchEngine.IgnoreUndefinedCharacters = true;

			m_phoneCache.AddUndefinedPhone("X");
			m_phoneCache.AddUndefinedPhone("Y");
			m_phoneCache.AddUndefinedPhone("Z");

			m_query.Pattern = "ab/#_*";
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser(" XYaZbeiou", false), out results));

			Assert.AreEqual(3, results[0]);
			Assert.AreEqual(3, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test not ignoring undefined characters in a search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UnignoreUndefinedCharacters()
		{
			int[] results;

			DataUtils.IPACharCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			DataUtils.IPACharCache.UndefinedCharacters.Add('X', "XYZ");
			DataUtils.IPACharCache.UndefinedCharacters.Add('Y', "XYZ");
			DataUtils.IPACharCache.UndefinedCharacters.Add('Z', "XYZ");
			SearchEngine.IgnoreUndefinedCharacters = false;

			DataUtils.IPACharCache.AddUndefinedCharacter('X');
			DataUtils.IPACharCache.AddUndefinedCharacter('Y');
			DataUtils.IPACharCache.AddUndefinedCharacter('Z');
			m_phoneCache.AddUndefinedPhone("X");
			m_phoneCache.AddUndefinedPhone("Y");
			m_phoneCache.AddUndefinedPhone("Z");

			m_query.Pattern = "ab/*_*";
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsFalse(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("XYaZbeiou", false), out results));

			m_query.Pattern = "aZb/*_*";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("XYaZbeiou", false), out results));

			Assert.AreEqual(2, results[0]);
			Assert.AreEqual(3, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for runs of IPA characters when some of them are modified by
		/// diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IPACharRunsWithDiacriticsTest()
		{
			int[] results;

			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = "o~b/*_*";
			SearchEngine engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bct^~de", false), out results));

			Assert.AreEqual(1, results[0]);
			Assert.AreEqual(2, results[1]);

			m_query.Pattern = "o~t^/*_*";
			engine = new SearchEngine(m_query);
			Assert.IsTrue(
				engine.SearchWord(DataUtils.IPACharCache.PhoneticParser("ao~bco~t^xyz", false), out results));

			Assert.AreEqual(4, results[0]);
			Assert.AreEqual(2, results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void MakeMockCacheEntries()
		{
			AddMockDiacritics("~^");
			AddMockPhones(new string[] { "t~", "t^", "t~^" }, IPACharacterType.Consonant);
			AddMockPhones(new string[] { "o~", "o^", "o~^" }, IPACharacterType.Vowel);

			if (DataUtils.IPACharCache[' '] == null)
			{
				IPACharInfo charInfo = new IPACharInfo();
				charInfo.CharType = IPACharacterType.Breaking;
				charInfo.IsBaseChar = true;
				charInfo.IPAChar = " ";
				DataUtils.IPACharCache.Add((int)' ', charInfo);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds each character in the specified string to the IPA character cache as a
		/// diacritic.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddMockDiacritics(string diacritics)
		{
			foreach (char c in diacritics)
			{
				if (DataUtils.IPACharCache[c] == null)
				{
					// Setup a tilde in the cache to look like a diacritic
					IPACharInfo charInfo = new IPACharInfo();
					charInfo.CharType = IPACharacterType.Diacritics;
					charInfo.IsBaseChar = false;
					charInfo.IPAChar = c.ToString();
					DataUtils.IPACharCache.Add((int)c, charInfo);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified phones to the phone cache with the specified char. type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddMockPhones(string[] phones, IPACharacterType type)
		{
			foreach (string phone in phones)
			{
				if (!m_phoneCache.ContainsKey(phone))
				{
					TestPhoneInfo phoneInfo = new TestPhoneInfo();
					phoneInfo.CharType = type;
					m_phoneCache[phone] = phoneInfo;
				}
			}
		}
	}
}
