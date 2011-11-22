using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests methods having to do with CV patterns.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class CVPatternTests : TestBase
	{
		private PhoneCache m_cache;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_cache = new PhoneCache(_prj);
			_prj.CVPatternInfoList = new List<CVPatternInfo>();
			_prj.AmbiguousSequences.Clear();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Simple()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			Assert.AreEqual("VCVC", m_cache.GetCVPattern("abec"));
			Assert.AreEqual("CCVV", m_cache.GetCVPattern("bcea"));
			Assert.AreEqual("VVCC", m_cache.GetCVPattern("eabc"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WithPhonesNotInPhoneCache()
		{
			Assert.AreEqual("VCVC", m_cache.GetCVPattern("abec"));
			Assert.AreEqual("CCVV", m_cache.GetCVPattern("bcea"));
			Assert.AreEqual("VVCC", m_cache.GetCVPattern("eabc"));
			//Assert.AreEqual("VCVC", m_cache.GetCVPattern("a\u0303be\u0303c"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WithSpaces()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			Assert.AreEqual("VC VC", m_cache.GetCVPattern("ab ec"));
			Assert.AreEqual("C CVV", m_cache.GetCVPattern("b cea"));
			Assert.AreEqual("VVC C", m_cache.GetCVPattern("eab c"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetCVPattern
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IncludeExplicitPhonesWithoutDiacritics()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("e", CVPatternInfo.PatternType.Custom));
			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("c", CVPatternInfo.PatternType.Custom));

			Assert.AreEqual("VCec", m_cache.GetCVPattern("abec"));
			Assert.AreEqual("CceV", m_cache.GetCVPattern("bcea"));
			Assert.AreEqual("eVCc", m_cache.GetCVPattern("eabc"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IncludeExplicitPhonesWithDiacritics()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("a\u0303");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");
			m_cache.AddPhone("e\u0301");

			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("e\u0301", CVPatternInfo.PatternType.Custom));
			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("a", CVPatternInfo.PatternType.Custom));

			Assert.AreEqual("aCe\u0301C", m_cache.GetCVPattern("abe\u0301c"));
			Assert.AreEqual("CCVV", m_cache.GetCVPattern("bcea\u0303"));
			Assert.AreEqual("e\u0301aCC", m_cache.GetCVPattern("e\u0301abc"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IncludeExplicitDiacriticsAfter()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("a\u0303");
			m_cache.AddPhone("b");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");
			m_cache.AddPhone("e\u0301");

			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("\u0301", CVPatternInfo.PatternType.Suprasegmental));
			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("a", CVPatternInfo.PatternType.Custom));
			_prj.CVPatternInfoList.Add(CVPatternInfo.Create(App.kDottedCircle + "\u0303", CVPatternInfo.PatternType.Custom));

			Assert.AreEqual("aCVC", m_cache.GetCVPattern("abec"));
			Assert.AreEqual("CCV\u0301a", m_cache.GetCVPattern("bce\u0301a"));
			Assert.AreEqual("aV\u0301CV\u0303C", m_cache.GetCVPattern("ae\u0301ba\u0303c"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IncludeExplicitDiacriticsBefore()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("b");
			m_cache.AddPhone("\u207Fb");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			_prj.AddAmbiguousSequence("\u207Fb");

			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("a", CVPatternInfo.PatternType.Custom));
			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("\u207F" + App.kDottedCircle, CVPatternInfo.PatternType.Custom));

			Assert.AreEqual("a\u207FCVC", m_cache.GetCVPattern("a\u207Fbec"));
			Assert.AreEqual("\u207FCCVa", m_cache.GetCVPattern("\u207Fbcea"));
			Assert.AreEqual("aV\u207FCC\u207FC", m_cache.GetCVPattern("ae\u207Fbc\u207Fb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IncludeExplicitDiacriticsOnBothSides()
		{
			m_cache.AddPhone("a");
			m_cache.AddPhone("a\u0303");
			m_cache.AddPhone("b");
			m_cache.AddPhone("\u207Fb");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			_prj.AddAmbiguousSequence("\u207Fb");

			_prj.CVPatternInfoList.Add(CVPatternInfo.Create(App.kDottedCircle + "\u0303", CVPatternInfo.PatternType.Custom));
			_prj.CVPatternInfoList.Add(CVPatternInfo.Create("\u207F" + App.kDottedCircle, CVPatternInfo.PatternType.Custom));

			Assert.AreEqual("VV\u0303\u207FCC", m_cache.GetCVPattern("ea\u0303\u207Fbc"));
			Assert.AreEqual("\u207FCCVV", m_cache.GetCVPattern("\u207Fbcea"));
			Assert.AreEqual("CV\u0303CV", m_cache.GetCVPattern("ba\u0303ce"));
			Assert.AreEqual("VCV\u207FC", m_cache.GetCVPattern("eca\u207Fb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WithExperimentalTrans1()
		{
			var trans = new TranscriptionChange();
			trans.WhatToReplace = "x";
			var list = new List<string>();
			list.Add("y");
			trans.SetReplacementOptions(list);
			trans.ReplaceWith = "y";

			_prj.AddTranscriptionChange(trans);

			m_cache.AddPhone("a");
			m_cache.AddPhone("x");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			Assert.AreEqual("VVVC", m_cache.GetCVPattern("axec"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WithExperimentalTrans2()
		{
			var trans = new TranscriptionChange();
			trans.WhatToReplace = "x";
			var replacementOptions = new List<string>();
			replacementOptions.Add("y");
			trans.SetReplacementOptions(replacementOptions);
			trans.ReplaceWith = "y";

			_prj.AddTranscriptionChange(trans);

			m_cache.AddPhone("a");
			m_cache.AddPhone("x");
			m_cache.AddPhone("c");
			m_cache.AddPhone("e");

			Assert.AreEqual("VCVC", m_cache.GetCVPattern("axec", false));
		}
	}
}
