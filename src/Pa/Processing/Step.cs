// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
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
		private readonly Dictionary<string, XslCompiledTransform> _transformsCache =
			new Dictionary<string, XslCompiledTransform>();

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
#if !__MonoCS__ // Mono build just use .xsl files in "Transforms" subfolder
			var xsltFilePath = Path.Combine(processingFolder, xsltFileName);
#else
			var xsltFilePath = Path.Combine(Path.Combine(processingFolder, "Transforms"), xsltFileName);
#endif

			return new Step(xsltFilePath);
		}

		/// ------------------------------------------------------------------------------------
		private Step(string xsltFilePath)
		{
			XsltFilePath = xsltFilePath;

			try
			{
				if (File.Exists(xsltFilePath)) // 'manual' override by putting .xsl file in Processing folder
				{
					if (_transformsCache.TryGetValue(xsltFilePath, out _xslt))
						return;
	
					// TO DO: If you enable the document() function, restrict the resources that can be accessed
					// by passing an XmlSecureResolver object to the Transform method.
					// The XmlResolver used to resolve any style sheets referenced in XSLT import and include elements.
					var settings = new XsltSettings(true, false);
					var resolver = new XmlUrlResolver();
					resolver.Credentials = CredentialCache.DefaultCredentials;
					
					_xslt = new XslCompiledTransform(true);
					_xslt.Load(XsltFilePath, settings, resolver);
					_transformsCache[xsltFilePath] = _xslt;
				}
				else
				{
#if !__MonoCS__ // Mono seems to not support pre-compiled XSLT
					_xslt = new XslCompiledTransform(true);
					var compiledTransformType = Type.GetType(Path.GetFileNameWithoutExtension(XsltFilePath) +
						", PaCompiledTransforms, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
					_xslt.Load(compiledTransformType);
					return;
#else
					throw new Exception("File Not Found");
#endif
				}
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
