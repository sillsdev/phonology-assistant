using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class SearchingOptionsPage : OptionsDlgPageBase
	{
		/// ------------------------------------------------------------------------------------
		public SearchingOptionsPage(PaProject project)
			: base(project)
		{
			InitializeComponent();

			_radioUseOldSearching.Font = FontHelper.UIFont;
			_radioUseRegExpSearching.Font = FontHelper.UIFont;

			_radioUseOldSearching.Checked = !Settings.Default.UseRegExpressionSearching;
			_radioUseRegExpSearching.Checked = Settings.Default.UseRegExpressionSearching;
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return "Searching"; }
			//get { return App.GetString("DialogBoxes.OptionsDlg.UserInterfaceTab.TabText", "User Interface"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return ""; }
		}
			
		/// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get { return _radioUseRegExpSearching.Checked != Settings.Default.UseRegExpressionSearching; }
		}

		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			Settings.Default.UseRegExpressionSearching = _radioUseRegExpSearching.Checked;
		}
	}
}
