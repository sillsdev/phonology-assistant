using System;
using System.Windows.Forms;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	public class SilCalendarCell : DataGridViewTextBoxCell
	{
		/// ------------------------------------------------------------------------------------
		public SilCalendarCell()
		{
			// Use the short date format.
			Style.Format = "d";
		}

		/// ------------------------------------------------------------------------------------
		public override void InitializeEditingControl(int rowIndex, object
			initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			// Set the value of the editing control to the current cell value.
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			var ctrl = DataGridView.EditingControl as SilCalendarEditingControl;

			if (Value != null && Value.GetType() == typeof(DateTime))
				ctrl.Value = (DateTime)Value;
		}

		/// ------------------------------------------------------------------------------------
		public override Type EditType
		{
			get { return typeof(SilCalendarEditingControl); }
		}

		/// ------------------------------------------------------------------------------------
		public override Type ValueType
		{
			get { return typeof(DateTime); }
		}

		/// ------------------------------------------------------------------------------------
		public override object DefaultNewRowValue
		{
			get { return null; }
		}
	}
}
