using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using SIL.Pa.FFSearchEngine;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa.Controls
{
	public partial class Histogram : UserControl, IxCoreColleague
	{
		private const int kMagnifiedCharSize = 22;
		private const int kPhoneLabelWidth = 40;
		private const int kLineGapSize = 25;
		private const int kPixelsFromTop = 10;

		private int m_maxTotalCount = 0;
		private int m_phoneHeight = 0;
		private decimal m_barHeightFactor = 0;
		private readonly ToolTip m_phoneToolTip;
		private readonly PhoneInfoPopup m_phoneInfoPopup;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Histogram()
		{
			InitializeComponent();

			m_phoneToolTip = new ToolTip();
			m_phoneToolTip.OwnerDraw = true;
			m_phoneToolTip.Draw += HandlePhoneToolTipDraw;
			m_phoneToolTip.Popup += HandlePhoneToolTipPopup;
			
			base.DoubleBuffered = true;
			
			pnlFixedBorder.Dock = DockStyle.Fill;
			pnlFixedBorder.BringToFront();

			pnlPhones.BackColor = Color.Transparent;
			pnlBars.BorderStyle = BorderStyle.None;
			pnlYaxis.BorderStyle = BorderStyle.None;

			m_phoneInfoPopup = new PhoneInfoPopup();
		}

		#region Loading query from query source
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create the appropriate labels and bars for the phones based on their placement 
		/// in the chart's grid.
		/// </summary>
		/// <param name="phoneList">List<string></param>
		/// <returns>bool</returns>
		/// ------------------------------------------------------------------------------------
		public bool LoadPhones(List<CharGridCell> phoneList)
		{
			pnlScroller.AutoScrollPosition = new Point(0, 0);
			pnlBars.Left = 0;

			// Dispose of the labels and clear the control arrays.
			pnlPhones.Controls.Clear();
			pnlBars.Controls.Clear();

			int xLocationOffset = 0;
			m_maxTotalCount = 0;

			foreach (CharGridCell cgc in phoneList)
			{
				// Create phone labels that appear under the bar.
				Label lblPhone = new Label();
				lblPhone.Font = FontHelper.MakeEticRegFontDerivative(16);
				lblPhone.Text = cgc.Phone;
				lblPhone.Paint += lbl_Paint;
				lblPhone.MouseEnter += HandleMouseEnter;
				lblPhone.MouseDoubleClick += HandleMouseDoubleClick;
				lblPhone.Location = new Point(xLocationOffset, 2);
				lblPhone.AutoSize = true;
				pnlPhones.Controls.Add(lblPhone);
				m_phoneHeight = lblPhone.Height;
				lblPhone.AutoSize = false;
				lblPhone.Size = new Size(kPhoneLabelWidth, m_phoneHeight);
				lblPhone.BringToFront();

				// Set the phone's magnified tooltip.
				m_phoneToolTip.SetToolTip(lblPhone, lblPhone.Text);

				// Create the bars.
				HistogramBar histBar = new HistogramBar();
				histBar.InitializeBar(cgc);
				histBar.MouseEnter += HandleMouseEnter;
				histBar.MouseDoubleClick += HandleMouseDoubleClick;
				pnlBars.Controls.Add(histBar);

				// Link the phone's label with its bar and vice versa.
				lblPhone.Tag = histBar;
				histBar.Tag = lblPhone;

				// Determine the tallest bar
				if (histBar.BarValue > m_maxTotalCount)
					m_maxTotalCount = histBar.BarValue;

				xLocationOffset += kPhoneLabelWidth; // pixels between the characters
			}

			pnlPhones.Width = (pnlPhones.Controls.Count * kPhoneLabelWidth);

			// Account for the fact that each phone's Y location is 2.
			m_phoneHeight += 2;

			// Make sure the labels have enough vertical space. This will increase
			// room for the labels when the scroll bar appears underneath them and
			// decrease room when the scroll bar disappears. In other words, this
			// calculation will make sure there's only enough vertical space for
			// the phone labels as is necessary so they won't get clipped.
			if (m_phoneHeight != pnlScroller.ClientSize.Height)
				pnlScroller.Height += (m_phoneHeight - pnlScroller.ClientSize.Height);

			// Add 5 additional pixels of space between the right-most bar and border
			pnlBars.Width = (pnlPhones.Controls.Count * kPhoneLabelWidth) + 5;

			// Force the bars to be resized.
			pnlFixedBorder_Resize(null, null);

			return true;
		}

		#endregion      

		#region Tooltips
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines the correct ToolTip size based on the font size
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneToolTipPopup(object sender, PopupEventArgs e)
		{
			// Only use custom toolTips for phone labels
			using (Font fnt = FontHelper.MakeEticRegFontDerivative(kMagnifiedCharSize))
			{
				e.ToolTipSize = TextRenderer.MeasureText(
					m_phoneToolTip.GetToolTip(e.AssociatedControl), fnt);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the ToolTip with the magnified font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneToolTipDraw(Object sender, DrawToolTipEventArgs e)
		{
			// Only use custom toolTips for IpaCharacter labels
			e.Graphics.FillRectangle(SystemBrushes.Info, e.Bounds);
			e.DrawBorder();

			TextFormatFlags flags = TextFormatFlags.NoPrefix |
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

			using (Font fnt = FontHelper.MakeEticRegFontDerivative(kMagnifiedCharSize))
			{
				TextRenderer.DrawText(e.Graphics, e.ToolTipText, fnt, e.Bounds,
					SystemColors.InfoText, flags);
			}
		}

		#endregion

		#region Misc. Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Perform a search anywhere when the user clicks on a phone or it's bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseDoubleClick(object sender, MouseEventArgs e)
		{
			HistogramBar bar = sender as HistogramBar;
			Label lbl = sender as Label;
			string srchPhone = (bar != null ? ((Label)bar.Tag).Text : lbl.Text);

			SearchQuery query = new SearchQuery();
			query.Pattern = srchPhone + "/*_*";
			query.IgnoreDiacritics = false;
			PaApp.MsgMediator.SendMessage("ViewFindPhones", query);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the bar's value in a label just above the bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseEnter(object sender, EventArgs e)
		{
			HistogramBar bar = sender as HistogramBar;
			Label lbl = sender as Label;
			bool useLabelForInfoPopup = (bar == null);
			
			if (bar == null)
			{
				if (lbl != null)
					bar = lbl.Tag as HistogramBar;

				m_phoneToolTip.Show(lbl.Text, this);
			}

			if (bar != null && m_phoneInfoPopup.Initialize(bar.CharGridCellPhoneInfo))
				m_phoneInfoPopup.Show(useLabelForInfoPopup ? lbl : bar, bar);
		}

		#endregion

		#region Scrolling/Resizing events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshLayout()
		{
			pnlFixedBorder_Resize(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Since the panel the bars are on is not owned by the scrolling panel, make sure the
		/// left edge of the panel that owns the bars is syncronized with the ipa character
		/// panel as it is scrolled.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlScroller_Scroll(object sender, ScrollEventArgs e)
		{
			if (pnlBars.Left != -e.NewValue)
				pnlBars.Left = -e.NewValue;

			pnlBars.Invalidate();
		}

		private bool m_ignoreFixedBorderResize = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the height of each bar based on the tipHeight of their owning panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlFixedBorder_Resize(object sender, EventArgs e)
		{
			if (m_maxTotalCount == 0 || m_ignoreFixedBorderResize || PaApp.DesignMode)
				return;

			// Make sure the labels have enough vertical space. This will increase
			// room for the labels when the scroll bar appears underneath them and
			// decrease room when the scroll bar disappears. In other words, this
			// calculation will make sure there's only enough vertical space for
			// the phone labels as is necessary so they won't get clipped.
			if (m_phoneHeight != pnlScroller.ClientSize.Height)
			{
				m_ignoreFixedBorderResize = true;
				pnlScroller.Height += (m_phoneHeight - pnlScroller.ClientSize.Height);
				m_ignoreFixedBorderResize = false;
			}

			STUtils.SetWindowRedraw(pnlBars, false, false);

			m_barHeightFactor =
				decimal.Divide(pnlBars.ClientSize.Height - kPixelsFromTop, m_maxTotalCount);

			int xLocationOffset = 0;

			// Reposition and resize bars
			foreach (HistogramBar bar in pnlBars.Controls)
			{
				int maxBarHeight = (int)((m_maxTotalCount - bar.BarValue) * m_barHeightFactor);

				// "5" is the column spacing on either side of a bar
				Point newLoc = new Point((xLocationOffset + 5), (maxBarHeight + kPixelsFromTop + 1));
				Size newSize = new Size(30, (int)(bar.BarValue * m_barHeightFactor));
				xLocationOffset += kPhoneLabelWidth; // pixels between the characters

				if (newSize != bar.Size)
					bar.Size = newSize;

				if (newLoc != bar.Location)
					bar.Location = newLoc;
			}

			STUtils.SetWindowRedraw(pnlBars, true, true);

			pnlScroller_Scroll(null, new ScrollEventArgs(ScrollEventType.EndScroll,
				-pnlScroller.AutoScrollPosition.X));
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint method for IPA Character labels.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lbl_Paint(object sender, PaintEventArgs e)
		{
			Label lbl = sender as Label;

			if (lbl == null)
				return;

			// Draw the label's text.
			using (StringFormat sf = STUtils.GetStringFormat(true))
			{
				e.Graphics.FillRectangle(SystemBrushes.Control, lbl.ClientRectangle);

				// Make sure the phone will fit in the label without being clipped. If it
				// doesn't then keep decreasing the font size by one point until it's
				// narrow enough to fit into the lable.
				Font fnt = lbl.Font;
				int phoneWidth = TextRenderer.MeasureText(e.Graphics, lbl.Text, fnt).Width;
				if (lbl.Width >= phoneWidth)
					fnt = null;
				else
				{
					while (phoneWidth > lbl.Width)
					{
						fnt = FontHelper.MakeFont(fnt, fnt.SizeInPoints - 1);
						phoneWidth = TextRenderer.MeasureText(e.Graphics, lbl.Text, fnt).Width;
					}
				}

				// If fnt is null it means the phone fits into the label without having
				// to shrink it down. Otherwise, fnt represents a smaller font than lbl.Font
				// and therefore, it's not referencing lbl.Font so it wil need to be
				// disposed after drawing the phone.
				if (fnt == null)
				{
					e.Graphics.DrawString(lbl.Text, lbl.Font,
						SystemBrushes.ControlText, lbl.ClientRectangle, sf);
				}
				else
				{
					e.Graphics.DrawString(lbl.Text, fnt, SystemBrushes.ControlText,
						lbl.ClientRectangle, sf);
					fnt.Dispose();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the horizontal lines on the pnlBars panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlBars_Paint(object sender, PaintEventArgs e)
		{
			using (Graphics g = pnlFixedBorder.CreateGraphics())
			using (SolidBrush br = new SolidBrush(pnlBars.BackColor))
			using (Pen pen = new Pen(Color.FromArgb(100, SystemColors.GrayText)))
			{
				e.Graphics.FillRectangle(br, pnlBars.ClientRectangle);
				g.FillRectangle(br, pnlFixedBorder.ClientRectangle);

				int yLocationOffset = pnlBars.ClientSize.Height - kLineGapSize;

				while (yLocationOffset > 0)
				{
					// Define line's end points.
					Point pt1 = new Point(0, yLocationOffset);
					Point pt2 = new Point(pnlBars.Width, yLocationOffset);

					e.Graphics.DrawLine(pen, pt1, pt2);
					yLocationOffset -= kLineGapSize;

					// Draw the line on the bar panel's owner in case the bar panel
					// doesn't extend to its owner's right edge.
					pt2.X = pnlFixedBorder.Width;
					g.DrawLine(pen, pt1, pt2);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create & place the horizontal line value numbers on the pnlBars panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlYaxis_Paint(object sender, PaintEventArgs e)
		{
			if (PaApp.DesignMode)
				return;

			decimal horzLineValue = 0;
			string horzLineValString = string.Empty;

			// Calculate (relative to pnlYaxis) where the bottom of the bar's panel is.
			Point pt = pnlBars.PointToScreen(new Point(0, pnlBars.ClientSize.Height));
			pt = pnlYaxis.PointToClient(pt);
			int yLocationOffset = pt.Y - (int)(kLineGapSize * 1.5);

			// Calculate (relative to pnlYaxis) where we should stop drawing numbers.
			pt = pnlBars.PointToScreen(new Point(0, 0));
			pt = pnlYaxis.PointToClient(pt);
			int minY = pt.Y;

			Rectangle rc =
				new Rectangle(0, yLocationOffset, pnlYaxis.ClientSize.Width - 4, kLineGapSize);

			using (StringFormat sf = STUtils.GetStringFormat(true))
			{
				sf.Alignment = StringAlignment.Far;

				while ((rc.Top + (rc.Height / 2)) > minY)
				{
					// Must round to 2 decimal places to update the line values more frequently.
					if (m_barHeightFactor != 0)
					{
						horzLineValue += Math.Round((kLineGapSize / m_barHeightFactor), 2);
						// Show 2 decimal places on the horz line value numbers if the tallest
						// bar is less than the number of horz lines
						if (m_maxTotalCount < pnlBars.ClientSize.Height / kLineGapSize)
							horzLineValString = Math.Round(horzLineValue, 2).ToString();
						else
							horzLineValString = Math.Round(horzLineValue).ToString();
					}

					e.Graphics.DrawString(horzLineValString, FontHelper.UIFont,
						SystemBrushes.ControlText, rc, sf);

					rc.Y -= kLineGapSize;
				}
			}
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}

	#region HistogramBar Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class HistogramBar : Label
	{
		private CharGridCell m_cgcPhoneInfo;

		private readonly Color m_clrRight = ColorHelper.CalculateColor(Color.White,
			SystemColors.GradientActiveCaption, 110);

		private readonly Color m_clrLeft = ColorHelper.CalculateColor(Color.White,
			SystemColors.ActiveCaption, 70);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// HistogramBar constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public HistogramBar()
		{
			Size = new Size(30, 22);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeBar(CharGridCell cgc)
		{
			m_cgcPhoneInfo = cgc;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			// Fill in the bar.
			using (LinearGradientBrush br =
				new LinearGradientBrush(ClientRectangle, m_clrLeft, m_clrRight, 120))
			{
				e.Graphics.FillRectangle(br, ClientRectangle);
			}

			// Draw a border along the left, top and right sides of the bar.
			using (Pen pen = new Pen(SystemColors.ActiveCaption))
			{
				e.Graphics.DrawLines(pen, new Point[] {new Point(0, Height),
					new Point(0, 0), new Point(Width - 1, 0), new Point(Width - 1, Height)});
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get and set BarValue.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BarValue
		{
			get
			{
				return m_cgcPhoneInfo.TotalCount +
					m_cgcPhoneInfo.CountAsPrimaryUncertainty +
					m_cgcPhoneInfo.CountAsNonPrimaryUncertainty;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the bar's associated phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Phone
		{
			get { return m_cgcPhoneInfo.Phone; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridCell CharGridCellPhoneInfo
		{
			get { return m_cgcPhoneInfo; }
		}

		#endregion
	}

	#endregion
}
