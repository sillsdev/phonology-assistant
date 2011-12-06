using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SilTools;

namespace SIL.Pa.Model
{
	/// ----------------------------------------------------------------------------------------
	public class GridLayoutInfo
	{
		public DataGridViewCellBorderStyle GridLines = DataGridViewCellBorderStyle.Single;
		public int ColHeaderHeight = -1;
		public bool SaveReorderedCols = true;
		public bool SaveAdjustedColHeaderHeight = true;
		public bool SaveAdjustedColWidths = true;
		public bool AutoAdjustPhoneticCol = true;
		public int AutoAjustedMaxWidth = 200;

		private PaProject _project;

		public const string kFiltersFilePrefix = "GridLayoutInfo.xml";
		
		/// ------------------------------------------------------------------------------------
		public GridLayoutInfo()
		{
		}

		/// ------------------------------------------------------------------------------------
		public GridLayoutInfo(PaProject project)
		{
			_project = project;
		}

		/// ------------------------------------------------------------------------------------
		public static GridLayoutInfo Load(PaProject project)
		{
			var filename = project.ProjectPathFilePrefix + kFiltersFilePrefix;

			var info = XmlSerializationHelper.DeserializeFromFile<GridLayoutInfo>(filename);
			if (info == null)
				info = new GridLayoutInfo(project);
			else
				info._project = project;

			return info;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the specified grid with values from the save layout information.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeGrid(DataGridView grid)
		{
			if (grid == null || grid.Columns.Count == 0 || _project == null)
				return;

			SilHierarchicalGridColumn.ShowHierarchicalColumns(grid, false, true, false);
			grid.CellBorderStyle = GridLines;

			// Set the column properties to the saved values.
			foreach (var col in grid.Columns.Cast<DataGridViewColumn>())
			{
				var field = _project.GetFieldForName(col.Name);
				if (field == null)
					continue;

				if (field.DisplayIndexInGrid < 0)
					col.Visible = false;
				else
				{
					col.Visible = field.VisibleInGrid;
					col.DisplayIndex =
						(field.DisplayIndexInGrid < grid.Columns.Count ?
						field.DisplayIndexInGrid : grid.Columns.Count - 1);
				}
			}

			SilHierarchicalGridColumn.ShowHierarchicalColumns(grid, true, false, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves some of the specified grid's properties.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(DataGridView grid)
		{
			if (grid == null || grid.Columns.Count == 0)
				return;

			GridLines = grid.CellBorderStyle;

			if (SaveAdjustedColHeaderHeight)
				ColHeaderHeight = grid.ColumnHeadersHeight;

			// Save the list of columns sorted by display index.
			var displayIndexes = new SortedList<int, PaField>();

			// Save the specified grid's column properties.
			foreach (var col in grid.Columns.Cast<DataGridViewColumn>())
			{
				var field = _project.GetFieldForName(col.Name);
				if (field != null)
				{
					if (SaveAdjustedColWidths)
						field.WidthInGrid = col.Width;

					if (SaveReorderedCols)
						displayIndexes[col.DisplayIndex] = field;
				}
			}

			if (displayIndexes.Count > 0)
			{
				// The display index order saved with the fields should begin with zero, but
				// since the grid may have some SilHerarchicalColumns showing, the first field's
				// display index may be greater than 1. Therefore, we adjust for that by setting
				// the display indexes in sequence beginning from zero.
				int i = 0;
				foreach (var field in displayIndexes.Values)
					field.DisplayIndexInGrid = i++;
			}

			var filename = _project.ProjectPathFilePrefix + kFiltersFilePrefix;
			XmlSerializationHelper.SerializeToFile(filename, this);
			PaField.SaveProjectFields(_project);
		}
	}
}
