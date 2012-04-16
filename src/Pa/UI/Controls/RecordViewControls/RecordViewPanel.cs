using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SilTools.Controls;

namespace SIL.Pa.UI.Controls.RecordViewControls
{
	public class RecordViewPanel : SilPanel, IRecordView
	{
		private readonly RtfRecordView _rtfView;
		private readonly HtmlRecordView _htmlView;

		/// ------------------------------------------------------------------------------------
		public RecordViewPanel()
		{
			Padding = new Padding(4, 4, 0, 0);

			_rtfView = new RtfRecordView();
			_rtfView.Visible = false;
			_rtfView.Dock = DockStyle.Fill;
			_rtfView.BackColor = SystemColors.Window;
			_rtfView.BorderStyle = BorderStyle.None;
			_rtfView.ReadOnly = true;
			_rtfView.WordWrap = false;

			_htmlView = new HtmlRecordView();
			_htmlView.Visible = false;
			_htmlView.Dock = DockStyle.Fill;

			Controls.Add(_rtfView);
			Controls.Add(_htmlView);
		}

		/// ------------------------------------------------------------------------------------
		public void Clear()
		{
			_htmlView.Visible = false;
			_rtfView.Clear();
			_rtfView.Visible = true;
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateFonts()
		{
			if (_rtfView.Visible)
				_rtfView.UpdateFonts();
			else
				_htmlView.UpdateFonts();
		}

		/// ------------------------------------------------------------------------------------
		public void ForceUpdate()
		{
			UpdateFonts();
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateRecord(Model.RecordCacheEntry entry)
		{
			UpdateRecord(entry, false);
		}

		/// ------------------------------------------------------------------------------------
		public void UpdateRecord(Model.RecordCacheEntry entry, bool forceUpdate)
		{
			if (entry != null && entry.InterlinearFields != null && entry.InterlinearFields.Count > 1)
			{
				_htmlView.Visible = false;
				_rtfView.UpdateRecord(entry, forceUpdate);
				_rtfView.Visible = true;
			}
			else
			{
				_rtfView.Visible = false;
				_htmlView.UpdateRecord(entry, forceUpdate);
				_htmlView.Visible = true;
			}
		}
	}
}
