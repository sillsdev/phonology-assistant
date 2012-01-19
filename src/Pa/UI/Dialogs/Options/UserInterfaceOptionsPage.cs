using System.Globalization;
using System.Linq;
using Localization;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class UserInterfaceOptionsPage : OptionsDlgPageBase
	{
		/// ------------------------------------------------------------------------------------
		public UserInterfaceOptionsPage(PaProject project)
			: base(project)
		{
			InitializeComponent();

			lblUILanguage.Font = FontHelper.UIFont;
			cboUILanguage.Font = FontHelper.UIFont;
			cboUILanguage.SelectedItem = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			_linkShowLocalizationDialogBox.Font = FontHelper.UIFont;

			_linkShowLocalizationDialogBox.LinkClicked += delegate
			{
				LocalizationManager.ShowLocalizationDialogBox();
				cboUILanguage.RefreshList();
			};
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return LocalizationManager.GetString("DialogBoxes.OptionsDlg.UserInterfaceTab.TabText", "User Interface"); }
		}

		/// ------------------------------------------------------------------------------------
		public override string HelpId
		{
			get { return ""; }
		}
			
		/// ------------------------------------------------------------------------------------
		public override bool IsDirty
		{
			get
			{
				if (cboUILanguage.SelectedItem == null)
					return false;

				return (LocalizationManager.UILanguageId != ((CultureInfo)cboUILanguage.SelectedItem).Name);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override void Save()
		{
			var newLangId = ((CultureInfo)cboUILanguage.SelectedItem).Name;
			Settings.Default.UserInterfaceLanguage = newLangId;
			LocalizationManager.SetUILanguage(newLangId, true);
			PaFieldDisplayProperties.ResetDisplayNameCache();
			App.MsgMediator.SendMessage("UserInterfaceLangaugeChanged", null);
		}
	}
}
