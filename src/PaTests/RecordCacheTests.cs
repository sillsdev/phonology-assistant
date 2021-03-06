// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.Model;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests Misc. methods in DataUtils that reqire a database.
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
		public override void FixtureSetup()
		{
			//m_cache = _prj.RecordCache;
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
        public void TestSetup()
        {
        }

		#endregion

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the GetInterlinearColumnWidths method.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetInterlinearColumnWidthsTest()
		//{
		//    string testString = "barney   rubble  trouble";
		//    List<int> result = GetResult(typeof(RecordCache), "GetInterlinearColumnWidths", testString) as List<int>;
		//    Assert.AreEqual(3, result.Count);
		//    Assert.AreEqual(9, result[0]);
		//    Assert.AreEqual(8, result[1]);
		//    Assert.AreEqual(7, result[2]);

		//    testString = "barney rubble   ";
		//    result = GetResult(typeof(RecordCache), "GetInterlinearColumnWidths", testString) as List<int>;
		//    Assert.AreEqual(2, result.Count);
		//    Assert.AreEqual(7, result[0]);

		//    testString = "barney";
		//    result = GetResult(typeof(RecordCache), "GetInterlinearColumnWidths", testString) as List<int>;
		//    Assert.AreEqual(1, result.Count);
		//    Assert.AreEqual(6, result[0]);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the ParseEntryAsInterlinear method.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void ParseEntryAsInterlinearTest()
		//{
		//    //PaProject proj = new PaProject(true);
		//    //App.FieldInfo = new PaFieldInfoList();
		//    //PaFieldInfo info = new PaFieldInfo();
		//    //info.FieldName = "x";
		//    //info.IsParsed = true;
		//    //App.FieldInfo.Add(info);
		//    //info = new PaFieldInfo();
		//    //info.FieldName = "y";
		//    //info.IsParsed = true;
		//    //App.FieldInfo.Add(info);
		//    //info = new PaFieldInfo();
		//    //info.FieldName = "z";
		//    //info.IsParsed = true;
		//    //App.FieldInfo.Add(info);
		//    //App.Project = proj;

		//    //List<string> interlinearFields = new List<string>();
		//    //interlinearFields.Add("x");
		//    //interlinearFields.Add("y");
		//    //interlinearFields.Add("z");

		//    //RecordCacheEntry entry = new RecordCacheEntry(true);
		//    //entry.WordEntries = new List<WordCacheEntry>();
		//    //entry.FirstInterlinearField = "x";
		//    //entry.InterlinearFields = interlinearFields;
		//    //entry.SetValue("x", "1234    567    890");
		//    //entry.SetValue("y", "AB- CD  GH - I JKLMN");
		//    //entry.SetValue("z", "abcd -e -fgh");

		//    //CallMethod(m_cache, "ParseEntryAsInterlinear", entry);

		//    //Assert.AreEqual(3, entry.WordEntries.Count);
		//    //Assert.AreEqual("1234", entry.WordEntries[0]["x"]);
		//    //Assert.AreEqual("AB- CD", entry.WordEntries[0]["y"]);
		//    //Assert.AreEqual("abcd -e", entry.WordEntries[0]["z"]);

		//    //Assert.AreEqual("567", entry.WordEntries[1]["x"]);
		//    //Assert.AreEqual("GH - I", entry.WordEntries[1]["y"]);
		//    //Assert.AreEqual("-fgh", entry.WordEntries[1]["z"]);

		//    //Assert.AreEqual("890", entry.WordEntries[2]["x"]);
		//    //Assert.AreEqual("JKLMN", entry.WordEntries[2]["y"]);
		//    //Assert.IsNull(entry.WordEntries[2].GetField("z", false));
		//}
	}
}