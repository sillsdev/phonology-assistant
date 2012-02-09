using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This form doesn't steal focus when it gets shown.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NoActivateWnd : Form
	{
#if !__MonoCS__
		[DllImport("user32.dll")]
		private extern static bool SetWindowPos(IntPtr hWnd,
		  IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int flags);
#else
		private static bool SetWindowPos(IntPtr hWnd,
		  IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int flags)
		{
			Console.WriteLine("Warning--using unimplemented method SetWindowPos"); // FIXME Linux
			return(false);
		}
#endif

		private const int HWND_TOPMOST = -1; // 0xffff 
		private const int SWP_NOSIZE = 1; // 0x0001 
		private const int SWP_NOMOVE = 2; // 0x0002 
		private const int SWP_NOACTIVATE = 16; // 0x0010 
		private const int SWP_SHOWWINDOW = 64; // 0x0040 

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new virtual void Show()
		{
			Show(Location, Size);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void Show(Point location, Size size)
		{
			// This is redundant, I know. But it seems to be necessary for the first
			// time the thing is shown. Otherwise, there is noticable flicker when it
			// adjusts to the appropriate size. Argh!!!
			Size = size;

			Location = location;

			if (!Visible)
			{
				SetWindowPos(Handle, (IntPtr)HWND_TOPMOST, Location.X, Location.Y,
					size.Width, size.Height, SWP_NOACTIVATE | SWP_SHOWWINDOW);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}
	}
}
