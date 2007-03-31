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
// File: MultiStateListBox.cs
// Responsibility: DavidO
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for MultiStateListBox.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class MultiStateListBox : EditableListBox
	{
		#region Enumerations
		public enum MultiStateTypes
		{
			/// <summary>Column displays checked or unchecked value.</summary>
			CheckBox,
			/// <summary>Column displays plus or minus values in checkbox-like box.</summary>
			PlusMinus,
			/// <summary>Column displays checked, unchecked and gray check values.</summary>
			TriStateCheckBox,
			/// <summary>Column displays plus, minus or empty values in checkbox-like box.</summary>
			TriStatePlusMinus,
		}

		public enum MultiStateValues
		{
			/// <summary>For Checkbox and plus/minius data types this is an empty box.</summary>
			NotSet,
			/// <summary>Value shows check mark.</summary>
			Checked,
			/// <summary>Value shows gray-check mark.</summary>
			GrayCheck,
			/// <summary>Value shows plus sign.</summary>
			Plus,
			/// <summary>Value shows minus sign.</summary>
			Minus,
		}
		#endregion

		#region Class Variables
		private bool m_allowStateEdit = true;
		private Hashtable m_htItemInfo = new Hashtable();
		private MultiStateTypes m_multiStateType = MultiStateTypes.CheckBox;
		private Image m_img;
		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the multi-state values of items
		/// may be changed by clicking them (or pressing the spacebar).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AllowStateEdit
		{
			get {return m_allowStateEdit;}
			set {m_allowStateEdit = value;}
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the type of multi-state list box this is. Once the window handle of
		/// the list box is created, this value cannot be changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[Category("Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public MultiStateTypes MultiStateType
		{
			get {return m_multiStateType;}
			set
			{
				if (!IsHandleCreated || DesignMode)
					m_multiStateType = value;
			}
		}

		#endregion

		#region Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index >= 0 && !DesignMode)
				m_img = GetMultiStateImage(e.Index);

			base.OnDrawItem(e);
			
			if (DesignMode)
				return;

			// Get the rectangle for the multistate box.
			Rectangle rcBox = new Rectangle(e.Bounds.X + 2,
				e.Bounds.Top + ((e.Bounds.Height - m_img.Height) / 2) + 1,
				m_img.Width, m_img.Height);
 
			// Draw the box then save the rectangle in which it was drawn.
			e.Graphics.DrawImage(m_img, rcBox.X, rcBox.Y);
			GetItemInfo(e.Index).m_rcBox = rcBox;
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
		protected override Rectangle GetItemTextRectangle(Rectangle rcItem)
		{
			return new Rectangle(rcItem.X + (m_img.Width + 6), rcItem.Y,
				rcItem.Width - (m_img.Width + 8), rcItem.Height); 
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
		protected override Rectangle GetItemHighlightRectangle(Rectangle rcItem)
		{
			return new Rectangle(rcItem.X + (m_img.Width + 4), rcItem.Y,
				rcItem.Width - (m_img.Width + 5), rcItem.Height); 
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
		protected override void GetEditBoxWidthAndLocation(int index, Rectangle rcItem,
			out int width, out Point pt)
		{
			Rectangle rcBox = GetItemInfo(index).m_rcBox;
			width = rcItem.Width - 8 - (rcBox.Width + 2);
			pt = new Point(rcItem.X + 4 + (rcBox.Width + 2), rcItem.Y + 1);
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
		protected override Rectangle GetEditBoxBorderRectangle(int index, Rectangle rcItem)
		{
			Rectangle rcBox = GetItemInfo(index).m_rcBox;
			return new Rectangle(rcItem.X + 1 + (rcBox.Width + 2), rcItem.Y - 1,
				rcItem.Width - 3 - (rcBox.Width + 2), rcItem.Height + 1);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			int i = IndexFromPoint(e.X, e.Y);

			// Did the user click on the multistate box?
			if (i >= 0 && i < Items.Count && GetItemInfo(i).m_rcBox.Contains(e.X, e.Y))
			{
				if (AllowStateEdit)
					SetItemState(i, GetNextMultiStateValue(GetItemState(i)));
				m_prevIndex = i;
			}
			else
				base.OnMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			
			if (m_allowStateEdit && e.KeyChar == ' ' && SelectedIndex >= 0)
				SetItemState(SelectedIndex, GetNextMultiStateValue(GetItemState(SelectedIndex)));
		}

		#endregion

		#region Public Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the multi-state value of the specified item.
		/// </summary>
		/// <param name="index">Index of item whose state is returned.</param>
		/// <returns>The specified item's state.</returns>
		/// ------------------------------------------------------------------------------------
		public MultiStateValues GetItemState(int index)
		{
			if (!m_htItemInfo.Contains(Items[index]))
				m_htItemInfo.Add(Items[index], new MultiStateItemInfo(MultiStateValues.NotSet));

			return ((MultiStateItemInfo)m_htItemInfo[Items[index]]).m_state;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the multi-state value of the specified item.
		/// </summary>
		/// <param name="index">Index of item whose state to set.</param>
		/// <param name="state">State of specified item.</param>
		/// ------------------------------------------------------------------------------------
		public void SetItemState(int index, MultiStateValues state)
		{
			// Do nothing if the index is out of range.
			if (index < 0 || index >= Items.Count)
				return;
		
			if (!m_htItemInfo.Contains(Items[index]))
			{
				m_htItemInfo.Add(Items[index], new MultiStateItemInfo(state));
				return;
			}

			((MultiStateItemInfo)m_htItemInfo[Items[index]]).m_state = state;		
			Invalidate(GetItemInfo(index).m_rcBox);
		}

		#endregion

		#region Misc. Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected virtual Image GetMultiStateImage(int index)
		{
			Image img;
			MultiStateValues state = GetItemState(index);

			switch (state)
			{
				case MultiStateValues.Checked:
					img = new Bitmap(GetType(), "Images.Check.bmp");
					break;
				case MultiStateValues.GrayCheck:
					img = new Bitmap(GetType(), "Images.GrayCheck.bmp");
					break;
				case MultiStateValues.Plus:
					img = new Bitmap(GetType(), "Images.Plus.bmp");
					break;
				case MultiStateValues.Minus:
					img = new Bitmap(GetType(), "Images.Minus.bmp");
					break;
				default:
					img = new Bitmap(GetType(), "Images.Empty.bmp");
					break;
			}

			return img;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the information object associated with the list item specified by index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private MultiStateItemInfo GetItemInfo(int index)
		{
			if (!m_htItemInfo.Contains(Items[index]))
				m_htItemInfo.Add(Items[index], new MultiStateItemInfo(MultiStateValues.NotSet));

			return (MultiStateItemInfo)m_htItemInfo[Items[index]];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the next appropriate value in a MultiStateValues list based on the
		/// column's data type and the specified current value.
		/// </summary>
		/// <param name="currValue"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		protected virtual MultiStateValues GetNextMultiStateValue(MultiStateValues currValue)
		{
			switch (m_multiStateType)
			{
				case MultiStateTypes.CheckBox:
					return (currValue == MultiStateValues.Checked ?
						MultiStateValues.NotSet : MultiStateValues.Checked);

				case MultiStateTypes.TriStateCheckBox:
					if (currValue == MultiStateValues.Checked)
						return MultiStateValues.GrayCheck;
					else
					{
						return (currValue == MultiStateValues.GrayCheck ?
							MultiStateValues.NotSet : MultiStateValues.Checked);
					}

				case MultiStateTypes.PlusMinus:
					return (currValue == MultiStateValues.Plus ?
						MultiStateValues.Minus : MultiStateValues.Plus);

				case MultiStateTypes.TriStatePlusMinus:
					if (currValue == MultiStateValues.Plus)
						return MultiStateValues.Minus;
					else
					{
						return (currValue == MultiStateValues.Minus ?
							MultiStateValues.NotSet : MultiStateValues.Plus);
					}

				default:
					return MultiStateValues.NotSet;
			}
		}

		#endregion
	}

	#region MultiStateItemInfo Class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates information about a single list box item for a MultiStateListBox.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class MultiStateItemInfo
	{
		internal MultiStateListBox.MultiStateValues m_state;
		internal Rectangle m_rcBox = Rectangle.Empty;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Instantiates an object to store item information for a MultiStateListBox.
		/// </summary>
		/// <param name="state"></param>
		/// ------------------------------------------------------------------------------------
		public MultiStateItemInfo(MultiStateListBox.MultiStateValues state)
		{
			m_state = state;
		}
	}

	#endregion
}