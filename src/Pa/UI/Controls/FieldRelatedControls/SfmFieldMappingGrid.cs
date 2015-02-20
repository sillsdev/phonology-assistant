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
using System.Linq;
using System.Windows.Forms;
using Localization;
using SIL.Pa.Model;
using SIL.Pa.Properties;

namespace SIL.Pa.UI.Controls
{
	public class SfmFieldMappingGrid : FieldMappingGridBase
	{
		private IDictionary<FieldType, string> m_displayableFieldTypes;
		private readonly IEnumerable<PaField> m_defaultFields;

		/// ------------------------------------------------------------------------------------
		public SfmFieldMappingGrid(IEnumerable<PaField> potentialFields, IEnumerable<FieldMapping> mappings,
			Func<string> srcFldColHdgTextHandler, Func<string> tgtFldColHdgTextHandler)
		{
			m_sourceFieldColumnHeadingTextHandler = srcFldColHdgTextHandler;
			m_targetFieldColumnHeadingTextHandler = tgtFldColHdgTextHandler;
			
			m_potentialFields = potentialFields;
			m_mappings = mappings.ToList();

			m_defaultFields = PaField.GetDefaultFields();

			ShowFontColumn(false);
			RowCount = m_mappings.Count;
		}
		
		/// ------------------------------------------------------------------------------------
		protected override void AddColumns()
		{
			m_displayableFieldTypes =
				PaField.GetDisplayableFieldTypes().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			// Create source field column.
			DataGridViewColumn col = CreateTextBoxColumn("srcfield");
			col.ReadOnly = true;
			Columns.Add(col);

			if (m_sourceFieldColumnHeadingTextHandler != null)
				col.HeaderText = m_sourceFieldColumnHeadingTextHandler();

			if (m_sourceFieldColumnHeadingTextHandler == null || string.IsNullOrEmpty(col.HeaderText))
			{
				col.HeaderText = LocalizationManager.GetString(
					"DialogBoxes.SfmDataSourcePropertiesDlg.FieldMappingGrid.ColumnHeadings.Field",
					"Field in Source Data", null, col);
			}

			// Create the column for the arrow.
			col = CreateImageColumn("arrow");
			col.HeaderText = string.Empty;
			col.ReadOnly = true;
			col.Width = Properties.Resources.FieldMappingArrow.Width + 6;
			col.Resizable = DataGridViewTriState.False;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			Columns.Add(col);

			base.AddColumns();

			// Create field type column.
			col = CreateDropDownListComboBoxColumn("fieldtype", m_displayableFieldTypes.Values);
			int i = FontColumnIndex;
			Columns.Insert(FontColumnIndex, col);
			col.HeaderText = LocalizationManager.GetString(
				"DialogBoxes.SfmDataSourcePropertiesDlg.FieldMappingGrid.ColumnHeadings.Type",
				"Type", null, col);

			// Create the parsed column.
			col = CreateCheckBoxColumn("parsed");
			Columns.Insert(i, col);
			col.HeaderText = LocalizationManager.GetString(
				"DialogBoxes.SfmDataSourcePropertiesDlg.FieldMappingGrid.ColumnHeadings.IsParsed",
				"Is Parsed?", null, col);

			// Create the interlinear column.
			col = CreateCheckBoxColumn("interlinear");
			Columns.Insert(i, col);
			col.HeaderText = LocalizationManager.GetString(
				"DialogBoxes.SfmDataSourcePropertiesDlg.FieldMappingGrid.ColumnHeadings.CanBeInterlinear",
				"Is Interlinear?", null, col);
		}

		/// ------------------------------------------------------------------------------------
		public void ShowIsParsedColumn(bool show)
		{
			Columns["parsed"].Visible = show;
		}

		/// ------------------------------------------------------------------------------------
		public void ShowIsInterlinearColumn(bool show)
		{
			Columns["interlinear"].Visible = show;
		}

