// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FeatureTests.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using NUnit.Framework;
using SilUtils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FeatureTests
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Clone method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Clone()
		{
			var feat1 = new Feature { Name = "Dogs", FullName = "Big Dogs" };
			ReflectionHelper.SetProperty(feat1, "Bit", 46);

			var feat2 = feat1.Clone();
			Assert.AreEqual("Dogs", feat2.Name);
			Assert.AreEqual("Big Dogs", feat2.FullName);
			Assert.AreEqual(46, feat2.Bit);
		}
	}
}
