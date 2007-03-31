using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.Pa.Controls;
using SIL.Pa.Data;
using SIL.SpeechTools.Utils;

namespace SIL.Pa.Dialogs
{
	/// <summary>
	/// Summary description for RtfExportDlg.
	/// </summary>
	public class RtfExportDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox grpTarget;
		private System.Windows.Forms.GroupBox grpFormat;
		private System.Windows.Forms.RadioButton rbToClipboard;
		private System.Windows.Forms.RadioButton rbToFileOpen;
		private System.Windows.Forms.RadioButton rbToFile;
		private System.Windows.Forms.RadioButton rbFmtTabDelim;
		private System.Windows.Forms.RadioButton rbFmtTable;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSetEditor;

		// Declare member variables
		private string m_rtfEditor;
		private PaWordListGrid m_grid;
		private RtfCreator.ExportTarget m_exportTarget;
		private RtfCreator.ExportFormat m_exportFormat;
		private Button btnHelp;
		private Panel panel1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Constructor & Closing
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// RtfExportDlg constructor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public RtfExportDlg(PaWordListGrid grid)
		{
			InitializeComponent();

			m_grid = grid;
			SetUiFonts();

			rbToFile.Tag = RtfCreator.ExportTarget.File;
			rbToFileOpen.Tag = RtfCreator.ExportTarget.FileAndOpen;
			rbToClipboard.Tag = RtfCreator.ExportTarget.Clipboard;
			rbFmtTable.Tag = RtfCreator.ExportFormat.Table;
			rbFmtTabDelim.Tag = RtfCreator.ExportFormat.TabDelimited;

			// Load saved window settings
			m_rtfEditor = PaApp.SettingsHandler.GetStringSettingsValue(Name, "editor", null);
			rbToFile.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "file", true);
			rbToFileOpen.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "fileopen", false);
			rbToClipboard.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "clipboard", false);
			rbFmtTable.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "table", true);
			rbFmtTabDelim.Checked = PaApp.SettingsHandler.GetBoolSettingsValue(Name, "tabdelim", false);

			Size savSize = Size;
			PaApp.SettingsHandler.LoadFormProperties(this);
			Size = savSize;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set UI Fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetUiFonts()
		{
			grpTarget.Font = FontHelper.UIFont;
			rbToFile.Font = FontHelper.UIFont;
			rbToFileOpen.Font = FontHelper.UIFont;
			rbToClipboard.Font = FontHelper.UIFont;
			grpFormat.Font = FontHelper.UIFont;
			rbFmtTable.Font = FontHelper.UIFont;
			rbFmtTabDelim.Font = FontHelper.UIFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// RtfExportDlg Closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			// Save settings
			PaApp.SettingsHandler.SaveSettingsValue(Name, "editor", m_rtfEditor);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "file", rbToFile.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "fileopen", rbToFileOpen.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "clipboard", rbToClipboard.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "table", rbFmtTable.Checked);
			PaApp.SettingsHandler.SaveSettingsValue(Name, "tabdelim", rbFmtTabDelim.Checked);
			PaApp.SettingsHandler.SaveFormProperties(this);
			
			base.OnClosing(e);
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RtfExportDlg));
			this.grpTarget = new System.Windows.Forms.GroupBox();
			this.rbToClipboard = new System.Windows.Forms.RadioButton();
			this.rbToFileOpen = new System.Windows.Forms.RadioButton();
			this.rbToFile = new System.Windows.Forms.RadioButton();
			this.grpFormat = new System.Windows.Forms.GroupBox();
			this.rbFmtTabDelim = new System.Windows.Forms.RadioButton();
			this.rbFmtTable = new System.Windows.Forms.RadioButton();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSetEditor = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grpTarget.SuspendLayout();
			this.grpFormat.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpTarget
			// 
			this.grpTarget.Controls.Add(this.rbToClipboard);
			this.grpTarget.Controls.Add(this.rbToFileOpen);
			this.grpTarget.Controls.Add(this.rbToFile);
			resources.ApplyResources(this.grpTarget, "grpTarget");
			this.grpTarget.Name = "grpTarget";
			this.grpTarget.TabStop = false;
			// 
			// rbToClipboard
			// 
			resources.ApplyResources(this.rbToClipboard, "rbToClipboard");
			this.rbToClipboard.Name = "rbToClipboard";
			this.rbToClipboard.CheckedChanged += new System.EventHandler(this.HandleExportTargetSelected);
			// 
			// rbToFileOpen
			// 
			resources.ApplyResources(this.rbToFileOpen, "rbToFileOpen");
			this.rbToFileOpen.Name = "rbToFileOpen";
			this.rbToFileOpen.CheckedChanged += new System.EventHandler(this.HandleExportTargetSelected);
			// 
			// rbToFile
			// 
			resources.ApplyResources(this.rbToFile, "rbToFile");
			this.rbToFile.Name = "rbToFile";
			this.rbToFile.CheckedChanged += new System.EventHandler(this.HandleExportTargetSelected);
			// 
			// grpFormat
			// 
			resources.ApplyResources(this.grpFormat, "grpFormat");
			this.grpFormat.Controls.Add(this.rbFmtTabDelim);
			this.grpFormat.Controls.Add(this.rbFmtTable);
			this.grpFormat.Name = "grpFormat";
			this.grpFormat.TabStop = false;
			// 
			// rbFmtTabDelim
			// 
			resources.ApplyResources(this.rbFmtTabDelim, "rbFmtTabDelim");
			this.rbFmtTabDelim.Name = "rbFmtTabDelim";
			this.rbFmtTabDelim.CheckedChanged += new System.EventHandler(this.HandleExportFormatSelected);
			// 
			// rbFmtTable
			// 
			resources.ApplyResources(this.rbFmtTable, "rbFmtTable");
			this.rbFmtTable.Name = "rbFmtTable";
			this.rbFmtTable.CheckedChanged += new System.EventHandler(this.HandleExportFormatSelected);
			// 
			// btnExport
			// 
			resources.ApplyResources(this.btnExport, "btnExport");
			this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnExport.Name = "btnExport";
			this.btnExport.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnExport_MouseClick);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			// 
			// btnSetEditor
			// 
			resources.ApplyResources(this.btnSetEditor, "btnSetEditor");
			this.btnSetEditor.Name = "btnSetEditor";
			this.btnSetEditor.Click += new System.EventHandler(this.btnSetEditor_Click);
			// 
			// btnHelp
			// 
			resources.ApplyResources(this.btnHelp, "btnHelp");
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnSetEditor);
			this.panel1.Controls.Add(this.btnExport);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnHelp);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// RtfExportDlg
			// 
			this.AcceptButton = this.btnExport;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.grpFormat);
			this.Controls.Add(this.grpTarget);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RtfExportDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.grpTarget.ResumeLayout(false);
			this.grpTarget.PerformLayout();
			this.grpFormat.ResumeLayout(false);
			this.grpFormat.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Events

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the ExportType selected event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleExportTargetSelected(object sender, EventArgs e)
		{
			RadioButton rb = sender as RadioButton;
			if (rb != null)
			{
				m_exportTarget = (RtfCreator.ExportTarget)rb.Tag;
				if (m_exportTarget == RtfCreator.ExportTarget.FileAndOpen)
				{
					btnSetEditor.Enabled = true;
					if (!File.Exists(m_rtfEditor))
						btnExport.Enabled = false;
				}
				else
				{
					btnSetEditor.Enabled = false;
					btnExport.Enabled = true;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handle the ExportFormat selected event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleExportFormatSelected(object sender, EventArgs e)
		{
			RadioButton rb = sender as RadioButton;
			if (rb != null)
				m_exportFormat = (RtfCreator.ExportFormat)rb.Tag;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Export the PaWordListGrid query in Rtf format.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnExport_MouseClick(object sender, MouseEventArgs e)
		{
			new RtfCreator(m_exportTarget, m_exportFormat, m_grid, m_grid.Cache, m_rtfEditor);
			Close();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Select the Rtf editor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnSetEditor_Click(object sender, EventArgs e)
		{
			// clear the status bar
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.Title = "Set RTF Editor...";
			dlg.Filter = "Exe File (*.exe)|*.EXE";
			// Set the initial directory to "C:\Program Files"
			dlg.InitialDirectory = (Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).ToString();
			dlg.Multiselect = false;
			dlg.ShowReadOnly = false;
			dlg.FilterIndex = 1;
			dlg.ValidateNames = true;
			dlg.ShowDialog(this);
			if (dlg.FileName.Length > 0)
			{
				m_rtfEditor = dlg.FileName;
				btnExport.Enabled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			PaApp.ShowHelpTopic(this);
		}

		#endregion
	}
}
