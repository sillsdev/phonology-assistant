using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
		private readonly Dictionary<TabPage, string> m_tabPageHelpTopicIds;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public OptionsDlg()
		{
			PaApp.InitializeProgressBar(Properties.Resources.kstidLoadingOptionsProgressBarText, 8);

			STUtils.WaitCursors(true);
			InitializeComponent();
			
			// Remove this until we implement it.
			tabOptions.TabPages.Remove(tpgColors);

			// Remove this for now. We may never use it, but
			// I'm hesitant to yank all the code just yet.
			tabOptions.TabPages.Remove(tpgFindPhones);

			PaApp.IncProgressBar();
			InitializeFontTab();
			PaApp.IncProgressBar();
			//InitializeFindPhonesTab();
			PaApp.IncProgressBar();
			InitializeWordListTab();
			PaApp.IncProgressBar();
			InitializeRecViewTab();
			PaApp.IncProgressBar();
			InitializeCVSyllablesTab();
			PaApp.IncProgressBar();
			InitializeSortingTab();
			PaApp.IncProgressBar();

			PaApp.SettingsHandler.LoadFormProperties(this);

			tabOptions.Font = FontHelper.UIFont;
			lblSaveInfo.Font = FontHelper.UIFont;
			lblSaveInfo.Top = (pnlButtons.Height - lblSaveInfo.Height) / 2;
			picSaveInfo.Top = lblSaveInfo.Top;

			PaApp.IncProgressBar();
			m_tabPageHelpTopicIds = new Dictionary<TabPage, string>();
			m_tabPageHelpTopicIds[tpgWordLists] = "hidWordListOptions";
			m_tabPageHelpTopicIds[tpgRecView] = "hidRecordViewOptions";
			m_tabPageHelpTopicIds[tpgCVSyllables] = "hidCVSyllablesOptions";
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
			STUtils.WaitCursors(false);
			base.OnShown(e);
			PaApp.UninitializeProgressBar();
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
				return (m_dirty || IsFontsTabDirty || /*IsFindPhoneTabDirty || */
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
			//SaveFindPhonesTabSettings();
			SaveWordListTabSettings();
			SaveRecViewTabSettings();
			SaveSortingTabSettings();
			SaveCvSyllablesTabSettings();
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
				PaApp.ShowHelpTopic(m_tabPageHelpTopicIds[tabOptions.SelectedTab]);
		}
	}
}
