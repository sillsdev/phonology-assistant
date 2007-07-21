using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Pa.Data;

namespace SIL.Pa.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class is similar to the CharPicker class except this class will combine several
	/// CharPicker classes to form several rows of characters from which to pick.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class CharPickerRows : UserControl, IPhoneListViewer
	{
		private string m_supraSegsToIgnore = PhoneCache.kDefaultChartSupraSegsToIgnore;
		public event ItemDragEventHandler ItemDrag;
		public event ToolStripItemClickedEventHandler ItemClicked;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPickerRows()
		{
			InitializeComponent();
			Reset();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			RefreshSize();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			for (int i = Controls.Count - 1; i >= 0; i--)
			{
				Control ctrl = Controls[i];
				Controls.RemoveAt(i);
				ctrl.Dispose();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadPhones(List<CharGridCell> phoneList)
		{
			if (phoneList == null)
				return;

			CharPicker currRow;
			SortedList<int, CharPicker> pickerRows = new SortedList<int, CharPicker>();

			// Create a collection of pickers (each being a row) sorted by the
			// phone rows.
			foreach (CharGridCell cgc in phoneList)
			{
				if (cgc.Visible)
				{
					if (!pickerRows.TryGetValue(cgc.Row, out currRow))
					{
						currRow = CreateNewPickerRow();
						pickerRows[cgc.Row] = currRow;
					}

					currRow.Items.Add(cgc.Phone);
				}
			}

			// Now add each row to the controls collection.
			SuspendLayout();
			foreach (CharPicker pickerRow in pickerRows.Values)
			{
				Controls.Add(pickerRow);
				pickerRow.BringToFront();
			}

			ResumeLayout(false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new picker row and returns it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public CharPicker CreateNewPickerRow()
		{
			CharPicker pickerRow = new CharPicker();
			pickerRow.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
			pickerRow.CheckItemsOnClick = false;
			pickerRow.AutoSize = true;
			pickerRow.AutoSizeItems = true;
			pickerRow.ShowItemToolTips = false;
			pickerRow.Dock = DockStyle.Top;

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

		/// --------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the list of suprasegmentals to ignore.
		/// </summary>
		/// --------------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SupraSegsToIgnore
		{
			get { return m_supraSegsToIgnore; }
			set { m_supraSegsToIgnore = value; }
		}
	}
}
