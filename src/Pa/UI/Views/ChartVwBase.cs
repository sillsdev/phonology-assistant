using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SilTools;
using SilTools.Controls;

namespace SIL.Pa.UI.Views
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides a base window class for vowel, consonant, diacritic and suprasegmental
	/// character charts.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ChartVwBase : UserControl, IxCoreColleague, ITabView
	{
		protected List<CharGridCell> _phoneList;
		protected ITMAdapter _tmAdapter;
		protected ChartOptionsDropDown _chartOptionsDropDown;

		protected CVChartGrid _newChartGrid;
		protected SilPanel _pnlGrid;
		protected WebBrowser _htmlVw;
		protected PaProject _project;

		private string _persistedInfoFilename;
		private bool _histogramOn = true;
		private bool _initialDock = true;
		private bool _activeView;

		/// ------------------------------------------------------------------------------------
		public ChartVwBase(PaProject project)
		{
			_project = project;
			InitializeComponent();

			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
				return;

			Utils.WaitCursors(true);
			base.DoubleBuffered = true;
			
			LoadToolbarAndContextMenus();
			_oldChrGrid.OwningViewType = GetType();

			_newChartGrid = new CVChartGrid(_tmAdapter);
			_newChartGrid.Dock = DockStyle.Fill;
			_newChartGrid.GridColor = ChartGridColor;
			_pnlGrid = new SilPanel();
			_pnlGrid.Dock = DockStyle.Fill;
			_pnlGrid.Controls.Add(_newChartGrid);

			_htmlVw = new WebBrowser();
			_htmlVw.Dock = DockStyle.Fill;
			_htmlVw.Visible = false;
			_htmlVw.AllowWebBrowserDrop = false;
			_pnlGrid.Controls.Add(_htmlVw);
			
			_oldChrGrid.Visible = false;
			_splitOuter.Panel1.Controls.Add(_pnlGrid);
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		public virtual CVChartType ChartType
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual Color ChartGridColor
		{
			get { return _newChartGrid.GridColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int ColumnHeaderHeight
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int RowHeaderWidth
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string DefaultHTMLOutputFile
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string DefaultWordXmlOutputFile
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string DefaultXLingPaperOutputFile
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool WasLastChartViewHTML
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected void DeleteHtmlChartFile(string chartAffix)
		{
			try
			{
				File.Delete(_project.ProjectPathFilePrefix + chartAffix + ".html");
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void LoadOldChart()
		{
			var bldr = new CharGridBuilder(_oldChrGrid, CharacterType);
			_phoneList = bldr.Build();
			_persistedInfoFilename = bldr.PersistedInfoFilename;

			// This should only be null when something has gone wrong...
			// which should never happen. :o)
			if (_phoneList == null)
				return;

			// Create a list of phones for a histogram based on the order of the
			// phones as they appear in the grid (from left to right, top to bottom).
			List<CharGridCell> histogramPhones = new List<CharGridCell>();
			for (int iCol = 0; iCol < _oldChrGrid.Grid.Columns.Count; iCol++)
			{
				for (int iRow = 0; iRow < _oldChrGrid.Grid.Rows.Count; iRow++)
				{
					var cgc = _oldChrGrid.Grid[iCol, iRow].Value as CharGridCell;
					if (cgc != null)
						histogramPhones.Add(cgc);
				}
			}

			_histogram.LoadPhones(histogramPhones);
			App.MsgMediator.PostMessage("LayoutHistogram", Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the newer version of the C or V chart (i.e. the one built purely from phone
		/// features and which cannot be customized except by changing phone features).
		/// This version of the chart is displayed by default and the old one may eventually
		/// go away, leaving this one only (in which case, it will no longer be called "new").
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void LoadChart()
		{
			_newChartGrid.ClearAll();

			var cgp = XmlSerializationHelper.DeserializeFromFile<CharGridPersistence>(LayoutFile);

			foreach (var col in cgp.ColHeadings)
				_newChartGrid.AddColumnGroup(col.HeadingText);

			foreach (var row in cgp.RowHeadings)
				_newChartGrid.AddRowGroup(row.HeadingText, row.SubHeadingCount);

			foreach (var phone in cgp.Phones)
				_newChartGrid[phone.Column, phone.Row].Value = phone.Phone;

			if (ColumnHeaderHeight > 0)
				_newChartGrid.ColumnHeadersHeight = ColumnHeaderHeight;
			else
				_newChartGrid.AdjustColumnHeaderHeight();

			if (RowHeaderWidth > 0)
				_newChartGrid.RowHeadersWidth = RowHeaderWidth;

			_newChartGrid.AdjustCellSizes();

			// Do this to make sure the message mediator is hooked up for
			// the toolbar/menu items.
			if (!_oldChrGrid.IsHandleCreated)
				_oldChrGrid.CreateControl();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadHtmlChart()
		{
			var outputFile = CreateHtmlViewFile();
			_htmlVw.Url = new Uri(File.Exists(outputFile) ? outputFile : "about:blank");
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Flip between the old chart and the new using Ctrl+Alt+Left or Ctrl+Alt+Right.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt && (keyData & Keys.Control) == Keys.Control &&
				(keyData & Keys.Left) == Keys.Left || (keyData & Keys.Right) == Keys.Right)
			{
				_pnlGrid.Visible = !_pnlGrid.Visible;
				_oldChrGrid.Visible = !_oldChrGrid.Visible;

				if (_oldChrGrid.Visible)
					_oldChrGrid.Focus();
				else
					_newChartGrid.Focus();
				return true;
			}

			return base.ProcessDialogKey(keyData);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool ShowHtmlChartWhenViewLoaded
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
			set { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string CreateHtmlViewFile()
		{
			throw new NotImplementedException("The method must be overridden in derived class.");
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string LayoutFile
		{
			get { throw new NotImplementedException("The property must be overridden in derived class."); }
		}

		/// ------------------------------------------------------------------------------------
		private void ShowHtmlChart(bool show)
		{
			_oldChrGrid.Visible = false;
			_pnlGrid.Visible = true;

			_htmlVw.Visible = show;
			_newChartGrid.Visible = !show;

			if (_htmlVw.Visible)
				_htmlVw.Focus();
			else
				_newChartGrid.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sometimes the histogram is initially shown without its horizontal scroll bar.
		/// This will force it to be displayed if it needs to be and this is done after all
		/// loading and layout is done -- which is the only place I've found when this will
		/// work.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnLayoutHistogram(object args)
		{
			if (_histogramOn && (args as string) == Name)
				_histogram.ForceLayout();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads a chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneMoved(object args)
		{
			try
			{
				var argArray = args as object[];
				if (argArray[0] == _oldChrGrid)
				{
					CharGridPersistence.Save(_oldChrGrid, _phoneList, _persistedInfoFilename);
					App.MsgMediator.SendMessage("PhoneChartArrangementChanged", CharacterType);
				}
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads a chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ReloadChart(bool restoreDefault)
		{
			LoadChart();
			LoadHtmlChart();

			if (restoreDefault)
				File.Delete(_persistedInfoFilename);
			else
				CharGridPersistence.Save(_oldChrGrid, _phoneList, _persistedInfoFilename);
			
			_oldChrGrid.Reset();
			LoadOldChart();
			_oldChrGrid.ForceCurrentCellUpdate();
			CharGridPersistence.Save(_oldChrGrid, _phoneList, _persistedInfoFilename);
			App.MsgMediator.SendMessage("PhoneChartArrangementChanged", CharacterType);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadToolbarAndContextMenus()
		{
			if (App.DesignMode)
				return;

			if (_tmAdapter != null)
			{
				App.UnPrepareAdapterForLocalizationSupport(_tmAdapter);
				_tmAdapter.Dispose();
			}

			_tmAdapter = AdapterHelper.CreateTMAdapter();
			_oldChrGrid.TMAdapter = _tmAdapter;

			if (_tmAdapter != null)
			{
				App.PrepareAdapterForLocalizationSupport(_tmAdapter);
				_tmAdapter.LoadControlContainerItem += m_tmAdapter_LoadControlContainerItem;
				var defs = new[] { FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName,
					"CVChartsTMDefinition.xml") };

				_tmAdapter.Initialize(this, App.MsgMediator, App.ApplicationRegKeyPath, defs);
				_tmAdapter.AllowUpdates = true;
			}

			// Give the chart Phone search toolbar button a default image.
			var childItemProps = _tmAdapter.GetItemProperties("tbbChartPhoneSearchAnywhere");
			var parentItemProps = _tmAdapter.GetItemProperties("tbbChartPhoneSearch");
			if (parentItemProps != null && childItemProps != null)
			{
				parentItemProps.Image = childItemProps.Image;
				parentItemProps.Visible = true;
				parentItemProps.Update = true;
				_tmAdapter.SetItemProperties("tbbChartPhoneSearch", parentItemProps);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Give the adapter the chars. to ignore drop-down control. We know that's the only
		/// control the adapter will request for this form. So there's no need to check the
		/// name passed to us.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Control m_tmAdapter_LoadControlContainerItem(string name)
		{
			_chartOptionsDropDown = new ChartOptionsDropDown(_oldChrGrid.SupraSegsToIgnore);
			_chartOptionsDropDown.lnkRefresh.Click += HandleRefreshChartClick;
			return _chartOptionsDropDown;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void BuildDefaultChart()
		{
			throw new NotImplementedException("The method must be overridden in derived class.");
		}

		#region ITabView Members
		/// ------------------------------------------------------------------------------------
		public bool ActiveView
		{
			get { return _activeView; }
		}

		/// ------------------------------------------------------------------------------------
		public void SetViewActive(bool makeActive, bool isDocked)
		{
			_activeView = makeActive;

			if (_activeView && isDocked && _oldChrGrid != null && _oldChrGrid.Grid != null)
			{
				if (_oldChrGrid.Visible)
					_oldChrGrid.Grid.Focus();
				else
					_newChartGrid.Focus();
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
		protected virtual float SplitterRatioSetting
		{
			get { throw new NotImplementedException("This property must be overridden"); }
			set { throw new NotImplementedException("This property must be overridden"); }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool HistogramVisibleSetting
		{
			get { throw new NotImplementedException("This property must be overridden"); }
			set { throw new NotImplementedException("This property must be overridden"); }
		}

		/// ------------------------------------------------------------------------------------
		public void SaveSettings()
		{
			CharGridPersistence.Save(_oldChrGrid, _phoneList, _persistedInfoFilename);
			SplitterRatioSetting = _splitOuter.SplitterDistance / (float)_splitOuter.Height;
			HistogramVisibleSetting = HistogramOn;
			ShowHtmlChartWhenViewLoaded = _htmlVw.Visible;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewClosing(object args)
		{
			if (args == this)
				SaveSettings();

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewUnDocking(object args)
		{
			if (args == this)
			{
				_oldChrGrid.Grid.SetDoubleBuffering(false);
				SaveSettings();
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnViewUndocked(object args)
		{
			if (args == this)
				_oldChrGrid.Grid.SetDoubleBuffering(true);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewDocking(object args)
		{
			if (args == this && IsHandleCreated)
			{
				SaveSettings();
				_oldChrGrid.Grid.SetDoubleBuffering(false);
			}

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
					_splitOuter.SplitterDistance = (int)(_splitOuter.Height * SplitterRatioSetting);
				}
				catch { }

				_oldChrGrid.Grid.SetDoubleBuffering(true);

				// Don't need to load the tool bar or menus if this is the first time
				// the view was docked since that all gets done during construction.
				if (_initialDock)
					_initialDock = false;
				else
				{
					// The toolbar has to be recreated each time the view is removed from it's
					// (undocked) form and docked back into the main form. The reason has to
					// do with tooltips. They seem to form an attachment, somehow, with the
					// form that owns the controls the tooltip is extending. When that form
					// gets pulled out from under the tooltips, sometimes the program will crash.
					LoadToolbarAndContextMenus();
				}

				_histogram.RefreshLayout();
			}

			return false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public bool HistogramOn
		{
			get { return _histogramOn; }
			set
			{
				if (_histogramOn != value)
				{
					_histogramOn = value;
					_splitOuter.Panel2Collapsed = !value;
					var padding = _splitOuter.Panel1.Padding;
					padding = new Padding(padding.Left, padding.Top, padding.Right,
						(value ? 0 : _splitOuter.Panel2.Padding.Bottom));
					_splitOuter.Panel1.Padding = padding;

					if (value)
						_histogram.ForceLayout();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load the form's settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (App.DesignMode)
				return;

			HistogramOn = HistogramVisibleSetting;
			_oldChrGrid.Reset();
			LoadOldChart();
			LoadChart();
			LoadHtmlChart();

			OnViewDocked(this);
			_initialDock = true;
			App.UninitializeProgressBar();

			if (ShowHtmlChartWhenViewLoaded)
				ShowHtmlChart(true);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Handle Moving a row up.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void btnMoveRowUp_Click(object sender, EventArgs e)
		//{
		//    PaApp.MsgMediator.SendMessage("MoveCharChartRowUp", null);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Handle moving a row down.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void btnMoveRowDown_Click(object sender, EventArgs e)
		//{
		//    PaApp.MsgMediator.SendMessage("MoveCharChartRowDown", null);
		//}

		#region Phone searching methods and searching command message/update handlers
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchAnywhere(object args)
		{
			if (!_activeView)
				return false;

			PerformSearch("*_*", "tbbChartPhoneSearchAnywhere");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchInitial(object args)
		{
			if (!_activeView)
				return false;

			PerformSearch("#_+", "tbbChartPhoneSearchInitial");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchMedial(object args)
		{
			if (!_activeView)
				return false;

			PerformSearch("+_+", "tbbChartPhoneSearchMedial");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchFinal(object args)
		{
			if (!_activeView)
				return false;

			PerformSearch("+_#", "tbbChartPhoneSearchFinal");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchAlone(object args)
		{
			if (!_activeView)
				return false;
				
			PerformSearch("#_#", "tbbChartPhoneSearchAlone");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void PerformSearch(string environment, string toolbarItemName)
		{
			string[] srchPhones = (_oldChrGrid == null || !_oldChrGrid.Visible ?
				null : _oldChrGrid.SelectedPhones);

			if (srchPhones == null)
			{
				srchPhones = (_pnlGrid == null || !_pnlGrid.Visible ?
					null : _newChartGrid.SelectedPhones);
			}

			if (srchPhones == null)
				return;

			var queries = new List<SearchQuery>();
			foreach (string phone in srchPhones)
			{
				var query = new SearchQuery();
				query.Pattern = phone + "/" + environment;
				query.IgnoreDiacritics = false;

				// Check if the phone only exists as an uncertain phone. If so,
				// then set the flag in the query to include searching words
				// made using all uncertain uncertain derivations.
				var phoneInfo = _project.PhoneCache[phone];
				if (phoneInfo != null && phoneInfo.TotalCount == 0)
					query.IncludeAllUncertainPossibilities = true;
				
				queries.Add(query);
			}

			App.MsgMediator.SendMessage("ViewSearch", queries);

			// Now set the image of the search button to the image associated
			// with the last search environment chosen by the user.
			TMItemProperties childItemProps = _tmAdapter.GetItemProperties(toolbarItemName);
			TMItemProperties parentItemProps = _tmAdapter.GetItemProperties("tbbChartPhoneSearch");
			if (parentItemProps != null && childItemProps != null)
			{
				parentItemProps.Image = childItemProps.Image;
				parentItemProps.Visible = true;
				parentItemProps.Update = true;
				parentItemProps.Tag = new[] {environment, toolbarItemName};
				_tmAdapter.SetItemProperties("tbbChartPhoneSearch", parentItemProps);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearch(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !_activeView)
				return false;

			// When the tag is nothing then perform a default search of the Phone anywhere.
			if (itemProps.Tag == null)
				OnChartPhoneSearchAnywhere(null);
			else
			{
				string[] srchArgs = itemProps.Tag as string[];
				if (srchArgs != null && srchArgs.Length == 2)
					PerformSearch(srchArgs[0], srchArgs[1]);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateChartPhoneSearch(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !_activeView)
				return false;

			bool enable = false;

			enable = ((_oldChrGrid != null && _oldChrGrid.Visible && _oldChrGrid.SelectedPhones != null) ||
				(_pnlGrid.Visible && _newChartGrid != null && _newChartGrid.Visible &&
				_newChartGrid.SelectedPhones != null));

			if (itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCharChartSearchContextMenu(object args)
		{
			return OnUpdateChartPhoneSearch(args);
		}

		#endregion

		#region Messages for ignore characters drop down
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateChartTBMenuIgnoredCharsParent(object args)
		//{
		//    var itemProps = args as TMItemProperties;
		//    if (itemProps == null || !m_activeView)
		//        return false;

		//    // TODO: Get ignored suprasegmentals working in new CV grid.
		//    itemProps.Update = true;
		//    itemProps.Visible = true;
		//    itemProps.Enabled = true;
		//    return true;
		//}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownChooseIgnoredCharactersTBMenu(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !_activeView)
				return false;

			if (itemProps.Control != null && itemProps.Control == _chartOptionsDropDown)
			{
				_chartOptionsDropDown.SetIgnoredChars(_oldChrGrid.SupraSegsToIgnore);
				_chartOptionsDropDown.SetIgnoredChars(_newChartGrid.SupraSegsToIgnore);
			}

			// This is a kludge and I really don't like to do it. But I don't know how
			// else to automatically get the custom drop-down to act like it has "focus".
			SendKeys.Send("{RIGHT}");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This gets fired when the user clicks on the "Refresh Chart" on the drop-down
		/// showing suprasegmentals to ignore.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleRefreshChartClick(object sender, EventArgs e)
		{
			_tmAdapter.HideBarItemsPopup("tbbView");
			Application.DoEvents();
			
			var ignoredSegments = _chartOptionsDropDown.GetIgnoredChars();

			// Only refresh when the list changed.
			if (_oldChrGrid.SupraSegsToIgnore != ignoredSegments)
				_oldChrGrid.SupraSegsToIgnore = _chartOptionsDropDown.GetIgnoredChars();

			if (_newChartGrid.SupraSegsToIgnore != ignoredSegments)
			{
				SaveIgnoredSuprasegmentals(ignoredSegments);
				_project.Save();
			}

			ReloadChart(false);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SaveIgnoredSuprasegmentals(string ignoredSegments)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Misc. Other Message handlers
		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsHTML(object args)
		{
			var outputFileName = GetExportFileName(DefaultHTMLOutputFile, App.kstidFileTypeHTML, "html");
				
			if (string.IsNullOrEmpty(outputFileName))
				return false;
			
			CVChartExporter.ToHtml(_project, ChartType, outputFileName, _newChartGrid,
				Settings.Default.OpenHtmlCVChartAfterExport);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private string GetExportFileName(string fmtFileName, string fileTypeFilter, string defaultFileType)
		{
			if (!_activeView)
				return null;

			string defaultOutputFileName = string.Format(fmtFileName, _project.LanguageName);

			var fileTypes = fileTypeFilter + "|" + App.kstidFileTypeAllFiles;

			int filterIndex = 0;

			return App.SaveFileDialog(defaultFileType, fileTypes, ref filterIndex,
				App.kstidSaveFileDialogGenericCaption, defaultOutputFileName, _project.Folder);
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsWordXml(object args)
		{
			var outputFileName = GetExportFileName(DefaultWordXmlOutputFile,
				App.kstidFileTypeWordXml, "xml");

			if (string.IsNullOrEmpty(outputFileName))
				return false;

			CVChartExporter.ToWordXml(_project, ChartType, outputFileName, _newChartGrid,
				Settings.Default.OpenWordXmlCVChartAfterExport);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsXLingPaper(object args)
		{
			var outputFileName = GetExportFileName(DefaultXLingPaperOutputFile,
				App.kstidFileTypeXLingPaper, "xml");

			if (string.IsNullOrEmpty(outputFileName))
				return false;

			CVChartExporter.ToXLingPaper(_project, ChartType, outputFileName, _newChartGrid,
				Settings.Default.OpenXLingPaperCVChartAfterExport);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsRTF(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsHTML(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!_activeView || itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = _activeView;
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
		protected bool OnRestoreDefaultLayoutTBMenu(object args)
		{
			if (!_activeView)
				return false;

			ReloadChart(true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRestoreDefaultLayoutTBMenu(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!_activeView || itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = _oldChrGrid.Visible;
			itemProps.Update = true;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnShowHistogram(object args)
		{
			if (!_activeView)
				return false;

			HistogramOn = !HistogramOn;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnShowHtmlChart(object args)
		{
			if (!_activeView)
				return false;

			ShowHtmlChart(!_htmlVw.Visible);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowHtmlChart(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!_activeView || itemProps == null)
				return false;

			bool shouldBechecked = _htmlVw.Visible;

			if (itemProps.Checked != shouldBechecked)
			{
				itemProps.Visible = true;
				itemProps.Checked = shouldBechecked;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowHistogram(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!_activeView || itemProps == null)
				return false;

			bool shouldBechecked = !_splitOuter.Panel2Collapsed;

			if (itemProps.Checked != shouldBechecked)
			{
				itemProps.Visible = true;
				itemProps.Checked = shouldBechecked;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			_project = args as PaProject;
			ReloadChart(false);
			return false;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Update enabled state of the move row up button.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateMoveCharChartRowUp(object args)
		//{
		//    if (!m_activeView || args.GetType() != typeof(bool))
		//        return false;

		//    btnMoveRowUp.Enabled = (bool)args;
		//    return true;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Update enabled state of the move row down button.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateMoveCharChartRowDown(object args)
		//{
		//    if (!m_activeView || args.GetType() != typeof(bool))
		//        return false;

		//    btnMoveRowDown.Enabled = (bool)args;
		//    return true;
		//}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual IPASymbolType CharacterType
		{
			get { return IPASymbolType.Unknown; }
		}

		#region Update handlers for menus that shouldn't be enabled when this view is current
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCIEResults(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupBySortedField(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExpandAllGroups(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCollapseAllGroups(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowRecordPane(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
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
			return new IxCoreColleague[] {this};
		}

		#endregion
	}
}