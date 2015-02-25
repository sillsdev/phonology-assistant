// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
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
			cache.LoadFromList(ReadFeaturesFromXElement(root, "descriptive"));
			return cache;
		}
	}
}
