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
			Settings.Default.VowelChartColHdrHeight = m_chartGrid.ColumnHeadersHeight;
			Settings.Default.VowelChartRowHdrWidth = m_chartGrid.RowHeadersWidth;
			Settings.Default.HtmlVowelChartVisible = m_htmlVw.Visible;

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
		protected override string InitializationMessage
		{
			get
			{
				return App.LocalizeString("InitializingVowelChartViewMsg",
					"Initializing Vowel Chart View...",
					"Message displayed whenever the consonant chart is being initialized.");
			}
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string LayoutFile
		{
			get { return App.Project.ProjectPathFilePrefix + "VowelChartBeta.xml"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override int ColumnHeaderHeight
		{
			get { return Settings.Default.VowelChartColHdrHeight; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override int RowHeaderWidth
		{
			get { return Settings.Default.VowelChartRowHdrWidth; }
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		protected override string DefaultHTMLOutputFile
		{
			get
			{
				return App.LocalizeString("DefaultVowelChartHtmlExportFileAffix",
					"{0}-VowelChart.html", "Export");
			}
		}

		/// --------------------------------------------------------------------------------------------
		protected override string DefaultWordXmlOutputFile
		{
			get
			{
				return App.LocalizeString("DefaultVowelChartWordXmlExportFileAffix",
					"{0}-VowelChart-(Word).xml", "Export");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string DefaultXLingPaperOutputFile
		{
			get
			{
				return App.LocalizeString("DefaultVowelChartXLingPaperFileAffix",
					"{0}-VowelChart-(XLingPaper).xml", "Export");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string CreateHtmlViewFile()
		{
			var outputFile = App.Project.ProjectPathFilePrefix + "HtmlVwVowelChart.html";
			return (CVChartExporter.ToHtml(App.Project, CVChartType.Vowel, outputFile,
				m_chartGrid, false, false) ? outputFile : string.Empty);
		}
	}
}