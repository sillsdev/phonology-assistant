using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using SilUtils;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("inventory")]
	public class ProjectInventory
	{
		public const string kVersion = "3.5";
		private const string kFileNameWrite = "PhoneticInventory-out.xml";
		private const string kFileNameRead = "PhoneticInventory.xml";

		#region Methods for writing project's phonetic inventory file.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Save(PaProject project)
		{
			var filename = project.ProjectPathFilePrefix + kFileNameWrite;

			using (var writer = XmlWriter.Create(filename))
			{
				writer.WriteStartDocument();

				WriteRoot(writer, project);
				WriteMetadata(writer, project);
				XmlSerializationHelper.SerializeDataAndWriteAsNode(writer, App.IPASymbolCache.AmbiguousSequences);

				writer.WriteStartElement("units");
				writer.WriteStartAttribute("type");
				writer.WriteString("phonetic");
				writer.WriteEndAttribute();

				WritePhones(writer, App.PhoneCache);
				WritePhones(writer, App.GetPhonesFromWordCache(App.RecordCache.WordsNotInCurrentFilter));
				writer.WriteEndElement();

				// Close root and the writer.
				writer.WriteEndElement();
				writer.Flush();
				writer.Close();
			}

			// This makes it all pretty, with proper indentation and line-breaking.
			var doc = new XmlDocument();
			doc.Load(filename);
			doc.Save(filename);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void WriteRoot(XmlWriter writer, PaProject project)
		{
			writer.WriteStartElement("inventory");
			writer.WriteStartAttribute("version");
			writer.WriteString(kVersion);
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("projectName");
			writer.WriteString(project.Name);
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("languageName");
			writer.WriteString(project.LanguageName);
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("languageCode");
			writer.WriteString(project.LanguageCode);
			writer.WriteEndAttribute();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void WriteMetadata(XmlWriter writer, PaProject project)
		{
			writer.WriteStartElement("div");
			writer.WriteStartAttribute("id");
			writer.WriteString("metadata");
			writer.WriteEndAttribute();
				
			// Open ul and div
			writer.WriteStartElement("ul");
			writer.WriteStartAttribute("id");
			writer.WriteString("settings");
			writer.WriteEndAttribute();

			writer.WriteStartElement("li");
			writer.WriteStartAttribute("class");
			writer.WriteString("projectFolder");
			writer.WriteEndAttribute();
			writer.WriteString(project.ProjectPath);
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteStartAttribute("class");
			writer.WriteString("configurationFolder");
			writer.WriteEndAttribute();
			writer.WriteString(App.ConfigFolder);
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteStartAttribute("class");
			writer.WriteString("programPhoneticInventoryFile");
			writer.WriteEndAttribute();
			writer.WriteString(InventoryHelper.kDefaultInventoryFileName);
			writer.WriteEndElement();
	
			// Close ul and div
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void WritePhones(XmlWriter writer, Dictionary<string, IPhoneInfo> cache)
		{
			foreach (var phone in cache.Keys)
			{
				writer.WriteStartElement("unit");
				writer.WriteStartAttribute("literal");
				writer.WriteString(phone);
				writer.WriteEndAttribute();
				writer.WriteEndElement();
			}
		}

		#endregion
	}
}
