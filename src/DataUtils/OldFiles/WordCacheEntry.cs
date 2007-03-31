using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Reflection;
using SIL.SpeechTools.Utils;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class contains a sinlge item in a word cache. There is only one word cache per
	/// instance of PA and the main word list window's grid for a project shows entries
	/// corresponding to the word cache. Each word cache entry contains a reference to the
	/// underlying RecordCacheEntry to which it's associated. Many of the properties of a
	/// WordCacheEntry return references to the underlying RecordCacheEntry. There is a
	/// one-to-many relationship between RecordCacheEntry and WordListEntry's - potentially,
	/// one RecordCacheEntry to severl WordListEntry objects.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WordCacheEntry
	{
		private List<PaFieldValue> m_fieldValuesList;
		private Dictionary<string, PaFieldValue> m_fieldValuesTable;
		private PaFieldValue m_phoneticValue;

		private int m_wordIndex;
		private RecordCacheEntry m_recEntry;
		private string m_moaKey;
		private string m_poaKey;
		private string[] m_phones;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordCacheEntry()
		{
			m_fieldValuesList = new List<PaFieldValue>();
			m_fieldValuesTable = new Dictionary<string, PaFieldValue>();

			int i = 0;
			foreach (PaFieldInfo fieldInfo in DBUtils.FieldInfo)
			{
				if (fieldInfo.IsParsed)
				{
					PaFieldValue value = new PaFieldValue();
					value.Name = fieldInfo.FieldName;
					value.Index = i++;
					m_fieldValuesList.Add(value);
					m_fieldValuesTable[value.Name] = value;

					if (fieldInfo.IsPhonetic)
						m_phoneticValue = value;
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
			get { return GetField(field); }
			set { SetValue(field, value); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string this[int fieldIndex]
		{
			get { return GetField(fieldIndex); }
			set { SetValue(fieldIndex, value); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the specified field with the specified value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetValue(string field, string value)
		{
			if (m_fieldValuesTable.ContainsKey(field))
			{
				PaFieldValue fieldValue = m_fieldValuesTable[field];
				fieldValue.Value = (fieldValue != m_phoneticValue ?
					value : value.Normalize(NormalizationForm.FormD));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set the specified field with the specified value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetValue(int fieldIndex, string value)
		{
			if (fieldIndex < m_fieldValuesList.Count && fieldIndex >= 0)
			{
				PaFieldValue fieldValue = m_fieldValuesList[fieldIndex];
				fieldValue.Value = (fieldValue != m_phoneticValue ?
					value : value.Normalize(NormalizationForm.FormD));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetField(string field)
		{
			return (m_fieldValuesTable.ContainsKey(field) ?
				m_fieldValuesTable[field].Value : m_recEntry[field]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetField(int fieldIndex)
		{
			return (fieldIndex < m_fieldValuesList.Count && fieldIndex >= 0 ?
				m_fieldValuesList[fieldIndex].Value : m_recEntry[fieldIndex]);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("ParsedFields")]
		public List<PaFieldValue> FieldValues
		{
			get { return m_fieldValuesList; }
			set { m_fieldValuesList = value; }
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
			internal set { m_recEntry = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string MOAKey
		{
			get
			{
				if (m_moaKey == null)
				{
					// TODO: When chow characters are supported, figure out how to deal with them.
					if (m_phoneticValue.Value == null)
						return "0";

					StringBuilder keybldr = new StringBuilder(m_phoneticValue.Value.Length * 3);
					foreach (char c in m_phoneticValue.Value)
					{
						IPACharInfo info = DBUtils.IPACharCache[c];
						keybldr.Append(info == null ? "000" :
							string.Format("{0:X3}", info.MOArticulation));
					}

					m_moaKey = keybldr.ToString();
				}
				
				return m_moaKey;
			}
			
			set { m_moaKey = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string POAKey
		{
			get
			{
				if (m_poaKey == null)
				{
					// TODO: When chow characters are supported, figure out how to deal with them.
					if (m_phoneticValue.Value == null)
						return "0";

					StringBuilder keybldr = new StringBuilder(m_phoneticValue.Value.Length * 3);
					foreach (char c in m_phoneticValue.Value)
					{
						IPACharInfo info = DBUtils.IPACharCache[c];
						keybldr.Append(info == null ? "000" :
							string.Format("{0:X3}", info.POArticulation));
					}

					m_poaKey = keybldr.ToString();
				}
				
				return m_poaKey;
			}

			set { m_poaKey = value; }
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
				{
					m_phones = IPACharCache.PhoneticParser(m_phoneticValue.Value);
					if (m_phones.Length == 0)
						m_phones = null;
				}

				return m_phones;
			}
			
			internal set { m_phones = value; }
		}

		#endregion
	}
}
