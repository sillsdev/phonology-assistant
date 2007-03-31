using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Cache of words to show in a grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordListCache : List<WordListCacheEntry>
	{
		private bool m_isForFindPhoneResults = false;
		private SortInformationList m_sortInfoList = new SortInformationList();

		#region Properties

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get SortInformationList.
		/// </summary>
		/// <returns>int</returns>
		/// ------------------------------------------------------------------------------------
		public List<SortInformation> SortInformationList
		{
			get { return m_sortInfoList; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sort the wordListCache based on a column.
		/// </summary>
		/// <param name="sortColName">Column name</param>
		/// ------------------------------------------------------------------------------------
		public bool Sort(object args, out string sortColName)
		{
			bool ascending = false;

			sortColName = 
				(args is String ? args as String : DBFields.Phonetic);

			// Remove the SortInformation if it already exists
			int sortFieldIndex = 0;
			if (m_sortInfoList.Contains(sortColName))
			{
				sortFieldIndex = m_sortInfoList.IndexOfSortField(sortColName);
				ascending = m_sortInfoList[sortFieldIndex].ascending;
				m_sortInfoList.Remove(m_sortInfoList[sortFieldIndex]);
			}

			// Reverse the sort direction every time the column is clicked
			m_sortInfoList.Insert(0, new SortInformation(sortColName, !ascending));

			CacheSortComparer cacheSorter;
			if (sortColName == DBFields.Phonetic)
				cacheSorter = new CacheSortComparer(m_sortInfoList, args);
			else
		   		cacheSorter = new CacheSortComparer(m_sortInfoList);

			//PhoneticSortComparer cacheSorter = new PhoneticSortComparer(m_sortInfoList);
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
		/// Uses the information in result to build three strings, the environment before,
		/// the environment after and the search item. Those three strings are then saved
		/// in a collection (m_ffEticPortions) whose key is the record index. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(WordCacheEntry entry)
		{
			Add(entry, null, new int[] {});
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Uses the information in result to build three strings, the environment before,
		/// the environment after and the search item. Those three strings are then saved
		/// in a collection (m_ffEticPortions) whose key is the record index. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Add(WordCacheEntry entry, string[] eticChars, int[] result)
		{
			WordListCacheEntry newEntry = new WordListCacheEntry();
			newEntry.WordCacheEntry = entry;
			Add(newEntry);

			if (eticChars == null)
				return;
			
			m_isForFindPhoneResults = true;
			
			int index = newEntry.SearchItemPhoneOffset = result[0];
			int len = newEntry.SearchItemPhoneLength = result[1];

			// Build the environment before string.
			if (index > 0)
				newEntry.EnvironmentBefore = string.Join(string.Empty, eticChars, 0, index);

			// Build the environment after string.
			if (index < eticChars.Length - 1)
			{
				int afterStart = index + len;
				newEntry.EnvironmentAfter = string.Join(string.Empty, eticChars,
					afterStart, eticChars.Length - afterStart);
			}

			// Build the search item string.
			newEntry.SearchItem = string.Join(string.Empty, eticChars, index, len);
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
		/// Returns true if the SortInformationList contains the sortField.
		/// </summary>
		/// <param name="sortField">Column name</param>
		/// ------------------------------------------------------------------------------------
		public bool Contains(string sortField)
		{
			foreach (SortInformation info in this)
			{
				if (info.sortField == sortField)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the index of the SortInformation object that has the sortField.
		/// </summary>
		/// <param name="sortField">Column name</param>
		/// ------------------------------------------------------------------------------------
		public int IndexOfSortField(string sortField)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].sortField == sortField)
					return i;
			}

			return -1; // Error! Did not find the sortField
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
		public string sortField;
		public bool ascending;

		public SortInformation(string columnName, bool sortDirection)
		{
			sortField = columnName;
			ascending = sortDirection;
		}
	}

	#endregion

	#region Phonetic Sort Options

	public enum PhoneticSortType { Unicode, MOA, POA };

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The PhoneticSortOptions class holds the sort options information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneticSortOptions
	{
		private PhoneticSortType m_sortType = PhoneticSortType.Unicode;
		private bool m_advancedEnabled;
		private int[] m_advSortOptions;
		private bool[] m_advRlOptions;
		
		#region Constructor and Loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// PhoneticSortOptions constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneticSortOptions()
		{
			// Keeps track of the Before, Item, & After sorting order
			AdvSortOrder = new int[] { 0, 1, 2 };

			// Keeps track of the R/L checkbox selections
			AdvRlOptions = new bool[] { false, false, false };

			m_sortType = 0; // Unicode sort
		}

		#endregion

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
		/// Gets or sets AdvancedChecked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
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
	}

	#endregion
}
