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
		Dictionary<int, string[]> uncertainties;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_ambigSeqList = new AmbiguousSequences();
			m_prj.AmbiguousSequences.Clear();
			m_prj.TranscriptionChanges.Clear();
			//App.IPASymbolCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			m_prj.PhoneticParser.LogUndefinedCharactersWhenParsing = true;
			m_prj.PhoneticParser.ResetAmbiguousSequencesForTests();
		}

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
			var result = Parse("abc", true, false, out uncertainties);
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
			App.IPASymbolCache.UndefinedCharacters.Clear();
			var result = Parse("abX", true, false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("X", result[2]);
			Assert.AreEqual(1, App.IPASymbolCache.UndefinedCharacters.Count);
			App.IPASymbolCache.UndefinedCharacters.Clear();

			result = Parse("aXb", true, false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("X", result[1]);
			Assert.AreEqual("b", result[2]);
			Assert.AreEqual(1, App.IPASymbolCache.UndefinedCharacters.Count);
			App.IPASymbolCache.UndefinedCharacters.Clear();

			result = Parse("XaXX", true, false, out uncertainties);
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
			var result = Parse("x\u0061\u0306\u0301x", true, false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("x", result[0]);
			Assert.AreEqual("\u0061\u0306\u0301", result[1]);
			Assert.AreEqual("x", result[2]);
			Assert.AreEqual(0, App.IPASymbolCache.UndefinedCharacters.Count);

			result = Parse("x\u1EAFx", true, true, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("x", result[0]);
			Assert.AreEqual("\u0061\u0306\u0301", result[1]);
			Assert.AreEqual("x", result[2]);
			Assert.AreEqual(0, App.IPASymbolCache.UndefinedCharacters.Count);

			result = Parse("xX\u0103\u0301X", true, true, out uncertainties);
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
			var result = Parse("\u0301\u0061\u0306\u0301", true, false, out uncertainties);
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
			// Top tie bar test
			var result = Parse("abk\u035Cpc", true, false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("k\u035Cp", result[2]);
			Assert.AreEqual("c", result[3]);

			// Bottom tie bar test
			result = Parse("abk\u0361pc", true, false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("a", result[0]);
			Assert.AreEqual("b", result[1]);
			Assert.AreEqual("k\u0361p", result[2]);
			Assert.AreEqual("c", result[3]);

			// Linking (absence of a break)
			result = Parse("abk\u203Fpc", true, false, out uncertainties);
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
			var result = Parse("b(a/o)g", true, false, out uncertainties);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("b", result[0]);
			Assert.AreEqual("a", result[1]);
			Assert.AreEqual("g", result[2]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[1].Length);
			Assert.AreEqual("a", uncertainties[1][0]);
			Assert.AreEqual("o", uncertainties[1][1]);

			result = Parse("bl(a/\u0103\u0301/o)g", false, false, out uncertainties);
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
			var result = Parse("b(a/o)gg(e/a/i)r", true, false, out uncertainties);
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
			var result = Parse("(d/b)og", true, false, out uncertainties);
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
			var result = Parse("di(n/m)", true, false, out uncertainties);
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
			var result = Parse("(1/2)x(3/4)", true, false, out uncertainties);
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
			var result = Parse("pe(0/i)t", true, false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("e", result[1]);
			Assert.AreEqual("", result[2]);
			Assert.AreEqual("t", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("", uncertainties[2][0]);
			Assert.AreEqual("i", uncertainties[2][1]);

			result = Parse("pe(i/0)t", true, false, out uncertainties);
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
			var result = Parse("pe(/i)t", true, false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("e", result[1]);
			Assert.AreEqual("", result[2]);
			Assert.AreEqual("t", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("", uncertainties[2][0]);
			Assert.AreEqual("i", uncertainties[2][1]);

			result = Parse("pe(i/)t", true, false, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("p", result[0]);
			Assert.AreEqual("e", result[1]);
			Assert.AreEqual("i", result[2]);
			Assert.AreEqual("t", result[3]);

			Assert.AreEqual(1, uncertainties.Count);
			Assert.AreEqual(2, uncertainties[2].Length);
			Assert.AreEqual("i", uncertainties[2][0]);
			Assert.AreEqual("", uncertainties[2][1]);

			result = Parse("pe(i//o)t", true, false, out uncertainties);
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
			App.IPASymbolCache.UndefinedCharacters.Clear();

			var result = Parse("p(ai)t", true, false, out uncertainties);
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
			var result = Parse("a(1/2)(3/4)(5/6)b", true, false, out uncertainties);
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
			m_ambigSeqList.Add("\u1D50b");
			m_prj.AmbiguousSequences.AddRange(m_ambigSeqList);

			string[] result = Parse("\u1D50bai", true, true, out uncertainties);
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
			m_ambigSeqList.Add("\u1D50b");
			
			m_prj.AmbiguousSequences.AddRange(m_ambigSeqList);

			var result = Parse("a\u1D50bi", true, true, out uncertainties);
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
			m_prj.AmbiguousSequences.Add("\u1D50b");
			var result = Parse("ai\u1D50b", true, true, out uncertainties);
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
			m_prj.AmbiguousSequences.Add("\u1D51g");
			m_prj.AmbiguousSequences.Add("t\u0283");

			var result = Parse("ab\u1D51gct\u0283de", true, true, out uncertainties);
			Assert.AreEqual(7, result.Length);
			Assert.AreEqual("\u1D51g", result[2]);
			Assert.AreEqual("t\u0283", result[4]);

			result = Parse("ab\u1D51gt\u0283de", true, true, out uncertainties);
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
			m_ambigSeqList.Add("\u1D51g");
			m_ambigSeqList.Add("t\u0283");
			m_prj.AmbiguousSequences.AddRange(m_ambigSeqList);

			// Test at the beginning of the uncertain group.
			var result = Parse("ab(\u1D51g/i/a)de", true, true, out uncertainties);
			Assert.AreEqual(5, result.Length);
			Assert.AreEqual("\u1D51g", result[2]);

			// Test in the middle of the uncertain group.
			result = Parse("ab(i/\u1D51g/a)de", true, true, out uncertainties);
			Assert.AreEqual(5, result.Length);
			Assert.AreEqual("\u1D51g", uncertainties[2][1]);

			// Test at the end of the uncertain group.
			result = Parse("a(i/a/\u1D51g)de", true, true, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("\u1D51g", uncertainties[1][2]);

			// Test two ambiguities in the uncertain group.
			result = Parse("a(i/t\u0283/a/\u1D51g)de", true, true, out uncertainties);
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
			var ambigItem = new AmbiguousSeq("\u1D4Ab");
			ambigItem.Convert = false;
			m_prj.AmbiguousSequences.Add(ambigItem);

			var result = Parse("d\u1D4Abai", true, true, out uncertainties);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("d\u1D4A", result[0]);
			Assert.AreEqual("b", result[1]);

			ambigItem.Convert = true;
			result = Parse("d\u1D4Abai", true, true, out uncertainties);
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
			m_ambigSeqList.Add("sc");
			m_ambigSeqList.Add("t\u0361s");
			m_prj.AmbiguousSequences.AddRange(m_ambigSeqList);

			var result = Parse("t\u0361sc", true, true, out uncertainties);
			Assert.AreEqual("t\u0361s", result[0]);
			Assert.AreEqual("c", result[1]);
		}
	}
}