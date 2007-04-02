using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("PaRecords")]
	public class RecordCache : List<RecordCacheEntry>, IDisposable
	{
		private WordCache m_wordCache;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCache()
		{
			RecordCacheEntry.ResetCounter();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			TempRecordCache.Dispose();
			if (m_wordCache != null)
			{
				m_wordCache.Clear();
				m_wordCache = null;
			}
			Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the word cache that was built from all the record entries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public WordCache WordCache
		{
			get { return m_wordCache; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes a PAXML file to a RecordCache instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RecordCache Load(PaDataSource dataSource)
		{
			if (dataSource == null)
				return null;

			string filename = dataSource.DataSourceFile;

			try
			{
				RecordCache cache =
					STUtils.DeserializeData(filename, typeof(RecordCache)) as RecordCache;

				if (cache != null)
				{
					string fwServer;
					string fwDBName;
					PaDataSource.GetPaXMLType(filename, out fwServer, out fwDBName);
					dataSource.FwServer = fwServer;
					dataSource.FwDBName = fwDBName;

					foreach (RecordCacheEntry entry in cache)
					{
						entry.PostDeserializeProcess(dataSource);

						if (entry.FieldValues.Count > 0 &&
							(entry.WordEntries == null || entry.WordEntries.Count == 0))
						{
							entry.NeedsParsing = true;
						}
					}
				}

				return cache;
			}
			catch (Exception e)
			{
				STUtils.STMsgBox(string.Format(Properties.Resources.kstidLoadingRecordCacheError,
					filename, "\n", e.Message), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Serializes the cache contents to an XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string filename)
		{
			try
			{
				STUtils.SerializeData(filename, this);
			}
			catch (Exception e)
			{
				STUtils.STMsgBox(string.Format(Properties.Resources.kstidSavingRecordCacheError,
					e.Message), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public WordCache BuildWordCache(ToolStripProgressBar progBar)
		{
			m_wordCache = new WordCache();

			foreach (RecordCacheEntry entry in this)
			{
				if (progBar != null)
					progBar.Increment(1);

				// A record entry doesn't need parsing if it came from a PAXML data source.
				// In that case, a word cache entry only needs to have two things done here:
				// 1) have its owning record entry set and 2) it needs to be added to the
				// word cache.
				if (entry.NeedsParsing)
					ParseEntry(entry);
				else
				{
					foreach (WordCacheEntry wentry in entry.WordEntries)
					{
						m_wordCache.Add(wentry);
						wentry.RecordEntry = entry;
					}
				}
			}

			PaApp.WordCache = m_wordCache;
			return m_wordCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a single record, going through its fields and parsing them individually
		/// as needed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ParseEntry(RecordCacheEntry entry)
		{
			PaFieldInfo phoneticField = PaApp.FieldInfo.PhoneticField;

			if (phoneticField != null)
				TempRecordCache.Add(entry.Id, entry[phoneticField.FieldName]);

			entry.WordEntries = new List<WordCacheEntry>();

			// Parse interlinear fields first, if there are any.
			if (entry.HasInterlinearData)
				ParseEntryAsInterlinear(entry);
			

			// If we didn't parse any interlinear fields or the phonetic wasn't among
			// them, make sure it gets parsed before any other non interlinear fields.
			if (phoneticField != null && phoneticField.IsParsed &&
				(entry.InterlinearFields == null ||
				!entry.InterlinearFields.Contains(phoneticField.FieldName)))
			{
				ParseSingleFieldInEntry(entry, phoneticField);
			}
			
			// Parse all the non phonetic, non interlinear fields.
			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
			{
				if (fieldInfo.IsParsed && !fieldInfo.IsPhonetic &&
					(entry.InterlinearFields == null ||
					!entry.InterlinearFields.Contains(fieldInfo.FieldName)))
				{
					ParseSingleFieldInEntry(entry, fieldInfo);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parses a non interlinear field (if necessary) and saves the field contents in one
		/// or more word cache entries. Parsing will depend on the data source's parse type
		/// and the field being parsed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ParseSingleFieldInEntry(RecordCacheEntry entry, PaFieldInfo fieldInfo)
		{
			entry.NeedsParsing = false;
			string unparsedData = entry[fieldInfo.FieldName];

			if (string.IsNullOrEmpty(unparsedData))
				return;

			// If we're not dealing with the phonetic field then check if our parsing type is
			// only phonetic or none at all. If either casecase then do nothing which will cause
			// any reference to the word cache entry's value for the field to defer to the
			// value that's stored in the word cache entry's owning record entry.
			if (!fieldInfo.IsPhonetic &&
				(entry.DataSource.ParseType == DataSourceParseType.PhoneticOnly ||
				entry.DataSource.ParseType == DataSourceParseType.None))
			{
				return;
			}

			// By this time we know we're dealing with one of three conditions: 1) the
			// field is phonetic or 2) the field should be parsed or 3) both 1 and
			// 2. When the field should be parsed then split it into individual words.
			string[] split = (entry.DataSource.ParseType == DataSourceParseType.None ?
				new string[] {unparsedData} : 
				unparsedData.Split(IPACharCache.kBreakChars.ToCharArray(),
					StringSplitOptions.RemoveEmptyEntries));

			for (int i = 0; i < split.Length; i++)
			{
				// Expand the capacity for more word entries if necessary.
				if (i == entry.WordEntries.Count)
				{
					entry.WordEntries.Add(new WordCacheEntry(true));
					entry.WordEntries[i].RecordEntry = entry;
					entry.WordEntries[i].WordIndex = i;
					m_wordCache.Add(entry.WordEntries[i]);
				}

				entry.WordEntries[i][fieldInfo.FieldName] = split[i];
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
		private void ParseEntryAsInterlinear(RecordCacheEntry entry)
		{
			string firstInterlinearLine = entry[entry.FirstInterlinearField];

			if (string.IsNullOrEmpty(firstInterlinearLine))
				return;

			// Get the width of each interlinear column.
			List<int> colWidths = GetInterlinearColumnWidths(firstInterlinearLine);

			// Store the unparsed interlinear lines in a collection of strings, then remove
			// those lines from the record cache entry so they no longer take up space.
			Dictionary<string, string> unparsedLines = new Dictionary<string, string>();
			foreach (string field in entry.InterlinearFields)
			{
				unparsedLines[field] = entry[field];
				entry.SetValue(field, null);
			}

			// Now parse each interlinear line.
			int i = 0;
			int wordIndex = 0;
			for (int w = 0; w < colWidths.Count; w++)
			{
				WordCacheEntry wordEntry = new WordCacheEntry(true);
				wordEntry.RecordEntry = entry;
				wordEntry.WordIndex = wordIndex++;
				
				foreach (KeyValuePair<string, string> line in unparsedLines)
				{
					if (line.Value != null && i < line.Value.Length)
					{
						wordEntry[line.Key] =
							(i + colWidths[w] >= line.Value.Length || w == colWidths.Count - 1 ?
							line.Value.Substring(i).Trim() :
							line.Value.Substring(i, colWidths[w]).Trim());

						if (line.Key == firstInterlinearLine)
							wordEntry.SetFieldAsFirstLineInterlinear(line.Key);
						else
							wordEntry.SetFieldAsSubordinateInterlinear(line.Key);
					}
				}

				entry.WordEntries.Add(wordEntry);
				m_wordCache.Add(wordEntry);
				i += colWidths[w];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private List<int> GetInterlinearColumnWidths(string firstInterlinearLine)
		{
			int i = 0;
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because of how dashes are treated in interlinear text, it makes parsing the data
		/// a little complicated. Therefore, all spaces directly before and after dashes are
		/// replaced with object replacement characters (ORCs). After parsing, the ORCs will
		/// be restored to spaces.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Dictionary<string, string> FormatForEasierParsing(Dictionary<string, string> fields)
		{
			Dictionary<string, string> converted = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> data in fields)
			{
				string[] splits = data.Value.Split("-".ToCharArray());
				StringBuilder bldr = new StringBuilder(data.Value.Length);
				
				// Were there any dashes in the text?
				if (splits.Length == 1)
				{
					converted[data.Key] = data.Value;
					continue;
				}

				for (int i = 0; i < splits.Length; i++)
				{
					char[] chrs = splits[i].ToCharArray();
					if (chrs.Length > 0)
					{
						// Replace any spaces at the beginning with ORCs.
						int c = 0;
						while (c < chrs.Length && chrs[c] == ' ')
							chrs[c++] = DataUtils.kOrc;

						// Replace any spaces at the end with ORCs.
						c = splits[i].Length - 1;
						while (c >= 0 && chrs[c] == ' ')
							chrs[c--] = DataUtils.kOrc;
					}

					bldr.Append(chrs);
					if (i < splits.Length - 1)
						bldr.Append('-');
				}

				converted[data.Key] = bldr.ToString();
			}

			return converted;
		}
	}

	#region TempRecordCache class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This temporary cache is used to store the full, original phonetic data from each record
	/// read from a data source. It is not saved in memory but is loaded when the user wants to
	/// jump to the source editor of an SFM data source and the field for jumping is phonetic.
	/// The reason the original is stored is because PA's working version may have been
	/// converted due to specified ambiguous item conversions.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TempRecordCache
	{
		private static string s_tmpFilename;
		private static Dictionary<int, string> s_cache;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an entry to the tempoary record cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Add(int id, string phonetic)
		{
			if (string.IsNullOrEmpty(phonetic))
				return;

			if (s_cache == null)
				s_cache = new Dictionary<int, string>();

			s_cache[id] = phonetic;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Dictionary<int, string> Load()
		{
			if (!File.Exists(s_tmpFilename))
				return null;

			List<TempRecordCacheEntry> tmpList = STUtils.DeserializeData(s_tmpFilename,
				typeof(List<TempRecordCacheEntry>)) as List<TempRecordCacheEntry>;

			if (tmpList == null)
				return null;

			s_cache = new Dictionary<int, string>();
			foreach (TempRecordCacheEntry entry in tmpList)
				s_cache[entry.Id] = entry.Phonetic;

			tmpList.Clear();
			tmpList = null;
			return s_cache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Save()
		{
			if (s_cache == null)
				return;

			if (string.IsNullOrEmpty(s_tmpFilename))
				s_tmpFilename = Path.GetTempFileName();

			List<TempRecordCacheEntry> tmpList = new List<TempRecordCacheEntry>();
			foreach (KeyValuePair<int, string> entry in s_cache)
				tmpList.Add(new TempRecordCacheEntry(entry.Key, entry.Value));

			STUtils.SerializeData(s_tmpFilename, tmpList);
			s_cache.Clear();
			tmpList.Clear();
			s_cache = null;
			tmpList = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets rid of the temporary cache file and clears out the temporary cache list. This
		/// Dispose is not the same as IDisposable's Dispose.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Dispose()
		{
			if (File.Exists(s_tmpFilename))
				File.Delete(s_tmpFilename);

			if (s_cache != null)
				s_cache.Clear();
	
			s_cache = null;
			s_tmpFilename = null;
		}
	}

	#endregion

	#region TempRecordCacheEntry class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("OriginalPhonetic")]
	public class TempRecordCacheEntry
	{
		[XmlAttribute("RecordId")]
		public int Id;
		[XmlAttribute]
		public string Phonetic;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Needed for Deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TempRecordCacheEntry()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs and initializes an TempRecordCacheEntry object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TempRecordCacheEntry(int id, string phonetic)
		{
			Id = id;
			Phonetic = phonetic;
		}
	}

	#endregion
}