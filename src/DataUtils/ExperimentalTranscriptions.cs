using System;
using System.Collections.Generic;
using System.Text;
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
			string filename = (projectFileName == null ? string.Empty : projectFileName);
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
					if (experimentalTrans[i].Item == null)
						experimentalTrans.RemoveAt(i);
				}
			}

			return (experimentalTrans == null ?
				new ExperimentalTranscriptions() : experimentalTrans);
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
						info.CurrentTransToConvert != null)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the items to convert. The returned list's key is the ambiguous item
		/// and the key's value is what the item should be converted to.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Dictionary<string, string> ConversionList
		{
			get
			{
				if (!AnyExperimentalTransToConvert)
					return null;

				Dictionary<string, string> list = new Dictionary<string, string>();
				foreach (ExperimentalTrans item in this)
				{
					string convertToItem = item.CurrentTransToConvert;
					if (item.Item != null && convertToItem != null &&
						item.Convert && !item.TreatAsSinglePhone)
					{
						list[item.Item] = convertToItem;
					}
				}

				return (list.Count > 0 ? list : null);
			}
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
				if (experimentalTrans.Item == item)
				{
					return (itemMustBeTreatedAsSinglePhone ?
						experimentalTrans.TreatAsSinglePhone : true);
				}
			}

			return false;
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
		private string m_item;
		private List<string> m_convertToItems;
		private string m_currentConvertToItem;
		private bool m_convert = true;
		private bool m_treatAsSinglePhone = false;

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
			m_item = item;
			m_treatAsSinglePhone = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the experimental transcription to be converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Item
		{
			get { return m_item; }
			set { m_item = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current item to which the experimental transcription will be
		/// converted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string CurrentTransToConvert
		{
			get
			{
				if (!m_convert)
					return null;

				if (m_treatAsSinglePhone)
					return m_item;

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
		public List<string> TranscriptionsToConvert
		{
			get { return m_convertToItems; }
			set { m_convertToItems = value; }
		}
	}
}
