using System.Collections.Generic;
using System.Linq;
using SilUtils;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// Class for handling phones whose features cannot be accuratly deduced by simply taking
	/// the result of performing a bitwise OR on the features of each codepoint making up the
	/// phone. Therefore, phones in this list have their features overridden.
	/// ----------------------------------------------------------------------------------------
	public class FeatureOverrides : PhoneCache
	{
		public const string kOverrideFile = "FeatureOverrides.xml";

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
		public static FeatureOverrides Load(string projectFileName)
		{
			string filename = BuildFileName(projectFileName);
			var list = Utils.DeserializeData(filename, typeof(List<PhoneInfo>)) as List<PhoneInfo>;

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
		public void Save(string projectFileName)
		{
			// Move the entries into a list because dictionaries are not serializable.
			var list = Values.Select(x => x as PhoneInfo).ToList();
			Utils.SerializeData(BuildFileName(projectFileName), list);
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
