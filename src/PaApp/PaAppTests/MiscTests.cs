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
			PaApp.WordCache = new WordCache();
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
			WordCacheEntry entry = new WordCacheEntry(true);
			entry["Phonetic"] = "abc";
			PaApp.WordCache.Add(entry);
			entry = new WordCacheEntry(true);
			entry["Phonetic"] = "xyz";
			PaApp.WordCache.Add(entry);

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
			WordCacheEntry entry = new WordCacheEntry(true);
			entry["Phonetic"] = "abc";
			PaApp.WordCache.Add(entry);
			entry = new WordCacheEntry(true);
			entry["Phonetic"] = "axc";
			PaApp.WordCache.Add(entry);

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
			WordCacheEntry entry = new WordCacheEntry(true);
			entry["Phonetic"] = "ab" + PaApp.BreakChars[0].ToString() + "xy";
			PaApp.WordCache.Add(entry);

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

			RecordCacheEntry recEntry = new RecordCacheEntry(true);
			recEntry.DataSource = new PaDataSource();
			WordCacheEntry entry = new WordCacheEntry(recEntry, true);
			entry["Phonetic"] = "abXY";
			PaApp.WordCache.Add(entry);
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