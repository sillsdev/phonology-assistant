using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Localization;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilUtils;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;

namespace SIL.Pa.UI.Views
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Form in which search patterns are defined and used for searching.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class SearchVw : UserControl, IxCoreColleague, ITabView, ISearchResultsViewHost
	{
		//private string PaApp.kOpenClassBracket = "\u2039";
		//private string PaApp.kCloseClassBracket = "\u203A";
		//private string PaApp.kOpenClassBracket = "\u2329";
		//private string PaApp.kCloseClassBracket = "\u232A";
		//private string PaApp.kOpenClassBracket = "\u3014";
		//private string PaApp.kCloseClassBracket = "\u3015";
		//private string PaApp.kOpenClassBracket = "\u3018";
		//private string PaApp.kCloseClassBracket = "\u3019";

		private const string kRecentlyUsedPatternFile = "RecentlyUsedPatterns.xml";

		private bool m_activeView;
		private ITMAdapter m_tmAdapter;
		private Point m_mouseDownLocationOnRecentlyUsedList = Point.Empty;
		private bool m_sidePanelDocked = true;
		private bool m_initialDock = true;
		private SlidingPanel m_slidingPanel;
		private SearchResultsViewManager m_rsltVwMngr;
		private readonly SplitterPanel m_dockedSidePanel;
		private readonly Keys m_savePatternHotKey = Keys.None;

		#region Form construction
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchVw()
		{
			App.InitializeProgressBarForLoadingView(Properties.Resources.kstidSearchViewText, 6);
			InitializeComponent();
			Name = "SearchVw";
			App.IncProgressBar();

			hlblRecentPatterns.TextFormatFlags &= ~TextFormatFlags.HidePrefix;
			hlblSavedPatterns.TextFormatFlags &= ~TextFormatFlags.HidePrefix;

			tvSavedPatterns.SetCutCopyPasteButtons(btnCategoryCut, btnCategoryCopy, btnCategoryPaste);
			ptrnTextBox.OwningView = this;
			LoadToolbarAndContextMenus();

			lblCurrPattern.Text = Utils.ConvertLiteralNewLines(lblCurrPattern.Text);
			App.IncProgressBar();

			SetToolTips();
			SetupSidePanelContents();
			App.IncProgressBar();
			SetupSlidingPanel();
			App.IncProgressBar();
			OnPaFontsChanged(null);
			App.IncProgressBar();

			m_dockedSidePanel = (m_slidingPanel.SlideFromLeft ? splitOuter.Panel1 : splitOuter.Panel2);

			LoadSettings();
			App.IncProgressBar();
			App.UninitializeProgressBar();

			base.DoubleBuffered = true;
			ReflectionHelper.SetProperty(splitOuter, "DoubleBuffered", true);
			ReflectionHelper.SetProperty(splitSideBarInner, "DoubleBuffered", true);
			ReflectionHelper.SetProperty(splitSideBarOuter, "DoubleBuffered", true);
			ReflectionHelper.SetProperty(splitResults, "DoubleBuffered", true);
			
			ptrnTextBox.SearchOptionsDropDown.lnkHelp.Click += SearchDropDownHelpLink_Click;
			Disposed += ViewDisposed;

			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("tbbSavePatternOnMenu");
			if (itemProps != null)
				m_savePatternHotKey = itemProps.ShortcutKey;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void ViewDisposed(object sender, EventArgs e)
		{
			Disposed -= ViewDisposed;

			if (ptrnBldrComponent != null && !ptrnBldrComponent.IsDisposed)
				ptrnBldrComponent.Dispose();
			
			if (ptrnTextBox != null && !ptrnTextBox.IsDisposed)
				ptrnTextBox.Dispose();
	
			if (tvSavedPatterns != null && !tvSavedPatterns.IsDisposed)
				tvSavedPatterns.Dispose();
			
			if (m_rsltVwMngr != null)
				m_rsltVwMngr.Dispose();

			if (splitOuter != null && !splitOuter.IsDisposed)
				splitOuter.Dispose();
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// There's a problem with Ctrl+S (save search patter) getting recognized when there
		/// is a search result word list showing. Therefore, we trap it at a lower level.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			// Make sure the user pressed the hotkey for saving a pattern.
			if (msg.Msg == 0x100 && keyData == m_savePatternHotKey)
			{
				// Make sure the button is enabled.
				TMItemProperties itemProps = m_tmAdapter.GetItemProperties("tbbSavePattern");
				if (itemProps != null && itemProps.Enabled)
				{
					// Make sure the user isn't in the middle of editing a saved pattern's name.
					if (tvSavedPatterns.SelectedNode == null || !tvSavedPatterns.SelectedNode.IsEditing)
					{
						App.MsgMediator.SendMessage("SavePattern", null);
						return true;
					}
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create an empty tab after the view has been shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewOpened(object args)
		{
			if (args == this)
			{
				// Start the view with one empty tab.
				m_rsltVwMngr.CreateEmptyTab();

				OnUpdateRemovePattern(null);
				OnUpdateClearRecentPatternList(null);
				tvSavedPatterns.UpdateButtons();
				ptrnTextBox.TextBox.Focus();
				ptrnTextBox.TextBox.SelectAll();
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadToolbarAndContextMenus()
		{
			if (m_tmAdapter != null)
			{
				App.UnPrepareAdapterForLocalizationSupport(m_tmAdapter);
				m_tmAdapter.Dispose();
			}

			m_tmAdapter = AdapterHelper.CreateTMAdapter();

			if (m_rsltVwMngr != null)
				m_rsltVwMngr.TMAdapter = m_tmAdapter;
			else
				m_rsltVwMngr = new SearchResultsViewManager(this, m_tmAdapter, splitResults, rtfRecVw);

			if (m_tmAdapter != null)
			{
				App.PrepareAdapterForLocalizationSupport(m_tmAdapter);
				m_tmAdapter.LoadControlContainerItem += m_tmAdapter_LoadControlContainerItem;

				string[] defs = new string[1];
				defs[0] = Path.Combine(App.ConfigFolder, "SearchTMDefinition.xml");
				m_tmAdapter.Initialize(this, App.MsgMediator, App.ApplicationRegKeyPath, defs);
				m_tmAdapter.AllowUpdates = true;
			}

			m_tmAdapter.SetContextMenuForControl(tvSavedPatterns, "cmnuSavedPatternList");
			m_tmAdapter.SetContextMenuForControl(lstRecentPatterns, "cmnuRecentPatternsList");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Control m_tmAdapter_LoadControlContainerItem(string name)
		{
			if (name == "tbbSearchOptionsDropDown")
				return ptrnTextBox.SearchOptionsDropDown;
			
			if (name == "tbbAdjustPlaybackSpeed")
				return m_rsltVwMngr.PlaybackSpeedAdjuster;

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupSidePanelContents()
		{
			ptrnBldrComponent.ConPickerClickedHandler = HandleVowConClicked;
			ptrnBldrComponent.ConPickerDragHandler = HandleItemDrag;
			ptrnBldrComponent.VowPickerClickedHandler = HandleVowConClicked;
			ptrnBldrComponent.VowPickerDragHandler = HandleItemDrag;
			ptrnBldrComponent.OtherCharDragHandler = HandleItemDrag;
			ptrnBldrComponent.OtherCharPickedHandler = HandleCharExplorerCharPicked;
			ptrnBldrComponent.FeatureListsItemDragHandler = HandleItemDrag;
			ptrnBldrComponent.FeatureListsKeyPressHandler = HandleFeatureListKeyPress;
			ptrnBldrComponent.FeatureListDoubleClickHandler = HandleFeatureListCustomDoubleClick;
			ptrnBldrComponent.ClassListItemDragHandler = HandleItemDrag;
			ptrnBldrComponent.ClassListKeyPressHandler = HandleClassListKeyPress;
			ptrnBldrComponent.ClassListDoubleClickHandler = HandleClassListDoubleClick;
			ptrnBldrComponent.Initialize();

			tvSavedPatterns.Load();

			btnAutoHide.Left = btnDock.Left = (pnlSideBarCaption.Width - btnDock.Width - 6);
			btnAutoHide.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupSlidingPanel()
		{
			pnlSideBarCaption.Height = FontHelper.UIFont.Height + 7;
			pnlSideBarCaption.Font = FontHelper.UIFont;

			btnAutoHide.Top = ((pnlSideBarCaption.Height - btnAutoHide.Height) / 2) - 1;
			btnDock.Top = btnAutoHide.Top;

			m_slidingPanel = new SlidingPanel(this, splitSideBarOuter, pnlSliderPlaceholder, Name);
			LocalizationManager.LocalizeObject(m_slidingPanel.Tab, "SearchVw.UndockedSideBarTabText",
				"Patterns & Pattern Building", null, null, "Text on vertical tab when the side " +
				"bar is undocked in the search view.", "Views", LocalizationPriority.High);

			SuspendLayout();
			Controls.Add(m_slidingPanel);
			splitOuter.BringToFront();
			ResumeLayout(false);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the view's result view manager.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchResultsViewManager ResultViewManger
		{
			get { return m_rsltVwMngr; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not class names should be shown in search
		/// patterns. If false, then the class' members are shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool ShowClassNames
		{
			get { return (App.Project == null || App.Project.ShowClassNamesInSearchPatterns); }
		}
		
		#endregion

		#region ITabView Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ActiveView
		{
			get { return m_activeView; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetViewActive(bool makeActive, bool isDocked)
		{
			m_activeView = makeActive;

			if (m_activeView && isDocked && m_rsltVwMngr.CurrentViewsGrid != null &&
				m_rsltVwMngr.CurrentViewsGrid.Focused)
			{
				m_rsltVwMngr.CurrentViewsGrid.SetStatusBarText();
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Form OwningForm
		{
			get { return FindForm(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewClosing(object args)
		{
			if (args == this)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewUnDocking(object args)
		{
			if (args == this)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
			if (m_slidingPanel.SlideFromLeft)
				App.SettingsHandler.SaveSettingsValue(Name, "sidepaneldocked", !splitOuter.Panel1Collapsed);
			else
				App.SettingsHandler.SaveSettingsValue(Name, "sidepaneldocked", !splitOuter.Panel2Collapsed);

			tvSavedPatterns.SaveSettings();

			// Save the list of recently used queries.
			List<SearchQuery> recentList = new List<SearchQuery>();
			for (int i = 0; i < lstRecentPatterns.Items.Count; i++)
				recentList.Add(lstRecentPatterns.Items[i] as SearchQuery);

			string path = Path.Combine(App.DefaultProjectFolder, kRecentlyUsedPatternFile);
			if (recentList.Count > 0)
				XmlSerializationHelper.SerializeToFile(path, recentList);
			else
				File.Delete(path);

			ptrnBldrComponent.SaveSettings(Name);

			App.SettingsHandler.SaveSettingsValue(Name, "recordpanevisible", m_rsltVwMngr.RecordViewOn);

			float splitRatio = splitOuter.SplitterDistance / (float)splitOuter.Width;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio1", splitRatio);

			splitRatio = splitResults.SplitterDistance / (float)splitResults.Height;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio2", splitRatio);

			splitRatio = splitSideBarOuter.SplitterDistance / (float)splitSideBarOuter.Height;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio3", splitRatio);

			splitRatio = splitSideBarInner.SplitterDistance / (float)splitSideBarInner.Height;
			App.SettingsHandler.SaveSettingsValue(Name, "splitratio4", splitRatio);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewDocking(object args)
		{
			if (args == this && IsHandleCreated)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewDocked(object args)
		{
			if (args == this)
			{

				try
				{
					// These are in a try/catch because sometimes they might throw an exception
					// in rare cases. The exception has to do with a condition in the underlying
					// .Net framework that I haven't been able to make sense of. Anyway, if an
					// exception is thrown, no big deal, the splitter distances will just be set
					// to their default values.
					float splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio1", 0.25f);
					splitOuter.SplitterDistance = (int)(splitOuter.Width * splitRatio);

					splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio2", 0.8f);
					splitResults.SplitterDistance = (int)(splitResults.Height * splitRatio);

					splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio3", 0.33f);
					splitSideBarOuter.SplitterDistance = (int)(splitSideBarOuter.Height * splitRatio);

					splitRatio = App.SettingsHandler.GetFloatSettingsValue(Name, "splitratio4", 0.5f);
					splitSideBarInner.SplitterDistance = (int)(splitSideBarInner.Height * splitRatio);
				}
				catch { }

				// Don't need to load the tool bar or menus if this is the first time
				// the view was docked since that all gets done during construction.
				if (m_initialDock)
					m_initialDock = false;
				else
				{
					// The toolbar has to be recreated each time the view is removed from it's
					// (undocked) form and docked back into the main form. The reason has to
					// do with tooltips. They seem to form an attachment, somehow, with the
					// form that owns the controls the tooltip is extending. When that form
					// gets pulled out from under the tooltips, sometimes the program will crash.
					LoadToolbarAndContextMenus();
					SetToolTips();
				}

				if (m_rsltVwMngr.CurrentViewsGrid != null)
					m_rsltVwMngr.CurrentViewsGrid.SetStatusBarText();
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			if (args == this)
				SetToolTips();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetToolTips()
		{
			m_tooltip = new ToolTip(components);
			string tip = Properties.Resources.kstidSearchPatternTooltip;
			m_tooltip.SetToolTip(ptrnTextBox.TextBox, Utils.ConvertLiteralNewLines(tip));
			LocalizationManager.RefreshToolTips();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the view's toolbar/menu adapter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ITMAdapter TMAdapter
		{
			get { return m_tmAdapter; }
		}

		#endregion

		#region Loading/Saving settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			ptrnBldrComponent.LoadSettings(Name);

			bool sidePanelDocked = App.SettingsHandler.GetBoolSettingsValue(Name,
				"sidepaneldocked", true);

			if (sidePanelDocked)
				btnDock_Click(null, null);
			else
				btnAutoHide_Click(null, null);

			OnViewDocked(this);
			m_initialDock = true;
			m_slidingPanel.LoadSettings();

			m_rsltVwMngr.RecordViewOn = App.SettingsHandler.GetBoolSettingsValue(Name,
				"recordpanevisible", true);

			try
			{
				string path = Path.Combine(App.DefaultProjectFolder, kRecentlyUsedPatternFile);
				var recentList = XmlSerializationHelper.DeserializeFromFile<List<SearchQuery>>(path);
				if (recentList != null)
					lstRecentPatterns.Items.AddRange(recentList.ToArray());
			}
			catch
			{
				lstRecentPatterns.Items.Clear();
			}
		}

		#endregion

		#region Side Panel button click and update handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove all items from the recently used queries list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnClearRecentList_Click(object sender, EventArgs e)
		{
			lstRecentPatterns.Items.Clear();
			OnUpdateRemovePattern(null);
			OnUpdateClearRecentPatternList(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnRemoveFromRecentList_Click(object sender, EventArgs e)
		{
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties(
				"cmnuRemovePattern-FromRecentList");

			OnRemovePattern(itemProps);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCategoryCut_Click(object sender, EventArgs e)
		{
			App.MsgMediator.SendMessage("CutSavedPattern", btnCategoryCut);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCategoryCopy_Click(object sender, EventArgs e)
		{
			App.MsgMediator.SendMessage("CopySavedPattern", btnCategoryCopy);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCategoryPaste_Click(object sender, EventArgs e)
		{
			App.MsgMediator.SendMessage("PasteSavedPattern", btnCategoryPaste);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCategoryNew_Click(object sender, EventArgs e)
		{
			tvSavedPatterns.AddCategory(!m_sidePanelDocked ? m_slidingPanel : null, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Dock the side panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDock_Click(object sender, EventArgs e)
		{
			// Swap which buttons are visible in the side panel's caption area.
			btnAutoHide.Visible = true;
			btnDock.Visible = false;
			m_sidePanelDocked = true;

			// Uncollapse the panel in which the side panel will be docked.
			if (m_slidingPanel.SlideFromLeft)
				splitOuter.Panel1Collapsed = false;
			else
				splitOuter.Panel2Collapsed = false;

			// Let the sliding panel control handle the rest of the docking procedure.
			m_slidingPanel.DockControl(m_dockedSidePanel);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Undock the side panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnAutoHide_Click(object sender, EventArgs e)
		{
			// Swap which buttons are visible in the side panel's caption area.
			btnAutoHide.Visible = false;
			btnDock.Visible = true;
			m_sidePanelDocked = false;

			// Let the sliding panel control handle most of the undocking/hiding procedure.
			m_slidingPanel.UnDockControl(m_dockedSidePanel);

			// Collapse the panel in which the side panel was docked.
			if (m_slidingPanel.SlideFromLeft)
				splitOuter.Panel1Collapsed = true;
			else
				splitOuter.Panel2Collapsed = true;
		}

		#endregion

		#region Message handlers for clearing and saving patterns
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear out the text in the current search pattern as well as the current tab's
		/// results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnClearPattern(object args)
		{
			if (!m_activeView)
				return false;

			ptrnTextBox.Clear();

			if (m_rsltVwMngr.CurrentTabGroup != null && m_rsltVwMngr.CurrentTabGroup.CurrentTab != null)
				m_rsltVwMngr.CurrentTabGroup.CurrentTab.Clear();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear out the text in the current search pattern as well as the current tab's
		/// results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateClearPattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_activeView)
				return false;

			bool enable = (!ptrnTextBox.IsPatternEmpty || m_rsltVwMngr.CurrentViewsGrid != null);
			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}

			// Use this update opportunity to update the enabled state of the refresh button.
			btnRefresh.Enabled = (m_rsltVwMngr.CurrentViewsGrid != null &&
				m_rsltVwMngr.CurrentViewsGrid.AreResultsStale && ptrnTextBox.IsPatternFull);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSavePattern(object args)
		{
			if (!m_activeView)
				return false;

			SearchQuery query = ptrnTextBox.SearchQuery;

			// When Id is zero it means the query has never been saved before.
			// Therefore, just show the save as dialog.
			if (query.Id == 0)
			{
				if (!OnSavePatternAs(args))
					return false;

				ptrnTextBox.Clear();
				if (m_rsltVwMngr.CurrentTabGroup != null &&
					m_rsltVwMngr.CurrentTabGroup.CurrentTab != null)
					m_rsltVwMngr.CurrentTabGroup.CurrentTab.Clear();

				ptrnTextBox.SetSearchQuery(tvSavedPatterns.CurrentQuery);
				m_rsltVwMngr.PerformSearch(ptrnTextBox.SearchQuery,
					SearchResultLocation.CurrentTabGroup);

				return true;
			}

			//// At this point, we know we're dealing with a previously saved query. Therefore,
			//// we must determine whether or not to show the query save as dialog. Find the
			//// original query so we can compare its pattern with the current pattern.
			//SearchQuery origQuery = PaApp.Project.SearchQueryGroups.GetQueryForId(query.Id);

			//// Show the dialog when the original pattern is different from the current and
			//// the query has never been assigned a name. Otherwise, just save without prompting.
			//if ((origQuery == null || query.Pattern != origQuery.Pattern) &&
			//    string.IsNullOrEmpty(query.Name))
			//{
			//    ShowSavePatternDlg(false);
			//}
			//else
			//    tvSavedPatterns.SavePattern(query);

			tvSavedPatterns.SavePattern(query);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSavePatternAs(object args)
		{
			if (!m_activeView)
				return false;

			return ShowSavePatternDlg(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the query save as dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ShowSavePatternDlg(bool canChangeQuerysCategory)
		{
			if (ptrnTextBox.SearchQuery == null)
			{
				// This should never happen.
				System.Media.SystemSounds.Beep.Play();
				return false;
			}

			using (SaveSearchQueryDlg dlg = new SaveSearchQueryDlg(ptrnTextBox.SearchQuery,
				tvSavedPatterns, canChangeQuerysCategory))
			{
				string saveName = ptrnTextBox.SearchQuery.Name;

				if (dlg.ShowDialog(ptrnTextBox.FindForm()) == DialogResult.Cancel)
					ptrnTextBox.SearchQuery.Name = saveName;
				else
				{
					if (!string.IsNullOrEmpty(ptrnTextBox.SearchQuery.Name))
						m_rsltVwMngr.CurrentTabGroup.UpdateCurrentTabsQueryName(ptrnTextBox.SearchQuery.Name);

					return true;
				}
			}

			return false;
		}

		#endregion

		#region Misc. methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void SearchDropDownHelpLink_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic("hidSearchOptionsSearchView");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the current search result view to indicate it needs to be refreshed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ptrnTextBox_SearchOptionsChanged(object sender, EventArgs e)
		{
			ptrnTextBox_PatternTextChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the current search result view to indicate it needs to be refreshed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ptrnTextBox_PatternTextChanged(object sender, EventArgs e)
		{
			if (m_rsltVwMngr.CurrentViewsGrid != null && !ptrnTextBox.ClassDisplayBehaviorChanged)
				m_rsltVwMngr.CurrentViewsGrid.AreResultsStale = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ptrnTextBox_SizeChanged(object sender, EventArgs e)
		{
			if (pnlCurrPattern.Height != ptrnTextBox.Height + 11)
				pnlCurrPattern.Height = ptrnTextBox.Height + 11;

			if (ptrnTextBox.Top != 5)
				ptrnTextBox.Top = 5;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints a nice gradient background behind the current search pattern text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlCurrPattern_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rc = pnlCurrPattern.ClientRectangle;

			Color clrTop =
				ColorHelper.CalculateColor(SystemColors.Control, Color.White, 100);

			Color clrBottom =
				ColorHelper.CalculateColor(SystemColors.ControlDark, Color.White, 75);

			using (LinearGradientBrush br = new LinearGradientBrush(rc, clrTop, clrBottom, 90))
			{
				e.Graphics.FillRectangle(br, rc);
				e.Graphics.DrawLine(SystemPens.ControlDark, rc.X, rc.Bottom - 1,
					rc.Right - 1, rc.Bottom - 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure to center the refresh button vertically in it's owning panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlCurrPattern_Resize(object sender, EventArgs e)
		{
			int top = (pnlCurrPattern.Height - btnRefresh.Height) / 2;
			if (top != btnRefresh.Top)
				btnRefresh.Top = top;

			top = (pnlCurrPattern.Height - ptrnTextBox.Height) / 2;
			if (top != ptrnTextBox.Top)
				ptrnTextBox.Top = top;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the results in the current tab in the current tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			// This is just like clicking the "Show Results" button.
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("tbbShowResults");
			App.MsgMediator.SendMessage("ShowResults", itemProps);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordListCache PerformSearch(SearchQuery query, SearchResultLocation resultLocation)
		{
			return m_rsltVwMngr.PerformSearch(query, resultLocation);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the search image watermark when the result pane changes sizes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void splitResults_Panel1_SizeChanged(object sender, EventArgs e)
		{
			if (splitResults.Panel1.Controls.Count == 0)
				splitResults.Panel1.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the search image watermark.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void splitResults_Panel1_Paint(object sender, PaintEventArgs e)
		{
			if (splitResults.Panel1.Controls.Count == 0)
			{
				App.DrawWatermarkImage("kimidSearchWatermark", e.Graphics,
					splitResults.Panel1.ClientRectangle);
			}
		}

		#endregion

		#region Non DragDrop keyboard and mouse events for controls supplying items to the search pattern
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFeatureListCustomDoubleClick(object sender, string feature)
		{
			if (!string.IsNullOrEmpty(feature))
				ptrnTextBox.Insert(feature);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the user pressing Enter on a feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void HandleFeatureListKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter && sender is FeatureListView)
			{	
				string text = ((FeatureListView)sender).CurrentFormattedFeature;
				if (text != null)
					ptrnTextBox.Insert(text);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts phones into the search pattern when they're clicked on in the character
		/// explorer on the "Other" tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCharExplorerCharPicked(CharPicker picker, ToolStripButton item)
		{
			if (!string.IsNullOrEmpty(item.Text.Replace(App.kDottedCircle, string.Empty)))
				ptrnTextBox.Insert(item.Text.Replace(App.kDottedCircle, string.Empty));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts phones into the search pattern when they're clicked on from the vowel or
		/// consonant tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleVowConClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.ClickedItem.Text))
				ptrnTextBox.Insert(e.ClickedItem.Text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When user double-clicks a class name, then put user in insert mode to insert that
		/// class in the pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClassListDoubleClick(object sender, MouseEventArgs e)
		{
			ClassListView lv = ptrnBldrComponent.ClassListView;

			if (lv.SelectedItems.Count > 0)
			{
				ClassListViewItem item = lv.SelectedItems[0] as ClassListViewItem;
				if (item != null)
				{
					ptrnTextBox.Insert((item.Pattern == null || ShowClassNames ?
						App.kOpenClassBracket + item.Text + App.kCloseClassBracket : item.Pattern));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the user presses enter on a class, then put the user in the insert mode to
		/// insert that class in the pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleClassListKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
				HandleClassListDoubleClick(null, null);
		}

		#endregion

		#region Methods for dragging and dropping items to search pattern panels
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If we're not already in the insert mode, begin dragging something to insert into
		/// the search pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			string dragText = null;

			if (e.Item is string)
				dragText = e.Item as string;
			else if (e.Item is CharGridCell)
				dragText = ((CharGridCell)e.Item).Phone;
			else if (sender is FeatureListView)
				dragText = ((FeatureListView)sender).CurrentFormattedFeature;
			else if (e.Item is ClassListViewItem)
			{
				ClassListViewItem item = e.Item as ClassListViewItem;
				if (item != null)
				{
					dragText = (item.Pattern == null || ShowClassNames ?
						App.kOpenClassBracket + item.Text + App.kCloseClassBracket :
						item.Pattern);
				}
			}

			// At this point, any text we've got we use to construct a query since
			// the pattern text box is only a drop target for search query objects.
			// Then begin dragging.
			if (dragText != null)
			{
				SearchQuery query = new SearchQuery();
				query.Pattern = dragText.Replace(App.kDottedCircle, string.Empty);
				query.PatternOnly = true;
				DoDragDrop(query, DragDropEffects.Copy);
			}
		}

		#endregion

		#region Methods for clearing, saving and loading the current search pattern
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the node clicked on is not a find Phone category but a search
		/// pattern, then load the pattern into the current pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvSavedPatterns_DoubleClick(object sender, EventArgs e)
		{
			if (tvSavedPatterns.CurrentQuery != null)
			{
				if (m_slidingPanel.Visible)
					m_slidingPanel.Close(true);

				ptrnTextBox.SetSearchQuery(tvSavedPatterns.CurrentQuery);
				m_rsltVwMngr.PerformSearch(ptrnTextBox.SearchQuery,
					SearchResultLocation.CurrentTabGroup);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Pressing enter on the saved patterns tree causes the saved pattern to be loaded
		/// into the current pattern.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvSavedPatterns_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
				tvSavedPatterns_DoubleClick(null, null);
			}
		}

		#endregion

		#region Methods for working with the recently used queries list
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddQueryToRecentlyUsedList(SearchQuery query)
		{
			if (query == null || query.IsPatternRegExpression)
				return;

			// TODO: should we consider more than just the pattern (i.e. query options)?
			// If the query is already in the list then remove it.
			int i = lstRecentPatterns.FindStringExact(query.Pattern);
			if (i >= 0)
				lstRecentPatterns.Items.RemoveAt(i);

			// Add the query to the top of the list.
			lstRecentPatterns.Items.Insert(0, query.Clone());

			// If we've exceeded the number of queries to save in
			// the list then remove the last one.
			if (lstRecentPatterns.Items.Count > Settings.Default.MaximumAllowedRecentlyUsedQueries)
				lstRecentPatterns.Items.RemoveAt(lstRecentPatterns.Items.Count - 1);

			OnUpdateRemovePattern(null);

			// This should always have at least one item at this point, but just in case...
			if (lstRecentPatterns.Items.Count > 0)
				lstRecentPatterns.SelectedIndex = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure an item is selected when the list gets focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstRecentPatterns_Enter(object sender, EventArgs e)
		{
			if (lstRecentPatterns.SelectedIndex < 0 && lstRecentPatterns.Items.Count > 0)
				lstRecentPatterns.SelectedIndex = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstRecentPatterns_KeyDown(object sender, KeyEventArgs e)
		{
			OnUpdateRemovePattern(null);
			OnUpdateClearRecentPatternList(null);

			// Treat enter like double-clicking on the query.
			if (e.KeyCode == Keys.Enter)
			{
				lstRecentPatterns_DoubleClick(null, null);
				return;
			}

			if (e.KeyCode != Keys.Delete || lstRecentPatterns.SelectedItem == null)
				return;

			// Save the index of the item to be removed and remove it.
			int index = lstRecentPatterns.SelectedIndex;
			lstRecentPatterns.Items.Remove(lstRecentPatterns.SelectedItem);

			// If there are still items in the list, select the
			// one in the list following the one just removed.
			if (lstRecentPatterns.Items.Count > 0)
			{
				lstRecentPatterns.SelectedIndex = (index >= lstRecentPatterns.Items.Count ?
					lstRecentPatterns.Items.Count - 1 : index);
			}

			OnUpdateRemovePattern(null);
			OnUpdateClearRecentPatternList(null);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load up the clicked-on query and search using it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstRecentPatterns_DoubleClick(object sender, EventArgs e)
		{
			if (lstRecentPatterns.SelectedItem == null)
				return;

			if (m_slidingPanel.Visible)
				m_slidingPanel.Close(true);

			SearchQuery query = lstRecentPatterns.SelectedItem as SearchQuery;
			if (query != null)
			{
				ptrnTextBox.SetSearchQuery(query);
				m_rsltVwMngr.PerformSearch(ptrnTextBox.SearchQuery,
					SearchResultLocation.CurrentTabGroup);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the location where the mouse was clicked down and also select the item
		/// clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstRecentPatterns_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				m_mouseDownLocationOnRecentlyUsedList = e.Location;
		
			// Select the item just clicked on.
			int i = lstRecentPatterns.IndexFromPoint(e.Location);
			if (i >= 0 && i < lstRecentPatterns.Items.Count)
				lstRecentPatterns.SelectedIndex = i;

			OnUpdateRemovePattern(null);
			OnUpdateClearRecentPatternList(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear the location where the mouse was clicked down since that variable also acts
		/// like a flag.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstRecentPatterns_MouseUp(object sender, MouseEventArgs e)
		{
			m_mouseDownLocationOnRecentlyUsedList = Point.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determine whether or not to begin dragging a recently used query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstRecentPatterns_MouseMove(object sender, MouseEventArgs e)
		{
			// This will be empty when the mouse button is not down.
			if (m_mouseDownLocationOnRecentlyUsedList.IsEmpty || lstRecentPatterns.SelectedItem == null)
				return;

			// Begin draging a query when the mouse is held down
			// and has moved 4 or more pixels in any direction.
			int dx = Math.Abs(m_mouseDownLocationOnRecentlyUsedList.X - e.X);
			int dy = Math.Abs(m_mouseDownLocationOnRecentlyUsedList.Y - e.Y);
			if (dx >= 4 || dy >= 4)
			{
				m_mouseDownLocationOnRecentlyUsedList = Point.Empty;
				DoDragDrop(lstRecentPatterns.SelectedItem as SearchQuery,
					DragDropEffects.Move | DragDropEffects.Copy);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle a pattern being dragged over an empty results area.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void splitResults_Panel1_DragOver(object sender, DragEventArgs e)
		{
			SearchQuery query = e.Data.GetData(typeof(SearchQuery)) as SearchQuery;
			e.Effect = (query == null ?	DragDropEffects.None : DragDropEffects.Move); 
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle dropping a pattern on a results area.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void splitResults_Panel1_DragDrop(object sender, DragEventArgs e)
		{
			SearchQuery query = e.Data.GetData(typeof(SearchQuery)) as SearchQuery;
			if (query != null)
				m_rsltVwMngr.PerformSearch(query, SearchResultLocation.CurrentTabGroup);
		}
		
		#endregion
	
		#region ISearchResultsViewHost Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void BeforeSearchPerformed(SearchQuery query, WordListCache resultCache)
		{
			if (ptrnTextBox.SearchQuery != query)
				ptrnTextBox.SetSearchQuery(query);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AfterSearchPerformed(SearchQuery query, WordListCache resultCache)
		{
			AddQueryToRecentlyUsedList(query);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShouldMenuBeEnabled(string menuName)
		{
			if (menuName == "tbbShowResults")
				return ptrnTextBox.IsPatternFull;

			if (!menuName.StartsWith("cmnu"))
				return ptrnTextBox.IsPatternFull;

			if (menuName.EndsWith("-FromSavedList"))
				return (tvSavedPatterns.CurrentQuery != null);

			if (menuName.EndsWith("-FromRecentList"))
				return (lstRecentPatterns.SelectedItem is SearchQuery);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery GetQueryForMenu(string menuName)
		{
			if (!menuName.StartsWith("cmnu"))
				return ptrnTextBox.SearchQuery;

			if (menuName.EndsWith("-FromSavedList"))
				return tvSavedPatterns.CurrentQuery;

			if (menuName.EndsWith("-FromRecentList"))
				return (lstRecentPatterns.SelectedItem as SearchQuery);

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This gets called when all the tabs or tab groups have been closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void NotifyAllTabsClosed()
		{
			ptrnTextBox.Clear();
			ptrnTextBox.TextBox.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This gets called when the current tab has changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void NotifyCurrentTabChanged(SearchResultTab newTab)
		{
			if (newTab != null && m_rsltVwMngr.CurrentTabGroup != null &&
				m_rsltVwMngr.CurrentTabGroup.CurrentTab == newTab)
			{
				ptrnTextBox.SetSearchQuery(newTab.SearchQuery);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the find dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ShowFindDlg(PaWordListGrid grid)
		{
			if (!FindInfo.FindDlgIsOpen)
			{
				FindDlg findDlg = new FindDlg(grid);
				findDlg.Show();
			}
		}
		
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares the grid sent in args with the current result view grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCompareGrid(object args)
		{
			PaWordListGrid grid = args as PaWordListGrid;
			return (grid != null && m_rsltVwMngr.CurrentViewsGrid == grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method gets called when the font(s) get changed in the options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPaFontsChanged(object args)
		{
			rtfRecVw.UpdateFonts();
			ptrnBldrComponent.RefreshFonts();
			lblCurrPattern.Font = FontHelper.UIFont;
			ptrnTextBox.TextBox.Font = FontHelper.PhoneticFont;
			hlblRecentPatterns.Font = FontHelper.UIFont;
			hlblSavedPatterns.Font = FontHelper.UIFont;
			
			int fontsz = App.SettingsHandler.GetIntSettingsValue(Name, "recentpatternslistfontsize", 10);
			lstRecentPatterns.Font = new Font(FontHelper.PhoneticFont.FontFamily, fontsz);
			fontsz = App.SettingsHandler.GetIntSettingsValue(Name, "savedpatternslistfontsize", 10);
			tvSavedPatterns.Font = new Font(FontHelper.PhoneticFont.FontFamily, fontsz);

			pnlCurrPattern.Invalidate();
			m_slidingPanel.RefreshFonts();

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			ptrnBldrComponent.RefreshComponents();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCloseAllTabGroups(object args)
		{
			if (m_activeView)
				ptrnTextBox.Clear();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Occurs when a search query is dragged and dropped on one of the tab groups
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPatternDroppedOnTabGroup(object args)
		{
			SearchQuery query = args as SearchQuery;
			if (query != null)
			{
				if (m_slidingPanel.Visible)
					m_slidingPanel.Close(true);

				ptrnTextBox.SetSearchQuery(query);
				m_rsltVwMngr.PerformSearch(ptrnTextBox.SearchQuery,
					SearchResultLocation.CurrentTabGroup);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is for one of the saved patterns tree view context menu items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnCopyToCurrentPattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_activeView)
				return false;

			if (itemProps.Name.EndsWith("-FromSavedList"))
				ptrnTextBox.SetSearchQuery(tvSavedPatterns.CurrentQuery);
			else
				ptrnTextBox.SetSearchQuery(lstRecentPatterns.SelectedItem as SearchQuery);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This message is for one of the saved patterns tree view context menu items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool OnUpdateCopyToCurrentPattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_activeView)
				return false;

			if (itemProps.Name.EndsWith("-FromSavedList"))
				itemProps.Enabled = (tvSavedPatterns.CurrentQuery != null);
			else
				itemProps.Enabled = (lstRecentPatterns.SelectedItem is SearchQuery);

			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRemovePattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_activeView || !itemProps.Name.EndsWith("-FromRecentList"))
				return false;

			lstRecentPatterns_KeyDown(null, new KeyEventArgs(Keys.Delete));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnClearRecentPatternList(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_activeView || !itemProps.Name.EndsWith("-FromRecentList"))
				return false;

			btnClearRecentList_Click(null, null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRemovePattern(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !m_activeView)
				return false;

			bool enable = false;

			if (itemProps.Name.EndsWith("-FromSavedList"))
				enable = (tvSavedPatterns.SelectedNode != null);
			else if (itemProps.Name.EndsWith("-FromRecentList"))
			{
				// For some reason, referencing the SelectedItem property occasionally throws
				// and error and I have no clue why. Since I cannot find the problem, I'll
				// solve it this way for now.
				try
				{
					enable = (lstRecentPatterns.SelectedItem != null);
				}
				catch { }
			
				if (btnRemoveFromRecentList.Enabled != enable)
					btnRemoveFromRecentList.Enabled = enable;
			}
			else
				return false;

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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateClearRecentPatternList(object args)
		{
			if (!m_activeView)
				return false;

			bool enable = (lstRecentPatterns.Items.Count > 0);

			if (btnClearRecentList.Enabled != enable)
				btnClearRecentList.Enabled = enable;
			
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

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
		/// Display the RtfExportDlg form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsRTF(object args)
		{
			if (!m_activeView)
				return false;

			RtfExportDlg rtfExp = new RtfExportDlg(m_rsltVwMngr.CurrentViewsGrid);
			rtfExp.ShowDialog(this);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsRTF(object args)
		{
			return EnableItemWhenFocusedAndHaveCurrentGrid(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsHTML(object args)
		{
			if (!m_activeView)
				return false;

			string outputFileName = m_rsltVwMngr.HTMLExport();

			if (outputFileName == null)
				return false;

			if (File.Exists(outputFileName))
				LaunchHTMLDlg.PostExportProcess(FindForm(), outputFileName);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsHTML(object args)
		{
			return EnableItemWhenFocusedAndHaveCurrentGrid(args as TMItemProperties);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns false when the specified TM item properties are null or when this form
		/// is not the active view. Returns true otherwise. When returning true, the enabled
		/// state of the item properties is set to true when there is a current search result
		/// grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool EnableItemWhenFocusedAndHaveCurrentGrid(TMItemProperties itemProps)
		{
			if (!m_activeView || itemProps == null)
				return false;

			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Enabled = (m_rsltVwMngr.CurrentViewsGrid != null);

			return true;
		}

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
}