using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SIL.Localization;
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

		protected readonly PaProject m_project;
		protected readonly PhoneCache m_phoneCache;
		protected string m_outputFileName;
		protected XmlWriter m_writer;

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
			
			var bldr = new ProjectInventoryBuilder(project, phoneCache);
			var buildResult = bldr.InternalProcess();
			
			App.MsgMediator.SendMessage("AfterBuildProjectInventory",
				new object[] { project, phoneCache, buildResult });

			CVChartBuilder.Process(project, phoneCache, CVChartBuilder.ChartType.Consonant);
			CVChartBuilder.Process(project, phoneCache, CVChartBuilder.ChartType.Vowel);

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
		protected ProjectInventoryBuilder(PaProject project, PhoneCache phoneCache)
		{
			m_project = project;
			m_phoneCache = phoneCache;
			m_outputFileName = m_project.ProjectInventoryFileName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool InternalProcess()
		{
			// Create a stream of xml data containing the phones in the project.
			var input = CreateInputToTransformPipeline();

			if (!RunPipeline(input))
				return false;

			PostBuildProcess();
			App.UninitializeProgressBar();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool RunPipeline(object input)
		{
			// Create a processing pipeline for a series of xslt transforms to be applied to the stream.
			var pipeline = ProcessHelper.CreatePipline(ProcessType);

			// REVIEW: Should we warn the user that this failed?
			if (pipeline == null)
				return false;

			App.InitializeProgressBar(ProgressMessage, pipeline.ProcessingSteps.Count);

			// Kick off the processing and then save the results to a file.
			pipeline.BeforeStepProcessed += BeforePipelineStepProcessed;
			
			if (input is MemoryStream)
				pipeline.Transform((MemoryStream)input, m_outputFileName);
			else if (input is string)
				pipeline.Transform((string)input, m_outputFileName);
			
			pipeline.BeforeStepProcessed -= BeforePipelineStepProcessed;

			// This makes it all pretty, with proper indentation and line-breaking.
			var doc = new XmlDocument();
			doc.Load(m_outputFileName);
			doc.Save(m_outputFileName);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void BeforePipelineStepProcessed(Pipeline pipeline, Step step)
		{
			App.IncProgressBar();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string ProgressMessage
		{
			get
			{
				return LocalizationManager.LocalizeString("ProcessingPhoneInventoryMsg",
					"Processing Phone Inventory: ",
					"Message displayed whenever the phone inventory is built or updated.",
					App.kLocalizationGroupInfoMsg, LocalizationCategory.GeneralMessage,
					LocalizationPriority.Medium);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string TempFileName
		{
			get { return m_project.ProjectPathFilePrefix + "PhoneticInventory.tmp"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool KeepTempFile
		{
			get { return Settings.Default.KeepTempProjectInventoryFile; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual Pipeline.ProcessType ProcessType
		{
			get { return Pipeline.ProcessType.PrepareInventory; }
		}

		#endregion

		#region Methods for writing inventory file to send through the xslt processing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual object CreateInputToTransformPipeline()
		{
			var memStream = new MemoryStream();
			
			using (m_writer = XmlWriter.Create(memStream))
			{
				m_writer.WriteStartDocument();
				WriteRoot();
				m_writer.Flush();
				m_writer.Close();
			}

			if (KeepTempFile)
				ProcessHelper.WriteStreamToFile(memStream, TempFileName);

			return memStream;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WriteRoot()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "inventory", "version", kVersion);
			WriteRootAttributes();

			ProcessHelper.WriteMetadata(m_writer, m_project, true);
			
			XmlSerializationHelper.SerializeDataAndWriteAsNode(m_writer, App.IPASymbolCache.TranscriptionChanges);
			XmlSerializationHelper.SerializeDataAndWriteAsNode(m_writer, App.IPASymbolCache.AmbiguousSequences);

			ProcessHelper.WriteStartElementWithAttrib(m_writer, "units", "type", "phonetic");
			WritePhones();
			
			// Close units
			m_writer.WriteEndElement();

			// Close inventory
			m_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void WriteRootAttributes()
		{
			m_writer.WriteAttributeString("projectName", m_project.Name);
			m_writer.WriteAttributeString("languageName", m_project.LanguageName);
			m_writer.WriteAttributeString("languageCode", m_project.LanguageCode);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void WritePhones()
		{
			foreach (var phone in m_phoneCache)
			{
				ProcessHelper.WriteStartElementWithAttrib(m_writer, "unit", "literal", phone.Key);

				if (phone.Value.AFeaturesAreOverridden)
				{
					ProcessHelper.WriteStartElementWithAttrib(m_writer,
						"articulatoryFeatures", "changed", "true");

					foreach (var feature in ((PhoneInfo)phone.Value).AFeatures)
						m_writer.WriteElementString("features", feature);

					m_writer.WriteEndElement();
				}
				
				m_writer.WriteEndElement();
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update each phone in the phone cache with information created in the process of
		/// building the project specific inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void PostBuildProcess()
		{
			var prjInventory = XmlSerializationHelper.DeserializeFromFile<ProjectInventoryBuilder>(m_outputFileName);
			if (prjInventory == null)
				return;

			foreach (var phone in prjInventory.Phones)
			{
				IPhoneInfo iPhoneInfo;
				if (m_phoneCache.TryGetValue(phone.Literal, out iPhoneInfo))
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
