using NUnit.Framework;
using SIL.Pa.TestUtils;

namespace SIL.Pa.PhoneticSearching
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class OrGroupValidatorTests : TestBase
	{
		private OrGroupValidator _validator;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			App.DottedCircle = "0";
			App.DottedCircleC = '0';
			App.DiacriticPlaceholder = "[0]";

			_validator = new OrGroupValidator(_prj);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsNoGroups_ReturnsNoErrors()
		{
			_validator.Verify("{a,b,c}");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsAndGroup_ReturnsNoErrors()
		{
			_validator.Verify("{a,[[high][low]],c}");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsClass_ReturnsNoErrors()
		{
			_validator.Verify("{a,<stupid>,c}");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsParentheticalRun_ReturnsNoErrors()
		{
			_validator.Verify("{a,(xy),c}");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsUnparentheticalRun_ReturnsErrors()
		{
			_validator.Verify("{a,xy,c}");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsUnbracketedAndGroup_ReturnsErrors()
		{
			_validator.Verify("{a,[high][low],c}");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsAndGroupAndClass_ReturnsErrors()
		{
			_validator.Verify("{a,[[high][low]]<stupid>,c}");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsClassInAndGroup_ReturnsNoErrors()
		{
			_validator.Verify("{a,[[high][low]<stupid>],c}");
			Assert.IsFalse(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsOnePhone_ReturnsErrors()
		{
			_validator.Verify("{a}");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsOneAndGroup_ReturnsErrors()
		{
			_validator.Verify("{[[high][low]]}");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsOneClass_ReturnsErrors()
		{
			_validator.Verify("{<stupid>}");
			Assert.IsTrue(_validator.HasErrors);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Verify_ContainsAllMannerOfErrors_ReturnsErrors()
		{
			_validator.Verify("{a,bc[[high][C]],x,y,z<stupid>}");
			Assert.AreEqual(2, _validator.Errors.Count);
		}
	}
}
