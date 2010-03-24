using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.Model.Project;
using SilUtils;

namespace SIL.Pa.Model
{
	#region PaFieldValue Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldValue()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldValue(string name)
		{
			Name = name;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
				if (field != null)
				{
					foreach (PaFieldInfo fieldInfo in this)
					{
						if (fieldInfo.FieldName.ToLower() == field.ToLower())
							return fieldInfo;
					}
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the fields sorted by their display index in the grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<PaFieldInfo> SortedList
		{
			get
			{
				SortedList<int, PaFieldInfo> sortedList = new SortedList<int, PaFieldInfo>();
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.DisplayIndexInGrid >= 0)
						sortedList[fieldInfo.DisplayIndexInGrid] = fieldInfo;
				}

				List<PaFieldInfo> returnList = new List<PaFieldInfo>();
				foreach (PaFieldInfo fieldInfo in sortedList.Values)
					returnList.Add(fieldInfo);

				return returnList;
			}
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
				if (m_phoneticField == null)
				{
					foreach (PaFieldInfo fieldInfo in this)
					{
						if (fieldInfo.IsPhonetic)
						{
							m_phoneticField = fieldInfo;
							break;
						}
					}
				}

				return m_phoneticField;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's phonemic field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo PhonemicField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsPhonemic)
						return fieldInfo;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's Tone field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo ToneField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsTone)
						return fieldInfo;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's Orthographic field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo OrthoField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsOrtho)
						return fieldInfo;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's Gloss field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo GlossField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsGloss)
						return fieldInfo;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's reference field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo ReferenceField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsReference)
						return fieldInfo;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's guid field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo GuidField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsGuid)
						return fieldInfo;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's Gloss field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo CVPatternField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsCVPattern)
						return fieldInfo;
				}

