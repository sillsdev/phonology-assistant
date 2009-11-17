using System.Collections.Generic;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// Class for handling phones whose features cannot be accuratly deduced by simply taking
	/// the result of performing a bitwise OR on the features of each codepoint making up the
	/// phone. Therefore, phones in this list have their features overridden.
	/// ----------------------------------------------------------------------------------------
	public class PhoneFeatureOverrides : PhoneCache
	{
		public const string kDefaultOverrideFile = "DefaultPhoneFeatureOverrides.xml";
		public const string kOverrideFile = "PhoneFeatureOverrides.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Construct the file name for the project-specific overrides.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string BuildFileName(string projectFileName)
		{
			string filename = (projectFileName ?? string.Empty);
			filename += (filename.EndsWith(".") ? string.Empty : ".") + kOverrideFile;
			return filename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the default and project-specific list of overriding phone features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PhoneFeatureOverrides Load(string projectFileName)
		{
			PhoneFeatureOverrides overrides = new PhoneFeatureOverrides();

			// Get the default list of overrides.
			List<PhoneInfo> defaultList = SilUtils.Utils.DeserializeData(kDefaultOverrideFile,
				typeof(List<PhoneInfo>)) as List<PhoneInfo>;

			// Get the project-specific list of overrides.
			string filename = BuildFileName(projectFileName);
			List<PhoneInfo> projectList = SilUtils.Utils.DeserializeData(filename,
				typeof(List<PhoneInfo>)) as List<PhoneInfo>;

			// Move the phones from the List<> to a dictionary.
			if (projectList != null && projectList.Count > 0)
			{
				foreach (PhoneInfo phoneInfo in projectList)
					overrides[phoneInfo.Phone] = phoneInfo;
			}

			// Combine the default list with the project specific list. If the same phone
			// is found in both lists, the project-specific instance takes precedence over
			// the default one.
			if (defaultList != null)
			{
				// Add those phones from the default list that aren't found in the
				// project-specific list.
				foreach (PhoneInfo phoneInfo in defaultList)
				{
					if (!overrides.ContainsKey(phoneInfo.Phone))
						overrides[phoneInfo.Phone] = phoneInfo;
				}
			}

			return (overrides.Count == 0 ? null : overrides);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the list of phones with overridden features to a project-specific xml file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string projectFileName)
		{
			List<PhoneInfo> overrideList = new List<PhoneInfo>();

			// Move the entries into a list because dictionaries are not serializable.
			foreach (PhoneInfo phoneInfo in Values)
				overrideList.Add(phoneInfo);

			string filename = BuildFileName(projectFileName);
			SilUtils.Utils.SerializeData(filename, overrideList);
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
				IPhoneInfo phoneCacheEntry = phoneCache[kvp.Key];
				if (phoneCacheEntry != null)
				{
					((PhoneInfo)phoneCacheEntry).AMask = kvp.Value.AMask.Clone();
					((PhoneInfo)phoneCacheEntry).BMask = kvp.Value.BMask.Clone();
				}
			}
		}
	}
}
