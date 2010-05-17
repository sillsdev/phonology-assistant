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
// File: PhoneticParserTests.cs
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
    /// Tests Misc. methods in App.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class PhoneticParserTests : TestBase
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
			InventoryHelper.Load(m_inventoryFile);
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
			App.IPASymbolCache.AmbiguousSequences.Clear();
			App.IPASymbolCache.TranscriptionChanges.Clear();
			App.IPASymbolCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			App.IPASymbolCache.LogUndefinedCharactersWhenParsing = true;
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
		/// Tests the phonetic parser with a simple string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneticParserTest_Simple()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("abc", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("c", result[2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the phonetic parser with strings containinig invalid characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneticParserTest_InvalidChars()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("abX", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("X", result[2]);
			Assert.AreEqual(1, App.IPASymbolCache.UndefinedCharacters.Count);
			App.IPASymbolCache.UndefinedCharacters.Clear();

			result = App.IPASymbolCache.PhoneticParser("aXb", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("X", result[1]);
			Assert.AreEqual("b", result[2]);
			Assert.AreEqual(1, App.IPASymbolCache.UndefinedCharacters.Count);
			App.IPASymbolCache.UndefinedCharacters.Clear();

			result = App.IPASymbolCache.PhoneticParser("XaXX", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("X", result[0]);
			Assert.AreEqual("a", result[1]);
			Assert.AreEqual("X", result[2]);
			Assert.AreEqual("X", result[3]);
			Assert.AreEqual(3, App.IPASymbolCache.UndefinedCharacters.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the phonetic parser with composite characters
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneticParserTest_CompositePhones()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("x\u0061\u0306\u0301x", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("x", result[0]);
			Assert.AreEqual("\u0061\u0306\u0301", result[1]);
			Assert.AreEqual("x", result[2]);
			Assert.AreEqual(0, App.IPASymbolCache.UndefinedCharacters.Count);

			result = App.IPASymbolCache.PhoneticParser("x\u1EAFx", true, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("x", result[0]);
			Assert.AreEqual("\u0061\u0306\u0301", result[1]);
			Assert.AreEqual("x", result[2]);
			Assert.AreEqual(0, App.IPASymbolCache.UndefinedCharacters.Count);

			result = App.IPASymbolCache.PhoneticParser("xX\u0103\u0301X", true, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("x", result[0]);
			Assert.AreEqual("X", result[1]);
			Assert.AreEqual("\u0061\u0306\u0301", result[2]);
			Assert.AreEqual("X", result[3]);
			Assert.AreEqual(2, App.IPASymbolCache.UndefinedCharacters.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the phonetic parser with a string containing a diacritic preceeding a
		/// base character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneticParserTest_DiacriticBeforeBase()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("\u0301\u0061\u0306\u0301", false, out uncertainties);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual("\u0301\u0061\u0306\u0301", result[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the phonetic parser with a string containing tie bars.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PhoneticParserTest_TieBars()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			// Top tie bar test
			result = App.IPASymbolCache.PhoneticParser("abk\u035Cpc", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("k\u035Cp", result[2]);
			Assert.AreEqual("c", result[3]);

			// Bottom tie bar test
			result = App.IPASymbolCache.PhoneticParser("abk\u0361pc", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("k\u0361p", result[2]);
			Assert.AreEqual("c", result[3]);

			// Linking (absence of a break)
			result = App.IPASymbolCache.PhoneticParser("abk\u203Fpc", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("k\u203Fp", result[2]);
			Assert.AreEqual("c", result[3]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string with one uncertain
		/// phone group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_OneUncertaintyGroup()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("b(a/o)g", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("b", result[0]);
			Assert.AreEqual("a", result[1]);
			Assert.AreEqual("g", result[2]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[1].Length);
			Assert.AreEqual("a", uncertainties[1][0]);
			Assert.AreEqual("o", uncertainties[1][1]);

			result = App.IPASymbolCache.PhoneticParser("bl(a/\u0103\u0301/o)g", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("b", result[0]);
			Assert.AreEqual("l", result[1]);
			Assert.AreEqual("a", result[2]);
			Assert.AreEqual("g", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(3, uncertainties[2].Length);
			Assert.AreEqual("a", uncertainties[2][0]);
			Assert.AreEqual("\u0103\u0301", uncertainties[2][1]);
			Assert.AreEqual("o", uncertainties[2][2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string with more than one
		/// uncertain phone group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_TwoUncertaintyGroup()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("b(a/o)gg(e/a/i)r", false, out uncertainties);
			Assert.AreEqual(6, result.Length);
			Assert.AreEqual("b", result[0]);
			Assert.AreEqual("a", result[1]);
			Assert.AreEqual("g", result[2]);
			Assert.AreEqual("g", result[3]);
			Assert.AreEqual("e", result[4]);
			Assert.AreEqual("r", result[5]);

			Assert.AreEqual(2, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[1].Length);
			Assert.AreEqual(3, uncertainties[4].Length);
			Assert.AreEqual("a", uncertainties[1][0]);
			Assert.AreEqual("o", uncertainties[1][1]);
			Assert.AreEqual("e", uncertainties[4][0]);
			Assert.AreEqual("a", uncertainties[4][1]);
			Assert.AreEqual("i", uncertainties[4][2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string when an uncertain phone
		/// group begins the word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_BeginningUncertaintyGroup()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("(d/b)og", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("d", result[0]);
			Assert.AreEqual("o", result[1]);
			Assert.AreEqual("g", result[2]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[0].Length);
			Assert.AreEqual("d", uncertainties[0][0]);
			Assert.AreEqual("b", uncertainties[0][1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string when an uncertain phone
		/// group ends the word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_EndingUncertaintyGroup()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("di(n/m)", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("d", result[0]);
			Assert.AreEqual("i", result[1]);
			Assert.AreEqual("n", result[2]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("n", uncertainties[2][0]);
			Assert.AreEqual("m", uncertainties[2][1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string when an uncertain phone
		/// group ends the word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_BeginAndEndUncertaintyGroup()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("(1/2)x(3/4)", false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("1", result[0]);
			Assert.AreEqual("x", result[1]);
			Assert.AreEqual("3", result[2]);

			Assert.AreEqual(2, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[0].Length);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("1", uncertainties[0][0]);
			Assert.AreEqual("2", uncertainties[0][1]);
			Assert.AreEqual("3", uncertainties[2][0]);
			Assert.AreEqual("4", uncertainties[2][1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string when an uncertain phone
		/// group contains an emtpy set in the form of the zero character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_EmptySetInUncertaintyGroup1()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("pe(0/i)t", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("e", result[1]);
			Assert.AreEqual("", result[2]);
			Assert.AreEqual("t", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("", uncertainties[2][0]);
			Assert.AreEqual("i", uncertainties[2][1]);

			result = App.IPASymbolCache.PhoneticParser("pe(i/0)t", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("e", result[1]);
			Assert.AreEqual("i", result[2]);
			Assert.AreEqual("t", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("i", uncertainties[2][0]);
			Assert.AreEqual("", uncertainties[2][1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string when an uncertain phone
		/// group contains an emtpy set in the form of no character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_EmptySetInUncertaintyGroup2()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("pe(/i)t", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("e", result[1]);
			Assert.AreEqual("", result[2]);
			Assert.AreEqual("t", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("", uncertainties[2][0]);
			Assert.AreEqual("i", uncertainties[2][1]);

			result = App.IPASymbolCache.PhoneticParser("pe(i/)t", false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("e", result[1]);
			Assert.AreEqual("i", result[2]);
			Assert.AreEqual("t", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("i", uncertainties[2][0]);
			Assert.AreEqual("", uncertainties[2][1]);

			result = App.IPASymbolCache.PhoneticParser("pe(i//o)t", false, out uncertainties);
			Assert.AreEqual(4, result.Length);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(3, uncertainties[2].Length);
			Assert.AreEqual("i", uncertainties[2][0]);
			Assert.AreEqual("", uncertainties[2][1]);
			Assert.AreEqual("o", uncertainties[2][2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string when there are open and
		/// closed parentheses without any slashes between.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_ParensWithoutSlashes()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("p(ai)t", false, out uncertainties);
			Assert.AreEqual(6, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("(", result[1]);
			Assert.AreEqual("a", result[2]);
			Assert.AreEqual("i", result[3]);
			Assert.AreEqual(")", result[4]);
			Assert.AreEqual("t", result[5]);

			Assert.AreEqual(0, uncertainties.Count);
			Assert.AreEqual(2, App.IPASymbolCache.UndefinedCharacters.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser properly parses a string when there are open and
		/// closed parentheses without any slashes between.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UncertainDataTest_SimultaneousGroups()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			result = App.IPASymbolCache.PhoneticParser("a(1/2)(3/4)(5/6)b", false, out uncertainties);
			Assert.AreEqual(5, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("1", result[1]);
			Assert.AreEqual("3", result[2]);
			Assert.AreEqual("5", result[3]);
			Assert.AreEqual("b", result[4]);

			Assert.AreEqual(3, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[1].Length);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual(2, uncertainties[3].Length);

			Assert.AreEqual("1", uncertainties[1][0]);
			Assert.AreEqual("2", uncertainties[1][1]);
			Assert.AreEqual("3", uncertainties[2][0]);
			Assert.AreEqual("4", uncertainties[2][1]);
			Assert.AreEqual("5", uncertainties[3][0]);
			Assert.AreEqual("6", uncertainties[3][1]);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser parses phonetic strings containing an ambiguous
		/// sequence at word intial.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AmbiguousSeqTest_WordInitial()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			m_ambigSeqList.Add("\u1D50b");
			App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

			result = App.IPASymbolCache.PhoneticParser("\u1D50bai", true, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("\u1D50b", result[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser parses phonetic strings containing an ambiguous
		/// sequence word medially.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AmbiguousSeqTest_WordMedial()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			m_ambigSeqList.Add("\u1D50b");
			App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

			result = App.IPASymbolCache.PhoneticParser("a\u1D50bi", true, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("\u1D50b", result[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser parses phonetic strings containing an ambiguous
		/// sequence at word final.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AmbiguousSeqTest_WordFinal()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			m_ambigSeqList.Add("\u1D50b");
			App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

			result = App.IPASymbolCache.PhoneticParser("ai\u1D50b", true, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("\u1D50b", result[2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser parses phonetic strings containing multiple
		/// ambiguous sequences relative to each other.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AmbiguousSeqTest_MultipleSequences()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			m_ambigSeqList.Add("\u1D51g");
			m_ambigSeqList.Add("t\u0283");
			App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

			result = App.IPASymbolCache.PhoneticParser("ab\u1D51gct\u0283de", true, out uncertainties);
			Assert.AreEqual(7, result.Length);
			Assert.AreEqual("\u1D51g", result[2]);
			Assert.AreEqual("t\u0283", result[4]);

			result = App.IPASymbolCache.PhoneticParser("ab\u1D51gt\u0283de", true, out uncertainties);
			Assert.AreEqual(6, result.Length);
			Assert.AreEqual("\u1D51g", result[2]);
			Assert.AreEqual("t\u0283", result[3]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser parses phonetic strings containing multiple
		/// ambiguous sequences relative to each other.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AmbiguousSeqTest_InUncertainGroup()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			m_ambigSeqList.Add("\u1D51g");
			m_ambigSeqList.Add("t\u0283");
			App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

			// Test at the beginning of the uncertain group.
			result = App.IPASymbolCache.PhoneticParser("ab(\u1D51g/i/a)de", true, out uncertainties);
			Assert.AreEqual(5, result.Length);
			Assert.AreEqual("\u1D51g", result[2]);

			// Test in the middle of the uncertain group.
			result = App.IPASymbolCache.PhoneticParser("ab(i/\u1D51g/a)de", true, out uncertainties);
			Assert.AreEqual(5, result.Length);
			Assert.AreEqual("\u1D51g", uncertainties[2][1]);

			// Test at the end of the uncertain group.
			result = App.IPASymbolCache.PhoneticParser("a(i/a/\u1D51g)de", true, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("\u1D51g", uncertainties[1][2]);

			// Test two ambiguities in the uncertain group.
			result = App.IPASymbolCache.PhoneticParser("a(i/t\u0283/a/\u1D51g)de", true, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("i", uncertainties[1][0]);
			Assert.AreEqual("t\u0283", uncertainties[1][1]);
			Assert.AreEqual("a", uncertainties[1][2]);
			Assert.AreEqual("\u1D51g", uncertainties[1][3]);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser does not convert any ambiguous seq. even when there
		/// are some to convert but the convert flag is turned off.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AmbiguousSeqTest_ToggleConvert()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			AmbiguousSeq ambigItem = new AmbiguousSeq("\u1D4Ab");
			ambigItem.Convert = false;
			m_ambigSeqList.Add(ambigItem);
			App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

			result = App.IPASymbolCache.PhoneticParser("d\u1D4Abai", true, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("d\u1D4A", result[0]);
			Assert.AreEqual("b", result[1]);

			ambigItem.Convert = true;
			result = App.IPASymbolCache.PhoneticParser("d\u1D4Abai", true, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("d", result[0]);
			Assert.AreEqual("\u1D4Ab", result[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the phonetic parser does not split a tie-barred phone when part of the
		/// tie-barred phone begins an ambiguous sequence. This requires the tie-barred phone
		/// to also be in the ambiguous sequences list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AmbiguousSeqTest_TieBarTest()
		{
			string[] result;
			Dictionary<int, string[]> uncertainties;

			m_ambigSeqList.Add("sc");
			m_ambigSeqList.Add("t\u0361s");
			App.IPASymbolCache.AmbiguousSequences = m_ambigSeqList;

			result = App.IPASymbolCache.PhoneticParser("t\u0361sc", true, out uncertainties);
			Assert.AreEqual("t\u0361s", result[0]);
			Assert.AreEqual("c", result[1]);
		}
	}
}