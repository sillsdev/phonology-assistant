using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Palaso.Progress.LogBox;
using SIL.Pa.Properties;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public class BackupDlgViewModel : IDisposable
	{
		public const string kBackupInfoFileName = ".paBackupInfo.xml";

		public PaProject Project { get; private set; }
		public string BackupFile { get; private set; }
		public string BackupFolder { get; set; }
		public LogBox LogBox { get; private set; }
		public bool CancelBackup { get; set; }
		public Exception BackupException { get; private set; }

		private readonly string _infoFile;
		private readonly string[] _prjFilesToBackup;
		private string[] _dataSourceFilesToBackup;
		private string[] _audioFilesToBackup;
		private BackgroundWorker _worker;

		/// ------------------------------------------------------------------------------------
		public BackupDlgViewModel(PaProject project)
		{
			Project = project;
			LogBox = new LogBox();
			LogBox.TabStop = false;
			LogBox.ShowMenu = false;

			var fmt = App.GetString("DialogBoxes.BackupDlg.BackupFileNameFormat", "{0}_({1}).pabackup");
			BackupFile = string.Format(fmt, Project.Name, DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"));
			BackupFolder = GetBackupFolderAndCreateIfNonExistant();

			_infoFile = CreateBackupInfoFile();
			_prjFilesToBackup = GetProjectFilesToBackup().ToArray();
			_dataSourceFilesToBackup = GetDataSourceFilesToBackup().ToArray();
			_audioFilesToBackup = GetAudioFilesToBackup().ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_infoFile != null && File.Exists(_infoFile))
				File.Delete(_infoFile);
		}

		/// ------------------------------------------------------------------------------------
		private string GetBackupFolderAndCreateIfNonExistant()
		{
			var folder = Settings.Default.LastBackupFolder;
			
			if (folder == null || !Directory.Exists(folder))
			{
				folder = Path.Combine(App.ProjectFolder, "Backups");
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
			int count = _prjFilesToBackup.Length;

			if (includeDataSourceFiles)
				count += _dataSourceFilesToBackup.Length;
			
			if (includeAudioFiles)
				count += _audioFilesToBackup.Length;

			// Add one for the info. file.
			return count + 1;
		}

		/// ------------------------------------------------------------------------------------
		public void Backup(bool includeDataSourceFiles, bool includeSoundFiles,
			Action<int> reportProgressAction, Action backupFinishedAction)
		{
			if (!includeDataSourceFiles)
				_dataSourceFilesToBackup = null;

			if (!includeSoundFiles)
				_audioFilesToBackup = null;

			try
			{
				using (_worker = new BackgroundWorker())
				{
					_worker.DoWork += CreateBackupFileInWorkerThread;
					_worker.WorkerReportsProgress = true;
					_worker.ProgressChanged += (s, e) => reportProgressAction(e.ProgressPercentage);
					_worker.RunWorkerCompleted += (s, e) => backupFinishedAction();
					_worker.RunWorkerAsync();
					while (_worker.IsBusy) Application.DoEvents();
				}
			}
			catch (Exception error)
			{
				BackupException = error;
			}
			finally
			{
				if (BackupException != null)
				{
					LogBox.Clear();
					LogBox.WriteException(BackupException);
				}

				_worker = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void CreateBackupFileInWorkerThread(object sender, DoWorkEventArgs e)
		{
			try
			{
				using (var zip = new ZipFile())
				{
					zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
					zip.SaveProgress += HandleZipSaveProgress;
					zip.AddFile(_infoFile, string.Empty);
					zip.AddFiles(_prjFilesToBackup, string.Empty);
					
					if (_dataSourceFilesToBackup != null)
						zip.AddFiles(_dataSourceFilesToBackup, "Data");

					if (_audioFilesToBackup != null)
						zip.AddFiles(_audioFilesToBackup, "Data");

					zip.Save(Path.Combine(BackupFolder, BackupFile));

					if (CancelBackup)
					{
						var text = App.GetString("DialogBoxes.BackupDlg.BackingCancelledMsg", "Backup Cancelled");
						LogBox.WriteMessageWithColorAndFontStyle(Color.Red, FontStyle.Bold, Environment.NewLine + text);
					}
					else
					{
						var text = App.GetString("DialogBoxes.BackupDlg.BackingUpCompleteMsg", "Backup Complete!");
						LogBox.WriteMessageWithColorAndFontStyle(Color.DarkGreen, FontStyle.Bold, Environment.NewLine + text);
					}
				}
			}
			catch (Exception error)
			{
				BackupException = error;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is called by the Save method on the ZipFile class as the zip file is being
		/// saved to the disk.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleZipSaveProgress(object s, SaveProgressEventArgs e)
		{
			if (CancelBackup)
			{
				e.Cancel = true;
				return;
			}

			if (e.EventType != ZipProgressEventType.Saving_BeforeWriteEntry)
				return;

			if (e.CurrentEntry.FileName != Path.GetFileName(_infoFile))
				WriteLogMessageForFileName(e.CurrentEntry.FileName.Replace("Data/", string.Empty));
	
			_worker.ReportProgress(e.EntriesSaved + 1);
		}

		/// ------------------------------------------------------------------------------------
		private void WriteLogMessageForFileName(string filename)
		{
			if (Path.GetFileName(_prjFilesToBackup[0]) == filename)
			{
				var text = App.GetString("DialogBoxes.BackupDlg.BackingUpProjectFilesMsg", "Backing Up Project Files...");
				LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, text);
			}

			if (_dataSourceFilesToBackup != null && _dataSourceFilesToBackup.Length > 0 &&
				Path.GetFileName(_dataSourceFilesToBackup[0]) == filename)
			{
				var text = App.GetString("DialogBoxes.BackupDlg.BackingUpDataSourceFilesMsg", "Backing Up Data Source Files...");
				LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, Environment.NewLine + text);
			}

			if (_audioFilesToBackup != null && _audioFilesToBackup.Length > 0 &&
				Path.GetFileName(_audioFilesToBackup[0]) == filename)
			{
				var text = App.GetString("DialogBoxes.BackupDlg.BackingUpAudioFilesMsg", "Backing Up Audio Files...");
				LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, Environment.NewLine + text);
			}

			LogBox.WriteMessage("\t" + filename);
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
