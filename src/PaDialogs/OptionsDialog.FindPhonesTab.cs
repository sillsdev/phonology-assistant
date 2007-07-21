using System;
using System.Drawing;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	public partial class OptionsDlg
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeFindPhonesTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (PaApp.Project == null)
			{
				tabOptions.TabPages.Remove(tpgFindPhones);
				return;
			}
			
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
			rdoClassName.Checked = PaApp.Project.ShowClassNamesInSearchPatterns;
			rdoClassMembers.Checked = !rdoClassName.Checked;

			chkShowDiamondPattern.Checked = PaApp.Project.ShowDiamondsInEmptySearchPattern;
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
				return rdoClassName.Checked != PaApp.Project.ShowClassNamesInSearchPatterns ||
				  chkShowDiamondPattern.Checked != PaApp.Project.ShowDiamondsInEmptySearchPattern;
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

			if (rdoClassName.Checked != PaApp.Project.ShowClassNamesInSearchPatterns)
			{
				PaApp.Project.ShowClassNamesInSearchPatterns = rdoClassName.Checked;
				PaApp.MsgMediator.SendMessage("ClassDisplayBehaviorChanged", null);
			}

			PaApp.Project.ShowDiamondsInEmptySearchPattern = chkShowDiamondPattern.Checked;
			PaApp.Project.Save();
			PaApp.MsgMediator.SendMessage("FindPhonesSettingsChanged", null);
		}
	}
}
