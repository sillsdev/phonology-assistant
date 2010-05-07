using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using SIL.Localization;
using SilUtils;
using SilUtils.Controls;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class XYChartCellInfoPopup : SilPopup
	{
		private enum MsgType
		{
			NonExistentPhones,
			InvalidCharacters,
			Exception,
			Other
		}

		private bool m_drawLeftArrow = true;
		private bool m_drawArrow = true;
		private Timer m_popupTimer;
		private Point m_popupLocation;
		private DataGridViewCell m_associatedCell;
		private readonly DataGridView m_associatedGrid;
		private readonly Font m_eticBold;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public XYChartCellInfoPopup(DataGridView associatedGrid)
		{
			InitializeComponent();
			m_associatedGrid = associatedGrid;

			m_popupTimer = new Timer();
			m_popupTimer.Interval = 700;
			m_popupTimer.Tick += m_popupTimer_Tick;
			m_popupTimer.Stop();

			m_eticBold = FontHelper.MakeFont(FontHelper.PhoneticFont, FontStyle.Bold);
			m_lblMsg.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_eticBold.Dispose();

				if (m_popupTimer != null)
				{
					m_popupTimer.Tick -= m_popupTimer_Tick;
					m_popupTimer.Dispose();
					m_popupTimer = null;
				}
			}

			base.Dispose(disposing);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern">Search pattern string to display.</param>
		/// <param name="associatedCell">Cell associated with the popup.</param>
		/// ------------------------------------------------------------------------------------
		public void Initialize(string pattern, DataGridViewCell associatedCell)
		{
			Initialize(pattern, associatedCell, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern">Search pattern string to display.</param>
		/// <param name="associatedCell">Cell associated with the popup.</param>
		/// <param name="informationMsg">Informational message to display.</param>
		/// ------------------------------------------------------------------------------------
		public void Initialize(string pattern, DataGridViewCell associatedCell,
			string informationMsg)
		{
			Debug.Assert(associatedCell != null);

			m_associatedCell = associatedCell;
			m_lblPattern.Font = m_eticBold;
			m_lblPattern.Text = pattern;
			
			int minWidth = m_lblPattern.PreferredWidth + m_lblPattern.Margin.Left +
				m_lblPattern.Margin.Right;

			m_tblLayout.MinimumSize = new Size(minWidth, 0);
			MaximumSize = new Size(Math.Max(minWidth + Padding.Left + Padding.Right, 250), 0);

			MsgType msgType = MsgType.Other;

			if (informationMsg != null)
			{
				if (m_associatedCell.Value is SearchQueryException)
					msgType = MsgType.Exception;
				
				InitializeLabels(msgType);
				m_lblInfo.Text = Utils.ConvertLiteralNewLines(informationMsg);
			}
			else
			{
				Debug.Assert(m_associatedCell.Tag != null);

				List<string> invalidItems = null;
				msgType = MsgType.NonExistentPhones;

				// Assume the information to display is a string
				// of invalidItems stored in the cell's tag property.
				if (associatedCell.Tag is string[])
					invalidItems = new List<string>(associatedCell.Tag as string[]);
				else if (associatedCell.Tag is char[])
				{
					msgType = MsgType.InvalidCharacters;
					invalidItems = new List<string>();
					foreach (char c in (associatedCell.Tag as char[]))
						invalidItems.Add(c.ToString());
				}
				
				InitializeLabels(msgType);

				StringBuilder bldr = new StringBuilder();
				if (invalidItems != null)
				{
					for (int i = 0; i < invalidItems.Count; i++)
					{
						if (msgType != MsgType.InvalidCharacters)
							bldr.Append(invalidItems[i]);
						else
						{
							var fmt = LocalizationManager.LocalizeString(
								"ChartPopupUndefinedSymbolFormatMsg", "{0} (U+{1})",
								"Views.Distribution Charts");

							bldr.AppendFormat(fmt, invalidItems[i],
								((int)invalidItems[i][0]).ToString("X4"));
						}

						if (i < invalidItems.Count - 1)
						{
							bldr.Append(msgType == MsgType.InvalidCharacters ?
								Environment.NewLine : ", ");
						}
					}
				}

				m_lblInfo.Text = bldr.ToString();
			}

			Width = m_tblLayout.PreferredSize.Width + Padding.Left + Padding.Right + 2;
			Height = m_tblLayout.Height + Padding.Top + Padding.Bottom + 2;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeLabels(MsgType msgType)
		{
			string msg;

			if (msgType == MsgType.Other || msgType == MsgType.Exception)
			{
				msg = XYGrid.PopupSyntaxErrorsMsg;
				m_lblInfo.Font = FontHelper.UIFont;
			}
			else if (msgType == MsgType.InvalidCharacters)
			{
				msg = XYGrid.PopupUndefinedSymbolsMsg;
				m_lblInfo.Font = FontHelper.PhoneticFont;
			}
			else
			{
				msg = XYGrid.PopupInvalidPhonesMsg;
				m_lblInfo.Font = FontHelper.PhoneticFont;
			}

			m_lblMsg.Text = Utils.ConvertLiteralNewLines(msg);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			Rectangle rc = ClientRectangle;
			PaintBodyBackground(e.Graphics);

			// Draw the color shading behind the search pattern.
			rc.Height = m_lblPattern.Height + Padding.Top + m_lblPattern.Top + m_lblPattern.Margin.Bottom;
			PaintHeadingBackground(e.Graphics, rc);

			if (m_drawArrow)
			{
				// Figure out the height of the row that owns the associated cell and calculate
				// the vertical midpoint in that row. Since the top of the heading is even with
				// the top of the row containing the associated cell, we can figure out where
				// the arrow glyph should go so it points to an imaginary line that goes
				// horizontally through the midpoint of the cell for whom the popup belongs.
				int arrowTipsY = m_associatedGrid.Rows[m_associatedCell.RowIndex].Height / 2;
				PaintArrow(e.Graphics, arrowTipsY, rc, m_drawLeftArrow);
			}

			DrawSeparatorLine(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawSeparatorLine(Graphics g)
		{
			Rectangle rc = ClientRectangle;

			Point pt1 = new Point(rc.Width / 2, m_lblMsg.Bottom + 6);
			Point pt2 = new Point(-5, m_lblMsg.Bottom + 6);
			using (LinearGradientBrush br = new LinearGradientBrush(pt1, pt2,
				kHeadDarkColor, kBodyDarkColor))
			{
				g.DrawLine(new Pen(br, 1), pt1, pt2);
			}

			pt2 = new Point(rc.Width + 4, m_lblMsg.Bottom + 6);
			using (LinearGradientBrush br = new LinearGradientBrush(pt1, pt2,
				kHeadDarkColor, kBodyDarkColor))
			{
				g.DrawLine(new Pen(br, 1), pt1, pt2);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if the mouse is over this popup or its associated cell. If not over either
		/// then hide this popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnTimerTick()
		{
			base.OnTimerTick();

			if (!IsMouseOverCellOrPopup())
				Hide();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if the mouse is over this popup or its associated cell. If not over either
		/// then hide this popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsMouseOverCellOrPopup()
		{
			if (m_associatedCell == null || m_associatedGrid == null || m_mouseOver)
				return true;

			// Get the rectangle for the associated cell.
			Rectangle rc = m_associatedGrid.GetCellDisplayRectangle(
				m_associatedCell.ColumnIndex, m_associatedCell.RowIndex, false);

			Point pt = m_associatedGrid.PointToClient(MousePosition);
			return rc.Contains(pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup next to the associated cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Show()
		{
			if (m_associatedGrid != null)
			{
				Form frm = m_associatedGrid.FindForm();
				if (frm != null && !frm.ContainsFocus)
					return;
			}

			m_popupTimer.Start();

			m_drawLeftArrow = true;

			// Get the rectangle for the associated cell.
			Rectangle rcCell = m_associatedGrid.GetCellDisplayRectangle(
				m_associatedCell.ColumnIndex, m_associatedCell.RowIndex, false);

			// Get the desired point, relative to the screen, where to show the popup.
			// The desired location is to the right of the associated cell.
			Point ptCell = m_associatedGrid.PointToScreen(rcCell.Location);
			Point ptPopup = new Point(ptCell.X + rcCell.Width - 1, ptCell.Y);

			bool tooWide;
			bool tooTall;
			CheckDesiredPopupLocation(ptPopup, out tooWide, out tooTall);

			// Determine the popup's display rectangle based on it's desired location and size.
			Rectangle rcPopup = new Rectangle(ptPopup, Size);

			// If the popup is too wide to be shown at the desired location then adjust
			// its X location to show it to the left of the cell.
			if (tooWide)
			{
				ptPopup.X = ptCell.X - rcPopup.Width + 1;
				m_drawLeftArrow = false;
			}

			// If the popup is too tall to be shown at the desired location, don't draw an
			// arrow and don't make any coordinate adjustments since .Net will make the
			// adjustment for us, automatically.
			m_drawArrow = !tooTall;

			m_popupLocation = m_associatedGrid.PointToClient(ptPopup);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void Hide()
		{
			base.Hide();
			m_popupTimer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_popupTimer_Tick(object sender, EventArgs e)
		{
			if (IsMouseOverCellOrPopup())
			{
				m_associatedGrid.Cursor = Cursors.Default;
				base.Show(m_associatedGrid, m_popupLocation);
				Application.DoEvents();
			}

			m_popupTimer.Stop();
		}
	}
}
