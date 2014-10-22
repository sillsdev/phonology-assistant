using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class DownloadSaDlg : Form
	{
		private readonly bool _appWaitCursorOn;

		/// ------------------------------------------------------------------------------------
		public DownloadSaDlg()
		{
			InitializeComponent();

			lnkSaDownload.Font = FontHelper.UIFont;
			lnkSaWebsite.Font = FontHelper.UIFont;
			lblMessage.Font = FontHelper.UIFont;

			picIcon.Image = SystemIcons.Information.ToBitmap();

			var link = Properties.Settings.Default.SaWebsiteLink;
			lnkSaWebsite.Text = string.Format(lnkSaWebsite.Text, link);
			lnkSaWebsite.Links.Add(lnkSaWebsite.Text.IndexOf(link, System.StringComparison.Ordinal), link.Length);

			_appWaitCursorOn = Application.UseWaitCursor;
			SilTools.Utils.WaitCursors(false);
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
			SilTools.Utils.WaitCursors(_appWaitCursorOn);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int dy = lnkSaDownload.Top - lnkSaDownload.Margin.Top;

			e.Graphics.DrawLine(SystemPens.ControlDark, lnkSaDownload.Left, dy,
				lnkSaDownload.Right, dy);
			
			dy = lnkSaDownload.Bottom + lnkSaDownload.Margin.Bottom;

			e.Graphics.DrawLine(SystemPens.ControlDark, lnkSaDownload.Left, dy,
				lnkSaDownload.Right, dy);
		}

		/// ------------------------------------------------------------------------------------
		private void lnkSaDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(Properties.Settings.Default.SaDownloadLink);
		}

		/// ------------------------------------------------------------------------------------
		private void lnkSaWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(Properties.Settings.Default.SaWebsiteLink);
		}
	}
}
