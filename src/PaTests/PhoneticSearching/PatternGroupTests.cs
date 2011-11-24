using System.Collections.Generic;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;
using NUnit.Framework;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PatternGroupTests : TestBase
	{
		private PatternParser _parser;
		
		#region Setup/Teardown
		///// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
			base.FixtureSetup();

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
			_parser = new PatternParser(_prj);
			SearchEngine.PhoneCache = _prj.PhoneCache;
			App.DottedCircle = "0";
			App.DottedCircleC = '0';
			App.DiacriticPlaceholder = "[0]";

			_prj.PhoneCache["a"] = new PhoneInfo { Phone = "a", CharType = IPASymbolType.vowel };
			_prj.PhoneCache["d"] = new PhoneInfo { Phone = "d", CharType = IPASymbolType.consonant };
			((PhoneInfo)_prj.PhoneCache["d"]).BFeatureNames = new List<string> { "+high", "-cons" };
			((PhoneInfo)_prj.PhoneCache["d"]).AFeatureNames = new List<string> { "dental" };
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
				new object[] { pattern, '[', ']' }));

			pattern = "{{a,e},[[+front][+round]]";
			Assert.IsFalse(GetBoolResult(typeof(PatternGroup), "VerifyMatchingBrackets",
				new object[] { pattern, '{', '}' }));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that Parse fails when erroneous data is sent to it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void MiscParseFailureTest()
		{
			var rootGroup = new PatternGroup(EnvironmentType.After);

			var group = new PatternGroup(EnvironmentType.Item);
			Assert.IsFalse(group.Parse(string.Empty));
			Assert.IsFalse(group.Parse(null));

			SetProperty(group, "RootGroup", rootGroup);
			Assert.IsFalse(GetBoolResult(group, "PreParseProcessing", "{[ab}]"));
			Assert.IsFalse(GetBoolResult(group, "PreParseProcessing", "a[bc]"));
			Assert.IsFalse(GetBoolResult(group, "PreParseProcessing", "{ab}c"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the DelimitMembers method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DelimitMembersTest()
		{
			var group = new PatternGroup(EnvironmentType.After);

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

			pattern = string.Format("[+con][[C][{0}~+]]", App.DottedCircle);
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual(string.Format("%+con$[%C$[{0}~+]]", App.DottedCircle), result);

			pattern = string.Format("[+con][[C][{0}~*]]", App.DottedCircle);
			result = GetStrResult(group, "DelimitMembers", pattern);
			Assert.AreEqual(string.Format("%+con$[%C$[{0}~*]]", App.DottedCircle), result);
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
			var group = new PatternGroup(EnvironmentType.After);
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
			string pattern = string.Format("[[C][{0}\u02B0]]", App.DottedCircle);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());

			// Parse any aspirated vowel
			pattern = string.Format("[[V][{0}\u02B0]]", App.DottedCircle);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());

			// Parse any aspirated consonant with zero or more diacritics
			pattern = string.Format("[[C][{0}\u02B0*]]", App.DottedCircle);
			group = new PatternGroup(EnvironmentType.Item);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());

			// Parse any aspirated consonant with one or more diacritics
			pattern = string.Format("[[V][{0}\u02B0*]]", App.DottedCircle);
			group = new PatternGroup(EnvironmentType.Before);
			group.Parse(pattern);
			Assert.AreEqual(pattern, group.ToString());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchGroup_OrGroupInAndGroup_ReturnsMatch()
		{
			int ip = 0;
			var group = _parser.Parse("[{[+high],[-cons]}[C]]", EnvironmentType.After);
			Assert.AreEqual(CompareResultType.Match, group.SearchGroup(new[] { "d" }, ref ip));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchGroup_OrGroupInAndGroup_ReturnsNoMatch()
		{
			int ip = 0;
			var group = _parser.Parse("[{[-high],[+cons]}[C]]", EnvironmentType.After);
			Assert.AreEqual(CompareResultType.NoMatch, group.SearchGroup(new[] { "d" }, ref ip));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchGroup_AndGroupInOrGroup_ReturnsMatch()
		{
			int ip = 0;
			var group = _parser.Parse("{[[+high][+cons]],[dental]}", EnvironmentType.After);
			Assert.AreEqual(CompareResultType.Match, group.SearchGroup(new[] { "d" }, ref ip));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchGroup_AndGroupInAndGroup_ReturnsNoMatch()
		{
			int ip = 0;
			var group = _parser.Parse("[[[+high][+cons]][dental]]", EnvironmentType.After);
			Assert.AreEqual(CompareResultType.NoMatch, group.SearchGroup(new[] { "d" }, ref ip));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchGroup_AndGroupInAndGroup_ReturnsMatch()
		{
			int ip = 0;
			var group = _parser.Parse("[[[+high][-cons]][dental]]", EnvironmentType.After);
			Assert.AreEqual(CompareResultType.Match, group.SearchGroup(new[] { "d" }, ref ip));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SearchGroup_VowelClass_ReturnsMatch()
		{
			int ip = 0;
			var group = _parser.Parse("[V]", EnvironmentType.After);
			Assert.AreEqual(CompareResultType.Match, group.SearchGroup(new[] { "a" }, ref ip));
		}

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