		/// ------------------------------------------------------------------------------------
		public void ShowTypeColumn(bool show)
		{
			Columns["fieldtype"].Visible = show;
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
			var invalidTypes = new[] { FieldType.AudioFilePath,
				FieldType.Phonetic, FieldType.Reference };

			// Build a list of field types the user may choose.
			var validTypeNames = m_displayableFieldTypes.Where(kvp =>
				!invalidTypes.Contains(kvp.Key)).Select(kvp => kvp.Value);

			return new KeyValuePair<object, IEnumerable<object>>(
				m_displayableFieldTypes[GetTypeAtOrDefault(cell.RowIndex)],
				validTypeNames.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
			{
				var mapping = m_mappings[e.RowIndex];

				switch (Columns[e.ColumnIndex].Name)
				{
					case "srcfield": e.Value = mapping.NameInDataSource; break;
					case "fieldtype": e.Value = m_displayableFieldTypes[GetTypeAtOrDefault(e.RowIndex)]; break;
					case "parsed": e.Value = mapping.IsParsed; break;
					case "interlinear": e.Value = mapping.IsInterlinear; break;
					case "arrow": e.Value = Properties.Resources.FieldMappingArrow; break;
				}
			}

			base.OnCellValueNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
		{
			base.OnCellValuePushed(e);

			if (e.ColumnIndex < 0 || e.RowIndex < 0)
				return;

			var valAsString = (e.Value as string ?? string.Empty);
			valAsString = valAsString.Trim();
			var mapping = m_mappings[e.RowIndex];

			switch (Columns[e.ColumnIndex].Name)
			{
				case "fieldtype":
					var newType = m_displayableFieldTypes.Single(kvp => kvp.Value == valAsString).Key;
					if (mapping.Field != null)
						mapping.Field.Type = newType;
					else
						mapping.Field = new PaField(null, newType);

					break;
				
				case "parsed":
					mapping.IsParsed = (bool)e.Value;

					// Unparsed fields cannot also be interlinear fields.
					// So make sure that property is turned off.
					if (!mapping.IsParsed)
						mapping.IsInterlinear = false;
					
					break;

				case "interlinear":
					mapping.IsInterlinear = (bool)e.Value;
					break;
			}

			InvalidateRow(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the cells for fields that can't be parsed or interlinear are set to
		/// readonly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				var mapping = m_mappings[e.RowIndex];
				var type = GetTypeAtOrDefault(e.RowIndex);
				var readOnly = false;

				switch (Columns[e.ColumnIndex].Name)
				{
					case "fieldtype":
						readOnly = mapping.Field != null && m_defaultFields.Any(f => f.Name == mapping.Field.Name);
						break;

					case "parsed":
						readOnly = (!PaField.GetIsTypeParsable(type) ||
							(mapping.Field != null && mapping.Field.Type == FieldType.Phonetic));
						break;
					
					case "interlinear":
						readOnly = !PaField.GetIsTypeInterlinearizable(type);
						break;
				}

				this[e.ColumnIndex, e.RowIndex].ReadOnly = readOnly;
			}
	
			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure any field that can't be marked as parsed or interlinear has it's check
		/// box painted over so the check box cannot be seen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			base.OnCellPainting(e);

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			var colName = Columns[e.ColumnIndex].Name;
			var type = GetTypeAtOrDefault(e.RowIndex);

			if (!("parsed interlinear").Contains(colName) ||
				(colName == "parsed" && PaField.GetIsTypeParsable(type)) ||
				(colName == "interlinear" && PaField.GetIsTypeInterlinearizable(type)))
			{
				return;
			}

			var parts = e.PaintParts & ~DataGridViewPaintParts.ContentForeground;
			e.Paint(e.ClipBounds, parts);
			e.Handled = true;
		}

		/// ------------------------------------------------------------------------------------
		public void MarkSourceFieldAsInterlinear(string srcFieldName)
		{
			var mapping = m_mappings.SingleOrDefault(m => m.NameInDataSource == srcFieldName);
			if (mapping != null)
			{
				mapping.IsInterlinear = true;
				InvalidateColumn(Columns["interlinear"].Index);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void ClearAllFieldsInterlinearFlag()
		{
			foreach (var mapping in m_mappings)
				mapping.IsInterlinear = false;

			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public void ClearAllFieldsParsedFlag()
		{
			foreach (var mapping in m_mappings)
				mapping.IsParsed = false;

			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public void SetDefaultParsedFlags()
		{
			foreach (var mapping in m_mappings.Where(m => m.Field != null))
				mapping.IsParsed = Settings.Default.DefaultParsedSfmFields.Contains(mapping.Field.Name);

			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		public void SetPhoneticAsOnlyParsedField()
		{
			foreach (var mapping in m_mappings.Where(m => m.Field != null))
				mapping.IsParsed = (mapping.Field.Type == FieldType.Phonetic);

			Invalidate();
		}
	}
}
