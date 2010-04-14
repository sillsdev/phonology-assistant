using System.Drawing;
using SIL.Localization;
using SIL.Pa.Model;
using SIL.Pa.Properties;

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
			InitializeComponent();
			Name = "ConsonantChartVw";
			m_defaultHTMLOutputFile = Properties.Resources.kstidConChartHTMLFileName;
			m_htmlChartName = Properties.Resources.kstidConChartHTMLChartType;
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
			
			base.OnHandleDestroyed(e);
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
	}
}