using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class DataUtils
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
		internal static AFeatureCache m_aFeatureCache;
		internal static BFeatureCache m_bFeatureCache;
		internal static IPACharCache m_ipaCharCache;

		#region Cache Loading methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forces the IPA character cache to be reloaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ReloadIPACharCache()
		{
			m_ipaCharCache.Clear();
			m_ipaCharCache = IPACharCache.Load();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forces the IPA character cache to be reloaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadIPACharCache(string projectFileName)
		{
			if (m_ipaCharCache != null)
				m_ipaCharCache.Clear();
			
			m_ipaCharCache = IPACharCache.Load(projectFileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the articulatory feature cache for the project whose file name is specified
		/// by projectFileName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadAFeatureCache(string projectFileName)
		{
			if (m_aFeatureCache != null)
				m_aFeatureCache.Clear();

			m_aFeatureCache = AFeatureCache.Load(projectFileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the articulatory feature cache for the project whose file name is specified
		/// by projectFileName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void LoadBFeatureCache(string projectFileName)
		{
			if (m_bFeatureCache != null)
				m_bFeatureCache.Clear();

			m_bFeatureCache = BFeatureCache.Load(projectFileName);
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
				if (m_ipaCharCache == null)
					m_ipaCharCache = IPACharCache.Load();

				return m_ipaCharCache;
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
				if (m_aFeatureCache == null)
					m_aFeatureCache = AFeatureCache.Load();

				return m_aFeatureCache;
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
				if (m_bFeatureCache == null)
					m_bFeatureCache = BFeatureCache.Load();

				return m_bFeatureCache;
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
				IPACharInfo info = DataUtils.IPACharCache[c];
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
				IPACharInfo info = DataUtils.IPACharCache[c];
				keybldr.Append(info == null ? "000" :
					string.Format("{0:X3}", info.POArticulation));
			}

			return keybldr.ToString();
		}

		#endregion
	}
}