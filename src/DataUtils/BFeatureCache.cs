using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SilUtils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class BFeatureCache : FeatureCacheBase
	{
		#region Overridden Properties/Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file name from which features are deserialized and serialized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string FileName
		{
			get { return "DefaultBFeatures.xml"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file name affix used for binary feature files specific to a project. This
		/// is only used if features are saved by project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string FileNameAffix
		{
			get { return ".BFeatures.xml"; }
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads binary features from the specified list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void LoadFromList(List<Feature> list)
		{
			Debug.Assert(list != null);
			Clear();

			// This will add all the plus features.
			base.LoadFromList(list);

			// Now add the minus features.
			int bit = list.Count;
			foreach (Feature plusFeature in list)
			{
				if (plusFeature.Name == null)
					continue;

				Feature minusFeature = plusFeature.Clone();

				minusFeature.Bit = bit++;
				minusFeature.Name = "-" + minusFeature.Name.Substring(1);
				string fullName = ReflectionHelper.GetField(minusFeature, "m_fullname") as string;
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
		protected override string CleanNameForLoad(string name)
		{
			name = base.CleanNameForLoad(name);

			if (name == null)
				return null;

			name = name.Trim();

			if (name.StartsWith("-"))
				name = name.Substring(1);

			if (!name.StartsWith("+"))
				name = "+" + name;

			return name;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the plus features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<Feature> PlusFeatures
		{
			get
			{
				return (from feat in Values
						where feat.Name.StartsWith("+")
						select feat).ToList();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the minus features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<Feature> MinusFeatures
		{
			get
			{
				return (from feat in Values
						where feat.Name.StartsWith("-")
						select feat).ToList();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the opposite feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Feature GetOppositeFeature(Feature feature)
		{
			return (feature == null ? null : GetOppositeFeature(feature.Name));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the opposite feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Feature GetOppositeFeature(string featName)
		{
			if (string.IsNullOrEmpty(featName))
				return null;

			StringBuilder name = new StringBuilder(featName);
			name[0] = (name[0] == '+' ? '-' : '+');
			return this[name.ToString()];
		}
	}
}
