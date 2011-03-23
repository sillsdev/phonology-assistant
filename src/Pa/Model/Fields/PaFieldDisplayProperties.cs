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
				case PaField.kCVPatternFieldName: return App.GetString("DisplayableFieldNames.CVPattern", "CV Pattern");
				case PaField.kDataSourceFieldName: return App.GetString("DisplayableFieldNames.DataSource", "Data Source");
				case PaField.kDataSourcePathFieldName: return App.GetString("DisplayableFieldNames.DataSourcePath", "Data Source Path");
				case "Reference": return App.GetString("DisplayableFieldNames.Reference", "Reference");
				case "Phonetic": return App.GetString("DisplayableFieldNames.Phonetic", "Phonetic");
				case "Gloss": return App.GetString("DisplayableFieldNames.Gloss", "Gloss");
				case "Gloss-Secondary": return App.GetString("DisplayableFieldNames.GlossSecondary", "Gloss (Secondary)");
				case "Gloss-Other": return App.GetString("DisplayableFieldNames.GlossOther", "Gloss (Other)");
				case "PartOfSpeech": return App.GetString("DisplayableFieldNames.PartOfSpeech", "Part of Speech");
				case "Tone": return App.GetString("DisplayableFieldNames.Tone", "Tone");
				case "Orthographic": return App.GetString("DisplayableFieldNames.Orthographic", "Orthographic");
				case "Phonemic": return App.GetString("DisplayableFieldNames.Phonemic", "Phonemic");
				case "AudioFile": return App.GetString("DisplayableFieldNames.AudioFile", "Audio File");
				case "AudioFileLabel": return App.GetString("DisplayableFieldNames.AudioFileLabel", "Audio File Label");
				case "FreeFormTranslation": return App.GetString("DisplayableFieldNames.FreeFormTranslation", "Free Translation");
				case "NoteBookReference": return App.GetString("DisplayableFieldNames.NoteBookReference", "Note Book Ref.");
				case "Dialect": return App.GetString("DisplayableFieldNames.Dialect", "Dialect");
				case "EthnologueId": return App.GetString("DisplayableFieldNames.EthnologueId", "Ethnologue Id");
				case "LanguageName": return App.GetString("DisplayableFieldNames.LanguageName", "Language Name");
				case "Region": return App.GetString("DisplayableFieldNames.Region", "Region");
				case "Country": return App.GetString("DisplayableFieldNames.Country", "Country");
				case "Family": return App.GetString("DisplayableFieldNames.Family", "Family");
				case "Transcriber": return App.GetString("DisplayableFieldNames.Transcriber", "Transcriber");
				case "SpeakerName": return App.GetString("DisplayableFieldNames.SpeakerName", "Speaker Name");
				case "SpeakerGender": return App.GetString("DisplayableFieldNames.SpeakerGender", "Speaker Gender");
				case "SADescription": return App.GetString("DisplayableFieldNames.SADescription", "Description");
				case "CitationForm": return App.GetString("DisplayableFieldNames.CitationForm", "Citation Form");
				case "MorphType": return App.GetString("DisplayableFieldNames.MorphType", "Morpheme Type");
				case "Etymology": return App.GetString("DisplayableFieldNames.Etymology", "Etymology");
				case "LiteralMeaning": return App.GetString("DisplayableFieldNames.LiteralMeaning", "Literal Meaning");
				case "Bibliography": return App.GetString("DisplayableFieldNames.Bibliography", "Bibliography");
				case "Restrictions": return App.GetString("DisplayableFieldNames.Restrictions", "Restrictions");
				case "SummaryDefinition": return App.GetString("DisplayableFieldNames.SummaryDefinition", "Summary Definition");
				case "Note": return App.GetString("DisplayableFieldNames.Note", "Note");
				case "CV-Pattern-Source": return App.GetString("DisplayableFieldNames.DataSourceCVPattern", "CV Pattern (from source)");
				case "Location": return App.GetString("DisplayableFieldNames.Location", "Location");
				case "ExcludeAsHeadword": return App.GetString("DisplayableFieldNames.ExcludeAsHeadword", "Exclude As Headword");
				case "ImportResidue": return App.GetString("DisplayableFieldNames.ImportResidue", "Import Residue");
				case "DateCreated": return App.GetString("DisplayableFieldNames.DateCreated", "Date Created");
				case "DateModified": return App.GetString("DisplayableFieldNames.DateModified", "Date Modified");
				case "Definition": return App.GetString("DisplayableFieldNames.Definition", "Definition");
				case "ScientificName": return App.GetString("DisplayableFieldNames.ScientificName", "Scientific Name");
				case "AnthropologyNote": return App.GetString("DisplayableFieldNames.AnthropologyNote", "Anthropology Note");
				case "Bibliography-Sense": return App.GetString("DisplayableFieldNames.Bibliography-Sense", "Bibliography (Sense)");
				case "DiscourseNote": return App.GetString("DisplayableFieldNames.DiscourseNote", "Discourse Note");
				case "EncyclopedicInfo": return App.GetString("DisplayableFieldNames.EncyclopedicInfo", "Encyclopedic Info.");
				case "GeneralNote": return App.GetString("DisplayableFieldNames.GeneralNote", "General Note");
				case "GrammarNote": return App.GetString("DisplayableFieldNames.Grammar Note", "Grammar Note");
				case "PhonologyNote": return App.GetString("DisplayableFieldNames.PhonologyNote", "Phonology Note");
				case "Restrictions-Sense": return App.GetString("DisplayableFieldNames.Restrictions-Sense", "Restrictions (Sense)");
				case "SemanticsNote": return App.GetString("DisplayableFieldNames.SemanticsNote", "Semantics Note");
				case "SociolinguisticsNote": return App.GetString("DisplayableFieldNames.SociolinguisticsNote", "Sociolinguistics Note");
				case "ReversalEntries": return App.GetString("DisplayableFieldNames.ReversalEntries", "Reversal Entries");
				case "Source": return App.GetString("DisplayableFieldNames.Source", "Source");
				case "SenseType": return App.GetString("DisplayableFieldNames.SenseType", "Sense Type");
				case "Status": return App.GetString("DisplayableFieldNames.Status", "Status");
				case "ImportResidue-Sense": return App.GetString("DisplayableFieldNames.ImportResidue-Sense", "Import Residue (Sense)");
				case "AnthroCodes": return App.GetString("DisplayableFieldNames.AnthroCategories", "Anthropology Categories");
				case "DomainTypes": return App.GetString("DisplayableFieldNames.DomainTypes", "Domain Types");
				case "SemanticDomains": return App.GetString("DisplayableFieldNames.SemanticDomains", "Semantic Domains");
				case "Usages": return App.GetString("DisplayableFieldNames.Usages", "Usages");
				case "Variants": return App.GetString("DisplayableFieldNames.Variants", "Variants");
				case "VariantTypes": return App.GetString("DisplayableFieldNames.VariantTypes", "Variant Types");
				case "VariantComments": return App.GetString("DisplayableFieldNames.VariantComments", "Variant Comments");
				case "ComplexForms": return App.GetString("DisplayableFieldNames.ComplexForms", "Complex Forms");
				case "Components": return App.GetString("DisplayableFieldNames.Components", "Components");
				case "ComplexTypes": return App.GetString("DisplayableFieldNames.ComplexTypes", "Complex Types");
				case "ComplexFormComments": return App.GetString("DisplayableFieldNames.ComplexFormComments", "Complex Form Comments");
				case "Allomorphs": return App.GetString("DisplayableFieldNames.Allomorphs", "Allomorphs");
			}

			return name;
		}

		#endregion
	}

	#endregion
}
