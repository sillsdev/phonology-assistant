// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: DBUtils.Select.cs
// Responsibility: ToddJ
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace SIL.SpeechTools.Database
{
	/// --------------------------------------------------------------------------------
	/// <summary>
	/// Contains code for selecting record(s) from the database tables. 
	/// </summary>
	/// --------------------------------------------------------------------------------
	public partial class DBUtils
	{
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Updates the PhoneticList and CharList TotalCounts for all records.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static void UpdateTotalCounts()
		{
			ExecuteNonSelectQuery("qryUpdateAllPhoneticListTotalCounts");
			ExecuteNonSelectQuery("qryUpdateAllCharListTotalCounts");
		}
		
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Updates the PhoneticList and CharList TotalCounts for the specified
		/// phonetic list id.
		/// </summary>
		/// <param name="phoneticListId">Record key to update TotalCount for.</param>
		/// <returns>Number of rows affected</returns>
		/// --------------------------------------------------------------------------------
		public static int UpdateTotalCounts(int phoneticListId)
		{
			if (phoneticListId == 0)
				return 0;

			string sql = string.Format("UPDATE PhoneticList SET PhoneticList.TotalCount = " +
				"DCount('*','AllWordsIndex','AllWordsIndex.PhoneticListID = {0}') " +
				"WHERE PhoneticList.PhoneticListID = {1};", phoneticListId, phoneticListId);

			int rowsAffected = ExecuteNonSelectSQL(sql);

			UpdateCharListTotalCountForPhoneticListID(phoneticListId);

			return rowsAffected;
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Updates the CharList TotalCount.
		/// </summary>
		/// <param name="charListID">Record key to update TotalCount for.</param>
		/// <returns>Number of rows affected</returns>
		/// --------------------------------------------------------------------------------
		public static int UpdateCharListTotalCount(int charListID)
		{
			if (charListID == 0)
				return 0;

			string sql = string.Format("UPDATE CharList SET CharList.TotalCount = " +
				"DCount('*','CharIndex','CharIndex.CharListID = {0}') " +
				"WHERE CharList.CharListID = {1};", charListID, charListID);

			return ExecuteNonSelectSQL(sql);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Updates the CharList TotalCount.
		/// </summary>
		/// <param name="phoneticListId">Record key to get the CharListID's for.</param>
		/// <returns>True if successful</returns>
		/// --------------------------------------------------------------------------------
		public static bool UpdateCharListTotalCountForPhoneticListID(int phoneticListId)
		{
			if (phoneticListId == 0)
				return false;

			// Get the CharListID's for the given phoneticListID and save them in an array.
			ArrayList charListIdArray = new ArrayList();
			string sql = GetCharListIDs(phoneticListId);

			using (OleDbDataReader reader = GetSQLResultsFromDB(sql))
			{
				while (reader.Read())
					charListIdArray.Add((int)reader[DBFields.CharListId]);

				reader.Close();
			}

			foreach (int charListId in charListIdArray)
				UpdateCharListTotalCount(charListId);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the default feature names in the articulatory feature masks table and
		/// the default character masks (i.e. Mask0 and Mask1) in the IPACharacters table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void RestoreDefaultArticulatoryFeatures()
		{
			ExecuteNonSelectSQL("UPDATE ArticulatoryFeatureMasks SET Feature = [DefaultFeature]");

			string sql = "UPDATE IPACharacters SET Mask0 = [DefaultMask0], " +
				"Mask1 = [DefaultMask1]";
			
			ExecuteNonSelectSQL(sql);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Restores the default feature names in the binary feature masks table and
		/// the default character binary mask (i.e. BinaryMask) in the IPACharacters table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void RestoreDefaultBinaryFeatures()
		{
			ExecuteNonSelectSQL("UPDATE BinaryFeatureMasks SET Feature = [DefaultFeature]");
			ExecuteNonSelectSQL("UPDATE IPACharacters SET BinaryMask = [DefaultBinaryMask]");
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static int InsertFilter(Filter selectedFilter)
		{
			if (selectedFilter == null)
				return 0;

			StringBuilder folderIds = new StringBuilder();
			StringBuilder dialects = new StringBuilder();
			string folder = string.Empty;

			// Reformat the folderIds string
			foreach (int folderId in selectedFilter.Folders)
			{
				folder = folderId.ToString();
				folderIds.Append(folder);
				folderIds.Append(",");
			}

			if (folderIds.Length > 1)
				folderIds.Remove((folderIds.Length - 1), 1);

			// Reformat the dialects string
			foreach (string dialect in selectedFilter.Dialects)
			{
				dialects.Append(dialect);
				dialects.Append(",");
			}

			if (dialects.Length > 1)
				dialects.Remove((dialects.Length - 1), 1);

			string sqlFmt = "INSERT INTO Filter (FilterName, FolderIDs, Dialects)" +
				"VALUES ('{0}','{1}','{2}');";

			return DBUtils.ExecuteNonSelectSQL
				(string.Format(sqlFmt, selectedFilter.Name, folderIds, dialects));
		}
	}
}