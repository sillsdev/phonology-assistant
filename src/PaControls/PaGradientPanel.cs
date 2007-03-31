using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a panel control whose background is a gradient fill like that of window
	/// caption bars (except, unlike a caption's gradient transition direction of left to
	/// right, the panel's transition direction is from bottom to top).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaGradientPanel : PaPanel
	{
		private bool m_dark = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaGradientPanel()
		{
			OnSystemColorsChanged(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the gradient background should
		/// be dark.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MakeDark
		{
			get { return m_dark; }
			set
			{
				m_dark = value;
				OnSystemColorsChanged(null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			//if (m_dark)
			//{
			//    m_clrTop = ColorHelper.CalculateColor(Color.White,
			//        SystemColors.ActiveCaption, 70);

			//    m_clrBottom = ColorHelper.CalculateColor(SystemColors.ActiveCaption,
			//        SystemColors.ActiveCaption, 0);
			//}
			//else
			//{
			//    m_clrTop = ColorHelper.CalculateColor(Color.White,
			//        SystemColors.GradientActiveCaption, 190);

			//    m_clrBottom = ColorHelper.CalculateColor(SystemColors.ActiveCaption,
			//        SystemColors.GradientActiveCaption, 50);
			//}

			base.OnSystemColorsChanged(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Force a repaint when the size changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			PaintingHelper.DrawGradientBackground(e.Graphics, ClientRectangle, m_dark);
		}
	}
}
