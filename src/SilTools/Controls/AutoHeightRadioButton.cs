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
// File: AutoHeightRadioButton.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SilTools.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class extends the RadioButton control to automatically adjust the height to
	/// accomodate all the text in the control. Then it can be added to a stacked group of
	/// controls in a flow layout panel and the controls below will automatically get pushed
	/// down as the RadioButton grows.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AutoHeighRadioButton : RadioButton
	{
		/// ------------------------------------------------------------------------------------
		public AutoHeighRadioButton()
		{
			AutoSize = false;
			AutoEllipsis = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (!AutoSize && !string.IsNullOrEmpty(Text))
			{
				var constraints = new Size(width, 0);

				using (var g = CreateGraphics())
				{
					height = Math.Max(TextRenderer.MeasureText(g, Text, Font, constraints,
						TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter).Height, height);
				}
			}

			base.SetBoundsCore(x, y, width, height, specified);
		}
	}
}
