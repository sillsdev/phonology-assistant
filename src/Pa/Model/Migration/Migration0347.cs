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
using SIL.Pa.PhoneticSearching;

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
		    if (!File.Exists(Path.Combine(_projectPathPrefix, DistributionChart.kFileName)))
		    {
		        var badPrefix = _projectPathPrefix.Substring(0, _projectPathPrefix.Length - 2) + "3.";
		        if (File.Exists(badPrefix + DistributionChart.kFileName))
		            File.Move(badPrefix + DistributionChart.kFileName, _projectPathPrefix + DistributionChart.kFileName);
		    }
		    MigrateLocalizationWord2003ToWord();
		    MigrateToAddRhymeEtc();
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
                var error = TransformFile(filepath, "SIL.Pa.Model.Migration.UpdatePaUI0347.xslt");
                if (error != null)
                    throw error;
            }
        }

        private void MigrateToAddRhymeEtc()
        {
            var filepath = DistributionChart.GetFileForProject(_projectPathPrefix);
            var error = TransformFile(filepath, "SIL.Pa.Model.Migration.AddRhymeToDistribution.xslt");
            if (error != null)
                throw error;
        }
    }
}
