// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: IPACharCache.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// An in-memory cache of the IPASymbol table.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneticParser
	{
		private const char kParseTokenMarker = '\u0001';
		private readonly string _ambigTokenFmt = kParseTokenMarker + "{0}";

		private AmbiguousSequences _sortedAmbiguousSeqList;
		private readonly AmbiguousSequences _unsortedAmbiguousSeqList;
		private readonly TranscriptionChanges _transcriptionChanges;

		/// ------------------------------------------------------------------------------------
		public PhoneticParser(AmbiguousSequences ambigSeqs, TranscriptionChanges transChanges)
		{
			_unsortedAmbiguousSeqList = ambigSeqs ?? new AmbiguousSequences();
			BuildSortedAmbiguousSequencesList();
			_transcriptionChanges = transChanges;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use this only for tests.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ResetAmbiguousSequencesForTests()
		{
			_sortedAmbiguousSeqList = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will build a list of ambiguous sequences that's in order of their length and
		/// will include the tone letters as well. The order is longest to shortest with those
		/// with the same lengths, staying in the order in which the user entered them in the
		/// Phone Inventory view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildSortedAmbiguousSequencesList()
		{
			_sortedAmbiguousSeqList = new AmbiguousSequences();

			// Copy the references from the specified list to our own.
			if (_unsortedAmbiguousSeqList != null)
			{
				foreach (var seq in _unsortedAmbiguousSeqList)
					_sortedAmbiguousSeqList.Add(seq);
			}

			// Go through the tone letters collection and add them to the ambiguous list.
			// Tone letters are special in that they're the only IPA characters in the
			// cache that are made up of multiple code points. When it comes to parsing a
			// phonetic string into its phones, tone letters need to be treated as
			// ambiguous sequences.
			if (App.IPASymbolCache.ToneLetters != null)
			{
				foreach (var info in App.IPASymbolCache.ToneLetters.Values.Where(info =>
					!_sortedAmbiguousSeqList.ContainsSeq(info.Literal, true)))
				{
					_sortedAmbiguousSeqList.Add(new AmbiguousSeq(info.Literal));
				}
			}

			// Now order the items in the list based on the length
			// of the ambiguous sequence -- longest to shortest.
			_sortedAmbiguousSeqList.SortByUnitLength();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the PhoneticParse method should
		/// add to the UndefinedCharacters collection any characters it runs across that
		/// aren't found in the phonetic character inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool LogUndefinedCharactersWhenParsing { get; set; }

		#region Ambiguous Sequence Finding
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scans the phonetic transcription, checking for possible ambiguous sequences at the
		/// beginning of each "word" within the transcription. These are the only ambiguous
		/// sequences the program will find automatically.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> FindAmbiguousSequences(string phonetic)
		{
			// Return an empty array if there's nothing in the phonetic.
			if (string.IsNullOrEmpty(phonetic))
				return null;

			phonetic = FFNormalizer.Normalize(phonetic);
			phonetic = _transcriptionChanges.Convert(phonetic);
			string[] words = phonetic.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			var bldr = new StringBuilder();
			var ambigSeqs = new List<string>();

			foreach (var word in words.Where(w => w.Length > 0))
			{
				foreach (char c in word)
				{
					var ci = App.IPASymbolCache[c];

					if (ci == null || ci.IsBase || ci.IsUndefined)
					{
						// If there's already something in the builder it means
						// we've previously found some non base characters before
						// the current one that we assume belong to the current one.
						if (bldr.Length > 0)
							bldr.Append(c);

						break;
					}

					bldr.Append(c);
				}

				if (bldr.Length > 0)
				{
					ambigSeqs.Add(bldr.ToString());
					bldr.Length = 0;
				}
			}

			return (ambigSeqs.Count > 0 ? ambigSeqs : null);
		}

		#endregion

		#region Phonetic string parser
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into a new string of phones delimited
		/// by commas.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string PhoneticParser_CommaDelimited(string phonetic, bool normalize,
			bool removeDuplicates)
		{
			Dictionary<int, string[]> uncertainPhones;
			var phones = Parse(phonetic, normalize, out uncertainPhones);

			if (phones == null)
				return null;
			
			if (removeDuplicates)
				phones = phones.Distinct(StringComparer.Ordinal).ToArray();

			var commaDelimitedPhones = new StringBuilder();
			foreach (var p in phones)
				commaDelimitedPhones.AppendFormat("{0},", p);

			return commaDelimitedPhones.ToString().TrimEnd(',');
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Parse(string phonetic, bool normalize)
		{
			Dictionary<int, string[]> uncertainPhones;
			return Parse(phonetic, normalize, out uncertainPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Parse(string phonetic, bool normalize,
			bool convertExperimentalTranscriptions)
		{
			Dictionary<int, string[]> uncertainPhones;
			return Parse(phonetic, normalize, convertExperimentalTranscriptions,
				out uncertainPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Parse(string phonetic, bool normalize,
			out Dictionary<int, string[]> uncertainPhones)
		{
			return Parse(phonetic, normalize, true, out uncertainPhones);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses the specified phonetic string into an array of phones.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string[] Parse(string phonetic, bool normalize,
			bool convertExperimentalTranscriptions, out Dictionary<int, string[]> uncertainPhones)
		{
			uncertainPhones = null;

			// Return an empty array if there's nothing in the phonetic.
			if (string.IsNullOrEmpty(phonetic))
				return null;

			var phones = new List<string>(phonetic.Length);
			IPASymbol ciPrev = null;

			// Normalize the string if necessary.
			if (normalize)
			    phonetic = FFNormalizer.Normalize(phonetic);

			var origPhoneticRunBeingParsed = phonetic;

			if (convertExperimentalTranscriptions)
				phonetic = _transcriptionChanges.Convert(phonetic);
	
			phonetic = MarkAmbiguousSequences(phonetic);

			int phoneStart = 0;
			for (int i = 0; i < phonetic.Length; i++)
			{
				char c = phonetic[i];
				char badChar = '\0';

				// Check if we've run into a marker indicating
				// the beginning of an ambiguous sequence.
				if (c == kParseTokenMarker)
				{
					// First, close the previous phone if there is one.
					if (i > phoneStart)
						phones.Add(phonetic.Substring(phoneStart, i - phoneStart));

					var ambigPhone =
						_sortedAmbiguousSeqList.GetAmbigSeqForToken(phonetic[++i]);

					if (!string.IsNullOrEmpty(ambigPhone))
						phones.Add(ambigPhone);

					phoneStart = i + 1;
					continue;
				}

				// Get the information for the current codepoint.
				var ciCurr = App.IPASymbolCache[c];

				// If there's no information for a code point or there is but there isn't
				// any for the previous character and the current character isn't a base
				// character, then treat the character as it's own phone.
				if (ciCurr == null || ciCurr.Type == IPASymbolType.notApplicable)
				{
					if (i > phoneStart)
						phones.Add(phonetic.Substring(phoneStart, i - phoneStart));

					// Check if we're at the beginning of an uncertain phone group
					if (c != '(')
					{
						phoneStart = i + 1;
						badChar = c;
					}
					else
					{
						int index = i + 1;
						var primaryPhone = GetUncertainties(phonetic, ref index,
							phones.Count, ref uncertainPhones, origPhoneticRunBeingParsed);

						// Primary phone should only be null when no slash was found
						// between the parentheses. In that situation, the parentheses are
						// not considered to be surrounding a group of uncertain phones.
						if (primaryPhone == null)
							badChar = c;
						else
						{
							phones.Add(primaryPhone);
							i = index;
						}

						phoneStart = i + 1;
					}

					ciPrev = null;

					if (badChar != '\0')
					{
						// Log the undefined character.
						if (LogUndefinedCharactersWhenParsing && App.IPASymbolCache.UndefinedCharacters != null)
							App.IPASymbolCache.UndefinedCharacters.Add(c, origPhoneticRunBeingParsed);

						phones.Add(c.ToString());
					}

					continue;
				}

				// If we've encountered a non base character but nothing precedes it,
				// then it must be a diacritic at the beginning of the phonetic
				// transcription so just put it with the following characters.
				if (ciPrev == null && !ciCurr.IsBase)
					continue;

				// Is the previous codepoint special in that it's not a base character
				// but a base character must follow it in the same phone (e.g. a tie bar)?
				// If yes, then make sure the current codepoint is a base character or
				// throw it away.
				if (ciPrev != null && ciPrev.CanPrecedeBase)
				{
					ciPrev = ciCurr;
					continue;
				}

				// At this point, if the current codepoint is a base character and
				// it's not the first in the string, close the previous phone. If
				// ciCurr.IsBase && i > phoneStart but ciPrev == null then it means
				// we've run across some non base characters at the beginning of the
				// transcription that aren't attached to a base character. Therefore,
				// attach them to the first base character that's found. In that case,
				// we don't want to add the phone to the collection yet. We'll wait
				// until we come across the beginning of the next phone.
				if (ciCurr.IsBase && i > phoneStart && ciPrev != null)
				{
					phones.Add(phonetic.Substring(phoneStart, i - phoneStart));
					phoneStart = i;
				}

				ciPrev = ciCurr;
			}

			// Save the last phone
			if (phoneStart < phonetic.Length)
				phones.Add(phonetic.Substring(phoneStart));

			return phones.ToArray();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method marks ambiguous sequences in the specified phonetic string.
		/// 
		/// The process of marking ambiguous sequences in a phonetic string involves replacing
		/// a series of characters that is recognied as an ambiguous sequence with two
		/// characters. The first character is always the same and informs the parsing process
		/// that what follows is an ambiguous sequence token. The second character is the
		/// token. Tokens uniquely identify the sequence of characters being replaced.
		/// 
		/// After all ambiguous sequences in a phonetic string are marked, the process of
		/// parsing a phonetic string into phones will find the tokens, adding the phones they
		/// represent to the collection of phones being built for the phonetic string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string MarkAmbiguousSequences(string phonetic)
		{
			if (_sortedAmbiguousSeqList == null && _unsortedAmbiguousSeqList != null)
				BuildSortedAmbiguousSequencesList();
			
			if (_sortedAmbiguousSeqList != null)
			{
				foreach (var seq in _sortedAmbiguousSeqList.Where(s => s.Convert))
				{
					phonetic = phonetic.Replace(seq.Literal,
						string.Format(_ambigTokenFmt, seq.ParseToken));
				}
			}

			return phonetic;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a list of uncertain phones and returns them in a collection.
		/// </summary>
		/// <param name="phonetic">Phonetic word containing uncertainties.</param>
		/// <param name="i">Starts out as the index of the charcter just after the open
		/// parenthesis. By the time the method returns, i is the index of the closed
		/// parenthesis.</param>
		/// <param name="phoneNumber">The index into the array of phones of the phone for
		/// which the uncertainties are possibilities.</param>
		/// <param name="uncertainPhones">Collection of uncertain phones found between
		/// the parenthese.</param>
		/// <param name="origPhoneticRunBeingParsed"></param>
		/// <returns>The first phone in the list of uncertain phones. It is treated
		/// as the primary (i.e. most likely candidate) phone.</returns>
		/// ------------------------------------------------------------------------------------
		private string GetUncertainties(string phonetic, ref int i, int phoneNumber,
			ref Dictionary<int, string[]> uncertainPhones, string origPhoneticRunBeingParsed)
		{
			if (uncertainPhones == null)
				uncertainPhones = new Dictionary<int, string[]>();

			int savCharPointer = i;
			string tmpPhone;
			var bldrPhone = new StringBuilder();
			var phones = new List<string>();

			for (; i < phonetic.Length && phonetic[i] != ')'; i++)
			{
				if (phonetic[i] != '/' && phonetic[i] != ',' && phonetic[i] != kParseTokenMarker)
				{
					bldrPhone.Append(phonetic[i]);
					continue;
				}

				if (phonetic[i] != kParseTokenMarker)
				{
					// Save the previous phone, if there is one, checking if the phone is the
					// empty set character. If it's the empty set, then just save an empty string.
					tmpPhone = bldrPhone.ToString();
					if (IPASymbolCache.UncertainGroupAbsentPhoneChars.Contains(tmpPhone))
						tmpPhone = string.Empty;

					phones.Add(tmpPhone);
					bldrPhone.Length = 0;
					continue;
				}

				// Ambiguous sequences in uncertain groups are actually redundant since
				// the slashes and the final close parenthesis should mark sequences that
				// are to be kept together as phones. But, we have to check for them...
				// just in case.
				var ambigPhone = _sortedAmbiguousSeqList.GetAmbigSeqForToken(phonetic[i + 1]);
				if (!string.IsNullOrEmpty(ambigPhone))
				{
					// Replace the token marker and the token with the ambiguous sequence.
					var dualCharMarker = string.Format(_ambigTokenFmt, phonetic[i + 1]);
					phonetic = phonetic.Replace(dualCharMarker, ambigPhone);
					bldrPhone.Append(phonetic[i]);
				}
			}

			// If we reached the end of the string it means we didn't
			// find the closed parenthesis, which is an error condition.
			if (i == phonetic.Length)
			{
				// TODO: Log error.
				return null;
			}

			// When the following is true, it's a special case (i.e. when a
			// parentheses group was found but didn't contain any slashes).
			// If that happens, then assume it isn't a uncertain group.
			if (phones.Count == 0)
			{
				i = savCharPointer;
				return null;
			}

			// REVIEW: Should the code points in the uncertain phones be validated
			// against those in the IPA cache?

			// If the phone is the empty set character just save an empty string.
			tmpPhone = bldrPhone.ToString();
			if (IPASymbolCache.UncertainGroupAbsentPhoneChars.Contains(tmpPhone))
				tmpPhone = string.Empty;

			// Save the last uncertain phone and add the list of uncertain phones to
			// the collection of those for the word
			phones.Add(tmpPhone);
			uncertainPhones[phoneNumber] = phones.ToArray();

			if (App.IPASymbolCache.UndefinedCharacters != null)
				ValidateCodepointsInUncertainPhones(phones, origPhoneticRunBeingParsed);

			return phones[0];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks all the Unicode values in the list of uncertain phones to make sure they
		/// are in PA's character code inventory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ValidateCodepointsInUncertainPhones(IEnumerable<string> phones,
			string origPhoneticRunBeingParsed)
		{
			foreach (char chr in phones.SelectMany(p => p.Where(c => !App.IPASymbolCache.ContainsKey(c))))
			{
				// Get the information for the current codepoint.
				App.IPASymbolCache.UndefinedCharacters.Add(chr, origPhoneticRunBeingParsed);
			}
		}

		#endregion
	}
}
