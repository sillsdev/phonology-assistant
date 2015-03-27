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
using SIL.Pa.UI.Dialogs;

namespace SIL.Pa.UI.Controls
{
	public class Fw7FieldMappingGrid : Fw6FieldMappingGrid
	{
		private string m_tgtFieldColName;

		/// ------------------------------------------------------------------------------------
		public Fw7FieldMappingGrid(PaDataSource ds, IEnumerable<PaField> potentialFields) : base(ds)
		{
			m_potentialFields = potentialFields.OrderBy(f => f.DisplayName);

			// We don't want to show the phonetic and audio file mappings in this grid.
			m_mappings = (from m in ds.FieldMappings
						  where m.Field.Type != FieldType.Phonetic && m.Field.Type != FieldType.AudioFilePath
						  select m.Copy()).ToList();

			CustomizeGrid();
			AllowUserToAddRows = true;
			RowCount = m_mappings.Count + 1;
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
		private void CustomizeGrid()
		{
			// Create the target field combo box column.
			DataGridViewColumn col = CreateDropDownListComboBoxColumn(m_tgtFieldColName,
				m_potentialFields.Select(f => f.DisplayName));

			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			Columns.Insert(0, col);
			col.HeaderText = LocalizationManager.GetString(
				"DialogBoxes.Fw7DataSourcePropertiesDlg.FieldMappingGrid.ColumnHeadings.Field", "Field", null, col);

			AddRemoveRowColumn(Properties.Resources.RemoveGridRowNormal, Properties.Resources.RemoveGridRowHot,
				() => LocalizationManager.GetString(
					"DialogBoxes.Fw7DataSourcePropertiesDlg.FieldMappingGrid.RemoveFieldToolTip", "Remove Field"),
						rowIndex => m_mappings.RemoveAt(rowIndex));
		}

		/// ------------------------------------------------------------------------------------
		protected override string GetNoWritingSystemText()
		{
			return LocalizationManager.GetString(
				"DialogBoxes.Fw7DataSourcePropertiesDlg.FieldMappingGrid.WritingSystemNotApplicableText", "(n/a)");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Modify the field combo list on the fly, only adding to the list those fields that
		/// haven't been mapped.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override KeyValuePair<object, IEnumerable<object>> OnGetComboCellList(
			DataGridViewCell cell, DataGridViewEditingControlShowingEventArgs e)
		{
			return (GetCurrentColumnName() == "tgtfield" ?
				GetFieldsDropDownItems(GetFieldAt(cell.RowIndex)) :
				base.OnGetComboCellList(cell, e));
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
		        where
		            !fieldsAlreadyMapped.Any(f => f.Name == fld.Name) &&
                     (Fw7DataSourcePropertiesDlg.m_selectedvernacularItem.Name == FwDBUtils.PhoneticStorageMethod.LexemeForm.ToString() ||
		            fld.Name != Fw7DataSourcePropertiesDlg.m_selectedvernacularItem.Name)
		        orderby fld.DisplayName
		        select fld.DisplayName;

			return new KeyValuePair<object, IEnumerable<object>>(
				field == null ? null : field.DisplayName, fldList.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<string> GetWritingSystemsForField(PaField field)
		{
			if (field.FwWsType == FwDBUtils.FwWritingSystemType.None ||
				field.FwWsType == FwDBUtils.FwWritingSystemType.CmPossibility)
			{
				yield return GetNoWritingSystemText();
			}
			else
			{
				foreach (var wsName in m_writingSystems.Where(ws => ws.Type == field.FwWsType).Select(ws => ws.Name))
					yield return wsName;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
		{
			// Check if we need to add a new field to the end of the list.
			if (e.RowIndex >= 0 && GetColumnName(e.ColumnIndex) == "tgtfield")
			{
				FieldMapping mapping;
				var field = m_potentialFields.Single(f => f.DisplayName == e.Value as string);

				if (e.RowIndex == m_mappings.Count)
				{
					mapping = new FieldMapping(field, false);
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
			}

			base.OnCellValuePushed(e);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldMapping> Mappings
		{
		    get { return m_mappings.Where(m => m.Field != null); }
		}
	}
}
