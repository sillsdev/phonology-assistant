using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		private SilGrid m_fontGrid;
		private bool m_fontChanged;

		/// ------------------------------------------------------------------------------------
		private void InitializeFontTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (App.Project == null)
			{
				tabOptions.TabPages.Remove(tpgFonts);
				return;
			}

			m_fontGrid = new SilGrid();
			m_fontGrid.Name = "OptionsFontsGrid";
			m_fontGrid.Dock = DockStyle.Fill;
			m_fontGrid.RowHeadersVisible = false;
			m_fontGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

			AddColumns();
			LoadFonts();

			tpgFonts.Controls.Add(m_fontGrid);
			m_fontGrid.BringToFront();
			((DataGridViewComboBoxColumn)m_fontGrid.Columns["font"]).DropDownWidth = 250;

			if (Settings.Default.OptionsDlgFontGrid != null)
				Settings.Default.OptionsDlgFontGrid.InitializeGrid(m_fontGrid);
			else
			{
				m_fontGrid.AutoResizeColumns();
				m_fontGrid.AutoResizeColumnHeadersHeight();
				m_fontGrid.AutoResizeRows();
				m_fontGrid.Columns["font"].Width = 160;
				m_fontGrid.Columns["field"].Width = 110;
			}

			m_fontGrid.CurrentCellDirtyStateChanged += HandleFontsGridCurrentCellDirtyStateChanged;
			App.SetGridSelectionColors(m_fontGrid, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to fonts grid
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			// Add the field column.
			var col = SilGrid.CreateTextBoxColumn("field");
			col.ReadOnly = true;
			m_fontGrid.Columns.Add(col);
			App.LocalizeObject(m_fontGrid.Columns["field"],
				"OptionsDlg.FontsGridFieldColumnHeadingText", "Field",
				App.kLocalizationGroupDialogs);

			// Get all the installed fonts and add the font name column.
			var fontList = (new InstalledFontCollection()).Families.Select(f => f.Name);
			col = SilGrid.CreateDropDownListComboBoxColumn("font", fontList);
			m_fontGrid.Columns.Add(col);
			App.LocalizeObject(m_fontGrid.Columns["font"],
				"OptionsDlg.FontsGridFontNameColumnHeadingText", "Font Name",
				App.kLocalizationGroupDialogs);

			// Add the list of default point sizes and add the size column.
			var sizeList = new object[] {8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26, 28, 30, 36, 48, 72};
			col = SilGrid.CreateDropDownListComboBoxColumn("size", sizeList);
			m_fontGrid.Columns.Add(col);
			App.LocalizeObject(m_fontGrid.Columns["size"],
				"OptionsDlg.FontsGridFontSizeColumnHeadingText", "Size",
				App.kLocalizationGroupDialogs);

			// Add the bold check box column.
			col = SilGrid.CreateCheckBoxColumn("bold");
			m_fontGrid.Columns.Add(col);
			App.LocalizeObject(m_fontGrid.Columns["bold"],
				"OptionsDlg.FontsGridFontBoldColumnHeadingText", "Bold",
				App.kLocalizationGroupDialogs);

			// Add the italic check box column.
			col = SilGrid.CreateCheckBoxColumn("italic");
			m_fontGrid.Columns.Add(col);
			App.LocalizeObject(m_fontGrid.Columns["italic"],
				"OptionsDlg.FontsGridFontItalicColumnHeadingText", "Italic",
				App.kLocalizationGroupDialogs);

			// Add the sample column.
			col = SilGrid.CreateTextBoxColumn("sample");
			col.ReadOnly = true;
			m_fontGrid.Columns.Add(col);
			App.LocalizeObject(m_fontGrid.Columns["sample"],
				"OptionsDlg.FontsGridFontSampleColumnHeadingText", "Sample",
				App.kLocalizationGroupDialogs);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load grid of all the field fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFonts()
		{
			foreach (var field in App.Fields.Where(f => f.Font != null).OrderBy(f => f.DisplayIndexInGrid))
			{
				m_fontGrid.AddRow(new object[]
				{
					field,
					field.Font.Name,
					(int)field.Font.SizeInPoints,
					field.Font.Bold,
					field.Font.Italic,
					App.LocalizeString("OptionsDlg.FontsSampleText", "Abc123", App.kLocalizationGroupDialogs)
				});

				m_fontGrid.Rows[m_fontGrid.RowCount - 1].Tag = field.Font;
				m_fontGrid.Rows[m_fontGrid.RowCount - 1].Cells["sample"].Style.Font = field.Font;
			}
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Update the sample column with the new font and store the new font to be saved when the
		/// dialog closes.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		void HandleFontsGridCurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			var row = m_fontGrid.CurrentRow;
			var prevFont = row.Tag as Font;

			try
			{
				// Save the edit to the current cell.
				m_fontGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);

				// Determine what the new font's style should be.
				var style = ((bool)row.Cells["bold"].Value ? FontStyle.Bold : FontStyle.Regular);

				if ((bool)row.Cells["italic"].Value)
					style |= FontStyle.Italic;

				// Create a new font with the specified characteristics.
				var font = new Font((string)row.Cells["font"].Value,
					(int)row.Cells["size"].Value, style, GraphicsUnit.Point);

				// Update the sample cell to use the new font and save the new font.
				row.Cells["sample"].Style.Font = font;
				row.Tag = font;
				
				if (prevFont != null)
					prevFont.Dispose();
	
				m_fontChanged = true;
			}
			catch (ArgumentException err)
			{
				Utils.MsgBox(err.Message);

				if (prevFont == null)
					return;

				row.Cells["font"].Value = prevFont.Name;
				row.Cells["size"].Value = (int)prevFont.Size;
				row.Cells["bold"].Value = prevFont.Bold;
				row.Cells["italic"].Value = prevFont.Italic;
				row.Cells["sample"].Style.Font = prevFont;
				row.Tag = prevFont;
				m_fontGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}

			// Resize the current row so it adjusts to fit the new font.
			m_fontGrid.AutoResizeColumn(m_fontGrid.ColumnCount - 1);
		}

		/// ------------------------------------------------------------------------------------
		private bool IsFontsTabDirty
		{
			get { return m_fontChanged; }
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private bool VerifyFontInfo()
		//{
		//    foreach (DataGridViewRow row in m_fontGrid.Rows)
		//    {
		//        Font fnt = (Font)row.Tag;
				
		//        FontStyle style = (fnt.Bold ? FontStyle.Bold : FontStyle.Regular);
		//        if (fnt.Italic)
		//            style |= FontStyle.Italic;

		//        Font fntTest = new Font(fnt.FontFamily, fnt.SizeInPoints, style, GraphicsUnit.Point);
		//    }

		//    return true;
		//}

		/// ------------------------------------------------------------------------------------
		private void SaveFontTabSettings()
		{
			Settings.Default.OptionsDlgFontGrid = GridSettings.Create(m_fontGrid);
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

			if (!m_fontChanged)
				return;

			try
			{
				foreach (var row in m_fontGrid.GetRows().Where(r => r.Cells[0].Value is PaField))
					(row.Cells[0].Value as PaField).Font = (Font)row.Tag;

				App.Project.InitializeFontHelperFonts();

				// Since the fonts changed, delete the project's style sheet file. This will
				// force it to be recreated the next time something is exported that needs it.
				if (File.Exists(App.Project.CssFileName))
					File.Delete(App.Project.CssFileName);

				App.MsgMediator.SendMessage("PaFontsChanged", null);
			}
			catch {}
		}
	}
}