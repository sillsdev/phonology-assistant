using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Localization;
using SIL.Pa.DataSource;
using SIL.Pa.Filters;
using SIL.Pa.Model;
using SIL.Pa.PhoneticSearching;
using SIL.SpeechTools.Utils;
using SilUtils;

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
		private bool m_newProject;
		private bool m_reloadingProjectInProcess;
		private string m_fileName;
		private SearchQueryGroupList m_queryGroups;
		private GridLayoutInfo m_gridLayoutInfo;
		private PaFieldInfoList m_fieldInfoList;
		private List<CVPatternInfo> m_CVPatternInfoList;
		private SearchClassList m_classes;
		private SortOptions m_dataCorpusVwSortOptions;
		private SortOptions m_searchVwSortOptions;
		private SortOptions m_xyChartVwSortOptions;
		private CIEOptions m_cieOptions;
		private Filter m_loadedFilter;
		private bool m_showClassNamesInSearchPatterns = true;
		private bool m_showDiamondsInEmptySearchPattern = true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject()
		{
			Name = Properties.Resources.kstidDefaultNewProjectName;
			ShowUndefinedCharsDlg = true;
			IgnoreUndefinedCharsInSearches = true;
			DataSources = new List<PaDataSource>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is really only for creating new projects. Therefore, the
		/// newProject argument should always be true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject(bool newProject)
		{
			Name = Properties.Resources.kstidDefaultNewProjectName;
			ShowUndefinedCharsDlg = true;
			IgnoreUndefinedCharsInSearches = true;
			DataSources = new List<PaDataSource>();
			if (newProject)
			{
				m_fieldInfoList = PaFieldInfoList.DefaultFieldInfoList;
				m_classes = SearchClassList.Load();
				m_queryGroups = SearchQueryGroupList.Load();
				m_newProject = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will make sure the list of mappings in all SFM and Toolbox data
		/// sources doesn't contain a mapping for a field that was removed. Also, if the
		/// Interlinear status of a custom field changed, this method will make sure the
		/// mappings don't have contradictory values for the interlinear status of a field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CleanUpMappings()
		{
			if (DataSources == null)
				return;

			// Go through the list of data sources in the project and clean up the
			// mappings for projects of type SFM and Toolbox.
			foreach (PaDataSource source in DataSources)
			{
				if (source.DataSourceType != DataSourceType.SFM &&
					source.DataSourceType != DataSourceType.Toolbox)
				{
					continue;
				}

				// Go through the mappings in the data source and make sure the field
				// for each mapping is still in the list of fields in the project.
				for (int i = source.SFMappings.Count - 1; i >= 0; i--)
				{
					SFMarkerMapping mapping = source.SFMappings[i];

					if (mapping.FieldName != PaDataSource.kRecordMarker)
					{
						PaFieldInfo fieldInfo = m_fieldInfoList[mapping.FieldName];

						// If the mapped field no longer exists, then remove it from the data
						// source's list of mappings. Otherwise, make sure that the mapping
						// for the field doesn't think it is an interlinear field if it no
						// longer is.
						if (fieldInfo == null)
							source.SFMappings.RemoveAt(i);
						else if (!fieldInfo.CanBeInterlinear)
							mapping.IsInterlinear = false;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure that FW, SFM and Toolbox data sources are informed that a field has
		/// changed names.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void MigrateFwDatasourceFieldNames()
		{
			// Only change these names. The others are just converted to lowercase.
			var oldNames = new List<string>(new[] {"Gloss", "GlossOther1", "GlossOther2"});
			var newNames = new List<string>(new[] {"gloss1", "gloss2", "gloss3"});

			foreach (PaDataSource source in DataSources)
			{
				if (source.DataSourceType == DataSourceType.FW &&
					source.FwDataSourceInfo != null &&
					source.FwDataSourceInfo.WritingSystemInfo != null)
				{
					foreach (FwDataSourceWsInfo wsi in source.FwDataSourceInfo.WritingSystemInfo)
					{
						int i = oldNames.IndexOf(wsi.FieldName);
						wsi.FieldName = (i < 0 ? wsi.FieldName.ToLower() : newNames[i]);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure that SFM and Toolbox data sources are informed that a field has
		/// changed names. This is so the SF mappings in the data source can be updated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ProcessRenamedField(string origName, string newName)
		{
			foreach (PaDataSource source in DataSources)
				source.RenameField(origName, newName);

			ProcessRenamedFieldInSortInfo(origName, newName, m_dataCorpusVwSortOptions);
			ProcessRenamedFieldInSortInfo(origName, newName, m_searchVwSortOptions);
			ProcessRenamedFieldInSortInfo(origName, newName, m_xyChartVwSortOptions);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the specified sort information list to make sure the specified field
		/// name gets renamed therein.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ProcessRenamedFieldInSortInfo(string origName, string newName,
			SortOptions sortOptions)
		{
			if (!string.IsNullOrEmpty(newName) && sortOptions != null &&
				sortOptions.SortInformationList != null)
			{
				foreach (SortInformation si in sortOptions.SortInformationList)
				{
					if (si.FieldInfo.FieldName == origName)
						si.FieldInfo.FieldName = newName;
				}
			}
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

			if (DataSources != null)
				DataSources.Clear();

			if (m_classes != null)
			    m_classes.Clear();

			if (m_queryGroups != null)
				m_queryGroups.Clear();

			if (m_fieldInfoList != null)
				m_fieldInfoList.Clear();

			DataSources = null;
			m_classes = null;
			m_queryGroups = null;
			m_fieldInfoList = null;
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject Load(string prjFileName, Form appWindow)
		{
			string msg = null;
			PaProject project = null;

			if (!File.Exists(prjFileName))
			{
				msg = LocalizationManager.LocalizeString("ProjectFileNonExistent",
					"Project file '{0}' does not exist.", "Message displayed when an " +
					"attempt is made to open a non existant project file. The parameter " +
					"is the project file name.", App.kLocalizationGroupInfoMsg,
					LocalizationCategory.ErrorOrWarningMessage, LocalizationPriority.Medium);

				msg = string.Format(msg, Utils.PrepFilePathForMsgBox(prjFileName));
			}

			if (msg == null)
				project = LoadProjectFileOnly(prjFileName, false, ref msg);

			if (msg == null)
			{
				FilterHelper.LoadFilters(project);

				if (project.m_loadedFilter != null)
					FilterHelper.SetCurrentFilter(project.m_loadedFilter.Name, false);
				
				project.LoadDataSources();
				if (appWindow != null)
				{
					appWindow.Activated += project.appWindow_Activated;
					project.m_appWindow = appWindow;
				}
			}
			else
			{
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				project = Utils.DeserializeData(projFileName, typeof(PaProject)) as PaProject;
				PhoneCache.CVPatternInfoList = project.m_CVPatternInfoList;
				project.m_fileName = projFileName;
				project.LoadFieldInfo();
				project.VerifyDataSourceMappings();
				RecordCacheEntry.InitializeDataSourceFields(project.FieldInfo);
			}
			catch (Exception e)
			{
				if (project == null)
				{
					errorMsg = string.Format(Properties.Resources.kstidErrorProjectInvalidFormat,
						Utils.PrepFilePathForMsgBox(projFileName));
				}
				else
				{
					errorMsg = string.Format(Properties.Resources.kstidErrorLoadingProject,
						Utils.PrepFilePathForMsgBox(projFileName), e.Message);
				}

				if (showErrors)
					Utils.MsgBox(errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
					m_appWindow.Activated += project.appWindow_Activated;
					project.m_appWindow = m_appWindow;
				}

				if (updateModfiedTimesInReloadedProject)
				{
					// Reloading a project resets all the data source's last modified
					// times to a default date a long, long time ago. Therefore, copy
					// the last modified times from this project's data sources.
					CopyLastModifiedTimes(project);
				}

				App.IPASymbolCache.TranscriptionChanges =
					TranscriptionChanges.Load(ProjectPathFilePrefix);

				App.IPASymbolCache.AmbiguousSequences = 
					AmbiguousSequences.Load(ProjectPathFilePrefix);
			}

			return project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void VerifyDataSourceMappings()
		{
			if (DataSources == null)
				return;

			foreach (PaDataSource ds in DataSources)
				ds.VerifyMappings(this);
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
				// Then go through the data sources in the target project.
				// When a data source in the target project matches one in this
				// project, then update the target's last modified time with the
				// one found in this project's data source.
				foreach (PaDataSource tgtDs in targetProj.DataSources)
				{
					if (ds.Matches(tgtDs, true))
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
				bool saveAfterLoadingFields;
				m_fieldInfoList = PaFieldInfoList.Load(this, out saveAfterLoadingFields);
				if (saveAfterLoadingFields)
					Save();

				App.FieldInfo = m_fieldInfoList;
				InitializeFontHelperFonts();
				DataCorpusVwSortOptions.SyncFieldInfo(m_fieldInfoList);
				SearchVwSortOptions.SyncFieldInfo(m_fieldInfoList);
				XYChartVwSortOptions.SyncFieldInfo(m_fieldInfoList);
			}
			catch (Exception e)
			{
				Utils.MsgBox(
					string.Format(Properties.Resources.kstidErrorLoadingProjectFieldInfo,
					Name, e.Message));
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
					const BindingFlags kFlags = BindingFlags.SetProperty |
						BindingFlags.Static | BindingFlags.Public;

					MemberInfo[] mi = typeof(FontHelper).GetMember(fieldInfo.FieldName + "Font");
					if (mi != null && mi.Length > 0)
					{
						typeof(FontHelper).InvokeMember(fieldInfo.FieldName + "Font",
								kFlags, null, typeof(FontHelper), new object[] { fieldInfo.Font });
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
		private void appWindow_Activated(object sender, EventArgs e)
		{
			if (App.SettingsHandler.GetBoolSettingsValue(App.kAppSettingsName,
				"reloadprojectsonactivate", true))
			{
				CheckForModifiedDataSources();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Go through the data source files and determine if any have changed since the
		/// application was deactivated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CheckForModifiedDataSources()
		{
			if (m_reloadingProjectInProcess)
				return;

			// We don't want to bother with updating just after a message box has been shown.
			// Especially if the chain of events that triggered displaying the message box was
			// started in this method. In that case, we'd get into an infinite loop of
			// displaying the message box.
			if (Utils.MessageBoxJustShown)
			{
				Utils.MessageBoxJustShown = false;
				return;
			}

			foreach (PaDataSource source in DataSources)
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
					// Post a message to force the project to be reloaded. Don't reload
					// it directly because there is an issue when project's are reloaded
					// (which causes a DoEvents to get called along the way) while still
					// in window activate events. See PA-440.
					App.MsgMediator.PostMessage("ReloadProject", null);
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
			App.MsgMediator.SendMessage("DataSourcesModified", ProjectFileName);
			m_reloadingProjectInProcess = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadDataSources()
		{
			App.IPASymbolCache.TranscriptionChanges =
				TranscriptionChanges.Load(ProjectPathFilePrefix);
			
			App.IPASymbolCache.AmbiguousSequences =
				AmbiguousSequences.Load(ProjectPathFilePrefix);
			
			PhoneCache.FeatureOverrides = FeatureOverrides.Load(ProjectPathFilePrefix);
			App.MsgMediator.SendMessage("BeforeLoadingDataSources", this);
			DataSourceReader reader = new DataSourceReader(this);
			reader.Read();
			EnsureSortOptionsValid();
			App.MsgMediator.SendMessage("AfterLoadingDataSources", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the project to it's specified project file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			m_fieldInfoList.Save(this);

			if (m_classes != null && m_classes.Count > 0)
				m_classes.Save(this);

			if (m_queryGroups != null && m_queryGroups.Count > 0)
				m_queryGroups.Save(this);

			if (m_fileName != null)
				Utils.SerializeData(m_fileName, this);

			if (m_newProject)
			{
				// Copy the default XY Chart definitions to the project's XY Chart def. file.
				try
				{
					string srcPath = Path.Combine(App.ConfigFolder, "DefaultXYCharts.xml");
					string destPath = ProjectPathFilePrefix + "XYCharts.xml";
					File.Copy(srcPath, destPath);
				}
				catch { }

				m_newProject = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure that if any of the sort options have their save manual flag set, that
		/// the project is saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureSortOptionsSaved()
		{
			if ((m_dataCorpusVwSortOptions != null && m_dataCorpusVwSortOptions.SaveManuallySetSortOptions) ||
				(m_searchVwSortOptions != null && m_searchVwSortOptions.SaveManuallySetSortOptions) ||
				(m_xyChartVwSortOptions != null && m_xyChartVwSortOptions.SaveManuallySetSortOptions))
			{
				Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the sort option's sort information list to make sure each field in
		/// there is a valid field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureSortOptionsValid()
		{
			EnsureSingleSortOptionValid(m_dataCorpusVwSortOptions);
			EnsureSingleSortOptionValid(m_searchVwSortOptions);
			EnsureSingleSortOptionValid(m_xyChartVwSortOptions);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the specified sort information list to make sure each field in there
		/// is a valid field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureSingleSortOptionValid(SortOptions sortOptions)
		{
			if (sortOptions != null && sortOptions.SortInformationList != null)
			{
				SortInformationList list = sortOptions.SortInformationList;

				for (int i = list.Count - 1; i >= 0; i--)
				{
					PaFieldInfo fieldInfo = m_fieldInfoList[list[i].FieldInfo.FieldName];
					if (fieldInfo == null)
						list.RemoveAt(i);
				}
			}
		}

		#endregion

		#region Loading/Saving Caches
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public void SaveIPACharCache()
		//{
		//    PaApp.IPASymbolCache.Save(ProjectPathFilePrefix);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public void SaveAFeatureCache()
		//{
		//    PaApp.AFeatureCache.Save(ProjectPathFilePrefix);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public void SaveBFeatureCache()
		//{
		//    PaApp.BFeatureCache.Save(ProjectPathFilePrefix);
		//}

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
			get	{return Path.GetDirectoryName(m_fileName);}
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
				return Path.Combine(string.IsNullOrEmpty(m_fileName) ? string.Empty :
					Path.GetDirectoryName(m_fileName), Name) + ".";
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is the full path to the .pap file.
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
		/// This is the full path to the project's phone inventory file recreated each time
		/// the project's data sources are read or the phone cache is updated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ProjectInventoryFileName
		{
			get { return ProjectPathFilePrefix + "PhoneticInventory.xml"; }
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
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageName { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageCode { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SpeakerName { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Comments { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string AlternateAudioFileFolder { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets an Options value indicating whether or not class names are shown in
		/// search patterns and nested class definitions. If this value is false, then class
		/// members are shown instead.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]	// Ignore for now. May never use this. But we'll see.
		public bool ShowClassNamesInSearchPatterns
		{
			get { return m_showClassNamesInSearchPatterns; }
			set {/*m_showClassNamesInSearchPatterns = value;*/}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to display the diamond
		/// pattern when the find phones search pattern text box is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]	// Ignore for now. May never use this. But we'll see.
		public bool ShowDiamondsInEmptySearchPattern
		{
			get	{ return m_showDiamondsInEmptySearchPattern;}
			set { /*m_showDiamondsInEmptySearchPattern = value;*/ }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to ignore in phonetic searches
		/// (i.e. on the Search view and XY Charts views) undefined phonetic characters found
		/// in data sources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IgnoreUndefinedCharsInSearches { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<PaDataSource> DataSources { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to display the undefined phonetic
		/// characters dialog whenever the data sources for the project are read. If no
		/// undefined phonetic characters are found in any of the project's data sources, then,
		/// of course, the dialog won't be displayed regardless of the value of this property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool ShowUndefinedCharsDlg { get; set; }

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
					m_queryGroups = SearchQueryGroupList.Load(this);

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
					m_classes = SearchClassList.Load(this);

				return m_classes;
			}
			set { m_classes = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the default sort options for the data corpus view word list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions DataCorpusVwSortOptions
		{
			get
			{
				if (m_dataCorpusVwSortOptions == null)
				{
					m_dataCorpusVwSortOptions = new SortOptions(true);
					m_dataCorpusVwSortOptions.AdvancedEnabled = false;
				}

				return m_dataCorpusVwSortOptions;
			}
			set
			{
				m_dataCorpusVwSortOptions = value;
				if (value != null)
					value.AdvancedEnabled = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the default sort options for search view word lists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions SearchVwSortOptions
		{
			get
			{
				if (m_searchVwSortOptions == null)
				{
					m_searchVwSortOptions = new SortOptions(true);
					m_searchVwSortOptions.AdvancedEnabled = true;
				}

				return m_searchVwSortOptions;
			}
			
			set 
			{
				m_searchVwSortOptions = value;
				if (value != null)
					value.AdvancedEnabled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the default sort options applied to word lists in XY Chart view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptions XYChartVwSortOptions
		{
			get
			{
				if (m_xyChartVwSortOptions == null)
				{
					m_xyChartVwSortOptions = new SortOptions(true);
					m_xyChartVwSortOptions.AdvancedEnabled = true;
				}
				
				return m_xyChartVwSortOptions;
			}
			set
			{
				m_xyChartVwSortOptions = value;
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
			set	{m_cieOptions = (value ?? new CIEOptions());}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("currentFilter")]
		public Filter CurrentFilter
		{
			get { return FilterHelper.CurrentFilter; }
			set { m_loadedFilter = value; }
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
			get	{return (m_gridLayoutInfo ?? new GridLayoutInfo(this));}
			set
			{
				m_gridLayoutInfo = (value ?? new GridLayoutInfo(this));
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
		public DataGridViewCellBorderStyle GridLines = DataGridViewCellBorderStyle.Single;
		public int ColHeaderHeight = -1;
		public bool SaveReorderedCols = true;
		public bool SaveAdjustedColHeaderHeight = true;
		public bool SaveAdjustedColWidths = true;
		public bool AutoAdjustPhoneticCol = true;
		public int AutoAjustedMaxWidth = 200;
		internal PaProject m_owningProject;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GridLayoutInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GridLayoutInfo(PaProject owningProj)
		{
			m_owningProject = owningProj;
		}

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
