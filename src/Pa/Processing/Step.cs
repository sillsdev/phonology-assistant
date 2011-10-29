using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Corresponds to one xslt element in the Processing.xml file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Step
	{
		public string XsltFilePath { get; private set; }
		
		private readonly XslCompiledTransform _xslt;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="navigator">The parent pipeline element containing the xslt
		/// elements.</param>
		/// <param name="namespaceManager">In the Processing.xml file, each pipeline and its
		/// children are in the XProc namespace.</param>
		/// <param name="processingFolder">Full path to folder containing Processing.xml
		/// and *.xsl files.</param>
		/// ------------------------------------------------------------------------------------
		public static Step Create(XPathNavigator navigator,
			XmlNamespaceManager namespaceManager, string processingFolder)
		{
			XPathNavigator documentNavigator =
				navigator.SelectSingleNode("p:input[@port='stylesheet']/p:document[@href]", namespaceManager);
			
			if (documentNavigator == null)
				return null;

			var xsltFileName = documentNavigator.GetAttribute("href", string.Empty); // No namespace for attributes.
			var xsltFilePath = Path.Combine(processingFolder, xsltFileName);

			return new Step(xsltFilePath);
		}

		/// ------------------------------------------------------------------------------------
		private Step(string xsltFilePath)
		{
			XsltFilePath = xsltFilePath;

			try
			{
				// TO DO: If you enable the document() function, restrict the resources that can be accessed
				// by passing an XmlSecureResolver object to the Transform method.
				// The XmlResolver used to resolve any style sheets referenced in XSLT import and include elements.
				var settings = new XsltSettings(true, false);
				var resolver = new XmlUrlResolver();
				resolver.Credentials = CredentialCache.DefaultCredentials;

				var compiledTransformType = Type.GetType(Path.GetFileNameWithoutExtension(XsltFilePath) +
					", PaCompiledTransforms, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
				
				_xslt = new XslCompiledTransform(true);
				
				if (compiledTransformType != null)
					_xslt.Load(compiledTransformType);
				else			
					_xslt.Load(XsltFilePath, settings, resolver);
			}
			catch (Exception e)
			{
				Exception exception = new Exception("Unable to build XSL Transformation filter.", e);
				exception.Data.Add("XSL Transformation file path", XsltFilePath);
				throw exception;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Given inputStream and outputStream, do one XSL Transformation step. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MemoryStream Transform(MemoryStream inputStream)
		{
			var readerSettings = new XmlReaderSettings();
			readerSettings.ProhibitDtd = false; // If true, it throws an exception if there is a DTD.
			readerSettings.ValidationType = ValidationType.None;
			var inputXML = XmlReader.Create(inputStream, readerSettings);

			// OutputSettings corresponds to the xsl:output element of the style sheet.
			var writerSettings = _xslt.OutputSettings.Clone();
			if (writerSettings.Indent) // Cause indent="true" to insert line breaks but not tabs.
				writerSettings.IndentChars = string.Empty; // Even if the document element is html.

			//var outputStream = new MemoryStream();
			var outputStream = new MemoryStream();
			var outputXML = XmlWriter.Create(outputStream, writerSettings);
			try
			{
				_xslt.Transform(inputXML, outputXML);
			}
			catch (Exception e)
			{
				var exception = new Exception("Unable to convert using XSL Transformation filter.", e);
				exception.Data.Add("XSL Transformation file path", XsltFilePath);
				throw exception;
			}
			finally
			{
				inputXML.Close();
				outputXML.Flush(); // The next filter or the data sink will close the stream
			}

			outputStream.Flush();
			return outputStream;
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return (string.IsNullOrEmpty(XsltFilePath) ?
				base.ToString() : Path.GetFileName(XsltFilePath));
		}
	}
}
