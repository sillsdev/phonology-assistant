// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: CheckBoxColumnHeader.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class draws a checkbox in a column header and lets the user click/unclick the
	/// check box, firing an event when they do so. IMPORTANT: This class must be instantiated
	/// after the column has been added to a DataGridView control.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CheckBoxColumnHeaderHandler
	{
		public delegate bool CheckChangeHandler(CheckBoxColumnHeaderHandler sender,
			CheckState oldState);

		public event CheckChangeHandler CheckChanged;
		
		private readonly DataGridViewColumn m_col;
		private readonly DataGridView m_grid;
		private readonly Size m_szCheckBox = Size.Empty;
		private CheckState m_state = CheckState.Checked;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CheckBoxColumnHeaderHandler(DataGridViewColumn col)
		{
			Debug.Assert(col != null);
			Debug.Assert(col is DataGridViewCheckBoxColumn);
			Debug.Assert(col.DataGridView != null);

			m_col = col;
			m_grid = col.DataGridView;
			m_grid.HandleDestroyed += HandleHandleDestroyed;
			m_grid.CellPainting += HandleHeaderCellPainting;
			m_grid.CellMouseMove += HandleHeaderCellMouseMove;
			m_grid.CellMouseClick += HandleHeaderCellMouseClick;
			m_grid.CellContentClick += HandleDataCellCellContentClick;
			m_grid.Scroll += HandleGridScroll;
			m_grid.RowsAdded += HandleGridRowsAdded;
			m_grid.RowsRemoved += HandleGridRowsRemoved;
			
			if (!PaintingHelper.CanPaintVisualStyle())
				m_szCheckBox = new Size(13, 13);
			else
			{
				var element = VisualStyleElement.Button.CheckBox.CheckedNormal;
				var renderer = new VisualStyleRenderer(element);
				using (var g = m_grid.CreateGraphics())
					m_szCheckBox = renderer.GetPartSize(g, ThemeSizeType.True);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the state of the column header's check box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CheckState HeadersCheckState
		{
			get { return m_state; }
			set
			{
				m_state = value;
				m_grid.InvalidateCell(m_col.HeaderCell);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleHandleDestroyed(object sender, EventArgs e)
		{
			m_grid.HandleDestroyed -= HandleHandleDestroyed;
			m_grid.CellPainting -= HandleHeaderCellPainting;
			m_grid.CellMouseMove -= HandleHeaderCellMouseMove;
			m_grid.CellMouseClick -= HandleHeaderCellMouseClick;
			m_grid.CellContentClick -= HandleDataCellCellContentClick;
			m_grid.Scroll -= HandleGridScroll;
			m_grid.RowsAdded -= HandleGridRowsAdded;
			m_grid.RowsRemoved -= HandleGridRowsRemoved;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGridRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			UpdateHeadersCheckStateFromColumnsValues();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGridRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			UpdateHeadersCheckStateFromColumnsValues();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleGridScroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
			{
				var rc = m_grid.ClientRectangle;
				rc.Height = m_grid.ColumnHeadersHeight;
				m_grid.Invalidate(rc);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateHeadersCheckStateFromColumnsValues()
		{
			bool foundOneChecked = false;
			bool foundOneUnChecked = false;

			foreach (DataGridViewRow row in m_grid.Rows)
			{
				object cellValue = row.Cells[m_col.Index].Value;
				if (!(cellValue is bool))
					continue;

				bool chked = (bool)cellValue;
				if (!foundOneChecked && chked)
					foundOneChecked = true;
				else if (!foundOneUnChecked && !chked)
					foundOneUnChecked = true;

				if (foundOneChecked && foundOneUnChecked)
				{
					HeadersCheckState = CheckState.Indeterminate;
					return;
				}
			}

			HeadersCheckState = (foundOneChecked ? CheckState.Checked : CheckState.Unchecked);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateColumnsDataValuesFromHeadersCheckState()
		{
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				if (row.Cells[m_col.Index] == m_grid.CurrentCell && m_grid.IsCurrentCellInEditMode)
					m_grid.EndEdit();

				row.Cells[m_col.Index].Value = (m_state == CheckState.Checked);
			}
		}

		#region Mouse move and click handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles toggling the selected state of a file in the file list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleDataCellCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == m_col.Index)
			{
				bool currCellValue = (bool)m_grid[e.ColumnIndex, e.RowIndex].Value;
				m_grid[e.ColumnIndex, e.RowIndex].Value = !currCellValue;
				UpdateHeadersCheckStateFromColumnsValues();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleHeaderCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0 || e.ColumnIndex != m_col.Index)
				return;

			CheckState oldState = HeadersCheckState;

			if (HeadersCheckState == CheckState.Checked)
				HeadersCheckState = CheckState.Unchecked;
			else
				HeadersCheckState = CheckState.Checked;

			m_grid.InvalidateCell(m_col.HeaderCell);

			bool updateValues = true;
			if (CheckChanged != null)
				updateValues = CheckChanged(this, oldState);

			if (updateValues)
				UpdateColumnsDataValuesFromHeadersCheckState();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleHeaderCellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == m_col.Index && e.RowIndex < 0)
				m_grid.InvalidateCell(m_col.HeaderCell);
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleHeaderCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex != m_col.Index)
				return;

			var rcCell = m_grid.GetCellDisplayRectangle(m_col.Index, -1, false);
			if (rcCell.IsEmpty)
				return;

			// At this point, we know at least part of the header cell is visible, therefore,
			// force the rectangle's width to that of the column's.
			rcCell.X = rcCell.Right - m_col.Width;

			// Subtract one so as not to include the left border in the width.
			rcCell.Width = m_col.Width - 1;

			int dx = (int)Math.Floor((rcCell.Width - m_szCheckBox.Width) / 2f);
			int dy = (int)Math.Floor((rcCell.Height - m_szCheckBox.Height) / 2f);
			var rc = new Rectangle(rcCell.X + dx, rcCell.Y + dy, m_szCheckBox.Width, m_szCheckBox.Height);

			if (PaintingHelper.CanPaintVisualStyle())
				DrawVisualStyleCheckBox(e.Graphics, rc);
			else
			{
				var state = ButtonState.Checked;
				if (HeadersCheckState == CheckState.Unchecked)
					state = ButtonState.Normal;
				else if (HeadersCheckState == CheckState.Indeterminate)
					state |= ButtonState.Inactive;

				ControlPaint.DrawCheckBox(e.Graphics, rc, state | ButtonState.Flat);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawVisualStyleCheckBox(IDeviceContext g, Rectangle rc)
		{
			var isHot = rc.Contains(m_grid.PointToClient(Control.MousePosition));
			var element = VisualStyleElement.Button.CheckBox.CheckedNormal;

			if (HeadersCheckState == CheckState.Unchecked)
			{
				element = (isHot ? VisualStyleElement.Button.CheckBox.UncheckedHot :
					VisualStyleElement.Button.CheckBox.UncheckedNormal);
			}
			else if (HeadersCheckState == CheckState.Indeterminate)
			{
				element = (isHot ? VisualStyleElement.Button.CheckBox.MixedHot :
					VisualStyleElement.Button.CheckBox.MixedNormal);
			}
			else if (isHot)
				element = VisualStyleElement.Button.CheckBox.CheckedHot;
			
			var renderer = new VisualStyleRenderer(element);
			renderer.DrawBackground(g, rc);
		}

		#endregion
	}
}
