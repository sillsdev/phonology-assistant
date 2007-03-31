using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace SIL.SpeechTools.Database
{
	public class Filter
	{
		private int m_id;
		private string m_name;
		private ArrayList m_folderIds = new ArrayList();
		private ArrayList m_dialects = new ArrayList();

		#region Construction, setup and loading from DB.
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create and load the filters from the database.
		/// </summary>
		/// <returns>ArrayList of the filters</returns>
		/// ------------------------------------------------------------------------------------
		public static ArrayList Load()
		{
			string sql = DBUtils.SelectSQL("Filter");

			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				ArrayList filterList = new ArrayList();

				while (reader.Read())
				{
					Filter filter = new Filter();
					filter.m_id = (int)reader[DBFields.FilterId];
					filter.m_name = (string)reader[DBFields.FilterName];

					string folders = reader[DBFields.FilterFolders] as string;
					if ((folders != null) && (folders != ""))
					{
						string[] parsedFolders =
							folders.Split(",".ToCharArray());

						foreach (string folderId in parsedFolders)
							filter.m_folderIds.Add(Int32.Parse(folderId));
					}

					string dialects = reader[DBFields.FilterDialects] as string;
					if ((dialects != null) && (dialects != ""))
					{
						string[] parsedDialects =
							dialects.Split(",".ToCharArray());

						foreach (string dialectName in parsedDialects)
							filter.m_dialects.Add(dialectName);
					}

					filterList.Add(filter);
				}

				reader.Close();

				return filterList;
			}
		}

		#endregion

		#region Database operations.

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save filter.
		/// </summary>
		/// <param name=""></param>
		/// <returns>bool</returns>
		/// ------------------------------------------------------------------------------------
		public int Save()
		{
			return DBUtils.InsertFilter(this);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete a filter.
		/// </summary>
		/// <param name="filter">Filter</param>
		/// <returns>bool</returns>
		/// ------------------------------------------------------------------------------------
		public static int DeleteAllFilters()
		{
			string sql = DBUtils.DeleteAllRecordsSQL(DBFields.Filter);

			return DBUtils.ExecuteNonSelectSQL(sql);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Insert a new filter.
		/// </summary>
		/// <param name="name">string</param>
		/// <returns>Filter</returns>
		/// ------------------------------------------------------------------------------------
		//public static Filter NamedFilter()
		//{
		//    Filter filter = new Filter();
		//    filter.Name = DBUtils.GetUniqueTitle(DBFields.FilterName, "Filter");

		//    return filter;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get a filter based on an id.
		/// </summary>
		/// <param name="filterId">Id of the Filter to return</param>
		/// <returns>Filter</returns>
		/// ------------------------------------------------------------------------------------
		public static Filter GetFilter(int filterId)
		{	
			string sql = DBUtils.SelectSQL("Filter", DBFields.FilterId, filterId);
			
			Filter filter = new Filter();
			
			using (OleDbDataReader reader = DBUtils.GetSQLResultsFromDB(sql))
			{
				while (reader.Read())
				{
					filter.Id = (int)reader[DBFields.FilterId];
					filter.Name = (string)reader[DBFields.FilterName];
				}
			}

			return filter;
		}

		#endregion

		#region Properties

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get and set the filter's id.
		/// </summary>
		/// <returns>int</returns>
		/// ------------------------------------------------------------------------------------
		public int Id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get and set the filter's name.
		/// </summary>
		/// <returns>string</returns>
		/// ------------------------------------------------------------------------------------
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the filter's folders.
		/// </summary>
		/// <returns>ArrayList of Folder Ids</returns>
		/// ------------------------------------------------------------------------------------
		public ArrayList Folders
		{
			get { return m_folderIds; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get the filter's dialects.
		/// </summary>
		/// <returns>ArrayList of dialect names</returns>
		/// ------------------------------------------------------------------------------------
		public ArrayList Dialects
		{
			get { return m_dialects; }
		}

		#endregion

		#region Overrides

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the filter's name.
		/// </summary>
		/// <returns>Filter name string</returns>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_name;
		}

		#endregion
	}
}