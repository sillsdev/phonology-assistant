// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005, SIL International. All Rights Reserved.   
// <copyright from='2005' to='2005' company='SIL International'>
//		Copyright (c) 2005, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: EditableListBox.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace SIL.Pa
{
	// -----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a list box with editable items.
	/// </summary>
	// -----------------------------------------------------------------------------------------
	public class EditableListBox : ListBox
	{
		/// <summary>Handler for begin item edit events.</summary>
		public delegate bool BeginItemEditHandler(object sender, int index);
		/// <summary>Handler for after item edit events.</summary>
		public delegate bool AfterItemEditHandler(object sender, int index, string newValue);
		/// <summary>Event fired at the beginning of editing an item but before the edit box is displayed.</summary>
		public event BeginItemEditHandler BeginItemEdit;
		/// <summary>Event fired after an item has been edited.</summary>
		public event AfterItemEditHandler AfterItemEdit;

		protected int m_prevIndex = -1;
		private int m_indexBeingEdited = -1;
		private bool m_allowLabelEdit = true;
		private ItemEditBox m_editBox;
		private Color m_clrSelectedItem;
		private StringFormat m_strFmt;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public EditableListBox() : base()
		{
			DrawMode = DrawMode.OwnerDrawFixed;
			IntegralHeight = false;
			SelectionMode = SelectionMode.One;
			ResizeRedraw = true;

			// Use the highlighted color to create the color for selected rows.
			m_clrSelectedItem = Color.FromArgb(130, SystemColors.Highlight);

			// Set the default string alignment for grid cell text.
			m_strFmt = (StringFormat)StringFormat.GenericTypographic.Clone();
			m_strFmt.Alignment = StringAlignment.Near;
			m_strFmt.LineAlignment = StringAlignment.Center;
			m_strFmt.Trimming = StringTrimming.EllipsisCharacter;
			m_strFmt.FormatFlags |= StringFormatFlags.NoWrap;

			Font = (Font)SystemInformation.MenuFont.Clone();
			ItemHeight = Font.Height + 1;

			m_editBox = new ItemEditBox(this);
			m_editBox.Visible = false;
			Controls.Add(m_editBox);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the multistate values and the text
		/// may be edited.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool AllowLabelEdit
		{
			get {return m_allowLabelEdit;}
			set {m_allowLabelEdit = value;}
		}
		#endregion

		#region Misc. Public Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Puts an item's label into the edit mode.
		/// </summary>
		/// <param name="index">The index of the item to edit.</param>
		/// ------------------------------------------------------------------------------------
		public void BeginEdit(int index)
		{
			if (!m_editBox.Visible)
				OnBeginItemEdit(index);
		}
		#endregion

		#region Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Custom draw our items to give them a nicer look than .Net's default list box.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);

			if (Items.Count == 0 || e.Index == m_indexBeingEdited)
				return;

			Color clrText = ForeColor;
			Rectangle rcText = GetItemTextRectangle(e.Bounds);

			// Check if this item is selected.
			if ((e.State & DrawItemState.Selected) != 0)
				clrText = DrawSelectedItemsBackground(e);
			
			// Draw the text
			e.Graphics.DrawString(Items[e.Index].ToString(), Font,
				new SolidBrush(clrText), rcText, m_strFmt);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draws the background for selected items (focused or unfocused).
		/// </summary>
		/// <param name="e"></param>
		/// <returns>The color that should be used to paint the item's text.</returns>
		/// ------------------------------------------------------------------------------------
		private Color DrawSelectedItemsBackground(DrawItemEventArgs e)
		{
			Rectangle rcHighlight = GetItemHighlightRectangle(e.Bounds);

			if (!Focused)
			{
				// Draw the selected item in the unfocused mode.
				e.Graphics.FillRectangle(SystemBrushes.Control, rcHighlight);
				return SystemColors.ControlText;
			}

			// Fill-in the selected item's background color.
			e.Graphics.FillRectangle(new SolidBrush(Color.White), rcHighlight);
			e.Graphics.FillRectangle(new SolidBrush(m_clrSelectedItem), rcHighlight);
			Rectangle rc = rcHighlight;
				
			// Subtract one from the height and width because DrawRectangle draws
			// the right and bottom edges one pixel outside of the rectangle. Then
			// draw the border around the selected item.
			rc.Width--;
			rc.Height--;
			e.Graphics.DrawRectangle(SystemPens.Highlight, rc);
			return ForeColor;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used during painting and returns the rectangle in which the item's text will be
		/// drawn.
		/// </summary>
		/// <param name="rcItem">The paint rectangle of the entire item (i.e. the rectangle
		/// passed to the OnDrawItem event).</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected virtual Rectangle GetItemTextRectangle(Rectangle rcItem)
		{
			return new Rectangle(rcItem.X + 4, rcItem.Y, rcItem.Width - 6, rcItem.Height); 
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used during painting and returns the rectangle used to paint the hightlight for
		/// selected items.
		/// </summary>
		/// <param name="rcItem">The paint rectangle of the entire item (i.e. the rectangle
		/// passed to the OnDrawItem event).</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected virtual Rectangle GetItemHighlightRectangle(Rectangle rcItem)
		{
			return new Rectangle(rcItem.X + 1, rcItem.Y, rcItem.Width - 2, rcItem.Height); 
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Trap the F2 key and put the user in the edit mode.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F2 && m_allowLabelEdit && SelectedIndex >= 0)
				OnBeginItemEdit(SelectedIndex);
			else
				base.OnKeyDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			int i = IndexFromPoint(e.X, e.Y);
			
			if (m_allowLabelEdit && Items.Count >= 0 && m_prevIndex == i && i >= 0 && i < Items.Count)
				OnBeginItemEdit(i);

			m_prevIndex = i;
		}

		#endregion

		#region Item Editing Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns><c>true</c> if item was put into edit mode. Otherwise, <c>false</c>.
		/// </returns>
		/// ------------------------------------------------------------------------------------
		protected virtual bool OnBeginItemEdit(int index)
		{
			if (index < 0 || index >= Items.Count)
				return false;

			// Make sure the selected item is the one referenced by index.
			if (index != SelectedIndex)
				SelectedIndex = index;
			
			if (BeginItemEdit != null)
			{
				if (!BeginItemEdit(this, index))
					return false;
			}
			
			Rectangle rc = GetItemRectangle(index);

			// Set the edit box's width and location.
			int width; 
			Point location;
			GetEditBoxWidthAndLocation(index, rc, out width, out location);
			m_editBox.Width = width;
			m_editBox.Location = location;

			// Set the edit box's font and setting it's height to zero will force it to be as
			// short as is allowed by the font.
			m_editBox.Font = Font;
			m_editBox.Height = 0;
			m_editBox.BackColor = BackColor;
			m_editBox.Text = Items[index].ToString();
			m_editBox.SelectAll();
			m_indexBeingEdited = index;
			
			// Show the edit box and give it focus.
			m_editBox.Visible = true;
			m_editBox.Focus();

			// Draw a solid border around it.
			Graphics graphics = CreateGraphics();
			graphics.DrawRectangle(new Pen(ForeColor, 1), GetEditBoxBorderRectangle(index, rc));
			graphics.Dispose();
			return true;
		}
	
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the width of and the location where the edit text box when the list box is
		/// about to go into the edit mode.
		/// </summary>
		/// <param name="index">Index of item being edited.</param>
		/// <param name="rcItem">The rectangle of the entire item.</param>
		/// <param name="width"></param>
		/// <param name="pt"></param>
		/// ------------------------------------------------------------------------------------
		protected virtual void GetEditBoxWidthAndLocation(int index, Rectangle rcItem,
			out int width, out Point pt)
		{
			width = rcItem.Width - 8;
			pt = new Point(rcItem.X + 4, rcItem.Y + 1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the width of and the location where the edit text box when the list box is
		/// about to go into the edit mode.
		/// </summary>
		/// <param name="index">Index of item being edited.</param>
		/// <param name="rcItem">The rectangle of the entire item.</param>
		/// <returns>The rectangle used to draw the border around the edit box.</returns>
		/// ------------------------------------------------------------------------------------
		protected virtual Rectangle GetEditBoxBorderRectangle(int index, Rectangle rcItem)
		{
			return new Rectangle(rcItem.X + 1, rcItem.Y - 1, rcItem.Width - 3, rcItem.Height + 1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// ------------------------------------------------------------------------------------
		internal void ItemEditingComplete(Keys key, bool shiftTab)
		{
			m_editBox.m_ignoreLostFocusEvent = true;

			if (key != Keys.Escape)
			{
//				// Validate the edit.
//				if (!OnValidateItemEdit(m_indexBeingEdited, m_editBox.Text))
//				{
//					// Edit failed validation so give focus back to edit box.
//					if (!m_editBox.Focused)
//						m_editBox.Focus();
//					return;
//				}
//				
				// Validation must have passed so check if edit should be kept.
				if (!OnAfterItemEdit(m_indexBeingEdited, m_editBox.Text))
					Items[m_indexBeingEdited] = m_editBox.Text;
			}
			
			m_editBox.m_ignoreLostFocusEvent = true;

			// Clean up after editing is complete.
			m_editBox.Visible = false;

			SuspendLayout();
			if (!Focused)
				Focus();
			
			// Get the rectangle of the item we just edited and inflate it in order to clear
			// the border that was around the edit box.
			Rectangle rc = GetItemRectangle(m_indexBeingEdited);
			m_indexBeingEdited = -1;
			rc.Inflate(0, 1);
			Invalidate(rc);
			ResumeLayout();

			if (key != Keys.Escape && key != Keys.Enter)
			{
				// If left edit box by pressing a directional key or tab key, then let the
				// owner process that key press.
				OnKeyDown(new KeyEventArgs(key));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This event occurs after an item has been sucessfully edited.
		/// </summary>
		/// <param name="index">The index of the item being edited.</param>
		/// <param name="newValue">The new value of the item.</param>
		/// <returns><c>true</c> if a delegate updates the item with the new value. Otherwise
		/// <c>false</c> to let the list box update the item automatically. Warning: if the
		/// items in the list box are not string objects, allowing the list box to
		/// automatically update the item with the new value will change that item's type to
		/// the string entered by the user, thus changing the type of the item to a string.
		/// The previous object referenced by the item will be lost.
		/// ------------------------------------------------------------------------------------
		protected virtual bool OnAfterItemEdit(int index, string newValue)
		{
			if (AfterItemEdit != null)
				return AfterItemEdit(this, m_indexBeingEdited, m_editBox.Text);

			return false;
		}
		
		#endregion
	}

	#region ItemEditBox Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for CellEditBox.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class ItemEditBox : System.Windows.Forms.TextBox
	{
		private EditableListBox m_owner;
		internal bool m_ignoreLostFocusEvent = false;

		/// -----------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of an ItemEditBox object.
		/// </summary>
		/// -----------------------------------------------------------------------------------
		public ItemEditBox(EditableListBox owner)
		{
			m_owner = owner;
			this.Visible = false;
			this.BorderStyle = BorderStyle.None;
			this.RightToLeft = m_owner.RightToLeft;
			this.TabStop = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Catching the escape, enter, tab and shift tab keys here (as opposed to IsInputKey,
		/// IsInputChar, OnKeyDown or OnKeyPress) is the only way I found with a TextBox to
		/// avoid those keys causing a ding (or whatever sound Windows uses for an invalid key
		/// press).
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys key)
		{
			if (key == Keys.Escape || key == Keys.Enter || key == Keys.Tab ||
				key == (Keys.Tab | Keys.Shift))
			{
				m_owner.ItemEditingComplete((key == (Keys.Tab | Keys.Shift) ? Keys.Tab : key), 
					(key & Keys.Shift) == Keys.Shift);
				msg.Result = IntPtr.Zero;
				return true;
			}

			return base.ProcessCmdKey(ref msg, key);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnLostFocus(EventArgs e)
		{
			if (!m_ignoreLostFocusEvent)
				m_owner.ItemEditingComplete(Keys.Enter, false);

			m_ignoreLostFocusEvent = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (Visible)
				m_ignoreLostFocusEvent = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			bool handleKey = false;
			bool shiftTab = false;

			switch(e.KeyCode)
			{
				case Keys.Tab:
					shiftTab = ((e.Modifiers & Keys.Shift) == Keys.Shift);
					handleKey = true;
					break;

				case Keys.Up:
				case Keys.Down:
				case Keys.Enter:
				case Keys.Escape:
					handleKey = true;
					break;

				default:
					break;
			}

			if (handleKey)
				m_owner.ItemEditingComplete(e.KeyCode, shiftTab);
			else
				base.OnKeyDown(e);
		}
	}

	#endregion
}
