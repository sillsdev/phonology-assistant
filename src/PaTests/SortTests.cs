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
		private RecordCache _recCache;
		private WordListCache _cache;
		private PaDataSource _dataSource;
		private SearchQuery _query;
		private SortOptions _sortOptions;
		private PaField _phoneticField;
		private PaField _cvField;
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

			_dataSource = new PaDataSource();
			_dataSource.Type = DataSourceType.Toolbox;

			_phoneticField = _prj.Fields.Single(f => f.Type == FieldType.Phonetic);
			_cvField = _prj.Fields.Single(f => f.Name == PaField.kCVPatternFieldName);
			
			_dataSource.FieldMappings = new System.Collections.Generic.List<FieldMapping>();
			_dataSource.FieldMappings.Add(new FieldMapping("\\ph", _phoneticField, true));
			_dataSource.FieldMappings.Add(new FieldMapping("\\cv", _cvField, true));
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_prj.PhoneCache.Clear();
			_prj.RecordCache.Clear();
			_recCache = _prj.RecordCache;
			_cache = new WordListCache();
			_query = new SearchQuery();
			_sortOptions = new SortOptions(true, _prj);
		}

		/// ------------------------------------------------------------------------------------
		private void AddWords(string words, string pattern)
		{
			var entry = new RecordCacheEntry(_prj);
			entry.NeedsParsing = true;
			entry.SetValue("Phonetic", words);
			entry.SetValue("CVPattern", pattern);
			_dataSource.ParseType = DataSourceParseType.OneToOne;
			entry.DataSource = _dataSource;
			_recCache.Add(entry);
			_recCache.BuildWordCache(null);
			BuildPhoneSortKeysForTests();

			_cache.Clear();
			foreach (var wcEntry in _recCache.WordCache)
				_cache.Add(wcEntry);
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
			AddWords("fib bit ebay bitter drillbit abdiging", "cvc cvc vcvc cvccvc ccvcccvc vccvcvcc");
			_sortOptions.SortType = PhoneticSortType.POA;
			_sortOptions.SetPrimarySortField(_phoneticField, false, kAscending);
			_cache.Sort(_sortOptions);

			Assert.AreEqual(6, _cache.Count);
			
			string[] words = { "bit", "bitter", "drillbit", "fib", "ebay", "abdiging" }; // expected answer
			for (int i = 0; i < _cache.Count; i++)
				Assert.AreEqual(words[i], _cache[i].WordCacheEntry["Phonetic"]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests decending POA sorting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CacheSortPoaDescTest()
		{
			AddWords("fib bit ebay bitter drillbit abdiging", "cvc cvc vcvc cvccvc ccvcccvc vccvcvcc");
			_sortOptions.SortType = PhoneticSortType.POA;
			_sortOptions.SetPrimarySortField(_phoneticField, false, kDescending);
			_cache.Sort(_sortOptions);

			Assert.AreEqual(6, _cache.Count);

			string[] words = { "abdiging", "ebay", "fib", "drillbit", "bitter", "bit" }; // expected answer
			for (int i = 0; i < _cache.Count; i++)
				Assert.AreEqual(words[i], _cache[i].WordCacheEntry["Phonetic"]);
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
			_query.Pattern = "b/*_*";
			WordListCache cache = App.Search(_query);
			_sortOptions.SortType = PhoneticSortType.POA;
			_sortOptions.AdvancedEnabled = true;
			_sortOptions.SetPrimarySortField(_phoneticField, false, kDescending); // 2nd sort
			_sortOptions.SetPrimarySortField(_cvField, false, kAscending); // 1st sort
			cache.Sort(_sortOptions);

			System.Diagnostics.Debug.WriteLine("b = " + _prj.PhoneCache["b"].POAKey);
			System.Diagnostics.Debug.WriteLine("f = " + _prj.PhoneCache["f"].POAKey);
			System.Diagnostics.Debug.WriteLine("t = " + _prj.PhoneCache["t"].POAKey);
			System.Diagnostics.Debug.WriteLine("d = " + _prj.PhoneCache["d"].POAKey);
			System.Diagnostics.Debug.WriteLine("n = " + _prj.PhoneCache["n"].POAKey);
			System.Diagnostics.Debug.WriteLine("r = " + _prj.PhoneCache["r"].POAKey);
			System.Diagnostics.Debug.WriteLine("l = " + _prj.PhoneCache["l"].POAKey);
			System.Diagnostics.Debug.WriteLine("g = " + _prj.PhoneCache["g"].POAKey);
			System.Diagnostics.Debug.WriteLine("i = " + _prj.PhoneCache["i"].POAKey);
			System.Diagnostics.Debug.WriteLine("e = " + _prj.PhoneCache["e"].POAKey);
			System.Diagnostics.Debug.WriteLine("a = " + _prj.PhoneCache["a"].POAKey);
			System.Diagnostics.Debug.WriteLine("y = " + _prj.PhoneCache["y"].POAKey);

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
			_query.Pattern = "i/*_*";
			WordListCache cache = App.Search(_query);
			_sortOptions.SortType = PhoneticSortType.POA;
			_sortOptions.AdvancedEnabled = true;
			_sortOptions.AdvSortOrder = new[] { 2, 0, 1 };
			_sortOptions.AdvRlOptions = new[] { true, false, true };
			_sortOptions.SetPrimarySortField(_phoneticField, false, kDescending); // 2nd sort
			_sortOptions.SetPrimarySortField(_cvField, false, kAscending); // 1st sort
			cache.Sort(_sortOptions);

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
			_query.Pattern = "[V]/*_*";
			WordListCache cache = App.Search(_query);
			Assert.IsNotNull(cache);

			_sortOptions.SortType = PhoneticSortType.POA;
			_sortOptions.AdvancedEnabled = true;
			_sortOptions.AdvSortOrder = new[] { 1, 0, 2 };
			_sortOptions.AdvRlOptions = new[] { true, false, false };
			_sortOptions.SetPrimarySortField(_phoneticField, false, kAscending);
			cache.Sort(_sortOptions);

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
			foreach (var entry in _recCache.WordCache)
				cache.Add(entry);

			Assert.IsNotNull(cache);

			_sortOptions.SortType = PhoneticSortType.POA;
			_sortOptions.AdvancedEnabled = false;
			_sortOptions.SetPrimarySortField(_phoneticField, false, kAscending);
			cache.Sort(_sortOptions);

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
			_query.Pattern = "p/*_*";
			WordListCache cache = App.Search(_query);

			Assert.IsNotNull(cache);

			_sortOptions.SortType = PhoneticSortType.POA;
			_sortOptions.AdvancedEnabled = true;
			_sortOptions.AdvSortOrder = new[] { 1, 0, 2 };
			_sortOptions.AdvRlOptions = new[] { true, false, false };
			_sortOptions.SetPrimarySortField(_phoneticField, false, kAscending);
			cache.Sort(_sortOptions);

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
			_sortOptions.SortType = PhoneticSortType.MOA;
			_sortOptions.SetPrimarySortField(_phoneticField, false, kAscending);
			_cache.Sort(_sortOptions);

			Assert.AreEqual(6, _cache.Count);
			string[] words = { "abdiging", "ebay", "fib", "drillbit", "bit", "bitter" }; // expected answer
			for (int i = 0; i < _cache.Count; i++)
				Assert.AreEqual(words[i], _cache[i].WordCacheEntry["Phonetic"]);
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
			_sortOptions.SortType = PhoneticSortType.MOA;
			_sortOptions.SetPrimarySortField(_phoneticField, false, kDescending);
			_cache.Sort(_sortOptions);

			Assert.AreEqual(6, _cache.Count);
			string[] words = { "bitter", "bit", "drillbit", "fib", "ebay", "abdiging" }; // expected answer
			for (int i = 0; i < _cache.Count; i++)
				Assert.AreEqual(words[i], _cache[i].WordCacheEntry["Phonetic"]);
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
			_query.Pattern = "b/*_*";
			WordListCache cache = App.Search(_query);
			_sortOptions.SortType = PhoneticSortType.MOA;
			_sortOptions.SetPrimarySortField(_phoneticField, false, kAscending); // 2nd sort
			_sortOptions.SetPrimarySortField(_cvField, false, kDescending); // 1st sort
			cache.Sort(_sortOptions);

			Assert.AreEqual(6, cache.Count);
			string[] words = { "ebay", "abdiging", "bitter", "fib", "bit", "drillbit" }; // expected answer
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
			_query.Pattern = "bi/*_*";
			WordListCache cache = App.Search(_query);
			_sortOptions.SortType = PhoneticSortType.MOA;
			_sortOptions.AdvancedEnabled = true;
			_sortOptions.AdvSortOrder = new[] { 0, 2, 1 };
			_sortOptions.AdvRlOptions = new[] { true, true, true };
			_sortOptions.SetPrimarySortField(_phoneticField, false, kDescending);
			cache.Sort(_sortOptions);

			Assert.AreEqual(5, cache.Count);
			string[] words = { "rabbit", "drillbit", "bit", "bitter", "bir" }; // expected answer
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
			_query.Pattern = "bi/*_*";
			WordListCache cache = App.Search(_query);
			_sortOptions.SortType = PhoneticSortType.MOA;
			_sortOptions.AdvancedEnabled = true;
			_sortOptions.AdvSortOrder = new[] { 0, 2, 1 };
			_sortOptions.AdvRlOptions = new[] { true, true, true };
			_sortOptions.SetPrimarySortField(_phoneticField, false, kAscending); // 2nd sort
			_sortOptions.SetPrimarySortField(_cvField, false, kDescending); // 1st sort
			cache.Sort(_sortOptions);

			Assert.AreEqual(5, cache.Count);
			string[] words = { "bitter", "rabbit", "bir", "bit", "drillbit", }; // expected answer
			for (int i = 0; i < cache.Count; i++)
				Assert.AreEqual(words[i], cache[i].WordCacheEntry["Phonetic"]);
		}

		#endregion
	}
}