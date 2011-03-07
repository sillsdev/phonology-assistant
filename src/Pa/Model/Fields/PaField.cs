using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Palaso.IO;
using SIL.Pa.DataSource.FieldWorks;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public enum FieldType
	{
		GeneralText,
		GeneralNumeric,
		GeneralFilePath,
		Date,
		Reference,
		Phonetic,
		AudioFilePath,
		AudioOffset,
		AudioLength,
	}

	/// ----------------------------------------------------------------------------------------
	[XmlType("field")]
	public class PaField : IDisposable
	{
		public const string kCVPatternFieldName = "CVPattern";
		public const string kDataSourceFieldName = "DataSource";
		public const string kDataSourcePathFieldName = "DataSourcePath";

		private string m_isCollection;
		private Font m_font;

		/// ------------------------------------------------------------------------------------
		public PaField()
		{
			AllowUserToMap = true;
			VisibleInGrid = true;
			VisibleInRecView = true;
			WidthInGrid = 100;
		}

		/// ------------------------------------------------------------------------------------
		public PaField(string name) : this(name, default(FieldType))
		{
		}

		/// ------------------------------------------------------------------------------------
		public PaField(string name, FieldType type) : this()
		{
			Name = name;
			Type = type;

			if (type == FieldType.Phonetic)
				Font = App.PhoneticFont;
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
		[XmlAttribute("type")]
		public FieldType Type { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used only for deserialization. It always returns null. Use the IsCollection
		/// property to get a boolean value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("collection")]
		public string collection
		{
			get { return null; }
			set { m_isCollection = value; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsCollection
		{
			get { return (m_isCollection != null && m_isCollection.ToLower().Trim() == "true"); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("possibleDataSourceFieldNames")]
		public string SerializablePossibleDataSourceFieldNames { get; set; }

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
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the default grid setup dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("visibleInGrid")]
		public bool VisibleInGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the word list tab in the options
		/// dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("visibleInRecView")]
		public bool VisibleInRecView { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the word list tab on the
		/// tools/options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("displayIndexInGrid")]
		public int DisplayIndexInGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the record view tab on the
		/// tools/options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("displayIndexInRecView")]
		public int DisplayIndexInRecView { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the default grid setup dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("widthInGrid")]
		public int WidthInGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("fwWritingSystemType")]
		public FwDBUtils.FwWritingSystemType FwWsType { get; set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool AllowUserToMap { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string DisplayName
		{
			get { return GetDisplayName(Name); }
		}

		/// ------------------------------------------------------------------------------------
		public string GetTypeDisplayName()
		{
			return GetDisplayableFieldTypes().Single(kvp => kvp.Key == Type).Value;
		}

		/// ------------------------------------------------------------------------------------
		public string[] GetPossibleDataSourceFieldNames()
		{
			return (SerializablePossibleDataSourceFieldNames == null ? new string[] { } :
				SerializablePossibleDataSourceFieldNames.Split(new[] { ';', ',' },
					StringSplitOptions.RemoveEmptyEntries));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the field's Type is one that allows its
		/// IsParsed property to be set to true. Allowed types are Phonetic, Reference,
		/// General Text and General Numeric.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetTypeAllowsFieldToBeParsed()
		{
			return (Type == FieldType.Phonetic || Type == FieldType.GeneralText ||
				Type == FieldType.GeneralNumeric || Type == FieldType.Reference);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the field's Type is one that allows its
		/// IsInterlinear property to be set to true. Allowed types are Phonetic
		/// and General Text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool GetTypeAllowsFieldToBeInterlinear()
		{
			return (Type == FieldType.Phonetic || Type == FieldType.GeneralText);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (DisplayName ?? base.ToString());
		}

		/// ------------------------------------------------------------------------------------
		public PaField Copy()
		{
			return new PaField
			{
				Name = Name,
				Type = Type,
				SerializablePossibleDataSourceFieldNames = SerializablePossibleDataSourceFieldNames,
				VisibleInGrid = VisibleInGrid,
				VisibleInRecView = VisibleInRecView,
				DisplayIndexInGrid = DisplayIndexInGrid,
				DisplayIndexInRecView = DisplayIndexInRecView,
				WidthInGrid = WidthInGrid,
				AllowUserToMap = AllowUserToMap,
				RightToLeft = RightToLeft,
				FwWsType = FwWsType,
				Font = m_font,
			};
		}

		#region Static methods
		/// ------------------------------------------------------------------------------------
		public static bool GetIsTypeParsable(FieldType type)
		{
			return (type == FieldType.Phonetic || type == FieldType.GeneralText ||
				type == FieldType.GeneralNumeric || type == FieldType.Reference);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsTypeInterlinearizable(FieldType type)
		{
			return (type == FieldType.Phonetic || type == FieldType.GeneralText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified field should have a font saved with it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool GetShouldFieldHaveFont(PaField field)
		{
			return (field.Type != FieldType.AudioLength && field.Type != FieldType.AudioOffset);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Merges two lists of fields, returning a list of distinct fields by name. A null
		/// list will be treated as an empty list, so null will never be returned, but an
		/// empty list may. The returned, merged list, contains deep copies of those fields
		/// found in the two lists. If a field is found in both lists, a copy of the one 
		/// from list1 will be returned in the merged list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<PaField> Merge(IEnumerable<PaField> list1, IEnumerable<PaField> list2)
		{
			// I think there's a slicker linq way to do this, but I couldn't figure it out and
			// as I tried, it looked like it may end up looking more complicated to read.

			var newList = (list1 == null ? new List<PaField>() : list1.ToList());
			if (list2 != null)
				newList.AddRange(list2.Where(field => !newList.Any(f => f.Name == field.Name)));
	
			return newList.Select(f => f.Copy());
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetSaFields()
		{
			var path = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultSaFields.xml");
			return LoadFields(path, "SaFields");
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetDefaultSfmFields()
		{
			var path = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultSfmFields.xml");
			return LoadFields(path, "SfmFields");
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetDefaultFw7Fields()
		{
			var path = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultFw7Fields.xml");
			return LoadFields(path, "Fw7Fields");
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetProjectFields(PaProject project)
		{
			var path = project.ProjectPathFilePrefix + "Fields.xml";
			var fields = (File.Exists(path) ? LoadFields(path, "Fields") : new List<PaField>());

			// Initialize the phonetic font if it hasn't been specified.
			var field = fields.SingleOrDefault(f => f.Type == FieldType.Phonetic);
			if (field != null && field.m_font == null)
				field.Font = App.PhoneticFont;

			// Initialize the CV pattern font if it hasn't been specified.
			field = fields.SingleOrDefault(f => f.Name == kCVPatternFieldName);
			if (field != null && field.m_font == null)
				field.Font = App.PhoneticFont;

			return fields;
		}

		/// ------------------------------------------------------------------------------------
		public static Exception SaveProjectFields(PaProject project)
		{
			var path = project.ProjectPathFilePrefix + "Fields.xml";
			Exception e = null;
			XmlSerializationHelper.SerializeToFile(path, project.Fields.ToList(), "Fields", out e);
			return e;
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> LoadFields(string path, string rootElementName)
		{
			Exception e;
			var list = XmlSerializationHelper.DeserializeFromFile<List<PaField>>(path, rootElementName, out e);

			if (e == null)
				return EnsureListContainsCalculatedFields(list).ToList();

			var msg = App.LocalizeString("ReadingFieldsFileErrorMsg",
				"The following error occurred when reading the file\n\n'{0}'\n\n{1}",
				App.kLocalizationGroupInfoMsg);

			while (e.InnerException != null)
				e = e.InnerException;

			Utils.MsgBox(string.Format(msg, path, e.Message));

			return null;
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<PaField> EnsureListContainsCalculatedFields(List<PaField> fields)
		{
			if (!fields.Any(f => f.Name == kDataSourceFieldName))
				fields.Add(GetUnmappableField(kDataSourceFieldName, default(FieldType)));

			if (!fields.Any(f => f.Name == kDataSourcePathFieldName))
				fields.Add(GetUnmappableField(kDataSourcePathFieldName, FieldType.GeneralFilePath));

			if (!fields.Any(f => f.Name == kCVPatternFieldName))
			{
				var phoneticField = fields.SingleOrDefault(f => f.Type == FieldType.Phonetic);
				var cvField = GetUnmappableField(kCVPatternFieldName, default(FieldType));
				cvField.Font = (phoneticField != null ? phoneticField.Font : App.PhoneticFont);
				fields.Add(cvField);
			}

			return fields.OrderBy(f => f.DisplayName);
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<PaField> GetCalculatedFieldsFromList(IEnumerable<PaField> fields)
		{
			return fields.Where(f => f.Name == kCVPatternFieldName ||
				f.Name == kDataSourceFieldName || f.Name == kDataSourcePathFieldName);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsCalculatedField(PaField field)
		{
			return (field.Name == kCVPatternFieldName ||
				field.Name == kDataSourceFieldName || field.Name == kDataSourcePathFieldName);
		}

		/// ------------------------------------------------------------------------------------
		private static PaField GetUnmappableField(string name, FieldType type)
		{
			return new PaField(name, type)
			{
				AllowUserToMap = false,
				VisibleInGrid = false,
				VisibleInRecView = false,
			};
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<KeyValuePair<FieldType, string>> GetDisplayableFieldTypes()
		{
			yield return new KeyValuePair<FieldType, string>(FieldType.GeneralText,
				App.LocalizeString("DisplayableFieldTypeNames.GeneralText", "General Text",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.GeneralNumeric,
				App.LocalizeString("DisplayableFieldTypeNames.GeneralNumeric", "General Numeric",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.GeneralFilePath,
				App.LocalizeString("DisplayableFieldTypeNames.GeneralFilePath", "General File Path",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.Date,
				App.LocalizeString("DisplayableFieldTypeNames.Date", "Date/Time",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.Reference,
				App.LocalizeString("DisplayableFieldTypeNames.Reference", "Reference",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.Phonetic,
				App.LocalizeString("DisplayableFieldTypeNames.Phonetic", "Phonetic",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.AudioFilePath,
				App.LocalizeString("DisplayableFieldTypeNames.AudioFilePath", "Audio File Path",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.AudioOffset,
				App.LocalizeString("DisplayableFieldTypeNames.AudioFileOffset", "Aduio Offset",
				App.kLocalizationGroupMisc));

			yield return new KeyValuePair<FieldType, string>(FieldType.AudioLength,
				App.LocalizeString("DisplayableFieldTypeNames.AudioFileLength", "Aduio Length",
				App.kLocalizationGroupMisc));
		}

		/// ------------------------------------------------------------------------------------
		public static string GetDisplayName(string name)
		{
			switch (name)
			{
				case kCVPatternFieldName: return App.LocalizeString("DisplayableFieldNames.CVPattern",
										"CV Pattern", App.kLocalizationGroupMisc);

				case kDataSourceFieldName: return App.LocalizeString("DisplayableFieldNames.DataSource",
										"Data Source", App.kLocalizationGroupMisc);

				case kDataSourcePathFieldName: return App.LocalizeString("DisplayableFieldNames.DataSourcePath",
										"Data Source Path", App.kLocalizationGroupMisc);

				case "Reference": return App.LocalizeString("DisplayableFieldNames.Reference",
										"Reference", App.kLocalizationGroupMisc);

				case "Phonetic": return App.LocalizeString("DisplayableFieldNames.Phonetic",
										"Phonetic", App.kLocalizationGroupMisc);

				case "Gloss": return App.LocalizeString("DisplayableFieldNames.Gloss",
										"Gloss", App.kLocalizationGroupMisc);

				case "NatGloss": return App.LocalizeString("DisplayableFieldNames.NatGloss",
										"Gloss (Nat.)", App.kLocalizationGroupMisc);

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

				case "AudioLength": return App.LocalizeString("DisplayableFieldNames.AudioLength",
										"Audio Length", App.kLocalizationGroupMisc);

				case "AudioOffset": return App.LocalizeString("DisplayableFieldNames.AudioOffset",
										"Audio Offset", App.kLocalizationGroupMisc);

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

	/// ----------------------------------------------------------------------------------------
	public class FieldNameComparer : IEqualityComparer<PaField>
	{
		#region IEqualityComparer<PaField> Members

		/// ------------------------------------------------------------------------------------
		public bool Equals(PaField x, PaField y)
		{
			if (x == null && y == null)
				return true;

			if (x == null || y == null)
				return false;

			return (x.Name == y.Name);
		}

		/// ------------------------------------------------------------------------------------
		public int GetHashCode(PaField obj)
		{
			return obj.Name.GetHashCode();
		}

		#endregion
	}
}
