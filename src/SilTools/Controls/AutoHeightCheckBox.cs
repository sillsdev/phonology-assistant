// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SilUtils.Controls
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
		private int m_cachedWidthOfTextOnSingleLine;
		private bool m_autoSizingInProgress;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AutoHeightCheckBox()
		{
			AutoSize = false;
			AutoEllipsis = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return base.Text; }
			set
			{
				m_cachedWidthOfTextOnSingleLine = TextRenderer.MeasureText(value, Font).Width;
				base.Text = value;
				AdjustHeight();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				m_cachedWidthOfTextOnSingleLine = TextRenderer.MeasureText(Text, value).Width;
				base.Font = value;
				AdjustHeight();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (!m_autoSizingInProgress)
				AdjustHeight();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustHeight()
		{
			if (AutoSize || string.IsNullOrEmpty(Text))
				return;

			var prefSize = GetPreferredSize(Size.Empty);
			var checkBoxAndPaddingWidth = prefSize.Width - m_cachedWidthOfTextOnSingleLine;
			var constraints = new Size(prefSize.Width - checkBoxAndPaddingWidth, 0);
			using (var g = CreateGraphics())
			{
				var sz = TextRenderer.MeasureText(Text, Font, constraints,
					TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter);

				var newHeight = sz.Height;

				if (newHeight > prefSize.Height)
				{
					m_autoSizingInProgress = true;
					Height = newHeight;
					m_autoSizingInProgress = false;
				}
			}
		}
	}
}
