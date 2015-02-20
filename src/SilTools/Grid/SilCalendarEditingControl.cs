// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Windows.Forms;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	internal class SilCalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
	{
		/// ------------------------------------------------------------------------------------
		public SilCalendarEditingControl()
		{
			Format = DateTimePickerFormat.Short;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Notify the DataGridView that the contents of the cell have changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnValueChanged(EventArgs eventargs)
		{
			EditingControlValueChanged = true;
			EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnValueChanged(eventargs);
		}

		#region IDataGridViewEditingControl implementations

		public int EditingControlRowIndex { get; set; }
		public bool EditingControlValueChanged { get; set; }
		public DataGridView EditingControlDataGridView { get; set; }

		/// ------------------------------------------------------------------------------------
		public Cursor EditingPanelCursor
		{
			get { return base.Cursor; }
		}

		/// ------------------------------------------------------------------------------------
		public object EditingControlFormattedValue
		{
			get { return Value.ToShortDateString(); }
			set
			{
				if (value is string && ((string)value).Trim() != string.Empty)
					Value = DateTime.Parse((string)value);
			}
		}

		/// ------------------------------------------------------------------------------------
		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		/// ------------------------------------------------------------------------------------
		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			Font = dataGridViewCellStyle.Font;
			CalendarForeColor = dataGridViewCellStyle.ForeColor;
			CalendarMonthBackground = dataGridViewCellStyle.BackColor;
		}

		/// ------------------------------------------------------------------------------------
		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			// Let the DateTimePicker handle the keys listed.
			switch (key & Keys.KeyCode)
			{
				case Keys.Left:
				case Keys.Up:
				case Keys.Down:
				case Keys.Right:
				case Keys.Home:
				case Keys.End:
				case Keys.PageDown:
				case Keys.PageUp:
					return true;
				default:
					return !dataGridViewWantsInputKey;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void PrepareEditingControlForEdit(bool selectAll)
		{
			// No preparation needs to be done.
		}

		/// ------------------------------------------------------------------------------------
		public bool RepositionEditingControlOnValueChange
		{
			get { return false; }
		}

		#endregion
	}
}
