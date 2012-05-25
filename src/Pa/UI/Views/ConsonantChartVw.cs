using System.Drawing;
using System.IO;
using Localization;
using SIL.Pa.Model;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
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
			Settings.Default.ConsonantChartColHdrHeight = _chartGrid.ColumnHeadersHeight;
			Settings.Default.ConsonantChartRowHdrWidth = _chartGrid.RowHeadersWidth;
			Settings.Default.HtmlConsonantChartVisible = (_htmlVw != null && _htmlVw.Visible);
			
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
			get { return Settings.Default.HtmlConsonantChartVisible; }
			set { Settings.Default.HtmlConsonantChartVisible = value; }
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
			get { return Settings.Default.ConsonantChartVwSplitRatio; }
			set { Settings.Default.ConsonantChartVwSplitRatio = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool HistogramVisibleSetting
		{
			get { return Settings.Default.ConsonantChartVwHistogramPaneVisible; }
			set { Settings.Default.ConsonantChartVwHistogramPaneVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Color ChartGridColor
		{
			get { return Settings.Default.ConsonantChartGridColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int ColumnHeaderHeight
		{
			get { return Settings.Default.ConsonantChartColHdrHeight; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int RowHeaderWidth
		{
			get { return Settings.Default.ConsonantChartRowHdrWidth; }
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