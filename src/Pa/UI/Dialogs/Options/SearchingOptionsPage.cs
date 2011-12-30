using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class SearchingOptionsPage : OptionsDlgPageBase
	{
		/// ------------------------------------------------------------------------------------
		public SearchingOptionsPage(PaProject project) : base(project)
		{
			InitializeComponent();

			_checkBoxSearchEngineSelction.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return "Searching"; }
			//get { return LocalizationManager.GetString("DialogBoxes.OptionsDlg.UserInterfaceTab.TabText", "User Interface"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return ""; }
		}
			
		/// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get { return _checkBoxSearchEngineSelction.Checked != Settings.Default.ShowSearchEngineChoiceButtons ; }
		}

		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			Settings.Default.ShowSearchEngineChoiceButtons = _checkBoxSearchEngineSelction.Checked;
			App.MsgMediator.SendMessage("ShowSearchSelectionButtonsChagned", null);
		}
	}
}
