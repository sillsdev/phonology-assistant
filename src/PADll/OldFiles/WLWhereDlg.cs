using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.FieldWorks.Common.Controls;

namespace SIL.Pa
{
	/// <summary>
	/// Summary description for WLWhereDlg.
	/// </summary>
	public class WLWhereDlg : System.Windows.Forms.Form
	{
		#region Variables added by designer
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboName;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRename;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtNewName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		private System.Windows.Forms.Panel pnlFilterGrid;
		private FwGrid m_filterGrid;

		public WLWhereDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			SetupFilterGrid();
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

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			if (m_filterGrid != null)
			{
				m_filterGrid.Columns["text"].Width = m_filterGrid.Width - m_filterGrid.Columns["lbracket"].Width -
					m_filterGrid.Columns["field"].Width - m_filterGrid.Columns["operator"].Width -
					m_filterGrid.Columns["rbracket"].Width - m_filterGrid.Columns["andor"].Width;
				foreach (FwGridColumn col in m_filterGrid.Columns)
				{
					int i = m_filterGrid.Columns.IndexOf(col);
					if (i > 0)
						col.Left = m_filterGrid.Columns[i-1].Right;
				}
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
			this.cboName = new System.Windows.Forms.ComboBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRename = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtNewName = new System.Windows.Forms.TextBox();
			this.pnlFilterGrid = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Filter &Name:";
			// 
			// cboName
			// 
			this.cboName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboName.Location = new System.Drawing.Point(79, 5);
			this.cboName.Name = "cboName";
			this.cboName.Size = new System.Drawing.Size(121, 21);
			this.cboName.TabIndex = 1;
			// 
			// btnAdd
			// 
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnAdd.Location = new System.Drawing.Point(208, 6);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "&Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRename
			// 
			this.btnRename.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnRename.Location = new System.Drawing.Point(289, 6);
			this.btnRename.Name = "btnRename";
			this.btnRename.TabIndex = 2;
			this.btnRename.Text = "&Rename";
			this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnDelete.Location = new System.Drawing.Point(370, 6);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(392, 232);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(312, 232);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "&OK";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 224);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(160, 32);
			this.label2.TabIndex = 3;
			this.label2.Text = "Enter a name and press Enter to save or ESC to cancel.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label2.Visible = false;
			// 
			// txtNewName
			// 
			this.txtNewName.Location = new System.Drawing.Point(168, 232);
			this.txtNewName.Name = "txtNewName";
			this.txtNewName.Size = new System.Drawing.Size(16, 20);
			this.txtNewName.TabIndex = 4;
			this.txtNewName.Text = "";
			this.txtNewName.Visible = false;
			this.txtNewName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewName_KeyDown);
			this.txtNewName.Leave += new System.EventHandler(this.txtNewName_Leave);
			// 
			// pnlFilterGrid
			// 
			this.pnlFilterGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlFilterGrid.Location = new System.Drawing.Point(0, 40);
			this.pnlFilterGrid.Name = "pnlFilterGrid";
			this.pnlFilterGrid.Size = new System.Drawing.Size(496, 184);
			this.pnlFilterGrid.TabIndex = 5;
			// 
			// WLWhereDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 261);
			this.Controls.Add(this.pnlFilterGrid);
			this.Controls.Add(this.txtNewName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.cboName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnRename);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "WLWhereDlg";
			this.ShowInTaskbar = false;
			this.Text = "Word List Additional Filters";
			this.ResumeLayout(false);

		}
		#endregion

		private void SetupFilterGrid()
		{
			int fontHeight = 0;

			m_filterGrid = new FwGrid();

			m_filterGrid.Dock = DockStyle.Fill;
			m_filterGrid.ColumnHeaderHeight = SystemInformation.MenuFont.Height + 8;
			m_filterGrid.BorderStyle = BorderStyle.Fixed3D;
			m_filterGrid.GridLineColor = Color.FromArgb(m_filterGrid.BackColor.R - 15,
				m_filterGrid.BackColor.G - 15, m_filterGrid.BackColor.B - 15);
			
			int index = m_filterGrid.Columns.Add(new FwGridColumn("(", 30));
			m_filterGrid.Columns[index].Font = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].AllowEdit = true;
			m_filterGrid.Columns[index].DataType = SIL.FieldWorks.Common.Controls.FwGridColumn.DataTypes.CheckBox;
			m_filterGrid.Columns[index].Name = "lbracket";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_filterGrid.Columns.Add(new FwGridColumn("Field", 100));
			m_filterGrid.Columns[index].Font = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].AllowEdit = false;
			m_filterGrid.Columns[index].TextFormat.Alignment = StringAlignment.Center;
			m_filterGrid.Columns[index].Name = "field";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;
			m_filterGrid.Columns[index].DropDownWindow = new FwGridColumnList();
			((FwGridColumnList)m_filterGrid.Columns[index].DropDownWindow).Items.
				AddRange(new string[] {"Phonetic", "Tone", "Phonemic", "Orthographic",
										  "Gloss", "Part of Speech", "Notebook Ref.",
										  "Reference", "Document Name", "Dialect",
										  "Audio File", "CV Pattern", "Char. Duration"});
			m_filterGrid.Columns[index].DropDownWindow.AfterDropDownClosed +=
				new DropDownContainer.AfterDropDownClosedHandler(AfterDropDownClosed);

			index = m_filterGrid.Columns.Add(new FwGridColumn("Operator", 80));
			m_filterGrid.Columns[index].Font = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].AllowEdit = false;
			m_filterGrid.Columns[index].TextFormat.Alignment = StringAlignment.Center;
			m_filterGrid.Columns[index].Name = "operator";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;
			m_filterGrid.Columns[index].DropDownWindow = new FwGridColumnList();
			((FwGridColumnList)m_filterGrid.Columns[index].DropDownWindow).Items.
				AddRange(new string[] {"=", "<>", ">",
										  "<", ">=", "<="});
			m_filterGrid.Columns[index].DropDownWindow.AfterDropDownClosed +=
				new DropDownContainer.AfterDropDownClosedHandler(AfterDropDownClosed);

			index = m_filterGrid.Columns.Add(new FwGridColumn("Text to Compare to Field", 200));
			m_filterGrid.Columns[index].Font = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].ColumnFont = PaApp.PhoneticFont;
			m_filterGrid.Columns[index].AllowEdit = true;
			m_filterGrid.Columns[index].TextFormat.Alignment = StringAlignment.Center;
			m_filterGrid.Columns[index].Name = "text";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;

			index = m_filterGrid.Columns.Add(new FwGridColumn(")", 30));
			m_filterGrid.Columns[index].Font = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].AllowEdit = true;
			m_filterGrid.Columns[index].DataType = SIL.FieldWorks.Common.Controls.FwGridColumn.DataTypes.CheckBox;
			m_filterGrid.Columns[index].Name = "rbracket";
			if (fontHeight < SystemInformation.MenuFont.Height)
				fontHeight = SystemInformation.MenuFont.Height;

			index = m_filterGrid.Columns.Add(new FwGridColumn("AND/OR", 50));
			m_filterGrid.Columns[index].Font = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].ColumnFont = SystemInformation.MenuFont;
			m_filterGrid.Columns[index].AllowEdit = false;
			m_filterGrid.Columns[index].Name = "andor";
			if (fontHeight < PaApp.PhoneticFont.Height)
				fontHeight = PaApp.PhoneticFont.Height;
			m_filterGrid.Columns[index].DropDownWindow = new FwGridColumnList();
			((FwGridColumnList)m_filterGrid.Columns[index].DropDownWindow).Items.AddRange(new string[]{"", "AND", "OR"});
			m_filterGrid.Columns[index].DropDownWindow.AfterDropDownClosed +=
				new SIL.FieldWorks.Common.Controls.DropDownContainer.AfterDropDownClosedHandler(AfterDropDownClosed);

			m_filterGrid.RowHeight = fontHeight + 3;
			pnlFilterGrid.Controls.Add(m_filterGrid);
			m_filterGrid.Columns["text"].Width = m_filterGrid.Width - m_filterGrid.Columns["lbracket"].Width -
				m_filterGrid.Columns["field"].Width - m_filterGrid.Columns["operator"].Width -
				m_filterGrid.Columns["rbracket"].Width - m_filterGrid.Columns["andor"].Width;

			m_filterGrid.Rows.Add(new FwGridRow(new string[] {"(","Phonetic","=","f","","AND"}));
			m_filterGrid.Rows.Add(new FwGridRow(new string[] {"","Phonemic","<>","z",")","OR"}));
		}

		private void AfterDropDownClosed(DropDownContainer dropDownContainer, object eventData)
		{
			try
			{
				((FwGridDropDownEventArgs)eventData).Cell.Text = ((FwGridColumnList)dropDownContainer).SelectedItem;
			}
			catch {}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			cboName.Visible = false;
			txtNewName.Width = cboName.Width;
			txtNewName.Visible = true;
			txtNewName.Location = cboName.Location;
			btnAdd.Visible = btnRename.Visible = btnDelete.Visible = false;
			label2.Location = new Point(btnAdd.Left, txtNewName.Top);
			label2.Visible = true;
			txtNewName.Select();
		}

		private void btnRename_Click(object sender, System.EventArgs e)
		{
			///Not completed for now.
			MessageBox.Show("This is not completed yet","Phonology Assistant",
				MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			///Not completed for now.
			MessageBox.Show("This is not completed yet","Phonology Assistant",
				MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void txtNewName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Modifiers == Keys.None)
				switch (e.KeyCode)
				{
					case Keys.Enter:
						MessageBox.Show("Should save the name", "WLWhereDlg");
						txtNewName.Text = "";
						txtNewName.Visible = label2.Visible = false;
						cboName.Visible = btnAdd.Visible = btnRename.Visible = btnDelete.Visible = true;
						return;
					case Keys.Escape:
						txtNewName.Text = "";
						txtNewName.Visible = label2.Visible = false;
						cboName.Visible = btnAdd.Visible = btnRename.Visible = btnDelete.Visible = true;
						return;
				}
		}

		private void txtNewName_Leave(object sender, System.EventArgs e)
		{
			if (txtNewName.Visible)
				txtNewName.Select();
		}
	}
}
