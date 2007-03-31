using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a button that acts like a link label. The problem with using a link label
	/// is that when a link label is give a mnemonic, it act like a label and passes focus on
	/// to the next control in the tab order. I wanted it to treat that like clicking on the
	/// link label. Therefore, I have created a button that acts like a link label except that
	/// using the mnemonic on the link button is like clicking on the link.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class LinkButton : Button
	{
		private Font m_fntHot;

		public LinkButton() : base()
		{
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			AutoSize = true;
			FlatStyle = FlatStyle.Flat;
			FlatAppearance.BorderSize = 0;
			FlatAppearance.MouseOverBackColor = BackColor;
			FlatAppearance.MouseDownBackColor = BackColor;
			Cursor = Cursors.Hand;
			ForeColor = Color.FromArgb(0, 0, 255);
			m_fntHot = new Font(FontHelper.UIFont, FontStyle.Underline);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Do some things that should be done in the constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Normally, I would set the font in the constructor. But I found that when I did,
			// the mnemonic underline gets drawn too close to the text. I don't know why.
			// Grrr! Setting the font after the handle is created seems to solve the problem.
			Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show a underline when the mouse is over the button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			Font = m_fntHot;
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turn off the underline when the mouse moves off the button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			Font = FontHelper.UIFont;
			base.OnMouseLeave(e);
		}
	}
}
