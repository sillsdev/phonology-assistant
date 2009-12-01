using SIL.Pa.Data;

namespace SIL.Pa
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
		protected override IPACharacterType CharacterType
		{
			get { return IPACharacterType.Consonant; }
		}
	}
}