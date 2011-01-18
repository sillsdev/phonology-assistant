using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SIL.PaToFdoInterfaces;
using SIL.ObjectBrowser;
using WeifenLuo.WinFormsUI.Docking;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This program allows the user to browse the data pulled from an FW database for use in
	/// Phonology Assistant.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PaToFdoBrowser : ObjectBrowser.ObjectBrowser
	{
		private PaFieldWorksHelper _paFwHelper;

		/// ------------------------------------------------------------------------------------
		public PaToFdoBrowser()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		protected override IInspectorList GetNewInspectorList()
		{
			return new PaToFdoInspectorList();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleFileOpenClick(object sender, EventArgs e)
		{
			string name;
			string server;
			Rectangle rcDlgBounds = Rectangle.Empty;
			int dlgSplitterPos = -1;
			var tmpHelper = new PaFieldWorksHelper();
			if (!tmpHelper.ShowFwOpenProject(this, ref rcDlgBounds,
				ref dlgSplitterPos, out name, out server))
			{
				tmpHelper.Dispose();
				return;
			}

			if (_paFwHelper != null)
				_paFwHelper.Dispose();

			_paFwHelper = tmpHelper;

			Cursor = Cursors.WaitCursor;
			try
			{
				// Close any windows from a previously opened project, if there is one.
				foreach (IDockContent dc in m_dockPanel.DocumentsToArray())
				{
					if (dc is InspectorWnd)
						((InspectorWnd)dc).Close();
				}

				if (!_paFwHelper.Initialize(name, server, 2000, 20000))
				{
					MessageBox.Show("There was a problem getting data from FieldWorks.");
					return;
				}

				var fifp = new FdoInfoForPa
				{
					LexEntries = _paFwHelper.LexEntries.ToList(),
					WritingSystems = _paFwHelper.WritingSystems.ToList()
				};

				var caption = Path.GetFileName(name);
				MakeAppCaption(caption);
				ShowNewInspectorWindow(fifp);
				base.OpenFile(name);
				m_currOpenedProject = name;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class FdoInfoForPa
	{
		public List<IPaLexEntry> LexEntries { get; set; }
		public List<IPaWritingSystem> WritingSystems { get; set; }
	}
}
