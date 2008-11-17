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
			m_project = (project ?? new LocalizerProject());
			m_isPrjNew = (project == null);

			cboTarget.Enabled = m_isPrjNew;
			txtSrc.Enabled = m_isPrjNew;
			btnSrc.Enabled = m_isPrjNew;
			fntDlg.ShowEffects = false;

			if (!m_isPrjNew)
			{
				cboTarget.Text = CultureInfo.GetCultureInfo(m_project.CultureId).DisplayName;
				txtSrc.Text = m_project.SourcePath;
				
				txtSrcTextFont.Tag = m_project.SourceTextFont;
				txtSrcTextFont.Text = string.Format(kfmtFont, m_project.SourceTextFont.Name,
					m_project.SourceTextFont.Style.ToString(), m_project.SourceTextFont.SizeInPoints);

				txtTransFont.Tag = m_project.TranslationFont;
				txtTransFont.Text = string.Format(kfmtFont, m_project.TranslationFont.Name,
					m_project.TranslationFont.Style.ToString(), m_project.TranslationFont.SizeInPoints);
			}
			else
			{
				// REVIEW: Should this be windows-only cultures;
				// only installed cutures; all cultures, etc.?
				foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
				{
					if (!ci.EnglishName.StartsWith("Invariant Language"))
						cboTarget.Items.Add(ci);
				}
			}
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
				if (m_isPrjNew)
				{
					m_project.CultureId = cboTarget.SelectedItem.ToString();
					m_project.SourcePath = txtSrc.Text.Trim();
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
		public LocalizerProject Project
		{
			get { return m_project; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnSrc_Click(object sender, EventArgs e)
		{
			// TODO: Internationalize
			fldrBrowser.Description = "Specify the folder containing the source language resource files.";
			fldrBrowser.ShowNewFolderButton = false;
			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
				txtSrc.Text = fldrBrowser.SelectedPath;
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
		private void HandlePathTextChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = ((cboTarget.SelectedIndex >= 0 || !m_isPrjNew) &&
				Directory.Exists(txtSrc.Text.Trim()) &&
				txtSrcTextFont.Tag != null && txtTransFont.Tag != null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cboTarget_SelectedIndexChanged(object sender, EventArgs e)
		{
			HandlePathTextChanged(null, null);
		}
	}

	#endregion
}
