// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2011, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2011' company='SIL International'>
//		Copyright (c) 2011, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: App.cs
// Responsibility: David O
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Localization;
using Localization.UI;
using Palaso.IO;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.ResourceStuff;
using SIL.Pa.UI.Views;
using SilTools;
using ShortcutKeysEditor=SilTools.Controls.ShortcutKeysEditor;
using Utils=SilTools.Utils;

namespace SIL.Pa
{
	#region ITabView interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface ITabView
	{
		void SetViewActive(bool makeActive, bool isDocked);
		bool ActiveView { get; }
		Form OwningForm { get;}
		ITMAdapter TMAdapter { get;}
	}

	#endregion

	#region IUndockedViewWnd interface
	/// ----------------------------------------------------------------------------------------
	public interface IUndockedViewWnd
	{
		ToolStripProgressBar ProgressBar { get;}
		ToolStripStatusLabel ProgressBarLabel { get;}
		ToolStripStatusLabel ProgressPercentLabel { get; }
		ToolStripStatusLabel StatusBarLabel { get;}
		StatusStrip StatusBar { get; }
	}

	#endregion

	/// ------------------------------------------------------------------------------------
	public static class App
	{
		public enum FeatureType
		{
			Articulatory,
			Binary
		}

		#region Constants
		public static char[] kTieBars = new[] { '\u0361', '\u035C' };
		public const string kTopTieBar = "\u0361";
		public const string kBottomTieBar = "\u035C";
		public const char kTopTieBarC = '\u0361';
		public const char kBottomTieBarC = '\u035C';
		public const string kDottedCircle = "\u25CC";
		public const char kDottedCircleC = '\u25CC';
		public const char kOrc = '\uFFFC';
		public const string kDiacriticPlaceholder = "[" + kDottedCircle + "]";
		public const string kSearchPatternDiamond = "\u25CA";
		public const string kEmptyDiamondPattern = kSearchPatternDiamond + "/" + kSearchPatternDiamond + "_" + kSearchPatternDiamond;

		public const string kHelpFileName = "Phonology_Assistant_Help.chm";
		public const string kHelpSubFolder = "Helps";
		public const string kTrainingSubFolder = "Training";
		public const string kPaRegKeyName = @"Software\SIL\Phonology Assistant";
		public const string kAppSettingsName = "application";

		#endregion

		private static string s_helpFilePath;
		private static ToolStripStatusLabel s_statusBarLabel;
		private static ToolStripProgressBar s_progressBar;
		private static ToolStripStatusLabel s_progressBarLabel;
		private static ToolStripStatusLabel s_percentLabel;
		private static ToolStripProgressBar s_activeProgressBar;
		private static ToolStripStatusLabel s_activeProgBarLabel;
		private static ToolStripStatusLabel s_activePercentLabel;
		private static string s_progressPercentFormat;
		private static string s_splashScreenLoadingMessageFormat;
		private static List<ITMAdapter> s_defaultMenuAdapters;
		private static readonly Dictionary<Type, Form> s_openForms = new Dictionary<Type, Form>();
		private static readonly List<IxCoreColleague> s_colleagueList = new List<IxCoreColleague>();

