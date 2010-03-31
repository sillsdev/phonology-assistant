using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilUtils;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("inventory")]
	public class ProjectInventoryBuilder
	{
		public const string kVersion = "3.5";
		private const string kFileNameIntermediate = "PhoneticInventory.Intermediate.xml";

		private readonly PaProject m_project;
		private readonly PhoneCache m_phoneCache;
		
		[XmlArray("units"), XmlArrayItem("unit")]
		public List<TempPhoneInfo> Phones { get; set; }

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
			
			var prjInventoryBldr = new ProjectInventoryBuilder(project, phoneCache);
			var buildResult = prjInventoryBldr.InternalProcess();
			
			App.MsgMediator.SendMessage("AfterBuildProjectInventory",
				new object[] { project, phoneCache, buildResult });
			
			return buildResult;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For serialization/deserialization
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ProjectInventoryBuilder()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Avoid external construction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private ProjectInventoryBuilder(PaProject project, PhoneCache phoneCache)
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
			{
				var intermediateFileName = m_project.ProjectPathFilePrefix + kFileNameIntermediate;
				ProcessHelper.WriteStreamToFile(inputStream, intermediateFileName);
			}

			// Create a processing pipeline for a series of xslt transforms to be applied to the stream.
			var processFileName = Path.Combine(App.ProcessingFolder, "Processing.xml");
			var pipeline = Pipeline.Create(Pipeline.ProcessType.PrepareInventory,
				processFileName, App.ProcessingFolder);

			// REVIEW: Should we warn the user that this failed?
			if (pipeline == null)
				return false;

			// Kick off the processing and then save the results to a file.
			var outputFileName = m_project.ProjectInventoryFileName;
			pipeline.Transform(inputStream, outputFileName);

			// This makes it all pretty, with proper indentation and line-breaking.
			var doc = new XmlDocument();
			doc.Load(outputFileName);
			doc.Save(outputFileName);

			UpdatePhoneInformation(outputFileName, m_phoneCache);
			return true;
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
			ProcessHelper.WriteStartElementWithAttrib(writer, "inventory", "version", kVersion);
			writer.WriteAttributeString("projectName", m_project.Name);
			writer.WriteAttributeString("languageName", m_project.LanguageName);
			writer.WriteAttributeString("languageCode", m_project.LanguageCode);

			ProcessHelper.WriteMetadata(writer, m_project, true);
			
			XmlSerializationHelper.SerializeDataAndWriteAsNode(writer, App.IPASymbolCache.TranscriptionChanges);
			XmlSerializationHelper.SerializeDataAndWriteAsNode(writer, App.IPASymbolCache.AmbiguousSequences);

			ProcessHelper.WriteStartElementWithAttrib(writer, "units", "type", "phonetic");
			WritePhones(writer);
			
			// Close units
			writer.WriteEndElement();

			// Close inventory
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
				ProcessHelper.WriteStartElementWithAttrib(writer, "unit", "literal", phone.Key);

				if (phone.Value.AFeaturesAreOverridden)
				{
					ProcessHelper.WriteStartElementWithAttrib(writer,
						"articulatoryFeatures", "changed", "true");

					foreach (var feature in ((PhoneInfo)phone.Value).AFeatures)
						writer.WriteElementString("features", feature);

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
			var prjInventory = XmlSerializationHelper.DeserializeFromFile<ProjectInventoryBuilder>(fileName);
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
						if (phone.AFeatures.Count > 0)
							phoneInfo.SetAFeatures(phone.AFeatures);

						if (phone.BFeatures.Count > 0)
							phoneInfo.SetBFeatures(phone.BFeatures);

						if (!string.IsNullOrEmpty(phone.Description))
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
