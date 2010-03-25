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
// File: PhoneCache.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SIL.Pa.Model
{
	#region PhoneCache class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneCache : Dictionary<string, IPhoneInfo>
	{
		public const string kDefaultChartSupraSegsToIgnore = "\u0300\u0301\u0302\u0304" +
			"\u0306\u030b\u030c\u030f\u1dc4\u1dc5\u1dc8\u203f";

		private readonly string m_conSymbol = "C";
		private readonly string m_vowSymbol = "V";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneCache()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneCache(string conSymbol, string vowSymbol)
		{
			if (!string.IsNullOrEmpty(conSymbol))
				m_conSymbol = conSymbol;

			if (!string.IsNullOrEmpty(vowSymbol))
				m_vowSymbol = vowSymbol;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the IPhoneInfo object for the specified key.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new IPhoneInfo this[string key]
		{
			get
			{
				IPhoneInfo phoneInfo;
				return (TryGetValue(key, out phoneInfo) ? phoneInfo : null);
			}
			set {base[key] = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds to the phone cache, information about the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddPhone(string phone)
		{
			if (!string.IsNullOrEmpty(phone))
				this[phone] = new PhoneInfo(phone);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddUndefinedPhone(string phone)
		{
			IPhoneInfo phoneInfo;
			if (!TryGetValue(phone, out phoneInfo))
				this[phone] = new PhoneInfo(phone, true);
			else
			{
				if (phoneInfo is PhoneInfo)
					(phoneInfo as PhoneInfo).IsUndefined = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern for the specified phonetic string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCVPattern(string phonetic)
		{
			return GetCVPattern(App.IPASymbolCache.PhoneticParser(phonetic, true));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern for the specified phonetic string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCVPattern(string phonetic, bool convertExperimentalTranscriptions)
		{
			if (convertExperimentalTranscriptions)
				return GetCVPattern(App.IPASymbolCache.PhoneticParser(phonetic, true));

			Dictionary<int, string[]> uncertainPhones;
			string[] phones = App.IPASymbolCache.PhoneticParser(
				phonetic, true, false, out uncertainPhones);
			
			return GetCVPattern(phones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern for the specified array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCVPattern(string[] phones)
		{
			if (phones == null || phones.Length == 0)
				return null;

			StringBuilder bldr = new StringBuilder();
			foreach (string phone in phones)
			{
				PhoneInfo phoneInfo = this[phone] as PhoneInfo;

				if (CVPatternInfo.Contains(phone))
					bldr.Append(phone);
				else if (phoneInfo == null || phoneInfo.IsUndefined)
				{
					IPASymbol charInfo = App.IPASymbolCache[phone];
					if (charInfo != null)
					{
						if (charInfo.Type == IPASymbolType.Breaking)
							bldr.Append(' ');
						else if (charInfo.Type == IPASymbolType.Consonant)
							bldr.Append(m_conSymbol);
						else if (charInfo.Type == IPASymbolType.Vowel)
							bldr.Append(m_vowSymbol);
					}
				}
				else if (phoneInfo.CharType == IPASymbolType.Breaking)
					bldr.Append(' ');
				else if (phoneInfo.CharType == IPASymbolType.Consonant ||
				   phoneInfo.CharType == IPASymbolType.Vowel)
				{
					string diacriticsAfterBase = null;

					if (phone.Length > 1)
						diacriticsAfterBase = CVPatternInfo.GetMatchingModifiers(phone, bldr);

					bldr.Append(phoneInfo.CharType == IPASymbolType.Consonant ?
						m_conSymbol : m_vowSymbol);

					if (diacriticsAfterBase != null)
						bldr.Append(diacriticsAfterBase);
				}
			}

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Consonants
		{
			get {return GetTypeOfPhones(IPASymbolType.Consonant);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CommaDelimitedConsonants
		{
			get {return GetCommaDelimitedPhones(IPASymbolType.Consonant);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Vowels
		{
			get { return GetTypeOfPhones(IPASymbolType.Vowel); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CommaDelimitedVowels
		{
			get { return GetCommaDelimitedPhones(IPASymbolType.Vowel); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of phones in the cache of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string[] GetTypeOfPhones(IPASymbolType type)
		{
			return (from kvp in this
					where kvp.Value.CharType == type
					select kvp.Key).ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of phones in the form of a comma delimited list that are found in
		/// the cache and are of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetCommaDelimitedPhones(IPASymbolType type)
		{
			string[] phones = GetTypeOfPhones(type);

			StringBuilder bldr = new StringBuilder();
			foreach (string phone in phones)
			{
				bldr.Append(phone);
				bldr.Append(",");
			}

			// Get rid of the last comma.
			if (bldr.Length > 0)
				bldr.Length--;

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of phones having the specified feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCommaDelimitedPhonesInFeature(string featureName)
		{
			Debug.Assert(featureName != null);

			featureName = featureName.Trim().ToLower();
			featureName = featureName.Replace("[", string.Empty);
			featureName = featureName.Replace("]", string.Empty);

			if (featureName.Length == 0)
				return string.Empty;

			bool isBinary = (featureName.StartsWith("+") || featureName.StartsWith("-"));
			StringBuilder bldr = new StringBuilder();
			int  bit = -1;
			if (isBinary)
			{
				Feature bfeature = App.BFeatureCache[featureName];
				if (bfeature != null)
					bit = bfeature.Bit;
			}
			else
			{
				Feature afeature = App.AFeatureCache[featureName];
				if (afeature != null)
					bit = afeature.Bit;
			}

			foreach (KeyValuePair<string, IPhoneInfo> kvp in this)
			{
				if ((isBinary && kvp.Value.BMask[bit]) || (!isBinary && kvp.Value.AMask[bit]))
				{
					bldr.Append(kvp.Key);
					bldr.Append(',');
				}
			}

			// Get rid of the last comma.
			if (bldr.Length > 0)
				bldr.Length--;

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the IPACharInfo for the base character in the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPASymbol GetBaseCharInfoForPhone(string phone)
		{
			if (App.IPASymbolCache == null)
				return null;

			// First check if it's a tone letter since they don't have base characters.
			IPASymbol charInfo = App.IPASymbolCache.ToneLetterInfo(phone);
			if (charInfo != null)
				return charInfo;
		
			IPhoneInfo phoneInfo = this[phone];
			return (phoneInfo == null ? null : App.IPASymbolCache[phoneInfo.BaseCharacter]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the CVPatternInfoList for the phone cache. This list should be set to the
		/// list owned by a PA project when the project is opened.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<CVPatternInfo> CVPatternInfoList { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of phones whose features should be overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FeatureOverrides FeatureOverrides { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of ambiguous sequences used while adding phones to the cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AmbiguousSequences AmbiguousSequences { get; set; }
	}

	#endregion
}
