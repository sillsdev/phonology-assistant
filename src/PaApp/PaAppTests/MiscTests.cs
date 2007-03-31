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
// File: MiscTests.cs
// Responsibility: DavidO & ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using NUnit.Framework;
using SIL.SpeechTools.TestUtils;
using SIL.Pa.Data;

namespace SIL.Pa
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DataUtils that reqire a database.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class MiscTests : TestBase
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
			PaApp.FieldInfo = null;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetInterlinearColumnWidths method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DataSourceReader_ParseSoundFileName()
		{
			DataSourceReader reader = new DataSourceReader(new PaProject());

			string filename = @"c:\junk1\junk2\junk3.wav";

			object[] args = new object[] { filename, (long)-1, (long)-1 };
			string result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(0, (long)args[1]);
			Assert.AreEqual(0, (long)args[2]);

			args = new object[] { filename + " 2.123", (long)-1, (long)-1 };
			result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(0, (long)args[2]);

			args = new object[] { filename + " 2.123 4.43265", (long)-1, (long)-1 };
			result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(4433, (long)args[2]);

			filename = @"c:\junk1\junk2\this is junk3.mp3";
			args = new object[] { filename + " 2.123 4.43265", (long)-1, (long)-1 };
			result = GetStrResult(reader, "ParseSoundFileName", args);
			Assert.AreEqual(result, filename);
			Assert.AreEqual(2123, (long)args[1]);
			Assert.AreEqual(4433, (long)args[2]);
		}
	}
}