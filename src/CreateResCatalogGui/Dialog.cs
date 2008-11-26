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

namespace SIL.Localize.CreateResourceCatalog
{
	#region Dialog class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class Dialog : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Dialog()
		{
			InitializeComponent();
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (!Verify())
				return;

			if (saveFileDlg.ShowDialog(this) != DialogResult.OK)
				return;

			progressBar.Value = 0;
			progressBar.Visible = true;
			lblScanning.Visible = true;
			Application.DoEvents();

			List<string> files = new List<string>();
			foreach (string file in lstSrcPaths.Items)
				files.Add(file);

			Invalidate();
			Application.DoEvents();
			ResourceCatalog list = ResourceCatalogCreator.Create(files, progressBar);
			list.ApplicationName = txtAppName.Text.Trim();

			if (list != null && list.Count > 0)
				LocalizingHelper.SerializeData(saveFileDlg.FileName, list);

			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool Verify()
		{
			string msg = null;

			if (txtAppName.Text.Trim() == string.Empty)
			{
				msg = "You must specify an application name.";
				txtAppName.Focus();
			}
			else if (lstSrcPaths.Items.Count == 0)
			{
			    msg = "You must specify one or more C# Project files.";
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
		private void btnAdd_Click(object sender, EventArgs e)
		{
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
			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
			{
				Application.DoEvents();
				AddFilesToList(Directory.GetFiles(fldrBrowser.SelectedPath, "*.csproj",
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
