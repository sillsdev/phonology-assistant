using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SilTools;

namespace SIL.Pa.Model.Migration
{
	public class Migration0330 : MigrationBase
	{
		private string m_projectFilePath;
		private string m_projectPathPrefix;

		/// ------------------------------------------------------------------------------------
		public static bool Migrate(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
		{
			var migrator = new Migration0330();
			return migrator.InternalMigration(prjfilepath, GetPrjPathPrefixAction);
		}

		/// ------------------------------------------------------------------------------------
		private bool InternalMigration(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
		{
			var e = BackupProject(prjfilepath, "0301");
			if (e != null)
			{
				var msg = App.LocalizeString("ProjectMigrationBackupErrMsg",
					"The following error occurred while attempting to backup your project before updating it for the latest version of {0}:\n\n{1}",
					App.kLocalizationGroupMisc);

				Utils.MsgBox(string.Format(msg, Application.ProductName, e.Message));
				return false;
			}

			m_projectFilePath = prjfilepath;
			m_projectPathPrefix = GetPrjPathPrefixAction(prjfilepath, ProjectName);

			MigrateAmbiguousSequences();
			MigrateExperimentalTranscriptions();
			MigrateFeatureOverrides();

			if (!MigrateProjectFile())
			{
				// TODO: Revert backed-up project.
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateAmbiguousSequences()
		{
			var filepath = AmbiguousSequences.GetFileForProject(m_projectPathPrefix);
			if (!File.Exists(filepath))
				return;

			string errMsg;
			if (TransformFile(filepath, "SIL.Pa.Model.Migration.UpdateAmbiguousSequenceFile.xslt", out errMsg))
				return;

			var msg = App.LocalizeString("AmbiguousSeqMigrationErrMsg",
				"The following error occurred while attempting to update your project’s ambiguous " +
				"sequences file:\n\n{0}\n\nIn order to continue working and because your project files " +
				"have been backed up, the program will continue without ambiguous sequences for this project.",
				App.kLocalizationGroupMisc);

			Utils.MsgBox(string.Format(msg, errMsg));
			return;
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateExperimentalTranscriptions()
		{
			var oldfilepath = m_projectPathPrefix + "ExperimentalTranscriptions.xml";
			if (!File.Exists(oldfilepath))
				return;

			var newfilepath = TranscriptionChanges.GetFileForProject(m_projectPathPrefix);

			string errMsg;
			if (TransformFile(oldfilepath, "SIL.Pa.Model.Migration.UpdateExperimentalTranscriptionFile.xslt", out errMsg))
			{
				// The old file has been transformed, now give it a new name since
				// experimental transcriptions are now called transcription changes.
				if (File.Exists(oldfilepath) && !File.Exists(newfilepath))
					File.Move(oldfilepath, newfilepath);
				
				return;
			}

			var msg = App.LocalizeString("TranscriptionChangesMigrationErrMsg",
				"The following error occurred while attempting to update your project’s " +
				"transcription changes file (formerly experimental transcriptions):\n\n{0}\n\n" +
				"In order to continue working and because your project files have been backed up, " +
				"the program will continue without transcription changes for this project.",
				App.kLocalizationGroupMisc);

			Utils.MsgBox(string.Format(msg, errMsg));

			try
			{
				File.Delete(oldfilepath);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateFeatureOverrides()
		{
			var filepath = FeatureOverrides.GetFileForProject(m_projectPathPrefix);
			if (!File.Exists(filepath))
				return;

			string errMsg;
			if (TransformFile(filepath, "SIL.Pa.Model.Migration.UpdateFeatureOverridesFile.xslt", out errMsg))
				return;
			
			var msg = App.LocalizeString("FeatureOverridesMigrationErrMsg",
				"The following error occurred while attempting to update your project’s " +
				"feature overrides file:\n\n{0}\n\n In order to continue working and because " +
				"your project files have been backed up, the program will continue without " +
				"overriding any features for this project.", App.kLocalizationGroupMisc);

			Utils.MsgBox(string.Format(msg, errMsg));
		}

		/// ------------------------------------------------------------------------------------
		private bool MigrateProjectFile()
		{
			string errMsg;

			if (TransformFile(m_projectFilePath, "SIL.Pa.Model.Migration.UpdateProjectFile.xslt", out errMsg))
			{
				UpdateOldFields();
				return true;
			}

			//var msg = App.LocalizeString("ProjectFileMigrationErrMsg",
			//    "The following error occurred while attempting to update your project’s " +
			//    "transcription changes file (formerly experimental transcriptions):\n\n{0}\n\n" +
			//    "In order to continue working and because your project files have been backed up, " +
			//    "the program will continue without transcription changes for this project.",
			//    App.kLocalizationGroupMisc);


			return false;
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateOldFields()
		{
			var fldInfoFilePath = m_projectPathPrefix + "FieldInfo.xml";
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

			var newFieldsFilePath = PaField.GetFileForProject(m_projectPathPrefix);
			File.Move(fldInfoFilePath, newFieldsFilePath);
			string errMsg;

			if (!TransformFile(newFieldsFilePath, "SIL.Pa.Model.Migration.UpdateFieldInfo.xslt", out errMsg))
			{
				try
				{
					File.Delete(newFieldsFilePath);
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateProjectMappingsIsParsedValuesFromOldFieldInfo(XElement xmlFieldInfo)
		{
			var xmlProject = XElement.Load(m_projectFilePath);

			// Get all mapping nodes for SFM/Toolbox type data sources.
			var sfmElements = xmlProject.Element("DataSources").Elements()
				.Where(e =>"SFM;Toolbox".Contains((string)e.Element("Type")))
				.Select(e => e.Element("FieldMappings"))
				.SelectMany(e => e.Elements("mapping"));

			// Get all parsed fields from the old field info.
			var parsedFields = from e in xmlFieldInfo.Elements()
							   where (string)e.Element("IsParsed") == "true"
							   select e.Attribute("Name").Value;

			// Update the mappings to make sure those fields that were marked as parsed in
			// the old field info. have their isParsed property set in the new mappings.
			foreach (var element in sfmElements.Where(e => parsedFields.Contains((string)e.Element("paFieldName"))))
				element.Add(new XElement("isParsed", "true"));
		
			xmlProject.Save(m_projectFilePath);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateFieldDisplayPropsFile(string fldInfoFilePath)
		{
			var displayPropsFilePath = PaFieldDisplayProperties.GetFileForProject(m_projectPathPrefix);
			File.Copy(fldInfoFilePath, displayPropsFilePath);
			string errMsg;
			
			if (!TransformFile(displayPropsFilePath, "SIL.Pa.Model.Migration.CreateFieldDisplayProperties.xslt", out errMsg))
			{
				try
				{
					File.Delete(displayPropsFilePath);
				}
				catch { }
			}
		}
	}
}
