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
// File: CVChartGrid.cs
// Responsibility: Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public enum CVChartType
	{
		Consonant,
		Vowel
	}

	/// ----------------------------------------------------------------------------------------
	public class CVChartGrid : SilGrid, IxCoreColleague
	{
		private readonly PhoneInfoPopup m_phoneInfoPopup;
		//private DataGridViewCell m_cellShowingPopup;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<CVChartColumnGroup> ColumnGroups { get; private set; }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<CVChartRowGroup> RowGroups { get; private set; }

		/// ------------------------------------------------------------------------------------
		public CVChartGrid()
		{
			Font = FontHelper.MakeRegularFontDerivative(App.PhoneticFont, 14);
			BorderStyle = BorderStyle.None;
			AllowUserToOrderColumns = false;
			AllowUserToResizeColumns = false;
			SelectionMode = DataGridViewSelectionMode.CellSelect;
			PaintHeaderAcrossFullGridWidth = false;

			ColumnGroups = new List<CVChartColumnGroup>();
			RowGroups = new List<CVChartRowGroup>();

			m_phoneInfoPopup = new PhoneInfoPopup(this);
			//m_phoneInfoPopup.PopupOpened += HandlePhoneInfoPopupOpened;
			//m_phoneInfoPopup.PopupClosed += HandlePhoneInfoPopupClosed;
		}

		/// ------------------------------------------------------------------------------------
		public CVChartGrid(ITMAdapter tmAdapter) : this()
		{
			if (tmAdapter != null)
			{
				tmAdapter.SetContextMenuForControl(this, "cmnuCharChartGrid");
				if (ContextMenuStrip != null)
				{
					ContextMenuStrip.Opening += ((sender, args) => m_phoneInfoPopup.Enabled = false);
					ContextMenuStrip.Closed += ((sender, args) => m_phoneInfoPopup.Enabled = true);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (m_phoneInfoPopup != null && !m_phoneInfoPopup.IsDisposed)
					m_phoneInfoPopup.Dispose();

				App.RemoveMediatorColleague(this);
			}
			
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public void ClearAll()
		{
			Columns.Clear();
			Rows.Clear();

			foreach (var grp in ColumnGroups)
				grp.Dispose();

			foreach (var grp in RowGroups)
				grp.Dispose();

			ColumnGroups.Clear();
			RowGroups.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current phone in the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CurrentPhone
		{
			get
			{
				return (CurrentCell == null || CurrentCell.Value == null ?
					null : CurrentCell.Value as string);
			}
		}

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of suprasegmentals to ignore.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SupraSegsToIgnore { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the collection of selected phones phone in the chart.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] SelectedPhones
		{
			get
			{
				var phones = new List<string>();

				if (SelectedCells.Count == 0)
				{
					string currPhone = CurrentPhone;
					if (!string.IsNullOrEmpty(currPhone))
						phones.Add(CurrentPhone);
				}
				else
				{
					phones = (from x in SelectedCells.Cast<DataGridViewCell>()
							  where !string.IsNullOrEmpty((x.Value as string))
							  select x.Value as string).ToList();
				}

				return (phones.Count == 0 ? null : phones.ToArray());
			}
		}
		
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (!App.DesignMode)
				App.AddMediatorColleague(this);
		}

		/// ------------------------------------------------------------------------------------
		public void AddColumnGroup(string text)
		{
			ColumnGroups.Add(CVChartColumnGroup.Create(text, this));
		}

		/// ------------------------------------------------------------------------------------
		public void AddRowGroup(string text, int rowCount)
		{
			var grp = CVChartRowGroup.Create(text, rowCount, this);
			RowHeadersWidth = Math.Max(RowHeadersWidth, grp.PreferredWidth);
			RowGroups.Add(grp);
		}

		/// ------------------------------------------------------------------------------------
		public void AdjustCellSizes()
		{
			int width = 0;
			int height = 0;

			using (var g = CreateGraphics())
			{
				foreach (var row in GetRows())
				{
					foreach (DataGridViewColumn col in Columns)
					{
						var phone = row.Cells[col.Index].Value as string;
						if (phone == null)
							continue;

						var sz = TextRenderer.MeasureText(g, phone,
							Font, Size.Empty, TextFormatFlags.LeftAndRightPadding);
						
						width = Math.Max(width, sz.Width);
						height = Math.Max(height, sz.Height);
					}
				}
			}

			foreach (DataGridViewColumn col in Columns)
				col.Width = width;

			foreach (var row in GetRows())
				row.Height = height;
		}

		/// ------------------------------------------------------------------------------------
		public void AdjustColumnHeaderHeight()
		{
			var newHeight = ColumnGroups.Aggregate(0, (curr, grp) =>
				Math.Max(curr, grp.GetPreferredHeaderHeight()));

			ColumnHeadersHeight = (newHeight > 10 ? newHeight : 17);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);
			
			// This will not be empty when the mouse button is down.
			if (MouseButtons != MouseButtons.None || e.ColumnIndex < 0 || e.RowIndex < 0 ||
				!App.IsFormActive(FindForm()))
			{
				return;
			}

			var rc = GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			if (m_phoneInfoPopup.Initialize(this[e.ColumnIndex, e.RowIndex]))
			{
				//m_cellShowingPopup = this[e.ColumnIndex, e.RowIndex];
				m_phoneInfoPopup.Show(rc);
			}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
		//{
		//    base.OnCellMouseLeave(e);
		//    m_cellShowingPopup = null;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void HandlePhoneInfoPopupOpened(object sender, EventArgs e)
		//{
		//    InvalidateCell(m_cellShowingPopup);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void HandlePhoneInfoPopupClosed(object sender, EventArgs e)
		//{
		//    var tmpCell = m_cellShowingPopup;
		//    m_cellShowingPopup = null;
		//    InvalidateCell(tmpCell);
		//}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			if (e.ColumnIndex == -1 && e.RowIndex == -1)
				PaintTopLeftCornerCell(e);
			//else if (m_cellShowingPopup != null && e.ColumnIndex >= 0 && e.RowIndex >= 0 &&
			//    m_cellShowingPopup == this[e.ColumnIndex, e.RowIndex])
			//{
			//    PaintCellShowingPhoneInfoPopup(e);
			//}
			else
				base.OnCellPainting(e);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void PaintCellShowingPhoneInfoPopup(DataGridViewCellPaintingEventArgs e)
		//{
		//    var rc = e.CellBounds;
		//    e.Paint(rc, e.PaintParts);

		//    rc.Inflate(-2, -2);

		//    using (Pen pen = new Pen(e.CellStyle.ForeColor))
		//        e.Graphics.DrawEllipse(pen, rc);

		//    e.Handled = true;
		//}

		/// ------------------------------------------------------------------------------------
		private void PaintTopLeftCornerCell(DataGridViewCellPaintingEventArgs e)
		{
			var rc = e.CellBounds;

			using (var br = new SolidBrush(BackgroundColor))
				e.Graphics.FillRectangle(br, rc);

			// Draw double lines on bottom and top of the top, left corner cell, making sure to
			// leave an open spot for the intersection with lines from the row and column headers.
			using (var pen = new Pen(GridColor))
			{
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Y, rc.Right - 1, rc.Bottom - 3);
				e.Graphics.DrawLine(pen, rc.Right - 3, rc.Y, rc.Right - 3, rc.Bottom - 3);
				e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 1, rc.Right - 3, rc.Bottom - 1);
				e.Graphics.DrawLine(pen, rc.X, rc.Bottom - 3, rc.Right - 3, rc.Bottom - 3);
				e.Graphics.DrawLine(pen, rc.Right - 1, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
			}

			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnScroll(ScrollEventArgs e)
		{
			base.OnScroll(e);

			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
				RefreshColumnHeaderPainting();
			else
				RefreshRowHeaderPainting();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			RefreshColumnHeaderPainting();
			RefreshRowHeaderPainting();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellLeave(DataGridViewCellEventArgs e)
		{
			base.OnCellLeave(e);
			RefreshColumnHeaderPainting();
			RefreshRowHeaderPainting();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDoubleClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseDoubleClick(e);

			if (e.Button == MouseButtons.Left && !string.IsNullOrEmpty(CurrentPhone))
				App.MsgMediator.SendMessage("ChartPhoneSearchAnywhere", null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Select cells that are right-clicked on.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
				CurrentCell = this[e.ColumnIndex, e.RowIndex];
			
			base.OnCellMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		private void RefreshColumnHeaderPainting()
		{
			var rc = ClientRectangle;
			rc.Height = ColumnHeadersHeight;
			Invalidate(rc);
		}

		/// ------------------------------------------------------------------------------------
		private void RefreshRowHeaderPainting()
		{
			// This used to base its calculations on GetCellDisplayRectangle(-1, -1, false) 
			// which throws ArgumentOutOfRangeException on Mono; see 
			// http://bugzilla.xamarin.com/show_bug.cgi?id=341
			var rc = ClientRectangle;
			rc.Width = RowHeadersWidth;
			Invalidate(rc);
		}

		#region IxCoreColleague Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			return (new IxCoreColleague[] { this });
		}

		#endregion
	}
}
