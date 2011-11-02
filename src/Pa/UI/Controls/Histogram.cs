using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public partial class Histogram : UserControl, IxCoreColleague
	{
		//private const int kMagnifiedCharSize = 22;
		private const int kPixelsFromTop = 10;

		private bool _ignoreFixedBorderResize;
		private int _maxTotalCount;
		private int _phoneHeight;
		private int _hashMarkIncrement;
		private readonly int _extraPhoneHeight;
		private readonly int _barWidth;
		private readonly int _phoneLabelWidth;
		private readonly int _hashMarkGap;
		private readonly int _phoneFontSize;
		private readonly PhoneInfoPopup _phoneInfoPopup;

		// Uncomment if the magnified tooltip of a histogram's phone is desired.
		//private readonly ToolTip m_phoneToolTip;

		/// ------------------------------------------------------------------------------------
		public Histogram()
		{
			InitializeComponent();

			_hashMarkGap =	Settings.Default.HistogramHashMarkGap;
			_phoneLabelWidth = Settings.Default.HistogramPhoneLabelWidth;
			_extraPhoneHeight = Settings.Default.HistogramExtraPhoneLabelHeight;
			_barWidth = Settings.Default.HistogramBarWidth;
			_phoneFontSize = Settings.Default.HistogramPhoneLabelFontSize;

			// Uncomment if the magnified tooltip of a histogram's phone is desired.
			//m_phoneToolTip = new ToolTip();
			//m_phoneToolTip.OwnerDraw = true;
			//m_phoneToolTip.Draw += HandlePhoneToolTipDraw;
			//m_phoneToolTip.Popup += HandlePhoneToolTipPopup;
			
			base.DoubleBuffered = true;
			
			pnlFixedBorder.Dock = DockStyle.Fill;
			pnlFixedBorder.BringToFront();

			pnlPhones.BackColor = Color.Transparent;
			pnlBars.BorderStyle = BorderStyle.None;
			pnlYaxis.BorderStyle = BorderStyle.None;

			_phoneInfoPopup = new PhoneInfoPopup();
		}

		#region Loading
		/// ------------------------------------------------------------------------------------
		public bool LoadPhones(IEnumerable<string> phoneList)
		{
			pnlScroller.AutoScrollPosition = new Point(0, 0);
			pnlBars.Left = 0;

			// Dispose of the labels and clear the control arrays.
			pnlPhones.Controls.Clear();
			pnlBars.Controls.Clear();

			int xLocationOffset = 0;
			_maxTotalCount = 0;
			_phoneHeight = 0;
			var fnt = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, _phoneFontSize);

			foreach (var phone in phoneList)
			{
				// Create phone labels that appear under the bar.
				var lblPhone = new Label();
				lblPhone.Font = fnt;
				lblPhone.Text = phone;
				lblPhone.Location = new Point(xLocationOffset, 2);
				lblPhone.AutoSize = false;
				lblPhone.Paint += lbl_Paint;
				lblPhone.MouseEnter += HandleMouseEnter;
				lblPhone.MouseDoubleClick += HandleMouseDoubleClick;
				pnlPhones.Controls.Add(lblPhone);

				if (_phoneHeight == 0)
				{
					_phoneHeight = TextRenderer.MeasureText(phone, lblPhone.Font).Height +
						_extraPhoneHeight;
				}
				
				lblPhone.Size = new Size(_phoneLabelWidth, _phoneHeight);
				lblPhone.BringToFront();

				// Set the phone's magnified tooltip.
				// Uncomment if the magnified tooltip of a histogram's phone is desired.
				//m_phoneToolTip.SetToolTip(lblPhone, lblPhone.Text);

				// Create the bars.
				var histBar = new HistogramBar(phone);
				histBar.MouseEnter += HandleMouseEnter;
				histBar.MouseDoubleClick += HandleMouseDoubleClick;
				pnlBars.Controls.Add(histBar);

				// Link the phone's label with its bar and vice versa.
				lblPhone.Tag = histBar;
				histBar.Tag = lblPhone;

				// Determine the tallest bar
				if (histBar.BarValue > _maxTotalCount)
					_maxTotalCount = histBar.BarValue;

				xLocationOffset += _phoneLabelWidth; // pixels between the characters
			}

			pnlPhones.Width = (pnlPhones.Controls.Count * _phoneLabelWidth);

			// Account for the fact that each phone's Y location is 2.
			_phoneHeight += 2;

			// Make sure the labels have enough vertical space. This will increase
			// room for the labels when the scroll bar appears underneath them and
			// decrease room when the scroll bar disappears. In other words, this
			// calculation will make sure there's only enough vertical space for
			// the phone labels as is necessary so they won't get clipped.
			if (_phoneHeight != pnlScroller.ClientSize.Height)
				pnlScroller.Height += (_phoneHeight - pnlScroller.ClientSize.Height);

			// Add 5 additional pixels of space between the right-most bar and border
			pnlBars.Width = (pnlPhones.Controls.Count * _phoneLabelWidth) + 5;

			// Force the bars to be resized.
			pnlFixedBorder_Resize(null, null);

			return true;
		}

		#endregion      

		#region Tooltips
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Determines the correct ToolTip size based on the font size
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void HandlePhoneToolTipPopup(object sender, PopupEventArgs e)
		//{
		//    // Only use custom toolTips for phone labels
		//    using (Font fnt = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, kMagnifiedCharSize))
		//    {
		//        e.ToolTipSize = TextRenderer.MeasureText(
		//            m_phoneToolTip.GetToolTip(e.AssociatedControl), fnt);
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Draws the ToolTip with the magnified font.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void HandlePhoneToolTipDraw(Object sender, DrawToolTipEventArgs e)
		//{
		//    // Only use custom toolTips for IpaCharacter labels
		//    e.Graphics.FillRectangle(SystemBrushes.Info, e.Bounds);
		//    e.DrawBorder();

		//    TextFormatFlags flags = TextFormatFlags.NoPrefix |
		//        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

		//    using (Font fnt = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, kMagnifiedCharSize))
		//    {
		//        TextRenderer.DrawText(e.Graphics, e.ToolTipText, fnt, e.Bounds,
		//            SystemColors.InfoText, flags);
		//    }
		//}

		#endregion

		#region Misc. Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Perform a search anywhere when the user clicks on a phone or it's bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleMouseDoubleClick(object sender, MouseEventArgs e)
		{
			var bar = sender as HistogramBar;
			var lbl = sender as Label;
			var srchPhone = (bar != null ? ((Label)bar.Tag).Text : lbl.Text);

			var query = new SearchQuery();
			query.Pattern = srchPhone + "/*_*";
			query.IgnoreDiacritics = false;
			App.MsgMediator.SendMessage("ViewSearch", query);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display a phone's information popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseEnter(object sender, EventArgs e)
		{
			var bar = sender as HistogramBar;
			var lbl = sender as Label;
			var useLabelForInfoPopup = (bar == null);
			
			if (bar == null)
			{
				if (lbl != null)
					bar = lbl.Tag as HistogramBar;

				// Uncomment if the magnified tooltip of a histogram's phone is desired.
				//m_phoneToolTip.Show(lbl.Text, this);
			}

			if (bar != null && _phoneInfoPopup.Initialize(bar.Phone))
				_phoneInfoPopup.Show(useLabelForInfoPopup ? lbl : bar, bar);
		}

		#endregion

		#region Scrolling/Resizing events
		/// ------------------------------------------------------------------------------------
		public void RefreshLayout()
		{
			pnlFixedBorder_Resize(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Since the panel the bars are on is not owned by the scrolling panel, make sure the
		/// left edge of the panel that owns the bars is syncronized with the phone panel as
		/// it is scrolled.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlScroller_Scroll(object sender, ScrollEventArgs e)
		{
			pnlBars.Left = -e.NewValue;
			pnlBars.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public void ForceLayout()
		{
			// This panel is docked filled so increasing its width will not change its
			// width, but it will force the control to layout again, which is what we
			// need. Kludgy, I know, but calling the PerformLayout() method does not
			// force the control to be layed out.
			pnlFixedBorder.Width++;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the height of each bar based on the tipHeight of their owning panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlFixedBorder_Resize(object sender, EventArgs e)
		{
			if (_maxTotalCount == 0 || _ignoreFixedBorderResize || App.DesignMode)
				return;

			// Make sure the labels have enough vertical space. This will increase
			// room for the labels when the scroll bar appears underneath them and
			// decrease room when the scroll bar disappears. In other words, this
			// calculation will make sure there's only enough vertical space for
			// the phone labels as is necessary so they won't get clipped.
			if (_phoneHeight != pnlScroller.ClientSize.Height)
			{
				_ignoreFixedBorderResize = true;
				pnlScroller.Height += (_phoneHeight - pnlScroller.ClientSize.Height);
				_ignoreFixedBorderResize = false;
			}

			Utils.SetWindowRedraw(pnlBars, false, false);

			int xLocationOffset = 0;
			_hashMarkIncrement = 0;
			decimal pixelsPerUnit = 0;

			int numberHashMarks = (int)Math.Round(
				decimal.Divide(pnlBars.ClientSize.Height - kPixelsFromTop, _hashMarkGap));

			if (numberHashMarks > 0)
			{
				_hashMarkIncrement = (int)Math.Ceiling(decimal.Divide(_maxTotalCount, numberHashMarks));
				pixelsPerUnit = decimal.Divide(_hashMarkGap, _hashMarkIncrement);
			}

			// Reposition and resize bars
			foreach (HistogramBar bar in pnlBars.Controls)
			{
				//int barHeight = (int)((m_maxTotalCount - bar.BarValue) * m_barHeightFactor);
				int barHeight = (int)Math.Floor(pixelsPerUnit * bar.BarValue);

				// "5" is the column spacing on either side of a bar
				Point newLoc = new Point((xLocationOffset + 5), pnlBars.Bottom - barHeight);
				Size newSize = new Size(_barWidth, barHeight);
				xLocationOffset += _phoneLabelWidth; // pixels between the characters

				if (newSize != bar.Size)
					bar.Size = newSize;

				if (newLoc != bar.Location)
					bar.Location = newLoc;
			}

			Utils.SetWindowRedraw(pnlBars, true, true);

			pnlScroller_Scroll(null, new ScrollEventArgs(ScrollEventType.EndScroll,
				-pnlScroller.AutoScrollPosition.X));
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint method for phone labels.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void lbl_Paint(object sender, PaintEventArgs e)
		{
			var lbl = sender as Label;

			if (lbl == null)
				return;

			const TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine |
				TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;

			// Draw the label's text.
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
			// and therefore, it's not referencing lbl.Font so it will need to be
			// disposed after drawing the phone.
			if (fnt == null)
			{
				TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font,
					lbl.ClientRectangle, SystemColors.ControlText, flags);
			}
			else
			{
				TextRenderer.DrawText(e.Graphics, lbl.Text, fnt,
					lbl.ClientRectangle, SystemColors.ControlText, flags);

				fnt.Dispose();
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

				int yLocationOffset = pnlBars.ClientSize.Height - _hashMarkGap;

				while (yLocationOffset > 0)
				{
					// Define line's end points.
					Point pt1 = new Point(0, yLocationOffset);
					Point pt2 = new Point(pnlBars.Width, yLocationOffset);

					e.Graphics.DrawLine(pen, pt1, pt2);
					yLocationOffset -= _hashMarkGap;

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
			if (App.DesignMode)
				return;

			decimal horzLineValue = 0;

			// Calculate (relative to pnlYaxis) where the bottom of the bar's panel is.
			Point pt = pnlBars.PointToScreen(new Point(0, pnlBars.ClientSize.Height));
			pt = pnlYaxis.PointToClient(pt);
			int yLocationOffset = pt.Y - (int)(_hashMarkGap * 1.5);

			// Calculate (relative to pnlYaxis) where we should stop drawing numbers.
			pt = pnlBars.PointToScreen(new Point(0, 0));
			pt = pnlYaxis.PointToClient(pt);
			int minY = pt.Y;

			Rectangle rc =
				new Rectangle(0, yLocationOffset, pnlYaxis.ClientSize.Width - 4, _hashMarkGap);

			using (StringFormat sf = Utils.GetStringFormat(true))
			{
				sf.Alignment = StringAlignment.Far;

				while ((rc.Top + (rc.Height / 2)) >= minY)
				{
					horzLineValue += _hashMarkIncrement;
					e.Graphics.DrawString(horzLineValue.ToString(), FontHelper.UIFont,
						SystemBrushes.ControlText, rc, sf);

					rc.Y -= _hashMarkGap;
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
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}

	#region HistogramBar Class
	/// ----------------------------------------------------------------------------------------
	public class HistogramBar : Label
	{
		private readonly Color _clrRight = ColorHelper.CalculateColor(Color.White,
			SystemColors.GradientActiveCaption, 110);

		private readonly Color _clrLeft = ColorHelper.CalculateColor(Color.White,
			SystemColors.ActiveCaption, 70);

		public string Phone { get; private set; }
		
		/// ------------------------------------------------------------------------------------
		public HistogramBar(string phone)
		{
			Size = new Size(30, 22);
			Phone = phone;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			// Fill in the bar.
			using (var br = new LinearGradientBrush(ClientRectangle, _clrLeft, _clrRight, 120))
				e.Graphics.FillRectangle(br, ClientRectangle);

			// Draw a border along the left, top and right sides of the bar.
			e.Graphics.DrawLines(SystemPens.ActiveCaption, new[] {new Point(0, Height),
				new Point(0, 0), new Point(Width - 1, 0), new Point(Width - 1, Height)});
		}

		/// -----------------------------------------------------------------------------------
		public int BarValue
		{
			get
			{
				var phoneInfo = App.Project.PhoneCache[Phone];
				return (phoneInfo == null ? 0 :
					phoneInfo.TotalCount + phoneInfo.CountAsPrimaryUncertainty +
					phoneInfo.CountAsNonPrimaryUncertainty);
			}
		}
	}

	#endregion
}
