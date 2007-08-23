using System.Xml;

namespace SIL.SpeechTools.Utils
{
	public class XMLHelper
	{
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="attrValue"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static int GetIntFromAttribute(XmlNode node, string attribute, int defaultValue)
		{
			string val = GetAttributeValue(node, attribute);
			return (val == null ? defaultValue : int.Parse(val));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attrValue"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static bool GetBoolFromAttribute(XmlNode node, string attribute)
		{
			return GetBoolFromAttribute(node, attribute, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attrValue"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static bool GetBoolFromAttribute(XmlNode node, string attribute, bool defaultValue)
		{
			string val = GetAttributeValue(node, attribute);
			return (val == null ? defaultValue : val.ToLower() == "true");
		}
	}
}
