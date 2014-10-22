using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Localization;
using SIL.Pa.Properties;
using SIL.Pa.UI.Controls;
using SilTools;

namespace SIL.Pa.UI.Dialogs
{
	/// <summary>
	/// Summary description for RtfExportDlg.
	/// </summary>
	public class RtfExportDlg : Form
	{
		private GroupBox grpTarget;
		private GroupBox grpFormat;
		private RadioButton rbToClipboard;
		private RadioButton rbToFileOpen;
		private RadioButton rbToFile;
		private RadioButton rbFmtTabDelim;
		private RadioButton rbFmtTable;
		private Button btnExport;
		private Button btnCancel;
		private Button btnSetEditor;

		// Declare member variables
		private string m_rtfEditor;
		private readonly PaWordListGrid m_grid;
		private RtfCreator.ExportTarget m_exportTarget;
		private RtfCreator.ExportFormat m_exportFormat;
		private Button btnHelp;
		private Panel panel1;
		private Localization.UI.LocalizationExtender locExtender;
		private IContainer components;

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
			m_rtfEditor = Settings.Default.RTFExportOptionRTFEditor;
			rbToFile.Checked = Settings.Default.RTFExportOptionToFile;
			rbToFileOpen.Checked = Settings.Default.RTFExportOptionToFileAndOpen;
			rbToClipboard.Checked = Settings.Default.RTFExportOptionToClipboard;
			rbFmtTable.Checked = Settings.Default.RTFExportOptionToTable;
			rbFmtTabDelim.Checked = Settings.Default.RTFExportOptionTabDelimited;

			App.InitializeForm(this, Settings.Default.RTFExportDlg);
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
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			App.MsgMediator.SendMessage(Name + "HandleCreated", this);

