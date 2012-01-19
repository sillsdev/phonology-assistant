using System;
using System.Drawing;
using System.Windows.Forms;

namespace SilTools.Controls
{
	public class NicerButton : Button
	{
		private bool _showFocusRectangle = true;
		private Color _focusBackColor = Color.Empty;

		/// ------------------------------------------------------------------------------------
		public NicerButton()
		{
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			FlatStyle = FlatStyle.Flat;
			FlatAppearance.BorderSize = 0;
			BackColor = Color.Transparent;
			FlatAppearance.MouseOverBackColor = Color.FromArgb(50, Color.Black);
			FlatAppearance.MouseDownBackColor = Color.FromArgb(75, Color.Black);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShowFocusCues
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the background color of the button when it has focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color FocusBackColor
		{
			get { return _focusBackColor; }
			set
			{
				_focusBackColor = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool ShowFocusRectangle
		{
			get { return _showFocusRectangle; }
			set
			{
				_showFocusRectangle = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			if (_focusBackColor != Color.Empty)
				BackColor = _focusBackColor;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			base.OnEnter(e);
			
			if (_focusBackColor != Color.Empty)
				BackColor = Color.Transparent;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DrawFocusRectangle(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		protected void DrawFocusRectangle(Graphics g)
		{
			if (Focused && ShowFocusRectangle)
				ControlPaint.DrawFocusRectangle(g, ClientRectangle);
		}
	}
}
