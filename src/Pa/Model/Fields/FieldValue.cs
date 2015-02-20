// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Xml.Serialization;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("FieldValueInfo")]
	public class FieldValue
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
		public FieldValue()
		{
		}

		/// ------------------------------------------------------------------------------------
		public FieldValue(string name)
		{
			Name = name;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format("{0}: {1}", Name, Value);
		}
	}
}
