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
	/// The CachePhoneticSort defines an alternative comparer for strings. The strings can be sorted
	/// either asc or desc with specified compare options and for a paticular culture.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneticSortComparer : IComparer<WordListCacheEntry>
	{
		private SortInformationList m_sortInfoList;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The CacheSort constructor.
		/// </summary>
		/// <param name="sortInfoList">The SortInformationList</param>
		/// ------------------------------------------------------------------------------------
		public PhoneticSortComparer(SortInformationList sortInfoList)
		{
			m_sortInfoList = sortInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare and sort two WordListCacheEntries.
		/// Does a nested comparison (both in asc & desc sort order) of multiple columns.
		/// </summary>
		/// <param name="x">WordListCacheEntry</param>
		/// <param name="y">WordListCacheEntry</param>
		/// ------------------------------------------------------------------------------------
		public int Compare(WordListCacheEntry x, WordListCacheEntry y)
		{
			int compareResult = 0;
			BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance;

			// Continue with the next iteration if the fieldValues are EQUAL
			//for (int i = 0; i < m_sortInfoList.Count; i++)
			//{
			//string fieldValue1 = x.GetType().InvokeMember(m_sortInfoList[i].sortField,
			//    flags, null, x, null) as string;

			//string fieldValue2 = y.GetType().InvokeMember(m_sortInfoList[i].sortField,
			//    flags, null, y, null) as string;


			string fieldValue1 = x.POAKey;
			string fieldValue2 = y.POAKey;

			string fieldValue3;
			string fieldValue4;
			if (x.SearchItem != null)
				fieldValue3 = x.POAKey.Substring(x.SearchItemPhoneOffset, x.SearchItem.Length);
			if (y.SearchItem != null)
				fieldValue4 = y.POAKey.Substring(y.SearchItemPhoneOffset, y.SearchItem.Length);

			//if (fieldValue1 == fieldValue2) continue;
			if (fieldValue1 == fieldValue2) compareResult = 0;
			if (fieldValue1 == null) compareResult = -1;
			if (fieldValue2 == null) compareResult = 1;

			compareResult = String.Compare(fieldValue1, fieldValue2, StringComparison.Ordinal);

			// Sort with Ordinal if Phonetic column else use the CurrentCulture
			//compareResult = (m_sortInfoList[i].sortField == DBFields.Phonetic ?
			//    String.Compare(fieldValue1, fieldValue2, StringComparison.Ordinal) :
			//    String.Compare(fieldValue1, fieldValue2, true, System.Globalization.CultureInfo.CurrentCulture));

			// Return a negative value for descending order
			//return (m_sortInfoList[i].ascending ? compareResult : -compareResult);
			return compareResult;
			//}

			//return 0; // They are equal
		}
	}
}
