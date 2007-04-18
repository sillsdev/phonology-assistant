using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TranslucentOverlay : SIL.SpeechTools.Utils.NoActivateWnd
	{
		private const int kDefaultSize = 50;
		private Control m_parent = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TranslucentOverlay()
		{
			TopLevel = true;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.Manual;
			FormBorderStyle = FormBorderStyle.None;
			DoubleBuffered = true;
			BackColor = Color.Magenta;
			TransparencyKey = Color.Magenta;
			Size = new Size(kDefaultSize, kDefaultSize);
			Left = 0;
			Top = Screen.PrimaryScreen.Bounds.Bottom + 10;
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
		protected override void OnPaint(PaintEventArgs e)
		{
			using (HatchBrush br = new HatchBrush(HatchStyle.Percent50, Color.Black, Color.Transparent))
				e.Graphics.FillRectangle(br, ClientRectangle);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new Point Location
		{
			get { return base.Location; }
			set
			{
				if (base.Location != value)
					base.Location = (m_parent != null ? m_parent.PointToScreen(value) : value);
			}
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new Control Parent
		{
			get { return m_parent; }
			set { m_parent = value; }
		}
	}
}
