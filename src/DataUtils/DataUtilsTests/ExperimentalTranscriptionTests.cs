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
using SIL.Pa.TestUtils;

namespace SIL.Pa.Data
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DataUtils.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class ExperimentalTranscriptionTests : TestBase
	{
		private ExperimentalTranscriptions m_exTransList;

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
			InventoryReader.Load(m_inventoryFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
        /// 
        /// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_exTransList = new ExperimentalTranscriptions();

			DataUtils.IPASymbolCache.ExperimentalTranscriptions = m_exTransList;
			DataUtils.IPASymbolCache.AmbiguousSequences.Clear();
			DataUtils.IPASymbolCache.UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
			DataUtils.IPASymbolCache.LogUndefinedCharactersWhenParsing = false;
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
		/// Tests that PA-684 is fixed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void TestWithStressMark()
		{
			List<string> transtToCvtrTo = new List<string>();
			transtToCvtrTo.Add("e");
			
			ExperimentalTrans extrans = new ExperimentalTrans();
			extrans.TranscriptionsToConvertTo = transtToCvtrTo;
			extrans.CurrentConvertToItem = transtToCvtrTo[0];
			extrans.ConvertFromItem = "e\u02D0";
			extrans.Convert = true;
			m_exTransList.Add(extrans);

			string text = m_exTransList.Convert("we\u02D0nop");
			Assert.AreEqual("wenop", text);

			text = m_exTransList.Convert("we\u02D0\u02C8nop");
			Assert.AreEqual("we\u02C8nop", text);
		}
	}
}