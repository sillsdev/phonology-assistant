using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PatternMemberTests : TestBase
	{
		/// ------------------------------------------------------------------------------------
		public override void FixtureSetup()
		{
			base.FixtureSetup();

			App.IPASymbolCache.Add('$', new IPASymbol { Literal = "$", IsBase = true, Type = IPASymbolType.suprasegmental });
			App.IPASymbolCache.Add('%', new IPASymbol { Literal = "%", IsBase = true, Type = IPASymbolType.suprasegmental });
		}

		///// ------------------------------------------------------------------------------------
		//[SetUp]
		//public void TestSetup()
		//{
		//}

		/// ------------------------------------------------------------------------------------
		private PatternMember CreateMember(PatternPart part, string wildcardSymbol)
		{
			var member = new PatternMember(part, false, new[] { "$", "%" });
			member.AddPhoneGroup(new[] { "a", "e", "i", "o", "u" });
			member.FinalizeParse(wildcardSymbol ?? string.Empty, _prj.PhoneticParser);
			return member;
		}

		/// ------------------------------------------------------------------------------------
		private bool GetMatchInEnvironment(PatternMember member, string phones)
		{
			return member.GetEnvironmentMatch(_prj.PhoneticParser.Parse(phones, true) ?? new string[0]);
		}
		
		#region GetEnvironmentMatch tests (for preceding environment)
		/// ------------------------------------------------------------------------------------
		private bool GetMatchResultInPrecedingEnv(string wildcardSymbol, string phones)
		{
			return GetMatchInEnvironment(
				CreateMember(PatternPart.Preceding, wildcardSymbol), phones);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetEnvironmentMatch_HasZeroOrMoreSymbol_AlwaysReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv("*", "abcd"));
			Assert.IsTrue(GetMatchResultInPrecedingEnv("*", ""));
			Assert.IsTrue(GetMatchResultInPrecedingEnv("*", "bacd"));

			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.FinalizeParse("*", _prj.PhoneticParser);
			Assert.IsTrue(member.GetEnvironmentMatch(new string[0]));
			Assert.IsTrue(member.GetEnvironmentMatch(new[] { "v", "r", "g" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_SimpleMatch_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv(null, "dcba"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_SimpleNonMatch_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInPrecedingEnv(null, "dcab"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_MatchAfterIgnoredSSeg_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv(null, "dca$"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOneOrMoreSymbol_MatchBeforeEnd_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv("+", "dcba"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOneOrMoreSymbol_MatchAtEnd_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInPrecedingEnv("+", "a"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOneOrMoreSymbolAtEndOfPattern_NoPhones_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInPrecedingEnv("+", ""));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOnlyOneOrMoreSymbolInPattern_NoPhones_ReturnsFalse()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsFalse(member.GetEnvironmentMatch(new string[0]));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOnlyOneOrMoreSymbolInPattern_HasPhones_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsTrue(member.GetEnvironmentMatch(new[] { "a" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOneOrMoreSymbol_MatchBeforeEndAfterIgnoredSegs_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv("+", "da$$"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasWordBoundary_MatchAtEnd_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv("#", "a"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasWordBoundary_MatchAtEndAfterIgnoredSegs_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv("#", "a%$"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasWordBoundaryAtEndOfPattern_NoPhones_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInPrecedingEnv("#", ""));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOnlyWordBoundaryInPattern_NoPhones_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.FinalizeParse("#", _prj.PhoneticParser);
			Assert.IsTrue(member.GetEnvironmentMatch(new string[0]));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_MultiPhonePatternMatched_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.FinalizeParse("", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "lkjyb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_MultiPhonePatternMatchedAfterIgnoredSsegs_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.FinalizeParse("", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "lkjyb$%$"));
			Assert.IsTrue(GetMatchInEnvironment(member, "yb$%$"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_MultiPhonePatternMatchedWithIntermediateIgnoredSsegs_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.FinalizeParse("", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "lkjy$%$b"));
			Assert.IsTrue(GetMatchInEnvironment(member, "y$b%"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOneOrMoreSymbol_MultiPhonePatternMatchedBeforeEnd_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "lyb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOneOrMoreSymbol_MultiPhonePatternMatchedAtEnd_ReturnsFalse()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsFalse(GetMatchInEnvironment(member, "yb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasWordBoundary_MultiPhonePatternMatchedBeforeEnd_ReturnsFalse()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.FinalizeParse("#", _prj.PhoneticParser);
			Assert.IsFalse(GetMatchInEnvironment(member, "lyb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasWordBoundary_MultiPhonePatternMatchedAtEnd_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Preceding, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.FinalizeParse("#", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "yb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasOneOrMoreSymbol_MatchBeforeIgnoredSegsToEnd_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInPrecedingEnv("+", "$a"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInPrecedingEnvironment_HasWordBoundary_MatchBeforeIgnoredSegsToEnd_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInPrecedingEnv("#", "$a"));
		}

		#endregion

		#region GetEnvironmentMatch tests (for following environment)
		/// ------------------------------------------------------------------------------------
		private bool GetMatchResultInFollowingEnv(string wildcardSymbol, string phones)
		{
			return GetMatchInEnvironment(
				CreateMember(PatternPart.Following, wildcardSymbol), phones);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasZeroOrMoreSymbol_AlwaysReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv("*", "abcd"));
			Assert.IsTrue(GetMatchResultInFollowingEnv("*", ""));
			Assert.IsTrue(GetMatchResultInFollowingEnv("*", "bacd"));

			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.FinalizeParse("*", _prj.PhoneticParser);
			Assert.IsTrue(member.GetEnvironmentMatch(new string[0]));
			Assert.IsTrue(member.GetEnvironmentMatch(new[] { "v", "r", "g" }));
		}
	
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_SimpleMatch_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv(null, "abcd"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_SimpleNonMatch_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInFollowingEnv(null, "bacd"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_MatchAfterIgnoredSSeg_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv(null, "$acd"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOneOrMoreSymbol_MatchBeforeEnd_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv("+", "abcd"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOneOrMoreSymbol_MatchAtEnd_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInFollowingEnv("+", "a"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOneOrMoreSymbolAtEndOfPattern_NoPhones_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInFollowingEnv("+", ""));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOnlyOneOrMoreSymbolInPattern_NoPhones_ReturnsFalse()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsFalse(member.GetEnvironmentMatch(new string[0]));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOnlyOneOrMoreSymbolInPattern_HasPhones_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsTrue(member.GetEnvironmentMatch(new [] { "a" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOneOrMoreSymbol_MatchBeforeEndAfterIgnoredSegs_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv("+", "$$ad"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasWordBoundary_MatchAtEnd_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv("#", "a"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasWordBoundary_MatchAtEndAfterIgnoredSegs_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv("#", "$%a"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasWordBoundaryAtEndOfPattern_NoPhones_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInFollowingEnv("#", ""));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOnlyWordBoundaryInPattern_NoPhones_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.FinalizeParse("#", _prj.PhoneticParser);
			Assert.IsTrue(member.GetEnvironmentMatch(new string[0]));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_MultiPhonePatternMatched_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.FinalizeParse("", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "byjkl"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_MultiPhonePatternMatchedAfterIgnoredSsegs_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.FinalizeParse("", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "$%$byjkl"));
			Assert.IsTrue(GetMatchInEnvironment(member, "$%$by"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_MultiPhonePatternMatchedWithIntermediateIgnoredSsegs_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.FinalizeParse("", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "b$%$yjkl"));
			Assert.IsTrue(GetMatchInEnvironment(member, "%b$y"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOneOrMoreSymbol_MultiPhonePatternMatchedBeforeEnd_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "byl"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOneOrMoreSymbol_MultiPhonePatternMatchedAtEnd_ReturnsFalse()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.FinalizeParse("+", _prj.PhoneticParser);
			Assert.IsFalse(GetMatchInEnvironment(member, "by"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasWordBoundary_MultiPhonePatternMatchedBeforeEnd_ReturnsFalse()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.FinalizeParse("#", _prj.PhoneticParser);
			Assert.IsFalse(GetMatchInEnvironment(member, "byl"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasWordBoundary_MultiPhonePatternMatchedAtEnd_ReturnsTrue()
		{
			var member = CreateMember(PatternPart.Following, null);
			member.Reset();
			member.AddPhoneGroup(new[] { "a", "b", "c" });
			member.AddPhoneGroup(new[] { "x", "y", "z" });
			member.FinalizeParse("#", _prj.PhoneticParser);
			Assert.IsTrue(GetMatchInEnvironment(member, "by"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasOneOrMoreSymbol_MatchBeforeIgnoredSegsToEnd_ReturnsFalse()
		{
			Assert.IsFalse(GetMatchResultInFollowingEnv("+", "a$"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMatchInFollowingEnvironment_HasWordBoundary_MatchBeforeIgnoredSegsToEnd_ReturnsTrue()
		{
			Assert.IsTrue(GetMatchResultInFollowingEnv("#", "a$"));
		}

		#endregion
	}
}
