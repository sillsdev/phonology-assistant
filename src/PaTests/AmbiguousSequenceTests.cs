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
// File: MiscTests.cs
// Responsibility: DavidO & ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DataUtils
    /// </summary>
    /// --------------------------------------------------------------------------------
	[TestFixture]
	public class AmbiguousSequenceTests : TestBase
	{
		private AmbiguousSequences m_ambigSeqList;

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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_ambigSeqList = new AmbiguousSequences();
			m_prj.AmbiguousSequences.Clear();
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
		/// Tests that ambiguous sequences get sorted by length when all sequences are of a
		/// different length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SortTest_DifferentLengths()
		{
			m_ambigSeqList.Add("12");
			m_ambigSeqList.Add("123");
			m_ambigSeqList.Add("1234");
			m_ambigSeqList.Add("12345");

			m_ambigSeqList.SortByUnitLength();

			Assert.AreEqual("12345", m_ambigSeqList[0].Literal);
			Assert.AreEqual("1234", m_ambigSeqList[1].Literal);
			Assert.AreEqual("123", m_ambigSeqList[2].Literal);
			Assert.AreEqual("12", m_ambigSeqList[3].Literal);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that ambiguous sequences get sorted properly when some are of the same
		/// length as others.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SortTest_WithSameLengths()
		{
			m_ambigSeqList.Add("12");
			m_ambigSeqList.Add("123");
			m_ambigSeqList.Add("1234");
			m_ambigSeqList.Add("abc");
			m_ambigSeqList.Add("12345");
			m_ambigSeqList.Add("ab");

			m_ambigSeqList.SortByUnitLength();

			Assert.AreEqual("12345", m_ambigSeqList[0].Literal);
			Assert.AreEqual("1234", m_ambigSeqList[1].Literal);
			Assert.AreEqual("123", m_ambigSeqList[2].Literal);
			Assert.AreEqual("abc", m_ambigSeqList[3].Literal);
			Assert.AreEqual("12", m_ambigSeqList[4].Literal);
			Assert.AreEqual("ab", m_ambigSeqList[5].Literal);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the ambiguous sequences list gets sorted properly when assigned to
		/// the cache's AmbiguousSequences property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SortTest_WhenAssignedToCache()
		{
			m_ambigSeqList.Add("12");
			m_ambigSeqList.Add("123");
			m_ambigSeqList.Add("1234");
			m_ambigSeqList.Add("abc");
			m_ambigSeqList.Add("12345");
			m_ambigSeqList.Add("ab");

			// This will influence the sorted list but we don't want it to.
			SetProperty(App.IPASymbolCache, "ToneLetters", null);

			m_prj.AmbiguousSequences.AddRange(m_ambigSeqList);

			// Get the value of the internal list that should be sorted.
			m_ambigSeqList =
				GetField(App.IPASymbolCache, "m_sortedAmbiguousSeqList") as AmbiguousSequences;

			Assert.AreEqual("12345", m_ambigSeqList[0].Literal);
			Assert.AreEqual("1234", m_ambigSeqList[1].Literal);
			Assert.AreEqual("123", m_ambigSeqList[2].Literal);
			Assert.AreEqual("abc", m_ambigSeqList[3].Literal);
			Assert.AreEqual("12", m_ambigSeqList[4].Literal);
			Assert.AreEqual("ab", m_ambigSeqList[5].Literal);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests that the ambiguous sequences list contains an ambiguous sequence
		///// automatically added from calling PhoneticParser with a phonetic string that begins
		///// with a diacritic.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void AmbiguousSeqTest_ParserAutoAdd1()
		//{
		//    Dictionary<int, string[]> uncertainties;
		//    App.IPASymbolCache.PhoneticParser("\u1D50bcdef", false, out uncertainties);
		//    Assert.AreEqual("\u1D50b", App.IPASymbolCache.AmbiguousSequences[0].Literal);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests that the ambiguous sequences list contains an ambiguous sequence
		///// automatically added from calling PhoneticParser with a phonetic string that begins
		///// with a diacritic. The internal list is checked to make sure the one automatically
		///// added is in the right order.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void AmbiguousSeqTest_ParserAutoAdd2()
		//{
		//    m_ambigSeqList.Add("12");
		//    m_ambigSeqList.Add("123");

		//    // This will influence the sorted list but we don't want it to.
		//    SetField(App.IPACharCache, "m_toneLetters", null);

		//    App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

		//    Dictionary<int, string[]> uncertainties;
		//    App.IPASymbolCache.PhoneticParser("\u1D50bcdef", false, out uncertainties);

		//    // Get the value of the internal list that should be sorted.
		//    m_ambigSeqList =
		//        GetField(App.IPACharCache, "m_sortedAmbiguousSeqList") as AmbiguousSequences;

		//    Assert.AreEqual("123", m_ambigSeqList[0].Literal);
		//    Assert.AreEqual("12", m_ambigSeqList[1].Literal);
		//    Assert.AreEqual("\u1D50b", m_ambigSeqList[2].Literal);
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FindAmbiguousSequences on the IPACharCache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindAmbiguousSequencesTest()
		{
			// TODO: Fix this test

			//Assert.IsNull(m_prj.AmbiguousSequences.Find(s => s.Literal == "abc"));
			//Assert.IsNull(m_prj.AmbiguousSequences.Find(s => s.Literal == "abc def"));
			//Assert.IsNull(m_prj.AmbiguousSequences.Find(s => s.Literal == "Xabc"));
			//Assert.IsNull(m_prj.AmbiguousSequences.Find(s => s.Literal == "Xabc Xdef"));

			//var ambigSeqs = App.IPASymbolCache.FindAmbiguousSequences("\u1D50abc");
			//Assert.AreEqual(1, ambigSeqs.Count);
			//Assert.AreEqual("\u1D50a", ambigSeqs[0]);

			//ambigSeqs = App.IPASymbolCache.FindAmbiguousSequences("\u1D50abc def");
			//Assert.AreEqual(1, ambigSeqs.Count);
			//Assert.AreEqual("\u1D50a", ambigSeqs[0]);

			//ambigSeqs = App.IPASymbolCache.FindAmbiguousSequences("\u1D50abc \u1D50def");
			//Assert.AreEqual(2, ambigSeqs.Count);
			//Assert.AreEqual("\u1D50a", ambigSeqs[0]);
			//Assert.AreEqual("\u1D50d", ambigSeqs[1]);

			// The code to make the following three asserts pass should be
			// written at some point, but not now. For now, the problem is
			// easily dealt with using an explicit ambiguous sequence.
			
			// Test with tie bars
			//ambigSeqs = App.IPASymbolCache.FindAmbiguousSequences("\u1D50t\u035Csab \u1D50t\u0361sab");
			//Assert.AreEqual(2, ambigSeqs.Count);
			//Assert.AreEqual("\u1D50t\u035Cs", ambigSeqs[0]);
			//Assert.AreEqual("\u1D50t\u0361s", ambigSeqs[1]);
		}
	}
}