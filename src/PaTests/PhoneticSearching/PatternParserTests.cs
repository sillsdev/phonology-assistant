using System;
using System.Linq;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.TestUtils;
using System.Collections.Generic;

namespace SIL.Pa.Tests
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PatternParserTests : TestBase
	{
		private PhoneCache _phoneCache;
		private PatternParser _parser;

		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
			base.FixtureSetup();
		
			App.IPASymbolCache.Add('~', new IPASymbol { Literal = "~", IsBase = false, Type = IPASymbolType.diacritic });
			App.IPASymbolCache.Add('=', new IPASymbol { Literal = "=", IsBase = false, Type = IPASymbolType.diacritic });
			App.IPASymbolCache.Add(';', new IPASymbol { Literal = ";", IsBase = false, Type = IPASymbolType.suprasegmental });
			App.IPASymbolCache.Add('`', new IPASymbol { Literal = "`", IsBase = false, Type = IPASymbolType.suprasegmental });
			App.IPASymbolCache.Add(',', new IPASymbol { Literal = ",", IsBase = false, Type = IPASymbolType.suprasegmental });
			App.IPASymbolCache.Add('X', new IPASymbol { Literal = "X", IsBase = true, Type = IPASymbolType.suprasegmental });
			App.IPASymbolCache.Add('Z', new IPASymbol { Literal = "Z", IsBase = true, Type = IPASymbolType.suprasegmental });
			App.IPASymbolCache.Add(-100, new IPASymbol { Literal = "OO", IsBase = true, Type = IPASymbolType.suprasegmental });
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_prj.SearchClasses.Clear();
			_parser = new PatternParser(_prj);
			_phoneCache = _prj.PhoneCache;
			
			_phoneCache.Clear();
			_phoneCache["b"] = new PhoneInfo { Phone = "b", CharType = IPASymbolType.consonant };
			_phoneCache["c"] = new PhoneInfo { Phone = "c", CharType = IPASymbolType.consonant };
			_phoneCache["d"] = new PhoneInfo { Phone = "d", CharType = IPASymbolType.consonant };
			_phoneCache["f"] = new PhoneInfo { Phone = "f", CharType = IPASymbolType.consonant };
			_phoneCache["g"] = new PhoneInfo { Phone = "g", CharType = IPASymbolType.consonant };
			_phoneCache["h"] = new PhoneInfo { Phone = "h", CharType = IPASymbolType.consonant };
			_phoneCache["j"] = new PhoneInfo { Phone = "j", CharType = IPASymbolType.consonant };
			_phoneCache["k"] = new PhoneInfo { Phone = "k", CharType = IPASymbolType.consonant };
			_phoneCache["l"] = new PhoneInfo { Phone = "l", CharType = IPASymbolType.consonant };
			_phoneCache["m"] = new PhoneInfo { Phone = "m", CharType = IPASymbolType.consonant };
			_phoneCache["n"] = new PhoneInfo { Phone = "n", CharType = IPASymbolType.consonant };
			_phoneCache["p"] = new PhoneInfo { Phone = "p", CharType = IPASymbolType.consonant };
			_phoneCache["r"] = new PhoneInfo { Phone = "r", CharType = IPASymbolType.consonant };
			_phoneCache["s"] = new PhoneInfo { Phone = "s", CharType = IPASymbolType.consonant };
			_phoneCache["t"] = new PhoneInfo { Phone = "t", CharType = IPASymbolType.consonant };
			_phoneCache["v"] = new PhoneInfo { Phone = "v", CharType = IPASymbolType.consonant };
			_phoneCache["w"] = new PhoneInfo { Phone = "w", CharType = IPASymbolType.consonant };
			_phoneCache["x"] = new PhoneInfo { Phone = "x", CharType = IPASymbolType.consonant };
			_phoneCache["y"] = new PhoneInfo { Phone = "y", CharType = IPASymbolType.consonant };
			_phoneCache["z"] = new PhoneInfo { Phone = "z", CharType = IPASymbolType.consonant };

			_phoneCache["a"] = new PhoneInfo { Phone = "a", CharType = IPASymbolType.vowel };
			_phoneCache["e"] = new PhoneInfo { Phone = "e", CharType = IPASymbolType.vowel };
			_phoneCache["i"] = new PhoneInfo { Phone = "i", CharType = IPASymbolType.vowel };
			_phoneCache["o"] = new PhoneInfo { Phone = "o", CharType = IPASymbolType.vowel };
			_phoneCache["u"] = new PhoneInfo { Phone = "u", CharType = IPASymbolType.vowel };

			_phoneCache["."] = new PhoneInfo { Phone = ".", CharType = IPASymbolType.suprasegmental };
			_phoneCache["'"] = new PhoneInfo { Phone = "'", CharType = IPASymbolType.suprasegmental };

			App.AFeatureCache.Clear();
			App.AFeatureCache.LoadFromList(new[]
			{
				new Feature { Name = "con" },
				new Feature { Name = "vow" },
				new Feature { Name = "aei" },
				new Feature { Name = "ou" },
				new Feature { Name = "tdb" },
				new Feature { Name = "xyz" },
				new Feature { Name = "justu" },
				new Feature { Name = "ta" },
				new Feature { Name = "ae~" },
				new Feature { Name = "ae~=" },
				new Feature { Name = "ae`" },
				new Feature { Name = "ae;`" }
			});

			_prj.BFeatureCache.Clear();
			_prj.BFeatureCache.LoadFromList(new[]
			{
				new Feature { Name = "+con" },
				new Feature { Name = "+vow" },
				new Feature { Name = "+aei" },
				new Feature { Name = "+ou" },
				new Feature { Name = "+tdb" },
				new Feature { Name = "+xyz" }
			});

			((PhoneInfo)_phoneCache["t"]).BFeatureNames = new List<string> { "+con", "+tdb", "-xyz", "-vow" };
			((PhoneInfo)_phoneCache["d"]).BFeatureNames = new List<string> { "+con", "+tdb", "-xyz", "-vow" };
			((PhoneInfo)_phoneCache["b"]).BFeatureNames = new List<string> { "+con", "+tdb", "-xyz", "-vow" };
			((PhoneInfo)_phoneCache["x"]).BFeatureNames = new List<string> { "+con", "+xyz", "-tdb", "-vow" };
			((PhoneInfo)_phoneCache["y"]).BFeatureNames = new List<string> { "+con", "+xyz", "-tdb", "-vow" };
			((PhoneInfo)_phoneCache["z"]).BFeatureNames = new List<string> { "+con", "+xyz", "-tdb", "-vow" };

			((PhoneInfo)_phoneCache["a"]).BFeatureNames = new List<string> { "-con", "+aei", "-ou", "+vow" };
			((PhoneInfo)_phoneCache["e"]).BFeatureNames = new List<string> { "-con", "+aei", "-ou", "+vow" };
			((PhoneInfo)_phoneCache["i"]).BFeatureNames = new List<string> { "-con", "+aei", "-ou", "+vow" };
			((PhoneInfo)_phoneCache["o"]).BFeatureNames = new List<string> { "-con", "+ou", "-aei", "+vow" };
			((PhoneInfo)_phoneCache["u"]).BFeatureNames = new List<string> { "-con", "+ou", "-aei", "+vow" };

			((PhoneInfo)_phoneCache["t"]).AFeatureNames = new List<string> { "con", "tdb", "ta" };
			((PhoneInfo)_phoneCache["d"]).AFeatureNames = new List<string> { "con", "tdb" };
			((PhoneInfo)_phoneCache["b"]).AFeatureNames = new List<string> { "con", "tdb" };
			((PhoneInfo)_phoneCache["x"]).AFeatureNames = new List<string> { "con", "xyz" };
			((PhoneInfo)_phoneCache["y"]).AFeatureNames = new List<string> { "con", "xyz" };
			((PhoneInfo)_phoneCache["z"]).AFeatureNames = new List<string> { "con", "xyz" };

			((PhoneInfo)_phoneCache["a"]).AFeatureNames = new List<string> { "aei", "vow", "ta" };
			((PhoneInfo)_phoneCache["e"]).AFeatureNames = new List<string> { "aei", "vow" };
			((PhoneInfo)_phoneCache["i"]).AFeatureNames = new List<string> { "aei", "vow" };
			((PhoneInfo)_phoneCache["o"]).AFeatureNames = new List<string> { "ou",  "vow" };
			((PhoneInfo)_phoneCache["u"]).AFeatureNames = new List<string> { "ou",  "vow", "justu" };
		}

		#region VerifyBracketedText tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_SingleDistinctiveFeature_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("[+con]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_SingleDescriptiveFeature_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("[vow]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_ConsonantClass_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("[C]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_VowelClass_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("[V]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_FeatureAndVowelClassSequence_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("[V][aei]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_FeatureAndVowelClassInBrackets_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("[[V][ou]]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_FeatureAndVowelClassInBraces_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("{[V][-ou]}"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_InvalidDescriptiveFeature_ReturnsFalse()
		{
			Assert.IsFalse(_parser.VerifyBracketedText("[ooo]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_InvalidDistinctiveFeature_ReturnsFalse()
		{
			Assert.IsFalse(_parser.VerifyBracketedText("[-zzz]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_PathologicalNesting_ReturnsTrue()
		{
			Assert.IsTrue(_parser.VerifyBracketedText("{[[C][-vow]],[[[V][+aei]][-ou]]}"));
		}

		#endregion

		#region ModifyListIfContainsDiacriticPlaceholderCluster tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ModifyListIfContainsDiacriticPlaceholderCluster_2Clusters_ReturnsEmptyList()
		{
			var list = new List<string>(new[] { "a", App.kDottedCircle, "b", App.kDottedCircle });
			list = _parser.ModifyListIfContainsDiacriticPlaceholderCluster(list).ToList();
			Assert.AreEqual(0, list.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ModifyListIfContainsDiacriticPlaceholderCluster_NoClusterFound_ReturnsInputList()
		{
			var list = new List<string>(new[] { "z", "a=", "b", "e~=" });
			list = _parser.ModifyListIfContainsDiacriticPlaceholderCluster(list).ToList();
			Assert.AreEqual(4, list.Count);
			Assert.AreEqual("z", list[0]);
			Assert.AreEqual("a=", list[1]);
			Assert.AreEqual("b", list[2]);
			Assert.AreEqual("e~=", list[3]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ModifyListIfContainsDiacriticPlaceholderCluster_2of4Match_ReturnsThem()
		{
			var list = new List<string>(new[] { "z", "a=", App.kDottedCircle + "=*", "b", "e~=" });
			list = _parser.ModifyListIfContainsDiacriticPlaceholderCluster(list).ToList();
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual("a=", list[0]);
			Assert.AreEqual("e~=", list[1]);
		}

		#endregion

		#region ReplaceBracketedClassNamesWithPattern tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_ClassDoesNotExist_ReturnsEmptyString()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "dummy1", Type = SearchClassType.Phones });
			_prj.SearchClasses.Add(new SearchClass { Name = "dummy2", Type = SearchClassType.Phones });
			Assert.AreEqual(string.Empty, _parser.ReplaceBracketedClassNamesWithPattern("<not there>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContainsOnlyPhoneClass_ReturnsClassPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "nop", Type = SearchClassType.Phones, Pattern = "{n,o,p}" });
			Assert.AreEqual("{n,o,p}", _parser.ReplaceBracketedClassNamesWithPattern("<nop>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContainsOnlyAFeatureClass_ReturnsClassPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "xyzORou", Type = SearchClassType.Articulatory, Pattern = "{[xyz],[ou]}" });
			Assert.AreEqual("{[xyz],[ou]}", _parser.ReplaceBracketedClassNamesWithPattern("<xyzORou>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContainsOnlyBFeatureClass_ReturnsClassPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "+vowAND-ou", Type = SearchClassType.Binary, Pattern = "[[+vow][-ou]]" });
			Assert.AreEqual("[[+vow][-ou]]", _parser.ReplaceBracketedClassNamesWithPattern("<+vowAND-ou>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContains2Classes_ReturnsModifiedPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "nop", Type = SearchClassType.Phones, Pattern = "{n,o,p}" });
			_prj.SearchClasses.Add(new SearchClass { Name = "+vowAND-ou", Type = SearchClassType.Binary, Pattern = "[[+vow][-ou]]" });
			Assert.AreEqual("[V]{[[+vow][-ou]],{n,o,p}}[C]", _parser.ReplaceBracketedClassNamesWithPattern("[V]{<+vowAND-ou>,<nop>}[C]"));
		}

		#endregion

		#region GetDoesPhoneMatchDiacriticPlaceholderCluster tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesPhoneMatchDiacriticPlaceholderCluster_PassBaseSymbolOnly_ReturnsFalse()
		{
			Assert.IsFalse(_parser.GetDoesPhoneMatchDiacriticPlaceholderCluster("a", "~="));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesPhoneMatchDiacriticPlaceholderCluster_Pass1of2Diacritics_ReturnsFalse()
		{
			Assert.IsFalse(_parser.GetDoesPhoneMatchDiacriticPlaceholderCluster("a~", "~="));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesPhoneMatchDiacriticPlaceholderCluster_Pass1Of2DiacriticsAndClusterHas0OrMoreSymbol_ReturnsTrue()
		{
			Assert.IsTrue(_parser.GetDoesPhoneMatchDiacriticPlaceholderCluster("a~", "~*"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesPhoneMatchDiacriticPlaceholderCluster_Pass1Of2DiacriticsAndClusterHas1OrMoreSymbol_ReturnsFalse()
		{
			Assert.IsFalse(_parser.GetDoesPhoneMatchDiacriticPlaceholderCluster("a~", "~+"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesPhoneMatchDiacriticPlaceholderCluster_Pass2Of2DiacriticsAndClusterHas1OrMoreSymbol_ReturnsTrue()
		{
			Assert.IsTrue(_parser.GetDoesPhoneMatchDiacriticPlaceholderCluster("a~=", "~+"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesPhoneMatchDiacriticPlaceholderCluster_Pass2Of2Diacritics_ReturnsTrue()
		{
			Assert.IsTrue(_parser.GetDoesPhoneMatchDiacriticPlaceholderCluster("a~=", "~="));
		}

		#endregion

		#region AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols_ListContainsAllBasePhones_ReturnsInputList()
		{
			var inputList = new List<string> { "t", "d" };
			var outputList = _parser.AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols(inputList,
				si => { throw new Exception("Should never get here"); });
			
			Assert.AreEqual(2, outputList.Count);
			Assert.AreEqual("t", outputList[0]);
			Assert.AreEqual("d", outputList[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols_PhonesWithSingleDiacritics_ReturnsExpandedList()
		{
			var outputList = _parser.AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols(new List<string> { "a~", "e=" },
				si => si.Type == IPASymbolType.diacritic);
			
			Assert.AreEqual(4, outputList.Count);
			Assert.AreEqual("a~", outputList[0]);
			Assert.AreEqual("e=", outputList[1]);
			Assert.AreEqual("a", outputList[2]);
			Assert.AreEqual("e", outputList[3]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols_PhonesWithMixedNumberDiacritics_ReturnsExpandedList()
		{
			var outputList = _parser.AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols(new List<string> { "a~", "e~=" },
				si => si.Type == IPASymbolType.diacritic);
			
			Assert.AreEqual(6, outputList.Count);
			Assert.AreEqual("a~", outputList[0]);
			Assert.AreEqual("e~=", outputList[1]);
			Assert.AreEqual("a", outputList[2]);
			Assert.AreEqual("e=", outputList[3]);
			Assert.AreEqual("e~", outputList[4]);
			Assert.AreEqual("e", outputList[5]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols_PhonesWithNonBaseNonDiacritics_ReturnsInputList()
		{
			var outputList = _parser.AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols(new List<string> { "a;", "e`;" },
				si => si.Type == IPASymbolType.diacritic);
			
			Assert.AreEqual(2, outputList.Count);
			Assert.AreEqual("a;", outputList[0]);
			Assert.AreEqual("e`;", outputList[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols_PhonesWithBaseSuprasegmentals_ReturnsInputList()
		{
			var outputList = _parser.AddPhonesToGroupThatMatchIgnoriedNonBaseSymbols(new List<string> { "X", "Z" },
				si => { throw new Exception("Should never get here"); });
			
			Assert.AreEqual(2, outputList.Count);
			Assert.AreEqual("X", outputList[0]);
			Assert.AreEqual("Z", outputList[1]);
		}

		#endregion

		#region GetRegExpressionForIngoredBaseSymbols tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRegExpressionForIngoredBaseSymbols_InputContainsNoBaseSuprasegmentals_ReturnsEmptyString()
		{
			Assert.IsEmpty(_parser.GetRegExpressionForIngoredBaseSymbols(new List<string> { ";", "`" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRegExpressionForIngoredBaseSymbols_InputContainsOnlyBaseSuprasegmentals_ReturnsCorrectPattern()
		{
			Assert.AreEqual("(Z|X)?", _parser.GetRegExpressionForIngoredBaseSymbols(new List<string> { "Z", "X" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRegExpressionForIngoredBaseSymbols_InputContainsBothBaseAndNonBaseSuprasegmentals_ReturnsPatternOnlyForBase()
		{
			Assert.AreEqual("(Z|X)?", _parser.GetRegExpressionForIngoredBaseSymbols(new List<string> { "Z", ";", "X", "`" }));
		}

		#endregion

		#region Parse tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_VowelClass_ReturnsVowels()
		{
			Assert.AreEqual("(a|e|i|o|u)", _parser.Parse("[V]", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_SingleDescriptiveFeature_ReturnsPhonesInFeature()
		{
			Assert.AreEqual("(a|e|i)", _parser.Parse("[aei]", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_SingleDistinctive_ReturnsPhonesInFeature()
		{
			Assert.AreEqual("(b|d|t)", _parser.Parse("[+tdb]", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_SequentialVowelClassAndFeature_ReturnsTwoPhoneGroups()
		{
			Assert.AreEqual("(a|e|i|o|u)(o|u)", _parser.Parse("[V][+ou]", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_OrGroupOnly_ReturnsUnchangedPattern()
		{
			Assert.AreEqual("(r|s|t)", _parser.Parse("{r,s,t}", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_AndGroup_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("(o|u)", _parser.Parse("[[V][+ou]]", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_TripleAndGroup_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("u", _parser.Parse("[[V][+ou][justu]]", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_OrGroupWithFeatures_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("(b|d|t|x|y|z|a|e|i)", _parser.Parse("{[con],[aei]}", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_OrGroupWithFeaturesAndPhone_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("(b|d|t|x|y|z|a|e|i|o)", _parser.Parse("{[con],[aei],o}", false, null));
		}
		
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_OrGroupInsideAndGroup_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("b", _parser.Parse("[{b,y}[-xyz]]", false, null));
		}
		
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_MoreComplexOrGroupInsideAndGroup_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("t", _parser.Parse("[{[C],[V]}[[con][ta]]]", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_TwoOrGroupsInsideAndGroup_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("(a|e|i|o|u)", _parser.Parse("[{[C],[V]}{[vow],[+aei]}]", false, null));
		}
		
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_OrGroupContainingPhoneSequence_ReturnsQualifiedPhones()
		{
			Assert.AreEqual("(jk|do|p)", _parser.Parse("{(jk),(do),p}", false, null));
			Assert.AreEqual("(x|y|z|jk|n)", _parser.Parse("{[+xyz],(jk),n}", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_SimplePatternContainsPhoneClass_ReturnsQualifiedPhones()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "nop", Type = SearchClassType.Phones, Pattern = "{n,o,p}" });
			Assert.AreEqual("(n|o|p)", _parser.Parse("<nop>", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_SimplePatternContainsAFeatureClass_ReturnsQualifiedPhones()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "xyzORou", Type = SearchClassType.Articulatory, Pattern = "{[xyz],[ou]}" });
			Assert.AreEqual("(x|y|z|o|u)", _parser.Parse("<xyzORou>", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_SimplePatternContainsBFeatureClass_ReturnsQualifiedPhones()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "+vowAND-ou", Type = SearchClassType.Binary, Pattern = "[[+vow][-ou]]" });
			Assert.AreEqual("(a|e|i)", _parser.Parse("<+vowAND-ou>", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_PatternContains2ClassesEtc_ReturnsQualifiedPhones()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "nop", Type = SearchClassType.Phones, Pattern = "{n,o,p}" });
			_prj.SearchClasses.Add(new SearchClass { Name = "+vowAND-ou", Type = SearchClassType.Binary, Pattern = "[[+vow][-ou]]" });
			Assert.AreEqual("(a|e|i|o|u)(a|e|i|n|o|p)", _parser.Parse("[V]{<+vowAND-ou>,<nop>}", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoreDiacriticsWhenPhoneHasSingleDiacritic_ReturnsQualifiedPhones()
		{
			_phoneCache["a~"] = new PhoneInfo { Phone = "a~", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a~"]).AFeatureNames = new List<string> { "ae~" };
			Assert.AreEqual("(o|u|a~|a)", _parser.Parse("{[+ou],[ae~]}", true, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoreDiacriticsWhenPhonesHaveMultipleDiacritics_ReturnsQualifiedPhones()
		{
			_phoneCache["a~="] = new PhoneInfo { Phone = "a~=", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a~="]).AFeatureNames = new List<string> { "ae~=" };
			Assert.AreEqual("(o|u|a~=|a=|a~|a)", _parser.Parse("{[+ou],[ae~=]}", true, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoreDiacriticsWhenMultiplePhonesWithDiacritics_ReturnsQualifiedPhones()
		{
			_phoneCache["a~="] = new PhoneInfo { Phone = "a~=", CharType = IPASymbolType.vowel };
			_phoneCache["e="] = new PhoneInfo { Phone = "e=", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a~="]).AFeatureNames = new List<string> { "ae~=" };
			((PhoneInfo)_phoneCache["e="]).AFeatureNames = new List<string> { "ae~=" };
			Assert.AreEqual("(b|d|t|a~=|e=|a=|a~|a|e)", _parser.Parse("{[+tdb],[ae~=]}", true, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoredSSegsSendNullList_DoesNotCrashAndReturnsQualifiedPhones()
		{
			_phoneCache["a;"] = new PhoneInfo { Phone = "a;", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a;"]).AFeatureNames = new List<string> { "ae;`" };
			Assert.AreEqual("(o|u|a;)", _parser.Parse("{[+ou],[ae;`]}", false, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoredSSegsPhoneHas1_ReturnsQualifiedPhones()
		{
			_phoneCache["a;"] = new PhoneInfo { Phone = "a;", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a;"]).AFeatureNames = new List<string> { "ae;`" };
			Assert.AreEqual("(a;|a)", _parser.Parse("[ae;`]", false, new List<string> { ";" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_2IgnoredSSegsPhoneHas3_ReturnsQualifiedPhones()
		{
			_phoneCache["a;`,"] = new PhoneInfo { Phone = "a;`,", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a;`,"]).AFeatureNames = new List<string> { "ae;`" };
			Assert.AreEqual("(a;`,|a`,|a;`|a`)", _parser.Parse("[ae;`]", false, new List<string> { ";", "," }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoredSSegsPhonesHasMultiple_ReturnsQualifiedPhones()
		{
			_phoneCache["a;`"] = new PhoneInfo { Phone = "a;`", CharType = IPASymbolType.vowel };
			_phoneCache["e;`,"] = new PhoneInfo { Phone = "e;`,", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a;`"]).AFeatureNames = new List<string> { "ae;`" };
			((PhoneInfo)_phoneCache["e;`,"]).AFeatureNames = new List<string> { "ae;`" };
			Assert.AreEqual("(a;`|e;`,|a`|e`,|e;`|e`)", _parser.Parse("[ae;`]", false, new List<string> { ";", "," }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoredSSegsAndDiacritics_ReturnsQualifiedPhones()
		{
			_phoneCache["a~="] = new PhoneInfo { Phone = "a~=", CharType = IPASymbolType.vowel };
			_phoneCache["e;`,"] = new PhoneInfo { Phone = "e;`,", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a~="]).AFeatureNames = new List<string> { "ae~=" };
			((PhoneInfo)_phoneCache["e;`,"]).AFeatureNames = new List<string> { "ae;`" };
			Assert.AreEqual("(a~=|e;`,|a=|a~|a|e`,|e;`|e`)", _parser.Parse("{[ae~=],[ae;`]}", true, new List<string> { ";", "," }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_MixedIgnoredSSegsIncludesWithNonBase_ReturnedPatternDoesNotIncludeIgnoredBase()
		{
			_phoneCache["X"] = new PhoneInfo { Phone = "X", CharType = IPASymbolType.suprasegmental };
			_phoneCache["a~="] = new PhoneInfo { Phone = "a~=", CharType = IPASymbolType.vowel };
			_phoneCache["e;`,"] = new PhoneInfo { Phone = "e;`,", CharType = IPASymbolType.vowel };
			((PhoneInfo)_phoneCache["a~="]).AFeatureNames = new List<string> { "ae~=" };
			((PhoneInfo)_phoneCache["e;`,"]).AFeatureNames = new List<string> { "ae;`" };
			Assert.AreEqual("(a~=|e;`,|a=|a~|a|e`,|e;`|e`)", _parser.Parse("{[ae~=],[ae;`]}", true, new List<string> { ";", ",", "X" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoredBaseSSegsWithTwoRegExGroups_ReturnedIgnoredSSegsPatternBetweenOthers()
		{
			_phoneCache["X"] = new PhoneInfo { Phone = "X", CharType = IPASymbolType.suprasegmental };
			_phoneCache["Z"] = new PhoneInfo { Phone = "Z", CharType = IPASymbolType.suprasegmental };
			Assert.AreEqual("(a|e|i)(Z|X)?(o|u)", _parser.Parse("[aei][ou]", true, new List<string> { "Z", ";", ",", "X" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_IgnoredBaseMultiCodepointSSegs_ReturnedIgnoredSSegsPatternBetweenOthers()
		{
			_phoneCache["OO"] = new PhoneInfo { Phone = "OO", CharType = IPASymbolType.suprasegmental };
			_phoneCache["Z"] = new PhoneInfo { Phone = "Z", CharType = IPASymbolType.suprasegmental };
			Assert.AreEqual("(a|e|i)(Z|OO)?(o|u)", _parser.Parse("[aei][ou]", true, new List<string> { "Z", ";", ",", "OO" }));
		}

		#endregion
	}
}
