using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.SpeechTools.Database;
using SIL.SpeechTools.Utils;
using XCore;
using SIL.Pa.Controls;

namespace SIL.Pa
{
	public partial class HistogramWnd : Form, IxCoreColleague, ITabView
	{
		#region Declaration

		// Declare constants
		private const int kMagnifiedCharSize = 22;
		private const string kIpaCharacter = "IPA";
		private const int kLblIpaCharWidth = 40;
		private const int kLineGapSize = 25;
		private const int kPixelsFromTop = 10;

		// Declare member variables
		private int m_maxTotalCount = 0;
		private decimal m_barHeightFactor = 0;

		#endregion

		#region Constructor and Loading and closing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// DispHist constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public HistogramWnd()
		{
			InitializeComponent();
			pnlFixedBorder.Dock = DockStyle.Fill;
			pnlFixedBorder.BringToFront();

			SetUiFonts();

			pnlIPAChars.BackColor = SystemColors.Control;
			pnlBars.BorderStyle = BorderStyle.None;
			pnlYaxis.BorderStyle = BorderStyle.None;

			rbAllCons.Checked = PaApp.SettingsHandler.GetBoolWindowValue(Name, "showcons", true);
			rbAllVows.Checked = PaApp.SettingsHandler.GetBoolWindowValue(Name, "showvows", false);

			LoadPhones();
			
			// Put LoadFormProperties() last so checked radio buttons load correctly
			PaApp.SettingsHandler.LoadFormProperties(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set UI Fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetUiFonts()
		{
			Font = FontHelper.UIFont;
			lblBarValue.Font = FontHelper.UIFont;
			rbAllCons.Font = FontHelper.UIFont;
			rbAllVows.Font = FontHelper.UIFont;
		}

		#region ITabView Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control DockableContainer
		{
			get { return pnlMasterOuter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewUndocking()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewDocked()
		{
		}

		#endregion
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save settings when the form is closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveWindowValue(Name, "rbCons", rbAllCons.Checked);
			PaApp.SettingsHandler.SaveWindowValue(Name, "rbVows", rbAllVows.Checked);
			PaApp.SettingsHandler.SaveFormProperties(this);
			base.OnFormClosing(e);
		}

		#endregion

		#region Loading data from data source
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the database and create the appropriate labels and bars.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool LoadPhones()
		{
			pnlScroller.AutoScrollPosition = new Point(0, 0);
			pnlBars.Left = 0;

			// Dispose of the labels and clear the control arrays.
			pnlIPAChars.Controls.Clear();
			pnlBars.Controls.Clear();

			int xLocationOffset = 0;

			m_maxTotalCount = 0;

			IPACharacterType currCharType = (rbAllCons.Checked ?
				IPACharacterType.Consonant : IPACharacterType.Vowel);

			foreach (KeyValuePair<string, IPhoneInfo> info in PaApp.PhoneCache)
			{
				if (info.Value.CharType != currCharType)
					continue;
				
				// Create labels for IPA Characters.
				Label lblIpaChar = new Label();
				lblIpaChar.Tag = kIpaCharacter;
				lblIpaChar.Font = new Font(FontHelper.PhoneticFont.Name, 16);
				lblIpaChar.Size = new Size(40, 25);
				lblIpaChar.Text = info.Key;
				lblIpaChar.Paint += new PaintEventHandler(lbl_Paint);
				lblIpaChar.Location = new Point(xLocationOffset, 2);
				pnlIPAChars.Controls.Add(lblIpaChar);

				// Create the bars.
				HistogramBar lblHistBar = new HistogramBar();
				lblHistBar.MouseEnter += new System.EventHandler(histBar_MouseEnter);
				lblHistBar.MouseLeave += new System.EventHandler(histBar_MouseLeave);
				lblHistBar.BarValue = info.Value.TotalCount +
					info.Value.CountAsPrimaryUncertainty + info.Value.CountAsNonPrimaryUncertainty;
				pnlBars.Controls.Add(lblHistBar);

				// Determine the tallest label bar
				if (lblHistBar.BarValue > m_maxTotalCount)
					m_maxTotalCount = lblHistBar.BarValue;

				// Display the magnified Phone character.
				m_histToolTip.SetToolTip(lblIpaChar, lblIpaChar.Text);

				xLocationOffset += kLblIpaCharWidth; // pixels between the characters
			}

			pnlIPAChars.Width = (pnlIPAChars.Controls.Count * kLblIpaCharWidth);

			// Add 5 additional pixels of space between the right-most bar and border
			pnlBars.Width = (pnlIPAChars.Controls.Count * kLblIpaCharWidth) + 5;

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
		private void histToolTip_Popup(object sender, PopupEventArgs e)
		{
			// Only use custom toolTips for IpaCharacter labels
			if ((e.AssociatedControl.Tag as string) == kIpaCharacter)
			{
				using (Font fnt = new Font(FontHelper.PhoneticFont.Name, kMagnifiedCharSize))
				{
					e.ToolTipSize = TextRenderer.MeasureText(
						m_histToolTip.GetToolTip(e.AssociatedControl), fnt);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the ToolTip with the magnified font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void histToolTip_Draw(System.Object sender, DrawToolTipEventArgs e)
		{
			if ((e.AssociatedControl.Tag as string) != kIpaCharacter)
			{
				e.DrawBackground();
				e.DrawText();
				e.DrawBorder();
				return;
			}

			// Only use custom toolTips for IpaCharacter labels
			e.Graphics.FillRectangle(SystemBrushes.Info, e.Bounds);
			e.DrawBorder();

			TextFormatFlags flags = TextFormatFlags.NoPrefix |
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

			using (Font fnt = new Font(FontHelper.PhoneticFont.Name, kMagnifiedCharSize))
			{
				TextRenderer.DrawText(e.Graphics, e.ToolTipText, fnt, e.Bounds,
					SystemColors.InfoText, flags);
			}
		}

		#endregion

		#region Misc. Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the bar value label just over the bar that the mouse has entered.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void histBar_MouseEnter(object sender, System.EventArgs e)
		{
			HistogramBar bar = sender as HistogramBar;
			if (bar != null)
			{
				Point pt = pnlBars.PointToScreen(bar.Location);
				pt = pnlMasterOuter.PointToClient(pt);

				lblBarValue.Text = bar.BarValue.ToString();
				lblBarValue.Location = new Point(
					pt.X + ((bar.Width - lblBarValue.Width) / 2),
					pt.Y - lblBarValue.Height - 3);

				lblBarValue.Visible = true;
				lblBarValue.BringToFront();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hide the bar value label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void histBar_MouseLeave(object sender, System.EventArgs e)
		{
			lblBarValue.Hide();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clicked event for the radio buttons
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void rb_Clicked(object sender, EventArgs e)
		{
			// TODO: Localize message
			if (!LoadPhones())
				MessageBox.Show("Error accessing data");
		}

		#endregion

		#region Scrolling/Resizing events
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the height of each bar based on the height of their owning panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlFixedBorder_Resize(object sender, EventArgs e)
		{
			if (m_maxTotalCount == 0)
				return;

			pnlBars.SuspendLayout();

			m_barHeightFactor = 
				decimal.Divide(pnlBars.ClientSize.Height - kPixelsFromTop, m_maxTotalCount);

			int xLocationOffset = 0;
			int maxBarHeight = 0;

			// Reposition and resize bars
			foreach (HistogramBar bar in pnlBars.Controls)
			{
				maxBarHeight = 
					(int)((decimal)(m_maxTotalCount - bar.BarValue) * m_barHeightFactor);

				// "5" is the column spacing on either side of a bar
				Point newLoc = new Point((xLocationOffset + 5), (maxBarHeight + kPixelsFromTop + 1));
				Size newSize = new Size(30, (int)(bar.BarValue * m_barHeightFactor));
				xLocationOffset += kLblIpaCharWidth; // pixels between the characters

				if (newSize != bar.Size)
					bar.Size = newSize;

				if (newLoc != bar.Location)
					bar.Location = newLoc;
			}

			pnlBars.ResumeLayout();
			pnlScroller_Scroll(null, new ScrollEventArgs(ScrollEventType.EndScroll,
				-pnlScroller.AutoScrollPosition.X));

			pnlYaxis.Invalidate();
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
				e.Graphics.DrawString(lbl.Text, lbl.Font, SystemBrushes.ControlText, lbl.ClientRectangle, sf);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the horizontal bldr on the pnlBars panel.
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
					// Create points that define line.
					Point point1 = new Point(0, yLocationOffset);
					Point point2 = new Point(pnlBars.Width, yLocationOffset);

					// Draw line to panel that owns the bars.
					e.Graphics.DrawLine(pen, point1, point2);
					yLocationOffset -= kLineGapSize;

					// Draw the line on the bar panel's owner in case the bar panel
					// doesn't extend to the owner's right edge.
					point2.X = pnlFixedBorder.Width;
					g.DrawLine(pen, point1, point2);
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
			decimal horzLineValue = 0;
			string horzLineValString = string.Empty;

			// Calculate (relative to pnlYaxis) where the bottom of the bar's panel is.
			Point pt = pnlBars.PointToScreen(new Point(0, pnlBars.ClientSize.Height));
			pt = pnlYaxis.PointToClient(pt);
			int yLocationOffset = pt.Y - (int)((double)kLineGapSize * 1.5);

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
					horzLineValue +=
						Math.Round(((decimal)kLineGapSize / m_barHeightFactor), 2);

					horzLineValString = (Math.Round(horzLineValue)).ToString();

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
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] {this}; 
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
		private int m_barValue = 0;

		private Color m_clrRight = ColorHelper.CalculateColor(Color.White,
			SystemColors.GradientActiveCaption, 110);

		private Color m_clrLeft = ColorHelper.CalculateColor(Color.White,
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
			get { return m_barValue; }
			set { m_barValue = value; }
		}

		#endregion
	}

	#endregion
}