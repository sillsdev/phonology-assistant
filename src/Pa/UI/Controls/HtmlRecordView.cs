using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	public class HtmlRecordView : WebBrowser, IRecordView
	{
		/// ------------------------------------------------------------------------------------
		public HtmlRecordView()
		{
			AllowWebBrowserDrop = false;
		}

		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			DocumentText = new XElement("html").ToString();

			//Url = new Uri("about:blank");
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateFonts()
		{
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateRecord(RecordCacheEntry entry)
		{
			UpdateRecord(entry, false);
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateRecord(RecordCacheEntry entry, bool forceUpdate)
		{
			if (entry == null)
			{
				Clear();
				return;
			}

			var fieldsAndValues = GetFieldsAndValuesToDisplay(entry);
			var bodyElement = CreateBodyElement(fieldsAndValues);
			var stylesheet = GetStyleSheet(fieldsAndValues.Keys);

			XNamespace ns = "http://www.w3.org/1999/xhtml";
			var root = new XElement(ns + "html", new XAttribute("lang", "en"),
				CreateHeaderElement(stylesheet), bodyElement);

			DocumentText = root.ToString();
		}

		/// ------------------------------------------------------------------------------------
		private IDictionary<PaField, string> GetFieldsAndValuesToDisplay(RecordCacheEntry recEntry)
		{
			var dict = new Dictionary<PaField, string>();

			foreach (var field in App.Project.Fields.OrderBy(f => f.DisplayIndexInRecView))
			{
				var fieldValue = recEntry[field.Name];

				if (field.VisibleInRecView && field.DisplayIndexInRecView >= 0 &&
					fieldValue != null && !recEntry.GetIsInterlinearField(field.Name))
				{
					dict[field] = fieldValue;
				}
			}

			return dict;
		}

		/// ------------------------------------------------------------------------------------
		private XElement CreateBodyElement(IDictionary<PaField, string> fieldsAndValues)
		{
			var tableBodyElement = new XElement("tbody", CreateRowElements(fieldsAndValues));
			return new XElement("body", new XElement("table", tableBodyElement));
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<XElement> CreateRowElements(IDictionary<PaField, string> fieldsAndValues)
		{
			var cellElements = CreateCellElements(fieldsAndValues).ToArray();

			for (int i = 0; i < cellElements.Length; )
			{
				var rowElement = new XElement("tr", cellElements[i++], cellElements[i++]);
				if (i < cellElements.Length)
					rowElement.Add(cellElements[i++], cellElements[i++]);

				yield return rowElement;
			}
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<XElement> CreateCellElements(IDictionary<PaField, string> fieldsAndValues)
		{
			foreach (var kvp in OrderFieldsAndValuesForColumnarDisplay(fieldsAndValues))
			{
				yield return new XElement("td", new XAttribute("class", "field"), kvp.Key.DisplayName + ":");
				yield return new XElement("td", new XAttribute("class", kvp.Key.Name), kvp.Value);
			}
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<KeyValuePair<PaField, string>> OrderFieldsAndValuesForColumnarDisplay(IDictionary<PaField, string> fieldsAndValues)
		{
			var kvpList = fieldsAndValues.ToArray();
			int rowsInCol = (int)Math.Ceiling(kvpList.Length / 2d);

			for (int i = 0; i < rowsInCol; i++)
			{
				yield return kvpList[i];
				if (i + rowsInCol < kvpList.Length)
					yield return kvpList[i + rowsInCol];
			}
		}
	
		/// ------------------------------------------------------------------------------------
		private XElement CreateHeaderElement(string cssString)
		{
			var meta1 = new XElement("meta",
				new XAttribute("http-equiv", "content-type"),
				new XAttribute("content", "text/html; charset=utf-8"));

			var meta2 = new XElement("meta",
				new XAttribute("http-equiv", "X-UA-Compatible"),
				new XAttribute("content", "IE=edge"));

			var stylesheet = new XElement("style",
				new XAttribute("type", "text/css"), cssString); 

			return new XElement("head", meta1, meta2,
				new XElement("title", "PA Record View"), stylesheet);
		}

		/// ------------------------------------------------------------------------------------
		private string GetStyleSheet(IEnumerable<PaField> fieldsToDisplay)
		{
			var bldr = new StringBuilder();

			// Add styles to reset default values of browsers
			bldr.AppendLine("* { font-size: 100%; margin: 0; border: 0;	padding: 0; }");
			bldr.AppendLine("table {border-spacing: 0; }");
			bldr.AppendLine("body { line-height: 1.2; }");

			bldr.Append(".field { font-weight: bold; padding-left: 10px; padding-right: 10px; ");
			bldr.AppendFormat("font-size: {0}pt; font-family: '{1}'; color: #{2}; }}",
				(int)FontHelper.UIFont.SizeInPoints, FontHelper.UIFont.Name,
				(Settings.Default.RecordViewFieldLabelColor.ToArgb() & 0x00FFFFFF).ToString("x"));

			foreach (var field in fieldsToDisplay)
			{
				bldr.AppendFormat(".{0} {{ font-size: {1}pt; font-family: '{2}'; padding-right: 10px; }}",
					field.Name, (int)field.Font.SizeInPoints, field.Font.Name);
			}

			return bldr.ToString();
		}
	}
}
