// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

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
		private PhoneCache _cache;

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
			_cache = new PhoneCache(_prj);
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
			Assert.AreEqual(0, _cache.Count);
			_cache.AddPhone("d");
			Assert.IsNotNull(_cache["d"]);
			Assert.AreEqual(1, _cache.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddUndefinedPhone method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddUndefinedPhone()
		{
			Assert.AreEqual(0, _cache.Count);
			_cache.AddUndefinedPhone("d");
			Assert.IsTrue(((PhoneInfo)_cache["d"]).IsUndefined);
			
			_cache.Clear();
			Assert.AreEqual(0, _cache.Count);
			_cache.AddPhone("d");
			Assert.IsFalse(((PhoneInfo)_cache["d"]).IsUndefined);
			Assert.AreEqual(1, _cache.Count);

			_cache.AddUndefinedPhone("d");
			Assert.IsTrue(((PhoneInfo)_cache["d"]).IsUndefined);
			Assert.AreEqual(1, _cache.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPhonesHavingType_AskForConsonants_ReturnsOnlyConsonants()
		{
			_cache.AddPhone("a");
			_cache.AddPhone("e");
			_cache.AddPhone("i");
			_cache.AddPhone("d");
			_cache.AddPhone("z");
			Assert.AreEqual(5, _cache.Count);

			var phones = _cache.GetPhonesHavingType(IPASymbolType.consonant);
			Assert.AreEqual(2, phones.Length);
			Assert.AreEqual("d", phones[0]);
			Assert.AreEqual("z", phones[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPhonesHavingType_AskForVowels_ReturnsOnlyVowels()
		{
			_cache.AddPhone("a");
			_cache.AddPhone("e");
			_cache.AddPhone("i");
			_cache.AddPhone("d");
			_cache.AddPhone("z");
			Assert.AreEqual(5, _cache.Count);

			var phones = _cache.GetPhonesHavingType(IPASymbolType.vowel);
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
			_cache.AddPhone("a");
			_cache.AddPhone("d");
			_cache.AddPhone("i");
			_cache.AddPhone("\u02E6");
			Assert.AreEqual(4, _cache.Count);

			var phones = _cache.GetPhonesHavingType(IPASymbolType.suprasegmental);

			Assert.AreEqual(1, phones.Length);
			Assert.AreEqual("\u02E6", phones[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCommaDelimitedPhones_AskForConsonants_ReturnsStringOfAskForConsonants()
		{
			_cache.AddPhone("a");
			_cache.AddPhone("e");
			_cache.AddPhone("i");
			_cache.AddPhone("d");
			_cache.AddPhone("z");
			Assert.AreEqual(5, _cache.Count);
			Assert.AreEqual("d,z", _cache.GetCommaDelimitedPhones(IPASymbolType.consonant));
		}
		
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCommaDelimitedPhones_AskForVowels_ReturnsStringOfVowels()
		{
			_cache.AddPhone("a");
			_cache.AddPhone("e");
			_cache.AddPhone("i");
			_cache.AddPhone("d");
			_cache.AddPhone("z");
			Assert.AreEqual(5, _cache.Count);
			Assert.AreEqual("a,e,i", _cache.GetCommaDelimitedPhones(IPASymbolType.vowel));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneWithoutIgnoredStuff_PassNoStuffToIgnore_ReturnsInput()
		{
			Assert.AreEqual("a01", _cache.PhoneWithoutIgnoredStuff("a01", string.Empty, false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneWithoutIgnoredStuff_PassCharsToIgnore_ReturnsInput()
		{
			Assert.AreEqual("a23", _cache.PhoneWithoutIgnoredStuff("a23", "01", false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneWithoutIgnoredStuff_PassCharsToIgnore_ReturnsBaseChar()
		{
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a01", "01", false));
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a0", "01", false));
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a1", "01", false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneWithoutIgnoredStuff_PassSomeCharsToIgnore_ReturnsPhoneWithoutIgnoredChars()
		{
			Assert.AreEqual("a1", _cache.PhoneWithoutIgnoredStuff("a01", "0", false));
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a0", "0", false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneWithoutIgnoredStuff_PassIgnoredDiacritics_ReturnsPhoneWithoutIgnoredDiacritics()
		{
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a\u0320", string.Empty, true));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneWithoutIgnoredStuff_PassIgnoredDiacriticsAndChar_ReturnsBasePhone()
		{
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a01\u0320", "01", true));
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a1\u0320", "1", true));
			Assert.AreEqual("a", _cache.PhoneWithoutIgnoredStuff("a0\u0320", "0", true));
		}

		/// ------------------------------------------------------------------------------------
		private void AddCachePhonesForGetDoesContainPhoneTests()
		{
			_cache.AddPhone("a01");
			_cache.AddPhone("a1");
			_cache.AddPhone("a0");
			_cache.AddPhone("a\u0320");
			_cache.AddPhone("a01\u0320");
			_cache.AddPhone("a0\u0320");
			_cache.AddPhone("a1\u0320");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesContainPhone_NoMatchInCache_ReturnsFalse()
		{
			Assert.IsFalse(_cache.GetDoesContainPhone("a01\u0320", "01", true));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesContainPhone_PassIgnoredDiacritics_ReturnsTrue()
		{
			AddCachePhonesForGetDoesContainPhoneTests();
			Assert.IsTrue(_cache.GetDoesContainPhone("a01\u0320", string.Empty, true));
			Assert.IsTrue(_cache.GetDoesContainPhone("a0\u0320", string.Empty, true));
			Assert.IsTrue(_cache.GetDoesContainPhone("a1\u0320", string.Empty, true));
			Assert.IsTrue(_cache.GetDoesContainPhone("a\u0320", string.Empty, true));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesContainPhone_PassIgnoredChars_ReturnsTrue()
		{
			AddCachePhonesForGetDoesContainPhoneTests();
			Assert.IsTrue(_cache.GetDoesContainPhone("a01\u0320", "1", false));
			Assert.IsTrue(_cache.GetDoesContainPhone("a\u0320", "01", false));
			Assert.IsTrue(_cache.GetDoesContainPhone("a01\u0320", "10", false));
			Assert.IsTrue(_cache.GetDoesContainPhone("a0\u0320", "0", false));
			Assert.IsTrue(_cache.GetDoesContainPhone("a1\u0320", "1", false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesContainPhone_PassIgnoredDiacriticsAndChars_ReturnsTrue()
		{
			AddCachePhonesForGetDoesContainPhoneTests();
			Assert.IsTrue(_cache.GetDoesContainPhone("a01\u0320", "01", true));
			Assert.IsTrue(_cache.GetDoesContainPhone("a1\u0320", "1", true));
			Assert.IsTrue(_cache.GetDoesContainPhone("a0\u0320", "0", true));
		}
	}
}