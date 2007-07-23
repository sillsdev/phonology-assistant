using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Extends the panel control to support text, including text containing mnemonic
	/// specifiers.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaTextPanel : Panel
	{
		public event EventHandler MnemonicInvoked;

		private TextFormatFlags m_txtFmtFlags = TextFormatFlags.VerticalCenter |
					TextFormatFlags.WordEllipsis | TextFormatFlags.SingleLine |
					TextFormatFlags.LeftAndRightPadding;

		private bool m_mnemonicGeneratesClick = false;
		private Control m_ctrlRcvingFocusOnMnemonic = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaTextPanel()
		{
			SetStyle(ControlStyles.UseTextForAccessibility, true);
			base.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the text in the header label acts like a normal label in that it
		/// responds to Alt+letter keys to send focus to the next control in the tab order.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessMnemonic(char charCode)
		{
			if (IsMnemonic(charCode, Text) && Parent != null)
			{
				if (m_mnemonicGeneratesClick)
				{
					InvokeOnClick(this, EventArgs.Empty);
					return true;
				}

				if (MnemonicInvoked != null)
				{
					MnemonicInvoked(this, EventArgs.Empty);
					return true;
				}

				if (m_ctrlRcvingFocusOnMnemonic != null)
				{
					m_ctrlRcvingFocusOnMnemonic.Focus();
					return true;
				}

				Control ctrl = this;
				
				do
				{
					ctrl = Parent.GetNextControl(ctrl, true);
				}
				while (ctrl != null && !ctrl.CanSelect);

				if (ctrl != null)
				{
					ctrl.Focus();
					return true;
				}
			}

			return base.ProcessMnemonic(charCode);
		}

		///  ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the header label's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		public override string Text
		{
			get	{return base.Text;}
			set
			{
				base.Text = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the control process the keyboard
		/// mnemonic as a click (like a button) or passes control on to the next control in
		/// the tab order (like a label).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		public bool MnemonicGeneratesClick
		{
			get { return m_mnemonicGeneratesClick; }
			set { m_mnemonicGeneratesClick = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the control that receives focus when the label's text is contains a
		/// mnumonic specifier. When this value is null, then focus is given to the next
		/// control in the tab order.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control ControlReceivingFocusOnMnemonic
		{
			get { return m_ctrlRcvingFocusOnMnemonic; }
			set { m_ctrlRcvingFocusOnMnemonic = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text format flags used to draw the header label's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextFormatFlags TextFormatFlags
		{
			get { return m_txtFmtFlags; }
			set { m_txtFmtFlags = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure to repaint when resizing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the text on the panel, if there is any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			if (!string.IsNullOrEmpty(Text))
			{
				// Draw the text.
				using (StringFormat sf = STUtils.GetStringFormat(false))
				{
					TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle,
						SystemColors.ControlText, m_txtFmtFlags);
				}
			}
		}
	}
}
