using System;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa
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
		private void InitializeRecViewTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (PaApp.Project == null)
			{
				tabOptions.TabPages.Remove(tpgRecView);
				return;
			}

			lblShowFields.Font = FontHelper.UIFont;
			grpFieldSettings.Font = FontHelper.UIFont;
			fldSelGridRecView.Load(true, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values on the word list tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveRecViewTabSettings()
		{
			if (!IsRecViewTabDirty)
				return;

			fldSelGridRecView.Save(false);
			PaApp.Project.Save();
			PaApp.MsgMediator.SendMessage("RecordViewOptionsChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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