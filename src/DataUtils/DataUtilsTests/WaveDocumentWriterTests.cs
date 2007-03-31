using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;

namespace SIL.SpeechTools.Database
{
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// Provides access to some protected WaveDocumentWriter methods and data.
	/// </summary>
	/// ------------------------------------------------------------------------------------
	internal class WaveDocumentWriterDummy : WaveDocumentWriter
	{
		public void PrepareSegmentsForCommitAccessor(
			out SortedDictionary<int, DocumentWord> wordData,
			out SortedDictionary<int, PhoneticWordInfo> eticWordInfo)
		{
			PrepareSegmentsForCommit(out wordData, out eticWordInfo);
		}

		public SortedDictionary<int, SegmentData> Segments
		{
			set {m_segments = value;}
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests the PhoneticWriter class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class WaveDocumentWriterTests : TestBase
	{
		#region Setup/Teardown
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

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PackagePhones method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PackagePhonesTest()
		{
			ArrayList phoneInfoArrayList = new ArrayList();
			phoneInfoArrayList.Add(new PhoneInfo("a"));
			phoneInfoArrayList.Add(new PhoneInfo("bb"));
			phoneInfoArrayList.Add(new PhoneInfo("ccc"));
			phoneInfoArrayList.Add(new PhoneInfo("dddd"));

			WaveDocumentWriter wdw = new WaveDocumentWriter();
			PhoneInfo[] phoneInfo = (PhoneInfo[])GetResult(wdw, "PackagePhones", phoneInfoArrayList);

			Assert.AreEqual(RelativePhoneLocation.Initial, phoneInfo[0].Location);
			Assert.AreEqual(RelativePhoneLocation.Medial, phoneInfo[1].Location);
			Assert.AreEqual(RelativePhoneLocation.Medial, phoneInfo[2].Location);
			Assert.AreEqual(RelativePhoneLocation.Final, phoneInfo[3].Location);

			Assert.AreEqual(0, phoneInfo[0].Order);
			Assert.AreEqual(1, phoneInfo[1].Order);
			Assert.AreEqual(2, phoneInfo[2].Order);
			Assert.AreEqual(3, phoneInfo[3].Order);

			Assert.AreEqual(0, phoneInfo[0].Offset);
			Assert.AreEqual(1, phoneInfo[1].Offset);
			Assert.AreEqual(3, phoneInfo[2].Offset);
			Assert.AreEqual(6, phoneInfo[3].Offset);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PrepareSegmentsForCommit method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PrepareSegmentsForCommitTest()
		{
			WaveDocumentWriterDummy wdw = new WaveDocumentWriterDummy();
			SortedDictionary<int, SegmentData> segments = SetupTestSegmentData();
			wdw.Segments = segments;
			
			SortedDictionary<int, DocumentWord> wordData = null;
			SortedDictionary<int, PhoneticWordInfo> eticWordInfo = null;
			wdw.PrepareSegmentsForCommitAccessor(out wordData, out eticWordInfo);

			Assert.AreEqual(2, wordData.Count);
			Assert.AreEqual(2, eticWordInfo.Count);
			
			Assert.AreEqual("abbccc", wordData[0].Phonetic);
			Assert.AreEqual("xqq", wordData[1].Phonetic);

			Assert.AreEqual("ee", wordData[0].Phonemic);
			Assert.AreEqual("ypp", wordData[1].Phonemic);

			Assert.AreEqual("to", wordData[0].Tone);
			Assert.AreEqual("z", wordData[1].Tone);

			Assert.AreEqual("itt", wordData[0].Ortho);
			Assert.IsNull(wordData[1].Ortho);

			Assert.AreEqual("gloss1", wordData[0].Gloss);
			Assert.AreEqual("gloss2", wordData[1].Gloss);

			Assert.AreEqual("pos1", wordData[0].POS);
			Assert.AreEqual("pos2", wordData[1].POS);

			Assert.AreEqual("ref1", wordData[0].Reference);
			Assert.IsNull(wordData[1].Reference);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads a collection of segment data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SortedDictionary<int, SegmentData> SetupTestSegmentData()
		{
			SortedDictionary<int, SegmentData> segments = new SortedDictionary<int, SegmentData>();
			segments[0] = new SegmentData();
			segments[0].MarkDuration = 100;
			segments[0].Phonetic = "a";
			segments[0].Phonemic = "e";
			segments[0].Orthographic = "i";
			segments[0].Tone = "t";
			segments[0].Gloss = "gloss1";
			segments[0].PartOfSpeech = "pos1";
			segments[0].Reference = "ref1";

			segments[10] = new SegmentData();
			segments[10].Phonetic = "bb";
			segments[10].Orthographic = "tt";

			segments[20] = new SegmentData();
			segments[20].Phonetic = "ccc";
			segments[20].Phonemic = "e";
			segments[20].Tone = "o";

			segments[300] = new SegmentData();
			segments[300].MarkDuration = 50;
			segments[300].Phonetic = "x";
			segments[300].Phonemic = "y";
			segments[300].Tone = "z";
			segments[300].Gloss = "gloss2";
			segments[300].PartOfSpeech = "pos2";

			segments[500] = new SegmentData();
			segments[500].Phonetic = "qq";
			segments[500].Phonemic = "pp";

			return segments;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the Commit method writes information to document, docheader, and word
		/// list tables.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CommitTest_1()
		{
			WaveDocumentWriter wdw = new WaveDocumentWriter();
			SetupCommitTestData(wdw);
			int docId = wdw.Commit();

			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentInfo", docId))
			{
				Assert.IsTrue(reader.Read());
				Assert.AreEqual("Southern", reader[DBFields.Dialect] as string);
				Assert.AreEqual("Burt", reader[DBFields.Transcriber] as string);
				Assert.AreEqual("Edna", reader[DBFields.SpeakerName] as string);
				Assert.AreEqual(123, (int)reader[DBFields.AvgBytesPerSecond]);
				Assert.AreEqual(908, (int)reader[DBFields.CalcFreqHigh]);
				Assert.AreEqual(456, (int)reader[DBFields.BitsPerSample]);
				reader.Close();
			}
			
			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentWords", docId))
			{
				Assert.IsTrue(reader.Read());
				Assert.AreEqual("abc", reader[DBFields.Phonetic] as string);
				Assert.IsNull(reader[DBFields.Phonemic] as string);
				Assert.IsNull(reader[DBFields.Ortho] as string);
				Assert.IsNull(reader[DBFields.Tone] as string);
				Assert.AreEqual("gloss1", reader[DBFields.Gloss] as string);
				Assert.AreEqual("ref1", reader[DBFields.Reference] as string);
				Assert.AreEqual("pos1", reader[DBFields.POS] as string);
				Assert.AreEqual(0, (int)reader[DBFields.AnnOffset]);
				Assert.AreEqual(3, (int)reader[DBFields.AnnLength]);
				Assert.AreEqual(0, (int)reader[DBFields.WavOffset]);
				Assert.AreEqual(100, (int)reader[DBFields.WavLength]);

				Assert.IsTrue(reader.Read());
				Assert.AreEqual("xxyyyzzzz", reader[DBFields.Phonetic] as string);
				Assert.AreEqual("dd", reader[DBFields.Phonemic] as string);
				Assert.AreEqual("kk", reader[DBFields.Ortho] as string);
				Assert.AreEqual("gloss2", reader[DBFields.Gloss] as string);
				Assert.IsNull(reader[DBFields.Reference] as string);
				Assert.AreEqual("pos2", reader[DBFields.POS] as string);
				Assert.AreEqual(1, (int)reader[DBFields.AnnOffset]);
				Assert.AreEqual(9, (int)reader[DBFields.AnnLength]);
				Assert.AreEqual(555, (int)reader[DBFields.WavOffset]);
				Assert.AreEqual(200, (int)reader[DBFields.WavLength]);

				Assert.IsTrue(reader.Read());
				Assert.AreEqual("ggs", reader[DBFields.Phonetic] as string);
				Assert.AreEqual("mmnnn", reader[DBFields.Phonemic] as string);
				Assert.AreEqual("t", reader[DBFields.Tone] as string);
				Assert.AreEqual("oooo", reader[DBFields.Ortho] as string);
				Assert.AreEqual("gloss3", reader[DBFields.Gloss] as string);
				Assert.AreEqual("ref3", reader[DBFields.Reference] as string);
				Assert.IsNull(reader[DBFields.POS] as string);
				Assert.AreEqual(2, (int)reader[DBFields.AnnOffset]);
				Assert.AreEqual(3, (int)reader[DBFields.AnnLength]);
				Assert.AreEqual(888, (int)reader[DBFields.WavOffset]);
				Assert.AreEqual(300, (int)reader[DBFields.WavLength]);
			
				reader.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the Commit method writes information to the segment table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CommitTest_2()
		{
			WaveDocumentWriter wdw = new WaveDocumentWriter();
			SetupCommitTestData(wdw);
			int docId = wdw.Commit();

			string sql = string.Format("SELECT * FROM Segment WHERE DocId={0}", docId);
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				Assert.IsTrue(reader.Read());
				Assert.AreEqual("a", reader["PhoneticChar"] as string);
				Assert.IsNull(reader["OrthoChar"] as string);
				Assert.IsNull(reader["PhonemicChar"] as string);
				Assert.IsNull(reader["ToneChar"] as string);
				Assert.AreEqual(0, (int)reader["PhoneticCharOffset"]);
				Assert.AreEqual(0, (int)reader["WavOffset"]);
				Assert.AreEqual(5, (int)reader["WavLength"]);
				Assert.IsTrue((bool)reader["WordBeginning"]);

				Assert.IsTrue(reader.Read());
				Assert.AreEqual("b", reader["PhoneticChar"] as string);
				Assert.IsNull(reader["OrthoChar"] as string);
				Assert.IsNull(reader["PhonemicChar"] as string);
				Assert.IsNull(reader["ToneChar"] as string);
				Assert.AreEqual(1, (int)reader["PhoneticCharOffset"]);
				Assert.AreEqual(75, (int)reader["WavOffset"]);
				Assert.AreEqual(15, (int)reader["WavLength"]);
				Assert.IsFalse((bool)reader["WordBeginning"]);

				Assert.IsTrue(reader.Read());
				Assert.AreEqual("c", reader["PhoneticChar"] as string);
				Assert.IsNull(reader["OrthoChar"] as string);
				Assert.IsNull(reader["PhonemicChar"] as string);
				Assert.IsNull(reader["ToneChar"] as string);
				Assert.AreEqual(2, (int)reader["PhoneticCharOffset"]);
				Assert.AreEqual(400, (int)reader["WavOffset"]);
				Assert.AreEqual(25, (int)reader["WavLength"]);
				Assert.IsFalse((bool)reader["WordBeginning"]);

				Assert.IsTrue(reader.Read());
				Assert.AreEqual("xx", reader["PhoneticChar"] as string);
				Assert.IsNull(reader["OrthoChar"] as string);
				Assert.AreEqual("dd", reader["PhonemicChar"] as string);
				Assert.IsNull(reader["ToneChar"] as string);
				Assert.AreEqual(4, (int)reader["PhoneticCharOffset"]);
				Assert.AreEqual(555, (int)reader["WavOffset"]);
				Assert.AreEqual(55, (int)reader["WavLength"]);
				Assert.IsTrue((bool)reader["WordBeginning"]);

				Assert.IsTrue(reader.Read());
				Assert.AreEqual("yyy", reader["PhoneticChar"] as string);
				Assert.AreEqual("kk", reader["OrthoChar"] as string);
				Assert.IsNull(reader["PhonemicChar"] as string);
				Assert.IsNull(reader["ToneChar"] as string);
				Assert.AreEqual(6, (int)reader["PhoneticCharOffset"]);
				Assert.AreEqual(606, (int)reader["WavOffset"]);
				Assert.AreEqual(65, (int)reader["WavLength"]);
				Assert.IsFalse((bool)reader["WordBeginning"]);

				Assert.IsTrue(reader.Read());
				Assert.AreEqual("zzzz", reader["PhoneticChar"] as string);
				Assert.IsNull(reader["OrthoChar"] as string);
				Assert.IsNull(reader["PhonemicChar"] as string);
				Assert.IsNull(reader["ToneChar"] as string);
				Assert.AreEqual(9, (int)reader["PhoneticCharOffset"]);
				Assert.AreEqual(777, (int)reader["WavOffset"]);
				Assert.AreEqual(75, (int)reader["WavLength"]);
				Assert.IsFalse((bool)reader["WordBeginning"]);

				reader.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads data into a WaveDocumentWriter for commit tests.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupCommitTestData(WaveDocumentWriter wdw)
		{
			wdw.AverageBytesPerSecond = 123;
			wdw.CalcFreqHigh = 908;
			wdw.BitsPerSample = 456;
			wdw.Dialect = "Southern";
			wdw.Speaker = "Edna";
			wdw.Transcriber = "Burt";
			
			wdw.AddNonSegmentInfo(0, 100, "gloss1", "ref1", "pos1");
			wdw.AddPhoneticSegment(0, 5, "a");
			
			wdw.AddPhoneticSegment(75, 15, "b");
			wdw.AddPhoneticSegment(400, 25, "c");

			wdw.AddNonSegmentInfo(555, 200, "gloss2", null, "pos2");
			wdw.AddPhoneticSegment(555, 55, "xx");
			wdw.AddPhonemicSegment(555, 55, "dd");

			wdw.AddPhoneticSegment(606, 65, "yyy");
			wdw.AddOrthographicSegment(606, 65, "kk");

			wdw.AddPhoneticSegment(777, 75, "zzzz");

			wdw.AddNonSegmentInfo(888, 300, "gloss3", "ref3", null);
			wdw.AddPhoneticSegment(888, 85, "gg");
			wdw.AddPhonemicSegment(888, 85, "mm");
			wdw.AddOrthographicSegment(888, 85, "oooo");

			wdw.AddPhoneticSegment(999, 95, "s");
			wdw.AddToneSegment(999, 95, "t");
			wdw.AddPhonemicSegment(999, 95, "nnn");
		}
	}
}
