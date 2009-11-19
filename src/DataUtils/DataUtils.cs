using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DataUtils
	{
		public static char[] kTieBars = new char[] { '\u0361', '\u035C' };
		public const string kTopTieBar = "\u0361";
		public const string kBottomTieBar = "\u035C";
		public const char kTopTieBarC = '\u0361';
		public const char kBottomTieBarC = '\u035C';
		public const string kDottedCircle = "\u25CC";
		public const char kDottedCircleC = '\u25CC';
		public const char kOrc = '\uFFFC';
		public const string kDiacriticPlaceholder = "[" + kDottedCircle + "]";
		public const string kSearchPatternDiamond = "\u25CA";
		public const string kEmptyDiamondPattern = kSearchPatternDiamond + "/" +
			kSearchPatternDiamond + "_" + kSearchPatternDiamond;

		private static Form m_mainWindow;
		internal static AFeatureCache s_aFeatureCache;
		internal static BFeatureCache s_bFeatureCache;
		internal static IPACharCache s_ipaCharCache;

		#region Cache Loading methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forces the IPA character cache to be reloaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ReloadIPACharCache()
		{
			s_ipaCharCache.Clear();
			s_ipaCharCache = IPACharCache.Load();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forces the IPA character cache to be reloaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadIPACharCache(string projectFileName)
		{
			if (s_ipaCharCache != null)
				s_ipaCharCache.Clear();
			
			s_ipaCharCache = IPACharCache.Load(projectFileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the articulatory feature cache for the project whose file name is specified
		/// by projectFileName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadAFeatureCache(string projFilePrefix)
		{
			// NOTE: For now, the projFilePrefix is not used. Use it if
			// we ever want to store features at the project level.

			s_aFeatureCache = new AFeatureCache();
			s_aFeatureCache.Load();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the articulatory feature cache for the project whose file name is specified
		/// by projectFileName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadBFeatureCache(string projFilePrefix)
		{
			// NOTE: For now, the projFilePrefix is not used. Use it if
			// we ever want to store features at the project level.

			s_bFeatureCache = new BFeatureCache();
			s_bFeatureCache.Load();
		}

		#endregion

		#region Misc. Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This property is included here because it is used in this assembly and I didn't
		/// want this assembly to be dependent on very many other assemblies.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Form MainWindow
		{
			get {return m_mainWindow;}
			set {m_mainWindow = value;}
		}

		#endregion

		#region Cache Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the IPACharacters cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IPACharCache IPACharCache
		{
			get
			{
				if (s_ipaCharCache == null)
					s_ipaCharCache = IPACharCache.Load();

				return s_ipaCharCache;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of articulatory features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AFeatureCache AFeatureCache
		{
			get
			{
				if (s_aFeatureCache == null)
				{
					s_aFeatureCache = new AFeatureCache();
					s_aFeatureCache.Load();
				}

				return s_aFeatureCache;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of binary features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static BFeatureCache BFeatureCache
		{
			get
			{
				if (s_bFeatureCache == null)
				{
					s_bFeatureCache = new BFeatureCache();
					s_bFeatureCache.Load();
				}
				return s_bFeatureCache;
			}
		}

		#endregion

		#region Misc. Helper Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a manner of articulation sort key for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMOAKey(string phone)
		{
			// TODO: When chow characters are supported, figure out how to deal with them.

			if (string.IsNullOrEmpty(phone))
				return null;

			StringBuilder keybldr = new StringBuilder(6);
			foreach (char c in phone)
			{
				IPACharInfo info = IPACharCache[c];
				keybldr.Append(info == null ? "000" :
					string.Format("{0:X3}", info.MOArticulation));
			}

			return keybldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a place of articulation sort key for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetPOAKey(string phone)
		{
			// TODO: When chou characters are supported, figure out how to deal with them.

			if (string.IsNullOrEmpty(phone))
				return null;

			StringBuilder keybldr = new StringBuilder(6);
			foreach (char c in phone)
			{
				IPACharInfo info = IPACharCache[c];
				keybldr.Append(info == null ? "000" :
					string.Format("{0:X3}", info.POArticulation));
			}

			return keybldr.ToString();
		}

		#endregion
	}
}