using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// Class for handling phones whose features cannot be accuratly deduced by simply taking
	/// the result of performing a bitwise OR on the features of each codepoint making up the
	/// phone. Therefore, phones in this list have their features overridden.
	/// ----------------------------------------------------------------------------------------
	public class FeatureOverrides : PhoneCache
	{
		public const string kFileName = "FeatureOverrides.xml";

		#region Method to migrate previous versions of ambiguous sequences file to current.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void MigrateToLatestVersion(string filename)
		{
			var errMsg = App.LocalizeString("FeatureOverridesMigrationErrMsg",
				"The following error occurred while attempting to update your project’s feature " +
				"overrides file:\n\n{0}\n\nIn order to continue working, your original feature " +
				"overrides file  will be renamed to the following file: '{1}'.",
				"Message displayed when updating ambiguous sequences file to new version.",
				App.kLocalizationGroupMisc);

			App.MigrateToLatestVersion(filename, Assembly.GetExecutingAssembly(),
				"SIL.Pa.Model.UpdateFileTransforms.UpdateFeatureOverridesFile.xslt", errMsg);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the default and project-specific list of overriding phone features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FeatureOverrides Load(string projectPathPrefix)
		{
			string filename = projectPathPrefix + kFileName;
			MigrateToLatestVersion(filename);
			var list = XmlSerializationHelper.DeserializeFromFile<List<PhoneInfo>>(filename, "phones");

			if (list == null || list.Count == 0)
				return null;

			var overrides = new FeatureOverrides();
			foreach (PhoneInfo phoneInfo in list)
				overrides[phoneInfo.Phone] = phoneInfo;

			return overrides;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the list of phones with overridden features to a project-specific xml file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string projectPathPrefix)
		{
			// Move the entries into a list because dictionaries are not serializable.
			var list = Values.Select(x => x as PhoneInfo).ToList();

			foreach (var phone in list)
			{
				if (!phone.AFeaturesAreOverridden)
					phone.AMask = FeatureMask.Empty;

				if (!phone.BFeaturesAreOverridden)
					phone.BMask = FeatureMask.Empty;
			}
			
			string filename = projectPathPrefix + kFileName;
			XmlSerializationHelper.SerializeToFile(filename, list, "phones");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the phones in the override list, finds it's phone entry in the phone
		/// cache and replaces the features in the phone cache entry with those from the
		/// override list entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void MergeWithPhoneCache(PhoneCache phoneCache)
		{
			foreach (KeyValuePair<string, IPhoneInfo> kvp in this)
			{
				var phoneOverride = kvp.Value as PhoneInfo;
				var phoneCacheEntry = phoneCache[kvp.Key] as PhoneInfo;
				if (phoneOverride == null || phoneCacheEntry == null)
					continue;

				if (phoneOverride.AFeaturesAreOverridden)
					phoneCacheEntry.OverrideAFeature(phoneOverride.AMask);

				if (phoneOverride.BFeaturesAreOverridden)
					phoneCacheEntry.OverrideBFeature(phoneOverride.BMask);
			}
		}
	}
}
