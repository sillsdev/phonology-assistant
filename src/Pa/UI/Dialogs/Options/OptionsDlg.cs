using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg : OKCancelDlgBase
	{
		private readonly PaProject m_project;

		/// ------------------------------------------------------------------------------------
		public OptionsDlg()
		{
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

			foreach (var pageContent in GetNewTabPageContents())
			{
				var page = new TabPage(pageContent.TabPageText) { Padding = new Padding(12), UseVisualStyleBackColor = true };
				page.Controls.Add(pageContent);
				tabOptions.TabPages.Add(page);
			}

			tabOptions.Font = FontHelper.UIFont;
			lblSaveInfo.Font = FontHelper.UIFont;
			lblSaveInfo.Top = (tblLayoutButtons.Height - lblSaveInfo.Height) / 2;
			picSaveInfo.Top = lblSaveInfo.Top;

			m_dirty = false;
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<OptionsDlgPageBase> GetNewTabPageContents()
		{
			yield return new WordListsOptionsPage(m_project);
			yield return new RecordViewOptionsPage(m_project);
			yield return new SortingOptionsPage(m_project, GetSelectedWordListFields);
			yield return new CVPatternsOptionsPage(m_project);
			yield return new FontsOptionsPage(m_project);
			//yield return new UserInterfaceOptionsPage(m_project);

			if ((ModifierKeys & Keys.Control) == Keys.Control && (ModifierKeys & Keys.Alt) == Keys.Alt)
				yield return new SearchingOptionsPage(m_project);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<IOptionsDlgPage> GetTabPageContents()
		{
			return (from page in tabOptions.TabPages.Cast<TabPage>()
					where page.Controls.Count > 0
					select page.Controls[0]).OfType<IOptionsDlgPage>();
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<PaField> GetSelectedWordListFields()
		{
			var wrdListOptionsPage = GetTabPageContents().FirstOrDefault(pc => pc is WordListsOptionsPage);
			return (wrdListOptionsPage != null ?
				((WordListsOptionsPage)wrdListOptionsPage).GetSelectedFields() : new PaField[0]);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Utils.WaitCursors(false);
			base.OnShown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return GetTabPageContents().Any(pc => pc.IsDirty); }
		}

		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			foreach (var pageContent in GetTabPageContents())
				pageContent.SaveSettings();

			Settings.Default.Save();
			base.SaveSettings();
		}

		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			foreach (var pageContent in GetTabPageContents())
				pageContent.Save();

			m_project.Save();
			Settings.Default.Save();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleHelpClick(object sender, EventArgs e)
		{
			if (tabOptions.SelectedTab.Controls.Count > 0 && tabOptions.SelectedTab.Controls[0] is IOptionsDlgPage)
				App.ShowHelpTopic(((IOptionsDlgPage)tabOptions.SelectedTab.Controls[0]).HelpId);
		}
	}
}
