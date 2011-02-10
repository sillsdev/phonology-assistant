using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Palaso.IO;
using SIL.Pa.DataSource;
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
	public class PaField
	{
		//private bool m_isParsed;
		//private bool m_isInterlinear;
		private Font m_font;

		/// ------------------------------------------------------------------------------------
		public PaField()
		{
		}

		/// ------------------------------------------------------------------------------------
		public PaField(string name, FieldType type)
		{
			Name = name;
			Type = type;
		}

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("type")]
		public FieldType Type { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("nameInSource")]
		public string NameInSource { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("possibleDataSourceFieldNames")]
		public string SerializablePossibleDataSourceFieldNames { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font Font
		{
			get { return (m_font ?? FontHelper.UIFont); }
			set { m_font = value; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("font")]
		public SerializableFont SFont
		{
			get { return (m_font == null ? null : new SerializableFont(m_font)); }
			set { if (value != null) m_font = value.Font; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlElement("rightToLeft")]
		public bool RightToLeft { get; set; }

		///// ------------------------------------------------------------------------------------
		//[XmlElement("isParsed")]
		//public bool IsParsed
		//{
		//    get { return m_isParsed; }
		//    set { m_isParsed = (value && GetTypeAllowsFieldToBeParsed()); }
		//}

		///// ------------------------------------------------------------------------------------
		//[XmlElement("isInterlinear")]
		//public bool IsInterlinear
		//{
		//    get { return m_isInterlinear; }
		//    set { m_isInterlinear = (value && GetTypeAllowsFieldToBeInterlinear()); }
		//}

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

		#endregion

		/// ------------------------------------------------------------------------------------
		[XmlArray("dataSourceMappings"), XmlArrayItem("mapping")]
		public List<FieldMapping> Mappings { get; set; }

		/// ------------------------------------------------------------------------------------
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
		public bool GetIsParsed(PaDataSource ds)
		{
			return true;
			//var mapping = Mappings.SingleOrDefault(m => m.DataSourceName == ds.ToString(true));
			//return (mapping != null && mapping.IsParsed);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (Name ?? base.ToString());
		}

		/// ------------------------------------------------------------------------------------
		public PaField Copy()
		{
			var copy = new PaField();
			copy.Name = Name;
			copy.Type = Type;
			copy.SerializablePossibleDataSourceFieldNames = SerializablePossibleDataSourceFieldNames;
			copy.NameInSource = NameInSource;
			//copy.IsParsed = IsParsed;
			//copy.IsInterlinear = IsInterlinear;
			//copy.FwWritingSystemType = FwWritingSystemType;
			copy.VisibleInGrid = VisibleInGrid;
			copy.VisibleInRecView = VisibleInRecView;
			copy.DisplayIndexInGrid = DisplayIndexInGrid;
			copy.DisplayIndexInRecView = DisplayIndexInRecView;
			copy.WidthInGrid = WidthInGrid;
			copy.Mappings = Mappings.Select(m => m.Copy()).ToList();
			copy.Font = (m_font != null ? m_font.Clone() as Font : null);
			
			return copy;
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
		/// empty list may.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<PaField> Merge(IEnumerable<PaField> list1, IEnumerable<PaField> list2)
		{
			// I think there's a slicker linq way to do this, but I couldn't figure it out and
			// as I tried, it looked like it may end up looking more complicated to read.

			var newList = (list1 == null ? new List<PaField>() : list1.ToList());
			if (list2 != null)
				newList.AddRange(list2.Where(field => !newList.Any(f => f.Name == field.Name)));
	
			return newList;
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetSaFields()
		{
			var path = FileLocator.GetFileDistributedWithApplication("Configuration", "DefaultSaFields.xml");
			return LoadFields(path, "SaFields");
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetDefaultSfmFields()
		{
			var path = FileLocator.GetFileDistributedWithApplication("Configuration", "DefaultSfmFields.xml");
			return LoadFields(path, "SfmFields");
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetProjectFields(PaProject project)
		{
			var path = project.ProjectPathFilePrefix + "Fields.xml";
			return (File.Exists(path) ? LoadFields(path, "Fields") : new List<PaField>());
		}

		/// ------------------------------------------------------------------------------------
		public static Exception SaveProjectFields(PaProject project)
		{
			var path = project.ProjectPathFilePrefix + "Fields.xml";
			Exception e = null;
			XmlSerializationHelper.SerializeToFile(path, project.Fields, "Fields", out e);
			return e;
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> LoadFields(string path, string rootElementName)
		{
			Exception e;
			var list = XmlSerializationHelper.DeserializeFromFile<List<PaField>>(path, rootElementName, out e);

			if (e == null)
				return list;

			var msg = App.LocalizeString("ReadingFieldsFileErrorMsg",
				"The following error occurred when reading the file\n\n'{0}'\n\n{1}",
				App.kLocalizationGroupInfoMsg);

			while (e.InnerException != null)
				e = e.InnerException;

			Utils.MsgBox(string.Format(msg, path, e.Message));

			return null;
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
				App.LocalizeString("DisplayableFieldTypeNames.Date", "Date",
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
				case "Reference": return App.LocalizeString("DisplayableFieldNames.Reference",
									   "Reference", App.kLocalizationGroupMisc);

				case "Phonetic": return App.LocalizeString("DisplayableFieldNames.Phonetic",
										"Phonetic", App.kLocalizationGroupMisc);

				case "Gloss": return App.LocalizeString("DisplayableFieldNames.Gloss",
										"Gloss", App.kLocalizationGroupMisc);

				case "NatGloss": return App.LocalizeString("DisplayableFieldNames.NatGloss",
										"Gloss (Nat.)", App.kLocalizationGroupMisc);

				case "PartOfSpeech": return App.LocalizeString("DisplayableFieldNames.Reference",
									   "Reference", App.kLocalizationGroupMisc);

				case "Tone": return App.LocalizeString("DisplayableFieldNames.Tone",
										"Tone", App.kLocalizationGroupMisc);

				case "Orthographic": return App.LocalizeString("DisplayableFieldNames.Orthographic",
										"Orthographic", App.kLocalizationGroupMisc);

				case "Phonemic": return App.LocalizeString("DisplayableFieldNames.Phonemic",
										"Phonemic", App.kLocalizationGroupMisc);

				case "AudioFile": return App.LocalizeString("DisplayableFieldNames.AudioFile",
										"Audio File", App.kLocalizationGroupMisc);

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
			}

			return name;
		}

		#endregion
	}
}
