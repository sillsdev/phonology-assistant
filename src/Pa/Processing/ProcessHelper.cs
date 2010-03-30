// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ProcessHelper.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Xml;
using SIL.Pa.Model;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ProcessHelper
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This should only be done for debugging.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStreamToFile(MemoryStream stream, string outputFileName)
		{
			using (var fileStream = new FileStream(outputFileName, FileMode.Create))
			{
				stream.WriteTo(fileStream);
				fileStream.Close();
			}

			// This makes it all pretty, with proper indentation and line-breaking.
			var doc = new XmlDocument();
			doc.Load(outputFileName);
			doc.Save(outputFileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteMetadata(XmlWriter writer, PaProject project, bool closeDiv)
		{
			writer.WriteStartElement("div");
			writer.WriteAttributeString("id", "metadata");

			// Open ul and div
			writer.WriteStartElement("ul");
			writer.WriteAttributeString("class", "settings");

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "projectFolder");
			writer.WriteString(TerminateFolderPath(project.ProjectPath));
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "programConfigurationFolder");
			writer.WriteString(TerminateFolderPath(App.ConfigFolder));
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "programPhoneticInventoryFile");
			writer.WriteString(InventoryHelper.kDefaultInventoryFileName);
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "userFolder");
			writer.WriteString(TerminateFolderPath(App.DefaultProjectFolder));
			writer.WriteEndElement();

			writer.WriteStartElement("li");
			writer.WriteAttributeString("class", "projectPhoneticInventoryFile");
			writer.WriteString(Path.GetFileName(project.ProjectInventoryFileName));
			writer.WriteEndElement();

			// Close ul
			writer.WriteEndElement();

			if (closeDiv)
				writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure the last character in the specified path is a directory separator
		/// character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string TerminateFolderPath(string path)
		{
			path = path.Trim();
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				path += Path.DirectorySeparatorChar.ToString();

			return path;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the attribute.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteAttrib(XmlWriter writer, string attribName, string attribValue)
		{
			writer.WriteStartAttribute(attribName);
			writer.WriteString(attribValue);
			writer.WriteEndAttribute();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteElement(XmlWriter writer, string elementName, string elementValue)
		{
			writer.WriteStartElement(elementName);
			writer.WriteString(elementValue);
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteEmptyElement(XmlWriter writer, string elementName)
		{
			writer.WriteStartElement(elementName);
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Does not close the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStartElementWithAttrib(XmlWriter writer, string elementName,
			string attribName, string attribValue)
		{
			writer.WriteStartElement(elementName);
			writer.WriteStartAttribute(attribName);
			writer.WriteString(attribValue);
			writer.WriteEndAttribute();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStartElementWithAttribAndValue(XmlWriter writer,
			string elementName, string attribName, string attribValue, string elementValue)
		{
			WriteStartElementWithAttrib(writer, elementName, attribName, attribValue);
			writer.WriteString(elementValue);
			writer.WriteEndElement();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Closes the element.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WriteStartElementWithAttribAndEmptyValue(XmlWriter writer,
			string elementName, string attribName, string attribValue)
		{
			WriteStartElementWithAttrib(writer, elementName, attribName, attribValue);
			writer.WriteEndElement();
		}
	}
}
