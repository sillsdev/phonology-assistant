using System.IO;
using System.Runtime.InteropServices;

namespace SIL.SpeechTools.Utils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for writing documents modified in SA.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[ProgId("SpeechToolsInterfaces.SaAudioDocumentWriter")]
	[ClassInterface(ClassInterfaceType.None)]
	[GuidAttribute("5F70259E-A867-470a-B114-6D516722CE39")]
	[ComVisible(true)]
	public class SaAudioDocumentWriter : ISaAudioDocumentWriter
	{
		private const string kTmpFilePathHolderName = "sa.~#!tmpfileholder~#!.tmp";

		private SaAudioDocument m_doc;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaAudioDocumentWriter()
		{
		}

		#region Create/Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes an object to write SA data to the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(string audioFilePath, string origMD5HashCode,
			bool isForTempOperation)
		{
			bool result = Initialize(audioFilePath, origMD5HashCode, null, isForTempOperation);
			
			if (isForTempOperation)
			{
				// Create a path for a temporary transcription file and add that path
				// to the temp. file that holds paths to temporary transcription files.
				string tmpFilePathHolder = Path.Combine(Path.GetTempPath(), kTmpFilePathHolderName);
				File.AppendAllText(tmpFilePathHolder, m_doc.TranscriptionFile + "\r\n");
			}

			return result;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes an object to write SA data to the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(string audioFilePath, string origMD5HashCode,
			string newMD5HashCode, bool isForTmpOperation)
		{
			m_doc = SaAudioDocument.Load(audioFilePath, isForTmpOperation, false);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not really necessary since the garbage collector should handle clearing
		/// memory. But this is provided so COM clients can force freeing some of the memory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Close()
		{
			m_doc = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the temporary files SA created for cut, copy and paste operations.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DeleteTempDB()
		{
			// Create the path to the temporary file that holds temporary transcription file
			// paths and make sure the file exists.
			string tmpFilePathHolder = Path.Combine(Path.GetTempPath(), kTmpFilePathHolderName);
			if (!File.Exists(tmpFilePathHolder))
				return;

			// Read all the lines in the file. (Each line is the full path to a temp. file used
			// for SA's cut, copy and paste operations.) Then delete each file in those paths.
			string[] tmpFilePaths = File.ReadAllLines(tmpFilePathHolder);
			foreach (string path in tmpFilePaths)
			{
				if (File.Exists(path))
					File.Delete(path);
			}

			File.Delete(tmpFilePathHolder);
		}

		#endregion

		#region Methods for committing audio document to the database.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Commits to a transcription file the data collected in SaAudioDocumentWriter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Commit()
		{
			return Commit(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Commits to a transcription file the data collected in SaAudioDocumentWriter.
		/// When backupOldAudioFile is true, the assumption is the audio file contains
		/// old SA chunks and they are stripped out after the file is backed up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Commit(bool backupOldAudioFile)
		{
			bool retVal = m_doc.Save();

			if (!retVal)
				return false;

			if (backupOldAudioFile)
			{
				// First, make a backup copy of the file.
				string newExt = "SA2" + Path.GetExtension(m_doc.AudioFile);
				string newPath = Path.GetDirectoryName(m_doc.AudioFile);
				if (!newPath.EndsWith("\\"))
					newPath += "\\";
				newPath += "SA2 Wav Files\\";
				Directory.CreateDirectory(newPath);
				newPath += Path.GetFileName(m_doc.AudioFile);
				newPath = Path.ChangeExtension(newPath, newExt);

				if (!File.Exists(newPath))		// don't copy if it's already backed up
					File.Copy(m_doc.AudioFile, newPath);
			}
		
			File.SetAttributes(m_doc.AudioFile, FileAttributes.Normal);
			int newFileLength;
			byte[] bytes = null;

			// Now, read all the bytes in the file that are not from SA chunks.
			using (FileStream stream = File.Open(m_doc.AudioFile, FileMode.Open,
				FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				// Get the offset in the file of the first SA chunk. That will
				// be the length of the file after the SA chunks are removed.
				newFileLength = (int)AudioReader.GetChunkOffset(stream, AudioReader.kidSAChunk);
				if (newFileLength > 0)
				{
					stream.Position = 0;
					BinaryReader reader = new BinaryReader(stream);
					bytes = reader.ReadBytes(newFileLength);
				}

				stream.Flush();
				stream.Close();
			}

			if (newFileLength <= 0 || bytes == null)
				return true;

			// Now rewrite the file without all the SA chunks. Write all the bytes
			// then skip over the 'RIFF' tag and write out the modify RIFF chunk
			// length, which is 8 bytes less than the length of the file since it
			// doesn't include 'RIFF' and the 4 bytes for the RIFF chunk length.
			using (FileStream stream = File.Open(m_doc.AudioFile, FileMode.Truncate,
				FileAccess.Write, FileShare.ReadWrite))
			{
				BinaryWriter writer = new BinaryWriter(stream);
				writer.Write(bytes);
				stream.Position = 4;
				writer.Write(newFileLength - 8);
				stream.Close();
			}

			return true;
		}

		#endregion

		#region Methods for adding segment information
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure a segment for the specified offset exists in the segment collection.
		/// If a segment for the specified offset doesn't exist, one is created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ValidateSegmentForOffset(uint offset, int annotationType)
		{
			if (annotationType < (int)AnnotationType.MusicPhraseLevel1)
			{
				int segIndex = GetSegmentIndexFromOffset(offset);
				if (segIndex > -1)
					return;

				uint segCount = (uint)m_doc.m_segments.Count;
				m_doc.m_segments[segCount] = new SegmentData(m_doc);
				m_doc.m_segments[segCount].OffsetInSeconds = m_doc.BytesToSeconds(offset);
			}
			else
			{
				MusicSegmentKey key = new MusicSegmentKey();
				key.PhraseLevel = (uint)(annotationType - AnnotationType.MusicPhraseLevel1 + 1);
				key.Offset = offset;
				if (!m_doc.m_musicSegments.ContainsKey(key))
				{
					m_doc.m_musicSegments[key] = new MusicSegmentData(m_doc);
					m_doc.m_musicSegments[key].OffsetInSeconds = m_doc.BytesToSeconds(offset);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the duration of the segment with the specified offset and annotation type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateDuration(uint offset, uint length, int annotationType)
		{
			if (annotationType < (int)AnnotationType.MusicPhraseLevel1)
				m_doc.m_segments[(uint)GetSegmentIndexFromOffset(offset)].DurationInSeconds = m_doc.BytesToSeconds(length);
			else
			{
				MusicSegmentKey key = new MusicSegmentKey();
				key.PhraseLevel = (uint)(annotationType - AnnotationType.MusicPhraseLevel1 + 1);
				key.Offset = offset;
				m_doc.m_musicSegments[key].DurationInSeconds = m_doc.BytesToSeconds(length);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the index of a segment from its offset.
		/// Return Value: segment index, if found; -1, otherwise.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int GetSegmentIndexFromOffset(uint offset)
		{
			int segCount = m_doc.m_segments.Count;
			for (int i = 0; i < segCount; i++)
			{
				if (m_doc.m_segments[(uint)i].Offset == offset)
					return i;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a segment string at the specified offset, with the specified length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddSegment(int annotationType, uint offset, uint length,
			string annotation)
		{
			ValidateSegmentForOffset(offset, annotationType);
			UpdateDuration(offset, length, annotationType);

			uint segIndex = (uint)GetSegmentIndexFromOffset(offset);

			switch ((AnnotationType)annotationType)
			{
				case AnnotationType.Phonetic:
					m_doc.m_segments[segIndex].Phonetic = annotation;
					break;

				case AnnotationType.Phonemic:
					m_doc.m_segments[segIndex].Phonemic = annotation;
					break;

				case AnnotationType.Tone:
					m_doc.m_segments[segIndex].Tone = annotation;
					break;

				case AnnotationType.Orthographic:
					m_doc.m_segments[segIndex].Orthographic = annotation;
					break;

				case AnnotationType.MusicPhraseLevel1:
				case AnnotationType.MusicPhraseLevel2:
				case AnnotationType.MusicPhraseLevel3:
				case AnnotationType.MusicPhraseLevel4:
					MusicSegmentKey key = new MusicSegmentKey();
					key.PhraseLevel = (uint)(annotationType - AnnotationType.MusicPhraseLevel1 + 1);
					key.Offset = offset;
					m_doc.m_musicSegments[key].PhraseLevel = key.PhraseLevel;
					m_doc.m_musicSegments[key].Annotation = annotation;
					break;

				default:
					return;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds information that spans one or more segment boundaries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddMarkSegment(uint offset, uint length, string gloss, string partOfSpeech,
			string reference, bool isBookmark)
		{
			ValidateSegmentForOffset(offset, (int)AnnotationType.Gloss);
			uint segIndex = (uint)GetSegmentIndexFromOffset(offset);
			m_doc.m_segments[segIndex].Gloss = gloss;
			m_doc.m_segments[segIndex].Reference = reference;
			m_doc.m_segments[segIndex].PartOfSpeech = partOfSpeech;
			m_doc.m_segments[segIndex].MarkDurationInSeconds = m_doc.BytesToSeconds(length);
			m_doc.m_segments[segIndex].IsBookmark = isBookmark;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds information that spans one or more segment boundaries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DeleteSegments()
		{
			m_doc.m_segments.Clear();
			m_doc.m_musicSegments.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Translates a SAMA music transcription and writes it to the XML database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void WriteAsMusicXML(string SAMAString)
		{
			ExportMusicXML(SAMAString, m_doc.MusicXMLFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Translates a SAMA music transcription and writes it to a MusicXML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ExportMusicXML(string SAMAString, string fileName)
		{
			if (SAMAString == string.Empty)
			{
				File.Delete(fileName);
				return;
			}
			
			MusicXML musicXML = new MusicXML(SAMAString);
			//
			musicXML.Save(fileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Copies the transcription data from one audio document to another.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Copy(string destFilePath, bool md5MustMatch)
		{
			// first check MD5
			if (md5MustMatch && (AudioReader.GetMD5HashCode(destFilePath) != m_doc.MD5HashCode))
				return;

			// copy the transcription data
			m_doc = m_doc.Clone();
			m_doc.AudioFile = destFilePath;
			Commit();
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the audio document associated with the writer.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SaAudioDocument AudioDocument
		{
			get { return m_doc; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the speaker's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SpeakerName
		{
			set {m_doc.SpeakerName = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the speaker's gender.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public char SpeakerGender
		{
			set {m_doc.SpeakerGender = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the ethnologue Id of langauge.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EthnologueId
		{
			set {m_doc.EthnologueId = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the region in which langauge is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Region
		{
			set {m_doc.Region = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the country in which language is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Country
		{
			set {m_doc.Country = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the language family.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Family
		{
			set {m_doc.Family = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the language name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageName
		{
			set {m_doc.LanguageName = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the dialect.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Dialect
		{
			set {m_doc.Dialect = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the notebook reference.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NoteBookReference
		{
			set {m_doc.NoteBookReference = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the free form translation
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FreeFormTranslation
		{
			set {m_doc.FreeFormTranslation = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the name of person who transcribed the audio data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber
		{
			set {m_doc.Transcriber = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the size of the data chunk in the audio. (This length does not include any
		/// pad byte).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DataChunkSize
		{
			set {m_doc.DataChunkSize = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int FormatTag
		{
			set {m_doc.FormatTag = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Channels
		{
			set {m_doc.Channels = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SamplesPerSecond
		{
			set {m_doc.SamplesPerSecond = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int AverageBytesPerSecond
		{
			set {m_doc.AverageBytesPerSecond = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BlockAlignment
		{
			set {m_doc.BlockAlignment = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitsPerSample
		{
			set {m_doc.BitsPerSample = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Description from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SADescription
		{
			set {m_doc.SADescription = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Flags from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SAFlags
		{
			set {m_doc.SAFlags = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordFileFormat
		{
			set {m_doc.RecordFileFormat = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordTimeStamp
		{
			set {m_doc.RecordTimeStamp = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordBandWidth
		{
			set {m_doc.RecordBandWidth = value;}
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordSampleSize
		{
			set {m_doc.RecordSampleSize = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NumberOfSamples
		{
			set {m_doc.NumberOfSamples = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMax
		{
			set {m_doc.SignalMax = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMin
		{
			set {m_doc.SignalMin = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalBandWidth
		{
			set {m_doc.SignalBandWidth = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalEffSampSize
		{
			set {m_doc.SignalEffSampSize = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqLow
		{
			set {m_doc.CalcFreqLow = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqHigh
		{
			set {m_doc.CalcFreqHigh = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcVoicingThd
		{
			set {m_doc.CalcVoicingThd = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcPercntChng
		{
			set {m_doc.CalcPercntChng = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcGrpSize
		{
			set {m_doc.CalcGrpSize = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcIntrpGap
		{
			set {m_doc.CalcIntrpGap = value;}
		}

		#endregion
	}
}
