using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SIL.Pa.DataSourceClasses.FieldWorks;
using SIL.Pa.Model;
using SIL.PaToFdoInterfaces;

namespace SIL.Pa.DataSource.FieldWorks
{
	public class Fw7DataSourceReader : IDisposable
	{
		private string m_phoneticFieldName;
		private string m_phoneticWsId;
        private string m_audioWsId;
        private PaProject m_project;
		private PaDataSource m_dataSource;
		private FwDataSourceInfo m_fwDsInfo;
		private BackgroundWorker m_worker;
        public Fw7CustomField m_customfield;

		/// ------------------------------------------------------------------------------------
		public static Fw7DataSourceReader Create(BackgroundWorker worker, PaProject project, PaDataSource ds)
		{
			var eticMapping = ds.FieldMappings.Single(m => m.Field.Type == FieldType.Phonetic);
			if (eticMapping == null)
				return null;

            var audioMapping = ds.FieldMappings.Single(m => m.Field.Type == FieldType.AudioFilePath);
            
			var reader = new Fw7DataSourceReader();
			reader.m_worker = worker;
			reader.m_project = project;
			reader.m_dataSource = ds;
			reader.m_fwDsInfo = ds.FwDataSourceInfo;
			reader.m_phoneticFieldName = eticMapping.NameInDataSource;
			reader.m_phoneticWsId = eticMapping.FwWsId;
            reader.m_audioWsId = audioMapping != null ? audioMapping.FwWsId : null;
            if (reader.m_dataSource != null)
            {
                reader.m_customfield = new Fw7CustomField(reader.m_dataSource);
            }
			return reader;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			m_worker = null;
			m_project = null;
			m_dataSource = null;
			m_fwDsInfo = null;
		}

		/// ------------------------------------------------------------------------------------
		public void Read(RecordCache recCache)
		{
			var allLexEntries = FwDBUtils.GetLexEntriesFromFw7Project(m_fwDsInfo).ToArray();
			m_worker.ReportProgress(allLexEntries.Length, m_dataSource.DisplayTextWhenReading);
            var customnames = m_customfield.FieldNames();
			foreach (var lxEntry in allLexEntries)
			{
				m_worker.ReportProgress(0);
				var entry = ReadSingleLexEntry(lxEntry);
                var customvalues = m_customfield.CustomValues.FindAll(m => m.Guid == lxEntry.Guid.ToString());
                SetCustomFieldsforEntry(customnames, customvalues, entry);
				if (entry != null)
					recCache.Add(entry);
			}

			m_dataSource.UpdateLastModifiedTime();
		}

	    private static void SetCustomFieldsforEntry(IEnumerable<string> customnames, List<Fw7CustomField.rt> customvalues, RecordCacheEntry entry)
	    {
	        foreach (var name in customnames)
	        {
	            foreach (var value in customvalues.Select(m => m.CustomFields))
	            {
	                var nm = value.SingleOrDefault(m => m.Name == name);
	                if (nm != null)
	                {
	                    entry.SetValue(name, nm.Value);
	                }
	            }
	        }
	    }

