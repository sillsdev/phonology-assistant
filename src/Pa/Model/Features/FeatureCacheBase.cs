using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class FeatureCacheBase : Dictionary<string, Feature>
	{
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<Feature> ReadFeaturesFromXElement(XElement root, string featureType)
		{
			var featureDefs = root.Elements("featureDefinitions")
				.FirstOrDefault(e => (string)e.Attribute("class") == featureType);

			return (featureDefs == null ? new List<Feature>(0) :
				featureDefs.Elements("featureDefinition").Select(e => Feature.FromXElement(e)));
		}

		#region Methods for loading and saving
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads binary features from the specified list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void LoadFromList(IEnumerable<Feature> list)
		{
			Debug.Assert(list != null);
			Clear();

			int bit = 0;
			foreach (var feature in list.Where(f => f.Name != null))
			{
				feature.Name = CleanNameForLoad(feature.Name);
				feature.FullName = CleanNameForLoad(feature.GetBaseFullName());
				feature.Bit = bit++;
				Add(feature);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cleans up a feature name before adding it to the cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string CleanNameForLoad(string name)
		{
			return (name == null ? null : name.Trim());
		}
	
		#endregion

		/// ------------------------------------------------------------------------------------
		public Feature this[int bit]
		{
			get	{ return Values.FirstOrDefault(feature => feature.Bit == bit);	}
		}

		/// ------------------------------------------------------------------------------------
		public new Feature this[string featureName]
		{
			get
			{
				if (featureName == null)
					return null;

				featureName = CleanUpFeatureName(featureName);
				Feature feature;
				if (TryGetValue(featureName, out feature))
					return feature;

				// If we failed to get a feature object from the specified name, then check
				// if the name is the full name of a feature by going through the collection
				// to see if one of their full names matches featureName.
				return Values.FirstOrDefault(f => featureName == f.FullName.ToLower());
			}
			set
			{
				if (featureName != null)
				{
					featureName = CleanUpFeatureName(featureName);
					if (featureName.Length > 0)
						base[featureName] = value;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified feature to the cache, using the feature's name as its key.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(Feature feature)
		{
			if (feature != null && !string.IsNullOrEmpty(feature.Name))
				this[feature.Name] = feature;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected static string CleanUpFeatureName(string featureName)
		{
			if (featureName == null)
				return string.Empty;

			featureName = featureName.Replace("[", string.Empty);
			featureName = featureName.Replace("]", string.Empty);
			return featureName.Trim().ToLower();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the cache contains a feature having the
		/// specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool FeatureExits(string featureName)
		{
			return FeatureExits(featureName, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the cache contains a feature having the
		/// specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool FeatureExits(string featureName, bool showMsgWhenExists)
		{
			string key = (featureName == null ? null : featureName.ToLower());

			if (key == null || !ContainsKey(key))
				return false;

			if (showMsgWhenExists)
			{
				var msg = App.GetString("FeatureExistsMsg", "Feature '{0}' already exists.",
					"Message displayed when user is trying to add a new feature that already exists.");
				
				Utils.MsgBox(string.Format(msg, featureName));
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of sorted feature names for the features in the specified masks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetFeatureList(FeatureMask mask)
		{
			return GetFeatureList(mask, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of feature names for the features in the specified masks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetFeatureList(FeatureMask mask, bool sorted)
		{
			if (mask == null)
				return new List<string>();

			var list = (from feature in Values
						where mask[feature.Bit]
						select feature.Name).ToList();

			if (sorted)
				list.Sort();

			return list;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string representing all the features in the specified mask. The feature
		/// names are joined and delimited by a comma and space.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetFeaturesText(FeatureMask mask)
		{
			var featureList = GetFeatureList(mask);
			var bldrfeatures = new StringBuilder();

			foreach (var feature in featureList)
				bldrfeatures.AppendFormat("{0}, ", feature);

			return (bldrfeatures.ToString().TrimEnd(',', ' '));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a mask initialized with as many bits as there are cache items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask GetEmptyMask()
		{
			return new FeatureMask(Count);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a mask for the specified feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask GetMask(string fname)
		{
			return GetMask(this[fname]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a feature mask having a single bit set that corresponds to the specified
		/// feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask GetMask(Feature feature)
		{
			var mask = GetEmptyMask();

			if (feature != null)
				mask[feature.Bit] = true;

			return mask;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the mask for the specified list of feature names.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask GetMask(List<string> features)
		{
			var mask = GetEmptyMask();

			if (features != null)
			{
				foreach (var feature in features.Select(fname => this[fname]).Where(f => f != null))
					mask[feature.Bit] = true;
			}

			return mask;
		}
	}
}
