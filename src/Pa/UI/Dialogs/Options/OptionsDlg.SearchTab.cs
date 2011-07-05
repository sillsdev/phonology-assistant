
namespace SIL.Pa.UI.Dialogs
{
	public partial class OptionsDlg
	{
		///// ------------------------------------------------------------------------------------
		//private void InitializeFindPhonesTab()
		//{
		//    // This tab isn't valid if there is no project loaded.
		//    if (m_project == null)
		//    {
		//        tabOptions.TabPages.Remove(tpgFindPhones);
		//        return;
		//    }
			
		//    lblClassDisplayBehavior.Font = FontHelper.UIFont;
		//    rdoClassName.Font = FontHelper.UIFont;
		//    rdoClassMembers.Font = FontHelper.UIFont;
		//    chkShowDiamondPattern.Font = FontHelper.UIFont;
		//    lblShowDiamondPattern.Font = FontHelper.UIFont;

		//    lblShowDiamondPattern.Text = string.Format(lblShowDiamondPattern.Text,
		//        App.kEmptyDiamondPattern);

		//    // Adjust the height of the label control to fit the text more tightly.
		//    using (Graphics g = lblClassDisplayBehavior.CreateGraphics())
		//    {
		//        lblClassDisplayBehavior.Height = (int)Math.Ceiling(
		//            g.MeasureString(lblClassDisplayBehavior.Text, FontHelper.UIFont,
		//            lblClassDisplayBehavior.ClientSize.Width).Height) + 2;
		//    }

		//    rdoClassName.Top = lblClassDisplayBehavior.Bottom + 6;
		//    rdoClassMembers.Top = rdoClassName.Bottom + 4;
		//    rdoClassName.Checked = m_project.ShowClassNamesInSearchPatterns;
		//    rdoClassMembers.Checked = !rdoClassName.Checked;

		//    chkShowDiamondPattern.Checked = m_project.ShowDiamondsInEmptySearchPattern;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private bool IsFindPhoneTabDirty
		//{
		//    get
		//    {
		//        return rdoClassName.Checked != m_project.ShowClassNamesInSearchPatterns ||
		//          chkShowDiamondPattern.Checked != m_project.ShowDiamondsInEmptySearchPattern;
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Saves changed find phones information if needed.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void SaveFindPhonesTabChanges()
		//{
		//    if (!IsFindPhoneTabDirty)
		//        return;

		//    if (rdoClassName.Checked != m_project.ShowClassNamesInSearchPatterns)
		//    {
		//        m_project.ShowClassNamesInSearchPatterns = rdoClassName.Checked;
		//        App.MsgMediator.SendMessage("ClassDisplayBehaviorChanged", null);
		//    }

		//    m_project.ShowDiamondsInEmptySearchPattern = chkShowDiamondPattern.Checked;
		//    App.MsgMediator.SendMessage("FindPhonesSettingsChanged", null);
		//}
	}
}
