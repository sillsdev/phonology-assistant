using SIL.Pa.Data;

namespace SIL.Pa
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
			Name = "vowelChartWnd";
			m_defaultHTMLOutputFile = Properties.Resources.kstidVowChartHTMLFileName;
			m_htmlChartName = Properties.Resources.kstidVowChartHTMLChartType;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Derived classes must override this.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override IPACharacterType CharacterType
		{
			get { return IPACharacterType.Vowel; }
		}
	}
}