using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PhoneInfoPopupContent : UserControl
	{
		private int m_origWidth1;
		private int m_origWidth2;
		private int m_origWidth3;
		private int m_origLeft;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopupContent()
		{
			InitializeComponent();
			DoubleBuffered = true;

			m_origWidth1 = lblPhone.Width;
			m_origWidth2 = lblBlkWhtBorder.Width;
			m_origWidth3 = lblCountHeading.Width;
			m_origLeft = lblCountHeading.Left;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFonts()
		{
			// Reset the with of labels that make up the black box with a white border as
			// well as the heading to it's right. Also reset the left location of the
			// heading label.
			lblPhone.Width = m_origWidth1;
			lblBlkWhtBorder.Width = m_origWidth2; 
			lblCountHeading.Width = m_origWidth3; 
			lblCountHeading.Left = m_origLeft;
			
			// Make the phone monogram label 15 point, regardless of the user's setting.
			lblPhone.Font = FontHelper.MakeEticRegFontDerivative(15);

			// Limit the sibling uncertainty list's font size to 14 point.
			lblSiblingPhones.Font = (FontHelper.PhoneticFont.SizeInPoints > 14 ?
				FontHelper.MakeEticRegFontDerivative(14) : FontHelper.PhoneticFont);

			// Now make sure the phone fully fits into the black box. If it doesn't,
			// then make it wide enough to accomodate the phone. That also means the
			// heading label needs to be made narrower and moved to the right a bit.
			using (Graphics g = lblPhone.CreateGraphics())
			{
				TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
					TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
					TextFormatFlags.NoClipping;
				
				int phoneWidth = TextRenderer.MeasureText(g, lblPhone.Text,
					lblPhone.Font, Size.Empty, flags).Width;

				if (lblPhone.Width < phoneWidth)
				{
					int dx = phoneWidth - lblPhone.Width;
					lblPhone.Width = phoneWidth;
					lblBlkWhtBorder.Width += dx;
					lblCountHeading.Width -= dx;
					lblCountHeading.Left += dx;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SetSiblingUncertainties(List<string> siblingUncertainties)
		{
			lblSiblingPhones.Text = string.Empty;

			if (siblingUncertainties == null || siblingUncertainties.Count == 0)
			{
				// If there's no sibling uncertain phones, then shrink the popup to show
				// only the count values.
				Height = lblNonPrimary.Bottom + Padding.Bottom + 2;
				return false;
			}

			StringBuilder bldr = new StringBuilder();
			for (int i = 0; i < siblingUncertainties.Count; i++)
			{
				string comma = (i > 0 ? ", " : string.Empty);
				lblSiblingPhones.Text = bldr.ToString() + comma + siblingUncertainties[i];

				// Determine whether or not to insert a new line or if there's room to
				// continue adding phones on the current line.
				bldr.Append(
					lblSiblingPhones.PreferredWidth <= lblSiblingPhones.Width ?	comma : ",\n");

				bldr.Append(siblingUncertainties[i]);
			}

			// Set the desired height of the sibling phones list and then set the
			// height of this popup based on that.
			lblSiblingPhones.Height = lblSiblingPhones.PreferredHeight;

			Height = lblSiblingPhones.Bottom + Padding.Bottom + 2;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the Y coordinate where the line should be that divides the phone count
		/// information from the sibling uncertainty information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SeparatorLineY
		{
			get { return lblNonPrimary.Bottom + 
				(lblUncertaintyHeading.Top - lblNonPrimary.Bottom) / 2; }
		}
	}
}
