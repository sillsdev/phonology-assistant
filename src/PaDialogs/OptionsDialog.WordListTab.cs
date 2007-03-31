using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;
using SIL.Pa.Resources;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
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
			if (PaApp.Project == null)
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

			// This indicates the tab is for project specific settings.
			tpgWordLists.Tag = true;

			InitializeWordListValues();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initialize the tab page's controls from the project settings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeWordListValues()
		{
			fldSelGridWrdList.Load(true, true);

			chkSaveReorderedColumns.Checked = PaApp.Project.GridLayoutInfo.SaveReorderedCols;
			chkSaveColHdrHeight.Checked = PaApp.Project.GridLayoutInfo.SaveAdjustedColHeaderHeight;
			chkSaveColWidths.Checked = PaApp.Project.GridLayoutInfo.SaveAdjustedColWidths;
			chkAutoAdjustPhoneticCol.Checked = PaApp.Project.GridLayoutInfo.AutoAdjustPhoneticCol;
			nudMaxEticColWidth.Value = PaApp.Project.GridLayoutInfo.AutoAjustedMaxWidth;

			switch (PaApp.Project.GridLayoutInfo.GridLines)
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
		private void SaveWordListTabSettings()
		{
			if (!IsWordListTabDirty)
				return;

			PaApp.Project.GridLayoutInfo.SaveReorderedCols = chkSaveReorderedColumns.Checked;
			PaApp.Project.GridLayoutInfo.SaveAdjustedColHeaderHeight = chkSaveColHdrHeight.Checked;
			PaApp.Project.GridLayoutInfo.SaveAdjustedColWidths = chkSaveColWidths.Checked;
			PaApp.Project.GridLayoutInfo.AutoAdjustPhoneticCol = chkAutoAdjustPhoneticCol.Checked;
			PaApp.Project.GridLayoutInfo.AutoAjustedMaxWidth = (int)nudMaxEticColWidth.Value;

			if (rbGridLinesBoth.Checked)
				PaApp.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.Single;
			else if (rbGridLinesVertical.Checked)
				PaApp.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleVertical;
			else if (rbGridLinesHorizontal.Checked)
				PaApp.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleHorizontal;
			else if (rbGridLinesNone.Checked)
				PaApp.Project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.None;

			fldSelGridWrdList.Save(true);
			PaApp.Project.Save();
			PaApp.MsgMediator.SendMessage("WordListOptionsChanged", null);
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
					PaApp.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.Single) ||
					(rbGridLinesVertical.Checked &&
					PaApp.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleVertical) ||
					(rbGridLinesHorizontal.Checked &&
					PaApp.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleHorizontal) ||
					(rbGridLinesNone.Checked &&
					PaApp.Project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.None));

				return (fldSelGridWrdList.IsDirty || gridLinesChanged ||
					chkSaveReorderedColumns.Checked != PaApp.Project.GridLayoutInfo.SaveReorderedCols ||
					chkSaveColHdrHeight.Checked != PaApp.Project.GridLayoutInfo.SaveAdjustedColHeaderHeight ||
					chkSaveColWidths.Checked != PaApp.Project.GridLayoutInfo.SaveAdjustedColWidths ||
					chkAutoAdjustPhoneticCol.Checked != PaApp.Project.GridLayoutInfo.AutoAdjustPhoneticCol);
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