using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a control used to drag as an indicator where the edge of a resized control
	/// will end up.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SizingLine : Label
	{
		public SizingLine(int width, int height)
		{
			DoubleBuffered = true;
			AutoSize = false;
			Size = new Size(width, height);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			using (HatchBrush br = new HatchBrush(HatchStyle.Percent50, Color.Black, BackColor))
				e.Graphics.FillRectangle(br, ClientRectangle);
		}
	}
}
