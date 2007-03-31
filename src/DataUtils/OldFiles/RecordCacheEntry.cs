using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class contains a sinlge item in a record cache. There is only one record cache per
	/// instance of PA. A RecordCacheEntry contains the data associated with a single record
	/// read from the data source. When a record contains multiple phonetic, tone, phonemic,
	/// orthographic, gloss, POS or CVPattern "words", the RecordCacheEntry references
	/// multiple WordCacheEntry objects for those "words".
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Record")]
	public class RecordCacheEntry
	{
		string m_firstInterlinearField = null;
		private List<WordCacheEntry> m_wordEntries;

		private List<PaFieldValue> m_fieldValuesList;
		private Dictionary<string, PaFieldValue> m_fieldValuesTable;

		private string m_phonetic;
		private string m_phonemic;
		private string m_tone;
		private string m_gloss;
		private string m_ortho;
		private string m_pos;
		private string m_cvPattern;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry()
		{
			m_fieldValuesList = new List<PaFieldValue>();
			m_fieldValuesTable = new Dictionary<string, PaFieldValue>();

			int i = 0;
			foreach (PaFieldInfo fieldInfo in DBUtils.FieldInfo)
			{
//				if (!fieldInfo.IsParsed)
				{
					PaFieldValue value = new PaFieldValue();
					value.Name = fieldInfo.FieldName;
					value.Index = i++;
					m_fieldValuesList.Add(value);
					m_fieldValuesTable[value.Name] = value;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to set the specified property with the specified value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetValue(string field, string value)
		{
			if (m_fieldValuesTable.ContainsKey(field))
				m_fieldValuesTable[field].Value = value;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string this[string field]
		{
			get { return GetField(field); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string this[int fieldIndex]
		{
			get { return GetField(fieldIndex); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetField(string field)
		{
			return (m_fieldValuesTable.ContainsKey(field) ?
				m_fieldValuesTable[field].Value : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetField(int fieldIndex)
		{
			return (fieldIndex < m_fieldValuesList.Count && fieldIndex >= 0 ?
				m_fieldValuesList[fieldIndex].Value : null);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("Fields")]
		public List<PaFieldValue> FieldValues
		{
			get { return m_fieldValuesList; }
			set { m_fieldValuesList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FirstInterlinearField
		{
			get { return m_firstInterlinearField; }
			set { m_firstInterlinearField = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Phonetic
		{
			get
			{
				return "FIX Me!";
				//if (m_phonetic != null)
				//    return m_phonetic;
				
				//StringBuilder words = new StringBuilder();
				//foreach (WordCacheEntry entry in m_wordEntries)
				//{
				//    if (entry.Phonetic != null)
				//    {
				//        words.Append(entry.Phonetic);
				//        words.Append(" ");
				//    }
				//}

				//return (words.Length == 0 ? null : words.ToString().Trim());
			}
			set {m_phonetic = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Phonemic
		{
			get
			{
				return "FIX Me!";
				//if (m_phonemic != null)
				//    return m_phonemic;

				//StringBuilder words = new StringBuilder();
				//foreach (WordCacheEntry entry in m_wordEntries)
				//{
				//    if (entry.Phonemic != null)
				//    {
				//        words.Append(entry.Phonetic);
				//        words.Append(" ");
				//    }
				//}

				//return (words.Length == 0 ? null : words.ToString().Trim());
			}
			set { m_phonemic = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Tone
		{
			get
			{
				return "FIX Me!";
				//if (m_tone != null)
				//    return m_tone;

				//StringBuilder words = new StringBuilder();
				//foreach (WordCacheEntry entry in m_wordEntries)
				//{
				//    if (entry.Tone != null)
				//    {
				//        words.Append(entry.Tone);
				//        words.Append(" ");
				//    }
				//}

				//return (words.Length == 0 ? null : words.ToString().Trim());
			}
			set { m_tone = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Gloss
		{
			get
			{
				return "FIX Me!";
				//if (m_gloss != null)
				//    return m_gloss;

				//StringBuilder words = new StringBuilder();
				//foreach (WordCacheEntry entry in m_wordEntries)
				//{
				//    if (entry.Gloss != null)
				//    {
				//        words.Append(entry.Gloss);
				//        words.Append(" ");
				//    }
				//}

				//return (words.Length == 0 ? null : words.ToString().Trim());
			}
			set { m_gloss = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Ortho
		{
			get
			{
				return "FIX Me!";
			//    if (m_ortho != null)
			//        return m_ortho;

			//    StringBuilder words = new StringBuilder();
			//    foreach (WordCacheEntry entry in m_wordEntries)
			//    {
			//        if (entry.Ortho != null)
			//        {
			//            words.Append(entry.Ortho);
			//            words.Append(" ");
			//        }
			//    }

			//    return (words.Length == 0 ? null : words.ToString().Trim());
			}
			set { m_ortho = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string POS
		{
			get
			{
				return "FIX Me!";
			//    if (m_pos != null)
			//        return m_pos;

			//    StringBuilder words = new StringBuilder();
			//    foreach (WordCacheEntry entry in m_wordEntries)
			//    {
			//        if (entry.POS != null)
			//        {
			//            words.Append(entry.POS);
			//            words.Append(" ");
			//        }
			//    }

			//    return (words.Length == 0 ? null : words.ToString().Trim());
			}
			set { m_pos = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string CVPattern
		{
			get
			{
				return "FIX Me!";
			//    if (m_cvPattern != null)
			//        return m_cvPattern;

			//    StringBuilder words = new StringBuilder();
			//    foreach (WordCacheEntry entry in m_wordEntries)
			//    {
			//        if (entry.CVPattern != null)
			//        {
			//            words.Append(entry.CVPattern);
			//            words.Append(" ");
			//        }
			//    }

			//    return (words.Length == 0 ? null : words.ToString().Trim());
			}
			set { m_cvPattern = value; }
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Reference
		//{
		//    get { return m_reference; }
		//    set { m_reference = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Dialect
		//{
		//    get { return m_dialect; }
		//    set { m_dialect = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Freeform
		//{
		//    get { return m_freeform; }
		//    set { m_freeform = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string NotebookRef
		//{
		//    get { return m_notebookRef; }
		//    set { m_notebookRef = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string EthnologueId
		//{
		//    get { return m_ethnologueId; }
		//    set { m_ethnologueId = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string LanguageName
		//{
		//    get { return m_languageName; }
		//    set { m_languageName = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Region
		//{
		//    get { return m_region; }
		//    set { m_region = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Country
		//{
		//    get { return m_country; }
		//    set { m_country = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Family
		//{
		//    get { return m_family; }
		//    set { m_family = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Transcriber
		//{
		//    get { return m_transcriber; }
		//    set { m_transcriber = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string WaveFile
		//{
		//    get { return m_waveFile; }
		//    set { m_waveFile = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string SpeakerName
		//{
		//    get { return m_speakerName; }
		//    set { m_speakerName = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlIgnore]
		//public string SpeakerGender
		//{
		//    get
		//    {
		//        // TODO: Localize
		//        switch (m_gender)
		//        {
		//            case "M": return "Male";
		//            case "F": return "Female";
		//            case "C": return "Child";
		//            case " ": return "Unknown";
		//        }

		//        return string.Empty;
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Gender
		//{
		//    get { return m_gender; }
		//    set { m_gender = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string Comment
		//{
		//    get { return m_comment; }
		//    set { m_comment = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlIgnore]
		//public string OriginalDate
		//{
		//    get { return m_origDate == DateTime.MinValue ? null : m_origDate.ToString(); }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlElement("Date")]
		//public DateTime OrigDateAsDate
		//{
		//    get { return m_origDate; }
		//    set { m_origDate = value; }
		//}

		//#region Custom fields
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField0
		//{
		//    get { return m_customField0; }
		//    set { m_customField0 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField1
		//{
		//    get { return m_customField1; }
		//    set { m_customField1 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField2
		//{
		//    get { return m_customField2; }
		//    set { m_customField2 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField3
		//{
		//    get { return m_customField3; }
		//    set { m_customField3 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField4
		//{
		//    get { return m_customField4; }
		//    set { m_customField4 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField5
		//{
		//    get { return m_customField5; }
		//    set { m_customField5 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField6
		//{
		//    get { return m_customField6; }
		//    set { m_customField6 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField7
		//{
		//    get { return m_customField7; }
		//    set { m_customField7 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField8
		//{
		//    get { return m_customField8; }
		//    set { m_customField8 = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string CustomField9
		//{
		//    get { return m_customField9; }
		//    set { m_customField9 = value; }
		//}

		//#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("Words")]
		public List<WordCacheEntry> WordEntries
		{
			get { return m_wordEntries; }
			set { m_wordEntries = value; }
		}

		#endregion
	}
}
