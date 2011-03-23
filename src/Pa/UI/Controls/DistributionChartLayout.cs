using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.Pa.PhoneticSearching;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	[XmlType("XYChart")]
	public class DistributionChartLayout
	{
		private string m_name;

		public DistributionChartLayout()
		{
			ColumnWidths = new List<int>();
			SearchQueries = new List<SearchQuery>();
			SearchItems = new List<string>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new DistributionChartLayout object from the specified DistributionGrid.
		/// If the grid is null or empty, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static DistributionChartLayout NewFromDistributionGrid(DistributionGrid grid)
		{
			if (grid == null || (grid.Rows.Count <= 1 && grid.Columns.Count <= 1))
				return null;

			DistributionChartLayout layout = new DistributionChartLayout();
			layout.UpdateFromDistributionGrid(grid);
			return layout;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the DistributionChartLayout with the information from the specified grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFromDistributionGrid(DistributionGrid grid)
		{
			if (grid == null)
				return;

			Reset();

			// Save the search items.
			for (int i = 1; i < grid.Rows.Count; i++)
			{
				if (i != grid.NewRowIndex)
					SearchItems.Add(grid[0, i].Value as string);
			}

			// Add the search item column.
			ColumnWidths.Add(grid.Columns[0].Width);

			// Save the environments and column widths.
			for (int i = 1; i < grid.Columns.Count - 1; i++)
			{
				ColumnWidths.Add(grid.Columns[i].Width);
				var query = grid.Columns[i].Tag as SearchQuery;
				if (query != null)
					SearchQueries.Add(query);
			}

			// Add the new environment column (this is like the column equivalent of the new row).
			ColumnWidths.Add(grid.Columns[grid.Columns.Count - 1].Width);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an exact copy of the layout.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DistributionChartLayout Clone()
		{
			var clone = new DistributionChartLayout();

			clone.m_name = m_name;
			
			foreach (var srchItem in SearchItems)
				clone.SearchItems.Add(srchItem);

			foreach (var query in SearchQueries)
				clone.SearchQueries.Add(query.Clone());

			foreach (int width in ColumnWidths)
				clone.ColumnWidths.Add(width);

			return clone;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the layout if it's not empty or null or "(none)" if it is.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string NameOrNone
		{
			get
			{
				return (string.IsNullOrEmpty(Name) ?
					App.GetString("DistributionChartLayout.EmptyName", "(none)", "Views") : Name);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the name of the layout.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlAttribute]
		public string Name
		{
			get { return (m_name == null ? null : m_name.Trim()); }
			set { m_name = value; }
		}

		/// ------------------------------------------------------------------------------------
		public List<string> SearchItems { get; set; }

		/// ------------------------------------------------------------------------------------
		public List<SearchQuery> SearchQueries { get; set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> Environments
		{
			get
			{
				List<string> environments = new List<string>();
				foreach (SearchQuery query in SearchQueries)
					environments.Add(query.Pattern);

				return environments;
			}
		}

		/// ------------------------------------------------------------------------------------
		public List<int> ColumnWidths { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear's the internal search items and queries lists. The XYCharLayout's name is
		/// preserved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			SearchItems.Clear();
			SearchQueries.Clear();
			ColumnWidths.Clear();
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_name;
		}
	}
}
