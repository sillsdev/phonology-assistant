using System;
using System.IO;
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
	public class UndefinedPhoneticCharactersInfoList : SortedDictionary<char, UndefinedPhoneticCharactersInfo>
	{
		private string m_sourceName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list's current source name. This could be a data source or some
		/// other string uniquely identifying the source of data to parse.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SourceName
		{
			get { return m_sourceName; }
			set { m_sourceName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an individual code point that is not defined in the IPA character cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(char c, string phoneticWord)
		{
			if (!string.IsNullOrEmpty(phoneticWord) && !ContainsKey(c))
			{
				UndefinedPhoneticCharactersInfo ucpInfo = new UndefinedPhoneticCharactersInfo();
				ucpInfo.PhoneticWord = phoneticWord;
				ucpInfo.SourceName = SourceName;
				this[c] = ucpInfo;
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
		public string PhoneticWord;
		public string SourceName;
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
		private static UndefinedPhoneticCharactersInfoList s_undefinedCodepoints;

		public enum SortType
		{
			MOArticulation,
			POArticulation,
			Unicode
		}

		private const string kEmptySetChars = "0\u2205";
		private const string kForcedPhoneDelimiterStr = "$";
		private const char kForcedPhoneDelimiter = '$';
		
		private static IPACharCache s_cache = null;
		private static ExperimentalTranscriptions s_experimentalTransList = null;
		private static AmbiguousSequences s_ambiguousSeqList = null;

		private static string s_forcedPhoneDelimiterFmt =
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
			s_experimentalTransList = new ExperimentalTranscriptions();
			s_ambiguousSeqList = new AmbiguousSequences();
			m_cacheFileName = BuildFileName(projectFileName, true);
			s_cache = this;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the name from which to load or save the cache file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string BuildFileName(string projectFileName, bool mustExist)
		{
			string filename = (projectFileName == null ? string.Empty : projectFileName);
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
			get { return s_ambiguousSeqList; }
			set
			{
				s_ambiguousSeqList.Clear();

				// Copy the references from the specified list to our own.
				if (value != null)
				{
					foreach (AmbiguousSeq ambiguousSeq in value)
						s_ambiguousSeqList.Add(ambiguousSeq);
				}

				/// Go through the tone letters collection and add them to the ambiguous list.
				/// Tone letters are special in that they're the only IPA characters in the
				/// cache that are made up of multiple code points. When it comes to parsing a
				/// phonetic string into its phones, tone letters need to be treated as
				/// ambiguous sequences.
				if (s_ambiguousSeqList != null && m_toneLetters != null)
				{
					foreach (IPACharInfo info in m_toneLetters.Values)
					{
						if (!s_ambiguousSeqList.ContainsSeq(info.IPAChar, true))
							s_ambiguousSeqList.Add(new AmbiguousSeq(info.IPAChar));
					}
				}

				// Now order the items in the list based on the length
				// of the ambiguous sequence -- longest to shortest.
				for (int i = s_ambiguousSeqList.Count - 1; i >= 0; i--)
				{
					int lenLast = (s_ambiguousSeqList[i].Unit == null ? 0 :
						s_ambiguousSeqList[i].Unit.Length);

					int lenFirst = (s_ambiguousSeqList[0].Unit == null ? 0 :
						s_ambiguousSeqList[0].Unit.Length);
					
					// If the current ambiguous item is longer than the first one in the list,
					// then move the current one to the beginning of the list.
					if (lenLast > lenFirst)
					{
						s_ambiguousSeqList.Insert(0, s_ambiguousSeqList[i]);
						s_ambiguousSeqList.RemoveAt(i + 1);
					}
				}
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExperimentalTranscriptions ExperimentalTranscriptions
		{
			get { return s_experimentalTransList; }
			set
			{
				s_experimentalTransList.Clear();

				// Copy the references from the specified list to our own.
				if (value != null)
				{
					foreach (ExperimentalTrans experimentalTrans in value)
						s_experimentalTransList.Add(experimentalTrans);
				}

				// Now order the items in the list on the "convert to"
				// lengths -- longest to shortest.
				for (int i = s_experimentalTransList.Count - 1; i >= 0; i--)
				{
					int lenLast = (s_experimentalTransList[i].CurrentTransToConvert == null ? 0 :
						s_experimentalTransList[i].CurrentTransToConvert.Length);

					int lenFirst = (s_experimentalTransList[0].CurrentTransToConvert == null ? 0 :
						s_experimentalTransList[0].CurrentTransToConvert.Length);
					
					// If the current phone is longer than the first phone in the list,
					// then move the current phone to the beginning of the list.
					if (lenLast > lenFirst)
					{
						s_experimentalTransList.Insert(0, s_experimentalTransList[i]);
						s_experimentalTransList.RemoveAt(i + 1);
					}
				}
			}
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
			s_cache = new IPACharCache(projectFileName);
			
			// Deserialize into a List<T> because a Dictionary<TKey, TValue>
			// (i.e. IPACharCache) isn't serializable nor deserializable.
			List<IPACharInfo> tmpCache = STUtils.DeserializeData(
				s_cache.CacheFileName, typeof(List<IPACharInfo>)) as List<IPACharInfo>;

			if (tmpCache == null)
			{
				s_cache = null;
				return null;
			}

			s_cache.LoadFromList(tmpCache);
			tmpCache.Clear();
			tmpCache = null;

			if (s_cache.Count == 0)
				s_cache = null;

			// This should never return null.
			return s_cache;
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
			List<IPACharInfo> tmpCache = ToList();
			STUtils.SerializeData(m_cacheFileName, tmpCache);
			tmpCache = null;
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
			List<IPACharInfo> tmpCache = new List<IPACharInfo>();
			foreach (KeyValuePair<int, IPACharInfo> info in this)
				tmpCache.Add(info.Value);

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
				
				return (ipaCharStr == null ? null : this[ipaCharStr[0]]);
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

		#endregion

		#region Phonetic string parser
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of code points found in the data but not found in the IPA
		/// character cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static UndefinedPhoneticCharactersInfoList UndefinedCodepoints
		{
			get { return IPACharCache.s_undefinedCodepoints; }
			set { IPACharCache.s_undefinedCodepoints = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into a new string of phones delimited
		/// by commas.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string PhoneticParser_CommaDelimited(string phonetic, bool normalize)
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
		public static string[] PhoneticParser(string phonetic, bool normalize)
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
		public static string[] PhoneticParser(string phonetic, bool normalize,
			out Dictionary<int, string[]> uncertainPhones)
		{
			uncertainPhones = null;

			// Return an empty array if there's nothing in the phonetic.
			if (string.IsNullOrEmpty(phonetic))
				return null;

			List<string> phones = new List<string>();
			IPACharInfo ciPrev = null;

			// Normalize the string if necessary.
			if (normalize)
			    phonetic = FFNormalizer.Normalize(phonetic);

			phonetic = DelimitExperimentalTranscriptions(phonetic);
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
				IPACharInfo ciCurr = s_cache[c];

				// If there's no information for a code point or there is but there isn't
				// any for the previous character and the current character isn't a base
				// character, then treat the character as it's own phone.
				if (ciCurr == null || (ciPrev == null && !ciCurr.IsBaseChar) ||
					ciCurr.CharType == IPACharacterType.Unknown)
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
					if (badChar != '\0' && s_undefinedCodepoints != null)
						s_undefinedCodepoints.Add(c, phonetic);

					// Uncomment the following line if it's desired to save the
					// junk character in the list of phones.
					// phones.Add(phonetic.Substring(i, 1));

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
				// it's not the first in the string, close the previous phone.
				if (ciCurr.IsBaseChar && i > phoneStart)
				{
					phones.Add(phonetic.Substring(phoneStart, i - phoneStart));
					phoneStart = i;
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
		private static string DelimitAmbiguousSequences(string phonetic)
		{
			foreach (AmbiguousSeq ambigSeq in s_ambiguousSeqList)
			{
				if (ambigSeq.Convert)
				{
					phonetic = phonetic.Replace(ambigSeq.Unit,
						string.Format(s_forcedPhoneDelimiterFmt, ambigSeq.Unit));
				}
			}

			return phonetic;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Put delimiters around all experimental transcriptions so they're sure to be
		/// parsed into their own phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string DelimitExperimentalTranscriptions(string phonetic)
		{
			foreach (ExperimentalTrans experimentalTrans in s_experimentalTransList)
			{
				if (experimentalTrans.CurrentTransToConvert != null)
				{
					phonetic = phonetic.Replace(experimentalTrans.Item,
						string.Format(s_forcedPhoneDelimiterFmt,
						experimentalTrans.CurrentTransToConvert));
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
		private static void AddPhoneFromBetweenDelimiters(string phonetic,
			List<string> phones, ref int i)
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
		private static string GetUncertainties(string phonetic, ref int i, int phoneNumber,
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
			// parentheses group was found but didn't contain any slashes.
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

			if (s_undefinedCodepoints != null)
				ValidateCodepointsInUncertainPhones(phonetic, phones);

			return phones[0];		
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks all the Unicode values in the list of uncertain phones to make sure they
		/// are in PA's character code inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ValidateCodepointsInUncertainPhones(string phonetic, List<string> phones)
		{
			foreach (string phone in phones)
			{
				foreach (char c in phone)
				{
					// Get the information for the current codepoint.
					if (!s_cache.ContainsKey((int)c))
						s_undefinedCodepoints.Add(c, phonetic);
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
	}
}
