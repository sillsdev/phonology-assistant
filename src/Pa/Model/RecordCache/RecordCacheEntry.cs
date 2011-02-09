using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using SIL.Pa.DataSource;

namespace SIL.Pa.Model
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
		private IDictionary<string, PaFieldValue> m_fieldValues;

		// This is only used for deserialization
		private List<PaFieldValue> m_fieldValuesList;

		private static string s_dataSourceFieldName;
		private static string s_dataSourcePathFieldName;
		private static int s_counter;

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
		public RecordCacheEntry()
		{
			Id = s_counter++;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only use the constructor when the data source is not PAXML or FW6 (or older)
		/// data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCacheEntry(bool newFromParsingSFMFile) : this()
		{
			m_fieldValues = App.FieldInfo.ToDictionary(fi => fi.FieldName,
				fi => new PaFieldValue(fi.FieldName));

			CanBeEditedInToolbox = newFromParsingSFMFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the static variables that hold the name of the data source field and
		/// the name of the data source path field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void InitializeDataSourceFields(PaFieldInfoList fieldInfoList)
		{
			if (fieldInfoList == null)
				return;

			var fieldInfo = fieldInfoList.DataSourceField;
			if (fieldInfo != null)
				s_dataSourceFieldName = fieldInfo.FieldName;

			fieldInfo = fieldInfoList.DataSourcePathField;
			if (fieldInfo != null)
				s_dataSourcePathFieldName = fieldInfo.FieldName;
		}

		#region Methods and Indexers for getting and setting field values
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to set the specified property with the specified value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetValue(string field, string value)
		{
			if (string.IsNullOrEmpty(value))
				return;
			
			PaFieldValue fieldValue;

			if (field != s_dataSourcePathFieldName && field != s_dataSourceFieldName &&
				m_fieldValues.TryGetValue(field, out fieldValue))
			{
				fieldValue.Value = value;
			}
		}

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
			if (m_fieldValues.TryGetValue(field, out fieldValue) && fieldValue.Value != null) 
				return fieldValue.Value;

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
				return (DataSource.DataSourceType == DataSourceType.FW &&
					DataSource.FwSourceDirectFromDB ? DataSource.FwDataSourceInfo.Server :
					Path.GetDirectoryName(DataSource.DataSourceFile)); 
			}

			// If the data source name is being requested then defer to
			// the record's data source object to get that information.
			if (field == s_dataSourceFieldName)
			{
				return (DataSource.DataSourceType == DataSourceType.FW &&
					DataSource.FwSourceDirectFromDB ? DataSource.FwDataSourceInfo.ToString() :
					Path.GetFileName(DataSource.DataSourceFile));
			}

			PaFieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue) && fieldValue.Value != null)
				return fieldValue.Value;

			// If the field isn't in the word cache entry's values, check if it's a parsed field.
			var fieldInfo = App.FieldInfo[field];
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
			var words = new StringBuilder();

			if (WordEntries != null)
			{
				foreach (var wentry in WordEntries)
				{
					words.Append(wentry.GetField(field, false));
					words.Append(' ');
				}
			}

			var trimmedWords = words.ToString().Trim();
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

			bool getOrigPhonetic = (useOriginalPhonetic && fieldInfo.IsPhonetic);
			string fldName = fieldInfo.FieldName;

			// Go through the parsed word entries and get the values for the specified field.
			var values = WordEntries.Select(we =>
			{
				var val = (getOrigPhonetic ? we.OriginalPhoneticValue : we.GetField(fldName, false));
				return (val != null ? val.Trim() : string.Empty);
			}).ToArray();

			return (values.Length == 0 ? null : values.ToArray());
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public object Tag { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a number that uniquely identifies the record entry among its peers within
		/// the application's record cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Id { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the record can be edited in a toolbox
		/// database (i.e. true means PA assumes the record came from a Toolbox DB).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool CanBeEditedInToolbox { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates whether or not the record entry needs to be parsed. (Entries created
		/// from an XML file or from a FW6 (or earlier) project should not need parsing,
		/// while those created from all other data sources, should.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool NeedsParsing { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the record's data source object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PaDataSource DataSource { get; set; }

		/// ------------------------------------------------------------------------------------
		public string FirstInterlinearField { get; set; }

		/// ------------------------------------------------------------------------------------
		public List<string> InterlinearFields { get; set; }

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
				bool projParseTypeOK = (DataSource.DataSourceType != DataSourceType.SFM &&
					DataSource.DataSourceType != DataSourceType.Toolbox ? true :
					DataSource.ParseType == DataSourceParseType.Interlinear);

				return (projParseTypeOK && InterlinearFields != null &&
					FirstInterlinearField != null);
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
					m_fieldValuesList = m_fieldValues.Values.ToList();

				return m_fieldValuesList;
			}
			set {m_fieldValuesList = value;}
		}

		/// ------------------------------------------------------------------------------------
		[XmlArray("ParsedFields")]
		public List<WordCacheEntry> WordEntries { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of channels in the record's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Channels { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of samples per second in the record's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public long SamplesPerSecond { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Number of bits per sample in the record's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int BitsPerSample { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified field is one of the
		/// interlinear fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsInterlinearField(string field)
		{
			return (HasInterlinearData && InterlinearFields.Contains(field));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because deserialization cannot deserialize a dictionary, moving field values from
		/// the deserialized values list to a dictionary has to be done in a separate process.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PostDeserializeProcess(PaDataSource dataSource)
		{
			DataSource = dataSource;

			if (m_fieldValuesList != null && m_fieldValuesList.Count() > 0)
			{
				m_fieldValues = m_fieldValuesList.ToDictionary(fv => fv.Name, fv => fv);
				m_fieldValuesList = null;
			}

			if (WordEntries == null)
				return;
			
			int i = 0;
			foreach (var entry in WordEntries)
			{
				if (entry.RecordEntry == null)
					entry.RecordEntry = this;

				entry.PostDeserializeProcess();
				entry.WordIndex = i++;
			}
		}
	}
}
