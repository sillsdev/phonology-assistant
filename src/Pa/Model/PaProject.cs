using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml.Linq;
using Palaso.IO;
using SIL.Pa.DataSource;
using SIL.Pa.Filters;
using SIL.Pa.Model.Migration;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SIL.Pa.UI.Views;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class PaProject : IDisposable
	{
		private const string kCurrVersion = "3.3";

		private Form m_appWindow;
		private bool m_newProject;
		private bool m_reloadingProjectInProcess;
		private string m_fileName;
		private GridLayoutInfo m_gridLayoutInfo;
		private SortOptions m_dataCorpusVwSortOptions;
		private SortOptions m_searchVwSortOptions;
		private SortOptions m_distChartVwSortOptions;
		private Filter m_loadedFilter;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject()
		{
			Name = App.LocalizeString("DefaultNewProjectName", "New Project", App.kLocalizationGroupMisc);
			ShowUndefinedCharsDlg = true;
			IgnoreUndefinedCharsInSearches = true;
			LastNewlyMappedFields = new List<string>(0);
			Version = kCurrVersion;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is really only for creating new projects. Therefore, the
		/// newProject argument should always be true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject(bool newProject) : this()
		{
			if (newProject)
			{
				Fields = PaField.GetDefaultFields();
				DataSources = new List<PaDataSource>();
				CVPatternInfoList = new List<CVPatternInfo>();
				SearchClasses = SearchClassList.LoadDefaults(this);
				SearchQueryGroups = SearchQueryGroupList.LoadDefaults(this);
				FilterHelper = new FilterHelper(this);
				CIEOptions = new CIEOptions();
				LoadAmbiguousSequences();
				LoadTranscriptionChanges();
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
			//if (DataSources == null)
			//    return;

			//// Go through the list of data sources in the project and clean up the
			//// mappings for projects of type SFM and Toolbox.
			//foreach (PaDataSource source in DataSources)
			//{
			//    if (source.Type != DataSourceType.SFM &&
			//        source.Type != DataSourceType.Toolbox)
			//    {
			//        continue;
			//    }

			//    // Go through the mappings in the data source and make sure the field
			//    // for each mapping is still in the list of fields in the project.
			//    for (int i = source.SFMappings.Count - 1; i >= 0; i--)
			//    {
			//        SFMarkerMapping mapping = source.SFMappings[i];

			//        if (mapping.FieldName != PaDataSource.kRecordMarker)
			//        {
			//            PaFieldInfo fieldInfo = m_fieldInfoList[mapping.FieldName];

			//            // If the mapped field no longer exists, then remove it from the data
			//            // source's list of mappings. Otherwise, make sure that the mapping
			//            // for the field doesn't think it is an interlinear field if it no
			//            // longer is.
			//            if (fieldInfo == null)
			//                source.SFMappings.RemoveAt(i);
			//            else if (!fieldInfo.CanBeInterlinear)
			//                mapping.IsInterlinear = false;
			//        }
			//    }
			//}
		}

		#region Method to migrate previous versions of .pap files to current.
		/// ------------------------------------------------------------------------------------
		public static bool MigrateToLatestVersion(string filename)
		{
			var xml = XElement.Load(filename);
			var ver = xml.Attribute("version");
			if (ver != null && ver.Value == kCurrVersion)
				return true;

			return Migration0330.Migrate(filename, GetProjectPathFilePrefix);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure that SFM and Toolbox data sources are informed that a field has
		/// changed names. This is so the SF mappings in the data source can be updated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ProcessRenamedField(string origName, string newName)
		{
			foreach (var source in DataSources)
				source.RenameField(origName, newName);

			ProcessRenamedFieldInSortInfo(origName, newName, m_dataCorpusVwSortOptions);
			ProcessRenamedFieldInSortInfo(origName, newName, m_searchVwSortOptions);
			ProcessRenamedFieldInSortInfo(origName, newName, m_distChartVwSortOptions);
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
				sortOptions.SortFields != null)
			{
				foreach (var si in sortOptions.SortFields.Where(si => si.Field.Name == origName))
					si.Field.Name = newName;
			}
		}

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (m_appWindow != null)
			{
				m_appWindow.Activated -= HandleApplicationWindowActivated;
				m_appWindow = null;
			}

			if (DataSources != null)
				DataSources.Clear();

			DataSources = null;
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
		public static PaProject Load(string prjFilePath, Form appWindow)
		{
			string msg = null;
			PaProject project = null;

			if (!File.Exists(prjFilePath))
			{
				msg = App.LocalizeString("ProjectFileMissingMsg", "Project file '{0}' does not exist.",
					"Message displayed when an attempt is made to open a non existant project file. The parameter is the project file name.",
					App.kLocalizationGroupInfoMsg);

				Utils.MsgBox(string.Format(msg, Utils.PrepFilePathForMsgBox(prjFilePath)));
				return null;
			}

			if (!MigrateToLatestVersion(prjFilePath))
				return null;

			project = LoadProjectFileOnly(prjFilePath, false, ref msg);

			if (msg != null)
			{
				Utils.MsgBox(msg);
				return null;
			}

			if (project.m_loadedFilter != null)
				project.FilterHelper.SetCurrentFilter(project.m_loadedFilter.Name, false);
			else
				project.FilterHelper.TurnOffCurrentFilter(false);

			project.LoadDataSources();

			if (appWindow != null)
			{
				appWindow.Activated -= project.HandleApplicationWindowActivated;
				appWindow.Activated += project.HandleApplicationWindowActivated;
				project.m_appWindow = appWindow;
			}

			return project;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads only the project file for the specified file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject LoadProjectFileOnly(string projFileName, bool showErrors,
			ref string msg)
		{
			PaProject project = null;

			try
			{
				// Load the cache of IPA symbols, articulatory and binary features.
				project = XmlSerializationHelper.DeserializeFromFile<PaProject>(projFileName);
				project.PostDeserializeInitialization(projFileName);
			}
			catch (Exception e)
			{
				if (project == null)
				{
					msg = string.Format(App.LocalizeString("InvalidProjectFileFormatMsg",
						"Project File '{0}' has an Invalid Format.", App.kLocalizationGroupInfoMsg),
						Utils.PrepFilePathForMsgBox(projFileName));	
				}
				else
				{
					msg = string.Format(App.LocalizeString("ErrorLoadingProjectMsg",
						"The followng error occurred loading project '{0}'.\n\n{1}",
						App.kLocalizationGroupInfoMsg),
						Utils.PrepFilePathForMsgBox(projFileName), e.Message);	
				}

				if (showErrors)
					Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				project = null;
			}

			return project;
		}

		/// ------------------------------------------------------------------------------------
		private void PostDeserializeInitialization(string projFileName)
		{
			if (string.IsNullOrEmpty(Version))
				Version = kCurrVersion;

			m_fileName = projFileName;
			Fields = PaField.GetProjectFields(this);
			FilterHelper = new FilterHelper(this);
			SearchClasses = SearchClassList.Load(this);
			SearchQueryGroups = SearchQueryGroupList.Load(this);
			CVPatternInfoList = (CVPatternInfoList ?? new List<CVPatternInfo>());
			LoadAmbiguousSequences();
			LoadTranscriptionChanges();
			PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);
			DataCorpusVwSortOptions.PostDeserializeInitialization(this);
			SearchVwSortOptions.PostDeserializeInitialization(this);
			DistributionChartVwSortOptions.PostDeserializeInitialization(this);
			FixupFieldsAndMappings();
			
			//project.VerifyDataSourceMappings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will go through all the field mappings of all the data source's, making sure
		/// each field mapping points to one of the fields in the project. If, along the way,
		/// a mapping is found with a field name that doesn't belong to one found in the
		/// project's fields, then an associated field is added to the project. It is assumed
		/// that fields added in this way are ones manually added in the SFM/Toolbox data
		/// source mappings dialog, since there's no where else to do so.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void FixupFieldsAndMappings()
		{
			var fields = Fields.ToList();

			foreach (var ds in DataSources.Where(d => d.FieldMappings != null))
			{
				foreach (var mapping in ds.FieldMappings)
				{
					var field = Fields.SingleOrDefault(f => f.Name == mapping.PaFieldName);

					// If field is null, it means the user has entered their own
					// field name in one of the SFM/Toolbox data source mappings.
					if (field == null)
						fields.Add(mapping.Field);
					else
						mapping.Field = field;
					
					mapping.PaFieldName = null;
				}
			}

			Fields = fields.OrderBy(f => f.Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reloads the project without reloading the data sources and returns a
		/// project object that represents the reloaded project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject ReLoadProjectFileOnly()
		{
			string errorMsg = null;
			var project = LoadProjectFileOnly(m_fileName, true, ref errorMsg);

			if (project == null)
				return null;

			if (m_appWindow != null)
			{
				m_appWindow.Activated -= HandleApplicationWindowActivated;
				m_appWindow.Activated += project.HandleApplicationWindowActivated;
				project.m_appWindow = m_appWindow;
			}

			// Reloading a project resets all the data source's last modified
			// times to a default date a long, long time ago. Therefore, copy
			// the last modified times from this project's data sources.
			// Go through each data source in this project.
			foreach (var srcDs in DataSources)
			{
				foreach (var tgtDs in project.DataSources.Where(ds => srcDs.Matches(ds, true)))
					tgtDs.LastModification = srcDs.LastModification;
			}

			return project;
		}

		///// ------------------------------------------------------------------------------------
		//private void VerifyDataSourceMappings()
		//{
		//    if (DataSources == null)
		//        return;

		//    foreach (var ds in DataSources)
		//        ds.VerifyMappings(this);
		//}

		/// ------------------------------------------------------------------------------------
		private void HandleApplicationWindowActivated(object sender, EventArgs e)
		{
			if (Settings.Default.ReloadProjectsWhenAppBecomesActivate)
				CheckForModifiedDataSources();
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

			// We don't want to bother updating just after a message box has been shown.
			// Especially if the chain of events that triggered displaying the message box was
			// started in this method. In that case, we'd get into an infinite loop of
			// displaying the message box.
			if (Utils.MessageBoxJustShown)
			{
				Utils.MessageBoxJustShown = false;
				return;
			}

			if (DataSources.Any(ds => !ds.SkipLoadingBecauseOfProblem && ds.UpdateLastModifiedTime()))
				App.MsgMediator.PostMessage("ReloadProject", null);
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
			App.MsgMediator.SendMessage("DataSourcesModified", this);
			m_reloadingProjectInProcess = false;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadDataSources()
		{
			LoadAmbiguousSequences();
			LoadTranscriptionChanges();
			FeatureOverrides = FeatureOverrides.Load(this);
			PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);

			App.MsgMediator.SendMessage("BeforeLoadingDataSources", this);
			var reader = new DataSourceReader(this);
			RecordCache = reader.Read();

			App.InitializeProgressBar(Properties.Resources.kstidParsingDataMsg, RecordCache.Count);
			RecordCache.BuildWordCache(App.ProgressBar);
			PhoneticParser.LogUndefinedCharactersWhenParsing = false;
			App.IncProgressBar();
			TempRecordCache.Save();
			App.UninitializeProgressBar();

			EnsureSortOptionsValid();
			App.MsgMediator.SendMessage("AfterLoadingDataSources", this);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadTranscriptionChanges()
		{
			TranscriptionChanges = TranscriptionChanges.Load(ProjectPathFilePrefix);
		}

		/// ------------------------------------------------------------------------------------
		public void SaveAndLoadTranscriptionChanges(TranscriptionChanges transChanges)
		{
			transChanges.Save(ProjectPathFilePrefix);
			LoadTranscriptionChanges();
			PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);
		}

		/// ------------------------------------------------------------------------------------
		public void LoadAmbiguousSequences()
		{
			AmbiguousSequences = AmbiguousSequences.Load(ProjectPathFilePrefix);
		}

		/// ------------------------------------------------------------------------------------
		public void SaveAndLoadAmbiguousSequences(AmbiguousSequences ambigSeqList)
		{
			ambigSeqList.Save(ProjectPathFilePrefix);
			LoadAmbiguousSequences();
			PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);
		}

		/// ------------------------------------------------------------------------------------
		public void SaveCIEOptions(CIEOptions newOptions)
		{
			CIEOptions = newOptions;
			Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the project to it's specified project file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			PaField.SaveProjectFields(this);
			SearchClasses.Save();
			SearchQueryGroups.Save();
			FilterHelper.Save();
			AmbiguousSequences.Save(ProjectPathFilePrefix);
			TranscriptionChanges.Save(ProjectPathFilePrefix);

			if (m_fileName != null)
				XmlSerializationHelper.SerializeToFile(m_fileName, this);

			if (!m_newProject)
				return;
			
			// Copy the default dist. Chart definitions to the project's dist. Chart def. file.
			try
			{
				var srcPath = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultDistributionCharts.xml");
				var destPath = ProjectPathFilePrefix + DistributionChartVw.kSavedChartsFile;
				File.Copy(srcPath, destPath);
			}
			catch { }

			m_newProject = false;
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
				(m_distChartVwSortOptions != null && m_distChartVwSortOptions.SaveManuallySetSortOptions))
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
			EnsureSingleSortOptionValid(m_distChartVwSortOptions);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the specified sort information list to make sure each field in there
		/// is a valid field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureSingleSortOptionValid(SortOptions sortOptions)
		{
			if (sortOptions == null || sortOptions.SortFields == null)
				return;
			
			var list = sortOptions.SortFields;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!Fields.Any(f => f.Name == list[i].Field.Name))
					list.RemoveAt(i);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public PaField GetPhoneticField()
		{
			return (Fields == null ? null : Fields.SingleOrDefault(f => f.Type == FieldType.Phonetic));
		}

		/// ------------------------------------------------------------------------------------
		public PaField GetAudioFileField()
		{
			return (Fields == null ? null : Fields.SingleOrDefault(f => f.Type == FieldType.AudioFilePath));
		}

		/// ------------------------------------------------------------------------------------
		public PaField GetAudioOffsetField()
		{
			return (Fields == null ? null : Fields.SingleOrDefault(f => f.Type == FieldType.AudioOffset));
		}

		/// ------------------------------------------------------------------------------------
		public PaField GetAudioLengthField()
		{
			return (Fields == null ? null : Fields.SingleOrDefault(f => f.Type == FieldType.AudioLength));
		}

		/// ------------------------------------------------------------------------------------
		public PaField GetDataSourceField()
		{
			return GetFieldForName(PaField.kDataSourceFieldName);
		}

		/// ------------------------------------------------------------------------------------
		public PaField GetDataSourcePathField()
		{
			return GetFieldForName(PaField.kDataSourcePathFieldName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field corresponding to the specified name. name is not the
		/// DisplayName.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaField GetFieldForName(string name)
		{
			return Fields.SingleOrDefault(f => f.Name == name);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the field corresponding to the specified display name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaField GetFieldForDisplayName(string displayName)
		{
			return Fields.SingleOrDefault(f => f.DisplayName == displayName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a subset of the project's fields based on whether or not a field is mapped.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<PaField> GetMappedFields()
		{
			var list = DataSources.SelectMany(ds => ds.FieldMappings).Select(m => m.Field).ToList();
			list.AddRange(PaField.GetCalculatedFieldsFromList(Fields));
			return list.Distinct(new FieldNameComparer());
		}

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
		public string Folder
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
			get { return GetProjectPathFilePrefix(m_fileName, Name); }
		}

		/// ------------------------------------------------------------------------------------
		private static string GetProjectPathFilePrefix(string fileName, string prjName)
		{
			return Path.Combine(string.IsNullOrEmpty(fileName) ? string.Empty :
				Path.GetDirectoryName(fileName), prjName) + ".";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is the full path to the .pap file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FileName
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
		/// Gets the full path to the project's style sheet file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CssFileName
		{
			get { return Path.Combine(Folder, Name.Replace(' ', '_') + ".css"); }
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
		[XmlElement("version")]
		public string Version { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("name")]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("languageName")]
		public string LanguageName { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("languageCode")]
		public string LanguageCode { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Researcher { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Transcriber { get; set; }

		/// ------------------------------------------------------------------------------------
		public string SpeakerName { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Comments { get; set; }

		/// ------------------------------------------------------------------------------------
		public string AlternateAudioFileFolder { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to ignore in phonetic searches
		/// (i.e. on the Search view and Distribution Charts views) undefined phonetic
		/// characters found in data sources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IgnoreUndefinedCharsInSearches { get; set; }

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
		public bool ShowUndefinedCharsDlg { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<PaField> Fields { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SearchQueryGroupList SearchQueryGroups { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SearchClassList SearchClasses { get; private set; }

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
					m_dataCorpusVwSortOptions = new SortOptions(true, this);
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
					m_searchVwSortOptions = new SortOptions(true, this);
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
		public SortOptions DistributionChartVwSortOptions
		{
			get
			{
				if (m_distChartVwSortOptions == null)
				{
					m_distChartVwSortOptions = new SortOptions(true, this);
					m_distChartVwSortOptions.AdvancedEnabled = true;
				}
				
				return m_distChartVwSortOptions;
			}
			set
			{
				m_distChartVwSortOptions = value;
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
		public CIEOptions CIEOptions { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public RecordCache RecordCache { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public WordCache WordCache
		{
			get { return RecordCache.WordCache; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PhoneticParser PhoneticParser { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of phones in the current project, without respect to current filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PhoneCache UnfilteredPhoneCache
		{
			get { return RecordCache.UnfilteredPhoneCache; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of phones in the current project, with respect to current filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PhoneCache PhoneCache
		{
			get { return RecordCache.PhoneCache; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public TranscriptionChanges TranscriptionChanges { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of ambiguous sequences used while adding phones to the cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public AmbiguousSequences AmbiguousSequences { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the CVPatternInfoList for the phone cache. This list should be set to the
		/// list owned by a PA project when the project is opened.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("CVPatternInfoList"), XmlArrayItem("CVPatternInfo")]
		public List<CVPatternInfo> CVPatternInfoList { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of phones whose features should be overridden.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FeatureOverrides FeatureOverrides { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FilterHelper FilterHelper { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("currentFilter")]
		public Filter CurrentFilter
		{
			get { return FilterHelper.CurrentFilter; }
			set { m_loadedFilter = value; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public IEnumerable<string> LastNewlyMappedFields { get; set; }

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
		public GridLayoutInfo()
		{
		}

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
			foreach (var col in grid.Columns.Cast<DataGridViewColumn>())
			{
				var field = m_owningProject.GetFieldForName(col.Name);
				if (field == null)
					continue;

				if (field.DisplayIndexInGrid < 0)
					col.Visible = false;
				else
				{
					col.Visible = field.VisibleInGrid;
					col.DisplayIndex =
						(field.DisplayIndexInGrid < grid.Columns.Count ?
						field.DisplayIndexInGrid : grid.Columns.Count - 1);
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
			var displayIndexes = new SortedList<int, PaField>();

			// Save the specified grid's column properties.
			foreach (var col in grid.Columns.Cast<DataGridViewColumn>())
			{
				var field = m_owningProject.GetFieldForName(col.Name);
				if (field != null)
				{
					if (SaveAdjustedColWidths)
						field.WidthInGrid = col.Width;

					if (SaveReorderedCols)
						displayIndexes[col.DisplayIndex] = field;
				}
			}

			if (displayIndexes.Count == 0)
				return;

			// The display index order saved with the fields should begin with zero, but
			// since the grid may have some SilHerarchicalColumns showing, the first field's
			// display index may be greater than 1. Therefore, we adjust for that by setting
			// the display indexes in sequence beginning from zero.
			int i = 0;
			foreach (var field in displayIndexes.Values)
				field.DisplayIndexInGrid = i++;
		}
	}

	#endregion
}
