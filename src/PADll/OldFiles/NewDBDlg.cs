using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.SpeechTools.Database;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for NewDB.
	/// </summary>
	public class NewDBDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtLangName;
		private System.Windows.Forms.Label lblDBPath;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private string string1;
		/// <summary>
		/// Determines whether this form is activated for rename or new database.
		/// </summary>
		public bool Rename;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewDBDlg()
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

		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
			{
				txtLangName.Text = txtLangName.Text.Trim();

				if (txtLangName.Text == "")
				{
					MessageBox.Show("You must enter a language name.", "Error",
						MessageBoxButtons.OK);
					txtLangName.Focus();
					e.Cancel = true;
					return;
				}

				if (txtLangName.Text.IndexOfAny(":*?<>+=|;\"/\\".ToCharArray())!= -1)
				{
					MessageBox.Show("Language name contains invalid characters.", "Error",
						MessageBoxButtons.OK);
					txtLangName.Focus();
					e.Cancel = true;
					return;
				}
				string1 = txtLangName.Text;
				///For now nothing is done yet.
				MessageBox.Show("Not Completed.","Phonology Assistant",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			base.OnClosing (e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtLangName = new System.Windows.Forms.TextBox();
			this.lblDBPath = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(56, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter New Language Information";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "&Language Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 1;
			this.label3.Text = "&Database Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label3.Visible = false;
			// 
			// txtLangName
			// 
			this.txtLangName.AutoSize = false;
			this.txtLangName.Location = new System.Drawing.Point(104, 48);
			this.txtLangName.Name = "txtLangName";
			this.txtLangName.Size = new System.Drawing.Size(160, 16);
			this.txtLangName.TabIndex = 2;
			this.txtLangName.Text = "";
			// 
			// lblDBPath
			// 
			this.lblDBPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblDBPath.Location = new System.Drawing.Point(104, 72);
			this.lblDBPath.Name = "lblDBPath";
			this.lblDBPath.Size = new System.Drawing.Size(160, 16);
			this.lblDBPath.TabIndex = 3;
			this.lblDBPath.Visible = false;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(64, 104);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "&OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(160, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "&Cancel";
			// 
			// NewDBDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(282, 133);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblDBPath);
			this.Controls.Add(this.txtLangName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "NewDBDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Database";
			this.Activated += new System.EventHandler(this.NewDB_Activated);
			this.ResumeLayout(false);

		}
		#endregion

		private void NewDB_Activated(object sender, System.EventArgs e)
		{
			if (Rename==true)
			{
				label3.Visible = true;
				lblDBPath.Text = DBUtils.DatabaseFile;
				lblDBPath.Visible = true;
				// where should the language name be stored?
				// do we open a connection just to get it?
				// see also: Histogram/HistogramWnd
				// txtLangName.Text = PaApp.LangName;
				// txtLangName.SelectAll();
			}

			CenterToScreen();
		}

		#region Properties
		/// <summary>
		/// Gets the string entered into the Language Name text box.
		/// </summary>
		[Browsable(false)]
		public string String1
		{
			get { return string1; }
		}
		#endregion

	}
}
