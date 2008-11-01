using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIL.SpeechTools.Utils;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Controls;

namespace SIL.Pa.FiltersAddOn
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FiltersDropDownCtrl : UserControl
	{
		private CustomDropDown m_dropDown = null;
		private SizableDropDownPanel m_sizablePanel = null;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FiltersDropDownCtrl()
		{
			InitializeComponent();

			lstFilters.Font = FontHelper.UIFont;
			lblFilters.Font = FontHelper.UIFont;

			Disposed += HandleDisposed;

			// Create a sizable panel that will be hosted on the drop-down and, itself,
			// will host the filter list and the define, apply, cancel links.
			m_sizablePanel = new SizableDropDownPanel("filtersdropdown", new Size(100, 200));
			m_sizablePanel.MinimumSize = new Size(MinimumSize.Width + 12, MinimumSize.Height + 9);
			m_sizablePanel.BorderStyle = BorderStyle.FixedSingle;
			m_sizablePanel.Padding = new Padding(5, 0, 5, 7);

			Dock = DockStyle.Fill;
			m_sizablePanel.Controls.Add(this);
			m_dropDown = new CustomDropDown();
			m_dropDown.AddControl(m_sizablePanel);
			m_dropDown.Opened += m_dropDown_Opened;

			LoadDropDownFilterList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleDisposed(object sender, EventArgs e)
		{
			Disposed -= HandleDisposed;
			m_dropDown.Opened -= m_dropDown_Opened;
			m_sizablePanel.Dispose();
			m_dropDown.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshFilterList()
		{
			LoadDropDownFilterList();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadDropDownFilterList()
		{
			lstFilters.Items.Clear();

			PaFiltersList filterList = FilterHelper.FilterList;
			if (filterList == null || filterList.Count == 0)
				return;

			lstFilters.Items.Add(Properties.Resources.kstidNoFilterText);
			foreach (PaFilter filter in filterList)
				lstFilters.Items.Add(filter);

			lstFilters.SelectedIndex = 0;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal CustomDropDown HostingDropDown
		{
			get { return m_dropDown; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal PaFilter SelectedFilter
		{
			get { return lstFilters.SelectedItem as PaFilter; }
			set
			{
				if (value == null)
					lstFilters.SelectedIndex = 0;
				else
				{
					for (int i = 0; i < lstFilters.Items.Count; i++)
					{
						if (lstFilters.Items[i] == value)
						{
							lstFilters.SelectedIndex = i;
							break;
						}
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_dropDown_Opened(object sender, EventArgs e)
		{
			SelectedFilter = FilterHelper.CurrentFilter;
			lstFilters.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkDefine_Click(object sender, EventArgs e)
		{
			m_dropDown.Close();
			string filterName = (SelectedFilter != null ? SelectedFilter.Name : null);
			using (DefineFiltersDlg dlg = new DefineFiltersDlg(filterName))
				dlg.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkApply_Click(object sender, EventArgs e)
		{
			m_dropDown.Close();

			if (lstFilters.SelectedItem == null)
				return;

			if (lstFilters.SelectedItem is PaFilter)
				((PaFilter)lstFilters.SelectedItem).ApplyFilter();
			else if (lstFilters.SelectedItem.ToString() == Properties.Resources.kstidNoFilterText)
			{
				FilterHelper.Restore();
				FilterHelper.FilterApplied(null);
			}

			PaApp.MsgMediator.SendMessage("FilterApplied", SelectedFilter);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lnkCancel_Click(object sender, EventArgs e)
		{
			m_dropDown.Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstFilters_KeyPress(object sender, KeyPressEventArgs e)
		{
			lnkApply_Click(null, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstFilters_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int i = lstFilters.IndexFromPoint(e.Location);
				if (i >= 0)
					lnkApply_Click(null, EventArgs.Empty);
			}
		}
	}
}