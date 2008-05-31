using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SIL.Pa;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;
using ICSharpCode.SharpZipLib.Zip;

namespace SIL.Pa.AddOn
{
	public partial class BackupDlg : Form
	{
		private string m_fmtInfo;
		private string m_fmtProgress;
		private string m_backupFile;
		private List<string> m_prjFiles;
		private List<string> m_dsFiles = new List<string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Backup()
		{
			using (BackupDlg dlg = new BackupDlg())
			{
				if (!string.IsNullOrEmpty(dlg.m_backupFile))
					dlg.ShowDialog();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BackupDlg()
		{
			m_backupFile = GetBackupZipFile();
			if (string.IsNullOrEmpty(m_backupFile))
				return;

			InitializeComponent();

			m_fmtInfo = lblInfo.Text;
			m_fmtProgress = lblProgress.Text;
			Initialize();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void Initialize()
		{
			// Get all the project's files.
			m_prjFiles = new List<string>
				(Directory.GetFiles(PaApp.Project.ProjectPath, PaApp.Project.ProjectName + ".*"));

			GetDataSourceFiles();
			lblInfo.Text = string.Format(lblInfo.Text, PaApp.Project.ProjectName, m_backupFile);
			chkIncludeDataSources.Checked = (m_dsFiles.Count > 0);
			chkIncludeDataSources.Enabled = (m_dsFiles.Count > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetDataSourceFiles()
		{
			if (PaApp.Project.DataSources == null)
				return;

			foreach (PaDataSource dataSource in PaApp.Project.DataSources)
			{
				if (dataSource.DataSourceFile != null)
				{
					m_dsFiles.Add(dataSource.DataSourceFile);

					// If the data source is an SA data source, then make sure the file
					// containing the transcriptions is also included in the back up.
					if (dataSource.DataSourceType == DataSourceType.SA)
						m_dsFiles.Add(Path.ChangeExtension(dataSource.DataSourceFile, "saxml"));
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private string GetBackupZipFile()
		{
			string fileName = string.Format(Properties.Resources.kstidBackupFilenameFmt,
				PaApp.Project.ProjectName, DateTime.Now.ToShortDateString());

			// Slashes are invalid in a file name.
			fileName = fileName.Replace("/", "-");

			int filterIndex = 0;
			return PaApp.SaveFileDialog("zip", Properties.Resources.kstidFileTypesForOFD,
				ref filterIndex, Properties.Resources.kstidBackupOFDCaption, fileName,
				PaApp.Project.ProjectPath);
		}	

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnBkup_Click(object sender, EventArgs e)
		{
			if (!chkIncludeDataSources.Checked)
				m_dsFiles.Clear();

			m_dsFiles.Add(STUtils.GetLocalPath(DataUtils.IPACharCache.CacheFileName, true));

			string normalizationExceptionFile = ReflectionHelper.GetField(
				typeof(FFNormalizer), "kstidNormalizationExceptionsFile") as string;

			if (!string.IsNullOrEmpty(normalizationExceptionFile))
				m_dsFiles.Add(Path.Combine(Application.StartupPath, normalizationExceptionFile));

			lblInfo.Visible = false;
			chkIncludeDataSources.Visible = false;
			btnBkup.Enabled = false;
			btnCancel.Enabled = false;

			lblProgress.Text = string.Empty;
			lblProgress.Visible = true;
			prgBar.Maximum = m_prjFiles.Count + m_dsFiles.Count;
			prgBar.Visible = true;
		
			ZipFile zip = ZipFile.Create(m_backupFile);
			BackupFileList(zip, m_prjFiles);
			BackupFileList(zip, m_dsFiles);
			prgBar.Value = m_prjFiles.Count + m_dsFiles.Count;
			Application.DoEvents();
			zip.Close();

			btnCancel.Enabled = true;
			btnCancel.Text = Properties.Resources.kstidCloseButtonText;
			prgBar.Visible = false;
			lblProgress.Visible = false;
			lblInfo.Text = Properties.Resources.kstidBackupCompleteMsg;
			lblInfo.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BackupFileList(ZipFile zip, List<string> list)
		{
			foreach (string filename in list)
			{
				if (File.Exists(filename))
				{
					lblProgress.Text = string.Format(m_fmtProgress, Path.GetFileName(filename));
					Application.DoEvents();
					zip.BeginUpdate();
					zip.Add(filename);
					zip.CommitUpdate();
				}

				prgBar.Value++;
			}
		}
	}
}
