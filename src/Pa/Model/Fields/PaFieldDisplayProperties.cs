using System;
using System.Collections.Generic;
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
		private static Dictionary<string, string> s_displayNames;

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
		public static void ResetDisplayNameCache()
		{
			s_displayNames.Clear();
		}

		/// ------------------------------------------------------------------------------------
		public static string GetDisplayName(string fname)
		{
			if (fname == null)
				return null;

			string dispName;

			if (s_displayNames == null)
				s_displayNames = new Dictionary<string, string>();
			else if (s_displayNames.TryGetValue(fname, out dispName))
				return dispName;

			switch (fname)
			{
				case PaField.kCVPatternFieldName: s_displayNames[fname] = App.GetString("DisplayableFieldNames.CVPattern", "CV Pattern"); break;
				case PaField.kDataSourceFieldName: s_displayNames[fname] = App.GetString("DisplayableFieldNames.DataSource", "Data Source"); break;
				case PaField.kDataSourcePathFieldName: s_displayNames[fname] = App.GetString("DisplayableFieldNames.DataSourcePath", "Data Source Path"); break;
				case "Reference": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Reference", "Reference"); break;
				case "Phonetic": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Phonetic", "Phonetic"); break;
				case "Gloss": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Gloss", "Gloss"); break;
				case "Gloss-Secondary": s_displayNames[fname] = App.GetString("DisplayableFieldNames.GlossSecondary", "Gloss (Secondary)"); break;
				case "Gloss-Other": s_displayNames[fname] = App.GetString("DisplayableFieldNames.GlossOther", "Gloss (Other)"); break;
				case "PartOfSpeech": s_displayNames[fname] = App.GetString("DisplayableFieldNames.PartOfSpeech", "Part of Speech"); break;
				case "Tone": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Tone", "Tone"); break;
				case "Orthographic": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Orthographic", "Orthographic"); break;
				case "Phonemic": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Phonemic", "Phonemic"); break;
				case "AudioFile": s_displayNames[fname] = App.GetString("DisplayableFieldNames.AudioFile", "Audio File"); break;
				case "AudioFileLabel": s_displayNames[fname] = App.GetString("DisplayableFieldNames.AudioFileLabel", "Audio File Label"); break;
				case "FreeFormTranslation": s_displayNames[fname] = App.GetString("DisplayableFieldNames.FreeFormTranslation", "Free Translation"); break;
				case "NoteBookReference": s_displayNames[fname] = App.GetString("DisplayableFieldNames.NoteBookReference", "Note Book Ref."); break;
				case "Dialect": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Dialect", "Dialect"); break;
				case "EthnologueId": s_displayNames[fname] = App.GetString("DisplayableFieldNames.EthnologueId", "Ethnologue Id"); break;
				case "LanguageName": s_displayNames[fname] = App.GetString("DisplayableFieldNames.LanguageName", "Language Name"); break;
				case "Region": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Region", "Region"); break;
				case "Country": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Country", "Country"); break;
				case "Family": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Family", "Family"); break;
				case "Transcriber": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Transcriber", "Transcriber"); break;
				case "SpeakerName": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SpeakerName", "Speaker Name"); break;
				case "SpeakerGender": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SpeakerGender", "Speaker Gender"); break;
				case "SADescription": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SADescription", "Description"); break;
				case "CitationForm": s_displayNames[fname] = App.GetString("DisplayableFieldNames.CitationForm", "Citation Form"); break;
				case "MorphType": s_displayNames[fname] = App.GetString("DisplayableFieldNames.MorphType", "Morpheme Type"); break;
				case "Etymology": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Etymology", "Etymology"); break;
				case "LiteralMeaning": s_displayNames[fname] = App.GetString("DisplayableFieldNames.LiteralMeaning", "Literal Meaning"); break;
				case "Bibliography": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Bibliography", "Bibliography"); break;
				case "Restrictions": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Restrictions", "Restrictions"); break;
				case "SummaryDefinition": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SummaryDefinition", "Summary Definition"); break;
				case "Note": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Note", "Note"); break;
				case "CV-Pattern-Source": s_displayNames[fname] = App.GetString("DisplayableFieldNames.DataSourceCVPattern", "CV Pattern (from source)"); break;
				case "Location": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Location", "Location"); break;
				case "ExcludeAsHeadword": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ExcludeAsHeadword", "Exclude As Headword"); break;
				case "ImportResidue": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ImportResidue", "Import Residue"); break;
				case "DateCreated": s_displayNames[fname] = App.GetString("DisplayableFieldNames.DateCreated", "Date Created"); break;
				case "DateModified": s_displayNames[fname] = App.GetString("DisplayableFieldNames.DateModified", "Date Modified"); break;
				case "Definition": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Definition", "Definition"); break;
				case "ScientificName": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ScientificName", "Scientific Name"); break;
				case "AnthropologyNote": s_displayNames[fname] = App.GetString("DisplayableFieldNames.AnthropologyNote", "Anthropology Note"); break;
				case "Bibliography-Sense": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Bibliography-Sense", "Bibliography (Sense)"); break;
				case "DiscourseNote": s_displayNames[fname] = App.GetString("DisplayableFieldNames.DiscourseNote", "Discourse Note"); break;
				case "EncyclopedicInfo": s_displayNames[fname] = App.GetString("DisplayableFieldNames.EncyclopedicInfo", "Encyclopedic Info."); break;
				case "GeneralNote": s_displayNames[fname] = App.GetString("DisplayableFieldNames.GeneralNote", "General Note"); break;
				case "GrammarNote": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Grammar Note", "Grammar Note"); break;
				case "PhonologyNote": s_displayNames[fname] = App.GetString("DisplayableFieldNames.PhonologyNote", "Phonology Note"); break;
				case "Restrictions-Sense": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Restrictions-Sense", "Restrictions (Sense)"); break;
				case "SemanticsNote": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SemanticsNote", "Semantics Note"); break;
				case "SociolinguisticsNote": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SociolinguisticsNote", "Sociolinguistics Note"); break;
				case "ReversalEntries": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ReversalEntries", "Reversal Entries"); break;
				case "Source": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Source", "Source"); break;
				case "SenseType": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SenseType", "Sense Type"); break;
				case "Status": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Status", "Status"); break;
				case "ImportResidue-Sense": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ImportResidue-Sense", "Import Residue (Sense)"); break;
				case "AnthroCodes": s_displayNames[fname] = App.GetString("DisplayableFieldNames.AnthroCategories", "Anthropology Categories"); break;
				case "DomainTypes": s_displayNames[fname] = App.GetString("DisplayableFieldNames.DomainTypes", "Domain Types"); break;
				case "SemanticDomains": s_displayNames[fname] = App.GetString("DisplayableFieldNames.SemanticDomains", "Semantic Domains"); break;
				case "Usages": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Usages", "Usages"); break;
				case "Variants": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Variants", "Variants"); break;
				case "VariantTypes": s_displayNames[fname] = App.GetString("DisplayableFieldNames.VariantTypes", "Variant Types"); break;
				case "VariantComments": s_displayNames[fname] = App.GetString("DisplayableFieldNames.VariantComments", "Variant Comments"); break;
				case "ComplexForms": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ComplexForms", "Complex Forms"); break;
				case "Components": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Components", "Components"); break;
				case "ComplexTypes": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ComplexTypes", "Complex Types"); break;
				case "ComplexFormComments": s_displayNames[fname] = App.GetString("DisplayableFieldNames.ComplexFormComments", "Complex Form Comments"); break;
				case "Allomorphs": s_displayNames[fname] = App.GetString("DisplayableFieldNames.Allomorphs", "Allomorphs"); break;
				default: return fname;
			}

			return s_displayNames[fname];
		}

		#endregion
	}

	#endregion
}
