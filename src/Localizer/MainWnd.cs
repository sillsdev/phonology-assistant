using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
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
		private const int kAsmCol = 1;
		private const int kResCol = 2;
		private const int kIdCol = 3;
		private const int kSrcCol = 4;
		private const int kTransCol = 5;
		private const int kCmntCol = 6;

		private const int kPrjNode = 0;
		private const int kAsmNode = 1;
		private const int kResNode = 2;

		private LocalizerProject m_currProject = null;
		private string m_currProjectFile;
		private int m_defaultLineHeight = 0;
		private string m_fmtWndText;
		private string m_wndText;
		private bool m_showComments = true;
		private bool m_showSrcText = true;
		private bool m_showTrans = true;
		private bool m_showOmittedItems = false;
		private Button m_btnCancelGoogleTranslate;
		private ToolStripControlHost m_cancelGoogleButtonHost;
		private bool m_googleTranslationTerminated;
		private List<int> m_indexes;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MainWnd()
		{
			InitializeComponent();
			m_fmtWndText = Properties.Resources.kstidMainWndTitleFmt;
			m_wndText = Text;

			// Restore column widths from settings file.
			foreach (DataGridViewColumn c in m_grid.Columns)
			{
				string settingName = string.Format("colWidth{0}", c.Index);
				try
				{
					c.Width = (int)Properties.Settings.Default[settingName];
				}
				catch { }
			}

			m_grid.MultiSelect = true;
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

			InitializeCancelGoogleTranslationButton();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeCancelGoogleTranslationButton()
		{
			m_btnCancelGoogleTranslate = new Button();
			m_btnCancelGoogleTranslate.Text = Properties.Resources.kstidCancelGoogleTransButtonText;
			
			int width = TextRenderer.MeasureText(m_btnCancelGoogleTranslate.Text,
				m_btnCancelGoogleTranslate.Font).Width;
			
			m_btnCancelGoogleTranslate.Size = new Size(width, 22);
			m_cancelGoogleButtonHost = new ToolStripControlHost(m_btnCancelGoogleTranslate);
			m_cancelGoogleButtonHost.Visible = false;
			m_statusbar.Items.Add(m_cancelGoogleButtonHost);

			m_btnCancelGoogleTranslate.Click += delegate
			{
				m_googleTranslationTerminated = true;
			};

			m_btnCancelGoogleTranslate.MouseEnter += delegate
			{
				UseWaitCursor = false;
			};

			m_btnCancelGoogleTranslate.MouseLeave += delegate
			{
				if (!m_googleTranslationTerminated)
					UseWaitCursor = true;
			};
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadProject(string fileName)
		{
			// TODO: Check if a project is already open and not saved.
			m_currProject = LocalizerProject.Load(fileName);

			// TODO: Show error if null.
			if (m_currProject != null)
			{
				m_currProjectFile = fileName;
				LoadTree();
				SetFonts();
				CalcRowHeight();
				Text = string.Format(m_fmtWndText, m_currProject.ProjectName, m_wndText);
			}
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
					m_grid.Columns[kSrcCol].DefaultCellStyle.Font = m_currProject.SourceTextFont;
					txtSrcText.TextBox.Font = m_currProject.SourceTextFont;
				}

				if (m_currProject.TargetLangFont != null)
				{
					m_grid.Columns[kTransCol].DefaultCellStyle.Font = m_currProject.TargetLangFont;
					txtTranslation.TextBox.Font = m_currProject.TargetLangFont;
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

			m_showOmittedItems = Properties.Settings.Default.showOmittedItems;
			m_showComments = Properties.Settings.Default.showCommentPane;

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

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string CurrentAssembly
		{
			get
			{
				TreeNode node = tvResources.SelectedNode;
				if (node != null)
				{
					if (node.Level == kAsmNode)
						return node.Text;
					if (node.Level == kResNode)
						return node.Parent.Text;
				}

				if (m_grid.CurrentRow != null)
				{
					ResourceEntry entry = GetResourceEntry(m_grid.CurrentRow.Index);
					if (entry != null)
						return entry.OwningAssembly.AssemblyName;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string CurrentResource
		{
			get
			{
				TreeNode node = tvResources.SelectedNode;
				if (node.Level == kResNode)
					return node.Text;

				if (m_grid.CurrentRow != null)
				{
					ResourceEntry entry = GetResourceEntry(m_grid.CurrentRow.Index);
					if (entry != null)
						return entry.OwningResource.ResourceName;
				}

				return null;
			}
		}

		#endregion

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

			if (result != DialogResult.OK)
				return;

			m_currProject.Scan(sslProgressBar, progressBar);
			LoadTree();
			SetFonts();
			CalcRowHeight();
			m_currProjectFile = null;

			// Force the project to be saved now.
			mnuSave_Click(null, null);
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
					//m_currProject.ReScan(sslProgressBar, progressBar);
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
		private void tbbRescan_Click(object sender, EventArgs e)
		{
			if (m_currProject != null && m_currProject.ReScan(sslProgressBar, progressBar))
			{
				LoadTree();
				CalcRowHeight();
			}
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
		private void mnuShowOmittedItems_Click(object sender, EventArgs e)
		{
			m_showOmittedItems = !m_showOmittedItems;
			string key = (tvResources.SelectedNode != null ? tvResources.SelectedNode.Name : null);
			LoadTree();
			TreeNode[] nodes = tvResources.Nodes.Find(key, true);
			tvResources.SelectedNode = (nodes.Length > 0 ? nodes[0] : tvResources.Nodes[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuShowSrcTextPane_Click(object sender, EventArgs e)
		{
			m_showSrcText = !m_showSrcText;
			HandleVisibleChangeForSrcTransPanes();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuShowTransPane_Click(object sender, EventArgs e)
		{
			m_showTrans = !m_showTrans;
			HandleVisibleChangeForSrcTransPanes();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuShowCommentPane_Click(object sender, EventArgs e)
		{
			m_showComments = !m_showComments;
			HandleVisibleChangeForSrcTransPanes();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void mnuView_DropDownOpening(object sender, EventArgs e)
		{
			mnuShowSrcTextPane.Checked = m_showSrcText;
			mnuShowTransPane.Checked = m_showTrans;
			mnuShowCommentPane.Checked = m_showComments;
			mnuShowOmittedItems.Checked = m_showOmittedItems;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleVisibleChangeForSrcTransPanes()
		{
			splitEntries.Panel2Collapsed = (!m_showSrcText && !m_showTrans && !m_showComments);
			splitSrcTransCmt.Panel1Collapsed = (!m_showSrcText && !m_showTrans);
			splitSrcTransCmt.Panel2Collapsed = !m_showComments;
			splitSrcTrans.Panel1Collapsed = !m_showSrcText;
			splitSrcTrans.Panel2Collapsed = !m_showTrans;
		}

		#region Grid events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (m_grid.CurrentRow is SilHierarchicalGridRow)
				return;
	
			e.Value = string.Empty;
			ResourceEntry entry = GetResourceEntry(e.RowIndex);
			if (entry == null)
				return;

			switch (e.ColumnIndex)
			{
				case kAsmCol: e.Value = entry.OwningAssembly.AssemblyName; break;
				case kResCol: e.Value = entry.OwningResource.ResourceName; break;
				case kIdCol: e.Value = entry.StringId; break;
				case kSrcCol: e.Value = entry.SourceText; break;
				case kTransCol: e.Value = entry.TargetText; break;

				case kStatusCol:
					if (entry.Omitted)
						e.Value = Properties.Resources.kimidOmitted;
					else
					{
						string imageId = "kimid" + entry.TranslationStatus.ToString();
						e.Value = Properties.Resources.ResourceManager.GetObject(imageId);
					}
					break;

				case kCmntCol:
					if (!string.IsNullOrEmpty(entry.Comment))
						e.Value = entry.Comment;
					else if (entry.StringId != null && m_currProject.ResourceCatalog != null)
					{
						e.Value = m_currProject.ResourceCatalog.GetComment(
							CurrentAssembly, CurrentResource, entry.StringId);
					}
					break;

				default: e.Value = string.Empty; break;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			ResourceEntry entry = GetResourceEntry(e.RowIndex);
			if (entry != null)
			{
				// When the translation value is being pushed, we don't update the
				// underlying source here because that should already have been done
				// when the cell is validated.
				if (e.ColumnIndex == kCmntCol)
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

			ResourceEntry entry = GetResourceEntry(e.RowIndex);
			if (entry != null && entry.Omitted)
				e.CellStyle.ForeColor = SystemColors.GrayText;
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
			if (m_grid.CurrentRow is SilHierarchicalGridRow)
			{
				sslMain.Text = string.Empty;
				txtSrcText.TextBox.Text = string.Empty;
				txtTranslation.TextBox.Text = string.Empty;
				txtComment.TextBox.Text = string.Empty;
			}
			else
			{
				txtSrcText.TextBox.Text = m_grid[kSrcCol, e.RowIndex].Value as string;
				txtTranslation.TextBox.Text = m_grid[kTransCol, e.RowIndex].Value as string;
				txtComment.TextBox.Text = m_grid[kCmntCol, e.RowIndex].Value as string;
				SetStatusBarText(e.RowIndex);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				m_grid.Rows[e.RowIndex] != m_grid.CurrentRow && e.RowIndex >= 0 &&
				e.RowIndex < m_grid.RowCount && e.Button == MouseButtons.Right)
			{
				if (!(m_grid.CurrentRow is SilHierarchicalGridRow))
					m_grid.CurrentCell = m_grid[e.ColumnIndex, e.RowIndex];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (e.ColumnIndex == kTransCol && e.RowIndex >= 0)
				UpdateEntryStatus(e.RowIndex, e.FormattedValue as string);
		}
	
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtComment_Validated(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow == null)
				return;

			ResourceEntry entry = GetResourceEntry(m_grid.CurrentRow.Index);
			if (entry != null)
			{
				string comment = txtComment.TextBox.Text.Trim();
				comment = comment.Replace("\r\n", " ");
				comment = comment.Replace("\n\r", " ");
				comment = comment.Replace("\r", " ");
				comment = comment.Replace("\n", " ");
				entry.Comment = comment;
				m_grid.InvalidateRow(m_grid.CurrentRow.Index);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void txtTranslation_Validated(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow != null)
				UpdateEntryStatus(m_grid.CurrentRow.Index, txtTranslation.TextBox.Text.Trim());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <param name="newValue"></param>
		/// ------------------------------------------------------------------------------------
		private void UpdateEntryStatus(int row, string newValue)
		{
			ResourceEntry entry = GetResourceEntry(row);
			if (entry != null && entry.TargetText != newValue)
			{
				entry.TargetText = newValue;
				TranslationStatus currStatus = entry.TranslationStatus;

				if (string.IsNullOrEmpty(newValue))
					entry.TranslationStatus = TranslationStatus.Untranslated;
				else if (currStatus == TranslationStatus.Untranslated)
					entry.TranslationStatus = TranslationStatus.Unreviewed;

				m_grid.InvalidateRow(row);
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
			m_grid.SuspendLayout();
			tvResources.BeginUpdate();
			m_grid.RowCount = 0;
			txtSrcText.TextBox.Text = string.Empty;
			txtTranslation.TextBox.Text = string.Empty;
			txtComment.TextBox.Text = string.Empty;
			tvResources.Nodes.Clear();

			if (m_currProject == null)
				return;

			TreeNode headNode = new TreeNode(m_currProject.ProjectName);
			headNode.Name = headNode.Text;

			// Go through all the assemblies in the project.
			foreach (AssemblyResourceInfo assembly in m_currProject.AssemblyInfoList)
			{
				if (assembly.Omitted && !m_showOmittedItems)
					continue;

				TreeNode asmNode = new TreeNode(assembly.AssemblyName);
				asmNode.Name = asmNode.Text;
				asmNode.ForeColor =
					(assembly.Omitted ? SystemColors.GrayText : SystemColors.WindowText);

				// Now go through the resx files found in the assembly.
				foreach (ResourceInfo info in assembly.ResourceInfoList)
				{
					if (!info.Omitted || m_showOmittedItems)
					{
						TreeNode resNode = new TreeNode(info.ResourceName);
						resNode.Name = resNode.Text;
						resNode.ForeColor = (info.Omitted ?
							SystemColors.GrayText : SystemColors.WindowText);

						asmNode.Nodes.Add(resNode);
					}
				}

				headNode.Nodes.Add(asmNode);
			}

			tvResources.Nodes.Add(headNode);
			headNode.Expand();
			m_grid.ResumeLayout();
			tvResources.EndUpdate();
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvResources_AfterSelect(object sender, TreeViewEventArgs e)
		{
			m_indexes = new List<int>();
			int count = GetResourceEntryCount(e.Node);
			int level = e.Node.Level;
			for (int i = 0; i < count; i++)
			{
				ResourceEntry entry = GetResourceEntry(i, e.Node);
				if (entry != null && (!entry.Omitted || m_showOmittedItems))
					m_indexes.Add(i);
			}

			m_grid.RowCount = m_indexes.Count;
			m_grid.Columns[kAsmCol].Visible = (e.Node.Level == kPrjNode);
			m_grid.Columns[kResCol].Visible = (e.Node.Level == kPrjNode || e.Node.Level == kAsmNode);

			if (m_grid.RowCount == 0)
				sslMain.Text = string.Empty;
			else
			{
				m_grid.CurrentCell = m_grid[kTransCol, 0];
				SetStatusBarText(0);
			}

			m_grid.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tbbGoogleTranslate_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (m_grid.RowCount == 0)
				return;

			m_grid.EndEdit();
			if (m_grid.CurrentCell != null && m_grid.CurrentCell.ColumnIndex == kTransCol)
				m_grid.CurrentCell = m_grid[kStatusCol, m_grid.CurrentCell.RowIndex];

			tbbGoogleTranslate.DropDown.Close();
			Application.DoEvents();
			GoogleTranslate(e.ClickedItem == mnuGoggleTransSelected);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GoogleTranslate(bool onlySelected)
		{
			GoogleTranslator translator = GoogleTranslator.Create(m_currProject.CultureId);
			if (translator == null)
				return;

			m_googleTranslationTerminated = false;
			progressBar.Value = 0;
			progressBar.Maximum = m_grid.RowCount;
			sslProgressBar.Visible = true;
			progressBar.Visible = true;
			m_cancelGoogleButtonHost.Visible = true;
			UseWaitCursor = true;

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (m_googleTranslationTerminated)
					break;

				if (onlySelected && !row.Selected)
					continue;

				ResourceEntry entry = GetResourceEntry(row.Index);
				if (entry == null)
					continue;

				progressBar.Value++;
				sslProgressBar.Text = entry.StringId;
				Application.DoEvents();

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
				{
					entry.TargetText = text;
					entry.TranslationStatus = TranslationStatus.Unreviewed;
				}
			}

			m_grid.Invalidate();
			UseWaitCursor = false;
			sslProgressBar.Visible = false;
			progressBar.Visible = false;
			m_cancelGoogleButtonHost.Visible = false;
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

			ResourceEntry entry = GetResourceEntry(m_grid.CurrentRow.Index);
			if (entry != null)
			{
				entry.TranslationStatus = TranslationStatus.Unreviewed;
				m_grid.InvalidateCell(kStatusCol, m_grid.CurrentRow.Index);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuCompleted_Click(object sender, EventArgs e)
		{
			if (m_grid.CurrentRow != null)
			{
				ResourceEntry entry = GetResourceEntry(m_grid.CurrentRow.Index);
				if (entry != null)
				{
					entry.TranslationStatus = TranslationStatus.Completed;
					m_grid.InvalidateCell(kStatusCol, m_grid.CurrentRow.Index);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetStatusBarText(int row)
		{
			sslMain.Text = string.Format(Properties.Resources.kstidStatusBarFmt,
				row + 1, m_grid.RowCount);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private ResourceEntry GetResourceEntry(int i)
		{
			if (m_indexes != null)
				i = m_indexes[i];

			return GetResourceEntry(i, tvResources.SelectedNode);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private ResourceEntry GetResourceEntry(int i, TreeNode node)
		{
			if (node == null || m_currProject == null || m_currProject.AssemblyInfoList == null)
				return null;

			if (node.Level == kPrjNode)
				return m_currProject.AssemblyInfoList.GetResourceEntry(i);

			if (node.Level == kAsmNode)
			{
				string assembly = node.Text;
				return m_currProject.AssemblyInfoList[assembly][i];
			}
			if (node.Level == kResNode)
			{
				string assembly = node.Parent.Text;
				string resource = node.Text;
				return m_currProject.AssemblyInfoList[assembly][resource][i];
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetResourceEntryCount()
		{
			return GetResourceEntryCount(tvResources.SelectedNode);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int GetResourceEntryCount(TreeNode node)
		{
			if (node == null || m_currProject == null || m_currProject.AssemblyInfoList == null)
				return 0;

			switch (node.Level)
			{
				case kPrjNode: return m_currProject.AssemblyInfoList.StringEntryCount;
				case kAsmNode: return m_currProject.AssemblyInfoList[CurrentAssembly].StringEntryCount;
				case kResNode: return m_currProject.AssemblyInfoList[CurrentAssembly][CurrentResource].StringEntryCount;
			}

			return 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tvResources_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeViewHitTestInfo tvhti = tvResources.HitTest(e.Location);
				if (tvhti != null && tvhti.Node != null && tvResources.SelectedNode != tvhti.Node)
					tvResources.SelectedNode = tvhti.Node;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void cmnuTree_Opening(object sender, CancelEventArgs e)
		{
			cmnuOmitAssembly.Visible = true;
			cmnuOmitAssembly.Enabled = false;
			cmnuOmitResource.Visible = false;

			if (tvResources.SelectedNode == null)
				return;

			if (tvResources.SelectedNode.Level == kAsmNode)
				cmnuOmitAssembly.Enabled = true;
			else if (tvResources.SelectedNode.Level == kResNode)
			{
				cmnuOmitAssembly.Visible = false;
				cmnuOmitResource.Visible = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cmnuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			TreeNode node = tvResources.SelectedNode;
			bool omitted = false;

			if (node.Level == kAsmNode)
			{
				omitted = !m_currProject.AssemblyInfoList[node.Text].Omitted; 
				m_currProject.AssemblyInfoList[node.Text].Omitted = omitted;
			}
			else if (node.Level == kResNode)
			{
				omitted = !m_currProject.AssemblyInfoList[node.Parent.Text][node.Text].Omitted;
				m_currProject.AssemblyInfoList[node.Parent.Text][node.Text].Omitted = omitted;
			}

			if (m_showOmittedItems)
				node.ForeColor = (omitted ? SystemColors.GrayText : SystemColors.WindowText);
			else
			{
				TreeNode newNode = node.PrevVisibleNode;
				node.Parent.Nodes.Remove(node);
				tvResources.SelectedNode = newNode;
			}
		}

		private void cmnuOmitResourceItem_Click(object sender, EventArgs e)
		{

		}
	}
}
