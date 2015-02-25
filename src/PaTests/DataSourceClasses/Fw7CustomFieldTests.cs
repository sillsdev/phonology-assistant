// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: Fw7CustomFieldTests.cs
// Responsibility: Greg Trihus
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using NUnit.Framework;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.DataSourceClasses.FieldWorks;
using SIL.Pa.TestUtils;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.DataSourceClasses
{
	[TestFixture]
	public class Fw7CustomFieldTests : TestBase
	{
		#region Declarations
		private PaDataSource _dataSource;
		#endregion

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

			_dataSource = new PaDataSource();
			_prj.DataSources = new List<PaDataSource>(new[] { _dataSource });

			FindInfo.ShowMessages = false;
			FwDBAccessInfo.ShowMsgOnFileLoadFailure = false;
			FwQueries.ShowMsgOnFileLoadFailure = false;
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
		}

		#endregion

		#region Tests
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests finding a whole word with a diacritic in a string. See PA-792.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFw7CustomFieldData()
		{
            _dataSource.FwDataSourceInfo = new FwDataSourceInfo(TestInput("Ikposo-01.fwdata"), null, DataSourceType.FW7);
            var customField = new Fw7CustomField(_dataSource);
            Assert.AreEqual(new[] { "Stem Syllable Type", "Surface Tone Temp", "Word-CV Pattern"}, customField.FieldNames());
            Assert.AreEqual("en", customField.FieldWs("Word-CV Pattern"));
            Assert.AreEqual(3, customField.CustomValues.Count);
            Assert.AreEqual("H H HF", customField.Value("Surface Tone Temp", "5375a8ef-6958-48ca-9c57-02246177e07f"));
            Assert.AreEqual("VCVCV", customField.Value("Word-CV Pattern", "5375a8ef-6958-48ca-9c57-02246177e07f"));
            Assert.AreEqual("en", customField.Ws("Stem Syllable Type", "5375a8ef-6958-48ca-9c57-02246177e07f"));
            Assert.AreEqual(FwDBUtils.FwWritingSystemType.Analysis, customField.FwWritingSystemType("Surface Tone Temp"));
        }

		#endregion
	}
}
