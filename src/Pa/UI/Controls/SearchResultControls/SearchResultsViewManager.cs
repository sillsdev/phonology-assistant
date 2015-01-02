using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public enum SearchResultLocation
	{
		/// <summary>Adds tab results to the current tab, even if the tab already contains results.</summary>
		CurrentTab,
		/// <summary>Adds tab results to a new tab created in the current tab group.</summary>
		CurrentTabGroup,
		/// <summary>Adds tab results to a new tab created in a new stacked tab group.</summary>
		NewStackedTabGroup,
		/// <summary>Adds tab results to a new tab created in a new side-by-side tab group.</summary>
		NewSideBySideTabGroup
	}

	#region ISearchResultsViewHost
	/// ----------------------------------------------------------------------------------------
	public interface ISearchResultsViewHost
	{
		void BeforeSearchPerformed(SearchQuery query, WordListCache resultCache);
		void AfterSearchPerformed(SearchQuery query, WordListCache resultCache);
		bool ShouldMenuBeEnabled(string menuName);
		SearchQuery GetQueryForMenu(string menuName);
		void NotifyAllTabsClosed();
		void NotifyCurrentTabChanged(SearchResultTab newTab);
		void ShowFindDlg(PaWordListGrid grid);
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates methods, properties and message handlers for managing a split container
	/// control that contains search result view tab groups and a record view.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SearchResultsViewManager : IxCoreColleague, IDisposable, IMessageFilter
	{
		private SortOptionsDropDown m_phoneticSortOptionsDropDown;
		private SearchResultTabPopup m_srchResultTabPopup;
		private float m_horzSplitterCount;
		private float m_vertSplitterCount;
		private bool m_ignoreTabGroupRemoval;
		private SplitterPanel m_resultsPanel;
		private bool m_recViewOn = true;
		private ITabView m_view;
		private ITMAdapter m_tmAdapter;
		private SplitContainer m_splitResults;
		private PlaybackSpeedAdjuster m_playbackSpeedAdjuster;
		private readonly ISearchResultsViewHost m_srchRsltVwHost;
		private readonly List<SearchResultView> m_searchResultViews = new List<SearchResultView>();
		private readonly Action<int> m_savePlaybackSpeedAction;
		private int m_playbackSpeed;

		/// ------------------------------------------------------------------------------------
		public SearchResultsViewManager(ITabView view, ITMAdapter tmAdapter,
			SplitContainer splitResults, IRecordView recView, int playbackSpeed,
			Action<int> savePlaybackSpeed)
		{
			m_view = view;
			m_srchRsltVwHost = view as ISearchResultsViewHost;
			Debug.Assert(m_srchRsltVwHost != null);
			m_tmAdapter = tmAdapter;
			m_splitResults = splitResults;
			m_resultsPanel = splitResults.Panel1;
			RecordView = recView;
			m_playbackSpeed = playbackSpeed;
			m_savePlaybackSpeedAction = savePlaybackSpeed;
			App.AddMediatorColleague(this);
			m_resultsPanel.ControlRemoved += HandleTabGroupRemoved;
			Application.AddMessageFilter(this);
		}

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			Application.RemoveMessageFilter(this);
			m_view = null;
			m_tmAdapter = null;
			m_splitResults = null;
			m_resultsPanel = null;
			CurrentTabGroup = null;
			App.RemoveMediatorColleague(this);

			if (m_srchResultTabPopup != null)
				m_srchResultTabPopup.Dispose();

			if (m_playbackSpeedAdjuster != null)
				m_playbackSpeedAdjuster.Dispose();
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		public IRecordView RecordView { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool RecordViewOn
		{
			get { return m_recViewOn; }
			set
			{
				if (m_recViewOn != value)
				{
					m_recViewOn = value;
					m_splitResults.Panel2Collapsed = !m_splitResults.Panel2Collapsed;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultTabGroup CurrentTabGroup { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the control hosted in the current result view tab on the current result
		/// view tab group. This should only be a word list grid or a
		/// SearchQueryValidationErrorControl.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control CurrentViewsControl
		{
			get
			{
				return (CurrentTabGroup == null ||
					CurrentTabGroup.CurrentTab == null ||
					CurrentTabGroup.CurrentTab.ResultView == null ||
					CurrentTabGroup.CurrentTab.ResultView.Controls.Count == 0 ? null :
					CurrentTabGroup.CurrentTab.ResultView.Controls[0]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the grid of the current result view tab on the current result view tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaWordListGrid CurrentViewsGrid
		{
			get { return (CurrentViewsControl as PaWordListGrid); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the view tabs manager's toolbar/menu adapter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return m_tmAdapter; }
			set
			{
				m_tmAdapter = value;
				foreach (var resultView in m_searchResultViews)
					resultView.TMAdapter = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the view manager contains any results and
		/// if so, does the current tab on the current tab group contain results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ContainsResults
		{
			get {return m_resultsPanel.Controls.Count > 0 && CurrentViewsGrid != null;}
		}

		#endregion

		#region Message mediator message handlers and update handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the logic for all methods OnUpdateEditFind(Next/Previous)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool HandleFindItemUpdate(TMItemProperties itemProps, bool enableAllow)
		{
			PaWordListGrid grid = CurrentViewsGrid;
			
			if (itemProps == null || !m_view.ActiveView)
				return false;

			bool enable = (enableAllow && App.Project != null &&
				grid != null && grid.RowCount > 0);

			if (itemProps.Enabled != enable)
			{
				itemProps.Enabled = enable;
				itemProps.Visible = true;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle OnUpdateEditFind.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFind(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle OnUpdateEditFindNext.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFindNext(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, FindInfo.CanFindAgain);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle OnUpdateEditFindPrevious.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFindPrevious(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, FindInfo.CanFindAgain);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownGroupByFieldParent(object args)
		{
			ToolBarPopupInfo itemProps = args as ToolBarPopupInfo;
			if (itemProps == null || !m_view.ActiveView || CurrentViewsGrid != null)
				return false;

			CurrentViewsGrid.BuildGroupByMenu(itemProps.Name, m_tmAdapter);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupByFieldParent(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!m_view.ActiveView || itemProps == null)
				return false;

			bool enable = (CurrentViewsGrid != null && CurrentViewsGrid.Cache != null &&
				CurrentViewsGrid.RowCount > 0 && !CurrentViewsGrid.Cache.IsCIEList);
			
			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}

            if (enable && itemProps.Name.StartsWith("mnu", StringComparison.Ordinal))
				CurrentViewsGrid.BuildGroupByMenu(itemProps.Name, App.TMAdapter);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnGroupBySortedField(object args)
		{
			if (!m_view.ActiveView)
				return false;

			if (CurrentViewsGrid != null)
			{
				PaWordListGrid grid = CurrentViewsGrid;

				if (grid.IsGroupedByField)
					grid.GroupByField = null;
				else if (grid.SortOptions.SortFields != null &&
					grid.SortOptions.SortFields.Count > 0)
				{
					grid.GroupByField = grid.SortOptions.SortFields[0].Field;

					if (Settings.Default.WordListCollapseOnGrouping)
						grid.ToggleGroupExpansion(false);
				}

				if (grid.CurrentCell != null && !grid.CurrentCell.Displayed)
					grid.ScrollRowToMiddleOfGrid(grid.CurrentCell.RowIndex);

				FindInfo.ResetStartSearchCell(true);
				FindInfo.CanFindAgain = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupBySortedField(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!m_view.ActiveView || itemProps == null)
				return false;

			bool enable = (CurrentViewsGrid != null && CurrentViewsGrid.Cache != null &&
				CurrentViewsGrid.RowCount > 1 && !CurrentViewsGrid.Cache.IsCIEList);
			
			bool check = (enable && CurrentViewsGrid.IsGroupedByField);

			if (itemProps.Checked != check || itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Checked = check;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnExpandAllGroups(object args)
		{
			if (!m_view.ActiveView)
				return false;

			if (CurrentViewsGrid != null)
				CurrentViewsGrid.ToggleGroupExpansion(true);
	
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExpandAllGroups(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!m_view.ActiveView || itemProps == null)
				return false;

			bool enable = (CurrentViewsGrid != null && (CurrentViewsGrid.IsGroupedByField ||
				(CurrentViewsGrid.Cache != null && CurrentViewsGrid.Cache.IsCIEList &&
				!CurrentViewsGrid.Cache.IsEmpty)));

			if (itemProps.Enabled != (enable && !CurrentViewsGrid.AllGroupsExpanded))
			{
				itemProps.Visible = true;
				itemProps.Enabled = (enable && !CurrentViewsGrid.AllGroupsExpanded);
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCollapseAllGroups(object args)
		{
			if (!m_view.ActiveView)
				return false;

			if (CurrentViewsGrid != null)
				CurrentViewsGrid.ToggleGroupExpansion(false);
	
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCollapseAllGroups(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!m_view.ActiveView || itemProps == null)
				return false;

			bool enable = (CurrentViewsGrid != null && (CurrentViewsGrid.IsGroupedByField ||
				(CurrentViewsGrid.Cache != null && CurrentViewsGrid.Cache.IsCIEList &&
				!CurrentViewsGrid.Cache.IsEmpty)));

			if (itemProps.Enabled != (enable && !CurrentViewsGrid.AllGroupsCollapsed))
			{
				itemProps.Visible = true;
				itemProps.Enabled = (enable && !CurrentViewsGrid.AllGroupsCollapsed);
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggles showing the record pane below the tab groups.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowRecordPane(object args)
		{
			if (!m_view.ActiveView)
				return false;

			RecordViewOn = !m_recViewOn;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowRecordPane(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			bool enable = true;

			// Check if the record view is in a split container (call it A) that is owned by
			// another split container (call it B). If so, then assume A is contained in
			// Panel2 of B and only enable the item when B is not collapsed.
			if (m_splitResults.Parent is SplitterPanel)
				enable = !((SplitContainer)m_splitResults.Parent.Parent).Panel2Collapsed;

			bool check = (m_recViewOn && itemProps.Enabled);
			if (itemProps.Checked != check || itemProps.Enabled != enable || !itemProps.Visible)
			{
				itemProps.Enabled = enable;
				itemProps.Checked = check;
				itemProps.Visible = true;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when a tab should be moved to a new side-by-side tab
		/// group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnReflectMoveToNewSideBySideTabGroup(object args)
		{
			if (!m_view.ActiveView)
				return false;

			return MoveTabToNewTabGroup(args as SearchResultTab,
				SearchResultLocation.NewSideBySideTabGroup);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when a tab should be moved to a new stacked tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnReflectMoveToNewStackedTabGroup(object args)
		{
			if (!m_view.ActiveView)
				return false;

			return MoveTabToNewTabGroup(args as SearchResultTab,
				SearchResultLocation.NewStackedTabGroup);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool MoveTabToNewTabGroup(SearchResultTab tab, SearchResultLocation resultLocation)
		{
			if (tab == null)
				return false;

			SearchResultTabGroup tabGroup = tab.OwningTabGroup;

			if (tabGroup != null)
				tabGroup.RemoveTab(tab, false);

			m_resultsPanel.SuspendLayout();
			tabGroup = CreateNewTabGroup(resultLocation);
			tabGroup.AddTab(tab);
			tabGroup.SelectTab(tab, true);
			m_resultsPanel.ResumeLayout(true);
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCloseAllTabGroups(object args)
		{
			if (m_view.ActiveView)
			{
				m_ignoreTabGroupRemoval = true;
				Control ctrl = m_resultsPanel.Controls[0];
				m_resultsPanel.Controls.RemoveAt(0);
				ctrl.Dispose();
				m_ignoreTabGroupRemoval = false;
				RecordView.UpdateRecord(null);
				CurrentTabGroup = null;
				m_horzSplitterCount = m_vertSplitterCount = 0;
				m_srchRsltVwHost.NotifyAllTabsClosed();
			}
			
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new tab in the current tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnNewTabInCurrentTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			CreateTab(SearchResultLocation.CurrentTabGroup);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new tab in a new stacked tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnNewTabInNewStackedTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			CreateTab(SearchResultLocation.NewStackedTabGroup);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not to enable the option to create a new stacked tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateNewTabInNewStackedTabGroup(object args)
		{
			return OnUpdateNewTabInNewSideBySideTabGroup(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new tab in a new side-by-side tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnNewTabInNewSideBySideTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			CreateTab(SearchResultLocation.NewSideBySideTabGroup);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not to enable the option to create a new side-by-side
		/// tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateNewTabInNewSideBySideTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			bool enable = (m_resultsPanel.Controls.Count > 0);
			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs a search and puts the results in the current tab group when the user
		/// presses enter when the focus is in the pattern text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnEnterPressedInSearchPatternTextBox(object args)
		{
			SearchQuery query = args as SearchQuery;
			if (query == null || !m_view.ActiveView)
				return false;

			PerformSearch(query, SearchResultLocation.CurrentTab);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs a search and puts the results in the current tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnShowResultsInCurrentTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			SearchQuery query = m_srchRsltVwHost.GetQueryForMenu(itemProps.Name);
			if (query != null)
				PerformSearch(query, SearchResultLocation.CurrentTabGroup);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowResultsInCurrentTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = m_srchRsltVwHost.ShouldMenuBeEnabled(itemProps.Name);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs a search and puts the results in a new stacked tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowResultsInNewStackedTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			SearchQuery query = m_srchRsltVwHost.GetQueryForMenu(itemProps.Name);
			if (query != null)
				PerformSearch(query, SearchResultLocation.NewStackedTabGroup);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowResultsInNewStackedTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			itemProps.Enabled = m_srchRsltVwHost.ShouldMenuBeEnabled(itemProps.Name) &&
				m_resultsPanel.Controls.Count > 0;

			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Performs a search and puts the results in a new side-by-side tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowResultsInNewSideBySideTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			SearchQuery query = m_srchRsltVwHost.GetQueryForMenu(itemProps.Name);
			if (query != null)
				PerformSearch(query, SearchResultLocation.NewSideBySideTabGroup);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowResultsInNewSideBySideTabGroup(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			itemProps.Enabled = m_srchRsltVwHost.ShouldMenuBeEnabled(itemProps.Name) &&
				m_resultsPanel.Controls.Count > 0;

			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView || ContainsResults)
				return false;

			if (itemProps.Enabled)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAdjustPlaybackSpeed(object args)
		{
			return OnUpdatePlayback(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			return OnUpdatePlayback(args);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when the current tab group changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void SearchResultTabGroupChanged(SearchResultTabGroup newGroup)
		{
			if (CurrentTabGroup != newGroup)
				CurrentTabGroup = newGroup;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Notifies those who care that the current tab has changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void CurrentSearchResultTabChanged(SearchResultTab tab)
		{
			m_srchRsltVwHost.NotifyCurrentTabChanged(tab);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure that when tab groups are removed a couple of other clean-up procedures
		/// are done.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTabGroupRemoved(object sender, ControlEventArgs e)
		{
			if (m_ignoreTabGroupRemoval)
				return;

			var tabGroup = e.Control as SearchResultTabGroup;
			var owningPanel = sender as SplitterPanel;
			if (tabGroup == null || owningPanel == null)
				return;

			m_splitResults.SuspendLayout();
			bool removedTabGroupWasCurrent = tabGroup.IsCurrent;
			Control siblingPaneToRelocate = null;

			// Get the splitter that owns the panel from which a tab group was just removed.
			var owningSplitContainer = owningPanel.GetContainerControl() as SplitContainer;

			// Determine whether or not the tab group was removed from the first or second panel.
			int paneOfRemovedGroup = (int)owningPanel.Tag;

			// If the tab group was not removed from the top level panel that is the parent for
			// all tab groups and their split containers, then remove the tab group's sibling
			// from it's split container and place it on the split container that's one level
			// up the parent chain of split containers.
			if (paneOfRemovedGroup > 0 && owningSplitContainer != null)
			{
				// Get the sibling tab group.
				siblingPaneToRelocate = (paneOfRemovedGroup == 2 ?
					owningSplitContainer.Panel1.Controls[0] :
					owningSplitContainer.Panel2.Controls[0]);

				// Determine the new owning split panel and remove from that panel the
				// split container that used to own the removed tab group. Then add to that
				// panel the removed tab group's sibling.
				m_ignoreTabGroupRemoval = true;
				var newOwningPanel = owningSplitContainer.Parent as SplitterPanel;
				if (newOwningPanel != null)
				{
					newOwningPanel.Controls.Remove(owningSplitContainer);
					newOwningPanel.Controls.Add(siblingPaneToRelocate);
				}

				m_ignoreTabGroupRemoval = false;

				if (owningSplitContainer.Orientation == Orientation.Horizontal)
					m_horzSplitterCount--;
				else
					m_vertSplitterCount--;

				owningSplitContainer.Dispose();
			}
			else
				m_horzSplitterCount = m_vertSplitterCount = 0;

			// Hide the record view when there are no more tab groups.
			if (m_resultsPanel.Controls.Count == 0)
			{
				CurrentTabGroup = null;
				RecordView.UpdateRecord(null);
				m_srchRsltVwHost.NotifyAllTabsClosed();
			}
			else if (removedTabGroupWasCurrent)
			{
				// When the removed tab group is the current one,
				// make sure a remaining group is made current.
				var newTabGroup = FindNewCurrentTabGroup(siblingPaneToRelocate);
				SearchResultTabGroupChanged(newTabGroup);
				App.MsgMediator.SendMessage("SearchResultTabGroupChanged", newTabGroup);
			}

			tabGroup.Dispose();
			m_splitResults.ResumeLayout();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Find a new current tab group from the control that was relocated (which may be a
		/// SearchResultTabGroup or it may be a SplitContainer) or, the only
		/// SearchResultTabGroup left showing (i.e. m_resultsPanel.Controls[0]).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SearchResultTabGroup FindNewCurrentTabGroup(Control relocatedControl)
		{
			if (relocatedControl is SearchResultTabGroup)
				return relocatedControl as SearchResultTabGroup;
			
			if (m_resultsPanel.Controls[0] is SearchResultTabGroup)
				return m_resultsPanel.Controls[0] as SearchResultTabGroup;
			
			if (relocatedControl != null)
			{
				var tmpSplit = relocatedControl as SplitContainer;
				if (tmpSplit != null && tmpSplit.Panel2.Controls.Count > 0)
					return tmpSplit.Panel2.Controls[0] as SearchResultTabGroup;
			}

			// We should never get this far.
			return null;
		}

		#region Methods for performing searches
		/// ------------------------------------------------------------------------------------
		protected bool OnShowResults(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			var query = m_srchRsltVwHost.GetQueryForMenu(itemProps.Name);
			if (query != null)
				PerformSearch(query, SearchResultLocation.CurrentTab);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowResults(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			bool enable = m_srchRsltVwHost.ShouldMenuBeEnabled(itemProps.Name);

			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public WordListCache PerformSearch(SearchQuery query, SearchResultLocation resultLocation)
		{
			if (query == null)
				return null;

			query = CheckQuery(query);
			m_srchRsltVwHost.BeforeSearchPerformed(query, null);
			App.InitializeProgressBar(App.kstidQuerySearchingMsg);
			var resultCache = App.Search(query);

			if (resultCache != null)
			{
				resultCache.SearchQuery = query.Clone();
				m_srchRsltVwHost.AfterSearchPerformed(query, resultCache);
				ShowResults(resultCache, resultLocation);
			}
			
			App.UninitializeProgressBar();
			return resultCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will check if the query's name is the same as one of the saved queries
		/// but it's pattern is different. If so, the assumption is the user edited the pattern
		/// after having viewed the results for a saved pattern. Editing a pattern in that
		/// case makes the pattern a different one from the saved one so we need to force
		/// that here. (cf. PA-1130)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SearchQuery CheckQuery(SearchQuery query)
		{
			if (!string.IsNullOrEmpty(query.Name) && App.Project.SearchQueryGroups
				.SelectMany(grp => grp.Queries)
				.Any(q => !string.IsNullOrEmpty(q.Name) && q.Name.Equals(query.Name, StringComparison.Ordinal) &&
					!q.Pattern.Equals(query.Pattern, StringComparison.Ordinal)))
			{
				var newQuery = query.Clone();
				newQuery.Name = "";//null
				newQuery.Pattern = query.Pattern;
				return newQuery;
			}

			return query;
		}

		/// ------------------------------------------------------------------------------------
		private void ShowResults(WordListCache resultCache, SearchResultLocation resultLocation)
		{
			// When this is true, it probably means there are no results showing.
			// But, since there is about to be, we'll need to show the record
			// view when m_rawRecViewOn is true.
			if (m_recViewOn && m_splitResults.Panel2Collapsed)
				m_splitResults.Panel2Collapsed = false;

			// When the results should be shown in the current tab group, then check if the
			// current tab is empty. If so, then use that tab instead of creating a new tab
			// in which to display the results.
			if (resultLocation == SearchResultLocation.CurrentTabGroup && CurrentTabGroup != null &&
				CurrentTabGroup.CurrentTab != null && CurrentTabGroup.CurrentTab.IsEmpty)
			{
				resultLocation = SearchResultLocation.CurrentTab;
			}

			if (resultLocation == SearchResultLocation.CurrentTab && ReuseExistingTab(resultCache))
				return;

			var resultView = new SearchResultView(m_view.GetType(), m_tmAdapter);
			resultView.Initialize(resultCache);
			CreateTab(resultLocation, resultView);
			m_searchResultViews.Add(resultView);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnSearchResultViewDestroying(object args)
		{
			var resultView = args as SearchResultView;
			if (resultView != null && m_searchResultViews.Contains(resultView))
				m_searchResultViews.Remove(resultView);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Try to reuse an exisiting tab's result view, just updating it using the specified
		/// word result cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ReuseExistingTab(WordListCache resultCache)
		{
			if (CurrentTabGroup != null && CurrentTabGroup.CurrentTab != null &&
				CurrentTabGroup.CurrentTab.ResultView != null)
			{
				CurrentTabGroup.CurrentTab.RefreshResultView(resultCache);
				CurrentTabGroup.AdjustTabContainerWidth();
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an empty tab to the current tab group. If there is no tab group, then a new
		/// tab group is created first.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CreateEmptyTab()
		{
			CreateTab(SearchResultLocation.CurrentTabGroup);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new tab in the specified tab group.
		/// Once the tab is created, it's selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CreateTab(SearchResultLocation resultLocation)
		{
			CreateTab(resultLocation, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new tab in the specified tab group. Once the tab is created,
		/// it's selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CreateTab(SearchResultLocation resultLocation, SearchResultView resultView)
		{
			SearchResultTab tab;
			m_resultsPanel.SuspendLayout();

			// Return the current tab in the current tab group if
			// it matches the specified location.
			if (CurrentTabGroup != null && CurrentTabGroup.CurrentTab != null &&
				resultLocation == SearchResultLocation.CurrentTab)
			{
				tab = CurrentTabGroup.CurrentTab;
				if (resultView != null)
				{
					tab.Text = null;
					CurrentTabGroup.InitializeTab(tab, resultView, true);
				}

				m_resultsPanel.ResumeLayout(true);

				// Must Select the Tab, so the Current Playback Grid for the tab is set to true.
				// This will ensure the sound file Playback will always work.
				CurrentTabGroup.SelectTab(tab, true);
				return;
			}

			var tabGroup = CurrentTabGroup;

			if (m_resultsPanel.Controls.Count == 0)
			{
				// No tab groups exist yet so just add one without any splitter.
				tabGroup = new SearchResultTabGroup(this);
				tabGroup.SuspendLayout();
				tabGroup.Size = new Size(m_resultsPanel.Width, m_resultsPanel.Height);
				tabGroup.Dock = DockStyle.Fill;
				m_resultsPanel.Controls.Add(tabGroup);
				m_resultsPanel.Tag = 0;
			}
			else if (resultLocation != SearchResultLocation.CurrentTab &&
				resultLocation != SearchResultLocation.CurrentTabGroup)
			{
				tabGroup = CreateNewTabGroup(resultLocation);
			}

			tab = (resultView == null ? tabGroup.AddTab() :
				tabGroup.AddTab(resultView));

			tabGroup.SelectTab(tab, true);
			tab.MouseEnter += tab_MouseEnter;
			tab.MouseLeave += tab_MouseLeave;

			// If the tab just added is a new, empty tab, selecting it will cause a chain
			// reaction caused by hiding the former selected tab's grid. Doing that forces
			// the .Net framework to look for the next visible control in line that can
			// be focused. Sometimes that means a grid on another tab will get focus and
			// ultimately cause that grid's tab to become selected, thus negating the fact
			// that we just got through setting the tab we wanted to be current. Therefore,
			// try again, when we get back from selecting our new tab and it's still not
			// selected.
			if (tabGroup != CurrentTabGroup || !tab.Selected)
				tabGroup.SelectTab(tab, true);

			tabGroup.ResumeLayout(false);
			m_resultsPanel.ResumeLayout();
			App.MsgMediator.SendMessage("SearchResultTabCreated", tab);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new stacked or side-by-side tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SearchResultTabGroup CreateNewTabGroup(SearchResultLocation resultLocation)
		{
			// At this point, we know the user wants to add the new search results to a
			// new tab group. Therefore, a split container is created for the new tab.
			m_resultsPanel.SuspendLayout();

			var split = new SplitContainer();
			split.SuspendLayout();
			split.SplitterWidth = 8;
			split.TabStop = false;
			split.Dock = DockStyle.Fill;
			split.Panel1.ControlRemoved += HandleTabGroupRemoved;
			split.Panel2.ControlRemoved += HandleTabGroupRemoved;
			split.Panel1.Tag = 1;
			split.Panel2.Tag = 2;

			// Determine whether the new tab group's split container
			// should be oriented horizontally or vertically.
			float newSplitDistance;
			if (resultLocation == SearchResultLocation.NewSideBySideTabGroup)
			{
				split.Orientation = Orientation.Vertical;
				m_vertSplitterCount++;
				newSplitDistance = m_resultsPanel.Width *
					(m_vertSplitterCount / (m_vertSplitterCount + 1f));
			}
			else
			{
				split.Orientation = Orientation.Horizontal;
				m_horzSplitterCount++;
				newSplitDistance = m_resultsPanel.Height *
					(m_horzSplitterCount / (m_horzSplitterCount + 1f));
			}

			// Create a new tab group and add it to the new split container.
			var tabGroup = new SearchResultTabGroup(this);
			tabGroup.SuspendLayout();
			tabGroup.Dock = DockStyle.Fill;
			tabGroup.Size = new Size(split.Panel2.Width, split.Panel2.Height);
			split.Panel2.Controls.Add(tabGroup);

			// Now, all tab groups that previously existed before creating the new one need
			// to be removed from their container and placed in the left or top pane of the
			// new split container. To do this, just remove the only control (which may well
			// be a bunch of nested split containers) that exists in the top most panel for
			// all tab groups and place it in the left or top pane of the new split container.
			// The new split container is then added to the top most panel for all tab groups.
			// In other words, every time a new split container is added for a tab group, it
			// becomes the wrapper for all subsequent tab group split containers. Whew!
			m_ignoreTabGroupRemoval = true;
			var ctrl = m_resultsPanel.Controls[0];
			m_resultsPanel.Controls.Remove(ctrl);

			// Set these values now, because, even though they're docked, we're still
			// in a suspended layout mode so setting the splitter before resuming layout
			// causes the splitter distance to be set on the undocked size of the control.
			// But, this problem can be remedied by first setting the sizes.
			split.Size = new Size(m_resultsPanel.Width, m_resultsPanel.Height);
			try { split.SplitterDistance = (int)newSplitDistance; }
			catch { }
			ctrl.Size = new Size(split.Panel1.Width, split.Panel1.Height);
			
			split.Panel1.Controls.Add(ctrl);
			m_resultsPanel.Controls.Add(split);
			
			tabGroup.ResumeLayout(false);
			split.ResumeLayout(false);
			m_resultsPanel.ResumeLayout();
			
			m_ignoreTabGroupRemoval = false;
			
			App.MsgMediator.SendMessage("SearchResultTabGroupCreated", tabGroup);
			
			return tabGroup;
		}

		#endregion

		#region Methods for showing and hiding the tab information popup.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show information about the tab's search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void tab_MouseEnter(object sender, EventArgs e)
		{
			var tab = sender as SearchResultTab;

			if (tab == null)
				return;

			var frm = tab.FindForm();
			if (frm != null && !frm.ContainsFocus)
				return;
			
			if (m_srchResultTabPopup == null)
				m_srchResultTabPopup = new SearchResultTabPopup();

			m_srchResultTabPopup.Show(tab);
		}

		/// ------------------------------------------------------------------------------------
		void tab_MouseLeave(object sender, EventArgs e)
		{
			if (m_srchResultTabPopup != null)
				m_srchResultTabPopup.HidePopup();
		}

		#endregion

		#region Phonetic Sort methods
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownSearchResultPhoneticSort(object args)
		{
			var itemProps = args as ToolBarPopupInfo;
			if (!m_view.ActiveView || CurrentViewsGrid == null || itemProps == null)
				return false;

			m_phoneticSortOptionsDropDown =
				new SortOptionsDropDown(CurrentViewsGrid.SortOptions, true);

			m_phoneticSortOptionsDropDown.SortOptionsChanged += HandlePhoneticSortOptionsChanged;
			itemProps.Control = m_phoneticSortOptionsDropDown;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedSearchResultPhoneticSort(object args)
		{
			if (!m_view.ActiveView)
				return false;

			m_phoneticSortOptionsDropDown.Dispose();
			m_phoneticSortOptionsDropDown = null;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSearchResultPhoneticSort(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			bool enable = (CurrentViewsGrid != null);
			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when sort options change.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneticSortOptionsChanged(SortOptions sortOptions)
		{
			if (CurrentViewsGrid != null)
			{
				CurrentViewsGrid.SortOptions = sortOptions;
				RecordView.UpdateRecord(CurrentViewsGrid.GetRecord());
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is received when a word list grid has just looked through its cache
		/// to find minimal pairs and it couldn't find any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void OnNoCIEResultsShowing(object args)
		{
			if (CurrentViewsGrid != null && CurrentViewsGrid == args)
				RecordView.UpdateRecord(null);
		}

		#region Playback related methods
		/// ------------------------------------------------------------------------------------
		public PlaybackSpeedAdjuster PlaybackSpeedAdjuster
		{
			get
			{
				if (m_playbackSpeedAdjuster == null)
				{
					m_playbackSpeedAdjuster = new PlaybackSpeedAdjuster();
					m_playbackSpeedAdjuster.lnkPlay.Click += HandlePlaybackSpeedAdjusterPlayClick;
					m_playbackSpeedAdjuster.Disposed += m_playbackSpeedAdjuster_Disposed;
				}
				
				return m_playbackSpeedAdjuster;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The only time this will be disposed before the program terminates is when the
		/// view is redocked after being undocked. That is because the toolbar/menu adapter
		/// is disposed and recreated when the view is being redocked. And when the TMAdapter
		/// is disposed, so are the custom controls it hosts in drop-downs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_playbackSpeedAdjuster_Disposed(object sender, EventArgs e)
		{
			m_playbackSpeedAdjuster.Disposed -= m_playbackSpeedAdjuster_Disposed;
			m_playbackSpeedAdjuster.lnkPlay.Click -= HandlePlaybackSpeedAdjusterPlayClick;
			m_playbackSpeedAdjuster = null;
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackSpeedAdjusterPlayClick(object sender, EventArgs e)
		{
			m_tmAdapter.HideBarItemsPopup("tbbAdjustPlaybackSpeedParent");
			m_tmAdapter.HideBarItemsPopup("tbbPlayback");
			CurrentViewsGrid.OnPlayback(null);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownAdjustPlaybackSpeed(object args)
		{
			if (!m_view.ActiveView || CurrentViewsGrid == null ||
				CurrentViewsGrid.Cache == null)
			{
				return false;
			}

			m_playbackSpeedAdjuster.PlaybackSpeed = m_playbackSpeed;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedAdjustPlaybackSpeed(object args)
		{
			if (!m_view.ActiveView)
				return false;

			m_playbackSpeed = m_playbackSpeedAdjuster.PlaybackSpeed;
			m_savePlaybackSpeedAction(m_playbackSpeed);
			return true;
		}

		#endregion

		#region CIE (i.e. minimal pair) methods
		/// ------------------------------------------------------------------------------------
		protected bool OnShowCIEResults(object args)
		{
			if (!m_view.ActiveView || CurrentViewsGrid == null || CurrentViewsGrid.Cache == null)
				return false;

			CurrentTabGroup.CurrentTab.ToggleCIEView();
			FindInfo.ResetStartSearchCell(true);
			FindInfo.CanFindAgain = true;

			if (CurrentViewsGrid.Cache.IsCIEList && !CurrentViewsGrid.Cache.IsEmpty &&
				Settings.Default.WordListCollapseOnMinimalPairs)
			{
				CurrentViewsGrid.ToggleGroupExpansion(false);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCIEResults(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !m_view.ActiveView)
				return false;

			bool enable = (CurrentViewsGrid != null && CurrentViewsGrid.Cache != null &&
				(CurrentViewsGrid.RowCount > 2 || CurrentViewsGrid.Cache.IsCIEList) &&
				!CurrentViewsGrid.IsGroupedByField);

			bool check = (CurrentViewsGrid != null && CurrentViewsGrid.Cache != null &&
				CurrentViewsGrid.Cache.IsCIEList);

			if (itemProps.Enabled != enable || itemProps.Checked != check)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Checked = check;
				itemProps.Update = true;
			}

			return true;
		}

		#endregion

		#region Export Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to export the manager's current grid contents to HTML and returns the
		/// html file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HTMLExport()
		{
			var fmt = LocalizationManager.GetString("Views.WordLists.SearchResults.Export.DefaultHtmlExportFileAffix",
				"{0}-{1}SearchResults.html");

			return Export(fmt, App.kstidFileTypeHTML, "html",
				Settings.Default.OpenHtmlSearchResultAfterExport, SearchResultExporter.ToHtml);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to export the manager's current grid contents to Word XML and returns the
		/// Word XML document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string WordXmlExport()
		{
			var fmt = LocalizationManager.GetString("Views.WordLists.SearchResults.Export.DefaultWordXmlExportFileAffix",
				"{0}-{1}SearchResults-(Word).xml");

			return Export(fmt, App.kstidFileTypeWordXml, "xml",
				Settings.Default.OpenWordXmlSearchResultAfterExport, SearchResultExporter.ToWordXml);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to export the manager's current grid contents to an XLingPaper XML file
		/// and returns the path to the file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string XLingPaperExport()
		{
			var fmt = LocalizationManager.GetString("Views.WordLists.SearchResults.Export.DefaultXLingPaperExportFileAffix",
				"{0}-{1}SearchResults-(XLingPaper).xml");

			return Export(fmt, App.kstidFileTypeXLingPaper, "xml",
				Settings.Default.OpenXLingPaperSearchResultAfterExport,
				SearchResultExporter.ToXLingPaper);
		}

		/// ------------------------------------------------------------------------------------
		private string Export(string fmtFileName, string fileTypeFilter, string defaultFileType,
			bool openAfterExport, Func<PaProject, string, PaWordListGrid, bool, bool> exportAction)
		{
			var grid = CurrentViewsGrid;

			if (grid == null)
				return null;

			var queryName = (string.IsNullOrEmpty(grid.Cache.SearchQuery.Name) ?
				string.Empty : grid.Cache.SearchQuery.Name);

			// The query name may just be the pattern and in that case, we won't use it as
			// part of the default output file name. But if all characters in the name
			// are valid, then it will be used as part of the default file name.
			if (Path.GetInvalidFileNameChars().Any(invalidChar => queryName.Contains(invalidChar.ToString())))
				queryName = string.Empty;

			var defaultFileName = string.Format(fmtFileName, App.Project.LanguageName, queryName);

			var fileTypes = fileTypeFilter + "|" + App.kstidFileTypeAllFiles;

			int filterIndex = 0;
			var outputFileName = App.SaveFileDialog(defaultFileType, fileTypes, ref filterIndex,
				App.kstidSaveFileDialogGenericCaption, defaultFileName, App.Project.Folder);

			if (string.IsNullOrEmpty(outputFileName))
				return null;

			exportAction(App.Project, outputFileName, grid, openAfterExport);
			return outputFileName;
		}

		#endregion

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return new IxCoreColleague[] { this };
		}

		#endregion

		#region IMessageFilter Members
		/// ------------------------------------------------------------------------------------
		public bool PreFilterMessage(ref Message m)
		{
			// Check for WM_KEYDOWN
			if (m.Msg != 0x100)
				return false;

			if ((int)m.WParam == (int)Keys.M && (Control.ModifierKeys & Keys.Control) > 0 &&
				(Control.ModifierKeys & Keys.Alt) > 0 && CurrentTabGroup != null &&
				CurrentTabGroup.CurrentTab != null)
			{
				CurrentTabGroup.CurrentTab.ShowCIEOptions();
				return true;
			}

			return false;
		}

		#endregion
	}
}
