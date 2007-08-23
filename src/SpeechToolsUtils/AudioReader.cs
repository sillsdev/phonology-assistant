using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using SilEncConverters22;

namespace SIL.SpeechTools.Utils
{
	public class AudioReader : IDisposable
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public enum InitResult
		{
			FileNotFound,
			InvalidFormat,
			NoSAData,
			Success
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public class SegmentInfo
		{
			public string segment;
			public byte[] segbytes;
			public uint offset;
			public uint length;
		}

		public const string kidRiff = "RIFF";
		public const string kidWave = "WAVE";
		public const string kidFmtChunk = "fmt ";
		public const string kidDataChunk = "data";
		public const string kidSAChunk = "sa  ";
		public const string kidUttChunk = "utt ";
		public const string kidEticChunk = "etic";
		public const string kidEmicChunk = "emic";
		public const string kidToneChunk = "tone";
		public const string kidOrthoChunk = "orth";
		public const string kidMarkChunk = "mark";
		public const string kidFontChunk = "font";
		public const string kidSpkrChunk = "spkr";
		public const string kidLangChunk = "lang";
		public const string kidRefChunk = "ref ";
		public const string kidMdatChunk = "mdat";
		public const string kidMPL1Chunk = "mpl1";
		public const string kidMPL2Chunk = "mpl2";
		public const string kidMPL3Chunk = "mpl3";
		public const string kidMPL4Chunk = "mpl4";
		public const string kidMFontChunk = "mfon";

		private FileStream m_stream = null;
		private BinaryReader m_reader = null;
		private SaAudioDocumentWriter m_writer;
		private string m_audioFile;
		private bool m_isWave = false;
		private TransConverterInfo m_transConverters;
		private bool m_backupAudioFile = true;

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Play nice and clean up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (m_stream != null)
				m_stream.Close();
			if (m_writer != null)
				m_writer.Close();
			
			m_stream = null;
			m_reader = null;
			m_writer = null;
		}

