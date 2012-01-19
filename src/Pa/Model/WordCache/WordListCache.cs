using System.Collections.Generic;
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
		private bool _isCIEList;

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the search pattern used to generate the contents of the cache. When
		/// the cache is not for search results, then this value is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of CIE group headings built when the contents of the cache
		/// represent a list of minimal pairs. When the cache is not for minimal pairs this
		/// list is null. The key of the list is the CIEGroupId and the value is the CIE
		/// group text, or heading, associated with the group id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortedList<int, string> CIEGroupTexts { get; set; }

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
			if (!App.MsgMediator.SendMessage("SortCache", sortInfo))
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
			var sortInfo = new object[] { this, sortOptions };
			if (!App.MsgMediator.SendMessage("SortCache", sortInfo))
				Sort(new CacheSortComparer(sortOptions));
			
			return ascending;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cache is for a search results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForSearchResults { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the cache is for a search result
		/// from a regular expression search.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForRegExpSearchResults { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsCIEList
		{
			get { return _isCIEList; }
			set
			{
				_isCIEList = value;
				if (!_isCIEList)
				{
					foreach (var entry in this)
						entry.ShowInList = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool IsEmpty
		{
			get { return Count == 0; }
		}

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
		/// <param name="savePhonesArray">When true, it forces the phones passed in the phones
		/// parameter to be saved in the new WordListCacheEntry rather than the
		/// new WordListCacheEntry deferring to the phones from its associated WordCacheEntry
		/// (which is passed in the entry parameter).</param>
		/// ------------------------------------------------------------------------------------
		public void Add(WordCacheEntry entry, string[] phones, int offset, int length,
			bool savePhonesArray)
		{
			var newEntry = new WordListCacheEntry();
			newEntry.WordCacheEntry = entry;
			Add(newEntry);

			// When the array of phones is non-existent, it means
			// we're not adding an entry for a search result cache.
			if (phones == null || phones.Length == 0)
				return;

			// Do some preprocessing for spaces before and after.
			ProcessSpaces(ref phones, ref offset, ref length);
			
			IsForSearchResults = true;
			newEntry.SearchItemOffset = offset;
			newEntry.SearchItemLength = length;

			if (savePhonesArray)
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
			var tmpPhones = new List<string>(phones);

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

		#region RegExExperimentation methods
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Uses the offset and length to build three groups of strings, the phones in the
		///// environment before, the phones in the environment after and the phones in the
		///// search item.
		///// </summary>
		///// <param name="entry">The underlying word cache entry from which we're building
		///// a WordListCacheEntry.</param>
		///// <param name="phones">Array of phones that make up the phonetic word. When
		///// this is null or empty, then it's assumed the WordListCacheEntry is not for a
		///// search result item.</param>
		///// <param name="offset">Offset in the phones array where a search item was found.
		///// </param>
		///// <param name="length">Length in phones of the matched search item.</param>
		///// <param name="savePhones">When true, it forces the phones passed in the phones
		///// parameter to be saved in the new WordListCacheEntry rather than the
		///// new WordListCacheEntry deferring to the phones from its associated WordCacheEntry
		///// (which is passed in the entry parameter). This is necessary for entries that
		///// are for words derived from non primary uncertain phones.</param>
		///// ------------------------------------------------------------------------------------
		//public void AddEntryFromRegExpSearch(WordCacheEntry entry, string[] phones,
		//    int offset, int length, bool savePhones)
		//{
		//    WordListCacheEntry newEntry = new WordListCacheEntry();
		//    newEntry.WordCacheEntry = entry;
		//    Add(newEntry);

		//    //// When the array of phones is non-existent, it means
		//    //// we're not adding an entry for a search result cache.
		//    //if (phones == null || phones.Length == 0)
		//    //    return;

		//    newEntry.SearchItemOffset = offset;
		//    newEntry.SearchItemLength = length;

		//    if (savePhones)
		//        newEntry.SetPhones(phones);

		//    //string phonetic = entry.PhoneticValue;

		//    //// Build the environment before string.
		//    //newEntry.EnvironmentBefore = (offset == 0 ? string.Empty :
		//    //    phonetic.Substring(0, offset));

		//    //// Build the environment after string.
		//    //newEntry.EnvironmentAfter = (offset == phonetic.Length - 1 ? string.Empty :
		//    //    phonetic.Substring(offset + length));

		//    //// Build the search item string.
		//    //newEntry.SearchItem = phonetic.Substring(offset, length);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Because it's possible for someone to write a regular expression for the search
		///// item that finds a match that starts and ends in the middle of phones, we need to
		///// go through all the matches and make sure the offset and length for the matches
		///// are extended to include the entire phones for those matches whose offset and
		///// length don't fall on phone boundaries.
		///// </summary>
		///// <remarks>
		///// At the beginning of this method, the SearchItemOffset for each entry in the cache
		///// is the character (i.e. codepoint) offset in the phonetic string where a match was
		///// found. The SearchItemLength for each entry in the cache is the length in characters
		///// (i.e. codepoints) of the match. By the end of the method, SearchItemOffset will
		///// be an offset into the phonetic string's phone collection and SearchItemLength will
		///// be the number of phones in the match.
		///// </remarks>
		///// ------------------------------------------------------------------------------------
		//public void ExtendRegExpMatchesToPhoneBoundaries()
		//{
		//    foreach (var entry in this)
		//    {
		//        int startOffset = entry.SearchItemOffset;
		//        int endOffset = entry.SearchItemOffset + entry.SearchItemLength - 1;
		//        int firstPhoneInMatch = -1;
		//        int lastPhoneInMatch = entry.Phones.Length - 1;
		//        int accumulatedPhoneLengths = 0;

		//        // Go through each phone in the entry's phone collection.
		//        for (int i = 0; i < entry.Phones.Length; i++)
		//        {
		//            int phoneLength = entry.Phones[i].Length;

		//            // Have we yet determined the first full phone in the match?
		//            if (firstPhoneInMatch < 0)
		//            {
		//                // Check if the accumulated phone lengths up to this point plus
		//                // the length of the current phone exceeds the beginning offset
		//                // of the match. If it does, then the current phone should be
		//                // considered the first phone in the match.
		//                if (accumulatedPhoneLengths + phoneLength > startOffset)
		//                    firstPhoneInMatch = i;
		//            }
					
		//            if (accumulatedPhoneLengths + phoneLength > endOffset)
		//            {
		//                // At this point, we know that accumulated phone lengths plus
		//                // the length of the current phone exceeds the offset of the
		//                // last character in the match, so the current phone should be
		//                // considered the last full phone in the match.
		//                lastPhoneInMatch = i;
		//                break;
		//            }

		//            accumulatedPhoneLengths += phoneLength;
		//        }

		//        // Change the matched offset and length. The offset is an offset into the
		//        // phone collection and the length is the number of phone in the match.
		//        entry.SearchItemOffset = firstPhoneInMatch;
		//        entry.SearchItemLength = lastPhoneInMatch - firstPhoneInMatch + 1;

		//        // Build the environment before string.
		//        if (entry.SearchItemOffset > 0)
		//        {
		//            entry.EnvironmentBefore =
		//                string.Join(string.Empty, entry.Phones, 0, entry.SearchItemOffset);
		//        }

		//        // Build the environment after string.
		//        if (entry.SearchItemOffset < entry.Phones.Length - 1)
		//        {
		//            int afterStart = entry.SearchItemOffset + entry.SearchItemLength;
		//            entry.EnvironmentAfter = string.Join(string.Empty, entry.Phones,
		//                afterStart, entry.Phones.Length - afterStart);
		//        }

		//        // Build the search item string.
		//        entry.SearchItem = string.Join(string.Empty, entry.Phones,
		//            entry.SearchItemOffset, entry.SearchItemLength);
		//    }
		//}

		#endregion
	}
}
