using System.Drawing;
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
			DeleteHtmlChartFile("HtmlVwConsonantChart");
			_newChartGrid.SupraSegsToIgnore = project.ConChartSupraSegsToIgnore;

			InitializeComponent();
			Name = "ConsonantChartVw";
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Settings.Default.ConsonantChartColHdrHeight = _newChartGrid.ColumnHeadersHeight;
			Settings.Default.ConsonantChartRowHdrWidth = _newChartGrid.RowHeadersWidth;
			Settings.Default.HtmlConsonantChartVisible = _htmlVw.Visible;
			
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveIgnoredSuprasegmentals(string ignoredSegments)
		{
			_project.ConChartSupraSegsToIgnore = ignoredSegments;
			DeleteHtmlChartFile("HtmlVwConsonantChart");
			CVChartBuilder.Process(_project, CVChartType.Consonant);
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
			get { return IPASymbolType.Consonant; }
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
			get { return _project.ProjectPathFilePrefix + "ConsonantChartBeta.xml"; }
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultHTMLOutputFile
		{
			get
			{
				return App.GetString("DefaultConsonantChartHtmlExportFileAffix",
					"{0}-ConsonantChart.html", "Export");
			}
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultWordXmlOutputFile
		{
			get
			{
				return App.GetString("DefaultConsonantChartWordXmlExportFileAffix",
					"{0}-ConsonantChart-(Word).xml", "Export");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string DefaultXLingPaperOutputFile
		{
			get
			{
				return App.GetString("DefaultConsonantChartXLingPaperExportFileAffix",
					"{0}-ConsonantChart-(XLingPaper).xml", "Export");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string CreateHtmlViewFile()
		{
			var outputFile = _project.ProjectPathFilePrefix + "HtmlVwConsonantChart.html";
			return (CVChartExporter.ToHtml(_project, CVChartType.Consonant, outputFile,
				_newChartGrid, false, false) ? outputFile : string.Empty);
		}
	}
}