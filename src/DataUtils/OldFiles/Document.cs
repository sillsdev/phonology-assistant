using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class that stores (and can commit) the information for a single document to be
	/// commited it to the DB.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Document
	{
		#region Const and Static data members
		private const string kcomma = ",";

		private static string[] s_audioFileFormats = new string[] {"CECIL (.UTT)", "Wave (.WAV)",
									   "Macintosh (.AIF)", "TIMIT"};

		private static ArrayList s_audioFields = new ArrayList(new string[] {
			DBFields.FormatTag, DBFields.Channels, DBFields.SamplesPerSecond,
			DBFields.AvgBytesPerSecond, DBFields.BlockAlign, DBFields.BitsPerSample,
			DBFields.SAVersion, DBFields.SADescription, DBFields.SAFlags, DBFields.RecordingLength,
			DBFields.RecordFileFormat, DBFields.RecordTimeStamp, DBFields.RecordBandWidth,
			DBFields.RecordSampleSize, DBFields.NumberOfSamples, DBFields.SignalMax,
			DBFields.SignalMin, DBFields.SignalBandWidth, DBFields.SignalEffSampSize,
			DBFields.CalcFreqLow, DBFields.CalcFreqHigh, DBFields.CalcVoicingThd,
			DBFields.CalcPercntChng, DBFields.CalcGrpSize, DBFields.CalcIntrpGap});
		
		private static ArrayList s_validDocTableFields = new ArrayList(new string[] {
			DBFields.DocTitle, DBFields.SAFileName, DBFields.Wave, DBFields.Comments,
			DBFields.Freeform, DBFields.NoteBookRef, DBFields.SpeakerId, DBFields.Dialect,
			DBFields.Transcriber, DBFields.Region, DBFields.Country, DBFields.Family,
			DBFields.EthnologueId, DBFields.LanguageName, DBFields.OriginalDate,
			DBFields.LastUpdate});

		#endregion

		#region Member Data
		private SortedDictionary<int, DocumentWord> m_wordData;
		private Dictionary<string, object> m_docData;
		private Dictionary<string, object> m_audioData;
		private int m_docId = 0;
		private int m_catId = 0;
		private int m_folderId = 0;
		private bool m_newFromImporting = false;
		private bool m_isDirty = false;
		#endregion

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor for new instance of a Document object for adding a new document to the
		/// database.
		/// </summary>
		/// <param name="newFromImport"><c>true</c> if the document is new via importing from
		/// SFM or XML file. Otherwise, <c>false</c>.</param>
		/// ------------------------------------------------------------------------------------
		public Document(bool newFromImport)	: this()
		{
			m_newFromImporting = newFromImport;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor for new instance of a DocumentUpdate object for updating an existing
		/// document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Document()
		{
			m_docData = new Dictionary<string, object>();
			m_audioData = new Dictionary<string, object>();
			m_wordData = new SortedDictionary<int, DocumentWord>();
			Clear();
		}

		#endregion

		#region Method for loading an existing document from DB
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and initializes a DocumentInfo object from the database using the
		/// specified document id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Document Load(int docId)
		{
			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentInfo", docId))
			{
				if (!reader.Read())
				{
					reader.Close();
					return null;
				}

				Document doc = new Document();
				doc.m_docId = docId;

				doc[DBFields.DocTitle] = reader[DBFields.DocTitle] as string;
				doc[DBFields.Wave] = (bool)reader[DBFields.Wave];
				doc[DBFields.SAFileName] = reader[DBFields.SAFileName] as string;
				doc[DBFields.Comments] = reader[DBFields.Comments] as string;
				doc[DBFields.Freeform] = reader[DBFields.Freeform] as string;
				doc[DBFields.NoteBookRef] = reader[DBFields.NoteBookRef] as string;
				doc[DBFields.Dialect] = reader[DBFields.Dialect] as string;
				doc[DBFields.Transcriber] = reader[DBFields.Transcriber] as string;

				doc[DBFields.OriginalDate] = (reader[DBFields.OriginalDate] is DBNull ?
					DateTime.MinValue : (DateTime)reader[DBFields.OriginalDate]);

				doc[DBFields.LastUpdate] = (reader[DBFields.LastUpdate] is DBNull ?
					DateTime.MinValue : (DateTime)reader[DBFields.LastUpdate]);

				doc[DBFields.SpeakerId] = (reader[DBFields.SpeakerId] is DBNull ? 0 : (int)reader[DBFields.SpeakerId]);
				if ((int)doc[DBFields.SpeakerId] == 0 || reader[DBFields.Gender] is DBNull)
					doc[DBFields.Gender] = Gender.Unspecified;
				else
				{
					doc[DBFields.SpeakerName] = reader[DBFields.SpeakerName] as string;
					doc[DBFields.Gender] = (Gender)(int)(byte)reader[DBFields.Gender];
				}

				if ((bool)doc[DBFields.Wave])
				{
					doc[DBFields.Channels] = (int)reader[DBFields.Channels];
					doc[DBFields.SamplesPerSecond] = (int)reader[DBFields.SamplesPerSecond];
					doc[DBFields.BitsPerSample] = (int)reader[DBFields.BitsPerSample];
					doc[DBFields.RecordingLength] = (double)reader[DBFields.RecordingLength];
					doc[DBFields.RecordFileFormat] = (int)reader[DBFields.RecordFileFormat];
					doc[DBFields.RecordTimeStamp] = (int)reader[DBFields.RecordTimeStamp];
				}

				doc.m_wordData = DocumentWord.Load(docId);
				doc.m_isDirty = false;

				reader.Close();
				return doc;
			}
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document data specified by dbField.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object this[string dbField]
		{
			get
			{
				if (dbField == null)
					return null;

				// Category id is only valid if the document is new since a document that
				// already exists may have links to more than one category.
				if (dbField == DBFields.CatId)
					return (m_docId == 0 ? m_catId : 0);

				// Folder id is only valid if the document is new since a document that
				// already exists may have links to more than one folder.
				if (dbField == DBFields.FolderId)
					return (m_docId == 0 ? m_folderId : 0);

				if (s_audioFields.Contains(dbField))
					return (m_audioData.ContainsKey(dbField) ? m_audioData[dbField] : null);

				return (m_docData.ContainsKey(dbField) ? m_docData[dbField] : null);
			}
			set
			{
				if (dbField == null)
					return;

				object cleanValue = value;

				// Get rid of extra spaces and single quotation marks since single
				// quotation marks will cause the Jet engine to barf.
				if (value is string)
				{
					cleanValue = (value as string).Replace((char)0x0027, (char)0x02BC);
					cleanValue = (cleanValue as string).Trim();
				}

				// Determine to what collection the data should be added.
				if (DBUtils.IsTranscritionField(dbField))
					ParseField(dbField, cleanValue as string);
				else if (s_audioFields.Contains(dbField))
				{
					if (!m_audioData.ContainsKey(dbField) && cleanValue == null)
						return;

					m_audioData[dbField] = value;
				}
				else
				{
					if (!m_docData.ContainsKey(dbField) && cleanValue == null)
						return;

					m_docData[dbField] = cleanValue;
				}

				m_isDirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the document's id When the document is for a new document and Commit hasn't
		/// been called, zero is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Id
		{
			get {return m_docId;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of words associated with the document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ArrayList Words
		{
			get {return (m_wordData == null ? new ArrayList() : new ArrayList(m_wordData.Values));}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the data in the document has been changed
		/// since it was read from the database, created or committed to the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDirty
		{
			get {return m_isDirty;}
			internal set {m_isDirty = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the recording's length in HH:MM:SS.dd format
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string RecordingLength
		{
			get
			{
				return (m_audioData.ContainsKey(DBFields.RecordingLength) ?
					SecondsToTime((double)m_audioData[DBFields.RecordingLength]) : string.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the recording format converted to a human readable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string RecordingFileFormat
		{
			get
			{
				return (m_audioData.ContainsKey(DBFields.RecordFileFormat) ?
					s_audioFileFormats[(int)m_audioData[DBFields.RecordFileFormat]] :
					string.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the recording's time stamp in a human readable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DateTime RecordingTimeStamp
		{
			get
			{
				return (m_audioData.ContainsKey(DBFields.RecordTimeStamp) ?
					DateTime.FromFileTime((int)m_audioData[DBFields.RecordTimeStamp]) :
					DateTime.MinValue);
			}
		}

		#endregion

		#region Misc. Public Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears that information that's been stored for the document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			m_catId = m_folderId = m_docId = 0;

			m_docData.Clear();
			m_audioData.Clear();
			m_wordData.Clear();

			// Initialize these fields to default values.
			m_docData[DBFields.OriginalDate] = DateTime.Now;
			m_docData[DBFields.Wave] = false;
			m_docData[DBFields.KBWave] = false;

			m_isDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's a single string containing the combined words for a specified transcription
		/// type specified by dbField.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetFullTranscription(string dbField)
		{
			if (m_wordData == null)
				return null;
			
			StringBuilder fullTrans = new StringBuilder();

			foreach (KeyValuePair<int, DocumentWord> word in m_wordData)
			{
				fullTrans.Append(word.Value[dbField]);
				fullTrans.Append(" ");
			}

			return fullTrans.ToString().Trim();
		}

		#endregion

		#region Helper Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a string of one or more words into individual words and stores them in
		/// a sorted list of DocumentWord objects.
		/// </summary>
		/// <param name="dbField">The type of word we're parsing</param>
		/// <param name="unparsedField">The string of unparsed words.</param>
		/// ------------------------------------------------------------------------------------
		private void ParseField(string dbField, string unparsedField)
		{
			if (unparsedField == null || unparsedField.Trim().Length == 0)
				return;

			// Split the string of words into an array of words.
			string[] words = unparsedField.Trim().Split(" ".ToCharArray());

			// Clear out old words if there are any.
			foreach (KeyValuePair<int, DocumentWord> word in m_wordData)
				word.Value[dbField] = null;

			// Now store each of the words in the array in separate DocumentWord objects.
			for (int i = 0; i < words.Length; i++)
			{
				if (!m_wordData.ContainsKey(i))
					m_wordData[i] = new DocumentWord(m_docId);

				m_wordData[i].m_annotationOffset = i;
				m_wordData[i][dbField] = words[i];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the number of seconds into a HH:MM:SS.00 time format.
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private static string SecondsToTime(double seconds)
		{
			string time = string.Empty;
			string abbrev = string.Empty;
			int i;

			// Math.Floor() is used to ensure that rounding DOWN always applied

			// Do hours segment if any.
			if (seconds >= 3600)
			{
				i = (int)Math.Floor(seconds / 3600);
				time = i.ToString("#:");
				seconds -= (3600 * i);
			}

			// Do minutes segment if any.
			i = (int)Math.Floor(seconds / 60);
			if (i > 0 || time.Length > 0)
				time += i.ToString("00:");

			seconds -= (60 * i);

			// The seconds must appear.
			time += seconds.ToString("00.00");

			// Strip off leading 0 of leading double-digit (minute/second)
			if (time[0] == '0')
				time = time.Remove(0, 1);

			return time;
		}

		#endregion

		#region Methods for committing new or updated documents to the database.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a way for the document to receive all it's word data in one fell swoop.
		/// This is used by the WaveDocumentWriter when the words (or annotations) come from
		/// transcribing a wave file in SA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Commit(SortedDictionary<int, DocumentWord> wordData)
		{
			m_isDirty = true;
			m_wordData = wordData;
			Commit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Commits to the database what's been changed in the document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Commit()
		{
			bool docIsNew = (m_docId <= 0);

			if (m_isDirty && m_docData != null && m_docData.Count > 0)
			{
				GetDocumentTitle();
				GetDocumentSpeaker();

				// Make sure to write the current date and time of this change.
				m_docData[DBFields.LastUpdate] = DateTime.Now;

				if (!docIsNew)
					DBUtils.ExecuteNonSelectSQL(GetUpdateDocumentSQL());
				else
				{
					// Add the new document and link it to a folder.
					DBUtils.ExecuteNonSelectSQL(GetAddDocumentSQL());
					m_docId = DBUtils.IdOfLastRecordAddedToDB;
					LinkDocument();
				}

				if (m_audioData.Count > 0)
					SaveAudioData(docIsNew);

				m_isDirty = false;
			}

			if (m_wordData != null)
			{
				PhoneticWriter phoneticWriter = new PhoneticWriter();

				// Commit all the word updates to the database.
				foreach (KeyValuePair<int, DocumentWord> docWord in m_wordData)
				{
					// If the phonetic is null, it means there more words of another type than
					// there are phonetic. Therefore, there isn't any phonetic to pin to those
					// words. So, what to do? For now just make the phonetic a question mark.
					// TODO: What should we do?
					if (docWord.Value[DBFields.Phonetic] == null)
						docWord.Value[DBFields.Phonetic] = "?";
					
					if (docIsNew)
						docWord.Value.m_docId = m_docId;

					docWord.Value.Commit(phoneticWriter);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a document link record to link the specified document to a folder.
		/// The document link record id is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int LinkDocument()
		{
			if (m_docId <= 0)
				return 0;

			// Create query to add a new document link record for the document.
			string sqlFmt = "INSERT INTO DocumentLinks ({0},{1}) VALUES ({2},{3})";
			string sql = string.Format(sqlFmt, DBFields.FolderId, DBFields.DocId,
				GetFolderId(), m_docId);

			return (DBUtils.ExecuteNonSelectSQL(sql) > 0 ? DBUtils.IdOfLastRecordAddedToDB : 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds or updates the doc. header table with the audio information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveAudioData(bool docIsNew)
		{
			if (!docIsNew)
				DBUtils.ExecuteNonSelectSQL(GetUpdateDocHeaderSQL());
			else
			{
				// When the document is new we have to add the document id to the audio
				// data fields so the new doc. header record gets the document id. Once
				// the record is added, then remove the document id from the audio data.
				s_audioFields.Add(DBFields.DocId);
				m_audioData[DBFields.DocId] = m_docId;

				DBUtils.ExecuteNonSelectSQL(GetAddDocHeaderSQL());

				m_audioData.Remove(DBFields.DocId);
				s_audioFields.Remove(DBFields.DocId);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a folder id for creating a document link for a new document. The id of the
		/// new folder is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetFolderId()
		{
			// If the folder id was specified, return it.
			if (m_docData.ContainsKey(DBFields.FolderId))
				m_folderId = (int)m_docData[DBFields.FolderId];

			if (m_folderId > 0)
				return m_folderId;

			string folderTitle = null;

			if (!m_docData.ContainsKey(DBFields.FolderTitle))
			{
				// Folder title wasn't specified, so get a default one.
				folderTitle = (m_newFromImporting ?
					DBUtilsStrings.kstidImportedDocFolderName :
					DBUtilsStrings.kstidNewFolderName);
			}
			else
			{
				// Folder title was specified so try to return it's id.
				folderTitle = m_docData[DBFields.FolderTitle] as string;
				m_folderId = DBUtils.GetIdForField(DBFields.FolderTitle, folderTitle);
				if (m_folderId > 0)
					return m_folderId;
			}

			// At this point we know we need to add a folder for the document.
			// Therefore we'll need a category to be the parent of the folder.
			int catId = 0;

			// If a category title was specified, it trumps any specified category id.
			// Get the category id of the specified category title. If the category
			// doesn't exist, then add it.
			if (m_docData.ContainsKey(DBFields.CatTitle))
			{
				catId = DBUtils.GetIdForField(DBFields.CatTitle,
					m_docData[DBFields.CatTitle] as string);

				if (catId == 0)
				{
					string catTitle = m_docData[DBFields.CatTitle] as string;
					catId = DBUtils.AddCategory(ref catTitle);
				}
			}

			if (catId == 0)
			{
				// Check if the category id was specified.
				if (m_docData.ContainsKey(DBFields.CatId))
					catId = (int)m_docData[DBFields.CatId];

				// Category id wasn't specified so check if the category title was specified.
				if (catId == 0 && m_docData.ContainsKey(DBFields.CatTitle))
				{
					catId = DBUtils.GetIdForField(DBFields.CatTitle,
						m_docData[DBFields.CatTitle] as string);
				}

				if (catId == 0)
					catId = DBUtils.AddCategory();
			}

			// Now we can finally add a folder.
			m_folderId = DBUtils.AddFolder(catId, ref folderTitle, !m_newFromImporting);
			return m_folderId;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the validity of an existing title or creates an appropriate new one
		/// if necessary.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private void GetDocumentTitle()
		{
			// If we're just updating an existing document and the title was specified
			// then remove it from our collection of fields to update because there's
			// no need to update it when it hasn't changed.
			if (m_docId > 0)
			{
				if (m_docData.ContainsKey(DBFields.DocTitle))
					m_docData.Remove(DBFields.DocTitle);

				return;
			}

			// Make sure document has a unique title.
			if (!m_docData.ContainsKey(DBFields.DocTitle))
			{
				string title = (m_newFromImporting ?
					DBUtilsStrings.kstidImportedDocName :
					DBUtilsStrings.kstidNewDocumentName);

				title = DBUtils.GetUniqueTitle(DBFields.DocTitle, title);
				m_docData[DBFields.DocTitle] = title;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that a record in the speaker table exists for the document's speaker.
		/// If a speaker hasn't been specified for the document, then nothing is done.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetDocumentSpeaker()
		{
			// If the document has a specified speaker id then that takes precedent.
			if (m_docData.ContainsKey(DBFields.SpeakerId))
				return;

			if (m_docData.ContainsKey(DBFields.SpeakerName))
			{
				Gender gender = Gender.Unspecified;

				if (m_docData.ContainsKey(DBFields.Gender))
					gender = (Gender)m_docData[DBFields.Gender];

				m_docData[DBFields.SpeakerId] =
					DBUtils.GetSpeakerId(m_docData[DBFields.SpeakerName] as string, gender, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an insert SQL statement used to add a new document record.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetAddDocumentSQL()
		{
			return GetAddSQL("Document", m_docData, s_validDocTableFields);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an insert SQL statement used to add a new docheader record.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetAddDocHeaderSQL()
		{
			return GetAddSQL("DocHeader", m_audioData, s_audioFields);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an insert SQL statement used to add a new document or docheader record. The
		/// statement is in the form:
		/// "INSERT INTO table (field1, field2, ...) VALUES (value1, value2, ...)"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetAddSQL(string table, Dictionary<string, object> data,
			ArrayList validFields)
		{
			StringBuilder fields = new StringBuilder(string.Format("INSERT INTO {0} (", table));
			StringBuilder values = new StringBuilder("(");

			// Loop through the fields and their values, building the appropriate pieces
			// used for the final construction of an Insert query.
			foreach (KeyValuePair<string, object> field in data)
			{
				if (field.Value != null && validFields.Contains(field.Key))
				{
					fields.Append(field.Key);
					fields.Append(kcomma);
					values.Append(GetCorrectlyDelimitedDataForSQL(field.Value));
				}
			}

			// Replace the last commas with closed parenthesis
			fields.Replace(kcomma, ")", fields.Length - 1, 1);
			values.Replace(kcomma, ")", values.Length - 1, 1);

			return string.Format("{0} VALUES {1}", fields.ToString(), values.ToString());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an update SQL statement used to update a document record.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetUpdateDocumentSQL()
		{
			return GetUpdateSQL("Document", m_docData, s_validDocTableFields);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an update SQL statement used to update a docheader record.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetUpdateDocHeaderSQL()
		{
			return GetUpdateSQL("DocHeader", m_audioData, s_audioFields);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds an update SQL statement used to update a document or docheader record. The
		/// statement is in the form:
		/// "UPDATE table SET field1=value1, field2=value2, ..."
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetUpdateSQL(string table, Dictionary<string, object> data,
			ArrayList validFields)
		{
			StringBuilder sql = new StringBuilder(string.Format("UPDATE {0} SET ", table));

			// Loop through the fields and their values, appending the appropriate
			// "SET" pieces for updating document fields.
			foreach (KeyValuePair<string, object> field in data)
			{
				if (validFields.Contains(field.Key))
				{
					sql.Append(field.Key);
					sql.Append("=");
					sql.Append(GetCorrectlyDelimitedDataForSQL(field.Value));
				}
			}

			// Remove the last comma
			sql.Remove(sql.Length - 1, 1);
			sql.Append(string.Format(" WHERE {0}={1}", DBFields.DocId, m_docId));

			return sql.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Receives a DB field value and returns a string containing that value surrounded
		/// by quotation marks when appropriate. When the value is a string or DateTime field
		/// then it is surrounded by quotation marks. Otherwise, not. The final return value
		/// also has a comma tacked onto the end.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetCorrectlyDelimitedDataForSQL(object fieldValue)
		{
			if (fieldValue == null)
				return "Null,";

			string retValue = fieldValue.ToString();
			Type type = fieldValue.GetType();

			// Make sure strings and DateTime values are surrounded
			// by single quotation marks.
			if (type == typeof(string) || type == typeof(DateTime))
			{
				if (type == typeof(string) && ((string)fieldValue).Trim() == string.Empty)
					return "Null,";

				retValue = string.Format("'{0}'", fieldValue);
			}

			return retValue + kcomma;
		}

		#endregion	
	}
}
