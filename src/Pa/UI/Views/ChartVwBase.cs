using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
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
	public partial class ChartVwBase : ViewBase, IxCoreColleague, ITabView
	{
		protected List<CharGridCell> _phoneList;
		protected ITMAdapter _tmAdapter;
		protected ChartOptionsDropDown _ignoredSymbolsDropDown;

		protected CVChartGrid _chartGrid;
		protected SilPanel _pnlGrid;
		protected WebBrowser _htmlVw;
		
		private bool _histogramOn = true;
		private bool _initialDock = true;
		private bool _activeView;

		/// ------------------------------------------------------------------------------------
		public ChartVwBase()
		{
			InitializeComponent();

			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
				return;

			Utils.WaitCursors(true);
			base.DoubleBuffered = true;

			LoadToolbarAndContextMenus();
			_chrGrid.OwningViewType = GetType();

			_chartGrid = new CVChartGrid(_tmAdapter);
			_chartGrid.Dock = DockStyle.Fill;
			_chartGrid.GridColor = ChartGridColor;
			_pnlGrid = new SilPanel();
			_pnlGrid.Dock = DockStyle.Fill;
			_pnlGrid.Controls.Add(_chartGrid);

			_htmlVw = new WebBrowser();
			_htmlVw.Dock = DockStyle.Fill;
			_htmlVw.Visible = false;
			_htmlVw.AllowWebBrowserDrop = false;
			_pnlGrid.Controls.Add(_htmlVw);
			
			_chrGrid.Visible = false;
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
			get { return _chartGrid.GridColor; }
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
		/// <summary>
		/// Loads the newer version of the C or V chart (i.e. the one built purely from phone
		/// features and which cannot be customized except by changing phone features).
		/// This version of the chart is displayed by default and the old one may eventually
		/// go away, leaving this one only (in which case, it will no longer be called "new").
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void LoadChart()
		{
			_chartGrid.ClearAll();

			var cgp = XmlSerializationHelper.DeserializeFromFile<CharGridPersistence>(LayoutFile);

			foreach (var col in cgp.ColHeadings)
				_chartGrid.AddColumnGroup(col.HeadingText);

			foreach (var row in cgp.RowHeadings)
				_chartGrid.AddRowGroup(row.HeadingText, row.SubHeadingCount);

			foreach (var phone in cgp.Phones)
				_chartGrid[phone.Column, phone.Row].Value = phone.Phone;

			if (ColumnHeaderHeight > 0)
				_chartGrid.ColumnHeadersHeight = ColumnHeaderHeight;
			else
				_chartGrid.AdjustColumnHeaderHeight();

			if (RowHeaderWidth > 0)
				_chartGrid.RowHeadersWidth = RowHeaderWidth;

			_chartGrid.AdjustCellSizes();

			_histogram.LoadPhones(cgp.Phones.Select(cell => cell.Phone));
		}

		/// ------------------------------------------------------------------------------------
		private void LoadHtmlChart()
		{
			var outputFile = CreateHtmlViewFile();
			_htmlVw.Url = new Uri(File.Exists(outputFile) ? outputFile : "about:blank");
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
			_chrGrid.Visible = false;
			_pnlGrid.Visible = true;

			_htmlVw.Visible = show;
			_chartGrid.Visible = !show;

			if (_htmlVw.Visible)
				_htmlVw.Focus();
			else
				_chartGrid.Focus();
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
		public void ReloadChart()
		{
			CVChartBuilder.Process(_project, ChartType);
			LoadChart();
			LoadHtmlChart();
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

			if (_tmAdapter != null)
			{
				App.PrepareAdapterForLocalizationSupport(_tmAdapter);
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

			if (_activeView && isDocked && _chartGrid != null)
				_chartGrid.Focus();
		}

		/// ------------------------------------------------------------------------------------
		public Form OwningForm
		{
			get { return FindForm(); }
		}

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
					_splitOuter.SplitterDistance = (int)(_splitOuter.Height * SplitterRatioSetting);
				}
				catch { }

				//_chrGrid.Grid.SetDoubleBuffering(true);

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
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (App.DesignMode)
				return;

			HistogramOn = HistogramVisibleSetting;
			ReloadChart();

			OnViewDocked(this);
			_initialDock = true;
			App.UninitializeProgressBar();

			if (ShowHtmlChartWhenViewLoaded)
				ShowHtmlChart(true);
		}

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
			var srchPhones = _chartGrid.SelectedPhones;

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
			var childItemProps = _tmAdapter.GetItemProperties(toolbarItemName);
			var parentItemProps = _tmAdapter.GetItemProperties("tbbChartPhoneSearch");
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
			var itemProps = args as TMItemProperties;
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
			var itemProps = args as TMItemProperties;
			if (itemProps == null || !_activeView)
				return false;

			var enable = (_chartGrid != null && _chartGrid.Visible && _chartGrid.SelectedPhones != null);

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

		#region Messages for ignore symbols drop down
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateChooseIgnoredSymbols(object args)
		{
		    var itemProps = args as TMItemProperties;
		    if (itemProps == null || !_activeView)
		        return false;

		    itemProps.Update = true;
		    itemProps.Visible = true;
		    itemProps.Enabled = true;
		    return true;
		}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownIgnoredSymbols(object args)
		{
			var itemProps = args as ToolBarPopupInfo;
			if (itemProps == null || !_activeView)
				return false;

			itemProps.Control = _ignoredSymbolsDropDown = new ChartOptionsDropDown();
			_ignoredSymbolsDropDown.SetIgnoredSymbols(_project.IgnoredSymbolsInCVCharts);
			_ignoredSymbolsDropDown.lnkRefresh.Click += HandleRefreshChartClick;
			 
			// This is a kludge and I really don't like to do it. But I don't know how
			// else to automatically get the custom drop-down to act like it has "focus".
			SendKeys.Send("{DOWN}");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedIgnoredSymbols(object args)
		{
			if (_ignoredSymbolsDropDown != null)
			{
				_ignoredSymbolsDropDown.lnkRefresh.Click -= HandleRefreshChartClick;
				_ignoredSymbolsDropDown.Dispose();
				_ignoredSymbolsDropDown = null;
			}
			
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
			var oldList = _project.IgnoredSymbolsInCVCharts.OrderBy(s => s, StringComparer.Ordinal).ToList();
			var newList = _ignoredSymbolsDropDown.GetIgnoredSymbols().OrderBy(s => s, StringComparer.Ordinal).ToList();

			var listsAreDifferent = (oldList.Count != newList.Count);
			if (!listsAreDifferent)
			{
				for (int i = 0; i < oldList.Count; i++)
				{
					listsAreDifferent = (oldList[i] != newList[i]);
					if (listsAreDifferent)
						break;
				}
			}

			_tmAdapter.HideBarItemsPopup("tbbIgnoredSymbols");
			Application.DoEvents();

			// Only refresh when the list changed.
			if (listsAreDifferent)
			{
				_project.IgnoredSymbolsInCVCharts = newList;
				_project.Save();
				ProjectInventoryBuilder.Process(_project);
				App.MsgMediator.SendMessage("RefreshCVChartAfterIgnoredSymbolsChanged", null);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnRefreshCVChartAfterIgnoredSymbolsChanged(object args)
		{
			ReloadChart();
			return false;
		}

		#endregion

		#region Misc. Other Message handlers
		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsHTML(object args)
		{
			var outputFileName = GetExportFileName(DefaultHTMLOutputFile, App.kstidFileTypeHTML, "html");
				
			if (string.IsNullOrEmpty(outputFileName))
				return false;

			CVChartExporter.ToHtml(_project, ChartType, outputFileName, _chartGrid,
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

			CVChartExporter.ToWordXml(_project, ChartType, outputFileName, _chartGrid,
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

			CVChartExporter.ToXLingPaper(_project, ChartType, outputFileName, _chartGrid,
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
			var itemProps = args as TMItemProperties;
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
			var itemProps = args as TMItemProperties;
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
			var itemProps = args as TMItemProperties;
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
			ReloadChart();
			return false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual IPASymbolType CharacterType
		{
			get { throw new NotImplementedException(); }
		}

		#region Update handlers for menus that shouldn't be enabled when this view is current
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}
		
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCIEResults(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupBySortedField(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExpandAllGroups(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCollapseAllGroups(object args)
		{
			return App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, GetType());
		}

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