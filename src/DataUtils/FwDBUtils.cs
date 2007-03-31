using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Data
{
	#region FwDBUtils class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Misc. static methods for accessing SQL server and getting Fieldworks information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwDBUtils
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Methods of storing phonetic data in FieldWorks Language Explorer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public enum PhoneticStorageMethod
		{
			LexemeForm,
			PronunciationField
		}

		private static int s_secondsToWaitForSQLToStart = 15;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the number of seconds to wait for SQL server to start if it's not
		/// already running.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int SecondsToWaitForSQLToStart
		{
			get { return s_secondsToWaitForSQLToStart; }
			set { s_secondsToWaitForSQLToStart = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FwDataSourceInfo[] FwDatabaseInfoList
		{
			get
			{
				// Make sure SQL server is running.
				if (!StartSQLServer(false))
					return null;
				
				List<FwDataSourceInfo> fwDBInfoList = new List<FwDataSourceInfo>();

				// Read all the SQL databases from the server's master table.
				using (SqlConnection connection = FwConnection("master"))
				{
					if (connection != null)
					{
						string sql = @"exec sp_GetFWDBs";
						SqlCommand command = new SqlCommand(sql, connection);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read() && !string.IsNullOrEmpty(reader[0] as string))
								fwDBInfoList.Add(new FwDataSourceInfo(reader[0] as string));

							reader.Close();
						}

						connection.Close();
					}
				}

				return (fwDBInfoList.Count == 0 ? null : fwDBInfoList.ToArray());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the FieldWorks SQL server name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FwServer
		{
			get { return string.Format(@"{0}\SILFW", Environment.MachineName); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Opens a connection to a SQL server database on the local machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static SqlConnection FwConnection(string dbName)
		{
			try
			{
				string connectionFmt = @"server={0}; Database={1}; User ID=FWDeveloper;" +
					@"Password=careful; Connect Timeout = 2; Pooling=false;";

				string connectionStr = string.Format(connectionFmt, FwServer, dbName);
				SqlConnection connection = new SqlConnection(connectionStr);
				connection.Open();
				return connection;
			}
			catch
			{
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Starts SQL Server (MSDE) or makes sure it's started.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool StartSQLServer(bool showErrMessages)
		{
			string msg;

			try
			{
				using (ServiceController svcController = new ServiceController("MSSQL$SILFW"))
				{
					// If the server instance is already running, we're good.
					if (svcController.Status == ServiceControllerStatus.Running)
						return true;

					using (StartingSQLServerWnd msgWnd = new StartingSQLServerWnd())
					{
						msgWnd.Show();
						Application.DoEvents();
						
						// Start the server instance and wait 15
						// seconds for it to finish starting.
						if (svcController.Status == ServiceControllerStatus.Paused)
							svcController.Continue();
						else
							svcController.Start();

						svcController.WaitForStatus(ServiceControllerStatus.Running,
							new TimeSpan((long)s_secondsToWaitForSQLToStart * (long)10000000));
					}

					if (svcController.Status == ServiceControllerStatus.Running)
						return true;
				}
			}
			catch (Exception e)
			{
				if (showErrMessages)
				{
					msg = Properties.Resources.kstidErrorStartingMSDE1;
					STUtils.STMsgBox(string.Format(msg, e.Message), MessageBoxButtons.OK);
				}
				
				return false;
			}

			if (showErrMessages)
			{
				msg = Properties.Resources.kstidErrorStartingMSDE2;
				STUtils.STMsgBox(msg, MessageBoxButtons.OK);
			}
			
			return false;
		}
	}

	#endregion

	#region FwDataSourceInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwDataSourceInfo
	{
		[XmlAttribute]
		public string Server;
		private string m_dbName;
		public int PhoneticWs;
		public int PhonemicWs;
		public int OrthographicWs;
		public int EnglishGlossWs;
		public int NationalGlossWs;
		public FwDBUtils.PhoneticStorageMethod PhoneticStorageMethod =
			FwDBUtils.PhoneticStorageMethod.LexemeForm;

		private string m_langProjName;
		public bool IsMissing = false;
		private byte[] m_lastModifiedStamp;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is needed for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo(string dbName)
		{
			DBName = dbName;
			Server = FwDBUtils.FwServer;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			string text = Properties.Resources.kstidFWDataSourceInfo;
			return string.Format(text, LangProjName, m_dbName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the FW database name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string DBName
		{
			get { return m_dbName; }
			set
			{
				if (m_dbName != value)
				{
					m_dbName = value;
					if (m_dbName != null)
						m_lastModifiedStamp = LastModifiedStamp;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the langauge project name for the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string LangProjName
		{
			get
			{
				if (IsMissing)
					return string.Format(Properties.Resources.kstidFwDBMissing, DBName);

				if (m_langProjName == null)
				{
					using (SqlConnection connection = FwDBUtils.FwConnection(DBName))
					{
						if (connection == null)
							return "Error retrieving project name";

						string sql = @"select TOP 1 txt from CmProject_Name (readuncommitted)";
						SqlCommand command = new SqlCommand(sql, connection);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.HasRows)
							{
								reader.Read();
								m_langProjName = reader[0] as string;
							}

							reader.Close();
						}

						connection.Close();
					}
				}

				return m_langProjName;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Looks into the database to determine when the last lexical entry was modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private byte[] LastModifiedStamp
		{
			get
			{
				byte[] retVal = null;

				using (SqlConnection connection = FwDBUtils.FwConnection(DBName))
				{
					string sql = "select top 1 Updstmp from CmObject " +
						"where class$ in (5002, 5016) order by Updstmp desc";

					if (connection == null)
						return null;

					SqlCommand command = new SqlCommand(sql, connection);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.HasRows)
						{
							reader.Read();
							retVal = reader[0] as byte[];
						}

						reader.Close();
					}

					connection.Close();
				}

				return retVal;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the FwDataSourcInfo's last modified stamp to reflect what's current in the
		/// database for lexical entries. The return value is a flag indicating whether or not
		/// the new value is different from the previous.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool UpdateLastModifiedStamp()
		{
			byte[] newModifiedStamp = LastModifiedStamp;
			bool changed = false;

			if (m_lastModifiedStamp == null && newModifiedStamp != null)
				changed = true;
			else
			{
				// Loop throught the two byte arrays, comparing each byte.
				for (int i = 0; i < newModifiedStamp.Length && m_lastModifiedStamp != null; i++)
				{
					if (newModifiedStamp[i] != m_lastModifiedStamp[i])
					{
						changed = true;
						break;
					}
				}
			}

			m_lastModifiedStamp = newModifiedStamp;
			return changed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all the information for the data source is
		/// complete.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsInfoComplete
		{
			get
			{
				return (PhoneticWs > 0 && PhonemicWs > 0 && OrthographicWs > 0 &&
					EnglishGlossWs > 0 && !string.IsNullOrEmpty(m_dbName) &&
					!string.IsNullOrEmpty(Server));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows a message indicating the database is missing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ShowMissingMessage()
		{
			if (IsMissing)
			{
				string msg = string.Format(Properties.Resources.kstidFwDBMissing, DBName);
				STUtils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}

	#endregion

	#region FwDataReader class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwDataReader
	{
		public delegate void DataRetrievedHandler(SqlDataReader reader);

		private FwDataSourceInfo m_sourceInfo;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataReader(FwDataSourceInfo sourceInfo)
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
			get {return GetWritingSystems("FwVernacularWs.sql", true);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the vernacular writing systems in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Dictionary<int, string> AnalysisWritingSystems
		{
			get { return GetWritingSystems("FwAnalysisWs.sql", false); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems returned by the SQL statement found in the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Dictionary<int, string> GetWritingSystems(string sqlFile, bool forVernacularWs)
		{
			string errMsg = (forVernacularWs ?
				Properties.Resources.kstidErrorRetrievingVernWsMsg :
				Properties.Resources.kstidErrorRetrievingAnalWsMsg);

			// Find the file that contains the query used to get the writing systems.
			string path = Path.GetDirectoryName(Application.ExecutablePath);
			sqlFile = Path.Combine(path, sqlFile);
			if (!File.Exists(sqlFile))
			{
				STUtils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
					Path.GetFileName(sqlFile), string.Empty),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				
				return null;
			}

			Dictionary<int, string> wsCollection = new Dictionary<int, string>();

			// Suck in all the contents of the file, which should be valid SQL.
			string sql = File.ReadAllText(sqlFile);
			try
			{
				using (SqlConnection connection = FwDBUtils.FwConnection(m_sourceInfo.DBName))
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

				// There should be at least one writing system defined.
				if (wsCollection.Count == 0)
				{
					STUtils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
						Path.GetFileName(sqlFile), string.Empty),
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception e)
			{
				STUtils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
					Path.GetFileName(sqlFile), e.Message), MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}

			return (wsCollection.Count == 0 ? null : wsCollection);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the data from the MSDE database. Returns false if reading failed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetData(DataRetrievedHandler dataRetrievedHdlr)
		{
			string errMsg = Properties.Resources.kstidErrorRetrievingFwDataMsg;

			// Find the file that contains the query used to get the writing systems.
			string path = Path.GetDirectoryName(Application.ExecutablePath);
			
			string sqlFile = Path.Combine(path,
				(m_sourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm ?
				"FwData-LexemeForm.sql" : "FwData-PronunciationField.sql"));
			
			if (!File.Exists(sqlFile))
			{
				STUtils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
					Path.GetFileName(sqlFile), string.Empty),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return false;
			}

			// Suck in all the contents of the file, which should be valid SQL and replace
			// parameters with real writing system numbers.
			string sql = File.ReadAllText(sqlFile);
			sql = sql.Replace("$PhoneticWs", m_sourceInfo.PhoneticWs.ToString());
			sql = sql.Replace("$PhonemicWs", m_sourceInfo.PhonemicWs.ToString());
			sql = sql.Replace("$OrthographicWs", m_sourceInfo.OrthographicWs.ToString());
			sql = sql.Replace("$EnglishGlossWs", m_sourceInfo.EnglishGlossWs.ToString());
			sql = sql.Replace("$NationalGlossWs", m_sourceInfo.NationalGlossWs.ToString());
			
			try
			{
				using (SqlConnection connection = FwDBUtils.FwConnection(m_sourceInfo.DBName))
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
				STUtils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
					Path.GetFileName(sqlFile), e.Message), MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);

				return false;
			}

			return true;
		}
	}

	#endregion
}
