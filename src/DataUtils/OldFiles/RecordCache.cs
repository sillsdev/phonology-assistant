using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.SpeechTools.Database
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("PaRecords")]
	public class RecordCache : List<RecordCacheEntry>
	{
		private WordCache m_wordCache;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RecordCache() : base()
		{
			DBUtils.RecordCache = this;
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
		/// Serializes the cache contents to an XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string filename)
		{
			try
			{
				DBUtils.SerializeData(filename, this);
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

				entry.WordEntries = new List<WordCacheEntry>();
				Dictionary<string, string> interlinearFields = null;

				foreach (PaFieldInfo fieldInfo in DBUtils.FieldInfo)
				{
					if (fieldInfo.IsParsed)
						ParseEntry(entry, fieldInfo.FieldName);
				}
			}
				
			//    if (entry.FirstInterlinearField != null)
			//        interlinearFields = new Dictionary<string, string>();

			//    if (entry.Phonetic != null)
			//    {
			//        if (interlinearFields != null)
			//            interlinearFields[DBFields.Phonetic] = entry.Phonetic;
			//        else
			//        {
			//            ParseEntry(entry, entry.Phonetic, DBFields.Phonetic);
			//            entry.Phonetic = null;
			//        }
			//    }

			//    if (entry.Phonemic != null)
			//    {
			//        if (interlinearFields != null)
			//            interlinearFields[DBFields.Phonemic] = entry.Phonemic;
			//        else
			//        {
			//            ParseEntry(entry, entry.Phonemic, DBFields.Phonemic);
			//            entry.Phonemic = null;
			//        }
			//    }

			//    if (entry.Tone != null)
			//    {
			//        if (interlinearFields != null)
			//            interlinearFields[DBFields.Tone] = entry.Tone;
			//        else
			//        {
			//            ParseEntry(entry, entry.Tone, DBFields.Tone);
			//            entry.Tone = null;
			//        }
			//    }

			//    if (entry.Gloss != null)
			//    {
			//        if (interlinearFields != null)
			//            interlinearFields[DBFields.Gloss] = entry.Gloss;
			//        else
			//        {
			//            ParseEntry(entry, entry.Gloss, DBFields.Gloss);
			//            entry.Gloss = null;
			//        }
			//    }

			//    if (entry.Ortho != null)
			//    {
			//        if (interlinearFields != null)
			//            interlinearFields[DBFields.Ortho] = entry.Ortho;
			//        else
			//        {
			//            ParseEntry(entry, entry.Ortho, DBFields.Ortho);
			//            entry.Ortho = null;
			//        }
			//    }

			//    if (entry.POS != null)
			//    {
			//        if (interlinearFields != null)
			//            interlinearFields[DBFields.POS] = entry.POS;
			//        else
			//        {
			//            ParseEntry(entry, entry.POS, DBFields.POS);
			//            entry.POS = null;
			//        }
			//    }

			//    if (entry.CVPattern != null)
			//    {
			//        if (interlinearFields != null)
			//            interlinearFields[DBFields.CVPattern] = entry.CVPattern;
			//        else
			//        {
			//            ParseEntry(entry, entry.CVPattern, DBFields.CVPattern);
			//            entry.CVPattern = null;
			//        }
			//    }

			//    if (interlinearFields != null)
			//        ParseEntryAsInterlinear(interlinearFields);
			//}

			DBUtils.WordCache = m_wordCache;
			return m_wordCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ParseEntry(RecordCacheEntry entry, string field)
		{
			string unparsedData = entry[field];

			if (string.IsNullOrEmpty(unparsedData))
				return;
			
			// Split up the string into individual words.
			string[] split = unparsedData.Split(" ".ToCharArray(),
				StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < split.Length; i++)
			{
				// Expand the capacity for more word entries if necessary.
				if (i == entry.WordEntries.Count)
				{
					entry.WordEntries.Add(new WordCacheEntry());
					entry.WordEntries[i].RecordEntry = entry;
					entry.WordEntries[i].WordIndex = i;
					m_wordCache.Add(entry.WordEntries[i]);
				}

				entry.WordEntries[i][field] = split[i];
				//// Set the property in the word cache entry.
				//BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public |
				//    BindingFlags.SetValue | BindingFlags.Instance;

				//typeof(WordCacheEntry).InvokeMember(property, flags,
				//    null, entry.WordEntries[i], new object[] {split[i]});
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
		private Dictionary<string, List<string>> ParseEntryAsInterlinear(Dictionary<string, string> fields)
		{
			fields = FormatForEasierParsing(fields);

			Dictionary<string, List<string>> parsedData = new Dictionary<string, List<string>>();
			Dictionary<string, bool> endOfLine = new Dictionary<string, bool>();
			Dictionary<string, StringBuilder> builders = new Dictionary<string, StringBuilder>();

			foreach (string field in fields.Keys)
				builders[field] = new StringBuilder();

			foreach (string field in fields.Keys)
				parsedData[field] = new List<string>();

			// Loop through the data values, one character at a time and build a list of strings
			// for each data field, each list of strings representing a different column.
			int i = 0;
			while (true)
			{
				int endOfLineCount = 0;
				int endOfWordCount = 0;
			    foreach (KeyValuePair<string, string> data in fields)
			    {
					if (i >= data.Value.Length)
					{
						endOfLineCount++;
						endOfWordCount++;
					}
					else if (data.Value[i] == ' ')
						endOfWordCount++;
					else
						builders[data.Key].Append(data.Value[i]);
			    }

				if (endOfWordCount == fields.Count || endOfLineCount == fields.Count)
				{
					foreach (string field in fields.Keys)
					{
						parsedData[field].Add(builders[field].ToString().Replace(DBUtils.kOrc, ' '));
						builders[field].Length = 0;
					}
				}

				if (endOfWordCount == fields.Count)
					break;

				i++;
			}

			return parsedData;
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
							chrs[c++] = DBUtils.kOrc;

						// Replace any spaces at the end with ORCs.
						c = splits[i].Length - 1;
						while (c >= 0 && chrs[c] == ' ')
							chrs[c--] = DBUtils.kOrc;
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
}
