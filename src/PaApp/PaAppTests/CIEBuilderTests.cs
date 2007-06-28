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
// File: MiscTests.cs
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
    /// Tests Misc. methods in DataUtils that reqire a database.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class CIEBuilderTests : TestBase
	{
		private RecordCache m_recCache;
		private WordListCache m_cache;
		private PaDataSource m_dataSource;
		private SortOptions m_sortOptions;

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

			m_sortOptions = new SortOptions();
			m_sortOptions.AdvancedEnabled = true;
			m_sortOptions.AdvRlOptions = new bool[] { false, false, false };
			m_sortOptions.SetPrimarySortField(PaApp.Project.FieldInfo.PhoneticField, false, true);
			m_sortOptions.SortType = PhoneticSortType.POA;
			m_sortOptions.AdvSortOrder = new int[] { 0, 2, 1 };
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

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddWords(string words)
		{
			RecordCacheEntry entry = new RecordCacheEntry(true);
			entry.SetValue("Phonetic", words);
			entry.NeedsParsing = true;
			entry.DataSource = m_dataSource;
			m_recCache.Add(entry);
			m_recCache.BuildWordCache(null);
			PaApp.BuildPhoneCache();

			SearchQuery query = new SearchQuery();
			query.IgnoreDiacritics = false;
			query.IgnoredLengthChars = null;
			query.IgnoredStressChars = null;
			query.IgnoredToneChars = null;
			query.Pattern = "[V]/*_*";
			m_cache = PaApp.Search(query);
			m_cache.Sort(m_sortOptions);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Both;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredLengthChars = null;
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(6, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);
			Assert.AreNotEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreEqual(retCache[3].CIEGroupId, retCache[4].CIEGroupId);
			Assert.AreEqual(retCache[4].CIEGroupId, retCache[5].CIEGroupId);

			Assert.AreEqual("bit", retCache[0].PhoneticValue);
			Assert.AreEqual("bat", retCache[1].PhoneticValue);
			Assert.AreEqual("bot", retCache[2].PhoneticValue);
			Assert.AreEqual("dig", retCache[3].PhoneticValue);
			Assert.AreEqual("dag", retCache[4].PhoneticValue);
			Assert.AreEqual("dog", retCache[5].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredLengthChars = null;
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(6, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);
			Assert.AreNotEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreEqual(retCache[3].CIEGroupId, retCache[4].CIEGroupId);
			Assert.AreEqual(retCache[4].CIEGroupId, retCache[5].CIEGroupId);

			Assert.AreEqual("bim", retCache[0].PhoneticValue);
			Assert.AreEqual("bab", retCache[1].PhoneticValue);
			Assert.AreEqual("bot", retCache[2].PhoneticValue);
			Assert.AreEqual("dig", retCache[3].PhoneticValue);
			Assert.AreEqual("daw", retCache[4].PhoneticValue);
			Assert.AreEqual("dok", retCache[5].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.After;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredLengthChars = null;
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(6, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);
			Assert.AreNotEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreEqual(retCache[3].CIEGroupId, retCache[4].CIEGroupId);
			Assert.AreEqual(retCache[4].CIEGroupId, retCache[5].CIEGroupId);

			Assert.AreEqual("bab", retCache[0].PhoneticValue);
			Assert.AreEqual("mib", retCache[1].PhoneticValue);
			Assert.AreEqual("tob", retCache[2].PhoneticValue);
			Assert.AreEqual("kod", retCache[3].PhoneticValue);
			Assert.AreEqual("gid", retCache[4].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Both;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredLengthChars = "\u0324\u02B0";
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("bat", retCache[0].PhoneticValue);
			Assert.AreEqual("b\u0324it", retCache[1].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredLengthChars = "\u0324\u02B0";
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("bag", retCache[0].PhoneticValue);
			Assert.AreEqual("b\u0324it", retCache[1].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.After;
			options.SearchQuery.IgnoreDiacritics = false;
			options.SearchQuery.IgnoredLengthChars = "\u0324\u02B0";
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("tib\u0324", retCache[0].PhoneticValue);
			Assert.AreEqual("gab", retCache[1].PhoneticValue);
			Assert.AreEqual("nod", retCache[2].PhoneticValue);
			Assert.AreEqual("g\u02B0id", retCache[3].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Both;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredLengthChars = null;
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("bat", retCache[0].PhoneticValue);
			Assert.AreEqual("b\u0324it", retCache[1].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredLengthChars = null;
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.After;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredLengthChars = null;
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(4, retCache.Count);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[2].CIEGroupId, retCache[3].CIEGroupId);
			Assert.AreNotEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);

			Assert.AreEqual("nod", retCache[0].PhoneticValue);
			Assert.AreEqual("g\u02B0id", retCache[1].PhoneticValue);
			Assert.AreEqual("tib\u0324", retCache[2].PhoneticValue);
			Assert.AreEqual("gab", retCache[3].PhoneticValue);
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

			CIEOptions options = new CIEOptions();
			options.Type = CIEOptions.IdenticalType.Before;
			options.SearchQuery.IgnoreDiacritics = true;
			options.SearchQuery.IgnoredLengthChars = null;
			options.SearchQuery.IgnoredStressChars = null;
			options.SearchQuery.IgnoredToneChars = null;
			CIEBuilder builder = new CIEBuilder(m_cache, options);
			WordListCache retCache = builder.FindMinimalPairs();

			Assert.AreEqual(3, retCache.Count);

			Assert.AreEqual("pat", retCache[0].PhoneticValue);
			Assert.AreEqual("p\u02B0it", retCache[1].PhoneticValue);
			Assert.AreEqual("p\u02B0ot", retCache[2].PhoneticValue);

			Assert.AreEqual(retCache[0].CIEGroupId, retCache[1].CIEGroupId);
			Assert.AreEqual(retCache[1].CIEGroupId, retCache[2].CIEGroupId);
		}
	}
}