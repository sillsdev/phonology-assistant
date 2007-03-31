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
		/// Returns an SQL string in the form: "SELECT * FROM table".
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string SelectSQL(string table)
		{
			if (table == null || table.Trim() == string.Empty)
				return string.Empty;

			return string.Format("SELECT * FROM {0}", table);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns an SQL string in the form: "SELECT * FROM table WHERE whereField
		/// = whereValue" where the whereValue is an int value.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string SelectSQL(string table, string whereField, int whereValue)
		{
			if (table == null || table.Trim() == string.Empty ||
				whereField == null || whereField.Trim() == string.Empty)
			{
				return string.Empty;
			}

			return string.Format("SELECT * FROM {0} WHERE {1} = {2}", table,
				whereField, whereValue);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns an SQL string in the form: "SELECT column FROM table WHERE whereField
		/// = whereValue" where the whereValue is an int value.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string SelectSQL(string table, string column, string whereField,
			int whereValue)
		{
			if (table == null || table.Trim() == string.Empty ||
				whereField == null || whereField.Trim() == string.Empty ||
				column == null || column.Trim() == string.Empty)
			{
				return string.Empty;
			}

			return string.Format("SELECT {0} FROM {1} WHERE {2}={3}", column, table,
				whereField, whereValue);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns an SQL string in the form: "SELECT * FROM table WHERE whereField
		/// = whereValue" where the whereValue is a boolean value.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string SelectSQL(string table, string whereField, bool whereValue)
		{
			if (table == null || table.Trim() == string.Empty ||
				whereField == null || whereField.Trim() == string.Empty)
			{
				return string.Empty;
			}

			return string.Format("SELECT * FROM {0} WHERE {1} = {2}", table,
				whereField, whereValue);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns an SQL string in the form: "SELECT * FROM table WHERE whereField
		/// = 'whereValue'" where the whereValue is a string value.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string SelectSQL(string table, string whereField, string whereValue)
		{
			if (table == null || table.Trim() == string.Empty ||
				whereField == null || whereField.Trim() == string.Empty)
			{
				return string.Empty;
			}

			return string.Format("SELECT * FROM {0} WHERE {1}='{2}'", table,
				whereField, whereValue);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns an SQL string in the form: "SELECT column FROM table WHERE whereField
		/// = 'whereValue'" where the whereValue is a string value.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static string SelectSQL(string table, string column, string whereField,
			string whereValue)
		{
			if (table == null || table.Trim() == string.Empty ||
				whereField == null || whereField.Trim() == string.Empty ||
				column == null || column.Trim() == string.Empty)
			{
				return string.Empty;
			}

			return string.Format("SELECT {0} FROM {1} WHERE {2}='{3}'", column, table,
				whereField, whereValue);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns a sql string to get CharListID's from CharIndex for a given phoneticListId.
		/// </summary>
		/// <param name="phoneticListId">phoneticListId to get CharIndex records for.</param>
		/// <returns>string</returns>
		/// --------------------------------------------------------------------------------
		public static string GetCharListIDs(int phoneticListId)
		{
			return string.Format("SELECT DISTINCT CharIndex.CharListID " +
				"FROM CharIndex WHERE CharIndex.PhoneticListID = {0};", phoneticListId);
		}

		///// --------------------------------------------------------------------------------
		///// <summary>
		///// Creates and returns the sql for selecting records.
		///// </summary>
		///// <param name="table"></param>
		///// <param name="key"></param>
		///// <param name="recordID"></param>
		///// <returns></returns>
		///// --------------------------------------------------------------------------------
		//public static string SelectByColumnSQL(string column, string table, string key, int recordID)
		//{
		//    if (column == null || column.Trim() == string.Empty
		//        || table == null || table.Trim() == string.Empty
		//        || key == null || key.Trim() == string.Empty 
		//        || recordID == 0)
		//        return string.Empty;

		//    return string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", column, table, key, recordID);
		//}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns the sql for selecting the number of records in a table for a given key.
		/// </summary>
		/// <param name="table">Table name in which you want to count records</param>
		/// <param name="recordID">Which records you want to count in the table</param>
		/// <returns>string</returns>
		/// --------------------------------------------------------------------------------
		public static string CountSQL(string table, int recId)
		{
			if (table == null || table.Trim() == string.Empty)
				return string.Empty;

			string key = DBFields.IdFromTable[table];

			return string.Format("SELECT Count({0}) AS CountOf{1} FROM {2} WHERE {3} = {4}", 
				key, key, table, key, recId);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Returns the sql for selecting the number of records in a table for a given key.
		/// </summary>
		/// <param name="table">Table name in which you want to count records</param>
		/// <param name="recordID">Which records you want to count in the table</param>
		/// <returns>string</returns>
		/// --------------------------------------------------------------------------------
		public static string CountSQL(string table, string key, int recId)
		{
			if (table == null || table.Trim() == string.Empty
				|| key == null || key.Trim() == string.Empty)
				return string.Empty;

			return string.Format("SELECT Count({0}) AS CountOf{1} FROM {2} WHERE {3} = {4}",
				key, key, table, key, recId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For the specified document id, this method retrieves the words for a particular
		/// transcription type (i.e. Phonemic, Tone, Orthographic, Gloss, Part of Speech).
		/// </summary>
		/// <param name="docId"></param>
		/// <param name="field"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static string GetFullTranscription(int docId, string field)
		{
			string sql = string.Empty;

			if (field == DBFields.Phonetic)
			{
				string sqlFmt = "SELECT Phonetic FROM PhoneticList INNER JOIN AllWordsIndex ON " +
					"PhoneticList.PhoneticListId = AllWordsIndex.PhoneticListId WHERE DocID={0} " +
					"ORDER BY AllWordsIndex.AnnOffset";

				sql = string.Format(sqlFmt, docId);
			}
			else if (field == DBFields.Reference)
			{
				string sqlFmt = SelectSQL("AllWordsIndex", DBFields.Reference,
					DBFields.DocId, docId) + " ORDER BY AnnOffset";
			}
			else
			{
				string idField = DBFields.IdFromMainField[field];
				string table = DBFields.TableFromField[field];

				string sqlFmt = "SELECT {0} FROM {1} INNER JOIN AllWordsIndex ON {2}.{3} = AllWordsIndex.{4} " +
					"WHERE AllWordsIndex.DocID={5} ORDER BY AllWordsIndex.AnnOffset";

				sql = string.Format(sqlFmt,
					new object[] {field, table, table, idField, idField, docId});
			}

			StringBuilder bldr = new StringBuilder();

			using (OleDbDataReader reader = GetSQLResultsFromDB(sql))
			{
				while (reader.Read())
				{
					bldr.Append(reader[0] as string);
					bldr.Append(' ');
				}

				reader.Close();
			}

			return bldr.ToString().Trim();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the articulatory feature mask records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static OleDbDataReader GetArticulatoryFeatures(string sortOrder)
		{
			string sqlFmt = "SELECT * FROM ArticulatoryFeatureMasks WHERE (FeatureType > -1) " +
				"ORDER BY {0}, Feature";

			return GetSQLResultsFromDB(string.Format(sqlFmt, sortOrder));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the binary feature mask records.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static OleDbDataReader GetBinaryFeatures(string sortOrder)
		{
			string sqlFmt = "SELECT * FROM BinaryFeatureMasks ORDER BY {0}, Feature";
			return GetSQLResultsFromDB(string.Format(sqlFmt, sortOrder));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the IPA characters to load into the list for the articulatory features dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static OleDbDataReader GetIPACharsForArticulatoryFeatures(string sortOrder)
		{
			string sqlFmt = "SELECT * FROM IPACharacters WHERE CharType > 0 ORDER BY {0}";
			return GetSQLResultsFromDB(string.Format(sqlFmt, sortOrder));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of find phone categories and their associated search patterns.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static OleDbDataReader GetFindPhoneCategoriesAndPatterns()
		{
			string sql = "SELECT FFCategories.*, FFPatterns.* FROM FFCategories " +
				"LEFT JOIN FFPatterns ON FFCategories.FFCategoryID = FFPatterns.FFCategoryID " +
				"ORDER BY FFCategories.Category;";

			return GetSQLResultsFromDB(sql);
		}
	}
}
