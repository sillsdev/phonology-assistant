using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Data;
using SilUtils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class UndefinedPhoneticCharactersDlg : Form
	{
		private int m_defaultRowHeight;
		private UndefinedPhoneticCharactersInfoList m_currUdpcil;
		private Dictionary<char, UndefinedPhoneticCharactersInfoList> m_udpciList;
		private readonly string m_infoFmt;
		private readonly string m_codepointColFmt =
			Properties.Resources.kstidUndefPhoneticChartsGridCodePointColFmt;

		private readonly string m_codepointHdgFmt;
		private readonly Color m_defaultSelectedRowForeColor;
		private readonly Color m_defaultSelectedRowBackColor;

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

			m_codepointHdgFmt = pgpWhere.Text;

			pgpChars.TextFormatFlags &= ~TextFormatFlags.HidePrefix;
			pgpWhere.TextFormatFlags &= ~TextFormatFlags.HidePrefix;

			m_udpciList = new Dictionary<char, UndefinedPhoneticCharactersInfoList>();
			m_infoFmt = lblInfo.Text;
			lblInfo.Font = FontHelper.UIFont;
			chkShowUndefinedCharDlg.Font = FontHelper.UIFont;
			chkIgnoreInSearches.Font = FontHelper.UIFont;
			pgpChars.Font = FontHelper.UIFont;
			pgpWhere.Font = FontHelper.UIFont;
			pgpChars.BorderStyle = BorderStyle.None;
			pgpWhere.BorderStyle = BorderStyle.None;

			AddColumnsToCharsGrid();
			AddColumnsToWhereGrid();
			CalcWhereGridRowHeight();

			m_defaultSelectedRowForeColor = m_gridChars.RowsDefaultCellStyle.ForeColor;
			m_defaultSelectedRowBackColor = m_gridChars.RowsDefaultCellStyle.BackColor;

			HandleGridEnter(m_gridChars, null);
			HandleGridLeave(m_gridWhere, null);

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
		public UndefinedPhoneticCharactersDlg(string projectName,
			UndefinedPhoneticCharactersInfoList list) : this()
		{
			list.Sort(CompareUndefinedCharValues);
			lblInfo.Text = string.Format(m_infoFmt, projectName, Application.ProductName);
			LoadCharGrid(list);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to fonts grid
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumnsToCharsGrid()
		{
			m_gridChars.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

			// Add the Unicode number column.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("codepoint");
			col.HeaderText = SilUtils.Utils.ConvertLiteralNewLines(Properties.Resources.kstidUnicodeNumHdg);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			m_gridChars.Columns.Add(col);

			// Add the sample column.
			col = SilGrid.CreateTextBoxColumn("char");
			col.HeaderText = SilUtils.Utils.ConvertLiteralNewLines(Properties.Resources.kstidCharHdg);
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			m_gridChars.Columns.Add(col);

			// Add the count number column.
			col = SilGrid.CreateTextBoxColumn("count");
			col.HeaderText = SilUtils.Utils.ConvertLiteralNewLines(Properties.Resources.kstidCountHdg);
			col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			m_gridChars.Columns.Add(col);

			m_gridChars.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			m_gridChars.AutoResizeColumnHeadersHeight();
			m_gridChars.Name = Name + "CharsGrid";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to fonts grid
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddColumnsToWhereGrid()
		{
			m_gridWhere.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			m_gridWhere.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("word");
			col.HeaderText = SilUtils.Utils.ConvertLiteralNewLines(Properties.Resources.kstidWordHdg);
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			m_gridWhere.Columns.Add(col);

			// Add the reference column.
			col = SilGrid.CreateTextBoxColumn("reference");
			col.HeaderText = SilUtils.Utils.ConvertLiteralNewLines(Properties.Resources.kstidReferenceHdg);
			m_gridWhere.Columns.Add(col);

			// Add the data source column.
			col = SilGrid.CreateTextBoxColumn("datasource");
			col.HeaderText = SilUtils.Utils.ConvertLiteralNewLines(Properties.Resources.kstidDataSourceHdg);
			m_gridWhere.Columns.Add(col);

			m_gridWhere.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			m_gridWhere.AutoResizeColumnHeadersHeight();
			m_gridWhere.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			m_gridWhere.Name = Name + "WhereGrid";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CalcWhereGridRowHeight()
		{
			m_defaultRowHeight = 0;

			foreach (DataGridViewColumn col in m_gridWhere.Columns)
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
		private void LoadCharGrid(UndefinedPhoneticCharactersInfoList list)
		{
			SilUtils.Utils.WaitCursors(true);
			m_gridChars.Rows.Clear();
			
			DataGridViewRow prevRow = null;
			int count = 0;
			char prevChar = (char)0;

			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (prevChar != list[i].Character)
				{
					if (prevRow != null)
						prevRow.Cells["count"].Value = count;

					count = 0;
					prevChar = list[i].Character;
					m_udpciList[prevChar] = new UndefinedPhoneticCharactersInfoList();
					m_gridChars.Rows.Insert(0, new object[] {
						string.Format(m_codepointColFmt, (int)prevChar), prevChar, 0});

					prevRow = m_gridChars.Rows[0];
				}

				count++;
				m_udpciList[prevChar].Add(list[i]);
			}

			if (prevRow != null)
				prevRow.Cells["count"].Value = count;

			m_gridChars.AutoResizeColumns();
			m_gridChars.CurrentCell = m_gridChars[0, 0];
			SilUtils.Utils.WaitCursors(false);
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
		private void m_gridChars_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
				return;

			char currChar;
			if (m_gridChars["char", e.RowIndex].Value is char)
				currChar = (char)m_gridChars["char", e.RowIndex].Value;
			else
			{
				m_gridWhere.Rows.Clear();
				return;
			}

			if (m_gridWhere.Tag is char && (char)m_gridWhere.Tag == currChar)
				return;

			m_gridWhere.Tag = currChar;
			m_gridWhere.Rows.Clear();

			if (m_udpciList.TryGetValue(currChar, out m_currUdpcil))
			{
				pgpWhere.Text = string.Format(m_codepointHdgFmt, (int)currChar);
				m_currUdpcil = m_udpciList[currChar];
				m_gridWhere.RowCount = m_udpciList[currChar].Count;
				
				if (m_gridWhere.RowCount > 0)
					m_gridWhere.CurrentCell = m_gridWhere[0, 0];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_gridWhere_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			e.Value = null;
			int row = e.RowIndex;

			if (m_currUdpcil == null)
				return;

			switch (e.ColumnIndex)
			{
				case 0: e.Value = m_currUdpcil[row].Transcription; break;
				case 1: e.Value = m_currUdpcil[row].Reference; break;
				case 2: e.Value = m_currUdpcil[row].SourceName; break;
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

			SilUtils.Utils.CenterFormInScreen(this);
			float splitRatio =
				PaApp.SettingsHandler.GetFloatSettingsValue(Name, "splitratio", 0f);

			// If the split ratio is zero, assume any form and grid settings found were
			// for the dialog as it was before my significant design changes made on
			// 11-Sep-07. As such, keep the default layout since the old saved settings
			// will probably make the dialog too small.
			if (splitRatio > 0)
			{
				PaApp.SettingsHandler.LoadFormProperties(this);
				PaApp.SettingsHandler.LoadGridProperties(m_gridChars);
				PaApp.SettingsHandler.LoadGridProperties(m_gridWhere);
				splitContainer1.SplitterDistance = (int)(splitContainer1.Width * splitRatio);
			}

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
			PaApp.SettingsHandler.SaveGridProperties(m_gridChars);
			PaApp.SettingsHandler.SaveGridProperties(m_gridWhere);

			float splitRatio = splitContainer1.SplitterDistance / (float)splitContainer1.Width;
			PaApp.SettingsHandler.SaveSettingsValue(Name, "splitratio", splitRatio);

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
				lblInfo.Height = sz.Height + 15;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the selected row color when the grid loses focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGridLeave(object sender, EventArgs e)
		{
			DataGridView grid = sender as DataGridView;
			if (grid != null)
			{
				grid.RowsDefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
				grid.RowsDefaultCellStyle.SelectionBackColor = SystemColors.Control;

				if (grid.CurrentRow != null)
					grid.InvalidateRow(grid.CurrentRow.Index);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Change the selected row color when the grid gains focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGridEnter(object sender, EventArgs e)
		{
			DataGridView grid = sender as DataGridView;
			if (grid != null)
			{
				grid.RowsDefaultCellStyle.SelectionForeColor = m_defaultSelectedRowForeColor;
				grid.RowsDefaultCellStyle.SelectionBackColor = m_defaultSelectedRowBackColor;

				if (grid.CurrentRow != null)
					grid.InvalidateRow(grid.CurrentRow.Index);
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