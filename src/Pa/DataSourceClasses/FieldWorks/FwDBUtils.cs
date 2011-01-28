using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Pa.DataSourceClasses.FieldWorks;
using SIL.Pa.Properties;
using SIL.PaToFdoInterfaces;
using SilTools;

namespace SIL.Pa.Model
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

		/// ------------------------------------------------------------------------------------
		public enum FwWritingSystemType
		{
			None,
			Analysis,
			Vernacular
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Methods of storing phonetic data in FLEx.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public enum PhoneticStorageMethod
		{
			LexemeForm,
			PronunciationField
		}

		public static bool ShowMsgWhenGatheringFWInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		internal static string CleanString(string str)
		{
			str = str.Replace('\n', ' ');
			str = str.Replace('\n', ' ');
			str = str.Replace('\t', ' ');
			return str.Trim();
		}

		#region Methods for FW6 and older database formats.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of FW databases on the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Fw6DataSourceInfo[] GetFwDataSourceInfoList(string machineName)
		{
			return GetFwDataSourceInfoList(machineName, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of FW databases on the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Fw6DataSourceInfo[] GetFwDataSourceInfoList(string machineName,
			bool showErrorOnFailure)
		{
			// Make sure SQL server is running.
			if (!IsSQLServerStarted)
				return null;

			s_showErrorOnConnectionFailure = showErrorOnFailure;
			var fwDBInfoList = new List<Fw6DataSourceInfo>();

			SmallFadingWnd msgWnd = null;
			if (ShowMsgWhenGatheringFWInfo)
				msgWnd = new SmallFadingWnd(Properties.Resources.kstidGettingFwProjInfoMsg);

			// Read all the SQL databases from the server's master table.
			using (SqlConnection connection = FwConnection(FwDBAccessInfo.MasterDB, machineName))
			{
				if (connection != null && FwDBAccessInfo.FwDatabasesSQL != null)
				{
					SqlCommand command = new SqlCommand(FwDBAccessInfo.FwDatabasesSQL, connection);

					// Get all the database names.
					using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
					{
						while (reader.Read() && !string.IsNullOrEmpty(reader[0] as string))
							fwDBInfoList.Add(new Fw6DataSourceInfo(reader[0] as string, machineName));

						reader.Close();
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
						new[] { server, dbName, "FWDeveloper", "careful" });

					SqlConnection connection = new SqlConnection(connectionStr);
					connection.Open();
					return connection;
				}
			}
			catch (Exception e)
			{
				if (s_showErrorOnConnectionFailure)
				{
					var msg = App.LocalizeString("SQLServerNotInstalledMsg",
						"The following error occurred when trying to establish\na connection to the {0} database on the machine '{1}'.\n\n{2}",
						App.kLocalizationGroupMisc);
					
					msg = string.Format(msg, dbName, machineName, e.Message);
					Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		public static bool IsSQLServerInstalled(bool showMsg)
		{
			if (ServiceController.GetServices().Any(svc => svc.ServiceName.ToLower() == FwDBAccessInfo.Service.ToLower()))
				return true;

			if (showMsg)
			{
				var msg = App.LocalizeString("SQLServerNotInstalledMsg",
					"Access to FieldWorks projects requires SQL Server but it is not installed on this computer.",
					App.kLocalizationGroupMisc);

				Utils.MsgBox(msg);
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

						var startingSQLMsg = App.LocalizeString("StartingSQLServerMsg",
							"Starting SQL Server...", App.kLocalizationGroupMisc);

						using (var msgWnd = new SmallFadingWnd(startingSQLMsg))
						{
							msgWnd.Show();
							Application.DoEvents();

							// Start the server instance and wait 15 seconds for it to finish starting.
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
						Utils.MsgBox(string.Format(Properties.Resources.kstidErrorStartingSQLServer1, msg));
						return false;
					}

					msg = string.Format(Properties.Resources.kstidErrorStartingSQLServer2,
						FwDBAccessInfo.SecsToWaitForDBEngineStartup);

					if (Utils.MsgBox(msg, MessageBoxButtons.YesNo,
						MessageBoxIcon.Question) != DialogResult.Yes)
					{
						return false;
					}
				}
			}
		}

		#endregion

		#region Methods for FW7 and newer database formats.
		/// ------------------------------------------------------------------------------------
		public static bool IsFw7Installed
		{
			get { return PaFieldWorksHelper.IsFwLoaded; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Displays a dialog box from which a user may choose an FW7 (or later) project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool GetFw7Project(Form frm, out string name, out string server)
		{
			var rc = Settings.Default.Fw7OpenDialogBounds;
			int splitPos = Settings.Default.Fw7OpenDialogSplitterPos;
			using (var helper = new PaFieldWorksHelper())
			{
				var retVal = helper.ShowFwOpenProject(frm, ref rc, ref splitPos, out name, out server);
				Settings.Default.Fw7OpenDialogBounds = rc;
				Settings.Default.Fw7OpenDialogSplitterPos = splitPos;
				return retVal;
			}
		}

		#endregion
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
				s_accessInfoFile = Path.Combine(App.ConfigFolder, "FwDBAccessInfo.xml");
				s_dbAccessInfo = XmlSerializationHelper.DeserializeFromFile<FwDBAccessInfo>(s_accessInfoFile);
			}

			if (s_dbAccessInfo == null && s_showMsgOnFileLoadFailure)
			{
				string filePath = Utils.PrepFilePathForMsgBox(s_accessInfoFile);
				Utils.MsgBox(string.Format(Properties.Resources.kstidErrorLoadingDBAccessInfoMsg, filePath));
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

		private readonly bool m_error;
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
							s_fwShortNameQueries =
								CheckForShortNameFile(dbName, machineName, "FwSQLQueriesShortNames.xml") ?
								Load("FwSQLQueriesShortNames.xml") : new FwQueries(true);
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
			string queryFile = Path.Combine(App.ConfigFolder, filename);

			if (!File.Exists(queryFile))
			{
				string path = Utils.PrepFilePathForMsgBox(App.ConfigFolder);
				string[] args = new[] {dbName, machineName, filename, path, filename};
				string msg = string.Format(Properties.Resources.kstidShortNameFileMissingMsg, args);
				Utils.MsgBox(msg, MessageBoxButtons.OK);
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
			var queryFile = Path.Combine(App.ConfigFolder, filename);
			var fwqueries = XmlSerializationHelper.DeserializeFromFile<FwQueries>(queryFile);

			if (fwqueries != null)
				fwqueries.m_queryFile = queryFile;
			else if (s_showMsgOnFileLoadFailure)
			{
				string filePath = Utils.PrepFilePathForMsgBox(queryFile);
				Utils.MsgBox(string.Format(Properties.Resources.kstidErrorLoadingQueriesMsg, filePath));
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
