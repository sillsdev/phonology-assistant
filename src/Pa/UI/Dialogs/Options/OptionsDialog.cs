using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SIL.Pa.Properties;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg : OKCancelDlgBase
	{
		private readonly Dictionary<TabPage, string> m_tabPageHelpTopicIds;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OptionsDlg()
		{
			App.InitializeProgressBar(Properties.Resources.kstidLoadingOptionsProgressBarText, 9);

			Utils.WaitCursors(true);
			InitializeComponent();

			if (DesignMode)
				return;

			// Remove this until we implement it.
			tabOptions.TabPages.Remove(tpgColors);

			// Remove this for now. We may never use it, but
			// I'm hesitant to yank all the code just yet.
			tabOptions.TabPages.Remove(tpgFindPhones);

			App.IncProgressBar();
			InitializeFontTab();
			App.IncProgressBar();
			//InitializeFindPhonesTab();
			App.IncProgressBar();
			InitializeWordListTab();
			App.IncProgressBar();
			InitializeRecViewTab();
			App.IncProgressBar();
			InitializeCVPatternsTab();
			App.IncProgressBar();
			InitializeSortingTab();
			App.IncProgressBar();
			InitializeUserInterfaceTab();
			App.IncProgressBar();

			App.SettingsHandler.LoadFormProperties(this, true);

			tabOptions.Font = FontHelper.UIFont;
			lblSaveInfo.Font = FontHelper.UIFont;
			lblSaveInfo.Top = (tblLayoutButtons.Height - lblSaveInfo.Height) / 2;
			picSaveInfo.Top = lblSaveInfo.Top;

			App.IncProgressBar();
			m_tabPageHelpTopicIds = new Dictionary<TabPage, string>();
			m_tabPageHelpTopicIds[tpgWordLists] = "hidWordListOptions";
			m_tabPageHelpTopicIds[tpgRecView] = "hidRecordViewOptions";
			m_tabPageHelpTopicIds[tpgCVPatterns] = "hidCVPatternsOptions";
			m_tabPageHelpTopicIds[tpgSorting] = "hidSortingOptions";
			m_tabPageHelpTopicIds[tpgFonts] = "hidFontsOptions";

			m_dirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Utils.WaitCursors(false);
			base.OnShown(e);
			App.UninitializeProgressBar();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the form's state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			App.SettingsHandler.SaveFormProperties(this);
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
				return (m_dirty || IsFontsTabDirty || /*IsFindPhoneTabDirty || */
					IsSortOrderTabDirty || IsRecViewTabDirty || IsWordListTabDirty ||
					IsUserInterfaceTabDirty);
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
			//SaveFindPhonesTabSettings();
			SaveWordListTabSettings();
			SaveRecViewTabSettings();
			SaveSortingTabSettings();
			SaveCvPatternsTabSettings();
			SaveUserInterfaceTabSettings();

			Settings.Default.Save();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			if (m_tabPageHelpTopicIds.ContainsKey(tabOptions.SelectedTab))
				App.ShowHelpTopic(m_tabPageHelpTopicIds[tabOptions.SelectedTab]);
		}
	}
}
