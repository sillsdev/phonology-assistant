using System.Drawing;
using System.IO;
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
		public ConsonantChartVw()
		{
			try
			{
				File.Delete(App.Project.ProjectPathFilePrefix + "HtmlVwConsonantChart.html");
			}
			catch { }
			
			InitializeComponent();
			Name = "ConsonantChartVw";
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Settings.Default.ConsonantChartColHdrHeight = m_chartGrid.ColumnHeadersHeight;
			Settings.Default.ConsonantChartRowHdrWidth = m_chartGrid.RowHeadersWidth;
			Settings.Default.HtmlConsonantChartVisible = m_htmlVw.Visible;
			
			base.OnHandleDestroyed(e);
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
			get { return App.Project.ProjectPathFilePrefix + "ConsonantChartBeta.xml"; }
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
			var outputFile = App.Project.ProjectPathFilePrefix + "HtmlVwConsonantChart.html";
			return (CVChartExporter.ToHtml(App.Project, CVChartType.Consonant, outputFile,
				m_chartGrid, false, false) ? outputFile : string.Empty);
		}
	}
}