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

namespace SIL.Pa
{
	public partial class DisablePASplashScreen : Form
	{
		/// ------------------------------------------------------------------------------------
		public DisablePASplashScreen()
		{
			InitializeComponent();
			rbEnable.Checked = App.ShouldShowSplashScreen;
			rbDisable.Checked = !rbEnable.Checked;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			App.ShouldShowSplashScreen = rbEnable.Checked;
		}

		/// ------------------------------------------------------------------------------------
		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}