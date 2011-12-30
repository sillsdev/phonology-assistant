using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using Microsoft.Win32;
using Palaso.Reporting;
using SIL.Pa.Properties;
using SIL.PaToFdoInterfaces;
using SilTools;

namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Misc. static methods for accessing SQL server and getting Fieldworks information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FwDBUtils
	{
		#region enumerations
		/// ------------------------------------------------------------------------------------
		public enum FwWritingSystemType
		{
			None,
			Analysis,
			Vernacular,
			CmPossibility
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Methods of storing phonetic data in FLEx.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public enum PhoneticStorageMethod
		{
			LexemeForm,
			PronunciationField,
			AllPronunciationFields
		}

		#endregion

		private static string s_fwRootDataDir;
		private static bool s_showErrorOnConnectionFailure = true;
		public static bool ShowMsgWhenGatheringFWInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		internal static string CleanString(string str)
		{
			str = str.Replace('\n', ' ');
			str = str.Replace('\n', ' ');
			str = str.Replace('\t', ' ');
			return str.Trim();
		}

		/// ------------------------------------------------------------------------------------
		public static string FwRootDataDir
		{
			get { return s_fwRootDataDir ?? (s_fwRootDataDir = GetFwRootDataDir()); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the location where FW tucks away data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetFwRootDataDir()
		{
			string key = FwDBAccessInfo.FwRegKey;
			if (String.IsNullOrEmpty(key))
				return null;

			using (var regKey = Registry.LocalMachine.OpenSubKey(key))
			{
				if (regKey != null)
					return regKey.GetValue(FwDBAccessInfo.RootDataDirValue, null) as string;
			}

			return null;
		}

		#region Methods for FW6 and older database formats.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of FW databases on the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FwDataSourceInfo> GetFwDataSourceInfoList(string machineName)
		{
			return GetFwDataSourceInfoList(machineName, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of FW databases on the specified machine.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FwDataSourceInfo> GetFwDataSourceInfoList(string server,
			bool showErrorOnFailure)
		{
			// Make sure SQL server is running.
			if (!IsSQLServerStarted)
				return null;

			s_showErrorOnConnectionFailure = showErrorOnFailure;
			var fwDBInfoList = new List<FwDataSourceInfo>();

			SmallFadingWnd msgWnd = null;
			if (ShowMsgWhenGatheringFWInfo)
				msgWnd = new SmallFadingWnd(App.GetString("GatheringFw6ProjectInfoMsg", "Gathering FieldWorks Project Information..."));

			try
			{
				// Read all the SQL databases from the server's master table.
				using (var connection = FwConnection(FwDBAccessInfo.MasterDB, server))
				{
					if (connection != null && FwDBAccessInfo.FwDatabasesSQL != null)
					{
						var command = new SqlCommand(FwDBAccessInfo.FwDatabasesSQL, connection);

						// Get all the database names.
						using (var reader = command.ExecuteReader(CommandBehavior.SingleResult))
						{
							while (reader.Read() && !String.IsNullOrEmpty(reader[0] as string))
								fwDBInfoList.Add(new FwDataSourceInfo(reader[0] as string, server, DataSourceType.FW));

							reader.Close();
						}

						connection.Close();
					}
				}
			}
			catch (Exception e)
			{
				if (s_showErrorOnConnectionFailure)
				{
					ErrorReport.NotifyUserOfProblem(e, App.GetString("GettingFwProjectErrorMsg",
						"An error occurred while trying to get a list of FieldWorks projects."));
				}

				fwDBInfoList = null;
			}

			if (msgWnd != null)
			{
				msgWnd.CloseFade();
				msgWnd.Dispose();
			}

			s_showErrorOnConnectionFailure = true;
			return fwDBInfoList;
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
				if (!String.IsNullOrEmpty(machineName) && StartSQLServer(true))
				{
					string server = FwDBAccessInfo.GetServer(machineName);
					string connectionStr = String.Format(FwDBAccessInfo.ConnectionString,
						new[] { server, dbName, "FWDeveloper", "careful" });

					var connection = new SqlConnection(connectionStr);
					connection.Open();
					return connection;
				}
			}
			catch (Exception e)
			{
				if (!s_showErrorOnConnectionFailure)
				{
					ErrorReport.NotifyUserOfProblem(e, App.GetString("SQLServerNotInstalledMsg",
						"An error occurred when trying to establish a connection to the '{0}' " +
						"database on the machine '{1}'."), dbName, machineName);
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
				ErrorReport.NotifyUserOfProblem(App.GetString("SQLServerNotInstalledMsg",
					"Access to FieldWorks projects requires SQL Server but it is not installed on this computer."));
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
					using (var svcController = new ServiceController(FwDBAccessInfo.Service))
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

			Exception error = null;

			while (true)
			{
				try
				{
					using (var svcController = new ServiceController(FwDBAccessInfo.Service))
					{
						// If the server instance is already running, we're good.
						if (svcController.Status == ServiceControllerStatus.Running)
							return true;

						var startingSQLMsg = App.GetString("StartingSQLServerMsg", "Starting SQL Server...");

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
					if (!showErrMessages)
						continue;
					
					error = e;
				}

				if (!showErrMessages || error == null)
					continue;

				// Check if we've timed out.
				if (error.Message.ToLower().IndexOf("time out") < 0)
				{
					ErrorReport.NotifyUserOfProblem(error, App.GetString("ErrorStartingSQLServer1",
						"SQL Server cannot be started. It may not be installed. Make sure " +
						"FieldWorks Language Explorer has been installed or restart Phonology " +
						"Assistant to try again."));

					return false;
				}

				var	msg = App.GetString("ErrorStartingSQLServer2",
						"Phonology Assistant waited {0} seconds for SQL Server to fully start up." +
						"\nEither that is not enough time for your computer or it may not be installed." +
						"\nMake sure FieldWorks Language Explorer has been installed. Would you\nlike to try again?");
						
				msg = String.Format(msg, FwDBAccessInfo.SecsToWaitForDBEngineStartup);

				if (Utils.MsgBox(msg, MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) != DialogResult.Yes)
				{
					return false;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems from the specified FW7 project on the specified server.
		/// When the project is not a db40 project, then server is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FwWritingSysInfo> GetWritingSystemsForFw7Project(FwDataSourceInfo dsInfo)
		{
			return GetWritingSystemsForFw7Project(dsInfo.Name, dsInfo.Server);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems from the specified FW7 project on the specified server.
		/// When the project is not a db40 project, then server is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FwWritingSysInfo> GetWritingSystemsForFw7Project(string name, string server)
		{
			using (var helper = new PaFieldWorksHelper())
			{
				if (helper.LoadOnlyWritingSystems(name, server,
					Settings.Default.Fw7TimeToWaitForProcessStart, Settings.Default.Fw7TimeToWaitForDataLoad))
				{
					return helper.WritingSystems.Select(ws => new FwWritingSysInfo(ws.IsVernacular ?
						FwWritingSystemType.Vernacular : FwWritingSystemType.Analysis, ws.Id, ws.DisplayName)
						{
							DefaultFontName = ws.DefaultFontName,
							IsDefaultAnalysis = ws.IsDefaultAnalysis,
							IsDefaultVernacular = ws.IsDefaultVernacular
						}).ToList();
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the assumed writing system for phonetic fields in an FW7 project. An attempt
		/// is first made to find a vernacular writing system that contains "ipa". If that
		/// fails, then another attempt is made to find one containing "phonetic". If that
		/// fails, then the default vernacular writing system is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static FwWritingSysInfo GetDefaultPhoneticWritingSystem(IEnumerable<FwWritingSysInfo> writingSystems)
		{
			var wsList = writingSystems.ToArray();

			var ws = wsList.Where(w => w.Type == FwWritingSystemType.Vernacular)
				.FirstOrDefault(w => w.Name.ToLower().Contains("ipa"));

			if (ws != null)
				return ws;

			ws = wsList.Where(w => w.Type == FwWritingSystemType.Vernacular)
				.FirstOrDefault(w => w.Name.ToLower().Contains("phonetic"));

			return (ws ?? wsList.Single(w => w.IsDefaultVernacular));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the lexical entries from the project and server specified in the data source
		/// information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<IPaLexEntry> GetLexEntriesFromFw7Project(FwDataSourceInfo dsInfo)
		{
			using (var helper = new PaFieldWorksHelper())
			{
				if (helper.Initialize(dsInfo.Name, dsInfo.Server,
					Settings.Default.Fw7TimeToWaitForProcessStart, Settings.Default.Fw7TimeToWaitForDataLoad))
				{
					return helper.LexEntries.ToList();
				}
			}

			return null;
		}

		#endregion
	}
}
