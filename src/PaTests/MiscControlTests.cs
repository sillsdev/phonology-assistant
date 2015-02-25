// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: MiscControlTests.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Windows.Forms;
using NUnit.Framework;
using SIL.Pa.TestUtils;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.Tests
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Misc. tests of stuff in PaControls.dll
	/// </summary>
	/// --------------------------------------------------------------------------------
	[TestFixture]
	public class MiscControlTests : TestBase
	{
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
