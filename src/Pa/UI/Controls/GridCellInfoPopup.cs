// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Localization;
using SIL.Pa.Model;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class GridCellInfoPopup : SilPopup
	{
		/// ------------------------------------------------------------------------------------
		public enum Purpose
		{
			/// <summary></summary>
			ExperimentalTranscription,
			/// <summary></summary>
			UncertainPossibilities
		}

		private const int kPopupHeadingVPadding = 15;
		private const int kPopupHeadingBodyGap = 5;
		private const int kPopupGapBetweenItems = 10;
		private const int kPopupBottomMargin = 10;

		private string m_headingText = string.Empty;
		private TextFormatFlags m_hdgTxtFmtFlags;
		private int m_popupListItemHeight;
		private Purpose m_purposeIndicator;
		private WordCacheEntry m_cacheEntry;
		private int m_hdgTextSidePadding;

		/// ------------------------------------------------------------------------------------
		public GridCellInfoPopup()
		{
			InitializeComponent();

			lnkCommand.Font = FontHelper.UIFont;
			lnkHelp.Font = FontHelper.UIFont;
			lnkHelp.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the heading text for the popup. This also determines the height for the
		/// heading and returns the width calculated for the heading's text
		/// (without any padding).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SetHeadingText(string text, Font fnt)
		{
			m_headingText = text;
			pnlHeading.Font = fnt;
			m_hdgTxtFmtFlags = TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;

			if (text.IndexOf('\n') < 0)
			{
				m_hdgTxtFmtFlags |=
					(TextFormatFlags.SingleLine | TextFormatFlags.HorizontalCenter);
			}

			Size sz = TextRenderer.MeasureText(text, fnt, Size.Empty, m_hdgTxtFmtFlags);

			// Add some vertical padding.
			int newHeight = (sz.Height + kPopupHeadingVPadding);
			pnlHeading.Height = sz.Height + kPopupHeadingVPadding;
			
			// Make sure the body and link area are still the same
			// size after resizing the heading panel.
			Height += newHeight;

			return sz.Width;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Measure's the height of the popup's body (i.e. the part below the heading and
		/// above the link labels).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void MeasureBodyHeight(Font fnt, int numberOfItems)
		{
			const TextFormatFlags flags = TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.Left | TextFormatFlags.NoPadding |
				TextFormatFlags.LeftAndRightPadding;

			int lineLabelAreaHeight = ClientSize.Height - dyLinkLabelSeparatorLine + 1;
			Size sz = TextRenderer.MeasureText("X", fnt, Size.Empty, flags);
			m_popupListItemHeight = sz.Height + kPopupGapBetweenItems;

			int bodyHeight = (m_popupListItemHeight * numberOfItems) +
				kPopupBottomMargin + kPopupHeadingBodyGap;

			// Add 2 for the bottom and top single borders.
			Height = pnlHeading.Height + bodyHeight + lineLabelAreaHeight + 2;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the Y value of where the separator line goes between the body and the link
		/// label area.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int dyLinkLabelSeparatorLine
		{
			get { return lnkHelp.Top - 7; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle where the first item should be displayed in the list of those
		/// displayed in the body. This rectangle spans the entire popup's width so it is up
		/// to the caller to adjust the rectangle for any desired left or right margins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Rectangle FirstItemRectangle
		{
			get
			{
				Rectangle rc = ClientRectangle;
				rc.Y = pnlHeading.Bottom + 1 + kPopupHeadingBodyGap;
				rc.Height = m_popupListItemHeight;
				return rc;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the amount of padding on either side of the heading text. This
		/// is only used when the heading text is not centered horizontally.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int HeadingTextSidePadding
		{
			get { return m_hdgTextSidePadding; }
			set { m_hdgTextSidePadding = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the cache entry this cell popup is for. This is not used internally to
		/// this class. It is provided for consumer's of this control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordCacheEntry CacheEntry
		{
			get { return m_cacheEntry; }
			set { m_cacheEntry = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating what purpose the popup is for. This value is used
		/// by comsumer's of the popup control, not internally to this control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Purpose PurposeIndicator
		{
			get { return m_purposeIndicator; }
			set
			{
				m_purposeIndicator = value;

				if (value == Purpose.UncertainPossibilities)
				{
					lnkCommand.Text = LocalizationManager.GetString(
						"Views.WordLists.CellInfoPopup.UncertainPhones.EditLink.Text", "Edit",
						"Text for command link on the uncertainty possibilities cell popup");

					m_toolTip.SetToolTip(lnkCommand, LocalizationManager.GetString(
						"Views.WordLists.CellInfoPopup.UncertainPhones.EditLink.ToolTip",
						"Edit source record"));
				}
				else
				{
					lnkCommand.Text = LocalizationManager.GetString(
						"Views.WordLists.CellInfoPopup.TranscriptionChanges.SettingsLink.Text", "Settings",
						"Text for command link on the experimental transcription items cell popup");

					m_toolTip.SetToolTip(lnkCommand, LocalizationManager.GetString(
						"Views.WordLists.CellInfoPopup.TranscriptionChanges.SettingsLink.ToolTip",
						"Transcription Changes"));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public Panel HeadingPanel
		{
			get { return pnlHeading; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the command link label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LinkLabel CommandLink
		{
			get { return lnkCommand; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the help link label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LinkLabel HelpLink
		{
			get { return lnkHelp; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the cell associated with this popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridViewCell AssociatedCell { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the grid associated with this popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataGridView AssociatedGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if the mouse is over this popup or its associated cell. If not over either
		/// then hide this popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnTimerTick()
		{
 			base.OnTimerTick();

			if (AssociatedCell == null || AssociatedGrid == null || m_mouseOver)
				return;

			var frm = AssociatedGrid.FindForm();
			if (frm != null && !frm.ContainsFocus)
				return;

			// Get the rectangle for the associated cell.
			var rc = AssociatedGrid.GetCellDisplayRectangle(
				AssociatedCell.ColumnIndex, AssociatedCell.RowIndex, false);

			var pt = AssociatedGrid.PointToClient(MousePosition);
			if (!rc.Contains(pt))
				Hide();
			else
			{
				// At this point we know we're over our associated cell and the popup is
				// showing. However, the associated grid still needs to process cell mouse
				// move message but it won't get any as long as this popup is showing
				// (ToolStripDropDowns tend to suspend some events). Therefore, we'll have
				// to force the issue by using reflection to send the associated grid
				// cell mouse move messages over the associated cell.
			    const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.InvokeMethod;

			    var args = new DataGridViewCellMouseEventArgs(AssociatedCell.ColumnIndex,
					AssociatedCell.RowIndex, pt.X - rc.X, pt.Y - rc.Y,
					new MouseEventArgs(MouseButtons, 0, pt.X, pt.Y, 0));
				
			    typeof(DataGridView).InvokeMember("OnCellMouseMove",
			        flags | BindingFlags.Instance, null, AssociatedGrid,
					new object[] {args});
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text for the heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HeadingText
		{
			get { return m_headingText; }
			set
			{
				m_headingText = value;
				pnlHeading.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text formatting flags for the heading's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TextFormatFlags HeadingTextFomatFlags
		{
			get { return m_hdgTxtFmtFlags; }
			set
			{
				m_hdgTxtFmtFlags = value;
				pnlHeading.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the popup is active (i.e. is showing).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Active()
		{
			return m_owningDropDown.Visible;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the popup is active and is for the
		/// specified purpose.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Active(Purpose purposeIndicator)
		{
			return (m_purposeIndicator == purposeIndicator && Active());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hides the popup only if the popup's current purpose indicator is the same as that
		/// specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Hide(Purpose purposeIndicator)
		{
			if (m_purposeIndicator == purposeIndicator && Active())
				Hide();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Center the link labels with a 10 pixel gap between them.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (lnkCommand != null && lnkHelp != null)
			{
				// 10 is the gap that should be between them.
				int lnkLabelWidths = lnkCommand.Width + lnkHelp.Width + 10;
				lnkCommand.Left = (Width - lnkLabelWidths) / 2;
				lnkHelp.Left = lnkCommand.Right + 10;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			// Fill-in the entire background.
			//Color clr1 = Color.FromArgb(110, Color.Wheat);
			//using (SolidBrush brWhite = new SolidBrush(Color.White))
			//using (LinearGradientBrush br = new LinearGradientBrush(rcRecCount, clr1, Color.White, 0f))
			//{
			//    e.Graphics.FillRectangle(brWhite, rcRecCount);
			//    e.Graphics.FillRectangle(br, rcRecCount);
			//}

			PaintBodyBackground(e.Graphics);

			// Draw a line to separate the link labels from the body of the popup.
			Point pt1 = new Point(0, dyLinkLabelSeparatorLine);
			Point pt2 = new Point(ClientSize.Width, pt1.Y);

			using (Pen pen = new Pen(Color.Black))
				e.Graphics.DrawLine(pen, pt1, pt2);
		}

		/// ------------------------------------------------------------------------------------
		private void pnlHeading_Paint(object sender, PaintEventArgs e)
		{
			var rc = pnlHeading.ClientRectangle;
			PaintHeadingBackground(e.Graphics, rc);

			if (!string.IsNullOrEmpty(m_headingText))
			{
				var rcText = rc;
				rcText.Inflate(-m_hdgTextSidePadding, 0);

				// Draw the heading text
				TextRenderer.DrawText(e.Graphics, m_headingText, pnlHeading.Font, rcText,
					Color.Black, m_hdgTxtFmtFlags);
			}

			// Figure out the height of the row that owns the associated cell and calculate
			// the vertical midpoint in that row. Since the top of the heading is even with
			// the top of the row containing the associated cell, we can figure out where
			// the arrow glyph should go so it points to an imaginary line that goes
			// horizontally through the midpoint of the cell for whom the popup belongs.
			int rowIndex = (AssociatedCell != null ? AssociatedCell.RowIndex : -1);
			if (AssociatedGrid != null && rowIndex >= 0 && rowIndex < AssociatedGrid.RowCount)
			{
				int arrowTipsY = AssociatedGrid.Rows[rowIndex].Height / 2;
				PaintArrow(e.Graphics, arrowTipsY, rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hides the popup when one of the links is clicked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleLinkClick(object sender, EventArgs e)
		{
			Hide();

			if (sender == lnkHelp)
			{
				App.ShowHelpTopic(m_purposeIndicator == Purpose.ExperimentalTranscription ?
					"hidExperimentalTranscriptionsPopup" : "hidUncertainPhonesPopup");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the popup has focus, using the up, down or esc will allow the user to get
		/// back to scrolling through the word list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Down || keyData == Keys.Up || keyData == Keys.Escape)
				Hide();
		
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}

