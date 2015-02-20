// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
//  
// File: CVChartRowGroup.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Manages painting one or more DataGridView row headings to they look like a single
	/// heading. Part of that involves drawing the group's heading text.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CVChartRowGroup : IDisposable
	{
		private const TextFormatFlags kHdrTextRenderingFlags =
			TextFormatFlags.VerticalCenter | TextFormatFlags.Left |
			TextFormatFlags.HidePrefix | TextFormatFlags.WordBreak |
			TextFormatFlags.WordEllipsis | TextFormatFlags.PreserveGraphicsClipping;

		private readonly CVChartGrid m_grid;
		private readonly int m_firstRowIndex;
		private readonly int m_lastRowIndex;
		private readonly string _textsLocalizationId;

		public List<DataGridViewRow> Rows { get; private set; }
		public string Text { get; private set; }

		/// ------------------------------------------------------------------------------------
		public static CVChartRowGroup Create(string headerText, int rowCount, CVChartGrid grid,
			string headerTextsLocalizationId)
		{
			return new CVChartRowGroup(headerText, rowCount, grid, headerTextsLocalizationId);
		}

		/// ------------------------------------------------------------------------------------
		private CVChartRowGroup(string headerText, int rowCount, CVChartGrid grid,
			string headerTextsLocalizationId)
		{
			m_grid = grid;
			Text = headerText;
			_textsLocalizationId = headerTextsLocalizationId;

			m_lastRowIndex = grid.Rows.Add(rowCount);
			m_firstRowIndex = m_lastRowIndex - rowCount + 1;

			Rows = (from x in grid.Rows.Cast<DataGridViewRow>()
					where x.Index >= m_firstRowIndex && x.Index <= m_lastRowIndex
					select x).ToList();

			m_grid.CellPainting += HandleCellPainting;
			m_grid.CellMouseClick += HandleGridCellMouseClick;
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
		public int PreferredWidth
		{
			get
			{
				try
				{
					// Adding 4 accounts for the double line along the group's right edge.
					using (var g = m_grid.CreateGraphics())
						return TextRenderer.MeasureText(g, Text, FontHelper.UIFont).Width + 4;
				}
				catch
				{
					return TextRenderer.MeasureText(Text, FontHelper.UIFont).Width + 4;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the entire group's rectangle, regardless of whether or not some or all of 
		/// it is currently not displayed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Rectangle GetGroupRectangle()
		{
			// Don't call GetCellDisplayRectangle(-1,...) below; see
			// http://bugzilla.xamarin.com/show_bug.cgi?id=341
			int top = m_grid.ColumnHeadersHeight - m_grid.VerticalScrollingOffset;
			for (int i = 0; i < m_firstRowIndex; i++)
				top += m_grid.Rows[i].Height;

			return (m_grid == null ? Rectangle.Empty :
				new Rectangle(0, top, m_grid.RowHeadersWidth, Rows.Sum(x => x.Height)));
		}

		/// ------------------------------------------------------------------------------------
		private Color CurrentGroupColor
		{
			get
			{
				int rowIndex = m_grid.CurrentCellAddress.Y;
				return (rowIndex >= m_firstRowIndex && rowIndex <= m_lastRowIndex ?
					ColorHelper.LightLightHighlight : m_grid.BackgroundColor);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex < 0 && Control.ModifierKeys == (Keys.Control | Keys.Shift) &&
				e.RowIndex >= m_firstRowIndex && e.RowIndex <= m_lastRowIndex)
			{
				LocalizationManager.ShowLocalizationDialogBox(_textsLocalizationId);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex >= 0 || e.RowIndex < m_firstRowIndex || e.RowIndex > m_lastRowIndex)
				return;

			e.Graphics.SetClip(new Rectangle(0, m_grid.ColumnHeadersHeight,
				m_grid.RowHeadersWidth, m_grid.ClientRectangle.Height));

			var rc = GetGroupRectangle();

			using (var br = new SolidBrush(m_grid.BackgroundColor))
				e.Graphics.FillRectangle(br, rc);
			
			DrawDoubleLine(e, rc);

			rc.Width -= 3;

			using (var br = new SolidBrush(CurrentGroupColor))
				e.Graphics.FillRectangle(br, rc);

			rc.Width--;

			TextRenderer.DrawText(e.Graphics, Text, FontHelper.UIFont, rc,
				SystemColors.WindowText, kHdrTextRenderingFlags);

			using (var pen = new Pen(m_grid.GridColor))
				e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 1, rc.Right, rc.Bottom - 1);

			e.Handled = true;
			e.Graphics.ResetClip();
		}

		/// ------------------------------------------------------------------------------------
		private void DrawDoubleLine(DataGridViewCellPaintingEventArgs e, Rectangle rc)
		{
			using (var pen = new Pen(m_grid.GridColor))
			{
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom - 1);
				e.Graphics.DrawLine(pen, rc.Right - 3, rc.Y, rc.Right - 3, rc.Bottom - 1);
			}
		}
	}
}
