using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a label control that acts like a toolbar button but doesn't have to
	/// be placed on a tool strip control in order to be used.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ButtonLabel : Button
	{
		private ToolTip m_toolTip;
		private string m_toolTipText;
		private PaintState m_state = PaintState.Normal;
		private bool m_mouseDown = false;
		private bool m_mouseOver = false;
		private bool m_checked = false;
		private bool m_showCheckedState = false;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ButtonLabel()
		{
			AutoSize = false;
			Margin = new Padding(0);
			Size = new Size(23, 23);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ButtonLabel(string text, string tooltip) : this()
		{
			if (tooltip != null)
			{
				m_toolTipText = tooltip;
				m_toolTip = new ToolTip();
				m_toolTip.SetToolTip(this, m_toolTipText);
			}

			Text = text;
		}
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the button is checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool Checked
		{
			get {return m_checked;}
			set
			{
				m_checked = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to visual indicate the button's
		/// checked state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowCheckedState
		{
			get {return m_showCheckedState;}
			set
			{
				m_showCheckedState = value;
				Invalidate();
			}
		}

		#region Mouse handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the proper checked state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClick(EventArgs e)
		{
			m_checked = !m_checked;
			base.OnClick(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change appearance when mouse is pressed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				m_mouseDown = true;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change appearance when the mouse button is released.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (e.Button == MouseButtons.Left)
			{
				m_mouseDown = false;
				Invalidate();
			}

			// Reset tooltip because it seems never to show up after the label's been
			// clicked on. However, hide it right after because resetting it will force
			// it to be shown immediately after the mouse button is released, which is
			// sort of annoying.
			m_toolTip.SetToolTip(this, m_toolTipText);
			m_toolTip.Hide(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Track when the mouse leaves the control when a mouse button is pressed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			m_mouseOver = ClientRectangle.Contains(e.Location);

			PaintState newState = (m_mouseOver ? PaintState.Hot : PaintState.Normal);
			
			if (m_mouseOver && (m_mouseDown || (m_checked && m_showCheckedState)))
				newState = PaintState.HotDown;

			if (newState != m_state)
			{
				m_state = newState;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change appearance when mouse leaves control. This method only gets called when the
		/// mouse leaves the control when a mouse button isn't pressed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			m_mouseOver = false;
			Invalidate();
		}

		#endregion

		#region Focus handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change appearance when ButtonLabel gains focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			Invalidate();
			base.OnEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change appearance when ButtonLabel loses focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			Invalidate();
			base.OnLeave(e);
		}

		#endregion

		#region Paint methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the label based on it's state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			if (BackColor == Color.Transparent)
				GetParentsBackColor();

			m_state = PaintState.Normal;
			if (m_mouseOver)
			{
				m_state = ((m_checked && m_showCheckedState) || m_mouseDown ?
					PaintState.HotDown : PaintState.Hot);
			}
			else if (m_checked && m_showCheckedState)
				m_state = PaintState.Hot;

			Rectangle rc = ClientRectangle;

			using (SolidBrush br = new SolidBrush(BackColor))
				e.Graphics.FillRectangle(br, rc);

			if (m_state != PaintState.Normal)
				PaintingHelper.DrawHotBackground(e.Graphics, rc, m_state);

			TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
				TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
				TextFormatFlags.NoPrefix;

			// Draw text
			TextRenderer.DrawText(e.Graphics, Text, Font, rc, ForeColor, flags);

			if (Focused)
			{
				rc.Inflate(-2, -2);
				ControlPaint.DrawFocusRectangle(e.Graphics, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes up the button's parent chain until a parent with a non transparent background
		/// is found to use as the unselected background color for the button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetParentsBackColor()
		{
			Control parent = this;
			while (parent != null)
			{
				if (parent.BackColor != Color.Transparent)
				{
					BackColor = parent.BackColor;
					break;
				}
				parent = parent.Parent;
			}
		}

		#endregion
	}
}
