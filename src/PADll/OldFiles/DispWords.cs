using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using XCore;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for DispWords.
	/// </summary>
	public class DispWords : System.Windows.Forms.Form, IxCoreColleague
	{
		#region Variables added by Designer
		private System.Windows.Forms.Panel pnlGrid;
		private System.Windows.Forms.Panel pnlMisc;
		private System.Windows.Forms.Label lblCond;
		private System.Windows.Forms.ComboBox cboCond;
		private System.Windows.Forms.Button btnCond;
		private System.Windows.Forms.Label lblSpeed;
		private System.Windows.Forms.NumericUpDown spinSpeed;
		private System.Windows.Forms.Label lblPercent;
		private System.Windows.Forms.Label lblDelay;
		private System.Windows.Forms.NumericUpDown spinDelay;
		private System.Windows.Forms.Label lblSec;
		private System.Windows.Forms.Label lblCharCaption;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		private FwGrid m_wordgrid;
		private int m_lastSelectedColIndex;

		public DispWords()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			m_lastSelectedColIndex = -1;
			SetupGrid();
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

		protected override void OnPaint(PaintEventArgs e)
		{
			// etic,eticffl,eticffm,eticffr,emic,tone,ortho,gloss,pos,ref
			m_wordgrid.Columns["etic"].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns["eticffl"].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns["eticffm"].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns["eticffr"].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns["emic"].ColumnFont = PaApp.PhonemicFont;
			m_wordgrid.Columns["tone"].ColumnFont = PaApp.ToneFont;
			m_wordgrid.Columns["ortho"].ColumnFont = PaApp.OrthograpicFont;
			m_wordgrid.Columns["gloss"].ColumnFont = PaApp.GlossFont;
			m_wordgrid.Columns["pos"].ColumnFont = PaApp.PartOfSpeechFont;
			m_wordgrid.Columns["ref"].ColumnFont = PaApp.ReferenceFont;
			base.OnPaint (e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlGrid = new System.Windows.Forms.Panel();
			this.pnlMisc = new System.Windows.Forms.Panel();
			this.lblSec = new System.Windows.Forms.Label();
			this.lblDelay = new System.Windows.Forms.Label();
			this.lblPercent = new System.Windows.Forms.Label();
			this.spinSpeed = new System.Windows.Forms.NumericUpDown();
			this.lblSpeed = new System.Windows.Forms.Label();
			this.btnCond = new System.Windows.Forms.Button();
			this.cboCond = new System.Windows.Forms.ComboBox();
			this.lblCond = new System.Windows.Forms.Label();
			this.spinDelay = new System.Windows.Forms.NumericUpDown();
			this.lblCharCaption = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlMisc.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinSpeed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinDelay)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlGrid
			// 
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlGrid.Location = new System.Drawing.Point(0, 81);
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.Size = new System.Drawing.Size(536, 192);
			this.pnlGrid.TabIndex = 0;
			// 
			// pnlMisc
			// 
			this.pnlMisc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMisc.Controls.Add(this.lblSec);
			this.pnlMisc.Controls.Add(this.lblDelay);
			this.pnlMisc.Controls.Add(this.lblPercent);
			this.pnlMisc.Controls.Add(this.spinSpeed);
			this.pnlMisc.Controls.Add(this.lblSpeed);
			this.pnlMisc.Controls.Add(this.btnCond);
			this.pnlMisc.Controls.Add(this.cboCond);
			this.pnlMisc.Controls.Add(this.lblCond);
			this.pnlMisc.Controls.Add(this.spinDelay);
			this.pnlMisc.Location = new System.Drawing.Point(0, 48);
			this.pnlMisc.Name = "pnlMisc";
			this.pnlMisc.Size = new System.Drawing.Size(512, 32);
			this.pnlMisc.TabIndex = 1;
			// 
			// lblSec
			// 
			this.lblSec.Location = new System.Drawing.Point(480, 4);
			this.lblSec.Name = "lblSec";
			this.lblSec.TabIndex = 7;
			this.lblSec.Text = "sec.";
			this.lblSec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDelay
			// 
			this.lblDelay.Location = new System.Drawing.Point(384, 3);
			this.lblDelay.Name = "lblDelay";
			this.lblDelay.Size = new System.Drawing.Size(40, 24);
			this.lblDelay.TabIndex = 6;
			this.lblDelay.Text = "D&elay:";
			this.lblDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblPercent
			// 
			this.lblPercent.Location = new System.Drawing.Point(360, 1);
			this.lblPercent.Name = "lblPercent";
			this.lblPercent.Size = new System.Drawing.Size(16, 29);
			this.lblPercent.TabIndex = 5;
			this.lblPercent.Text = "%";
			this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// spinSpeed
			// 
			this.spinSpeed.Location = new System.Drawing.Point(304, 5);
			this.spinSpeed.Name = "spinSpeed";
			this.spinSpeed.Size = new System.Drawing.Size(56, 20);
			this.spinSpeed.TabIndex = 4;
			this.spinSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.spinSpeed.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
			// 
			// lblSpeed
			// 
			this.lblSpeed.Location = new System.Drawing.Point(264, 4);
			this.lblSpeed.Name = "lblSpeed";
			this.lblSpeed.Size = new System.Drawing.Size(40, 23);
			this.lblSpeed.TabIndex = 3;
			this.lblSpeed.Text = "&Speed:";
			this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnCond
			// 
			this.btnCond.Location = new System.Drawing.Point(184, 4);
			this.btnCond.Name = "btnCond";
			this.btnCond.TabIndex = 2;
			this.btnCond.Text = "Defi&ne...";
			this.btnCond.Click += new System.EventHandler(this.btnCond_Click);
			// 
			// cboCond
			// 
			this.cboCond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCond.Location = new System.Drawing.Point(64, 5);
			this.cboCond.Name = "cboCond";
			this.cboCond.Size = new System.Drawing.Size(120, 21);
			this.cboCond.TabIndex = 1;
			// 
			// lblCond
			// 
			this.lblCond.Location = new System.Drawing.Point(0, 0);
			this.lblCond.Name = "lblCond";
			this.lblCond.Size = new System.Drawing.Size(56, 32);
			this.lblCond.TabIndex = 0;
			this.lblCond.Text = "&Additional Filter:";
			this.lblCond.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// spinDelay
			// 
			this.spinDelay.Increment = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		65536});
			this.spinDelay.Location = new System.Drawing.Point(424, 5);
			this.spinDelay.Maximum = new System.Decimal(new int[] {
																	  10,
																	  0,
																	  0,
																	  0});
			this.spinDelay.Name = "spinDelay";
			this.spinDelay.Size = new System.Drawing.Size(56, 20);
			this.spinDelay.TabIndex = 4;
			this.spinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.spinDelay.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
			// 
			// lblCharCaption
			// 
			this.lblCharCaption.Location = new System.Drawing.Point(0, 8);
			this.lblCharCaption.Name = "lblCharCaption";
			this.lblCharCaption.Size = new System.Drawing.Size(48, 32);
			this.lblCharCaption.TabIndex = 3;
			this.lblCharCaption.Text = "Selected Thing:";
			this.lblCharCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(56, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 32);
			this.label1.TabIndex = 4;
			this.label1.Text = "#";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// DispWords
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 273);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblCharCaption);
			this.Controls.Add(this.pnlMisc);
			this.Controls.Add(this.pnlGrid);
			this.Name = "DispWords";
			this.ShowInTaskbar = false;
			this.Text = "Word List!";
			this.pnlMisc.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.spinSpeed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinDelay)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region IxCoreColleague Members

		public void Init(Mediator mediator, System.Xml.XmlNode configurationParameters)
		{
			// TODO:  Add DispWords.Init implementation
		}

		public IxCoreColleague[] GetMessageTargets()
		{
			// TODO:  Add DispWords.GetMessageTargets implementation
			return (new IxCoreColleague[] {this});
		}

		#endregion

		#region Setup for the FwGrid control
		private void SetupGrid()
		{
			m_wordgrid = new FwGrid();

			m_wordgrid.Dock = DockStyle.Fill;
			m_wordgrid.FullRowSelect = true;
			m_wordgrid.ColumnHeaderHeight = SystemInformation.MenuFont.Height * 2 + 8;
			m_wordgrid.BorderStyle = BorderStyle.Fixed3D;
			m_wordgrid.GridLineColor = Color.FromArgb(m_wordgrid.BackColor.R - 15,
				m_wordgrid.BackColor.G - 15, m_wordgrid.BackColor.B - 15);

			int index = m_wordgrid.Columns.Add(new FwGridColumn("Phonetic", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns[index].Name = "etic";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("EticLeft", 50));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns[index].Name = "eticffl";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("EticMid", 50));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns[index].Name = "eticffm";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("EticRight", 50));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_wordgrid.Columns[index].Name = "eticffr";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Phonemic", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.PhonemicFont;
			m_wordgrid.Columns[index].Name = "emic";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Tone", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.ToneFont;
			m_wordgrid.Columns[index].Name = "tone";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Orthographic", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.OrthograpicFont;
			m_wordgrid.Columns[index].Name = "ortho";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Gloss", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.GlossFont;
			m_wordgrid.Columns[index].Name = "gloss";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("POS-(Part of Speech)", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.PartOfSpeechFont;
			m_wordgrid.Columns[index].Name = "pos";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Reference", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = PaApp.ReferenceFont;
			m_wordgrid.Columns[index].Name = "ref";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Document Name", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].Name = "docname";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Dialect", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].Name = "dialect";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Audio File", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].Name = "audiofile";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Notebook Ref.", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].Name = "notebook";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("Character Duration", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].Name = "duration";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			index = m_wordgrid.Columns.Add(new FwGridColumn("CV Pattern", 100));
			m_wordgrid.Columns[index].Font = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_wordgrid.Columns[index].Name = "cvpat";
			m_wordgrid.Columns[index].HeaderIsClickable = true;
			m_wordgrid.Columns[index].Click += new EventHandler(ColumnHeader_Click);

			m_wordgrid.ShowColumnHeadings = true;
			pnlGrid.Controls.Add(m_wordgrid);
		}
		#endregion

		private void ColumnHeader_Click(object sender, EventArgs e)
		{
			if (m_wordgrid.Columns.IndexOf((FwGridColumn)sender) == m_lastSelectedColIndex)
				return;

			if (m_lastSelectedColIndex != -1) 
			{
				int len = m_wordgrid.Columns[m_lastSelectedColIndex].Text.Length;
				m_wordgrid.Columns[m_lastSelectedColIndex].Text =
					m_wordgrid.Columns[m_lastSelectedColIndex].Text.Substring(0, len-2);
			}

			((FwGridColumn)sender).Text += " *";
			m_lastSelectedColIndex = m_wordgrid.Columns.IndexOf((FwGridColumn)sender);
			m_wordgrid.Refresh();
		}

		private void btnCond_Click(object sender, System.EventArgs e)
		{
			WLWhereDlg wlwhere = new WLWhereDlg();
			wlwhere.ShowDialog();
		}
	}
}
