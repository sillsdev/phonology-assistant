using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public class FieldMapperGrid : SilGrid
	{
		public Func<string> SourceFieldColumnHeadingTextHandler;
		public Func<string> TargetFieldColumnHeadingTextHandler;

		private readonly List<PaField> m_mappedFields;
		private readonly IDictionary<FieldType, string> m_displayableFieldTypes;
		private readonly IEnumerable<PaField> m_potentialFields;
		private readonly CellCustomDropDownList m_potentialFieldsDropDown;

		/// ------------------------------------------------------------------------------------
		public FieldMapperGrid(IEnumerable<PaField> mappedFields, IEnumerable<PaField> potentialFields)
		{
			VirtualMode = true;
			Font = FontHelper.UIFont;
			RowHeadersVisible = false;
			BorderStyle = BorderStyle.None;

			m_mappedFields = mappedFields.Select(m => m.Copy()).ToList();
			m_potentialFields = potentialFields;
			m_displayableFieldTypes =
				PaField.GetDisplayableFieldTypes().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			m_potentialFieldsDropDown = new CellCustomDropDownList();

			AddColumns();

			RowCount = m_mappedFields.Count;

			App.SetGridSelectionColors(this, true);
		}

		/// ------------------------------------------------------------------------------------
		private void AddColumns()
		{
			// Create source field column.
			DataGridViewColumn col = CreateTextBoxColumn("srcfield");
			col.ReadOnly = true;
			var text = (SourceFieldColumnHeadingTextHandler != null ?
				SourceFieldColumnHeadingTextHandler() : null);

			if (string.IsNullOrEmpty(text))
			{
				text = App.LocalizeString(
					"FieldMappingGrid.DefaultSourceFieldColumnHeadingText",
					"Field in Source Data", App.kLocalizationGroupUICtrls);
			}
	
			col.HeaderText = text;
			Columns.Add(col);

			// Create the column for the arrow.
			col = CreateImageColumn("arrow");
			col.HeaderText = string.Empty;
			col.ReadOnly = true;
			col.Width = Properties.Resources.FieldMappingArrow.Width + 6;
			col.Resizable = DataGridViewTriState.False;
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			Columns.Add(col);

			// Create target field column.
			col = new SilButtonColumn("field");
			((SilButtonColumn)col).ButtonStyle = SilButtonColumn.ButtonType.MinimalistCombo;
			((SilButtonColumn)col).ButtonClicked += OnFieldColumnButtonClicked;
			((SilButtonColumn)col).DrawDefaultComboButtonWidth = false;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;

			text = (TargetFieldColumnHeadingTextHandler != null ?
				TargetFieldColumnHeadingTextHandler() : null);

			if (string.IsNullOrEmpty(text))
			{
				text = App.LocalizeString(
					"FieldMappingGrid.TargetFieldColumnHeadingText", "Field in PA",
					App.kLocalizationGroupUICtrls);
			}

			col.HeaderText = text;
			Columns.Add(col);

			// Create field type column.
			col = CreateDropDownListComboBoxColumn("fieldtype", m_displayableFieldTypes.Values);
			int i = Columns.Add(col);
			App.LocalizeObject(Columns[i], "FieldMappingGrid.FieldTypeColumnHeadingText",
				"Type", App.kLocalizationGroupUICtrls);

			// Create the parsed column.
			col = CreateCheckBoxColumn("parsed");
			i = Columns.Add(col);
			App.LocalizeObject(Columns[i], "FieldMappingGrid.FieldIsParsedColumnHeadingText",
				"Is Parsed?", App.kLocalizationGroupUICtrls);

			// Create the interlinear column.
			col = CreateCheckBoxColumn("interlinear");
			i = Columns.Add(col);
			App.LocalizeObject(Columns[i], "FieldMappingGrid.FieldCanBeInterlinearColumnHeadingText",
				"Is Interlinear?", App.kLocalizationGroupUICtrls);

			// TODO: Add font columns.
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetColumnHeadingText(string colName)
		{


			switch (colName)
			{
				case "srcfield": return App.LocalizeString(
					"FieldMappingGrid.SourceFieldColumnHeadingText", "Field in Source Data",
					App.kLocalizationGroupUICtrls);

				case "field": return App.LocalizeString(
					"FieldMappingGrid.TargetFieldColumnHeadingText", "Field in PA",
					App.kLocalizationGroupUICtrls);
				
				case "fieldtype": return App.LocalizeString(
					"FieldMappingGrid.FieldTypeColumnHeadingText", "Type",
					App.kLocalizationGroupUICtrls);
				
				case "parsed": return App.LocalizeString(
					"FieldMappingGrid.FieldIsParsedColumnHeadingText", "Is Parsed?",
					App.kLocalizationGroupUICtrls);

				case "interlinear": return App.LocalizeString(
					"FieldMappingGrid.FieldCanBeInterlinearColumnHeadingText", "Is Interlinear?",
					App.kLocalizationGroupUICtrls);
			}

			return string.Empty;
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
		protected virtual void OnFieldColumnButtonClicked(object sender, DataGridViewCellMouseEventArgs e)
		{
			var potentialFieldNames = m_potentialFields.Select(f => f.DisplayName).ToList();
			potentialFieldNames.Insert(0, GetNoMappingText());
			m_potentialFieldsDropDown.Show(this[e.ColumnIndex, e.RowIndex], potentialFieldNames);
		}

		/// ------------------------------------------------------------------------------------
		private string GetNoMappingText()
		{
			return App.LocalizeString("FieldMapperGrid.NoMappingText",
				"(no mapping)", App.kLocalizationGroupUICtrls);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
			{
				switch (Columns[e.ColumnIndex].Name)
				{
					case "srcfield": e.Value = m_mappedFields[e.RowIndex].NameInSource; break;
					case "field": e.Value = m_mappedFields[e.RowIndex].DisplayName; break;
					case "fieldtype": e.Value = m_mappedFields[e.RowIndex].GetTypeDisplayName(); break;
					case "parsed": e.Value = m_mappedFields[e.RowIndex].IsParsed; break;
					case "interlinear": e.Value = m_mappedFields[e.RowIndex].IsInterlinear; break;
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

			var valAsString = e.Value as string;

			switch (Columns[e.ColumnIndex].Name)
			{
				case "field":
					if (valAsString == GetNoMappingText())
						m_mappedFields[e.RowIndex].Name = null;
					else
					{
						var field = m_potentialFields.SingleOrDefault(f => f.DisplayName == valAsString);
						m_mappedFields[e.RowIndex].Name = (field != null ? field.Name : valAsString);
					}
					break;

				case "fieldtype":
					m_mappedFields[e.RowIndex].Type =
						m_displayableFieldTypes.Single(kvp => kvp.Value == valAsString).Key;
					break;
				
				case "parsed":
					m_mappedFields[e.RowIndex].IsParsed = (bool)e.Value;

					// Unparsed fields cannot also be interlinear fields.
					// So make sure that property is turned off.
					if (!m_mappedFields[e.RowIndex].IsParsed)
					{
						m_mappedFields[e.RowIndex].IsInterlinear = false;
						InvalidateCell(Columns["interlinear"].Index, e.RowIndex);
					}
					break;

				case "interlinear":
					m_mappedFields[e.RowIndex].IsInterlinear = (bool)e.Value;
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
			{
				this[e.ColumnIndex, e.RowIndex].ReadOnly = false;
				var colName = Columns[e.ColumnIndex].Name;
				var field = m_mappedFields[e.RowIndex];

				if (colName == "interlinear" &&
					(!field.GetTypeAllowsFieldToBeInterlinear() || !field.IsParsed))
				{
					this[e.ColumnIndex, e.RowIndex].ReadOnly = true;
				}
				else if (colName == "parsed" && !field.GetTypeAllowsFieldToBeParsed())
				{
					this[e.ColumnIndex, e.RowIndex].ReadOnly = true;
				}
			}
	
			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
		{
			//if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
			//    Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && (bool)e.FormattedValue)
			//{
			//    var colName = Columns[e.ColumnIndex].Name;
			//    var field = m_mappedFields[e.RowIndex];

			//    if (colName == "interlinear" &&
			//        (!field.GetDoesTypeAllowToBeInterlinear() || !field.IsParsed))
			//    {
			//        e.Cancel = true;
			//    }
			//    else if (colName == "parsed" && !field.GetDoesTypeAllowToBeParsed())
			//    {
			//        e.Cancel = true;
			//    }
			//}
			
			base.OnCellValidating(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
		{
			base.OnCellPainting(e);

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			var colName = Columns[e.ColumnIndex].Name;
			var field = m_mappedFields[e.RowIndex];

			if (!("parsed interlinear").Contains(colName) ||
				(colName == "parsed" && field.GetTypeAllowsFieldToBeParsed()) ||
				(colName == "interlinear" && field.GetTypeAllowsFieldToBeInterlinear()))
			{
				return;
			}

			var parts = e.PaintParts & ~DataGridViewPaintParts.ContentForeground;
			e.Paint(e.ClipBounds, parts);
			e.Handled = true;
		}
	}
}
