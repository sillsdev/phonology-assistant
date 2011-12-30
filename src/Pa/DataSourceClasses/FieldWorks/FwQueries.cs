using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using Palaso.IO;
using SilTools;

namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public class FwQueries
	{
		internal static FwQueries s_fwLongNameQueries;
		internal static FwQueries s_fwShortNameQueries;

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
		internal FwQueries()
		{
		}

		/// ------------------------------------------------------------------------------------
		internal FwQueries(bool error)
		{
			Error = error;
		}

		static FwQueries()
		{
			ShowMsgOnFileLoadFailure = true;
		}

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
		private static bool CheckForShortNameFile(string dbName, string machineName, string filename)
		{
			// Find the file that contains the queries.
			var queryFile = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, filename);
			
			if (!File.Exists(queryFile))
			{
				var path = Utils.PrepFilePathForMsgBox(Path.GetDirectoryName(queryFile));
				var args = new[] { dbName, machineName, filename, path, filename };

				var msg = App.GetString("ShortNameFileMissingMsg",
					"FieldWorks Project: {0}\nServer: {1}\n\nThe version of this FieldWorks project " +
					"indicates it is too\nrecent to be read by Phonology Assistant. You must\ndownload " + 
					"the file '{2}' from\nthe Phonology Assistant website and copy it to the\nfollowing " +
					"folder:\n\n{3}\n\nWhen '{4}' is present in that folder,\nyour FieldWorks project " +
					"can be read by Phonology Assistant.\n\nThe Phonology Assistant website can be found " +
					"at:\nhttp://www.sil.org/computing/pa/index.htm");
				
				Utils.MsgBox(string.Format(msg, args));
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private static FwQueries Load(string filename)
		{
			// Find the file that contains the queries.
			var queryFile = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, filename);
			var fwqueries = XmlSerializationHelper.DeserializeFromFile<FwQueries>(queryFile);

			if (fwqueries != null)
				fwqueries.QueryFile = queryFile;
			else if (ShowMsgOnFileLoadFailure)
			{
				string filePath = Utils.PrepFilePathForMsgBox(queryFile);

				var msg = App.GetString("LoadingSQLQueriesErrorMsg",
					"The file that contains FieldWorks queries '{0}' is either missing or corrupt. " +
					"Until this problem is corrected, FieldWorks data sources cannot be accessed or " +
					"added as data sources.");

				App.NotifyUserOfProblem(msg, filePath);
				fwqueries = new FwQueries(true);
			}

			return fwqueries;
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool Error { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public static bool ShowMsgOnFileLoadFailure { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string QueryFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ProjectNameSQL
		{
			get { return FwDBUtils.CleanString(m_projectNameSQL); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string LastModifiedStampSQL
		{
			get { return FwDBUtils.CleanString(m_lastModifiedStampSQL); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string AnalysisWs
		{
			get { return FwDBUtils.CleanString(m_analysisWs); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string VernacularWsSQL
		{
			get { return FwDBUtils.CleanString(m_vernacularWsSQL); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string LexemeFormSQL
		{
			get { return FwDBUtils.CleanString(m_lexemeFormSQL); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string PronunciationFieldSQL
		{
			get { return FwDBUtils.CleanString(m_pronunciationFieldSQL); }
		}
	}
}
