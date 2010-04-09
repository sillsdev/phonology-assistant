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
using SIL.Localization;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilUtils;

namespace SIL.Pa.Processing
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVChartBuilder : ProjectInventoryBuilder
	{
		public enum ChartType
		{
			Consonant,
			Vowel
		}

		protected ChartType m_chartType;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Process(PaProject project, PhoneCache phoneCache, ChartType chartType)
		{
			if (project == null)
				return false;

			App.MsgMediator.SendMessage("BeforeBuildCVChart",
				new object[] { project, phoneCache, chartType });

			var bldr = new CVChartBuilder(project, phoneCache, chartType);
			var buildResult = bldr.InternalProcess();

			App.MsgMediator.SendMessage("AfterBuildCVChart", 
				new object[] { project, phoneCache, chartType, buildResult });

			return buildResult;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected CVChartBuilder(PaProject project, PhoneCache phoneCache, ChartType chartType)
			: base(project, phoneCache)
		{
			m_chartType = chartType;
			m_outputFileName = m_project.ProjectPathFilePrefix + chartType + "Chart.xml";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool InternalProcess()
		{
			var doc1 = new XmlDocument();
			doc1.Load(m_project.ProjectInventoryFileName);
			var node = doc1.SelectSingleNode("inventory");

			var attrib = doc1.CreateAttribute("view");
			attrib.Value = m_chartType + " Chart";
			node.Attributes.Append(attrib);

			// Load the list of articulatory features into an XML document.
			var doc2 = new XmlDocument();
			doc2.LoadXml(XmlSerializationHelper.SerializeToString(
				InventoryHelper.AFeatureCache.Values.ToList()));

			// Create a new node containing all the articulatory features and append that node
			// to the project inventory.
			var aFeatureNode = doc1.CreateNode(XmlNodeType.Element, "articulatoryFeatures", null);
			aFeatureNode.InnerXml = doc2.SelectSingleNode("ArrayOfFeature").InnerXml;
			node.AppendChild(aFeatureNode);

			// Now save the new project inventory to a temporary file.
			var tmpFile = Path.ChangeExtension(m_outputFileName, "tmp");
			doc1.Save(tmpFile);

			var pipeline = ProcessHelper.CreatePipline(ProcessType);
			if (pipeline != null)
				pipeline.Transform(tmpFile, m_outputFileName);

			// This makes it all pretty, with proper indentation and line-breaking.
			doc1.Load(m_outputFileName);
			doc1.Save(m_outputFileName);

			if (!Settings.Default.KeepTempCVChartBuilderFile)
				File.Delete(tmpFile);

			return true;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string ProgressMessage
		{
			get
			{
				var id = m_chartType + "ChartProcessingMsg";
				var msg = string.Format("Processing {0} Chart: ", m_chartType);

				return LocalizationManager.LocalizeString(id, msg,
					"Status bar message displayed when building a consonant or vowel chart.",
					App.kLocalizationGroupInfoMsg, LocalizationCategory.GeneralMessage,
					LocalizationPriority.Medium);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string TempFileName
		{
			get { return Path.ChangeExtension(m_outputFileName, "tmp"); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool KeepTempFile
		{
			get { return Settings.Default.KeepTempCVChartBuilderFile; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override Pipeline.ProcessType ProcessType
		{
			get { return Pipeline.ProcessType.ViewCVChart; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void WriteRootAttributes()
		{
			base.WriteRootAttributes();
			m_writer.WriteAttributeString("view", m_chartType + " Chart");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void PostBuildUpdate()
		{
		}
	}
}
