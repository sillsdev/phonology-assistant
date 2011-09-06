using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.Processing
{
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

		// I'm not thrilled about this approach for causing processing to be skipped when
		// tests are run, but it will work for now.
		public static bool SkipProcessingForTests;

		/// ------------------------------------------------------------------------------------
		public static bool Process(PaProject project)
		{
			if (project == null || SkipProcessingForTests ||
				Settings.Default.SkipAdditionalProcessingWhenPhonesAreLoaded)
			{
				return false;
			}

			App.MsgMediator.SendMessage("BeforeBuildProjectInventory", project);
			
			var bldr = new ProjectInventoryBuilder(project);
			var buildResult = bldr.InternalProcess();
			
			App.MsgMediator.SendMessage("AfterBuildProjectInventory",
				new object[] { project, buildResult });

			CVChartBuilder.Process(project, CVChartType.Consonant);
			CVChartBuilder.Process(project, CVChartType.Vowel);

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
		protected ProjectInventoryBuilder(PaProject project)
		{
			m_project = project;
			m_phoneCache = project.PhoneCache;
			m_outputFileName = m_project.ProjectInventoryFileName;
		}

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
		protected virtual bool RunPipeline(object input)
		{
			// Create a processing pipeline for a series of xslt transforms to be applied to the stream.
			var pipeline = ProcessHelper.CreatePipeline(ProcessType);

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

			// Some people were receiving the following exception:
			// "The requested operation cannot be performed on a file with a user-mapped section open."
			// This was happening on the doc.Save line. Therefore, give it 5 tries and then give up
			// without complaint. The only reason we do this part anyway, is to make the output pretty,
			// with proper indentation and line-breaking.
			for (int i = 0; i < 5; i++)
			{
				try
				{
					var doc = new XmlDocument();
					doc.Load(m_outputFileName);
					doc.Save(m_outputFileName);
					break;
				}
				catch
				{
					Thread.Sleep(500);
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected void BeforePipelineStepProcessed(Pipeline pipeline, Step step)
		{
			App.IncProgressBar();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		protected virtual string ProgressMessage
		{
			get
			{
				return App.GetString("ProcessingPhoneInventoryMsg",
					"Building Phone Inventory...",
					"Message displayed whenever the phone inventory is built or updated.");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string TempFileName
		{
			get
			{
				return ProcessHelper.MakeTempFilePath(m_project,
					m_project.ProjectPathFilePrefix + "PhoneticInventory.tmp");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool KeepTempFile
		{
			get { return Settings.Default.KeepTempProjectInventoryFile; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual Pipeline.ProcessType ProcessType
		{
			get { return Pipeline.ProcessType.PrepareInventory; }
		}

		#endregion

		#region Methods for writing inventory file to send through the xslt processing
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
		private void WriteRoot()
		{
			ProcessHelper.WriteStartElementWithAttrib(m_writer, "inventory", "version", kVersion);
			WriteRootAttributes();

			ProcessHelper.WriteMetadata(m_writer, m_project, true);
			
			XmlSerializationHelper.SerializeDataAndWriteAsNode(m_writer, m_project.TranscriptionChanges);
			XmlSerializationHelper.SerializeDataAndWriteAsNode(m_writer, m_project.AmbiguousSequences);

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
						m_writer.WriteElementString("feature", feature);

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
