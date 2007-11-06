using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SIL.Pa
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
		private readonly int m_id;
		private readonly bool m_canBeEditedInToolbox = false;
		private PaDataSource m_dataSource;
		private bool m_needsParsing = false;
		private string m_firstInterlinearField = null;
		private List<string> m_interlinearFields;
		private List<WordCacheEntry> m_wordEntries;
		private Dictionary<string, PaFieldValue> m_fieldValues;
		private object m_tag = null;
		
		// These three variables are for records associated with SA audio files.
		private int m_channels;
		private long m_samplesPerSec;
		private int m_bitsPerSample;

		// This is only used for deserialization
		private List<PaFieldValue> m_fieldValuesList;

		private static string s_dataSourceFieldName;
		private static string s_dataSourcePathFieldName;
		private static int s_counter = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows the RecordCache to reset the counter that assigns Id's to record cache
		/// entries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void ResetCounter()
		{
			s_counter = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry()
		{
			m_id = s_counter++;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only use the constructor when the data source is an SFM file or SA sound file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry(bool newFromParsingSFMFile) : this()
		{
			m_fieldValues = new Dictionary<string, PaFieldValue>();

			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
				m_fieldValues[fieldInfo.FieldName] = new PaFieldValue(fieldInfo.FieldName);

			m_canBeEditedInToolbox = newFromParsingSFMFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the static variables that hold the name of the data source field and
		/// the name of the data source path field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void InitializeDataSourceFields(PaFieldInfoList fieldInfoList)
		{
			if (fieldInfoList != null)
			{
				PaFieldInfo fieldInfo = fieldInfoList.DataSourceField;
				if (fieldInfo != null)
					s_dataSourceFieldName = fieldInfo.FieldName;

				fieldInfo = fieldInfoList.DataSourcePathField;
				if (fieldInfo != null)
					s_dataSourcePathFieldName = fieldInfo.FieldName;
			}
		}

		#region Methods and Indexers for getting and setting field values
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to set the specified property with the specified value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetValue(string field, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				PaFieldValue fieldValue;

				if (field != s_dataSourcePathFieldName && field != s_dataSourceFieldName &&
					m_fieldValues.TryGetValue(field, out fieldValue))
				{
					fieldValue.Value = value;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string this[string field]
		{
			get { return GetValue(field); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Unlike GetValue, this method tries to get the value of the specified field and
		/// when it fails, null is returned and nothing else is tried, nor special handling
		/// given to certain fields. This method was introduced to fix PA-691.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetValueBasic(string field)
		{
			PaFieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue))
			{
				if (fieldValue.Value != null)
					return fieldValue.Value;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value of the specified field, giving specially handling for certain
		/// fields or deferring to the record's word entries if necessary.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetValue(string field)
		{
			// If the data source file path is being requested then defer
			// to the record's data source object to get that information.
			if (field == s_dataSourcePathFieldName)
			{
				return (m_dataSource.DataSourceType == DataSourceType.FW &&
					m_dataSource.FwSourceDirectFromDB ?	m_dataSource.FwDataSourceInfo.MachineName :
					Path.GetDirectoryName(m_dataSource.DataSourceFile)); 
			}

			// If the data source name is being requested then defer to
			// the record's data source object to get that information.
			if (field == s_dataSourceFieldName)
			{
				return (m_dataSource.DataSourceType == DataSourceType.FW &&
					m_dataSource.FwSourceDirectFromDB ?	m_dataSource.FwDataSourceInfo.ToString() :
					Path.GetFileName(m_dataSource.DataSourceFile));
			}

			PaFieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue))
			{
				if (fieldValue.Value != null)
					return fieldValue.Value;
			}

			// If the field isn't in the word cache entry's values, check if it's a parsed field.
			PaFieldInfo fieldInfo = PaApp.FieldInfo[field];
			if (fieldInfo == null || !fieldInfo.IsParsed)
				return null;

			// At this point, we know 2 things: 1) either this record cache entry doesn't
			// contain a value for the specified field or it does and the value is null, or
			// 2) the specified field is a parsed field. Therefore, gather together all the
			// words (from this record cache entry's owned word cache entries) for the field.
			// When gathering the word cache entry's, use the GetField method instead of
			// wentry[field] because when using wentry[field] to get word cache entry values
			// and any of the word cache entry values are null, word cache entries defer to
			// the value from their owning record cache entry. But that will put us right back
			// here, thus causing a circular problem ending in a stack overflow. Hence the
			// call to GetField() passing false in the second argument.
			StringBuilder words = new StringBuilder();

			if (m_wordEntries != null)
			{
				foreach (WordCacheEntry wentry in m_wordEntries)
				{
					words.Append(wentry.GetField(field, false));
					words.Append(' ');
				}
			}

			string trimmedWords = words.ToString().Trim();
			return (trimmedWords == string.Empty ? null : trimmedWords);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the values for an interlinear field. The values can be considered all
		/// the column values for the specified field. When the fieldInfo parameter is for
		/// the phonetic field, then useOriginalPhonetic tells the method to get the value
		/// of the phonetic as it was before applying any experimental transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] GetParsedFieldValues(PaFieldInfo fieldInfo, bool useOriginalPhonetic)
		{
			if (fieldInfo == null)
				return null;

			List<string> values = new List<string>();
			bool getOrigPhonetic = (useOriginalPhonetic && fieldInfo.IsPhonetic);
			string fldName = fieldInfo.FieldName;

			// Go through the parsed word entries and get the values for the specified field.
			foreach (WordCacheEntry wentry in m_wordEntries)
			{
				string fieldValue = (getOrigPhonetic ?
					wentry.OriginalPhoneticValue : wentry.GetField(fldName, false));
				
				values.Add(fieldValue != null ? fieldValue.Trim() : string.Empty);
			}

			return (values.Count == 0 ? null : values.ToArray());
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public object Tag
		{
			get { return m_tag; }
			set { m_tag = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a number that uniquely identifies the record entry among its peers within
		/// the application's record cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Id
		{
			get { return m_id; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the record can be edited in a toolbox
		/// database (i.e. true means PA assumes the record came from a Toolbox DB).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool CanBeEditedInToolbox
		{
			get { return m_canBeEditedInToolbox; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates whether or not the record entry needs to be parsed. (Entries created
		/// from an XML file or from a FW database should not need parsing, while those
		/// created from SFM records should.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool NeedsParsing
		{
			get { return m_needsParsing; }
			set { m_needsParsing = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the record's data source object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PaDataSource DataSource
		{
			get { return m_dataSource; }
			set { m_dataSource = value; }
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> InterlinearFields
		{
			get { return m_interlinearFields; }
			set { m_interlinearFields = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the record contains interlinear data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool HasInterlinearData
		{
			get
			{
				// If data source is not an SFM file then set this to true and let the
				// values of m_interlinearFields and m_firstInterlinearField determine
				// what's returned. Otherwise, this data source's parse type must be
				// interlinear.
				bool projParseTypeOK = (m_dataSource.DataSourceType != DataSourceType.SFM &&
					m_dataSource.DataSourceType != DataSourceType.Toolbox ? true :
					m_dataSource.ParseType == DataSourceParseType.Interlinear);

				return (projParseTypeOK && m_interlinearFields != null &&
					m_firstInterlinearField != null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This property is really only used for serialization and deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("Fields")]
		public List<PaFieldValue> FieldValues
		{
			get
			{
				if (m_fieldValues != null)
				{
					m_fieldValuesList = new List<PaFieldValue>();
					foreach (KeyValuePair<string, PaFieldValue> fieldValue in m_fieldValues)
						m_fieldValuesList.Add(fieldValue.Value);
				}

				return m_fieldValuesList;
			}
			set {m_fieldValuesList = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlArray("ParsedFields")]
		public List<WordCacheEntry> WordEntries
		{
			get { return m_wordEntries; }
			set { m_wordEntries = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of channels in the record's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Channels
		{
			get { return m_channels; }
			set { m_channels = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of samples per second in the record's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public long SamplesPerSecond
		{
			get { return m_samplesPerSec; }
			set { m_samplesPerSec = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of bits per sample in the record's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int BitsPerSample
		{
			get { return m_bitsPerSample; }
			set { m_bitsPerSample = value; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified field is one of the
		/// interlinear fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsInterlinearField(string field)
		{
			return (HasInterlinearData && m_interlinearFields.Contains(field));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because deserialization cannot deserialize a dictionary, moving field values from
		/// the deserialized values list to a dictionary has to be done in a separate process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PostDeserializeProcess(PaDataSource dataSource)
		{
			m_dataSource = dataSource;

			if (m_fieldValuesList != null && m_fieldValuesList.Count > 0)
			{
				m_fieldValues = new Dictionary<string, PaFieldValue>();
				foreach (PaFieldValue fieldValue in m_fieldValuesList)
					m_fieldValues[fieldValue.Name] = fieldValue;

				m_fieldValuesList = null;
			}

			int i = 0;
			if (m_wordEntries != null)
			{
				foreach (WordCacheEntry entry in m_wordEntries)
				{
					// This should never happen, but I did see it happen
					// for some unexplainable reason.
					if (entry.RecordEntry == null)
						entry.RecordEntry = this;

					entry.PostDeserializeProcess();
					entry.WordIndex = i++;
				}
			}
		}
	}
}
