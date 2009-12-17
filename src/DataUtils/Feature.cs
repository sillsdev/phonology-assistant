// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: FeatureBase.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Xml.Serialization;

namespace SIL.Pa.Data
{
	#region Feature
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("feature")]
	public class Feature
	{
		protected int m_bit;
		protected string m_name;
		protected string m_fullname;
		protected string m_class;
		protected string m_clements;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones the specified binary feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Feature Clone()
		{
			var clone = new Feature();
			clone.m_bit = m_bit;
			clone.m_name = m_name;
			clone.m_fullname = m_fullname;
			clone.m_class = m_class;
			clone.m_clements = m_clements;
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FullName + " (bit: " + m_bit + ")";
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("class")]
		public string Class
		{
			get { return m_class; }
			set { m_class = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute("clements")]
		public string Clements
		{
			get { return m_clements; }
			set { m_clements = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public int Bit
		{
			get { return m_bit; }
			internal set { m_bit = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("name")]
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlElement("fullname")]
		public string FullName
		{
			get { return (string.IsNullOrEmpty(m_fullname) ? Name : m_fullname); }
			set { m_fullname = value; }
		}

		#endregion
	}

	#endregion
}
