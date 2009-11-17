// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FeatureMask.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Text;

namespace SIL.Pa.Data
{
	#region FeatureMask class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FeatureMask
	{
		private readonly UInt64[] m_masks;
		private readonly int m_size;
		private readonly int m_maskCount;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureMask"/> class.
		/// </summary>
		/// <param name="size">The number of bits in the mask.</param>
		/// ------------------------------------------------------------------------------------
		public FeatureMask(int size)
		{
			if (size <= 0)
				throw new ArgumentException("Mask size may not be negative.");

			int remainder;
			m_maskCount = Math.DivRem(size, 64, out remainder);
			if (remainder > 0)
				m_maskCount++;

			m_size = size;
			m_masks = new UInt64[m_maskCount];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureMask Clone()
		{
			FeatureMask clone = new FeatureMask(m_size);
			m_masks.CopyTo(clone.m_masks, 0);
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all the bits are set in the mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsEmpty
		{
			get { return (m_masks.Count(x => x > 0) == 0); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of bits in the mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Size
		{
			get { return m_size; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the true/false value of the specified bit in the mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool this[int bit]
		{
			get
			{
				if (bit >= m_size || bit < 0)
				{
					throw new IndexOutOfRangeException("bit " + bit +
						" is out of range. Bit must be >= 0 and < " + m_size + ".");
				}

				int actualBit;
				int maskNum = Math.DivRem(bit, 64, out actualBit);
				UInt64 tmpMask = 1;
				tmpMask <<= actualBit;
				return ((m_masks[maskNum] & tmpMask) > 0);
			}
			set
			{
				if (bit >= m_size || bit < 0)
				{
					throw new IndexOutOfRangeException("bit " + bit +
						" is out of range. Bit must be >= 0 and < " + m_size + ".");
				}

				int actualBit;
				int maskNum = Math.DivRem(bit, 64, out actualBit);
				UInt64 tmpMask = 1;
				tmpMask <<= actualBit;

				if (value)
					m_masks[maskNum] |= tmpMask;
				else
					m_masks[maskNum] &= ~tmpMask;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the bits in the mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			for (int i = 0; i < m_maskCount; i++)
				m_masks[i] = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Ands the mask with the specified mask and returns a value indicating whether or
		/// not the bitwise AND operation yields a non-zero result.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AndResult(FeatureMask mask)
		{
			if (m_maskCount != mask.m_maskCount)
				throw new ArgumentException("Bit count mismatch: both masks must contain same number of bits.");

			for (int i = 0; i < m_maskCount; i++)
			{
				if ((m_masks[i] & mask.m_masks[i]) > 0)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// ORs the the mask with the specified mask and returns a value indicating whether or
		/// not the bitwise AND operation yields a non-zero result.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FeatureMask operator |(FeatureMask m1, FeatureMask m2)
		{
			if (m1.m_size != m2.m_size)
				throw new ArgumentException("Bit count mismatch: both masks must contain same number of bits.");

			var result = new FeatureMask(m1.m_size);

			for (int i = 0; i < m1.m_maskCount; i++)
				result.m_masks[i] = (m1.m_masks[i] | m2.m_masks[i]);

			return result;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			StringBuilder bldr = new StringBuilder();

			for (int i = m_maskCount - 1; i >= 0; i--)
			{
				if (m_masks[i] > 0 || bldr.Length > 0)
					bldr.AppendFormat("{0:X16}, ", m_masks[i]);
			}

			// Get rid of trailing comma and space.
			bldr.Length -= 2;
			return bldr.ToString();
		}
	}

	#endregion
}
