using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public class FieldSelectorGrid : SilGrid
	{
		/// <summary>Handler for the AfterUserChangedValue event</summary>
		public delegate void AfterUserChangedValueHandler(PaField field,
			bool selectAllValueChanged, bool newValue);

		/// <summary>
		/// This event is fired when the user has changed one of the check box values in the
		/// list. This event is not fired when a check box's value is changed programmatically.
		/// </summary>
		public event AfterUserChangedValueHandler AfterUserChangedValue;

		private const int kIndeterminate = -1;
		private const string kCheckCol = "check";
		private const string kOrderCol = "order";
		private const string kFieldCol = "field";

		private DataGridViewRow m_phoneticRow;
		private int m_currRowIndex = -1;
		private bool m_ignoreRowEnter;
		private IEnumerable<PaField> m_fieldList;

		/// ------------------------------------------------------------------------------------
		public FieldSelectorGrid()
		{
			RowHeadersVisible = false;
			ColumnHeadersVisible = false;
			AllowUserToOrderColumns = false;
			AllowUserToResizeColumns = false;
			CellBorderStyle = DataGridViewCellBorderStyle.None;
			App.SetGridSelectionColors(this, false);

			if (App.DesignMode)
				return;

			// Add the column for the check box.
			DataGridViewColumn col = CreateCheckBoxColumn(kCheckCol);
			Columns.Add(col);

			// Add the column for the field name.
			col = CreateTextBoxColumn(kFieldCol);
			col.ReadOnly = true;
			col.CellTemplate.Style.Font = FontHelper.UIFont;
			Columns.Add(col);

			// Add a column for a value on which to sort. This column is not visible.
			col = CreateTextBoxColumn(kOrderCol);
			col.ReadOnly = true;
			col.Visible = false;
			col.ValueType = typeof(int);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			Columns.Add(col);

			App.SetGridSelectionColors(this, false);
		}

		/// ------------------------------------------------------------------------------------
		public void Load(IEnumerable<KeyValuePair<PaField, bool>> fieldsInList)
		{
			Rows.Clear();

			var order = 0;

			foreach (var rowData in fieldsInList)
			{
				int i = Rows.Add(new object[] { rowData.Value, rowData.Key.DisplayName, order++ } );
				if (rowData.Key.Type == FieldType.Phonetic)
					m_phoneticRow = Rows[i];
			}

			m_fieldList = fieldsInList.Select(kvp => kvp.Key);

			// Add the select all item, make it a tri-state cell and
			// set its order so it always sorts to the top of the list.
			Rows.Insert(0, new object[] { false,
				App.GetString("FieldChooserGridSelectAllText", "Select All"), -100 });

			((DataGridViewCheckBoxCell)Rows[0].Cells[kCheckCol]).ThreeState = true;
			((DataGridViewCheckBoxCell)Rows[0].Cells[kCheckCol]).IndeterminateValue = kIndeterminate;
			((DataGridViewCheckBoxCell)Rows[0].Cells[kCheckCol]).TrueValue = true;
			((DataGridViewCheckBoxCell)Rows[0].Cells[kCheckCol]).FalseValue = false;

			SetSelectAllItemsValue();
			AutoResizeColumns();
			AutoResizeRows();
			CurrentCell = this[0, 0];
			IsDirty = false;
			OnResize(null);
		}

		/// ------------------------------------------------------------------------------------
		private void SetSelectAllItemsValue()
		{
			if (AreAllItemsChecked)
				Rows[0].Cells[kCheckCol].Value = true;
			else if (AnyItemsChecked)
				Rows[0].Cells[kCheckCol].Value = kIndeterminate;
			else
				Rows[0].Cells[kCheckCol].Value = false;

			InvalidateCell(0, 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Eat the double-click because it allows the state of the "Select All" to get out
		/// of sync. with the rest of the items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellContentDoubleClick(DataGridViewCellEventArgs e)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the user clicking on check box. This will make sure the state of the select
		/// all item is correct and that, if the select all item is the one clicked on, the
		/// state of the rest of the items is correct.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellContentClick(DataGridViewCellEventArgs e)
		{
			base.OnCellContentClick(e);

			if (e.ColumnIndex != 0)
			{
				// Don't allow the field name cell to become current.
				CurrentCell = this[0, e.RowIndex];
				return;
			}

			// Force commital of the click's change.
			CommitEdit(DataGridViewDataErrorContexts.Commit);

			// When the new value of the Select All item is indeterminate, force it to
			// false since the user clicking on it should never make it indeterminate.
			if (e.RowIndex == 0 && Rows[0].Cells[kCheckCol].Value is int)
			{
				Rows[0].Cells[kCheckCol].Value = false;

				// Do this because I can't get the indeterminate value to go away
				// without changing the current cell. So change it and restore it.
				CurrentCell = this[1, e.RowIndex];
				CurrentCell = this[0, e.RowIndex];
			}

			if (e.RowIndex > 0)
				SetSelectAllItemsValue();
			else
			{
				if ((bool)Rows[0].Cells[kCheckCol].Value)
					CheckAll();
				else
					UncheckAll();
			}

			if (AfterUserChangedValue != null)
			{
				var fieldDisplayName = this[kFieldCol, e.RowIndex].Value as string;
				AfterUserChangedValue(m_fieldList.Single(f => f.DisplayName == fieldDisplayName),
					e.RowIndex == 0, (bool)Rows[e.RowIndex].Cells[0].Value);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			if (e != null)
				base.OnResize(e);

			if (Columns[kFieldCol] == null)
				return;

			// Adjust the field name's column width so it extends all the way to the grid'
			// right edge. Account for the vertical scroll bar if it's showing.
			Columns[kFieldCol].Width = (ClientSize.Width - Columns[kCheckCol].Width -
				(DisplayedRowCount(false) < Rows.Count ?
				SystemInformation.VerticalScrollBarWidth : 0) - 5);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the checked fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<PaField> GetCheckedFields()
		{
			return from row in GetRows()
				   where row.Index > 0 && (bool)row.Cells[kCheckCol].Value
				   select m_fieldList.Single(f => f.DisplayName == row.Cells[kFieldCol].Value as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a collection of field names with their corresponding check value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, bool>> GetSelections()
		{
			return from row in GetRows()
				   where row.Index > 0
				   select new KeyValuePair<string, bool>(row.Cells[kFieldCol].Value as string,
						(bool)row.Cells[kCheckCol].Value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the phonetic column is selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsPhoneticChecked
		{
			get
			{
				if (m_phoneticRow != null)
					return (bool)m_phoneticRow.Cells[kCheckCol].Value;

				var field = m_fieldList.SingleOrDefault(f => f.Type == FieldType.Phonetic);
				if (field == null)
					return false;

				foreach (var row in GetRows().Where(r => r.Cells[kFieldCol].Value as string == field.DisplayName))
				{
					m_phoneticRow = row;
					return (bool)row.Cells[kCheckCol].Value;
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the index in the checked list box of the phonetic item. If it's not in the
		/// list then -1 is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PhoneticItemIndex
		{
			get	{return (m_phoneticRow != null ? m_phoneticRow.Index : -1);}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not any items are checked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AnyItemsChecked
		{
			get { return (CheckedItemCount > 0); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not all the items are checked. The "Select All"
		/// item is not counted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AreAllItemsChecked
		{
			get { return (CheckedItemCount == RowCount - 1); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating how many items are checked. The "Select All" item is not
		/// included in the count.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CheckedItemCount
		{
			get { return GetCheckedFields().Count(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the selected item can be moved up.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanMoveSelectedItemUp
		{
			get { return (m_currRowIndex > 1); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the selected item can be moved down.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanMoveSelectedItemDown
		{
			get { return (m_currRowIndex > 0 && m_currRowIndex < Rows.Count - 1); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selects all the items in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CheckAll()
		{
			foreach (DataGridViewRow row in Rows)
				row.Cells[kCheckCol].Value = true;

			InvalidateColumn(0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Unselects all the items in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UncheckAll()
		{
			foreach (DataGridViewRow row in Rows)
				row.Cells[kCheckCol].Value = false;

			InvalidateColumn(0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnRowEnter(DataGridViewCellEventArgs e)
		{
			m_currRowIndex = e.RowIndex;

			if (!m_ignoreRowEnter)
				base.OnRowEnter(e);
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the selected item in the list up by one.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MoveSelectedItemUp()
		{
			// Selected item must be after the "Select All" item and
			// after the item right after it.
			if (CanMoveSelectedItemUp)
			{
				int index = CurrentCellAddress.Y;
				int order = (int)Rows[index].Cells[kOrderCol].Value;
				Rows[index].Cells[kOrderCol].Value = Rows[index - 1].Cells[kOrderCol].Value;
				Rows[index - 1].Cells[kOrderCol].Value = order;
				m_ignoreRowEnter = true;
				Sort(Columns[kOrderCol], ListSortDirection.Ascending);
				m_ignoreRowEnter = false;
				CurrentCell = this[0, index - 1];
				IsDirty = true;

				try
				{
					if (!CurrentRow.Displayed || CurrentRow.Index == 1)
						FirstDisplayedScrollingRowIndex = (CurrentRow.Index == 1 ? 0 : CurrentRow.Index);
				}
				catch { }
			}

			return CanMoveSelectedItemUp;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the selected item in the list down by one.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MoveSelectedItemDown()
		{
			// Selected item must be after the "Select All" item and
			// after the item right after it.
			if (CanMoveSelectedItemDown)
			{
				int index = CurrentCellAddress.Y;
				int order = (int)Rows[index].Cells[kOrderCol].Value;
				Rows[index].Cells[kOrderCol].Value = Rows[index + 1].Cells[kOrderCol].Value;
				Rows[index + 1].Cells[kOrderCol].Value = order;
				m_ignoreRowEnter = true;
				Sort(Columns[kOrderCol], ListSortDirection.Ascending);
				m_ignoreRowEnter = false;
				CurrentCell = this[0, index + 1];
				IsDirty = true;

				try
				{
					while (!CurrentRow.Displayed)
						FirstDisplayedScrollingRowIndex++;

					// Because the Displayed property for the row is true even when only part
					// of the row is visible, we need to make sure all of the row is visible.
					Rectangle rc1 = GetRowDisplayRectangle(CurrentRow.Index, true);
					Rectangle rc2 = GetRowDisplayRectangle(CurrentRow.Index, false);
					if (rc1 != rc2)
						FirstDisplayedScrollingRowIndex++;
				}
				catch { }
			}

			return CanMoveSelectedItemDown;
		}
	}
}
