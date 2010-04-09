using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
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
		public enum ProcessType
		{
			PrepareInventory,
			ViewCVChart,
			ExportDataCorpus,
			ExportSearchResult,
			ExportCVChart,
			ExportDistributionChart,
			ExportToXHTML,
			ExportToCss,
			ExportToWord
		}

		public delegate void StepProcessEventHandler(Pipeline pipeline, Step step);
		public event StepProcessEventHandler BeforeStepProcessed;
		public event StepProcessEventHandler AfterStepProcessed;

		public List<Step> ProcessingSteps { get; private set; }
		private MemoryStream m_finalStream;

		private const string kNamespacePrefix = "p";
		private const string kNamespaceURI = "http://www.w3.org/ns/xproc";
		
		// In the Processing.xml file, each pipeline and its children are in the XProc namespace.
		// However, the file does not necessarily specify the p: prefix on the inndividual elements.
		private static XmlNameTable s_nameTable;
		private static XmlNamespaceManager s_namespaceManager;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Pipeline Create(ProcessType prsType, string processFileName,
			string processingFolder)
		{
			var settings = new XmlReaderSettings();
			var processingDocument = new XPathDocument(XmlReader.Create(processFileName, settings));
			var processingNavigator = processingDocument.CreateNavigator();

			var xpath = GetProcessXPath(prsType);
			if (xpath == null)
				return null;
	
			var processNavigator = processingNavigator.SelectSingleNode(xpath);
			if (processNavigator == null)
				return null;
			
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
		private static string GetProcessXPath(ProcessType prsType)
		{
			switch (prsType)
			{
				case ProcessType.PrepareInventory:
					return "processing/process[@type='inventory']";

				case ProcessType.ViewCVChart:
					return "processing/process[@type='view'][@view='CV Chart']";

				case ProcessType.ExportDataCorpus:
					return "processing/process[@type='export'][@view='Data Corpus']";

				case ProcessType.ExportSearchResult:
					return "processing/process[@type='export'][@view='Search']";

				case ProcessType.ExportCVChart:
					return "processing/process[@type='export'][@view='CV Chart']";

				case ProcessType.ExportDistributionChart:
					return "processing/process[@type='export'][@view='Distribution Chart']";

				case ProcessType.ExportToXHTML:
					return "processing/process[@type='export'][@format='XHTML']";
	
				case ProcessType.ExportToCss:
					return "processing/process[@type='export'][@format='CSS']";
			
				case ProcessType.ExportToWord:
					return "processing/process[@type='export'][@format='Word_2003_XML']";
			}

			return null;
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
					s_namespaceManager.AddNamespace(kNamespacePrefix, kNamespaceURI);
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
			ProcessingSteps = new List<Step>(stepList);
        }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Transform the specified file and return the results in the specified output file.
		/// If the output file already exists, its contents will be lost and replaced with
		/// the new results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Transform(string inputFileName, string outputFileName)
		{
			using (var outputStream = Transform(inputFileName))
			using (var fileStream = new FileStream(outputFileName, FileMode.Create))
			{
				outputStream.WriteTo(fileStream);
				outputStream.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Transform the specified file and return the results in a memory stream.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MemoryStream Transform(string inputFileName)
		{
			using (var inputStream = new FileStream(inputFileName, FileMode.Open))
			{
				// Write all the bytes from the file stream to a memory stream.
				// It seems to me there has to be a better way to do this, but
				// I could not find one in my limited search.
				var memStream = new MemoryStream();
				for (long i = 0; i < inputStream.Length; i++)
					memStream.WriteByte((byte)inputStream.ReadByte());

				inputStream.Close();
				return Transform(memStream);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Transform the contents of the specified input stream and returns the results in
		/// the specified output file. If the output file already exists, its contents will
		/// be lost and replaced with the new results.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Transform(MemoryStream inputStream, string outputFileName)
		{
			using (var outputStream = Transform(inputStream))
			using (var fileStream = new FileStream(outputFileName, FileMode.Create))
			{
				outputStream.WriteTo(fileStream);
				outputStream.Close();
			}
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Given stream, return an output stream containing results from the pipeline. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MemoryStream Transform(MemoryStream stream)
		{
			Debug.Assert(stream != null);
			Debug.Assert(ProcessingSteps.Count != 0);

			m_finalStream = stream;

			using (var worker = new BackgroundWorker())
			{
				worker.WorkerReportsProgress = true;
				worker.WorkerSupportsCancellation = false;
				worker.DoWork += worker_DoWork;
				worker.ProgressChanged += worker_ProgressChanged;
				worker.RunWorkerCompleted += worker_RunWorkerCompleted;
				worker.RunWorkerAsync(new object[] { stream, ProcessingSteps });

				while (worker.IsBusy)
					Application.DoEvents();
			}

			return m_finalStream;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			var worker = sender as BackgroundWorker;
			var args = e.Argument as object[];
			var stream = args[0] as MemoryStream;
			var processingSteps = args[1] as List<Step>;

			foreach (var step in processingSteps)
			{
				worker.ReportProgress(0, step);
				stream.Seek(0, SeekOrigin.Begin);
				stream = step.Transform(stream);
				worker.ReportProgress(1, step);
			}

			e.Result = stream;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage == 0)
			{
				if (BeforeStepProcessed != null)
					BeforeStepProcessed(this, e.UserState as Step);
			}
			else if (AfterStepProcessed != null)
				AfterStepProcessed(this, e.UserState as Step);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			m_finalStream = e.Result as MemoryStream;
		}
	}
}
