using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml.Linq;
using Localization;
using Palaso.IO;
using SIL.Pa.DataSource;
using SIL.Pa.Filters;
using SIL.Pa.Model.Migration;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class PaProject : IDisposable
	{
		public const string kCurrVersion = "3.3.3";

		private Form _appWindow;
		private bool _newProject;
		private bool _reloadingProjectInProcess;
		private string _fileName;
		private SortOptions _dataCorpusVwSortOptions;
		private SortOptions _searchVwSortOptions;
		private SortOptions _distChartVwSortOptions;
		private string _currentFilterName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject()
		{
			Name = LocalizationManager.GetString("ProjectMessages.Creating.DefaultNewProjectName", "New Project");
			ShowUndefinedCharsDlg = true;
			IgnoreUndefinedCharsInSearches = true;
			IgnoredSymbolsInCVCharts = new List<string>();
			LastNewlyMappedFields = new List<string>(0);
			DataSources = new List<PaDataSource>(0);
			Version = kCurrVersion;
			DistinctiveFeatureSet = BFeatureCache.DefaultFeatureSetName;
			App.BFeatureCache = BFeatureCache = new BFeatureCache();
			CVPatternInfoList = new List<CVPatternInfo>();
			GridLayoutInfo = new GridLayoutInfo(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is really only for creating new projects. Therefore, the
		/// newProject argument should always be true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaProject(bool newProject) : this()
		{
			if (!newProject)
				return;

			DistinctiveFeatureSet = BFeatureCache.DefaultFeatureSetName;
			Fields = PaField.GetProjectFields(this);
			DataSources = new List<PaDataSource>();
			SearchClasses = SearchClassList.LoadDefaults(this);
			SearchQueryGroups = SearchQueryGroupList.LoadDefaults(this);
			FilterHelper = new FilterHelper(this);
			CIEOptions = new CIEOptions();
			LoadAmbiguousSequences();
			LoadTranscriptionChanges();
			_newProject = true;
			RecordCache = new RecordCache(this);
			PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Makes sure that SFM and Toolbox data sources are informed that a field has
		///// changed names. This is so the SF mappings in the data source can be updated.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public void ProcessRenamedField(string origName, string newName)
		//{
		//    foreach (var source in DataSources)
		//        source.RenameField(origName, newName);

		//    ProcessRenamedFieldInSortInfo(origName, newName, _dataCorpusVwSortOptions);
		//    ProcessRenamedFieldInSortInfo(origName, newName, _searchVwSortOptions);
		//    ProcessRenamedFieldInSortInfo(origName, newName, _distChartVwSortOptions);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Goes through the specified sort information list to make sure the specified field
		///// name gets renamed therein.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private static void ProcessRenamedFieldInSortInfo(string origName, string newName,
		//    SortOptions sortOptions)
		//{
		//    if (!string.IsNullOrEmpty(newName) && sortOptions != null &&
		//        sortOptions.SortFields != null)
		//    {
		//        foreach (var si in sortOptions.SortFields.Where(si => si.Field.Name == origName))
		//            si.Field.Name = newName;
		//    }
		//}

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_appWindow != null)
			{
				_appWindow.Activated -= HandleApplicationWindowActivated;
				_appWindow = null;
			}

			if (DataSources != null)
				DataSources.Clear();

			DataSources = null;
		}

		#endregion

		#region Method to migrate previous versions of .pap files to current.
		/// ------------------------------------------------------------------------------------
		public static bool MigrateToLatestVersion(string filename)
		{
			var xml = XElement.Load(filename);
			var prevVersion = (string)xml.Attribute("version") ?? "3.0.1";
			if (prevVersion == kCurrVersion)
				return true;

			var projectName = Path.GetFileNameWithoutExtension(filename);
			int i = projectName.IndexOf('.');
			if (i >= 0)
				projectName = projectName.Substring(0, i);

			var backupFolder = MigrationBase.BackupProject(filename, projectName, prevVersion);
			if (backupFolder == null)
				return false;

			Exception error = null;

			if (prevVersion == "3.0.1")
				error = Migration0330.Migrate(filename, GetProjectPathFilePrefix);

			if (error == null && prevVersion == "3.3.0" || prevVersion == "3.0.1")
				error = Migration0333.Migrate(filename, GetProjectPathFilePrefix);

			if (error == null)
			{
				var msg = LocalizationManager.GetString("ProjectMessages.Migrating.MigrationSuccessfulMsg",
					"The '{0}' project has succssfully been upgraded to work with this version of Phonology Assistant. A backup of your old project has been made in:\n\n{1}");

				Utils.MsgBox(string.Format(msg, projectName, backupFolder));
				return true;
			}

			var errMsg = LocalizationManager.GetString("ProjectMessages.Migrating.MigrationFailureMsg",
				"There was an error upgrading the '{0}' project to work with this version of Phonology Assistant. " +
				"Until the problem is resolved, this project cannot be opened using this version of Phonology Assistant.");

			App.NotifyUserOfProblem(error, errMsg, projectName);
			return false;
		}

		/// ------------------------------------------------------------------------------------
		private static void PerformPostProjectLoadMigration(PaProject project)
		{
			Migration0333.PostProjectLoadMigration(project);
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
			Exception e;
			PaProject project = null;

			if (!File.Exists(prjFilePath))
			{
				msg = LocalizationManager.GetString("ProjectMessages.Loading.ProjectFileMissingMsg", "The project file '{0}' is missing.",
					"Message displayed when an attempt is made to open a non existant project file. The parameter is the project file name.");

				App.NotifyUserOfProblem(msg, prjFilePath);
				return null;
			}

			if (!MigrateToLatestVersion(prjFilePath))
				return null;

			project = LoadProjectFileOnly(prjFilePath, false, ref msg, out e);

			if (msg != null || e != null)
			{
				App.NotifyUserOfProblem(e, msg);
				return null;
			}

			if (project._currentFilterName != null)
				project.FilterHelper.SetCurrentFilter(project._currentFilterName, false);
			else
				project.FilterHelper.TurnOffCurrentFilter(false);

			project.LoadDataSources();

			PerformPostProjectLoadMigration(project);

			if (appWindow != null)
			{
				appWindow.Activated -= project.HandleApplicationWindowActivated;
				appWindow.Activated += project.HandleApplicationWindowActivated;
				project._appWindow = appWindow;
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
			Exception e;
			return LoadProjectFileOnly(projFileName, showErrors, ref msg, out e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads only the project file for the specified file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaProject LoadProjectFileOnly(string projFileName, bool showErrors,
			ref string msg, out Exception error)
		{
			PaProject project = null;
			error = null;

			try
			{
				// Load the cache of IPA symbols, articulatory and binary features.
				project = XmlSerializationHelper.DeserializeFromFile<PaProject>(projFileName, out error);
				if (error == null)
					project.PostDeserializeInitialization(projFileName);
			}
			catch (Exception e)
			{
				error = e;
			}

			if (error != null)
			{
				msg = (project == null ?
					LocalizationManager.GetString("ProjectMessages.Loading.InvalidProjectFileErrorMsg", "The project file '{0}' has an invalid format.") :
					LocalizationManager.GetString("ProjectMessages.Loading.LoadingProjectErrorMsg", "There was an error loading the project file '{0}'"));

				msg = string.Format(msg, projFileName);

				if (showErrors)
					App.NotifyUserOfProblem(error, msg);

				project = null;
			}

			return project;
		}

		/// ------------------------------------------------------------------------------------
		private void PostDeserializeInitialization(string projFileName)
		{
			if (string.IsNullOrEmpty(Version))
				Version = kCurrVersion;

			_fileName = projFileName;
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
			SynchronizeProjectFieldMappingsWithDataSourceFieldMappings();
			LoadDistinctiveFeatureSet();
			LoadFeatureOverrides();
			GridLayoutInfo = GridLayoutInfo.Load(this);

			if (CIEOptions == null)
				CIEOptions = new CIEOptions();

			foreach (var ds in DataSources)
				ds.PostDeserializeInitialization(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will go through all the field mappings of all the data source's, making sure
		/// each field mapping points to one of the fields in the project. If, along the way,
		/// a mapping is found with a field name that doesn't belong to one found in the
		/// project's fields, then an associated field is added to the project. It is assumed
		/// that fields added in this way are ones manually added in the SFM/Toolbox data
		/// source mappings dialog, since there's no where else to do so.
		/// 
		/// The final process is go through the sort options and make sure each field found
		/// therein still exists in the project's field collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SynchronizeProjectFieldMappingsWithDataSourceFieldMappings()
		{
			var fields = Fields.ToList();

			foreach (var ds in DataSources.Where(d => d.FieldMappings != null))
			{
				foreach (var mapping in ds.FieldMappings)
				{
					var field = fields.SingleOrDefault(f => f.Name == mapping.PaFieldName);

					// If field is null, it means the user has entered their own
					// field name in one of the SFM/Toolbox data source mappings.
					if (field != null)
						mapping.Field = field;
					else
					{
						if (mapping.Field == null)
							mapping.Field = new PaField(mapping.PaFieldName);

						fields.Add(mapping.Field);
					}

					mapping.PaFieldName = null;
				}
			}

			// Now remove any fields that no longer have a mapping and are not in the default set (i.e. custom).
			var mappedFieldNames = DataSources.SelectMany(d => d.FieldMappings).Select(m => m.PaFieldName).ToList();
			var defaultFieldNames = PaField.GetDefaultFields().Select(f => f.Name).ToList();

			for (int i = fields.Count - 1; i >= 0; i--)
			{
				if (!mappedFieldNames.Contains(fields[i].Name) && !defaultFieldNames.Contains(fields[i].Name))
					fields.RemoveAt(i);
			}

			Fields = fields.OrderBy(f => f.Name);
			EnsureSortOptionsValid();
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
			var project = LoadProjectFileOnly(_fileName, true, ref errorMsg);

			if (project == null)
				return null;

			if (_appWindow != null)
			{
				_appWindow.Activated -= HandleApplicationWindowActivated;
				_appWindow.Activated += project.HandleApplicationWindowActivated;
				project._appWindow = _appWindow;
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
			if (_reloadingProjectInProcess)
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
			_reloadingProjectInProcess = true;
			LoadDataSources();
			App.MsgMediator.SendMessage("DataSourcesModified", this);
			_reloadingProjectInProcess = false;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadDataSources()
		{
			LoadAmbiguousSequences();
			LoadTranscriptionChanges();
			PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);

			App.MsgMediator.SendMessage("BeforeLoadingDataSources", this);
			var reader = new DataSourceReader(this);
			RecordCache = reader.Read();

			var msg = LocalizationManager.GetString("ProjectMessages.Loading.ParsingDataMsg", "Parsing Data...",
				"Progress message after data source is read and the data is being parsed.");

			App.InitializeProgressBar(msg, RecordCache.Count);
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
		public void UpdateAbiguousSequencesWithGeneratedOnes(IEnumerable<string> generatedSequences)
		{
			AmbiguousSequences list;
			if (AmbiguousSequences != null)
				list = new AmbiguousSequences(AmbiguousSequences.Where(s => !s.IsGenerated));
			else
				list = new AmbiguousSequences();

			foreach (var seq in generatedSequences)
			{
				var existingSeq = list.FirstOrDefault(s => s.Literal == seq);
				if (existingSeq == null)
					list.Add(new AmbiguousSeq(seq, true, true));
				else
					existingSeq.IsGenerated = true;
			}

			SaveAndLoadAmbiguousSequences(list);
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
		public void LoadDistinctiveFeatureSet()
		{
			var filePath = BFeatureCache.GetAvailableFeatureSetFiles()
                .FirstOrDefault(f => Path.GetFileName(f).StartsWith(DistinctiveFeatureSet, StringComparison.Ordinal));

			if (filePath != null)
			{
				var root = XElement.Load(filePath);
				BFeatureCache.LoadFromList(FeatureCacheBase.ReadFeaturesFromXElement(root, "distinctive"));
				return;
			}

			var msg = LocalizationManager.GetString("ProjectMessages.Loading.LoadingDistinctiveFeatureSetFileErrorMsg",
				"The file containing the '{0}' distinctive feature set is missing. The default set will be used instead.");
				
			App.NotifyUserOfProblem(msg, DistinctiveFeatureSet);
			BFeatureCache.LoadFromList(BFeatureCache.GetFeaturesFromDefaultSet());
		}

		/// ------------------------------------------------------------------------------------
		public void LoadFeatureOverrides()
		{
			FeatureOverrides = FeatureOverrides.Load(this);
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateFeatureOverrides(IEnumerable<PhoneInfo> phonesWithOverrides)
		{
			App.MsgMediator.SendMessage("BeforePhoneFeatureOverridesSaved",
				new object[] { this, phonesWithOverrides });

			FeatureOverrides.Save(phonesWithOverrides);
			
			App.MsgMediator.SendMessage("AfterPhoneFeatureOverridesSaved",
				new object[] { this, phonesWithOverrides });
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

			if (_fileName != null)
				XmlSerializationHelper.SerializeToFile(_fileName, this);

			if (!_newProject)
				return;
			
			// Copy the default dist. Chart definitions to the project's dist. Chart def. file.
			try
			{
				var srcPath = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultDistributionCharts.xml");
				var destPath = DistributionChart.GetFileForProject(ProjectPathFilePrefix);
				File.Copy(srcPath, destPath);
			}
			catch { }

			_newProject = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure that if any of the sort options have their save manual flag set, that
		/// the project is saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void EnsureSortOptionsSaved()
		{
			if ((_dataCorpusVwSortOptions != null && _dataCorpusVwSortOptions.SaveManuallySetSortOptions) ||
				(_searchVwSortOptions != null && _searchVwSortOptions.SaveManuallySetSortOptions) ||
				(_distChartVwSortOptions != null && _distChartVwSortOptions.SaveManuallySetSortOptions))
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
			EnsureSingleSortOptionValid(_dataCorpusVwSortOptions);
			EnsureSingleSortOptionValid(_searchVwSortOptions);
			EnsureSingleSortOptionValid(_distChartVwSortOptions);
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
				if (!Fields.Any(f => f.Name == list[i].PaFieldName))
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

		/// ------------------------------------------------------------------------------------
		public void AddAmbiguousSequence(string sequence)
		{
			if (!AmbiguousSequences.Any(s => s.Literal == sequence))
			{
				AmbiguousSequences.Add(sequence);
				PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void AddTranscriptionChange(TranscriptionChange transChange)
		{
			TranscriptionChanges.Add(transChange);
			PhoneticParser = new PhoneticParser(AmbiguousSequences, TranscriptionChanges);
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
			get	{return Path.GetDirectoryName(_fileName);}
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
			get { return GetProjectPathFilePrefix(_fileName, GetCleanNameForFileName()); }
		}

		/// ------------------------------------------------------------------------------------
		public string GetCleanNameForFileName()
		{
			return GetCleanNameForFileName(Name);
		}

		/// ------------------------------------------------------------------------------------
		public static string GetCleanNameForFileName(string name)
		{
			return Path.GetInvalidFileNameChars()
				.Aggregate(name, (curr, illegalChar) => curr.Replace(illegalChar, '_'));
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
			get { return _fileName; }
			set
			{
				// Only allow this when there hasn't already been a file name specified.
				// This should only be the case when creating new projects.
				if (_fileName == null)
					_fileName = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the project's style sheet file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CssFileName
		{
			get { return Path.Combine(Folder, GetCleanNameForFileName().Replace(' ', '_') + ".css"); }
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
				return (string.IsNullOrEmpty(_fileName) ?
					string.Empty : Path.GetFileNameWithoutExtension(_fileName));
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("version")]
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
		public string DistinctiveFeatureSet { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlArray("ignoredSymbolsInCVCharts"), XmlArrayItem("symbol")]
		public List<string> IgnoredSymbolsInCVCharts { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public BFeatureCache BFeatureCache { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("currentFilter")]
		public string CurrentFilterName
		{
			get { return (CurrentFilter != null ? CurrentFilter.Name : null); }
			set { _currentFilterName = value; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Filter CurrentFilter
		{
			get { return (FilterHelper != null ? FilterHelper.CurrentFilter : null); }
		}

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
				if (_dataCorpusVwSortOptions == null)
				{
					_dataCorpusVwSortOptions = new SortOptions(true, this);
					_dataCorpusVwSortOptions.AdvancedEnabled = false;
				}

				return _dataCorpusVwSortOptions;
			}
			set
			{
				_dataCorpusVwSortOptions = value;
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
				if (_searchVwSortOptions == null)
				{
					_searchVwSortOptions = new SortOptions(true, this);
					_searchVwSortOptions.AdvancedEnabled = true;
				}

				return _searchVwSortOptions;
			}
			
			set 
			{
				_searchVwSortOptions = value;
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
				if (_distChartVwSortOptions == null)
				{
					_distChartVwSortOptions = new SortOptions(true, this);
					_distChartVwSortOptions.AdvancedEnabled = true;
				}
				
				return _distChartVwSortOptions;
			}
			set
			{
				_distChartVwSortOptions = value;
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
		[XmlIgnore]
		public IEnumerable<string> LastNewlyMappedFields { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public GridLayoutInfo GridLayoutInfo { get; private set; }

		#endregion

		#region Segment grid information
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ConsonantChartLayoutFile
		{
			get { return ProjectPathFilePrefix + "ConsonantChart.xml"; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string VowelChartLayoutFile
		{
			get { return ProjectPathFilePrefix + "VowelChart.xml"; }
		}
		
		#endregion
	}
}
