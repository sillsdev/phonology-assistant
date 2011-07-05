// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ExperimentalTranscriptionTests.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in App.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class TranscriptionChangesTests : TestBase
	{
		private TranscriptionChanges m_exTransList;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
			base.FixtureSetup();
			InventoryHelper.Load();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_exTransList = new TranscriptionChanges();

			m_prj.TranscriptionChanges.Clear();
			m_prj.TranscriptionChanges.AddRange(m_exTransList);
			m_prj.AmbiguousSequences.Clear();
			//App.IPASymbolCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			m_prj.PhoneticParser.LogUndefinedCharactersWhenParsing = false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that PA-684 is fixed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void TestWithStressMark()
		{
			List<string> transtToCvtrTo = new List<string>();
			transtToCvtrTo.Add("e");

			TranscriptionChange extrans = new TranscriptionChange();
			extrans.SetReplacementOptions(transtToCvtrTo);
			extrans.ReplaceWith = transtToCvtrTo[0];
			extrans.WhatToReplace = "e\u02D0";
			m_exTransList.Add(extrans);

			string text = m_exTransList.Convert("we\u02D0nop");
			Assert.AreEqual("wenop", text);

			text = m_exTransList.Convert("we\u02D0\u02C8nop");
			Assert.AreEqual("we\u02C8nop", text);
		}
	}
}