// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FeatureCacheBase.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SIL.Pa.Data.Properties;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FeatureCacheBase : SortedDictionary<string, Feature>
	{
		#region Methods for loading and saving
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file name from which features are deserialized and serialized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string FileName
		{
			get { throw new NotImplementedException("FileName"); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file name affix used for feature files specific to a project. This is
		/// only used if features are saved by project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string FileNameAffix
		{
			get { throw new NotImplementedException("FileNameAffix"); }
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full file name path by combining the path of the executing assembly and
		/// the instances FileName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected string FullFileNamePath
		{
			get { return Path.Combine(Utils.GetMyAssemblyPath(), FileName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the binary feature table from the database into a memory cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool Load()
		{
			var list = Utils.DeserializeData(FullFileNamePath, typeof(List<Feature>)) as List<Feature>;

			if (list == null)
				return false;

			LoadFromList(list);
			return true;
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads binary features from the specified list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void LoadFromList(List<Feature> list)
		{
			Debug.Assert(list != null);
			Clear();

			int bit = 0;
			foreach (Feature feature in list)
			{
				if (feature.Name != null)
				{
					feature.Name = CleanNameForLoad(feature.Name);
					string fullName = ReflectionHelper.GetField(feature, "m_fullname") as string;
					feature.FullName = CleanNameForLoad(fullName);
					feature.Bit = bit++;
					Add(feature);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cleans up a feature name before adding it to the cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual string CleanNameForLoad(string name)
		{
			return (name == null ? null : name.Trim());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the cache to it's XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			Save(null);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the cache to it's XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string fileprefix)
		{
			string filename = FullFileNamePath;
			if (fileprefix != null)
			{
				filename = fileprefix + FileNameAffix;
				filename = Path.Combine(Utils.GetMyAssemblyPath(), filename);
			}
			
			Utils.SerializeData(filename, Values.ToList());
		}
	
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the feature for the specified bit.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Feature this[int bit]
		{
			get	{ return Values.FirstOrDefault(feature => feature.Bit == bit);	}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
				foreach (Feature feat in Values)
				{
					if (featureName == feat.FullName.ToLower())
						return feat;
				}

				return null;
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
				Utils.MsgBox(string.Format(Resources.kstidFeatureExistsMsg, featureName),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an array of feature names for the features in the specified masks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> GetFeatureList(FeatureMask mask)
		{
			if (mask == null)
				return new List<string>();

			return new List<string>(from feature in Values
									where mask[feature.Bit]
									select feature.Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string representing all the features in the specified mask. The feature
		/// names are joined and delimited by a comma and space.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetFeaturesText(FeatureMask mask)
		{
			List<string> featureList = GetFeatureList(mask);
			var bldrfeatures = new StringBuilder();

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
			FeatureMask mask = GetEmptyMask();

			if (features != null)
			{
				foreach (string fname in features)
				{
					Feature feature = this[fname];
					if (feature != null)
						mask[feature.Bit] = true;
				}
			}

			return mask;
		}
	}
}
