using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SIL.Pa.DataSource;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class contains a sinlge item in a word cache. There is only one word cache per
	/// instance of PA and the word list grids in PA (e.g. in data corpus view and search view)
	/// for a project shows entries corresponding to entries in the word cache. Furthermore,
	/// each word cache entry contains a reference to the underlying RecordCacheEntry to which
	/// it is associated. Many of the properties of a WordCacheEntry return references to the
	/// underlying RecordCacheEntry. There is a one-to-many relationship between
	/// RecordCacheEntry and WordListEntry's - potentially, one RecordCacheEntry to severl
	/// WordListEntry objects.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("ParsedFieldGroup")]
	public class WordCacheEntry
	{
		private Dictionary<string, FieldValue> m_fieldValues;
		private FieldValue m_phoneticValue;
		private string m_origPhoneticValue;
		private Dictionary<int, string[]> m_uncertainPhones;
		private string[] m_phones;

		// This is only used for deserialization
		private List<FieldValue> m_fieldValuesList;

		// This conversion list is specific to each WordCacheEntry list and contains only
		// those conversions that were applied to the phonetic word. Conversions contained
		// in s_experimentalTranscriptionList that were not applied to the phonetic word
		// do not get added to this list. This list exists so each cache entry knows what
		// experimental transcription information to display in an experimental transcription
		// popup for the user.
		private Dictionary<string, string> m_experimentalTranscriptionList;

		#region Constructors
		/// ------------------------------------------------------------------------------------
		public WordCacheEntry()
		{
		}

		/// ------------------------------------------------------------------------------------
		public WordCacheEntry(bool allocateSpaceForFieldValues) :
			this(null, allocateSpaceForFieldValues)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WordCacheEntry(RecordCacheEntry recEntry, bool allocateSpaceForFieldValues) :
			this(recEntry, 0, allocateSpaceForFieldValues)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WordCacheEntry(RecordCacheEntry recEntry, int wordIndex,
			bool allocateSpaceForFieldValues)
		{
			RecordEntry = recEntry;
			WordIndex = wordIndex;

			if (!allocateSpaceForFieldValues)
				return;

			m_fieldValues = (from m in recEntry.DataSource.FieldMappings
							 where m.IsParsed && m.Field != null
							 select m.Field).ToDictionary(f => f.Name, f => new FieldValue(f.Name));

			var mapping = recEntry.DataSource.FieldMappings
				.SingleOrDefault(m => m.Field != null && m.Field.Type == FieldType.Phonetic);

			if (mapping != null)
				m_phoneticValue = m_fieldValues[mapping.Field.Name];
		}

		#endregion

		#region Methods and Indexers for getting and setting field values
		/// ------------------------------------------------------------------------------------
		public string this[string field]
		{
			get { return GetField(field, true); }
			set { SetValue(field, value); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the specified field with the specified value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetValue(string field, string value)
		{
			if (string.IsNullOrEmpty(value))
				return;

			FieldValue fieldValue;
			if (!m_fieldValues.TryGetValue(field, out fieldValue))
				return;

			// Are we setting the phonetic value?
			if (fieldValue != m_phoneticValue)
				fieldValue.Value = value;
			else
			{
				SetPhoneticValue(value);
				ParsePhoneticValue();
			}
		}

		/// ------------------------------------------------------------------------------------
		public string GetField(string field)
		{
			return GetField(field, true);
		}

		/// ------------------------------------------------------------------------------------
		public string GetField(string field, bool deferToParentRecEntryWhenMissingValue)
		{
			if (field == null)
				return null;

			FieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue) && fieldValue.Value != null)
				return fieldValue.Value;

			// At this point, we know we don't have a value for the specified field or we
			// do and the value is null.
 
			if (field == PaField.kCVPatternFieldName)
			{
				// Check if the CV value is in the record cache. If so, return it.
				var recEntryVal = RecordEntry.GetValueBasic(field);
				if (deferToParentRecEntryWhenMissingValue && recEntryVal != null)
					return recEntryVal;

				// Build the CV pattern since it didn't come from the data source.
				return (m_phones == null || Project.PhoneCache == null ?
					null : Project.PhoneCache.GetCVPattern(m_phones));
			}
			
			// If deferToParentRecEntryWhenMissingValue is true then the value returned
			// is defered to the owning record entry's value for the field. Otherwise,
			// just return null.
			return (deferToParentRecEntryWhenMissingValue ? RecordEntry[field] : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the specified field to the first interlinear line in a block of interlinear
		/// fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetFieldAsFirstLineInterlinear(string field)
		{
			FieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue))
				fieldValue.IsFirstLineInterlinearField = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the specified field to be an interlinear field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetFieldAsSubordinateInterlinear(string field)
		{
			FieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue))
				fieldValue.IsSubordinateInterlinearField = true;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public object Tag { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlArray("Fields")]
		public List<FieldValue> FieldValues
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
		/// <summary>
		/// Gets the entry's phonetic value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string PhoneticValue
		{
			get { return m_phoneticValue == null ? null : m_phoneticValue.Value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the entry's phonetic value before any experimental transcription conversions
		/// were applied. If none were applied, then this value just returns the PhoneticValue.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string OriginalPhoneticValue
		{
			get { return m_origPhoneticValue ?? PhoneticValue; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the entry's phonetic value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string PhoneticValueWithPrimaryUncertainty
		{
			get
			{
				if (ContiansUncertainties && m_phones != null)
				{
					var bldr = new StringBuilder();
					foreach (var phone in m_phones)
						bldr.Append(phone);

					return bldr.ToString();
				}

				return m_phoneticValue == null ? null : m_phoneticValue.Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the entry's phonetic word contains
		/// uncertain phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool ContiansUncertainties
		{
			get { return m_uncertainPhones != null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of uncertain phones for the entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Dictionary<int, string[]> UncertainPhones
		{
			get { return m_uncertainPhones; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the index of the word within the record entry. When a record has only one
		/// phonetic (or phonemic, gloss, orthographic, part of speech, tone, cv pattern) then
		/// this will always be zero.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int WordIndex { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the record to which the word belongs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public RecordCacheEntry RecordEntry { get; internal set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PaProject Project
		{
			get { return RecordEntry.Project; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the array of phones that make up the phonetic word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] Phones
		{
			get
			{
				if (m_phones == null)
					ParsePhoneticValue();

				return m_phones;
			}
			
			internal set { m_phones = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of experimental transcriptions that were applied to the phonetic word
		/// of the cache entry.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Dictionary<string, string> AppliedExperimentalTranscriptions
		{
			get {return m_experimentalTranscriptionList;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the path to an entry's audio file when the path specified in the
		/// data source is a relative path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string AbsoluteAudioFilePath { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because deserialization cannot deserialize a dictionary, moving field values from
		/// the deserialized values list to a dictionary has to be done in a separate process.
		/// Also, it's important to set the phonetic value member variable and normalize its
		/// string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void PostDeserializeProcess()
		{
			if (m_fieldValuesList == null || m_fieldValuesList.Count == 0)
				return;

			m_fieldValues = new Dictionary<string, FieldValue>();
			
			foreach (var fieldValue in m_fieldValuesList)
			{
				m_fieldValues[fieldValue.Name] = fieldValue;

				if (m_phoneticValue == null)
				{
					var field = Project.GetFieldForName(fieldValue.Name);
					if (field.Type == FieldType.Phonetic)
					{
						m_phoneticValue = fieldValue;
						SetPhoneticValue(fieldValue.Value);
						ParsePhoneticValue();
					}
				}
			}

			m_fieldValuesList = null;
		}

		/// ------------------------------------------------------------------------------------
		private void SetPhoneticValue(string origPhonetic)
		{
			string phonetic = origPhonetic;

			if (!string.IsNullOrEmpty(phonetic))
			{
				// Normalize the phonetic string.
				phonetic = FFNormalizer.Normalize(phonetic);

				if (Project.TranscriptionChanges != null)
				{
					// Convert experimental transcriptions within the phonetic string.
					phonetic = Project.TranscriptionChanges.Convert(
						phonetic, out m_experimentalTranscriptionList);

					// Save this for displaying in the record view.
					if (origPhonetic != phonetic)
						m_origPhoneticValue = origPhonetic;
				}
			}

			m_phoneticValue.Value = phonetic;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the phonetic value into it's individual phones, gathering uncertain phones
		/// along the way.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ParsePhoneticValue()
		{
			if (m_phoneticValue.Value == null)
				return;

			if (App.IPASymbolCache.UndefinedCharacters != null)
			{
				var field = Project.Fields.SingleOrDefault(f => f.Type == FieldType.Reference);
				if (field != null)
					App.IPASymbolCache.UndefinedCharacters.CurrentReference = GetField(field.Name, true);

				App.IPASymbolCache.UndefinedCharacters.CurrentDataSourceName =
					(RecordEntry.DataSource.Type == DataSourceType.FW &&
					RecordEntry.DataSource.FwDataSourceInfo != null ?
					RecordEntry.DataSource.FwDataSourceInfo.ToString() :
					Path.GetFileName(RecordEntry.DataSource.SourceFile));
			}

			m_phones = Project.PhoneticParser.Parse(m_phoneticValue.Value,
				false, false, out m_uncertainPhones);
			
			if (m_phones != null && m_phones.Length == 0)
			{
				m_phones = null;
				m_uncertainPhones = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of all possible words in the entry if the entry contains
		/// uncertain phones. The words are returned as a two dimensional array of strings.
		/// The first array being an array of words and each array within those array elements
		/// is an array of the phones that make up the word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[][] GetAllPossibleUncertainWords(bool includeFormattingMarker)
		{
			if (m_uncertainPhones == null)
				return null;

			string formattingMarker = (includeFormattingMarker ? "|" : string.Empty);

			// Determine how many words will be returned.
			int totalWords = m_uncertainPhones.Values
				.Aggregate(1, (current, uncertainties) => current * uncertainties.Length);

			// Preallocate the a copy of all the words that will be returned. For now, each
			// preallocated copy will contain the same phones (i.e. the phones already in
			// the word's phone collection -- the uncertain ones being the first (or primary)
			// uncertain phone from the uncertainty list(s)).
			var unsortedInfo = new List<string[]>(totalWords);
			for (int w = 0; w < totalWords; w++)
			{
				unsortedInfo.Add(new string[m_phones.Length]);
				for (int i = 0; i < m_phones.Length; i++)
					unsortedInfo[w][i] = m_phones[i];
			}

			int dividend = totalWords;

			// Go through all the uncertain phones, stuffing them in the proper
			// locations in each of the words that will be returned.
			foreach (KeyValuePair<int, string[]> uncertainties in m_uncertainPhones)
			{
				// Number of consecutive words each phone in the current uncertainty group
				// is inserted into before moving to the next uncertain phone in the group.
				int consecutiveWords = dividend / uncertainties.Value.Length;

				int currWord = 0;
				while (currWord < totalWords)
				{
					foreach (string uncertainPhone in uncertainties.Value)
					{
						for (int i = 0; i < consecutiveWords; i++)
						{
							unsortedInfo[currWord][uncertainties.Key] = 
								formattingMarker + uncertainPhone;
							currWord++;
						}
					}
				}

				dividend = consecutiveWords;
			}

			return unsortedInfo.ToArray();
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the entry's phonetic value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return PhoneticValue;
		}
	}
}
