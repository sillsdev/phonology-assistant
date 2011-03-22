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
		/// <summary>
		/// Gets the project's fields file path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetFileForProject(string projectPathPrefix)
		{
			return projectPathPrefix + "FieldDisplayProperties.xml";
		}

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
				case PaField.kCVPatternFieldName: return App.LocalizeString("DisplayableFieldNames.CVPattern", "CV Pattern");
				case PaField.kDataSourceFieldName: return App.LocalizeString("DisplayableFieldNames.DataSource", "Data Source");
				case PaField.kDataSourcePathFieldName: return App.LocalizeString("DisplayableFieldNames.DataSourcePath", "Data Source Path");
				case "Reference": return App.LocalizeString("DisplayableFieldNames.Reference", "Reference");
				case "Phonetic": return App.LocalizeString("DisplayableFieldNames.Phonetic", "Phonetic");
				case "Gloss": return App.LocalizeString("DisplayableFieldNames.Gloss", "Gloss");
				case "Gloss-Secondary": return App.LocalizeString("DisplayableFieldNames.GlossSecondary", "Gloss (Secondary)");
				case "Gloss-Other": return App.LocalizeString("DisplayableFieldNames.GlossOther", "Gloss (Other)");
				case "PartOfSpeech": return App.LocalizeString("DisplayableFieldNames.PartOfSpeech", "Part of Speech");
				case "Tone": return App.LocalizeString("DisplayableFieldNames.Tone", "Tone");
				case "Orthographic": return App.LocalizeString("DisplayableFieldNames.Orthographic", "Orthographic");
				case "Phonemic": return App.LocalizeString("DisplayableFieldNames.Phonemic", "Phonemic");
				case "AudioFile": return App.LocalizeString("DisplayableFieldNames.AudioFile", "Audio File");
				case "AudioFileLabel": return App.LocalizeString("DisplayableFieldNames.AudioFileLabel", "Audio File Label");
				case "FreeFormTranslation": return App.LocalizeString("DisplayableFieldNames.FreeFormTranslation", "Free Translation");
				case "NoteBookReference": return App.LocalizeString("DisplayableFieldNames.NoteBookReference", "Note Book Ref.");
				case "Dialect": return App.LocalizeString("DisplayableFieldNames.Dialect", "Dialect");
				case "EthnologueId": return App.LocalizeString("DisplayableFieldNames.EthnologueId", "Ethnologue Id");
				case "LanguageName": return App.LocalizeString("DisplayableFieldNames.LanguageName", "Language Name");
				case "Region": return App.LocalizeString("DisplayableFieldNames.Region", "Region");
				case "Country": return App.LocalizeString("DisplayableFieldNames.Country", "Country");
				case "Family": return App.LocalizeString("DisplayableFieldNames.Family", "Family");
				case "Transcriber": return App.LocalizeString("DisplayableFieldNames.Transcriber", "Transcriber");
				case "SpeakerName": return App.LocalizeString("DisplayableFieldNames.SpeakerName", "Speaker Name");
				case "SpeakerGender": return App.LocalizeString("DisplayableFieldNames.SpeakerGender", "Speaker Gender");
				case "SADescription": return App.LocalizeString("DisplayableFieldNames.SADescription", "Description");
				case "CitationForm": return App.LocalizeString("DisplayableFieldNames.CitationForm", "Citation Form");
				case "MorphType": return App.LocalizeString("DisplayableFieldNames.MorphType", "Morpheme Type");
				case "Etymology": return App.LocalizeString("DisplayableFieldNames.Etymology", "Etymology");
				case "LiteralMeaning": return App.LocalizeString("DisplayableFieldNames.LiteralMeaning", "Literal Meaning");
				case "Bibliography": return App.LocalizeString("DisplayableFieldNames.Bibliography", "Bibliography");
				case "Restrictions": return App.LocalizeString("DisplayableFieldNames.Restrictions", "Restrictions");
				case "SummaryDefinition": return App.LocalizeString("DisplayableFieldNames.SummaryDefinition", "Summary Definition");
				case "Note": return App.LocalizeString("DisplayableFieldNames.Note", "Note");
				case "CV-Pattern-Source": return App.LocalizeString("DisplayableFieldNames.DataSourceCVPattern", "CV Pattern (from source)");
				case "Location": return App.LocalizeString("DisplayableFieldNames.Location", "Location");
				case "ExcludeAsHeadword": return App.LocalizeString("DisplayableFieldNames.ExcludeAsHeadword", "Exclude As Headword");
				case "ImportResidue": return App.LocalizeString("DisplayableFieldNames.ImportResidue", "Import Residue");
				case "DateCreated": return App.LocalizeString("DisplayableFieldNames.DateCreated", "Date Created");
				case "DateModified": return App.LocalizeString("DisplayableFieldNames.DateModified", "Date Modified");
				case "Definition": return App.LocalizeString("DisplayableFieldNames.Definition", "Definition");
				case "ScientificName": return App.LocalizeString("DisplayableFieldNames.ScientificName", "Scientific Name");
				case "AnthropologyNote": return App.LocalizeString("DisplayableFieldNames.AnthropologyNote", "Anthropology Note");
				case "Bibliography-Sense": return App.LocalizeString("DisplayableFieldNames.Bibliography-Sense", "Bibliography (Sense)");
				case "DiscourseNote": return App.LocalizeString("DisplayableFieldNames.DiscourseNote", "Discourse Note");
				case "EncyclopedicInfo": return App.LocalizeString("DisplayableFieldNames.EncyclopedicInfo", "Encyclopedic Info.");
				case "GeneralNote": return App.LocalizeString("DisplayableFieldNames.GeneralNote", "General Note");
				case "GrammarNote": return App.LocalizeString("DisplayableFieldNames.Grammar Note", "Grammar Note");
				case "PhonologyNote": return App.LocalizeString("DisplayableFieldNames.PhonologyNote", "Phonology Note");
				case "Restrictions-Sense": return App.LocalizeString("DisplayableFieldNames.Restrictions-Sense", "Restrictions (Sense)");
				case "SemanticsNote": return App.LocalizeString("DisplayableFieldNames.SemanticsNote", "Semantics Note");
				case "SociolinguisticsNote": return App.LocalizeString("DisplayableFieldNames.SociolinguisticsNote", "Sociolinguistics Note");
				case "ReversalEntries": return App.LocalizeString("DisplayableFieldNames.ReversalEntries", "Reversal Entries");
				case "Source": return App.LocalizeString("DisplayableFieldNames.Source", "Source");
				case "SenseType": return App.LocalizeString("DisplayableFieldNames.SenseType", "Sense Type");
				case "Status": return App.LocalizeString("DisplayableFieldNames.Status", "Status");
				case "ImportResidue-Sense": return App.LocalizeString("DisplayableFieldNames.ImportResidue-Sense", "Import Residue (Sense)");
				case "AnthroCodes": return App.LocalizeString("DisplayableFieldNames.AnthroCategories", "Anthropology Categories");
				case "DomainTypes": return App.LocalizeString("DisplayableFieldNames.DomainTypes", "Domain Types");
				case "SemanticDomains": return App.LocalizeString("DisplayableFieldNames.SemanticDomains", "Semantic Domains");
				case "Usages": return App.LocalizeString("DisplayableFieldNames.Usages", "Usages");
				case "Variants": return App.LocalizeString("DisplayableFieldNames.Variants", "Variants");
				case "VariantTypes": return App.LocalizeString("DisplayableFieldNames.VariantTypes", "Variant Types");
				case "VariantComments": return App.LocalizeString("DisplayableFieldNames.VariantComments", "Variant Comments");
				case "ComplexForms": return App.LocalizeString("DisplayableFieldNames.ComplexForms", "Complex Forms");
				case "Components": return App.LocalizeString("DisplayableFieldNames.Components", "Components");
				case "ComplexTypes": return App.LocalizeString("DisplayableFieldNames.ComplexTypes", "Complex Types");
				case "ComplexFormComments": return App.LocalizeString("DisplayableFieldNames.ComplexFormComments", "Complex Form Comments");
				case "Allomorphs": return App.LocalizeString("DisplayableFieldNames.Allomorphs", "Allomorphs");
			}

			return name;
		}

		#endregion
	}

	#endregion
}
