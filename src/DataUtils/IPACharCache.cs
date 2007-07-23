using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Wraps the IPA character type and sub type into a single object for passing both pieces
	/// of information to methods and classes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public struct IPACharacterTypeInfo
	{
		public IPACharacterType Type;
		public IPACharacterSubType SubType;

		public IPACharacterTypeInfo(IPACharacterType type)
		{
			Type = type;
			SubType = IPACharacterSubType.Unknown;
		}

		public IPACharacterTypeInfo(IPACharacterType type, IPACharacterSubType subType)
		{
			Type = type;
			SubType = subType;
		}
	}

	#region Enumerations
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// IPA phone types.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum IPACharacterType : int
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
	/// IPA phone Subtypes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum IPACharacterSubType : int
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
	public enum IPACharIgnoreTypes : int
	{
		NotApplicable = 0,
		StressSyllable = 1,
		Tone = 2,
		Length = 3
	}
	
	#endregion

	#region UndefinedPhoneticCharactersInfoList and UndefinedPhoneticCharactersInfo classes
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class UndefinedPhoneticCharactersInfoList : List<UndefinedPhoneticCharactersInfo>
	{
		public string CurrentDataSourceName = null;
		public string CurrentReference = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an individual code point that is not defined in the IPA character cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(char c, string transcription)
		{
			if (!string.IsNullOrEmpty(transcription))
			{
				UndefinedPhoneticCharactersInfo ucpInfo = new UndefinedPhoneticCharactersInfo();
				Add(ucpInfo);
				ucpInfo.Character = c;
				ucpInfo.SourceName = CurrentDataSourceName;
				ucpInfo.Reference = CurrentReference;
				ucpInfo.Transcription =
					transcription.Replace(IPACharCache.kForcedPhoneDelimiterStr, string.Empty);
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Contains information about a code point read from a data source that cannot be found
	/// in the IPA character cache.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class UndefinedPhoneticCharactersInfo
	{
		public char Character;
		public string Transcription;
		public string SourceName;
		public string Reference;
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// An in-memory cache of the IPACharacters table. As of Feb. 13, 2007 the file from which
	/// this class reads its data is called PhoneticInventory.xml. I would change the name
	/// of the class but I don't want to go to the trouble now. Perhaps some day. -DDO
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class IPACharCache : Dictionary<int, IPACharInfo>
	{
		public const string kBreakChars = " ";

		public enum SortType
		{
			MOArticulation,
			POArticulation,
			Unicode
		}

		private const string kEmptySetChars = "0\u2205";

		// Use object replacement character.
		internal const string kForcedPhoneDelimiterStr = "\uFFFC";
		private const char kForcedPhoneDelimiter = '\uFFFC';
		
		private ExperimentalTranscriptions m_experimentalTransList = null;
		private AmbiguousSequences m_sortedAmbiguousSeqList = null;
		private AmbiguousSequences m_unsortedAmbiguousSeqList = null;
		private UndefinedPhoneticCharactersInfoList m_undefinedCharacters;

		private static readonly string s_forcedPhoneDelimiterFmt =
			kForcedPhoneDelimiterStr + "{0}" + kForcedPhoneDelimiterStr;

		public const string kDefaultIPACharCacheFile = "PhoneticCharacterInventory.xml";
		public const string kIPACharCacheFile = "PhoneticCharacterInventory.xml";
		private string m_cacheFileName = null;
		private Dictionary<string, IPACharInfo> m_toneLetters = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal IPACharCache(string projectFileName)
		{
			m_experimentalTransList = new ExperimentalTranscriptions();
			m_unsortedAmbiguousSeqList = new AmbiguousSequences();
			m_cacheFileName = BuildFileName(projectFileName, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the name from which to load or save the cache file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string BuildFileName(string projectFileName, bool mustExist)
		{
			string filename = (projectFileName ?? string.Empty);
			filename += (filename.EndsWith(".") ? string.Empty : ".") + kIPACharCacheFile;

			// Uncomment if phonetic inventories can exist at the project level.
			//if (!File.Exists(filename) && mustExist)
				filename = kDefaultIPACharCacheFile;

			return filename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CacheFileName
		{
			get { return m_cacheFileName; }
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
			if (m_sortedAmbiguousSeqList != null && m_toneLetters != null)
			{
				foreach (IPACharInfo info in m_toneLetters.Values)
				{
					if (!m_sortedAmbiguousSeqList.ContainsSeq(info.IPAChar, true))
						m_sortedAmbiguousSeqList.Add(new AmbiguousSeq(info.IPAChar));
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
		public ExperimentalTranscriptions ExperimentalTranscriptions
		{
			get { return m_experimentalTransList; }
			set { m_experimentalTransList = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the binary feature table from the database into a memory cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IPACharCache Load()
		{
			return Load(null);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the IPA character cache file into a memory cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IPACharCache Load(string projectFileName)
		{
			IPACharCache cache = new IPACharCache(projectFileName);
			
			// Deserialize into a List<T> because a Dictionary<TKey, TValue>
			// (i.e. IPACharCache) isn't serializable nor deserializable.
			List<IPACharInfo> tmpCache = STUtils.DeserializeData(
				cache.CacheFileName, typeof(List<IPACharInfo>)) as List<IPACharInfo>;

			if (tmpCache == null)
				return null;

			cache.LoadFromList(tmpCache);
			tmpCache.Clear();

			// This should never return null.
			return (cache.Count == 0 ? null : cache);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the cache to project-specific XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string projectFileName)
		{
			m_cacheFileName = BuildFileName(projectFileName, false);
			Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the cache to it's XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			// Copy the items into a temp list because Dictionaries cannot be serialized.
			List<IPACharInfo> tmpCache = ToList(false);
			STUtils.SerializeData(m_cacheFileName, tmpCache);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadFromList(List<IPACharInfo> tmpCache)
		{
			if (tmpCache == null || tmpCache.Count == 0)
				return;
			
			Clear();

			if (m_toneLetters != null)
				m_toneLetters.Clear();

			// Copy the items from the list to the "real" cache.
			foreach (IPACharInfo info in tmpCache)
			{
				this[info.Codepoint] = info;
				
				// If the code point is less than zero it means the character is
				// made up of multiple code points and is one of the tone letters.
				// In that case, make sure to add the character to the list of
				// tone letters.
				if (info.Codepoint < 0)
				{
					if (m_toneLetters == null)
						m_toneLetters = new Dictionary<string, IPACharInfo>();

					m_toneLetters[info.IPAChar] = info;
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
				IPACharInfo charInfo = new IPACharInfo();
				charInfo.IPAChar = c.ToString();
				charInfo.Codepoint = c;
				charInfo.HexIPAChar = charInfo.Codepoint.ToString("X4");
				charInfo.CharType = IPACharacterType.Unknown;
				charInfo.CharSubType = IPACharacterSubType.Unknown;
				charInfo.IsBaseChar = true;
				charInfo.IsUndefined = true;
				this[(int)c] = charInfo;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified phone is one of the tone letters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharInfo ToneLetterInfo(string phone)
		{
			IPACharInfo charInfo;
			return (m_toneLetters != null && m_toneLetters.TryGetValue(phone, out charInfo) ?
				charInfo : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the cache in a collection of IPACharInfo objects in the form of a generic
		/// List (i.e. List<IPACharInfo>).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<IPACharInfo> ToList()
		{
			return ToList(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the cache in a collection of IPACharInfo objects in the form of a generic
		/// List (i.e. List<IPACharInfo>).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<IPACharInfo> ToList(bool includeUndefined)
		{
			List<IPACharInfo> tmpCache = new List<IPACharInfo>();
			foreach (KeyValuePair<int, IPACharInfo> info in this)
			{
				if (!info.Value.IsUndefined || includeUndefined)
					tmpCache.Add(info.Value);
			}

			return tmpCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of references to cache items, sorted by the specified sort type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortedList<int, IPACharInfo> GetSortedReferenceList(SortType sortType)
		{
			SortedList<int, IPACharInfo> list = new SortedList<int, IPACharInfo>();

			foreach (IPACharInfo info in Values)
			{
				switch (sortType)
				{
					case SortType.MOArticulation: list[info.MOArticulation] = info; break;
					case SortType.POArticulation: list[info.POArticulation] = info; break;
					case SortType.Unicode: list[info.Codepoint] = info; break;
				}
			}

			return list;
		}

		#region Indexer overloads
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the information for the IPA character specified by the codepoint.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new IPACharInfo this[int codepoint]
		{
			get
			{
				IPACharInfo charInfo;
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
		public IPACharInfo this[string ipaCharStr]
		{
			get
			{
				IPACharInfo charInfo;
				if (m_toneLetters != null && m_toneLetters.TryGetValue(ipaCharStr, out charInfo))
					return charInfo;
				
				return (ipaCharStr == null || ipaCharStr.Length < 1 ? null : this[ipaCharStr[0]]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the IPA character information for the specified IPA character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharInfo this[char ipaChar]
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
			get { return m_undefinedCharacters; }
			set { m_undefinedCharacters = value; }
		}

		#endregion

		#region Phonetic string parser
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into a new string of phones delimited
		/// by commas.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PhoneticParser_CommaDelimited(string phonetic, bool normalize)
		{
			Dictionary<int, string[]> uncertainPhones;
			string[] phones = PhoneticParser(phonetic, normalize, out uncertainPhones);

			if (phones == null)
				return null;

			StringBuilder commaDelimitedPhones = new StringBuilder();
			for (int i = 0; i < phones.Length; i++)
			{
				commaDelimitedPhones.Append(phones[i]);
				if (i < phones.Length - 1)
					commaDelimitedPhones.Append(',');
			}

			return commaDelimitedPhones.ToString();
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
		/// Parses the specified phonetic string into a new string of phones delimited
		/// by commas.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] PhoneticParser(string phonetic, bool normalize,
			out Dictionary<int, string[]> uncertainPhones)
		{
			// This flag gets set when we encounter a phonetic string that begins with a non
			// base character (e.g. prenasalized 'd'). When the end of the phone beginning
			// the string is found, the phone is added to the ambiguous sequences list.
			bool addPhoneToAmbiguousSeqList = false;

			uncertainPhones = null;

			// Return an empty array if there's nothing in the phonetic.
			if (string.IsNullOrEmpty(phonetic))
				return null;

			List<string> phones = new List<string>();
			IPACharInfo ciPrev = null;

			// Normalize the string if necessary.
			if (normalize)
			    phonetic = FFNormalizer.Normalize(phonetic);

			phonetic = m_experimentalTransList.Convert(phonetic);
			phonetic = DelimitAmbiguousSequences(phonetic);

			int phoneStart = 0;
			for (int i = 0; i < phonetic.Length; i++)
			{
				char c = phonetic[i];
				char badChar = '\0';

				// Check if we've run into a marker indicating the beginning of
				// an ambiguous sequence or experimental phone transcription.
				if (c == kForcedPhoneDelimiter)
				{
					// First, close the previous phone if there is one.
					if (i > phoneStart)
						phones.Add(phonetic.Substring(phoneStart, i - phoneStart));

					AddPhoneFromBetweenDelimiters(phonetic, phones, ref i);
					phoneStart = i + 1;
					continue;
				}

				// Get the information for the current codepoint.
				IPACharInfo ciCurr = this[c];

				// If there's no information for a code point or there is but there isn't
				// any for the previous character and the current character isn't a base
				// character, then treat the character as it's own phone.
				if (ciCurr == null || ciCurr.CharType == IPACharacterType.Unknown)
				{
					if (i > phoneStart)
						phones.Add(phonetic.Substring(phoneStart, i - phoneStart));

					if (c != '(')
					{
						phoneStart = i + 1;
						badChar = c;
					}
					else
					{
						int index = i + 1;
						string primaryPhone = GetUncertainties(phonetic, ref index,
							phones.Count, ref uncertainPhones);

						// Primary phone should only be null when no slash was found
						// between the parentheses. In that situation, the parentheses are
						// not considered to be surrounding a group of uncertain phones.
						if (primaryPhone == null)
						{
							phoneStart = i + 1;
							badChar = c;
						}
						else
						{
							phones.Add(primaryPhone);
							phoneStart = index + 1;
							i = index;
						}
					}

					ciPrev = null;

					// Log the undefined character.
					if (badChar != '\0' && m_undefinedCharacters != null)
					{
						m_undefinedCharacters.Add(c, phonetic);

						// Add the undefined phonetic character to the list of phones. Later,
						// the character will be added to the IPA character cache.
						phones.Add(c.ToString());
					}

					continue;
				}

				// If we've encountered a non base character but nothing precedes it,
				// then it must be a diacritic at the beginning of the phonetic
				// transcription so just put it with the following characters.
				if (ciPrev == null && ciCurr != null && !ciCurr.IsBaseChar)
				{
					addPhoneToAmbiguousSeqList = true;
					continue;
				}

				// Is the previous codepoint special in that it's not a base character
				// but a base character must follow it in the same phone (e.g. a tie bar)?
				// If yes, then make sure the current codepoint is a base character or
				// throw it away.
				if (ciPrev != null && ciPrev.CanPreceedBaseChar)
				{
					ciPrev = ciCurr;
					continue;
				}

				// At this point, if the current codepoint is a base character and
				// it's not the first in the string, close the previous phone. If
				// ciCurr.IsBaseChar && i > phoneStart but ciPrev == null then it means
				// we've run across some non base characters at the beginning of the
				// transcription that aren't attached to a base character. Therefore,
				// attach them to the first base character that's found. In that case,
				// we don't want to add the phone to the collection yet. We'll wait
				// until we come across the beginning of the next phone.
				if (ciCurr.IsBaseChar && i > phoneStart && ciPrev != null)
				{
					string phone = phonetic.Substring(phoneStart, i - phoneStart);
					phones.Add(phone);
					phoneStart = i;

					if (addPhoneToAmbiguousSeqList)
					{
						AmbiguousSeq seq = new AmbiguousSeq(phone);
						seq.Convert = true;
						seq.IsProjectDefault = true;

						if (m_unsortedAmbiguousSeqList == null)
							m_unsortedAmbiguousSeqList = new AmbiguousSequences();

						m_unsortedAmbiguousSeqList.Add(seq);
						BuildSortedAmbiguousSequencesList();
						addPhoneToAmbiguousSeqList = false;
					}
				}

				ciPrev = ciCurr;
			}

			// Save the last phone
			if (phoneStart < phonetic.Length)
				phones.Add(phonetic.Substring(phoneStart));

			return phones.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Put delimiters around all ambiguous sequences so they're sure to be parsed into
		/// their own phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string DelimitAmbiguousSequences(string phonetic)
		{
			if (m_sortedAmbiguousSeqList == null && m_unsortedAmbiguousSeqList != null)
				BuildSortedAmbiguousSequencesList();
			
			if (m_sortedAmbiguousSeqList != null)
			{
				foreach (AmbiguousSeq ambigSeq in m_sortedAmbiguousSeqList)
				{
					if (ambigSeq.Convert)
					{
						phonetic = phonetic.Replace(ambigSeq.Unit,
							string.Format(s_forcedPhoneDelimiterFmt, ambigSeq.Unit));
					}
				}
			}

			return phonetic;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the end of an ambiguous sequence or experimental transcription, extracts the
		/// sequence from the specified phonetic string and adds it to the specified list of
		/// phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddPhoneFromBetweenDelimiters(string phonetic, List<string> phones,
			ref int i)
		{
			int end = phonetic.IndexOf(kForcedPhoneDelimiter, i + 1);
			if (end < 0)
				return;

			phones.Add(phonetic.Substring(i + 1, end - (i + 1)));
			i = end;
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
				// Experimental transcription delimiters in uncertain groups are actually
				// redundant since uncertain groups should be transcribed as there are
				// no experimental transcriptions. Therefore, skip the delimiters when
				// they're found.
				if (phonetic[i] == kForcedPhoneDelimiter)
					continue;

				if (phonetic[i] == '/' || phonetic[i] == ',')
				{
					// If the phone is the empty set character just save an empty string.
					tmpPhone = bldrPhone.ToString();
					if (kEmptySetChars.Contains(tmpPhone))
						tmpPhone = string.Empty;

					phones.Add(tmpPhone);
					bldrPhone.Length = 0;
					continue;
				}

				bldrPhone.Append(phonetic[i]);
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
			if (kEmptySetChars.Contains(tmpPhone))
				tmpPhone = string.Empty;

			// Save the last uncertain phone and add the list of uncertain phones to
			// the collection of those for the word
			phones.Add(tmpPhone);
			uncertainPhones[phoneNumber] = phones.ToArray();

			if (m_undefinedCharacters != null)
				ValidateCodepointsInUncertainPhones(phonetic, phones);

			return phones[0];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks all the Unicode values in the list of uncertain phones to make sure they
		/// are in PA's character code inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ValidateCodepointsInUncertainPhones(string phonetic, List<string> phones)
		{
			foreach (string phone in phones)
			{
				foreach (char c in phone)
				{
					// Get the information for the current codepoint.
					if (!ContainsKey(c))
						m_undefinedCharacters.Add(c, phonetic);
				}
			}
		}

		#endregion
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Stores information about IPA characters.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class IPACharInfo
	{
		[XmlAttribute]
		public int Codepoint;
		[XmlAttribute]
		public string IPAChar;
		[XmlAttribute]
		public string HexIPAChar;
		public string Name;
		public string Description;
		public IPACharacterType CharType;
		public IPACharacterSubType CharSubType;
		public IPACharIgnoreTypes IgnoreType;
		public bool IsBaseChar;
		public bool CanPreceedBaseChar;
		public bool DisplayWDottedCircle;
		public int DisplayOrder;
		public int MOArticulation;
		public int POArticulation;
		public ulong Mask0;
		public ulong Mask1;
		public ulong BinaryMask;
		public int ChartColumn;
		public int ChartGroup;

		[XmlIgnore]
		public bool IsUndefined = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0}: U+{1:X4}, {2}, {3}", IPAChar, Codepoint, Name, Description);
		}
	}
}
