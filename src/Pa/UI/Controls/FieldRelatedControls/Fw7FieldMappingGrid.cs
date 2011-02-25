using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SIL.Pa.DataSource.FieldWorks;
using SIL.Pa.Model;

namespace SIL.Pa.UI.Controls
{
	public class Fw7FieldMappingGrid : FieldMappingGridBase
	{
		private readonly IEnumerable<FwWritingSysInfo> m_writingSystems;

		/// ------------------------------------------------------------------------------------
		public Fw7FieldMappingGrid(FwDataSourceInfo fwds, IEnumerable<PaField> potentialFields)
		{
			m_potentialFields = potentialFields;
			m_mappings = potentialFields.Select(f => new FieldMapping { Field = f }).ToList();
			m_writingSystems = FwDBUtils.GetWritingSystemsForFw7Project(fwds);

			ShowFontColumn(false);
			RowCount = m_mappings.Count;
		}
		
		/// ------------------------------------------------------------------------------------
		protected override void AddColumns()
		{
			base.AddColumns();

			// Create FW writing system column.
			DataGridViewColumn col = CreateDropDownListComboBoxColumn("fwws");
			col.SortMode = DataGridViewColumnSortMode.NotSortable;
			int i = FontColumnIndex;
			Columns.Insert(i, col);
			App.LocalizeObject(Columns[i], "FieldMappingGrid.FwWritingSystemColumnHeadingText",
				"Writing System", App.kLocalizationGroupUICtrls);
		}

		/// ------------------------------------------------------------------------------------
		protected override KeyValuePair<object, IEnumerable<object>> OnGetComboCellList(
			DataGridViewCell cell, DataGridViewEditingControlShowingEventArgs e)
		{
			var field = GetFieldAt(cell.RowIndex);

			var wslist = (field.FwWsType == FwDBUtils.FwWritingSystemType.Mixed ?
				m_writingSystems : m_writingSystems.Where(ws => ws.WsType == field.FwWsType)).Select(ws => ws.WsName).ToList();

			wslist.Insert(0, GetNoWritingSystemText());
			return new KeyValuePair<object, IEnumerable<object>>(0, wslist.Cast<object>());
		}

		/// ------------------------------------------------------------------------------------
		private static string GetNoWritingSystemText()
		{
			return App.LocalizeString("FieldMappingGrid.NoWritingSystemText",
				"(none)", App.kLocalizationGroupUICtrls);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && Columns[e.ColumnIndex].Name == "fwws")
				e.Value = m_mappings[e.RowIndex].FwWritingSystem;

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
				//case "tgtfield":
				//    if (valAsString == GetNoMappingText() || valAsString == string.Empty)
				//    {
				//        mapping.Field = null;
				//        mapping.IsParsed = false;
				//        mapping.IsInterlinear = false;
				//    }
				//    else
				//    {
				//        mapping.Field =
				//            m_potentialFields.SingleOrDefault(f => f.DisplayName == valAsString) ??
				//            new PaField(valAsString, GetTypeAtOrDefault(e.RowIndex));
				//    }

				//    break;

				case "fwws":
					mapping.FwWritingSystem = valAsString;
					break;
			}

			InvalidateRow(e.RowIndex);
		}
	}
}
