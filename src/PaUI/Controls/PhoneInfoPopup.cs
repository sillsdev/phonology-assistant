using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Subclasses the popup class specifically for a popup displaying a phone's information
	/// for Consonant and Vowel charts and their histograms.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneInfoPopup : SilPopup
	{
		private bool m_drawLeftArrow = true;
		private bool m_drawArrow = true;
		private bool m_showRelativeToScreen = false;
		private Point m_popupLocation;
		private Control m_ctrl;
		private DataGridViewCell m_associatedCell;
		private readonly DataGridView m_associatedGrid;
		private readonly PhoneInfoPopupContent m_content;
		private readonly Timer m_popupTimer;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopup()
		{
			base.DoubleBuffered = true;
			m_content = new PhoneInfoPopupContent(this);
			Controls.Add(m_content);

			m_popupTimer = new Timer();
			m_popupTimer.Interval = 700;
			m_popupTimer.Tick += m_popupTimer_Tick;
			m_popupTimer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopup(DataGridView associatedGrid) : this()
		{
			m_associatedGrid = associatedGrid;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="associatedCell">Cell associated with the popup.</param>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(DataGridViewCell associatedCell)
		{
			if (associatedCell == null)
				return false;

			CharGridCell cgc = associatedCell.Value as CharGridCell;
			if (cgc != null)
			{
				Initialize(cgc);
				m_associatedCell = associatedCell;
			}
			
			return (cgc != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(CharGridCell cgc)
		{
			m_content.Initialize(cgc);
			Size = m_content.Size;
			m_drawLeftArrow = true;
			m_drawArrow = true;
			m_showRelativeToScreen = false;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal bool DrawArrow
		{
			get { return m_drawArrow; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal bool DrawLeftArrow
		{
			get { return m_drawLeftArrow; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal DataGridViewCell AssociatedCell
		{
			get { return m_associatedCell; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal DataGridView AssociatedGrid
		{
			get { return m_associatedGrid; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
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
			if (m_mouseOver || ((m_associatedCell == null || m_associatedGrid == null) &&
				m_ctrl == null))
			{
				return true;
			}

			Rectangle rc;
			Point pt;

			try
			{
				if (m_associatedGrid == null)
				{
					rc = m_ctrl.ClientRectangle;
					pt = m_ctrl.PointToClient(MousePosition);
				}
				else
				{
					// Get the rectangle for the associated cell.
					rc = m_associatedGrid.GetCellDisplayRectangle(
						m_associatedCell.ColumnIndex, m_associatedCell.RowIndex, false);

					pt = m_associatedGrid.PointToClient(MousePosition);
				}
			}
			catch
			{
				return false;
			}

			return rc.Contains(pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShouldShowPopup(Control ctrl)
		{
			if (ctrl == null)
			{
				Form frm = ctrl.FindForm();
				if (frm != null && !frm.ContainsFocus)
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup with the specified owning control for the specified histogram bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Show(Control ctrl, HistogramBar bar)
		{
			if (!Enabled)
				return;

			Debug.Assert(ctrl != null);
			Debug.Assert(bar != null);

			if (!ShouldShowPopup(ctrl))
				return;

			m_popupTimer.Start();

			// The desired bottom of the popup is 60 pixels down from the top of the bar.
			// If that's below the bar's bottom, then set the bottom of the popup to 5
			// pixels above the bar's bottom edge.
			int popupTop = (bar.Height > 65 ? 60 : bar.Height - 5) - Height;

			// The desired left edge for the popup is on the right side of the bar,
			// overlapping 25% of its right side. If that causes the popup to extend
			// beyond the right edge of the screen, then move the popup so its shown
			// on the left side of the bar, overlapping 25% of it's left side.
			int popupLeft = bar.Width - (bar.Width / 4);

			Point ptPopup = bar.PointToScreen(new Point(popupLeft, popupTop));

			bool tooWide;
			bool tooTall;
			CheckDesiredPopupLocation(ptPopup, out tooWide, out tooTall);

			if (tooWide)
				ptPopup = bar.PointToScreen(new Point((bar.Width / 4) - Width, popupTop));

			m_showRelativeToScreen = true;
			m_ctrl = ctrl;
			m_popupLocation = ptPopup;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup for a grid cell having the specified display rectangle relative to
		/// the specified grid specified owning control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Show(Rectangle rcCell)
		{
			if (!Enabled)
				return;

			Debug.Assert(m_associatedGrid != null);

			if (!ShouldShowPopup(m_associatedGrid))
				return;

			m_popupTimer.Start();
			
			m_drawLeftArrow = true;

			// Get the desired point, relative to the screen, where to show the popup.
			// The desired location is to the right of the associated cell.
			Point ptCell = m_associatedGrid.PointToScreen(rcCell.Location);
			Point ptPopup = new Point(ptCell.X + rcCell.Width, ptCell.Y);

			bool tooWide;
			bool tooTall;
			CheckDesiredPopupLocation(ptPopup, out tooWide, out tooTall);

			// Determine the popup's display rectangle based on it's desired location and size.
			Rectangle rcPopup = new Rectangle(ptPopup, Size);

			// If the popup is too wide to be shown at the desired location then adjust
			// its X location to show it to the left of the cell.
			if (tooWide)
			{
				ptPopup.X = ptCell.X - rcPopup.Width;
				m_drawLeftArrow = false;
			}

			// If the popup is too tall to be shown at the desired location, don't draw an
			// arrow and don't make any coordinate adjustments since .Net will make the
			// adjustment for us, automatically.
			m_drawArrow = !tooTall;

			m_ctrl = m_associatedGrid;
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
			if (Enabled)
			{
				if (IsMouseOverCellOrPopup())
					base.Show(m_showRelativeToScreen ? null : m_ctrl, m_popupLocation);

				m_popupTimer.Stop();
			}
		}
	}
}
