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
// File: DeleteTests.cs
// Responsibility: ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIL.Pa.TestUtils;
using SIL.Pa;


namespace SIL.Pa.Database
{
	///// --------------------------------------------------------------------------------
	///// <summary>
	///// Tests the deletion of DB information such as categories, folders, and documents.
	///// </summary>
	///// --------------------------------------------------------------------------------
	//[TestClass]
	//public class DeleteTests
	//{
	//    private int m_catId;
	//    private int m_folderId;
	//    private int m_documentId;
	//    private int m_WordListId;
	//    private int m_TotalCount;

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Create temporary test records.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    [TestInitialize]
	//    public void TestInitialize()
	//    {
	//        string dbPath = TestDBMaker.MakeEmptyTestDB();
	//        DBUtils.DatabaseFile = dbPath;

	//        // Create a record
	//        Record rec = new Record();
	//        rec.CatTitle = "Category1 Title";
	//        rec.FolderTitle = "Folder1 Title";
	//        rec.DocTitle = "Cat1 Fdr1 Doc1 Title";
	//        rec.SpeakerName = "Richard Drufus";
	//        rec.Gloss = "Gloss111";
	//        rec.POS = "POS111";
	//        rec.Phonemic = "Phonemic111";
	//        rec.Tone = "Tone111";
	//        rec.Ortho = "Ortho111";
	//        rec.Phonetic = "Phonetic111";
	//        rec.Reference = "wordref";

	//        // Add the record to the database
	//        DocUpdate docUpdate = new DocUpdate();
	//        docUpdate.Add(rec);

	//        // Get Category ID
	//        PaDataTable m_table = new PaDataTable("Category");
	//        m_catId = DBUtils.GetIdForField(DBFields.CatTitle, "Category1 Title");

	//        // Get Folder ID
	//        m_table = new PaDataTable("Folder");
	//        m_folderId = DBUtils.GetIdForField(DBFields.FolderTitle, "Folder1 Title");

	//        // Get Document ID
	//        m_table = new PaDataTable("Document");
	//        m_documentId = DBUtils.GetIdForField(DBFields.DocTitle, "Cat1 Fdr1 Doc1 Title");

	//        // Get TotalCount from the WordList table
	//        m_WordListId = (int)DBUtils.GetScalerValueFromSQL(
	//            DBUtils.SelectSQL("WordListID", "AllWordsIndex", DBFields.DocId, m_documentId));
	//        m_TotalCount = (int)DBUtils.GetScalerValueFromSQL(
	//            DBUtils.SelectSQL("TotalCount", "WordList", DBFields.WordListId, m_WordListId));
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Close and delete the test database.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    [TestCleanup]
	//    public void TestCleanup()
	//    {
	//        DBUtils.CloseDatabase();
	//        TestDBMaker.DeleteDB();
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Ensure the database records were added correctly before testing.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    public void EnsureRecordsAdded()
	//    {
	//        int result;

	//        // Test to make sure the database records were added
	//        result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Category", m_catId));
	//        Assert.AreEqual(1, result, "DID NOT ADD CATEGORY");

	//        result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Folder", m_folderId));
	//        Assert.AreEqual(1, result, "DID NOT ADD FOLDER");

	//        result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Document", m_documentId));
	//        Assert.AreEqual(1, result, "DID NOT ADD DOCUMENT");

	//        result =
	//            (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("AllWordsIndex", m_documentId));
	//        Assert.AreEqual(1, result, "DID NOT ADD ALLWORDSINDEX");

	//        Assert.AreEqual(1, m_TotalCount, "DID NOT INCREMENT WORDLIST TOTALCOUNT");
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Ensure TotalCount was updated correctly
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    public void CheckTotalCount()
	//    {
	//        m_TotalCount = (int)DBUtils.GetScalerValueFromSQL(
	//            DBUtils.SelectSQL("TotalCount", "WordList", DBFields.WordListId, m_WordListId));
	//        Assert.AreEqual(0, m_TotalCount, "DID NOT DECREMENT WORDLIST TOTALCOUNT");
	//    }

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Test the deletion of a category.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[TestMethod]
		//public void DeleteCategory()
		//{
		//    int result;

		//    // Ensure the database records were added
		//    EnsureRecordsAdded();

		//    // Delete the Category
		//    DBUtils.DeleteCategory(m_catId);

		//    // Test to make sure the database records were deleted
		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Category", m_catId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE CATEGORY");

		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Folder", m_folderId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE FOLDER");

		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Document", m_documentId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE DOCUMENT");

		//    result = 
		//        (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("AllWordsIndex", m_documentId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE ALLWORDSINDEX");

		//    // Check to see if TotalCount was updated correctly
		//    CheckTotalCount();
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Test the deletion of a folder. Ensure that the cooresponding Category is NOT deleted.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[TestMethod]
		//public void DeleteFolder()
		//{
		//    int result;

		//    // Ensure the database records were added
		//    EnsureRecordsAdded();

		//    // Delete the Folder
		//    DBUtils.DeleteFolder(DBFields.FolderId, m_folderId);

		//    // Test to make sure the database records were NOT deleted
		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Category", m_catId));
		//    Assert.AreEqual(1, result, "DELETED CATEGORY");

		//    // Test to make sure the database records were deleted
		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Folder", m_folderId));
		//    Assert.AreEqual(0, result, "DELETED FOLDER");

		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Document", m_documentId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE DOCUMENT");

		//    result =
		//        (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("AllWordsIndex", m_documentId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE ALLWORDSINDEX");

		//    // Check to see if TotalCount was updated correctly
		//    CheckTotalCount();
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Test the deletion of a document. Ensure that the cooresponding Category and Folder
		///// are NOT deleted.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[TestMethod]
		//public void DeleteDocument()
		//{
		//    int result;

		//    // Ensure the database records were added
		//    EnsureRecordsAdded();

		//    // Delete the Document
		//    DBUtils.DeleteDocument(DBFields.DocId, m_documentId);

		//    // Test to make sure the database records were NOT deleted
		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Category", m_catId));
		//    Assert.AreEqual(1, result, "DELETED CATEGORY");

		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Folder", m_folderId));
		//    Assert.AreEqual(1, result, "DELETED FOLDER");

		//    // Test to make sure the database records were deleted
		//    result = (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("Document", m_documentId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE DOCUMENT");

		//    result = 
		//        (int)DBUtils.GetScalerValueFromSQL(DBUtils.CountSQL("AllWordsIndex", m_documentId));
		//    Assert.AreEqual(0, result, "DID NOT DELETE ALLWORDSINDEX");

		//    // Check to see if TotalCount was updated correctly
		//    CheckTotalCount();
		//}
	//}
}