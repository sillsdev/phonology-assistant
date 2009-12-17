using System.Collections.Generic;
using System.Xml.Serialization;
using SilUtils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Used for deserialization
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("articulatoryFeatures")]
	public class AFeatureList : List<Feature>
	{
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AFeatureCache : FeatureCacheBase
	{
		//public const string kDefaultCacheFile = "DefaultAFeatures.xml";

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the file name from which features are deserialized and serialized.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override string FileName
		//{
		//    get { return kDefaultAFeatureCacheFile; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the file name affix used for articulatory feature files specific to a
		///// project. This is only used if features are saved by project.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override string FileNameAffix
		//{
		//    get { return ".AFeatures.xml"; }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the articulatory features into a memory cache from the specified
		/// chunk of data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AFeatureCache Load(string data)
		{
			var list = Utils.DeserializeFromString<AFeatureList>(data);

			if (list == null)
				return null;

			var cache = new AFeatureCache();
			cache.LoadFromList(list);
			return cache;
		}
	}
}
