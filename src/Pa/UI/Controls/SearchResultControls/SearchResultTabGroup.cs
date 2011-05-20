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
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Control that contains a row of tabs and its associated add new tab, scroll right and
	/// scroll left buttons. It also contains a single search result word list below the
	/// row of tabs.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchResultTabGroup : Panel, IxCoreColleague
	{
		internal Panel TabsContainer { get; private set; }
		private SearchResultTabGroupButtonPanel m_buttonPanel;
		private SearchResultTabGroup m_contextMenuTabGroup;
		private readonly Panel m_tabsPanel;
		private readonly SearchResultsViewManager m_rsltVwMngr;
		private readonly TabDropIndicator m_dropIndicator;

		/// ------------------------------------------------------------------------------------
		public SearchResultTabGroup(SearchResultsViewManager rsltVwMngr)
		{
			Visible = true;
			base.DoubleBuffered = true;
			base.AllowDrop = true;

			// Create the panel that holds everything that will be displayed
			// above a result view (i.e. tabs, close button and scroll buttons).
			TabsContainer = new Panel();
			TabsContainer.Dock = DockStyle.Top;
			TabsContainer.Padding = new Padding(0, 0, 0, 5);
			TabsContainer.Paint += HandlePanelPaint;
			TabsContainer.Resize += HandleHeaderBandPanelResize;
			TabsContainer.Click += HandleClick;
			TabsContainer.MouseDown += delegate { HandleClick(null, null); };
			Controls.Add(TabsContainer);

			// Create the panel that holds all the tabs. 
			m_tabsPanel = new Panel();
			m_tabsPanel.Visible = true;
			m_tabsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			m_tabsPanel.Location = new Point(0, 0);
			m_tabsPanel.Click += HandleClick;
			TabsContainer.Controls.Add(m_tabsPanel);

			AdjustPanelHeights();
			SetupButtonPanel();

			m_dropIndicator = new TabDropIndicator(this, m_tabsPanel.Height);

			Tabs = new List<SearchResultTab>();
			m_rsltVwMngr = rsltVwMngr;
			App.AddMediatorColleague(this);

			SetContextMenus();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (TabsContainer.ContextMenuStrip != null)
				TabsContainer.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;
			
			TabsContainer.Dispose();
			m_buttonPanel.Dispose();
			m_tabsPanel.Dispose();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void SetupButtonPanel()
		{
			m_buttonPanel = new SearchResultTabGroupButtonPanel();
			m_buttonPanel.Dock = DockStyle.Right;
			TabsContainer.Controls.Add(m_buttonPanel);
			m_buttonPanel.BringToFront();

			m_buttonPanel.AddClickAction = (() => SelectTab(AddTab(), true));

			m_buttonPanel.AddInSideBySideGroupClickAction = (() =>
				m_rsltVwMngr.CreateTab(SearchResultLocation.NewSideBySideTabGroup));

			m_buttonPanel.AddInStackedGroupClickAction = (() =>
				m_rsltVwMngr.CreateTab(SearchResultLocation.NewStackedTabGroup));

			m_buttonPanel.ScrollLeftClickAction = (() =>
			{
				int left = m_tabsPanel.Left;

				// Find furthest right tab that's partially obscurred and needs to be scrolled into view.
				foreach (SearchResultTab tab in Tabs)
				{
					if (left < 0 && left + tab.Width >= 0)
					{
						SlideTabs(m_tabsPanel.Left + Math.Abs(left));
						break;
					}

					left += tab.Width;
				}
			});

			m_buttonPanel.ScrollRightClickAction = (() =>
			{
				int left = m_tabsPanel.Left;

				// Find furthest left tab that's partially obscurred and needs to be scrolled into view.
				foreach (SearchResultTab tab in Tabs)
				{
					if (left <= m_buttonPanel.Left && left + tab.Width > m_buttonPanel.Left)
					{
						int dx = (left + tab.Width) - m_buttonPanel.Left;
						SlideTabs(m_tabsPanel.Left - dx);
						break;
					}

					left += tab.Width;
				}
			});
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

				TabsContainer.Height = TextRenderer.MeasureText(g, "X", App.PhoneticFont,
					new Size(int.MaxValue, int.MaxValue), kFlags).Height + extraTabHeight;
			}

			m_tabsPanel.Height = TabsContainer.Height - 5;
			TabsContainer.Invalidate(true);
		}

		/// ------------------------------------------------------------------------------------
		private void SetContextMenus()
		{
			if (TabsContainer.ContextMenuStrip != null)
				TabsContainer.ContextMenuStrip.Opening -= ContextMenuStrip_Opening;

			if (TMAdapter != null)
				TMAdapter.SetContextMenuForControl(TabsContainer, "cmnuSearchResultTabGroup");

			if (TabsContainer.ContextMenuStrip != null)
				TabsContainer.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
		}

		#region Message mediator message handler and update handler methods
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

			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the Class Display Behavior has been changed by the user.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnClassDisplayBehaviorChanged(object args)
		{
			foreach (var tab in Tabs)
			{
				if (tab.SearchQuery.Pattern == null)
					continue;

				// PaApp.ShowClassNames has not been set yet to the new value
				// in OptionsDialog.FindPhonesTab>>SaveFindPhonesTabSettings()

				var replacedText = m_rsltVwMngr.Project.SearchClasses.ModifyPatternText(tab.Text);
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
					tab.Font = App.PhoneticFont;
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
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			App.RemoveMediatorColleague(this);
		}

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

			var rc = ClientRectangle;
			rc.Y = TabsContainer.Bottom;
			rc.Height -= TabsContainer.Height;
			var clr = (IsCurrent ? SystemColors.ControlText : SystemColors.GrayText);

			using (var fnt = FontHelper.MakeFont(FontHelper.UIFont, FontStyle.Bold))
			{
				var text = App.GetString("SearchResultTabGroup.EmptyTabInfoText",
					"Define a search pattern above and click Show Results.");
				
				TextRenderer.DrawText(e.Graphics, text, fnt, rc, clr, kFlags);
			}

			App.DrawWatermarkImage("kimidSearchWatermark", e.Graphics, ClientRectangle);
		}

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
			if (m_tabsPanel.Left > 0)
				m_tabsPanel.Left = 0;

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
			m_tabsPanel.Controls.Add(tab);
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
			if (tab.GetDoesHaveEmptyText() && resultView != null && resultView.SearchQuery != null)
				tab.Text = resultView.SearchQuery.ToString();

			tab.AdjustWidth();
			tab.OwningTabGroup = this;

			if (resultView != null)
			{
				tab.ResultView = resultView;
				tab.ResultView.Size = new Size(Width, Height - TabsContainer.Height);
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
			m_tabsPanel.SuspendLayout();
			m_tabsPanel.Width = Tabs.Sum(tab => tab.Width);
			RefreshScrollButtonPanel();
			m_tabsPanel.ResumeLayout(true);
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
				int i = tabGroup.m_tabsPanel.Controls.IndexOf(tab);

				if (i - 1 >= 0)
					newTabInSendingGroup = tabGroup.m_tabsPanel.Controls[i - 1] as SearchResultTab;
				else if (i + 1 < tabGroup.m_tabsPanel.Controls.Count)
					newTabInSendingGroup = tabGroup.m_tabsPanel.Controls[i + 1] as SearchResultTab;

				if (newTabInSendingGroup != null)
					tabGroup.SelectTab(newTabInSendingGroup, true);
			}

			if (m_tabsPanel.Controls.Contains(tab))
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

				m_tabsPanel.Controls.Remove(tab);
				Tabs.Remove(tab);

				if (disposeOfTab)
					tab.Dispose();

				AdjustTabContainerWidth();
				RefreshScrollButtonPanel();

				// If removing the tab left a gap between the furthest right tab and the
				// button panel... and all or a portion of the furthest left tab are
				// scrolled out of view, then scroll right to close that gap.
				if (m_tabsPanel.Left < 0 && m_tabsPanel.Right < m_buttonPanel.Left)
				{
					int dx = (m_buttonPanel.Left - m_tabsPanel.Right);
					SlideTabs(m_tabsPanel.Left + dx);
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
			
			int availableWidth = TabsContainer.Width - m_buttonPanel.GetMinWidth();

			if (tab.Width > availableWidth)
				return;

			int maxRight = m_buttonPanel.Left;
			
			// Get the tab's left and right edge relative to the header panel.	
			int left = tab.Left + m_tabsPanel.Left;
			int right = left + tab.Width;

			// Check if it's already fully visible.
			if (left >= 0 && right < maxRight)
				return;

			// Slide the panel in the proper direction to make it visible.
			int dx = (left < 0 ? left : right - maxRight);
			SlideTabs(m_tabsPanel.Left - dx);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetIsTabsRightEdgeVisible(SearchResultTab tab)
		{
			var pt = tab.PointToScreen(new Point(tab.Width, 0));
			pt = TabsContainer.PointToClient(pt);
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
		void HandleHeaderBandPanelResize(object sender, EventArgs e)
		{
			RefreshScrollButtonPanel();
		}

		/// ------------------------------------------------------------------------------------
		private void RefreshScrollButtonPanel()
		{
			if (m_tabsPanel == null || TabsContainer == null || m_buttonPanel == null)
				return;

			m_buttonPanel.ScrollButtonsVisible =
				(m_tabsPanel.Width > (TabsContainer.Width - m_buttonPanel.GetMinWidth(false)));

			int maxButtonPanelWidth = TabsContainer.Width - m_tabsPanel.Width;
			m_buttonPanel.Width = Math.Max(maxButtonPanelWidth, m_buttonPanel.GetMinWidth());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Slides the container for all the tab controls to the specified new left value. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SlideTabs(int newLeft)
		{
			//float dx = Math.Abs(m_pnlTabs.Left - newLeft);
			//int pixelsPerIncrement = (int)Math.Ceiling(dx / 75f);
			int pixelsPerIncrement = 1;
			bool slidingLeft = (newLeft < m_tabsPanel.Left);
			while (m_tabsPanel.Left != newLeft)
			{
				if (slidingLeft)
				{
					if (m_tabsPanel.Left - pixelsPerIncrement < newLeft)
						m_tabsPanel.Left = newLeft;
					else
						m_tabsPanel.Left -= pixelsPerIncrement;
				}
				else
				{
					if (m_tabsPanel.Left + pixelsPerIncrement > newLeft)
						m_tabsPanel.Left = newLeft;
					else
						m_tabsPanel.Left += pixelsPerIncrement;
				}

				Utils.UpdateWindow(m_tabsPanel.Handle);
			}

			RefreshScrollButtonPanel();
		}

		#endregion

		#region Painting methods
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
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			m_dropIndicator.Visible = false;
		}

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
		internal void ShowCIEOptions(Control ctrl)
		{
			if (CurrentTab == null || CurrentTab.ResultView == null || CurrentTab.ResultView.Grid == null)
				return;

			if (CurrentTab.CieOptionsDropDown == null)
				CurrentTab.CieOptionsDropDown = new CIEOptionsDropDown(m_rsltVwMngr.Project);

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
		void m_cieOptionsDropDownContainer_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			// Make sure the drop-down completely goes away before proceeding.
			Application.DoEvents();

			if (CurrentTab.CieOptionsDropDown.OptionsChanged)
			{
				// Save the options as the new defaults for the project.
				m_rsltVwMngr.Project.SaveCIEOptions(CurrentTab.CieOptionsDropDown.CIEOptions);
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
