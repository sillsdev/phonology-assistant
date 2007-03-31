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
	/// <summary>
	/// DispVow displays charts showing vowels currently in the database
	/// </summary>
	public class DispVow : System.Windows.Forms.Form, IxCoreColleague
	{
		#region Member variables added by designer
		private System.Windows.Forms.TabPage tabAllVowels;
		private System.Windows.Forms.TabPage tabGenSum;
		private System.Windows.Forms.Button btnMoveRowDown;
		private System.Windows.Forms.Button btnMoveRowUp;
		private System.Windows.Forms.Button btnInsert;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Panel panYFixed;
		private System.Windows.Forms.Label lblRedMsg;
		private System.Windows.Forms.Panel panYScroll;
		private System.Windows.Forms.TabControl tabControl;
		/// <summary>
		/// Required designer variable
		/// </summary>
		private System.ComponentModel.IContainer components;		
		#endregion

		private Label [] m_lblVowel;
		private System.Windows.Forms.DataGrid dataGrid1;
		private ChartLine lineFront;
		private ChartLine lineCentral;
		private Panel pnlGenSummary;
		private SIL.Pa.Controls.XButton lbl0;
		private Label lblClose;
		private Label lblOpen;
		private Label lblCloseMid;
		private Label lblBack;
		private Label lblFront;
		private Label lblOpenMid;
		private Label lblCentral;
		private ChartLine linClose;
		private SIL.Pa.Controls.XButton xButton7;
		private SIL.Pa.Controls.XButton xButton6;
		private SIL.Pa.Controls.XButton xButton5;
		private SIL.Pa.Controls.XButton lbl1;
		private SIL.Pa.Controls.XButton xButton3;
		private SIL.Pa.Controls.XButton xButton2;
		private SIL.Pa.Controls.XButton xButton1;
		private int m_currentindex;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public DispVow()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			//m_lblVowel = new Label [] {lblVowel0, lblVowel1, lblVowel2,
			//                              lblVowel3, lblVowel4, lblVowel5,
			//                              lblVowel6, lblVowel7, lblVowel8,
			//                              lblVowel9, lblVowel10, lblVowel11,
			//                              lblVowel12, lblVowel13, lblVowel14,
			//                              lblVowel15, lblVowel16, lblVowel17,
			//                              lblVowel18, lblVowel19, lblVowel20,
			//                              lblVowel21, lblVowel22, lblVowel23,
			//                              lblVowel24, lblVowel25, lblVowel26,
			//                              lblVowel27};
			//foreach (Label lbl in m_lblVowel)
			//{
			//    lbl.Click += new EventHandler(lblVowel_Click);
			//    lbl.DoubleClick += new EventHandler(lblVowel_DoubleClick);
			//    lbl.Font = new Font (FontHelper.PhoneticFont.Name, 14);

			//    // Transparent looks funny when loading, but is better because it shows
			//    // label backgrounds through to the tab background (which is a bitmap 
			//    // with the dots and connecting bldr
			//    lbl.BackColor = Color.Transparent;

			//    lbl.ForeColor = SystemColors.ControlText;
			//    lbl.Enabled = false;
			//}
			m_currentindex = 0;

			// Access dB here
			if (ReadVowelsFromDB() == false)
				MessageBox.Show("Error accessing the database to enable appropriate vowels");

			MinimumSize = new Size(Width, Height);
			MaximumSize = new Size(Width, 0x7FFF);
			//Text = DBUtils.LanguageName + " Vowels";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if(disposing && components != null)
				components.Dispose();
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
			tabControl.SelectedIndex = PaApp.SettingsHandler.GetIntWindowValue(Name, "tab", 0);
			base.OnLoad(e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			PaApp.SettingsHandler.SaveWindowValue(Name, "tab", tabControl.SelectedIndex);
			PaApp.SettingsHandler.SaveFormProperties(this);
			base.OnFormClosing(e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DispVow));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabAllVowels = new System.Windows.Forms.TabPage();
			this.panYFixed = new System.Windows.Forms.Panel();
			this.panYScroll = new System.Windows.Forms.Panel();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnInsert = new System.Windows.Forms.Button();
			this.btnMoveRowUp = new System.Windows.Forms.Button();
			this.btnMoveRowDown = new System.Windows.Forms.Button();
			this.tabGenSum = new System.Windows.Forms.TabPage();
			this.lblRedMsg = new System.Windows.Forms.Label();
			this.pnlGenSummary = new System.Windows.Forms.Panel();
			this.xButton7 = new SIL.Pa.Controls.XButton();
			this.xButton6 = new SIL.Pa.Controls.XButton();
			this.xButton5 = new SIL.Pa.Controls.XButton();
			this.lbl1 = new SIL.Pa.Controls.XButton();
			this.xButton3 = new SIL.Pa.Controls.XButton();
			this.xButton2 = new SIL.Pa.Controls.XButton();
			this.xButton1 = new SIL.Pa.Controls.XButton();
			this.lblOpen = new System.Windows.Forms.Label();
			this.lblCloseMid = new System.Windows.Forms.Label();
			this.lblBack = new System.Windows.Forms.Label();
			this.lblFront = new System.Windows.Forms.Label();
			this.lblOpenMid = new System.Windows.Forms.Label();
			this.lblCentral = new System.Windows.Forms.Label();
			this.lblClose = new System.Windows.Forms.Label();
			this.lbl0 = new SIL.Pa.Controls.XButton();
			this.lineCentral = new SIL.Pa.ChartLine();
			this.linClose = new SIL.Pa.ChartLine();
			this.tabControl.SuspendLayout();
			this.tabAllVowels.SuspendLayout();
			this.panYFixed.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.pnlGenSummary.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabAllVowels);
			this.tabControl.Controls.Add(this.tabGenSum);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(456, 277);
			this.tabControl.TabIndex = 0;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl_KeyDown);
			// 
			// tabAllVowels
			// 
			this.tabAllVowels.Controls.Add(this.panYFixed);
			this.tabAllVowels.Controls.Add(this.dataGrid1);
			this.tabAllVowels.Controls.Add(this.btnDelete);
			this.tabAllVowels.Controls.Add(this.btnInsert);
			this.tabAllVowels.Controls.Add(this.btnMoveRowUp);
			this.tabAllVowels.Controls.Add(this.btnMoveRowDown);
			this.tabAllVowels.Location = new System.Drawing.Point(4, 22);
			this.tabAllVowels.Name = "tabAllVowels";
			this.tabAllVowels.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.tabAllVowels.Size = new System.Drawing.Size(448, 251);
			this.tabAllVowels.TabIndex = 0;
			this.tabAllVowels.Text = "All Vowels";
			// 
			// panYFixed
			// 
			this.panYFixed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panYFixed.Controls.Add(this.panYScroll);
			this.panYFixed.Location = new System.Drawing.Point(16, 16);
			this.panYFixed.Name = "panYFixed";
			this.panYFixed.Size = new System.Drawing.Size(56, 192);
			this.panYFixed.TabIndex = 5;
			// 
			// panYScroll
			// 
			this.panYScroll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panYScroll.Location = new System.Drawing.Point(9, 20);
			this.panYScroll.Name = "panYScroll";
			this.panYScroll.Size = new System.Drawing.Size(35, 153);
			this.panYScroll.TabIndex = 0;
			// 
			// dataGrid1
			// 
			this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.dataGrid1.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dataGrid1.CaptionBackColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.DataMember = "";
			this.dataGrid1.ForeColor = System.Drawing.SystemColors.Window;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(93, 16);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(320, 184);
			this.dataGrid1.TabIndex = 4;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnDelete.Location = new System.Drawing.Point(320, 208);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(96, 32);
			this.btnDelete.TabIndex = 3;
			this.btnDelete.Text = "De&lete Row";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnInsert
			// 
			this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnInsert.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnInsert.Location = new System.Drawing.Point(216, 208);
			this.btnInsert.Name = "btnInsert";
			this.btnInsert.Size = new System.Drawing.Size(96, 32);
			this.btnInsert.TabIndex = 2;
			this.btnInsert.Text = "&Insert Row";
			this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
			// 
			// btnMoveRowUp
			// 
			this.btnMoveRowUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveRowUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnMoveRowUp.Location = new System.Drawing.Point(112, 208);
			this.btnMoveRowUp.Name = "btnMoveRowUp";
			this.btnMoveRowUp.Size = new System.Drawing.Size(96, 32);
			this.btnMoveRowUp.TabIndex = 1;
			this.btnMoveRowUp.Text = "Move Row &Up";
			this.btnMoveRowUp.Click += new System.EventHandler(this.btnMoveRowUp_Click);
			// 
			// btnMoveRowDown
			// 
			this.btnMoveRowDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveRowDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnMoveRowDown.Location = new System.Drawing.Point(8, 208);
			this.btnMoveRowDown.Name = "btnMoveRowDown";
			this.btnMoveRowDown.Size = new System.Drawing.Size(96, 32);
			this.btnMoveRowDown.TabIndex = 0;
			this.btnMoveRowDown.Text = "Move Row Dow&n";
			this.btnMoveRowDown.Click += new System.EventHandler(this.btnMoveRowDown_Click);
			// 
			// tabGenSum
			// 
			this.tabGenSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabGenSum.Location = new System.Drawing.Point(4, 22);
			this.tabGenSum.Name = "tabGenSum";
			this.tabGenSum.Size = new System.Drawing.Size(448, 251);
			this.tabGenSum.TabIndex = 1;
			this.tabGenSum.Text = "General Summary";
			// 
			// lblRedMsg
			// 
			this.lblRedMsg.ForeColor = System.Drawing.Color.Red;
			this.lblRedMsg.Location = new System.Drawing.Point(216, 0);
			this.lblRedMsg.Name = "lblRedMsg";
			this.lblRedMsg.Size = new System.Drawing.Size(208, 16);
			this.lblRedMsg.TabIndex = 1;
			this.lblRedMsg.Text = "Red vowels do not occur as base only.";
			this.lblRedMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRedMsg.Visible = false;
			// 
			// pnlGenSummary
			// 
			this.pnlGenSummary.Controls.Add(this.xButton7);
			this.pnlGenSummary.Controls.Add(this.xButton6);
			this.pnlGenSummary.Controls.Add(this.xButton5);
			this.pnlGenSummary.Controls.Add(this.lbl1);
			this.pnlGenSummary.Controls.Add(this.xButton3);
			this.pnlGenSummary.Controls.Add(this.xButton2);
			this.pnlGenSummary.Controls.Add(this.xButton1);
			this.pnlGenSummary.Controls.Add(this.lblOpen);
			this.pnlGenSummary.Controls.Add(this.lblCloseMid);
			this.pnlGenSummary.Controls.Add(this.lblBack);
			this.pnlGenSummary.Controls.Add(this.lblFront);
			this.pnlGenSummary.Controls.Add(this.lblOpenMid);
			this.pnlGenSummary.Controls.Add(this.lblCentral);
			this.pnlGenSummary.Controls.Add(this.lblClose);
			this.pnlGenSummary.Controls.Add(this.lbl0);
			this.pnlGenSummary.Location = new System.Drawing.Point(497, 30);
			this.pnlGenSummary.Name = "pnlGenSummary";
			this.pnlGenSummary.Size = new System.Drawing.Size(448, 409);
			this.pnlGenSummary.TabIndex = 2;
			// 
			// xButton7
			// 
			this.xButton7.BackColor = System.Drawing.Color.Transparent;
			this.xButton7.DrawLeftArrowButton = false;
			this.xButton7.DrawRightArrowButton = false;
			this.xButton7.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xButton7.Image = null;
			this.xButton7.Location = new System.Drawing.Point(161, 252);
			this.xButton7.Name = "xButton7";
			this.xButton7.Size = new System.Drawing.Size(24, 24);
			this.xButton7.TabIndex = 14;
			this.xButton7.Text = "i";
			// 
			// xButton6
			// 
			this.xButton6.BackColor = System.Drawing.Color.Transparent;
			this.xButton6.DrawLeftArrowButton = false;
			this.xButton6.DrawRightArrowButton = false;
			this.xButton6.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xButton6.Image = null;
			this.xButton6.Location = new System.Drawing.Point(244, 278);
			this.xButton6.Name = "xButton6";
			this.xButton6.Size = new System.Drawing.Size(24, 24);
			this.xButton6.TabIndex = 13;
			this.xButton6.Text = "i";
			// 
			// xButton5
			// 
			this.xButton5.BackColor = System.Drawing.Color.Transparent;
			this.xButton5.DrawLeftArrowButton = false;
			this.xButton5.DrawRightArrowButton = false;
			this.xButton5.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xButton5.Image = null;
			this.xButton5.Location = new System.Drawing.Point(141, 326);
			this.xButton5.Name = "xButton5";
			this.xButton5.Size = new System.Drawing.Size(24, 24);
			this.xButton5.TabIndex = 12;
			this.xButton5.Text = "i";
			// 
			// lbl1
			// 
			this.lbl1.BackColor = System.Drawing.Color.Transparent;
			this.lbl1.DrawLeftArrowButton = false;
			this.lbl1.DrawRightArrowButton = false;
			this.lbl1.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl1.Image = null;
			this.lbl1.Location = new System.Drawing.Point(117, 32);
			this.lbl1.Name = "lbl1";
			this.lbl1.Size = new System.Drawing.Size(24, 24);
			this.lbl1.TabIndex = 11;
			this.lbl1.Text = "y";
			// 
			// xButton3
			// 
			this.xButton3.BackColor = System.Drawing.Color.Transparent;
			this.xButton3.DrawLeftArrowButton = false;
			this.xButton3.DrawRightArrowButton = false;
			this.xButton3.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xButton3.Image = null;
			this.xButton3.Location = new System.Drawing.Point(212, 156);
			this.xButton3.Name = "xButton3";
			this.xButton3.Size = new System.Drawing.Size(24, 24);
			this.xButton3.TabIndex = 10;
			this.xButton3.Text = "i";
			// 
			// xButton2
			// 
			this.xButton2.BackColor = System.Drawing.Color.Transparent;
			this.xButton2.DrawLeftArrowButton = false;
			this.xButton2.DrawRightArrowButton = false;
			this.xButton2.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xButton2.Image = null;
			this.xButton2.Location = new System.Drawing.Point(131, 183);
			this.xButton2.Name = "xButton2";
			this.xButton2.Size = new System.Drawing.Size(24, 24);
			this.xButton2.TabIndex = 9;
			this.xButton2.Text = "i";
			// 
			// xButton1
			// 
			this.xButton1.BackColor = System.Drawing.Color.Transparent;
			this.xButton1.DrawLeftArrowButton = false;
			this.xButton1.DrawRightArrowButton = false;
			this.xButton1.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xButton1.Image = null;
			this.xButton1.Location = new System.Drawing.Point(91, 290);
			this.xButton1.Name = "xButton1";
			this.xButton1.Size = new System.Drawing.Size(24, 24);
			this.xButton1.TabIndex = 8;
			this.xButton1.Text = "i";
			// 
			// lblOpen
			// 
			this.lblOpen.AutoSize = true;
			this.lblOpen.BackColor = System.Drawing.Color.Transparent;
			this.lblOpen.Location = new System.Drawing.Point(9, 210);
			this.lblOpen.Name = "lblOpen";
			this.lblOpen.Size = new System.Drawing.Size(33, 13);
			this.lblOpen.TabIndex = 7;
			this.lblOpen.Text = "Open";
			// 
			// lblCloseMid
			// 
			this.lblCloseMid.AutoSize = true;
			this.lblCloseMid.BackColor = System.Drawing.Color.Transparent;
			this.lblCloseMid.Location = new System.Drawing.Point(9, 91);
			this.lblCloseMid.Name = "lblCloseMid";
			this.lblCloseMid.Size = new System.Drawing.Size(52, 13);
			this.lblCloseMid.TabIndex = 6;
			this.lblCloseMid.Text = "Close-mid";
			// 
			// lblBack
			// 
			this.lblBack.AutoSize = true;
			this.lblBack.BackColor = System.Drawing.Color.Transparent;
			this.lblBack.Location = new System.Drawing.Point(332, 8);
			this.lblBack.Name = "lblBack";
			this.lblBack.Size = new System.Drawing.Size(32, 13);
			this.lblBack.TabIndex = 5;
			this.lblBack.Text = "Back";
			// 
			// lblFront
			// 
			this.lblFront.AutoSize = true;
			this.lblFront.BackColor = System.Drawing.Color.Transparent;
			this.lblFront.Location = new System.Drawing.Point(69, 8);
			this.lblFront.Name = "lblFront";
			this.lblFront.Size = new System.Drawing.Size(31, 13);
			this.lblFront.TabIndex = 4;
			this.lblFront.Text = "Front";
			// 
			// lblOpenMid
			// 
			this.lblOpenMid.AutoSize = true;
			this.lblOpenMid.BackColor = System.Drawing.Color.Transparent;
			this.lblOpenMid.Location = new System.Drawing.Point(9, 156);
			this.lblOpenMid.Name = "lblOpenMid";
			this.lblOpenMid.Size = new System.Drawing.Size(52, 13);
			this.lblOpenMid.TabIndex = 3;
			this.lblOpenMid.Text = "Open-mid";
			// 
			// lblCentral
			// 
			this.lblCentral.AutoSize = true;
			this.lblCentral.BackColor = System.Drawing.Color.Transparent;
			this.lblCentral.Location = new System.Drawing.Point(199, 6);
			this.lblCentral.Name = "lblCentral";
			this.lblCentral.Size = new System.Drawing.Size(40, 13);
			this.lblCentral.TabIndex = 2;
			this.lblCentral.Text = "Central";
			// 
			// lblClose
			// 
			this.lblClose.AutoSize = true;
			this.lblClose.BackColor = System.Drawing.Color.Transparent;
			this.lblClose.Location = new System.Drawing.Point(9, 39);
			this.lblClose.Name = "lblClose";
			this.lblClose.Size = new System.Drawing.Size(33, 13);
			this.lblClose.TabIndex = 1;
			this.lblClose.Text = "Close";
			// 
			// lbl0
			// 
			this.lbl0.BackColor = System.Drawing.Color.Transparent;
			this.lbl0.DrawLeftArrowButton = false;
			this.lbl0.DrawRightArrowButton = false;
			this.lbl0.Font = new System.Drawing.Font("Lucida Sans Unicode", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl0.Image = null;
			this.lbl0.Location = new System.Drawing.Point(91, 32);
			this.lbl0.Name = "lbl0";
			this.lbl0.Size = new System.Drawing.Size(24, 24);
			this.lbl0.TabIndex = 0;
			this.lbl0.Text = "i";
			// 
			// lineCentral
			// 
			this.lineCentral.Control = this.pnlGenSummary;
			this.lineCentral.EndDots = true;
			this.lineCentral.EndPoint = new System.Drawing.Point(308, 231);
			this.lineCentral.StartPoint = new System.Drawing.Point(254, 45);
			// 
			// linClose
			// 
			this.linClose.Control = this.pnlGenSummary;
			this.linClose.EndDots = true;
			this.linClose.EndPoint = new System.Drawing.Point(308, 45);
			this.linClose.StartPoint = new System.Drawing.Point(115, 45);
			// 
			// DispVow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(981, 462);
			this.Controls.Add(this.pnlGenSummary);
			this.Controls.Add(this.lblRedMsg);
			this.Controls.Add(this.tabControl);
			this.MaximizeBox = false;
			this.Name = "DispVow";
			this.Text = "IPA Vowels";
			this.tabControl.ResumeLayout(false);
			this.tabAllVowels.ResumeLayout(false);
			this.panYFixed.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.pnlGenSummary.ResumeLayout(false);
			this.pnlGenSummary.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			Refresh();
			if (tabControl.SelectedTab == tabAllVowels)
				lblRedMsg.Visible = false;
			else
			{
				lblRedMsg.Visible = true;

				if (m_lblVowel[m_currentindex].BackColor == Color.Transparent)
				{
					// Advance to the first enabled vowel; do not pass go, do not collect $200  --ALB
					for(; m_currentindex < m_lblVowel.Length && !m_lblVowel[m_currentindex].Enabled; 
						m_currentindex++)
						;
				}
				
				// Some vowel is enabled, and it's lable index is m_currentindex
				if(m_currentindex != m_lblVowel.Length) 
				{
					m_lblVowel[m_currentindex].BackColor = SystemColors.Highlight;
					m_lblVowel[m_currentindex].ForeColor = SystemColors.HighlightText;
				} 

				// No vowels are enabled
				else    
				{	
					// Return the index to zero and don't highlight anything
					m_currentindex = 0;
				}
			}
		}


		#region Methods of General Summary tab
		private void lblVowel_Click(object sender, EventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null || !lbl.Enabled)
				return;
			
			// Look up sound file path, and play if it exists
			string filename = PaApp.GetIpasoundPath(lbl.Text);
			if (filename != null)
			{
				Audio.WAVSounds ws = new Audio.WAVSounds();
				if (ws.Open(filename))
					ws.Play();
			}

			int i = getLblIndex(lbl);
			if (i == m_currentindex)
				return;

			// Only change labels' highlighting if we've clicked a different label
			m_lblVowel[m_currentindex].BackColor = Color.Transparent; //SystemColors.Control;
			m_lblVowel[m_currentindex].ForeColor = (System.Drawing.Color)m_lblVowel[m_currentindex].Tag;
			lbl.BackColor = SystemColors.Highlight;
			lbl.ForeColor = SystemColors.HighlightText;
			lbl.BringToFront();
			m_currentindex = i;

		}

		private void lblVowel_DoubleClick(object sender, EventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null || !lbl.Enabled) 
				return;

			m_currentindex = getLblIndex(lbl);

			// NEED CODE HERE
			MessageBox.Show("Should be searching for " + lbl.Text);
		}

		private void tabControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Modifiers != 0 || tabControl.SelectedTab != tabGenSum) 
				return;

			// Open up details about the vowel
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				e.Handled = true;
				lblVowel_DoubleClick(m_lblVowel[m_currentindex], e);
				return;
			}
			if (e.KeyCode == Keys.Space)
			{	
				e.Handled = true;
				lblVowel_Click(m_lblVowel[m_currentindex], e);
				return;
			}

			int i = m_currentindex;

			// Navigate to the right, wrapping down at the end of the line of vowels
			if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
			{
				e.Handled = true;

				// Wrap to the beginning when at the end of all vowels
				i = (m_currentindex == (m_lblVowel.Length - 1) ? 0 : m_currentindex + 1);

				// Skip over disabled vowels
				while (!m_lblVowel[i].Enabled && (i != m_currentindex))
					i = (i == (m_lblVowel.Length - 1) ? 0 : (i + 1));

				if (!m_lblVowel[i].Enabled)
					return;
	
				m_lblVowel[m_currentindex].BackColor = Color.Transparent; //SystemColors.Control;
				m_lblVowel[m_currentindex].ForeColor = (System.Drawing.Color)m_lblVowel[m_currentindex].Tag;
				m_lblVowel[i].BackColor = SystemColors.Highlight;
				m_lblVowel[i].ForeColor = SystemColors.HighlightText;
				m_lblVowel[i].BringToFront();

				m_currentindex = i;
				return;
			}

			// Navigate to the left, wrapping up at the end of the line of vowels
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
			{
				e.Handled = true;

				// Wrap to the end when at the beginning of all vowels
				i = ((m_currentindex == 0) ? (m_lblVowel.Length - 1) : m_currentindex - 1);

				// Skip over disabled vowels
				while (!m_lblVowel[i].Enabled && i != m_currentindex)
					i = ((i == 0) ? (m_lblVowel.Length - 1) : (i - 1));

				if (!m_lblVowel[i].Enabled)
					return;

				m_lblVowel[m_currentindex].BackColor = Color.Transparent; //SystemColors.Control;
				m_lblVowel[m_currentindex].ForeColor = (System.Drawing.Color)m_lblVowel[m_currentindex].Tag;
				m_lblVowel[i].BackColor = SystemColors.Highlight;
				m_lblVowel[i].ForeColor = SystemColors.HighlightText;
				m_lblVowel[i].BringToFront();

				m_currentindex = i;
				return;
			}

		}

		#endregion

		#region Methods of All Vowels tab
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnMoveRowDown_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnMoveRowUp_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnInsert_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Parse the label name to return its number as an index.
		/// </summary>
		/// <param name="ctrl">A label with name of the form "lblVowel###"</param>
		/// <returns>Index number ### for the label</returns>
		private int getLblIndex(Label lbl)
		{
			return int.Parse(lbl.Name.Substring(8));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disable/enable vowels according to the current database.
		/// </summary>
		/// <returns>True indicates success</returns>
		/// ------------------------------------------------------------------------------------
		private bool ReadVowelsFromDB() 
		{
			//string sSQL = "SELECT DISTINCTROW CharList.CharStr, IPACharacters.DsplyOrder " +
			//    "FROM (PhoneticList INNER JOIN (Folder INNER JOIN ((Document INNER JOIN " +
			//    "AllWordsIndex ON Document.DocID = AllWordsIndex.DocID) INNER JOIN " +
			//    "DocumentLinks ON Document.DocID = DocumentLinks.DocID) ON Folder.FolderID = " +
			//    "DocumentLinks.FolderID) ON PhoneticList.PhoneticListID = " +
			//    "AllWordsIndex.PhoneticListID) INNER JOIN (IPACharacters INNER JOIN " +
			//    "(CharList INNER JOIN CharIndex ON CharList.CharListID = " +
			//    "CharIndex.CharListID) ON IPACharacters.IPACharacterID = " +
			//    "CharList.IPACharacterID) ON PhoneticList.PhoneticListID = " +
			//    "CharIndex.PhoneticListID WHERE (((IPACharacters.CharType)=2) " +
			//    "AND ((CharList.TotalCount)>0))";

			//PaDataTable vowelTable;
			//try 
			//{
			//    vowelTable = new PaDataTable(sSQL);
			//}
			//catch (Exception e)
			//{
			//    MessageBox.Show("Error reading from database: \n" + e);
			//    return false;
			//}

			//// Go through all vowels returned by the query (as present in the database)
			//foreach (DataRow row in vowelTable.Rows) 
			//{
			//    int lblIndex = (System.Int16)row[DBFields.DsplyOrder];
			//    if (lblIndex < 0)
			//        continue;

			//    // Database is stored as 1-28, rather than 0-27, so need to shift indexing
			//    if (!m_lblVowel[lblIndex-1].Enabled)
			//    {
			//        m_lblVowel[lblIndex - 1].Enabled = true;
			//        m_lblVowel[lblIndex - 1].Font = new Font(FontHelper.PhoneticFont.Name, 14, FontStyle.Bold);
			//        m_lblVowel[lblIndex - 1].ForeColor = Color.Red;
			//        m_lblVowel[lblIndex - 1].Tag = Color.Red;
			//    }

			//    // Initially font was set to red, change to black only if the
			//    // vowel exists as not base only (i.e., ANSICharStr has more than one phone)
			//    string CharStr = (string)row[DBFields.CharStr];
			//    if (CharStr.Length == 1) 
			//    {
			//        m_lblVowel[lblIndex-1].ForeColor = SystemColors.ControlText;
			//        m_lblVowel[lblIndex-1].Tag = SystemColors.ControlText;
			//    }
			//}

			return true;
		}
		#endregion

		#region IxCoreColleague Members

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
			// TODO:  Add DispVow.Init implementation
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// ------------------------------------------------------------------------------------
		public IxCoreColleague[] GetMessageTargets()
		{
			// TODO:  Add DispVow.GetMessageTargets implementation

			//return (IxCoreColleague[])(new object[] {this});
			return (new IxCoreColleague[] {this});
		}

		#endregion

	}
}
