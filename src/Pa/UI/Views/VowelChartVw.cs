using System.Drawing;
using SIL.Pa.Model;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Views
{
	/// ----------------------------------------------------------------------------------------
	public partial class VowelChartVw : ChartVwBase
	{
		/// ------------------------------------------------------------------------------------
		public VowelChartVw(PaProject project) : base(project)
		{
			DeleteHtmlChartFile("HtmlVwVowelChart");
			_newChartGrid.SupraSegsToIgnore = project.VowChartSupraSegsToIgnore;
			
			InitializeComponent();
			Name = "VowelChartVw";
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Settings.Default.VowelChartColHdrHeight = _newChartGrid.ColumnHeadersHeight;
			Settings.Default.VowelChartRowHdrWidth = _newChartGrid.RowHeadersWidth;
			Settings.Default.HtmlVowelChartVisible = _htmlVw != null ? _htmlVw.Visible : false;

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveIgnoredSuprasegmentals(string ignoredSegments)
		{
			_project.VowChartSupraSegsToIgnore = ignoredSegments;
			DeleteHtmlChartFile("HtmlVwVowelChart");
			CVChartBuilder.Process(_project, CVChartType.Vowel);
		}

		/// ------------------------------------------------------------------------------------
		public override CVChartType ChartType
		{
			get { return CVChartType.Vowel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ShowHtmlChartWhenViewLoaded
		{
			get { return Settings.Default.HtmlVowelChartVisible; }
			set { Settings.Default.HtmlVowelChartVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override IPASymbolType CharacterType
		{
			get { return IPASymbolType.Vowel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float SplitterRatioSetting
		{
			get { return Settings.Default.VowelChartVwSplitRatio; }
			set { Settings.Default.VowelChartVwSplitRatio = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override bool HistogramVisibleSetting
		{
			get { return Settings.Default.VowelChartVwHistogramPaneVisible; }
			set { Settings.Default.VowelChartVwHistogramPaneVisible = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override Color ChartGridColor
		{
			get { return Settings.Default.VowelChartGridColor; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string LayoutFile
		{
			get { return _project.ProjectPathFilePrefix + "VowelChartBeta.xml"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int ColumnHeaderHeight
		{
			get { return Settings.Default.VowelChartColHdrHeight; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int RowHeaderWidth
		{
			get { return Settings.Default.VowelChartRowHdrWidth; }
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultHTMLOutputFile
		{
			get
			{
				return App.GetString("DefaultVowelChartHtmlExportFileAffix",
					"{0}-VowelChart.html", "Export");
			}
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultWordXmlOutputFile
		{
			get
			{
				return App.GetString("DefaultVowelChartWordXmlExportFileAffix",
					"{0}-VowelChart-(Word).xml", "Export");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string DefaultXLingPaperOutputFile
		{
			get
			{
				return App.GetString("DefaultVowelChartXLingPaperExportFileAffix",
					"{0}-VowelChart-(XLingPaper).xml", "Export");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string CreateHtmlViewFile()
		{
			var outputFile = _project.ProjectPathFilePrefix + "HtmlVwVowelChart.html";
			return (CVChartExporter.ToHtml(_project, CVChartType.Vowel, outputFile,
				_newChartGrid, false, false) ? outputFile : string.Empty);
		}
	}
}