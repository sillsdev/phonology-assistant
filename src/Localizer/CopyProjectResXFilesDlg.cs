using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SIL.Localize.LocalizingUtils;

namespace SIL.Localize.Localizer
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CopyProjectResXFilesDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CopyProjectResXFilesDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			//base.OnFormClosing(e);
			//if (btnOK.Enabled && DialogResult == DialogResult.OK)
			//{
			//    Cursor = Cursors.WaitCursor;
			//    LocalizingHelper.CopyProjectResXFiles(txtSrc.Text.Trim(), txtDest.Text.Trim());
			//    Cursor = Cursors.Default;
			//}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnSrc_Click(object sender, EventArgs e)
		{
			// TODO: Internationalize
			fldrBrowser.Description = "Specify the folder containing the .Net project whose .resx files should be copied.";
			fldrBrowser.ShowNewFolderButton = false;
			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
				txtSrc.Text = fldrBrowser.SelectedPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDest_Click(object sender, EventArgs e)
		{
			// TODO: Internationalize
			fldrBrowser.Description = "Specify the folder where the .resx files will copied.";
			fldrBrowser.ShowNewFolderButton = true;
			if (fldrBrowser.ShowDialog(this) == DialogResult.OK)
				txtDest.Text = fldrBrowser.SelectedPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePathTextChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = (Directory.Exists(txtSrc.Text.Trim()) &&
				Directory.Exists(txtDest.Text.Trim()));
		}
	}
}
