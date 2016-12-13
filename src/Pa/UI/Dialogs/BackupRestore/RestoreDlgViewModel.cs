// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Ionic.Zip;
using L10NSharp;
using SIL.Pa.Model;
using SilTools;
using SIL.Windows.Forms.Miscellaneous;

namespace SIL.Pa.UI.Dialogs
{
	public class RestoreDlgViewModel : BackupRestoreDlgViewModelBase
	{
		public string SuggestedDestFolder { get; protected set; }
		public string NameOfProjectToRestore { get; private set; }
		public string CurrentProjectFileName { get; private set; }
		public List<KeyValuePair<string, string>> AvailableBackups { get; private set; }

		private bool _targetFolderPreexisted;

		/// ------------------------------------------------------------------------------------
		public static RestoreDlgViewModel Create(string backupFile)
		{
			if (backupFile == null)
				return new RestoreDlgViewModel(null as PaProject);

			if (!File.Exists(backupFile))
			{
				var msg = LocalizationManager.GetString("DialogBoxes.RestoreDlg.SpecifiedBackupFileDoesNotExistMsg",
					"The backup file '{0}' does not exist.");
				App.NotifyUserOfProblem(msg, backupFile);
				return null;
			}

			return !GetIsValidBackupFile(backupFile, true) ?
				null : new RestoreDlgViewModel(backupFile);
		}

		/// ------------------------------------------------------------------------------------
		public RestoreDlgViewModel(string backupFile) : base(null)
		{
			var prjName = GetProjectNameFromBackupFile(backupFile);
			AvailableBackups = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(prjName, backupFile) };
			SetCurrentBackupFile(0);
		}

