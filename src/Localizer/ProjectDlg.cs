using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;
using SIL.Localize.LocalizingUtils;

namespace SIL.Localize.Localizer
{
	#region ProjectDlg class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ProjectDlg : Form
	{
		// TODO: Internationalize
		private const string kfmtFont = "{0}, {1}, {2} points";

		protected LocalizerProject m_project;
		protected bool m_isPrjNew = true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ProjectDlg()
		{
			InitializeComponent();
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ProjectDlg(LocalizerProject project)	: this()
		{
			m_isPrjNew = (project == null);
			m_project = (project ?? new LocalizerProject());
			openFileDlg.InitialDirectory = string.Empty;

			txtPrjName.Enabled = m_isPrjNew;
			cboTarget.Enabled = m_isPrjNew;
			fntDlg.ShowEffects = false;

			Font fntSrcText;
			Font fntTrans;

			// REVIEW: Should this be windows-only cultures;
			// only installed cutures; all cultures, etc.?
			foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
			{
				if (!ci.EnglishName.StartsWith("Invariant Language"))
					cboTarget.Items.Add(ci);
			}

			if (m_isPrjNew)
			{
				fntSrcText = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point);
				fntTrans = fntSrcText;
			}
			else
			{
				txtPrjName.Text = m_project.ProjectName;
				cboTarget.Text = CultureInfo.GetCultureInfo(m_project.CultureId).DisplayName;
				txtExe.Text = m_project.ExePath;
				txtResCatalog.Text = m_project.ResourceCatalogPath;
				fntSrcText = m_project.SourceTextFont;
				fntTrans = m_project.TargetLangFont;

				foreach (string path in m_project.SourceFiles)
					lstSrcPaths.Items.Add(path);
			}

			txtSrcTextFont.Tag = fntSrcText;
			txtSrcTextFont.Text = string.Format(kfmtFont, fntSrcText.Name,
				fntSrcText.Style.ToString(), fntSrcText.SizeInPoints);

			txtTransFont.Tag = fntTrans;
			txtTransFont.Text = string.Format(kfmtFont, fntTrans.Name,
				fntTrans.Style.ToString(), fntTrans.SizeInPoints);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			
			if (btnOK.Enabled && DialogResult == DialogResult.OK)
			{
				if (m_isPrjNew && !Verify())
				{
					e.Cancel = true;
					return;
				}

				if (m_isPrjNew)
				{
					m_project.ProjectName = txtPrjName.Text.Trim();
					m_project.CultureId = cboTarget.SelectedItem.ToString();
				}

				m_project.SourceFiles = new List<string>();
				foreach (string path in lstSrcPaths.Items)
					m_project.SourceFiles.Add(path);

				m_project.SourceTextFont = txtSrcTextFont.Tag as Font;
				m_project.TargetLangFont = txtTransFont.Tag as Font;
				m_project.ExePath = txtExe.Text.Trim();
				m_project.ResourceCatalogPath = txtResCatalog.Text.Trim();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool Verify()
		{
			string msg = null;

			// TODO: internationalize
			if (txtPrjName.Text.Trim() == string.Empty)
			{
				msg = "You must specify a project name.";
				txtPrjName.Focus();
			}
			else if (cboTarget.SelectedIndex < 0)
			{
				msg = "You must specify a target language.";
				cboTarget.Focus();
			}
			else if (txtExe.Text.Trim() == string.Empty)
			{
				msg = "You must specify the program file to localize.";
				txtExe.Focus();
			}
			else if (lstSrcPaths.Items.Count == 0)
			{
				msg = "You must specify one or more resource files.";
				btnAdd.Focus();
			}

			if (msg != null)
			{
				MessageBox.Show(msg, Application.ProductName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			return (msg == null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LocalizerProject Project
		{
			get { return m_project; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnSrcFont_Click(object sender, EventArgs e)
		{
			fntDlg.Font = txtSrcTextFont.Tag as Font;
			if (fntDlg.ShowDialog(this) == DialogResult.OK)
			{
				Font fnt = fntDlg.Font;
				txtSrcTextFont.Tag = (Font)fnt.Clone();
				txtSrcTextFont.Text = string.Format(kfmtFont, fnt.Name, fnt.Style.ToString(),
					Math.Round(fnt.SizeInPoints, MidpointRounding.AwayFromZero));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnTransFont_Click(object sender, EventArgs e)
		{
			fntDlg.Font = txtTransFont.Tag as Font;
			if (fntDlg.ShowDialog(this) == DialogResult.OK)
			{
				Font fnt = fntDlg.Font;
				txtTransFont.Text = string.Format(kfmtFont, fnt.Name, fnt.Style.ToString(), fnt.SizeInPoints);
				txtTransFont.Tag = (Font)fnt.Clone();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnExe_Click(object sender, EventArgs e)
		{
			if (File.Exists(txtExe.Text.Trim()))
				openFileDlg.FileName = txtExe.Text.Trim();

			openFileDlg.Title = Properties.Resources.kstidOFDExeFileTitle;
			openFileDlg.Filter = Properties.Resources.kstidOFDExeFilter;
			openFileDlg.Multiselect = false;

			if (txtExe.Text != string.Empty)
				openFileDlg.FileName = txtExe.Text;

			if (openFileDlg.ShowDialog() == DialogResult.OK)
				txtExe.Text = openFileDlg.FileName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnResCatalog_Click(object sender, EventArgs e)
		{
			openFileDlg.Title = Properties.Resources.kstidOFDCatFileTitle;
			openFileDlg.Filter = Properties.Resources.kstidOFDCatFilter;
			openFileDlg.Multiselect = false;

			if (txtResCatalog.Text != string.Empty)
				openFileDlg.FileName = txtResCatalog.Text;

			if (openFileDlg.ShowDialog() == DialogResult.OK)
				txtResCatalog.Text = openFileDlg.FileName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (File.Exists(txtExe.Text.Trim()) && openFileDlg.InitialDirectory == string.Empty)
				openFileDlg.InitialDirectory = Path.GetDirectoryName(txtExe.Text.Trim());

			openFileDlg.Title = Properties.Resources.kstidOFDResFilesTitle;
			openFileDlg.Filter = Properties.Resources.kstidOFDResFilter;
			openFileDlg.Multiselect = true;

			if (openFileDlg.ShowDialog() == DialogResult.OK)
				AddFilesToList(openFileDlg.FileNames);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (lstSrcPaths.SelectedItems.Count < 0)
				return;

			int i = lstSrcPaths.SelectedIndex;

			List<string> selPaths = new List<string>();
			foreach (string path in lstSrcPaths.SelectedItems)
				selPaths.Add(path);

			foreach (string path in selPaths)
				lstSrcPaths.Items.Remove(path);

			while (i >= lstSrcPaths.Items.Count && i >= 0)
				i--;

			if (i >= 0)
				lstSrcPaths.SelectedIndex = i;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnScan_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(fldrBrowser.SelectedPath) && txtExe.Text != string.Empty)
				fldrBrowser.SelectedPath = Path.GetDirectoryName(txtExe.Text);

			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
			{
				AddFilesToList(Directory.GetFiles(fldrBrowser.SelectedPath, "*.exe",
					SearchOption.AllDirectories));
				AddFilesToList(Directory.GetFiles(fldrBrowser.SelectedPath, "*.dll",
					SearchOption.AllDirectories));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddFilesToList(string[] paths)
		{
			if (paths == null || paths.Length == 0)
				return;

			progressBar.Maximum = paths.Length;
			progressBar.Value = 0;
			progressBar.Visible = true;
			lblScanning.Visible = true;

			foreach (string newPath in paths)
			{
				progressBar.Value++;
				Application.DoEvents();

				bool inList = false;
				foreach (string prevPath in lstSrcPaths.Items)
				{
					if (newPath == prevPath)
					{
						inList = true;
						break;
					}
				}

				if (!inList && File.Exists(newPath))
					lstSrcPaths.Items.Add(newPath);
			}
		
			progressBar.Visible = false;
			lblScanning.Visible = false;
		}
	}

	#endregion
}
