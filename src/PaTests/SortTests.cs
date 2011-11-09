using System.Linq;
using NUnit.Framework;
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests phonetic sorting.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class SortTests : TestBase
	{
		private RecordCache m_recCache;
		private WordListCache m_cache;
		private PaDataSource m_dataSource;
		private SearchQuery m_query;
		private SortOptions m_sortOptions;
		private PaField m_phoneticField;
		private PaField m_cvField;
		private const bool kAscending = true;
		private const bool kDescending = false;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
			base.FixtureSetup();

			InventoryHelper.Load();

			m_dataSource = new PaDataSource();
			m_dataSource.Type = DataSourceType.Toolbox;

			m_phoneticField = m_prj.Fields.Single(f => f.Type == FieldType.Phonetic);
			m_cvField = m_prj.Fields.Single(f => f.Name == PaField.kCVPatternFieldName);
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_recCache = new RecordCache();
			m_cache = new WordListCache();
			m_query = new SearchQuery();
			m_sortOptions = new SortOptions();
		}

		/// ------------------------------------------------------------------------------------
		private void AddWords(string words, string pattern)
		{
			m_recCache.Clear();
			RecordCacheEntry entry = new RecordCacheEntry(m_prj);
			entry.SetValue("Phonetic", words);
			entry.SetValue("CVPattern", pattern);
			entry.NeedsParsing = true;
			m_dataSource.ParseType = DataSourceParseType.OneToOne;
			entry.DataSource = m_dataSource;
			m_recCache.Add(entry);
			m_recCache.BuildWordCache(null);

			m_cache.Clear();
			foreach (var wcEntry in m_recCache.WordCache)
				m_cache.Add(wcEntry);
		}

		#endregion

		#region Unicode Sort Tests
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests ascending unicode sorting.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CacheSortUnicodeAscTest()
		//{
		//    AddWords("fib bit ebay bitter drillbit abdiging",
		//        "cvc cvc vcvc cvccvc ccvcccvc vccvcvcc");
		//    m_sortOptions.SortType = PhoneticSortType.Unicode;
		//    m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
		//    m_cache.Sort(m_sortOptions);

		//    Assert.AreEqual(6, m_cache.Count);
		//    string[] words = { "abdiging", "bit", "bitter", "drillbit", "ebay", "fib" }; // expected answer
		//    for (int i = 0; i < m_cache.Count; i++)
		//        Assert.AreEqual(words[i], m_cache[i].WordCacheEntry["Phonetic"]);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests decending unicode sorting.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CacheSortUnicodeDescTest()
		//{
		//    AddWords("fib bit ebay bitter drillbit abdiging", null);
			
		//    m_sortOptions.SortType = PhoneticSortType.Unicode;
		//    m_sortOptions.SetPrimarySortField(m_phoneticField, false, kDescending);
		//    m_cache.Sort(m_sortOptions);

		//    Assert.AreEqual(6, m_cache.Count);
		//    string[] words = { "fib", "ebay", "drillbit", "bitter", "bit", "abdiging" }; // expected answer
		//    for (int i = 0; i < m_cache.Count; i++)
		//        Assert.AreEqual(words[i], m_cache[i].WordCacheEntry["Phonetic"]);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests nested sort with ascending cvc and descending unicode.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CacheSortCvcAscUnicodeDescTest()
		//{
		//    AddWords("fib bit dig ebay bitter digger drillbit abdiging",
		//        "cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
			
		//    m_query.Pattern = "i/*_*";
		//    WordListCache cache = App.Search(m_query);
		//    m_sortOptions.SortType = PhoneticSortType.Unicode;
		//    m_sortOptions.SetPrimarySortField(m_phoneticField, false, kDescending); // 2nd sort
		//    m_sortOptions.SetPrimarySortField(m_cvField, false, kAscending); // 1st sort
		//    cache.Sort(m_sortOptions);

		//    Assert.AreEqual(9, cache.Count);
		//    string[] words =
		//        { "drillbit", "drillbit", "fib", "dig", "bit", "digger", "bitter", "abdiging", "abdiging" }; // expected answer
		//    for (int i = 0; i < cache.Count; i++)
		//        Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests ascending unicode adv sorting with defaults.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CacheAdvDefaultsSortUnicodeAscTest()
		//{
		//    AddWords("fib bit dig ebay bitter digger drillbit abdiging",
		//                    "cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
		//    m_query.Pattern = "b/*_*";
		//    WordListCache cache = App.Search(m_query);
		//    m_sortOptions.SortType = PhoneticSortType.Unicode;
		//    m_sortOptions.AdvancedEnabled = true;
		//    m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
		//    cache.Sort(m_sortOptions);

		//    Assert.AreEqual(6, cache.Count);
		//    string[] words = { "bit", "bitter", "abdiging", "ebay", "fib", "drillbit" }; // expected answer
		//    for (int i = 0; i < cache.Count; i++)
		//        Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests ascending unicode adv sorting with modified sort order & Rl options.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CacheAdvSortUnicodeAscTest()
		//{
		//    AddWords("fib bit dig ebay bitter digger drillbit abdiging",
		//                    "cvc cvc cvc vcvc cvccvc cvccvc ccvcccvc vccvcvcc");
		//    m_query.Pattern = "b/*_*";
		//    WordListCache cache = App.Search(m_query);
		//    m_sortOptions.SortType = PhoneticSortType.Unicode;
		//    m_sortOptions.AdvancedEnabled = true;
		//    m_sortOptions.AdvSortOrder = new[] { 2, 1, 0 };
		//    m_sortOptions.AdvRlOptions = new[] { false, true, true };
		//    m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
		//    cache.Sort(m_sortOptions);

		//    Assert.AreEqual(6, cache.Count);
		//    string[] words = { "fib", "abdiging", "bitter", "bit", "drillbit", "ebay" }; // expected answer
		//    for (int i = 0; i < cache.Count; i++)
		//        Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
		//}
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
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "bit", "bitter", "fib", "drillbit", "ebay", "abdiging" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry["Phonetic"]);
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
			WordListCache cache = App.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kDescending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "abdiging", "ebay", "drillbit", "fib", "bitter", "bit" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
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
							"cvc cvc cvc vcvv cvccvc cvccvc ccvcccvc vccvcvcc");
			m_query.Pattern = "b/*_*";
			WordListCache cache = App.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kDescending); // 2nd sort
			m_sortOptions.SetPrimarySortField(m_cvField, false, kAscending); // 1st sort
			cache.Sort(m_sortOptions);

			System.Diagnostics.Debug.WriteLine("b = " + m_prj.PhoneCache["b"].POAKey);
			System.Diagnostics.Debug.WriteLine("f = " + m_prj.PhoneCache["f"].POAKey);
			System.Diagnostics.Debug.WriteLine("t = " + m_prj.PhoneCache["t"].POAKey);
			System.Diagnostics.Debug.WriteLine("d = " + m_prj.PhoneCache["d"].POAKey);
			System.Diagnostics.Debug.WriteLine("n = " + m_prj.PhoneCache["n"].POAKey);
			System.Diagnostics.Debug.WriteLine("r = " + m_prj.PhoneCache["r"].POAKey);
			System.Diagnostics.Debug.WriteLine("l = " + m_prj.PhoneCache["l"].POAKey);
			System.Diagnostics.Debug.WriteLine("g = " + m_prj.PhoneCache["g"].POAKey);
			System.Diagnostics.Debug.WriteLine("i = " + m_prj.PhoneCache["i"].POAKey);
			System.Diagnostics.Debug.WriteLine("e = " + m_prj.PhoneCache["e"].POAKey);
			System.Diagnostics.Debug.WriteLine("a = " + m_prj.PhoneCache["a"].POAKey);
			System.Diagnostics.Debug.WriteLine("y = " + m_prj.PhoneCache["y"].POAKey);

			Assert.AreEqual(6, cache.Count);
			Assert.AreEqual("drillbit", cache[0].WordCacheEntry["Phonetic"]);
			Assert.AreEqual("fib", cache[1].WordCacheEntry["Phonetic"]);
			Assert.AreEqual("bit", cache[2].WordCacheEntry["Phonetic"]);
			Assert.AreEqual("bitter", cache[3].WordCacheEntry["Phonetic"]);
			Assert.AreEqual("abdiging", cache[4].WordCacheEntry["Phonetic"]);
			Assert.AreEqual("ebay", cache[5].WordCacheEntry["Phonetic"]);
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
			WordListCache cache = App.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new[] { 2, 0, 1 };
			m_sortOptions.AdvRlOptions = new[] { true, false, true };
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kDescending); // 2nd sort
			m_sortOptions.SetPrimarySortField(m_cvField, false, kAscending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(10, cache.Count);
			// Expected answer
			string[] words = { "drillbit", "drillbit", "dig", "pig", "bit", "fib", "digger", "bitter", "abdiging", "abdiging" };
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
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
			WordListCache cache = App.Search(m_query);
			Assert.IsNotNull(cache);

			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new[] { 1, 0, 2 };
			m_sortOptions.AdvRlOptions = new[] { true, false, false };
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(3, cache.Count);
			string[] words = { "pig", "p\u02B0ig", "big" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
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
			var cache = new WordListCache();
			foreach (var entry in m_recCache.WordCache)
				cache.Add(entry);

			Assert.IsNotNull(cache);

			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = false;
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(2, cache.Count);
			Assert.AreEqual("bag", cache[0].WordCacheEntry["Phonetic"]);
			Assert.AreEqual("b\u0324it", cache[1].WordCacheEntry["Phonetic"]);
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
			WordListCache cache = App.Search(m_query);

			Assert.IsNotNull(cache);

			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new[] { 1, 0, 2 };
			m_sortOptions.AdvRlOptions = new[] { true, false, false };
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(4, cache.Count);
			Assert.AreEqual("sippu", cache[0].PhoneticValue);
			Assert.AreEqual(3, cache[0].SearchItemOffset);
			Assert.AreEqual("mappu", cache[1].PhoneticValue);
			Assert.AreEqual(3, cache[1].SearchItemOffset);
			Assert.AreEqual("sippu", cache[2].PhoneticValue);
			Assert.AreEqual(2, cache[2].SearchItemOffset);
			Assert.AreEqual("mappu", cache[3].PhoneticValue);
			Assert.AreEqual(2, cache[3].SearchItemOffset);
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
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "bit", "bitter", "drillbit", "fib", "ebay", "abdiging" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry["Phonetic"]);
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
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kDescending);
			m_cache.Sort(m_sortOptions);

			Assert.AreEqual(6, m_cache.Count);
			string[] words = { "abdiging", "ebay", "fib", "drillbit", "bitter", "bit" }; // expected answer
			for (int i = 0; i < m_cache.Count; i++)
				Assert.AreEqual(words[i], m_cache[i].WordCacheEntry["Phonetic"]);
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
			WordListCache cache = App.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending); // 2nd sort
			m_sortOptions.SetPrimarySortField(m_cvField, false, kDescending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "ebay", "abdiging", "bitter", "bit", "fib", "drillbit" }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
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
			WordListCache cache = App.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new[] { 0, 2, 1 };
			m_sortOptions.AdvRlOptions = new[] { true, true, true };
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kDescending);
			cache.Sort(m_sortOptions);

			Assert.AreEqual(5, cache.Count);
			string[] words = { "drillbit", "rabbit", "bitter", "bir", "bit", }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
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
			WordListCache cache = App.Search(m_query);
			m_sortOptions.SortType = PhoneticSortType.MOA;
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvSortOrder = new[] { 0, 2, 1 };
			m_sortOptions.AdvRlOptions = new[] { true, true, true };
			m_sortOptions.SetPrimarySortField(m_phoneticField, false, kAscending); // 2nd sort
			m_sortOptions.SetPrimarySortField(m_cvField, false, kDescending); // 1st sort
			cache.Sort(m_sortOptions);

			Assert.AreEqual(5, cache.Count);
			string[] words = { "bitter", "rabbit", "bit", "bir", "drillbit", }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
		}

		#endregion
	}
}