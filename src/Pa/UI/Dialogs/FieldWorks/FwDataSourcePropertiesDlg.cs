using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// ----------------------------------------------------------------------------------------
	public partial class FwDataSourcePropertiesDlg : OKCancelDlgBase
	{
		private const string kFieldCol = "field";
		private const string kWsNameCol = "wsname";
		private const string kWsTypeCol = "wstype";

		private readonly PaProject m_project;
		private readonly FwDataSourceInfo m_fwDsInfo;
		private readonly IEnumerable<FwWritingSysInfo> m_writingSystems;
		private readonly FwWritingSysInfo m_noWritingSystemOption;

		#region Construction and initialization
		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public FwDataSourcePropertiesDlg(PaProject project, FwDataSourceInfo fwSourceInfo) : this()
		{
			Utils.WaitCursors(true);
			m_project = project;
			m_fwDsInfo = fwSourceInfo;

			m_noWritingSystemOption = new FwWritingSysInfo(FwDBUtils.FwWritingSystemType.None, 0, NoWritingSystemText);
			var list = FwDataReader.GetWritingSystems(fwSourceInfo).ToList();
			list.Insert(0, m_noWritingSystemOption);
			m_writingSystems = list;

			// For an FW 7 project, create default mappings from fields
			// to writing systems if it's not been done before.
			if (fwSourceInfo.DataSourceType == DataSource.DataSourceType.FW7 &&
				(fwSourceInfo.WsMappings == null || fwSourceInfo.WsMappings.Count == 0))
			{
				CreateInitialWritingSystemMappings();
			}

			lblProjectValue.Text = m_fwDsInfo.ToString();

			lblProject.Font = FontHelper.UIFont;
			lblProjectValue.Font = FontHelper.UIFont;
			grpWritingSystems.Font = FontHelper.UIFont;
			grpPhoneticDataStoreType.Font = FontHelper.UIFont;
			rbLexForm.Font = FontHelper.UIFont;
			rbPronunField.Font = FontHelper.UIFont;

			BuildGrid();
			LoadGrid();

			rbLexForm.Checked =
				(m_fwDsInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.LexemeForm);

			rbPronunField.Checked =
				(m_fwDsInfo.PhoneticStorageMethod == FwDBUtils.PhoneticStorageMethod.PronunciationField);

			m_dirty = false;
			m_grid.IsDirty = false;
			Utils.WaitCursors(false);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateInitialWritingSystemMappings()
		{
			m_fwDsInfo.WsMappings = new List<FwFieldWsMapping>(5);

			var defaultVernWs = m_writingSystems.SingleOrDefault(ws => ws.IsDefaultVernacular);
			var defaultAnalWs = m_writingSystems.SingleOrDefault(ws => ws.IsDefaultAnalysis);

			//m_fwDsInfo.WsMappings = (m_project.FieldInfo
			//    .Where(fi => fi.FwWritingSystemType != FwDBUtils.FwWritingSystemType.None)
			//    .Select(fi => new FwFieldWsMapping(fi.FwQueryFieldName,
			//        fi.FwWritingSystemType == FwDBUtils.FwWritingSystemType.Vernacular ?
			//            defaultVernWs : defaultAnalWs))).ToList();
		}

		/// ------------------------------------------------------------------------------------
		private void BuildGrid()
		{
			m_grid.Name = Name + "Grid";
			m_grid.AutoGenerateColumns = false;
			m_grid.Font = FontHelper.UIFont;
			m_grid.EditingControlShowing += HandleGridEditingControlShowing;
			App.SetGridSelectionColors(m_grid, false);

			// Add a column for the field name.
			DataGridViewColumn col = SilGrid.CreateTextBoxColumn(kFieldCol);
			col.ReadOnly = true;
			col.Width = 150;
			m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns[kFieldCol],
				"Fw6DataSourcePropertiesDlg.FieldNameColumnHdg", "Field",
				"Heading for the field column of the writing system grid on FieldWorks data source properties dialog.",
				App.kLocalizationGroupDialogs);

			// Add a column for the writing system name (or 'none').
			col = SilGrid.CreateDropDownListComboBoxColumn(kWsNameCol, m_writingSystems.Select(ws => ws.Name));
			col.Width = 110;
			m_grid.Columns.Add(col);
			App.LocalizeObject(m_grid.Columns[kWsNameCol],
				"Fw6DataSourcePropertiesDlg.WritingSystemColumnHdg", "Writing System",
				"Heading for the writing system column of the writing system grid on the FieldWorks data source properties dialog.",
				App.kLocalizationGroupDialogs);

			// Add a hidden column to store the writing system
			// type (i.e. vernacular or analysis) for the field.
			col = SilGrid.CreateTextBoxColumn(kWsTypeCol);
			col.Visible = false;
			m_grid.Columns.Add(col);

			m_grid.AutoResizeColumnHeadersHeight();

			if (Settings.Default.FwDataSourcePropertiesDlgGrid != null)
				Settings.Default.FwDataSourcePropertiesDlgGrid.InitializeGrid(m_grid);
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

			//// Go through the PA fields that have FW writing system types (i.e. vern. or analysis).
			//foreach (var fieldInfo in m_project.FieldInfo.SortedList
			//    .Where(fi => fi.FwWritingSystemType != FwDBUtils.FwWritingSystemType.None))
			//{
			//    var fieldName = fieldInfo.FwQueryFieldName;

			//    // Add the field to the list and, for now, assume
			//    // it has no writing system assigned.
			//    int index = m_grid.Rows.Add(new object[] { fieldInfo.DisplayText,
			//        NoWritingSystemText, fieldInfo.FwWritingSystemType });

			//    // Each row's tag property references an object indicating
			//    // what writing system is currently assigned to the row.
			//    if (m_fwDsInfo.WsMappings == null || m_fwDsInfo.WsMappings.Count == 0)
			//    {
			//        m_grid.Rows[index].Tag = new FwFieldWsMapping(fieldName, 0);
			//        continue;
			//    }

			//    // Find the mapping for the field just added to the grid and save a clone of
			//    // it in the row's tag.
			//    var wsMapping = m_fwDsInfo.WsMappings.SingleOrDefault(wsm => wsm.FieldName == fieldName);
			//    if (wsMapping != null)
			//    {
			//        m_grid.Rows[index].Tag = wsMapping.Clone();

			//        // Finally, put the writing system name in the proper row cell.
			//        if (IsWsNameSpecified(wsMapping.WsName))
			//            m_grid.Rows[index].Cells[kWsNameCol].Value = wsMapping.WsName;
			//        else
			//        {
			//            // The name wasn't found in the mapping so use the hvo from the mapping
			//            // to get the name of the writing system from the list of all the writing
			//            // systems found in the project.
			//            var ws = m_writingSystems.SingleOrDefault(w => w.WsHvo == wsMapping.WsHvo);
			//            m_grid.Rows[index].Cells[kWsNameCol].Value = ws != null ?
			//                ws.WsName : NoWritingSystemText;
			//        }
			//    }
			//}
		}

		/// ------------------------------------------------------------------------------------
		private string NoWritingSystemText
		{
			get
			{
				return App.LocalizeString("FwDataSourcePropertiesDlg.NoWritingSystemSpecifiedText", "(none)",
					"Item for no writing system specified for a field in the FW data source properties dialog.",
					App.kLocalizationGroupDialogs);
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
		void HandleGridEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			ComboBox cbo = e.Control as ComboBox;
			if (cbo == null || m_grid.CurrentRow == null)
				return;

			// Get the writing system type (analysis or vernacular) for the current row.
			var currWsType = (FwDBUtils.FwWritingSystemType)m_grid.CurrentRow.Cells[kWsTypeCol].Value;
			
			// Get the writing system information to which the current row is set.
			var currWsMapping = m_grid.CurrentRow.Tag as FwFieldWsMapping;
		
			// Clear the combo list and add the '(none)' options first.
			cbo.Items.Clear();
			cbo.Items.Add(m_noWritingSystemOption);

			// Add only the writing systems of the proper type (vern. or analysis).
			cbo.Items.AddRange(m_writingSystems.Where(ws => ws.Type == currWsType).ToArray());

			// Select the proper writing system.
			var itemToSelect = m_writingSystems.SingleOrDefault(ws => ws.Name == currWsMapping.WsName);
			if (itemToSelect == null)
				cbo.SelectedIndex = 0;
			else
				cbo.SelectedItem = itemToSelect;

			cbo.SelectionChangeCommitted -= HandleWritingSystemComboSelectionChangeCommitted;
			cbo.HandleDestroyed -= HandleWritingSystemComboHandleDestroyed;
			cbo.SelectionChangeCommitted += HandleWritingSystemComboSelectionChangeCommitted;
			cbo.HandleDestroyed += HandleWritingSystemComboHandleDestroyed;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWritingSystemComboHandleDestroyed(object sender, EventArgs e)
		{
			var cbo = sender as ComboBox;
			cbo.SelectionChangeCommitted -= HandleWritingSystemComboSelectionChangeCommitted;
			cbo.HandleDestroyed -= HandleWritingSystemComboHandleDestroyed;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// After the user has selected a writing system for the field, save the info.
		/// for the selected one in the row's tag property so it can be retrieved when
		/// the dialog's settings are saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleWritingSystemComboSelectionChangeCommitted(object sender, EventArgs e)
		{
			var cbo = sender as ComboBox;
			if (cbo == null || cbo.SelectedItem == null || m_grid.CurrentRow == null)
				return;

			var currWsInfo = m_grid.CurrentRow.Tag as FwFieldWsMapping;
			var pickedWs = cbo.SelectedItem as FwWritingSysInfo;

			if (currWsInfo != null && pickedWs != null)
			{
				currWsInfo.WsName = pickedWs.Name;
				currWsInfo.WsHvo = pickedWs.Hvo;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This will make sure that only writing system cells gets focus since they are the
		/// only one the user may change. I hate using SendKeys and it's sort of a kludge,
		/// but setting CurrentCell causing a reentrant error.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleGridCellEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
				SendKeys.Send("{TAB}");
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePhoneticStorageTypeCheckedChanged(object sender, EventArgs e)
		{
			m_dirty = true;
		}

		#endregion

		#region Misc. Overridden methods
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

		#region Methods for verifying changes and saving them before closing
		/// ------------------------------------------------------------------------------------
		protected override void SaveSettings()
		{
			Settings.Default.FwDataSourcePropertiesDlgGrid = GridSettings.Create(m_grid);
			base.SaveSettings();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies that a writing system has been specified for at least one field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool Verify()
		{
			if ((from DataGridViewRow row in m_grid.Rows select row.Tag as FwFieldWsMapping)
				.Any(ffwm => ffwm != null && IsWsNameSpecified(ffwm.WsName)))
			{
				return true;
			}

			var msg = App.LocalizeString("FwDataSourcePropertiesDlg.MissingWritingSystemMsg",
				"You must specify a writing system for at least one field.",
				"Message displayed in the FieldWorks data source properties dialog box when the user clicks OK without having assigned a writing system to any field.",
				App.kLocalizationGroupDialogs);

			Utils.MsgBox(msg);
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
				m_fwDsInfo.WsMappings = m_grid.Rows.Cast<DataGridViewRow>()
					.Select(r => r.Tag).OfType<FwFieldWsMapping>().ToList();

				foreach (var mapping in m_fwDsInfo.WsMappings
					.Where(mapping => !IsWsNameSpecified(mapping.WsName)))
				{
					mapping.WsName = null;
				}

				m_fwDsInfo.PhoneticStorageMethod = (rbLexForm.Checked ?
					FwDBUtils.PhoneticStorageMethod.LexemeForm :
					FwDBUtils.PhoneticStorageMethod.PronunciationField);
				
				return true;
			}
			catch
			{
				return false;
			}

		}

		#endregion
	
		/// ------------------------------------------------------------------------------------
		private bool IsWsNameSpecified(string wsName)
		{
			return (!string.IsNullOrEmpty(wsName) && wsName != NoWritingSystemText);
		}
	}
}