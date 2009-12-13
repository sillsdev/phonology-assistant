using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SilUtils.Controls
{
	public partial class CustomDropDownComboBox : UserControl
	{
		private bool m_mouseDown;
		private bool m_buttonHot;
		private SilPopup m_popupCtrl;

		public EventHandler PopupClosed;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomDropDownComboBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CustomDropDownComboBox()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

			m_txtBox.BackColor = SystemColors.Window;

			Padding = new Padding(Application.RenderWithVisualStyles ?
				SystemInformation.BorderSize.Width : SystemInformation.Border3DSize.Width);

			m_button.Width = SystemInformation.VerticalScrollBarWidth;
			m_txtBox.Left = Padding.Left + 2;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return m_txtBox.Text; }
			set { m_txtBox.Text = value; }
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
				m_txtBox.BackColor = value;
				base.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TextBox TextBox
		{
			get { return m_txtBox; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the popup Control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilPopup PopupCtrl
		{
			get { return m_popupCtrl; }
			set
			{
				if (m_popupCtrl != null)
					m_popupCtrl.PopupClosed -= m_popupCtrl_PopupClosed;

				m_popupCtrl = value;

				if (m_popupCtrl != null)
					m_popupCtrl.PopupClosed += m_popupCtrl_PopupClosed;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the VisibleChanged event of the m_popupCtrl control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_popupCtrl_PopupClosed(object sender, EventArgs e)
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
			int newTop = (Height - m_txtBox.Height) / 2;
			m_txtBox.Top = (newTop < 0 ? 0 : newTop);
			m_txtBox.Width = (ClientSize.Width - Padding.Left - Padding.Right - m_button.Width - 2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

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

				using (SolidBrush br = new SolidBrush(m_txtBox.BackColor))
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
			else if (m_mouseDown)
			{
				state = ButtonState.Pushed;
				element = VisualStyleElement.ComboBox.DropDownButton.Pressed;
			}
			else if (m_buttonHot)
				element = VisualStyleElement.ComboBox.DropDownButton.Hot;

			if (!Application.RenderWithVisualStyles)
				PaintNonThemeButton(e.Graphics, state);
			else
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(e.Graphics, m_button.ClientRectangle);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PaintNonThemeButton(Graphics g, ButtonState state)
		{
			ControlPaint.DrawButton(g, m_button.ClientRectangle, state);

			using (Font fnt = new Font("Marlett", 10))
			{
				TextRenderer.DrawText(g, "6", fnt, m_button.ClientRectangle, SystemColors.ControlDarkDark,
					TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_button_MouseEnter(object sender, System.EventArgs e)
		{
			m_buttonHot = true;
			m_button.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_button_MouseLeave(object sender, System.EventArgs e)
		{
			m_buttonHot = false;
			m_button.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_button_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Repaint the drop down button so that it displays normal instead of pressed
			if (m_mouseDown)
			{
				m_mouseDown = false;
				m_button.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected void m_button_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			// Repaint the drop down button so that it displays pressed
			if (e.Button == MouseButtons.Left)
			{
				m_mouseDown = true;
				m_button.Invalidate();
			}

			if (m_popupCtrl != null)
				m_popupCtrl.Show(this, new Point(0, Height));
		}
	}
}
