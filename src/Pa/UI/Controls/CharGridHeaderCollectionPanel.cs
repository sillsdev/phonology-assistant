using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a panel that holds a collection of CharGridHeader controls.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class CharGridHeaderCollectionPanel : Panel, IMessageFilter
	{
		private int m_splitPosition = -1;
		private Rectangle m_sizingRectangle;
		private bool m_resizeInProcess = false;
		private SizingLine m_sizingLine;
		private readonly bool m_isForColumns = true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharGridHeaderCollectionPanel(bool forColumns)
		{
			base.DoubleBuffered = true;
			base.BackColor = SystemColors.Window;
			m_isForColumns = forColumns;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (m_splitPosition < 0)
				SplitPosition = (m_isForColumns ? Height / 2 : Width / 2);

			Application.AddMessageFilter(this);
		
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Stop the timer in case it gets fired right after the handle is destroyed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			Application.RemoveMessageFilter(this);
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjust the resizing rectangle when the panel changes sizes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (m_splitPosition < MinSplitPosition)
				SplitPosition = MinSplitPosition;
			else if (m_splitPosition > MaxSplitPosition)
				SplitPosition = MaxSplitPosition;

			SetSizingRectangle();
		}

		#region IMessageFilter Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor where the mouse is so we know when we're over the virtual splitter between
		/// headers and sub headers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg != 0x0200)
				return false;

			if (!PaApp.IsFormActive(FindForm()))
				return false;

			if (m_resizeInProcess)
			{
				if (MouseButtons == MouseButtons.Left)
					MoveSizer();
				else
					FinishResize();

				return false;
			}

			Point pt = PointToClient(MousePosition);
			CharGridHeader hdr = HeaderMouseIsOver;

			if (!m_sizingRectangle.Contains(pt) || hdr == null || !hdr.SubHeadingsVisible)
			{
				if (Cursor != Cursors.Default)
					Cursor = Cursors.Default;
			}
			else
			{
				Cursor sizingCursor = (m_isForColumns ? Cursors.HSplit : Cursors.VSplit);

				if (Cursor != sizingCursor)
					Cursor = sizingCursor;

				if (MouseButtons == MouseButtons.Left)
					StartResize();
			}

			return false;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the sizing line to where the mouse is as long as the mouse is where the
		/// headings sub headings will be at least 10 pixels wide/high.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void MoveSizer()
		{
			Point pt = PointToClient(MousePosition);

			if (m_isForColumns)
			{
				if (pt.Y > MinSplitPosition && pt.Y < MaxSplitPosition)
					m_sizingLine.Top = pt.Y - 2;
			}
			else if (pt.X > MinSplitPosition && pt.X < MaxSplitPosition)
			{
				m_sizingLine.Left = pt.X - 2;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Begins the process of allowing the user to resize the headings and sub headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void StartResize()
		{
			m_resizeInProcess = true;

			m_sizingLine = (m_isForColumns ? 
				new SizingLine(ClientSize.Width, 4) :
				new SizingLine(4, ClientSize.Height));

			m_sizingLine.Visible = true;
			m_sizingLine.Location = m_sizingRectangle.Location;
			Controls.Add(m_sizingLine);
			m_sizingLine.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finishes the process of resizing headings and sub headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FinishResize()
		{
			m_resizeInProcess = false;
			m_sizingRectangle = m_sizingLine.Bounds;
			Controls.Remove(m_sizingLine);
			m_sizingLine.Dispose();
			SplitPosition = (m_isForColumns ?
				m_sizingRectangle.Top + 2 : m_sizingRectangle.Left + 2);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the CharGridHeader the mouse is over.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private CharGridHeader HeaderMouseIsOver
		{
			get
			{
				foreach (Control ctrl in Controls)
				{
					if (ctrl.Bounds.Contains(PointToClient(MousePosition)))
						return ctrl as CharGridHeader;
				}

				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the maximum position for the fake splitter between the heading and
		/// sub headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int MaxSplitPosition
		{
			get { return (m_isForColumns ? ClientSize.Height - 10 : ClientSize.Width - 10); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the minimum position for the fake splitter between the heading and
		/// sub headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int MinSplitPosition
		{
			get { return 10; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the panel is for a collection of chart
		/// column headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsForColumnHeadings
		{
			get { return m_isForColumns; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int SplitPosition
		{
			get { return m_splitPosition; }
			set
			{
				m_splitPosition = value;

				foreach (Control ctrl in Controls)
				{
					if (ctrl is CharGridHeader)
						((CharGridHeader)ctrl).ResizeHeading(m_splitPosition);
				}

				SetSizingRectangle();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not at least one of the headings has its
		/// sub headings visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AreAnySubHeadingsVisible
		{
			get
			{
				foreach (Control ctrl in Controls)
				{
					if (ctrl is CharGridHeader && ((CharGridHeader)ctrl).SubHeadingsVisible)
						return true;
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the rectangle that acts like a splitter between headings and sub headings.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetSizingRectangle()
		{
			if (m_isForColumns)
				m_sizingRectangle = new Rectangle(0, m_splitPosition - 2, ClientSize.Width, 4);
			else
				m_sizingRectangle = new Rectangle(m_splitPosition - 2, 0, 4, ClientSize.Height);
		}
	}
}
