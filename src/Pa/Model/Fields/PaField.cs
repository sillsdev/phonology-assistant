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
	public class PaField
	{
		private static FieldDisplayPropsCache s_displayPropsCache;

		public const string kPhoneticFieldName = "Phonetic";
		public const string kCVPatternFieldName = "CVPattern";
		public const string kDataSourceFieldName = "DataSource";
		public const string kDataSourcePathFieldName = "DataSourcePath";
		public const string kAudioFileFieldName = "AudioFile";
		public const string kAudioOffsetFieldName = "AudioOffset";
		public const string kAudioLengthFieldName = "AudioLength";

		private string m_isCollection;

		/// ------------------------------------------------------------------------------------
		public PaField()
		{
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
		}

		/// ------------------------------------------------------------------------------------
		public PaField Copy()
		{
			return new PaField
			{
				Name = Name,
				Type = Type,
				SerializablePossibleDataSourceFieldNames = SerializablePossibleDataSourceFieldNames,
				IsHidden = IsHidden,
				FwWsType = FwWsType,
			};
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
		[XmlElement("fwWritingSystemType")]
		public FwDBUtils.FwWritingSystemType FwWsType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsHidden { get; set; }

		#region Display properties
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font Font
		{
			get { return s_displayPropsCache.GetFont(Name); }
			set { s_displayPropsCache.SetFont(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool RightToLeft
		{
			get { return s_displayPropsCache.GetIsRightToLeft(Name); }
			set { s_displayPropsCache.SetIsRightToLeft(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool VisibleInGrid
		{
			get { return !IsHidden && s_displayPropsCache.GetIsVisibleInGrid(Name); }
			set { s_displayPropsCache.SetIsVisibleInGrid(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool VisibleInRecView
		{
			get { return !IsHidden && s_displayPropsCache.GetIsVisibleInRecView(Name); }
			set { s_displayPropsCache.SetIsVisibleInRecView(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DisplayIndexInGrid
		{
			get { return (IsHidden ? -1 :s_displayPropsCache.GetIndexInGrid(Name)); }
			set { s_displayPropsCache.SetIndexInGrid(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DisplayIndexInRecView
		{
			get { return (IsHidden ? -1 : s_displayPropsCache.GetIndexInRecView(Name)); }
			set { s_displayPropsCache.SetIndexInRecView(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int WidthInGrid
		{
			get { return s_displayPropsCache.GetWidthInGrid(Name); }
			set { s_displayPropsCache.SetWidthInGrid(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string DisplayName
		{
			get { return PaFieldDisplayProperties.GetDisplayName(Name); }
		}

		#endregion

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

		#region Static methods
		/// ------------------------------------------------------------------------------------
		public static bool GetIsReservedFieldName(string name)
		{
			return ((kCVPatternFieldName + ";" + kDataSourceFieldName + ";" +
				kDataSourcePathFieldName + ";" + kAudioOffsetFieldName + ";" +
				kAudioLengthFieldName).Contains(name));
		}

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
		public static List<PaField> GetProjectFields(PaProject project)
		{
			var path = project.ProjectPathFilePrefix + "Fields.xml";
			var fields = (File.Exists(path) ? LoadFields(path, "Fields") : GetDefaultFields());
			s_displayPropsCache = FieldDisplayPropsCache.LoadProjectFieldDisplayProps(project);
			return fields;
		}

		/// ------------------------------------------------------------------------------------
		public static List<PaField> GetDefaultFields()
		{
			var path = FileLocator.GetFileDistributedWithApplication(App.ConfigFolderName, "DefaultFields.xml");
			return EnsureListContainsCalculatedFields(LoadFields(path, "DefaultPaFields")).ToList();
		}

		/// ------------------------------------------------------------------------------------
		public static Exception SaveProjectFields(PaProject project)
		{
			if (s_displayPropsCache == null)
				s_displayPropsCache = FieldDisplayPropsCache.LoadProjectFieldDisplayProps(project);

			Exception e = s_displayPropsCache.SaveProjectFieldDisplayProps(project);
			var path = project.ProjectPathFilePrefix + "Fields.xml";
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
			var field = fields.SingleOrDefault(f => f.Name == kAudioOffsetFieldName);
			if (field != null)
				field.IsHidden = true;
			else
			{
				field = new PaField(kAudioOffsetFieldName, FieldType.AudioOffset);
				field.IsHidden = true;
				fields.Add(field);
			}

			field = fields.SingleOrDefault(f => f.Name == kAudioLengthFieldName);
			if (field != null)
				field.IsHidden = true;
			else
			{
				field = new PaField(kAudioLengthFieldName, FieldType.AudioLength);
				field.IsHidden = true;
				fields.Add(field);
			}

			if (!fields.Any(f => f.Name == kDataSourcePathFieldName))
				fields.Add(new PaField(kDataSourcePathFieldName, FieldType.GeneralFilePath));

			if (!fields.Any(f => f.Name == kDataSourceFieldName))
				fields.Add(new PaField(kDataSourceFieldName, FieldType.GeneralFilePath));

			if (!fields.Any(f => f.Name == kCVPatternFieldName))
				fields.Add(new PaField(kCVPatternFieldName, default(FieldType)));

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
