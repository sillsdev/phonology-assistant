using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using SilTools;

namespace SIL.Pa.DataSource.Sa
{
	/// ----------------------------------------------------------------------------------------
	public class SaAudioDocument
	{
		internal const float kCurrSaDocVersion = 2.0F;

		private static readonly List<string> m_genders = new List<string>(new [] {" ", "M", "F", "C"});
		
		internal SortedDictionary<uint, SegmentData> m_segments;
		private bool m_isForTmpOperation;
		private string m_audioFile;
		private string m_MD5HashCode;
		private string m_speakerName;
		private string m_serializedGender;
		internal float m_docVer;
		private static string s_audioFileLoading;

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("Audio File: {0},  MD5: {1}", m_audioFile, m_MD5HashCode);
		}

		/// ------------------------------------------------------------------------------------
		public SaAudioDocument()
		{
			SamplesPerSecond = -1;
			BitsPerSample = -1;
			BlockAlignment = -1;
			AverageBytesPerSecond = -1;
			Channels = -1;
			DataChunkSize = -1;
			NumberOfSamples = -1;

			CalcFreqLow = 40;
			CalcFreqHigh = 500;
			CalcVoicingThd = 40;
			CalcPercntChng = 32;
			CalcGrpSize = 6;
			CalcIntrpGap = 7;
			m_segments = new SortedDictionary<uint, SegmentData>();
		}

		/// ------------------------------------------------------------------------------------
		public SaAudioDocument(string audioFilePath) : this()
		{
			AudioFile = audioFilePath;
		}

