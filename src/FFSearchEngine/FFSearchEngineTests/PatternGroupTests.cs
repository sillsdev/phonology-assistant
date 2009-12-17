using NUnit.Framework;
using SIL.Pa.Data;
using SIL.Pa.TestUtils;
using System.Collections.Generic;

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
		private SearchEngine m_engine;
		private int[] m_results;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			//DataUtils.LoadIPACharCache(null);
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

			m_phoneCache["t"] = new PhoneInfo();
			m_phoneCache["t"].CharType = IPASymbolType.Consonant;
			m_phoneCache["p"] = new PhoneInfo();
			m_phoneCache["p"].CharType = IPASymbolType.Consonant;
			m_phoneCache["b"] = new PhoneInfo();
			m_phoneCache["b"].CharType = IPASymbolType.Consonant;
			m_phoneCache["d"] = new PhoneInfo();
			m_phoneCache["d"].CharType = IPASymbolType.Consonant;
			m_phoneCache["h"] = new PhoneInfo();
			m_phoneCache["h"].CharType = IPASymbolType.Consonant;
			m_phoneCache["s"] = new PhoneInfo();
			m_phoneCache["s"].CharType = IPASymbolType.Consonant;
			m_phoneCache["x"] = new PhoneInfo();
			m_phoneCache["x"].CharType = IPASymbolType.Consonant;
			m_phoneCache["g"] = new PhoneInfo();
			m_phoneCache["g"].CharType = IPASymbolType.Consonant;
			m_phoneCache["l"] = new PhoneInfo();
			m_phoneCache["l"].CharType = IPASymbolType.Consonant;
			m_phoneCache["k"] = new PhoneInfo();
			m_phoneCache["k"].CharType = IPASymbolType.Consonant;
			m_phoneCache["m"] = new PhoneInfo();
			m_phoneCache["m"].CharType = IPASymbolType.Consonant;
			m_phoneCache["n"] = new PhoneInfo();
			m_phoneCache["n"].CharType = IPASymbolType.Consonant;

			m_phoneCache["a"] = new PhoneInfo();
			m_phoneCache["a"].CharType = IPASymbolType.Vowel;
			m_phoneCache["e"] = new PhoneInfo();
			m_phoneCache["e"].CharType = IPASymbolType.Vowel;
			m_phoneCache["i"] = new PhoneInfo();
			m_phoneCache["i"].CharType = IPASymbolType.Vowel;
			m_phoneCache["o"] = new PhoneInfo();
			m_phoneCache["o"].CharType = IPASymbolType.Vowel;
			m_phoneCache["u"] = new PhoneInfo();
			m_phoneCache["u"].CharType = IPASymbolType.Vowel;

			m_phoneCache["."] = new PhoneInfo();
			m_phoneCache["."].CharType = IPASymbolType.Suprasegmentals;
			m_phoneCache["'"] = new PhoneInfo();
			m_phoneCache["'"].CharType = IPASymbolType.Suprasegmentals;

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
			// Put mock data in the articulatory and binary features of the phone.
			PhoneInfo phoneInfo = (PhoneInfo)SearchEngine.PhoneCache["d"];
			phoneInfo.BFeatures = new List<string> { "+high", "-voice" };
			phoneInfo.AFeatures = new List<string> { "nasal" };

			object[] args = new object[] { new[] { "d" }, 0 };

			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("[{[+high],[-voice]}[C]]");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", args));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[{[-high],[+voice]}[C]]");
			Assert.AreEqual(CompareResultType.NoMatch, GetResult(group, "SearchGroup", args));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[[+high][+voice]],[nasal]}");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", args));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[[+high][+voice]],[dental]}");
			Assert.AreEqual(CompareResultType.NoMatch, GetResult(group, "SearchGroup", args));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[[+high][+voice]][nasal]]");
			Assert.AreEqual(CompareResultType.NoMatch, GetResult(group, "SearchGroup", args));

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[[+high][-voice]][nasal]]");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", args));

			args[0] = new[] { "a" };
			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[V]}");
			Assert.AreEqual(CompareResultType.Match, GetResult(group, "SearchGroup", args));
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
			((PhoneInfo)m_phoneCache["b"]).BFeatures = new List<string>();
			((PhoneInfo)m_phoneCache["a"]).BFeatures = new List<string>();
			((PhoneInfo)m_phoneCache["d"]).BFeatures = new List<string> { "+high", "-voice" };
			((PhoneInfo)m_phoneCache["d"]).AFeatures = new List<string> { "+dental" };

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,d}[+high]]");
			Assert.IsTrue(group.Search("bad", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,e}[+high]]");
			Assert.IsFalse(group.Search("bad", 0, out m_results));

			((PhoneInfo)m_phoneCache["a"]).BFeatures = new List<string> { "+high", "-voice" };
			//m_phoneCache["a"].BinaryMask = (4 | 8);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,e}[+high]]");
			Assert.IsTrue(group.Search("bad", 1, out m_results));
			Assert.AreEqual(1, m_results[0]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("{a,b,c,e}");
			Assert.IsTrue(group.Search("balderdash", 2, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.IsTrue(group.Search("balderdash", 5, out m_results));
			Assert.AreEqual(7, m_results[0]);
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
			PatternGroup group = new PatternGroup(EnvironmentType.Before);
			group.Parse("{a,b,c,e}");
			Assert.IsTrue(group.Search("badlerdash", 4, out m_results));
			Assert.AreEqual(4, m_results[0]);
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
			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("{a,b,c,e}");
			Assert.IsTrue(group.Search("badlerdash", 4, out m_results));
			Assert.AreEqual(4, m_results[0]);
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
			((PhoneInfo)m_phoneCache["d"]).BFeatures = new List<string> { "+high", "-voice" };
			((PhoneInfo)m_phoneCache["a"]).AFeatures = new List<string> { "nasal" };

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("der[+high]");
			Assert.IsTrue(group.Search("balderdash", 0, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(4, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("der[+high][nasal]");
			Assert.IsTrue(group.Search("balderdash", 0, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(5, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("al[+high]{i,a,e}");
			Assert.IsTrue(group.Search("balderdash", 1, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(4, m_results[1]);

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
			((PhoneInfo)m_phoneCache["d"]).BFeatures = new List<string> { "+high", "-voice" };

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
			((PhoneInfo)m_phoneCache["b"]).BFeatures = new List<string> { "+high", "-voice" };

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
			((PhoneInfo)m_phoneCache["h"]).BFeatures = new List<string> { "+high", "-voice" };

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

			group.Parse(string.Format("[[C][{0}~]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group.Parse(string.Format("[[V][{0}~]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("anmo~xyz", 0, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			group.Parse(string.Format("[[C][{0}~*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}^*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(group.Search("ateit~^ou", 2, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}'*]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("ateit~^ou", 0, out m_results));
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
			AddMockPhones(new[] {"a", "e", "c", "i", "o", "c2", "c23", "c234",
				"c2345", "c2356", "c23456", "c234567"}, IPASymbolType.Consonant);

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
			AddMockPhones(new[] {"a", "e", "c", "i", "o", "c2", "c23", "c32",
				"c234",	"c2345", "c2356", "c23456", "c234567"}, IPASymbolType.Consonant);

			//TestDiacriticPattern("[[C][{0}+]]/*_*", "c", false);
			//TestDiacriticPattern("[[C][{0}+]]/*_*", "c2", true);
			//TestDiacriticPattern("[[C][{0}2+]]/*_*", "c2", false);
			//TestDiacriticPattern("[[C][{0}2+]]/*_*", "c23", true);
			//TestDiacriticPattern("[[C][{0}+2]]/*_*", "c2", false);
			//TestDiacriticPattern("[[C][{0}+3]]/*_*", "c23", true);
			//TestDiacriticPattern("[[C][{0}23+56]]/*_*", "c2356", false);
			//TestDiacriticPattern("[[C][{0}23+56]]/*_*", "c23456", true);

			TestDiacriticPattern("[[C][{0}+]]/*_*", "c", false);
			TestDiacriticPattern("[[C][{0}+]]/*_*", "c2", true);
			TestDiacriticPattern("[[C][{0}2+]]/*_*", "c2", false);
			TestDiacriticPattern("[[C][{0}2+]]/*_*", "c23", true);
			TestDiacriticPattern("[[C][{0}2+]]/*_*", "c32", true);
			TestDiacriticPattern("[[C][{0}2356+]]/*_*", "c2356", false);
			TestDiacriticPattern("[[C][{0}2356+]]/*_*", "c23456", true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void TestDiacriticPattern(string pattern, string phone, bool expectedResult)
		{
			m_query = new SearchQuery();
			m_query.IgnoreDiacritics = true;
			m_query.Pattern = string.Format(pattern, DataUtils.kDottedCircle);
			m_engine = new SearchEngine(m_query);
			string[] word = DataUtils.IPASymbolCache.PhoneticParser("ae" + phone + "io", false);
			Assert.AreEqual(expectedResult, m_engine.SearchWord(word, out m_results));

			if (expectedResult)
			{
				Assert.AreEqual(2, m_results[0]);
				Assert.AreEqual(1, m_results[1]);
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

			group.Parse(string.Format("[[V][{0}~*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}^*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}*]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(group.Search("amo~^xyz", 2, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}'*]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("amo~^xyz", 0, out m_results));
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

			group.Parse(string.Format("[[C][{0}~+]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			group.Parse(string.Format("[[C][{0}~+]]", DataUtils.kDottedCircle));
			Assert.IsFalse(group.Search("ateit~ou", 0, out m_results));

			group.Parse(string.Format("[[C][{0}~+]]", DataUtils.kDottedCircle));
			Assert.IsTrue(group.Search("at~eit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			string pattern = string.Format("t[{0}~+]", DataUtils.kDottedCircle);

			group.Parse(pattern);
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			group.Parse(pattern);
			Assert.IsFalse(group.Search("ateit~ou", 0, out m_results));

			group.Parse(pattern);
			Assert.IsTrue(group.Search("at~eit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			group.Parse("[V][V]");
			Assert.IsTrue(group.Search("xabax", 0, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
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

			m_engine = new SearchEngine(query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("t^at", true), out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_engine = new SearchEngine(query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ieout^o~t", true), out m_results));
			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			group.Parse("aa");
			Assert.IsTrue(group.Search("xa.ax", 0, out m_results));

			group.Parse("[V][C]");
			Assert.IsTrue(group.Search("a.sa", 0, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
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

			m_engine = new SearchEngine(query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("his bat", true), out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
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

			string[] phones = DataUtils.IPASymbolCache.PhoneticParser(" his bat", true);
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("his bat ", true);
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(6, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			m_engine = new SearchEngine(query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("his bat", true), out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
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

			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("bea ", true);
			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));

			query.Pattern = "[V]/*_#";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			query.Pattern = "[V]#/*_*";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			query.Pattern = "[V]#/*_+";
			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));
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

			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beat", true);

			query.Pattern = "[V]/_*";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beat", true);

			query.Pattern = "[V]/*_";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beat", true);

			query.Pattern = "[V]/_";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			query.Pattern = "[V]/";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			query.Pattern = "[V]";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord("beat", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
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

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(" .xawadambo ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" .hawadambta ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" h.awadambta ", out m_results));
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

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("abcdef", out m_results));
			Assert.IsTrue(m_engine.SearchWord(".abcdef", out m_results));
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

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("abcdef", out m_results));
			Assert.IsTrue(m_engine.SearchWord("abcdef.", out m_results));
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

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("abcdef", out m_results));
			Assert.IsTrue(m_engine.SearchWord("ab.cdef", out m_results));
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

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(" hawadambo. ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" hawadambta. ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" hawadambt.a ", out m_results));
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

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(".xawadambo", out m_results));
			Assert.IsFalse(m_engine.SearchWord(".hawadambta", out m_results));
			Assert.IsTrue(m_engine.SearchWord("x.h.awadambta", out m_results));
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

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("hawadambt.a", out m_results));
			Assert.IsFalse(m_engine.SearchWord("hawadambta.", out m_results));
			Assert.IsFalse(m_engine.SearchWord("hawadambt.a", out m_results));
			Assert.IsTrue(m_engine.SearchWord("hawadambta.a", out m_results));
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

			group.Parse("t");
			Assert.IsTrue(group.Search("xat~ax", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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

			group.Parse("t");
			Assert.IsFalse(group.Search("xat~ax", 0, out m_results));
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

			group.Parse("t");
			Assert.IsTrue(group.Search("xat~ax", 0, out m_results));

			group.Parse("t");
			Assert.IsFalse(group.Search("xat^~ax", 0, out m_results));

			group.Parse("t");
			Assert.IsFalse(group.Search("xat~^ax", 0, out m_results));
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
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "o/*_u*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "o/*a_u*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "t/*u_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("tatoutb", out m_results));
			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord("taoub", out m_results));

			m_query.Pattern = "o/*_x*";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord("taoub", out m_results));

			m_query.Pattern = "o/*x_x*";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord("taoub", out m_results));

			m_query.Pattern = "t/*u_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord("taoub", out m_results));
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
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "o/+_u+";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "o/+a_u+";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "ao/+_+";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = "a[V]/+_+";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord("taoub", out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
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
			m_query.Pattern = "t/+_+";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("taoub", false), out m_results));

			m_query.Pattern = "a/+t_+";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("taoub", false), out m_results));

			m_query.Pattern = "u/+_b+";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("taoub", false), out m_results));

			m_query.Pattern = "o/+ta_ub+";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("taoub", false), out m_results));
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

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("abxio", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("abxi", false), out m_results));
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

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("aixio", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ixio", false), out m_results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that when the first char. in a word is ignored, it's not matched to '+'
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithFirstPhoneIgnored()
		{
			if (DataUtils.IPASymbolCache['\''] == null)
			{
				IPASymbol charInfo = new IPASymbol();
				charInfo.Type = IPASymbolType.Suprasegmentals;
				charInfo.IsBase = true;
				charInfo.Literal = "'";
				DataUtils.IPASymbolCache.Add((int)'\'', charInfo);
			}

			SearchQuery query = new SearchQuery();
			query.Pattern = "t/+_*";
			query.IgnoredStressChars = "'";
			query.IgnoredLengthChars = string.Empty;
			query.IgnoredToneChars = string.Empty;
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("'tub", out m_results));
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

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("teehee", true), out m_results));
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

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("heeheet", true), out m_results));
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

			m_engine = new SearchEngine(m_query);

			// Target phone is: dental t/dental/top tie bar/s
			string word = string.Format("ab{0}{1}scd", dentalT, tiebar);
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser(word, true);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Target phone is: dental t/top tie bar/s/dental
			word = string.Format("abct{0}{1}d", tiebar, dentalS);
			phones = DataUtils.IPASymbolCache.PhoneticParser(word, true);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Target phone is: dental t/dental/top tie bar/s/dental
			word = string.Format("a{0}{1}{2}cd", dentalT, tiebar, dentalS);
			phones = DataUtils.IPASymbolCache.PhoneticParser(word, true);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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
			MakeMockCacheEntries();
			m_query.IgnoredToneChars = "^,~";

			// Test when ignored diacritics in search item
			m_query.Pattern = "o~^/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("abo~^cd", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in environment before
			m_query.Pattern = "c/o~^_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("abo~^cd", false), out m_results));

			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in environment after
			m_query.Pattern = "b/*_o~^";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("abo~^cd", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[[V][{0}~^]]/*_*", DataUtils.kDottedCircle);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("abo~^cd", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in search item
			m_query.Pattern = "o~^/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("abo^cd", false), out m_results));
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

			group.Parse("[V]b[V]");
			Assert.IsFalse(group.Search("xaax", 0, out m_results));
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
			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[{{t,o}}[{0}~]]/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct~de", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a diacritic place holder is AND'd with an OR group
		/// containing phones without diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderInOrGroupWithTest1()
		{
			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("{{[t[{0}~]],(bc)}}/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct~de", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = string.Format("{{(t[{0}~]),(bt)}}/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct~de", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = string.Format("{{t[{0}~],(bt)}}/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct~de", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = string.Format("{{[t[{0}~]],(bt)}}/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct~de", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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
			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			string pattern = "{t^,e}[0~]/*_*";
			m_query.Pattern = pattern.Replace('0', DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct^~de", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in search item
			pattern = "[{t^,e}[0~]]/*_*";
			m_query.Pattern = pattern.Replace('0', DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct^~de", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
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
			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("{{{{t^,e}}{{o,a}}}}[{0}~]/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct^~de", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test diacritic placeholder following a feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingFeature()
		{
			MakeMockCacheEntries();

			((PhoneInfo)m_phoneCache["a"]).AFeatures = new List<string> { "nasal" };
			((PhoneInfo)m_phoneCache["o~"]).AFeatures = new List<string> { "nasal" };
	
			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[nasal][{0}~]/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct^~de", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test diacritic placeholder following an And group containing two features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingFeatureAndGroup()
		{
			MakeMockCacheEntries();

			((PhoneInfo)m_phoneCache["a"]).BFeatures = new List<string> { "+high" };
			((PhoneInfo)m_phoneCache["a"]).AFeatures = new List<string> { "nasal" };
			((PhoneInfo)m_phoneCache["o~"]).BFeatures = new List<string> { "+high" };
			((PhoneInfo)m_phoneCache["o~"]).AFeatures = new List<string> { "nasal" };

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[[+high][nasal]][{0}~]/*_*", DataUtils.kDottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("ao~bct^~de", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test ignoring undefined characters in a search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IgnoreUndefinedCharacters()
		{
			DataUtils.IPASymbolCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			DataUtils.IPASymbolCache.UndefinedCharacters.Add('X', "XYZ");
			DataUtils.IPASymbolCache.UndefinedCharacters.Add('Y', "XYZ");
			DataUtils.IPASymbolCache.UndefinedCharacters.Add('Z', "XYZ");
			SearchEngine.IgnoreUndefinedCharacters = true;

			m_phoneCache.AddUndefinedPhone("X");
			m_phoneCache.AddUndefinedPhone("Y");
			m_phoneCache.AddUndefinedPhone("Z");

			m_query.Pattern = "ab/#_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser(" XYaZbeiou", false), out m_results));

			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test not ignoring undefined characters in a search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UnignoreUndefinedCharacters()
		{
			DataUtils.IPASymbolCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			DataUtils.IPASymbolCache.UndefinedCharacters.Add('X', "XYZ");
			DataUtils.IPASymbolCache.UndefinedCharacters.Add('Y', "XYZ");
			DataUtils.IPASymbolCache.UndefinedCharacters.Add('Z', "XYZ");
			SearchEngine.IgnoreUndefinedCharacters = false;

			DataUtils.IPASymbolCache.AddUndefinedCharacter('X');
			DataUtils.IPASymbolCache.AddUndefinedCharacter('Y');
			DataUtils.IPASymbolCache.AddUndefinedCharacter('Z');
			m_phoneCache.AddUndefinedPhone("X");
			m_phoneCache.AddUndefinedPhone("Y");
			m_phoneCache.AddUndefinedPhone("Z");

			m_query.Pattern = "ab/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("XYaZbeiou", false), out m_results));

			m_query.Pattern = "aZb/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(DataUtils.IPASymbolCache.PhoneticParser("XYaZbeiou", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
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
			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = "o~b/*_*";
			m_engine = new SearchEngine(m_query);
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("ao~bct^~de", false);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = "o~t^/*_*";
			m_engine = new SearchEngine(m_query);
			phones = DataUtils.IPASymbolCache.PhoneticParser("ao~bco~t^xyz", false);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in search item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInSearchItem()
		{
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beagle", false);

			m_query.Pattern = "{a,e}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "{e,a}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in environment before.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInEnvBefore()
		{
			m_query.Pattern = "[C]/{e,a}_*";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beagle", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));

			m_query.Pattern = "[C]/{a,e}_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in environment before.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInEnvAfter()
		{
			m_query.Pattern = "[C]/*_{e,a}";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beagle", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "[C]/*_{a,e}";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in the search item and environment before.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInItemAndEnvBefore()
		{
			m_query.Pattern = "{g,b}/{e,a}_*";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("ebeagle", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			
			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "{g,b}/{a,e}_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in the search item and environment after.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInItemAndEnvAfter()
		{
			m_query.Pattern = "{l,b}/*_{e,a}";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beagle", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "{l,b}/*_{a,e}";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in the environment before and after.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInEnvBeforeAndAfter()
		{
			m_query.Pattern = "[C]/{b,l,d,m,g,n}_{a,e}";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beagle", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in the search item and the environments
		/// before and after.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInSrchItemAndEnvBeforeAndAfter()
		{
			m_query.Pattern = "{i,u}/{b,m,g,n}_{l,d}";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("beagule", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in the search item and the environments
		/// before when there's an ignored phone in the env. before.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInSrchItemAndEnvBeforeWithIgnore()
		{
			m_query.Pattern = "{e,a}/{a,e}_*";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("bexagule", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));

			m_query.IgnoredLengthChars = "x";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));

			m_query.Pattern = "{a,e}/{e,a}_*";
			phones = DataUtils.IPASymbolCache.PhoneticParser("bexagule", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching for phones in an OR group in the search item and the environments
		/// after when there's an ignored phone in the env. after.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupTestInSrchItemAndEnvAfterWithIgnore()
		{
			m_query.Pattern = "{e,a}/*_{a,e}";
			string[] phones = DataUtils.IPASymbolCache.PhoneticParser("bexagule", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));

			m_query.IgnoredLengthChars = "x";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));

			m_query.Pattern = "{a,e}/*_{e,a}";
			phones = DataUtils.IPASymbolCache.PhoneticParser("bexagule", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a sequence of phones is contained in an OR group
		/// in search item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupWithSequencesTest1()
		{
			MakeMockCacheEntries();

			m_query.Pattern = "{m,(bc)}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = "{(bc),m}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = "{m,(klbc),(klop),(obct),(obop)}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(4, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a sequence of C or V classes is contained in an
		/// OR group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupWithSequencesTest2()
		{
			MakeMockCacheEntries();

			m_query.Pattern = "{([C][V]),[V]}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "{([C][V]),m}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a sequence of phones is contained in an OR group
		/// in preceding environment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupWithSequencesInPrecedingEnv()
		{
			MakeMockCacheEntries();

			m_query.Pattern = "t/{m,(obc),(ob)}_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a sequence of phones is contained in an OR group
		/// in following environment.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupWithSequencesInFollowingEnv()
		{
			MakeMockCacheEntries();

			m_query.Pattern = "b/*_{m,(tde),(ct)}";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a sequence of phones is contained in an OR group
		/// when the data contains ignored phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OrGroupWithSequencesAndIgnoredPhones()
		{
			MakeMockCacheEntries();

			// Verify in search item.
			m_query.IgnoredLengthChars = "x";
			m_query.Pattern = "{(oc),(bc)}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobxctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(3, m_results[1]);

			// Verify in preceding environment
			m_query.Pattern = "t/{(oc),(bc)}_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobxctde", false), out m_results));

			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Verify in following environment
			m_query.Pattern = "o/*_{(oc),(bc)}";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobxctde", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests fixes for PA-702
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWhenIgnoringStressAndOrGroupInPrecedingEnv()
		{
			m_query.Pattern = "[C]/{[C],[V]}_+";
			m_query.IgnoredStressChars = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("pau'pat", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests fixes for PA-702
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWhenIgnoringStressAndOrGroupInFollowingEnv()
		{
			m_query.Pattern = "[V]/+_{[C],[V]}";
			m_query.IgnoredStressChars = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("phu'pat", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests fixes for PA-702
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWhenIgnoringStressAndAndGroupInPrecedingEnv()
		{
			((PhoneInfo)m_phoneCache["u"]).AFeatures = new List<string> { "close", "back" };

			m_query.Pattern = "[C]/[[close],[back]]_+";
			m_query.IgnoredStressChars = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("pau'pat", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests fixes for PA-702
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWhenIgnoringStressAndAndGroupInFollowingEnv()
		{
			((PhoneInfo)m_phoneCache["p"]).AFeatures = new List<string> { "bilabial", "voiceless" };

			m_query.Pattern = "[V]/+_[[bilabial],[voiceless]]";
			m_query.IgnoredStressChars = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("phu'pat", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests sequences of phones are surrounded by parentheses.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ParentheticalSequencesTest1()
		{
			MakeMockCacheEntries();

			m_query.Pattern = "(bct)/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("aobctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(3, m_results[1]);

			m_query.Pattern = "a(bcd)([C][V]){x,z}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(
				DataUtils.IPASymbolCache.PhoneticParser("omnabcdhizstu", false), out m_results));

			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(7, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void MakeMockCacheEntries()
		{
			AddMockDiacritics("~^");
			AddMockPhones(new[] { "t~", "t^", "t~^" }, IPASymbolType.Consonant);
			AddMockPhones(new[] { "o~", "o^", "o~^" }, IPASymbolType.Vowel);

			if (DataUtils.IPASymbolCache[' '] == null)
			{
				IPASymbol charInfo = new IPASymbol();
				charInfo.Type = IPASymbolType.Breaking;
				charInfo.IsBase = true;
				charInfo.Literal = " ";
				DataUtils.IPASymbolCache.Add((int)' ', charInfo);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds each character in the specified string to the IPA character cache as a
		/// diacritic.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddMockDiacritics(string diacritics)
		{
			foreach (char c in diacritics)
			{
				if (DataUtils.IPASymbolCache[c] == null)
				{
					// Setup a tilde in the cache to look like a diacritic
					IPASymbol charInfo = new IPASymbol();
					charInfo.Type = IPASymbolType.Diacritics;
					charInfo.IsBase = false;
					charInfo.Literal = c.ToString();
					DataUtils.IPASymbolCache.Add(c, charInfo);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified phones to the phone cache with the specified char. type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddMockPhones(IEnumerable<string> phones, IPASymbolType type)
		{
			foreach (string phone in phones)
			{
				if (!m_phoneCache.ContainsKey(phone))
				{
					PhoneInfo phoneInfo = new PhoneInfo();
					phoneInfo.CharType = type;
					m_phoneCache[phone] = phoneInfo;
				}
			}
		}
	}
}
