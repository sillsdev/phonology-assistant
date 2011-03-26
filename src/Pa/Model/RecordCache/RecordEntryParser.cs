using System;
using System.Collections.Generic;
using System.Linq;
using SIL.Pa.DataSource;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class provides the parsing code to go through the fields of a record entry and,
	/// for those that should be saved at the word level, will split them up into their
	/// individual words, saving each in the word entry collection owned by the record entry.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class RecordEntryParser
	{
		private readonly string m_phoneticFieldName;
		private readonly Action<int, string> TmpRecCacheAddAction;

		/// ------------------------------------------------------------------------------------
		public RecordEntryParser(string phoneticFieldName, Action<int, string> tmpRecCacheAddAction)
		{
			m_phoneticFieldName = phoneticFieldName;
			TmpRecCacheAddAction = tmpRecCacheAddAction;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a single record, going through its fields and parsing them into individual
		/// word entries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ParseEntry(RecordCacheEntry entry)
		{
			TmpRecCacheAddAction(entry.Id, entry[m_phoneticFieldName]);
			entry.WordEntries = new List<WordCacheEntry>();

			// Parse interlinear fields first, if there are any.
			if (entry.HasInterlinearData)
				ParseEntryAsInterlinear(entry);

			var eticField = entry.Project.GetPhoneticField();
//			var eticMapping = entry.DataSource.FieldMappings.Single(m => m.Field.Name == eticField.Name);

			// If we didn't parse any interlinear fields or the phonetic wasn't among
			// them, make sure it gets parsed before any other non interlinear fields.
			//if (eticMapping.IsParsed && !entry.GetIsInterlinearField(eticField.Name))
			//    ParseSingleFieldInEntry(entry, eticField);
			
			// I've commented out the check above because it doesn't seem right that the
			// phonetic mapping's IsParsed can be false. Therefore, I will always parse
			// it. I hesitate to delete the commented out code in case I'm overlooking
			// something.
			if (!entry.GetIsInterlinearField(eticField.Name))
				ParseSingleFieldInEntry(entry, eticField);

			// Parse all the non phonetic, non interlinear fields.
			foreach (var mapping in entry.DataSource.FieldMappings.Where(m => m.IsParsed &&
				m.Field.Type != FieldType.Phonetic && !entry.GetIsInterlinearField(m.Field.Name)))
			{
				ParseSingleFieldInEntry(entry, mapping.Field);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a non interlinear field (if necessary) and saves the field contents in one
		/// or more word cache entries. Parsing will depend on the data source's parse type
		/// and the field being parsed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ParseSingleFieldInEntry(RecordCacheEntry entry, PaField field)
		{
			entry.NeedsParsing = false;
			string unparsedData = entry[field.Name];

			if (string.IsNullOrEmpty(unparsedData))
				return;

			// If we're not dealing with the phonetic field then check if our parsing type is
			// only phonetic or none at all. If either casecase then do nothing which will cause
			// any reference to the word cache entry's value for the field to defer to the
			// value that's stored in the word cache entry's owning record entry.
			if (field.Type != FieldType.Phonetic &&
				(entry.DataSource.ParseType == DataSourceParseType.PhoneticOnly ||
				entry.DataSource.ParseType == DataSourceParseType.None))
			{
				return;
			}

			// By this time we know we're dealing with one of three conditions: 1) the
			// field is phonetic or 2) the field should be parsed or 3) both 1 and
			// 2. When the field should be parsed then split it into individual words.
			string[] split = (entry.DataSource.ParseType == DataSourceParseType.None ?
				new[] { unparsedData } : unparsedData.Split(App.BreakChars.ToCharArray(),
					StringSplitOptions.RemoveEmptyEntries));

			for (int i = 0; i < split.Length; i++)
			{
				// Expand the capacity for more word entries if necessary.
				if (i == entry.WordEntries.Count)
					entry.WordEntries.Add(new WordCacheEntry(entry, i));

				entry.WordEntries[i].SetValue(field.Name, split[i]);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// \tx nimeolewa.
		/// \mb ni-  me - oa    -le  -wa
		/// \ge 1S-  PF - marry -DER -PASS
		/// \ps pro- tns- v     -sfx -sfx
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void ParseEntryAsInterlinear(RecordCacheEntry entry)
		{
			string firstInterlinearLine = entry[entry.FirstInterlinearField];

			if (string.IsNullOrEmpty(firstInterlinearLine))
				return;

			// Get the width of each interlinear column.
			var colWidths = GetInterlinearColumnWidths(firstInterlinearLine);

			// Store the unparsed interlinear lines in a collection of strings, then remove
			// those lines from the record cache entry so they no longer take up space.
			var unparsedLines = entry.InterlinearFields.ToDictionary(f => f, f => entry[f]);
			foreach (var field in entry.InterlinearFields)
				entry.SetValue(field, null);

			// Now parse each interlinear line.
			int startIndex = 0;
			int wordIndex = 0;
			foreach (int width in colWidths)
			{
				var wordEntry = new WordCacheEntry(entry, wordIndex++);

				foreach (var line in unparsedLines.Where(l => l.Value != null && startIndex < l.Value.Length))
				{
					wordEntry[line.Key] = (startIndex + width > line.Value.Length ?
						line.Value.Substring(startIndex) : line.Value.Substring(startIndex, width)).Trim();

					if (line.Key == entry.FirstInterlinearField)
						wordEntry.SetFieldAsFirstLineInterlinear(line.Key);
					else
						wordEntry.SetFieldAsSubordinateInterlinear(line.Key);
				}

				entry.WordEntries.Add(wordEntry);
				startIndex += width;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static List<int> GetInterlinearColumnWidths(string firstInterlinearLine)
		{
			int i;
			int start = 0;
			List<int> colWidths = new List<int>();

			while ((i = firstInterlinearLine.IndexOf(' ', start)) >= 0)
			{
				while (i < firstInterlinearLine.Length && firstInterlinearLine[i] == ' ')
					i++;

				if (i == firstInterlinearLine.Length)
					break;

				colWidths.Add(i - start);
				start = i;
			}

			colWidths.Add(firstInterlinearLine.Length - start);
			return colWidths;
		}
	}
}
