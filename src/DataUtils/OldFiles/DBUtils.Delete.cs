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
// File: DBUtils.Delete.cs
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
	/// Contains code for deleting record(s) from the database tables. 
	/// </summary>
	/// --------------------------------------------------------------------------------
	public partial class DBUtils
	{
		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Creates and returns the sql for deleting all records in a table.
		/// </summary>
		/// <param name="table">Table name</param>
		/// <returns>formated sql string</returns>
		/// --------------------------------------------------------------------------------
		public static string DeleteAllRecordsSQL(string table)
		{
			if (table == null || table.Trim() == string.Empty)
				return string.Empty;

			return string.Format("DELETE * FROM {0}", table);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Creates and returns the sql for deleting records.
		/// </summary>
		/// <param name="table">Table name</param>
		/// <param name="key">Key name</param>
		/// <param name="recId">Key value</param>
		/// <returns>formated sql string</returns>
		/// --------------------------------------------------------------------------------
		public static string DeleteRecordSQL(string table, string key, int recId)
		{
			if (table == null || table.Trim() == string.Empty
				|| key == null || key.Trim() == string.Empty || recId <= 0)
				return string.Empty;

			return string.Format("DELETE * FROM {0} WHERE {1} = {2}", table, key, recId);
		}

		/// --------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the Category, Folder or Document as specified by dbField and the
		/// record id. If the delete succeeded, then true is returned. Otherwise, false
		/// is returned.
		/// </summary>
		/// --------------------------------------------------------------------------------
		public static bool DeleteRecord(string dbField, int recId)
		{
			//if (dbField == null || dbField.Trim() == string.Empty || recId <= 0)
			//    return false;

			//string table = DBFields.TableFromField[dbField];

			//// Save the phoneticListIds for all the words in the document we're deleting
			//ArrayList phoneticListIds = new ArrayList();
			//using (OleDbDataReader reader =
			//    GetQueryResultsFromDB("qryGetPhoneticListIdsFor" + table, recId))
			//{
			//    while (reader.Read())
			//        phoneticListIds.Add((int)reader[DBFields.PhoneticListId]);
			//}

			//string sql = DeleteRecordSQL(table, dbField, recId);

			//if (DBUtils.ExecuteNonSelectSQL(sql) > 0)
			//{
			//    // Update the total count fields in the phonetic list and char list tables.
			//    foreach (int wli in phoneticListIds)
			//        UpdateTotalCounts(wli);

			//    // When deleting a document, delete the document from the cache.
			//    if (table == "Document")
			//        DocCache.Remove(recId);
			//    else
			//    {
			//        // Make sure that after deleting a folder or category, all documents
			//        // orphaned (i.e. documents no longer linked to) by deleting the
			//        // folder or category are deleted as well.
			//        DeleteUnlinkedDocuments();
			//    }

			//    return true;
			//}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete all documents to which there are no longer any any links.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void DeleteUnlinkedDocuments()
		{
			Dictionary<int, int> links = GetDocumentLinkCounts();
			foreach (KeyValuePair<int, int> docLink in links)
			{
				if (docLink.Value == 0)
					DeleteRecord(DBFields.DocId, docLink.Key);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the document link record with the specified id document id the link points
		/// to. Returns the remaining number of documents links still pointing to the
		/// specified document or -1 if the document link record failed to be deleted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static int DeleteDocumentLink(int docLinkId, int docId)
		{
			if (docLinkId <= 0)
				return -1;

			string sql = "DELETE * FROM DocumentLinks WHERE {0}={1}";
			if (ExecuteNonSelectSQL(string.Format(sql, DBFields.DocLinkId, docLinkId)) == 0)
				return -1;
		
			// Now check for any more links pointing to the specified document.
			return (int)GetScalerValueFromQuery("qryDocumentLinksCountForDocument", docId);
		}
	}
 }
