using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.SpeechTools.Database;
using SIL.SpeechTools.Utils;
using XCore;

namespace SIL.Pa
{
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// Summary description for DispDia.
	/// </summary>
	/// <param name="e"></param>
	/// ------------------------------------------------------------------------------------
	public class DispDia : System.Windows.Forms.Form, IxCoreColleague
	{
		#region Member variables added by designer
		private System.Windows.Forms.Label lblDia0;
		private System.Windows.Forms.Label lblDia1;
		private System.Windows.Forms.Label lblDia2;
		private System.Windows.Forms.Label lblDia3;
		private System.Windows.Forms.Label lblDia4;
		private System.Windows.Forms.Label lblDia5;
		private System.Windows.Forms.Label lblDia6;
		private System.Windows.Forms.Label lblDia7;
		private System.Windows.Forms.Label lblDia8;
		private System.Windows.Forms.Label lblDia9;
		private System.Windows.Forms.Label lblDia10;
		private System.Windows.Forms.Label lblDia11;
		private System.Windows.Forms.Label lblDia12;
		private System.Windows.Forms.Label lblDia13;
		private System.Windows.Forms.Label lblDia14;
		private System.Windows.Forms.Label lblDia15;
		private System.Windows.Forms.Label lblDia16;
		private System.Windows.Forms.Label lblDia17;
		private System.Windows.Forms.Label lblDia18;
		private System.Windows.Forms.Label lblDia19;
		private System.Windows.Forms.Label lblDia20;
		private System.Windows.Forms.Label lblDia21;
		private System.Windows.Forms.Label lblDia22;
		private System.Windows.Forms.Label lblDia23;
		private System.Windows.Forms.Label lblDia24;
		private System.Windows.Forms.Label lblDia25;
		private System.Windows.Forms.Label lblDia26;
		private System.Windows.Forms.Label lblDia27;
		private System.Windows.Forms.Label lblDia28;
		private System.Windows.Forms.Label lblDia29;
		private System.Windows.Forms.Label lblDia30;
		private System.Windows.Forms.Label lblExample0;
		private System.Windows.Forms.Label lblExample1;
		private System.Windows.Forms.Label lblExample2;
		private System.Windows.Forms.Label lblExample3;
		private System.Windows.Forms.Label lblExample4;
		private System.Windows.Forms.Label lblExample5;
		private System.Windows.Forms.Label lblExample6;
		private System.Windows.Forms.Label lblExample7;
		private System.Windows.Forms.Label lblExample8;
		private System.Windows.Forms.Label lblExample9;
		private System.Windows.Forms.Label lblExample10;
		private System.Windows.Forms.Label lblExample11;
		private System.Windows.Forms.Label lblExample12;
		private System.Windows.Forms.Label lblExample13;
		private System.Windows.Forms.Label lblExample14;
		private System.Windows.Forms.Label lblExample15;
		private System.Windows.Forms.Label lblExample16;
		private System.Windows.Forms.Label lblExample17;
		private System.Windows.Forms.Label lblExample18;
		private System.Windows.Forms.Label lblExample19;
		private System.Windows.Forms.Label lblExample20;
		private System.Windows.Forms.Label lblExample21;
		private System.Windows.Forms.Label lblExample22;
		private System.Windows.Forms.Label lblExample23;
		private System.Windows.Forms.Label lblExample24;
		private System.Windows.Forms.Label lblExample25;
		private System.Windows.Forms.Label lblExample26;
		private System.Windows.Forms.Label lblExample27;
		private System.Windows.Forms.Label lblExample28;
		private System.Windows.Forms.Label lblExample29;
		private System.Windows.Forms.Label lblExample30;
		private System.Windows.Forms.Label lblExample31;
		private System.Windows.Forms.Label lblExample32;
		private System.Windows.Forms.Label lblExample33;
		private System.Windows.Forms.Label lblExample34;
		private System.Windows.Forms.Label lblExample35;
		private System.Windows.Forms.Label lblExample36;
		private System.Windows.Forms.Label label0;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		private Label [] m_lblDia;
		private int m_currentindex;
		private PaDataTable m_charList;
		private PaDataTable m_diaIPACharacters;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public DispDia()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//m_charList = new PaDataTable("SELECT * FROM CharList");
			//m_diaIPACharacters = 
			//    new PaDataTable("SELECT * FROM IPACharacters WHERE CharType = 7 AND DsplyOrder Is Not Null");
			////
			//// TODO: Add any constructor code after InitializeComponent call
			////
			//m_lblDia = new Label [] { lblDia0, lblDia1, lblDia2, lblDia3, lblDia4,
			//                            lblDia5, lblDia6, lblDia7, lblDia8, lblDia9,
			//                            lblDia10, lblDia11, lblDia12, lblDia13, lblDia14,
			//                            lblDia15, lblDia16, lblDia17, lblDia18, lblDia19,
			//                            lblDia20, lblDia21, lblDia22, lblDia23, lblDia24,
			//                            lblDia25, lblDia26, lblDia27, lblDia28, lblDia29,
			//                            lblDia30};
			//foreach (Label lbl in m_lblDia)
			//{
			//    lbl.Click += new EventHandler(lblDia_Click);
			//    lbl.DoubleClick += new EventHandler(lblDia_DoubleClick);
			//    lbl.Font = new Font(FontHelper.PhoneticFont.Name, 14);
			//    lbl.ForeColor = SystemColors.ControlText;
			//    lbl.Enabled = false;
				
			//    m_currentindex = 0;
			//}

