using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The CacheSort defines an alternative comparer for strings. The strings can be sorted
	/// either asc or desc with specified compare options and for a paticular culture.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CacheSortComparer : IComparer<WordListCacheEntry>
	{
		private const char kMOAPOAPadChar = '0';
		private readonly List<SortField> m_sortInfoList;
		private readonly SortOptions m_sortOptions;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The CacheSort constructor.
		/// </summary>
		/// <param name="sortOptions">The PhoneticSortOptions</param>
		/// ------------------------------------------------------------------------------------
		public CacheSortComparer(SortOptions sortOptions)
		{
			m_sortInfoList = sortOptions.SortFields;
			m_sortOptions = sortOptions;
		}

		#region Methods for parsing phonetic for advanced phonetic sorting
		/// ------------------------------------------------------------------------------------
		private void ParsePhoneticForUnicodeCompare(WordListCacheEntry cacheEntry,
			out string before, out string item, out string after)
		{
			var bldrBefore = new StringBuilder();
			var bldrItem = new StringBuilder();
			var bldrAfter = new StringBuilder();

			if (cacheEntry.Phones != null)
			{
				int firstAfterPhone = cacheEntry.SearchItemOffset +
					cacheEntry.SearchItemLength;

				for (int i = 0; i < cacheEntry.Phones.Length; i++)
				{
					if (i < cacheEntry.SearchItemOffset)
					{
						if (m_sortOptions.AdvRlOptions[0])
							bldrBefore.Insert(0, cacheEntry.Phones[i]);
						else
							bldrBefore.Append(cacheEntry.Phones[i]);
					}
					else if (i >= firstAfterPhone)
					{
						if (m_sortOptions.AdvRlOptions[2])
							bldrAfter.Insert(0, cacheEntry.Phones[i]);
						else
							bldrAfter.Append(cacheEntry.Phones[i]);
					}
					else
					{
						if (m_sortOptions.AdvRlOptions[1])
							bldrItem.Insert(0, cacheEntry.Phones[i]);
						else
							bldrItem.Append(cacheEntry.Phones[i]);
					}
				}
			}

			before = (bldrBefore.Length == 0 ? null : bldrBefore.ToString());
			item = (bldrItem.Length == 0 ? null : bldrItem.ToString());
			after = (bldrAfter.Length == 0 ? null : bldrAfter.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the MOA or POA key for the phones in the specified entry. This method builds
		/// the MOA or POA key for non-advanced phonetic searches.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int CompareMOAOrPOAKeys(WordListCacheEntry x, WordListCacheEntry y)
		{
			if (x == null && y == null)
				return 0;

			if (x == null)
				return -1;

			if (y == null)
				return 1;
			
			var bldrXKey = new StringBuilder();
			var bldrYKey = new StringBuilder();
				
			IPhoneInfo phoneInfo;
			string xkey;
			string ykey;

			int xPhoneCount = (x.Phones != null ? x.Phones.Length : 0);
			int yPhoneCount = (y.Phones != null ? y.Phones.Length : 0);

			// Loop through the phones in each entry and assemble a hex key for them.
			for (int i = 0; i < xPhoneCount || i < yPhoneCount; i++)
			{
				xkey = string.Empty;
				ykey = string.Empty;

				// Build the key for the current phone in the 'x' entry.
				if (i < xPhoneCount)
				{
					phoneInfo = x.WordCacheEntry.Project.PhoneCache[x.Phones[i]];
					if (phoneInfo != null)
					{
						xkey = (m_sortOptions.SortType == PhoneticSortType.MOA ?
							phoneInfo.MOAKey : phoneInfo.POAKey);
					}
				}

				// Build the key for the current phone in the 'y' entry.
				if (i < yPhoneCount)
				{
					phoneInfo = x.WordCacheEntry.Project.PhoneCache[y.Phones[i]];
					if (phoneInfo != null)
					{
						ykey = (m_sortOptions.SortType == PhoneticSortType.MOA ?
							phoneInfo.MOAKey : phoneInfo.POAKey);
					}
				}

				// Make sure the keys for each phone are the same length.
				if (xkey.Length < ykey.Length)
					xkey = xkey.PadRight(ykey.Length, kMOAPOAPadChar);
				else if (ykey.Length < xkey.Length)
					ykey = ykey.PadRight(xkey.Length, kMOAPOAPadChar);

				bldrXKey.Append(xkey);
				bldrYKey.Append(ykey);
			}

			return (string.CompareOrdinal(bldrXKey.ToString(), bldrYKey.ToString()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method combines all the keys for each entry list into a single key. Along the
		/// way, the individual keys in each list are padded with zeros as necessary so keys in
		/// each list at any given index in the two lists are the same length. The
		/// "as necessary" means, for example, that if a key at index 3 in xKeys is longer
		/// than the key at index 3 in yKeys, then yKeys[3] is padded with zeros so its length
		/// is equal to the key in xKeys[3]. Also, when one list of keys runs out before the
		/// other, zero-filled keys are inserted into the shorter of the two lists so there
		/// are the same number of keys in each list. Note: This method is used for advanced
		/// phonetic sorting on MOA or POA.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ModifyAndCombineKeys(IList<string> xKeys, IList<string> yKeys,
			out string xKey, out string yKey)
		{
			var bldrXKey = new StringBuilder();
			var bldrYKey = new StringBuilder();

			// Loop through the phones in each entry and assemble a hex key for them.
			for (int i = 0; i < xKeys.Count || i < yKeys.Count; i++)
			{
				if (i >= xKeys.Count)
					xKeys.Add(string.Empty.PadRight(yKeys[i].Length, kMOAPOAPadChar));
				else if (i >= yKeys.Count)
					yKeys.Add(string.Empty.PadRight(xKeys[i].Length, kMOAPOAPadChar));
				else
				{
					if (xKeys[i].Length < yKeys[i].Length)
						xKeys[i] = xKeys[i].PadRight(yKeys[i].Length, kMOAPOAPadChar);
					else if (yKeys[i].Length < xKeys[i].Length)
						yKeys[i] = yKeys[i].PadRight(xKeys[i].Length, kMOAPOAPadChar);
				}

				bldrXKey.Append(xKeys[i]);
				bldrYKey.Append(yKeys[i]);
			}

			xKey = bldrXKey.ToString();
			yKey = bldrYKey.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the POA or MOA hex keys for the pieces (i.e. before, item and after) of a 
		/// phonetic value generated from a search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetMOAOrPOAKeysForPhoneticCompare(WordListCacheEntry cacheEntry,
			out List<string> before, out List<string> item, out List<string> after)
		{
			IPhoneInfo phoneInfo;
			before = new List<string>();
			item = new List<string>();
			after = new List<string>();

			int srchItemOffset = cacheEntry.SearchItemOffset;
			int afterEnvOffset = srchItemOffset + cacheEntry.SearchItemLength;

			// Loop through the phones and assemble a hex key for the phones in the entry
			// according to the current sort options (i.e. MOA or POA, R/L or L/R).
			for (int i = 0; i < cacheEntry.Phones.Length; i++)
			{
				phoneInfo = cacheEntry.WordCacheEntry.Project.PhoneCache[cacheEntry.Phones[i]];
				if (phoneInfo == null)
					continue;

				string phoneKey = (m_sortOptions.SortType == PhoneticSortType.MOA ?
					phoneInfo.MOAKey : phoneInfo.POAKey);

				// Determine in what environment the current phone is found.
				if (i >= afterEnvOffset)
					after.Add(phoneKey);
				else if (i >= srchItemOffset)
					item.Add(phoneKey);
				else
					before.Add(phoneKey);
			}

			// Reverse those lists of keys that are supposed to be sorted right-to-left.
			if (m_sortOptions.AdvRlOptions[0])
			    before.Reverse();
			if (m_sortOptions.AdvRlOptions[1])
			    item.Reverse();
			if (m_sortOptions.AdvRlOptions[2])
			    after.Reverse();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares the 3 phonetic pieces of two search result entries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int CompareAdvancedPhonetic(WordListCacheEntry x, WordListCacheEntry y)
		{
			string beforeEnvX, searchItemX, afterEnvX;
			string beforeEnvY, searchItemY, afterEnvY;

			List<string> xBefore, xItem, xAfter;
			List<string> yBefore, yItem, yAfter;
			GetMOAOrPOAKeysForPhoneticCompare(x, out xBefore, out xItem, out xAfter);
			GetMOAOrPOAKeysForPhoneticCompare(y, out yBefore, out yItem, out yAfter);

			ModifyAndCombineKeys(xBefore, yBefore, out beforeEnvX, out beforeEnvY);
			ModifyAndCombineKeys(xItem, yItem, out searchItemX, out searchItemY);
			ModifyAndCombineKeys(xAfter, yAfter, out afterEnvX, out afterEnvY);

			for (int i = 0; i < 3; i++)
			{
				// Compare the environments before.
				if (m_sortOptions.AdvSortOrder[0] == i)
				{
					if (beforeEnvX == beforeEnvY)
						continue;

					return string.CompareOrdinal(beforeEnvX, beforeEnvY);
				}

				// Compare the search items.
				if (m_sortOptions.AdvSortOrder[1] == i)
				{
					if (searchItemX == searchItemY)
						continue;

					return string.CompareOrdinal(searchItemX, searchItemY);
				}

				// Compare the environments after.
				if (m_sortOptions.AdvSortOrder[2] == i)
				{
					if (afterEnvX == afterEnvY)
						continue;

					return string.CompareOrdinal(afterEnvX, afterEnvY);
				}
			}

			return 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare and sort two Phonetic WordListCacheEntries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int ComparePhonetic(WordListCacheEntry x, WordListCacheEntry y)
		{
			if (m_sortOptions == null)
				return 0;

			if (m_sortOptions.AdvancedEnabled)
				return CompareAdvancedPhonetic(x, y);

			// Compare POA or MOA keys.
			return CompareMOAOrPOAKeys(x, y);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare references.
		/// Treat the reference field specially. When we find two equal references,
		/// then we need to sort secondarily by the order in which the words are
		/// found in the record. In addition, if the entry is from a search result
		/// then add a third level of compare on the search item's offset.
		/// </summary>
		/// <param name="x">WordListCacheEntry</param>
		/// <param name="y">WordListCacheEntry</param>
		/// ------------------------------------------------------------------------------------
		public int CompareReferences(WordListCacheEntry x, WordListCacheEntry y)
		{
			int compareResult = x.WordCacheEntry.WordIndex - y.WordCacheEntry.WordIndex;
			
			return (compareResult != 0 ? compareResult :
				x.SearchItemOffset - y.SearchItemOffset);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare dates.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CompareDates(string fieldValue1, string fieldValue2)
		{
			if (fieldValue1 == null && fieldValue2 == null)
				return 0;

			DateTime dateTime1;
			DateTime dateTime2;
			
			if (fieldValue1 == null || !DateTime.TryParse(fieldValue1, out dateTime1))
				return -1;
			
			if (fieldValue2 == null || !DateTime.TryParse(fieldValue2, out dateTime2))
				return 1;

			return DateTime.Compare(dateTime1, dateTime2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare numerics.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CompareNumerics(string fieldValue1, string fieldValue2)
		{
			if (fieldValue1 == null && fieldValue2 == null)
				return 0;

			int number1;
			int number2;
			
			if (fieldValue1 == null || !Int32.TryParse(fieldValue1, out number1))
				return -1;

			if (fieldValue2 == null || !Int32.TryParse(fieldValue2, out number2))
				return 1;
			
			return number1 - number2;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare strings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CompareStrings(string fieldValue1, string fieldValue2)
		{
			if (fieldValue1 == null && fieldValue2 == null)
				return 0;
			if (fieldValue1 == null)
				return -1;
			if (fieldValue2 == null)
				return 1;

			return string.Compare(fieldValue1, fieldValue2, true, CultureInfo.CurrentCulture);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare and sort two WordListCacheEntries.
		/// Does a nested comparison (both in asc & desc sort order) of multiple columns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Compare(WordListCacheEntry x, WordListCacheEntry y)
		{
			if (x == null && y == null)
				return 0;

			bool ascending = (m_sortInfoList.Count > 0 ? m_sortInfoList[0].Ascending : true);

			if (x == null)
				return (ascending ? -1 : 1);

			if (y == null)
				return (ascending ? 1 : -1);
			
			// First compare CIE group Id's before anything else.
			if (x.CIEGroupId >= 0 && y.CIEGroupId >= 0 && x.CIEGroupId != y.CIEGroupId)
				return (x.CIEGroupId - y.CIEGroupId);

			// Continue with the next iteration if the fieldValues are EQUAL
			foreach (var si in m_sortInfoList)
			{
				ascending = si.Ascending;
				int compareResult;

				// Use a special comparison for phonetic fields.
				if (si.Field.Type == FieldType.Phonetic)
				{
					compareResult = ComparePhonetic(x, y);
					if (compareResult == 0)
						continue;
					
					return (ascending ? compareResult : -compareResult);
				}

				var fieldValue1 = x[si.Field.Name];
				var fieldValue2 = y[si.Field.Name];

				if (fieldValue1 == fieldValue2)
				{
					// Use a special comparison for references.
					if (si.Field.Type == FieldType.Reference)
					{
						compareResult = CompareReferences(x, y);
						if (compareResult == 0)
							continue;

						return (ascending ? compareResult : -compareResult);
					}

					// If we're sorting by the entry's audio file and the audio file for each
					// entry is the same, then compare the order in which the words occur within
					// the sound file transcription.
					if (si.Field.Type == FieldType.AudioFilePath &&
						x.WordCacheEntry.WordIndex != y.WordCacheEntry.WordIndex)
					{
						compareResult = x.WordCacheEntry.WordIndex - y.WordCacheEntry.WordIndex;
						return (ascending ? compareResult : -compareResult);
					}

					// Fields are equal, so continue onto the next comparison column
					continue;
				}

				// Check for date or numeric fields and compare appropriately.
				if (si.Field.Type == FieldType.Date)
					compareResult = CompareDates(fieldValue1, fieldValue2);
				else if (si.Field.Type == FieldType.GeneralNumeric)
					compareResult = CompareNumerics(fieldValue1, fieldValue2);
				else
					compareResult = CompareStrings(fieldValue1, fieldValue2);

				if (compareResult == 0)
					continue;

				// Return a negative value for descending order
				return (ascending ? compareResult : -compareResult);
			}

			return 0; // They are equal
		}
	}
}
