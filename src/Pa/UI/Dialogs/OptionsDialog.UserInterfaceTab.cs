using System.Linq;
using System.Globalization;
using SIL.Localization;
using SIL.Pa.Properties;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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

			CultureInfo currCulture = CultureInfo.GetCultureInfo(LocalizationManager.UILangId);
			cboUILanguage.SelectedItem = currCulture;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values on the word list tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveUserInterfaceTabSettings()
		{
			if (!IsUserInterfaceTabDirty)
				return;

			string newLangId = ((CultureInfo)cboUILanguage.SelectedItem).Name;
			Settings.Default.UserInterfaceLanguage = newLangId;
			LocalizationManager.UILangId = newLangId;
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
				return (LocalizationManager.UILangId != ((CultureInfo)cboUILanguage.SelectedItem).Name);
			}
		}
	}
}