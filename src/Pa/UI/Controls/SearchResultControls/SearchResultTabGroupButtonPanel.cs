using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	public partial class SearchResultTabGroupButtonPanel : UserControl
	{
		private bool m_scrollButtonsVisible = true;

		public Action AddClickAction;
		public Action AddInSideBySideGroupClickAction;
		public Action AddInStackedGroupClickAction;
		public Action ScrollLeftClickAction;
		public Action ScrollRightClickAction;
		
		/// ------------------------------------------------------------------------------------
		public SearchResultTabGroupButtonPanel()
		{
			InitializeComponent();

			m_btnLeft.Font = m_btnRight.Font = new Font("Marlett", 9, GraphicsUnit.Point);

			m_btnAddTab.MouseEnter += delegate { m_btnAddTab.Image = Properties.Resources.NewTabHot; };
			m_btnAddTab.MouseLeave += delegate { m_btnAddTab.Image = Properties.Resources.NewTabNormal; };

			m_btnAddTab.ButtonClick += delegate { AddClickAction(); };
			m_mnuAddInSideBySideGroup.Click += delegate { AddInSideBySideGroupClickAction(); };
			m_mnuAddInStackedGroup.Click += delegate { AddInStackedGroupClickAction(); };
			m_btnLeft.Click += delegate { ScrollLeftClickAction(); };
			m_btnRight.Click += delegate { ScrollRightClickAction(); };
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public bool ScrollButtonsVisible
		{
			get { return m_scrollButtonsVisible; }
			set 
			{
				m_scrollButtonsVisible = value;
				m_btnLeft.Visible = value;
				m_btnRight.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public int GetMinWidthWithoutScrollButtons()
		{
			int width = m_btnAddTab.Width + m_btnAddTab.Margin.Left +
				m_btnAddTab.Margin.Right;

			return width;
		}
		/// ------------------------------------------------------------------------------------
		public int GetMinWidth()
		{
			return GetMinWidth(m_scrollButtonsVisible);
		}

		/// ------------------------------------------------------------------------------------
		public int GetMinWidth(bool includeScrollBars)
		{
			return includeScrollBars ? toolStrip1.PreferredSize.Width :
				GetMinWidthWithoutScrollButtons();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int y = ClientRectangle.Bottom - 1;
			e.Graphics.DrawLine(SystemPens.ControlDark, 0, y, Right, y);
		}
		
		/// ------------------------------------------------------------------------------------
		private void HandleAddTabDropDownClick(object sender, EventArgs e)
		{
			//var pt = new Point(m_btnAddTabDropDown.Width, m_btnAddTabDropDown.Height);
			//pt.X -= m_cmnuAddTab.Width;
			//pt = m_btnAddTabDropDown.PointToScreen(pt);
			//m_cmnuAddTab.Show(pt);
		}

		private void HandleAddTabInHorizTabGroupClick(object sender, EventArgs e)
		{

		}

		private void HandleAddTabInVertTabGroupClick(object sender, EventArgs e)
		{

		}
	}
}
