using System;
using System.IO;

namespace SIL.Pa.Model.Migration
{
    public class Migration0347 : MigrationBase
    {
		/// ------------------------------------------------------------------------------------
		public static Exception Migrate(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
		{
			var migrator = new Migration0347(prjfilepath, getPrjPathPrefixAction);
			return migrator.DoMigration();
		}
		
		/// ------------------------------------------------------------------------------------
		private Migration0347(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
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
                var error = TransformFile(filepath, "SIL.Pa.Model.Migration.UpdateWord2003XML.xslt");
                if (error != null)
                    throw error;
            }
        }
    }
}
