using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum DataSourceType
	{
		PAXML,
		FW,
		SA,
		SFM,
		Toolbox,
		XML,
		Unknown
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public enum DataSourceParseType
	{
		PhoneticOnly,
		None,
		OneToOne,
		Interlinear
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("DataSource")]
	public class PaDataSource
	{
		public const string kRecordMarker = "RecMrkr";
		public const string kShoeboxMarker = "\\_sh ";
		private string m_file;
		private string m_xsltFile;
		private string m_firstInterlinearField = null;
		private DateTime m_lastModification = DateTime.MinValue;
		private int m_linesInFile = 1;
		private DataSourceType m_sourceType = DataSourceType.Unknown;
		private List<SFMarkerMapping> m_mappings;
		private bool m_skipLoading = false;
		private DataSourceParseType m_parseType = DataSourceParseType.PhoneticOnly;
		private string m_fwServer;
		private string m_fwDBName;
		private FwDataSourceInfo m_fwSourceInfo = null;
		private string m_toolboxSortField;
		private string m_editor = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaDataSource()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a FieldWorks data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaDataSource(FwDataSourceInfo fwDbItem)
		{
			m_fwSourceInfo = fwDbItem;
			m_sourceType = DataSourceType.FW;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaDataSource(string filename)
		{
			m_file = filename.Trim();

			if (m_file.ToLower().EndsWith(".wav") /* || m_file.ToLower().EndsWith(".mp3") ||
				m_file.ToLower().EndsWith(".wma") */)
			{
				m_sourceType = DataSourceType.SA;
			}
			else if (!IsXMLFile(m_file))
			{
				bool isShoeboxFile;
				if (IsSFMFile(m_file, out isShoeboxFile))
					m_sourceType = (isShoeboxFile ? DataSourceType.Toolbox : DataSourceType.SFM);
			}

			if (m_sourceType != DataSourceType.Unknown)
				m_lastModification = File.GetLastWriteTimeUtc(filename);
			
			if (m_sourceType == DataSourceType.SFM || m_sourceType == DataSourceType.Toolbox)
				MakeNewMappingsList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Builds a list of mappings for an SFM or Toolbox data source.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void MakeNewMappingsList()
		{
			m_mappings = new List<SFMarkerMapping>();
			m_mappings.Add(new SFMarkerMapping(PaDataSource.kRecordMarker));

			// Add a mapping for each PA field in the specified PaFieldInfoList. For each new
			// mapping, check if there is a mapping for the same field in the specified
			// default mappings list. If there is, then assign the new mapping's marker and
			// interlinear flag to that of the one found in the default mapping list.
			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
			{
				SFMarkerMapping newMapping = new SFMarkerMapping(fieldInfo);
				m_mappings.Add(newMapping);
			}
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
				string line;
				int linesBeginningWithBackslash = 0;

				// Check if the file is a shoebox file.
				line = reader.ReadLine();
				if (line != null && line.StartsWith(kShoeboxMarker))
				{
					isShoeboxFile = true;
					return true;
				}

				do
				{
					if (line.Trim() != string.Empty)
					{
						m_linesInFile++;
						if (line.StartsWith("\\"))
							linesBeginningWithBackslash++;
					}
				} while ((line = reader.ReadLine()) != null);

				reader.Close();

				// Assume that it's an SFM file if at least 60% of the lines begin with a backslash
				return (((float)linesBeginningWithBackslash / (float)m_linesInFile) >= 0.60);
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
			RecordCache cache =
				STUtils.DeserializeData(filename, typeof(RecordCache)) as RecordCache;

			if (cache == null)
				m_sourceType = DataSourceType.XML;
			else
			{
				// We know we have a PAXML file. Now check if it was written by FieldWorks.
				string fwServer;
				string fwDBName;
				m_sourceType = GetPaXMLType(filename, out fwServer, out fwDBName);				
				m_linesInFile = cache.Count;
				cache.Clear();
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
		/// Get's the data source's file name or database name when the data source is 
		/// FieldWorks data direct from a FW database.
		/// </summary>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (m_sourceType == DataSourceType.FW && m_fwSourceInfo != null ?
				  m_fwSourceInfo.ToString() : DataSourceFile);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the data source file. (The setter is only for XML deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string DataSourceFile
		{
			get { return m_file; }
			set	{ m_file = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the type of the data source. (The setter is only for XML deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataSourceType DataSourceType
		{
			get { return m_sourceType; }
			set { m_sourceType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value determining how Toolbox/Shoebox (or SFM) files are parsed.
		/// This value is only relevant for data sources whose type is SFM or Toolbox.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DataSourceParseType ParseType
		{
			get { return m_parseType; }
			set { m_parseType = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the FW data source info. object. The setter for this is only for
		/// deserialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourceInfo FwDataSourceInfo
		{
			get { return m_fwSourceInfo; }
			set { m_fwSourceInfo = value; }
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
			get { return m_fwSourceInfo != null; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the SIL FieldWorks server name. This is set only when an FW data
		/// source is read in DataSourceReader.cs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FwServer
		{
			get { return m_fwSourceInfo != null ? m_fwSourceInfo.Server : m_fwServer; }
			set { m_fwServer = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the SIL FieldWorks database name. This is set only when an FW data
		/// source is read in DataSourceReader.cs.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FwDBName
		{
			get { return (m_fwSourceInfo == null ? m_fwDBName : m_fwSourceInfo.DBName); }
			set { m_fwDBName = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not a data source file should be
		/// loaded. This only gets set to true when the user first opens a project with one or
		/// more data source files that cannot be found and the user chooses to skip loading
		/// those for the current instance of PA. This value is not persisted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool SkipLoading
		{
			get { return m_skipLoading; }
			set { m_skipLoading = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string DataSourceTypeString
		{
			get { return m_sourceType.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string XSLTFile
		{
			get { return m_xsltFile; }
			set { m_xsltFile = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FirstInterlinearField
		{
			get { return m_firstInterlinearField; }
			set { m_firstInterlinearField = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int TotalLinesInFile
		{
			get { return m_linesInFile; }
			set { m_linesInFile = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The field whose value is written to the registry for Toolbox to pick up when PA
		/// tells Toolbox to jump to a record containing the field's value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string ToolboxSortField
		{
			get { return m_toolboxSortField; }
			set { m_toolboxSortField = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the full path and file name of the program the user has specified
		/// as being the editor for the record source. This property is only relevant for
		/// SFM data sources.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Editor
		{
			get { return m_editor; }
			set { m_editor = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<SFMarkerMapping> SFMappings
		{
			get { return m_mappings; }
			set { m_mappings = value; }
		}

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
				if (string.IsNullOrEmpty(m_firstInterlinearField) ||
					(m_sourceType != DataSourceType.SFM && m_sourceType != DataSourceType.Toolbox))
				{
					return null;
				}

				List<string> interlinearFields = new List<string>();
				foreach (SFMarkerMapping mapping in m_mappings)
				{
					if (mapping.IsInterlinear)
						interlinearFields.Add(mapping.FieldName);
				}

				return (interlinearFields.Count == 0 ? null : interlinearFields.ToArray());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public DateTime LastModification
		{
			get { return m_lastModification; }
			set { m_lastModification = value; }
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
				if (m_mappings == null)
					return false;

				// Go through the mappings to determine how to set the MappingsExist flag.
				int count = 0;
				foreach (SFMarkerMapping mapping in m_mappings)
				{
					if (mapping.FieldName == PaDataSource.kRecordMarker && string.IsNullOrEmpty(mapping.Marker))
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
	}
}
