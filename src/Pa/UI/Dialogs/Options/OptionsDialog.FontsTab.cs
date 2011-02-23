using System.Linq;
using System.IO;
using System.Windows.Forms;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		private FieldMappingGrid m_fntGrid;

		/// ------------------------------------------------------------------------------------
		private void InitializeFontTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (m_project == null || m_project.Fields == null || m_project.Fields.Count() == 0)
			{
				tabOptions.TabPages.Remove(tpgFonts);
				return;
			}

			m_fntGrid = new FieldMappingGrid(m_project.Fields);
			m_fntGrid.Dock = DockStyle.Fill;
			pnlFonts.Controls.Add(m_fntGrid);

			if (Settings.Default.OptionsDlgFontGrid != null)
				Settings.Default.OptionsDlgFontGrid.InitializeGrid(m_fntGrid);
			else
			{
				m_fntGrid.AutoResizeColumnHeadersHeight();
				m_fntGrid.AutoResizeColumns();
				m_fntGrid.AutoResizeRows();
				m_fntGrid.Columns["tgtfield"].Width = 150;
				m_fntGrid.Columns["font"].Width = 250;
			}
		}

		/// ------------------------------------------------------------------------------------
		private bool IsFontsTabDirty
		{
			get { return m_fntGrid.IsDirty; }
		}

		/// ------------------------------------------------------------------------------------
		private void SaveFontTabSettings()
		{
			Settings.Default.OptionsDlgFontGrid = GridSettings.Create(m_fntGrid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves changed font information if needed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveFontTabChanges()
		{
			if (!IsFontsTabDirty)
				return;

			try
			{
				m_project.SetFields(m_fntGrid.Mappings.Select(m => m.Field), false);

				// Since the fonts changed, delete the project's style sheet file. This will
				// force it to be recreated the next time something is exported that needs it.
				if (File.Exists(m_project.CssFileName))
					File.Delete(m_project.CssFileName);

				App.MsgMediator.SendMessage("PaFontsChanged", null);
			}
			catch {}
		}
	}
}