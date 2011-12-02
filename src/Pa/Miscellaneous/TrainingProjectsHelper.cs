using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Ionic.Zip;
using SIL.Pa.Properties;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	public class TrainingProjectsHelper
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will unpack sample data into a sub-foldere of the user's default
		/// project folder. This is only done once, the first time the current user has
		/// run PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Setup()
		{
			try
			{
				// Don't bother unpacking if that's been done before.
				if (Settings.Default.TrainingProjectsUnpacked)
					return;

				var tpsi = TrainingProjectSetupInfo.Load();
				if (tpsi == null)
					return;

				// Can't unpack the samples if the training projects zip file doesn't exist.
				var zipFile = Path.Combine(Application.StartupPath, App.kTrainingSubFolder);
				zipFile = Path.Combine(zipFile, tpsi.TrainingProjectsZipFile);
				if (!File.Exists(zipFile))
					return;

				var destFolder = Path.Combine(App.ProjectFolder, tpsi.TrainingProjectFolder);

				// Make sure the target folder for the training projects exists.
				if (!Directory.Exists(destFolder))
					Directory.CreateDirectory(destFolder);

				using (var zip = new ZipFile(zipFile))
					zip.ExtractAll(destFolder, ExtractExistingFileAction.OverwriteSilently);

				// Save a value to the settings so training projects won't be unpacked
				// again. I could write this to the settings file but I don't want to
				// unpack if the user has deleted the training projects and his settings
				// file at some point after having already unpacked the training projects.
				Settings.Default.TrainingProjectsUnpacked = true;
				Settings.Default.Save();

				foreach (var papmod in tpsi.PapModifications)
					papmod.Modify(destFolder);
			}
			catch { }
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class TrainingProjectSetupInfo
	{
		public string TrainingProjectsZipFile;
		public string TrainingProjectFolder;

		[XmlArray("PapModifications"), XmlArrayItem("PapModification")]
		public List<RestoredPapDataSourceUpdater> PapModifications;

		/// ------------------------------------------------------------------------------------
		internal static TrainingProjectSetupInfo Load()
		{
			var path = Path.Combine(Application.StartupPath, App.kTrainingSubFolder);
			path = Path.Combine(path, "TrainingProjectsSetup.xml");

			try
			{
				return XmlSerializationHelper.DeserializeFromFile<TrainingProjectSetupInfo>(path);
			}
			catch { }

			return null;
		}
	}
}
