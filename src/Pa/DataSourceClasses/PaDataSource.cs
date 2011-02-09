using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.DataSource.Sa;
using SIL.Pa.Model;
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

		private DataSourceType m_dataSourceType;
		private FwDataSourceInfo m_fwDataSourceInfo;
		private string m_dataSourceFile;

		/// ------------------------------------------------------------------------------------
		public PaDataSource()
		{
			DataSourceType = DataSourceType.Unknown;
			ParseType = DataSourceParseType.PhoneticOnly;
			TotalLinesInFile = 1;
			SkipLoading = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a FieldWorks data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaDataSource(FwDataSourceInfo fwDbItem) : this()
		{
			FwDataSourceInfo = fwDbItem;
			DataSourceType = fwDbItem.DataSourceType;
			FwPrjName = FwDataSourceInfo.Name;
		}

		/// ------------------------------------------------------------------------------------
		public PaDataSource(string filename) : this()
		{
			DataSourceFile = filename.Trim();

			if (DataSourceFile.ToLower().EndsWith(".wav") /* || m_file.ToLower().EndsWith(".mp3") ||
				m_file.ToLower().EndsWith(".wma") */)
			{
				DataSourceType = DataSourceType.SA;
			}
			else if (!IsXMLFile(DataSourceFile))
			{
				bool isShoeboxFile;
				if (IsSFMFile(DataSourceFile, out isShoeboxFile))
					DataSourceType = (isShoeboxFile ? DataSourceType.Toolbox : DataSourceType.SFM);
			}
			
			if (DataSourceType == DataSourceType.SFM || DataSourceType == DataSourceType.Toolbox)
				MakeNewMappingsList();
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
			bool typesMatch = (DataSourceType == ds.DataSourceType);

			if (!typesMatch && treatToolboxAsSFM)
			{
				// Determine if the TOOLBOX/SFM types match.
				typesMatch = ((DataSourceType == DataSourceType.Toolbox ||
					DataSourceType == DataSourceType.SFM) &&
					(ds.DataSourceType == DataSourceType.Toolbox ||
					ds.DataSourceType == DataSourceType.SFM));
			}

			if (!typesMatch)
				return false;

			if (DataSourceType == DataSourceType.FW || DataSourceType == DataSourceType.FW7)
			{
				return (FwDataSourceInfo != null && ds.FwDataSourceInfo != null &&
					FwDataSourceInfo.Name == ds.FwDataSourceInfo.Name &&
					FwDataSourceInfo.Server == ds.FwDataSourceInfo.Server);
			}
			
			return (DataSourceFile.ToLower() == ds.DataSourceFile.ToLower());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a list of mappings for an SFM or Toolbox data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void MakeNewMappingsList()
		{
			// Add a mapping for each PA field in the specified PaFieldInfoList. For each new
			// mapping, check if there is a mapping for the same field in the specified
			// default mappings list. If there is, then assign the new mapping's marker and
			// interlinear flag to that of the one found in the default mapping list.
			SFMappings = PaFieldInfoList.DefaultFieldInfoList.Select(fi => new SFMarkerMapping(fi)).ToList();
			SFMappings.Insert(0, new SFMarkerMapping(kRecordMarker));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks to see if the data source should have mappings (i.e. for SFM and Toolbox
		/// data source types). If so, then makes sure the collection of mappings is complete.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void VerifyMappings(PaProject project)
		{
			if (DataSourceType != DataSourceType.Toolbox && DataSourceType != DataSourceType.SFM &&
				SFMappings != null)
			{
				SFMappings.Clear();
				return;
			}

			if (project == null)
				return;

			if (SFMappings == null || SFMappings.Count == 0)
			{
				MakeNewMappingsList();
				return;
			}

			// Verify that each field in the project has an item in the mappings collection.
			foreach (PaFieldInfo fieldinfo in project.FieldInfo)
				SFMarkerMapping.VerifyMappingForField(SFMappings, fieldinfo);

			// Now make sure a mapping exists for the record marker.
			if (!SFMappings.Any(m => m.FieldName == kRecordMarker))
				SFMappings.Add(new SFMarkerMapping(kRecordMarker));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not a file is an SFM file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsSFMFile(string filename, out bool isShoeboxFile)
		{
			using (StreamReader reader = new StreamReader(filename))
			{
				isShoeboxFile = false;
				int linesBeginningWithBackslash = 0;

				// Check if the file is a shoebox file.
				var line = reader.ReadLine();
				if (line != null && line.StartsWith(kShoeboxMarker))
				{
					isShoeboxFile = true;
					return true;
				}

				do
				{
					if (line != null && line.Trim() != string.Empty)
					{
						TotalLinesInFile++;
						if (line.StartsWith("\\"))
							linesBeginningWithBackslash++;
					}
				} while ((line = reader.ReadLine()) != null);

				reader.Close();

				// Assume that it's an SFM file if at least 60% of the lines begin with a backslash
				return (((float)linesBeginningWithBackslash / TotalLinesInFile) >= 0.60);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not the specified data source file is an xml file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsXMLFile(string filename)
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
				DataSourceType = DataSourceType.XML;
			else
			{
				// We know we have a PAXML file. Now check if it was written by FieldWorks.
				string fwServer;
				string fwDBName;
				DataSourceType = GetPaXMLType(filename, out fwServer, out fwDBName);
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
		public static DataSourceType GetPaXMLType(string filename, out string fwServer,
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
			// If the data source is an SFM or Toolbox data source then rename the fields
			// in the mappings collection.
			if (SFMappings != null)
			{
				foreach (var mapping in SFMappings.Where(m => m.FieldName == origName))
					mapping.FieldName = newName;
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the data source file. (The setter is only for XML deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DataSourceFile
		{
			get
			{
				return (m_fwDataSourceInfo != null && m_dataSourceType == DataSourceType.FW7 ?
					m_fwDataSourceInfo.Name : m_dataSourceFile);
			}
			set
			{
				if (m_fwDataSourceInfo == null || m_dataSourceType != DataSourceType.FW7)
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
		public DataSourceType DataSourceType
		{
			get { return m_dataSourceType; }
			set
			{
				m_dataSourceType = value;
				SetFwDataSourceInfoType();
			}
		}

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
				(m_dataSourceType == DataSourceType.FW || m_dataSourceType == DataSourceType.FW7))
			{
 				m_fwDataSourceInfo.DataSourceType = m_dataSourceType;
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
		public string DataSourceTypeString
		{
			get { return DataSourceType.ToString(); }
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
		public List<SFMarkerMapping> SFMappings { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the interlinear fields for the SFM/Toolbox data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] InterlinearFields
		{
			get
			{
				if (string.IsNullOrEmpty(FirstInterlinearField) ||
					(DataSourceType != DataSourceType.SFM && DataSourceType != DataSourceType.Toolbox))
				{
					return null;
				}

				var interlinearFields =
					(from mapping in SFMappings where mapping.IsInterlinear select mapping.FieldName).ToArray();

				return (interlinearFields.Length == 0 ? null : interlinearFields);
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public DateTime LastModification { get; set; }

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
				if (SFMappings == null)
					return false;

				// Go through the mappings to determine how to set the MappingsExist flag.
				int count = 0;
				foreach (SFMarkerMapping mapping in SFMappings)
				{
					if (mapping.FieldName == kRecordMarker && string.IsNullOrEmpty(mapping.Marker))
						count = -999;
					else
						count += (string.IsNullOrEmpty(mapping.Marker) ? 0 : 1);
				}

				// At least two mappings should include one
				// for the record marker and any other one.
				return (count >= 2);
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
			if (DataSourceType == DataSourceType.FW && FwSourceDirectFromDB)
				return m_fwDataSourceInfo.UpdateLastModifiedTime();

			// We don't have a way to get the last modified time for server-based projects.
			// TODO: Figure out how to deal with getting modifed time for server-based projects.
			if (DataSourceType == DataSourceType.FW7 && m_fwDataSourceInfo.IsMultiAccessProject)
				return false;

			var latestModification = (DataSourceType == DataSourceType.SA ?
				SaAudioDocument.GetTranscriptionFileModifiedTime(DataSourceFile) :
				File.GetLastWriteTimeUtc(DataSourceFile));

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
			if (DataSourceType == DataSourceType.FW || DataSourceType == DataSourceType.FW7)
				return FwDataSourceInfo.ToString(showServerForFwDataSource);
		
			return DataSourceFile;
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
					FwDataSourceInfo.ProjectName : Path.GetFileName(DataSourceFile));
			}
		}
	}
}
