using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SilTools.Controls;

namespace SilTools
{
	public class CellCustomDropDownList : SilPanel
	{
		protected DataGridViewCell m_cell;
		protected readonly ListBox m_listBox;
		protected readonly CustomDropDown m_dropDown;

		/// ------------------------------------------------------------------------------------
		public CellCustomDropDownList()
		{
			DoubleBuffered = true;
			m_dropDown = new CustomDropDown();
			m_dropDown.AutoCloseWhenMouseLeaves = false;
			m_dropDown.AddControl(this);
			m_dropDown.Closed += delegate { m_cell = null; };

			m_listBox = new ListBox();
			m_listBox.BorderStyle = BorderStyle.None;
			m_listBox.Dock = DockStyle.Fill;
			m_listBox.KeyDown += HandleListBoxKeyDown;
			m_listBox.MouseClick += HandleListBoxMouseClick;
			m_listBox.MouseMove += HandleListBoxMouseMove;
			
			BorderStyle = BorderStyle.FixedSingle;
			Padding = new Padding(1);
			Controls.Add(m_listBox);

			Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				m_listBox.Font = value;
				base.Font = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public ListBox.ObjectCollection Items
		{
			get { return m_listBox.Items; }
		}

		/// ------------------------------------------------------------------------------------
		public object SelectedItem
		{
			get { return m_listBox.SelectedItem; }
			set { m_listBox.SelectedItem = value; }
		}

		/// ------------------------------------------------------------------------------------
		public int SelectedIndex
		{
			get { return m_listBox.SelectedIndex; }
			set { m_listBox.SelectedIndex = value; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsDroppedDown
		{
			get { return m_cell != null; }
		}

		/// ------------------------------------------------------------------------------------
		public void Close()
		{
			m_dropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		public void Show(DataGridViewCell cell, IEnumerable<string> items)
		{
			// This is sort of a kludge, but right before the first time the list is
			// displayed, it's handle hasn't been created therefore the preferred
			// size cannot be accurately determined and the preferred width is needed
			// below. So to ensure the handle gets created, show then hide the drop-down.
			if (!IsHandleCreated)
			{
				Size = new Size(0, 0);
				m_dropDown.Show(0, 0);
				m_dropDown.Close();
			}

			Items.Clear();
			Items.AddRange(items.ToArray());
			SelectedItem = cell.Value as string;

			if (SelectedIndex < 0 && Items.Count > 0)
				SelectedIndex = 0;

			m_cell = cell;
			int col = cell.ColumnIndex;
			int row = cell.RowIndex;
			Width = Math.Max(cell.DataGridView.Columns[col].Width, m_listBox.PreferredSize.Width);
			Height = (Math.Min(Items.Count, 15) * m_listBox.ItemHeight) + Padding.Vertical + 2;
			var rc = cell.DataGridView.GetCellDisplayRectangle(col, row, false);
			rc.Y += rc.Height;
			m_dropDown.Show(cell.DataGridView.PointToScreen(rc.Location));
			m_listBox.Focus();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleListBoxMouseMove(object sender, MouseEventArgs e)
		{
			int i = m_listBox.IndexFromPoint(e.Location);
			if (i >= 0 && i != SelectedIndex)
				SelectedIndex = i;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleListBoxMouseClick(object sender, MouseEventArgs e)
		{
			int i = m_listBox.IndexFromPoint(e.Location);
			if (i >= 0)
				m_cell.Value = Items[i] as string;

			m_dropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleListBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				m_dropDown.Close();
			else if (e.KeyCode == Keys.Return && SelectedItem != null)
			{
				m_cell.Value = SelectedItem as string;
				m_dropDown.Close();
			}
		}
	}
}
