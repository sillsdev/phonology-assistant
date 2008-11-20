using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SIL.SpeechTools.Utils;
using SIL.Localize.LocalizingUtils;

namespace SIL.Localize.Localizer
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class MainWnd : Form
	{
		private const int kStatusCol = 0;
		private const int kIdCol = 1;
		private const int kSrcCol = 2;
		private const int kTransCol = 3;
		private const int kCmntCol = 4;

		private LocalizerProject m_currProject = null;
		private List<ResourceEntry> m_resourceEntries;
		private string m_currProjectFile;
		private int m_defaultLineHeight = 0;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MainWnd()
		{
			InitializeComponent();

			foreach (DataGridViewColumn c in m_grid.Columns)
			{
				string settingName = string.Format("colWidth{0}", c.Index);
				try
				{
					c.Width = (int)Properties.Settings.Default[settingName];
				}
				catch { }
			}

			m_grid.AllowUserToOrderColumns = true;
			m_grid.AllowUserToResizeRows = true;
			m_grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			m_grid.IsDirty = false;

			Size sz = Properties.Settings.Default.mainWndSz;
			if (sz != Size.Empty)
				Size = sz;

			Point pt = Properties.Settings.Default.mainWndLocation;
			if (pt != Point.Empty)
				Location = pt;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal string TargetResXPath
		{
			get
			{
				// TODO: Add some error checking.
				string path = Path.GetDirectoryName(m_currProjectFile);
				return Path.Combine(path, m_currProject.CultureId);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadProject(string fileName)
		{
			// TODO: Check if a project is already open and not saved.
			if (!File.Exists(fileName))
				return;

			m_currProject =
				Program.DeserializeData(fileName, typeof(LocalizerProject)) as LocalizerProject;

			// TODO: Show error if null.
			if (m_currProject == null)
				return;

			m_currProjectFile = fileName;
			LoadTree();
			SetFonts();
			CalcRowHeight();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetFonts()
		{
			if (m_currProject != null)
			{
				if (m_currProject.SourceTextFont != null)
				{
					m_grid.Columns[1].DefaultCellStyle.Font = m_currProject.SourceTextFont;
					txtSrcText.TextBox.Font = m_currProject.SourceTextFont;
				}

				if (m_currProject.TranslationFont != null)
				{
					m_grid.Columns[2].DefaultCellStyle.Font = m_currProject.TranslationFont;
					txtTranslation.TextBox.Font = m_currProject.TranslationFont;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CalcRowHeight()
		{
			//int defaultGridRowHeight = Properties.Settings.Default.defaultGridRowHeight;

			//if (defaultGridRowHeight > 0)
			//    m_defaultLineHeight = Properties.Settings.Default.defaultGridRowHeight;

			m_defaultLineHeight = 0;

			foreach (DataGridViewColumn col in m_grid.Columns)
			{
				Font fnt = (col.DefaultCellStyle.Font ?? m_grid.Font);
				m_defaultLineHeight = Math.Max(fnt.Height, m_defaultLineHeight);
			}

			m_defaultLineHeight += (int)(m_defaultLineHeight * 0.75);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			float ratio = Properties.Settings.Default.vSplitRatio;
			try
			{
				splitOuter.SplitterDistance = (int)(splitOuter.Width * ratio);
			}
			catch { }

			ratio = Properties.Settings.Default.hSplitRatio;
			try
			{
				splitEntries.SplitterDistance = (int)(splitEntries.Height * ratio);
			}
			catch { }

			string projFile = Properties.Settings.Default.lastLoadedProject;
			if (!string.IsNullOrEmpty(projFile))
				LoadProject(projFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
		
			Properties.Settings.Default.vSplitRatio =
				(float)splitOuter.SplitterDistance / splitOuter.Width;

			Properties.Settings.Default.hSplitRatio =
				(float)splitEntries.SplitterDistance / splitEntries.Height;

			if (!string.IsNullOrEmpty(m_currProjectFile))
				Properties.Settings.Default.lastLoadedProject = m_currProjectFile;

			Properties.Settings.Default.mainWndLocation = Location;
			Properties.Settings.Default.mainWndSz = Size;
			Properties.Settings.Default.defaultGridRowHeight = m_defaultLineHeight;

			foreach (DataGridViewColumn col in m_grid.Columns)
			{
				string settingName = string.Format("colWidth{0}", col.Index);
				try
				{
					Properties.Settings.Default[settingName] = col.Width;
				}
				catch {}
			}

			Properties.Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuNewProject_Click(object sender, EventArgs e)
		{
			DialogResult result = DialogResult.Cancel;

			using (ProjectDlg dlg = new ProjectDlg(null))
			{
				result = dlg.ShowDialog(this);
				if (result == DialogResult.OK)
					m_currProject = dlg.Project;

				dlg.Hide();
			}

			if (result == DialogResult.OK)
			{
				ResXReader reader = new ResXReader();
				m_currProject.AssemblyInfoList =
					reader.Read(m_currProject.SourcePath, sslProgressBar, progressBar);
				
				LoadTree();
				SetFonts();
				CalcRowHeight();
				m_currProjectFile = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuOpenProject_Click(object sender, EventArgs e)
		{
			if (openFileDlg.ShowDialog(this) == DialogResult.OK && File.Exists(openFileDlg.FileName))
				LoadProject(openFileDlg.FileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuProjectSettings_Click(object sender, EventArgs e)
		{
			if (m_currProject == null)
				return;

			using (ProjectDlg dlg = new ProjectDlg(m_currProject))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					SetFonts();
					CalcRowHeight();
					m_grid.Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuSave_Click(object sender, EventArgs e)
		{
			tbbSave_Click(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbSave_Click(object sender, EventArgs e)
		{
			if (m_currProject == null)
				return;

			if (m_currProjectFile == null)
			{
				saveFileDlg.FileName = m_currProject.ProjectName + ".lop";
				if (saveFileDlg.ShowDialog(this) != DialogResult.OK)
					return;

				m_currProjectFile = saveFileDlg.FileName;
			}

			// Save the project file.
			m_currProject.Save(m_currProjectFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuSaveAs_Click(object sender, EventArgs e)
		{
			if (m_currProjectFile == null)
				mnuSave_Click(null, null);
			else
			{
				saveFileDlg.FileName = m_currProjectFile;
				if (saveFileDlg.ShowDialog(this) != DialogResult.OK)
					return;

				m_currProjectFile = saveFileDlg.FileName;
			}

			// Save the project file.
			m_currProject.Save(m_currProjectFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbCompile_Click(object sender, EventArgs e)
		{
			tbbSave_Click(null, null);
			m_currProject.Compile(Path.GetDirectoryName(m_currProject.ExePath));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbShowCommentsPane_Click(object sender, EventArgs e)
		{
			splitSrcTransCmt.Panel2Collapsed = !tbbShowCommentsPane.Checked;
			//splitEntries.Panel2Collapsed = (splitSrcTrans.Panel1Collapsed && splitSrcTransCmt.Panel2Collapsed &&
			//    splitSrcTrans.Panel2Collapsed);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbShowSrcTextPane_Click(object sender, EventArgs e)
		{
			splitSrcTrans.Panel1Collapsed = !tbbShowSrcTextPane.Checked;
			//splitSrcTransCmt.Panel1Collapsed =
			//    (splitSrcTrans.Panel1Collapsed && splitSrcTrans.Panel2Collapsed);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbShowTransPane_Click(object sender, EventArgs e)
		{
			splitSrcTrans.Panel2Collapsed = !tbbShowTransPane.Checked;
			//splitSrcTransCmt.Panel1Collapsed =
			//    (splitSrcTrans.Panel1Collapsed && splitSrcTrans.Panel2Collapsed);
		}

		#region Grid events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			e.Value = string.Empty;

			if (m_resourceEntries != null && m_resourceEntries.Count > 0 && e.RowIndex < m_resourceEntries.Count)
			{
				ResourceEntry entry = m_resourceEntries[e.RowIndex];
				switch (e.ColumnIndex)
				{
					case kIdCol: e.Value = entry.StringId; break;
					case kSrcCol: e.Value = entry.SourceText; break;
					case kTransCol: e.Value = entry.TargetText; break;
					case kCmntCol: e.Value = entry.Comment; break;
					case kStatusCol:
						string imageId = "kimid" + entry.TranslationStatus.ToString();
						e.Value = Properties.Resources.ResourceManager.GetObject(imageId);
						break;
					
					default: e.Value = string.Empty; break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			if (m_resourceEntries != null && m_resourceEntries.Count > 0 && e.RowIndex < m_resourceEntries.Count)
			{
				ResourceEntry entry = m_resourceEntries[e.RowIndex];
				if (e.ColumnIndex == kTransCol)
					entry.TargetText = e.Value as string;
				else if (e.ColumnIndex == kCmntCol)
					entry.Comment = e.Value as string;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == kSrcCol || e.ColumnIndex == kTransCol &&
				m_grid.Columns[e.ColumnIndex].DefaultCellStyle.Font != null)
			{
				e.CellStyle.Font = m_grid.Columns[e.ColumnIndex].DefaultCellStyle.Font;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowHeightInfoPushed(object sender, DataGridViewRowHeightInfoPushedEventArgs e)
		{
			if (Properties.Settings.Default.universalRowResize)
			{
				m_defaultLineHeight = e.Height;
				m_grid.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowHeightInfoNeeded(object sender, DataGridViewRowHeightInfoNeededEventArgs e)
		{
			e.Height = m_defaultLineHeight;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			int index = e.RowIndex;
			if (m_resourceEntries != null && index < m_resourceEntries.Count && index >= 0)
			{
				txtSrcText.TextBox.Text = m_resourceEntries[index].SourceText;
				txtTranslation.TextBox.Text = m_resourceEntries[index].TargetText;
				txtComment.TextBox.Text = m_resourceEntries[index].Comment;
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtTranslation_Validated(object sender, EventArgs e)
		{
			int index = (m_grid.CurrentRow != null ? m_grid.CurrentRow.Index : -1);
			if (m_resourceEntries != null && index < m_resourceEntries.Count && index >= 0)
			{
				string translation = txtTranslation.TextBox.Text.Trim();
				m_resourceEntries[index].TargetText = translation;
				m_grid.InvalidateRow(m_grid.CurrentRow.Index);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtComment_Validated(object sender, EventArgs e)
		{
			int index = (m_grid.CurrentRow != null ? m_grid.CurrentRow.Index : -1);
			if (m_resourceEntries != null && index < m_resourceEntries.Count && index >= 0)
			{
				string comment = txtComment.TextBox.Text.Trim();
				comment = comment.Replace("\r\n", " ");
				comment = comment.Replace("\n\r", " ");
				comment = comment.Replace("\r", " ");
				comment = comment.Replace("\n", " ");
				m_resourceEntries[index].Comment = comment;
				m_grid.InvalidateRow(m_grid.CurrentRow.Index);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (m_grid.RowCount == 0 || m_grid.CurrentRow == null)
				e.SuppressKeyPress = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadTree()
		{
			m_grid.RowCount = 0;
			txtSrcText.TextBox.Text = string.Empty;
			txtTranslation.TextBox.Text = string.Empty;
			txtComment.TextBox.Text = string.Empty;
			tvResources.Nodes.Clear();

			if (m_currProject == null)
				return;

			TreeNode headNode = new TreeNode(m_currProject.ProjectName);

			// Go through all the assemblies in the project.
			foreach (AssemblyResourceInfo assembly in m_currProject.AssemblyInfoList)
			{
				TreeNode assemNode = new TreeNode(assembly.AssemblyName);

				// Now go through the resx files found in the assembly.
				foreach (RessourceInfo info in assembly.ResourceInfoList)
				{
					TreeNode resxNode = new TreeNode(info.ResourceName);
					resxNode.Tag = info;
					assemNode.Nodes.Add(resxNode);
				}

				headNode.Nodes.Add(assemNode);
			}

			tvResources.Nodes.Add(headNode);
			headNode.Expand();
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvResXList_AfterSelect(object sender, TreeViewEventArgs e)
		{
			//m_grid.RowCount = 0;
			//m_resxEntries = null;

			//ResXInfo resxInfo = (e.Node == null ? null : e.Node.Tag as ResXInfo);
			//if (resxInfo == null)
			//    return;

			//m_resxEntries = resxInfo.GetStringEntries(TargetResXPath);
			//if (m_resxEntries != null && m_resxEntries.Count > 0)
			//{
			//    m_grid.RowCount = m_resxEntries.Count;
			//    m_grid.CurrentCell = m_grid[2, 0];
			//}

			m_grid.RowCount = 0;
			m_resourceEntries = null;

			RessourceInfo resxInfo = (e.Node == null ? null : e.Node.Tag as RessourceInfo);
			if (resxInfo == null)
				return;

			m_resourceEntries = resxInfo.StringEntries;
			if (m_resourceEntries != null && m_resourceEntries.Count > 0)
			{
				m_grid.RowCount = m_resourceEntries.Count;
				m_grid.CurrentCell = m_grid[kTransCol, 0];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbGoogleTranslate_Click(object sender, EventArgs e)
		{
			if (m_resourceEntries.Count == 0)
				return;

			GoogleTranslator translator = GoogleTranslator.Create(m_currProject.CultureId);
			if (translator == null)
				return;

			foreach (ResourceEntry entry in m_resourceEntries)
			{
				string text = entry.SourceText;
				if (!string.IsNullOrEmpty(entry.TargetText) || text == null)
					continue;

				// ENHANCE: should probably be smarter about removing ampersands.
				// e.g. Remove them only if there is not a space on either side.
				text = text.Replace("&&", "~~");
				text = text.Replace("&", string.Empty);
				text = text.Replace("~~", "&");
				text = translator.TranslateText(text);
				if (!string.IsNullOrEmpty(text))
					entry.TargetText = text;
			}

			m_grid.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuUnreviewed_Click(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow == null)
				return;

			m_resourceEntries[m_grid.CurrentRow.Index].TranslationStatus = TranslationStatus.Unreviewed;
			m_grid.InvalidateCell(kStatusCol, m_grid.CurrentRow.Index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuCompleted_Click(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow == null)
				return;

			m_resourceEntries[m_grid.CurrentRow.Index].TranslationStatus = TranslationStatus.Completed;
			m_grid.InvalidateCell(kStatusCol, m_grid.CurrentRow.Index);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != kTransCol || e.RowIndex < 0)
				return;

			string cellValue = m_grid[kTransCol, e.RowIndex].Value as string;
			TranslationStatus currStatus = m_resourceEntries[e.RowIndex].TranslationStatus;

			if (string.IsNullOrEmpty(cellValue))
				m_resourceEntries[e.RowIndex].TranslationStatus = TranslationStatus.Untranslated;
			else if (currStatus == TranslationStatus.Untranslated)
				m_resourceEntries[e.RowIndex].TranslationStatus = TranslationStatus.Unreviewed;

			m_grid.InvalidateCell(kStatusCol, e.RowIndex);
		}
	}
}
