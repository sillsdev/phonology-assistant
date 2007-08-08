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
		private readonly string m_infoFmt;
		
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
					dlg.ShowDialog();
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
					dlg.ShowDialog();
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

			if (PaApp.Project != null)
			{
				chkShowUndefinedCharDlg.Checked = PaApp.Project.ShowUndefinedCharsDlg;
				chkIgnoreInSearches.Checked = PaApp.Project.IgnoreUndefinedCharsInSearches;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedPhoneticCharactersDlg(string projectName, UndefinedPhoneticCharactersInfoList list) : this()
		{
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
		private void LoadGrid(UndefinedPhoneticCharactersInfoList list)
		{
			m_grid.Rows.Clear();

			foreach (UndefinedPhoneticCharactersInfo upci in list)
			{
				m_grid.Rows.Add(new object[] {
					string.Format("U+{0:X4}", (int)upci.Character), upci.Character,
					upci.Transcription, upci.Reference, upci.SourceName});
			}

			m_grid.AutoResizeColumns();
			m_grid.AutoResizeRows();

			// Add a couple of pixels because I observed the auto sizing comes up a couple
			// pixels short when certain size fonts are used in the column headers.
			m_grid.Columns[0].Width += 2;

			PaApp.SettingsHandler.LoadGridProperties(m_grid);
			m_grid.Sort(m_grid.Columns[0], ListSortDirection.Ascending);
			Group();
			m_grid.CurrentCell = m_grid[0, 0];

			if (PaApp.SettingsHandler.GetBoolSettingsValue(Name, "expandonopen", false))
				return;

			// Collapse all the groups.
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row is SilHierarchicalGridRow)
					((SilHierarchicalGridRow)row).SetExpandedState(false, true);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Groups rows according to the unicode value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Group()
		{
			SilHierarchicalGridRow row;
			Font fnt = FontHelper.MakeFont(FontHelper.PhoneticFont, FontStyle.Bold);
			int lastChild = m_grid.RowCount - 1;
			string prevCodepoint = m_grid[0, lastChild].Value as string;
			object prevChar = m_grid[1, lastChild].Value;
			string fmt = Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgFmt;
			string[] countFmt = new string[] {
				Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgCountFmtLong,
				Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgCountFmtMed,
				Properties.Resources.kstidUndefPhoneticCharsGridGroupHdgCountFmtShort};
			
			// Go from the bottom of the grid up, finding where the values differ from
			// one row to the next. At those boundaries, a hierarchical row is inserted.
			for (int i = m_grid.RowCount - 1; i >= 0; i--)
			{
				if (prevCodepoint != (m_grid[0, i].Value as string))
				{
					row = new SilHierarchicalGridRow(m_grid,
						string.Format(fmt, prevChar, prevCodepoint), fnt, i + 1, lastChild);

					row.CountFormatStrings = countFmt;
					m_grid.Rows.Insert(i + 1, row);
					prevCodepoint = m_grid[0, i].Value as string;
					prevChar = m_grid[1, i].Value;
					lastChild = i;
				}
			}

			// Insert the first group heading row.
			row = new SilHierarchicalGridRow(m_grid,
				string.Format(fmt, prevChar, prevCodepoint), fnt, 0, lastChild);

			row.CountFormatStrings = countFmt;
			m_grid.Rows.Insert(0, row);

			// Insert a hierarchical column for the + and - glpyhs.
			m_grid.Columns.Insert(0, new SilHierarchicalGridColumn());
		}

		#region Event handlers and overrides
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