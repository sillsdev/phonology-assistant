using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Reflection;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;

namespace SIL.Pa
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
		private Dictionary<string, PaFieldValue> m_fieldValues;
		private PaFieldValue m_phoneticValue;
		private int m_wordIndex;
		private Dictionary<int, string[]> m_uncertainPhones = null;
		private RecordCacheEntry m_recEntry;
		private string[] m_phones;
		private string m_absoluteAudioFilePath;

		// This is only used for deserialization
		private List<PaFieldValue> m_fieldValuesList;

		// This conversion list is specific to each WordCacheEntry list and contains only
		// those conversions that were applied to the phonetic word. Conversions contained
		// in s_experimentalTranscriptionList that were not applied to the phonetic word
		// do not get added to this list. This list exists so each cache entry knows what
		// experimental transcription information to display in an experimental transcription
		// popup for the user.
		private Dictionary<string, string> m_experimentalTranscriptionList = null;

		// This is the conversion list specified by the user in the experimental transcription
		// list of the phone inventory view. It is set before data sources for a project are
		// loaded and cleared after loading. This list is global for all WordCacheEntry objects.
		private static Dictionary<string, string> s_experimentalTranscriptionList = null;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a list of the from -> to experimental transcription conversions.
		/// This list should only be non null while data sources are being read during
		/// project loading. After project loading, the value is set to null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Dictionary<string, string> ExperimentalTranscriptionList
		{
			get { return s_experimentalTranscriptionList; }
			set { s_experimentalTranscriptionList = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordCacheEntry()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordCacheEntry(bool allocateSpaceForFieldValues)
		{
			if (!allocateSpaceForFieldValues)
				return;

			m_fieldValues = new Dictionary<string, PaFieldValue>();

			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
			{
				if (fieldInfo.IsParsed)
				{
					m_fieldValues[fieldInfo.FieldName] = new PaFieldValue(fieldInfo.FieldName);
					if (fieldInfo.IsPhonetic)
						m_phoneticValue = m_fieldValues[fieldInfo.FieldName];
				}
			}
		}

		#region Methods and Indexers for getting and setting field values
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
			PaFieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue))
			{
				// Are we setting the phonetic value?
				if (fieldValue != m_phoneticValue)
					fieldValue.Value = value;
				else if (value != null)
				{
					// We're setting the phonetic value so normalize
					// the string and convert any experimental transcriptions.
					fieldValue.Value = ConvertExperimentalTranscriptions(FFNormalizer.Normalize(value));

					// Check if it contains uncertain phones. If so, then force immediate
					// parsing to make sure the list of uncertain phones is ready when
					// this entry is displayed in a word list grid.
					if (value.IndexOfAny("(".ToCharArray()) >= 0)
						ParsePhoneticValue();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetField(string field)
		{
			return GetField(field, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetField(string field, bool deferToParentRecEntryWhenMissingValue)
		{
			if (field == null)
				return null;

			PaFieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue))
			{
				if (fieldValue.Value != null)
					return fieldValue.Value;
			}

			// At this point, we know we don't have a value for the specified field or we
			// do and the value is null.
 
			// If we're after the CV pattern then build it since it didn't come from the
			// data source.
			if (PaApp.Project != null)
			{
				PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[field];
				if (fieldInfo != null && fieldInfo.IsCVPattern)
					return (m_phones == null ? null : PaApp.PhoneCache.GetCVPattern(m_phones));
			}
			
			// If deferToParentRecEntryWhenMissingValue is true then the value returned
			// is defered to the owning record entry's value for the field. Otherwise,
			// just return null.
			return (deferToParentRecEntryWhenMissingValue ? m_recEntry[field] : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the specified field to the first interlinear line in a block of interlinear
		/// fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetFieldAsFirstLineInterlinear(string field)
		{
			PaFieldValue fieldValue;
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
			PaFieldValue fieldValue;
			if (m_fieldValues.TryGetValue(field, out fieldValue))
				fieldValue.IsSubordinateInterlinearField = true;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
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
		public int WordIndex
		{
			get { return m_wordIndex; }
			set { m_wordIndex = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the record to which the word belongs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public RecordCacheEntry RecordEntry
		{
			get { return m_recEntry; }
			internal set {m_recEntry = value;}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the manner of articulation key for the entry.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlIgnore]
		//public string MOAKey
		//{
		//    get
		//    {
		//        if (m_moaKey == null)
		//        {
		//            // TODO: When chow characters are supported, figure out how to deal with them.
		//            if (m_phoneticValue.Value == null)
		//                return "0";

		//            m_moaKey = DataUtils.GetMOAKey(Phones);
		//        }
				
		//        return m_moaKey;
		//    }
			
		//    set { m_moaKey = value; }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the place of articulation key for the entry.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlIgnore]
		//public string POAKey
		//{
		//    get
		//    {
		//        if (m_poaKey == null)
		//        {
		//            // TODO: When chow characters are supported, figure out how to deal with them.
		//            if (m_phoneticValue.Value == null)
		//                return "0";

		//            m_poaKey = DataUtils.GetPOAKey(Phones);
		//        }
				
		//        return m_poaKey;
		//    }

		//    set { m_poaKey = value; }
		//}

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
		public string AbsoluteAudioFilePath
		{
			get { return m_absoluteAudioFilePath; }
			set { m_absoluteAudioFilePath = value; }
		}
		
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

			m_fieldValues = new Dictionary<string, PaFieldValue>();
			foreach (PaFieldValue fieldValue in m_fieldValuesList)
			{
				m_fieldValues[fieldValue.Name] = fieldValue;

				if (m_phoneticValue == null)
				{
					PaFieldInfo fieldInfo = PaApp.FieldInfo[fieldValue.Name];
					if (fieldInfo.IsPhonetic)
					{
						m_phoneticValue = fieldValue;

						if (m_phoneticValue.Value != null)
						{
							// Normalize the phonetic string and convert experimental transcriptions.
							m_phoneticValue.Value = ConvertExperimentalTranscriptions(
								FFNormalizer.Normalize(fieldValue.Value));

							// Check if the phonetic value contains uncertain phones. If so,
							// then parse into phones to make sure the list of uncertain phones
							// is ready when this entry is displayed in a word list grid.
							if (m_phoneticValue.Value.IndexOfAny("(".ToCharArray()) >= 0)
								ParsePhoneticValue();
						}
					}
				}
			}

			m_fieldValuesList = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts all experimental transcriptions in the specified word and returns the
		/// word with all the conversions applied.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string ConvertExperimentalTranscriptions(string phonetic)
		{
			if (s_experimentalTranscriptionList != null)
			{
				foreach (KeyValuePair<string, string> convertItem in s_experimentalTranscriptionList)
				{
					// Does the phonetic string contain the item to be converted?
					if (phonetic.IndexOf(convertItem.Key) >= 0)
					{
						// At this point, we know we need to make a conversion. So make sure
						// we have a list to store this conversion.
						if (m_experimentalTranscriptionList == null)
							m_experimentalTranscriptionList = new Dictionary<string, string>();

						// Save the information for this conversion and do the conversion.
						m_experimentalTranscriptionList[convertItem.Key] = convertItem.Value;
						phonetic = phonetic.Replace(convertItem.Key, convertItem.Value);
					}
				}
			}

			return phonetic;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the phonetic value into it's individual phones, gathering uncertain phones
		/// along the way.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ParsePhoneticValue()
		{
			if (PaApp.UndefinedCodepoints != null)
			{
				PaApp.UndefinedCodepoints.SourceName =
					(RecordEntry.DataSource.DataSourceType == DataSourceType.FW &&
					RecordEntry.DataSource.FwDataSourceInfo != null ?
					RecordEntry.DataSource.FwDataSourceInfo.ToString() :
					System.IO.Path.GetFileName(RecordEntry.DataSource.DataSourceFile));
			}

			m_phones = IPACharCache.PhoneticParser(m_phoneticValue.Value, false,
				out m_uncertainPhones);

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
			int totalWords = 1;
			foreach (KeyValuePair<int, string[]> uncertainties in m_uncertainPhones)
				totalWords *= uncertainties.Value.Length;

			// Preallocate the a copy of all the words that will be returned. For now, each
			// preallocated copy will contain the same phones (i.e. the phones already in
			// the word's phone collection -- the uncertain ones being the first (or primary)
			// uncertain phone from the uncertainty list(s)).
			List<string[]> unsortedInfo = new List<string[]>(totalWords);
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
