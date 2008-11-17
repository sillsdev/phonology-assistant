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
		private LocalizerProject m_currProject = null;
		private List<ResXEntry> m_resxEntries;
		private string m_currProjectFile;
		private SortedDictionary<string, AssemblyResXList> m_resxInfoList;
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
				m_grid.Columns[1].DefaultCellStyle.Font = m_currProject.SourceTextFont;
				m_grid.Columns[2].DefaultCellStyle.Font = m_currProject.TranslationFont;
				txtSrcText.TextBox.Font = m_currProject.SourceTextFont;
				txtTranslation.TextBox.Font = m_currProject.TranslationFont;
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
			using (ProjectDlg dlg = new ProjectDlg(null))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					m_currProject = dlg.Project;
					LoadTree();
				}
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
				if (fldrBrowser.ShowDialog(this) != DialogResult.OK)
					return;

				m_currProjectFile = Path.Combine(fldrBrowser.SelectedPath, m_currProject.Filename);
			}

			// Save the project file (i.e. .lop)
			m_currProject.Save(m_currProjectFile);

			// Save the resx resource files.
			SaveResXEntries(tvResXList.Nodes, TargetResXPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="path"></param>
		/// ------------------------------------------------------------------------------------
		private void SaveResXEntries(TreeNodeCollection nodes, string path)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Tag is ResXInfo)
					((ResXInfo)node.Tag).Save(path, m_currProject.CultureId);
				
				SaveResXEntries(node.Nodes, path);
			}
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void tbbCompile_Click(object sender, EventArgs e)
		{
			tbbSave_Click(null, null);

			foreach (TreeNode assemblyNode in tvResXList.Nodes)
			{
				if (assemblyNode.Nodes.Count == 0)
					continue;

				List<string> resourceFiles = new List<string>();
				foreach (TreeNode node in assemblyNode.Nodes)
				{
					ResXInfo resxInfo = node.Tag as ResXInfo;
					if (resxInfo != null && !string.IsNullOrEmpty(resxInfo.ResourceFile))
						resourceFiles.Add(resxInfo.ResourceFile);
				}

				if (resourceFiles.Count > 0)
				{
					LocalizingHelper.CompileLocalizedAssembly(m_currProject.CultureId,
						TargetResXPath,	assemblyNode.Text, resourceFiles.ToArray(),
						@"c:\phonology assistant\output\release");
				}
			}
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuCopyPrjResXFiles_Click(object sender, EventArgs e)
		{
			using (CopyProjectResXFilesDlg dlg = new CopyProjectResXFilesDlg())
				dlg.ShowDialog(this);
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

			if (m_resxEntries != null && m_resxEntries.Count > 0 && e.RowIndex < m_resxEntries.Count)
			{
				ResXEntry entry = m_resxEntries[e.RowIndex];
				switch (e.ColumnIndex)
				{
					case 0: e.Value = entry.StringId; break;
					case 1: e.Value = entry.SourceText; break;
					case 2: e.Value = entry.TargetText; break;
					case 3: e.Value = entry.Comment; break;
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
			if (m_resxEntries != null && m_resxEntries.Count > 0 && e.RowIndex < m_resxEntries.Count)
			{
				ResXEntry entry = m_resxEntries[e.RowIndex];
				if (e.ColumnIndex == 2)
					entry.TargetText = e.Value as string;
				else if (e.ColumnIndex == 3)
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
			if (e.ColumnIndex == 1 || e.ColumnIndex == 2 &&
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
			if (m_resxEntries != null && index < m_resxEntries.Count && index >= 0)
			{
				txtSrcText.TextBox.Text = m_resxEntries[index].SourceText;
				txtTranslation.TextBox.Text = m_resxEntries[index].TargetText;
				txtComment.TextBox.Text = m_resxEntries[index].Comment;
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
			if (m_resxEntries != null && index < m_resxEntries.Count && index >= 0)
			{
				string translation = txtTranslation.TextBox.Text.Trim();
				m_resxEntries[index].TargetText = translation;
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
			if (m_resxEntries != null && index < m_resxEntries.Count && index >= 0)
			{
				string comment = txtComment.TextBox.Text.Trim();
				comment = comment.Replace("\r\n", " ");
				comment = comment.Replace("\n\r", " ");
				comment = comment.Replace("\r", " ");
				comment = comment.Replace("\n", " ");
				m_resxEntries[index].Comment = comment;
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
			tvResXList.Nodes.Clear();

			if (m_currProject == null)
				return;

			m_resxInfoList = LocalizingHelper.ReadResXFiles(m_currProject.SourcePath);
			if (m_resxInfoList == null || m_resxInfoList.Count == 0)
				return;

			// Go through all the assemblies in the project.
			foreach (string assembly in m_resxInfoList.Keys)
			{
				TreeNode assemNode = new TreeNode(assembly);

				// Now go through the resx files found in the assembly.
				foreach (ResXInfo info in m_resxInfoList[assembly])
				{
					TreeNode resxNode = new TreeNode(Path.GetFileName(info.ResxFile));
					resxNode.Tag = info;
					assemNode.Nodes.Add(resxNode);
				}

				tvResXList.Nodes.Add(assemNode);
			}
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvResXList_AfterSelect(object sender, TreeViewEventArgs e)
		{
			m_grid.RowCount = 0;
			m_resxEntries = null;

			ResXInfo resxInfo = (e.Node == null ? null : e.Node.Tag as ResXInfo);
			if (resxInfo == null)
				return;

			m_resxEntries = resxInfo.GetStringEntries(TargetResXPath);
			if (m_resxEntries != null && m_resxEntries.Count > 0)
			{
				m_grid.RowCount = m_resxEntries.Count;
				m_grid.CurrentCell = m_grid[2, 0];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbGoogleTranslate_Click(object sender, EventArgs e)
		{
			if (m_resxEntries.Count == 0)
				return;

			GoogleTranslator translator = GoogleTranslator.Create(m_currProject.CultureId);
			if (translator == null)
				return;

			foreach (ResXEntry entry in m_resxEntries)
			{
				string text = entry.SourceText;
				if (text == null)
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
	}
}
