namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AFeatureCache : FeatureCacheBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file name from which features are deserialized and serialized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string FileName
		{
			get { return "DefaultAFeatures.xml"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file name affix used for articulatory feature files specific to a
		/// project. This is only used if features are saved by project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string FileNameAffix
		{
			get { return ".AFeatures.xml"; }
		}
	}
}
