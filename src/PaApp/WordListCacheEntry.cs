using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SIL.Pa.Data;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class contains a sinlge item in a word list window grid for find phone search
	/// results. It contains a reference to the underlying WordListEntry it represents and
	/// most of it's properties just return a reference to the underlying WordListEntry.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordListCacheEntry
	{
		private WordCacheEntry m_wordRec;
		private string[] m_phones = null;
		private string m_envBefore = null;
		private string m_srchItem = null;
		private string m_envAfter = null;
		private int m_srchItemPhoneOffset;
		private int m_srchItemPhonesLength;
		private bool m_showInList = true;
		private int m_cieGroupId = -1;

		#region Indexer overloads
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string this[string field]
		{
			get { return m_wordRec.GetField(field); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string this[PaFieldInfo fieldInfo]
		{
			get { return (fieldInfo == null ? null : this[fieldInfo.FieldName]); }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Normally, the phones for the WordListCacheEntry come from m_wordRec.Phones, but
		/// when the entry needs to contain non primary uncertain phones, then this method
		/// provides a way to give the entry its own collection of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetPhones(string[] phones)
		{
			if (phones != null && phones.Length > 0)
				m_phones = phones;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's the phonetic field's value for the word list cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PhoneticValue
		{
			get { return m_wordRec.PhoneticValue; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordCacheEntry WordCacheEntry
		{
			get { return m_wordRec; }
			set { m_wordRec = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EnvironmentBefore
		{
			get { return m_envBefore; }
			set { m_envBefore = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SearchItem
		{
			get { return m_srchItem; }
			set { m_srchItem = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EnvironmentAfter
		{
			get { return m_envAfter; }
			set { m_envAfter = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SearchItemPhoneOffset
		{
			get { return m_srchItemPhoneOffset; }
			set { m_srchItemPhoneOffset = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SearchItemPhoneLength
		{
			get { return m_srchItemPhonesLength; }
			set { m_srchItemPhonesLength = value; }
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the manner of articulation key for the entry.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string MOAKey
		//{
		//    get	{return m_wordRec.MOAKey;}
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the place of articulation key for the entry.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string POAKey
		//{
		//    get	{return m_wordRec.POAKey;}
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the array of phones that make up the phonetic word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Phones
		{
			get { return (m_phones == null ? m_wordRec.Phones : m_phones); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show this in a filtered cache
		/// list. This is used on a temporary basis when building temporary cache lists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal bool ShowInList
		{
			get { return m_showInList; }
			set { m_showInList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating an entry's group. The id is arbitrary but must be
		/// the same as all other entries in the same group and Ids must be unique by group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CIEGroupId
		{
			get { return m_cieGroupId; }
			set { m_cieGroupId = value; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the entry's phonetic value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return PhoneticValue;
		}
	}
}
