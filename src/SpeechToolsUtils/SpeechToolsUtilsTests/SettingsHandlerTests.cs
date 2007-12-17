using System.IO;
using System.Windows.Forms;
using System.Xml;
using NUnit.Framework;

namespace SIL.SpeechTools.Utils
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

		///  ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_frm.Name = "testwnd";
			m_frm.WindowState = FormWindowState.Normal;
			m_frm.Left = m_frm.Top = -10;
			m_frm.Width = m_frm.Height = 250;
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
		/// Tests the FindChildNode method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FindChildNode()
		{
			string xml = string.Format(m_xmlShell, "<windowstates><window id=\"wnd1\"/>" +
				"<window id=\"wnd2\"/><window id=\"wnd3\"/></windowstates>");

			CreateTempFile(xml);

			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);

			// Test the overloaded version that takes an xpath to a parentNode as first argument.
			Assert.IsNotNull(hndlr.FindChildNode("/settings/windowstates", "wnd1"));
			XmlNode node = hndlr.FindChildNode("/settings/windowstates", "wnd3");
			Assert.IsNotNull(node);

			// Now test the overloaded version that takes a parentNode as first argument.
			node = node.ParentNode;
			Assert.IsNotNull(node);
			Assert.IsNotNull(hndlr.FindChildNode(node, "wnd2"));
		}

		#region Test loading/saving form properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests reading and settings for normal state form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFormProperties_NormalState()
		{
			string xml = string.Format(m_xmlShell, "<windowstates><window id=\"testwnd\" " +
				"state=\"Normal\" height=\"150\" width=\"200\" top=\"30\" left=\"20\" " +
				"dpi=\"96.0\"/></windowstates>");
			
			CreateTempFile(xml);

			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);

			Assert.IsTrue(hndlr.LoadFormProperties(m_frm));
			Assert.AreEqual(200, m_frm.Width);
			Assert.AreEqual(150, m_frm.Height);
			Assert.AreEqual(30, m_frm.Top);
			Assert.AreEqual(20, m_frm.Left);
			Assert.AreEqual(FormWindowState.Normal, m_frm.WindowState);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests reading and settings for maximized form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFormProperties_MaximizedState()
		{
			string xml = string.Format(m_xmlShell, "<windowstates><window id=\"testwnd\" " +
				"state=\"Maximized\" dpi=\"96.0\"/></windowstates>");

			CreateTempFile(xml);
			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);
			hndlr.LoadFormProperties(m_frm);
			Assert.AreEqual(FormWindowState.Maximized, m_frm.WindowState);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that form properties are saved when settings file dose not contain a node
		/// for the form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveFormProperties_NodeDoesNotExist()
		{
			CreateTempFile(m_xmlShell);

			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);

			m_frm.Left = 8;
			m_frm.Top = 9;
			m_frm.Width = 321;
			m_frm.Height = 250;

			VerifyWindowNodeContents(hndlr, m_frm);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests saving a normal state form's properties when a node for the form already
		/// exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveFormProperties_NodeAlreadyExists()
		{
			string xml = string.Format(m_xmlShell, "<windowstates><window state=\"Normal\" " +
				"height=\"1\" width=\"1\" top=\"1\" left=\"1\"/></windowstates>");

			CreateTempFile(xml);

			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);

			m_frm.Left = 33;
			m_frm.Top = 44;
			m_frm.Width = 222;
			m_frm.Height = 111;

			VerifyWindowNodeContents(hndlr, m_frm);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the contents of a window node.
		/// </summary>
		/// <param name="hndlr"></param>
		/// <param name="frm"></param>
		/// ------------------------------------------------------------------------------------
		private static void VerifyWindowNodeContents(SettingsHandler hndlr, Form frm)
		{
			hndlr.SaveFormProperties(frm, true);

			XmlNode node = hndlr.FindChildNode("/settings/windowstates", "testwnd");
			Assert.IsNotNull(node);
			Assert.AreEqual(frm.Width, XMLHelper.GetIntFromAttribute(node, "width", 0));
			Assert.AreEqual(frm.Height, XMLHelper.GetIntFromAttribute(node, "height", 0));
			Assert.AreEqual(frm.Top, XMLHelper.GetIntFromAttribute(node, "top", 0));
			Assert.AreEqual(frm.Left, XMLHelper.GetIntFromAttribute(node, "left", 0));
			Assert.AreEqual(frm.WindowState.ToString(), XMLHelper.GetAttributeValue(node, "state"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that only a form's state is saved when it's maximized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveFormProperties_Maximized()
		{
			CreateTempFile(m_xmlShell);

			m_frm.WindowState = FormWindowState.Maximized;
			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);
			hndlr.SaveFormProperties(m_frm, true);

			XmlNode node = hndlr.FindChildNode("/settings/windowstates", "testwnd");
			Assert.IsNotNull(node);
			Assert.AreEqual("Maximized", XMLHelper.GetAttributeValue(node, "state"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that a form's properties are not set when it's minimized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveFormProperties_Minimized()
		{
			CreateTempFile(m_xmlShell);
			m_frm.WindowState = FormWindowState.Minimized;
			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);
			hndlr.SaveFormProperties(m_frm, true);
			Assert.IsNull(hndlr.FindChildNode("/settings/windowstates", "testwnd"));
		}

		#endregion

		#region Tests for Loading/Saving grid settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that a form's properties are not set when it's minimized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveGridProperties()
		{
			CreateTempFile(m_xmlShell);

			using (DataGridView grid = CreateTestGrid())
			{
				SettingsHandler hndlr = new SettingsHandler(m_tempFilename);
				hndlr.SaveGridProperties(grid, null);

				XmlNode node = hndlr.FindChildNode("/settings/grids", "testgrid");
				Assert.IsNotNull(node);

				Assert.AreEqual(3, node.ChildNodes.Count);

				Assert.AreEqual(grid.ColumnHeadersHeight, XMLHelper.GetIntFromAttribute(node, "colheaderheight", -1));
				Assert.AreEqual(3, node.ChildNodes.Count);

				node = node.FirstChild;
				Assert.AreEqual("col3", XMLHelper.GetAttributeValue(node, "id"));
				Assert.AreEqual(grid.Columns[2].Width, XMLHelper.GetIntFromAttribute(node, "width", 0));
				Assert.AreEqual(grid.Columns[2].DisplayIndex, XMLHelper.GetIntFromAttribute(node, "displayindex", -1));
				Assert.IsTrue(XMLHelper.GetBoolFromAttribute(node, "visible"));

				node = node.NextSibling;
				Assert.AreEqual("col1", XMLHelper.GetAttributeValue(node, "id"));
				Assert.AreEqual(grid.Columns[0].Width, XMLHelper.GetIntFromAttribute(node, "width", 0));
				Assert.AreEqual(grid.Columns[0].DisplayIndex, XMLHelper.GetIntFromAttribute(node, "displayindex", -1));
				Assert.IsTrue(XMLHelper.GetBoolFromAttribute(node, "visible"));

				node = node.NextSibling;
				Assert.AreEqual("col2", XMLHelper.GetAttributeValue(node, "id"));
				Assert.AreEqual(grid.Columns[1].Width, XMLHelper.GetIntFromAttribute(node, "width", 0));
				Assert.IsFalse(XMLHelper.GetBoolFromAttribute(node, "visible"));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that a form's properties are not set when it's minimized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadGridProperties()
		{
			string xml = string.Format(m_xmlShell, "<grids><grid id=\"testgrid\" " +
				"colheaderheight=\"456\" dpi=\"96.0\">" +
				"<column id=\"col1\" width=\"100\" visible=\"false\"/>" +
				"<column id=\"col2\" width=\"200\" visible=\"true\" displayindex=\"1\"/>" +
				"<column id=\"col3\" width=\"300\" visible=\"true\" displayindex=\"0\"/></grid></grids>");

			CreateTempFile(xml);

			using (DataGridView grid = CreateTestGrid())
			{
				SettingsHandler hndlr = new SettingsHandler(m_tempFilename);
				string dummy;
				hndlr.LoadGridProperties(grid, out dummy);

				Assert.AreEqual(456, grid.ColumnHeadersHeight);
				Assert.AreEqual(100, grid.Columns[0].Width);
				Assert.IsFalse(grid.Columns[0].Visible);
				Assert.AreEqual(200, grid.Columns[1].Width);
				Assert.IsTrue(grid.Columns[1].Visible);
				Assert.AreEqual(1, grid.Columns[1].DisplayIndex);
				Assert.AreEqual(300, grid.Columns[2].Width);
				Assert.IsTrue(grid.Columns[2].Visible);
				Assert.AreEqual(0, grid.Columns[2].DisplayIndex);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private DataGridView CreateTestGrid()
		{
			DataGridView grid = new DataGridView();
			grid.Name = "testgrid";
			grid.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
			grid.ColumnHeadersHeight = 100;
			DataGridViewTextBoxCell templateCell = new DataGridViewTextBoxCell();

			grid.Columns.Add(new DataGridViewColumn(templateCell));
			grid.Columns[0].Name = "col1";
			grid.Columns[0].Width = 5;
			grid.Columns[0].Visible = true;

			grid.Columns.Add(new DataGridViewColumn(templateCell));
			grid.Columns[1].Name = "col2";
			grid.Columns[1].Width = 10;
			grid.Columns[1].Visible = false;

			grid.Columns.Add(new DataGridViewColumn(templateCell));
			grid.Columns[2].Name = "col3";
			grid.Columns[2].Width = 20;
			grid.Columns[2].Visible = true;
			grid.Columns[2].DisplayIndex = 0;

			return grid;
		}

		#endregion

		#region Tests for getting/setting general settings 
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that generic values are set for a window.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveWindowValues()
		{
			CreateTempFile(m_xmlShell);

			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);

			hndlr.SaveSettingsValue("gumby", "string", "pokey");
			hndlr.SaveSettingsValue("gumby", "int", 6372);
			hndlr.SaveSettingsValue("gumby", "bool", true);

			XmlNode node = hndlr.FindChildNode("/settings/misc", "gumby");
			Assert.IsNotNull(node);
			Assert.AreEqual("pokey", XMLHelper.GetAttributeValue(node, "string"));
			Assert.AreEqual(6372, XMLHelper.GetIntFromAttribute(node, "int", -1));
			Assert.IsTrue(XMLHelper.GetBoolFromAttribute(node, "bool", false));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that generic values are retrieved for a window.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetWindowValues()
		{
			string xml = string.Format(m_xmlShell, "<misc><setting id=\"gumby\" " +
				"string=\"pokey\" int=\"6372\" bool=\"True\" /></misc>");

			CreateTempFile(xml);

			SettingsHandler hndlr = new SettingsHandler(m_tempFilename);

			Assert.AreEqual("pokey", hndlr.GetStringSettingsValue("gumby", "string", string.Empty));
			Assert.AreEqual(6372, hndlr.GetIntSettingsValue("gumby", "int", -1));
			Assert.IsTrue(hndlr.GetBoolSettingsValue("gumby", "bool", false));
		}

		#endregion
	}
}
