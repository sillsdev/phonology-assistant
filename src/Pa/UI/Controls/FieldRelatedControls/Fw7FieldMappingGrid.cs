using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.DataSource;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Controls
{
	public class Fw7FieldMappingGrid : FieldMappingGridBase
	{
		private readonly IEnumerable<FwWritingSysInfo> m_writingSystems;
		private string m_tgtFieldColName;

		/// ------------------------------------------------------------------------------------
		public Fw7FieldMappingGrid(PaDataSource ds, IEnumerable<PaField> potentialFields)
		{
			m_writingSystems = ds.FwDataSourceInfo.GetWritingSystems();
			m_potentialFields = from field in potentialFields
								where field.AllowUserToMap && field.Type != FieldType.Phonetic &&
									!PaField.GetIsCalculatedField(field)
								orderby field.Name
								select field;
				
			if (ds.FieldMappings == null)
				m_mappings = FieldMapping.GetDefaultFw7Mappings(m_writingSystems).ToList();
			else
			{
				// Don't want to show the phonetic mapping in this grid.
				m_mappings = ds.FieldMappings
					.Where(m => m.Field.Type != FieldType.Phonetic).Select(m => m.Copy()).ToList();
			}

			AddOurColumns();
			ShowFontColumn(false);
			AllowUserToAddRows = true;
			RowCount = m_mappings.Count + 1;
		}

		/// ------------------------------------------------------------------------------------
		private void AddOurColumns()
		{
			// Create the target field combo box column.
			DataGridViewColumn col = CreateDropDownListComboBoxColumn(m_tgtFieldColName,
				m_potentialFields.Select(f => f.DisplayName));
			
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.HeaderText = GetFieldColumnHeadingText();
			Columns.Insert(0, col);

			var wslist = m_writingSystems.Select(ws => ws.Name).ToList();
			wslist.Insert(0, GetNoWritingSystemText());

			// Create FW writing system column.
			col = CreateDropDownListComboBoxColumn("fwws", wslist);
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			int i = FontColumnIndex;
			Columns.Insert(i, col);
			App.LocalizeObject(Columns[i], "FieldMappingGrid.FwWritingSystemColumnHeadingText",
			                   "Writing System", App.kLocalizationGroupUICtrls);

			AddRemoveRowColumn(Properties.Resources.RemoveGridRowNormal, Properties.Resources.RemoveGridRowHot,
			    () => App.LocalizeString("Fw7DataSourcePropertiesDlg.RemoveFieldToolTip", "Remove Field", App.kLocalizationGroupDialogs),
			    rowIndex => m_mappings.RemoveAt(rowIndex));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Override because we want our field column to be a combo box column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void AddFieldColumn(string colName)
		{
			m_tgtFieldColName = colName;
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
			var field = GetFieldAt(cell.RowIndex);
			return (GetCurrentColumnName() == "fwws" ?
				GetWritingSystemDropDownItems(field) : GetFieldsDropDownItems(field));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems appropriate for the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private KeyValuePair<object, IEnumerable<object>> GetFieldsDropDownItems(PaField field)
		{
			var fieldsAlreadyMapped = new List<PaField>();
			for (int i = 0; i < NewRowIndex; i++)
				fieldsAlreadyMapped.Add(GetFieldAt(i));
			
			if (field != null)
				fieldsAlreadyMapped.Remove(field);

			var fldList = from fld in m_potentialFields
						  where !fieldsAlreadyMapped.Any(f => f.Name == fld.Name)
						  orderby fld.DisplayName
						  select fld.DisplayName;

			return new KeyValuePair<object, IEnumerable<object>>(
				field == null ? null : field.DisplayName, fldList.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the writing systems appropriate for the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private KeyValuePair<object, IEnumerable<object>> GetWritingSystemDropDownItems(PaField field)
		{
			var wslist = new List<string>();

			if (field != null)
			{
				if (field.FwWsType == FwDBUtils.FwWritingSystemType.None)
					wslist.Insert(0, GetNoWritingSystemText());
				else
				{
					wslist.AddRange((field.FwWsType == FwDBUtils.FwWritingSystemType.Mixed ?
						m_writingSystems :
						m_writingSystems.Where(ws => ws.Type == field.FwWsType)).Select(ws => ws.Name));
				}
			}

			return new KeyValuePair<object, IEnumerable<object>>(
				field == null ? -1 : 0, wslist.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		private static string GetNoWritingSystemText()
		{
			return App.LocalizeString("FieldMappingGrid.NoWritingSystemText",
				"(n/a)", App.kLocalizationGroupUICtrls);
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

			FieldMapping mapping;

			// Check if we need to add a new field to the end of the list.
			switch (GetColumnName(e.ColumnIndex))
			{
				case "tgtfield":
					var field = m_potentialFields.Single(f => f.DisplayName == e.Value as string);

					if (e.RowIndex == m_mappings.Count)
					{
						mapping = new FieldMapping(field.Name, field, false);
						m_mappings.Add(mapping);
					}
					else
					{
						mapping = m_mappings[e.RowIndex];
						mapping.NameInDataSource = field.Name;
						mapping.Field = field;
					}

					FieldMapping.CheckMappingsFw7WritingSystem(mapping, m_writingSystems);
					UpdateCellValue(Columns["fwws"].Index, e.RowIndex);
					break;

				case "fwws":
					var valAsString = (e.Value as string ?? string.Empty);
					valAsString = valAsString.Trim();
					mapping = m_mappings[e.RowIndex];
					var fwws = m_writingSystems.SingleOrDefault(ws => ws.Name == valAsString);
					mapping.FwWsId = (fwws != null ? fwws.Id : null);
					break;
			}

			InvalidateRow(e.RowIndex);
		}
	}
}
