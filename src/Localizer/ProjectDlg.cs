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
		public ProjectDlg(LocalizerProject project)
			: this()
		{
			m_isPrjNew = (project == null);
			m_project = (project ?? new LocalizerProject());

			txtPrjName.Enabled = m_isPrjNew;
			cboTarget.Enabled = m_isPrjNew;
			txtExe.Enabled = m_isPrjNew;
			txtSrc.Enabled = m_isPrjNew;
			btnExe.Enabled = m_isPrjNew;
			btnSrc.Enabled = m_isPrjNew;
			rbScanResx.Enabled = m_isPrjNew;
			rbScanDll.Enabled = m_isPrjNew;
			fntDlg.ShowEffects = false;

			Font fntSrcText;
			Font fntTrans;

			if (m_isPrjNew)
			{
				// REVIEW: Should this be windows-only cultures;
				// only installed cutures; all cultures, etc.?
				foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
				{
					if (!ci.EnglishName.StartsWith("Invariant Language"))
						cboTarget.Items.Add(ci);
				}

				fntSrcText = new Font("Tahoma", 9f, FontStyle.Regular, GraphicsUnit.Point);
				fntTrans = fntSrcText;
			}
			else
			{
				txtPrjName.Text = m_project.ProjectName;
				cboTarget.Text = CultureInfo.GetCultureInfo(m_project.CultureId).DisplayName;
				txtExe.Text = m_project.ExePath;
				txtSrc.Text = m_project.SourcePath;
				fntSrcText = m_project.SourceTextFont;
				fntTrans = m_project.TranslationFont;
			}

			txtSrcTextFont.Tag = fntSrcText;
			txtSrcTextFont.Text = string.Format(kfmtFont, fntSrcText.Name,
				fntSrcText.Style.ToString(), fntSrcText.SizeInPoints);

			txtTransFont.Tag = fntTrans;
			txtTransFont.Text = string.Format(kfmtFont, fntTrans.Name,
				fntTrans.Style.ToString(), fntTrans.SizeInPoints);

			rbScanResx.Checked = m_project.ScanResXFiles;
			rbScanDll.Checked = !m_project.ScanResXFiles;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			// TODO: Verify source path exists and that destination path is in valid format.

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
					m_project.ExePath = txtExe.Text.Trim();
					m_project.SourcePath = txtSrc.Text.Trim();
					m_project.ScanResXFiles = rbScanResx.Checked;
				}

				m_project.SourceTextFont = txtSrcTextFont.Tag as Font;
				m_project.TranslationFont = txtTransFont.Tag as Font;
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
			else if (txtSrc.Text.Trim() == string.Empty)
			{
				msg = "You must specify the source folder to scan for resources.";
				txtSrc.Focus();
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

			if (openFileDlg.ShowDialog() == DialogResult.OK)
			{
				txtExe.Text = openFileDlg.FileName;
				if (txtSrc.Text.Trim() == string.Empty)
					txtSrc.Text = Path.GetDirectoryName(openFileDlg.FileName);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnSrc_Click(object sender, EventArgs e)
		{
			// TODO: Internationalize
			fldrBrowser.Description = "Specify the folder containing the source language resources.";
			fldrBrowser.ShowNewFolderButton = false;

			if (Directory.Exists(txtSrc.Text.Trim()))
				fldrBrowser.SelectedPath = txtSrc.Text.Trim();

			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
				txtSrc.Text = fldrBrowser.SelectedPath;
		}
	}

	#endregion
}
