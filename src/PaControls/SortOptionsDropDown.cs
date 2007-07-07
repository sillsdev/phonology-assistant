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
		
		// These values may be adjusted when the system's dpi setting is greater than 96.
		private const int kHeightWithoutAdvacedOpts = 110;
		private const int kFullHeightWithoutHelp = 217;
		private const int kFullHeightWithHelp = 242;

		private RadioButton[] m_rbSort;
		private RadioButton[] m_rbAdvSort0;
		private RadioButton[] m_rbAdvSort1;
		private RadioButton[] m_rbAdvSort2;
		private CheckBox[] m_cbRL;
		private int[] m_checkedIndexes;
		private SortOptions m_sortOptions;
		private ITMAdapter m_tmAdapter;
		private bool m_makePhoneticPrimarySortFieldOnChange = true;
		private bool m_showHelpLink = true;
		private bool m_showAdvancedOptions = false;
		private int m_widestSortTypeRadioButton;

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
			m_cbRL = new CheckBox[] { cbBeforeRL, cbItemRL, cbAfterRL };

			// Keeps track of selected advanced sorting radio buttons
			m_checkedIndexes = new int[3];
			m_checkedIndexes[0] = m_sortOptions.AdvSortOrder[0];
			m_checkedIndexes[1] = m_sortOptions.AdvSortOrder[1];
			m_checkedIndexes[2] = m_sortOptions.AdvSortOrder[2];

			AdjustControls();
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

			// Reset the SortOptions object
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adjusts the size of controls on the drop-down and the drop-down itself. Because
		/// things get sort of out of wack when the system's dpi is changed from 96, we need
		/// to adjust these things manually.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustControls()
		{
			// Find the widest of the 3 upper radio buttons.
			m_widestSortTypeRadioButton = rbPlaceArticulation.Width;
			m_widestSortTypeRadioButton =
				Math.Max(m_widestSortTypeRadioButton, rbMannerArticulation.Width);
			m_widestSortTypeRadioButton =
				Math.Max(m_widestSortTypeRadioButton, rbUnicodeOrder.Width);

			int addToWidth = 0;

			if (lblBefore.Width < lblBefore.PreferredWidth)
				addToWidth += (lblBefore.PreferredWidth - lblBefore.Width);

			if (lblItem.Width < lblItem.PreferredWidth)
				addToWidth += (lblItem.PreferredWidth - lblItem.Width);

			if (lblAfter.Width < lblAfter.PreferredWidth)
				addToWidth += (lblAfter.PreferredWidth - lblAfter.Width);

			if (lblFirst.Width < lblFirst.PreferredWidth)
				addToWidth += (lblFirst.PreferredWidth - lblFirst.Width);

			if (lblSecond.Width < lblSecond.PreferredWidth)
				addToWidth += (lblSecond.PreferredWidth - lblSecond.Width);

			if (lblThird.Width < lblThird.PreferredWidth)
				addToWidth += (lblThird.PreferredWidth - lblThird.Width);

			if (lblRL.Width < lblRL.PreferredWidth)
				addToWidth += (lblRL.PreferredWidth - lblRL.Width);

			// Add 10 more for good measure and set the width of the drop-down.
			grpAdvSortOptions.Width += (addToWidth + 15);
			Width = Math.Max(grpAdvSortOptions.Width + 12,
				m_widestSortTypeRadioButton + 12);

			lnkHelp.Top = ClientRectangle.Bottom - lnkHelp.Height - 8;
			lnkHelp.Left = ClientRectangle.Right - lnkHelp.Width - 10;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			// Center the advanced sort options group box.
			grpAdvSortOptions.Left = (Width - grpAdvSortOptions.Width) / 2;
			
			// Center the advanced sort options group box and the sort type radio buttons.
			grpAdvSortOptions.Left = (Width - grpAdvSortOptions.Width) / 2;
			rbPlaceArticulation.Left = (Width - m_widestSortTypeRadioButton) / 2;
			rbMannerArticulation.Left = (Width - m_widestSortTypeRadioButton) / 2;
			rbUnicodeOrder.Left = (Width - m_widestSortTypeRadioButton) / 2;
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
				tblAdvSorting.Enabled = m_sortOptions.AdvancedEnabled;

				if (grpAdvSortOptions.Visible != m_showAdvancedOptions)
				{
					grpAdvSortOptions.Visible = m_showAdvancedOptions;
					SetHeight();
				}
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
			int	height = (m_showAdvancedOptions ?
				grpAdvSortOptions.Bottom + 7 : grpAdvSortOptions.Top);

			if (m_showHelpLink)
				height += (lnkHelp.Height + 8);

			Height = height;
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
		private void HandleRightToLeftCheckBoxChecked(object sender, EventArgs e)
		{
			for (int i = 0; i < 3; i++)
				m_sortOptions.AdvRlOptions[i] = m_cbRL[i].Checked;

			SendChangedEvent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Redraw the advanced radio buttons and R/L checkboxes so they don't have a focus
		/// rectangle drawn around them..
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleAdvancedOptionItemLeave(object sender, EventArgs e)
		{
			Control ctrl = sender as Control;

			if (ctrl is RadioButton)
			{
				ctrl.Tag = 0;
				ctrl.Parent.Invalidate();
			}
			else
				ctrl.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Redraw the advanced radio buttons and R/L checkboxes so they either have a focus
		/// rectangle drawn around them or they have it erased.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleAdvancedOptionItemEnter(object sender, EventArgs e)
		{
			(sender as Control).Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'paint' event for the Right/Left checkboxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleAdvancedOptionItemPaint(object sender, PaintEventArgs e)
		{
			Control ctrl = sender as Control;

			if (!ctrl.Focused || ctrl.Tag != null)
			{
				ctrl.Tag = null;
				return;
			}

			Rectangle rc;
			Graphics g = e.Graphics;

			if (ctrl is RadioButton)
			{
				rc = ctrl.Bounds;
				rc.Inflate(3, 3);
				g = ctrl.Parent.CreateGraphics();
			}
			else if (ctrl is CheckBox)
			{
				rc = ctrl.ClientRectangle;
				rc.Inflate(-5, 0);
				rc.Width -= 1;
				rc.Height -= 2;
			}
			else
				return;

			ControlPaint.DrawFocusRectangle(g, rc);

			if (ctrl is RadioButton)
				g.Dispose();
		}

		#endregion
	}
}
