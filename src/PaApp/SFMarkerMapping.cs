using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Defines a class used to store mappings from SF markers to PA fields.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("Mapping")]
	public class SFMarkerMapping
	{
		private static readonly string s_noneText = Properties.Resources.kstidDropDownNoneEntry;
		private readonly PaFieldInfo m_fieldInfo;
		private string m_fieldName = string.Empty;
		private string m_marker = string.Empty;
		private bool m_isInterlinear = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SFMarkerMapping()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SFMarkerMapping(string fieldName)
		{
			m_fieldName = fieldName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SFMarkerMapping(PaFieldInfo fieldInfo)
		{
			m_fieldInfo = fieldInfo;
			
			if (m_fieldInfo != null)
				m_fieldName = m_fieldInfo.FieldName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes a copies of the instance of SFMarkerMapping
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SFMarkerMapping Clone()
		{
			SFMarkerMapping clone = new SFMarkerMapping(m_fieldInfo);
			clone.m_fieldName = m_fieldName;
			clone.m_marker = m_marker;
			clone.m_isInterlinear = m_isInterlinear;
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks the specified mapping list for the specified PA field. If a mapping for the
		/// field cannot be found, then one is created and added to the specified mappings
		/// list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static SFMarkerMapping VerifyMappingForField(List<SFMarkerMapping> mappingList,
			PaFieldInfo fieldInfo)
		{
			foreach (SFMarkerMapping mapping in mappingList)
			{
				if (mapping.FieldName == fieldInfo.FieldName)
					return null;
			}
			
			// At this point, we know we didn't find the mapping
			SFMarkerMapping newMapping = new SFMarkerMapping(fieldInfo);
			mappingList.Add(newMapping);
			return newMapping;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the PA field the marker is mapped to. (The setter is for deserialization.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string FieldName
		{
			get {return m_fieldName;}
			set {m_fieldName = value;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets marker assigned to the PA field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Marker
		{
			get { return m_marker; }
			set
			{
				if (value == null)
					m_marker = string.Empty;
				else
					m_marker = (value == s_noneText ? string.Empty : value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the mapping represents an
		/// interlinearized field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public bool IsInterlinear
		{
			get { return m_isInterlinear; }
			set { m_isInterlinear = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the displayable value of the field a marker is mapped to.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string DisplayText
		{
			get
			{
				if (m_fieldInfo != null)
					return m_fieldInfo.DisplayText;

				return (m_fieldName == PaDataSource.kRecordMarker ?
					Properties.Resources.kstidRecordMarkerFieldDisplayText :
					PaApp.FieldInfo[m_fieldName].DisplayText);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used for the displayed text in a grid combo box of the import dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string MarkerComboBoxDisplayText
		{
			get	{return (m_marker == string.Empty ? s_noneText : m_marker);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used as the DataPropertyName for the SFM import mapping grid on the import dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public Image MapToSymbol
		{
			get
			{
				return (m_fieldName == PaDataSource.kRecordMarker ?
					Properties.Resources.kimidMarkerMapEqualRecID :
					Properties.Resources.kimidMarkerMapToSymbol);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the localized version of "<none>"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public static string NoneText
		{
			get {return s_noneText;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the localized version of "<none>"
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return DisplayText;
		}
	}
}
