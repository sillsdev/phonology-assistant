using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.PaToFdoInterfaces;

namespace SIL.Pa.DataSource.FieldWorks
{
	public class Fw7DataSourceReader
	{
		/// ------------------------------------------------------------------------------------
		public void Read(PaProject project, RecordCache recCache, PaDataSource ds)
		{
			recCache.AddRange(FwDBUtils.GetLexEntriesFromFw7Project(ds.FwDataSourceInfo)
				.Select(entry => ReadSingleLexEntry(project, ds, entry)));

			ds.UpdateLastModifiedTime();
		}

		/// ------------------------------------------------------------------------------------
		private RecordCacheEntry ReadSingleLexEntry(PaProject project, PaDataSource ds, IPaLexEntry lxEntry)
		{
			// Make a new record entry.
			var recCacheEntry = new RecordCacheEntry(false, project);
			recCacheEntry.DataSource = ds;
			recCacheEntry.NeedsParsing = false;
			recCacheEntry.WordEntries = new List<WordCacheEntry>();

			ReadWordEntryFieldsFromLexEntry(ds, lxEntry, recCacheEntry);

			foreach (var mapping in ds.FieldMappings)
			{
				var wsId = mapping.FwWsId;
				object value = null;

				switch (mapping.NameInDataSource)
				{
					case "CitationForm": value = GetMultiStringValue(lxEntry.CitationForm, wsId); break;
					case "MorphType": value = GetPossibilityValue(lxEntry.MorphType, ds.FwDataSourceInfo, false); break;
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

					case "PartOfSpeech":
						value = lxEntry.Senses.Where(s => s.PartOfSpeech != null)
							.Select(s => GetPossibilityValue(s.PartOfSpeech, ds.FwDataSourceInfo, false));
						break;

					case "SenseType":
						value = lxEntry.Senses.Where(s => s.SenseType != null)
							.Select(s => GetPossibilityValue(s.SenseType, ds.FwDataSourceInfo, false));
						break;

					case "Status":
						value = lxEntry.Senses.Where(s => s.SenseType != null)
							.Select(s => GetPossibilityValue(s.SenseType, ds.FwDataSourceInfo, false));
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

					//case "AnthroCodes":
					//    value = lxEntry.Senses.Where(s => s.AnthroCodes != null).SelectMany(s => s.AnthroCodes), ds.FwDataSourceInfo);
					//    break;

					//case "DomainTypes":
					//    value = GetPossibilityValuesFromCollection(lxEntry.Senses
					//        .Where(s => s.DomainTypes != null).SelectMany(s => s.DomainTypes), ds.FwDataSourceInfo);
					//    break;

					//case "SemanticDomains":
					//    value = GetPossibilityValuesFromCollection(lxEntry.Senses
					//        .Where(s => s.SemanticDomains != null).SelectMany(s => s.SemanticDomains), ds.FwDataSourceInfo);
					//    break;

					//case "Usages":
					//    value = GetPossibilityValuesFromCollection(lxEntry.Senses
					//        .Where(s => s.Usages != null).SelectMany(s => s.Usages), ds.FwDataSourceInfo);
					//    break;
					
					case "Variants":
						value = lxEntry.Variants.Select(v => v.VariantForm.GetString(wsId));
						break;

					case "VariantTypes":
						value = lxEntry.VariantOfInfo.Select(vi =>
							GetCommaDelimitedPossibilityList(vi.VariantType, ds.FwDataSourceInfo, false)); 
						break;
		
					case "VariantComments":
						value = lxEntry.VariantOfInfo.Select(vi => vi.VariantComment.GetString(wsId));
						break;
					
					case "ComplexForms":
						value = GetCommaDelimitedList(lxEntry.ComplexForms.Select(c => c.GetString(wsId)));
						break;
					
					case "Components":
						value = lxEntry.ComplexFormInfo.Select(ci => GetCommaDelimitedList(ci.Components));
						break;
					
					case "ComplexTypes":
						value = lxEntry.ComplexFormInfo.Select(ci =>
							GetCommaDelimitedPossibilityList(ci.ComplexFormType, ds.FwDataSourceInfo, false)); 
						break;
					
					case "ComplexFormComments":
						value = lxEntry.ComplexFormInfo.Select(c => c.ComplexFormComment.GetString(wsId));
						break;
					
					case "Allomorphs":
						value = lxEntry.Allomorphs.Select(a => a.GetString(wsId));
						break;
				}

				if (value is string)
					recCacheEntry.SetValue(mapping.PaFieldName, (string)value);
				else if (value != null)
				{
					// TODO: Handle all values after the first.

					var valueList = (IEnumerable<string>)value;
					if (valueList.Count() > 0)
						recCacheEntry.SetValue(mapping.PaFieldName, valueList.ElementAt(0));
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
		private void ReadWordEntryFieldsFromLexEntry(PaDataSource ds, IPaLexEntry lxEntry, RecordCacheEntry recCacheEntry)
		{
			WordCacheEntry wentry;
			var parsedFields = Settings.Default.ParsedFw7Fields.Cast<string>();
			var phoneticMapping = ds.FieldMappings.Single(m => m.Field.Type == FieldType.Phonetic);

			if (ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.AllPronunciationFields)
			{
				foreach (var pro in lxEntry.Pronunciations)
				{
					wentry = new WordCacheEntry(recCacheEntry, parsedFields, "Phonetic");
					ReadSinglePronunciation(ds, phoneticMapping, pro, wentry);
					recCacheEntry.WordEntries.Add(wentry);
				}
			}
			else
			{
				wentry = new WordCacheEntry(recCacheEntry, parsedFields, "Phonetic");

				if (ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm)
				{
					wentry.SetValue(phoneticMapping.NameInDataSource,
						lxEntry.LexemeForm.GetString(phoneticMapping.FwWsId));

					phoneticMapping = null;
				}

				if (lxEntry.Pronunciations.Count() > 0)
				{
					ReadSinglePronunciation(ds, phoneticMapping,
						lxEntry.Pronunciations.ElementAt(0), wentry);
				}

				recCacheEntry.WordEntries.Add(wentry);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ReadSinglePronunciation(PaDataSource ds, FieldMapping phoneticMapping,
			IPaLexPronunciation pro, WordCacheEntry wentry)
		{
			if (phoneticMapping != null)
				wentry.SetValue(phoneticMapping.NameInDataSource, pro.Form.GetString(phoneticMapping.FwWsId));

			var mapping = ds.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "CV-Pattern-Flex");
			if (mapping != null)
				wentry.SetValue(mapping.NameInDataSource, pro.CVPattern);

			mapping = ds.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "Tone");
			if (mapping != null)
				wentry.SetValue(mapping.NameInDataSource, pro.Tone);

			mapping = ds.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "Location");
			if (mapping != null)
			{
				wentry.SetValue(mapping.NameInDataSource,
					GetPossibilityValue(pro.Location, ds.FwDataSourceInfo, false));
			}

			if (pro.MediaFiles.Count() == 0)
				return;

			// TODO: Verify that the media file is audio.
			var mediaFile = pro.MediaFiles.ElementAt(0);

			// TODO: Figure out a way to deal with more than one media file and label.
			wentry.SetValue("AudioFile", mediaFile.AbsoluteInternalPath);

			mapping = ds.FieldMappings.SingleOrDefault(m => m.NameInDataSource == "AudioFileLabel");
			if (mapping != null)
				wentry.SetValue("AudioFileLabel", GetMultiStringValue(mediaFile.Label, mapping.FwWsId));
		}

		/// ------------------------------------------------------------------------------------
		private string GetCommaDelimitedPossibilityList(IEnumerable<IPaCmPossibility> list,
			FwDataSourceInfo dsInfo, bool returnAbbreviation)
		{
			return (list.Count() == 0 ? null : GetCommaDelimitedList(list.Select(p =>
				GetPossibilityValue(p, dsInfo, returnAbbreviation))));
		}

		/// ------------------------------------------------------------------------------------
		private string GetCommaDelimitedList(IEnumerable<string> list)
		{
			var bldr = new StringBuilder();
			foreach (var text in list)
				bldr.AppendFormat("{0}, ", text);

			return (bldr.Length == 0 ? null : bldr.ToString().TrimEnd(',', ' '));
		}

		/// ------------------------------------------------------------------------------------
		private string GetMultiStringValue(IPaMultiString multiStr, string wsId)
		{
			return (multiStr == null ? null : multiStr.GetString(wsId));
		}

		/// ------------------------------------------------------------------------------------
		private string GetPossibilityValue(IPaCmPossibility poss, FwDataSourceInfo dsInfo,
			bool returnAbbreviation)
		{
			if (poss == null)
				return null;

			var value = (returnAbbreviation ?
				poss.Abbreviation.GetString(App.GetUILanguageId()) :
				poss.Name.GetString(App.GetUILanguageId()));

			if (value == null)
			{
				foreach (var wsId in dsInfo.GetWritingSystems()
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
