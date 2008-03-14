// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2008, SIL International. All Rights Reserved.
// <copyright from='2008' to='2008' company='SIL International'>
//		Copyright (c) 2008, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: MiscControlTests.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Windows.Forms;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;

namespace SIL.Pa.Controls
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Misc. tests of stuff in PaControls.dll
	/// </summary>
	/// --------------------------------------------------------------------------------
	[TestFixture]
	public class MiscControlTests : TestBase
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the SurroundingCVWithBrackets method in PatternTextBox class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PatternTextBox_SurroundingCVWithBracketsTest()
		{
			Assert.IsTrue(GetCVBracketsResult(string.Empty, 0));
			Assert.IsTrue(GetCVBracketsResult("12", 1));
			Assert.IsTrue(GetCVBracketsResult("12", 2));
			Assert.IsFalse(GetCVBracketsResult("<12", 2));
			Assert.IsFalse(GetCVBracketsResult("[12", 2));
			Assert.IsFalse(GetCVBracketsResult("<12>", 3));
			Assert.IsFalse(GetCVBracketsResult("[12]", 3));
			Assert.IsTrue(GetCVBracketsResult("<12>", 4));
			Assert.IsTrue(GetCVBracketsResult("[12]", 4));
			Assert.IsFalse(GetCVBracketsResult("<1]2>", 3));
			Assert.IsFalse(GetCVBracketsResult("<1[2]3>", 5));
			Assert.IsFalse(GetCVBracketsResult("[1>2]", 3));
			Assert.IsFalse(GetCVBracketsResult("[1<2>3]", 5));
			Assert.IsTrue(GetCVBracketsResult("<1[2]3>", 7));
			Assert.IsTrue(GetCVBracketsResult("[1<2>3]", 7));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetCVBracketsResult(string text, int selStart)
		{
			TextBox txt = new TextBox();
			txt.Text = text;
			txt.SelectionStart = selStart;
			return GetBoolResult(typeof(PatternTextBox), "SurroundCVInBrackets", txt);
		}
	}
}
