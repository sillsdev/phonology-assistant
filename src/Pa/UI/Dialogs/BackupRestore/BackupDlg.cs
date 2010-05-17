using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using SIL.Localization;
using SIL.Pa.DataSource;
using ICSharpCode.SharpZipLib.Zip;
using SIL.Pa.Properties;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class BackupDlg : Form
	{
		//private string m_fmtInfo;
		private readonly string m_fmtProgress;
		private readonly string m_backupFile;
		private readonly List<string> m_prjFiles;
		private readonly List<string> m_dsFiles = new List<string>();
		private BRProgressDlg m_progressDlg;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Backup()
		{
			using (BackupDlg dlg = new BackupDlg())
			{
				try
				{
					if (!string.IsNullOrEmpty(dlg.m_backupFile))
						dlg.ShowDialog(App.MainForm);
				}
				catch { }
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

			btnClose.Location = btnCancel.Location;

			//m_fmtInfo = lblInfo.Text;
			m_fmtProgress = lblProgress.Text;

			try
			{
				// Get all the project's files.
				m_prjFiles = new List<string>
					(Directory.GetFiles(App.Project.Folder, App.Project.Name + ".*"));

				GetDataSourceFiles();
				lblInfo.Text = string.Format(lblInfo.Text, App.Project.Name, m_backupFile);
				chkIncludeDataSources.Checked = (m_dsFiles.Count > 0);
				chkIncludeDataSources.Enabled = (m_dsFiles.Count > 0);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.BackupDlg = App.InitializeForm(this, Settings.Default.BackupDlg);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetDataSourceFiles()
		{
			if (App.Project.DataSources == null)
				return;

			foreach (PaDataSource dataSource in App.Project.DataSources)
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
			var fmt = LocalizationManager.LocalizeString(Name + ".BackupFilenameFmt",
				"{0} Backup ({1}).zip", App.kLocalizationGroupDialogs);

			var fileName = string.Format(fmt, App.Project.Name, DateTime.Now.ToShortDateString());

			// Slashes are invalid in a file name.
			fileName = fileName.Replace("/", "-");

			var caption = LocalizationManager.LocalizeString(Name + ".BackupOFDCaption",
				"Backup File to Create", App.kLocalizationGroupDialogs);
			
			int filterIndex = 0;

			return App.SaveFileDialog("zip", App.kstidFileTypeZip + "|" + App.kstidFileTypeAllFiles,
				ref filterIndex, caption, fileName, App.Project.Folder);
		}	

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnBkup_Click(object sender, EventArgs e)
		{
			// I'm not sure what I was doing with all these changes. This version of the click
			// event is very different from version 1.0.0 of the release version of this DLL.
			// Apparently, I started working on some changes and stopped before they were
			// finished, and upon returning to the code, I don't recall what I was doing. I
			// will have to get into the code later to see if I can figure it all out.

			m_progressDlg = new BRProgressDlg();
			m_progressDlg.lblMsg.Text = string.Empty;
			m_progressDlg.prgressBar.Maximum = m_prjFiles.Count + m_dsFiles.Count;
			m_progressDlg.CenterInParent(this);
			m_progressDlg.Show();
			Hide();
			Application.DoEvents();
			//RestoreProjectFiles();
			//RestoreDataSources();
			//m_progressDlg.prgressBar.Value = m_progressDlg.prgressBar.Maximum;
			//ModifyDataSourcePathsInRestoredProject();
			m_progressDlg.Hide();
			//LoadRestoredProject();

			if (!chkIncludeDataSources.Checked)
				m_dsFiles.Clear();

			// Fix the following lines, since we don't want to use GetLocalPath anymore.
			//m_dsFiles.Add(Utils.GetLocalPath(InventoryHelper.kDefaultInventoryFileName, true));

			//var normalizationExceptionFile =
			//	Utils.GetLocalPath(FFNormalizer.kstidNormalizationExceptionsFile, true);
			
			//if (File.Exists(normalizationExceptionFile))
			//	m_dsFiles.Add(normalizationExceptionFile);

			lblInfo.Visible = false;
			chkIncludeDataSources.Visible = false;
			btnBkup.Visible = false;
			btnCancel.Enabled = false;

			lblProgress.Text = string.Empty;
			lblProgress.Visible = true;
			//prgBar.Maximum = m_prjFiles.Count + m_dsFiles.Count;
			//prgBar.Visible = true;
		
			ZipFile zip = ZipFile.Create(m_backupFile);
			BackupFileList(zip, m_prjFiles);
			BackupFileList(zip, m_dsFiles);
			prgBar.Value = m_prjFiles.Count + m_dsFiles.Count;
			Application.DoEvents();
			zip.Close();

			btnCancel.Visible = false;
			btnClose.Visible = true;
			
			//prgBar.Visible = false;
			//lblProgress.Visible = false;
			//lblInfo.Text = Properties.Resources.kstidBackupCompleteMsg;
			//lblInfo.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BackupFileList(ZipFile zip, IEnumerable<string> list)
		{
			foreach (string filename in list)
			{
				if (File.Exists(filename))
				{
					//lblProgress.Text = string.Format(m_fmtProgress, Path.GetFileName(filename));
					m_progressDlg.lblMsg.Text = string.Format(m_fmtProgress, Path.GetFileName(filename));
					Application.DoEvents();
					zip.BeginUpdate();
					zip.Add(filename);
					zip.CommitUpdate();
				}

				m_progressDlg.prgressBar.Value++;
				//prgBar.Value++;
			}
		}
	}
}
