// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: SortTests.cs
// Responsibility: DavidO & ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;
using SIL.Pa.Data;
using SIL.Pa.FFSearchEngine;

namespace SIL.Pa
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests phonetic sorting.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class SortTests : TestBase
	{
		#region Declarations
		private RecordCache m_recCache;
		private WordCache m_wordCache;
		private WordListCache m_cache;
		private PaDataSource m_dataSource;
		private SearchQuery m_query;
		private SortOptions m_sortOptions;
		private PaFieldInfo m_phoneticFieldInfo;
		private PaFieldInfo m_cvcFieldInfo;
		private const string kPhonetic = "Phonetic";
		private const string kCvc = "CVC";
		private const bool kAscending = true;
		private const bool kDescending = false;
		#endregion

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
			
			PaProject proj = new PaProject(true);
			proj.Language = "dummy";
			proj.ProjectName = "dummy";
			PaApp.Project = proj;

			m_dataSource = new PaDataSource();
			m_dataSource.DataSourceType = DataSourceType.Toolbox;

			m_phoneticFieldInfo = PaApp.FieldInfo.PhoneticField;
			m_cvcFieldInfo = PaApp.FieldInfo.CVPatternField;
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_recCache = new RecordCache();
			PaApp.RecordCache = m_recCache;
			m_cache = new WordListCache();
			m_query = new SearchQuery();
			m_sortOptions = new SortOptions();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddWords(string words, string pattern)
		{
			m_recCache.Clear();
			RecordCacheEntry entry = new RecordCacheEntry(true);
			entry.SetValue(kPhonetic, words);
			entry.SetValue(kCvc, pattern);
			entry.NeedsParsing = true;
			entry.DataSource = m_dataSource;
			m_recCache.Add(entry);
			m_wordCache = m_recCache.BuildWordCache(null);

			PaApp.BuildPhoneCache();

			m_cache.Clear();
			foreach (WordCacheEntry wcEntry in m_wordCache)
				m_cache.Add(wcEntry);
		}
		#endregion

		#region Unicode Sort Tests
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests ascending unicode sorting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortUnicodeAscTest()
		{
			AddWords("fib bit ebay bitter drillbit abdiging",
							"cvc cvc vcvc cvccvc ccvcccvc vccvcvcc");
			m_sortOptions.SortType = PhoneticSortType.Unicode;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "abdiging", "bit", "bitter", "drillbit", "ebay", "fib" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests decending unicode sorting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortUnicodeDescTest()
		{
			AddWords("fib bit ebay bitter drillbit abdiging",
							"cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_sortOptions.SortType = PhoneticSortType.Unicode;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kDescending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "fib", "ebay", "drillbit", "bitter", "bit", "abdiging" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests nested sort with ascending cvc and descending unicode.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortCvcAscUnicodeDescTest()
		{
			AddWords("fib bit dig ebay bitter digger drillbit abdiging",
							"cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "i/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.Unicode;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kDescending); // 2nd sort
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.CVPatternField, false, kAscending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(9, cache.Count);
			string[] words =
				{ "drillbit", "drillbit", "fib", "dig", "bit", "digger", "bitter", "abdiging", "abdiging" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests ascending unicode adv sorting with defaults.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheAdvDefaultsSortUnicodeAscTest()
		{
			AddWords("fib bit dig ebay bitter digger drillbit abdiging",
							"cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "b/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.Unicode;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "bit", "bitter", "abdiging", "ebay", "fib", "drillbit" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests ascending unicode adv sorting with modified sort order & Rl options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheAdvSortUnicodeAscTest()
		{
			AddWords("fib bit dig ebay bitter digger drillbit abdiging",
							"cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "b/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.Unicode;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new int[] { 2, 1, 0 };
			m_sortOptions.AdvRlOptions = new bool[] { false, true, true };
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "fib", "abdiging", "bitter", "bit", "drillbit", "ebay" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}
		#endregion

		#region POA Sort Tests
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests ascending POA sorting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortPoaAscTest()
		{
			AddWords("fib bit ebay bitter drillbit abdiging",
							"cvc cvc vcvc cvccvc ccvcccvc vccvcvcc");
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "bit", "bitter", "fib", "drillbit", "ebay", "abdiging" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests decending POA sorting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortPoaDescTest()
		{
			AddWords("fib bit dig ebay bitter digger drillbit abdiging",
							"cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "b/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kDescending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "abdiging", "ebay", "drillbit", "fib", "bitter", "bit" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests nested sort with ascending cvc and descending poa.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortCvcAscPoaDescTest()
		{
			AddWords("fib bit dig ebay bitter digger drillbit abdiging",
							"cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "b/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kDescending); // 2nd sort
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.CVPatternField, false, kAscending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "drillbit", "fib", "bit", "bitter", "abdiging", "ebay" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests nested ascending cvc and descending poa adv sorting with modified 
		/// sort order & Rl options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheAdvSortCvcAscPoaDescTest()
		{
			AddWords("pig fib bit dig ebay bitter digger drillbit abdiging",
							"cvc cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "i/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new int[] { 2, 0, 1 };
			m_sortOptions.AdvRlOptions = new bool[] { true, false, true };
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kDescending); // 2nd sort
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.CVPatternField, false, kAscending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(10, cache.Count);
			// Expected answer
			string[] words = { "drillbit", "drillbit", "dig", "pig", "bit", "fib", "digger", "bitter", "abdiging", "abdiging" };
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test that R/L sorting is done properly when some phones have modifying diacritics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheAdvSortPoaRLTest()
		{
			AddWords("pig big p\u02B0ig", null);
			m_query.Pattern = "[V]/*_*";
			WordListCache cache = PaApp.Search(m_query);
			Assert.IsNotNull(cache);

			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new int[] { 1, 0, 2 };
			m_sortOptions.AdvRlOptions = new bool[] { true, false, false };
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(3, cache.Count);
			string[] words = { "pig", "p\u02B0ig", "big" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests ascending POA simple sorting with diacritic.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SimplePOASortTest1()
		{
			AddWords("b\u0324it bag", null);
			WordListCache cache = new WordListCache();
			foreach (WordCacheEntry entry in m_wordCache)
				cache.Add(entry);

			Assert.IsNotNull(cache);

			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = false;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(2, cache.Count);
			string[] words = { "bag", "b\u0324it" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests ascending POA advanced sorting with diacritic.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AdvancedPOASortTest1()
		{
			AddWords("mappu sippu", null);
			m_query.Pattern = "p/*_*";
			WordListCache cache = PaApp.Search(m_query);

			Assert.IsNotNull(cache);

			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new int[] { 1, 0, 2 };
			m_sortOptions.AdvRlOptions = new bool[] { true, false, false };
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(4, cache.Count);
			Assert.AreEqual("sippu", cache[0].PhoneticValue);
			Assert.AreEqual(3, cache[0].SearchItemPhoneOffset);
			Assert.AreEqual("mappu", cache[1].PhoneticValue);
			Assert.AreEqual(3, cache[1].SearchItemPhoneOffset);
			Assert.AreEqual("sippu", cache[2].PhoneticValue);
			Assert.AreEqual(2, cache[2].SearchItemPhoneOffset);
			Assert.AreEqual("mappu", cache[3].PhoneticValue);
			Assert.AreEqual(2, cache[3].SearchItemPhoneOffset);
		}
		#endregion

		#region MOA Sort Tests
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests ascending MOA sorting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortMoaAscTest()
		{
			AddWords("fib bit ebay bitter drillbit abdiging",
							"cvc cvc vcvc cvccvc ccvcccvc vccvcvcc");
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "bit", "bitter", "drillbit", "fib", "ebay", "abdiging" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests descending MOA sorting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortMoaDescTest()
		{
			AddWords("fib bit ebay bitter drillbit abdiging",
							"cvc cvc vcvc cvccvc ccvcccvc vccvcvcc");
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kDescending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "abdiging", "ebay", "fib", "drillbit", "bitter", "bit" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests nested sort with descending cvc and ascending moa.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortCvcDescMoaAscTest()
		{
			AddWords("fib bit dig ebay bitter digger drillbit abdiging",
							"cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "b/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending); // 2nd sort
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.CVPatternField, false, kDescending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "ebay", "abdiging", "bitter", "bit", "fib", "drillbit" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests descending MOA adv sorting with modified ort order & Rl options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheAdvSortMoaDescTest()
		{
			AddWords("rabbit bir fib bit dig ebay bitter digger drillbit abdiging",
							"cvccvc cvc cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "bi/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new int[] { 0, 2, 1 };
			m_sortOptions.AdvRlOptions = new bool[] { true, true, true };
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kDescending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(5, cache.Count);
			string[] words = { "drillbit", "rabbit", "bitter", "bir", "bit", }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests nested descending cvc and ascending MOA adv sorting with modified 
		/// sort order & Rl options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheAdvSortCvcDescMoaAscTest()
		{
			AddWords("rabbit bir fib bit dig ebay bitter digger drillbit abdiging",
							"cvccvc cvc cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "bi/*_*";
			WordListCache cache = PaApp.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new int[] { 0, 2, 1 };
			m_sortOptions.AdvRlOptions = new bool[] { true, true, true };
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, kAscending); // 2nd sort
			m_sortOptions.SetPrimarySortField(PaApp.FieldInfo.CVPatternField, false, kDescending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(5, cache.Count);
			string[] words = { "bitter", "rabbit", "bit", "bir", "drillbit", }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry[kPhonetic]);
		}
		#endregion
	}
}