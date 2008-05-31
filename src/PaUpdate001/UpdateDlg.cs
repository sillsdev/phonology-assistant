using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace SIL.Pa.Updates
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The updater updates the FieldWorks query file (xml file) in the PA installation
	/// folder. There was a bug in the query found a couple of weeks after the corporate
	/// release. See PA-930 for more information. This update will replace the old query
	/// file with a fixed version. In addition, this updater should be used to provide
	/// the query file that will allow users of FieldWorks versions 5.3 or later to see
	/// their FieldWorks data in PA.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class UpdateDlg : Form
	{
		private string[] m_filesToUpdate =
			new string[] {"FwSQLQueries.xml", "FwSQLQueriesShortNames.xml"};

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UpdateDlg()
		{
			InitializeComponent();
			string msg = Properties.Resources.kstidUpdateMsg;
			lblUpdateMsg.Text = msg.Replace("\\n", "\n"); 
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnUpdate_Click(object sender, EventArgs e)
		{
			btnUpdate.Visible = false;
			if (!DoUpdate())
			{
				btnUpdate.Visible = true;
				return;
			}

			lblAdminMsg.Visible = false;
			lblUpdateMsg.Text = Properties.Resources.kstidUpdateCompleteMsg;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool DoUpdate()
		{
			try
			{
				string paPath = PaPathFinder.GetPath();
				if (paPath == null)
					return false;

				foreach (string file in m_filesToUpdate)
					UpdateFile(paPath, file);
			}
			catch (Exception e)
			{
				string msg = Properties.Resources.kstidErrorUpdating.Replace("\\n", "\n");
				msg = string.Format(msg, e.Message);
				MessageBox.Show(msg, Properties.Resources.kstidMsgBoxCaption);
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateFile(string path, string file)
		{
			string fullPath = Path.Combine(path, file);

			// If the file exists, then back it up.
			if (File.Exists(fullPath))
			{
				string oldFile = Path.Combine(path, file);
				while (File.Exists(oldFile))
					oldFile += ".bak";

				File.Move(fullPath, oldFile);
			}

			// Use the file name without its extension as the name of the resource.
			// Then get the string resource that will be the contents of the new file.
			// Then write the new file.
			string resName = Path.GetFileNameWithoutExtension(file);
			string newFileContent = Properties.Resources.ResourceManager.GetString(resName);
			File.WriteAllText(fullPath, newFileContent, Encoding.UTF8);
		}
	}
}
