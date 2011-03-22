using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.DataSource.FieldWorks
{
	public class Fw6WritingSystemReader
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of all analysis and vernacular writing systems in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<FwWritingSysInfo> GetWritingSystems(FwDataSourceInfo fwDsInfo)
		{
			var msg = App.GetString("RetrievingFieldWorks6WritingSystemsErrorMsg",
				"There was an error retrieving writing systems from the {0}\nproject. It's possible the file {1} is either missing or corrupt.",
				"Displayed when there is and error retrieving writing systems from a FieldWorks database, version 6.0 or earlier.");

			msg = string.Format(msg, fwDsInfo.Name, Path.GetFileName(fwDsInfo.Queries.QueryFile));

			foreach (var ws in GetWritingSystemsUsingQuery(fwDsInfo.Queries.AnalysisWs, fwDsInfo, msg))
			{
				yield return new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Analysis,
					ws.Key, ws.Value) { Id = ws.Key.ToString() };
			}

			// Build a list of the analysis writing systems.
			foreach (var ws in GetWritingSystemsUsingQuery(fwDsInfo.Queries.VernacularWsSQL, fwDsInfo, msg))
			{
				yield return new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.Vernacular,
					ws.Key, ws.Value) { Id = ws.Key.ToString() };
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems returned by the SQL statement found in the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static Dictionary<int, string> GetWritingSystemsUsingQuery(string sql,
			FwDataSourceInfo fwDsInfo, string errMsg)
		{
			var wsCollection = new Dictionary<int, string>();

			try
			{
				using (var connection = FwDBUtils.FwConnection(fwDsInfo.Name, fwDsInfo.Server))
				{
					if (connection != null && !string.IsNullOrEmpty(sql))
					{
						var command = new SqlCommand(sql, connection);
						using (var reader = command.ExecuteReader())
						{
							while (reader.Read())
								wsCollection[(int)reader["Obj"]] = reader["Txt"] as string;

							reader.Close();
						}

						connection.Close();
					}
				}

				// There should be at least one writing system defined.
				if (wsCollection.Count == 0)
				{
					Utils.MsgBox(string.Format(errMsg, fwDsInfo.Name,
						Path.GetFileName(fwDsInfo.Queries.QueryFile), string.Empty),
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception e)
			{
				Utils.MsgBox(string.Format(errMsg, fwDsInfo.Name,
					Path.GetFileName(fwDsInfo.Queries.QueryFile), e.Message),
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return wsCollection;
		}
	}
}
