using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TrainingProjectsHelper
	{
		private const string kRegKey = "TrainingProjectsUnpacked";

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
				RegistryKey key = Registry.CurrentUser.OpenSubKey(PaApp.kPaRegKeyName);
				if (key != null && (int)key.GetValue(kRegKey, 0) > 0)
					return;

				TrainingProjectSetupInfo tpsi = TrainingProjectSetupInfo.Load();
				if (tpsi == null)
					return;

				// Can't unpack the samples if the training projects zip file doesn't exist.
				string zipFile = Path.Combine(Application.StartupPath, PaApp.kTrainingSubFolder);
				zipFile = Path.Combine(zipFile, tpsi.TrainingProjectsZipFile);
				if (!File.Exists(zipFile))
					return;

				// Make sure the target folder for the training projects exists.
				string destFolder = Path.Combine(PaApp.DefaultProjectFolder,
					tpsi.TrainingProjectFolder);

				// Do this just in case there's a dot or dot, dot at the end.
				destFolder = Path.GetDirectoryName(destFolder);

				if (!Directory.Exists(destFolder))
					Directory.CreateDirectory(destFolder);

				ZipHelper.UncompressFilesInZip(zipFile, destFolder);

				// Write a value to the registry so training projects won't be unpacked
				// again. I could write this to the settings file but I don't want to
				// unpack if the user has deleted the training projects and his settings
				// file at some point after having already unpacked the training projects.
				key = Registry.CurrentUser.CreateSubKey(PaApp.kPaRegKeyName);
				key.SetValue(kRegKey, 1);

				foreach (PapModification papmod in tpsi.PapModifications)
					papmod.Modify(destFolder);

				tpsi.Delete();
			}
			catch { }
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TrainingProjectSetupInfo
	{
		public string TrainingProjectsZipFile;
		public string TrainingProjectFolder;
		public List<PapModification> PapModifications;
		private static string s_tpsPath;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static TrainingProjectSetupInfo Load()
		{
			s_tpsPath = Path.Combine(Application.StartupPath, PaApp.kTrainingSubFolder);
			s_tpsPath = Path.Combine(s_tpsPath, "TrainingProjectsSetup.xml");

			try
			{
				TrainingProjectSetupInfo tpsi = SilUtils.Utils.DeserializeData(
					s_tpsPath, typeof(TrainingProjectSetupInfo)) as TrainingProjectSetupInfo;

				return tpsi;
			}
			catch { }

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Delete()
		{
			try
			{
				File.Delete(s_tpsPath);
			}
			catch
			{
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PapModification
	{
		public string PapFile;
		public string DataSourcePath;

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
			string papFilePath = Path.Combine(path, PapFile);
			if (!File.Exists(papFilePath))
				return;

			PaProject prj = SilUtils.Utils.DeserializeData(papFilePath, typeof(PaProject)) as PaProject;
			if (prj == null)
				return;

			foreach (PaDataSource dataSource in prj.DataSources)
			{
				if (dataSource.DataSourceFile != null)
				{
					string newPath = Path.Combine(path, DataSourcePath);
					string filename = Path.GetFileName(dataSource.DataSourceFile);
					dataSource.DataSourceFile = Path.Combine(newPath, filename);
				}
			}

			SilUtils.Utils.SerializeData(papFilePath, prj);
			PaApp.AddProjectToRecentlyUsedProjectsList(papFilePath, true);
		}
	}
}