		/// ------------------------------------------------------------------------------------
		public RestoreDlgViewModel(PaProject project) : base(project)
		{
			AvailableBackups = (from backupFile in GetBackupFiles()
								let prjName = GetProjectNameFromBackupFile(backupFile)
								orderby prjName
								select new KeyValuePair<string, string>(prjName, backupFile)).ToList();
				
			SetCurrentBackupFile(0);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetBackupFiles()
		{
			var folder = Properties.Settings.Default.LastOtherBackupFolder;

			if (folder != null && Directory.Exists(folder))
			{
				foreach (var backupFile in Directory.GetFiles(folder, "*.pabackup"))
					yield return backupFile;
			}

			folder = Path.Combine(App.ProjectFolder, "Backups");
			if (folder != Properties.Settings.Default.LastOtherBackupFolder && Directory.Exists(folder))
			{
				foreach (var backupFile in Directory.GetFiles(folder, "*.pabackup"))
					yield return backupFile;
			}

			// Check the desktop
			folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			if (folder != Properties.Settings.Default.LastOtherBackupFolder)
			{
				foreach (var backupFile in Directory.GetFiles(folder, "*.pabackup"))
					yield return backupFile;
			}

			// Check the downloads folder
			folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Downloads");
			if (folder != Properties.Settings.Default.LastOtherBackupFolder && Directory.Exists(folder))
			{
				foreach (var backupFile in Directory.GetFiles(folder, "*.pabackup"))
					yield return backupFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		public int GetNumberOfFilesToRestore()
		{
			return _prjFiles.Count + _dataSourceFiles.Count + _audioFiles.Count;
		}

		/// ------------------------------------------------------------------------------------
		public void AddBackupFileToListAndMakeCurrent(string backupFile)
		{
			AvailableBackups.Insert(0,
				new KeyValuePair<string, string>(GetProjectNameFromBackupFile(backupFile), backupFile));
			
			SetCurrentBackupFile(0);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCurrentBackupFile(int index)
		{
			if (index < 0 || index >= AvailableBackups.Count)
				return;

			BackupFile = AvailableBackups[index].Value;
			NameOfProjectToRestore = AvailableBackups[index].Key;
			SuggestedDestFolder = GetSuggestedTargetFolder(index);
			ReadFilesFromBackupFile(BackupFile);
		}

		/// ------------------------------------------------------------------------------------
		private string GetSuggestedTargetFolder(int index)
		{
			var prjName = PaProject.GetCleanNameForFileName(AvailableBackups[index].Key);
			var backupFile = AvailableBackups[index].Value;

			var folder = Path.Combine(App.ProjectFolder, prjName);
			if (!Directory.Exists(folder) || Directory.GetFiles(folder, "*.pap").Length == 0)
				return folder;

			folder = Path.Combine(App.ProjectFolder, Path.GetFileNameWithoutExtension(backupFile));
			if (!Directory.Exists(folder) || Directory.GetFiles(folder, "*.pap").Length == 0)
				return folder;

			int i = 2;
			do { folder = Path.Combine(App.ProjectFolder, string.Format("{0} ({1})", prjName, i++)); }
			while (Directory.Exists(folder) && Directory.GetFiles(folder, "*.pap").Length > 0);
			return folder;
		}

		/// ------------------------------------------------------------------------------------
		public void ReadFilesFromBackupFile(string backupFile)
		{
			WaitCursor.Show();
			Utils.SetWindowRedraw(LogBox, false);
			
			LogBox.Clear();
			_prjFiles.Clear();
			_dataSourceFiles.Clear();
			_audioFiles.Clear();

			using (var zip = new ZipFile(backupFile))
			{
				foreach (var filename in zip.EntryFileNames)
				{
                    if (filename.StartsWith("Data/", StringComparison.Ordinal))
						_dataSourceFiles.Add(filename.Replace("Data/", string.Empty));
                    else if (filename.StartsWith("Audio/", StringComparison.Ordinal))
						_audioFiles.Add(filename.Replace("Audio/", string.Empty));
					else if (filename != kBackupInfoFileName)
					{
						_prjFiles.Add(filename);
                        if (filename.ToLowerInvariant().EndsWith(".pap", StringComparison.Ordinal))
							CurrentProjectFileName = filename;
					}
				}
			}

			WriteLogMessagesForFileType(LocalizationManager.GetString("DialogBoxes.RestoreDlg.ProjectFilesFoundInBackupMsg",
				"Project Files Found In Backup File...") , _prjFiles);

			WriteLogMessagesForFileType(Environment.NewLine + 
				LocalizationManager.GetString("DialogBoxes.RestoreDlg.DataSourceFilesFoundInBackupMsg",
				"Data Source Files Found In Backup File..."), _dataSourceFiles);

			WriteLogMessagesForFileType(Environment.NewLine + 
				LocalizationManager.GetString("DialogBoxes.RestoreDlg.AudioFilesFoundInBackupMsg",
				"Aduio Files Found In Backup File..."), _audioFiles);

			LogBox.ScrollToTop();

			Utils.SetWindowRedraw(LogBox, true);
			WaitCursor.Hide();
		}

		/// ------------------------------------------------------------------------------------
		private void WriteLogMessagesForFileType(string initialMsg, IEnumerable<string> fileNames)
		{
			var list = fileNames.ToArray();

			if (list.Length == 0)
				return;

			LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, initialMsg);

			foreach (var filename in list)
				LogBox.WriteMessage("\t" + filename);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsValidBackupFile(string backupFile, bool showMessageIfInvalid)
		{
			if (GetProjectNameFromBackupFile(backupFile) != null)
				return true;

			if (showMessageIfInvalid)
			{
				var msg = LocalizationManager.GetString("DialogBoxes.RestoreDlg.SelectOtherBackupFileDlg.BackupFileNotValid",
							"The selected file '{0}' is not a valid Phonology Assistant backup file.");
				App.NotifyUserOfProblem(msg, backupFile);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetProjectNameFromBackupFile(string backupFile)
		{
			if (!ZipFile.IsZipFile(backupFile, true))
				return null;

			using (var zip = new ZipFile(backupFile))
			{
				if (!zip.Any(ze => ze.FileName == kBackupInfoFileName))
					return null;

				zip.ExtractSelectedEntries(kBackupInfoFileName, string.Empty,
					Path.GetTempPath(), ExtractExistingFileAction.OverwriteSilently);
				
				var infoFile = Path.Combine(Path.GetTempPath(), kBackupInfoFileName);
				var root = XElement.Load(infoFile);
				try { File.Delete(infoFile); }
				catch { }
				return (string)root.Element("project");
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Restore(Action<int> reportProgressAction, Action finishedAction)
		{
			PerformBackupOrRestore(reportProgressAction, finishedAction);
		}
	
		/// ------------------------------------------------------------------------------------
		protected override ZipFile GetZipFileForProcessing()
		{
			return new ZipFile(BackupFile);
		}

		/// ------------------------------------------------------------------------------------
		protected override void PerformZipWork(ZipFile zip)
		{
			var restoreFolder = OtherDestFolder ?? SuggestedDestFolder;
			_targetFolderPreexisted = Directory.Exists(restoreFolder);

			zip.ExtractProgress += HandleZipExtractProgress;

			var text = LocalizationManager.GetString("DialogBoxes.RestoreDlg.RestoringProjectFilesMsg", "Restoring Project Files...");
			LogBox.Invoke((Action)(() => LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, text)));
			zip.ExtractSelectedEntries("*", string.Empty, restoreFolder, ExtractExistingFileAction.OverwriteSilently);

			if (_dataSourceFiles.Count > 0)
			{
				text = LocalizationManager.GetString("DialogBoxes.RestoreDlg.RestoringDataSourceFilesMsg", "Restoring Data Source Files...");
				LogBox.Invoke((Action)(() => LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, Environment.NewLine + text)));
				zip.ExtractSelectedEntries("*.*", "Data", restoreFolder, ExtractExistingFileAction.OverwriteSilently);
			}

			if (_audioFiles.Count > 0)
			{
				text = LocalizationManager.GetString("DialogBoxes.RestoreDlg.RestoringAudioFilesMsg", "Restoring Audio Files...");
				LogBox.Invoke((Action)(() => LogBox.WriteMessageWithFontStyle(FontStyle.Bold | FontStyle.Underline, Environment.NewLine + text)));
				zip.ExtractSelectedEntries("*.*", "Audio", restoreFolder, ExtractExistingFileAction.OverwriteSilently);
			}

			if (File.Exists(Path.Combine(restoreFolder, kBackupInfoFileName)))
				File.Delete(Path.Combine(restoreFolder, kBackupInfoFileName));

			if (Cancel || BackupRestoreException != null)
				CleanUpAfterCancel(restoreFolder);
			else
			{
				var updater = new RestoredPapDataSourceUpdater { PapFile = CurrentProjectFileName };
				updater.Modify(restoreFolder, Path.Combine(restoreFolder, "Data"), false);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void CleanUpAfterCancel(string restoreFolder)
		{
			if (!_targetFolderPreexisted)
			{
				try
				{
					Directory.Delete(restoreFolder, true);
					return;
				}
				catch { }
			}

			DeletListOfFiles(restoreFolder, _prjFiles);
			DeletListOfFiles(restoreFolder, _dataSourceFiles);
			DeletListOfFiles(restoreFolder, _audioFiles);
		}

		/// ------------------------------------------------------------------------------------
		private void DeletListOfFiles(string restoreFolder, IEnumerable<string> files)
		{
			foreach (var path in files.Select(f => Path.Combine(restoreFolder, f)).Where(p => File.Exists(p)))
			{
				try { File.Delete(path); }
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is called by the Save method on the ZipFile class as the zip file is being
		/// saved to the disk.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleZipExtractProgress(object s, ExtractProgressEventArgs e)
		{
			if (Cancel)
			{
				e.Cancel = true;
				return;
			}

			if (e.EventType != ZipProgressEventType.Extracting_BeforeExtractEntry)
				return;

			if (e.CurrentEntry.FileName != Path.GetFileName(_infoFile))
			{
				var filename = e.CurrentEntry.FileName.Replace("Data/", string.Empty).Replace("Audio/", string.Empty);
				LogBox.WriteMessage("\t" + filename);
			}
	
			_worker.ReportProgress(0);
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetCompleteMessage()
		{
			return LocalizationManager.GetString("DialogBoxes.RestoreDlg.RestoringCompleteMsg", "Restore Complete!");
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetCancelledMessage()
		{
			return LocalizationManager.GetString("DialogBoxes.RestoreDlg.RestoringCancelledMsg", "Restore Cancelled");
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class RestoredPapDataSourceUpdater
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
			Modify(path, null, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines the specified path with the PapFile path to point to a pap file. That
		/// pap file is deserialized and each data source path in the pap file is modified
		/// to use that combined path. Then the pap file is rewritten and added to the
		/// program's list of recent projects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Modify(string path, string dataSourcePath, bool addToRecentlyUsedProjectsList)
		{
			var papFilePath = Path.Combine(path, PapFile);
			if (!File.Exists(papFilePath))
				return;

			var prj = XmlSerializationHelper.DeserializeFromFile<PaProject>(papFilePath);
			if (prj == null)
				return;

			foreach (var dataSource in prj.DataSources.Where(ds => ds.SourceFile != null))
			{
				var newPath = dataSourcePath ?? Path.GetDirectoryName(papFilePath);
				var filename = Path.GetFileName(dataSource.SourceFile);
				dataSource.SourceFile = Path.Combine(newPath, filename);
			}

			XmlSerializationHelper.SerializeToFile(papFilePath, prj);
			
			if (addToRecentlyUsedProjectsList)
				App.AddProjectToRecentlyUsedProjectsList(papFilePath, true);
		}
	}
}
