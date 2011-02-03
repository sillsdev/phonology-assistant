using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
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
			string queryFile = Path.Combine(App.ConfigFolder, filename);

			if (!File.Exists(queryFile))
			{
				string path = Utils.PrepFilePathForMsgBox(App.ConfigFolder);
				string[] args = new[] { dbName, machineName, filename, path, filename };
				string msg = string.Format(Properties.Resources.kstidShortNameFileMissingMsg, args);
				Utils.MsgBox(msg, MessageBoxButtons.OK);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private static FwQueries Load(string filename)
		{
			// Find the file that contains the queries.
			var queryFile = Path.Combine(App.ConfigFolder, filename);
			var fwqueries = XmlSerializationHelper.DeserializeFromFile<FwQueries>(queryFile);

			if (fwqueries != null)
				fwqueries.QueryFile = queryFile;
			else if (ShowMsgOnFileLoadFailure)
			{
				string filePath = Utils.PrepFilePathForMsgBox(queryFile);
				Utils.MsgBox(string.Format(Properties.Resources.kstidErrorLoadingQueriesMsg, filePath));
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
