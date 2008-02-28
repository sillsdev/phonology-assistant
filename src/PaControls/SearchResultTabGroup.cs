using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.FFSearchEngine;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchResultTabGroup : Panel, IxCoreColleague
	{
		private bool m_closingTabInProcess = false;
		private bool m_isCurrentTabGroup = false;
		private List<SearchResultTab> m_tabs;
		private SearchResultTab m_currTab;
		internal Panel m_pnlHdrBand;
		private Panel m_pnlScroll;
		private XButton m_btnLeft;
		private XButton m_btnRight;
		internal ToolTip m_tooltip;
		private SearchResultTab m_contextMenuTab = null;
		private SearchResultTabGroup m_contextMenuTabGroup = null;
		private readonly XButton m_btnClose;
		private readonly Panel m_pnlTabs;
		private readonly Panel m_pnlClose;
		private readonly SearchResultsViewManager m_rsltVwMngr;
		private readonly TabDropIndicator m_dropIndicator;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The panel m_pnlHdrBand owns both the m_pnlTabs and the m_pnlUndock panels.
		/// m_pnlUndock contains the close buttons and the arrow buttons that allow the user
		/// to scroll all the tabs left and right. m_pnlTabs contains all the tabs and is the
		/// panel that moves left and right (i.e. scrolls) when the number of tabs in the
		/// group exceeds the available space in which to display them all.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTabGroup(SearchResultsViewManager rsltVwMngr)
		{
			Visible = true;
			base.DoubleBuffered = true;
			base.AllowDrop = true;

			// Create the panel that holds everything that will be displayed
			// above a result view (i.e. tabs, close button and scroll buttons).
			m_pnlHdrBand = new Panel();
			m_pnlHdrBand.Dock = DockStyle.Top;
			m_pnlHdrBand.Padding = new Padding(0, 0, 0, 5);
			m_pnlHdrBand.Paint += HandlePanelPaint;
			m_pnlHdrBand.Resize += m_pnlHdrBand_Resize;
			m_pnlHdrBand.Click += HandleClick;
			m_pnlHdrBand.MouseDown += delegate { HandleClick(null, null); };
			Controls.Add(m_pnlHdrBand);

			// Create the panel that holds all the tabs. 
			m_pnlTabs = new Panel();
			m_pnlTabs.Visible = true;
			m_pnlTabs.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			m_pnlTabs.Location = new Point(0, 0);
			m_pnlTabs.Click += HandleClick;
			m_pnlHdrBand.Controls.Add(m_pnlTabs);

			AdjustPanelHeights();

			// Create the panel that will hold the close button
			m_pnlClose = new Panel();
			m_pnlClose.Width = 22;
			m_pnlClose.Visible = true;
			m_pnlClose.Dock = DockStyle.Right;
			m_pnlClose.Paint += HandleCloseScrollPanelPaint;
			m_pnlHdrBand.Controls.Add(m_pnlClose);

			// Create a button that will close a tab.
			m_btnClose = new XButton();
			m_btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			m_btnClose.Click += m_btnClose_Click;
			m_btnClose.Location = new Point(m_pnlClose.Width - m_btnClose.Width,
				(m_pnlHdrBand.Height - m_btnClose.Height) / 2 - 3);

			m_pnlClose.Controls.Add(m_btnClose);
			m_pnlClose.BringToFront();

			SetupScrollPanel();

			m_dropIndicator = new TabDropIndicator(this, m_pnlTabs.Height);

			m_tabs = new List<SearchResultTab>();
			m_rsltVwMngr = rsltVwMngr;
			PaApp.AddMediatorColleague(this);

			if (TMAdapter != null)
				TMAdapter.SetContextMenuForControl(m_pnlHdrBand, "cmnuSearchResultTabGroup");

			if (m_pnlHdrBand.ContextMenuStrip != null)
				m_pnlHdrBand.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

			SetToolTips();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (m_pnlHdrBand.ContextMenuStrip != null)
				m_pnlHdrBand.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;
			
			m_btnClose.Dispose();
			m_btnLeft.Dispose();
			m_btnRight.Dispose();
			m_pnlHdrBand.Dispose();
			m_pnlTabs.Dispose();
			m_pnlClose.Dispose();
			m_pnlScroll.Dispose();

			if (m_tooltip != null)
				m_tooltip.Dispose();

			base.Dispose(disposing);
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
			m_pnlScroll.Paint += HandleCloseScrollPanelPaint;
			m_pnlHdrBand.Controls.Add(m_pnlScroll);
			m_pnlScroll.Visible = false;
			m_pnlScroll.BringToFront();

			// Create a left scrolling button.
			m_btnLeft = new XButton();
			m_btnLeft.DrawLeftArrowButton = true;
			m_btnLeft.Size = new Size(18, 18);
			m_btnLeft.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			m_btnLeft.Click += m_btnLeft_Click;
			m_btnLeft.Location = new Point(4, (m_pnlHdrBand.Height - m_btnLeft.Height) / 2 - 3);
			m_pnlScroll.Controls.Add(m_btnLeft);

			// Create a right scrolling button.
			m_btnRight = new XButton();
			m_btnRight.DrawRightArrowButton = true;
			m_btnRight.Size = new Size(18, 18);
			m_btnRight.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			m_btnRight.Click += m_btnRight_Click;
			m_btnRight.Location = new Point(22, (m_pnlHdrBand.Height - m_btnRight.Height) / 2 - 3);
			m_pnlScroll.Controls.Add(m_btnRight);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			ContextMenuStrip cms = sender as ContextMenuStrip;
			if (cms != null && cms.SourceControl != null)
				m_contextMenuTabGroup = cms.SourceControl.Parent as SearchResultTabGroup;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustPanelHeights()
		{
			using (Graphics g = CreateGraphics())
			{
				int extraTabHeight = PaApp.SettingsHandler.GetIntSettingsValue(
					"SearchVw", "extrasearchtabheight", 12);

				TextFormatFlags flags = TextFormatFlags.VerticalCenter |
					TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

				m_pnlHdrBand.Height = TextRenderer.MeasureText(g, "X",
					FontHelper.PhoneticFont, new Size(int.MaxValue, int.MaxValue),
					flags).Height + extraTabHeight;
			}

			m_pnlTabs.Height = m_pnlHdrBand.Height - 5;

			if (m_btnClose != null)
				m_btnClose.Top = (m_pnlHdrBand.Height - m_btnClose.Height) / 2 - 3;

			if (m_btnLeft != null)
				m_btnLeft.Top = (m_pnlHdrBand.Height - m_btnLeft.Height) / 2 - 3;

			if (m_btnRight != null)
				m_btnRight.Top = (m_pnlHdrBand.Height - m_btnLeft.Height) / 2 - 3;

			m_pnlHdrBand.Invalidate(true);
		}

		#region Message mediator message handler and update handler methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetToolTips()
		{
			m_tooltip = new ToolTip();
			m_tooltip.SetToolTip(m_btnClose, Properties.Resources.kstidCloseActiveTabButtonToolTip);
			m_tooltip.SetToolTip(m_btnLeft, Properties.Resources.kstidScrollTabsLeftToolTip);
			m_tooltip.SetToolTip(m_btnRight, Properties.Resources.kstidScrollTabsRightToolTip);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewDocked(object args)
		{
			SetToolTips();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			SetToolTips();
			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the Class Display Behavior has been changed by the user.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnClassDisplayBehaviorChanged(object args)
		{
			foreach (SearchResultTab tab in Tabs)
			{
				if (tab.SearchQuery.Pattern == null)
					continue;

				// PaApp.ShowClassNames has not been set yet to the new value
				// in OptionsDialog.FindPhonesTab>>SaveFindPhonesTabSettings()

				string replacedText = PaApp.Project.SearchClasses.ModifyPatternText(tab.Text);
				if (replacedText != string.Empty)
				{
					UpdateTabsText(tab, replacedText);
					tab.SearchQuery.Pattern = replacedText;
				}
			}
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			if (m_tabs != null)
			{
				foreach (SearchResultTab tab in m_tabs)
				{
					tab.Font = FontHelper.PhoneticFont;
					tab.AdjustWidth();
				}

				AdjustTabContainerWidth();
				AdjustPanelHeights();
			}

			// Return false to allow other windows to update their fonts.
			return false;
		}

		#endregion

		#region Overridden methods
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
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display a message informing the user what to do. This always gets displayed but
		/// is only visible when the current tab is empty. Otherwise, the tab's result view
		/// covers the message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding |
				TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter |
				TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping;

			Rectangle rc = ClientRectangle;
			rc.Y = m_pnlHdrBand.Bottom;
			rc.Height -= m_pnlHdrBand.Height;
			Color clr = (m_isCurrentTabGroup ? SystemColors.ControlText : SystemColors.GrayText);

			using (Font fnt = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold))
			{
				TextRenderer.DrawText(e.Graphics,
					Properties.Resources.kstidEmtpyTabInfoText, fnt, rc, clr, flags);
			}

			PaApp.DrawWatermarkImage("kimidSearchWatermark", e.Graphics, ClientRectangle);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			HandleClick(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClick(object sender, EventArgs e)
		{
			if (m_currTab != null)
				tab_Click(m_currTab, EventArgs.Empty);
		}

		#endregion

		#region Tab managment methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an empty tab to the tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTab AddTab()
		{
			SearchResultTab tab = new SearchResultTab(this);
			tab.Text = Properties.Resources.kstidEmptySrchResultTabText;
			AddTab(tab);
			return tab;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTab AddTab(SearchResultView resultView)
		{
			if (m_pnlTabs.Left > 0)
				m_pnlTabs.Left = 0;

			SearchResultTab tab = new SearchResultTab(this);
			tab.ResultView = resultView;
			AddTab(tab);
			return tab;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an empty tab to the tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddTab(SearchResultTab tab)
		{
			tab.Dock = DockStyle.Left;
			tab.Click += tab_Click;
			tab.MouseDown += HandleMouseDown;
			InitializeTab(tab, tab.ResultView, false);
			m_pnlTabs.Controls.Add(tab);
			tab.BringToFront();
			m_tabs.Add(tab);
			AdjustTabContainerWidth();
			UseWaitCursor = false;
			Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the specified tab with the specified text and result view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeTab(SearchResultTab tab, SearchResultView resultView,
			bool removePreviousResults)
		{
			if (tab == null)
				return;

			bool viewWasInCIEView = tab.CIEOptionsButton.Visible;

			// Make sure that if tab already has a result view, it gets removed.
			if (removePreviousResults)
				tab.RemoveResultView();

			// If there is no tab text, then get it from the result view's search query.
			if (string.IsNullOrEmpty(tab.Text) && resultView != null && resultView.SearchQuery != null)
				tab.Text = resultView.SearchQuery.ToString();

			tab.AdjustWidth();
			tab.OwningTabGroup = this;

			if (resultView != null)
			{
				tab.ResultView = resultView;
				tab.ResultView.Size = new Size(Width, Height - m_pnlHdrBand.Height);
				tab.ResultView.Click += HandleClick;
				tab.ResultView.MouseDown += HandleMouseDown;
				Controls.Add(resultView);
				AdjustTabContainerWidth();
				resultView.BringToFront();

				if (viewWasInCIEView)
					tab.CIEViewRefresh();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void AdjustTabContainerWidth()
		{
			int totalWidth = 0;
			foreach (SearchResultTab tab in m_tabs)
				totalWidth += tab.Width;

			m_pnlTabs.SuspendLayout();
			m_pnlTabs.Width = totalWidth;
			RefreshScrollButtonPanel();
			m_pnlTabs.ResumeLayout(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the name assigned to the current tab's search query.
		/// </summary>
		/// <param name="newName"></param>
		/// ------------------------------------------------------------------------------------
		public void UpdateCurrentTabsQueryName(string newName)
		{
			if (CurrentTab != null && CurrentTab.SearchQuery != null)
			{
				CurrentTab.SearchQuery.Name = newName;
				UpdateTabsText(CurrentTab, newName);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the text on the specified tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateTabsText(SearchResultTab tab, string text)
		{
			if (tab != null && tab.SearchQuery != null)
			{
				tab.Text = text;
				tab.AdjustWidth();
				AdjustTabContainerWidth();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the specified tab from the group's collection of tabs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveTab(SearchResultTab tab, bool disposeOfTab)
		{
			// If the tab being removed is selected and owned by a tab group, then make
			// sure we select an adjacent tab before removing the tab because a tab group
			// always has to have a selected tab.
			if (tab.Selected && tab.OwningTabGroup != null)
			{
				SearchResultTabGroup tabGroup = tab.OwningTabGroup;
				SearchResultTab newTabInSendingGroup = null;
				int i = tabGroup.m_pnlTabs.Controls.IndexOf(tab);

				if (i - 1 >= 0)
					newTabInSendingGroup = tabGroup.m_pnlTabs.Controls[i - 1] as SearchResultTab;
				else if (i + 1 < tabGroup.m_pnlTabs.Controls.Count)
					newTabInSendingGroup = tabGroup.m_pnlTabs.Controls[i + 1] as SearchResultTab;

				if (newTabInSendingGroup != null)
					tabGroup.SelectTab(newTabInSendingGroup, true);
			}

			if (m_pnlTabs.Controls.Contains(tab))
			{
				tab.Click -= tab_Click;
				tab.MouseDown -= HandleMouseDown;

				if (tab.ResultView != null)
				{
					tab.ResultView.Click -= HandleClick;
					tab.ResultView.MouseDown -= HandleMouseDown;
				}

				if (Controls.Contains(tab.ResultView))
					Controls.Remove(tab.ResultView);

				m_pnlTabs.Controls.Remove(tab);
				m_tabs.Remove(tab);

				if (disposeOfTab)
					tab.Dispose();

				AdjustTabContainerWidth();
			}

			// If the last tab was removed from the group, then close the tab group by
			// removing ourselves from our parent's control collection.
			if (m_tabs.Count == 0 && Parent != null)
			{
				Controls.Clear();
				Parent.Controls.Remove(this);
				Dispose();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SelectTab(SearchResultTab newSelectedTab, bool makeTabCurrent)
		{
			if (newSelectedTab == null)
				return;

			if (makeTabCurrent)
			{
				m_rsltVwMngr.SearchResultTabGroupChanged(this);

				// This is used to inform other tab groups in the same view tabs manager.
				PaApp.MsgMediator.SendMessage("SearchResultTabGroupChanged", this);
			}

			newSelectedTab.Selected = true;
			m_currTab = newSelectedTab;

			foreach (SearchResultTab tab in m_tabs)
			{
				if (tab != newSelectedTab)
					tab.Selected = false;
			}

			if (makeTabCurrent)
			{
				EnsureTabVisible(m_currTab);

				// Make sure the tab's grid has focus.
				if (m_currTab.ResultView != null && m_currTab.ResultView.Grid != null)
					m_currTab.ResultView.Grid.Focus();

				m_rsltVwMngr.CurrentSearchResultTabChanged(m_currTab);

				// Sometimes selecting an empty tab causes a chain reaction caused by hiding
				// the former selected tab's grid. Doing that forces the .Net framework to
				// look for the next visible control in line that can be focused. Sometimes
				// that means a grid on another tab will get focus and ultimately cause that
				// grid's tab to become selected, thus negating the fact that we just got
				// through setting this tab group as current. Therefore, force the issue
				// again.
				if (!m_isCurrentTabGroup)
				{
					m_rsltVwMngr.SearchResultTabGroupChanged(this);
					PaApp.MsgMediator.SendMessage("SearchResultTabGroupChanged", this);
					m_rsltVwMngr.CurrentSearchResultTabChanged(m_currTab);
				}

				if (m_currTab.ResultView != null && m_currTab.ResultView.Grid != null)
					m_currTab.ResultView.Grid.IsCurrentPlaybackGrid = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureTabVisible(SearchResultTab tab)
		{
			// Make sure the tab isn't wider than the available width.
			// Just leave if there's no hope of making the tab fully visible.
			int availableWidth = m_pnlHdrBand.Width - (m_pnlClose.Width +
				(m_pnlScroll.Visible ? m_pnlScroll.Width : 0));

			if (tab.Width > availableWidth)
				return;

			int maxRight = (m_pnlScroll.Visible ? m_pnlScroll.Left : m_pnlClose.Left);

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
			SelectTab(sender as SearchResultTab, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the tab the mouse was right-clicked on so this tab group will know what tab
		/// the user clicked on when one of the tab's context menu message handlers is called.
		/// Also make sure the tab becomes the current tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (sender is SearchResultView)
					tab_Click(m_currTab, null);
				else
					tab_Click(sender, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the selected tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnClose_Click(object sender, EventArgs e)
		{
			// m_closingTabInProcess prevents reentrancy
			if (m_currTab != null && !m_closingTabInProcess)
			{
				m_closingTabInProcess = true;
				RemoveTab(m_currTab, true);
				m_closingTabInProcess = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when the current tab should be moved to a new
		/// side-by-side tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMoveToNewSideBySideTabGroup(object args)
		{
			if (m_contextMenuTab != null)
			{
				PaApp.MsgMediator.SendMessage("ReflectMoveToNewSideBySideTabGroup",
					m_contextMenuTab);

				m_contextMenuTab = null;
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when the current tab should be moved to a new
		/// stacked tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnMoveToNewStackedTabGroup(object args)
		{
			if (m_contextMenuTab != null)
			{
				PaApp.MsgMediator.SendMessage("ReflectMoveToNewStackedTabGroup",
					m_contextMenuTab);

				m_contextMenuTab = null;
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCloseTab(object args)
		{
			if (m_contextMenuTab != null)
			{
				m_btnClose_Click(null, null);
				m_contextMenuTab = null;
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCloseTabGroup(object args)
		{
			if (m_contextMenuTab != null || m_contextMenuTabGroup == this)
			{
				Close();
				m_contextMenuTab = null;
				m_contextMenuTabGroup = null;
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Close()
		{
			while (m_tabs.Count > 0)
			{
				m_contextMenuTab = m_tabs[0];
				m_btnClose_Click(null, null);
			}

			PaApp.RemoveMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't allow this menu item to be chosen when there's only one tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateMoveToNewSideBySideTabGroup(object args)
		{
			// If we're not the tab group that owns the tab that was
			// clicked on, then we don't want to handle the message.
			if (!m_tabs.Contains(m_contextMenuTab))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (m_tabs.Count > 1);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Don't allow this menu item to be chosen when there's only one tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateMoveToNewStackedTabGroup(object args)
		{
			// If we're not the tab group that owns the tab that was
			// clicked on, then we don't want to handle the message.
			if (!m_tabs.Contains(m_contextMenuTab))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (m_tabs.Count > 1);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when the current tab group changes. We catch it here
		/// for every group in order to invalidate the tabs contained in the group. They're
		/// invalidated to force painting consistent with their selected status.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSearchResultTabGroupChanged(object args)
		{
			SearchResultTabGroup group = args as SearchResultTabGroup;
			if (group != null && group.m_rsltVwMngr == m_rsltVwMngr)
			{
				m_isCurrentTabGroup = (group == this);

				foreach (SearchResultTab tab in m_tabs)
				{
					tab.Invalidate();
					if (tab.ResultView != null && tab.ResultView.Grid != null)
						tab.ResultView.Grid.IsCurrentPlaybackGrid = false;
				}

				// Force the text in empty views to be redrawn. This is only necessary
				// when there is more than one tab group and the current tab in any one
				// of those groups is empty. (The text is drawn disabled looking when
				// the tab's owning tab group isn't current).
				if (m_currTab != null && m_currTab.ResultView == null)
					Invalidate();
			}

			// There's a strange problem in which a tab group's wait cursor gets turned on
			// and I can't find where. It's not explicitly so it must be implicitly.
			UseWaitCursor = false;
			Cursor = Cursors.Default;

			return false;
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
			if (m_pnlTabs == null || m_pnlHdrBand == null || m_pnlClose == null)
				return;

			// Determine whether or not the scroll button panel should
			// be visible and set its visible state accordingly.
			bool shouldBeVisible = (m_pnlTabs.Width > m_pnlHdrBand.Width - m_pnlClose.Width);
			if (m_pnlScroll.Visible != shouldBeVisible)
				m_pnlScroll.Visible = shouldBeVisible;

			// Determine whether or not the tabs are scrolled to either left or right
			// extreme. If so, then the appropriate scroll buttons needs to be disabled.
			m_btnLeft.Enabled = (m_pnlTabs.Left < 0);
			m_btnRight.Enabled = (m_pnlTabs.Right > m_pnlClose.Left ||
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
			foreach (SearchResultTab tab in m_tabs)
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
			int left = m_pnlTabs.Left;

			// Find the furthest left tab that is partially
			// obscurred and needs to be scrolled into view.
			foreach (SearchResultTab tab in m_tabs)
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

				STUtils.UpdateWindow(m_pnlTabs.Handle);
			}

			RefreshScrollButtonPanel();
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandlePanelPaint(object sender, PaintEventArgs e)
		{
			Panel pnl = sender as Panel;

			if (pnl != null)
			{
				int y = pnl.ClientRectangle.Bottom - 6;
				e.Graphics.DrawLine(SystemPens.ControlDark, 0, y, pnl.Right, y);

				using (SolidBrush br = new SolidBrush(Color.White))
					e.Graphics.FillRectangle(br, 0, y + 1, pnl.Right, pnl.Bottom);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a line that's the continuation of the line drawn on the owner of m_pnlUndock.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleCloseScrollPanelPaint(object sender, PaintEventArgs e)
		{
			Panel pnl = sender as Panel;
			if (pnl != null)
			{
				int y = pnl.ClientRectangle.Bottom - 1;
				e.Graphics.DrawLine(SystemPens.ControlDark, 0, y, pnl.Right, y);
			}
		}

		#endregion

		#region Drag and drop methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides an accessor for a tab to call it's owning tab group's drag over event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void InternalDragOver(DragEventArgs e)
		{
			OnDragOver(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides an accessor for a tab to call it's owning tab group's drag drop event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void InternalDragDrop(DragEventArgs e)
		{
			OnDragDrop(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides an accessor for a tab to call it's owning tab group's drag drop event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void InternalDragLeave(EventArgs e)
		{
			OnDragLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			m_dropIndicator.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			SearchResultTab tab = e.Data.GetData(typeof(SearchResultTab)) as SearchResultTab;
			SearchQuery query = e.Data.GetData(typeof(SearchQuery)) as SearchQuery;

			// Check if a query from the saved pattern list or the recent pattern list is
			// be dragged. If so, that is allowed to be dropping.
			if (query != null && !query.PatternOnly)
			{
				e.Effect = e.AllowedEffect;
				m_dropIndicator.Locate(false);
				return;
			}

			// Check if a search result tab is being dragged. If so, that is allowed to be
			// dropped so long as it's not being dragged over the group it's already in.
			if (tab != null && tab.OwningTabGroup != this)
			{
				e.Effect = DragDropEffects.Move;
				m_dropIndicator.Locate(true);
				return;
			}

			e.Effect = DragDropEffects.None;
			m_dropIndicator.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragDrop(DragEventArgs e)
		{
			m_dropIndicator.Visible = false;

			base.OnDragDrop(e);
			SearchResultTab tab = e.Data.GetData(typeof(SearchResultTab)) as SearchResultTab;
			SearchQuery query = e.Data.GetData(typeof(SearchQuery)) as SearchQuery;

			// Is what was dropped appropriate to be dropped in a search pattern?
			if (tab != null && tab.OwningTabGroup != this)
			{
				// Remove the tab from it's owning group.
				tab.OwningTabGroup.RemoveTab(tab, false);
				AddTab(tab);
				tab_Click(tab, null);
			}
			else if (query != null && !query.PatternOnly)
			{
				SelectTab(m_currTab, true);
				PaApp.MsgMediator.SendMessage("PatternDroppedOnTabGroup", query);
			}
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used by the group's tabs to inform their owning group on what tab a context menu
		/// was opened.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal SearchResultTab ContextMenuTab
		{
			set { m_contextMenuTab = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current tab in the group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTab CurrentTab
		{
			get { return m_currTab; }
			set { m_currTab = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the tab control is the tab control with
		/// the focused child grid or record view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCurrent
		{
			get { return m_isCurrentTabGroup; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab group's record view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IRecordView RecordView
		{
			get { return m_rsltVwMngr.RecordView; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab group's toolbar/menu adapter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return m_rsltVwMngr.TMAdapter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<SearchResultTab> Tabs
		{
			get { return m_tabs; }
			set { m_tabs = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab group's wait cursor state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new bool UseWaitCursor
		{
			get { return base.UseWaitCursor; }
			set
			{
				base.UseWaitCursor = value;
				Cursor = (value ? Cursors.WaitCursor : Cursors.Default);

				// Cascade the setting to each of the tab group's tabs. 
				foreach (SearchResultTab tab in m_tabs)
				{
					tab.UseWaitCursor = value;
					if (tab.ResultView != null && tab.ResultView.Grid != null)
					{
						tab.ResultView.Grid.UseWaitCursor = value;
						tab.Cursor = (value ? Cursors.WaitCursor : Cursors.Default);
					}
				}
			}
		}

		#endregion

		#region Minimal pair (i.e. CIE) options drop-down handling methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void ShowCIEOptions(Control ctrl)
		{
			if (m_currTab == null || m_currTab.ResultView == null || m_currTab.ResultView.Grid == null)
				return;

			if (m_currTab.CieOptionsDropDown == null)
				m_currTab.CieOptionsDropDown = new CIEOptionsDropDown();

			if (m_currTab.CieOptionsDropDownContainer == null)
			{
				m_currTab.CieOptionsDropDownContainer = new CustomDropDown();
				m_currTab.CieOptionsDropDownContainer.AddControl(m_currTab.CieOptionsDropDown);
			}

			m_currTab.CieOptionsDropDown.CIEOptions = m_currTab.ResultView.Grid.CIEOptions;
			m_currTab.CieOptionsDropDownContainer.Closed += m_cieOptionsDropDownContainer_Closed;
			Point pt = ctrl.PointToScreen(new Point(0, ctrl.Height));
			m_currTab.CieOptionsDropDownContainer.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_cieOptionsDropDownContainer_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			// Make sure the drop-down completely goes away before proceeding.
			Application.DoEvents();

			if (m_currTab.CieOptionsDropDown.OptionsChanged)
			{
				// Save the options as the new defaults for the project.
				PaApp.Project.CIEOptions = m_currTab.CieOptionsDropDown.CIEOptions;
				PaApp.Project.Save();
				m_currTab.ResultView.Grid.CIEOptions = m_currTab.CieOptionsDropDown.CIEOptions;
				m_currTab.CIEViewRefresh();
			}

			m_currTab.CieOptionsDropDownContainer.Closed -= m_cieOptionsDropDownContainer_Closed;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

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
	public class SearchResultTab : Panel, IxCoreColleague
	{
		// The combined left and right margins of the image. 
		private const int kleftImgMargin = 6;

		private Point m_mouseDownLocation = Point.Empty;
		private bool m_mouseOver = false;
		private bool m_selected = false;
		private SearchResultTabGroup m_owningTabGroup;
		private SearchResultView m_resultView;
		private SearchQuery m_query;
		private bool m_tabTextClipped = false;
		private Image m_image;
		private XButton m_btnCIEOptions;
		private ToolTip m_CIEButtonToolTip;
		private CustomDropDown m_cieOptionsDropDownContainer;
		private CIEOptionsDropDown m_cieOptionsDropDown;
		private Color m_activeTabInactiveGroupBack1;
		private Color m_activeTabInactiveGroupBack2;
		private Color m_activeTabInactiveGroupFore;
		private Color m_activeTabFore;
		private Color m_activeTabBack;
		private Color m_inactiveTabFore;
		private Color m_inactiveTabBack;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTab(SearchResultTabGroup owningTabControl)
		{
			base.DoubleBuffered = true;
			base.AutoSize = false;
			base.AllowDrop = true;
			base.Font = FontHelper.PhoneticFont;
			m_owningTabGroup = owningTabControl;
			m_query = new SearchQuery();
			PaApp.AddMediatorColleague(this);

			if (m_owningTabGroup.TMAdapter != null)
				m_owningTabGroup.TMAdapter.SetContextMenuForControl(this, "cmnuSearchResultTab");

			if (base.ContextMenuStrip != null)
				base.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

			// Prepare the tab's minimal pair options button.
			Image img = Properties.Resources.kimidMinimalPairOptions;
			m_btnCIEOptions = new XButton();
			m_btnCIEOptions.Image = img;
			m_btnCIEOptions.Size = new Size(img.Width + 4, img.Height + 4);
			m_btnCIEOptions.BackColor = Color.Transparent;
			m_btnCIEOptions.Visible = false;
			m_btnCIEOptions.Left = kleftImgMargin;
			m_btnCIEOptions.Click += m_btnCIEOptions_Click;
			m_btnCIEOptions.MouseEnter += m_btnCIEOptions_MouseEnter;
			m_btnCIEOptions.MouseLeave += m_btnCIEOptions_MouseLeave;
			Controls.Add(m_btnCIEOptions);
			GetTabColors();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetTabColors()
		{
			m_activeTabInactiveGroupBack1 = PaApp.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activeininactivegroup1", Color.White);

			m_activeTabInactiveGroupBack2 = PaApp.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activeininactivegroup1", 0xFFD7D1C4);

			m_activeTabInactiveGroupFore = PaApp.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activeininactivegroupfore", Color.Black);

			m_activeTabBack = PaApp.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activetabback", Color.White);

			m_activeTabFore = PaApp.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "activetabfore", Color.Black);

			m_inactiveTabBack = PaApp.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "inactivetabback", SystemColors.Control);

			m_inactiveTabFore = PaApp.SettingsHandler.GetColorSettingsValue(
				"srchresulttabs", "inactivetabfore", SystemColors.ControlText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up a little.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UnsubscribeToGridEvents();

				if (ContextMenuStrip != null && !ContextMenuStrip.IsDisposed)
					ContextMenuStrip.Opening -= ContextMenuStrip_Opening;

				PaApp.RemoveMediatorColleague(this);
				
				if (!m_btnCIEOptions.IsDisposed)
					m_btnCIEOptions.Dispose();

				if (m_image != null)
				{
					m_image.Dispose();
					m_image = null;
				}

				if (m_resultView != null)
				{
					m_resultView.Dispose();
					m_resultView = null;
				}

				if (m_query != null)
					m_query = null;
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal XButton CIEOptionsButton
		{
			get { return m_btnCIEOptions; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			ContextMenuStrip cms = sender as ContextMenuStrip;

			if (cms == null)
				return;

			if (cms.SourceControl == this && Selected && m_owningTabGroup.IsCurrent ||
				cms.SourceControl == m_resultView || cms.SourceControl == Grid)
			{
				m_owningTabGroup.ContextMenuTab = this;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the tab's result view with a new result cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshResultView(WordListCache resultCache)
		{
			Text = (resultCache == null ||
				resultCache.SearchQuery == null ? string.Empty :
				resultCache.SearchQuery.ToString());

			if (resultCache != null)
			{
				UnsubscribeToGridEvents();
				m_resultView.Initialize(resultCache);
				UpdateRecordView();
				SubscribeToGridEvents();
				m_query = resultCache.SearchQuery;
			}

			if (m_btnCIEOptions.Visible)
			{
				FindInfo.CanFindAgain = false;
				m_btnCIEOptions.Visible = m_resultView.Grid.Cache.IsCIEList;
			}

			AdjustWidth();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & Set the CieOptionsDropDownContainer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CustomDropDown CieOptionsDropDownContainer
		{
			get { return m_cieOptionsDropDownContainer; }
			set { m_cieOptionsDropDownContainer = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & Set the CieOptionsDropDown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptionsDropDown CieOptionsDropDown
		{
			get { return m_cieOptionsDropDown; }
			set { m_cieOptionsDropDown = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get { return m_image; }
			set
			{
				if (m_image != value)
				{
					m_image = value;
					Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the tab contains any results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsEmpty
		{
			get { return m_resultView == null; }
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
				if (m_selected != value)
				{
					m_selected = value;
					Invalidate();
					STUtils.UpdateWindow(Handle);

					if (m_resultView != null)
					{
						if (m_resultView.Grid != null)
							m_resultView.Grid.IsCurrentPlaybackGrid = value;

						m_resultView.Visible = value;
						if (value)
						{
							m_resultView.BringToFront();
							if (m_resultView.Grid != null)
							{
								m_resultView.Grid.Focus();
								FindInfo.Grid = m_resultView.Grid;
							}
						}
					}
				}
				else if (m_owningTabGroup.IsCurrent && m_resultView != null &&
					m_resultView.Grid != null && !m_resultView.Grid.Focused)
				{
					m_resultView.Grid.Focus();
				}
				
				UpdateRecordView();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab's result view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultView ResultView
		{
			get { return m_resultView; }
			set
			{
				if (m_resultView == value)
					return;

				if (value == null)
					Clear();

				m_resultView = value;
				if (m_resultView != null)
				{
					m_query = m_resultView.SearchQuery;
					m_resultView.Dock = DockStyle.Fill;
					SubscribeToGridEvents();
					UpdateRecordView();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's grid control from it's result view control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid Grid
		{
			get	{return (m_resultView == null ? null : m_resultView.Grid);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab's search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery SearchQuery
		{
			get { return m_query; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTabGroup OwningTabGroup
		{
			get { return m_owningTabGroup; }
			set { m_owningTabGroup = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the text in a tab was clipped (i.e. was
		/// too long so it is displayed with ellipses).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool TabTextClipped
		{
			get { return m_tabTextClipped; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get
			{
				if (!m_selected)
					return m_inactiveTabFore;

				return (m_owningTabGroup.IsCurrent ?
					m_activeTabFore : m_activeTabInactiveGroupFore);
			}
			set { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get
			{
				if (!m_selected)
					return m_inactiveTabBack;

				return (m_owningTabGroup.IsCurrent ? m_activeTabBack : SystemColors.Control);
			}
			set
			{
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the tab's width based on it's text and font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void AdjustWidth()
		{
			TextFormatFlags flags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

			int width;

			// Get the text's width.
			using (Graphics g = CreateGraphics())
				width = TextRenderer.MeasureText(g, Text, Font, Size.Empty, flags).Width;

			// Add a little for good measure.
			width += 6;

			if (m_image != null)
				width += (kleftImgMargin + m_image.Width);

			if (m_btnCIEOptions.Visible)
				width += (kleftImgMargin + m_btnCIEOptions.Width);

			// Don't allow the width of a tab to be any
			// wider than 3/4 of it's owning group's width.
			Width = Math.Min(width, (int)(m_owningTabGroup.Width * 0.75));

			m_tabTextClipped = (Width < width);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the search results on the tab and sets the tab to an empty tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			RemoveResultView();

			if (m_owningTabGroup.RecordView != null)
				m_owningTabGroup.RecordView.Clear();

			m_query = new SearchQuery();
			m_btnCIEOptions.Visible = false;
			Text = Properties.Resources.kstidEmptySrchResultTabText;
			AdjustWidth();
			m_owningTabGroup.AdjustTabContainerWidth();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the tab's result view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveResultView()
		{
			if (m_resultView != null)
			{
				UnsubscribeToGridEvents();
				if (m_owningTabGroup != null && m_owningTabGroup.Controls.Contains(m_resultView))
					m_owningTabGroup.Controls.Remove(m_resultView);

				m_resultView.Dispose();
				m_resultView = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the search results as a result of the project's underlying data sources
		/// changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			if (m_resultView != null)
			{
				UnsubscribeToGridEvents();
				m_resultView.RefreshResults();
				SubscribeToGridEvents();
				UpdateRecordView();
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SubscribeToGridEvents()
		{
			if (m_resultView == null)
				return;

			if (m_owningTabGroup.TMAdapter != null)
			{
				m_owningTabGroup.TMAdapter.SetContextMenuForControl(
					m_resultView, "cmnuSearchResultTab");
			}

			if (m_resultView.Grid != null)
			{
				if (m_owningTabGroup.TMAdapter != null)
				{
					m_owningTabGroup.TMAdapter.SetContextMenuForControl(
						m_resultView.Grid, "cmnuSearchResultTab");
				}

				m_resultView.Grid.AllowDrop = true;
				m_resultView.Grid.DragOver += HandleResultViewDragOver;
				m_resultView.Grid.DragDrop += HandleResultViewDragDrop;
				m_resultView.Grid.DragLeave +=HandleResultViewDragLeave;
				m_resultView.Grid.RowEnter += HandleResultViewRowEnter;
				m_resultView.Grid.Enter += HandleResultViewEnter;
				m_resultView.Grid.AllowDrop = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UnsubscribeToGridEvents()
		{
			if (m_resultView != null && m_resultView.Grid != null)
			{
				m_resultView.Grid.AllowDrop = false;
				m_resultView.Grid.DragOver -= HandleResultViewDragOver;
				m_resultView.Grid.DragDrop -= HandleResultViewDragDrop;
				m_resultView.Grid.DragLeave -= HandleResultViewDragLeave;
				m_resultView.Grid.RowEnter -= HandleResultViewRowEnter;
				m_resultView.Grid.Enter -= HandleResultViewEnter;
			}
		}

		#region Overridden methods and event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the current tab is selected when its grid get's focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleResultViewEnter(object sender, EventArgs e)
		{
			if (!m_selected || !m_owningTabGroup.IsCurrent)
				m_owningTabGroup.SelectTab(this, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the record pane with the data source's record for the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleResultViewRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			UpdateRecordView(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the record view after a sort has taken place since the grid's RowEnter
		/// event doesn't seem to take care of it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnWordListGridSorted(object args)
		{
			UpdateRecordView();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateRecordView()
		{
			UpdateRecordView(-1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateRecordView(int rowIndex)
		{
			if (!m_selected || (m_owningTabGroup != null && !m_owningTabGroup.IsCurrent))
				return;

			if (m_owningTabGroup.RecordView == null || m_resultView == null ||
				!m_owningTabGroup.IsCurrent || m_resultView.Grid == null)
			{
				m_owningTabGroup.RecordView.UpdateRecord(null);
			}
			else
			{
				RecordCacheEntry entry = (rowIndex < 0 ? m_resultView.Grid.GetRecord() :
					m_resultView.Grid.GetRecord(rowIndex));

				m_owningTabGroup.RecordView.UpdateRecord(entry);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the CV patterns get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCVPatternsChanged(object args)
		{
			return OnRecordViewOptionsChanged(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the record view when the user changed the order or visibility of fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRecordViewOptionsChanged(object args)
		{
			if (m_selected && m_owningTabGroup.IsCurrent &&
				m_owningTabGroup.RecordView != null &&
				m_resultView != null && m_resultView.Grid != null)
			{
				m_owningTabGroup.RecordView.UpdateRecord(
					m_resultView.Grid.GetRecord(), true);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat dragging on a result view grid just like dragging on the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleResultViewDragOver(object sender, DragEventArgs e)
		{
			m_owningTabGroup.InternalDragOver(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat dragging on a result view grid just like dragging on the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleResultViewDragLeave(object sender, EventArgs e)
		{
			m_owningTabGroup.InternalDragLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Treat dropping on a result view grid just like dropping on the tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleResultViewDragDrop(object sender, DragEventArgs e)
		{
			m_owningTabGroup.InternalDragDrop(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reflects drag over events to the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			m_owningTabGroup.InternalDragOver(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reflects drag leave events to the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			m_owningTabGroup.InternalDragLeave(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reflects drag drop events to the tab's owning group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			m_owningTabGroup.InternalDragDrop(e);
		}

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
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			m_btnCIEOptions.Top = (Height - m_btnCIEOptions.Height) / 2 + 1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rc = ClientRectangle;
			e.Graphics.FillRectangle(SystemBrushes.Control, rc);

			int topMargin = (m_selected ? 0 : 2);

			// Establish the points that outline the region for the tab outline (which
			// also marks off it's interior).
			Point[] pts = new Point[] {
				new Point(0, rc.Bottom), new Point(0, rc.Top + topMargin + 3),
				new Point(3, topMargin), new Point(rc.Right - 4, topMargin),
				new Point(rc.Right - 1, rc.Top + topMargin + 3),
				new Point(rc.Right - 1, rc.Bottom)};

			// First, clear the decks with an all white background.
			using (SolidBrush br = new SolidBrush(Color.White))
				e.Graphics.FillPolygon(br, pts);

			if (!m_selected || m_owningTabGroup.IsCurrent)
			{
				using (SolidBrush br = new SolidBrush(BackColor))
					e.Graphics.FillPolygon(br, pts);
			}
			else
			{
				// The tab is the current tab but is not in the current
				// tab group so paint with a gradient background.
				//Color clr1 = Color.FromArgb(120, SystemColors.ControlDark);
				//Color clr2 = Color.FromArgb(150, SystemColors.Control);
				using (LinearGradientBrush br = new LinearGradientBrush(rc,
					m_activeTabInactiveGroupBack1, m_activeTabInactiveGroupBack2, 70))
				{
					e.Graphics.FillPolygon(br, pts);
				}
			}

			e.Graphics.DrawLines(SystemPens.ControlDark, pts);

			if (!m_selected)
			{
				// The tab is not the selected tab, so draw a
				// line across the bottom of the tab.
				e.Graphics.DrawLine(SystemPens.ControlDark,
					0, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
			}

			if (!m_btnCIEOptions.Visible)
				DrawImage(e.Graphics, ref rc);
			else
			{
				rc.X += (kleftImgMargin + m_btnCIEOptions.Width);
				rc.Width -= (kleftImgMargin + m_btnCIEOptions.Width);
			}

			if (!m_selected)
			{
				rc.Y += topMargin;
				rc.Height -= topMargin;
			}

			DrawText(e.Graphics, ref rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the tab's image.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawImage(Graphics g, ref Rectangle rc)
		{
			if (m_image != null)
			{
				Rectangle rcImage = new Rectangle();
				rcImage.Size = m_image.Size;
				rcImage.X = rc.Left + kleftImgMargin;
				rcImage.Y = rc.Top + (rc.Height - rcImage.Height) / 2;
				g.DrawImage(m_image, rcImage);
				rc.X += (kleftImgMargin + rcImage.Width);
				rc.Width -= (kleftImgMargin + rcImage.Width);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the tab's text
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawText(Graphics g, ref Rectangle rc)
		{
			TextFormatFlags flags = TextFormatFlags.VerticalCenter |
				TextFormatFlags.WordEllipsis | TextFormatFlags.SingleLine |
				TextFormatFlags.NoPadding | TextFormatFlags.LeftAndRightPadding |
				TextFormatFlags.PreserveGraphicsClipping;

			if (m_image == null)
				flags |= TextFormatFlags.HorizontalCenter;

			rc.Height -= 3;
			TextRenderer.DrawText(g, Text, Font, rc, ForeColor, flags);

			if (m_mouseOver)
			{
				// Draw the lines that only show when the mouse is over the tab.
				using (Pen pen = new Pen(Color.DarkOrange))
				{
					int topLine = (m_selected ? 1 : 3);
					g.DrawLine(pen, 3, topLine, rc.Right - 4, topLine);
					g.DrawLine(pen, 2, topLine + 1, rc.Right - 3, topLine + 1);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Text;
		}

		#endregion

		#region Minimal pair (i.e. CIE) options drop-down related methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ShowCIEOptions()
		{
			if (m_btnCIEOptions.Visible)
				m_owningTabGroup.ShowCIEOptions(m_btnCIEOptions);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnCIEOptions_Click(object sender, EventArgs e)
		{
			if (!m_selected || !m_owningTabGroup.IsCurrent)
				m_owningTabGroup.SelectTab(this, true);

			ShowCIEOptions();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnCIEOptions_MouseLeave(object sender, EventArgs e)
		{
			m_CIEButtonToolTip.Hide(this);
			m_CIEButtonToolTip.Dispose();
			m_CIEButtonToolTip = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnCIEOptions_MouseEnter(object sender, EventArgs e)
		{
			m_CIEButtonToolTip = new ToolTip();
			Point pt = PointToClient(MousePosition);
			pt.Y += (Cursor.Size.Height - (int)(Cursor.Size.Height * 0.3));
			m_CIEButtonToolTip.Show(Properties.Resources.kstidCIEOptionsButtonToolTip, this, pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ToggleCIEView()
		{
			if (m_resultView != null && m_resultView.Grid != null && m_resultView.Grid.Cache != null)
			{
				if (m_resultView.Grid.Cache.IsCIEList)
					m_resultView.Grid.CIEViewOff();
				else
					m_resultView.Grid.CIEViewOn();

				// Force users to restart Find when toggling the CIEView
				FindInfo.CanFindAgain = false;

				m_btnCIEOptions.Visible = m_resultView.Grid.Cache.IsCIEList;
				AdjustWidth();
				m_owningTabGroup.AdjustTabContainerWidth();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void CIEViewRefresh()
		{
			if (m_resultView.Grid == null || !m_resultView.Grid.CIEViewRefresh())
			{
				m_btnCIEOptions.Visible = false;
				AdjustWidth();
				m_owningTabGroup.AdjustTabContainerWidth();
			}
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never used in PA.
		/// </summary>
		/// <param name="mediator"></param>
		/// <param name="configurationParameters"></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, XmlNode configurationParameters)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the message target.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion
	}


	#region TabDropIndicator class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TabDropIndicator : TranslucentOverlay
	{
		private const int kDefaultIndicatorWidth = 50;
		private readonly SearchResultTabGroup m_tabGroup;
		private readonly int m_height;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TabDropIndicator(SearchResultTabGroup tabGroup, int height) : base(tabGroup)
		{
			m_tabGroup = tabGroup;
			m_height = height;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rc = ClientRectangle;

			Point[] pts = new Point[] {new Point(rc.X, rc.Bottom),
		        new Point(rc.X, rc.Y + 3), new Point(rc.X + 3, rc.Y),
		        new Point(rc.Right - 4, rc.Y), new Point(rc.Right - 1, rc.Y + 3),
		        new Point(rc.Right - 1, rc.Bottom)};

			using (HatchBrush br = new HatchBrush(HatchStyle.Percent50, Color.Black, Color.Transparent))
				e.Graphics.FillPolygon(br, pts);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When draggingTab is true it means the indicator is used for a tab being dragged.
		/// When draggingTab is false it means the indicator is used for a search pattern
		/// being dragged (e.g. from the saved patterns list).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Locate(bool draggingTab)
		{
			Point pt = GetIndicatorLocation(draggingTab);

			// If the point where we figured on placing the indicator
			// is too far to the right, then bump it left so it just fits.
			if (pt.X + Width > m_tabGroup.Width)
				pt.X = m_tabGroup.Width - Width + 1;

			Location = pt;
			Show();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will determine where to place the indicator and how wide it should be.
		/// In making that determination, consideration is made for what is being dragged and
		/// whether or not the active tab in the target tab group is empty. When tabs are
		/// being dragged, then the indicator will always show up at the end of all existing
		/// tabs in the target tab group. If a pattern is being dragged and the active tab
		/// in the target tab group is empty, the indicator will be placed over that tab.
		/// Otherwise, the indicator will be shown at the end of all existing tabs in the
		/// target tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Point GetIndicatorLocation(bool draggingTab)
		{
			Point pt;

			if (m_tabGroup.Tabs == null || m_tabGroup.Tabs.Count == 0)
				pt = m_tabGroup.m_pnlHdrBand.PointToScreen(m_tabGroup.m_pnlHdrBand.Location);
			else
			{
				SearchResultTab tab = (!draggingTab && m_tabGroup.CurrentTab.IsEmpty ?
					m_tabGroup.CurrentTab : m_tabGroup.Tabs[m_tabGroup.Tabs.Count - 1]);

				if (!draggingTab && tab.IsEmpty && tab.Selected)
				{
					pt = tab.PointToScreen(new Point(0, 0));
					Size = new Size(tab.Width, m_height);
				}
				else
				{
					pt = tab.PointToScreen(new Point(tab.Width, 0));
					Size = new Size(kDefaultIndicatorWidth, m_height);
				}
			}

			return m_tabGroup.PointToClient(pt);
		}
	}

	#endregion
}
