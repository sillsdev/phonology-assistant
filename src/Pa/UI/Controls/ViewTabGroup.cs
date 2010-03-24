using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.FieldWorks.Common.UIAdapters;
using SilUtils;
using SilUtils.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ViewTabGroup : Panel, IxCoreColleague
	{
		private const TextFormatFlags kTxtFmtFlags = TextFormatFlags.VerticalCenter |
			TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

		private bool m_isCurrentTabControl;
		private List<ViewTab> m_tabs;
		private ViewTab m_currTab;
		private SilGradientPanel m_pnlCaption;
		private Panel m_pnlHdrBand;
		private Panel m_pnlTabs;
		private Panel m_pnlUndock;
		private Panel m_pnlScroll;
		private XButton m_btnLeft;
		private XButton m_btnRight;
		private readonly ToolTip m_tooltip;
		private XButton m_btnHelp;
		private string m_captionText;

		internal static Font s_tabFont;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The panel m_pnlHdrBand owns both the m_pnlTabs and the m_pnlUndock panels.
		/// m_pnlUndock contains the close buttons and the arrow buttons that allow the user
		/// to scroll all the tabs left and right. m_pnlTabs contains all the tabs and is the
		/// panel that moves left and right (i.e. scrolls) when the number of tabs in the
		/// group exceeds the available space in which to display them all.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTabGroup()
		{
			Visible = true;
			base.DoubleBuffered = true;
			base.AllowDrop = true;
			base.BackColor = SystemColors.Control;
			s_tabFont = FontHelper.MakeFont(SystemInformation.MenuFont, 9);
			m_tooltip = new ToolTip();

			SetupOuterTabCollectionContainer();
			SetupCaptionPanel();
			SetupInnerTabCollectionContainer();
			SetupUndockingControls();
			SetupScrollPanel();

			m_tabs = new List<ViewTab>();

			if (!PaApp.DesignMode)
				PaApp.AddMediatorColleague(this);
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
				if (m_tabs != null)
				{
					for (int i = m_tabs.Count - 1; i >= 0; i--)
					{
						if (m_tabs[i] != null && !m_tabs[i].IsDisposed)
							m_tabs[i].Dispose();
					}
				}

				if (m_pnlCaption != null && !m_pnlCaption.IsDisposed)
				{
					m_pnlCaption.Paint -= m_pnlCaption_Paint;
					m_pnlCaption.Dispose();
				}

				if (s_tabFont != null)
				{
					s_tabFont.Dispose();
					s_tabFont = null;
				}
			}
			
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the panel that holds the tab collection's panel and in which the tab
		/// collection's panel slides back and forth when it's not wide enough to see all
		/// the tabs at once.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupOuterTabCollectionContainer()
		{
			m_pnlHdrBand = new Panel();
			m_pnlHdrBand.Dock = DockStyle.Top;
			m_pnlHdrBand.Padding = new Padding(0, 0, 0, 5);
			m_pnlHdrBand.Paint += m_pnlHdrBand_Paint;
			m_pnlHdrBand.Resize += m_pnlHdrBand_Resize;
			using (Graphics g = CreateGraphics())
			{
				m_pnlHdrBand.Height = TextRenderer.MeasureText(g, "X",
					s_tabFont, Size.Empty, kTxtFmtFlags).Height + 24;
			}

			Controls.Add(m_pnlHdrBand);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the panel to which the tabs are directly added.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupInnerTabCollectionContainer()
		{
			// Create the panel that holds all the tabs. 
			m_pnlTabs = new Panel();
			m_pnlTabs.Visible = true;
			m_pnlTabs.Paint += HandleLinePaint;
			m_pnlTabs.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			m_pnlTabs.Padding = new Padding(3, 3, 0, 0);
			m_pnlTabs.Location = new Point(0, 0);
			m_pnlTabs.Height = m_pnlHdrBand.Height - 5;
			m_pnlTabs.Width = 0;
			m_pnlHdrBand.Controls.Add(m_pnlTabs);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the panel that is visually the top-most panel and contains a caption.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupCaptionPanel()
		{
			m_pnlCaption = new SilGradientPanel();
			m_pnlCaption.Height = 28;
			m_pnlCaption.Dock = DockStyle.Top;
			m_pnlCaption.MakeDark = true;
			m_pnlCaption.Paint += m_pnlCaption_Paint;
			m_pnlCaption.Font = FontHelper.MakeFont(SystemInformation.MenuFont, 11,	FontStyle.Bold);
			Controls.Add(m_pnlCaption);

			m_btnHelp = new XButton();
			m_btnHelp.Size = new Size(20, 20);
			m_btnHelp.Anchor = AnchorStyles.Right;
			m_btnHelp.Image = Properties.Resources.kimidHelp;
			int gap = (m_pnlCaption.Height - m_btnHelp.Height) / 2;
			m_btnHelp.Location = new Point(m_pnlCaption.Width - (gap * 2) - m_btnHelp.Width, gap);
			m_btnHelp.Click += m_btnHelp_Click;
			m_pnlCaption.Controls.Add(m_btnHelp);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the undocking button and the panel that owns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupUndockingControls()
		{
			// Create the panel that will hold the undocking button
			m_pnlUndock = new Panel();
			
			// NOTE: Remove the following line and uncomment the rest of the code in this
			// method to display the undock view button to the right of the tabs.
			m_pnlUndock.Width = 5;
			//m_pnlUndock.Width = 27;
			m_pnlUndock.Visible = true;
			m_pnlUndock.Dock = DockStyle.Right;
			m_pnlUndock.Paint += HandleLinePaint;
			m_pnlHdrBand.Controls.Add(m_pnlUndock);
			m_pnlUndock.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupScrollPanel()
		{
			// Create the panel that will hold the close button
			m_pnlScroll = new Panel();
			m_pnlScroll.Width = 40;
			m_pnlScroll.Visible = true;
			m_pnlScroll.Dock = DockStyle.Right;
			m_pnlScroll.Paint += HandleLinePaint;
			m_pnlHdrBand.Controls.Add(m_pnlScroll);
			m_pnlScroll.Visible = false;
			m_pnlScroll.BringToFront();

			int top = ((m_pnlHdrBand.Height - m_pnlHdrBand.Padding.Bottom - 18) / 2);

			// Create a left scrolling button.
			m_btnLeft = new XButton();
			m_btnLeft.DrawLeftArrowButton = true;
			m_btnLeft.Size = new Size(18, 18);
			m_btnLeft.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			m_btnLeft.Click += m_btnLeft_Click;
			m_btnLeft.Location = new Point(4, top);
			m_pnlScroll.Controls.Add(m_btnLeft);

			// Create a right scrolling button.
			m_btnRight = new XButton();
			m_btnRight.DrawRightArrowButton = true;
			m_btnRight.Size = new Size(18, 18);
			m_btnRight.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			m_btnRight.Click += m_btnRight_Click;
			m_btnRight.Location = new Point(22, top);
			m_pnlScroll.Controls.Add(m_btnRight);

			m_tooltip.SetToolTip(m_btnLeft, Properties.Resources.kstidScrollTabsLeftToolTip);
			m_tooltip.SetToolTip(m_btnRight, Properties.Resources.kstidScrollTabsRightToolTip);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			PaApp.RemoveMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_btnHelp_Click(object sender, EventArgs e)
		{
			if (m_currTab != null)
				PaApp.ShowHelpTopic(m_currTab.HelpTopicId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab whose view is that specified by viewType.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab GetTab(Type viewType)
		{
			foreach (ViewTab tab in m_tabs)
			{
				if (tab.ViewType == viewType)
					return tab;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes all the views associated with the tabs in the tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CloseAllViews()
		{
			foreach (ViewTab tab in m_tabs)
				tab.CloseView();

			AdjustTabContainerWidth(true);
		}

		#region Tab managment methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab AddTab(string text, Type viewType)
		{
			return AddTab(text, null, null, null, null, viewType);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab AddTab(string text, string tooltip, string helptootip,
			string helptopicid, Image img, Type viewType)
		{
			if (m_pnlTabs.Left > 0)
				m_pnlTabs.Left = 0; 
			
			ViewTab tab = new ViewTab(this, img, viewType);
			tab.Text = Utils.RemoveAcceleratorPrefix(text);
			tab.HelpToolTipText = helptootip;
			tab.HelpTopicId = helptopicid;
			tab.Dock = DockStyle.Left;
			tab.Click += tab_Click;

			// Get the text's width.
			using (Graphics g = CreateGraphics())
			{
				tab.Width = TextRenderer.MeasureText(g, text, tab.Font,
					Size.Empty, kTxtFmtFlags).Width;
				
				if (img != null)
					tab.Width += (img.Width + 5);
			}

			tab.Width += 6;
			m_pnlTabs.Controls.Add(tab);
			tab.BringToFront();
			m_tabs.Add(tab);
			AdjustTabContainerWidth(true);

			if (tooltip != null)
				m_tooltip.SetToolTip(tab, tooltip);

			return tab;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustTabContainerWidth(bool includeInVisibleTabs)
		{
			int totalWidth = 0;
			foreach (ViewTab tab in m_tabs)
			{
				if (tab.Visible || includeInVisibleTabs)
					totalWidth += tab.Width;
			}

			m_pnlTabs.Width = totalWidth + m_pnlTabs.Padding.Left + m_pnlTabs.Padding.Right;
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void SetActiveView(ITabView view, bool activateViewsForm)
		{
			if (view == null)
				return;

			foreach (ViewTab tab in m_tabs)
			{
				if (tab.View is ITabView)
				{
					ITabView tabsView = tab.View as ITabView;
					bool active = (tabsView == view);
					tabsView.SetViewActive(active, tab.IsViewDocked);
					tabsView.TMAdapter.AllowUpdates = active;
				}
			}

			PaApp.CurrentView = view;
			PaApp.CurrentViewType = view.GetType();

			Control ctrl = view as Control;
			if (activateViewsForm && ctrl != null && ctrl.FindForm() != null)
			{
				if (ctrl.FindForm().WindowState == FormWindowState.Minimized)
					ctrl.FindForm().WindowState = FormWindowState.Normal;
				
				ctrl.FindForm().Activate();
			}

			PaApp.MsgMediator.SendMessage("ActiveViewChanged", view);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Activates the tab whose view is the specified type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control ActivateView(Type viewType)
		{
			ViewTab tab = GetTab(viewType);
			if (tab != null)
			{
				SelectTab(tab);
				SetActiveView(tab.View as ITabView, true);
				return tab.View;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SelectTab(ViewTab newSelectedTab)
		{
			newSelectedTab.Selected = true;
			m_currTab = newSelectedTab;
			FindInfo.Grid = null;

			if (!m_currTab.IsViewDocked)
				return;

			SuspendLayout();
			foreach (ViewTab tab in m_tabs)
			{
				if (tab != newSelectedTab && tab.Selected)
					tab.Selected = false;
			}

			EnsureTabVisible(newSelectedTab);
			ResumeLayout();
			m_pnlCaption.Invalidate();
			m_captionText = newSelectedTab.Text;
			m_tooltip.SetToolTip(m_btnHelp, newSelectedTab.HelpToolTipText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureTabVisible(ViewTab tab)
		{
			// Make sure the tab isn't wider than the available width.
			// Just leave if there's no hope of making the tab fully visible.
			int availableWidth = m_pnlHdrBand.Width - (m_pnlUndock.Width +
				(m_pnlScroll.Visible ? m_pnlScroll.Width : 0));

			if (tab.Width > availableWidth)
				return;

			int maxRight = (m_pnlScroll.Visible ? m_pnlScroll.Left : m_pnlUndock.Left);

			// Get the tab's left and right edge relative to the header panel.	
			int left = tab.Left + m_pnlTabs.Left;
			int right = left + tab.Width;

			// Check if it's already fully visible.
			if (left >= 0 && right < maxRight)
				return;

			// Slide the panel in the proper direction to make it visible.
			int dx = (left < 0 ? left : right - maxRight);
			SlideTabs(m_pnlTabs.Left - dx);
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tab_Click(object sender, EventArgs e)
		{
			ViewTab tab = sender as ViewTab;
			if (tab != null && !tab.Selected)
			{
				SelectTab(tab);
				SetActiveView(tab.View as ITabView, false);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the undock view message from the global message mediator.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUnDockView(object args)
		{
			int visibleCount = 0;

			foreach (Control ctrl in m_pnlTabs.Controls)
			{
				if (ctrl.Visible)
					visibleCount++;
			}

			// Don't undock the last tab.
			if (m_currTab != null && visibleCount > 1)
				m_currTab.IsViewDocked = false;
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void ViewWasDocked(ViewTab tab)
		{
			AdjustTabContainerWidth(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will make sure that, when a view is undocked, one of the other
		/// remaining docked views is made active.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void ViewWasUnDocked(ViewTab tab)
		{
			Application.DoEvents();

			// One of these has to succeed.
			ViewTab newTab = FindFirstVisibleTabToLeft(tab) ?? FindFirstVisibleTabToRight(tab);
			if (newTab != null)
				SelectTab(newTab);

			AdjustTabContainerWidth(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the first visible tab to the left of the specified tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab FindFirstVisibleTabToLeft(ViewTab tab)
		{
			int i = m_pnlTabs.Controls.IndexOf(tab);
			if (i == -1)
				return null;

			// Tabs are in the control collection in reverse order from how they appear
			// (i.e. Control[0] is the furthest right tab.
			while (++i < m_pnlTabs.Controls.Count && !m_pnlTabs.Controls[i].Visible);

			return (i == m_pnlTabs.Controls.Count ? null : m_pnlTabs.Controls[i] as ViewTab);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the first visible tab to the right of the specified tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab FindFirstVisibleTabToRight(ViewTab tab)
		{
			int i = m_pnlTabs.Controls.IndexOf(tab);
			if (i == -1)
				return null;

			// Tabs are in the control collection in reverse order from how they appear
			// (i.e. Control[0] is the furthest right tab.
			while (--i >= 0 && !m_pnlTabs.Controls[i].Visible);

			return (i < 0 ? null : m_pnlTabs.Controls[i] as ViewTab);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the first visible tab to the right of the
		/// specified tab is selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsRightAdjacentTabSelected(ViewTab tab)
		{
			ViewTab adjacentTab = FindFirstVisibleTabToRight(tab);
			return (adjacentTab == null ? false : adjacentTab.Selected);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when the current view tab changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewTabChanging(object args)
		{
			ViewTabGroup ctrl = args as ViewTabGroup;
			if (ctrl != null)
			{
				m_isCurrentTabControl = (ctrl == this);
				
				foreach (ViewTab tab in m_tabs)
					tab.Invalidate();
			}
			
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDockView(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || itemProps.ParentControl.FindForm() == FindForm())
				return false;

			
			if (itemProps.ParentControl.FindForm() != null)
				itemProps.ParentControl.FindForm().Close();

			return true;
		}

		#endregion

		#region Methods for managing scrolling of the tabs
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_pnlHdrBand_Resize(object sender, EventArgs e)
		{
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RefreshScrollButtonPanel()
		{
			if (m_pnlTabs == null || m_pnlHdrBand == null || m_pnlUndock == null)
				return;

			// Determine whether or not the scroll button panel should
			// be visible and set its visible state accordingly.
			bool shouldBeVisible = (m_pnlTabs.Width > m_pnlHdrBand.Width - m_pnlUndock.Width);
			if (m_pnlScroll.Visible != shouldBeVisible)
				m_pnlScroll.Visible = shouldBeVisible;

			// Determine whether or not the tabs are scrolled to either left or right
			// extreme. If so, then the appropriate scroll buttons needs to be disabled.
			m_btnLeft.Enabled = (m_pnlTabs.Left < 0);
			m_btnRight.Enabled = (m_pnlTabs.Right > m_pnlUndock.Left ||
				(shouldBeVisible && m_pnlTabs.Right > m_pnlScroll.Left));

			// If the scroll buttons are hidden and the tab panel is
			// not all visible, then move it so all the tabs are visible.
			if (!shouldBeVisible && m_pnlTabs.Left < 0)
				m_pnlTabs.Left = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scroll the tabs to the right (i.e. move the tab's panel to the right) so user is
		/// able to see tabs obscured on the left side.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnLeft_Click(object sender, EventArgs e)
		{
			int left = m_pnlTabs.Left;

			// Find the furthest right tab that is partially
			// obscurred and needs to be scrolled into view.
			foreach (ViewTab tab in m_tabs)
			{
				if (left < 0 && left + tab.Width >= 0)
				{
					SlideTabs(m_pnlTabs.Left + Math.Abs(left));
					break;
				}

				left += tab.Width;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scroll the tabs to the left (i.e. move the tab's panel to the left) so user is
		/// able to see tabs obscured on the right side.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnRight_Click(object sender, EventArgs e)
		{
			int left = m_pnlTabs.Left + m_pnlTabs.Padding.Left;

			// Find the furthest left tab that is partially
			// obscurred and needs to be scrolled into view.
			foreach (ViewTab tab in m_tabs)
			{
				if (left <= m_pnlScroll.Left && left + tab.Width > m_pnlScroll.Left)
				{
					int dx = (left + tab.Width) - m_pnlScroll.Left;
					SlideTabs(m_pnlTabs.Left - dx);
					break;
				}

				left += tab.Width;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Slides the container for all the tab controls to the specified new left value. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SlideTabs(int newLeft)
		{
			float dx = Math.Abs(m_pnlTabs.Left - newLeft);
			int pixelsPerIncrement = (int)Math.Ceiling(dx / 75f);
			bool slidingLeft = (newLeft < m_pnlTabs.Left);

			while (m_pnlTabs.Left != newLeft)
			{
				if (slidingLeft)
				{
					if (m_pnlTabs.Left - pixelsPerIncrement < newLeft)
						m_pnlTabs.Left = newLeft;
					else
						m_pnlTabs.Left -= pixelsPerIncrement;
				}
				else
				{
					if (m_pnlTabs.Left + pixelsPerIncrement > newLeft)
						m_pnlTabs.Left = newLeft;
					else
						m_pnlTabs.Left += pixelsPerIncrement;
				}

				Utils.UpdateWindow(m_pnlTabs.Handle);
			}

			RefreshScrollButtonPanel();
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the current view's text in the caption bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_pnlCaption_Paint(object sender, PaintEventArgs e)
		{
			if (string.IsNullOrEmpty(m_captionText))
				return;

			Rectangle rc = m_pnlCaption.ClientRectangle;
			rc.X += 6;
			rc.Width -= 6;

			const TextFormatFlags kFlags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.Left |
				TextFormatFlags.HidePrefix | TextFormatFlags.EndEllipsis |
				TextFormatFlags.PreserveGraphicsClipping;

			TextRenderer.DrawText(e.Graphics, m_captionText, m_pnlCaption.Font,
				rc, SystemColors.ActiveCaptionText, kFlags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_pnlHdrBand_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = m_pnlHdrBand.ClientRectangle;
			int y = rc.Bottom - 6;
			e.Graphics.DrawLine(SystemPens.ControlDark, rc.Left, y, rc.Right, y);

			using (SolidBrush br = new SolidBrush(Color.White))
				e.Graphics.FillRectangle(br, rc.Left, y + 1, rc.Right, rc.Bottom);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a line that's the continuation of the line drawn underneath all the tabs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void HandleLinePaint(object sender, PaintEventArgs e)
		{
			Panel pnl = sender as Panel;

			if (pnl == null)
				return;

			Rectangle rc = pnl.ClientRectangle;
			int y = rc.Bottom - 1;
			e.Graphics.DrawLine(SystemPens.ControlDark, rc.Left, y, rc.Right, y);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of docked view tabs in the group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int DockedTabCount
		{
			get
			{
				int count = 0;
				foreach (ViewTab tab in m_tabs)
				{
					if (tab.IsViewDocked)
						count++;
				}
				
				return count;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of undocked view tabs in the group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int UnDockedTabCount
		{
			get
			{
				int count = 0;
				foreach (ViewTab tab in m_tabs)
				{
					if (!tab.IsViewDocked)
						count++;
				}

				return count;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the view tab group's caption panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SilGradientPanel CaptionPanel
		{
			get { return m_pnlCaption; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current tab in the group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ViewTab CurrentTab
		{
			get { return m_currTab; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the tab control is the tab control with
		/// the focused child grid or record view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsCurrent
		{
			get	{return m_isCurrentTabControl;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<ViewTab> Tabs
		{
			get { return m_tabs; }
			set { m_tabs = value; }
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ViewTab : Label
	{
		private static bool s_viewSelectionInProgress;
		private bool m_ignoreTabSelection;
		private Point m_mouseDownLocation = Point.Empty;
		private bool m_mouseOver;
		private bool m_selected;
		private ViewTabGroup m_owningTabGroup;
		private readonly Image m_image;
		private Control m_viewsControl;
		private UndockedViewWnd m_viewsForm;
		private readonly Type m_viewType;
		private bool m_viewDocked;
		private bool m_undockingInProgress;
		private string m_helpToolTipText = string.Empty;
		private string m_helpTopicId;

		/// <summary>
		/// This flag gets set when a view is undocking. Suppose view A is being undocked.
		/// That means  another view in the main application window has to become active within
		/// the main window. Suppose that view is B. If view B was not previously opened
		/// (opening a view for the first time also triggers a docking of that view) then it
		/// will be and this flag will prevent the opening and docking of view B from causing
		/// its parent window (i.e. the main application window) to become activated, which
		/// is the normal behavior when a view is docked.
		/// </summary>
		private static bool s_undockingInProgress;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTab(ViewTabGroup owningTabControl, Image img, Type viewType)
		{
			base.DoubleBuffered = true;
			base.AutoSize = false;
			base.AllowDrop = true;
			base.Font = ViewTabGroup.s_tabFont;
			m_owningTabGroup = owningTabControl;
			m_viewType = viewType;
			m_image = img;

			if (PaApp.MainForm != null)
				PaApp.MainForm.Activated += MainForm_Activated;
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
				if (PaApp.MainForm != null)
					PaApp.MainForm.Activated -= MainForm_Activated;

				if (m_viewsForm != null && !m_viewsForm.IsDisposed)
				{
					m_viewsForm.FormClosing -= m_viewsForm_FormClosing;
					m_viewsForm.FormClosed -= m_viewsForm_FormClosed;
					m_viewsForm.Activated -= m_viewsForm_Activated;
					m_viewsForm.Dispose();
				}
			}
			
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control OpenView()
		{
			PaApp.StatusBarLabel.Text = string.Empty;

			// Check if the view is already loaded.
			if (m_viewsControl != null || m_viewType == null)
			{
				if (m_viewsForm != null)
					m_viewsForm.Activate();
				else if ( m_viewsControl != null)
					m_viewsControl.Visible = true;

				return m_viewsControl;
			}

			// Create an instance of the view's form
			m_viewsControl = (Control)m_viewType.Assembly.CreateInstance(m_viewType.FullName);
			PaApp.MsgMediator.SendMessage("BeginViewOpen", m_viewsControl);
			m_viewsControl.Dock = DockStyle.Fill;

			if (!(m_viewsControl is ITabView))
			{
				Utils.MsgBox(string.Format("Error: {0} is not based on ITabView!",
					m_viewType));
			}

			try
			{
				if (m_viewsControl is IxCoreColleague)
					PaApp.AddMediatorColleague(m_viewsControl as IxCoreColleague);
			}
			catch { }

			DockView();
			PaApp.MsgMediator.SendMessage("ViewOpened", m_viewsControl);
			return m_viewsControl;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the tab's view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CloseView()
		{
			if (m_undockingInProgress || m_viewsControl == null)
				return;

			PaApp.MsgMediator.SendMessage("BeginViewClosing", m_viewsControl);
			Visible = true;

			if (!m_viewsControl.IsDisposed)
			{
				if (m_owningTabGroup != null && m_owningTabGroup.Controls.Contains(m_viewsControl))
					m_owningTabGroup.Controls.Remove(m_viewsControl);

				if (m_viewsControl is IxCoreColleague)
					PaApp.RemoveMediatorColleague(m_viewsControl as IxCoreColleague);

				if (m_viewsControl is ITabView)
					((ITabView)m_viewsControl).TMAdapter.Dispose();

				m_viewsControl.Dispose();
				m_viewsControl = null;
			}

			if (m_viewsForm != null)
				m_viewsForm.Close();

			PaApp.MsgMediator.SendMessage("ViewClosed", m_viewType);
			m_viewDocked = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the view's control from its form to the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DockView()
		{
			if (m_undockingInProgress || m_viewDocked)
				return;

			PaApp.MsgMediator.SendMessage("BeginViewDocking", m_viewsControl);
			Utils.SetWindowRedraw(m_owningTabGroup, false, false);
			Visible = true;
			
			m_owningTabGroup.ViewWasDocked(this);
			m_viewsControl.Size = m_owningTabGroup.ClientSize;
			m_owningTabGroup.Controls.Add(m_viewsControl);
			m_viewsControl.PerformLayout();
			m_viewsControl.BringToFront();

			m_viewDocked = true;
			m_ignoreTabSelection = true;
			m_owningTabGroup.SelectTab(this);
			m_ignoreTabSelection = false;

			Utils.SetWindowRedraw(m_owningTabGroup, true, true);
			m_viewsControl.Focus();
			m_owningTabGroup.SetActiveView(m_viewsControl as ITabView, false);
			PaApp.MsgMediator.SendMessage("ViewDocked", m_viewsControl);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the view's control from its form to the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UnDockView()
		{
			if (s_undockingInProgress || !m_viewDocked)
				return;

			PaApp.MsgMediator.SendMessage("BeginViewUnDocking", m_viewsControl);
			m_undockingInProgress = true;
			s_undockingInProgress = true;

			if (m_owningTabGroup.Controls.Contains(m_viewsControl))
				m_owningTabGroup.Controls.Remove(m_viewsControl);

			// Prepare the undocked view's form to host the view and be displayed.
			m_viewsForm = new UndockedViewWnd(m_viewsControl);
			m_viewsForm.FormClosing += m_viewsForm_FormClosing;
			m_viewsForm.FormClosed += m_viewsForm_FormClosed;
			m_viewsForm.Activated += m_viewsForm_Activated;

			if (m_image != null)
				m_viewsForm.Icon = Icon.FromHandle(((Bitmap)m_image).GetHicon());
			
			// Strip out accelerator key prefixes but keep ampersands that should be kept.
			string caption = Utils.RemoveAcceleratorPrefix(Text);
			m_viewsForm.Text = string.Format(Properties.Resources.kstidUndockedViewCaption,
				PaApp.Project.ProjectName, caption, Application.ProductName);
			
			Visible = false;

			// Inform the tab group that one of it's views has been undocked.
			m_ignoreTabSelection = true;
			m_owningTabGroup.ViewWasUnDocked(this);
			m_ignoreTabSelection = false;
			s_undockingInProgress = false;
			m_viewDocked = false;
			m_undockingInProgress = false;

			m_viewsForm.Show();
			m_viewsForm.Activate();
			PaApp.MsgMediator.SendMessage("ViewUndocked", m_viewsControl);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void MainForm_Activated(object sender, EventArgs e)
		{
			if (m_viewDocked && m_viewsControl != null && m_viewsControl.Visible)
				m_owningTabGroup.SetActiveView(m_viewsControl as ITabView, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the global view and view type are set when an undocked view gets focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void m_viewsForm_Activated(object sender, EventArgs e)
		{
			m_owningTabGroup.SetActiveView(m_viewsControl as ITabView, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When closing because the user closed the window, we just want to treat that as
		/// docking back into the tab. So dock the control and cancel the closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_viewsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (m_undockingInProgress)
			{
				e.Cancel = true;
				return;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure to clean up after a view is closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_viewsForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			m_viewsForm.FormClosing -= m_viewsForm_FormClosing;
			m_viewsForm.FormClosed -= m_viewsForm_FormClosed;
			m_viewsForm.Activated -= m_viewsForm_Activated;
			
			if (m_viewsControl != null)
				DockView();
			
			m_viewsForm.Dispose();
			m_viewsForm = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes and reopens the view's form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshView()
		{
			CloseView();
			OpenView();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab's tool tip for displaying over the tab group's help button
		/// (i.e. the button to the far right of the view tab group's caption bar).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HelpToolTipText
		{
			get { return m_helpToolTipText; }
			set { m_helpToolTipText = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the help topic ID for the view's overview topic.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HelpTopicId
		{
			get { return m_helpTopicId; }
			set { m_helpTopicId = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's view type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Type ViewType
		{
			get { return m_viewType; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's view form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control View
		{
			get { return m_viewsControl; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the tab's view is docked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsViewDocked
		{
			get { return m_viewDocked; }
			set
			{
				if (m_viewDocked != value)
				{
					if (value)
						DockView();
					else
						UnDockView();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Selected
		{
			get { return m_selected; }
			set
			{
				if (m_selected == value || s_viewSelectionInProgress)
					return;

				s_viewSelectionInProgress = true;
				m_selected = value;
				Invalidate();
				Utils.UpdateWindow(Handle);

				// Invalidate the tab to the left of this one in
				// case it needs to redraw its etched right border.
				ViewTab adjacentTab = m_owningTabGroup.FindFirstVisibleTabToLeft(this);
				if (adjacentTab != null)
				{
					adjacentTab.Invalidate();
					Utils.UpdateWindow(adjacentTab.Handle);
				}

				if (!m_ignoreTabSelection)
				{
					if (value)
						OpenView();
					else if (m_viewsControl != null && m_viewDocked)
						m_viewsControl.Visible = false;
				}

				s_viewSelectionInProgress = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ViewTabGroup OwningTabGroup
		{
			get { return m_owningTabGroup; }
			set { m_owningTabGroup = value; }
		}

		#endregion

		#region Overridden methods and event handlers
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Make sure the current tab is selected when its grid get's focus.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void HandleResultViewEnter(object sender, EventArgs e)
		//{
		//    if (!m_selected || !m_owningTabGroup.IsCurrent)
		//        m_owningTabGroup.SelectTab(this, true);
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				m_mouseDownLocation = e.Location;
			else
			{
				Form frm = FindForm();
				if (!PaApp.IsFormActive(frm))
					frm.Focus();
			}

			base.OnMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(MouseEventArgs e)
		{
			m_mouseDownLocation = Point.Empty; 
			base.OnMouseUp(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// This will be empty when the mouse button is not down.
			if (m_mouseDownLocation.IsEmpty)
				return;
			
			// Begin draging a tab when the mouse is held down
			// and has moved 4 or more pixels in any direction.
			int dx = Math.Abs(m_mouseDownLocation.X - e.X);
			int dy = Math.Abs(m_mouseDownLocation.Y - e.Y);
			if (dx >= 4 || dy >= 4)
			{
				m_mouseDownLocation = Point.Empty;
				DoDragDrop(this, DragDropEffects.Move);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			m_mouseOver = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			m_mouseOver = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			DrawBackground(e.Graphics);
			DrawImage(e.Graphics);
			DrawText(e.Graphics);
			DrawHoverIndicator(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the tab's background.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawBackground(Graphics g)
		{
			Rectangle rc = ClientRectangle;

			// First, fill the entire background with the control color.
			g.FillRectangle(SystemBrushes.Control, rc);

			Point[] pts = new[] {new Point(0, rc.Bottom), new Point(0, rc.Top + 3),
				new Point(3, 0), new Point(rc.Right - 4, 0), new Point(rc.Right - 1, rc.Top + 3),
				new Point(rc.Right - 1, rc.Bottom)};

			if (m_selected)
			{
				using (SolidBrush br = new SolidBrush(Color.White))
					g.FillPolygon(br, pts);

				g.DrawLines(SystemPens.ControlDark, pts);
			}
			else
			{
				// Draw the etched line on the right edge to act as a separator. But
				// only draw it when the tab to the right of this one is not selected.
				if (!m_owningTabGroup.IsRightAdjacentTabSelected(this))
				{
					g.DrawLine(SystemPens.ControlDark, rc.Width - 2, 1, rc.Width - 2, rc.Height - 5);
					g.DrawLine(SystemPens.ControlLight, rc.Width - 1, 1, rc.Width - 1, rc.Height - 5);
				}

				// The tab is not selected tab, so draw a
				// line across the bottom of the tab.
				g.DrawLine(SystemPens.ControlDark, 0, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw's the tab's image.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawImage(Graphics g)
		{
			if (m_image == null)
				return;

			Rectangle rc = ClientRectangle;
			rc.X = 7;
			rc.Y = (rc.Height - m_image.Height) / 2;
			rc.Size = m_image.Size;

			if (m_selected)
				rc.Y++;

			g.DrawImage(m_image, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the tab's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawText(IDeviceContext g)
		{
			const TextFormatFlags kFlags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.HorizontalCenter | TextFormatFlags.WordEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.NoPadding |
				TextFormatFlags.HidePrefix | TextFormatFlags.PreserveGraphicsClipping;

			Color clrText = (m_selected ? Color.Black :
				ColorHelper.CalculateColor(SystemColors.ControlText,
				SystemColors.Control, 145));
			
			Rectangle rc = ClientRectangle;

			// Account for the image if there is one.
			if (m_image != null)
			{
				rc.X += (5 + m_image.Width);
				rc.Width -= (5 + m_image.Width);
			}

			// When the tab is selected, then bump the text down a couple of pixels.
			if (m_selected)
			{
				rc.Y += 2;
				rc.Height -= 2;
			}

			TextRenderer.DrawText(g, Text, Font, rc, clrText, kFlags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the mouse is over the tab, draw a line across the top to hightlight the tab
		/// the mouse is over.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawHoverIndicator(Graphics g)
		{
			if (!m_mouseOver)
				return;

			Rectangle rc = ClientRectangle;

			Color clr = (PaintingHelper.CanPaintVisualStyle() ?
				VisualStyleInformation.ControlHighlightHot : SystemColors.Highlight);

			// Draw the lines that only show when the mouse is over the tab.
			using (Pen pen = new Pen(clr))
			{
				if (m_selected)
				{
					g.DrawLine(pen, 3, 1, rc.Right - 4, 1);
					g.DrawLine(pen, 2, 2, rc.Right - 3, 2);
				}
				else
				{
					g.DrawLine(pen, 0, 0, rc.Right - 3, 0);
					g.DrawLine(pen, 0, 1, rc.Right - 3, 1);
				}
			}
		}

		#endregion
	}
}
