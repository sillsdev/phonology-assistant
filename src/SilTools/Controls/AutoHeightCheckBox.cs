// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2011, SIL International. All Rights Reserved.
// <copyright from='2010' to='2011' company='SIL International'>
//		Copyright (c) 2011, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: AutoHeightCheckBox.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Drawing;
using System.Windows.Forms;

namespace SilTools.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class extends the checkbox control to automatically adjust the height to
	/// accomodate all the text in the control. Then it can be added to a stacked group of
	/// controls in a flow layout panel and the controls below will automatically get pushed
	/// down as the checkbox grows.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AutoHeightCheckBox : CheckBox
	{
		/// ------------------------------------------------------------------------------------
		public AutoHeightCheckBox()
		{
			AutoSize = false;
			AutoEllipsis = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, PreferredSize.Height, specified);
		}

		/// ------------------------------------------------------------------------------------
		public override Size GetPreferredSize(Size proposedSize)
		{
			if (!string.IsNullOrEmpty(Text))
			{
				var constraints = new Size(Width, 0);

				using (var g = CreateGraphics())
				{
					proposedSize.Height = TextRenderer.MeasureText(g, Text, Font, constraints,
						TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter).Height + 4;
				}
			}

			return proposedSize;
		}
	}
}
