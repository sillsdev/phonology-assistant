using System.Collections.Generic;
using System.Xml.Serialization;
using SIL.Pa.FFSearchEngine;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("XYChart")]
	public class XYChartLayout
	{
		private string m_name;
		private List<string> m_srchItems = new List<string>();
		private List<SearchQuery> m_queries = new List<SearchQuery>();
		private List<int> m_colWidths = new List<int>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a new XYChartLayout object from the specified XYGrid. If the grid is null
		/// or empty, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static XYChartLayout NewFromXYGrid(XYGrid grid)
		{
			if (grid == null || (grid.Rows.Count <= 1 && grid.Columns.Count <= 1))
				return null;

			XYChartLayout layout = new XYChartLayout();
			layout.UpdateFromXYGrid(grid);
			return layout;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the XYChartLayout with the information from the specified grid.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateFromXYGrid(XYGrid grid)
		{
			if (grid == null)
				return;

			Reset();

			// Save the search items.
			for (int i = 1; i < grid.Rows.Count; i++)
			{
				if (i != grid.NewRowIndex)
					m_srchItems.Add(grid[0, i].Value as string);
			}

			// Add the search item column.
			m_colWidths.Add(grid.Columns[0].Width);

			// Save the environments and column widths.
			for (int i = 1; i < grid.Columns.Count - 1; i++)
			{
				m_colWidths.Add(grid.Columns[i].Width);
				SearchQuery query = grid.Columns[i].Tag as SearchQuery;
				if (query != null)
					m_queries.Add(query);
			}

			// Add the new environment column (this is like the column equivalent of the new row).
			m_colWidths.Add(grid.Columns[grid.Columns.Count - 1].Width);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an exact copy of the layout.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public XYChartLayout Clone()
		{
			XYChartLayout clone = new XYChartLayout();

			clone.m_name = m_name;
			
			foreach (string srchItem in m_srchItems)
				clone.m_srchItems.Add(srchItem);

			foreach (SearchQuery query in m_queries)
				clone.m_queries.Add(query.Clone());

			foreach (int width in m_colWidths)
				clone.m_colWidths.Add(width);

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
					Properties.Resources.kstidXYChartLayoutEmptyName : Name);
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
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<string> SearchItems
		{
			get { return m_srchItems; }
			set { m_srchItems = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<SearchQuery> SearchQueries
		{
			get { return m_queries; }
			set { m_queries = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<string> Environments
		{
			get
			{
				List<string> environments = new List<string>();
				foreach (SearchQuery query in m_queries)
					environments.Add(query.Pattern);

				return environments;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<int> ColumnWidths
		{
			get { return m_colWidths; }
			set { m_colWidths = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clear's the internal search items and queries lists. The XYCharLayout's name is
		/// preserved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Reset()
		{
			m_srchItems.Clear();
			m_queries.Clear();
			m_colWidths.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return m_name;
		}
	}
}
