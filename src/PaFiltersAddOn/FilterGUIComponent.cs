using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Controls;
using System.Drawing.Drawing2D;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.FiltersAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FilterGUIComponent : IDisposable
	{
		private Form m_form;
		private StatusStrip m_statusStrip = null;
		private ToolStripStatusLabel m_statusLbl = null;
		private MenuStrip m_menuStrip = null;
		private ToolStripDropDownButton m_filterButton = null;
		private ToolStripSeparator m_separator = null;
		private FiltersDropDownCtrl m_dropDownContent = null;
		private ITMAdapter m_adapter = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FilterGUIComponent(Form frm)
		{
			m_form = frm;

			if (frm is PaMainWnd)
				m_adapter = PaApp.TMAdapter;
			else if (frm is UndockedViewWnd)
				m_adapter = ReflectionHelper.GetField(frm, "m_mainMenuAdapter") as ITMAdapter;

			SetupFilterToolbarButton();
			SetupFilterStatusBarLabel();
			SetupFilterMenu();
		}

		#region IDisposable Members
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			m_form = null;
			m_adapter = null;
			m_menuStrip = null;
			m_statusStrip = null;

			if (m_statusLbl != null)
			{
				m_statusLbl.Dispose();
				m_statusLbl = null;
			}

			if (m_filterButton != null)
			{
				m_filterButton.Dispose();
				m_filterButton = null;
			}

			if (m_separator != null)
			{
				m_separator.Dispose();
				m_separator = null;
			}

			if (m_dropDownContent != null)
			{
				m_dropDownContent.Dispose();
				m_dropDownContent = null;
			}
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripStatusLabel FilterStatusStripLabel
		{
			get { return m_statusLbl; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ToolStripDropDownButton FilterToolBarButton
		{
			get { return m_filterButton; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public StatusStrip StatusStrip
		{
			get { return m_statusStrip; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FiltersDropDownCtrl DropDownCtrl
		{
			get { return m_dropDownContent; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupFilterToolbarButton()
		{
			try
			{
				// Find the form's main menu strip because we're going to put the filter
				// button and separator on the far right, to the left of the options button.
				foreach (Control ctrl in m_form.Controls)
				{
					if (ctrl.GetType() == typeof(MenuStrip))
					{
						m_menuStrip = ctrl as MenuStrip;
						break;
					}
				}

				if (m_menuStrip == null)
					return;

				// Add the separator that's between the options button and the filter button.
				m_separator = new ToolStripSeparator();
				m_separator.Alignment = ToolStripItemAlignment.Right;
				m_separator.Margin = new Padding(0);
				m_menuStrip.Items.Add(m_separator);

				//m_dropDownContent = new FiltersDropDownCtrl();
				m_filterButton = new ToolStripDropDownButton(Properties.Resources.kimidFilter);
				//m_filterButton.DropDown = m_dropDownContent.HostingDropDown;
				m_filterButton.Margin = new Padding(0);
				m_filterButton.Alignment = ToolStripItemAlignment.Right;
				m_filterButton.DropDown.ItemClicked += HandleFilterButtonItemClicked;
				m_filterButton.DropDownOpening += m_filterButton_DropDownOpening;
				m_menuStrip.Items.Add(m_filterButton);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupFilterStatusBarLabel()
		{
			try
			{
				if (m_filterButton == null)
					return;

				if (m_form is PaMainWnd)
					m_statusStrip = ((PaMainWnd)m_form).statusStrip;
				else if (m_form is IUndockedViewWnd)
					m_statusStrip = ((IUndockedViewWnd)m_form).StatusBar;

				if (m_statusStrip == null)
					return;

				m_statusLbl = new ToolStripStatusLabel();
				Padding margin = m_statusLbl.Margin;
				margin.Right = 3;
				m_statusLbl.Margin = margin;
				m_statusLbl.Visible = false;
				m_statusLbl.AutoSize = false;
				m_statusLbl.Width = Math.Min(175, m_statusStrip.Width / 3);
				m_statusLbl.Paint += HandleStatusLabelPaint;
				m_statusStrip.Items.Add(m_statusLbl);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetupFilterMenu()
		{
			try
			{
				if (m_adapter == null)
					return;

				m_adapter.AddCommandItem("CmdFilters", "DisplayFilterDlg",
					Properties.Resources.kstidFiltersMenuText, null, null, null, null,
					null, Keys.None, null, Properties.Resources.kimidDefineFilters);

				TMItemProperties itemProps = new TMItemProperties();
				itemProps.CommandId = "CmdFilters";
				itemProps.Name = "mnuFilters";
				itemProps.Text = null;
				m_adapter.AddMenuItem(itemProps, "mnuFile", "mnuUndefinedCharacters");
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleStatusLabelPaint(object sender, PaintEventArgs e)
		{
			Rectangle rc = m_statusLbl.ContentRectangle;

			// Fill in shaded background
			using (LinearGradientBrush br = new LinearGradientBrush(rc,
				Color.Gold, Color.Khaki, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			// Draw side borders
			using (Pen pen = new Pen(Color.Goldenrod))
			{
				e.Graphics.DrawLine(pen, 0, 0, 0, rc.Height);
				e.Graphics.DrawLine(pen, rc.Width - 1, 0, rc.Width - 1, rc.Height);
			}

			// Draw little filter image
			Image img = Properties.Resources.kimidFilterSmall;
			rc = m_statusLbl.ContentRectangle;
			Rectangle rcImage = new Rectangle(0, 0, img.Width, img.Height);
			rcImage.X = 3;
			rcImage.Y = (int)(Math.Ceiling(((decimal)rc.Height - rcImage.Height) / 2));
			e.Graphics.DrawImageUnscaledAndClipped(img, rcImage);

			// Draw text
			rc.X = rcImage.Width + 4;
			rc.Width -= rc.X;
			TextFormatFlags flags = TextFormatFlags.EndEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;
			TextRenderer.DrawText(e.Graphics, m_statusLbl.Text, m_statusLbl.Font, rc, Color.Black, flags);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFilterList()
		{
			//m_dropDownContent.RefreshFilterList();

			m_filterButton.DropDown.Items.Clear();
			m_filterButton.DropDown.Items.Add(Properties.Resources.kstidNoFilterText);
			m_filterButton.DropDown.Items.Add(new ToolStripSeparator());

			PaFiltersList filterList = FilterHelper.FilterList;
			if (filterList == null || filterList.Count == 0)
				return;

			foreach (PaFilter filter in filterList)
			{
				if (filter.ShowInToolbarList)
				{
					ToolStripMenuItem item = new ToolStripMenuItem(filter.Name);
					item.Tag = filter;
					m_filterButton.DropDown.Items.Add(item);
				}
			}

			m_filterButton.DropDown.Items.Add(new ToolStripSeparator());
			m_filterButton.DropDown.Items.Add(Properties.Resources.kstidFiltersMenuText,
				Properties.Resources.kimidDefineFilters);
			
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleFilterButtonItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			ToolStripMenuItem item = e.ClickedItem as ToolStripMenuItem;
			if (item == null)
				return;

			if (item.Tag != null && item.Tag.GetType() == typeof(PaFilter))
				FilterHelper.ApplyFilter(item.Tag as PaFilter);
			else if (item.Text == Properties.Resources.kstidNoFilterText)
				FilterHelper.ApplyFilter(null);
			else if (item.Text == Properties.Resources.kstidFiltersMenuText)
			{
				string filterName = null;
				foreach (ToolStripItem mnu in m_filterButton.DropDown.Items)
				{
					PaFilter filter = mnu.Tag as PaFilter;
					if (filter != null && ((ToolStripMenuItem)mnu).Checked)
					{
						filterName = filter.Name;
						break;
					}
				}

				using (DefineFiltersDlg dlg = new DefineFiltersDlg(filterName))
					dlg.ShowDialog();

				return;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_filterButton_DropDownOpening(object sender, EventArgs e)
		{
			PaFilter currFilter = FilterHelper.CurrentFilter;
			for (int i = 0; i < m_filterButton.DropDown.Items.Count; i++)
			{
				ToolStripMenuItem item = m_filterButton.DropDown.Items[i] as ToolStripMenuItem;
				if (item != null)
				{
					item.Checked = (currFilter == item.Tag &&
						item.Text != Properties.Resources.kstidFiltersMenuText);
				}
			}
		}
	}

	#region FilterDropDownButton class (currently not used)
	///// ----------------------------------------------------------------------------------------
	///// <summary>
	///// 
	///// </summary>
	///// ----------------------------------------------------------------------------------------
	//public class FilterDropDownButton : ToolStripSplitButton
	//{
	//    private bool m_checked = false;

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// 
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    public FilterDropDownButton(Image img) : base(img)
	//    {
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// 
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    public bool Checked
	//    {
	//        get { return m_checked; }
	//        set
	//        {
	//            m_checked = value;
	//            Invalidate();
	//        }
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// 
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    protected override void OnPaint(PaintEventArgs e)
	//    {
	//        base.OnPaint(e);

	//        if (!m_checked || this.Selected)
	//            return;

	//        Rectangle rc = this.ButtonBounds;

	//        // Draw the background
	//        using (LinearGradientBrush br = new LinearGradientBrush(rc,
	//            ProfessionalColors.ButtonCheckedGradientBegin,
	//            ProfessionalColors.ButtonCheckedGradientEnd, LinearGradientMode.Vertical))
	//        {
	//            e.Graphics.FillRectangle(br, rc);
	//        }

	//        // Draw the border
	//        rc.Height--;
	//        using (Pen pen = new Pen(ProfessionalColors.ButtonPressedBorder))
	//            e.Graphics.DrawRectangle(pen, rc);

	//        // Draw the  image
	//        rc = ButtonBounds;
	//        while (rc.Width > Image.Width)
	//            rc.Inflate(-1, 0);

	//        while (rc.Height > Image.Height)
	//            rc.Inflate(0, -1);

	//        e.Graphics.DrawImage(Image, rc);
	//    }
	//}

	#endregion
}
