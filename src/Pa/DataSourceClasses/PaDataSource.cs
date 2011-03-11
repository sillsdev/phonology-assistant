using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.DataSource.Sa;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.DataSource
{
	/// ----------------------------------------------------------------------------------------
	public enum DataSourceType
	{
		PAXML,
		FW,				// This is for FW6 or older data sources.
		FW7,			// This is for FW7 and newer data sources.
		SA,
		SFM,
		Toolbox,
		XML,
		LIFT,
		Unknown
	}

	/// ----------------------------------------------------------------------------------------
	public enum DataSourceParseType
	{
		PhoneticOnly,
		None,
		OneToOne,
		Interlinear
	}

	/// ----------------------------------------------------------------------------------------
	[XmlType("DataSource")]
	public class PaDataSource
	{
		public const string kRecordMarker = "RecMrkr";
		public const string kShoeboxMarker = "\\_sh ";

		private DataSourceType m_type;
		private FwDataSourceInfo m_fwDataSourceInfo;
		private string m_dataSourceFile;
		private IEnumerable<string> m_markersInFile;

		/// ------------------------------------------------------------------------------------
		public PaDataSource()
		{
			Type = DataSourceType.Unknown;
			ParseType = DataSourceParseType.PhoneticOnly;
			TotalLinesInFile = 1;
			SkipLoading = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a FieldWorks data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaDataSource(IEnumerable<PaField> projectFields, FwDataSourceInfo fwDbItem) : this()
		{
			FwDataSourceInfo = fwDbItem;
			Type = fwDbItem.DataSourceType;
			FwPrjName = FwDataSourceInfo.Name;
			FieldMappings = (Type == DataSourceType.FW7 ?
				CreateDefaultFw7Mappings(projectFields) : CreateDefaultFwMappings(projectFields)).ToList();
		}

		/// ------------------------------------------------------------------------------------
		public PaDataSource(IEnumerable<PaField> fields, string filename) : this()
		{
			SourceFile = filename.Trim();

			if (SourceFile.ToLower().EndsWith(".wav") /* || m_file.ToLower().EndsWith(".mp3") ||
				m_file.ToLower().EndsWith(".wma") */)
			{
				Type = DataSourceType.SA;
			}
			else if (!GetIsXmlFile(SourceFile))
			{
				bool isShoeboxFile;
				if (GetIsSfmFile(SourceFile, out isShoeboxFile))
				{
					Type = (isShoeboxFile ? DataSourceType.Toolbox : DataSourceType.SFM);
					FieldMappings = CreateDefaultSfmMappings(fields).ToList();
					SfmRecordMarker = Settings.Default.DefaultSfmRecordMarker.Split(';')
						.SingleOrDefault(mkr => GetSfMarkers(false).Contains(mkr));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a deep copy of the data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaDataSource Copy()
		{
			return new PaDataSource
			{
				SourceFile = SourceFile,
				Type = Type,
				ParseType = ParseType,
				FirstInterlinearField = FirstInterlinearField,
				Editor = Editor,
				FwPrjName = FwPrjName,
				LastModification = LastModification,
				SfmRecordMarker = SfmRecordMarker,
				SkipLoadingBecauseOfProblem = SkipLoadingBecauseOfProblem,
				SkipLoading = SkipLoading,
				ToolboxSortField = ToolboxSortField,
				TotalLinesInFile = TotalLinesInFile,
				XSLTFile = XSLTFile,
				FwDataSourceInfo = (FwDataSourceInfo == null ? null : FwDataSourceInfo.Copy()),
				FieldMappings = (FieldMappings == null ? null : FieldMappings.Select(m => m.Copy()).ToList()),
			};
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<FieldMapping> CreateDefaultFwMappings(IEnumerable<PaField> projectFields)
		{
			return null;
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<FieldMapping> CreateDefaultFw7Mappings(IEnumerable<PaField> projectFields)
		{
			var writingSystems = FwDataSourceInfo.GetWritingSystems();
			var defaultFieldNames = Settings.Default.DefaultMappedFw7Fields.Cast<string>()
				.Where(n => n != PaField.kAudioFileFieldName && n != PaField.kPhoneticFieldName).ToList();

			var mappings = new List<FieldMapping>();

			// Add a mapping for the phonetic field.
			var field = projectFields.Single(f => f.Type == FieldType.Phonetic);
			mappings.Add(new FieldMapping(field.Name, field, true)
				{ FwWsId = FwDBUtils.GetDefaultPhoneticWritingSystem(writingSystems).Id });

			// Add a mapping for the audio file field.
			field = projectFields.Single(f => f.Type == FieldType.AudioFilePath);
			mappings.Add(new FieldMapping(field.Name, field, false));

			mappings.AddRange(projectFields.Where(f => defaultFieldNames.Contains(f.Name)).Select(f =>
			{
				var mapping = new FieldMapping(f.Name, f, Settings.Default.ParsedFw7Fields.Contains(f.Name));
				FieldMapping.CheckMappingsFw7WritingSystem(mapping, writingSystems);
				return mapping;
			}));

			return mappings;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<FieldMapping> CreateDefaultSfmMappings(IEnumerable<PaField> fields)
		{
			var defaultParsedFlds = Settings.Default.DefaultParsedSfmFields;

			return (from mkr in GetSfMarkers(true)
					let field = fields.SingleOrDefault(f => f.GetPossibleDataSourceFieldNames().Contains(mkr))
					where field != null
					orderby mkr
					select new FieldMapping(mkr, field, defaultParsedFlds.Contains(field.Name)));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the datasource is an SFM file, this will return a list of all unique
		/// markers (e.g., "\name") in a file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetSfMarkers(bool showMsgOnError)
		{
			if (m_markersInFile != null && m_markersInFile.Count() > 0)
				return m_markersInFile;

			try
			{
				var allLines = File.ReadAllLines(SourceFile);
				TotalLinesInFile = allLines.Length;

				// Go through all lines that start with backslashes, excluding
				// the ones used to identify a Shoebox/Toolbox file.
				m_markersInFile = (from line in allLines
								   where line.StartsWith("\\") && !line.StartsWith("\\_Date") && !line.StartsWith(kShoeboxMarker)
								   select line.Split(' ')[0]).Distinct().ToList();
			}
			catch (Exception e)
			{
				if (showMsgOnError)
				{
					var msg = App.LocalizeString("ErrorReadingMarkersFromStandardFormatFileMsg",
						"The following error occurred trying to read the source file '{0}'.\n\n{1}",
						App.kLocalizationGroupDialogs);

					Utils.MsgBox(string.Format(msg, e.Message));
				}
			}

			return m_markersInFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will go through the data source's field mappings and point each one to the
		/// PaField to which it's mapped. It will also verify that each mapping really has
		/// a PaField in the specified collection. Whenever a mapping is found that doesn't
		/// have a corresponding PaField to which it's mapped, it's assumed it's a custom
		/// field and therefore a new PaField is created and added to a collection of fields
		/// returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<PaField> PostDeserializeInitialization(IEnumerable<PaField> fields)
		{
			var customFields = new List<PaField>();

			if (FieldMappings == null)
				return customFields;

			foreach (var mapping in FieldMappings.Where(m => m.PaFieldName != null))
			{
				var field = fields.SingleOrDefault(f => f.Name == mapping.PaFieldName);
				if (field == null)
				{
					field = new PaField(mapping.PaFieldName);
					customFields.Add(field);
				}

				mapping.Field = field;
				mapping.PaFieldName = null;
			}

			return customFields;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks to see if the data source should have mappings (i.e. for SFM and Toolbox
		/// data source types). If so, then makes sure the collection of mappings is complete.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void VerifyMappings(PaProject project)
		{
			// TODO: Fix for new system.
			//if (Type != DataSourceType.Toolbox && Type != DataSourceType.SFM &&
			//    SFMappings != null)
			//{
			//    SFMappings.Clear();
			//    return;
			//}

			//if (project == null)
			//    return;

			//if (SFMappings == null || SFMappings.Count == 0)
			//{
			//    MakeNewMappingsList();
			//    return;
			//}

			//// Verify that each field in the project has an item in the mappings collection.
			//foreach (PaFieldInfo fieldinfo in project.FieldInfo)
			//    SFMarkerMapping.VerifyMappingForField(SFMappings, fieldinfo);

			//// Now make sure a mapping exists for the record marker.
			//if (!SFMappings.Any(m => m.FieldName == kRecordMarker))
			//    SFMappings.Add(new SFMarkerMapping(kRecordMarker));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified file is an SFM file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetIsSfmFile(string filename, out bool isShoeboxFile)
		{
			var allLines = File.ReadAllLines(filename);
			TotalLinesInFile = allLines.Length;

			if (TotalLinesInFile > 0 && allLines[0].StartsWith(kShoeboxMarker))
			{
				isShoeboxFile = true;
				return true;
			}

			isShoeboxFile = false;
			int linesBeginningWithBackslash = allLines.Count(l => l != null && l.TrimStart().StartsWith("\\"));

			// Assume that it's an SFM file if at least 60% of the lines begin with a backslash
			return (((float)linesBeginningWithBackslash / TotalLinesInFile) >= 0.60);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified data source matches this data source. A
		/// match is determined by the data source types and their file names (or project
		/// name in the case of FW data sources).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Matches(PaDataSource ds, bool treatToolboxAsSFM)
		{
			bool typesMatch = (Type == ds.Type);

			if (!typesMatch && treatToolboxAsSFM)
			{
				// Determine if the TOOLBOX/SFM types match.
				typesMatch = ((Type == DataSourceType.Toolbox ||
					Type == DataSourceType.SFM) &&
					(ds.Type == DataSourceType.Toolbox ||
					ds.Type == DataSourceType.SFM));
			}

			if (!typesMatch)
				return false;

			if (Type == DataSourceType.FW || Type == DataSourceType.FW7)
			{
				return (FwDataSourceInfo != null && ds.FwDataSourceInfo != null &&
					FwDataSourceInfo.Name == ds.FwDataSourceInfo.Name &&
					FwDataSourceInfo.Server == ds.FwDataSourceInfo.Server);
			}

			return (SourceFile.ToLower() == ds.SourceFile.ToLower());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified data source file is an xml file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool GetIsXmlFile(string filename)
		{
			XmlDocument xmldoc = new XmlDocument();
			try
			{
				xmldoc.Load(filename);
			}
			catch
			{
				return false;
			}

			// At this point, we know we have a valid XML file. Now check if it's PAXML.
			var paxmlContent = XmlSerializationHelper.DeserializeFromFile<PaXMLContent>(filename);

			if (paxmlContent == null)
				Type = DataSourceType.XML;
			else
			{
				// We know we have a PAXML file. Now check if it was written by FieldWorks.
				string fwServer;
				string fwDBName;
				Type = GetPaXmlType(filename, out fwServer, out fwDBName);
				TotalLinesInFile = paxmlContent.Cache.Count;
				paxmlContent.Cache.Clear();
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// By the time this method is called, we know the specified file is in PAXML format.
		/// This method will determine whether or not the PAXML format is from FieldWorks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DataSourceType GetPaXmlType(string filename, out string fwServer,
			out string fwDBname)
		{
			fwServer = null;
			fwDBname = null;
			string sourceType = null;

			XmlDocument xmldoc = new XmlDocument();
			xmldoc.Load(filename);

			if (xmldoc.DocumentElement.Attributes["source"] != null)
				sourceType = xmldoc.DocumentElement.Attributes["source"].Value;
	
			if (xmldoc.DocumentElement.Attributes["silfwserver"] != null)
				fwServer = xmldoc.DocumentElement.Attributes["silfwserver"].Value;
			
			if (xmldoc.DocumentElement.Attributes["database"] != null)
				fwDBname = xmldoc.DocumentElement.Attributes["database"].Value;

			DataSourceType type = DataSourceType.PAXML;
			
			try
			{
				type = (DataSourceType)Enum.Parse(typeof(DataSourceType), sourceType, true);
			}
			catch {}

			return type;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the mappings for an SFM data source and changes field names in those
		/// collections.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RenameField(string origName, string newName)
		{
			//// If the data source is an SFM or Toolbox data source then rename the fields
			//// in the mappings collection.
			//if (SFMappings != null)
			//{
			//    foreach (var mapping in SFMappings.Where(m => m.FieldName == origName))
			//        mapping.FieldName = newName;
			//}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the data source file. (The setter is only for XML deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("DataSourceFile")]
		public string SourceFile
		{
			get
			{
				return (m_fwDataSourceInfo != null && m_type == DataSourceType.FW7 ?
					m_fwDataSourceInfo.Name : m_dataSourceFile);
			}
			set
			{
				if (m_fwDataSourceInfo == null || m_type != DataSourceType.FW7)
					m_dataSourceFile = value;

				if (LastModification == default(DateTime) && File.Exists(m_dataSourceFile))
					LastModification = File.GetLastWriteTimeUtc(m_dataSourceFile);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value determining how Toolbox/Shoebox (or SFM) files are parsed.
		/// This value is only relevant for data sources whose type is SFM or Toolbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataSourceParseType ParseType { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the type of the data source. (The setter is only for XML deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataSourceType Type
		{
			get { return m_type; }
			set
			{
				m_type = value;
				SetFwDataSourceInfoType();
			}
		}

		/// ------------------------------------------------------------------------------------
		public string SfmRecordMarker { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the FieldWorks data source info. object. The setter for this is only for
		/// deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo FwDataSourceInfo
		{
			get { return m_fwDataSourceInfo; }
			set
			{
				m_fwDataSourceInfo = value;
				SetFwDataSourceInfoType();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetFwDataSourceInfoType()
		{
			if (m_fwDataSourceInfo != null &&
				(m_type == DataSourceType.FW || m_type == DataSourceType.FW7))
			{
 				m_fwDataSourceInfo.DataSourceType = m_type;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a flag indicating whether or not a FW data source's data should come
		/// directly from the database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool FwSourceDirectFromDB
		{
			get { return FwDataSourceInfo != null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the SIL FieldWorks database name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FwPrjName { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a data source file should be
		/// loaded. This only gets set to true when the user first opens a project with one or
		/// more data source files that cannot be found and the user chooses to skip loading
		/// those for the current instance of PA.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool SkipLoadingBecauseOfProblem { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a data source file should be
		/// loaded. This is a persisted value the user can control on the project settings
		/// dialog box. It allows the user to keep a data source in the project, but
		/// temporarily prevent it from being loaded.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SkipLoading { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string TypeAsString
		{
			get { return Type.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		public string XSLTFile { get; set; }

		/// ------------------------------------------------------------------------------------
		public string FirstInterlinearField { get; set; }

		/// ------------------------------------------------------------------------------------
		public int TotalLinesInFile { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The field whose value is written to the registry for Toolbox to pick up when PA
		/// tells Toolbox to jump to a record containing the field's value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ToolboxSortField { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the full path and file name of the program the user has specified
		/// as being the editor for the record source. This property is only relevant for
		/// SFM data sources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Editor { get; set; }

		/// ------------------------------------------------------------------------------------
		public List<FieldMapping> FieldMappings { get; set; }

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the interlinear fields for the SFM/Toolbox data source.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlIgnore]
		//public string[] InterlinearFields
		//{
		//    get
		//    {
		//        if (string.IsNullOrEmpty(FirstInterlinearField) ||
		//            (Type != DataSourceType.SFM && Type != DataSourceType.Toolbox))
		//        {
		//            return null;
		//        }

		//        var interlinearFields = 
		//            (from mapping in SFMappings where mapping.IsInterlinear select mapping.FieldName).ToArray();

		//        return (interlinearFields.Length == 0 ? null : interlinearFields);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public DateTime LastModification { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool IsSfmType
		{
			get { return Type == DataSourceType.SFM || Type == DataSourceType.Toolbox; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not mappings exist for a data source of
		/// type SFM or Toolbox. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool MappingsExist
		{
			get
			{
				return true;
				//if (SFMappings == null)
				//    return false;

				//// Go through the mappings to determine how to set the MappingsExist flag.
				//int count = 0;
				//foreach (SFMarkerMapping mapping in SFMappings)
				//{
				//    if (mapping.FieldName == kRecordMarker && string.IsNullOrEmpty(mapping.Marker))
				//        count = -999;
				//    else
				//        count += (string.IsNullOrEmpty(mapping.Marker) ? 0 : 1);
				//}

				//// At least two mappings should include one
				//// for the record marker and any other one.
				//return (count >= 2);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the date/time the data source was last modified. The return value is a
		/// flag indicating whether or not the new value is different from the previous.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool UpdateLastModifiedTime()
		{
			if (Type == DataSourceType.FW && FwSourceDirectFromDB)
				return m_fwDataSourceInfo.UpdateLastModifiedTime();

			// We don't have a way to get the last modified time for server-based projects.
			// TODO: Figure out how to deal with getting modifed time for server-based projects.
			if (Type == DataSourceType.FW7 && m_fwDataSourceInfo.IsMultiAccessProject)
				return false;

			var latestModification = (Type == DataSourceType.SA ?
				SaAudioDocument.GetTranscriptionFileModifiedTime(SourceFile) :
				File.GetLastWriteTimeUtc(SourceFile));

			if (latestModification <= LastModification)
				return false;

			LastModification = latestModification;
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's the data source's file name or database name when the data source is 
		/// FieldWorks data direct from a FW database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return ToString(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get's the data source's file name or database name when the data source is 
		/// FieldWorks data direct from a FW database.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ToString(bool showServerForFwDataSource)
		{
			if (Type == DataSourceType.FW || Type == DataSourceType.FW7)
				return FwDataSourceInfo.ToString(showServerForFwDataSource);
		
			return SourceFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used to display the data source when progress is displayed while reading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DisplayTextWhenReading
		{
			get
			{
				return (FwSourceDirectFromDB && FwDataSourceInfo != null ?
					FwDataSourceInfo.ProjectName : Path.GetFileName(SourceFile));
			}
		}
	}
}
