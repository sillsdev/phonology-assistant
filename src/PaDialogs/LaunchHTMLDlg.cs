using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	public partial class LaunchHTMLDlg : Form
	{
		public const string kstidHTMLExportSetting = "HTMLExport";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether or not to show the exported html file, the dialog asking the
		/// user or both.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void PostExportProcess(Form parent, string filename)
		{
			bool autoLaunch = true;
			bool showdialog = !PaApp.SettingsHandler.GetBoolSettingsValue(
				kstidHTMLExportSetting, "dontshowdialog", false);

			if (showdialog)
			{
				using (LaunchHTMLDlg dlg = new LaunchHTMLDlg())
					autoLaunch = (dlg.ShowDialog(parent) == DialogResult.Yes);
			}
			else
			{
				autoLaunch = PaApp.SettingsHandler.GetBoolSettingsValue(kstidHTMLExportSetting,
					"autolaunch", false);
			}

			if (autoLaunch && !string.IsNullOrEmpty(filename))
				Process.Start("\"" + filename + "\"");
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LaunchHTMLDlg()
		{
			InitializeComponent();

			chkAlwaysOpen.Font = FontHelper.UIFont;
			chkDontShowAgain.Font = FontHelper.UIFont;
			lblQuestion.Font = FontHelper.UIFont;

			chkAlwaysOpen.Checked =
				PaApp.SettingsHandler.GetBoolSettingsValue(kstidHTMLExportSetting,
				"autolaunch", false);

			chkDontShowAgain.Checked =
				PaApp.SettingsHandler.GetBoolSettingsValue(kstidHTMLExportSetting,
				"dontshowdialog", false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void chkAlwaysOpen_CheckedChanged(object sender, EventArgs e)
		{
			chkDontShowAgain.Enabled = !chkAlwaysOpen.Checked;
			chkDontShowAgain.Checked = chkAlwaysOpen.Checked;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnYes_Click(object sender, EventArgs e)
		{
			Close();
			DialogResult = DialogResult.Yes;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnNo_Click(object sender, EventArgs e)
		{
			Close();
			DialogResult = DialogResult.No;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{

		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			PaApp.SettingsHandler.SaveSettingsValue(kstidHTMLExportSetting, "dontshowdialog",
				chkDontShowAgain.Checked);

			PaApp.SettingsHandler.SaveSettingsValue(kstidHTMLExportSetting, "autolaunch",
				chkAlwaysOpen.Checked);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint an etched line above the buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				Point pt1 = new Point(10, btnYes.Top - 10);
				Point pt2 = new Point(ClientRectangle.Right - 10, pt1.Y);
				e.Graphics.DrawLine(pen, pt1, pt2);
				pt1.Y++;
				pt2.Y++;
				pen.Color = SystemColors.ControlLight;
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}
	}
}