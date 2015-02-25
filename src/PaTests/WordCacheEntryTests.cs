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
		string[][] _results;
		WordCacheEntry _entry;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		[SetUp]
        public void TestSetup()
        {
			_entry = new WordCacheEntry(
				new RecordCacheEntry(_prj) { DataSource = new PaDataSource() } , "Phonetic");
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_1x0()
		{
			_entry["Phonetic"] = "(1/2)n";
			_results = _entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(2, _results.Length);
			
			Assert.AreEqual("1", _results[0][0]);
			Assert.AreEqual("n", _results[0][1]);
			
			Assert.AreEqual("2", _results[1][0]);
			Assert.AreEqual("n", _results[1][1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_3x2()
		{
			_entry["Phonetic"] = "(1/2/3)x(4/5)";
			_results = _entry.GetAllPossibleUncertainWords(true);
			Assert.AreEqual(6, _results.Length);
			
			Assert.AreEqual("|1", _results[0][0]);
			Assert.AreEqual("|4", _results[0][2]);

			Assert.AreEqual("|1", _results[1][0]);
			Assert.AreEqual("|5", _results[1][2]);

			Assert.AreEqual("|2", _results[2][0]);
			Assert.AreEqual("|4", _results[2][2]);

			Assert.AreEqual("|2", _results[3][0]);
			Assert.AreEqual("|5", _results[3][2]);

			Assert.AreEqual("|3", _results[4][0]);
			Assert.AreEqual("|4", _results[4][2]);

			Assert.AreEqual("|3", _results[5][0]);
			Assert.AreEqual("|5", _results[5][2]);
		}


		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_2x3()
		{
			_entry["Phonetic"] = "(1/2)x(3/4/5)";
			_results = _entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(6, _results.Length);

			Assert.AreEqual("1", _results[0][0]);
			Assert.AreEqual("3", _results[0][2]);

			Assert.AreEqual("1", _results[1][0]);
			Assert.AreEqual("4", _results[1][2]);

			Assert.AreEqual("1", _results[2][0]);
			Assert.AreEqual("5", _results[2][2]);

			Assert.AreEqual("2", _results[3][0]);
			Assert.AreEqual("3", _results[3][2]);

			Assert.AreEqual("2", _results[4][0]);
			Assert.AreEqual("4", _results[4][2]);

			Assert.AreEqual("2", _results[5][0]);
			Assert.AreEqual("5", _results[5][2]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PossibleWordsFromUncertainties_2x2()
		{
			_entry["Phonetic"] = "(1/2)x(3/4)";
			var result = _entry.GetAllPossibleUncertainWords(true);
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
		[Test]
		public void PossibleWordsFromUncertainties_EmptySet()
		{
			_entry["Phonetic"] = "ols(o/e/0)n";
			_results = _entry.GetAllPossibleUncertainWords(false);
			Assert.AreEqual(3, _results.Length);
			
			Assert.AreEqual("o", _results[0][0]);
			Assert.AreEqual("l", _results[0][1]);
			Assert.AreEqual("s", _results[0][2]);
			Assert.AreEqual("o", _results[0][3]);
			Assert.AreEqual("n", _results[0][4]);

			Assert.AreEqual("o", _results[1][0]);
			Assert.AreEqual("l", _results[1][1]);
			Assert.AreEqual("s", _results[1][2]);
			Assert.AreEqual("e", _results[1][3]);
			Assert.AreEqual("n", _results[1][4]);

			Assert.AreEqual("o", _results[2][0]);
			Assert.AreEqual("l", _results[2][1]);
			Assert.AreEqual("s", _results[2][2]);
			Assert.AreEqual("", _results[2][3]);
			Assert.AreEqual("n", _results[2][4]);
		}
	}
}