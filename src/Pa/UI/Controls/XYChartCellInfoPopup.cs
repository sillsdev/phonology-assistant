using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using SilUtils;
using SilUtils.Controls;

namespace SIL.Pa.UI.Controls
{
	public class XYChartCellInfoPopup : SilPopup
	{
		private enum MsgType
		{
			NonExistentPhones,
			BadCharacters,
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
		private readonly Font m_eticMsg;
		private readonly Label m_lblMsg;
		private readonly Label m_lblPattern;
		private readonly Label m_lblInfo;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public XYChartCellInfoPopup(DataGridView associatedGrid)
		{
			m_associatedGrid = associatedGrid;

			m_lblPattern = new Label();
			m_lblPattern.BackColor = Color.Transparent;
			m_lblPattern.ForeColor = Color.Black;
			Controls.Add(m_lblPattern);

			m_lblMsg = new Label();
			m_lblMsg.BackColor = Color.Transparent;
			m_lblMsg.ForeColor = Color.Black;
			Controls.Add(m_lblMsg);
		
			m_lblInfo = new Label();
			m_lblInfo.BackColor = Color.Transparent;
			m_lblInfo.ForeColor = Color.Black;
			Controls.Add(m_lblInfo);

			m_popupTimer = new Timer();
			m_popupTimer.Interval = 700;
			m_popupTimer.Tick += m_popupTimer_Tick;
			m_popupTimer.Stop();

			m_eticBold = FontHelper.MakeFont(FontHelper.PhoneticFont, FontStyle.Bold);
			m_eticMsg = FontHelper.MakeEticRegFontDerivative(10);

			Disposed += XYChartCellInfoPopup_Disposed;
		}

		///  ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void XYChartCellInfoPopup_Disposed(object sender, EventArgs e)
		{
			m_eticBold.Dispose();
			m_eticMsg.Dispose();
			Disposed -= XYChartCellInfoPopup_Disposed;
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

			Padding = new Padding(10);
			BorderStyle = BorderStyle.FixedSingle;

			m_associatedCell = associatedCell;
			m_lblPattern.Text = pattern;
			MsgType msgType = MsgType.Other;

			if (informationMsg != null)
			{
				if (m_associatedCell.Value is XYChartException)
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
					msgType = MsgType.BadCharacters;
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
						if (msgType != MsgType.BadCharacters)
							bldr.Append(invalidItems[i]);
						else
						{
							bldr.AppendFormat(
								Properties.Resources.kstidXYChartPopupInvalidCharFmt,
								invalidItems[i], ((int)invalidItems[i][0]).ToString("X4"));
						}

						if (i < invalidItems.Count - 1)
						{
							bldr.Append(msgType == MsgType.BadCharacters ?
								Environment.NewLine : ", ");
						}
					}
				}

				m_lblInfo.Text = bldr.ToString();
			}

			m_lblPattern.Size = m_lblPattern.PreferredSize;
			m_lblInfo.Size = (msgType != MsgType.Exception ?
				m_lblInfo.PreferredSize : CalculateExceptionInfoLabelSize());

			LocateLabels();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeLabels(MsgType msgType)
		{
			m_lblPattern.Location = new Point(Padding.Left, Padding.Top);
			m_lblPattern.Font = m_eticBold;

			string msg;

			if (msgType == MsgType.Other || msgType == MsgType.Exception)
			{
				msg = Properties.Resources.kstidXYChartPopupInfoSyntaxErrorsMsg;
				m_lblInfo.Font = FontHelper.UIFont;
			}
			else
			{
				msg = (msgType == MsgType.BadCharacters ?
					Properties.Resources.kstidXYChartPopupInfoBadCharsMsg :
					Properties.Resources.kstidXYChartPopupInfoInvalidPhonesMsg);

				m_lblInfo.Font = FontHelper.PhoneticFont;
			}

			m_lblMsg.Text = Utils.ConvertLiteralNewLines(msg);
			if (msgType == MsgType.Exception)
				m_lblMsg.Text = m_lblMsg.Text.Replace("\n", " ");

			m_lblMsg.Location = new Point(Padding.Left, m_lblPattern.Bottom + 10);
			m_lblMsg.Font = FontHelper.UIFont;
			m_lblMsg.Size = m_lblMsg.PreferredSize;

			m_lblInfo.Location = new Point(Padding.Left, m_lblMsg.Bottom +
				(msgType == MsgType.Other ? 13 : 10));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Size CalculateExceptionInfoLabelSize()
		{
			int maxWidth = (int)(m_lblMsg.Width * 1.1);
			Size sz = new Size(maxWidth, int.MaxValue);
			const TextFormatFlags kFlags = TextFormatFlags.Default | TextFormatFlags.WordBreak;

			m_lblInfo.TextAlign = ContentAlignment.MiddleLeft;
			sz = TextRenderer.MeasureText(m_lblInfo.Text, m_lblInfo.Font, sz, kFlags);
			sz.Height += 8;
			return sz;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LocateLabels()
		{
			int maxWidth = m_lblPattern.Width;
			maxWidth = Math.Max(maxWidth, m_lblMsg.Width);
			maxWidth = Math.Max(maxWidth, m_lblInfo.Width);
			Width = maxWidth + Padding.Left + Padding.Right + 2;
			Height = Padding.Top + m_lblInfo.Bottom + Padding.Bottom - 6;

			// Center the labels.
			if (m_lblPattern.Width != maxWidth)
				m_lblPattern.Left = (Width - m_lblPattern.Width) / 2;

			if (m_lblMsg.Width != maxWidth)
				m_lblMsg.Left = (Width - m_lblMsg.Width) / 2;

			if (m_lblInfo.Width != maxWidth)
				m_lblInfo.Left = (Width - m_lblInfo.Width) / 2;
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
				if (m_lblPattern != null && !m_lblPattern.IsDisposed)
					m_lblPattern.Dispose();

				if (m_lblMsg != null && !m_lblMsg.IsDisposed)
					m_lblMsg.Dispose();

				if (m_lblInfo != null && !m_lblInfo.IsDisposed)
					m_lblInfo.Dispose();

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
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			Rectangle rc = ClientRectangle;
			PaintBodyBackground(e.Graphics);

			// Draw the color shading behind the search pattern.
			rc.Height = m_lblPattern.Height + Padding.Top + (Padding.Top - 4);
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
