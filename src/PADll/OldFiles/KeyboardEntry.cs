using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for KeyboardEntry.
	/// </summary>
	public class KeyboardEntry : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox lbtTitle;
		private System.Windows.Forms.TextBox lbtFolder;
		private System.Windows.Forms.TextBox lbtTone;
		private System.Windows.Forms.TextBox lbtPhonetic;
		private System.Windows.Forms.TextBox lbtPOS;
		private System.Windows.Forms.TextBox lbtGloss;
		private System.Windows.Forms.TextBox lbtOrtho;
		private System.Windows.Forms.TextBox lbtPhonemic;
		private System.Windows.Forms.TextBox lbtWordRef;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox lbtDocTitle;
		private System.Windows.Forms.Label label10;

		#region Member variables

		private string m_category;
		private string m_folder;

		#endregion

		#region Properties to access data field member variables

		public string CategoryTitle
		{
			get {return m_category;}
			set {m_category = value;}
		}
		
		public string FolderTitle
		{
			get {return m_folder;}
			set {m_folder = value;}
		}

		#endregion

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Constructor/Dispose
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public KeyboardEntry()
		{
			InitializeComponent();

			lbtTitle.Text = CategoryTitle;
			lbtTitle.Enabled = false;
			lbtFolder.Text = FolderTitle;
			lbtFolder.Enabled = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
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
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.lbtTitle = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lbtFolder = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lbtTone = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lbtPhonetic = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lbtPOS = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.lbtGloss = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.lbtOrtho = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.lbtPhonemic = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.lbtWordRef = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.lbtDocTitle = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Category";
			// 
			// lbtTitle
			// 
			this.lbtTitle.Location = new System.Drawing.Point(104, 16);
			this.lbtTitle.Name = "lbtTitle";
			this.lbtTitle.Size = new System.Drawing.Size(176, 20);
			this.lbtTitle.TabIndex = 1;
			this.lbtTitle.Text = "";
			// 
			// btnSave
			// 
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnSave.Location = new System.Drawing.Point(64, 408);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 32);
			this.btnSave.TabIndex = 11;
			this.btnSave.Text = "Save";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(160, 408);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 32);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "Cancel";
			// 
			// lbtFolder
			// 
			this.lbtFolder.Location = new System.Drawing.Point(104, 56);
			this.lbtFolder.Name = "lbtFolder";
			this.lbtFolder.Size = new System.Drawing.Size(176, 20);
			this.lbtFolder.TabIndex = 2;
			this.lbtFolder.Text = "";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 24);
			this.label2.TabIndex = 4;
			this.label2.Text = "Folder";
			// 
			// lbtTone
			// 
			this.lbtTone.Location = new System.Drawing.Point(104, 176);
			this.lbtTone.Name = "lbtTone";
			this.lbtTone.Size = new System.Drawing.Size(176, 20);
			this.lbtTone.TabIndex = 5;
			this.lbtTone.Text = "";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 176);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 24);
			this.label3.TabIndex = 8;
			this.label3.Text = "Tone";
			// 
			// lbtPhonetic
			// 
			this.lbtPhonetic.Location = new System.Drawing.Point(104, 136);
			this.lbtPhonetic.Name = "lbtPhonetic";
			this.lbtPhonetic.Size = new System.Drawing.Size(176, 20);
			this.lbtPhonetic.TabIndex = 4;
			this.lbtPhonetic.Text = "";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 136);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 24);
			this.label4.TabIndex = 5;
			this.label4.Text = "Phonetic";
			// 
			// lbtPOS
			// 
			this.lbtPOS.Location = new System.Drawing.Point(104, 336);
			this.lbtPOS.Name = "lbtPOS";
			this.lbtPOS.Size = new System.Drawing.Size(176, 20);
			this.lbtPOS.TabIndex = 9;
			this.lbtPOS.Text = "";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 336);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 24);
			this.label5.TabIndex = 16;
			this.label5.Text = "POS";
			// 
			// lbtGloss
			// 
			this.lbtGloss.Location = new System.Drawing.Point(104, 296);
			this.lbtGloss.Name = "lbtGloss";
			this.lbtGloss.Size = new System.Drawing.Size(176, 20);
			this.lbtGloss.TabIndex = 8;
			this.lbtGloss.Text = "";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(8, 296);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 24);
			this.label6.TabIndex = 13;
			this.label6.Text = "Gloss";
			// 
			// lbtOrtho
			// 
			this.lbtOrtho.Location = new System.Drawing.Point(104, 256);
			this.lbtOrtho.Name = "lbtOrtho";
			this.lbtOrtho.Size = new System.Drawing.Size(176, 20);
			this.lbtOrtho.TabIndex = 7;
			this.lbtOrtho.Text = "";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(8, 256);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 24);
			this.label7.TabIndex = 12;
			this.label7.Text = "Ortho";
			// 
			// lbtPhonemic
			// 
			this.lbtPhonemic.Location = new System.Drawing.Point(104, 216);
			this.lbtPhonemic.Name = "lbtPhonemic";
			this.lbtPhonemic.Size = new System.Drawing.Size(176, 20);
			this.lbtPhonemic.TabIndex = 6;
			this.lbtPhonemic.Text = "";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(8, 216);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 24);
			this.label8.TabIndex = 9;
			this.label8.Text = "Phonemic";
			// 
			// lbtWordRef
			// 
			this.lbtWordRef.Location = new System.Drawing.Point(104, 376);
			this.lbtWordRef.Name = "lbtWordRef";
			this.lbtWordRef.Size = new System.Drawing.Size(176, 20);
			this.lbtWordRef.TabIndex = 10;
			this.lbtWordRef.Text = "";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.Location = new System.Drawing.Point(8, 376);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 24);
			this.label9.TabIndex = 18;
			this.label9.Text = "Word Ref";
			// 
			// lbtDocTitle
			// 
			this.lbtDocTitle.Location = new System.Drawing.Point(104, 96);
			this.lbtDocTitle.Name = "lbtDocTitle";
			this.lbtDocTitle.Size = new System.Drawing.Size(176, 20);
			this.lbtDocTitle.TabIndex = 3;
			this.lbtDocTitle.Text = "";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label10.Location = new System.Drawing.Point(8, 96);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(80, 24);
			this.label10.TabIndex = 20;
			this.label10.Text = "Doc Title";
			// 
			// KeyboardEntry
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(292, 447);
			this.Controls.Add(this.lbtDocTitle);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.lbtWordRef);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.lbtPOS);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbtGloss);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.lbtOrtho);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lbtPhonemic);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.lbtTone);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lbtPhonetic);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lbtFolder);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.lbtTitle);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "KeyboardEntry";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Keyboard Entry";
			this.ResumeLayout(false);

		}
		#endregion

		#region Overridden Methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a record to the database if the SAVE button is clicked.
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.DialogResult != DialogResult.OK)
				return;

			Record record = new Record();

			if (lbtDocTitle.Text.Length > 0)
				record.DocTitle = lbtDocTitle.Text;
				
			if (lbtPhonetic.Text.Length > 0)
				record.Phonetic = lbtPhonetic.Text;

			if (lbtTone.Text.Length > 0)
				record.Tone = lbtTone.Text;
				
			if (lbtPhonemic.Text.Length > 0)
				record.Phonemic = lbtPhonemic.Text;
				
			if (lbtOrtho.Text.Length > 0)
				record.Ortho = lbtOrtho.Text;

			if (lbtGloss.Text.Length > 0)
				record.Gloss = lbtGloss.Text;

			if (lbtPOS.Text.Length > 0)
				record.POS = lbtPOS.Text;

			if (lbtWordRef.Text.Length > 0)
				record.WordRef = lbtWordRef.Text;

			record.CatTitle = m_category;
			record.FolderTitle = m_folder;

			DocUpdate docUpdate = new DocUpdate();
			if (docUpdate.Add(record))
			{
				PaApp.MsgMediator.SendMessage("RefreshTree", null);
			}
			else
			{
				MessageBox.Show("Record Not Added!", Application.ProductName,  MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
			}
		}

		#endregion
	}
}
