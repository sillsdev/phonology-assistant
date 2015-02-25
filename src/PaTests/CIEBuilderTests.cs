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
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Processing;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DataUtils that reqire a database.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class CIEBuilderTests : TestBase
	{
		private RecordCache _recCache;
		private WordListCache _cache;
		private PaDataSource _dataSource;

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
			_dataSource.FieldMappings = new System.Collections.Generic.List<FieldMapping>();
			_dataSource.FieldMappings.Add(new FieldMapping("\\ph", _prj.GetPhoneticField(), true));
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
        public void TestSetup()
        {
			_prj.PhoneCache.Clear();
			_prj.RecordCache.Clear();
			_recCache = _prj.RecordCache;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void AddWords(string words)
		{
			var entry = new RecordCacheEntry(_prj);
			entry.SetValue("Phonetic", words);
			entry.NeedsParsing = true;
			entry.DataSource = _dataSource;
			_recCache.Add(entry);
			_recCache.BuildWordCache(null);
			BuildPhoneSortKeysForTests();

			var query = new SearchQuery();
			query.IgnoreDiacritics = false;
			query.IgnoredCharacters = null;
			query.Pattern = "[V]/*_*";
			_cache = App.Search(query);
			_cache.SearchQuery = query;
		}

		/// ------------------------------------------------------------------------------------
		private WordListCache GetCIEResults(CIEOptions options)
		{
			var sortOptions = new SortOptions(true, _prj) { AdvancedEnabled = true };
			
			if (options.Type != CIEOptions.IdenticalType.After)
				sortOptions.AdvSortOrder = new[] { 0, 1, 2 };
			else
			{
				sortOptions.AdvSortOrder = new[] { 2, 1, 0 };
				sortOptions.AdvRlOptions[2] = true;
			}
			
			_cache.Sort(sortOptions);

			sortOptions = new SortOptions(true, _prj) { AdvancedEnabled = true };
			return new CIEBuilder(_cache, sortOptions, options).FindMinimalPairs();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs using simple options
		/// (i.e. nothing ignored) where both environments must match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SimpleMatchOnBothEnvironmentsTest()
		{
			AddWords("pim bit dig bat dag bot dog mop");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Both;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredCharacters = null;
			var retCache = GetCIEResults(options);

			Assert.AreEqual(6, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);
			Assert.AreNotEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreEqual(retCache[3].CIEGroupId, retCache[4].CIEGroupId);
			Assert.AreEqual(retCache[4].CIEGroupId, retCache[5].CIEGroupId);

			Assert.AreEqual("bit", retCache[0].PhoneticValue);
			Assert.AreEqual("bot", retCache[1].PhoneticValue);
			Assert.AreEqual("bat", retCache[2].PhoneticValue);
			Assert.AreEqual("dig", retCache[3].PhoneticValue);
			Assert.AreEqual("dog", retCache[4].PhoneticValue);
			Assert.AreEqual("dag", retCache[5].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs using simple options
		/// (i.e. nothing ignored) where the before environments must match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SimpleMatchOnEnvironmentBeforeTest()
		{
			AddWords("pnm bim dig bab daw bot dok mnp pig");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredCharacters = null;
			var retCache = GetCIEResults(options);

			Assert.AreEqual(6, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);
			Assert.AreNotEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreEqual(retCache[3].CIEGroupId, retCache[4].CIEGroupId);
			Assert.AreEqual(retCache[4].CIEGroupId, retCache[5].CIEGroupId);

			Assert.AreEqual("bim", retCache[0].PhoneticValue);
			Assert.AreEqual("bot", retCache[1].PhoneticValue);
			Assert.AreEqual("bab", retCache[2].PhoneticValue);
			Assert.AreEqual("dig", retCache[3].PhoneticValue);
			Assert.AreEqual("dok", retCache[4].PhoneticValue);
			Assert.AreEqual("daw", retCache[5].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs using simple options
		/// (i.e. nothing ignored) where the after environments must match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SimpleMatchOnEnvironmentAfterTest()
		{
			AddWords("pnm mib gid bab wad tob kod mnp pig");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.After;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredCharacters = null;
			var retCache = GetCIEResults(options);

			Assert.AreEqual(6, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);
			Assert.AreNotEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreEqual(retCache[3].CIEGroupId, retCache[4].CIEGroupId);
			Assert.AreEqual(retCache[4].CIEGroupId, retCache[5].CIEGroupId);

			Assert.AreEqual("mib", retCache[0].PhoneticValue);
			Assert.AreEqual("tob", retCache[1].PhoneticValue);
			Assert.AreEqual("bab", retCache[2].PhoneticValue);
			Assert.AreEqual("gid", retCache[3].PhoneticValue);
			Assert.AreEqual("kod", retCache[4].PhoneticValue);
			Assert.AreEqual("wad", retCache[5].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs where both environments must match and
		/// when there are ignore options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchOnBothEnvironmentsWithIngoreItemsTest()
		{
			AddWords("b\u0324it bat dig\u02B0 dog");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Both;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredCharacters = "\u0324,\u02B0";
			var retCache = GetCIEResults(options);

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("b\u0324it",retCache[0].PhoneticValue);
			Assert.AreEqual("bat", retCache[1].PhoneticValue);
			Assert.AreEqual("dig\u02B0", retCache[2].PhoneticValue);
			Assert.AreEqual("dog", retCache[3].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs where the before environments must 
		/// match and when there are ignore options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchOnBeforeEnvironmentsWithIngoreItemsTest()
		{
			AddWords("b\u0324it bag dig\u02B0 don");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredCharacters = "\u0324,\u02B0";
			var retCache = GetCIEResults(options);

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("b\u0324it", retCache[0].PhoneticValue);
			Assert.AreEqual("bag", retCache[1].PhoneticValue);
			Assert.AreEqual("dig\u02B0", retCache[2].PhoneticValue);
			Assert.AreEqual("don", retCache[3].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs where the after environments must 
		/// match and when there are ignore options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchOnAfterEnvironmentsWithIngoreItemsTest()
		{
			AddWords("tib\u0324 gab g\u02B0id nod");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.After;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredCharacters = "\u0324,\u02B0";
			var retCache = GetCIEResults(options);

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("tib\u0324", retCache[0].PhoneticValue);
			Assert.AreEqual("gab", retCache[1].PhoneticValue);
			Assert.AreEqual("g\u02B0id", retCache[2].PhoneticValue);
			Assert.AreEqual("nod", retCache[3].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs where both environments must match and
		/// when diacritics are ignored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchOnBothEnvironmentsWhenIgnoreingDiacriticsTest()
		{
			AddWords("b\u0324it bat dig\u02B0 dog");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Both;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredCharacters = null;
			var retCache = GetCIEResults(options);

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("b\u0324it", retCache[0].PhoneticValue);
			Assert.AreEqual("bat", retCache[1].PhoneticValue);
			Assert.AreEqual("dig\u02B0", retCache[2].PhoneticValue);
			Assert.AreEqual("dog", retCache[3].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs where the before environments must 
		/// match and when diacritics are ignored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchOnEnvironmentBeforeIgnoreingDiacriticsTest()
		{
			AddWords("b\u0324it bag dig\u02B0 don");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredCharacters = null;
			var retCache = GetCIEResults(options);

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("b\u0324it", retCache[0].PhoneticValue);
			Assert.AreEqual("bag", retCache[1].PhoneticValue);
			Assert.AreEqual("dig\u02B0", retCache[2].PhoneticValue);
			Assert.AreEqual("don", retCache[3].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs where the after environments must 
		/// match and when diacritics are ignored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchOnEnvironmentAfterIgnoreingDiacriticsTest()
		{
			AddWords("tib\u0324 gab g\u02B0id nod");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.After;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredCharacters = null;
			var retCache = GetCIEResults(options);

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("tib\u0324", retCache[0].PhoneticValue);
			Assert.AreEqual("gab", retCache[1].PhoneticValue);
			Assert.AreEqual("g\u02B0id", retCache[2].PhoneticValue);
			Assert.AreEqual("nod", retCache[3].PhoneticValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the builder finds minimal pairs where the after environments must 
		/// match and when diacritics are ignored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MatchOnEnvironmentBeforeWithIgnoredDiacriticsTest2()
		{
			AddWords("p\u02B0it pbit pat p\u02B0ot");

			var options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredCharacters = null;
			var retCache = GetCIEResults(options);

			Assert.AreEqual(3, retCache.Count);
			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("p\u02B0it", retCache[0].PhoneticValue);
			Assert.AreEqual("p\u02B0ot", retCache[1].PhoneticValue);
			Assert.AreEqual("pat", retCache[2].PhoneticValue);
		}
	}
}