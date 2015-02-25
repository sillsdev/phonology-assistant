// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Localization;
using SIL.Pa.UI.Dialogs;
using SilTools;

namespace SIL.Pa.DataSource.Sa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Annotation type for 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum AnnotationType
	{
		/// ------------------------------------------------------------------------------------
		Phonetic = 0,
		/// ------------------------------------------------------------------------------------
		Tone = 1,
		/// ------------------------------------------------------------------------------------
		Phonemic = 2,
		/// ------------------------------------------------------------------------------------
		Orthographic = 3,
		/// ------------------------------------------------------------------------------------
		Gloss = 4,
		/// ------------------------------------------------------------------------------------
		Reference = 5,
		/// ------------------------------------------------------------------------------------
		MusicPhraseLevel1 = 6,
		/// ------------------------------------------------------------------------------------
		MusicPhraseLevel2 = 7,
		/// ------------------------------------------------------------------------------------
		MusicPhraseLevel3 = 8,
		/// ------------------------------------------------------------------------------------
		MusicPhraseLevel4 = 9,
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Class for writing documents modified in SA.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SaAudioDocumentReader
	{
		private SaAudioDocument m_doc;
		private uint m_eticEnumIndex;
		private uint m_emicEnumIndex;
		private uint m_toneEnumIndex;
		private uint m_orthoEnumIndex;
		private uint m_markEnumIndex;
		private readonly BackgroundWorker m_worker;

		/// ------------------------------------------------------------------------------------
		public SaAudioDocumentReader(BackgroundWorker worker)
		{
			m_worker = worker;
		}

		#region Initialize/Close methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes an object to read SA data from the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(string audioFilePath)
		{
			return Initialize(audioFilePath, false);
		}

		/// ------------------------------------------------------------------------------------
		public static void ShowWaveFileNotFoundMsg(string path)
		{
			var msg = LocalizationManager.GetString(
				"Miscellaneous.Messages.DataSourceReading.WaveFileNotFoundMsg",
				"The wave  file '{0}' cannot be found.");
			
			Utils.MsgBox(string.Format(msg, Utils.PrepFilePathForMsgBox(path)));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes an object to read SA data from the SA database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Initialize(string audioFilePath, bool isForTmpOperation)
		{
			m_doc = SaAudioDocument.Load(audioFilePath, isForTmpOperation, true);
			
			if (m_doc != null)
			{
				ResetSegmentEnumerators();
				return true;
			}

			try
			{
				using (var audioReader = new AudioReader())
				{
					var result = audioReader.Initialize(audioFilePath);
					if (result == AudioReader.InitResult.FileNotFound)
					{
						ShowWaveFileNotFoundMsg(audioFilePath);
						return false;
					}

					if ((result == AudioReader.InitResult.InvalidFormat))
					{
						var msg = LocalizationManager.GetString("Miscellaneous.Messages.DataSourceReading.InvalidWaveFileMsg",
							"The file '{0}' is not a valid wave file.");
						
						Utils.MsgBox(string.Format(msg, Utils.PrepFilePathForMsgBox(audioFilePath)));
						return false;
					}

					// If audio file is old SA format, then tell user to use SA 3.0.1 to convert first.
					if (audioReader.IsOldSaFormat)
					{
						if (m_worker != null)
							m_worker.ReportProgress(-1);

						var msg = LocalizationManager.GetString(
							"Miscellaneous.Messages.DataSourceReading.AudioFileNeedsConversionMsg",
							"It appears the audio file '{0}' may have been created using an old version " + 
							"of Speech Analyzer. In order for {1} to read data associated with this audio " + 
							"file it must first be converted using Speech Analyzer 3.0.1.");

						msg = string.Format(msg, audioFilePath, Application.ProductName);
						using (var dlg = new DownloadSaDlg(msg))
							dlg.ShowDialog();
						
						return false;
					}

					// Now try reading the companion transcription file again.
					m_doc = SaAudioDocument.Load(audioFilePath, isForTmpOperation, false);
					ResetSegmentEnumerators();
				}
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString(
					"Miscellaneous.Messages.DataSourceReading.ErrorInitializingSaDocumentReaderMsg",
					"Error initializing SA Document reader.");
				
				Palaso.Reporting.ErrorReport.ReportNonFatalExceptionWithMessage(e, msg);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is really necessary since the garbage collector should handle clearing
		/// memory. But this is provided so COM clients can force freeing some of the memory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Close()
		{
			m_doc = null;
		}

		#endregion

		#region Methods for enumerating segment information
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resets the enumerating indexes for enumerating through the segments.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ResetSegmentEnumerators()
		{
			m_eticEnumIndex = 0;
			m_emicEnumIndex = 0;
			m_toneEnumIndex = 0;
			m_orthoEnumIndex = 0;
			m_markEnumIndex = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the next annotation information for the specified annotation type. Returns
		/// false when no segment was read and there are no more to read.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ReadSegment(int annotationType, out uint offset, out uint length,
			out string annotation)
		{
			offset = length = 0;
			annotation = null;
			
			if (m_doc == null)
				return false;

			switch ((AnnotationType)annotationType)
			{
				case AnnotationType.Phonetic:
					return ReadPhoneticSegment(out offset, out length, out annotation);

				case AnnotationType.Phonemic:
					return ReadPhonemicSegment(out offset, out length, out annotation);

				case AnnotationType.Tone:
					return ReadToneSegment(out offset, out length, out annotation);

				case AnnotationType.Orthographic:
					return ReadOrthographicSegment(out offset, out length, out annotation);
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the next phonetic annotation information. Returns false when
		/// no segment was read and there are no more to read.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ReadPhoneticSegment(out uint offset, out uint length, out string annotation)
		{
			while (m_doc.m_segments != null && m_eticEnumIndex < m_doc.m_segments.Count &&
				m_doc.m_segments[m_eticEnumIndex].Phonetic == null)
			{
				m_eticEnumIndex++;
			}

			bool ret = ReadSegNumbers(m_eticEnumIndex, out offset, out length);
			annotation = (ret ? m_doc.m_segments[m_eticEnumIndex++].Phonetic : null);
			return ret;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the next phonemic annotation information. Returns false when
		/// no segment was read and there are no more to read.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ReadPhonemicSegment(out uint offset, out uint length, out string annotation)
		{
			while (m_doc.m_segments != null && m_emicEnumIndex < m_doc.m_segments.Count &&
				m_doc.m_segments[m_emicEnumIndex].Phonemic == null)
			{
				m_emicEnumIndex++;
			}

			bool ret = ReadSegNumbers(m_emicEnumIndex, out offset, out length);
			annotation = (ret ? m_doc.m_segments[m_emicEnumIndex++].Phonemic : null);
			return ret;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the next tone annotation information. Returns false when
		/// no segment was read and there are no more to read.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ReadToneSegment(out uint offset, out uint length, out string annotation)
		{
			while (m_doc.m_segments != null && m_toneEnumIndex < m_doc.m_segments.Count &&
				m_doc.m_segments[m_toneEnumIndex].Tone == null)
			{
				m_toneEnumIndex++;
			}

			bool ret = ReadSegNumbers(m_toneEnumIndex, out offset, out length);
			annotation = (ret ? m_doc.m_segments[m_toneEnumIndex++].Tone : null);
			return ret;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the next orthographic annotation information. Returns false when
		/// no segment was read and there are no more to read.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ReadOrthographicSegment(out uint offset, out uint length, out string annotation)
		{
			while (m_doc.m_segments != null && m_orthoEnumIndex < m_doc.m_segments.Count &&
				m_doc.m_segments[m_orthoEnumIndex].Orthographic == null)
			{
				m_orthoEnumIndex++;
			}

			bool ret = ReadSegNumbers(m_orthoEnumIndex, out offset, out length);
			annotation = (ret ? m_doc.m_segments[m_orthoEnumIndex++].Orthographic : null);
			return ret;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the next "word" level mark annotation information. Returns false when
		/// no segment was read and there are no more to read.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ReadMarkSegment(out uint offset, out uint length, out string gloss,
			out string partOfSpeech, out string reference, out bool isBookmark)
		{
			// Get the intialization out of the way.
			offset = length = 0;
			gloss = partOfSpeech = reference = null;
			isBookmark = false;
		
			// Check if there is no record for the audio file or we've finished reading segments.
			if (m_doc == null || m_doc.m_segments == null || m_markEnumIndex == m_doc.m_segments.Count)
				return false;

			// Increment the mark enumerator until we find a non-zero mark length
			// (or until there are no more segments to enumerate).
			while (m_markEnumIndex < m_doc.m_segments.Count && m_doc.m_segments[m_markEnumIndex].MarkDuration == 0)
				m_markEnumIndex++;

			// If we didn't find the beginning of a mark segment then there's nothing left to do.
			if (m_markEnumIndex == m_doc.m_segments.Count)
				return false;

			offset = m_doc.m_segments[m_markEnumIndex].Offset;
			length = m_doc.m_segments[m_markEnumIndex].MarkDuration;
			gloss = m_doc.m_segments[m_markEnumIndex].Gloss;
			partOfSpeech = m_doc.m_segments[m_markEnumIndex].PartOfSpeech;
			reference = m_doc.m_segments[m_markEnumIndex].Reference;
			isBookmark = m_doc.m_segments[m_markEnumIndex].IsBookmark;
			m_markEnumIndex++;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the annotation offset and length for the specified index.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ReadSegNumbers(uint enumIndex, out uint offset, out uint length)
		{
			// Check if there is no record for the audio file or we've finished reading segments.
			if (m_doc == null || m_doc.m_segments == null || enumIndex == m_doc.m_segments.Count)
			{
				offset = 0;
				length = 0;
				return false;
			}

			offset = m_doc.m_segments[enumIndex].Offset;
			length = m_doc.m_segments[enumIndex].Duration;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///  Gets a collection of the words in the audio document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortedDictionary<uint, AudioDocWords> Words
		{
			get
			{
				var words = GetMarkInfo();
				ResetSegmentEnumerators();

				if (words != null)
				{
					BuildAnnotationWords(AnnotationType.Phonetic, words);
					BuildAnnotationWords(AnnotationType.Phonemic, words);
					BuildAnnotationWords(AnnotationType.Tone, words);
					BuildAnnotationWords(AnnotationType.Orthographic, words);
					return words;
				}

				// At this point we know there were no mark segments added to the audio
				// document in SA to indicate word boundaries. Therefore combine all the
				// existing segments into single words for each annotation type. This
				// should fix JIRA issue SPM-404.
				words = new SortedDictionary<uint, AudioDocWords>();
				words[0] = new AudioDocWords();

				BuildSingleAnnotationWord(AnnotationType.Phonetic, words[0]);
				BuildSingleAnnotationWord(AnnotationType.Phonemic, words[0]);
				BuildSingleAnnotationWord(AnnotationType.Tone, words[0]);
				BuildSingleAnnotationWord(AnnotationType.Orthographic, words[0]);

				return (string.IsNullOrEmpty(words[0].m_words[AnnotationType.Phonetic]) &&
					string.IsNullOrEmpty(words[0].m_words[AnnotationType.Phonemic]) &&
					string.IsNullOrEmpty(words[0].m_words[AnnotationType.Tone]) &&
					string.IsNullOrEmpty(words[0].m_words[AnnotationType.Orthographic]) ?
					null : words);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds the word from segments in the specified annotation type. An understanding
		/// of how SA handles annotations, glosses and references is important to understanding
		/// what's going on in this method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildAnnotationWords(AnnotationType atype,
			SortedDictionary<uint, AudioDocWords> words)
		{
			uint offset;
			uint length;
			uint firstSegOffset = 0;
			uint lengthSum = 0;
			string segment;
			var bldr = new StringBuilder();
			AudioDocWords prevAdw = null;

			// Read all the segments for the annotation type.
			while (ReadSegment((int)atype, out offset, out length, out segment))
			{
				AudioDocWords currWord;
				
				// We'll only use lengthSum and firstSegOffset in the case when the first
				// word's offset isn't the same as the offset of the first phonetic segment. 
				lengthSum += length;
				if (bldr.Length == 0)
					firstSegOffset = offset;

				// When the offset for the current segment matches one already in the
				// collection of words we know we've come to the beginning of the next
				// word (or the first word if the string builder is empty).
				if (words.TryGetValue(offset, out currWord))
				{
					// If we have a word that's been constructed, save it and reset the
					// builder to accept the next word coming down the pike.
					if (bldr.Length > 0)
					{
						// This should only happen when the first word's offset is not the same as
						// the first phonetic segment's offset. When that happens, we need to add
						// a word at the beginning of the collection to accomodate the fact that
						// the audio file contains one or more phonetic segments at the beginning
						// of the transcription that do not belong to a word.
						if (prevAdw == null)
						{
							prevAdw = new AudioDocWords();
							prevAdw.AudioLength = lengthSum;
							words[firstSegOffset] = prevAdw;
						}

						prevAdw.m_words[atype] = bldr.ToString();
						bldr.Length = 0;
					}

					// Save a reference to the AudioDocWords object so we can
					// store in it the word we're just beginning to construct.
					prevAdw = currWord;
				}

				bldr.Append(segment);
			}

			// Make sure to save the last word constructed.
			if (bldr.Length > 0 && prevAdw != null)
				prevAdw.m_words[atype] = bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there are no mark segments added to the audio document in SA to indicate word
		/// boundaries, the assumption is that all segments belong to a single word. Therefore
		/// all segments found for the specified annotation type will be combined into a single
		/// word in the specified AudioDocWords object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildSingleAnnotationWord(AnnotationType atype, AudioDocWords adw)
		{
			if (adw == null)
				return;

			uint offset;
			uint length;
			string segment;
			var bldr = new StringBuilder();

			// Read all the segments for the annotation type.
			while (ReadSegment((int)atype, out offset, out length, out segment))
				bldr.Append(segment);

			// Make sure to save the last word constructed.
			if (bldr.Length > 0)
				adw.m_words[atype] = bldr.ToString();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of Mark chunk words. An understanding
		/// of how SA handles annotations, glosses and references is important to understanding
		/// what's going on in this method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SortedDictionary<uint, AudioDocWords> GetMarkInfo()
		{
			var words = new SortedDictionary<uint, AudioDocWords>();
			ResetSegmentEnumerators();
			uint offset;
			uint length;
			bool isBkMrk;
			string gloss;
			string pos;
			string reference;

			while (ReadMarkSegment(out offset, out length, out gloss, out pos, out reference, out isBkMrk))
			{
				AudioDocWords adw = new AudioDocWords();
				adw.m_words[AnnotationType.Gloss] = gloss;
				adw.m_words[AnnotationType.Reference] = reference;
				adw.AudioLength = length;
				words[offset] = adw;
			}

			return (words.Count == 0 ? null : words);
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not, after initialization, the document was
		/// found in the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool DocumentExistsInDB
		{
			get { return (m_doc != null); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the MD5 hash code of the audio file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string MD5HashCode
		{
			get { return (m_doc == null ? null : m_doc.MD5HashCode); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the speakeer's name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SpeakerName
		{
			get {return (m_doc == null ? null : m_doc.SpeakerName);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the speaker's gender.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public char SpeakerGender
		{
			get { return (m_doc == null ? ' ' : m_doc.SpeakerGender);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the ethnologue Id of langauge.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string EthnologueId
		{
			get {return (m_doc == null ? null : m_doc.EthnologueId);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the region in which langauge is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Region
		{
			get {return (m_doc == null ? null : m_doc.Region);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the country in which language is spoken.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Country
		{
			get {return (m_doc == null ? null : m_doc.Country);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the language family.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Family
		{
			get {return (m_doc == null ? null : m_doc.Family);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the language name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string LanguageName
		{
			get {return (m_doc == null ? null : m_doc.LanguageName);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the dialect.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Dialect
		{
			get {return (m_doc == null ? null : m_doc.Dialect);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the notebook reference.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NoteBookReference
		{
			get {return (m_doc == null ? null : m_doc.NoteBookReference);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the free form translation
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FreeFormTranslation
		{
			get {return (m_doc == null ? null : m_doc.FreeFormTranslation);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of person who transcribed the audio data.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Transcriber
		{
			get {return (m_doc == null ? null : m_doc.Transcriber);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the size of the data chunk in the audio. (This length does not include any
		/// pad byte).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DataChunkSize
		{
			get {return (m_doc == null ? 0 : m_doc.DataChunkSize);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int FormatTag
		{
			get {return (m_doc == null ? 0 : m_doc.FormatTag);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Channels
		{
			get {return (m_doc == null ? 0 : m_doc.Channels);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SamplesPerSecond
		{
			get {return (m_doc == null ? 0 : m_doc.SamplesPerSecond);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int AverageBytesPerSecond
		{
			get {return (m_doc == null ? 0 : m_doc.AverageBytesPerSecond);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BlockAlignment
		{
			get {return (m_doc == null ? 0 : m_doc.BlockAlignment);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the value from the fmt Chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitsPerSample
		{
			get {return (m_doc == null ? 0 : m_doc.BitsPerSample);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Description from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SADescription
		{
			get {return (m_doc == null ? null : m_doc.SADescription);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Flags from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SAFlags
		{
			get {return (m_doc == null ? 0 : m_doc.SAFlags);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordFileFormat
		{
			get {return (m_doc == null ? 0 : m_doc.RecordFileFormat);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordTimeStamp
		{
			get {return (m_doc == null ? 0 : m_doc.RecordTimeStamp);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordBandWidth
		{
			get {return (m_doc == null ? 0 : m_doc.RecordBandWidth);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int RecordSampleSize
		{
			get {return (m_doc == null ? 0 : m_doc.RecordSampleSize);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NumberOfSamples
		{
			get {return (m_doc == null ? 0 : m_doc.NumberOfSamples);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMax
		{
			get {return (m_doc == null ? 0 : m_doc.SignalMax);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalMin
		{
			get {return (m_doc == null ? 0 : m_doc.SignalMin);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalBandWidth
		{
			get {return (m_doc == null ? 0 : m_doc.SignalBandWidth);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the sa chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SignalEffSampSize
		{
			get {return (m_doc == null ? 0 : m_doc.SignalEffSampSize);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqLow
		{
			get {return (m_doc == null ? 0 : m_doc.CalcFreqLow);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcFreqHigh
		{
			get {return (m_doc == null ? 0 : m_doc.CalcFreqHigh);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcVoicingThd
		{
			get {return (m_doc == null ? 0 : m_doc.CalcVoicingThd);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcPercntChng
		{
			get {return (m_doc == null ? 0 : m_doc.CalcPercntChng);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcGrpSize
		{
			get {return (m_doc == null ? 0 : m_doc.CalcGrpSize);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get from the utt chunk
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int CalcIntrpGap
		{
			get {return (m_doc == null ? 0 : m_doc.CalcIntrpGap);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the time the audio document was last modified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DateTime LastModified
		{
			get { return (m_doc == null ? DateTime.MinValue : m_doc.LastModifiedAsDateTime); }
		}

		#endregion
	}

	#region AudioDocWords
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Stores information for a single word in a audio file transcription.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AudioDocWords
	{
		internal SortedDictionary<AnnotationType, string> m_words;

		/// ------------------------------------------------------------------------------------
		public uint AudioLength;

		/// ------------------------------------------------------------------------------------
		public AudioDocWords()
		{
			m_words = new SortedDictionary<AnnotationType, string>();
			m_words[AnnotationType.Phonetic] = null;
			m_words[AnnotationType.Phonemic] = null;
			m_words[AnnotationType.Tone] = null;
			m_words[AnnotationType.Orthographic] = null;
			m_words[AnnotationType.Gloss] = null;
			m_words[AnnotationType.Reference] = null;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public string Phonetic { get { return m_words[AnnotationType.Phonetic]; } }
		/// ------------------------------------------------------------------------------------
		public string Phonemic { get { return m_words[AnnotationType.Phonemic]; } }
		/// ------------------------------------------------------------------------------------
		public string Tone { get { return m_words[AnnotationType.Tone]; } }
		/// ------------------------------------------------------------------------------------
		public string Orthographic { get { return m_words[AnnotationType.Orthographic]; } }
		/// ------------------------------------------------------------------------------------
		public string Gloss { get { return m_words[AnnotationType.Gloss]; } }
		/// ------------------------------------------------------------------------------------
		public string Reference { get { return m_words[AnnotationType.Reference]; } }
	
		#endregion
	}

	#endregion
}
