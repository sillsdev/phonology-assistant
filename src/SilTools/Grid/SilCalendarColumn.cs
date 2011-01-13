using System;
using System.Drawing;
using System.Windows.Forms;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	public class SilCalendarColumn : DataGridViewColumn
	{
		/// ------------------------------------------------------------------------------------
		public SilCalendarColumn() : base(new SilCalendarCell())
		{
			base.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			base.DefaultCellStyle.BackColor = SystemColors.Window;
			base.DefaultCellStyle.Font = SystemFonts.MenuFont;
			base.CellTemplate.Style = DefaultCellStyle;
		}

		/// ------------------------------------------------------------------------------------
		public override DataGridViewCell CellTemplate
		{
			get { return base.CellTemplate; }
			set
			{
				// Ensure that the cell used for the template is a CalendarCell.
				if (value != null && !value.GetType().IsAssignableFrom(typeof(SilCalendarCell)))
					throw new InvalidCastException("Cell template must be an SilCalendarCell");

				base.CellTemplate = value;
			}
		}
	}
}

