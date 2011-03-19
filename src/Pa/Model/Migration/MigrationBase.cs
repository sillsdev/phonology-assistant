using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using SilTools;

namespace SIL.Pa.Model.Migration
{
	public class MigrationBase
	{
		/// ------------------------------------------------------------------------------------
		public string ProjectName { get; private set; }

		/// ------------------------------------------------------------------------------------
		public string BackupFolder { get; private set; }

		/// ------------------------------------------------------------------------------------
		protected void CalculateProjectName(string prjfilename)
		{
			var xml = XElement.Load(prjfilename);
			ProjectName = (xml.Element("ProjectName") ?? xml.Element("name")).Value;
		}

		/// ------------------------------------------------------------------------------------
		protected Exception BackupProject(string prjfilepath, string oldversion)
		{
			try
			{
				CalculateProjectName(prjfilepath);

				var path = Path.GetDirectoryName(prjfilepath);
				string ver = oldversion;
				char letter = 'a';

				// Find an unused backup folder name.
				do
				{
					BackupFolder = Path.Combine(path, string.Format("Backup-{0}-{1}", ProjectName, ver));
					ver = oldversion + letter;
					letter += (char)1;
				}
				while (Directory.Exists(BackupFolder));

				Directory.CreateDirectory(BackupFolder);

				// Copy the project files to the backup folder.
				foreach (var filepath in Directory.GetFiles(path, ProjectName + ".*"))
				{
					var filename = Path.GetFileName(filepath);
					File.Copy(filepath, Path.Combine(BackupFolder, filename));
				}

				// This will make sure the project file (i.e. .pap) gets backed-up
				// in case its file name is not the same as the project name.
				var prjfilename = Path.GetFileName(prjfilepath);
				if (!File.Exists(Path.Combine(BackupFolder, prjfilename)))
					File.Copy(prjfilepath, Path.Combine(BackupFolder, prjfilename));

				return null;
			}
			catch (Exception e)
			{
				return e;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected bool TransformFile(string filename, string transformNamespace, out string errMsg)
		{
			errMsg = null;
			var assembly = Assembly.GetExecutingAssembly();

			using (var stream = assembly.GetManifestResourceStream(transformNamespace))
			{
				var updatedFile = XmlHelper.TransformFile(filename, stream);
				if (updatedFile == null)
				{
					errMsg = App.LocalizeString("ProjectMigrationTransformationFailureMsg",
						"Migration transformation failed.", App.kLocalizationGroupInfoMsg);
					return false;
				}

				try
				{
					File.Delete(filename);
					File.Move(updatedFile, filename);
					return true;
				}
				catch (Exception e)
				{
					errMsg = e.Message;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected void ShowSuccessMsg()
		{
			var msg = App.LocalizeString("ProjectMigrationSuccessfulMsg",
				"The '{0}' project has succssfully been upgraded to work with this version of {1}. A backup of your old project has been made in:\n\n{2}",
				App.kLocalizationGroupInfoMsg);

			Utils.MsgBox(string.Format(msg, ProjectName, Application.ProductName, BackupFolder));
		}
	}
}
