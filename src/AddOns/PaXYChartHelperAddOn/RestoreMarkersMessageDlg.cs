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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.AddOn
{
	public partial class RestoreMarkersMessageDlg : Form
	{
		private bool m_beforeEditing;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void Show(bool beforeEditing)
		{
			bool showMsg = !PaApp.SettingsHandler.GetBoolSettingsValue("XYChartHelperAddOn",
				beforeEditing ? "dontshowbegineditmsg" : "dontshowsavingmsg", false);

			if (showMsg)
			{
				using (RestoreMarkersMessageDlg dlg = new RestoreMarkersMessageDlg(beforeEditing))
					dlg.ShowDialog();
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RestoreMarkersMessageDlg()
		{
			InitializeComponent();
			Text = Application.ProductName;
			picIcon.Image = SystemIcons.Information.ToBitmap();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RestoreMarkersMessageDlg(bool beforeEditing) : this()
		{
			m_beforeEditing = beforeEditing;

			lblmsg.Text = (beforeEditing ? Properties.Resources.kstidBeforeEditingMsg :
				Properties.Resources.kstidBeforeSavingMsg);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (chkDontShowAgain.Checked)
			{
				PaApp.SettingsHandler.SaveSettingsValue("XYChartHelperAddOn",
					m_beforeEditing ? "dontshowbegineditmsg" : "dontshowsavingmsg", true);
			}
		}
	}
}