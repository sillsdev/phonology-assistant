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
			string xsltFilePath;
			// if Mono run-time, just use .xsl files in "Transforms" subfolder
			if (Type.GetType("Mono.Runtime") == null) // running .NET (not Mono Windows, not Mono Linux)
				xsltFilePath = Path.Combine(processingFolder, xsltFileName);
			else
				xsltFilePath = Path.Combine(Path.Combine(processingFolder, "Transforms"), xsltFileName);

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
					// MS didn't publish API for pre-compiled XSLT - http://tinyurl.com/mono-compiled-xslt and http://www.mono-project.com/XML#System.Xml.XPath_and_System.Xml.Xsl_2.0
					if (Type.GetType("Mono.Runtime") == null) // running .NET (not Mono Windows, not Mono Linux)
					{
						_xslt = new XslCompiledTransform(true);
						LoadPrecompiled ();
						return;
					}
					else
					{
						throw new Exception("File Not Found");
					}
				}
			}
			catch (Exception e)
			{
				Exception exception = new Exception("Unable to build XSL Transformation filter.", e);
				exception.Data.Add("XSL Transformation file path", XsltFilePath);
				throw exception;
			}
		}

		internal void LoadPrecompiled()
		{
			// This overload of XslCompiledTransform.Load() won't even build on Mono; see
			// http://lists.ximian.com/pipermail/mono-list/2009-September/043464.html
			// The consequence is that Mono builds won't run on .NET, but at least I can test Visual Studio
			// builds on Mono Windows.
			// Also, the Visual Studio build on Mono Windows throws the excpetion:
			//     Method not found: 'System.Xml.Xsl.XslCompiledTransform.Load'
			// on the code below _unless_ it is in a separate method which is never called on Mono.
			#if !__MonoCS__
			var compiledTransformType = Type.GetType(Path.GetFileNameWithoutExtension(XsltFilePath) +
				", PaCompiledTransforms, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
			_xslt.Load(compiledTransformType);
			#endif
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
