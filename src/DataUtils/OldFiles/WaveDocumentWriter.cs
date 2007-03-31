using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Xml.Serialization;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for writing documents modified in SA.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class WaveDocumentWriter
	{
		protected Document m_document;
		protected SortedDictionary<int, SegmentData> m_segments;

		#region Constructor
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs a WaveDocumentWriter object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WaveDocumentWriter()
		{
			m_document = new Document();
			m_segments = new SortedDictionary<int, SegmentData>();
		}

		#endregion

		#region Methods for committing wave document to the database.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Commits to the database the data collected in WaveDocumentWriter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Commit()
		{
			SortedDictionary<int, DocumentWord> wordData;
			SortedDictionary<int, PhoneticWordInfo> eticWordInfo;
			PrepareSegmentsForCommit(out wordData, out eticWordInfo);
			m_document.Commit(wordData);
			CommitSegments(wordData);

			return m_document.Id;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will go through all the segments in a transcription and build the
		/// words and prepare the phonetic data for the PhoneticWriter to save in the DB.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void PrepareSegmentsForCommit(out SortedDictionary<int, DocumentWord> wordData,
			out SortedDictionary<int, PhoneticWordInfo> eticWordInfo)
		{
			wordData = null;
			eticWordInfo = null;

			if (m_segments.Count == 0)
				return;

			wordData = new SortedDictionary<int, DocumentWord>();
			eticWordInfo = new SortedDictionary<int, PhoneticWordInfo>();
			ArrayList phoneInfo = new ArrayList();

			StringBuilder phonetic = new StringBuilder();
			StringBuilder phonemic = new StringBuilder();
			StringBuilder ortho = new StringBuilder();
			StringBuilder tone = new StringBuilder();

			int wordNumber = 0;
			int segmentNumber = 1;
			KeyValuePair<int, SegmentData> wordBoundarySegment = new KeyValuePair<int, SegmentData>();

			// Loop through all the segments in the transcription
			foreach (KeyValuePair<int, SegmentData> segment in m_segments)
			{
				if (segment.Value.MarkDuration > 0)
				{
					if (segmentNumber > 1)
					{
						eticWordInfo[wordNumber] = new PhoneticWordInfo();
						eticWordInfo[wordNumber].Phones = PackagePhones(phoneInfo);
						wordData[wordNumber] = PackageWords(wordBoundarySegment, wordNumber,
							phonetic, phonemic, ortho, tone);
						wordData[wordNumber].m_eticWordInfo = eticWordInfo[wordNumber++];
					}

					wordBoundarySegment = segment;

					// If we're here and it's the last segment in the transcription, it means
					// the last segment is a word unto itself. Therefore, clear the counter
					// to indicate the last word has already been written.
					if (segmentNumber == m_segments.Count)
						segmentNumber = 0;
				}

				// Collect phonetic characters for a word.
				if (segment.Value.Phonetic != null)
				{
					phonetic.Append(segment.Value.Phonetic);
					phoneInfo.Add(new PhoneInfo(segment.Value.Phonetic));
				}

				// Collect phonemic characters for a word.
				if (segment.Value.Phonemic != null)
					phonemic.Append(segment.Value.Phonemic);

				// Collect orthographic characters for a word.
				if (segment.Value.Orthographic != null)
					ortho.Append(segment.Value.Orthographic);

				// Collect tone characters for a word.
				if (segment.Value.Tone != null)
					tone.Append(segment.Value.Tone);

				// Check if we need to save the last words worth of information.
				if (segmentNumber == m_segments.Count)
				{
					eticWordInfo[wordNumber] = new PhoneticWordInfo();
					eticWordInfo[wordNumber].Phones = PackagePhones(phoneInfo);
					wordData[wordNumber] = PackageWords(wordBoundarySegment, wordNumber,
						phonetic, phonemic, ortho, tone);
					wordData[wordNumber].m_eticWordInfo = eticWordInfo[wordNumber];
				}

				segmentNumber++;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method creates a DocumentWord object, fills it with the specified word
		/// information passed as arguments and returns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private DocumentWord PackageWords(KeyValuePair<int, SegmentData> segment, int annOffset,
			StringBuilder phonetic, StringBuilder phonemic, StringBuilder ortho, StringBuilder tone)
		{
			DocumentWord wordData = new DocumentWord(0);
			wordData.Gloss = segment.Value.Gloss;
			wordData.Reference = segment.Value.Reference;
			wordData.POS = segment.Value.PartOfSpeech;
			wordData.m_wavLength = segment.Value.MarkDuration;
			wordData.m_wavOffset = segment.Key;
			wordData.m_annotationOffset = annOffset;
			
			if (phonetic.Length > 0)
				wordData.Phonetic = phonetic.ToString();

			if (phonemic.Length > 0)
				wordData.Phonemic = phonemic.ToString();

			if (ortho.Length > 0)
				wordData.Ortho = ortho.ToString();

			if (tone.Length > 0)
				wordData.Tone = tone.ToString();

			phonetic.Length = phonemic.Length = ortho.Length = tone.Length = 0;

			return wordData;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method creates an array of PhoneInfo objects, one for each phonetic character
		/// in a word. That array is then returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private PhoneInfo[] PackagePhones(ArrayList phoneInfoArrayList)
		{
			PhoneInfo[] phoneInfo = (PhoneInfo[])phoneInfoArrayList.ToArray(typeof(PhoneInfo));
			phoneInfoArrayList.Clear();

			int order = 0;
			int offset = 0;

			foreach (PhoneInfo pi in phoneInfo)
			{
				// Determine where the current phone falls in the word.
				if (phoneInfo.Length == 1)
					pi.Location = RelativePhoneLocation.Alone;
				else if (order == 0)
					pi.Location = RelativePhoneLocation.Initial;
				else if (order == phoneInfo.Length - 1)
					pi.Location = RelativePhoneLocation.Final;
				else
					pi.Location = RelativePhoneLocation.Medial;

				pi.Order = order++;
				pi.Offset = offset;
				offset += pi.Phone.Length;
			}

			return phoneInfo;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Writes all the segment information to the segment table.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CommitSegments(SortedDictionary<int, DocumentWord> wordData)
		{
			// First, delete all the records for the specified document id.
			string sql = DBUtils.DeleteRecordSQL("Segment", "DocId", m_document.Id);
			DBUtils.ExecuteNonSelectSQL(sql);

			PaDataTable table = new PaDataTable("Segment");

			int wordNumber = 0;
			int offset = 0;
			foreach (KeyValuePair<int, SegmentData> segment in m_segments)
			{
				// If this segment begins a word (and it's not the first word) then increment
				// the offset to account for a space between words, and increment the word
				// number so we'll get the correct AllWordIndexId from the wordData collection.
				if (segment.Value.MarkDuration > 0 && offset > 0)
				{
					offset++;
					wordNumber++;
				}

				DataRow row = table.NewRow();
				row[DBFields.DocId] = m_document.Id;
				row["PhoneticChar"] = segment.Value.Phonetic;
				row["ToneChar"] = segment.Value.Tone;
				row["PhonemicChar"] = segment.Value.Phonemic;
				row["OrthoChar"] = segment.Value.Orthographic;
				row["HexPhoneticChar"] = DBUtils.StrToHex(segment.Value.Phonetic);
				row["PhoneticCharOffset"] = offset;
				row["WavOffset"] = segment.Key;
				row["WavLength"] = segment.Value.Duration;
				row["WordBeginning"] = (segment.Value.MarkDuration > 0);
				row[DBFields.AllWordIndexId] = wordData[wordNumber].AllWordIndexId;
				table.Rows.Add(row);
				table.Commit();

				// Increase the offset by the length of the phonetic character.
				offset += segment.Value.Phonetic.Length;
			}

			table = null;
		}

		#endregion

		#region Methods for adding segment information
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure a segment for the specified offset exists in the segment collection.
		/// If a segment for the specified offset doesn't exist, one is created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ValidateSegmentForOffset(int offset)
		{
			if (!m_segments.ContainsKey(offset))
				m_segments[offset] = new SegmentData();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a phonetic annotation string at the specified offset, with the specified
		/// length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddPhoneticSegment(int offset, int length, string annotation)
		{
			ValidateSegmentForOffset(offset);
			m_segments[offset].Phonetic = annotation;
			m_segments[offset].Duration = length;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a phonemic annotation string at the specified offset, with the specified
		/// length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddPhonemicSegment(int offset, int length, string annotation)
		{
			ValidateSegmentForOffset(offset);
			m_segments[offset].Phonemic = annotation;
			m_segments[offset].Duration = length;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a orthographic annotation string at the specified offset, with the specified
		/// length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddOrthographicSegment(int offset, int length, string annotation)
		{
			ValidateSegmentForOffset(offset);
			m_segments[offset].Orthographic = annotation;
			m_segments[offset].Duration = length;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds a tone annotation string at the specified offset, with the specified
		/// length.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddToneSegment(int offset, int length, string annotation)
		{
			ValidateSegmentForOffset(offset);
			m_segments[offset].Tone = annotation;
			m_segments[offset].Duration = length;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds information that spans one or more segment boundaries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddNonSegmentInfo(int offset, int length, string gloss, string reference,
			string partOfSpeech)
		{
			ValidateSegmentForOffset(offset);
			m_segments[offset].Gloss = gloss;
			m_segments[offset].Reference = reference;
			m_segments[offset].PartOfSpeech = partOfSpeech;
			m_segments[offset].MarkDuration = length;
		}

		#endregion

		#region Misc. Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is only temporary until the code is in place to use an MD5 key to find the
		/// database and document id associated with the wave document being written.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DatabaseFile
		{
			get { return DBUtils.DatabaseFile; }
			set { DBUtils.DatabaseFile = value; }
		}

		#endregion

		#region Properties for setting DocHeader Fields
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int FormatTag
		{
			set { m_document[DBFields.FormatTag] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Channels
		{
			set { m_document[DBFields.Channels] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SamplesPerSecond
		{
			set { m_document[DBFields.SamplesPerSecond] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int AverageBytesPerSecond
		{
			set { m_document[DBFields.AvgBytesPerSecond] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BlockAlignment
		{
			set { m_document[DBFields.BlockAlign] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitsPerSample
		{
			set { m_document[DBFields.BitsPerSample] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The length of a recording in seconds. Set this value by making the following
		/// calculation (data chunk length / BlockAlign) / SamplesPerSecond.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public double RecordingLength
		{
			set { m_document[DBFields.RecordingLength] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// RIFFVersion from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float SAVersion
		{
			set { m_document[DBFields.SAVersion] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Description from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SADescription
		{
			set { m_document[DBFields.SADescription] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Flags from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SAFlags
		{
			set { m_document[DBFields.SAFlags] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordFileFormat
		{
			set { m_document[DBFields.RecordFileFormat] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordTimeStamp
		{
			set { m_document[DBFields.RecordTimeStamp] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordBandWidth
		{
			set { m_document[DBFields.RecordBandWidth] = value; }
		}
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordSampleSize
		{
			set { m_document[DBFields.RecordSampleSize] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NumberOfSamples
		{
			set { m_document[DBFields.NumberOfSamples] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMax
		{
			set { m_document[DBFields.SignalMax] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMin
		{
			set { m_document[DBFields.SignalMin] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalBandWidth
		{
			set { m_document[DBFields.SignalBandWidth] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalEffSampSize
		{
			set { m_document[DBFields.SignalEffSampSize] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqLow
		{
			set { m_document[DBFields.CalcFreqLow] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqHigh
		{
			set { m_document[DBFields.CalcFreqHigh] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcVoicingThd
		{
			set { m_document[DBFields.CalcVoicingThd] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcPercntChng
		{
			set { m_document[DBFields.CalcPercntChng] = value; }
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcGrpSize
		{
			set { m_document[DBFields.CalcGrpSize] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcIntrpGap
		{
			set { m_document[DBFields.CalcIntrpGap] = value; }
		}

		#endregion

		#region Properties for setting speaker, language and reference fields
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the speaker's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Speaker
		{
			set { m_document[DBFields.SpeakerName] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the speaker's gender.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public char SpeakerGender
		{
			set
			{
				switch (value)
				{
					case 'M': m_document[DBFields.Gender] = Gender.Male; break;
					case 'F': m_document[DBFields.Gender] = Gender.Female; break;
					case 'C': m_document[DBFields.Gender] = Gender.Child; break;
					default: m_document[DBFields.Gender] = Gender.Unspecified; break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the ethnologue Id of langauge.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EthnologueId
		{
			set { m_document[DBFields.EthnologueId] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the region in which langauge is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Region
		{
			set { m_document[DBFields.Region] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the country in which language is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Country
		{
			set { m_document[DBFields.Country] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the language family.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Family
		{
			set { m_document[DBFields.Family] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the language name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageName
		{
			set { m_document[DBFields.LanguageName] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the dialect.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Dialect
		{
			set { m_document[DBFields.Dialect] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the note book reference.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NoteBookReference
		{
			set { m_document[DBFields.NoteBookRef] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the free form translation
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FreeFormTranslation
		{
			set { m_document[DBFields.Freeform] = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the name of person who transcribed the wave data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber
		{
			set { m_document[DBFields.Transcriber] = value; }
		}

		#endregion
	}
}
