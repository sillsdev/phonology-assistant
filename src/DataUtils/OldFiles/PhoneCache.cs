using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using SIL.SpeechTools.Database;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneCache : Dictionary<string, IPhoneInfo>
	{
	}

	#region PhoneInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A class defining an object to store the information for a single phonetic character.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneInfo : SIL.SpeechTools.Database.IPhoneInfo
	{
		private int m_totalCount = 1;
		private IPACharacterType m_charType;
		private ulong m_binaryMask;
		private ulong[] m_masks = new ulong[] { 0, 0 };

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int TotalCount
		{
			get { return m_totalCount; }
			set { m_totalCount = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharacterType CharType
		{
			get { return m_charType; }
			set { m_charType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ulong BinaryMask
		{
			get { return m_binaryMask; }
			set { m_binaryMask = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the articulatory feature masks for the phonetic character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ulong[] Masks
		{
			get {return m_masks;}
			set {m_masks = new ulong[] {value[0], value[1] };}
		}
	}

	#endregion
}
