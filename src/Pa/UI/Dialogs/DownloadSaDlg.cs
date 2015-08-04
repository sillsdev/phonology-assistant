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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using L10NSharp;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class DownloadSaDlg : Form
	{
		private readonly bool _appWaitCursorOn;
		private bool isLoading;
		/// ------------------------------------------------------------------------------------
		public DownloadSaDlg()
		{
			InitializeComponent();
			string DownloadMessage1 = string.Empty;
			string DownloadMessage2 = string.Empty;
			DownloadMessage1 = LocalizationManager.GetString(
				"DownloadSaDlg.DownloadMessage1",
				"<html><body style='margin: -100px;padding:-100px;'>Speech Analyzer can be downloaded from the SIL website <a href=''>{0}</a>.</body></html>");
			DownloadMessage2 = LocalizationManager.GetString(
				"DownloadSaDlg.DownloadMessage2",
				"by clicking here");
			DownloadMessage1 = string.Format(DownloadMessage1, DownloadMessage2);
			webBrowser1.DocumentText = DownloadMessage1;
			webBrowser1.Font = FontHelper.UIFont;
			webBrowser2.Font = FontHelper.UIFont;
			lblMessage.Font = FontHelper.UIFont;
			picIcon.Image = SystemIcons.Information.ToBitmap();

			var link = Properties.Settings.Default.SaWebsiteLink;
			DownloadMessage1 = LocalizationManager.GetString(
				"DownloadSaDlg.WebsiteMessage1",
				"<html><body style='margin: -100px;padding:-100px;'>For more information about Speech Analyzer, visit the SIL website at <a href=''>{0}</a>.</body></html>");
			DownloadMessage1 = string.Format(DownloadMessage1, link);
			webBrowser2.DocumentText = DownloadMessage1;
			isLoading = true;
			_appWaitCursorOn = Application.UseWaitCursor;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		public DownloadSaDlg(string message)
			: this()
		{
			lblMessage.Text = message;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(System.EventArgs e)
		{
			base.OnShown(e);
			TopMost = true;
			Focus();
			BringToFront();
			TopMost = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			Utils.WaitCursors(_appWaitCursorOn);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int dy = webBrowser1.Top - webBrowser1.Margin.Top;

			e.Graphics.DrawLine(SystemPens.ControlDark, webBrowser1.Left, dy,
				webBrowser1.Right, dy);

			dy = webBrowser1.Bottom + webBrowser1.Margin.Bottom;

			e.Graphics.DrawLine(SystemPens.ControlDark, webBrowser1.Left, dy,
				webBrowser1.Right, dy);
		}

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Escape:
                    {
                        this.Close();
                        return true;
                    }
                case Keys.Control | Keys.Tab:
                    {
                        return true;
                    }
                case Keys.Control | Keys.Shift | Keys.Tab:
                    {
                        return true;
                    }
            }
            return base.ProcessCmdKey(ref message, keys);
        }

		private void webBrowser1_Navigating_1(object sender, WebBrowserNavigatingEventArgs e)
		{
			if (isLoading)
			{
				e.Cancel = true;
				System.Diagnostics.Process.Start(Properties.Settings.Default.SaDownloadLink);
			}
		}

		private void webBrowser2_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if(isLoading)
			{
				e.Cancel = true;
				System.Diagnostics.Process.Start(Properties.Settings.Default.SaWebsiteLink);
			}
		}
	}
}
