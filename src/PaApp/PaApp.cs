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
using SIL.Pa.Data;
using SIL.Pa.FFSearchEngine;
using SIL.Pa.Resources;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface ITabView
	{
		Control DockableContainer { get;}
		void ViewDocking();
		void ViewDocked();
		void ViewUndocking();
		void ViewUndocked();
		void ViewActivatedWhileDocked();
		void SaveSettings();
		ToolStripProgressBar ProgressBar { get;}
		ToolStripStatusLabel ProgressBarLabel { get;}
		ToolStripStatusLabel StatusBarLabel { get;}
		StatusStrip StatusBar { get;}
		ToolTip ViewsToolTip { get;}
		ITMAdapter TMAdapter { get;}
	}

	/// ------------------------------------------------------------------------------------
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
		
		public static string kOpenClassBracket = ResourceHelper.GetString("kstidOpenClassSymbol");
		public static string kCloseClassBracket = ResourceHelper.GetString("kstidCloseClassSymbol");
		public const string kSQLServerOptions = "sqlserver";
		public const string kHelpFileName = "Phonology_Assistant_Help.chm";
		public const string kHelpSubFolder = "Helps";
		public const string kPaRegKeyName = @"Software\SIL\Phonology Assistant";

		private static string s_helpFilePath = null;
		private static ITMAdapter s_tmAdapter;
		private static ToolStripStatusLabel s_statusBarLabel;
		private static ToolStripProgressBar s_progressBar;
		private static ToolStripStatusLabel s_progressBarLabel;
		private static ToolStripProgressBar s_savProgressBar;
		private static ToolStripStatusLabel s_savProgressBarLabel;
		private static bool s_statusBarHasBeenInitialized = false;
		private static Form s_mainForm;
		private static Form s_currentView;
		private static Type s_currentViewType;
		private static bool s_projectLoadInProcess = false;
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
			if (Application.ExecutablePath.ToLower().EndsWith("pa.exe"))
			{
				s_splashScreen = new SplashScreen(false, false);
				s_splashScreen.Show();
				s_splashScreen.Message = Properties.Resources.kstidSplashScreenLoadingMsg;
			}

			InitializePaRegKey();
			
			s_msgMediator = new Mediator();
			s_settingsFile = Path.Combine(s_defaultProjFolder, "pa.xml");
			s_settingsHndlr = new PaSettingsHandler(s_settingsFile);

			// Create the master set of PA fields. When a project is opened, any
			// custom fields belonging to the project will be added to this list.
			s_fieldInfo = PaFieldInfoList.DefaultFieldInfoList;

			s_minViewWindowSize = new Size(
				s_settingsHndlr.GetIntSettingsValue("minviewwindowsize", "width", 550),
				s_settingsHndlr.GetIntSettingsValue("minviewwindowsize", "height", 450));

			ReadAddOns();
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
			// Construct the default project path.
			string projPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			projPath = Path.Combine(projPath, Application.ProductName);

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
			set
			{
				if (value != null && s_wordCache != value)
				{
					s_wordCache = value;
					BuildPhoneCache();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the cache of phones from the data corpus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void BuildPhoneCache()
		{
			s_phoneCache = new PhoneCache();
			SearchEngine.PhoneCache = s_phoneCache;

			foreach (WordCacheEntry entry in s_wordCache)
			{
				string[] phones = entry.Phones;

				if (phones == null)
					continue;

				for (int i = 0; i < phones.Length; i++)
				{
					// Don't bother adding break characters.
					if (IPACharCache.kBreakChars.Contains(phones[i]))
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
							AddUncertainPhonesToCache(entry.UncertainPhones);
							UpdateSiblingUncertaintys(entry.UncertainPhones);
						}
					}
				}
			}

			if (PhoneCache.FeatureOverrides != null)
				PhoneCache.FeatureOverrides.MergeWithPhoneCache(s_phoneCache);

			if (DataUtils.IPACharCache.UndefinedCharacters != null &&
				DataUtils.IPACharCache.UndefinedCharacters.Count > 0)
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
		private static void AddUncertainPhonesToCache(IDictionary<int, string[]> uncertainPhones)
		{
			// Go through the uncertain phone groups, skipping the
			// primary one in each group since that was already added
			// to the cache above.
			foreach (string[] uPhones in uncertainPhones.Values)
			{
				for (int i = 1; i < uPhones.Length; i++)
				{
					// Don't bother adding break characters.
					if (!IPACharCache.kBreakChars.Contains(uPhones[i]))
					{
						if (!s_phoneCache.ContainsKey(uPhones[i]))
							s_phoneCache.AddPhone(uPhones[i]);

						s_phoneCache[uPhones[i]].CountAsNonPrimaryUncertainty++;
					}
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
							phoneUpdating.SiblingUncertainties.Add(uPhones[j]);
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
			foreach (UndefinedPhoneticCharactersInfo upci in DataUtils.IPACharCache.UndefinedCharacters)
			{
				DataUtils.IPACharCache.AddUndefinedCharacter(upci.Character);
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
		public static Form CurrentView
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
				if (s_currentView != null && s_currentView.Visible && s_currentView is ITabView &&
					((ITabView)s_currentView).StatusBarLabel != null)
				{
					return ((ITabView)s_currentView).StatusBarLabel;
				}

				return s_statusBarLabel;
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
				if (s_currentView != null && s_currentView.Visible && s_currentView is ITabView &&
					((ITabView)s_currentView).ProgressBar != null)
				{
					return ((ITabView)s_currentView).ProgressBar;
				}
			
				return s_progressBar;
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
				if (s_currentView != null && s_currentView.Visible && s_currentView is ITabView &&
					((ITabView)s_currentView).ProgressBarLabel != null)
				{
					return ((ITabView)s_currentView).ProgressBarLabel;
				}

				return s_progressBarLabel;
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
			get {return SettingsHandler.GetColorSettingsValue("recordviewcolors", "lable", Color.DarkRed);}
			set {SettingsHandler.SaveSettingsValue("recordviewcolors", "label", value);}
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
			if (s_progressBar != null)
			{
				if (s_mainForm != null)
				{
					Application.UseWaitCursor = true;
					Application.DoEvents();
				}

				// Save the current progress bar and initialize s_progressBar with the one
				// returned by the property since it may not be the same. Normally, the one
				// stored in s_progressBar is the one on the main form but the one returned
				// from the property may be one from an undocked form. s_progressBar will
				// be restored to the one on the main form in UninitializeProgressBar.
				if (!s_statusBarHasBeenInitialized)
				{
					s_savProgressBar = s_progressBar;
					s_savProgressBarLabel = s_progressBarLabel;
					s_progressBar = ProgressBar;
					s_progressBarLabel = ProgressBarLabel;
					s_statusBarHasBeenInitialized = true;
				}

				s_progressBar.Maximum = maxValue;
				s_progressBar.Value = 0;
				s_progressBarLabel.Text = text;
				s_progressBarLabel.Visible = s_progressBar.Visible = true;
				Application.DoEvents();
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
			if (s_progressBar != null)
				s_progressBar.Visible = false;

			if (s_progressBarLabel != null)
				s_progressBarLabel.Visible = false;

			if (s_statusBarHasBeenInitialized)
			{
				if (s_savProgressBar != null)
					s_progressBar = s_savProgressBar;

				if (s_savProgressBarLabel != null)
					s_progressBarLabel = s_savProgressBarLabel;

				s_savProgressBar = null;
				s_savProgressBarLabel = null;
				s_statusBarHasBeenInitialized = false;
			}

			Application.UseWaitCursor = false;
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
			if (s_progressBar != null)
			{
				if (amount != -1 && (s_progressBar.Value + amount) <= s_progressBar.Maximum)
					s_progressBar.Value += amount;
				else
					s_progressBar.Value = s_progressBar.Maximum;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the progress bar label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void UpdateProgressBarLabel(string label)
		{
			if (s_progressBarLabel != null)
			{
				if (s_splashScreen != null && s_splashScreen.StillAlive)
					s_splashScreen.Message = label;
				
				s_progressBarLabel.Text = label;
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
	
			SearchEngine engine = new SearchEngine(modifiedQuery, PhoneCache);
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
					SearchClass srchClass = Project.SearchClasses[className];
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
							STUtils.STMsgBox(errorMsg, MessageBoxButtons.OK,
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
			{
				StringBuilder errors = new StringBuilder();
				for (int i = 0; i < engine.ErrorMessages.Length; i++)
				{
					errors.Append(engine.ErrorMessages[i]);
					if (i < engine.ErrorMessages.Length - 1)
						errors.Append('\n');
				}

				msg = string.Format(Properties.Resources.kstidPatternParsingErrorMsg, errors);
			}

			if (msg != null && showErrMsg)
				STUtils.STMsgBox(msg, MessageBoxButtons.OK);

			return (msg == null);
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
					s_helpFilePath = Path.GetDirectoryName(Application.ExecutablePath);
					s_helpFilePath = Path.Combine(s_helpFilePath, kHelpSubFolder);
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
					STUtils.PrepFilePathForSTMsgBox(s_helpFilePath));
				
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}
}
