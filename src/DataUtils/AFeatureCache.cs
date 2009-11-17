using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AFeatureCache : FeatureCacheBase<AFeature>
	{
		public const string kDefaultAFeatureCacheFile = "DefaultAFeatures.xml";
		public const string kAFeatureCacheFile = "AFeatures.xml";
		
		private string m_cacheFileName = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal AFeatureCache(string projectFileName)
		{
			m_cacheFileName = BuildFileName(projectFileName, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the name from which to load or save the cache file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string BuildFileName(string projectFileName, bool mustExist)
		{
			string filename = (projectFileName ?? string.Empty);
			filename += (filename.EndsWith(".") ? string.Empty : ".") + kAFeatureCacheFile;

			if (!File.Exists(filename) && mustExist)
				filename = kDefaultAFeatureCacheFile;

			return filename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the articulatory feature table from the database into a memory cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AFeatureCache Load()
		{
			return Load(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the articulatory feature table from the database into a memory cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AFeatureCache Load(string projectFileName)
		{
			AFeatureCache cache = new AFeatureCache(projectFileName);

			// Deserialize to a list because Dictionaries are not deserializable.
			List<AFeature> tmpList = SilUtils.Utils.DeserializeData(cache.CacheFileName,
				typeof(List<AFeature>)) as List<AFeature>;

			if (tmpList == null)
				return null;

			int bit = 0;
			foreach (AFeature feature in tmpList)
			{
				feature.Bit = bit++;
				cache[feature.Name.ToLower()] = feature;
			}

			tmpList.Clear();
			return (cache.Count == 0 ? null : cache);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the cache to it's XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string projectFileName)
		{
			m_cacheFileName = BuildFileName(projectFileName, false);
			Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the cache to it's XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			// Copy cache to a sorted list (sorted by bit), then to a list because
			// Dictionaries cannot be serialized.
			SortedList<int, AFeature> tmpSortedList = new SortedList<int, AFeature>();
			List<AFeature> tmpList = new List<AFeature>();

			foreach (KeyValuePair<string, AFeature> feature in this)
				tmpSortedList[feature.Value.Bit] = feature.Value;

			foreach (KeyValuePair<int, AFeature> feature in tmpSortedList)
				tmpList.Add(feature.Value);

			SilUtils.Utils.SerializeData(m_cacheFileName, tmpList);
			tmpSortedList.Clear();
			tmpList.Clear();
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Adds the custom feature name to the cache.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public AFeature Add(string name, bool showMsgWhenAlreadyExists)
		//{
		//    string key = (name == null ? null : name.ToLower());
			
		//    if (string.IsNullOrEmpty(key) || FeatureExits(name, showMsgWhenAlreadyExists))
		//        return null;

		//    // Go through the cache to find the first blank
		//    // feature where the new feature can be added.
		//    foreach (KeyValuePair<string, AFeature> feature in this)
		//    {
		//        if (feature.Key.StartsWith(AFeature.kBlankPrefix))
		//        {
		//            AFeature newFeature = feature.Value;
		//            Remove(feature.Value.Name.ToLower());
		//            newFeature.Name = name;
		//            this[key] = newFeature;
		//            return newFeature;
		//        }
		//    }

		//    return null;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CacheFileName
		{
			get {return m_cacheFileName;}
		}
	}

	#region AFeature class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class defining an object to store the information for a single articulatory feature.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AFeature : FeatureBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones the specified binary feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AFeature Clone()
		{
			return Clone(new AFeature()) as AFeature;
		}
	}

	#endregion
}
