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
			using (var outputStream = Transform(inputStream))
			{
				inputStream.Close();

				using (var fileStream = new FileStream(outputFileName, FileMode.Create))
				{
					outputStream.WriteTo(fileStream);
					outputStream.Close();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Given inputStream, return an output stream containing results from the pipeline. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MemoryStream Transform(Stream stream)
		{
			Debug.Assert(stream != null);
			Debug.Assert(m_processingSteps.Count != 0);

			// Write all the bytes from the input stream to a memory stream.
			stream.Seek(0, SeekOrigin.Begin);
			var memStream = new MemoryStream();
			for (long i = 0; i < stream.Length; i++)
				memStream.WriteByte((byte)stream.ReadByte());

			stream.Close();

			foreach (var step in m_processingSteps)
			{
				memStream.Seek(0, SeekOrigin.Begin);
				memStream = step.Transform(memStream);
			}

			return memStream;
		}
	}
}
