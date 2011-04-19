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
			m_cache = new PhoneCache(m_prj);
			m_prj.CVPatternInfoList = new List<CVPatternInfo>();
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
		/// <summary>
		/// Tests the GetTypeOfPhones method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTypeOfPhones_ConsVows()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("e");
			m_cache.AddPhone("i");
			m_cache.AddPhone("d");
			m_cache.AddPhone("z");
			Assert.AreEqual(5, m_cache.Count);

			var cons = (string[])ReflectionHelper.GetResult(
				m_cache, "GetTypeOfPhones", IPASymbolType.Consonant);
			
			Assert.AreEqual(2, cons.Length);
			Assert.AreEqual("d", cons[0]);
			Assert.AreEqual("z", cons[1]);

			var vows = (string[])ReflectionHelper.GetResult(
				m_cache, "GetTypeOfPhones", IPASymbolType.Vowel);

			Assert.AreEqual(3, vows.Length);
			Assert.AreEqual("a", vows[0]);
			Assert.AreEqual("e", vows[1]);
			Assert.AreEqual("i", vows[2]);		
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetTypeOfPhones method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTypeOfPhones_SSeg()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("d");
			m_cache.AddPhone("i");
			m_cache.AddPhone("\u02E6");
			Assert.AreEqual(4, m_cache.Count);

			var ssegs = (string[])ReflectionHelper.GetResult(
				m_cache, "GetTypeOfPhones", IPASymbolType.Suprasegmentals);

			Assert.AreEqual(1, ssegs.Length);
			Assert.AreEqual("\u02E6", ssegs[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCommaDelimitedPhones method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCommaDelimitedPhones()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("e");
			m_cache.AddPhone("i");
			m_cache.AddPhone("d");
			m_cache.AddPhone("z");
			Assert.AreEqual(5, m_cache.Count);

			var cons = (string)ReflectionHelper.GetResult(
				m_cache, "GetCommaDelimitedPhones", IPASymbolType.Consonant);

			Assert.AreEqual("d,z", cons);

			var vows = (string)ReflectionHelper.GetResult(
				m_cache, "GetCommaDelimitedPhones", IPASymbolType.Vowel);

			Assert.AreEqual("a,e,i", vows);
		}


	}
}