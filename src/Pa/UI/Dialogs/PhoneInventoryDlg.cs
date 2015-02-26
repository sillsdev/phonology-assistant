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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.FieldWorks.Common.UIAdapters;
using SIL.Pa.Model;
using SIL.Pa.UI.Controls;
using SIL.Pa.UI.Dialogs;
using SilUtils;

namespace SIL.Pa
{
	public partial class PhoneInventoryDlg : OKCancelDlgBase
	{
		private SilGrid m_grid;
		private SizableDropDownPanel m_sddpFeatures;
		private CustomDropDown m_dropdown;
		private FeatureListView m_lvFeatures;
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PhoneInventoryDlg()
		{
			InitializeComponent();
			BuildGrid();
			LoadGrid();

			m_sddpFeatures = new SizableDropDownPanel(Name + "FeatureDropDown",
				new Size(m_grid.Columns["features"].Width, 100));
			m_sddpFeatures.BorderStyle = BorderStyle.FixedSingle;
			m_sddpFeatures.Padding = new Padding(7, 7, 7, m_sddpFeatures.Padding.Bottom);

			m_lvFeatures = new FeatureListView(PaApp.FeatureType.Articulatory);
			m_lvFeatures.Dock = DockStyle.Fill;
			m_lvFeatures.Load();
			m_lvFeatures.FeatureChanged +=
				new FeatureListView.FeatureChangedHandler(m_lvFeatures_FeatureChanged);
			m_sddpFeatures.Controls.Add(m_lvFeatures);

			m_dropdown = new CustomDropDown();
			m_dropdown.AutoCloseWhenMouseLeaves = false;
			m_dropdown.AddControl(m_sddpFeatures);
			m_dropdown.Closing += m_dropdown_Closing;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid = new SilGrid();
			m_grid.Name = Name + "Grid";
			m_grid.AutoGenerateColumns = false;
			m_grid.Dock = DockStyle.Fill;
			m_grid.Font = FontHelper.UIFont;
			m_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
			//			m_grid.RowEnter += new DataGridViewCellEventHandler(m_grid_RowEnter);

			DataGridViewColumn col = SilGrid.CreateTextBoxColumn("phone");
			col.ReadOnly = true;
			col.Width = 55;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			col.DefaultCellStyle.Font = FontHelper.PhoneticFont;
			col.CellTemplate.Style.Font = FontHelper.PhoneticFont;
			col.HeaderText = "Phone";
			m_grid.Columns.Add(col);

			col = SilGrid.CreateTextBoxColumn("count");
			col.ReadOnly = true;
			col.Width = 55;
			col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			col.HeaderText = "Count";
			m_grid.Columns.Add(col);

			col = SilGrid.CreateSilButtonColumn("features");
			col.ReadOnly = true;
			col.Width = 300;
			col.HeaderText = "Features";
			((SilButtonColumn)col).ButtonWidth = 23;
			((SilButtonColumn)col).DrawTextWithEllipsisPath = true;
			((SilButtonColumn)col).ButtonFont = new Font("Marlett", 9);
			((SilButtonColumn)col).ButtonText = "6";
			((SilButtonColumn)col).ButtonClicked += HandleFeaturesListClick;
			m_grid.Columns.Add(col);

			PaApp.SettingsHandler.LoadGridProperties(m_grid);
			Controls.Add(m_grid);
			m_grid.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			SortedList<string, KeyValuePair<string, IPhoneInfo>> sortedPhones =
				new SortedList<string, KeyValuePair<string, IPhoneInfo>>();

			// Create a sorted list of phones by place of articulation.
			foreach (KeyValuePair<string, IPhoneInfo> phoneInfo in PaApp.PhoneCache)
			{
				KeyValuePair<string, IPhoneInfo> kvpPhoneInfo =
					new KeyValuePair<string, IPhoneInfo>(phoneInfo.Key, phoneInfo.Value.Clone());

				if (phoneInfo.Key.Trim() != string.Empty)
					sortedPhones[phoneInfo.Value.POAKey] = kvpPhoneInfo;
			}

			// Now fill the grid with the sorted list.
			m_grid.Rows.Add(sortedPhones.Count);

			int i = 0;
			foreach (KeyValuePair<string, IPhoneInfo> phoneInfo in sortedPhones.Values)
			{
				m_grid.Rows[i].Cells[0].Value = phoneInfo.Value;
				m_grid.Rows[i].Cells[1].Value = phoneInfo.Value.TotalCount;
				m_grid.Rows[i++].Cells[2].Value =
					PaApp.AFeatureCache.GetFeaturesText(phoneInfo.Value.Masks);
			}
		}

		#region Saving Settings and Verifying/Saving changes
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			base.SaveSettings();
			PaApp.SettingsHandler.SaveFormProperties(this);
			PaApp.SettingsHandler.SaveGridProperties(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get { return (m_dirty || m_grid.IsDirty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			return true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save the changes in response to closing the dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{

			return true;
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFeaturesListClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0)
				return;

			PhoneInfo phoneInfo = m_grid.Rows[e.RowIndex].Cells["phone"].Value as PhoneInfo;

			if (phoneInfo != null)
			{
				m_lvFeatures.CurrentMasks = phoneInfo.Masks;
				Rectangle rc = m_grid.GetCellDisplayRectangle(0, e.RowIndex, true);
				Point pt = new Point(rc.Left, rc.Bottom);
				pt = m_grid.PointToScreen(pt);
				m_dropdown.Show(pt);
				m_lvFeatures.Focus();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_lvFeatures_FeatureChanged(object sender, ulong[] newMasks)
		{
			if (m_grid.CurrentRow == null)
				return;

			IPhoneInfo phoneInfo = m_grid.CurrentRow.Cells["phone"].Value as IPhoneInfo;
			if (phoneInfo == null)
				return;

			phoneInfo.Masks = m_lvFeatures.CurrentMasks;
			m_grid.CurrentRow.Cells["features"].Value =
				DataUtils.AFeatureCache.GetFeaturesText(phoneInfo.Masks);
		}

		void m_dropdown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
		}
	}
}