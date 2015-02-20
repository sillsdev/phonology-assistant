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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilTools;

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

			m_btnScrollLeft.Font = m_btnScrollRight.Font = new Font("Marlett", 9, GraphicsUnit.Point);
			m_btnAddTab.ButtonClick += delegate { AddClickAction(); };
			m_mnuAddInSideBySideGroup.Click += delegate { AddInSideBySideGroupClickAction(); };
			m_mnuAddInStackedGroup.Click += delegate { AddInStackedGroupClickAction(); };
			m_btnScrollLeft.Click += delegate { ScrollLeftClickAction(); };
			m_btnScrollRight.Click += delegate { ScrollRightClickAction(); };
			m_toolstrip.Renderer = new NoToolStripBorderRenderer();
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public bool ScrollButtonsVisible
		{
			get { return m_scrollButtonsVisible; }
			set 
			{
				m_scrollButtonsVisible = value;
				m_btnScrollLeft.Visible = value;
				m_btnScrollRight.Visible = value;
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
			return includeScrollBars ? m_toolstrip.PreferredSize.Width :
				GetMinWidthWithoutScrollButtons();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int y = ClientRectangle.Bottom - 1;
			e.Graphics.DrawLine(SystemPens.ControlDark, 0, y, Right, y);
		}
	}
}
