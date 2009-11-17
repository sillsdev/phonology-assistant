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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SIL.Pa.Data.Properties;
using System.Windows.Forms;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FeatureCacheBase<T> : SortedDictionary<string, T> where T : FeatureBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the feature for the specified bit.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public T this[int bit]
		{
			get	{ return Values.FirstOrDefault(feature => feature.Bit == bit);	}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new T this[string featureName]
		{
			get
			{
				if (featureName == null)
					return null;

				featureName = CleanUpFeatureName(featureName);
				T feature;
				if (TryGetValue(featureName, out feature))
					return feature;

				// If we failed to get a feature object from the specified name, then check
				// if the name is the full name of a feature by going through the collection
				// to see if one of their full names matches featureName.
				foreach (T feat in Values)
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected static string CleanUpFeatureName(string featureName)
		{
			if (featureName == null)
				return string.Empty;

			featureName = featureName.Trim().ToLower();
			featureName = featureName.Replace("[", string.Empty);
			return featureName.Replace("]", string.Empty);
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
				SilUtils.Utils.STMsgBox(
					string.Format(Resources.kstidFeatureExistsMsg, featureName),
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
		/// Gets the mask for the specified list of feature names.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask GetMask(List<string> features)
		{
			FeatureMask mask = new FeatureMask(Count);

			foreach (string fname in features)
			{
				T feature = this[fname];
				if (feature != null)
					mask[feature.Bit] = true;
			}

			return mask;
		}
	}
}
