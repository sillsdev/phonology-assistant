using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SIL.Pa.PhoneticSearching;
using SilTools;

namespace SIL.Pa.Model.Migration
{
	public class Migration0333 : MigrationBase
	{
		private static bool s_performPostProjectLoadMigration;
		
		/// ------------------------------------------------------------------------------------
		public static Exception Migrate(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
		{
			var migrator = new Migration0333(prjfilepath, GetPrjPathPrefixAction);
			return migrator.DoMigration();
		}
		
		/// ------------------------------------------------------------------------------------
		private Migration0333(string prjfilepath, Func<string, string, string> GetPrjPathPrefixAction)
			: base(prjfilepath, GetPrjPathPrefixAction)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override void InternalMigration()
		{
			var filepath = SearchQueryGroupList.GetFileForProject(_projectPathPrefix);
			if (File.Exists(filepath))
				MigrateSearchQueries(filepath);

			if (File.Exists(App.GetPathToRecentlyUsedSearchQueriesFile()))
				MigrateRecentlyUsedSearchQueries(App.GetPathToRecentlyUsedSearchQueriesFile());

			filepath = FeatureOverrides.GetFileForProject(_projectPathPrefix);
			if (File.Exists(filepath))
				MigrateFeatureOverrides(filepath);

			if (File.Exists(_projectFilePath))
			{
				MigrateProjectFile();
				UpdateProjectFileToLatestVersion();
			}

			s_performPostProjectLoadMigration = true;

			var msg = App.GetString("ProjectMigrationMessages.VerifyUpdatedFeatureOverridesMsg",
				"Important Note: Some features in Phonology Assistant have changed and the feature overrides " +
				"for the '{0}' project have been updated to work with this version of the program. From the " +
				"Tools menu, please go to the 'Descriptive Features' and 'Distinctive Features' dialog boxes " +
				"and verify the accuracy of overridden features. Overridden phones and features will be " +
				"highlighted in yellow.");

			Utils.MsgBox(string.Format(msg, ProjectName));
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
		private void MigrateFeatureOverrides(string filePath)
		{
			var root = XElement.Load(filePath);

			if (!root.HasElements)
				return;

			var defaultDescriptiveFeatures = App.AFeatureCache.Select(kvp => kvp.Value).ToList();
			var defaultDistinctiveFeatures = BFeatureCache.GetFeaturesFromDefaultSet().ToList();

			var newOverrideList = new List<FeatureOverride>();

			var phoneElements = from e in root.Elements("PhoneInfo")
								where (string)e.Attribute("articulatoryFeaturesChanged") == "true" ||
									(string)e.Attribute("binaryFeaturesChanged") == "true"
								select e;

			foreach (var element in phoneElements)
			{
				var foverride = new FeatureOverride { Phone = element.Attribute("Phone").Value };

				if ((string)element.Attribute("articulatoryFeaturesChanged") == "true")
				{
					foverride.AFeatureNames = GetFeatureNamesForType(element,
						"articulatoryFeatures", defaultDescriptiveFeatures);
				}

				if ((string)element.Attribute("binaryFeaturesChanged") == "true")
				{
					foverride.BFeatureNames = GetFeatureNamesForType(element,
						"binaryFeatures", defaultDistinctiveFeatures);
				}

				if (foverride.AFeatureNames.Count() > 0 || foverride.BFeatureNames.Count() > 0)
					newOverrideList.Add(foverride);
			}

			FeatureOverrides.BuildXmlForOverrides(newOverrideList, BFeatureCache.DefaultFeatureSetName).Save(filePath);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetFeatureNamesForType(XElement element,
			string featureType, List<Feature> defaultFeatureSet)
		{
			foreach (var fname in element.Element(featureType).Elements("feature").Select(e => e.Value))
			{
				if (fname.Substring(1) == "ATR") 
					yield return fname;
				if (fname.Substring(1) == "DORS")
					yield return fname.Replace("DORS", "dorsal");
				if (fname.Substring(1) == "LAB")
					yield return fname.Replace("LAB", "labial");
				if (fname.Substring(1) == "COR")
					yield return fname.Replace("COR", "coronal");
				if (fname.Substring(1) == "delayed release")
					yield return fname.Replace("delayed release", "delayed");
				if (fname.Substring(1) == "Labio-dental")
					yield return fname.Replace("Labio-dental", "labiodental");
				if (fname.Substring(1) == "PHAR")
					yield return fname.Replace("PHAR", "radical");

				if (defaultFeatureSet.Any(f => f.Name == fname.TrimStart('-', '+').ToLowerInvariant()))
					yield return fname.ToLowerInvariant();
			}
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

		/// ------------------------------------------------------------------------------------
		public static void PostProjectLoadMigration(PaProject project)
		{
			if (s_performPostProjectLoadMigration)
				RemoveFeatureOverridesThatAreNotOverridden(project);

			s_performPostProjectLoadMigration = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because of a bug in version 3.3.0 to 3.3.2, the feature overrides file may contain
		/// phones whose overridden features are the same as the phone's default features.
		/// This method will clean out the feature overrides file of phones whose features
		/// really aren't overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void RemoveFeatureOverridesThatAreNotOverridden(PaProject project)
		{
			var newOverrideList = new List<FeatureOverride>();
			
			foreach (var phoneInfo in project.PhoneCache.Values)
			{
				var foverride = project.FeatureOverrides.GetOverridesForPhone(phoneInfo.Phone);
				if (foverride == null)
					continue;

				if (!phoneInfo.HasAFeatureOverrides && foverride.AFeatureNames.Count() > 0)
					foverride.AFeatureNames = new List<string>(0);

				if (!phoneInfo.HasBFeatureOverrides && foverride.BFeatureNames.Count() > 0)
					foverride.BFeatureNames = new List<string>(0);

				if (foverride.AFeatureNames.Count() > 0 || foverride.BFeatureNames.Count() > 0)
					newOverrideList.Add(foverride);
			}

			var filePath = FeatureOverrides.GetFileForProject(project.ProjectPathFilePrefix);
			FeatureOverrides.BuildXmlForOverrides(newOverrideList, project.DistinctiveFeatureSet).Save(filePath);
			project.LoadFeatureOverrides();
		}
	}
}
