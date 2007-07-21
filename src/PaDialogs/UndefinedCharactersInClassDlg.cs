using System;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
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
			PaApp.ShowHelpTopic(this);
		}
	}
}