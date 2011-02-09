using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model.Project;
using SilTools;

namespace SIL.Pa.Model
{
	#region PaFieldValue Class
	/// ----------------------------------------------------------------------------------------
	[XmlType("FieldValueInfo")]
	public class PaFieldValue
	{
		[XmlAttribute("FieldName")]
		public string Name;
		[XmlAttribute]
		public string Value;
		[XmlAttribute]
		public bool IsFirstLineInterlinearField;
		[XmlAttribute]
		public bool IsSubordinateInterlinearField;

		/// ------------------------------------------------------------------------------------
		public PaFieldValue()
		{
		}

		/// ------------------------------------------------------------------------------------
		public PaFieldValue(string name)
		{
			Name = name;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0}: {1}", Name, Value);
		}
	}

	#endregion

	#region PaFieldInfoList Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("PaFields")]
	public class PaFieldInfoList : List<PaFieldInfo>
	{
		private PaFieldInfo m_phoneticField;
		private PaFieldInfo m_dataSourceField;
		private PaFieldInfo m_dataSourcePathField;
		private PaFieldInfo m_audioFileField;

		private const float kCurrVersion = 2.2f;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the PaFieldInfo object of the specified field name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo this[string field]
		{
			get
			{
				return (field == null ? null :
					this.FirstOrDefault(fieldInfo => fieldInfo.FieldName.ToLower() == field.ToLower()));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the fields sorted by their display index in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<PaFieldInfo> SortedList
		{
			get { return this.OrderBy(fi => fi.DisplayIndexInGrid).ToList(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's phonetic field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo PhoneticField
		{
			get 
			{
				return m_phoneticField ??
					(m_phoneticField = this.SingleOrDefault(fi => fi.IsPhonetic));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's reference field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo ReferenceField
		{
			get { return this.SingleOrDefault(fi => fi.IsReference); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's guid field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo GuidField
		{
			get { return this.SingleOrDefault(fi => fi.IsGuid); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's Gloss field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo CVPatternField
		{
			get { return this.SingleOrDefault(fi => fi.IsCVPattern); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's data source field information object. For non FW data sources,
		/// this is a file name. Otherwise, it's the FW project and database name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo DataSourceField
		{
			get
			{
				return (m_dataSourceField ??
					(m_dataSourceField = this.SingleOrDefault(fi => fi.IsDataSource)));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's data source path field information object. This is empty or null
		/// when the data source is a FW data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo DataSourcePathField
		{
			get
			{
				return (m_dataSourcePathField ??
					(m_dataSourcePathField = this.SingleOrDefault(fi => fi.IsDataSourcePath)));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's audio file field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo AudioFileField
		{
			get
			{
				return (m_audioFileField ??
					(m_audioFileField = this.SingleOrDefault(fi => fi.IsAudioFile)));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's audio file offset field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo AudioFileOffsetField
		{
			get { return this.SingleOrDefault(fi => fi.IsAudioOffset); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's audio file length field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo AudioFileLengthField
		{
			get { return this.SingleOrDefault(fi => fi.IsAudioLength); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all the custom fields from the field list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveCustomFields()
		{
			int i = 0;
			while (i < Count)
			{
				if (this[i].IsCustom)
					RemoveAt(i);
				else
					i++;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of default PA fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFieldInfoList DefaultFieldInfoList
		{
			get
			{
				var filename = Path.Combine(App.ConfigFolder, "DefaultFieldInfo.xml");
				var fieldInfoList = XmlSerializationHelper.DeserializeFromFile<PaFieldInfoList>(filename);

				if (fieldInfoList != null)
				{
					foreach (PaFieldInfo fieldInfo in fieldInfoList)
					{
						if (!PaFieldInfo.ShouldFieldHaveFont(fieldInfo))
							fieldInfo.Font = null;
						else if (fieldInfo.Font == null ||
							!FontHelper.FontInstalled(fieldInfo.Font.Name))
						{
							fieldInfo.Font = FontHelper.UIFont;
						}
					}
				}

				return fieldInfoList;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the field information for the specified project. If the project is null or
		/// the file in which the project's field information cannot be found, then the
		/// default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFieldInfoList Load(PaProject project)
		{
			bool saveProjAfterLoad;
			return Load(project, out saveProjAfterLoad);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the field information for the specified project. If the project is null or
		/// the file in which the project's field information cannot be found, then the
		/// default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFieldInfoList Load(PaProject project, out bool saveProjAfterLoad)
		{
			saveProjAfterLoad = false;

			if (project == null)
				return DefaultFieldInfoList;

			bool migrate = true;
			bool doCleanup = false;
			string filename = project.ProjectPathFilePrefix + "FieldInfo.xml";

			// Get the project specific field information.
			var fieldInfoList = XmlSerializationHelper.DeserializeFromFile<PaFieldInfoList>(filename);

			if (fieldInfoList != null)
			{
				doCleanup = true;
				float version = ManageVersion(filename, true);
				migrate = (version < kCurrVersion);
			}
			else
			{
				// The only time we should get here is if the project's field info. file was
				// deleted, moved or corrupted. If that's the case, then just use the
				// default field information and make sure to save the project's new field info.
				fieldInfoList = DefaultFieldInfoList;
			}

			if (migrate)
			{
				AddFwQueryFieldNames(fieldInfoList);
				MigrateFieldNames(fieldInfoList, project);

				if (doCleanup)
					CleanupLoadedFieldList(fieldInfoList);
	
				saveProjAfterLoad = true;
			}

			fieldInfoList.Save(project);
			return fieldInfoList;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will perform various integrity checks on a project's field
		/// information list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void CleanupLoadedFieldList(PaFieldInfoList fieldInfoList)
		{
			// Go through the default fields and make sure the project's
			// field list contains any default fields that may have been
			// added since the project was created.
			foreach (PaFieldInfo defaultField in DefaultFieldInfoList)
			{
				if (fieldInfoList[defaultField.FieldName] == null)
					fieldInfoList.Add(PaFieldInfo.Clone(defaultField));
			}

			SortedList<float, PaFieldInfo> gridIndexes = new SortedList<float, PaFieldInfo>();
			SortedList<float, PaFieldInfo> recVwIndexes = new SortedList<float, PaFieldInfo>();

			// Got through the fields and make sure we don't have any uncessary
			// font objects floating around and make sure there are no duplicate
			// display indexes.
			foreach (PaFieldInfo fieldInfo in fieldInfoList)
			{
				if (!PaFieldInfo.ShouldFieldHaveFont(fieldInfo))
					fieldInfo.Font = null;

				// Check if this field's word list grid display index is already being used.
				float index = fieldInfo.DisplayIndexInGrid;
				if (index > -1)
				{
					while (gridIndexes.ContainsKey(index))
						index += 0.001f;

					gridIndexes[index] = fieldInfo;
				}

				// Check if this field's record view display index is already being used.
				index = fieldInfo.DisplayIndexInRecView;
				if (index > -1)
				{
					while (recVwIndexes.ContainsKey(index))
						index += 0.001f;

					recVwIndexes[index] = fieldInfo;
				}
			}

			// Go through the word list grid display indexes and renumber using integers.
			int newIndex = 0;
			foreach (PaFieldInfo fieldInfo in gridIndexes.Values)
				fieldInfo.DisplayIndexInGrid = newIndex++;

			// Go through the record view display indexes and renumber using integers.
			newIndex = 0;
			foreach (PaFieldInfo fieldInfo in recVwIndexes.Values)
				fieldInfo.DisplayIndexInRecView = newIndex++;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates (migrate) field info. lists older than version 2.1 with the FW query
		/// field names from the default list of fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddFwQueryFieldNames(PaFieldInfoList fieldInfoList)
		{
			foreach (var fieldInfo in DefaultFieldInfoList)
			{
				PaFieldInfo pfi = fieldInfoList[fieldInfo.FieldName];
				if (pfi != null)
					pfi.FwQueryFieldName = fieldInfo.FwQueryFieldName;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update (migrate) field names so the internal names for fields is the same as the
		/// field's display text. This method is only to update the field names for projects
		/// whose field info. version is earlier than 2.1.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void MigrateFieldNames(IEnumerable<PaFieldInfo> fieldInfoList, PaProject project)
		{
			if (project == null)
				return;
			
			project.MigrateFwDatasourceFieldNames();

			foreach (var fieldInfo in fieldInfoList)
			{
				project.ProcessRenamedField(fieldInfo.FieldName, fieldInfo.DisplayText);
				fieldInfo.FieldName = fieldInfo.DisplayText;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the field information for the specified project. If project is null then
		/// the default list is loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(PaProject project)
		{
			if (project != null)
			{
				string filename = project.ProjectPathFilePrefix + "FieldInfo.xml";
				XmlSerializationHelper.SerializeToFile(filename, this);
				ManageVersion(filename, false);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the list of default PA fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var filename = Path.Combine(App.ConfigFolder, "DefaultFieldInfo.xml");
			XmlSerializationHelper.SerializeToFile(filename, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method manages reading and writing the file version. It would be great if the
		/// version could just be a public variable that was serialized and deserialized with
		/// this class object, but for some reason, (I think it's because this class is
		/// derived from a generic list) serializing this list doesn't serialize public
		/// variables and properties in classes derived from List, which this class is.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static float ManageVersion(string filename, bool getVer)
		{
			try
			{
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(filename);
				var node = xmlDoc.SelectSingleNode("PaFields");

				if (getVer)
					return XmlHelper.GetFloatFromAttribute(node, "version", 0f);

				// Write the version to the file. Save the file using a XmlTextWriter
				// instead of just passing the file name to the save method. Otherwise
				// the XML file will contain a BOM and that causes trouble.
				XmlTextWriter writer = new XmlTextWriter(filename, new System.Text.UTF8Encoding(false));
				writer.Formatting = Formatting.Indented;
				xmlDoc.DocumentElement.SetAttribute("version", kCurrVersion.ToString());
				xmlDoc.Save(writer);
				writer.Flush();
				writer.Close();
			}
			catch { }

			return 0f;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal field name for the specified display text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetFieldNameFromDisplayText(string displayText)
		{
			var fieldInfo = GetFieldFromDisplayText(displayText);
			return (fieldInfo == null ? null : fieldInfo.FieldName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal field name for the specified display text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo GetFieldFromDisplayText(string displayText)
		{
			PaFieldInfo retVal = null;

			if (displayText != null)
			{
				displayText = displayText.ToLower();
				retVal = this.SingleOrDefault(fi =>
					fi.DisplayText != null && fi.DisplayText.ToLower() == displayText);
			}

			return retVal;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal field name for the specified display text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo GetFieldFromFwQueryFieldName(string fwQueryFieldNam)
		{
			PaFieldInfo retVal = null;

			if (fwQueryFieldNam != null)
			{
				fwQueryFieldNam = fwQueryFieldNam.ToLower();
				retVal = this.SingleOrDefault(fi =>
					fi.FwQueryFieldName != null && fi.FwQueryFieldName.ToLower() == fwQueryFieldNam);
			}

			return retVal;
		}
	}

	#endregion

	#region PaFieldInfo
	/// ----------------------------------------------------------------------------------------
	[XmlType("Field")]
	public class PaFieldInfo
	{
		public const string kCustomFieldPrefix = "CustomField";

		private string m_displayText;
		private bool m_isFwField;
		private Font m_font = FontHelper.UIFont;

		/// ------------------------------------------------------------------------------------
		public PaFieldInfo()
		{
			WidthInGrid = -1;
			DisplayIndexInRecView = -1;
			DisplayIndexInGrid = -1;
			VisibleInRecView = true;
			VisibleInGrid = true;
			FwWritingSystemType = FwDBUtils.FwWritingSystemType.None;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones the specified PaFieldInfo object and returns the new, clone instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static PaFieldInfo Clone(PaFieldInfo source)
		{
			if (source == null)
				return null;

			PaFieldInfo clone = new PaFieldInfo();
			clone.FieldName = source.FieldName;
			clone.m_displayText = source.m_displayText;
			clone.RightToLeft = source.RightToLeft;
			clone.CanBeInterlinear = source.CanBeInterlinear;
			clone.IsPhonetic = source.IsPhonetic;
			clone.IsReference = source.IsReference;
			clone.IsDate = source.IsDate;
			clone.m_isFwField = source.m_isFwField;
			clone.FwQueryFieldName = source.FwQueryFieldName;
			clone.IsDataSource = source.IsDataSource;
			clone.IsDataSourcePath = source.IsDataSourcePath;
			clone.IsAudioFile = source.IsAudioFile;
			clone.IsAudioOffset = source.IsAudioOffset;
			clone.IsAudioLength = source.IsAudioLength;
			clone.IsCustom = source.IsCustom;
			clone.IsParsed = source.IsParsed;
			clone.IsNumeric = source.IsNumeric;
			clone.IsGuid = source.IsGuid;
			clone.VisibleInGrid = source.VisibleInGrid;
			clone.VisibleInRecView = source.VisibleInRecView;
			clone.DisplayIndexInGrid = source.DisplayIndexInGrid;
			clone.DisplayIndexInRecView = source.DisplayIndexInRecView;
			clone.WidthInGrid = source.WidthInGrid;
			clone.FwWritingSystemType = source.FwWritingSystemType;
			clone.Font = null;

			if (source.m_font != null)
			{
				FontStyle style = FontStyle.Regular;
				if (source.m_font.Bold)
					style = FontStyle.Bold;
				if (source.m_font.Italic)
					style |= FontStyle.Italic;

				clone.m_font = new Font(source.m_font, style);
			}

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified field should have a font saved with it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool ShouldFieldHaveFont(PaFieldInfo fieldInfo)
		{
			return (!fieldInfo.IsGuid && !fieldInfo.IsAudioLength && !fieldInfo.IsAudioOffset);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The field's name the user sees in the UI.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return DisplayText;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Name")]
		public string FieldName { get; set; }

		/// ------------------------------------------------------------------------------------
		public string DisplayText
		{
			get 
			{
				if (!string.IsNullOrEmpty(m_displayText))
					return m_displayText;

				string text = PaFieldsDisplayText.ResourceManager.GetString("kstid" + FieldName);
				return (string.IsNullOrEmpty(text) ? FieldName : text);
			}
			set { m_displayText = value; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font Font
		{
			get { return (m_font ?? SystemInformation.MenuFont); }
			set { m_font = value; }
		}

		/// ------------------------------------------------------------------------------------
		public FwDBUtils.FwWritingSystemType FwWritingSystemType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("Font")]
		public SerializableFont SFont
		{
			get { return (m_font == null ? null : new SerializableFont(m_font)); }
			set { if (value != null) m_font = value.Font; }
		}

		/// ------------------------------------------------------------------------------------
		public bool RightToLeft { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool CanBeInterlinear { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsParsed { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsPhonetic { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsReference { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsCVPattern { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsDate { get; set; }

		/// ------------------------------------------------------------------------------------
		public string FwQueryFieldName { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsDataSource { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsDataSourcePath { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsAudioFile { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsAudioOffset { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsAudioLength { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsCustom { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsNumeric { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool IsGuid { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the default grid setup dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool VisibleInGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the word list tab in the options
		/// dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool VisibleInRecView { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the word list tab on the
		/// tools/options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DisplayIndexInGrid { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the record view tab on the
		/// tools/options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DisplayIndexInRecView { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the default grid setup dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int WidthInGrid { get; set; }

		#endregion
	}

	#endregion
}
