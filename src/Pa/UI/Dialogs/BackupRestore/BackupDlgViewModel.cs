using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using Ionic.Zip;
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.Properties;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public class BackupDlgViewModel : BackupRestoreDlgViewModelBase
	{
		/// ------------------------------------------------------------------------------------
		public BackupDlgViewModel(PaProject project) : base(project)
		{
			var fmt = App.GetString("DialogBoxes.BackupDlg.BackupFileNameFormat", "{0}_({1}).pabackup");
			BackupFile = string.Format(fmt, Project.Name, DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"));
			TargetFolder = GetTargetFolderAndCreatingDefaultIfNonExistant();

			_infoFile = CreateBackupInfoFile();
			_prjFiles = GetProjectFilesToBackup().ToList();
			_dataSourceFiles = GetDataSourceFilesToBackup().ToList();
			_audioFiles = GetAudioFilesToBackup().ToList();
		}

		/// ------------------------------------------------------------------------------------
		private string GetTargetFolderAndCreatingDefaultIfNonExistant()
		{
			var folder = Settings.Default.LastBackupFolder;
			
			if (folder == null || !Directory.Exists(folder))
			{
				folder = DefaultBackupFolder;
				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);
			}

			return folder;
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetProjectFilesToBackup()
		{
			yield return Project.FileName;

			foreach (var file in Directory.GetFiles(Project.Folder, Project.Name + ".*.xml"))
			{
				if (file != Project.ConsonantChartLayoutFile &&
					file != Project.VowelChartLayoutFile &&
					file != Project.ProjectInventoryFileName)
				{
					yield return file;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetDataSourceFilesToBackup()
		{
			if (Project.DataSources != null)
			{
				foreach (var ds in Project.DataSources.Where(d => d.SourceFile != null &&
					d.Type != DataSourceType.FW && d.Type != DataSourceType.FW7))
				{
					yield return ds.SourceFile;

					// If the data source is an SA data source, then make sure the file
					// containing the transcriptions is also included in the back up.
					if (ds.Type == DataSourceType.SA)
						yield return Path.ChangeExtension(ds.SourceFile, "saxml");
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetAudioFilesToBackup()
		{
			return (from wentry in Project.WordCache
					let audioFile = wentry.GetAudioFileUsingFallBackIfNecessary()
					where audioFile != null
					select audioFile).Distinct(StringComparer.Ordinal);
		}

		/// ------------------------------------------------------------------------------------
		public int GetNumberOfFilesToBackup(bool includeDataSourceFiles, bool includeAudioFiles)
		{
			int count = _prjFiles.Count;

			if (includeDataSourceFiles)
				count += _dataSourceFiles.Count;
			
			if (includeAudioFiles)
				count += _audioFiles.Count;

			// Add one for the info. file.
			return count + 1;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetAreAllDataSourcesFieldWorks()
		{
			return Project.DataSources.All(ds => ds.Type == DataSourceType.FW ||
				ds.Type == DataSourceType.FW7);
		}

		/// ------------------------------------------------------------------------------------
		public void Backup(bool includeDataSourceFiles, bool includeSoundFiles,
			Action<int> reportProgressAction, Action finishedAction)
		{
			if (!includeDataSourceFiles)
				_dataSourceFiles.Clear();

			if (!includeSoundFiles)
				_audioFiles.Clear();

			PerformBackupOrRestore(reportProgressAction, finishedAction);
		}

		/// ------------------------------------------------------------------------------------
		protected override void PerformZipWork(ZipFile zip)
		{
			zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
			zip.SaveProgress += HandleZipSaveProgress;
			zip.AddFile(_infoFile, string.Empty);
			zip.AddFiles(_prjFiles, string.Empty);

			if (_dataSourceFiles.Count > 0)
				zip.AddFiles(_dataSourceFiles, "Data");

			if (_audioFiles.Count > 0)
				zip.AddFiles(_audioFiles, "Audio");

			zip.Save(Path.Combine(TargetFolder, BackupFile));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is called by the Save method on the ZipFile class as the zip file is being
		/// saved to the disk.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleZipSaveProgress(object s, SaveProgressEventArgs e)
		{
			if (Cancel)
			{
				e.Cancel = true;
				return;
			}

			if (e.EventType != ZipProgressEventType.Saving_BeforeWriteEntry)
				return;

			if (e.CurrentEntry.FileName != Path.GetFileName(_infoFile))
				WriteLogMessageForFileName(e.CurrentEntry.FileName.Replace("Data/", string.Empty).Replace("Audio/", string.Empty));
	
			_worker.ReportProgress(e.EntriesSaved + 1);
		}

		/// ------------------------------------------------------------------------------------
		private void WriteLogMessageForFileName(string filename)
		{
			if (Path.GetFileName(_prjFiles[0]) == filename)
			{
				var text = App.GetString("DialogBoxes.BackupDlg.BackingUpProjectFilesMsg", "Backing Up Project Files...");
				LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, text);
			}

			if (_dataSourceFiles.Count > 0 &&
				Path.GetFileName(_dataSourceFiles[0]) == filename)
			{
				var text = App.GetString("DialogBoxes.BackupDlg.BackingUpDataSourceFilesMsg", "Backing Up Data Source Files...");
				LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, Environment.NewLine + text);
			}

			if (_audioFiles.Count > 0 &&
				Path.GetFileName(_audioFiles[0]) == filename)
			{
				var text = App.GetString("DialogBoxes.BackupDlg.BackingUpAudioFilesMsg", "Backing Up Audio Files...");
				LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, Environment.NewLine + text);
			}

			LogBox.WriteMessage("\t" + filename);
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetCompleteMessage()
		{
			return App.GetString("DialogBoxes.BackupDlg.BackingUpCompleteMsg", "Backup Complete!");
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetCancelledMessage()
		{
			return App.GetString("DialogBoxes.BackupDlg.BackingCancelledMsg", "Backup Cancelled");
		}

		/// ------------------------------------------------------------------------------------
		private string CreateBackupInfoFile()
		{
			var root = new XElement("paBackupInfo");

			root.Add(new XElement("project", Project.Name));
			root.Add(new XElement("projectFolder", Project.Folder));
			root.Add(new XElement("paProjectFolder", App.ProjectFolder));
			root.Add(new XElement("paExeFolder", Path.GetDirectoryName(Application.StartupPath)));
			root.Add(new XElement("paVersion", Assembly.GetEntryAssembly().GetName().Version.ToString()));
			root.Add(new XElement("apparentBuildDate", File.GetLastWriteTimeUtc(Application.ExecutablePath).Date.ToString("yyyy-MMM-dd")));
			root.Add(new XElement("culture", CultureInfo.CurrentCulture.ToString()));
            root.Add(new XElement("machineName", Environment.MachineName));
            root.Add(new XElement("osVersion", Environment.OSVersion.VersionString));
            root.Add(new XElement("userDomainName", Environment.UserDomainName));
            root.Add(new XElement("userName", Environment.UserName));

			var infoFile = Path.Combine(Path.GetTempPath(), kBackupInfoFileName);
			root.Save(infoFile);
			return infoFile;
		}
	}
}
