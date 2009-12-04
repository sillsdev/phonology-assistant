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
// File: RawRecordViewTests.cs
// Responsibility: DavidO & ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.TestUtils;
using SIL.Pa.UI.Controls;

namespace SIL.Pa
{
    /// --------------------------------------------------------------------------------
    /// <summary>
    /// Tests the record view
    /// </summary>
    /// --------------------------------------------------------------------------------
    [TestFixture]
    public class RecordViewTests : TestBase
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
		/// Sets the m_rtfFields values;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetData(string[] values)
		{
			//m_rtfFields = new Dictionary<int, RawRecordView.RTFFieldInfo>();

			//for (int i = 0; i < values.Length; i++)
			//{
			//    m_tmpData = new RawRecordView.RTFFieldInfo();
			//    m_tmpData.isInterlinear = true;
			//    m_tmpData.fieldValue = values[i];
			//    m_rtfFields[i] = m_tmpData;
			//}

			//SetField(m_resultView, "m_rtfFields", m_rtfFields);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetInterlinearSubColumnContents method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetInterlinearSubColumnContentsTest1()
		{
			List<string> data = new List<string>(new string[] {
				"i-   na-   ishi",
				"cl-  PRES- live",
				"prf- TEMP- v"});

			Dictionary<int, List<string>> result = GetResult(typeof(RtfRecordView),
				"GetInterlinearSubColumnContents", data) as Dictionary<int, List<string>>;

			Assert.AreEqual(3, result.Count);
			Assert.AreEqual(3, result[0].Count);
			Assert.AreEqual(3, result[1].Count);
			Assert.AreEqual(3, result[2].Count);

			Assert.AreEqual("i-", result[0][0]);
			Assert.AreEqual("na-", result[0][1]);
			Assert.AreEqual("ishi", result[0][2]);

			Assert.AreEqual("cl-", result[1][0]);
			Assert.AreEqual("PRES-", result[1][1]);
			Assert.AreEqual("live", result[1][2]);

			Assert.AreEqual("prf-", result[2][0]);
			Assert.AreEqual("TEMP-", result[2][1]);
			Assert.AreEqual("v", result[2][2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetInterlinearSubColumnContents method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetInterlinearSubColumnContentsTest2()
		{
			List<string> data = new List<string>(new string[] {
				"angu    j-",
				"1s POSS cl-"});

			Dictionary<int, List<string>> result = GetResult(typeof(RtfRecordView),
				"GetInterlinearSubColumnContents", data) as Dictionary<int, List<string>>;

			Assert.AreEqual(2, result.Count);
			Assert.AreEqual(2, result[0].Count);
			Assert.AreEqual(2, result[1].Count);

			Assert.AreEqual("angu", result[0][0]);
			Assert.AreEqual("j-", result[0][1]);

			Assert.AreEqual("1s POSS", result[1][0]);
			Assert.AreEqual("cl-", result[1][1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetInterlinearSubColumnContents method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetInterlinearSubColumnContentsTest3()
		{
			List<string> data = new List<string>(new string[] {"1s POSS cl-"});

			Dictionary<int, List<string>> result = GetResult(typeof(RtfRecordView),
				"GetInterlinearSubColumnContents", data) as Dictionary<int, List<string>>;

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(1, result[0].Count);
			Assert.AreEqual("1s POSS cl-", result[0][0]);
		}
	}
}