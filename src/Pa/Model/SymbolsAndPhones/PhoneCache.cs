// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// ---------------------------------------------------------------------------------------------
// 
// File: PhoneCache.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SIL.Pa.Properties;

namespace SIL.Pa.Model
{
	#region PhoneCache class
	/// ----------------------------------------------------------------------------------------
	public class PhoneCache : Dictionary<string, IPhoneInfo>
	{
		public const string kDefaultChartSupraSegsToIgnore = "\u0300\u0301\u0302\u0304" +
			"\u0306\u030b\u030c\u030f\u1dc4\u1dc5\u1dc8\u203f";

		protected readonly string _conSymbol = "C";
		protected readonly string _vowSymbol = "V";

		protected readonly PaProject _project;

		/// ------------------------------------------------------------------------------------
		public PhoneCache(PaProject project) 
		{
			if (!string.IsNullOrEmpty(Settings.Default.ConsonantSymbol))
				_conSymbol = Settings.Default.ConsonantSymbol;

			if (!string.IsNullOrEmpty(Settings.Default.VowelSymbol))
				_vowSymbol = Settings.Default.VowelSymbol;

			_project = project;
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
				this[phone] = new PhoneInfo(_project.AmbiguousSequences, phone);
		}

		/// ------------------------------------------------------------------------------------
		public void AddUndefinedPhone(string phone)
		{
			IPhoneInfo phoneInfo;
			if (!TryGetValue(phone, out phoneInfo))
				this[phone] = new PhoneInfo(_project.AmbiguousSequences, phone, true);
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
			return GetCVPattern(_project.PhoneticParser.Parse(phonetic, true));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CV pattern for the specified phonetic string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCVPattern(string phonetic, bool convertExperimentalTranscriptions)
		{
			if (convertExperimentalTranscriptions)
				return GetCVPattern(_project.PhoneticParser.Parse(phonetic, true));

			Dictionary<int, string[]> uncertainPhones;
			var phones = _project.PhoneticParser.Parse(phonetic, true, false, out uncertainPhones);
			
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

			var bldr = new StringBuilder();
			foreach (var phone in phones)
			{
				var phoneInfo = this[phone] as PhoneInfo;

				if (_project.CVPatternInfoList.Any(cvpi => cvpi.Phone == phone))
					bldr.Append(phone);
				else if (phoneInfo == null || phoneInfo.IsUndefined)
				{
					var charInfo = App.IPASymbolCache[phone];
					if (charInfo != null)
					{
						switch (charInfo.Type)
						{
							case IPASymbolType.consonant: bldr.Append(_conSymbol); break;
							case IPASymbolType.vowel: bldr.Append(_vowSymbol); break;
							default: bldr.Append(charInfo.Literal); break;
						}
					}
				}
				//else if (phoneInfo.CharType == IPASymbolType.boundary)
				//    bldr.Append(' ');
				else if (phoneInfo.CharType == IPASymbolType.consonant ||
				   phoneInfo.CharType == IPASymbolType.vowel)
				{
					string diacriticsAfterBase = null;

					if (phone.Length > 1)
						diacriticsAfterBase = GetMatchingModifiers(phone, bldr);

					bldr.Append(phoneInfo.CharType == IPASymbolType.consonant ?
						_conSymbol : _vowSymbol);

					if (diacriticsAfterBase != null)
						bldr.Append(diacriticsAfterBase);
				}
			}

			return bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks the specified phone for any modifying diacritics found in any of the items
		/// in the project's CVPatternInfoList.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetMatchingModifiers(string phone, StringBuilder bldr)
		{
			if (_project.CVPatternInfoList == null)
				return null;

			// Find out which codepoint in the phone represents the base character. For
			// tie-barred phones, just use the first of the two base characters.
			int baseIndex = -1;
			for (int i = 0; i < phone.Length; i++)
			{
				var charInfo = App.IPASymbolCache[phone[i]];
				if (charInfo != null && charInfo.IsBase)
				{
					baseIndex = i;
					break;
				}
			}

			// This should never happen, but if the phone has no base character,
			// what more can we do. 
			if (baseIndex < 0)
				return null;

			// Get the pieces of the phone that are before and after the base character.
			string preBase = (baseIndex == 0 ? string.Empty : phone.Substring(0, baseIndex));
			string postBase = (baseIndex == phone.Length - 1 ? string.Empty : phone.Substring(baseIndex + 1));

			foreach (var diacritic in preBase)
			{
				if (_project.CVPatternInfoList.Any(cvpi => cvpi.GetIsLeftSideDiacritic(diacritic)))
					bldr.Append(diacritic);
			}

			var diacriticsAfterBase = new StringBuilder();
			foreach (var diacritic in postBase)
			{
				if (_project.CVPatternInfoList.Any(cvpi => cvpi.GetIsRightSideDiacritic(diacritic)))
					diacriticsAfterBase.Append(diacritic);
			}

			return (diacriticsAfterBase.Length == 0 ? null : diacriticsAfterBase.ToString());
		}

		/// ------------------------------------------------------------------------------------
		public string[] Consonants
		{
			get {return GetPhonesHavingType(IPASymbolType.consonant);}
		}

		/// ------------------------------------------------------------------------------------
		public string CommaDelimitedConsonants
		{
			get {return GetCommaDelimitedPhones(IPASymbolType.consonant);}
		}

		/// ------------------------------------------------------------------------------------
		public string[] Vowels
		{
			get { return GetPhonesHavingType(IPASymbolType.vowel); }
		}

		/// ------------------------------------------------------------------------------------
		public string CommaDelimitedVowels
		{
			get { return GetCommaDelimitedPhones(IPASymbolType.vowel); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of phones in the cache of the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] GetPhonesHavingType(IPASymbolType type)
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
		public string GetCommaDelimitedPhones(IPASymbolType type)
		{
			var phones = GetPhonesHavingType(type);

			var bldr = new StringBuilder();
			foreach (var phone in phones)
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
		/// Gets the IPACharInfo for the base character in the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPASymbol GetBaseCharInfoForPhone(string phone)
		{
			// First check if it's a tone letter since they don't have base characters.
			var charInfo = App.IPASymbolCache.ToneLetterInfo(phone);
			if (charInfo != null)
				return charInfo;
		
			var phoneInfo = this[phone];
			return (phoneInfo == null ? null : App.IPASymbolCache[phoneInfo.BaseCharacter]);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesContainPhone(string phone, string ignoredChars, bool ignoreDiacritics)
		{
			if (ContainsKey(phone))
				return true;

			ignoredChars = ignoredChars ?? string.Empty;
			if (ignoredChars.Length == 0 && !ignoreDiacritics)
				return false;

			var ph = PhoneWithoutIgnoredStuff(phone, ignoredChars, ignoreDiacritics);

			return Values.Select(phoneInfo => phoneInfo.Phone)
				.Any(p => ph == PhoneWithoutIgnoredStuff(p, ignoredChars, ignoreDiacritics));
		}

		/// ------------------------------------------------------------------------------------
		public string PhoneWithoutIgnoredStuff(string phone, string ignoredChars, bool ignoreDiacritics)
		{
			var ph = ignoredChars.Aggregate(phone, (curr, ignoredChr) =>
				curr.Replace(ignoredChr.ToString(CultureInfo.InvariantCulture), string.Empty));

			return App.IPASymbolCache.Values.Where(s => s.Type == IPASymbolType.diacritic)
				.Select(s => s.Literal).Aggregate(ph, (curr, diacritic) => curr.Replace(diacritic, string.Empty));
		}
	}

	#endregion
}
