using System;
using System.IO;

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
		    MigrateLocalizationWord2003ToWord();
            UpdateProjectFileToLatestVersion();
        }

        /// ------------------------------------------------------------------------------------
        private void MigrateLocalizationWord2003ToWord()
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
