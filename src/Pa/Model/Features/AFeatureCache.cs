using System.Xml.Linq;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class AFeatureCache : FeatureCacheBase
	{
		/// ------------------------------------------------------------------------------------
		public static AFeatureCache Load(string phoneticInventoryFilePath)
		{
			var root = XElement.Load(phoneticInventoryFilePath);
			var cache = new AFeatureCache();
			cache.LoadFromList(FeatureCacheBase.ReadFeaturesFromXElement(root, "descriptive"));
			return cache;
		}
	}
}
