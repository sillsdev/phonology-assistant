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

		private readonly PaProject m_project;
		private readonly PhoneCache m_phoneCache;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Process(PaProject project, PhoneCache phoneCache)
		{
			if (project == null || Settings.Default.SkipAdditionalProcessingWhenPhonesAreLoaded)
				return false;

			App.MsgMediator.SendMessage("BeforeBuildProjectInventory",
				new object[] { project, phoneCache });
			
			var prjInventoryBldr = new ProjectInventory(project, phoneCache);
			var buildResult = prjInventoryBldr.InternalProcess();
			
			App.MsgMediator.SendMessage("AfterBuildProjectInventory",
				new object[] { project, phoneCache, buildResult });
			
			return buildResult;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Avoid external construction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private ProjectInventory(PaProject project, PhoneCache phoneCache)
		{
			m_project = project;
			m_phoneCache = phoneCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool InternalProcess()
		{
			// Create a stream of xml data containing the phones in the project.
			var inputStream = CreateIntermediateInventory();

			if (Settings.Default.KeepIntermediateProjectInventoryFile)
				WriteIntermediateFile(inputStream);

			// Create a processing pipeline for a series of xslt transforms to be applied to the stream.
			var processFileName = Path.Combine(App.ProcessingFolder, "Processing.xml");
			var pipeline = Pipeline.Create("inventory", processFileName, App.ProcessingFolder);

			// REVIEW: Should we warn the user that this failed?
			if (pipeline == null)
				return false;

			// Kick off the processing and then save the results to a file.
			var outputFileName = m_project.ProjectPathFilePrefix + kFileNameRead;
			pipeline.Transform(inputStream, outputFileName);

			// This makes it all pretty, with proper indentation and line-breaking.
			var doc = new XmlDocument();
			doc.Load(outputFileName);
			doc.Save(outputFileName);

			UpdatePhoneInformation(outputFileName, m_phoneCache);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This should only be done for debugging.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteIntermediateFile(MemoryStream stream)
		{
			var intermediateFileName = m_project.ProjectPathFilePrefix + kFileNameIntermediate;
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
		private MemoryStream CreateIntermediateInventory()
		{
			var memStream = new MemoryStream();
			
			using (var writer = XmlWriter.Create(memStream))
			{
				writer.WriteStartDocument();

				WriteRoot(writer);
				WriteMetadata(writer);
				XmlSerializationHelper.SerializeDataAndWriteAsNode(writer, App.IPASymbolCache.TranscriptionChanges);
				XmlSerializationHelper.SerializeDataAndWriteAsNode(writer, App.IPASymbolCache.AmbiguousSequences);

				writer.WriteStartElement("units");
				writer.WriteStartAttribute("type");
				writer.WriteString("phonetic");
				writer.WriteEndAttribute();
				WritePhones(writer);
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
		private void WriteRoot(XmlWriter writer)
		{
			writer.WriteStartElement("inventory");
			writer.WriteStartAttribute("version");
			writer.WriteString(kVersion);
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("projectName");
			writer.WriteString(m_project.Name);
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("languageName");
			writer.WriteString(m_project.LanguageName);
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("languageCode");
			writer.WriteString(m_project.LanguageCode);
			writer.WriteEndAttribute();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteMetadata(XmlWriter writer)
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

			var path = m_project.ProjectPath;
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
		private void WritePhones(XmlWriter writer)
		{
			foreach (var phone in m_phoneCache)
			{
				writer.WriteStartElement("unit");
				writer.WriteStartAttribute("literal");
				writer.WriteString(phone.Key);
				writer.WriteEndAttribute();

				if (phone.Value.AFeaturesAreOverridden)
				{
					writer.WriteStartElement("articulatoryFeatures");
					writer.WriteStartAttribute("changed");
					writer.WriteString("true");
					writer.WriteEndAttribute();

					foreach (var feature in ((PhoneInfo)phone.Value).AFeatures)
					{
						writer.WriteStartElement("features");
						writer.WriteString(feature);
						writer.WriteEndElement();
					}

					writer.WriteEndElement();
				}
				
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
