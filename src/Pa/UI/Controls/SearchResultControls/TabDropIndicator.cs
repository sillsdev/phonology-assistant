// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
// File: TabDropIndicator.cs
// Responsibility: D. Olson
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TabDropIndicator : TranslucentOverlay
	{
		private const int kDefaultIndicatorWidth = 50;
		private readonly SearchResultTabGroup m_tabGroup;
		private readonly int m_height;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TabDropIndicator(SearchResultTabGroup tabGroup, int height)
			: base(tabGroup)
		{
			m_tabGroup = tabGroup;
			m_height = height;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// 
		///// </summary>
		///// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rc = ClientRectangle;

			Point[] pts = new[] {new Point(rc.X, rc.Bottom),
		        new Point(rc.X, rc.Y + 3), new Point(rc.X + 3, rc.Y),
		        new Point(rc.Right - 4, rc.Y), new Point(rc.Right - 1, rc.Y + 3),
		        new Point(rc.Right - 1, rc.Bottom)};

			using (HatchBrush br = new HatchBrush(HatchStyle.Percent50, Color.Black, Color.Transparent))
				e.Graphics.FillPolygon(br, pts);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When draggingTab is true it means the indicator is used for a tab being dragged.
		/// When draggingTab is false it means the indicator is used for a search pattern
		/// being dragged (e.g. from the saved patterns list).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Locate(bool draggingTab)
		{
			Point pt = GetIndicatorLocation(draggingTab);

			// If the point where we figured on placing the indicator
			// is too far to the right, then bump it left so it just fits.
			if (pt.X + Width > m_tabGroup.Width)
				pt.X = m_tabGroup.Width - Width + 1;

			Location = pt;
			Show();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will determine where to place the indicator and how wide it should be.
		/// In making that determination, consideration is made for what is being dragged and
		/// whether or not the active tab in the target tab group is empty. When tabs are
		/// being dragged, then the indicator will always show up at the end of all existing
		/// tabs in the target tab group. If a pattern is being dragged and the active tab
		/// in the target tab group is empty, the indicator will be placed over that tab.
		/// Otherwise, the indicator will be shown at the end of all existing tabs in the
		/// target tab group.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Point GetIndicatorLocation(bool draggingTab)
		{
			Point pt;

			if (m_tabGroup.Tabs == null || m_tabGroup.Tabs.Count == 0)
				pt = m_tabGroup.TabsContainer.PointToScreen(m_tabGroup.TabsContainer.Location);
			else
			{
				SearchResultTab tab = (!draggingTab && m_tabGroup.CurrentTab.IsEmpty ?
					m_tabGroup.CurrentTab : m_tabGroup.Tabs[m_tabGroup.Tabs.Count - 1]);

				if (!draggingTab && tab.IsEmpty && tab.Selected)
				{
					pt = tab.PointToScreen(new Point(0, 0));
					Size = new Size(tab.Width, m_height);
				}
				else
				{
					pt = tab.PointToScreen(new Point(tab.Width, 0));
					Size = new Size(kDefaultIndicatorWidth, m_height);
				}
			}

			return m_tabGroup.PointToClient(pt);
		}
	}
}
