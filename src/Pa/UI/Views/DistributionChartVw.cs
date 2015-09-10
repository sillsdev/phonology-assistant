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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using Palaso.IO;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa.UI.Views
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Form in which search patterns are defined and used for searching.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public partial class DistributionChartVw : ViewBase, ITabView, ISearchResultsViewHost
    {
        private bool _initialDock = true;
        private bool _editingSavedChartName;
        private SlidingPanel _slidingPanel;
        private List<DistributionChart> _savedCharts;
        private ITMAdapter _tmAdapter;
        private readonly string _openClass = App.kOpenClassBracket;
        private readonly string _closeClass = App.kCloseClassBracket;
        private readonly SplitterPanel _dockedSidePanel;
        private readonly DistributionGrid _grid;
        private readonly Keys _saveChartHotKey = Keys.None;
        private Rectangle _dragBoxFromMouseDown;
        private int _rowIndexFromMouseDown;
        private int _rowIndexOfItemUnderMouseToDrop;
        private int _columnIndexFromMouseDown;
        private int _columnIndexOfItemUnderMouseToDrop;
        // where need to change the "moving" back to false (not finished)
        private bool _moving = false;
        private bool _movingDouble = false;
        private int _moveColumnIndex;

        /// ------------------------------------------------------------------------------------
        public DistributionChartVw(PaProject project)
            : base(project)
        {
            Utils.WaitCursors(true);
            InitializeComponent();
            Name = "XYChartVw";

            hlblSavedCharts.TextFormatFlags &= ~TextFormatFlags.HidePrefix;

            _grid = new DistributionGrid(Project);
            _grid.OwningView = this;
            _grid.Dock = DockStyle.Fill;
            _grid.TabIndex = lblChartName.TabIndex + 1;
            _grid.KeyDown += HandleGridKeyDown;
            _grid.CellMouseDoubleClick += HandleGridCellMouseDoubleClick;
            _grid.AllowUserToOrderColumns = false;
            _grid.MouseMove += new MouseEventHandler(Grid_MouseMove);
            _grid.DragDrop += new DragEventHandler(Grid_DragDrop);
            _grid.MouseDown += new MouseEventHandler(Grid_MouseDown);
            _grid.DragOver += new DragEventHandler(Grid_DragOver);
            _grid.CurrentCellDirtyStateChanged += new EventHandler(Grid_CurrentCellDirtyStateChanged);
            _grid.DataError += new DataGridViewDataErrorEventHandler(Grid_DataError);

            LoadToolbarAndContextMenus();
            _grid.TMAdapter = _tmAdapter;
            _grid.OwnersNameLabelControl = lblChartNameValue;

            SetupSidePanelContents();
            SetupSlidingPanel();
            OnPaFontsChanged(null);

            _dockedSidePanel = (_slidingPanel.SlideFromLeft ? splitOuter.Panel1 : splitOuter.Panel2);

            LoadSettings();
            _grid.Reset();
            splitChart.Panel1.Controls.Add(_grid);
            _grid.BringToFront();
            splitChart.Panel1MinSize = _grid.Top + 10;
            splitChart.Panel2Collapsed = true;

            UpdateButtons();

            base.DoubleBuffered = true;

            var itemProps = _tmAdapter.GetItemProperties("tbbSaveChartOnMenu");
            if (itemProps != null)
                _saveChartHotKey = itemProps.ShortcutKey;

            lblChartNameValue.Left = lblChartName.Right + 10;

            Utils.WaitCursors(false);
        }

        /// ------------------------------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (_grid != null && !_grid.IsDisposed)
                    _grid.Dispose();

                if (ptrnBldrComponent != null && !ptrnBldrComponent.IsDisposed)
                    ptrnBldrComponent.Dispose();

                if (ResultViewManger != null)
                    ResultViewManger.Dispose();

                if (splitOuter != null && !splitOuter.IsDisposed)
                    splitOuter.Dispose();

                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// ------------------------------------------------------------------------------------
        private void LoadToolbarAndContextMenus()
        {
            if (_tmAdapter != null)
                _tmAdapter.Dispose();

            _tmAdapter = AdapterHelper.CreateTMAdapter();

            if (ResultViewManger != null)
                ResultViewManger.TMAdapter = _tmAdapter;
            else
            {
                ResultViewManger = new SearchResultsViewManager(this, _tmAdapter,
                    splitResults, _recView, Settings.Default.DistChartVwPlaybackSpeed,
                    newSpeed => Settings.Default.DistChartVwPlaybackSpeed = newSpeed);
            }

            if (_tmAdapter == null)
                return;

            _tmAdapter.LoadControlContainerItem += m_tmAdapter_LoadControlContainerItem;

            var defs = new[] { FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
				"DistributionChartsTMDefinition.xml") };

            _tmAdapter.Initialize(this, App.MsgMediator, App.ApplicationRegKeyPath, defs);
            _tmAdapter.AllowUpdates = true;
            _tmAdapter.SetContextMenuForControl(_grid, "cmnuXYChart");
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Hand off a couple of drop-down controls to the toobar/menu adapter.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private Control m_tmAdapter_LoadControlContainerItem(string name)
        {
            if (name == "tbbSearchOptionsDropDown")
                return _grid.SearchOptionsDropDown;

            if (name == "tbbAdjustPlaybackSpeed")
                return ResultViewManger.PlaybackSpeedAdjuster;

            return null;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// There's a problem with Ctrl+S (save chart) getting recognized when there is a
        /// search result word list showing. Therefore, we trap it at a lower level.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Make sure the user pressed the hotkey for saving a chart and
            // that he isn't in the middle of editing a saved chart name.
            if (msg.Msg == 0x100 && keyData == _saveChartHotKey && !_editingSavedChartName)
            {
                // Make sure the button is enabled.
                var itemProps = _tmAdapter.GetItemProperties("tbbSaveChart");
                if (itemProps != null && itemProps.Enabled)
                {
                    App.MsgMediator.SendMessage("SaveChart", null);
                    return true;
                }
            }
            else if (keyData == (Keys)117) //Key-press "F6"
            {
                ptrnBldrComponent.Focus();
                ptrnBldrComponent.tabPatternBlding.SelectedTab = ptrnBldrComponent.tabPatternBlding.TabPages[0];

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the view's result view manager.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public SearchResultsViewManager ResultViewManger { get; private set; }

        /// ------------------------------------------------------------------------------------
        private static string FillChartMessage
        {
            get
            {
                return LocalizationManager.GetString("Views.DistributionChart.FillChartMsg",
                    "You must first choose 'Fill Chart' before seeing search results.");
            }
        }

        #region Loading and saving charts
        /// ------------------------------------------------------------------------------------
        private void LoadSavedChartsList()
        {
            var filename = DistributionChart.GetFileForProject(Project.ProjectPathFilePrefix);
            _savedCharts = XmlSerializationHelper.DeserializeFromFile<List<DistributionChart>>(
                filename, "distributionCharts");

            if (_savedCharts == null)
                return;

            lvSavedCharts.Items.Clear();
            foreach (var layout in _savedCharts)
            {
                var item = new ListViewItem(layout.Name);
                item.Tag = layout;
                lvSavedCharts.Items.Add(item);
            }
        }

        /// ------------------------------------------------------------------------------------
        private void SaveCharts()
        {
            if (_savedCharts != null)
            {
                XmlSerializationHelper.SerializeToFile(DistributionChart.GetFileForProject(
                    Project.ProjectPathFilePrefix), _savedCharts, "distributionCharts");
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Searches the collection of saved chart layouts and returns the one whose name
        /// is that of the one specified. If the layout cannot be found, null is returned.
        /// </summary>
        /// <param name="layoutToSkip">Layout to skip as the collection of saved
        /// layouts is searched for the one having the specified name.</param>
        /// <param name="nameToCheck">The name of the saved layout to search for.</param>
        /// <param name="showMsg">true to show a message box if the layout cannot be
        /// found. Otherwise, false.</param>
        /// ------------------------------------------------------------------------------------
        private DistributionChart GetExistingLayoutByName(DistributionChart layoutToSkip,
            string nameToCheck, bool showMsg)
        {
            // Check if chart name already exists. If it does,
            // tell the user and don't cancel the current edit.
            foreach (var savedLayout in _savedCharts.Where(sl => sl != layoutToSkip && sl.Name == nameToCheck))
            {
                if (showMsg)
                {
                    var fmt = LocalizationManager.GetString(
                        "Views.DistributionChart.SavedChartNameAlreadyExistsMsg",
                        "There is already a saved chart with the name '{0}'.");
                    App.NotifyUserOfProblem(fmt, nameToCheck);
                }

                return savedLayout;
            }

            return null;
        }

        #endregion

        #region Methods for setting up side panel
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

            LoadSavedChartsList();

            btnAutoHide.Left = btnDock.Left = (pnlSideBarCaption.Width - btnDock.Width - 6);
            btnAutoHide.Visible = false;
        }

        /// ------------------------------------------------------------------------------------
        private void SetupSlidingPanel()
        {
            pnlSideBarCaption.Height = FontHelper.UIFont.Height + 7;
            pnlSideBarCaption.Font = FontHelper.UIFont;

            btnAutoHide.Top = ((pnlSideBarCaption.Height - btnAutoHide.Height) / 2) - 1;
            btnDock.Top = btnAutoHide.Top;

            _slidingPanel = new SlidingPanel(this, splitSideBarOuter, pnlSliderPlaceholder,
                Settings.Default.DistChartVwSidePanelWidth,
                newWidth => Settings.Default.DistChartVwSidePanelWidth = newWidth);

            // For the code scanner to work, the control (parameter 4 in GetString) cannot
            // refer to a property, it has to refer to the actual object. Therefore, create
            // a reference to the actual object (the button, in this case) and pass that
            // to GetString.
            var button = _slidingPanel.Tab;
            button.Text = LocalizationManager.GetString("Views.DistributionChartVw.UndockedSideBarTabText", "Charts & Chart Building", null, button);

            Controls.Add(_slidingPanel);
            splitOuter.BringToFront();
        }

        #endregion

        #region Loading and saving settings
        /// ------------------------------------------------------------------------------------
        private void LoadSettings()
        {
            ptrnBldrComponent.LoadSettings(Name,
                Settings.Default.DistributionChartsIPACharExplorerExpandedStates);

            if (Settings.Default.DistChartVwSidePanelDocked)
                btnDock_Click(null, null);
            else
                btnAutoHide_Click(null, null);

            OnViewDocked(this);
            _initialDock = true;
            ResultViewManger.RecordViewOn = Settings.Default.DistChartVwrRcordPaneVisible;

            // Hide the record view pane until the first search, at which time the value of
            // m_histogramOn will determine whether or not to show the record view pane.
            splitResults.Panel2Collapsed = true;
        }

        #endregion

        #region ITabView Members
        /// ------------------------------------------------------------------------------------
        public bool ActiveView { get; private set; }

        /// ------------------------------------------------------------------------------------
        public void SetViewActive(bool makeActive, bool isDocked)
        {
            ActiveView = makeActive;

            if (ActiveView && isDocked && ResultViewManger.CurrentViewsGrid != null &&
                ResultViewManger.CurrentViewsGrid.Focused)
            {
                ResultViewManger.CurrentViewsGrid.SetStatusBarText();
            }
        }

        /// ------------------------------------------------------------------------------------
        public Form OwningForm
        {
            get { return FindForm(); }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the view's toolbar/menu adapter.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public ITMAdapter TMAdapter
        {
            get { return _tmAdapter; }
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
            Settings.Default.DistChartVwSidePanelDocked = _slidingPanel.SlideFromLeft ?
                !splitOuter.Panel1Collapsed : !splitOuter.Panel2Collapsed;

            ptrnBldrComponent.SaveSettings(Name, charExplorerStates =>
                Settings.Default.DistributionChartsIPACharExplorerExpandedStates = charExplorerStates);

            Settings.Default.DistChartVwrRcordPaneVisible = ResultViewManger.RecordViewOn;

            try
            {
                // These are in a try/catch because sometimes they might throw an exception
                // in rare cases. The exception has to do with a condition in the underlying
                // .Net framework that I haven't been able to make sense of. Anyway, if an
                // exception is thrown, no big deal, the splitter distances will just be set
                // to their default values.
                Settings.Default.DistChartVwSplitRatio1 =
                    splitOuter.SplitterDistance / (float)splitOuter.Width;

                Settings.Default.DistChartVwSplitRatio2 =
                    splitResults.SplitterDistance / (float)splitResults.Height;

                Settings.Default.DistChartVwSplitRatio3 =
                    splitSideBarOuter.SplitterDistance / (float)splitSideBarOuter.Height;

                Settings.Default.DistChartVwSplitRatio4 =
                    splitChart.SplitterDistance / (float)splitChart.Height;
            }
            catch { }
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
                    splitOuter.SplitterDistance = (int)(splitOuter.Width *
                        Settings.Default.DistChartVwSplitRatio1);
                    splitResults.SplitterDistance = (int)(splitResults.Height *
                        Settings.Default.DistChartVwSplitRatio2);
                    splitSideBarOuter.SplitterDistance = (int)(splitSideBarOuter.Height *
                        Settings.Default.DistChartVwSplitRatio3);
                    splitChart.SplitterDistance = (int)(splitChart.Height *
                        Settings.Default.DistChartVwSplitRatio4);
                }
                catch { }

                // Don't need to load the tool bar or menus if this is the first time
                // the view was docked since that all gets done during construction.
                if (_initialDock)
                {
                    _initialDock = false;
                    _grid.Focus();
                }
                else
                {
                    // The toolbar has to be recreated each time the view is removed from it's
                    // (undocked) form and docked back into the main form. The reason has to
                    // do with tooltips. They seem to form an attachment, somehow, with the
                    // form that owns the controls the tooltip is extending. When that form
                    // gets pulled out from under the tooltips, sometimes the program will crash.
                    App.RefreshToolTipsOnLocalizationManager();
                }
            }

            return false;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnViewUndocked(object args)
        {
            if (args == this)
                App.RefreshToolTipsOnLocalizationManager();

            return false;
        }

        #endregion

        #region Side Panel button click and update handlers
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

            // Uncollapse the panel in which the side panel will be docked.
            if (_slidingPanel.SlideFromLeft)
                splitOuter.Panel1Collapsed = false;
            else
                splitOuter.Panel2Collapsed = false;

            // Let the sliding panel control handle the rest of the docking procedure.
            _slidingPanel.DockControl(_dockedSidePanel);
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

            // Let the sliding panel control handle most of the undocking/hiding procedure.
            _slidingPanel.UnDockControl(_dockedSidePanel);

            // Collapse the panel in which the side panel was docked.
            if (_slidingPanel.SlideFromLeft)
                splitOuter.Panel1Collapsed = true;
            else
                splitOuter.Panel2Collapsed = true;
        }

        #endregion

        #region DistributionGrid event methods
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Perform a search when the user clicks on a cell containing a count.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        void HandleGridCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (_grid.IsEmpty || col <= 0 || row <= 0 || _grid[col, row].Value is IList<SearchQueryValidationError>)
                return;

            if (_grid.IsCurrentCellValidForSearch)
                Search(row, col, SearchResultLocation.CurrentTabGroup);
            else
                Utils.MsgBox(FillChartMessage);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Perform a search when the user presses enter on a cell.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        void HandleGridKeyDown(object sender, KeyEventArgs e)
        {
            var pt = _grid.CurrentCellAddress;

            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None || _grid.IsEmpty ||
                pt.X <= 0 || pt.Y <= 0 || pt.X == _grid.Columns.Count - 1 ||
                pt.Y == _grid.NewRowIndex)
            {
                return;
            }

            if (_grid.IsCurrentCellValidForSearch)
                Search(_grid.CurrentCell, SearchResultLocation.CurrentTabGroup);
            else
                Utils.MsgBox(FillChartMessage);

            e.Handled = true;
        }

        #endregion

        #region Non DragDrop keyboard and mouse events for inserting text into grid
        /// ------------------------------------------------------------------------------------
        private void HandleFeatureListCustomDoubleClick(object sender, string feature)
        {
            _grid.InsertTextInCell(feature);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Handles the user pressing Enter on a feature.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public void HandleFeatureListKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && sender is FeatureListViewBase)
            {
                string text = ((FeatureListViewBase)sender).CurrentFormattedFeature;
                _grid.InsertTextInCell(text);
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
            _grid.InsertTextInCell(item.Text.Replace(App.DottedCircle, string.Empty));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Inserts phones into the search pattern when they're clicked on from the vowel or
        /// consonant tab.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleVowConClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _grid.InsertTextInCell(e.ClickedItem.Text);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// When user double-clicks a class name, then put user in insert mode to insert that
        /// class in the pattern.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleClassListDoubleClick(object sender, MouseEventArgs e)
        {
            var lv = ptrnBldrComponent.ClassListView;

            if (lv.SelectedItems.Count > 0)
            {
                var item = lv.SelectedItems[0] as ClassListViewItem;
                if (item != null)
                {
                    _grid.InsertTextInCell((
                        item.Pattern == null || Settings.Default.ShowClassNamesInSearchPatterns ?
                        _openClass + item.Text + _closeClass : item.Pattern));
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
        /// Begin dragging something to insert into a search item or environment cell.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void HandleItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            string dragText = null;

            if (e.Item is string)
                dragText = e.Item as string;
            else if (sender is FeatureListViewBase)
                dragText = ((FeatureListViewBase)sender).CurrentFormattedFeature;
            else if (e.Item is ClassListViewItem)
            {
                var item = e.Item as ClassListViewItem;
                dragText = (item.Pattern == null || Settings.Default.ShowClassNamesInSearchPatterns ?
                    _openClass + item.Text + _closeClass : item.Pattern);
            }

            // Any text we begin dragging.
            if (dragText != null)
            {
                if (_slidingPanel.Visible)
                    _slidingPanel.Close(true);

                DoDragDrop(dragText.Replace(App.DottedCircle, string.Empty), DragDropEffects.Copy);
            }
        }

        #endregion

        #region Event methods for saved charts list view
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Remove the selected saved chart when the remove button is pressed.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void btnRemoveSavedChart_Click(object sender, EventArgs e)
        {
            if (lvSavedCharts.SelectedItems.Count == 0)
                return;

            var msg = LocalizationManager.GetString("Views.DistributionChart.ConfirmSavedChartRemoveMsg",
                "Are you sure you want to remove the saved chart?");

            if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var layout = lvSavedCharts.SelectedItems[0].Tag as DistributionChart;

            if (layout != null)
            {
                if (layout == _grid.ChartLayout ||
                    (_grid.ChartLayout != null && layout.Name == _grid.ChartLayout.Name))
                {
                    // Don't delete the m_xyGrid.ChartLayout if the saved chart name is in edited mode
                    if (layout.Name != null)
                    {
                        _grid.Reset();
                        _grid.ChartLayout = null;
                    }
                }

                _savedCharts.Remove(layout);
                int index = lvSavedCharts.SelectedIndices[0];
                lvSavedCharts.Items.Remove(lvSavedCharts.SelectedItems[0]);
                if (index >= lvSavedCharts.Items.Count)
                    index--;

                if (index >= 0)
                    lvSavedCharts.Items[index].Selected = true;

                SaveCharts();
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Restore all the default Charts when the restore button is pressed.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void btnRestoreDefaultCharts_Click(object sender, EventArgs e)
        {
            var msg = LocalizationManager.GetString("Views.DistributionChart.ConfirmRestoreDefaultChartsMsg",
                "Are you sure you want to restore default charts?");

            if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            RestoreDefaultChartsItem();
            SaveCharts();

            int? selectedIndex = null;
            if (lvSavedCharts.SelectedItems.Count != 0)
                selectedIndex = lvSavedCharts.SelectedIndices[0];

            LoadSavedChartsList();
            LoadGridValue(selectedIndex);
        }

        private void LoadGridValue(int? selectedIndex)
        {
            if (selectedIndex.HasValue || lvSavedCharts.SelectedItems.Count != 0)
            {
                if (selectedIndex != null) lvSavedCharts.Items[(int) selectedIndex].Selected = true;
                LoadSavedLayout(lvSavedCharts.SelectedItems[0]);
            }
        }

        private void RestoreDefaultChartsItem()
        {
            var defaultChartFile = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
                "DefaultDistributionCharts.xml");
            var defaultChart = XmlSerializationHelper.DeserializeFromFile<List<DistributionChart>>(defaultChartFile,
                "distributionCharts");

            List<DistributionChart> saveCustomCharts = _savedCharts.ToList();
            foreach (var removeDefault in defaultChart)
            {
                foreach (var chartVal in _savedCharts)
                {
                    if (removeDefault.Name == chartVal.Name)
                    {
                        saveCustomCharts.Remove(chartVal);
                        break;
                    }
                }
            }
            _savedCharts = new List<DistributionChart>();
            _savedCharts = defaultChart;

            foreach (var addCustomCharts in saveCustomCharts)
            {
                if (!_savedCharts.Contains(addCustomCharts))
                    _savedCharts.Add(addCustomCharts);
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Remove the selected saved chart when the delete key is pressed. Load the saved
        /// chart into the grid when pressing enter and put the user into the edit mode
        /// in the saved charts list when he presses F2.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void lvSavedCharts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.None || lvSavedCharts.SelectedItems.Count == 0)
                return;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    btnRemoveSavedChart_Click(null, null);
                    e.Handled = true;
                    break;
                case Keys.F2:
                    lvSavedCharts.SelectedItems[0].BeginEdit();
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    LoadSavedLayout(lvSavedCharts.SelectedItems[0]);
                    e.Handled = true;
                    break;
            }
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// When double-clicking on a saved chart, that chart will be loaded in the grid.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void lvSavedCharts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // When a saved chart is double-clicked on, load it into the grid.
            LoadSavedLayout(lvSavedCharts.HitTest(e.Location).Item);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the saved item represented by the item in the saved item's list view.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void LoadSavedLayout(ListViewItem item)
        {
            if (item == null)
                return;

            if (_slidingPanel.Visible)
                _slidingPanel.Close(true);

            _grid.LoadFromLayout(item.Tag as DistributionChart);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void lvSavedCharts_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            _editingSavedChartName = true;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Verify the new saved chart's name is not a duplicate. If not, then save the
        /// change to disk.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void lvSavedCharts_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            string newName = (e.Label != null ? e.Label.Trim() : null);
            if (string.IsNullOrEmpty(newName))
                newName = null;

            DistributionChart layout = lvSavedCharts.Items[e.Item].Tag as DistributionChart;
            if (layout == null || layout.Name == newName || newName == null)
            {
                e.CancelEdit = true;
                _editingSavedChartName = false;
                return;
            }

            // Check if chart name already exists. If it does, cancel the current edit.
            if (GetExistingLayoutByName(layout, newName, true) != null)
            {
                e.CancelEdit = true;
                return;
            }

            // If the chart loaded in the grid is the one whose name was just edited,
            // then update the loaded name and the label above the grid.
            if (_grid.ChartLayout != null &&
                _grid.ChartLayout.Name == lvSavedCharts.Items[e.Item].Text)
            {
                _grid.ChartLayout.Name = newName;
                lblChartNameValue.Text = newName;
            }

            // Keep the new name and save the change to disk.
            layout.Name = newName;
            SaveCharts();

            lvSavedCharts.Items[e.Item].Text = newName;
            e.CancelEdit = true;
            _editingSavedChartName = false;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void lvSavedCharts_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem item = e.Item as ListViewItem;
            if (e.Button != MouseButtons.Left || item == null || item.Tag == null ||
                !(item.Tag is DistributionChart))
                return;

            if (_slidingPanel.Visible)
                _slidingPanel.Close(true);

            DoDragDrop(item.Tag as DistributionChart, DragDropEffects.Move);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Makes sure the list has a selected item.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void lvSavedCharts_Enter(object sender, EventArgs e)
        {
            // Make sure an item is selected when the list gets focus. Probably the only
            // time the list will get focus and not have a selected item is the first
            // time the list gains focus after the view has been loaded.
            if (lvSavedCharts.SelectedIndices.Count == 0 && lvSavedCharts.Items.Count > 0)
                lvSavedCharts.Items[0].Selected = true;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Make sure the only column in the saved charts list is the same with as its
        /// owning list view. Also make sure the list view's size fills the panel underneath
        /// the header label.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void pnlSavedCharts_Resize(object sender, EventArgs e)
        {
            // Save the index of the selected item.
            int i = (lvSavedCharts.SelectedIndices.Count > 0 ? lvSavedCharts.SelectedIndices[0] : 0);

            // This is sort of a kludge, but it's necessary due to the possiblity that
            // the list view's column header will change size. It turns out that if there
            // were any items scrolled off the top of the list and the column header is
            // resized and the new size of the list view will cause the vertical scroll
            // bar to go away, there will be one or more blank lines at the top of the
            // list view. Making sure the first item is visible before changing the column
            // header's size will prevent this. See PA-676.
            if (lvSavedCharts.Items.Count > 0)
                lvSavedCharts.EnsureVisible(0);

            // Make sure the list view fills the panel it's (accounting for the fact that
            // it's also in the panel underneath the hlblSaveCharts control).
            lvSavedCharts.Size = new Size(pnlSavedCharts.ClientSize.Width,
                pnlSavedCharts.ClientSize.Height - hlblSavedCharts.Size.Height);

            // Resize the list view's colum header so it fits just
            // inside the list view's client area.
            hdrSavedCharts.Width = lvSavedCharts.ClientSize.Width - 3;

            // Make sure the previously selected item is visible.
            if (i >= 0 && lvSavedCharts.Items.Count > 0)
                lvSavedCharts.EnsureVisible(i);
        }

        /// ------------------------------------------------------------------------------------
        private void lvSavedCharts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdateButtons();
        }

        /// ------------------------------------------------------------------------------------
        private void UpdateButtons()
        {
            btnRemoveSavedChart.Enabled = (lvSavedCharts.SelectedItems.Count > 0);
        }

        #endregion

        #region Searching methods
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Perform a search for the specified cell.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void Search(DataGridViewCell cell, SearchResultLocation resultLocation)
        {
            if (cell != null)
                Search(cell.RowIndex, cell.ColumnIndex, resultLocation);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Perform a search for the cell specified by row and col.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void Search(int row, int col, SearchResultLocation resultLocation)
        {
            if (!_grid.IsCellValidForSearch(row, col))
                return;

            _grid.RefreshCellValue(row, col);

            SearchQuery querySearchPattern = _grid.GetCellsFullSearchQuery(row, col);
            if (querySearchPattern != null)
            {
                PerformSearch(row, col, querySearchPattern.ToString());
            }
        }

        /// ------------------------------------------------------------------------------------
        private void PerformSearch(int row, int col, string querySearchPattern)
        {
            var srchPhones = querySearchPattern;
            string toolbarItemName = querySearchPattern;
            if (srchPhones == null)
                return;

            var queries = new List<SearchQuery>();

            var query = new SearchQuery();
            query.Pattern = srchPhones;
            query.IgnoreDiacritics = false;

            // Check if the phone only exists as an uncertain phone. If so,
            // then set the flag in the query to include searching words
            // made using all uncertain uncertain derivations.
            var phoneInfo = Project.PhoneCache[_grid[0, row].Value as string];
            if (phoneInfo != null && phoneInfo.TotalCount == 0)
                query.IncludeAllUncertainPossibilities = true;

            queries.Add(query);

            App.MsgMediator.SendMessage("ViewSearch", queries);

            // Now set the image of the search button to the image associated
            // with the last search environment chosen by the user.
            var childItemProps = _tmAdapter.GetItemProperties(toolbarItemName);
            var parentItemProps = _tmAdapter.GetItemProperties("tbbChartPhoneSearch");
            if (parentItemProps != null && childItemProps != null)
            {
                parentItemProps.Image = childItemProps.Image;
                parentItemProps.Visible = true;
                parentItemProps.Update = true;
                parentItemProps.Tag = new[] { _grid[col, 0].Value as string, toolbarItemName };
                _tmAdapter.SetItemProperties("tbbChartPhoneSearch", parentItemProps);
            }
        }

        #endregion

        #region ISearchResultsViewHost Members
        /// ------------------------------------------------------------------------------------
        public void BeforeSearchPerformed(SearchQuery query, WordListCache resultCache)
        {
        }

        /// ------------------------------------------------------------------------------------
        public void AfterSearchPerformed(SearchQuery query, WordListCache resultCache)
        {
            if (resultCache != null && splitChart.Panel2Collapsed)
            {
                splitChart.Panel2Collapsed = false;
                if (splitChart.SplitterDistance < splitChart.Panel1.Padding.Top * 2.5)
                    splitChart.SplitterDistance = (int)(splitChart.Panel1.Padding.Top * 2.5);
            }
        }

        /// ------------------------------------------------------------------------------------
        public bool ShouldMenuBeEnabled(string menuName)
        {
            return _grid.IsCurrentCellValidForSearch;
        }

        /// ------------------------------------------------------------------------------------
        public SearchQuery GetQueryForMenu(string menuName)
        {
            return _grid.CurrentCellsFullSearchQuery;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// This gets called when all the tabs or tab groups have been closed.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public void NotifyAllTabsClosed()
        {
            // When there are no more search results showing,
            // then close the pane that holds them.
            splitChart.Panel2Collapsed = true;
        }
        
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// This gets called when the current tab has changed.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public void NotifyCurrentTabChanged(SearchResultTab newTab)
        {
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

        #region Message mediator message handler and update handler methods
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Compares the grid sent in args with the current result view grid.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected bool OnCompareGrid(object args)
        {
            var grid = args as PaWordListGrid;
            return (grid != null && ResultViewManger.CurrentViewsGrid == grid);
        }

        /// ------------------------------------------------------------------------------------
        protected override bool OnUserInterfaceLangaugeChanged(object args)
        {
            _recView.ForceUpdate();
            LoadToolbarAndContextMenus();
            App.L10NMngr.RefreshToolTips();
            return base.OnUserInterfaceLangaugeChanged(args);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// This method gets called when the font(s) get changed in the options dialog.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected bool OnPaFontsChanged(object args)
        {
            lvSavedCharts.Font = App.PhoneticFont;
            hlblSavedCharts.Font = FontHelper.UIFont;
            lblChartName.Font = FontHelper.UIFont;
            lblChartNameValue.Font = FontHelper.MakeFont(App.PhoneticFont, FontStyle.Bold);

            int lblHeight = Math.Max(lblChartName.Height, lblChartNameValue.Height);
            int padding = lblHeight + 14;

            splitChart.Panel1.Padding = new Padding(splitChart.Panel1.Padding.Left, padding,
                splitChart.Panel1.Padding.Right, splitChart.Panel1.Padding.Bottom);

            // Center the labels vertically.
            lblChartName.Top =
                (int)Math.Ceiling((padding - lblChartName.Height) / 2f) + 1;

            lblChartNameValue.Top =
                (int)Math.Ceiling((padding - lblChartNameValue.Height) / 2f);

            _recView.UpdateFonts();
            ptrnBldrComponent.RefreshFonts();
            _slidingPanel.RefreshFonts();

            // Return false to allow other windows to update their fonts.
            return false;
        }

        /// ------------------------------------------------------------------------------------
        protected override bool OnDataSourcesModified(object args)
        {
            base.OnDataSourcesModified(args);
            ptrnBldrComponent.RefreshComponents();
            return false;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Fills the grid with search result totals.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected bool OnFillInChart(object args)
        {
            if (!ActiveView)
                return false;

            _grid.FillChart();
            return true;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Fills the grid with search result totals.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateFillInChart(object args)
        {
            var itemProps = args as TMItemProperties;
            if (itemProps == null || !ActiveView)
                return false;

            if (itemProps.Enabled == _grid.IsEmpty)
            {
                itemProps.Visible = true;
                itemProps.Enabled = !_grid.IsEmpty;
                itemProps.Update = true;
            }

            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnSaveChart(object args)
        {
            if (!ActiveView)
                return false;

            // Commit changes and end the edit mode if necessary. Fixes PA-714
            if (_grid.IsCurrentCellInEditMode)
                _grid.EndEdit();

            // If the name isn't specified, then use the save as dialog.
            if (string.IsNullOrEmpty(_grid.ChartName))
                return OnSaveChartAs(args);

            SaveCurrentChart(_grid.ChartLayout);
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnSaveChartAs(object args)
        {
            if (!ActiveView)
                return false;

            // Commit changes and end the edit mode if necessary. Fixes PA-714
            if (_grid.IsCurrentCellInEditMode)
                _grid.EndEdit();

            using (var dlg = new SaveDistributionChartDlg(_grid, _savedCharts))
            {
                if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
                    SaveCurrentChart(dlg.LayoutToOverwrite);
            }

            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnResetChart(object args)
        {
            if (lvSavedCharts.SelectedIndices.Count <= 0 || _grid.ChartLayout == null || _grid.ChartLayout.Name == null || !ActiveView)
                return false;

            ResetCurrentChart(_grid.ChartLayout);
            return true;
        }


        /// ------------------------------------------------------------------------------------
        protected bool OnGridColumnDelete(object args)
        {
            int selectedColumn = _grid.CurrentCell.ColumnIndex;
            int gridColumnCount = _grid.Columns.Count - 1;
            if (selectedColumn != gridColumnCount && selectedColumn != 0)
            {
                _grid.Columns.RemoveAt(selectedColumn);
                _grid.ChartLayout = DistributionChart.ModifyFromDistributionGrid(_grid);
            }

            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnGridRowDelete(object args)
        {
            int selectedRow = _grid.CurrentCell.RowIndex;
            int gridRowCount = _grid.Rows.Count - 1;
            if (selectedRow != gridRowCount && selectedRow != 0)
            {
                _grid.Rows.RemoveAt(selectedRow);
                _grid.ChartLayout = DistributionChart.ModifyFromDistributionGrid(_grid);
            }
            return true;
        }


        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Saves the current layout to the project file.
        /// </summary>
        /// <param name="layoutToOverwrite">The layout to overwrite when saving. This should
        /// be null if the layout is to be added to the list of saved layouts.</param>
        /// ------------------------------------------------------------------------------------
        private void SaveCurrentChart(DistributionChart layoutToOverwrite)
        {
            ListViewItem item = null;

            // When the user wants to overwrite an existing layout, we need to find the
            // item in the saved list that corresponds to the one being overwritten.
            if (layoutToOverwrite != null)
            {
                foreach (ListViewItem lvi in lvSavedCharts.Items)
                {
                    DistributionChart tmpLayout = lvi.Tag as DistributionChart;
                    if (tmpLayout != null && tmpLayout.Name == layoutToOverwrite.Name)
                    {
                        item = lvi;
                        break;
                    }
                }
            }

            if (_savedCharts == null)
                _savedCharts = new List<DistributionChart>();

            var layoutCopy = _grid.ChartLayout.Clone();

            if (item != null)
            {
                // Overwrite an existing layout.
                int i = _savedCharts.IndexOf(item.Tag as DistributionChart);
                _savedCharts[i] = layoutCopy;
                item.Tag = layoutCopy;
                item.Text = layoutCopy.Name;
            }
            else
            {
                // Save a new layout.
                _savedCharts.Add(layoutCopy);
                item = new ListViewItem(layoutCopy.Name);
                item.Tag = layoutCopy;
                lvSavedCharts.Items.Add(item);
            }

            SaveCharts();
            _grid.LoadFromLayout(layoutCopy);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Reset the current Chart Value in xml file.
        /// </summary>
        /// <param name="layoutToOverwrite">The layout to overwrite when saving. This should
        /// be null if the layout is to be added to the list of saved layouts.</param>
        /// ------------------------------------------------------------------------------------
        private void ResetCurrentChart(DistributionChart layoutToOverwrite)
        {
            var defaultChartFile = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultDistributionCharts.xml");
            var defaultChart = XmlSerializationHelper.DeserializeFromFile<List<DistributionChart>>(defaultChartFile, "distributionCharts");

            ListViewItem item = null;
            if (_savedCharts == null)
                _savedCharts = new List<DistributionChart>();

            // When the user wants to overwrite an existing layout, we need to find the
            // item in the DefaultDistributionCharts file  list that corresponds to the one being overwritten.
            if (layoutToOverwrite != null)
            {
                foreach (ListViewItem lvi in lvSavedCharts.Items)
                {
                    DistributionChart tmpLayout = lvi.Tag as DistributionChart;
                    if (tmpLayout != null && tmpLayout.Name == layoutToOverwrite.Name)
                    {
                        item = lvi;
                        break;
                    }
                }
            }

            if (item != null)
            {
                // Overwrite an existing layout.
                int i = _savedCharts.IndexOf(item.Tag as DistributionChart);
                foreach (var chartValue in defaultChart)
                {
                    if (_savedCharts[i].Name == chartValue.Name)
                    {
                        var msg = LocalizationManager.GetString("Views.DistributionChart.ConfirmResetChartMsg", "Are you sure you want to reset the chart?");
                        if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.No)
                            return;

                        _savedCharts[i] = chartValue;
                        item.Tag = chartValue;
                        item.Text = chartValue.Name;

                        SaveCharts();
                        lvSavedCharts.Items[i].Selected = true;
                        LoadSavedLayout(lvSavedCharts.SelectedItems[0]);

                        break;
                    }
                }
            }
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateSaveChart(object args)
        {
            var itemProps = args as TMItemProperties;
            if (itemProps == null || !ActiveView)
                return false;

            if (itemProps.Enabled != (_grid.ChartLayout != null))
            {
                itemProps.Visible = true;
                itemProps.Enabled = (_grid.ChartLayout != null);
                itemProps.Update = true;
            }

            return true;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Performs a search.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        protected bool OnBeginSearch(object args)
        {
            if (!ActiveView)
                return false;

            var itemProps = args as TMItemProperties;
            if (itemProps != null && itemProps.Name.StartsWith("cmnu", StringComparison.Ordinal))
                return false;

            Search(_grid.CurrentCell, SearchResultLocation.CurrentTabGroup);
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateBeginSearch(object args)
        {
            if (!ActiveView)
                return false;

            var itemProps = args as TMItemProperties;
            if (itemProps != null && !itemProps.Name.StartsWith("cmnu", StringComparison.Ordinal))
            {
                if (itemProps.Enabled != _grid.IsCurrentCellValidForSearch)
                {
                    itemProps.Visible = true;
                    itemProps.Enabled = _grid.IsCurrentCellValidForSearch;
                    itemProps.Update = true;
                }

                return true;
            }

            return false;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnExportAsRTF(object args)
        {
            if (!ActiveView)
                return false;

            var rtfExp = new RtfExportDlg(ResultViewManger.CurrentViewsGrid);
            rtfExp.ShowDialog(this);
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateExportAsRTF(object args)
        {
            var itemProps = args as TMItemProperties;
            if (itemProps == null || !ActiveView)
                return false;

            bool enable = (ResultViewManger.CurrentViewsGrid != null &&
                ResultViewManger.CurrentViewsGrid.Focused);

            if (itemProps.Enabled != enable)
            {
                itemProps.Update = true;
                itemProps.Visible = true;
                itemProps.Enabled = enable;
            }

            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnExportAsHTML(object args)
        {
            var fmt = LocalizationManager.GetString("Views.DistributionChart.DefaultHtmlExportFileAffix",
                "{0}-{1}DistributionChart.html");

            return Export(ResultViewManger.HTMLExport, fmt, App.kstidFileTypeHTML, "html",
                Settings.Default.OpenHtmlDistChartAfterExport, DistributionChartExporter.ToHtml);
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

        /// ------------------------------------------------------------------------------------
        protected bool Export(Func<string> wordListExportAction, string fmtFileName,
            string fileTypeFilter, string defaultFileType, bool openAfterExport,
            Func<PaProject, string, DistributionGrid, bool, bool> exportAction)
        {
            if (!ActiveView)
                return false;

            var objForExport = ObjectForExport;

            // Determine whether to export the XY Chart or a search result word list.
            if (!(objForExport is DistributionGrid))
                return (wordListExportAction != null);

            var prefix = (PaProject.GetCleanNameForFileName(Project.LanguageName) ??
                (Project.LanguageCode ?? Project.GetCleanNameForFileName()));

            var defaultFileName = string.Format(fmtFileName, prefix, _grid.ChartName).Replace(" ", string.Empty);

            string fileTypes = fileTypeFilter + "|" + App.kstidFileTypeAllFiles;

            int filterIndex = 0;
            var outputFileName = App.SaveFileDialog(defaultFileType, fileTypes, ref filterIndex,
                App.kstidSaveFileDialogGenericCaption, defaultFileName, Project.Folder);

            if (string.IsNullOrEmpty(outputFileName))
                return false;

            exportAction(Project, outputFileName, _grid, openAfterExport);
            return true;
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnExportAsWordXml(object args)
        {
            var fmt = LocalizationManager.GetString("Views.DistributionChart.DefaultWordXmlExportFileAffix",
                "{0}-{1}DistributionChart-(Word).xml");

            return Export(ResultViewManger.WordXmlExport, fmt, App.kstidFileTypeWordXml, "xml",
                Settings.Default.OpenWordXmlDistChartAfterExport, DistributionChartExporter.ToWordXml);
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnExportAsXLingPaper(object args)
        {
            var fmt = LocalizationManager.GetString("Views.DistributionChart.DefaultXLingPaperExportFileAffix",
                "{0}-{1}DistributionChart-(XLingPaper).xml");

            return Export(ResultViewManger.XLingPaperExport, fmt, App.kstidFileTypeXLingPaper, "xml",
                Settings.Default.OpenXLingPaperDistChartAfterExport, DistributionChartExporter.ToXLingPaper);
        }

        /// ------------------------------------------------------------------------------------
        protected bool OnUpdateExportAsHTML(object args)
        {
            var itemProps = args as TMItemProperties;
            if (itemProps == null || !ActiveView)
                return false;

            bool enable = (ObjectForExport != null);
            itemProps.Enabled = enable;
            itemProps.Visible = true;
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

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Determines which object in the view should be exported to HTML, the XY chart grid,
        /// or one of the search result word lists, if there are any.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private object ObjectForExport
        {
            get
            {
                // If a search result grid has focus, it wins the contest.
                if (ResultViewManger.CurrentViewsGrid != null && ResultViewManger.CurrentViewsGrid.Focused)
                    return ResultViewManger.CurrentViewsGrid;

                // Otherwise the grid does if it's not empty.
                return (!_grid.IsEmpty ? _grid : null);
            }
        }

        public bool Moving
        {
            get { return _moving; }
            set { _moving = value; }
        }

        public int MoveColumnIndex
        {
            get { return _moveColumnIndex; }
            set { _moveColumnIndex = value; }
        }

        #endregion

        #region Grid Events and methods

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_grid.Cursor == Cursors.Arrow)
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    // If the mouse moves outside the rectangle, start the drag.
                    if (_dragBoxFromMouseDown != Rectangle.Empty &&
                        !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {

                        // Proceed with the drag and drop, passing in the list item.                   
                        DragDropEffects dropEffect = _grid.DoDragDrop(
                            _grid.Rows[_rowIndexFromMouseDown],
                            DragDropEffects.Move);
                    }
                }

                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    // If the mouse moves outside the rectangle, start the drag.
                    if (_dragBoxFromMouseDown != Rectangle.Empty &&
                        !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                    {
                        // Proceed with the drag and drop, passing in the list item.                   
                        DragDropEffects dropEffect = _grid.DoDragDrop(
                            _grid.Columns[_columnIndexFromMouseDown],
                            DragDropEffects.Move);
                    }

                }
            }
        }

        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            _movingDouble = false;
            // Get the index of the item the mouse is below.
            _rowIndexFromMouseDown = _grid.HitTest(e.X, e.Y).RowIndex;

            if (_rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.               
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                    e.Y - (dragSize.Height / 2)),
                    dragSize);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragBoxFromMouseDown = Rectangle.Empty;
            }


            // Get the index of the item the mouse is below.
            _columnIndexFromMouseDown = _grid.HitTest(e.X, e.Y).ColumnIndex;

            // decide which column can't be reorder
            if (_columnIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred.
                // The DragSize indicates the size that the mouse can move
                // before a drag event should be started.               
                Size dragSize = SystemInformation.DragSize;
                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                e.Y - (dragSize.Height / 2)),
                dragSize);

                if (_columnIndexFromMouseDown == 0)
                {
                    _movingDouble = true;
                }

            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            // get the hovered column index
            // how to deal wiht if mouse is not in a client area? (not finished)
            Point currentPoint = _grid.PointToClient(new Point(e.X, e.Y));

            MoveColumnIndex = _grid.HitTest(currentPoint.X, currentPoint.Y).ColumnIndex;
            Moving = true;

            // force datagridview to repaint to draw the rectangle
            _grid.Refresh();
        }

        private void Grid_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be
            // converted to client coordinates.
            Point clientPoint = _grid.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below.
            _rowIndexOfItemUnderMouseToDrop = _grid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            //// Get the column index of the item the mouse is below.
            _columnIndexOfItemUnderMouseToDrop = _grid.HitTest(clientPoint.X, clientPoint.Y).ColumnIndex;


            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                if (_rowIndexOfItemUnderMouseToDrop != 0)
                {
                    DataGridViewRow rowToMove = e.Data.GetData(
                                 typeof(DataGridViewRow)) as DataGridViewRow;
                    if (rowToMove.Index != -1 && _rowIndexFromMouseDown != -1 && _rowIndexOfItemUnderMouseToDrop != -1 && _grid.Rows.Count - 1 != _rowIndexOfItemUnderMouseToDrop && _grid.Rows.Count - 1 != _rowIndexFromMouseDown)
                    {
                        _grid.Rows.RemoveAt(_rowIndexFromMouseDown);
                        _grid.Rows.Insert(_rowIndexOfItemUnderMouseToDrop, rowToMove);
                    }
                }
            }


            Moving = false;
            if (_columnIndexOfItemUnderMouseToDrop > 1)
            {
                // Get the column index of the item the mouse is below.
                int newcolumnIndex = _grid.HitTest(clientPoint.X, clientPoint.Y).ColumnIndex;

                _columnIndexOfItemUnderMouseToDrop = _grid.Columns[newcolumnIndex].DisplayIndex;

                // If the drag operation was a move then remove and insert the column.
                if (e.Effect == DragDropEffects.Move)
                {
                    //DataGridViewColumn columnToMove = e.Data.GetData(
                    //typeof(DataGridViewColumn)) as DataGridViewColumn;

                    // change the display index
                    // It should be consided more. If the target column is last column.(not finished)
                    if (_movingDouble)
                    {
                        _grid.Columns[0].DisplayIndex = _columnIndexOfItemUnderMouseToDrop + 1;
                        _grid.Columns[1].DisplayIndex = _columnIndexOfItemUnderMouseToDrop + 1;
                    }
                    else
                    {
                        //_grid.Columns[_columnIndexFromMouseDown].DisplayIndex = _columnIndexOfItemUnderMouseToDrop;
                        DataGridViewColumn tempcolumn = _grid.Columns[_columnIndexFromMouseDown];
                        _grid.Columns.RemoveAt(_columnIndexFromMouseDown);
                        _grid.Columns.Insert(_columnIndexOfItemUnderMouseToDrop, tempcolumn);

                    }
                }
            }
            _grid.ChartLayout = DistributionChart.ModifyFromDistributionGrid(_grid);
            SaveCurrentChart(_grid.ChartLayout);
        }

        private void Grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_grid.IsCurrentCellDirty)
            {
                _grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void Grid_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                Console.WriteLine("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                Console.WriteLine("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                Console.WriteLine("Parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                Console.WriteLine("Leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }

        #endregion
    }
}