using System;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	public partial class UndefinedCharactersInClassDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedCharactersInClassDlg()
		{
			InitializeComponent();
			lblInfo.Font = FontHelper.UIFont;
			txtChars.Font = FontHelper.PhoneticFont;

			while (true)
			{
				TextFormatFlags flags = TextFormatFlags.Default | TextFormatFlags.WordBreak;
				int height = TextRenderer.MeasureText(lblInfo.Text, lblInfo.Font,
					lblInfo.ClientSize, flags).Height;

				if (height > lblInfo.Height)
					Height++;
				else
					break;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UndefinedCharactersInClassDlg(char[] undefinedChars) : this()
		{
			for (int i = 0; i < undefinedChars.Length; i++)
			{
				txtChars.Text += undefinedChars[i].ToString();
				if (i < undefinedChars.Length - 1)
					txtChars.Text += ", ";
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			App.MsgMediator.SendMessage(Name + "HandleCreated", this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show some temp. help until the help files are ready.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic(this);
		}

		private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}