		#region Construction and startup methods
		/// --------------------------------------------------------------------------------
		static App()
		{
			if (DesignMode)
				return;

			InitializeProjectFolder();
			InitializePhoneticFont();
			MsgMediator = new Mediator();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Construct the default project path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void InitializeProjectFolder()
		{
			// Specifying the UI language on the command-line trumps the one in
			// the settings file (i.e. the one set in the options dialog box).
			foreach (var arg in Environment.GetCommandLineArgs()
				.Where(arg => arg.ToLower().StartsWith("/pf:") || arg.ToLower().StartsWith("-pf:")))
			{
				ProjectFolder = arg.Substring(4);
				if (Directory.Exists(ProjectFolder))
					return;
			}

			// Construct the default project path.
			ProjectFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			// I found that in limited user mode on Vista, Environment.SpecialFolder.MyDocuments
			// returns an empty string. Argh!!! Therefore, I have to make sure there is
			// a valid and full path. Do that by getting the user's desktop folder and
			// chopping off everything that follows the last backslash. If getting the user's
			// desktop folder fails, then fall back to the program's folder, which is
			// probably not right, but we'll have to assume it will never happen. :o)
			if (string.IsNullOrEmpty(ProjectFolder))
			{
				ProjectFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				if (string.IsNullOrEmpty(ProjectFolder) || !Directory.Exists(ProjectFolder))
				{
					ProjectFolder = AssemblyPath;
					return;
				}

				ProjectFolder = ProjectFolder.TrimEnd('\\');
				int i = ProjectFolder.LastIndexOf('\\');
				ProjectFolder = ProjectFolder.Substring(0, i);
			}

			ProjectFolder = Path.Combine(ProjectFolder, "Phonology Assistant");

			// Create the folder if it doesn't exist.
			if (!Directory.Exists(ProjectFolder))
				Directory.CreateDirectory(ProjectFolder);
		}

		/// ------------------------------------------------------------------------------------
		public static void InitializeSettingsFileLocation()
		{
			string path = null;

			try
			{
				// Specifying the UI language on the command-line trumps the one in
				// the settings file (i.e. the one set in the options dialog box).
				foreach (var arg in Environment.GetCommandLineArgs()
					.Where(arg => arg.ToLower().StartsWith("/sf:") || arg.ToLower().StartsWith("-sf:")))
				{
					path = arg.Substring(4);
					PortableSettingsProvider.SettingsFileFolder = Path.GetDirectoryName(path);
					PortableSettingsProvider.SettingsFileName = Path.GetFileName(path);
					return;
				}
			}
			catch
			{
				var msg = string.Format("There was an error retrieving the settings file\n\n" +
					"'{0}'\n\nThe default settings file will be used instead.", path);

				Utils.MsgBox(msg);
			}

			if (ProjectFolder == null)
				InitializeProjectFolder();

			PortableSettingsProvider.SettingsFileFolder = ProjectFolder;
			PortableSettingsProvider.SettingsFileName = "Pa.settings";

			//bring in settings from any previous version
			if (Settings.Default.SettingsVersion == Application.ProductVersion)
				return;
			
			//see http://stackoverflow.com/questions/3498561/net-applicationsettingsbase-should-i-call-upgrade-every-time-i-load
			Settings.Default.Upgrade();
			Settings.Default.SettingsVersion = Application.ProductVersion;
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		public static void InitializeLocalization()
		{
			SetUILanguage();

			// Copy the localization file to where the settings file is located.
			var localizationFilePath = Path.Combine(PortableSettingsProvider.SettingsFileFolder, "Pa.tmx");
			if (!File.Exists(localizationFilePath))
			{
				var srcLocalizationFilePath =
					FileLocator.GetFileDistributedWithApplication(ConfigFolderName, "Pa.tmx");

				File.Copy(srcLocalizationFilePath, localizationFilePath);
			}

			L10NMngr = LocalizationManager.Create("Pa", "Phonology Assistant", ProjectFolder);
			LocalizeItemDlg.SaveDialogSplitterPosition += (pos =>
				Settings.Default.LocalizeDlgSplitterPos = pos);
			
			LocalizeItemDlg.SetDialogSplitterPosition += (currPos =>
				(Settings.Default.LocalizeDlgSplitterPos > 0 ? Settings.Default.LocalizeDlgSplitterPos : currPos));
			
			LocalizeItemDlg.SaveDialogBounds += (dlg =>
				Settings.Default.LocalizeDlgBounds = dlg.Bounds);
			
			LocalizeItemDlg.SetDialogBounds += (dlg =>
			{
				if (!Settings.Default.LocalizeDlgBounds.IsEmpty)
					dlg.Bounds = Settings.Default.LocalizeDlgBounds;
			});

			LocalizeItemDlg.StringsLocalized += (() =>
			{
				MsgMediator.SendMessage("StringsLocalized", L10NMngr);
				PaFieldDisplayProperties.ResetDisplayNameCache();
			});
		}

		/// ------------------------------------------------------------------------------------
		public static void InitializePhoneticFont()
		{
			// These should get intialized via the settings file, but in case there is no
			// settings file, this will ensure at least something.
			if (FontHelper.FontInstalled("Doulos SIL"))
				PhoneticFont = FontHelper.MakeFont("Doulos SIL", 13, FontStyle.Regular);
			else if (FontHelper.FontInstalled("Charis SIL"))
				PhoneticFont = FontHelper.MakeFont("Charis SIL", 13, FontStyle.Regular);
			else if (FontHelper.FontInstalled("Galatia SIL"))
				PhoneticFont = FontHelper.MakeFont("Galatia SIL", 13, FontStyle.Regular);
			else if (FontHelper.FontInstalled("Gentium Plus"))
				PhoneticFont = FontHelper.MakeFont("Gentium Plus", 13, FontStyle.Regular);
			else if (FontHelper.FontInstalled("Arial Unicode"))
				PhoneticFont = FontHelper.MakeFont("Arial Unicode", 11, FontStyle.Regular);
			else if (FontHelper.FontInstalled("Lucida Sans Unicode"))
				PhoneticFont = FontHelper.MakeFont("Lucida Sans Unicode", 11, FontStyle.Regular);
			else
				PhoneticFont = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		private static void SetUILanguage()
		{
			string langId = Settings.Default.UserInterfaceLanguage;

			// Specifying the UI language on the command-line trumps the one in
			// the settings file (i.e. the one set in the options dialog box).
			foreach (var arg in Environment.GetCommandLineArgs()
				.Where(arg => arg.ToLower().StartsWith("/uilang:") || arg.ToLower().StartsWith("-uilang:")))
			{
				langId = arg.Substring(8);
				break;
			}

			LocalizationManager.UILanguageId = (string.IsNullOrEmpty(langId) ?
				LocalizationManager.kDefaultLang : langId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add Ons are undocumented and it's assumed that each add-on assembly contains at
		/// least a class called "PaAddOnManager". If a class by that name is not found in
		/// an assembly in the AddOns folder, then it's not considered to be an AddOn
		/// assembly for PA. It's up to the PaAddOnManager class in each add-on to do all
		/// the proper initialization it needs. There's nothing in the PA code that recognizes
		/// AddOns. It's all up to the Add On to reference the PA code, not the other way
		/// around. So, if an Add On needs to add a menu to the main menu, it's up to the
		/// add on to do it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ReadAddOns()
		{
			if (DesignMode)
				return;

			string[] addOnAssemblyFiles;

			try
			{
				var addOnPath = Path.Combine(AssemblyPath, "AddOns");
				addOnAssemblyFiles = Directory.GetFiles(addOnPath, "*.dll");
			}
			catch
			{
				return;
			}

			if (addOnAssemblyFiles.Length == 0)
				return;

			foreach (string filename in addOnAssemblyFiles)
			{
				try
				{
					Assembly assembly = ReflectionHelper.LoadAssembly(filename);
					if (assembly != null)
					{
						object instance =
							ReflectionHelper.CreateClassInstance(assembly, "PaAddOnManager");

						if (instance != null)
						{
							if (AddOnAssemblys == null)
								AddOnAssemblys = new List<Assembly>();

							if (AddOnManagers == null)
								AddOnManagers = new List<object>();

							AddOnAssemblys.Add(assembly);
							AddOnManagers.Add(instance);
						}
					}
				}
				catch { }
			}
		}

		#endregion

		#region Misc. localized global strings
		/// ------------------------------------------------------------------------------------
		public static string kOpenClassBracket
		{
			get
			{
				return GetString("OpenClassSymbol", "<",
					"Character used to delineate the opening of a phonetic search class.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kCloseClassBracket
		{
			get
			{
				return GetString("CloseClassSymbol", ">",
					"Character used to delineate the closing of a phonetic search class.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeAllExe
		{
			get
			{
				return GetString("FileTypes.ExecutableFileTypes",
					"All Executables (*.exe;*.com;*.pif;*.bat;*.cmd)|*.exe;*.com;*.pif;*.bat;*.cmd",
					"File types for executable files.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeAllFiles
		{
			get
			{
				return GetString("FileTypes.AllFileTypes",
					"All Files (*.*)|*.*", "Used in open/save file dialogs as the type for all files.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeHTML
		{
			get
			{
				return GetString("FileTypes.HTMLFileType", "HTML Files (*.html)|*.html");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeWordXml
		{
			get
			{
				return GetString("FileTypes.Word2003XmlFileType", "Word 2003 XML Files (*.xml)|*.xml");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeXLingPaper
		{
			get
			{
				return GetString("FileTypes.XLingPaperFileType", "XLingPaper Files (*.xml)|*.xml");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypePAXML
		{
			get
			{
				return GetString("FileTypes.PaXMLFileType",
					"{0} XML Files (*.paxml)|*.paxml", "Parameter is the application name.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypePAProject
		{
			get
			{
				return GetString("FileTypes.PaProjectFileType", "{0} Projects (*.pap)|*.pap",
					"File type for Phonology Assistant projects. The parameter is the application name.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFiletypeRTF
		{
			get
			{
				return GetString("FileTypes.RTFFileType", "Rich Text Format (*.rtf)|*.rtf",
					"File type for rich text format output.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFiletypeSASoundMP3
		{
			get
			{
				return GetString("FileTypes.Mp3FileType", "Speech Analyzer MP3 Files (*.mp3)|*.mp3");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFiletypeSASoundWave
		{
			get
			{
				return GetString("FileTypes.WaveFileType", "Speech Analyzer Wave Files (*.wav)|*.wav");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFiletypeSASoundWMA
		{
			get
			{
				return GetString("FileTypes.WindowsMediaAudioFileType", "Speech Analyzer WMA Files (*.wma)|*.wma");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeToolboxDB
		{
			get
			{
				return GetString("FileTypes.ToolboxFileType", "Toolbox Files (*.db)|*.db");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeToolboxITX
		{
			get
			{
				return GetString("FileTypes.ToolboxInterlinearFileType", "Interlinear Toolbox Files (*.itx)|*.itx");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeXML
		{
			get
			{
				return GetString("FileTypes.XmlFileType", "XML Files (*.xml)|*.xml");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeXSLT
		{
			get
			{
				return GetString("FileTypes.XsltTFileType", "XSLT Files (*.xslt)|*.xslt");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidFileTypeZip
		{
			get
			{
				return GetString("FileTypes.ZipFileType", "Zip Files (*.zip)|*.zip");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidQuerySearchingMsg
		{
			get
			{
				return GetString("PhoneticSearchingInProgressMessage", "Searching...",
					"Message displayed in status bar next to the progress bar when doing a query searches.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidSaveChangesMsg
		{
			get
			{
				return GetString("GenericSaveChangesQuestion", "Would you like to save your changes?");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static string kstidSaveFileDialogGenericCaption
		{
			get
			{
				return GetString("GenericSaveFileDialogCaption", "Save File");
			}
		}

		#endregion

		#region SplashScreen stuff
		/// ------------------------------------------------------------------------------------
		public static bool ShouldShowSplashScreen
		{
			get
			{
				return (Settings.Default.ShowSplashScreen &&
					(Control.ModifierKeys & Keys.Control) != Keys.Control);
			}
			set
			{
				Settings.Default.ShowSplashScreen = value;
				Settings.Default.Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void ShowSplashScreen()
		{
			if (ShouldShowSplashScreen)
			{
				SplashScreen = new SplashScreen(true, VersionType.Alpha);
				SplashScreen.Show();
				SplashScreen.Message = GetString("SplashScreenLoadingMsg", "Loading...");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the splash screen if it's showing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void CloseSplashScreen()
		{
			if (SplashScreen != null && SplashScreen.StillAlive)
			{
				Application.DoEvents();
				if (MainForm != null)
					MainForm.Activate();

				// Occassionally, I found that we could get to this point and the splash screen
				// is null. This was due to the fact that in some cases, I force the closure of
				// the splash screen in another thread when data sources are read.
				if (SplashScreen != null)
					SplashScreen.Close();
			}

			SplashScreen = null;
		}
		
		#endregion

		#region Message mediator adding/removing
		/// ------------------------------------------------------------------------------------
		public static void AddMediatorColleague(IxCoreColleague colleague)
		{
			if (colleague != null && !DesignMode)
			{
				MsgMediator.AddColleague(colleague);

				if (!s_colleagueList.Contains(colleague))
					s_colleagueList.Add(colleague);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void RemoveMediatorColleague(IxCoreColleague colleague)
		{
			if (colleague != null && !DesignMode)
			{
				try
				{
					MsgMediator.RemoveColleague(colleague);
				}
				catch { }

				if (s_colleagueList.Contains(colleague))
					s_colleagueList.Remove(colleague);
			}
		}

		#endregion

		#region Misc. Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The normal DesignMode property doesn't work when derived classes are loaded in
		/// designer. (This is a kludge, I know.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool DesignMode
		{
			get { return Process.GetCurrentProcess().ProcessName == "devenv"; }
		}

		/// ------------------------------------------------------------------------------------
		public static Font PhoneticFont { get; set; }

		/// ------------------------------------------------------------------------------------
		public static string BreakChars
		{
			get
			{
				return (string.IsNullOrEmpty(Settings.Default.WordBreakCharacters) ?
					" " : Settings.Default.WordBreakCharacters);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the XCore message mediator for the application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Mediator MsgMediator { get; internal set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the minimum allowed size of a view's window, including the main window that
		/// can hold all the views.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Size MinimumViewWindowSize { get; internal set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the default location for PA projects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ProjectFolder { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's splash screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ISplashScreen SplashScreen { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When running normally, this should be the same as Application.StartupPath. When
		/// running tests, this will be the folder that contains the assembly in which this
		/// class is found.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string AssemblyPath
		{
			get
			{
				// CodeBase prepends "file:/", which must be removed.
				return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the top-level registry key path to the application's registry entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ApplicationRegKeyPath
		{
			get { return @"Software\SIL\Phonology Assistant"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets just the folder name (no leading path) where the application's factory
		/// configuration files are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ConfigFolderName
		{
			get { return "Configuration"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the just the name of the folder (no leading path) where the application's
		/// processing files (i.e. xslt) are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ProcessingFolderName
		{
			get { return "Processing"; }
		}

		/// ------------------------------------------------------------------------------------
		public static IPASymbolCache IPASymbolCache
		{
			get { return InventoryHelper.IPASymbolCache; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of articulatory features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static AFeatureCache AFeatureCache
		{
			get { return InventoryHelper.AFeatureCache; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of binary features.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static BFeatureCache BFeatureCache
		{
			get { return InventoryHelper.BFeatureCache; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the currently opened PA project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject Project { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a project is being loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ProjectLoadInProcess { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the toolbar menu adapter PaMainWnd. This value should only be set
		/// by the PaMainWnd class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ITMAdapter TMAdapter { get; set; }

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and filename of the application's help file.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string HelpFile
		{
			get { return null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's main form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Form MainForm { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's current view form. When the view is docked, then
		/// this form will not be visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ITabView CurrentView { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's current view type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Type CurrentViewType { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// PA add-on DLL provides undocumented features, if it exists in the pa.exe folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<Assembly> AddOnAssemblys { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// PA add-on manager provides undocumented features, if it exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<object> AddOnManagers { get; private set; }

		#endregion

		#region Grid color methods and properties
		/// ------------------------------------------------------------------------------------
		public static void SetCellColors(DataGridView grid, DataGridViewCellFormattingEventArgs e)
		{
			if (grid.CurrentRow == null || grid.CurrentRow.Index != e.RowIndex)
				return;

			if (!grid.Focused)
			{
				e.CellStyle.SelectionBackColor = GridRowUnfocusedSelectionBackColor;
				e.CellStyle.SelectionForeColor = GridRowUnfocusedSelectionForeColor;
				return;
			}

			if (grid.CurrentCell != null && grid.CurrentCell.ColumnIndex == e.ColumnIndex)
			{
				// Set the selected cell's background color to be
				// distinct from the rest of the current row.
				e.CellStyle.SelectionBackColor = GridCellFocusedBackColor;
				e.CellStyle.SelectionForeColor = GridCellFocusedForeColor;
			}
			else
			{
				// Set the selected row's background color.
				e.CellStyle.SelectionBackColor = GridRowFocusedBackColor;
				e.CellStyle.SelectionForeColor = GridRowFocusedForeColor;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void SetGridSelectionColors(SilGrid grid, bool makeSelectedCellsDifferent)
		{
			grid.SelectedRowBackColor = GridRowFocusedBackColor;
			grid.SelectedRowForeColor = GridRowFocusedForeColor;

			grid.SelectedCellBackColor = (makeSelectedCellsDifferent ?
				GridCellFocusedBackColor : GridRowFocusedBackColor);

			grid.SelectedCellForeColor = (makeSelectedCellsDifferent ?
				GridCellFocusedForeColor : GridRowFocusedForeColor);
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridColor
		{
			get
			{
				var clr = Settings.Default.WordListGridColor;
				return (clr != Color.Transparent && clr != Color.Empty ? clr :
					ColorHelper.CalculateColor(SystemColors.WindowText, SystemColors.Window, 25));
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowFocusedForeColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? SystemColors.WindowText :
					Settings.Default.GridRowSelectionForeColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowFocusedBackColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? ColorHelper.LightHighlight :
					Settings.Default.GridRowSelectionBackColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridCellFocusedForeColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? SystemColors.WindowText :
					Settings.Default.GridCellSelectionForeColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridCellFocusedBackColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? ColorHelper.LightLightHighlight :
					Settings.Default.GridCellSelectionBackColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowUnfocusedSelectionBackColor
		{
			get
			{
				return (Settings.Default.UseSystemColors ? SystemColors.Control :
					Settings.Default.GridRowUnfocusedSelectionBackColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		public static Color GridRowUnfocusedSelectionForeColor
		{
			get
			{
				if (!Settings.Default.UseSystemColors)
					return Settings.Default.GridRowUnfocusedSelectionForeColor;

				// It turns out the control color for the silver Windows XP theme is very close
				// to the default color calculated for selected rows in PA word lists. Therefore,
				// when a word list grid looses focus and a selected row's background color gets
				// changed to the control color, it's very hard to tell the difference between a
				// selected row in a focused grid from that of a non focused grid. So, when the
				// theme is the silver (i.e. Metallic) then also make the text gray for selected
				// rows in non focused grid's.
				if (PaintingHelper.CanPaintVisualStyle() &&
					System.Windows.Forms.VisualStyles.VisualStyleInformation.DisplayName == "Windows XP style" &&
					System.Windows.Forms.VisualStyles.VisualStyleInformation.ColorScheme == "Metallic")
				{
					return SystemColors.GrayText;
				}
					
				return SystemColors.ControlText;
			}
		}

		#endregion

		#region Localization Manager Access methods
		/// ------------------------------------------------------------------------------------
		private static LocalizationManager L10NMngr { get; set; }

		/// ------------------------------------------------------------------------------------
		internal static void SaveOnTheFlyLocalizations()
		{
			if (L10NMngr != null)
				L10NMngr.SaveOnTheFlyLocalizations();
		}

		/// ------------------------------------------------------------------------------------
		internal static void ReapplyLocalizationsToAllObjects()
		{
			if (L10NMngr != null)
				L10NMngr.ReapplyLocalizationsToAllObjects();
		}

		/// ------------------------------------------------------------------------------------
		internal static void RefreshToolTipsOnLocalizationManager()
		{
			if (L10NMngr != null)
				L10NMngr.RefreshToolTips();
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetUILanguageId()
		{
			return LocalizationManager.UILanguageId;
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText)
		{
			return (L10NMngr == null ? defaultText : L10NMngr.LocalizeString(id, defaultText));
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetString(string id, string defaultText, string comment)
		{
			return (L10NMngr == null ? defaultText : L10NMngr.LocalizeString(id, defaultText, comment, null));
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetStringForObject(object obj, string defaultText)
		{
		    return (L10NMngr == null ? defaultText : (L10NMngr.GetString(obj) ?? defaultText));
		}

		/// ------------------------------------------------------------------------------------
		internal static void RegisterForLocalization(object obj, string id)
		{
			if (L10NMngr != null)
				L10NMngr.LocalizeObject(obj, id);
		}

		/// ------------------------------------------------------------------------------------
		internal static void RegisterForLocalization(object obj, string id, string defaultText)
		{
			if (L10NMngr == null)
				return;
			
			L10NMngr.LocalizeString(id, defaultText);
			L10NMngr.LocalizeObject(obj, id);
		}

		/// ------------------------------------------------------------------------------------
		internal static void RegisterForLocalization(object obj, string id,
			string defaultText, string comment)
		{
			if (L10NMngr == null)
				return;

			L10NMngr.LocalizeObject(obj, id, defaultText, null, comment);
			L10NMngr.LocalizeObject(obj, id);
		}

		#endregion

		#region Misc. methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Prepares the adapter for localization support.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PrepareAdapterForLocalizationSupport(ITMAdapter adapter)
		{
			adapter.LocalizeItem += HandleLocalizingTMItem;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reverses what the method PrepareAdapterForLocalizationSupport does to prepare
		/// the specified adapter for localization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UnPrepareAdapterForLocalizationSupport(ITMAdapter adapter)
		{
			adapter.LocalizeItem -= HandleLocalizingTMItem;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles localizing a toolbar/menu item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void HandleLocalizingTMItem(object item, string id, TMItemProperties itemProps)
		{
			if (L10NMngr != null)
			{
				L10NMngr.LocalizeObject(item, id, itemProps.Text, itemProps.Tooltip,
					ShortcutKeysEditor.KeysToString(itemProps.ShortcutKey),
					"Menu or Toolbar item", "UI.Menus and Toolbars");
			}
		}

		/// ------------------------------------------------------------------------------------
		public static int GetPlaybackSpeedForVwType(Type vwType)
		{
			if (vwType == typeof(DataCorpusVw))
				return Settings.Default.DataCorpusVwPlaybackSpeed;

			if (vwType == typeof(SearchVw))
				return Settings.Default.SearchVwPlaybackSpeed;

			if (vwType == typeof(DistributionChartVw))
				return Settings.Default.DistChartVwPlaybackSpeed;

			return 100;
		}
		
		/// ------------------------------------------------------------------------------------
		public static FormSettings InitializeForm(Form frm, FormSettings settings)
		{
			if (settings != null)
				settings.InitializeForm(frm);
			else
				settings = FormSettings.Create(frm);

			return settings;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the default phonology assistant menu in the specified container control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ITMAdapter LoadDefaultMenu(Control menuContainer)
		{
			var adapter = AdapterHelper.CreateTMAdapter();

			if (adapter != null)
			{
				PrepareAdapterForLocalizationSupport(adapter);

				var defs = new[] { FileLocator.GetFileDistributedWithApplication(ConfigFolderName,
					"PaTMDefinition.xml") };
				
				adapter.Initialize(menuContainer, MsgMediator, ApplicationRegKeyPath, defs);
				adapter.AllowUpdates = true;
				adapter.RecentFilesList = (MruFiles.Paths ?? new string[] { });
				adapter.RecentlyUsedItemChosen += (filename => MsgMediator.SendMessage("RecentlyUsedProjectChosen", filename));
			}

			if (s_defaultMenuAdapters == null)
				s_defaultMenuAdapters = new List<ITMAdapter>();

			s_defaultMenuAdapters.Add(adapter);
			return adapter;
		}

		/// ------------------------------------------------------------------------------------
		public static void UnloadDefaultMenu(ITMAdapter adapter)
		{
			if (s_defaultMenuAdapters != null && s_defaultMenuAdapters.Contains(adapter))
				s_defaultMenuAdapters.Remove(adapter);

			if (adapter != null)
			{
				adapter.Dispose();
				adapter = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified file to the recently used projects list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void AddProjectToRecentlyUsedProjectsList(string filename)
		{
			AddProjectToRecentlyUsedProjectsList(filename, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified file to the recently used projects list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void AddProjectToRecentlyUsedProjectsList(string filename, bool addToEnd)
		{
			MruFiles.AddNewPath(filename, addToEnd);

			if (s_defaultMenuAdapters != null)
			{
				foreach (ITMAdapter adapter in s_defaultMenuAdapters)
					adapter.RecentFilesList = (MruFiles.Paths ?? new string[] { });
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enables or disables a TM item based on whether or not there is project loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void EnableWhenProjectOpen(TMItemProperties itemProps)
		{
			bool enable = (Project != null);

			if (itemProps != null && itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Enables or disables a TM item based on whether or not there is a project loaded
		/// and the specified view type is current.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool DetermineMenuStateBasedOnViewType(TMItemProperties itemProps, Type viewType)
		{
			bool enable = (Project != null && CurrentViewType != viewType);

			if (itemProps != null && itemProps.Enabled != enable)
			{
				itemProps.Visible = true;
				itemProps.Enabled = enable;
				itemProps.Update = true;
			}

			return (itemProps != null && CurrentViewType == viewType);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified view type or form is the active.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsViewOrFormActive(Type viewType, Form frm)
		{
			return (viewType == CurrentViewType && frm != null && frm.ContainsFocus);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified form is the active form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsFormActive(Form frm)
		{
			if (frm == null)
				return false;

			if (frm.ContainsFocus || frm.GetType() == CurrentViewType)
				return true;

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes all MDI child forms.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void CloseAllForms()
		{
			s_openForms.Clear();

			if (MainForm == null)
				return;

			// There may be some child forms not in the s_openForms collection. If that's
			// the case, then close them this way.
			for (int i = MainForm.MdiChildren.Length - 1; i >= 0; i--)
				MainForm.MdiChildren[i].Close();
		}

		/// ------------------------------------------------------------------------------------
		public static void DrawWatermarkImage(string imageId, Graphics g, Rectangle clientRectangle)
		{
			Image watermark = Properties.Resources.ResourceManager.GetObject(imageId) as Image;
			if (watermark == null)
				return;

			Rectangle rc = new Rectangle();
			rc.Size = watermark.Size;
			rc.X = clientRectangle.Right - rc.Width - 10;
			rc.Y = clientRectangle.Bottom - rc.Height - 10;
			g.DrawImage(watermark, rc);
		}

		#endregion

		#region Progress/Status bar properties and methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the main status bar label on PaMainWnd.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripStatusLabel StatusBarLabel
		{
			get
			{
				IUndockedViewWnd udvwnd = (CurrentView != null && CurrentView.ActiveView ?
					CurrentView.OwningForm : MainForm) as IUndockedViewWnd;

				return (udvwnd != null ? udvwnd.StatusBarLabel : s_statusBarLabel);
			}
			set { s_statusBarLabel = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the progress bar on the PaMainWnd.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripProgressBar ProgressBar
		{
			get
			{
				IUndockedViewWnd udvwnd = (CurrentView != null && CurrentView.ActiveView ?
					CurrentView.OwningForm : MainForm) as IUndockedViewWnd;

				return (udvwnd != null ? udvwnd.ProgressBar : s_progressBar);
			}
			set { s_progressBar = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the progress bar's label on the PaMainWnd.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripStatusLabel ProgressBarLabel
		{
			get
			{
				IUndockedViewWnd udvwnd = (CurrentView != null && CurrentView.ActiveView ?
					CurrentView.OwningForm : MainForm) as IUndockedViewWnd;

				return (udvwnd != null ? udvwnd.ProgressBarLabel : s_progressBarLabel);
			}
			set { s_progressBarLabel = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the progress percentage label on the PaMainWnd.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripStatusLabel PercentLabel
		{
			get
			{
				IUndockedViewWnd udvwnd = (CurrentView != null && CurrentView.ActiveView ?
				    CurrentView.OwningForm : MainForm) as IUndockedViewWnd;

				return (udvwnd != null ? udvwnd.ProgressPercentLabel : s_percentLabel);
			}
			set { s_percentLabel = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the progress bar, assuming the max. value will be the count of items
		/// in the current project's word cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripProgressBar InitializeProgressBar(string text)
		{
			return InitializeProgressBar(text, Project.WordCache.Count);
		}

		/// ------------------------------------------------------------------------------------
		public static ToolStripProgressBar InitializeProgressBar(string text, int maxValue)
		{
			s_splashScreenLoadingMessageFormat =
				GetString("SplashScreenProgressMessageFormat", "{0}  {1}",
				"Format for message displayed in the splash screen when loading a project. First parameter is the message; second is the percentage value.");

			var udvwnd = (CurrentView != null && CurrentView.ActiveView ?
				CurrentView.OwningForm : MainForm) as IUndockedViewWnd;

			var bar = (udvwnd != null ? udvwnd.ProgressBar : s_progressBar);
			var lbl = (udvwnd != null ? udvwnd.ProgressBarLabel : s_progressBarLabel);
			var lblPct = (udvwnd != null ? udvwnd.ProgressPercentLabel : s_percentLabel);
			lblPct.Tag = maxValue;

			if (bar != null)
			{
				s_progressPercentFormat = GetString("ProgressPercentageFormat", "{0}%");
				bar.Maximum = maxValue;
				bar.Value = 0;
				bar.Visible = Settings.Default.UseProgressBarInMainWindow;
				lblPct.Text = string.Format(s_progressPercentFormat, 0);
				lblPct.Visible = (SplashScreen == null && !bar.Visible);
				lblPct.Tag = 0;
				lbl.Text = text;
				lbl.Visible = (SplashScreen == null);
				s_activeProgBarLabel = lbl;
				s_activePercentLabel = lblPct;
				s_activeProgressBar = bar;
				UpdateSplashScreenMessage(lbl, lblPct.Text);
				Utils.WaitCursors(true);
			}

			return s_progressBar;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the progress bar for the specified view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void InitializeProgressBarForLoadingView(string msg, int maxValue)
		{
			InitializeProgressBar(msg, maxValue);

			if (SplashScreen != null && SplashScreen.StillAlive)
				SplashScreen.Message = msg;
		}

		/// ------------------------------------------------------------------------------------
		public static void UninitializeProgressBar()
		{
			var bar = (s_activeProgressBar ?? s_progressBar);
			var lbl = (s_activeProgBarLabel ?? s_progressBarLabel);
			var lblPct = (s_activePercentLabel ?? s_percentLabel);

			if (bar != null)
				bar.Visible = false;

			if (lbl != null)
				lbl.Visible = false;

			if (lblPct != null)
				lblPct.Visible = false;

			s_activeProgBarLabel = null;
			s_activePercentLabel = null;
			s_activeProgressBar = null;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Increments the progress bar by one.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void IncProgressBar()
		{
			IncProgressBar(1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Increments the progress bar by the specified amount. Passing -1 to this method
		/// will cause the progress bar to go to it's max. value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void IncProgressBar(int amount)
		{
			var bar = (s_activeProgressBar ?? s_progressBar);

			if (bar != null)
			{
				if (amount != -1 && (bar.Value + amount) <= bar.Maximum)
					bar.Value += amount;
				else
					bar.Value = bar.Maximum;

				var lblPct = (s_activePercentLabel ?? s_percentLabel);
				if (lblPct == null)
					return;

				int pct = (int)((bar.Value / (float)bar.Maximum) * 100);
				if (pct >= (int)s_percentLabel.Tag + 5 || pct == 100)
				{
					lblPct.Text = string.Format(s_progressPercentFormat, pct);
					lblPct.Tag = pct;
					UpdateSplashScreenMessage(s_activeProgBarLabel ?? s_progressBarLabel, lblPct.Text);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the progress bar label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UpdateProgressBarLabel(string label)
		{
			var lbl = (s_activeProgBarLabel ?? s_progressBarLabel);

			if (lbl != null)
			{
				lbl.Text = label;
				UpdateSplashScreenMessage(lbl, null);
				Application.DoEvents();
			}
		}

		/// ------------------------------------------------------------------------------------
		private static void UpdateSplashScreenMessage(ToolStripStatusLabel lblMsg, string pct)
		{
			if (SplashScreen == null || !SplashScreen.StillAlive || lblMsg == null)
				return;

			SplashScreen.Message = (string.IsNullOrEmpty(pct) ? lblMsg.Text :
				string.Format(s_splashScreenLoadingMessageFormat, lblMsg.Text, pct));
		}

		#endregion

		#region Open/Save file dialog methods
		/// ------------------------------------------------------------------------------------
		public static string OpenFileDialog(string defaultFileType, string filter, string dlgTitle)
		{
			int filterIndex = 0;
			return OpenFileDialog(defaultFileType, filter, ref filterIndex, dlgTitle);
		}

		/// --------------------------------------------------------------------------------
		public static string OpenFileDialog(string defaultFileType, string filter,
			ref int filterIndex, string dlgTitle)
		{
			string[] filenames = OpenFileDialog(defaultFileType, filter, ref filterIndex,
				dlgTitle, false, null);

			return (filenames == null || filenames.Length == 0 ? null : filenames[0]);
		}

		/// --------------------------------------------------------------------------------
		public static string[] OpenFileDialog(string defaultFileType, string filter,
			ref int filterIndex, string dlgTitle, bool multiSelect)
		{
			return OpenFileDialog(defaultFileType, filter, ref filterIndex,
				dlgTitle, multiSelect, null);
		}

		/// --------------------------------------------------------------------------------
		public static string[] OpenFileDialog(string defaultFileType, string filter,
			ref int filterIndex, string dlgTitle, bool multiSelect, string initialDirectory)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.DefaultExt = defaultFileType;
			dlg.Filter = filter;
			dlg.Multiselect = multiSelect;
			dlg.ShowReadOnly = false;
			dlg.ShowHelp = false;
			dlg.Title = dlgTitle;
			dlg.RestoreDirectory = false;
			dlg.InitialDirectory = (initialDirectory ?? Environment.CurrentDirectory);

			if (filterIndex > 0)
				dlg.FilterIndex = filterIndex;

			dlg.ShowDialog();

			filterIndex = dlg.FilterIndex;
			return dlg.FileNames;
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Open the Save File dialog.
		/// </summary>
		/// <param name="defaultFileType">Default file type to save</param>
		/// <param name="filter">The parent's saved filter</param>
		/// <param name="filterIndex">The new filter index</param>
		/// <param name="dlgTitle">Title of the Save File dialog</param>
		/// <param name="initialDir">Directory where the dialog starts</param>
		/// --------------------------------------------------------------------------------
		public static string SaveFileDialog(string defaultFileType, string filter,
			ref int filterIndex, string dlgTitle, string initialDir)
		{
			return SaveFileDialog(defaultFileType, filter, ref filterIndex, dlgTitle,
				null, initialDir);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Open the Save File dialog.
		/// </summary>
		/// <param name="defaultFileType">Default file type to save</param>
		/// <param name="filter">The parent's saved filter</param>
		/// <param name="filterIndex">The new filter index</param>
		/// <param name="initialFileName">The default filename</param>
		/// <param name="initialDir">Directory where the dialog starts</param>
		/// <param name="dlgTitle">Title of the Save File dialog</param>
		/// --------------------------------------------------------------------------------
		public static string SaveFileDialog(string defaultFileType, string filter,
			ref int filterIndex, string dlgTitle, string initialFileName, string initialDir)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.AddExtension = true;
			dlg.DefaultExt = defaultFileType;
			dlg.OverwritePrompt = true;
			dlg.Filter = filter;
			dlg.Title = dlgTitle;
			dlg.RestoreDirectory = false;

			if (!string.IsNullOrEmpty(initialFileName))
				dlg.FileName = initialFileName;

			if (!string.IsNullOrEmpty(initialDir))
				dlg.InitialDirectory = initialDir;

			if (filterIndex > 0)
				dlg.FilterIndex = filterIndex;

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				filterIndex = dlg.FilterIndex;
				return dlg.FileName;
			}

			return string.Empty;
		}

		#endregion

		#region Search query methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and loads a result cache for the specified search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WordListCache Search(SearchQuery query)
		{
			int resultCount;
			return Search(query, true, false, 1, out resultCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and loads a result cache for the specified search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WordListCache Search(SearchQuery query, int incAmount)
		{
			int resultCount;
			return Search(query, true, false, incAmount, out resultCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and loads a result cache for the specified search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WordListCache Search(SearchQuery query, bool incProgressBar,
			bool returnCountOnly, int incAmount, out int resultCount)
		{
			return Search(query, incProgressBar, returnCountOnly,
				true, incAmount, out resultCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and loads a result cache for the specified search query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WordListCache Search(SearchQuery query, bool incProgressBar,
			bool returnCountOnly, bool showErrMsg, int incAmount, out int resultCount)
		{
			resultCount = 0;
			bool patternContainsWordBoundaries = (query.Pattern.IndexOf('#') >= 0);
			int incCounter = 0;

			query.ErrorMessages.Clear();
			SearchQuery modifiedQuery;
			if (!ConvertClassesToPatterns(query, out modifiedQuery, showErrMsg))
				return null;

			SearchEngine.IgnoreUndefinedCharacters = Project.IgnoreUndefinedCharsInSearches;
			var engine = new SearchEngine(modifiedQuery, Project.PhoneCache);

			string msg = engine.GetCombinedErrorMessages();
			if (!string.IsNullOrEmpty(msg))
			{
				if (showErrMsg)
					Utils.MsgBox(msg);

				query.ErrorMessages.AddRange(modifiedQuery.ErrorMessages);
				resultCount = -1;
				return null;
			}

			if (!VerifyMiscPatternConditions(engine, showErrMsg))
			{
				query.ErrorMessages.AddRange(modifiedQuery.ErrorMessages);
				resultCount = -1;
				return null;
			}

			var resultCache = (returnCountOnly ? null : new WordListCache());

			foreach (var wordEntry in Project.WordCache)
			{
				if (incProgressBar && (incCounter++) == incAmount)
				{
					IncProgressBar(incAmount);
					incCounter = 0;
				}

				string[][] eticWords = new string[1][];

				if (query.IncludeAllUncertainPossibilities && wordEntry.ContiansUncertainties)
				{
					// Get a list of all the words (each word being in the form of
					// an array of phones) that can be derived from all the primary
					// and non primary uncertainties.
					eticWords = wordEntry.GetAllPossibleUncertainWords(false);

					if (eticWords == null)
						continue;
				}
				else
				{
					// Not all uncertain possibilities should be included in the search, so
					// just load up the phones that only include the primary uncertain Phone(s).
					eticWords[0] = wordEntry.Phones;
					if (eticWords[0] == null)
						continue;
				}

				// If eticWords.Length = 1 then either the word we're searching doesn't contain
				// uncertain phones or it does but they are only primary uncertain phones. When
				// eticWords.Length > 1, we know the uncertain phones in the first word are only
				// primary uncertainities while at least one phone in the remaining words is a
				// non primary uncertainy.
				for (int i = 0; i < eticWords.Length; i++)
				{
					// If the search pattern contains the word breaking character,
					// then add a space at the beginning and end of the array of
					// phones so the word breaking character has something to
					// match at the extremes of the phonetic values.
					if (patternContainsWordBoundaries)
					{
						var tmpChars = new List<string>(eticWords[i]);
						tmpChars.Insert(0, " ");
						tmpChars.Add(" ");
						eticWords[i] = tmpChars.ToArray();
					}

					int[] result;
					bool matchFound = engine.SearchWord(eticWords[i], out result);
					while (matchFound)
					{
						if (returnCountOnly)
							resultCount++;
						else
						{
							// The last parameter sent to Add is true when the word contains
							// non primary phones, which will be the case when the eticWords
							// collection contains more than one word (I use the term 'word'
							// loosely since it's really just a transcription that may contain
							// spaces) and we're searching any word other than the first in
							// the collection.
							resultCache.Add(wordEntry, eticWords[i], result[0], result[1], i > 0);
						}

						matchFound = engine.SearchWord(out result);
					}
				}
			}

			if (incProgressBar)
				IncProgressBar(-1);

			return resultCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified query's pattern for search class specifications and replaces
		/// them with the pattern the classes represent.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ConvertClassesToPatterns(SearchQuery query,
			out SearchQuery modifiedQuery, bool showMsgOnErr)
		{
			string msg;
			return ConvertClassesToPatterns(query, out modifiedQuery, showMsgOnErr, out msg);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Searches the specified query's pattern for search class specifications and replaces
		/// them with the pattern the classes represent.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ConvertClassesToPatterns(SearchQuery query,
			out SearchQuery modifiedQuery, bool showMsgOnErr, out string errorMsg)
		{
			Debug.Assert(query != null);
			Debug.Assert(query.Pattern != null);
			errorMsg = null;

			modifiedQuery = query.Clone();

			// Get the index of the first opening bracket and check if we need go further.
			int i = query.Pattern.IndexOf(kOpenClassBracket);
			if (Project == null || Project.SearchClasses.Count == 0 || i < 0)
				return true;

			while (i >= 0)
			{
				// Save the offset of the open bracket and find
				// its corresponding closing bracket.
				int start = i;
				i = modifiedQuery.Pattern.IndexOf(kCloseClassBracket, i);

				if (i > start)
				{
					// Extract the class name from the query's pattern and
					// find the SearchClass object having that class name.
					string className = modifiedQuery.Pattern.Substring(start, i - start + 1);
					SearchClass srchClass = Project.SearchClasses.GetSearchClass(className, true);
					if (srchClass != null)
						modifiedQuery.Pattern = modifiedQuery.Pattern.Replace(className, srchClass.Pattern);
					else
					{
						errorMsg = GetString("ClassMissingMsg", "The class '{0}' is not in this project. It may have been deleted.",
							"Message displayed when a search pattern contains a class that doesn't exist in the project.");
						
						errorMsg = string.Format(errorMsg, className);
						modifiedQuery.ErrorMessages.Add(errorMsg);
						query.ErrorMessages.Add(errorMsg);

						if (showMsgOnErr)
						{
							Utils.MsgBox(errorMsg, MessageBoxButtons.OK,
								   MessageBoxIcon.Exclamation);
						}

						return false;
					}
				}

				// Get the next open class bracket.
				i = modifiedQuery.Pattern.IndexOf(kOpenClassBracket);
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private static bool VerifyMiscPatternConditions(SearchEngine engine, bool showErrMsg)
		{
			if (engine == null)
				return false;

			string msg = null;

			if (engine.GetWordBoundaryCondition() != SearchEngine.WordBoundaryCondition.NoCondition)
				msg = SearchQueryException.WordBoundaryError;
			else if (engine.GetZeroOrMoreCondition() != SearchEngine.ZeroOrMoreCondition.NoCondition)
				msg = SearchQueryException.ZeroOrMoreError;
			else if (engine.GetOneOrMoreCondition() != SearchEngine.OneOrMoreCondition.NoCondition)
				msg = SearchQueryException.OneOrMoreError;

			if (msg == null)
				msg = engine.GetCombinedErrorMessages();

			if (msg != null && showErrMsg)
				Utils.MsgBox(msg);

			return (msg == null);
		}

		#endregion

		#region Help related properties and methods
		/// ------------------------------------------------------------------------------------
		public static string HelpFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(s_helpFilePath))
				{
					s_helpFilePath = AssemblyPath;
					//s_helpFilePath = Path.Combine(s_helpFilePath, kHelpSubFolder);
					s_helpFilePath = Path.Combine(s_helpFilePath, kHelpFileName);
				}

				return s_helpFilePath;
			}
		}

		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(Control ctrl)
		{
			ShowHelpTopic("hid" + ctrl.Name);
		}

		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(string hid)
		{
			if (File.Exists(HelpFilePath))
				Help.ShowHelp(new Label(), HelpFilePath, HelpTopicPaths.ResourceManager.GetString(hid));
			else
			{
				var msg = GetString("HelpFileMissingMsg", "The help file '{0}' cannot be found.");
				Utils.MsgBox(string.Format(msg, Utils.PrepFilePathForMsgBox(s_helpFilePath)));
			}
		}

		#endregion

		#region MOA and POA key generating methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a manner of articulation sort key for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMOAKey(string phone)
		{
			// TODO: When chow characters are supported, figure out how to deal with them.

			if (string.IsNullOrEmpty(phone))
				return null;

			var keybldr = new StringBuilder(6);
			foreach (char c in phone)
			{
				var info = IPASymbolCache[c];
				keybldr.Append(info == null ? "000" :
					string.Format("{0:X3}", info.MOArticulation));
			}

			return keybldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a place of articulation sort key for the specified phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetPOAKey(string phone)
		{
			// TODO: When chou characters are supported, figure out how to deal with them.

			if (string.IsNullOrEmpty(phone))
				return null;

			var keybldr = new StringBuilder(6);
			foreach (char c in phone)
			{
				var info = IPASymbolCache[c];
				keybldr.Append(info == null ? "000" :
					string.Format("{0:X3}", info.POArticulation));
			}

			return keybldr.ToString();
		}

		#endregion
	}
}
