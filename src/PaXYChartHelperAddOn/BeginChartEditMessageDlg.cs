using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.Pa.AddOn
{
	public partial class BeginChartEditMessageDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static new void Show()
		{
			if (!PaApp.SettingsHandler.GetBoolSettingsValue("XYChartHelperAddOn",
				"dontshowbegineditmsg", false))
			{
				using (BeginChartEditMessageDlg dlg = new BeginChartEditMessageDlg())
					dlg.ShowDialog();
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BeginChartEditMessageDlg()
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
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (chkDontShowAgain.Checked)
			{
				PaApp.SettingsHandler.SaveSettingsValue("XYChartHelperAddOn",
					"dontshowbegineditmsg", true);
			}
		}
	}
}