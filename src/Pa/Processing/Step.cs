using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SIL.Pa.Processing
{
	/// <summary>
	/// Corresponds to one xslt element in the Processing.xml file.
	/// </summary>
	public class Step
	{
		private string m_xsltFilePath;
		private XslCompiledTransform m_xslt;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="navigator">The parent pipeline element containing the xslt
		/// elements.</param>
		/// <param name="namespaceManager">In the Processing.xml file, each pipeline and its
		/// children are in the XProc namespace.</param>
		/// <param name="processingFolderPath">Full path to folder containing Processing.xml
		/// and *.xsl files.</param>
		/// <param name="step">Initially null, returns the object if this function
		/// returns true.</param>
		/// ------------------------------------------------------------------------------------
		static public bool Create(XPathNavigator navigator,
			XmlNamespaceManager namespaceManager, string processingFolderPath, ref Step step)
		{
			Debug.Assert(step == null);
			
			XPathNavigator documentNavigator =
				navigator.SelectSingleNode("p:input[@port='stylesheet']/p:document[@href]", namespaceManager);
			
			if (documentNavigator == null)
				return false;

			string xsltFileName = documentNavigator.GetAttribute("href", string.Empty); // No namespace for attributes.
			string xsltFilePath = Path.Combine(processingFolderPath, xsltFileName);
			XslCompiledTransform xslt = new XslCompiledTransform();

			try
			{
				// TO DO: If you enable the document() function, restrict the resources that can be accessed
				// by passing an XmlSecureResolver object to the Transform method.
				// The XmlResolver used to resolve any style sheets referenced in XSLT import and include elements.
				XsltSettings settings = new XsltSettings(true, false);
				XmlUrlResolver resolver = new XmlUrlResolver();
				resolver.Credentials = null;
				xslt.Load(xsltFilePath, settings, null);
			}
			catch (Exception innerException)
			{
				Exception exception = new Exception("Unable to build XSL Transformation filter.", innerException);
				exception.Data.Add("XSL Transformation file path", xsltFilePath);
				throw exception;
			}

			step = new Step(xsltFilePath, xslt);
			return true;
		}

		Step(string xsltFilePath, XslCompiledTransform xslt)
		{
			m_xsltFilePath = xsltFilePath;
			m_xslt = xslt;
		}

		// Given inputStream and outputStream, do one XSL Transformation step.
		public void Transform(Stream inputStream, MemoryStream outputStream)
		{
			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.ProhibitDtd = false; // If true, it throws an exception if there is a DTD.
			readerSettings.ValidationType = ValidationType.None;
			XmlReader inputXML = XmlReader.Create(inputStream, readerSettings);

			// OutputSettings corresponds to the xsl:output element of the style sheet.
			XmlWriterSettings writerSettings = m_xslt.OutputSettings.Clone();
			if (writerSettings.Indent) // Cause indent="true" to insert line breaks but not tabs.
				writerSettings.IndentChars = ""; // Even if the document element is html.

			XmlWriter outputXML = XmlWriter.Create(outputStream, writerSettings);
			try
			{
				m_xslt.Transform(inputXML, outputXML);
			}
			catch (Exception innerException)
			{
				Exception exception = new Exception("Unable to convert using XSL Transformation filter.", innerException);
				exception.Data.Add("XSL Transformation file path", m_xsltFilePath);
				throw exception;
			}
			finally
			{
				inputXML.Close();
				outputXML.Flush(); // The next filter or the data sink will close the stream
			}
		}
	}
}
