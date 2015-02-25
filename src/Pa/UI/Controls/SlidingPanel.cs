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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using SilTools.Controls;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public class SlidingPanel : Panel
	{
		private const int kContainerPadding = 5;

		private bool m_sliderOpen;
		private bool m_resizeInProcess;
		private int m_leftEdgeWhenClosed;
		private int m_leftEdgeWhenOpened;
		private int m_slidingIncrement;
		private Rectangle m_sizingRectangle;
		private SizingLine m_sizingLine;
		private SilPanel m_pnlContainer;
		private System.Windows.Forms.Timer m_tmrMouseLocationMonitor;
		private System.Windows.Forms.Timer m_tmrCloser;
		private System.Windows.Forms.Timer m_tmrOpener;
		private readonly Panel m_pnlPlaceholder;
		private readonly Control m_owningContainer;
		//private readonly string m_tabText;
		//private readonly bool m_opening;
		private readonly int m_initialPanelWidth;
		private readonly Action<int> m_saveWidthAction;

		/// ------------------------------------------------------------------------------------
		public SlidingPanel(Control owningContainer, Control control, Panel pnlPlaceHolder,
			int initialPanelWidth, Action<int> saveWidthAction)
		{
			SlideFromLeft = true;
			SuspendLayout();
			base.DoubleBuffered = true;
			base.Cursor = Cursors.Default;
			base.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;

			//m_tabText = tabText;
			m_owningContainer = owningContainer;
			HostedControl = control;
			m_owningContainer.SuspendLayout();
			HostedControl.SuspendLayout();
			HostedControl.Dock = DockStyle.Fill;
			m_pnlPlaceholder = pnlPlaceHolder;
			m_initialPanelWidth = initialPanelWidth;
			m_saveWidthAction = saveWidthAction;

			SetupPanels();
			SetupTimers();

			m_pnlPlaceholder.SizeChanged += m_pnlPlaceholder_SizeChanged;
			m_owningContainer.Controls.Add(m_pnlContainer);
			m_owningContainer.Resize += HandleOwningContainerResize;
			m_owningContainer.ParentChanged += m_owningContainer_ParentChanged;

			HostedControl.ResumeLayout(false);
			m_owningContainer.ResumeLayout(false);
			ResumeLayout(false);

			Localization.UI.LocalizeItemDlg.StringsLocalized += ResizeTab;
		}

		/// ------------------------------------------------------------------------------------
		private void SetupPanels()
		{
			Tab = new Label();
			Tab.BackColor = Color.Transparent;
			Tab.MouseLeave += m_lblTab_MouseLeave;
			Tab.MouseEnter += m_lblTab_MouseEnter;
			Tab.Paint += m_lblTab_Paint;
			Tab.Click += m_pnlTab_Click;
			Controls.Add(Tab);

			m_pnlContainer = new SilPanel();
			m_pnlContainer.Padding = new Padding(kContainerPadding);
			m_pnlContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
			m_pnlContainer.Visible = false;
			m_pnlContainer.MouseMove += m_pnlContainer_MouseMove;
			m_pnlContainer.MouseDown += m_pnlContainer_MouseDown;
			m_pnlContainer.MouseUp += m_pnlContainer_MouseUp;

			LoadSettings();
			m_pnlPlaceholder_SizeChanged(null, null);
			m_slidingIncrement = m_pnlContainer.Width / 5;

			if (SlideFromLeft)
			{
				// When on left side.
				Anchor |= AnchorStyles.Left;
				m_pnlContainer.Anchor |= AnchorStyles.Left;
				Tab.Location = new Point(0, 3);
				m_leftEdgeWhenClosed = Right - m_pnlContainer.Width;
				m_leftEdgeWhenOpened = Right;
				m_sizingRectangle = new Rectangle(m_pnlContainer.Width - kContainerPadding,
					0, kContainerPadding, m_pnlContainer.Height);
			}
			else
			{
				// When on right side.
				Anchor |= AnchorStyles.Right;
				m_pnlContainer.Anchor |= AnchorStyles.Right;
				Tab.Location = new Point(7, 3);
				m_leftEdgeWhenClosed = Left;
				m_leftEdgeWhenOpened = Left - m_pnlContainer.Width;
				m_sizingRectangle = new Rectangle(0, 0, kContainerPadding, m_pnlContainer.Height);
			}

			m_pnlContainer.Location = new Point(m_leftEdgeWhenClosed, Top);
		}

		/// ------------------------------------------------------------------------------------
		private void SetupTimers()
		{
			m_tmrMouseLocationMonitor = new System.Windows.Forms.Timer();
			m_tmrMouseLocationMonitor.Interval = 1;
			m_tmrMouseLocationMonitor.Tick += m_tmrMouseLocationMonitor_Tick;

			m_tmrCloser = new System.Windows.Forms.Timer();
			m_tmrCloser.Interval = 1000;
			m_tmrCloser.Tick += m_tmrCloser_Tick;

			m_tmrOpener = new System.Windows.Forms.Timer();
			m_tmrOpener.Interval = 400;
			m_tmrOpener.Tick += m_tmrOpener_Tick;
		}

		/// ------------------------------------------------------------------------------------
		public void LoadSettings()
		{
			if (m_initialPanelWidth > 0)
				m_pnlContainer.Width = m_initialPanelWidth;
	
			ProcessUndockedContainerSizeChanged();
		}

		/// ------------------------------------------------------------------------------------
		private void m_pnlPlaceholder_SizeChanged(object sender, EventArgs e)
		{
			ResizeTab();
			m_pnlContainer.Height = Height;
			m_pnlContainer.Location = new Point(m_leftEdgeWhenClosed, Top);
		}

		/// ------------------------------------------------------------------------------------
		private void ResizeTab()
		{
			const TextFormatFlags kFlags = TextFormatFlags.SingleLine |
				TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding |
				TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPrefix;
		    try
		    {
		        using (var g = CreateGraphics())
		        {
		            var sz = TextRenderer.MeasureText(g, Tab.Text, FontHelper.UIFont, Size.Empty, kFlags);
		            sz.Height += 15;
		            m_pnlPlaceholder.Width = sz.Height + 7;
		            Size = new Size(sz.Height + 7, m_pnlPlaceholder.Height);
		            Location = m_pnlPlaceholder.Location;
		            Tab.Size = new Size(sz.Height, sz.Width);
		        }
		    }
		    catch (Exception)
		    {
		        
		    }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the vertical label's font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFonts()
		{
			m_pnlPlaceholder_SizeChanged(null, null);
		}

		#region Properties

		/// ------------------------------------------------------------------------------------
		public Control HostedControl { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab label control that's visible when the control is undocked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Label Tab { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the slider panel should stay open
		/// even when the mouse is not over it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Freeze { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the slider panel slides out from
		/// the left side. If false, then the panel slides from the right.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SlideFromLeft { get; set; }

		#endregion

		#region Owning form events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_owningContainer_ParentChanged(object sender, EventArgs e)
		{
			m_pnlContainer.BringToFront();
			BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the location at which the container panel is considered to be opened.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleOwningContainerResize(object sender, EventArgs e)
		{
			if (SlideFromLeft)
			{
				m_leftEdgeWhenClosed = Right - m_pnlContainer.Width;
				m_leftEdgeWhenOpened = Right;
			}
			else
			{
				m_leftEdgeWhenOpened = Left - m_pnlContainer.Width;
				m_leftEdgeWhenClosed = Left;
			}
		}

		#endregion

		#region Timer and related events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turn on the timer that will show the control when tick is reached.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_lblTab_MouseEnter(object sender, EventArgs e)
		{
			if (App.IsFormActive(FindForm()) && Enabled)
			{
				if (!m_pnlContainer.Visible && !m_tmrOpener.Enabled)
					m_tmrOpener.Start();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turn off the timer that will show the control when tick is reached.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_lblTab_MouseLeave(object sender, EventArgs e)
		{
			if (!m_pnlContainer.Visible && m_tmrOpener.Enabled)
				m_tmrOpener.Stop();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the user is outside the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_tmrMouseLocationMonitor_Tick(object sender, EventArgs e)
		{
			if (!m_pnlContainer.Visible || m_resizeInProcess || Freeze)
				return;

			Point pt = MousePosition;
			Point ptInContainer = m_pnlContainer.PointToClient(pt);
			bool mouseOverContainer = m_pnlContainer.ClientRectangle.Contains(ptInContainer);

			// Make sure that the cusor is the correct cursor when it's over the
			// container panel but not over the edge where the user may resize. 
			if (mouseOverContainer && !m_sizingRectangle.Contains(ptInContainer) &&
				m_pnlContainer.Cursor != Cursors.Default)
			{
				m_pnlContainer.Cursor = Cursors.Default;
			}

			// Check if the mouse moved where we should start thinking
			// about sliding the container panel closed.
			bool mouseOverThis = ClientRectangle.Contains(PointToClient(pt));
			if (mouseOverThis || mouseOverContainer)
			{
				if (m_tmrCloser.Enabled)
					m_tmrCloser.Stop();
			}
			else if (!m_tmrCloser.Enabled)
				m_tmrCloser.Start();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Opens the container panel to its opened position.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_tmrOpener_Tick(object sender, EventArgs e)
		{
			m_tmrOpener.Stop();
			Open(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Begins sliding the container panel to its closed position.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_tmrCloser_Tick(object sender, EventArgs e)
		{
			m_tmrCloser.Stop();
			Close(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way to open the container panel immediately or allow it to slide open.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Open(bool immediate)
		{
			m_pnlContainer.Visible = true;

			if (!immediate)
			{
				if (SlideFromLeft)
				{
					int delay = 50;

					while (m_pnlContainer.Left < m_leftEdgeWhenOpened)
					{
						var newLeft = m_pnlContainer.Left + (m_pnlContainer.Width / 2);
						m_pnlContainer.Left = Math.Min(newLeft, m_leftEdgeWhenOpened);
						Thread.Sleep(delay);
						delay = Math.Max(0, delay - 5);
					}
				}
				else
				{
					while (m_pnlContainer.Left - m_slidingIncrement > m_leftEdgeWhenOpened)
					{
						m_pnlContainer.Left -= m_slidingIncrement;
						Thread.Sleep(40);
					}
				}
			}
			
			m_pnlContainer.Left = m_leftEdgeWhenOpened;
			m_tmrMouseLocationMonitor.Start();
			m_sliderOpen = true;
			Invalidate();
			Application.DoEvents();
			App.MsgMediator.SendMessage("SlidingPanelOpened", this);	
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way to close the container panel immediately or allow it to slide closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Close(bool immediate)
		{
			if (!m_pnlContainer.Visible)
				return;

			m_tmrMouseLocationMonitor.Stop();

			if (!immediate)
			{
				if (SlideFromLeft)
				{
					int delay = 50;
					while (m_pnlContainer.Left > m_leftEdgeWhenClosed)
					{
						var newLeft = m_pnlContainer.Left - (m_pnlContainer.Width / 2);
						m_pnlContainer.Left = Math.Max(newLeft, m_leftEdgeWhenClosed);
						Thread.Sleep(delay);
						delay = Math.Max(0, delay - 5);
					}
				}
				else
				{
					while (m_pnlContainer.Left + m_slidingIncrement < m_leftEdgeWhenClosed)
					{
						m_pnlContainer.Left += m_slidingIncrement;
						Thread.Sleep(40);
					}
				}
			}

			m_pnlContainer.Left = m_leftEdgeWhenClosed;
			m_pnlContainer.Visible = false;
			m_sliderOpen = false;
			Invalidate();
			App.MsgMediator.SendMessage("SlidingPanelClosed", this);
		}

		/// ------------------------------------------------------------------------------------
		void m_pnlTab_Click(object sender, EventArgs e)
		{
			if (!m_sliderOpen)
				Open(false);
		}

		#endregion

		#region Container panel resizing methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If terminating a resize process, then resize the container panel based on where
		/// the user left the sizing line.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_pnlContainer_MouseUp(object sender, MouseEventArgs e)
		{
			m_pnlContainer.Cursor = Cursors.Default;
			if (m_resizeInProcess)
			{
				m_resizeInProcess = false;
				ProcessUndockedContainerSizeChanged();
				m_owningContainer.Controls.Remove(m_sizingLine);
				m_slidingIncrement = m_pnlContainer.Width / 4;
				m_sizingLine.Dispose();

				// Save the new size in the settings file.
				m_saveWidthAction(m_pnlContainer.Width);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs misc. calculations after the undocked side panel changes its width.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ProcessUndockedContainerSizeChanged()
		{
			if (SlideFromLeft)
			{
				if (m_sizingLine != null)
					m_pnlContainer.Width = ((m_sizingLine.Left + 2) - m_pnlContainer.Left);
				
				m_leftEdgeWhenClosed = Right - m_pnlContainer.Width;
				m_sizingRectangle = new Rectangle(m_pnlContainer.Width - kContainerPadding,
					0, kContainerPadding, m_pnlContainer.Height);
			}
			else
			{
				if (m_sizingLine != null)
					m_pnlContainer.Width += (m_pnlContainer.Left - (m_sizingLine.Left - 2));
				
				m_leftEdgeWhenOpened = Left - m_pnlContainer.Width;
				m_pnlContainer.Left = m_leftEdgeWhenOpened;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the mouse goes down over the edge of the container panel, then put the user
		/// in the resize mode.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_pnlContainer_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && m_sizingRectangle.Contains(e.Location) /*&& !m_opening */)
			{
				m_owningContainer.SuspendLayout();
				m_resizeInProcess = true;
				m_sizingLine = new SizingLine(4, Height);
				m_sizingLine.Visible = true;
				Point pt = m_pnlContainer.PointToScreen(m_sizingRectangle.Location);
				pt = m_owningContainer.PointToClient(pt);
				m_sizingLine.Location = pt;
				m_owningContainer.Controls.Add(m_sizingLine);
				m_sizingLine.BringToFront();
				m_owningContainer.ResumeLayout();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_pnlContainer_MouseMove(object sender, MouseEventArgs e)
		{
			// Show the sizing cursor if the mouse is over the edge of the container panel.
			if (m_sizingRectangle.Contains(e.Location) &&
				m_pnlContainer.Cursor != Cursors.VSplit && m_sliderOpen)
			{
				m_pnlContainer.Cursor = Cursors.VSplit;
			}

			// Move the sizing line when in the process of resizing.
			if (m_resizeInProcess)
			{
				Point pt = m_pnlContainer.PointToScreen(e.Location);
				pt = m_owningContainer.PointToClient(pt);
				m_sizingLine.Left = pt.X;
			}
		}

		#endregion

		#region Docking/Undocking methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Docks the currently slider-hosted control in the specified docking target and
		/// hides the controls associated with the sliding panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DockControl(Control dockingTarget)
		{
			dockingTarget.SuspendLayout();
			m_pnlContainer.SuspendLayout();
			
			m_pnlContainer.Visible = false;
			m_pnlContainer.Controls.Remove(HostedControl);
			dockingTarget.Controls.Add(HostedControl);
			m_pnlPlaceholder.Visible = false;
			Visible = false;

			m_pnlContainer.ResumeLayout();
			dockingTarget.ResumeLayout();
			App.MsgMediator.SendMessage("SlidingPanelsControlDocked", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Undocks the hosted control from it's current control container and puts it in the
		/// sliding control host.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UnDockControl(Control dockingHost)
		{
			m_pnlContainer.Width = HostedControl.Width + (kContainerPadding * 2) + 2;
			m_slidingIncrement = m_pnlContainer.Width / 4;
			m_leftEdgeWhenOpened = (SlideFromLeft ? Right : Left - m_pnlContainer.Width);
			m_pnlContainer.Left = m_leftEdgeWhenOpened;

			if (SlideFromLeft)
			{
				m_sizingRectangle = new Rectangle(m_pnlContainer.Width - kContainerPadding,
					0, kContainerPadding, m_pnlContainer.Height);
			}

			dockingHost.SuspendLayout();
			m_owningContainer.SuspendLayout();
			m_pnlContainer.SuspendLayout();

			dockingHost.Controls.Remove(HostedControl);
			m_owningContainer.Controls.Remove(m_sizingLine);
			m_pnlContainer.Controls.Add(HostedControl);
			m_pnlPlaceholder.Visible = true;
			Visible = true;
			m_sliderOpen = false;
			Invalidate();
			BringToFront();
			m_pnlContainer.BringToFront();

			m_pnlContainer.ResumeLayout();
			m_owningContainer.ResumeLayout();
			dockingHost.ResumeLayout();
			App.MsgMediator.SendMessage("SlidingPanelsControlUndocked", this);
		}

		#endregion

		#region Painting Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the line down the panel and give the panel a gradient background.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rc = ClientRectangle;

			Color clrLeft =
				ColorHelper.CalculateColor(SystemColors.Control, Color.White, 100);

			Color clrRight =
				ColorHelper.CalculateColor(SystemColors.ControlDark, Color.White, 75);

			using (LinearGradientBrush br = new LinearGradientBrush(rc, clrLeft, clrRight, 0f))
				e.Graphics.FillRectangle(br, rc);

			if (!m_sliderOpen)
			{
				if (SlideFromLeft)
					e.Graphics.DrawLine(SystemPens.ControlDark, rc.Right - 4, 0, rc.Right - 4, rc.Bottom);
				else
					e.Graphics.DrawLine(SystemPens.ControlDark, 3, 0, 3, rc.Bottom);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw on the tab panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_lblTab_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = Tab.ClientRectangle;

			Point[] pts;
			if (SlideFromLeft)
			{
				// When on left side.
				pts = new[] {new Point(0, 0), new Point(rc.Right - 3, 0),
				    new Point(rc.Right - 1, 2), new	Point(rc.Right - 1, rc.Bottom - 3),
				    new	Point(rc.Right - 3, rc.Bottom - 1), new Point(0, rc.Bottom - 1)};
			}
			else
			{
				// When on right side.
				pts = new[] {new Point(rc.Right - 1, 0), new Point(2, 0),
					new Point(0, 2), new Point(0, rc.Bottom - 3),
					new	Point(2, rc.Bottom - 1), new Point(rc.Right - 1, rc.Bottom - 1)};
			}

			e.Graphics.FillPolygon(SystemBrushes.ControlLight, pts);
			e.Graphics.DrawPolygon(SystemPens.ControlDarkDark, pts);

			// Paint over the fourth side (i.e. the one against the edge
			// of the window) that got painted when DrawPolygon was called.
			if (SlideFromLeft)
				e.Graphics.DrawLine(SystemPens.ControlLight, 0, 1, 0, rc.Bottom - 2);
			else
				e.Graphics.DrawLine(SystemPens.ControlLight, rc.Right - 1, 1, rc.Right - 1, rc.Bottom - 2);

			using (StringFormat sf = Utils.GetStringFormat(true))
			using (SolidBrush br = new SolidBrush(Enabled ?
				SystemColors.ControlText : SystemColors.GrayText))
			{
				e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
				sf.FormatFlags |= StringFormatFlags.DirectionVertical;
				sf.HotkeyPrefix = HotkeyPrefix.None;
				e.Graphics.DrawString(Tab.Text, FontHelper.UIFont, br, rc, sf);
			}
		}

		#endregion
	}
}
