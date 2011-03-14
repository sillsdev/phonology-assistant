using System;
using System.Drawing;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.DataSource
{
	public partial class MissingFWDatabaseMsgBox : Form
	{
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
		public static DialogResult ShowDialog(string dbName)
		{
			using (var msgBox = new MissingFWDatabaseMsgBox())
			{
				msgBox.lblDBName.Text = dbName;
				App.CloseSplashScreen();
				return msgBox.ShowDialog();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic(this);
		}
	}
}