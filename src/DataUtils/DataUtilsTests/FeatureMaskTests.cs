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
// File: FeatureMaskTests.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using NUnit.Framework;
using SilUtils;

namespace SIL.Pa.Data
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FeatureMaskTests
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the that a correct number of long integers is allocated for holding the
		/// specified number of bits.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyInternalMaskArraySize()
		{
			// Make the mask 64 bits long.
			var fmask = new FeatureMask(64);
			Assert.AreEqual(1, ReflectionHelper.GetField(fmask, "m_maskCount"));

			// Make the mask 65 bits long.
			fmask = new FeatureMask(65);
			Assert.AreEqual(2, ReflectionHelper.GetField(fmask, "m_maskCount"));

			// Make the mask 128 bits long.
			fmask = new FeatureMask(128);
			Assert.AreEqual(2, ReflectionHelper.GetField(fmask, "m_maskCount"));

			// Make the mask 129 bits long.
			fmask = new FeatureMask(129);
			Assert.AreEqual(3, ReflectionHelper.GetField(fmask, "m_maskCount"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Clone method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Clone()
		{
			var fmask1 = new FeatureMask(64);
			fmask1[10] = true;
			fmask1[20] = true;
			fmask1[30] = true;
			fmask1[40] = true;

			var fmask2 = fmask1.Clone();
			
			Assert.AreEqual(64, fmask2.Size);
			Assert.AreEqual(1, (int)ReflectionHelper.GetField(fmask2, "m_maskCount"));
			Assert.IsTrue(fmask2[10]);
			Assert.IsTrue(fmask2[20]);
			Assert.IsTrue(fmask2[30]);
			Assert.IsTrue(fmask2[40]);

			UInt64 mask = 1;
			mask <<= 10;
			mask |= 1;
			mask <<= 10;
			mask |= 1;
			mask <<= 10;
			mask |= 1;
			mask <<= 10;
			var masks = (UInt64[])ReflectionHelper.GetField(fmask2, "m_masks");
			Assert.AreEqual(mask, masks[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that a negative mask size is invalid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(ArgumentException), "Mask size may not be negative.")]
		public void ConstructWithNegativeBitSize()
		{
			new FeatureMask(-1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies setting and getting bit values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BitGetting()
		{
			var fmask = new FeatureMask(100);

			fmask[45] = true;
			Assert.IsTrue(fmask[45]);
			fmask[45] = false;
			Assert.IsFalse(fmask[45]);

			fmask[60] = true;
			Assert.IsTrue(fmask[60]);
			fmask[60] = false;
			Assert.IsFalse(fmask[60]);

			fmask[80] = true;
			Assert.IsTrue(fmask[80]);
			fmask[80] = false;
			Assert.IsFalse(fmask[80]);

			fmask[75] = true;
			Assert.IsTrue(fmask[75]);
			fmask[75] = false;
			Assert.IsFalse(fmask[75]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the IsEmpty property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsEmtpy()
		{
			var fmask = new FeatureMask(100);

			fmask[45] = true;
			fmask[75] = true;
			Assert.IsTrue(fmask[45]);
			Assert.IsTrue(fmask[75]);

			fmask[45] = false;
			fmask[75] = false;
			Assert.IsTrue(fmask.IsEmpty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the Clear method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Clear()
		{
			var fmask = new FeatureMask(256);

			Assert.IsTrue(fmask.IsEmpty);

			fmask[60] = true;
			fmask[120] = true;
			fmask[200] = true;
			fmask[250] = true;
			Assert.IsFalse(fmask.IsEmpty);

			fmask.Clear();
			Assert.IsTrue(fmask.IsEmpty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that calling the OR method throws an exception when the number
		/// of bits in the masks are different.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(ArgumentException),
			"Bit count mismatch: both masks must contain same number of bits.")]
		public void OR_WithDiffSizeBits()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(200);
			Assert.IsNotNull(fmask1 | fmask2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that calling the OR method throws an exception when the number
		/// of bits in the masks are different.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(ArgumentException),
			"Bit count mismatch: both masks must contain same number of bits.")]
		public void ContainsAll_WithDiffSizeBits()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(200);
			Assert.IsNotNull(fmask1.ContainsAll(fmask2));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that calling the OR method throws an exception when the number
		/// of bits in the masks are different.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(ArgumentException),
			"Bit count mismatch: both masks must contain same number of bits.")]
		public void ContainsOneOrMore_WithDiffSizeBits()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(200);
			Assert.IsNotNull(fmask1.ContainsOneOrMore(fmask2));
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the ContainsAll method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ContainsAll()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(100);

			fmask1[45] = true;
			fmask1[50] = true;
			fmask1[75] = true;
			Assert.IsFalse(fmask1.ContainsAll(fmask2));

			fmask2[45] = true;
			Assert.IsTrue(fmask1.ContainsAll(fmask2));

			fmask2[50] = true;
			Assert.IsTrue(fmask1.ContainsAll(fmask2));

			fmask2[75] = true;
			Assert.IsTrue(fmask1.ContainsAll(fmask2));

			fmask2[90] = true;
			Assert.IsFalse(fmask1.ContainsAll(fmask2));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the ContainsOneOrMore method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ContainsOneOrMore()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(100);

			Assert.IsFalse(fmask1.ContainsOneOrMore(fmask2));

			fmask1[45] = true;
			fmask2[75] = true;
			Assert.IsFalse(fmask1.ContainsOneOrMore(fmask2));

			fmask1[75] = true;
			fmask2[45] = true;
			Assert.IsTrue(fmask1.ContainsOneOrMore(fmask2));

			fmask1[45] = false;
			Assert.IsTrue(fmask1.ContainsOneOrMore(fmask2));

			fmask1.Clear();
			fmask2.Clear();
			Assert.IsFalse(fmask1.ContainsOneOrMore(fmask2));

			// Overlap by one bit.
			for (int i = 60; i <= 64; i++)
				fmask1[i] = true;

			for (int i = 64; i < 70; i++)
				fmask2[i] = true;

			Assert.IsTrue(fmask1.ContainsOneOrMore(fmask2));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that getting a negative bit throws an exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException),
			"bit -2 is out of range. Bit must be >= 0 and < 128.")]
		public void BitGetting_BitIsNegative()
		{
			var fmask = new FeatureMask(128);
			Assert.IsFalse(fmask[-2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that getting a bit that too large throws an exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException),
			"bit 130 is out of range. Bit must be >= 0 and < 128.")]
		public void BitGetting_BitIsTooBig()
		{
			var fmask = new FeatureMask(128);
			Assert.IsFalse(fmask[130]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that setting a negative bit throws an exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException),
			"bit -2 is out of range. Bit must be >= 0 and < 128.")]
		public void BitSetting_BitIsNegative()
		{
			var fmask = new FeatureMask(128);
			fmask[-2] = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that setting a bit that's too large throws an exception.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException),
			"bit 130 is out of range. Bit must be >= 0 and < 128.")]
		public void BitSetting_BitIsTooBig()
		{
			var fmask = new FeatureMask(128);
			fmask[130] = true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that setting a bit works when there is only one bit set in the mask.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BitSetting_SingleBit()
		{
			var fmask = new FeatureMask(100);
			var masks = (UInt64[])ReflectionHelper.GetField(fmask, "m_masks");

			Assert.AreEqual(0, masks[0]);
			Assert.AreEqual(0, masks[1]);

			fmask[0] = true;
			Assert.AreEqual(1, masks[0]);

			fmask[0] = false;
			Assert.AreEqual(0, masks[0]);

			Int64 expected = 1;
			expected <<= 9;
			fmask[9] = true;
			Assert.AreEqual(expected, masks[0]);

			expected = 1;
			expected <<= (90 - 64);
			fmask[90] = true;
			Assert.AreEqual(expected, masks[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that setting multiple bits sets the correct bits.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void BitSetting_MultipleBits()
		{
			var fmask = new FeatureMask(128);
			var masks = (UInt64[])ReflectionHelper.GetField(fmask, "m_masks");

			fmask[15] = true;
			fmask[31] = true;
			fmask[47] = true;
			fmask[63] = true;
			Assert.AreEqual(0x8000800080008000, masks[0]);

			fmask[69] = true;
			fmask[85] = true;
			fmask[101] = true;
			fmask[117] = true;
			Assert.AreEqual(0x0020002000200020, masks[1]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that setting multiple bits sets the correct bits.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyToString()
		{
			var fmask = new FeatureMask(128);

			fmask[15] = true;
			fmask[31] = true;
			fmask[47] = true;
			fmask[63] = true;
			Assert.AreEqual("8000800080008000", fmask.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the OR operator.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void OR()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(100);

			fmask1[10] = true;
			fmask1[20] = true;
			fmask1[30] = true;
			var masks = (UInt64[])ReflectionHelper.GetField(fmask1, "m_masks");
			Assert.IsTrue(masks[0] > 0);
			Assert.IsTrue(masks[1] == 0);			
			
			fmask2[70] = true;
			fmask2[80] = true;
			fmask2[90] = true;
			masks = (UInt64[])ReflectionHelper.GetField(fmask2, "m_masks");
			Assert.IsTrue(masks[0] == 0);
			Assert.IsTrue(masks[1] > 0);
			
			fmask1 |= fmask2;
			Assert.IsTrue(fmask1[10]);
			Assert.IsTrue(fmask1[20]);
			Assert.IsTrue(fmask1[30]);
			Assert.IsTrue(fmask1[70]);
			Assert.IsTrue(fmask1[80]);
			Assert.IsTrue(fmask1[90]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the == operator.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Equal()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(100);

			fmask1[10] = true;
			fmask1[20] = true;
			fmask1[70] = true;
			fmask1[90] = true;
			var masks = (UInt64[])ReflectionHelper.GetField(fmask1, "m_masks");
			Assert.IsTrue(masks[0] > 0);
			Assert.IsTrue(masks[1] > 0);

			fmask2[10] = true;
			fmask2[20] = true;
			fmask2[70] = true;
			fmask2[90] = true;
			masks = (UInt64[])ReflectionHelper.GetField(fmask2, "m_masks");
			Assert.IsTrue(masks[0] > 0);
			Assert.IsTrue(masks[1] > 0);

			Assert.IsTrue(fmask1 == fmask2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the == operator when masks have different number of bits in them.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Equal_WithDiffSizes()
		{
			var fmask1 = new FeatureMask(10);
			var fmask2 = new FeatureMask(20);
			fmask1[5] = true;
			fmask2[5] = true;
			Assert.IsFalse(fmask1 == fmask2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the == operator when one or both masks are null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Equal_WithNull()
		{
			FeatureMask fmask1 = null;
			FeatureMask fmask2 = null;
			Assert.IsTrue(fmask1 == fmask2);

			fmask1 = new FeatureMask(1);
			Assert.IsFalse(fmask1 == fmask2);

			fmask1 = null;
			fmask2 = new FeatureMask(1);
			Assert.IsFalse(fmask1 == fmask2);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the != operator.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotEqual()
		{
			var fmask1 = new FeatureMask(100);
			var fmask2 = new FeatureMask(100);

			fmask1[10] = true;
			fmask1[20] = true;
			fmask1[70] = true;
			fmask1[90] = true;
			var masks = (UInt64[])ReflectionHelper.GetField(fmask1, "m_masks");
			Assert.IsTrue(masks[0] > 0);
			Assert.IsTrue(masks[1] > 0);

			fmask2[11] = true;
			fmask2[21] = true;
			fmask2[70] = true;
			fmask2[90] = true;
			masks = (UInt64[])ReflectionHelper.GetField(fmask2, "m_masks");
			Assert.IsTrue(masks[0] > 0);
			Assert.IsTrue(masks[1] > 0);

			Assert.IsTrue(fmask1 != fmask2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the != operator when masks have different number of bits in them.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotEqual_WithDiffSizes()
		{
			var fmask1 = new FeatureMask(10);
			var fmask2 = new FeatureMask(20);
			fmask1[5] = true;
			fmask2[5] = true;
			Assert.IsTrue(fmask1 != fmask2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the != operator when one or both masks are null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotEqual_WithNull()
		{
			FeatureMask fmask1 = null;
			FeatureMask fmask2 = null;
			Assert.IsFalse(fmask1 != fmask2);

			fmask1 = new FeatureMask(1);
			Assert.IsTrue(fmask1 != fmask2);

			fmask1 = null;
			fmask2 = new FeatureMask(1);
			Assert.IsTrue(fmask1 != fmask2);
		}
	}
}
