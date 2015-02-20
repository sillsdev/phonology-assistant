// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2005-2015, SIL International.
// <copyright from='2005' to='2015' company='SIL International'>
//		Copyright (c) 2005-2015, SIL International.
//    
//		This software is distributed under the MIT License, as specified in the LICENSE.txt file.
// </copyright> 
#endregion
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Localization;
using SIL.Pa.Model;
using SIL.Pa.Properties;
using SilTools;

namespace SIL.Pa.UI.Controls
{
	/// ----------------------------------------------------------------------------------------
	public class HtmlRecordView : WebBrowser
	{
		private RecordCacheEntry _recEntry;

		/// ------------------------------------------------------------------------------------
		public HtmlRecordView()
		{
			AllowWebBrowserDrop = false;
		}

		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			var bodyElement = new XElement("p", new XAttribute("class", "field"),
				LocalizationManager.GetString("Views.WordLists.RtfRecordView.EmtpyView", "(no data)",
					"What's displayed in the record view when there is no data."));

			SetDocumentText(new PaField[0], bodyElement);
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateFonts()
		{
			UpdateRecord(_recEntry, true);
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateRecord(RecordCacheEntry entry, bool forceUpdate)
		{
			if (entry == null)
			{
				Clear();
				return;
			}

			// Don't bother rebuilding the RTF if the record hasn't changed.
			if (entry == _recEntry && !forceUpdate)
				return;

			_recEntry = entry;
			var fieldsAndValues = GetFieldsAndValuesToDisplay(entry);
			SetDocumentText(fieldsAndValues.Keys, CreateBodyElement(fieldsAndValues));
		}

		/// ------------------------------------------------------------------------------------
		private void SetDocumentText(IEnumerable<PaField> fields, XElement bodyElement)
		{
			var stylesheet = GetStyleSheet(fields);

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
				bldr.AppendFormat(".{0} {{ font-size: {1}pt; font-family: '{2}'; padding-right: 13px; }}",
					field.Name, (int)field.Font.SizeInPoints, field.Font.Name);
			}

			return bldr.ToString();
		}
	}
}
