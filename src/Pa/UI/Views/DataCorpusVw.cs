using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using Palaso.IO;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;
using SIL.Pa.UI.Dialogs;

namespace SIL.Pa.UI.Views
{
	/// ----------------------------------------------------------------------------------------
	public partial class DataCorpusVw : ViewBase, ITabView
	{
		private WordListCache m_cache;
		private ITMAdapter m_tmAdapter;
		private SortOptionsDropDown m_phoneticSortOptionsDropDown;
		private bool m_rawRecViewOn = true;
		private bool m_activeView;
		private PlaybackSpeedAdjuster m_playbackSpeedAdjuster;
		private bool m_initialDock = true;

		/// ------------------------------------------------------------------------------------
		public DataCorpusVw(PaProject project) : base(project)
		{
			InitializeComponent();
			Name = "DataCorpusVw";

			if (App.DesignMode)
				return;

			Utils.WaitCursors(true);
			LoadToolbar();
			LoadWindow();
			base.DoubleBuffered = true;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadToolbar()
		{
			if (m_tmAdapter != null)
				m_tmAdapter.Dispose();

			m_tmAdapter = AdapterHelper.CreateTMAdapter();

			if (WordListGrid != null)
				WordListGrid.TMAdapter = m_tmAdapter;

			if (m_tmAdapter != null)
			{
				m_tmAdapter.LoadControlContainerItem += m_tmAdapter_LoadControlContainerItem;
				var defs = new[] { FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
					"DataCorpusTMDefinition.xml") };
				
				m_tmAdapter.Initialize(this, App.MsgMediator, App.ApplicationRegKeyPath, defs);
				m_tmAdapter.AllowUpdates = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Give the toolbar/menu adapter the playback speed adjuster control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Control m_tmAdapter_LoadControlContainerItem(string name)
		{
			if (name == "tbbAdjustPlaybackSpeed")
			{
				if (m_playbackSpeedAdjuster == null)
				{
					m_playbackSpeedAdjuster = new PlaybackSpeedAdjuster();
					m_playbackSpeedAdjuster.lnkPlay.Click += HandlePlaybackSpeedAdjusterPlayClick;
					m_playbackSpeedAdjuster.Disposed += m_playbackSpeedAdjuster_Disposed;
				}
				
				return m_playbackSpeedAdjuster;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		private void m_playbackSpeedAdjuster_Disposed(object sender, EventArgs e)
		{
			m_playbackSpeedAdjuster.lnkPlay.Click -= HandlePlaybackSpeedAdjusterPlayClick;
			m_playbackSpeedAdjuster.Disposed -= m_playbackSpeedAdjuster_Disposed;
			m_playbackSpeedAdjuster = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a wordEntry list grid with the specified cache and adds it to the form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Initialize(WordListCache cache)
		{
			m_cache = cache;

			if (WordListGrid != null)
				WordListGrid.Cache = m_cache;
			else
			{
				WordListGrid = new PaWordListGrid(cache, GetType(), false);
				WordListGrid.BorderStyle = BorderStyle.None;
				WordListGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
				WordListGrid.TMAdapter = m_tmAdapter;

				// Even thought the grid is docked, setting it's size here prevents the user
				// from seeing that split second during which time the grid goes from it's
				// small, default size to its docked size.
				WordListGrid.Size = new Size(splitOuter.Panel1.Width, splitOuter.Panel1.Height);

				WordListGrid.Name = Name + "Grid";
				WordListGrid.LoadSettings();
				WordListGrid.RowEnter += m_grid_RowEnter;
				WordListGrid.Visible = false;
				pnlGrid.Controls.Add(WordListGrid);
				WordListGrid.Visible = true;
				WordListGrid.TabIndex = 0;
				WordListGrid.Focus();
				WordListGrid.SortOptions = Project.DataCorpusVwSortOptions;
				WordListGrid.IsCurrentPlaybackGrid = true;
				WordListGrid.UseWaitCursor = false;
				WordListGrid.Cursor = Cursors.Default;
			}

			// This will enforce an update of the record pane.
			_recView.UpdateRecord(WordListGrid.GetRecord(), true);
		}

		#region Method for loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when the desire is to build a word list that is not a list of find Phone
		/// search results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadWindow()
		{
			var cache = new WordListCache();
			foreach (var entry in Project.WordCache)
				cache.Add(entry);

			Initialize(cache);
		}

		#endregion

		#region ITabView Members
		/// ------------------------------------------------------------------------------------
		public bool ActiveView
		{
			get { return m_activeView; }
		}

		/// ------------------------------------------------------------------------------------
		public void SetViewActive(bool makeActive, bool isDocked)
		{
			m_activeView = makeActive;

			if (makeActive)
			{
				FindInfo.Grid = WordListGrid;

				if (isDocked && WordListGrid != null)
				{
					WordListGrid.SetStatusBarText();
					WordListGrid.Focus();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public Form OwningForm
		{
			get { return FindForm(); }
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewUnDocking(object args)
		{
			if (args == this)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
			WordListGrid.SaveSettings();
			Settings.Default.DataCorpusVwSplitRatio = splitOuter.SplitterDistance / (float)splitOuter.Height;
			Settings.Default.DataCorpusVwRecordPaneVisible = RawRecViewOn;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewClosing(object args)
		{
			if (args == this)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewDocking(object args)
		{
			if (args == this && IsHandleCreated)
				SaveSettings();

			return false;
		}

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
					splitOuter.SplitterDistance = (int)(splitOuter.Height *
						Settings.Default.DataCorpusVwSplitRatio);
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
					LoadToolbar();
				}

				if (WordListGrid != null)
					WordListGrid.SetStatusBarText();
			}

			return false;
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

		#region Playback related methods
		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackSpeedAdjusterPlayClick(object sender, EventArgs e)
		{
			m_tmAdapter.HideBarItemsPopup("tbbAdjustPlaybackSpeedParent");
			m_tmAdapter.HideBarItemsPopup("tbbPlayback");
			WordListGrid.OnPlayback(null);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownAdjustPlaybackSpeed(object args)
		{
			if (!m_activeView || WordListGrid == null || WordListGrid.Cache == null)
				return false;

			m_playbackSpeedAdjuster.PlaybackSpeed = Settings.Default.DataCorpusVwPlaybackSpeed;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedAdjustPlaybackSpeed(object args)
		{
			if (!m_activeView)
				return false;

			Settings.Default.DataCorpusVwPlaybackSpeed = m_playbackSpeedAdjuster.PlaybackSpeed;
			return true;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			RawRecViewOn = Settings.Default.DataCorpusVwRecordPaneVisible;
			OnViewDocked(this);
			m_initialDock = true;
			Application.DoEvents();
			WordListGrid.Focus();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnProjectLoaded(object args)
		{
			base.OnProjectLoaded(args);
			WordListGrid.SortOptions = Project.DataCorpusVwSortOptions;
			return false;
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the record pane with the raw record query for the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			_recView.UpdateRecord(WordListGrid.GetRecord(e.RowIndex));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the record view after a sort has taken place since the grid's RowEnter
		/// event doesn't seem to take care of it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnWordListGridSorted(object args)
		{
			PaWordListGrid grid = args as PaWordListGrid;
			if (grid != WordListGrid)
				return false;

			_recView.UpdateRecord(WordListGrid.GetRecord(WordListGrid.CurrentCellAddress.Y));
			return true;
		}

		#region Properties

		/// ------------------------------------------------------------------------------------
		public PaWordListGrid WordListGrid { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool RawRecViewOn
		{
			get { return m_rawRecViewOn; }
			set
			{
				if (m_rawRecViewOn != value)
				{
					m_rawRecViewOn = value;
					splitOuter.Panel2Collapsed = !value;
					Padding padding = splitOuter.Panel1.Padding;
					padding = new Padding(padding.Left, padding.Top, padding.Right,
						(value ? 0 : splitOuter.Panel2.Padding.Bottom));
					splitOuter.Panel1.Padding = padding;
				}
			}
		}

		#endregion

		#region Message Handlers
		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			if (args == this)
				WordListGrid.SetStatusBarText();

			return false;
		}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownGroupByFieldParent(object args)
		{
			var itemProps = args as ToolBarPopupInfo;
			if (itemProps == null || !m_activeView)
				return false;

			WordListGrid.BuildGroupByMenu(itemProps.Name, m_tmAdapter);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupByFieldParent(object args)
		{
			var itemProps = args as TMItemProperties;
            if (!m_activeView || itemProps == null || itemProps.Name.StartsWith("tbb", StringComparison.Ordinal))
				return false;

			WordListGrid.BuildGroupByMenu(itemProps.Name, App.TMAdapter);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnGroupBySortedField(object args)
		{
			if (!m_activeView)
				return false;

			if (WordListGrid.IsGroupedByField)
				WordListGrid.GroupByField = null;
			else if (WordListGrid.SortOptions.SortFields != null &&
				WordListGrid.SortOptions.SortFields.Count > 0)
			{
				WordListGrid.GroupByField = WordListGrid.SortOptions.SortFields[0].Field;
				if (Settings.Default.CollapseWordListsOnGrouping)
					WordListGrid.ToggleGroupExpansion(false);
			}

			if (!WordListGrid.CurrentCell.Displayed && WordListGrid.CurrentCell != null)
				WordListGrid.ScrollRowToMiddleOfGrid(WordListGrid.CurrentCell.RowIndex);

			FindInfo.ResetStartSearchCell(true);
			FindInfo.CanFindAgain = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupBySortedField(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!m_activeView || itemProps == null)
				return false;

			bool enable = (WordListGrid != null && WordListGrid.Cache != null && WordListGrid.RowCount > 1);
			
			if (WordListGrid.RowCount == 0)
			{
				if (itemProps.Enabled)
				{
					itemProps.Visible = true;
					itemProps.Checked = false;
					itemProps.Enabled = false;
					itemProps.Update = true;
				}
			}
			else if (itemProps.Checked != WordListGrid.IsGroupedByField || enable != itemProps.Enabled)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Checked = WordListGrid.IsGroupedByField;
				itemProps.Update = true;
			}

			return true;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateShowCIEResults(object args)
		//{
		//    TMItemProperties itemProps = args as TMItemProperties;
		//    if (!m_activeView || itemProps == null)
		//        return false;

		//    if (itemProps.Enabled || itemProps.Checked)
		//    {
		//        itemProps.Visible = true;
		//        itemProps.Enabled = false;
		//        itemProps.Checked = false;
		//        itemProps.Update = true;
		//    }

		//    return true;
		//}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the logic for all methods OnUpdateEditFind(Next/Previous)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool HandleFindItemUpdate(TMItemProperties itemProps, bool enableAllow)
		{
			if (!m_activeView || itemProps == null)
				return false;

			bool enable = (enableAllow && WordListGrid != null &&
				WordListGrid.Cache != null && WordListGrid.RowCount > 0);

			if (itemProps.Enabled != enable)
			{
				itemProps.Enabled = enable;
				itemProps.Visible = true;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFind(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, true);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFindNext(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, FindInfo.CanFindAgain);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditFindPrevious(object args)
		{
			return HandleFindItemUpdate(args as TMItemProperties, FindInfo.CanFindAgain);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExpandAllGroups(object args)
		{
			if (!m_activeView)
				return false;

			WordListGrid.ToggleGroupExpansion(true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExpandAllGroups(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!m_activeView || itemProps == null)
				return false;

			if (itemProps.Enabled != (WordListGrid.IsGroupedByField && !WordListGrid.AllGroupsExpanded))
			{
				itemProps.Visible = true;
				itemProps.Enabled = (WordListGrid.IsGroupedByField && !WordListGrid.AllGroupsExpanded);
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnCollapseAllGroups(object args)
		{
			if (!m_activeView)
				return false;

			WordListGrid.ToggleGroupExpansion(false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCollapseAllGroups(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!m_activeView || itemProps == null)
				return false;

			if (itemProps.Enabled != (WordListGrid.IsGroupedByField && !WordListGrid.AllGroupsCollapsed))
			{
				itemProps.Visible = true;
				itemProps.Enabled = (WordListGrid.IsGroupedByField && !WordListGrid.AllGroupsCollapsed);
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
			if (!m_activeView)
				return false;

			RawRecViewOn = !RawRecViewOn;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Toggles showing the record pane below the tab groups.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowRecordPane(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!m_activeView || itemProps == null)
				return false;

			bool enable = (WordListGrid != null);

			if (itemProps.Checked != m_rawRecViewOn || enable != itemProps.Enabled)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Checked = m_rawRecViewOn;
				itemProps.Update = true;
			}
			
			return true;
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
		protected override bool OnUserInterfaceLangaugeChanged(object args)
		{
			_recView.ForceUpdate();
			return base.OnUserInterfaceLangaugeChanged(args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the record view when the user changed the order or visibility of fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRecordViewOptionsChanged(object args)
		{
			_recView.UpdateRecord(WordListGrid.GetRecord(), true);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnDataSourcesModified(object args)
		{
			base.OnDataSourcesModified(args);

			int savCurrRowIndex = 0;
			int savCurrColIndex = 0;
			int savFirstRowIndex = 0;
			SortOptions savSortOptions = null;

			if (WordListGrid != null)
			{
				// Save the index of the row that's current and the index of the first visible row.
				savCurrRowIndex = (WordListGrid.CurrentRow != null ? WordListGrid.CurrentRow.Index : 0);
				savCurrColIndex = (WordListGrid.CurrentCell != null ? WordListGrid.CurrentCell.ColumnIndex : 0);
				savFirstRowIndex = WordListGrid.FirstDisplayedScrollingRowIndex;
				
				// Save the current sort options
				savSortOptions = WordListGrid.SortOptions;
			}

			// Update the fonts in case a custom field's name
			// has changed (since each field has it's own font).
			_recView.UpdateFonts();

			// Update the record in case we're pointing to a new record or it's data changed.
			_recView.UpdateRecord(null);

			// Rebuild the contents of the window.
			LoadWindow();

			// Restore the current row to what it was before rebuilding.
			// Then make sure the row is visible.
			if (WordListGrid != null)
			{
				WordListGrid.PostDataSourceModifiedRestore(
					savCurrRowIndex, savCurrColIndex, savFirstRowIndex, savSortOptions);
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
			_recView.UpdateFonts();

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compares the grid sent in args with the data corpus' grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnCompareGrid(object args)
		{
			var grid = args as PaWordListGrid;
			return (grid != null && WordListGrid == grid);
		}

		#endregion

		#region Export Message Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the RtfExportDlg form.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsRTF(object args)
		{
			if (!m_activeView)
				return false;

			var rtfExp = new RtfExportDlg(WordListGrid);
			rtfExp.ShowDialog(this);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsRTF(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!m_activeView || itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = m_activeView;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsHTML(object args)
		{
			var fmt = LocalizationManager.GetString(
				"Views.DataCorpus.DefaultHTMLExportFileAffix", "{0}-DataCorpus.html");

			return Export(fmt, App.kstidFileTypeHTML, "html",
				Settings.Default.OpenHtmlDataCorpusAfterExport,
				DataCorpusExporter.ToHtml);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsWordXml(object args)
		{
			var fmt = LocalizationManager.GetString(
				"Views.DataCorpus.DefaultWordXmlExportFileAffix",
				"{0}-DataCorpus-(Word).xml");

			return Export(fmt, App.kstidFileTypeWordXml, "xml",
				Settings.Default.OpenWordXmlDataCorpusAfterExport,
				DataCorpusExporter.ToWordXml);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsXLingPaper(object args)
		{
			var fmt = LocalizationManager.GetString(
				"Views.DataCorpus.DefaultXLingPaperExportFileAffix",
				"{0}-DataCorpus-(XLingPaper).xml");

			return Export(fmt, App.kstidFileTypeXLingPaper, "xml",
				Settings.Default.OpenXLingPaperDataCorpusAfterExport,
				DataCorpusExporter.ToXLingPaper);
		}

		/// ------------------------------------------------------------------------------------
		private bool Export(string fmtFileName, string fileTypeFilter, string defaultFileType,
			bool openAfterExport, Func<PaProject, string, PaWordListGrid, bool, bool> exportAction)
		{
			if (!m_activeView)
				return false;

			var defaultFileName = string.Format(fmtFileName,
				PaProject.GetCleanNameForFileName(Project.LanguageName));

			var fileTypes = fileTypeFilter + "|" + App.kstidFileTypeAllFiles;

			int filterIndex = 0;
			var outputFileName = App.SaveFileDialog(defaultFileType, fileTypes, ref filterIndex,
				App.kstidSaveFileDialogGenericCaption, defaultFileName, Project.Folder);

			if (string.IsNullOrEmpty(outputFileName))
				return false;

			exportAction(Project, outputFileName, WordListGrid, openAfterExport);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsHTML(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!m_activeView || itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsWordXml(object args)
		{
			return OnUpdateExportAsHTML(args);
		}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsXLingPaper(object args)
		{
			return OnUpdateExportAsHTML(args);
		}

		#endregion

		#region Phonetic Sort methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownDataCorpusPhoneticSort(object args)
		{
			if (!m_activeView)
				return false;

			var itemProps = args as ToolBarPopupInfo;
			if (itemProps == null)
				return false;

			m_phoneticSortOptionsDropDown =
				new SortOptionsDropDown(WordListGrid.SortOptions, false);

			m_phoneticSortOptionsDropDown.SortOptionsChanged += HandlePhoneticSortOptionsChanged;
			itemProps.Control = m_phoneticSortOptionsDropDown;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Called when sort options change.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneticSortOptionsChanged(SortOptions sortOptions)
		{
			WordListGrid.SortOptions = sortOptions;
			_recView.UpdateRecord(WordListGrid.GetRecord());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedDataCorpusPhoneticSort(object args)
		{
			if (!m_activeView)
				return false;

			m_phoneticSortOptionsDropDown = null;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateDataCorpusPhoneticSort(object args)
		{
			var itemProps = args as TMItemProperties;
			if (!m_activeView || itemProps == null)
				return false;

			if (!itemProps.Enabled)
			{
				itemProps.Visible = true;
				itemProps.Enabled = true;
				itemProps.Update = true;
			}
			
			return true;
		}

        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateShowHtmlChart(object args)
        {
            return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateShowHistogram(object args)
        {
            return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
        }
		
		#endregion
	}
}