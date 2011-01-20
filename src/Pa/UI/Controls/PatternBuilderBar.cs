using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	public partial class PatternBuilderBar : UserControl
	{
		public PatternBuilderBar()
		{
			InitializeComponent();

			m_toolstrip.Renderer.RenderToolStripBorder += ((sender, e) =>
			{
				using (var br = new SolidBrush(e.ToolStrip.BackColor))
				{
					// Paint over bottom edge to get rid of border.
					var rc = e.ToolStrip.ClientRectangle;
					rc.Y = rc.Bottom - 2;
					rc.Height = 2;
					e.Graphics.FillRectangle(br, rc);

					// Paint over right edge to get rid of border.
					rc = e.ToolStrip.ClientRectangle;
					rc.X = rc.Right - 2;
					rc.Width = 2;
					e.Graphics.FillRectangle(br, rc);
				}
			});
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			m_toolstrip.Height = m_toolstrip.PreferredSize.Height;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Force the height of the control to be one pixel larger than the tool strip.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			height = m_toolstrip.PreferredSize.Height + 2;
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			var rc = ClientRectangle;
			rc.Width--;
			rc.Height--;
			e.Graphics.DrawRectangle(Pens.WhiteSmoke, rc);
		}
	}
}
