using System;
using System.Collections.Generic;
using System.Text;
using SIL.Pa.Data;

namespace SIL.Pa.FFSearchEngine
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneCache : Dictionary<string, IPhoneInfo>
	{
	}

	public class TestPhoneInfo : IPhoneInfo
	{
		private int m_totalCount = 1;
		private int m_countAsNonPrimaryUncertainty = 0;
		private int m_countAsPrimaryUncertainty = 0;
		private IPACharacterType m_charType;
		private ulong m_binaryMask;
		private ulong[] m_masks = new ulong[] { 0, 0 };
		private List<string> m_siblingUncertainties = new List<string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPhoneInfo Clone()
		{
			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of phones found in the same uncertain group(s) with the phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> SiblingUncertainties
		{
			get { return m_siblingUncertainties; }
		}

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
		public int CountAsNonPrimaryUncertainty
		{
			get { return m_countAsNonPrimaryUncertainty; }
			set { m_countAsNonPrimaryUncertainty = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CountAsPrimaryUncertainty
		{
			get { return m_countAsPrimaryUncertainty; }
			set { m_countAsPrimaryUncertainty = value; }
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
			get { return m_masks; }
			set { m_masks = new ulong[] { value[0], value[1] }; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public char BaseCharacter
		{
			get { return '\0';}
			set { ;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string POAKey
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string MOAKey
		{
			get { return null; }
		}
	}
}
