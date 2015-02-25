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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SilTools;
using SilTools.Controls;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public partial class DistributionChartCellInfoPopup : SilPopup
	{
		//private bool m_drawLeftArrow = true;
		//private bool m_drawArrow = true;
		private Timer _popupTimer;
		private Point _popupLocation;
		private DataGridViewCell _associatedCell;
		private readonly DataGridView _associatedGrid;
		private readonly Font _eticBold;
		
		/// ------------------------------------------------------------------------------------
		public DistributionChartCellInfoPopup(DataGridView associatedGrid)
		{
			InitializeComponent();
			_associatedGrid = associatedGrid;

			_popupTimer = new Timer();
			_popupTimer.Interval = 700;
			_popupTimer.Tick += HandlePopupTimerTick;
			_popupTimer.Stop();

			_eticBold = FontHelper.MakeFont(App.PhoneticFont, FontStyle.Bold);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_eticBold.Dispose();

				if (_popupTimer != null)
				{
					_popupTimer.Tick -= HandlePopupTimerTick;
					_popupTimer.Dispose();
					_popupTimer = null;
				}
			}

			base.Dispose(disposing);
		}
		
		/// ------------------------------------------------------------------------------------
		public void Initialize(string pattern, DataGridViewCell associatedCell)
		{
			Controls.Clear();
			_associatedCell = associatedCell;
			var errors = associatedCell.Value as IEnumerable<SearchQueryValidationError>;
			var errorControl = new SearchQueryValidationErrorControl(pattern, errors.ToList(), true);
			errorControl.Dock = DockStyle.Fill;
			Height = errorControl.GetPreferredHeight();
			Controls.Add(errorControl);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			//Rectangle rc = ClientRectangle;
			//PaintBodyBackground(e.Graphics);

			//// Draw the color shading behind the search pattern.
			//rc.Height = m_lblPattern.Height + Padding.Top + m_lblPattern.Top + m_lblPattern.Margin.Bottom;
			//PaintHeadingBackground(e.Graphics, rc);

			//if (m_drawArrow)
			//{
			//    // Figure out the height of the row that owns the associated cell and calculate
			//    // the vertical midpoint in that row. Since the top of the heading is even with
			//    // the top of the row containing the associated cell, we can figure out where
			//    // the arrow glyph should go so it points to an imaginary line that goes
			//    // horizontally through the midpoint of the cell for whom the popup belongs.
			//    int arrowTipsY = m_associatedGrid.Rows[m_associatedCell.RowIndex].Height / 2;
			//    PaintArrow(e.Graphics, arrowTipsY, rc, m_drawLeftArrow);
			//}

			//DrawSeparatorLine(e.Graphics);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if the mouse is over this popup or its associated cell. If not over either
		/// then hide this popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnTimerTick()
		{
			base.OnTimerTick();

			if (!IsMouseOverCellOrPopup())
				Hide();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Check if the mouse is over this popup or its associated cell. If not over either
		/// then hide this popup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool IsMouseOverCellOrPopup()
		{
			if (_associatedCell == null || _associatedGrid == null || m_mouseOver)
				return true;

			// Get the rectangle for the associated cell.
			var rc = _associatedGrid.GetCellDisplayRectangle(
				_associatedCell.ColumnIndex, _associatedCell.RowIndex, false);

			return rc.Contains(_associatedGrid.PointToClient(MousePosition));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup next to the associated cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Show()
		{
			if (_associatedGrid != null)
			{
				var frm = _associatedGrid.FindForm();
				if (frm != null && !frm.ContainsFocus)
					return;
			}

			_popupTimer.Start();

			//m_drawLeftArrow = true;

			// Get the rectangle for the associated cell.
			var rcCell = _associatedGrid.GetCellDisplayRectangle(
				_associatedCell.ColumnIndex, _associatedCell.RowIndex, false);

			// Get the desired point, relative to the screen, where to show the popup.
			// The desired location is to the right of the associated cell.
			var ptCell = _associatedGrid.PointToScreen(rcCell.Location);
			var ptPopup = new Point(ptCell.X + rcCell.Width - 1, ptCell.Y);

			bool tooWide;
			bool tooTall;
			CheckDesiredPopupLocation(ptPopup, out tooWide, out tooTall);

			// Determine the popup's display rectangle based on it's desired location and size.
			var rcPopup = new Rectangle(ptPopup, Size);

			// If the popup is too wide to be shown at the desired location then adjust
			// its X location to show it to the left of the cell.
			if (tooWide)
			{
				ptPopup.X = ptCell.X - rcPopup.Width + 1;
				//m_drawLeftArrow = false;
			}

			// If the popup is too tall to be shown at the desired location, don't draw an
			// arrow and don't make any coordinate adjustments since .Net will make the
			// adjustment for us, automatically.
			//m_drawArrow = !tooTall;

			_popupLocation = _associatedGrid.PointToClient(ptPopup);
		}

		/// ------------------------------------------------------------------------------------
		public override void Hide()
		{
			base.Hide();
			_popupTimer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		void HandlePopupTimerTick(object sender, EventArgs e)
		{
			if (IsMouseOverCellOrPopup())
			{
				_associatedGrid.Cursor = Cursors.Default;
				base.Show(_associatedGrid, _popupLocation);
				Application.DoEvents();
			}

			_popupTimer.Stop();
		}
	}
}
