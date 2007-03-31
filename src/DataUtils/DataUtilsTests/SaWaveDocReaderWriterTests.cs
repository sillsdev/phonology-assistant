using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;
using NUnit.Framework;
using SIL.SpeechTools.Utils;
using SIL.SpeechTools.TestUtils;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests the PhoneticWriter class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SaWaveDocReaderWriterTests : TestBase
	{
		private OleDbConnection m_dbConnection;
		private string m_saDbConnectionStr;
		private string m_mockWaveFile1Path;
		private string m_mockWaveFile2Path;
		private SaWaveDocumentWriter m_writer;
		private SaWaveDocumentReader m_reader;
		private object[] m_3args;
		private object[] m_2args;
		private string m_md5ForWaveFile1;
		private string m_md5ForWaveFile2;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary
		/// <param name="testContext"></param>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			// These will be used to get MD5 hash codes.
			// For the test, we don't need real wave files.
			m_mockWaveFile1Path = TestWaveMaker.MakeTestWave(1);
			m_mockWaveFile2Path = TestWaveMaker.MakeTestWave(2);

			string dbPath = TestSaDBMaker.MakeEmptyTestDB();
			m_saDbConnectionStr = string.Format(DBUtils.kJetDBConnectionString, dbPath);
			m_dbConnection = new OleDbConnection(m_saDbConnectionStr);
			m_dbConnection.Open();
			
			if (m_writer == null)
				m_writer = new SaWaveDocumentWriter();
			
			if (m_reader == null)
				m_reader = new SaWaveDocumentReader();
		
			m_md5ForWaveFile1 = STUtils.GetFilesMD5Hash(m_mockWaveFile1Path);
			m_md5ForWaveFile2 = STUtils.GetFilesMD5Hash(m_mockWaveFile2Path);
			m_2args = new object[] { m_mockWaveFile1Path, m_dbConnection };
			m_3args = new object[] { m_mockWaveFile1Path, m_md5ForWaveFile1, m_dbConnection };
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Close and delete the test database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			if (m_writer != null)
				m_writer.Close();
			
			if (m_reader != null)
				m_reader.Close();

			m_writer = null;
			m_reader = null;

			TestWaveMaker.Delete(1);
			TestWaveMaker.Delete(2);
			TestSaDBMaker.DeleteDB();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the DB connections for the reader and writer are closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
			OleDbCommand cmd = new OleDbCommand("DELETE * FROM WaveDocuments", m_dbConnection);
			cmd.ExecuteNonQuery();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the writer stores non segment data in the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteRead_NonSegmentInfoTest()
		{
			m_3args[0] = m_mockWaveFile1Path;
			CallMethod(m_writer, "Initialize", m_3args);
			
			Assert.AreEqual(m_md5ForWaveFile1, GetField(m_writer, "m_origMD5HashCode") as string);
			Assert.AreEqual(0, m_writer.WaveDocId);

			m_writer.BitsPerSample = 111;
			m_writer.BlockAlignment = 222;
			m_writer.Country = "Olsonstan";
			m_writer.EthnologueId = "oly";
			int wavDocId = m_writer.Commit();

			m_2args[0] = m_mockWaveFile1Path;
			CallMethod(m_reader, "Initialize", m_2args);

			Assert.AreEqual(wavDocId, m_reader.WaveDocId);
			Assert.AreEqual(m_md5ForWaveFile1, m_reader.MD5HashCode);
			Assert.AreEqual(111, m_reader.BitsPerSample);
			Assert.AreEqual(222, m_reader.BlockAlignment);
			Assert.AreEqual("Olsonstan", m_reader.Country);
			Assert.AreEqual("oly", m_reader.EthnologueId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the writer stores non segment data in the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Write_ReferencingDatabasesTest()
		{
			// Add a wave document record
			m_3args[0] = m_mockWaveFile1Path;
			CallMethod(m_writer, "Initialize", m_3args);
			int wavDocId = m_writer.Commit();

			// Add a database reference to the wave document just added.
			Guid dbGuid = Guid.NewGuid();
			string sql = string.Format("insert into ReferencingDatabases (WaveDocId, DatabaseGuid) " +
				"values ({0}, '{1}')", wavDocId, dbGuid);
			OleDbCommand cmd = new OleDbCommand(sql, m_dbConnection);
			cmd.ExecuteNonQuery();

			// Make sure the LastMD5HashCode field of the record just added to the
			// ReferencingDatabases table is null;
			sql = string.Format("select LastMD5HashCode from ReferencingDatabases where " +
				"WaveDocId={0}", wavDocId);
			cmd = new OleDbCommand(sql, m_dbConnection);
			Assert.IsNull(cmd.ExecuteScalar() as string);

			// update the wave document record
			m_3args[0] = m_mockWaveFile2Path;
			CallMethod(m_writer, "Initialize", m_3args);

			m_writer.EthnologueId = "pso";
			m_writer.Commit();

			// Make sure the md5 code in the referencing table got updated when the writer's
			// commit method was called.
			Assert.AreEqual(m_md5ForWaveFile1, cmd.ExecuteScalar() as string);

			// Finally, make sure the WaveDocument record that was updated is
			// the same record but has a new MD5 in it.
			m_2args[0] = m_mockWaveFile2Path;
			CallMethod(m_reader, "Initialize", m_2args);

			Assert.AreEqual(wavDocId, m_reader.WaveDocId);
			Assert.AreEqual(STUtils.GetFilesMD5Hash(m_mockWaveFile2Path), m_reader.MD5HashCode);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the writer stores non segment data in the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteRead_SegmentInfoTest()
		{
			WriteDummyDataToSADatabase();
			VerifyTestSegmentsWritten(m_mockWaveFile1Path);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the read result when attempting to read a record for a wave file that isn't
		/// in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Read_ResultWhenWaveNotInDBTest()
		{
			m_2args[0] = m_mockWaveFile1Path;
			CallMethod(m_reader, "Initialize", m_2args);

			int offset, length;
			string annotation;
			bool isBookmark;

			Assert.AreEqual(0, m_reader.WaveDocId);
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation));
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Phonemic, out offset, out length, out annotation));
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Tone, out offset, out length, out annotation));
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Orthographic, out offset, out length, out annotation));

			string gloss, pos, reference;
			Assert.IsFalse(m_reader.ReadMarkSegment(out offset, out length, out gloss,
				out pos, out reference, out isBookmark));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the writer updates information for wave file that already has an
		/// existing record in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Write_UpdateExistingInfoTest()
		{
			// Write non segment info. for wave file for the first time.
			m_3args[0] = m_mockWaveFile1Path;
			CallMethod(m_writer, "Initialize", m_3args);

			m_writer.Country = "Olsonstan";
			m_writer.EthnologueId = "oly";
			int wavDocId = m_writer.Commit();

			// Verify non segment data written.
			m_2args[0] = m_mockWaveFile1Path;
			CallMethod(m_reader, "Initialize", m_2args);

			Assert.AreEqual(wavDocId, m_reader.WaveDocId);
			Assert.AreEqual("Olsonstan", m_reader.Country);
			Assert.AreEqual("oly", m_reader.EthnologueId);

			// Write then verify segments written.
			WriteDummyDataToSADatabase();
			VerifyTestSegmentsWritten(m_mockWaveFile1Path);

			m_3args[0] = m_mockWaveFile2Path;
			CallMethod(m_writer, "Initialize", m_3args);

			m_writer.Country = "Davidsville";
			m_writer.EthnologueId = "ddo";
			m_writer.AddSegment((int)AnnotationType.Phonetic, 1, 11, "steelers");
			m_writer.AddSegment((int)AnnotationType.Phonemic, 2, 22, "seahawks");
			m_writer.AddSegment((int)AnnotationType.Orthographic, 3, 33, "panthers");
			m_writer.AddSegment((int)AnnotationType.Tone, 4, 44, "mariners");
			m_writer.Commit();

			// Verify updated information was written.
			m_2args[0] = m_mockWaveFile2Path;
			CallMethod(m_reader, "Initialize", m_2args);
			
			Assert.AreEqual(wavDocId, m_reader.WaveDocId);
			Assert.AreEqual(m_reader.MD5HashCode, m_md5ForWaveFile2);
			Assert.AreEqual("Davidsville", m_reader.Country);
			Assert.AreEqual("ddo", m_reader.EthnologueId);

			int offset, length;
			string annotation;

			bool ret = m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(1, offset);
			Assert.AreEqual(11, length);
			Assert.AreEqual("steelers", annotation);
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation));

			ret = m_reader.ReadSegment((int)AnnotationType.Phonemic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(2, offset);
			Assert.AreEqual(22, length);
			Assert.AreEqual("seahawks", annotation);
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Phonemic, out offset, out length, out annotation));

			ret = m_reader.ReadSegment((int)AnnotationType.Orthographic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(3, offset);
			Assert.AreEqual(33, length);
			Assert.AreEqual("panthers", annotation);
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Orthographic, out offset, out length, out annotation));

			ret = m_reader.ReadSegment((int)AnnotationType.Tone, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(4, offset);
			Assert.AreEqual(44, length);
			Assert.AreEqual("mariners", annotation);
			Assert.IsFalse(m_reader.ReadSegment((int)AnnotationType.Tone, out offset, out length, out annotation));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes some test data to the SA database.
		/// </summary>
		/// <param name="md5"></param>
		/// ------------------------------------------------------------------------------------
		private void WriteDummyDataToSADatabase()
		{
			m_3args[0] = m_mockWaveFile1Path;
			CallMethod(m_writer, "Initialize", m_3args);

			m_writer.AddSegment((int)AnnotationType.Phonetic, 1, 11, "pig");
			m_writer.AddSegment((int)AnnotationType.Phonemic, 1, 11, "cow");
			m_writer.AddSegment((int)AnnotationType.Phonetic, 200, 22, "chicken");
			m_writer.AddSegment((int)AnnotationType.Phonemic, 200, 22, "dog");
			m_writer.AddSegment((int)AnnotationType.Phonetic, 300, 33, "horse");
			m_writer.AddSegment((int)AnnotationType.Orthographic, 300, 33, "goat");
			m_writer.AddSegment((int)AnnotationType.Phonetic, 400, 44, "llama");
			m_writer.AddSegment((int)AnnotationType.Phonetic, 500, 55, "sheep");
			m_writer.AddMarkSegment(1, 88, "cat", "bird", "bug", false);
			m_writer.AddMarkSegment(300, 99, "car", "truck", null, false);
			m_writer.Commit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that segments were properly written to the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void VerifyTestSegmentsWritten(string waveFilePath)
		{
			m_2args[0] = waveFilePath;
			CallMethod(m_reader, "Initialize", m_2args);

			int offset, length;
			string annotation;

			bool ret = m_reader.ReadSegment((int)AnnotationType.Tone, out offset, out length, out annotation);
			Assert.IsFalse(ret);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonemic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(1, offset);
			Assert.AreEqual(11, length);
			Assert.AreEqual("cow", annotation);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonemic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(200, offset);
			Assert.AreEqual(22, length);
			Assert.AreEqual("dog", annotation);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonemic, out offset, out length, out annotation);
			Assert.IsFalse(ret);

			ret = m_reader.ReadSegment((int)AnnotationType.Orthographic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(300, offset);
			Assert.AreEqual(33, length);
			Assert.AreEqual("goat", annotation);

			ret = m_reader.ReadSegment((int)AnnotationType.Orthographic, out offset, out length, out annotation);
			Assert.IsFalse(ret);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(1, offset);
			Assert.AreEqual(11, length);
			Assert.AreEqual("pig", annotation);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(200, offset);
			Assert.AreEqual(22, length);
			Assert.AreEqual("chicken", annotation);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(300, offset);
			Assert.AreEqual(33, length);
			Assert.AreEqual("horse", annotation);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(400, offset);
			Assert.AreEqual(44, length);
			Assert.AreEqual("llama", annotation);

			ret = m_reader.ReadSegment((int)AnnotationType.Phonetic, out offset, out length, out annotation);
			Assert.IsTrue(ret);
			Assert.AreEqual(500, offset);
			Assert.AreEqual(55, length);
			Assert.AreEqual("sheep", annotation);

			bool isBookmark;
			string gloss, pos, reference;
			ret = m_reader.ReadMarkSegment(out offset, out length, out gloss, out pos, out reference, out isBookmark);
			Assert.IsTrue(ret);
			Assert.AreEqual(1, offset);
			Assert.AreEqual(88, length);
			Assert.AreEqual("cat", gloss);
			Assert.AreEqual("bird", pos);
			Assert.AreEqual("bug", reference);

			ret = m_reader.ReadMarkSegment(out offset, out length, out gloss, out pos, out reference, out isBookmark);
			Assert.IsTrue(ret);
			Assert.AreEqual(300, offset);
			Assert.AreEqual(99, length);
			Assert.AreEqual("car", gloss);
			Assert.AreEqual("truck", pos);
			Assert.IsNull(reference);

			ret = m_reader.ReadMarkSegment(out offset, out length, out gloss, out pos, out reference, out isBookmark);
			Assert.IsFalse(ret);
		}
	}
}
