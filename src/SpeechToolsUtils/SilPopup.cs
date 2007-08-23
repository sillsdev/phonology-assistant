using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SIL.SpeechTools.Utils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SilPopup : UserControl
	{
		public static Color kBodyDarkColor = Color.FromArgb(110, Color.Wheat);
		public static Color kBodyLightColor = Color.White;
		public static Color kHeadDarkColor = Color.FromArgb(170, Color.BurlyWood);
		public static Color kHeadLightColor = Color.Wheat;

		public event EventHandler MouseEntered;
		public event EventHandler MouseLeft;

		protected bool m_mouseOver = false;
		protected bool m_monitorMouseOver = true;
		protected Timer m_timer;
		protected ToolStripControlHost m_host;
		protected ToolStripDropDown m_owningDropDown;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilPopup()
		{
			InitializeComponent();
			base.DoubleBuffered = true;

			if (DesignMode)
				return;

			base.Dock = DockStyle.Fill;
			
			m_host = new ToolStripControlHost(this);
			m_host.Padding = Padding.Empty;
			m_host.Margin = Padding.Empty;
			m_host.AutoSize = false;
			m_host.Size = Size;
			m_host.Dock = DockStyle.Fill;

			m_owningDropDown = new ToolStripDropDown();
			m_owningDropDown.Padding = Padding.Empty;
			m_owningDropDown.AutoSize = false;
			m_owningDropDown.LayoutStyle = ToolStripLayoutStyle.Table;
			m_owningDropDown.Size = Size;
			m_owningDropDown.Items.Add(m_host);
			m_owningDropDown.VisibleChanged += m_owningDropDown_VisibleChanged;

			Disposed += SilPopup_Disposed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void SilPopup_Disposed(object sender, EventArgs e)
		{
			Disposed -= SilPopup_Disposed;

			if (m_timer != null)
			{
				m_timer.Stop();
				m_timer.Dispose();
				m_timer = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Start and stop the timer when the owning drop-down's visibility changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_owningDropDown_VisibleChanged(object sender, EventArgs e)
		{
			if (m_owningDropDown.Visible && m_monitorMouseOver)
				InitializeTimer();
			else if (!m_owningDropDown.Visible && m_timer != null)
				m_timer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			if (m_owningDropDown != null)
				m_owningDropDown.Size = Size;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup at the specified screen location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void Show(Point screenLocation)
		{
			m_owningDropDown.Show(screenLocation);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup at the specified location (which is relative to ctrl) with the
		/// specified owning control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void Show(Control ctrl, Point location)
		{
			if (ctrl == null)
				m_owningDropDown.Show(location);
			else
				m_owningDropDown.Show(ctrl, location);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hides the popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new virtual void Hide()
		{
			m_owningDropDown.Hide();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the mouse is over the panel or any
		/// controls in its control collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsMouseOver
		{
			get {return m_mouseOver && m_owningDropDown.Visible;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the owning drop-down control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ToolStripDropDown OwningDropDown
		{
			get { return m_owningDropDown; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the panel should keep track of
		/// when the mouse is over it or any controls contained therein.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MonitorMouseOver
		{
			get { return m_monitorMouseOver; }
			set { m_monitorMouseOver = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeTimer()
		{
			if (m_timer == null)
			{
				m_timer = new Timer();
				m_timer.Tick += m_timer_Tick;
			}

			m_timer.Interval = 1;
			m_timer.Start();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_timer_Tick(object sender, EventArgs e)
		{
			OnTimerTick();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fires when the timer Tick event occurs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnTimerTick()
		{
			bool prevMouseOverValue = m_mouseOver;
			Point pt = PointToClient(MousePosition);
			m_mouseOver = ClientRectangle.Contains(pt);

			if (!m_mouseOver && prevMouseOverValue)
				OnMouseLeft(EventArgs.Empty);
			else if (m_mouseOver && !prevMouseOverValue)
				OnMouseEntered(EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fires the mouse entered event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnMouseEntered(EventArgs e)
		{
			if (MouseEntered != null)
				MouseEntered(this, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fires the MouseLeft event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnMouseLeft(EventArgs e)
		{
			if (MouseLeft != null)
				MouseLeft(this, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PaintBodyBackground(Graphics g)
		{
			using (SolidBrush brWhite = new SolidBrush(kBodyLightColor))
			using (LinearGradientBrush br = new LinearGradientBrush(ClientRectangle,
				kBodyDarkColor,	kBodyLightColor, 0f))
			{
				g.FillRectangle(brWhite, ClientRectangle);
				g.FillRectangle(br, ClientRectangle);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PaintHeadingBackground(Graphics g, Rectangle rcHead)
		{
			using (LinearGradientBrush br = new LinearGradientBrush(rcHead,
				kHeadDarkColor,	kHeadLightColor, 0.0))
			{
				g.FillRectangle(br, rcHead);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws an arrow whose tip is at the specified Y location (or centered in the
		/// heading) and points to the left.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PaintArrow(Graphics g, int dyArrowTip, Rectangle rcHead)
		{
			PaintArrow(g, dyArrowTip, rcHead, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws an arrow whose tip is at the specified Y location (or centered in the
		/// heading).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PaintArrow(Graphics g, int dyArrowTip, Rectangle rcHead, bool drawLeftArrow)
		{
			// Take the lesser value of the row's midpoint and the heading's midpoint.
			dyArrowTip = Math.Min(dyArrowTip, rcHead.Height / 2);

			// Assume the arrow should be drawn on the left side, pointing left.
			Point pt1 = new Point(1, rcHead.Y + dyArrowTip);
			Point pt2 = new Point(6, pt1.Y - 6);
			Point pt3 = new Point(6, pt1.Y + 6);

			if (!drawLeftArrow)
			{
				// Recalculate the points to draw an arrow on right side, pointing right. 
				pt1.X = rcHead.Right - 3;
				pt2.X = rcHead.Right - 9;
				pt3.X = pt2.X;
			}

			// Draw an arrow pointing to the left and is against the left edge of the popup.
			using (SolidBrush br = new SolidBrush(Color.Black))
			{
				g.FillPolygon(br, new Point[] { pt1, pt2, pt3 });
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the popup's right or bottom edge will extend beyond the
		/// bounds of the screen if shown at the specified location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CheckDesiredPopupLocation(Point ptDesired, out bool tooWide, out bool tooTall)
		{
			// Determine the popup's display rectangle based on it's desired location and size.
			Rectangle rcPopup = new Rectangle(ptDesired, Size);

			// Get the screen on which the popup will be shown. Ususally,
			// this will be the primary and only screen, since most users will
			// probably have a single monitor setup.
			Screen scrn = Screen.FromPoint(ptDesired);

			// Check if the popup will extend beyond the screen's right edge.
			tooWide = (rcPopup.Right > scrn.WorkingArea.Right);

			// Check if the popup will extend below the screen's bottom edge.
			tooTall = (rcPopup.Bottom > scrn.WorkingArea.Bottom);
		}
	}
}
