using System.Drawing;
using System.IO;
using SIL.Localization;
using SIL.Pa.Model;
using SIL.Pa.Processing;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;

namespace SIL.Pa.UI.Views
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ConsonantChartVw : ChartVwBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Settings.Default.ConsonantChartColHdrHeight = m_chartGrid.ColumnHeadersHeight;
			Settings.Default.ConsonantChartRowHdrWidth = m_chartGrid.RowHeadersWidth;
			Settings.Default.HtmlConsonantChartVisible = m_htmlVw.Visible;
			
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string InitializationMessage
		{
			get
			{
				return LocalizationManager.LocalizeString("InitializingConsonantChartViewMsg",
					"Initializing Consonant Chart View...",
					"Message displayed whenever the consonant chart is being initialized.",
					App.kLocalizationGroupInfoMsg, LocalizationCategory.GeneralMessage,
					LocalizationPriority.Medium);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override Color ChartGridColor
		{
			get { return Settings.Default.ConsonantChartGridColor; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override int ColumnHeaderHeight
		{
			get { return Settings.Default.ConsonantChartColHdrHeight; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override int RowHeaderWidth
		{
			get { return Settings.Default.ConsonantChartRowHdrWidth; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string LayoutFile
		{
			get { return App.Project.ProjectPathFilePrefix + "ConsonantChart.xml"; }
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
				return LocalizationManager.LocalizeString(
					"DefaultConsonantChartHtmlExportFileAffix", "{0}-ConsonantChart.html");
			}
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		protected override string DefaultWordXmlOutputFile
		{
			get
			{
				return LocalizationManager.LocalizeString(
					"DefaultConsonantChartWordXmlExportFileAffix", "{0}-ConsonantChart.xml");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string CreateHtmlViewFile()
		{
			var outputFile = App.Project.ProjectPathFilePrefix + "HtmlVwConsonantChart.html";
			return (CVChartExporter.ToHtml(App.Project, CVChartType.Consonant, outputFile,
				m_chartGrid, false) ? outputFile : string.Empty);
		}
	}
}