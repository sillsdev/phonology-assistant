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
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.TestUtils;
using System.Collections.Generic;

namespace SIL.Pa.Tests
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PatternGroupTests : TestBase
	{
		private SearchQuery m_query;
		private PhoneCache m_phoneCache;
		private SearchEngine m_engine;
		private int[] m_results;
		private PatternParser _parser;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the default state of the search query is that they contain nothing to
		/// ignore.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_parser = new PatternParser(_prj);
			App.DottedCircle = "0";
			App.DottedCircleC = '0';
			App.DiacriticPlaceholder = "[0]";
			m_phoneCache = _prj.PhoneCache;
			m_query = new SearchQuery();
			m_query.IgnoredCharacters = string.Empty;
			m_query.IgnoreDiacritics = false;
			m_engine = new SearchEngine(m_query);

			m_phoneCache["t"] = new PhoneInfo { Phone = "t" };
			m_phoneCache["t"].CharType = IPASymbolType.consonant;
			m_phoneCache["p"] = new PhoneInfo { Phone = "p" };
			m_phoneCache["p"].CharType = IPASymbolType.consonant;
			m_phoneCache["b"] = new PhoneInfo { Phone = "b" };
			m_phoneCache["b"].CharType = IPASymbolType.consonant;
			m_phoneCache["d"] = new PhoneInfo { Phone = "d" };
			m_phoneCache["d"].CharType = IPASymbolType.consonant;
			m_phoneCache["h"] = new PhoneInfo { Phone = "h" };
			m_phoneCache["h"].CharType = IPASymbolType.consonant;
			m_phoneCache["s"] = new PhoneInfo { Phone = "s" };
			m_phoneCache["s"].CharType = IPASymbolType.consonant;
			m_phoneCache["x"] = new PhoneInfo { Phone = "x" };
			m_phoneCache["x"].CharType = IPASymbolType.consonant;
			m_phoneCache["g"] = new PhoneInfo { Phone = "g" };
			m_phoneCache["g"].CharType = IPASymbolType.consonant;
			m_phoneCache["l"] = new PhoneInfo { Phone = "l" };
			m_phoneCache["l"].CharType = IPASymbolType.consonant;
			m_phoneCache["k"] = new PhoneInfo { Phone = "k" };
			m_phoneCache["k"].CharType = IPASymbolType.consonant;
			m_phoneCache["m"] = new PhoneInfo { Phone = "m" };
			m_phoneCache["m"].CharType = IPASymbolType.consonant;
			m_phoneCache["n"] = new PhoneInfo { Phone = "n" };
			m_phoneCache["n"].CharType = IPASymbolType.consonant;

			m_phoneCache["a"] = new PhoneInfo();
			m_phoneCache["a"].CharType = IPASymbolType.vowel;
			m_phoneCache["e"] = new PhoneInfo();
			m_phoneCache["e"].CharType = IPASymbolType.vowel;
			m_phoneCache["i"] = new PhoneInfo();
			m_phoneCache["i"].CharType = IPASymbolType.vowel;
			m_phoneCache["o"] = new PhoneInfo();
			m_phoneCache["o"].CharType = IPASymbolType.vowel;
			m_phoneCache["u"] = new PhoneInfo();
			m_phoneCache["u"].CharType = IPASymbolType.vowel;

			m_phoneCache["."] = new PhoneInfo();
			m_phoneCache["."].CharType = IPASymbolType.suprasegmental;
			m_phoneCache["'"] = new PhoneInfo();
			m_phoneCache["'"].CharType = IPASymbolType.suprasegmental;

			SearchEngine.PhoneCache = m_phoneCache;

			App.BFeatureCache.LoadFromList(new[]
			{
				new Feature { Name = "+high" },
				new Feature { Name = "+voice" },
			});
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
		/// Tests the Search method for AND and OR groups. This tests matches in members
		/// of type SinglePhone and one when the environment is the search item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchNonSequentialGroupsTest_SrchItem()
		{
			// Put mock data in the articulatory and binary feature caches.
			((PhoneInfo)m_phoneCache["b"]).BFeatureNames = new List<string>();
			((PhoneInfo)m_phoneCache["a"]).BFeatureNames = new List<string>();
			((PhoneInfo)m_phoneCache["d"]).BFeatureNames = new List<string> { "+high", "-voice" };
			((PhoneInfo)m_phoneCache["d"]).AFeatureNames = new List<string> { "+dental" };

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,d}[+high]]");
			Assert.IsTrue(group.Search("bad", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b,c,e}[+high]]");
			Assert.IsFalse(group.Search("bad", 0, out m_results));

			((PhoneInfo)m_phoneCache["a"]).BMask = App.BFeatureCache.GetMask(new List<string> { "+high", "-voice" });
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
        /// Tests the Search method for NOT groups in Item.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void SearchNonSequentialGroupsTest_NOTForItem()
        {
            PatternGroup group = new PatternGroup(EnvironmentType.Item);
            group.Parse("NOTa/#_*");
            Assert.IsTrue(group.Search("dlerdash", 0, out m_results));
            Assert.AreEqual(0, m_results[0]);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests the Search method for NOT groups in preceding environment.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void SearchNonSequentialGroupsTest_NOTForBefore()
        {
            PatternGroup group = new PatternGroup(EnvironmentType.Before);
            group.Parse("a/NOT#_*");
            Assert.IsTrue(group.Search("dlerdash", 0, out m_results));
            Assert.AreEqual(0, m_results[0]);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests the Search method for NOT groups in following environment.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void SearchNonSequentialGroupsTest_NOTForAfter()
        {
            PatternGroup group = new PatternGroup(EnvironmentType.After);
            group.Parse("a/*_NOT#");
            Assert.IsTrue(group.Search("dlerdash", 0, out m_results));
            Assert.AreEqual(0, m_results[0]);
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
			((PhoneInfo)m_phoneCache["d"]).BFeatureNames = new List<string> { "+high", "-voice" };
			((PhoneInfo)m_phoneCache["a"]).AFeatureNames = new List<string> { "nasal" };

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
			((PhoneInfo)m_phoneCache["d"]).BFeatureNames = new List<string> { "+high", "-voice" };

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
			((PhoneInfo)m_phoneCache["b"]).BFeatureNames = new List<string> { "+high", "-voice" };

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
			((PhoneInfo)m_phoneCache["h"]).BFeatureNames = new List<string> { "+high", "-voice" };

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

			group.Parse(string.Format("[[C][{0}~]]", App.DottedCircle));
			Assert.IsTrue(group.Search("ateit~ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group.Parse(string.Format("[[V][{0}~]]", App.DottedCircle));
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

			group.Parse(string.Format("[[C][{0}~*]]", App.DottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}^*]]", App.DottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}*]]", App.DottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(group.Search("ateit~^ou", 2, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}'*]]", App.DottedCircle));
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
				"c2345", "c2356", "c23456", "c234567"}, IPASymbolType.consonant);

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
				"c234",	"c2345", "c2356", "c23456", "c234567"}, IPASymbolType.consonant);

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
		private void TestDiacriticPattern(string pattern, string phone, bool expectedResult)
		{
			m_query = new SearchQuery();
			m_query.IgnoreDiacritics = true;
			m_query.Pattern = string.Format(pattern, App.DottedCircle);
			m_engine = new SearchEngine(m_query);
			string[] word = Parse("ae" + phone + "io", false);
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

			group.Parse(string.Format("[[V][{0}~*]]", App.DottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}^*]]", App.DottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}*]]", App.DottedCircle));
			Assert.IsTrue(group.Search("amo~^xyz", 0, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(group.Search("amo~^xyz", 2, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}'*]]", App.DottedCircle));
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

			group.Parse(string.Format("[[C][{0}~+]]", App.DottedCircle));
			Assert.IsTrue(group.Search("ateit~^ou", 0, out m_results));
			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			group.Parse(string.Format("[[C][{0}~+]]", App.DottedCircle));
			Assert.IsFalse(group.Search("ateit~ou", 0, out m_results));

			group.Parse(string.Format("[[C][{0}~+]]", App.DottedCircle));
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

			string pattern = string.Format("t[{0}~+]", App.DottedCircle);

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
		[Test]
		public void SearchWithIgnoreWithNonIPAPattern()
		{
			MakeMockCacheEntries();
			var query = new SearchQuery();
			query.IgnoredCharacters = "b";
			query.IgnoreDiacritics = false;
			m_engine = new SearchEngine(query);

			var group = new PatternGroup(EnvironmentType.Item);

			group.Parse("[V][V]");
			Assert.IsTrue(group.Search("xabax", 0, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForAnyVowConWithDiacriticsInData()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[V]/[C]_[C]";
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(Parse("t^at", true), out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(Parse("ieout^o~t", true), out m_results));
			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithBaseCharsInIgnoreList()
		{
			MakeMockCacheEntries();
			var query = new SearchQuery();
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;
			m_engine = new SearchEngine(query);

			var group = new PatternGroup(EnvironmentType.Item);

			group.Parse("aa");
			Assert.IsTrue(group.Search("xa.ax", 0, out m_results));

			group.Parse("[V][C]");
			Assert.IsTrue(group.Search("a.sa", 0, out m_results));
			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace1()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]#/*_*";
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsTrue(
				m_engine.SearchWord(Parse("his bat", true), out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace2()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]/#_*";
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = Parse(" his bat", true);
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace3()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]/*_#";
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = Parse("his bat ", true);
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			Assert.IsTrue(m_engine.SearchWord(out m_results));
			Assert.AreEqual(6, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace4()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[C]#[C]/*_*";
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(Parse("his bat", true), out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(3, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithSpace5()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[V]#[C]/*_*";
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = Parse("bea ", true);
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
		[Test]
		public void PatternWithNoBeforeEnvironment()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			string[] phones = Parse("beat", true);

			query.Pattern = "[V]/_*";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternWithNoAfterEnvironment()
		{
			MakeMockCacheEntries();
			var query = new SearchQuery();
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			var phones = Parse("beat", true);

			query.Pattern = "[V]/*_";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternWithNoEnvironment()
		{
			MakeMockCacheEntries();
			var query = new SearchQuery();
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			var phones = Parse("beat", true);

			query.Pattern = "[V]/_";
			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			query.Pattern = "[V]/";
			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));

			query.Pattern = "[V]";
			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));
			//Assert.AreEqual(1, m_results[0]);
			//Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchForVCWithVVCInWord()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "[V][C]/*_*";
			query.IgnoredCharacters = string.Empty;
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsTrue(m_engine.SearchWord("beat", out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordInitial1()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "h[V]/#_*";
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(" .xawadambo ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" .hawadambta ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" h.awadambta ", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordInitialButInPattern()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = ".abc/*_*";
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("abcdef", out m_results));
			Assert.IsTrue(m_engine.SearchWord(".abcdef", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordFinalButInPattern()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "ef./*_*";
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("abcdef", out m_results));
			Assert.IsTrue(m_engine.SearchWord("abcdef.", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordMedialButInPattern()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "b.c/*_*";
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("abcdef", out m_results));
			Assert.IsTrue(m_engine.SearchWord("ab.cdef", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoreFoundAtWordFinal()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "t[V]/*_#";
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(" hawadambo. ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" hawadambta. ", out m_results));
			Assert.IsTrue(m_engine.SearchWord(" hawadambt.a ", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnorePhoneFoundAtWordInitial_OneOrMore()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "h[V]/+_*";
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord(".xawadambo", out m_results));
			Assert.IsFalse(m_engine.SearchWord(".hawadambta", out m_results));
			Assert.IsTrue(m_engine.SearchWord("x.h.awadambta", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoredPhoneFoundAtWordFinal_OneOrMore()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.Pattern = "t[V]/*_+";
			query.IgnoredCharacters = ".";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("hawadambt.a", out m_results));
			Assert.IsFalse(m_engine.SearchWord("hawadambta.", out m_results));
			Assert.IsFalse(m_engine.SearchWord("hawadambt.a", out m_results));
			Assert.IsTrue(m_engine.SearchWord("hawadambta.a", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithIgnoringDiacritics()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoreDiacritics = true;
			SearchEngine.CurrentSearchQuery = query;

			var group = new PatternGroup(EnvironmentType.Item);

			group.Parse("t");
			Assert.IsTrue(group.Search("xat~ax", 0, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

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
		[Test]
		public void SearchWithDiacriticsInIgnoreList()
		{
			MakeMockCacheEntries();
			SearchQuery query = new SearchQuery();
			query.IgnoredCharacters = "~";
			query.IgnoreDiacritics = false;
			SearchEngine.CurrentSearchQuery = query;

			var group = new PatternGroup(EnvironmentType.Item);

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
			Assert.IsFalse(m_engine.SearchWord(Parse("taoub", false), out m_results));

			m_query.Pattern = "a/+t_+";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(Parse("taoub", false), out m_results));

			m_query.Pattern = "u/+_b+";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(Parse("taoub", false), out m_results));

			m_query.Pattern = "o/+ta_ub+";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(Parse("taoub", false), out m_results));
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
			Assert.IsTrue(m_engine.SearchWord(Parse("abxio", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(Parse("abxi", false), out m_results));
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
			Assert.IsTrue(m_engine.SearchWord(Parse("aixio", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(Parse("ixio", false), out m_results));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that when the first char. in a word is ignored, it's not matched to '+'
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithFirstPhoneIgnored()
		{
			if (App.IPASymbolCache['\''] == null)
			{
				IPASymbol charInfo = new IPASymbol();
				charInfo.Type = IPASymbolType.suprasegmental;
				charInfo.IsBase = true;
				charInfo.Literal = "'";
				App.IPASymbolCache.Add('\'', charInfo);
			}

			SearchQuery query = new SearchQuery();
			query.Pattern = "t/+_*";
			query.IgnoredCharacters = "'";
			query.IgnoreDiacritics = false;

			m_engine = new SearchEngine(query);
			Assert.IsFalse(m_engine.SearchWord("'tub", out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithMatchOnlyWordInitial()
		{
			m_query.Pattern = "t/*_*";

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("teehee", true), out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithMatchOnlyWordFinal()
		{
			m_query.Pattern = "t/*_*";

			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("heeheet", true), out m_results));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
        [Ignore]
		public void SearchWithTieBarTest1()
		{
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", App.kTopTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", App.kTopTieBarC, false);
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", App.kBottomTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest("[[C][{0}{1}]]/*_*", App.kBottomTieBarC, false);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchWithTieBarTest2()
		{
			string patternFmt = "[t" + App.kTopTieBar + "s[{0}{1}]]/*_*";
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, App.kTopTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, App.kTopTieBarC, false);
			
			patternFmt = "[t" + App.kBottomTieBar + "s[{0}{1}]]/*_*";
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, App.kBottomTieBarC, true);
			SearchForTieBarWithDiacriticsInDataTest(patternFmt, App.kBottomTieBarC, false);
		}

		/// ------------------------------------------------------------------------------------
		private void SearchForTieBarWithDiacriticsInDataTest(string patternFmt, char tiebar,
			bool ignoreDiacritics)
		{
			string dental = "\u032A";
			string dentalT = "t" + dental;
			string dentalS = "s" + dental;

			m_query.Pattern = string.Format(patternFmt, App.DottedCircle, dental);
			m_query.IgnoreDiacritics = ignoreDiacritics;

			m_engine = new SearchEngine(m_query);

			// Target phone is: dental t/dental/top tie bar/s
			string word = string.Format("ab{0}{1}scd", dentalT, tiebar);
			string[] phones = Parse(word, true);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Target phone is: dental t/top tie bar/s/dental
			word = string.Format("abct{0}{1}d", tiebar, dentalS);
			phones = Parse(word, true);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Target phone is: dental t/dental/top tie bar/s/dental
			word = string.Format("a{0}{1}{2}cd", dentalT, tiebar, dentalS);
			phones = Parse(word, true);
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
			m_query.IgnoredCharacters = "^,~";

			// Test when ignored diacritics in search item
			m_query.Pattern = "o~^/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(
				m_engine.SearchWord(Parse("abo~^cd", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in environment before
			m_query.Pattern = "c/o~^_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("abo~^cd", false), out m_results));

			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in environment after
			m_query.Pattern = "b/*_o~^";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("abo~^cd", false), out m_results));

			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[[V][{0}~^]]/*_*", App.DottedCircle);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("abo~^cd", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Test when ignored diacritics in search item
			m_query.Pattern = "o~^/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(Parse("abo^cd", false), out m_results));
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
			query.IgnoredCharacters = "b";
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
			m_query.Pattern = string.Format("[{{t,o}}[{0}~]]/*_*", App.DottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("ao~bct~de", false), out m_results));
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
			m_query.Pattern = "{[t[0~]],(bc)}/*_*";
			m_query.Pattern = m_query.Pattern.Replace("0", App.DottedCircle);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("ao~bct~de", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);
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
			string pattern = "[{t^,e}[0~]]/*_*";
			m_query.Pattern = pattern.Replace('0', App.DottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("ao~bct^~de", false), out m_results));

			Assert.AreEqual(4, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests searching results when a diacritic place holder is AND'd with an OR group
		/// (with insane number of braces) containing phones with diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiacriticPlaceholderFollowingOrGroupTest3()
		{
			MakeMockCacheEntries();

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[{{{{t^,e}},{{o,a}}}}[{0}~]]/*_*", App.DottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("ao~bct^~de", false), out m_results));

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

			((PhoneInfo)m_phoneCache["a"]).AFeatureNames = new List<string> { "nasal" };
			((PhoneInfo)m_phoneCache["o~"]).AFeatureNames = new List<string> { "nasal" };
	
			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[[nasal][{0}~]]/*_*", App.DottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("ao~bct^~de", false), out m_results));

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

			((PhoneInfo)m_phoneCache["a"]).BFeatureNames = new List<string> { "+high" };
			((PhoneInfo)m_phoneCache["a"]).AFeatureNames = new List<string> { "nasal" };
			((PhoneInfo)m_phoneCache["o~"]).BFeatureNames = new List<string> { "+high" };
			((PhoneInfo)m_phoneCache["o~"]).AFeatureNames = new List<string> { "nasal" };

			// Test when ignored diacritics in search item
			m_query.Pattern = string.Format("[[[+high][nasal]][{0}~]]/*_*", App.DottedCircleC);
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("ao~bct^~de", false), out m_results));

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
			App.IPASymbolCache.UndefinedCharacters.Add('X', "XYZ");
			App.IPASymbolCache.UndefinedCharacters.Add('Y', "XYZ");
			App.IPASymbolCache.UndefinedCharacters.Add('Z', "XYZ");
			SearchEngine.IgnoreUndefinedCharacters = true;

			m_phoneCache.AddUndefinedPhone("X");
			m_phoneCache.AddUndefinedPhone("Y");
			m_phoneCache.AddUndefinedPhone("Z");

			m_query.Pattern = "ab/#_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse(" XYaZbeiou", false), out m_results));

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
			App.IPASymbolCache.UndefinedCharacters.Add('X', "XYZ");
			App.IPASymbolCache.UndefinedCharacters.Add('Y', "XYZ");
			App.IPASymbolCache.UndefinedCharacters.Add('Z', "XYZ");
			SearchEngine.IgnoreUndefinedCharacters = false;

			App.IPASymbolCache.AddUndefinedCharacter('X');
			App.IPASymbolCache.AddUndefinedCharacter('Y');
			App.IPASymbolCache.AddUndefinedCharacter('Z');
			m_phoneCache.AddUndefinedPhone("X");
			m_phoneCache.AddUndefinedPhone("Y");
			m_phoneCache.AddUndefinedPhone("Z");

			m_query.Pattern = "ab/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(Parse("XYaZbeiou", false), out m_results));

			m_query.Pattern = "aZb/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("XYaZbeiou", false), out m_results));

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
			string[] phones = Parse("ao~bct^~de", false);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = "o~t^/*_*";
			m_engine = new SearchEngine(m_query);
			phones = Parse("ao~bco~t^xyz", false);
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
			string[] phones = Parse("beagle", false);

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
			string[] phones = Parse("beagle", false);

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
			string[] phones = Parse("beagle", false);

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
			string[] phones = Parse("ebeagle", false);

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
			string[] phones = Parse("beagle", false);

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
			string[] phones = Parse("beagle", false);

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
			string[] phones = Parse("beagule", false);

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
			string[] phones = Parse("bexagule", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));

			m_query.IgnoredCharacters = "x";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(3, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));

			m_query.Pattern = "{a,e}/{e,a}_*";
			phones = Parse("bexagule", false);

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
			string[] phones = Parse("bexagule", false);

			m_engine = new SearchEngine(m_query);
			Assert.IsFalse(m_engine.SearchWord(phones, out m_results));

			m_query.IgnoredCharacters = "x";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(phones, out m_results));
			Assert.AreEqual(1, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
			Assert.IsFalse(m_engine.SearchWord(out m_results));

			m_query.Pattern = "{a,e}/*_{e,a}";
			phones = Parse("bexagule", false);

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
			Assert.IsTrue(m_engine.SearchWord(Parse("aobctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = "{(bc),m}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("aobctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(2, m_results[1]);

			m_query.Pattern = "{m,(klbc),(klop),(obct),(obop)}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("aobctde", false), out m_results));

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
			Assert.IsTrue(m_engine.SearchWord(Parse("aobctde", false), out m_results));

			Assert.AreEqual(0, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			m_query.Pattern = "{([C][V]),m}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("aobctde", false), out m_results));

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
			Assert.IsTrue(m_engine.SearchWord(Parse("aobctde", false), out m_results));

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
			Assert.IsTrue(m_engine.SearchWord(Parse("aobctde", false), out m_results));

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
			m_query.IgnoredCharacters = "x";
			m_query.Pattern = "{(oc),(bc)}/*_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("aobxctde", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(3, m_results[1]);

			// Verify in preceding environment
			m_query.Pattern = "t/{(oc),(bc)}_*";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("aobxctde", false), out m_results));

			Assert.AreEqual(5, m_results[0]);
			Assert.AreEqual(1, m_results[1]);

			// Verify in following environment
			m_query.Pattern = "o/*_{(oc),(bc)}";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("aobxctde", false), out m_results));

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
			m_query.IgnoredCharacters = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("pau'pat", false), out m_results));

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
			m_query.IgnoredCharacters = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("phu'pat", false), out m_results));

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
			((PhoneInfo)m_phoneCache["u"]).AFeatureNames = new List<string> { "close", "back" };

			m_query.Pattern = "[C]/[[close][back]]_+";
			m_query.IgnoredCharacters = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("pau'pat", false), out m_results));

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
			((PhoneInfo)m_phoneCache["p"]).AFeatureNames = new List<string> { "bilabial", "voiceless" };

			m_query.Pattern = "[V]/+_[[bilabial][voiceless]]";
			m_query.IgnoredCharacters = "'";
			m_engine = new SearchEngine(m_query);
			Assert.IsTrue(m_engine.SearchWord(Parse("phu'pat", false), out m_results));

			Assert.AreEqual(2, m_results[0]);
			Assert.AreEqual(1, m_results[1]);
		}

		/// ------------------------------------------------------------------------------------
		private void MakeMockCacheEntries()
		{
			AddMockDiacritics("~^");
			AddMockPhones(new[] { "t~", "t^", "t~^" }, IPASymbolType.consonant);
			AddMockPhones(new[] { "o~", "o^", "o~^" }, IPASymbolType.vowel);

			if (App.IPASymbolCache[' '] == null)
			{
				var charInfo = new IPASymbol();
				charInfo.SubType = IPASymbolSubType.boundary;
				charInfo.IsBase = true;
				charInfo.Literal = " ";
				App.IPASymbolCache.Add(' ', charInfo);
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
				if (App.IPASymbolCache[c] == null)
				{
					// Setup a tilde in the cache to look like a diacritic
					IPASymbol charInfo = new IPASymbol();
					charInfo.Type = IPASymbolType.diacritic;
					charInfo.IsBase = false;
					charInfo.Literal = c.ToString();
					App.IPASymbolCache.Add(c, charInfo);
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
