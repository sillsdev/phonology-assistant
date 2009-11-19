using NUnit.Framework;
using SIL.Pa.Data;
using SIL.Pa.TestUtils;

namespace SIL.Pa.FFSearchEngine
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PatternGroupParsingTests : TestBase
	{
		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			// This will force the cache to be built.
			if (DataUtils.IPACharCache == null)
			{
			}
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
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the VerifyMatchingBrackets method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyMatchingBracketsTest()
		{
			string pattern = "{[[+high][+cons],[[+vcd][+cons]]}";
			Assert.IsFalse(GetBoolResult(typeof(PatternGroup), "VerifyMatchingBrackets",
				new object[] {pattern, '[', ']'}));

			pattern = "{{a,e},[[+front][+round]]";
			Assert.IsFalse(GetBoolResult(typeof(PatternGroup), "VerifyMatchingBrackets",
				new object[] { pattern, '{', '}' }));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the DelimitMembers method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DelimitMembersTest()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.After);

			string pattern = "[[+high][+vcd][V]]";
			string result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("[%+high$%+vcd$%V$]", result);

			pattern = "{[[+high][+cons]][[+vcd][+cons]]}";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("{[%+high$%+cons$][%+vcd$%+cons$]}", result);

			pattern = "[{[+high][+vcd]}[+cons]]";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("[{%+high$%+vcd$}%+cons$]", result);

			pattern = "{{a,e}[[+front][+round]]}";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("{{a,e}[%+front$%+round$]}", result);

			// Test a pattern from which some brackets don't need removing.
			pattern = "[+front]{[+round][+vcd]}";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("%+front${%+round$%+vcd$}", result);

			pattern = "[[{a,e}{[+high][DENTAL]}][-dOrSaL]]";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("[[{a,e}{%+high$%DENTAL$}]%-dOrSaL$]", result);

			pattern = "[+high]abc[+con]";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("%+high$abc%+con$", result);

			pattern = "[+high]abc{[+con],[dental]}";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("%+high$abc{%+con$%dental$}", result);

			pattern = "[+high]abc[[+con],[dental]]";
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual("%+high$abc[%+con$%dental$]", result);

			pattern = string.Format("[+con][[C][{0}~+]]", DataUtils.kDottedCircle);
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual(string.Format("%+con$[%C$[{0}~+]]", DataUtils.kDottedCircle), result);

			pattern = string.Format("[+con][[C][{0}~*]]", DataUtils.kDottedCircle);
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual(string.Format("%+con$[%C$[{0}~*]]", DataUtils.kDottedCircle), result);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that Parse fails when erroneous data is sent to it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MiscParseFailureTest()
		{
			PatternGroup rootGroup = new PatternGroup(EnvironmentType.After);

			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			Assert.IsFalse(group.Parse(string.Empty));
			Assert.IsFalse(group.Parse(null));
			
			SetField(group, "m_rootGroup", rootGroup);
			Assert.IsFalse(GetBoolResult(group, "PreParseProcessing", "{[ab}]"));
			Assert.IsFalse(GetBoolResult(group, "PreParseProcessing", "a[bc]"));
			Assert.IsFalse(GetBoolResult(group, "PreParseProcessing", "{ab}c"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest1()
		{
			char dental = '\u032A';
			char aspiration = '\u02B0';
			
			PatternGroupMember member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(dental);
			member.AddToMember(DataUtils.kBottomTieBarC);
			member.AddToMember('s');
			member.DiacriticPattern = aspiration.ToString();
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", DataUtils.kBottomTieBarC), member.Member);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(dental) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(aspiration) >= 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest2()
		{
			char dental = '\u032A';
			char aspiration = '\u02B0';

			PatternGroupMember member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(DataUtils.kBottomTieBarC);
			member.AddToMember('s');
			member.AddToMember(dental);
			member.DiacriticPattern = aspiration + "+";
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", DataUtils.kBottomTieBarC), member.Member);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(dental) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(aspiration) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf('+') >= 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest3()
		{
			char dental = '\u032A';
			char aspiration = '\u02B0';

			PatternGroupMember member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(DataUtils.kTopTieBarC);
			member.AddToMember('s');
			member.AddToMember(dental);
			member.DiacriticPattern = aspiration + "*";
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", DataUtils.kTopTieBarC), member.Member);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(dental) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf(aspiration) >= 0);
			Assert.IsTrue(member.DiacriticPattern.IndexOf('*') >= 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CloseIPACharacterMember methods in PatternGroupMember.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseSinglePhoneMemberTest4()
		{
			char dental = '\u032A';
			char aspiration = '\u02B0';

			PatternGroupMember member = new PatternGroupMember();
			member.AddToMember('t');
			member.AddToMember(dental);
			member.AddToMember(DataUtils.kTopTieBarC);
			member.AddToMember('s');
			member.AddToMember(aspiration);
			member.CloseMember();

			Assert.AreEqual(string.Format("t{0}s", DataUtils.kTopTieBarC), member.Member);
			Assert.AreEqual(string.Format("{0}{1}", dental, aspiration), member.DiacriticPattern);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a simple test
		/// with only OR'd features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupToStringTest()
		{
			// Verify an OR group
			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[+high],[DENTAL]}");
			Assert.AreEqual("{[+high],[dental]}", group.ToString());

			// Verify an AND group
			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[{a,e}{[+high][DENTAL]}][-dOrS]]");
			Assert.AreEqual("[[{a,e}{[+high],[dental]}][-dors]]", group.ToString());

			// Verify a sequential group
			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{a,e}pple{[+high][DENTAL]}");
			Assert.AreEqual("{a,e}pple{[+high],[dental]}", group.ToString());

			// Verify a pattern with [C] and [V]
			group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[V]{[+high],[-voice]}[C]]");
			Assert.AreEqual("[[V]{[+high],[-voice]}[C]]", group.ToString());

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("der[+high]");
			Assert.AreEqual("der[+high]", group.ToString());

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse("der[+high]abc[dental]{[-voice][+dors]}");
			Assert.AreEqual("der[+high]abc[dental]{[-voice],[+dors]}", group.ToString());

			group = new PatternGroup(EnvironmentType.Before);
			group.Parse("#{[-voice],[+dors]}");
			Assert.AreEqual("#{[-voice],[+dors]}", group.ToString());

			group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[-voice],[+dors]}#");
			Assert.AreEqual("{[-voice],[+dors]}#", group.ToString());

			// Parse any aspirated consonant
			string pattern = string.Format("[[C][{0}\u02B0]]", DataUtils.kDottedCircle);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());

			// Parse any aspirated vowel
			pattern = string.Format("[[V][{0}\u02B0]]", DataUtils.kDottedCircle);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());

			// Parse any aspirated consonant with zero or more diacritics
			pattern = string.Format("[[C][{0}\u02B0*]]", DataUtils.kDottedCircle);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());

			// Parse any aspirated consonant with one or more diacritics
			pattern = string.Format("[[V][{0}\u02B0*]]", DataUtils.kDottedCircle);
			group = new PatternGroup(EnvironmentType.Before);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());
		}

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
			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[{a,e}{[+high][DENTAL]}][-dOrS]]");

			Assert.IsNotNull(group);
			Assert.AreEqual(group.GroupType, GroupType.And);
			Assert.AreEqual(2, group.Members.Count);

			// Verify the second member of the group.
			PatternGroupMember groupMember = group.Members[1] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Binary, groupMember.MemberType);
			Assert.AreEqual("-dors", groupMember.Member);
			FeatureMask mask = DataUtils.BFeatureCache.GetMask("-dors");
			Assert.IsTrue(groupMember.BMask.AndResult(mask));

			// Nested within the first group should be 2 other groups.
			PatternGroup nestedGroup = group.Members[0] as PatternGroup;
			Assert.IsNotNull(nestedGroup);
			Assert.AreEqual(2, nestedGroup.Members.Count);
			Assert.AreEqual(GroupType.And, nestedGroup.GroupType);

			// Verify the first group nested in the nested group.
			PatternGroup nestedGroup1 = nestedGroup.Members[0] as PatternGroup;
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
			PatternGroup nestedGroup2 = nestedGroup.Members[1] as PatternGroup;
			Assert.IsNotNull(nestedGroup2);
			Assert.AreEqual(2, nestedGroup2.Members.Count);
			Assert.AreEqual(GroupType.Or, nestedGroup2.GroupType);

			// Verify the first member of the nested, nested group.
			groupMember = nestedGroup2.Members[0] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Binary, groupMember.MemberType);
			Assert.AreEqual("+high", groupMember.Member);
			mask = DataUtils.BFeatureCache.GetMask("+high");
			Assert.IsTrue(groupMember.BMask.AndResult(mask));

			// Verify the second member of the nested, nested group.
			groupMember = nestedGroup2.Members[1] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Articulatory, groupMember.MemberType);
			Assert.AreEqual("dental", groupMember.Member);
			mask = DataUtils.AFeatureCache.GetMask("dental");
			Assert.IsTrue(groupMember.AMask.AndResult(mask));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a simple test
		/// with only AND'd features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_FeaturesOnlyAND_1()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("[[+high][DENTAL]]");
			VerifyGroup_1(group, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a simple test
		/// with only AND'd features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_FeaturesOnlyAND_2()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.After);
			// Include a comma between features. This should not be considered an OR
			group.Parse("[[+high],[DENTAL]]");
			VerifyGroup_1(group, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a simple test
		/// with only OR'd features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTests_FeaturesOnlyOR_1()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.After);
			group.Parse("{[+high],[DENTAL]}");
			VerifyGroup_1(group, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PatternGroup and PatternGroupMember classes (via the PatternParser class)
		/// to make sure patterns are parsed and stored correctly. This test is a simple test
		/// with only OR'd features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTests_FeaturesOnlyOR_2()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.After);
			// Don't include the comma between features. This should not be considered and AND.
			group.Parse("{[+high][DENTAL]}");
			VerifyGroup_1(group, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void VerifyGroup_1(PatternGroup group, bool andMembers)
		{
			Assert.IsNotNull(group);
			Assert.AreEqual((andMembers ? GroupType.And : GroupType.Or), group.GroupType);
			Assert.AreEqual(2, group.Members.Count);

			PatternGroupMember groupMember = group.Members[0] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Binary, groupMember.MemberType);
			Assert.AreEqual("+high", groupMember.Member);
			FeatureMask mask = DataUtils.BFeatureCache.GetMask("+high");
			Assert.IsTrue(groupMember.BMask.AndResult(mask));

			groupMember = group.Members[1] as PatternGroupMember;
			Assert.IsNotNull(groupMember);
			Assert.AreEqual(MemberType.Articulatory, groupMember.MemberType);
			Assert.AreEqual("dental", groupMember.Member);

			// Check the ToString just for kicks.
			Assert.AreEqual((andMembers ? "[[+high][dental]]" : "{[+high],[dental]}"), group.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that parsing modifying diacritics on consonants and vowels works.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_VowConWithModifier()
		{
			// Parse any aspirated consonant
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[C][{0}\u02B0]]", DataUtils.kDottedCircle));
			Assert.AreEqual(1, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.AreEqual("\u02B0", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
			Assert.AreEqual(MemberType.AnyConsonant,
				((PatternGroupMember)group.Members[0]).MemberType);

			// Parse any aspirated vowel
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[V][{0}\u02B0]]", DataUtils.kDottedCircle));
			Assert.AreEqual(1, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.AreEqual("\u02B0", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
			Assert.AreEqual(MemberType.AnyVowel,
				((PatternGroupMember)group.Members[0]).MemberType);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that parsing modifying diacritics on a feature member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_FeatureModifier()
		{
			// Parse any aspirated consonant
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("[[+stop][{0}\u02B0+]]", DataUtils.kDottedCircle));
			Assert.AreEqual(1, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.AreEqual("\u02B0+", ((PatternGroupMember)group.Members[0]).DiacriticPattern);
			Assert.AreEqual(MemberType.Binary, ((PatternGroupMember)group.Members[0]).MemberType);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that parsing modifying diacritics on IPA characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_IPACharModifier()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("{a,b\u02B0}");
			Assert.AreEqual(2, group.Members.Count);
			Assert.IsTrue(group.Members[0] is PatternGroupMember);
			Assert.IsTrue(group.Members[1] is PatternGroupMember);
			Assert.AreEqual("a", ((PatternGroupMember)group.Members[0]).Member);
			Assert.AreEqual("b", ((PatternGroupMember)group.Members[1]).Member);
			Assert.AreEqual("\u02B0", ((PatternGroupMember)group.Members[1]).DiacriticPattern);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("{{a,b[{0}\u02B0*]}}", DataUtils.kDottedCircle));
			Assert.AreEqual("a", ((PatternGroupMember)group.Members[0]).Member);
			Assert.AreEqual("b", ((PatternGroupMember)group.Members[1]).Member);
			Assert.AreEqual("\u02B0*", ((PatternGroupMember)group.Members[1]).DiacriticPattern);

			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(string.Format("{{a,b[{0}\u02B0+]}}", DataUtils.kDottedCircle));
			Assert.AreEqual("a", ((PatternGroupMember)group.Members[0]).Member);
			Assert.AreEqual("b", ((PatternGroupMember)group.Members[1]).Member);
			Assert.AreEqual("\u02B0+", ((PatternGroupMember)group.Members[1]).DiacriticPattern);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that parsing of 2 OR groups nested in another OR group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_NestedOrGroup()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("{{a,b}{c,d}}");
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.Or, group.GroupType);

			// group {a,b}
			PatternGroup grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			PatternGroupMember member = grp.Members[0] as PatternGroupMember;
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
		public void PatternGroupTest_NestedAndGroup()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			group.Parse("[{a,b}{[+high][nasal]}]");
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.And, group.GroupType);

			// group {a,b}
			PatternGroup grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			PatternGroupMember member = grp.Members[0] as PatternGroupMember;
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
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			string pattern = "{{a,b}{c,d}}[0~]";
			pattern = pattern.Replace('0', DataUtils.kDottedCircleC);
			group.Parse(pattern);
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.Or, group.GroupType);

			// group {a,b}
			PatternGroup grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			PatternGroupMember member = grp.Members[0] as PatternGroupMember;
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
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			string pattern = "{{a,b}[0^]{c,d}}[0~]";
			pattern = pattern.Replace('0', DataUtils.kDottedCircleC);
			group.Parse(pattern);
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.Or, group.GroupType);

			// group {a,b}
			PatternGroup grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			PatternGroupMember member = grp.Members[0] as PatternGroupMember;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests parsing 2 groups nested in another OR group (with one of the OR groups
		/// having a diacritic placeholder and one of the members in the OR group having
		/// a diacritic placeholder), followed by a diacritic placeholder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternGroupTest_NestedOrGroupWithDiacriticPlaceholder3()
		{
			PatternGroup group = new PatternGroup(EnvironmentType.Item);
			string pattern = "{{a[0'],b}[0^]{c,d}}[0~]";
			pattern = pattern.Replace('0', DataUtils.kDottedCircleC);
			group.Parse(pattern);
			Assert.AreEqual(2, group.Members.Count);
			Assert.AreEqual(GroupType.Or, group.GroupType);

			// group {a,b}
			PatternGroup grp = group.Members[0] as PatternGroup;
			Assert.IsNotNull(grp);
			Assert.AreEqual(2, grp.Members.Count);
			Assert.AreEqual(GroupType.Or, grp.GroupType);

			PatternGroupMember member = grp.Members[0] as PatternGroupMember;
			Assert.IsNotNull(member);
			Assert.AreEqual("a", member.Member);
			Assert.AreEqual("'^~", member.DiacriticPattern);

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
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void DelimitOrGroupTest1()
		//{
		//    Assert.AreEqual("{([C][V]),[V]}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers", "{[C][V],[V]}"));

		//    Assert.AreEqual("{([C][V]),[C]}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers", "{[C][V],[C]}"));

		//    Assert.AreEqual("{[V],([C][V])}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers", "{[V],[C][V]}"));

		//    Assert.AreEqual("{[C],([C][V])}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers", "{[C],[C][V]}"));

		//    Assert.AreEqual("{([V][C]),(abc[C][V])}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers", "{[V][C],abc[C][V]}"));

		//    Assert.AreEqual("{([C]abc[V]),([C][V])}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers", "{[C]abc[V],[C][V]}"));
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void DelimitOrGroupTest2()
		//{
		//    Assert.AreEqual("{([C][V]),{(ab),(cd),(ef),{(xy),(yz)},[V]},(mn)}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers",
		//        "{[C][V],{ab,cd,ef,{xy,yz},[V]},mn}"));

		//    Assert.AreEqual("{{([C][V]),[V]},{(ab),(cd),(ef)}}",
		//        GetStrResult(typeof(PatternGroup), "DelimitOrGroupMembers",
		//        "{{[C][V],[V]},{ab,cd,ef}}"));
		//}
	}
}
