using SIL.Pa.Model;

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
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override IPASymbolType CharacterType
		{
			get { return IPASymbolType.Consonant; }
		}
	}
}