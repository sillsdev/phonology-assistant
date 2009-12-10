using System;
using System.Windows.Forms;

namespace SilUtils.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a panel control whose background is a gradient fill like that of window
	/// caption bars (except, unlike a caption's gradient transition direction of left to
	/// right, the panel's transition direction is from bottom to top).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SilGradientPanel : SilPanel
	{
		private bool m_dark;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilGradientPanel()
		{
			OnSystemColorsChanged(null);
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
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
			PaintingHelper.DrawGradientBackground(e.Graphics, ClientRectangle, m_dark);
		}
	}
}
