using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Corresponds to one pipeline element in the Processing.xml file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Pipeline
	{
		private const string s_namespacePrefix = "p";
		private const string s_namespaceURI = "http://www.w3.org/ns/xproc";
		
		// In the Processing.xml file, each pipeline and its children are in the XProc namespace.
		// However, the file does not necessarily specify the p: prefix on the inndividual elements.
		private static XmlNameTable s_nameTable;
		private static XmlNamespaceManager s_namespaceManager;

		private readonly List<Step> m_processingSteps;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		static public Pipeline Create(string processName, string processFileName,
			string processingFolder)
		{
			var settings = new XmlReaderSettings();
			var processingDocument = new XPathDocument(XmlReader.Create(processFileName, settings));
			var processingNavigator = processingDocument.CreateNavigator();
			var xpath = string.Format("processing/process[@type='{0}']", processName);
			var processNavigator = processingNavigator.SelectSingleNode(xpath);
			
			processNavigator = processNavigator.SelectSingleNode("p:pipeline", NamespaceManager);
			if (processNavigator == null)
				return null;

			var stepList = new List<Step>();
			var iterator = processNavigator.Select("p:xslt", NamespaceManager);
			while (iterator.MoveNext())
			{
				var node = iterator.Current;
				var step = Step.Create(node, NamespaceManager, processingFolder);
				if (step != null)
					stepList.Add(step);
			}
	
			return (stepList.Count == 0 ? null : new Pipeline(stepList));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static XmlNamespaceManager NamespaceManager
		{
			get
			{
				// Create nameTable and namespaceManager once, when first needed.
				if (s_namespaceManager == null)
				{
					s_nameTable = new NameTable();
					s_namespaceManager = new XmlNamespaceManager(s_nameTable);
					s_namespaceManager.AddNamespace(s_namespacePrefix, s_namespaceURI);
				}

				return s_namespaceManager;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Pipeline(IEnumerable<Step> stepList)
        {
			m_processingSteps = new List<Step>(stepList);
        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Transform(string inputFileName, string outputFileName)
		{
			using (var inputStream = new FileStream(inputFileName, FileMode.Open))
			{
				MemoryStream outputStream = null;
				Transform(inputStream, ref outputStream);
				inputStream.Close();

				using (var outputFileStream = new FileStream(outputFileName, FileMode.Create))
				{
					outputStream.WriteTo(outputFileStream);
					outputStream.Close();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Given inputStream, return outputStream containing results from the pipeline. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Transform(Stream inputStream, ref MemoryStream outputStream)
		{
			Debug.Assert(outputStream == null);
			//outputStream = new MemoryStream();
			Debug.Assert(m_processingSteps.Count != 0);

			foreach (var step in m_processingSteps)
			{
				outputStream = new MemoryStream();
				step.Transform(inputStream, outputStream);
				inputStream = outputStream;
				inputStream.Seek(0, SeekOrigin.Begin);
			}

			//m_processingSteps[0].Transform(inputStream, outputStream);
			
			//for (int i = 1; i != m_processingSteps.Count; i++)
			//{
			//    inputStream = outputStream;
				
			//    // Because the output from the current filter is the input
			//    // to the next filter, seek to the beginning of the stream.
			//    inputStream.Seek(0, SeekOrigin.Begin);
			//    m_processingSteps[i].Transform(inputStream, new MemoryStream());
			//}
		}
	}
}
