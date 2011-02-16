using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg : OKCancelDlgBase
	{
		private readonly Dictionary<TabPage, string> m_tabPageHelpTopicIds;
		private readonly PaProject m_project;

		/// ------------------------------------------------------------------------------------
		public OptionsDlg()
		{
			App.InitializeProgressBar(Properties.Resources.kstidLoadingOptionsProgressBarText, 9);

			Utils.WaitCursors(true);
			InitializeComponent();

			if (DesignMode)
			{
				Utils.WaitCursors(false);
				return;
			}

			// Remove this until we implement it.
			tabOptions.TabPages.Remove(tpgColors);

			// Remove this for now. We may never use it, but
			// I'm hesitant to yank all the code just yet.
			tabOptions.TabPages.Remove(tpgFindPhones);
		}

		/// ------------------------------------------------------------------------------------
		public OptionsDlg(PaProject project) : this()
		{
			m_project = project;

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
		protected override void OnShown(EventArgs e)
		{
			Utils.WaitCursors(false);
			base.OnShown(e);
			App.UninitializeProgressBar();
		}

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
		protected override void SaveSettings()
		{
			SaveFontTabSettings();
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save any changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			SaveFontTabChanges();
			//SaveFindPhonesTabSettings();
			SaveWordListTabChanges();
			SaveRecViewTabChanges();
			SaveSortingTabChanges();
			SaveCvPatternsTabChanges();
			SaveUserInterfaceTabChanges();

			m_project.Save();
			Settings.Default.Save();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			if (m_tabPageHelpTopicIds.ContainsKey(tabOptions.SelectedTab))
				App.ShowHelpTopic(m_tabPageHelpTopicIds[tabOptions.SelectedTab]);
		}
	}
}
