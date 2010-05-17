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
using NUnit.Framework;
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;


namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DataUtils.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class MiscTests : TestBase
	{
		RecordCacheEntry m_recEntry;
		WordCacheEntry m_entry;

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
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
        public void TestSetup()
        {
			App.IPASymbolCache.UndefinedCharacters = null;
			App.WordCache = new WordCache();
			m_recEntry = new RecordCacheEntry(true);
			m_recEntry.DataSource = new PaDataSource();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
        public void TestTearDown()
        {
			App.FieldInfo = null;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Simple phone cache building test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheTest1()
		{
			var wordCache = new WordCache();
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "abc";
			wordCache.Add(m_entry);
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "xyz";
			wordCache.Add(m_entry);
			
			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(6, phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["c"]);
			Assert.IsNotNull(phoneCache["x"]);
			Assert.IsNotNull(phoneCache["y"]);
			Assert.IsNotNull(phoneCache["z"]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache repeated phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheTest2()
		{
			var wordCache = new WordCache();
			
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "abc";
			wordCache.Add(m_entry);
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "axc";
			wordCache.Add(m_entry);

			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(4, phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["c"]);
			Assert.IsNotNull(phoneCache["x"]);

			Assert.AreEqual(2, phoneCache["a"].TotalCount);
			Assert.AreEqual(1, phoneCache["b"].TotalCount);
			Assert.AreEqual(2, phoneCache["c"].TotalCount);
			Assert.AreEqual(1, phoneCache["x"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache when phonetic words contain word breaks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithWordBreaksInPhonetic()
		{
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "ab" + App.BreakChars[0] + "xy";
			var wordCache = new WordCache();
			wordCache.Add(m_entry);

			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(4,phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["x"]);
			Assert.IsNotNull(phoneCache["y"]);
			Assert.IsNull(phoneCache[App.BreakChars[0].ToString()]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache when phonetic words contain word breaks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUndefinedChars()
		{
			var wordCache = new WordCache();

			App.IPASymbolCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			App.IPASymbolCache.LogUndefinedCharactersWhenParsing = true;

			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "abXY";
			wordCache.Add(m_entry);
			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(2, App.IPASymbolCache.UndefinedCharacters.Count);
			Assert.AreEqual('X', App.IPASymbolCache.UndefinedCharacters[0].Character);
			Assert.AreEqual('Y', App.IPASymbolCache.UndefinedCharacters[1].Character);
			Assert.IsTrue(App.IPASymbolCache["X"].IsUndefined);
			Assert.IsTrue(App.IPASymbolCache["Y"].IsUndefined);

			Assert.AreEqual(4, phoneCache.Count);
			Assert.IsFalse((phoneCache["a"] as PhoneInfo).IsUndefined);
			Assert.IsFalse((phoneCache["b"] as PhoneInfo).IsUndefined);
			Assert.IsTrue((phoneCache["X"] as PhoneInfo).IsUndefined);
			Assert.IsTrue((phoneCache["Y"] as PhoneInfo).IsUndefined);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Simple test for building phone cache with uncertain phone group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertainties1()
		{
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "ab(t/d)c";
			var wordCache = new WordCache();
			wordCache.Add(m_entry);
			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(5, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["d"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].TotalCount);
			Assert.AreEqual(0, phoneCache["d"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Simple test for building phone cache with uncertain phone group, testing that each
		/// uncertainty's sibling(s) is/are correct.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertainties2()
		{
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "ab(t/d)c(e/i/o)";
			var wordCache = new WordCache();
			wordCache.Add(m_entry);
			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(8, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["e"]);
			Assert.IsNotNull(phoneCache["i"]);
			Assert.IsNotNull(phoneCache["o"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["e"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["o"].CountAsNonPrimaryUncertainty);

			Assert.AreEqual(1, phoneCache["t"].SiblingUncertainties.Count);
			Assert.AreEqual(1, phoneCache["d"].SiblingUncertainties.Count);
			Assert.AreEqual(2, phoneCache["e"].SiblingUncertainties.Count);
			Assert.AreEqual(2, phoneCache["i"].SiblingUncertainties.Count);
			Assert.AreEqual(2, phoneCache["o"].SiblingUncertainties.Count);

			Assert.AreEqual("d", phoneCache["t"].SiblingUncertainties[0]);
			Assert.AreEqual("t", phoneCache["d"].SiblingUncertainties[0]);

			Assert.AreEqual("i", phoneCache["e"].SiblingUncertainties[0]);
			Assert.AreEqual("o", phoneCache["e"].SiblingUncertainties[1]);
			Assert.AreEqual("e", phoneCache["i"].SiblingUncertainties[0]);
			Assert.AreEqual("o", phoneCache["i"].SiblingUncertainties[1]);
			Assert.AreEqual("e", phoneCache["o"].SiblingUncertainties[0]);
			Assert.AreEqual("i", phoneCache["o"].SiblingUncertainties[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with uncertain phone group, when there the
		/// uncertainties indicate the absence or presence of a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithEmptySetInUncertaintyGroup1()
		{
			VerifyUncertaintyGroup("ab(t/)c");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with uncertain phone group, when there the
		/// uncertainties indicate the absence or presence of a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithEmptySetInUncertaintyGroup2()
		{
			VerifyUncertaintyGroup("ab(t/0)c");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with uncertain phone group, when there the
		/// uncertainties indicate the absence or presence of a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithEmptySetInUncertaintyGroup3()
		{
			VerifyUncertaintyGroup(string.Format("ab(t/{0})c", IPASymbolCache.UncertainGroupAbsentPhoneChars));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void VerifyUncertaintyGroup(string phontic)
		{
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = phontic;
			var wordCache = new WordCache();
			wordCache.Add(m_entry);
			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(4, phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["c"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].CountAsNonPrimaryUncertainty);

			Assert.AreEqual(1, phoneCache["t"].SiblingUncertainties.Count);
			Assert.AreEqual(IPASymbolCache.UncertainGroupAbsentPhoneChar,
				phoneCache["t"].SiblingUncertainties[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with 2 uncertain phone groups in a word. (This test
		/// will test that PA-710 is fixed).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertaintiesWithMultiGroups1()
		{
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "ab(t/d)cxy(u/i)z";
			var wordCache = new WordCache();
			wordCache.Add(m_entry);
			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(10, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["u"]);
			Assert.IsNotNull(phoneCache["i"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].TotalCount);
			Assert.AreEqual(0, phoneCache["d"].TotalCount);
			Assert.AreEqual(1, phoneCache["u"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["u"].TotalCount);
			Assert.AreEqual(0, phoneCache["i"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with 3 uncertain phone groups in a word. (This test
		/// will test that PA-710 is fixed).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertaintiesWithMultiGroups2()
		{
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "ab(t/d)cxy(u/i)z(t/d)mn";
			var wordCache = new WordCache();
			wordCache.Add(m_entry);
			var phoneCache = GetResult(typeof(RecordCache), "GetPhonesFromWordCache", wordCache) as PhoneCache;

			Assert.AreEqual(12, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["u"]);
			Assert.IsNotNull(phoneCache["i"]);

			Assert.AreEqual(2, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(2, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].TotalCount);
			Assert.AreEqual(0, phoneCache["d"].TotalCount);
			Assert.AreEqual(1, phoneCache["u"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["u"].TotalCount);
			Assert.AreEqual(0, phoneCache["i"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetInterlinearColumnWidths method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DataSourceReader_ParseSoundFileName()
		{
			var reader = new DataSourceReader(new PaProject());

			string filename = @"c:\junk1\junk2\junk3.wav";

			object[] args = new object[] { filename, (long)-1, (long)-1 };
			string result = GetStrResult(typeof(DataSourceReader), "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(0, (long)args[1]);
			Assert.AreEqual(0, (long)args[2]);

			args = new object[] { filename + " 2.123", (long)-1, (long)-1 };
			result = GetStrResult(typeof(DataSourceReader), "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(0, (long)args[2]);

			args = new object[] { filename + " 2.123 4.43265", (long)-1, (long)-1 };
			result = GetStrResult(typeof(DataSourceReader), "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(4433, (long)args[2]);

			filename = @"c:\junk1\junk2\this is junk3.mp3";
			args = new object[] { filename + " 2.123 4.43265", (long)-1, (long)-1 };
			result = GetStrResult(typeof(DataSourceReader), "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(4433, (long)args[2]);
		}
	}
}