				return null;
			}
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
				if (m_dataSourceField == null)
				{
					foreach (PaFieldInfo fieldInfo in this)
					{
						if (fieldInfo.IsDataSource)
						{
							m_dataSourceField = fieldInfo;
							break;
						}
					}
				}

				return m_dataSourceField;
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
				if (m_dataSourcePathField == null)
				{
					foreach (PaFieldInfo fieldInfo in this)
					{
						if (fieldInfo.IsDataSourcePath)
						{
							m_dataSourcePathField = fieldInfo;
							break;
						}
					}
				}

				return m_dataSourcePathField;
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
				if (m_audioFileField == null)
				{
					foreach (PaFieldInfo fieldInfo in this)
					{
						if (fieldInfo.IsAudioFile)
						{
							m_audioFileField = fieldInfo;
							break;
						}
					}
				}

				return m_audioFileField;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's audio file offset field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo AudioFileOffsetField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsAudioOffset)
						return fieldInfo;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list's audio file length field information object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo AudioFileLengthField
		{
			get
			{
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.IsAudioLength)
						return fieldInfo;
				}

				return null;
			}
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
				PaFieldInfoList fieldInfoList = Utils.DeserializeData("DefaultFieldInfo.xml",
					typeof(PaFieldInfoList)) as PaFieldInfoList;

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
			PaFieldInfoList fieldInfoList = Utils.DeserializeData(filename,
				typeof(PaFieldInfoList)) as PaFieldInfoList;

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
			foreach (PaFieldInfo fieldInfo in DefaultFieldInfoList)
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
			if (project != null)
			{
				project.MigrateFwDatasourceFieldNames();

				foreach (PaFieldInfo fieldInfo in fieldInfoList)
				{
					project.ProcessRenamedField(fieldInfo.FieldName, fieldInfo.DisplayText);
					fieldInfo.FieldName = fieldInfo.DisplayText;
				}
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
				Utils.SerializeData(filename, this);
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
			Utils.SerializeData("DefaultFieldInfo.xml", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method manages reading and writing the file version. It would be great if the
		/// version could just be a public variable that was serialized and deserialized with
		/// this class object, but for some reason, (I think it's because this class is
		/// derived from a generic list) serializing this list doesn't serialize public
		/// variables and properties in classes derived from List<>, which this class is.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static float ManageVersion(string filename, bool getVer)
		{
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(filename);
				XmlNode node = xmlDoc.SelectSingleNode("PaFields");

				if (getVer)
					return XMLHelper.GetFloatFromAttribute(node, "version", 0f);

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
			PaFieldInfo fieldInfo = GetFieldFromDisplayText(displayText);
			return (fieldInfo == null ? null : fieldInfo.FieldName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal field name for the specified display text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo GetFieldFromDisplayText(string displayText)
		{
			if (displayText != null)
			{
				displayText = displayText.ToLower();
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.DisplayText != null &&
						displayText == fieldInfo.DisplayText.ToLower())
					{
						return fieldInfo;
					}
				}
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the internal field name for the specified display text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfo GetFieldFromFwQueryFieldName(string fwQueryFieldNam)
		{
			if (fwQueryFieldNam != null)
			{
				fwQueryFieldNam = fwQueryFieldNam.ToLower();
				foreach (PaFieldInfo fieldInfo in this)
				{
					if (fieldInfo.FwQueryFieldName != null &&
						fwQueryFieldNam == fieldInfo.FwQueryFieldName.ToLower())
					{
						return fieldInfo;
					}
				}
			}

			return null;
		}
	}

	#endregion

	#region PaFieldInfo
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Field")]
	public class PaFieldInfo
	{
		public const string kCustomFieldPrefix = "CustomField";

		private string m_fieldName;
		private string m_displayText;
		private bool m_isRightToLeft;
		private bool m_canBeInterlinear;
		private bool m_isPhonetic;
		private bool m_isPhonemic;
		private bool m_isTone;
		private bool m_isOrtho;
		private bool m_isGloss;
		private bool m_isReference;
		private bool m_isCVPattern;
		private bool m_isDate;
		private string m_fwQueryFieldName;
		private bool m_isFwField;
		private bool m_isDataSource;
		private bool m_isDataSourcePath;
		private bool m_isAudioFile;
		private bool m_isAudioOffset;
		private bool m_isAudioLength;
		private bool m_isCustom;
		private bool m_isParsed;
		private bool m_isNumeric;
		private bool m_isGuid;
		private FwDBUtils.FwWritingSystemType m_fwWs = FwDBUtils.FwWritingSystemType.None;
		private Font m_font = FontHelper.UIFont;
		private string m_saField;
		private bool m_visibleInGrid = true;
		private bool m_visibleInRecVw = true;
		private int m_displayIndexInGrid = -1;
		private int m_displayIndexInRecVw = -1;
		private int m_widthInGrid = -1;

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
			clone.m_fieldName = source.m_fieldName;
			clone.m_displayText = source.m_displayText;
			clone.m_isRightToLeft = source.m_isRightToLeft;
			clone.m_canBeInterlinear = source.m_canBeInterlinear;
			clone.m_isPhonetic = source.m_isPhonetic;
			clone.m_isPhonemic = source.m_isPhonemic;
			clone.m_isOrtho = source.m_isOrtho;
			clone.m_isTone = source.m_isTone;
			clone.m_isGloss = source.m_isGloss;
			clone.m_isReference = source.m_isReference;
			clone.m_isDate = source.m_isDate;
			clone.m_isFwField = source.m_isFwField;
			clone.m_fwQueryFieldName = source.m_fwQueryFieldName;
			clone.m_isDataSource = source.m_isDataSource;
			clone.m_isDataSourcePath = source.m_isDataSourcePath;
			clone.m_isAudioFile = source.m_isAudioFile;
			clone.m_isAudioOffset = source.m_isAudioOffset;
			clone.m_isAudioLength = source.m_isAudioLength;
			clone.m_isCustom = source.m_isCustom;
			clone.m_isParsed = source.m_isParsed;
			clone.m_isNumeric = source.m_isNumeric;
			clone.m_isGuid = source.m_isGuid;
			clone.m_saField = source.m_saField;
			clone.m_visibleInGrid = source.m_visibleInGrid;
			clone.m_visibleInRecVw = source.m_visibleInRecVw;
			clone.m_displayIndexInGrid = source.m_displayIndexInGrid;
			clone.m_displayIndexInRecVw = source.m_displayIndexInRecVw;
			clone.m_widthInGrid = source.m_widthInGrid;
			clone.m_fwWs = source.m_fwWs;
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("Name")]
		public string FieldName
		{
			get { return m_fieldName; }
			set { m_fieldName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DisplayText
		{
			get 
			{
				if (!string.IsNullOrEmpty(m_displayText))
					return m_displayText;

				string text = PaFieldsDisplayText.ResourceManager.GetString("kstid" + m_fieldName);
				return (string.IsNullOrEmpty(text) ? m_fieldName : text);
			}
			set { m_displayText = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Font Font
		{
			get { return (m_font ?? SystemInformation.MenuFont); }
			set { m_font = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDBUtils.FwWritingSystemType FwWritingSystemType
		{
			get { return m_fwWs; }
			set { m_fwWs = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("Font")]
		public SerializableFont SFont
		{
			get { return (m_font == null ? null : new SerializableFont(m_font)); }
			set { if (value != null) m_font = value.Font; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool RightToLeft
		{
			get { return m_isRightToLeft; }
			set { m_isRightToLeft = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool CanBeInterlinear
		{
			get { return m_canBeInterlinear; }
			set { m_canBeInterlinear = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsParsed
		{
			get { return m_isParsed; }
			set { m_isParsed = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsPhonetic
		{
			get { return m_isPhonetic; }
			set { m_isPhonetic = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsPhonemic
		{
			get { return m_isPhonemic; }
			set { m_isPhonemic = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsTone
		{
			get { return m_isTone; }
			set { m_isTone = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsOrtho
		{
			get { return m_isOrtho; }
			set { m_isOrtho = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsGloss
		{
			get { return m_isGloss; }
			set { m_isGloss = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsReference
		{
			get { return m_isReference; }
			set { m_isReference = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCVPattern
		{
			get { return m_isCVPattern; }
			set { m_isCVPattern = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDate
		{
			get { return m_isDate; }
			set { m_isDate = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FwQueryFieldName
		{
			get { return m_fwQueryFieldName; }
			set { m_fwQueryFieldName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDataSource
		{
			get { return m_isDataSource; }
			set { m_isDataSource = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDataSourcePath
		{
			get { return m_isDataSourcePath; }
			set { m_isDataSourcePath = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAudioFile
		{
			get { return m_isAudioFile; }
			set { m_isAudioFile = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAudioOffset
		{
			get { return m_isAudioOffset; }
			set { m_isAudioOffset = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsAudioLength
		{
			get { return m_isAudioLength; }
			set { m_isAudioLength = value; }
		}
			
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsCustom
		{
			get { return m_isCustom; }
			set { m_isCustom = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsNumeric
		{
			get { return m_isNumeric; }
			set { m_isNumeric = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsGuid
		{
			get { return m_isGuid; }
			set { m_isGuid = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///  
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SaFieldName
		{
			get { return m_saField; }
			set { m_saField = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the default grid setup dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool VisibleInGrid
		{
			get { return m_visibleInGrid; }
			set { m_visibleInGrid = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the word list tab in the options
		/// dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool VisibleInRecView
		{
			get { return m_visibleInRecVw; }
			set { m_visibleInRecVw = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the word list tab on the
		/// tools/options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DisplayIndexInGrid
		{
			get { return m_displayIndexInGrid; }
			set { m_displayIndexInGrid = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the record view tab on the
		/// tools/options dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int DisplayIndexInRecView
		{
			get { return m_displayIndexInRecVw; }
			set { m_displayIndexInRecVw = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is not serialized but is set from information read from a project file after
		/// it has been loaded. The user sets this value via the default grid setup dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int WidthInGrid
		{
			get { return m_widthInGrid; }
			set { m_widthInGrid = value; }
		}

		#endregion
	}

	#endregion

	#region FieldLayoutInfo Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FieldLayoutInfo
	{
		[XmlAttribute]
		public string Name;
		[XmlAttribute]
		public bool Visible;
		[XmlAttribute]
		public int Width;
		[XmlAttribute]
		public int DisplayIndex;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a sorted (on DisplayIndex) version of the specified FieldLayoutInfo
		/// collection.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static List<FieldLayoutInfo> GetSortedList(List<FieldLayoutInfo> layoutInfo)
		{
			SortedList<int, FieldLayoutInfo> sortedLayoutInfo = new SortedList<int, FieldLayoutInfo>();

			foreach (FieldLayoutInfo fli in layoutInfo)
				sortedLayoutInfo[fli.DisplayIndex] = fli;

			List<FieldLayoutInfo> newLayoutInfo = new List<FieldLayoutInfo>();
			foreach (FieldLayoutInfo fli in sortedLayoutInfo.Values)
				newLayoutInfo.Add(fli);

			return newLayoutInfo;
		}
	}

	#endregion
}
