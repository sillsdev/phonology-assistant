using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SIL.SpeechTools.Utils
{
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// Possible painting states for DrawHotBackground
	/// </summary>
	/// ------------------------------------------------------------------------------------
	public enum PaintState
	{
		Normal,
		Hot,
		HotDown,
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Contains misc. static methods for various customized painting.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class PaintingHelper
	{
		#region Windows API imported methods
		[DllImport("User32.dll")]
		extern static public IntPtr GetWindowDC(IntPtr hwnd);

		[DllImport("User32.dll")]
		extern static public int ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		#endregion

		public static int WM_NCPAINT = 0x85;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws around the specified control, a fixed single border the color of text
		/// boxes in a themed environment. If themes are not enabled, the border is black.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void DrawCustomBorder(Control ctrl)
		{
			DrawCustomBorder(ctrl, CanPaintVisualStyle() ?
				VisualStyleInformation.TextControlBorder : Color.Black);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws around the specified control, a fixed single border of the specified color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void DrawCustomBorder(Control ctrl, Color clrBorder)
		{
			IntPtr hdc = GetWindowDC(ctrl.Handle);

			using (Graphics g = Graphics.FromHdc(hdc))
			{
				Rectangle rc = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
				ControlPaint.DrawBorder(g, rc, clrBorder, ButtonBorderStyle.Solid);
			}

			ReleaseDC(ctrl.Handle, hdc);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws a background in the specified rectangle that looks like a toolbar button
		/// when the mouse is over it, with consideration for whether the look should be like
		/// the mouse is down or not. Note, when a PaintState of normal is specified, this
		/// method does nothing. Normal background painting is up to the caller.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void DrawHotBackground(Graphics g, Rectangle rc, PaintState state)
		{
			// The caller has to handle painting when the state is normal.
			if (state == PaintState.Normal)
				return;

			// Determine the highlight color.
			Color clrHot = (CanPaintVisualStyle() ?
				VisualStyleInformation.ControlHighlightHot : SystemColors.MenuHighlight);

			int alpha = (CanPaintVisualStyle() ? 95 : 120);

			// Determine the angle and one of the colors for the gradient highlight. When state is
			// hot down, the gradiant goes from bottom (lighter) to top (darker). When the state
			// is just hot, the gradient is from top (lighter) to bottom (darker).
			float angle = (state == PaintState.HotDown ? 270 : 90);
			Color clr2 = ColorHelper.CalculateColor(Color.White, clrHot, alpha);

			// Draw the label's background.
			if (state == PaintState.Hot)
			{
				using (LinearGradientBrush br = new LinearGradientBrush(rc, Color.White, clr2, angle))
					g.FillRectangle(br, rc);
			}
			else
			{
				using (LinearGradientBrush br = new LinearGradientBrush(rc, clr2, clrHot, angle))
					g.FillRectangle(br, rc);
			}

			// Draw a black border around the label.
			ControlPaint.DrawBorder(g, rc, Color.Black, ButtonBorderStyle.Solid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not visual style rendering is supported
		/// in the application and if the specified element can be rendered.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CanPaintVisualStyle(VisualStyleElement element)
		{
			return (CanPaintVisualStyle() && VisualStyleRenderer.IsElementDefined(element));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a value indicating whether or not visual style rendering is supported
		/// in the application.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool CanPaintVisualStyle()
		{
			return (Application.VisualStyleState != VisualStyleState.NoneEnabled &&
				VisualStyleInformation.IsSupportedByOS &&
				VisualStyleInformation.IsEnabledByUser &&
				VisualStyleRenderer.IsSupported);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Because the popup containers forces a little padding above and below, we need to get
		/// the popup's parent (which is the popup container) and paint its background to match
		/// the menu color.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Graphics PaintDropDownContainer(IntPtr hwnd, bool returnGraphics)
		{
			IntPtr hwndParent = GetParent(hwnd);
			Graphics g = Graphics.FromHwnd(hwndParent);
			RectangleF rc = g.VisibleClipBounds;
			rc.Inflate(-1, -1);
			g.FillRectangle(SystemBrushes.Menu, rc);

			if (!returnGraphics)
			{
				g.Dispose();
				g = null;
			}

			return g;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fills the specified rectangle with a gradient background consistent with the
		/// current system's color scheme. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void DrawGradientBackground(Graphics g, Rectangle rc)
		{
			DrawGradientBackground(g, rc, false);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fills the specified rectangle with a gradient background consistent with the
		/// current system's color scheme. 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void DrawGradientBackground(Graphics g, Rectangle rc, bool makeDark)
 		{
			Color clrTop;
			Color clrBottom;

			if (makeDark)
			{
				clrTop = ColorHelper.CalculateColor(Color.White,
					SystemColors.ActiveCaption, 70);

				clrBottom = ColorHelper.CalculateColor(SystemColors.ActiveCaption,
					SystemColors.ActiveCaption, 0);
			}
			else
			{
				clrTop = ColorHelper.CalculateColor(Color.White,
					SystemColors.GradientActiveCaption, 190);

				clrBottom = ColorHelper.CalculateColor(SystemColors.ActiveCaption,
					SystemColors.GradientActiveCaption, 50);
			}

			try
			{
				if (rc.Width > 0 && rc.Height > 0)
				{
					using (LinearGradientBrush br = new LinearGradientBrush(rc, clrTop, clrBottom, 90))
						g.FillRectangle(br, rc);
				}
			}
			catch { }
		}
	}
}
