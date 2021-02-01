// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
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
			else if (ds.Type == DataSourceType.FW7)
			{
                if (File.Exists(ds.FwDataSourceInfo.Name))
                {
	                if (!File.Exists(ds.FwDataSourceInfo.Name))
	                {
		                var oldProjectFolder = Path.GetDirectoryName(Path.GetDirectoryName(ds.FwDataSourceInfo.Name));
		                var newFwProjectFolder = Utils.FwProjectsPath;
		                if (newFwProjectFolder != null)
		                {
			                if (oldProjectFolder != null)
				                ds.FwDataSourceInfo.Name = ds.FwDataSourceInfo.Name.Replace(oldProjectFolder, newFwProjectFolder).Replace(@"\\", @"\");
		                }
		                if (!File.Exists(ds.FwDataSourceInfo.Name))
		                {
			                App.NotifyUserOfProblem(LocalizationManager.GetString(
					                "Miscellaneous.Messages.DataSourceReading.FieldWorks7DataMissingMsg",
					                "FieldWorks 7.0 (or later) data is missing. It must be acquired in order for Phonology Assistant to read the data source '{0}'. This data source will be skipped."),
				                ds.FwDataSourceInfo.Name);

			                ds.SkipLoadingBecauseOfProblem = true;
			                return;
		                }
	                }
				}
			}
			else if (!File.Exists(ds.SourceFile))
			{
			    if (!Path.IsPathRooted(ds.SourceFile))
			        ds.SourceFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ds.SourceFile);
			    if (!File.Exists(ds.SourceFile))
			    {
                    string newPath = GetMissingDataSourceAction(ds.SourceFile);
                    if (newPath == null)
                    {
                        ds.SkipLoadingBecauseOfProblem = true;
                        return;
                    }

                    ds.SourceFile = newPath;
			    }
                m_project.Save();
            }

			if (ds.SkipLoadingBecauseOfProblem)
				return;

			if (ds.Type != DataSourceType.XML && ds.Type != DataSourceType.Unknown)
				m_dataSources.Add(ds);

			if (!ds.VerifyMappings())
			{
				App.NotifyUserOfProblem(LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceReading.MarkersMissingFromDataSourceMsg",
					"The data source file '{0}' is missing some standard format markers that were " +
					"assigned to Phonology Assistant fields. Those assignments have been removed. To verify the " +
					"assignment of markers to fields, go to the project settings dialog box, select " +
					"the data source and click 'Properties'."), ds.SourceFile);
			}
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
			dlg.Title = LocalizationManager.GetString(
				"Miscellaneous.Messages.DataSourceReading.SpecifyNewLocationForDatasourceOpenFileDlgCaption",
				"Choose New Data Source Location");
			
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
				var msg = LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceReading.ReadingDataSourceProgressMsg",
					"Reading {0}...");
				
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
			else if (e.ProgressPercentage == -1)
				App.CloseSplashScreen();
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


					if (readSuccess)
					{
						ds.UpdateLastModifiedTime();
						worker.ReportProgress(0, new object[] { "AfterReadingDataSource", ds });
					}
					else
					{
						worker.ReportProgress(-1);
						var msg = LocalizationManager.GetString(
							"Miscellaneous.Messages.DataSourceReading.DataSourceFileUnsuccessfullyReadMsg",
							"Error processing data source file '{0}'.");
						App.NotifyUserOfProblem(msg, ds.SourceFile);

						worker.ReportProgress(0, new object[] { "AfterReadingDataSourceFailure", ds });
					}
				}
				catch (Exception ex)
				{
					worker.ReportProgress(-1);
					var msg = LocalizationManager.GetString(
						"Miscellaneous.Messages.DataSourceReading.DatasourceFileReadingErrorMsg",
						"An error occurred while reading data source file '{0}'.");
					App.NotifyUserOfProblem(ex, msg, ds.SourceFile);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private string GetPhoneticMappingErrorMsg()
		{
			return LocalizationManager.GetString("Miscellaneous.Messages.DataSourceReading.DataSourcePhoneticMappingErrorMsg",
				"A field mapping to the phonetic field could not be found for the data source '{0}'");
		}

		#region FieldWorks 6 (and older) data source reading
		/// ------------------------------------------------------------------------------------
		private void ReadFw6DataSource(BackgroundWorker worker, PaDataSource ds)
		{
			var reader = Fw6DataSourceReader.Create(worker, m_project, ds);

			if (reader == null)
				App.NotifyUserOfProblem(GetPhoneticMappingErrorMsg(), ds.FwPrjName);
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
				App.NotifyUserOfProblem(GetPhoneticMappingErrorMsg(), ds.FwPrjName);
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
				App.NotifyUserOfProblem(GetPhoneticMappingErrorMsg(), ds.SourceFile);
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
				App.NotifyUserOfProblem(GetPhoneticMappingErrorMsg(), ds.FwPrjName);
				return false;
			}

			reader.Read(m_recCache);
			reader.Dispose();
			return true;
		}
		
		#endregion
	}  
}