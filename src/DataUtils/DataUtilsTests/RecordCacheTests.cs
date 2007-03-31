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

namespace SIL.SpeechTools.Database
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DBUtils that reqire a database.
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class RecordCacheTests : TestBase
	{
		RecordCache m_cache;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			m_cache = new RecordCache();
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
		/// Tests the FormatForEasierParsing method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FormatForEasierParsingTest()
		{
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields["testline1"] = "oo-   oo - oo  oo  -oo";
			fields["testline2"] = "-oo-  oo       oo  -oo- ";

			Dictionary<string, string> result =
				GetResult(m_cache, "FormatForEasierParsing", fields) as Dictionary<string, string>;

			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count);
			Assert.AreEqual("oo-\uFFFC\uFFFC\uFFFCoo\uFFFC-\uFFFCoo  oo\uFFFC\uFFFC-oo", result["testline1"]);
			Assert.AreEqual("-oo-\uFFFC\uFFFCoo       oo\uFFFC\uFFFC-oo-\uFFFC", result["testline2"]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the ParseEntryAsInterlinear method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ParseEntryAsInterlinearTest()
		{
			Dictionary<string, string> fields = new Dictionary<string, string>();
			fields["testline1"] = "oo- ooo - oo  oo  -oo";
			fields["testline2"] = "dd- dd        d   -ddd dddd";

			Dictionary<string, List<string>>result =
				GetResult(m_cache, "ParseEntryAsInterlinear", fields) as
				Dictionary<string, List<string>>;
			
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count);

			Assert.AreEqual(2, result["testline1"].Count);
			Assert.AreEqual(3, result["testline3"].Count);

			Assert.AreEqual("oo- ooo - oo", result["testline1"][0]);
			Assert.AreEqual("oo -oo", result["testline1"][1]);
			Assert.AreEqual("dd- dd", result["testline2"][0]);
			Assert.AreEqual("d   -ddd", result["testline2"][1]);
			Assert.AreEqual("dddd", result["testline2"][2]);
		}
	}
}