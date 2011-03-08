using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.DataSource.Sa;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.DataSource
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Importer class parses through an SF file and sends Records to Import
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DataSourceReader
	{
		protected RecordCache m_recCache;
		protected RecordCacheEntry m_recCacheEntry;
		protected PaProject m_project;
		protected List<PaDataSource> m_dataSources;
		protected Dictionary<string, List<string>> m_fieldsForMarkers;
		protected IEnumerable<string> m_interlinearFields;
		protected ToolStripProgressBar m_progressBar;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a data source reader object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataSourceReader(PaProject project)
		{
			m_project = project;
			m_dataSources = new List<PaDataSource>();

			CheckNeedForSQLServer(project.DataSources.Where(ds => !ds.SkipLoading));

			foreach (var ds in project.DataSources.Where(ds => !ds.SkipLoading && !ds.SkipLoadingBecauseOfProblem))
				Initialize(ds);
		}

		/// ------------------------------------------------------------------------------------
		private void Initialize(PaDataSource ds)
		{
			if (ds.Type == DataSourceType.FW && ds.FwSourceDirectFromDB)
			{
				CheckExistenceOfFwDatabase(ds);
			}
			else if (!File.Exists(ds.SourceFile))
			{
				string newPath = GetMissingDataSourceAction(ds.SourceFile);
				if (newPath == null)
				{
					ds.SkipLoadingBecauseOfProblem = true;
					return;
				}

				ds.SourceFile = newPath;
				m_project.Save();
			}

			if (ds.SkipLoadingBecauseOfProblem)
				return;

			if (ds.Type == DataSourceType.FW7 && !FwDBUtils.IsFw7Installed)
			{
				var msg = App.LocalizeString("FieldWorks7NotInstalledMsg",
				    "FieldWorks 7.0 (or later) is not installed. It must be installed\nin order for {0} to read the data source\n\n'{1}'.\n\nThis data source will be skipped.",
				    App.kLocalizationGroupMisc);

				Utils.MsgBox(string.Format(msg, Application.ProductName, ds.SourceFile));
				ds.SkipLoadingBecauseOfProblem = true;
				return;
			}

			if (ds.Type != DataSourceType.XML && ds.Type != DataSourceType.Unknown)
				m_dataSources.Add(ds);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cycles through the data sources, checking if any are sources directly from a
		/// FW database. If so, then an attempt is made to start SQL server if it isn't
		/// already started.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void CheckNeedForSQLServer(IEnumerable<PaDataSource> dataSourcesToLoad)
		{
			bool alreadyTriedToStartSQLServer = false;

			foreach (var ds in dataSourcesToLoad.Where(ds => ds.Type == DataSourceType.FW && ds.FwSourceDirectFromDB))
			{
				if (!alreadyTriedToStartSQLServer && FwDBUtils.StartSQLServer(true))
					return;

				alreadyTriedToStartSQLServer = true;
				ds.SkipLoadingBecauseOfProblem = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the existence of the specified FW data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void CheckExistenceOfFwDatabase(PaDataSource ds)
		{
			if (ds == null)
				return;

			if (!FwDBUtils.IsSQLServerStarted && !FwDBUtils.StartSQLServer(true))
				return;

			if (ds.FwDataSourceInfo != null)
			{
				var fwDBInfoList =
					FwDBUtils.GetFwDataSourceInfoList(ds.FwDataSourceInfo.Server, false);

				if (fwDBInfoList.Any(fwinfo => ds.FwPrjName == fwinfo.Name))
					return;

				ds.FwDataSourceInfo.IsMissing = true;
			}

			MissingFWDatabaseMsgBox.ShowDialog(ds.ToString(true));
			ds.SkipLoadingBecauseOfProblem = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns null if the user wants to skip loading a missing data source or a full
		/// file path the user specified as the relocated data source file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetMissingDataSourceAction(string dataSourceFile)
		{
			if (MissingDataSourceMsgBox.ShowDialog(dataSourceFile) == DialogResult.Cancel)
				return null;
		
			var dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.FileName = Path.GetFileName(dataSourceFile);
			dlg.Filter = App.kstidFileTypeAllFiles;
			dlg.ShowReadOnly = false;
			dlg.InitialDirectory = Path.GetFullPath(dataSourceFile);
			dlg.Title = App.LocalizeString(
				"SpecifyNewLocationForDatasourceOpenFileDlgCaption",
				"Choose New Data Source Location", App.kLocalizationGroupDialogs);
			
			while (dlg.ShowDialog() == DialogResult.Cancel)
			{
				if (MissingDataSourceMsgBox.ShowDialog(dataSourceFile) == DialogResult.Cancel)
					return null;
			}

			return dlg.FileName;
		}

		/// ------------------------------------------------------------------------------------
		public RecordCache Read()
		{
			TempRecordCache.Dispose();
			m_recCache = new RecordCache(m_project);
			App.IPASymbolCache.ClearUndefinedCharacterCollection();

			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.DoWork += HandleBackgroundProcessStart;
			worker.ProgressChanged += HandleBackgroundProcessProgressChanged;
			worker.RunWorkerAsync();

			while (worker.IsBusy)
				Application.DoEvents();
			
			return m_recCache;
		}

		/// ------------------------------------------------------------------------------------
		void HandleBackgroundProcessProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState is string)
			{
				var msg = App.LocalizeString("ReadingDataSourceProgressMsg", "Reading {0}",
					App.kLocalizationGroupInfoMsg);

				msg = string.Format(msg, e.UserState);
				App.InitializeProgressBar(msg, e.ProgressPercentage);
			}
			else if (e.UserState is object[])
			{
				var msg = (string)((object[])e.UserState)[0];
				var ds = (PaDataSource)((object[])e.UserState)[1];
				App.MsgMediator.SendMessage(msg, ds);
			}
			else if (e.ProgressPercentage == 0)
				App.IncProgressBar();
			else
				App.IncProgressBar(e.ProgressPercentage);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read each data source file into the record cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleBackgroundProcessStart(object sender, DoWorkEventArgs e)
		{
			var worker = sender as BackgroundWorker;

			foreach (var ds in m_dataSources)
			{
				App.IPASymbolCache.UndefinedCharacters.CurrentDataSourceName =
					((ds.Type == DataSourceType.FW || ds.Type == DataSourceType.FW7) &&
					ds.FwDataSourceInfo != null ? ds.FwDataSourceInfo.ToString() : Path.GetFileName(ds.SourceFile));

				m_project.PhoneticParser.LogUndefinedCharactersWhenParsing = true;
				worker.ReportProgress(ds.TotalLinesInFile, ds.DisplayTextWhenReading);
				
				string fmt;

				try
				{
					worker.ReportProgress(0, new object[] { "BeforeReadingDataSource", ds });
					bool readSuccess = true;

					switch (ds.Type)
					{
						case DataSourceType.Toolbox:
						case DataSourceType.SFM: readSuccess = ReadSFMFile(worker, ds); break;
						case DataSourceType.SA: ReadSaSource(worker, ds); break;
						case DataSourceType.FW7: ReadFw7DataSource(worker, ds); break;
						case DataSourceType.XML: break;
						case DataSourceType.FW:
						case DataSourceType.PAXML:
							if (ds.FwSourceDirectFromDB)
								ReadFwDataSource(worker, ds);
							else
								ReadPaXmlFile(worker, ds);
							break;
					}

					worker.ReportProgress(-1);

					if (readSuccess)
					{
						ds.UpdateLastModifiedTime();
						worker.ReportProgress(0, new object[] { "AfterReadingDataSource", ds });
					}
					else
					{
						fmt = App.LocalizeString("DatasourceFileUnsuccessfullyReadMsg",
							"Error processing data source file '{0}'.", App.kLocalizationGroupInfoMsg);

						string msg = string.Format(fmt, Utils.PrepFilePathForMsgBox(ds.SourceFile));
						Utils.MsgBox(msg, MessageBoxIcon.Exclamation);
						worker.ReportProgress(0, new object[] { "AfterReadingDataSourceFailure", ds });
					}
				}
				catch (Exception ex)
				{
					fmt = App.LocalizeString("DatasourceFileReadingErrorMsg",
							"The following error occurred while reading data source file '{0}'.{1}",
							"First parameter is data source file name; second parameter is error message.",
							App.kLocalizationGroupInfoMsg);
		
					string msg = string.Format(fmt, Utils.PrepFilePathForMsgBox(ds.SourceFile), ex.Message);
					Utils.MsgBox(msg, MessageBoxIcon.Exclamation);
				}
			}
		}

		#region FieldWorks 6 (and older) data source reading
		/// ------------------------------------------------------------------------------------
		private void ReadFwDataSource(BackgroundWorker worker, PaDataSource ds)
		{
			worker.ReportProgress(0);

			if (ds.FwDataSourceInfo == null)
				return;

			// Get the lexical data from the FW database.
			var fwReader = new FwDataReader(ds);
			ds.SkipLoadingBecauseOfProblem = !fwReader.GetData(HandleReadingFwData);
			ds.FwDataSourceInfo.IsMissing = ds.SkipLoadingBecauseOfProblem;

			if (!ds.SkipLoadingBecauseOfProblem && !ds.FwDataSourceInfo.IsMissing)
				ds.UpdateLastModifiedTime();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleReadingFwData(PaDataSource ds, SqlDataReader reader)
		{
			if (reader == null || reader.IsClosed)
				return;

			// First, get a list of the fields returned from the query
			// and translate those to their corresponding PA fields.
			var fieldNames = new List<string>();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				// TODO: Fix for new system
				
				//				var field = m_project.FieldInfo.GetFieldFromFwQueryFieldName(reader.GetName(i));
	//			fieldNames.Add(field != null ? fieldInfo.FieldName : null);
			}

			while (reader.Read())
			{
				// Make a new record entry.
				m_recCacheEntry = new RecordCacheEntry(false, m_project);
				m_recCacheEntry.DataSource = ds;
				m_recCacheEntry.NeedsParsing = false;
				m_recCacheEntry.WordEntries = new List<WordCacheEntry>();

				// Make a new word entry because for FW data sources read directly from the
				// database, there will be a one-to-one correspondence between record cache
				// entries and word cache entries.
				var wentry = new WordCacheEntry(m_recCacheEntry, true);

				// Read the data for all columns. If there are columns the record
				// or word entries don't recognize, they'll just be ignored.
				for (int i = 0; i < fieldNames.Count; i++)
				{
					if ((reader[i] is DBNull) || fieldNames[i] == null)
						continue;
					
					m_recCacheEntry.SetValue(fieldNames[i], reader[i].ToString());
					wentry.SetValue(fieldNames[i], reader[i].ToString());
				}

				// Add the entries to the caches.
				m_recCacheEntry.WordEntries.Add(wentry);
				m_recCache.Add(m_recCacheEntry);
			}
		}

		#endregion

		#region FieldWorks 7 and later data source reading methods
		/// ------------------------------------------------------------------------------------
		private void ReadFw7DataSource(BackgroundWorker worker, PaDataSource ds)
		{
			var reader = Fw7DataSourceReader.Create(worker, m_project, ds);

			if (reader != null)
			{
				reader.Read(m_recCache);
				reader.Dispose();
			}
			else
			{
				var msg = App.LocalizeString("DatasourcePhoneticMappingErrorMsg",
							"A field mapping to the phonetic field could not be found for the data source '{0}'",
							"First parameter is data source name.",
							App.kLocalizationGroupInfoMsg);

				Utils.MsgBox(string.Format(msg, ds.FwPrjName));
			}
		}

		#endregion

		#region PaXml data source reading methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reads a PA XML file into the record cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ReadPaXmlFile(BackgroundWorker worker, PaDataSource ds)
		{
			var cache = RecordCache.Load(ds, m_project);
			if (cache == null)
				return;

			AddCustomFieldsFromPaXML(cache);

			// If the record cache member variable currently points to an empty cache, then
			// just set it to point to the cache into which we just read the specified PaXML
			// file. Otherwise move the records from the cache we just read into to the
			// member variable cache.
			if (m_recCache.Count == 0)
			{
				worker.ReportProgress(-1);
				m_recCache = cache;
			}
			else
			{
				while (cache.Count > 0)
				{
					worker.ReportProgress(0);
					m_recCache.Add(cache[0]);
					cache.RemoveAt(0);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure custom fields stored in the PaXML file are added to the project's field
		/// list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddCustomFieldsFromPaXML(RecordCache cache)
		{
			// TODO: Fix
			//if (cache.DeserializedCustomFields != null &&
			//    cache.DeserializedCustomFields.Count > 0)
			//{
			//    foreach (PaFieldInfo customField in cache.DeserializedCustomFields)
			//    {
			//        PaFieldInfo fieldInfo = m_project.FieldInfo[customField.FieldName];
			//        if (fieldInfo == null)
			//            m_project.FieldInfo.Add(customField);
			//    }

			//    cache.DeserializedCustomFields = null;
			//}
		}

		#endregion

		#region SA data source reading methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reads an SA sound file data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ReadSaSource(BackgroundWorker worker, PaDataSource ds)
		{
			var reader = new SaAudioDocumentReader();
			if (!reader.Initialize(ds.SourceFile))
				return;

			if (reader.Words == null)
				return;
			
			// Make only a single record entry for the entire wave file.
			m_recCacheEntry = new RecordCacheEntry(false, m_project);
			m_recCacheEntry.DataSource = ds;
			m_recCacheEntry.NeedsParsing = false;
			m_recCacheEntry.Channels = reader.Channels;
			m_recCacheEntry.BitsPerSample = reader.BitsPerSample;
			m_recCacheEntry.SamplesPerSecond = reader.SamplesPerSecond;

			var saFields = PaField.GetSaFields();

			var audioField = saFields.SingleOrDefault(f => f.Type == FieldType.AudioFilePath);
			if (audioField != null)
				m_recCacheEntry.SetValue(audioField.Name, ds.SourceFile);

			worker.ReportProgress(0);
			int wordIndex = 0;
			var wordEntries = new List<WordCacheEntry>();

			// Get all the unparsed fields.
			foreach (var fname in ds.FieldMappings.Where(m => !m.IsParsed).Select(m => m.Field.Name))
			{
				var value = GetPropertyValueFromObject(typeof(SaAudioDocumentReader), fname, reader);
				if (value != null)
					m_recCacheEntry.SetValue(fname, value.ToString());
			}

			// Get all the parsed fields.
			foreach (var adw in reader.Words.Values)
			{
				var wentry = new WordCacheEntry(m_recCacheEntry, wordIndex++, true);

				foreach (var fname in ds.FieldMappings.Where(m => !m.IsParsed).Select(m => m.Field.Name))
				{
					var value = GetPropertyValueFromObject(typeof(AudioDocWords), fname, adw);
					if (value != null)
						wentry.SetValue(fname, value.ToString());
				}

				wordEntries.Add(wentry);
			}

			m_recCacheEntry.WordEntries = wordEntries;
			m_recCache.Add(m_recCacheEntry);
		}

		/// ------------------------------------------------------------------------------------
		private static object GetPropertyValueFromObject(Type type, string propName, object srcObject)
		{
			const BindingFlags kFlags = BindingFlags.GetProperty |
				BindingFlags.Instance | BindingFlags.Public;

			try
			{
				return type.InvokeMember(propName, kFlags, null, srcObject, null);
			}
			catch
			{
				return null;
			}
		}

		///// ------------------------------------------------------------------------------------
		// TODO: Figure out if needed.
		//private static bool HandleWaveFileNeedsConverting(string filename)
		//{
		//    if (AudioPlayer.GetSaPath() != null)
		//        return true;

		//    var msg = App.LocalizeString("AudioConvertProblemMsg",
		//        "It appears the audio file '{0}' may have been created using an old version " +
		//        "of Speech Analyzer. In order for Phonology Assistant to read data associated " +
		//        "with the audio file it must first be converted using some components of " +
		//        "Speech Analyzer 3.0.1, but it is not installed. Please install Speech Analyzer " +
		//        "3.0.1 and try again.", "Message displayed when SA 3.0.1 is not installed and " +
		//        "PA is trying to read an audio file created using a version SA older than 3.0.1.",
		//        App.kLocalizationGroupInfoMsg);

		//    msg = string.Format(msg, filename);

		//    using (var dlg = new DownloadSaDlg(msg))
		//        dlg.ShowDialog();

		//    return false;
		//}

		#endregion

		#region SFM file reading methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reads a single SFM file
		/// </summary>
		/// <returns>True indicates success</returns>
		/// ------------------------------------------------------------------------------------
		protected bool ReadSFMFile(BackgroundWorker worker, PaDataSource ds)
		{
			BuildListOfFieldsForMarkers(ds);
			var recMarker = ds.SfmRecordMarker;
			if (string.IsNullOrEmpty(recMarker))
				return false;

			var field = new StringBuilder();
			bool onFirstRecordMarker = false;
			bool foundFirstRecord = false;
			m_recCacheEntry = null;

			foreach (var line in File.ReadAllLines(ds.SourceFile))
			{
				worker.ReportProgress(0);
				var currLine = line.Trim();
				string currMarker = null;

				if (currLine.StartsWith("\\"))
				{
					string[] split = currLine.Split(" ".ToCharArray(), 2);
					if (split.Length >= 1)
						currMarker = split[0];
				}

				// Ignore all data up to the first record.
				if (!foundFirstRecord)
				{
					if (currMarker != recMarker)
						continue;

					foundFirstRecord = true;
					onFirstRecordMarker = true;
				}

				// If the current line doesn't start a new field, then add it to the
				// previous line's data.
				if (!currLine.StartsWith("\\"))
				{
					field.Append(" " + currLine);
					continue;
				}

				// At this point, if the string builder contains any data, we know we've
				// finished reading the data for a single field. Therefore, save the field's
				// data.
				if (field.Length > 0)
					ParseAndStoreSFMField(ds, field.ToString());

				// Check if we've come to the beginning of a record. If so, then make sure
				// to save the contents of the previous record and begin a new one.
				if (currMarker == recMarker)
				{
					if (onFirstRecordMarker)
						onFirstRecordMarker = false;
					else if (m_recCacheEntry != null)
					{
						m_recCache.Add(m_recCacheEntry);
						m_recCacheEntry = null;
					}
				}

				// Prepare to start a new field's data accumulation and add the line
				// just read from the file.
				field.Length = 0;
				field.Append(currLine);
			}

			// Process the final field in the file
			if (field.Length > 0)
				ParseAndStoreSFMField(ds, field.ToString());

			// Save the final record in the file
			if (m_recCacheEntry != null)
			{
				m_recCache.Add(m_recCacheEntry);
				m_recCacheEntry = null;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a list of mappings and all the fields that are mapped to those markers.
		/// Also, a list of all the fields marked as interlinear are saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildListOfFieldsForMarkers(PaDataSource ds)
		{
			m_interlinearFields = ds.FieldMappings.Where(m => m.IsInterlinear).Select(m => m.Field.Name).ToList();

			m_fieldsForMarkers = new Dictionary<string, List<string>>();
			foreach (var mapping in ds.FieldMappings)
			{
				if (!m_fieldsForMarkers.ContainsKey(mapping.NameInDataSource))
					m_fieldsForMarkers[mapping.NameInDataSource] = new List<string>();

				m_fieldsForMarkers[mapping.NameInDataSource].Add(mapping.Field.Name);
			}

			//foreach (SFMarkerMapping mapping in m_mappings)
			//{
			//    if (mapping.FieldName != PaDataSource.kRecordMarker && !string.IsNullOrEmpty(mapping.Marker))
			//    {
			//        if (!m_fieldsForMarkers.ContainsKey(mapping.Marker))
			//            m_fieldsForMarkers[mapping.Marker] = new List<string>();

			//        m_fieldsForMarkers[mapping.Marker].Add(mapping.FieldName);

			//        if (mapping.IsInterlinear)
			//            m_interlinearFields.Add(mapping.FieldName);
			//    }
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds to the record cache the data for a single field read from an SFM data source
		/// record.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ParseAndStoreSFMField(PaDataSource ds, string field)
		{
			if (field == null)
				return;

			field = field.Trim();

			if (!field.StartsWith("\\"))
				return;

			// Get two strings, the first containing the backslash marker
			// and the other contianing the data following the marker.
			string[] split = field.Split(" ".ToCharArray(), 2);

			// If we didn't get two strings back then ignore this field.
			if (split.Length != 2)
				return;

			// If there's no mapping for the marker, ignore this field.
			if (!m_fieldsForMarkers.ContainsKey(split[0]))
				return;

			if (m_recCacheEntry == null)
			{
				m_recCacheEntry = new RecordCacheEntry(true, m_project);
				m_recCacheEntry.NeedsParsing = true;
				m_recCacheEntry.DataSource = ds;
				m_recCacheEntry.FirstInterlinearField = ds.FirstInterlinearField;
				m_recCacheEntry.InterlinearFields = m_interlinearFields.ToList();
			}

			var audioFilefieldInfo = m_project.GetAudioFileField();
			var offsetFieldInfo = m_project.GetAudioOffsetField();
			var lengthFieldInfo = m_project.GetAudioLengthField();

			foreach (string fld in m_fieldsForMarkers[split[0]])
			{
				if (audioFilefieldInfo == null || audioFilefieldInfo.Name != fld)
				{
					m_recCacheEntry.SetValue(fld, split[1]);
					continue;
				}

				long startPoint;
				long endPoint;
				string audioFileName = ParseSoundFileName(split[1], out startPoint, out endPoint);

				if (!string.IsNullOrEmpty(audioFileName))
				{
					m_recCacheEntry.SetValue(audioFilefieldInfo.Name, audioFileName);

					if (offsetFieldInfo != null && lengthFieldInfo != null)
					{
						long length = endPoint - startPoint;
						m_recCacheEntry.SetValue(offsetFieldInfo.Name, startPoint.ToString());
						m_recCacheEntry.SetValue(lengthFieldInfo.Name, length.ToString());
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parse the sound file name returning the file name and beginning / ending points
		/// for playing, if any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string ParseSoundFileName(string fileName, out long startPoint, out long endPoint)
		{
			startPoint = 0L;
			endPoint = 0L;

			// Handle null
			if (fileName == null)
				return string.Empty;

			// Handle empty string
			fileName = fileName.Trim();
			if (fileName == string.Empty)
				return string.Empty;

			// Check if the end of the file name string is one of the common audio file types.
			// If so, there's no sense in checking for start and end points since the entire
			// string ends with a file extension.
			if (new[] { ".wav", ".mp3", ".wma", ".ogg", ".ram", ".aif", ".au", ".voc" }.Any(ext => fileName.ToLower().EndsWith(ext)))
			{
				return fileName;
			}

			// Find the last space in the file name string. If one cannot be
			// found then we know there is no start and stop point.
			int iSpace = fileName.LastIndexOf(' ');
			if (iSpace < 0)
				return fileName;

			// Remove everything following the last space and assume it's a numeric value
			// indicating a start or stop point (in seconds) in the audio file.
			string sTime2 = fileName.Substring(iSpace);
			
			// Try to convert the value to a numeric.
			float fTime2;
			if (!Utils.TryFloatParse(sTime2, out fTime2))
				return fileName;

			// Remove from the file name string the text removed that represents a numeric value.
			fileName = fileName.Substring(0, iSpace);

			// Find the last space in the file name string. If one cannot be
			// found then we know there is only a start point and stop point.
			iSpace = fileName.LastIndexOf(' ');
			if (iSpace < 0)
			{
				// Assume the time was specified in seconds. We want it in milliseconds.
				startPoint = (long)(Math.Ceiling(fTime2 * 1000f));
				return fileName;
			}

			// Remove everything following the last space and assume it's a numeric value
			// indicating a start point (in seconds) in the audio file.
			string sTime1 = fileName.Substring(iSpace);

			// Try to convert the value to a numeric.
			float fTime1;
			if (!Utils.TryFloatParse(sTime1, out fTime1))
			{
				// Assume the time was specified in seconds. We want it in milliseconds.
				// The parse attempt failed so just make the start and end times the same.
				startPoint = (long)(Math.Ceiling(fTime2 * 1000f));
				return fileName;
			}

			// Assume the times are specified in seconds. We want it in milliseconds.
			startPoint = (long)(Math.Ceiling(fTime1 * 1000f));
			endPoint = (long)(Math.Ceiling(fTime2 * 1000f));

			// Remove from the file name string the text removed that represents the start point.
			return fileName.Substring(0, iSpace);
		}

		#endregion
	}  
}