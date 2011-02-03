using System;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.DataSource.FieldWorks
{
	/// ----------------------------------------------------------------------------------------
	public partial class EditFwUrlDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		public EditFwUrlDlg()
		{
			InitializeComponent();
			txtUrl.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		public EditFwUrlDlg(string url) : this()
		{
			txtUrl.Text = url;
		}

		/// ------------------------------------------------------------------------------------
		public string Url
		{
			get { return txtUrl.Text; }
		}

		/// ------------------------------------------------------------------------------------
		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}