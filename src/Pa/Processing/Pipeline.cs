using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace SIL.Pa.Processing
{
	/// <summary>
	/// Corresponds to one pipeline element in the Processing.xml file.
	/// </summary>
	public class Pipeline
	{
		private List<Step> m_stepList;

		// In the Processing.xml file, each pipeline and its children are in the XProc namespace.
		// However, the file does not necessarily specify the p: prefix on the inndividual elements.
		static private XmlNameTable s_nameTable = null;
		static private XmlNamespaceManager s_namespaceManager = null;
		static private readonly string s_NamespacePrefix = "p";
		static private readonly string s_NamespaceURI = "http://www.w3.org/ns/xproc";

		// navigator: The parent process element containing the pipeline element.
		// processingFolderPath: Full path to folder containing the *.xsl files.
		// pipeline: Initially null, returns the object if this function returns true.
		static public bool Create(XPathNavigator navigator, string processingFolderPath,
			ref Pipeline pipeline)
		{
			Debug.Assert(pipeline == null);

			// Create nameTable and namespaceManager once, when first needed.
			// DAVID: Is this okay, and is there a better way to do it in C#?
			if (s_namespaceManager == null)
			{
				s_nameTable = new NameTable();
				s_namespaceManager = new XmlNamespaceManager(s_nameTable);
				s_namespaceManager.AddNamespace(s_NamespacePrefix, s_NamespaceURI);
			}

			navigator = navigator.SelectSingleNode("p:pipeline", s_namespaceManager);
			if (navigator == null)
				return false;

			List<Step> stepList = new List<Step>();
			XPathNodeIterator iterator = navigator.Select("p:xslt", s_namespaceManager);
			while (iterator.MoveNext())
			{
				XPathNavigator node = iterator.Current;
				Step step = null;
				if (Step.Create(node, s_namespaceManager, processingFolderPath, ref step))
					stepList.Add(step);
			}
			if (stepList.Count == 0)
				return false;

			pipeline = new Pipeline(stepList);
			return true;
		}

		Pipeline(IList<Step> stepList)
        {
			m_stepList = new List<Step>(stepList);
        }

		// Given inputStream, return outputStream containing results from the pipeline.
		public void Transform(Stream inputStream, ref MemoryStream outputStream)
		{
			Debug.Assert(outputStream == null);
			outputStream = new MemoryStream();
			Debug.Assert(m_stepList.Count != 0);
			m_stepList[0].Transform(inputStream, outputStream);
			for (int i = 1; i != m_stepList.Count; i++)
			{
				MemoryStream outputStreamFromPreviousFilter = outputStream;
				outputStream = new MemoryStream();
				// Because the output from the current filter is the input to the next filter
				// or the data sink, seek to the beginning of the stream.
				inputStream.Seek(0, SeekOrigin.Begin);
				m_stepList[i].Transform(outputStreamFromPreviousFilter, outputStream);
			}
		}

		// Here is sample code to create a pipeline and transform data.
		static void example()
		{
			// Create the pipeline.
			string processingFolderPath = "...\\Processing";
			string processingFilePath = Path.Combine(processingFolderPath, "Processing.xml");
			XmlReaderSettings settings = new XmlReaderSettings();
			XPathDocument processingDocument = new XPathDocument(XmlReader.Create(processingFilePath, settings));
			XPathNavigator processingNavigator = processingDocument.CreateNavigator();
			XPathNavigator processNavigator = processingNavigator.SelectSingleNode("processing/process[@type='inventory']");
			Pipeline pipeline = null;
			if (!Pipeline.Create(processNavigator, processingFolderPath, ref pipeline))
				return;

			// Use the pipeline.
			Stream inputStream = null; // Represents what the program produces.
			MemoryStream outputStream = null;
			pipeline.Transform(inputStream, ref outputStream);
			string outputFilePath = "...\\PROJECT.PhoneticInventory.xml";
			using (Stream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
			{
				outputStream.WriteTo(outputFileStream);
				outputStream.Close();
			}
		}
	}
}
