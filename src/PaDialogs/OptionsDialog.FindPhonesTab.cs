using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using SIL.Pa.Resources;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	public partial class OptionsDlg
	{
		private const string kEmptyDiamondPattern = "\u25CA/\u25CA_\u25CA";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeFindPhonesTab()
		{
			lblClassDisplayBehavior.Font = FontHelper.UIFont;
			rdoClassName.Font = FontHelper.UIFont;
			rdoClassMembers.Font = FontHelper.UIFont;
			chkShowDiamondPattern.Font = FontHelper.UIFont;
			lblShowDiamondPattern.Font = FontHelper.UIFont;

			lblShowDiamondPattern.Text = string.Format(lblShowDiamondPattern.Text,
				DataUtils.kEmptyDiamondPattern);

			// Adjust the height of the label control to fit the text more tightly.
			using (Graphics g = lblClassDisplayBehavior.CreateGraphics())
			{
				lblClassDisplayBehavior.Height = (int)Math.Ceiling(
					g.MeasureString(lblClassDisplayBehavior.Text, FontHelper.UIFont,
					lblClassDisplayBehavior.ClientSize.Width).Height) + 2;
			}

			rdoClassName.Top = lblClassDisplayBehavior.Bottom + 6;
			rdoClassMembers.Top = rdoClassName.Bottom + 4;
			rdoClassName.Checked = PaApp.ShowClassNames;
			rdoClassMembers.Checked = !rdoClassName.Checked;

			chkShowDiamondPattern.Checked = PaApp.ShowEmptyDiamondSearchPattern;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsFindPhoneTabDirty
		{
			get
			{
				return rdoClassName.Checked != PaApp.ShowClassNames ||
				  chkShowDiamondPattern.Checked != PaApp.ShowEmptyDiamondSearchPattern;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves changed find phones information if needed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveFindPhonesTabSettings()
		{
			if (!IsFindPhoneTabDirty)
				return;

			if (rdoClassName.Checked != PaApp.ShowClassNames)
				PaApp.MsgMediator.SendMessage("ClassDisplayBehaviorChanged", null);

			PaApp.ShowClassNames = rdoClassName.Checked;
			PaApp.ShowEmptyDiamondSearchPattern = chkShowDiamondPattern.Checked;

			PaApp.MsgMediator.SendMessage("FindPhonesSettingsChanged", null);
		}
	}
}
