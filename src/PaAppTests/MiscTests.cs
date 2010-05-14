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
using SIL.Pa.Data;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;

namespace SIL.Pa
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
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
        public void TestSetup()
        {
			DataUtils.IPACharCache.UndefinedCharacters = null;
			PaApp.WordCache = new WordCache();
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
			PaApp.FieldInfo = null;
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
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "abc";
			PaApp.WordCache.Add(m_entry);
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "xyz";
			PaApp.WordCache.Add(m_entry);

			PaApp.BuildPhoneCache();

			Assert.AreEqual(6, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["a"]);
			Assert.IsNotNull(PaApp.PhoneCache["b"]);
			Assert.IsNotNull(PaApp.PhoneCache["c"]);
			Assert.IsNotNull(PaApp.PhoneCache["x"]);
			Assert.IsNotNull(PaApp.PhoneCache["y"]);
			Assert.IsNotNull(PaApp.PhoneCache["z"]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache repeated phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheTest2()
		{
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "abc";
			PaApp.WordCache.Add(m_entry);
			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "axc";
			PaApp.WordCache.Add(m_entry);

			PaApp.BuildPhoneCache();

			Assert.AreEqual(4, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["a"]);
			Assert.IsNotNull(PaApp.PhoneCache["b"]);
			Assert.IsNotNull(PaApp.PhoneCache["c"]);
			Assert.IsNotNull(PaApp.PhoneCache["x"]);

			Assert.AreEqual(2, PaApp.PhoneCache["a"].TotalCount);
			Assert.AreEqual(1, PaApp.PhoneCache["b"].TotalCount);
			Assert.AreEqual(2, PaApp.PhoneCache["c"].TotalCount);
			Assert.AreEqual(1, PaApp.PhoneCache["x"].TotalCount);
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
			m_entry["Phonetic"] = "ab" + PaApp.BreakChars[0].ToString() + "xy";
			PaApp.WordCache.Add(m_entry);

			PaApp.BuildPhoneCache();

			Assert.AreEqual(4, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["a"]);
			Assert.IsNotNull(PaApp.PhoneCache["b"]);
			Assert.IsNotNull(PaApp.PhoneCache["x"]);
			Assert.IsNotNull(PaApp.PhoneCache["y"]);
			Assert.IsNull(PaApp.PhoneCache[PaApp.BreakChars[0].ToString()]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache when phonetic words contain word breaks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUndefinedChars()
		{
			DataUtils.IPACharCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			DataUtils.IPACharCache.LogUndefinedCharactersWhenParsing = true;

			m_entry = new WordCacheEntry(m_recEntry, true);
			m_entry["Phonetic"] = "abXY";
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(2, DataUtils.IPACharCache.UndefinedCharacters.Count);
			Assert.AreEqual('X', DataUtils.IPACharCache.UndefinedCharacters[0].Character);
			Assert.AreEqual('Y', DataUtils.IPACharCache.UndefinedCharacters[1].Character);
			Assert.IsTrue((DataUtils.IPACharCache["X"] as IPACharInfo).IsUndefined);
			Assert.IsTrue((DataUtils.IPACharCache["Y"] as IPACharInfo).IsUndefined);

			Assert.AreEqual(4, PaApp.PhoneCache.Count);
			Assert.IsFalse((PaApp.PhoneCache["a"] as PhoneInfo).IsUndefined);
			Assert.IsFalse((PaApp.PhoneCache["b"] as PhoneInfo).IsUndefined);
			Assert.IsTrue((PaApp.PhoneCache["X"] as PhoneInfo).IsUndefined);
			Assert.IsTrue((PaApp.PhoneCache["Y"] as PhoneInfo).IsUndefined);
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
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(5, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
			Assert.IsNotNull(PaApp.PhoneCache["d"]);

			Assert.AreEqual(1, PaApp.PhoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, PaApp.PhoneCache["t"].TotalCount);
			Assert.AreEqual(0, PaApp.PhoneCache["d"].TotalCount);
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
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(8, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
			Assert.IsNotNull(PaApp.PhoneCache["e"]);
			Assert.IsNotNull(PaApp.PhoneCache["i"]);
			Assert.IsNotNull(PaApp.PhoneCache["o"]);

			Assert.AreEqual(1, PaApp.PhoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["e"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["o"].CountAsNonPrimaryUncertainty);

			Assert.AreEqual(1, PaApp.PhoneCache["t"].SiblingUncertainties.Count);
			Assert.AreEqual(1, PaApp.PhoneCache["d"].SiblingUncertainties.Count);
			Assert.AreEqual(2, PaApp.PhoneCache["e"].SiblingUncertainties.Count);
			Assert.AreEqual(2, PaApp.PhoneCache["i"].SiblingUncertainties.Count);
			Assert.AreEqual(2, PaApp.PhoneCache["o"].SiblingUncertainties.Count);

			Assert.AreEqual("d", PaApp.PhoneCache["t"].SiblingUncertainties[0]);
			Assert.AreEqual("t", PaApp.PhoneCache["d"].SiblingUncertainties[0]);

			Assert.AreEqual("i", PaApp.PhoneCache["e"].SiblingUncertainties[0]);
			Assert.AreEqual("o", PaApp.PhoneCache["e"].SiblingUncertainties[1]);
			Assert.AreEqual("e", PaApp.PhoneCache["i"].SiblingUncertainties[0]);
			Assert.AreEqual("o", PaApp.PhoneCache["i"].SiblingUncertainties[1]);
			Assert.AreEqual("e", PaApp.PhoneCache["o"].SiblingUncertainties[0]);
			Assert.AreEqual("i", PaApp.PhoneCache["o"].SiblingUncertainties[1]);
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
			VerifyUncertaintyGroup(string.Format("ab(t/{0})c", IPACharCache.UncertainGroupAbsentPhoneChars));
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
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(4, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["a"]);
			Assert.IsNotNull(PaApp.PhoneCache["b"]);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
			Assert.IsNotNull(PaApp.PhoneCache["c"]);

			Assert.AreEqual(1, PaApp.PhoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(0, PaApp.PhoneCache["t"].CountAsNonPrimaryUncertainty);

			Assert.AreEqual(1, PaApp.PhoneCache["t"].SiblingUncertainties.Count);
			Assert.AreEqual(IPACharCache.UncertainGroupAbsentPhoneChar,
				PaApp.PhoneCache["t"].SiblingUncertainties[0]);
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
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(10, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
			Assert.IsNotNull(PaApp.PhoneCache["u"]);
			Assert.IsNotNull(PaApp.PhoneCache["i"]);

			Assert.AreEqual(1, PaApp.PhoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, PaApp.PhoneCache["t"].TotalCount);
			Assert.AreEqual(0, PaApp.PhoneCache["d"].TotalCount);
			Assert.AreEqual(1, PaApp.PhoneCache["u"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, PaApp.PhoneCache["u"].TotalCount);
			Assert.AreEqual(0, PaApp.PhoneCache["i"].TotalCount);
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
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(12, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
			Assert.IsNotNull(PaApp.PhoneCache["u"]);
			Assert.IsNotNull(PaApp.PhoneCache["i"]);

			Assert.AreEqual(2, PaApp.PhoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(2, PaApp.PhoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, PaApp.PhoneCache["t"].TotalCount);
			Assert.AreEqual(0, PaApp.PhoneCache["d"].TotalCount);
			Assert.AreEqual(1, PaApp.PhoneCache["u"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, PaApp.PhoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, PaApp.PhoneCache["u"].TotalCount);
			Assert.AreEqual(0, PaApp.PhoneCache["i"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetInterlinearColumnWidths method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DataSourceReader_ParseSoundFileName()
		{
			DataSourceReader reader = new DataSourceReader(new PaProject());

			string filename = @"c:\junk1\junk2\junk3.wav";

			object[] args = new object[] { filename, (long)-1, (long)-1 };
			string result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(0, (long)args[1]);
			Assert.AreEqual(0, (long)args[2]);

			args = new object[] { filename + " 2.123", (long)-1, (long)-1 };
			result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(0, (long)args[2]);

			args = new object[] { filename + " 2.123 4.43265", (long)-1, (long)-1 };
			result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(4433, (long)args[2]);

			filename = @"c:\junk1\junk2\this is junk3.mp3";
			args = new object[] { filename + " 2.123 4.43265", (long)-1, (long)-1 };
			result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(4433, (long)args[2]);
		}
	}
}