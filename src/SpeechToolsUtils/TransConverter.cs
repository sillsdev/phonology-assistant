using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using SilEncConverters22;
using System;
using SilUtils;

namespace SIL.SpeechTools.Utils
{
	[XmlType("TransConverterInfo")]
	public class TransConverterInfo : List<TransConverter>
	{
		private const string kdefaultEticEmicConverter = "ASAP>Unicode";
		private const string kdefaultEticEmicConverterFilename = "Asap2Unicode.tec";
		private const string kPersistedInfoFilename = "SaTransConverters.xml";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to the SIL software folder within the user's "My Documents" folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string SASettingsPath
		{
			get
			{
				string saSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
				saSettingsPath = Path.Combine(saSettingsPath, @"SIL\Speech Analyzer");

				return saSettingsPath;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the transcription converter object having the specified name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TransConverter this[string chunkId]
		{
			get
			{
				foreach (TransConverter converter in this)
				{
					if (converter.ChunkId == chunkId)
						return converter;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes the converter information for each SA transcription.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static TransConverterInfo Load()
		{
			string filename = Path.Combine(SASettingsPath,
				kPersistedInfoFilename);
			
			var info = XmlSerializationHelper.DeserializeFromFile<TransConverterInfo>(filename) ?? new TransConverterInfo();

			foreach (TransConverter converter in info)
				info.EnsureConverterExistsForId(converter.ChunkId, converter.TransName);

			return info;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Serializes the converter information for each SA transcription.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			string filename = Path.Combine(SASettingsPath, kPersistedInfoFilename);
			XmlSerializationHelper.SerializeToFile(filename, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure the encoding converter exists, if specified, and that phonetic and 
		/// phonemic converters are specified in this collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void EnsureConverterExistsForId(string chunkId, string transName)
		{
			TransConverter transConverter = this[chunkId];

			// If the transcription converter object couldn't be found for the specified 
			// transcription, then add one.
			if (transConverter == null)
			{
				transConverter = new TransConverter();
				transConverter.ChunkId = chunkId;
				transConverter.TransName = transName;
				Add(transConverter);
				Save();
			}

			// If the converter for the transcription has already been specified, 
			// then assume it's good.
			if (!string.IsNullOrEmpty(transConverter.Converter))
			{
				if (EnsureConverterExists(transConverter))
					return;
			}

			// Don't do any further checking for non-etic/emic fields
			if (!((chunkId == "etic") || (chunkId == "emic")))
				return;

			// If the transcription's converter is unspecified, check if the default etic/emic 
			// converter exists in the repository and assign it to the transcription.
			TransConverter defConverter = new TransConverter();
			defConverter.Converter = kdefaultEticEmicConverter;
			defConverter.Filename = kdefaultEticEmicConverterFilename;
			if (EnsureConverterExists(defConverter))
			{
				// Save the default converter's mapping file name in the transcription converter 
				// as the transcription converter's encoding converter name.
				transConverter = defConverter;
				Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure the encoding converter exists. Otherwise attempt to add it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool EnsureConverterExists(TransConverter transConverter)
		{
			// Check if the converter exists in the repository and is assigned to 
			// the transcription if the transcription's converter is unspecified.
			EncConverter converter = GetConverter(transConverter.Converter);

			if (converter == null)
			{
				// If the converter doesn't exist in the repository try the filename.
				converter = GetConverter(transConverter.Filename);

				if (converter != null)
				{
					transConverter.Converter = transConverter.Filename;
					Save();
				}
				else
				{
					// If that doesn't exist either add the converter
					string converterFilename =
						Path.Combine(SASettingsPath, transConverter.Filename);
					if (!File.Exists(converterFilename))
						return false;
					EncodingConverters.Add(transConverter.Converter, converterFilename,
						ConvType.Unknown, string.Empty, string.Empty, ProcessTypeFlags.DontKnow);
				}
			}

			return true;
		}

		private static EncConverters s_ec = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of encoding converters from the encoding converter repository
		/// installed on the computer. The setter is only for setting it to null;
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static EncConverters EncodingConverters
		{
			get
			{
				if (s_ec == null)
					s_ec = new EncConverters();

				return s_ec;
			}
			set
			{
				if (value == null)
					s_ec = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the encoding converter for the specified mapping (or encoding converter name).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static EncConverter GetConverter(string mapping)
		{
			if (EncodingConverters == null)
				return null;

			return (EncodingConverters[mapping] as EncConverter);
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TransConverter
	{
		[XmlAttribute("transcription")]
		public string TransName;
		[XmlAttribute("converter")]
		public string Converter;
		[XmlAttribute("filename")]
		public string Filename;
		[XmlAttribute("chunkid")]
		public string ChunkId;
	}
}
