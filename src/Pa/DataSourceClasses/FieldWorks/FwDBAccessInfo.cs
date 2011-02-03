using System.IO;
using System.Xml.Serialization;
using SilTools;

namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public class FwDBAccessInfo
	{
		internal static FwDBAccessInfo s_dbAccessInfo;
		private static string s_accessInfoFile;

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
		static FwDBAccessInfo()
		{
			ShowMsgOnFileLoadFailure = true;
		}

		/// ------------------------------------------------------------------------------------
		internal static void Load()
		{
			if (s_dbAccessInfo == null)
			{
				// Find the file that contains information about connecting to an FW database.
				s_accessInfoFile = Path.Combine(App.ConfigFolder, "FwDBAccessInfo.xml");
				s_dbAccessInfo = XmlSerializationHelper.DeserializeFromFile<FwDBAccessInfo>(s_accessInfoFile);
			}

			if (s_dbAccessInfo == null && ShowMsgOnFileLoadFailure)
			{
				string filePath = Utils.PrepFilePathForMsgBox(s_accessInfoFile);
				Utils.MsgBox(string.Format(Properties.Resources.kstidErrorLoadingDBAccessInfoMsg, filePath));
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public static bool ShowMsgOnFileLoadFailure { get; set; }

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
		public static string DBAccessInfoFile
		{
			get
			{
				Load();
				return s_accessInfoFile;
			}
		}

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
		public static string RootDataDirValue
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_rootDataDirValue : null);
			}
		}

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
		public static string Service
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_service : "MSSQL$SILFW");
			}
		}

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
		public static int SecsToWaitForDBEngineStartup
		{
			get
			{
				Load();
				return (s_dbAccessInfo != null ? s_dbAccessInfo.m_secsToWaitForDBEngineStartup : 25);
			}
		}

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
}
