using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.DataSourceClasses.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public class Fw6DataSourceInfo
	{
		private string m_machineName;
		private string m_dbName;
		private string m_projName;
		private byte[] m_dateLastModified;
		private FwQueries m_queries;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is needed for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Fw6DataSourceInfo()
		{
			m_machineName = Environment.MachineName;
			PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.LexemeForm;
		}

		/// ------------------------------------------------------------------------------------
		public Fw6DataSourceInfo(string dbName, string machineName)
		{
			m_dbName = dbName;
			m_machineName = machineName;

			// As of the Summer 2007 release of FW, projects names are now just the DB name.
			m_projName = dbName;
		}

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
		[XmlAttribute]
		public FwDBUtils.PhoneticStorageMethod PhoneticStorageMethod { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool IsMissing { get; set; }
	
		/// ------------------------------------------------------------------------------------
		public List<FwDataSourceWsInfo> WritingSystemInfo { get; set; }

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
			get { return m_projName ?? (m_projName = DBName); }
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
			else if (newDateLastModified.Where((t, i) => t != m_dateLastModified[i]).Any())
				changed = true;

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
			return (WritingSystemInfo == null ? false:
				WritingSystemInfo.Any(ws => ws.FieldName == phoneticFieldName));
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
				var fmt = App.LocalizeString("FieldWorks6DatabaseMissingMsg",
					"FieldWorks database '{0}' is missing.", App.kLocalizationGroupMisc);

				string msg = string.Format(fmt, DBName);
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return ToString(false);
		}

		/// ------------------------------------------------------------------------------------
		public string ToString(bool includeMachineName)
		{
			if (!includeMachineName || MachineName.ToLower() == Environment.MachineName.ToLower())
				return ProjectName;

			var fmt = App.LocalizeString("FieldWorksProjectAndServerDisplayFormat", "{0} on '{1}'",
				"This is used to display the project name and server for an FW data source. The project name is the first parameter.",
				App.kLocalizationGroupMisc);

			return string.Format(fmt, ProjectName, MachineName);
		}
	}
}
