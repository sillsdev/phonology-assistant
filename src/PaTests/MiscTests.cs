using NUnit.Framework;
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class MiscTests : TestBase
	{
		private RecordCacheEntry m_recEntry;
		private PaDataSource _dataSource;

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

			_dataSource = new PaDataSource();
			_dataSource.Type = DataSourceType.Toolbox;
			_dataSource.FieldMappings = new System.Collections.Generic.List<FieldMapping>();
			_dataSource.FieldMappings.Add(new FieldMapping("\\ph", _prj.GetPhoneticField(), true));
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
        public void TestSetup()
        {
			App.IPASymbolCache.UndefinedCharacters.Clear();
			_prj.PhoneticParser.LogUndefinedCharactersWhenParsing = true;
			m_recEntry = new RecordCacheEntry(_prj);
			m_recEntry.DataSource = new PaDataSource();
			m_recEntry.NeedsParsing = true;
			m_recEntry.DataSource = _dataSource;
			_prj.RecordCache.PhoneCache.Clear();
			_prj.RecordCache.Clear();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private PhoneCache AddWordsToRecEntry(string words)
		{
			m_recEntry.SetValue("Phonetic", words);
			_prj.RecordCache.Add(m_recEntry);
			_prj.RecordCache.BuildWordCache(null);

			return _prj.RecordCache.PhoneCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Simple phone cache building test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheTest1()
		{
			var phoneCache = AddWordsToRecEntry("abc xyz");
			Assert.AreEqual(6, phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["c"]);
			Assert.IsNotNull(phoneCache["x"]);
			Assert.IsNotNull(phoneCache["y"]);
			Assert.IsNotNull(phoneCache["z"]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache repeated phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheTest2()
		{
			var phoneCache = AddWordsToRecEntry("abc axc");
			Assert.AreEqual(4, phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["c"]);
			Assert.IsNotNull(phoneCache["x"]);

			Assert.AreEqual(2, phoneCache["a"].TotalCount);
			Assert.AreEqual(1, phoneCache["b"].TotalCount);
			Assert.AreEqual(2, phoneCache["c"].TotalCount);
			Assert.AreEqual(1, phoneCache["x"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache when phonetic words contain word breaks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithWordBreaksInPhonetic()
		{
			var phoneCache = AddWordsToRecEntry("ab" + App.BreakChars[0] + "xy");

			Assert.AreEqual(4,phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["x"]);
			Assert.IsNotNull(phoneCache["y"]);
			Assert.IsNull(phoneCache[App.BreakChars[0].ToString()]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test building phone cache when phonetic words contain word breaks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUndefinedChars()
		{
			_prj.PhoneticParser.LogUndefinedCharactersWhenParsing = true;
			var phoneCache = AddWordsToRecEntry("abXY");

			Assert.AreEqual(2, App.IPASymbolCache.UndefinedCharacters.Count);
			Assert.AreEqual('X', App.IPASymbolCache.UndefinedCharacters[0].Character);
			Assert.AreEqual('Y', App.IPASymbolCache.UndefinedCharacters[1].Character);
			Assert.IsTrue(App.IPASymbolCache["X"].IsUndefined);
			Assert.IsTrue(App.IPASymbolCache["Y"].IsUndefined);

			Assert.AreEqual(4, phoneCache.Count);
			Assert.IsFalse((phoneCache["a"] as PhoneInfo).IsUndefined);
			Assert.IsFalse((phoneCache["b"] as PhoneInfo).IsUndefined);
			Assert.IsTrue((phoneCache["X"] as PhoneInfo).IsUndefined);
			Assert.IsTrue((phoneCache["Y"] as PhoneInfo).IsUndefined);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Simple test for building phone cache with uncertain phone group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertainties1()
		{
			var phoneCache = AddWordsToRecEntry("ab(t/d)c");

			Assert.AreEqual(5, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["d"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].TotalCount);
			Assert.AreEqual(0, phoneCache["d"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Simple test for building phone cache with uncertain phone group, testing that each
		/// uncertainty's sibling(s) is/are correct.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertainties2()
		{
			var phoneCache = AddWordsToRecEntry("ab(t/d)c(e/i/o)");

			Assert.AreEqual(8, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["e"]);
			Assert.IsNotNull(phoneCache["i"]);
			Assert.IsNotNull(phoneCache["o"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["e"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["o"].CountAsNonPrimaryUncertainty);

			Assert.AreEqual(1, phoneCache["t"].SiblingUncertainties.Count);
			Assert.AreEqual(1, phoneCache["d"].SiblingUncertainties.Count);
			Assert.AreEqual(2, phoneCache["e"].SiblingUncertainties.Count);
			Assert.AreEqual(2, phoneCache["i"].SiblingUncertainties.Count);
			Assert.AreEqual(2, phoneCache["o"].SiblingUncertainties.Count);

			Assert.AreEqual("d", phoneCache["t"].SiblingUncertainties[0]);
			Assert.AreEqual("t", phoneCache["d"].SiblingUncertainties[0]);

			Assert.AreEqual("i", phoneCache["e"].SiblingUncertainties[0]);
			Assert.AreEqual("o", phoneCache["e"].SiblingUncertainties[1]);
			Assert.AreEqual("e", phoneCache["i"].SiblingUncertainties[0]);
			Assert.AreEqual("o", phoneCache["i"].SiblingUncertainties[1]);
			Assert.AreEqual("e", phoneCache["o"].SiblingUncertainties[0]);
			Assert.AreEqual("i", phoneCache["o"].SiblingUncertainties[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with uncertain phone group, when there the
		/// uncertainties indicate the absence or presence of a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithEmptySetInUncertaintyGroup1()
		{
			VerifyUncertaintyGroup("ab(t/)c");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with uncertain phone group, when there the
		/// uncertainties indicate the absence or presence of a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithEmptySetInUncertaintyGroup2()
		{
			VerifyUncertaintyGroup("ab(t/0)c");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with uncertain phone group, when there the
		/// uncertainties indicate the absence or presence of a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithEmptySetInUncertaintyGroup3()
		{
			VerifyUncertaintyGroup(string.Format("ab(t/{0})c", IPASymbolCache.UncertainGroupAbsentPhoneChars));
		}

		/// ------------------------------------------------------------------------------------
		private void VerifyUncertaintyGroup(string phontic)
		{
			var phoneCache = AddWordsToRecEntry(phontic);

			Assert.AreEqual(4, phoneCache.Count);
			Assert.IsNotNull(phoneCache["a"]);
			Assert.IsNotNull(phoneCache["b"]);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["c"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].CountAsNonPrimaryUncertainty);

			Assert.AreEqual(1, phoneCache["t"].SiblingUncertainties.Count);
			Assert.AreEqual(IPASymbolCache.UncertainGroupAbsentPhoneChar,
				phoneCache["t"].SiblingUncertainties[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with 2 uncertain phone groups in a word. (This test
		/// will test that PA-710 is fixed).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertaintiesWithMultiGroups1()
		{
			var phoneCache = AddWordsToRecEntry("ab(t/d)cxy(u/i)z");

			Assert.AreEqual(10, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["u"]);
			Assert.IsNotNull(phoneCache["i"]);

			Assert.AreEqual(1, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].TotalCount);
			Assert.AreEqual(0, phoneCache["d"].TotalCount);
			Assert.AreEqual(1, phoneCache["u"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["u"].TotalCount);
			Assert.AreEqual(0, phoneCache["i"].TotalCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Test for building phone cache with 3 uncertain phone groups in a word. (This test
		/// will test that PA-710 is fixed).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildPhoneCacheWithUncertaintiesWithMultiGroups2()
		{
			var phoneCache = AddWordsToRecEntry("ab(t/d)cxy(u/i)z(t/d)mn");

			Assert.AreEqual(12, phoneCache.Count);
			Assert.IsNotNull(phoneCache["t"]);
			Assert.IsNotNull(phoneCache["u"]);
			Assert.IsNotNull(phoneCache["i"]);

			Assert.AreEqual(2, phoneCache["t"].CountAsPrimaryUncertainty);
			Assert.AreEqual(2, phoneCache["d"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["t"].TotalCount);
			Assert.AreEqual(0, phoneCache["d"].TotalCount);
			Assert.AreEqual(1, phoneCache["u"].CountAsPrimaryUncertainty);
			Assert.AreEqual(1, phoneCache["i"].CountAsNonPrimaryUncertainty);
			Assert.AreEqual(0, phoneCache["u"].TotalCount);
			Assert.AreEqual(0, phoneCache["i"].TotalCount);
		}
	}
}