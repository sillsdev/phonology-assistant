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
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
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
			m_entry = new WordCacheEntry();
		}

		#endregion

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
			var entry = new WordCacheEntry();

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