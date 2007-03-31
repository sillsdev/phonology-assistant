using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics.SymbolStore;
using SIL.SpeechTools.Database;
using XCore;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for DispSSeg.
	/// </summary>
	public class DispSSeg : System.Windows.Forms.Form, IxCoreColleague
	{
		#region Member variables added by designer
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
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label lblSS0;
		private System.Windows.Forms.Label lblSS1;
		private System.Windows.Forms.Label lblSS2;
		private System.Windows.Forms.Label lblSS3;
		private System.Windows.Forms.Label lblSS4;
		private System.Windows.Forms.Label lblSS5;
		private System.Windows.Forms.Label lblSS6;
		private System.Windows.Forms.Label lblSS7;
		private System.Windows.Forms.Label lblSS8;
		private System.Windows.Forms.Label lblSS9;
		private System.Windows.Forms.Label lblSS10;
		private System.Windows.Forms.Label lblSS11;
		private System.Windows.Forms.Label lblSS12;
		private System.Windows.Forms.Label lblSS13;
		private System.Windows.Forms.Label lblSS14;
		private System.Windows.Forms.Label lblSS15;
		private System.Windows.Forms.Label lblSS16;
		private System.Windows.Forms.Label lblSS17;
		private System.Windows.Forms.Label lblSS18;
		private System.Windows.Forms.Label lblSS19;
		private System.Windows.Forms.Label lblSS20;
		private System.Windows.Forms.Label lblSS21;
		private System.Windows.Forms.Label lblSS22;
		private System.Windows.Forms.Label lblSS23;
		private System.Windows.Forms.Label lblSS24;
		private System.Windows.Forms.Label lblSS25;
		private System.Windows.Forms.Label lblSS26;
		private System.Windows.Forms.Label lblSS27;
		private System.Windows.Forms.Label lblSS28;
		private System.Windows.Forms.Label lblSS29;
		private System.Windows.Forms.Label lblSS30;
		private System.Windows.Forms.Label lblSS31;
		private System.Windows.Forms.Label lblSS32;
		private System.Windows.Forms.Label lblSS33;
		private System.Windows.Forms.Label lblSS34;
		private System.Windows.Forms.Label lblSS35;
		private System.Windows.Forms.Label lblSS36;
		private System.Windows.Forms.Label lblSS37;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		private Label [] m_lblSS;
		private int m_currentindex;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public DispSSeg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			m_lblSS = new Label [] { lblSS0, lblSS1, lblSS2, lblSS3, lblSS4,
									   lblSS5, lblSS6, lblSS7, lblSS8, lblSS9,
									   lblSS10, lblSS11, lblSS12, lblSS13, lblSS14,
									   lblSS15, lblSS16, lblSS17, lblSS18, lblSS19,
									   lblSS20, lblSS21, lblSS22, lblSS23, lblSS24,
									   lblSS25, lblSS26, lblSS27, lblSS28, lblSS29,
									   lblSS30, lblSS31, lblSS32, lblSS33, lblSS34,
									   lblSS35, lblSS36, lblSS37};
			foreach (Label lbl in m_lblSS)
			{
				lbl.Click += new EventHandler(lblSS_Click);
				lbl.DoubleClick += new EventHandler(lblSS_DoubleClick);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name=""></param>
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
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			PaApp.SettingsHandler.LoadFormProperties(this);
			base.OnLoad(e);
			Text = DBUtils.LanguageName + " Suprasegmentals";
			while (!(m_lblSS[m_currentindex].Enabled))
				m_currentindex++;
			m_lblSS[m_currentindex].BackColor = Color.Teal;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
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
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			int i;
			if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Right))
			{
				e.Handled = true;
				i = ((m_currentindex == (m_lblSS.Length - 1)) ? 0 : m_currentindex + 1);
				while (!(m_lblSS[i].Enabled) && (i != m_currentindex))
					i = ((i == (m_lblSS.Length - 1)) ? 0 : (i + 1));
				m_lblSS[m_currentindex].BackColor = Color.Transparent;
				m_lblSS[i].BackColor = Color.Teal;
				m_currentindex = i;
			}
			else if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left))
			{
				e.Handled = true;
				i = ((m_currentindex == 0) ? (m_lblSS.Length - 1) : m_currentindex - 1);
				while (!(m_lblSS[i].Enabled) && (i != m_currentindex))
					i = ((i == 0) ? (m_lblSS.Length - 1) : (i - 1));
				m_lblSS[m_currentindex].BackColor = Color.Transparent;
				m_lblSS[i].BackColor = Color.Teal;
				m_currentindex = i;
			}
		}

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
			this.lblSS0 = new System.Windows.Forms.Label();
			this.lblSS1 = new System.Windows.Forms.Label();
			this.lblSS2 = new System.Windows.Forms.Label();
			this.lblSS3 = new System.Windows.Forms.Label();
			this.lblSS4 = new System.Windows.Forms.Label();
			this.lblSS5 = new System.Windows.Forms.Label();
			this.lblSS6 = new System.Windows.Forms.Label();
			this.lblSS7 = new System.Windows.Forms.Label();
			this.lblSS8 = new System.Windows.Forms.Label();
			this.lblSS9 = new System.Windows.Forms.Label();
			this.lblSS10 = new System.Windows.Forms.Label();
			this.lblSS11 = new System.Windows.Forms.Label();
			this.lblSS12 = new System.Windows.Forms.Label();
			this.lblSS13 = new System.Windows.Forms.Label();
			this.lblSS14 = new System.Windows.Forms.Label();
			this.lblSS15 = new System.Windows.Forms.Label();
			this.lblSS16 = new System.Windows.Forms.Label();
			this.lblSS17 = new System.Windows.Forms.Label();
			this.lblSS18 = new System.Windows.Forms.Label();
			this.lblSS19 = new System.Windows.Forms.Label();
			this.lblSS20 = new System.Windows.Forms.Label();
			this.lblSS21 = new System.Windows.Forms.Label();
			this.lblSS22 = new System.Windows.Forms.Label();
			this.lblSS23 = new System.Windows.Forms.Label();
			this.lblSS24 = new System.Windows.Forms.Label();
			this.lblSS25 = new System.Windows.Forms.Label();
			this.lblSS26 = new System.Windows.Forms.Label();
			this.lblSS27 = new System.Windows.Forms.Label();
			this.lblSS28 = new System.Windows.Forms.Label();
			this.lblSS29 = new System.Windows.Forms.Label();
			this.lblSS30 = new System.Windows.Forms.Label();
			this.lblSS31 = new System.Windows.Forms.Label();
			this.lblSS32 = new System.Windows.Forms.Label();
			this.lblSS33 = new System.Windows.Forms.Label();
			this.lblSS34 = new System.Windows.Forms.Label();
			this.lblSS35 = new System.Windows.Forms.Label();
			this.lblSS36 = new System.Windows.Forms.Label();
			this.lblSS37 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "SUPRASEGMENTALS";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(32, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Primary stress";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(32, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(92, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Secondary stress";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(32, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "Long";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(32, 112);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 16);
			this.label5.TabIndex = 0;
			this.label5.Text = "Half-long";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(32, 136);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 16);
			this.label6.TabIndex = 0;
			this.label6.Text = "Extra-short";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(32, 160);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(75, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "Syllable break";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(32, 192);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(94, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Minor (foot) group";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(32, 220);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(124, 16);
			this.label9.TabIndex = 0;
			this.label9.Text = "Major (intonation) group";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(32, 244);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(147, 16);
			this.label10.TabIndex = 0;
			this.label10.Text = "Linking (absence of a break)";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(352, 8);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(150, 16);
			this.label11.TabIndex = 0;
			this.label11.Text = "TONES && WORD ACCENTS";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(304, 32);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(39, 16);
			this.label12.TabIndex = 0;
			this.label12.Text = "LEVEL";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(280, 64);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(14, 16);
			this.label13.TabIndex = 0;
			this.label13.Text = "or";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(336, 64);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(14, 16);
			this.label14.TabIndex = 0;
			this.label14.Text = "or";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(384, 48);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(55, 16);
			this.label15.TabIndex = 0;
			this.label15.Text = "Extra high";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(384, 80);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(28, 16);
			this.label16.TabIndex = 0;
			this.label16.Text = "High";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(384, 120);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(22, 16);
			this.label17.TabIndex = 0;
			this.label17.Text = "Mid";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(384, 152);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(25, 16);
			this.label18.TabIndex = 0;
			this.label18.Text = "Low";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(384, 176);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(51, 16);
			this.label19.TabIndex = 0;
			this.label19.Text = "Extra low";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(296, 216);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(55, 16);
			this.label20.TabIndex = 0;
			this.label20.Text = "Downstep";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(296, 248);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(40, 16);
			this.label21.TabIndex = 0;
			this.label21.Text = "Upstep";
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(480, 32);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(61, 16);
			this.label22.TabIndex = 0;
			this.label22.Text = "CONTOUR";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(504, 64);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(14, 16);
			this.label23.TabIndex = 0;
			this.label23.Text = "or";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(560, 64);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(36, 16);
			this.label24.TabIndex = 0;
			this.label24.Text = "Rising";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(560, 96);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(38, 16);
			this.label25.TabIndex = 0;
			this.label25.Text = "Falling";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(560, 128);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(58, 16);
			this.label26.TabIndex = 0;
			this.label26.Text = "High rising";
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(560, 160);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(55, 16);
			this.label27.TabIndex = 0;
			this.label27.Text = "Low rising";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(560, 192);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(69, 16);
			this.label28.TabIndex = 0;
			this.label28.Text = "Rising-falling";
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Location = new System.Drawing.Point(512, 224);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(58, 16);
			this.label29.TabIndex = 0;
			this.label29.Text = "Global rise";
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(512, 256);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(55, 16);
			this.label30.TabIndex = 0;
			this.label30.Text = "Global fall";
			// 
			// lblSS0
			// 
			this.lblSS0.AutoSize = true;
			this.lblSS0.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS0.Location = new System.Drawing.Point(8, 32);
			this.lblSS0.Name = "lblSS0";
			this.lblSS0.Size = new System.Drawing.Size(12, 36);
			this.lblSS0.TabIndex = 0;
			this.lblSS0.Text = "ˈ";
			this.lblSS0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS1
			// 
			this.lblSS1.AutoSize = true;
			this.lblSS1.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS1.Location = new System.Drawing.Point(8, 56);
			this.lblSS1.Name = "lblSS1";
			this.lblSS1.Size = new System.Drawing.Size(12, 36);
			this.lblSS1.TabIndex = 0;
			this.lblSS1.Text = "ˌ";
			this.lblSS1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS2
			// 
			this.lblSS2.AutoSize = true;
			this.lblSS2.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS2.Location = new System.Drawing.Point(8, 80);
			this.lblSS2.Name = "lblSS2";
			this.lblSS2.Size = new System.Drawing.Size(12, 36);
			this.lblSS2.TabIndex = 0;
			this.lblSS2.Text = "ː";
			this.lblSS2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS3
			// 
			this.lblSS3.AutoSize = true;
			this.lblSS3.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS3.Location = new System.Drawing.Point(8, 104);
			this.lblSS3.Name = "lblSS3";
			this.lblSS3.Size = new System.Drawing.Size(12, 36);
			this.lblSS3.TabIndex = 0;
			this.lblSS3.Text = "ˑ";
			this.lblSS3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS4
			// 
			this.lblSS4.AutoSize = true;
			this.lblSS4.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS4.Location = new System.Drawing.Point(8, 128);
			this.lblSS4.Name = "lblSS4";
			this.lblSS4.Size = new System.Drawing.Size(14, 36);
			this.lblSS4.TabIndex = 0;
			this.lblSS4.Text = "˘";
			this.lblSS4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS5
			// 
			this.lblSS5.AutoSize = true;
			this.lblSS5.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS5.Location = new System.Drawing.Point(8, 152);
			this.lblSS5.Name = "lblSS5";
			this.lblSS5.Size = new System.Drawing.Size(13, 36);
			this.lblSS5.TabIndex = 0;
			this.lblSS5.Text = ".";
			this.lblSS5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS6
			// 
			this.lblSS6.AutoSize = true;
			this.lblSS6.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS6.Location = new System.Drawing.Point(9, 176);
			this.lblSS6.Name = "lblSS6";
			this.lblSS6.Size = new System.Drawing.Size(12, 36);
			this.lblSS6.TabIndex = 0;
			this.lblSS6.Text = "|";
			this.lblSS6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS7
			// 
			this.lblSS7.AutoSize = true;
			this.lblSS7.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS7.Location = new System.Drawing.Point(6, 208);
			this.lblSS7.Name = "lblSS7";
			this.lblSS7.Size = new System.Drawing.Size(19, 36);
			this.lblSS7.TabIndex = 0;
			this.lblSS7.Text = "‖";
			this.lblSS7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS8
			// 
			this.lblSS8.AutoSize = true;
			this.lblSS8.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS8.Location = new System.Drawing.Point(8, 232);
			this.lblSS8.Name = "lblSS8";
			this.lblSS8.Size = new System.Drawing.Size(14, 36);
			this.lblSS8.TabIndex = 0;
			this.lblSS8.Text = "‿";
			this.lblSS8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS9
			// 
			this.lblSS9.AutoSize = true;
			this.lblSS9.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS9.Location = new System.Drawing.Point(264, 40);
			this.lblSS9.Name = "lblSS9";
			this.lblSS9.Size = new System.Drawing.Size(17, 36);
			this.lblSS9.TabIndex = 0;
			this.lblSS9.Text = "e̋";
			this.lblSS9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS10
			// 
			this.lblSS10.AutoSize = true;
			this.lblSS10.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS10.Location = new System.Drawing.Point(304, 40);
			this.lblSS10.Name = "lblSS10";
			this.lblSS10.Size = new System.Drawing.Size(16, 36);
			this.lblSS10.TabIndex = 0;
			this.lblSS10.Text = "˥";
			this.lblSS10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS11
			// 
			this.lblSS11.AutoSize = true;
			this.lblSS11.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS11.Location = new System.Drawing.Point(360, 40);
			this.lblSS11.Name = "lblSS11";
			this.lblSS11.Size = new System.Drawing.Size(16, 36);
			this.lblSS11.TabIndex = 0;
			this.lblSS11.Text = "⁵";
			this.lblSS11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS12
			// 
			this.lblSS12.AutoSize = true;
			this.lblSS12.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS12.Location = new System.Drawing.Point(264, 72);
			this.lblSS12.Name = "lblSS12";
			this.lblSS12.Size = new System.Drawing.Size(17, 36);
			this.lblSS12.TabIndex = 0;
			this.lblSS12.Text = "é";
			this.lblSS12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS13
			// 
			this.lblSS13.AutoSize = true;
			this.lblSS13.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS13.Location = new System.Drawing.Point(304, 72);
			this.lblSS13.Name = "lblSS13";
			this.lblSS13.Size = new System.Drawing.Size(16, 36);
			this.lblSS13.TabIndex = 0;
			this.lblSS13.Text = "˦";
			this.lblSS13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS14
			// 
			this.lblSS14.AutoSize = true;
			this.lblSS14.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS14.Location = new System.Drawing.Point(360, 72);
			this.lblSS14.Name = "lblSS14";
			this.lblSS14.Size = new System.Drawing.Size(16, 36);
			this.lblSS14.TabIndex = 0;
			this.lblSS14.Text = "⁴";
			this.lblSS14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS15
			// 
			this.lblSS15.AutoSize = true;
			this.lblSS15.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS15.Location = new System.Drawing.Point(264, 104);
			this.lblSS15.Name = "lblSS15";
			this.lblSS15.Size = new System.Drawing.Size(17, 36);
			this.lblSS15.TabIndex = 0;
			this.lblSS15.Text = "ē";
			this.lblSS15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS16
			// 
			this.lblSS16.AutoSize = true;
			this.lblSS16.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS16.Location = new System.Drawing.Point(304, 104);
			this.lblSS16.Name = "lblSS16";
			this.lblSS16.Size = new System.Drawing.Size(16, 36);
			this.lblSS16.TabIndex = 0;
			this.lblSS16.Text = "˧";
			this.lblSS16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS17
			// 
			this.lblSS17.AutoSize = true;
			this.lblSS17.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS17.Location = new System.Drawing.Point(360, 104);
			this.lblSS17.Name = "lblSS17";
			this.lblSS17.Size = new System.Drawing.Size(16, 36);
			this.lblSS17.TabIndex = 0;
			this.lblSS17.Text = "³";
			this.lblSS17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS18
			// 
			this.lblSS18.AutoSize = true;
			this.lblSS18.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS18.Location = new System.Drawing.Point(264, 136);
			this.lblSS18.Name = "lblSS18";
			this.lblSS18.Size = new System.Drawing.Size(17, 36);
			this.lblSS18.TabIndex = 0;
			this.lblSS18.Text = "è";
			this.lblSS18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS19
			// 
			this.lblSS19.AutoSize = true;
			this.lblSS19.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS19.Location = new System.Drawing.Point(304, 136);
			this.lblSS19.Name = "lblSS19";
			this.lblSS19.Size = new System.Drawing.Size(16, 36);
			this.lblSS19.TabIndex = 0;
			this.lblSS19.Text = "˨";
			this.lblSS19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS20
			// 
			this.lblSS20.AutoSize = true;
			this.lblSS20.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS20.Location = new System.Drawing.Point(360, 136);
			this.lblSS20.Name = "lblSS20";
			this.lblSS20.Size = new System.Drawing.Size(16, 36);
			this.lblSS20.TabIndex = 0;
			this.lblSS20.Text = "²";
			this.lblSS20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS21
			// 
			this.lblSS21.AutoSize = true;
			this.lblSS21.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS21.Location = new System.Drawing.Point(264, 168);
			this.lblSS21.Name = "lblSS21";
			this.lblSS21.Size = new System.Drawing.Size(17, 36);
			this.lblSS21.TabIndex = 0;
			this.lblSS21.Text = "ȅ";
			this.lblSS21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS22
			// 
			this.lblSS22.AutoSize = true;
			this.lblSS22.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS22.Location = new System.Drawing.Point(304, 168);
			this.lblSS22.Name = "lblSS22";
			this.lblSS22.Size = new System.Drawing.Size(16, 36);
			this.lblSS22.TabIndex = 0;
			this.lblSS22.Text = "˩";
			this.lblSS22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS23
			// 
			this.lblSS23.AutoSize = true;
			this.lblSS23.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS23.Location = new System.Drawing.Point(360, 168);
			this.lblSS23.Name = "lblSS23";
			this.lblSS23.Size = new System.Drawing.Size(16, 36);
			this.lblSS23.TabIndex = 0;
			this.lblSS23.Text = "¹";
			this.lblSS23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS24
			// 
			this.lblSS24.AutoSize = true;
			this.lblSS24.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS24.Location = new System.Drawing.Point(264, 200);
			this.lblSS24.Name = "lblSS24";
			this.lblSS24.Size = new System.Drawing.Size(18, 36);
			this.lblSS24.TabIndex = 0;
			this.lblSS24.Text = "↓";
			this.lblSS24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS25
			// 
			this.lblSS25.AutoSize = true;
			this.lblSS25.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS25.Location = new System.Drawing.Point(264, 232);
			this.lblSS25.Name = "lblSS25";
			this.lblSS25.Size = new System.Drawing.Size(18, 36);
			this.lblSS25.TabIndex = 0;
			this.lblSS25.Text = "↑";
			this.lblSS25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS26
			// 
			this.lblSS26.AutoSize = true;
			this.lblSS26.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS26.Location = new System.Drawing.Point(472, 48);
			this.lblSS26.Name = "lblSS26";
			this.lblSS26.Size = new System.Drawing.Size(17, 36);
			this.lblSS26.TabIndex = 0;
			this.lblSS26.Text = "ě";
			this.lblSS26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS27
			// 
			this.lblSS27.AutoSize = true;
			this.lblSS27.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS27.Location = new System.Drawing.Point(528, 48);
			this.lblSS27.Name = "lblSS27";
			this.lblSS27.Size = new System.Drawing.Size(24, 36);
			this.lblSS27.TabIndex = 0;
			this.lblSS27.Text = "˩˥";
			this.lblSS27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS28
			// 
			this.lblSS28.AutoSize = true;
			this.lblSS28.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS28.Location = new System.Drawing.Point(472, 80);
			this.lblSS28.Name = "lblSS28";
			this.lblSS28.Size = new System.Drawing.Size(17, 36);
			this.lblSS28.TabIndex = 0;
			this.lblSS28.Text = "ê";
			this.lblSS28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS29
			// 
			this.lblSS29.AutoSize = true;
			this.lblSS29.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS29.Location = new System.Drawing.Point(528, 80);
			this.lblSS29.Name = "lblSS29";
			this.lblSS29.Size = new System.Drawing.Size(24, 36);
			this.lblSS29.TabIndex = 0;
			this.lblSS29.Text = "˥˩";
			this.lblSS29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS30
			// 
			this.lblSS30.AutoSize = true;
			this.lblSS30.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS30.Location = new System.Drawing.Point(472, 112);
			this.lblSS30.Name = "lblSS30";
			this.lblSS30.Size = new System.Drawing.Size(17, 36);
			this.lblSS30.TabIndex = 0;
			this.lblSS30.Text = "e";
			this.lblSS30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS31
			// 
			this.lblSS31.AutoSize = true;
			this.lblSS31.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS31.Location = new System.Drawing.Point(528, 112);
			this.lblSS31.Name = "lblSS31";
			this.lblSS31.Size = new System.Drawing.Size(24, 36);
			this.lblSS31.TabIndex = 0;
			this.lblSS31.Text = "˧˥";
			this.lblSS31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS32
			// 
			this.lblSS32.AutoSize = true;
			this.lblSS32.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS32.Location = new System.Drawing.Point(472, 144);
			this.lblSS32.Name = "lblSS32";
			this.lblSS32.Size = new System.Drawing.Size(17, 36);
			this.lblSS32.TabIndex = 0;
			this.lblSS32.Text = "e";
			this.lblSS32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS33
			// 
			this.lblSS33.AutoSize = true;
			this.lblSS33.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS33.Location = new System.Drawing.Point(528, 144);
			this.lblSS33.Name = "lblSS33";
			this.lblSS33.Size = new System.Drawing.Size(24, 36);
			this.lblSS33.TabIndex = 0;
			this.lblSS33.Text = "˩˧";
			this.lblSS33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS34
			// 
			this.lblSS34.AutoSize = true;
			this.lblSS34.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS34.Location = new System.Drawing.Point(472, 176);
			this.lblSS34.Name = "lblSS34";
			this.lblSS34.Size = new System.Drawing.Size(17, 36);
			this.lblSS34.TabIndex = 0;
			this.lblSS34.Text = "e";
			this.lblSS34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS35
			// 
			this.lblSS35.AutoSize = true;
			this.lblSS35.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS35.Location = new System.Drawing.Point(528, 176);
			this.lblSS35.Name = "lblSS35";
			this.lblSS35.Size = new System.Drawing.Size(33, 36);
			this.lblSS35.TabIndex = 0;
			this.lblSS35.Text = "˧˥˧";
			this.lblSS35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS36
			// 
			this.lblSS36.AutoSize = true;
			this.lblSS36.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS36.Location = new System.Drawing.Point(472, 208);
			this.lblSS36.Name = "lblSS36";
			this.lblSS36.Size = new System.Drawing.Size(29, 36);
			this.lblSS36.TabIndex = 0;
			this.lblSS36.Text = "↗";
			this.lblSS36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSS37
			// 
			this.lblSS37.AutoSize = true;
			this.lblSS37.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.lblSS37.Location = new System.Drawing.Point(472, 232);
			this.lblSS37.Name = "lblSS37";
			this.lblSS37.Size = new System.Drawing.Size(29, 36);
			this.lblSS37.TabIndex = 0;
			this.lblSS37.Text = "↘";
			this.lblSS37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.label31.Location = new System.Drawing.Point(120, 32);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(113, 36);
			this.label31.TabIndex = 0;
			this.label31.Text = "ˌfoʊnə ˈtɪʃən";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.label32.Location = new System.Drawing.Point(160, 72);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(28, 36);
			this.label32.TabIndex = 0;
			this.label32.Text = "e ː";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.label33.Location = new System.Drawing.Point(160, 96);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(28, 36);
			this.label33.TabIndex = 0;
			this.label33.Text = "e ˑ";
			this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.label34.Location = new System.Drawing.Point(160, 128);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(17, 36);
			this.label34.TabIndex = 0;
			this.label34.Text = "ĕ";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.Font = new System.Drawing.Font("Doulos SIL", 16F);
			this.label35.Location = new System.Drawing.Point(136, 152);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(59, 36);
			this.label35.TabIndex = 0;
			this.label35.Text = "ɹi.ækt";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DispSSeg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(642, 279);
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
			this.Controls.Add(this.lblSS0);
			this.Controls.Add(this.lblSS1);
			this.Controls.Add(this.lblSS2);
			this.Controls.Add(this.lblSS3);
			this.Controls.Add(this.lblSS4);
			this.Controls.Add(this.lblSS5);
			this.Controls.Add(this.lblSS6);
			this.Controls.Add(this.lblSS7);
			this.Controls.Add(this.lblSS8);
			this.Controls.Add(this.lblSS9);
			this.Controls.Add(this.lblSS10);
			this.Controls.Add(this.lblSS11);
			this.Controls.Add(this.lblSS14);
			this.Controls.Add(this.lblSS12);
			this.Controls.Add(this.lblSS13);
			this.Controls.Add(this.lblSS16);
			this.Controls.Add(this.lblSS15);
			this.Controls.Add(this.lblSS17);
			this.Controls.Add(this.lblSS18);
			this.Controls.Add(this.lblSS20);
			this.Controls.Add(this.lblSS19);
			this.Controls.Add(this.lblSS21);
			this.Controls.Add(this.lblSS22);
			this.Controls.Add(this.lblSS23);
			this.Controls.Add(this.lblSS24);
			this.Controls.Add(this.lblSS25);
			this.Controls.Add(this.lblSS26);
			this.Controls.Add(this.lblSS27);
			this.Controls.Add(this.lblSS28);
			this.Controls.Add(this.lblSS29);
			this.Controls.Add(this.lblSS30);
			this.Controls.Add(this.lblSS31);
			this.Controls.Add(this.lblSS32);
			this.Controls.Add(this.lblSS33);
			this.Controls.Add(this.lblSS34);
			this.Controls.Add(this.lblSS35);
			this.Controls.Add(this.lblSS36);
			this.Controls.Add(this.lblSS37);
			this.Controls.Add(this.label31);
			this.Controls.Add(this.label32);
			this.Controls.Add(this.label33);
			this.Controls.Add(this.label34);
			this.Controls.Add(this.label35);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(648, 304);
			this.MinimumSize = new System.Drawing.Size(648, 304);
			this.Name = "DispSSeg";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "IPA Suprasegmentals";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DispSSeg_KeyDown);
			this.ResumeLayout(false);

		}
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		private void lblSS_Click(object sender, EventArgs e)
		{
			if (!(((Label)sender).Enabled)) return;
			int i = int.Parse(((Label)sender).Name.Substring(5));
			if (i == m_currentindex) return;
			m_lblSS[m_currentindex].BackColor = Color.Transparent;
			((Label)sender).BackColor = Color.Teal;
			m_currentindex = i;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		private void lblSS_DoubleClick(object sender, EventArgs e)
		{
			if (!(((Label)sender).Enabled)) return;
			int i = int.Parse(((Label)sender).Name.Substring(5));
			if (i != m_currentindex)
			{
				m_lblSS[m_currentindex].BackColor = Color.Transparent;
				m_currentindex = i;
				((Label)sender).BackColor = Color.Teal;
			}
			MessageBox.Show("Should be searching for " + ((Label)sender).Text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		private void DispSSeg_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Modifiers > 0) return;
			else if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
			{
				lblSS_DoubleClick(m_lblSS[m_currentindex], e);
			}
			else if (e.KeyCode == Keys.Escape)
			{
				Dispose(true);
			}
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
			// TODO:  Add DispSSeg.Init implementation
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			// TODO:  Add DispSSeg.GetMessageTargets implementation
			//return (IxCoreColleague[])(new object[] { this });
			return (IxCoreColleague[])(new IxCoreColleague[] { this });
		}

		#endregion
	}
}
