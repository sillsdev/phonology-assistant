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
using System.Text;
using SIL.Pa.PhoneticSearching;

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
			SubType = IPASymbolSubType.Unknown;
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
	public enum IPASymbolType
	{
		Unknown = 0,
		Consonant = 1,
		Vowel = 2,
		Suprasegmentals = 3,
		Diacritics = 4,
		Breaking
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// IPA symbol Subtypes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum IPASymbolSubType
	{
		Unknown = 0,
		Pulmonic = 1,
		NonPulmonic = 2,
		OtherSymbols = 3,
		StressAndLength = 4,
		ToneAndAccents = 5
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Types used for grouping characters to ignore in find phone searching.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum IPASymbolIgnoreType
	{
		NotApplicable = 0,
		StressSyllable = 1,
		Tone = 2,
		Length = 3
	}
	
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

		private static string s_currentPhoneticBeingParsed;

		private const char kParseTokenMarker = '\u0001';
		private readonly string m_ambigTokenFmt = kParseTokenMarker + "{0}";

		private AmbiguousSequences m_sortedAmbiguousSeqList;
		private AmbiguousSequences m_unsortedAmbiguousSeqList;
		private UndefinedPhoneticCharactersInfoList m_undefinedChars;

		private Dictionary<string, IPASymbol> m_toneLetters;
		private bool m_logUndefinedCharacters;

		// This is a collection that is created every time the PhoneticParser method is
		// called and holds all the phones that have been parsed out of a transcription.
		private List<string> m_phones;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal IPASymbolCache()
		{
			TranscriptionChanges = new TranscriptionChanges();
			m_unsortedAmbiguousSeqList = new AmbiguousSequences();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static IPASymbolCache()
		{
			UncertainGroupAbsentPhoneChar = "\u2205";
			UncertainGroupAbsentPhoneChars = "0\u2205";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AmbiguousSequences AmbiguousSequences
		{
			get { return m_unsortedAmbiguousSeqList; }
			set
			{
				m_unsortedAmbiguousSeqList = value;
				BuildSortedAmbiguousSequencesList();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will build a list of ambiguous sequences that's in order of their length and
		/// will include the tone letters as well. The order is longest to shortest with those
		/// with the same lengths, staying in the order in which the user entered them in the
		/// Phone Inventory view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildSortedAmbiguousSequencesList()
		{
			m_sortedAmbiguousSeqList = new AmbiguousSequences();

			// Copy the references from the specified list to our own.
			if (m_unsortedAmbiguousSeqList != null)
			{
				foreach (AmbiguousSeq ambiguousSeq in m_unsortedAmbiguousSeqList)
					m_sortedAmbiguousSeqList.Add(ambiguousSeq);
			}

			/// Go through the tone letters collection and add them to the ambiguous list.
			/// Tone letters are special in that they're the only IPA characters in the
			/// cache that are made up of multiple code points. When it comes to parsing a
			/// phonetic string into its phones, tone letters need to be treated as
			/// ambiguous sequences.
			if (m_toneLetters != null)
			{
				foreach (IPASymbol info in m_toneLetters.Values)
				{
					if (!m_sortedAmbiguousSeqList.ContainsSeq(info.Literal, true))
						m_sortedAmbiguousSeqList.Add(new AmbiguousSeq(info.Literal));
				}
			}

			// Now order the items in the list based on the length
			// of the ambiguous sequence -- longest to shortest.
			m_sortedAmbiguousSeqList.SortByUnitLength();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TranscriptionChanges TranscriptionChanges { get; set; }

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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadFromList(List<IPASymbol> list)
		{
			if (list == null || list.Count == 0)
				return;
			
			Clear();

			if (m_toneLetters != null)
				m_toneLetters.Clear();

			// Copy the items from the list to the "real" cache.
			foreach (IPASymbol info in list)
			{
				this[info.Decimal] = info;
				
				// If the code point is less than zero it means the character is
				// made up of multiple code points and is one of the tone letters.
				// In that case, make sure to add the character to the list of
				// tone letters.
				if (info.Decimal < 0)
				{
					if (m_toneLetters == null)
						m_toneLetters = new Dictionary<string, IPASymbol>();

					m_toneLetters[info.Literal] = info;
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
				IPASymbol charInfo = new IPASymbol();
				charInfo.Literal = c.ToString();
				charInfo.Decimal = c;
				charInfo.Hexadecimal = charInfo.Decimal.ToString("X4");
				charInfo.Type = IPASymbolType.Unknown;
				charInfo.SubType = IPASymbolSubType.Unknown;
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
			return (m_toneLetters != null && m_toneLetters.TryGetValue(phone, out charInfo) ?
				charInfo : null);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a list of references to cache items, sorted by the specified sort type.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public SortedList<int, IPASymbol> GetSortedReferenceList(SortType sortType)
		//{
		//    SortedList<int, IPASymbol> list = new SortedList<int, IPASymbol>();

		//    foreach (IPASymbol info in Values)
		//    {
		//        switch (sortType)
		//        {
		//            case SortType.MOArticulation: list[info.MOArticulation] = info; break;
		//            case SortType.POArticulation: list[info.POArticulation] = info; break;
		//            case SortType.Unicode: list[info.Decimal] = info; break;
		//        }
		//    }

		//    return list;
		//}

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
			set {base[codepoint] = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the IPA character information for the specified IPA character (in string
		/// form). If the string contains more than one codepoint, information for the
		/// first codepoint is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPASymbol this[string ipaCharStr]
		{
			get
			{
				if (string.IsNullOrEmpty(ipaCharStr))
					return null;

				IPASymbol charInfo;
				if (m_toneLetters != null && m_toneLetters.TryGetValue(ipaCharStr, out charInfo))
					return charInfo;
				
				return this[ipaCharStr[0]];
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of code points found in the data but not found in the IPA
		/// character cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedPhoneticCharactersInfoList UndefinedCharacters
		{
			get { return m_undefinedChars; }
			set { m_undefinedChars = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the PhoneticParse method should
		/// add to the UndefinedCharacters collection any characters it runs across that
		/// aren't found in the phonetic character inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool LogUndefinedCharactersWhenParsing
		{
			get { return m_logUndefinedCharacters; }
			set { m_logUndefinedCharacters = value; }
		}

		#endregion

		#region Ambiguous Sequence Finding
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scans the phonetic transcription, checking for possible ambiguous sequences at the
		/// beginning of each "word" within the transcription. These are the only ambiguous
		/// sequences the program will find automatically.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> FindAmbiguousSequences(string phonetic)
		{
			// Return an empty array if there's nothing in the phonetic.
			if (string.IsNullOrEmpty(phonetic))
				return null;

			phonetic = FFNormalizer.Normalize(phonetic);
			phonetic = TranscriptionChanges.Convert(phonetic);
			string[] words = phonetic.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			StringBuilder bldr = new StringBuilder();
			List<string> ambigSeqs = new List<string>();

			foreach (string word in words)
			{
				if (word.Length == 0)
					continue;

				for (int i = 0; i < word.Length; i++)
				{
					IPASymbol ci = this[word[i]];

					if (ci == null || ci.IsBase || ci.IsUndefined)
					{
						// If there's already something in the builder it means
						// we've previously found some non base characters before
						// the current one that we assume belong to the current one.
						if (bldr.Length > 0)
							bldr.Append(word[i]);

						break;
					}

					bldr.Append(word[i]);
				}

				if (bldr.Length > 0)
				{
					ambigSeqs.Add(bldr.ToString());
					bldr.Length = 0;
				}
			}

			return (ambigSeqs.Count > 0 ? ambigSeqs : null);
		}

		#endregion

		#region Phonetic string parser
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into a new string of phones delimited
		/// by commas.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PhoneticParser_CommaDelimited(string phonetic, bool normalize,
			bool removeDuplicates)
		{
			Dictionary<int, string[]> uncertainPhones;
			string[] phones = PhoneticParser(phonetic, normalize, out uncertainPhones);

			if (phones == null)
				return null;

			StringBuilder commaDelimitedPhones = new StringBuilder();
			for (int i = 0; i < phones.Length; i++)
			{
				if (!removeDuplicates || commaDelimitedPhones.ToString().IndexOf(phones[i]) < 0)
				{
					commaDelimitedPhones.Append(phones[i]);
					commaDelimitedPhones.Append(',');
				}
			}

			return commaDelimitedPhones.ToString().TrimEnd((", ").ToCharArray());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] PhoneticParser(string phonetic, bool normalize)
		{
			Dictionary<int, string[]> uncertainPhones;
			return PhoneticParser(phonetic, normalize, out uncertainPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] PhoneticParser(string phonetic, bool normalize,
			bool convertExperimentalTranscriptions)
		{
			Dictionary<int, string[]> uncertainPhones;
			return PhoneticParser(phonetic, normalize, convertExperimentalTranscriptions,
				out uncertainPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] PhoneticParser(string phonetic, bool normalize,
			out Dictionary<int, string[]> uncertainPhones)
		{
			return PhoneticParser(phonetic, normalize, true, out uncertainPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] PhoneticParser(string phonetic, bool normalize,
			bool convertExperimentalTranscriptions, out Dictionary<int, string[]> uncertainPhones)
		{
			uncertainPhones = null;

			// Return an empty array if there's nothing in the phonetic.
			if (string.IsNullOrEmpty(phonetic))
				return null;

			m_phones = new List<string>(phonetic.Length);
			IPASymbol ciPrev = null;

			// Normalize the string if necessary.
			if (normalize)
			    phonetic = FFNormalizer.Normalize(phonetic);

			s_currentPhoneticBeingParsed = phonetic;

			if (convertExperimentalTranscriptions)
				phonetic = TranscriptionChanges.Convert(phonetic);
	
			phonetic = MarkAmbiguousSequences(phonetic);

			int phoneStart = 0;
			for (int i = 0; i < phonetic.Length; i++)
			{
				char c = phonetic[i];
				char badChar = '\0';

				// Check if we've run into a marker indicating
				// the beginning of an ambiguous sequence.
				if (c == kParseTokenMarker)
				{
					// First, close the previous phone if there is one.
					if (i > phoneStart)
						m_phones.Add(phonetic.Substring(phoneStart, i - phoneStart));

					string ambigPhone =
						m_sortedAmbiguousSeqList.GetAmbigSeqForToken(phonetic[++i]);

					if (!string.IsNullOrEmpty(ambigPhone))
						m_phones.Add(ambigPhone);

					phoneStart = i + 1;
					continue;
				}

				// Get the information for the current codepoint.
				IPASymbol ciCurr = this[c];

				// If there's no information for a code point or there is but there isn't
				// any for the previous character and the current character isn't a base
				// character, then treat the character as it's own phone.
				if (ciCurr == null || ciCurr.Type == IPASymbolType.Unknown)
				{
					if (i > phoneStart)
						m_phones.Add(phonetic.Substring(phoneStart, i - phoneStart));

					// Check if we're at the beginning of an uncertain phone group
					if (c != '(')
					{
						phoneStart = i + 1;
						badChar = c;
					}
					else
					{
						int index = i + 1;
						string primaryPhone = GetUncertainties(phonetic, ref index,
							m_phones.Count, ref uncertainPhones);

						// Primary phone should only be null when no slash was found
						// between the parentheses. In that situation, the parentheses are
						// not considered to be surrounding a group of uncertain phones.
						if (primaryPhone == null)
							badChar = c;
						else
						{
							m_phones.Add(primaryPhone);
							i = index;
						}

						phoneStart = i + 1;
					}

					ciPrev = null;

					if (badChar != '\0')
					{
						// Log the undefined character.
						if (m_logUndefinedCharacters && m_undefinedChars != null)
							m_undefinedChars.Add(c, s_currentPhoneticBeingParsed);

						m_phones.Add(c.ToString());
					}

					continue;
				}

				// If we've encountered a non base character but nothing precedes it,
				// then it must be a diacritic at the beginning of the phonetic
				// transcription so just put it with the following characters.
				if (ciPrev == null && !ciCurr.IsBase)
					continue;

				// Is the previous codepoint special in that it's not a base character
				// but a base character must follow it in the same phone (e.g. a tie bar)?
				// If yes, then make sure the current codepoint is a base character or
				// throw it away.
				if (ciPrev != null && ciPrev.CanPrecedeBase)
				{
					ciPrev = ciCurr;
					continue;
				}

				// At this point, if the current codepoint is a base character and
				// it's not the first in the string, close the previous phone. If
				// ciCurr.IsBase && i > phoneStart but ciPrev == null then it means
				// we've run across some non base characters at the beginning of the
				// transcription that aren't attached to a base character. Therefore,
				// attach them to the first base character that's found. In that case,
				// we don't want to add the phone to the collection yet. We'll wait
				// until we come across the beginning of the next phone.
				if (ciCurr.IsBase && i > phoneStart && ciPrev != null)
				{
					m_phones.Add(phonetic.Substring(phoneStart, i - phoneStart));
					phoneStart = i;
				}

				ciPrev = ciCurr;
			}

			// Save the last phone
			if (phoneStart < phonetic.Length)
				m_phones.Add(phonetic.Substring(phoneStart));

			return m_phones.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method marks ambiguous sequences in the specified phonetic string.
		/// 
		/// The process of marking ambiguous sequences in a phonetic string involves replacing
		/// a series of characters that is recognied as an ambiguous sequence with two
		/// characters. The first character is always the same and informs the parsing process
		/// that what follows is an ambiguous sequence token. The second character is the
		/// token. Tokens uniquely identify the sequence of characters being replaced.
		/// 
		/// After all ambiguous sequences in a phonetic string are marked, the process of
		/// parsing a phonetic string into phones will find the tokens, adding the phones they
		/// represent to the collection of phones being built for the phonetic string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string MarkAmbiguousSequences(string phonetic)
		{
			if (m_sortedAmbiguousSeqList == null && m_unsortedAmbiguousSeqList != null)
				BuildSortedAmbiguousSequencesList();
			
			if (m_sortedAmbiguousSeqList != null)
			{
				foreach (AmbiguousSeq ambigSeq in m_sortedAmbiguousSeqList)
				{
					if (ambigSeq.Convert)
					{
						phonetic = phonetic.Replace(ambigSeq.Literal,
							string.Format(m_ambigTokenFmt, ambigSeq.ParseToken));
					}
				}
			}

			return phonetic;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a list of uncertain phones and returns them in a collection.
		/// </summary>
		/// <param name="phonetic">Phonetic word containing uncertainties.</param>
		/// <param name="i">Starts out as the index of the charcter just after the open
		/// parenthesis. By the time the method returns, i is the index of the closed
		/// parenthesis.</param>
		/// <param name="phoneNumber">The index into the array of phones of the phone for
		/// which the uncertainties are possibilities.</param>
		/// <param name="uncertainPhones">Collection of uncertain phones found between
		/// the parenthese.</param>
		/// <returns>The first phone in the list of uncertain phones. It is treated
		/// as the primary (i.e. most likely candidate) phone.</returns>
		/// ------------------------------------------------------------------------------------
		private string GetUncertainties(string phonetic, ref int i, int phoneNumber,
			ref Dictionary<int, string[]> uncertainPhones)
		{
			if (uncertainPhones == null)
				uncertainPhones = new Dictionary<int, string[]>();

			int savCharPointer = i;
			string tmpPhone;
			StringBuilder bldrPhone = new StringBuilder();
			List<string> phones = new List<string>();

			for (; i < phonetic.Length && phonetic[i] != ')'; i++)
			{
				if (phonetic[i] != '/' && phonetic[i] != ',' && phonetic[i] != kParseTokenMarker)
				{
					bldrPhone.Append(phonetic[i]);
					continue;
				}

				if (phonetic[i] != kParseTokenMarker)
				{
					// Save the previous phone, if there is one, checking if the phone is the
					// empty set character. If it's the empty set, then just save an empty string.
					tmpPhone = bldrPhone.ToString();
					if (UncertainGroupAbsentPhoneChars.Contains(tmpPhone))
						tmpPhone = string.Empty;

					phones.Add(tmpPhone);
					bldrPhone.Length = 0;
					continue;
				}

				// Ambiguous sequences in uncertain groups are actually redundant since
				// the slashes and the final close parenthesis should mark sequences that
				// are to be kept together as phones. But, we have to check for them...
				// just in case.
				string ambigPhone =
					m_sortedAmbiguousSeqList.GetAmbigSeqForToken(phonetic[i + 1]);

				if (!string.IsNullOrEmpty(ambigPhone))
				{
					// Replace the token marker and the token with the ambiguous sequence.
					string dualCharMarker =
						string.Format(m_ambigTokenFmt, phonetic[i + 1]);

					phonetic = phonetic.Replace(dualCharMarker, ambigPhone);
					bldrPhone.Append(phonetic[i]);
				}
			}

			// If we reached the end of the string it means we didn't
			// find the closed parenthesis, which is an error condition.
			if (i == phonetic.Length)
			{
				// TODO: Log error.
				return null;
			}

			// When the following is true, it's a special case (i.e. when a
			// parentheses group was found but didn't contain any slashes).
			// If that happens, then assume it isn't a uncertain group.
			if (phones.Count == 0)
			{
				i = savCharPointer;
				return null;
			}

			// REVIEW: Should the code points in the uncertain phones be validated
			// against those in the IPA cache?

			// If the phone is the empty set character just save an empty string.
			tmpPhone = bldrPhone.ToString();
			if (UncertainGroupAbsentPhoneChars.Contains(tmpPhone))
				tmpPhone = string.Empty;

			// Save the last uncertain phone and add the list of uncertain phones to
			// the collection of those for the word
			phones.Add(tmpPhone);
			uncertainPhones[phoneNumber] = phones.ToArray();

			if (m_undefinedChars != null)
				ValidateCodepointsInUncertainPhones(phones);

			return phones[0];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks all the Unicode values in the list of uncertain phones to make sure they
		/// are in PA's character code inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ValidateCodepointsInUncertainPhones(IEnumerable<string> phones)
		{
			foreach (string phone in phones)
			{
				foreach (char c in phone)
				{
					// Get the information for the current codepoint.
					if (!ContainsKey(c))
						m_undefinedChars.Add(c, s_currentPhoneticBeingParsed);
				}
			}
		}

		#endregion
	}

	#endregion
}
