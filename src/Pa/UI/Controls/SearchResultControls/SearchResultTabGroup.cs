using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchResultTabGroup : Panel, IxCoreColleague
	{
		internal Panel m_pnlHdrBand;
		//private Panel m_pnlScroll;
		//private XButton m_btnAddTab;
		//private XButton m_btnLeft;
		//private XButton m_btnRight;
		//private int m_minScrollPanelWidth;

		private SearchResultTabGroupButtonPanel m_buttonPanel;
		
		internal ToolTip m_tooltip;
		private SearchResultTabGroup m_contextMenuTabGroup;
		//private Panel m_pnlClose;
		//private XButton m_btnClose;
		private readonly Panel m_pnlTabs;
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

			SetupCloseTabsButton();
			// Create the panel that will hold the close button
			
			SetupButtonPanel();

			m_dropIndicator = new TabDropIndicator(this, m_pnlTabs.Height);

			Tabs = new List<SearchResultTab>();
			m_rsltVwMngr = rsltVwMngr;
			App.AddMediatorColleague(this);

			SetContextMenus();
			SetToolTips();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (m_pnlHdrBand.ContextMenuStrip != null)
				m_pnlHdrBand.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;
			
			m_pnlHdrBand.Dispose();
			m_pnlTabs.Dispose();

			if (m_tooltip != null)
				m_tooltip.Dispose();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void SetupCloseTabsButton()
		{
			//m_pnlClose = new Panel();
			//m_pnlClose.Width = (Settings.Default.ShowSingleCloseButtonForSearchResultTabs ? 22 : 0);
			//m_pnlClose.Visible = (m_pnlClose.Width > 0);
			//m_pnlClose.Dock = DockStyle.Right;
			//m_pnlClose.Paint += HandleCloseScrollPanelPaint;
			//m_pnlHdrBand.Controls.Add(m_pnlClose);

			//// Create a button that will close a tab.
			//m_btnClose = new XButton();
			//m_btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
			//m_btnClose.Click += delegate
			//{
			//    if (CurrentTab != null)
			//        RemoveTab(CurrentTab, true);
			//};
				
			//m_btnClose.Location = new Point(m_pnlClose.Width - m_btnClose.Width,
			//    (m_pnlHdrBand.Height - m_btnClose.Height) / 2 - 3);

			//m_pnlClose.Controls.Add(m_btnClose);
			//m_pnlClose.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupButtonPanel()
		{
			m_buttonPanel = new SearchResultTabGroupButtonPanel();
			m_buttonPanel.Dock = DockStyle.Right;
			m_pnlHdrBand.Controls.Add(m_buttonPanel);
			m_buttonPanel.BringToFront();

			m_buttonPanel.AddClickAction = (() => SelectTab(AddTab(), true));

			m_buttonPanel.AddInSideBySideGroupClickAction = (() =>
				m_rsltVwMngr.CreateTab(SearchResultLocation.NewSideBySideTabGroup));

			m_buttonPanel.AddInStackedGroupClickAction = (() =>
				m_rsltVwMngr.CreateTab(SearchResultLocation.NewStackedTabGroup));

			m_buttonPanel.ScrollLeftClickAction = (() =>
			{
				int left = m_pnlTabs.Left;

				// Find furthest right tab that's partially obscurred and needs to be scrolled into view.
				foreach (SearchResultTab tab in Tabs)
				{
					if (left < 0 && left + tab.Width >= 0)
					{
						SlideTabs(m_pnlTabs.Left + Math.Abs(left));
						break;
					}

					left += tab.Width;
				}
			});

			m_buttonPanel.ScrollRightClickAction = (() =>
			{
				//int left = m_pnlTabs.Left;

				//// Find furthest left tab that's partially obscurred and needs to be scrolled into view.
				//foreach (SearchResultTab tab in Tabs)
				//{
				//    if (left <= m_pnlScroll.Left && left + tab.Width > m_pnlScroll.Left)
				//    {
				//        int dx = (left + tab.Width) - m_pnlScroll.Left;
				//        SlideTabs(m_pnlTabs.Left - dx);
				//        break;
				//    }

				//    left += tab.Width;
				//}
			});





//            // Create the panel that will hold the close button
//            m_pnlScroll = new Panel();
//            //m_pnlScroll.Width = 40;
//            m_pnlScroll.Visible = true;
//            m_pnlScroll.Dock = DockStyle.Right;
//            m_pnlScroll.Paint += HandleCloseScrollPanelPaint;
////			m_pnlHdrBand.Controls.Add(m_pnlScroll);
//            m_pnlScroll.Visible = false;
//            m_pnlScroll.BringToFront();

//            // Create a left scrolling button.
//            m_btnLeft = new XButton();
//            m_btnLeft.DrawLeftArrowButton = true;
//            m_btnLeft.Size = new Size(18, 18);
//            m_btnLeft.Anchor = AnchorStyles.Right | AnchorStyles.Top;
//            m_btnLeft.Click += m_btnLeft_Click;
//            m_btnLeft.Location = new Point(4, (m_pnlHdrBand.Height - m_btnLeft.Height) / 2 - 3);
//            //m_pnlScroll.Controls.Add(m_btnLeft);

//            // Create a right scrolling button.
//            m_btnRight = new XButton();
//            m_btnRight.DrawRightArrowButton = true;
//            m_btnRight.Size = new Size(18, 18);
//            m_btnRight.Anchor = AnchorStyles.Right | AnchorStyles.Top;
//            m_btnRight.Click += m_btnRight_Click;
//            m_btnRight.Location = new Point(22, (m_pnlHdrBand.Height - m_btnRight.Height) / 2 - 3);
//            //m_pnlScroll.Controls.Add(m_btnRight);

		//    // Create a button for adding a single tab.
		//    m_btnAddTab = new XButton();
		//    m_btnAddTab.BackColor = Color.Transparent;
		//    m_btnAddTab.Image = Properties.Resources.NewTabNormal;
		//    m_btnAddTab.Size = m_btnAddTab.Image.Size;
		//    m_btnAddTab.Anchor = AnchorStyles.Left;
		//    m_btnAddTab.Top = (m_pnlScroll.Height - m_btnAddTab.Height) / 2;
		//    m_btnAddTab.Left = 3;
		//    m_btnAddTab.DrawBackground += delegate { return true; };
		//    m_btnAddTab.MouseEnter += delegate { m_btnAddTab.Image = Properties.Resources.NewTabHot; };
		//    m_btnAddTab.MouseLeave += delegate { m_btnAddTab.Image = Properties.Resources.NewTabNormal; };
		//    m_btnAddTab.Click += delegate { AddTab(); };
		////	m_pnlScroll.Controls.Add(m_btnAddTab);

		//    // TODO: Factor in close button width.
		//    //m_minScrollPanelWidth = m_btnAddTab.Width + m_btnLeft.Width + m_btnRight.Width + 7;
		}

		/// ------------------------------------------------------------------------------------
		void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			var cms = sender as ContextMenuStrip;
			if (cms != null && cms.SourceControl != null)
				m_contextMenuTabGroup = cms.SourceControl.Parent as SearchResultTabGroup;
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustPanelHeights()
		{
			using (Graphics g = CreateGraphics())
			{
				int extraTabHeight = Settings.Default.SearchVwExtraSearchTabHeight;

				const TextFormatFlags kFlags = TextFormatFlags.VerticalCenter |
					TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding;

				m_pnlHdrBand.Height = TextRenderer.MeasureText(g, "X", FontHelper.PhoneticFont,
					new Size(int.MaxValue, int.MaxValue), kFlags).Height + extraTabHeight;
			}

			m_pnlTabs.Height = m_pnlHdrBand.Height - 5;
			m_pnlHdrBand.Invalidate(true);
		}

		/// ------------------------------------------------------------------------------------
		private void SetContextMenus()
		{
			if (m_pnlHdrBand.ContextMenuStrip != null)
				m_pnlHdrBand.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;

			if (TMAdapter != null)
				TMAdapter.SetContextMenuForControl(m_pnlHdrBand, "cmnuSearchResultTabGroup");

			if (m_pnlHdrBand.ContextMenuStrip != null)
				m_pnlHdrBand.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
		}

		/// ------------------------------------------------------------------------------------
		private void SetToolTips()
		{
			//m_tooltip = new ToolTip();

			//m_tooltip.SetToolTip(m_btnClose,
			//    App.LocalizeString("SearchResultTabs.CloseToolTipText", "Close Active Tab",
			//    App.kLocalizationGroupMisc));

			//m_tooltip.SetToolTip(m_btnLeft,
			//    App.LocalizeString("SearchResultTabs.ScrollLeftToolTipText", "Scroll Left",
			//    App.kLocalizationGroupMisc));

			//m_tooltip.SetToolTip(m_btnRight,
			//    App.LocalizeString("SearchResultTabs.ScrollRightToolTipText", "Scroll Right",
			//    App.kLocalizationGroupMisc));
		}

		#region Message mediator message handler and update handler methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewDocked(object args)
		{
			// Restore the tab group's context menu.
			SetContextMenus();

			// Restore the context menu for each tab.
			if (Tabs != null)
			{
				foreach (SearchResultTab tab in Tabs)
					tab.SetContextMenus();
			}

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

				string replacedText = App.Project.SearchClasses.ModifyPatternText(tab.Text);
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
			if (Tabs != null)
			{
				foreach (SearchResultTab tab in Tabs)
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
			App.RemoveMediatorColleague(this);
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

			const TextFormatFlags kFlags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding |
				TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter |
				TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping;

			Rectangle rc = ClientRectangle;
			rc.Y = m_pnlHdrBand.Bottom;
			rc.Height -= m_pnlHdrBand.Height;
			Color clr = (IsCurrent ? SystemColors.ControlText : SystemColors.GrayText);

			using (Font fnt = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold))
			{
				var text = App.LocalizeString("SearchResultTabs.EmptyTabInfoText",
					"Define a search pattern above and click Show Results.",
					App.kLocalizationGroupMisc);
				
				TextRenderer.DrawText(e.Graphics, text, fnt, rc, clr, kFlags);
			}

			App.DrawWatermarkImage("kimidSearchWatermark", e.Graphics, ClientRectangle);
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
		private void HandleClick(object sender, EventArgs e)
		{
			if (CurrentTab != null)
				HandleTabClick(CurrentTab, EventArgs.Empty);
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
			var tab = new SearchResultTab(this);
			AddTab(tab);
			return tab;
		}

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
			tab.Click += HandleTabClick;
			tab.MouseDown += HandleMouseDown;
			InitializeTab(tab, tab.ResultView, false);
			m_pnlTabs.Controls.Add(tab);
			tab.BringToFront();
			Tabs.Add(tab);
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
		internal void AdjustTabContainerWidth()
		{
			m_pnlTabs.SuspendLayout();
			m_pnlTabs.Width = Tabs.Sum(tab => tab.Width);
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
				tab.Click -= HandleTabClick;
				tab.MouseDown -= HandleMouseDown;

				if (tab.ResultView != null)
				{
					tab.ResultView.Click -= HandleClick;
					tab.ResultView.MouseDown -= HandleMouseDown;
				}

				if (Controls.Contains(tab.ResultView))
					Controls.Remove(tab.ResultView);

				m_pnlTabs.Controls.Remove(tab);
				Tabs.Remove(tab);

				if (disposeOfTab)
					tab.Dispose();

				AdjustTabContainerWidth();
				m_btnLeft_Click(null, null);
				RefreshScrollButtonPanel();

				// If removing the tab left a gap between the furthest right tab and the
				// button panel... and all or a portion of the furthest left tab are
				// scrolled out of view, then scroll right to close that gap.
				if (m_pnlTabs.Left < 0 && m_pnlTabs.Right < m_buttonPanel.Left)
				{
					int dx = (m_buttonPanel.Left - m_pnlTabs.Right);
					SlideTabs(m_pnlTabs.Left + dx);
				}
			}

			// If the last tab was removed from the group, then close the tab group by
			// removing ourselves from our parent's control collection.
			if (Tabs.Count == 0 && Parent != null)
			{
				Controls.Clear();
				Parent.Controls.Remove(this);
				Dispose();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SelectTab(SearchResultTab newSelectedTab, bool makeTabCurrent)
		{
			if (newSelectedTab == null)
				return;

			if (makeTabCurrent)
			{
				m_rsltVwMngr.SearchResultTabGroupChanged(this);

				// This is used to inform other tab groups in the same view tabs manager.
				App.MsgMediator.SendMessage("SearchResultTabGroupChanged", this);
			}

			newSelectedTab.Selected = true;
			CurrentTab = newSelectedTab;

			foreach (SearchResultTab tab in Tabs)
			{
				if (tab != newSelectedTab)
					tab.Selected = false;
			}

			if (makeTabCurrent)
			{
				EnsureTabVisible(CurrentTab);

				// Make sure the tab's grid has focus.
				if (CurrentTab.ResultView != null && CurrentTab.ResultView.Grid != null)
					CurrentTab.ResultView.Grid.Focus();

				m_rsltVwMngr.CurrentSearchResultTabChanged(CurrentTab);

				// Sometimes selecting an empty tab causes a chain reaction caused by hiding
				// the former selected tab's grid. Doing that forces the .Net framework to
				// look for the next visible control in line that can be focused. Sometimes
				// that means a grid on another tab will get focus and ultimately cause that
				// grid's tab to become selected, thus negating the fact that we just got
				// through setting this tab group as current. Therefore, force the issue
				// again.
				if (!IsCurrent)
				{
					m_rsltVwMngr.SearchResultTabGroupChanged(this);
					App.MsgMediator.SendMessage("SearchResultTabGroupChanged", this);
					m_rsltVwMngr.CurrentSearchResultTabChanged(CurrentTab);
				}

				if (CurrentTab.ResultView != null && CurrentTab.ResultView.Grid != null)
					CurrentTab.ResultView.Grid.IsCurrentPlaybackGrid = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void EnsureTabVisible(SearchResultTab tab)
		{
			RefreshScrollButtonPanel();
			
			int availableWidth = m_pnlHdrBand.Width - m_buttonPanel.GetMinWidth();

			if (tab.Width > availableWidth)
				return;

			int maxRight = m_buttonPanel.Left;
			
			// Get the tab's left and right edge relative to the header panel.	
			int left = tab.Left + m_pnlTabs.Left;
			int right = left + tab.Width;

			// Check if it's already fully visible.
			if (left >= 0 && right < maxRight)
				return;

			// Slide the panel in the proper direction to make it visible.
			int dx = (left < 0 ? left : right - maxRight);
			SlideTabs(m_pnlTabs.Left - dx);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsTabsRightEdgeVisible(SearchResultTab tab)
		{
			var pt = tab.PointToScreen(new Point(tab.Width, 0));
			pt = m_pnlHdrBand.PointToClient(pt);
			return (pt.X <= m_buttonPanel.Left);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTabClick(object sender, EventArgs e)
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
					HandleTabClick(CurrentTab, null);
				else
					HandleTabClick(sender, null);
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
			if (ContextMenuTab != null)
			{
				App.MsgMediator.SendMessage("ReflectMoveToNewSideBySideTabGroup",
					ContextMenuTab);

				ContextMenuTab = null;
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
			if (ContextMenuTab != null)
			{
				App.MsgMediator.SendMessage("ReflectMoveToNewStackedTabGroup",
					ContextMenuTab);

				ContextMenuTab = null;
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnCloseTab(object args)
		{
			if (ContextMenuTab != null)
			{
				RemoveTab(ContextMenuTab, true);
				ContextMenuTab = null;
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnCloseTabGroup(object args)
		{
			if (ContextMenuTab != null || m_contextMenuTabGroup == this)
			{
				Close();
				ContextMenuTab = null;
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
			while (Tabs.Count > 0)
				RemoveTab(Tabs[0], true);

			App.RemoveMediatorColleague(this);
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
			if (!Tabs.Contains(ContextMenuTab))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (Tabs.Count > 1);
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
			if (!Tabs.Contains(ContextMenuTab))
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (Tabs.Count > 1);
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
				IsCurrent = (group == this);

				foreach (SearchResultTab tab in Tabs)
				{
					tab.Invalidate();
					if (tab.ResultView != null && tab.ResultView.Grid != null)
						tab.ResultView.Grid.IsCurrentPlaybackGrid = false;
				}

				// Force the text in empty views to be redrawn. This is only necessary
				// when there is more than one tab group and the current tab in any one
				// of those groups is empty. (The text is drawn disabled looking when
				// the tab's owning tab group isn't current).
				if (CurrentTab != null && CurrentTab.ResultView == null)
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
		void m_pnlHdrBand_Resize(object sender, EventArgs e)
		{
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		private void RefreshScrollButtonPanel()
		{
			if (m_pnlTabs == null || m_pnlHdrBand == null || m_buttonPanel == null)
				return;

			m_buttonPanel.ScrollButtonsVisible =
				(m_pnlTabs.Width > (m_pnlHdrBand.Width - m_buttonPanel.GetMinWidth(false)));

			int maxButtonPanelWidth = m_pnlHdrBand.Width - m_pnlTabs.Width;
			m_buttonPanel.Width = Math.Max(maxButtonPanelWidth, m_buttonPanel.GetMinWidth());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scroll the tabs to the right (i.e. move the tab's panel to the right) so user is
		/// able to see tabs obscured on the left side.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnLeft_Click(object sender, EventArgs e)
		{
			//int left = m_pnlTabs.Left;

			//// Find the furthest right tab that is partially
			//// obscurred and needs to be scrolled into view.
			//foreach (SearchResultTab tab in Tabs)
			//{
			//    if (left < 0 && left + tab.Width >= 0)
			//    {
			//        SlideTabs(m_pnlTabs.Left + Math.Abs(left));
			//        break;
			//    }

			//    left += tab.Width;
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scroll the tabs to the left (i.e. move the tab's panel to the left) so user is
		/// able to see tabs obscured on the right side.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_btnRight_Click(object sender, EventArgs e)
		{
			//int left = m_pnlTabs.Left;

			//// Find the furthest left tab that is partially
			//// obscurred and needs to be scrolled into view.
			//foreach (SearchResultTab tab in Tabs)
			//{
			//    if (left <= m_pnlScroll.Left && left + tab.Width > m_pnlScroll.Left)
			//    {
			//        int dx = (left + tab.Width) - m_pnlScroll.Left;
			//        SlideTabs(m_pnlTabs.Left - dx);
			//        break;
			//    }

			//    left += tab.Width;
			//}
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
				HandleTabClick(tab, null);
			}
			else if (query != null && !query.PatternOnly)
			{
				SelectTab(CurrentTab, true);
				App.MsgMediator.SendMessage("PatternDroppedOnTabGroup", query);
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
		internal SearchResultTab ContextMenuTab { private get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current tab in the group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTab CurrentTab { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the tab control is the tab control with
		/// the focused child grid or record view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCurrent { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab group's record view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IRecordView RecordView
		{
			get { return (m_rsltVwMngr != null ? m_rsltVwMngr.RecordView : null); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the tab group's toolbar/menu adapter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return (m_rsltVwMngr != null ? m_rsltVwMngr.TMAdapter : null); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<SearchResultTab> Tabs { get; set; }

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
				foreach (SearchResultTab tab in Tabs)
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
			if (CurrentTab == null || CurrentTab.ResultView == null || CurrentTab.ResultView.Grid == null)
				return;

			if (CurrentTab.CieOptionsDropDown == null)
				CurrentTab.CieOptionsDropDown = new CIEOptionsDropDown();

			if (CurrentTab.CieOptionsDropDownContainer == null)
			{
				CurrentTab.CieOptionsDropDownContainer = new CustomDropDown();
				CurrentTab.CieOptionsDropDownContainer.AddControl(CurrentTab.CieOptionsDropDown);
			}

			CurrentTab.CieOptionsDropDown.CIEOptions = CurrentTab.ResultView.Grid.CIEOptions;
			CurrentTab.CieOptionsDropDownContainer.Closed += m_cieOptionsDropDownContainer_Closed;
			Point pt = ctrl.PointToScreen(new Point(0, ctrl.Height));
			CurrentTab.CieOptionsDropDownContainer.Show(pt);
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

			if (CurrentTab.CieOptionsDropDown.OptionsChanged)
			{
				// Save the options as the new defaults for the project.
				App.Project.CIEOptions = CurrentTab.CieOptionsDropDown.CIEOptions;
				App.Project.Save();
				CurrentTab.ResultView.Grid.CIEOptions = CurrentTab.CieOptionsDropDown.CIEOptions;
				CurrentTab.CIEViewRefresh();
			}

			CurrentTab.CieOptionsDropDownContainer.Closed -= m_cieOptionsDropDownContainer_Closed;
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
