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
	public class CacheSortComparer : IComparer<WordListCacheEntry>
	{
		private SortInformationList m_sortInfoList;
		private PhoneticSortOptions m_phoneticSortOptions;
		private StringBuilder m_sbCompareFieldX;
		private StringBuilder m_sbCompareFieldY;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The CacheSort constructor.
		/// </summary>
		/// <param name="sortInfoList">The SortInformationList</param>
		/// ------------------------------------------------------------------------------------
		public CacheSortComparer(SortInformationList sortInfoList)
		{
			m_sortInfoList = sortInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The CacheSort constructor.
		/// </summary>
		/// <param name="sortInfoList">The SortInformationList</param>
		/// ------------------------------------------------------------------------------------
		public CacheSortComparer(SortInformationList sortInfoList, object args)
		{
			m_sortInfoList = sortInfoList;
			m_phoneticSortOptions = args as PhoneticSortOptions;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Build the X and Y compare fields.
		/// </summary>
		/// <param name="xKey">The x string to compare with</param>
		/// <param name="yKey">The y string to compare with</param>
		/// <param name="rightToLeft">True if comparing right to left</param>
		/// ------------------------------------------------------------------------------------
		public void BuildCompareFields(string xKey, string yKey, PhoneticSortType sortType,
			bool rightToLeft)
		{
			if (!rightToLeft)
			{
				m_sbCompareFieldX.Insert(m_sbCompareFieldX.Length, xKey);
				m_sbCompareFieldY.Insert(m_sbCompareFieldY.Length, yKey);
			}
			else
			{
				// Reverse the key
				StringBuilder sbRlX = new StringBuilder();
				StringBuilder sbRlY = new StringBuilder();

				if (sortType == PhoneticSortType.Unicode)
				{
					// Reverse 1 code point at a time
					for (int ix = 0; ix < xKey.Length; ix++)
						sbRlX.Insert(0, xKey[ix]);

					for (int iy = 0; iy < yKey.Length; iy++)
						sbRlY.Insert(0, yKey[iy]);
				}
				// POA & MOA Sorting
				else
				{
					// POA & MOA Keys have to be reversed 3 code points at a time
					for (int ix = 0; ix < xKey.Length;)
					{
						sbRlX.Insert(0, xKey.Substring(ix,3));
						ix += 3;
					}

					for (int iy = 0; iy < yKey.Length;)
					{
						sbRlY.Insert(0, yKey.Substring(iy, 3));
						iy += 3;
					}
				}

				m_sbCompareFieldX.Insert(m_sbCompareFieldX.Length, sbRlX);
				m_sbCompareFieldY.Insert(m_sbCompareFieldY.Length, sbRlY);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parse the cacheEntry's hex key returning the before env, search item, and after env.
		/// </summary>
		/// <param name="cacheEntry">The WordListCacheEntry to parse</param>
		/// ------------------------------------------------------------------------------------
		public void ParseHexKey(WordListCacheEntry cacheEntry, PhoneticSortType sortType,
			out string before, out string item, out string after)
		{
			if (sortType == PhoneticSortType.Unicode)
			{
				before = cacheEntry.EnvironmentBefore;
				item = cacheEntry.SearchItem;
				after = cacheEntry.EnvironmentAfter;
				return;
			}

			int beforeEnvLength = 0;
			for (int i = 0; i < cacheEntry.SearchItemPhoneOffset; i++)
				beforeEnvLength += cacheEntry.PhoneticChars[i].Length;

			// Multiply 3 to get the Hex length. Each xxx is a code pnt
			beforeEnvLength *= 3;

			int searchItemLength = cacheEntry.SearchItem.Length * 3;
			int afterEnvStartCodePoint = beforeEnvLength + searchItemLength;
			int afterEnvLength = cacheEntry.MOAKey.Length - afterEnvStartCodePoint;

			if (sortType == PhoneticSortType.MOA)
			{
				// Sort by manner of articulation
				before = cacheEntry.MOAKey.Substring(0, beforeEnvLength);
				item = cacheEntry.MOAKey.Substring(beforeEnvLength, searchItemLength);
				after = cacheEntry.MOAKey.Substring(afterEnvStartCodePoint, afterEnvLength);
			}
			else
			{
				// Sort by place of articulation
				before = cacheEntry.POAKey.Substring(0, beforeEnvLength);
				item = cacheEntry.POAKey.Substring(beforeEnvLength, searchItemLength);
				after = cacheEntry.POAKey.Substring(afterEnvStartCodePoint, afterEnvLength);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare and sort two WordListCacheEntries.
		/// Does a nested comparison (both in asc & desc sort order) of multiple columns.
		/// </summary>
		/// <param name="x">WordListCacheEntry</param>
		/// <param name="y">WordListCacheEntry</param>
		/// ------------------------------------------------------------------------------------
		public int ComparePhonetic(int i, WordListCacheEntry x, WordListCacheEntry y)
		{
			string fieldValue1 = String.Empty;
			string fieldValue2 = String.Empty;
			int compareResult = 0;

			if (m_phoneticSortOptions != null)
			{
				// Sort by Advanced Options
				if (m_phoneticSortOptions.AdvancedEnabled)
				{
					PhoneticSortType sortType = m_phoneticSortOptions.SortType;
					string before;
					string item;
					string after;

					ParseHexKey(x, sortType, out before, out item, out after);

					string beforeEnvX = before;
					string searchItemX = item;
					string afterEnvX = after;

					ParseHexKey(y, sortType, out before, out item, out after);

					string beforeEnvY = before;
					string searchItemY = item;
					string afterEnvY = after;

					m_sbCompareFieldX = new StringBuilder();
					m_sbCompareFieldY = new StringBuilder();

					// Build the sort strings in the proper order
					for (int i2 = 0; i2 < 3; i2++)
					{
						if (m_phoneticSortOptions.AdvSortOrder[0] == i2)
							BuildCompareFields(beforeEnvX, beforeEnvY, sortType, m_phoneticSortOptions.AdvRlOptions[0]);
						else if (m_phoneticSortOptions.AdvSortOrder[1] == i2)
							BuildCompareFields(searchItemX, searchItemY, sortType, m_phoneticSortOptions.AdvRlOptions[1]);
						else if (m_phoneticSortOptions.AdvSortOrder[2] == i2)
							BuildCompareFields(afterEnvX, afterEnvY, sortType, m_phoneticSortOptions.AdvRlOptions[2]);
					}

					fieldValue1 = m_sbCompareFieldX.ToString();
					fieldValue2 = m_sbCompareFieldY.ToString();
				}

				// Sort by Manner of Articulation
				else if (m_phoneticSortOptions.SortType == PhoneticSortType.MOA)
				{
					fieldValue1 = x.MOAKey;
					fieldValue2 = y.MOAKey;
				}

				// Sort by Place of Articulation
				else if (m_phoneticSortOptions.SortType == PhoneticSortType.POA)
				{
					fieldValue1 = x.POAKey;
					fieldValue2 = y.POAKey;
				}
			}

			// Default - Sort by Unicode Index
			if (fieldValue1 == String.Empty && fieldValue2 == String.Empty)
			{
				BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance;

				//fieldValue1 = x.GetType().InvokeMember(m_sortInfoList[i].sortField,
				fieldValue1 = x.GetType().InvokeMember(
					DBFields.Phonetic, flags, null, x, null) as string;
				fieldValue2 = y.GetType().InvokeMember(
					DBFields.Phonetic, flags, null, y, null) as string;
			}

			if (fieldValue1 == fieldValue2) return 0;
			if (fieldValue1 == null) compareResult = -1;
			if (fieldValue2 == null) compareResult = 1;
			if (compareResult == 0)
				compareResult = String.Compare(fieldValue1, fieldValue2, StringComparison.Ordinal);

			// Return a negative value for descending order
			return (m_sortInfoList[i].ascending ? compareResult : -compareResult);
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
			BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public |
				BindingFlags.GetProperty | BindingFlags.Instance;

			// Continue with the next iteration if the fieldValues are EQUAL
			for (int i = 0; i < m_sortInfoList.Count; i++)
			{
				compareResult = 0;
				if (m_sortInfoList[i].sortField == DBFields.Phonetic)
				{
					compareResult = ComparePhonetic(i, x, y);
					if (compareResult == 0)
						continue;
					else
						return compareResult;
				}

				string fieldValue1 = x.GetType().InvokeMember(m_sortInfoList[i].sortField,
				    flags, null, x, null) as string;
				string fieldValue2 = y.GetType().InvokeMember(m_sortInfoList[i].sortField,
				    flags, null, y, null) as string;

				if (fieldValue1 == fieldValue2) continue;
				if (fieldValue1 == null) compareResult = -1;
				if (fieldValue2 == null) compareResult = 1;
				if (compareResult == 0)
					compareResult = String.Compare(fieldValue1, fieldValue2, true, CultureInfo.CurrentCulture);

				// Return a negative value for descending order
				return (m_sortInfoList[i].ascending ? compareResult : -compareResult);
			}

			return 0; // They are equal
		}
	}
}
