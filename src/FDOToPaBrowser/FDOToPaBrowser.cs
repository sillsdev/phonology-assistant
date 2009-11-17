using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SIL.FdoToPaInterfaces;
using SIL.ObjectBrowser;
using WeifenLuo.WinFormsUI.Docking;

namespace SIL.Pa
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This program allows the user to browse the data pulled from a FW database for use in
	/// Phonology Assistant.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class FDOToPaBrowser : ObjectBrowser.ObjectBrowser
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="FDOToPaBrowser"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public FDOToPaBrowser()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the title for the open file dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string OpenFileDlgTitle
		{
			get { return "Open Fieldworks Language Project"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the file filter for the open file dialog box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override string OpenFileDlgFilter
		{
			get { return "XML Project Files|*.xml|DB4o Project Files|*.db4o|Berkeley DB Project Files|*.bdb|Firebird Project Files|*.fdb"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Opens the specified file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OpenFile(string fileName)
		{
			if (fileName == null || !File.Exists(fileName) || m_currOpenedProject == fileName)
				return;

			Cursor = Cursors.WaitCursor;
			try
			{
				// Close any windows from a previously opened project, if there is one.
				foreach (IDockContent dc in m_dockPanel.DocumentsToArray())
				{
					if (dc is InspectorWnd)
						((InspectorWnd)dc).Close();
				}

				IPaLexicalInfo lexEntryServer = FieldWorksHelper.LexEntryServer;
				if (lexEntryServer == null)
				{
					MessageBox.Show(fileName + " cannot be found or is not a valid FieldWorks project.");
					return;
				}

				lexEntryServer.Initialize(fileName);
				var fifp = new FDOInfoForPa();
				fifp.WritingSystems = new List<IPaWritingSystem>(lexEntryServer.WritingSystems);
				fifp.LexEntries = new List<IPaLexEntry>(lexEntryServer.LexEntries);

				string caption = Path.GetFileName(fileName);
				MakeAppCaption(caption);
				ShowNewInspectorWindow(fifp);
				base.OpenFile(fileName);
				m_currOpenedProject = fileName;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FDOInfoForPa
	{
		public List<IPaWritingSystem> WritingSystems { get; set; }
		public List<IPaLexEntry> LexEntries { get; set; }
	}
}
