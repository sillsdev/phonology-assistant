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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Subclasses the popup class specifically for a popup displaying a phone's information
	/// for Consonant and Vowel charts and their histograms.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PhoneInfoPopup : SilPopup
	{
		private bool _showRelativeToScreen;
		private Point _popupLocation;
		private Control _ctrl;
		private readonly PhoneInfoPopupContent _content;
		private readonly Timer _popupTimer;

		internal bool DrawArrow { get; private set; }
		internal bool DrawLeftArrow { get; private set; }
		internal DataGridViewCell AssociatedCell { get; private set; }
		internal DataGridView AssociatedGrid { get; private set; }

		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopup()
		{
			DrawArrow = true;
			DrawLeftArrow = true;
			base.DoubleBuffered = true;
			_content = new PhoneInfoPopupContent(this);
			Controls.Add(_content);

			_popupTimer = new Timer();
			_popupTimer.Interval = 700;
			_popupTimer.Tick += m_popupTimer_Tick;
			_popupTimer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		public PhoneInfoPopup(DataGridView associatedGrid) : this()
		{
			AssociatedGrid = associatedGrid;
		}

		/// ------------------------------------------------------------------------------------
		public bool Initialize(DataGridViewCell associatedCell)
		{
			if (associatedCell == null || associatedCell.Value == null)
				return false;

			try
			{
				var phoneInfo = App.Project.PhoneCache[associatedCell.Value as string];
				if (phoneInfo != null)
				{
					_content.Initialize(phoneInfo);
					InternalInitialize();
					AssociatedCell = associatedCell;
					return true;
				}
			}
			catch { }

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public bool Initialize(string phone)
		{
			if (App.Project.PhoneCache[phone] == null)
				return false;

			_content.Initialize(App.Project.PhoneCache[phone]);
			InternalInitialize();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void InternalInitialize()
		{
			Size = _content.Size;
			DrawLeftArrow = true;
			DrawArrow = true;
			_showRelativeToScreen = false;
		}

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
			if (m_mouseOver || ((AssociatedCell == null || AssociatedGrid == null) &&
				_ctrl == null))
			{
				return true;
			}

			Rectangle rc;
			Point pt;

			try
			{
				if (AssociatedGrid == null)
				{
					rc = _ctrl.ClientRectangle;
					pt = _ctrl.PointToClient(MousePosition);
				}
				else
				{
					// Get the rectangle for the associated cell.
					rc = AssociatedGrid.GetCellDisplayRectangle(
						AssociatedCell.ColumnIndex, AssociatedCell.RowIndex, false);

					pt = AssociatedGrid.PointToClient(MousePosition);
				}
			}
			catch
			{
				return false;
			}

			return rc.Contains(pt);
		}

		/// ------------------------------------------------------------------------------------
		public bool ShouldShowPopup(Control ctrl)
		{
			if (ctrl == null)
			{
				var frm = ctrl.FindForm();
				if (frm != null && !frm.ContainsFocus)
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public void Show(Control ctrl, HistogramBar bar)
		{
			if (!Enabled)
				return;

			Debug.Assert(ctrl != null);
			Debug.Assert(bar != null);

			if (!ShouldShowPopup(ctrl))
				return;

			_popupTimer.Start();

			// The desired bottom of the popup is 60 pixels down from the top of the bar.
			// If that's below the bar's bottom, then set the bottom of the popup to 5
			// pixels above the bar's bottom edge.
			int popupTop = (bar.Height > 65 ? 60 : bar.Height - 5) - Height;

			// The desired left edge for the popup is on the right side of the bar,
			// overlapping 25% of its right side. If that causes the popup to extend
			// beyond the right edge of the screen, then move the popup so its shown
			// on the left side of the bar, overlapping 25% of it's left side.
			int popupLeft = bar.Width - (bar.Width / 4);

			Point ptPopup = bar.PointToScreen(new Point(popupLeft, popupTop));

			bool tooWide;
			bool tooTall;
			CheckDesiredPopupLocation(ptPopup, out tooWide, out tooTall);

			if (tooWide)
				ptPopup = bar.PointToScreen(new Point((bar.Width / 4) - Width, popupTop));

			_showRelativeToScreen = true;
			_ctrl = ctrl;
			_popupLocation = ptPopup;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the popup for a grid cell having the specified display rectangle relative to
		/// the specified grid specified owning control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Show(Rectangle rcCell)
		{
			if (!Enabled)
				return;

			Debug.Assert(AssociatedGrid != null);

			if (!ShouldShowPopup(AssociatedGrid))
				return;

			_popupTimer.Start();
			
			DrawLeftArrow = true;

			// Get the desired point, relative to the screen, where to show the popup.
			// The desired location is to the right of the associated cell.
			var ptCell = AssociatedGrid.PointToScreen(rcCell.Location);
			var ptPopup = new Point(ptCell.X + rcCell.Width, ptCell.Y);

			bool tooWide;
			bool tooTall;
			CheckDesiredPopupLocation(ptPopup, out tooWide, out tooTall);

			// Determine the popup's display rectangle based on it's desired location and size.
			var rcPopup = new Rectangle(ptPopup, Size);

			// If the popup is too wide to be shown at the desired location then adjust
			// its X location to show it to the left of the cell.
			if (tooWide)
			{
				ptPopup.X = ptCell.X - rcPopup.Width;
				DrawLeftArrow = false;
			}

			// If the popup is too tall to be shown at the desired location, don't draw an
			// arrow and don't make any coordinate adjustments since .Net will make the
			// adjustment for us, automatically.
			DrawArrow = !tooTall;

			_ctrl = AssociatedGrid;
			_popupLocation = AssociatedGrid.PointToClient(ptPopup);
		}

		/// ------------------------------------------------------------------------------------
		public override void Hide()
		{
			base.Hide();
			_popupTimer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		void m_popupTimer_Tick(object sender, EventArgs e)
		{
			if (!Enabled)
				return;
			
			if (IsMouseOverCellOrPopup())
				base.Show(_showRelativeToScreen ? null : _ctrl, _popupLocation);

			_popupTimer.Stop();
		}
	}
}
