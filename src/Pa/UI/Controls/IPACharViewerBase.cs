// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Drawing;
using System.Windows.Forms;
using SilUtils;

namespace SIL.Pa.UI.Controls
{
	public class IPACharViewerBase : GroupBox
	{
		protected int[][] m_codes;
		protected Font m_font;
		protected ulong m_mask;
		protected bool m_charsMustMatchAllFeatures;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IPACharViewerBase()
		{
			m_font = new Font(FontHelper.PhoneticFont.Name, 14, GraphicsUnit.Point);
			DoubleBuffered = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We don't want no text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get {return string.Empty;}
			set {;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Forcing the font really small shrinks the gap between the top of the control and
		/// the top edge of the group box's frame.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get {return new Font(base.Font.Name, 1);}
			set {;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mask"></param>
		/// <param name="charsMustMatchAllFeatures"></param>
		/// ------------------------------------------------------------------------------------
		public void SetMask(ulong mask, bool charsMustMatchAllFeatures)
		{
			m_mask = mask;
			m_charsMustMatchAllFeatures = charsMustMatchAllFeatures;
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paint the IPA characters inside the frame.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (m_codes == null || m_codes.Length == 0)
				return;

			// Figure out the size of each character's cell.
			int totalRows = m_codes.Length;
			int totalCols = m_codes[0].Length;
			int width = (ClientRectangle.Width - 5) / totalCols;
			int height = (ClientRectangle.Height - 5) / totalRows;
			Rectangle rc = new Rectangle(5, 5, width, height);

			using (SolidBrush br = new SolidBrush(Color.Black))
			using (StringFormat sf = Utils.GetStringFormat(true))
			{
				for (int row = 0; row < totalRows; row++, rc.Y += height)
				{
					rc.X = 4;
					for (int col = 0; col < totalCols; col++, rc.X += width)
					{
						if (m_codes[row][col] == 0)
							continue;

						int codepoint = m_codes[row][col];

						if (!PaApp.DesignMode)
						{
							br.Color = SystemColors.GrayText;
							IPACharInfo charInfo = PaApp.IPACharCache[codepoint];

							// Determine whether or not the character should appear bold.
							if ((m_charsMustMatchAllFeatures && charInfo.BinaryMask == m_mask) ||
								(!m_charsMustMatchAllFeatures && (charInfo.BinaryMask & m_mask) > 0))
							{
								br.Color = SystemColors.ControlText;
							}
						}

						Rectangle rcAdjusted = AdjustRectanlge(rc, codepoint);
						string chr = new string((char)codepoint, 1);
						e.Graphics.DrawString(chr, m_font, br, rcAdjusted, sf);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Allows subclasses to tweak the display rectangle for particular characters.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual Rectangle AdjustRectanlge(Rectangle rc, int codepoint)
		{
			return rc;
		}
	}
}
