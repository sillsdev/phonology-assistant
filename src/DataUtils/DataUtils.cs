using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DataUtils
	{
		public static char[] kTieBars = new[] { '\u0361', '\u035C' };
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
		internal static IPASymbolCache s_ipaCharCache;

		#region Cache Loading methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forces the IPA character cache to be reloaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ReloadIPACharCache()
		{
			s_ipaCharCache.Clear();
			s_ipaCharCache = IPASymbolCache.Load();
		}

				/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forces the IPA character cache to be reloaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadIPASymbolsAndFeatures(string path)
		{
			XmlTypeAttribute[] attribs = typeof(AFeatureList).GetCustomAttributes(
				typeof(XmlTypeAttribute), false) as XmlTypeAttribute[];

			string afeatureNodeName =  (attribs != null ? attribs[0].TypeName : "articulatoryFeatures");

			attribs = typeof(BFeatureList).GetCustomAttributes(
				typeof(XmlTypeAttribute), false) as XmlTypeAttribute[];

			string bfeatureNodeName = (attribs != null ? attribs[0].TypeName : "binaryFeatures");

			attribs = typeof(IPASymbolList).GetCustomAttributes(
				typeof(XmlTypeAttribute), false) as XmlTypeAttribute[];

			string symbolsNodeName = (attribs != null ? attribs[0].TypeName : "symbols");
			
			path = Path.GetDirectoryName(path);
			path = Path.Combine(path, IPASymbolCache.kDefaultIPASymbolCacheFile);
			XmlTextReader reader = new XmlTextReader(path);

			reader.WhitespaceHandling = WhitespaceHandling.None;
			reader.ReadStartElement();

			while (!reader.EOF)
			{
				string nodeName = reader.Name;
				string data = reader.ReadOuterXml();

				if (nodeName == symbolsNodeName)
					s_ipaCharCache = IPASymbolCache.Load(path, data);
				else if (nodeName == afeatureNodeName)
					s_aFeatureCache = AFeatureCache.Load(data);
				else if (nodeName == bfeatureNodeName)
					s_bFeatureCache = BFeatureCache.Load(data);
			}

			reader.Close();
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Forces the IPA character cache to be reloaded.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static void LoadIPACharCache(string projectFileName)
		//{
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Loads the articulatory feature cache for the project whose file name is specified
		///// by projectFileName.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static void LoadAFeatureCache(string projFilePrefix)
		//{
		//    // NOTE: For now, the projFilePrefix is not used. Use it if
		//    // we ever want to store features at the project level.

		//    s_aFeatureCache = new AFeatureCache();
		//    s_aFeatureCache.Load(typeof(AFeatureList));
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Loads the articulatory feature cache for the project whose file name is specified
		///// by projectFileName.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static void LoadBFeatureCache(string projFilePrefix)
		//{
		//    // NOTE: For now, the projFilePrefix is not used. Use it if
		//    // we ever want to store features at the project level.

		//    s_bFeatureCache = new BFeatureCache();
		//    s_bFeatureCache.Load(typeof(BFeatureList));
		//}

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
		public static IPASymbolCache IPASymbolCache
		{
			get
			{
				if (s_ipaCharCache == null)
					LoadIPASymbolsAndFeatures(Application.ExecutablePath);
				
				//s_ipaCharCache = IPASymbolCache.Load();

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
					LoadIPASymbolsAndFeatures(Application.ExecutablePath);
					//s_aFeatureCache = new AFeatureCache();
					//s_aFeatureCache.Load(typeof(AFeatureList));
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
					LoadIPASymbolsAndFeatures(Application.ExecutablePath);
					//s_bFeatureCache = new BFeatureCache();
					//s_bFeatureCache.Load(typeof(BFeatureList));
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
				IPASymbol info = IPASymbolCache[c];
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
				IPASymbol info = IPASymbolCache[c];
				keybldr.Append(info == null ? "000" :
					string.Format("{0:X3}", info.POArticulation));
			}

			return keybldr.ToString();
		}

		#endregion
	}
}