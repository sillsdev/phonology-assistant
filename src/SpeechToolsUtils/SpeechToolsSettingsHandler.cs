using System.Drawing;
using System.IO;
using System.Xml;

namespace SIL.SpeechTools.Utils
{
	public class SpeechToolsSettingsHandler : SettingsHandler
	{
		private const string kSettingsFileName = "CommonStSettings.xml";
		private const string kIPASoundNode = kMiscSettingsNode + "/ipasounds";
		private const string kFontsNode = kRootNodeName + "/fonts";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SpeechToolsSettingsHandler() : base(null)
		{
			m_settingsFile = Path.Combine(STUtils.SilSoftwareCommonFilesPath, kSettingsFileName);
			m_xmlDoc = new XmlDocument();

			try
			{
				m_xmlDoc.Load(m_settingsFile);
			}
			catch
			{
				CreateNewSettingsFile();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the relative path for the IPA sound files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string IPASoundsFile
		{
			set {SetStringValue(kIPASoundNode, "RelPath", value);}
			get
			{
				if (m_xmlDoc == null)
					return null;

				XmlNode node = m_xmlDoc.SelectSingleNode(kIPASoundNode);
				if (node == null)
					return null;

				return XMLHelper.GetAttributeValue(node, "RelPath");
			}
		}

		#region Font Related Methods
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Loads fonts for transcription fields.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public void LoadFonts()
		{
			XmlNode node = VerifyNodeExists(kFontsNode);
			if (node == null)
				return;

			foreach (XmlNode childNode in node.ChildNodes)
			{
				string id = XMLHelper.GetAttributeValue(childNode, "id");
				string name = XMLHelper.GetAttributeValue(childNode, "name");
				if (id == null || name == null)
					continue;

				int size = XMLHelper.GetIntFromAttribute(childNode, "size", 12);

				FontStyle style = FontStyle.Regular;
				if (XMLHelper.GetBoolFromAttribute(childNode, "bold"))
					style = FontStyle.Bold;

				if (XMLHelper.GetBoolFromAttribute(childNode, "italic"))
					style |= FontStyle.Italic;

				Font font = FontHelper.MakeFont(name, size, style);

				switch (id)
				{
					case "etic": FontHelper.PhoneticFont = font; break;
					case "tone": FontHelper.ToneFont = font; break;
					case "emic": FontHelper.PhonemicFont = font; break;
					case "ortho": FontHelper.OrthograpicFont = font; break;
					case "gloss": FontHelper.GlossFont = font; break;
					case "pos": FontHelper.POSFont = font; break;
					case "ref": FontHelper.ReferenceFont = font; break;
				}
			}
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Loads fonts for transcription fields.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public void SaveFonts()
		{
			XmlNode node = VerifyNodeExists(kFontsNode);
			if (node != null)
				node.RemoveAll();

			node.AppendChild(GetFontElement("etic", FontHelper.PhoneticFont));
			node.AppendChild(GetFontElement("tone", FontHelper.ToneFont));
			node.AppendChild(GetFontElement("emic", FontHelper.PhonemicFont));
			node.AppendChild(GetFontElement("ortho", FontHelper.OrthograpicFont));
			node.AppendChild(GetFontElement("gloss", FontHelper.GlossFont));
			node.AppendChild(GetFontElement("pos", FontHelper.POSFont));
			node.AppendChild(GetFontElement("ref", FontHelper.ReferenceFont));
			m_xmlDoc.Save(m_settingsFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a font element for the settings file.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="fnt"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private XmlElement GetFontElement(string id, Font fnt)
		{
			XmlElement element = m_xmlDoc.CreateElement("font");
			element.SetAttribute("id", id);
			element.SetAttribute("name", fnt.Name);
			element.SetAttribute("size", fnt.SizeInPoints.ToString());
			element.SetAttribute("bold", fnt.Bold.ToString());
			element.SetAttribute("italic", fnt.Italic.ToString());
			return element;
		}

		#endregion
	}
}
