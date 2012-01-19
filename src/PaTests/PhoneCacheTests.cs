using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;
using SilTools;

namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in PhoneCache
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class PhoneCacheTests : TestBase
	{
		private PhoneCache m_cache;

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
			InventoryHelper.Load();
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_cache = new PhoneCache(_prj);
			_prj.CVPatternInfoList = new List<CVPatternInfo>();
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
        public void TestTearDown()
        {
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddPhone method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddPhone()
		{
			Assert.AreEqual(0, m_cache.Count);
			m_cache.AddPhone("d");
			Assert.IsNotNull(m_cache["d"]);
			Assert.AreEqual(1, m_cache.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddUndefinedPhone method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddUndefinedPhone()
		{
			Assert.AreEqual(0, m_cache.Count);
			m_cache.AddUndefinedPhone("d");
			Assert.IsTrue(((PhoneInfo)m_cache["d"]).IsUndefined);
			
			m_cache.Clear();
			Assert.AreEqual(0, m_cache.Count);
			m_cache.AddPhone("d");
			Assert.IsFalse(((PhoneInfo)m_cache["d"]).IsUndefined);
			Assert.AreEqual(1, m_cache.Count);

			m_cache.AddUndefinedPhone("d");
			Assert.IsTrue(((PhoneInfo)m_cache["d"]).IsUndefined);
			Assert.AreEqual(1, m_cache.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPhonesHavingType_AskForConsonants_ReturnsOnlyConsonants()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("e");
			m_cache.AddPhone("i");
			m_cache.AddPhone("d");
			m_cache.AddPhone("z");
			Assert.AreEqual(5, m_cache.Count);

			var phones = m_cache.GetPhonesHavingType(IPASymbolType.consonant);
			Assert.AreEqual(2, phones.Length);
			Assert.AreEqual("d", phones[0]);
			Assert.AreEqual("z", phones[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPhonesHavingType_AskForVowels_ReturnsOnlyVowels()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("e");
			m_cache.AddPhone("i");
			m_cache.AddPhone("d");
			m_cache.AddPhone("z");
			Assert.AreEqual(5, m_cache.Count);

			var phones = m_cache.GetPhonesHavingType(IPASymbolType.vowel);
			Assert.AreEqual(3, phones.Length);
			Assert.AreEqual("a", phones[0]);
			Assert.AreEqual("e", phones[1]);
			Assert.AreEqual("i", phones[2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetTypeOfPhones method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPhonesHavingType_AskForSsegs_ReturnsOnlySsegs()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("d");
			m_cache.AddPhone("i");
			m_cache.AddPhone("\u02E6");
			Assert.AreEqual(4, m_cache.Count);

			var phones = m_cache.GetPhonesHavingType(IPASymbolType.suprasegmental);

			Assert.AreEqual(1, phones.Length);
			Assert.AreEqual("\u02E6", phones[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCommaDelimitedPhones_AskForConsonants_ReturnsStringOfAskForConsonants()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("e");
			m_cache.AddPhone("i");
			m_cache.AddPhone("d");
			m_cache.AddPhone("z");
			Assert.AreEqual(5, m_cache.Count);
			Assert.AreEqual("d,z", m_cache.GetCommaDelimitedPhones(IPASymbolType.consonant));
		}
		
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCommaDelimitedPhones_AskForVowels_ReturnsStringOfVowels()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("e");
			m_cache.AddPhone("i");
			m_cache.AddPhone("d");
			m_cache.AddPhone("z");
			Assert.AreEqual(5, m_cache.Count);
			Assert.AreEqual("a,e,i", m_cache.GetCommaDelimitedPhones(IPASymbolType.vowel));
		}
	}
}