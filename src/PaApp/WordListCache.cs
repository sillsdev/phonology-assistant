using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.Pa.FFSearchEngine;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Cache of words to show in a grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordListCache : List<WordListCacheEntry>
	{
		private bool m_isForSearchResults = false;
		private bool m_isForRegExpSearchResults = false;
		private bool m_isCIEList = false;
		private SortedList<int, string> m_cieGroupTexts;
		private SearchQuery m_searchQuery = null;

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the search pattern used to generate the contents of the cache. When
		/// the cache is not for search results, then this value is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery
		{
			get { return m_searchQuery; }
			set { m_searchQuery = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of CIE group headings built when the contents of the cache
		/// represent a list of minimal pairs. When the cache is not for minimal pairs this
		/// list is null. The key of the list is the CIEGroupId and the value is the CIE
		/// group text, or heading, associated with the group id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortedList<int, string> CIEGroupTexts
		{
			get { return m_cieGroupTexts; }
			set { m_cieGroupTexts = value; }
		}
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort the wordListCache based on the specified sort options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Sort(SortOptions sortOptions)
		{
			CacheSortComparer cacheSorter = new CacheSortComparer(sortOptions);
			Sort(cacheSorter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort the wordListCache based on a column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Sort(SortOptions sortOptions, string sortColName, bool changeSortDirection)
		{
			bool ascending = sortOptions.SetPrimarySortField(sortColName, changeSortDirection);
			CacheSortComparer cacheSorter = new CacheSortComparer(sortOptions);
			Sort(cacheSorter);
			return ascending;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cache is for a search results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForSearchResults
		{
			get { return m_isForSearchResults; }
			set { m_isForSearchResults = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cache is for a search result
		/// from a regular expression search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForRegExpSearchResults
		{
			get { return m_isForRegExpSearchResults; }
			set { m_isForRegExpSearchResults = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCIEList
		{
			get { return m_isCIEList; }
			set
			{
				m_isCIEList = value;
				if (!m_isCIEList)
				{
					foreach (WordListCacheEntry entry in this)
						entry.ShowInList = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Uses the offset and length to build three groups of strings, the phones in the
		/// environment before, the phones in the environment after and the phones in the
		/// search item.
		/// </summary>
		/// <param name="entry">The underlying word cache entry from which we're building
		/// a WordListCacheEntry.</param>
		/// <param name="phones">Array of phones that make up the phonetic word. When
		/// this is null or empty, then it's assumed the WordListCacheEntry is not for a
		/// search result item.</param>
		/// <param name="offset">Offset in the phones array where a search item was found.
		/// </param>
		/// <param name="length">Length in phones of the matched search item.</param>
		/// <param name="savePhones">When true, it forces the phones passed in the phones
		/// parameter to be saved in the new WordListCacheEntry rather than the
		/// new WordListCacheEntry deferring to the phones from its associated WordCacheEntry
		/// (which is passed in the entry parameter). This is necessary for entries that
		/// are for words derived from non primary uncertain phones.</param>
		/// ------------------------------------------------------------------------------------
		public void AddEntryFromRegExpSearch(WordCacheEntry entry, string[] phones,
			int offset, int length, bool savePhones)
		{
			WordListCacheEntry newEntry = new WordListCacheEntry();
			newEntry.WordCacheEntry = entry;
			Add(newEntry);

			//// When the array of phones is non-existent, it means
			//// we're not adding an entry for a search result cache.
			//if (phones == null || phones.Length == 0)
			//    return;

			newEntry.SearchItemOffset = offset;
			newEntry.SearchItemLength = length;

			if (savePhones)
				newEntry.SetPhones(phones);

			string phonetic = entry.PhoneticValue;

			//// Build the environment before string.
			//newEntry.EnvironmentBefore = (offset == 0 ? string.Empty :
			//    phonetic.Substring(0, offset));

			//// Build the environment after string.
			//newEntry.EnvironmentAfter = (offset == phonetic.Length - 1 ? string.Empty :
			//    phonetic.Substring(offset + length));

			//// Build the search item string.
			//newEntry.SearchItem = phonetic.Substring(offset, length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(WordCacheEntry entry)
		{
			Add(entry, null, 0, 0, false);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Uses the offset and length to build three groups of strings, the phones in the
		/// environment before, the phones in the environment after and the phones in the
		/// search item.
		/// </summary>
		/// <param name="entry">The underlying word cache entry from which we're building
		/// a WordListCacheEntry.</param>
		/// <param name="phones">Array of phones that make up the phonetic word. When
		/// this is null or empty, then it's assumed the WordListCacheEntry is not for a
		/// search result item.</param>
		/// <param name="offset">Offset in the phones array where a search item was found.
		/// </param>
		/// <param name="length">Length in phones of the matched search item.</param>
		/// <param name="savePhones">When true, it forces the phones passed in the phones
		/// parameter to be saved in the new WordListCacheEntry rather than the
		/// new WordListCacheEntry deferring to the phones from its associated WordCacheEntry
		/// (which is passed in the entry parameter).</param>
		/// ------------------------------------------------------------------------------------
		public void Add(WordCacheEntry entry, string[] phones, int offset, int length,
			bool savePhones)
		{
			WordListCacheEntry newEntry = new WordListCacheEntry();
			newEntry.WordCacheEntry = entry;
			Add(newEntry);

			// When the array of phones is non-existent, it means
			// we're not adding an entry for a search result cache.
			if (phones == null || phones.Length == 0)
				return;

			// If we're adding a search result entry then do some preprocessing first.
			if (offset >= 0 && length > 0)
				ProcessSpaces(ref phones, ref offset, length);
			
			m_isForSearchResults = true;
			newEntry.SearchItemOffset = offset;
			newEntry.SearchItemLength = length;

			if (savePhones)
				newEntry.SetPhones(phones);

			// Build the environment before string.
			if (offset > 0)
				newEntry.EnvironmentBefore = string.Join(string.Empty, phones, 0, offset);

			// Build the environment after string.
			if (offset < phones.Length - 1)
			{
				int afterStart = offset + length;
				newEntry.EnvironmentAfter = string.Join(string.Empty, phones,
					afterStart, phones.Length - afterStart);
			}

			// Build the search item string.
			newEntry.SearchItem = string.Join(string.Empty, phones, offset, length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For search results we want to strip off the initial and/or final space when the
		/// space is not part of the search item (and will, therefore, not be displayed in
		/// the highlighted portion of the phontic search result column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ProcessSpaces(ref string[] phones, ref int offset, int length)
		{
			// Determine whether or not we found a match at the beginning
			// of the word and whether or not that includes an initial space.
			bool removeInitialSpace = (offset > 0 && phones[0] == " ");

			// Determine whether or not we found a match at the end of
			// the word and whether or not that includes a final space.
			bool removeFinalSpace =
				(offset + length < phones.Length && phones[phones.Length - 1] == " ");

			if (removeInitialSpace || removeFinalSpace)
			{
				List<string> tmpPhones = new List<string>(phones);
				if (removeFinalSpace)
					tmpPhones.RemoveAt(phones.Length - 1);

				if (removeInitialSpace)
				{
					tmpPhones.RemoveAt(0);
					offset--;
				}

				phones = tmpPhones.ToArray();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because it's possible for someone to write a regular expression for the search
		/// item that finds a match that starts and ends in the middle of phones, we need to
		/// go through all the matches and make sure the offset and length for the matches
		/// are extended to include the entire phones for those matches whose offset and
		/// length don't fall on phone boundaries.
		/// </summary>
		/// <remarks>
		/// At the beginning of this method, the SearchItemOffset for each entry in the cache
		/// is the character (i.e. codepoint) offset in the phonetic string where a match was
		/// found. The SearchItemLength for each entry in the cache is the length in characters
		/// (i.e. codepoints) of the match. By the end of the method, SearchItemOffset will
		/// be an offset into the phonetic string's phone collection and SearchItemLength will
		/// be the number of phones in the match.
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public void ExtendRegExpMatchesToPhoneBoundaries()
		{
			foreach (WordListCacheEntry entry in this)
			{
				int startOffset = entry.SearchItemOffset;
				int endOffset = entry.SearchItemOffset + entry.SearchItemLength - 1;
				int firstPhoneInMatch = -1;
				int lastPhoneInMatch = entry.Phones.Length - 1;
				int accumulatedPhoneLengths = 0;

				// Go through each phone in the entry's phone collection.
				for (int i = 0; i < entry.Phones.Length; i++)
				{
					int phoneLength = entry.Phones[i].Length;

					// Have we yet determined the first full phone in the match?
					if (firstPhoneInMatch < 0)
					{
						// Check if the accumulated phone lengths up to this point plus
						// the length of the current phone exceeds the beginning offset
						// of the match. If it does, then the current phone should be
						// considered the first phone in the match.
						if (accumulatedPhoneLengths + phoneLength > startOffset)
							firstPhoneInMatch = i;
					}
					
					if (accumulatedPhoneLengths + phoneLength > endOffset)
					{
						// At this point, we know that accumulated phone lengths plus
						// the length of the current phone exceeds the offset of the
						// last character in the match, so the current phone should be
						// considered the last full phone in the match.
						lastPhoneInMatch = i;
						break;
					}

					accumulatedPhoneLengths += phoneLength;
				}

				// Change the matched offset and length. The offset is an offset into the
				// phone collection and the length is the number of phone in the match.
				entry.SearchItemOffset = firstPhoneInMatch;
				entry.SearchItemLength = lastPhoneInMatch - firstPhoneInMatch + 1;

				// Build the environment before string.
				if (entry.SearchItemOffset > 0)
				{
					entry.EnvironmentBefore =
						string.Join(string.Empty, entry.Phones, 0, entry.SearchItemOffset);
				}

				// Build the environment after string.
				if (entry.SearchItemOffset < entry.Phones.Length - 1)
				{
					int afterStart = entry.SearchItemOffset + entry.SearchItemLength;
					entry.EnvironmentAfter = string.Join(string.Empty, entry.Phones,
						afterStart, entry.Phones.Length - afterStart);
				}

				// Build the search item string.
				entry.SearchItem = string.Join(string.Empty, entry.Phones,
					entry.SearchItemOffset, entry.SearchItemLength);
			}
		}
	}

	#region SortInformationList class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// SortInformationList contains a list of SortInformation objects.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SortInformationList : List<SortInformation>
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns true if the SortInformationList contains the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Contains(string sortField)
		{
			foreach (SortInformation info in this)
			{
				if (info.FieldInfo.FieldName == sortField)
					return true;
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the index of the SortInformation object for the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int IndexOf(string sortField)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].FieldInfo.FieldName == sortField)
					return i;
			}

			return -1;
		}
	}

	#endregion

	#region SortInformation struct
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The SortInformation struct holds sort information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SortInformation
	{
		public PaFieldInfo FieldInfo;
		public bool ascending;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortInformation()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortInformation(PaFieldInfo fieldInfo, bool sortDirection)
		{
			FieldInfo = fieldInfo;
			ascending = sortDirection;
		}
	}

	#endregion

	#region Phonetic Sort Options

	public enum PhoneticSortType { POA, MOA, Unicode };

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The PhoneticSortOptions class holds the sort options information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SortOptions
	{
		private PhoneticSortType m_sortType = PhoneticSortType.POA;
		private bool m_advancedEnabled = false;
		private int[] m_advSortOptions;
		private bool[] m_advRlOptions;
		private SortInformationList m_sortInfoList;
		private bool m_saveManuallySetSortOptions = false;
		
		#region Constructor and Loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// SortOptions constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions() : this(false)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// SortOptions constructor.
		/// </summary>
		/// <param name="initializeWithPhonetic">Indicates whether or not the new SortOptions
		/// object's SortInformationList should contain phonetic as the default field
		/// on which to sort.</param>
		/// ------------------------------------------------------------------------------------
		public SortOptions(bool initializeWithPhonetic)
		{
			// Keeps track of the Before, Item, & After sorting order. Set the default
			// as follows.
			AdvSortOrder = new int[] { 1, 0, 2 };

			// Keeps track of the R/L selections. Set the defaults as follows.
			AdvRlOptions = new bool[] { true, false, false };

			m_sortInfoList = new SortInformationList();

			// Default sort is by point of articulation and phonetic field.
			m_sortType = PhoneticSortType.POA;

			if (initializeWithPhonetic && PaApp.FieldInfo != null &&
				PaApp.FieldInfo.PhoneticField != null)
			{
				SetPrimarySortField(PaApp.FieldInfo.PhoneticField, false, true);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a clone (i.e. deep copy) of the sort options object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions Clone()
		{
			SortOptions clone = new SortOptions(false);
			clone.m_sortType = m_sortType;
			clone.m_saveManuallySetSortOptions = m_saveManuallySetSortOptions;
			clone.m_advancedEnabled = m_advancedEnabled;

			for (int i = 0; i < m_advSortOptions.Length; i++)
				clone.m_advSortOptions[i] = m_advSortOptions[i];

			for (int i = 0; i < m_advRlOptions.Length; i++)
				clone.m_advRlOptions[i] = m_advRlOptions[i];

			if (m_sortInfoList != null)
			{
				// This should copy each element since each element is a struct, not a ref. type.
				SortInformationList siList = new SortInformationList();
				foreach (SortInformation info in m_sortInfoList)
					siList.Add(info);

				clone.m_sortInfoList = siList;
			}

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializing a project brings in the field info. list for sort options from the
		/// project file (i.e. .pap file) but the field info list for the sort options should
		/// really just be references to the project's field info. list
		/// (i.e. PaApp.Project.FieldInfo). This method will iterate through the sort option's
		/// field info list, updating it's references to those found in the project's field
		/// info. list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SyncFieldInfo(PaFieldInfoList fieldInfoList)
		{
			if (fieldInfoList == null)
				return;

			for (int i = 0; i < m_sortInfoList.Count; i++)
			{
				if (m_sortInfoList[i].FieldInfo != null)
				{
					PaFieldInfo fieldInfo = fieldInfoList[m_sortInfoList[i].FieldInfo.FieldName];
					if (fieldInfo != null && fieldInfo != m_sortInfoList[i].FieldInfo)
						m_sortInfoList[i].FieldInfo = fieldInfo;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the specified field the first, or primary, field on which to sort.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetPrimarySortField(string newPrimarySortField, bool changeDirection)
		{
			return SetPrimarySortField(PaApp.Project.FieldInfo[newPrimarySortField],
				changeDirection);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the specified field the first, or primary, field on which to sort.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetPrimarySortField(PaFieldInfo fieldInfo, bool changeDirection)
		{
			return SetPrimarySortField(fieldInfo, changeDirection, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the specified field the first, or primary, field on which to sort.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetPrimarySortField(PaFieldInfo fieldInfo, bool changeDirection, bool ascending)
		{
			if (fieldInfo == null)
				return ascending;

			int index = m_sortInfoList.IndexOf(fieldInfo.FieldName);

			// If the sort information list already contains an item for the specified field,
			// we need to remove it before reinserting it at the beginning of the list.
			if (index > -1)
			{
				ascending = m_sortInfoList[index].ascending;
				m_sortInfoList.RemoveAt(index);
			}

			if (changeDirection)
				ascending = !ascending;

			// Now insert an item at the beginning of the list since the specified field
			// has now become the first (i.e. primary) field on which to sort.
			m_sortInfoList.Insert(0, new SortInformation(fieldInfo, ascending));

			return ascending;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets SortType.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneticSortType SortType
		{
			get { return m_sortType; }
			set { m_sortType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to save as the defaults, those sort
		/// sort options the user specifies as he clicks grid column headings or changes
		/// phonetic sort options via the phonetic sort options drop-down. Setting this value
		/// to true will cause the defaults set in the options dialog to be overridden as the
		/// user clicks column headings or changes phonetic sort options from the drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SaveManuallySetSortOptions
		{
			get { return m_saveManuallySetSortOptions; }
			set { m_saveManuallySetSortOptions = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvancedChecked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AdvancedEnabled
		{
			get { return m_advancedEnabled; }
			set { m_advancedEnabled = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvSortOrder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int[] AdvSortOrder
		{
			get { return m_advSortOptions; }
			set { m_advSortOptions = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvRlOptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool[] AdvRlOptions
		{
			get { return m_advRlOptions; }
			set { m_advRlOptions = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets SortInformationList.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortInformationList SortInformationList
		{
			get 
			{
				if (m_sortInfoList == null)
					m_sortInfoList = new SortInformationList();
				
				return m_sortInfoList; 
			}
			set { m_sortInfoList = value; }
		}
	}

	#endregion
}
