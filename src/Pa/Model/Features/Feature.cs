// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SIL.Pa.Model
{
	#region Feature
	/// ----------------------------------------------------------------------------------------
	[XmlType("featureDefinition")]
	public class Feature
	{
		protected string m_fullname;

		/// ------------------------------------------------------------------------------------
		public static Feature FromXElement(XElement element)
		{
			var feature = new Feature();

			feature.Name = element.Element("name").Value;
			
			if (element.Element("fullname") != null)
				feature.FullName = element.Element("fullname").Value;

			if (element.Attribute("class") != null)
				feature.Class = element.Attribute("class").Value;

			if (element.Attribute("category") != null)
				feature.Category = element.Attribute("category").Value;

			if (element.Attribute("type") != null)
				feature.FeatureType = element.Attribute("type").Value;

			return feature;
		}

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
			clone.Category = Category;
			clone.FeatureType = FeatureType;
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
		[XmlAttribute("category")]
		public string Category { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlAttribute("type")]
		public string FeatureType { get; set; }

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
