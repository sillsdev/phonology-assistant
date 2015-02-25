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

namespace SIL.Pa.Model.Migration
{
    public class Migration0351 : MigrationBase
    {
		/// ------------------------------------------------------------------------------------
		public static Exception Migrate(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
		{
			var migrator = new Migration0351(prjfilepath, getPrjPathPrefixAction);
			return migrator.DoMigration();
		}

		/// ------------------------------------------------------------------------------------
		private Migration0351(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
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
                var error = TransformFile(filepath, "SIL.Pa.Model.Migration.UpdatePaUI0351.xslt");
                if (error != null)
                    throw error;
            }
        }
    }
}
