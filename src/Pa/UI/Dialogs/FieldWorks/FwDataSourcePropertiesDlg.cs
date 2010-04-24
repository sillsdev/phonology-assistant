using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilUtils;

namespace SIL.Pa.UI.Dialogs
{
	#region FwDataSourcePropertiesDlg class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FwDataSourcePropertiesDlg : OKCancelDlgBase
	{
		private const string kFieldCol = "field";
		private const string kWsNameCol = "wsname";
		private const string kWsTypeCol = "wstype";

		private readonly PaProject m_project;
		private readonly FwDataSourceInfo m_fwSourceInfo;
		private List<FwWritingSysInfo> m_wsInfo;
		private List<string> m_allWsNames;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg(PaProject project, FwDataSourceInfo fwSourceInfo) : this()
		{
			Debug.Assert(project != null);
			Debug.Assert(project.FieldInfo != null);
			Debug.Assert(fwSourceInfo != null);

			m_project = project;
			m_fwSourceInfo = fwSourceInfo;

			lblProjectValue.Text = m_fwSourceInfo.ToString();

			SetControlFonts();
			GetWritingSystems();
			BuildGrid();
			LoadGrid();

			rbLexForm.Checked = 
				(m_fwSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm);

			rbPronunField.Checked =
				(m_fwSourceInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.PronunciationField);

			App.SettingsHandler.LoadFormProperties(this);
			m_dirty = false;

			// This is annoying to have to do this, but setting the tab order in the
			// designer doesn't seem to work. Therefore, I am forcing the inherited
			// stuff to be last in the tab order.
			tblLayoutButtons.TabIndex = 100;
			btnOK.TabIndex = 101;
			btnCancel.TabIndex = 102;
			btnHelp.TabIndex = 103;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add columns to fonts grid
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SetControlFonts()
		{
			lblProject.Font = FontHelper.UIFont;
			lblProjectValue.Font = FontHelper.UIFont;
			grpWritingSystems.Font = FontHelper.UIFont;
			grpPhoneticDataStoreType.Font = FontHelper.UIFont;
			rbLexForm.Font = FontHelper.UIFont;
			rbPronunField.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid.Name = Name + "Grid";
			m_grid.AutoGenerateColumns = false;
			m_grid.Font = FontHelper.UIFont;
			m_grid.EditingControlShowing += m_grid_EditingControlShowing;

			// Add a column for the field name.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(kFieldCol);
			col.HeaderText = Properties.Resources.kstidFwWsFieldHdg;
			col.ReadOnly = true;
			col.Width = 150;
			m_grid.Columns.Add(col);

			// Add a column for the writing system name (or 'none').
			col = SilGrid.CreateDropDownListComboBoxColumn(kWsNameCol, m_allWsNames);
			col.HeaderText = Properties.Resources.kstidFwWsWsHdg;
			col.Width = 110;
			m_grid.Columns.Add(col);

			// Add a hidden column to store the writing system
			// type (i.e. vernacular or analysis) for the field.
			col = SilGrid.CreateTextBoxColumn(kWsTypeCol);
			col.Visible = false;
			m_grid.Columns.Add(col);

			m_grid.AutoResizeColumnHeadersHeight();
			App.SettingsHandler.LoadGridProperties(m_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Load up the grid with PA field names and the FW writing systems assigned to them,
		/// if any.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadGrid()
		{
			m_grid.Rows.Clear();

			// Go through each PA field and find the fields marked as FW fields that
			// are marked with either the vernacular or analysis writing system.
			foreach (PaFieldInfo fieldInfo in m_project.FieldInfo.SortedList)
			{
				if (fieldInfo.FwWritingSystemType != FwDBUtils.FwWritingSystemType.None)
				{
					// Add the field to the list and, for now, assume
					// it has no writing system assigned.
					m_grid.Rows.Add(new object[] { fieldInfo.DisplayText,
						Properties.Resources.kstidFwWsNoneSpecified,
						fieldInfo.FwWritingSystemType });

					// Get the row just added. Each row's tag property references an object
					// indicating what writing system is currently assigned to the row's field.
					// For now, assign all new rows no writing system (i.e. 'none').
					DataGridViewRow newRow = m_grid.Rows[m_grid.RowCount - 1];
					newRow.Tag = new FwDataSourceWsInfo(fieldInfo.FwQueryFieldName, 0);

					if (m_fwSourceInfo.WritingSystemInfo == null)
						continue;

					// Now go through the list of fields that have been assigned
					// writing systems and find the one corresponding to the row just
					// added. Then change the row's tag property to reference that
					// writing system.
					foreach (FwDataSourceWsInfo dswsi in m_fwSourceInfo.WritingSystemInfo)
					{
						if (fieldInfo.FwQueryFieldName == dswsi.FieldName)
						{
							newRow.Tag = dswsi.Clone();

							foreach (FwWritingSysInfo wsi in m_wsInfo)
							{
								if (wsi.WsNumber == dswsi.Ws)
									newRow.Cells[kWsNameCol].Value = wsi.WsName;
							}
						}
					}
				}
			}
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
			ComboBox cbo = e.Control as ComboBox;
			if (cbo == null || m_grid.CurrentRow == null)
				return;

			// Get the writing system type (analysis or vernacular) for the current row.
			FwDBUtils.FwWritingSystemType wsType =
				(FwDBUtils.FwWritingSystemType)m_grid.CurrentRow.Cells[kWsTypeCol].Value;
			
			// Get the writing system information to which the current row is set.
			FwDataSourceWsInfo currWsInfo = m_grid.CurrentRow.Tag as FwDataSourceWsInfo;
		
			// Clear the combo list and add the '(none)' options first.
			cbo.Items.Clear();
			cbo.Items.Add(m_wsInfo[0]);

			// Now iterate through the writing systems and only add ones to the list
			// whose type is the same as the type of writing system for the current row.
			foreach (FwWritingSysInfo wsInfo in m_wsInfo)
			{
				if (wsInfo.WsType == wsType)
					cbo.Items.Add(wsInfo);

				if (currWsInfo.Ws == wsInfo.WsNumber)
					cbo.SelectedItem = wsInfo;
			}

			// This should never happend, but if, by this point, the combo's
			// selected item hasn't been set, set it to the first item in the list.
			if (cbo.SelectedIndex < 0)
				cbo.SelectedIndex = 0;

			cbo.SelectionChangeCommitted += cbo_SelectionChangeCommitted;
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
			ComboBox cbo = sender as ComboBox;
			if (cbo == null || cbo.SelectedItem == null || m_grid.CurrentRow == null)
				return;

			FwDataSourceWsInfo currWsInfo = m_grid.CurrentRow.Tag as FwDataSourceWsInfo;
			FwWritingSysInfo pickedWs = cbo.SelectedItem as FwWritingSysInfo;

			if (currWsInfo != null && pickedWs != null)
				currWsInfo.Ws = pickedWs.WsNumber;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will make sure that only writing system cells gets focus since they are the
		/// only one the user may change. I hate using SendKeys, but setting CurrentCell
		/// causing a reentrant error.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_grid_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
				SendKeys.Send("{TAB}");
		}

		#endregion

		#region Writing System Combos Setup
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the writing systems from the database into two lists. One contains objects
		/// the represent all the necessary writing system information and the other contains
		/// just a list of all the writing system names. Both lists contain all the analysis
		/// and vernacular writing systems.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void GetWritingSystems()
		{
			// Keep a list of all the writing system names, whether vernacular or analysis.
			m_allWsNames = new List<string>();
			FwDataReader reader = new FwDataReader(m_fwSourceInfo);

			m_wsInfo = reader.AllWritingSystems;

			// Add a (none) option.
			m_wsInfo.Insert(0, new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.None, 0,
				Properties.Resources.kstidFwWsNoneSpecified));

			foreach (FwWritingSysInfo wsInfo in m_wsInfo)
				m_allWsNames.Add(wsInfo.WsName);
		}

		#endregion

		#region Overridden methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not values on the dialog changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool IsDirty
		{
			get	{return base.IsDirty || m_grid.IsDirty;}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For some reason, this needs to be done here or we end up in the second row as
		/// a result of code in the m_grid_CellEnter event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			if (m_grid.Rows.Count > 0)
				m_grid.CurrentCell = m_grid[kWsNameCol, 0];
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			base.SaveSettings();
			App.SettingsHandler.SaveFormProperties(this);
			App.SettingsHandler.SaveGridProperties(m_grid);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that a writing system has been specified for at least one field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			foreach (DataGridViewRow row in m_grid.Rows)
			{
				FwDataSourceWsInfo wsInfo = row.Tag as FwDataSourceWsInfo;
				if (wsInfo != null && wsInfo.Ws > 0)
					return true;
			}

			Utils.MsgBox(Properties.Resources.kstidFwMissingWsMsg);
			return false;
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
				List<FwDataSourceWsInfo> wsInfoList = new List<FwDataSourceWsInfo>();

				foreach (DataGridViewRow row in m_grid.Rows)
				{
					FwDataSourceWsInfo wsInfo = row.Tag as FwDataSourceWsInfo;
					if (wsInfo != null)
						wsInfoList.Add(wsInfo);
				}

				m_fwSourceInfo.WritingSystemInfo = wsInfoList;

				m_fwSourceInfo.PhoneticStorageMethod = (rbLexForm.Checked ?
					FwDBUtils.PhoneticStorageMethod.LexemeForm :
					FwDBUtils.PhoneticStorageMethod.PronunciationField);
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