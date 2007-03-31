using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using SIL.Pa.Resources;
using SIL.Pa.Controls;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg : OKCancelDlgBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OptionsDlg()
		{
			InitializeComponent();
			
			// Remove this until we implement it.
			tabOptions.TabPages.Remove(tpgColors);

			Application.UseWaitCursor = true;
			InitializeFontTab();
			InitializeFindPhonesTab();
			InitializeWordListTab();
			InitializeRecViewTab();
			InitializeCVSyllablesTab();
			InitializeSortingTab();

			PaApp.SettingsHandler.LoadFormProperties(this);

			tabOptions.Font = FontHelper.UIFont;
			lblSaveInfo.Font = FontHelper.UIFont;

			lblSaveInfo.Top = (pnlButtons.Height - lblSaveInfo.Height) / 2;
			picSaveInfo.Top = lblSaveInfo.Top;

			m_dirty = false;
			Application.UseWaitCursor = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the form's state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get
			{
				return (m_dirty || IsFontsTabDirty || IsFindPhoneTabDirty ||
					IsSortOrderTabDirty || IsRecViewTabDirty || IsWordListTabDirty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save any changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			SaveFontTabSettings();
			SaveFindPhonesTabSettings();
			SaveWordListTabSettings();
			SaveRecViewTabSettings();
			SaveSortingTabSettings();
			SaveCvSyllablesTabSettings();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the visible state of the information message and icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabOptions_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool saveInfoVisible = (tabOptions.SelectedTab.Tag != null &&
				(bool)tabOptions.SelectedTab.Tag);

			picSaveInfo.Visible = saveInfoVisible;
			lblSaveInfo.Visible = saveInfoVisible;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			string helpId = null;

			switch (tabOptions.SelectedIndex)
			{
				case 0: helpId = "hidWordListOptions"; break;
				case 1: helpId = "hidRecordViewOptions"; break;
				case 2: helpId = "hidSearchPatternOptions"; break;
				case 3: helpId = "hidCVSyllablesOptions"; break;
				case 4: helpId = "hidSortingOptions"; break;
				case 5: helpId = "hidFontsOptions"; break;
			}

			PaApp.ShowHelpTopic(helpId);
		}
	}
}
