using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ionic.Zip;
using Palaso.Progress;
using Palaso.Progress.LogBox;
using Palaso.Reporting;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Dialogs
{
	public class BackupRestoreDlgViewModelBase : IDisposable
	{
		public const string kBackupInfoFileName = ".paBackupInfo.xml";

		public PaProject Project { get; protected set; }
		public string BackupFile { get; protected set; }
		public string OtherDestFolder { get; set; }
		public LogBox LogBox { get; protected set; }
		public bool Cancel { get; set; }
		public Exception BackupRestoreException { get; protected set; }

		protected string _infoFile;
		protected List<string> _prjFiles = new List<string>();
		protected List<string> _dataSourceFiles = new List<string>();
		protected List<string> _audioFiles = new List<string>();
		protected BackgroundWorker _worker;

		/// ------------------------------------------------------------------------------------
		public BackupRestoreDlgViewModelBase(PaProject project)
		{
			Project = project;
			LogBox = new LogBox();
			LogBox.TabStop = false;
			LogBox.ShowMenu = false;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_infoFile != null && File.Exists(_infoFile))
				File.Delete(_infoFile);
		}

		/// ------------------------------------------------------------------------------------
		public string DefaultBackupFolder
		{
			get { return Path.Combine(App.ProjectFolder, "Backups"); }
		}

		/// ------------------------------------------------------------------------------------
		public bool SpecifyTargetFolder(Form dialogBox, string description,
			string folderContainsProjectMsg)
		{
			using (var dlg = new FolderBrowserDialog())
			{
				dlg.ShowNewFolderButton = true;
				dlg.Description = description;
				dlg.SelectedPath = OtherDestFolder;

				while (dlg.ShowDialog(dialogBox) == DialogResult.OK)
				{
					if (folderContainsProjectMsg != null && Directory.GetFiles(dlg.SelectedPath, "*.pap").Length > 0)
						ErrorReport.NotifyUserOfProblem(folderContainsProjectMsg);
					else
					{
						OtherDestFolder = dlg.SelectedPath;
						return true;
					}
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void PerformBackupOrRestore(Action<int> reportProgressAction, Action finishedAction)
		{
			WaitCursor.Show();
			LogBox.Clear();

			try
			{
				using (_worker = new BackgroundWorker())
				{
					_worker.DoWork += PerformBackgroundWork;
					_worker.WorkerReportsProgress = true;
					_worker.ProgressChanged += (s, e) => reportProgressAction(e.ProgressPercentage);
					_worker.RunWorkerCompleted += (s, e) => finishedAction();
					_worker.RunWorkerAsync();
					while (_worker.IsBusy) Application.DoEvents();
				}
			}
			catch (Exception error)
			{
				BackupRestoreException = error;
			}
			finally
			{
				if (BackupRestoreException != null)
				{
					LogBox.Clear();
					LogBox.WriteException(BackupRestoreException);
				}

				_worker = null;
				WaitCursor.Hide();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual ZipFile GetZipFileForProcessing()
		{
			return new ZipFile();
		}

		/// ------------------------------------------------------------------------------------
		private void PerformBackgroundWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				using (var zip = GetZipFileForProcessing())
				{
					PerformZipWork(zip);

					if (Cancel)
					{
						LogBox.WriteMessageWithColorAndFontStyle(Color.Red, FontStyle.Bold,
							Environment.NewLine + GetCancelledMessage());
					}
					else
					{
						LogBox.WriteMessageWithColorAndFontStyle(Color.DarkGreen, FontStyle.Bold,
							Environment.NewLine + GetCompleteMessage());
					}
				}
			}
			catch (Exception error)
			{
				BackupRestoreException = error;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void PerformZipWork(ZipFile zip)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetCompleteMessage()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetCancelledMessage()
		{
			throw new NotImplementedException();
		}
	}
}
