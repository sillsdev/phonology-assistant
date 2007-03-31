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

namespace SIL.SpeechTools.Database
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DBUtils that reqire a database.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class MiscDBUtilesTests
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
			DBUtils.DatabaseFile = TestDBMaker.MakeEmptyTestDB();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Close and delete the test database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
            DBUtils.CloseDatabase();
			TestDBMaker.DeleteDB();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
        public void TestSetup()
        {
			DBUtils.BeginTransaction();
        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
        public void TestTearDown()
        {
            DBUtils.RollBackTransaction();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test the saving tree expansion states in the DB.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveExpansionStatesTest()
		{
			int[] fldrids = new int[5];

			string title = "testcat1";
			int	catid1 = DBUtils.AddCategory(ref title);
			title = "testcat2";
			int catid2 = DBUtils.AddCategory(ref title);

			title = "testfldr1";
			fldrids[0] = DBUtils.AddFolder(catid1, ref title);
			title = "testfldr2";
			fldrids[1] = DBUtils.AddFolder(catid1, ref title);
			title = "testfldr3";
			fldrids[2] = DBUtils.AddFolder(catid1, ref title);
			title = "testfldr4";
			fldrids[3] = DBUtils.AddFolder(catid2, ref title);
			title = "testfldr5";
			fldrids[4] = DBUtils.AddFolder(catid2, ref title);

			Dictionary<int, bool> catExpStates = new Dictionary<int, bool>();
			Dictionary<int, bool> fldrExpStates = new Dictionary<int, bool>();

			catExpStates.Add(catid1, true);
			catExpStates.Add(catid2, false);
			fldrExpStates.Add(fldrids[0], false);
			fldrExpStates.Add(fldrids[1], false);
			fldrExpStates.Add(fldrids[2], true);
			fldrExpStates.Add(fldrids[3], true);
			fldrExpStates.Add(fldrids[4], false);

			DBUtils.SaveCategoryFolderExpansionStates(catExpStates, fldrExpStates);

			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB("Category"))
			{
				reader.Read();
				Assert.IsTrue((bool)reader["CExp"]);
				reader.Read();
				Assert.IsFalse((bool)reader["CExp"]);
				reader.Close();
			}

			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB("Folder"))
			{
				reader.Read();
				Assert.IsFalse((bool)reader["FExp"]);
				reader.Read();
				Assert.IsFalse((bool)reader["FExp"]);
				reader.Read();
				Assert.IsTrue((bool)reader["FExp"]);
				reader.Read();
				Assert.IsTrue((bool)reader["FExp"]);
				reader.Read();
				Assert.IsFalse((bool)reader["FExp"]);
				reader.Close();
			}			
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddCategory and AddFolder methods.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddCategoryAndFolderTest()
		{
			string title = "grommit";
			int catid = DBUtils.AddCategory(ref title);
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'Category', \"CatTitle='grommit'\")"));
			Assert.IsTrue(catid > 0);

			title = "wrong trousers";
			int folderid = DBUtils.AddFolder(catid, ref title);
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'Folder', \"FolderTitle='wrong trousers'\")"));
			Assert.IsTrue(folderid > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test adding records to list tables and getting back the record id. Also test that
		/// records are not added when the word already exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UpdateListTablesTest()
		{
			Assert.AreEqual(0, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'GlossList')"));
			int id = DBUtils.UpdateListTable(DBFields.Gloss, "mojo");
			Assert.IsTrue(id > 0);
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'GlossList')"));
			Assert.AreEqual(id, DBUtils.UpdateListTable(DBFields.Gloss, "mojo"));
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'GlossList')"));
			string hexGloss = (string)DBUtils.GetScalerValueFromSQL("SELECT HexGloss FROM GlossList");
			Assert.AreEqual("006D 006F 006A 006F", hexGloss);

			Assert.AreEqual(0, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'POSList')"));
			id = DBUtils.UpdateListTable(DBFields.POS, "gumby-yaya");
			Assert.IsTrue(id > 0);
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'POSList')"));
			Assert.AreEqual(id, DBUtils.UpdateListTable(DBFields.POS, "gumby-yaya"));
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'POSList')"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test renaming a category, folder or document title.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameTitleTest()
		{
			// Test Category.
			string title = "bugs-c";
			int idc = DBUtils.AddCategory(ref title);
			DBUtils.RenameTitle(idc, DBFields.CatTitle, "foghorn-c");
			Assert.AreEqual(0, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'Category', \"CatTitle='bugs-c'\")"));
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'Category', \"CatTitle='foghorn-c'\")"));

			// Test Folder.
			title = "bugs-f";
			int idf = DBUtils.AddFolder(idc, ref title);
			DBUtils.RenameTitle(idf, DBFields.FolderTitle, "foghorn-f");
			Assert.AreEqual(0, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'Folder', \"FolderTitle='bugs-f'\")"));
			Assert.AreEqual(1, DBUtils.GetScalerValueFromSQL("SELECT DCOUNT('*', 'Folder', \"FolderTitle='foghorn-f'\")"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetSpeakerId method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSpeakerIdTest()
		{
			int id = DBUtils.GetSpeakerId("DomFerrari", Gender.Male, false);
			Assert.AreEqual(0, id);

			// Add the speaker.
			id = DBUtils.GetSpeakerId("DomFerrari", Gender.Male, true);
			Assert.IsTrue(id > 0);
			Assert.AreEqual(id, DBUtils.GetSpeakerId("DomFerrari", Gender.Male, false));

			// Verify the gender was specified.
			string sql = DBUtils.SelectSQL("Speaker", "Gender", DBFields.SpeakerId,	id);
			Assert.AreEqual(Gender.Male, (Gender)(byte)DBUtils.GetScalerValueFromSQL(sql));

			// Add another speaker.
			id = DBUtils.GetSpeakerId("FranFerrari", Gender.Female, true);
			sql = DBUtils.SelectSQL("Speaker", "Gender", DBFields.SpeakerName, "FranFerrari");
			Assert.AreEqual(Gender.Female, (Gender)(byte)DBUtils.GetScalerValueFromSQL(sql));
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the hex representation of a string is returned properly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void StrToHexTest()
		{
			Assert.AreEqual("0041 0042 0059 005A 0061 0062 0079 007A", DBUtils.StrToHex("ABYZabyz"));
		}
	}
}