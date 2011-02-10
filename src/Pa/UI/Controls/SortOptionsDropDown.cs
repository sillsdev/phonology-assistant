using System;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public partial class SortOptionsDropDown : UserControl
	{
		public delegate void SortOptionsChangedHandler(SortOptions sortOptions);
		public event SortOptionsChangedHandler SortOptionsChanged;
		
		private bool m_makePhoneticPrimarySortFieldOnChange = true;
		private bool m_showHelpLink = true;
		private bool m_showAdvancedOptions;
		private SortOptions m_sortOptions;
		private readonly int[] m_checkedIndexes;
		private readonly RadioButton[] m_rbSort;
		private readonly RadioButton[] m_rbAdvSort0;
		private readonly RadioButton[] m_rbAdvSort1;
		private readonly RadioButton[] m_rbAdvSort2;
		private readonly CheckBox[] m_chkRL;
		private readonly int m_dxAdvGrpTbl;
		private readonly Point m_advTableLocation;

		#region Constructor and Loading
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptionsDropDown()
		{
			InitializeComponent();

			// Save the difference between the width of the advanced
			// options group and it's contained table layout panel.
			m_dxAdvGrpTbl = grpAdvSortOptions.Width - tblAdvSorting.Width;

			// Save the location of the table layout panel.
			m_advTableLocation = tblAdvSorting.Location;

			// Horizontally center the advanced panel. This should be done in the designer
			// but it always seems to get mucked up by a pixel or two, one way or the other.
			grpAdvSortOptions.Left = (ClientSize.Width - grpAdvSortOptions.Width) / 2;

			SetUiFonts();

			m_sortOptions = new SortOptions(true);

			m_rbSort = new[] { rbPlaceArticulation, rbMannerArticulation, rbUnicodeOrder };
			m_rbAdvSort0 = new[] { rbBefore1st, rbItem1st, rbAfter1st };
			m_rbAdvSort1 = new[] { rbBefore2nd, rbItem2nd, rbAfter2nd };
			m_rbAdvSort2 = new[] { rbBefore3rd, rbItem3rd, rbAfter3rd };
			m_chkRL = new[] { chkBeforeRL, chkItemRL, chkAfterRL };

			// Keeps track of selected advanced sorting radio buttons
			m_checkedIndexes = new int[3];
			m_checkedIndexes[0] = m_sortOptions.AdvSortOrder[0];
			m_checkedIndexes[1] = m_sortOptions.AdvSortOrder[1];
			m_checkedIndexes[2] = m_sortOptions.AdvSortOrder[2];

			LayoutControls();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// SortOptions constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public SortOptionsDropDown(SortOptions sortOptions,	bool showAdvancedOptions) : this()
		{
			if (App.DesignMode)
				return;

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
		public void LayoutControls()
		{
			grpAdvSortOptions.Width = tblAdvSorting.Width = 0;

			// Find the widest of the 3 upper radio buttons.
			int widestSortTypeRB = rbPlaceArticulation.Width;
			widestSortTypeRB = Math.Max(widestSortTypeRB, rbMannerArticulation.Width);
			widestSortTypeRB = Math.Max(widestSortTypeRB, rbUnicodeOrder.Width);

			if (m_showAdvancedOptions)
			{
				int width = 0;

				// First, check which label of "Preceding", "Item" and "Following" is longest.
				width = Math.Max(width, lblBefore.PreferredWidth);
				width = Math.Max(width, lblItem.PreferredWidth);
				width = Math.Max(width, lblAfter.PreferredWidth);

				lblBefore.Width = lblItem.Width = lblAfter.Width = width;

				// Now check which of the column headings is widest
				width = lblFirst.PreferredWidth;
				width = Math.Max(width, lblSecond.PreferredWidth);
				width = Math.Max(width, lblThird.PreferredWidth);
				width = Math.Max(width, lblRL.PreferredWidth);

				lblFirst.Width = lblSecond.Width = lblThird.Width = lblRL.Width = width;
				pnlAdvSort0.Width = pnlAdvSort1.Width = pnlAdvSort2.Width = width;

				tblAdvSorting.Width = tblAdvSorting.PreferredSize.Width;
				grpAdvSortOptions.Width = tblAdvSorting.Width + m_dxAdvGrpTbl;
				tblAdvSorting.Location = m_advTableLocation;
			}

			Width = Math.Max(grpAdvSortOptions.Width, widestSortTypeRB) + 16;
			
			rbPlaceArticulation.Left = rbMannerArticulation.Left =
				rbUnicodeOrder.Left = (Width - widestSortTypeRB) / 2;

			if (m_showAdvancedOptions)
				grpAdvSortOptions.Left = (Width - grpAdvSortOptions.Width) / 2;

			lnkHelp.Top = ClientRectangle.Bottom - lnkHelp.Height - 8;
			lnkHelp.Left = ClientRectangle.Right - lnkHelp.Width - 10;
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
				m_sortOptions = (value ?? new SortOptions(true));

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

				m_chkRL[0].Checked = m_sortOptions.AdvRlOptions[0];
				m_chkRL[1].Checked = m_sortOptions.AdvRlOptions[1];
				m_chkRL[2].Checked = m_sortOptions.AdvRlOptions[2];

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
					grpAdvSortOptions.Enabled = value;
					LayoutControls();
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
					const BindingFlags flags = BindingFlags.Instance |
						BindingFlags.InvokeMethod | BindingFlags.Public;
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
			
			App.ShowHelpTopic((m_showAdvancedOptions ?
				"hidAdvancedPhoneticSortOptions" : "hidBasicPhoneticSortOptions"));
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Send the changed message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SendChangedEvent()
		{
			if (SortOptionsChanged == null)
				return;
			
			if (m_makePhoneticPrimarySortFieldOnChange)
			{
				m_sortOptions.SetPrimarySortField(
				App.Fields.Single(f => f.Type == FieldType.Phonetic), false);
			}

			SortOptionsChanged(m_sortOptions);
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
				m_sortOptions.AdvRlOptions[i] = m_chkRL[i].Checked;

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

			if (ctrl != null)
			{
				if (ctrl is RadioButton)
				{
					ctrl.Tag = 0;
					ctrl.Parent.Invalidate();
				}
				else
					ctrl.Invalidate();
			}
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
			var ctrl = sender as Control;

			if (ctrl != null && (!ctrl.Focused || ctrl.Tag != null))
			{
				ctrl.Tag = null;
				return;
			}

			Rectangle rc;
			var g = e.Graphics;

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
