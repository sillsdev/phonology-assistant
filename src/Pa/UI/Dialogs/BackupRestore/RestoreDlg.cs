using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SIL.Pa.DataSource;
using ICSharpCode.SharpZipLib.Zip;
using SIL.Pa.Properties;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class RestoreDlg : Form
	{
		private const string kTmpRstFolder = "~PaRestore";

		private string m_lastFolderPicked = App.DefaultProjectFolder;
		private string m_prjName;
		private string m_tmpFolder;
		private string m_papPath;
		private BRProgressDlg m_progressDlg;
		private readonly string m_restoreRoot;
		private readonly string m_fmtProjMsg;
		private readonly string m_zipPath;
		private readonly List<string> m_origPaths = new List<string>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Restore()
		{
			var caption = App.L10NMngr.LocalizeString("RestoreDlg.OFDCaption",
				"File to Restore", App.kLocalizationGroupDialogs);

			string zipFile = App.OpenFileDialog("zip",
				App.kstidFileTypeZip + "|" + App.kstidFileTypeAllFiles, caption);

			if (string.IsNullOrEmpty(zipFile) || !File.Exists(zipFile))
				return;

			try
			{
				using (RestoreDlg dlg = new RestoreDlg(zipFile))
					dlg.ShowDialog(App.MainForm);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RestoreDlg()
		{
			InitializeComponent();

			m_restoreRoot = Path.GetPathRoot(Application.StartupPath);
			m_fmtProjMsg = lblProject.Text;
			lblProject.Text = string.Format(m_fmtProjMsg, string.Empty);
			grid.Name = Name + "Grid";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RestoreDlg(string zipPath) : this()
		{
			m_zipPath = zipPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.RestoreDlg = App.InitializeForm(this, Settings.Default.RestoreDlg);
			
			if (Settings.Default.RestoreDlgGrid != null)
				Settings.Default.RestoreDlgGrid.InitializeGrid(grid);
			
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			Settings.Default.RestoreDlgGrid = GridSettings.Create(grid);

			base.OnClosing(e);

			// Clean up the files in the temp. folder.
			if (m_tmpFolder != null && Directory.Exists(m_tmpFolder))
				Directory.Delete(m_tmpFolder, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Application.DoEvents();

			m_progressDlg = new BRProgressDlg();
			m_progressDlg.lblMsg.Text = App.L10NMngr.LocalizeString(
				Name + ".ReadingBackupFileMsg", "Reading backup file...",
				App.kLocalizationGroupDialogs);
			
			m_progressDlg.CenterInParent(this);
			m_progressDlg.Show();
			Application.DoEvents();

			UnpackToTempFolder();

			if (m_origPaths.Count == 0 || string.IsNullOrEmpty(m_papPath) || !File.Exists(m_papPath))
			{
				m_progressDlg.Close();
				m_progressDlg.Dispose();
				m_progressDlg = null;
				Hide();

				var msg = App.L10NMngr.LocalizeString(Name + ".NoPrjInZipFileMsg",
					"The specified zip file does not appear to contain a Phonology Assistant project.",
					App.kLocalizationGroupDialogs);

				Utils.MsgBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Close();
				return;
			}

			GetDataSourcePathsFromPap();
			lblProject.Text = string.Format(m_fmtProjMsg, m_prjName);

			m_progressDlg.Hide();
			m_progressDlg.prgressBar.Value = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UnpackToTempFolder()
		{
			FileStream stream = null;
			ZipInputStream zipStream = null;

			try
			{
				// Create a temporary folder in which to unzip the files.
				m_tmpFolder = Path.Combine(Path.GetTempPath(), kTmpRstFolder);
				if (Directory.Exists(m_tmpFolder))
					Directory.Delete(m_tmpFolder, true);
				Directory.CreateDirectory(m_tmpFolder);

				m_progressDlg.prgressBar.Value = 0;
				ZipFile zipFile = new ZipFile(m_zipPath);
				m_progressDlg.prgressBar.Maximum = (int)zipFile.Count;
				zipFile.Close();

				// Unpack the files to the temp. folder we just created.
				stream = new FileStream(m_zipPath, FileMode.Open);
				zipStream = new ZipInputStream(stream);
				ZipEntry zipEntry;
				while ((zipEntry = zipStream.GetNextEntry()) != null)
				{
					m_progressDlg.prgressBar.Value++;
					UnpackEntry(zipStream, zipEntry, m_tmpFolder);
				}

				m_progressDlg.prgressBar.Value = m_progressDlg.prgressBar.Maximum;
			}
			finally
			{
				if (zipStream != null)
					zipStream.Close();

				if (stream != null)
					stream.Close();

				zipStream = null;
				stream = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Unpack the file specified by the zip entry to the specified path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UnpackEntry(ZipInputStream zipStream, ZipEntry zipEntry, string path)
		{
			string filePath = Path.Combine(path, Path.GetFileName(zipEntry.Name));
			string entryName = zipEntry.Name;
			entryName = entryName.Replace('/', '\\');
			m_origPaths.Add(entryName);

			if (Path.GetExtension(filePath).ToLower() == ".pap")
			{
				txtPrjLocation.Text = Path.Combine(m_restoreRoot, Path.GetDirectoryName(entryName));
				m_papPath = filePath;
			}

			FileInfo fi = new FileInfo(filePath);
			FileStream outStream = null;
			try
			{
				outStream = fi.Create();
				int size = 2048;
				byte[] data = new byte[size];
				while (true)
				{
					if ((size = zipStream.Read(data, 0, data.Length)) == 0)
						break;

					outStream.Write(data, 0, size);
				}
			}
			finally
			{
				if (outStream != null)
					outStream.Close();
				outStream = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Read the data sources from the restored project file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetDataSourcePathsFromPap()
		{
			string dummy = string.Empty;
			PaProject prj = PaProject.LoadProjectFileOnly(m_papPath, true, ref dummy); 

			if (prj == null || prj.DataSources == null || prj.DataSources.Count == 0)
			{
				var msg = App.L10NMngr.LocalizeString(Name + ".PrjIsEmptyMsg",
					"There are no data sources in the project.", App.kLocalizationGroupDialogs);

				Utils.MsgBox(msg);
				return;
			}

			m_progressDlg.prgressBar.Value = 0;
			m_progressDlg.prgressBar.Maximum = prj.DataSources.Count;

			foreach (PaDataSource dataSource in prj.DataSources)
			{
				m_progressDlg.prgressBar.Value++;

				// Skip FieldWorks data source since we don't backup those.
				if (dataSource.DataSourceType != DataSourceType.FW)
				{
					int i = ProcessDataSourceFromPap(dataSource);
					dataSource.DataSourceFile = (i < 0 ? "X" : i.ToString());
				}
			}

			// This should never happen, but just in case, remove all data
			// sources whose file couldn't be found for some reason.
			for (int i = prj.DataSources.Count - 1; i >= 0; i--)
			{
				if (prj.DataSources[i].DataSourceFile == "X")
					prj.DataSources.RemoveAt(i);
			}

			m_progressDlg.prgressBar.Value = m_progressDlg.prgressBar.Maximum;
			m_prjName = prj.Name;
			XmlSerializationHelper.SerializeToFile(m_papPath, prj);
			prj.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int ProcessDataSourceFromPap(PaDataSource dataSource)
		{
			string fileOnly = Path.GetFileName(dataSource.DataSourceFile);
			string pathInPap = Path.GetDirectoryName(dataSource.DataSourceFile);
			string pathInZip = pathInPap;

			// Strip off the root portion of the path (e.g. c:\)
			if (Path.IsPathRooted(pathInZip))
			{
				string root = Path.GetPathRoot(pathInZip);
				pathInZip = pathInZip.Substring(root.Length);
			}

			// Make sure the data source file can be found among those whose
			// entries were found in the backup file. If not (which should
			// never happen), skip the data source and go to the next one.
			int i = m_origPaths.IndexOf(Path.Combine(pathInZip, fileOnly));
			if (i < 0)
				return -1;

			// Remove the path and file name from the list found in the zip file.
			m_origPaths.RemoveAt(i);

			// Make sure the file was extracted from the zip file.
			if (!File.Exists(Path.Combine(m_tmpFolder, fileOnly)))
				return -1;

			// Add a row in the data source grid containing the data source file name
			// and the propsed location where the data source file will be restored
			// (i.e. the path specified in the pap file).
			string path = Path.Combine(m_restoreRoot, pathInPap);
			grid.Rows.Add(new[] { fileOnly, path, path, string.Empty });
			DataGridViewRow row = grid.Rows[grid.RowCount - 1];

			// If the data source is an SA audio file, then make sure to include
			// the audio file's companion transcriptions file.
			if (dataSource.DataSourceType == DataSourceType.SA)
			{
				string saxmlFile = Path.ChangeExtension(fileOnly, "saxml");
				if (File.Exists(Path.Combine(m_tmpFolder, saxmlFile)))
				{
					row.Cells[3].Value = saxmlFile;
					m_origPaths.Remove(Path.Combine(pathInZip, saxmlFile));
				}
			}

			return row.Index;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnRestore_Click(object sender, EventArgs e)
		{
			if (!CheckIfPapAlreadyExists())
				return;

			var msg = App.L10NMngr.LocalizeString(Name + ".RestoringMsg",
				"Restoring {0}...", App.kLocalizationGroupDialogs);

			m_progressDlg.lblMsg.Text = string.Format(msg, m_prjName);
			m_progressDlg.prgressBar.Maximum = m_origPaths.Count + grid.RowCount;
			m_progressDlg.CenterInParent(this);
			m_progressDlg.Show();
			Hide();
			Application.DoEvents();
			RestoreProjectFiles();
			RestoreDataSources();
			m_progressDlg.prgressBar.Value = m_progressDlg.prgressBar.Maximum;
			ModifyDataSourcePathsInRestoredProject();
			m_progressDlg.Hide();
			LoadRestoredProject();
			
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool CheckIfPapAlreadyExists()
		{
			string papFile = Path.GetFileName(m_papPath);

			if (!File.Exists(Path.Combine(txtPrjLocation.Text, papFile)))
				return true;

			var msg = App.L10NMngr.LocalizeString(Name + ".PrjAlreadyInFolderMsg",
				"There is already a project file by the name of '{0}' in the specified restore\nfolder. Do you want to overwrite the existing project files? If you answer 'Yes'\nthen data source files that may already exist will also be overwritten.",
				App.kLocalizationGroupDialogs);

			msg = string.Format(msg, papFile);
			
			return (Utils.MsgBox(msg, MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning) == DialogResult.Yes);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RestoreProjectFiles()
		{
			// Move the project files to the user-specified restore folder.
			foreach (string file in m_origPaths)
			{
				m_progressDlg.prgressBar.Value++;
				string filename = Path.GetFileName(file);
				string dst = Path.Combine(txtPrjLocation.Text, filename);
				if (File.Exists(dst))
					File.Delete(dst);

				File.Move(Path.Combine(m_tmpFolder, filename), dst);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RestoreDataSources()
		{
			// Move the data source files to the user-specified restore folder.
			foreach (DataGridViewRow row in grid.Rows)
			{
				string dstPath = row.Cells[1].Value as string;
				if (!Directory.Exists(dstPath))
					continue;

				m_progressDlg.prgressBar.Value++;
				string filename = row.Cells[0].Value as string;
				string dst = Path.Combine(dstPath, filename);
				if (File.Exists(dst))
					File.Delete(dst);

				File.Move(Path.Combine(m_tmpFolder, filename), dst);

				// Check if this row has an saxml file associated. If so, then move it too.
				string saxmlFile = row.Cells[3].Value as string;
				if (!string.IsNullOrEmpty(saxmlFile))
				{
					dst = Path.Combine(dstPath, saxmlFile);
					if (File.Exists(dst))
						File.Delete(dst);

					File.Move(Path.Combine(m_tmpFolder, saxmlFile), dst);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Goes through the data sources in the project just restored and changes the data
		/// source file paths to the path where the data sources were restored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ModifyDataSourcePathsInRestoredProject()
		{
			m_papPath = Path.Combine(txtPrjLocation.Text, Path.GetFileName(m_papPath));
			var prj = XmlSerializationHelper.DeserializeFromFile<PaProject>(m_papPath);
			if (prj == null)
				return;

			foreach (PaDataSource dataSource in prj.DataSources)
			{
				int i;
				if (int.TryParse(dataSource.DataSourceFile, out i))
				{
					string path = grid[1, i].Value as string;
					string file = grid[0, i].Value as string;
					dataSource.DataSourceFile = Path.Combine(path, file);
				}
			}

			XmlSerializationHelper.SerializeToFile(m_papPath, prj);
			prj.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Ask the user if he would like to load the project just restored. If yes, then
		/// load it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadRestoredProject()
		{
			if (!File.Exists(m_papPath))
				return;

			var msg = App.L10NMngr.LocalizeString(Name + ".LoadPrjMsg",
				"Restore is complete. Would you\nlike to open the restored project?",
				App.kLocalizationGroupDialogs);

			if (Utils.MsgBox(msg, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				PaMainWnd mainWnd = App.MainForm as PaMainWnd;
				if (mainWnd != null)
					ReflectionHelper.CallMethod(mainWnd, "LoadProject", m_papPath);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnChgPrjLocation_Click(object sender, EventArgs e)
		{
			var msg = App.L10NMngr.LocalizeString(Name + ".BrowseForPrjFolderDesc",
				"Specify folder in which to restore {0} project files.", App.kLocalizationGroupDialogs);

			fldrBrowser.Description = string.Format(msg, m_prjName);
			fldrBrowser.SelectedPath = m_lastFolderPicked;

			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
				txtPrjLocation.Text = m_lastFolderPicked = fldrBrowser.SelectedPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnChgDSLocation_Click(object sender, EventArgs e)
		{
			var msg = App.L10NMngr.LocalizeString(Name + ".BrowseForDataSourceFolderDesc",
				"Specify folder in which to restore {0} data source file(s).", App.kLocalizationGroupDialogs);

			fldrBrowser.SelectedPath = m_lastFolderPicked;
			fldrBrowser.Description = string.Format(msg, m_prjName);

			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
			{
				m_lastFolderPicked = fldrBrowser.SelectedPath;
				SetDataSourceRestoreLocation(m_lastFolderPicked);
				foreach (DataGridViewRow row in grid.Rows)
					row.Cells[2].Value = m_lastFolderPicked;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void chkUseSame_CheckedChanged(object sender, EventArgs e)
		{
			btnChgDSLocation.Enabled = !chkUseSame.Checked;

			if (chkUseSame.Checked)
				SetDataSourceRestoreLocation(txtPrjLocation.Text);
			else
			{
				foreach (DataGridViewRow row in grid.Rows)
					row.Cells[1].Value = row.Cells[2].Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the location for all the data sources to the specified path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetDataSourceRestoreLocation(string path)
		{
			foreach (DataGridViewRow row in grid.Rows)
				row.Cells[1].Value = path;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint cells without the focus rectangle and for the data source restore folder
		/// cells, paint the folder using path ellipsis.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.ColumnIndex > 1 ||  e.RowIndex < 0)
				return;

			if (e.ColumnIndex == 0)
			{
				// Draw default everything but focus rectangle.
				DataGridViewPaintParts paintParts = DataGridViewPaintParts.All;
				paintParts &= ~DataGridViewPaintParts.Focus;
				e.Paint(e.ClipBounds, paintParts);
			}
			else
			{
				// Draw default everything but text and focus rectangle.
				DataGridViewPaintParts paintParts = DataGridViewPaintParts.All;
				paintParts &= ~DataGridViewPaintParts.ContentForeground;
				paintParts &= ~DataGridViewPaintParts.Focus;
				e.Paint(e.ClipBounds, paintParts);

				string path = e.Value as string;
				if (string.IsNullOrEmpty(path))
					return;

				Color clr = (grid.Rows[e.RowIndex].Selected ?
					grid.DefaultCellStyle.SelectionForeColor : grid.DefaultCellStyle.ForeColor);

				TextFormatFlags flags = TextFormatFlags.VerticalCenter |
					TextFormatFlags.SingleLine | TextFormatFlags.PathEllipsis |
					TextFormatFlags.PreserveGraphicsClipping;

				if (grid.RightToLeft == RightToLeft.Yes)
					flags |= TextFormatFlags.RightToLeft;

				TextRenderer.DrawText(e.Graphics, path, grid.Font, e.CellBounds, clr, flags);
			}
			
			e.Handled = true;
		}
	}
}
