using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.DataSourceClasses.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public class Fw7DataSourceInfo
	{

		private DateTime m_timeLastModified;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is needed for deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Fw7DataSourceInfo()
		{
			PhoneticStorageMethod = FwDBUtils.PhoneticStorageMethod.LexemeForm;
		}

		/// ------------------------------------------------------------------------------------
		public Fw7DataSourceInfo(string name, string server)
		{
			Name = name;
			Server = server;
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Server { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool IsMissing { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public FwDBUtils.PhoneticStorageMethod PhoneticStorageMethod { get; set; }

		/// ------------------------------------------------------------------------------------
		public List<FwDataSourceWsInfo> WritingSystemInfo { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsMultiAccessProject
		{
			get { return (!string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Server)); }
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
			get { return (IsMultiAccessProject ? Path.GetFileNameWithoutExtension(Name) : Name); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the Fw7DataSourcInfo's last modified stamp to reflect what's current.
		/// The return value is a flag indicating whether or not the new value is different
		/// from the previous.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool UpdateLastModifiedStamp()
		{
			// We don't have a way to get the last modified time for server-based projects.
			// TODO: Figure out how to deal with getting modifed time for server-based projects.
			if (IsMultiAccessProject)
				return false;

			var fileInfo = new FileInfo(Name);
			if (fileInfo.LastWriteTime <= m_timeLastModified)
				return false;
			
			m_timeLastModified = fileInfo.LastWriteTime;
			return true;
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
				var fmt = App.LocalizeString("FieldWorks7orLaterProjectMissingMsg",
					"FieldWorks project '{0}' is missing.", App.kLocalizationGroupMisc);

				string msg = string.Format(fmt, ProjectName);
				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			if (!IsMultiAccessProject)
				return ProjectName;

			var fmt = App.LocalizeString("FieldWorksProjectAndServerDisplayFormat", "{0} on '{1}'",
				"This is used to display the project name and server for an FW data source. The project name is the first parameter.",
				App.kLocalizationGroupMisc);

			return string.Format(fmt, ProjectName, Server);
		}
	}
}
