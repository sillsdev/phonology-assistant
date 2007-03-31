using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.Pa.Data;
using SIL.Pa;
using SIL.FieldWorks.Common.UIAdapters;

namespace SIL.Pa.Controls
{
	public partial class SortOptionsDropDown : UserControl
	{
		public delegate void SortOptionsChangedHandler(SortOptions sortOptions);
		public event SortOptionsChangedHandler SortOptionsChanged;
		
		private const int kHeightWithoutAdvacedOpts = 110;
		private const int kFullHeightWithoutHelp = 217;
		private const int kFullHeightWithHelp = 242;

		private RadioButton[] m_rbSort;
		private RadioButton[] m_rbAdvSort0;
		private RadioButton[] m_rbAdvSort1;
		private RadioButton[] m_rbAdvSort2;
		private CheckBox[] m_cbRL;
		private Panel[] m_pnlAdvSort;
		private int[] m_checkedIndexes;
		private SortOptions m_sortOptions;
		private ITMAdapter m_tmAdapter;
		private bool m_makePhoneticPrimarySortFieldOnChange = true;
		private bool m_showHelpLink = true;
		private bool m_showAdvancedOptions = false;

		#region Constructor and Loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptionsDropDown()
		{
			InitializeComponent();
			
			// Horizontally center the advanced panel. This should be done in the designer
			// but it always seems to get mucked up by a pixel or two, one way or the other.
			grpAdvSortOptions.Left = (ClientSize.Width - grpAdvSortOptions.Width) / 2;

			SetUiFonts();

			m_sortOptions = new SortOptions(true);

			m_rbSort = new RadioButton[] { rbPlaceArticulation, rbMannerArticulation, rbUnicodeOrder };
			m_rbAdvSort0 = new RadioButton[] { rbBefore1st, rbItem1st, rbAfter1st };
			m_rbAdvSort1 = new RadioButton[] { rbBefore2nd, rbItem2nd, rbAfter2nd };
			m_rbAdvSort2 = new RadioButton[] { rbBefore3rd, rbItem3rd, rbAfter3rd };
			m_pnlAdvSort = new Panel[] { pnl0AdvSort, pnl1AdvSort, pnl2AdvSort };
			m_cbRL = new CheckBox[] { cbBeforeRL, cbItemRL, cbAfterRL };

			// Keeps track of selected advanced sorting radio buttons
			m_checkedIndexes = new int[3];
			m_checkedIndexes[0] = m_sortOptions.AdvSortOrder[0];
			m_checkedIndexes[1] = m_sortOptions.AdvSortOrder[1];
			m_checkedIndexes[2] = m_sortOptions.AdvSortOrder[2];
			
			// Assign EventHandlers
			AssignEventHandlers();

			lnkHelp.Top = ClientRectangle.Bottom - lnkHelp.Height - 10;
			lnkHelp.Left = ClientRectangle.Right - lnkHelp.Width - 10;
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// SortOptions constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptionsDropDown(SortOptions sortOptions)	: this(sortOptions, true, null)
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// SortOptions constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptionsDropDown(SortOptions sortOptions,	bool showAdvancedOptions,
			ITMAdapter tmAdapter) : this()
		{
			if (PaApp.DesignMode)
				return;

			m_tmAdapter = (tmAdapter == null ? PaApp.TMAdapter : tmAdapter);

			// Initialize the SortOptions object
			SortOptions = sortOptions;
			ShowAdvancedOptions = showAdvancedOptions;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set UI Fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetUiFonts()
		{
			rbUnicodeOrder.Font = FontHelper.UIFont;
			rbMannerArticulation.Font = FontHelper.UIFont;
			rbPlaceArticulation.Font = FontHelper.UIFont;
			lblBefore.Font = FontHelper.UIFont;
			lblItem.Font = FontHelper.UIFont;
			lblAfter.Font = FontHelper.UIFont;
			lblFirst.Font = FontHelper.UIFont;
			lblSecond.Font = FontHelper.UIFont;
			lblThird.Font = FontHelper.UIFont;
			lblRL.Font = FontHelper.UIFont;
			lnkHelp.Font = FontHelper.UIFont;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get SortInformationList.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SortOptions SortOptions
		{
			get { return m_sortOptions; }
			set 
			{
				m_sortOptions = (value == null ? new SortOptions(true) : value);

				UnassignEventHandlers();
				
				m_rbSort[(int)PhoneticSortType.Unicode].Checked =
					(m_sortOptions.SortType == PhoneticSortType.Unicode);
				m_rbSort[(int)PhoneticSortType.MOA].Checked =
					(m_sortOptions.SortType == PhoneticSortType.MOA);
				m_rbSort[(int)PhoneticSortType.POA].Checked =
					(m_sortOptions.SortType == PhoneticSortType.POA);

				UpdateCheckedIndexes();

				m_rbAdvSort0[m_checkedIndexes[0]].Checked = true;
				m_rbAdvSort1[m_checkedIndexes[1]].Checked = true;
				m_rbAdvSort2[m_checkedIndexes[2]].Checked = true;

				m_cbRL[0].Checked = m_sortOptions.AdvRlOptions[0];
				m_cbRL[1].Checked = m_sortOptions.AdvRlOptions[1];
				m_cbRL[2].Checked = m_sortOptions.AdvRlOptions[2];

				AdvancedOptionsEnabled = m_sortOptions.AdvancedEnabled;
				pnlAdvSorting.Enabled = m_sortOptions.AdvancedEnabled;

				if (grpAdvSortOptions.Visible != m_showAdvancedOptions)
				{
					grpAdvSortOptions.Visible = m_showAdvancedOptions;
					SetHeight();
				}

				AssignEventHandlers();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool MakePhoneticPrimarySortFieldWhenOptionsChange
		{
			get { return m_makePhoneticPrimarySortFieldOnChange; }
			set { m_makePhoneticPrimarySortFieldOnChange = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show the advanced sorting
		/// options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowAdvancedOptions
		{
			get { return m_showAdvancedOptions; }
			set
			{
				if (m_showAdvancedOptions != value)
				{
					m_showAdvancedOptions = value;
					grpAdvSortOptions.Visible = value;
					SetHeight();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to show the help link at the
		/// bottom of the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ShowHelpLink
		{
			get { return m_showHelpLink; }
			set
			{
				if (m_showHelpLink != value)
				{
					m_showHelpLink = value;
					lnkHelp.Visible = value;
					SetHeight();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not to enable the advanced sorting
		/// options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AdvancedOptionsEnabled
		{
			get { return grpAdvSortOptions.Enabled; }
			set { grpAdvSortOptions.Enabled = value;}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the height of the control based on settings for the help link and the
		/// advanced options.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetHeight()
		{
			if (!m_showAdvancedOptions)
				Height = kHeightWithoutAdvacedOpts;
			else
				Height = (m_showHelpLink ? kFullHeightWithHelp : kFullHeightWithoutHelp);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Assign EventHandlers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AssignEventHandlers()
		{
			foreach (RadioButton rb in m_rbSort)
				rb.Click += new EventHandler(HandleSortTypeChecked);

			foreach (RadioButton rb in m_rbAdvSort0)
				rb.Click += new EventHandler(HandleCheckedColumn0);

			foreach (RadioButton rb in m_rbAdvSort1)
				rb.Click += new EventHandler(HandleCheckedColumn1);

			foreach (RadioButton rb in m_rbAdvSort2)
				rb.Click += new EventHandler(HandleCheckedColumn2);

			foreach (CheckBox cb in m_cbRL)
			{
				cb.Click += new EventHandler(HandleRightLeftCbChecked);
				cb.Enter += new EventHandler(HandleRightLeftCbEnter);
				cb.Leave += new EventHandler(HandleRightLeftCbLeave);
				cb.Paint += new PaintEventHandler(HandleRightLeftCbPaint);
			}

			foreach (Panel pnl in m_pnlAdvSort)
			{
				pnl.Enter += new EventHandler(HandleAdvSortRbEnter);
				pnl.Leave += new EventHandler(HandleAdvSortRbLeave);
				pnl.Paint += new PaintEventHandler(HandleAdvSortRbPaint);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Unassign EventHandlers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UnassignEventHandlers()
		{
			foreach (RadioButton rb in m_rbSort)
				rb.Click -= HandleSortTypeChecked;

			foreach (RadioButton rb in m_rbAdvSort0)
				rb.Click -= HandleCheckedColumn0;

			foreach (RadioButton rb in m_rbAdvSort1)
				rb.Click -= HandleCheckedColumn1;

			foreach (RadioButton rb in m_rbAdvSort2)
				rb.Click -= HandleCheckedColumn2;

			foreach (CheckBox cb in m_cbRL)
			{
				cb.Click -= HandleRightLeftCbChecked;
				cb.Enter -= HandleRightLeftCbEnter;
				cb.Leave -= HandleRightLeftCbLeave;
				cb.Paint -= HandleRightLeftCbPaint;
			}

			foreach (Panel pnl in m_pnlAdvSort)
			{
				pnl.Enter -= HandleAdvSortRbEnter;
				pnl.Leave -= HandleAdvSortRbLeave;
				pnl.Paint -= HandleAdvSortRbPaint;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void Close()
		{
			if (Parent is ToolStripDropDown)
				((ToolStripDropDown)Parent).Close();
			else
			{
				try
				{
					BindingFlags flags = BindingFlags.Instance | BindingFlags.InvokeMethod |
						BindingFlags.Public;
					Parent.GetType().InvokeMember("Close", flags, null, Parent, null);
				}
				catch { }
			}
		}

		#endregion

		#region Events
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void HandleHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Close();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Send the changed message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SendChangedEvent()
		{
			if (SortOptionsChanged != null)
			{
				if (m_makePhoneticPrimarySortFieldOnChange)
					m_sortOptions.SetPrimarySortField(PaApp.Project.FieldInfo.PhoneticField, false);
	
				SortOptionsChanged(m_sortOptions);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the checked indexes array.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateCheckedIndexes()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int i2 = 0; i2 < 3; i2++)
				{
					if (m_sortOptions.AdvSortOrder[i] == i2)
						m_checkedIndexes[i2] = i;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the advance sort order array.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void UpdateAdvSortOrder()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int i2 = 0; i2 < 3; i2++)
				{
					if (m_checkedIndexes[i] == i2)
						m_sortOptions.AdvSortOrder[i2] = i;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle click events for the basic 3 sort types.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSortTypeChecked(object sender, EventArgs e)
		{
			int clickIndex = 0;
			for (int i = 0; i < 3; i++)
			{
				if (m_rbSort[i].Checked)
				{
					clickIndex = i;
					break;
				}
			}

			// Check if sort type was already selected
			if (m_sortOptions.SortType != (PhoneticSortType)clickIndex)
			{
				m_sortOptions.SortType = (PhoneticSortType)clickIndex;
				SendChangedEvent();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Keep the before/item/after radio buttons in sync.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCheckedColumn0(object sender, EventArgs e)
		{
			int clickIndex = 0;
			for (int i = 0; i < 3; i++)
			{
				if (m_rbAdvSort0[i].Checked)
				{
					clickIndex = i;
					break;
				}
			}

			// Return if the sort selection did not change
			if (m_sortOptions.AdvSortOrder[clickIndex] == 0)
				return;

			if (m_checkedIndexes[1] == clickIndex)
			{
				m_checkedIndexes[1] = m_checkedIndexes[0];
				m_rbAdvSort1[m_checkedIndexes[1]].Checked = true;
			}
			else
			{
				m_checkedIndexes[2] = m_checkedIndexes[0];
				m_rbAdvSort2[m_checkedIndexes[2]].Checked = true;
			}

			m_checkedIndexes[0] = clickIndex;
			UpdateAdvSortOrder();
			SendChangedEvent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Keep the before/item/after radio buttons in sync.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCheckedColumn1(object sender, EventArgs e)
		{
			int clickIndex = 0;
			for (int i = 0; i < 3; i++)
			{
				if (m_rbAdvSort1[i].Checked)
				{
					clickIndex = i;
					break;
				}
			}

			// Return if the sort selection did not change
			if (m_sortOptions.AdvSortOrder[clickIndex] == 1)
				return;

			if (m_checkedIndexes[2] == clickIndex)
			{
				m_checkedIndexes[2] = m_checkedIndexes[1];
				m_rbAdvSort2[m_checkedIndexes[2]].Checked = true;
			}
			else
			{
				m_checkedIndexes[0] = m_checkedIndexes[1];
				m_rbAdvSort0[m_checkedIndexes[0]].Checked = true;
			}

			m_checkedIndexes[1] = clickIndex;
			UpdateAdvSortOrder();
			SendChangedEvent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Keep the before/item/after radio buttons in sync.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCheckedColumn2(object sender, EventArgs e)
		{
			int clickIndex = 0;
			for (int i = 0; i < 3; i++)
			{
				if (m_rbAdvSort2[i].Checked)
				{
					clickIndex = i;
					break;
				}
			}

			// Return if the sort selection did not change
			if (m_sortOptions.AdvSortOrder[clickIndex] == 2)
				return;

			if (m_checkedIndexes[0] == clickIndex)
			{
				m_checkedIndexes[0] = m_checkedIndexes[2];
				m_rbAdvSort0[m_checkedIndexes[0]].Checked = true;
			}
			else
			{
				m_checkedIndexes[1] = m_checkedIndexes[2];
				m_rbAdvSort1[m_checkedIndexes[1]].Checked = true;
			}

			m_checkedIndexes[2] = clickIndex;
			UpdateAdvSortOrder();
			SendChangedEvent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the check event for R/L checkboxes.
		/// Update AdvRlOptions with the checked/unchecked state.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleRightLeftCbChecked(object sender, EventArgs e)
		{
			for (int i = 0; i < 3; i++)
				m_sortOptions.AdvRlOptions[i] = m_cbRL[i].Checked;

			SendChangedEvent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'enter' event for the advanced sort radio buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleAdvSortRbEnter(object sender, EventArgs e)
		{
			Panel panel = sender as Panel;
			if (panel != null)
			{
				panel.Tag = 0;
				panel.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'leave' event for the advanced sort radio buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleAdvSortRbLeave(object sender, EventArgs e)
		{
			Panel panel = sender as Panel;
			if (panel != null)
			{
				panel.Tag = null;
				panel.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'paint' event for the advanced sort radio buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleAdvSortRbPaint(object sender, PaintEventArgs e)
		{
			Panel panel = sender as Panel;
			// Draw a 'selection' rectangle around the radio button with focus.
			if (panel.Tag != null)
				ControlPaint.DrawFocusRectangle(e.Graphics, panel.ClientRectangle);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'enter' event for the Right/Left checkboxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleRightLeftCbEnter(object sender, EventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			if (cb != null)
			{
				cb.Tag = 0;
				cb.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'leave' event for the Right/Left checkboxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleRightLeftCbLeave(object sender, EventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			if (cb != null)
			{
				cb.Tag = null;
				cb.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'paint' event for the Right/Left checkboxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleRightLeftCbPaint(object sender, PaintEventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			Rectangle rc = cb.Bounds;
			rc.Inflate(3, 3);
			rc.X--;
			rc.Y--;
			rc.Height++;

			using (Graphics g = pnlAdvSorting.CreateGraphics())
			{
				// Draw a 'selection' rectangle around the checkbox with focus.
				if (cb.Tag != null)
					ControlPaint.DrawFocusRectangle(g, rc);
				else
				{
					// Must decrease the size by 1 to erase the rectangle
					rc.Height--;
					rc.Width--;
					using (Pen pen = new Pen(BackColor))
						g.DrawRectangle(pen, rc);
				}
			}
		}

		#endregion
	}
}
