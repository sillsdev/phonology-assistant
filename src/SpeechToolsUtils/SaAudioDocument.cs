using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using SIL.SpeechTools.Utils.Properties;

namespace SIL.SpeechTools.Utils
{
	#region MusicSegmentKey class

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Key class for music segments. 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class MusicSegmentKey : IComparable
	{
		private uint m_phraseLevel;
		private uint m_offset;

		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Constructor for MusicSegmentKey class. 
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		public MusicSegmentKey()
		{
			m_phraseLevel = 1;
			m_offset = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Phrase Level of transcription.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public uint PhraseLevel
		{
			get { return m_phraseLevel; }
			set { m_phraseLevel = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Offset into audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public uint Offset
		{
			get { return m_offset; }
			set { m_offset = value; }
		}

		#region IComparable Members

		int IComparable.CompareTo(object obj)
		{
			MusicSegmentKey key = obj as MusicSegmentKey;

			if (key == null)
				return 1;

			// sort by PhraseLevel first...
			if (key.PhraseLevel > m_phraseLevel)
				return -1;

			if (key.PhraseLevel < m_phraseLevel)
				return 1;

			// ...then by Offset
			if (key.Offset > m_offset)
				return -1;

			if (key.Offset < m_offset)
				return 1;

			return 0; // the objects are the same
		}

		#endregion
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SaAudioDocument
	{
		public const float kCurrSaDocVersion = 2.0F;

		private static List<string> m_genders = new List<string>(new string[] {" ", "M", "F", "C"});
		
		internal SortedDictionary<uint, SegmentData> m_segments;
		internal SortedDictionary<MusicSegmentKey, MusicSegmentData> m_musicSegments;
		private bool m_isForTmpOperation = false;
		private string m_audioFile;
		private string m_MD5HashCode;
		private string m_speakerName;
		private string m_serializedGender;
		private string m_ethnologueId;
		private string m_region;
		private string m_country;
		private string m_family;
		private string m_languageName;
		private string m_dialect;
		private string m_noteBookRef;
		private string m_freeFormTrans;
		private string m_transcriber;
		private string m_saDescription;
		private int m_dataChunkSize;
		private int m_formatTag;
		private int m_channels;
		private int m_samplesPerSecond;
		private int m_averageBytesPerSecond;
		private int m_blockAlignment;
		private int m_bitsPerSample;
		private int m_saFlags;
		private int m_recordFileFormat;
		private int m_recordTimeStamp;
		private int m_recordBandWidth;
		private int m_recordSampleSize;
		private int m_numberOfSamples;
		private int m_signalMax;
		private int m_signalMin;
		private int m_signalBandWidth;
		private int m_signalEffSampSize;
		private int m_calcFreqLow;
		private int m_calcFreqHigh;
		private int m_calcVoicingThd;
		private int m_calcPercntChng;
		private int m_calcGrpSize;
		private int m_calcIntrpGap;
		internal float m_docVer;
		private static string s_audioFileLoading;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			string fmt = "Audio File: {0},  MD5: {1}";
			return string.Format(fmt, m_audioFile, m_MD5HashCode);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaAudioDocument()
		{
			m_samplesPerSecond = -1;
			m_bitsPerSample = -1;
			m_blockAlignment = -1;
			m_averageBytesPerSecond = -1;
			m_channels = -1;
			m_dataChunkSize = -1;
			m_numberOfSamples = -1;

			m_calcFreqLow = 40;
			m_calcFreqHigh = 500;
			m_calcVoicingThd = 40;
			m_calcPercntChng = 32;
			m_calcGrpSize = 6;
			m_calcIntrpGap = 7;
			m_segments = new SortedDictionary<uint, SegmentData>();
			m_musicSegments = new SortedDictionary<MusicSegmentKey, MusicSegmentData>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaAudioDocument(string audioFilePath) : this()
		{
			AudioFile = audioFilePath;
		}

		/// ----------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaAudioDocument Clone()
		{
			SaAudioDocument clone = new SaAudioDocument();

			clone.m_isForTmpOperation = m_isForTmpOperation;
			clone.DocVersion = m_docVer;
			clone.Segments = (SegmentData[])this.Segments.Clone();
			clone.MusicSegments = (MusicSegmentData[])this.MusicSegments.Clone();
			clone.AudioFile = m_audioFile;
			clone.MD5HashCode = m_MD5HashCode;
			clone.SpeakerName = m_speakerName;
			clone.SpeakerGender = m_serializedGender[0];
			clone.EthnologueId = m_ethnologueId;
			clone.Region = m_region;
			clone.Country = m_country;
			clone.Family = m_family;
			clone.LanguageName = m_languageName;
			clone.Dialect = m_dialect;
			clone.NoteBookReference = m_noteBookRef;
			clone.FreeFormTranslation = m_freeFormTrans;
			clone.Transcriber = m_transcriber;
			clone.SADescription = m_saDescription;
			clone.DataChunkSize = m_dataChunkSize;
			clone.FormatTag = m_formatTag;
			clone.Channels = m_channels;
			clone.SamplesPerSecond = m_samplesPerSecond;
			clone.AverageBytesPerSecond = m_averageBytesPerSecond;
			clone.BlockAlignment = m_blockAlignment;
			clone.BitsPerSample = m_bitsPerSample;
			clone.SAFlags = m_saFlags;
			clone.RecordFileFormat = m_recordFileFormat;
			clone.RecordTimeStamp = m_recordTimeStamp;
			clone.RecordBandWidth = m_recordBandWidth;
			clone.RecordSampleSize = m_recordSampleSize;
			clone.NumberOfSamples = m_numberOfSamples;
			clone.SignalMax = m_signalMax;
			clone.SignalMin = m_signalMin;
			clone.SignalBandWidth = m_signalBandWidth;
			clone.SignalEffSampSize = m_signalEffSampSize;
			clone.CalcFreqLow = m_calcFreqLow;
			clone.CalcFreqHigh = m_calcFreqHigh;
			clone.CalcVoicingThd = m_calcVoicingThd;
			clone.CalcPercntChng = m_calcPercntChng;
			clone.CalcGrpSize = m_calcGrpSize;
			clone.CalcIntrpGap = m_calcIntrpGap;

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
				string msg = string.Format(Resources.kstidWaveFileNotFound,
					STUtils.PrepFilePathForSTMsgBox(audioFilePath));

				STUtils.STMsgBox(msg, MessageBoxButtons.OK);
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
				doc = STUtils.DeserializeData(transcriptionFile,
					typeof(SaAudioDocument), out e) as SaAudioDocument;

				if (e != null)
				{
					ExceptionViewer viewer = new ExceptionViewer(e);
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
			string tmpFile = Path.GetDirectoryName(audioFile);
			tmpFile = Path.Combine(tmpFile, "~#!tstWrite!#~");
			try
			{
				// Check to see if the folder and transcription file are writable
				File.WriteAllText(tmpFile, string.Empty);
				string transFilePath = Path.ChangeExtension(audioFile, ".saxml");
				if (File.Exists(transFilePath))
					if ((File.GetAttributes(transFilePath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
						throw new Exception();
			}
			catch
			{
				// Warn the user that we don't have write privileges
				string audioFileNameOnly = Path.GetFileName(audioFile);
				string transFileNameOnly = Path.ChangeExtension(audioFileNameOnly, ".saxml");
				string msg = Resources.kstidReadOnlyFolderMsg;
				msg = string.Format(msg, audioFileNameOnly, audioFileNameOnly, transFileNameOnly);
				msg = STUtils.ConvertLiteralNewLines(msg);
				STUtils.STMsgBox(msg, MessageBoxButtons.OK);

				using (FolderBrowserDialog dlg = new FolderBrowserDialog())
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

			return STUtils.SerializeData(TranscriptionFile, this);
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

			return (double)byteVal / (double)bytesPerSecond;
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

			return (uint)(Math.Round(seconds * (double)bytesPerSecond,
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
			string audioFilePath = (m_audioFile != null || s_audioFileLoading == null)
				? m_audioFile : s_audioFileLoading;
			if (m_samplesPerSecond == -1)
			{
				if (File.Exists(audioFilePath))
					GetAudioFormatValues(audioFilePath);
				else
					return 0;
			}

			return BytesToSeconds(byteVal, m_channels,
				m_samplesPerSecond, m_bitsPerSample);
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
			string audioFilePath = (m_audioFile != null || s_audioFileLoading == null)
				? m_audioFile : s_audioFileLoading;
			if (m_samplesPerSecond == -1)
			{
				if (File.Exists(audioFilePath))
					GetAudioFormatValues(audioFilePath);
				else
					return 0;
			}


			return SecondsToBytes(seconds, m_channels,
				m_samplesPerSecond, m_bitsPerSample);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets format values from the audio file, such as sample rate, sample size, etc.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetAudioFormatValues(string audioFilePath)
		{
			try
			{
				FileStream stream = File.Open(audioFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);

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

				if (stream != null)
					stream.Close();

				stream = null;
				reader = null;
			}
			catch
			{
				return false;
			}

			return true;

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

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a temporary companion transcription file for the specified audio file.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public static string GetTempTranscriptionFile(string audioFile)
		//{
		//    return Path.ChangeExtension(audioFile, ".saxml.tmp");
		//}

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the companion MusicXML file for the specified audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetMusicXMLFile(string audioFile, bool isForTmpOperation)
		{
			return Path.ChangeExtension(audioFile, isForTmpOperation ? ".music.xml.tmp" : ".music.xml");
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
					string msg = string.Format(Resources.kstidWaveFileNotFound,
						STUtils.PrepFilePathForSTMsgBox(value));

					STUtils.STMsgBox(msg, MessageBoxButtons.OK);
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
		/// Gets the companion MusicXML file for the document's audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string MusicXMLFile
		{
			get { return GetMusicXMLFile(m_audioFile, m_isForTmpOperation); }
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
					string.IsNullOrEmpty(m_serializedGender) ||
					!m_genders.Contains(m_serializedGender) ?
					m_genders[0] : m_serializedGender);
			}
			set
			{
				string test = value.Trim();
				if (value.Trim() == m_serializedGender)
					return;

				m_serializedGender = m_genders.Contains(value.Trim())? value.Trim() : m_genders[0];
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
		public string EthnologueId
		{
			get { return m_ethnologueId; }
			set {m_ethnologueId = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the region in which langauge is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Region
		{
			get { return m_region; }
			set	{m_region = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the country in which language is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Country
		{
			get { return m_country; }
			set	{ m_country = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the language family.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Family
		{
			get { return m_family; }
			set	{ m_family = value;	}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the language name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageName
		{
			get { return m_languageName; }
			set	{ m_languageName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the dialect.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Dialect
		{
			get { return m_dialect; }
			set	{ m_dialect = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the note book reference.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NoteBookReference
		{
			get { return m_noteBookRef; }
			set	{ m_noteBookRef = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the free form translation
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FreeFormTranslation
		{
			get { return m_freeFormTrans; }
			set	{ m_freeFormTrans = value;	}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of person who transcribed the audio data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber
		{
			get { return m_transcriber; }
			set { m_transcriber = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the size of the data chunk in the audio. (This length does not include any
		/// pad byte).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DataChunkSize
		{
			get { return m_dataChunkSize; }
			set	{ m_dataChunkSize = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int FormatTag
		{
			get { return m_formatTag; }
			set	{ m_formatTag = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Channels
		{
			get { return m_channels; }
			set	{ m_channels = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int SamplesPerSecond
		{
			get { return m_samplesPerSecond; }
			set	{ m_samplesPerSecond = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int AverageBytesPerSecond
		{
			get { return m_averageBytesPerSecond; }
			set	{ m_averageBytesPerSecond = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int BlockAlignment
		{
			get { return m_blockAlignment; }
			set	{ m_blockAlignment = value;	}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int BitsPerSample
		{
			get { return m_bitsPerSample; }
			set	{m_bitsPerSample = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Description from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SADescription
		{
			get { return m_saDescription; }
			set { m_saDescription = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Flags from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SAFlags
		{
			get { return m_saFlags; }
			set { m_saFlags = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordFileFormat
		{
			get { return m_recordFileFormat; }
			set { m_recordFileFormat = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordTimeStamp
		{
			get { return m_recordTimeStamp; }
			set { m_recordTimeStamp = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordBandWidth
		{
			get { return m_recordBandWidth; }
			set { m_recordBandWidth = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordSampleSize
		{
			get { return m_recordSampleSize; }
			set { m_recordSampleSize = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int NumberOfSamples
		{
			get { return m_numberOfSamples; }
			set { m_numberOfSamples = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMax
		{
			get { return m_signalMax; }
			set { m_signalMax = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMin
		{
			get { return m_signalMin; }
			set { m_signalMin = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalBandWidth
		{
			get { return m_signalBandWidth; }
			set { m_signalBandWidth = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalEffSampSize
		{
			get { return m_signalEffSampSize; }
			set { m_signalEffSampSize = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqLow
		{
			get { return m_calcFreqLow; }
			set { m_calcFreqLow = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqHigh
		{
			get { return m_calcFreqHigh; }
			set { m_calcFreqHigh = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcVoicingThd
		{
			get { return m_calcVoicingThd; }
			set { m_calcVoicingThd = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcPercntChng
		{
			get { return m_calcPercntChng; }
			set { m_calcPercntChng = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcGrpSize
		{
			get { return m_calcGrpSize; }
			set { m_calcGrpSize = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcIntrpGap
		{
			get { return m_calcIntrpGap; }
			set { m_calcIntrpGap = value; }
		}

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the music transcription segments.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MusicSegmentData[] MusicSegments
		{
			get
			{
				uint i = 0;
				MusicSegmentData[] segments = new MusicSegmentData[m_musicSegments.Count];
				foreach (KeyValuePair<MusicSegmentKey, MusicSegmentData> seg in m_musicSegments)
					segments[i++] = seg.Value;

				return segments;
			}
			set
			{
				m_musicSegments.Clear();
				if (value == null)
					return;

				foreach (MusicSegmentData seg in value)
				{
					MusicSegmentKey key = new MusicSegmentKey();
					seg.SaAudioDocument = this;
					key.PhraseLevel = seg.PhraseLevel;
					key.Offset = seg.Offset;
					m_musicSegments.Add(key, seg);
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
		double m_offsetInSeconds = 0;
		double m_durationInSeconds = 0;
		double m_markDurationInSeconds = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SegmentData()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SegmentData(SaAudioDocument owningAudioDoc)
		{
			SaAudioDocument = owningAudioDoc;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public uint Offset
		{
			get { return (m_owningAudioDoc == null ? 0 : m_owningAudioDoc.SecondsToBytes(m_offsetInSeconds)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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

		[XmlAttribute]
		public bool IsBookmark = false;

		public string Phonetic = null;

		public string Phonemic = null;

		public string Tone = null;

		public string Orthographic = null;

		/// <summary>This will only contain a value when the segment is the first in a word.</summary>
		public string PartOfSpeech = null;

		/// <summary>This will only contain a value when the segment is the first in a word.</summary>
		public string Gloss = null;

		/// <summary>This will only contain a value when the segment is the first in a word.</summary>
		public string Reference = null;
	}

	#endregion

	#region MusicSegmentData Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Internal class to store information about a single segment in a audio file transcription.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class MusicSegmentData
	{
		// This is the first SaAudioDocument version number when offsets, durations
		// and mark durations were stored in segments as seconds rather than bytes.
		private const float kFirstVerWithByteSecondSwitch = 2.0f;

		private SaAudioDocument m_owningAudioDoc;
		double m_offsetInSeconds = 0;
		double m_durationInSeconds = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MusicSegmentData()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MusicSegmentData(SaAudioDocument owningAudioDoc)
		{
			SaAudioDocument = owningAudioDoc;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
				}
			}
		}

		[XmlAttribute]
		public uint PhraseLevel = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public uint Offset
		{
			get { return (m_owningAudioDoc == null ? 0 : m_owningAudioDoc.SecondsToBytes(m_offsetInSeconds)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public uint Duration
		{
			get { return (m_owningAudioDoc == null ? 0 : m_owningAudioDoc.SecondsToBytes(m_durationInSeconds)); }
		}

		public string Annotation = null;
	}

	#endregion
}
