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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using L10NSharp;
using SIL.Windows.Forms.Miscellaneous;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public class OpenProjectDlgViewModel
	{
		private List<PaProjectLite> _availableProjects;

		[XmlIgnore]
		public PaProjectLite SelectedProject { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public OpenProjectDlgViewModel()
		{
			RefreshAvailableProjectsList();
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshAvailableProjectsList()
		{
			WaitCursor.Show();

			_availableProjects = (from prjFile in GetProjectFiles()
								  let prjInfo = PaProjectLite.Create(prjFile)
								  where prjInfo != null
								  orderby prjInfo.Name
								  select prjInfo).ToList();

			SetCurrentBackupFile(0);

			WaitCursor.Hide();
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

			if (Properties.Settings.Default.ProjectsNotInDefaultFolderToShowInOpenDlg == null)
				Properties.Settings.Default.ProjectsNotInDefaultFolderToShowInOpenDlg = new StringCollection();

			foreach (var prjFile in Properties.Settings.Default.ProjectsNotInDefaultFolderToShowInOpenDlg.Cast<string>()
				.Where(f => File.Exists(f) && ShouldAddProjectFileToAvailableList(f)))
			{
				yield return prjFile;
			}

			var defaultFwProjectsFolder = Utils.FwProjectsPath;
			if (Directory.Exists(defaultFwProjectsFolder))
			{
				foreach (var prjFile in new DirectoryInfo(defaultFwProjectsFolder).GetFiles("*.fwdata", SearchOption.AllDirectories))
				{
					yield return prjFile.FullName;
				}
			}
        }

		/// ------------------------------------------------------------------------------------
		public bool ShouldAddProjectFileToAvailableList(string prjFilePath)
		{
			// Make sure the project file is not an old one contained in one of the backup
			// folders created when migrating a project from an older version of the program.
			var prjFolder = Path.GetDirectoryName(prjFilePath);
            return !Path.GetFileName(prjFolder).StartsWith("Backup-", StringComparison.Ordinal);
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
				LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.DisplayedTextWhenProjectNameMissing",
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
		public bool ResetFoldersToScan()
		{
			int savePrjCount = GetProjectFileCount();

			if (Properties.Settings.Default.ProjectsNotInDefaultFolderToShowInOpenDlg.Count == 0)
				return false;

			Properties.Settings.Default.ProjectsNotInDefaultFolderToShowInOpenDlg.Clear();
			RefreshAvailableProjectsList();
			return (savePrjCount != GetProjectFileCount());
		}

		/// ------------------------------------------------------------------------------------
		public bool LetUserSelectSpecificProjectFile(Form parent)
		{
			try
			{
				int filterindex = 0;

				string filter = string.Format(App.kstidFileTypePAProject,
					Application.ProductName) + "|" + App.kstidFileTypeAllFiles;

				var fmt = LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.SelectSpecificProjectOpenFileDialogText", "Open Project File");

				var initialDir = (Properties.Settings.Default.LastFolderForOpenProjectDlg ?? string.Empty);
				if (!Directory.Exists(initialDir))
					initialDir = App.ProjectFolder;

				var filenames = App.OpenFileDialog("pap", filter, ref filterindex,
					string.Format(fmt, Application.ProductName), false, initialDir);

				if (filenames.Length > 0 && File.Exists(filenames[0]))
				{
					var path = Path.GetDirectoryName(filenames[0]);
					if (!path.Equals(App.ProjectFolder, StringComparison.Ordinal) &&
						!Properties.Settings.Default.ProjectsNotInDefaultFolderToShowInOpenDlg.Contains(filenames[0]))
					{
						Properties.Settings.Default.ProjectsNotInDefaultFolderToShowInOpenDlg.Add(filenames[0]);
					}

					var prjInfo = PaProjectLite.Create(filenames[0]);

					if (prjInfo != null)
					{
						Properties.Settings.Default.LastFolderForOpenProjectDlg = path;
						SelectedProject = prjInfo;
						return true;
					}

					App.NotifyUserOfProblem(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.InvalidProjectFileSelectedMsg",
						"The file '{0}' is not a valid project file."), filenames[0]);
				}
			}
			catch (Exception e)
			{
				App.NotifyUserOfProblem(e, "There was an error while selecting a folder to scan.");
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public int FindNextProjectStartingWithLetter(int startIndex, char letter)
		{
			int index = (startIndex < _availableProjects.Count - 1 ? startIndex + 1 : 0);

			while (index != startIndex)
			{
				var name = _availableProjects[index].Name;
				if (name != null && name.StartsWith(letter.ToString(), StringComparison.CurrentCultureIgnoreCase))
					return index;

				index++;
				if (index == _availableProjects.Count)
					index = 0;
			}

			return -1;
		}
	}
}
