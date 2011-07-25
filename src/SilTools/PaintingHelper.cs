using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SilTools
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
		#region OS-specific stuff
#if !__MonoCS__
		[DllImport("User32.dll")]
		extern static public IntPtr GetWindowDC(IntPtr hwnd);
#else
		static public IntPtr GetWindowDC(IntPtr hwnd)
		{
			Console.WriteLine("Warning--using unimplemented method GetWindowDC"); // FIXME Linux
			return(IntPtr.Zero);
		}
#endif

#if !__MonoCS__
		[DllImport("User32.dll")]
		extern static public int ReleaseDC(IntPtr hwnd, IntPtr hdc);
#else
		static public int ReleaseDC(IntPtr hwnd, IntPtr hdc)
		{
			Console.WriteLine("Warning--using unimplemented method ReleaseDC"); // FIXME Linux
			return(-1);
		}
#endif

#if !__MonoCS__
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);
#else
		public static IntPtr GetParent(IntPtr hWnd)
		{
			Console.WriteLine("Warning--using unimplemented method GetParent"); // FIXME Linux
			return(IntPtr.Zero);
		}
#endif

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
			if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) {
				// FIXME Linux - is custom border needed on Linux?
			} else { // Windows
				IntPtr hdc = GetWindowDC(ctrl.Handle);
	
				if (hdc != IntPtr.Zero) {
					using (Graphics g = Graphics.FromHdc(hdc))
					{
						Rectangle rc = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
						ControlPaint.DrawBorder(g, rc, clrBorder, ButtonBorderStyle.Solid);
					}
					ReleaseDC(ctrl.Handle, hdc);
				}
			}
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

			var hotDown = (state == PaintState.HotDown);

			var clr1 = (hotDown ? ProfessionalColors.ButtonPressedGradientBegin :
				ProfessionalColors.ButtonSelectedGradientBegin);

			var clr2 = (hotDown ? ProfessionalColors.ButtonPressedGradientEnd :
				 ProfessionalColors.ButtonSelectedGradientEnd);

			using (var br = new LinearGradientBrush(rc, clr1, clr2, 90))
					g.FillRectangle(br, rc);

			var clrBrdr = (hotDown ? ProfessionalColors.ButtonPressedHighlightBorder :
				ProfessionalColors.ButtonSelectedHighlightBorder);

			ControlPaint.DrawBorder(g, rc, clrBrdr, ButtonBorderStyle.Solid);

			//// Determine the highlight color.
			//Color clrHot = (CanPaintVisualStyle() ?
			//    VisualStyleInformation.ControlHighlightHot : SystemColors.MenuHighlight);

			//int alpha = (CanPaintVisualStyle() ? 95 : 120);

			//// Determine the angle and one of the colors for the gradient highlight. When state is
			//// hot down, the gradiant goes from bottom (lighter) to top (darker). When the state
			//// is just hot, the gradient is from top (lighter) to bottom (darker).
			//float angle = (state == PaintState.HotDown ? 270 : 90);
			//Color clr2 = ColorHelper.CalculateColor(Color.White, clrHot, alpha);

			//// Draw the label's background.
			//if (state == PaintState.Hot)
			//{
			//    using (LinearGradientBrush br = new LinearGradientBrush(rc, Color.White, clr2, angle))
			//        g.FillRectangle(br, rc);
			//}
			//else
			//{
			//    using (LinearGradientBrush br = new LinearGradientBrush(rc, clr2, clrHot, angle))
			//        g.FillRectangle(br, rc);
			//}

			//// Draw a black border around the label.
			//ControlPaint.DrawBorder(g, rc, Color.Black, ButtonBorderStyle.Solid);
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
			var hwndParent = GetParent(hwnd);
			var g = Graphics.FromHwnd(hwndParent);
			var rc = g.VisibleClipBounds;
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

			DrawGradientBackground(g, rc, clrTop, clrBottom, makeDark);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fills the specified rectangle with a gradient background using the specified
		/// colors.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void DrawGradientBackground(Graphics g, Rectangle rc,
			Color clrTop, Color clrBottom, bool makeDark)
 		{
			try
			{
				if (rc.Width > 0 && rc.Height > 0)
				{
					using (var br = new LinearGradientBrush(rc, clrTop, clrBottom, 90))
						g.FillRectangle(br, rc);
				}
			}
			catch { }
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class NoToolStripBorderRenderer : ToolStripProfessionalRenderer
	{
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			// Eat this event.
		}
	}
}
