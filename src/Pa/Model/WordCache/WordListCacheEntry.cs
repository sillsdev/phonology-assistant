
namespace SIL.Pa.Model
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
		private string[] m_phones;

		/// ------------------------------------------------------------------------------------
		public WordListCacheEntry()
		{
			CIEGroupId = -1;
			ShowInList = true;
			EnvironmentAfter = null;
			SearchItem = null;
			EnvironmentBefore = null;
			Tag = null;
		}

		#region Indexer overloads
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value for the specified field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string this[string field]
		{
			get { return WordCacheEntry.GetField(field); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value for the field specified by fieldInfo
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
		public object Tag { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's the phonetic field's value for the word list cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PhoneticValue
		{
			get { return WordCacheEntry.PhoneticValue; }
		}

		/// ------------------------------------------------------------------------------------
		public WordCacheEntry WordCacheEntry { get; set; }

		/// ------------------------------------------------------------------------------------
		public string EnvironmentBefore { get; set; }

		/// ------------------------------------------------------------------------------------
		public string SearchItem { get; set; }

		/// ------------------------------------------------------------------------------------
		public string EnvironmentAfter { get; set; }

		/// ------------------------------------------------------------------------------------
		public int SearchItemOffset { get; set; }

		/// ------------------------------------------------------------------------------------
		public int SearchItemLength { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the array of phones that make up the phonetic word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Phones
		{
			get { return (m_phones ?? WordCacheEntry.Phones); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show this in a filtered cache
		/// list. This is used on a temporary basis when building temporary cache lists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal bool ShowInList { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating an entry's CIE group. The id is arbitrary but must
		/// be the same as all other entries in the same group and Ids must be unique by group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CIEGroupId { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the word is visible in the current filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MatchesCurrentFilter { get; set; }

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
