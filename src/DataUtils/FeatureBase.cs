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
	#region FeatureBase
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FeatureBase
	{
		protected int m_bit;
		protected string m_name;
		protected string m_fullname;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clones the specified binary feature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FeatureBase Clone(FeatureBase clone)
		{
			clone.m_bit = m_bit;
			clone.m_name = m_name;
			clone.m_fullname = m_fullname;
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
		public string FullName
		{
			get { return (string.IsNullOrEmpty(m_fullname) ? Name : m_fullname); }
			set { m_fullname = value; }
		}

		#endregion
	}

	#endregion
}
