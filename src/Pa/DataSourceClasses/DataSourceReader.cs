using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
		protected PaProject m_project;
		protected List<PaDataSource> m_dataSources;
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

			var worker = new BackgroundWorker { WorkerReportsProgress = true };
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
						case DataSourceType.SFM: readSuccess = ReadSfmDataSource(worker, ds); break;
						case DataSourceType.SA: ReadSaDataSource(worker, ds); break;
						case DataSourceType.FW7: ReadFw7DataSource(worker, ds); break;
						case DataSourceType.XML: break;
						case DataSourceType.FW:
						case DataSourceType.PAXML:
							if (ds.FwSourceDirectFromDB)
								ReadFw6DataSource(worker, ds);
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

		/// ------------------------------------------------------------------------------------
		private string GetPhoneticMappingErrorMsg()
		{
			return App.LocalizeString("DatasourcePhoneticMappingErrorMsg",
				"A field mapping to the phonetic field could not be found for the data source '{0}'",
				"First parameter is data source name.",
				App.kLocalizationGroupInfoMsg);
		}

		#region FieldWorks 6 (and older) data source reading
		/// ------------------------------------------------------------------------------------
		private void ReadFw6DataSource(BackgroundWorker worker, PaDataSource ds)
		{
			var reader = Fw6DataSourceReader.Create(worker, m_project, ds);

			if (reader == null)
				Utils.MsgBox(string.Format(GetPhoneticMappingErrorMsg(), ds.FwPrjName));
			else
			{
				reader.Read(m_recCache);
				reader.Dispose();
			}
		}

		#endregion

		#region FieldWorks 7 and later data source reading methods
		/// ------------------------------------------------------------------------------------
		private void ReadFw7DataSource(BackgroundWorker worker, PaDataSource ds)
		{
			var reader = Fw7DataSourceReader.Create(worker, m_project, ds);

			if (reader == null)
				Utils.MsgBox(string.Format(GetPhoneticMappingErrorMsg(), ds.FwPrjName));
			else
			{
				reader.Read(m_recCache);
				reader.Dispose();
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
		protected bool ReadSaDataSource(BackgroundWorker worker, PaDataSource ds)
		{
			var reader = SaDataSourceReader.Create(worker, m_project, ds);

			if (reader == null)
			{
				Utils.MsgBox(string.Format(GetPhoneticMappingErrorMsg(), ds.SourceFile));
				return false;
			}

			reader.Read(m_recCache);
			reader.Dispose();
			return true;
		}

		#endregion

		#region SFM file reading methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reads a single SFM file
		/// </summary>
		/// <returns>True indicates success</returns>
		/// ------------------------------------------------------------------------------------
		protected bool ReadSfmDataSource(BackgroundWorker worker, PaDataSource ds)
		{
			var reader = SfmDataSourceReader.Create(worker, m_project, ds);

			if (reader == null)
			{
				Utils.MsgBox(string.Format(GetPhoneticMappingErrorMsg(), ds.FwPrjName));
				return false;
			}

			reader.Read(m_recCache);
			reader.Dispose();
			return true;
		}
		
		#endregion
	}  
}