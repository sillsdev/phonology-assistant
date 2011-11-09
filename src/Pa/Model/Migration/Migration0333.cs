using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.Model.Migration
{
	public class Migration0333 : MigrationBase
	{
		private static bool s_performPostProjectLoadMigration;

		private readonly List<Feature> _defaultDescriptiveFeatures;
		private readonly List<Feature> _defaultDistinctiveFeatures;
		
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
			_defaultDescriptiveFeatures = App.AFeatureCache.Select(kvp => kvp.Value).ToList();
			_defaultDistinctiveFeatures = BFeatureCache.GetFeaturesFromDefaultSet().ToList();
		}

		/// ------------------------------------------------------------------------------------
		protected override void InternalMigration()
		{
			var filepath = FeatureOverrides.GetFileForProject(_projectPathPrefix);
			if (File.Exists(filepath))
				MigrateFeatureOverrides(filepath);

			filepath = SearchQueryGroupList.GetFileForProject(_projectPathPrefix);
			if (File.Exists(filepath))
				MigrateSearchQueries(filepath);

			filepath = DistributionChart.GetFileForProject(_projectPathPrefix);
			if (File.Exists(filepath))
				MigrateDistributionCharts(filepath);

			if (File.Exists(App.GetPathToRecentlyUsedSearchQueriesFile()))
				MigrateRecentlyUsedSearchQueries(App.GetPathToRecentlyUsedSearchQueriesFile());



			//filepath = SearchClassList.GetFileForProject(_projectPathPrefix);
			//if (File.Exists(filepath))
			//    MigrateSearchClasses(filepath);

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

		#region Methods for migrating feature overrides file.
		/// ------------------------------------------------------------------------------------
		private void MigrateFeatureOverrides(string filePath)
		{
			var root = XElement.Load(filePath);

			if (!root.HasElements)
				return;

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
						"articulatoryFeatures", _defaultDescriptiveFeatures);
				}

				if ((string)element.Attribute("binaryFeaturesChanged") == "true")
				{
					foverride.BFeatureNames = GetFeatureNamesForType(element,
						"binaryFeatures", _defaultDistinctiveFeatures);
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

		#endregion

		#region Method to migrate search queries file
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
		private void MigrateSingleSearchQuery(XElement element)
		{
			element.Attribute("version").SetValue(SearchQuery.kCurrVersion);
			
			if ((string)element.Attribute("Pattern") != null)
				element.Attribute("Pattern").SetValue(UpdateFeatureNamesInPattern((string)element.Attribute("Pattern")));

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
		private string UpdateFeatureNamesInPattern(string pattern)
		{
			var regex = new Regex(@"\[(?<bracketedText>[^\[\]]+)\]");
			var match = regex.Match(pattern);
			var matches = new List<string>();

			while (match.Success)
			{
				matches.Add(match.Result("${bracketedText}"));
				match = match.NextMatch();
			}

			foreach (var text in matches)
			{
				if (_defaultDescriptiveFeatures.Any(f => f.Name == text.ToLowerInvariant()) ||
					_defaultDistinctiveFeatures.Any(f => f.Name == text.ToLowerInvariant()))
				{
					pattern = pattern.Replace(text, text.ToLowerInvariant());
				}
				else if (text == "Tap/Flap")
					pattern = pattern.Replace("Tap/Flap", "flap");
				else
				{
					switch (text.Substring(1))
					{
						case "DORS": pattern = pattern.Replace("DORS", "dorsal"); break;
						case "LAB": pattern = pattern.Replace("LAB", "labial"); break;
						case "COR": pattern = pattern.Replace("COR", "coronal"); break;
						case "delayed release": pattern = pattern.Replace("delayed release", "delayed"); break;
						case "Labio-dental": pattern = pattern.Replace("Labio-dental", "labiodental"); break;
						case "PHAR": pattern = pattern.Replace("PHAR", "radical"); break;
					}
				}
			}

			return pattern;
		}

		#endregion

		#region Method for migrating distribution charts
		/// ------------------------------------------------------------------------------------
		private void MigrateDistributionCharts(string filePath)
		{
			var oldRoot = XElement.Load(filePath);
			var newRoot = new XElement("distributionCharts");

			foreach (var oldChartElement in oldRoot.Elements("XYChart"))
			{
				var newChartElement = new XElement("chart",
					new XAttribute("name", (string)oldChartElement.Attribute("Name")));

				// Migrate the search items.
				var newSrchItemElement = GetDistChartSearchItems(oldChartElement.Element("SearchItems"));
				if (newSrchItemElement != null)
					newChartElement.Add(newSrchItemElement);

				// Migrate the search queries.
				var newSrchQueriesElement = GetDistChartSearchQueries(oldChartElement.Element("SearchQueries"));
				if (newSrchQueriesElement != null)
					newChartElement.Add(newSrchQueriesElement);

				// Migrate the column widths.
				var newColWidthsElement = GetDistChartColWidths(oldChartElement.Element("ColumnWidths"));
				if (newColWidthsElement != null)
					newChartElement.Add(newColWidthsElement);

				newRoot.Add(newChartElement);
			}

			newRoot.Save(filePath);
		}

		/// ------------------------------------------------------------------------------------
		private XElement GetDistChartSearchItems(XElement oldSrchItemsElement)
		{
			if (oldSrchItemsElement == null)
				return null;

			var newSrchItemElement = new XElement("searchItems");

			foreach (var itemElement in oldSrchItemsElement.Elements("string").Where(e => e.Value != string.Empty))
				newSrchItemElement.Add(new XElement("item", UpdateFeatureNamesInPattern(itemElement.Value)));

			return newSrchItemElement;
		}

		/// ------------------------------------------------------------------------------------
		private XElement GetDistChartSearchQueries(XElement oldSrchQueriesElement)
		{
			if (oldSrchQueriesElement == null)
				return null;
			
			var newSrchQueriesElement = new XElement("searchQueries");

			foreach (var oldSrchQueryElement in oldSrchQueriesElement.Elements("SearchQuery"))
			{
				MigrateSingleSearchQuery(oldSrchQueryElement);
				newSrchQueriesElement.Add(new XElement("query",
					oldSrchQueryElement.Attribute("version"),
					oldSrchQueryElement.Attribute("Pattern"),
					oldSrchQueryElement.Elements()));
			}

			return newSrchQueriesElement;
		}

		/// ------------------------------------------------------------------------------------
		private XElement GetDistChartColWidths(XElement oldColWidthsElement)
		{
			if (oldColWidthsElement == null)
				return null;

			var newColWidthsElement = new XElement("columnWidths");

			foreach (var widthElement in oldColWidthsElement.Elements("int").Where(e => e.Value != string.Empty))
				newColWidthsElement.Add(new XElement("width", widthElement.Value));

			return newColWidthsElement;
		}

		#endregion

		#region Method to migrate recently used search query file
		/// ------------------------------------------------------------------------------------
		private void MigrateRecentlyUsedSearchQueries(string filePath)
		{
			var root = XElement.Load(filePath);

			foreach (var queryElement in root.Elements("SearchQuery"))
				MigrateSingleSearchQuery(queryElement);

			root.Save(filePath);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void MigrateSearchClasses(string filepath)
		{
		}

		#region Method to migrate project file.
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
		public static void PostProjectLoadMigration(PaProject project)
		{
			if (s_performPostProjectLoadMigration)
				RemoveFeatureOverridesThatAreNotOverridden(project);

			s_performPostProjectLoadMigration = false;
		}

		#endregion
	}
}
