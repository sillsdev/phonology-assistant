using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	#region FwDataSourcePropertiesDlg class
	/// ----------------------------------------------------------------------------------------
	public partial class Fw7DataSourcePropertiesDlg : OKCancelDlgBase
	{
		private readonly PaProject m_project;
		private readonly List<FwWritingSysInfo> m_wsInfo;
		private readonly List<string> m_allWsNames;

		private IEnumerable<PaField> m_potentialFields;
		private readonly PaDataSource m_datasource;
		private Fw7FieldMappingGrid m_fieldGrid;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		public Fw7DataSourcePropertiesDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public Fw7DataSourcePropertiesDlg(PaDataSource ds) : this()
		{
			if (App.DesignMode)
				return;

			// Merge the project's mapped fields with the default set.
			m_potentialFields = PaField.GetDefaultFw7Fields().Where(f => f.AllowUserToMap);

			lblProjectValue.Text = ds.FwDataSourceInfo.ToString();
			lblProject.Font = FontHelper.UIFont;
			lblProjectValue.Font = FontHelper.UIFont;
			grpWritingSystems.Font = FontHelper.UIFont;
			grpPhoneticDataStoreType.Font = FontHelper.UIFont;
			rbLexForm.Font = FontHelper.UIFont;
			rbPronunField.Font = FontHelper.UIFont;

			InitializeGrid(ds);

			rbLexForm.Checked = 
				(ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm);

			rbPronunField.Checked =
				(ds.FwDataSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.PronunciationField);

			m_dirty = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load up the grid with PA field names and the FW writing systems assigned to them,
		/// if any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(PaDataSource ds)
		{
			m_fieldGrid = new Fw7FieldMappingGrid(ds.FwDataSourceInfo, m_potentialFields);
			m_fieldGrid.Dock = DockStyle.Fill;
			pnlGrid.Controls.Add(m_fieldGrid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We need to intercept things here so we can modify the contents of the combo. before
		/// the user drops-down its list. When the grid's writing system column is created, the
		/// entire list of analysis and vernacular writing systems are sent to its Items
		/// collection so the grid will consider any of those writing systems as valid choices.
		/// However, since some fields in the grid can only be assigned analysis writing
		/// systems and others only vernacular, we need to modify the grid's combo's list just
		/// before it's shown to only include the proper subset of writing systems for the
		/// current row's field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void m_grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			//var cbo = e.Control as ComboBox;
			//if (cbo == null || m_grid.CurrentRow == null)
			//    return;

			//// Get the writing system type (analysis or vernacular) for the current row.
			//var wsType = (FwDBUtils.FwWritingSystemType)m_grid.CurrentRow.Cells[kWsTypeCol].Value;
			
			//// Get the writing system information to which the current row is set.
			//var currWsInfo = m_grid.CurrentRow.Tag as FwDataSourceWsInfo;
		
			//// Clear the combo list and add the '(none)' options first.
			//cbo.Items.Clear();
			//cbo.Items.Add(m_wsInfo[0]);

			//// Now iterate through the writing systems and only add ones to the list
			//// whose type is the same as the type of writing system for the current row.
			//foreach (var wsInfo in m_wsInfo)
			//{
			//    if (wsInfo.WsType == wsType)
			//        cbo.Items.Add(wsInfo);

			//    if (currWsInfo.Ws == wsInfo.WsNumber)
			//        cbo.SelectedItem = wsInfo;
			//}

			//// This should never happend, but if, by this point, the combo's
			//// selected item hasn't been set, set it to the first item in the list.
			//if (cbo.SelectedIndex < 0)
			//    cbo.SelectedIndex = 0;

			//cbo.SelectionChangeCommitted += cbo_SelectionChangeCommitted;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// After the user has selected a writing system for the field, save the info.
		/// for the selected one in the row's tag property so it can be retrieved when
		/// the dialog's settings are saved when it's closed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void cbo_SelectionChangeCommitted(object sender, EventArgs e)
		{
			//var cbo = sender as ComboBox;
			//if (cbo == null || cbo.SelectedItem == null || m_grid.CurrentRow == null)
			//    return;

			//var currWsInfo = m_grid.CurrentRow.Tag as FwDataSourceWsInfo;
			//var pickedWs = cbo.SelectedItem as FwWritingSysInfo;

			//if (currWsInfo != null && pickedWs != null)
			//    currWsInfo.Ws = pickedWs.WsNumber;
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// This will make sure that only writing system cells gets focus since they are the
		///// only one the user may change. I hate using SendKeys, but setting CurrentCell
		///// causing a reentrant error.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private void m_grid_CellEnter(object sender, DataGridViewCellEventArgs e)
		//{
		//    if (e.ColumnIndex == 0)
		//        SendKeys.Send("{TAB}");
		//}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not values on the dialog changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return base.IsDirty || m_fieldGrid.IsDirty;}
		}

		#endregion

		#region Event Handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePhoneticStorageTypeCheckedChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		#endregion

		#region Methods for verifying changes and saving them before closing
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
//			Settings.Default.FwDataSourcePropertiesDlgGrid = GridSettings.Create(m_grid);
			base.SaveSettings();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that a writing system has been specified for at least one field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			return true;
			//if ((from DataGridViewRow row in m_grid.Rows select row.Tag as FwDataSourceWsInfo)
			//    .Any(wsInfo => wsInfo != null && wsInfo.Ws > 0))
			//{
			//    return true;
			//}

			//var msg = App.LocalizeString("FwDataSourcePropertiesDlg.MissingWritingSystemMsg",
			//    "You must specify a writing system for at least one field.",
			//    "Message displayed in the FieldWorks data source dialog when the user clicks OK when no field has been assigned a writing system.",
			//    App.kLocalizationGroupDialogs);

			//Utils.MsgBox(msg);
			//return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool SaveChanges()
		{
			try
			{
				//var wsInfoList = (from DataGridViewRow row in m_grid.Rows select row.Tag)
				//    .OfType<FwDataSourceWsInfo>().ToList();

				//m_dsInfo.WritingSystemInfo = wsInfoList;
				//m_dsInfo.PhoneticStorageMethod = (rbLexForm.Checked ?
				//    FwDBUtils.PhoneticStorageMethod.LexemeForm :
				//    FwDBUtils.PhoneticStorageMethod.PronunciationField);
			}
			catch
			{
				return false;
			}

			return true;
		}

		#endregion
	}

	#endregion
}