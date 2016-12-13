// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using L10NSharp;
using SIL.Reporting;

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
			var msg = LocalizationManager.GetString(
				"Miscellaneous.Messages.DataSourceReading.RetrievingFieldWorks6WritingSystemsErrorMsg",
				"There was an error retrieving writing systems from the '{0}' project. It's possible the file '{1}' is either missing or corrupt.",
				"Displayed when there is and error retrieving writing systems from a FieldWorks database, version 6.0 or earlier.");

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
					ErrorReport.NotifyUserOfProblem(errMsg, fwDsInfo.Name, Path.GetFileName(fwDsInfo.Queries.QueryFile));
			}
			catch (Exception e)
			{
				ErrorReport.NotifyUserOfProblem(e, errMsg, fwDsInfo.Name,
					Path.GetFileName(fwDsInfo.Queries.QueryFile));
			}

			return wsCollection;
		}
	}
}
