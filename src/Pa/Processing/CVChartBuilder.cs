// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.IO;
using System.Xml;
using Localization;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

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
		protected CVChartType _chartType;

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
			_chartType = chartType;
			
			_outputFileName = (chartType == CVChartType.Consonant ?
				_project.ConsonantChartLayoutFile : _project.VowelChartLayoutFile);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		protected override string ProgressMessage
		{
			get
			{
				if (_chartType == CVChartType.Consonant)
				{
					return LocalizationManager.GetString("Views.ConsonantChart.BuildingConsonantChartStatusMsg",
						"Building Consonant Chart...", "Status bar message displayed when building consonant chart.");
				}

				return LocalizationManager.GetString("Views.VowelChart.BuildingVowelChartStatusMsg",
					"Building Vowel Chart...", "Status bar message displayed when building building vowel chart.");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string TempFileName
		{
			get
			{
				return ProcessHelper.MakeTempFilePath(_project,
					Path.ChangeExtension(_outputFileName, "tmp"));
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
		/// This method will create a temporary project inventory file for the purpose of
		/// building a C or V chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override object CreateInputToTransformPipeline()
		{
			// Read the current project inventory file and find the root node.
			var doc1 = new XmlDocument();
			doc1.Load(_project.ProjectInventoryFileName);
			var node = doc1.SelectSingleNode("inventory");

			// Add an attribute to the root node indicating the chart type.
			var attrib = doc1.CreateAttribute("view");
			attrib.Value = (_chartType == CVChartType.Consonant ? "Consonants" : "Vowels");
			node.Attributes.Append(attrib);

			// Save the temporary file.
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
