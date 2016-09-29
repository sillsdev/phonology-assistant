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
	public partial class ConsonantChartVw : ChartVwBase
	{
		/// ------------------------------------------------------------------------------------
		public ConsonantChartVw(PaProject project) : base(project)
		{
			try
			{
				File.Delete(Project.ProjectPathFilePrefix + "HtmlVwConsonantChart.html");
			}
			catch { }
			
			InitializeComponent();
			Name = "ConsonantChartVw";
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Properties.Settings.Default.ConsonantChartColHdrHeight = _chartGrid.ColumnHeadersHeight;
			Properties.Settings.Default.ConsonantChartRowHdrWidth = _chartGrid.RowHeadersWidth;
			Properties.Settings.Default.HtmlConsonantChartVisible = (_htmlVw != null && _htmlVw.Visible);
			
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		public override string LocalizationStringIdPrefix
		{
			get { return "Views.ConsonantChart.Labels."; }
		}

		/// ------------------------------------------------------------------------------------
		public override CVChartType ChartType
		{
			get { return CVChartType.Consonant; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShowHtmlChartWhenViewLoaded
		{
			get { return Properties.Settings.Default.HtmlConsonantChartVisible; }
			set { Properties.Settings.Default.HtmlConsonantChartVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override IPASymbolType CharacterType
		{
			get { return IPASymbolType.consonant; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float SplitterRatioSetting
		{
			get { return Properties.Settings.Default.ConsonantChartVwSplitRatio; }
			set { Properties.Settings.Default.ConsonantChartVwSplitRatio = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool HistogramVisibleSetting
		{
			get { return Properties.Settings.Default.ConsonantChartVwHistogramPaneVisible; }
			set { Properties.Settings.Default.ConsonantChartVwHistogramPaneVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Color ChartGridColor
		{
			get { return Properties.Settings.Default.ConsonantChartGridColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int ColumnHeaderHeight
		{
			get { return Properties.Settings.Default.ConsonantChartColHdrHeight; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int RowHeaderWidth
		{
			get { return Properties.Settings.Default.ConsonantChartRowHdrWidth; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string LayoutFile
		{
			get { return Project.ConsonantChartLayoutFile; }
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultHTMLOutputFile
		{
			get { return LocalizationManager.GetString("Views.ConsonantChart.DefaultHtmlExportFileAffix", "{0}-ConsonantChart.html"); }
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultWordXmlOutputFile
		{
			get { return LocalizationManager.GetString("Views.ConsonantChart.DefaultWordXmlExportFileAffix", "{0}-ConsonantChart-(Word).xml"); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string DefaultXLingPaperOutputFile
		{
			get { return LocalizationManager.GetString("Views.ConsonantChart.DefaultXLingPaperExportFileAffix", "{0}-ConsonantChart-(XLingPaper).xml"); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string CreateHtmlViewFile()
		{
			var outputFile = Project.ProjectPathFilePrefix + "HtmlVwConsonantChart.html";
			return (CVChartExporter.ToHtml(Project, CVChartType.Consonant, outputFile,
				_chartGrid, false, false) ? outputFile : string.Empty);
		}
	}
}