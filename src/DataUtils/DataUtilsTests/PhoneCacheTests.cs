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
// File: PhoneCacheTests.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;

namespace SIL.Pa.Data
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
		public void FixtureSetup()
		{
			DataUtils.LoadIPACharCache(null);
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_cache = new PhoneCache();
			PhoneCache.CVPatternInfoList = new List<CVPatternInfo>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
        public void TestTearDown()
        {
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCVPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CVPatternTest_Simple()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			Assert.AreEqual("VCVC", m_cache.GetCVPattern("abec"));
			Assert.AreEqual("CCVV", m_cache.GetCVPattern("bcea"));
			Assert.AreEqual("VVCC", m_cache.GetCVPattern("eabc"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCVPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CVPatternTest_IncludeExplicitPhonesWithoutDiacritics()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("c", IPACharIgnoreTypes.NotApplicable));
			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("e", IPACharIgnoreTypes.NotApplicable));

			Assert.AreEqual("VCec", m_cache.GetCVPattern("abec"));
			Assert.AreEqual("CceV", m_cache.GetCVPattern("bcea"));
			Assert.AreEqual("eVCc", m_cache.GetCVPattern("eabc"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCVPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CVPatternTest_IncludeExplicitPhonesWithDiacritics()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("a\u0303");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");
			m_cache.AddPhone("e\u0301");

			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("a", IPACharIgnoreTypes.NotApplicable));
			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("e\u0301", IPACharIgnoreTypes.NotApplicable));

			Assert.AreEqual("aCe\u0301C", m_cache.GetCVPattern("abe\u0301c"));
			Assert.AreEqual("CCVV", m_cache.GetCVPattern("bcea\u0303"));
			Assert.AreEqual("e\u0301aCC", m_cache.GetCVPattern("e\u0301abc"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCVPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CVPatternTest_IncludeExplicitDiacriticsAfter()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("a\u0303");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");
			m_cache.AddPhone("e\u0301");

			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("a",
				IPACharIgnoreTypes.NotApplicable));
			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("\u0301",
				IPACharIgnoreTypes.Tone));
			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create(DataUtils.kDottedCircle + "\u0303",
				IPACharIgnoreTypes.NotApplicable));

			Assert.AreEqual("aCVC", m_cache.GetCVPattern("abec"));
			Assert.AreEqual("CCV\u0301a", m_cache.GetCVPattern("bce\u0301a"));
			Assert.AreEqual("aV\u0301CV\u0303C", m_cache.GetCVPattern("ae\u0301ba\u0303c"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCVPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CVPatternTest_IncludeExplicitDiacriticsBefore()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("b");
			m_cache.AddPhone("\u207Fb");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");
			AmbiguousSequences ambigSeqs = new AmbiguousSequences();
			ambigSeqs.Add("\u207Fb");
			DataUtils.IPACharCache.AmbiguousSequences = ambigSeqs;			

			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("a",
				IPACharIgnoreTypes.NotApplicable));
			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("\u207F" + DataUtils.kDottedCircle,
				IPACharIgnoreTypes.NotApplicable));

			Assert.AreEqual("a\u207FCVC", m_cache.GetCVPattern("a\u207Fbec"));
			Assert.AreEqual("\u207FCCVa", m_cache.GetCVPattern("\u207Fbcea"));
			Assert.AreEqual("aV\u207FCC\u207FC", m_cache.GetCVPattern("ae\u207Fbc\u207Fb"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCVPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CVPatternTest_IncludeExplicitDiacriticsOnBothSides()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("a\u0303");
			m_cache.AddPhone("b");
			m_cache.AddPhone("\u207Fb");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			AmbiguousSequences ambigSeqs = new AmbiguousSequences();
			ambigSeqs.Add("\u207Fb");
			DataUtils.IPACharCache.AmbiguousSequences = ambigSeqs;

			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create(DataUtils.kDottedCircle + "\u0303",
				IPACharIgnoreTypes.NotApplicable));

			PhoneCache.CVPatternInfoList.Add(CVPatternInfo.Create("\u207F" + DataUtils.kDottedCircle,
				IPACharIgnoreTypes.NotApplicable));

			Assert.AreEqual("VV\u0303\u207FCC", m_cache.GetCVPattern("ea\u0303\u207Fbc"));
			Assert.AreEqual("\u207FCCVV", m_cache.GetCVPattern("\u207Fbcea"));
			Assert.AreEqual("CV\u0303CV", m_cache.GetCVPattern("ba\u0303ce"));
			Assert.AreEqual("VCV\u207FC", m_cache.GetCVPattern("eca\u207Fb"));
		}
	}
}