using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using SIL.SpeechTools.Utils;
using ZipUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SampleDataHelper
	{
		public const string kPaSampleDataZipFile = "SamplePaData.zip";
		public const string kPaSampleFolder = "Sample Project";
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will unpack sample data into a sub-foldere of the user's default
		/// project folder. This is only done once, the first time the current user has
		/// run PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UnpackSampleData()
		{
			// Don't bother unpacking if that's been done before.
			RegistryKey key = Registry.CurrentUser.OpenSubKey(PaApp.kPaRegKeyName);
			if (key != null && (int)key.GetValue("SamplesUnpacked", 0) > 0)
				return;

			// Can't unpack the samples if the samples zip file doesn't exist.
			string sampleZipFile = Path.Combine(Application.StartupPath, kPaSampleDataZipFile);
			if (!File.Exists(sampleZipFile))
				return;

			// Make sure the target folder for the samples exists.
			string sampleFolder = Path.Combine(PaApp.DefaultProjectFolder, kPaSampleFolder);
			if (!Directory.Exists(sampleFolder))
				Directory.CreateDirectory(sampleFolder);

			try
			{
				ZipHelper.UncompressFilesInZip(sampleZipFile, sampleFolder);
			}
			catch
			{
				return;
			}

			// Write a value to the registry so samples won't be unpacked again. I could
			// write this to the settings file but I don't want to unpack if the user
			// has deleted the samples and his settings file at some point after having
			// already unpacked the samples.
			key = Registry.CurrentUser.CreateSubKey(PaApp.kPaRegKeyName);
			key.SetValue("SamplesUnpacked", 1);

			ModifySampleProjectDataSourcePaths(sampleFolder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ModifySampleProjectDataSourcePaths(string sampleFolder)
		{
			string[] samplePapFiles = new string[] {};

			try
			{
				samplePapFiles = Directory.GetFiles(sampleFolder, "*.pap");
			}
			catch
			{
				return;
			}

			for (int i = 0; i < samplePapFiles.Length; i++)
			{
				try
				{
					PaProject prj = STUtils.DeserializeData(
						samplePapFiles[i], typeof(PaProject)) as PaProject;
					
					if (prj == null)
						continue;

					foreach (PaDataSource dataSource in prj.DataSources)
					{
						if (dataSource.DataSourceFile != null)
						{
							string filename = Path.GetFileName(dataSource.DataSourceFile);
							dataSource.DataSourceFile = Path.Combine(sampleFolder, filename);
						}
					}

					STUtils.SerializeData(samplePapFiles[i], prj);
					if (i == 0)
						PaApp.AddProjectToRecentlyUsedProjectsList(samplePapFiles[i], true);
				}
				catch { }
			}
		}
	}
}
