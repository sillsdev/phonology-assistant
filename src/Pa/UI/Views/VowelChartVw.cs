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
	public partial class VowelChartVw : ChartVwBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			Settings.Default.VowelChartColHdrHeight = m_chartGrid.ColumnHeadersHeight;
			Settings.Default.VowelChartRowHdrWidth = m_chartGrid.RowHeadersWidth;
			Settings.Default.HtmlVowelChartVisible = m_htmlVw.Visible;

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string InitializationMessage
		{
			get
			{
				return LocalizationManager.LocalizeString("InitializingVowelChartViewMsg",
					"Initializing Vowel Chart View...",
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
			get { return Settings.Default.VowelChartGridColor; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string LayoutFile
		{
			get { return App.Project.ProjectPathFilePrefix + "VowelChart.xml"; }
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
				return LocalizationManager.LocalizeString(
					"DefaultVowelChartHtmlExportFileAffix", "{0}-VowelChart.html");
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
					"DefaultVowelChartWordXmlExportFileAffix", "{0}-VowelChart.xml");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string CreateHtmlViewFile()
		{
			var outputFile = App.Project.ProjectPathFilePrefix + "HtmlVwVowelChart.html";
			return (CVChartExporter.ToHtml(App.Project, CVChartType.Vowel, outputFile,
				m_chartGrid, false) ? outputFile : string.Empty);
		}
	}
}