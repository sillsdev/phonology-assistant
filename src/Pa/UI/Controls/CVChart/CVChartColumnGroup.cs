using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Manages painting two DataGridView column headings so they look like a single
	/// heading. Part of that involves drawing the group's heading text.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVChartColumnGroup : IDisposable
	{
		private const TextFormatFlags kHdrTextRenderingFlags =
			TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter |
			TextFormatFlags.HidePrefix | TextFormatFlags.WordBreak |
			TextFormatFlags.WordEllipsis | TextFormatFlags.PreserveGraphicsClipping;
		
		private readonly CVChartGrid m_grid;
		private readonly string m_headerText;
		private readonly string _textsLocalizationId;

		public DataGridViewTextBoxColumn LeftColumn { get; private set; }
		public DataGridViewTextBoxColumn RightColumn { get; private set; }

		/// ------------------------------------------------------------------------------------
		public static CVChartColumnGroup Create(string headerText, CVChartGrid grid,
			string headerTextsLocalizationId)
		{
			return new CVChartColumnGroup(headerText, grid, headerTextsLocalizationId);
		}

		/// ------------------------------------------------------------------------------------
		private CVChartColumnGroup(string headerText, CVChartGrid grid,
			string headerTextsLocalizationId)
		{
			m_grid = grid;
			m_headerText = headerText;
			_textsLocalizationId = headerTextsLocalizationId;

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
			grid.CellMouseClick += HandleGridCellMouseClick;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (m_grid != null)
			{
				m_grid.CellPainting -= HandleCellPainting;
				m_grid.CellMouseClick -= HandleGridCellMouseClick;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string Text
		{
			get { return LeftColumn.HeaderText; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the entire group's rectangle, regardless of whether or not some or all of 
		/// it is currently not displayed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Rectangle GetGroupRectangle()
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

		/// ------------------------------------------------------------------------------------
		public int GetPreferredHeaderHeight()
		{
			if (m_grid == null || !m_grid.IsHandleCreated || m_grid.IsDisposed)
				return 0;

			var sz = new Size(LeftColumn.Width + RightColumn.Width, 0);

			try
			{
				// Someone running Chinese Windows was having a problem with accessing
				// the released object (presumably the graphics object). I'm not sure
				// what that was all about or how it could happen, but that's why the
				// try catch.
				using (var g = m_grid.CreateGraphics())
				{
					return TextRenderer.MeasureText(g, Text, FontHelper.UIFont,
						sz, kHdrTextRenderingFlags).Height + 5;
				}
			}
			catch
			{
			    return TextRenderer.MeasureText(Text, FontHelper.UIFont,
			        sz, kHdrTextRenderingFlags).Height + 5;
			}
		}

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
		private void HandleGridCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex < 0 && Control.ModifierKeys == (Keys.Control | Keys.Shift) &&
				(LeftColumn.Index == e.ColumnIndex || RightColumn.Index == e.ColumnIndex))
			{
				LocalizationManager.ShowLocalizationDialogBox(_textsLocalizationId);
			}
		}

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

			// Do the painting only when getting this event for the right column in the group,
			// unless the right column is scrolled out of view, then do the painting when the
			// event is for the left column.
			var rcRightColHdr = m_grid.GetCellDisplayRectangle(RightColumn.Index, -1, false);
			if (e.ColumnIndex == RightColumn.Index || rcRightColHdr == Rectangle.Empty)
			{
				var rc = GetGroupRectangle();

				using (var br = new SolidBrush(m_grid.BackgroundColor))
					e.Graphics.FillRectangle(br, rc);

				DrawDoubleLine(e, rc);

				rc.Height -= 3;
				
				using (var br = new SolidBrush(CurrentGroupColor))
					e.Graphics.FillRectangle(br, rc);

				rc.Height--;

				TextRenderer.DrawText(e.Graphics, m_headerText, FontHelper.UIFont, rc,
					SystemColors.WindowText, kHdrTextRenderingFlags);

				using (var pen = new Pen(m_grid.GridColor))
					e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom);
			}

			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Erase the cell's right border (i.e. the line between the voiced and voiceless
		/// column in consonant charts and between rounded and unrounded in vowel charts).
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