		#endregion
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initialize WaveReader object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public InitResult Initialize(string audioFile)
		{
			if (!File.Exists(audioFile))
				return InitResult.FileNotFound;
			
			m_audioFile = audioFile;
			m_stream = File.Open(audioFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			m_reader = new BinaryReader(m_stream, Encoding.ASCII);

			if (!IsValidAudioFile())
				return InitResult.InvalidFormat;

			m_isWave = IsValidWaveFile();
			if (!m_isWave || (GetChunkOffset(m_stream, kidSAChunk) == -1))
				return InitResult.NoSAData;

			return InitResult.Success;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if this is a valid audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsValidAudioFile()
		{
			// TODO: add format checking for all media types
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if this is a valid wave file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool IsValidWaveFile(FileStream stream)
		{
			BinaryReader reader = new BinaryReader(stream);

			// Verify RIFF chunk exists
			stream.Position = 0;
			if (new string(Encoding.ASCII.GetChars(reader.ReadBytes(4))) != kidRiff)
				return false;

			// Verify WAVE chunk exists
			stream.Position = 8;
			if (new string(Encoding.ASCII.GetChars(reader.ReadBytes(4))) != kidWave)
				return false;

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if this is a valid wave file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsValidWaveFile()
		{
			return IsValidWaveFile(m_stream);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full file name and path to the audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string AudioFile
		{
			get { return m_audioFile; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the reader's MD5 Hash Code for the audio file.
		/// For wave files it is based on audio content alone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string MD5HashCode
		{
			get {return GetMD5HashCode(m_stream);}
		}

        /// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the MD5 hash code for the specified file.
		/// For wave files it is based on audio content alone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMD5HashCode(string audioFile)
		{
			return (File.Exists(audioFile) ?
				GetMD5FromByteArray(File.ReadAllBytes(audioFile)) : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the MD5 hash code for the file opened in the specified stream.
		/// For wave files it is based on audio content alone.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetMD5HashCode(FileStream stream)
		{
			BinaryReader reader = new BinaryReader(stream);

			stream.Position = 0;
			int audioSize = (int)stream.Length;

			// if this is a wave file, only calculate on the data chunk
			if (IsValidWaveFile(stream))
			{
				stream.Position = GetChunkOffset(stream, kidDataChunk) + 4;
				audioSize = reader.ReadInt32();
			}

			return GetMD5FromByteArray(reader.ReadBytes(audioSize));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates the MD5 hash code for the specified byte array.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetMD5FromByteArray(byte[] byteArray)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			Guid guid = new Guid(md5.ComputeHash(byteArray));
			return guid.ToString().ToUpper();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the wave file's bytes per sample (i.e. Hz value).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public long BytesPerSecond
		{
			get
			{
				if (!m_isWave)
					return 0;

				m_stream.Position = GetChunkOffset(m_stream, kidFmtChunk) + 8;
				m_reader.ReadUInt16();
				int channels = m_reader.ReadUInt16();
				int samplesPerSec = m_reader.ReadInt32();
				m_reader.ReadInt32();
				m_reader.ReadUInt16();
				int bitsPerSample = m_reader.ReadUInt16();

				int bytesPerSample = (bitsPerSample / 8) * channels;
				return (bytesPerSample * samplesPerSec);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary> 
		/// This function will return the byte offset, in a WAV file, of the beginning of the
		/// desired chunk. The chunk is specified by passing it's four character ID via sChunk.
		/// Each chunk begins with a 4 character ID followed by 4 bytes that indicate how long
		/// the chunk is. That will allow us to find out where the next chunk begins. If,
		/// however, a chunks length is odd, there will be a 1 byte pad at the end of that
		/// chunk so the next chunk always starts on an even byte boundary.  
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static long GetChunkOffset(FileStream stream, string chunkId)
		{
			BinaryReader reader = new BinaryReader(stream);

			// Start at beginning of 'fmt' chunk.
			stream.Position = 12;

			while (stream.Position < stream.Length)
			{
				// Read the chunk id.
				string id = new string(Encoding.ASCII.GetChars(reader.ReadBytes(4)));

				// If we've found the chunk id we're looking for, then return the
				// current position less the 4 bytes we just read to get the id.
				if (id == chunkId)
					return stream.Position - 4;

				// Get chunk length.
				uint chunkLen = reader.ReadUInt32();

				//Account for odd length chunks.
				if ((chunkLen & 1) != 0)
					chunkLen++;

				// Make sure we don't move the file pointer beyond EOF.
				if ((stream.Position + chunkLen) >= stream.Length - 4)
					break;

				// Move the file pointer over the chunk to the beginning of the next chunk.
				stream.Position += chunkLen;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Method for reading the audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Read(bool showConverterDlg)
		{
			m_writer = new SaAudioDocumentWriter();
			m_writer.Initialize(m_audioFile, null, MD5HashCode, false);

			// Check if this is a wave file. If not, save the transcription file and return.
			if (!IsValidWaveFile())
			{
				m_writer.Commit();
				m_writer.Close();
				return true;
			}

			if (!ReadFmtChunk())
				return false;

			if (!ReadDataChunk())
				return false;

			// Check if the audio file contains SA chunks. If not,
			// save the transcription file and return.
			if (GetChunkOffset(m_stream, kidSAChunk) == -1)
			{
				m_writer.Commit();
				m_writer.Close();
				return true;
			}

			if (!PrepareConverters(showConverterDlg))
				return false;

			if (!ReadSaChunk())
				return false;

			if (!ReadUttChunk())
				return false;

			// Process Phonetic, Phonemic, Tone and Ortho chunks.
			ProcessLangTranscriptionChunks();

			// Read the mark chunk data (gloss and ref).
			if (!ReadMarkChunk())
			{
				// TODO: Log a message
			}

			if (!ReadSpkrChunk())
			{
				// TODO: Log a message
			}

			if (!ReadLangChunk())
			{
				// TODO: Log a message
			}

			if (!ReadRefChunk())
			{
				// TODO: Log a message
			}

			if (!ReadMdatChunk())
			{
				// TODO: Log a message
			}

			// Process Music Phrase Level chunks into database
			ProcessMusicTranscriptionChunks();

			// TODO: When code exists to read wave files in batch mode, then move this so
			// it's done after all wave files are read and converted.
			STUtils.EncodingConverters = null;

			m_writer.Commit(m_backupAudioFile);
			m_writer.Close();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Prepares the encoding converters to apply to each SA transcription.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool PrepareConverters(bool showConverterDlg)
		{
			if (m_transConverters == null)
				m_transConverters = TransConverterInfo.Load();

			// TODO: What do we do when there are no converters?
			if (m_transConverters.Count == 0)
			{
			}

			if (showConverterDlg)
			{
				string[] legacyFontNames = ReadFontChunks();

				using (TransConvertersDlg dlg = new TransConvertersDlg(m_transConverters, legacyFontNames))
				{
					// If there's a splash screen open, then close it before showing the
					// dialog. Otherwise, the dialog will popup behind the splash screen.
					if (STUtils.s_splashScreen != null)
						STUtils.s_splashScreen.Close();

					DialogResult res = dlg.ShowDialog();
					if (res == DialogResult.Cancel)
						return false;
					m_backupAudioFile = dlg.BackupAudioFile;
					m_transConverters.Save();
				}

				Application.DoEvents();
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the format chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadFmtChunk()
		{
			try
			{
				//Start at the beginning of fmt chunk plus advance eight 
				//positions to move ahead of the chunk ID and chunk size.
				m_stream.Position = GetChunkOffset(m_stream, kidFmtChunk) + 8;

				//Read the correct values that correspond with each category
				//of the fmt chunk, then write the information to the database.
				m_writer.FormatTag = m_reader.ReadUInt16();
				m_writer.Channels = m_reader.ReadUInt16();
				int samplesPerSecond = m_reader.ReadInt32();
				m_writer.SamplesPerSecond = samplesPerSecond;
				m_writer.AverageBytesPerSecond = m_reader.ReadInt32();
				m_writer.BlockAlignment = m_reader.ReadUInt16();
				int bitsPerSample = m_reader.ReadUInt16();
				m_writer.BitsPerSample = bitsPerSample;

				// Also set default record info
				m_writer.RecordFileFormat = 1; // This is for wave files
				long createTime = File.GetCreationTimeUtc(m_audioFile).Ticks;
				DateTime timeBase = new DateTime(1970, 1, 1, 0, 0, 0);
				// Convert from int64 .NET timestamp to int32 MFC timestamp 
				m_writer.RecordTimeStamp = (int)((createTime - timeBase.Ticks) / 10000000);
				if (m_isWave)
				{
					m_writer.RecordBandWidth = samplesPerSecond / 2;
					m_writer.RecordSampleSize = bitsPerSample;
					m_writer.SignalBandWidth = samplesPerSecond / 2;
					m_writer.SignalEffSampSize = bitsPerSample;
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the data chunk and save the data read to the SA document writer.
		/// Note:  The Speech Data does not need to be read to the database.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadDataChunk()
		{
			try
			{
				//Start at the beginning of data chunk plus advance  
				//four positions to move ahead of the chunk ID.
				m_stream.Position = GetChunkOffset(m_stream, kidDataChunk) + 4;

				//Read the data chunk size and write to the database.
				int dataChunkSize = m_reader.ReadInt32();
				m_writer.DataChunkSize = dataChunkSize;

				//Calculate number of samples
				m_stream.Position = GetChunkOffset(m_stream, kidFmtChunk) + 22;
				int bitsPerSample = m_reader.ReadUInt16();
				m_writer.NumberOfSamples = 8 * dataChunkSize / bitsPerSample;
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the sa chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadSaChunk()
		{
			try
			{
				// Find the SA chunk, skipping over the chunk size and read the SA version.
				m_stream.Position = GetChunkOffset(m_stream, kidSAChunk) + 8;
				m_reader.ReadSingle(); // skip the RIFF version

				// Get SA description.
				string saDesc = new string(Encoding.ASCII.GetChars(m_reader.ReadBytes(256)));
				// Strip out any garbage after first null.
				m_writer.SADescription = saDesc.Substring(0, saDesc.IndexOf('\0'));
				
				// Read the rest of the SA information.
				m_writer.SAFlags = m_reader.ReadUInt16();
				m_writer.RecordFileFormat = m_reader.ReadByte();
				m_writer.RecordTimeStamp = m_reader.ReadInt32();
				m_writer.RecordBandWidth = m_reader.ReadInt32();
				m_writer.RecordSampleSize = m_reader.ReadByte();
				m_writer.NumberOfSamples = m_reader.ReadInt32();
				m_writer.SignalMax = m_reader.ReadInt32();
				m_writer.SignalMin = m_reader.ReadInt32();
				m_writer.SignalBandWidth = m_reader.ReadInt32();
				m_writer.SignalEffSampSize = m_reader.ReadByte();
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the utt chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadUttChunk()
		{
			try
			{
				//Start at the beginning of utt chunk plus advance eight
				//positions to move ahead of the chunk ID and chunk size.
				m_stream.Position = GetChunkOffset(m_stream, kidUttChunk) + 8;

				//Read the correct values that correspond with each category
				//of the utt chunk, then write the information to the database.
				m_writer.CalcFreqLow = m_reader.ReadInt16();
				m_writer.CalcFreqHigh = m_reader.ReadInt16();
				m_writer.CalcVoicingThd = m_reader.ReadInt16();
				m_writer.CalcPercntChng = m_reader.ReadInt16();
				m_writer.CalcGrpSize = m_reader.ReadInt16();
				m_writer.CalcIntrpGap = m_reader.ReadInt16();
			}
			catch
			{
				return false;
			}
			
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes language transcription chunks into database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ProcessLangTranscriptionChunks()
		{
			Dictionary<string, AnnotationType> chunkDict = new Dictionary<string, AnnotationType>();
			chunkDict.Add(kidEticChunk, AnnotationType.Phonetic);
			chunkDict.Add(kidEmicChunk, AnnotationType.Phonemic);
			chunkDict.Add(kidToneChunk, AnnotationType.Tone);
			chunkDict.Add(kidOrthoChunk, AnnotationType.Orthographic);

			foreach (KeyValuePair<string, AnnotationType> entry in chunkDict)
			{
				string kid = entry.Key;
				AnnotationType annType = entry.Value;

				// Read the transcription data.
				List<SegmentInfo> segInfo = ReadTranscriptionChunk(kid);
				if (segInfo != null)
				{
					ConvertSegments(kid, segInfo);
					LoadWriterWithSegments(annType, segInfo);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Processes music transcription chunks into database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ProcessMusicTranscriptionChunks()
		{
			Dictionary<string, AnnotationType> chunkDict = new Dictionary<string, AnnotationType>();
			chunkDict.Add(kidMPL1Chunk, AnnotationType.MusicPhraseLevel1);
			chunkDict.Add(kidMPL2Chunk, AnnotationType.MusicPhraseLevel2);
			chunkDict.Add(kidMPL3Chunk, AnnotationType.MusicPhraseLevel3);
			chunkDict.Add(kidMPL4Chunk, AnnotationType.MusicPhraseLevel4);

			foreach (KeyValuePair<string, AnnotationType> entry in chunkDict)
			{
				string kid = entry.Key;
				AnnotationType annType = entry.Value;

				// Read the transcription data.
				List<SegmentInfo> segInfo = ReadTranscriptionChunk(kid);
				if (segInfo != null)
				{
					ConvertSegments(kid, segInfo);
					LoadWriterWithSegments(annType, segInfo);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reads an annotation chunk.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<SegmentInfo> ReadTranscriptionChunk(string chunkID)
		{
			List<SegmentInfo> segInfo = new List<SegmentInfo>();
			
			try
			{
				// Find chunk and read length
				m_stream.Position = GetChunkOffset(m_stream, chunkID) + 4;
				if (m_reader.ReadUInt32() == 0)
					return null;

				// Read transcription, making sure to include the null byte.
				int annStrLen = m_reader.ReadInt16();
				byte[] annData = m_reader.ReadBytes(annStrLen + 1);

				List<byte> bytes = new List<byte>();
				uint prevOffset = uint.MaxValue;
				uint prevDuration = uint.MaxValue;

				for (int i = 0; i < annStrLen; i++)
				{
					uint offset = m_reader.ReadUInt32();
					uint duration = m_reader.ReadUInt32();
					if (offset != prevOffset && bytes.Count > 0)
					{
						SegmentInfo info = new SegmentInfo();
						info.segbytes = bytes.ToArray();
						info.offset = prevOffset;
						info.length = prevDuration;
						segInfo.Add(info);

						bytes.Clear();
					}

					bytes.Add(annData[i]);
					prevOffset = offset;
					prevDuration = duration;
				}

				if (bytes.Count > 0)
				{
					SegmentInfo info = new SegmentInfo();
					info.segbytes = bytes.ToArray();
					info.offset = prevOffset;
					info.length = prevDuration;
					segInfo.Add(info);
				}
			}
			catch
			{
				return null;
			}

			return segInfo;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the mdat chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadMdatChunk()
		{
			try
			{
				//Start at the beginning of utt chunk plus advance four 
				//positions to move past the chunk ID.
				m_stream.Position = GetChunkOffset(m_stream, kidMdatChunk) + 4;
				int chunkSize = (int)m_reader.ReadUInt32();
				if (chunkSize == 0)
					return true;

				//Read the SAMA string
				string samaString = new string(m_reader.ReadChars(chunkSize));
				char[] trimChars = { '\0' };
				samaString = samaString.TrimEnd(trimChars);
				if (samaString.Length > 0)
				{
					// adjust SAMA ver 2 to SAMA ver 3
					string samaStringConverted = ConvertSAMA2To3(samaString);
					m_writer.WriteAsMusicXML(samaStringConverted);
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Converts SAMA version 2 to SAMA version 3.
		/// This involves changing the octave divisions to match MusicXML.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string ConvertSAMA2To3(string samaIn)
		{
			string samaOut = samaIn;
			string incrementOctave = "CDEFG";

			for (int i = 21; i < samaOut.Length; i += 7)
			{
				if (incrementOctave.Contains(samaOut[i].ToString()))
				{
					int newOctave = int.Parse(samaOut[i + 2].ToString()) + 1;
					samaOut = samaOut.Remove(i + 2, 1);
					samaOut = samaOut.Insert(i + 2, newOctave.ToString());
				}
			}

			return samaOut;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Perform's an encoding conversion on the segments in the specified chunk.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ConvertSegments(string chunkId, List<SegmentInfo> segInfo)
		{
			// Get the encoding converter used for the specified chunk.
			EncConverter ec = GetConverterForChunk(chunkId);
			if (segInfo == null || ec == null)
				return;

			// Convert each segment to Unicode
			for (int i = 0; i < segInfo.Count; i++)
				segInfo[i].segment = ec.ConvertToUnicode(segInfo[i].segbytes);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the encoding converter for the SA transcription specified by chunkId
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private EncConverter GetConverterForChunk(string chunkId)
		{
			if (m_transConverters == null || m_transConverters.Count == 0 ||
				m_transConverters[chunkId] == null)
			{
				return null;
			}

			// Get the encoding converter used for the specified chunk.
			return STUtils.GetConverter(m_transConverters[chunkId].Converter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the audio document writer with the data for the specified annotation type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadWriterWithSegments(AnnotationType type, List<SegmentInfo> segInfo)
		{
			foreach (SegmentInfo segment in segInfo)
				m_writer.AddSegment((int)type, segment.offset, segment.length, segment.segment);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the mark chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadMarkChunk()
		{
			try
			{
				// Go to mark chunk
				if ((m_stream.Position = GetChunkOffset(m_stream, kidMarkChunk)) < 0)
					return true;
				
				// Skip the chunk Id and read the chunk's length.
				m_stream.Position += 4;
				uint markDataLen = m_reader.ReadUInt32();
				uint bytesRead = 0;

				while (bytesRead < markDataLen)
				{
					// Read the length for the string.
					int labelLen = m_reader.ReadInt16();
					string label = new string(Encoding.ASCII.GetChars(m_reader.ReadBytes(labelLen)));

					// Skip over label's null terminator and get the mark's offset and duration.
					m_stream.Position++;
					uint offset = m_reader.ReadUInt32();
					uint duration = m_reader.ReadUInt32();

					// Increment the byte counter by the number of bytes read for this mark.
					bytesRead += ((uint)labelLen + 11);

					string gloss;
					string reference;
					bool isBookMark;
					if (GetMarkInfo(label, out gloss, out reference, out isBookMark))
						m_writer.AddMarkSegment(offset, duration, gloss, null, reference, isBookMark);
				}
			}
			catch
			{
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the information from a single mark label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetMarkInfo(string label, out string gloss, out string reference,
			out bool isBookMark)
		{
			gloss = string.Empty;
			reference = string.Empty;
			isBookMark = false;

			if (string.IsNullOrEmpty(label))
				return false;

			// Split the label into its sub pieces.
			string[] labelPieces = label.Split(new char[] {'\0'});
			if (labelPieces.Length == 0)
				return false;

			// Strip off the word break or bookmark indicator.
			if (labelPieces[0][0] == '#' || labelPieces[0][0] == '!')
			{
				isBookMark = labelPieces[0][0] == '!';
				labelPieces[0] = labelPieces[0].Substring(1);
			}

			gloss = labelPieces[0];

			// When the label contains contains a null terminator in its middle, it means
			// the portion following should start with "ref:" and is considered the word's
			// reference. However, if we've found anything after a medial null in the lable
			// we'll consider a reference whether or not it begins with "ref:" since that's
			// the only option.
			if (labelPieces.Length == 2 && labelPieces[1] != "ref:")
			{
				reference = (labelPieces[1].StartsWith("ref:") ?
					labelPieces[1].Substring(4) : labelPieces[1]);
			}

			// Convert the gloss segment to Unicode
			EncConverter ec = GetConverterForChunk("mark");
			if (!string.IsNullOrEmpty(gloss) && ec != null)
				gloss = ec.Convert(gloss);

			// Convert the reference segment to Unicode
			ec = GetConverterForChunk("ref");
			if (!string.IsNullOrEmpty(reference) && ec != null)
				reference = ec.Convert(reference);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the font chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public string[] ReadFontChunks()
		{
			string[] fontName = new string[10];

			try
			{
				// read font chunk
				if ((m_stream.Position = GetChunkOffset(m_stream, kidFontChunk)) > 0)
				{
					// Skip the chunk Id and read the chunk's length.
					m_stream.Position += 4;
					uint fontDataLen = m_reader.ReadUInt32();
					uint fontIndex = 0;

					// read font names 1 byte at a time
					for (uint bytesRead = 0; ((bytesRead < fontDataLen) && (fontIndex < 6)); bytesRead++)
					{
						// Read another char.
						char nextChar = Encoding.ASCII.GetChars(m_reader.ReadBytes(1))[0];
						if (nextChar == '\0')
							fontIndex++;
						else
							fontName[fontIndex] += nextChar;
					}
				}

				// read mfon chunk
				if ((m_stream.Position = GetChunkOffset(m_stream, kidMFontChunk)) > 0)
				{
					// Skip the chunk Id and read the chunk's length.
					m_stream.Position += 4;
					uint fontDataLen = m_reader.ReadUInt32();
					uint fontIndex = 6;

					// read font names 1 byte at a time
					for (uint bytesRead = 0; ((bytesRead < fontDataLen) && (fontIndex < 10)); bytesRead++)
					{
						// Read another char.
						char nextChar = Encoding.ASCII.GetChars(m_reader.ReadBytes(1))[0];
						if (nextChar == '\0')
							fontIndex++;
						else
							fontName[fontIndex] += nextChar;
					}
				}
			}
			catch
			{
				return fontName;
			}

			return fontName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the spkr chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadSpkrChunk()
		{
			try
			{
				// Go to the speaker chunk, advance to the chunk size, read it to
				// get the length of the speaker's name (chunk length - 1 byte for
				// gender) and then read the gender.
				// Go to mark chunk
				if ((m_stream.Position = GetChunkOffset(m_stream, kidSpkrChunk)) < 0)
					return true;

				// Skip the chunk Id and read the chunk's length.
				m_stream.Position += 4;
				int chunkLen = m_reader.ReadInt32();
				if (chunkLen == 0)
					return true;

				// Read gender character.
				m_writer.SpeakerGender = (char)m_reader.ReadByte();

				// 2 for the gender character and the null at the end of the speaker name.
				if (chunkLen > 2)
				{
					string spkrName =
						new string(Encoding.ASCII.GetChars(m_reader.ReadBytes(chunkLen - 2)));
					spkrName = spkrName.Trim();
					if (spkrName.Length > 0)
						m_writer.SpeakerName = spkrName.Trim();
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the lang chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadLangChunk()
		{
			try
			{
				// Go to the language chunk and read the chunk length less 3 (which is the
				// length of the ethnologue id).
				if ((m_stream.Position = GetChunkOffset(m_stream, kidLangChunk)) < 0)
					return true;

				// Skip chunk Id and read data length.
				m_stream.Position += 4;
				int langDataLen = m_reader.ReadInt32() - 3;
 
				//Read the 3 characters for the Ethnologue ID and write to database.
				string ethId = new string(Encoding.ASCII.GetChars(m_reader.ReadBytes(3)));
				m_writer.EthnologueId = ethId.Trim();

				string langData = 
					new string(Encoding.ASCII.GetChars(m_reader.ReadBytes(langDataLen)));

				// Parse the language data into region, country, family, language and dialect.
				string[] parsedLangData =
					langData.Split(new char[] { '\0' }, StringSplitOptions.None);

				if (parsedLangData.Length > 0)
					m_writer.Region = parsedLangData[0];

				if (parsedLangData.Length > 1)
					m_writer.Country = parsedLangData[1];

				if (parsedLangData.Length > 2)
					m_writer.Family = parsedLangData[2];

				if (parsedLangData.Length > 3)
					m_writer.LanguageName = parsedLangData[3];

				if (parsedLangData.Length > 4)
					m_writer.Dialect = parsedLangData[4];
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the ref chunk and save the data read to the SA document writer.
		/// </summary>
		/// <returns>true if success. Otherwise, false</returns>
		/// ------------------------------------------------------------------------------------
		public bool ReadRefChunk()
		{
			try
			{
				// Go to the reference chunk and read the chunk length.
				if ((m_stream.Position = GetChunkOffset(m_stream, kidRefChunk)) < 0)
					return true;

				// Skip chunk Id and read data length.
				m_stream.Position += 4;
				int refDataLen = m_reader.ReadInt32();

				string refData =
					new string(Encoding.ASCII.GetChars(m_reader.ReadBytes(refDataLen)));

				// Parse reference data into notebook ref, free translation and transcriber.
				string[] parsedRefData =
					refData.Split(new char[] { '\0' }, StringSplitOptions.None);

				if (parsedRefData.Length > 0)
					m_writer.NoteBookReference = parsedRefData[0];

				if (parsedRefData.Length > 1)
					m_writer.FreeFormTranslation = parsedRefData[1];

				if (parsedRefData.Length > 2)
					m_writer.Transcriber = parsedRefData[2];
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}
