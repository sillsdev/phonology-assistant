using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Filters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
using SIL.Pa.Resources;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;
using SIL.Pa.UI.Views;
using SilTools;
using AboutDlg = SIL.Pa.UI.Dialogs.AboutDlg;
using Utils=SilTools.Utils;

namespace SIL.Pa.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for PaMainWnd.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PaMainWnd : Form, IxCoreColleague
	{
		private readonly bool _doNotLoadLastProject;
		private ITMAdapter _tmAdapter;
		private PaProject _project;

		#region Construction and Setup
		/// ------------------------------------------------------------------------------------
		public PaMainWnd()
		{
			_doNotLoadLastProject = ((ModifierKeys & Keys.Shift) == Keys.Shift);
			
			App.InitializeSettingsFileLocation();
			
			// If the user knows enough to add an entry to the settings file to
			// override the default UI font and the phonetic font, then use those.
			if (Settings.Default.UIFont != null)
				FontHelper.UIFont = Settings.Default.UIFont;
			if (Settings.Default.PhoneticFont != null)
				App.PhoneticFont = Settings.Default.PhoneticFont;

			LocalizeItemDlg.DefaultDisplayFont = FontHelper.UIFont;
			App.InitializeLocalization();
			App.MinimumViewWindowSize = Settings.Default.MinimumViewWindowSize;
			FwDBUtils.ShowMsgWhenGatheringFWInfo = Settings.Default.ShowMsgWhenGatheringFwInfo;

			var chrs = Settings.Default.UncertainGroupAbsentPhoneChars;
			if (!string.IsNullOrEmpty(chrs))
				IPASymbolCache.UncertainGroupAbsentPhoneChars = chrs;

			chrs = Settings.Default.UncertainGroupAbsentPhoneChar;
			if (!string.IsNullOrEmpty(chrs))
				IPASymbolCache.UncertainGroupAbsentPhoneChar = chrs;

			App.ReadAddOns();

			InitializeComponent();
			
			Settings.Default.MainWindow = App.InitializeForm(this, Settings.Default.MainWindow);
			InventoryHelper.Load();
			Settings.Default.MRUList = MruFiles.Initialize(Settings.Default.MRUList);
			ProcessHelper.CopyFilesThatMakePrettyHTMLExports();
		}

		/// ------------------------------------------------------------------------------------
		public PaMainWnd(bool showSplashScreen) : this()
		{
			if (showSplashScreen)
				App.ShowSplashScreen();

			sblblMain.Text = string.Empty;

			sblblProgress.Font = FontHelper.MakeFont(FontHelper.UIFont, 9, FontStyle.Bold);
			sblblPercent.Font = sblblProgress.Font;
			App.MainForm = this;
			App.StatusBarLabel = sblblMain;
			App.ProgressBar = sbProgress;
			App.ProgressBarLabel = sblblProgress;
			App.PercentLabel = sblblPercent;
			App.AddMediatorColleague(this);
			sbProgress.Visible = false;
			sblblProgress.Visible = false;
			sblblPercent.Visible = false;
			sblblFilter.Text = string.Empty;
			sblblFilter.Visible = false;
			sblblFilter.Paint += HandleFilterStatusStripLabelPaint;

			if (!Settings.Default.UseSystemColors)
			{
				vwTabGroup.CaptionPanel.ColorTop = Settings.Default.GradientPanelTopColor;
				vwTabGroup.CaptionPanel.ColorBottom = Settings.Default.GradientPanelBottomColor;
				vwTabGroup.CaptionPanel.ForeColor = Settings.Default.GradientPanelTextColor;
			}

			base.MinimumSize = App.MinimumViewWindowSize;
			LoadToolbarsAndMenus();

			Show();

			if (App.SplashScreen != null && App.SplashScreen.StillAlive)
				App.SplashScreen.Activate();

			Application.DoEvents();

			// Unpack training projects if it's never been done before.
			var tph = new TrainingProjectsHelper();
			tph.Setup();

			LocalizeItemDlg.StringsLocalized += () => SetWindowText(_project);
			SetWindowText(_project);
		}

		/// ------------------------------------------------------------------------------------
		private void SetWindowText(PaProject project)
		{
			if (project == null || string.IsNullOrEmpty(project.Name))
				Text = LocalizationManager.GetStringForObject(this, null);
			else
			{
				var fmt = LocalizationManager.GetString("MainWindow.WindowTitle.WithProject","{0} - Phonology Assistant");
				Text = string.Format(fmt, project.Name);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			EnableOptionsMenus(false);
			EnableUndockMenu(false);

			base.OnShown(e);

			// If there's a project specified on the command line, then load that.
			var commandLineProjFileArg = (from arg in Environment.GetCommandLineArgs()
                                          where arg.ToLower().EndsWith(".pap", StringComparison.Ordinal) && File.Exists(arg)
										  select arg).FirstOrDefault();

			// If there's a backup file specified on the command line, then load that into
			// the restore dialog box and allow the user to restore it.
			var commandLineBackupFileArg = (from arg in Environment.GetCommandLineArgs()
                                            where arg.ToLower().EndsWith(".pabackup", StringComparison.Ordinal) && File.Exists(arg)
											select arg).FirstOrDefault();

			if (commandLineProjFileArg != null)
				LoadProject(commandLineProjFileArg);
			else if (commandLineBackupFileArg != null)
			{
				App.CloseSplashScreen();
				OnRestoreProject(commandLineBackupFileArg);
				App.MsgMediator.SendMessage("MainViewOpened", this);
				return;
			}
			else if (!_doNotLoadLastProject)
				LoadProject(Settings.Default.LastProjectLoaded);

			App.CloseSplashScreen();

			if (_project != null)
			{
				OnDataSourcesModified(_project);
				OnFilterChanged(_project.CurrentFilter);
			}

			App.MsgMediator.SendMessage("MainViewOpened", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enable/disable "mnuOptionsMain".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EnableOptionsMenus(bool enable)
		{
			var itemProps = _tmAdapter.GetItemProperties("mnuOptionsMain");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
				_tmAdapter.SetItemProperties("mnuOptionsMain", itemProps);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enable/disable "mnuUnDockView".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnableUndockMenu(bool enable)
		{
			var itemProps = _tmAdapter.GetItemProperties("mnuUnDockView");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
				_tmAdapter.SetItemProperties("mnuUnDockView", itemProps);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void LoadProject(string projectFileName)
		{
			if (string.IsNullOrEmpty(projectFileName))
				return;

			if (_project != null)
			{
				_project.EnsureSortOptionsSaved();
				_project.Save();
			}

			App.ProjectLoadInProcess = true;
			Utils.WaitCursors(true);
			var project = PaProject.Load(projectFileName, this);

			if (project != null)
			{
				vwTabGroup.CloseAllViews();

				if (_project != null)
					_project.Dispose();

				App.Project = _project = project;
				Settings.Default.LastProjectLoaded = projectFileName;

				SetWindowText(project);
	
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

				OnFilterChanged(_project.CurrentFilter);
				EnableOptionsMenus(true);
				EnableUndockMenu(true);

				App.MsgMediator.SendMessage("ProjectLoaded", project);
			}

			BackColor = vwTabGroup.BackColor;
			App.ProjectLoadInProcess = false;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadViewTabs()
		{
			if (vwTabGroup.Tabs.Count > 0)
			{
				vwTabGroup.Visible = true;
				if (vwTabGroup.CurrentTab != null)
					vwTabGroup.CurrentTab.RefreshView();

				return;
			}

			var itemProps = _tmAdapter.GetItemProperties("mnuDataCorpus");
			var img = (itemProps == null ? null : itemProps.Image);
			var tab = new ViewTab(vwTabGroup, img, typeof(DataCorpusVw),
				() => new DataCorpusVw(_project), "hidDataCorpusView",
				() => LocalizationManager.GetString("MainWindow.ViewTabs.DataCorpusHelpButtonToolTip",
					"Data Corpus View Help"));

			tab.Text = LocalizationManager.GetString("MainWindow.ViewTabs.DataCorpusTab",
				"Data Corpus", null, "Data Corpus View (Ctrl+Alt+D)", null, tab);

			vwTabGroup.AddTab(tab);

			itemProps = _tmAdapter.GetItemProperties("mnuFindPhones");
			img = (itemProps == null ? null : itemProps.Image);
			tab = new ViewTab(vwTabGroup, img, typeof(SearchVw),
				() => new SearchVw(_project), "hidSearchView",
				() => LocalizationManager.GetString("MainWindow.ViewTabs.SearchTabHelpButtonToolTip",
					"Search View Help"));

			tab.Text = LocalizationManager.GetString("MainWindow.ViewTabs.SearchTab",
				"Search", null, "Search View (Ctrl+Alt+S)", null, tab);

			vwTabGroup.AddTab(tab);

			itemProps = _tmAdapter.GetItemProperties("mnuConsonantChart");
			img = (itemProps == null ? null : itemProps.Image);
			tab = new ViewTab(vwTabGroup, img, typeof(ConsonantChartVw),
				() => new ConsonantChartVw(_project), "hidConsonantChartView",
				() => LocalizationManager.GetString("MainWindow.ViewTabs.ConsonantChartHelpButtonToolTip",
					"Consonant Chart View Help"));

			tab.Text = LocalizationManager.GetString("MainWindow.ViewTabs.ConsonantChartTab",
				"Consonant Chart", null, "Consonant Chart View (Ctrl+Alt+C)", null, tab);

			vwTabGroup.AddTab(tab);

			itemProps = _tmAdapter.GetItemProperties("mnuVowelChart");
			img = (itemProps == null ? null : itemProps.Image);
			tab = new ViewTab(vwTabGroup, img, typeof(VowelChartVw),
				() => new VowelChartVw(_project), "hidVowelChartView",
				() => LocalizationManager.GetString("MainWindow.ViewTabs.VowelChartHelpButtonToolTip",
					"Vowel Chart View Help"));

			tab.Text = LocalizationManager.GetString("MainWindow.ViewTabs.VowelChartTab",
				"Vowel Chart", null, "Vowel Chart View (Ctrl+Alt+V)", null, tab);

			vwTabGroup.AddTab(tab);

			itemProps = _tmAdapter.GetItemProperties("mnuXYChart");
			img = (itemProps == null ? null : itemProps.Image);
			tab = new ViewTab(vwTabGroup, img, typeof(DistributionChartVw),
				() => new DistributionChartVw(_project), "hidDistributionChartsView",
				() => LocalizationManager.GetString("MainWindow.ViewTabs.DistributionChartHelpButtonToolTip",
					"Distribution Charts View Help"));

			tab.Text = LocalizationManager.GetString("MainWindow.ViewTabs.DistributionChartsTab",
				"Distribution Charts", null, "Distribution Charts View (Ctrl+Alt+B)", null, tab);

			vwTabGroup.AddTab(tab);
			
			if (Type.GetType("Mono.Runtime") != null) // running Mono (any platform)
			{
				this.Controls.Remove(vwTabGroup); // work-around for bug in Mono--POSSIBLY https://bugzilla.novell.com/show_bug.cgi?id=597582
				vwTabGroup.Visible = true;
				this.Controls.Add(vwTabGroup);

			}
			else // running .NET
			{
				vwTabGroup.Visible = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void LoadToolbarsAndMenus()
		{
			_tmAdapter = App.LoadDefaultMenu(this);
			App.TMAdapter = _tmAdapter;

			// This item is only visible for the main PA window (i.e. this one).
			var itemProps = _tmAdapter.GetItemProperties("mnuUnDockView");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Update = true;
				_tmAdapter.SetItemProperties("mnuUnDockView", itemProps);
			}

			// This item is only visible for undocked views, but not this window.
			itemProps = _tmAdapter.GetItemProperties("mnuDockView");
			if (itemProps != null)
			{
				itemProps.Visible = false;
				itemProps.Update = true;
				_tmAdapter.SetItemProperties("mnuDockView", itemProps);
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
			if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX) {
				Utils.UpdateWindow(Handle);
			}
			base.OnActivated(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClosed(EventArgs e)
		{
			Settings.Default.Save();
			base.OnClosed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
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

			App.MsgMediator.SendMessage("StopAllPlayback", null);

			if (_project != null)
				_project.EnsureSortOptionsSaved();

			if (vwTabGroup.CurrentTab != null)
				Settings.Default.LastViewShowing = vwTabGroup.CurrentTab.ViewType.ToString();

			// Close all the instances of SA that we started, if there are any.
			DataSourceEditor.CloseSAInstances();
			
			TempRecordCache.Dispose();
			vwTabGroup.CloseAllViews();
			IsShuttingDown = false;
			base.OnClosing(e);

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
			if (_project != null)
			{
				base.OnPaintBackground(e);
				return;
			}

			var clr1 = ColorHelper.CalculateColor(Color.White,
				SystemColors.AppWorkspace, 200);

			using (var br = new LinearGradientBrush(ClientRectangle,
				clr1, SystemColors.AppWorkspace, 45))
			{
				e.Graphics.FillRectangle(br, ClientRectangle);
			}

			// Draw the PA logo at the bottom right corner of the application workspace.
			var img = Properties.Resources.kimidPaLogo;
			var rc = new Rectangle(0, 0, img.Width, img.Height);
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

			if (_project == null)
				Invalidate();
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

		#region Misc. event handlers
		/// ------------------------------------------------------------------------------------
		private void HandleProgressLabelVisibleChanged(object sender, EventArgs e)
		{
			sblblMain.BorderSides = (sblblProgress.Visible ?
				ToolStripStatusLabelBorderSides.Right : ToolStripStatusLabelBorderSides.None);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFilterStatusStripLabelPaint(object sender, PaintEventArgs e)
		{
			if (_project != null && _project.CurrentFilter != null)
			{
				PaintFilterStatusStripLabel(sender as ToolStripStatusLabel,
					_project.CurrentFilter.Name, e);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void PaintFilterStatusStripLabel(ToolStripStatusLabel lbl, string filterName,
			PaintEventArgs e)
		{
			if (lbl == null || !lbl.Visible || filterName == null)
				return;

			var rc = lbl.ContentRectangle;

			// Fill in shaded background
			using (var br = new LinearGradientBrush(rc,
				Color.Gold, Color.Khaki, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			// Draw side borders
			using (Pen pen = new Pen(Color.Goldenrod))
			{
				e.Graphics.DrawLine(pen, 0, 0, 0, rc.Height);
				e.Graphics.DrawLine(pen, rc.Width - 1, 0, rc.Width - 1, rc.Height);
			}

			// Draw little filter image
			var img = Properties.Resources.kimidFilterSmall;
			rc = lbl.ContentRectangle;
			var rcImage = new Rectangle(0, 0, img.Width, img.Height);
			rcImage.X = rc.X + 10;
			rcImage.Y = rc.Y + (int)(Math.Ceiling(((decimal)rc.Height - rcImage.Height) / 2));
			e.Graphics.DrawImageUnscaledAndClipped(img, rcImage);

			// Draw text
			rc.X = rcImage.Right + 3;
			rc.Width -= rc.X;
			const TextFormatFlags flags = TextFormatFlags.EndEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;

			TextRenderer.DrawText(e.Graphics, filterName, lbl.Font, rc, Color.Black, flags);
		}

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
			var itemProps = _tmAdapter.GetItemProperties("mnuStopPlayback");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = true;
				itemProps.Update = true;
				_tmAdapter.SetItemProperties("mnuStopPlayback", itemProps);
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
			var itemProps = _tmAdapter.GetItemProperties("mnuStopPlayback");
			if (itemProps != null)
			{
				itemProps.Visible = true;
				itemProps.Enabled = false;
				itemProps.Update = true;
				_tmAdapter.SetItemProperties("mnuStopPlayback", itemProps);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnStringsLocalized(object args)
		{
			vwTabGroup.AdjustTabWidths();
			vwTabGroup.RefreshCaption();
			App.RefreshToolTipsOnLocalizationManager();
			return false;
		}

		///// ------------------------------------------------------------------------------------
		//protected bool OnUserInterfaceLangaugeChanged(object args)
		//{
		//    App.ReapplyLocalizationsToAllObjects();
		//    OnStringsLocalized(null);
		//    return false;
		//}

		/// ------------------------------------------------------------------------------------
		protected bool OnRecentlyUsedProjectChosen(object args)
		{
			var filename = args as string;

			if (!File.Exists(filename))
			{
				var fmt = LocalizationManager.GetString("Miscellaneous.Messages.RecentlyOpenedProjectIsMissingMsg",
					"The project file '{0}' is missing.");

				Utils.MsgBox(string.Format(fmt, filename), MessageBoxIcon.Exclamation);
			}
			else if (_project == null || _project.FileName != filename)
			{
				LoadProject(filename);
				UndefinedPhoneticCharactersDlg.Show(_project);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDataSourcesModified(object args)
		{
			var project = args as PaProject;
			if (project != null)
			{
				SetWindowText(project);
				Invalidate();
			}

			UndefinedPhoneticCharactersDlg.Show(project);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnHelpPA(object args)
		{
			App.ShowHelpTopic("hidGettingStarted");
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnHelpAbout(object args)
		{
			using (var dlg = new AboutDlg(true))    // true for beta version
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnStudentManual(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingStudentManualDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExercises(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingExercisesDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnInstructorGuide(object args)
		{
			OpenTrainingDocument(ResourceHelper.GetHelpString("hidHelpTrainingInstructorGuideDoc"));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private static void OpenTrainingDocument(string docName)
		{
			string path = Path.Combine(App.AssemblyPath, App.kTrainingSubFolder);
			path = Path.Combine(path, docName);

			if (!File.Exists(path))
			{
				var fmt = LocalizationManager.GetString("Miscellaneous.Messages.TrainingFileIsMissingMsg",
					"The training file '{0}' is missing.");

				var msg = string.Format(fmt, Utils.PrepFilePathForMsgBox(path));
				Utils.MsgBox(msg, MessageBoxIcon.Exclamation);
				return;
			}

			var prs = new Process();
			prs.StartInfo.UseShellExecute = true;
			prs.StartInfo.FileName = "\"" + path + "\"";
			prs.Start();
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFileExit(object args)
		{
			Close();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnNewProject(object args)
		{
			var dlg = new ProjectSettingsDlg(null);

			if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Project != null)
			{
				var fmt = LocalizationManager.GetString("Miscellaneous.Messages.LoadNewlyCreatedProjectQuestion",
					"Would you like to load the '{0}' project?");

				var msg = string.Format(fmt, dlg.Project.Name);

				if (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					LoadProject(dlg.Project.FileName);
					UndefinedPhoneticCharactersDlg.Show(_project, true);
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnCloseProject(object args)
		{
			if (_project == null)
				return true;

			_project.EnsureSortOptionsSaved();
			_project.Save();
			vwTabGroup.CloseAllViews();
			vwTabGroup.Visible = false;
			_project.Dispose();

			App.Project = _project = null;
			Settings.Default.LastProjectLoaded = null;
			SetWindowText(null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateCloseProject(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (_project != null);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnOpenProject(object args)
		{
			var viewModel = new OpenProjectDlgViewModel();
			using (var dlg = new OpenProjectDlg(viewModel))
			{
				if (dlg.ShowDialog() != DialogResult.OK)
					return true;

				if (_project != null &&
					_project.FileName.Equals(viewModel.SelectedProject.FilePath, StringComparison.Ordinal))
				{
					return true;
				}

				if (!Settings.Default.OpenProjectsInNewWindowCheckedValue)
					LoadProject(viewModel.SelectedProject.FilePath);
				else
				{
					using (var prs = new Process())
					{
						prs.StartInfo.Arguments = "\"" + viewModel.SelectedProject.FilePath + "\"";
						prs.StartInfo.UseShellExecute = true;
						prs.StartInfo.FileName = Application.ExecutablePath;
						prs.Start();
					}
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnProjectSettings(object args)
		{
			using (var dlg = new ProjectSettingsDlg(_project))
			{
				if (dlg.ShowDialog(this) != DialogResult.OK || !dlg.ChangesWereMade)
					return true;

				Utils.WaitCursors(false);

				// Fully reload the project and blow away the previous project instance.
				var project = PaProject.Load(_project.FileName, this);
				if (project != null)
				{
					// If there was a project loaded before this,
					// then get rid of it to make way for the new one.
					if (_project != null)
					{
						_project.Dispose();
						_project = null;
					}

					project.LastNewlyMappedFields = dlg.NewlyMappedFields;
					App.Project = _project = project;
					App.MsgMediator.SendMessage("DataSourcesModified", project);
				}

				Utils.WaitCursors(false);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateProjectSettings(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (_project != null);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnBackupProject(object args)
		{
			using (var viewModel = new BackupDlgViewModel(_project))
			using (var dlg = new BackupDlg(viewModel))
				dlg.ShowDialog();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateBackupProject(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = (_project != null);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnRestoreProject(object args)
		{
			var viewModel = RestoreDlgViewModel.Create(args as string);
			if (viewModel == null)
				return true;

			using (var dlg = new RestoreDlg(viewModel))
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					LoadProject(dlg.RestoredProjectFileName);
					UndefinedPhoneticCharactersDlg.Show(_project, false);
				}

				viewModel.Dispose();
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateRestoreProject(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled = true;
			itemProps.Update = true;
			return true;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected bool OnUpdateDefineAmbiguousItems(object args)
		//{
		//    PaApp.EnableWhenProjectOpen(args as TMItemProperties);
		//    return true;
		//}

		///----------------------------------------------------------------------------------
		protected bool OnExportAsPAXML(object args)
		{
			//var dlg = new SaveFileDialog();
			//dlg.OverwritePrompt = true;
			//dlg.CheckFileExists = false;
			//dlg.CheckPathExists = true;
			//dlg.AddExtension = true;
			//dlg.ShowHelp = false;
			//dlg.RestoreDirectory = false;
			//dlg.InitialDirectory = Environment.CurrentDirectory;
			//dlg.DefaultExt = "paxml";

			//var fmt = LocalizationManager.GetString("PaXmlExportSaveFileDialogText", "Export to {0} XML");
			//dlg.Title = string.Format(fmt, Application.ProductName);
			//dlg.FileName = _project.GetCleanNameForFileName() + ".paxml";
			//dlg.FilterIndex = 0;
			//dlg.Filter = string.Format(App.kstidFileTypePAXML, Application.ProductName) +
			//    "|" + App.kstidFileTypeAllFiles;

			//if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
			//    _project.RecordCache.Save(dlg.FileName);

			return true;
		}

		///----------------------------------------------------------------------------------
		protected bool OnUpdateExportAsPAXML(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			// TODO: Make this visible when PaXml is supported again.
			itemProps.Visible = false;
			itemProps.Update = true;
			
			//itemProps.Visible = true;
			//itemProps.Text = string.Format(itemProps.OriginalText, Application.ProductName);
			//itemProps.Enabled = (m_project != null && m_project.RecordCache != null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnToolsOptions(object args)
		{
			using (var optionsDlg = new OptionsDlg(_project))
			{
				// TODO: Send a message indicating the options were changed.
				if (optionsDlg.ShowDialog(this) == DialogResult.OK)
				{
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateToolsOptions(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Update = true;
			itemProps.Visible = true;
			itemProps.Enabled = (_project != null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUndefinedCharacters(object args)
		{
			UndefinedPhoneticCharactersDlg.Show(_project, true);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateUndefinedCharacters(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Update = true;
			itemProps.Enabled = (_project != null &&
				App.IPASymbolCache.UndefinedCharacters != null &&
				App.IPASymbolCache.UndefinedCharacters.Count > 0);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnReloadProject(object args)
		{
			_project.ReloadDataSources();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateReloadProject(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Visible = true;
			itemProps.Enabled =
				(_project != null && _project.DataSources != null && _project.DataSources.Count > 0);
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disable the Undock Menu when there is only one docked view remaining in
		/// the main window.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnEndViewChangingStatus(object args)
		{
			EnableUndockMenu(vwTabGroup.DockedTabCount != 1);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enable / disable the Edit Source Record menu selection and toolbar button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateEditSourceRecord(object args)
		{
			var itemProps = args as TMItemProperties;
			bool enabled = true;

			if (itemProps == null)
				return false;

			PaWordListGrid grid = null;

			if (vwTabGroup != null && vwTabGroup.CurrentTab != null)
			{
				if (vwTabGroup.CurrentTab.View is DataCorpusVw)
					grid = (vwTabGroup.CurrentTab.View as DataCorpusVw).WordListGrid;
				else if (vwTabGroup.CurrentTab.View is SearchVw)
				{
					var view = vwTabGroup.CurrentTab.View as SearchVw;
					grid = view.ResultViewManger.CurrentViewsGrid;
				}
				else if (vwTabGroup.CurrentTab.View is DistributionChartVw)
				{
					var view = vwTabGroup.CurrentTab.View as DistributionChartVw;
					grid = view.ResultViewManger.CurrentViewsGrid;
				}
				else
					enabled = false;
			}

			// Disable UpdateEditSourceRecord if the current row is a hierarchical group row
			if (grid != null && grid.CurrentRow is SilHierarchicalGridRow)
				enabled = false;

			itemProps.Visible = true;
			itemProps.Update = true;
			itemProps.Enabled = (_project != null && grid != null && enabled && _project.WordCache.Count != 0);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hides the menu item that's a place holder for adding the menu items for each
		/// filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateFiltersPlaceholder(object args)
		{
			var itemProps = args as TMItemProperties;

			if (itemProps != null)
			{
				itemProps.Visible = false;
				itemProps.Update = true;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a menu item for each filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownFiltersParent(object args)
		{
			var tbpi = args as ToolBarPopupInfo;
			if (tbpi == null || _project == null)
				return false;

			const string cmdId = "CmdExecuteFilter";
			tbpi.Adapter.AddCommandItem(cmdId, "EnableFilter");

			bool firstItem = true;
			foreach (var filter in _project.FilterHelper.Filters.Where(f => f.ShowInToolbarList).OrderBy(f => f.Name))
			{
				var props = new TMItemProperties();
				props.BeginGroup = firstItem;
				props.Text = filter.Name;
				props.CommandId = cmdId;
				props.Name = "FILTER:" + filter.Name;
				props.Tag = filter;
				props.Checked = (_project.FilterHelper.CurrentFilter == filter);
				props.Visible = true;
				props.Update = true;
				tbpi.Adapter.AddMenuItem(props, "mnuFiltersMain", "mnuFilterPlaceholder");
				firstItem = false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the filter menu items added in OnDropDownFiltersParent().
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected bool OnDropDownClosedFiltersParent(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null || _project == null)
				return false;

			foreach (var filter in _project.FilterHelper.Filters.Where(f => f.ShowInToolbarList))
				itemProps.Adapter.RemoveItem("FILTER:" + filter.Name);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnEnableFilter(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			_project.FilterHelper.ApplyFilter(itemProps.Tag as Filter);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFilters(object args)
		{
			using (var dlg = new FiltersDlg(_project))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateFilters(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnNoFilter(object args)
		{
			OnFilterTurnedOff(null);
			_project.FilterHelper.TurnOffCurrentFilter();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateNoFilter(object args)
		{
			var itemProps = args as TMItemProperties;
			if (itemProps == null)
				return false;

			itemProps.Enabled = (_project != null && _project.FilterHelper.CurrentFilter != null);
			itemProps.Visible = true;
			itemProps.Update = true;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFilterChanged(object args)
		{
			var filter = args as Filter;
			sblblFilter.Visible = (filter != null);
			if (filter != null)
			{
				sblblFilter.Text = filter.Name;
				var constraint = new Size(statusStrip.Width / 3, 0);
				sblblFilter.Width = sblblFilter.GetPreferredSize(constraint).Width + 20;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnFilterTurnedOff(object args)
		{
			sblblFilter.Visible = false;
			sblblFilter.Text = string.Empty;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDescriptiveFeatures(object args)
		{
			var viewModel = new FeaturesDlgViewModel(_project, App.FeatureType.Articulatory);

			using (var dlg = new DescriptiveFeaturesDlg(viewModel))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateDescriptiveFeatures(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnDistinctiveFeatures(object args)
		{
			var viewModel = new FeaturesDlgViewModel(_project, App.FeatureType.Binary);

			using (var dlg = new DistinctiveFeaturesDlg(viewModel))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateDistinctiveFeatures(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnAmbiguousSequences(object args)
		{
			using (var dlg = new AmbiguousSequencesDlg(_project))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateAmbiguousSequences(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnExperimentalTranscriptions(object args)
		{
			using (var dlg = new TranscriptionChangesDlg())
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateExperimentalTranscriptions(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnClasses(object args)
		{
			using (var dlg = new ClassesDlg(_project))
				dlg.ShowDialog(this);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateClasses(object args)
		{
			App.EnableWhenProjectOpen(args as TMItemProperties);
			return true;
		}

		#endregion

		#region Message handlers for Views
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the query corpus view.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewDataCorpus(object args)
		{
			vwTabGroup.ActivateView(typeof(DataCorpusVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the query corpus view menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewDataCorpus(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(DataCorpusVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnViewSearch(object args)
		{
			SearchVw vw = vwTabGroup.ActivateView(typeof(SearchVw)) as SearchVw;

			if (vw == null)
				return true;

			if (args is SearchQuery)
				vw.PerformSearch(args as SearchQuery, SearchResultLocation.CurrentTabGroup);
			else if (args is List<SearchQuery>)
			{
				foreach (SearchQuery query in (args as List<SearchQuery>))
					vw.PerformSearch(query, SearchResultLocation.CurrentTabGroup);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewSearch(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(SearchVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the consonant chart.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewConsonantChart(object args)
		{
			vwTabGroup.ActivateView(typeof(ConsonantChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the Consonant chart menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewConsonantChart(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(ConsonantChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the vowel chart.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewVowelChart(object args)
		{
			vwTabGroup.ActivateView(typeof(VowelChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the vowel chart menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewVowelChart(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(VowelChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Display the xy chart.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnViewXYChart(object args)
		{
			vwTabGroup.ActivateView(typeof(DistributionChartVw));
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the xy chart menu.
		/// </summary>
		/// <param name="args"></param>
		/// <returns>true if the message was handled</returns>
		/// ------------------------------------------------------------------------------------
		protected bool OnUpdateViewXYChart(object args)
		{
			App.DetermineMenuStateBasedOnViewType(args as TMItemProperties, typeof(DistributionChartVw));
			return true;
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
			return new IxCoreColleague[] { this };
		}

		#endregion
	}
}
