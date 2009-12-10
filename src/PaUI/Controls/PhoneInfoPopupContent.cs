using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Data;
using SilUtils;
using SilUtils.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PhoneInfoPopupContent : UserControl
	{
		private readonly int m_extraMonogramHeight;
		private readonly int m_extraUncertainListHeight;
		private readonly int m_origWidth1;
		private readonly int m_origWidth2;
		private readonly int m_origWidth3;
		private readonly int m_origLeft;
		private readonly int m_origUncertaintyHeadingHeight;
		private readonly int m_countPanelOrigHeight;
		private readonly PhoneInfoPopup m_hostingPopup;
		private bool m_siblingUncertaintiesExist = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopupContent()
		{
			InitializeComponent();
			base.DoubleBuffered = true;

			m_origWidth1 = lblMonogram.Width;
			m_origWidth2 = pnlMonogram.Width;
			m_origWidth3 = lblCountHeading.Width;
			m_origLeft = lblCountHeading.Left;
			m_origUncertaintyHeadingHeight = lblUncertaintyHeading.Height;
			m_countPanelOrigHeight = pnlCounts.Height;

			m_extraMonogramHeight = PaApp.SettingsHandler.GetIntSettingsValue(
				"cvcharts", "extrapopupmonogramheight", 7);

			m_extraUncertainListHeight = PaApp.SettingsHandler.GetIntSettingsValue(
				"cvcharts", "extrapopupuncertainlistheight", 5);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopupContent(PhoneInfoPopup hostingPopup) : this()
		{
			m_hostingPopup = hostingPopup;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Initialize(CharGridCell cgc)
		{
			lblMonogram.Text = cgc.Phone;
			lblNormallyCount.Text = cgc.TotalCount.ToString();
			lblPrimaryCount.Text = cgc.CountAsPrimaryUncertainty.ToString();
			lblNonPrimaryCount.Text = cgc.CountAsNonPrimaryUncertainty.ToString();
			RefreshFonts();
			m_siblingUncertaintiesExist = SetSiblingUncertainties(cgc.SiblingUncertainties);
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
			//lblMonogram.Width = m_origWidth1;
			//pnlMonogram.Width = m_origWidth2; 
			//lblCountHeading.Width = m_origWidth3; 
			//lblCountHeading.Left = m_origLeft;
			
			// Make the phone monogram label 15 point, regardless of the user's setting.
			lblMonogram.Font = FontHelper.MakeEticRegFontDerivative(15);

			// Limit the sibling uncertainty list's font size to 14 point.
			lblSiblingPhones.Font = (FontHelper.PhoneticFont.SizeInPoints > 14 ?
				FontHelper.MakeEticRegFontDerivative(14) : FontHelper.PhoneticFont);

			// Now make sure the phone fully fits into the black box (i.e. the monogram
			// label. If it doesn't, then make it wide enough to accomodate the phone.
			// That also means the heading label needs to be made narrower and moved to
			// the right a bit.
			using (Graphics g = lblMonogram.CreateGraphics())
			{
				TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
					TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
					TextFormatFlags.NoClipping;

				Size sz = TextRenderer.MeasureText(g, lblMonogram.Text,
					lblMonogram.Font, new Size(int.MaxValue, int.MaxValue) , flags);

				// Set the height of the monogram label by setting the height of the
				// heading panel (the monogram label will adjust accordingly because
				// its container, pnlMonogram, is docked left in the heading panel).
				int height = sz.Height + m_extraMonogramHeight;
				int dy = pnlHeading.MinimumSize.Height - lblMonogram.MinimumSize.Height;
				pnlHeading.Height = height + dy;

				// Set the width of the monogram label. It's docked in its container
				// so set its width by setting the width of its container.
				int dx = pnlMonogram.Width - lblMonogram.Width;
				pnlMonogram.Width = (lblMonogram.MinimumSize.Width < sz.Width ?
					sz.Width : lblMonogram.MinimumSize.Width) + dx;

				// Now adjust width of the label containing the heading text,
				// which is to the right of the monogram label.
				lblCountHeading.Width = pnlHeading.Width - (pnlHeading.Padding.Left +
					pnlHeading.Padding.Right + pnlMonogram.Width + 5);
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
				// If there's no sibling uncertain phones, then
				// shrink the popup to show only the count values.
				lblUncertaintyHeading.Height = 0;
				lblSiblingPhones.Height = 0;
				pnlCounts.Height = lblNonPrimary.Bottom;
				return false;
			}

			lblUncertaintyHeading.Height = m_origUncertaintyHeadingHeight;
			pnlCounts.Height = m_countPanelOrigHeight;

			// Weed out duplicates.
			List<string> tmpSiblings = new List<string>();
			foreach (string sibling in siblingUncertainties)
			{
				if (!tmpSiblings.Contains(sibling))
					tmpSiblings.Add(sibling);
			}

			// Now build the display string.
			StringBuilder bldr = new StringBuilder();
			foreach (string sibling in tmpSiblings)
				bldr.AppendFormat("{0}, ", sibling);

			lblSiblingPhones.Text = bldr.ToString().TrimEnd(", ".ToCharArray());

			// Set the desired height of the sibling phones list so all the phones are visible.
			TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
				TextFormatFlags.NoPadding | TextFormatFlags.WordBreak;

			lblSiblingPhones.Height =
				TextRenderer.MeasureText(lblSiblingPhones.Text, lblSiblingPhones.Font,
				lblSiblingPhones.ClientRectangle.Size, flags).Height + m_extraUncertainListHeight;

			return true;
		}

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Give the heading a shading consistent with all the other PA popup headings. Then
		/// draw the arrow glyph in the appropriate place.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlHeading_Paint(object sender, PaintEventArgs e)
		{
			if (m_hostingPopup == null)
				return;

			Rectangle rc = pnlHeading.ClientRectangle;

			// Draw the color shading in the heading
			m_hostingPopup.PaintHeadingBackground(e.Graphics, rc);

			// Figure out the height of the row that owns the associated cell and calculate
			// the vertical midpoint in that row. Since the top of the heading is even with
			// the top of the row containing the associated cell, we can figure out where
			// the arrow glyph should go so it points to an imaginary line that goes
			// horizontally through the midpoint of the cell for whom the popup belongs.
			if (m_hostingPopup.AssociatedGrid != null && m_hostingPopup.DrawArrow)
			{
				int row = m_hostingPopup.AssociatedCell.RowIndex;
				int arrowTipsY = m_hostingPopup.AssociatedGrid.Rows[row].Height / 2;
				m_hostingPopup.PaintArrow(e.Graphics, arrowTipsY, rc, m_hostingPopup.DrawLeftArrow);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Give shading to the area below the heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlInfo_Paint(object sender, PaintEventArgs e)
		{
			if (m_hostingPopup != null)
				m_hostingPopup.PaintBodyBackground(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the line separating the phone counts from the sibling uncertain phone info.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlCounts_Paint(object sender, PaintEventArgs e)
		{
			if (!m_siblingUncertaintiesExist)
				return;

			Rectangle rc = pnlCounts.ClientRectangle;
			int dy = rc.Height - 2;

			Point pt1 = new Point(rc.Width / 2, dy);
			Point pt2 = new Point(10, dy);
			using (LinearGradientBrush br = new LinearGradientBrush(pt1, pt2,
				SilPopup.kHeadDarkColor, SilPopup.kBodyDarkColor))
			{
				e.Graphics.DrawLine(new Pen(br, 1), pt1, pt2);
			}

			pt2 = new Point(rc.Width - 11, dy);
			using (LinearGradientBrush br = new LinearGradientBrush(pt1, pt2,
				SilPopup.kHeadDarkColor, SilPopup.kBodyDarkColor))
			{
				e.Graphics.DrawLine(new Pen(br, 1), pt1, pt2);
			}
		}

		#endregion

		private void lblPhone_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}
