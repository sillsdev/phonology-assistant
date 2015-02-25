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
using System.Text;
using SIL.SpeechTools;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.SaDataSourceAddOn
{
	internal class FixedAudioDocReader
	{
		private const string kNullSegment = "\xFFFF";

		private SaAudioDocument m_doc = null;
		private uint m_eticEnumIndex = 0;
		private uint m_emicEnumIndex = 0;
		private uint m_toneEnumIndex = 0;
		private uint m_orthoEnumIndex = 0;
		private uint m_markEnumIndex = 0;
		private SortedDictionary<uint, SegmentData> m_segments;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void Read(RecordCacheEntry recEntry)
		{
			if (recEntry == null || !System.IO.File.Exists(recEntry.DataSource.DataSourceFile))
				return;

			if (!Initialize(recEntry.DataSource.DataSourceFile))
				return;

			m_segments = ReflectionHelper.GetField(m_doc, "m_segments") as
				SortedDictionary<uint, SegmentData>;

			if (m_segments == null)
				return;

			SortedDictionary<uint, AudioDocWords> adWords = GetWords();
			if (adWords == null)
				return;

			int wordIndex = 0;
			PaFieldInfo fieldInfo;
			recEntry.WordEntries = new List<WordCacheEntry>();

			// Go through each word, adding a word cache entry for each.
			foreach (KeyValuePair<uint, AudioDocWords> adw in adWords)
			{
				WordCacheEntry wentry = new WordCacheEntry(recEntry, wordIndex++, true);

				Dictionary<string, PaFieldValue> fieldValues =
					ReflectionHelper.GetField(wentry, "m_fieldValues") as
					Dictionary<string, PaFieldValue>;

				if (fieldValues != null)
					fieldValues["Reference"] = new PaFieldValue("Reference");

				fieldInfo = PaApp.FieldInfo.PhoneticField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Value.Phonetic;

				fieldInfo = PaApp.FieldInfo.PhonemicField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Value.Phonemic;

				fieldInfo = PaApp.FieldInfo.ToneField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Value.Tone;

				fieldInfo = PaApp.FieldInfo.OrthoField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Value.Orthographic;

				fieldInfo = PaApp.FieldInfo.GlossField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Value.Gloss;

				fieldInfo = PaApp.FieldInfo.ReferenceField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Value.Reference;

				fieldInfo = PaApp.FieldInfo.AudioFileLengthField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Value.AudioLength.ToString();

				fieldInfo = PaApp.FieldInfo.AudioFileOffsetField;
				if (fieldInfo != null)
					wentry[fieldInfo.FieldName] = adw.Key.ToString();

				recEntry.WordEntries.Add(wentry);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool Initialize(string audioFilePath)
		{
			m_doc = SaAudioDocument.Load(audioFilePath, false, true);
			if (m_doc != null)
			{
				ResetSegmentEnumerators();
				return true;
			}

			try
			{
				using (AudioReader audioReader = new AudioReader())
				{
					AudioReader.InitResult result = audioReader.Initialize(audioFilePath);
					if (result == AudioReader.InitResult.FileNotFound)
						return false;

					if ((result == AudioReader.InitResult.InvalidFormat))
						return false;

					// Try reading data from older SA audio files, converting
					// it to Unicode along the way.
					if (!audioReader.Read(true))
						return false;

					// Now try reading the companion transcription file again.
					m_doc = SaAudioDocument.Load(audioFilePath, false, false);
					ResetSegmentEnumerators();
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

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
			while (m_segments != null && m_eticEnumIndex < m_segments.Count &&
				m_segments[m_eticEnumIndex].Phonetic == null)
			{
				m_eticEnumIndex++;
			}

			bool ret = ReadSegNumbers(m_eticEnumIndex, out offset, out length);
			annotation = (ret ? m_segments[m_eticEnumIndex++].Phonetic : null);
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
			if (m_segments != null && m_emicEnumIndex < m_segments.Count &&
				m_segments[m_emicEnumIndex].Phonemic == null)
			{
				m_segments[m_emicEnumIndex].Phonemic = kNullSegment;
			}

			bool ret = ReadSegNumbers(m_emicEnumIndex, out offset, out length);
			annotation = (ret ? m_segments[m_emicEnumIndex++].Phonemic : null);
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
			if (m_segments != null && m_toneEnumIndex < m_segments.Count &&
				m_segments[m_toneEnumIndex].Tone == null)
			{
				m_segments[m_toneEnumIndex].Tone = kNullSegment;
			}

			bool ret = ReadSegNumbers(m_toneEnumIndex, out offset, out length);
			annotation = (ret ? m_segments[m_toneEnumIndex++].Tone : null);
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
			if (m_segments != null && m_orthoEnumIndex < m_segments.Count &&
				m_segments[m_orthoEnumIndex].Orthographic == null)
			{
				m_segments[m_orthoEnumIndex].Orthographic = kNullSegment;
			}

			bool ret = ReadSegNumbers(m_orthoEnumIndex, out offset, out length);
			annotation = (ret ? m_segments[m_orthoEnumIndex++].Orthographic : null);
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
			if (m_doc == null || m_segments == null || m_markEnumIndex == m_segments.Count)
				return false;

			// Increment the mark enumerator until we find a non-zero mark length
			// (or until there are no more segments to enumerate).
			while (m_markEnumIndex < m_segments.Count && m_segments[m_markEnumIndex].MarkDuration == 0)
				m_markEnumIndex++;

			// If we didn't find the beginning of a mark segment then there's nothing left to do.
			if (m_markEnumIndex == m_segments.Count)
				return false;

			offset = m_segments[m_markEnumIndex].Offset;
			length = m_segments[m_markEnumIndex].MarkDuration;
			gloss = m_segments[m_markEnumIndex].Gloss;
			partOfSpeech = m_segments[m_markEnumIndex].PartOfSpeech;
			reference = m_segments[m_markEnumIndex].Reference;
			isBookmark = m_segments[m_markEnumIndex].IsBookmark;
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
			if (m_doc == null || m_segments == null || enumIndex == m_segments.Count)
			{
				offset = 0;
				length = 0;
				return false;
			}

			offset = m_segments[enumIndex].Offset;
			length = m_segments[enumIndex].Duration;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///  Gets a collection of the words in the audio document.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortedDictionary<uint, AudioDocWords> GetWords()
		{
			SortedDictionary<uint, AudioDocWords> words = GetMarkInfo();
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

			SortedDictionary<AnnotationType, string> wrds =
				ReflectionHelper.GetField(words[0], "m_words") as
				SortedDictionary<AnnotationType, string>;

			if (wrds == null)
				return null;

			return (string.IsNullOrEmpty(wrds[AnnotationType.Phonetic]) &&
				string.IsNullOrEmpty(wrds[AnnotationType.Phonemic]) &&
				string.IsNullOrEmpty(wrds[AnnotationType.Tone]) &&
				string.IsNullOrEmpty(wrds[AnnotationType.Orthographic]) ?
				null : words);
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
			StringBuilder bldr = new StringBuilder();
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

						SortedDictionary<AnnotationType, string> wrds =
							ReflectionHelper.GetField(prevAdw, "m_words") as
							SortedDictionary<AnnotationType, string>;

						if (wrds != null)
							wrds[atype] = bldr.ToString().Replace(kNullSegment, string.Empty);
	
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
			{
				SortedDictionary<AnnotationType, string> wrds =
					ReflectionHelper.GetField(prevAdw, "m_words") as
					SortedDictionary<AnnotationType, string>;
				
				if (wrds != null)
					wrds[atype] = bldr.ToString().Replace(kNullSegment, string.Empty);
			}
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
			StringBuilder bldr = new StringBuilder();

			// Read all the segments for the annotation type.
			while (ReadSegment((int)atype, out offset, out length, out segment))
				bldr.Append(segment);

			// Make sure to save the last word constructed.
			if (bldr.Length > 0)
			{
				SortedDictionary<AnnotationType, string> wrds =
					ReflectionHelper.GetField(adw, "m_words") as
					SortedDictionary<AnnotationType, string>;

				if (wrds != null)
					wrds[atype] = bldr.ToString();
			}
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
			SortedDictionary<uint, AudioDocWords> words = new SortedDictionary<uint, AudioDocWords>();
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

				SortedDictionary<AnnotationType, string> wrds =
					ReflectionHelper.GetField(adw, "m_words") as
					SortedDictionary<AnnotationType, string>;

				if (wrds != null)
				{
					wrds[AnnotationType.Gloss] = gloss;
					wrds[AnnotationType.Reference] = reference;
				}
				
				adw.AudioLength = length;
				words[offset] = adw;
			}

			return (words.Count == 0 ? null : words);
		}

		#endregion
	}
}
