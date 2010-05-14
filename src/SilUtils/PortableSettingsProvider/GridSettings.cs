using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SilUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("gridSettings")]
	public class GridSettings
	{
		[XmlElement("columnHeaderHeight")]
		public int ColumnHeaderHeight { get; set; }
		[XmlElement("dpi")]
		public float DPI { get; set; }
		[XmlArray("columns"), XmlArrayItem("col")]
		public GridColumnSettings[] Columns { get; set; }

		private readonly float m_currDpi;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public GridSettings()
		{
			ColumnHeaderHeight = -1;
			Columns = new GridColumnSettings[] { };
			using (Form frm = new Form())
			using (Graphics g = frm.CreateGraphics())
				m_currDpi = DPI = g.DpiX;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static GridSettings Create(DataGridView grid)
		{
			var gridSettings = new GridSettings();

			gridSettings.ColumnHeaderHeight = grid.ColumnHeadersHeight;

			var colSettings = new List<GridColumnSettings>();

			foreach (DataGridViewColumn col in grid.Columns)
			{
				colSettings.Add(new GridColumnSettings
				{
					Id = col.Name,
					Width = col.Width,
					Visible = col.Visible,
					DisplayIndex = col.DisplayIndex
				});
			}

			gridSettings.Columns = colSettings.ToArray();
			return gridSettings;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeGrid(DataGridView grid)
		{
			foreach (var col in Columns)
			{
				grid.Columns[col.Id].Visible = col.Visible;

				if (col.Width >= 0)
					grid.Columns[col.Id].Width = col.Width;

				if (col.DisplayIndex >= 0)
					grid.Columns[col.Id].DisplayIndex = col.DisplayIndex;
			}

			// If the column header height or the former dpi settings are different,
			// then auto. calculate the height of the column headings.
			if (ColumnHeaderHeight <= 0 || DPI != m_currDpi)
				grid.AutoResizeColumnHeadersHeight();
			else
				grid.ColumnHeadersHeight = ColumnHeaderHeight;
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class GridColumnSettings
	{
		[XmlAttribute("id")]
		public string Id { get; set; }
		[XmlAttribute("visible")]
		public bool Visible { get; set; }
		[XmlAttribute("width")]
		public int Width { get; set; }
		[XmlAttribute("displayIndex")]
		public int DisplayIndex { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal GridColumnSettings()
		{
			Visible = true;
			Width = -1;
			DisplayIndex = -1;
		}
	}
}
