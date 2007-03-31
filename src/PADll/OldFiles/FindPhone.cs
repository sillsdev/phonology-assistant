using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SIL.Pa.Database;
using SIL.FieldWorks.Common.Controls;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for FindPhone.
	/// </summary>
	public class FindPhone : System.Windows.Forms.Form
	{
		#region Variables added by Designer
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel pnlPattern0;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlPattern2;
		private System.Windows.Forms.Panel pnlPattern1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnDefClasses;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.ListBox lstPatterns;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label lblCatList;
		private System.Windows.Forms.Label lblPatternList;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.CheckBox chkAllOccurences;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		/// <summary>
		/// This would have been EditableListBox if the item didn't get
		/// changed into type string every time someone tried editing it.
		/// We need a way to store the Category ID, so we've chosen to
		/// use a hidden column.
		/// </summary>
		private FwGrid m_CatGrid;
		private Point m_P0Loc, m_P1Loc, m_P2Loc, m_L1Loc, m_L2Loc, m_ClrLoc;
		private Size m_P0Size, m_P1Size, m_P2Size;
		private ListBox lstPopup;
		private Panel m_selectedPanel;
		private int m_PrevCatIndex;
		private PaDataTable m_CatTable, m_PatternTable;

		public FindPhone()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			m_P0Loc = pnlPattern0.Location;
			m_P0Size = pnlPattern0.Size;
			m_P1Loc = pnlPattern1.Location;
			m_P1Size = pnlPattern1.Size;
			m_P2Loc = pnlPattern2.Location;
			m_P2Size = pnlPattern2.Size;
			m_L1Loc = label1.Location;
			m_L2Loc = label2.Location;
			m_ClrLoc = btnClear.Location;
			lstPatterns.Font = new Font(PaApp.PhoneticFont.Name, 10);
			try
			{
				lstPopupSetup();
				CatGridSetup();
				m_PatternTable = new PaDataTable("SELECT * FROM FFPatterns");
				pnlPattern0.ControlAdded += new ControlEventHandler(PatternAdded);
				pnlPattern1.ControlAdded += new ControlEventHandler(PatternAdded);
				pnlPattern2.ControlAdded += new ControlEventHandler(PatternAdded);
				pnlPattern0.Click += new EventHandler(Pattern_Click);
				pnlPattern1.Click += new EventHandler(Pattern_Click);
				pnlPattern2.Click += new EventHandler(Pattern_Click);
				pnlPattern0.Tag = "Empty";
				pnlPattern1.Tag = "Empty";
				pnlPattern2.Tag = "Empty";
			}
			catch
			{
				MessageBox.Show("Please open a Database first.", "Phonology Assistant",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
			lblPatternList.Left = splitter1.Left;
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

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);
			groupBox1.Refresh();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Panel pnl = pnlPattern0;
			pnlPattern_MouseLeave(pnl, null);
			pnl = pnlPattern1;
			pnlPattern_MouseLeave(pnl, null);
			pnl = pnlPattern2;
			pnlPattern_MouseLeave(pnl, null);
			lstPatterns.Font = new Font(PaApp.PhoneticFont.Name, 10);
			base.OnPaint (e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.btnClear = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.pnlPattern0 = new System.Windows.Forms.Panel();
			this.pnlPattern2 = new System.Windows.Forms.Panel();
			this.pnlPattern1 = new System.Windows.Forms.Panel();
			this.chkAllOccurences = new System.Windows.Forms.CheckBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnDefClasses = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lstPatterns = new System.Windows.Forms.ListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.lblCatList = new System.Windows.Forms.Label();
			this.lblPatternList = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.pnlSearch);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 14);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(520, 66);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search Pattern";
			// 
			// pnlSearch
			// 
			this.pnlSearch.AutoScroll = true;
			this.pnlSearch.Controls.Add(this.btnClear);
			this.pnlSearch.Controls.Add(this.label1);
			this.pnlSearch.Controls.Add(this.label2);
			this.pnlSearch.Controls.Add(this.pnlPattern0);
			this.pnlSearch.Controls.Add(this.pnlPattern2);
			this.pnlSearch.Controls.Add(this.pnlPattern1);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(3, 16);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(514, 47);
			this.pnlSearch.TabIndex = 3;
			this.pnlSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSearch_Paint);
			// 
			// btnClear
			// 
			this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnClear.Location = new System.Drawing.Point(400, 12);
			this.btnClear.Name = "btnClear";
			this.btnClear.TabIndex = 2;
			this.btnClear.Text = "C&lear";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(72, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "/";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(148, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "_";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlPattern0
			// 
			this.pnlPattern0.Location = new System.Drawing.Point(16, 16);
			this.pnlPattern0.Name = "pnlPattern0";
			this.pnlPattern0.Size = new System.Drawing.Size(48, 16);
			this.pnlPattern0.TabIndex = 0;
			this.pnlPattern0.MouseEnter += new System.EventHandler(this.pnlPattern_MouseEnter);
			this.pnlPattern0.MouseLeave += new System.EventHandler(this.pnlPattern_MouseLeave);
			// 
			// pnlPattern2
			// 
			this.pnlPattern2.Location = new System.Drawing.Point(168, 16);
			this.pnlPattern2.Name = "pnlPattern2";
			this.pnlPattern2.Size = new System.Drawing.Size(48, 16);
			this.pnlPattern2.TabIndex = 0;
			this.pnlPattern2.MouseEnter += new System.EventHandler(this.pnlPattern_MouseEnter);
			this.pnlPattern2.MouseLeave += new System.EventHandler(this.pnlPattern_MouseLeave);
			// 
			// pnlPattern1
			// 
			this.pnlPattern1.Location = new System.Drawing.Point(96, 16);
			this.pnlPattern1.Name = "pnlPattern1";
			this.pnlPattern1.Size = new System.Drawing.Size(48, 16);
			this.pnlPattern1.TabIndex = 0;
			this.pnlPattern1.MouseEnter += new System.EventHandler(this.pnlPattern_MouseEnter);
			this.pnlPattern1.MouseLeave += new System.EventHandler(this.pnlPattern_MouseLeave);
			// 
			// chkAllOccurences
			// 
			this.chkAllOccurences.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkAllOccurences.Location = new System.Drawing.Point(8, 88);
			this.chkAllOccurences.Name = "chkAllOccurences";
			this.chkAllOccurences.Size = new System.Drawing.Size(248, 24);
			this.chkAllOccurences.TabIndex = 1;
			this.chkAllOccurences.Text = "Search s&hows all occurences of each word";
			// 
			// btnSearch
			// 
			this.btnSearch.Enabled = false;
			this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSearch.Location = new System.Drawing.Point(272, 88);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(64, 23);
			this.btnSearch.TabIndex = 2;
			this.btnSearch.Text = "&Search";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnDefClasses
			// 
			this.btnDefClasses.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnDefClasses.Location = new System.Drawing.Point(339, 88);
			this.btnDefClasses.Name = "btnDefClasses";
			this.btnDefClasses.Size = new System.Drawing.Size(104, 23);
			this.btnDefClasses.TabIndex = 2;
			this.btnDefClasses.Text = "Defi&ne Classes...";
			this.btnDefClasses.Click += new System.EventHandler(this.btnDefClasses_Click);
			// 
			// btnSave
			// 
			this.btnSave.Enabled = false;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSave.Location = new System.Drawing.Point(446, 88);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(82, 23);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Sa&ve Pattern";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lstPatterns);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 141);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(536, 152);
			this.panel1.TabIndex = 3;
			// 
			// lstPatterns
			// 
			this.lstPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstPatterns.HorizontalScrollbar = true;
			this.lstPatterns.Location = new System.Drawing.Point(3, 0);
			this.lstPatterns.Name = "lstPatterns";
			this.lstPatterns.Size = new System.Drawing.Size(533, 147);
			this.lstPatterns.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(0, 0);
			this.splitter1.MinExtra = 120;
			this.splitter1.MinSize = 120;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 152);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
			// 
			// lblCatList
			// 
			this.lblCatList.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblCatList.Location = new System.Drawing.Point(8, 120);
			this.lblCatList.Name = "lblCatList";
			this.lblCatList.Size = new System.Drawing.Size(104, 16);
			this.lblCatList.TabIndex = 4;
			this.lblCatList.Text = "Search &Categories:";
			// 
			// lblPatternList
			// 
			this.lblPatternList.AutoSize = true;
			this.lblPatternList.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblPatternList.Location = new System.Drawing.Point(208, 120);
			this.lblPatternList.Name = "lblPatternList";
			this.lblPatternList.Size = new System.Drawing.Size(0, 16);
			this.lblPatternList.TabIndex = 0;
			// 
			// FindPhone
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 293);
			this.Controls.Add(this.lblCatList);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.chkAllOccurences);
			this.Controls.Add(this.btnDefClasses);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.lblPatternList);
			this.Controls.Add(this.groupBox1);
			this.Name = "FindPhone";
			this.ShowInTaskbar = false;
			this.Text = "Find Phones";
			this.groupBox1.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Makes the panel have a pop up feel whenever the mouse is over the panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlPattern_MouseEnter(object sender, EventArgs e)
		{
			Panel pnl = sender as Panel;
			Text = pnl.Name;
			ControlPaint.DrawBorder3D(pnlSearch.CreateGraphics(), pnl.Left - 2,
				pnl.Top - 2, pnl.Width + 4, pnl.Height + 4,
				Border3DStyle.Raised);
		}

		/// <summary>
		/// Reverts back to normal when mouse leaves the panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlPattern_MouseLeave(object sender, EventArgs e)
		{
			Panel pnl = sender as Panel;
			Text = "Find Phones";
			///this is to check if the panel has an item selected or not. 
			///because the border style is different for an empty panel 
			///as compared to a panel which has a search string.
			if ((string)((Panel)sender).Tag == "Empty")
			{
				ControlPaint.DrawBorder3D(pnlSearch.CreateGraphics(), pnl.Left - 2,
					pnl.Top - 2, pnl.Width + 4, pnl.Height + 4,
					Border3DStyle.Sunken);
			}
			else
			{
				pnlSearch.CreateGraphics().DrawRectangle(new Pen(Color.FromKnownColor(KnownColor.Control), 4.0f),
					pnl.Left - 2, pnl.Top - 2, pnl.Width + 4, pnl.Height + 4);
			}
		}

		private void pnlSearch_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Panel pnl = pnlPattern0;
			pnlPattern_MouseLeave(pnl, null);
			pnl = pnlPattern1;
			pnlPattern_MouseLeave(pnl, null);
			pnl = pnlPattern2;
			pnlPattern_MouseLeave(pnl, null);
		}

		private void lstPopupSetup()
		{
			lstPopup = new ListBox();
			lstPopup.Items.AddRange(new string[] {"<all consonants>",
													 "<all vowels>",
													 "<something>",
													 "<something>"});
			lstPopup.Click += new EventHandler(lstPopup_Click);
			lstPopup.LostFocus += new EventHandler(lstPopup_LostFocus);
			lstPopup.KeyDown += new KeyEventHandler(lstPopup_KeyDown);
			lstPopup.Visible = false;
		}

		private void lstPopup_Click(object sender, EventArgs e)
		{
			if (lstPopup.SelectedIndex == -1)
				return;
			Label lbl = new Label();
			lbl.Text = (string)lstPopup.SelectedItem;
			lbl.AutoSize = true;
			lbl.Enabled = false;
			m_selectedPanel.Controls.Add(lbl);
			m_selectedPanel.Tag = "UnEmpty";
			lstPopup.Visible = false;
			lstPopup.SelectedIndex = -1;
			Controls.Remove(lstPopup);
			ArrangeGroupBox();
			groupBox1.Refresh();
			btnSearch.Enabled = btnSave.Enabled =
				(((string)pnlPattern0.Tag == "UnEmpty") &&
				((string)pnlPattern1.Tag == "UnEmpty") &&
				((string)pnlPattern2.Tag == "UnEmpty"));
		}

		private void PatternAdded(object sender, ControlEventArgs e)
		{
			int x=0;

			foreach (Control lbl in ((Panel)sender).Controls)
			{
				lbl.Location = new Point(x,0);
				x = lbl.Right;
			}

			((Panel)sender).Width = x;
		}

		private void ArrangeGroupBox()
		{
			label1.Location = new Point(pnlPattern0.Right + 2, label1.Top);
			pnlPattern1.Location = new Point(label1.Right + 2, pnlPattern1.Top);
			label2.Location = new Point(pnlPattern1.Right + 2, label2.Top);
			pnlPattern2.Location = new Point(label2.Right + 2, pnlPattern2.Top);
			if (pnlPattern2.Right > btnClear.Left + 4)
				btnClear.Location = new Point(pnlPattern2.Right + 4,btnClear.Top);
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			pnlSearch.AutoScrollPosition = new Point(0,0);
			pnlPattern0.Controls.Clear();
			pnlPattern0.Tag = "Empty";
			pnlPattern1.Controls.Clear();
			pnlPattern1.Tag = "Empty";
			pnlPattern2.Controls.Clear();
			pnlPattern2.Tag = "Empty";
			pnlPattern0.Size = m_P0Size;
			pnlPattern0.Location = m_P0Loc;
			pnlPattern1.Size = m_P1Size;
			pnlPattern1.Location = m_P1Loc;
			pnlPattern2.Size = m_P2Size;
			pnlPattern2.Location = m_P2Loc;
			label1.Location = m_L1Loc;
			label2.Location = m_L2Loc;
			btnClear.Location = m_ClrLoc;
			groupBox1.Refresh();
			btnSave.Enabled = btnSearch.Enabled = false;
		}

		private void Pattern_Click(object sender, EventArgs e)
		{
			btnClear.Enabled = false;
			m_selectedPanel = sender as Panel;
			pnlSearch.ScrollControlIntoView(m_selectedPanel);
			Point lstLoc = new Point(m_selectedPanel.Left, m_selectedPanel.Bottom);
			lstPopup.Location = PointToClient(pnlSearch.PointToScreen(lstLoc));
			lstPopup.Visible = true;
			Controls.Add(lstPopup);
			lstPopup.BringToFront();
			this.ActiveControl = lstPopup;
			btnClear.Enabled = true;
		}

		private void lstPopup_LostFocus(object sender, EventArgs e)
		{
			lstPopup.Visible = false;
			lstPopup.SelectedIndex = -1;
			Controls.Remove(lstPopup);
		}

		private void lstPopup_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					Label lbl = new Label();
					lbl.Text = (string)lstPopup.SelectedItem;
					lbl.AutoSize = true;
					lbl.Enabled = false;
					m_selectedPanel.Controls.Add(lbl);
					m_selectedPanel.Tag = "UnEmpty";
					lstPopup.Visible = false;
					lstPopup.SelectedIndex = -1;
					Controls.Remove(lstPopup);
					ArrangeGroupBox();
					groupBox1.Refresh();
					btnSearch.Enabled = btnSave.Enabled =
						(((string)pnlPattern0.Tag == "UnEmpty") &&
						((string)pnlPattern1.Tag == "UnEmpty") &&
						((string)pnlPattern2.Tag == "UnEmpty"));
					return;
				case Keys.Escape:
					lstPopup.Visible = false;
					lstPopup.SelectedIndex = -1;
					Controls.Remove(lstPopup);
					return;
			}
		}

		private void CatGridSetup()
		{
			m_CatGrid = new FwGrid();
			m_CatGrid.FullRowSelect = false;
			m_CatGrid.Dock = DockStyle.Left;
			m_CatGrid.BorderStyle = BorderStyle.Fixed3D;
			m_CatGrid.GridLineColor = m_CatGrid.BackColor;
			m_CatGrid.ShowColumnHeadings = false;
			m_CatGrid.Columns.Add(new FwGridColumn("FFCategoryID", 0));
			m_CatGrid.Columns[0].Visible = false;
			m_CatGrid.Columns[0].ColumnFont = SystemInformation.MenuFont;
			m_CatGrid.Columns[0].Name = "FFCategoryID";

			m_CatGrid.Columns.Add(new FwGridColumn("Category"));
			m_CatGrid.Columns[1].Visible = true;
			m_CatGrid.Columns[1].ColumnFont = SystemInformation.MenuFont;
			m_CatGrid.Columns[1].Name = "Category";

			m_CatGrid.AfterRowChange += new FwGrid.RowColChangeHandler(m_CatGrid_AfterRowChange);
			m_CatGrid.Resize += new EventHandler(m_CatGrid_Resize);

			m_CatTable = new PaDataTable("SELECT * FROM FFCategories ORDER BY Category;");

			m_CatGrid.Rows.Clear();

			foreach (DataRow row in m_CatTable.Rows)
			{
				int i = m_CatGrid.Rows.Add(new FwGridRow(new object[] {
																		  row["FFCategoryID"],
																		  row["Category"]
																	  }));
			}

			panel1.Controls.Add(m_CatGrid);
		}

		private void splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			lblPatternList.Left = PointToClient(PointToScreen(splitter1.Location)).X;
		}

		private void m_CatGrid_AfterRowChange(object sender, FwGridRowColChangeArgs args)
		{
			int CatID;

			lblPatternList.Text = args.NewRow.Cells[1].Text;
			m_PrevCatIndex = CatID = (int) args.NewRow.Cells[0].Value;
			lstPatterns.Items.Clear();
			foreach (DataRow row in m_PatternTable.Rows)
			{
				if ((int) row["FFCategoryID"] == CatID)
				{
					FFInfo ff = new FFInfo();
					ff.FFString = (string) row["Pattern"];
					ff.ID = (int) row["FFPatternID"];
					lstPatterns.Items.Add(ff);
				}
			}
		}

		private void m_CatGrid_Resize(object sender, EventArgs e)
		{
			m_CatGrid.Columns[1].Width = m_CatGrid.ClientSize.Width;
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnDefClasses_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			///For now display messagebox to say not done yet
			MessageBox.Show("Not Completed.","Phonology Assistant",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}

	/// <summary>
	/// This class is used to store the Category/Pattern string
	/// as well as the respective ID. Originally used for Category and
	/// Pattern, but due to problems when editing in EditableListBox,
	/// this class is now only used for Pattern.
	/// </summary>
	internal class FFInfo
	{
		internal string FFString;
		internal int ID;

		public override string ToString()
		{
			return FFString;
		}
	}
}
