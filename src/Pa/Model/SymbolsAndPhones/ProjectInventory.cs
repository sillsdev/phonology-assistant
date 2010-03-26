using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using SIL.Pa.Processing;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Process(PaProject project)
		{
			if (project == null)
				return;

			var initialFilename = project.ProjectPathFilePrefix + kFileNameWrite;
			CreateProjectIventory(initialFilename, project);
			
			var finalFilename = project.ProjectPathFilePrefix + kFileNameRead;
			var processFileName = Path.Combine(App.ProcessingFolder, "Processing.xml");
			
			var pipeline = Pipeline.Create("inventory", processFileName, App.ProcessingFolder);
			if (pipeline != null)
				pipeline.Transform(initialFilename, finalFilename);
			
			//var xsltFilename = Path.Combine(App.ConfigFolder, "phonology_project_inventory.xsl");
			//var readFilename = project.ProjectPathFilePrefix + kFileNameRead;

			//using (var reader = new XmlTextReader(xsltFilename))
			//{
			//    var xslt = new XslCompiledTransform();
			//    var settings = new XsltSettings();
			//    settings.EnableDocumentFunction = true;
			//    xslt.Load(reader, settings, null);
			//    xslt.Transform(writeFilename, readFilename);
			//    reader.Close();

			//    // This makes it all pretty, with proper indentation and line-breaking.
			//    var doc = new XmlDocument();
			//    doc.Load(readFilename);
			//    doc.Save(readFilename);
			//}

		}
		
				// Here is sample code to create a pipeline and transform data.
		//static void example()
		//{
		//    // Create the pipeline.
		//    var processFileName = Path.Combine(App.ProcessingFolder, "Processing.xml");
		//    var pipeline = Pipeline.Create("inventory", processFileName, App.ProcessingFolder);
		//    if (pipeline == null)
		//        return;

		//    // Use the pipeline.
		//    Stream inputStream = null; // Represents what the program produces.
		//    MemoryStream outputStream = null;
		//    pipeline.Transform(inputStream, ref outputStream);
		//    string outputFilePath = "...\\PROJECT.PhoneticInventory.xml";
		//    using (Stream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
		//    {
		//        outputStream.WriteTo(outputFileStream);
		//        outputStream.Close();
		//    }
		//}

		#region Methods for writing project's phonetic inventory file.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void CreateProjectIventory(string outputFile, PaProject project)
		{
			if (project == null)
				return;
			
			using (var writer = XmlWriter.Create(outputFile))
			{
				writer.WriteStartDocument();

				WriteRoot(writer, project);
				WriteMetadata(writer, project);
				XmlSerializationHelper.SerializeDataAndWriteAsNode(writer, App.IPASymbolCache.TranscriptionChanges);
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
			doc.Load(outputFile);
			doc.Save(outputFile);
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

			var path = project.ProjectPath;
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				path += Path.DirectorySeparatorChar.ToString();

			writer.WriteStartElement("li");
			writer.WriteStartAttribute("class");
			writer.WriteString("projectFolder");
			writer.WriteEndAttribute();
			writer.WriteString(path);
			writer.WriteEndElement();

			path = App.ConfigFolder;
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				path += Path.DirectorySeparatorChar.ToString();

			writer.WriteStartElement("li");
			writer.WriteStartAttribute("class");
			writer.WriteString("programConfigurationFolder");
			writer.WriteEndAttribute();
			writer.WriteString(path);
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
