using NUnit.Framework;
using SIL.Pa.TestUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class AndGroupValidatorTests : TestBase
	{
		private AndGroupValidator _validator;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			App.DottedCircle = "0";
			App.DottedCircleC = '0';
			App.DiacriticPlaceholder = "[0]";

			_validator = new AndGroupValidator(_prj);
		}
		
		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateInnerSquareBracketedTextToTokens_ContainsNoSquareBrackets_ReturnsInput()
		{
			var result = _validator.TranslateInnerSquareBracketedTextToTokens("abcd");
			Assert.AreEqual("abcd", result);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateInnerSquareBracketedTextToTokens_ContainsOneSetOfSquareBrackets_ReturnsModifiedString()
		{
			var result = _validator.TranslateInnerSquareBracketedTextToTokens("ab[high]cd");
			Assert.AreEqual("ab", result.Substring(0, 2));
			Assert.IsTrue(result[2] > 60000);
			Assert.AreEqual("cd", result.Substring(3, 2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateInnerSquareBracketedTextToTokens_ContainsMultipleSetsOfSquareBrackets_ReturnsModifiedString()
		{
			var result = _validator.TranslateInnerSquareBracketedTextToTokens("ab[high]c[low]d");
			Assert.AreEqual("ab", result.Substring(0, 2));
			Assert.IsTrue(result[2] > 60000);
			Assert.AreEqual('c', result[3]);
			Assert.IsTrue(result[4] > 60000);
			Assert.AreEqual('d', result[5]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TranslateInnerSquareBracketedTextToTokens_ContainsNestedSetsOfSquareBrackets_ReturnsModifiedString()
		{
			var result = _validator.TranslateInnerSquareBracketedTextToTokens("ab[[high]c[low]]d");
			Assert.AreEqual("ab[", result.Substring(0, 3));
			Assert.IsTrue(result[3] > 60000);
			Assert.AreEqual('c', result[4]);
			Assert.IsTrue(result[5] > 60000);
			Assert.AreEqual("]d", result.Substring(6, 2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsNoGroups_ReturnsNoErrors()
		{
			_validator.Verify("abc");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsOrGroup_ReturnsNoErrors()
		{
			_validator.Verify("wx[[high]{a,b,c}[low]]yz");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsClass_ReturnsNoErrors()
		{
			_validator.Verify("wx[[high]<stupid>[low]]yz");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsMixtureOfEmbeddedGroups_ReturnsNoErrors()
		{
			_validator.Verify("wx[a[0~]{a,[[high][low]],c}<stupid>]yz");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsComma_ReturnsErrors()
		{
			_validator.Verify("wx[[high],[low]]yz");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsOnePhone_ReturnsNoErrors()
		{
			_validator.Verify("wx[a[0~]]yz");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsMultiplePhones_ReturnsErrors()
		{
			_validator.Verify("wx[ab[0~]]yz");
			Assert.IsTrue(_validator.HasErrors);

			_validator.Verify("wx[a[0~]b]yz");
			Assert.IsTrue(_validator.HasErrors);
		}
	}
}
