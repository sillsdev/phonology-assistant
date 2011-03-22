using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SilTools;

namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public class FwDataSourceInfo
	{
		private byte[] m_lastModified;
		private FwQueries m_queries;
		private List<FwWritingSysInfo> m_writingSystems;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is needed for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo()
		{
			PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.LexemeForm;
		}

		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo(string name, string server, DataSourceType dsType) : this()
		{
			Name = name;
			Server = server;
			DataSourceType = dsType;

			if (dsType == DataSourceType.FW)
				Server = Environment.MachineName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a deep copy of the FW data source information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo Copy()
		{
			return new FwDataSourceInfo
			{
				Name = Name,
				Server = Server,
				DataSourceType = DataSourceType,
				m_lastModified = m_lastModified,
				m_writingSystems = m_writingSystems,
				IsMissing = IsMissing,
				PhoneticStorageMethod = PhoneticStorageMethod,
			};
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<FwWritingSysInfo> GetWritingSystems()
		{
			return m_writingSystems ?? (m_writingSystems = (DataSourceType == DataSourceType.FW ?
				Fw6WritingSystemReader.GetWritingSystems(this) :
				FwDBUtils.GetWritingSystemsForFw7Project(Name, Server)).ToList());
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
		[XmlAttribute("MachineName")]
		public string Server { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the FW database name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("DBName")]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public DataSourceType DataSourceType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public FwDBUtils.PhoneticStorageMethod PhoneticStorageMethod { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool IsMissing { get; set; }

		/// ------------------------------------------------------------------------------------
		private void GetQueries()
		{
			if (DataSourceType == DataSourceType.FW && m_queries == null &&
				Name != null && Server != null)
			{
				m_queries = FwQueries.GetQueriesForDB(Name, Server);
				m_lastModified = DateLastModified;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the langauge project name. If the server is null, then the path and
		/// extension are stripped off the Name property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectName
		{
			get { return (IsMultiAccessProject || DataSourceType != DataSourceType.FW7 ?
				Name : Path.GetFileNameWithoutExtension(Name)); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsMultiAccessProject
		{
			get 
			{
				return (DataSourceType == DataSourceType.FW7 &&
					!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Server));
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
				if (Queries == null || Queries.Error)
					return null;

				byte[] retVal = null;

				using (var connection = FwDBUtils.FwConnection(Name, Server))
				{
					if (connection == null)
						return null;

					var command = new SqlCommand(Queries.LastModifiedStampSQL, connection);
					using (var reader = command.ExecuteReader())
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
		/// Updates the FwDataSourcInfo's last modified date/time to reflect what's current
		/// in the database for lexical entries. The return value is a flag indicating whether
		/// or not the new value is different from the previous.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool UpdateLastModifiedTime()
		{
			if (DataSourceType == DataSourceType.FW)
			{
				byte[] newDateLastModified = DateLastModified;
				bool changed = false;

				if (m_lastModified == null || newDateLastModified == null)
					changed = true;
				else if (newDateLastModified.Where((t, i) => t != m_lastModified[i]).Any())
					changed = true;

				m_lastModified = newDateLastModified;
				return changed;
			}

			return false;
			//// We don't have a way to get the last modified time for server-based projects.
			//// TODO: Figure out how to deal with getting modifed time for server-based projects.
			//if (IsMultiAccessProject)
			//    return false;

			//var lastWriteTime = File.GetLastWriteTimeUtc(Name);
			
			//if (lastWriteTime <= m_lastModified7)
			//    return false;

			//m_lastModified7 = lastWriteTime;
			//return true;
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
				var fmt = App.LocalizeString("FieldWorksProjectMissingMsg", "FieldWorks project '{0}' is missing.");
				Utils.MsgBox(string.Format(fmt, Name));
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return ToString(false);
		}

		/// ------------------------------------------------------------------------------------
		public string ToString(bool includeServerName)
		{
			if ((DataSourceType == DataSourceType.FW7 && !IsMultiAccessProject) || Server == null)
				return ProjectName;

			if (!includeServerName || Server.ToLower() == Environment.MachineName.ToLower())
				return ProjectName;

			var fmt = App.LocalizeString("FieldWorksProjectAndServerDisplayFormat", "{0} on '{1}'",
				"This is used to display the project name and server for an FW data source. The project name is the first parameter.");

			return string.Format(fmt, ProjectName, Server);
		}
	}
}
