using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.Model;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public class FieldMappingGridBase : SilGrid
	{
		protected Func<string> m_sourceFieldColumnHeadingTextHandler;
		protected Func<string> m_targetFieldColumnHeadingTextHandler;

		protected List<FieldMapping> m_mappings;
		protected IEnumerable<PaField> m_potentialFields;
		protected readonly CellCustomDropDownList m_cellDropDown;
		protected readonly FontPicker m_fontPicker;

		/// ------------------------------------------------------------------------------------
		public FieldMappingGridBase()
		{
			VirtualMode = true;
			Font = FontHelper.UIFont;
			RowHeadersVisible = false;
			BorderStyle = BorderStyle.None;
			App.SetGridSelectionColors(this, true);
			
			Fonts = new Dictionary<string, Font>();
			m_cellDropDown = new CellCustomDropDownList();
			m_fontPicker = new FontPicker();
			m_fontPicker.Closed += HandleFontPickerClosed;

			AddColumns();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void AddColumns()
		{
			AddFieldColumn("tgtfield");
			AddFontColumn("font");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create and add the target field column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void AddFieldColumn(string colName)
		{
			DataGridViewColumn col = new SilButtonColumn(colName);
			((SilButtonColumn)col).ButtonStyle = SilButtonColumn.ButtonType.MinimalistCombo;
			((SilButtonColumn)col).ButtonClicked += OnFieldColumnButtonClicked;
			((SilButtonColumn)col).DrawDefaultComboButtonWidth = false;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.HeaderText = GetFieldColumnHeadingText();
			Columns.Add(col);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Create and add the font column.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual void AddFontColumn(string colName)
		{
			DataGridViewColumn col = new SilButtonColumn(colName);
			((SilButtonColumn)col).ButtonStyle = SilButtonColumn.ButtonType.MinimalistCombo;
			((SilButtonColumn)col).ButtonClicked += OnFontColumnButtonClicked;
			((SilButtonColumn)col).DrawDefaultComboButtonWidth = false;
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			col.Visible = false;
			int i = Columns.Add(col);
			App.RegisterForLocalization(Columns[i], "FieldMappingGridBase.FontColumnHeadingText", "Font");
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetFieldColumnHeadingText()
		{
			var text = (m_targetFieldColumnHeadingTextHandler != null ?
				m_targetFieldColumnHeadingTextHandler() : null);

			if (string.IsNullOrEmpty(text))
				text = App.GetString("FieldMappingGridBase.TargetFieldColumnHeadingText", "Field");

			return text;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<FieldMapping> Mappings
		{
			get { return m_mappings.Where(m => m.Field != null); }
		}

		/// ------------------------------------------------------------------------------------
		public virtual Dictionary<string, Font> Fonts { get; private set; }

		/// ------------------------------------------------------------------------------------
		public virtual void ShowFontColumn(bool show)
		{
			Columns["font"].Visible = show;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes the target field column readonly and turns off display of the combobox
		/// drop-down button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void LockTargetFieldColumn()
		{
			var col = Columns["tgtfield"] as SilButtonColumn;
			col.ShowButton = false;
			col.ReadOnly = true;
		}

		/// ------------------------------------------------------------------------------------
		protected int FontColumnIndex
		{
			get { return Columns["font"].Index; }
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnFieldColumnButtonClicked(object sender, DataGridViewCellMouseEventArgs e)
		{
			var potentialFieldNames = m_potentialFields.Select(f => f.DisplayName).ToList();
			potentialFieldNames.Insert(0, GetNoMappingText());
			m_cellDropDown.Show(this[e.ColumnIndex, e.RowIndex], potentialFieldNames);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnFontColumnButtonClicked(object sender, DataGridViewCellMouseEventArgs e)
		{
			var field = GetFieldAt(e.RowIndex);
			Font fnt;
			
			if (field != null && Fonts.TryGetValue(field.Name, out fnt))
				m_fontPicker.ShowForGridCell(fnt, this[e.ColumnIndex, e.RowIndex], true);
	
			// In the else case, display the fading message window.
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleFontPickerClosed(FontPicker sender, DialogResult result)
		{
			if (result == DialogResult.OK)
			{
				var field = GetFieldAt(CurrentCellAddress.Y);
				Fonts[field.Name] = (Font)m_fontPicker.Font.Clone();
				UpdateCellValue(CurrentCellAddress.X, CurrentCellAddress.Y);
				IsDirty = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual string GetNoMappingText()
		{
			return App.GetString("FieldMappingGridBase.NoMappingText", "(no mapping)");
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex >= 0 && e.RowIndex < m_mappings.Count)
			{
				var mapping = m_mappings[e.RowIndex];

				switch (GetColumnName(e.ColumnIndex))
				{
					case "tgtfield": e.Value = (mapping.Field == null ? null : mapping.Field.DisplayName); break;
					case "font":
						Font fnt = null;
						if (mapping.Field != null && !Fonts.TryGetValue(mapping.Field.Name, out fnt))
						{
							fnt = (Font)mapping.Field.Font.Clone();
							Fonts[mapping.Field.Name] = fnt;
						}

						e.Value = GetFontDisplayString(fnt);
						break;
				}
			}

			base.OnCellValueNeeded(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
		{
			base.OnCellValuePushed(e);

			if (e.ColumnIndex < 0 || e.RowIndex == m_mappings.Count)
				return;

			if (GetColumnName(e.ColumnIndex) == "tgtfield")
			{
				var valAsString = (e.Value as string ?? string.Empty);
				PushFieldName(m_mappings[e.RowIndex], valAsString.Trim(), e.RowIndex);
				IsDirty = true;
			}

			InvalidateRow(e.RowIndex);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void PushFieldName(FieldMapping mapping, string fieldName, int rowIndex)
		{
			if (fieldName == GetNoMappingText() || fieldName == string.Empty)
			{
				mapping.Field = null;
				mapping.IsParsed = false;
				mapping.IsInterlinear = false;
			}
			else
			{
				mapping.Field =
					m_potentialFields.SingleOrDefault(f => f.DisplayName == fieldName) ??
					new PaField(fieldName, GetTypeAtOrDefault(rowIndex));
			}
		}

		/// ------------------------------------------------------------------------------------
		private static string GetFontDisplayString(Font fnt)
		{
			if (fnt == null)
				return null;

			string fmt;

			if (fnt.Bold && fnt.Italic)
				fmt = App.GetString("FieldMappingGridBase.FontDisplayFormatAll", "{0}, {1}pt, Bold, Italic");
			else if (fnt.Bold)
				fmt = App.GetString("FieldMappingGridBase.FontDisplayFormatBold", "{0}, {1}pt, Bold");
			else if (fnt.Italic)
				fmt = App.GetString("FieldMappingGridBase.FontDisplayFormatItalic", "{0}, {1}pt, Italic");
			else
				fmt = App.GetString("FieldMappingGridBase.FontDisplayFormat", "{0}, {1}pt");

			return string.Format(fmt, fnt.FontFamily.Name, (int)fnt.SizeInPoints);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual FieldType GetTypeAtOrDefault(int index)
		{
			var field = GetFieldAt(index);
			return (field != null ? field.Type : default(FieldType));
		}

		/// ------------------------------------------------------------------------------------
		protected virtual PaField GetFieldAt(int index)
		{
			return (index >= 0 && index < m_mappings.Count ?  m_mappings[index].Field : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the cells for fields that can't be parsed or interlinear are set to
		/// readonly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
		{
			if (e.RowIndex >= 0 && e.RowIndex < m_mappings.Count)
			{
				var mapping = m_mappings[e.RowIndex];
				if (GetColumnName(e.ColumnIndex) == "font" && mapping.Field != null)
					e.CellStyle.Font = Fonts[mapping.Field.Name];
			}
	
			base.OnCellFormatting(e);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetAreAnyFieldsMappedMultipleTimes()
		{
			return Mappings.Select(m => m.Field.Name)
				.Any(fname => Mappings.Count(m => m.Field.Name == fname) > 1);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetIsPhoneticMappedMultipleTimes()
		{
			return (Mappings.Count(m => m.Field.Type == FieldType.Phonetic) > 1);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetIsTargetFieldMapped(string fieldName)
		{
			return Mappings.Any(m => m.Field.Name == fieldName);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetIsSourceFieldMapped(string fieldName)
		{
			return Mappings.Any(m => m.NameInDataSource == fieldName);
		}

		/// ------------------------------------------------------------------------------------
		public virtual PaField GetMappedFieldForSourceField(string fieldName)
		{
			return (from m in Mappings where m.NameInDataSource == fieldName select m.Field).FirstOrDefault();
		}
	}
}
