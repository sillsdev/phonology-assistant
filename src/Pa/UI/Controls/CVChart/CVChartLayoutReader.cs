using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace SIL.Pa.UI.Controls
{
	public class CVChartLayoutReader
	{
		public Dictionary<string, int> ColHeadings { get; private set; }
		public Dictionary<string, int> RowHeadings { get; private set; }
		public Dictionary<string, Point> Phones { get; private set; }

		/// ------------------------------------------------------------------------------------
		public CVChartLayoutReader(string filename)
		{
			ColHeadings = new Dictionary<string, int>();
			RowHeadings = new Dictionary<string, int>();
			Phones = new Dictionary<string, Point>();
			
			var root = XElement.Load(filename);
			ReadColumnHeadings(root);
			ReadRowsHeadings(root);
		}

		/// ------------------------------------------------------------------------------------
		private XName GetTag(string tagName)
		{
			return XName.Get(tagName, "http://www.w3.org/1999/xhtml");
		}

		/// ------------------------------------------------------------------------------------
		private void ReadColumnHeadings(XElement elements)
		{
			XElement theadRow;
			try { theadRow = elements.Element(GetTag("body")).Element(GetTag("table")).Element(GetTag("thead")).Element(GetTag("tr")); }
			catch { return; }

			foreach (var th in theadRow.Elements(GetTag("th")).Where(e => (string)e.Attribute("scope") == "colgroup"))
			{
				int subColCount;
				if (int.TryParse((string)th.Attribute("colspan"), out subColCount))
					ColHeadings[th.Value] = subColCount;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ReadRowsHeadings(XElement element)
		{
			IEnumerable<XElement> tbodies;
			try { tbodies = element.Element(GetTag("body")).Element(GetTag("table")).Elements(GetTag("tbody")); }
			catch { return; }

			int row = 0;

			foreach (var tbody in tbodies)
			{
				int subRowCount = 0;
				var headingText = string.Empty;
				
				foreach (var tr in tbody.Elements(GetTag("tr")))
				{
					subRowCount++;
					int col = 0;
					
					if (tr.Element(GetTag("th")).Value != string.Empty)
						headingText = tr.Element(GetTag("th")).Value;

					foreach (var td in tr.Elements(GetTag("td")))
					{
						if (td.Value != string.Empty)
							Phones[td.Value] = new Point(col, row);
						col++;
					}

					row++;
				}
			
				RowHeadings[headingText] = subRowCount;
			}
		}
	}
}
