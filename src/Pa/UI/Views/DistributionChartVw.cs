using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
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
	public partial class DistributionChartVw : UserControl, IxCoreColleague, ITabView, ISearchResultsViewHost
	{
		public const string kSavedChartsFile = "DistributionCharts.xml";

		private bool m_initialDock = true;
		private bool m_editingSavedChartName;
		private SlidingPanel m_slidingPanel;
		private List<DistributionChartLayout> m_savedCharts;
		private ITMAdapter m_tmAdapter;
		private readonly string m_openClass = App.kOpenClassBracket;
		private readonly string m_closeClass = App.kCloseClassBracket;
		private readonly SplitterPanel m_dockedSidePanel;
		private readonly DistributionGrid m_grid;
		private readonly Keys m_saveChartHotKey = Keys.None;

		/// ------------------------------------------------------------------------------------
		public DistributionChartVw()
		{
			var msg = App.GetString("DistributionChartVw.InitializingViewMsg",
				"Initializing Distribution Chart View...",
				"Message displayed whenever the distribution chart view is being initialized.");

			App.InitializeProgressBarForLoadingView(msg, 6);
			InitializeComponent();
			Name = "XYChartVw";
			App.IncProgressBar();

			hlblSavedCharts.TextFormatFlags &= ~TextFormatFlags.HidePrefix;

			m_grid = new DistributionGrid();
			m_grid.OwningView = this;
			m_grid.Dock = DockStyle.Fill;
			m_grid.TabIndex = lblChartName.TabIndex + 1;
			m_grid.KeyDown += HandleGridKeyDown;
			m_grid.CellMouseDoubleClick += HandleGridCellMouseDoubleClick;
			App.IncProgressBar();

			LoadToolbarAndContextMenus();
			m_grid.TMAdapter = m_tmAdapter;
			m_grid.OwnersNameLabelControl = lblChartNameValue;
			App.IncProgressBar();

			SetupSidePanelContents();
			App.IncProgressBar();
			SetupSlidingPanel();
			OnPaFontsChanged(null);
			App.IncProgressBar();

			m_dockedSidePanel = (m_slidingPanel.SlideFromLeft ? splitOuter.Panel1 : splitOuter.Panel2);
			
			LoadSettings();
			m_grid.Reset();
			splitChart.Panel1.Controls.Add(m_grid);
			m_grid.BringToFront();
			splitChart.Panel1MinSize = m_grid.Top + 10;
			splitChart.Panel2Collapsed = true;
			App.IncProgressBar();

			UpdateButtons();
			App.UninitializeProgressBar();

			base.DoubleBuffered = true;
			Disposed += ViewDisposed;

			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("tbbSaveChartOnMenu");
			if (itemProps != null)
				m_saveChartHotKey = itemProps.ShortcutKey;

			lblChartNameValue.Left = lblChartName.Right + 10;
		}

		/// ------------------------------------------------------------------------------------
		void ViewDisposed(object sender, EventArgs e)
		{
			Disposed -= ViewDisposed;

			if (m_grid != null && !m_grid.IsDisposed)
				m_grid.Dispose();

			if (ptrnBldrComponent != null && !ptrnBldrComponent.IsDisposed)
				ptrnBldrComponent.Dispose();

			if (ResultViewManger != null)
				ResultViewManger.Dispose();

			if (splitOuter != null && !splitOuter.IsDisposed)
				splitOuter.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadToolbarAndContextMenus()
		{
			if (m_tmAdapter != null)
			{
				App.UnPrepareAdapterForLocalizationSupport(m_tmAdapter);
				m_tmAdapter.Dispose();
			}

			m_tmAdapter = AdapterHelper.CreateTMAdapter();

			if (ResultViewManger != null)
				ResultViewManger.TMAdapter = m_tmAdapter;
			else
			{
				ResultViewManger = new SearchResultsViewManager(this, m_tmAdapter,
					splitResults, rtfRecVw, Settings.Default.DistChartVwPlaybackSpeed,
					newSpeed => Settings.Default.DistChartVwPlaybackSpeed = newSpeed);
			}

			if (m_tmAdapter == null)
				return;

			App.PrepareAdapterForLocalizationSupport(m_tmAdapter);
			m_tmAdapter.LoadControlContainerItem += m_tmAdapter_LoadControlContainerItem;

			var defs = new[] { FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
				"XYChartsTMDefinition.xml") };

			m_tmAdapter.Initialize(this, App.MsgMediator, App.ApplicationRegKeyPath, defs);
			m_tmAdapter.AllowUpdates = true;
			m_tmAdapter.SetContextMenuForControl(m_grid, "cmnuXYChart");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hand off a couple of drop-down controls to the toobar/menu adapter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Control m_tmAdapter_LoadControlContainerItem(string name)
		{
			if (name == "tbbSearchOptionsDropDown")
				return m_grid.SearchOptionsDropDown;

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
			if (msg.Msg == 0x100 && keyData == m_saveChartHotKey && !m_editingSavedChartName)
			{
				// Make sure the button is enabled.
				TMItemProperties itemProps = m_tmAdapter.GetItemProperties("tbbSaveChart");
				if (itemProps != null && itemProps.Enabled)
				{
					App.MsgMediator.SendMessage("SaveChart", null);
					return true;
				}
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
				return App.GetString("DistributionChartVw.FillChartMsg",
					"You must first choose 'Fill Chart' before seeing search results.",
					"Views");
			}
		}

		#region Loading and saving charts
		/// ------------------------------------------------------------------------------------
		private void LoadSavedChartsList()
		{
			var filename = App.Project.ProjectPathFilePrefix + kSavedChartsFile;
			var oldFileName = App.Project.ProjectPathFilePrefix + "XYCharts.xml";
			
			// Migrate the old file to the new one and rename the old one.
			if (File.Exists(oldFileName))
			{
				try
				{
					File.Copy(oldFileName, filename);
					File.Move(oldFileName, Path.ChangeExtension(oldFileName, "old"));
				}
				catch
				{
					return;
				}
			}

			m_savedCharts = XmlSerializationHelper.DeserializeFromFile<List<DistributionChartLayout>>(filename);
			if (m_savedCharts == null)
				return;

			foreach (DistributionChartLayout layout in m_savedCharts)
			{
				ListViewItem item = new ListViewItem(layout.Name);
				item.Tag = layout;
				lvSavedCharts.Items.Add(item);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SaveCharts()
		{
			if (m_savedCharts != null)
			{
				var filename = App.Project.ProjectPathFilePrefix + kSavedChartsFile;
				XmlSerializationHelper.SerializeToFile(filename, m_savedCharts);
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
		private DistributionChartLayout GetExistingLayoutByName(DistributionChartLayout layoutToSkip,
			string nameToCheck, bool showMsg)
		{
			// Check if chart name already exists. If it does,
			// tell the user and don't cancel the current edit.
			foreach (DistributionChartLayout savedLayout in m_savedCharts)
			{
				if (savedLayout != layoutToSkip && savedLayout.Name == nameToCheck)
				{
					if (showMsg)
					{
						string msg = string.Format(
							Properties.Resources.kstidSavedChartNameAlreadyExistsMsg, nameToCheck);
						Utils.MsgBox(msg);
					}
					
					return savedLayout;
				}
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

			m_slidingPanel = new SlidingPanel(this, splitSideBarOuter, pnlSliderPlaceholder,
				Settings.Default.DistChartVwSidePanelWidth,
				newWidth => Settings.Default.DistChartVwSidePanelWidth = newWidth);

			App.GetStringForObject(m_slidingPanel.Tab,
				"DistributionChartVw.UndockedSideBarTabText", "Charts & Chart Building",
				"Text on vertical tab when the side bar is undocked in the distribution charts view.");

			Controls.Add(m_slidingPanel);
			splitOuter.BringToFront();
		}

		#endregion

		#region Loading and saving settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadSettings()
		{
			ptrnBldrComponent.LoadSettings(Name);

			if (Settings.Default.DistChartVwSidePanelDocked)
				btnDock_Click(null, null);
			else
				btnAutoHide_Click(null, null);

			OnViewDocked(this);
			m_initialDock = true;
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
			get { return m_tmAdapter; }
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
			Settings.Default.DistChartVwSidePanelDocked = m_slidingPanel.SlideFromLeft ?
				!splitOuter.Panel1Collapsed : !splitOuter.Panel2Collapsed;

			ptrnBldrComponent.SaveSettings(Name);
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
				if (m_initialDock)
				{
					m_initialDock = false;
					m_grid.Focus();
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

			// Let the sliding panel control handle most of the undocking/hiding procedure.
			m_slidingPanel.UnDockControl(m_dockedSidePanel);

			// Collapse the panel in which the side panel was docked.
			if (m_slidingPanel.SlideFromLeft)
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

			if (m_grid.IsEmpty || col <= 0 || row <= 0 || m_grid[col, row].Value is SearchQueryException)
				return;

			if (m_grid.IsCurrentCellValidForSearch)
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
			Point pt = m_grid.CurrentCellAddress;

			if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None || m_grid.IsEmpty ||
				pt.X <= 0 || pt.Y <= 0 || pt.X == m_grid.Columns.Count - 1 ||
				pt.Y == m_grid.NewRowIndex)
			{
				return;
			}

			if (m_grid.IsCurrentCellValidForSearch)
				Search(m_grid.CurrentCell, SearchResultLocation.CurrentTabGroup);
			else
				Utils.MsgBox(FillChartMessage);

			e.Handled = true;
		}

		#endregion

		#region Non DragDrop keyboard and mouse events for inserting text into grid
		/// ------------------------------------------------------------------------------------
		private void HandleFeatureListCustomDoubleClick(object sender, string feature)
		{
			m_grid.InsertTextInCell(feature);
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
				m_grid.InsertTextInCell(text);
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
			m_grid.InsertTextInCell(item.Text.Replace(App.kDottedCircle, string.Empty));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts phones into the search pattern when they're clicked on from the vowel or
		/// consonant tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleVowConClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			m_grid.InsertTextInCell(e.ClickedItem.Text);
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
					m_grid.InsertTextInCell((
						item.Pattern == null || Settings.Default.ShowClassNamesInSearchPatterns ?
						m_openClass + item.Text + m_closeClass : item.Pattern));
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
			else if (e.Item is CharGridCell)
				dragText = ((CharGridCell)e.Item).Phone;
			else if (sender is FeatureListView)
				dragText = ((FeatureListView)sender).CurrentFormattedFeature;
			else if (e.Item is ClassListViewItem)
			{
				var item = e.Item as ClassListViewItem;
				dragText = (item.Pattern == null || Settings.Default.ShowClassNamesInSearchPatterns ?
					m_openClass + item.Text + m_closeClass : item.Pattern);
			}

			// Any text we begin dragging.
			if (dragText != null)
			{
				if (m_slidingPanel.Visible)
					m_slidingPanel.Close(true);
	
				DoDragDrop(dragText.Replace(App.kDottedCircle, string.Empty), DragDropEffects.Copy);
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

			var msg = Properties.Resources.kstidConfirmSavedChartRemoval;
			if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.No)
				return;

			var layout = lvSavedCharts.SelectedItems[0].Tag as DistributionChartLayout;

			if (layout != null)
			{
				if (layout == m_grid.ChartLayout ||
					(m_grid.ChartLayout != null && layout.Name == m_grid.ChartLayout.Name))
				{
					// Don't delete the m_xyGrid.ChartLayout if the saved chart name is in edited mode
					if (layout.Name != null)
					{
						m_grid.Reset();
						m_grid.ChartLayout = null;
					}
				}

				m_savedCharts.Remove(layout);
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
			
			if (m_slidingPanel.Visible)
				m_slidingPanel.Close(true);

			m_grid.LoadFromLayout(item.Tag as DistributionChartLayout);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvSavedCharts_BeforeLabelEdit(object sender, LabelEditEventArgs e)
		{
			m_editingSavedChartName = true;
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

			DistributionChartLayout layout = lvSavedCharts.Items[e.Item].Tag as DistributionChartLayout;
			if (layout == null || layout.Name == newName || newName == null)
			{
				e.CancelEdit = true;
				m_editingSavedChartName = false;
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
			if (m_grid.ChartLayout != null &&
				m_grid.ChartLayout.Name == lvSavedCharts.Items[e.Item].Text)
			{
				m_grid.ChartLayout.Name = newName;
				lblChartNameValue.Text = newName;
			}

			// Keep the new name and save the change to disk.
			layout.Name = newName;
			SaveCharts();

			lvSavedCharts.Items[e.Item].Text = newName;
			e.CancelEdit = true;
			m_editingSavedChartName = false;
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
				!(item.Tag is DistributionChartLayout))
				return;

			if (m_slidingPanel.Visible)
				m_slidingPanel.Close(true);

			DoDragDrop(item.Tag as DistributionChartLayout, DragDropEffects.Move);
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
			if (!m_grid.IsCellValidForSearch(row, col))
				return;

			m_grid.RefreshCellValue(row, col);

			SearchQuery query = m_grid.GetCellsFullSearchQuery(row, col);
			if (query != null)
				ResultViewManger.PerformSearch(query, resultLocation);
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
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShouldMenuBeEnabled(string menuName)
		{
			return m_grid.IsCurrentCellValidForSearch;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SearchQuery GetQueryForMenu(string menuName)
		{
			return m_grid.CurrentCellsFullSearchQuery;
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
			PaWordListGrid grid = args as PaWordListGrid;
			return (grid != null && ResultViewManger.CurrentViewsGrid == grid);
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

			rtfRecVw.UpdateFonts();
			ptrnBldrComponent.RefreshFonts();
			m_slidingPanel.RefreshFonts();

			// Return false to allow other windows to update their fonts.
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
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

			m_grid.FillChart();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fills the grid with search result totals.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateFillInChart(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !ActiveView)
				return false;

			if (itemProps.Enabled == m_grid.IsEmpty)
			{
				itemProps.Visible = true;
				itemProps.Enabled = !m_grid.IsEmpty;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSaveChart(object args)
		{
			if (!ActiveView)
				return false;

			// Commit changes and end the edit mode if necessary. Fixes PA-714
			if (m_grid.IsCurrentCellInEditMode)
				m_grid.EndEdit();

			if (!m_grid.IsDirty)
				return false;

			// If the name isn't specified, then use the save as dialog.
			if (string.IsNullOrEmpty(m_grid.ChartName))
				return OnSaveChartAs(args);

			SaveCurrentChart(m_grid.ChartLayout);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnSaveChartAs(object args)
		{
			if (!ActiveView)
				return false;

			// Commit changes and end the edit mode if necessary. Fixes PA-714
			if (m_grid.IsCurrentCellInEditMode)
				m_grid.EndEdit();

			using (SaveDistributionChartDlg dlg = new SaveDistributionChartDlg(m_grid, m_savedCharts))
			{
				if (dlg.ShowDialog() == DialogResult.OK)
					SaveCurrentChart(dlg.LayoutToOverwrite);
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
		private void SaveCurrentChart(DistributionChartLayout layoutToOverwrite)
		{
			ListViewItem item = null;

			// When the user wants to overwrite an existing layout, we need to find the
			// item in the saved list that corresponds to the one being overwritten.
			if (layoutToOverwrite != null)
			{
				foreach (ListViewItem lvi in lvSavedCharts.Items)
				{
					DistributionChartLayout tmpLayout = lvi.Tag as DistributionChartLayout;
					if (tmpLayout != null && tmpLayout.Name == layoutToOverwrite.Name)
					{
						item = lvi;
						break;
					}
				}
			}

			if (m_savedCharts == null)
				m_savedCharts = new List<DistributionChartLayout>();
			
			DistributionChartLayout layoutCopy = m_grid.ChartLayout.Clone();

			if (item != null)
			{
				// Overwrite an existing layout.
				int i = m_savedCharts.IndexOf(item.Tag as DistributionChartLayout);
				m_savedCharts[i] = layoutCopy;
				item.Tag = layoutCopy;
				item.Text = layoutCopy.Name;
			}
			else
			{
				// Save a new layout.
				m_savedCharts.Add(layoutCopy);
				item = new ListViewItem(layoutCopy.Name);
				item.Tag = layoutCopy;
				lvSavedCharts.Items.Add(item);
			}

			SaveCharts();
			m_grid.LoadFromLayout(layoutCopy);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateSaveChart(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !ActiveView)
				return false;

			if (itemProps.Enabled != (m_grid.ChartLayout != null))
			{
				itemProps.Visible = true;
				itemProps.Enabled = (m_grid.ChartLayout != null);
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

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null && itemProps.Name.StartsWith("cmnu"))
				return false;

			Search(m_grid.CurrentCell, SearchResultLocation.CurrentTabGroup);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateBeginSearch(object args)
		{
			if (!ActiveView)
				return false;

			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps != null && !itemProps.Name.StartsWith("cmnu"))
			{
				if (itemProps.Enabled != m_grid.IsCurrentCellValidForSearch)
				{
					itemProps.Visible = true;
					itemProps.Enabled = m_grid.IsCurrentCellValidForSearch;
					itemProps.Update = true;
				}
				
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the RtfExportDlg form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsRTF(object args)
		{
			if (!ActiveView)
				return false;

			RtfExportDlg rtfExp = new RtfExportDlg(ResultViewManger.CurrentViewsGrid);
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
			TMItemProperties itemProps = args as TMItemProperties;
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
			var fmt = App.GetString("DefaultDistributionChartHtmlExportFileAffix",
				"{0}-{1}DistributionChart.html", "Export");

			return Export(ResultViewManger.HTMLExport, fmt, App.kstidFileTypeHTML, "html",
				Settings.Default.OpenHtmlDistChartAfterExport, DistributionChartExporter.ToHtml);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool Export(Func<string> wordListExportAction, string fmtFileName,
			string fileTypeFilter, string defaultFileType, bool openAfterExport,
			Func<PaProject, string, DistributionGrid, bool, bool> exportAction)
		{
			if (!ActiveView)
				return false;

			object objForExport = ObjectForExport;

			// Determine whether to export the XY Chart or a search result word list.
			if (!(objForExport is DistributionGrid))
				return (wordListExportAction != null);

			var prefix = (App.Project.LanguageName ?? (App.Project.LanguageCode ?? App.Project.Name));
			var defaultFileName = string.Format(fmtFileName, prefix, m_grid.ChartName).Replace(" ", string.Empty);

			string fileTypes = fileTypeFilter + "|" + App.kstidFileTypeAllFiles;

			int filterIndex = 0;
			var outputFileName = App.SaveFileDialog(defaultFileType, fileTypes, ref filterIndex,
				App.kstidSaveFileDialogGenericCaption, defaultFileName, App.Project.Folder);

			if (string.IsNullOrEmpty(outputFileName))
				return false;

			exportAction(App.Project, outputFileName, m_grid, openAfterExport);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsWordXml(object args)
		{
			var fmt = App.GetString("DefaultDistributionChartWordXmlExportFileAffix",
				"{0}-{1}DistributionChart-(Word).xml", "Export");

			return Export(ResultViewManger.WordXmlExport, fmt, App.kstidFileTypeWordXml, "xml",
				Settings.Default.OpenWordXmlDistChartAfterExport, DistributionChartExporter.ToWordXml);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsXLingPaper(object args)
		{
			var fmt = App.GetString("DefaultDistributionChartXLingPaperExportFileAffix",
				"{0}-{1}DistributionChart-(XLingPaper).xml", "Export");

			return Export(ResultViewManger.XLingPaperExport, fmt, App.kstidFileTypeXLingPaper, "xml",
				Settings.Default.OpenXLingPaperDistChartAfterExport, DistributionChartExporter.ToXLingPaper);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsHTML(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !ActiveView)
				return false;

			bool enable = (ObjectForExport != null);
			itemProps.Enabled = enable;
			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
				return (!m_grid.IsEmpty ? m_grid : null);
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
}