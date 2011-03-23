using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This control is designed as a drop-down list of check boxes from which the user may
	/// choose columns in a grid to display or hide.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class GridColumnVisibilitySetter : Panel
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="grid"></param>
		/// ------------------------------------------------------------------------------------
		public GridColumnVisibilitySetter(DataGridView grid)
		{
			if (DesignMode || grid == null || grid.Columns.Count == 0)
				return;

			base.AutoScroll = true;
			base.BackColor = SystemColors.Menu;
			Padding = new Padding(10, 10, 10, 6);

			int height = 0;
			int maxWidth = 0;

			// Save the columns sorted by their display order.
			SortedList<int, DataGridViewColumn> colList =
				new SortedList<int, DataGridViewColumn>(grid.Columns.Count - 1);
	
			foreach (DataGridViewColumn col in grid.Columns)
			{
				var field = App.Project.GetFieldForName(col.Name);
				
				// Phonetic column cannot be hidden so don't include it in the list.
				if (field.Type != FieldType.Phonetic)
					colList[col.DisplayIndex] = col;
			}

			foreach (var col in colList)
			{
				CheckBox chkbox = new CheckBox();
				chkbox.Text = col.Value.HeaderText;
				chkbox.Font = FontHelper.UIFont;
				chkbox.Dock = DockStyle.Top;
				chkbox.Checked = col.Value.Visible;
				chkbox.AutoEllipsis = true;
				chkbox.Tag = col.Value;
				chkbox.CheckedChanged += HandleCheckedChanged;

				height += chkbox.Height;
				if (chkbox.PreferredSize.Width > maxWidth)
					maxWidth = chkbox.PreferredSize.Width;

				Controls.Add(chkbox);
				chkbox.BringToFront();
			}

			Size = new Size(maxWidth + 20, height);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default size of the drop-down, used when the size hasn't yet been saved
		/// in the settings file (i.e. as a child control of a SizableDropDownPanel).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Size DefltSize
		{
			get
			{
				// The default height will allow for 12 check boxes.
				int defHeight = (Height > Controls[0].Height * 12 ? Controls[0].Height * 12 : Height);

				// Determine whether or not the default width should allow for a scroll bar.
				int defWidth = (Height > defHeight ?
					Width + SystemInformation.VerticalScrollBarWidth : Width);

				return new Size(defWidth, defHeight);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show or hide a column based on the state of a checked item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleCheckedChanged(object sender, EventArgs e)
		{
			CheckBox chkbox = sender as CheckBox;
			if (chkbox == null)
				return;

			DataGridViewColumn col = chkbox.Tag as DataGridViewColumn;
			if (col == null)
				return;

			col.Visible = chkbox.Checked;
		}
	}
}
