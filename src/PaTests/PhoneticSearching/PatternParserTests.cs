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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the default state of the search query is that they contain nothing to
		/// ignore.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_phoneCache = _prj.PhoneCache;

			_phoneCache["t"] = new PhoneInfo { Phone = "t" };
			_phoneCache["t"].CharType = IPASymbolType.consonant;
			_phoneCache["p"] = new PhoneInfo { Phone = "p" };
			_phoneCache["p"].CharType = IPASymbolType.consonant;
			_phoneCache["b"] = new PhoneInfo { Phone = "b" };
			_phoneCache["b"].CharType = IPASymbolType.consonant;
			_phoneCache["d"] = new PhoneInfo { Phone = "d" };
			_phoneCache["d"].CharType = IPASymbolType.consonant;
			_phoneCache["h"] = new PhoneInfo { Phone = "h" };
			_phoneCache["h"].CharType = IPASymbolType.consonant;
			_phoneCache["s"] = new PhoneInfo { Phone = "s" };
			_phoneCache["s"].CharType = IPASymbolType.consonant;
			_phoneCache["x"] = new PhoneInfo { Phone = "x" };
			_phoneCache["x"].CharType = IPASymbolType.consonant;
			_phoneCache["g"] = new PhoneInfo { Phone = "g" };
			_phoneCache["g"].CharType = IPASymbolType.consonant;
			_phoneCache["l"] = new PhoneInfo { Phone = "l" };
			_phoneCache["l"].CharType = IPASymbolType.consonant;
			_phoneCache["k"] = new PhoneInfo { Phone = "k" };
			_phoneCache["k"].CharType = IPASymbolType.consonant;
			_phoneCache["m"] = new PhoneInfo { Phone = "m" };
			_phoneCache["m"].CharType = IPASymbolType.consonant;
			_phoneCache["n"] = new PhoneInfo { Phone = "n" };
			_phoneCache["n"].CharType = IPASymbolType.consonant;

			_phoneCache["a"] = new PhoneInfo();
			_phoneCache["a"].CharType = IPASymbolType.vowel;
			_phoneCache["e"] = new PhoneInfo();
			_phoneCache["e"].CharType = IPASymbolType.vowel;
			_phoneCache["i"] = new PhoneInfo();
			_phoneCache["i"].CharType = IPASymbolType.vowel;
			_phoneCache["o"] = new PhoneInfo();
			_phoneCache["o"].CharType = IPASymbolType.vowel;
			_phoneCache["u"] = new PhoneInfo();
			_phoneCache["u"].CharType = IPASymbolType.vowel;

			_phoneCache["."] = new PhoneInfo();
			_phoneCache["."].CharType = IPASymbolType.suprasegmental;
			_phoneCache["'"] = new PhoneInfo();
			_phoneCache["'"].CharType = IPASymbolType.suprasegmental;

			SearchEngine.PhoneCache = _phoneCache;

			App.BFeatureCache.LoadFromList(new[]
			{
				new Feature { Name = "+high" },
				new Feature { Name = "+voice" },
			});
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_SingleDistinctiveFeature_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("[+high]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_SingleDescriptiveFeature_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("[nasal]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_ConsonantClass_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("[C]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_VowelClass_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("[V]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_FeatureAndVowelClassSequence_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("[V][nasal]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_FeatureAndVowelClassInBrackets_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("[[V][nasal]]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_FeatureAndVowelClassInBraces_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("{[V][nasal]}"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_InvalidDescriptiveFeature_ReturnsFalse()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsFalse(group.VerifyBracketedText("[blah]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_InvalidDistinctiveFeature_ReturnsFalse()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsFalse(group.VerifyBracketedText("[-blah]"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyBracketedText_PathologicalNesting_ReturnsTrue()
		{
			var group = new PatternGroup(EnvironmentType.After);
			Assert.IsTrue(group.VerifyBracketedText("{[[C][nasal]],[[[V][+high]][-voice]]}"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AAA()
		{
			((PhoneInfo)_phoneCache["d"]).BFeatureNames = new List<string> { "+high", "+voice" };
			((PhoneInfo)_phoneCache["t"]).BFeatureNames = new List<string> { "+high" };
			((PhoneInfo)_phoneCache["h"]).BFeatureNames = new List<string> { "+high", "+voice" };
			((PhoneInfo)_phoneCache["d"]).AFeatureNames = new List<string> { "dental" };
			var group = new PatternGroup(EnvironmentType.After);

			var pattern = group.Parse1("[[[+high][+voice]][C]]");

			pattern = group.Parse1("[{[+high],[-voice]}[C]]");
			pattern = group.Parse1("{[[+high][+voice]],[C]}");
			pattern = group.Parse1("[[+high][+voice]][dental]abc{(gh),(mn)}");
	
		
		
		}
	}
}
