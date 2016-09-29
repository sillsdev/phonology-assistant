// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Drawing;
using System.IO;
using L10NSharp;
using SIL.Pa.Model;
using SIL.Pa.Processing;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Views
{
	/// ----------------------------------------------------------------------------------------
	public partial class VowelChartVw : ChartVwBase
	{
		/// ------------------------------------------------------------------------------------
		public VowelChartVw(PaProject project) : base(project)
		{
			try
			{
				File.Delete(Project.ProjectPathFilePrefix + "HtmlVwVowelChart.html");
			}
			catch { }

			InitializeComponent();
			Name = "VowelChartVw";
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Properties.Settings.Default.VowelChartColHdrHeight = _chartGrid.ColumnHeadersHeight;
			Properties.Settings.Default.VowelChartRowHdrWidth = _chartGrid.RowHeadersWidth;
			Properties.Settings.Default.HtmlVowelChartVisible = (_htmlVw != null && _htmlVw.Visible);

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		public override string LocalizationStringIdPrefix
		{
			get { return "Views.VowelChart.Labels."; }
		}

		/// ------------------------------------------------------------------------------------
		public override CVChartType ChartType
		{
			get { return CVChartType.Vowel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShowHtmlChartWhenViewLoaded
		{
			get { return Properties.Settings.Default.HtmlVowelChartVisible; }
			set { Properties.Settings.Default.HtmlVowelChartVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override IPASymbolType CharacterType
		{
			get { return IPASymbolType.vowel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float SplitterRatioSetting
		{
			get { return Properties.Settings.Default.VowelChartVwSplitRatio; }
			set { Properties.Settings.Default.VowelChartVwSplitRatio = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool HistogramVisibleSetting
		{
			get { return Properties.Settings.Default.VowelChartVwHistogramPaneVisible; }
			set { Properties.Settings.Default.VowelChartVwHistogramPaneVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Color ChartGridColor
		{
			get { return Properties.Settings.Default.VowelChartGridColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string LayoutFile
		{
			get { return Project.VowelChartLayoutFile; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int ColumnHeaderHeight
		{
			get { return Properties.Settings.Default.VowelChartColHdrHeight; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int RowHeaderWidth
		{
			get { return Properties.Settings.Default.VowelChartRowHdrWidth; }
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultHTMLOutputFile
		{
			get { return LocalizationManager.GetString("Views.VowelChart.DefaultHtmlExportFileAffix", "{0}-VowelChart.html"); }
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultWordXmlOutputFile
		{
			get { return LocalizationManager.GetString("Views.VowelChart.DefaultWordXmlExportFileAffix", "{0}-VowelChart-(Word).xml"); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string DefaultXLingPaperOutputFile
		{
			get { return LocalizationManager.GetString("Views.VowelChart.DefaultXLingPaperExportFileAffix", "{0}-VowelChart-(XLingPaper).xml"); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string CreateHtmlViewFile()
		{
			var outputFile = Project.ProjectPathFilePrefix + "HtmlVwVowelChart.html";
			return (CVChartExporter.ToHtml(Project, CVChartType.Vowel, outputFile,
				_chartGrid, false, false) ? outputFile : string.Empty);
		}
	}
}