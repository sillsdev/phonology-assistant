using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	public partial class UndefinedPhoneticCharactersDlg : Form
	{
		private int m_defaultRowHeight;
		private readonly bool m_expandGroupsOpen;
		private readonly UndefinedPhoneticCharactersInfoList m_list;
		private readonly string m_infoFmt;
		private readonly string m_codepointColFmt =
			Properties.Resources.kstidUndefPhoneticChartsGridCodePointColFmt;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Show(string projectName)
		{
			Show(projectName, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Show(string projectName, bool forceShow)
		{
			if (!forceShow && (PaApp.Project != null && !PaApp.Project.ShowUndefinedCharsDlg))
				return;

			if (DataUtils.IPACharCache.UndefinedCharacters != null &&
				DataUtils.IPACharCache.UndefinedCharacters.Count > 0)
			{
				using (UndefinedPhoneticCharactersDlg dlg =	new UndefinedPhoneticCharactersDlg(
					projectName, DataUtils.IPACharCache.UndefinedCharacters))
				{
					if (PaApp.MainForm != null)
						PaApp.MainForm.AddOwnedForm(dlg);

					dlg.ShowDialog();

					if (PaApp.MainForm != null)
						PaApp.MainForm.RemoveOwnedForm(dlg);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Show(string projectName, UndefinedPhoneticCharactersInfoList list)
		{
			if (list != null && list.Count > 0)
			{
				using (UndefinedPhoneticCharactersDlg dlg = new UndefinedPhoneticCharactersDlg(projectName, list))
				{
					if (PaApp.MainForm != null)
						PaApp.MainForm.AddOwnedForm(dlg);

					dlg.ShowDialog();

					if (PaApp.MainForm != null)
						PaApp.MainForm.RemoveOwnedForm(dlg);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedPhoneticCharactersDlg()
		{
			InitializeComponent();

			m_infoFmt = lblInfo.Text;
			lblInfo.Font = FontHelper.UIFont;
			chkShowUndefinedCharDlg.Font = FontHelper.UIFont;
			chkIgnoreInSearches.Font = FontHelper.UIFont;

			Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
			Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;

			PaApp.SettingsHandler.LoadFormProperties(this);
			AddColumns();
			CalcRowHeight();

			m_grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

			if (PaApp.Project != null)
			{
				chkShowUndefinedCharDlg.Checked = PaApp.Project.ShowUndefinedCharsDlg;
				chkIgnoreInSearches.Checked = PaApp.Project.IgnoreUndefinedCharsInSearches;
			}

			m_expandGroupsOpen =
				PaApp.SettingsHandler.GetBoolSettingsValue(Name, "expandonopen", false);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedPhoneticCharactersDlg(string projectName,
			UndefinedPhoneticCharactersInfoList list) : this()
		{
			m_list = list;
			lblInfo.Text = string.Format(m_infoFmt, projectName, Application.ProductName);
			LoadGrid(list);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to fonts grid
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

			// Add the Unicode number column.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("codepoint");
			col.HeaderText = STUtils.ConvertLiteralNewLines(Properties.Resources.kstidUnicodeNumHdg);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			m_grid.Columns.Add(col);

			// Add the sample column.
			col = SilGrid.CreateTextBoxColumn("character");
			col.HeaderText = STUtils.ConvertLiteralNewLines(Properties.Resources.kstidCharHdg);
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			m_grid.Columns.Add(col);

			// Add the sample column.
			col = SilGrid.CreateTextBoxColumn("word");
			col.HeaderText = STUtils.ConvertLiteralNewLines(Properties.Resources.kstidWordHdg);
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			m_grid.Columns.Add(col);

			// Add the reference column.
			col = SilGrid.CreateTextBoxColumn("reference");
			col.HeaderText = STUtils.ConvertLiteralNewLines(Properties.Resources.kstidReferenceHdg);
			m_grid.Columns.Add(col);

			// Add the data source column.
			col = SilGrid.CreateTextBoxColumn("datasource");
			col.HeaderText = STUtils.ConvertLiteralNewLines(Properties.Resources.kstidDataSourceHdg);
			m_grid.Columns.Add(col);

			m_grid.Name = Name + "Grid";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CalcRowHeight()
		{
			m_defaultRowHeight = 0;

			foreach (DataGridViewColumn col in m_grid.Columns)
			{
				m_defaultRowHeight = Math.Max(m_defaultRowHeight,
					(col.DefaultCellStyle.Font ?? FontHelper.UIFont).Height);
			}

			// Add a little vertical padding.
			m_defaultRowHeight += 4;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadGrid(UndefinedPhoneticCharactersInfoList list)
		{
			STUtils.WaitCursors(true);
			m_grid.Rows.Clear();
			m_grid.RowCount = list.Count;

			// Save in each row the index of it's entry in the undefined char. list
			// because the row index won't do it for us after the grid is grouped.
			for (int i = 0; i < m_grid.RowCount; i++)
				m_grid.Rows[i].Tag = i;

			m_grid.AutoResizeColumns();

			// Add a couple of pixels because I observed the auto sizing comes up a couple
			// pixels short when certain size fonts are used in the column headers.
			m_grid.Columns[0].Width += 2;
			PaApp.SettingsHandler.LoadGridProperties(m_grid);
			
			m_list.Sort(CompareUndefinedCharValues);

			Group();

			if (!m_expandGroupsOpen && m_grid.RowCount <= 5000)
				CollapseGroups();

			m_grid.CurrentCell = m_grid[0, 0];
			STUtils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int CompareUndefinedCharValues(UndefinedPhoneticCharactersInfo x,
			UndefinedPhoneticCharactersInfo y)
		{
			if (x == null && y == null)
				return 0;

			if (x == null)
				return -1;

			if (y == null)
				return 1;

			return (x.Character.CompareTo(y.Character));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups rows according to the unicode value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Group()
		{
			SilHierarchicalGridRow shgrow;
			Font fnt = FontHelper.MakeFont(FontHelper.PhoneticFont, FontStyle.Bold);
			int lastChild = m_list.Count - 1;
			char prevChar = m_list[lastChild].Character;
			string fmt = Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgFmt;
			string[] countFmt = new string[] {
				Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgCountFmtLong,
				Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgCountFmtMed,
				Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgCountFmtShort};

			m_grid.SuspendLayout();

			// Go from the bottom of the grid up, finding where the values differ from
			// one row to the next. At those boundaries, a hierarchical row is inserted.
			for (int i = m_list.Count - 1; i >= 0; i--)
			{
				if (prevChar != m_list[i].Character)
				{
					shgrow = new SilHierarchicalGridRow(m_grid,
						string.Format(fmt, prevChar, (int)prevChar), fnt, i + 1, lastChild);

					shgrow.CountFormatStrings = countFmt;
					m_grid.Rows.Insert(i + 1, shgrow);
					prevChar = m_list[i].Character;
					lastChild = i;
				}
			}

			// Insert the first group heading row.
			shgrow = new SilHierarchicalGridRow(m_grid,
				string.Format(fmt, prevChar, (int)prevChar), fnt, 0, lastChild);

			shgrow.CountFormatStrings = countFmt;
			m_grid.Rows.Insert(0, shgrow);

			// Insert a hierarchical column for the + and - glpyhs.
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
			m_grid.ResumeLayout();

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				shgrow = row as SilHierarchicalGridRow;
				if (shgrow != null)
					shgrow.SubscribeToOwningGridEvents();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CollapseGroups()
		{
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row is SilHierarchicalGridRow)
					((SilHierarchicalGridRow)row).SetExpandedState(false, false);
			}
		}

		#region Event handlers and overrides
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowHeightInfoNeeded(object sender, DataGridViewRowHeightInfoNeededEventArgs e)
		{
			e.Height = m_defaultRowHeight;
			e.MinimumHeight = 10;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			e.Value = null;
			int row = e.RowIndex;

			if (e.ColumnIndex <= 0 || row < 0 || m_grid.Rows[row] is SilHierarchicalGridRow ||
				m_grid.Rows[row].Tag == null || m_grid.Rows[row].Tag.GetType() != typeof(int))
			{
				return;
			}

			int i = (int)m_grid.Rows[row].Tag;

			switch (e.ColumnIndex)
			{
				case 1: e.Value = string.Format("U+{0:X4}", (int)m_list[i].Character); break;
				case 2: e.Value = m_list[i].Character; break;
				case 3: e.Value = m_list[i].Transcription; break;
				case 4: e.Value = m_list[i].Reference; break;
				case 5: e.Value = m_list[i].SourceName; break;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			PaApp.MsgMediator.SendMessage(Name + "HandleCreated", this);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			PaApp.SettingsHandler.SaveGridProperties(m_grid);

			if (PaApp.Project != null)
			{
				if (PaApp.Project.ShowUndefinedCharsDlg != chkShowUndefinedCharDlg.Checked ||
					PaApp.Project.IgnoreUndefinedCharsInSearches != chkIgnoreInSearches.Checked)
				{
					PaApp.Project.ShowUndefinedCharsDlg = chkShowUndefinedCharDlg.Checked;
					PaApp.Project.IgnoreUndefinedCharsInSearches = chkIgnoreInSearches.Checked;
					PaApp.Project.Save();
				}
			}
			
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resize the information label when it's size 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			using (Graphics g = lblInfo.CreateGraphics())
			{
				TextFormatFlags flags = TextFormatFlags.NoClipping | TextFormatFlags.WordBreak;
				Size propSz = new Size(lblInfo.Width, int.MaxValue);
				Size sz = TextRenderer.MeasureText(g, lblInfo.Text, lblInfo.Font, propSz, flags);
				lblInfo.Height = sz.Height + 6;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show some temp. help until the help files are ready.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			PaApp.ShowHelpTopic(this);
		}

		#endregion
	}
}