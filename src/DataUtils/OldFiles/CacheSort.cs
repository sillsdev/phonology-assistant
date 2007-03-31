using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The CacheSort defines an alternative comparer for strings. The strings can be sorted
	/// either asc or desc with specified compare options and for a paticular culture.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CacheSort : IComparer<WordListCacheEntry>
	{
		private SortInformationList m_sortInfoList;
		private int compareResult = 0;

		// Use the current culture compare information
		private CompareInfo compareInfo =
			System.Globalization.CultureInfo.CurrentCulture.CompareInfo;
		// "Ordinal" compares strings based on the Unicode values of each element of the string
		private CompareOptions compareOptions = CompareOptions.Ordinal;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The CacheSort constructor.
		/// </summary>
		/// <param name="sortInfoList">The SortInformationList</param>
		/// ------------------------------------------------------------------------------------
		public CacheSort(SortInformationList sortInfoList)
		{
			m_sortInfoList = sortInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare and sort two WordListCacheEntries.
		/// </summary>
		/// <param name="x">WordListCacheEntry</param>
		/// <param name="y">WordListCacheEntry</param>
		/// ------------------------------------------------------------------------------------
		public int Compare(WordListCacheEntry x, WordListCacheEntry y)
		{
			BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance;

			// Continue with the next iteration if the fieldValues are EQUAL
			for (int i = 0; i < m_sortInfoList.Count; i++)
			{
				string fieldValue1 = x.GetType().InvokeMember(m_sortInfoList[i].sortField,
					flags, null, x, null) as string;

				string fieldValue2 = y.GetType().InvokeMember(m_sortInfoList[i].sortField,
					flags, null, y, null) as string;

				if (fieldValue1 == fieldValue2) continue;
				if (fieldValue1 == null) compareResult = -1;
				if (fieldValue2 == null) compareResult = 1;
				compareResult = compareInfo.Compare(fieldValue1, fieldValue2, compareOptions);

				if (m_sortInfoList[i].sortDirection)
					return compareResult; // ascending sort
				else
					return (-compareResult); // descending sort
			}

			return 0; // They are equal
		}
	}
}
