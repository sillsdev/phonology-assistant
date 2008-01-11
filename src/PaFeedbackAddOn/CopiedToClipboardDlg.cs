using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.AddOn
{
	public partial class CopiedToClipboardDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CopiedToClipboardDlg()
		{
			InitializeComponent();
			txtMailAddress.Text = Properties.Resources.kstidFeedbackMailAddress;
		}
	}
}