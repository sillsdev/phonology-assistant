using System.Linq;
using System.Globalization;
using Localization;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		/// ------------------------------------------------------------------------------------
		private void InitializeUserInterfaceTab()
		{
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
		/// <summary>
		/// Saves the values on the word list tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveUserInterfaceTabChanges()
		{
			if (!IsUserInterfaceTabDirty)
				return;

			string newLangId = ((CultureInfo)cboUILanguage.SelectedItem).Name;
			Settings.Default.UserInterfaceLanguage = newLangId;
			LocalizationManager.UILanguageId = newLangId;
			App.MsgMediator.SendMessage("UserInterfaceLangaugeChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsUserInterfaceTabDirty
		{
			get
			{
				if (cboUILanguage.SelectedItem == null)
					return false;

				return (LocalizationManager.UILanguageId != ((CultureInfo)cboUILanguage.SelectedItem).Name);
			}
		}
	}
}