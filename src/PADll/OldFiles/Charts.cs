using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.FieldWorks.Common.Controls;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for Charts.
	/// </summary>
	public class Charts : System.Windows.Forms.Form
	{
		#region Variables added by Designer

		private System.Windows.Forms.TabPage tpgHexCV;
		private System.Windows.Forms.TabPage tpgCV;
		private System.Windows.Forms.TabPage tpgVV;
		private System.Windows.Forms.TabPage tpgCC;
		private System.Windows.Forms.TabPage tpgVowelAny;
		private System.Windows.Forms.TabPage tpgCVC;
		private System.Windows.Forms.Panel panel0;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label lblSortKey;
		private System.Windows.Forms.ComboBox cboSortKey;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkIncludeAll0;
		private System.Windows.Forms.CheckBox chkShowEmpty0;
		private System.Windows.Forms.CheckBox chkShowBase0;
		private System.Windows.Forms.CheckBox chkIncludeAll1;
		private System.Windows.Forms.CheckBox chkShowEmpty1;
		private System.Windows.Forms.CheckBox chkShowBase1;
		private System.Windows.Forms.CheckBox chkIncludeAll2;
		private System.Windows.Forms.CheckBox chkShowEmpty2;
		private System.Windows.Forms.CheckBox chkShowBase2;
		private System.Windows.Forms.CheckBox chkIncludeAll3;
		private System.Windows.Forms.CheckBox chkShowEmpty3;
		private System.Windows.Forms.CheckBox chkShowBase3;
		private System.Windows.Forms.CheckBox chkIncludeAll4;
		private System.Windows.Forms.CheckBox chkShowEmpty4;
		private System.Windows.Forms.CheckBox chkShowBase4;
		private System.Windows.Forms.CheckBox chkIncludeAll5;
		private System.Windows.Forms.CheckBox chkShowEmpty5;
		private System.Windows.Forms.CheckBox chkShowBase5;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		private FwGrid m_distGrid0;
		private FwGrid m_distGrid1;
		private FwGrid m_distGrid2;
		private FwGrid m_distGrid3;
		private FwGrid m_distGrid4;
		private FwGrid m_distGrid5;
		private System.Windows.Forms.TabControl tabCharts;
		//private bool m_cancelSearch;

		public Charts()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			SetupDistGrids();
			cboSortKey.Items.AddRange(new string[] {"ANSI Order (A-Z)",
													   "ANSI Order (Z-A)",
													   "Manner of Articulation",
													   "Place of Articulation",
													   "User-Defined Order"});
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
			m_distGrid0.Columns["char"].ColumnFont = PaApp.PhoneticFont;
			m_distGrid1.Columns["char"].ColumnFont = PaApp.PhoneticFont;
			m_distGrid2.Columns["char"].ColumnFont = PaApp.PhoneticFont;
			m_distGrid3.Columns["char"].ColumnFont = PaApp.PhoneticFont;
			m_distGrid4.Columns["char"].ColumnFont = PaApp.PhoneticFont;
			m_distGrid5.Columns["char"].ColumnFont = PaApp.PhoneticFont;
			base.OnPaint (e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabCharts = new System.Windows.Forms.TabControl();
			this.tpgHexCV = new System.Windows.Forms.TabPage();
			this.chkIncludeAll0 = new System.Windows.Forms.CheckBox();
			this.panel0 = new System.Windows.Forms.Panel();
			this.chkShowEmpty0 = new System.Windows.Forms.CheckBox();
			this.chkShowBase0 = new System.Windows.Forms.CheckBox();
			this.tpgCV = new System.Windows.Forms.TabPage();
			this.chkIncludeAll1 = new System.Windows.Forms.CheckBox();
			this.chkShowEmpty1 = new System.Windows.Forms.CheckBox();
			this.chkShowBase1 = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tpgVV = new System.Windows.Forms.TabPage();
			this.chkIncludeAll2 = new System.Windows.Forms.CheckBox();
			this.chkShowEmpty2 = new System.Windows.Forms.CheckBox();
			this.chkShowBase2 = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.tpgCC = new System.Windows.Forms.TabPage();
			this.chkIncludeAll3 = new System.Windows.Forms.CheckBox();
			this.chkShowEmpty3 = new System.Windows.Forms.CheckBox();
			this.chkShowBase3 = new System.Windows.Forms.CheckBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tpgVowelAny = new System.Windows.Forms.TabPage();
			this.chkIncludeAll4 = new System.Windows.Forms.CheckBox();
			this.chkShowEmpty4 = new System.Windows.Forms.CheckBox();
			this.chkShowBase4 = new System.Windows.Forms.CheckBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.tpgCVC = new System.Windows.Forms.TabPage();
			this.chkIncludeAll5 = new System.Windows.Forms.CheckBox();
			this.chkShowEmpty5 = new System.Windows.Forms.CheckBox();
			this.chkShowBase5 = new System.Windows.Forms.CheckBox();
			this.panel5 = new System.Windows.Forms.Panel();
			this.lblSortKey = new System.Windows.Forms.Label();
			this.cboSortKey = new System.Windows.Forms.ComboBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tabCharts.SuspendLayout();
			this.tpgHexCV.SuspendLayout();
			this.tpgCV.SuspendLayout();
			this.tpgVV.SuspendLayout();
			this.tpgCC.SuspendLayout();
			this.tpgVowelAny.SuspendLayout();
			this.tpgCVC.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCharts
			// 
			this.tabCharts.Controls.Add(this.tpgHexCV);
			this.tabCharts.Controls.Add(this.tpgCV);
			this.tabCharts.Controls.Add(this.tpgVV);
			this.tabCharts.Controls.Add(this.tpgCC);
			this.tabCharts.Controls.Add(this.tpgVowelAny);
			this.tabCharts.Controls.Add(this.tpgCVC);
			this.tabCharts.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabCharts.Location = new System.Drawing.Point(0, 0);
			this.tabCharts.Name = "tabCharts";
			this.tabCharts.SelectedIndex = 0;
			this.tabCharts.Size = new System.Drawing.Size(568, 280);
			this.tabCharts.TabIndex = 0;
			// 
			// tpgHexCV
			// 
			this.tpgHexCV.Controls.Add(this.chkIncludeAll0);
			this.tpgHexCV.Controls.Add(this.panel0);
			this.tpgHexCV.Controls.Add(this.chkShowEmpty0);
			this.tpgHexCV.Controls.Add(this.chkShowBase0);
			this.tpgHexCV.Location = new System.Drawing.Point(4, 22);
			this.tpgHexCV.Name = "tpgHexCV";
			this.tpgHexCV.Size = new System.Drawing.Size(560, 254);
			this.tpgHexCV.TabIndex = 0;
			this.tpgHexCV.Text = "#/C/V";
			// 
			// chkIncludeAll0
			// 
			this.chkIncludeAll0.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkIncludeAll0.Location = new System.Drawing.Point(16, 192);
			this.chkIncludeAll0.Name = "chkIncludeAll0";
			this.chkIncludeAll0.Size = new System.Drawing.Size(104, 16);
			this.chkIncludeAll0.TabIndex = 1;
			this.chkIncludeAll0.Text = "chkIncludeAll0";
			// 
			// panel0
			// 
			this.panel0.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel0.Location = new System.Drawing.Point(0, 0);
			this.panel0.Name = "panel0";
			this.panel0.Size = new System.Drawing.Size(560, 176);
			this.panel0.TabIndex = 0;
			// 
			// chkShowEmpty0
			// 
			this.chkShowEmpty0.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowEmpty0.Location = new System.Drawing.Point(16, 216);
			this.chkShowEmpty0.Name = "chkShowEmpty0";
			this.chkShowEmpty0.Size = new System.Drawing.Size(112, 16);
			this.chkShowEmpty0.TabIndex = 1;
			this.chkShowEmpty0.Text = "chkShowEmpty0";
			// 
			// chkShowBase0
			// 
			this.chkShowBase0.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowBase0.Location = new System.Drawing.Point(216, 216);
			this.chkShowBase0.Name = "chkShowBase0";
			this.chkShowBase0.Size = new System.Drawing.Size(104, 16);
			this.chkShowBase0.TabIndex = 1;
			this.chkShowBase0.Text = "chkShowBase0";
			// 
			// tpgCV
			// 
			this.tpgCV.Controls.Add(this.chkIncludeAll1);
			this.tpgCV.Controls.Add(this.chkShowEmpty1);
			this.tpgCV.Controls.Add(this.chkShowBase1);
			this.tpgCV.Controls.Add(this.panel1);
			this.tpgCV.Location = new System.Drawing.Point(4, 22);
			this.tpgCV.Name = "tpgCV";
			this.tpgCV.Size = new System.Drawing.Size(560, 254);
			this.tpgCV.TabIndex = 1;
			this.tpgCV.Text = "CV";
			// 
			// chkIncludeAll1
			// 
			this.chkIncludeAll1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkIncludeAll1.Location = new System.Drawing.Point(16, 192);
			this.chkIncludeAll1.Name = "chkIncludeAll1";
			this.chkIncludeAll1.Size = new System.Drawing.Size(104, 16);
			this.chkIncludeAll1.TabIndex = 4;
			this.chkIncludeAll1.Text = "chkIncludeAll1";
			// 
			// chkShowEmpty1
			// 
			this.chkShowEmpty1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowEmpty1.Location = new System.Drawing.Point(16, 216);
			this.chkShowEmpty1.Name = "chkShowEmpty1";
			this.chkShowEmpty1.Size = new System.Drawing.Size(112, 16);
			this.chkShowEmpty1.TabIndex = 3;
			this.chkShowEmpty1.Text = "chkShowEmpty1";
			// 
			// chkShowBase1
			// 
			this.chkShowBase1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowBase1.Location = new System.Drawing.Point(216, 216);
			this.chkShowBase1.Name = "chkShowBase1";
			this.chkShowBase1.Size = new System.Drawing.Size(104, 16);
			this.chkShowBase1.TabIndex = 2;
			this.chkShowBase1.Text = "chkShowBase1";
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(560, 176);
			this.panel1.TabIndex = 0;
			// 
			// tpgVV
			// 
			this.tpgVV.Controls.Add(this.chkIncludeAll2);
			this.tpgVV.Controls.Add(this.chkShowEmpty2);
			this.tpgVV.Controls.Add(this.chkShowBase2);
			this.tpgVV.Controls.Add(this.panel2);
			this.tpgVV.Location = new System.Drawing.Point(4, 22);
			this.tpgVV.Name = "tpgVV";
			this.tpgVV.Size = new System.Drawing.Size(560, 254);
			this.tpgVV.TabIndex = 2;
			this.tpgVV.Text = "VV";
			// 
			// chkIncludeAll2
			// 
			this.chkIncludeAll2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkIncludeAll2.Location = new System.Drawing.Point(16, 192);
			this.chkIncludeAll2.Name = "chkIncludeAll2";
			this.chkIncludeAll2.Size = new System.Drawing.Size(104, 16);
			this.chkIncludeAll2.TabIndex = 7;
			this.chkIncludeAll2.Text = "chkIncludeAll2";
			// 
			// chkShowEmpty2
			// 
			this.chkShowEmpty2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowEmpty2.Location = new System.Drawing.Point(16, 216);
			this.chkShowEmpty2.Name = "chkShowEmpty2";
			this.chkShowEmpty2.Size = new System.Drawing.Size(112, 16);
			this.chkShowEmpty2.TabIndex = 6;
			this.chkShowEmpty2.Text = "chkShowEmpty2";
			// 
			// chkShowBase2
			// 
			this.chkShowBase2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowBase2.Location = new System.Drawing.Point(216, 216);
			this.chkShowBase2.Name = "chkShowBase2";
			this.chkShowBase2.Size = new System.Drawing.Size(104, 16);
			this.chkShowBase2.TabIndex = 5;
			this.chkShowBase2.Text = "chkShowBase2";
			// 
			// panel2
			// 
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(560, 176);
			this.panel2.TabIndex = 0;
			// 
			// tpgCC
			// 
			this.tpgCC.Controls.Add(this.chkIncludeAll3);
			this.tpgCC.Controls.Add(this.chkShowEmpty3);
			this.tpgCC.Controls.Add(this.chkShowBase3);
			this.tpgCC.Controls.Add(this.panel3);
			this.tpgCC.Location = new System.Drawing.Point(4, 22);
			this.tpgCC.Name = "tpgCC";
			this.tpgCC.Size = new System.Drawing.Size(560, 254);
			this.tpgCC.TabIndex = 3;
			this.tpgCC.Text = "CC";
			// 
			// chkIncludeAll3
			// 
			this.chkIncludeAll3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkIncludeAll3.Location = new System.Drawing.Point(16, 192);
			this.chkIncludeAll3.Name = "chkIncludeAll3";
			this.chkIncludeAll3.Size = new System.Drawing.Size(104, 16);
			this.chkIncludeAll3.TabIndex = 7;
			this.chkIncludeAll3.Text = "chkIncludeAll3";
			// 
			// chkShowEmpty3
			// 
			this.chkShowEmpty3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowEmpty3.Location = new System.Drawing.Point(16, 216);
			this.chkShowEmpty3.Name = "chkShowEmpty3";
			this.chkShowEmpty3.Size = new System.Drawing.Size(112, 16);
			this.chkShowEmpty3.TabIndex = 6;
			this.chkShowEmpty3.Text = "chkShowEmpty3";
			// 
			// chkShowBase3
			// 
			this.chkShowBase3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowBase3.Location = new System.Drawing.Point(216, 216);
			this.chkShowBase3.Name = "chkShowBase3";
			this.chkShowBase3.Size = new System.Drawing.Size(104, 16);
			this.chkShowBase3.TabIndex = 5;
			this.chkShowBase3.Text = "chkShowBase3";
			// 
			// panel3
			// 
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(560, 176);
			this.panel3.TabIndex = 0;
			// 
			// tpgVowelAny
			// 
			this.tpgVowelAny.Controls.Add(this.chkIncludeAll4);
			this.tpgVowelAny.Controls.Add(this.chkShowEmpty4);
			this.tpgVowelAny.Controls.Add(this.chkShowBase4);
			this.tpgVowelAny.Controls.Add(this.panel4);
			this.tpgVowelAny.Location = new System.Drawing.Point(4, 22);
			this.tpgVowelAny.Name = "tpgVowelAny";
			this.tpgVowelAny.Size = new System.Drawing.Size(560, 254);
			this.tpgVowelAny.TabIndex = 4;
			this.tpgVowelAny.Text = ".CV.";
			// 
			// chkIncludeAll4
			// 
			this.chkIncludeAll4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkIncludeAll4.Location = new System.Drawing.Point(16, 192);
			this.chkIncludeAll4.Name = "chkIncludeAll4";
			this.chkIncludeAll4.Size = new System.Drawing.Size(104, 16);
			this.chkIncludeAll4.TabIndex = 7;
			this.chkIncludeAll4.Text = "chkIncludeAll4";
			// 
			// chkShowEmpty4
			// 
			this.chkShowEmpty4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowEmpty4.Location = new System.Drawing.Point(16, 216);
			this.chkShowEmpty4.Name = "chkShowEmpty4";
			this.chkShowEmpty4.Size = new System.Drawing.Size(112, 16);
			this.chkShowEmpty4.TabIndex = 6;
			this.chkShowEmpty4.Text = "chkShowEmpty4";
			// 
			// chkShowBase4
			// 
			this.chkShowBase4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowBase4.Location = new System.Drawing.Point(216, 216);
			this.chkShowBase4.Name = "chkShowBase4";
			this.chkShowBase4.Size = new System.Drawing.Size(104, 16);
			this.chkShowBase4.TabIndex = 5;
			this.chkShowBase4.Text = "chkShowBase4";
			// 
			// panel4
			// 
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(560, 176);
			this.panel4.TabIndex = 0;
			// 
			// tpgCVC
			// 
			this.tpgCVC.Controls.Add(this.chkIncludeAll5);
			this.tpgCVC.Controls.Add(this.chkShowEmpty5);
			this.tpgCVC.Controls.Add(this.chkShowBase5);
			this.tpgCVC.Controls.Add(this.panel5);
			this.tpgCVC.Location = new System.Drawing.Point(4, 22);
			this.tpgCVC.Name = "tpgCVC";
			this.tpgCVC.Size = new System.Drawing.Size(560, 254);
			this.tpgCVC.TabIndex = 5;
			this.tpgCVC.Text = "C1VC2";
			// 
			// chkIncludeAll5
			// 
			this.chkIncludeAll5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkIncludeAll5.Location = new System.Drawing.Point(16, 192);
			this.chkIncludeAll5.Name = "chkIncludeAll5";
			this.chkIncludeAll5.Size = new System.Drawing.Size(104, 16);
			this.chkIncludeAll5.TabIndex = 7;
			this.chkIncludeAll5.Text = "chkIncludeAll5";
			// 
			// chkShowEmpty5
			// 
			this.chkShowEmpty5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowEmpty5.Location = new System.Drawing.Point(16, 216);
			this.chkShowEmpty5.Name = "chkShowEmpty5";
			this.chkShowEmpty5.Size = new System.Drawing.Size(112, 16);
			this.chkShowEmpty5.TabIndex = 6;
			this.chkShowEmpty5.Text = "chkShowEmpty5";
			// 
			// chkShowBase5
			// 
			this.chkShowBase5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowBase5.Location = new System.Drawing.Point(216, 216);
			this.chkShowBase5.Name = "chkShowBase5";
			this.chkShowBase5.Size = new System.Drawing.Size(104, 16);
			this.chkShowBase5.TabIndex = 5;
			this.chkShowBase5.Text = "chkShowBase5";
			// 
			// panel5
			// 
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(0, 0);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(560, 176);
			this.panel5.TabIndex = 0;
			// 
			// lblSortKey
			// 
			this.lblSortKey.Location = new System.Drawing.Point(16, 299);
			this.lblSortKey.Name = "lblSortKey";
			this.lblSortKey.Size = new System.Drawing.Size(120, 16);
			this.lblSortKey.TabIndex = 1;
			this.lblSortKey.Text = "&Sort Char. Column by:";
			// 
			// cboSortKey
			// 
			this.cboSortKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSortKey.Location = new System.Drawing.Point(131, 296);
			this.cboSortKey.Name = "cboSortKey";
			this.cboSortKey.Size = new System.Drawing.Size(121, 21);
			this.cboSortKey.TabIndex = 2;
			// 
			// btnSearch
			// 
			this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSearch.Location = new System.Drawing.Point(392, 296);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.TabIndex = 3;
			this.btnSearch.Text = "Search";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Enabled = false;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(472, 296);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			// 
			// Charts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(568, 341);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.cboSortKey);
			this.Controls.Add(this.lblSortKey);
			this.Controls.Add(this.tabCharts);
			this.MinimumSize = new System.Drawing.Size(400, 200);
			this.Name = "Charts";
			this.Text = "Distribution Charts";
			this.tabCharts.ResumeLayout(false);
			this.tpgHexCV.ResumeLayout(false);
			this.tpgCV.ResumeLayout(false);
			this.tpgVV.ResumeLayout(false);
			this.tpgCC.ResumeLayout(false);
			this.tpgVowelAny.ResumeLayout(false);
			this.tpgCVC.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void SetupDistGrids()
		{
			int fontHeight = 0;

			#region Setup for m_distGrid0
			m_distGrid0 = new FwGrid();
			panel0.Controls.Add(m_distGrid0);

			m_distGrid0.Dock = DockStyle.Fill;
			m_distGrid0.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_distGrid0.BorderStyle = BorderStyle.Fixed3D;
			m_distGrid0.GridLineColor = Color.FromArgb(m_distGrid0.BackColor.R - 15,
				m_distGrid0.BackColor.G - 15, m_distGrid0.BackColor.B - 15);
			
			int index = m_distGrid0.Columns.Add(new FwGridColumn("Char.", 30));
			m_distGrid0.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_distGrid0.Columns[index].AllowEdit = false;
			m_distGrid0.Columns[index].Name = "char";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			index = m_distGrid0.Columns.Add(new FwGridColumn("#_", 30));
			m_distGrid0.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].AllowEdit = false;
			m_distGrid0.Columns[index].Name = "col1";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid0.Columns.Add(new FwGridColumn("_#", 30));
			m_distGrid0.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].AllowEdit = false;
			m_distGrid0.Columns[index].Name = "col2";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid0.Columns.Add(new FwGridColumn("C_", 30));
			m_distGrid0.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].AllowEdit = false;
			m_distGrid0.Columns[index].Name = "col3";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid0.Columns.Add(new FwGridColumn("_C", 30));
			m_distGrid0.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].AllowEdit = false;
			m_distGrid0.Columns[index].Name = "col4";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid0.Columns.Add(new FwGridColumn("V_", 30));
			m_distGrid0.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].AllowEdit = false;
			m_distGrid0.Columns[index].Name = "col5";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid0.Columns.Add(new FwGridColumn("_V", 30));
			m_distGrid0.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid0.Columns[index].AllowEdit = false;
			m_distGrid0.Columns[index].Name = "col6";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			m_distGrid0.RowHeight = fontHeight + 3;
			#endregion

			#region Setup for m_distGrid1
			m_distGrid1 = new FwGrid();
			panel1.Controls.Add(m_distGrid1);

			m_distGrid1.Dock = DockStyle.Fill;
			m_distGrid1.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_distGrid1.BorderStyle = BorderStyle.Fixed3D;
			m_distGrid1.GridLineColor = Color.FromArgb(m_distGrid1.BackColor.R - 15,
				m_distGrid1.BackColor.G - 15, m_distGrid1.BackColor.B - 15);
			
			index = m_distGrid1.Columns.Add(new FwGridColumn("Char.", 30));
			m_distGrid1.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid1.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_distGrid1.Columns[index].AllowEdit = false;
			m_distGrid1.Columns[index].Name = "char";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			m_distGrid1.RowHeight = fontHeight + 3;
			#endregion

			#region Setup for m_distGrid2
			m_distGrid2 = new FwGrid();
			panel2.Controls.Add(m_distGrid2);

			m_distGrid2.Dock = DockStyle.Fill;
			m_distGrid2.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_distGrid2.BorderStyle = BorderStyle.Fixed3D;
			m_distGrid2.GridLineColor = Color.FromArgb(m_distGrid2.BackColor.R - 15,
				m_distGrid2.BackColor.G - 15, m_distGrid2.BackColor.B - 15);
			
			index = m_distGrid2.Columns.Add(new FwGridColumn("Char.", 30));
			m_distGrid2.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid2.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_distGrid2.Columns[index].AllowEdit = false;
			m_distGrid2.Columns[index].Name = "char";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			m_distGrid2.RowHeight = fontHeight + 3;
			#endregion

			#region Setup for m_distGrid3
			m_distGrid3 = new FwGrid();
			panel3.Controls.Add(m_distGrid3);

			m_distGrid3.Dock = DockStyle.Fill;
			m_distGrid3.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_distGrid3.BorderStyle = BorderStyle.Fixed3D;
			m_distGrid3.GridLineColor = Color.FromArgb(m_distGrid3.BackColor.R - 15,
				m_distGrid3.BackColor.G - 15, m_distGrid3.BackColor.B - 15);
			
			index = m_distGrid3.Columns.Add(new FwGridColumn("Char.", 30));
			m_distGrid3.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid3.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_distGrid3.Columns[index].AllowEdit = false;
			m_distGrid3.Columns[index].Name = "char";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			m_distGrid3.RowHeight = fontHeight + 3;
			#endregion

			#region Setup for m_distGrid4
			m_distGrid4 = new FwGrid();
			panel4.Controls.Add(m_distGrid4);

			m_distGrid4.Dock = DockStyle.Fill;
			m_distGrid4.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_distGrid4.BorderStyle = BorderStyle.Fixed3D;
			m_distGrid4.GridLineColor = Color.FromArgb(m_distGrid4.BackColor.R - 15,
				m_distGrid4.BackColor.G - 15, m_distGrid4.BackColor.B - 15);
			
			index = m_distGrid4.Columns.Add(new FwGridColumn("Char.", 30));
			m_distGrid4.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_distGrid4.Columns[index].AllowEdit = false;
			m_distGrid4.Columns[index].Name = "char";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			index = m_distGrid4.Columns.Add(new FwGridColumn("_V", 30));
			m_distGrid4.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].AllowEdit = false;
			m_distGrid4.Columns[index].Name = "col1";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid4.Columns.Add(new FwGridColumn("_V_", 30));
			m_distGrid4.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].AllowEdit = false;
			m_distGrid4.Columns[index].Name = "col2";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid4.Columns.Add(new FwGridColumn("_V_ _", 30));
			m_distGrid4.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].AllowEdit = false;
			m_distGrid4.Columns[index].Name = "col3";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid4.Columns.Add(new FwGridColumn("_ _V_", 30));
			m_distGrid4.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].AllowEdit = false;
			m_distGrid4.Columns[index].Name = "col4";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid4.Columns.Add(new FwGridColumn("_ _V_ _", 30));
			m_distGrid4.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid4.Columns[index].AllowEdit = false;
			m_distGrid4.Columns[index].Name = "col5";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			m_distGrid4.RowHeight = fontHeight + 3;
			#endregion

			#region Setup for m_distGrid5
			m_distGrid5 = new FwGrid();
			panel5.Controls.Add(m_distGrid5);

			m_distGrid5.Dock = DockStyle.Fill;
			m_distGrid5.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_distGrid5.BorderStyle = BorderStyle.Fixed3D;
			m_distGrid5.GridLineColor = Color.FromArgb(m_distGrid5.BackColor.R - 15,
				m_distGrid5.BackColor.G - 15, m_distGrid5.BackColor.B - 15);
			
			index = m_distGrid5.Columns.Add(new FwGridColumn("Char.", 30));
			m_distGrid5.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_distGrid5.Columns[index].AllowEdit = false;
			m_distGrid5.Columns[index].Name = "char";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			index = m_distGrid5.Columns.Add(new FwGridColumn("#_VC", 30));
			m_distGrid5.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].AllowEdit = false;
			m_distGrid5.Columns[index].Name = "col1";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid5.Columns.Add(new FwGridColumn("_VC", 30));
			m_distGrid5.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].AllowEdit = false;
			m_distGrid5.Columns[index].Name = "col2";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid5.Columns.Add(new FwGridColumn("CV_", 30));
			m_distGrid5.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].AllowEdit = false;
			m_distGrid5.Columns[index].Name = "col3";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_distGrid5.Columns.Add(new FwGridColumn("CV_#", 30));
			m_distGrid5.Columns[index].Font = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_distGrid5.Columns[index].AllowEdit = false;
			m_distGrid5.Columns[index].Name = "col4";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			m_distGrid5.RowHeight = fontHeight + 3;
			#endregion
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
			
			btnCancel.Enabled = true;
			btnCancel.Focus();
			btnSearch.Enabled = false;
			//m_cancelSearch = false;

			ClearOutTempWLInfoTable(tabCharts.SelectedIndex);
			ClearCounts(tabCharts.SelectedIndex);
			GetCounts(tabCharts.SelectedIndex);
			switch (tabCharts.SelectedIndex)
			{
				case 0:
					m_distGrid0.Focus();
					break;
				case 1:
					m_distGrid1.Focus();
					break;
				case 2:
					m_distGrid2.Focus();
					break;
				case 3:
					m_distGrid3.Focus();
					break;
				case 4:
					m_distGrid4.Focus();
					break;
				case 5:
					m_distGrid5.Focus();
					break;
			}
			btnCancel.Enabled = false;
			btnSearch.Enabled = true;
		}

		private void ClearOutTempWLInfoTable(int gridIndex)
		{
		}

		private void ClearCounts(int gridIndex)
		{
		}

		private void GetCounts(int gridIndex)
		{
		}
	}
}
