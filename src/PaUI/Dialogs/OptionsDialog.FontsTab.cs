using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		private SilGrid m_fontGrid;
		private const string kSampleText = "Abc123";
		private bool m_fontChanged;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeFontTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (PaApp.Project == null)
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

			string gridLinesValue;
			if (!PaApp.SettingsHandler.LoadGridProperties(m_fontGrid, out gridLinesValue))
			{
				m_fontGrid.AutoResizeColumns();
				m_fontGrid.AutoResizeColumnHeadersHeight();
				m_fontGrid.AutoResizeRows();
				m_fontGrid.Columns["font"].Width = 160;
				m_fontGrid.Columns["field"].Width = 110;
			}

			m_fontGrid.CurrentCellDirtyStateChanged += m_fontGrid_CurrentCellDirtyStateChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to fonts grid
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			// Add the database field column.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("field");
			col.ReadOnly = true;
			col.HeaderText = Properties.Resources.kstidFontGridDBField;
			m_fontGrid.Columns.Add(col);

			// Get all the installed fonts and add the font name column.
			List<string> fontList = new List<string>();
			InstalledFontCollection installedFonts = new InstalledFontCollection();
			foreach (FontFamily family in installedFonts.Families)
				fontList.Add(family.Name);

			col = SilGrid.CreateDropDownListComboBoxColumn("font", fontList.ToArray());
			col.HeaderText = Properties.Resources.kstidFontGridFontName;
			m_fontGrid.Columns.Add(col);

			// Add the list of default point sizes and add the size column.
			object[] sizeList = new object[] {8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72};
			col = SilGrid.CreateDropDownListComboBoxColumn("size", sizeList);
			col.HeaderText = Properties.Resources.kstidFontGridSize;
			m_fontGrid.Columns.Add(col);

			// Add the bold check box column.
			col = SilGrid.CreateCheckBoxColumn("bold");
			col.HeaderText = Properties.Resources.kstidFontGridBold;
			m_fontGrid.Columns.Add(col);

			// Add the italic check box column.
			col = SilGrid.CreateCheckBoxColumn("italic");
			col.HeaderText = Properties.Resources.kstidFontGridItalic;
			m_fontGrid.Columns.Add(col);

			// Add the sample column.
			col = SilGrid.CreateTextBoxColumn("sample");
			col.ReadOnly = true;
			col.HeaderText = Properties.Resources.kstidFontGridSample;
			m_fontGrid.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load grid of all the field fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFonts()
		{
			foreach (PaFieldInfo fieldInfo in PaApp.Project.FieldInfo.SortedList)
			{
				if (fieldInfo.Font == null)
					continue;

				m_fontGrid.AddRow(new object[] {fieldInfo, fieldInfo.Font.Name,
					(int)fieldInfo.Font.SizeInPoints, fieldInfo.Font.Bold,
					fieldInfo.Font.Italic, kSampleText});

				m_fontGrid.Rows[m_fontGrid.RowCount - 1].Tag = fieldInfo.Font;
				m_fontGrid.Rows[m_fontGrid.RowCount - 1].Cells["sample"].Style.Font = fieldInfo.Font;
			}
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Update the sample column with the new font and store the new font to be saved when the
		/// dialog closes.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		void m_fontGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			DataGridViewRow row = m_fontGrid.CurrentRow;
			Font prevFont = row.Tag as Font;

			try
			{
				// Save the edit to the current cell.
				m_fontGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);

				// Determine what the new font's style should be.
				FontStyle style = ((bool)row.Cells["bold"].Value ?
					FontStyle.Bold : FontStyle.Regular);

				if ((bool)row.Cells["italic"].Value)
					style |= FontStyle.Italic;

				// Create a new font with the specified characteristics.
				Font font = new Font((string)row.Cells["font"].Value,
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
				SilUtils.Utils.MsgBox(err.Message);

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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// Saves changed font information if needed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveFontTabSettings()
		{
			if (!IsFontsTabDirty)
				return;

			PaApp.SettingsHandler.SaveGridProperties(m_fontGrid, null);

			if (!m_fontChanged)
				return;

			try
			{
				foreach (DataGridViewRow row in m_fontGrid.Rows)
				{
					PaFieldInfo fieldInfo = row.Cells[0].Value as PaFieldInfo;
					if (fieldInfo != null)
						fieldInfo.Font = (Font)row.Tag;
				}

				PaApp.Project.Save();
				PaApp.Project.InitializeFontHelperFonts();
				PaApp.MsgMediator.SendMessage("PaFontsChanged", null);
			}
			catch {}
		}
	}
}