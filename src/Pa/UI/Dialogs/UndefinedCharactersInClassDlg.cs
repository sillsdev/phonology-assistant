using System;
using System.Globalization;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	public partial class UndefinedCharactersInClassDlg : Form
	{
		/// ------------------------------------------------------------------------------------
		public UndefinedCharactersInClassDlg()
		{
			InitializeComponent();
			lblInfo.Font = FontHelper.UIFont;
			txtChars.Font = App.PhoneticFont;

			while (true)
			{
				const TextFormatFlags flags = TextFormatFlags.Default | TextFormatFlags.WordBreak;
				int height = TextRenderer.MeasureText(lblInfo.Text, lblInfo.Font,
					lblInfo.ClientSize, flags).Height;

				if (height > lblInfo.Height)
					Height++;
				else
					break;
			}
		}
		
		/// ------------------------------------------------------------------------------------
		public UndefinedCharactersInClassDlg(char[] undefinedChars) : this()
		{
			for (int i = 0; i < undefinedChars.Length; i++)
			{
				txtChars.Text += undefinedChars[i].ToString(CultureInfo.InvariantCulture);
				if (i < undefinedChars.Length - 1)
					txtChars.Text += ", ";
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			App.MsgMediator.SendMessage(Name + "HandleCreated", this);
		}

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
	}
}