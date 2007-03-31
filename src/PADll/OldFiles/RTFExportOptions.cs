using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for RTFExportOptions.
	/// </summary>
	public class RTFExportOptions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbToClipboard;
		private System.Windows.Forms.RadioButton rbToFileOpen;
		private System.Windows.Forms.RadioButton rbToFile;
		private System.Windows.Forms.RadioButton rbFmtTabDelim;
		private System.Windows.Forms.RadioButton rbFmtTable;
		private System.Windows.Forms.CheckBox chkUseZero;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSetEditor;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RTFExportOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbToClipboard = new System.Windows.Forms.RadioButton();
			this.rbToFileOpen = new System.Windows.Forms.RadioButton();
			this.rbToFile = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbFmtTabDelim = new System.Windows.Forms.RadioButton();
			this.rbFmtTable = new System.Windows.Forms.RadioButton();
			this.chkUseZero = new System.Windows.Forms.CheckBox();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSetEditor = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbToClipboard);
			this.groupBox1.Controls.Add(this.rbToFileOpen);
			this.groupBox1.Controls.Add(this.rbToFile);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(208, 72);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Export RTF...";
			// 
			// rbToClipboard
			// 
			this.rbToClipboard.Location = new System.Drawing.Point(8, 48);
			this.rbToClipboard.Name = "rbToClipboard";
			this.rbToClipboard.Size = new System.Drawing.Size(104, 16);
			this.rbToClipboard.TabIndex = 2;
			this.rbToClipboard.Text = "To &Clipboard";
			// 
			// rbToFileOpen
			// 
			this.rbToFileOpen.Location = new System.Drawing.Point(8, 32);
			this.rbToFileOpen.Name = "rbToFileOpen";
			this.rbToFileOpen.Size = new System.Drawing.Size(192, 16);
			this.rbToFileOpen.TabIndex = 1;
			this.rbToFileOpen.Text = "To File and open with RTF &editor";
			// 
			// rbToFile
			// 
			this.rbToFile.Location = new System.Drawing.Point(8, 16);
			this.rbToFile.Name = "rbToFile";
			this.rbToFile.Size = new System.Drawing.Size(104, 16);
			this.rbToFile.TabIndex = 0;
			this.rbToFile.Text = "To &File";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbFmtTabDelim);
			this.groupBox2.Controls.Add(this.rbFmtTable);
			this.groupBox2.Location = new System.Drawing.Point(224, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(120, 72);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Export Format";
			// 
			// rbFmtTabDelim
			// 
			this.rbFmtTabDelim.Location = new System.Drawing.Point(8, 32);
			this.rbFmtTabDelim.Name = "rbFmtTabDelim";
			this.rbFmtTabDelim.Size = new System.Drawing.Size(104, 16);
			this.rbFmtTabDelim.TabIndex = 1;
			this.rbFmtTabDelim.Text = "Tab &Delimited";
			// 
			// rbFmtTable
			// 
			this.rbFmtTable.Location = new System.Drawing.Point(8, 16);
			this.rbFmtTable.Name = "rbFmtTable";
			this.rbFmtTable.Size = new System.Drawing.Size(104, 16);
			this.rbFmtTable.TabIndex = 0;
			this.rbFmtTable.Text = "&Table";
			// 
			// chkUseZero
			// 
			this.chkUseZero.Location = new System.Drawing.Point(304, 128);
			this.chkUseZero.Name = "chkUseZero";
			this.chkUseZero.Size = new System.Drawing.Size(168, 16);
			this.chkUseZero.TabIndex = 2;
			this.chkUseZero.Text = "Use &zero for empty values";
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(376, 8);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(64, 32);
			this.btnExport.TabIndex = 3;
			this.btnExport.Text = "E&xport";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(376, 48);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 32);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			// 
			// btnSetEditor
			// 
			this.btnSetEditor.Location = new System.Drawing.Point(344, 96);
			this.btnSetEditor.Name = "btnSetEditor";
			this.btnSetEditor.Size = new System.Drawing.Size(104, 23);
			this.btnSetEditor.TabIndex = 5;
			this.btnSetEditor.Text = "&Set RTF Editor";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(240, 56);
			this.label1.TabIndex = 6;
			this.label1.Text = "(Note: If \'To File and Open in RTF Editor\' is disabled, it means you have not spe" +
				"cified PA\'s default RTF editor. Choose \'Set RTF Editor\' to do so.)";
			// 
			// RTFExportOptions
			// 
			this.AcceptButton = this.btnExport;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(456, 149);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSetEditor);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.chkUseZero);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RTFExportOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export to Rich Text Format";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

	}
}