			//// Access dB here
			//if (ReadDiacriticsFromDB() == false)
			//    MessageBox.Show("Error accessing the database to enable appropriate diacritics");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="e"></param>
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			PaApp.SettingsHandler.LoadFormProperties(this);
			base.OnLoad (e);
			Text = DBUtils.LanguageName + " Diacritics";
			while (!(m_lblDia[m_currentindex].Enabled))
				m_currentindex++;
			m_lblDia[m_currentindex].BackColor = Color.Teal;
			Refresh();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveFormProperties(this);
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);
			if (e.Modifiers > 0) return;
			int i = m_currentindex;
			m_lblDia[i].BackColor = Color.Transparent;
			if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Right))
			{
				i = ((m_currentindex == (m_lblDia.Length - 1)) ? 0 : m_currentindex + 1);
				while (!(m_lblDia[i].Enabled) && (i != m_currentindex))
					i = ((i == (m_lblDia.Length - 1)) ? 0 : (i + 1));
				m_lblDia[i].BackColor = Color.Teal;
				m_currentindex = i;
			}
			else if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left))
			{
				i = ((m_currentindex == 0) ? (m_lblDia.Length - 1) : m_currentindex - 1);
				while (!(m_lblDia[i].Enabled) && (i != m_currentindex))
					i = ((i == 0) ? (m_lblDia.Length - 1) : (i - 1));
				m_lblDia[i].BackColor = Color.Teal;
				m_currentindex = i;
			}
			else if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
				lblDia_DoubleClick(m_lblDia[m_currentindex], e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnMove(EventArgs e)
		{
			base.OnMove (e);
			Refresh();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);
			Refresh();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public override void Refresh()
		{
			base.Refresh ();
		}

//		protected override void OnPaint(PaintEventArgs e)
//		{
//			base.OnPaint (e);
//			int tmp1, tmp2;
//			tmp1 = lblDia0.Left;
//			tmp2 = lblExample30.Right;
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia0.Top, tmp2, lblDia0.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia1.Top, tmp2, lblDia1.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia2.Top, tmp2, lblDia2.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia3.Top, tmp2, lblDia3.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia4.Top, tmp2, lblDia4.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia5.Top, tmp2, lblDia5.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia6.Top, tmp2, lblDia6.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia7.Top, tmp2, lblDia7.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia8.Top, tmp2, lblDia8.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia9.Top, tmp2, lblDia9.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia10.Top, tmp2, lblDia10.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia11.Top, tmp2, lblDia11.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, tmp1, lblDia11.Bottom, tmp2, lblDia11.Bottom);
//			tmp1 = lblDia0.Top;
//			tmp2 = lblDia11.Bottom;
//			e.Graphics.DrawLine(SystemPens.ControlText, lblDia0.Left, tmp1, lblDia0.Left, tmp2);
//			e.Graphics.DrawLine(SystemPens.ControlText, lblDia12.Left, tmp1, lblDia12.Left, tmp2);
//			e.Graphics.DrawLine(SystemPens.ControlText, lblDia24.Left, tmp1, lblDia24.Left, lblDia19.Top);
//			e.Graphics.DrawLine(SystemPens.ControlText, lblExample30.Right, tmp1, lblExample30.Right, tmp2);
//		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
					return true;
				default:
					return base.IsInputKey (keyData);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disable/enable vowels according to the current database.
		/// </summary>
		/// <returns>True indicates success</returns>
		/// ------------------------------------------------------------------------------------
		private bool ReadDiacriticsFromDB() 
		{
			string sHexIPAChar = String.Empty;
			int iDsplyOrder = 0;
//			Font fntCon = new Font(FontHelper.PhoneticFont.Name, 14, FontStyle.Bold);
			Font fntCon = new Font(FontHelper.PhoneticFont.Name, 16, FontStyle.Bold);

			foreach (DataRow row in m_diaIPACharacters.Rows)
			{
				sHexIPAChar = (row[DBFields.HexIPAChar]).ToString();

				DataRow[] row2 = m_charList.Select("HexCharStr LIKE '*" + sHexIPAChar + "*'");
				if (row2.Length > 0)
				{
					iDsplyOrder = (int)((System.Int16)row[DBFields.DsplyOrder]);
					int lblIndex = iDsplyOrder;		
					if (lblIndex < 0)
						continue;

					// Database is stored as 1-31, rather than 0-30, so need to shift indexing
					m_lblDia[lblIndex-1].Enabled = true;
					m_lblDia[lblIndex-1].Font = fntCon;
					// NOTE - Diacritics are never base characters, so they will always be black
					// and NOT red.
					m_lblDia[lblIndex-1].ForeColor = SystemColors.ControlText;
					m_lblDia[lblIndex-1].Tag = SystemColors.ControlText;
				}
			}
			return true;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblDia0 = new System.Windows.Forms.Label();
			this.lblDia1 = new System.Windows.Forms.Label();
			this.lblDia2 = new System.Windows.Forms.Label();
			this.lblDia3 = new System.Windows.Forms.Label();
			this.lblDia4 = new System.Windows.Forms.Label();
			this.lblDia5 = new System.Windows.Forms.Label();
			this.lblDia6 = new System.Windows.Forms.Label();
			this.lblDia7 = new System.Windows.Forms.Label();
			this.lblDia8 = new System.Windows.Forms.Label();
			this.lblDia9 = new System.Windows.Forms.Label();
			this.lblDia10 = new System.Windows.Forms.Label();
			this.lblDia11 = new System.Windows.Forms.Label();
			this.lblDia12 = new System.Windows.Forms.Label();
			this.lblDia13 = new System.Windows.Forms.Label();
			this.lblDia14 = new System.Windows.Forms.Label();
			this.lblDia15 = new System.Windows.Forms.Label();
			this.lblDia16 = new System.Windows.Forms.Label();
			this.lblDia17 = new System.Windows.Forms.Label();
			this.lblDia18 = new System.Windows.Forms.Label();
			this.lblDia19 = new System.Windows.Forms.Label();
			this.lblDia20 = new System.Windows.Forms.Label();
			this.lblDia21 = new System.Windows.Forms.Label();
			this.lblDia22 = new System.Windows.Forms.Label();
			this.lblDia23 = new System.Windows.Forms.Label();
			this.lblDia24 = new System.Windows.Forms.Label();
			this.lblDia25 = new System.Windows.Forms.Label();
			this.lblDia26 = new System.Windows.Forms.Label();
			this.lblDia27 = new System.Windows.Forms.Label();
			this.lblDia28 = new System.Windows.Forms.Label();
			this.lblDia29 = new System.Windows.Forms.Label();
			this.lblDia30 = new System.Windows.Forms.Label();
			this.lblExample0 = new System.Windows.Forms.Label();
			this.lblExample1 = new System.Windows.Forms.Label();
			this.lblExample2 = new System.Windows.Forms.Label();
			this.lblExample3 = new System.Windows.Forms.Label();
			this.lblExample4 = new System.Windows.Forms.Label();
			this.lblExample5 = new System.Windows.Forms.Label();
			this.lblExample6 = new System.Windows.Forms.Label();
			this.lblExample7 = new System.Windows.Forms.Label();
			this.lblExample8 = new System.Windows.Forms.Label();
			this.lblExample9 = new System.Windows.Forms.Label();
			this.lblExample10 = new System.Windows.Forms.Label();
			this.lblExample11 = new System.Windows.Forms.Label();
			this.lblExample12 = new System.Windows.Forms.Label();
			this.lblExample13 = new System.Windows.Forms.Label();
			this.lblExample14 = new System.Windows.Forms.Label();
			this.lblExample15 = new System.Windows.Forms.Label();
			this.lblExample16 = new System.Windows.Forms.Label();
			this.lblExample17 = new System.Windows.Forms.Label();
			this.lblExample18 = new System.Windows.Forms.Label();
			this.lblExample19 = new System.Windows.Forms.Label();
			this.lblExample20 = new System.Windows.Forms.Label();
			this.lblExample21 = new System.Windows.Forms.Label();
			this.lblExample22 = new System.Windows.Forms.Label();
			this.lblExample23 = new System.Windows.Forms.Label();
			this.lblExample24 = new System.Windows.Forms.Label();
			this.lblExample25 = new System.Windows.Forms.Label();
			this.lblExample26 = new System.Windows.Forms.Label();
			this.lblExample27 = new System.Windows.Forms.Label();
			this.lblExample28 = new System.Windows.Forms.Label();
			this.lblExample29 = new System.Windows.Forms.Label();
			this.lblExample30 = new System.Windows.Forms.Label();
			this.lblExample31 = new System.Windows.Forms.Label();
			this.lblExample32 = new System.Windows.Forms.Label();
			this.lblExample33 = new System.Windows.Forms.Label();
			this.lblExample34 = new System.Windows.Forms.Label();
			this.lblExample35 = new System.Windows.Forms.Label();
			this.lblExample36 = new System.Windows.Forms.Label();
			this.label0 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblDia0
			// 
			this.lblDia0.BackColor = System.Drawing.Color.Transparent;
			this.lblDia0.Font = new System.Drawing.Font("Doulos SIL", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDia0.Location = new System.Drawing.Point(16, 8);
			this.lblDia0.Name = "lblDia0";
			this.lblDia0.Size = new System.Drawing.Size(32, 40);
			this.lblDia0.TabIndex = 0;
			this.lblDia0.Text = "◌̥ ";
			this.lblDia0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia1
			// 
			this.lblDia1.BackColor = System.Drawing.Color.Transparent;
			this.lblDia1.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia1.Location = new System.Drawing.Point(16, 48);
			this.lblDia1.Name = "lblDia1";
			this.lblDia1.Size = new System.Drawing.Size(32, 40);
			this.lblDia1.TabIndex = 1;
			this.lblDia1.Text = "◌̬";
			this.lblDia1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia2
			// 
			this.lblDia2.BackColor = System.Drawing.Color.Transparent;
			this.lblDia2.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia2.Location = new System.Drawing.Point(16, 88);
			this.lblDia2.Name = "lblDia2";
			this.lblDia2.Size = new System.Drawing.Size(40, 40);
			this.lblDia2.TabIndex = 1;
			this.lblDia2.Text = "◌ʰ";
			this.lblDia2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia3
			// 
			this.lblDia3.BackColor = System.Drawing.Color.Transparent;
			this.lblDia3.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia3.Location = new System.Drawing.Point(16, 128);
			this.lblDia3.Name = "lblDia3";
			this.lblDia3.Size = new System.Drawing.Size(32, 40);
			this.lblDia3.TabIndex = 0;
			this.lblDia3.Text = "◌̹";
			this.lblDia3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia4
			// 
			this.lblDia4.BackColor = System.Drawing.Color.Transparent;
			this.lblDia4.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia4.Location = new System.Drawing.Point(16, 168);
			this.lblDia4.Name = "lblDia4";
			this.lblDia4.Size = new System.Drawing.Size(32, 40);
			this.lblDia4.TabIndex = 1;
			this.lblDia4.Text = "◌̜";
			this.lblDia4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia5
			// 
			this.lblDia5.BackColor = System.Drawing.Color.Transparent;
			this.lblDia5.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia5.Location = new System.Drawing.Point(16, 208);
			this.lblDia5.Name = "lblDia5";
			this.lblDia5.Size = new System.Drawing.Size(32, 40);
			this.lblDia5.TabIndex = 0;
			this.lblDia5.Text = "◌̟";
			this.lblDia5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia6
			// 
			this.lblDia6.BackColor = System.Drawing.Color.Transparent;
			this.lblDia6.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia6.Location = new System.Drawing.Point(16, 248);
			this.lblDia6.Name = "lblDia6";
			this.lblDia6.Size = new System.Drawing.Size(32, 40);
			this.lblDia6.TabIndex = 1;
			this.lblDia6.Text = "◌̠";
			this.lblDia6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia7
			// 
			this.lblDia7.BackColor = System.Drawing.Color.Transparent;
			this.lblDia7.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia7.Location = new System.Drawing.Point(16, 288);
			this.lblDia7.Name = "lblDia7";
			this.lblDia7.Size = new System.Drawing.Size(32, 40);
			this.lblDia7.TabIndex = 0;
			this.lblDia7.Text = "◌̈";
			this.lblDia7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia8
			// 
			this.lblDia8.BackColor = System.Drawing.Color.Transparent;
			this.lblDia8.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia8.Location = new System.Drawing.Point(16, 328);
			this.lblDia8.Name = "lblDia8";
			this.lblDia8.Size = new System.Drawing.Size(32, 40);
			this.lblDia8.TabIndex = 1;
			this.lblDia8.Text = "◌̽";
			this.lblDia8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia9
			// 
			this.lblDia9.BackColor = System.Drawing.Color.Transparent;
			this.lblDia9.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia9.Location = new System.Drawing.Point(16, 368);
			this.lblDia9.Name = "lblDia9";
			this.lblDia9.Size = new System.Drawing.Size(32, 40);
			this.lblDia9.TabIndex = 0;
			this.lblDia9.Text = "◌̩";
			this.lblDia9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia10
			// 
			this.lblDia10.BackColor = System.Drawing.Color.Transparent;
			this.lblDia10.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia10.Location = new System.Drawing.Point(16, 408);
			this.lblDia10.Name = "lblDia10";
			this.lblDia10.Size = new System.Drawing.Size(32, 40);
			this.lblDia10.TabIndex = 0;
			this.lblDia10.Text = "◌̯";
			this.lblDia10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia11
			// 
			this.lblDia11.BackColor = System.Drawing.Color.Transparent;
			this.lblDia11.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia11.Location = new System.Drawing.Point(16, 448);
			this.lblDia11.Name = "lblDia11";
			this.lblDia11.Size = new System.Drawing.Size(32, 40);
			this.lblDia11.TabIndex = 1;
			this.lblDia11.Text = "◌˞";
			this.lblDia11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia12
			// 
			this.lblDia12.BackColor = System.Drawing.Color.Transparent;
			this.lblDia12.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia12.Location = new System.Drawing.Point(186, 8);
			this.lblDia12.Name = "lblDia12";
			this.lblDia12.Size = new System.Drawing.Size(32, 40);
			this.lblDia12.TabIndex = 0;
			this.lblDia12.Text = "◌̤";
			this.lblDia12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia13
			// 
			this.lblDia13.BackColor = System.Drawing.Color.Transparent;
			this.lblDia13.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia13.Location = new System.Drawing.Point(186, 48);
			this.lblDia13.Name = "lblDia13";
			this.lblDia13.Size = new System.Drawing.Size(32, 40);
			this.lblDia13.TabIndex = 1;
			this.lblDia13.Text = "◌̰";
			this.lblDia13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia14
			// 
			this.lblDia14.BackColor = System.Drawing.Color.Transparent;
			this.lblDia14.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia14.Location = new System.Drawing.Point(186, 88);
			this.lblDia14.Name = "lblDia14";
			this.lblDia14.Size = new System.Drawing.Size(32, 40);
			this.lblDia14.TabIndex = 0;
			this.lblDia14.Text = "◌̼";
			this.lblDia14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia15
			// 
			this.lblDia15.BackColor = System.Drawing.Color.Transparent;
			this.lblDia15.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia15.Location = new System.Drawing.Point(186, 128);
			this.lblDia15.Name = "lblDia15";
			this.lblDia15.Size = new System.Drawing.Size(42, 40);
			this.lblDia15.TabIndex = 1;
			this.lblDia15.Text = "◌ʷ";
			this.lblDia15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia16
			// 
			this.lblDia16.BackColor = System.Drawing.Color.Transparent;
			this.lblDia16.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia16.Location = new System.Drawing.Point(186, 168);
			this.lblDia16.Name = "lblDia16";
			this.lblDia16.Size = new System.Drawing.Size(32, 40);
			this.lblDia16.TabIndex = 1;
			this.lblDia16.Text = "◌ʲ";
			this.lblDia16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia17
			// 
			this.lblDia17.BackColor = System.Drawing.Color.Transparent;
			this.lblDia17.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia17.Location = new System.Drawing.Point(186, 208);
			this.lblDia17.Name = "lblDia17";
			this.lblDia17.Size = new System.Drawing.Size(38, 40);
			this.lblDia17.TabIndex = 1;
			this.lblDia17.Text = "◌ˠ";
			this.lblDia17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia18
			// 
			this.lblDia18.BackColor = System.Drawing.Color.Transparent;
			this.lblDia18.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia18.Location = new System.Drawing.Point(186, 248);
			this.lblDia18.Name = "lblDia18";
			this.lblDia18.Size = new System.Drawing.Size(42, 40);
			this.lblDia18.TabIndex = 1;
			this.lblDia18.Text = "◌ˁ";
			this.lblDia18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia19
			// 
			this.lblDia19.BackColor = System.Drawing.Color.Transparent;
			this.lblDia19.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia19.Location = new System.Drawing.Point(186, 288);
			this.lblDia19.Name = "lblDia19";
			this.lblDia19.Size = new System.Drawing.Size(32, 40);
			this.lblDia19.TabIndex = 0;
			this.lblDia19.Text = "◌̴";
			this.lblDia19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia20
			// 
			this.lblDia20.BackColor = System.Drawing.Color.Transparent;
			this.lblDia20.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia20.Location = new System.Drawing.Point(186, 328);
			this.lblDia20.Name = "lblDia20";
			this.lblDia20.Size = new System.Drawing.Size(32, 40);
			this.lblDia20.TabIndex = 0;
			this.lblDia20.Text = "◌̝";
			this.lblDia20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia21
			// 
			this.lblDia21.BackColor = System.Drawing.Color.Transparent;
			this.lblDia21.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia21.Location = new System.Drawing.Point(186, 368);
			this.lblDia21.Name = "lblDia21";
			this.lblDia21.Size = new System.Drawing.Size(32, 40);
			this.lblDia21.TabIndex = 0;
			this.lblDia21.Text = "◌̞";
			this.lblDia21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia22
			// 
			this.lblDia22.BackColor = System.Drawing.Color.Transparent;
			this.lblDia22.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia22.Location = new System.Drawing.Point(186, 408);
			this.lblDia22.Name = "lblDia22";
			this.lblDia22.Size = new System.Drawing.Size(32, 40);
			this.lblDia22.TabIndex = 1;
			this.lblDia22.Text = "◌̘";
			this.lblDia22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia23
			// 
			this.lblDia23.BackColor = System.Drawing.Color.Transparent;
			this.lblDia23.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia23.Location = new System.Drawing.Point(186, 448);
			this.lblDia23.Name = "lblDia23";
			this.lblDia23.Size = new System.Drawing.Size(32, 40);
			this.lblDia23.TabIndex = 0;
			this.lblDia23.Text = "◌̙";
			this.lblDia23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia24
			// 
			this.lblDia24.BackColor = System.Drawing.Color.Transparent;
			this.lblDia24.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia24.Location = new System.Drawing.Point(392, 8);
			this.lblDia24.Name = "lblDia24";
			this.lblDia24.Size = new System.Drawing.Size(32, 40);
			this.lblDia24.TabIndex = 0;
			this.lblDia24.Text = "◌̪";
			this.lblDia24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia25
			// 
			this.lblDia25.BackColor = System.Drawing.Color.Transparent;
			this.lblDia25.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia25.Location = new System.Drawing.Point(392, 48);
			this.lblDia25.Name = "lblDia25";
			this.lblDia25.Size = new System.Drawing.Size(32, 40);
			this.lblDia25.TabIndex = 0;
			this.lblDia25.Text = "◌̺";
			this.lblDia25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia26
			// 
			this.lblDia26.BackColor = System.Drawing.Color.Transparent;
			this.lblDia26.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia26.Location = new System.Drawing.Point(392, 88);
			this.lblDia26.Name = "lblDia26";
			this.lblDia26.Size = new System.Drawing.Size(32, 40);
			this.lblDia26.TabIndex = 1;
			this.lblDia26.Text = "◌̻";
			this.lblDia26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia27
			// 
			this.lblDia27.BackColor = System.Drawing.Color.Transparent;
			this.lblDia27.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia27.Location = new System.Drawing.Point(392, 128);
			this.lblDia27.Name = "lblDia27";
			this.lblDia27.Size = new System.Drawing.Size(40, 40);
			this.lblDia27.TabIndex = 0;
			this.lblDia27.Text = "◌ ̃";
			this.lblDia27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia28
			// 
			this.lblDia28.BackColor = System.Drawing.Color.Transparent;
			this.lblDia28.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia28.Location = new System.Drawing.Point(392, 168);
			this.lblDia28.Name = "lblDia28";
			this.lblDia28.Size = new System.Drawing.Size(40, 40);
			this.lblDia28.TabIndex = 1;
			this.lblDia28.Text = "◌ⁿ";
			this.lblDia28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia29
			// 
			this.lblDia29.BackColor = System.Drawing.Color.Transparent;
			this.lblDia29.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia29.Location = new System.Drawing.Point(392, 208);
			this.lblDia29.Name = "lblDia29";
			this.lblDia29.Size = new System.Drawing.Size(32, 40);
			this.lblDia29.TabIndex = 1;
			this.lblDia29.Text = "◌ˡ";
			this.lblDia29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDia30
			// 
			this.lblDia30.BackColor = System.Drawing.Color.Transparent;
			this.lblDia30.Font = new System.Drawing.Font("Doulos SIL", 21.75F);
			this.lblDia30.Location = new System.Drawing.Point(392, 248);
			this.lblDia30.Name = "lblDia30";
			this.lblDia30.Size = new System.Drawing.Size(32, 40);
			this.lblDia30.TabIndex = 0;
			this.lblDia30.Text = "◌̚";
			this.lblDia30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample0
			// 
			this.lblExample0.AutoSize = true;
			this.lblExample0.BackColor = System.Drawing.Color.Transparent;
			this.lblExample0.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample0.Location = new System.Drawing.Point(120, 8);
			this.lblExample0.Name = "lblExample0";
			this.lblExample0.Size = new System.Drawing.Size(36, 36);
			this.lblExample0.TabIndex = 0;
			this.lblExample0.Text = "n̥ d̥";
			this.lblExample0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample1
			// 
			this.lblExample1.BackColor = System.Drawing.Color.Transparent;
			this.lblExample1.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample1.Location = new System.Drawing.Point(120, 48);
			this.lblExample1.Name = "lblExample1";
			this.lblExample1.Size = new System.Drawing.Size(36, 36);
			this.lblExample1.TabIndex = 0;
			this.lblExample1.Text = "s̬  t̬";
			this.lblExample1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample2
			// 
			this.lblExample2.AutoSize = true;
			this.lblExample2.BackColor = System.Drawing.Color.Transparent;
			this.lblExample2.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample2.Location = new System.Drawing.Point(120, 88);
			this.lblExample2.Name = "lblExample2";
			this.lblExample2.Size = new System.Drawing.Size(48, 36);
			this.lblExample2.TabIndex = 0;
			this.lblExample2.Text = "tʰ dʰ";
			this.lblExample2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample3
			// 
			this.lblExample3.AutoSize = true;
			this.lblExample3.BackColor = System.Drawing.Color.Transparent;
			this.lblExample3.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample3.Location = new System.Drawing.Point(138, 128);
			this.lblExample3.Name = "lblExample3";
			this.lblExample3.Size = new System.Drawing.Size(17, 36);
			this.lblExample3.TabIndex = 0;
			this.lblExample3.Text = "ɔ̹";
			this.lblExample3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample4
			// 
			this.lblExample4.AutoSize = true;
			this.lblExample4.BackColor = System.Drawing.Color.Transparent;
			this.lblExample4.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample4.Location = new System.Drawing.Point(138, 168);
			this.lblExample4.Name = "lblExample4";
			this.lblExample4.Size = new System.Drawing.Size(17, 36);
			this.lblExample4.TabIndex = 0;
			this.lblExample4.Text = "ɔ̜";
			this.lblExample4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample5
			// 
			this.lblExample5.BackColor = System.Drawing.Color.Transparent;
			this.lblExample5.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample5.Location = new System.Drawing.Point(136, 208);
			this.lblExample5.Name = "lblExample5";
			this.lblExample5.Size = new System.Drawing.Size(22, 36);
			this.lblExample5.TabIndex = 0;
			this.lblExample5.Text = "u̟";
			this.lblExample5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample6
			// 
			this.lblExample6.AutoSize = true;
			this.lblExample6.BackColor = System.Drawing.Color.Transparent;
			this.lblExample6.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample6.Location = new System.Drawing.Point(138, 248);
			this.lblExample6.Name = "lblExample6";
			this.lblExample6.Size = new System.Drawing.Size(17, 36);
			this.lblExample6.TabIndex = 0;
			this.lblExample6.Text = "e̠";
			this.lblExample6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample7
			// 
			this.lblExample7.AutoSize = true;
			this.lblExample7.BackColor = System.Drawing.Color.Transparent;
			this.lblExample7.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample7.Location = new System.Drawing.Point(138, 288);
			this.lblExample7.Name = "lblExample7";
			this.lblExample7.Size = new System.Drawing.Size(17, 36);
			this.lblExample7.TabIndex = 0;
			this.lblExample7.Text = "ë";
			this.lblExample7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample8
			// 
			this.lblExample8.AutoSize = true;
			this.lblExample8.BackColor = System.Drawing.Color.Transparent;
			this.lblExample8.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample8.Location = new System.Drawing.Point(138, 328);
			this.lblExample8.Name = "lblExample8";
			this.lblExample8.Size = new System.Drawing.Size(17, 36);
			this.lblExample8.TabIndex = 0;
			this.lblExample8.Text = "e̽";
			this.lblExample8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample9
			// 
			this.lblExample9.AutoSize = true;
			this.lblExample9.BackColor = System.Drawing.Color.Transparent;
			this.lblExample9.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample9.Location = new System.Drawing.Point(138, 368);
			this.lblExample9.Name = "lblExample9";
			this.lblExample9.Size = new System.Drawing.Size(19, 36);
			this.lblExample9.TabIndex = 0;
			this.lblExample9.Text = "n̩";
			this.lblExample9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample10
			// 
			this.lblExample10.AutoSize = true;
			this.lblExample10.BackColor = System.Drawing.Color.Transparent;
			this.lblExample10.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample10.Location = new System.Drawing.Point(138, 408);
			this.lblExample10.Name = "lblExample10";
			this.lblExample10.Size = new System.Drawing.Size(17, 36);
			this.lblExample10.TabIndex = 0;
			this.lblExample10.Text = "e̯";
			this.lblExample10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample11
			// 
			this.lblExample11.AutoSize = true;
			this.lblExample11.BackColor = System.Drawing.Color.Transparent;
			this.lblExample11.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample11.Location = new System.Drawing.Point(120, 448);
			this.lblExample11.Name = "lblExample11";
			this.lblExample11.Size = new System.Drawing.Size(39, 36);
			this.lblExample11.TabIndex = 0;
			this.lblExample11.Text = "ə˞ a˞";
			this.lblExample11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample12
			// 
			this.lblExample12.AutoSize = true;
			this.lblExample12.BackColor = System.Drawing.Color.Transparent;
			this.lblExample12.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample12.Location = new System.Drawing.Point(314, 8);
			this.lblExample12.Name = "lblExample12";
			this.lblExample12.Size = new System.Drawing.Size(41, 36);
			this.lblExample12.TabIndex = 0;
			this.lblExample12.Text = "b̤  a̤";
			this.lblExample12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample13
			// 
			this.lblExample13.AutoSize = true;
			this.lblExample13.BackColor = System.Drawing.Color.Transparent;
			this.lblExample13.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample13.Location = new System.Drawing.Point(314, 48);
			this.lblExample13.Name = "lblExample13";
			this.lblExample13.Size = new System.Drawing.Size(41, 36);
			this.lblExample13.TabIndex = 0;
			this.lblExample13.Text = "b̰  a̰";
			this.lblExample13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample14
			// 
			this.lblExample14.AutoSize = true;
			this.lblExample14.BackColor = System.Drawing.Color.Transparent;
			this.lblExample14.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample14.Location = new System.Drawing.Point(314, 88);
			this.lblExample14.Name = "lblExample14";
			this.lblExample14.Size = new System.Drawing.Size(37, 36);
			this.lblExample14.TabIndex = 0;
			this.lblExample14.Text = "t̼  d̼";
			this.lblExample14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample15
			// 
			this.lblExample15.AutoSize = true;
			this.lblExample15.BackColor = System.Drawing.Color.Transparent;
			this.lblExample15.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample15.Location = new System.Drawing.Point(314, 128);
			this.lblExample15.Name = "lblExample15";
			this.lblExample15.Size = new System.Drawing.Size(54, 36);
			this.lblExample15.TabIndex = 0;
			this.lblExample15.Text = "tʷ dʷ";
			this.lblExample15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample16
			// 
			this.lblExample16.AutoSize = true;
			this.lblExample16.BackColor = System.Drawing.Color.Transparent;
			this.lblExample16.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample16.Location = new System.Drawing.Point(314, 168);
			this.lblExample16.Name = "lblExample16";
			this.lblExample16.Size = new System.Drawing.Size(40, 36);
			this.lblExample16.TabIndex = 0;
			this.lblExample16.Text = "tʲ dʲ";
			this.lblExample16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample17
			// 
			this.lblExample17.AutoSize = true;
			this.lblExample17.BackColor = System.Drawing.Color.Transparent;
			this.lblExample17.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample17.Location = new System.Drawing.Point(314, 208);
			this.lblExample17.Name = "lblExample17";
			this.lblExample17.Size = new System.Drawing.Size(47, 36);
			this.lblExample17.TabIndex = 0;
			this.lblExample17.Text = "tˠ dˠ";
			this.lblExample17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample18
			// 
			this.lblExample18.AutoSize = true;
			this.lblExample18.BackColor = System.Drawing.Color.Transparent;
			this.lblExample18.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample18.Location = new System.Drawing.Point(314, 248);
			this.lblExample18.Name = "lblExample18";
			this.lblExample18.Size = new System.Drawing.Size(45, 36);
			this.lblExample18.TabIndex = 0;
			this.lblExample18.Text = "tˁ dˁ";
			this.lblExample18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample19
			// 
			this.lblExample19.BackColor = System.Drawing.Color.Transparent;
			this.lblExample19.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample19.Location = new System.Drawing.Point(369, 288);
			this.lblExample19.Name = "lblExample19";
			this.lblExample19.Size = new System.Drawing.Size(17, 36);
			this.lblExample19.TabIndex = 0;
			this.lblExample19.Text = "l̴";
			this.lblExample19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample20
			// 
			this.lblExample20.BackColor = System.Drawing.Color.Transparent;
			this.lblExample20.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample20.Location = new System.Drawing.Point(288, 328);
			this.lblExample20.Name = "lblExample20";
			this.lblExample20.Size = new System.Drawing.Size(17, 36);
			this.lblExample20.TabIndex = 0;
			this.lblExample20.Text = "e̝";
			this.lblExample20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample21
			// 
			this.lblExample21.BackColor = System.Drawing.Color.Transparent;
			this.lblExample21.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample21.Location = new System.Drawing.Point(369, 328);
			this.lblExample21.Name = "lblExample21";
			this.lblExample21.Size = new System.Drawing.Size(17, 36);
			this.lblExample21.TabIndex = 0;
			this.lblExample21.Text = "ɹ̝";
			this.lblExample21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample22
			// 
			this.lblExample22.AutoSize = true;
			this.lblExample22.BackColor = System.Drawing.Color.Transparent;
			this.lblExample22.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.lblExample22.Location = new System.Drawing.Point(360, 336);
			this.lblExample22.Name = "lblExample22";
			this.lblExample22.Size = new System.Drawing.Size(10, 19);
			this.lblExample22.TabIndex = 0;
			this.lblExample22.Text = "(";
			this.lblExample22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample23
			// 
			this.lblExample23.AutoSize = true;
			this.lblExample23.BackColor = System.Drawing.Color.Transparent;
			this.lblExample23.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.lblExample23.Location = new System.Drawing.Point(385, 336);
			this.lblExample23.Name = "lblExample23";
			this.lblExample23.Size = new System.Drawing.Size(168, 19);
			this.lblExample23.TabIndex = 0;
			this.lblExample23.Text = "= voiced alveolar fricative )";
			this.lblExample23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample24
			// 
			this.lblExample24.BackColor = System.Drawing.Color.Transparent;
			this.lblExample24.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample24.Location = new System.Drawing.Point(288, 368);
			this.lblExample24.Name = "lblExample24";
			this.lblExample24.Size = new System.Drawing.Size(17, 36);
			this.lblExample24.TabIndex = 0;
			this.lblExample24.Text = "e̞";
			this.lblExample24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample25
			// 
			this.lblExample25.AutoSize = true;
			this.lblExample25.BackColor = System.Drawing.Color.Transparent;
			this.lblExample25.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.lblExample25.Location = new System.Drawing.Point(360, 376);
			this.lblExample25.Name = "lblExample25";
			this.lblExample25.Size = new System.Drawing.Size(10, 19);
			this.lblExample25.TabIndex = 0;
			this.lblExample25.Text = "(";
			this.lblExample25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample26
			// 
			this.lblExample26.BackColor = System.Drawing.Color.Transparent;
			this.lblExample26.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample26.Location = new System.Drawing.Point(369, 368);
			this.lblExample26.Name = "lblExample26";
			this.lblExample26.Size = new System.Drawing.Size(17, 36);
			this.lblExample26.TabIndex = 0;
			this.lblExample26.Text = "β̞";
			this.lblExample26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample27
			// 
			this.lblExample27.AutoSize = true;
			this.lblExample27.BackColor = System.Drawing.Color.Transparent;
			this.lblExample27.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.lblExample27.Location = new System.Drawing.Point(385, 376);
			this.lblExample27.Name = "lblExample27";
			this.lblExample27.Size = new System.Drawing.Size(191, 19);
			this.lblExample27.TabIndex = 0;
			this.lblExample27.Text = "= voiced bilabial approximant )";
			this.lblExample27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample28
			// 
			this.lblExample28.BackColor = System.Drawing.Color.Transparent;
			this.lblExample28.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample28.Location = new System.Drawing.Point(369, 408);
			this.lblExample28.Name = "lblExample28";
			this.lblExample28.Size = new System.Drawing.Size(17, 36);
			this.lblExample28.TabIndex = 0;
			this.lblExample28.Text = "e̘";
			this.lblExample28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample29
			// 
			this.lblExample29.BackColor = System.Drawing.Color.Transparent;
			this.lblExample29.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample29.Location = new System.Drawing.Point(369, 448);
			this.lblExample29.Name = "lblExample29";
			this.lblExample29.Size = new System.Drawing.Size(17, 36);
			this.lblExample29.TabIndex = 0;
			this.lblExample29.Text = "e̙";
			this.lblExample29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample30
			// 
			this.lblExample30.BackColor = System.Drawing.Color.Transparent;
			this.lblExample30.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample30.Location = new System.Drawing.Point(539, 8);
			this.lblExample30.Name = "lblExample30";
			this.lblExample30.Size = new System.Drawing.Size(32, 36);
			this.lblExample30.TabIndex = 0;
			this.lblExample30.Text = "t̪  d̪";
			this.lblExample30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample31
			// 
			this.lblExample31.BackColor = System.Drawing.Color.Transparent;
			this.lblExample31.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample31.Location = new System.Drawing.Point(528, 48);
			this.lblExample31.Name = "lblExample31";
			this.lblExample31.Size = new System.Drawing.Size(37, 36);
			this.lblExample31.TabIndex = 0;
			this.lblExample31.Text = "t̺  d̺";
			this.lblExample31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample32
			// 
			this.lblExample32.BackColor = System.Drawing.Color.Transparent;
			this.lblExample32.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample32.Location = new System.Drawing.Point(528, 88);
			this.lblExample32.Name = "lblExample32";
			this.lblExample32.Size = new System.Drawing.Size(37, 36);
			this.lblExample32.TabIndex = 0;
			this.lblExample32.Text = "t̻  d̻";
			this.lblExample32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample33
			// 
			this.lblExample33.BackColor = System.Drawing.Color.Transparent;
			this.lblExample33.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample33.Location = new System.Drawing.Point(539, 128);
			this.lblExample33.Name = "lblExample33";
			this.lblExample33.Size = new System.Drawing.Size(32, 36);
			this.lblExample33.TabIndex = 0;
			this.lblExample33.Text = "ẽ";
			this.lblExample33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample34
			// 
			this.lblExample34.BackColor = System.Drawing.Color.Transparent;
			this.lblExample34.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample34.Location = new System.Drawing.Point(539, 168);
			this.lblExample34.Name = "lblExample34";
			this.lblExample34.Size = new System.Drawing.Size(32, 36);
			this.lblExample34.TabIndex = 0;
			this.lblExample34.Text = "dⁿ";
			this.lblExample34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample35
			// 
			this.lblExample35.BackColor = System.Drawing.Color.Transparent;
			this.lblExample35.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample35.Location = new System.Drawing.Point(539, 208);
			this.lblExample35.Name = "lblExample35";
			this.lblExample35.Size = new System.Drawing.Size(32, 36);
			this.lblExample35.TabIndex = 0;
			this.lblExample35.Text = "dˡ";
			this.lblExample35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExample36
			// 
			this.lblExample36.BackColor = System.Drawing.Color.Transparent;
			this.lblExample36.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblExample36.Location = new System.Drawing.Point(539, 248);
			this.lblExample36.Name = "lblExample36";
			this.lblExample36.Size = new System.Drawing.Size(32, 36);
			this.lblExample36.TabIndex = 0;
			this.lblExample36.Text = "d̚";
			this.lblExample36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label0
			// 
			this.label0.BackColor = System.Drawing.Color.Transparent;
			this.label0.Location = new System.Drawing.Point(56, 24);
			this.label0.Name = "label0";
			this.label0.Size = new System.Drawing.Size(64, 16);
			this.label0.TabIndex = 2;
			this.label0.Text = "Voiceless";
			this.label0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(56, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Voiced";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(56, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Aspirated";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(56, 144);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "More rounded";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(56, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.TabIndex = 2;
			this.label4.Text = "Less rounded";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Location = new System.Drawing.Point(56, 224);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "Advanced";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Location = new System.Drawing.Point(56, 264);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 2;
			this.label6.Text = "Retracted";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Location = new System.Drawing.Point(56, 304);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(88, 16);
			this.label7.TabIndex = 2;
			this.label7.Text = "Centralized";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.BackColor = System.Drawing.Color.Transparent;
			this.label8.Location = new System.Drawing.Point(56, 344);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 16);
			this.label8.TabIndex = 2;
			this.label8.Text = "Mid-centralized";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Location = new System.Drawing.Point(56, 384);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(88, 16);
			this.label9.TabIndex = 2;
			this.label9.Text = "Syllabic";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.Transparent;
			this.label10.Location = new System.Drawing.Point(56, 424);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(88, 16);
			this.label10.TabIndex = 2;
			this.label10.Text = "Non-syllabic";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(56, 464);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(88, 16);
			this.label11.TabIndex = 2;
			this.label11.Text = "Rhoticity";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Location = new System.Drawing.Point(231, 24);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(80, 16);
			this.label12.TabIndex = 2;
			this.label12.Text = "Breathy voiced";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.BackColor = System.Drawing.Color.Transparent;
			this.label13.Location = new System.Drawing.Point(231, 64);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(80, 16);
			this.label13.TabIndex = 2;
			this.label13.Text = "Creaky voiced";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.BackColor = System.Drawing.Color.Transparent;
			this.label14.Location = new System.Drawing.Point(231, 104);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(72, 16);
			this.label14.TabIndex = 2;
			this.label14.Text = "Linguolabial";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.BackColor = System.Drawing.Color.Transparent;
			this.label15.Location = new System.Drawing.Point(231, 144);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(56, 16);
			this.label15.TabIndex = 2;
			this.label15.Text = "Labialized";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label16
			// 
			this.label16.BackColor = System.Drawing.Color.Transparent;
			this.label16.Location = new System.Drawing.Point(231, 184);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(64, 16);
			this.label16.TabIndex = 2;
			this.label16.Text = "Palatalized";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label17
			// 
			this.label17.BackColor = System.Drawing.Color.Transparent;
			this.label17.Location = new System.Drawing.Point(231, 224);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(56, 16);
			this.label17.TabIndex = 2;
			this.label17.Text = "Velarized";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label18
			// 
			this.label18.BackColor = System.Drawing.Color.Transparent;
			this.label18.Location = new System.Drawing.Point(231, 264);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(88, 16);
			this.label18.TabIndex = 2;
			this.label18.Text = "Pharyngealized";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.BackColor = System.Drawing.Color.Transparent;
			this.label19.Location = new System.Drawing.Point(231, 304);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(152, 16);
			this.label19.TabIndex = 2;
			this.label19.Text = "Velarized or pharyngealized";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.BackColor = System.Drawing.Color.Transparent;
			this.label20.Location = new System.Drawing.Point(231, 344);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(40, 16);
			this.label20.TabIndex = 2;
			this.label20.Text = "Raised";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label21
			// 
			this.label21.BackColor = System.Drawing.Color.Transparent;
			this.label21.Location = new System.Drawing.Point(231, 384);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(48, 16);
			this.label21.TabIndex = 2;
			this.label21.Text = "Lowered";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.BackColor = System.Drawing.Color.Transparent;
			this.label22.Location = new System.Drawing.Point(231, 424);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(128, 16);
			this.label22.TabIndex = 2;
			this.label22.Text = "Advanced Tongue Root";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label23
			// 
			this.label23.BackColor = System.Drawing.Color.Transparent;
			this.label23.Location = new System.Drawing.Point(231, 464);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(128, 16);
			this.label23.TabIndex = 2;
			this.label23.Text = "Retracted Tongue Root";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label24
			// 
			this.label24.BackColor = System.Drawing.Color.Transparent;
			this.label24.Location = new System.Drawing.Point(429, 24);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(99, 16);
			this.label24.TabIndex = 2;
			this.label24.Text = "Dental";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label25
			// 
			this.label25.BackColor = System.Drawing.Color.Transparent;
			this.label25.Location = new System.Drawing.Point(429, 64);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(99, 16);
			this.label25.TabIndex = 2;
			this.label25.Text = "Apical";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label26
			// 
			this.label26.BackColor = System.Drawing.Color.Transparent;
			this.label26.Location = new System.Drawing.Point(429, 104);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(99, 16);
			this.label26.TabIndex = 2;
			this.label26.Text = "Laminal";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label27
			// 
			this.label27.BackColor = System.Drawing.Color.Transparent;
			this.label27.Location = new System.Drawing.Point(429, 144);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(99, 16);
			this.label27.TabIndex = 2;
			this.label27.Text = "Nasalized";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.BackColor = System.Drawing.Color.Transparent;
			this.label28.Location = new System.Drawing.Point(429, 184);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(99, 16);
			this.label28.TabIndex = 2;
			this.label28.Text = "Nasal release";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label29
			// 
			this.label29.BackColor = System.Drawing.Color.Transparent;
			this.label29.Location = new System.Drawing.Point(429, 224);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(99, 16);
			this.label29.TabIndex = 2;
			this.label29.Text = "Lateral release";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label30
			// 
			this.label30.BackColor = System.Drawing.Color.Transparent;
			this.label30.Location = new System.Drawing.Point(429, 264);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(99, 16);
			this.label30.TabIndex = 2;
			this.label30.Text = "No audible release";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DispDia
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(586, 495);
			this.Controls.Add(this.label0);
			this.Controls.Add(this.lblDia1);
			this.Controls.Add(this.lblDia0);
			this.Controls.Add(this.lblDia2);
			this.Controls.Add(this.lblDia3);
			this.Controls.Add(this.lblDia4);
			this.Controls.Add(this.lblDia5);
			this.Controls.Add(this.lblDia6);
			this.Controls.Add(this.lblDia7);
			this.Controls.Add(this.lblDia8);
			this.Controls.Add(this.lblDia9);
			this.Controls.Add(this.lblDia10);
			this.Controls.Add(this.lblDia11);
			this.Controls.Add(this.lblDia12);
			this.Controls.Add(this.lblDia13);
			this.Controls.Add(this.lblDia14);
			this.Controls.Add(this.lblDia15);
			this.Controls.Add(this.lblDia16);
			this.Controls.Add(this.lblDia17);
			this.Controls.Add(this.lblDia18);
			this.Controls.Add(this.lblDia19);
			this.Controls.Add(this.lblDia20);
			this.Controls.Add(this.lblDia21);
			this.Controls.Add(this.lblDia22);
			this.Controls.Add(this.lblDia23);
			this.Controls.Add(this.lblDia24);
			this.Controls.Add(this.lblDia25);
			this.Controls.Add(this.lblDia26);
			this.Controls.Add(this.lblDia27);
			this.Controls.Add(this.lblDia28);
			this.Controls.Add(this.lblDia29);
			this.Controls.Add(this.lblDia30);
			this.Controls.Add(this.lblExample0);
			this.Controls.Add(this.lblExample2);
			this.Controls.Add(this.lblExample3);
			this.Controls.Add(this.lblExample4);
			this.Controls.Add(this.lblExample6);
			this.Controls.Add(this.lblExample7);
			this.Controls.Add(this.lblExample8);
			this.Controls.Add(this.lblExample9);
			this.Controls.Add(this.lblExample10);
			this.Controls.Add(this.lblExample11);
			this.Controls.Add(this.lblExample12);
			this.Controls.Add(this.lblExample13);
			this.Controls.Add(this.lblExample14);
			this.Controls.Add(this.lblExample15);
			this.Controls.Add(this.lblExample16);
			this.Controls.Add(this.lblExample17);
			this.Controls.Add(this.lblExample18);
			this.Controls.Add(this.lblExample22);
			this.Controls.Add(this.lblExample23);
			this.Controls.Add(this.lblExample25);
			this.Controls.Add(this.lblExample27);
			this.Controls.Add(this.lblExample1);
			this.Controls.Add(this.lblExample5);
			this.Controls.Add(this.lblExample19);
			this.Controls.Add(this.lblExample20);
			this.Controls.Add(this.lblExample21);
			this.Controls.Add(this.lblExample24);
			this.Controls.Add(this.lblExample26);
			this.Controls.Add(this.lblExample28);
			this.Controls.Add(this.lblExample29);
			this.Controls.Add(this.lblExample30);
			this.Controls.Add(this.lblExample31);
			this.Controls.Add(this.lblExample32);
			this.Controls.Add(this.lblExample33);
			this.Controls.Add(this.lblExample34);
			this.Controls.Add(this.lblExample35);
			this.Controls.Add(this.lblExample36);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.label24);
			this.Controls.Add(this.label25);
			this.Controls.Add(this.label26);
			this.Controls.Add(this.label27);
			this.Controls.Add(this.label28);
			this.Controls.Add(this.label29);
			this.Controls.Add(this.label30);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximumSize = new System.Drawing.Size(592, 520);
			this.MinimumSize = new System.Drawing.Size(592, 520);
			this.Name = "DispDia";
			this.ShowInTaskbar = false;
			this.Text = "IPA Diacritics";
			this.ResumeLayout(false);

		}
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		private void lblDia_Click(object sender, EventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null || !lbl.Enabled)
				return;

			int i = int.Parse(lbl.Name.Substring(6));
			if (i == m_currentindex)
				return;
			
			m_lblDia[m_currentindex].BackColor = SystemColors.Control;
			m_lblDia[m_currentindex].ForeColor = SystemColors.ControlText;
			lbl.BackColor = SystemColors.Highlight;
			lbl.ForeColor = SystemColors.HighlightText;
			m_currentindex = i;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		private void lblDia_DoubleClick(object sender, EventArgs e)
		{
			if (!(((Label)sender).Enabled)) return;
			int i = int.Parse(((Label)sender).Name.Substring(6));
			if (i != m_currentindex)
			{
				m_lblDia[m_currentindex].BackColor = Color.Transparent;
				m_currentindex = i;
				((Label)sender).BackColor = Color.Teal;
			}
			MessageBox.Show("Should be searching for " + ((Label)sender).Text);
		}

		#region IxCoreColleague Members

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
			// TODO:  Add DispDia.Init implementation
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			// TODO:  Add DispDia.GetMessageTargets implementation
			return (IxCoreColleague[])(new IxCoreColleague[] {this});
		}

		#endregion
	}
}
