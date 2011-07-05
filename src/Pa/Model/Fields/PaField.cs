using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Palaso.IO;
using Palaso.Reporting;
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
			get { return s_displayPropsCache.GetIsVisibleInGrid(Name); }
			set { s_displayPropsCache.SetIsVisibleInGrid(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool VisibleInRecView
		{
			get { return s_displayPropsCache.GetIsVisibleInRecView(Name); }
			set { s_displayPropsCache.SetIsVisibleInRecView(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DisplayIndexInGrid
		{
			get { return (s_displayPropsCache.GetIndexInGrid(Name)); }
			set { s_displayPropsCache.SetIndexInGrid(Name, value); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int DisplayIndexInRecView
		{
			get { return (s_displayPropsCache.GetIndexInRecView(Name)); }
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
				kDataSourcePathFieldName).Contains(name));
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
			s_displayPropsCache = FieldDisplayPropsCache.LoadProjectFieldDisplayProps(project);
			var defaultFields = GetDefaultFields();
			var path = GetFileForProject(project.ProjectPathFilePrefix);

			if (!File.Exists(path))
				return defaultFields;

			// Go through the project's fields making sure that
			// all the default fields are contained therein.
			var fields = LoadFields(path, "Fields");
			foreach (var fld in defaultFields.Where(fld => !fields.Any(f => f.Name == fld.Name)))
				fields.Add(fld);
			
			return fields;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the project's fields file path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetFileForProject(string projectPathPrefix)
		{
			return projectPathPrefix + "Fields.xml";
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

			var e = s_displayPropsCache.SaveProjectFieldDisplayProps(project);
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

			var msg = App.GetString("ReadingFieldsFileErrorMsg",
				"An error occurred reading the file\n\n'{0}'.");

			ErrorReport.NotifyUserOfProblem(e, msg, path);
			return null;
		}

		/// ------------------------------------------------------------------------------------
		public static IEnumerable<PaField> EnsureListContainsCalculatedFields(List<PaField> fields)
		{
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
				App.GetString("DisplayableFieldTypeNames.GeneralText", "Text"));

			yield return new KeyValuePair<FieldType, string>(FieldType.GeneralNumeric,
				App.GetString("DisplayableFieldTypeNames.GeneralNumeric", "Numeric"));

			yield return new KeyValuePair<FieldType, string>(FieldType.GeneralFilePath,
				App.GetString("DisplayableFieldTypeNames.GeneralFilePath", "File Path"));

			yield return new KeyValuePair<FieldType, string>(FieldType.Date,
				App.GetString("DisplayableFieldTypeNames.Date", "Date/Time"));

			yield return new KeyValuePair<FieldType, string>(FieldType.Reference,
				App.GetString("DisplayableFieldTypeNames.Reference", "Reference"));

			yield return new KeyValuePair<FieldType, string>(FieldType.Phonetic,
				App.GetString("DisplayableFieldTypeNames.Phonetic", "Phonetic"));

			yield return new KeyValuePair<FieldType, string>(FieldType.AudioFilePath,
				App.GetString("DisplayableFieldTypeNames.AudioFilePath", "Audio File Path"));
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
