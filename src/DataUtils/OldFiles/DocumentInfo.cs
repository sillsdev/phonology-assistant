using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace SIL.Pa.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class DocumentInfo
	{
		internal static string[] m_audioFileFormats = new string[] {"CECIL (.UTT)", "Wave (.WAV)",
									   "Macintosh (.AIF)", "TIMIT"};

		internal DocumentWord[] m_docWords;
		internal bool m_isAudioDoc = false;
		internal int m_id = 0;
		internal int m_folderId = 0;
		internal int m_speakerId = 0;
		internal int m_channels;
		internal int m_samplesPerSecond;
		internal int m_bitsPerSample;
		internal double m_recordingLength;
		internal string m_recordingLengthFormatted;
		internal string m_title;
		internal string m_saFilename;
		internal string m_comments;
		internal string m_freeform;
		internal string m_notebookRef;
		internal string m_dialect;
		internal string m_transcriber;
		internal string m_speaker;
		internal string m_recordingFmt;
		internal DateTime m_origDate = DateTime.Now;
		internal DateTime m_lastUpdate = DateTime.Now;
		internal DateTime m_recordingTime;
		internal Gender m_gender;

		#region Static Creating/Loading methods
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Creates a new document for the document cache.
		///// </summary>
		///// <param name="docId">The temporary id to assign the document. This id will be
		///// replaced by a permanent one when the document is saved to the database.</param>
		///// <param name="rawData">A table of fields and the unparsed data associated with
		///// the field. The key for rawData is one of the DBFields enumeration values and
		///// the value is the unparsed string of data for that field.</param>
		///// <returns></returns>
		///// ------------------------------------------------------------------------------------
		//public static DocumentInfo Create(int docId, Dictionary<string, string> rawData)
		//{
		//    Debug.Assert(rawData != null);
		//    Debug.Assert(rawData.Count > 0);
		//    Debug.Assert(docId < 0);

		//    Dictionary<string, string> dataForWordParsing = new Dictionary<string, string>();

		//    DocumentInfo docInfo = new DocumentInfo();
		//    docInfo.m_id = docId;

		//    // Go through the raw data assigning the appropriate document fields.
		//    foreach (KeyValuePair<string, string> field in rawData)
		//    {
		//        // TODO: Deal with catTitle and FolderTitle


		//        if (field.Key == DBFields.DocTitle)
		//            docInfo.m_title = field.Value;
		//        else if (field.Key == DBFields.SAFileName)
		//            docInfo.m_saFilename = field.Value;
		//        else if (field.Key == DBFields.Freeform)
		//            docInfo.m_freeform = field.Value;
		//        else if (field.Key == DBFields.Comments)
		//            docInfo.m_comments = field.Value;
		//        else if (field.Key == DBFields.Reference)
		//            docInfo.m_notebookRef = field.Value;
		//        else if (field.Key == DBFields.Reference)
		//            docInfo.m_transcriber = field.Value;
		//        else if (field.Key == DBFields.Dialect)
		//            docInfo.m_dialect = field.Value;
		//        else if (field.Key == DBFields.SpeakerName)
		//            docInfo.m_speaker = field.Value;
		//        else if (field.Key == DBFields.OriginalDate)
		//        {
		//            if (!DateTime.TryParse(field.Value, out docInfo.m_origDate))
		//                docInfo.m_origDate = DateTime.Now;
		//        }
		//        else if (field.Key == DBFields.LastUpdate)
		//        {
		//            if (!DateTime.TryParse(field.Value, out docInfo.m_lastUpdate))
		//                docInfo.m_lastUpdate = DateTime.Now;
		//        }
		//        else if (field.Key == DBFields.Gender)
		//            docInfo.m_gender = ParseGender(field.Value);
		//        else
		//            dataForWordParsing[field.Key] = field.Value;
		//    }

		//    // Take care of all the raw data that parses into words.
		//    if (dataForWordParsing.Count > 0)
		//        docInfo.m_words = DocumentWord.Create(docId, dataForWordParsing);

		//    return docInfo;
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates and initializes a DocumentInfo object from the database using the
		/// specified document id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DocumentInfo Load(int docId)
		{
			using (OleDbDataReader reader = DBUtils.GetQueryResultsFromDB("qryDocumentInfo", docId))
			{
				if (!reader.Read())
				{
					reader.Close();
					return null;
				}

				DocumentInfo docInfo = new DocumentInfo();

				docInfo.m_id = docId;
				docInfo.m_isAudioDoc = (bool)reader["Wave"];
				docInfo.m_folderId = (int)reader[DBFields.FolderId];
				docInfo.m_title = reader[DBFields.DocTitle] as string;
				docInfo.m_saFilename = reader[DBFields.SAFileName] as string;
				docInfo.m_comments = reader[DBFields.Comments] as string;
				docInfo.m_freeform = reader[DBFields.Freeform] as string;
				docInfo.m_notebookRef = reader[DBFields.Reference] as string;
				docInfo.m_dialect = reader[DBFields.Dialect] as string;
				docInfo.m_transcriber = reader[DBFields.Transcriber] as string;
				
				docInfo.m_origDate = (reader[DBFields.OriginalDate] is DBNull ?
					DateTime.MinValue : (DateTime)reader[DBFields.OriginalDate]);

				docInfo.m_lastUpdate = (reader[DBFields.LastUpdate] is DBNull ?
					DateTime.MinValue : (DateTime)reader[DBFields.LastUpdate]);

				docInfo.m_speakerId = (reader["SpeakerId"] is DBNull ? 0 : (int)reader["SpeakerId"]);
				if (docInfo.m_speakerId == 0)
					docInfo.m_gender = Gender.Unspecified;
				else
				{
					docInfo.m_gender = ((byte)reader["Gender"] == 1 ? Gender.Male : Gender.Female);
					docInfo.m_speaker = reader[DBFields.SpeakerName] as string;
				}

				if (docInfo.m_isAudioDoc)
				{
					docInfo.m_channels = Convert.ToInt32((short)reader["Channels"]);
					docInfo.m_samplesPerSecond = (int)reader["SamplesPerSecond"];
					docInfo.m_bitsPerSample = Convert.ToInt32((short)reader["BitsPerSample"]);
					docInfo.m_recordingLength = (double)reader["RecordingLength"];
					docInfo.m_recordingLengthFormatted = SecondsToTime(docInfo.m_recordingLength);

					int recFmt = Convert.ToInt32((byte)reader["RecordFileFormat"]);
					docInfo.m_recordingFmt = m_audioFileFormats[recFmt];

					int timeStamp = (int)reader["RecordTimeStamp"];
					docInfo.m_recordingTime = DateTime.FromFileTime(timeStamp);
				}

				docInfo.m_docWords = DocumentWord.Load(docId);

				reader.Close();
				return docInfo;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's a single string containing the combined words for a specified transcription
		/// type.
		/// </summary>
		/// <param name="dbField"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public string GetFullTranscription(string dbField)
		{
			StringBuilder fullTrans = new StringBuilder();

			foreach (DocumentWord word in m_docWords)
			{
				fullTrans.Append(word.GetWord(dbField));
				fullTrans.Append(" ");
			}

			return fullTrans.ToString().Trim();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the document exists in the database yet.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool InDatabase
		{
			// Document's with negative id's have not been added to the DB yet.
			get {return m_id > 0;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DocumentWord[] Words
		{
			get {return m_docWords;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the document's Id
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Id
		{
			get {return m_id;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the document's 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int FolderId
		{
			get {return m_folderId;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Title
		{
			get {return m_title;}
			set {m_title = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the document's 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SpeakerId
		{
			get {return m_speakerId;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's speaker name
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Speaker
		{
			get {return m_speaker;}
			set {m_speaker = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Gender SpeakersGender
		{
			get {return m_gender;}
			set {m_gender = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's dialect
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Dialect
		{
			get {return m_dialect;}
			set {m_dialect = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's comments
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Comments
		{
			get {return m_comments;}
			set {m_comments = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's freeform comments
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FreeformComments
		{
			get {return m_freeform;}
			set {m_freeform = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's notebook reference
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NotebookReference
		{
			get {return m_notebookRef;}
			set {m_notebookRef = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the document's transcriber's name
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber
		{
			get {return m_transcriber;}
			set {m_transcriber = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the document's 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAudioDocument
		{
			get {return m_isAudioDoc;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document's WAV file name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SAFilename
		{
			get {return m_saFilename;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the number of channels the audio document was recorded in.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Channels
		{
			get {return m_channels;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document's samples per second
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SamplesPerSecond
		{
			get {return m_samplesPerSecond;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document's length in seconds
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public double RecordingLength
		{
			get {return m_recordingLength;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document's length in HH:MM:SS.00 format.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string RecordingLengthFormatted
		{
			get {return m_recordingLengthFormatted;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document's bits per sample.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitsPerSample
		{
			get {return m_bitsPerSample;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document's recording date/time
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DateTime RecordingTime
		{
			get {return m_recordingTime;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document's format (CECIL (UTT), Wave, AIF or TIMIT)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string RecordingFormat
		{
			get {return m_recordingFmt;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the date/time the document was submitted to the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DateTime OriginalDate
		{
			get {return m_origDate;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the date/time the document was last updated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DateTime LastUpdate
		{
			get {return m_lastUpdate;}
			set {m_lastUpdate = value;}
		}

		#endregion

		#region Helper methods
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to parse a string to determine what gender it specified.
		/// </summary>
		/// <param name="genderString"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private static Gender ParseGender(string genderString)
		{
			genderString = genderString.Trim().ToLower();

			if ("male man boy gentleman".Contains(genderString))
				return Gender.Male;
			else if ("female woman girl lady".Contains(genderString))
				return Gender.Female;

			return Gender.Unspecified;
		}

		#endregion
	}
}
