// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: CVChartBuilder.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Linq;
using System.Xml;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class will build a project's consonant and vowel chart xml files (i.e. files from
	/// which the Consonant and Vowel chart views are constructed).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVChartBuilder : ProjectInventoryBuilder
	{
		protected CVChartType m_chartType;

		/// ------------------------------------------------------------------------------------
		public static bool Process(PaProject project, CVChartType chartType)
		{
			if (project == null)
				return false;

			App.MsgMediator.SendMessage("BeforeBuildCVChart", new object[] { project, chartType });
			var bldr = new CVChartBuilder(project, chartType);
			var buildResult = bldr.InternalProcess();
			App.MsgMediator.SendMessage("AfterBuildCVChart", new object[] { project, chartType, buildResult });

			return buildResult;
		}

		/// ------------------------------------------------------------------------------------
		protected CVChartBuilder(PaProject project, CVChartType chartType) : base(project)
		{
			m_chartType = chartType;
			m_outputFileName = m_project.ProjectPathFilePrefix + chartType + "ChartBeta.xml";
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		protected override string ProgressMessage
		{
			get
			{
				if (m_chartType == CVChartType.Consonant)
				{
					return App.GetString("ProcessingConsonantChartMsg",
						"Separating Consonants...",
						"Status bar message displayed when building list of consonants from phone list.");
				}

				return App.GetString("ProcessingVowelChartMsg", "Separating Vowels...",
					"Status bar message displayed when building list of vowels from phone list.");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string TempFileName
		{
			get
			{
				return ProcessHelper.MakeTempFilePath(m_project,
					Path.ChangeExtension(m_outputFileName, "tmp"));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override bool KeepTempFile
		{
			get { return Settings.Default.KeepTempCVChartBuilderFile; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Pipeline.ProcessType ProcessType
		{
			get { return Pipeline.ProcessType.ViewCVChart; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will create a temporary project inventory file containing all 
		/// possible articulatory features. It will also add an attribute to the root
		/// element indicating what kind of chart to make (i.e. C or V).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override object CreateInputToTransformPipeline()
		{
			// Read the current project inventory file and find the root node.
			var doc1 = new XmlDocument();
			doc1.Load(m_project.ProjectInventoryFileName);
			var node = doc1.SelectSingleNode("inventory");

			// Add an attribute to the root node indicating the chart type.
			var attrib = doc1.CreateAttribute("view");
			attrib.Value = m_chartType + " Chart";
			node.Attributes.Append(attrib);

			// Load the list of articulatory features into an XML document.
			var doc2 = new XmlDocument();
			doc2.LoadXml(XmlSerializationHelper.SerializeToString(
				App.AFeatureCache.Values.ToList()));

			// Create a new node containing all the articulatory features and append that node
			// to the project inventory.
			var aFeatureNode = doc1.CreateNode(XmlNodeType.Element, "articulatoryFeatures", null);
			aFeatureNode.InnerXml = doc2.SelectSingleNode("ArrayOfFeature").InnerXml;
			node.AppendChild(aFeatureNode);

			// Save the new project inventory to a temporary file.
			doc1.Save(TempFileName);

			return TempFileName;
		}

		/// ------------------------------------------------------------------------------------
		protected override void PostBuildProcess()
		{
			if (!Settings.Default.KeepTempCVChartBuilderFile && File.Exists(TempFileName))
				File.Delete(TempFileName);
		}
	}
}
