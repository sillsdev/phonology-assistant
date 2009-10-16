using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Pa.Data.Properties;
using SilUtils;

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
		private static bool s_showErrorOnConnectionFailure = true;

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

		public static bool ShowMsgWhenGatheringFWInfo = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of FW databases on the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FwDataSourceInfo[] GetFwDataSourceInfoList(string machineName)
		{
			return GetFwDataSourceInfoList(machineName, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of FW databases on the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FwDataSourceInfo[] GetFwDataSourceInfoList(string machineName,
			bool showErrorOnFailure)
		{
			// Make sure SQL server is running.
			if (!IsSQLServerStarted)
				return null;

			s_showErrorOnConnectionFailure = showErrorOnFailure;
			List<FwDataSourceInfo> fwDBInfoList = new List<FwDataSourceInfo>();

			SmallFadingWnd msgWnd = null;
			if (ShowMsgWhenGatheringFWInfo)
				msgWnd = new SmallFadingWnd(Resources.kstidGettingFwProjInfoMsg);

			// Read all the SQL databases from the server's master table.
			using (SqlConnection connection = FwConnection(FwDBAccessInfo.MasterDB, machineName))
			{
				if (connection != null && FwDBAccessInfo.FwDatabasesSQL != null)
				{
					SqlCommand command = new SqlCommand(FwDBAccessInfo.FwDatabasesSQL, connection);

					// Get all the database names.
					using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
					{
						if (reader != null)
						{
							while (reader.Read() && !string.IsNullOrEmpty(reader[0] as string))
								fwDBInfoList.Add(new FwDataSourceInfo(reader[0] as string, machineName));

							reader.Close();
						}
					}

					connection.Close();
				}
			}

			if (msgWnd != null)
			{
				msgWnd.CloseFade();
				msgWnd.Dispose();
			}

			s_showErrorOnConnectionFailure = true;
			return (fwDBInfoList.Count == 0 ? null : fwDBInfoList.ToArray());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Opens a connection to an FW SQL server database on the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static SqlConnection FwConnection(string dbName, string machineName)
		{
			try
			{
				if (!string.IsNullOrEmpty(machineName) && StartSQLServer(true))
				{
					string server = FwDBAccessInfo.GetServer(machineName);
					string connectionStr = string.Format(FwDBAccessInfo.ConnectionString,
						new string[] { server, dbName, "FWDeveloper", "careful" });

					SqlConnection connection = new SqlConnection(connectionStr);
					connection.Open();
					return connection;
				}
			}
			catch (Exception e)
			{
				if (s_showErrorOnConnectionFailure)
				{
					string msg = string.Format(Resources.kstidSQLConnectionErrMsg,
						dbName, machineName, e.Message);


					SilUtils.Utils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsSQLServerInstalled(bool showMsg)
		{
			foreach (ServiceController service in ServiceController.GetServices())
			{
				if (service.ServiceName.ToLower() == FwDBAccessInfo.Service.ToLower())
					return true;
			}

			if (showMsg)
			{
				SilUtils.Utils.STMsgBox(Resources.kstidSQLServerNotInstalledMsg,
					MessageBoxButtons.OK);
			}

			return false;
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
				if (!IsSQLServerInstalled(false))
					return false;
				
				try
				{
					using (ServiceController svcController = new ServiceController(FwDBAccessInfo.Service))
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
			if (!IsSQLServerInstalled(showErrMessages))
				return false;

			string msg = null;

			while (true)
			{
				try
				{
					using (ServiceController svcController = new ServiceController(FwDBAccessInfo.Service))
					{
						// If the server instance is already running, we're good.
						if (svcController.Status == ServiceControllerStatus.Running)
							return true;

						using (SmallFadingWnd msgWnd =
							new SmallFadingWnd(Resources.kstidStartingSQLServerMsg))
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
								new TimeSpan(FwDBAccessInfo.SecsToWaitForDBEngineStartup * (long)10000000));

							msgWnd.CloseFade();
						}

						if (svcController.Status == ServiceControllerStatus.Running)
							return true;
					}
				}
				catch (Exception e)
				{
					msg = e.Message;
				}

				if (showErrMessages)
				{
					// Check if we've timed out.
					if (msg != null && msg.ToLower().IndexOf("time out") < 0)
					{
						SilUtils.Utils.STMsgBox(string.Format(Resources.kstidErrorStartingSQLServer1, msg));
						return false;
					}

					msg = string.Format(Resources.kstidErrorStartingSQLServer2,
						FwDBAccessInfo.SecsToWaitForDBEngineStartup);

					if (SilUtils.Utils.STMsgBox(msg, MessageBoxButtons.YesNo,
						MessageBoxIcon.Question) != DialogResult.Yes)
					{
						return false;
					}
				}
			}
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static string CleanString(string str)
		{
			str = str.Replace('\n', ' ');
			str = str.Replace('\n', ' ');
			str = str.Replace('\t', ' ');
			return str.Trim();
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
		public FwDBUtils.PhoneticStorageMethod PhoneticStorageMethod =
			FwDBUtils.PhoneticStorageMethod.LexemeForm;

		private string m_machineName;
		private string m_dbName;
		private string m_projName;
		public bool IsMissing = false;
		private byte[] m_dateLastModified = null;
		private FwQueries m_queries;

		public List<FwDataSourceWsInfo> WritingSystemInfo;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is needed for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo()
		{
			m_machineName = Environment.MachineName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo(string dbName, string machineName)
		{
			m_dbName = dbName;
			m_machineName = machineName;

			// As of the Summer 2007 release of FW, projects names are now just the DB name.
			m_projName = dbName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return ToString(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ToString(bool includeMachineName)
		{
			string text = ProjectName;

			// When the project name is the same as the DB name,
			// then just return the project name.
			if (ProjectName.ToLower() != m_dbName.ToLower())
			{
				// When they're different, return the project name,
				// followed by the DB name in parentheses.
				text = string.Format(Resources.kstidFwDataSourceInfo, ProjectName, m_dbName);
			}

			return (!includeMachineName ||
				MachineName.ToLower() == Environment.MachineName.ToLower() ? text :
				string.Format(Resources.kstidFwDataSourceInfoWithMachineName, text, MachineName));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public FwQueries Queries
		{
			get
			{
				if (m_queries == null)
					GetQueries();

				return m_queries;
			}
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string MachineName
		{
			get { return m_machineName; }
			set
			{
				m_machineName = value;
				GetQueries();
			}
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
					GetQueries();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetQueries()
		{
			if (m_queries == null && m_dbName != null && m_machineName != null)
			{
				m_queries = FwQueries.GetQueriesForDB(m_dbName, m_machineName);
				m_dateLastModified = DateLastModified;
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
				if (m_projName == null)
					m_projName = DBName;

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
				if (m_queries == null || m_queries.Error)
					return null;

				byte[] retVal = null;

				using (SqlConnection connection = FwDBUtils.FwConnection(DBName, MachineName))
				{
					if (connection == null)
						return null;

					SqlCommand command = new SqlCommand(Queries.LastModifiedStampSQL, connection);
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
		/// Gets a value indicating whether or not, at least, the phonetic writing system
		/// was specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool HasWritingSystemInfo(string phoneticFieldName)
		{
			if (WritingSystemInfo != null)
			{
				foreach (FwDataSourceWsInfo wsInfo in WritingSystemInfo)
				{
					if (wsInfo.FieldName == phoneticFieldName)
						return true;
				}
			}

			return false;
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
				string msg = string.Format(Resources.kstidFwDBMissing, DBName);
				SilUtils.Utils.STMsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

		private readonly FwDataSourceInfo m_sourceInfo;

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
				return GetWritingSystems(m_sourceInfo.Queries.VernacularWsSQL,
					Resources.kstidErrorRetrievingVernWsMsg);
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
				return GetWritingSystems(m_sourceInfo.Queries.AnalysisWs,
					Resources.kstidErrorRetrievingAnalWsMsg);
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
				using (SqlConnection connection =
					FwDBUtils.FwConnection(m_sourceInfo.DBName, m_sourceInfo.MachineName))
				{
					if (connection != null && !string.IsNullOrEmpty(sql))
					{
						SqlCommand command = new SqlCommand(sql, connection);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader != null)
							{
								while (reader.Read())
									wsCollection[(int) reader["Obj"]] = reader["Txt"] as string;

								reader.Close();
							}
						}

						connection.Close();
					}
				}

				// There should be at least one writing system defined.
				if (wsCollection.Count == 0)
				{
					SilUtils.Utils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
						Path.GetFileName(m_sourceInfo.Queries.QueryFile), string.Empty),
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception e)
			{
				SilUtils.Utils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
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
			
			string errMsg = Resources.kstidErrorRetrievingFwDataMsg;
			string sql =
				(m_sourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm ?
				m_sourceInfo.Queries.LexemeFormSQL : m_sourceInfo.Queries.PronunciationFieldSQL);

			if (m_sourceInfo.WritingSystemInfo == null || m_sourceInfo.WritingSystemInfo.Count == 0)
			{
				errMsg = string.Format(Resources.kstidMissingWsMsg,
					m_sourceInfo.ProjectName);

				SilUtils.Utils.STMsgBox(errMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
				SilUtils.Utils.STMsgBox(string.Format(errMsg, m_sourceInfo.DBName,
					Path.GetFileName(m_sourceInfo.Queries.QueryFile), e.Message),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return false;
			}

			return true;
		}
	}

	#endregion

	#region FwDBAccessInfo class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwDBAccessInfo
	{
		internal static FwDBAccessInfo s_dbAccessInfo;
		private static string s_accessInfoFile;
		private static bool s_showMsgOnFileLoadFailure = true;

		[XmlElement("longnamecheck")]
		public string m_longNameCheckSQL;

		[XmlElement("fwregkey")]
		public string m_fwRegKey;

		[XmlElement("rootdatadirvalue")]
		public string m_rootDataDirValue;

		[XmlElement("jumpurl")]
		public string m_jumpUrl;

		[XmlElement("service")]
		public string m_service;

		[XmlElement("secondstowaitfordbenginestartup")]
		public int m_secsToWaitForDBEngineStartup = 25;

		[XmlElement("connectionstring")]
		public string m_connectionString;

		[XmlElement("server")]
		public string m_server;

		[XmlElement("masterdatabase")]
		public string m_masterdb;

		[XmlElement("databases")]
		public string m_fwDatabasesSQL;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static void Load()
		{
			if (s_dbAccessInfo == null)
			{
				// Find the file that contains information about connecting to an FW database.
				s_accessInfoFile = Path.GetDirectoryName(Application.ExecutablePath);
				s_accessInfoFile = Path.Combine(s_accessInfoFile, "FwDBAccessInfo.xml");
				s_dbAccessInfo =
					SilUtils.Utils.DeserializeData(s_accessInfoFile, typeof(FwDBAccessInfo)) as FwDBAccessInfo;
			}

			if (s_dbAccessInfo == null && s_showMsgOnFileLoadFailure)
			{
				string filePath = SilUtils.Utils.PrepFilePathForSTMsgBox(s_accessInfoFile);
				SilUtils.Utils.STMsgBox(string.Format(Resources.kstidErrorLoadingDBAccessInfoMsg, filePath));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public static bool ShowMsgOnFileLoadFailure
		{
			get { return s_showMsgOnFileLoadFailure; }
			set { s_showMsgOnFileLoadFailure = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ServerFormatString
		{
			get
			{
				Load();
				return (s_dbAccessInfo == null || string.IsNullOrEmpty(s_dbAccessInfo.m_server) ?
					@"{0}\SILFW" : s_dbAccessInfo.m_server);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string LongNameCheckSQL
		{
			get
			{
				Load();
				return (s_dbAccessInfo == null ?
					null : FwDBUtils.CleanString(s_dbAccessInfo.m_longNameCheckSQL));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string DBAccessInfoFile
		{
			get
			{
				Load();
				return s_accessInfoFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FwRegKey
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_fwRegKey : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string RootDataDirValue
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_rootDataDirValue : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string JumpUrl
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_jumpUrl : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string Service
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_service : "MSSQL$SILFW");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string ConnectionString
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ?
					FwDBUtils.CleanString(s_dbAccessInfo.m_connectionString) : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's the FW server for the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetServer(string machineName)
		{
			Load();
			return string.Format(ServerFormatString, machineName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int SecsToWaitForDBEngineStartup
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_secsToWaitForDBEngineStartup : 25);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MasterDB
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_masterdb : null);
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
				return (s_dbAccessInfo != null ?
					FwDBUtils.CleanString(s_dbAccessInfo.m_fwDatabasesSQL) : null);
			}
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
		internal static FwQueries s_fwLongNameQueries;
		internal static FwQueries s_fwShortNameQueries;
		private static bool s_showMsgOnFileLoadFailure = true;

		private bool m_error = false;
		private string m_queryFile;

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
		internal FwQueries()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal FwQueries(bool error)
		{
			m_error = error;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static FwQueries GetQueriesForDB(string dbName, string machineName)
		{
			string longNameCheckSQL = FwDBAccessInfo.LongNameCheckSQL;

			if (string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(longNameCheckSQL))
				return null;

			FwQueries fwqueries = null;

			using (SqlConnection connection = FwDBUtils.FwConnection(dbName, machineName))
			{
				if (connection != null)
				{
					try
					{
						SqlCommand command = new SqlCommand(longNameCheckSQL, connection);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							reader.Close();

							if (s_fwLongNameQueries == null)
								s_fwLongNameQueries = Load("FwSQLQueries.xml");

							fwqueries = s_fwLongNameQueries;
						}
					}
					catch
					{
						if (s_fwShortNameQueries == null)
						{
							if (CheckForShortNameFile(dbName, machineName, "FwSQLQueriesShortNames.xml"))
								s_fwShortNameQueries = Load("FwSQLQueriesShortNames.xml");
							else
								s_fwShortNameQueries = new FwQueries(true);
						}

						fwqueries = s_fwShortNameQueries;
					}

					connection.Close();
				}
			}

			return fwqueries;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool CheckForShortNameFile(string dbName, string machineName, string filename)
		{
			// Find the file that contains the queries.
			string queryFile = Path.Combine(Application.StartupPath, filename);

			if (!System.IO.File.Exists(queryFile))
			{
				string path = SilUtils.Utils.PrepFilePathForSTMsgBox(Application.StartupPath);
				string[] args = new string[] {dbName, machineName, filename, path, filename};
				string msg = string.Format(Properties.Resources.kstidShortNameFileMissingMsg, args);
				SilUtils.Utils.STMsgBox(msg, MessageBoxButtons.OK);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static FwQueries Load(string filename)
		{
			// Find the file that contains the queries.
			string queryFile = Path.GetDirectoryName(Application.ExecutablePath);
			queryFile = Path.Combine(queryFile, filename);
			FwQueries fwqueries = SilUtils.Utils.DeserializeData(queryFile, typeof(FwQueries)) as FwQueries;

			if (fwqueries != null)
				fwqueries.m_queryFile = queryFile;
			else if (s_showMsgOnFileLoadFailure)
			{
				string filePath = SilUtils.Utils.PrepFilePathForSTMsgBox(queryFile);
				SilUtils.Utils.STMsgBox(string.Format(Resources.kstidErrorLoadingQueriesMsg, filePath));
				fwqueries = new FwQueries(true);
			}

			return fwqueries;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool Error
		{
			get { return m_error; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public static bool ShowMsgOnFileLoadFailure
		{
			get { return s_showMsgOnFileLoadFailure; }
			set { s_showMsgOnFileLoadFailure = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string QueryFile
		{
			get	{return m_queryFile;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectNameSQL
		{
			get	{return FwDBUtils.CleanString(m_projectNameSQL);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string LastModifiedStampSQL
		{
			get {return FwDBUtils.CleanString(m_lastModifiedStampSQL);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string AnalysisWs
		{
			get {return FwDBUtils.CleanString(m_analysisWs);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string VernacularWsSQL
		{
			get	{return FwDBUtils.CleanString(m_vernacularWsSQL);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string LexemeFormSQL
		{
			get	{return FwDBUtils.CleanString(m_lexemeFormSQL);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string PronunciationFieldSQL
		{
			get	{return FwDBUtils.CleanString(m_pronunciationFieldSQL);}
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
