using System;
using System.IO;
using System.Xml;

namespace SIL.Pa.Model.Migration
{
    public class Migration0350 : MigrationBase
    {
		/// ------------------------------------------------------------------------------------
		public static Exception Migrate(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
		{
			var migrator = new Migration0350(prjfilepath, getPrjPathPrefixAction);
			return migrator.DoMigration();
		}

		/// ------------------------------------------------------------------------------------
		private Migration0350(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
			: base(prjfilepath, getPrjPathPrefixAction)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override void InternalMigration()
		{
		    MigrateLocalization();
		    AddTrainingLanguageCode();
            UpdateProjectFileToLatestVersion();
        }

        private void AddTrainingLanguageCode()
        {
            if (_projectFilePath.Contains("Training") && _projectFilePath.Contains("Sekpele"))
            {
                var xDoc = new XmlDocument();
                xDoc.Load(_projectFilePath);
                var node = xDoc.SelectSingleNode("//languageName");
                if (xDoc.SelectSingleNode("languageCode") == null && node != null)
                {
                    var languageCodeNode = xDoc.CreateElement("languageCode");
                    languageCodeNode.InnerText = "lip";
                    node.ParentNode.InsertAfter(languageCodeNode, node);
                    xDoc.Save(_projectFilePath);
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        private void MigrateLocalization()
        {
            var localizedStringFilesFolder = Path.Combine(App.ProjectFolder, App.kLocalizationsFolder);
            var dirInfo = new DirectoryInfo(localizedStringFilesFolder);
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                var filepath = fileInfo.FullName;
                var error = TransformFile(filepath, "SIL.Pa.Model.Migration.UpdatePaUI0350.xslt");
                if (error != null)
                    throw error;
            }
        }
    }
}
