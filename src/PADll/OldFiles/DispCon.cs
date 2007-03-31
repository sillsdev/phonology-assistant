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
	/// DispCon displays charts showing consonants currently in the database
	/// </summary>
	public class DispCon : System.Windows.Forms.Form, IxCoreColleague
	{
		#region Member variables added by designer
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabAllCons;
		private System.Windows.Forms.TabPage tabGenSum;
		private System.Windows.Forms.Label lblCons0;
		private System.Windows.Forms.Label lblCons1;
		private System.Windows.Forms.Label lblCons2;
		private System.Windows.Forms.Label lblCons3;
		private System.Windows.Forms.Label lblCons4;
		private System.Windows.Forms.Label lblCons5;
		private System.Windows.Forms.Label lblCons6;
		private System.Windows.Forms.Label lblCons7;
		private System.Windows.Forms.Label lblCons8;
		private System.Windows.Forms.Label lblCons9;
		private System.Windows.Forms.Label lblCons10;
		private System.Windows.Forms.Label lblCons11;
		private System.Windows.Forms.Label lblCons12;
		private System.Windows.Forms.Label lblCons13;
		private System.Windows.Forms.Label lblCons14;
		private System.Windows.Forms.Label lblCons15;
		private System.Windows.Forms.Label lblCons16;
		private System.Windows.Forms.Label lblCons17;
		private System.Windows.Forms.Label lblCons18;
		private System.Windows.Forms.Label lblCons19;
		private System.Windows.Forms.Label lblCons20;
		private System.Windows.Forms.Label lblCons21;
		private System.Windows.Forms.Label lblCons22;
		private System.Windows.Forms.Label lblCons23;
		private System.Windows.Forms.Label lblCons24;
		private System.Windows.Forms.Label lblCons25;
		private System.Windows.Forms.Label lblCons26;
		private System.Windows.Forms.Label lblCons27;
		private System.Windows.Forms.Label lblCons28;
		private System.Windows.Forms.Label lblCons29;
		private System.Windows.Forms.Label lblCons30;
		private System.Windows.Forms.Label lblCons31;
		private System.Windows.Forms.Label lblCons32;
		private System.Windows.Forms.Label lblCons33;
		private System.Windows.Forms.Label lblCons34;
		private System.Windows.Forms.Label lblCons35;
		private System.Windows.Forms.Label lblCons36;
		private System.Windows.Forms.Label lblCons37;
		private System.Windows.Forms.Label lblCons38;
		private System.Windows.Forms.Label lblCons39;
		private System.Windows.Forms.Label lblCons40;
		private System.Windows.Forms.Label lblCons41;
		private System.Windows.Forms.Label lblCons42;
		private System.Windows.Forms.Label lblCons43;
		private System.Windows.Forms.Label lblCons44;
		private System.Windows.Forms.Label lblCons45;
		private System.Windows.Forms.Label lblCons46;
		private System.Windows.Forms.Label lblCons47;
		private System.Windows.Forms.Label lblCons48;
		private System.Windows.Forms.Label lblCons49;
		private System.Windows.Forms.Label lblCons50;
		private System.Windows.Forms.Label lblCons51;
		private System.Windows.Forms.Label lblCons52;
		private System.Windows.Forms.Label lblCons53;
		private System.Windows.Forms.Label lblCons54;
		private System.Windows.Forms.Label lblCons55;
		private System.Windows.Forms.Label lblCons56;
		private System.Windows.Forms.Label lblCons57;
		private System.Windows.Forms.Label lblCons58;
		private System.Windows.Forms.Label lblCons59;
		private System.Windows.Forms.Label lblCons60;
		private System.Windows.Forms.Label lblCons61;
		private System.Windows.Forms.Label lblCons62;
		private System.Windows.Forms.Label lblCons63;
		private System.Windows.Forms.Label lblCons64;
		private System.Windows.Forms.Label lblCons65;
		private System.Windows.Forms.Label lblCons66;
		private System.Windows.Forms.Label lblCons67;
		private System.Windows.Forms.GroupBox grpNonPulmonic;
		private System.Windows.Forms.GroupBox grpOtherSymbols;
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
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.Label label44;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.Label label48;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.Panel panel12;
		private System.Windows.Forms.Panel panel13;
		private System.Windows.Forms.Panel panel14;
		private System.Windows.Forms.Panel panel15;
		private System.Windows.Forms.Panel panel16;
		private System.Windows.Forms.Panel panel17;
		private System.Windows.Forms.Panel panel18;
		private System.Windows.Forms.Panel panel19;
		private System.Windows.Forms.Panel panel20;
		private System.Windows.Forms.Panel panel21;
		private System.Windows.Forms.Panel panel22;
		private System.Windows.Forms.Panel panel23;
		private System.Windows.Forms.Panel panel24;
		private System.Windows.Forms.Panel panel25;
		private System.Windows.Forms.Panel panel26;
		private System.Windows.Forms.Panel panel27;
		private System.Windows.Forms.Panel panel28;
		private System.Windows.Forms.Panel panel29;
		private System.Windows.Forms.Panel panel30;
		private System.Windows.Forms.Panel panel31;
		private System.Windows.Forms.Panel panel32;
		private System.Windows.Forms.Panel panel33;
		private System.Windows.Forms.Panel panel34;
		private System.Windows.Forms.Panel panel35;
		private System.Windows.Forms.Panel panel36;
		private System.Windows.Forms.Panel panel37;
		private System.Windows.Forms.Panel panel38;
		private System.Windows.Forms.Panel panel39;
		private System.Windows.Forms.Panel panel40;
		private System.Windows.Forms.Panel panel41;
		private System.Windows.Forms.Panel panel42;
		private System.Windows.Forms.Panel panel43;
		private System.Windows.Forms.Panel panel44;
		private System.Windows.Forms.Panel panel45;
		private System.Windows.Forms.Panel panel46;
		private System.Windows.Forms.Panel panel47;
		private System.Windows.Forms.Panel panel48;
		private System.Windows.Forms.Panel panel49;
		private System.Windows.Forms.Panel panel50;
		private System.Windows.Forms.Panel panel51;
		private System.Windows.Forms.Panel panel52;
		private System.Windows.Forms.Panel panel53;
		private System.Windows.Forms.Panel panel54;
		private System.Windows.Forms.Panel panel55;
		private System.Windows.Forms.Panel panel56;
		private System.Windows.Forms.Panel panel57;
		private System.Windows.Forms.Panel panel58;
		private System.Windows.Forms.Panel panel59;
		private System.Windows.Forms.Panel panel60;
		private System.Windows.Forms.Panel panel61;
		private System.Windows.Forms.Panel panel62;
		private System.Windows.Forms.Panel panel63;
		private System.Windows.Forms.Panel panel64;
		private System.Windows.Forms.Panel panel65;
		private System.Windows.Forms.Panel panel66;
		private System.Windows.Forms.Panel panel67;
		private System.Windows.Forms.Panel panel68;
		private System.Windows.Forms.Panel panel69;
		private System.Windows.Forms.Panel panel70;
		private System.Windows.Forms.Panel panel71;
		private System.Windows.Forms.Panel panel72;
		private System.Windows.Forms.Panel panel73;
		private System.Windows.Forms.Panel panel74;
		private System.Windows.Forms.Panel panel75;
		private System.Windows.Forms.Panel panel76;
		private System.Windows.Forms.Label lblConSamp4;
		private System.Windows.Forms.Label lblConSamp5;
		private System.Windows.Forms.Label lblRedMsg;
		private System.Windows.Forms.Label lblConsLongBar;
		private System.Windows.Forms.Label lblConsShortBar;
		/// <summary>
		/// Required designer variable
		/// </summary>
		private System.ComponentModel.IContainer components;
		#endregion

		private Label [] m_lblCons;
		private System.Windows.Forms.Label lblCons83;
		private System.Windows.Forms.Label lblCons73;
		private System.Windows.Forms.Label lblCons74;
		private System.Windows.Forms.Label lblCons75;
		private System.Windows.Forms.Label lblCons76;
		private System.Windows.Forms.Label lblCons77;
		private System.Windows.Forms.Label lblCons78;
		private System.Windows.Forms.Label lblCons79;
		private System.Windows.Forms.Label lblCons80;
		private System.Windows.Forms.Label lblCons81;
		private System.Windows.Forms.Label lblCons82;
		private System.Windows.Forms.Label lblCons84;
		private System.Windows.Forms.Label lblCons69;
		private System.Windows.Forms.Label lblCons70;
		private System.Windows.Forms.Label lblCons72;
		private System.Windows.Forms.Label lblCons71;
		private System.Windows.Forms.Label lblCons68;
		private int m_currentindex;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public DispCon()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			m_lblCons = new Label [] {lblCons0, lblCons1, lblCons2, lblCons3, lblCons4,
										 lblCons5, lblCons6, lblCons7, lblCons8, lblCons9,
										 lblCons10, lblCons11, lblCons12, lblCons13, lblCons14,
										 lblCons15, lblCons16, lblCons17, lblCons18, lblCons19,
										 lblCons20, lblCons21, lblCons22, lblCons23, lblCons24,
										 lblCons25, lblCons26, lblCons27, lblCons28, lblCons29,
										 lblCons30, lblCons31, lblCons32, lblCons33, lblCons34,
										 lblCons35, lblCons36, lblCons37, lblCons38, lblCons39,
										 lblCons40, lblCons41, lblCons42, lblCons43, lblCons44,
										 lblCons45, lblCons46, lblCons47, lblCons48, lblCons49,
										 lblCons50, lblCons51, lblCons52, lblCons53, lblCons54,
										 lblCons55, lblCons56, lblCons57, lblCons58, lblCons59,
										 lblCons60, lblCons61, lblCons62, lblCons63, lblCons64,
										 lblCons65, lblCons66, lblCons67, lblCons68, lblCons69,
										 lblCons70, lblCons71, lblCons72, lblCons73, lblCons74,
										 lblCons75, lblCons76, lblCons77, lblCons78, lblCons79,
										 lblCons80, lblCons81, lblCons82, lblCons83, lblCons84};
			
			foreach (Label lbl in m_lblCons)
			{
				lbl.Click += new EventHandler(lblCons_Click);
				lbl.DoubleClick += new EventHandler(lblCons_DoubleClick);
				lbl.Font = new Font(FontHelper.PhoneticFont.Name, 14);
				lbl.ForeColor = SystemColors.ControlText;
				lbl.Enabled = false;
			}
			
			// Add additional event handlers to allow searching for ejectives and affricates 
			// selecting or playing sounds from these labels is not allowed
			lblCons68.DoubleClick += new EventHandler(lblCons_DoubleClick);
			lblConsLongBar.DoubleClick += new EventHandler(lblCons_DoubleClick);
			lblConsShortBar.DoubleClick += new EventHandler(lblCons_DoubleClick);

			m_currentindex = 0;

			// Access dB here
			if (ReadConsFromDB() == false)
				MessageBox.Show("Error accessing the database to enable appropriate consonants");

			MinimumSize = new Size(Width, Height);
			MaximumSize = new Size(Width, 0x7FFF);
			//Text = DBUtils.LanguageName + " Consonants";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose( bool disposing )
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
		/// <param name="e"></param>
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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabAllCons = new System.Windows.Forms.TabPage();
			this.tabGenSum = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblCons0 = new System.Windows.Forms.Label();
			this.lblCons1 = new System.Windows.Forms.Label();
			this.grpOtherSymbols = new System.Windows.Forms.GroupBox();
			this.lblConSamp5 = new System.Windows.Forms.Label();
			this.lblConSamp4 = new System.Windows.Forms.Label();
			this.label49 = new System.Windows.Forms.Label();
			this.label38 = new System.Windows.Forms.Label();
			this.label39 = new System.Windows.Forms.Label();
			this.label40 = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.label42 = new System.Windows.Forms.Label();
			this.label43 = new System.Windows.Forms.Label();
			this.label44 = new System.Windows.Forms.Label();
			this.label45 = new System.Windows.Forms.Label();
			this.label46 = new System.Windows.Forms.Label();
			this.label47 = new System.Windows.Forms.Label();
			this.lblCons83 = new System.Windows.Forms.Label();
			this.lblCons73 = new System.Windows.Forms.Label();
			this.lblCons74 = new System.Windows.Forms.Label();
			this.lblCons75 = new System.Windows.Forms.Label();
			this.lblCons76 = new System.Windows.Forms.Label();
			this.lblCons77 = new System.Windows.Forms.Label();
			this.lblCons78 = new System.Windows.Forms.Label();
			this.lblCons79 = new System.Windows.Forms.Label();
			this.lblCons80 = new System.Windows.Forms.Label();
			this.lblCons81 = new System.Windows.Forms.Label();
			this.lblCons82 = new System.Windows.Forms.Label();
			this.lblConsLongBar = new System.Windows.Forms.Label();
			this.lblConsShortBar = new System.Windows.Forms.Label();
			this.lblCons84 = new System.Windows.Forms.Label();
			this.label48 = new System.Windows.Forms.Label();
			this.grpNonPulmonic = new System.Windows.Forms.GroupBox();
			this.lblCons69 = new System.Windows.Forms.Label();
			this.lblCons70 = new System.Windows.Forms.Label();
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
			this.label31 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.label37 = new System.Windows.Forms.Label();
			this.lblCons72 = new System.Windows.Forms.Label();
			this.lblCons71 = new System.Windows.Forms.Label();
			this.lblCons58 = new System.Windows.Forms.Label();
			this.lblCons59 = new System.Windows.Forms.Label();
			this.lblCons60 = new System.Windows.Forms.Label();
			this.lblCons61 = new System.Windows.Forms.Label();
			this.lblCons62 = new System.Windows.Forms.Label();
			this.lblCons63 = new System.Windows.Forms.Label();
			this.lblCons64 = new System.Windows.Forms.Label();
			this.lblCons65 = new System.Windows.Forms.Label();
			this.lblCons66 = new System.Windows.Forms.Label();
			this.lblCons67 = new System.Windows.Forms.Label();
			this.lblCons68 = new System.Windows.Forms.Label();
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
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblCons13 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lblCons20 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel5 = new System.Windows.Forms.Panel();
			this.lblCons25 = new System.Windows.Forms.Label();
			this.lblCons26 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.panel7 = new System.Windows.Forms.Panel();
			this.panel8 = new System.Windows.Forms.Panel();
			this.panel9 = new System.Windows.Forms.Panel();
			this.panel10 = new System.Windows.Forms.Panel();
			this.panel11 = new System.Windows.Forms.Panel();
			this.lblCons14 = new System.Windows.Forms.Label();
			this.panel12 = new System.Windows.Forms.Panel();
			this.lblCons27 = new System.Windows.Forms.Label();
			this.lblCons28 = new System.Windows.Forms.Label();
			this.panel13 = new System.Windows.Forms.Panel();
			this.panel14 = new System.Windows.Forms.Panel();
			this.lblCons49 = new System.Windows.Forms.Label();
			this.panel15 = new System.Windows.Forms.Panel();
			this.panel16 = new System.Windows.Forms.Panel();
			this.panel17 = new System.Windows.Forms.Panel();
			this.lblCons54 = new System.Windows.Forms.Label();
			this.panel18 = new System.Windows.Forms.Panel();
			this.lblCons2 = new System.Windows.Forms.Label();
			this.lblCons3 = new System.Windows.Forms.Label();
			this.panel19 = new System.Windows.Forms.Panel();
			this.lblCons15 = new System.Windows.Forms.Label();
			this.panel20 = new System.Windows.Forms.Panel();
			this.lblCons29 = new System.Windows.Forms.Label();
			this.lblCons30 = new System.Windows.Forms.Label();
			this.panel21 = new System.Windows.Forms.Panel();
			this.lblCons21 = new System.Windows.Forms.Label();
			this.panel22 = new System.Windows.Forms.Panel();
			this.lblCons50 = new System.Windows.Forms.Label();
			this.panel23 = new System.Windows.Forms.Panel();
			this.lblCons48 = new System.Windows.Forms.Label();
			this.lblCons47 = new System.Windows.Forms.Label();
			this.panel24 = new System.Windows.Forms.Panel();
			this.lblCons23 = new System.Windows.Forms.Label();
			this.panel25 = new System.Windows.Forms.Panel();
			this.lblCons55 = new System.Windows.Forms.Label();
			this.panel26 = new System.Windows.Forms.Panel();
			this.lblCons4 = new System.Windows.Forms.Label();
			this.lblCons5 = new System.Windows.Forms.Label();
			this.panel27 = new System.Windows.Forms.Panel();
			this.lblCons16 = new System.Windows.Forms.Label();
			this.panel28 = new System.Windows.Forms.Panel();
			this.lblCons36 = new System.Windows.Forms.Label();
			this.lblCons35 = new System.Windows.Forms.Label();
			this.panel29 = new System.Windows.Forms.Panel();
			this.panel30 = new System.Windows.Forms.Panel();
			this.lblCons51 = new System.Windows.Forms.Label();
			this.panel31 = new System.Windows.Forms.Panel();
			this.panel32 = new System.Windows.Forms.Panel();
			this.lblCons24 = new System.Windows.Forms.Label();
			this.panel33 = new System.Windows.Forms.Panel();
			this.panel34 = new System.Windows.Forms.Panel();
			this.lblCons56 = new System.Windows.Forms.Label();
			this.panel35 = new System.Windows.Forms.Panel();
			this.lblCons6 = new System.Windows.Forms.Label();
			this.lblCons7 = new System.Windows.Forms.Label();
			this.panel36 = new System.Windows.Forms.Panel();
			this.lblCons17 = new System.Windows.Forms.Label();
			this.panel37 = new System.Windows.Forms.Panel();
			this.lblCons37 = new System.Windows.Forms.Label();
			this.lblCons38 = new System.Windows.Forms.Label();
			this.panel38 = new System.Windows.Forms.Panel();
			this.panel39 = new System.Windows.Forms.Panel();
			this.lblCons52 = new System.Windows.Forms.Label();
			this.panel40 = new System.Windows.Forms.Panel();
			this.panel41 = new System.Windows.Forms.Panel();
			this.panel42 = new System.Windows.Forms.Panel();
			this.lblCons57 = new System.Windows.Forms.Label();
			this.panel43 = new System.Windows.Forms.Panel();
			this.lblCons8 = new System.Windows.Forms.Label();
			this.lblCons9 = new System.Windows.Forms.Label();
			this.panel44 = new System.Windows.Forms.Panel();
			this.lblCons18 = new System.Windows.Forms.Label();
			this.panel45 = new System.Windows.Forms.Panel();
			this.lblCons40 = new System.Windows.Forms.Label();
			this.lblCons39 = new System.Windows.Forms.Label();
			this.panel46 = new System.Windows.Forms.Panel();
			this.panel47 = new System.Windows.Forms.Panel();
			this.lblCons53 = new System.Windows.Forms.Label();
			this.panel48 = new System.Windows.Forms.Panel();
			this.panel49 = new System.Windows.Forms.Panel();
			this.lblCons10 = new System.Windows.Forms.Label();
			this.lblCons11 = new System.Windows.Forms.Label();
			this.panel50 = new System.Windows.Forms.Panel();
			this.panel51 = new System.Windows.Forms.Panel();
			this.panel52 = new System.Windows.Forms.Panel();
			this.lblCons19 = new System.Windows.Forms.Label();
			this.panel53 = new System.Windows.Forms.Panel();
			this.lblCons42 = new System.Windows.Forms.Label();
			this.lblCons41 = new System.Windows.Forms.Label();
			this.panel54 = new System.Windows.Forms.Panel();
			this.lblCons22 = new System.Windows.Forms.Label();
			this.panel55 = new System.Windows.Forms.Panel();
			this.panel56 = new System.Windows.Forms.Panel();
			this.panel57 = new System.Windows.Forms.Panel();
			this.panel58 = new System.Windows.Forms.Panel();
			this.panel59 = new System.Windows.Forms.Panel();
			this.panel60 = new System.Windows.Forms.Panel();
			this.lblCons44 = new System.Windows.Forms.Label();
			this.lblCons43 = new System.Windows.Forms.Label();
			this.panel61 = new System.Windows.Forms.Panel();
			this.panel62 = new System.Windows.Forms.Panel();
			this.panel63 = new System.Windows.Forms.Panel();
			this.panel64 = new System.Windows.Forms.Panel();
			this.panel75 = new System.Windows.Forms.Panel();
			this.panel65 = new System.Windows.Forms.Panel();
			this.panel66 = new System.Windows.Forms.Panel();
			this.panel67 = new System.Windows.Forms.Panel();
			this.panel68 = new System.Windows.Forms.Panel();
			this.lblCons46 = new System.Windows.Forms.Label();
			this.lblCons45 = new System.Windows.Forms.Label();
			this.panel69 = new System.Windows.Forms.Panel();
			this.panel70 = new System.Windows.Forms.Panel();
			this.panel71 = new System.Windows.Forms.Panel();
			this.panel72 = new System.Windows.Forms.Panel();
			this.panel73 = new System.Windows.Forms.Panel();
			this.lblCons32 = new System.Windows.Forms.Label();
			this.lblCons31 = new System.Windows.Forms.Label();
			this.panel74 = new System.Windows.Forms.Panel();
			this.lblCons34 = new System.Windows.Forms.Label();
			this.lblCons33 = new System.Windows.Forms.Label();
			this.panel76 = new System.Windows.Forms.Panel();
			this.lblCons12 = new System.Windows.Forms.Label();
			this.lblRedMsg = new System.Windows.Forms.Label();
			this.tabControl.SuspendLayout();
			this.tabGenSum.SuspendLayout();
			this.panel1.SuspendLayout();
			this.grpOtherSymbols.SuspendLayout();
			this.grpNonPulmonic.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel11.SuspendLayout();
			this.panel12.SuspendLayout();
			this.panel14.SuspendLayout();
			this.panel17.SuspendLayout();
			this.panel18.SuspendLayout();
			this.panel19.SuspendLayout();
			this.panel20.SuspendLayout();
			this.panel21.SuspendLayout();
			this.panel22.SuspendLayout();
			this.panel23.SuspendLayout();
			this.panel24.SuspendLayout();
			this.panel25.SuspendLayout();
			this.panel26.SuspendLayout();
			this.panel27.SuspendLayout();
			this.panel28.SuspendLayout();
			this.panel30.SuspendLayout();
			this.panel32.SuspendLayout();
			this.panel34.SuspendLayout();
			this.panel35.SuspendLayout();
			this.panel36.SuspendLayout();
			this.panel37.SuspendLayout();
			this.panel39.SuspendLayout();
			this.panel42.SuspendLayout();
			this.panel43.SuspendLayout();
			this.panel44.SuspendLayout();
			this.panel45.SuspendLayout();
			this.panel47.SuspendLayout();
			this.panel49.SuspendLayout();
			this.panel52.SuspendLayout();
			this.panel53.SuspendLayout();
			this.panel54.SuspendLayout();
			this.panel60.SuspendLayout();
			this.panel64.SuspendLayout();
			this.panel68.SuspendLayout();
			this.panel73.SuspendLayout();
			this.panel74.SuspendLayout();
			this.panel76.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabAllCons);
			this.tabControl.Controls.Add(this.tabGenSum);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(656, 509);
			this.tabControl.TabIndex = 0;
			this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl_KeyDown);
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabAllCons
			// 
			this.tabAllCons.Location = new System.Drawing.Point(4, 22);
			this.tabAllCons.Name = "tabAllCons";
			this.tabAllCons.Size = new System.Drawing.Size(648, 483);
			this.tabAllCons.TabIndex = 1;
			this.tabAllCons.Text = "All Consonants";
			// 
			// tabGenSum
			// 
			this.tabGenSum.Controls.Add(this.panel1);
			this.tabGenSum.Controls.Add(this.grpOtherSymbols);
			this.tabGenSum.Controls.Add(this.grpNonPulmonic);
			this.tabGenSum.Controls.Add(this.label0);
			this.tabGenSum.Controls.Add(this.label1);
			this.tabGenSum.Controls.Add(this.label2);
			this.tabGenSum.Controls.Add(this.label3);
			this.tabGenSum.Controls.Add(this.label4);
			this.tabGenSum.Controls.Add(this.label5);
			this.tabGenSum.Controls.Add(this.label6);
			this.tabGenSum.Controls.Add(this.label7);
			this.tabGenSum.Controls.Add(this.label8);
			this.tabGenSum.Controls.Add(this.label9);
			this.tabGenSum.Controls.Add(this.label10);
			this.tabGenSum.Controls.Add(this.label11);
			this.tabGenSum.Controls.Add(this.label12);
			this.tabGenSum.Controls.Add(this.label13);
			this.tabGenSum.Controls.Add(this.label14);
			this.tabGenSum.Controls.Add(this.label15);
			this.tabGenSum.Controls.Add(this.label16);
			this.tabGenSum.Controls.Add(this.label17);
			this.tabGenSum.Controls.Add(this.label18);
			this.tabGenSum.Controls.Add(this.label19);
			this.tabGenSum.Controls.Add(this.panel2);
			this.tabGenSum.Controls.Add(this.panel3);
			this.tabGenSum.Controls.Add(this.panel4);
			this.tabGenSum.Controls.Add(this.panel5);
			this.tabGenSum.Controls.Add(this.panel6);
			this.tabGenSum.Controls.Add(this.panel7);
			this.tabGenSum.Controls.Add(this.panel8);
			this.tabGenSum.Controls.Add(this.panel9);
			this.tabGenSum.Controls.Add(this.panel10);
			this.tabGenSum.Controls.Add(this.panel11);
			this.tabGenSum.Controls.Add(this.panel12);
			this.tabGenSum.Controls.Add(this.panel13);
			this.tabGenSum.Controls.Add(this.panel14);
			this.tabGenSum.Controls.Add(this.panel15);
			this.tabGenSum.Controls.Add(this.panel16);
			this.tabGenSum.Controls.Add(this.panel17);
			this.tabGenSum.Controls.Add(this.panel18);
			this.tabGenSum.Controls.Add(this.panel19);
			this.tabGenSum.Controls.Add(this.panel20);
			this.tabGenSum.Controls.Add(this.panel21);
			this.tabGenSum.Controls.Add(this.panel22);
			this.tabGenSum.Controls.Add(this.panel23);
			this.tabGenSum.Controls.Add(this.panel24);
			this.tabGenSum.Controls.Add(this.panel25);
			this.tabGenSum.Controls.Add(this.panel26);
			this.tabGenSum.Controls.Add(this.panel27);
			this.tabGenSum.Controls.Add(this.panel28);
			this.tabGenSum.Controls.Add(this.panel29);
			this.tabGenSum.Controls.Add(this.panel30);
			this.tabGenSum.Controls.Add(this.panel31);
			this.tabGenSum.Controls.Add(this.panel32);
			this.tabGenSum.Controls.Add(this.panel33);
			this.tabGenSum.Controls.Add(this.panel34);
			this.tabGenSum.Controls.Add(this.panel35);
			this.tabGenSum.Controls.Add(this.panel36);
			this.tabGenSum.Controls.Add(this.panel37);
			this.tabGenSum.Controls.Add(this.panel38);
			this.tabGenSum.Controls.Add(this.panel39);
			this.tabGenSum.Controls.Add(this.panel40);
			this.tabGenSum.Controls.Add(this.panel41);
			this.tabGenSum.Controls.Add(this.panel42);
			this.tabGenSum.Controls.Add(this.panel43);
			this.tabGenSum.Controls.Add(this.panel44);
			this.tabGenSum.Controls.Add(this.panel45);
			this.tabGenSum.Controls.Add(this.panel46);
			this.tabGenSum.Controls.Add(this.panel47);
			this.tabGenSum.Controls.Add(this.panel48);
			this.tabGenSum.Controls.Add(this.panel49);
			this.tabGenSum.Controls.Add(this.panel50);
			this.tabGenSum.Controls.Add(this.panel51);
			this.tabGenSum.Controls.Add(this.panel52);
			this.tabGenSum.Controls.Add(this.panel53);
			this.tabGenSum.Controls.Add(this.panel54);
			this.tabGenSum.Controls.Add(this.panel55);
			this.tabGenSum.Controls.Add(this.panel56);
			this.tabGenSum.Controls.Add(this.panel57);
			this.tabGenSum.Controls.Add(this.panel58);
			this.tabGenSum.Controls.Add(this.panel59);
			this.tabGenSum.Controls.Add(this.panel60);
			this.tabGenSum.Controls.Add(this.panel61);
			this.tabGenSum.Controls.Add(this.panel62);
			this.tabGenSum.Controls.Add(this.panel63);
			this.tabGenSum.Controls.Add(this.panel64);
			this.tabGenSum.Controls.Add(this.panel65);
			this.tabGenSum.Controls.Add(this.panel66);
			this.tabGenSum.Controls.Add(this.panel67);
			this.tabGenSum.Controls.Add(this.panel68);
			this.tabGenSum.Controls.Add(this.panel69);
			this.tabGenSum.Controls.Add(this.panel70);
			this.tabGenSum.Controls.Add(this.panel71);
			this.tabGenSum.Controls.Add(this.panel72);
			this.tabGenSum.Controls.Add(this.panel73);
			this.tabGenSum.Controls.Add(this.panel74);
			this.tabGenSum.Controls.Add(this.panel76);
			this.tabGenSum.Location = new System.Drawing.Point(4, 22);
			this.tabGenSum.Name = "tabGenSum";
			this.tabGenSum.Size = new System.Drawing.Size(648, 483);
			this.tabGenSum.TabIndex = 0;
			this.tabGenSum.Text = "General Summary";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lblCons0);
			this.panel1.Controls.Add(this.lblCons1);
			this.panel1.Location = new System.Drawing.Point(88, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(48, 32);
			this.panel1.TabIndex = 4;
			// 
			// lblCons0
			// 
			this.lblCons0.BackColor = System.Drawing.Color.Transparent;
			this.lblCons0.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons0.Location = new System.Drawing.Point(1, 1);
			this.lblCons0.Name = "lblCons0";
			this.lblCons0.Size = new System.Drawing.Size(22, 28);
			this.lblCons0.TabIndex = 0;
			this.lblCons0.Text = "p";
			this.lblCons0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons1
			// 
			this.lblCons1.BackColor = System.Drawing.Color.Transparent;
			this.lblCons1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons1.Location = new System.Drawing.Point(23, 1);
			this.lblCons1.Name = "lblCons1";
			this.lblCons1.Size = new System.Drawing.Size(22, 28);
			this.lblCons1.TabIndex = 0;
			this.lblCons1.Text = "b";
			this.lblCons1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpOtherSymbols
			// 
			this.grpOtherSymbols.Controls.Add(this.lblConSamp5);
			this.grpOtherSymbols.Controls.Add(this.lblConSamp4);
			this.grpOtherSymbols.Controls.Add(this.label49);
			this.grpOtherSymbols.Controls.Add(this.label38);
			this.grpOtherSymbols.Controls.Add(this.label39);
			this.grpOtherSymbols.Controls.Add(this.label40);
			this.grpOtherSymbols.Controls.Add(this.label41);
			this.grpOtherSymbols.Controls.Add(this.label42);
			this.grpOtherSymbols.Controls.Add(this.label43);
			this.grpOtherSymbols.Controls.Add(this.label44);
			this.grpOtherSymbols.Controls.Add(this.label45);
			this.grpOtherSymbols.Controls.Add(this.label46);
			this.grpOtherSymbols.Controls.Add(this.label47);
			this.grpOtherSymbols.Controls.Add(this.lblCons83);
			this.grpOtherSymbols.Controls.Add(this.lblCons73);
			this.grpOtherSymbols.Controls.Add(this.lblCons74);
			this.grpOtherSymbols.Controls.Add(this.lblCons75);
			this.grpOtherSymbols.Controls.Add(this.lblCons76);
			this.grpOtherSymbols.Controls.Add(this.lblCons77);
			this.grpOtherSymbols.Controls.Add(this.lblCons78);
			this.grpOtherSymbols.Controls.Add(this.lblCons79);
			this.grpOtherSymbols.Controls.Add(this.lblCons80);
			this.grpOtherSymbols.Controls.Add(this.lblCons81);
			this.grpOtherSymbols.Controls.Add(this.lblCons82);
			this.grpOtherSymbols.Controls.Add(this.lblConsLongBar);
			this.grpOtherSymbols.Controls.Add(this.lblConsShortBar);
			this.grpOtherSymbols.Controls.Add(this.lblCons84);
			this.grpOtherSymbols.Controls.Add(this.label48);
			this.grpOtherSymbols.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpOtherSymbols.Location = new System.Drawing.Point(296, 296);
			this.grpOtherSymbols.Name = "grpOtherSymbols";
			this.grpOtherSymbols.Size = new System.Drawing.Size(352, 184);
			this.grpOtherSymbols.TabIndex = 2;
			this.grpOtherSymbols.TabStop = false;
			this.grpOtherSymbols.Text = "OTHER SYMBOLS";
			// 
			// lblConSamp5
			// 
			this.lblConSamp5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblConSamp5.Location = new System.Drawing.Point(320, 67);
			this.lblConSamp5.Name = "lblConSamp5";
			this.lblConSamp5.Size = new System.Drawing.Size(22, 28);
			this.lblConSamp5.TabIndex = 4;
			this.lblConSamp5.Text = "x";
			this.lblConSamp5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblConSamp4
			// 
			this.lblConSamp4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblConSamp4.Location = new System.Drawing.Point(280, 67);
			this.lblConSamp4.Name = "lblConSamp4";
			this.lblConSamp4.Size = new System.Drawing.Size(16, 28);
			this.lblConSamp4.TabIndex = 4;
			this.lblConSamp4.Text = "ʃ";
			this.lblConSamp4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label49
			// 
			this.label49.Location = new System.Drawing.Point(204, 162);
			this.label49.Name = "label49";
			this.label49.Size = new System.Drawing.Size(16, 16);
			this.label49.TabIndex = 3;
			this.label49.Text = "or";
			this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label38
			// 
			this.label38.Location = new System.Drawing.Point(32, 19);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(136, 16);
			this.label38.TabIndex = 3;
			this.label38.Text = "Voiceless labial-velar fric.";
			this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label39
			// 
			this.label39.Location = new System.Drawing.Point(32, 46);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(144, 16);
			this.label39.TabIndex = 3;
			this.label39.Text = "Voiced labial-velar approx.";
			this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label40
			// 
			this.label40.Location = new System.Drawing.Point(32, 73);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(152, 16);
			this.label40.TabIndex = 3;
			this.label40.Text = "Voiced labial-palatal approx.";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label41
			// 
			this.label41.Location = new System.Drawing.Point(32, 100);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(128, 16);
			this.label41.TabIndex = 3;
			this.label41.Text = "Voiceless epiglottal fric.";
			this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label42
			// 
			this.label42.Location = new System.Drawing.Point(32, 127);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(120, 16);
			this.label42.TabIndex = 3;
			this.label42.Text = "Voiceled epiglottal fric.";
			this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label43
			// 
			this.label43.Location = new System.Drawing.Point(32, 154);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(96, 16);
			this.label43.TabIndex = 3;
			this.label43.Text = "Epiglottal plosive";
			this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label44
			// 
			this.label44.Location = new System.Drawing.Point(213, 19);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(128, 16);
			this.label44.TabIndex = 3;
			this.label44.Text = "Alveolo-palatal fricatives";
			this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label45
			// 
			this.label45.Location = new System.Drawing.Point(213, 46);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(104, 16);
			this.label45.TabIndex = 3;
			this.label45.Text = "Alveolar lateral flap";
			this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label46
			// 
			this.label46.Location = new System.Drawing.Point(213, 73);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(80, 16);
			this.label46.TabIndex = 3;
			this.label46.Text = "Simultaneous";
			this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label47
			// 
			this.label47.Location = new System.Drawing.Point(296, 73);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(24, 16);
			this.label47.TabIndex = 3;
			this.label47.Text = "and";
			this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblCons83
			// 
			this.lblCons83.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons83.Location = new System.Drawing.Point(263, 154);
			this.lblCons83.Name = "lblCons83";
			this.lblCons83.Size = new System.Drawing.Size(32, 24);
			this.lblCons83.TabIndex = 4;
			this.lblCons83.Text = "k͡p";
			this.lblCons83.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblCons73
			// 
			this.lblCons73.BackColor = System.Drawing.Color.Transparent;
			this.lblCons73.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons73.Location = new System.Drawing.Point(8, 13);
			this.lblCons73.Name = "lblCons73";
			this.lblCons73.Size = new System.Drawing.Size(22, 28);
			this.lblCons73.TabIndex = 0;
			this.lblCons73.Text = "ʍ";
			this.lblCons73.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons74
			// 
			this.lblCons74.BackColor = System.Drawing.Color.Transparent;
			this.lblCons74.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons74.Location = new System.Drawing.Point(8, 40);
			this.lblCons74.Name = "lblCons74";
			this.lblCons74.Size = new System.Drawing.Size(22, 28);
			this.lblCons74.TabIndex = 0;
			this.lblCons74.Text = "w";
			this.lblCons74.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons75
			// 
			this.lblCons75.BackColor = System.Drawing.Color.Transparent;
			this.lblCons75.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons75.Location = new System.Drawing.Point(8, 67);
			this.lblCons75.Name = "lblCons75";
			this.lblCons75.Size = new System.Drawing.Size(22, 28);
			this.lblCons75.TabIndex = 0;
			this.lblCons75.Text = "ɥ";
			this.lblCons75.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons76
			// 
			this.lblCons76.BackColor = System.Drawing.Color.Transparent;
			this.lblCons76.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons76.Location = new System.Drawing.Point(8, 94);
			this.lblCons76.Name = "lblCons76";
			this.lblCons76.Size = new System.Drawing.Size(22, 28);
			this.lblCons76.TabIndex = 0;
			this.lblCons76.Text = "ʜ";
			this.lblCons76.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons77
			// 
			this.lblCons77.BackColor = System.Drawing.Color.Transparent;
			this.lblCons77.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons77.Location = new System.Drawing.Point(8, 121);
			this.lblCons77.Name = "lblCons77";
			this.lblCons77.Size = new System.Drawing.Size(22, 28);
			this.lblCons77.TabIndex = 0;
			this.lblCons77.Text = "ʢ";
			this.lblCons77.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons78
			// 
			this.lblCons78.BackColor = System.Drawing.Color.Transparent;
			this.lblCons78.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons78.Location = new System.Drawing.Point(8, 148);
			this.lblCons78.Name = "lblCons78";
			this.lblCons78.Size = new System.Drawing.Size(22, 28);
			this.lblCons78.TabIndex = 0;
			this.lblCons78.Text = "ʡ";
			this.lblCons78.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons79
			// 
			this.lblCons79.BackColor = System.Drawing.Color.Transparent;
			this.lblCons79.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons79.Location = new System.Drawing.Point(170, 13);
			this.lblCons79.Name = "lblCons79";
			this.lblCons79.Size = new System.Drawing.Size(22, 28);
			this.lblCons79.TabIndex = 0;
			this.lblCons79.Text = "ɕ";
			this.lblCons79.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons80
			// 
			this.lblCons80.BackColor = System.Drawing.Color.Transparent;
			this.lblCons80.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons80.Location = new System.Drawing.Point(191, 13);
			this.lblCons80.Name = "lblCons80";
			this.lblCons80.Size = new System.Drawing.Size(22, 28);
			this.lblCons80.TabIndex = 0;
			this.lblCons80.Text = "ʑ";
			this.lblCons80.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons81
			// 
			this.lblCons81.BackColor = System.Drawing.Color.Transparent;
			this.lblCons81.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons81.Location = new System.Drawing.Point(191, 40);
			this.lblCons81.Name = "lblCons81";
			this.lblCons81.Size = new System.Drawing.Size(22, 28);
			this.lblCons81.TabIndex = 0;
			this.lblCons81.Text = "ɺ";
			this.lblCons81.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons82
			// 
			this.lblCons82.BackColor = System.Drawing.Color.Transparent;
			this.lblCons82.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons82.Location = new System.Drawing.Point(191, 67);
			this.lblCons82.Name = "lblCons82";
			this.lblCons82.Size = new System.Drawing.Size(22, 28);
			this.lblCons82.TabIndex = 0;
			this.lblCons82.Text = "ɧ";
			this.lblCons82.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblConsLongBar
			// 
			this.lblConsLongBar.BackColor = System.Drawing.Color.Transparent;
			this.lblConsLongBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblConsLongBar.Location = new System.Drawing.Point(172, 154);
			this.lblConsLongBar.Name = "lblConsLongBar";
			this.lblConsLongBar.Size = new System.Drawing.Size(24, 24);
			this.lblConsLongBar.TabIndex = 0;
			this.lblConsLongBar.Text = " ͡  ";
			this.lblConsLongBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblConsShortBar
			// 
			this.lblConsShortBar.BackColor = System.Drawing.Color.Transparent;
			this.lblConsShortBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblConsShortBar.Location = new System.Drawing.Point(228, 154);
			this.lblConsShortBar.Name = "lblConsShortBar";
			this.lblConsShortBar.Size = new System.Drawing.Size(8, 24);
			this.lblConsShortBar.TabIndex = 0;
			this.lblConsShortBar.Text = "‿";
			this.lblConsShortBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons84
			// 
			this.lblCons84.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons84.Location = new System.Drawing.Point(296, 154);
			this.lblCons84.Name = "lblCons84";
			this.lblCons84.Size = new System.Drawing.Size(32, 24);
			this.lblCons84.TabIndex = 4;
			this.lblCons84.Text = "t‿s";
			this.lblCons84.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label48
			// 
			this.label48.Location = new System.Drawing.Point(173, 104);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(177, 40);
			this.label48.TabIndex = 3;
			this.label48.Text = "Affricates and double articulation can be represented by 2 symbols joined by a ti" +
				"e bar if necessary.";
			this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// grpNonPulmonic
			// 
			this.grpNonPulmonic.Controls.Add(this.lblCons69);
			this.grpNonPulmonic.Controls.Add(this.lblCons70);
			this.grpNonPulmonic.Controls.Add(this.label20);
			this.grpNonPulmonic.Controls.Add(this.label21);
			this.grpNonPulmonic.Controls.Add(this.label22);
			this.grpNonPulmonic.Controls.Add(this.label23);
			this.grpNonPulmonic.Controls.Add(this.label24);
			this.grpNonPulmonic.Controls.Add(this.label25);
			this.grpNonPulmonic.Controls.Add(this.label26);
			this.grpNonPulmonic.Controls.Add(this.label27);
			this.grpNonPulmonic.Controls.Add(this.label28);
			this.grpNonPulmonic.Controls.Add(this.label29);
			this.grpNonPulmonic.Controls.Add(this.label30);
			this.grpNonPulmonic.Controls.Add(this.label31);
			this.grpNonPulmonic.Controls.Add(this.label32);
			this.grpNonPulmonic.Controls.Add(this.label33);
			this.grpNonPulmonic.Controls.Add(this.label34);
			this.grpNonPulmonic.Controls.Add(this.label35);
			this.grpNonPulmonic.Controls.Add(this.label36);
			this.grpNonPulmonic.Controls.Add(this.label37);
			this.grpNonPulmonic.Controls.Add(this.lblCons72);
			this.grpNonPulmonic.Controls.Add(this.lblCons71);
			this.grpNonPulmonic.Controls.Add(this.lblCons58);
			this.grpNonPulmonic.Controls.Add(this.lblCons59);
			this.grpNonPulmonic.Controls.Add(this.lblCons60);
			this.grpNonPulmonic.Controls.Add(this.lblCons61);
			this.grpNonPulmonic.Controls.Add(this.lblCons62);
			this.grpNonPulmonic.Controls.Add(this.lblCons63);
			this.grpNonPulmonic.Controls.Add(this.lblCons64);
			this.grpNonPulmonic.Controls.Add(this.lblCons65);
			this.grpNonPulmonic.Controls.Add(this.lblCons66);
			this.grpNonPulmonic.Controls.Add(this.lblCons67);
			this.grpNonPulmonic.Controls.Add(this.lblCons68);
			this.grpNonPulmonic.Location = new System.Drawing.Point(8, 296);
			this.grpNonPulmonic.Name = "grpNonPulmonic";
			this.grpNonPulmonic.Size = new System.Drawing.Size(280, 184);
			this.grpNonPulmonic.TabIndex = 1;
			this.grpNonPulmonic.TabStop = false;
			this.grpNonPulmonic.Text = "(NON-PULMONIC)";
			// 
			// lblCons69
			// 
			this.lblCons69.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons69.Location = new System.Drawing.Point(184, 67);
			this.lblCons69.Name = "lblCons69";
			this.lblCons69.Size = new System.Drawing.Size(22, 28);
			this.lblCons69.TabIndex = 4;
			this.lblCons69.Text = "pʼ";
			this.lblCons69.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons70
			// 
			this.lblCons70.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons70.Location = new System.Drawing.Point(184, 94);
			this.lblCons70.Name = "lblCons70";
			this.lblCons70.Size = new System.Drawing.Size(22, 28);
			this.lblCons70.TabIndex = 5;
			this.lblCons70.Text = "tʼ";
			this.lblCons70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(8, 19);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(40, 16);
			this.label20.TabIndex = 3;
			this.label20.Text = "Clicks";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(32, 46);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(48, 16);
			this.label21.TabIndex = 3;
			this.label21.Text = "Bilabial";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(32, 73);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(40, 16);
			this.label22.TabIndex = 3;
			this.label22.Text = "Dental";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(32, 100);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(64, 16);
			this.label23.TabIndex = 3;
			this.label23.Text = "(Post) alvlr";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(32, 127);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(64, 16);
			this.label24.TabIndex = 3;
			this.label24.Text = "Palatoalvlr";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(32, 154);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(64, 16);
			this.label25.TabIndex = 3;
			this.label25.Text = "Alvlr lateral";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(88, 19);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(100, 16);
			this.label26.TabIndex = 3;
			this.label26.Text = "Voiced implosives";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(120, 46);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(48, 16);
			this.label27.TabIndex = 3;
			this.label27.Text = "Bilabial";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(120, 73);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(48, 16);
			this.label28.TabIndex = 3;
			this.label28.Text = "Dntl/alvlr";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(120, 100);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(40, 16);
			this.label29.TabIndex = 3;
			this.label29.Text = "Palatal";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(120, 127);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(32, 16);
			this.label30.TabIndex = 3;
			this.label30.Text = "Velar";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(128, 154);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(40, 16);
			this.label31.TabIndex = 3;
			this.label31.Text = "Uvular";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(192, 19);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(56, 16);
			this.label32.TabIndex = 3;
			this.label32.Text = "Ejectives";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(208, 46);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(64, 16);
			this.label33.TabIndex = 3;
			this.label33.Text = "Examples:";
			this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(208, 73);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(48, 16);
			this.label34.TabIndex = 3;
			this.label34.Text = "Bilabial";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(208, 100);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(48, 16);
			this.label35.TabIndex = 3;
			this.label35.Text = "Dntl/alvlr";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(208, 127);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(32, 16);
			this.label36.TabIndex = 3;
			this.label36.Text = "Velar";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(208, 154);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(48, 24);
			this.label37.TabIndex = 3;
			this.label37.Text = "Alveolar fricative";
			this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblCons72
			// 
			this.lblCons72.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons72.Location = new System.Drawing.Point(184, 148);
			this.lblCons72.Name = "lblCons72";
			this.lblCons72.Size = new System.Drawing.Size(22, 28);
			this.lblCons72.TabIndex = 5;
			this.lblCons72.Text = "sʼ";
			this.lblCons72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons71
			// 
			this.lblCons71.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons71.Location = new System.Drawing.Point(184, 121);
			this.lblCons71.Name = "lblCons71";
			this.lblCons71.Size = new System.Drawing.Size(22, 28);
			this.lblCons71.TabIndex = 4;
			this.lblCons71.Text = "kʼ";
			this.lblCons71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons58
			// 
			this.lblCons58.BackColor = System.Drawing.Color.Transparent;
			this.lblCons58.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons58.Location = new System.Drawing.Point(8, 40);
			this.lblCons58.Name = "lblCons58";
			this.lblCons58.Size = new System.Drawing.Size(22, 28);
			this.lblCons58.TabIndex = 0;
			this.lblCons58.Text = "ʘ";
			this.lblCons58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons59
			// 
			this.lblCons59.BackColor = System.Drawing.Color.Transparent;
			this.lblCons59.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons59.Location = new System.Drawing.Point(8, 67);
			this.lblCons59.Name = "lblCons59";
			this.lblCons59.Size = new System.Drawing.Size(22, 28);
			this.lblCons59.TabIndex = 0;
			this.lblCons59.Text = "ǀ";
			this.lblCons59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons60
			// 
			this.lblCons60.BackColor = System.Drawing.Color.Transparent;
			this.lblCons60.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons60.Location = new System.Drawing.Point(8, 94);
			this.lblCons60.Name = "lblCons60";
			this.lblCons60.Size = new System.Drawing.Size(22, 28);
			this.lblCons60.TabIndex = 0;
			this.lblCons60.Text = "ǃ";
			this.lblCons60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons61
			// 
			this.lblCons61.BackColor = System.Drawing.Color.Transparent;
			this.lblCons61.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons61.Location = new System.Drawing.Point(8, 121);
			this.lblCons61.Name = "lblCons61";
			this.lblCons61.Size = new System.Drawing.Size(22, 28);
			this.lblCons61.TabIndex = 0;
			this.lblCons61.Text = "ǂ";
			this.lblCons61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons62
			// 
			this.lblCons62.BackColor = System.Drawing.Color.Transparent;
			this.lblCons62.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons62.Location = new System.Drawing.Point(8, 148);
			this.lblCons62.Name = "lblCons62";
			this.lblCons62.Size = new System.Drawing.Size(22, 28);
			this.lblCons62.TabIndex = 0;
			this.lblCons62.Text = "ǁ";
			this.lblCons62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons63
			// 
			this.lblCons63.BackColor = System.Drawing.Color.Transparent;
			this.lblCons63.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons63.Location = new System.Drawing.Point(98, 40);
			this.lblCons63.Name = "lblCons63";
			this.lblCons63.Size = new System.Drawing.Size(22, 28);
			this.lblCons63.TabIndex = 0;
			this.lblCons63.Text = "ɓ";
			this.lblCons63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons64
			// 
			this.lblCons64.BackColor = System.Drawing.Color.Transparent;
			this.lblCons64.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons64.Location = new System.Drawing.Point(98, 67);
			this.lblCons64.Name = "lblCons64";
			this.lblCons64.Size = new System.Drawing.Size(22, 28);
			this.lblCons64.TabIndex = 0;
			this.lblCons64.Text = "ɗ";
			this.lblCons64.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons65
			// 
			this.lblCons65.BackColor = System.Drawing.Color.Transparent;
			this.lblCons65.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons65.Location = new System.Drawing.Point(98, 94);
			this.lblCons65.Name = "lblCons65";
			this.lblCons65.Size = new System.Drawing.Size(22, 28);
			this.lblCons65.TabIndex = 0;
			this.lblCons65.Text = "ʄ";
			this.lblCons65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons66
			// 
			this.lblCons66.BackColor = System.Drawing.Color.Transparent;
			this.lblCons66.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons66.Location = new System.Drawing.Point(98, 121);
			this.lblCons66.Name = "lblCons66";
			this.lblCons66.Size = new System.Drawing.Size(22, 28);
			this.lblCons66.TabIndex = 0;
			this.lblCons66.Text = "ɠ";
			this.lblCons66.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons67
			// 
			this.lblCons67.BackColor = System.Drawing.Color.Transparent;
			this.lblCons67.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons67.Location = new System.Drawing.Point(98, 148);
			this.lblCons67.Name = "lblCons67";
			this.lblCons67.Size = new System.Drawing.Size(22, 28);
			this.lblCons67.TabIndex = 0;
			this.lblCons67.Text = "ʛ";
			this.lblCons67.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons68
			// 
			this.lblCons68.BackColor = System.Drawing.Color.Transparent;
			this.lblCons68.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons68.Location = new System.Drawing.Point(184, 40);
			this.lblCons68.Name = "lblCons68";
			this.lblCons68.Size = new System.Drawing.Size(22, 28);
			this.lblCons68.TabIndex = 0;
			this.lblCons68.Text = "ʼ";
			this.lblCons68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label0
			// 
			this.label0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label0.Location = new System.Drawing.Point(8, 16);
			this.label0.Name = "label0";
			this.label0.Size = new System.Drawing.Size(80, 16);
			this.label0.TabIndex = 3;
			this.label0.Text = "(PULMONIC)";
			this.label0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new System.Drawing.Point(88, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Bilabial";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Location = new System.Drawing.Point(136, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Labioden";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(192, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Dental";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Location = new System.Drawing.Point(240, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Alveolar";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label5.Location = new System.Drawing.Point(288, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Postalv";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label6.Location = new System.Drawing.Point(336, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 16);
			this.label6.TabIndex = 3;
			this.label6.Text = "Retroflex";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label7.Location = new System.Drawing.Point(392, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(48, 16);
			this.label7.TabIndex = 3;
			this.label7.Text = "Palatal";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label8.Location = new System.Drawing.Point(440, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 16);
			this.label8.TabIndex = 3;
			this.label8.Text = "Velar";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label9
			// 
			this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label9.Location = new System.Drawing.Point(488, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(48, 16);
			this.label9.TabIndex = 3;
			this.label9.Text = "Uvular";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label10
			// 
			this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label10.Location = new System.Drawing.Point(536, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(56, 16);
			this.label10.TabIndex = 3;
			this.label10.Text = "Pharyngl";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label11
			// 
			this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label11.Location = new System.Drawing.Point(592, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(48, 16);
			this.label11.TabIndex = 3;
			this.label11.Text = "Glottal";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label12.Location = new System.Drawing.Point(8, 32);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(80, 32);
			this.label12.TabIndex = 3;
			this.label12.Text = "Plosive";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label13.Location = new System.Drawing.Point(8, 64);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(80, 32);
			this.label13.TabIndex = 3;
			this.label13.Text = "Nasal";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label14.Location = new System.Drawing.Point(8, 96);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(80, 32);
			this.label14.TabIndex = 3;
			this.label14.Text = "Trill";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label15.Location = new System.Drawing.Point(8, 128);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(80, 32);
			this.label15.TabIndex = 3;
			this.label15.Text = "Tap or Flap";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label16
			// 
			this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label16.Location = new System.Drawing.Point(8, 160);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(80, 32);
			this.label16.TabIndex = 3;
			this.label16.Text = "Fricative";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label17
			// 
			this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label17.Location = new System.Drawing.Point(8, 192);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(80, 32);
			this.label17.TabIndex = 3;
			this.label17.Text = "Lat. Fric.";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label18
			// 
			this.label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label18.Location = new System.Drawing.Point(8, 224);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(80, 32);
			this.label18.TabIndex = 3;
			this.label18.Text = "Approx.";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label19.Location = new System.Drawing.Point(8, 256);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(80, 32);
			this.label19.TabIndex = 3;
			this.label19.Text = "Lat. Approx.";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.lblCons13);
			this.panel2.Location = new System.Drawing.Point(88, 64);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(48, 32);
			this.panel2.TabIndex = 4;
			// 
			// lblCons13
			// 
			this.lblCons13.BackColor = System.Drawing.Color.Transparent;
			this.lblCons13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons13.Location = new System.Drawing.Point(23, 1);
			this.lblCons13.Name = "lblCons13";
			this.lblCons13.Size = new System.Drawing.Size(22, 28);
			this.lblCons13.TabIndex = 0;
			this.lblCons13.Text = "m";
			this.lblCons13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel3
			// 
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.lblCons20);
			this.panel3.Location = new System.Drawing.Point(88, 96);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(48, 32);
			this.panel3.TabIndex = 4;
			// 
			// lblCons20
			// 
			this.lblCons20.BackColor = System.Drawing.Color.Transparent;
			this.lblCons20.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons20.Location = new System.Drawing.Point(23, 1);
			this.lblCons20.Name = "lblCons20";
			this.lblCons20.Size = new System.Drawing.Size(22, 28);
			this.lblCons20.TabIndex = 0;
			this.lblCons20.Text = "ʙ";
			this.lblCons20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel4
			// 
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel4.Location = new System.Drawing.Point(88, 128);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(48, 32);
			this.panel4.TabIndex = 4;
			// 
			// panel5
			// 
			this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel5.Controls.Add(this.lblCons25);
			this.panel5.Controls.Add(this.lblCons26);
			this.panel5.Location = new System.Drawing.Point(88, 160);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(48, 32);
			this.panel5.TabIndex = 4;
			// 
			// lblCons25
			// 
			this.lblCons25.BackColor = System.Drawing.Color.Transparent;
			this.lblCons25.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons25.Location = new System.Drawing.Point(1, 1);
			this.lblCons25.Name = "lblCons25";
			this.lblCons25.Size = new System.Drawing.Size(22, 28);
			this.lblCons25.TabIndex = 0;
			this.lblCons25.Text = "ɸ";
			this.lblCons25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons26
			// 
			this.lblCons26.BackColor = System.Drawing.Color.Transparent;
			this.lblCons26.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons26.Location = new System.Drawing.Point(23, 1);
			this.lblCons26.Name = "lblCons26";
			this.lblCons26.Size = new System.Drawing.Size(22, 28);
			this.lblCons26.TabIndex = 0;
			this.lblCons26.Text = "β";
			this.lblCons26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel6.Location = new System.Drawing.Point(88, 192);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(48, 32);
			this.panel6.TabIndex = 4;
			// 
			// panel7
			// 
			this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel7.Location = new System.Drawing.Point(88, 224);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(48, 32);
			this.panel7.TabIndex = 4;
			// 
			// panel8
			// 
			this.panel8.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel8.Location = new System.Drawing.Point(88, 256);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(48, 32);
			this.panel8.TabIndex = 4;
			// 
			// panel9
			// 
			this.panel9.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel9.Location = new System.Drawing.Point(136, 256);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(56, 32);
			this.panel9.TabIndex = 4;
			// 
			// panel10
			// 
			this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel10.Location = new System.Drawing.Point(136, 32);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(56, 32);
			this.panel10.TabIndex = 4;
			// 
			// panel11
			// 
			this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel11.Controls.Add(this.lblCons14);
			this.panel11.Location = new System.Drawing.Point(136, 64);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(56, 32);
			this.panel11.TabIndex = 4;
			// 
			// lblCons14
			// 
			this.lblCons14.BackColor = System.Drawing.Color.Transparent;
			this.lblCons14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons14.Location = new System.Drawing.Point(31, 1);
			this.lblCons14.Name = "lblCons14";
			this.lblCons14.Size = new System.Drawing.Size(22, 28);
			this.lblCons14.TabIndex = 0;
			this.lblCons14.Text = "ɱ";
			this.lblCons14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel12
			// 
			this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel12.Controls.Add(this.lblCons27);
			this.panel12.Controls.Add(this.lblCons28);
			this.panel12.Location = new System.Drawing.Point(136, 160);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(56, 32);
			this.panel12.TabIndex = 4;
			// 
			// lblCons27
			// 
			this.lblCons27.BackColor = System.Drawing.Color.Transparent;
			this.lblCons27.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons27.Location = new System.Drawing.Point(1, 1);
			this.lblCons27.Name = "lblCons27";
			this.lblCons27.Size = new System.Drawing.Size(22, 28);
			this.lblCons27.TabIndex = 0;
			this.lblCons27.Text = "f";
			this.lblCons27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons28
			// 
			this.lblCons28.BackColor = System.Drawing.Color.Transparent;
			this.lblCons28.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons28.Location = new System.Drawing.Point(31, 1);
			this.lblCons28.Name = "lblCons28";
			this.lblCons28.Size = new System.Drawing.Size(22, 28);
			this.lblCons28.TabIndex = 0;
			this.lblCons28.Text = "v";
			this.lblCons28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel13
			// 
			this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel13.Location = new System.Drawing.Point(136, 96);
			this.panel13.Name = "panel13";
			this.panel13.Size = new System.Drawing.Size(56, 32);
			this.panel13.TabIndex = 4;
			// 
			// panel14
			// 
			this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel14.Controls.Add(this.lblCons49);
			this.panel14.Location = new System.Drawing.Point(136, 224);
			this.panel14.Name = "panel14";
			this.panel14.Size = new System.Drawing.Size(56, 32);
			this.panel14.TabIndex = 4;
			// 
			// lblCons49
			// 
			this.lblCons49.BackColor = System.Drawing.Color.Transparent;
			this.lblCons49.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons49.Location = new System.Drawing.Point(31, 1);
			this.lblCons49.Name = "lblCons49";
			this.lblCons49.Size = new System.Drawing.Size(22, 28);
			this.lblCons49.TabIndex = 0;
			this.lblCons49.Text = "ʋ";
			this.lblCons49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel15
			// 
			this.panel15.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel15.Location = new System.Drawing.Point(136, 192);
			this.panel15.Name = "panel15";
			this.panel15.Size = new System.Drawing.Size(56, 32);
			this.panel15.TabIndex = 4;
			// 
			// panel16
			// 
			this.panel16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel16.Location = new System.Drawing.Point(136, 128);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(56, 32);
			this.panel16.TabIndex = 4;
			// 
			// panel17
			// 
			this.panel17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel17.Controls.Add(this.lblCons54);
			this.panel17.Location = new System.Drawing.Point(192, 256);
			this.panel17.Name = "panel17";
			this.panel17.Size = new System.Drawing.Size(144, 32);
			this.panel17.TabIndex = 4;
			// 
			// lblCons54
			// 
			this.lblCons54.BackColor = System.Drawing.Color.Transparent;
			this.lblCons54.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons54.Location = new System.Drawing.Point(71, 1);
			this.lblCons54.Name = "lblCons54";
			this.lblCons54.Size = new System.Drawing.Size(22, 28);
			this.lblCons54.TabIndex = 0;
			this.lblCons54.Text = "l";
			this.lblCons54.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel18
			// 
			this.panel18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel18.Controls.Add(this.lblCons2);
			this.panel18.Controls.Add(this.lblCons3);
			this.panel18.Location = new System.Drawing.Point(192, 32);
			this.panel18.Name = "panel18";
			this.panel18.Size = new System.Drawing.Size(144, 32);
			this.panel18.TabIndex = 4;
			// 
			// lblCons2
			// 
			this.lblCons2.BackColor = System.Drawing.Color.Transparent;
			this.lblCons2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons2.Location = new System.Drawing.Point(49, 1);
			this.lblCons2.Name = "lblCons2";
			this.lblCons2.Size = new System.Drawing.Size(22, 28);
			this.lblCons2.TabIndex = 0;
			this.lblCons2.Text = "t";
			this.lblCons2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons3
			// 
			this.lblCons3.BackColor = System.Drawing.Color.Transparent;
			this.lblCons3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons3.Location = new System.Drawing.Point(71, 1);
			this.lblCons3.Name = "lblCons3";
			this.lblCons3.Size = new System.Drawing.Size(22, 28);
			this.lblCons3.TabIndex = 0;
			this.lblCons3.Text = "d";
			this.lblCons3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel19
			// 
			this.panel19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel19.Controls.Add(this.lblCons15);
			this.panel19.Location = new System.Drawing.Point(192, 64);
			this.panel19.Name = "panel19";
			this.panel19.Size = new System.Drawing.Size(144, 32);
			this.panel19.TabIndex = 4;
			// 
			// lblCons15
			// 
			this.lblCons15.BackColor = System.Drawing.Color.Transparent;
			this.lblCons15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons15.Location = new System.Drawing.Point(71, 1);
			this.lblCons15.Name = "lblCons15";
			this.lblCons15.Size = new System.Drawing.Size(22, 28);
			this.lblCons15.TabIndex = 0;
			this.lblCons15.Text = "n";
			this.lblCons15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel20
			// 
			this.panel20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel20.Controls.Add(this.lblCons29);
			this.panel20.Controls.Add(this.lblCons30);
			this.panel20.Location = new System.Drawing.Point(192, 160);
			this.panel20.Name = "panel20";
			this.panel20.Size = new System.Drawing.Size(48, 32);
			this.panel20.TabIndex = 4;
			// 
			// lblCons29
			// 
			this.lblCons29.BackColor = System.Drawing.Color.Transparent;
			this.lblCons29.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons29.Location = new System.Drawing.Point(1, 1);
			this.lblCons29.Name = "lblCons29";
			this.lblCons29.Size = new System.Drawing.Size(22, 28);
			this.lblCons29.TabIndex = 0;
			this.lblCons29.Text = "θ";
			this.lblCons29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons30
			// 
			this.lblCons30.BackColor = System.Drawing.Color.Transparent;
			this.lblCons30.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons30.Location = new System.Drawing.Point(23, 1);
			this.lblCons30.Name = "lblCons30";
			this.lblCons30.Size = new System.Drawing.Size(22, 28);
			this.lblCons30.TabIndex = 0;
			this.lblCons30.Text = "ð";
			this.lblCons30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel21
			// 
			this.panel21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel21.Controls.Add(this.lblCons21);
			this.panel21.Location = new System.Drawing.Point(192, 96);
			this.panel21.Name = "panel21";
			this.panel21.Size = new System.Drawing.Size(144, 32);
			this.panel21.TabIndex = 4;
			// 
			// lblCons21
			// 
			this.lblCons21.BackColor = System.Drawing.Color.Transparent;
			this.lblCons21.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons21.Location = new System.Drawing.Point(71, 1);
			this.lblCons21.Name = "lblCons21";
			this.lblCons21.Size = new System.Drawing.Size(22, 28);
			this.lblCons21.TabIndex = 0;
			this.lblCons21.Text = "r";
			this.lblCons21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel22
			// 
			this.panel22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel22.Controls.Add(this.lblCons50);
			this.panel22.Location = new System.Drawing.Point(192, 224);
			this.panel22.Name = "panel22";
			this.panel22.Size = new System.Drawing.Size(144, 32);
			this.panel22.TabIndex = 4;
			// 
			// lblCons50
			// 
			this.lblCons50.BackColor = System.Drawing.Color.Transparent;
			this.lblCons50.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons50.Location = new System.Drawing.Point(71, 1);
			this.lblCons50.Name = "lblCons50";
			this.lblCons50.Size = new System.Drawing.Size(22, 28);
			this.lblCons50.TabIndex = 0;
			this.lblCons50.Text = "ɹ";
			this.lblCons50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel23
			// 
			this.panel23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel23.Controls.Add(this.lblCons48);
			this.panel23.Controls.Add(this.lblCons47);
			this.panel23.Location = new System.Drawing.Point(192, 192);
			this.panel23.Name = "panel23";
			this.panel23.Size = new System.Drawing.Size(144, 32);
			this.panel23.TabIndex = 4;
			// 
			// lblCons48
			// 
			this.lblCons48.BackColor = System.Drawing.Color.Transparent;
			this.lblCons48.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons48.Location = new System.Drawing.Point(71, 1);
			this.lblCons48.Name = "lblCons48";
			this.lblCons48.Size = new System.Drawing.Size(22, 28);
			this.lblCons48.TabIndex = 0;
			this.lblCons48.Text = "ɮ";
			this.lblCons48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons47
			// 
			this.lblCons47.BackColor = System.Drawing.Color.Transparent;
			this.lblCons47.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons47.Location = new System.Drawing.Point(49, 1);
			this.lblCons47.Name = "lblCons47";
			this.lblCons47.Size = new System.Drawing.Size(22, 28);
			this.lblCons47.TabIndex = 0;
			this.lblCons47.Text = "ɬ";
			this.lblCons47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel24
			// 
			this.panel24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel24.Controls.Add(this.lblCons23);
			this.panel24.Location = new System.Drawing.Point(192, 128);
			this.panel24.Name = "panel24";
			this.panel24.Size = new System.Drawing.Size(144, 32);
			this.panel24.TabIndex = 4;
			// 
			// lblCons23
			// 
			this.lblCons23.BackColor = System.Drawing.Color.Transparent;
			this.lblCons23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons23.Location = new System.Drawing.Point(71, 1);
			this.lblCons23.Name = "lblCons23";
			this.lblCons23.Size = new System.Drawing.Size(22, 28);
			this.lblCons23.TabIndex = 0;
			this.lblCons23.Text = "ɾ";
			this.lblCons23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel25
			// 
			this.panel25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel25.Controls.Add(this.lblCons55);
			this.panel25.Location = new System.Drawing.Point(336, 256);
			this.panel25.Name = "panel25";
			this.panel25.Size = new System.Drawing.Size(56, 32);
			this.panel25.TabIndex = 4;
			// 
			// lblCons55
			// 
			this.lblCons55.BackColor = System.Drawing.Color.Transparent;
			this.lblCons55.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons55.Location = new System.Drawing.Point(31, 1);
			this.lblCons55.Name = "lblCons55";
			this.lblCons55.Size = new System.Drawing.Size(22, 28);
			this.lblCons55.TabIndex = 0;
			this.lblCons55.Text = "ɭ";
			this.lblCons55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel26
			// 
			this.panel26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel26.Controls.Add(this.lblCons4);
			this.panel26.Controls.Add(this.lblCons5);
			this.panel26.Location = new System.Drawing.Point(336, 32);
			this.panel26.Name = "panel26";
			this.panel26.Size = new System.Drawing.Size(56, 32);
			this.panel26.TabIndex = 4;
			// 
			// lblCons4
			// 
			this.lblCons4.BackColor = System.Drawing.Color.Transparent;
			this.lblCons4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons4.Location = new System.Drawing.Point(1, 1);
			this.lblCons4.Name = "lblCons4";
			this.lblCons4.Size = new System.Drawing.Size(22, 28);
			this.lblCons4.TabIndex = 0;
			this.lblCons4.Text = "ʈ";
			this.lblCons4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons5
			// 
			this.lblCons5.BackColor = System.Drawing.Color.Transparent;
			this.lblCons5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons5.Location = new System.Drawing.Point(31, 1);
			this.lblCons5.Name = "lblCons5";
			this.lblCons5.Size = new System.Drawing.Size(22, 28);
			this.lblCons5.TabIndex = 0;
			this.lblCons5.Text = "ɖ";
			this.lblCons5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel27
			// 
			this.panel27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel27.Controls.Add(this.lblCons16);
			this.panel27.Location = new System.Drawing.Point(336, 64);
			this.panel27.Name = "panel27";
			this.panel27.Size = new System.Drawing.Size(56, 32);
			this.panel27.TabIndex = 4;
			// 
			// lblCons16
			// 
			this.lblCons16.BackColor = System.Drawing.Color.Transparent;
			this.lblCons16.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons16.Location = new System.Drawing.Point(31, 1);
			this.lblCons16.Name = "lblCons16";
			this.lblCons16.Size = new System.Drawing.Size(22, 28);
			this.lblCons16.TabIndex = 0;
			this.lblCons16.Text = "ɳ";
			this.lblCons16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel28
			// 
			this.panel28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel28.Controls.Add(this.lblCons36);
			this.panel28.Controls.Add(this.lblCons35);
			this.panel28.Location = new System.Drawing.Point(336, 160);
			this.panel28.Name = "panel28";
			this.panel28.Size = new System.Drawing.Size(56, 32);
			this.panel28.TabIndex = 4;
			// 
			// lblCons36
			// 
			this.lblCons36.BackColor = System.Drawing.Color.Transparent;
			this.lblCons36.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons36.Location = new System.Drawing.Point(31, 1);
			this.lblCons36.Name = "lblCons36";
			this.lblCons36.Size = new System.Drawing.Size(22, 28);
			this.lblCons36.TabIndex = 0;
			this.lblCons36.Text = "ʐ";
			this.lblCons36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons35
			// 
			this.lblCons35.BackColor = System.Drawing.Color.Transparent;
			this.lblCons35.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons35.Location = new System.Drawing.Point(1, 1);
			this.lblCons35.Name = "lblCons35";
			this.lblCons35.Size = new System.Drawing.Size(22, 28);
			this.lblCons35.TabIndex = 0;
			this.lblCons35.Text = "ʂ";
			this.lblCons35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel29
			// 
			this.panel29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel29.Location = new System.Drawing.Point(336, 96);
			this.panel29.Name = "panel29";
			this.panel29.Size = new System.Drawing.Size(56, 32);
			this.panel29.TabIndex = 4;
			// 
			// panel30
			// 
			this.panel30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel30.Controls.Add(this.lblCons51);
			this.panel30.Location = new System.Drawing.Point(336, 224);
			this.panel30.Name = "panel30";
			this.panel30.Size = new System.Drawing.Size(56, 32);
			this.panel30.TabIndex = 4;
			// 
			// lblCons51
			// 
			this.lblCons51.BackColor = System.Drawing.Color.Transparent;
			this.lblCons51.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons51.Location = new System.Drawing.Point(31, 1);
			this.lblCons51.Name = "lblCons51";
			this.lblCons51.Size = new System.Drawing.Size(22, 28);
			this.lblCons51.TabIndex = 0;
			this.lblCons51.Text = "ɻ";
			this.lblCons51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel31
			// 
			this.panel31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel31.Location = new System.Drawing.Point(336, 192);
			this.panel31.Name = "panel31";
			this.panel31.Size = new System.Drawing.Size(56, 32);
			this.panel31.TabIndex = 4;
			// 
			// panel32
			// 
			this.panel32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel32.Controls.Add(this.lblCons24);
			this.panel32.Location = new System.Drawing.Point(336, 128);
			this.panel32.Name = "panel32";
			this.panel32.Size = new System.Drawing.Size(56, 32);
			this.panel32.TabIndex = 4;
			// 
			// lblCons24
			// 
			this.lblCons24.BackColor = System.Drawing.Color.Transparent;
			this.lblCons24.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons24.Location = new System.Drawing.Point(31, 1);
			this.lblCons24.Name = "lblCons24";
			this.lblCons24.Size = new System.Drawing.Size(22, 28);
			this.lblCons24.TabIndex = 0;
			this.lblCons24.Text = "ɽ";
			this.lblCons24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel33
			// 
			this.panel33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel33.Location = new System.Drawing.Point(392, 128);
			this.panel33.Name = "panel33";
			this.panel33.Size = new System.Drawing.Size(48, 32);
			this.panel33.TabIndex = 4;
			// 
			// panel34
			// 
			this.panel34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel34.Controls.Add(this.lblCons56);
			this.panel34.Location = new System.Drawing.Point(392, 256);
			this.panel34.Name = "panel34";
			this.panel34.Size = new System.Drawing.Size(48, 32);
			this.panel34.TabIndex = 4;
			// 
			// lblCons56
			// 
			this.lblCons56.BackColor = System.Drawing.Color.Transparent;
			this.lblCons56.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons56.Location = new System.Drawing.Point(23, 1);
			this.lblCons56.Name = "lblCons56";
			this.lblCons56.Size = new System.Drawing.Size(22, 28);
			this.lblCons56.TabIndex = 0;
			this.lblCons56.Text = "ʎ";
			this.lblCons56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel35
			// 
			this.panel35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel35.Controls.Add(this.lblCons6);
			this.panel35.Controls.Add(this.lblCons7);
			this.panel35.Location = new System.Drawing.Point(392, 32);
			this.panel35.Name = "panel35";
			this.panel35.Size = new System.Drawing.Size(48, 32);
			this.panel35.TabIndex = 4;
			// 
			// lblCons6
			// 
			this.lblCons6.BackColor = System.Drawing.Color.Transparent;
			this.lblCons6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons6.Location = new System.Drawing.Point(1, 1);
			this.lblCons6.Name = "lblCons6";
			this.lblCons6.Size = new System.Drawing.Size(22, 28);
			this.lblCons6.TabIndex = 0;
			this.lblCons6.Text = "c";
			this.lblCons6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons7
			// 
			this.lblCons7.BackColor = System.Drawing.Color.Transparent;
			this.lblCons7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons7.Location = new System.Drawing.Point(23, 1);
			this.lblCons7.Name = "lblCons7";
			this.lblCons7.Size = new System.Drawing.Size(22, 28);
			this.lblCons7.TabIndex = 0;
			this.lblCons7.Text = "ɟ";
			this.lblCons7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel36
			// 
			this.panel36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel36.Controls.Add(this.lblCons17);
			this.panel36.Location = new System.Drawing.Point(392, 64);
			this.panel36.Name = "panel36";
			this.panel36.Size = new System.Drawing.Size(48, 32);
			this.panel36.TabIndex = 4;
			// 
			// lblCons17
			// 
			this.lblCons17.BackColor = System.Drawing.Color.Transparent;
			this.lblCons17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons17.Location = new System.Drawing.Point(23, 1);
			this.lblCons17.Name = "lblCons17";
			this.lblCons17.Size = new System.Drawing.Size(22, 28);
			this.lblCons17.TabIndex = 0;
			this.lblCons17.Text = "ɲ";
			this.lblCons17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel37
			// 
			this.panel37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel37.Controls.Add(this.lblCons37);
			this.panel37.Controls.Add(this.lblCons38);
			this.panel37.Location = new System.Drawing.Point(392, 160);
			this.panel37.Name = "panel37";
			this.panel37.Size = new System.Drawing.Size(48, 32);
			this.panel37.TabIndex = 4;
			// 
			// lblCons37
			// 
			this.lblCons37.BackColor = System.Drawing.Color.Transparent;
			this.lblCons37.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons37.Location = new System.Drawing.Point(1, 1);
			this.lblCons37.Name = "lblCons37";
			this.lblCons37.Size = new System.Drawing.Size(22, 28);
			this.lblCons37.TabIndex = 0;
			this.lblCons37.Text = "ç";
			this.lblCons37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons38
			// 
			this.lblCons38.BackColor = System.Drawing.Color.Transparent;
			this.lblCons38.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons38.Location = new System.Drawing.Point(23, 1);
			this.lblCons38.Name = "lblCons38";
			this.lblCons38.Size = new System.Drawing.Size(22, 28);
			this.lblCons38.TabIndex = 0;
			this.lblCons38.Text = "ʝ";
			this.lblCons38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel38
			// 
			this.panel38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel38.Location = new System.Drawing.Point(392, 96);
			this.panel38.Name = "panel38";
			this.panel38.Size = new System.Drawing.Size(48, 32);
			this.panel38.TabIndex = 4;
			// 
			// panel39
			// 
			this.panel39.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel39.Controls.Add(this.lblCons52);
			this.panel39.Location = new System.Drawing.Point(392, 224);
			this.panel39.Name = "panel39";
			this.panel39.Size = new System.Drawing.Size(48, 32);
			this.panel39.TabIndex = 4;
			// 
			// lblCons52
			// 
			this.lblCons52.BackColor = System.Drawing.Color.Transparent;
			this.lblCons52.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons52.Location = new System.Drawing.Point(23, 1);
			this.lblCons52.Name = "lblCons52";
			this.lblCons52.Size = new System.Drawing.Size(22, 28);
			this.lblCons52.TabIndex = 0;
			this.lblCons52.Text = "j";
			this.lblCons52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel40
			// 
			this.panel40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel40.Location = new System.Drawing.Point(392, 192);
			this.panel40.Name = "panel40";
			this.panel40.Size = new System.Drawing.Size(48, 32);
			this.panel40.TabIndex = 4;
			// 
			// panel41
			// 
			this.panel41.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel41.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel41.Location = new System.Drawing.Point(440, 128);
			this.panel41.Name = "panel41";
			this.panel41.Size = new System.Drawing.Size(48, 32);
			this.panel41.TabIndex = 4;
			// 
			// panel42
			// 
			this.panel42.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel42.Controls.Add(this.lblCons57);
			this.panel42.Location = new System.Drawing.Point(440, 256);
			this.panel42.Name = "panel42";
			this.panel42.Size = new System.Drawing.Size(48, 32);
			this.panel42.TabIndex = 4;
			// 
			// lblCons57
			// 
			this.lblCons57.BackColor = System.Drawing.Color.Transparent;
			this.lblCons57.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons57.Location = new System.Drawing.Point(23, 1);
			this.lblCons57.Name = "lblCons57";
			this.lblCons57.Size = new System.Drawing.Size(22, 28);
			this.lblCons57.TabIndex = 0;
			this.lblCons57.Text = "ʟ";
			this.lblCons57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel43
			// 
			this.panel43.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel43.Controls.Add(this.lblCons8);
			this.panel43.Controls.Add(this.lblCons9);
			this.panel43.Location = new System.Drawing.Point(440, 32);
			this.panel43.Name = "panel43";
			this.panel43.Size = new System.Drawing.Size(48, 32);
			this.panel43.TabIndex = 4;
			// 
			// lblCons8
			// 
			this.lblCons8.BackColor = System.Drawing.Color.Transparent;
			this.lblCons8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons8.Location = new System.Drawing.Point(1, 1);
			this.lblCons8.Name = "lblCons8";
			this.lblCons8.Size = new System.Drawing.Size(22, 28);
			this.lblCons8.TabIndex = 0;
			this.lblCons8.Text = "k";
			this.lblCons8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons9
			// 
			this.lblCons9.BackColor = System.Drawing.Color.Transparent;
			this.lblCons9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons9.Location = new System.Drawing.Point(23, 1);
			this.lblCons9.Name = "lblCons9";
			this.lblCons9.Size = new System.Drawing.Size(22, 28);
			this.lblCons9.TabIndex = 0;
			this.lblCons9.Text = "g";
			this.lblCons9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel44
			// 
			this.panel44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel44.Controls.Add(this.lblCons18);
			this.panel44.Location = new System.Drawing.Point(440, 64);
			this.panel44.Name = "panel44";
			this.panel44.Size = new System.Drawing.Size(48, 32);
			this.panel44.TabIndex = 4;
			// 
			// lblCons18
			// 
			this.lblCons18.BackColor = System.Drawing.Color.Transparent;
			this.lblCons18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons18.Location = new System.Drawing.Point(23, 1);
			this.lblCons18.Name = "lblCons18";
			this.lblCons18.Size = new System.Drawing.Size(22, 28);
			this.lblCons18.TabIndex = 0;
			this.lblCons18.Text = "ŋ";
			this.lblCons18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel45
			// 
			this.panel45.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel45.Controls.Add(this.lblCons40);
			this.panel45.Controls.Add(this.lblCons39);
			this.panel45.Location = new System.Drawing.Point(440, 160);
			this.panel45.Name = "panel45";
			this.panel45.Size = new System.Drawing.Size(48, 32);
			this.panel45.TabIndex = 4;
			// 
			// lblCons40
			// 
			this.lblCons40.BackColor = System.Drawing.Color.Transparent;
			this.lblCons40.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons40.Location = new System.Drawing.Point(23, 1);
			this.lblCons40.Name = "lblCons40";
			this.lblCons40.Size = new System.Drawing.Size(22, 28);
			this.lblCons40.TabIndex = 0;
			this.lblCons40.Text = "ɣ";
			this.lblCons40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons39
			// 
			this.lblCons39.BackColor = System.Drawing.Color.Transparent;
			this.lblCons39.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons39.Location = new System.Drawing.Point(1, 1);
			this.lblCons39.Name = "lblCons39";
			this.lblCons39.Size = new System.Drawing.Size(22, 28);
			this.lblCons39.TabIndex = 0;
			this.lblCons39.Text = "x";
			this.lblCons39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel46
			// 
			this.panel46.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel46.Location = new System.Drawing.Point(440, 96);
			this.panel46.Name = "panel46";
			this.panel46.Size = new System.Drawing.Size(48, 32);
			this.panel46.TabIndex = 4;
			// 
			// panel47
			// 
			this.panel47.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel47.Controls.Add(this.lblCons53);
			this.panel47.Location = new System.Drawing.Point(440, 224);
			this.panel47.Name = "panel47";
			this.panel47.Size = new System.Drawing.Size(48, 32);
			this.panel47.TabIndex = 4;
			// 
			// lblCons53
			// 
			this.lblCons53.BackColor = System.Drawing.Color.Transparent;
			this.lblCons53.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons53.Location = new System.Drawing.Point(23, 1);
			this.lblCons53.Name = "lblCons53";
			this.lblCons53.Size = new System.Drawing.Size(22, 28);
			this.lblCons53.TabIndex = 0;
			this.lblCons53.Text = "ɰ";
			this.lblCons53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel48
			// 
			this.panel48.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel48.Location = new System.Drawing.Point(440, 192);
			this.panel48.Name = "panel48";
			this.panel48.Size = new System.Drawing.Size(48, 32);
			this.panel48.TabIndex = 4;
			// 
			// panel49
			// 
			this.panel49.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel49.Controls.Add(this.lblCons10);
			this.panel49.Controls.Add(this.lblCons11);
			this.panel49.Location = new System.Drawing.Point(488, 32);
			this.panel49.Name = "panel49";
			this.panel49.Size = new System.Drawing.Size(48, 32);
			this.panel49.TabIndex = 4;
			// 
			// lblCons10
			// 
			this.lblCons10.BackColor = System.Drawing.Color.Transparent;
			this.lblCons10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons10.Location = new System.Drawing.Point(1, 1);
			this.lblCons10.Name = "lblCons10";
			this.lblCons10.Size = new System.Drawing.Size(22, 28);
			this.lblCons10.TabIndex = 0;
			this.lblCons10.Text = "q";
			this.lblCons10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons11
			// 
			this.lblCons11.BackColor = System.Drawing.Color.Transparent;
			this.lblCons11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons11.Location = new System.Drawing.Point(23, 1);
			this.lblCons11.Name = "lblCons11";
			this.lblCons11.Size = new System.Drawing.Size(22, 28);
			this.lblCons11.TabIndex = 0;
			this.lblCons11.Text = "ɢ";
			this.lblCons11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel50
			// 
			this.panel50.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel50.Location = new System.Drawing.Point(488, 256);
			this.panel50.Name = "panel50";
			this.panel50.Size = new System.Drawing.Size(48, 32);
			this.panel50.TabIndex = 4;
			// 
			// panel51
			// 
			this.panel51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel51.Location = new System.Drawing.Point(488, 128);
			this.panel51.Name = "panel51";
			this.panel51.Size = new System.Drawing.Size(48, 32);
			this.panel51.TabIndex = 4;
			// 
			// panel52
			// 
			this.panel52.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel52.Controls.Add(this.lblCons19);
			this.panel52.Location = new System.Drawing.Point(488, 64);
			this.panel52.Name = "panel52";
			this.panel52.Size = new System.Drawing.Size(48, 32);
			this.panel52.TabIndex = 4;
			// 
			// lblCons19
			// 
			this.lblCons19.BackColor = System.Drawing.Color.Transparent;
			this.lblCons19.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons19.Location = new System.Drawing.Point(23, 1);
			this.lblCons19.Name = "lblCons19";
			this.lblCons19.Size = new System.Drawing.Size(22, 28);
			this.lblCons19.TabIndex = 0;
			this.lblCons19.Text = "ɴ";
			this.lblCons19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel53
			// 
			this.panel53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel53.Controls.Add(this.lblCons42);
			this.panel53.Controls.Add(this.lblCons41);
			this.panel53.Location = new System.Drawing.Point(488, 160);
			this.panel53.Name = "panel53";
			this.panel53.Size = new System.Drawing.Size(48, 32);
			this.panel53.TabIndex = 4;
			// 
			// lblCons42
			// 
			this.lblCons42.BackColor = System.Drawing.Color.Transparent;
			this.lblCons42.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons42.Location = new System.Drawing.Point(23, 1);
			this.lblCons42.Name = "lblCons42";
			this.lblCons42.Size = new System.Drawing.Size(22, 28);
			this.lblCons42.TabIndex = 0;
			this.lblCons42.Text = "ʁ";
			this.lblCons42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons41
			// 
			this.lblCons41.BackColor = System.Drawing.Color.Transparent;
			this.lblCons41.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons41.Location = new System.Drawing.Point(1, 1);
			this.lblCons41.Name = "lblCons41";
			this.lblCons41.Size = new System.Drawing.Size(22, 28);
			this.lblCons41.TabIndex = 0;
			this.lblCons41.Text = "χ";
			this.lblCons41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel54
			// 
			this.panel54.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel54.Controls.Add(this.lblCons22);
			this.panel54.Location = new System.Drawing.Point(488, 96);
			this.panel54.Name = "panel54";
			this.panel54.Size = new System.Drawing.Size(48, 32);
			this.panel54.TabIndex = 4;
			// 
			// lblCons22
			// 
			this.lblCons22.BackColor = System.Drawing.Color.Transparent;
			this.lblCons22.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons22.Location = new System.Drawing.Point(23, 1);
			this.lblCons22.Name = "lblCons22";
			this.lblCons22.Size = new System.Drawing.Size(22, 28);
			this.lblCons22.TabIndex = 0;
			this.lblCons22.Text = "ʀ";
			this.lblCons22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel55
			// 
			this.panel55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel55.Location = new System.Drawing.Point(488, 224);
			this.panel55.Name = "panel55";
			this.panel55.Size = new System.Drawing.Size(48, 32);
			this.panel55.TabIndex = 4;
			// 
			// panel56
			// 
			this.panel56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel56.Location = new System.Drawing.Point(488, 192);
			this.panel56.Name = "panel56";
			this.panel56.Size = new System.Drawing.Size(48, 32);
			this.panel56.TabIndex = 4;
			// 
			// panel57
			// 
			this.panel57.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel57.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel57.Location = new System.Drawing.Point(536, 192);
			this.panel57.Name = "panel57";
			this.panel57.Size = new System.Drawing.Size(56, 32);
			this.panel57.TabIndex = 4;
			// 
			// panel58
			// 
			this.panel58.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel58.Location = new System.Drawing.Point(536, 224);
			this.panel58.Name = "panel58";
			this.panel58.Size = new System.Drawing.Size(56, 32);
			this.panel58.TabIndex = 4;
			// 
			// panel59
			// 
			this.panel59.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel59.Location = new System.Drawing.Point(536, 96);
			this.panel59.Name = "panel59";
			this.panel59.Size = new System.Drawing.Size(56, 32);
			this.panel59.TabIndex = 4;
			// 
			// panel60
			// 
			this.panel60.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel60.Controls.Add(this.lblCons44);
			this.panel60.Controls.Add(this.lblCons43);
			this.panel60.Location = new System.Drawing.Point(536, 160);
			this.panel60.Name = "panel60";
			this.panel60.Size = new System.Drawing.Size(56, 32);
			this.panel60.TabIndex = 4;
			// 
			// lblCons44
			// 
			this.lblCons44.BackColor = System.Drawing.Color.Transparent;
			this.lblCons44.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons44.Location = new System.Drawing.Point(31, 1);
			this.lblCons44.Name = "lblCons44";
			this.lblCons44.Size = new System.Drawing.Size(22, 28);
			this.lblCons44.TabIndex = 0;
			this.lblCons44.Text = "ʕ";
			this.lblCons44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons43
			// 
			this.lblCons43.BackColor = System.Drawing.Color.Transparent;
			this.lblCons43.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons43.Location = new System.Drawing.Point(1, 1);
			this.lblCons43.Name = "lblCons43";
			this.lblCons43.Size = new System.Drawing.Size(22, 28);
			this.lblCons43.TabIndex = 0;
			this.lblCons43.Text = "ħ";
			this.lblCons43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel61
			// 
			this.panel61.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel61.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel61.Location = new System.Drawing.Point(536, 64);
			this.panel61.Name = "panel61";
			this.panel61.Size = new System.Drawing.Size(56, 32);
			this.panel61.TabIndex = 4;
			// 
			// panel62
			// 
			this.panel62.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel62.Location = new System.Drawing.Point(536, 128);
			this.panel62.Name = "panel62";
			this.panel62.Size = new System.Drawing.Size(56, 32);
			this.panel62.TabIndex = 4;
			// 
			// panel63
			// 
			this.panel63.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel63.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel63.Location = new System.Drawing.Point(536, 256);
			this.panel63.Name = "panel63";
			this.panel63.Size = new System.Drawing.Size(56, 32);
			this.panel63.TabIndex = 4;
			// 
			// panel64
			// 
			this.panel64.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel64.Controls.Add(this.panel75);
			this.panel64.Location = new System.Drawing.Point(536, 32);
			this.panel64.Name = "panel64";
			this.panel64.Size = new System.Drawing.Size(56, 32);
			this.panel64.TabIndex = 4;
			// 
			// panel75
			// 
			this.panel75.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel75.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel75.Location = new System.Drawing.Point(27, -1);
			this.panel75.Name = "panel75";
			this.panel75.Size = new System.Drawing.Size(32, 32);
			this.panel75.TabIndex = 0;
			// 
			// panel65
			// 
			this.panel65.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel65.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel65.Location = new System.Drawing.Point(592, 192);
			this.panel65.Name = "panel65";
			this.panel65.Size = new System.Drawing.Size(48, 32);
			this.panel65.TabIndex = 4;
			// 
			// panel66
			// 
			this.panel66.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel66.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel66.Location = new System.Drawing.Point(592, 224);
			this.panel66.Name = "panel66";
			this.panel66.Size = new System.Drawing.Size(48, 32);
			this.panel66.TabIndex = 4;
			// 
			// panel67
			// 
			this.panel67.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel67.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel67.Location = new System.Drawing.Point(592, 96);
			this.panel67.Name = "panel67";
			this.panel67.Size = new System.Drawing.Size(48, 32);
			this.panel67.TabIndex = 4;
			// 
			// panel68
			// 
			this.panel68.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel68.Controls.Add(this.lblCons46);
			this.panel68.Controls.Add(this.lblCons45);
			this.panel68.Location = new System.Drawing.Point(592, 160);
			this.panel68.Name = "panel68";
			this.panel68.Size = new System.Drawing.Size(48, 32);
			this.panel68.TabIndex = 4;
			// 
			// lblCons46
			// 
			this.lblCons46.BackColor = System.Drawing.Color.Transparent;
			this.lblCons46.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons46.Location = new System.Drawing.Point(23, 1);
			this.lblCons46.Name = "lblCons46";
			this.lblCons46.Size = new System.Drawing.Size(22, 28);
			this.lblCons46.TabIndex = 0;
			this.lblCons46.Text = "ɦ";
			this.lblCons46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons45
			// 
			this.lblCons45.BackColor = System.Drawing.Color.Transparent;
			this.lblCons45.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons45.Location = new System.Drawing.Point(1, 1);
			this.lblCons45.Name = "lblCons45";
			this.lblCons45.Size = new System.Drawing.Size(22, 28);
			this.lblCons45.TabIndex = 0;
			this.lblCons45.Text = "h";
			this.lblCons45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel69
			// 
			this.panel69.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel69.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel69.Location = new System.Drawing.Point(592, 64);
			this.panel69.Name = "panel69";
			this.panel69.Size = new System.Drawing.Size(48, 32);
			this.panel69.TabIndex = 4;
			// 
			// panel70
			// 
			this.panel70.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel70.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel70.Location = new System.Drawing.Point(592, 128);
			this.panel70.Name = "panel70";
			this.panel70.Size = new System.Drawing.Size(48, 32);
			this.panel70.TabIndex = 4;
			// 
			// panel71
			// 
			this.panel71.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel71.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel71.Location = new System.Drawing.Point(592, 256);
			this.panel71.Name = "panel71";
			this.panel71.Size = new System.Drawing.Size(48, 32);
			this.panel71.TabIndex = 4;
			// 
			// panel72
			// 
			this.panel72.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel72.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel72.Location = new System.Drawing.Point(617, 32);
			this.panel72.Name = "panel72";
			this.panel72.Size = new System.Drawing.Size(23, 32);
			this.panel72.TabIndex = 4;
			// 
			// panel73
			// 
			this.panel73.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel73.Controls.Add(this.lblCons32);
			this.panel73.Controls.Add(this.lblCons31);
			this.panel73.Location = new System.Drawing.Point(240, 160);
			this.panel73.Name = "panel73";
			this.panel73.Size = new System.Drawing.Size(48, 32);
			this.panel73.TabIndex = 4;
			// 
			// lblCons32
			// 
			this.lblCons32.BackColor = System.Drawing.Color.Transparent;
			this.lblCons32.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons32.Location = new System.Drawing.Point(23, 1);
			this.lblCons32.Name = "lblCons32";
			this.lblCons32.Size = new System.Drawing.Size(22, 28);
			this.lblCons32.TabIndex = 0;
			this.lblCons32.Text = "z";
			this.lblCons32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons31
			// 
			this.lblCons31.BackColor = System.Drawing.Color.Transparent;
			this.lblCons31.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons31.Location = new System.Drawing.Point(1, 1);
			this.lblCons31.Name = "lblCons31";
			this.lblCons31.Size = new System.Drawing.Size(22, 28);
			this.lblCons31.TabIndex = 0;
			this.lblCons31.Text = "s";
			this.lblCons31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel74
			// 
			this.panel74.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel74.Controls.Add(this.lblCons34);
			this.panel74.Controls.Add(this.lblCons33);
			this.panel74.Location = new System.Drawing.Point(288, 160);
			this.panel74.Name = "panel74";
			this.panel74.Size = new System.Drawing.Size(48, 32);
			this.panel74.TabIndex = 4;
			// 
			// lblCons34
			// 
			this.lblCons34.BackColor = System.Drawing.Color.Transparent;
			this.lblCons34.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons34.Location = new System.Drawing.Point(23, 1);
			this.lblCons34.Name = "lblCons34";
			this.lblCons34.Size = new System.Drawing.Size(22, 28);
			this.lblCons34.TabIndex = 0;
			this.lblCons34.Text = "ʒ";
			this.lblCons34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCons33
			// 
			this.lblCons33.BackColor = System.Drawing.Color.Transparent;
			this.lblCons33.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons33.Location = new System.Drawing.Point(1, 1);
			this.lblCons33.Name = "lblCons33";
			this.lblCons33.Size = new System.Drawing.Size(22, 28);
			this.lblCons33.TabIndex = 0;
			this.lblCons33.Text = "ʃ";
			this.lblCons33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel76
			// 
			this.panel76.BackColor = System.Drawing.SystemColors.Control;
			this.panel76.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel76.Controls.Add(this.lblCons12);
			this.panel76.Location = new System.Drawing.Point(592, 32);
			this.panel76.Name = "panel76";
			this.panel76.Size = new System.Drawing.Size(48, 32);
			this.panel76.TabIndex = 5;
			// 
			// lblCons12
			// 
			this.lblCons12.BackColor = System.Drawing.SystemColors.Control;
			this.lblCons12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCons12.Location = new System.Drawing.Point(1, 1);
			this.lblCons12.Name = "lblCons12";
			this.lblCons12.Size = new System.Drawing.Size(22, 28);
			this.lblCons12.TabIndex = 0;
			this.lblCons12.Text = "ʔ";
			this.lblCons12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblRedMsg
			// 
			this.lblRedMsg.ForeColor = System.Drawing.Color.Red;
			this.lblRedMsg.Location = new System.Drawing.Point(312, 4);
			this.lblRedMsg.Name = "lblRedMsg";
			this.lblRedMsg.Size = new System.Drawing.Size(224, 16);
			this.lblRedMsg.TabIndex = 1;
			this.lblRedMsg.Text = "Red consonants do not occur as base only.";
			this.lblRedMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblRedMsg.Visible = false;
			// 
			// DispCon
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 509);
			this.Controls.Add(this.lblRedMsg);
			this.Controls.Add(this.tabControl);
			this.Name = "DispCon";
			this.Text = "Consonants";
			this.tabControl.ResumeLayout(false);
			this.tabGenSum.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.grpOtherSymbols.ResumeLayout(false);
			this.grpNonPulmonic.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel11.ResumeLayout(false);
			this.panel12.ResumeLayout(false);
			this.panel14.ResumeLayout(false);
			this.panel17.ResumeLayout(false);
			this.panel18.ResumeLayout(false);
			this.panel19.ResumeLayout(false);
			this.panel20.ResumeLayout(false);
			this.panel21.ResumeLayout(false);
			this.panel22.ResumeLayout(false);
			this.panel23.ResumeLayout(false);
			this.panel24.ResumeLayout(false);
			this.panel25.ResumeLayout(false);
			this.panel26.ResumeLayout(false);
			this.panel27.ResumeLayout(false);
			this.panel28.ResumeLayout(false);
			this.panel30.ResumeLayout(false);
			this.panel32.ResumeLayout(false);
			this.panel34.ResumeLayout(false);
			this.panel35.ResumeLayout(false);
			this.panel36.ResumeLayout(false);
			this.panel37.ResumeLayout(false);
			this.panel39.ResumeLayout(false);
			this.panel42.ResumeLayout(false);
			this.panel43.ResumeLayout(false);
			this.panel44.ResumeLayout(false);
			this.panel45.ResumeLayout(false);
			this.panel47.ResumeLayout(false);
			this.panel49.ResumeLayout(false);
			this.panel52.ResumeLayout(false);
			this.panel53.ResumeLayout(false);
			this.panel54.ResumeLayout(false);
			this.panel60.ResumeLayout(false);
			this.panel64.ResumeLayout(false);
			this.panel68.ResumeLayout(false);
			this.panel73.ResumeLayout(false);
			this.panel74.ResumeLayout(false);
			this.panel76.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			Refresh();
			if (tabControl.SelectedTab == tabAllCons)
				lblRedMsg.Visible = false;
			else
			{
				lblRedMsg.Visible = true;

				if (m_lblCons[m_currentindex].BackColor == Color.Transparent)
				{
					// Advance to the first enabled consonant; do not pass go, do not collect $200  --ALB
					for (; m_currentindex < m_lblCons.Length && !m_lblCons[m_currentindex].Enabled; 
						m_currentindex++);
				}
				
				// Some consonant is enabled, and it's lable index is m_currentindex
				if (m_currentindex != m_lblCons.Length) 
				{
					m_lblCons[m_currentindex].BackColor = SystemColors.Highlight;
					m_lblCons[m_currentindex].ForeColor = SystemColors.HighlightText;
				} 

					// No consonants are enabled
				else    
				{	
					// Return the index to zero and don't highlight anything
					m_currentindex = 0;
				}
			}
		}

		
		#region Methods of General Summary tab
		private void lblCons_Click(object sender, EventArgs e)
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
			m_lblCons[m_currentindex].BackColor = SystemColors.Control;
			m_lblCons[m_currentindex].ForeColor = (System.Drawing.Color)m_lblCons[m_currentindex].Tag;
			lbl.BackColor = SystemColors.Highlight;
			lbl.ForeColor = SystemColors.HighlightText;
			lbl.BringToFront();
			m_currentindex = i;

		}

		private void lblCons_DoubleClick(object sender, EventArgs e)
		{
			Label lbl = sender as Label;
			if (lbl == null || !lbl.Enabled) 
				return;

			string searchTarget = lbl.Text;

			// For the ejectives and affricates, only examples are clickable - but these
			// must search for any ejective/affricate, not the example chosen
			switch (m_currentindex)
			{
				case 68: searchTarget = lblCons68.Text; break;
				case 69: searchTarget = lblCons68.Text; break;
				case 70: searchTarget = lblCons68.Text; break;
				case 71: searchTarget = lblCons68.Text; break;
				case 82: searchTarget = lblConsLongBar.Text; break;
				case 83: searchTarget = lblConsShortBar.Text; break;
			}

			MessageBox.Show("Should be searching for " + searchTarget);
		}

		private void tabControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Modifiers != 0 || tabControl.SelectedTab != tabGenSum) 
				return;

			// Open up details about the consonant
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				e.Handled = true;
				lblCons_DoubleClick(m_lblCons[m_currentindex], e);
				return;
			}
			if (e.KeyCode == Keys.Space)
			{	
				e.Handled = true;
				lblCons_Click(m_lblCons[m_currentindex], e);
				return;
			}

			int i = m_currentindex;

			// Navigate to the right, wrapping down at the end of the line of consonants
			if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
			{
				e.Handled = true;

				// Wrap to the beginning when at the end of all consonants
				i = (m_currentindex == (m_lblCons.Length - 1) ? 0 : m_currentindex + 1);

				// Skip over disabled consonants
				while (!m_lblCons[i].Enabled && (i != m_currentindex))
					i = (i == (m_lblCons.Length - 1) ? 0 : (i + 1));

				if (!m_lblCons[i].Enabled)
					return;
	
				m_lblCons[m_currentindex].BackColor = SystemColors.Control;
				m_lblCons[m_currentindex].ForeColor = (System.Drawing.Color)m_lblCons[m_currentindex].Tag;
				m_lblCons[i].BackColor = SystemColors.Highlight;
				m_lblCons[i].ForeColor = SystemColors.HighlightText;
				m_lblCons[i].BringToFront();

				m_currentindex = i;
				return;
			}

			// Navigate to the left, wrapping up at the end of the line of consonants
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
			{
				e.Handled = true;

				// Wrap to the end when at the beginning of all consonants
				i = ((m_currentindex == 0) ? (m_lblCons.Length - 1) : m_currentindex - 1);

				// Skip over disabled consonants
				while (!m_lblCons[i].Enabled && i != m_currentindex)
					i = ((i == 0) ? (m_lblCons.Length - 1) : (i - 1));

				if (!m_lblCons[i].Enabled)
					return;

				m_lblCons[m_currentindex].BackColor = SystemColors.Control;
				m_lblCons[m_currentindex].ForeColor = (System.Drawing.Color)m_lblCons[m_currentindex].Tag;
				m_lblCons[i].BackColor = SystemColors.Highlight;
				m_lblCons[i].ForeColor = SystemColors.HighlightText;
				m_lblCons[i].BringToFront();

				m_currentindex = i;
				return;
			}

		}

		#endregion

		#region Methods of All Consonants tab
		#endregion

		#region Static Methods
		/// <summary>
		/// Parse the label name to return its number as an index.
		/// </summary>
		/// <param name="ctrl">A label with name of the form "lblCons###"</param>
		/// <returns>Index number ### for the label</returns>
		private int getLblIndex(Label lbl)
		{
			return int.Parse(lbl.Name.Substring(7));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Disable/enable consonants according to the current database.
		/// </summary>
		/// <returns>True indicates success</returns>
		/// ------------------------------------------------------------------------------------
		private bool ReadConsFromDB() 
		{
			//string sql = "SELECT DISTINCTROW CharList.CharStr, IPACharacters.DsplyOrder " +
			//    "FROM (PhoneticList INNER JOIN (Folder INNER JOIN ((Document INNER JOIN " +
			//    "AllWordsIndex ON Document.DocID = AllWordsIndex.DocID) INNER JOIN " +
			//    "DocumentLinks ON Document.DocID = DocumentLinks.DocID) ON Folder.FolderID = " +
			//    "DocumentLinks.FolderID) ON PhoneticList.PhoneticListID = " +
			//    "AllWordsIndex.PhoneticListID) INNER JOIN (IPACharacters INNER JOIN " +
			//    "(CharList INNER JOIN CharIndex ON CharList.CharListID = CharIndex.CharListID) " +
			//    "ON IPACharacters.IPACharacterID = CharList.IPACharacterID) ON " +
			//    "PhoneticList.PhoneticListID = CharIndex.PhoneticListID WHERE " +
			//    "(((IPACharacters.CharType)=1 Or (IPACharacters.CharType)=3 Or " +
			//    "(IPACharacters.CharType)=4) AND ((CharList.TotalCount)>0));";

			//PaDataTable tblCon;
			
			//try 
			//{
			//    tblCon = new PaDataTable(sql);
			//}
			//catch (Exception e)
			//{
			//    MessageBox.Show("Error reading from database: \n" + e);
			//    return false;
			//}

			//Font fntCon = new Font(FontHelper.PhoneticFont.Name, 14, FontStyle.Bold);
			
			//// Go through all consonants returned by the query (as present in the database)
			//foreach (DataRow row in tblCon.Rows) 
			//{
			//    int lblIndex = (System.Int16)row[DBFields.DsplyOrder];
			//    if (lblIndex < 0)
			//        continue;

			//    // Database is stored as 1-85, rather than 0-84, so need to shift indexing
			//    m_lblCons[lblIndex-1].Enabled = true;			            
			//    m_lblCons[lblIndex-1].Font = fntCon;			
			//    m_lblCons[lblIndex-1].ForeColor = Color.Red;	
			//    m_lblCons[lblIndex-1].Tag = Color.Red;

			//    // Initially font was set to red, change to black only if the
			//    // consonant exists as not base only
			//    // (i.e., CharStr (in CharList table) has more than one phone)
			//    string chr = row[DBFields.CharStr] as string;
			//    if (chr != null && chr.Length == 1) 
			//    {
			//        m_lblCons[lblIndex-1].ForeColor = SystemColors.ControlText;
			//        m_lblCons[lblIndex-1].Tag = SystemColors.ControlText;
			//    }
			//}

			return true;
		}
		#endregion

		#region IxCoreColleague Members

		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
			// TODO:  Add DispCon.Init implementation
		}

		public IxCoreColleague[] GetMessageTargets()
		{
			
			// TODO:  Add DispCon.GetMessageTargets implementation
			return (new IxCoreColleague[] {this});
		}

		#endregion

	}
}
