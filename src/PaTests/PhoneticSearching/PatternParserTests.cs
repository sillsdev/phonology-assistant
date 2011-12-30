using System;
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
		
			//App.IPASymbolCache.Add('~', new IPASymbol { Literal = "~", IsBase = false, Type = IPASymbolType.diacritic });
			//App.IPASymbolCache.Add('=', new IPASymbol { Literal = "=", IsBase = false, Type = IPASymbolType.diacritic });
			//App.IPASymbolCache.Add(';', new IPASymbol { Literal = ";", IsBase = false, Type = IPASymbolType.suprasegmental });
			//App.IPASymbolCache.Add('`', new IPASymbol { Literal = "`", IsBase = false, Type = IPASymbolType.suprasegmental });
			//App.IPASymbolCache.Add(',', new IPASymbol { Literal = ",", IsBase = false, Type = IPASymbolType.suprasegmental });
			//App.IPASymbolCache.Add('X', new IPASymbol { Literal = "X", IsBase = true, Type = IPASymbolType.suprasegmental });
			//App.IPASymbolCache.Add('Z', new IPASymbol { Literal = "Z", IsBase = true, Type = IPASymbolType.suprasegmental });
			//App.IPASymbolCache.Add(-100, new IPASymbol { Literal = "OO", IsBase = true, Type = IPASymbolType.suprasegmental });
			
			App.BFeatureCache.Clear();
			App.BFeatureCache.LoadFromList(new[]
			{
				new Feature { Name = "+high" },
				new Feature { Name = "+dorsal" },
				new Feature { Name = "+dors" },
				new Feature { Name = "+voice" },
				new Feature { Name = "+cons" },
				new Feature { Name = "+vcd" },
				new Feature { Name = "+front" },
				new Feature { Name = "+round" }
			});
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_prj.SearchClasses.Clear();
			_parser = new PatternParser(_prj);
			App.DottedCircle = "0";
			App.DottedCircleC = '0';
			App.DiacriticPlaceholder = "[0]";

			//_phoneCache = _prj.PhoneCache;
			
			//_phoneCache.Clear();
			//_phoneCache["b"] = new PhoneInfo { Phone = "b", CharType = IPASymbolType.consonant };
			//_phoneCache["c"] = new PhoneInfo { Phone = "c", CharType = IPASymbolType.consonant };
			//_phoneCache["d"] = new PhoneInfo { Phone = "d", CharType = IPASymbolType.consonant };
			//_phoneCache["f"] = new PhoneInfo { Phone = "f", CharType = IPASymbolType.consonant };
			//_phoneCache["g"] = new PhoneInfo { Phone = "g", CharType = IPASymbolType.consonant };
			//_phoneCache["h"] = new PhoneInfo { Phone = "h", CharType = IPASymbolType.consonant };
			//_phoneCache["j"] = new PhoneInfo { Phone = "j", CharType = IPASymbolType.consonant };
			//_phoneCache["k"] = new PhoneInfo { Phone = "k", CharType = IPASymbolType.consonant };
			//_phoneCache["l"] = new PhoneInfo { Phone = "l", CharType = IPASymbolType.consonant };
			//_phoneCache["m"] = new PhoneInfo { Phone = "m", CharType = IPASymbolType.consonant };
			//_phoneCache["n"] = new PhoneInfo { Phone = "n", CharType = IPASymbolType.consonant };
			//_phoneCache["p"] = new PhoneInfo { Phone = "p", CharType = IPASymbolType.consonant };
			//_phoneCache["r"] = new PhoneInfo { Phone = "r", CharType = IPASymbolType.consonant };
			//_phoneCache["s"] = new PhoneInfo { Phone = "s", CharType = IPASymbolType.consonant };
			//_phoneCache["t"] = new PhoneInfo { Phone = "t", CharType = IPASymbolType.consonant };
			//_phoneCache["v"] = new PhoneInfo { Phone = "v", CharType = IPASymbolType.consonant };
			//_phoneCache["w"] = new PhoneInfo { Phone = "w", CharType = IPASymbolType.consonant };
			//_phoneCache["x"] = new PhoneInfo { Phone = "x", CharType = IPASymbolType.consonant };
			//_phoneCache["y"] = new PhoneInfo { Phone = "y", CharType = IPASymbolType.consonant };
			//_phoneCache["z"] = new PhoneInfo { Phone = "z", CharType = IPASymbolType.consonant };

			//_phoneCache["a"] = new PhoneInfo { Phone = "a", CharType = IPASymbolType.vowel };
			//_phoneCache["e"] = new PhoneInfo { Phone = "e", CharType = IPASymbolType.vowel };
			//_phoneCache["i"] = new PhoneInfo { Phone = "i", CharType = IPASymbolType.vowel };
			//_phoneCache["o"] = new PhoneInfo { Phone = "o", CharType = IPASymbolType.vowel };
			//_phoneCache["u"] = new PhoneInfo { Phone = "u", CharType = IPASymbolType.vowel };

			//_phoneCache["."] = new PhoneInfo { Phone = ".", CharType = IPASymbolType.suprasegmental };
			//_phoneCache["'"] = new PhoneInfo { Phone = "'", CharType = IPASymbolType.suprasegmental };

			//App.AFeatureCache.Clear();
			//App.AFeatureCache.LoadFromList(new[]
			//{
			//    new Feature { Name = "con" },
			//    new Feature { Name = "vow" },
			//    new Feature { Name = "aei" },
			//    new Feature { Name = "ou" },
			//    new Feature { Name = "tdb" },
			//    new Feature { Name = "xyz" },
			//    new Feature { Name = "justu" },
			//    new Feature { Name = "ta" },
			//    new Feature { Name = "ae~" },
			//    new Feature { Name = "ae~=" },
			//    new Feature { Name = "ae`" },
			//    new Feature { Name = "ae;`" }
			//});

			//_prj.BFeatureCache.Clear();
			//_prj.BFeatureCache.LoadFromList(new[]
			//{
			//    new Feature { Name = "+con" },
			//    new Feature { Name = "+vow" },
			//    new Feature { Name = "+aei" },
			//    new Feature { Name = "+ou" },
			//    new Feature { Name = "+tdb" },
			//    new Feature { Name = "+xyz" }
			//});

			//((PhoneInfo)_phoneCache["t"]).BFeatureNames = new List<string> { "+con", "+tdb", "-xyz", "-vow" };
			//((PhoneInfo)_phoneCache["d"]).BFeatureNames = new List<string> { "+con", "+tdb", "-xyz", "-vow" };
			//((PhoneInfo)_phoneCache["b"]).BFeatureNames = new List<string> { "+con", "+tdb", "-xyz", "-vow" };
			//((PhoneInfo)_phoneCache["x"]).BFeatureNames = new List<string> { "+con", "+xyz", "-tdb", "-vow" };
			//((PhoneInfo)_phoneCache["y"]).BFeatureNames = new List<string> { "+con", "+xyz", "-tdb", "-vow" };
			//((PhoneInfo)_phoneCache["z"]).BFeatureNames = new List<string> { "+con", "+xyz", "-tdb", "-vow" };

			//((PhoneInfo)_phoneCache["a"]).BFeatureNames = new List<string> { "-con", "+aei", "-ou", "+vow" };
			//((PhoneInfo)_phoneCache["e"]).BFeatureNames = new List<string> { "-con", "+aei", "-ou", "+vow" };
			//((PhoneInfo)_phoneCache["i"]).BFeatureNames = new List<string> { "-con", "+aei", "-ou", "+vow" };
			//((PhoneInfo)_phoneCache["o"]).BFeatureNames = new List<string> { "-con", "+ou", "-aei", "+vow" };
			//((PhoneInfo)_phoneCache["u"]).BFeatureNames = new List<string> { "-con", "+ou", "-aei", "+vow" };

			//((PhoneInfo)_phoneCache["t"]).AFeatureNames = new List<string> { "con", "tdb", "ta" };
			//((PhoneInfo)_phoneCache["d"]).AFeatureNames = new List<string> { "con", "tdb" };
			//((PhoneInfo)_phoneCache["b"]).AFeatureNames = new List<string> { "con", "tdb" };
			//((PhoneInfo)_phoneCache["x"]).AFeatureNames = new List<string> { "con", "xyz" };
			//((PhoneInfo)_phoneCache["y"]).AFeatureNames = new List<string> { "con", "xyz" };
			//((PhoneInfo)_phoneCache["z"]).AFeatureNames = new List<string> { "con", "xyz" };

			//((PhoneInfo)_phoneCache["a"]).AFeatureNames = new List<string> { "aei", "vow", "ta" };
			//((PhoneInfo)_phoneCache["e"]).AFeatureNames = new List<string> { "aei", "vow" };
			//((PhoneInfo)_phoneCache["i"]).AFeatureNames = new List<string> { "aei", "vow" };
			//((PhoneInfo)_phoneCache["o"]).AFeatureNames = new List<string> { "ou",  "vow" };
			//((PhoneInfo)_phoneCache["u"]).AFeatureNames = new List<string> { "ou",  "vow", "justu" };
		}

		#region ReplaceBracketedClassNamesWithPattern tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_ClassDoesNotExist_ThrowsException()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "dummy1", Type = SearchClassType.Phones });
			_prj.SearchClasses.Add(new SearchClass { Name = "dummy2", Type = SearchClassType.Phones });
			Assert.Throws<NullReferenceException>(() => _parser.ReplaceBracketedClassNamesWithPatterns("<not there>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContainsOnlyPhoneClass_ReturnsClassPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "nop", Type = SearchClassType.Phones, Pattern = "{n,o,p}" });
			Assert.AreEqual("{n,o,p}", _parser.ReplaceBracketedClassNamesWithPatterns("<nop>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContainsOnlyAFeatureClass_ReturnsClassPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "xyzORou", Type = SearchClassType.Articulatory, Pattern = "{[xyz],[ou]}" });
			Assert.AreEqual("{[xyz],[ou]}", _parser.ReplaceBracketedClassNamesWithPatterns("<xyzORou>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContainsOnlyBFeatureClass_ReturnsClassPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "+vowAND-ou", Type = SearchClassType.Binary, Pattern = "[[+vow][-ou]]" });
			Assert.AreEqual("[[+vow][-ou]]", _parser.ReplaceBracketedClassNamesWithPatterns("<+vowAND-ou>"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ReplaceBracketedClassNamesWithPattern_InputPatternContains2Classes_ReturnsModifiedPattern()
		{
			_prj.SearchClasses.Add(new SearchClass { Name = "nop", Type = SearchClassType.Phones, Pattern = "{n,o,p}" });
			_prj.SearchClasses.Add(new SearchClass { Name = "+vowAND-ou", Type = SearchClassType.Binary, Pattern = "[[+vow][-ou]]" });
			Assert.AreEqual("[V]{[[+vow][-ou]],{n,o,p}}[C]", _parser.ReplaceBracketedClassNamesWithPatterns("[V]{<+vowAND-ou>,<nop>}[C]"));
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a test of a
		/// complex pattern with nesting, ANDing and ORing. The tested pattern is:
		///
		/// "[ [ {a,e} { [+high][DENTAL] } ] [-dOrSaL] ]"
		/// 
		/// The group and member hierarchy should be as follows:
		/// 
		/// group = [[{a,e}{[+high][[dental]}][-dorsal]]
		///    |
		///    +-- nestedGroup = [{a,e}{[+high][[dental]}]
		///    |     |
		///    |     +-- nestedGroup1 = {a,e}
		///    |     |     |
		///    |     |     +-- Pattern Member 'a'
		///    |     |     +-- Pattern Member 'e'
		///    |     |
		///    |     +-- nestedGroup2 {[+high][[dental]}
		///    |           |
		///    |           +-- Pattern Member [+high]
		///    |           +-- Pattern Member [dental]
		///    |
		///    +-- Pattern Member [-dorsal]
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_Complex()
		{
			var group = _parser.Parse("[[{a,e}{[+high],[dental]}][-dorsal]]", EnvironmentType.After);

			Assert.IsNotNull(group);
			Assert.AreEqual(group.GroupType, GroupType.And);
			Assert.AreEqual(2, group.Members.Count);

			// Verify the second member of the group.
			var groupMember = group.Members[1] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Binary, groupMember.MemberType);
			Assert.AreEqual("-dorsal", groupMember.Member);
			FeatureMask mask = App.BFeatureCache.GetMask("-dorsal");
			Assert.IsTrue(groupMember.BMask.ContainsOneOrMore(mask));

			// Nested within the first group should be 2 other groups.
			PatternGroup nestedGroup = group.Members[0] as PatternGroup;
			Assert.IsNotNull(nestedGroup);
			Assert.AreEqual(2, nestedGroup.Members.Count);
			Assert.AreEqual(GroupType.And, nestedGroup.GroupType);

			// Verify the first group nested in the nested group.
			var nestedGroup1 = nestedGroup.Members[0] as PatternGroup;
			Assert.IsNotNull(nestedGroup1);
			Assert.AreEqual(2, nestedGroup1.Members.Count);
			Assert.AreEqual(GroupType.Or, nestedGroup1.GroupType);

			// Verify the first member of the nested, nested group.
			groupMember = nestedGroup1.Members[0] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.SinglePhone, groupMember.MemberType);
			Assert.AreEqual("a", groupMember.Member);

			// Verify the second member of the nested, nested group.
			groupMember = nestedGroup1.Members[1] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.SinglePhone, groupMember.MemberType);
			Assert.AreEqual("e", groupMember.Member);

			// Verify the second group nested in the nested group.
			var nestedGroup2 = nestedGroup.Members[1] as PatternGroup;
			Assert.IsNotNull(nestedGroup2);
			Assert.AreEqual(2, nestedGroup2.Members.Count);
			Assert.AreEqual(GroupType.Or, nestedGroup2.GroupType);

			// Verify the first member of the nested, nested group.
			groupMember = nestedGroup2.Members[0] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Binary, groupMember.MemberType);
			Assert.AreEqual("+high", groupMember.Member);
			mask = App.BFeatureCache.GetMask("+high");
			Assert.IsTrue(groupMember.BMask.ContainsOneOrMore(mask));

			// Verify the second member of the nested, nested group.
			groupMember = nestedGroup2.Members[1] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Articulatory, groupMember.MemberType);
			Assert.AreEqual("dental", groupMember.Member);
			mask = App.AFeatureCache.GetMask("dental");
			Assert.IsTrue(groupMember.AMask.ContainsOneOrMore(mask));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a simple test
		/// with only AND'd features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_FeaturesOnlyAND()
		{
			VerifyGroup_1(_parser.Parse("[[+high][dental]]", EnvironmentType.After), true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a simple test
		/// with only OR'd features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTests_FeaturesOnlyOR()
		{
			VerifyGroup_1(_parser.Parse("{[+high],[dental]}", EnvironmentType.After), false);
		}

		/// ------------------------------------------------------------------------------------
		private static void VerifyGroup_1(PatternGroup group, bool andMembers)
		{
			Assert.IsNotNull(group);
			Assert.AreEqual((andMembers ? GroupType.And : GroupType.Or), group.GroupType);
			Assert.AreEqual(2, group.Members.Count);

			var groupMember = group.Members[0] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Binary, groupMember.MemberType);
			Assert.AreEqual("+high", groupMember.Member);
			FeatureMask mask = App.BFeatureCache.GetMask("+high");
			Assert.IsTrue(groupMember.BMask.ContainsOneOrMore(mask));

			groupMember = group.Members[1] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Articulatory, groupMember.MemberType);
			Assert.AreEqual("dental", groupMember.Member);

			// Check the ToString just for kicks.
			Assert.AreEqual((andMembers ? "[[+high][dental]]" : "{[+high],[dental]}"), group.ToString());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_AspiratedConsonant_ReturnsCorrectGroup()
		{
			// Parse any aspirated consonant
			var group = _parser.Parse("[[C][0\u02B0]]", EnvironmentType.Item);
			Assert.AreEqual(1, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.AreEqual("\u02B0", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
			Assert.AreEqual(MemberType.AnyConsonant, ((PatternGroupMember)group.Members[0]).MemberType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_AspiratedVowel_ReturnsCorrectGroup()
		{
			// Parse any aspirated vowel
			var group = _parser.Parse("[[V][0\u02B0]]", EnvironmentType.Item);
			Assert.AreEqual(1, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.AreEqual("\u02B0", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
			Assert.AreEqual(MemberType.AnyVowel, ((PatternGroupMember)group.Members[0]).MemberType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_ModifyFeatureWithDiacriticPlaceholder_ReturnsCorrectGroup()
		{
			// Parse any aspirated consonant
			var group = _parser.Parse("[[+front][0\u02B0+]]", EnvironmentType.Item);
			Assert.AreEqual(1, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.AreEqual("\u02B0+", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
			Assert.AreEqual(MemberType.Binary, ((PatternGroupMember)group.Members[0]).MemberType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_PhoneWithDiacritic_ReturnsCorrectGroup()
		{
			var group = _parser.Parse("{a,b\u02B0}", EnvironmentType.Item);
			Assert.AreEqual(2, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.IsTrue(group.Members[1] is PatternGroupMember);
			Assert.AreEqual("a", ((PatternGroupMember)group.Members[0]).Member);
			Assert.AreEqual("b", ((PatternGroupMember)group.Members[1]).Member);
			Assert.AreEqual("\u02B0", ((PatternGroupMember)group.Members[1]).DiacriticPattern);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_PhoneWithDiacriticPlaceholderAndZeroOrMore_ReturnsCorrectGroup()
		{
			var group = _parser.Parse("{a,[b[0\u02B0*]]}", EnvironmentType.Item);
			Assert.AreEqual("a", ((PatternGroupMember)group.Members[0]).Member);

			group = (PatternGroup)group.Members[1];
			Assert.AreEqual(GroupType.And, group.GroupType);
			Assert.AreEqual("b", ((PatternGroupMember)group.Members[0]).Member);
			Assert.AreEqual("\u02B0*", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_PhoneWithDiacriticPlaceholderAndOneOrMore_ReturnsCorrectGroup()
		{
			var group = _parser.Parse("{a,[b[0\u02B0+]]}", EnvironmentType.Item);
			Assert.AreEqual("a", ((PatternGroupMember)group.Members[0]).Member);

			group = (PatternGroup)group.Members[1];
			Assert.AreEqual(GroupType.And, group.GroupType);
			Assert.AreEqual("b", ((PatternGroupMember)group.Members[0]).Member);
			Assert.AreEqual("\u02B0+", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that parsing of 2 OR groups nested in another OR group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_NestedOrGroups_ReturnsCorrectGroup()
		{
			var group = _parser.Parse("{{a,b},{c,d}}", EnvironmentType.Item);
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.Or, group.GroupType);

			// group {a,b}
			var grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			var member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("a", member.Member);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("b", member.Member);

			// group {c,d}
			grp = group.Members[1] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("c", member.Member);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("d", member.Member);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that parsing of 2 OR groups nested in an AND group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Parse_OrGroupsNestedInAndGroup_ReturnsCorrectGroup()
		{
			var group = _parser.Parse("[{a,b}{[+high],[nasal]}]", EnvironmentType.Item);
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.And, group.GroupType);

			// group {a,b}
			var grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			var member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("a", member.Member);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("b", member.Member);

			// group {+high,nasal}
			grp = group.Members[1] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("+high", member.Member);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("nasal", member.Member);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests parsing 2 OR groups nested in another OR group, followed by a
		/// diacritic placeholder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_NestedOrGroupWithDiacriticPlaceholder1()
		{
			var group = _parser.Parse("[{{a,b},{c,d}}[0~]]", EnvironmentType.Item);
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.Or, group.GroupType);

			// group {a,b}
			var grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			var member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("a", member.Member);
			Assert.AreEqual("~", member.DiacriticPattern);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("b", member.Member);
			Assert.AreEqual("~", member.DiacriticPattern);

			// group {c,d}
			grp = group.Members[1] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("c", member.Member);
			Assert.AreEqual("~", member.DiacriticPattern);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("d", member.Member);
			Assert.AreEqual("~", member.DiacriticPattern);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests parsing 2 OR groups nested in another OR group (with one of the OR groups
		/// having a diacritic placeholder), followed by a diacritic placeholder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_NestedOrGroupWithDiacriticPlaceholder2()
		{
			var group = _parser.Parse("[{[{a,b}[0^]],{c,d}}[0~]]", EnvironmentType.Item);
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.Or, group.GroupType);

			// group {a,b}
			var grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			var member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("a", member.Member);
			Assert.AreEqual("^~", member.DiacriticPattern);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("b", member.Member);
			Assert.AreEqual("^~", member.DiacriticPattern);

			// group {c,d}
			grp = group.Members[1] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("c", member.Member);
			Assert.AreEqual("~", member.DiacriticPattern);

			member = grp.Members[1] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("d", member.Member);
			Assert.AreEqual("~", member.DiacriticPattern);
		}




		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests parsing 2 groups nested in another OR group (with one of the OR groups
		///// having a diacritic placeholder and one of the members in the OR group having
		///// a diacritic placeholder), followed by a diacritic placeholder.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void PatternGroupTest_NestedOrGroupWithDiacriticPlaceholder3()
		//{
			//var group = _parser.Parse("[{[{[a[0']],b}[0^]],{c,d}}[0~]]", EnvironmentType.Item);
			//Assert.AreEqual(2, group.Members.Count);
			//Assert.AreEqual(GroupType.Or, group.GroupType);

			//// group {a,b}
			//var grp = group.Members[0] as PatternGroup;
			//Assert.IsNotNull(grp);
			//Assert.AreEqual(2, grp.Members.Count);
			//Assert.AreEqual(GroupType.Or, grp.GroupType);

			//Assert.AreEqual(GroupType.And, ((PatternGroup)grp.Members[0]).GroupType);
			//Assert.AreEqual("^", ((PatternGroup)grp.Members[0]).DiacriticPattern);
			//Assert.AreEqual("a", ((PatternGroupMember)((PatternGroup)grp.Members[0]).Members[0]).Member);
			//Assert.AreEqual("'", ((PatternGroupMember)((PatternGroup)grp.Members[0]).Members[0]).DiacriticPattern);
			
			//var member = group.Members[1] as PatternGroupMember;
			//Assert.IsNotNull(member);
			//Assert.AreEqual("b", member.Member);
			//Assert.AreEqual("^~", member.DiacriticPattern);

			//// group {c,d}
			//grp = group.Members[1] as PatternGroup;
			//Assert.IsNotNull(grp);
			//Assert.AreEqual(2, grp.Members.Count);
			//Assert.AreEqual(GroupType.Or, grp.GroupType);

			//member = grp.Members[0] as PatternGroupMember;
			//Assert.IsNotNull(member);
			//Assert.AreEqual("c", member.Member);
			//Assert.AreEqual("~", member.DiacriticPattern);

			//member = grp.Members[1] as PatternGroupMember;
			//Assert.IsNotNull(member);
			//Assert.AreEqual("d", member.Member);
			//Assert.AreEqual("~", member.DiacriticPattern);
		//}




	}
}
