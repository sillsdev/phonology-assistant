using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using SIL.Pa.Model;
using SIL.Pa.Properties;
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
				ZipHelper.UncompressFilesInZip(zipFile, destFolder); // creates destFolder if necessary

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
		public List<PapModification> PapModifications;

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

	/// ----------------------------------------------------------------------------------------
	public class PapModification
	{
		[XmlAttribute("papFile")]
		public string PapFile;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines the specified path with the PapFile path to point to a pap file. That
		/// pap file is deserialized and each data source path in the pap file is modified
		/// to use that combined path. Then the pap file is rewritten and added to the
		/// program's list of recent projects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Modify(string path)
		{
			// .xml file set up with Windows-style paths so replace with appropriate separator
			string papFilePath = Path.Combine(path, PapFile.Replace('\\',Path.DirectorySeparatorChar));
			if (!File.Exists(papFilePath))
				return;

			var prj = XmlSerializationHelper.DeserializeFromFile<PaProject>(papFilePath);
			if (prj == null)
				return;

			foreach (var dataSource in prj.DataSources.Where(ds => ds.SourceFile != null))
			{
				string newPath = Path.GetDirectoryName(papFilePath);
				string filename = dataSource.SourceFile;
				// like Path.GetFileName(), keep only filename portion but
				// using Windows separator, even on Linux
				var lastSlash = filename.LastIndexOf('\\');
				if (lastSlash >= 0)
					filename = filename.Substring(lastSlash + 1);
				dataSource.SourceFile = Path.Combine(newPath, filename);
			}

			XmlSerializationHelper.SerializeToFile(papFilePath, prj);
			App.AddProjectToRecentlyUsedProjectsList(papFilePath, true);
		}
	}
}
