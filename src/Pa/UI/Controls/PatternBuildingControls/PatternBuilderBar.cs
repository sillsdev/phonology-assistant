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
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class PatternBuilderBar : UserControl
	{
		public Action<string> ItemSelectedHandler;

		/// ------------------------------------------------------------------------------------
		public PatternBuilderBar()
		{
			InitializeComponent();

			BackColor = Properties.Settings.Default.GradientPanelTopColor;

			m_menuStrip.BackColor = Properties.Settings.Default.GradientPanelTopColor;
			m_menuStrip.ForeColor = Properties.Settings.Default.GradientPanelTextColor;
			m_menuStrip.Renderer.RenderItemText += ((s, e) =>
			{
				if (e.Item.OwnerItem == null && e.Item is ToolStripMenuItem &&
					(e.Item.Selected || ((ToolStripMenuItem)e.Item).DropDown.Visible))
				{
					TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont,
						e.TextRectangle, Color.Black, e.TextFormat);
				}
			});
				
			((ToolStripDropDownMenu)m_mnuSpecial.DropDown).ShowImageMargin = false;
			((ToolStripDropDownMenu)m_mnuSpecial.DropDown).ShowCheckMargin = false;

			foreach (ToolStripItem mnu in m_mnuSpecial.DropDownItems)
				mnu.Font = SystemFonts.MenuFont;

			m_mnuPhones.DropDown = PatternBuilderPhoneDropDown.Create(() => Width, text =>
			{
				m_mnuPhones.DropDown.Close();
				ItemSelectedHandler(text);
			});
		}

		/// ------------------------------------------------------------------------------------
		static void HandleRenderMenuItemText(object sender, ToolStripItemTextRenderEventArgs e)
		{
			if (e.Item.OwnerItem == null && e.Item is ToolStripMenuItem &&
				(e.Item.Selected || ((ToolStripMenuItem)e.Item).DropDown.Visible))
			{
				TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont,
					e.TextRectangle, Color.Black, e.TextFormat);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Force the height of the control to be a bit larger than the tool strip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			height = m_menuStrip.Height + 2;
//			height = m_toolstrip.PreferredSize.Height + 2;
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			var rc = ClientRectangle;
			rc.Width--;
			rc.Height--;

			// Paint border around the bar.
			using (var pen = new Pen(Properties.Settings.Default.GradientPanelTextColor))
				e.Graphics.DrawRectangle(pen, rc);
		}
	}
}
