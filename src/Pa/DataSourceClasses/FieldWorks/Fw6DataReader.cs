using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.DataSourceClasses.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public class Fw6DataReader
	{
		public delegate void DataRetrievedHandler(SqlDataReader reader);

		private readonly Fw6DataSourceInfo m_sourceInfo;

		/// ------------------------------------------------------------------------------------
		public Fw6DataReader(Fw6DataSourceInfo sourceInfo)
		{
			m_sourceInfo = sourceInfo;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the vernacular writing systems in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Dictionary<int, string> VernacularWritingSystems
		{
			get
			{
				var msg = App.LocalizeString("ErrorRetrievingFwVernacularWritingSystemsMsg",
					"There was an error retrieving vernacular writing systems from the {0}\ndatabase. It's possible the file {1} is either missing or corrupt.\n\n{2}",
					App.kLocalizationGroupMisc);

				return GetWritingSystems(m_sourceInfo.Queries.VernacularWsSQL, msg);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the vernacular writing systems in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Dictionary<int, string> AnalysisWritingSystems
		{
			get
			{
				var msg = App.LocalizeString("ErrorRetrievingFwAnalysisWritingSystemsMsg",
					"There was an error retrieving vernacular writing systems from the {0}\ndatabase. It's possible the file {1} is either missing or corrupt.\n\n{2}",
					App.kLocalizationGroupMisc);

				return GetWritingSystems(m_sourceInfo.Queries.AnalysisWs, msg);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of all analysis and vernacular writing systems in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<FwWritingSysInfo> AllWritingSystems
		{
			get
			{
				var wsInfoList = AnalysisWritingSystems
					.Select(ws => new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Analysis, ws.Key, ws.Value)).ToList();

				// Build a list of the analysis writing systems.
				wsInfoList.AddRange(VernacularWritingSystems
					.Select(ws => new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Vernacular, ws.Key, ws.Value)));

				return wsInfoList;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems returned by the SQL statement found in the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Dictionary<int, string> GetWritingSystems(string sql, string errMsg)
		{
			Dictionary<int, string> wsCollection = new Dictionary<int, string>();

			try
			{
				using (SqlConnection connection =
					FwDBUtils.FwConnection(m_sourceInfo.DBName, m_sourceInfo.MachineName))
				{
					if (connection != null && !string.IsNullOrEmpty(sql))
					{
						SqlCommand command = new SqlCommand(sql, connection);
						using (SqlDataReader reader = command.ExecuteReader())
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
					Utils.MsgBox(string.Format(errMsg, m_sourceInfo.DBName,
						Path.GetFileName(m_sourceInfo.Queries.QueryFile), string.Empty),
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception e)
			{
				Utils.MsgBox(string.Format(errMsg, m_sourceInfo.DBName,
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

			if (m_sourceInfo.WritingSystemInfo == null || m_sourceInfo.WritingSystemInfo.Count == 0)
			{
				errMsg = string.Format(Properties.Resources.kstidMissingWsMsg,
					m_sourceInfo.ProjectName);

				Utils.MsgBox(errMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}

			foreach (FwDataSourceWsInfo dswsi in m_sourceInfo.WritingSystemInfo)
			{
				string replace = string.Format("${0}Ws$", dswsi.FieldName);
				sql = sql.Replace(replace, dswsi.Ws.ToString());
			}

			try
			{
				using (SqlConnection connection =
					FwDBUtils.FwConnection(m_sourceInfo.DBName, m_sourceInfo.MachineName))
				{
					SqlCommand command = new SqlCommand(sql, connection);
					using (SqlDataReader reader = command.ExecuteReader())
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
				Utils.MsgBox(string.Format(errMsg, m_sourceInfo.DBName,
					Path.GetFileName(m_sourceInfo.Queries.QueryFile), e.Message),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return false;
			}

			return true;
		}
	}
}
