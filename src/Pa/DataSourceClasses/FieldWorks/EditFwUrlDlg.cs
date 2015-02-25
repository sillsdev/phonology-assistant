// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
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