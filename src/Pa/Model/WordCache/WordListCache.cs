using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Cache of words to show in a grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordListCache : List<WordListCacheEntry>
	{
		private bool m_isForSearchResults;
		private bool m_isForRegExpSearchResults;
		private bool m_isCIEList;
		private SortedList<int, string> m_cieGroupTexts;
		private SearchQuery m_searchQuery;

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
			// First, send a message that the cache needs to be sorted. If true
			// is returned then something handled the sort (e.g. an add-on).
			// Otherwise use the default sorting routine.
			object[] sortInfo = new object[] { this, sortOptions };
			if (!PaApp.MsgMediator.SendMessage("SortCache", sortInfo))
				Sort(new CacheSortComparer(sortOptions));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort the wordListCache based on a column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Sort(SortOptions sortOptions, string sortColName, bool changeSortDirection)
		{
			bool ascending = sortOptions.SetPrimarySortField(sortColName, changeSortDirection);

			// First, send a message that the cache needs to be sorted. If true
			// is returned then something handled the sort (e.g. an add-on).
			// Otherwise use the default sorting routine.
			object[] sortInfo = new object[] { this, sortOptions };
			if (!PaApp.MsgMediator.SendMessage("SortCache", sortInfo))
				Sort(new CacheSortComparer(sortOptions));
			
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsEmpty
		{
			get { return Count == 0; }
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

			//string phonetic = entry.PhoneticValue;

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

			// Do some preprocessing for spaces before and after.
			ProcessSpaces(ref phones, ref offset, ref length);
			
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
			newEntry.SearchItem = (length == 0 ? string.Empty :
				string.Join(string.Empty, phones, offset, length));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For search results we want to strip off the initial and/or final space and adjust
		/// the offset and length as necessary.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ProcessSpaces(ref string[] phones, ref int offset, ref int length)
		{
			bool changeMade = false;
			List<string> tmpPhones = new List<string>(phones);

			// If there's an initial space, remove it. If the match was found on
			// that space, then the length includes the space so subtract one
			// from the length to account for removing the space. If the match
			// does not include the space, then adjust the offset to account for
			// the fact that the space was removed.
			if (tmpPhones[0] == " ")
			{
				changeMade = true;
				tmpPhones.RemoveAt(0);
				if (offset == 0)
					length--;
				else
					offset--;
			}

			// If there's a final space, remove it. If the match includes that
			// space, then adjust the match's length to account for the fact that
			// the space has been removed.
			int lastPhone = tmpPhones.Count - 1;
			if (tmpPhones[lastPhone] == " " && offset + length >= tmpPhones.Count)
			{
				changeMade = true;
				length--;
				tmpPhones.RemoveAt(lastPhone);
			}

			if (length < 0)
				length = 0;

			if (changeMade)
				phones = tmpPhones.ToArray();
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FieldInfo.FieldName + ": " + (ascending ? "Ascending" : "Descending");
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
		private SortInformationList m_sortInfoList;

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
			AdvSortOrder = new[] { 1, 0, 2 };

			// Keeps track of the R/L selections. Set the defaults as follows.
			AdvRlOptions = new[] { true, false, false };

			m_sortInfoList = new SortInformationList();

			// Default sort is by point of articulation and phonetic field.
			SortType = PhoneticSortType.POA;

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
			clone.SortType = SortType;
			clone.SaveManuallySetSortOptions = SaveManuallySetSortOptions;
			clone.AdvancedEnabled = AdvancedEnabled;

			for (int i = 0; i < AdvSortOrder.Length; i++)
				clone.AdvSortOrder[i] = AdvSortOrder[i];

			for (int i = 0; i < AdvRlOptions.Length; i++)
				clone.AdvRlOptions[i] = AdvRlOptions[i];

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

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Compares the contents of this SortOptions object with the one specified.
		///// TODO: Write some tests for this method. It could be used to fix PA-830.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public bool AreEqual(SortOptions otherOptions)
		//{
		//    if (otherOptions == null)
		//        return false;

		//    if (m_sortType != otherOptions.m_sortType ||
		//        m_advancedEnabled != otherOptions.m_advancedEnabled ||
		//        m_saveManuallySetSortOptions != otherOptions.m_saveManuallySetSortOptions)
		//    {
		//        return false;
		//    }

		//    for (int i = 0; i < m_advSortOptions.Length; i++)
		//    {
		//        if (m_advSortOptions[i] != otherOptions.m_advSortOptions[i])
		//            return false;
		//    }

		//    for (int i = 0; i < m_advRlOptions.Length; i++)
		//    {
		//        if (m_advRlOptions[i] != otherOptions.m_advRlOptions[i])
		//            return false;
		//    }

		//    if (m_sortInfoList == null && otherOptions.m_sortInfoList != null ||
		//        m_sortInfoList != null && otherOptions.m_sortInfoList == null ||
		//        m_sortInfoList.Count != otherOptions.m_sortInfoList.Count)
		//    {
		//        return false;
		//    }

		//    for (int i = 0; i < m_sortInfoList.Count; i++)
		//    {
		//        if (m_sortInfoList[i].ascending != otherOptions.m_sortInfoList[i].ascending ||
		//            m_sortInfoList[i].FieldInfo.FieldName !=
		//            otherOptions.m_sortInfoList[i].FieldInfo.FieldName)
		//        {
		//            return false;
		//        }
		//    }

		//    return true;
		//}

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
		public PhoneticSortType SortType { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to save as the defaults, those sort
		/// sort options the user specifies as he clicks grid column headings or changes
		/// phonetic sort options via the phonetic sort options drop-down. Setting this value
		/// to true will cause the defaults set in the options dialog to be overridden as the
		/// user clicks column headings or changes phonetic sort options from the drop-down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SaveManuallySetSortOptions { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvancedChecked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AdvancedEnabled { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvSortOrder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int[] AdvSortOrder { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets AdvRlOptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool[] AdvRlOptions { get; set; }

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
