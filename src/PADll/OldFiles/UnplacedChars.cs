using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for UnplacedChars.
	/// </summary>
	public class UnplacedChars : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private DataSet data;
		private int chartype;
		private System.Windows.Forms.ListBox lstChars;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private PaMainWnd m_boss;

		public UnplacedChars()
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

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			m_boss = (PaMainWnd) MdiParent;
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
			this.lstChars = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "# Unplaced";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Consonants";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lstChars
			// 
			this.lstChars.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lstChars.Location = new System.Drawing.Point(0, 38);
			this.lstChars.Name = "lstChars";
			this.lstChars.Size = new System.Drawing.Size(72, 56);
			this.lstChars.TabIndex = 1;
			// 
			// UnplacedChars
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(72, 94);
			this.Controls.Add(this.lstChars);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(80, 120);
			this.Name = "UnplacedChars";
			this.Resize += new System.EventHandler(this.UnplacedChars_Resize);
			this.Load += new System.EventHandler(this.UnplacedChars_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void UnplacedChars_Resize(object sender, EventArgs e)
		{
			lstChars.Location = new Point(0, 38);
			lstChars.Size = new Size(ClientSize.Width, ClientSize.Height - lstChars.Top);
		}

		private void UnplacedChars_Load(object sender, System.EventArgs e)
		{
			// go read vb code and see what to retain or discard
		}

		#region Properties
		public int CharType
		{
			set { chartype = value; }
		}
		#endregion

		#region Miscellaneous
		public void ShowList(bool Show)
		{
			if (Show)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
		}
		#endregion
	}
}
