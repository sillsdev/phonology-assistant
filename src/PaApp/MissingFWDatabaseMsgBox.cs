using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa
{
	public partial class MissingFWDatabaseMsgBox : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public MissingFWDatabaseMsgBox()
		{
			InitializeComponent();
			base.Text = Application.ProductName;
			picIcon.Image = SystemIcons.Information.ToBitmap();
			lblMsg.Font = FontHelper.UIFont;
			lblDBName.Font = FontHelper.UIFont;
			lblDBName.Text = string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DialogResult ShowDialog(string dbName)
		{
			using (MissingFWDatabaseMsgBox msgBox = new MissingFWDatabaseMsgBox())
			{
				msgBox.lblDBName.Text = dbName;
				PaApp.CloseSplashScreen();
				return msgBox.ShowDialog();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			PaApp.ShowHelpTopic(this);
		}
	}
}