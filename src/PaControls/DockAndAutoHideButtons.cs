using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Button for allowing a sliding panel to be docked.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DockButton : XButton
	{
		private readonly ToolTip m_tooltip = new ToolTip();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DockButton()
		{
			base.AutoSize = false;
			m_tooltip.ShowAlways = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			Form frm = FindForm();
			Point pt = frm.PointToClient(MousePosition);
			pt.X += (SystemInformation.CursorSize.Width / 2);
			pt.Y += (int)(SystemInformation.CursorSize.Height * 1.5f);
			m_tooltip.Show(Properties.Resources.kstidDockToolTip, frm, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			m_tooltip.Hide(FindForm());
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Button for allowing a sliding panel to be automatically hidden.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AutoHideButton : XButton
	{
		private readonly ToolTip m_tooltip = new ToolTip();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AutoHideButton()
		{
			base.AutoSize = false;
			m_tooltip.ShowAlways = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			Form frm = FindForm();
			Point pt = frm.PointToClient(MousePosition);
			pt.X += (SystemInformation.CursorSize.Width / 2);
			pt.Y += (int)(SystemInformation.CursorSize.Height * 1.5f);
			m_tooltip.Show(Properties.Resources.kstidAutoHideToolTip, frm, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			m_tooltip.Hide(FindForm());
		}
	}
}