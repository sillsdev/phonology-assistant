using SIL.Pa.Data;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ConsonantChartWnd : ChartWndBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ConsonantChartWnd() : base()
		{
			InitializeComponent();
			Name = "consonantChartWnd";
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