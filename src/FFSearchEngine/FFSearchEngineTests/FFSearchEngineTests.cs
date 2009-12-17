using NUnit.Framework;
using SIL.Pa.Data;
using SIL.Pa.TestUtils;

namespace SIL.Pa.FFSearchEngine
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FFSearchEngineTests : TestBase
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
			// This will force the cache to be built.
			if (DataUtils.IPASymbolCache == null)
			{
			}
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
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhonesInPatternTest()
		{
			SearchQuery query = new SearchQuery();
			query.Pattern = "ab{o,e}/[C]_[+Rounded]xyz";
			SearchEngine engine = new SearchEngine(query);

			string[] phones = engine.PhonesInPattern;
			Assert.AreEqual(7, phones.Length);
			Assert.AreEqual("a", phones[0]);
			Assert.AreEqual("b", phones[1]);
			Assert.AreEqual("o", phones[2]);
			Assert.AreEqual("e", phones[3]);
			Assert.AreEqual("x", phones[4]);
			Assert.AreEqual("y", phones[5]);
			Assert.AreEqual("z", phones[6]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ParsePhoneTest()
		{
			string resultPhone;
			string resultDiacritics;

			string dental = "\u032A";
			string dentalT = "t" + dental;
			string dentalS = "s" + dental;

			string phone = dentalT + DataUtils.kTopTieBar + dentalS;
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t" + DataUtils.kTopTieBar + "s");
			Assert.AreEqual(resultDiacritics, dental);

			phone = "t" + DataUtils.kTopTieBar + dentalS;
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t" + DataUtils.kTopTieBar + "s");
			Assert.AreEqual(resultDiacritics, dental);

			phone = dentalT + DataUtils.kBottomTieBar + "s";
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t" + DataUtils.kBottomTieBar + "s");
			Assert.AreEqual(resultDiacritics, dental);

			phone = dentalT;
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t");
			Assert.AreEqual(resultDiacritics, dental);

			phone = dentalT + "\u02B0";
			SearchEngine.ParsePhone(phone, out resultPhone, out	resultDiacritics);
			Assert.AreEqual(resultPhone, "t");
			Assert.AreEqual(resultDiacritics, dental + "\u02B0");
		}
	}
}
