using System;
using System.Text;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is a test class for SIL.SpeechTools.Database.Document and is intended
	/// to contain all SIL.SpeechTools.Database.Document Unit Tests
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class DocumentTest : TestBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="testContext"></param>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
        public void FixtureSetUp()
		{
			string dbPath = TestDBMaker.MakeEmptyTestDB();
			DBUtils.DatabaseFile = dbPath;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A test for GetFullTranscription
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullTranscriptionTest()
		{
			Document doc = new Document(true);
			doc[DBFields.Phonetic] = "now is the time for all good men";
			doc[DBFields.Tone] = "hi low mid hi-to-low";

			string actual = doc.GetFullTranscription(DBFields.Phonetic);
			Assert.AreEqual("now is the time for all good men", actual);

			actual = doc.GetFullTranscription(DBFields.Tone);
			Assert.AreEqual("hi low mid hi-to-low", actual);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A test for committing additions and changes to the document table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CommitDocumentTest()
		{
			// Add a category and folder that will own the document.
			int catId = DBUtils.AddCategory();
			string folderTitle = "dudleys Folder";
			int folderId = DBUtils.AddFolder(catId, ref folderTitle);

			// This is admittedly funky. I do this because when a date field is assigned a
			// date in the form of a string, (which is how we write dates to the DB) the DB
			// Engine has no clue about milliseconds and they are effectively stripped off.
			// Therefore, we need to make sure our date is the same.
			DateTime now = DateTime.Parse(DateTime.Now.ToString());

			Document doc = new Document();
			doc[DBFields.FolderId] = folderId;
			doc[DBFields.DocTitle] = "dudley";
			doc[DBFields.Comments] = "dudleys Comment";
			doc[DBFields.SpeakerName] = "Nell";
			doc[DBFields.Wave] = true;
			doc[DBFields.OriginalDate] = now;
			doc.Commit();
			Assert.IsTrue(doc.Id > 0);

			// Check for a single document link record.
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB("DocumentLinks"))
			{
				reader.Read();
				Assert.AreEqual(doc.Id, (int)reader[DBFields.DocId]);
				Assert.AreEqual(folderId, (int)reader[DBFields.FolderId]);
				reader.Close();
			}

			// Now verify the contents of the document record from the DB.
			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentInfo", doc.Id))
			{
				reader.Read();
				Assert.AreEqual("dudley", reader[DBFields.DocTitle] as string);
				Assert.AreEqual("dudleys Comment", reader[DBFields.Comments] as string);
				Assert.AreEqual("Nell", reader[DBFields.SpeakerName] as string);
				Assert.IsTrue((bool)reader[DBFields.Wave]);
				Assert.AreEqual(now, (DateTime)reader[DBFields.OriginalDate]);
				Assert.IsTrue(reader[DBFields.LastUpdate] is DateTime);
				Assert.IsNotNull(reader[DBFields.LastUpdate]);
				reader.Close();
			}

			// Now change some fields in the document and make sure they get
			// updated in the database.
			doc[DBFields.Dialect] = "dooright";
			doc[DBFields.Comments] = null;
			doc[DBFields.Wave] = false;
			doc.Commit();
			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentInfo", doc.Id))
			{
				reader.Read();
				Assert.AreEqual("dooright", reader[DBFields.Dialect] as string);
				Assert.IsNull(reader[DBFields.Comments] as string);
				Assert.IsFalse((bool)reader[DBFields.Wave]);
				reader.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A test for committing additions and changes to the document header table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CommitDocHeaderTest()
		{
			// Create a document and fill-in some audio data.
			Document doc = new Document();
			doc[DBFields.FormatTag] = 100;
			doc[DBFields.BlockAlign] = 200;
			doc[DBFields.CalcFreqHigh] = 300;
			doc[DBFields.SADescription] = "snidley";
			doc.Commit();

			// Now verify the contents of the document record from the DB.
			string sql = DBUtils.SelectSQL("DocHeader", DBFields.DocId, doc.Id);
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				reader.Read();
				Assert.AreEqual(100, (int)reader[DBFields.FormatTag]);
				Assert.AreEqual(200, (int)reader[DBFields.BlockAlign]);
				Assert.AreEqual(300, (int)reader[DBFields.CalcFreqHigh]);
				Assert.AreEqual("snidley", (string)reader[DBFields.SADescription]);
				reader.Close();
			}

			// Now modify the data and verify the changes were saved.
			doc[DBFields.FormatTag] = 111;
			doc[DBFields.BlockAlign] = 222;
			doc[DBFields.CalcFreqHigh] = 333;
			doc[DBFields.SADescription] = "whiplash";
			doc.Commit();

			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				reader.Read();
				Assert.AreEqual(111, (int)reader[DBFields.FormatTag]);
				Assert.AreEqual(222, (int)reader[DBFields.BlockAlign]);
				Assert.AreEqual(333, (int)reader[DBFields.CalcFreqHigh]);
				Assert.AreEqual("whiplash", (string)reader[DBFields.SADescription]);
				reader.Close();
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A test for GetAddDocumentSQL.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAddDocumentSQLTest()
		{
			DateTime now = DateTime.Now;
			Document doc = new Document();

			// The constructor of the Document initializes it's internal hash table of
			// data values and for the sake of this test we want to start with a virgin
			// internal hash table. Hence these two lines.
			SetField(doc, "m_docData", null);
			SetField(doc, "m_docData", new Dictionary<string, object>());
			
			doc[DBFields.FolderId] = 333;
			doc[DBFields.DocTitle] = "sylvester";
			doc[DBFields.Wave] = true;
			doc[DBFields.OriginalDate] = now;

			string expected = "INSERT INTO Document (" + DBFields.DocTitle + "," +
				DBFields.Wave + "," + DBFields.OriginalDate +
				") VALUES ('sylvester',True,'" + now.ToString() + "')";

			string actual = GetStrResult(doc, "GetAddDocumentSQL", null);
			Assert.AreEqual(expected, actual);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A test for GetUpdateDocumentSQL.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetUpdateDocumentSQLTest()
		{
			DateTime now = DateTime.Now;
			
			Document doc = new Document();
			SetField(doc, "m_docId", 29);
			Assert.AreEqual(29, doc.Id);

			// The constructor of the Document initializes it's internal hash table of
			// data values and for the sake of this test we want to start with a virgin
			// internal hash table. Hence these two lines.
			SetField(doc, "m_docData", null);
			SetField(doc, "m_docData", new Dictionary<string, object>());
			
			doc[DBFields.FolderId] = 333;
			doc[DBFields.Dialect] = "gromovian";
			doc[DBFields.Wave] = true;
			doc[DBFields.OriginalDate] = now;

			string expected = "UPDATE Document SET " + DBFields.Dialect + "='gromovian'," +
				DBFields.Wave + "=True," + DBFields.OriginalDate + "='" + now.ToString() +
				"' WHERE " + DBFields.DocId + "=29";

			string actual = GetStrResult(doc, "GetUpdateDocumentSQL", null);
			Assert.AreEqual(expected, actual);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A test for LinkDocument
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LinkDocumentTest()
		{
			// Add a category to make sure a new folders added in GetDocsFolder has an owner.
			Document doc = new Document(true);
			doc[DBFields.CatId] = DBUtils.AddCategory();
			doc[DBFields.FolderTitle] = "dudleys Folder";

			int id = GetIntResult(doc, "GetFolderId", null);
			Assert.IsTrue(id > 0);

			string sql = DBUtils.SelectSQL("folder", DBFields.FolderId, id);
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				reader.Read();
				Assert.AreEqual(id, (int)reader[DBFields.FolderId]);
				Assert.AreEqual("dudleys Folder", (string)reader[DBFields.FolderTitle]);
				reader.Close();
			}
		}
	}
}