	    /// ------------------------------------------------------------------------------------
		private RecordCacheEntry ReadSingleLexEntry(IPaLexEntry lxEntry)
		{
			// Make a new record entry.
			var recCacheEntry = new RecordCacheEntry(false, m_project);
			recCacheEntry.DataSource = m_dataSource;
			recCacheEntry.NeedsParsing = false;
			recCacheEntry.WordEntries = new List<WordCacheEntry>();

			if (!ReadWordEntryFieldsFromLexEntry(lxEntry, recCacheEntry))
				return null;

			foreach (var mapping in m_dataSource.FieldMappings)
			{
				var wsId = mapping.FwWsId;
				object value = null;

				switch (mapping.NameInDataSource)
				{
					case "CitationForm": value = GetMultiStringValue(lxEntry.CitationForm, wsId); break;
                    case "LexemeForm": value = GetMultiStringValue(lxEntry.LexemeForm, wsId); break;
                    case "MorphType": value = GetPossibilityValue(lxEntry.MorphType, false); break;
					case "Etymology": value = GetMultiStringValue(lxEntry.Etymology, wsId); break;
					case "LiteralMeaning": value = GetMultiStringValue(lxEntry.LiteralMeaning, wsId); break;
					case "Bibliography": value = GetMultiStringValue(lxEntry.Bibliography, wsId); break;
					case "Note": value = GetMultiStringValue(lxEntry.Note, wsId); break;
					case "Restrictions": value = GetMultiStringValue(lxEntry.Restrictions, wsId); break;
					case "SummaryDefinition": value = GetMultiStringValue(lxEntry.SummaryDefinition, wsId); break;
					case "ExcludeAsHeadword": value = lxEntry.ExcludeAsHeadword.ToString(); break;
					case "DateCreated": value = lxEntry.DateCreated.ToString(); break;
					case "DateModified": value = lxEntry.DateModified.ToString(); break;
					case "ImportResidue": value = lxEntry.ImportResidue; break;

					case "Gloss-Secondary":
					case "Gloss-Other":
					case "Gloss":
						value = lxEntry.Senses.Where(s => s.Gloss != null)
							.Select(s => GetMultiStringValue(s.Gloss, wsId));
						break;

					case "Definition":
						value = lxEntry.Senses.Where(s => s.Definition != null)
							.Select(s => GetMultiStringValue(s.Definition, wsId));
						break;

					case "AnthropologyNote":
						value = lxEntry.Senses.Where(s => s.AnthropologyNote != null)
							.Select(s => GetMultiStringValue(s.AnthropologyNote, wsId));
						break;

					case "Bibliography-Sense":
						value = lxEntry.Senses.Where(s => s.Bibliography != null)
							.Select(s => GetMultiStringValue(s.Bibliography, wsId));
						break;

					case "DiscourseNote":
						value = lxEntry.Senses.Where(s => s.DiscourseNote != null)
							.Select(s => GetMultiStringValue(s.DiscourseNote, wsId));
						break;

					case "EncyclopedicInfo":
						value = lxEntry.Senses.Where(s => s.EncyclopedicInfo != null)
							.Select(s => GetMultiStringValue(s.EncyclopedicInfo, wsId));
						break;

					case "GeneralNote":
						value = lxEntry.Senses.Where(s => s.GeneralNote != null)
							.Select(s => GetMultiStringValue(s.GeneralNote, wsId));
						break;

					case "GrammarNote":
						value = lxEntry.Senses.Where(s => s.GrammarNote != null)
							.Select(s => GetMultiStringValue(s.GrammarNote, wsId));
						break;

					case "PhonologyNote":
						value = lxEntry.Senses.Where(s => s.PhonologyNote != null)
							.Select(s => GetMultiStringValue(s.PhonologyNote, wsId));
						break;

					case "Restrictions-Sense":
						value = lxEntry.Senses.Where(s => s.Restrictions != null)
							.Select(s => GetMultiStringValue(s.Restrictions, wsId));
						break;

					case "SemanticsNote":
						value = lxEntry.Senses.Where(s => s.SemanticsNote != null)
							.Select(s => GetMultiStringValue(s.SemanticsNote, wsId));
						break;

					case "SociolinguisticsNote":
						value = lxEntry.Senses.Where(s => s.SociolinguisticsNote != null)
							.Select(s => GetMultiStringValue(s.SociolinguisticsNote, wsId));
						break;

					case "ReversalEntries":
						value = lxEntry.Senses.Where(s => s.ReversalEntries.Any())
							.Select(s => GetCommaDelimitedList(s.ReversalEntries.Select(r => r.GetString(wsId))));
						break;

					case "PartOfSpeech":
						value = lxEntry.Senses.Where(s => s.PartOfSpeech != null)
							.Select(s => GetPossibilityValue(s.PartOfSpeech, false));
						break;

					case "SenseType":
						value = lxEntry.Senses.Where(s => s.SenseType != null)
							.Select(s => GetPossibilityValue(s.SenseType, false));
						break;

					case "Status":
						value = lxEntry.Senses.Where(s => s.SenseType != null)
							.Select(s => GetPossibilityValue(s.SenseType, false));
						break;

					case "ScientificName":
						value = lxEntry.Senses.Where(s => s.ScientificName != null).Select(s => s.ScientificName);
						break;

					case "Source":
						value = lxEntry.Senses.Where(s => s.Source != null).Select(s => s.Source);
						break;

					case "ImportResidue-Sense":
						value = lxEntry.Senses.Where(s => s.ImportResidue != null).Select(s => s.ImportResidue);
						break;

					case "AnthroCodes":
					    value = lxEntry.Senses.Where(s => s.AnthroCodes != null && s.AnthroCodes.Any())
							.Select(s => GetCommaDelimitedPossibilityList(s.AnthroCodes, false));
					    break;

					case "DomainTypes":
						value = lxEntry.Senses.Where(s => s.DomainTypes != null && s.DomainTypes.Any())
							.Select(s => GetCommaDelimitedPossibilityList(s.DomainTypes, false));
						break;

					case "SemanticDomains":
						value = lxEntry.Senses.Where(s => s.SemanticDomains != null && s.SemanticDomains.Any())
							.Select(s => GetCommaDelimitedPossibilityList(s.SemanticDomains, false));
						break;

					case "Usages":
						value = lxEntry.Senses.Where(s => s.Usages != null && s.Usages.Any())
							.Select(s => GetCommaDelimitedPossibilityList(s.Usages, false));
						break;
					
					case "Variants":
						value = lxEntry.Variants.Select(v => v.VariantForm.GetString(wsId));
						break;

					case "VariantTypes":
						value = lxEntry.VariantOfInfo.Select(vi =>
							GetCommaDelimitedPossibilityList(vi.VariantType, false)); 
						break;
		
					case "VariantComments":
						value = lxEntry.VariantOfInfo.Where(vi => vi.VariantComment != null)
							.Select(vi => GetMultiStringValue(vi.VariantComment, wsId));
						break;
					
					case "ComplexForms":
						value = GetCommaDelimitedList(lxEntry.ComplexForms.Select(c => c.GetString(wsId)));
						break;
					
					case "Components":
						value = lxEntry.ComplexFormInfo.Select(ci => GetCommaDelimitedList(ci.Components));
						break;
					
					case "ComplexTypes":
						value = lxEntry.ComplexFormInfo.Select(ci =>
							GetCommaDelimitedPossibilityList(ci.ComplexFormType, false)); 
						break;
					
					case "ComplexFormComments":
						value = lxEntry.ComplexFormInfo.Where(c => c.ComplexFormComment != null)
							.Select(c => GetMultiStringValue(c.ComplexFormComment, wsId));
						break;
					
					case "Allomorphs":
						value = lxEntry.Allomorphs.Where(a => a != null).Select(a => a.GetString(wsId));
						break;
				}

				if (value is string)
					recCacheEntry.SetValue(mapping.PaFieldName, (string)value);
				else if (value != null)
				{
					var valueList = (IEnumerable<string>)value;
					int count = valueList.Count();

					if (count > 0)
						recCacheEntry.SetValue(mapping.PaFieldName, valueList.ElementAt(0));

					if (count > 1)
						recCacheEntry.SetCollection(mapping.PaFieldName, valueList.Skip(1));
				}
			}

			return recCacheEntry;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reads all the data for a lex. entry that get written to word cache entries (e.g.
		/// phonetic, audio file info., etc.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool ReadWordEntryFieldsFromLexEntry(IPaLexEntry lxEntry, RecordCacheEntry recCacheEntry)
		{
			if (m_fwDsInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.AllPronunciationFields)
				return CreateWordEntriesFromPronunciations(lxEntry, recCacheEntry);

			recCacheEntry.Guid = new Guid(lxEntry.Guid.ToString());

			var pro = (!lxEntry.Pronunciations.Any() ? null : lxEntry.Pronunciations.ElementAt(0));

			string eticValue = null;

			if (m_fwDsInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm)
			{
				if (lxEntry.LexemeForm != null)
					eticValue = lxEntry.LexemeForm.GetString(m_phoneticWsId);
			}
			else
			{
				if (pro != null && pro.Form != null)
					eticValue = pro.Form.GetString(m_phoneticWsId);
			}

			if (eticValue == null)
				return false;

			var wentry = new WordCacheEntry(recCacheEntry, m_phoneticFieldName);
			wentry.SetValue(m_phoneticFieldName, eticValue);

			if (pro != null)
				ReadSinglePronunciation(pro, wentry);

            if (wentry.GetField("AudioFile", false) == null)
                SearchForAudioWritingSystems(wentry, lxEntry, pro);

			recCacheEntry.WordEntries.Add(wentry);
			return true;
		}

        /// <summary>
        /// Audio files can be store in special audio writing systems in the LexememForm, the CitationForm or the
        /// Example field of the pronunciation.
        /// First field found will be used.
        /// </summary>
        private void SearchForAudioWritingSystems(WordCacheEntry wentry, IPaLexEntry lxEntry, IPaLexPronunciation pro)
        {
            if (m_audioWsId == null)
                return;

            string audioFile = null;
            if (lxEntry.LexemeForm != null)
                audioFile = lxEntry.LexemeForm.GetString(m_audioWsId);
            if (audioFile == null && lxEntry.CitationForm != null)
                audioFile = lxEntry.CitationForm.GetString(m_audioWsId);

            // TODO: Should look at examples also, but examples are not part of the data PA can get from FLEx.

            if (audioFile != null)
                wentry["AudioFile"] = audioFile;
            
        }

		/// ------------------------------------------------------------------------------------
		private bool CreateWordEntriesFromPronunciations(IPaLexEntry lxEntry, RecordCacheEntry recCacheEntry)
		{
			foreach (var pro in lxEntry.Pronunciations.Where(p => p.Form != null))
			{
				var eticValue = pro.Form.GetString(m_phoneticWsId);
				if (eticValue != null)
				{
					var wentry = new WordCacheEntry(recCacheEntry, m_phoneticFieldName);
					wentry.SetValue(m_phoneticFieldName, eticValue);
					wentry.Guid = new Guid(lxEntry.Guid.ToString());
					ReadSinglePronunciation(pro, wentry);
					recCacheEntry.WordEntries.Add(wentry);
				}
			}

			return (recCacheEntry.WordEntries.Count > 0);
		}

		/// ------------------------------------------------------------------------------------
		private void ReadSinglePronunciation(IPaLexPronunciation pro, WordCacheEntry wentry)
		{
			var mapping = m_dataSource.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "CV-Pattern-Source");
			if (mapping != null)
				wentry.SetValue(mapping.NameInDataSource, pro.CVPattern);

			mapping = m_dataSource.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "Tone");
			if (mapping != null)
				wentry.SetValue(mapping.NameInDataSource, pro.Tone);

			mapping = m_dataSource.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "Location");
			if (mapping != null)
			{
				wentry.SetValue(mapping.NameInDataSource,
					GetPossibilityValue(pro.Location, false));
			}

