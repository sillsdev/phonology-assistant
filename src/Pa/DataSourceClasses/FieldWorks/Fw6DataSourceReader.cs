using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

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
				var noWsMsg = App.GetString("MissingFieldWorks6WritingSystemsMsg",
					"There are no writing systems for the '{0}'\n project. Therefore, no data from it can be displayed.\n\nTo fix this problem, modify the FieldWorks data source\nproperties for this project by selecting 'Project Settings'\nfrom the File menu. Then select the project in the data\nsources list and click the 'Properties' button.");

				Utils.MsgBox(string.Format(noWsMsg, m_fwDsInfo.ProjectName));
				return true;
			}

			foreach (var fname in Settings.Default.DefaultFw6Fields.Cast<string>())
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
				var msg = App.GetString("ErrorRetrievingFieldWorks6DataMsg",
					"There was an error retrieving the data from the {0} database.\nIt's possible the file {1} is either missing or\ncorrupt. Reading this data will be skipped.\n\n{2}");

				Utils.MsgBox(string.Format(msg, m_fwDsInfo.Name,
					Path.GetFileName(m_fwDsInfo.Queries.QueryFile), e.Message));

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

			while (reader.Read())
			{
				// Make a new record entry for each row returned from the query.
				var recCacheEntry = new RecordCacheEntry(false, m_project)
				{
					DataSource = m_dataSource,
					NeedsParsing = false,
					WordEntries = new List<WordCacheEntry>(),
				};

				// Make a new word entry because for FW data sources read directly from the
				// database, there will be a one-to-one correspondence between record cache
				// entries and word cache entries.
				var wentry = new WordCacheEntry(recCacheEntry, true);

				// Read the data for all columns having a mapped field.
				foreach (var kvp in fieldNames)
				{
					var dbValue = reader[kvp.Key];

					if (dbValue is DBNull)
						continue;

					recCacheEntry.SetValue(kvp.Value, dbValue.ToString());
					wentry.SetValue(kvp.Value, dbValue.ToString());
				}

				// Add the entries to the caches.
				recCacheEntry.WordEntries.Add(wentry);
				m_recCache.Add(recCacheEntry);
			}
		}
	}
}
