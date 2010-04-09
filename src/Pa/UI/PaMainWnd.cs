// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: PaMainWnd.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.DataSource;
using SIL.Pa.Model;
using SIL.Pa.Filters;
using SIL.Pa.Properties;
using SIL.Pa.UI.Views;
using SilUtils;

namespace SIL.Pa.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for PaMainWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PaMainWnd : Form, IxCoreColleague
	{
		private ITMAdapter m_tmAdapter;

		#region Construction and Setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaMainWnd()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaMainWnd(bool showSplashScreen) : this()
		{
			if (showSplashScreen)
				App.ShowSplashScreen();

			sblblMain.Text = string.Empty;
			App.MainForm = this;
			App.StatusBarLabel = sblblMain;
			App.ProgressBar = sbProgress;
			App.ProgressBarLabel = sblblProgress;
			App.AddMediatorColleague(this);
			sbProgress.Visible = false;
			sblblProgress.Visible = false;
			sblblFilter.Text = string.Empty;
			sblblFilter.Visible = false;
			sblblFilter.Paint += FilterHelper.HandleFilterStatusStripLabelPaint;

			base.MinimumSize = App.MinimumViewWindowSize;
			LoadToolbarsAndMenus();
			SetUIFont();
			Show();

			if (App.SplashScreen != null && App.SplashScreen.StillAlive)
				App.SplashScreen.Activate();

			Application.DoEvents();

			// Unpack training projects if it's never been done before.
			TrainingProjectsHelper tph = new TrainingProjectsHelper();
			tph.Setup();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			EnableOptionsMenus(false);
			EnableUndockMenu(false);

			base.OnShown(e);

			// If there's a project specified on the command line, then load that.
			// Otherwise, load the last loaded project whose name is in the settings file.
			string projArg = (from args in Environment.GetCommandLineArgs()
							  where args.StartsWith("/o:") || args.StartsWith("-o:")
							  select args).FirstOrDefault();

			if (projArg != null)
				LoadProject(projArg.Substring(3));
			else
				LoadProject(Settings.Default.LastProjectLoaded);

			App.CloseSplashScreen();

			if (App.Project != null)
			{
				OnDataSourcesModified(App.Project.Name);
				OnFilterChanged(FilterHelper.CurrentFilter);
			}

			App.MsgMediator.SendMessage("MainViewOpened", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the user knows enough to add an entry to the settings file to override the
		/// default UI font, then read it and use it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetUIFont()
		{
			const string kFontEntry = "UIFont";

			string name = App.SettingsHandler.GetStringSettingsValue(kFontEntry, "name", null);
			if (name == null)
				return;

			float size = App.SettingsHandler.GetFloatSettingsValue(kFontEntry, "size",
				SystemInformation.MenuFont.SizeInPoints);

			FontStyle style = FontStyle.Regular;

			if (App.SettingsHandler.GetBoolSettingsValue(
				kFontEntry, "bold", SystemInformation.MenuFont.Bold))
			{
				style |= FontStyle.Bold;
			}

			if (App.SettingsHandler.GetBoolSettingsValue(kFontEntry, "italic",
				SystemInformation.MenuFont.Italic))
			{
				style |= FontStyle.Italic;
			}

			FontHelper.UIFont = FontHelper.MakeFont(name, size, style);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enable/disable "mnuOptionsMain".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EnableOptionsMenus(bool enable)
		{
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("mnuOptionsMain");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("mnuOptionsMain", itemProps);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enable/disable "mnuUnDockView".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnableUndockMenu(bool enable)
		{
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("mnuUnDockView");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("mnuUnDockView", itemProps);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadProject(string projectFileName)
		{
			if (string.IsNullOrEmpty(projectFileName))
				return;

			if (App.Project != null)
				App.Project.EnsureSortOptionsSaved();

			App.ProjectLoadInProcess = true;
			Utils.WaitCursors(true);
			PaProject project = PaProject.Load(projectFileName, this);

			if (project != null)
			{
				vwTabGroup.CloseAllViews();

				if (App.Project != null)
					App.Project.Dispose();

				App.Project = project;
				Settings.Default.LastProjectLoaded = projectFileName;

				Text = string.Format(Properties.Resources.kstidMainWindowCaption,
					project.Name, Application.ProductName);

				// When there are already tabs it means there was a project loaded before
				// the one just loaded. Therefore, save the current view so it may be
				// restored after the tabs are loaded for the new project.
				if (vwTabGroup.CurrentTab != null)
					Settings.Default.LastViewShowing = vwTabGroup.CurrentTab.ViewType.ToString();

				LoadViewTabs();

				// Make the last tab that was current the current one now.
				var type = Type.GetType(typeof(DataCorpusVw).FullName);
				try
				{
					type = Type.GetType(Settings.Default.LastViewShowing);
				}
				catch { }

				vwTabGroup.ActivateView(type ?? typeof(DataCorpusVw));

				App.AddProjectToRecentlyUsedProjectsList(projectFileName);
				
				EnableOptionsMenus(true);
				EnableUndockMenu(true);
			}

			BackColor = vwTabGroup.BackColor;
			App.ProjectLoadInProcess = false;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadViewTabs()
		{
			if (vwTabGroup.Visible)
			{
				if (vwTabGroup.CurrentTab != null)
					vwTabGroup.CurrentTab.RefreshView();

				return;
			}

			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("mnuDataCorpus");
			string text = (itemProps == null ? "Error!" : itemProps.Text);
			string tooltip = Properties.Resources.kstidDataCorpusViewToolTip;
			string helptooltip = Properties.Resources.kstidDataCorpusViewHelpToolTip;
			Image img = (itemProps == null ? null : itemProps.Image);
			vwTabGroup.AddTab(text, tooltip, helptooltip, "hidDataCorpusView", img, typeof(DataCorpusVw));

			itemProps = m_tmAdapter.GetItemProperties("mnuFindPhones");
			text = (itemProps == null ? "Error!" : itemProps.Text);
			tooltip = Properties.Resources.kstidSearchViewToolTip;
			helptooltip = Properties.Resources.kstidSearchViewHelpToolTip;
			img = (itemProps == null ? null : itemProps.Image);
			vwTabGroup.AddTab(text, tooltip, helptooltip, "hidSearchView", img, typeof(SearchVw));

			itemProps = m_tmAdapter.GetItemProperties("mnuConsonantChart");
			text = (itemProps == null ? "Error!" : itemProps.Text);
			tooltip = Properties.Resources.kstidConsonantChartViewToolTip;
			helptooltip = Properties.Resources.kstidConsonantChartViewHelpToolTip;
			img = (itemProps == null ? null : itemProps.Image);
			vwTabGroup.AddTab(text, tooltip, helptooltip, "hidConsonantChartView", img, typeof(ConsonantChartVw));

			itemProps = m_tmAdapter.GetItemProperties("mnuVowelChart");
			text = (itemProps == null ? "Error!" : itemProps.Text);
			tooltip = Properties.Resources.kstidVowelChartViewToolTip;
			helptooltip = Properties.Resources.kstidVowelChartViewHelpToolTip;
			img = (itemProps == null ? null : itemProps.Image);
			vwTabGroup.AddTab(text, tooltip, helptooltip, "hidVowelChartView", img, typeof(VowelChartVw));

			itemProps = m_tmAdapter.GetItemProperties("mnuXYChart");
			text = (itemProps == null ? "Error!" : itemProps.Text);
			tooltip = Properties.Resources.kstidXYChartsViewToolTip;
			helptooltip = Properties.Resources.kstidXYChartsViewHelpToolTip;
			img = (itemProps == null ? null : itemProps.Image);
			vwTabGroup.AddTab(text, tooltip, helptooltip, "hidXYChartsView", img, typeof(XYChartVw));

			itemProps = m_tmAdapter.GetItemProperties("mnuPhoneInventory");
			text = (itemProps == null ? "Error!" : itemProps.Text);
			tooltip = Properties.Resources.kstidPhoneInventoryViewToolTip;
			helptooltip = Properties.Resources.kstidPhoneInventoryViewHelpToolTip;
			img = (itemProps == null ? null : itemProps.Image);
			vwTabGroup.AddTab(text, tooltip, helptooltip, "hidPhoneInventoryView", img, typeof(PhoneInventoryVw));
			
			vwTabGroup.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadToolbarsAndMenus()
		{
			m_tmAdapter = App.LoadDefaultMenu(this);
			App.TMAdapter = m_tmAdapter;

			// This item is only visible for the main PA window (i.e. this one).
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("mnuUnDockView");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("mnuUnDockView", itemProps);
			}

			// This item is only visible for undocked views, but not this window.
			itemProps = m_tmAdapter.GetItemProperties("mnuDockView");
			if (itemProps != null)
			{
				itemProps.Visible = false;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("mnuDockView", itemProps);
			}
		}

		#endregion

		#region Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fix for PA-62.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnActivated(EventArgs e)
		{
			Utils.UpdateWindow(Handle);
			base.OnActivated(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			App.SettingsHandler.LoadFormProperties(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.Save();

			// Closing isn't allowed in the middle of loading a project.
			if (App.ProjectLoadInProcess)
			{
				e.Cancel = true;
				return;
			}

			if (App.MsgMediator.SendMessage("PaShuttingDown", e))
			{
				e.Cancel = true;
				return;
			}

			if (App.Project != null)
				App.Project.EnsureSortOptionsSaved();

			App.SettingsHandler.SaveFormProperties(this);

			if (vwTabGroup.CurrentTab != null)
				Settings.Default.LastViewShowing = vwTabGroup.CurrentTab.ViewType.ToString();

			// Close all the instances of SA that we started, if there are any.
			DataSourceEditor.CloseSAInstances();
			
			TempRecordCache.Dispose();
			vwTabGroup.CloseAllViews();
			IsShuttingDown = false;
			base.OnClosing(e);

			Settings.Default.Save();

			// This shouldn't be necessary but is in order to fix PA-431, which is
			// a little disconcerting. I have no clue how PA could get into a state
			// where it can get this far without the app. window going away and
			// PA being unloaded from memory.
			Dispose();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws a gradient fill in the application workspace when there is no project open.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (App.Project != null)
			{
				base.OnPaintBackground(e);
				return;
			}

			Color clr1 = ColorHelper.CalculateColor(Color.White,
				SystemColors.AppWorkspace, 200);

			using (LinearGradientBrush br = new LinearGradientBrush(ClientRectangle,
				clr1, SystemColors.AppWorkspace, 45))
			{
				e.Graphics.FillRectangle(br, ClientRectangle);
			}

			// Draw the PA logo at the bottom right corner of the application workspace.
			Image img = Properties.Resources.kimidPaLogo;
			Rectangle rc = new Rectangle(0, 0, img.Width, img.Height);
			rc.X = ClientRectangle.Right - img.Width - 20;
			rc.Y = ClientRectangle.Bottom - img.Height - 20 - statusStrip.Height;
			e.Graphics.DrawImageUnscaledAndClipped(img, rc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there is no project open, this forces the gradient background to be repainted
		/// on the application workspace.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (App.Project == null)
				Invalidate();

			sblblFilter.Width = Math.Max(175, statusStrip.Width / 3);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// True if the application is shutting down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsShuttingDown { get; private set; }

		#endregion

		#region Message mediator message handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This gets called whenever playback is about to begin. Use the message to enable
		/// the menu item for stopping playback. That way, the shortcut key for stopping will
		/// be enabled. Normally this gets done in the menu item's update handler but that
		/// only gets called when the menu pops up. We need to enable the stop option now in
		/// order for the application to respond to it's shortcut key.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPlaybackBeginning(object args)
		{
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("mnuStopPlayback");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = true;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("mnuStopPlayback", itemProps);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This gets called whenever playback has just ended. Use the message to disable
		/// the menu item for stopping playback. Normally the update handler for the stop
		/// menu item would take care of this, but that only gets called when the menu
		/// pops up. We need to disable the options before that.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnPlaybackEnded(object args)
		{
			TMItemProperties itemProps = m_tmAdapter.GetItemProperties("mnuStopPlayback");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				m_tmAdapter.SetItemProperties("mnuStopPlayback", itemProps);
			}

			return false;
		}

		#endregion
	}
}
