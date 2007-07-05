using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaProject : IDisposable
	{
		private Form m_appWindow;
		private bool m_reloadingProjectInProcess = false;
		private string m_name = Properties.Resources.kstidDefaultNewProjectName;
		private string m_fileName = null;
		private string m_language = null;
		private string m_transcriber = null;
		private string m_speakerName = null;
		private string m_comments = null;
		private List<PaDataSource> m_dataSources = new List<PaDataSource>();
		private SearchQueryGroupList m_queryGroups;
		private GridLayoutInfo m_gridLayoutInfo;
		private PaFieldInfoList m_fieldInfoList;
		private ExperimentalTranscriptions m_experimentalTransList;
		private AmbiguousSequences m_ambiguousSeqList;
		private List<CVPatternInfo> m_CVPatternInfoList;
		private SearchClassList m_classes;
		private SortOptions m_DataCorpusSortOptions;
		private SortOptions m_FindPhoneSortOptions;
		private SortOptions m_XYChartSortOptions;
		private CIEOptions m_cieOptions;
		private bool m_showUndefinedCharsDlg = true;
		private bool m_ignoreUndefinedCharsInSearches = true;
		private bool m_showClassNamesInSearchPatterns = true;
		private bool m_showDiamondsInEmptySearchPattern = true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject()
		{
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is really only for creating new projects. Therefore, the
		/// newProject argument should always be true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject(bool newProject)
		{
			if (newProject)
				m_fieldInfoList = PaFieldInfoList.DefaultFieldInfoList;
		}

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (m_appWindow != null)
			{
				m_appWindow.Activated -= appWindow_Activated;
				m_appWindow = null;
			}

			if (m_dataSources != null)
				m_dataSources.Clear();

			if (m_classes != null)
			    m_classes.Clear();

			if (m_queryGroups != null)
				m_queryGroups.Clear();

			if (m_fieldInfoList != null)
				m_fieldInfoList.Clear();

			if (m_experimentalTransList != null)
			    m_experimentalTransList.Clear();

			if (m_ambiguousSeqList != null)
				m_ambiguousSeqList.Clear();

			m_dataSources = null;
			m_classes = null;
			m_queryGroups = null;
			m_fieldInfoList = null;
			m_experimentalTransList = null;
			m_ambiguousSeqList = null;
		}

		#endregion

		#region Loading and saving
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new project object, loading its information from the specified
		/// project file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject Load(string projFileName)
		{
			return Load(projFileName, null);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new project object, loading its information from the specified
		/// project file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject Load(string projFileName, Form appWindow)
		{
			string errorMsg = null;
			PaProject project = null;

			if (!File.Exists(projFileName))
			{
				errorMsg = string.Format(Properties.Resources.kstidProjectFileNonExistent,
					projFileName);
			}

			string tmpProjPathFilePrefix = Path.GetFullPath(projFileName);
			int i = tmpProjPathFilePrefix.LastIndexOf(".");
			if (i >= 0)
				tmpProjPathFilePrefix = tmpProjPathFilePrefix.Remove(i);

			if (errorMsg == null)
				errorMsg = LoadCacheFiles(tmpProjPathFilePrefix);

			if (errorMsg == null)
				project = LoadProjectFileOnly(projFileName, false, ref errorMsg);

			if (errorMsg == null)
			{
				project.LoadDataSources();
				if (appWindow != null)
				{
					appWindow.Activated += new EventHandler(project.appWindow_Activated);
					project.m_appWindow = appWindow;
				}
			}
			else
			{
				STUtils.STMsgBox(errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				project = null;
			}

			return project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads only the project file for the specified file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject LoadProjectFileOnly(string projFileName, bool showErrors,
			ref string errorMsg)
		{
			PaProject project = null;

			try
			{
				project = STUtils.DeserializeData(projFileName, typeof(PaProject)) as PaProject;
				PhoneCache.CVPatternInfoList = project.m_CVPatternInfoList;
				project.m_fileName = projFileName;
				project.LoadFieldInfo();
				RecordCacheEntry.InitializeDataSourceFields(project.FieldInfo);
			}
			catch (Exception e)
			{
				if (project == null)
				{
					errorMsg = string.Format(Properties.Resources.kstidErrorProjectInvalidFormat,
						projFileName);
				}
				else
				{
					errorMsg = string.Format(Properties.Resources.kstidErrorLoadingProject,
						projFileName, e.Message);
				}

				if (showErrors)
					STUtils.STMsgBox(errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				project = null;
			}

			return project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads the project without reloading the data sources and returns a
		/// project object that represents the reloaded project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject ReLoadProjectFileOnly(bool updateModfiedTimesInReloadedProject)
		{
			string errorMsg = null;
			PaProject project = LoadProjectFileOnly(m_fileName, true, ref errorMsg);

			if (project != null)
			{
				if (m_appWindow != null)
				{
					m_appWindow.Activated -= appWindow_Activated;
					m_appWindow.Activated += new EventHandler(project.appWindow_Activated);
					project.m_appWindow = m_appWindow;
				}

				if (updateModfiedTimesInReloadedProject)
				{
					// Reloading a project resets all the data source's last modified
					// times to a default date a long, long time ago. Therefore, copy
					// the last modified times from this project's data sources.
					CopyLastModifiedTimes(project);
				}

				project.ExperimentalTranscriptions =
					ExperimentalTranscriptions.Load(ProjectPathFilePrefix);

				project.AmbiguousSequences = PhoneCache.AmbiguousSequences =
					AmbiguousSequences.Load(ProjectPathFilePrefix);
			}

			return project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Copies the last modified times of the data sources in the project to the data
		/// sources in the specified target project. The data source names must match.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CopyLastModifiedTimes(PaProject targetProj)
		{
			// Go through each data source in this project.
			foreach (PaDataSource ds in DataSources)
			{
				// Then to through the data sources in the target project. When a data
				// source in the target project matches one in this project, then update
				// its last modified time with the one found in this project's data source.
				foreach (PaDataSource tgtDs in targetProj.DataSources)
				{
					if (tgtDs.DataSourceType != DataSourceType.FW ||
						!tgtDs.FwSourceDirectFromDB)
					{
						if (tgtDs.DataSourceFile == ds.DataSourceFile)
							tgtDs.LastModification = ds.LastModification;
					}
					else if (tgtDs.FwDataSourceInfo.ProjectName == ds.FwDataSourceInfo.ProjectName)
						tgtDs.LastModification = ds.LastModification;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the projects field information from its XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadFieldInfo()
		{
			try
			{
				m_fieldInfoList = PaFieldInfoList.Load(this);
				PaApp.FieldInfo = m_fieldInfoList;
				InitializeFontHelperFonts();
				DataCorpusSortOptions.SyncFieldInfo(m_fieldInfoList);
				FindPhoneSortOptions.SyncFieldInfo(m_fieldInfoList);
				XYChartSortOptions.SyncFieldInfo(m_fieldInfoList);
			}
			catch (Exception e)
			{
				STUtils.STMsgBox(
					string.Format(Properties.Resources.kstidErrorLoadingProjectFieldInfo,
					m_name, e.Message), MessageBoxButtons.OK);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets all the font helper fonts to those specified in the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeFontHelperFonts()
		{
			if (m_fieldInfoList == null)
				return;
			
			foreach (PaFieldInfo fieldInfo in m_fieldInfoList)
			{
				if (fieldInfo.Font == null)
					continue;

				try
				{
					BindingFlags flags = BindingFlags.SetProperty | BindingFlags.Static |
						BindingFlags.Public;

					typeof(FontHelper).InvokeMember(fieldInfo.FieldName + "Font",
							flags, null, typeof(FontHelper), new object[] { fieldInfo.Font });
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string LoadCacheFiles(string projPathFilePrefix)
		{
			PaApp.InitializeProgressBar(Properties.Resources.kstidReadingCacheFiles, 3);

			try
			{
				// Load the caches.
				DataUtils.LoadIPACharCache(projPathFilePrefix);
				PaApp.IncProgressBar();
				DataUtils.LoadAFeatureCache(projPathFilePrefix);
				PaApp.IncProgressBar();
				DataUtils.LoadBFeatureCache(projPathFilePrefix);
				PaApp.IncProgressBar();
			}
			catch (Exception e)
			{
				return string.Format(Properties.Resources.kstidErrorLoadingCacheFiles, e.Message);
			}
			finally
			{
				PaApp.UninitializeProgressBar();
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Go through the data source files and determine if any have changed since the
		/// application was deactivated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void appWindow_Activated(object sender, EventArgs e)
		{
			if (m_reloadingProjectInProcess)
				return;

			// We don't want to bother with updating just after a message box has been shown.
			// Especially if the chain of events that triggered displaying the message box was
			// started in this method. In that case, we'd get into an infinite loop of
			// displaying the message box.
			if (STUtils.MessageBoxJustShown)
			{
				STUtils.MessageBoxJustShown = false;
				return;
			}

			foreach (PaDataSource source in m_dataSources)
			{
				if (source.SkipLoading)
					continue;

				DateTime latestModification = DateTime.MinValue;
				bool reloadFWdb = false;

				// For FW data sources, get the last modified time from the FW database.
				// For SA data sources, get the last modified time from the SA database.
				// For all other data sources, get the date and time stamp on the data
				// source file.
				if (source.DataSourceType == DataSourceType.FW && source.FwSourceDirectFromDB)
					reloadFWdb = source.FwDataSourceInfo.UpdateLastModifiedStamp();
				else if (source.DataSourceType == DataSourceType.SA)
				{
					latestModification =
						SaAudioDocument.GetTranscriptionFileModifiedTime(source.DataSourceFile);
				}
				else
				{
					if (File.Exists(source.DataSourceFile))
						latestModification = File.GetLastWriteTimeUtc(source.DataSourceFile);
				}

				if (source.LastModification < latestModification || reloadFWdb)
				{
					ReloadDataSources();
					break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads the project's data sources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ReloadDataSources()
		{
			m_reloadingProjectInProcess = true;
			LoadDataSources();
			PaApp.MsgMediator.SendMessage("DataSourcesModified", ProjectFileName);
			m_reloadingProjectInProcess = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadDataSources()
		{
			ExperimentalTranscriptions = ExperimentalTranscriptions.Load(ProjectPathFilePrefix);

			WordCacheEntry.ExperimentalTranscriptionList =
				DataUtils.IPACharCache.ExperimentalTranscriptions.ConversionList;

			AmbiguousSequences = AmbiguousSequences.Load(ProjectPathFilePrefix);
			PhoneCache.AmbiguousSequences = AmbiguousSequences;
			PhoneCache.FeatureOverrides = PhoneFeatureOverrides.Load(ProjectPathFilePrefix);
			DataSourceReader reader = new DataSourceReader(this);
			reader.Read();
			WordCacheEntry.ExperimentalTranscriptionList = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the project to it's specified project file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			m_fieldInfoList.Save(this);

			if (m_fileName != null)
				STUtils.SerializeData(m_fileName, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure that if any of the sort options have their save manual flag set, that
		/// the project is saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureSortOptionsSaved()
		{
			if ((m_DataCorpusSortOptions != null && m_DataCorpusSortOptions.SaveManuallySetSortOptions) ||
				(m_FindPhoneSortOptions != null && m_FindPhoneSortOptions.SaveManuallySetSortOptions) ||
				(m_XYChartSortOptions != null && m_XYChartSortOptions.SaveManuallySetSortOptions))
			{
				Save();
			}
		}

		#endregion

		#region Loading/Saving Caches
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadIPACharCache()
		{
			DataUtils.LoadIPACharCache(ProjectPathFilePrefix);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadAFeatureCache()
		{
			DataUtils.LoadAFeatureCache(ProjectPathFilePrefix);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadBFeatureCache()
		{
			DataUtils.LoadBFeatureCache(ProjectPathFilePrefix);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveIPACharCache()
		{
			DataUtils.IPACharCache.Save(ProjectPathFilePrefix);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveAFeatureCache()
		{
			DataUtils.AFeatureCache.Save(ProjectPathFilePrefix);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SaveBFeatureCache()
		{
			DataUtils.BFeatureCache.Save(ProjectPathFilePrefix);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path where the project is stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectPath
		{
			get	{return System.IO.Path.GetDirectoryName(m_fileName);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path where the project is stored plus a backslash, the project name
		/// and a period to be used for creating project specific files for persisting project
		/// data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectPathFilePrefix
		{
			get
			{
				return System.IO.Path.Combine(string.IsNullOrEmpty(m_fileName) ? string.Empty :
					System.IO.Path.GetDirectoryName(m_fileName), m_name) + ".";
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectFileName
		{
			get { return m_fileName; }
			set
			{
				// Only allow this when there hasn't already been a file name specified.
				// This should only be the case when creating new projects.
				if (m_fileName == null)
					m_fileName = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the project file without it's path or extension.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectFileNameWOExt
		{
			get
			{
				return (string.IsNullOrEmpty(m_fileName) ?
					string.Empty : Path.GetFileNameWithoutExtension(m_fileName));
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ProjectName
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Language
		{
			get { return m_language; }
			set { m_language = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber
		{
			get { return m_transcriber; }
			set { m_transcriber = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SpeakerName
		{
			get { return m_speakerName; }
			set { m_speakerName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Comments
		{
			get { return m_comments; }
			set { m_comments = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets an Options value indicating whether or not class names are shown in
		/// search patterns and nested class definitions. If this value is false, then class
		/// members are shown instead.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowClassNamesInSearchPatterns
		{
			get {return m_showClassNamesInSearchPatterns;}
			set {m_showClassNamesInSearchPatterns = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to display the diamond
		/// pattern when the find phones search pattern text box is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowDiamondsInEmptySearchPattern
		{
			get {return m_showDiamondsInEmptySearchPattern;}
			set {m_showDiamondsInEmptySearchPattern = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to ignore in phonetic searches
		/// (i.e. on the Search view and XY Charts views) undefined phonetic characters found
		/// in data sources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IgnoreUndefinedCharsInSearches
		{
			get { return m_ignoreUndefinedCharsInSearches; }
			set { m_ignoreUndefinedCharsInSearches = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<PaDataSource> DataSources
		{
			get { return m_dataSources; }
			set { m_dataSources = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to display the undefined phonetic
		/// characters dialog whenever the data sources for the project are read. If no
		/// undefined phonetic characters are found in any of the project's data sources, then,
		/// of course, the dialog won't be displayed regardless of the value of this property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool ShowUndefinedCharsDlg
		{
			get { return m_showUndefinedCharsDlg; }
			set { m_showUndefinedCharsDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's list of ambiguous sequences.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public AmbiguousSequences AmbiguousSequences
		{
			get { return m_ambiguousSeqList; }
			set
			{
				m_ambiguousSeqList = value;
				DataUtils.IPACharCache.AmbiguousSequences = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's list of experimental transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public ExperimentalTranscriptions ExperimentalTranscriptions
		{
			get { return m_experimentalTransList; }
			set
			{
				m_experimentalTransList = value;

				// The IPA char. cache keeps it's own clone of the experimental transcriptions
				// since it needs to keep them in a sorted order that may not be the way
				// the user entered them on the phone inventory tab.
				DataUtils.IPACharCache.ExperimentalTranscriptions = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets and sets the CVPatternInfoList.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<CVPatternInfo> CVPatternInfoList
		{
			get
			{
				if (m_CVPatternInfoList == null)
					m_CVPatternInfoList = new List<CVPatternInfo>();
				
				return m_CVPatternInfoList;
			}
			set
			{
				m_CVPatternInfoList = value;
				PhoneCache.CVPatternInfoList = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field information for the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PaFieldInfoList FieldInfo
		{
			get { return m_fieldInfoList; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SearchQueryGroupList SearchQueryGroups
		{
			get
			{
				if (m_queryGroups == null)
					m_queryGroups = SearchQueryGroupList.Load();

				return m_queryGroups;
			}
			set { m_queryGroups = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SearchClassList SearchClasses
		{
			get
			{
				if (m_classes == null)
					m_classes = SearchClassList.Load();

				return m_classes;
			}
			set { m_classes = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & Set the DataCorpusSortOptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions DataCorpusSortOptions
		{
			get
			{
				if (m_DataCorpusSortOptions == null)
				{
					m_DataCorpusSortOptions = new SortOptions(true);
					m_DataCorpusSortOptions.AdvancedEnabled = false;
				}

				return m_DataCorpusSortOptions;
			}
			set
			{
				m_DataCorpusSortOptions = value;
				if (value != null)
					value.AdvancedEnabled = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get & Set the FindPhoneSortOptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions FindPhoneSortOptions
		{
			get
			{
				if (m_FindPhoneSortOptions == null)
				{
					m_FindPhoneSortOptions = new SortOptions(true);
					m_FindPhoneSortOptions.AdvancedEnabled = true;
				}

				return m_FindPhoneSortOptions;
			}
			
			set 
			{
				m_FindPhoneSortOptions = value;
				if (value != null)
					value.AdvancedEnabled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the sort options applied to word lists in XY Chart view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions XYChartSortOptions
		{
			get
			{
				if (m_XYChartSortOptions == null)
				{
					m_XYChartSortOptions = new SortOptions(true);
					m_XYChartSortOptions.AdvancedEnabled = true;
				}
				
				return m_XYChartSortOptions;
			}
			set
			{
				m_XYChartSortOptions = value;
				if (value != null)
					value.AdvancedEnabled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the options applied to search results the user views using the
		/// minimal pairs feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CIEOptions CIEOptions
		{
			get
			{
				if (m_cieOptions == null)
					m_cieOptions = new CIEOptions();

				return m_cieOptions;
			}
			set	{m_cieOptions = (value == null ? new CIEOptions() : value);}
		}
		
		#endregion

		#region Grid and Record View layout properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GridLayoutInfo GridLayoutInfo
		{
			get {return (m_gridLayoutInfo != null ? m_gridLayoutInfo : new GridLayoutInfo());}
			set
			{
				m_gridLayoutInfo = (value != null ? value : new GridLayoutInfo());
				m_gridLayoutInfo.m_owningProject = this;
			}
		}

		#endregion
	}

	#region GridLayoutInfo Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class GridLayoutInfo
	{
		public System.Windows.Forms.DataGridViewCellBorderStyle GridLines =
			DataGridViewCellBorderStyle.Single;

		public int ColHeaderHeight = -1;
		public bool SaveReorderedCols = true;
		public bool SaveAdjustedColHeaderHeight = true;
		public bool SaveAdjustedColWidths = true;
		public bool AutoAdjustPhoneticCol = true;
		public int AutoAjustedMaxWidth = 200;
		internal PaProject m_owningProject;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the specified grid with values from the save layout information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load(DataGridView grid)
		{
			if (grid == null || grid.Columns.Count == 0 || m_owningProject == null)
				return;

			SilHierarchicalGridColumn.ShowHierarchicalColumns(grid, false, true, false);
			grid.CellBorderStyle = GridLines;
			
			// Set the column properties to the saved values.
			for (int i = 0; i < grid.Columns.Count; i++)
			{
				PaFieldInfo fieldInfo = m_owningProject.FieldInfo[grid.Columns[i].Name];
				if (fieldInfo == null)
					continue;

				if (fieldInfo.DisplayIndexInGrid < 0)
					grid.Columns[i].Visible = false;
				else
				{
					grid.Columns[i].Visible = fieldInfo.VisibleInGrid;
					grid.Columns[i].DisplayIndex =
						(fieldInfo.DisplayIndexInGrid < grid.Columns.Count ?
						fieldInfo.DisplayIndexInGrid : grid.Columns.Count - 1);
				}
			}

			SilHierarchicalGridColumn.ShowHierarchicalColumns(grid, true, false, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves some of the specified grid's properties.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(DataGridView grid)
		{
			if (grid == null || grid.Columns.Count == 0)
				return;

			GridLines = grid.CellBorderStyle;

			if (SaveAdjustedColHeaderHeight)
				ColHeaderHeight = grid.ColumnHeadersHeight;

			// Save the list of columns sorted by display index.
			SortedList<int, PaFieldInfo> displayIndexes = new SortedList<int, PaFieldInfo>();

			// Save the specified grid's column properties.
			foreach (DataGridViewColumn col in grid.Columns)
			{
				PaFieldInfo fieldInfo = m_owningProject.FieldInfo[col.Name];
				if (fieldInfo != null)
				{
					if (SaveAdjustedColWidths)
						fieldInfo.WidthInGrid = col.Width;

					if (SaveReorderedCols)
						displayIndexes[col.DisplayIndex] = fieldInfo;
				}
			}

			if (displayIndexes.Count == 0)
				return;

			// The display index order saved with the fields should begin with zero, but
			// since the grid may have some SilHerarchicalColumns showing, the first field's
			// display index may be greater than 1. Therefore, we adjust for that by setting
			// the display indexes in sequence beginning from zero.
			int i = 0;
			foreach (PaFieldInfo fieldInfo in displayIndexes.Values)
				fieldInfo.DisplayIndexInGrid = i++;
		}
	}

	#endregion
}
