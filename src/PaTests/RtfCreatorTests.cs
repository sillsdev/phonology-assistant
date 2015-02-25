// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: RtfCreatorTests.cs
// Responsibility: DavidO & ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using NUnit.Framework;
using SIL.Pa.TestUtils;

namespace SIL.Pa.Tests
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Tests Misc. methods in RtfCreator.
	/// </summary>
	/// --------------------------------------------------------------------------------
	[TestFixture]
	public class RtfCreatorTests : TestBase
	{
		//Dictionary<int, int> m_maxColumnWidths;
		//RtfCreator m_rtfCreator;

		#region Setup/Teardown
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary test records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
			//PaProject proj = new PaProject(true);
			//proj.Language = "dummy";
			//proj.ProjectName = "dummy";
			//PaApp.Project = proj;

			//m_maxColumnWidths = new Dictionary<int, int>();
			//m_maxColumnWidths.Add(0, 91);		// Phonetic
			//m_maxColumnWidths.Add(1, 99);		// Gloss
			//m_maxColumnWidths.Add(2, 149);		// CV Pattern
			//m_maxColumnWidths.Add(3, 270);		// Audio File

			//WordListCache cache = new WordListCache();

			//m_rtfCreator = new RtfCreator(RtfCreator.ExportTarget.Clipboard, 
			//    RtfCreator.ExportFormat.Table, new PaWordListGrid(cache), cache, null);
			
			//SetField(m_rtfCreator, "m_maxColumnWidths", m_maxColumnWidths);
			//SetField(m_rtfCreator, "m_pixelsPerInch", 96f);
			//SetField(m_rtfCreator, "m_MaxColWidth", 2340);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Close and delete the test database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureTearDown]
		public override void FixtureTearDown()
		{
			App.Project = null;
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
		/// Tests the CalcExtraColumnSpace method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CalcExtraColumnSpaceTest()
		{
			//Object[] parameters;
			//parameters = new Object[1] { 1845f };
			//int extraSpace = GetIntResult(m_rtfCreator, "ExtraSpaceToShare", null);
			
			//Assert.AreEqual(1110, extraSpace);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the ShortenColumns method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ShortenColumnsTest()
		{
			//Object[] parameters;
			//float columnStartPoint;

			//SetField(m_rtfCreator, "m_extraSpaceToShare", 1110);
			//if ((int)GetField(m_rtfCreator, "m_largeColCount") != 2)
			//    SetField(m_rtfCreator, "m_largeColCount", 2);

			//parameters = new Object[3] { 1365f, 0f, PaApp.Project.FieldInfo.PhoneticField };
			//columnStartPoint = GetFloatResult(m_rtfCreator, "ShortenColumns", parameters);
			//Assert.AreEqual(1365f, columnStartPoint);

			//parameters = new Object[3] { 1485f, 1365f, PaApp.Project.FieldInfo.GlossField };
			//columnStartPoint = GetFloatResult(m_rtfCreator, "ShortenColumns", parameters);
			//Assert.AreEqual(3210f, columnStartPoint);

			//parameters = new Object[3] { 2235f, 3210f, PaApp.Project.FieldInfo.CVPatternField };
			//columnStartPoint = GetFloatResult(m_rtfCreator, "ShortenColumns", parameters);
			//Assert.AreEqual(6105f, columnStartPoint);

			//parameters = new Object[3] { 4050f, 6105f, PaApp.Project.FieldInfo.AudioFileField };
			//columnStartPoint = GetFloatResult(m_rtfCreator, "ShortenColumns", parameters);
			//Assert.AreEqual(9360f, columnStartPoint);
		}
	}
}
