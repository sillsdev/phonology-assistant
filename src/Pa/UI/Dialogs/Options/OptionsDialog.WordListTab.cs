using System;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeWordListTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (App.Project == null)
			{
				tabOptions.TabPages.Remove(tpgWordLists);
				return;
			}

			lblShowColumns.Font = FontHelper.UIFont;
			lblExplanation.Font = FontHelper.UIFont;
			grpColChanges.Font = FontHelper.UIFont;
			grpColSettings.Font = FontHelper.UIFont;
			grpGridLines.Font = FontHelper.UIFont;
			rbGridLinesBoth.Font = FontHelper.UIFont;
			rbGridLinesHorizontal.Font = FontHelper.UIFont;
			rbGridLinesNone.Font = FontHelper.UIFont;
			rbGridLinesVertical.Font = FontHelper.UIFont;
			chkSaveReorderedColumns.Font = FontHelper.UIFont;
			chkSaveColHdrHeight.Font = FontHelper.UIFont;
			chkSaveColWidths.Font = FontHelper.UIFont;
			chkAutoAdjustPhoneticCol.Font = FontHelper.UIFont;
			nudMaxEticColWidth.Font = FontHelper.UIFont;

			InitializeWordListValues();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Reset the tab page's controls from the project settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeWordListValues()
		{
			fldSelGridWrdList.Load(true, true);

			chkSaveReorderedColumns.Checked = App.Project.GridLayoutInfo.SaveReorderedCols;
			chkSaveColHdrHeight.Checked = App.Project.GridLayoutInfo.SaveAdjustedColHeaderHeight;
			chkSaveColWidths.Checked = App.Project.GridLayoutInfo.SaveAdjustedColWidths;
			chkAutoAdjustPhoneticCol.Checked = App.Project.GridLayoutInfo.AutoAdjustPhoneticCol;
			nudMaxEticColWidth.Value = App.Project.GridLayoutInfo.AutoAjustedMaxWidth;

			switch (App.Project.GridLayoutInfo.GridLines)
			{
				case DataGridViewCellBorderStyle.Single:
					rbGridLinesBoth.Checked = true;
					break;

				case DataGridViewCellBorderStyle.SingleVertical:
					rbGridLinesVertical.Checked = true;
					break;

				case DataGridViewCellBorderStyle.SingleHorizontal:
					rbGridLinesHorizontal.Checked = true;
					break;

				case DataGridViewCellBorderStyle.None:
					rbGridLinesNone.Checked = true;
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values on the word list tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveWordListTabChanges()
		{
			if (!IsWordListTabDirty)
				return;

			App.Project.GridLayoutInfo.SaveReorderedCols = chkSaveReorderedColumns.Checked;
			App.Project.GridLayoutInfo.SaveAdjustedColHeaderHeight = chkSaveColHdrHeight.Checked;
			App.Project.GridLayoutInfo.SaveAdjustedColWidths = chkSaveColWidths.Checked;
			App.Project.GridLayoutInfo.AutoAdjustPhoneticCol = chkAutoAdjustPhoneticCol.Checked;
			App.Project.GridLayoutInfo.AutoAjustedMaxWidth = (int)nudMaxEticColWidth.Value;

			if (rbGridLinesBoth.Checked)
				App.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.Single;
			else if (rbGridLinesVertical.Checked)
				App.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleVertical;
			else if (rbGridLinesHorizontal.Checked)
				App.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleHorizontal;
			else if (rbGridLinesNone.Checked)
				App.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.None;

			fldSelGridWrdList.Save(true);
			App.MsgMediator.SendMessage("WordListOptionsChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsWordListTabDirty
		{
			get
			{
				if (!tabOptions.TabPages.Contains(tpgWordLists))
					return false;
				
				bool gridLinesChanged = ((rbGridLinesBoth.Checked &&
					App.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.Single) ||
					(rbGridLinesVertical.Checked &&
					App.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleVertical) ||
					(rbGridLinesHorizontal.Checked &&
					App.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleHorizontal) ||
					(rbGridLinesNone.Checked &&
					App.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.None));

				return (fldSelGridWrdList.IsDirty || gridLinesChanged ||
					chkSaveReorderedColumns.Checked != App.Project.GridLayoutInfo.SaveReorderedCols ||
					chkSaveColHdrHeight.Checked != App.Project.GridLayoutInfo.SaveAdjustedColHeaderHeight ||
					chkSaveColWidths.Checked != App.Project.GridLayoutInfo.SaveAdjustedColWidths ||
					chkAutoAdjustPhoneticCol.Checked != App.Project.GridLayoutInfo.AutoAdjustPhoneticCol);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the enabled state of the max. automatic phonetic column width.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void chkAutoAdjustPhoneticCol_CheckedChanged(object sender, EventArgs e)
		{
			nudMaxEticColWidth.Enabled = chkAutoAdjustPhoneticCol.Checked;
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Move a column up in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoveColUp_Click(object sender, EventArgs e)
		{
			btnMoveColUp.Enabled = fldSelGridWrdList.MoveSelectedItemUp();
			btnMoveColDown.Enabled = fldSelGridWrdList.CanMoveSelectedItemDown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Move a column down in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoveColDown_Click(object sender, EventArgs e)
		{
			btnMoveColDown.Enabled = fldSelGridWrdList.MoveSelectedItemDown();
			btnMoveColUp.Enabled = fldSelGridWrdList.CanMoveSelectedItemUp;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the enabled state of the up and down buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void fldSelGridWrdList_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			btnMoveColUp.Enabled = fldSelGridWrdList.CanMoveSelectedItemUp;
			btnMoveColDown.Enabled = fldSelGridWrdList.CanMoveSelectedItemDown;
		}
	}
}