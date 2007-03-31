using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Controls
{
	public class FieldSelector : CheckedListBox
	{
		private const int m_selAllItemIndex = 0;
		private bool m_ignoreItemCheck = false;
		private int m_phoneticItemIndex = -1;
		private bool m_isDirty;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load(bool includeHiddenFields, bool forGrid)
		{
			Font = FontHelper.UIFont;
			IntegralHeight = false;
			FormattingEnabled = false;

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

			m_ignoreItemCheck = true;
			Items.Clear();
			SortedList<int, PaFieldInfo> sortedFieldList = new SortedList<int, PaFieldInfo>();

			// Build a sorted list based on display index.
			foreach (PaFieldInfo fieldInfo in PaApp.FieldInfo)
			{
				if ((!forGrid && fieldInfo.DisplayIndexInRecView >= 0) ||
					(forGrid && fieldInfo.DisplayIndexInGrid >= 0))
				{
					int key = -1;
					if (!forGrid)
						key = fieldInfo.DisplayIndexInRecView;
					else if (includeHiddenFields || fieldInfo.VisibleInGrid)
						key = fieldInfo.DisplayIndexInGrid;

					if (key != -1)
						sortedFieldList[key] = fieldInfo;
				}
			}

			// Build the check box's list from the sorted list.
			foreach (PaFieldInfo fieldInfo in sortedFieldList.Values)
			{
				if (forGrid)
				{
					Items.Add(fieldInfo, (fieldInfo.VisibleInGrid ?
						CheckState.Checked : CheckState.Unchecked));
				}
				else
				{
					Items.Add(fieldInfo, (fieldInfo.VisibleInRecView ?
						CheckState.Checked : CheckState.Unchecked));
				}

				if (fieldInfo.IsPhonetic)
					m_phoneticItemIndex = Items.Count - 1;
			}

			// Add the select all item.
			Items.Insert(m_selAllItemIndex, Properties.Resources.kstidGridColumnSelectorAllItem);

			if (initialCheckedList != null)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					PaFieldInfo fieldInfo = Items[i] as PaFieldInfo;
					if (fieldInfo != null)
						SetItemChecked(i, initialCheckedList.Contains(fieldInfo.FieldName));
				}
			}

			// Determine what the value of the select all item should be.
			CheckState newSelAllValue = CheckState.Indeterminate;

			if (AreAllItemsChecked)
				newSelAllValue = CheckState.Checked;
			else if (CheckedItemCount == 0)
				newSelAllValue = CheckState.Unchecked;

			// If the value of the select all item isn't what it should be, then change it.
			if (newSelAllValue != GetItemCheckState(m_selAllItemIndex))
			{
				SetItemCheckState(m_selAllItemIndex, newSelAllValue);
				Invalidate();
			}

			m_ignoreItemCheck = false;
			SelectedIndex = 0;
			m_isDirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the checked values and order in the PA field objects. This should not be
		/// called when the FieldSelector is being used for things like choosing columns in
		/// which to find query.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(bool forGrid)
		{
			int displayIndex = 0;

			for (int i = 0; i < Items.Count; i++)
			{
				bool visible = (GetItemCheckState(i) == CheckState.Checked);
				PaFieldInfo fieldInfo = Items[i] as PaFieldInfo;
				if (fieldInfo == null)
					continue;

				if (forGrid)
				{
					fieldInfo.VisibleInGrid = visible;
					fieldInfo.DisplayIndexInGrid = displayIndex++;
				}
				else
				{
					fieldInfo.VisibleInRecView = visible;
					fieldInfo.DisplayIndexInRecView = displayIndex++;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the checked values in the list changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsDirty
		{
			get { return m_isDirty; }
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
				for (int i = 0; i < CheckedItems.Count; i++)
				{
					PaFieldInfo fieldInfo = CheckedItems[i] as PaFieldInfo;
					if (fieldInfo != null)
						checkedList.Add(fieldInfo);
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
				int i = PhoneticItemIndex;
				return (i == -1 ? false : GetItemChecked(i));
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
			get
			{
				for (int i = 0; i < Items.Count; i++)
				{
					PaFieldInfo fieldInfo = Items[i] as PaFieldInfo;
					if (fieldInfo != null && fieldInfo.IsPhonetic)
						return i;
				}
				return -1;
			}
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
			// Check if it's greater than one instead of zero
			// since the select all item doesn't count.
			get	{return (CheckedItems.Count > 1);}
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
			get {return (CheckedItemCount == Items.Count - 1);}
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
			get	{return CheckedItems.Count - (GetItemChecked(m_selAllItemIndex) ? 1 : 0);}
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
			get { return (SelectedIndex != m_selAllItemIndex && SelectedIndex > 0 &&
				SelectedIndex > m_selAllItemIndex + 1); }
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
			get { return (SelectedIndex != m_selAllItemIndex && SelectedIndex < Items.Count - 1); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selects all the items in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CheckAll()
		{
			m_ignoreItemCheck = true;

			for (int i = 0; i < Items.Count; i++)
				SetItemChecked(i, true);

			m_ignoreItemCheck = false;
			m_isDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Unselects all the items in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UncheckAll()
		{
			m_ignoreItemCheck = true;

			for (int i = 0; i < Items.Count; i++)
				SetItemChecked(i, false);

			m_ignoreItemCheck = false;
			m_isDirty = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle item's check state changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			base.OnItemCheck(e);

			if (m_ignoreItemCheck)
				return;

			if (e.Index == m_selAllItemIndex)
			{
				if (e.CurrentValue == CheckState.Checked)
					UncheckAll();
				else
					CheckAll();

				return;
			}

			// Determine what the value of the select all item should be.
			CheckState newSelAllValue = CheckState.Indeterminate;

			if (CheckedItemCount == Items.Count - 2 && e.NewValue == CheckState.Checked)
				newSelAllValue = CheckState.Checked;
			else if (CheckedItemCount == 1 && e.NewValue == CheckState.Unchecked)
				newSelAllValue = CheckState.Unchecked;

			// If the value of the select all item isn't what it should be, then change it.
			if (newSelAllValue != GetItemCheckState(m_selAllItemIndex))
			{
				m_ignoreItemCheck = true;
				SetItemCheckState(m_selAllItemIndex, newSelAllValue);
				m_ignoreItemCheck = false;
			}

			m_isDirty = true;
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
				int index = SelectedIndex;
				CheckState state = GetItemCheckState(index);
				Items.Insert(index - 1, Items[index]);
				Items.RemoveAt(index + 1);
				index--;
				SetItemCheckState(index, state);
				SelectedIndex = index;
				m_isDirty = true;
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
				int index = SelectedIndex;
				CheckState state = GetItemCheckState(index);
				Items.Insert(index + 2, Items[index]);
				Items.RemoveAt(index);
				index++;
				SetItemCheckState(index, state);
				SelectedIndex = index;
				m_isDirty = true;
			}

			return CanMoveSelectedItemDown;
		}
	}
}
