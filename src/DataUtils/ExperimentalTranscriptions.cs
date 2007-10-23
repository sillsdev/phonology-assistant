using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Data
{
	public class ExperimentalTranscriptions : List<ExperimentalTrans>
	{
		public const string kExperimentalTransFile = "ExperimentalTranscriptions.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Construct the file name for the project's experimental transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string BuildFileName(string projectFileName)
		{
			string filename = (projectFileName ?? string.Empty);
			filename += (filename.EndsWith(".") ? string.Empty : ".") + kExperimentalTransFile;
			return filename;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the project's experimental transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ExperimentalTranscriptions Load(string projectFileName)
		{
			string filename = BuildFileName(projectFileName);
			
			ExperimentalTranscriptions experimentalTrans = STUtils.DeserializeData(filename,
				typeof(ExperimentalTranscriptions)) as ExperimentalTranscriptions;

			// This should never need to be done, but just in case there are entries in
			// the list whose source transcription (i.e. Item) is null, remove them from
			// the list since those entries will cause problems further down the line.
			if (experimentalTrans != null)
			{
				for (int i = experimentalTrans.Count - 1; i >= 0; i--)
				{
					if (experimentalTrans[i].ConvertFromItem == null)
						experimentalTrans.RemoveAt(i);
				}
			}

			return (experimentalTrans ?? new ExperimentalTranscriptions());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the project's experimental transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string projectFileName)
		{
			string filename = BuildFileName(projectFileName);
			STUtils.SerializeData(filename, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not there are any experimental transcriptions
		/// defined.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AnyExperimentalTrans
		{
			get { return (Count > 0); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not there are any experimental transcriptions
		/// that should be converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AnyExperimentalTransToConvert
		{
			get
			{
				foreach (ExperimentalTrans info in this)
				{
					if (info.Convert && !info.TreatAsSinglePhone &&
						info.CurrentConvertToItem != null)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the items to convert. The returned list's key is what to convert
		/// from and the key's value is what the item should be converted to.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Dictionary<string, string> ConversionList
		{
			get
			{
				if (!AnyExperimentalTransToConvert)
					return null;

				// First, sort the list of items to convert on the length of the item
				// to convert to -- longest to shortest.
				List<ExperimentalTrans> tmpList = new List<ExperimentalTrans>();

				// Copy the ExperimentalTrans references.
				for (int i = 0; i < Count; i++)
				{
					this[i].DisplayIndex = i;
					tmpList.Add(this[i]);
				}

				// Now order the items.
				tmpList.Sort(ExperimentalTransComparer);

				// Now put the sorted items in a list whose keys are what
				// to convert from and whose values are what to convert to.
				Dictionary<string, string> list = new Dictionary<string, string>();
				foreach (ExperimentalTrans item in tmpList)
				{
					string convertToItem = item.CurrentConvertToItem;
					if (item.ConvertFromItem != null && convertToItem != null &&
						item.Convert && !item.TreatAsSinglePhone)
					{
						list[item.ConvertFromItem] = convertToItem;
					}
				}
				
				return (list.Count > 0 ? list : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Compare method for the length of the item to convert from.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int ExperimentalTransComparer(ExperimentalTrans x, ExperimentalTrans y)
		{
			if (x == y || ((x == null || x.ConvertFromItem == null) &&
				(y == null || y.ConvertFromItem == null)))
			{
				return 0;
			}

			if (x == null || x.ConvertFromItem == null)
				return 1;

			if (y == null || y.ConvertFromItem == null)
				return -1;

			// For items of the same length, this will preserve the order in
			// which the user entered the items in the Phone Inventory view.
			if (x.ConvertFromItem.Length == y.ConvertFromItem.Length)
				return x.DisplayIndex.CompareTo(y.DisplayIndex);

			return -(x.ConvertFromItem.Length.CompareTo(y.ConvertFromItem.Length));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating if the specified item is in the collection of experimental
		/// transcriptions to convert. When itemMustBeTreatedAsSinglePhone is true, it means
		/// that a match will only be returned if the item is found and the ExperimentalTrans
		/// object to which it belongs has its TreatAsSinglePhone flag set to true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ContainsItem(string item, bool itemMustBeTreatedAsSinglePhone)
		{
			foreach (ExperimentalTrans experimentalTrans in this)
			{
				if (experimentalTrans.ConvertFromItem == item)
				{
					return (itemMustBeTreatedAsSinglePhone ?
						experimentalTrans.TreatAsSinglePhone : true);
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts all the specified text to contain the experimental transcriptions that
		/// are marked for converting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Convert(string text)
		{
			Dictionary<string, string> actualConversions;
			return Convert(text, out actualConversions);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts all the specified text to contain the experimental transcriptions that
		/// are marked for converting.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Convert(string text, out Dictionary<string, string> actualConversions)
		{
			if (ConversionList == null || ConversionList.Count == 0)
			{
				actualConversions = null;
				return text;
			}

			char token = (char)1;
			Dictionary<char, KeyValuePair<string, string>> transAndMarkerInfo =
				new Dictionary<char, KeyValuePair<string, string>>();

			// This loop will go through the phonetic string and replace each occurance of
			// an experimental transcription with a single character token. Each experimental
			// transcription receives a unique token which is later replaced by the experimantal
			// transcription itself.
			foreach (KeyValuePair<string, string> kvp in ConversionList)
			{
				if (kvp.Key != null && kvp.Value != null &&
					text.IndexOf(kvp.Key/*, StringComparison.Ordinal*/) >= 0)
				{
					text = text.Replace(kvp.Key, token.ToString());
					transAndMarkerInfo[token] = kvp;
					token++;
				}
			}

			actualConversions = new Dictionary<string, string>();
			
			// Now replace each token with it's experimental transcription
			// and build a list of the conversions that were actually made.
			foreach (KeyValuePair<char, KeyValuePair<string, string>> kvp in transAndMarkerInfo)
			{
				if (text.IndexOf(kvp.Key) >= 0)
				{
					string convertTo = kvp.Value.Value;
					text = text.Replace(kvp.Key.ToString(), convertTo);
					actualConversions[kvp.Value.Key] = convertTo;
				}
			}

			if (actualConversions.Count == 0)
				actualConversions = null;

			return text;
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("ExperimentalTranscription")]
	public class ExperimentalTrans
	{
		private string m_convertFromItem;
		private List<string> m_convertToItems;
		private string m_currentConvertToItem;
		private bool m_convert = true;
		private bool m_treatAsSinglePhone = false;
		private int m_displayIndex;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExperimentalTrans()
		{
			m_convertToItems = new List<string>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates an experimental transcription with the specified item that will be treated
		/// as a single phone. That means the object's TreatAsSinglePhone flag will be set to
		/// true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ExperimentalTrans(string item) : this()
		{
			m_convertFromItem = item;
			m_treatAsSinglePhone = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the experimental transcription to be converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string ConvertFromItem
		{
			get { return m_convertFromItem; }
			set { m_convertFromItem = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current item to which the experimental transcription will be
		/// converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string CurrentConvertToItem
		{
			get
			{
				if (!m_convert)
					return null;

				if (m_treatAsSinglePhone)
					return m_convertFromItem;

				return (m_convertToItems.Contains(m_currentConvertToItem) ?
					m_currentConvertToItem : null);
			}
			set { m_currentConvertToItem = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the experimental transcription will
		/// be converted when data sources are read. This value can turned on and off to
		/// provide a way for the user to store experimental transcriptions to be converted
		/// while controlling when conversion takes place. For example, the user may
		/// temporarily want to see the source data unconverted, then return to analyzing
		/// converted data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool Convert
		{
			get { return m_convert; }
			set { m_convert = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the item to convert is not really
		/// to be converted to anything but rather forced to be recognized as a single phone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool TreatAsSinglePhone
		{
			get { return m_treatAsSinglePhone; }
			set { m_treatAsSinglePhone = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of possible items to which the experimental transcription
		/// will be converted. (The setting is mainly for XML deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("TranscriptionToConvert")]
		public List<string> TranscriptionsToConvertTo
		{
			get { return m_convertToItems; }
			set { m_convertToItems = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		internal int DisplayIndex
		{
			get { return m_displayIndex; }
			set { m_displayIndex = value; }
		}
	}
}