		/// ------------------------------------------------------------------------------------
		public SaAudioDocument Clone()
		{
			var clone = new SaAudioDocument();

			clone.m_isForTmpOperation = m_isForTmpOperation;
			clone.DocVersion = m_docVer;
			clone.Segments = (SegmentData[])Segments.Clone();
			clone.AudioFile = m_audioFile;
			clone.MD5HashCode = m_MD5HashCode;
			clone.SpeakerName = m_speakerName;
			clone.SpeakerGender = m_serializedGender[0];
			clone.EthnologueId = EthnologueId;
			clone.Region = Region;
			clone.Country = Country;
			clone.Family = Family;
			clone.LanguageName = LanguageName;
			clone.Dialect = Dialect;
			clone.NoteBookReference = NoteBookReference;
			clone.FreeFormTranslation = FreeFormTranslation;
			clone.Transcriber = Transcriber;
			clone.SADescription = SADescription;
			clone.DataChunkSize = DataChunkSize;
			clone.FormatTag = FormatTag;
			clone.Channels = Channels;
			clone.SamplesPerSecond = SamplesPerSecond;
			clone.AverageBytesPerSecond = AverageBytesPerSecond;
			clone.BlockAlignment = BlockAlignment;
			clone.BitsPerSample = BitsPerSample;
			clone.SAFlags = SAFlags;
			clone.RecordFileFormat = RecordFileFormat;
			clone.RecordTimeStamp = RecordTimeStamp;
			clone.RecordBandWidth = RecordBandWidth;
			clone.RecordSampleSize = RecordSampleSize;
			clone.NumberOfSamples = NumberOfSamples;
			clone.SignalMax = SignalMax;
			clone.SignalMin = SignalMin;
			clone.SignalBandWidth = SignalBandWidth;
			clone.SignalEffSampSize = SignalEffSampSize;
			clone.CalcFreqLow = CalcFreqLow;
			clone.CalcFreqHigh = CalcFreqHigh;
			clone.CalcVoicingThd = CalcVoicingThd;
			clone.CalcPercntChng = CalcPercntChng;
			clone.CalcGrpSize = CalcGrpSize;
			clone.CalcIntrpGap = CalcIntrpGap;

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the transcription file for the specified audio file path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SaAudioDocument Load(string audioFilePath, bool isForTmpOperation,
			bool returnNullIfNoExist)
		{
			// Make sure the wave file exists.
			if (!File.Exists(audioFilePath))
			{
				SaAudioDocumentReader.ShowWaveFileNotFoundMsg(audioFilePath);				
				return null;
			}

			// Get what the file name should be for the transcription file.
			string transcriptionFile = GetTranscriptionFile(audioFilePath, isForTmpOperation);

			SaAudioDocument doc;

			// If it doesn't exist, it means the audio file doesn't have any transcriptions.
			if (!File.Exists(transcriptionFile))
			{
				if (returnNullIfNoExist)
					return null;

				doc = new SaAudioDocument(audioFilePath);
			}
			else
			{
				// Get the transcription data from the companion transcription file.
				Exception e;
				s_audioFileLoading = audioFilePath;
				doc = XmlSerializationHelper.DeserializeFromFile<SaAudioDocument>(transcriptionFile, out e);

				if (e != null)
				{
					var viewer = new ExceptionViewer(e);
					viewer.ShowDialog();
					return null;
				}

				s_audioFileLoading = null;
				doc.AudioFile = audioFilePath;
			}

			doc.m_docVer = kCurrSaDocVersion;
			doc.m_isForTmpOperation = isForTmpOperation;
			return doc;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the transcription file for the current document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Save()
		{
			return Save(m_audioFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the transcription file for the current document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool Save(string audioFile)
		{
			var tmpFile = Path.GetDirectoryName(audioFile);
			tmpFile = Path.Combine(tmpFile, "~#!tstWrite!#~");
			try
			{
				// Check to see if the folder and transcription file are writable
				File.WriteAllText(tmpFile, string.Empty);
				string transFilePath = Path.ChangeExtension(audioFile, ".saxml");
				if (File.Exists(transFilePath) &&
					((File.GetAttributes(transFilePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly))
				{
					throw new Exception();
				}
			}
			catch
			{
				// Warn the user that we don't have write privileges
				string audioFileNameOnly = Path.GetFileName(audioFile);
				string transFileNameOnly = Path.ChangeExtension(audioFileNameOnly, ".saxml");

				var msg = App.LocalizeString("ReadOnlyFolderMsg",
					"You are attempting to save the transcriptions for {0} in a location for which you do not have write access. Please specify another location in which to make a copy of {1} along with its transcription file ({2}).",
					App.kLocalizationGroupInfoMsg);
				
				msg = string.Format(msg, audioFileNameOnly, audioFileNameOnly, transFileNameOnly);
				Utils.MsgBox(Utils.ConvertLiteralNewLines(msg));

				using (var dlg = new FolderBrowserDialog())
				{

					// Ask the user for a different folder in which to write the transcription file.
					dlg.ShowNewFolderButton = true;
					if (dlg.ShowDialog() == DialogResult.Cancel)
						return false;

					// Construct the new path for the audio file based on the path the user chose.
					audioFile = Path.Combine(dlg.SelectedPath, audioFileNameOnly);
				}

				// Try again.
				Save(audioFile);
			}

			try
			{
				File.Delete(tmpFile);
			}
			catch { }

			// If the specified audio file doesn't exist, it means it's original
			// location is read-only. Therefore, copy it to the newly specified
			// location.
			if (!File.Exists(audioFile))
			{
				// Copy the audio file to the new path and save the new path.
				File.Copy(AudioFile, audioFile);
				AudioFile = audioFile;
			}

			return XmlSerializationHelper.SerializeToFile(TranscriptionFile, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates a byte value (assumed to be within the audio data portion of a wave
		/// file) to its equivalent value in seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static double BytesToSeconds(ulong byteVal, int channels, long samplesPerSec,
			int bitsPerSample)
		{
			int bytesPerSample = (bitsPerSample / 8) * channels;
			long bytesPerSecond = bytesPerSample * samplesPerSec;

			return (double)byteVal / bytesPerSecond;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the specified second value to a byte offset (and only understood
		/// by SA).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static uint SecondsToBytes(double seconds, int channels, long samplesPerSec,
			int bitsPerSample)
		{
			int bytesPerSample = (bitsPerSample / 8) * channels;
			long bytesPerSecond = bytesPerSample * samplesPerSec;

			return (uint)(Math.Round(seconds * bytesPerSecond,
				MidpointRounding.AwayFromZero));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates a byte value (assumed to be within the audio data portion of a wave
		/// file) to its equivalent value in seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public double BytesToSeconds(ulong byteVal)
		{
			// Make sure we have valid sampling rate, channels and bits per sample
			var audioFilePath = (m_audioFile != null || s_audioFileLoading == null) ?
				m_audioFile : s_audioFileLoading;
			
			if (SamplesPerSecond == -1)
			{
				if (File.Exists(audioFilePath))
					GetAudioFormatValues(audioFilePath);
				else
					return 0;
			}

			return BytesToSeconds(byteVal, Channels, SamplesPerSecond, BitsPerSample);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts the specified second value to a byte offset (and only understood
		/// by SA).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public uint SecondsToBytes(double seconds)
		{
			// Make sure we have valid sampling rate, channels and bits per sample
			var audioFilePath = (m_audioFile != null || s_audioFileLoading == null) ?
				m_audioFile : s_audioFileLoading;
			
			if (SamplesPerSecond == -1)
			{
				if (File.Exists(audioFilePath))
					GetAudioFormatValues(audioFilePath);
				else
					return 0;
			}

			return SecondsToBytes(seconds, Channels, SamplesPerSecond, BitsPerSample);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets format values from the audio file, such as sample rate, sample size, etc.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetAudioFormatValues(string audioFilePath)
		{
			FileStream stream = null;
			BinaryReader reader = null;

			try
			{
				stream = File.Open(audioFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				reader = new BinaryReader(stream, Encoding.ASCII);

				//Start at the beginning of fmt chunk plus advance eight 
				//positions to move ahead of the chunk ID and chunk size.
				stream.Position = AudioReader.GetChunkOffset(stream, AudioReader.kidFmtChunk) + 8;

				//Read the correct values that correspond with each category
				//of the fmt chunk, then write the information to the database.
				FormatTag = reader.ReadUInt16();
				Channels = reader.ReadUInt16();
				int samplesPerSecond = reader.ReadInt32();
				SamplesPerSecond = samplesPerSecond;
				AverageBytesPerSecond = reader.ReadInt32();
				BlockAlignment = reader.ReadUInt16();
				int bitsPerSample = reader.ReadUInt16();
				BitsPerSample = bitsPerSample;

				// Also set default record info
				RecordFileFormat = 1; // This is for wave files

				//Start at the beginning of data chunk plus advance  
				//four positions to move ahead of the chunk ID.
				stream.Position = AudioReader.GetChunkOffset(stream, AudioReader.kidDataChunk) + 4;

				//Read the data chunk size and write to the database.
				int dataChunkSize = reader.ReadInt32();
				DataChunkSize = dataChunkSize;

				//Calculate number of samples
				NumberOfSamples = 8 * dataChunkSize / bitsPerSample;

			}
			catch
			{
			}
			finally
			{
				if (stream != null)
					stream.Close();
	
				stream = null;
				reader = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the companion transcription file for the specified audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetTranscriptionFile(string audioFile, bool isForTmpOperation)
		{
			return Path.ChangeExtension(audioFile, isForTmpOperation ? ".saxml.tmp" : ".saxml");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the modified date/time for the transcription associated with the specified
		/// audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DateTime GetTranscriptionFileModifiedTime(string audioFile)
		{
			string transcriptionFile = GetTranscriptionFile(audioFile, false);

			return (File.Exists(transcriptionFile) ?
				File.GetLastWriteTimeUtc(transcriptionFile) : DateTime.MinValue);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public float DocVersion
		{
			get { return kCurrSaDocVersion; }
			set { m_docVer = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio file associated with the audio document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string AudioFile
		{
			get { return m_audioFile; }
			set
			{
				// Make sure the wave file exists.
				if (!File.Exists(value))
				{
					SaAudioDocumentReader.ShowWaveFileNotFoundMsg(value);
					return;
				}

				m_audioFile = value;
				MD5HashCode = AudioReader.GetMD5HashCode(value);
				GetAudioFormatValues(value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the companion transcription file for the document's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string TranscriptionFile
		{
			get { return GetTranscriptionFile(m_audioFile, m_isForTmpOperation); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the MD5 hash code of the audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string MD5HashCode
		{
			get {return m_MD5HashCode;}
			set	{m_MD5HashCode = value;}
		}

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the speaker's name.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public string SpeakerName
        {
            get { return m_speakerName; }
            set { m_speakerName = value; }
        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("SpeakerGender")]
		public string SerializedGender
		{
			get
			{
				return (m_serializedGender == null || 
					string.IsNullOrEmpty(m_serializedGender) || !m_genders.Contains(m_serializedGender) ?
					m_genders[0] : m_serializedGender);
			}
			set
			{
				var gender = (value ?? string.Empty);
				if (gender.Trim() == m_serializedGender)
					return;

				m_serializedGender = m_genders.Contains(gender.Trim()) ? gender.Trim() : m_genders[0];
			}
		}

        /// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the speaker's gender.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public char SpeakerGender
		{
			get {return (m_serializedGender == null ? m_genders[0][0] : m_serializedGender[0]);}
			set {SerializedGender = value.ToString();}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ethnologue Id of langauge.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EthnologueId { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the region in which langauge is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Region { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the country in which language is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Country { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the language family.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Family { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the language name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageName { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the dialect.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Dialect { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the note book reference.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NoteBookReference { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the free form translation
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FreeFormTranslation { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of person who transcribed the audio data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the size of the data chunk in the audio. (This length does not include any
		/// pad byte).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DataChunkSize { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int FormatTag { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Channels { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int SamplesPerSecond { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int AverageBytesPerSecond { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int BlockAlignment { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int BitsPerSample { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Description from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SADescription { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Flags from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SAFlags { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordFileFormat { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordTimeStamp { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordBandWidth { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordSampleSize { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int NumberOfSamples { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMax { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMin { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalBandWidth { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalEffSampSize { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqLow { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqHigh { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcVoicingThd { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcPercntChng { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcGrpSize { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcIntrpGap { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public long AudioFileCreateTime
		{
			get { return File.GetCreationTimeUtc(m_audioFile).Ticks; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the last modified time as a DateTime structure.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public DateTime LastModifiedAsDateTime
		{
			get { return File.GetLastWriteTimeUtc(TranscriptionFile); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the last modified time in ticks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public long LastModifiedTime
		{
			get { return LastModifiedAsDateTime.Ticks; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the last time the audio file's record was updated in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string HumanReadableLastModifiedTime
		{
			get { return LastModifiedAsDateTime.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the annotation segments.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SegmentData[] Segments
		{
			get
			{
				uint i = 0;
				SegmentData[] segments = new SegmentData[m_segments.Count];
				foreach (KeyValuePair<uint, SegmentData> seg in m_segments)
					segments[i++] = seg.Value;

				return segments;
			}
			set
			{
				m_segments.Clear();
				if (value == null)
					return;

				uint i = 0;
				foreach (SegmentData seg in value)
				{
					seg.SaAudioDocument = this;
					m_segments.Add(i++, seg);
				}
			}
		}

		#endregion
	}

	#region SegmentData Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Internal class to store information about a single segment in a audio file transcription.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SegmentData
	{
		// This is the first SaAudioDocument version number when offsets, durations
		// and mark durations were stored in segments as seconds rather than bytes.
		private const float kFirstVerWithByteSecondSwitch = 2.0f;

		private SaAudioDocument m_owningAudioDoc;
		double m_offsetInSeconds;
		double m_durationInSeconds;
		double m_markDurationInSeconds;

		/// ------------------------------------------------------------------------------------
		public SegmentData()
		{
		}

		/// ------------------------------------------------------------------------------------
		public SegmentData(SaAudioDocument owningAudioDoc)
		{
			SaAudioDocument = owningAudioDoc;
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SaAudioDocument SaAudioDocument
		{
			get { return m_owningAudioDoc; }
			set
			{
				m_owningAudioDoc = value;
				if (value != null && m_owningAudioDoc.m_docVer < kFirstVerWithByteSecondSwitch)
				{
					m_offsetInSeconds = m_owningAudioDoc.BytesToSeconds((ulong)m_offsetInSeconds);
					m_durationInSeconds = m_owningAudioDoc.BytesToSeconds((ulong)m_durationInSeconds);
					m_markDurationInSeconds = m_owningAudioDoc.BytesToSeconds((ulong)m_markDurationInSeconds);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Offset")]
		public double OffsetInSeconds
		{
			get { return m_offsetInSeconds; }
			set
			{
				// This setter is mainly used by the XML deserialization.
				m_offsetInSeconds = (m_owningAudioDoc == null ||
					m_owningAudioDoc.m_docVer >= kFirstVerWithByteSecondSwitch ? value :
					m_owningAudioDoc.BytesToSeconds((ulong)value));
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Duration")]
		public double DurationInSeconds
		{
			get { return m_durationInSeconds; }
			set
			{
				// This setter is mainly used by the XML deserialization.
				m_durationInSeconds = (m_owningAudioDoc == null ||
					m_owningAudioDoc.m_docVer >= kFirstVerWithByteSecondSwitch ? value :
					m_owningAudioDoc.BytesToSeconds((ulong)value));
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("MarkDuration")]
		public double MarkDurationInSeconds
		{
			get { return m_markDurationInSeconds; }
			set
			{
				// This setter is mainly used by the XML deserialization.
				m_markDurationInSeconds = (m_owningAudioDoc == null ||
					m_owningAudioDoc.m_docVer >= kFirstVerWithByteSecondSwitch ? value :
					m_owningAudioDoc.BytesToSeconds((ulong)value));
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public uint Offset
		{
			get { return (m_owningAudioDoc == null ? 0 : m_owningAudioDoc.SecondsToBytes(m_offsetInSeconds)); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public uint Duration
		{
			get { return (m_owningAudioDoc == null ? 0 : m_owningAudioDoc.SecondsToBytes(m_durationInSeconds)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will only be non zero when the segment is the first in a word.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public uint MarkDuration
		{
			get { return (m_owningAudioDoc == null ? 0 : m_owningAudioDoc.SecondsToBytes(m_markDurationInSeconds)); }
		}

		/// <summary></summary>
		[XmlAttribute]
		public bool IsBookmark;

		/// <summary></summary>
		public string Phonetic;

		/// <summary></summary>
		public string Phonemic;

		/// <summary></summary>
		public string Tone;

		/// <summary></summary>
		public string Orthographic;

		/// <summary>This will only contain a value when the segment is the first in a word.</summary>
		public string PartOfSpeech;

		/// <summary>This will only contain a value when the segment is the first in a word.</summary>
		public string Gloss;

		/// <summary>This will only contain a value when the segment is the first in a word.</summary>
		public string Reference;
	}

	#endregion
}
