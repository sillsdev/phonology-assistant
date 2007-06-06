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
		public enum FwWritingSystemType
		{
			None,
			Analysis,
			Vernacular
		}

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

		private static int s_secondsToWaitForSQLToStart = 20;

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
		public static FwDataSourceInfo[] FwDataSourceInfoList
		{
			get
			{
				// Make sure SQL server is running.
				if (!IsSQLServerStarted)
					return null;
				
				List<FwDataSourceInfo> fwDBInfoList = new List<FwDataSourceInfo>();

				SQLServerMessageWnd msgWnd =
					new SQLServerMessageWnd(Properties.Resources.kstidGettingFwProjInfoMsg);

				// Read all the SQL databases from the server's master table.
				using (SqlConnection connection = FwConnection("master"))
				{
					if (connection != null && FwQueries.FwDatabasesSQL != null)
					{
						SqlCommand command = new SqlCommand(FwQueries.FwDatabasesSQL, connection);
						if (command != null)
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader != null)
								{
									while (reader.Read() && !string.IsNullOrEmpty(reader[0] as string))
										fwDBInfoList.Add(new FwDataSourceInfo(reader[0] as string));

									reader.Close();
								}
							}

							connection.Close();
						}
					}
				}

				msgWnd.CloseFade();
				msgWnd.Dispose();

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
		/// Checks if SQL server is started.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsSQLServerStarted
		{
			get
			{
				try
				{
					using (ServiceController svcController = new ServiceController("MSSQL$SILFW"))
						return (svcController.Status == ServiceControllerStatus.Running);
				}
				catch { }

				return false;
			}
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

					using (SQLServerMessageWnd msgWnd = new SQLServerMessageWnd())
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

						msgWnd.CloseFade();
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

		public FwDBUtils.PhoneticStorageMethod PhoneticStorageMethod =
			FwDBUtils.PhoneticStorageMethod.LexemeForm;

		private string m_dbName;
		private string m_projName;
		public bool IsMissing = false;
		private byte[] m_dateLastModified;

		public List<FwDataSourceWsInfo> WritingSystemInfo;

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
			// When the project name is the same as the DB name,
			// then just return the project name.
			if (ProjectName.ToLower() == m_dbName.ToLower())
				return ProjectName;

			// When they're different, return the project name,
			// followed by the DB name in parentheses.
			string text = Properties.Resources.kstidFWDataSourceInfo;
			return string.Format(text, ProjectName, m_dbName);
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
						m_dateLastModified = DateLastModified;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the langauge project name for the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectName
		{
			get
			{
				if (IsMissing)
					return string.Format(Properties.Resources.kstidFwDBMissing, DBName);

				if (m_projName == null)
				{
					using (SqlConnection connection = FwDBUtils.FwConnection(DBName))
					{
						if (connection == null)
							return "Error retrieving project name";

						SqlCommand command = new SqlCommand(FwQueries.ProjectNameSQL, connection);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.HasRows)
							{
								reader.Read();
								m_projName = reader[0] as string;
							}

							reader.Close();
						}

						connection.Close();
					}
				}

				return m_projName;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Looks into the database to determine when the last lexical entry was modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private byte[] DateLastModified
		{
			get
			{
				byte[] retVal = null;

				using (SqlConnection connection = FwDBUtils.FwConnection(DBName))
				{
					if (connection == null)
						return null;

					SqlCommand command = new SqlCommand(FwQueries.LastModifiedStampSQL, connection);
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
			byte[] newDateLastModified = DateLastModified;
			bool changed = false;

			if (m_dateLastModified == null || newDateLastModified == null)
				changed = true;
			else
			{
				// Loop throught the two byte arrays, comparing each byte.
				for (int i = 0; i < newDateLastModified.Length; i++)
				{
					if (newDateLastModified[i] != m_dateLastModified[i])
					{
						changed = true;
						break;
					}
				}
			}

			m_dateLastModified = newDateLastModified;
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
				return (/* PhoneticWs > 0 && PhonemicWs > 0 && OrthographicWs > 0 &&
					EnglishGlossWs > 0 && */ !string.IsNullOrEmpty(m_dbName) &&
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

	#region FwDataSourceWsInfo
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Serialized with an FwDataSourceInfo class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("FieldWsInfo")]
	public class FwDataSourceWsInfo
	{
		[XmlAttribute]
		public string FieldName;
		[XmlAttribute]
		public int Ws;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceWsInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceWsInfo(string fieldname, int ws)
		{
			FieldName = fieldname;
			Ws = ws;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a deep copy of the FwDataSourceWsInfo object and returns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceWsInfo Clone()
		{
			FwDataSourceWsInfo clone = new FwDataSourceWsInfo();
			clone.FieldName = FieldName;
			clone.Ws = Ws;
			return clone;
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
			get
			{
				return GetWritingSystems(FwQueries.VernacularWsSQL,
					Properties.Resources.kstidErrorRetrievingVernWsMsg);
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
				return GetWritingSystems(FwQueries.AnalysisWs,
					Properties.Resources.kstidErrorRetrievingAnalWsMsg);
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
				List<FwWritingSysInfo> wsInfoList = new List<FwWritingSysInfo>();
			
				// Add the analysis writing systems.
				foreach (KeyValuePair<int, string> ws in AnalysisWritingSystems)
				{
					wsInfoList.Add(new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Analysis,
						ws.Key, ws.Value));
				}

				// Build a list of the analysis writing systems.
				foreach (KeyValuePair<int, string> ws in VernacularWritingSystems)
				{
					wsInfoList.Add(new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Vernacular,
						ws.Key, ws.Value));
				}
				
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
				using (SqlConnection connection = FwDBUtils.FwConnection(m_sourceInfo.DBName))
				{
					if (connection != null && !string.IsNullOrEmpty(sql))
					{
						SqlCommand command = new SqlCommand(sql, connection);
						if (command != null)
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader != null)
								{
									while (reader.Read())
										wsCollection[(int)reader["Obj"]] = reader["Txt"] as string;

									reader.Close();
								}
							}
						}

						connection.Close();
					}
				}

				// There should be at least one writing system defined.
				if (wsCollection.Count == 0)
				{
					STUtils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
						Path.GetFileName(FwQueries.QueryFile), string.Empty),
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception e)
			{
				STUtils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
					Path.GetFileName(FwQueries.QueryFile), e.Message),
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
			string errMsg = Properties.Resources.kstidErrorRetrievingFwDataMsg;
			string sql =
				(m_sourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm ?
				FwQueries.LexemeFormSQL : FwQueries.PronunciationFieldSQL);

			if (m_sourceInfo.WritingSystemInfo == null || m_sourceInfo.WritingSystemInfo.Count == 0)
			{
				errMsg = string.Format(Properties.Resources.kstidMissingWsMsg,
					m_sourceInfo.ProjectName);

				STUtils.STMsgBox(errMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}

			foreach (FwDataSourceWsInfo dswsi in m_sourceInfo.WritingSystemInfo)
			{
				string replace = string.Format("${0}Ws$", dswsi.FieldName);
				sql = sql.Replace(replace, dswsi.Ws.ToString());
			}
			
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
					Path.GetFileName(FwQueries.QueryFile), e.Message),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return false;
			}

			return true;
		}
	}

	#endregion

	#region FwQueries class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwQueries
	{
		private static FwQueries s_fwqueries;
		private static string s_queryFile;

		[XmlElement("mastertable")]
		public string m_masterTable;

		[XmlElement("databases")]
		public string m_fwDatabasesSQL;

		[XmlElement("projectname")]
		public string m_projectNameSQL;

		[XmlElement("lastmodifiedstamp")]
		public string m_lastModifiedStampSQL;

		[XmlElement("analysiswritingsystems")]
		public string m_analysisWs;

		[XmlElement("veracularwritingsystems")]
		public string m_vernacularWsSQL;

		[XmlElement("datafromlexemeform")]
		public string m_lexemeFormSQL;

		[XmlElement("datafrompronunciationfield")]
		public string m_pronunciationFieldSQL;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void Load()
		{
			if (s_fwqueries == null)
			{
				// Find the file that contains the queries.
				s_queryFile = Path.GetDirectoryName(Application.ExecutablePath);
				s_queryFile = Path.Combine(s_queryFile, "FwSQLQueries.xml");
				s_fwqueries = STUtils.DeserializeData(s_queryFile, typeof(FwQueries)) as FwQueries;
			}

			if (s_fwqueries == null)
			{
				string msg = string.Format(
					Properties.Resources.kstidErrorLoadingQueriesMsg, s_queryFile);

				STUtils.STMsgBox(msg, MessageBoxButtons.OK);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string QueryFile
		{
			get
			{
				Load();
				return s_queryFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FwDatabasesSQL
		{
			get
			{
				Load();
				return (s_fwqueries != null ? s_fwqueries.m_fwDatabasesSQL : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ProjectNameSQL
		{
			get
			{
				Load();
				return (s_fwqueries != null ? s_fwqueries.m_projectNameSQL : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string LastModifiedStampSQL
		{
			get
			{
				Load();
				return (s_fwqueries != null ? s_fwqueries.m_lastModifiedStampSQL : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string AnalysisWs
		{
			get
			{
				Load();
				return (s_fwqueries != null ? s_fwqueries.m_analysisWs : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string VernacularWsSQL
		{
			get
			{
				Load();
				return (s_fwqueries != null ? s_fwqueries.m_vernacularWsSQL : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string LexemeFormSQL
		{
			get
			{
				Load();
				return (s_fwqueries != null ? s_fwqueries.m_lexemeFormSQL : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string PronunciationFieldSQL
		{
			get
			{
				Load();
				return (s_fwqueries != null ? s_fwqueries.m_pronunciationFieldSQL : null);
			}
		}
	}

	#endregion

	#region FwWritingSysInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a single item in the writing system drop-downs in the grid.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwWritingSysInfo
	{
		public string WsName;
		public int WsNumber;
		public FwDBUtils.FwWritingSystemType WsType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwWritingSysInfo(FwDBUtils.FwWritingSystemType wsType, int wsNumber,
			string wsName)
		{
			WsType = wsType;
			WsNumber = wsNumber;
			WsName = wsName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return WsName;
		}
	}

	#endregion
}
