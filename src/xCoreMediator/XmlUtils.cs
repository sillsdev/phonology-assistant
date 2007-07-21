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
		/// <summary>
		/// Constructor.
		/// </summary>
		public XmlUtils()
		{
		}
		
		/// <summary>
		/// Returns true if value of attrName is 'true' or 'yes' (case ignored)
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="attrName">The optional attribute to find.</param>
		/// <returns></returns>
		public static bool GetBooleanAttributeValue(XmlNode node, string attrName)
		{
			return GetBooleanAttributeValue(GetOptionalAttributeValue(node, attrName));
		}

		/// <summary>
		/// Returns true if sValue is 'true' or 'yes' (case ignored)
		/// </summary>
		public static bool GetBooleanAttributeValue(string sValue)
		{
			return (sValue != null
				&& (sValue.ToLower().Equals("true")
				|| sValue.ToLower().Equals("yes")));
		}

		/// <summary>
		/// Returns a integer obtained from the (mandatory) attribute named.
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="attrName">The mandatory attribute to find.</param>
		/// <returns>The value, or 0 if attr is missing.</returns>
		public static int GetMandatoryIntegerAttributeValue(XmlNode node, string attrName)
		{
			return Int32.Parse(GetManditoryAttributeValue(node, attrName));
		}

		/// <summary>
		/// Return an optional integer attribute value, or if not found, the default value.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attrName"></param>
		/// <param name="defaultVal"></param>
		/// <returns></returns>
		public static int GetOptionalIntegerValue(XmlNode node, string attrName, int defaultVal)
		{
			string val = GetOptionalAttributeValue(node, attrName);
			if (val == null)
				return defaultVal;
			return Int32.Parse(val);
		}

		/// <summary>
		/// Retrieve an array, given an attribute consisting of a comma-separated list of integers
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attrName"></param>
		/// <returns></returns>
		public static int[] GetMandatoryIntegerListAttributeValue(XmlNode node, string attrName)
		{
			string input = GetManditoryAttributeValue(node, attrName);
			string[] vals = input.Split(',');
			int[] result = new int[vals.Length];
			for (int i = 0; i < vals.Length; i++)
				result[i] = Int32.Parse(vals[i]);
			return result;
		}

		/// <summary>
		/// Make a value suitable for GetMandatoryIntegerListAttributeValue to parse.
		/// </summary>
		/// <param name="vals"></param>
		/// <returns></returns>
		public static string MakeIntegerListValue(int[] vals)
		{
			StringBuilder builder = new StringBuilder(vals.Length * 7); // enough unless VERY big numbers
			for (int i = 0; i < vals.Length; i++)
			{
				if (i != 0)
					builder.Append(",");
				builder.Append(vals[i].ToString());
			}
			return builder.ToString();
		}
		/// <summary>
		/// Make a comma-separated list of the ToStrings of the values in the list.
		/// </summary>
		/// <param name="vals"></param>
		/// <returns></returns>
		public static string MakeListValue(List<int> vals)
		{
			StringBuilder builder = new StringBuilder(vals.Count * 7); // enough unless VERY big numbers
			for (int i = 0; i < vals.Count; i++)
			{
				if (i != 0)
					builder.Append(",");
				builder.Append(vals[i].ToString());
			}
			return builder.ToString();
		}
	
		/// <summary>
		/// Get an optional attribute value from an XmlNode.
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="attrName">The attribute to find.</param>
		/// <param name="defaultValue"></param>
		/// <returns>The value of the attribute, or the default value, if the attribute dismissing</returns>
		public static bool GetOptionalBooleanAttributeValue(XmlNode node, string attrName, bool defaultValue)
		{
			return GetBooleanAttributeValue(GetOptionalAttributeValue(node, attrName, defaultValue?"true":"false"));
		}

		/// <summary>
		/// Deprecated: use GetOptionalAttributeValue instead.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attrName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetAttributeValue(XmlNode node, string attrName, string defaultValue)
		{
			return GetOptionalAttributeValue(node, attrName, defaultValue);
		}

		/// <summary>
		/// Get an optional attribute value from an XmlNode.
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="attrName">The attribute to find.</param>
		/// <returns>The value of the attribute, or null, if not found.</returns>
		public static string GetAttributeValue(XmlNode node, string attrName)
		{
			return GetOptionalAttributeValue(node, attrName);
		}

		/// <summary>
		/// Get an optional attribute value from an XmlNode.
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="attrName">The attribute to find.</param>
		/// <returns>The value of the attribute, or null, if not found.</returns>
		public static string GetOptionalAttributeValue(XmlNode node, string attrName)
		{
			return GetOptionalAttributeValue(node, attrName, null);
		}

		/// <summary>
		/// Get an optional attribute value from an XmlNode.
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="attrName">The attribute to find.</param>
		/// <returns>The value of the attribute, or null, if not found.</returns>
		public static string GetOptionalAttributeValue(XmlNode node, string attrName, string defaultString)
		{
			if (node != null && node.Attributes != null)
			{
				XmlAttribute xa = node.Attributes[attrName];
				if (xa != null)
					return xa.Value;
			}
			return defaultString;
		}

		/// <summary>
		/// Get an optional attribute value from an XmlNode, and look up its localized value in the
		/// given StringTable.
		/// </summary>
		/// <param name="tbl"></param>
		/// <param name="node"></param>
		/// <param name="attrName"></param>
		/// <param name="defaultString"></param>
		/// <returns></returns>
		public static string GetLocalizedAttributeValue(StringTable tbl, XmlNode node,
			string attrName, string defaultString)
		{
			string sValue = GetOptionalAttributeValue(node, attrName, defaultString);
			if (tbl == null)
				return sValue;
			else
				return tbl.LocalizeAttributeValue(sValue);
		}

		/// <summary>
		/// Return the node that has the desired 'name', either the input node or a decendent.
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="name">The XmlNode name to find.</param>
		/// <returns></returns>
		public static XmlNode FindNode(XmlNode node, string name)
		{
			if (node.Name == name)
				return node;
			foreach (XmlNode childNode in node.ChildNodes)
			{
				if (childNode.Name == name)
					return childNode;
				XmlNode n = FindNode(childNode, name);
				if (n != null)
					return n;
			}
			return null;
		}

		/// <summary>
		/// Get an obligatory attribute value.
		/// </summary>
		/// <param name="node">The XmlNode to look in.</param>
		/// <param name="attrName">The required attribute to find.</param>
		/// <returns>The value of the attribute.</returns>
		/// <exception cref="ApplicationException">
		/// Thrown when the value is not found in the node.
		/// </exception>
		public static string GetManditoryAttributeValue(XmlNode node, string attrName)
		{
			string retval = GetOptionalAttributeValue(node, attrName, null);
			if (retval == null)
			{
				throw new ApplicationException("The attribute'"
					+ attrName
					+ "' is mandatory, but was missing. "
					+ node.OuterXml);
			}
			return retval;
		}

		/// <summary>
		/// Append an attribute with the specified name and value to parent.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="attrName"></param>
		/// <param name="attrVal"></param>
		public static void AppendAttribute(XmlNode parent, string attrName, string attrVal)
		{
			XmlAttribute xa = parent.OwnerDocument.CreateAttribute(attrName);
			xa.Value = attrVal;
			parent.Attributes.Append(xa);
		}

		/// <summary>
		/// Return true if the two nodes match. Corresponding children should match, and
		/// corresponding attributes (though not necessarily in the same order).
		/// The nodes are expected to be actually XmlElements; not tested for other cases.
		/// Comments do not affect equality.
		/// </summary>
		/// <param name="node1"></param>
		/// <param name="node2"></param>
		/// <returns></returns>
		static public bool NodesMatch(XmlNode node1, XmlNode node2)
		{
			if (node1 == null && node2 == null)
				return true;
			if (node1 == null || node2 == null)
				return false;
			if (node1.Name != node2.Name)
				return false;
			if (node1.InnerText != node2.InnerText)
				return false;
			if (node1.Attributes == null && node2.Attributes != null)
				return false;
			if (node1.Attributes != null && node2.Attributes == null)
				return false;
			if (node1.Attributes != null)
			{
				if (node1.Attributes.Count != node2.Attributes.Count)
					return false;
				for (int i = 0; i < node1.Attributes.Count; i++)
				{
					XmlAttribute xa1 = node1.Attributes[i];
					XmlAttribute xa2 = node2.Attributes[xa1.Name];
					if (xa2 == null || xa1.Value != xa2.Value)
						return false;
				}
			}
			if (node1.ChildNodes == null && node2.ChildNodes != null)
				return false;
			if (node1.ChildNodes != null && node2.ChildNodes == null)
				return false;
			if (node1.ChildNodes != null)
			{
				int ichild1 = 0; // index node1.ChildNodes
				int ichild2 = 0; // index node2.ChildNodes
				while (ichild1 < node1.ChildNodes.Count && ichild2 < node1.ChildNodes.Count)
				{
					XmlNode child1 = node1.ChildNodes[ichild1];

					// Note that we must defer doing the 'continue' until after we have checked to see if both children are comments
					// If we continue immediately and the last node of both elements is a comment, the second node will not have
					// ichild2 incremented and the final test will fail.
					bool foundComment = false;

					if (child1 is XmlComment)
					{
						ichild1++;
						foundComment = true;
					}
					XmlNode child2 = node2.ChildNodes[ichild2];
					if (child2 is XmlComment)
					{
						ichild2++;
						foundComment = true;
					}

					if (foundComment)
						continue;

					if (!NodesMatch(child1, child2))
						return false;
					ichild1++;
					ichild2++;
				}
				// If we finished both lists we got a match.
				return ichild1 == node1.ChildNodes.Count && ichild2 == node2.ChildNodes.Count;
			}
			else
			{
				// both lists are null
				return true;
			}
		}

		/// <summary>
		/// Return the first child of the node that is not a comment (or null).
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static XmlNode GetFirstNonCommentChild(XmlNode node)
		{
			if (node == null)
				return null;
			foreach(XmlNode child in node.ChildNodes)
				if (!(child is XmlComment))
					return child;
			return null;
		}
		/// <summary>
		/// Apply an XSLT transform on a DOM to produce a resulting file
		/// </summary>
		/// <param name="sTransformName">full path name of the XSLT transform</param>
		/// <param name="inputDOM">XmlDocument DOM containing input to be transformed</param>
		/// <param name="sOutputName">full path of the resulting output file</param>
		public static void TransformDomToFile(string sTransformName, XmlDocument inputDOM, string sOutputName)
		{
			string sTempInput = FwTempFile.CreateTempFileAndGetPath("xml");
			try
			{
				inputDOM.Save(sTempInput);
				TransformFileToFile(sTransformName, sTempInput, sOutputName); 
			}
			finally
			{
				if (File.Exists(sTempInput))
					File.Delete(sTempInput);
			}
		}
		/// <summary>
		/// Apply an XSLT transform on a file to produce a resulting file
		/// </summary>
		/// <param name="sTransformName">full path name of the XSLT transform</param>
		/// <param name="sInputPath">full path of the input file</param>
		/// <param name="sOutputName">full path of the resulting output file</param>
		public static void TransformFileToFile(string sTransformName, string sInputPath, string sOutputName)
		{
			TransformFileToFile(sTransformName, null, sInputPath, sOutputName);
		}

		/// <summary>
		/// Fix the string to be safe in a text region of XML.
		/// </summary>
		/// <param name="sInput"></param>
		/// <returns></returns>

		public static string MakeSafeXml(string sInput)
		{
			string sOutput = sInput;

			if (sOutput != null && sOutput.Length != 0)
			{
				sOutput = sOutput.Replace("&", "&amp;");
				sOutput = sOutput.Replace("<", "&lt;");
				sOutput = sOutput.Replace(">", "&gt;");
			}
			return sOutput;
		}

		/// <summary>
		/// Fix the string to be safe in an attribute value of XML.
		/// </summary>
		/// <param name="sInput"></param>
		/// <returns></returns>

		public static string MakeSafeXmlAttribute(string sInput)
		{
			string sOutput = sInput;

			if (sOutput != null && sOutput.Length != 0)
			{
				sOutput = sOutput.Replace("&", "&amp;");
				sOutput = sOutput.Replace("\"", "&quot;");
				sOutput = sOutput.Replace("'", "&apos;");
				sOutput = sOutput.Replace("<", "&lt;");
				sOutput = sOutput.Replace(">", "&gt;");
			}
			return sOutput;
		}

		/// <summary>
		/// Utility function to find a methodInfo for the named method.
		/// It is a static method of the class specified in the EditRowClass of the EditRowAssembly.
		/// </summary>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public static MethodInfo GetStaticMethod(XmlNode node, string sAssemblyAttr, string sClassAttr, 
			string sMethodName, out Type typeFound)
		{
			string sAssemblyName = GetAttributeValue(node, sAssemblyAttr);
			string sClassName = GetAttributeValue(node, sClassAttr);
			MethodInfo mi = GetStaticMethod(sAssemblyName, sClassName, sMethodName, 
				"node " + node.OuterXml, out typeFound);
			return mi;
		}
		/// <summary>
		/// Utility function to find a methodInfo for the named method.
		/// It is a static method of the class specified in the EditRowClass of the EditRowAssembly.
		/// </summary>
		/// <param name="methodName"></param>
		/// <returns></returns>
		public static MethodInfo GetStaticMethod(string sAssemblyName, string sClassName, 
			string sMethodName, string sContext, out Type typeFound)
		{
			typeFound = null;
			Assembly assemblyFound = null;
			try
			{
				string baseDir = Path.GetDirectoryName(
					Assembly.GetExecutingAssembly().CodeBase).
					Substring(6);
				assemblyFound = Assembly.LoadFrom(
					Path.Combine(baseDir, sAssemblyName));
			}
			catch (Exception error)
			{
				string sMainMsg = "DLL at " + sAssemblyName;
				string sMsg = MakeGetStaticMethodErrorMessage(sMainMsg, sContext);
				throw new RuntimeConfigurationException(sMsg, error);
			}
			Debug.Assert(assemblyFound != null);
			try
			{
				typeFound = assemblyFound.GetType(sClassName);
			}
			catch (Exception error)
			{
				string sMainMsg = "class called " + sClassName;
				string sMsg = MakeGetStaticMethodErrorMessage(sMainMsg, sContext);
				throw new RuntimeConfigurationException(sMsg, error);
			}
			Debug.Assert(typeFound != null);
			MethodInfo mi = null;
			try
			{
				mi = typeFound.GetMethod(sMethodName);
			}
			catch (Exception error)
			{
				string sMainMsg = "method called " + sMethodName + " of class " + sClassName +
					" in assembly " + sAssemblyName;
				string sMsg = MakeGetStaticMethodErrorMessage(sMainMsg, sContext);
				throw new RuntimeConfigurationException(sMsg, error);
			}
			return mi;
		}
		static protected string MakeGetStaticMethodErrorMessage(string sMainMsg, string sContext)
		{
			string sResult = "GetStaticMethod() could not find the " + sMainMsg +
				" while processing " + sContext;
			return sResult;
		}

		/// <summary>
		/// Apply an XSLT transform on a file to produce a resulting file
		/// </summary>
		/// <param name="sTransformName">full path name of the XSLT transform</param>
		/// <param name="parameterList">list of parameters to pass to the transform</param>
		/// <param name="sInputPath">full path of the input file</param>
		/// <param name="sOutputName">full path of the resulting output file</param>
		public static void TransformFileToFile(string sTransformName, XSLParameter[] parameterList, string sInputPath, string sOutputName)
		{
			// Rewrite method using .Net 2.0 classes.
			throw new Exception("The method TransformFileToFile is not implemented.");

//#if UsingDotNetTransforms
//#if DEBUG
//            DateTime start = DateTime.Now;
//            Debug.WriteLine("\tStarting at: " + start.TimeOfDay.ToString());
//#endif
//            TextWriter writer = null;
//            try
//            {
//                // set up transform
//                XslTransform transformer = new XslTransform();
//                transformer.Load(sTransformName);

//                // add any parameters
//                XsltArgumentList args;
//                AddParameters(out args, parameterList);

//                // setup output file
//                writer = File.CreateText(sOutputName);

//                // load input file
//#if DEBUG
//                DateTime beforeDom = DateTime.Now;
//                Debug.WriteLine("\t\tBefore loading DOM, time is: " + beforeDom.TimeOfDay.ToString());
//#endif
//                XPathDocument inputDOM = new XPathDocument(sInputPath);
//#if DEBUG
//                DateTime afterDom = DateTime.Now;
//                Debug.WriteLine("\t\tAfter  loading DOM, time is: " + afterDom.TimeOfDay.ToString());
//                System.TimeSpan diffDom = afterDom.Subtract(beforeDom);
//                Debug.WriteLine("\t\tDom load took " + diffDom.ToString());
//#endif
//                // Apply transform
//                transformer.Transform(inputDOM, args, writer, null);
//#if DEBUG
//                DateTime afterXSLT = DateTime.Now;
//                Debug.WriteLine("\t\tAfter    transform, time is: " + afterXSLT.TimeOfDay.ToString());
//                System.TimeSpan diffXSLT = afterXSLT.Subtract(afterDom);
//                Debug.WriteLine("\t\tTransform took " + diffXSLT.ToString());
//#endif
//            }
//            finally
//            {
//                if (writer != null)
//                    writer.Close();
//            }
//#if DEBUG
//            DateTime end = DateTime.Now;
//            Debug.WriteLine("\tEnding at: " + end.TimeOfDay.ToString());
//            System.TimeSpan diff = end.Subtract(start);
//            Debug.WriteLine("\tProcess took: " + diff.ToString() + " " + sOutputName);
//#endif
//#else // not UsingDotNetTransforms
//            //.Net framework XML transform is still slower than something like MSXML2
//            // (this is so especially for transforms using xsl:key).
//            MSXML2.XSLTemplate40Class xslt = new MSXML2.XSLTemplate40Class();
//            MSXML2.FreeThreadedDOMDocument40Class xslDoc = new
//                MSXML2.FreeThreadedDOMDocument40Class();
//            MSXML2.DOMDocument40Class xmlDoc = new MSXML2.DOMDocument40Class();
//            MSXML2.IXSLProcessor xslProc;

//            xslDoc.async = false;
//            xslDoc.load(sTransformName);
//            xslt.stylesheet = xslDoc;
//            xmlDoc.async = false;
//            xmlDoc.load(sInputPath);
//            xslProc = xslt.createProcessor();
//            xslProc.input = xmlDoc;
//            AddParameters(parameterList, xslProc);
//            xslProc.transform();
//            StreamWriter sr = File.CreateText(sOutputName);
//            sr.Write(xslProc.output);
//            sr.Close();
//#endif // UsingDotNetTransforms
		}

