using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BFeatureCache : FeatureCacheBase<BFeature>
	{
		public const string kDefaultBFeatureCacheFile = "DefaultBFeatures.xml";
		public const string kBFeatureCacheFile = "BFeatures.xml";
		private string m_cacheFileName;

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
		private static string BuildFileName(string projectFileName, bool mustExist)
		{
			string filename = (projectFileName ?? string.Empty);
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
			var cache = new BFeatureCache(projectFileName);

			// Deserialize to a list because Dictionaries are not deserializable.
			var tmpList = SilUtils.Utils.DeserializeData(cache.CacheFileName,
				typeof(List<BFeature>)) as List<BFeature>;

			if (tmpList == null)
				return null;

			int bit = 0;
			foreach (BFeature plusFeature in tmpList)
			{
				if (plusFeature.Name == null)
					continue;
				
				BFeature minusFeature = plusFeature.Clone();

				plusFeature.Name = "+" + plusFeature.Name;
				plusFeature.FullName = "+" + plusFeature.FullName;
				plusFeature.Bit = bit++;
				plusFeature.IsPlus = true;

				minusFeature.Name = "-" + minusFeature.Name;
				minusFeature.FullName = "-" + minusFeature.FullName;
				minusFeature.Bit = bit++;

				cache[plusFeature.Name.ToLower()] = plusFeature;
				cache[minusFeature.Name.ToLower()] = minusFeature;
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
			// Copy cache to a sorted list (sorted by either one of the bits), then
			// to a list because Dictionaries cannot be serialized.
			var tmpSortedList = new SortedList<int, BFeature>();
			var tmpList = new List<BFeature>();

			//foreach (KeyValuePair<string, BFeature> feature in this)
			//    tmpSortedList[feature.Value.PlusBit] = feature.Value;

			foreach (KeyValuePair<int, BFeature> feature in tmpSortedList)
				tmpList.Add(feature.Value);

			SilUtils.Utils.SerializeData(m_cacheFileName, tmpList);
			tmpSortedList.Clear();
			tmpList.Clear();
		}
	}

	#region BFeature class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class defining an object to store the information for a single binary feature.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BFeature : FeatureBase
	{
		private bool m_isPlus;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones the specified binary feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BFeature Clone()
		{
			var clone = Clone(new BFeature()) as BFeature;
			clone.m_isPlus = m_isPlus;
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FullName;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether the feature is plus. If not, it is minus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsPlus
		{
			get { return m_isPlus; }
			internal set { m_isPlus = value; }
		}

		#endregion
	}

	#endregion
}
