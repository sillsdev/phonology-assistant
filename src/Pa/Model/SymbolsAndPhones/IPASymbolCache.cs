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
// File: IPACharCache.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIL.Pa.Model
{
	#region IPASymbolTypeInfo struct
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Wraps the IPA symbol type and sub type into a single object for passing both pieces
	/// of information to methods and classes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public struct IPASymbolTypeInfo
	{
		public IPASymbolType Type;
		public IPASymbolSubType SubType;

		public IPASymbolTypeInfo(IPASymbolType type)
		{
			Type = type;
			SubType = IPASymbolSubType.notApplicable;
		}

		public IPASymbolTypeInfo(IPASymbolType type, IPASymbolSubType subType)
		{
			Type = type;
			SubType = subType;
		}
	}

	#endregion

	#region Enumerations
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// IPA symbol types.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[Flags]
	public enum IPASymbolType
	{
		notApplicable = 0,
		data = 1,
		pattern = 2,
		consonant = 4,
		vowel = 8,
		diacritic = 16,
		suprasegmental = 32,
		All = consonant | vowel | diacritic | suprasegmental,
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// IPA symbol Subtypes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[Flags]
	public enum IPASymbolSubType
	{
		notApplicable = 0,
		stress = 64,
		length = 128,
		boundary = 256,
		tone = 512,
		All = stress | length | boundary | tone,
	}

	///// ----------------------------------------------------------------------------------------
	///// <summary>
	///// Types used for grouping characters to ignore in find phone searching.
	///// </summary>
	///// ----------------------------------------------------------------------------------------
	//public enum IPASymbolIgnoreType
	//{
	//    NotApplicable = 0,
	//    StressSyllable = 1,
	//    Tone = 2,
	//    Length = 3
	//}
	
	#endregion

	#region IPASymbolCache class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// An in-memory cache of the IPASymbol table.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class IPASymbolCache : Dictionary<int, IPASymbol>
	{
		public enum SortType
		{
			MOArticulation,
			POArticulation,
			Unicode
		}

		//private const char kParseTokenMarker = '\u0001';
		//private readonly string m_ambigTokenFmt = kParseTokenMarker + "{0}";

		//private AmbiguousSequences m_sortedAmbiguousSeqList;
		//private AmbiguousSequences m_unsortedAmbiguousSeqList;

		//private readonly PaProject m_project;

		// This is a collection that is created every time the PhoneticParser method is
		// called and holds all the phones that have been parsed out of a transcription.
		//private List<string> m_phones;

		/// ------------------------------------------------------------------------------------
		static IPASymbolCache()
		{
			UncertainGroupAbsentPhoneChar = "\u2205";
			UncertainGroupAbsentPhoneChars = "0\u2205";
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFromList(List<IPASymbol> list)
		{
			if (list == null || list.Count == 0)
				return;
			
			Clear();

			if (ToneLetters != null)
				ToneLetters.Clear();

			// Copy the items from the list to the "real" cache.
			foreach (var info in list)
			{
				this[info.Decimal] = info;
				
				// If the code point is less than zero it means the character is
				// made up of multiple code points and is one of the tone letters.
				// In that case, make sure to add the character to the list of
				// tone letters.
				if (info.Decimal < 0)
				{
					if (ToneLetters == null)
						ToneLetters = new Dictionary<string, IPASymbol>();

					ToneLetters[info.Literal] = info;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a temporary (i.e. as long as this instance of PA is running) entry in the
		/// cache for the specified undefined phonetic character. Undefined means it cannot
		/// be found in the phonetic character inventory loaded from the XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddUndefinedCharacter(char c)
		{
			if (!ContainsKey(c))
			{
				var charInfo = new IPASymbol();
				charInfo.Literal = c.ToString();
				charInfo.HexCharCode = charInfo.Decimal.ToString("X4");
				charInfo.Type = IPASymbolType.notApplicable;
				charInfo.SubType = IPASymbolSubType.notApplicable;
				charInfo.IsBase = true;
				charInfo.IsUndefined = true;
				this[(int)c] = charInfo;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified phone is one of the tone letters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPASymbol ToneLetterInfo(string phone)
		{
			IPASymbol charInfo;
			return (ToneLetters != null && ToneLetters.TryGetValue(phone, out charInfo) ?
				charInfo : null);
		}

		/// ------------------------------------------------------------------------------------
		public void ClearUndefinedCharacterCollection()
		{
			UndefinedCharacters = new UndefinedPhoneticCharactersInfoList();
		}

		#region Indexer overloads
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the information for the IPA character specified by the codepoint.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new IPASymbol this[int codepoint]
		{
			get
			{
				IPASymbol charInfo;
				return (TryGetValue(codepoint, out charInfo) ? charInfo : null);
			}
			set { base[codepoint] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the IPA character information for the specified IPA character (in string
		/// form). This is mainly for the Chao tone letters which should be the only symbols
		/// that contain more than one codepoint.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPASymbol this[string ipaCharStr]
		{
			get
			{
				if (string.IsNullOrEmpty(ipaCharStr))
					return null;

				IPASymbol charInfo;
				if (ToneLetters != null && ToneLetters.TryGetValue(ipaCharStr, out charInfo))
					return charInfo;
				
				return Values.FirstOrDefault(symbolInfo => symbolInfo.Literal == ipaCharStr);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the IPA character information for the specified IPA character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPASymbol this[char ipaChar]
		{
			get	{return this[Convert.ToInt32(ipaChar)];}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public Dictionary<string, IPASymbol> ToneLetters { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The characters recognized by the program as those that, when included in an
		/// uncertain phone group, indicate the absence of a phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string UncertainGroupAbsentPhoneChars { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Character displayed in the phone information popup on C and V charts for sibling
		/// uncertainties that are not a phone (i.e. indicating the possible absence of a
		/// phone).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string UncertainGroupAbsentPhoneChar { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of code points found in the data but not found in the IPA
		/// character cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedPhoneticCharactersInfoList UndefinedCharacters { get; private set; }
	}

	#endregion
}
