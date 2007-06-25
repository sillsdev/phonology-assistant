using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BFeatureCache : SortedDictionary<string, BFeature>
	{
		public const string kDefaultBFeatureCacheFile = "DefaultBFeatures.xml";
		public const string kBFeatureCacheFile = "BFeatures.xml";
		private string m_cacheFileName = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal BFeatureCache(string projectFileName)
		{
			m_cacheFileName = BuildFileName(projectFileName, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the name from which to load or save the cache file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string BuildFileName(string projectFileName, bool mustExist)
		{
			string filename = (projectFileName == null ? string.Empty : projectFileName);
			filename += (filename.EndsWith(".") ? string.Empty : ".") + kBFeatureCacheFile;

			if (!File.Exists(filename) && mustExist)
				filename = kDefaultBFeatureCacheFile;

			return filename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CacheFileName
		{
			get { return m_cacheFileName; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the binary feature table from the database into a memory cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static BFeatureCache Load()
		{
			return Load(null);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the binary feature table from the database into a memory cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static BFeatureCache Load(string projectFileName)
		{
			BFeatureCache cache = new BFeatureCache(projectFileName);

			// Deserialize to a list because Dictionaries are not deserializable.
			List<BFeature> tmpList = STUtils.DeserializeData(cache.CacheFileName,
				typeof(List<BFeature>)) as List<BFeature>;

			foreach (BFeature feature in tmpList)
			{
				if (feature.Name != null)
					cache[feature.Name] = feature;
			}

			tmpList.Clear();
			tmpList = null;
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
			SortedList<int, BFeature> tmpSortedList = new SortedList<int, BFeature>();
			List<BFeature> tmpList = new List<BFeature>();

			foreach (KeyValuePair<string, BFeature> feature in this)
				tmpSortedList[feature.Value.Bit] = feature.Value;

			foreach (KeyValuePair<int, BFeature> feature in tmpSortedList)
				tmpList.Add(feature.Value);

			STUtils.SerializeData(m_cacheFileName, tmpList);
			tmpSortedList.Clear();
			tmpSortedList = null;
			tmpList.Clear();
			tmpList = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new BFeature this[string featureName]
		{
			get
			{
				featureName = CleanUpFeatureName(featureName);
				BFeature feature;
				return (TryGetValue(featureName, out feature) ? feature : null);
			}
			set
			{
				System.Diagnostics.Debug.Assert(value != null);
				featureName = CleanUpFeatureName(featureName);
				base[featureName] = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string CleanUpFeatureName(string featureName)
		{
			System.Diagnostics.Debug.Assert(featureName != null);
			featureName = featureName.Trim().ToLower();
			featureName = featureName.Replace("[", string.Empty);
			featureName = featureName.Replace("]", string.Empty);
			System.Diagnostics.Debug.Assert(featureName.Length > 0);
			if (featureName[0] == '+' || featureName[0] == '-')
				featureName = featureName.Substring(1);

			return featureName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified key (that's been converted to lowercase and
		/// had all its spaces removed) is in the cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BFeature FeatureFromCompactedKey(string compactKey)
		{
			foreach (KeyValuePair<string, BFeature> feature in this)
			{
				string modifiedKey = feature.Key.Replace(" ", string.Empty);
				if (compactKey == modifiedKey.ToLower())
					return feature.Value;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of feature names for the features in the specified mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] GetFeatureList(ulong mask)
		{
			List<string> featureList = new List<string>();

			foreach (KeyValuePair<string, BFeature> feature in this)
			{
				if ((mask & feature.Value.PlusMask) > 0)
					featureList.Add("+" + feature.Value.Name);

				if ((mask & feature.Value.MinusMask) > 0)
					featureList.Add("-" + feature.Value.Name);
			}

			return featureList.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string representing all the features in the specified mask. The feature
		/// names are joined and delimited by a comma and space.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetFeaturesText(ulong mask)
		{
			string[] featureList = GetFeatureList(mask);
			StringBuilder bldrfeatures = new StringBuilder();

			foreach (string feature in featureList)
			{
				bldrfeatures.Append(feature);
				bldrfeatures.Append(", ");
			}

			// Remove the last comma and space.
			if (bldrfeatures.Length >= 2)
				bldrfeatures.Length -= 2;

			return (bldrfeatures.ToString());
		}
	}

	#region BFeature class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class defining an object to store the information for a single binary feature.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BFeature
	{
		private int m_bit;
		private string m_name;
		private ulong m_plusMask;
		private ulong m_minusMask;

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public int Bit
		{
			get { return m_bit; }
			set { m_bit = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ulong PlusMask
		{
			get { return m_plusMask; }
			set { m_plusMask = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ulong MinusMask
		{
			get { return m_minusMask; }
			set { m_minusMask = value; }
		}

		#endregion
	}

	#endregion
}
