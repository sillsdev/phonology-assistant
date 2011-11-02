using System.Drawing;
using System.IO;
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
		public VowelChartVw()
		{
			try
			{
				File.Delete(App.Project.ProjectPathFilePrefix + "HtmlVwVowelChart.html");
			}
			catch { }

			InitializeComponent();
			Name = "VowelChartVw";
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Settings.Default.VowelChartColHdrHeight = _chartGrid.ColumnHeadersHeight;
			Settings.Default.VowelChartRowHdrWidth = _chartGrid.RowHeadersWidth;
			Settings.Default.HtmlVowelChartVisible = _htmlVw.Visible;

			base.OnHandleDestroyed(e);
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
			get { return IPASymbolType.vowel; }
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
			get { return App.Project.ProjectPathFilePrefix + "VowelChartBeta.xml"; }
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
			var outputFile = App.Project.ProjectPathFilePrefix + "HtmlVwVowelChart.html";
			return (CVChartExporter.ToHtml(App.Project, CVChartType.Vowel, outputFile,
				_chartGrid, false, false) ? outputFile : string.Empty);
		}
	}
}