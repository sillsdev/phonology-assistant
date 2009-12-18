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
// File: Pacs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Localize.LocalizationUtils;
using SIL.Pa.Data;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.Resources;
using SilUtils;
using SilUtils.Controls;

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
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IUndockedViewWnd
	{
		ToolStripProgressBar ProgressBar { get;}
		ToolStripStatusLabel ProgressBarLabel { get;}
		ToolStripStatusLabel StatusBarLabel { get;}
		StatusStrip StatusBar { get; }
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ------------------------------------------------------------------------------------
	public static class PaApp
	{
		public enum FeatureType
		{
			Articulatory,
			Binary
		}

		public const string kLocalizationGroupTMItems = "Toolbar and Menu Items";
		public const string kLocalizationGroupUICtrls = "User Interface Controls";
		public const string kLocalizationGroupDialogs = "Dialog Boxes";
		public const string kLocalizationGroupInfoMsg = "Information Messages";
		public const string kLocalizationGroupMisc = "Miscellaneous Strings";

		public static string kOpenClassBracket = ResourceHelper.GetString("kstidOpenClassSymbol");
		public static string kCloseClassBracket = ResourceHelper.GetString("kstidCloseClassSymbol");
		public const string kHelpFileName = "Phonology_Assistant_Help.chm";
		public const string kHelpSubFolder = "Helps";
		public const string kTrainingSubFolder = "Training";
		public const string kPaRegKeyName = @"Software\SIL\Phonology Assistant";
		public const string kAppSettingsName = "application";

		private static string s_breakChars;
		private static string s_helpFilePath;
		private static ITMAdapter s_tmAdapter;
		private static ToolStripStatusLabel s_statusBarLabel;
		private static ToolStripProgressBar s_progressBar;
		private static ToolStripStatusLabel s_progressBarLabel;
		private static ToolStripProgressBar s_activeProgressBar;
		private static ToolStripStatusLabel s_activeProgBarLabel;
		private static Form s_mainForm;
		private static ITabView s_currentView;
		private static Type s_currentViewType;
		private static bool s_projectLoadInProcess;
		private static PaProject s_project;
		private static RecordCache s_recCache;
		private static WordCache s_wordCache;
		private static PhoneCache s_phoneCache;
		private static PaFieldInfoList s_fieldInfo;
		private static ISplashScreen s_splashScreen;
		private static string s_defaultProjFolder;
		private static List<ITMAdapter> s_defaultMenuAdapters;
		private static readonly string s_settingsFile;
		private static readonly PaSettingsHandler s_settingsHndlr;
		private static readonly Mediator s_msgMediator;
		private static readonly Dictionary<Type, Form> s_openForms = new Dictionary<Type, Form>();
		private static readonly Size s_minViewWindowSize;
		private static readonly List<IxCoreColleague> s_colleagueList = new List<IxCoreColleague>();

		// The PA add-on DLL provides undocumented features, if it exists in the pa.exe
		// folder. The add-on manager class is the class in the DLL that links PA with
		// the add-on. At this point, it's all done through reflection.
		private static List<Assembly> s_addOnAssemblys;
		private static List<object> s_addOnManagers;

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------
		static PaApp()
		{
			InitializePaRegKey();
			s_settingsFile = Path.Combine(s_defaultProjFolder, "pa.xml");
			s_settingsHndlr = new PaSettingsHandler(s_settingsFile);
			s_msgMediator = new Mediator();

			LocalizationManager.Initialize();
			string langId = s_settingsHndlr.GetStringSettingsValue("UserInterface", "lang", null);
			LocalizationManager.UILangId = langId ?? LocalizationManager.kDefaultLang;

			// Create the master set of PA fields. When a project is opened, any
			// custom fields belonging to the project will be added to this list.
			s_fieldInfo = PaFieldInfoList.DefaultFieldInfoList;

			s_minViewWindowSize = new Size(
				s_settingsHndlr.GetIntSettingsValue("minviewwindowsize", "width", 550),
				s_settingsHndlr.GetIntSettingsValue("minviewwindowsize", "height", 450));

			string val = s_settingsHndlr.GetStringSettingsValue("uncertainphonegroups",
				"absenceofphonechars", null);
			
			if (val != null)
				IPASymbolCache.UncertainGroupAbsentPhoneChars = val;

			val = s_settingsHndlr.GetStringSettingsValue("uncertainphonegroups",
				"absenceofphonecharinpopup", "\u2205");
				
			if (val != null)
				IPASymbolCache.UncertainGroupAbsentPhoneChar = val;
				
			FwDBUtils.ShowMsgWhenGatheringFWInfo = s_settingsHndlr.GetBoolSettingsValue(
				"fieldworks", "showmsgwhengatheringinfo", false);

			ReadAddOns();

			LocalizeItemDlg.SetDialogSplitterPosition += LocalizeItemDlg_SetDialogSplitterPosition;
			LocalizeItemDlg.SaveDialogSplitterPosition += LocalizeItemDlg_SaveDialogSplitterPosition;
			LocalizeItemDlg.SetDialogBounds += LocalizeItemDlg_SetDialogBounds;
			LocalizeItemDlg.SaveDialogBounds += LocalizeItemDlg_SaveDialogBounds;

			// Load the cache of IPA symbols, articulatory and binary features.
			InventoryHelper.Load();
		}

		#region event handlers for saving and restoring localization dialog settings
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the localization dialog's size and location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void LocalizeItemDlg_SetDialogBounds(LocalizeItemDlg dlg)
		{
			SettingsHandler.LoadFormProperties(dlg);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves localization dialog size and location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void LocalizeItemDlg_SaveDialogBounds(LocalizeItemDlg dlg)
		{
			SettingsHandler.SaveFormProperties(dlg);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the saved splitter distance value for Localizing dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static int LocalizeItemDlg_SetDialogSplitterPosition()
		{
			return SettingsHandler.GetIntSettingsValue("LocalizeDlg", "splitdistance", 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the splitter distance value for Localizing dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static void LocalizeItemDlg_SaveDialogSplitterPosition(int pos)
		{
			SettingsHandler.SaveSettingsValue("LocalizeDlg", "splitdistance", pos);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShowSplashScreen()
		{
			// Show the splash screen only if the show property of the
			// SplashScreen item in the settings file is not set to false.
			if (s_settingsHndlr.GetBoolSettingsValue("SplashScreen", "show", true))
			{
				s_splashScreen = new SplashScreen(false, false);
				s_splashScreen.Show();
				s_splashScreen.Message = Properties.Resources.kstidSplashScreenLoadingMsg;
			}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// For versions of PA newer than 3.0.1, remove this property and let the splashscreen
		///// set the version using it's default way. I added this override to the default way
		///// because I had removed the "Beta" from the splash and about box too early and, as a
		///// result, there are too many test users with versions of the program labeled 3.0
		///// that are really beta or test versions. Therefore, I had to build my own version
		///// 3.0.1 using the Application.ProductVersion property. I could have just set that
		///// property to (in the properties for the Pa.exe project) but that would mess up
		///// the third number representing the build number which I want to keep at * so the
		///// build date is accurate (because that's what the third number is).
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static string ProdVersion
		//{
		//    get
		//    {
		//        Version ver = new Version(Application.ProductVersion);
		//        return string.Format("{0}.1", ver.ToString(2));
		//    }
		//}

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
		private static void ReadAddOns()
		{
			if (DesignMode)
				return;

			string[] addOnAssemblyFiles;

			try
			{
				string addOnPath = Path.GetDirectoryName(Application.ExecutablePath);
				addOnPath = Path.Combine(addOnPath, "AddOns");
				addOnAssemblyFiles = Directory.GetFiles(addOnPath, "*.dll");
			}
			catch
			{
				return;
			}

			if (addOnAssemblyFiles == null || addOnAssemblyFiles.Length == 0)
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
							if (s_addOnAssemblys == null)
								s_addOnAssemblys = new List<Assembly>();

							if (s_addOnManagers == null)
								s_addOnManagers = new List<object>();

							s_addOnAssemblys.Add(assembly);
							s_addOnManagers.Add(instance);
						}
					}
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void InitializePaRegKey()
		{
			string projPath = GetDefaultProjectFolder();

			// Check if an entry in the registry specifies the default project path.
			RegistryKey key = Registry.CurrentUser.CreateSubKey(kPaRegKeyName);

			if (key != null)
			{
				string tmpProjPath = key.GetValue("DefaultProjectsLocation") as string;

				// If the registry value was not found, then create it. Otherwise, use
				// the path found in the registry and not the one constructed above.
				if (string.IsNullOrEmpty(tmpProjPath))
					key.SetValue("DefaultProjectsLocation", projPath);
				else
					projPath = tmpProjPath;

				key.Close();
			}

			s_defaultProjFolder = projPath;

			// Create the folder if it doesn't exist.
			if (!Directory.Exists(projPath))
				Directory.CreateDirectory(projPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Construct the default project path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetDefaultProjectFolder()
		{
			// Construct the default project path.
			string projPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			// I found that in limited user mode on Vista, Environment.SpecialFolder.MyDocuments
			// returns an empty string. Argh!!! Therefore, I have to make sure there is
			// a valid and full path . Do that by getting the user's desktop folder and
			// chopping off everything that follows the last backslash. If getting the user's
			// desktop folder fails, then fall back to the program's folder, which is
			// probably not right, but we'll have to assume it will never happen. :o)
			if (string.IsNullOrEmpty(projPath))
			{
				projPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				if (string.IsNullOrEmpty(projPath) || !Directory.Exists(projPath))
					return Path.GetDirectoryName(Application.ExecutablePath);

				projPath = projPath.TrimEnd('\\');
				int i = projPath.LastIndexOf('\\');
				projPath = projPath.Substring(0, i);
			}

			return Path.Combine(projPath, Properties.Resources.kstidDefaultProjFileFolderName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The normal DesignMode property doesn't work when derived classes are loaded in
		/// designer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool DesignMode
		{
			get { return Process.GetCurrentProcess().ProcessName == "devenv"; }
		}

		#region Cache related properties and methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RecordCache RecordCache
		{
			get { return s_recCache; }
			set { s_recCache = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static WordCache WordCache
		{
			get { return s_wordCache; }
			set { s_wordCache = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the cache of phones from the data corpus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void BuildPhoneCache()
		{
			string conSymbol =
				s_settingsHndlr.GetStringSettingsValue("cvpattern", "consonantsymbol", "C");

			string vowSymbol =
				s_settingsHndlr.GetStringSettingsValue("cvpattern", "vowelsymbol", "V");

			s_phoneCache = new PhoneCache(conSymbol, vowSymbol);
			SearchEngine.PhoneCache = s_phoneCache;

			foreach (WordCacheEntry entry in s_wordCache)
			{
				string[] phones = entry.Phones;

				if (phones == null)
					continue;

				for (int i = 0; i < phones.Length; i++)
				{
					// Don't bother adding break characters.
					if (BreakChars.Contains(phones[i]))
						continue;

					if (!s_phoneCache.ContainsKey(phones[i]))
						s_phoneCache.AddPhone(phones[i]);

					// Determine if the current phone is the primary
					// phone in an uncertain group.
					bool isPrimaryUncertainPhone = (entry.ContiansUncertainties &&
						entry.UncertainPhones.ContainsKey(i));

					// When the phone is the primary phone in an uncertain group, we
					// don't add it to the total count but to the counter that keeps
					// track of the primary	uncertain phones. Then we also add to the
					// cache the non primary uncertain phones.
					if (!isPrimaryUncertainPhone)
						s_phoneCache[phones[i]].TotalCount++;
					else
					{
						s_phoneCache[phones[i]].CountAsPrimaryUncertainty++;

						// Go through the uncertain phones and add them to the cache.
						if (entry.ContiansUncertainties)
						{
							AddUncertainPhonesToCache(entry.UncertainPhones[i]);
							UpdateSiblingUncertaintys(entry.UncertainPhones);
						}
					}
				}
			}

			if (PhoneCache.FeatureOverrides != null)
				PhoneCache.FeatureOverrides.MergeWithPhoneCache(s_phoneCache);

			if (DataUtils.IPASymbolCache.UndefinedCharacters != null &&
				DataUtils.IPASymbolCache.UndefinedCharacters.Count > 0)
			{
				AddUndefinedCharsToCaches();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified list of uncertain phones to the phone cache. It is assumed the
		/// first (i.e. primary) phone in the list has already been added to the cache and,
		/// therefore, it will not be added nor its count incremented.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddUncertainPhonesToCache(string[] uncertainPhoneGroup)
		{
			// Go through the uncertain phone groups, skipping the
			// primary one in each group since that was already added
			// to the cache above.
			for (int i = 1; i < uncertainPhoneGroup.Length; i++)
			{
				string phone = uncertainPhoneGroup[i];

				// Don't bother adding break characters.
				if (!BreakChars.Contains(phone))
				{
					if (!s_phoneCache.ContainsKey(phone))
						s_phoneCache.AddPhone(phone);

					s_phoneCache[phone].CountAsNonPrimaryUncertainty++;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates a uncertain phone sibling lists for each phone in each uncertain group for
		/// the specified uncertain groups.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void UpdateSiblingUncertaintys(IDictionary<int, string[]> uncertainPhones)
		{
			// Go through the uncertain phone groups
			foreach (string[] uPhones in uncertainPhones.Values)
			{
				// Go through the uncertain phones in this group.
				for (int i = 0; i < uPhones.Length; i++)
				{
					IPhoneInfo phoneUpdating;
					
					// TODO: Log an error that the phone isn't found in the the cache
					// Get the cache entry for the phone whose sibling list will be updated.
					if (!s_phoneCache.TryGetValue(uPhones[i], out phoneUpdating))
						continue;

					// Go through the sibling phones, adding them to
					// the updated phones sibling list.
					for (int j = 0; j < uPhones.Length; j++)
					{
						// Add the phone pointed to by j if it's not the phone whose
						// cache entry we're updating and if it's not a phone already
						// in the sibling list of the cache entry we're updating.
						if (j != i && !phoneUpdating.SiblingUncertainties.Contains(uPhones[j]))
						{
							phoneUpdating.SiblingUncertainties.Add(
								IPASymbolCache.UncertainGroupAbsentPhoneChars.Contains(uPhones[j]) ?
								IPASymbolCache.UncertainGroupAbsentPhoneChar : uPhones[j]);
						}
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through all the undefined phonetic characters found in data sources and adds
		/// temporary (i.e. as long as this session of PA is running) records for them in the
		/// IPA character cache and the phone cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddUndefinedCharsToCaches()
		{
			foreach (UndefinedPhoneticCharactersInfo upci in DataUtils.IPASymbolCache.UndefinedCharacters)
			{
				DataUtils.IPASymbolCache.AddUndefinedCharacter(upci.Character);
				s_phoneCache.AddUndefinedPhone(upci.Character.ToString());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of phones in the current project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PhoneCache PhoneCache
		{
			get
			{
				if (s_phoneCache == null)
					BuildPhoneCache();

				return s_phoneCache;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void AddMediatorColleague(IxCoreColleague colleague)
		{
			if (colleague != null && !DesignMode)
			{
				s_msgMediator.AddColleague(colleague);

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
					s_msgMediator.RemoveColleague(colleague);
				}
				catch { }

				if (s_colleagueList.Contains(colleague))
					s_colleagueList.Remove(colleague);
			}
		}

		#region Misc. Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string BreakChars
		{
			get
			{
				if (s_breakChars == null)
				{
					s_breakChars = SettingsHandler.GetStringSettingsValue(
						"application", "wordbreakchars", " ");
				}

				return s_breakChars;
			}
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the XCore message mediator for the application.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static Mediator MsgMediator
		{
			get { return s_msgMediator; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the minimum allowed size of a view's window, including the main window that
		/// can hold all the views.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Size MinimumViewWindowSize
		{
			get { return s_minViewWindowSize; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the default location for PA projects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string DefaultProjectFolder
		{
			get { return s_defaultProjFolder; }
			set { s_defaultProjFolder = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's splash screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ISplashScreen SplashScreen
		{
			get { return s_splashScreen; }
			set { s_splashScreen = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the currently opened PA project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject Project
		{
			get { return s_project; }
			set { s_project = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a project is being loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ProjectLoadInProcess
		{
			get { return s_projectLoadInProcess; }
			set { s_projectLoadInProcess = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFieldInfoList FieldInfo
		{
			get
			{
				if (s_fieldInfo == null)
					s_fieldInfo = PaFieldInfoList.DefaultFieldInfoList;

				return s_fieldInfo;
			}
			set { s_fieldInfo = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the toolbar menu adapter PaMainWnd. This value should only be set
		/// by the PaMainWnd class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ITMAdapter TMAdapter
		{
			get {return s_tmAdapter;}
			set {s_tmAdapter = value;}
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and filename of the XML file that stores the application's
		/// settings.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string SettingsFile
		{
			get	{return s_settingsFile;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaSettingsHandler SettingsHandler
		{
			get {return s_settingsHndlr;}
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and filename of the application's help file.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string HelpFile
		{
			get {return null;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's main form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Form MainForm
		{
			get { return s_mainForm; }
			set { s_mainForm = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's current view form. When the view is docked, then
		/// this form will not be visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ITabView CurrentView
		{
			get { return s_currentView; }
			set { s_currentView = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the application's current view type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Type CurrentViewType
		{
			get { return s_currentViewType; }
			set { s_currentViewType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the main status bar label on PaMainWnd.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripStatusLabel StatusBarLabel
		{
			get
			{
				IUndockedViewWnd udvwnd = (s_currentView != null && s_currentView.ActiveView ?
					s_currentView.OwningForm : MainForm) as IUndockedViewWnd;

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
				IUndockedViewWnd udvwnd = (s_currentView != null && s_currentView.ActiveView ?
					s_currentView.OwningForm : MainForm) as IUndockedViewWnd;

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
				IUndockedViewWnd udvwnd = (s_currentView != null && s_currentView.ActiveView ?
					s_currentView.OwningForm : MainForm) as IUndockedViewWnd;

				return (udvwnd != null ? udvwnd.ProgressBarLabel : s_progressBarLabel);
			}
			set	{s_progressBarLabel = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// PA add-on DLL provides undocumented features, if it exists in the pa.exe folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<Assembly> AddOnAssemblys
		{
			get { return s_addOnAssemblys; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// PA add-on manager provides undocumented features, if it exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<object> AddOnManagers
		{
			get { return s_addOnManagers; }
		}

		#endregion

		#region Options Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the number of queries to remember in the recently used queries list
		/// in the search window's side panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int NumberOfRecentlyUsedQueries
		{
			get {return SettingsHandler.GetIntSettingsValue("recentlyusedqueries", "maxallowed", 20);}
			set {SettingsHandler.SaveSettingsValue("recentlyusedqueries", "maxallowed", value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color of the record view's field labels.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color RecordViewFieldLabelColor
		{
			get {return SettingsHandler.GetColorSettingsValue("recordviewcolors", "label", Color.DarkRed);}
			set {SettingsHandler.SaveSettingsValue("recordviewcolors", "label", value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color WordListGridColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue("wordlists", "gridcolor",
					ColorHelper.CalculateColor(SystemColors.Window, SystemColors.GrayText, 100));
			}
			set { SettingsHandler.SaveSettingsValue("wordlists", "gridcolor", value); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color SelectedFocusedWordListRowBackColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"selectedfocusedwordlistrowcolors", "background", ColorHelper.LightHighlight);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"selectedfocusedwordlistrowcolors", "background", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color SelectedFocusedWordListRowForeColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"selectedfocusedwordlistrowcolors", "foreground", SystemColors.WindowText);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"selectedfocusedwordlistrowcolors", "foreground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color SelectedUnFocusedWordListRowBackColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"selectedunfocusedwordlistrowcolors", "background", SystemColors.Control);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"selectedunfocusedwordlistrowcolors", "background", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color SelectedUnFocusedWordListRowForeColor
		{
			get
			{
				// It turns out the control color for the silver Windows XP theme is very
				// close to the default color calculated for selected rows in PA word lists.
				// Therefore, when a word list grid looses focus and a selected row's
				// background color gets changed to the control color, it's very hard to
				// tell the difference between a selected row in a focused grid from that
				// of a non focused grid. So, when the theme is the silver (i.e. Metallic)
				// then also make the text gray for selected rows in non focused grid's.
				if (PaintingHelper.CanPaintVisualStyle() &&
					System.Windows.Forms.VisualStyles.VisualStyleInformation.DisplayName == "Windows XP style" &&
					System.Windows.Forms.VisualStyles.VisualStyleInformation.ColorScheme == "Metallic")
				{
					return SettingsHandler.GetColorSettingsValue(
						"selectedunfocusedwordlistrowcolors", "foreground", SystemColors.GrayText);
				}

				return SettingsHandler.GetColorSettingsValue(
					"selectedunfocusedwordlistrowcolors", "foreground", SystemColors.ControlText);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"selectedunfocusedwordlistrowcolors", "foreground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color SelectedWordListCellBackColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"selectedwordlistcellcolors", "background", ColorHelper.LightLightHighlight);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"selectedwordlistcellcolors", "background", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color SelectedWordListCellForeColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"selectedwordlistcellcolors", "foreground", SystemColors.WindowText);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"selectedwordlistcellcolors", "foreground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color UncertainPhoneForeColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"uncertainphonecolors", "foreground", Color.RoyalBlue);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"uncertainphonecolors", "foreground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color QuerySearchItemForeColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"searchresultcolors", "srchitemforeground", Color.Black);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"searchresultcolors", "srchitemforeground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color QuerySearchItemBackColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"searchresultcolors", "srchitembackground", Color.Yellow);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"searchresultcolors", "srchitembackground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the background color for result cells in an XY grid whose value is
		/// zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color XYChartZeroBackColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"xychartcolors", "zerobackground", Color.PaleGoldenrod);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"xychartcolors", "zerobackground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the foreground color for result cells in an XY grid whose value is
		/// zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color XYChartZeroForeColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"xychartcolors", "zeroforeground", Color.Black);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"xychartcolors", "zeroforeground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the background color for result cells in an XY grid whose value is
		/// greater than zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color XYChartNonZeroBackColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"xychartcolors", "nonzerobackground", Color.LightSteelBlue);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"xychartcolors", "nonzerobackground", value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the foreground color for result cells in an XY grid whose value is
		/// greater than zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Color XYChartNonZeroForeColor
		{
			get
			{
				return SettingsHandler.GetColorSettingsValue(
					"xychartcolors", "nonzeroforeground", Color.Black);
			}
			set
			{
				SettingsHandler.SaveSettingsValue(
					"xychartcolors", "nonzeroforeground", value);
			}
		}
		
		#endregion

		#region Misc. methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the default phonology assistant menu in the specified container control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ITMAdapter LoadDefaultMenu(Control menuContainer)
		{
			ITMAdapter adapter = AdapterHelper.CreateTMAdapter();

			if (adapter != null)
			{
				PrepareAdapterForLocalizationSupport(adapter);

				string[] defs = new string[1];
				defs[0] = Path.Combine(Application.StartupPath, "PaTMDefinition.xml");
				adapter.Initialize(menuContainer, MsgMediator, ApplicationRegKeyPath, defs);
				adapter.AllowUpdates = true;
				adapter.RecentFilesList = RecentlyUsedProjectList;
				adapter.RecentlyUsedItemChosen += delegate(string filename)
				{
					MsgMediator.SendMessage("RecentlyUsedProjectChosen", filename);
				};
			}

			if (s_defaultMenuAdapters == null)
				s_defaultMenuAdapters = new List<ITMAdapter>();

			s_defaultMenuAdapters.Add(adapter);
			return adapter;
		}

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
			LocalizationManager.LocalizeObject(item, id, itemProps.Text, itemProps.Tooltip,
				ShortcutKeysEditor.KeysToString(itemProps.ShortcutKey), "Toolbar or Menu item",
				kLocalizationGroupTMItems, LocalizationPriority.High);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
			int maxruf = SettingsHandler.GetIntSettingsValue("recentlyusedfiles", "maxallowed", 10);

			List<string> rufList = new List<string>(RecentlyUsedProjectList);

			// First, remove the filename from the list if it's in there.
			if (rufList.Contains(filename))
				rufList.Remove(filename);

			if (addToEnd)
				rufList.Add(filename);
			else
				rufList.Insert(0, filename);

			for (int i = 1; i < maxruf && i <= rufList.Count; i++)
			{
				string entry = string.Format("ruf{0}", i);
				SettingsHandler.SaveSettingsValue(entry, "file", rufList[i - 1]);
			}

			if (s_defaultMenuAdapters != null)
			{
				foreach (ITMAdapter adapter in s_defaultMenuAdapters)
					adapter.RecentFilesList = rufList.ToArray();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of recently used projects from the PA settings file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string[] RecentlyUsedProjectList
		{
			get
			{
				int maxruf = SettingsHandler.GetIntSettingsValue("recentlyusedfiles", "maxallowed", 10);
				List<string> rufList = new List<string>(maxruf);

				for (int i = 1; i < maxruf; i++)
				{
					string entry = string.Format("ruf{0}", i);
					string filename = SettingsHandler.GetStringSettingsValue(entry, "file", null);
					if (File.Exists(filename))
						rufList.Add(filename);
				}

				return rufList.ToArray();
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
		/// Initializes the progress bar, assuming the max. value will be the count of items
		/// in the current project's word cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripProgressBar InitializeProgressBar(string text)
		{
			return InitializeProgressBar(text, s_wordCache.Count);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ToolStripProgressBar InitializeProgressBar(string text, int maxValue)
		{
			IUndockedViewWnd udvwnd = (s_currentView != null && s_currentView.ActiveView ?
				s_currentView.OwningForm : MainForm) as IUndockedViewWnd;

			ToolStripProgressBar bar =
				(udvwnd != null ? udvwnd.ProgressBar : s_progressBar);

			ToolStripStatusLabel lbl =
				(udvwnd != null ? udvwnd.ProgressBarLabel : s_progressBarLabel);

			if (bar != null)
			{
				bar.Maximum = maxValue;
				bar.Value = 0;
				lbl.Text = text;
				lbl.Visible = bar.Visible = true;
				s_activeProgBarLabel = lbl;
				s_activeProgressBar = bar;
				Utils.WaitCursors(true);
			}

			return s_progressBar;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the progress bar for the specified view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void InitializeProgressBarForLoadingView(string viewName, int maxValue)
		{
			string text = string.Format(Properties.Resources.kstidViewInitProgBarTemplate, viewName);
			InitializeProgressBar(text, maxValue);

			if (s_splashScreen != null && s_splashScreen.StillAlive)
				s_splashScreen.Message = text;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UninitializeProgressBar()
		{
			ToolStripProgressBar bar = (s_activeProgressBar ?? s_progressBar);
			ToolStripStatusLabel lbl = (s_activeProgBarLabel ?? s_progressBarLabel);

			if (bar != null)
				bar.Visible = false;

			if (lbl != null)
				lbl.Visible = false;

			s_activeProgBarLabel = null;
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
			ToolStripProgressBar bar = (s_activeProgressBar ?? s_progressBar);

			if (bar != null)
			{
				if (amount != -1 && (bar.Value + amount) <= bar.Maximum)
					bar.Value += amount;
				else
					bar.Value = bar.Maximum;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the progress bar label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UpdateProgressBarLabel(string label)
		{
			ToolStripStatusLabel lbl = (s_activeProgBarLabel ?? s_progressBarLabel);

			if (lbl != null)
			{
				if (s_splashScreen != null && s_splashScreen.StillAlive)
					s_splashScreen.Message = label;

				lbl.Text = label;
				Application.DoEvents();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the splash screen if it's showing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void CloseSplashScreen()
		{
			if (s_splashScreen != null && s_splashScreen.StillAlive)
			{
				Application.DoEvents();
				if (MainForm != null)
					MainForm.Activate();

				s_splashScreen.Close();
			}

			s_splashScreen = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string OpenFileDialog(string defaultFileType, string filter, string dlgTitle)
		{
			int filterIndex = 0;
			return OpenFileDialog(defaultFileType, filter, ref filterIndex,	dlgTitle);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string OpenFileDialog(string defaultFileType, string filter,
			ref int filterIndex, string dlgTitle)
		{
			string[] filenames = OpenFileDialog(defaultFileType, filter, ref filterIndex,
				dlgTitle, false, null);

			return (filenames == null || filenames.Length == 0 ? null : filenames[0]);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string[] OpenFileDialog(string defaultFileType, string filter,
			ref int filterIndex, string dlgTitle, bool multiSelect)
		{
			return OpenFileDialog(defaultFileType, filter, ref filterIndex,
				dlgTitle, multiSelect, null);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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

			return String.Empty;
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

			if (s_mainForm == null)
				return;

			// There may be some child forms not in the s_openForms collection. If that's
			// the case, then close them this way.
			for (int i = s_mainForm.MdiChildren.Length - 1; i >= 0; i--)
				s_mainForm.MdiChildren[i].Close();
		}

		#endregion

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

			if (Project != null)
				SearchEngine.IgnoreUndefinedCharacters = Project.IgnoreUndefinedCharsInSearches;

			SearchEngine.ConvertPatternWithExperimentalTrans =
				SettingsHandler.GetBoolSettingsValue("searchengine",
				"convertpatternswithexperimentaltrans", false);

			SearchEngine engine = new SearchEngine(modifiedQuery, PhoneCache);

			string msg = CombineErrorMessages(modifiedQuery.ErrorMessages.ToArray());
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

			WordListCache resultCache = (returnCountOnly ? null : new WordListCache());

			foreach (WordCacheEntry wordEntry in WordCache)
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
						List<string> tmpChars = new List<string>(eticWords[i]);
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
						errorMsg = Properties.Resources.kstidMissingClassMsg;
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool VerifyMiscPatternConditions(SearchEngine engine, bool showErrMsg)
		{
			if (engine == null)
				return false;

			string msg = null;

			if (engine.GetWordBoundaryCondition() != SearchEngine.WordBoundaryCondition.NoCondition)
				msg = Properties.Resources.kstidSrchPatternWordBoundaryError;
			else if (engine.GetZeroOrMoreCondition() != SearchEngine.ZeroOrMoreCondition.NoCondition)
				msg = Properties.Resources.kstidSrchPatternZeroOrMoreError;
			else if (engine.GetOneOrMoreCondition() != SearchEngine.OneOrMoreCondition.NoCondition)
				msg = Properties.Resources.kstidSrchPatternOneOrMoreError;

			if (engine.ErrorMessages != null && engine.ErrorMessages.Length > 0)
				msg = CombineErrorMessages(engine.ErrorMessages);

			if (msg != null && showErrMsg)
				Utils.MsgBox(msg);

			return (msg == null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Combines the list of error messages into a single message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string CombineErrorMessages(string[] errorMessages)
		{
			if (errorMessages == null || errorMessages.Length == 0)
				return null;

			StringBuilder errors = new StringBuilder();
			for (int i = 0; i < errorMessages.Length; i++)
			{
				errors.Append(errorMessages[i]);
				if (i < errorMessages.Length - 1)
					errors.Append('\n');
			}

			return string.Format(Properties.Resources.kstidPatternParsingErrorMsg, errors);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string HelpFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(s_helpFilePath))
				{
					s_helpFilePath = Application.StartupPath;
					//s_helpFilePath = Path.Combine(s_helpFilePath, kHelpSubFolder);
					s_helpFilePath = Path.Combine(s_helpFilePath, kHelpFileName);
				}

				return s_helpFilePath;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(Control ctrl)
		{
			ShowHelpTopic("hid" + ctrl.Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ShowHelpTopic(string hid)
		{
			if (File.Exists(HelpFilePath))
				Help.ShowHelp(new Label(), HelpFilePath, ResourceHelper.GetHelpString(hid));
			else
			{
				string msg = string.Format(Properties.Resources.kstidHelpFileMissingMsg,
					Utils.PrepFilePathForSTMsgBox(s_helpFilePath));
				
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}
}
