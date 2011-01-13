using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SilTools
{
	#region SilButtonCell class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SilButtonCell : DataGridViewTextBoxCell
	{
		private bool m_mouseOverButton;
		private bool m_mouseDownOnButton;
		private bool m_enabled = true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Repaint the cell when it's enabled property changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Enabled
		{
			get { return m_enabled; }
			set
			{
				m_enabled = value;
				DataGridView.InvalidateCell(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the cell's owning SilButtonColumn.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SilButtonColumn OwningButtonColumn
		{
			get
			{
				if (DataGridView == null || ColumnIndex < 0 || ColumnIndex >=
					DataGridView.Columns.Count)
				{
					return null;
				}

				return (DataGridView.Columns[ColumnIndex] as SilButtonColumn);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the cell's button should be shown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowButton
		{
			get
			{
				bool owningColShowValue =
					(OwningButtonColumn != null && OwningButtonColumn.ShowButton);
				
				var row = DataGridView.CurrentRow;

				return (owningColShowValue && RowIndex >= 0 &&
					((row == null && DataGridView.AllowUserToAddRows) ||
					(row != null && RowIndex == row.Index)));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the specified point is over the cell's
		/// radio button. The relativeToCell flag is true when the specified point's origin
		/// is relative to the upper right corner of the cell. When false, it's assumed the
		/// point's origin is relative to the cell's owning grid control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsPointOverButton(Point pt, bool relativeToCell)
		{
			// Get the rectangle for the radion button area.
			Rectangle rc = DataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, false);
			Rectangle rcrb;
			Rectangle rcText;
			GetRectangles(rc, out rcrb, out rcText);

			if (relativeToCell)
			{
				// Set the button's rectangle location
				// relative to the cell instead of the grid.
				rcrb.X -= rc.X;
				rcrb.Y -= rc.Y;
			}

			return rcrb.Contains(pt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(int rowIndex)
		{
			base.OnMouseLeave(rowIndex);
			m_mouseOverButton = false;
			DataGridView.InvalidateCell(this);
			ManageButtonToolTip();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (!m_enabled)
				return;

			if (!IsPointOverButton(e.Location, true) && m_mouseOverButton)
			{
				m_mouseOverButton = false;
				DataGridView.InvalidateCell(this);
				ManageButtonToolTip();
			}
			else if (IsPointOverButton(e.Location, true) && !m_mouseOverButton)
			{
				m_mouseOverButton = true;
				DataGridView.InvalidateCell(this);
				ManageButtonToolTip();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor when the mouse button goes down over the button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (m_mouseOverButton && !m_mouseDownOnButton)
			{
				m_mouseDownOnButton = true;
				DataGridView.InvalidateCell(this);
				ManageButtonToolTip();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Monitor when the user releases the mouse button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			m_mouseDownOnButton = false;
			base.OnMouseUp(e);
			DataGridView.InvalidateCell(this);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (!IsPointOverButton(e.Location, true) || !ShowButton || IsInEditMode)
			{
				base.OnMouseClick(e);
				return;
			}

			SilButtonColumn col =
				DataGridView.Columns[ColumnIndex] as SilButtonColumn;

			if (col != null)
				col.InvokeButtonClick(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ManageButtonToolTip()
		{
			if (OwningButtonColumn != null && m_mouseOverButton && !m_mouseDownOnButton)
				OwningButtonColumn.ShowToolTip();
			else
				OwningButtonColumn.HideToolTip();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Paint(Graphics g, Rectangle clipBounds,
			Rectangle bounds, int rowIndex, DataGridViewElementStates state,
			object value, object formattedValue, string errorText, DataGridViewCellStyle style,
			DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts parts)
		{
			bool useEllipsisPath = (OwningButtonColumn != null &&
				OwningButtonColumn.DrawTextWithEllipsisPath);

			if (!ShowButton && !useEllipsisPath)
			{
				base.Paint(g, clipBounds, bounds, rowIndex, state, value,
					formattedValue, errorText, style, advancedBorderStyle, parts);

				return;
			}

			// Draw default everything but text.
			parts &= ~DataGridViewPaintParts.ContentForeground;
			base.Paint(g, clipBounds, bounds, rowIndex, state, value,
				formattedValue, errorText, style, advancedBorderStyle, parts);

			// Get the rectangles for the two parts of the cell.
			Rectangle rcbtn;
			Rectangle rcText;
			GetRectangles(bounds, out rcbtn, out rcText);
			DrawButton(g, rcbtn);
			DrawCellText(g, value as string, style, rcText);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the button in the cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawButton(Graphics g, Rectangle rcbtn)
		{
			if (!ShowButton)
				return;

			var buttonStyle = OwningButtonColumn.ButtonStyle;

			if (buttonStyle == SilButtonColumn.ButtonType.MinimalistCombo)
				DrawMinimalistButton(g, rcbtn);
			else
			{
				if ((buttonStyle == SilButtonColumn.ButtonType.VisualStyleCombo ||
					buttonStyle == SilButtonColumn.ButtonType.VisualStylePush) &&
					!DrawVisualStyledButton(buttonStyle, g, rcbtn))
				{
					DrawPlainButton(buttonStyle, g, rcbtn);
				}
			}

			string buttonText = (OwningButtonColumn == null ? null : OwningButtonColumn.ButtonText);
			if (string.IsNullOrEmpty(buttonText))
				return;

			Font buttonFont = (OwningButtonColumn == null ?
				SystemInformation.MenuFont : OwningButtonColumn.ButtonFont);

			// Draw text
			const TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
				TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis |
				TextFormatFlags.NoPadding | TextFormatFlags.PreserveGraphicsClipping;

			Color clrText = (m_enabled ? SystemColors.ControlText : SystemColors.GrayText);
			TextRenderer.DrawText(g, buttonText, buttonFont, rcbtn, clrText, flags);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawMinimalistButton(Graphics g, Rectangle rc)
		{
			var element = GetVisualStyleComboButton();
			rc = AdjustRectToDefaultComboButtonWidth(rc);

			if (element != VisualStyleElement.ComboBox.DropDownButton.Normal &&
				element != VisualStyleElement.ComboBox.DropDownButton.Disabled &&
				PaintingHelper.CanPaintVisualStyle(element))
			{
				var renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(g, rc);
			}
			else
			{
				var pen = (element == VisualStyleElement.ComboBox.DropDownButton.Disabled ?
					SystemPens.GrayText : SystemPens.WindowText);

				var x = rc.X + (int)Math.Round((rc.Width - 7) / 2f, MidpointRounding.AwayFromZero);
				var y = rc.Y + (int)Math.Round((rc.Height - 4) / 2f, MidpointRounding.AwayFromZero);
				g.DrawLine(pen, x, y, x + 6, y++);
				g.DrawLine(pen, x + 1, y, x + 5, y++);
				g.DrawLine(pen, x + 2, y, x + 4, y);
				g.DrawLine(pen, x + 3, y, x + 3, y + 1);
				return;
			}
		}

		/// ------------------------------------------------------------------------------------
		private bool DrawVisualStyledButton(SilButtonColumn.ButtonType buttonStyle,
			IDeviceContext g, Rectangle rcbtn)
		{
			VisualStyleElement element = (buttonStyle == SilButtonColumn.ButtonType.VisualStyleCombo ?
				GetVisualStyleComboButton() : GetVisualStylePushButton());

			if (!PaintingHelper.CanPaintVisualStyle(element))
				return false;

			VisualStyleRenderer renderer = new VisualStyleRenderer(element);
			rcbtn = AdjustRectToDefaultComboButtonWidth(rcbtn);
			renderer.DrawBackground(g, rcbtn);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle AdjustRectToDefaultComboButtonWidth(Rectangle rc)
		{
			if (!OwningButtonColumn.DrawDefaultComboButtonWidth)
				return rc;

			var rcNew = rc;
			rcNew.Width = SystemInformation.VerticalScrollBarWidth;
			rcNew.X = (rc.Right - rcNew.Width);
			return rcNew;
		}

		/// ------------------------------------------------------------------------------------
		private void DrawPlainButton(SilButtonColumn.ButtonType type, Graphics g, Rectangle rcbtn)
		{
			ButtonState state = (m_mouseDownOnButton && m_mouseOverButton && m_enabled ?
				ButtonState.Pushed : ButtonState.Normal);

			if (!m_enabled)
				state |= ButtonState.Inactive;

			if (type != SilButtonColumn.ButtonType.PlainCombo)
				ControlPaint.DrawButton(g, rcbtn, state);
			else
			{
				rcbtn = AdjustRectToDefaultComboButtonWidth(rcbtn);
				ControlPaint.DrawComboButton(g, rcbtn, state);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the cell's text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void DrawCellText(IDeviceContext g, string text, DataGridViewCellStyle style,
			Rectangle rcText)
		{
			if (string.IsNullOrEmpty(text))
				return;
			
			// Determine the text's proper foreground color.
			Color clrText = SystemColors.GrayText;
			if (m_enabled && DataGridView != null)
				clrText = (Selected ? style.SelectionForeColor : style.ForeColor);

			bool useEllipsisPath = (OwningButtonColumn != null &&
				OwningButtonColumn.DrawTextWithEllipsisPath);

			TextFormatFlags flags = TextFormatFlags.LeftAndRightPadding |
				TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
				TextFormatFlags.NoPrefix | (useEllipsisPath ?
				TextFormatFlags.PathEllipsis : TextFormatFlags.EndEllipsis) |
				TextFormatFlags.PreserveGraphicsClipping;

			TextRenderer.DrawText(g, text, style.Font, rcText, clrText, flags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the correct visual style push button given the state of the cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private VisualStyleElement GetVisualStylePushButton()
		{
			VisualStyleElement element = VisualStyleElement.Button.PushButton.Normal;

			if (!m_enabled)
				element = VisualStyleElement.Button.PushButton.Disabled;
			else if (m_mouseOverButton)
			{
				element = (m_mouseDownOnButton ?
					VisualStyleElement.Button.PushButton.Pressed :
					VisualStyleElement.Button.PushButton.Hot);
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the correct visual style combo button given the state of the cell.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private VisualStyleElement GetVisualStyleComboButton()
		{
			VisualStyleElement element = VisualStyleElement.ComboBox.DropDownButton.Normal;

			if (!m_enabled)
				element = VisualStyleElement.ComboBox.DropDownButton.Disabled;
			else if (m_mouseOverButton)
			{
				element = (m_mouseDownOnButton ?
					VisualStyleElement.ComboBox.DropDownButton.Pressed :
					VisualStyleElement.ComboBox.DropDownButton.Hot);
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the rectangle for the radio button and the text, given the specified cell
		/// bounds.
		/// </summary>
		/// <param name="bounds">The rectangle of the entire cell.</param>
		/// <param name="rcbtn">The returned rectangle for the button.</param>
		/// <param name="rcText">The returned rectangle for the text.</param>
		/// ------------------------------------------------------------------------------------
		public void GetRectangles(Rectangle bounds, out Rectangle rcbtn, out Rectangle rcText)
		{
			if (!ShowButton)
			{
				rcbtn = Rectangle.Empty;
				rcText = bounds;
				return;
			}

			int buttonWidth = (OwningButtonColumn == null ?
				SystemInformation.VerticalScrollBarWidth : OwningButtonColumn.ButtonWidth);
			
			bool paintComboButton = (OwningButtonColumn == null ? false :
				OwningButtonColumn.ButtonStyle != SilButtonColumn.ButtonType.PlainPush &&
				OwningButtonColumn.ButtonStyle != SilButtonColumn.ButtonType.VisualStylePush);
				
			if (paintComboButton)
				buttonWidth += 2;

			rcText = bounds;
			rcText.Width -= buttonWidth;

			rcbtn = bounds;
			rcbtn.Width = buttonWidth;
			rcbtn.X = bounds.Right - buttonWidth - 1;
			rcbtn.Y--;

			if (paintComboButton)
			{
				rcbtn.Width -= 2;
				rcbtn.Height -= 3;
				rcbtn.X++;
				rcbtn.Y += 2;
			}
		}
	}

	#endregion
}
