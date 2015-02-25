// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is similar to the CharPicker class except this class will combine several
	/// CharPicker classes to form several rows of characters from which to pick.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CharPickerRows : UserControl
	{
		private readonly Func<IEnumerable<PhoneInfo>> _phonesToLoadProvider;

		public event ItemDragEventHandler ItemDrag;
		public event ToolStripItemClickedEventHandler ItemClicked;

		/// ------------------------------------------------------------------------------------
		public CharPickerRows(Func<IEnumerable<PhoneInfo>> phonesToLoadProvider)
		{
			_phonesToLoadProvider = phonesToLoadProvider;
			InitializeComponent();
			Reset();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			RefreshSize();
		}

		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			for (int i = Controls.Count - 1; i >= 0; i--)
			{
				var ctrl = Controls[i];
				Controls.RemoveAt(i);
				ctrl.Dispose();
			}

			LoadPhones();
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshFont()
		{
			foreach (CharPicker picker in Controls)
				picker.RefreshFont();

			RefreshSize();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the width and height of this control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RefreshSize()
		{
			// Set the size of the control based on the width of the widest picker and the
			// number of pickers and their heights.
			int maxRowWidth = 0;
			int height = 0;
			foreach (CharPicker picker in Controls)
			{
				maxRowWidth = Math.Max(maxRowWidth, picker.PreferredSize.Width);
				height += picker.Height;
			}

			Width = maxRowWidth;
			Height = height + 4;
			MinimumSize = new Size(maxRowWidth, 0);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPhones()
		{
			SuspendLayout();
			PhoneInfo prevPhoneInfo = null;
			CharPicker currRow = null;

			foreach (var phoneInfo in _phonesToLoadProvider())
			{
				if (prevPhoneInfo == null || prevPhoneInfo.RowGroup != phoneInfo.RowGroup)
				{
					Controls.Add((currRow = CreateNewPickerRow()));
					currRow.BringToFront();
				}

				currRow.Items.Add(phoneInfo.Phone);
				prevPhoneInfo = phoneInfo;
			}

			ResumeLayout(false);
		}

		/// ------------------------------------------------------------------------------------
		public CharPicker CreateNewPickerRow()
		{
			var pickerRow = new CharPicker
			{
				LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow,
				CheckItemsOnClick = false,
				AutoSize = true,
				AutoSizeItems = true,
				ShowItemToolTips = false,
				Dock = DockStyle.Top
			};

			pickerRow.ItemDrag += delegate(object sender, ItemDragEventArgs e)
			{
				if (ItemDrag != null)
					ItemDrag(this, e);
			};

			pickerRow.ItemClicked += delegate(object sender, ToolStripItemClickedEventArgs e)
			{
				if (ItemClicked != null)
					ItemClicked(this, e);
			};

			return pickerRow;
		}
	}
}
