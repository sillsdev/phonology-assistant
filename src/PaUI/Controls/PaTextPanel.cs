using SilUtils;
using SilUtils.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Extends the panel control to support text, including text containing mnemonic
	/// specifiers.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaTextPanel : SilTextPanel
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaTextPanel()
		{
			Font = FontHelper.UIFont;
		}
	}
}
