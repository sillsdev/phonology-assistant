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

			var allowedUILangs = from ci in CultureInfo.GetCultures(CultureTypes.AllCultures)
								 where ci.Name.Length == 2
								 orderby ci.DisplayName
								 select ci;

			foreach (var ci in allowedUILangs)
				cboUILanguage.Items.Add(ci);

			var currCulture = CultureInfo.GetCultureInfo(LocalizationManager.UILanguageId);
			cboUILanguage.SelectedItem = currCulture;
		}

		/// ------------------------------------------------------------------------------------
		public override string TabPageText
		{
			get { return App.GetString("DialogBoxes.OptionsDlg.UserInterfaceTab.TabText", "User Interface"); }
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
			LocalizationManager.UILanguageId = newLangId;
			PaFieldDisplayProperties.ResetDisplayNameCache();
			App.MsgMediator.SendMessage("UserInterfaceLangaugeChanged", null);
		}
	}
}
