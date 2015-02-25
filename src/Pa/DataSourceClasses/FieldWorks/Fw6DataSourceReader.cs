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
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Localization;
using Palaso.Reporting;
using SIL.Pa.Model;
using SIL.Pa.Properties;

namespace SIL.Pa.DataSource.FieldWorks
{
	public class Fw6DataSourceReader : IDisposable
	{
		private PaProject m_project;
		private PaDataSource m_dataSource;
		private FwDataSourceInfo m_fwDsInfo;
		private BackgroundWorker m_worker;
		private RecordCache m_recCache;

		/// ------------------------------------------------------------------------------------
		public static Fw6DataSourceReader Create(BackgroundWorker worker, PaProject project, PaDataSource ds)
		{
			var eticMapping = ds.FieldMappings.Single(m => m.Field.Type == FieldType.Phonetic);
			if (eticMapping == null)
				return null;

			var reader = new Fw6DataSourceReader();
			reader.m_worker = worker;
			reader.m_project = project;
			reader.m_dataSource = ds;
			reader.m_fwDsInfo = ds.FwDataSourceInfo;

			return reader;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			m_worker = null;
			m_project = null;
			m_dataSource = null;
			m_fwDsInfo = null;
		}

		/// ------------------------------------------------------------------------------------
		public void Read(RecordCache recCache)
		{
			m_worker.ReportProgress(0);

			if (m_dataSource.FwDataSourceInfo == null)
				return;

			m_recCache = recCache;

			// Get the lexical data from the FW database.
			m_dataSource.SkipLoadingBecauseOfProblem = !GetData();
			m_dataSource.FwDataSourceInfo.IsMissing = m_dataSource.SkipLoadingBecauseOfProblem;

			if (!m_dataSource.SkipLoadingBecauseOfProblem && !m_dataSource.FwDataSourceInfo.IsMissing)
				m_dataSource.UpdateLastModifiedTime();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the data from the SQL database. Returns false if reading failed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetData()
		{
			if (m_fwDsInfo.Queries == null || m_fwDsInfo.Queries.Error)
				return false;

			string sql = (m_fwDsInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm ?
				m_fwDsInfo.Queries.LexemeFormSQL : m_fwDsInfo.Queries.PronunciationFieldSQL);

			if (m_dataSource.FieldMappings == null || m_dataSource.FieldMappings.Count == 0)
			{
				ErrorReport.NotifyUserOfProblem(LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceReading.MissingFieldWorks6WritingSystemsMsg",
					"There are no writing systems for the '{0}' project. Therefore, no data " +
					"from it can be displayed. To fix this problem, modify the FieldWorks data " +
					"source properties for this project by selecting 'Project Settings' from " +
					"the File menu. Then select the project in the data sources list and click " +
					"the 'Properties' button."), m_fwDsInfo.ProjectName);
				return true;
			}

			foreach (var fname in Settings.Default.AllPossibleFw6Fields.Cast<string>())
			{
				var mapping = m_dataSource.FieldMappings.SingleOrDefault(m => m.Field.Name == fname);
				var replace = string.Format("${0}Ws$", fname);
				sql = sql.Replace(replace, (mapping != null && mapping.FwWsId != null ? mapping.FwWsId : "0"));
			}

			try
			{
				using (var connection = FwDBUtils.FwConnection(m_fwDsInfo.Name, m_fwDsInfo.Server))
				{
					var command = new SqlCommand(sql, connection);
					using (var reader = command.ExecuteReader())
					{
						if (reader.HasRows)
							HandleReadingFwData(reader);

						reader.Close();
					}

					connection.Close();
				}
			}
			catch (Exception e)
			{
				ErrorReport.NotifyUserOfProblem(e, LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceReading.ErrorRetrievingFieldWorks6DataMsg",
					"There was an error retrieving the data from the {0} database. It's " +
					"possible the file '{1}' is either missing or corrupt. Reading this " +
					"data will be skipped."), m_fwDsInfo.Name, Path.GetFileName(m_fwDsInfo.Queries.QueryFile));

			    return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleReadingFwData(SqlDataReader reader)
		{
			if (reader == null || reader.IsClosed)
				return;

			// First, get a list of the fields returned from the query
			// and translate those to their corresponding PA fields.
			var fieldNames = new Dictionary<int, string>();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				if (m_dataSource.FieldMappings.Any(m => m.Field.Name == reader.GetName(i)))
					fieldNames[i] = reader.GetName(i);
			}

			var eticField = m_dataSource.FieldMappings.FirstOrDefault(m => m.Field.Type == FieldType.Phonetic);
			var eticFieldName = (eticField != null ? eticField.PaFieldName : null);

			while (reader.Read())
			{
				// Make a new record entry for each row returned from the query.
				var recCacheEntry = new RecordCacheEntry(false, m_project)
				{
					DataSource = m_dataSource,
					NeedsParsing = false,
					WordEntries = new List<WordCacheEntry>(),
				};

				var wentry = new WordCacheEntry(recCacheEntry);

				// Read the data for all columns having a mapped field.
				foreach (var kvp in fieldNames)
				{
					var dbValue = reader[kvp.Key];

					if (dbValue is DBNull)
						continue;

					// Put the phonetic field in a word entry and all the other data in the
					// record entry.
					if (kvp.Value != eticFieldName)
						recCacheEntry.SetValue(kvp.Value, dbValue.ToString());
					else
						wentry.SetValue(kvp.Value, dbValue.ToString());
				}

				var guid = reader["Guid"];
				if (!(guid is DBNull))
					recCacheEntry.Guid = new Guid(guid.ToString());

				// Add the entries to the caches.
				recCacheEntry.WordEntries.Add(wentry);
				m_recCache.Add(recCacheEntry);
			}
		}
	}
}
