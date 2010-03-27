using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
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
		private const string kFileNameIntermediate = "PhoneticInventory.Intermediate.xml";
		private const string kFileNameRead = "PhoneticInventory.xml";

		[XmlArray("units"), XmlArrayItem("unit")]
		public List<TempPhoneInfo> Phones { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Process(PaProject project, PhoneCache phoneCache)
		{
			if (project == null || Settings.Default.SkipAdditionalProcessingWhenPhonesAreLoaded)
				return;

			// Create a stream of xml data containing the phones in the project.
			var inputStream = CreateIntermediateInventory(project, phoneCache);

			if (Settings.Default.KeepIntermediateProjectInventoryFile)
				WriteIntermediateFile(project, inputStream);

			// Create a processing pipeline for a series of xslt transforms to be applied to the stream.
			var processFileName = Path.Combine(App.ProcessingFolder, "Processing.xml");
			var pipeline = Pipeline.Create("inventory", processFileName, App.ProcessingFolder);
			if (pipeline == null)
				return;

			// Kick off the processing and then save the results to a file.
			var outputStream = pipeline.Transform(inputStream);
			var outputFileName = project.ProjectPathFilePrefix + kFileNameRead;
			using (var fileStream = new FileStream(outputFileName, FileMode.Create))
			{
				outputStream.WriteTo(fileStream);
				outputStream.Close();
				fileStream.Close();
			}

			// This makes it all pretty, with proper indentation and line-breaking.
			var doc = new XmlDocument();
			doc.Load(outputFileName);
			doc.Save(outputFileName);

			UpdatePhoneInformation(outputFileName, phoneCache);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This should only be done for debugging.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void WriteIntermediateFile(PaProject project, MemoryStream stream)
		{
			var intermediateFileName = project.ProjectPathFilePrefix + kFileNameIntermediate;
			using (var fileStream = new FileStream(intermediateFileName, FileMode.Create))
			{
				stream.WriteTo(fileStream);
				fileStream.Close();
			}

			// This makes it all pretty, with proper indentation and line-breaking.
			var doc = new XmlDocument();
			doc.Load(intermediateFileName);
			doc.Save(intermediateFileName);
		}
		
		#region Methods for writing inventory file to send through the xslt processing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static MemoryStream CreateIntermediateInventory(PaProject project,
			Dictionary<string, IPhoneInfo> phoneCache)
		{
			var memStream = new MemoryStream();
			
			using (var writer = XmlWriter.Create(memStream))
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
				WritePhones(writer, phoneCache);
				writer.WriteEndElement();

				// Close root and the writer.
				writer.WriteEndElement();
				writer.Flush();
				writer.Close();
			}

			return memStream;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void UpdatePhoneInformation(string fileName,
			IDictionary<string, IPhoneInfo> phoneCache)
		{
			var prjInventory = XmlSerializationHelper.DeserializeFromFile<ProjectInventory>(fileName);
			if (prjInventory == null)
				return;

			foreach (var phone in prjInventory.Phones)
			{
				IPhoneInfo iPhoneInfo;
				if (phoneCache.TryGetValue(phone.Literal, out iPhoneInfo))
				{
					var phoneInfo = iPhoneInfo as PhoneInfo;
					if (phoneInfo != null)
					{
						phoneInfo.AFeatures = phone.AFeatures;
						phoneInfo.BFeatures = phone.BFeatures;
						phoneInfo.Description = phone.Description;
						phoneInfo.MOAKey = phone.GetSortKey("mannerOfArticulation", phoneInfo.MOAKey);
						phoneInfo.POAKey = phone.GetSortKey("placeOfArticulation", phoneInfo.POAKey);
					}
				}
			}
		}

		#region TempPhoneInfo and TempPhoneSortKey classes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public class TempPhoneInfo : IPASymbol
		{
			[XmlArray("keys"), XmlArrayItem("sortKey")]
			public List<TempPhoneSortKey> SortKeys { get; set; }

			/// --------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// --------------------------------------------------------------------------------
			public string GetSortKey(string sortType, string origKey)
			{
				var newKey = SortKeys.FirstOrDefault(x => x.SortType == sortType);
				return (newKey != null ? newKey.Key : origKey);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public class TempPhoneSortKey
		{
			[XmlAttribute("class")]
			public string SortType { get; set; }

			[XmlText]
			public string Key { get; set; }
		}

		#endregion
	}
}
