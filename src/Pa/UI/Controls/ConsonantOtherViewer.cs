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

namespace SIL.Pa
{
	public class ConsonantOtherViewer : IPACharViewerBase
	{
		public ConsonantOtherViewer() : base()
		{
			//new int[] { 0x0298, 0x007C, 0x01C3, 0x01C2, 0x2016, 0x0253, 0x0257, 0x0284, 0x0260, 0x029B, 0x028D,
			//            0x0077, 0x0265, 0x029C, 0x02A2, 0x02A1, 0x0255, 0x0291, 0x027A, 0x0267, 0x0000, 0x0000},
			m_codes = new int[][]
			{
				new int[] {0x0298, 0x007C, 0x01C3},
				new int[] {0x01C2, 0x2016, 0x0253},
				new int[] {0x0257, 0x0284, 0x0260},
				new int[] {0x029B, 0x028D,	0x0077},
				new int[] {0x0265, 0x029C, 0x02A2},
				new int[] {0x02A1, 0x0255, 0x0291},
				new int[] {0x027A, 0x0267, 0x0000}
			};
		}
	}
}