using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;

namespace SIL.Pa.UI.Dialogs
{
	public class OpenProjectDlgViewModel
	{
		private List<PaProjectLite> _availableProjects;
		public PaProjectLite SelectedProject { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public OpenProjectDlgViewModel()
		{
			RefreshAvailableProjectsList();
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshAvailableProjectsList()
		{
			_availableProjects = (from prjFile in GetProjectFiles()
								  let prjInfo = PaProjectLite.Create(prjFile)
								  where prjInfo != null
								  orderby prjInfo.Name
								  select prjInfo).ToList();

			SetCurrentBackupFile(0);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetProjectFiles()
		{
			var folder = App.ProjectFolder;

			if (folder != null && Directory.Exists(folder))
			{
				foreach (var prjFile in Directory.GetFiles(folder, "*.pap", SearchOption.AllDirectories)
					.Where(ShouldAddProjectFileToAvailableList))
				{
					yield return prjFile;
				}
			}

			if (Settings.Default.NonDefaultFoldersToScanForProjectFiles == null)
				Settings.Default.NonDefaultFoldersToScanForProjectFiles = new StringCollection();

			foreach (var prjFile in Settings.Default.NonDefaultFoldersToScanForProjectFiles.Cast<string>()
				.SelectMany(fldr => Directory.GetFiles(fldr, "*.pap", SearchOption.AllDirectories))
				.Where(ShouldAddProjectFileToAvailableList))
			{
				yield return prjFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool ShouldAddProjectFileToAvailableList(string prjFilePath)
		{
			// Make sure the project file is not an old one contained in one of the backup
			// folders created when migrating a project from an older version of the program.
			var prjName = Path.GetFileNameWithoutExtension(prjFilePath);
			var prjFolder = Path.GetDirectoryName(prjFilePath);
			return !Path.GetFileName(prjFolder).StartsWith("Backup-");
		}

		/// ------------------------------------------------------------------------------------
		public int GetProjectFileCount()
		{
			return _availableProjects.Count;
		}

		/// ------------------------------------------------------------------------------------
		public string GetProjectNameForIndex(int index)
		{
			if (index < 0 || index >= _availableProjects.Count)
				return null;

			return (GetDoesProjectHaveName(index) ? _availableProjects[index].Name :
				App.GetString("DialogBoxes.OpenProjectDlg.DisplayedTextWhenProjectNameMissing",
					"(no project name specified)"));
			}

		/// ------------------------------------------------------------------------------------
		public string GetProjectFilePathForIndex(int index)
		{
			return (index >= 0 && index < _availableProjects.Count ?
				_availableProjects[index].FilePath : null);
		}

		/// ------------------------------------------------------------------------------------
		public string GetProjectFileNameForIndex(int index)
		{
			return (index >= 0 && index < _availableProjects.Count ?
				Path.GetFileName(_availableProjects[index].FilePath) : null);
		}

		/// ------------------------------------------------------------------------------------
		public string GetProjectDataSourceTypesForIndex(int index)
		{
			return (index >= 0 && index < _availableProjects.Count ?
				_availableProjects[index].DataSourceTypes : null);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesProjectHaveName(int index)
		{
			return (_availableProjects[index].Name != null);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCurrentBackupFile(int index)
		{
			if (index < 0 || index >= _availableProjects.Count)
				return;

			SelectedProject = _availableProjects[index];
		}

		/// ------------------------------------------------------------------------------------
		public bool LetUserAddAdditionalFolderToScan(Form parent)
		{
			try
			{
				var description = App.GetString("DialogBoxes.OpenProjectDlg.ChangeFolderBrowserDlg.Description",
					"Specify a folder to scan for Phonology Assistant project files.");
				
				using (var dlg = new FolderBrowserDialog())
				{
					dlg.ShowNewFolderButton = false;
					dlg.Description = description;

					if (dlg.ShowDialog(parent) != DialogResult.OK)
						return false;

					if (!dlg.SelectedPath.Equals(App.ProjectFolder, StringComparison.Ordinal) &&
						!Settings.Default.NonDefaultFoldersToScanForProjectFiles.Contains(dlg.SelectedPath))
					{
						Settings.Default.NonDefaultFoldersToScanForProjectFiles.Add(dlg.SelectedPath);
						RefreshAvailableProjectsList();
						return true;
					}
				}
			}
			catch (Exception e)
			{
				App.NotifyUserOfProblem(e, "There was an error while selecting a folder to scan.");
			}

			return false;
		}
	}
}
