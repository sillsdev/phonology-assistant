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
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsTest_1x0()
		{
			string[][] result;
			WordCacheEntry entry = new WordCacheEntry(true);

			entry["Phonetic"] = "(1/2)n";
			result = entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(2, result.Length);
			
			Assert.AreEqual("1", result[0][0]);
			Assert.AreEqual("n", result[0][1]);
			
			Assert.AreEqual("2", result[1][0]);
			Assert.AreEqual("n", result[1][1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsTest_3x2()
		{
			string[][] result;
			WordCacheEntry entry = new WordCacheEntry(true);

			entry["Phonetic"] = "(1/2/3)x(4/5)";
			result = entry.GetAllPossibleUncertainWords(true);
			Assert.AreEqual(6, result.Length);
			
			Assert.AreEqual("|1", result[0][0]);
			Assert.AreEqual("|4", result[0][2]);

			Assert.AreEqual("|1", result[1][0]);
			Assert.AreEqual("|5", result[1][2]);

			Assert.AreEqual("|2", result[2][0]);
			Assert.AreEqual("|4", result[2][2]);

			Assert.AreEqual("|2", result[3][0]);
			Assert.AreEqual("|5", result[3][2]);

			Assert.AreEqual("|3", result[4][0]);
			Assert.AreEqual("|4", result[4][2]);

			Assert.AreEqual("|3", result[5][0]);
			Assert.AreEqual("|5", result[5][2]);
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsTest_2x3()
		{
			string[][] result;
			WordCacheEntry entry = new WordCacheEntry(true);

			entry["Phonetic"] = "(1/2)x(3/4/5)";
			result = entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(6, result.Length);

			Assert.AreEqual("1", result[0][0]);
			Assert.AreEqual("3", result[0][2]);

			Assert.AreEqual("1", result[1][0]);
			Assert.AreEqual("4", result[1][2]);

			Assert.AreEqual("1", result[2][0]);
			Assert.AreEqual("5", result[2][2]);

			Assert.AreEqual("2", result[3][0]);
			Assert.AreEqual("3", result[3][2]);

			Assert.AreEqual("2", result[4][0]);
			Assert.AreEqual("4", result[4][2]);

			Assert.AreEqual("2", result[5][0]);
			Assert.AreEqual("5", result[5][2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetAllPossibleUncertainWords property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsTest_2x2()
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
		public void PossibleWordsTest_EmptySet()
		{
			string[][] result;
			WordCacheEntry entry = new WordCacheEntry(true);

			entry["Phonetic"] = "ols(o/e/0)n";
			result = entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(3, result.Length);
			
			Assert.AreEqual("o", result[0][0]);
			Assert.AreEqual("l", result[0][1]);
			Assert.AreEqual("s", result[0][2]);
			Assert.AreEqual("o", result[0][3]);
			Assert.AreEqual("n", result[0][4]);

			Assert.AreEqual("o", result[1][0]);
			Assert.AreEqual("l", result[1][1]);
			Assert.AreEqual("s", result[1][2]);
			Assert.AreEqual("e", result[1][3]);
			Assert.AreEqual("n", result[1][4]);

			Assert.AreEqual("o", result[2][0]);
			Assert.AreEqual("l", result[2][1]);
			Assert.AreEqual("s", result[2][2]);
			Assert.AreEqual("", result[2][3]);
			Assert.AreEqual("n", result[2][4]);
		}
	}
}