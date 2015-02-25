// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace SIL.Pa.SearchResultAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CAESorter : IComparer<WordListCacheEntry>
	{
		private string[] m_xItemPhones;
		private string[] m_xBeforePhones;
		private string[] m_xAfterPhones;

		private string[] m_yItemPhones;
		private string[] m_yBeforePhones;
		private string[] m_yAfterPhones;

		#region IComparer<WordListCacheEntry> Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Compare(WordListCacheEntry x, WordListCacheEntry y)
		{
			//GetParts(x, out m_xItemPhones, out m_xBeforePhones, out m_xAfterPhones);


			return 0;

		}
		
		#endregion

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void GetParts(WordListCacheEntry wlentry, out string[] itemPhones,
		//    out string[] beforePhones, out string[] afterPhones)
		//{
		//    int srchItemOffset = wlentry.SearchItemOffset;
		//    int srchItemLength = wlentry.SearchItemLength;
		//    int envAfterOffset = srchItemOffset + srchItemLength;

		//    // Make sure the search item offset + length doesn't exceed the number of
		//    // phones in the entry. This could happen with some searches (see PA-755).
		//    if (srchItemOffset + srchItemLength > wlentry.Phones.Length)
		//        srchItemLength = Math.Abs(wlentry.Phones.Length - srchItemOffset);

		//    string srchItem;

		//    try
		//    {
		//        // Get the text that makes up the search item.
		//        // This is used only to measure it's width.
		//        srchItem = string.Join(string.Empty, wlentry.Phones,
		//            srchItemOffset, srchItemLength);
		//    }
		//    catch
		//    {
		//        srchItem = (wlentry.SearchItem == null ? string.Empty : wlentry.SearchItem);
		//    }

		//}
	}
}
