// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class PhoneInfoPopupContent : UserControl
	{
		private readonly int m_extraMonogramHeight;
		private readonly int m_extraUncertainListHeight;
		private readonly int m_origUncertaintyHeadingHeight;
		private readonly int m_countPanelOrigHeight;
		private readonly PhoneInfoPopup m_hostingPopup;
		private bool m_siblingUncertaintiesExist;

		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopupContent()
		{
			InitializeComponent();
			base.DoubleBuffered = true;

			m_origUncertaintyHeadingHeight = lblUncertaintyHeading.Height;
			m_countPanelOrigHeight = pnlCounts.Height;
			m_extraMonogramHeight = Settings.Default.CVChartExtraPopupMonogramHeight;
			m_extraUncertainListHeight = Settings.Default.CVChartExtraPopupUncertainListHeight;
		}

		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopupContent(PhoneInfoPopup hostingPopup) : this()
		{
			m_hostingPopup = hostingPopup;
		}

		/// ------------------------------------------------------------------------------------
		internal void Initialize(IPhoneInfo phoneInfo)
		{
			lblMonogram.Text = phoneInfo.Phone;
			lblNormallyCount.Text = phoneInfo.TotalCount.ToString();
			lblPrimaryCount.Text = phoneInfo.CountAsPrimaryUncertainty.ToString();
			lblNonPrimaryCount.Text = phoneInfo.CountAsNonPrimaryUncertainty.ToString();
			RefreshFonts();
			m_siblingUncertaintiesExist = SetSiblingUncertainties(phoneInfo.SiblingUncertainties);
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshFonts()
		{
			// Make the phone monogram label 15 point, regardless of the user's setting.
			lblMonogram.Font = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, 15);

			// Limit the sibling uncertainty list's font size to 14 point.
			lblSiblingPhones.Font = (App.PhoneticFont.SizeInPoints > 14 ?
				FontHelper.MakeRegularFontDerivative(App.PhoneticFont, 14) : App.PhoneticFont);

			// Now make sure the phone fully fits into the black box (i.e. the monogram
			// label. If it doesn't, then make it wide enough to accomodate the phone.
			// That also means the heading label needs to be made narrower and moved to
			// the right a bit.
			using (Graphics g = lblMonogram.CreateGraphics())
			{
				const TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
					TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
					TextFormatFlags.NoClipping;

				var sz = TextRenderer.MeasureText(g, lblMonogram.Text,
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
			var tmpSiblings = siblingUncertainties.Distinct().ToList();

			// Now build the display string.
			var bldr = new StringBuilder();
			foreach (var sibling in tmpSiblings)
				bldr.AppendFormat("{0}, ", sibling);

			lblSiblingPhones.Text = bldr.ToString().TrimEnd(',', ' ');

			// Set the desired height of the sibling phones list so all the phones are visible.
			const TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
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

			var rc = pnlHeading.ClientRectangle;

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

			var rc = pnlCounts.ClientRectangle;
			int dy = rc.Height - 2;

			var pt1 = new Point(rc.Width / 2, dy);
			var pt2 = new Point(10, dy);
			using (var br = new LinearGradientBrush(pt1, pt2, SilPopup.kHeadDarkColor, SilPopup.kBodyDarkColor))
				e.Graphics.DrawLine(new Pen(br, 1), pt1, pt2);

			pt2 = new Point(rc.Width - 11, dy);
			using (var br = new LinearGradientBrush(pt1, pt2, SilPopup.kHeadDarkColor, SilPopup.kBodyDarkColor))
				e.Graphics.DrawLine(new Pen(br, 1), pt1, pt2);
		}

		#endregion
	}
}
