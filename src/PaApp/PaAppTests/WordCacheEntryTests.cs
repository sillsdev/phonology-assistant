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
using SIL.Pa.Data;
using SIL.SpeechTools.TestUtils;

namespace SIL.Pa
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DataUtils that reqire a database.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class WordCacheEntryTests : TestBase
	{
		string[][] m_results;
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
			PaApp.WordCache = new WordCache();
			m_entry = new WordCacheEntry(true);
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
		/// Simple test for building phone cache with uncertain phone group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertainties1()
		{
			m_entry = new WordCacheEntry(true);
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
			m_entry = new WordCacheEntry(true);
			m_entry["Phonetic"] = "ab(t/d)c(e/i/o)";
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(8, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
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
		/// Test for building phone cache with 2 uncertain phone groups in a word. (This test
		/// will test that PA-710 is fixed).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertaintiesWithMultiGroups1()
		{
			m_entry = new WordCacheEntry(true);
			m_entry["Phonetic"] = "ab(t/d)cxy(u/i)z";
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(10, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
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
			m_entry = new WordCacheEntry(true);
			m_entry["Phonetic"] = "ab(t/d)cxy(u/i)z(t/d)mn";
			PaApp.WordCache.Add(m_entry);
			PaApp.BuildPhoneCache();

			Assert.AreEqual(12, PaApp.PhoneCache.Count);
			Assert.IsNotNull(PaApp.PhoneCache["t"]);
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
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_1x0()
		{
			m_entry["Phonetic"] = "(1/2)n";
			m_results = m_entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(2, m_results.Length);
			
			Assert.AreEqual("1", m_results[0][0]);
			Assert.AreEqual("n", m_results[0][1]);
			
			Assert.AreEqual("2", m_results[1][0]);
			Assert.AreEqual("n", m_results[1][1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_3x2()
		{
			m_entry["Phonetic"] = "(1/2/3)x(4/5)";
			m_results = m_entry.GetAllPossibleUncertainWords(true);
			Assert.AreEqual(6, m_results.Length);
			
			Assert.AreEqual("|1", m_results[0][0]);
			Assert.AreEqual("|4", m_results[0][2]);

			Assert.AreEqual("|1", m_results[1][0]);
			Assert.AreEqual("|5", m_results[1][2]);

			Assert.AreEqual("|2", m_results[2][0]);
			Assert.AreEqual("|4", m_results[2][2]);

			Assert.AreEqual("|2", m_results[3][0]);
			Assert.AreEqual("|5", m_results[3][2]);

			Assert.AreEqual("|3", m_results[4][0]);
			Assert.AreEqual("|4", m_results[4][2]);

			Assert.AreEqual("|3", m_results[5][0]);
			Assert.AreEqual("|5", m_results[5][2]);
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_2x3()
		{
			m_entry["Phonetic"] = "(1/2)x(3/4/5)";
			m_results = m_entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(6, m_results.Length);

			Assert.AreEqual("1", m_results[0][0]);
			Assert.AreEqual("3", m_results[0][2]);

			Assert.AreEqual("1", m_results[1][0]);
			Assert.AreEqual("4", m_results[1][2]);

			Assert.AreEqual("1", m_results[2][0]);
			Assert.AreEqual("5", m_results[2][2]);

			Assert.AreEqual("2", m_results[3][0]);
			Assert.AreEqual("3", m_results[3][2]);

			Assert.AreEqual("2", m_results[4][0]);
			Assert.AreEqual("4", m_results[4][2]);

			Assert.AreEqual("2", m_results[5][0]);
			Assert.AreEqual("5", m_results[5][2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_2x2()
		{
			string[][] result;
			WordCacheEntry entry = new WordCacheEntry(true);

			entry["Phonetic"] = "(1/2)x(3/4)";
			result = entry.GetAllPossibleUncertainWords(true);
			Assert.AreEqual(4, result.Length);
			
			Assert.AreEqual("|1", result[0][0]);
			Assert.AreEqual("|3", result[0][2]);

			Assert.AreEqual("|1", result[1][0]);
			Assert.AreEqual("|4", result[1][2]);

			Assert.AreEqual("|2", result[2][0]);
			Assert.AreEqual("|3", result[2][2]);

			Assert.AreEqual("|2", result[3][0]);
			Assert.AreEqual("|4", result[3][2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_EmptySet()
		{
			m_entry["Phonetic"] = "ols(o/e/0)n";
			m_results = m_entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(3, m_results.Length);
			
			Assert.AreEqual("o", m_results[0][0]);
			Assert.AreEqual("l", m_results[0][1]);
			Assert.AreEqual("s", m_results[0][2]);
			Assert.AreEqual("o", m_results[0][3]);
			Assert.AreEqual("n", m_results[0][4]);

			Assert.AreEqual("o", m_results[1][0]);
			Assert.AreEqual("l", m_results[1][1]);
			Assert.AreEqual("s", m_results[1][2]);
			Assert.AreEqual("e", m_results[1][3]);
			Assert.AreEqual("n", m_results[1][4]);

			Assert.AreEqual("o", m_results[2][0]);
			Assert.AreEqual("l", m_results[2][1]);
			Assert.AreEqual("s", m_results[2][2]);
			Assert.AreEqual("", m_results[2][3]);
			Assert.AreEqual("n", m_results[2][4]);
		}
	}
}