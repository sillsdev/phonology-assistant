using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TranslucentOverlay
	{
		[DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
		private static extern int GetDesktopWindow();

		[DllImport("User32.dll")]
		public static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll", EntryPoint = "ReleaseDC")]
		public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

		public delegate void FillOverlayHandler(Graphics g, Rectangle rc);
		public event FillOverlayHandler FillOverlay;

		private const int kDefaultSize = 50;

		private bool m_visible = false;
		private Point m_location = Point.Empty;
		private Size m_size = Size.Empty;
		private Control m_parent = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TranslucentOverlay()
		{
			Size = new Size(kDefaultSize, kDefaultSize);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TranslucentOverlay(Control parent) : this()
		{
			Parent = parent;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void FillOverlayInternal()
		{
			if (m_visible && m_parent != null)
			{
				IntPtr hdc = GetDC(IntPtr.Zero);

				using (Graphics g = Graphics.FromHdc(hdc))
					OnFillOverlay(g, new Rectangle(m_parent.PointToScreen(m_location), m_size));

				ReleaseDC(IntPtr.Zero, hdc);
				Application.DoEvents();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void OnFillOverlay(Graphics g, Rectangle rc)
		{
			if (FillOverlay != null)
				FillOverlay(g, rc);
			else
			{
				using (SolidBrush br = new SolidBrush(Color.FromArgb(100, Color.Gray)))
					g.FillRectangle(br, rc);
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool Visible
		{
			get { return m_visible; }
			set
			{
				if (m_visible != value)
				{
					m_visible = value;
					if (m_visible)
						FillOverlayInternal();
					else if (Parent != null)
					{
						Parent.Invalidate(new Rectangle(m_location, m_size), true);
						Application.DoEvents();
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual Point Location
		{
			get { return m_location; }
			set
			{
				if (m_location != value)
				{
					m_location = value;
					FillOverlayInternal();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual Size Size
		{
			get { return m_size; }
			set 
			{
				if (m_size != value)
				{
					m_size = value;
					//m_bmp = new Bitmap(m_size.Width, m_size.Height,	PixelFormat.Format32bppArgb);
					FillOverlayInternal();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Width
		{
			get { return m_size.Width; }
			set
			{
				if (m_size.Width != value && value > 0)
					Size = new Size(value, m_size.Height);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int Height
		{
			get { return m_size.Height; }
			set
			{
				if (m_size.Height != value && value > 0)
					Size = new Size(m_size.Width, value);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual Control Parent
		{
			get { return m_parent; }
			set { m_parent = value; }
		}

		#endregion
	}
}
