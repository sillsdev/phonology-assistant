using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
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

			if (!App.DesignMode)
				App.AddMediatorColleague(this);
		}

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
		/// Gets a value indicating whether or not the control is currently in design mode.
		/// I have had some problems with the base class' DesignMode property being true
		/// when in design mode. I'm not sure why, but adding a couple more checks fixes the
		/// problem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected new bool DesignMode
		{
			get
			{
				return (base.DesignMode || GetService(typeof(IDesignerHost)) != null) ||
					(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			}
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

			if (!DesignMode)
			{
				m_tooltip.SetToolTip(m_btnLeft, App.L10NMngr.LocalizeString(
					"ViewTabsScrollLeftToolTipText", "Scroll Left", App.kLocalizationGroupMisc));

				m_tooltip.SetToolTip(m_btnRight, App.L10NMngr.LocalizeString(
					"ViewTabsScrollRightToolTipText", "Scroll Right", App.kLocalizationGroupMisc));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			App.RemoveMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_btnHelp_Click(object sender, EventArgs e)
		{
			if (m_currTab != null)
				App.ShowHelpTopic(m_currTab.HelpTopicId);
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

			App.CurrentView = view;
			App.CurrentViewType = view.GetType();

			Control ctrl = view as Control;
			if (activateViewsForm && ctrl != null && ctrl.FindForm() != null)
			{
				if (ctrl.FindForm().WindowState == FormWindowState.Minimized)
					ctrl.FindForm().WindowState = FormWindowState.Normal;
				
				ctrl.FindForm().Activate();
			}

			App.MsgMediator.SendMessage("ActiveViewChanged", view);
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
			while (++i < m_pnlTabs.Controls.Count && !m_pnlTabs.Controls[i].Visible) { }

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
			while (--i >= 0 && !m_pnlTabs.Controls[i].Visible) { }

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
				rc, m_pnlCaption.ForeColor, kFlags);
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
}
