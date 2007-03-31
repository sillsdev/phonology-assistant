using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single item within a SimpleExplorerBar control.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ExplorerBarItem : Panel
	{
		public event EventHandler Collapsed;
		public event EventHandler Expanded;

		private bool m_drawHot = false;
		private Button m_button;
		private Control m_control;
		private int m_controlsExpandedHeight;
		private bool m_expanded = true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Construct an SimpleExplorerBar  
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExplorerBarItem(string text, Control hostedControl)
		{
			m_button = new Button();
			m_button.Text = text;
			m_button.Dock = DockStyle.Top;
			m_button.Font = new Font(FontHelper.UIFont, FontStyle.Bold);
			m_button.Height = (int)(1.7 * (double)m_button.Font.Height);
			m_button.Cursor = Cursors.Hand;
			m_button.Click += new EventHandler(m_button_Click);
			m_button.Paint += new PaintEventHandler(m_button_Paint);
			m_button.MouseEnter += new EventHandler(m_button_MouseEnter);
			m_button.MouseLeave += new EventHandler(m_button_MouseLeave);
			Controls.Add(m_button);

			m_control = hostedControl;
			SetHostedControlHeight(m_control.Height);
			m_control.Dock = DockStyle.Fill;
			Controls.Add(m_control);
			m_control.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the item's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return m_button.Text; }
			set
			{
				m_button.Text = value;
				m_button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font used to display the item's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return m_button.Font; }
			set
			{
				m_button.Font = value;
				m_button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the item is expanded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsExpanded
		{
			get {return m_expanded;}
			set
			{
				if (m_expanded != value)
					m_button_Click(null, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the control portion of the ExplorerBarItem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control Control
		{
			get {return m_control;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the button portion of the ExplorerBarItem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Button Button
		{
			get {return m_button;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Redraw button in normal state when mouse moves leaves it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_button_MouseLeave(object sender, EventArgs e)
		{
			m_drawHot = false;
			m_button.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Redraw button in hot state when mouse moves over it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_button_MouseEnter(object sender, EventArgs e)
		{
			m_drawHot = true;
			m_button.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We don't want a typical looking button. Therefore, draw it ourselves.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_button_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = m_button.ClientRectangle;

			using (SolidBrush br = new SolidBrush(BackColor))
				e.Graphics.FillRectangle(br, rc);

			// Draw the item's glyph.
			DrawExpandCollapseGlyph(e.Graphics);

			// Draw the item's text.
			TextFormatFlags flags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;

			rc.Inflate(-2, 0);
			TextRenderer.DrawText(e.Graphics, m_button.Text, m_button.Font,
				rc, SystemColors.WindowText, flags);
			rc.Inflate(2, 0);

			// Draw a line separating the button area from what collapses and expands below it.
			Color clr1 = ColorHelper.CalculateColor(Color.White, SystemColors.MenuHighlight, 90);
			Point pt1 = new Point(rc.X + 1, rc.Bottom - 3);
			Point pt2 = new Point(rc.Right, rc.Bottom - 3);
			using (LinearGradientBrush br = new LinearGradientBrush(pt1, pt2,
				clr1, SystemColors.Window))
			{
				e.Graphics.DrawLine(new Pen(br, 1), pt1, pt2);
			}

			rc.Inflate(-1, -1);
			if (m_button.Focused)
				ControlPaint.DrawFocusRectangle(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the expand or collapse glyph. The glyph drawn depends on the visible state
		/// of the hosted control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawExpandCollapseGlyph(Graphics graphics)
		{
			// Determine the rectangle in which the expanding/collapsing button will be drawn.
			Rectangle rc = new Rectangle(0, 0, m_button.Height, m_button.Height);
			if (RightToLeft == RightToLeft.No)
				rc.X = (m_button.ClientRectangle.Right - rc.Width);

			VisualStyleElement element;

			if (m_drawHot)
			{
				element = (m_control.Visible ?
					VisualStyleElement.ExplorerBar.NormalGroupCollapse.Hot :
					VisualStyleElement.ExplorerBar.NormalGroupExpand.Hot);
			}
			else
			{
				element = (m_control.Visible ?
					VisualStyleElement.ExplorerBar.NormalGroupCollapse.Normal :
					VisualStyleElement.ExplorerBar.NormalGroupExpand.Normal);
			}

			if (PaintingHelper.CanPaintVisualStyle(element))
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(graphics, rc);
			}
			else
			{
				Image glyph = (m_expanded ? Properties.Resources.kimidCollapseGlyph :
					Properties.Resources.kimidExpandGlyph);

				if (RightToLeft == RightToLeft.No)
					rc.X = rc.Right - (glyph.Width + 1);

				rc.Y += (rc.Height - glyph.Height) / 2;
				rc.Width = glyph.Width;
				rc.Height = glyph.Height;
				graphics.DrawImage(glyph, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggle item's expanded state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_button_Click(object sender, EventArgs e)
		{
			m_expanded = !m_expanded;
			m_control.Visible = m_expanded;
			Height = m_button.Height + (m_control.Visible ? m_controlsExpandedHeight : 0);

			if (m_control.Visible && Expanded != null)
				Expanded(this, EventArgs.Empty);
			else if (!m_control.Visible && Collapsed != null)
				Collapsed(this, EventArgs.Empty);

			// Force the expand/collase glyph to be repainted.
			Rectangle rc = m_button.ClientRectangle;
			rc.X = rc.Right - rc.Height + 2;
			rc.Width = rc.Height + 2;
			m_button.Invalidate(rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetHostedControlHeight(int height)
		{
			m_controlsExpandedHeight = height;

			if (IsExpanded)
				Height = m_button.Height + height +	m_control.Margin.Top + m_control.Margin.Bottom;
		}
	}
}
