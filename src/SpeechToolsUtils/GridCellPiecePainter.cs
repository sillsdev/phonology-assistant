using System.Drawing;
using System.Windows.Forms;

namespace SIL.SpeechTools.Utils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Draws a string to be displayed like a phonetic search result is drawn in a grid cell
	/// of a list of phonetic search results.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class GridCellPiecePainter
	{
		public delegate void DrawPiecesHandler(Graphics g, string[] subpieces, int firstSubPiece,
			int lastSubPiece, Rectangle rc, Color textColor, TextFormatFlags flags, bool drawForward);
		
		private readonly Font m_font;
		private readonly Color m_textColorNormal;
		private readonly Color m_textColorSelected;
		private readonly Color m_middlePieceBackColor;
		private readonly Color m_middlePieceForeColor;
		private readonly DrawPiecesHandler m_drawPieces;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GridCellPiecePainter(Font font, Color textColorNormal, Color textColorSelected,
			Color middlePieceForeColor,	Color middlePieceBackColor, DrawPiecesHandler drawPieces)
		{
			m_font = font;
			m_textColorNormal = textColorNormal;
			m_textColorSelected = textColorSelected;
			m_middlePieceForeColor = middlePieceForeColor;
			m_middlePieceBackColor = middlePieceBackColor;
			m_drawPieces = drawPieces;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Paint(DataGridViewCellPaintingEventArgs e, string[] pieces,
			int middlePieceOffset, int middlePieceLength)
		{
			Paint(e, pieces, middlePieceOffset, middlePieceLength,
				(e.State & DataGridViewElementStates.Selected) > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Paint(DataGridViewCellPaintingEventArgs e, string[] pieces,
			int middlePieceOffset, int middlePieceLength, bool selected)
		{
			Rectangle rc = e.CellBounds;

			// Draw default everything but text.
			DataGridViewPaintParts parts = DataGridViewPaintParts.All;
			parts &= ~DataGridViewPaintParts.ContentForeground;
			e.Paint(rc, parts);

			int rightPieceOffset = middlePieceOffset + middlePieceLength;

			// Get the text that makes up the search item.
			// This is used only to measure it's width.
			string middlePiece = string.Join(string.Empty, pieces,	middlePieceOffset,
				middlePieceLength);

			TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

			// Calculate the width in pixels of the middle piece.
			int middlePieceWidth = TextRenderer.MeasureText(e.Graphics, middlePiece,
				m_font, Size.Empty, flags).Width;

			// Calculate the center of the cell less half the width of the middle piece.
			int middlePieceLeft = rc.X + (rc.Width / 2) - (middlePieceWidth / 2);

			DrawMiddlePieceBackground(e.Graphics, selected, ref rc,
				middlePieceWidth, middlePieceLeft);

			Color textColor = (selected ? m_textColorSelected : m_textColorNormal);

			// Draw the middle piece
			rc.X = middlePieceLeft;
			rc.Width = e.CellBounds.Right - middlePieceLeft;
			if (m_drawPieces != null)
			{
				m_drawPieces(e.Graphics, pieces, middlePieceOffset, rightPieceOffset,
					rc, m_middlePieceForeColor, flags, true);
			}

			// Draw right piece.
			rc.X = middlePieceLeft + middlePieceWidth;
			rc.Width = e.CellBounds.Right - (middlePieceLeft + middlePieceWidth);
			if (m_drawPieces != null)
			{
				m_drawPieces(e.Graphics, pieces, rightPieceOffset, pieces.Length,
					rc, textColor, flags, true);
			}

			// Draw left piece.
			rc.X = e.CellBounds.X;
			rc.Width = middlePieceLeft - e.CellBounds.X;
			if (m_drawPieces != null)
			{
				m_drawPieces(e.Graphics, pieces, middlePieceOffset - 1, -1,
					rc, textColor, flags, false);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the middle piece's background, taking into consideration when the
		/// cell is selected. In that case, the highlighted background is made a
		/// little transparent so the row's selection color shows through.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawMiddlePieceBackground(Graphics g, bool selected, ref Rectangle rc,
			int middlePieceWidth, int middlePieceLeft)
		{
			Rectangle rcMiddlePieceBackground =
				new Rectangle(middlePieceLeft, rc.Y, middlePieceWidth, rc.Height - 1);
			
			Color backColor = (selected ?
				Color.FromArgb(90, m_middlePieceBackColor) : m_middlePieceBackColor);
			
			using (SolidBrush br = new SolidBrush(backColor))
				g.FillRectangle(br, rcMiddlePieceBackground);
		}
	}
}
