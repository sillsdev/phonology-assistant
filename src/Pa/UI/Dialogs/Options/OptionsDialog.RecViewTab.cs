using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		/// ------------------------------------------------------------------------------------
		private void InitializeRecViewTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (m_project == null)
			{
				tabOptions.TabPages.Remove(tpgRecView);
				return;
			}

			lblShowFields.Font = FontHelper.UIFont;
			grpFieldSettings.Font = FontHelper.UIFont;
			fldSelGridRecView.Load(m_project.Fields.OrderBy(f => f.DisplayIndexInRecView)
				.Select(f => new KeyValuePair<PaField, bool>(f, f.VisibleInRecView)));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values on the word list tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveRecViewTabChanges()
		{
			if (!IsRecViewTabDirty)
				return;

			int i = 0;
			foreach (var kvp in fldSelGridRecView.GetSelections())
			{
				var field = m_project.GetFieldForDisplayName(kvp.Key);
				field.VisibleInRecView = kvp.Value;
				field.DisplayIndexInRecView = i++;
			}

			App.MsgMediator.SendMessage("RecordViewOptionsChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		private bool IsRecViewTabDirty
		{
			get { return (fldSelGridRecView != null && fldSelGridRecView.IsDirty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Move a field up in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoveRecVwFldUp_Click(object sender, EventArgs e)
		{
			btnMoveRecVwFldUp.Enabled = fldSelGridRecView.MoveSelectedItemUp();
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Move a field down in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoveRecVwFldDown_Click(object sender, EventArgs e)
		{
			btnMoveRecVwFldDown.Enabled = fldSelGridRecView.MoveSelectedItemDown();
			m_dirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the enabled state of the up and down buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void fldSelGridRecView_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			btnMoveRecVwFldUp.Enabled = fldSelGridRecView.CanMoveSelectedItemUp;
			btnMoveRecVwFldDown.Enabled = fldSelGridRecView.CanMoveSelectedItemDown;
		}
	}
}