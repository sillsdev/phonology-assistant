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
		private ToolTip m_tooltip;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DockButton()
		{
			base.AutoSize = false;
			SetToolTips();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetToolTips()
		{
			m_tooltip = new ToolTip();
			m_tooltip.SetToolTip(this, Properties.Resources.kstidDockToolTip);
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Button for allowing a sliding panel to be automatically hidden.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AutoHideButton : XButton
	{
		private ToolTip m_tooltip;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public AutoHideButton()
		{
			base.AutoSize = false;
			SetToolTips();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetToolTips()
		{
			m_tooltip = new ToolTip();
			m_tooltip.SetToolTip(this, Properties.Resources.kstidAutoHideToolTip);
		}
	}
}