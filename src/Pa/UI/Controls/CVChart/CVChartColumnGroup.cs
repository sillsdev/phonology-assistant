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
// File: CVChartColumnGroup.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Drawing;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Manages painting two DataGridView column headings so they look like a single
	/// heading. Part of that involves drawing the group's heading text.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVChartColumnGroup
	{
		private readonly CVChartGrid m_grid;
		private readonly string m_headerText;

		public DataGridViewTextBoxColumn LeftColumn { get; private set; }
		public DataGridViewTextBoxColumn RightColumn { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static CVChartColumnGroup Create(string headerText, CVChartGrid grid)
		{
			return new CVChartColumnGroup(headerText, grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CVChartColumnGroup(string headerText, CVChartGrid grid)
		{
			m_grid = grid;
			m_headerText = headerText;

			LeftColumn = new DataGridViewTextBoxColumn();
			LeftColumn.Name = headerText + "(A)";
			LeftColumn.HeaderText = headerText;
			LeftColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			LeftColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			LeftColumn.ReadOnly = true;
			int i = grid.Columns.Add(LeftColumn);
			LeftColumn = grid.Columns[i] as DataGridViewTextBoxColumn;

			RightColumn = new DataGridViewTextBoxColumn();
			RightColumn.Name = headerText + "(B)";
			RightColumn.HeaderText = headerText;
			RightColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			RightColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			RightColumn.ReadOnly = true;
			i = grid.Columns.Add(RightColumn);
			RightColumn = grid.Columns[i] as DataGridViewTextBoxColumn;

			grid.CellPainting += HandleCellPainting;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the entire group's rectangle, regardless of whether or not some or all of 
		/// it is currently not displayed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Rectangle GroupRectangle
		{
			get
			{
				var rc = m_grid.GetCellDisplayRectangle(LeftColumn.Index, -1, false);

				var dx = rc.X;

				if (rc != Rectangle.Empty)
				{
					if (rc.Width < LeftColumn.Width)
						dx = m_grid.RowHeadersWidth - (LeftColumn.Width - rc.Width);
				}
				else
				{
					rc = m_grid.GetCellDisplayRectangle(RightColumn.Index, -1, false);
					dx = (rc.Width == RightColumn.Width ?
						rc.X : m_grid.RowHeadersWidth - (RightColumn.Width - rc.Width));
					dx -= LeftColumn.Width;
				}
				
				return (m_grid == null ? Rectangle.Empty :
					new Rectangle(dx, 0, LeftColumn.Width + RightColumn.Width, m_grid.ColumnHeadersHeight));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Color CurrentGroupColor
		{
			get
			{
				int colIndex = m_grid.CurrentCellAddress.X;
				return (colIndex == LeftColumn.Index || colIndex == RightColumn.Index ?
					ColorHelper.LightLightHighlight : m_grid.BackgroundColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex < LeftColumn.Index || e.ColumnIndex > RightColumn.Index)
				return;

			if (e.RowIndex >= 0)
			{
				if (e.ColumnIndex == LeftColumn.Index)
					PaintPhoneCell(e);
				
				return;
			}

			var rcLeftColHdr = m_grid.GetCellDisplayRectangle(LeftColumn.Index, -1, false);
			if (e.ColumnIndex == LeftColumn.Index || rcLeftColHdr == Rectangle.Empty)
			{
				var rc = GroupRectangle;

				using (var br = new SolidBrush(CurrentGroupColor))
					e.Graphics.FillRectangle(br, rc);

				DrawDoubleLine(e, rc);

				rc.Height -= 4;

				const TextFormatFlags flags = TextFormatFlags.Bottom |
					TextFormatFlags.HorizontalCenter | TextFormatFlags.HidePrefix |
					TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis |
					TextFormatFlags.PreserveGraphicsClipping;

				TextRenderer.DrawText(e.Graphics, m_headerText, FontHelper.UIFont, rc,
					SystemColors.WindowText, flags);

				using (var pen = new Pen(m_grid.GridColor))
					e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom);
			}

			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void PaintPhoneCell(DataGridViewCellPaintingEventArgs e)
		{
			e.Paint(e.ClipBounds, e.PaintParts);
			Rectangle rc = e.CellBounds;

			using (var pen = new Pen(m_grid.BackgroundColor))
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom - 2);

			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawDoubleLine(DataGridViewCellPaintingEventArgs e, Rectangle rc)
		{
			using (var pen = new Pen(m_grid.GridColor))
			{
				e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
				e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 3, rc.Right, rc.Bottom - 3);
			}
		}
	}
}
