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
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Localization;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.Model.Migration
{
	public class MigrationBase
	{
		protected string _projectFilePath;
		protected string _projectPathPrefix;

		/// ------------------------------------------------------------------------------------
		protected MigrationBase(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
		{
			_projectFilePath = prjfilepath;
			
			var xml = XElement.Load(prjfilepath);
			ProjectName = (xml.Element("ProjectName") ?? xml.Element("name")).Value;
			
			_projectPathPrefix = GetPrjPathPrefixAction(prjfilepath, ProjectName);
		}

		/// ------------------------------------------------------------------------------------
		protected Exception DoMigration()
		{
			MigrateXYChartFile();

			try { InternalMigration(); }
			catch (Exception e) { return e; }
			return null;
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateXYChartFile()
		{
			var filename = DistributionChart.GetFileForProject(_projectPathPrefix);
			var oldFileName = _projectPathPrefix + "XYCharts.xml";

			if (!File.Exists(oldFileName))
				return;

			try 
			{
				File.Copy(oldFileName, filename);
				File.Delete(oldFileName);
			}
			catch { return; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InternalMigration()
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		public string ProjectName { get; private set; }

		/// ------------------------------------------------------------------------------------
		protected Exception TransformFile(string filename, string transformNamespace)
		{
			var assembly = Assembly.GetExecutingAssembly();

			using (var stream = assembly.GetManifestResourceStream(transformNamespace))
			{
				string updatedFile;
				var error = XmlHelper.TransformFile(filename, stream, out updatedFile);
				if (error != null)
					return error;

				try
				{
					File.Delete(filename);
					File.Move(updatedFile, filename);
				}
				catch (Exception e)
				{
					return e;
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateProjectFileToLatestVersion()
		{
			var root = XElement.Load(_projectFilePath);

			if (root.Attribute("version") == null)
				root.Add(new XAttribute("version", PaProject.kCurrVersion));
			else
				root.Attribute("version").SetValue(PaProject.kCurrVersion);
			
			root.Save(_projectFilePath);
		}

		/// ------------------------------------------------------------------------------------
		public static string BackupProject(string projectFilePath, string projectName, string oldversion)
		{
			string backupFolder = null;

			try
			{
				var path = Path.GetDirectoryName(projectFilePath);
				string ver = oldversion;
				char letter = 'a';

				// Find an unused backup folder name.
				do
				{
					backupFolder = Path.Combine(path, string.Format("Backup-{0}-{1}", projectName, ver));
					ver = oldversion + letter;
					letter += (char)1;
				}
				while (Directory.Exists(backupFolder));

				Directory.CreateDirectory(backupFolder);

				// Copy the project files to the backup folder.
				foreach (var filepath in Directory.GetFiles(path, projectName + ".*"))
				{
					var filename = Path.GetFileName(filepath);
					File.Copy(filepath, Path.Combine(backupFolder, filename));
				}

				// This will make sure the project file (i.e. .pap) gets backed-up
				// in case its file name is not the same as the project name.
				var prjfilename = Path.GetFileName(projectFilePath);
				if (!File.Exists(Path.Combine(backupFolder, prjfilename)))
					File.Copy(projectFilePath, Path.Combine(backupFolder, prjfilename));
			}
			catch (Exception e)
			{
				backupFolder = null;
				var errMsg = LocalizationManager.GetString("ProjectMessages.Migrating.BackupOfOldProjectFilesFailureMsg",
					"An error occurred attempting to backup your project before updating it for the latest version of Phonology Assistant.\n\n" +
					"Until the problem is resolved, this project cannot be opened using this version of Phonology Assistant.");

				App.NotifyUserOfProblem(e, errMsg);
			}

			return backupFolder;
		}
	}
}