			if (Parent is Form)
				((Form)Parent).AddOwnedForm(this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// RtfExportDlg Closing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			if (Parent is Form)
				((Form)Parent).RemoveOwnedForm(this);
			
			// Save settings
			Settings.Default.RTFExportOptionRTFEditor = m_rtfEditor;
			Settings.Default.RTFExportOptionToFile = rbToFile.Checked;
			Settings.Default.RTFExportOptionToFileAndOpen = rbToFileOpen.Checked;
			Settings.Default.RTFExportOptionToClipboard = rbToClipboard.Checked;
			Settings.Default.RTFExportOptionToTable = rbFmtTable.Checked;
			Settings.Default.RTFExportOptionTabDelimited = rbFmtTabDelim.Checked;

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
			this.components = new System.ComponentModel.Container();
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
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.grpTarget.SuspendLayout();
			this.grpFormat.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// grpTarget
			// 
			this.grpTarget.Controls.Add(this.rbToClipboard);
			this.grpTarget.Controls.Add(this.rbToFileOpen);
			this.grpTarget.Controls.Add(this.rbToFile);
			this.locExtender.SetLocalizableToolTip(this.grpTarget, null);
			this.locExtender.SetLocalizationComment(this.grpTarget, "Frame around export destination options in RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.grpTarget, "DialogBoxes.RtfExportDlg.TargetGroupBox");
			this.grpTarget.Location = new System.Drawing.Point(10, 10);
			this.grpTarget.Name = "grpTarget";
			this.grpTarget.Size = new System.Drawing.Size(236, 90);
			this.grpTarget.TabIndex = 0;
			this.grpTarget.TabStop = false;
			this.grpTarget.Text = "Export To";
			// 
			// rbToClipboard
			// 
			this.rbToClipboard.AutoSize = true;
			this.rbToClipboard.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.locExtender.SetLocalizableToolTip(this.rbToClipboard, null);
			this.locExtender.SetLocalizationComment(this.rbToClipboard, "Export destination options on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.rbToClipboard, "DialogBoxes.RtfExportDlg.ToClipboardRadioBox");
			this.rbToClipboard.Location = new System.Drawing.Point(12, 60);
			this.rbToClipboard.Name = "rbToClipboard";
			this.rbToClipboard.Size = new System.Drawing.Size(69, 17);
			this.rbToClipboard.TabIndex = 2;
			this.rbToClipboard.Text = "&Clipboard";
			this.rbToClipboard.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbToClipboard.CheckedChanged += new System.EventHandler(this.HandleExportTargetSelected);
			// 
			// rbToFileOpen
			// 
			this.rbToFileOpen.AutoSize = true;
			this.rbToFileOpen.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.locExtender.SetLocalizableToolTip(this.rbToFileOpen, null);
			this.locExtender.SetLocalizationComment(this.rbToFileOpen, "Export destination options on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.rbToFileOpen, "DialogBoxes.RtfExportDlg.ToFileOpenRadioBox");
			this.rbToFileOpen.Location = new System.Drawing.Point(12, 40);
			this.rbToFileOpen.Name = "rbToFileOpen";
			this.rbToFileOpen.Size = new System.Drawing.Size(164, 17);
			this.rbToFileOpen.TabIndex = 1;
			this.rbToFileOpen.Text = "File and open with &RTF editor";
			this.rbToFileOpen.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbToFileOpen.CheckedChanged += new System.EventHandler(this.HandleExportTargetSelected);
			// 
			// rbToFile
			// 
			this.rbToFile.AutoSize = true;
			this.rbToFile.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.locExtender.SetLocalizableToolTip(this.rbToFile, null);
			this.locExtender.SetLocalizationComment(this.rbToFile, "Export destination options on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.rbToFile, "DialogBoxes.RtfExportDlg.ToFileRadioBoxes");
			this.rbToFile.Location = new System.Drawing.Point(12, 20);
			this.rbToFile.Name = "rbToFile";
			this.rbToFile.Size = new System.Drawing.Size(41, 17);
			this.rbToFile.TabIndex = 0;
			this.rbToFile.Text = "&File";
			this.rbToFile.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbToFile.CheckedChanged += new System.EventHandler(this.HandleExportTargetSelected);
			// 
			// grpFormat
			// 
			this.grpFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpFormat.Controls.Add(this.rbFmtTabDelim);
			this.grpFormat.Controls.Add(this.rbFmtTable);
			this.locExtender.SetLocalizableToolTip(this.grpFormat, null);
			this.locExtender.SetLocalizationComment(this.grpFormat, "Frame around export format options in RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.grpFormat, "DialogBoxes.RtfExportDlg.FormatGroupBox");
			this.grpFormat.Location = new System.Drawing.Point(254, 10);
			this.grpFormat.Name = "grpFormat";
			this.grpFormat.Size = new System.Drawing.Size(130, 90);
			this.grpFormat.TabIndex = 1;
			this.grpFormat.TabStop = false;
			this.grpFormat.Text = "Format";
			// 
			// rbFmtTabDelim
			// 
			this.rbFmtTabDelim.AutoSize = true;
			this.rbFmtTabDelim.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.locExtender.SetLocalizableToolTip(this.rbFmtTabDelim, null);
			this.locExtender.SetLocalizationComment(this.rbFmtTabDelim, "Export format options on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.rbFmtTabDelim, "DialogBoxes.RtfExportDlg.FormatTabDelimitedRadioButton");
			this.rbFmtTabDelim.Location = new System.Drawing.Point(12, 40);
			this.rbFmtTabDelim.Name = "rbFmtTabDelim";
			this.rbFmtTabDelim.Size = new System.Drawing.Size(90, 17);
			this.rbFmtTabDelim.TabIndex = 1;
			this.rbFmtTabDelim.Text = "Tab &Delimited";
			this.rbFmtTabDelim.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbFmtTabDelim.CheckedChanged += new System.EventHandler(this.HandleExportFormatSelected);
			// 
			// rbFmtTable
			// 
			this.rbFmtTable.AutoSize = true;
			this.rbFmtTable.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.locExtender.SetLocalizableToolTip(this.rbFmtTable, null);
			this.locExtender.SetLocalizationComment(this.rbFmtTable, "Export format options on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.rbFmtTable, "DialogBoxes.RtfExportDlg.FmtTableRadioButton");
			this.rbFmtTable.Location = new System.Drawing.Point(12, 20);
			this.rbFmtTable.Name = "rbFmtTable";
			this.rbFmtTable.Size = new System.Drawing.Size(52, 17);
			this.rbFmtTable.TabIndex = 0;
			this.rbFmtTable.Text = "&Table";
			this.rbFmtTable.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.rbFmtTable.CheckedChanged += new System.EventHandler(this.HandleExportFormatSelected);
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.btnExport, null);
			this.locExtender.SetLocalizationComment(this.btnExport, "Button on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.btnExport, "DialogBoxes.RtfExportDlg.ExportButton");
			this.btnExport.Location = new System.Drawing.Point(123, 7);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(80, 26);
			this.btnExport.TabIndex = 1;
			this.btnExport.Text = "E&xport";
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, "Button on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.btnCancel, "DialogBoxes.RtfExportDlg.CancelButton");
			this.btnCancel.Location = new System.Drawing.Point(209, 7);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 26);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// btnSetEditor
			// 
			this.locExtender.SetLocalizableToolTip(this.btnSetEditor, null);
			this.locExtender.SetLocalizationComment(this.btnSetEditor, "Button on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.btnSetEditor, "DialogBoxes.RtfExportDlg.SetEditorButton");
			this.btnSetEditor.Location = new System.Drawing.Point(0, 7);
			this.btnSetEditor.Name = "btnSetEditor";
			this.btnSetEditor.Size = new System.Drawing.Size(80, 26);
			this.btnSetEditor.TabIndex = 0;
			this.btnSetEditor.Text = "Set &Editor...";
			this.btnSetEditor.Click += new System.EventHandler(this.btnSetEditor_Click);
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnHelp, null);
			this.locExtender.SetLocalizationComment(this.btnHelp, "Button on RTF export dialog box.");
			this.locExtender.SetLocalizingId(this.btnHelp, "DialogBoxes.RtfExportDlg.HelpButton");
			this.btnHelp.Location = new System.Drawing.Point(295, 7);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(80, 26);
			this.btnHelp.TabIndex = 3;
			this.btnHelp.Text = "Help";
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnSetEditor);
			this.panel1.Controls.Add(this.btnExport);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnHelp);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(10, 115);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(374, 40);
			this.panel1.TabIndex = 2;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "Pa";
			// 
			// RtfExportDlg
			// 
			this.AcceptButton = this.btnExport;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(394, 155);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.grpFormat);
			this.Controls.Add(this.grpTarget);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.RtfExportDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RtfExportDlg";
			this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export to Rich Text Format";
			this.grpTarget.ResumeLayout(false);
			this.grpTarget.PerformLayout();
			this.grpFormat.ResumeLayout(false);
			this.grpFormat.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Events
		/// ------------------------------------------------------------------------------------
		private void HandleExportTargetSelected(object sender, EventArgs e)
		{
			RadioButton rb = sender as RadioButton;
			if (rb != null)
				m_exportTarget = (RtfCreator.ExportTarget)rb.Tag;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExportFormatSelected(object sender, EventArgs e)
		{
			var rb = sender as RadioButton;
			if (rb != null)
				m_exportFormat = (RtfCreator.ExportFormat)rb.Tag;
		}

		/// ------------------------------------------------------------------------------------
		private void btnExport_Click(object sender, EventArgs e)
		{
			//if (m_exportTarget == RtfCreator.ExportTarget.FileAndOpen && !File.Exists(m_rtfEditor))
			//{
			//    SilTools.Utils.MsgBox(Properties.Resources.kstidMissingRTFEditorMsg);
			//    return;
			//}

			new RtfCreator(App.Project, m_exportTarget, m_exportFormat,
				m_grid, m_grid.Cache, m_rtfEditor);
			
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void btnSetEditor_Click(object sender, EventArgs e)
		{
			// clear the status bar
			var dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.Title = LocalizationManager.GetString("DialogBoxes.RtfExportDlg.SetRTFEditorOpenFileDialogText",
				"Set RTF Editor...", "Caption on the open file dialog used to let the user browse the file system for the RTF editor they want PA to use when opening RTF files.");

			dlg.Filter = LocalizationManager.GetString("DialogBoxes.RtfExportDlg.SetRTFEditorOpenFileDialogFilter",
				"All Executables (*.exe;*.com;*.pif;*.bat;*.cmd)|*.exe;*.com;*.pif;*.bat;*.cmd|All Files (*.*)|*.*");

			// Default the initial directory to "C:\Program Files"
			dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

			if (!string.IsNullOrEmpty(m_rtfEditor))
			{
				var filename = Path.GetFileName(m_rtfEditor);
				var path = Path.GetDirectoryName(m_rtfEditor);
				if (!string.IsNullOrEmpty(path))
				{
					dlg.InitialDirectory = path;
					if (File.Exists(m_rtfEditor))
						dlg.FileName = filename;
				}
			}

			dlg.Multiselect = false;
			dlg.ShowReadOnly = false;
			dlg.ValidateNames = true;
			dlg.AddExtension = true;
			dlg.RestoreDirectory = true;
			dlg.FilterIndex = 1;

			if (dlg.ShowDialog(this) == DialogResult.OK && dlg.FileName.Length > 0)
				m_rtfEditor = dlg.FileName;
		}

		/// ------------------------------------------------------------------------------------
		private void btnHelp_Click(object sender, EventArgs e)
		{
			App.ShowHelpTopic(this);
		}

		#endregion
	}
}
