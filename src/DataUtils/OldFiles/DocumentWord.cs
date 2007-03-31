using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using SIL.SpeechTools.Utils;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class containing information about a single set of transcription words for a document.
	/// This class will contain information about one phonetic word and, if there are any, the
	/// other words (i.e. tone, phonemic, gloss, orthographic, part of speech, and reference),
	/// associated with that phonetic word.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DocumentWord
	{
		#region Private Cosntants
		// Format string for a Delete SQL statement for the AllWordsIndex table.
		private const string kAWIDeleteCmd = "DELETE * FROM AllWordsIndex " + 
			"WHERE AllWordIndexId={0}";

		// Format string for an Insert SQL statement for the AllWordsIndex table.
		private const string kAWIInsertCmd = "INSERT INTO AllWordsIndex (DocId, " +
			"PhonemicListId, ToneListId, OrthoListId, GlossListId, POSListId," +
			"PhoneticListId, AnnOffset, AnnLength, WavOffset, WavLength, Reference) " +
			"VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},'{11}')";

		#endregion

		#region Member data
		private bool m_isDirty = false;
		internal int m_docId;
		internal int m_allWordIndexId = 0;
		internal int m_phoneticListId = 0;

		/// <summary>Offset (in characters) where word begins in the transcription.</summary>
		internal int m_annotationOffset = 0;
		/// <summary>Offset (in bytes) of the word starting from the beginning of the wave file.</summary>
		internal int m_wavOffset = 0;
		/// <summary>Length (in bytes) of the word in the wave file.</summary>
		internal int m_wavLength = 0;
		
		/// <summary>
		/// Used to store phonetic character information for the characters in the phonetic
		/// word. This is used by the WaveDocumentWriter.
		/// </summary>
		internal PhoneticWordInfo m_eticWordInfo = null;
		
		private Dictionary<string, string> m_words = new Dictionary<string, string>();

		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="docId"></param>
		/// ------------------------------------------------------------------------------------
		public DocumentWord(int docId)
		{
			m_docId = docId;
		}

		#endregion

		#region Static method for loading a collection of words from a document id
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and initializes an array of DocumentWord objects from the database, using
		/// the specified document id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SortedDictionary<int, DocumentWord> Load(int docId)
		{
			SortedDictionary<int, DocumentWord> allWords = new SortedDictionary<int, DocumentWord>();

			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentWords", docId))
			{
				while (reader.Read())
				{
					DocumentWord word = new DocumentWord(docId);

					word.m_allWordIndexId = (int)reader[DBFields.AllWordIndexId];
					word.m_annotationOffset = (int)reader[DBFields.AnnOffset];
					word.m_phoneticListId = (int)reader[DBFields.PhoneticListId];
					word[DBFields.Phonetic] = reader[DBFields.Phonetic] as string;
					word[DBFields.Phonemic] = reader[DBFields.Phonemic] as string;
					word[DBFields.Tone] = reader[DBFields.Tone] as string;
					word[DBFields.Ortho] = reader[DBFields.Ortho] as string;
					word[DBFields.Gloss] = reader[DBFields.Gloss] as string;
					word[DBFields.POS] = reader[DBFields.POS] as string;
					word[DBFields.Reference] = reader[DBFields.Reference] as string;
					word[DBFields.CVPattern] = reader[DBFields.CVPattern] as string;

					word.m_wavOffset = (reader[DBFields.WavOffset] is DBNull ? -1 :
						(int)reader[DBFields.WavOffset]);

					word.m_wavLength = (reader[DBFields.WavLength] is DBNull ? -1 :
						(int)reader[DBFields.WavLength]);

					word.IsDirty = false;

					allWords[word.m_annotationOffset] = word;
				}

				reader.Close();
				return (allWords.Count == 0 ? null : allWords);
			}
		}

		#endregion

		#region Methods for committing changes to the Database
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Commits to the database any changes to the words.
		/// </summary>
		/// <returns>Zero if no changes required committing. Otherwise the AllWordIndexId of
		/// the AllWordsIndex record added or modfied.</returns>
		/// ------------------------------------------------------------------------------------
		public int Commit(PhoneticWriter phoneticWriter)
		{
			// Don't bother doing anything if there weren't any changes and the words
			// aren't new (i.e. m_allWordIndexId > 0).
			if (!IsDirty && m_allWordIndexId > 0)
				return 0;

			// If we're just updating existing words, remove all the AllWordsIndex records
			// before updating. We'll just re-add the record with updated values.
			if (m_allWordIndexId > 0)
				DBUtils.ExecuteNonSelectSQL(string.Format(kAWIDeleteCmd, m_allWordIndexId));

			int emicId = DBUtils.UpdateListTable(DBFields.Phonemic, Phonemic);
			int toneId = DBUtils.UpdateListTable(DBFields.Tone, Tone);
			int orthoId = DBUtils.UpdateListTable(DBFields.Ortho, Ortho);
			int glossId = DBUtils.UpdateListTable(DBFields.Gloss, Gloss);
			int posId = DBUtils.UpdateListTable(DBFields.POS, POS);

			if (phoneticWriter != null)
				m_phoneticListId = phoneticWriter.Write(Phonetic, m_eticWordInfo);

			// Prepare to add the AllWordsIndex record.
			string sql = string.Format(kAWIInsertCmd, new object[] {m_docId, emicId, toneId,
				orthoId, glossId, posId, m_phoneticListId, PhoneticAnnotationOffset,
				PhoneticAnnotationLength, m_wavOffset, m_wavLength, Reference});

			// When reference is null or empty string an empty string is written to the DB
			// when we really would like a null to be in the DB. This will ensure that.
			sql = sql.Replace("''", "Null");

			// ExecuteNonSelectQuery should always return 1.
			if (DBUtils.ExecuteNonSelectSQL(sql) == 0)
				return 0;

			IsDirty = false;
			m_allWordIndexId = DBUtils.IdOfLastRecordAddedToDB;
			return m_allWordIndexId;
		}

		#endregion

		#region Helper Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the word specified by the dbField.
		/// </summary>
		/// <param name="dbfield"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public string GetWord(string dbField)
		{
			return (m_words.ContainsKey(dbField) ? m_words[dbField] : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the word specified by dbField.
		/// </summary>
		/// <param name="dbField"></param>
		/// <param name="value"></param>
		/// ------------------------------------------------------------------------------------
		public void SetWord(string dbField, string val)
		{
			string oldValue = GetWord(dbField);

			if (oldValue == val)
				return;

			if (val == null || val.Trim() == string.Empty)
			{
				if (m_words.ContainsKey(dbField))
					m_words.Remove(dbField);
			}
			else
			{
				// Decompose the word if it's phonetic.
				m_words[dbField] =
					(dbField == DBFields.Phonetic && !val.IsNormalized(NormalizationForm.FormD) ?
					val.Normalize(NormalizationForm.FormD) : val);
			}

			m_isDirty = true;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not any of the words changed and are out of
		/// sync. with what's in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDirty
		{
			get {return m_isDirty;}
			internal set {m_isDirty = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the document id associated with the words.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DocId
		{
			get {return m_docId;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the Id of the AllWordsIndex record for the words.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int AllWordIndexId
		{
			get {return m_allWordIndexId;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the word offset of the phonetic word in the transcription. (i.e the first
		/// phonetic word in the transcription is 0, the second is 1 and so on.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int PhoneticAnnotationOffset
		{
			get {return m_annotationOffset;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the length of the phonetic word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int PhoneticAnnotationLength
		{
			get {return Phonetic == null ? 0 : Phonetic.Length;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the word specified by the dbField.
		/// </summary>
		/// <param name="dbField"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public string this[string dbField]
		{
			get {return GetWord(dbField);}
			set {SetWord(dbField, value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Phonetic
		{
			get {return GetWord(DBFields.Phonetic);}
			set {SetWord(DBFields.Phonetic, value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Tone
		{
			get {return GetWord(DBFields.Tone);}
			set {SetWord(DBFields.Tone, value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Phonemic
		{
			get {return GetWord(DBFields.Phonemic);}
			set {SetWord(DBFields.Phonemic, value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Ortho
		{
			get {return GetWord(DBFields.Ortho);}
			set {SetWord(DBFields.Ortho, value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Gloss
		{
			get {return GetWord(DBFields.Gloss);}
			set {SetWord(DBFields.Gloss, value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string POS
		{
			get {return GetWord(DBFields.POS);}
			set
			{
				if (value == null)
					value = string.Empty;
			
				SetWord(DBFields.POS, (value == DBUtils.kstidNoneText ? string.Empty : value));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used for the displayed text in a grid combo box. When POS is an empty string or
		/// null then a localized version of "none" is shown for the part of speech.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string POSDropDownDisplayText
		{
			get
			{
				string pos = GetWord(DBFields.POS);
				return (pos == null || pos == string.Empty ? DBUtils.kstidNoneText : pos);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Reference
		{
			get {return GetWord(DBFields.Reference);}
			set {SetWord(DBFields.Reference, value);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CVPattern
		{
			get {return GetWord(DBFields.CVPattern);}
			set {SetWord(DBFields.CVPattern, value);}
		}

		#endregion
	}
}