#if UsingDotNetTransforms
		static private void AddParameters(out XsltArgumentList args, XSLParameter[] parameterList)
		{
			args = new XsltArgumentList();
			if (parameterList != null)
			{
				foreach(XSLParameter rParam in parameterList)
				{
					// Following is a specially recognized parameter name
					if (rParam.Name == "prmSDateTime")
					{
						args.AddParam(rParam.Name, "", GetCurrentDateTime());
					}
					else
						args.AddParam(rParam.Name, "", rParam.Value);
				}
			}
		}
#else
		/// <summary>
		/// Add parameters to a transform
		/// </summary>
		/// <param name="parameterList"></param>
		/// <param name="xslProc"></param>
		private static void AddParameters(XSLParameter[] parameterList, MSXML2.IXSLProcessor xslProc)
		{
			if (parameterList != null)
			{
				foreach(XSLParameter rParam in parameterList)
				{
					// Following is a specially recognized parameter name
					if (rParam.Name == "prmSDateTime")
					{
						xslProc.addParameter(rParam.Name, GetCurrentDateTime(), "");
					}
					else
						xslProc.addParameter(rParam.Name, rParam.Value, "");
				}
			}
		}
#endif
		/// <summary>
		/// Are we using the .Net XSLT transforms?
		/// </summary>
		/// <returns>true if we're using .Net XSLT transforms
		/// false if we're using MSXML2</returns>
		public static bool UsingDotNetTransforms()
		{
#if UsingDotNetTransforms
			return true;
#else
			return false;
#endif
		}
			private static string GetCurrentDateTime()
		{
			DateTime now;
			now = DateTime.Now;
			return (now.ToShortDateString() + " " + now.ToLongTimeString());
		}
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
