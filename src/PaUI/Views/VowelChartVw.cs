using SIL.Pa.Model;

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
			InitializeComponent();
			Name = "VowelChartVw";
			m_defaultHTMLOutputFile = Properties.Resources.kstidVowChartHTMLFileName;
			m_htmlChartName = Properties.Resources.kstidVowChartHTMLChartType;
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
	}
}