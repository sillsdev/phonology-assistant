// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SIL.IO;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class BFeatureCache : FeatureCacheBase
	{
		#region Overridden Properties/Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads binary features from the specified list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void LoadFromList(IEnumerable<Feature> list)
		{
			Debug.Assert(list != null);
			Clear();

			// This will add all the plus features.
			base.LoadFromList(list);

			// Now add the minus features.
			int bit = Values.Count;
			foreach (var minusFeature in Values.Where(f => f.Name != null).Select(plusFeature => plusFeature.Clone()).ToArray())
			{
				minusFeature.Bit = bit++;
				minusFeature.Name = "-" + minusFeature.Name.Substring(1);
				var fullName = minusFeature.GetBaseFullName();
				if (fullName != null)
					minusFeature.FullName = "-" + fullName.Substring(1);
				
				Add(minusFeature);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cleans up a binary feature name before adding it to the cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string CleanNameForLoad(string name)
		{
			name = base.CleanNameForLoad(name);

			if (name == null)
				return null;

			name = name.Trim();

			if (name.StartsWith("-", StringComparison.Ordinal))
				name = name.Substring(1);

            if (!name.StartsWith("+", StringComparison.Ordinal))
				name = "+" + name;

			return name;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the plus features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<Feature> PlusFeatures
		{
			get
			{
				return from feat in Values
                       where feat.Name.StartsWith("+", StringComparison.Ordinal)
					   select feat;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the minus features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<Feature> MinusFeatures
		{
			get
			{
				return from feat in Values
                       where feat.Name.StartsWith("-", StringComparison.Ordinal)
					   select feat;
			}
		}

		/// ------------------------------------------------------------------------------------
		public Feature GetOppositeFeature(Feature feature)
		{
			return (feature == null ? null : GetOppositeFeature(feature.Name));
		}

		/// ------------------------------------------------------------------------------------
		public Feature GetOppositeFeature(string featName)
		{
			if (String.IsNullOrEmpty(featName))
				return null;

			var name = new StringBuilder(featName);
			name[0] = (name[0] == '+' ? '-' : '+');
			return this[name.ToString()];
		}

		#region public, static methods
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<string> GetAvailableFeatureSetFiles()
		{
			var folder = Path.GetDirectoryName(DefaultFeatureSetFile);
			return (Directory.GetFiles(folder, "*.DistinctiveFeatures.xml"));
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<string> GetAvailableFeatureSetNames()
		{
			return GetAvailableFeatureSetFiles()
				.Select(f => Path.GetFileName(f).Replace(".DistinctiveFeatures.xml", string.Empty));
				//.Select(name => (name == DefaultFeatureSetName ? "(default)" : name));
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<Feature> GetFeaturesFromDefaultSet()
		{
			var root = XElement.Load(DefaultFeatureSetFile);
			return ReadFeaturesFromXElement(root, "distinctive");
		}

		/// ------------------------------------------------------------------------------------
		public static string DefaultFeatureSetFile
		{
			get
			{
				return FileLocationUtilities.GetFileDistributedWithApplication(
					App.ConfigFolderName, "default.DistinctiveFeatures.xml");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string DefaultFeatureSetName
		{
			get { return "default"; }
		}

		#endregion
	}
}
