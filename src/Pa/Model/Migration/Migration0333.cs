using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.Model.Migration
{
	public class Migration0333 : MigrationBase
	{
		/// ------------------------------------------------------------------------------------
		public static bool Migrate(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
		{
			var migrator = new Migration0333(prjfilepath, GetPrjPathPrefixAction);
			return migrator.InternalMigration();
		}
		
		/// ------------------------------------------------------------------------------------
		private Migration0333(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
			: base(prjfilepath, GetPrjPathPrefixAction)
		{
		}

		/// ------------------------------------------------------------------------------------
		private bool InternalMigration()
		{
			var e = BackupProject("0332");
			if (e != null)
			{
				var msg = App.GetString("ProjectMigrationBackupErrorMsg",
					"The following error occurred while attempting to backup your project before updating it for the latest version of {0}:\n\n{1}");

				Utils.MsgBox(string.Format(msg, Application.ProductName, e.Message));
				return false;
			}

			var filepath = SearchQueryGroupList.GetSearchQueryFileForProject(_projectPathPrefix);
			if (File.Exists(filepath))
				MigrateSearchQueries(filepath);

			if (File.Exists(App.GetPathToRecentlyUsedSearchQueriesFile()))
			    MigrateRecentlyUsedSearchQueries(App.GetPathToRecentlyUsedSearchQueriesFile());

			if (File.Exists(_projectFilePath))
			{
				MigrateProjectFile();
				UpdateProjectFileToLatestVersion();
			}

			ShowSuccessMsg();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateSearchQueries(string filePath)
		{
			var root = XElement.Load(filePath);

			foreach (var queryElement in root.Elements("QueryGroup")
				.Select(grp => grp.Element("Queries"))
				.Where(e => e != null)
				.SelectMany(e => e.Elements("SearchQuery")))
			{
				MigrateSingleSearchQuery(queryElement);
			}

			root.Save(filePath);
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateRecentlyUsedSearchQueries(string filePath)
		{
			var root = XElement.Load(filePath);

			foreach (var queryElement in root.Elements("SearchQuery"))
				MigrateSingleSearchQuery(queryElement);

			root.Save(filePath);
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateProjectFile()
		{
			var changesMade = false;
			var root = XElement.Load(_projectFilePath);
			
			var cieOptionsElement = root.Element("CIEOptions");
			if (cieOptionsElement != null)
			{
				var queryElement = cieOptionsElement.Element("SearchQuery");
				if (queryElement != null)
				{
					MigrateSingleSearchQuery(queryElement);
					changesMade = true;
				}
			}

			var cvPatternListElement = root.Element("CVPatternInfoList");
			if (cvPatternListElement != null)
			{
				foreach (var cvPatternElement in cvPatternListElement.Elements("CVPatternInfo"))
				{
					changesMade = true;
					switch ((string)cvPatternElement.Attribute("Type"))
					{
						case "StressSyllable": cvPatternElement.Attribute("Type").Value = CVPatternInfo.PatternType.Suprasegmental.ToString(); break;
						case "Length": cvPatternElement.Attribute("Type").Value = CVPatternInfo.PatternType.Suprasegmental.ToString(); break;
						case "Tone": cvPatternElement.Attribute("Type").Value = CVPatternInfo.PatternType.Suprasegmental.ToString(); break;
						default: cvPatternElement.Attribute("Type").Value = CVPatternInfo.PatternType.Custom.ToString(); break;
					}
				}
			}

			if (changesMade)
				root.Save(_projectFilePath);
		}

		/// ------------------------------------------------------------------------------------
		private void MigrateSingleSearchQuery(XElement element)
		{
			element.Attribute("version").SetValue(SearchQuery.kCurrVersion);
			
			var completeIgnoredListNode = element.Element("CompleteIgnoredList");
			if (completeIgnoredListNode != null)
				completeIgnoredListNode.Remove();

			var ignoredCharacters = string.Empty;

			var ignoredCharsElement = element.Element("IgnoredStressChars");
			if (ignoredCharsElement != null)
			{
				ignoredCharacters = ignoredCharsElement.Value.Trim(',', ' ') + ",";
				ignoredCharsElement.Remove();
			}

			ignoredCharsElement = element.Element("IgnoredToneChars");
			if (ignoredCharsElement != null)
			{
				ignoredCharacters += ignoredCharsElement.Value.Trim(',', ' ') + ",";
				ignoredCharsElement.Remove();
			}

			ignoredCharsElement = element.Element("IgnoredLengthChars");
			if (ignoredCharsElement != null)
			{
				ignoredCharacters += ignoredCharsElement.Value.Trim(',', ' ') + ",";
				ignoredCharsElement.Remove();
			}
			
			if (ignoredCharacters.Trim(',').Length > 0)
				element.Add(new XElement("ignoredCharacters", ignoredCharacters));
		}
	}
}
