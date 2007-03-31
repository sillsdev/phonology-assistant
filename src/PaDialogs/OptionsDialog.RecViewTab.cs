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

			// This indicates the tab is for project specific settings.
			tpgRecView.Tag = true;

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