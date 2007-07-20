using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SIL.Pa.Controls;
using SIL.Pa.Dialogs;
using SIL.Pa.Resources;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using SIL.Pa.FFSearchEngine;
using SIL.FieldWorks.Common.UIAdapters;
using XCore;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides a base window class for vowel, consonant, diacritic and suprasegmental
	/// character charts.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ChartWndBase : Form, IxCoreColleague, ITabView
	{
		protected List<CharGridCell> m_phoneList;
		protected ITMAdapter m_tmAdapter;
		protected ITMAdapter m_mainMenuAdapter;
		protected ChartOptionsDropDown m_chartOptionsDropDown;
		protected string m_defaultHTMLOutputFile;
		protected string m_htmlChartName;
		private string m_persistedInfoFilename;
		private bool m_histogramOn = true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ChartWndBase()
		{
			PaApp.InitializeProgressBarForLoadingView(
				(CharacterType == IPACharacterType.Consonant ?
				Properties.Resources.kstidConsonantChartViewText :
				Properties.Resources.kstidVowelChartViewText), 5);
			
			InitializeComponent();
			PaApp.IncProgressBar();
			
			LoadToolbarAndContextMenus();
			m_chrGrid.TMAdapter = m_tmAdapter;
			m_chrGrid.OwningViewType = this.GetType();
			PaApp.IncProgressBar();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize()
		{
			CharGridBuilder bldr = new CharGridBuilder(m_chrGrid, CharacterType);
			m_phoneList = bldr.Build();
			m_persistedInfoFilename = bldr.PersistedInfoFilename;
			bldr = null;
			PaApp.IncProgressBar();

			// This should only be null when something has gone wrong...
			// which should never happen. :o)
			if (m_phoneList == null)
				return;

			// Create a list of phones for a histogram based on the order of the
			// phones as they appear in the grid (from left to right, top to bottom).
			List<CharGridCell> histogramPhones = new List<CharGridCell>();
			for (int iCol = 0; iCol < m_chrGrid.Grid.Columns.Count; iCol++)
			{
				for (int iRow = 0; iRow < m_chrGrid.Grid.Rows.Count; iRow++)
				{
					CharGridCell cgc = m_chrGrid.Grid[iCol, iRow].Value as CharGridCell;
					if (cgc != null)
						histogramPhones.Add(cgc);
				}
			}

			PaApp.IncProgressBar();
			m_histogram.LoadPhones(histogramPhones);
			PaApp.IncProgressBar();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads a chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ReloadChart(bool restoreDefault)
		{
			if (restoreDefault)
				System.IO.File.Delete(m_persistedInfoFilename);
			else
				CharGridPersistence.Save(m_chrGrid, m_phoneList, m_persistedInfoFilename);
			
			m_chrGrid.Reset();
			Initialize();
			m_chrGrid.ForceCurrentCellUpdate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadToolbarAndContextMenus()
		{
			if (PaApp.DesignMode)
				return;

			m_mainMenuAdapter = PaApp.LoadDefaultMenu(this);
			m_mainMenuAdapter.AllowUpdates = false;
			m_tmAdapter = AdapterHelper.CreateTMAdapter();

			if (m_tmAdapter != null)
			{
				m_tmAdapter.LoadControlContainerItem +=
					new LoadControlContainerItemHandler(m_tmAdapter_LoadControlContainerItem);

				string[] defs = new string[1];
				defs[0] = System.IO.Path.Combine(Application.StartupPath, "CVChartsTMDefinition.xml");
				m_tmAdapter.Initialize(DockableContainer,
					PaApp.MsgMediator, PaApp.ApplicationRegKeyPath, defs);
				
				m_tmAdapter.AllowUpdates = true;
			}

			// Give the chart Phone search toolbar button a default image.
			TMItemProperties childItemProps = m_tmAdapter.GetItemProperties("tbbChartPhoneSearchAnywhere");
			TMItemProperties parentItemProps = m_tmAdapter.GetItemProperties("tbbChartPhoneSearch");
			if (parentItemProps != null && childItemProps != null)
			{
				parentItemProps.Image = childItemProps.Image;
				parentItemProps.Visible = true;
				parentItemProps.Update = true;
				m_tmAdapter.SetItemProperties("tbbChartPhoneSearch", parentItemProps);
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
			m_chartOptionsDropDown = new ChartOptionsDropDown(m_chrGrid.SupraSegsToIgnore);
			m_chartOptionsDropDown.lnkRefresh.Click += new EventHandler(HandleRefreshChartClick);
			return m_chartOptionsDropDown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void BuildDefaultChart()
		{
			throw new Exception("The method must be overridden in derived class.");
		}

		#region ITabView Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Control DockableContainer
		{
			get { return pnlMasterOuter; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewUndocking()
		{
			m_mainMenuAdapter.AllowUpdates = true;
			SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveSettings()
		{
			float splitRatio =
				(float)splitContainer1.SplitterDistance / (float)splitContainer1.Height;

			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitratio", splitRatio);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "histpanevisible", HistogramOn);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewDocked()
		{
			try
			{
				// These are in a try/catch because sometimes they might throw an exception
				// in rare cases. The exception has to do with a condition in the underlying
				// .Net framework that I haven't been able to make sense of. Anyway, if an
				// exception is thrown, no big deal, the splitter distances will just be set
				// to their default values.
				float splitRatio = PaApp.SettingsHandler.GetFloatSettingsValue(Name, "splitratio", 0.6f);
				splitContainer1.SplitterDistance = (int)((float)splitContainer1.Height * splitRatio);
			}
			catch { }
		
			m_mainMenuAdapter.AllowUpdates = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ViewActivatedWhileDocked()
		{
			if (m_chrGrid != null && m_chrGrid.Grid != null)
				m_chrGrid.Grid.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the status bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public StatusStrip StatusBar
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the status bar label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel StatusBarLabel
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the progress bar.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripProgressBar ProgressBar
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the progress bar's label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel ProgressBarLabel
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the view's tooltip control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolTip ViewsToolTip
		{
			get { return m_toopTip; }
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
		/// <summary>
		/// Gets called any time any view is about to be opened, docked or undocked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnBeginViewChangingStatus(object args)
		{
			m_tmAdapter.AllowUpdates = false;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets called any time any view is finished being opened, docked or undocked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnEndViewChangingStatus(object args)
		{
			m_tmAdapter.AllowUpdates = true;
			return false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool HistogramOn
		{
			get { return m_histogramOn; }
			set
			{
				if (m_histogramOn != value)
				{
					m_histogramOn = value;
					splitContainer1.Panel2Collapsed = !value;
					Padding padding = splitContainer1.Panel1.Padding;
					padding = new Padding(padding.Left, padding.Top, padding.Right,
						(value ? 0 : splitContainer1.Panel2.Padding.Bottom));
					splitContainer1.Panel1.Padding = padding;
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

			if (PaApp.DesignMode)
				return;

			PaApp.SettingsHandler.LoadFormProperties(this);
			HistogramOn = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "histpanevisible", true);

			Initialize();
			
			ViewDocked();
			PaApp.UninitializeProgressBar();
			MinimumSize = PaApp.MinimumViewWindowSize;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save settings
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			CharGridPersistence.Save(m_chrGrid, m_phoneList, m_persistedInfoFilename);
			PaApp.SettingsHandler.SaveFormProperties(this);
			SaveSettings();
			base.OnFormClosing(e);
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchAnywhere(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;

			PerformSearch("*_*", "tbbChartPhoneSearchAnywhere");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchInitial(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;

			PerformSearch("#_+", "tbbChartPhoneSearchInitial");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchMedial(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;

			PerformSearch("+_+", "tbbChartPhoneSearchMedial");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchFinal(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;

			PerformSearch("+_#", "tbbChartPhoneSearchFinal");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearchAlone(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;
				
			PerformSearch("#_#", "tbbChartPhoneSearchAlone");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PerformSearch(string environment, string toolbarItemName)
		{
			string[] srchPhones = (m_chrGrid == null ? null : m_chrGrid.SelectedPhones);
			if (srchPhones == null)
				return;

			List<SearchQuery> queries = new List<SearchQuery>();
			foreach (string phone in srchPhones)
			{
				SearchQuery query = new SearchQuery();
				query.Pattern = phone + "/" + environment;
				query.IgnoreDiacritics = false;

				// Check if the phone only exists as an uncertain phone. If so,
				// then set the flag in the query to include searching words
				// made using all uncertain uncertain derivations.
				IPhoneInfo phoneInfo = PaApp.PhoneCache[phone];
				if (phoneInfo != null && phoneInfo.TotalCount == 0)
					query.IncludeAllUncertainPossibilities = true;
				
				queries.Add(query);
			}

			PaApp.MsgMediator.SendMessage("ViewFindPhones", queries);

			// Now set the image of the search button to the image associated
			// with the last search environment chosen by the user.
			TMItemProperties childItemProps = m_tmAdapter.GetItemProperties(toolbarItemName);
			TMItemProperties parentItemProps = m_tmAdapter.GetItemProperties("tbbChartPhoneSearch");
			if (parentItemProps != null && childItemProps != null)
			{
				parentItemProps.Image = childItemProps.Image;
				parentItemProps.Visible = true;
				parentItemProps.Update = true;
				parentItemProps.Tag = new string[] {environment, toolbarItemName};
				m_tmAdapter.SetItemProperties("tbbChartPhoneSearch", parentItemProps);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnChartPhoneSearch(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !PaApp.IsFormActive(this))
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateChartPhoneSearch(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !PaApp.IsFormActive(this))
				return false;

			bool enable = (m_chrGrid != null && m_chrGrid.SelectedPhones != null);

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
		protected bool OnUpdateCharChartSearchContextMenu(object args)
		{
			return OnUpdateChartPhoneSearch(args);
		}

		#endregion

		#region Messages for ignore characters drop down
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownChooseIgnoredCharactersTBMenu(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null || !PaApp.IsFormActive(this))
				return false;

			if (itemProps.Control != null && itemProps.Control == m_chartOptionsDropDown)
				m_chartOptionsDropDown.SetIgnoredChars(m_chrGrid.SupraSegsToIgnore);

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
			// Only refresh when the list changed.
			if (m_chrGrid.SupraSegsToIgnore != m_chartOptionsDropDown.GetIgnoredChars())
			{
				m_tmAdapter.HideBarItemsPopup("tbbView");
				Application.DoEvents();
				m_chrGrid.SupraSegsToIgnore = m_chartOptionsDropDown.GetIgnoredChars();
				ReloadChart(false);
			}
		}

		#endregion

		#region Misc. Other Message handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnExportAsHTML(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;

			string defaultHTMLFileName = 
				string.Format(m_defaultHTMLOutputFile, PaApp.Project.Language);

			string outputFileName =
				HTMLChartWriter.Export(m_chrGrid, defaultHTMLFileName, m_htmlChartName);

			if (System.IO.File.Exists(outputFileName))
				LaunchHTMLDlg.PostExportProcess(pnlMasterOuter.FindForm(), outputFileName);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExportAsHTML(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = PaApp.IsFormActive(this);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnRestoreDefaultLayoutTBMenu(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;

			ReloadChart(true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnShowHistogram(object args)
		{
			if (!PaApp.IsFormActive(this))
				return false;

			HistogramOn = !HistogramOn;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowHistogram(object args)
		{
			TMItemProperties itemProps = args as TMItemProperties;
			if (!PaApp.IsFormActive(this) || itemProps == null)
				return false;

			bool shouldBechecked = !splitContainer1.Panel2Collapsed;

			if (itemProps.Checked != shouldBechecked)
			{
				itemProps.Visible = true;
				itemProps.Checked = shouldBechecked;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
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
		//    if (!PaApp.IsFormActive(this) || args.GetType() != typeof(bool))
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
		//    if (!PaApp.IsFormActive(this) || args.GetType() != typeof(bool))
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
		protected virtual IPACharacterType CharacterType
		{
			get { return IPACharacterType.Unknown; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint a single line at the top of the panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
		{
			Color clr = (PaintingHelper.CanPaintVisualStyle() ?
				VisualStyleInformation.TextControlBorder : SystemColors.ControlDark);

			using (Pen pen = new Pen(clr))
			{
				e.Graphics.DrawLine(pen, splitContainer1.Panel2.Padding.Left, 0,
					splitContainer1.Panel2.ClientSize.Width -
					(splitContainer1.Panel2.Padding.Right + 1), 0);
			}
		}

		#region Update handlers for menus that shouldn't be enabled when this view is current
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlayback(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdatePlaybackRepeatedly(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateStopPlayback(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowCIEResults(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateGroupBySortedField(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExpandAllGroups(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCollapseAllGroups(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateShowRecordPane(object args)
		{
			return PaApp.DetermineMenuStateBasedOnViewType(args as TMItemProperties, this.GetType());
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
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
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