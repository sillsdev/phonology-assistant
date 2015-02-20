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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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

		private bool m_showButtons = true;
		private bool m_showAdvancedOptions;
		private SortOptions m_sortOptions;
		private readonly int[] m_checkedIndexes;
		private readonly RadioButton[] m_rbAdvSort0;
		private readonly RadioButton[] m_rbAdvSort1;
		private readonly RadioButton[] m_rbAdvSort2;
		private readonly CheckBox[] m_chkRL;

		#region Constructor and Loading
		/// ------------------------------------------------------------------------------------
		public SortOptionsDropDown()
		{
			MakePhoneticPrimarySortFieldWhenOptionsChange = true;
			DrawWithGradientBackground = true;
			DoubleBuffered = true;

			InitializeComponent();

			SetUiFonts();

			m_sortOptions = new SortOptions(true, App.Project);

			m_rbAdvSort0 = new[] { rbBefore1st, rbItem1st, rbAfter1st };
			m_rbAdvSort1 = new[] { rbBefore2nd, rbItem2nd, rbAfter2nd };
			m_rbAdvSort2 = new[] { rbBefore3rd, rbItem3rd, rbAfter3rd };
			m_chkRL = new[] { chkBeforeRL, chkItemRL, chkAfterRL };

			// Keeps track of selected advanced sorting radio buttons
			m_checkedIndexes = new int[3];
			m_checkedIndexes[0] = m_sortOptions.AdvSortOrder[0];
			m_checkedIndexes[1] = m_sortOptions.AdvSortOrder[1];
			m_checkedIndexes[2] = m_sortOptions.AdvSortOrder[2];

			SetupStaticEventSubscriptions(m_rbAdvSort0);
			SetupStaticEventSubscriptions(m_rbAdvSort1);
			SetupStaticEventSubscriptions(m_rbAdvSort2);
			SetupStaticEventSubscriptions(m_chkRL);
		}

		/// ------------------------------------------------------------------------------------
		private static void SetupStaticEventSubscriptions(IEnumerable<Control> ctrlArray)
		{
			foreach (var ctrl in ctrlArray)
			{
				ctrl.Paint += HandleAdvancedOptionItemPaint;
				ctrl.Enter += HandleAdvancedOptionItemEnter;
				ctrl.Leave += HandleAdvancedOptionItemLeave;
			}
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
			rbMannerArticulation.Font = FontHelper.UIFont;
			rbPlaceArticulation.Font = FontHelper.UIFont;
			lblBefore.Font = FontHelper.UIFont;
			lblItem.Font = FontHelper.UIFont;
			lblAfter.Font = FontHelper.UIFont;
			lblFirst.Font = FontHelper.UIFont;
			lblSecond.Font = FontHelper.UIFont;
			lblThird.Font = FontHelper.UIFont;
			lblRL.Font = FontHelper.UIFont;
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SortOptions SortOptions
		{
			get { return m_sortOptions; }
			set 
			{
				m_sortOptions = (value ?? new SortOptions(true, App.Project));

				if (m_sortOptions.SortType == PhoneticSortType.MOA)
					rbMannerArticulation.Checked = true;
				else
					rbPlaceArticulation.Checked = true;

				UpdateCheckedIndexes();

				m_rbAdvSort0[m_checkedIndexes[0]].Checked = true;
				m_rbAdvSort1[m_checkedIndexes[1]].Checked = true;
				m_rbAdvSort2[m_checkedIndexes[2]].Checked = true;

				m_chkRL[0].Checked = m_sortOptions.AdvRlOptions[0];
				m_chkRL[1].Checked = m_sortOptions.AdvRlOptions[1];
				m_chkRL[2].Checked = m_sortOptions.AdvRlOptions[2];

				AdvancedOptionsEnabled = m_sortOptions.AdvancedEnabled;
				tblAdvSorting.Enabled = m_sortOptions.AdvancedEnabled;
				pnlAdvOptions.Visible = m_showAdvancedOptions;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool MakePhoneticPrimarySortFieldWhenOptionsChange { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool DrawWithGradientBackground { get; set; }

		/// ------------------------------------------------------------------------------------
		public bool ShowAdvancedOptions
		{
			get { return m_showAdvancedOptions; }
			set
			{
				m_showAdvancedOptions = value;
				pnlAdvOptions.Visible = value;
				pnlAdvOptions.Enabled = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool ShowButtons
		{
			get { return m_showButtons; }
			set
			{
				m_showButtons = value;
				btnHelp.Visible = value;
				btnClose.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool AdvancedOptionsEnabled
		{
			get { return pnlAdvOptions.Enabled; }
			set { pnlAdvOptions.Enabled = value;}
		}

		#endregion

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
		private void HandleOuterTableLayoutSizeChanged(object sender, EventArgs e)
		{
			Size = tblLayoutOuter.Size;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHelpButtonClick(object sender, EventArgs e)
		{
			Close();

			App.ShowHelpTopic((m_showAdvancedOptions ?
				"hidAdvancedPhoneticSortOptions" : "hidBasicPhoneticSortOptions"));
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCloseButtonClick(object sender, EventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void SendChangedEvent()
		{
			if (SortOptionsChanged == null)
				return;
			
			if (MakePhoneticPrimarySortFieldWhenOptionsChange)
				m_sortOptions.SetPrimarySortField(App.Project.GetPhoneticField(), false);

			SortOptionsChanged(m_sortOptions);
		}

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
			if (rbMannerArticulation.Checked && m_sortOptions.SortType != PhoneticSortType.MOA)
			{
				m_sortOptions.SortType = PhoneticSortType.MOA;
				SendChangedEvent();
			}
			else if (rbPlaceArticulation.Checked && m_sortOptions.SortType != PhoneticSortType.POA)
			{
				m_sortOptions.SortType = PhoneticSortType.POA;
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
		private static void HandleAdvancedOptionItemLeave(object sender, EventArgs e)
		{
			var ctrl = sender as Control;

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
		private static void HandleAdvancedOptionItemEnter(object sender, EventArgs e)
		{
			(sender as Control).Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the 'paint' event for the Right/Left checkboxes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void HandleAdvancedOptionItemPaint(object sender, PaintEventArgs e)
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


		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (DrawWithGradientBackground)
			{
				var clr1 = Color.White;
				var clr2 = ColorHelper.CalculateColor(Color.White, SystemColors.GradientActiveCaption, 100);
				var rc = ClientRectangle;
				rc.Inflate(-1, -1);

				using (var br = new LinearGradientBrush(rc, clr1, clr2, 45f))
					e.Graphics.FillRectangle(br, rc);
			}
		}

		#endregion
	}
}
