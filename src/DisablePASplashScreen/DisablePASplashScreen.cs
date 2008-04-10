using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa
{
	public partial class DisablePASplashScreen : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DisablePASplashScreen()
		{
			InitializeComponent();

			rbEnable.Checked =
				PaApp.SettingsHandler.GetBoolSettingsValue("SplashScreen", "show", true);

			rbDisable.Checked = !rbEnable.Checked;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			PaApp.SettingsHandler.SaveSettingsValue("SplashScreen", "show", rbEnable.Checked);
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
	}
}