using System.Xml.Serialization;

namespace SIL.Pa.Model
{
	#region Feature
	/// ----------------------------------------------------------------------------------------
	[XmlType("feature")]
	public class Feature
	{
		protected string m_fullname;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones the specified binary feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Feature Clone()
		{
			var clone = new Feature();
			clone.Bit = Bit;
			clone.Name = Name;
			clone.m_fullname = m_fullname;
			clone.Class = Class;
			clone.SubClass = SubClass;
			clone.FeatureType = FeatureType;
			clone.Clements = Clements;
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the full name without returning the Name when full name is null or emtpy.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetBaseFullName()
		{
			return m_fullname;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FullName + " (bit: " + Bit + ")";
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("class")]
		public string Class { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("subclass")]
		public string SubClass { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("type")]
		public string FeatureType { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("clements")]
		public string Clements { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("name")]
		public string Name { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlElement("fullname")]
		public string FullName
		{
			get { return (string.IsNullOrEmpty(m_fullname) ? Name : m_fullname); }
			set { m_fullname = value; }
		}
	
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Bit { get; protected internal set; }

		#endregion
	}

	#endregion
}