			if (!pro.MediaFiles.Any())
				return;

			// TODO: Verify that the media file is audio.
			var mediaFile = pro.MediaFiles.ElementAt(0);

			// TODO: Figure out a way to deal with more than one media file and label.
			wentry.SetValue("AudioFile", mediaFile.AbsoluteInternalPath);

			mapping = m_dataSource.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "AudioFileLabel");
			if (mapping != null)
				wentry.SetValue("AudioFileLabel", GetMultiStringValue(mediaFile.Label, mapping.FwWsId));
		}

		/// ------------------------------------------------------------------------------------
		private string GetCommaDelimitedPossibilityList(IEnumerable<IPaCmPossibility> cmPossibilities,
			bool returnAbbreviation)
		{
			var list = cmPossibilities.ToArray();

			return (list.Length == 0 ? null : GetCommaDelimitedList(list.Select(p =>
				GetPossibilityValue(p, returnAbbreviation))));
		}

		/// ------------------------------------------------------------------------------------
		private string GetCommaDelimitedList(IEnumerable<string> list)
		{
			var bldr = new StringBuilder();
			foreach (var text in list)
				bldr.AppendFormat("{0}, ", text);

			return (bldr.Length == 0 ? null : bldr.ToString().Trim(',', ' '));
		}

		/// ------------------------------------------------------------------------------------
		private string GetMultiStringValue(IPaMultiString multiStr, string wsId)
		{
			return (multiStr == null ? null : multiStr.GetString(wsId));
		}

		/// ------------------------------------------------------------------------------------
		private string GetPossibilityValue(IPaCmPossibility poss, bool returnAbbreviation)
		{
			if (poss == null)
				return null;

			var value = (returnAbbreviation ?
				poss.Abbreviation.GetString(App.GetUILanguageId()) :
				poss.Name.GetString(App.GetUILanguageId()));

			if (value == null)
			{
				foreach (var wsId in m_fwDsInfo.GetWritingSystems()
					.Where(ws => ws.Type == FwDBUtils.FwWritingSystemType.Analysis)
					.Select(ws => ws.Id))
				{
					value = (returnAbbreviation ?
						poss.Abbreviation.GetString(wsId) : poss.Name.GetString(wsId));

					if (value != null)
						return value;
				}
			}

			// If the value is still null, make a last ditch effort to get an English string.
			return (value ?? (returnAbbreviation ?
				   poss.Abbreviation.GetString("en") : poss.Name.GetString("en")));
		}
	}
}
