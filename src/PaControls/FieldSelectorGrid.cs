using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	public class FieldSelectorGrid : SilGrid
	{
		/// <summary>Handler for the AfterUserChangedValue event</summary>
		public delegate void AfterUserChangedValueHandler(PaFieldInfo fieldInfo,
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
		private bool m_ignoreRowEnter = false;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FieldSelectorGrid() : base()
		{
			RowHeadersVisible = false;
			ColumnHeadersVisible = false;
			AllowUserToOrderColumns = false;
			AllowUserToResizeColumns = false;
			CellBorderStyle = DataGridViewCellBorderStyle.None;

			if (PaApp.DesignMode)
				return;

			// Add the column for the check box.
			DataGridViewColumn col = SilGrid.CreateCheckBoxColumn(kCheckCol);
			Columns.Add(col);

			// Add the column for the field name.
			col = SilGrid.CreateTextBoxColumn(kFieldCol);
			col.ReadOnly = true;
			col.CellTemplate.Style.Font = FontHelper.UIFont;
			Columns.Add(col);

			// Add a column for a value on which to sort. This column is not visible.
			col = SilGrid.CreateTextBoxColumn(kOrderCol);
			col.ReadOnly = true;
			col.Visible = false;
			col.ValueType = typeof(int);
			col.SortMode = DataGridViewColumnSortMode.Automatic;
			Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load(bool includeHiddenFields, bool forGrid)
		{
			Load(includeHiddenFields, forGrid, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load(bool includeHiddenFields, bool forGrid, List<string> initialCheckedList)
		{
			if (initialCheckedList != null && initialCheckedList.Count == 0)
				initialCheckedList = null;

			Rows.Clear();

			// Build a sorted list based on display index.
			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
			{
				if ((!forGrid && fieldInfo.DisplayIndexInRecView >= 0) ||
					(forGrid && fieldInfo.DisplayIndexInGrid >= 0))
				{
					int order = -1;
					bool check = false;

					if (!forGrid)
					{
						order = fieldInfo.DisplayIndexInRecView;
						check = fieldInfo.VisibleInRecView;
					}
					else if (includeHiddenFields || fieldInfo.VisibleInGrid)
					{
						order = fieldInfo.DisplayIndexInGrid;
						check = fieldInfo.VisibleInGrid;
					}

					if (order != -1)
					{
						Rows.Add(new object[] { check, fieldInfo.ToString(), order });
						Rows[Rows.Count - 1].Tag = fieldInfo.FieldName;

						if (fieldInfo.IsPhonetic)
							m_phoneticRow = Rows[Rows.Count - 1];
					}
				}
			}

			Sort(Columns[kOrderCol], ListSortDirection.Ascending);

			// If a list was supplied that contains field name's whose initial value should be
			// checked, then go through the grid rows and check items's whose name is in that list.
			if (initialCheckedList != null)
			{
				foreach (DataGridViewRow row in Rows)
				{
					string fieldName = row.Tag as string;
					if (fieldName != null)
						row.Cells[kCheckCol].Value = initialCheckedList.Contains(fieldName);
				}
			}

			// Add the select all item, make it a tri-state cell and
			// set its order so it always sorts to the top of the list.
			Rows.Insert(0, new object[] { false,
				Properties.Resources.kstidGridColumnSelectorAllItem, -100});

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
		/// <summary>
		/// 
		/// </summary>
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
		/// Saves the checked values and order in the PA field objects. This should not be
		/// called when the FieldSelector is being used for things like choosing columns in
		/// which to find a query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(bool forGrid)
		{
			int displayIndex = 0;

			foreach (DataGridViewRow row in Rows)
			{
				string fieldName = row.Tag as string;
				if (fieldName == null)
					continue;

				PaFieldInfo fieldInfo = PaApp.Project.FieldInfo[fieldName];

				if (forGrid)
				{
					fieldInfo.VisibleInGrid = (bool)row.Cells[kCheckCol].Value;
					fieldInfo.DisplayIndexInGrid = displayIndex++;
				}
				else
				{
					fieldInfo.VisibleInRecView = (bool)row.Cells[kCheckCol].Value;
					fieldInfo.DisplayIndexInRecView = displayIndex++;
				}
			}
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

			System.Diagnostics.Debug.WriteLine(MousePosition.ToString() + "   Value: " + Rows[0].Cells[kCheckCol].Value); 



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
				string fieldName = Rows[e.RowIndex].Tag as string;
				AfterUserChangedValue(PaApp.Project.FieldInfo[fieldName],
					e.RowIndex == 0, (bool)Rows[e.RowIndex].Cells[0].Value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the check box cell manually to get rid of the focus rectangle around it.
		/// This will also not paint the selection color around the check box which is the
		/// way the checked list box works.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				e.PaintBackground(e.CellBounds, false);
				e.PaintContent(e.CellBounds);
				e.Handled = true;
			}

			base.OnCellPainting(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
				(this.DisplayedRowCount(false) < Rows.Count ?
				SystemInformation.VerticalScrollBarWidth : 0) - 5);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a collection of the checked fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PaFieldInfoList CheckedFields
		{
			get
			{
				PaFieldInfoList checkedList = new PaFieldInfoList();
				foreach (DataGridViewRow row in Rows)
				{
					string fieldName = row.Tag as string;
					if (fieldName != null && (bool)row.Cells[kCheckCol].Value)
						checkedList.Add(PaApp.Project.FieldInfo[fieldName]);
				}

				return checkedList;
			}
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

				foreach (DataGridViewRow row in Rows)
				{
					string fieldName = row.Tag as string;
					if (fieldName != null && PaApp.Project.FieldInfo[fieldName].IsPhonetic)
					{
						m_phoneticRow = row;
						return (bool)row.Cells[kCheckCol].Value;
					}
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
			get
			{
				foreach (DataGridViewRow row in Rows)
				{
					if (row.Index > 0 && (bool)row.Cells[kCheckCol].Value)
						return true;
				}

				return false;
			}
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
			get
			{
				foreach (DataGridViewRow row in Rows)
				{
					if (row.Index > 0 && !((bool)row.Cells[kCheckCol].Value))
						return false;
				}

				return true;
			}
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
			get
			{
				int count = 0;
				foreach (DataGridViewRow row in Rows)
				{
					if (row.Index > 0 && (bool)row.Cells[kCheckCol].Value)
						count++;
				}

				return count;
			}
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
