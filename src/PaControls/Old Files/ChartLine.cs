using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ChartLine : System.ComponentModel.Component
	{
		private Control m_control;
		private Point m_start = new Point(10, 10);
		private Point m_end = new Point(30, 10);
		private List<Point> m_dots = new List<Point>();
		private List<Point[]> m_gaps = new List<Point[]>();
		private bool m_endDots = true;

		//private Point[][2] m_gaps = new 

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Control Control
		{
			get { return m_control; }
			set
			{
				m_control = value;
				if (value != null)
				{
					m_control.Paint += new PaintEventHandler(m_control_Paint);
					m_control.Invalidate();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Point StartPoint
		{
			get { return m_start; }
			set
			{
				m_start = value;
				if (m_control != null)
					m_control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Point EndPoint
		{
			get { return m_end; }
			set
			{
				m_end = value;
				if (m_control != null)
					m_control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<Point> DotPoints
		{
			get {return m_dots;}
			set
			{
				if (value == null)
					m_dots.Clear();
				else
					m_dots = value;

				if (m_control != null)
					m_control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<Point[]> Gaps
		{
			get { return m_gaps; }
			set
			{
				if (value == null)
					m_gaps.Clear();
				else
					m_gaps = value;

				if (m_control != null)
					m_control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool EndDots
		{
			get { return m_endDots; }
			set
			{
				m_endDots = value;
				if (m_control != null)
					m_control.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_control_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			if (m_start != Point.Empty && m_end != Point.Empty)
			{
				using (Pen pen = new Pen(m_control.ForeColor))
					e.Graphics.DrawLine(pen, m_start, m_end);
			}

			if (m_endDots)
			{
				DrawDot(e.Graphics, m_control.ForeColor, m_start);
				DrawDot(e.Graphics, m_control.ForeColor, m_end);
			}

			if (m_gaps != null && m_gaps.Count > 0)
			{
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				using (Pen pen = new Pen(m_control.BackColor, 3))
				{
					foreach (Point[] pts in m_gaps)
						e.Graphics.DrawLine(pen, pts[0], pts[1]);
				}
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			}

			if (m_dots != null && m_dots.Count > 0)
			{
				foreach (Point dot in m_dots)
					DrawDot(e.Graphics, m_control.ForeColor, dot);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawDot(Graphics g, Color color, Point pt)
		{
			using (SolidBrush br = new SolidBrush(color))
			{
				// I get a more precise and predictable dot when I draw it as a
				// series of three rectangles of varying sizes and locations.
				
				// Fill in the longest horizontal chunk in the circle.
				Rectangle rc = new Rectangle(pt.X - 3, pt.Y - 1, 7, 3);
				g.FillRectangle(br, rc);

				// Fill in the longest vertical chunk in the circle.
				rc.Inflate(-2, 2);
				g.FillRectangle(br, rc);

				// Fill in the most square chunk in the circle.
				rc.Inflate(1, -1);
				g.FillRectangle(br, rc);
			}
		}
	}
}
