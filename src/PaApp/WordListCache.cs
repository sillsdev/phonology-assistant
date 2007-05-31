using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;
using System.Xml.Serialization;
using SIL.Pa.Data;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Cache of words to show in a grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordListCache : List<WordListCacheEntry>
	{
		private bool m_isForFindPhoneResults = false;
		private bool m_isCIEList = false;
		private SortedList<int, string> m_cieGroupTexts;
		private FFSearchEngine.SearchQuery m_searchQuery = null;

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the search pattern used to generate the contents of the cache. When
		/// the cache is not for search results, then this value is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FFSearchEngine.SearchQuery SearchQuery
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForFindPhoneResults
		{
			get { return m_isForFindPhoneResults; }
			set { m_isForFindPhoneResults = value; }
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
		/// (which is passed to in the entry parameter).</param>
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
			
			m_isForFindPhoneResults = true;
			newEntry.SearchItemPhoneOffset = offset;
			newEntry.SearchItemPhoneLength = length;

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
		private void ProcessSpaces(ref string[] phones, ref int offset, int length)
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
			for (int i = 0; i < this.Count; i++)
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
	public struct SortInformation
	{
		public PaFieldInfo FieldInfo;
		public bool ascending;

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
