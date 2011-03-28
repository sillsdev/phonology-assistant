using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		/// ------------------------------------------------------------------------------------
		private void InitializeWordListTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (m_project == null)
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
			fldSelGridWrdList.Load(from field in m_project.GetMappedFields()
								   orderby field.DisplayIndexInGrid
								   select new KeyValuePair<PaField, bool>(field, field.VisibleInGrid));

			chkSaveReorderedColumns.Checked = m_project.GridLayoutInfo.SaveReorderedCols;
			chkSaveColHdrHeight.Checked = m_project.GridLayoutInfo.SaveAdjustedColHeaderHeight;
			chkSaveColWidths.Checked = m_project.GridLayoutInfo.SaveAdjustedColWidths;
			chkAutoAdjustPhoneticCol.Checked = m_project.GridLayoutInfo.AutoAdjustPhoneticCol;
			nudMaxEticColWidth.Value = m_project.GridLayoutInfo.AutoAjustedMaxWidth;

			switch (m_project.GridLayoutInfo.GridLines)
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

			m_project.GridLayoutInfo.SaveReorderedCols = chkSaveReorderedColumns.Checked;
			m_project.GridLayoutInfo.SaveAdjustedColHeaderHeight = chkSaveColHdrHeight.Checked;
			m_project.GridLayoutInfo.SaveAdjustedColWidths = chkSaveColWidths.Checked;
			m_project.GridLayoutInfo.AutoAdjustPhoneticCol = chkAutoAdjustPhoneticCol.Checked;
			m_project.GridLayoutInfo.AutoAjustedMaxWidth = (int)nudMaxEticColWidth.Value;

			if (rbGridLinesBoth.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.Single;
			else if (rbGridLinesVertical.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleVertical;
			else if (rbGridLinesHorizontal.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.SingleHorizontal;
			else if (rbGridLinesNone.Checked)
				m_project.GridLayoutInfo.GridLines = DataGridViewCellBorderStyle.None;

			int i = 0;
			foreach (var kvp in fldSelGridWrdList.GetSelections())
			{
				var field = m_project.GetFieldForDisplayName(kvp.Key);
				field.VisibleInGrid = kvp.Value;
				field.DisplayIndexInGrid = i++;
			}

			App.MsgMediator.SendMessage("WordListOptionsChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		private bool IsWordListTabDirty
		{
			get
			{
				if (!tabOptions.TabPages.Contains(tpgWordLists))
					return false;
				
				bool gridLinesChanged = ((rbGridLinesBoth.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.Single) ||
					(rbGridLinesVertical.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleVertical) ||
					(rbGridLinesHorizontal.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.SingleHorizontal) ||
					(rbGridLinesNone.Checked &&
					m_project.GridLayoutInfo.GridLines != DataGridViewCellBorderStyle.None));

				return (fldSelGridWrdList.IsDirty || gridLinesChanged ||
					chkSaveReorderedColumns.Checked != m_project.GridLayoutInfo.SaveReorderedCols ||
					chkSaveColHdrHeight.Checked != m_project.GridLayoutInfo.SaveAdjustedColHeaderHeight ||
					chkSaveColWidths.Checked != m_project.GridLayoutInfo.SaveAdjustedColWidths ||
					chkAutoAdjustPhoneticCol.Checked != m_project.GridLayoutInfo.AutoAdjustPhoneticCol);
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