using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SilTools.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CustomDropDownComboBox : UserControl
	{
		private bool _mouseDown;
		private bool _buttonHot;
		private SilPopup _popupCtrl;

		public EventHandler PopupClosed;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomDropDownComboBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CustomDropDownComboBox()
		{
			InitializeComponent();

			AlignDropToLeft = true;

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

			_textBox.BackColor = SystemColors.Window;

			Padding = new Padding(Application.RenderWithVisualStyles ?
				SystemInformation.BorderSize.Width : SystemInformation.Border3DSize.Width);

			_button.Width = SystemInformation.VerticalScrollBarWidth;
			_textBox.Left = Padding.Left + 2;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return _textBox.Text; }
			set { _textBox.Text = value; }
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets or sets the font of the text displayed by the control.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public override Font Font
		//{
		//    get { return m_txtBox.Font; }
		//    set { m_txtBox.Font = value; }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				_textBox.BackColor = value;
				base.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public TextBox TextBox
		{
			get { return _textBox; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the left edge of the drop-down is aligned
		/// with the left edge of the combo control. To align the left edges, set this value
		/// to true. To align the right edge of the drop-down, set this value to false.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AlignDropToLeft { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the popup Control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilPopup PopupCtrl
		{
			get { return _popupCtrl; }
			set
			{
				if (_popupCtrl != null)
					_popupCtrl.PopupClosed -= OnPopupClosed;

				_popupCtrl = value;

				if (_popupCtrl != null)
					_popupCtrl.PopupClosed += OnPopupClosed;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the VisibleChanged event of the m_popupCtrl control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnPopupClosed(object sender, EventArgs e)
		{
			if (PopupClosed != null)
				PopupClosed(this, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will center the text box vertically within the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			int newTop = (Height - _textBox.Height) / 2;
			_textBox.Top = (newTop < 0 ? 0 : newTop);
			_textBox.Width = (ClientSize.Width - Padding.Left - Padding.Right - _button.Width - 2);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!Application.RenderWithVisualStyles)
				ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
			else
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(Enabled ?
					VisualStyleElement.TextBox.TextEdit.Normal :
					VisualStyleElement.TextBox.TextEdit.Disabled);

				renderer.DrawBackground(e.Graphics, ClientRectangle, e.ClipRectangle);

				// When the textbox background is drawn in normal mode (at least when the
				// theme is one of the standard XP themes), it's drawn with a white background
				// and not the System Window background color. Therefore, we need to create
				// a rectangle that doesn't include the border. Then fill it with the text
				// box's background color.
				Rectangle rc = renderer.GetBackgroundExtent(e.Graphics, ClientRectangle);
				int dx = (rc.Width - ClientRectangle.Width) / 2;
				int dy = (rc.Height - ClientRectangle.Height) / 2;
				rc = ClientRectangle;
				rc.Inflate(-dx, -dy);

				using (SolidBrush br = new SolidBrush(_textBox.BackColor))
					e.Graphics.FillRectangle(br, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the painting the button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_button_Paint(object sender, PaintEventArgs e)
		{
			ButtonState state = ButtonState.Normal;

			VisualStyleElement element = VisualStyleElement.ComboBox.DropDownButton.Normal;
			if (!Enabled)
			{
				state = ButtonState.Inactive;
				element = VisualStyleElement.ComboBox.DropDownButton.Disabled;
			}
			else if (_mouseDown)
			{
				state = ButtonState.Pushed;
				element = VisualStyleElement.ComboBox.DropDownButton.Pressed;
			}
			else if (_buttonHot)
				element = VisualStyleElement.ComboBox.DropDownButton.Hot;

			if (!Application.RenderWithVisualStyles)
				PaintNonThemeButton(e.Graphics, state);
			else
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(e.Graphics, _button.ClientRectangle);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void PaintNonThemeButton(Graphics g, ButtonState state)
		{
			ControlPaint.DrawButton(g, _button.ClientRectangle, state);

			using (Font fnt = new Font("Marlett", 10))
			{
				TextRenderer.DrawText(g, "6", fnt, _button.ClientRectangle, SystemColors.ControlDarkDark,
					TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonMouseEnter(object sender, EventArgs e)
		{
			_buttonHot = true;
			_button.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonMouseLeave(object sender, EventArgs e)
		{
			_buttonHot = false;
			_button.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonMouseUp(object sender, MouseEventArgs e)
		{
			// Repaint the drop down button so that it displays normal instead of pressed
			if (_mouseDown)
			{
				_mouseDown = false;
				_button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleButtonMouseDown(object sender, MouseEventArgs e)
		{
			// Repaint the drop down button so that it displays pressed
			if (e.Button == MouseButtons.Left)
			{
				_mouseDown = true;
				_button.Invalidate();
			}

			if (_popupCtrl != null)
			{
				var pt = new Point(0, Height);
				if (!AlignDropToLeft)
					pt.X -= (_popupCtrl.Width - Width);

				_popupCtrl.Show(this, pt);
			}
		}
	}
}
