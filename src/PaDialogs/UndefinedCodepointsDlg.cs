using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;

namespace SIL.Pa.Dialogs
{
	public partial class UndefinedCodepointsDlg : Form
	{
		private string m_infoFmt;
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

			if (PaApp.UndefinedCodepoints != null && PaApp.UndefinedCodepoints.Count > 0)
			{
				using (UndefinedCodepointsDlg dlg = new UndefinedCodepointsDlg(projectName,
					PaApp.UndefinedCodepoints))
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
		public static void Show(string projectName, UndefinedCodePointInfoList list)
		{
			if (list != null && list.Count > 0)
			{
				using (UndefinedCodepointsDlg dlg = new UndefinedCodepointsDlg(projectName, list))
					dlg.ShowDialog();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedCodepointsDlg()
		{
			InitializeComponent();

			m_infoFmt = lblInfo.Text;
			lblInfo.Font = FontHelper.UIFont;
			chkShowUndefinedCharDlg.Font = FontHelper.UIFont;

			Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
			Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;

			PaApp.SettingsHandler.LoadFormProperties(this);
			AddColumns();

			if (PaApp.Project != null)
				chkShowUndefinedCharDlg.Checked = PaApp.Project.ShowUndefinedCharsDlg;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedCodepointsDlg(string projectName, UndefinedCodePointInfoList list) : this()
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
			// Add the Unicode number column.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("codepoint");
			col.HeaderText = STUtils.ConvertLiteralNewLines(Properties.Resources.kstidUnicodeNumHdg);
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

			// Add the sample column.
			col = SilGrid.CreateTextBoxColumn("datasource");
			col.HeaderText = STUtils.ConvertLiteralNewLines(Properties.Resources.kstidDataSourceHdg);
			m_grid.Columns.Add(col);

			m_grid.Name = Name + "Grid";
			PaApp.SettingsHandler.LoadGridProperties(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadGrid(UndefinedCodePointInfoList list)
		{
			m_grid.Rows.Clear();

			foreach (KeyValuePair<char, UndefinedCodePointInfo> mcpInfo in list)
			{
				m_grid.Rows.Add(new object[] {
					string.Format("U+{0:X4}", (int)mcpInfo.Key), mcpInfo.Key,
					mcpInfo.Value.PhoneticWord, mcpInfo.Value.SourceName});
			}

			m_grid.AutoResizeColumns();
			m_grid.AutoResizeRows();
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

			if (PaApp.Project != null &&
				PaApp.Project.ShowUndefinedCharsDlg != chkShowUndefinedCharDlg.Checked)
			{
				PaApp.Project.ShowUndefinedCharsDlg = chkShowUndefinedCharDlg.Checked;
				PaApp.Project.Save();
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
	}
}