using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using NUnit.Framework;

namespace SIL.Pa
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Tests the settings handler.
	/// </summary>
	/// --------------------------------------------------------------------------------
	[TestFixture]
	public class SettingsHandlerTests
	{
		private const string m_xmlShell = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + 
				"<settings>{0}</settings>";

		private static string m_tempFilename;
		private static Form m_frm;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			m_tempFilename = Path.GetTempFileName();
			m_frm = new Form();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			File.Delete(m_tempFilename);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create temporary XML file based upon an XML string
		/// </summary>
		/// <param name="xml">String of XML to dump to a file</param>
		/// ------------------------------------------------------------------------------------
		public void CreateTempFile(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			doc.Save(m_tempFilename);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests for proper return values based on element name and other arguments. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadImportMarkers_ArrayList()
		{
			string xml = string.Format(m_xmlShell, "<markermap>" +
				"<field dbfield=\"rock\" marker=\"\\rk\" required=\"true\" />" +
				"<field dbfield=\"paper\" marker=\"\\pp\" required=\"false\" />" +
				"<field dbfield=\"scissors\" marker=\"\\ss\" required=\"true\" />" +
				"</markermap>");

			
			CreateTempFile(xml);

			PaSettingsHandler hndlr = new PaSettingsHandler(m_tempFilename);
			List<SFMarkerMapping> mappings = hndlr.LoadImportMarkers();

			Assert.AreEqual(mappings[0].PaField, "rock");
			Assert.AreEqual(mappings[0].Marker, "\\rk");
			Assert.IsTrue(mappings[0].Required);
			Assert.AreEqual(mappings[1].PaField, "paper");
			Assert.AreEqual(mappings[1].Marker, "\\pp");
			Assert.IsFalse(mappings[1].Required);
			Assert.AreEqual(mappings[2].PaField, "scissors");
			Assert.AreEqual(mappings[2].Marker, "\\ss");
			Assert.IsTrue(mappings[2].Required);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveImportMarkers()
		{
			string xml = string.Format(m_xmlShell, "<markermap><field dbfield=\"RecID\" " +
				"marker=\"\\recID\" required=\"true\" /></markermap>");

			CreateTempFile(xml);

			PaSettingsHandler hndlr = new PaSettingsHandler(m_tempFilename);

			// Test existing values.
			List<SFMarkerMapping> mappings = hndlr.LoadImportMarkers();
			Assert.AreEqual(mappings[0].PaField, "RecID");
			Assert.AreEqual(mappings[0].Marker, "\\recID");
			Assert.IsTrue(mappings[0].Required);

			mappings = new List<SFMarkerMapping>();
			mappings.Add(new SFMarkerMapping("topcat", "\\tc", false));
			mappings.Add(new SFMarkerMapping("foghorn", "\\fh", true));

			hndlr.SaveImportMarkers(mappings);

			mappings = hndlr.LoadImportMarkers();
			Assert.AreEqual(2, mappings.Count);
			Assert.AreEqual(mappings[0].PaField, "topcat");
			Assert.AreEqual(mappings[0].Marker, "\\tc");
			Assert.IsFalse(mappings[0].Required);
			Assert.AreEqual(mappings[1].PaField, "foghorn");
			Assert.AreEqual(mappings[1].Marker, "\\fh");
			Assert.IsTrue(mappings[1].Required);
		}
	}
}
