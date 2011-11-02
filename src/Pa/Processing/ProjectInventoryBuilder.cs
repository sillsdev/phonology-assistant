using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	public class ProjectInventoryBuilder
	{
		public const string kVersion = "3.7";

		protected readonly PaProject _project;
		protected readonly PhoneCache _phoneCache;
		protected string _outputFileName;
		protected XmlWriter _writer;

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

			//CVChartBuilder.Process(project, CVChartType.Consonant);
			//CVChartBuilder.Process(project, CVChartType.Vowel);

			return buildResult;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Avoid external construction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ProjectInventoryBuilder(PaProject project)
		{
			_project = project;
			_phoneCache = project.PhoneCache;
			_outputFileName = _project.ProjectInventoryFileName;
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

			// REVIEW: Should we warn the user when this fails?
			if (pipeline == null)
				return false;

			App.InitializeProgressBar(ProgressMessage, pipeline.ProcessingSteps.Count);

			// Kick off the processing and then save the results to a file.
			pipeline.BeforeStepProcessed += BeforePipelineStepProcessed;

			if (input is MemoryStream)
				pipeline.Transform((MemoryStream)input, _outputFileName);
			else if (input is string)
				pipeline.Transform((string)input, _outputFileName);

			pipeline.BeforeStepProcessed -= BeforePipelineStepProcessed;

			// Some people were receiving the following exception:
			// "The requested operation cannot be performed on a file with a user-mapped section open."
			// This was happening on the doc.Save line, I think becaue the user's anti-virus program
			// had open the XML file just created). Therefore, give it 5 tries and then give up
			// without complaint. The only reason we do this part anyway, is to make the output pretty,
			// with proper indentation and line-breaking.
			for (int i = 0; i < 5; i++)
			{
				try
				{
					var doc = new XmlDocument();
					doc.Load(_outputFileName);
					doc.Save(_outputFileName);
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
				return ProcessHelper.MakeTempFilePath(_project,
					_project.ProjectPathFilePrefix + "PhoneticInventory.tmp");
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

		#region Methods for writing temp. inventory file to send through the xslt processing pipeline
		/// ------------------------------------------------------------------------------------
		protected virtual object CreateInputToTransformPipeline()
		{
			var memStream = new MemoryStream();
			
			using (_writer = XmlWriter.Create(memStream))
			{
				_writer.WriteStartDocument();
				WriteRoot();
				_writer.Flush();
				_writer.Close();
			}

			if (KeepTempFile)
				ProcessHelper.WriteStreamToFile(memStream, TempFileName);

			return memStream;
		}

		/// ------------------------------------------------------------------------------------
		private void WriteRoot()
		{
			ProcessHelper.WriteStartElementWithAttrib(_writer, "inventory", "version", kVersion);
			WriteRootAttributes();

			ProcessHelper.WriteMetadata(_writer, _project, true);
			
			XmlSerializationHelper.SerializeDataAndWriteAsNode(_writer, _project.TranscriptionChanges);
			XmlSerializationHelper.SerializeDataAndWriteAsNode(_writer, _project.AmbiguousSequences);

			if (_project.IgnoredSymbolsInCVCharts.Count > 0)
			{
				ProcessHelper.WriteStartElementWithAttrib(_writer, "symbols", "class", "ignoredInChart");
				foreach (var symbol in _project.IgnoredSymbolsInCVCharts)
					ProcessHelper.WriteStartElementWithAttribAndEmptyValue(_writer, "symbol", "literal", symbol);

				_writer.WriteEndElement();
			}

			_writer.WriteStartElement("segments");
			WritePhones();
			_writer.WriteEndElement();
			
			// Close inventory
			_writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void WriteRootAttributes()
		{
			_writer.WriteAttributeString("projectName", _project.Name);
			_writer.WriteAttributeString("languageName", _project.LanguageName);
			_writer.WriteAttributeString("languageCode", _project.LanguageCode);
		}

		/// ------------------------------------------------------------------------------------
		private void WritePhones()
		{
			foreach (var phone in _phoneCache.Keys)
			{
				ProcessHelper.WriteStartElementWithAttrib(_writer, "segment", "literal", phone);

				var phoneOverrides = _project.FeatureOverrides.GetOverridesForPhone(phone);
				if (phoneOverrides != null)
				{
					WritePhoneFeatureOverrides(phoneOverrides.AFeatureNames.ToArray(), "descriptive");
					WritePhoneFeatureOverrides(phoneOverrides.BFeatureNames.ToArray(), "distinctive");
				}
				
				_writer.WriteEndElement();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void WritePhoneFeatureOverrides(ICollection<string> featureNames, string featureType)
		{
			if (featureNames == null || featureNames.Count <= 0)
				return;

			ProcessHelper.WriteStartElementWithAttrib(_writer, "features", "class", featureType);

			foreach (var name in featureNames)
				_writer.WriteElementString("feature", name);

			_writer.WriteEndElement();
		}

		#endregion

		#region Methods for updating phone cache after a project inventory is created
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update each phone in the phone cache with information created in the process of
		/// building the project specific inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void PostBuildProcess()
		{
			var root = XElement.Load(_outputFileName);

			foreach (var segment in root.Elements("segments").Elements("segment"))
			{
				IPhoneInfo iPhoneInfo;
				if (_phoneCache.TryGetValue(segment.Attribute("literal").Value, out iPhoneInfo))
					UpdatePhoneInfoFromProjectInventory(segment, iPhoneInfo as PhoneInfo);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void UpdatePhoneInfoFromProjectInventory(XElement element, PhoneInfo phoneToUpdate)
		{
			if (!string.IsNullOrEmpty((string)element.Element("description")))
				phoneToUpdate.Description = (string)element.Element("description");

			phoneToUpdate.SetAFeatures(GetFeatureNames(element, "descriptive"));
			phoneToUpdate.SetBFeatures(GetFeatureNames(element, "distinctive"));
			phoneToUpdate.SetDefaultAFeatures(GetFeatureNames(element, "descriptive default"));
			phoneToUpdate.SetDefaultBFeatures(GetFeatureNames(element, "distinctive default"));
			phoneToUpdate.MOAKey = GetSortKeyFromSegmentXElement(element, "manner_or_height");
			phoneToUpdate.POAKey = GetSortKeyFromSegmentXElement(element, "place_or_backness");
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetFeatureNames(XElement element, string featureType)
		{
			foreach (var featureElements in element.Elements("features")
				.Where(e => (string)e.Attribute("class") == featureType))
			{
				return featureElements.Elements("feature").Select(e => e.Value);
			}

			return new List<string>(0);
		}

		/// ------------------------------------------------------------------------------------
		private string GetSortKeyFromSegmentXElement(XElement element, string sortType)
		{
			if (element.Element("keys") != null)
			{
				foreach (var sortKey in element.Element("keys").Elements("sortKey")
					.Where(e => (string)e.Attribute("class") == sortType).Select(e => e.Value))
				{
					return sortKey;
				}
			}

			return ("0");
		}

		#endregion
	}
}
