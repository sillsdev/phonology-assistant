using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public class FwDataReader
	{
		public delegate void DataRetrievedHandler(SqlDataReader reader);

		private readonly FwDataSourceInfo m_sourceInfo;

		/// ------------------------------------------------------------------------------------
		public FwDataReader(FwDataSourceInfo sourceInfo)
		{
			m_sourceInfo = sourceInfo;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of all analysis and vernacular writing systems in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<FwWritingSysInfo> WritingSystems
		{
			get
			{
				if (m_sourceInfo.DataSourceType == DataSourceType.FW)
				{
					var msg = App.LocalizeString("ErrorRetrievingFwWritingSystemsMsg",
						"There was an error retrieving writing systems from the {0}\nproject. It's possible the file {1} is either missing or corrupt.",
						App.kLocalizationGroupMisc);

					msg = string.Format(msg, m_sourceInfo.Name, Path.GetFileName(m_sourceInfo.Queries.QueryFile));
					var wsInfoList = GetWritingSystems(m_sourceInfo.Queries.AnalysisWs, msg)
						.Select(ws => new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Analysis, ws.Key, ws.Value)).ToList();

					// Build a list of the analysis writing systems.
					wsInfoList.AddRange(GetWritingSystems(m_sourceInfo.Queries.VernacularWsSQL, msg)
						.Select(ws => new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Vernacular, ws.Key, ws.Value)));

					return wsInfoList;
				}
		
				// Return writing systems from a 7.0 (or later) project).
				try
				{
					return FwDBUtils.GetWritingSystemsForFw7Project(m_sourceInfo.Name, m_sourceInfo.Server);
				}
				catch (Exception e)
				{
					var msg = App.LocalizeString("ErrorRetrievingFwWritingSystemsMsg",
						"There was an error retrieving writing systems from\nthe {0} project.\n\n{1}",
						App.kLocalizationGroupMisc);

					msg = string.Format(msg, m_sourceInfo.ProjectName, e.Message);
					Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return null;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems returned by the SQL statement found in the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Dictionary<int, string> GetWritingSystems(string sql, string errMsg)
		{
			var wsCollection = new Dictionary<int, string>();

			try
			{
				using (var connection = FwDBUtils.FwConnection(m_sourceInfo.Name, m_sourceInfo.Server))
				{
					if (connection != null && !string.IsNullOrEmpty(sql))
					{
						var command = new SqlCommand(sql, connection);
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
								wsCollection[(int)reader["Obj"]] = reader["Txt"] as string;

							reader.Close();
						}

						connection.Close();
					}
				}

				// There should be at least one writing system defined.
				if (wsCollection.Count == 0)
				{
					Utils.MsgBox(string.Format(errMsg, m_sourceInfo.Name,
						Path.GetFileName(m_sourceInfo.Queries.QueryFile), string.Empty),
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception e)
			{
				Utils.MsgBox(string.Format(errMsg, m_sourceInfo.Name,
					Path.GetFileName(m_sourceInfo.Queries.QueryFile), e.Message),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return wsCollection;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the data from the MSDE database. Returns false if reading failed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetData(DataRetrievedHandler dataRetrievedHdlr)
		{
			if (m_sourceInfo.Queries == null || m_sourceInfo.Queries.Error)
				return false;

			string errMsg = Properties.Resources.kstidErrorRetrievingFwDataMsg;
			string sql =
				(m_sourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm ?
				m_sourceInfo.Queries.LexemeFormSQL : m_sourceInfo.Queries.PronunciationFieldSQL);

			if (m_sourceInfo.WsMappings == null || m_sourceInfo.WsMappings.Count == 0)
			{
				errMsg = string.Format(Properties.Resources.kstidMissingWsMsg,
					m_sourceInfo.ProjectName);

				Utils.MsgBox(errMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}

			foreach (var dswsi in m_sourceInfo.WsMappings)
			{
				var replace = string.Format("${0}Ws$", dswsi.FieldName);
				sql = sql.Replace(replace, dswsi.WsHvo.ToString());
			}

			try
			{
				using (var connection = FwDBUtils.FwConnection(m_sourceInfo.Name, m_sourceInfo.Server))
				{
					var command = new SqlCommand(sql, connection);
					using (var reader = command.ExecuteReader())
					{
						if (reader.HasRows && dataRetrievedHdlr != null)
							dataRetrievedHdlr(reader);

						reader.Close();
					}

					connection.Close();
				}
			}
			catch (Exception e)
			{
				Utils.MsgBox(string.Format(errMsg, m_sourceInfo.Name,
					Path.GetFileName(m_sourceInfo.Queries.QueryFile), e.Message),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return false;
			}

			return true;
		}
	}
}
