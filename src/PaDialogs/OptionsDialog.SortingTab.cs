using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OptionsDlg
	{
		SortOptionsTypeComboItem m_prevListType = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeSortingTab()
		{
			// This tab isn't valid if there is no project loaded.
			if (PaApp.Project == null)
			{
				tabOptions.TabPages.Remove(tpgSorting);
				return;
			}

			lblSortInfo.Font = FontHelper.UIFont;
			lblSortFldsHdr.Font = FontHelper.UIFont;
			lblListType.Font = FontHelper.UIFont;
			grpPhoneticSortOptions.Font = FontHelper.UIFont;
			grpColSortOptions.Font = FontHelper.UIFont;
			cboListType.Font = FontHelper.UIFont;
			chkSaveManual.Font = FontHelper.UIFont;
			lblSaveManual.Font = FontHelper.UIFont;

			SortOptionsTypeComboItem item;

			item = new SortOptionsTypeComboItem(cboListType.Items[0].ToString(),
				PaApp.Project.DataCorpusVwSortOptions.Clone());
			cboListType.Items.RemoveAt(0);
			cboListType.Items.Insert(0, item);

			item = new SortOptionsTypeComboItem(cboListType.Items[1].ToString(),
				PaApp.Project.SearchVwSortOptions.Clone());
			cboListType.Items.RemoveAt(1);
			cboListType.Items.Insert(1, item);

			item = new SortOptionsTypeComboItem(cboListType.Items[2].ToString(),
				PaApp.Project.XYChartVwSortOptions.Clone());
			cboListType.Items.RemoveAt(2);
			cboListType.Items.Add(item);

			BuildGrid();
			
			cboListType.SelectedIndex = 0;
			m_sortingGrid.IsDirty = false;

			Shown += OptionsDlg_Shown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void OptionsDlg_Shown(object sender, EventArgs e)
		{
			phoneticSortOptions.LayoutControls();
			Shown -= OptionsDlg_Shown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_sortingGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

			// Create the column for the ascending check box.
			DataGridViewColumn col = SilGrid.CreateCheckBoxColumn("include");
			col.HeaderText = string.Empty;
			col.DividerWidth = 0;
			m_sortingGrid.Columns.Add(col);

			// Create the column for the column name.
			col = SilGrid.CreateTextBoxColumn("column");
			col.HeaderText = Properties.Resources.kstidDefineSortOrderColumnCol;
			col.ReadOnly = true;
			m_sortingGrid.Columns.Add(col);

			// Create the column for the ascending check box.
			col = SilGrid.CreateCheckBoxColumn("direction");
			col.HeaderText = Properties.Resources.kstidDefineSortOrderAscendingCol;
			m_sortingGrid.Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values on the sorting tab.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SaveSortingTabSettings()
		{
			if (!IsSortOrderTabDirty)
				return;

			if (m_prevListType != null)
				m_prevListType.SortOptions = GetSortOptionsFromTab();

			SortOptionsTypeComboItem item = cboListType.Items[0] as SortOptionsTypeComboItem;
			if (item != null)
				PaApp.Project.DataCorpusVwSortOptions = item.SortOptions;

			item = cboListType.Items[1] as SortOptionsTypeComboItem;
			if (item != null)
				PaApp.Project.SearchVwSortOptions = item.SortOptions;

			item = cboListType.Items[2] as SortOptionsTypeComboItem;
			if (item != null)
				PaApp.Project.XYChartVwSortOptions = item.SortOptions;

			PaApp.Project.Save();
			PaApp.MsgMediator.SendMessage("SortingOptionsChanged", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsSortOrderTabDirty
		{
			get
			{
				foreach (object item in cboListType.Items)
				{
					SortOptionsTypeComboItem sotcbi = item as SortOptionsTypeComboItem;
					if (sotcbi != null && sotcbi.IsDirty)
						return true;
				}
				
				return (m_sortingGrid != null && m_sortingGrid.IsDirty);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the sort options for the selected word list type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cboListType_SelectedIndexChanged(object sender, EventArgs e)
		{
			phoneticSortOptions.AdvancedOptionsEnabled = (cboListType.SelectedIndex > 0);
			SortOptionsTypeComboItem item = cboListType.SelectedItem as SortOptionsTypeComboItem;

			if (item == null)
				return;

			// Store the options before loading the new current item's sort options.
			if (m_prevListType != null)
				m_prevListType.SortOptions = GetSortOptionsFromTab();

			m_prevListType = item;
			SortOptions sortOptions = item.SortOptions;
			phoneticSortOptions.SortOptions = sortOptions;
			chkSaveManual.Checked = sortOptions.SaveManuallySetSortOptions;
			List<string> sortFieldNames = LoadListFromSortOptions(sortOptions);

			grpPhoneticSortOptions.Enabled = true;

			// Now look through the list of checked items in the list on the Word List
			// tab to make sure we include those items in our list of potential sort fields.
			foreach (PaFieldInfo fieldInfo in fldSelGridWrdList.CheckedFields)
			{
				if (!sortFieldNames.Contains(fieldInfo.FieldName))
				{
					m_sortingGrid.Rows.Add(new object[] { false, fieldInfo, true });

					// If the field is the phonetic field then disable the phonetic sort options.
					if (fieldInfo.IsPhonetic)
						grpPhoneticSortOptions.Enabled = false;
				}
			}

			if (m_sortingGrid.Rows.Count > 0)
			{
				m_sortingGrid.CurrentCell = m_sortingGrid[0, 0];
				m_grid_RowEnter(null, new DataGridViewCellEventArgs(0, 0));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the selector list of the fields currently in the specified sort options.
		/// </summary>
		/// <param name="sortOptions"></param>
		/// <returns>A list of field names for those fields added to the list.</returns>
		/// ------------------------------------------------------------------------------------
		private List<string> LoadListFromSortOptions(SortOptions sortOptions)
		{
			List<string> sortFieldNames = new List<string>();
			m_sortingGrid.Rows.Clear();

			foreach (SortInformation sinfo in sortOptions.SortInformationList)
			{
				if (sinfo.FieldInfo != null)
				{
					m_sortingGrid.Rows.Add(new object[] {true, sinfo.FieldInfo, sinfo.ascending });
					sortFieldNames.Add(sinfo.FieldInfo.FieldName);
				}
			}

			return sortFieldNames;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private SortOptions GetSortOptionsFromTab()
		{
			SortOptions sortOptions = phoneticSortOptions.SortOptions;
			sortOptions.SaveManuallySetSortOptions = chkSaveManual.Checked;
			sortOptions.SortInformationList.Clear();

			for (int i = m_sortingGrid.Rows.Count - 1; i >= 0; i--)
			{
				DataGridViewRow row = m_sortingGrid.Rows[i];
				if ((bool)row.Cells[0].Value)
				{
					sortOptions.SetPrimarySortField(row.Cells[1].Value as PaFieldInfo,
						false, (bool)row.Cells[2].Value);
				}
			}

			return sortOptions;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void phoneticSortOptions_SortOptionsChanged(SortOptions sortOptions)
		{
			m_dirty = true;

			SortOptionsTypeComboItem item = cboListType.SelectedItem as SortOptionsTypeComboItem;

			if (item != null)
				item.IsDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void chkSaveManual_Click(object sender, EventArgs e)
		{
			m_dirty = true;

			SortOptionsTypeComboItem item = cboListType.SelectedItem as SortOptionsTypeComboItem;

			if (item != null)
				item.IsDirty = true;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Move a column (i.e. field) up in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoveSortFieldUp_Click(object sender, EventArgs e)
		{
			DataGridViewRow currRow = m_sortingGrid.CurrentRow;
			if (currRow != null)
			{
				int i = currRow.Index;
				m_sortingGrid.Rows.Remove(currRow);
				m_sortingGrid.Rows.Insert(i - 1, currRow);
				m_sortingGrid.CurrentCell = m_sortingGrid[0, i - 1];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Move a column (i.e. field) down in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoveSortFieldDown_Click(object sender, EventArgs e)
		{
			DataGridViewRow currRow = m_sortingGrid.CurrentRow;
			if (currRow != null)
			{
				int i = currRow.Index;
				m_sortingGrid.Rows.Remove(currRow);
				m_sortingGrid.Rows.Insert(i + 1, currRow);
				m_sortingGrid.CurrentCell = m_sortingGrid[0, i + 1];
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the enabled state of the up and down buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			btnMoveSortFieldUp.Enabled = (e.RowIndex > 0);
			btnMoveSortFieldDown.Enabled = (e.RowIndex > -1 && e.RowIndex < m_sortingGrid.Rows.Count - 1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Only enable the phonetic sort options when the phonetic column is checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_sortingGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			PaFieldInfo fieldInfo = m_sortingGrid[1, e.RowIndex].Value as PaFieldInfo;

			if (fieldInfo != null && fieldInfo.IsPhonetic)
			{
				m_sortingGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
				grpPhoneticSortOptions.Enabled = (bool)m_sortingGrid[0, e.RowIndex].Value;
			}
		}

		#region SortOptionsTypeComboItem class
		/// ----------------------------------------------------------------------------------------
		/// <summary>
		/// Encapsulates a single item in the list type combo box on the sorting options tab page.
		/// </summary>
		/// ----------------------------------------------------------------------------------------
		private class SortOptionsTypeComboItem
		{
			private readonly string m_text;
			private SortOptions m_sortOptions;
			private bool m_isDirty = false;

			/// ------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// ------------------------------------------------------------------------------------
			internal SortOptionsTypeComboItem(string text, SortOptions sortOptions)
			{
				m_text = text;
				m_sortOptions = sortOptions;
			}

			/// ------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// ------------------------------------------------------------------------------------
			internal SortOptions SortOptions
			{
				get { return m_sortOptions; }
				set { m_sortOptions = value; }
			}

			/// --------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// --------------------------------------------------------------------------------
			public bool IsDirty
			{
				get { return m_isDirty; }
				set { m_isDirty = value; }
			}

			/// ------------------------------------------------------------------------------------
			/// <summary>
			/// Returns the text 
			/// </summary>
			/// ------------------------------------------------------------------------------------
			public override string ToString()
			{
				return m_text;
			}
		}

		#endregion
	}
}