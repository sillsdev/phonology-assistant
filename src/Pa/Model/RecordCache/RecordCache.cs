using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Localization;
using SIL.Pa.DataSource;
using SIL.Pa.PhoneticSearching;
using SIL.Pa.Processing;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is strictly for serializing and deserializing PaXML files.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaXMLContent
	{
		[XmlElement("PaRecords")]
		public RecordCache Cache;
	}

	/// ----------------------------------------------------------------------------------------
	public class RecordCache : List<RecordCacheEntry>, IDisposable
	{
		private string _phoneticFieldName;
		private PaProject _project;

		/// ------------------------------------------------------------------------------------
		public RecordCache()
		{
		}

		/// ------------------------------------------------------------------------------------
		public RecordCache(PaProject project)
		{
			_project = project;
			_phoneticFieldName = _project.GetPhoneticField().Name;
			PhoneCache = new PhoneCache(project);
			RecordCacheEntry.ResetCounter();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			TempRecordCache.Dispose();
			if (WordCache != null)
			{
				WordCache.Clear();
				WordCache = null;
			}

			Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the word cache that was built from all the record entries.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public WordCache WordCache { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of words that did not match the current filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public WordCache WordsNotInCurrentFilter { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PhoneCache PhoneCache { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cache of phones without respect to current filter.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public PhoneCache UnfilteredPhoneCache { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deserializes a PAXML file to a RecordCache instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RecordCache Load(PaDataSource dataSource, PaProject project)
		{
			if (dataSource == null)
				return null;

			string filename = dataSource.SourceFile;

			try
			{
				var paxmlcontent = XmlSerializationHelper.DeserializeFromFile<PaXMLContent>(filename);
				var cache = (paxmlcontent == null ? null : paxmlcontent.Cache);

				if (cache == null)
					return null;

				cache._project = project;
				cache._phoneticFieldName = project.GetPhoneticField().Name;
				string fwServer;
				string fwDBName;
				PaDataSource.GetPaXmlType(filename, out fwServer, out fwDBName);

				foreach (var entry in cache)
				{
					entry.PostDeserializeProcess(dataSource, project);

					if (entry.FieldValues.Count > 0 &&
						(entry.WordEntries == null || entry.WordEntries.Count == 0))
					{
						entry.NeedsParsing = true;
					}
				}

				return cache;
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString("Miscellaneous.Messages.DataSourceReading.LoadingRecordCacheErrorMsg",
					"The following error occurred while loading '{0}'",
					"Message displayed when failing to load a PaXml file.");

				App.NotifyUserOfProblem(e, msg, filename);
				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Serializes the cache contents to an XML file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string filename)
		{
			//try
			//{
			//    var paxmlcontent = new PaXMLContent();
			//    paxmlcontent.Cache = this;

			//    //paxmlcontent.CustomFields = new PaFieldInfoList();

			//    //foreach (var field in m_project.Fiel.Where(fi => fi.IsCustom))
			//    //    paxmlcontent.CustomFields.Add(fieldInfo);

			//    //if (paxmlcontent.CustomFields.Count == 0)
			//    //    paxmlcontent.CustomFields = null;

			//    XmlSerializationHelper.SerializeToFile(filename, paxmlcontent);
			//}
			//catch (Exception e)
			//{
			//    var msg = LocalizationManager.GetString("SavingRecordCacheErrorMsg", "Error saving records: {0}",
			//        "Message displayed if there was an error saving the records to an XML file.");
			//    Utils.MsgBox(string.Format(msg, e.Message));
			//}
		}

		/// ------------------------------------------------------------------------------------
		public void BuildWordCache(ToolStripProgressBar progBar)
		{
			FindWordInitialGeneratedAmbigousSequences();
			var tmpWordCache = new WordCache();
			var recEntryParser = new RecordEntryParser(_phoneticFieldName, TempRecordCache.Add);

			foreach (var entry in this)
			{
				if (progBar != null)
					progBar.Increment(1);

				//// A record entry doesn't need parsing if it came from a PAXML data source
				//// or from an SA data source.
				//// In that case, a word cache entry only needs to have two things done here:
				//// 1) have its owning record entry set and 2) it needs to be added to the
				//// word cache.
				if (entry.NeedsParsing)
					recEntryParser.ParseEntry(entry);

				foreach (var wentry in entry.WordEntries)
				{
					wentry.RecordEntry = entry;
					tmpWordCache.Add(wentry);
				}	
			}

			UnfilteredPhoneCache = GetPhonesFromWordCache(tmpWordCache);
			FindOtherGeneratedAmbiguousSequences();
			SearchEngine.PhoneCache = UnfilteredPhoneCache;
			BuildFilteredWordCache(tmpWordCache);
		}

		/// ------------------------------------------------------------------------------------
		public void BuildFilteredWordCache()
		{
			BuildFilteredWordCache(WordCache.Union(WordsNotInCurrentFilter));
		}

		/// ------------------------------------------------------------------------------------
		private void BuildFilteredWordCache(IEnumerable<WordCacheEntry> tmpWordCache)
		{
			var wordCache = tmpWordCache.ToArray();
			WordsNotInCurrentFilter = new WordCache();
			WordCache = new WordCache();
			PhoneCache = GetPhonesFromWordCache(wordCache);

			foreach (var wentry in wordCache)
			{
				if (_project.FilterHelper.EntryMatchesCurrentFilter(wentry))
					WordCache.Add(wentry);
				else
					WordsNotInCurrentFilter.Add(wentry);
			}

			PhoneCache = GetPhonesFromWordCache(WordCache);
			_project.FilterHelper.PostCacheBuildingFinalize();
			ProjectInventoryBuilder.Process(_project);
		}

		/// ------------------------------------------------------------------------------------
		private PhoneCache GetPhonesFromWordCache(IEnumerable<WordCacheEntry> wordCache)
		{
			var phoneCache = new PhoneCache(_project);

			foreach (var entry in wordCache)
			{
				var phones = entry.Phones;

				if (phones == null)
					continue;

				for (int i = 0; i < phones.Length; i++)
				{
					// Don't bother adding break characters.
					if (App.BreakChars.Contains(phones[i]))
						continue;

					if (!phoneCache.ContainsKey(phones[i]))
						phoneCache.AddPhone(phones[i]);

					// Determine if the current phone is the primary
					// phone in an uncertain group.
					bool isPrimaryUncertainPhone = (entry.ContiansUncertainties &&
						entry.UncertainPhones.ContainsKey(i));

					// When the phone is the primary phone in an uncertain group, we
					// don't add it to the total count but to the counter that keeps
					// track of the primary	uncertain phones. Then we also add to the
					// cache the non primary uncertain phones.
					if (!isPrimaryUncertainPhone)
						phoneCache[phones[i]].TotalCount++;
					else
					{
						phoneCache[phones[i]].CountAsPrimaryUncertainty++;

						// Go through the uncertain phones and add them to the cache.
						if (entry.ContiansUncertainties)
						{
							AddUncertainPhonesToCache(entry.UncertainPhones[i], phoneCache);
							UpdateSiblingUncertainties(entry.UncertainPhones, phoneCache);
						}
					}
				}
			}

			AddUndefinedCharsToCaches(phoneCache);
			return phoneCache;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates a uncertain phone sibling lists for each phone in each uncertain group for
		/// the specified uncertain groups.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void UpdateSiblingUncertainties(
			IDictionary<int, string[]> uncertainPhones, IDictionary<string, IPhoneInfo> phoneCache)
		{
			// Go through the uncertain phone groups
			foreach (var uPhones in uncertainPhones.Values)
			{
				// Go through the uncertain phones in this group.
				for (int i = 0; i < uPhones.Length; i++)
				{
					IPhoneInfo phoneUpdating;

					// TODO: Log an error when the phone isn't found in the the cache
					// Get the cache entry for the phone whose sibling list will be updated.
					if (!phoneCache.TryGetValue(uPhones[i], out phoneUpdating))
						continue;

					// Go through the sibling phones, adding them to
					// the updated phones sibling list.
					for (int j = 0; j < uPhones.Length; j++)
					{
						// Add the phone pointed to by j if it's not the phone whose
						// cache entry we're updating and if it's not a phone already
						// in the sibling list of the cache entry we're updating.
						if (j != i && !phoneUpdating.SiblingUncertainties.Contains(uPhones[j]))
						{
							phoneUpdating.SiblingUncertainties.Add(
								IPASymbolCache.UncertainGroupAbsentPhoneChars.Contains(uPhones[j]) ?
								IPASymbolCache.UncertainGroupAbsentPhoneChar : uPhones[j]);
						}
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through all the undefined phonetic characters found in data sources and adds
		/// temporary (i.e. as long as this session of PA is running) records for them in the
		/// IPA character cache and the phone cache.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddUndefinedCharsToCaches(PhoneCache phoneCache)
		{
			if (App.IPASymbolCache.UndefinedCharacters == null ||
				App.IPASymbolCache.UndefinedCharacters.Count == 0)
			{
				return;
			}

			foreach (var upci in App.IPASymbolCache.UndefinedCharacters)
			{
				App.IPASymbolCache.AddUndefinedCharacter(upci.Character);
				phoneCache.AddUndefinedPhone(upci.Character.ToString());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified list of uncertain phones to the phone cache. It is assumed the
		/// first (i.e. primary) phone in the list has already been added to the cache and,
		/// therefore, it will not be added nor its count incremented.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddUncertainPhonesToCache(string[] uncertainPhoneGroup,
			PhoneCache phoneCache)
		{
			// Go through the uncertain phone groups, skipping the
			// primary one in each group since that was already added
			// to the cache above.
			for (int i = 1; i < uncertainPhoneGroup.Length; i++)
			{
				string phone = uncertainPhoneGroup[i];

				// Don't bother adding break characters.
				if (!App.BreakChars.Contains(phone))
				{
					if (!phoneCache.ContainsKey(phone))
						phoneCache.AddPhone(phone);

					phoneCache[phone].CountAsNonPrimaryUncertainty++;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Scans the phonetic transcription in each record for possible ambiguous sequences
		/// at the beginning of words.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FindWordInitialGeneratedAmbigousSequences()
		{
			var sequences = (from entry in this
							select entry.GetValue(_phoneticFieldName) into phonetic
							where phonetic != null
							select _project.PhoneticParser.FindAmbiguousSequences(phonetic) into seqs
							where seqs != null
							select seqs);
			
			var generatedSeqs = new List<string>();
			foreach (var seqs in sequences)
				generatedSeqs.AddRange(seqs);

			if (generatedSeqs.Count > 0)
				_project.UpdateAbiguousSequencesWithGeneratedOnes(generatedSeqs.Distinct(StringComparer.Ordinal));
		}

		/// ------------------------------------------------------------------------------------
		private void FindOtherGeneratedAmbiguousSequences()
		{
			var generatedSeqs = UnfilteredPhoneCache.Keys
				.Where(p => p.Contains(App.kBottomTieBar) || p.Contains(App.kTopTieBar)).ToArray();

			if (generatedSeqs.Length > 0)
				_project.UpdateAbiguousSequencesWithGeneratedOnes(generatedSeqs.Distinct(StringComparer.Ordinal));
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
		public static Dictionary<int, string> Load()
		{
			if (!File.Exists(s_tmpFilename))
				return null;

			var tmpList = XmlSerializationHelper.DeserializeFromFile<List<TempRecordCacheEntry>>(s_tmpFilename);
			if (tmpList == null)
				return null;

			s_cache = tmpList.ToDictionary(e => e.Id, e => e.Phonetic);
			tmpList.Clear();
			return s_cache;
		}

		/// ------------------------------------------------------------------------------------
		public static void Save()
		{
			if (s_cache == null)
				return;

			if (string.IsNullOrEmpty(s_tmpFilename))
				s_tmpFilename = Path.GetTempFileName();

			var tmpList = s_cache.Select(e => new TempRecordCacheEntry(e.Key, e.Value)).ToList();
			XmlSerializationHelper.SerializeToFile(s_tmpFilename, tmpList);
			s_cache.Clear();
			tmpList.Clear();
			s_cache = null;
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
