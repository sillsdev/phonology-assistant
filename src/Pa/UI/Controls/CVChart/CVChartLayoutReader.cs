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
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Localization;

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

		/// ------------------------------------------------------------------------------------
		public static string LocalizeCVChartLabel(string enLabel)
		{
			// The text for chart labels originates 
			if (LocalizationManager.UILanguageId == LocalizationManager.kDefaultLang)
				return enLabel;

			switch (enLabel)
			{
				case "bilabial": return LocalizationManager.GetString("Views.ConsonantChart.Labels.bilabial", "bilabial");
				case "labiodental": return LocalizationManager.GetString("Views.ConsonantChart.Labels.labiodental", "labiodental");
				case "linguolabial": return LocalizationManager.GetString("Views.ConsonantChart.Labels.linguolabial", "linguolabial");
				case "dental": return LocalizationManager.GetString("Views.ConsonantChart.Labels.dental", "dental");
				case "dental/alveolar": return LocalizationManager.GetString("Views.ConsonantChart.Labels.dental_alveolar", "dental/alveolar");
				case "alveolar": return LocalizationManager.GetString("Views.ConsonantChart.Labels.alveolar", "alveolar");
				case "postalveolar": return LocalizationManager.GetString("Views.ConsonantChart.Labels.postalveolar", "postalveolar");
				case "retroflex": return LocalizationManager.GetString("Views.ConsonantChart.Labels.retroflex", "retroflex");
				case "alveolo-palatal": return LocalizationManager.GetString("Views.ConsonantChart.Labels.alveolo-palatal", "alveolo-palatal");
				case "palatal": return LocalizationManager.GetString("Views.ConsonantChart.Labels.palatal", "palatal");
				case "velar": return LocalizationManager.GetString("Views.ConsonantChart.Labels.velar", "velar");
				case "uvular": return LocalizationManager.GetString("Views.ConsonantChart.Labels.uvular", "uvular");
				case "pharyngeal": return LocalizationManager.GetString("Views.ConsonantChart.Labels.pharyngeal", "pharyngeal");
				case "epiglottal": return LocalizationManager.GetString("Views.ConsonantChart.Labels.epiglottal", "epiglottal");
				case "glottal": return LocalizationManager.GetString("Views.ConsonantChart.Labels.glottal", "glottal");
				case "implosive": return LocalizationManager.GetString("Views.ConsonantChart.Labels.implosive", "implosive");
				case "plosive": return LocalizationManager.GetString("Views.ConsonantChart.Labels.plosive", "plosive");
				case "click": return LocalizationManager.GetString("Views.ConsonantChart.Labels.click", "click");
				case "affricate": return LocalizationManager.GetString("Views.ConsonantChart.Labels.affricate", "affricate");
				case "nasal": return LocalizationManager.GetString("Views.ConsonantChart.Labels.nasal", "nasal");
				case "trill": return LocalizationManager.GetString("Views.ConsonantChart.Labels.trill", "trill");
				case "tap": return LocalizationManager.GetString("Views.ConsonantChart.Labels.tap", "tap");
				case "flap": return LocalizationManager.GetString("Views.ConsonantChart.Labels.flap", "flap");
				case "fricative": return LocalizationManager.GetString("Views.ConsonantChart.Labels.fricative", "fricative");
				case "approximant": return LocalizationManager.GetString("Views.ConsonantChart.Labels.approximant", "approximant");
				case "close": return LocalizationManager.GetString("Views.VowelChart.Labels.close", "close");
				case "near-close": return LocalizationManager.GetString("Views.VowelChart.Labels.near-close", "near-close");
				case "close-mid": return LocalizationManager.GetString("Views.VowelChart.Labels.close-mid", "close-mid");
				case "mid": return LocalizationManager.GetString("Views.VowelChart.Labels.mid", "mid");
				case "open-mid": return LocalizationManager.GetString("Views.VowelChart.Labels.open-mid", "open-mid");
				case "near-open": return LocalizationManager.GetString("Views.VowelChart.Labels.near-open", "near-open");
				case "open": return LocalizationManager.GetString("Views.VowelChart.Labels.open", "open");
				case "front": return LocalizationManager.GetString("Views.VowelChart.Labels.front", "front");
				case "near-front": return LocalizationManager.GetString("Views.VowelChart.Labels.near-front", "near-front");
				case "central": return LocalizationManager.GetString("Views.VowelChart.Labels.central", "central");
				case "near-back": return LocalizationManager.GetString("Views.VowelChart.Labels.near-back", "near-back");
				case "back": return LocalizationManager.GetString("Views.VowelChart.Labels.back", "back");
			}

			return enLabel;
		}
	}
}
