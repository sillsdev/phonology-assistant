using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	public class XmlHelper
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Full path and file name of the xml file to verify.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool IsEmptyOrInvalid(string fileName)
		{
			var doc = new XmlDocument();

			try
			{
				doc.Load(fileName);
				return false;
			}
			catch { }

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Transforms the specified input file using the xslt contained in the specifed
		/// stream. The result is returned in a temporary file. It's expected the caller
		/// will move the file to the desired location.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Exception TransformFile(string inputFile, Stream xsltStream, out string outputFile)
		{
			outputFile = null;

			if (xsltStream == null || IsEmptyOrInvalid(inputFile))
				return null;

			outputFile = Path.GetTempFileName();

			try
			{
				using (var reader = new XmlTextReader(xsltStream))
				{
					var xslt = new XslCompiledTransform();
					xslt.Load(reader);
					xslt.Transform(inputFile, outputFile);
					reader.Close();
					if (!IsEmptyOrInvalid(outputFile))
						return null;

					var msg = string.Format(
						"Xsl Transformation seemed successful, but its output file '{0}' cannot be found.", outputFile);
					return new FileNotFoundException(msg);
				}
			}
			catch (Exception e)
			{
				return e;
			}
			finally
			{
				xsltStream.Close();
				try { File.Delete(outputFile); }
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an attribute's value from the specified node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attribute"></param>
		/// <returns>String value of the attribute or null if it cannot be found.</returns>
		/// ------------------------------------------------------------------------------------
		public static string GetAttributeValue(XmlNode node, string attribute)
		{
			if (node == null || node.Attributes[attribute] == null)
				return null;

			return node.Attributes.GetNamedItem(attribute).Value.Trim();
		}

		/// ------------------------------------------------------------------------------------
		public static int GetIntFromAttribute(XmlNode node, string attribute, int defaultValue)
		{
			string val = GetAttributeValue(node, attribute);
			int retVal;
			return (int.TryParse(val, out retVal) ? retVal : defaultValue);
		}

		/// ------------------------------------------------------------------------------------
		public static float GetFloatFromAttribute(XmlNode node, string attribute, float defaultValue)
		{
			string val = GetAttributeValue(node, attribute);
			float retVal;
			return (float.TryParse(val, out retVal) ? retVal : defaultValue);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetBoolFromAttribute(XmlNode node, string attribute)
		{
			return GetBoolFromAttribute(node, attribute, false);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetBoolFromAttribute(XmlNode node, string attribute, bool defaultValue)
		{
			string val = GetAttributeValue(node, attribute);
			return (val == null ? defaultValue : val.ToLower() == "true");
		}
	}
}
