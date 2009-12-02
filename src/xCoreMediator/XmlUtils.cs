// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2003, SIL International. All Rights Reserved.   
// <copyright from='2003' to='2003' company='SIL International'>
//		Copyright (c) 2003, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: XmlUtils.cs
// Responsibility: Andy Black
// Last reviewed: 
// 
// <remarks>
// This makes available some utilities for handling XML Nodes
// </remarks>
// --------------------------------------------------------------------------------------------
#define UsingDotNetTransforms
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SIL.FieldWorks.Common.Utils;
//using MSXML2;

namespace SIL.Utils
{
	/// <summary>
	/// Summary description for XmlUtils.
	/// </summary>
	public class XmlUtils
	{

		///// <summary>
		///// Append an attribute with the specified name and value to parent.
		///// </summary>
		///// <param name="parent"></param>
		///// <param name="attrName"></param>
		///// <param name="attrVal"></param>
		//public static void AppendAttribute(XmlNode parent, string attrName, string attrVal)
		//{
		//    XmlAttribute xa = parent.OwnerDocument.CreateAttribute(attrName);
		//    xa.Value = attrVal;
		//    parent.Attributes.Append(xa);
		//}

		/// <summary>
		/// A class that represents a parameter of an XSL stylesheet.
		/// </summary>
		public class XSLParameter
		{
			/// <summary>
			/// Parameter name.
			/// </summary>
			private string m_name;
    
			/// <summary>
			/// Parameter value.
			/// </summary>
			private string m_value;

			public XSLParameter(string sName, string sValue)
			{
				m_name = sName;
				m_value = sValue;
			}

			public string Name
			{
				get { return m_name; }
				set { m_name = value; }
			}

			public string Value
			{
				get { return m_value; }
				set { m_value = value; }
			}
		}
	}
}
