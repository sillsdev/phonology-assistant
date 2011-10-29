using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SIL.Pa.Model.Migration
{
	public class Migration0330 : MigrationBase
	{
		/// ------------------------------------------------------------------------------------
		public static Exception Migrate(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
		{
			var migrator = new Migration0330(prjfilepath, GetPrjPathPrefixAction);
			return migrator.DoMigration();
		}
		
		/// ------------------------------------------------------------------------------------
		private Migration0330(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
			: base(prjfilepath, GetPrjPathPrefixAction)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override void InternalMigration()
		{
			MigrateAmbiguousSequences();
			MigrateExperimentalTranscriptions();
			MigrateFeatureOverrides();
			MigrateProjectFile();
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateAmbiguousSequences()
		{
			var filepath = AmbiguousSequences.GetFileForProject(_projectPathPrefix);
			if (!File.Exists(filepath))
				return;

			var error = TransformFile(filepath, "SIL.Pa.Model.Migration.UpdateAmbiguousSequenceFile.xslt");
			if (error != null)
				throw error;
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateExperimentalTranscriptions()
		{
			var oldfilepath = _projectPathPrefix + "ExperimentalTranscriptions.xml";
			if (!File.Exists(oldfilepath))
				return;

			var newfilepath = TranscriptionChanges.GetFileForProject(_projectPathPrefix);

			var error = TransformFile(oldfilepath, "SIL.Pa.Model.Migration.UpdateExperimentalTranscriptionFile.xslt");
			if (error == null)
			{
				// The old file has been transformed, now give it a new name since
				// experimental transcriptions are now called transcription changes.
				if (File.Exists(oldfilepath) && !File.Exists(newfilepath))
					File.Move(oldfilepath, newfilepath);
				
				return;
			}

			try
			{
				File.Delete(oldfilepath);
			}
			catch { }

			throw error;
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateFeatureOverrides()
		{
			var filepath = FeatureOverrides.GetFileForProject(_projectPathPrefix);
			if (!File.Exists(filepath))
				return;

			var error = TransformFile(filepath, "SIL.Pa.Model.Migration.UpdateFeatureOverridesFile.xslt");
			if (error != null)
				throw error;
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateProjectFile()
		{
			var error = TransformFile(_projectFilePath, "SIL.Pa.Model.Migration.UpdateProjectFile.xslt");

			if (error != null)
				throw error;

			UpdateFields();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateFields()
		{
			var fldInfoFilePath = _projectPathPrefix + "FieldInfo.xml";
			if (!File.Exists(fldInfoFilePath))
				return;

			var xmlFieldInfo = XElement.Load(fldInfoFilePath);

			foreach (var element in xmlFieldInfo.Elements())
			{
				string newName = null;

				switch (element.Attribute("Name").Value)
				{
					case "Secondary Gloss": newName = "Gloss-Secondary"; break;
					case "Other Gloss": newName = "Gloss-Other"; break;
					case "Part of Speech": newName = "PartOfSpeech"; break;
					case "CV Pattern": newName = PaField.kCVPatternFieldName; break;
					case "Free Translation": newName = "FreeFormTranslation"; break;
					case "Notebook Ref.": newName = "NoteBookReference"; break;
					case "Ethnologue Id": newName = "EthnologueId"; break;
					case "Language Name": newName = "LanguageName"; break;
					case "Speaker": newName = "SpeakerName"; break;
					case "Speaker's Gender": newName = "SpeakerGender"; break;
					case "Comment": newName = "Note"; break;
					case "Data Source": newName = PaField.kDataSourceFieldName; break;
					case "Data Source Path": newName = PaField.kDataSourcePathFieldName; break;
					case "Audio File": newName = "AudioFile"; break;
				}

				if (newName != null)
					element.Attribute("Name").Value = newName;
			}

			xmlFieldInfo.Save(fldInfoFilePath);
			UpdateProjectMappingsIsParsedValuesFromOldFieldInfo(xmlFieldInfo);
			CreateFieldDisplayPropsFile(fldInfoFilePath);

			var newFieldsFilePath = PaField.GetFileForProject(_projectPathPrefix);
			File.Move(fldInfoFilePath, newFieldsFilePath);

			var error = TransformFile(newFieldsFilePath, "SIL.Pa.Model.Migration.UpdateFieldInfo.xslt");
			if (error != null)
				throw error;

			try
			{
				File.Delete(newFieldsFilePath);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateProjectMappingsIsParsedValuesFromOldFieldInfo(XElement xmlFieldInfo)
		{
			var xmlProject = XElement.Load(_projectFilePath);

			// Get all mapping nodes.
			var mappings = (from e in xmlProject.Element("DataSources").Elements()
							let fm = e.Element("FieldMappings")
							where fm != null
							select fm).SelectMany(e => e.Elements("mapping"));

			// Get all parsed fields from the old field info.
			var parsedFields = from e in xmlFieldInfo.Elements()
							   where (string)e.Element("IsParsed") == "true"
							   select e.Attribute("Name").Value;

			// Update the mappings to make sure those fields that were marked as parsed in
			// the old field info. have their isParsed property set in the new mappings.
			foreach (var element in mappings.Where(e => parsedFields.Contains((string)e.Element("paFieldName"))))
				element.Add(new XElement("isParsed", "true"));
		
			xmlProject.Save(_projectFilePath);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateFieldDisplayPropsFile(string fldInfoFilePath)
		{
			var displayPropsFilePath = PaFieldDisplayProperties.GetFileForProject(_projectPathPrefix);
			File.Copy(fldInfoFilePath, displayPropsFilePath);

			var error = TransformFile(displayPropsFilePath, "SIL.Pa.Model.Migration.CreateFieldDisplayProperties.xslt");
			if (error != null)
				throw error;

			try
			{
				File.Delete(displayPropsFilePath);
			}
			catch { }
		}
	}
}
