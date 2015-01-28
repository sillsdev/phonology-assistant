using System;
using System.IO;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.Model.Migration
{
    public class Migration0349 : MigrationBase
    {
		/// ------------------------------------------------------------------------------------
		public static Exception Migrate(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
		{
			var migrator = new Migration0349(prjfilepath, getPrjPathPrefixAction);
			return migrator.DoMigration();
		}
		
		/// ------------------------------------------------------------------------------------
		private Migration0349(string prjfilepath, Func<string, string, string> getPrjPathPrefixAction)
			: base(prjfilepath, getPrjPathPrefixAction)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override void InternalMigration()
		{
		    MigrateToAddRhymeEtc();
            UpdateProjectFileToLatestVersion();
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
