using System;
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
			rbEnable.Checked = App.ShouldShowSplashScreen;
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
			App.ShouldShowSplashScreen = rbEnable.Checked;
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