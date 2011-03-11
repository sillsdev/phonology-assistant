using System;
using System.Drawing;
using System.Xml.Serialization;
using SilTools;

namespace SIL.Pa.Model
{
	#region PaFieldDisplayProperties class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is sort of paired with the PaField class. It holds only the properties of
	/// a field related to its display. It is cached in a FieldDisplayPropsCache and each
	/// instance should be a singleton. Therefore, no matter how many copies there are
	/// floating around of a PaField whose names are identical, the display properties for all
	/// those instances will remain constant since they come from a single
	/// PaFieldDisplayProperties object stored in the FieldDisplayPropsCache.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaFieldDisplayProperties : IDisposable
	{
		public const int kDefaultWidthInGrid = 120;
		
		private Font m_font;

		/// ------------------------------------------------------------------------------------
		public PaFieldDisplayProperties()
		{
			VisibleInGrid = false;
			VisibleInRecView = false;
			DisplayIndexInGrid = 999;
			DisplayIndexInRecView = 999;
			WidthInGrid = kDefaultWidthInGrid;
		}

		/// ------------------------------------------------------------------------------------
		public PaFieldDisplayProperties(string name) : this()
		{
			Name = name;

			if (name == PaField.kPhoneticFieldName || name == PaField.kCVPatternFieldName)
				Font = App.PhoneticFont;
		}

		/// ------------------------------------------------------------------------------------
		public PaFieldDisplayProperties(string name, bool visibleInGrid, bool visibleInRecView)
			: this(name)
		{
			VisibleInGrid = visibleInGrid;
			VisibleInRecView = visibleInRecView;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			Font = null;
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font Font
		{
			get { return (m_font ?? FontHelper.UIFont); }
			set
			{
				if (m_font != null)
					m_font.Dispose();

				m_font = (value == null ? null : (Font)value.Clone());
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("font")]
		public SerializableFont SFont
		{
			get { return (m_font == null ? null : new SerializableFont(m_font)); }
			set { if (value != null) m_font = (Font)value.Font.Clone(); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("rightToLeft")]
		public bool RightToLeft { get; set; }

		#region Properties for field's visibility in grids and rec. views
		/// ------------------------------------------------------------------------------------
		[XmlElement("visibleInGrid")]
		public bool VisibleInGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("visibleInRecView")]
		public bool VisibleInRecView { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("displayIndexInGrid")]
		public int DisplayIndexInGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("displayIndexInRecView")]
		public int DisplayIndexInRecView { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("widthInGrid")]
		public int WidthInGrid { get; set; }

		#endregion

		#region static methods
		/// ------------------------------------------------------------------------------------
		public static string GetDisplayName(string name)
		{
			switch (name)
			{
				case PaField.kCVPatternFieldName: return App.LocalizeString("DisplayableFieldNames.CVPattern",
										"CV Pattern", App.kLocalizationGroupMisc);

				case PaField.kDataSourceFieldName: return App.LocalizeString("DisplayableFieldNames.DataSource",
										"Data Source", App.kLocalizationGroupMisc);

				case PaField.kDataSourcePathFieldName: return App.LocalizeString("DisplayableFieldNames.DataSourcePath",
										"Data Source Path", App.kLocalizationGroupMisc);

				case "Reference": return App.LocalizeString("DisplayableFieldNames.Reference",
										"Reference", App.kLocalizationGroupMisc);

				case "Phonetic": return App.LocalizeString("DisplayableFieldNames.Phonetic",
										"Phonetic", App.kLocalizationGroupMisc);

				case "Gloss": return App.LocalizeString("DisplayableFieldNames.Gloss",
										"Gloss", App.kLocalizationGroupMisc);

				case "Gloss-Secondary": return App.LocalizeString("DisplayableFieldNames.GlossSecondary",
										"Gloss (Secondary)", App.kLocalizationGroupMisc);

				case "Gloss-Other": return App.LocalizeString("DisplayableFieldNames.GlossOther",
										"Gloss (Other)", App.kLocalizationGroupMisc);

				case "PartOfSpeech": return App.LocalizeString("DisplayableFieldNames.PartOfSpeech",
										"Part of Speech", App.kLocalizationGroupMisc);

				case "Tone": return App.LocalizeString("DisplayableFieldNames.Tone",
										"Tone", App.kLocalizationGroupMisc);

				case "Orthographic": return App.LocalizeString("DisplayableFieldNames.Orthographic",
										"Orthographic", App.kLocalizationGroupMisc);

				case "Phonemic": return App.LocalizeString("DisplayableFieldNames.Phonemic",
										"Phonemic", App.kLocalizationGroupMisc);

				case "AudioFile": return App.LocalizeString("DisplayableFieldNames.AudioFile",
										"Audio File", App.kLocalizationGroupMisc);

				case "AudioFileLabel": return App.LocalizeString("DisplayableFieldNames.AudioFileLabel",
										"Audio File Label", App.kLocalizationGroupMisc);

				case "FreeFormTranslation": return App.LocalizeString("DisplayableFieldNames.FreeFormTranslation",
										"Free Translation", App.kLocalizationGroupMisc);

				case "NoteBookReference": return App.LocalizeString("DisplayableFieldNames.NoteBookReference",
										"Note Book Ref.", App.kLocalizationGroupMisc);

				case "Dialect": return App.LocalizeString("DisplayableFieldNames.Dialect",
										"Dialect", App.kLocalizationGroupMisc);

				case "EthnologueId": return App.LocalizeString("DisplayableFieldNames.EthnologueId",
										"Ethnologue Id", App.kLocalizationGroupMisc);

				case "LanguageName": return App.LocalizeString("DisplayableFieldNames.LanguageName",
										"Language Name", App.kLocalizationGroupMisc);

				case "Region": return App.LocalizeString("DisplayableFieldNames.Region",
										"Region", App.kLocalizationGroupMisc);

				case "Country": return App.LocalizeString("DisplayableFieldNames.Country",
										"Country", App.kLocalizationGroupMisc);

				case "Family": return App.LocalizeString("DisplayableFieldNames.Family",
										"Family", App.kLocalizationGroupMisc);

				case "Transcriber": return App.LocalizeString("DisplayableFieldNames.Transcriber",
										"Transcriber", App.kLocalizationGroupMisc);

				case "SpeakerName": return App.LocalizeString("DisplayableFieldNames.SpeakerName",
										"Speaker Name", App.kLocalizationGroupMisc);

				case "SpeakerGender": return App.LocalizeString("DisplayableFieldNames.SpeakerGender",
										"Speaker Gender", App.kLocalizationGroupMisc);

				case "SADescription": return App.LocalizeString("DisplayableFieldNames.SADescription",
										"Description", App.kLocalizationGroupMisc);

				case "CitationForm": return App.LocalizeString("DisplayableFieldNames.CitationForm",
										"Citation Form", App.kLocalizationGroupMisc);

				case "MorphType": return App.LocalizeString("DisplayableFieldNames.MorphType",
										"Morpheme Type", App.kLocalizationGroupMisc);

				case "Etymology": return App.LocalizeString("DisplayableFieldNames.Etymology",
										"Etymology", App.kLocalizationGroupMisc);

				case "LiteralMeaning": return App.LocalizeString("DisplayableFieldNames.LiteralMeaning",
										"Literal Meaning", App.kLocalizationGroupMisc);

				case "Bibliography": return App.LocalizeString("DisplayableFieldNames.Bibliography",
										"Bibliography", App.kLocalizationGroupMisc);

				case "Restrictions": return App.LocalizeString("DisplayableFieldNames.Restrictions",
										"Restrictions", App.kLocalizationGroupMisc);

				case "SummaryDefinition": return App.LocalizeString("DisplayableFieldNames.SummaryDefinition",
										"Summary Definition", App.kLocalizationGroupMisc);

				case "Note": return App.LocalizeString("DisplayableFieldNames.Note",
										"Note", App.kLocalizationGroupMisc);

				case "CV-Pattern-Flex": return App.LocalizeString("DisplayableFieldNames.FlexCVPattern",
										"CV Pattern (FLEx)", App.kLocalizationGroupMisc);

				case "Location": return App.LocalizeString("DisplayableFieldNames.Location",
										"Location", App.kLocalizationGroupMisc);

				case "ExcludeAsHeadword": return App.LocalizeString("DisplayableFieldNames.ExcludeAsHeadword",
										"Exclude As Headword", App.kLocalizationGroupMisc);

				case "ImportResidue": return App.LocalizeString("DisplayableFieldNames.ImportResidue",
										"Import Residue", App.kLocalizationGroupMisc);

				case "DateCreated": return App.LocalizeString("DisplayableFieldNames.DateCreated",
										"Date Created", App.kLocalizationGroupMisc);

				case "DateModified": return App.LocalizeString("DisplayableFieldNames.DateModified",
										"Date Modified", App.kLocalizationGroupMisc);

				case "Definition": return App.LocalizeString("DisplayableFieldNames.Definition",
										"Definition", App.kLocalizationGroupMisc);

				case "ScientificName": return App.LocalizeString("DisplayableFieldNames.ScientificName",
										"Scientific Name", App.kLocalizationGroupMisc);

				case "AnthropologyNote": return App.LocalizeString("DisplayableFieldNames.AnthropologyNote",
										"Anthropology Note", App.kLocalizationGroupMisc);

				case "Bibliography-Sense": return App.LocalizeString("DisplayableFieldNames.Bibliography-Sense",
										"Bibliography (Sense)", App.kLocalizationGroupMisc);

				case "DiscourseNote": return App.LocalizeString("DisplayableFieldNames.DiscourseNote",
										"Discourse Note", App.kLocalizationGroupMisc);

				case "EncyclopedicInfo": return App.LocalizeString("DisplayableFieldNames.EncyclopedicInfo",
										"Encyclopedic Info.", App.kLocalizationGroupMisc);

				case "GeneralNote": return App.LocalizeString("DisplayableFieldNames.GeneralNote",
										"General Note", App.kLocalizationGroupMisc);

				case "GrammarNote": return App.LocalizeString("DisplayableFieldNames.Grammar Note",
										"Grammar Note", App.kLocalizationGroupMisc);

				case "PhonologyNote": return App.LocalizeString("DisplayableFieldNames.PhonologyNote",
										"Phonology Note", App.kLocalizationGroupMisc);

				case "Restrictions-Sense": return App.LocalizeString("DisplayableFieldNames.Restrictions-Sense",
										"Restrictions (Sense)", App.kLocalizationGroupMisc);

				case "SemanticsNote": return App.LocalizeString("DisplayableFieldNames.SemanticsNote",
										"Semantics Note", App.kLocalizationGroupMisc);

				case "SociolinguisticsNote": return App.LocalizeString("DisplayableFieldNames.SociolinguisticsNote",
										"Sociolinguistics Note", App.kLocalizationGroupMisc);

				case "ReversalEntries": return App.LocalizeString("DisplayableFieldNames.ReversalEntries",
									   "Reversal Entries", App.kLocalizationGroupMisc);

				case "Source": return App.LocalizeString("DisplayableFieldNames.Source",
										"Source", App.kLocalizationGroupMisc);

				case "SenseType": return App.LocalizeString("DisplayableFieldNames.SenseType",
										"Sense Type", App.kLocalizationGroupMisc);

				case "Status": return App.LocalizeString("DisplayableFieldNames.Status",
										"Status", App.kLocalizationGroupMisc);

				case "ImportResidue-Sense": return App.LocalizeString("DisplayableFieldNames.ImportResidue-Sense",
										"Import Residue (Sense)", App.kLocalizationGroupMisc);

				case "AnthroCodes": return App.LocalizeString("DisplayableFieldNames.AnthroCategories",
										"Anthropology Categories", App.kLocalizationGroupMisc);

				case "DomainTypes": return App.LocalizeString("DisplayableFieldNames.DomainTypes",
										"Domain Types", App.kLocalizationGroupMisc);

				case "SemanticDomains": return App.LocalizeString("DisplayableFieldNames.SemanticDomains",
										"Semantic Domains", App.kLocalizationGroupMisc);

				case "Usages": return App.LocalizeString("DisplayableFieldNames.Usages",
										"Usages", App.kLocalizationGroupMisc);

				case "Variants": return App.LocalizeString("DisplayableFieldNames.Variants",
										"Variants", App.kLocalizationGroupMisc);

				case "VariantTypes": return App.LocalizeString("DisplayableFieldNames.VariantTypes",
										"Variant Types", App.kLocalizationGroupMisc);

				case "VariantComments": return App.LocalizeString("DisplayableFieldNames.VariantComments",
										"Variant Comments", App.kLocalizationGroupMisc);

				case "ComplexForms": return App.LocalizeString("DisplayableFieldNames.ComplexForms",
										"Complex Forms", App.kLocalizationGroupMisc);

				case "Components": return App.LocalizeString("DisplayableFieldNames.Components",
										"Components", App.kLocalizationGroupMisc);

				case "ComplexTypes": return App.LocalizeString("DisplayableFieldNames.ComplexTypes",
										"Complex Types", App.kLocalizationGroupMisc);

				case "ComplexFormComments": return App.LocalizeString("DisplayableFieldNames.ComplexFormComments",
										"Complex Form Comments", App.kLocalizationGroupMisc);

				case "Allomorphs": return App.LocalizeString("DisplayableFieldNames.Allomorphs",
										"Allomorphs", App.kLocalizationGroupMisc);
			}

			return name;
		}

		#endregion
	}

	#endregion
}
