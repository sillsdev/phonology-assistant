using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests the PhoneticWriter class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PhoneticWriterTests : TestBase
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
		/// Tests the Write method. Most of what goes on when the Write method is called is
		/// tested in the other tests in this fixture.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTest()
		{
			string sqlEticLstCount = "SELECT Count(PhoneticListID) AS cnt FROM PhoneticList";
			string sqlCharIdxCount = "SELECT Count(CharIndexID) AS cnt FROM CharIndex";
			string sqlCharLstCount = "SELECT Count(CharListID) AS cnt FROM CharList";

			PhoneticWriter writer = new PhoneticWriter();
			int id1 = writer.Write("dad");

			// Verify one record in the PhoneticList table.
			Assert.AreEqual(1, (int)DBUtils.GetScalerValueFromSQL(sqlEticLstCount));

			// Verify there are 3 records in the CharIndex table: one for each letter in the word.
			Assert.AreEqual(3, (int)DBUtils.GetScalerValueFromSQL(sqlCharIdxCount));

			// Verify there are 2 records in the CharList table: one for 'd ' and one for 'a'.
			Assert.AreEqual(2, (int)DBUtils.GetScalerValueFromSQL(sqlCharLstCount));

			// Verify we get the same id back when writing another copy of the same word.
			int id2 = writer.Write("dad");
			Assert.AreEqual(id1, id2);

			// Should still only be one record for the word "dad"
			Assert.AreEqual(1, (int)DBUtils.GetScalerValueFromSQL(sqlEticLstCount));

			// Should still only have 3 records in the CharIndex.
			Assert.AreEqual(3, (int)DBUtils.GetScalerValueFromSQL(sqlCharIdxCount));

			// Verify there are still only 2 records in the CharList table.
			Assert.AreEqual(2, (int)DBUtils.GetScalerValueFromSQL(sqlCharLstCount));

			// Now write a different word having the same letters.
			id1 = writer.Write("da");

			// Should have records for "dad" and "da"
			Assert.AreEqual(2, (int)DBUtils.GetScalerValueFromSQL(sqlEticLstCount));

			// Should have records for links to 5 phones.
			Assert.AreEqual(5, (int)DBUtils.GetScalerValueFromSQL(sqlCharIdxCount));

			// Should still only have records for 'd' and 'a'
			Assert.AreEqual(2, (int)DBUtils.GetScalerValueFromSQL(sqlCharLstCount));		
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the ParseWord method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ParseWordTest()
		{
			// \u0303 = tilde (Nasalized), \u0308 = umlaut (Centralized),
			// \u0301 = acute accent (hight tone), and \u0302 = circumflex (tone falling).
			string word = "pue\u0303do\u0308\u0301a\u0302";

			PhoneticWordInfo wordInfo =
				GetResult(typeof(PhoneticWriter), "ParseWord", word) as PhoneticWordInfo;

			Assert.AreEqual("0070 0075 0065 0064 006F 0061", wordInfo.BaseHexPhonetic);
			Assert.AreEqual("CVVCVV", wordInfo.CVPattern);
			Assert.AreEqual(6, wordInfo.Phones.Length);

			Assert.AreEqual("p", wordInfo.Phones[0].Phone);
			Assert.AreEqual(0, wordInfo.Phones[0].Order);
			Assert.AreEqual(0, wordInfo.Phones[0].Offset);
			Assert.AreEqual(RelativePhoneLocation.Initial, wordInfo.Phones[0].Location);
			Assert.AreEqual(IPACharacterType.Consonant, wordInfo.Phones[0].BaseCharType);

			Assert.AreEqual("u", wordInfo.Phones[1].Phone);
			Assert.AreEqual(1, wordInfo.Phones[1].Order);
			Assert.AreEqual(1, wordInfo.Phones[1].Offset);
			Assert.AreEqual(RelativePhoneLocation.Medial, wordInfo.Phones[1].Location);
			Assert.AreEqual(IPACharacterType.Vowel, wordInfo.Phones[1].BaseCharType);

			Assert.AreEqual("e\u0303", wordInfo.Phones[2].Phone);
			Assert.AreEqual(2, wordInfo.Phones[2].Order);
			Assert.AreEqual(2, wordInfo.Phones[2].Offset);
			Assert.AreEqual(RelativePhoneLocation.Medial, wordInfo.Phones[2].Location);
			Assert.AreEqual(IPACharacterType.Vowel, wordInfo.Phones[2].BaseCharType);

			Assert.AreEqual("d", wordInfo.Phones[3].Phone);
			Assert.AreEqual(3, wordInfo.Phones[3].Order);
			Assert.AreEqual(4, wordInfo.Phones[3].Offset);
			Assert.AreEqual(RelativePhoneLocation.Medial, wordInfo.Phones[3].Location);
			Assert.AreEqual(IPACharacterType.Consonant, wordInfo.Phones[3].BaseCharType);

			Assert.AreEqual("o\u0308\u0301", wordInfo.Phones[4].Phone);
			Assert.AreEqual(4, wordInfo.Phones[4].Order);
			Assert.AreEqual(5, wordInfo.Phones[4].Offset);
			Assert.AreEqual(RelativePhoneLocation.Medial, wordInfo.Phones[4].Location);
			Assert.AreEqual(IPACharacterType.Vowel, wordInfo.Phones[4].BaseCharType);

			Assert.AreEqual("a\u0302", wordInfo.Phones[5].Phone);
			Assert.AreEqual(5, wordInfo.Phones[5].Order);
			Assert.AreEqual(8, wordInfo.Phones[5].Offset);
			Assert.AreEqual(RelativePhoneLocation.Final, wordInfo.Phones[5].Location);
			Assert.AreEqual(IPACharacterType.Vowel, wordInfo.Phones[5].BaseCharType);

			// Now make sure the location for a single character comes back as alone.
			wordInfo = GetResult(typeof(PhoneticWriter), "ParseWord", "p") as PhoneticWordInfo;
			Assert.AreEqual(1, wordInfo.Phones.Length);
			Assert.AreEqual("p", wordInfo.Phones[0].Phone);
			Assert.AreEqual(RelativePhoneLocation.Alone, wordInfo.Phones[0].Location);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the ProcessPhoneInfo method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ProcessPhoneInfoTest()
		{
			int c = Convert.ToInt32('a');
			DBUtils.IPACharCache[c].BinaryMask = 1;
			DBUtils.IPACharCache[c].Mask0 = 1;
			DBUtils.IPACharCache[c].Mask1 = 1;
			DBUtils.IPACharCache[c].MOArticulation = 0x123;
			DBUtils.IPACharCache[c].POArticulation = 0x456;

			c = Convert.ToInt32('b');
			DBUtils.IPACharCache[c].BinaryMask = 2;
			DBUtils.IPACharCache[c].Mask0 = 2;
			DBUtils.IPACharCache[c].Mask1 = 2;
			DBUtils.IPACharCache[c].MOArticulation = 0x789;
			DBUtils.IPACharCache[c].POArticulation = 0xABC;

			c = Convert.ToInt32('c');
			DBUtils.IPACharCache[c].BinaryMask = 4;
			DBUtils.IPACharCache[c].Mask0 = 4;
			DBUtils.IPACharCache[c].Mask1 = 4;
			DBUtils.IPACharCache[c].MOArticulation = 0xDEF;
			DBUtils.IPACharCache[c].POArticulation = 0x555;

			PhoneticWordInfo eticWordInfo = new PhoneticWordInfo();
			eticWordInfo.Phones = new PhoneInfo[] {new PhoneInfo("a"), new PhoneInfo("b"), new PhoneInfo("ca")};

			CallMethod(typeof(PhoneticWriter), "ProcessPhoneInfo", eticWordInfo);

			// Verify MOA and POA for word
			Assert.AreEqual(DBUtils.StrToHex("abc"), eticWordInfo.BaseHexPhonetic);
			Assert.AreEqual("VCC", eticWordInfo.CVPattern);

			// Verify MOA and POA for each phone
			Assert.AreEqual("123", eticWordInfo.Phones[0].MOArticulation);
			Assert.AreEqual("456", eticWordInfo.Phones[0].POArticulation);
			Assert.AreEqual("789", eticWordInfo.Phones[1].MOArticulation);
			Assert.AreEqual("ABC", eticWordInfo.Phones[1].POArticulation);
			Assert.AreEqual("DEF123", eticWordInfo.Phones[2].MOArticulation);
			Assert.AreEqual("555456", eticWordInfo.Phones[2].POArticulation);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CommitWord method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CommitWord()
		{
			PhoneticWordInfo wordInfo = new PhoneticWordInfo();
			wordInfo.BaseHexPhonetic = "basehexphoneticstring";
			wordInfo.HexPhonetic = "fullhexphoneticstring";
			wordInfo.CVPattern = "cvpatternformword";

			PhoneticWriter writer = new PhoneticWriter();
			int id = GetIntResult(writer, "CommitWord", new object[] {"dogbreath", wordInfo});

			Assert.IsTrue(id > 0);

			string sql = DBUtils.SelectSQL("PhoneticList", DBFields.PhoneticListId, id);
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				reader.Read();
				Assert.AreEqual(wordInfo.BaseHexPhonetic, (string)reader["BaseHexPhonetic"]);
				Assert.AreEqual(wordInfo.HexPhonetic, (string)reader["HexPhonetic"]);
				Assert.AreEqual(wordInfo.CVPattern, (string)reader["CVPattern"]);
				Assert.AreEqual("dogbreath", (string)reader[DBFields.Phonetic]);
				reader.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CommitPhones method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CommitPhonesTest()
		{
			// First add a record to the phonetic list table so we have a phonetic list id.
			PhoneticWordInfo eticWordInfo = new PhoneticWordInfo();
			PhoneticWriter writer = new PhoneticWriter();

			// Create dummy phone data for the word "abc"
			eticWordInfo.Phones = SetupPhoneData();
			CallMethod(writer, "CommitPhones", eticWordInfo);
			Assert.AreEqual("MOA0MOA1MOA2", eticWordInfo.MOASortKey);
			Assert.AreEqual("POA0POA1POA2", eticWordInfo.POASortKey);
			Assert.AreEqual("000000000", eticWordInfo.UDSortKey);

			PostCommitPhonesVerify(eticWordInfo);

			eticWordInfo.MOASortKey = null;
			eticWordInfo.POASortKey = null;
			eticWordInfo.UDSortKey = null;

			// Now add the same phones to the database again.
			CallMethod(writer, "CommitPhones", eticWordInfo);

			// Verify there are still only 3 CharList records.
			string sql = "SELECT Count(CharListID) AS cnt FROM CharList";
			Assert.AreEqual(3, (int)DBUtils.GetScalerValueFromSQL(sql));
			Assert.AreEqual("MOA0MOA1MOA2", eticWordInfo.MOASortKey);
			Assert.AreEqual("POA0POA1POA2", eticWordInfo.POASortKey);
			Assert.AreEqual("000000000", eticWordInfo.UDSortKey);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets up some dummy phone data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static PhoneInfo[] SetupPhoneData()
		{
			PhoneInfo[] phones = new PhoneInfo[3];

			phones[0] = new PhoneInfo("a");
			phones[0].BaseCharType = IPACharacterType.Vowel;
			phones[0].IPACharacterId = 30;
			phones[0].MOArticulation = "MOA0";
			phones[0].POArticulation = "POA0";
			phones[0].UDSortOrder = "UD0";
			phones[0].BinaryMask = 0x1122;
			phones[0].Mask0 = 111;
			phones[0].Mask1 = 222;
			phones[0].Order = 1;
			phones[0].Offset = 1;
			phones[0].Location = RelativePhoneLocation.Initial;

			phones[1] = new PhoneInfo("b");
			phones[1].BaseCharType = IPACharacterType.Consonant;
			phones[1].IPACharacterId = 30;
			phones[1].MOArticulation = "MOA1";
			phones[1].POArticulation = "POA1";
			phones[0].UDSortOrder = "UD1";
			phones[1].BinaryMask = 0xABC;
			phones[1].Mask0 = 1111;
			phones[1].Mask1 = 2222;
			phones[1].Order = 2;
			phones[1].Offset = 2;
			phones[1].Location = RelativePhoneLocation.Medial;

			phones[2] = new PhoneInfo("c");
			phones[2].BaseCharType = IPACharacterType.Consonant;
			phones[2].IPACharacterId = 30;
			phones[2].MOArticulation = "MOA2";
			phones[2].POArticulation = "POA2";
			phones[0].UDSortOrder = "UD2";
			phones[2].BinaryMask = 1;
			phones[2].Mask0 = 1;
			phones[2].Mask1 = 2;
			phones[2].Order = 3;
			phones[2].Offset = 3;
			phones[2].Location = RelativePhoneLocation.Final;

			return phones;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifys the data in the DB after calling CommitPhones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void PostCommitPhonesVerify(PhoneticWordInfo eticWordInfo)
		{
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB("CharList"))
			{
				for (int i = 0; i < 3; i++)
				{
					bool read = reader.Read();
					Assert.AreEqual(eticWordInfo.Phones[i].Phone, (string)reader["CharStr"]);
					Assert.AreEqual(eticWordInfo.Phones[i].MOArticulation, (string)reader["MOArticulation"]);
					Assert.AreEqual(eticWordInfo.Phones[i].POArticulation, (string)reader["POArticulation"]);
					Assert.AreEqual(eticWordInfo.Phones[i].BinaryMask, (ulong)(double)reader["BinaryMask"]);
					Assert.AreEqual(eticWordInfo.Phones[i].Mask0, (ulong)(double)reader["Mask0"]);
					Assert.AreEqual(eticWordInfo.Phones[i].Mask1, (ulong)(double)reader["Mask1"]);
					Assert.AreEqual((int)eticWordInfo.Phones[i].BaseCharType, (int)reader["BaseCharType"]);
					Assert.AreEqual(DBUtils.StrToHex(eticWordInfo.Phones[i].Phone), (string)reader["HexCharStr"]);
				}

				reader.Close();
			}
		}
	}
}
