// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Controls
{
	public class Fw6FieldMappingGrid : FieldMappingGridBase
	{
		protected IEnumerable<FwWritingSysInfo> m_writingSystems;
	
		/// ------------------------------------------------------------------------------------
		public Fw6FieldMappingGrid(PaDataSource ds)
		{
			m_writingSystems = ds.FwDataSourceInfo.GetWritingSystems();
			AddWritingSystemColumn();
			ShowFontColumn(false);
		}

		/// ------------------------------------------------------------------------------------
		public Fw6FieldMappingGrid(PaDataSource ds, IEnumerable<PaField> projectFields) : this(ds)
		{
			var mappableFields = Properties.Settings.Default.Fw6FieldsMappableInPropsDlg.Cast<string>();
			m_potentialFields = projectFields.Where(f => mappableFields.Contains(f.Name));

			m_mappings = (from fname in mappableFields
						  let mapping = ds.FieldMappings.SingleOrDefault(m => m.Field.Name == fname)
						  select (mapping != null ? mapping.Copy() :
								new FieldMapping(m_potentialFields.Single(f => f.Name == fname), false))).ToList();
			
			LockTargetFieldColumn();
			RowCount = m_mappings.Count;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void AddWritingSystemColumn()
		{
			var wslist = m_writingSystems.Select(ws => ws.Name).ToList();
			wslist.Insert(0, GetNoWritingSystemText());

			// Create FW writing system column.
			var col = CreateDropDownListComboBoxColumn("fwws", wslist);
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			Columns.Insert(FontColumnIndex, col);
			col.HeaderText = LocalizationManager.GetString("DialogBoxes.Fw6DataSourcePropertiesDlg.FieldMappingGrid.ColumnHeadings.WritingSystem", "Writing System", null, col);
		}
		
		/// ------------------------------------------------------------------------------------
		protected virtual string GetNoWritingSystemText()
		{
			return LocalizationManager.GetString(
				"DialogBoxes.Fw6DataSourcePropertiesDlg.FieldMappingGrid.NoWritingSystemText", "(none)");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modify the writing system combo list on the fly, only adding to the list those
		/// writing systems appropriate for the field in the current row.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override KeyValuePair<object, IEnumerable<object>> OnGetComboCellList(
			DataGridViewCell cell, DataGridViewEditingControlShowingEventArgs e)
		{
			return GetWritingSystemDropDownItems(GetFieldAt(cell.RowIndex));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems appropriate for the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual KeyValuePair<object, IEnumerable<object>> GetWritingSystemDropDownItems(PaField field)
		{
			var wslist = (field == null ? new List<string>(0) : GetWritingSystemsForField(field));

			var currWs = (m_mappings.Count == CurrentCellAddress.Y) ? null :
				m_writingSystems.SingleOrDefault(ws => ws.Id == m_mappings[CurrentCellAddress.Y].FwWsId);

			object currValue = 0;
			if (currWs != null)
				currValue = currWs.Name;

			return new KeyValuePair<object, IEnumerable<object>>(currValue, wslist.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<string> GetWritingSystemsForField(PaField field)
		{
			if (field.Type != FieldType.Phonetic)
				yield return GetNoWritingSystemText();

			var wsValidType = (field.FwWsType == FwDBUtils.FwWritingSystemType.Vernacular ?
				FwDBUtils.FwWritingSystemType.Vernacular : FwDBUtils.FwWritingSystemType.Analysis);

			foreach (var wsName in m_writingSystems.Where(ws => ws.Type == wsValidType).Select(ws => ws.Name))
				yield return wsName;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex != NewRowIndex && (GetColumnName(e.ColumnIndex) == "fwws" && e.RowIndex < m_mappings.Count))
			{
				var fwws = m_writingSystems.SingleOrDefault(ws => ws.Id == m_mappings[e.RowIndex].FwWsId);
				e.Value = (fwws != null ? fwws.Name : GetNoWritingSystemText());
			}

			base.OnCellValueNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				base.OnCellValuePushed(e);
				return;
			}

			if (GetColumnName(e.ColumnIndex) == "fwws")
			{
				var valAsString = (e.Value as string ?? string.Empty);
				valAsString = valAsString.Trim();
				var fwws = m_writingSystems.SingleOrDefault(ws => ws.Name == valAsString);
				m_mappings[e.RowIndex].FwWsId = (fwws != null ? fwws.Id : null);
			}

			InvalidateRow(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldMapping> Mappings
		{
			get { return base.Mappings.Where(m => m.FwWsId != "0" && m.FwWsId != null); }
		}
	}
}
